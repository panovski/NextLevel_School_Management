#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class TransferDetails : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            if (Request.QueryString["ID"] != "")
            {
                FillDetails();
                DisableControls(this, false);
            }            
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
        String PermLevel = Functions.Decrypt(Request.Cookies["PermLevel"].Value);
        if (PermLevel != ConfigurationManager.AppSettings["Admin"].ToString() &&
          PermLevel != ConfigurationManager.AppSettings["Advanced"].ToString() &&
          PermLevel != ConfigurationManager.AppSettings["Edit"].ToString())
        {
            Response.Redirect("Default.aspx");
        }
    }
    public void FillDetails()
    {
        String[] Transfer = Functions.ReturnIntoArray(@"SELECT st.StudentTransferID, s.FirstName+' '+s.LastName as Student, 
                                            gf.GroupName+'-'+gtf.Language+'-'+gtf.LevelDescription+'-'+gtf.Level as FromGroup,
                                            gt.GroupName+'-'+gtt.Language+'-'+gtt.LevelDescription+'-'+gtt.Level as ToGroup,
                                            st.TransferDate, st.CreatedDate, u.FirstName+' '+u.LastName as CreatedBy 
                                            FROM StudentTransfer st
                                            LEFT OUTER JOIN Student s ON s.StudentID=st.StudentID
                                            LEFT OUTER JOIN [Group] gf ON gf.GroupID=st.fromGroupID
                                            LEFT OUTER JOIN [Group] gt ON gt.GroupID=st.toGroupID
                                            LEFT OUTER JOIN GroupType gtf ON gtf.GroupTypeID=gf.GroupTypeID
                                            LEFT OUTER JOIN GroupType gtt ON gtt.GroupTypeID=gt.GroupTypeID
                                            LEFT OUTER JOIN [User] u ON u.UserID=st.CreatedBy WHERE st.fromGroupID=" +
                                            Request.QueryString["GroupID"] + " AND st.StudentID=" + Request.QueryString["StudentID"], 7);
        tbStudent.Text = Transfer[1];
        tbFromGroup.Text = Transfer[2];
        tbToGroup.Text = Transfer[3];
        tbTransferDate.Text = Convert.ToDateTime(Transfer[4]).ToString("yyyy-MM-dd");
        tbCreatedDate.Text = Convert.ToDateTime(Transfer[5]).ToString("yyyy-MM-dd");
        tbCreatedBy.Text = Transfer[6];
    }
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
    #endregion

    #region Handled Events
    #endregion    
}