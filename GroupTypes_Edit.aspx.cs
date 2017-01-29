#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class GroupTypes_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            Fill_Grid();
        }
    }
    #endregion

    #region Functions
    protected void Login_Redirect()
    {
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
    }
    protected void Fill_Grid()
    {
        dsMain.SelectCommand = @"SELECT *, GroupTypeID as ID FROM GroupType";
    }
    protected void Fill_Details(String SelectedId)
    {
        String[] GroupType = Functions.ReturnIntoArray(@"SELECT gt.GroupTypeID,gt.Language,gt.Program, gt.Level, 
        gt.LevelDescription, gt.CreatedDate, u.UserName FROM GroupType gt LEFT OUTER JOIN [User] as u ON 
        u.UserID=gt.CreatedBy WHERE GroupTypeID=" + SelectedId, 7);
        tbLanguage.Text = GroupType[1];
        tbProgram.Text = GroupType[2];
        tbLevel.Text = GroupType[3];
        tbLevelDescription.Text = GroupType[4];
        tbCreatedDate.Text = Convert.ToDateTime(GroupType[5]).ToString("yyyy-MM-dd");
        tbCreatedBy.Text = GroupType[6];
    }
    #endregion

    #region Handled Events
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvMain, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowIndex == gvMain.SelectedIndex)
            {
                //lblAlreadyAdded.Visible = false;
                row.ToolTip = string.Empty;
                Fill_Details(gvMain.SelectedValue.ToString());
                btnSave.Visible = true;
                //pnlAddPermission.Visible = true;
                //Login_Redirect();
                break;
            }
            else
            {
                row.ToolTip = "Click to select this row.";
            }
        }
    }
    protected void gvMain_Sorted(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedValue != null)
        {
            String SQL = @"UPDATE GroupType SET Language=N'" + tbLanguage.Text.Replace("'", "''") + "', Program=N'" + tbProgram.Text +
                "', Level=N'" + tbLevel.Text.Replace("'", "''") + "', LevelDescription=N'" + tbLevelDescription.Text.Replace("'", "''") +
                "' WHERE GroupTypeID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Grid();
        }
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        String SQL = @"INSERT INTO GroupType (Language,Program,Level,LevelDescription,CreatedBy) 
            VALUES(N'" + tbLanguage.Text.Replace("'", "''") + "',N'" + tbProgram.Text.Replace("'", "''") +
            "',N'" + tbLevel.Text.Replace("'", "''") + "',N'" + tbLevelDescription.Text.Replace("'", "''") + "'," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
        Functions.ExecuteCommand(SQL);
        Fill_Grid();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        String SQL = @"DELETE FROM GroupType WHERE GroupTypeID=" + gvMain.SelectedValue;
        Functions.ExecuteCommand(SQL);
        Fill_Grid();
    }
    #endregion
    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Fill_Grid();
    }
}