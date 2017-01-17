#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class _Default : System.Web.UI.Page
{
    #region Variables
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["najaven"] != null)
        {
            if (Convert.ToBoolean(Session["najaven"])!= false)
                Login_Redirect();           
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        Session["Loaded"] = true;
            if (Convert.ToBoolean(Session["najaven"]) && Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString())
                Response.Redirect("AdministrationPage.aspx");
            else
                Response.Redirect("Groups.aspx");
    }
    #endregion

    #region Handled Events
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Session["PermLevel"] = 0;
        Session["najaven"] = false;
        lblInfoNajava.Text = "Wrong username or password!";
        lblInfoNajava.Visible = true;

        String SQL = "SELECT COUNT(*) as Broj FROM [User] WHERE UserName=N'" + tbUserName.Text + "' AND Password='" + Functions.Encrypt(tbPassword.Text) + "' AND Enabled=1";
        String Br = Functions.ExecuteScalar(SQL);
        if (Convert.ToInt32(Br) > 0)
        {
            Session["najaven"] = true;
            SQL = "SELECT UserID FROM [User] WHERE UserName=N'" + tbUserName.Text + "' AND Password='" + Functions.Encrypt(tbPassword.Text) + "'";
            Session["UserID"] = Functions.ExecuteScalar(SQL);

            SQL = "SELECT Count(UserTypeID) as Broj FROM UserAccess WHERE UserID=" + Session["UserID"];
            String Broj = Functions.ExecuteScalar(SQL);
            SQL = "SELECT UserTypeID FROM UserAccess WHERE UserID=" + Session["UserID"] + " ORDER BY UserTypeID DESC";
            Session["Permissions"] = Functions.ReadIntoArray(SQL, "UserTypeID");

            SQL = "SELECT Username as Name FROM [User] WHERE UserID=" + Session["UserID"];
            Session["UserLoged"]=Functions.ExecuteScalar(SQL);

            int HighPermission = 99;
            foreach (string perm in (List<String>)Session["Permissions"])
            {
                if (Convert.ToInt32(perm) < HighPermission)
                {
                    HighPermission = Convert.ToInt32(perm);
                    if (perm == ConfigurationManager.AppSettings["Admin"].ToString()) Session["PermLevel"] = ConfigurationManager.AppSettings["Admin"].ToString();
                    if (perm == ConfigurationManager.AppSettings["Edit"].ToString()) Session["PermLevel"] = ConfigurationManager.AppSettings["Edit"].ToString();
                    if (perm == ConfigurationManager.AppSettings["Readonly"].ToString()) Session["PermLevel"] = ConfigurationManager.AppSettings["Readonly"].ToString();
                    if (perm == ConfigurationManager.AppSettings["Advanced"].ToString()) Session["PermLevel"] = ConfigurationManager.AppSettings["Advanced"].ToString();
                }
            }
            Login_Redirect();
        }
    }
    #endregion
}