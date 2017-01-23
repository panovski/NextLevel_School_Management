#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Services : System.Web.UI.Page
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
            btnSearch_Click(sender, e);
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        if (Session["PermLevel"] == null) Response.Redirect("Default.aspx");
        if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString()
           || Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Edit"].ToString()
           || Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Advanced"].ToString())
        {
            pnlTopMeni.Visible = true;
        }
        else
        {
            pnlTopMeni.Visible = false;
        }

        if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString())
        {
            btnServiceType.Visible = true;
            btnDelete.Visible = true;
            btnEdit.Visible = true;
        }
        else
        {
            btnEdit.Visible = false;
        }
    }
    public void FillDataGrid(String WherePart)
    {
        dsMain.SelectCommand = @"SELECT s.ServiceID, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as ServiceName, c.FirstName+' '+c.LastName as Customer, e.FirstName+' '+e.LastName as Employee,
                                CASE WHEN s.Status=0 THEN 'Done' WHEN s.Status=1 THEN 'Active' END as Status
                                FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                                LEFT OUTER JOIN Customer c ON c.CustomerID=s.CustomerID
                                LEFT OUTER JOIN Employee e ON e.EmployeeID = s.EmployeeID " + WherePart;
    }
    private string FillWherePart()
    {
        String WherePart = "";

        WherePart = Functions.VratiWherePart(tbServiceName, "st.ServiceName", WherePart);
        WherePart = Functions.VratiWherePart(tbCustomer, "c.FirstName+c.LastName", WherePart);
        WherePart = Functions.VratiWherePart(tbEmployee, "e.FirstName+e.LastName", WherePart);

        if (WherePart.Length > 0) WherePart = " WHERE " + WherePart;
        return WherePart;
    }
    protected void Fill_Payments()
    {
        dsPayments.SelectCommand = @"SELECT * FROM Payment p 
                                    WHERE p.ServiceID='" + gvMain.SelectedValue + "'";
        gvPayments.DataBind();
        CalculatePayments();
    }
    protected void CalculatePayments()
    {
        String SQL = @"SELECT COUNT(p.PaymentID) as NumberOfPayments, 
                    (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalPaid, st.Cost - (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalRemain					
					FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
					LEFT OUTER JOIN Payment p ON p.ServiceID = s.ServiceID
                    WHERE s.ServiceID= " + gvMain.SelectedValue + "	GROUP BY st.Cost";

        string[] Payments = Functions.ReturnIntoArray(SQL, 3);

        if (Payments.Count() > 0)
        {
            tbNumberOfPayments.Text = Payments[0];
            tbTotalPaid.Text = Payments[1];
            tbRemainingCosts.Text = Payments[2];

            if (tbRemainingCosts.Text.Length > 0)
            {
                if (Convert.ToInt32(tbRemainingCosts.Text) > 0)
                {
                    tbRemainingCosts.BackColor = System.Drawing.Color.Red;
                    tbRemainingCosts.ForeColor = System.Drawing.Color.White;
                }
                else if (Convert.ToInt32(tbRemainingCosts.Text) == 0)
                {
                    tbRemainingCosts.BackColor = System.Drawing.Color.Green;
                    tbRemainingCosts.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    tbRemainingCosts.BackColor = System.Drawing.Color.Yellow;
                    tbRemainingCosts.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                tbRemainingCosts.BackColor = tbTotalPaid.BackColor;
                tbRemainingCosts.ForeColor = tbTotalPaid.ForeColor;
            }
        }
    }
    #endregion

    #region Handled Events
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (!Edited)
        {
            try
            {
                string Id = gvMain.SelectedValue.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Edit Service','');", true);
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
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage('', 'Insert Service','');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }

        btnSearch_Click(sender, e);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"DELETE FROM Service WHERE ServiceID=" + gvMain.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            btnSearch_Click(sender, e);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        String WherePart = FillWherePart();
        FillDataGrid(WherePart);
        gvMain.DataBind();
        if (gvMain.Rows.Count < 1)
            lblNoRows.Visible = true;
        else
            lblNoRows.Visible = false;

        gvMain.SelectedIndex = -1;
    }
    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        Edited = false;
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowIndex == gvMain.SelectedIndex)
            {
                row.ToolTip = string.Empty;
                Login_Redirect();
                Fill_Payments();
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
        btnSearch_Click(sender, e);
    }
    protected void btnPrintPayment_Click(object sender, EventArgs e)
    {
        if (gvPayments.SelectedRow != null)
        {
            string SQLPrint = @"SELECT p.AccountNumber,p.Ammount, p.AmmountWords, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as PaymentGroup,
                        c.FirstName+' '+c.LastName as PaymentName, p.PaymentNumber, c.Place as PaymentPlace,
						convert(varchar,p.DateOfPayment,104) as DateOfPayment, st.Cost as TotalCost, st.Cost-p.Ammount as RemainingCost,
						(SELECT COUNT(*) FROM Payment p2 where p2.ServiceID=s.ServiceID) as NoPayments,
						(SELECT COUNT(*) FROM Payment p2 where p2.ServiceID=s.ServiceID) as TotalNoPayment						                    
						FROM Payment p LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID 
						LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
						LEFT OUTER JOIN Customer c ON c.CustomerID=s.CustomerID
						WHERE p.PaymentID=" + gvPayments.SelectedValue;

            String PaymentPath = Functions.ExecuteScalar("SELECT TOP 1 TemplateFile FROM Template WHERE TemplateType=3 ORDER BY CreatedDate DESC");
            String PathDoc = Server.MapPath(PaymentPath.Replace(".dotx", ""));
            Functions.PrintWord(PathDoc, SQLPrint);
            //FileInfo fileInfo = new FileInfo(PathDoc + ".pdf");
            FileInfo fileInfo = new FileInfo(PathDoc + "-1.docx");
            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(fileInfo.FullName);
                Response.End();
            }
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvMain, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvPayments_Sorted(object sender, EventArgs e)
    {
        Fill_Payments();
    }
    protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPayments, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnServiceType_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowServiceTypes('Service Types');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    #endregion
}