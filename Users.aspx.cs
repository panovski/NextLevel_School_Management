#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Users : System.Web.UI.Page
{
    #region Variables
    private static bool Edited = false;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Functions.FillCombo("SELECT -1 as Status, ' ' as Description UNION SELECT 1 as Status, 'Active' as Description UNION SELECT 0 as Status, 'Deactivated' as Description", ddlStatus, "Description", "Status");
            Functions.FillCombo("SELECT UserTypeID, Type from UserType", ddlUserType, "Type", "UserTypeID");
            Login_Redirect();
            btnSearch_Click(sender, e);
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
        String PermLevel = Functions.Decrypt(Request.Cookies["PermLevel"].Value);
        if (PermLevel != ConfigurationManager.AppSettings["Admin"].ToString())
        {
            Response.Redirect("Default.aspx");
        }
    }
    public void FillDataGrid(String WherePart)
    {
        dsUsers.SelectCommand = @"SELECT *, CASE WHEN Enabled=1 THEN 'Yes' WHEN Enabled=0 THEN 'No' END as Active FROM [User] " + WherePart;
    }
    private string FillWherePart()
    {
        String WherePart = "";

        WherePart = Functions.VratiWherePart(tbUserName, "Username", WherePart);
        WherePart = Functions.VratiWherePart(tbFirstName, "FirstName", WherePart);
        WherePart = Functions.VratiWherePart(tbLastName, "LastName", WherePart);
        WherePart = Functions.VratiWherePartDDL(ddlStatus, "Enabled", WherePart);

        if (WherePart.Length > 0) WherePart = " WHERE " + WherePart;
        return WherePart;
    }
    private void Fill_Details()
    {
        dsDetails.SelectCommand = @"SELECT ua.UserAccessID AS ID, u.UserName, ut.Type FROM 
                                  [UserAccess] ua LEFT OUTER JOIN [User] u ON u.UserID=ua.UserID
                                  LEFT OUTER JOIN UserType ut ON ut.UserTypeID=ua.UserTypeID WHERE ua.UserID=" + gvUsers.SelectedValue.ToString();
        gvDetails.DataBind();
        if (gvDetails.Rows.Count > 0)
        {
            lblNoDetails.Visible = false;
        }
        else
        {
            lblNoDetails.Visible = true;
        }
    }
    #endregion

    #region Handled Events
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        String WherePart = FillWherePart();
        FillDataGrid(WherePart);
        gvUsers.DataBind();
        if (gvUsers.Rows.Count < 1)
            lblNoRows.Visible = true;
        else
            lblNoRows.Visible = false;

        gvUsers.SelectedIndex = -1;
    }
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUsers, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }
    protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        Edited = false;
        foreach (GridViewRow row in gvUsers.Rows)
        {
            if (row.RowIndex == gvUsers.SelectedIndex)
            {
                row.ToolTip = string.Empty;
                Fill_Details();
                pnlAddPermission.Visible = true;
                if (gvUsers.SelectedRow.Cells[5].Text == "Yes")
                {
                    btnDeactivate.Visible = true;
                    btnActivate.Visible = false;
                }
                else
                {
                    btnDeactivate.Visible = false;
                    btnActivate.Visible = true;
                }
            }
            else
            {
                row.ToolTip = "Click to select this row.";
            }
        }
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvDetails, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (gvUsers.SelectedRow != null)
        {
            String SQL = "INSERT INTO UserAccess (UserID,UserTypeID,CreatedBy) VALUES (" + gvUsers.SelectedValue.ToString() + "," + ddlUserType.SelectedValue.ToString() + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
            Functions.ExecuteCommand(SQL);
            Fill_Details();
        }
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedRow != null)
        {
            String SQL = "DELETE FROM UserAccess WHERE UserAccessID =" + gvDetails.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            Fill_Details();
        }
    }
    protected void btnDeactivate_Click(object sender, EventArgs e)
    {
        if (gvUsers.SelectedRow != null)
        {
            String SQL = "UPDATE [User] SET Enabled=0 WHERE UserID=" + gvUsers.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            btnSearch_Click(sender, e);
            pnlAddPermission.Visible = false;
        }
    }
    protected void btnActivate_Click(object sender, EventArgs e)
    {
        if (gvUsers.SelectedRow != null)
        {
            String SQL = "UPDATE [User] SET Enabled=1 WHERE UserID=" + gvUsers.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            btnSearch_Click(sender, e);
            pnlAddPermission.Visible = false;
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (!Edited)
        {
            try
            {
                string Id = gvUsers.SelectedValue.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Edit User');", true);
                Edited = true;
            }
            catch { }
        }
        btnSearch_Click(sender, e);
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage('', 'Insert User');", true);
        }
        catch { }

        btnSearch_Click(sender, e);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvUsers.SelectedRow != null)
        {
            String SQL = @"UPDATE Employee SET UserID=null WHERE UserID=" + gvUsers.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            SQL = @"DELETE FROM [UserAccess] WHERE UserID=" + gvUsers.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            SQL = @"DELETE FROM [User] WHERE UserID=" + gvUsers.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            btnSearch_Click(sender, e);
        }
    }
    protected void gvUsers_Sorted(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    protected void gvDetails_Sorted(object sender, EventArgs e)
    {
        Fill_Details();
    }
    protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    #endregion
}