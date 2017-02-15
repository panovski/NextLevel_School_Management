#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class StudentsContract_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
                Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
            {
                Functions.FillCombo(@"SELECT gs.GroupStudentID, g.GroupName + '-' + gt.Language + '-' + gt.Level as [Description] FROM 
                                GroupStudent gs LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                                LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                                LEFT OUTER JOIN Employee e ON e.EmployeeID=g.EmployeeID
                                WHERE gs.StudentID=" + Request.QueryString["ID"] + " AND e.UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value), ddlCourse, "Description", "GroupStudentID");
            }
            else
                Functions.FillCombo(@"SELECT gs.GroupStudentID, g.GroupName + '-' + gt.Language + '-' + gt.Level as [Description] FROM 
                                GroupStudent gs LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                                LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                                WHERE gs.StudentID=" + Request.QueryString["ID"], ddlCourse, "Description", "GroupStudentID");

            Fill_Grid();

            if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
            else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
            String PermLevel = Functions.Decrypt(Request.Cookies["PermLevel"].Value);

            if (PermLevel == ConfigurationManager.AppSettings["Admin"].ToString())
            {
                btnDelete.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                btnDelete.Visible = false;
                btnSave.Visible = false;
            }
        }
    }
    #endregion

    #region Functions
    protected void Fill_Grid()
    {
        dsMain.SelectCommand = @"SELECT c.*, g.GroupName + '-' + gt.Language+'-'+gt.Level as Course
                            FROM [Contract] c LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=c.GroupStudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                            WHERE gs.StudentID=" + Request.QueryString["ID"];
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
        String[] Contract = Functions.ReturnIntoArray("SELECT c.*, u.FirstName+' '+u.LastName as CreatedByName FROM [Contract] c LEFT OUTER JOIN [User] u ON u.UserID=c.CreatedBy WHERE c.ContractID=" + gvMain.SelectedValue, 8);
        ddlCourse.SelectedValue = Contract[1];
        tbStartDate.Text = Convert.ToDateTime(Contract[2]).ToString("dd.MM.yyyy");
        if(Contract[3]!="")
            tbEndDate.Text = Convert.ToDateTime(Contract[3]).ToString("dd.MM.yyyy");
        tbCreatedDate.Text = Contract[5];
        tbCreatedBy.Text = Contract[7];
        lblInfo.Visible = false;
        btnSave.Visible = true;
    }
    protected void gvMain_Sorted(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd.MM.yyyy"; 

            String EndD = "";
            if (tbEndDate.Text == "")
                EndD = "NULL";
            else
                EndD = "'" + Convert.ToDateTime(tbEndDate.Text.Replace("'", "''"),dateInfo) + "'";

            String SQL = "UPDATE [Contract] SET GroupStudentID="+ddlCourse.SelectedValue+", StartDate='" + tbStartDate.Text.Replace("'", "''") + "', EndDate=" + EndD +
            " WHERE ContractID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            lblInfo.Text = "The changes are saved!";
            lblInfo.Visible = true;
            Fill_Grid();            
        }
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
        dateInfo.ShortDatePattern = "dd.MM.yyyy"; 

        String EndD = "";
        if (tbEndDate.Text == "")
            EndD = "NULL";
        else
            EndD = "'" + Convert.ToDateTime(tbEndDate.Text.Replace("'", "''"),dateInfo)+"'";

        //int NotClosed = Convert.ToInt32(Functions.ExecuteScalar("SELECT Count(*) FROM [Contract] WHERE EndDate IS NULL AND StudentID="+Request.QueryString["ID"] ));

        //if (NotClosed == 0)
        //{
            String SQL = "INSERT INTO [Contract] (GroupStudentID,StartDate,EndDate,CreatedBy) VALUES('" + ddlCourse.SelectedValue + "','" +
                tbStartDate.Text.Replace("'", "''") + "'," + EndD + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
            Functions.ExecuteCommand(SQL);
            lblInfo.Text = "New contract is created!";
            lblInfo.Visible = true;
            Fill_Grid();
        //}
        //else
        //{
        //    lblInfo.Text = "There are contracts that are still active! Please close the ones first!";
        //    lblInfo.Visible = true;
        //}
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = "DELETE FROM [Contract] WHERE ContractID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            lblInfo.Text = "The contract is deleted!";
            lblInfo.Visible = true;
            Fill_Grid();
        }
    }
    #endregion
}