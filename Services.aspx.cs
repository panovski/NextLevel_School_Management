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
            Functions.FillCombo("SELECT TemplateName, TemplateFile FROM Template WHERE TemplateType=5 ORDER BY CreatedDate DESC", ddlTemplateCertificate, "TemplateName", "TemplateFile");
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
        if (PermLevel == ConfigurationManager.AppSettings["Admin"].ToString()
           || PermLevel == ConfigurationManager.AppSettings["Edit"].ToString()
           || PermLevel == ConfigurationManager.AppSettings["Advanced"].ToString())
        {
            pnlTopMeni.Visible = true;
        }
        else
        {
            pnlTopMeni.Visible = false;
        }

        if (PermLevel == ConfigurationManager.AppSettings["Admin"].ToString())
        {
            btnServiceType.Visible = true;
            btnDelete.Visible = true;
            btnEdit.Visible = true;
            btnDeleteAllCertificates.Visible = true;
        }
        else
        {
            btnEdit.Visible = false;
        }
    }
    public void FillDataGrid(String WherePart)
    {
        dsMain.SelectCommand = @"SELECT s.ServiceID, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName + '-' + st.Description as ServiceName, c.FirstName+' '+c.LastName as Customer, e.FirstName+' '+e.LastName as Employee,
                                CASE WHEN s.Status=0 THEN 'Done' WHEN s.Status=1 THEN 'Active' END as Status
                                FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                                LEFT OUTER JOIN Customer c ON c.CustomerID=s.CustomerID
                                LEFT OUTER JOIN Employee e ON e.EmployeeID = s.EmployeeID " + WherePart + " ORDER BY s.ServiceID DESC";
    }
    private string FillWherePart()
    {
        String WherePart = "";

        WherePart = Functions.VratiWherePart(tbServiceName, "st.ServiceName", WherePart);
        WherePart = Functions.VratiWherePart(tbCustomer, "c.FirstName+c.LastName", WherePart);
        WherePart = Functions.VratiWherePart(tbEmployee, "e.FirstName+e.LastName", WherePart);

        if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
               Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
        {
            TextBox tbTempEmpl = new TextBox();
            tbTempEmpl.Text = Functions.Decrypt(Request.Cookies["UserID"].Value);
            WherePart = Functions.VratiWherePart(tbTempEmpl, "e.UserID", WherePart);
            tbEmployee.Enabled = false;
        }

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
                    (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalPaid, s.TotalCost - (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalRemain					
					FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
					LEFT OUTER JOIN Payment p ON p.ServiceID = s.ServiceID
                    WHERE s.ServiceID= " + gvMain.SelectedValue + "	GROUP BY s.TotalCost";

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

    protected void Fill_Certificates(String ServiceGroupID = "")
    {
        if (gvMain.SelectedRow != null)
        {
            dsCertificates.SelectCommand = @"SELECT c.*, cu.FirstName + ' ' + cu.LastName as Customer
                                            FROM [Certificate] c LEFT OUTER JOIN [Service] s ON s.ServiceID=c.ServiceID
                                            LEFT OUTER JOIN Customer cu ON cu.CustomerID=s.CustomerID
                                            WHERE c.ServiceID=" + gvMain.SelectedValue;

            if (ServiceGroupID != "")
            {
                dsCertificates.SelectCommand += " AND c.ServiceID=" + ServiceGroupID;
            }
            gvCertificates.DataBind();
        }
    }
    private void PrintPDF(String Path)
    {
        //FileInfo fileInfo = new FileInfo(Path + ".pdf");
        FileInfo fileInfo = new FileInfo(Path + "-1.docx");
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
            String SQL = @"DELETE FROM Payment WHERE ServiceID=" + gvMain.SelectedValue.ToString() + ";DELETE FROM Service WHERE ServiceID=" + gvMain.SelectedValue.ToString();
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
                Fill_Certificates(gvMain.SelectedValue.ToString());
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
						convert(varchar,p.DateOfPayment,104) as DateOfPayment, s.TotalCost as TotalCost, 
						s.TotalCost-(SELECT SUM(p2.Ammount) FROM Payment p2 where p2.ServiceID=s.ServiceID AND p2.DateOfPayment<=p.DateOfPayment) as RemainingCost,
						(SELECT COUNT(*) FROM Payment p2 where p2.ServiceID=s.ServiceID AND p2.DateOfPayment<=p.DateOfPayment) as NoPayments,
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
    protected void gvCertificates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCertificates, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvCertificates_Sorted(object sender, EventArgs e)
    {
        Fill_Certificates();
    }
    protected void imgbtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Certificates();
    }
    protected void btnCreateAllCertificates_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"INSERT INTO [Certificate] (ServiceID, CreatedBy) 
                           VALUES ("+gvMain.SelectedValue+","+ Functions.Decrypt(Request.Cookies["UserID"].Value)+")";
            
            Functions.ExecuteCommand(SQL);
            Fill_Certificates();
        }
    }
    protected void btnDeleteAllCertificates_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"DELETE FROM [Certificate] WHERE ServiceID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Certificates();
        }
    }
    protected void btnPrintSelectedCertificate_Click(object sender, EventArgs e)
    {
        if (gvCertificates.SelectedRow != null)
        {
            string FullDescription = Functions.ExecuteScalar(@"SELECT st.Description 
                                    FROM ServiceType st LEFT OUTER JOIN [Service] s ON s.ServiceTypeID=st.ServiceTypeID
                                    WHERE s.ServiceID=" + gvMain.SelectedValue);
            String[] Description = FullDescription.Split(';');

            String Language = "", LevelDescription = "", Level = "", Program = "", NumberOfClasses = "";
            if (Description.Length >= 1)
                Language = Description[0];
            if (Description.Length >= 2)
                LevelDescription = Description[1];
            if (Description.Length >= 3)
                Level = Description[2];
            if (Description.Length >= 4)
                Program = Description[3];
            if (Description.Length == 5)
                NumberOfClasses = Description[4];
            
            string SQLPrint = @"SELECT c.RegNo, cu.FirstName + ' ' + cu.LastName as StudentName, convert(varchar,cu.DateOfBirth,104) as DateOfBirth,
                            cu.Place, N'" + Language + "' as Language, N'" + LevelDescription + "' as LevelDescription, N'" + Level + "' as Level, N'" + Program + "' as Program, N'" + NumberOfClasses + @"' as NumberOfClasses, 
							convert(varchar,se.CreatedDate,104) as StartDate, convert(varchar,se.ToDate,104) as EndDate, 
							convert(varchar,se.ToDate,104) as DateOfPrint, e.FirstName + ' ' + e.LastName as Teacher
                            FROM [Certificate] c LEFT OUTER JOIN [Service] se ON se.ServiceID=c.ServiceID
							LEFT OUTER JOIN Customer cu ON cu.CustomerID=se.CustomerID
							LEFT OUTER JOIN Employee e ON e.EmployeeID=se.EmployeeID
							LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=se.ServiceTypeID
                            WHERE c.CertificateID=" + gvCertificates.SelectedValue;

            String[] Detali = Functions.ReturnIntoArray(SQLPrint, 13);
            String StudentName = Detali[1];
            String Place = Detali[3];
            String Lang = Detali[4];
            String Teacher = Detali[12];
            StudentName = Functions.ReturnLatin(StudentName);
            Place = Functions.ReturnLatin(Place);
            Lang = Functions.ReturnLatin(Lang);
            Teacher = Functions.ReturnLatin(Teacher);

            SQLPrint = @"SELECT c.RegNo, '"+StudentName +@"' as StudentName, convert(varchar,cu.DateOfBirth,104) as DateOfBirth, '"+Place+@"' as Place,
                            N'" + Lang + "' as Language, N'" + LevelDescription + "' as LevelDescription, N'" + Level + "' as Level, N'" + Program + "' as Program, N'" + NumberOfClasses + @"' as NumberOfClasses, 
							convert(varchar,se.CreatedDate,104) as StartDate, convert(varchar,se.ToDate,104) as EndDate, 
							convert(varchar,se.ToDate,104) as DateOfPrint, '" + Teacher + @"' as Teacher, (CASE WHEN cu.Gender = 1 THEN '' ELSE 'a' END) as Gender
                            FROM [Certificate] c LEFT OUTER JOIN [Service] se ON se.ServiceID=c.ServiceID
							LEFT OUTER JOIN Customer cu ON cu.CustomerID=se.CustomerID
							LEFT OUTER JOIN Employee e ON e.EmployeeID=se.EmployeeID
							LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=se.ServiceTypeID
                            WHERE c.CertificateID=" + gvCertificates.SelectedValue;

            String PathDoc = Server.MapPath(ddlTemplateCertificate.SelectedValue.Replace(".dotx", ""));
            Functions.PrintWord(PathDoc, SQLPrint);
            PrintPDF(PathDoc);
        }
    }
}