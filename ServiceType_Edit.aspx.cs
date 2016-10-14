#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class ServiceType_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           Fill_Grid();
        }
    }
    #endregion

    #region Functions
    protected void Fill_Grid()
    {
        dsMain.SelectCommand = @"SELECT *, ServiceTypeID as ID FROM ServiceType";
    }
    protected void Fill_Details(String SelectedId)
    {
        String[] ServiceType = Functions.ReturnIntoArray(@"SELECT st.*, u.UserName FROM ServiceType st LEFT OUTER JOIN [User] as u ON 
        u.UserID=st.CreatedBy WHERE ServiceTypeID=" + SelectedId, 8);
        tbServiceName.Text = ServiceType[1];
        tbDescription.Text = ServiceType[2];
        tbCost.Text = ServiceType[3];
        tbEmployeePercentage.Text = ServiceType[4];
        tbCreatedDate.Text = Convert.ToDateTime(ServiceType[5]).ToString("yyyy-MM-dd");
        tbCreatedBy.Text = ServiceType[7];
    }    
    #endregion

    #region Handled Events
    protected void gvMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowIndex == gvMain.SelectedIndex)
            {
                row.ToolTip = string.Empty;
                Fill_Details(gvMain.SelectedValue.ToString());
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvMain, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        String SQL = @"UPDATE ServiceType SET ServiceName=N'" + tbServiceName.Text.Replace("'", "''") + "', Description=N'" + tbDescription.Text +
               "', Cost=N'" + tbCost.Text.Replace("'", "''") + "', EmployeePercentage=N'" + tbEmployeePercentage.Text.Replace("'", "''") + "'" +
               " WHERE ServiceTypeID=" + gvMain.SelectedValue;
        Functions.ExecuteCommand(SQL);
        Fill_Grid();

    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        String SQL = @"INSERT INTO ServiceType (ServiceName,Description,Cost,CreatedBy,EmployeePercentage) 
            VALUES(N'" + tbServiceName.Text.Replace("'", "''") + "',N'" + tbDescription.Text.Replace("'", "''") +
            "',N'" + tbCost.Text.Replace("'", "''") + "'," + Session["UserID"] + ", N'" + tbEmployeePercentage.Text.Replace("'", "''") + "')";
        Functions.ExecuteCommand(SQL);
        Fill_Grid();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"DELETE FROM ServiceType WHERE ServiceTypeID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Grid();
        }
    }
    #endregion
}