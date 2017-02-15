#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Groups_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            Functions.FillCombo("SELECT GroupTypeID, Language + ' - ' + LevelDescription + ' - ' + Program as Course FROM GroupType", ddlCourse, "Course", "GroupTypeID");
            //Functions.FillCombo("SELECT EmployeeID, FirstName +' ' + LastName as Name FROM Employee WHERE Status = 1", ddlTeacher, "Name", "EmployeeID");
            Functions.FillCombo(@"SELECT e.EmployeeID, e.FirstName+' '+e.LastName as Name 
                                FROM Employee e LEFT OUTER JOIN [User] u ON e.UserID=u.UserID LEFT OUTER JOIN UserAccess ua ON ua.UserID=u.UserID
                                WHERE e.Status=1 AND ua.UserTypeID NOT IN (" + ConfigurationManager.AppSettings["Admin"].ToString() +
                                "," + ConfigurationManager.AppSettings["Advanced"].ToString() + ") GROUP BY e.EmployeeID, e.FirstName, e.LastName", ddlTeacher, "Name", "EmployeeID");

            if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
               Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
            {
                String Employee = Functions.ExecuteScalar("SELECT EmployeeID FROM Employee WHERE UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value));
                ddlTeacher.SelectedValue = Employee;
                ddlTeacher.Enabled = false;
            }

            Functions.FillCombo("SELECT 1 as Enable, 'Active' as Description UNION SELECT 0 as Enable, 'Finished' as Description", ddlStatus, "Description", "Enable");

            if (Request.QueryString["ID"] != "")
            {
                FillDetailsEdit();
                btnSave.Visible = true;
            }
            else
            {
                btnInsert.Visible = true;
                String GroupName = "";
                GroupName = DateTime.Now.ToString("yy") + "/" + DateTime.Now.ToString("MM") + "-G";

                Int32 BrojTekoven = 0;
                BrojTekoven = Convert.ToInt32(Functions.ExecuteScalar("SELECT COUNT(*) FROM [Group] WHERE GroupName LIKE '"+GroupName+"%'"));
                BrojTekoven++;
                GroupName += BrojTekoven.ToString();

                tbGroupName.Text = GroupName;

            }

            if (Request.QueryString["Type"] == "Preview")
            {
                FillDetailsEdit();
                DisableControls(this, false);
                btnSave.Visible = false;
                btnInsert.Visible = false;
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
          PermLevel != ConfigurationManager.AppSettings["Advanced"].ToString() &&
          PermLevel != ConfigurationManager.AppSettings["Edit"].ToString())
        {
            Response.Redirect("Default.aspx");
        }
    }
    public void FillDetailsEdit()
    {
        String[] Group = Functions.ReturnIntoArray("SELECT * FROM [Group] WHERE GroupID=" + Request.QueryString["ID"], 15);
        tbGroupName.Text = Group[1];
        ddlCourse.SelectedValue = Group[2];
        tbStartDate.Text = Convert.ToDateTime(Group[3]).ToString("dd.MM.yyyy");
        if (Group[4]!="")
            tbEndDate.Text = Convert.ToDateTime(Group[4]).ToString("dd.MM.yyyy");
        tbNoClasses.Text = Group[5];
        tbNoPayments.Text = Group[6];
        tbCost.Text = Group[7];
        ddlTeacher.SelectedValue = Group[8];
        tbTeacherPercentage.Text = Group[9];
        ddlStatus.SelectedValue = Group[10];
        Boolean Checked = false;
        if (Convert.ToBoolean(Group[11])) Checked = true;
        cbInvoice.Checked = Checked;
        tbCreatedDate.Text = Group[12];
        tbCreatedBy.Text = Group[13];
        cbIndividual.Checked = Convert.ToBoolean(Group[14]);

        String PermLevel = Functions.Decrypt(Request.Cookies["PermLevel"].Value);
        if (PermLevel == ConfigurationManager.AppSettings["Edit"].ToString())
        {
            DisableControls(this, false);
            ddlStatus.Enabled = true;
            tbEndDate.Enabled = true;
        }
    }
    #endregion

    #region Handled Events
    protected void DisableControls(Control parent, bool State)
    {
        foreach (Control c in parent.Controls)
        {
            if (c is DropDownList)
            {
                ((DropDownList)(c)).Enabled = State;
            }
            else if (c is TextBox)
                ((TextBox)(c)).Enabled = State;
            else if (c is CheckBox)
                ((CheckBox)(c)).Enabled = State;

            DisableControls(c, State);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if(!cbIndividual.Checked)
            Page.Validate("2");

        if (Page.IsValid)
        {

            String Invoice = "0";
            if (cbInvoice.Checked)
                Invoice = "1";

            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd.MM.yyyy";

            String NoClases = "NULL", NoPayments = "NULL", Cost = "NULL";
            String IndividualText = "";

            if (tbNoClasses.Text != "") NoClases = tbNoClasses.Text;
            if (tbNoPayments.Text != "") NoPayments = tbNoPayments.Text;
            if (tbCost.Text != "") Cost = tbCost.Text;
            if (cbIndividual.Checked) { IndividualText = ", IndividualGroup=1";}

            String EndDateText = "";
            if (tbEndDate.Text != "") EndDateText = "', EndDate= '" + Convert.ToDateTime(tbEndDate.Text.Replace("'", "''"), dateInfo);

            String SQL = @"UPDATE [Group] SET GroupName = N'" + tbGroupName.Text.Replace("'", "''") +
          "', GroupTypeID= " + ddlCourse.SelectedValue +
          ", StartDate= '" + Convert.ToDateTime(tbStartDate.Text.Replace("'", "''"), dateInfo) +
          EndDateText +
          "', NumberOfClasses=" + NoClases +
          ", NumberOfPayments=" + NoPayments +
          ", Cost= " + Cost +
          ", EmployeeID= " + ddlTeacher.SelectedValue +
          ", TeacherPercentage= " + tbTeacherPercentage.Text.Replace("'", "''") +
          ", Status=" + ddlStatus.SelectedValue +
          ", Invoice=" + Invoice + IndividualText +
          " WHERE GroupID=" + Request.QueryString["ID"];
            Functions.ExecuteCommand(SQL);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
        }
    }
   
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        if(!cbIndividual.Checked)
            Page.Validate("2");
        
        if (Page.IsValid)
        {

            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd.MM.yyyy";

            String Invoice = "0";
            String NoClases = "NULL", NoPayments = "NULL", Cost = "NULL";
            String IndividualValue = "", IndividualText = "";

            if (tbNoClasses.Text != "") NoClases = tbNoClasses.Text;
            if (tbNoPayments.Text != "") NoPayments = tbNoPayments.Text;
            if (tbCost.Text != "") Cost = tbCost.Text;
            if (cbIndividual.Checked) { IndividualText = ",IndividualGroup"; IndividualValue = ",1"; }

            if (cbInvoice.Checked)
                Invoice = "1";

            String SQL = @"INSERT INTO [Group] (GroupName,GroupTypeID,StartDate,NumberOfClasses,NumberOfPayments,
                    Cost,EmployeeID, TeacherPercentage, Status, Invoice, CreatedBy"+IndividualText+@")
                   VALUES(N'" + tbGroupName.Text.Replace("'", "''") + "'," + ddlCourse.SelectedValue + ",N'" + Convert.ToDateTime(tbStartDate.Text.Replace("'", "''"), dateInfo) +
                      "'," + NoClases + "," + NoPayments +
                      "," + Cost + "," + ddlTeacher.SelectedValue + "," + tbTeacherPercentage.Text.Replace("'", "''") + ",'" + ddlStatus.SelectedValue + "'," + Invoice + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + IndividualValue + ")";

            if (tbEndDate.Text != "")
            {
                SQL = @"INSERT INTO [Group] (GroupName,GroupTypeID,StartDate,EndDate,NumberOfClasses,NumberOfPayments,
                    Cost,EmployeeID, TeacherPercentage, Status, Invoice, CreatedBy"+IndividualText+@")
                   VALUES(N'" + tbGroupName.Text.Replace("'", "''") + "'," + ddlCourse.SelectedValue + ",N'" + Convert.ToDateTime(tbStartDate.Text.Replace("'", "''"), dateInfo) +
                          "',N'" + Convert.ToDateTime(tbEndDate.Text.Replace("'", "''"), dateInfo) + "'," + NoClases + "," + NoPayments +
                          "," + Cost + "," + ddlTeacher.SelectedValue + "," + tbTeacherPercentage.Text.Replace("'", "''") + ",'" + ddlStatus.SelectedValue + "'," + Invoice + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + IndividualValue+ ")";
            }
            Functions.ExecuteCommand(SQL);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvMain_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvMain_Sorted(object sender, EventArgs e)
    {
    }
    #endregion
}