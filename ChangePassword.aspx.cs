#region Using
using System;
using System.Collections.Generic;
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
            tbUserName.Text = Session["UserLoged"].ToString();
        }
    }
    #endregion

    #region Functions
    #endregion

    #region Handled Events
    protected void btnChange_Click(object sender, EventArgs e)
    {
        String SQL = "SELECT Password FROM [User] WHERE UserID="+ Session["UserID"]; 
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