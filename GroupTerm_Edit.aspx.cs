#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class GroupTerm_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Functions.FillCombo("SELECT ClassroomID, Name FROM Classroom", ddlClassroom, "Name", "ClassroomID");

            Functions.FillCombo(@"SELECT 1 as Value,'Monday' as DayDesc UNION
                                    SELECT 2 as Value,'Tuesday' as DayDesc UNION
                                    SELECT 3 as Value,'Wednesday' as DayDesc UNION
                                    SELECT 4 as Value,'Thursday' as DayDesc UNION
                                    SELECT 5 as Value,'Friday' as DayDesc UNION
                                    SELECT 6 as Value,'Saturday' as DayDesc UNION
                                    SELECT 7 as Value,'Sunday' as DayDesc", ddlTerminDay, "DayDesc", "Value");
            Fill_Grid();
            if (Request.QueryString["Type"] == "Preview")
            {
                DisableControls(this, false);
                btnAdd.Visible = false;
                btnRemove.Visible = false;
            }

            if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString())
            {
                btnRemove.Visible = true;
            }
            else
            {
                btnRemove.Visible = false;
            }
        }
    }
    #endregion

    #region Functions
    protected void DisableControls(Control parent, bool State)
    {
        foreach (Control c in parent.Controls)
        {
            if (c is DropDownList)
            {
                ((DropDownList)(c)).Enabled = State;
            }
            else if (c is TextBox)
                ((TextBox)(c)).Enabled = State;

            DisableControls(c, State);
        }
    }
    protected void Fill_Grid()
    {
        if (Request.QueryString["ID"] != "")
        {
            //String[] Termin = Functions.ReturnIntoArray("SELECT * FROM Termin WHERE TerminId=" + Request.QueryString["ID"], 7);
            //ddlTerminDay.SelectedValue = Termin[1];
            //tbTerminFrom.Text = Convert.ToDateTime(Termin[2]).ToString("hh:mm");
            //tbTerminTo.Text = Convert.ToDateTime(Termin[3]).ToString("hh:mm");
            //ddlClassroom.SelectedValue = Termin[4];
            dsMain.SelectCommand = @"SELECT t.*,c.Name as Classroom,
                                    CASE WHEN Day = 1 THEN 'Monday'
                                    WHEN Day = 2 THEN 'Tuesday'
                                    WHEN Day = 3 THEN 'Wednesday'
                                    WHEN Day = 4 THEN 'Thursday'
                                    WHEN Day = 5 THEN 'Friday'
                                    WHEN Day = 6 THEN 'Saturday'
                                    WHEN Day = 7 THEN 'Sunday'
                                    END as DayDesc
                                    FROM Termin t LEFT OUTER JOIN ClassRoom c ON c.ClassRoomID = t.ClassRoomID 
                                    WHERE t.GroupID=" + Request.QueryString["ID"];
            gvMain.DataBind();
        }
    }
    protected void Fill_Details(String SelectedId)
    {
        if (gvMain.SelectedRow != null)
        {
            String[] Termin = Functions.ReturnIntoArray(@"SELECT * FROM Termin WHERE TerminID=" + SelectedId, 8);
            tbTerminId.Text = Termin[0];
            ddlTerminDay.SelectedValue = Termin[1];
            tbTerminFrom.Text = Termin[2].Substring(0, 5);// Convert.ToDateTime(Termin[2]).ToString("hh:mm");
            tbTerminTo.Text = Termin[3].Substring(0, 5); //Convert.ToDateTime(Termin[3]).ToString("hh:mm");
            ddlClassroom.SelectedValue = Termin[4];
            lblInfo.Visible = false;
        }
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
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //String Exist = Functions.ExecuteScalar(@"SELECT TerminID FROM Termin WHERE [Day]='" + ddlTerminDay.SelectedValue +
        //  "' AND TimeStart='" + tbTerminFrom.Text + "' AND TimeEnd='" + tbTerminTo.Text + "' AND ClassRoomID=" + ddlClassroom.SelectedValue);

        //if (Exist != "" && Request.QueryString["ID"] == Exist)
        //{
        //    //nema promena na termin
        //}
        //else //proveri dali e zafaten
        //{
        String Bussy = Functions.ExecuteScalar(@"SELECT COUNT(*) FROM Termin t LEFT OUTER JOIN [Group] g ON g.GroupID=t.GroupID 
                WHERE [Day]='" + ddlTerminDay.SelectedValue + "' AND ClassRoomID='" + ddlClassroom.SelectedValue +
            "' AND ((TimeStart<='" + tbTerminFrom.Text + "' AND '" + tbTerminFrom.Text + "'<TimeEnd) OR (TimeStart < '" + tbTerminTo.Text +
            "' AND '" + tbTerminTo.Text + "' <= TimeEnd)) AND (g.EndDate>=getdate() OR year(g.EndDate)<'2001')");

        if (Convert.ToInt32(Bussy) > 0)
        {
            //zafaten
            lblInfo.Text = "The term is already assigned to another group!";
            lblInfo.Visible = true;
        }
        else
        {
            // ne e zafaten
            String SQL = @"INSERT INTO Termin(Day,TimeStart,TimeEnd,ClassRoomID,GroupID,CreatedBy) VALUES('" + ddlTerminDay.SelectedValue +
                "','" + tbTerminFrom.Text + "','" + tbTerminTo.Text + "'," + ddlClassroom.SelectedValue + "," + Request.QueryString["ID"] + "," + Session["UserID"] + ")";

            Functions.ExecuteCommand(SQL);
            Fill_Grid();

            ddlTerminDay.SelectedIndex = -1;
            ddlClassroom.SelectedIndex = -1;
            tbTerminFrom.Text = "";
            tbTerminTo.Text = "";
        }
        // }
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"DELETE FROM Termin WHERE TerminID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            lblInfo.Text = "The term is removed!";
            lblInfo.Visible = true;
            Fill_Grid();
        }
    }
    #endregion
}