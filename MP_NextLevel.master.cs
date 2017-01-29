#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class MP_NextLevel : System.Web.UI.MasterPage
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Functions.Decrypt(Request.Cookies["UserID"].Value) == "")
        //{
        //    Response.Redirect("Default.aspx");
        //}
        lblUser.Text = "";
        if (Session["Loaded"] == null) Session["Loaded"] = false;
        
        
        //if (Functions.Decrypt(Request.Cookies["UserID"].Value) != "")
        //if (Session["najaven"] != null && Convert.ToBoolean(Session["Loaded"]) != false)
        if (Request.Cookies["UserID"] != null)
        if (Functions.Decrypt(Request.Cookies["UserID"].Value) != "" && Convert.ToBoolean(Session["Loaded"]) != false)
        {
            String SQL = "SELECT COUNT(*) FROM UserAccess WHERE UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value);// Session["UserID"];
            int rez = Convert.ToInt32(Functions.ExecuteScalar(SQL));
            //if (!Convert.ToBoolean(Session["najaven"]))
            if (Functions.Decrypt(Request.Cookies["UserID"].Value) == "")
                pnlMeni.Visible = false;
            else if (rez < 1)
            {
                pnlMeni.Visible = false;
                btnLogout.Visible = true;
                lblUser.Visible = true;
                pnlUser.Visible = true;
                lblUser.Text = Functions.Decrypt(Request.Cookies["UserLoged"].Value);// Session["UserLoged"].ToString();
            }
            else
            {
                btnLogout.Visible = true;
                pnlMeni.Visible = true;
                pnlMiStudents.Visible = true;
                pnlMiGroups.Visible = true;
                pnlMiPayments.Visible = true;
                pnlMiServices.Visible = true;
                pnlMiChangePassword.Visible = true;
                lblUser.Text = Functions.Decrypt(Request.Cookies["UserLoged"].Value);// Session["UserLoged"].ToString();

                if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == "") Response.Redirect("Default.aspx");
                //if (Session["PermLevel"] == null) Response.Redirect("Default.aspx");

                //if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Edit"].ToString() ||
                //    Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Readonly"].ToString())
                if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
                        Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
                
                {
                    pnlMiTeachers.Visible = false;
                    pnlMiAdministrate.Visible = false;
                }
                //if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString() ||
                //    Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Advanced"].ToString())
                if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Admin"].ToString() ||
                        Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Advanced"].ToString())
                
                {
                    pnlMiTeachers.Visible = true;
                }
                //if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString())
                if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Admin"].ToString())
                {
                    pnlMiAdministrate.Visible = true;
                }
                Session["Loaded"] = true;
            }
        }
        else
        {
            pnlMeni.Visible = false;
        }        
    }
    #endregion

    #region Functions
    #endregion

    #region Handled Eventes
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        System.Web.Security.FormsAuthentication.SetAuthCookie("", true);
        Response.Cookies.Remove("PermLevel");
        Response.Cookies.Remove("UserID");
        Response.Cookies["UserID"].Value = "";// Expires.AddMilliseconds(1);
        Response.Cookies["PermLevel"].Expires.AddMilliseconds(1);
        Response.Cookies["UserID"].Expires.AddMilliseconds(1);

        Session["Loaded"] = false;
        //Session["najaven"] = false;
        
        //Session["Loaded"] = false;
        //Session["najaven"] = false;
        //Session["PermLevel"] = null;
        btnLogout.Visible = false;
        
        Response.Redirect("Default.aspx");
    }
    #endregion
}
