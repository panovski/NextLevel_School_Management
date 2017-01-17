#region Using
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Templates_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Functions.FillCombo(@"SELECT 1 as Value, 'Contract' as Description UNION " +
                                " SELECT 2 as Value, 'Certificate' as Description UNION SELECT 3 as Value, 'Payment' as Description", ddlType, "Description", "Value");
            Fill_Grid();
        }
    }
    #endregion

    #region Functions
    protected void Fill_Grid()
    {
        dsMain.SelectCommand = @"SELECT t.*, u.FirstName + ' ' + u.LastName as CreatedByUser FROM Template t LEFT OUTER JOIN [User] u ON u.UserID=t.CreatedBy";
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
        String[] Template = Functions.ReturnIntoArray("SELECT t.*, u.FirstName+' '+u.LastName as CreatedByName FROM [Template] t LEFT OUTER JOIN [User] u ON u.UserID=t.CreatedBy WHERE t.TemplateId=" + gvMain.SelectedValue, 7);
        tbTemplateName.Text = Template[1];
        //fuFile.PostedFile.FileName = Template[2];
        ddlType.SelectedValue = Template[3];
        tbCreatedDate.Text = Template[4];
        tbCreatedBy.Text = Template[6];
        lblInfo.Visible = false;
    }
    protected void gvMain_Sorted(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvMain.SelectedRow != null)
            {
                String SQL = "";
                string fileName = Path.GetFileName(fuFile.PostedFile.FileName);
                if (fileName != "")
                {
                    fuFile.PostedFile.SaveAs(Server.MapPath("~/Templates/Templates/") + fileName);

                    SQL = "UPDATE Template SET TemplateName=N'" + tbTemplateName.Text.Replace("'", "''") +
                            "',TemplateFile='~/Templates/Templates/" + fileName + "',TemplateType=" + ddlType.SelectedValue + ", CreatedDate=GetDate() WHERE TemplateID="+gvMain.SelectedValue;
                }
                else
                {
                    SQL = "UPDATE Template SET TemplateName=N'" + tbTemplateName.Text.Replace("'", "''") + "',TemplateType=" + ddlType.SelectedValue + ", CreatedDate=GetDate() WHERE TemplateID=" + gvMain.SelectedValue;
                }
                
                Functions.ExecuteCommand(SQL);
                Fill_Grid();

                lblInfo.Text = "All changes are saved!";
                lblInfo.Visible = true;
            }
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            string fileName = Path.GetFileName(fuFile.PostedFile.FileName);
            if (fileName != "")
            {
                fuFile.PostedFile.SaveAs(Server.MapPath("~/Templates/Templates/") + fileName);
                String SQL = "INSERT INTO Template (TemplateName,TemplateFile,TemplateType,CreatedBy) VALUES (N'" + tbTemplateName.Text.Replace("'", "''") +
                            "','~/Templates/Templates/" + fileName + "'," + ddlType.SelectedValue + "," + Session["UserID"] + ")";
                Functions.ExecuteCommand(SQL);
                Fill_Grid();

                lblInfo.Text = "The template is created!";
                lblInfo.Visible = true;
            }
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvMain.SelectedRow != null)
            {
                String SQL = "DELETE FROM Template WHERE TemplateID=" + gvMain.SelectedValue;
                Functions.ExecuteCommand(SQL);
                Fill_Grid();

                lblInfo.Text = "The template is deleted!";
                lblInfo.Visible = true;
            }
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }       
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            string[] FileName = gvMain.SelectedRow.Cells[2].Text.ToString().Split('/');
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + FileName[3]+ ";");
            response.TransmitFile(Server.MapPath(gvMain.SelectedRow.Cells[2].Text));
            response.Flush();
            response.End();
        }
    }
    #endregion
}