#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
#endregion

public partial class Employees : System.Web.UI.Page
{
    #region Variables
    private static bool Edited = false;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            FillDataGrid("");
            Functions.FillCombo("SELECT -1 as Status, ' ' as Description UNION SELECT 1 as Status, 'Active' as Description UNION SELECT 0 as Status, 'Left' as Description", ddlStatus, "Description", "Status");
            String PermLevel = Functions.Decrypt(Request.Cookies["PermLevel"].Value);

            if (PermLevel != ConfigurationManager.AppSettings["Admin"].ToString() &&
                PermLevel != ConfigurationManager.AppSettings["Advanced"].ToString())
                gvEmployees.Columns[0].Visible = false;
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        String PermLevel = "";
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
        else
            PermLevel = Functions.Decrypt(Request.Cookies["PermLevel"].Value);

        if (PermLevel != ConfigurationManager.AppSettings["Admin"].ToString() &&
            PermLevel != ConfigurationManager.AppSettings["Advanced"].ToString())
        {
            Response.Redirect("Default.aspx");
        }
    }
    public void FillDataGrid(String WherePart)
    {
        dsEmployees.SelectCommand = @"SELECT e.EmployeeID,e.FirstName,e.LastName,e.ContactPhone,e.StartDate,e.EndDate,u.UserName as UserID,
                                    e.CreatedDate, u2.FirstName + ' ' + u2.LastName as CreatedBy, e.Email,
                                    CASE WHEN e.Status = 0 THEN 'Left'
                                    WHEN e.Status = 1 THEN 'Active' END as Status
                                    FROM Employee e LEFT OUTER JOIN[User] u ON e.UserID = u.UserId
                                    LEFT OUTER JOIN[User] u2 on e.CreatedBy = u2.UserID " + WherePart;
    }
    private string FillWherePart()
    {
        String WherePart = "";

        WherePart = Functions.VratiWherePart(tbFirstName, "e.FirstName", WherePart);
        WherePart = Functions.VratiWherePart(tbLastName, "e.LastName", WherePart);
        WherePart = Functions.VratiWherePartDDL(ddlStatus, "e.Status", WherePart);

        if (WherePart.Length > 0) WherePart = " WHERE " + WherePart;
        return WherePart;
    }
    #endregion

    #region Handled Events
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        String WherePart = FillWherePart();
        FillDataGrid(WherePart);
        gvEmployees.DataBind();
        if (gvEmployees.Rows.Count < 1)
            lblNoRows.Visible = true;
        else
            lblNoRows.Visible = false;

        gvEmployees.SelectedIndex = -1;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (!Edited)
        {
            try
            {
                string Id = gvEmployees.SelectedValue.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Edit Employee');", true);
                Edited = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
                if (HttpContext.Current != null)
                    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            }
        }
        btnSearch_Click(sender, e);
    }
    protected void gvEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gvEmployees.SelectedValue.ToString();
        Edited = false;
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage('', 'Insert Employee');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }

        btnSearch_Click(sender, e);

    }
    protected void gvEmployees_Sorted(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvEmployees.SelectedRow != null)
        {
            String GetUserID = Functions.ExecuteScalar("SELECT UserID FROM Employee WHERE EmployeeID=" + gvEmployees.SelectedValue.ToString());
            String SQL = @"DELETE FROM [UserAccess] WHERE UserID=" + GetUserID;
            Functions.ExecuteCommand(SQL);
            SQL = @"DELETE FROM [User] WHERE UserID=" + GetUserID;
            Functions.ExecuteCommand(SQL);

            SQL = @"DELETE FROM Employee WHERE EmployeeID=" + gvEmployees.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);

            btnSearch_Click(sender, e);
        }
    }
    protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvEmployees, "Select$" + e.Row.RowIndex);

            //LinkButton _doubleClickButton = (LinkButton)lnkDoubleClick;
            //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            //e.Row.Attributes["ondblclick"] = _jsDouble;
        }
    }
    protected void gvEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //btnSearch_Click(sender, e);
    }
    protected void gvEmployees_PageIndexChanged(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    #endregion    
}