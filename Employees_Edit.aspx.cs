#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Employees_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            Functions.FillCombo("SELECT 1 as Status, 'Active' as Description UNION SELECT 0 as Status, 'Left' as Description", ddlStatus, "Description", "Status");
            Functions.FillCombo("SELECT NULL as UserID, ' ' as UserName UNION SELECT UserID, UserName FROM [User]", ddlUserName, "UserName", "UserID");

            if (Request.QueryString["ID"] != "")
            {
                FillDetailsEdit();
                btnSave.Visible = true;
            }
            else
            {
                btnInsert.Visible = true;
                //calStartDate.SelectedDate = DateTime.Today.AddHours(Convert.ToDouble(ConfigurationManager.AppSettings["HourZone"]));
                cbUserName.Visible = true;
            }
        }
    }
    #endregion

    #region Functions
    public void FillDetailsEdit()
    {
        String CurrentEmployee = Functions.ExecuteScalar(@"SELECT FirstName +';' + LastName+';'+ ContactPhone+';'+ convert(varchar(25), startdate) +';'+
                                                            convert(varchar(25), Status) + ';' + convert(varchar(25), CASE WHEN UserId IS NULL THEN '-1' ELSE UserId END) +';' + Email FROM[Employee]
                                                            WHERE EmployeeID =" + Request.QueryString["ID"]);
        string[] curempl = CurrentEmployee.Split(';');

        tbFirstName.Text = curempl[0];
        tbLastName.Text = curempl[1];
        tbContactPhone.Text = curempl[2];
        tbStartDate.Text = Convert.ToDateTime(curempl[3]).ToString("yyyy-MM-dd");
        //DateTime selectedDate = Convert.ToDateTime(curempl[3]);//.AddHours(Convert.ToDouble(ConfigurationManager.AppSettings["HourZone"]));
        //calStartDate.TodaysDate = selectedDate;
        //calStartDate.SelectedDate = calStartDate.TodaysDate;
        ddlStatus.SelectedValue = curempl[4];
        ddlUserName.SelectedValue = curempl[5];
        tbEmail.Text = curempl[6];

        if (curempl[5] == "-1")
            cbUserName.Visible = true;

    }
    public void Login_Redirect()
    {
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
        String PermLevel = Functions.Decrypt(Request.Cookies["PermLevel"].Value);
        if (PermLevel != ConfigurationManager.AppSettings["Admin"].ToString() &&
          PermLevel != ConfigurationManager.AppSettings["Advanced"].ToString())
        {
            Response.Redirect("Default.aspx");
        }
    }
    #endregion

    #region Handled Events
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //Sredi go UserID za da raboti so NULL
        String UserID = "NULL";
        if (ddlUserName.SelectedValue.Length > 0)
            UserID = ddlUserName.SelectedValue;

        //Proveri ako ima promena na status vo Left, togas da smeni i EndDate
        String EndDate = "";
        String SQL2 = "";
        if (Convert.ToInt32(ddlStatus.SelectedValue) == 0)
        {
            EndDate = ", EndDate='" + DateTime.Now.AddHours(Convert.ToDouble(ConfigurationManager.AppSettings["HourZone"])) + "' ";
            if (UserID != "NULL") SQL2 = "UPDATE [User] SET Enabled=0 WHERE UserID=" + UserID;
        }

        String UserLoginID = "";
        if (cbUserName.Checked)
        {
            String UserName = tbFirstName.Text.Replace("'", "''").ToLower().Trim() + "." + tbLastName.Text.Replace("'", "''").ToLower().Trim();
            String AllreadyExist = Functions.ExecuteScalar("SELECT COUNT(*) FROM [User] WHERE UserName=N'" + UserName + "'");
            if (Convert.ToInt32(AllreadyExist) < 1)
            {
                String Password = Functions.Encrypt(tbLastName.Text.Replace("'", "''").ToLower().Trim());
                String SQLInsert = @"INSERT INTO [User] (UserName,Password,FirstName,LastName,ContactPhone,Enabled,CreatedBy) " +
                     "VALUES(N'" + UserName + "',N'" + Password + "',N'" + tbFirstName.Text.Replace("'", "''") + "',N'" + tbLastName.Text.Replace("'", "''") + "',N'" + tbContactPhone.Text.Replace("'", "''")
                     + "',1," + Functions.Decrypt(Request.Cookies["UserID"].Value) + "); SELECT SCOPE_IDENTITY()";

                UserLoginID = Functions.ExecuteScalar(SQLInsert);
            }
            else { lblAlreadyExist.Visible = true; }
        }

        if (UserLoginID != "")
            UserID = UserLoginID;

        String SQLCheck = "SELECT COUNT(*) FROM Employee WHERE UserID=" + UserID;
        Int32 Exists = Convert.ToInt32(Functions.ExecuteScalar(SQLCheck));

        SQLCheck = "SELECT UserID FROM Employee WHERE EmployeeID=" + Request.QueryString["ID"];
        String OldUserID = Functions.ExecuteScalar(SQLCheck);

        if (Exists < 1 || OldUserID == UserID)
        {
            String SQL = @"UPDATE Employee SET FirstName=N'" + tbFirstName.Text.Replace("'", "''") + "',LastName=N'" + tbLastName.Text.Replace("'", "''") +
                        "',ContactPhone=N'" + tbContactPhone.Text.Replace("'", "''") + "', StartDate='" + tbStartDate.Text.Replace("'", "''") + "', Status='" + ddlStatus.SelectedValue.Replace("'", "''") +
                        "', UserId=" + UserID.Replace("'", "''") + EndDate + ",Email='" + tbEmail.Text + "' WHERE EmployeeID=" + Request.QueryString["ID"];
            Functions.ExecuteCommand(SQL);

            if (UserID != "NULL")
            {
                SQL = @"UPDATE [User] SET FirstName=N'" + tbFirstName.Text.Replace("'", "''") + "', LastName=N'" + tbLastName.Text.Replace("'", "''") +
                       "', ContactPhone=N'" + tbContactPhone.Text.Replace("'", "''") + "' WHERE UserID=" + UserID;
                Functions.ExecuteCommand(SQL);
            }
            //Da napravi update i na user dokolku premine vo Inactive
            if (SQL2 != "")
                Functions.ExecuteCommand(SQL2);

            lblInfo.Visible = false;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
        }
        else
        {
            lblInfo.Visible = true;
        }
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        String UserID = "NULL";
        if (ddlUserName.SelectedValue.Length > 0)
            UserID = ddlUserName.SelectedValue;

        String UserLoginID = "";
        if (cbUserName.Checked)
        {
            String UserName = tbFirstName.Text.Replace("'", "''").ToLower().Trim() + "." + tbLastName.Text.Replace("'", "''").ToLower().Trim();
            String AllreadyExist = Functions.ExecuteScalar("SELECT COUNT(*) FROM [User] WHERE UserName=N'" + UserName + "'");
            if (Convert.ToInt32(AllreadyExist) < 1)
            {
                String Password = Functions.Encrypt(tbLastName.Text.Replace("'", "''").ToLower().Trim());
                String SQLInsert = @"INSERT INTO [User] (UserName,Password,FirstName,LastName,ContactPhone,Enabled,CreatedBy) " +
                     "VALUES(N'" + UserName + "',N'" + Password + "',N'" + tbFirstName.Text.Replace("'", "''") + "',N'" + tbLastName.Text.Replace("'", "''") + "',N'" + tbContactPhone.Text.Replace("'", "''")
                     + "',1," + Functions.Decrypt(Request.Cookies["UserID"].Value) + "); SELECT SCOPE_IDENTITY()";

                UserLoginID = Functions.ExecuteScalar(SQLInsert);
            }
            else { lblAlreadyExist.Visible = true; }
        }

        if (UserLoginID != "")
            UserID = UserLoginID;

        String SQL = @"INSERT INTO Employee  (FirstName,LastName,ContactPhone,StartDate,Status,UserId,CreatedBy,Email)
                    VALUES(N'" + tbFirstName.Text.Replace("'", "''") + "',N'" + tbLastName.Text.Replace("'", "''") +
                    "',N'" + tbContactPhone.Text.Replace("'", "''") + "','" + Convert.ToDateTime(tbStartDate.Text.Replace("'", "''")) + "','" +
                    ddlStatus.SelectedValue.Replace("'", "''") + "'," + UserID.Replace("'", "''") + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ",'" + tbEmail.Text + "')";
        Functions.ExecuteCommand(SQL);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
    }
    #endregion
    protected void tbStartDate_TextChanged(object sender, EventArgs e)
    {

    }
}