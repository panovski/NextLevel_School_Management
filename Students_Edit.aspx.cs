#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Students_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            Functions.FillCombo("SELECT 1 as Enable, 'Active' as Description UNION SELECT 0 as Enable, 'Deactivated' as Description", ddlStatus, "Description", "Enable");

            if (Request.QueryString["ID"] != "")
            {
                FillDetailsEdit();
                btnSave.Visible = true;
            }
            else
            {
                btnInsert.Visible = true;
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
        String[] Student = Functions.ReturnIntoArray("SELECT * FROM Student WHERE StudentID=" + Request.QueryString["ID"], 20);
        tbFirstName.Text = Student[1];
        tbLastName.Text = Student[2];
        tbSocialNumber.Text = Student[3];
        tbContactPhone.Text = Student[4];
        ddlStatus.SelectedValue = Student[5];
        tbDateOfBirth.Text = Convert.ToDateTime(Student[6]).ToString("yyyy-MM-dd");
        tbAddress.Text = Student[7];
        tbHouseNumber.Text = Student[8];
        tbPlace.Text = Student[9];
        tbEmail.Text = Student[10];
        tbParrentFirstName.Text = Student[11];
        tbParrentLastName.Text = Student[12];
        tbParrentAddress.Text = Student[13];
        tbParrentHouseNumber.Text = Student[14];
        tbParrentSN.Text = Student[15];
        tbParrentPlace.Text = Student[16];
        tbCreatedDate.Text = Student[17];
        tbCreatedBy.Text = Student[18];
        rblGender.SelectedValue = Student[19];

        double denovi = (DateTime.Now - Convert.ToDateTime(Student[6])).TotalDays;
        double godini = denovi / 365;

        if (godini < 18)
            pnlParrents.Visible = true;
    }
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

            DisableControls(c, State);
        }
    }
    #endregion

    #region Handled Events
    protected void btnSave_Click(object sender, EventArgs e)
    {
        String SQL = @"UPDATE Student SET FirstName = N'" + tbFirstName.Text.Replace("'", "''") +
         "', LastName= N'" + tbLastName.Text.Replace("'", "''") +
         "', SocialNumber= N'" + tbSocialNumber.Text.Replace("'","''") +
         "', ContactPhone= N'" + tbContactPhone.Text.Replace("'", "''") +
         "', Status=" + ddlStatus.SelectedValue +
         ", DateOfBirth='" + tbDateOfBirth.Text.Replace("'", "''") +
         "', Address= N'" + tbAddress.Text.Replace("'", "''") +
         "', HouseNumber= N'" + tbHouseNumber.Text.Replace("'", "''") +
         "', Place= N'" + tbPlace.Text.Replace("'", "''") +
         "', Email='" + tbEmail.Text.Replace("'", "''") +
         "', Parent_FirstName= N'" + tbParrentFirstName.Text.Replace("'", "''") +
         "', Parent_LastName= N'" + tbParrentLastName.Text.Replace("'", "''") +
         "', Parent_Address= N'" + tbParrentAddress.Text.Replace("'", "''") +
         "', Parent_HouseNumber= N'" + tbParrentHouseNumber.Text.Replace("'", "''") +
         "', Parent_SocialNumber= N'" + tbParrentSN.Text.Replace("'", "''") +
         "', Parent_Place= N'" + tbParrentPlace.Text + 
         "', Gender='"+ rblGender.SelectedValue +
         "' WHERE StudentID=" + Request.QueryString["ID"];
        Functions.ExecuteCommand(SQL);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        String AllreadyExist = Functions.ExecuteScalar("SELECT COUNT(*) FROM Student WHERE SocialNumber='" + tbSocialNumber.Text.Replace("'", "''") + "'");
        if (Convert.ToInt32(AllreadyExist) < 1)
        {
            String SQL = @"INSERT INTO Student (FirstName,LastName,SocialNumber,ContactPhone,Status,DateOfBirth,Address,
                   HouseNumber,Place,Email,Parent_FirstName,Parent_LastName,Parent_Address,Parent_HouseNumber,Parent_SocialNumber,Parent_Place, CreatedBy, Gender)
                   VALUES(N'" + tbFirstName.Text.Replace("'", "''") + "',N'" + tbLastName.Text.Replace("'", "''") + "',N'" + tbSocialNumber.Text.Replace("'", "''") +
                   "',N'" + tbContactPhone.Text.Replace("'", "''") + "','" + ddlStatus.SelectedValue + "','" + tbDateOfBirth.Text.Replace("'", "''") +
                   "',N'" + tbAddress.Text.Replace("'", "''") + "',N'" + tbHouseNumber.Text.Replace("'", "''") + "',N'" + tbPlace.Text.Replace("'", "''") +
                   "','" + tbEmail.Text.Replace("'", "''") + "',N'" + tbParrentFirstName.Text.Replace("'", "''") + "',N'" + tbParrentLastName.Text.Replace("'", "''") +
                   "',N'" + tbParrentAddress.Text.Replace("'", "''") + "',N'" + tbParrentHouseNumber.Text.Replace("'", "''") + "',N'" + tbParrentSN.Text.Replace("'", "''") +
                   "',N'" + tbParrentPlace.Text.Replace("'", "''") + "'," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ",'" + rblGender.SelectedValue + "')";
           Functions.ExecuteCommand(SQL);
           Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
        }
        else
            lblAlreadyExist.Visible = true;
    }
    protected void tbDateOfBirth_TextChanged(object sender, EventArgs e)
    {
        double denovi = (DateTime.Now - Convert.ToDateTime(tbDateOfBirth.Text)).TotalDays;
        double godini = denovi / 365;

        if (godini < 18)
            pnlParrents.Visible = true;
        else
            pnlParrents.Visible = false;
    }
    protected void tbSocialNumber_TextChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] == "")
        {
            String AllreadyExist = Functions.ExecuteScalar("SELECT COUNT(*) FROM Student WHERE SocialNumber='" + tbSocialNumber.Text.Replace("'", "''") + "'");
            if (Convert.ToInt32(AllreadyExist) > 0)
                lblAlreadyExist.Visible = true;
            else
                lblAlreadyExist.Visible = false;
        }
    }
    #endregion
}