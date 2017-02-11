#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Users_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            Functions.FillCombo("SELECT 1 as Enable, 'Enabled' as Description UNION SELECT 0 as Enable, 'Disabled' as Description", ddlEnable, "Description", "Enable");

            if (Request.QueryString["ID"] != "")
            {
                FillDetailsEdit();
                btnSave.Visible = true;
                tbUserName.ReadOnly = true;
                tbUserName.Enabled = false;
            }
            else
            {
                btnInsert.Visible = true;
                pnlPassword.Visible = true;
                cbChangePassword.Visible = false;
                tbChangePassword.Visible = false;          
            }
        }
    }
    #endregion

    #region Functions
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

        if (PermLevel == ConfigurationManager.AppSettings["Admin"].ToString())
        {
            btnReset.Visible = true;
        }
    }
    public void FillDetailsEdit()
    {
        String CurrentEmployee = Functions.ExecuteScalar(@"SELECT u1.Username + ';'+ u1.FirstName +';' + u1.LastName+';'+ u1.ContactPhone+';'+ 
                                                        convert(varchar(25), u1.Enabled) + ';' + convert(varchar(25), u1.CreatedDate, 104)+';'+ 
                                                        u2.UserName FROM [User] as u1 LEFT OUTER JOIN [User] as u2 ON u2.UserID = u1.CreatedBy
                                                        WHERE u1.UserID =" + Request.QueryString["ID"]);

//        String CurrentEmployee = Functions.ExecuteScalar(@"SELECT u1.Username + ';'+ u1.FirstName +';' + u1.LastName+';'+ u1.ContactPhone+';'+ 
//                                                        convert(varchar(25), u1.Enabled) + ';' + convert(varchar(25), u1.CreatedDate, 104) 
//                                                        FROM [User] as u1 
//                                                        WHERE u1.UserID =" + Request.QueryString["ID"]);
        string[] curempl = CurrentEmployee.Split(';');
        tbUserName.Text = curempl[0];
        tbFirstName.Text = curempl[1];
        tbLastName.Text = curempl[2];
        tbContactPhone.Text = curempl[3];
        ddlEnable.SelectedValue = curempl[4];
        tbCreatedDate.Text = curempl[5];
        tbCreatedBy.Text = curempl[6];
    }
    #endregion

    #region Handled Events
    protected void btnSave_Click(object sender, EventArgs e)
    {
        String ChangePass = "";
        if (cbChangePassword.Checked)
        {
            ChangePass = "Password='" + Functions.Encrypt(tbChangePassword.Text) + "',";
        }
        String SQL = @"UPDATE [User] SET " + ChangePass + " Username=N'" + tbUserName.Text.Replace("'", "'") + "', FirstName=N'" + tbFirstName.Text.Replace("'", "''") + "',LastName=N'" + tbLastName.Text.Replace("'", "''") +
                       "',ContactPhone=N'" + tbContactPhone.Text.Replace("'", "''") + "', Enabled='" + ddlEnable.SelectedValue.Replace("'", "''") +
                       "' WHERE UserID=" + Request.QueryString["ID"];
        Functions.ExecuteCommand(SQL);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        String AllreadyExist = Functions.ExecuteScalar("SELECT COUNT(*) FROM [User] WHERE UserName=N'" + tbUserName.Text.Replace("'", "''") + "'");
        if (Convert.ToInt32(AllreadyExist) < 1)
        {

            String password = Functions.Encrypt(tbPassword.Text.Replace("'", "''"));
            String SQL = @"INSERT INTO [User] (Username,Password,FirstName,LastName,ContactPhone,Enabled,CreatedBy)
                    VALUES('" + tbUserName.Text.Replace("'", "''") + "','" + password + "',N'" + tbFirstName.Text.Replace("'", "''") +
                        "',N'" + tbLastName.Text.Replace("'", "''") + "',N'" + tbContactPhone.Text.Replace("'", "''") + "'," +
                        ddlEnable.SelectedValue.Replace("'", "''") + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
            Functions.ExecuteCommand(SQL);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
        }
        else
            lblAlreadyExist.Visible = true;
    }
    #endregion
    protected void btnReset_Click(object sender, EventArgs e)
    {
        string lastname = tbUserName.Text;
        string[] array = lastname.Split('.');
        string pass = array[1];
        pass = Functions.Encrypt(pass);

        String SQL = @"UPDATE [User] SET Password=N'"+pass+"' WHERE UserID=" + Request.QueryString["ID"];
        Functions.ExecuteCommand(SQL);
        lblAlreadyExist.Text = "Password was reset to the user's last name!";
        lblAlreadyExist.Visible = true;
    }
}