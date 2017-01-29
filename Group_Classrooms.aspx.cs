#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Group_Classrooms : System.Web.UI.Page
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
        dsMain.SelectCommand = @"SELECT c.*, u.FirstName+' '+u.LastName as CreatedByUser FROM Classroom c LEFT OUTER JOIN [User] u ON u.UserID=c.CreatedBy";
    }
    protected void Fill_Details(String SelectedId)
    {
        String[] GroupType = Functions.ReturnIntoArray(@"SELECT c.*, u.FirstName+' '+u.LastName as CreatedByUser FROM Classroom c LEFT OUTER JOIN [User] u ON u.UserID=c.CreatedBy WHERE c.ClassroomID=" + SelectedId, 6);
        tbClassroomName.Text = GroupType[1];
        tbDescription.Text = GroupType[2];
        tbCreatedDate.Text = Convert.ToDateTime(GroupType[3]).ToString("yyyy-MM-dd");
        tbCreatedBy.Text = GroupType[5];
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
                row.ToolTip = string.Empty;
                Fill_Details(gvMain.SelectedValue.ToString());
                btnSave.Visible = true;
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
    protected void gvMain_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        String SQL = @"INSERT INTO Classroom (Name,Description,CreatedBy) 
            VALUES(N'" + tbClassroomName.Text.Replace("'", "''") + "',N'" + tbDescription.Text.Replace("'", "''") +
            "'," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
        Functions.ExecuteCommand(SQL);
        Fill_Grid();
        lblInfo.Text = "The classroom is inserted!";
        lblInfo.Visible = true;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedValue != null)
        {
            String SQL = @"DELETE FROM Termin WHERE ClassroomID=" + gvMain.SelectedValue + "; DELETE FROM Classroom WHERE ClassroomID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Grid();
            lblInfo.Text = "The classroom is deleted!";
            lblInfo.Visible = true;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedValue != null)
        {
            String SQL = @"UPDATE Classroom SET Name=N'" + tbClassroomName.Text.Replace("'", "''") + "', Description=N'" + tbDescription.Text +
                         "' WHERE ClassroomID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Grid();
            lblInfo.Text = "The changes are saved!";
            lblInfo.Visible = true;
        }
    }
    #endregion
}