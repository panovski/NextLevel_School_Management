#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class ChangePassword : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            tbUserName.Text = Functions.Decrypt(Request.Cookies["UserLoged"].Value);// Session["UserLoged"].ToString();
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");       
    }
    #endregion

    #region Handled Events
    protected void btnChange_Click(object sender, EventArgs e)
    {
        String SQL = "SELECT Password FROM [User] WHERE UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value);// Session["UserID"]; 
        String OldPassword = Functions.ExecuteScalar(SQL);
        OldPassword = Functions.Decrypt(OldPassword);

        if (OldPassword == tbOldPassword.Text)
        {
            if (tbNewPassword.Text == tbConfirmPassword.Text)
            {
                String PassEnc = Functions.Encrypt(tbNewPassword.Text);
                SQL = "UPDATE [User] SET Password='"+PassEnc.Replace("'","''")+"'";
                Functions.ExecuteCommand(SQL);
                lblInfoMessage.Text = "The password is changed!";
                lblInfoMessage.Visible = true;
            }
            else
            {
                lblInfoMessage.Text = "The new password and confirm password are not same!";
                lblInfoMessage.Visible = true;
            }
        }
        else
        {
            lblInfoMessage.Text = "The old password is not correct!";
            lblInfoMessage.Visible = true;
        }
    }
    #endregion
}