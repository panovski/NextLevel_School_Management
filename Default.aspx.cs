#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
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

        if(Request.Cookies["UserID"]!=null)
        if (Functions.Decrypt(Request.Cookies["UserID"].Value) != "")
        {
            //System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            Login_Redirect();  
        }

        CheckLoginNumber();
        //if (Session["najaven"] != null)
        //{
        //    if (Convert.ToBoolean(Session["najaven"])!= false)
        //        Login_Redirect();           
        //}
    }
    #endregion

    #region Functions
    protected void CheckLoginNumber()
    {
        if (Session["LoginTries"] != null)
        {
            if (Convert.ToInt32(Session["LoginTries"]) >= 5)
            {
                btnLogin.Enabled = false;
                btnLogin.Visible = false;
                lblInfoNajava.Text = "Maximum 5 tries! Please wait for 30 minutes!";
                lblInfoNajava.Visible = true;
            }
        }
    }
    public void Login_Redirect()
    {
        HttpCookie Loaded = new HttpCookie("Loaded");
        Loaded.Value = "true";
        Loaded.Expires = DateTime.Now.AddHours(8);
        Response.SetCookie(Loaded);

        Session["Loaded"] = true;
            //if (Convert.ToBoolean(Session["najaven"]) && Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString())
        if (Request.Cookies["UserID"]!=null && Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Admin"].ToString())
                Response.Redirect("AdministrationPage.aspx");
            else
                Response.Redirect("Groups.aspx");
    }
    #endregion

    #region Handled Events
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Session["LoginTries"] != null)
            Session["LoginTries"] = Convert.ToInt32(Session["LoginTries"]) + 1;
        else
            Session["LoginTries"] = 1;

        if (Convert.ToInt32(Session["LoginTries"]) <= 5)
        {

            //Session["PermLevel"] = 0;
            //Session["najaven"] = false;

            //Request.Cookies["PermLevel"].Value = "0";
            lblInfoNajava.Text = "Wrong username or password!";
            lblInfoNajava.Visible = true;

            String SQL = "SELECT COUNT(*) as Broj FROM [User] WHERE UserName=N'" + tbUserName.Text + "' AND Password='" + Functions.Encrypt(tbPassword.Text) + "' AND Enabled=1";
            String Br = Functions.ExecuteScalar(SQL);
            if (Convert.ToInt32(Br) > 0)
            {
                //Session["najaven"] = true;

                SQL = "SELECT UserID FROM [User] WHERE UserName=N'" + tbUserName.Text + "' AND Password='" + Functions.Encrypt(tbPassword.Text) + "'";
                //Session["UserID"] = Functions.ExecuteScalar(SQL);

                String UserId = Functions.ExecuteScalar(SQL);
                HttpCookie UserID = new HttpCookie("UserID");
                UserID.Value = Functions.Encrypt(UserId);
                UserID.Expires = DateTime.Now.AddHours(8);
                Response.SetCookie(UserID);

                //FormsAuthentication.SetAuthCookie(Functions.Encrypt(Session["UserID"].ToString()), true);
                //string name = HttpContext.Current.User.Identity.Name;

                SQL = "SELECT Count(UserTypeID) as Broj FROM UserAccess WHERE UserID=" + UserId;
                String Broj = Functions.ExecuteScalar(SQL);
                SQL = "SELECT UserTypeID FROM UserAccess WHERE UserID=" + UserId + " ORDER BY UserTypeID DESC";
                Session["Permissions"] = Functions.ReadIntoArray(SQL, "UserTypeID");

                //HttpCookie Permissions = new HttpCookie("Permissions");
                //Permissions.Value = Functions.Encrypt(Session["Permissions"].ToString());
                //Permissions.Expires = DateTime.Now.AddHours(1);
                //Response.SetCookie(Permissions);

                SQL = "SELECT Username as Name FROM [User] WHERE UserID=" + UserId;// Session["UserID"];
                //Session["UserLoged"]=Functions.ExecuteScalar(SQL);

                HttpCookie UserLoged = new HttpCookie("UserLoged");
                UserLoged.Value = Functions.Encrypt(Functions.ExecuteScalar(SQL));
                UserLoged.Expires = DateTime.Now.AddHours(8);
                Response.SetCookie(UserLoged);

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

                        HttpCookie PermLevel = new HttpCookie("PermLevel");
                        PermLevel.Value = Functions.Encrypt(Session["PermLevel"].ToString());
                        PermLevel.Expires = DateTime.Now.AddHours(8);
                        Response.SetCookie(PermLevel);
                    }
                }
                Session["LoginTries"]=0;
                //Request.Cookies["PermLevel"].Value = Session["PermLevel"].ToString();
                Login_Redirect();
            }
        }
        else
            CheckLoginNumber();
    }
    #endregion
}