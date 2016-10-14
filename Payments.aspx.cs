﻿#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Payments : System.Web.UI.Page
{
    #region Variables
    private static bool Edited = false;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Functions.FillCombo("SELECT -1 as GroupID, '' as Description UNION SELECT g.GroupId, g.GroupName + '-' + gt.Language + '-' + gt.LevelDescription as Description FROM [Group] g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID ORDER BY GroupID", ddlGroup, "Description", "GroupID");
            Functions.FillCombo(@"SELECT CustomerID, FirstName+' '+LastName as Name FROM Customer", ddlCustomers, "Name", "CustomerID");
            Functions.FillCombo(@"SELECT g.GroupID, g.GroupName + ' - ' + gt.Language + ' - ' + gt.LevelDescription as Course
                                FROM [Group] g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID = g.GroupTypeID
                                WHERE g.Invoice=1", ddlInvoiceGroup, "Course", "GroupID");
            Login_Redirect();
            btnSearch_Click(sender, e);
            rblType.SelectedValue = "0";
            tbAddPaymentNumber.Text = Get_PaymentNumber();
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        if (Session["PermLevel"] == null) Response.Redirect("Default.aspx");
        if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Admin"].ToString())
        {
            pnlTopMeni.Visible = true;
            pnlPayment.Visible = true;
        }
        else if (Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Edit"].ToString()
            || Session["PermLevel"].ToString() == ConfigurationManager.AppSettings["Advanced"].ToString())
        {
            pnlTopMeni.Visible = false;
            pnlPayment.Visible = true;
        }
        else
        {
            pnlTopMeni.Visible = false;
            pnlPayment.Visible = false;
            btnAddPayment.Visible = false;
            btnRecalculate.Visible = false;
            btnInserNewPayment.Visible = false;
            btnPrintPayment.Visible = false;

            btnRefreshInvoices.Enabled = false;
            btnAllStudents.Enabled = false;
            btnCustomersRefresh.Enabled = false;
        }

    }
    public void FillDataGrid(String WherePart)
    {
        dsMain.SelectCommand = @"SELECT PaymentID, PaymentNumber, Ammount, 
							(SELECT CASE WHEN p.GroupStudentID IS NOT NULL THEN s.FirstName+' ' +s.LastName WHEN p.ServiceID IS NOT NULL THEN cu.FirstName+' ' +cu.LastName WHEN p.InvoiceID IS NOT NULL THEN 'Group Invoice' END) as StudentClient,
							(SELECT CASE WHEN p.GroupStudentID IS NOT NULL THEN e.FirstName + ' ' +e.LastName WHEN p.ServiceID IS NOT NULL THEN u2.FirstName+' ' +u2.LastName WHEN p.InvoiceID IS NOT NULL THEN u3.FirstName+' ' +u3.LastName END) as Teacher,
                            (SELECT CASE WHEN p.GroupStudentID IS NOT NULL THEN 'Course' WHEN p.ServiceID IS NOT NULL THEN 'Service' WHEN p.InvoiceID IS NOT NULL THEN 'Invoice' END) as PaymentType, p.Transfered
                            FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                            LEFT OUTER JOIN Student s ON s.StudentID=gs.StudentID LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                            LEFT OUTER JOIN GroupType gt on gt.GroupTypeID=g.GroupTypeID
                            LEFT OUTER JOIN Employee e ON e.EmployeeID=g.EmployeeID
                            LEFT OUTER JOIN [Service] srv ON srv.ServiceID=p.ServiceID
                            LEFT OUTER JOIN Customer cu ON cu.CustomerID=srv.CustomerID
                            LEFT OUTER JOIN [Employee] u2 ON u2.EmployeeID=srv.EmployeeID
                            LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=srv.ServiceTypeID
							LEFT OUTER JOIN [Group] inv ON inv.GroupID=p.InvoiceID
							LEFT OUTER JOIN [Employee] u3 ON u3.EmployeeID=inv.EmployeeID " + WherePart + " ORDER BY p.PaymentID DESC";
    }
    private string FillWherePart()
    {
        String WherePart = "";

        WherePart = Functions.VratiWherePart(tbPaymentNumber, "p.PaymentNumber", WherePart);
        WherePart = Functions.VratiWherePart(tbStudent, "s.FirstName+s.LastName", WherePart);
        WherePart = Functions.VratiWherePartDDL(ddlGroup, "g.GroupID", WherePart);
        WherePart = Functions.VratiWherePartInteger(tbDateFrom, "p.DateOfPayment", ">=", WherePart);
        WherePart = Functions.VratiWherePartInteger(tbDateTo, "p.DateOfPayment", "<=", WherePart);

        if (WherePart.Length > 0) WherePart = " WHERE " + WherePart;
        return WherePart;
    }
    protected void Fill_Payment(String PaymentID)
    {
        lblInfo.Visible = false;
        String[] Payment = Functions.ReturnIntoArray("SELECT * FROM Payment WHERE PaymentID=" + PaymentID, 15);
        tbPaymentID.Text = Payment[0];
        tbAddPaymentNumber.Text = Payment[1];
        tbAddAmmount.Text = Payment[2];
        tbAddAmmountWords.Text = Payment[3];
        tbAccountNumber.Text = Payment[4];
        tbAddDateOfPayment.Text = Convert.ToDateTime(Payment[5]).ToString("yyyy-MM-dd"); ;

        if (Payment[6] != "")
        {
            rblType.SelectedValue = "0";
            pnlStudent.Visible = true;
            pnlAdditionalService.Visible = false;
            pnlInvoice.Visible = false;
            String[] GroupStudent = Functions.ReturnIntoArray("SELECT StudentID,GroupID FROM GroupStudent WHERE GroupStudentID = " + Payment[6], 2);
            //String StudentId = Functions.ExecuteScalar("SELECT StudentID FROM GroupStudent WHERE GroupStudentID=" + Payment[6]);
            Functions.FillCombo(@"SELECT StudentID, FirstName+' '+LastName as Name FROM Student WHERE StudentId=" + GroupStudent[0], ddlStudents, "Name", "StudentID");

            Functions.FillCombo(@"SELECT gs.GroupID, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as Description
                            FROM GroupStudent gs LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID 
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID WHERE gs.StudentID=" +
                            GroupStudent[0], ddlAddGroup, "Description", "GroupId");

            ddlStudents.SelectedValue = GroupStudent[0];
            //ddlStudents_SelectedIndexChanged(null, null);
            ddlAddGroup.SelectedValue = GroupStudent[1];
            CalculatePayments();
            tbStudentsSearch.Enabled = false;
            ddlStudents.Enabled = false;
            ddlAddGroup.Enabled = false;
        }
        else if (Payment[7] != "")
        {
            // ovde za service
            rblType.SelectedValue = "1";
            pnlStudent.Visible = false;
            pnlAdditionalService.Visible = true;
            pnlInvoice.Visible = false;
            String[] Service = Functions.ReturnIntoArray("SELECT ServiceID, CustomerID FROM [Service] WHERE ServiceID = " + Payment[7], 2);
            Functions.FillCombo(@"SELECT s.ServiceID, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as ServiceName FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID WHERE ServiceID=" + Service[0], ddlService, "ServiceName", "ServiceID");
            Functions.FillCombo(@"SELECT CustomerID, FirstName+' '+LastName as Name FROM Customer WHERE CustomerID=" + Service[1], ddlCustomers, "Name", "CustomerID");

            ddlService.SelectedValue = Service[0];
            ddlCustomers.SelectedValue = Service[1];

            CalculatePaymentsService();
            tbCustomerSearch.Enabled = false;
            ddlCustomers.Enabled = false;
            ddlService.Enabled = false;

            tbCustomerPaid.Enabled = false;
            tbCustomerPayments.Enabled = false;
            tbCustomerRemaining.Enabled = false;
        }
        else if (Payment[14] != "")
        {
            //za invoice:
            rblType.SelectedValue = "2";
            pnlStudent.Visible = false;
            pnlAdditionalService.Visible = false;
            pnlInvoice.Visible = true;
            String[] Invoice = Functions.ReturnIntoArray("SELECT InvoiceID FROM Payment WHERE PaymentID=" + Payment[0], 1);
            Functions.FillCombo(@"SELECT g.GroupID, g.GroupName + ' - ' + gt.Language + ' - ' + gt.LevelDescription as Course
                                FROM [Group] g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID = g.GroupTypeID
                                WHERE g.Invoice=1 AND g.GroupID=" + Invoice[0], ddlInvoiceGroup, "Course", "GroupID");
            ddlInvoiceGroup.SelectedValue = Invoice[0];

            CalculatePaymentsInvoice();
            ddlInvoiceGroup.Enabled = false;
        }

        rblType.Enabled = false;
        tbPaymentID.Enabled = false;
        tbAddPaymentNumber.Enabled = false;
        tbAddAmmount.Enabled = false;
        tbAddAmmountWords.Enabled = false;
        tbAccountNumber.Enabled = false;
        tbAddDateOfPayment.Enabled = false;

        pnlButtonsAdd.Visible = false;
        pnlButtonsNew.Visible = true;
    }
    protected void CalculatePayments()
    {
        String SQL = @"SELECT COUNT(p.PaymentID) as NumberOfPayments, g.NumberOfPayments-COUNT(p.PaymentID) as RemainPayments,
                    (SELECT CASE WHEN SUM(CASE WHEN p.Transfered=0 THEN p.Ammount END) IS NULL THEN 0 ELSE SUM(CASE WHEN p.Transfered=0 THEN p.Ammount END) END) as TotalPaid, (gs.TotalCost - gs.TotalCost*gs.Discount/100) - (SELECT CASE WHEN SUM(CASE WHEN p.Transfered=0 THEN p.Ammount END) IS NULL THEN 0 ELSE SUM(CASE WHEN p.Transfered=0 THEN p.Ammount END) END) as TotalRemain
                    FROM GroupStudent gs LEFT OUTER JOIN Payment p ON p.GroupStudentID=gs.GroupStudentID
                    LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                    WHERE gs.GroupID=" + ddlAddGroup.SelectedValue + " AND gs.StudentID=" + ddlStudents.SelectedValue +
                    "GROUP BY g.NumberOfPayments,gs.TotalCost, gs.Discount";
        string[] Payments = Functions.ReturnIntoArray(SQL, 4);

        if (Payments.Count() > 0)
        {
            tbNumberOfPayments.Text = Payments[0];
            tbRemainingPayments.Text = Payments[1];
            tbTotalPaid.Text = Payments[2];
            tbRemainingCosts.Text = Payments[3];

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
    protected string Get_PaymentNumber()
    {
        String MaxID = Functions.ExecuteScalar(@"SELECT Max(CAST(LEFT(PaymentNumber, 4) AS INT)) as PaymentN FROM Payment WHERE CAST(RIGHT(PaymentNumber, 2) AS INT)='" + DateTime.Now.ToString("yy") + "'");
        if (MaxID == "") MaxID = "0";
        int NewID = 0;
        NewID = Convert.ToInt32(MaxID) + 1;

        return NewID.ToString().PadLeft(4, '0') + "/" + DateTime.Now.ToString("yy");
    }
    protected void PrintPayment(String PaymentID)
    {
        string SQLPrint = @"SELECT p.AccountNumber,p.Ammount, p.AmmountWords, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as PaymentGroup,
                        s.FirstName+' '+s.LastName as PaymentName, p.PaymentNumber, s.Place as PaymentPlace
                        FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                        LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                        LEFT OUTER JOIN Student s ON s.StudentID=gs.StudentID WHERE p.PaymentID=" + PaymentID;

        if (rblType.SelectedValue == "1")
        {
            SQLPrint = @"SELECT p.AccountNumber,p.Ammount, p.AmmountWords, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as PaymentGroup,
                        c.FirstName+' '+c.LastName as PaymentName, p.PaymentNumber, c.Place as PaymentPlace                        
						FROM Payment p LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID 
						LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
						LEFT OUTER JOIN Customer c ON c.CustomerID=s.CustomerID
						WHERE p.PaymentID=" + PaymentID;
        }

        String PaymentPath = Functions.ExecuteScalar("SELECT TOP 1 TemplateFile FROM Template WHERE TemplateType=3 ORDER BY CreatedDate DESC");
        String PathDoc = Server.MapPath(PaymentPath.Replace(".dotx", ""));
        Functions.PrintWord(PathDoc, SQLPrint);
        FileInfo fileInfo = new FileInfo(PathDoc + ".pdf");
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
    public static string NumberToWords(int number, string Tip)
    {
        if (number == 0)
            return "нула";

        if (number < 0)
            return "минус " + NumberToWords(Math.Abs(number), "");

        string words = "";

        if ((number / 1000000) > 0)
        {
            String Add = "";
            if (number / 1000000 == 1)
                Add = " милион ";
            else
                Add = " милиони ";

            words += NumberToWords(number / 1000000, "milion") + Add;// " милион ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            String Add = "";
            if (number / 1000 == 1)
                Add = " илјада ";
            else
                Add = " илјади ";

            words += NumberToWords(number / 1000, "iljada") + Add;// " илјади ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            String Add = "";
            int Ostatok = number % 100;
            if (Ostatok == 0 && words.Length > 0)
                Add = " и ";
            words += Add + NumberToWords(number / 100, "stotka") + "";
            number %= 100;
        }
        if ((number / 10) > 0)
        {
            String Add = "";
            int Ostatok = number % 10;
            if (Ostatok == 0 && words.Length > 0)
                Add = " и ";
            words += Add;// + NumberToWords(number / 100, "stotka") + "";
            //number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += " ";
            //words += " и ";

            var unitsMap = new[] { "нула", "еден", "два", "три", "четири", "пет", "шест", "седум", "осум", "девет", "десет", "единаесет", "дванаесет", "тринаесет", "четиринаесет", "петнаесет", "шеснаесет", "седумнаесет", "осумнаесет", "деветнаесет" };
            var tensMap = new[] { "нула", "десет", "дваесет", "триесет", "четириесет", "педесет", "шеесет", "седумдесет", "осумдесет", "деведесет" };
            var HundredMap = new[] { "нула", "сто", "двестa", "триста", "четиристотини", "петстотини", "шестотини", "седумстотини", "осумстотини", "деветстотини" };
            var unitsMapThousend = new[] { "нула", "", "две", "три", "четири", "пет", "шест", "седум", "осум", "девет", "десет", "единаесет", "дванаесет", "тринаесет", "четиринаесет", "петнаесет", "шеснаесет", "седумнаесет", "осумнаесет", "деветнаесет" };

            if (number < 20)
            {
                if (Tip == "stotka")
                    words += HundredMap[number];
                else if (Tip == "iljada")
                    words += unitsMapThousend[number];
                else
                    words += unitsMap[number];
            }
            else
            {
                if (Tip == "stotka")
                {
                    words += HundredMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
                else if (Tip == "iljada")
                {
                    words += unitsMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
                else if (Tip == "milion")
                {
                    words += HundredMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMapThousend[number % 10];
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " и " + unitsMap[number % 10];
                }
            }
        }

        return words;
    }
    protected void CalculatePaymentsService()
    {
        String SQL = @"SELECT COUNT(p.PaymentID) as NumberOfPayments, 
                    (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalPaid, st.Cost - (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalRemain					
					FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
					LEFT OUTER JOIN Payment p ON p.ServiceID = s.ServiceID
                    WHERE s.ServiceID= " + ddlService.SelectedValue + "	GROUP BY st.Cost";

        string[] Payments = Functions.ReturnIntoArray(SQL, 3);

        if (Payments.Count() > 0)
        {
            tbCustomerPayments.Text = Payments[0];
            tbCustomerPaid.Text = Payments[1];
            tbCustomerRemaining.Text = Payments[2];

            if (tbCustomerRemaining.Text.Length > 0)
            {
                if (Convert.ToInt32(tbCustomerRemaining.Text) > 0)
                {
                    tbCustomerRemaining.BackColor = System.Drawing.Color.Red;
                    tbCustomerRemaining.ForeColor = System.Drawing.Color.White;
                }
                else if (Convert.ToInt32(tbCustomerRemaining.Text) == 0)
                {
                    tbCustomerRemaining.BackColor = System.Drawing.Color.Green;
                    tbCustomerRemaining.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    tbCustomerRemaining.BackColor = System.Drawing.Color.Yellow;
                    tbCustomerRemaining.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                tbCustomerRemaining.BackColor = tbTotalPaid.BackColor;
                tbCustomerRemaining.ForeColor = tbTotalPaid.ForeColor;
            }
        }
    }
    protected void CalculatePaymentsInvoice()
    {
        String SQL = @"SELECT COUNT(p.PaymentID) as NumberOfPayments, g.NumberOfPayments-COUNT(p.PaymentID) as RemainPayments,
                    (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalPaid, 
                    g.Cost - (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalRemain
                    FROM [Group] g LEFT OUTER JOIN Payment p ON p.InvoiceID=g.GroupID
                    WHERE g.GroupID=" + ddlInvoiceGroup.SelectedValue + " GROUP BY g.NumberOfPayments,g.Cost";

        string[] Payments = Functions.ReturnIntoArray(SQL, 4);

        if (Payments.Count() > 0)
        {
            tbInvoicePayments.Text = Payments[0];
            tbInvoiceRemainingPayments.Text = Payments[1];
            tbInvoicePaid.Text = Payments[2];
            tbInvoiceRemainingCosts.Text = Payments[3];

            if (tbInvoiceRemainingCosts.Text.Length > 0)
            {
                if (Convert.ToInt32(tbInvoiceRemainingCosts.Text) > 0)
                {
                    tbInvoiceRemainingCosts.BackColor = System.Drawing.Color.Red;
                    tbInvoiceRemainingCosts.ForeColor = System.Drawing.Color.White;
                }
                else if (Convert.ToInt32(tbInvoiceRemainingCosts.Text) == 0)
                {
                    tbInvoiceRemainingCosts.BackColor = System.Drawing.Color.Green;
                    tbInvoiceRemainingCosts.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    tbInvoiceRemainingCosts.BackColor = System.Drawing.Color.Yellow;
                    tbInvoiceRemainingCosts.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                tbInvoiceRemainingCosts.BackColor = tbTotalPaid.BackColor;
                tbInvoiceRemainingCosts.ForeColor = tbTotalPaid.ForeColor;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Edit Payment','');", true);
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
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"DELETE FROM Payment WHERE PaymentID=" + gvMain.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            btnSearch_Click(sender, e);
        }
    }
    protected void bthEditGrType_Click(object sender, EventArgs e)
    {

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
    protected void lnkDoubleClick_Click(object sender, EventArgs e)
    {
        try
        {
            string Id = gvMain.SelectedValue.ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Preview payment details', 'Preview');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvMain, "Select$" + e.Row.RowIndex);

            LinkButton _doubleClickButton = (LinkButton)lnkDoubleClick;
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            e.Row.Attributes["ondblclick"] = _jsDouble;
        }
    }
    protected void gvMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        Edited = false;
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowIndex == gvMain.SelectedIndex)
            {
                row.ToolTip = string.Empty;
                Fill_Payment(gvMain.SelectedValue.ToString());
                pnlPayment.Visible = true;
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
    protected void tbStudentsSearch_TextChanged(object sender, EventArgs e)
    {
        ddlStudents.Items.Clear();
        Functions.FillCombo(@"SELECT StudentID, FirstName+' '+LastName as Name FROM Student WHERE 
                            (FirstName LIKE N'%" + tbStudentsSearch.Text.Replace("'", "''") + "%' OR LastName LIKE N'%" + tbStudentsSearch.Text.Replace("'", "''") + "%')", ddlStudents, "Name", "StudentID");
        if (ddlStudents.Items.Count > 0)
        {
            ddlStudents.SelectedIndex = 0;
            ddlStudents_SelectedIndexChanged(sender, e);
        }
    }
    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblType.SelectedValue == "0")
        {
            pnlStudent.Visible = true;
            pnlAdditionalService.Visible = false;
            pnlInvoice.Visible = false;
        }
        else if (rblType.SelectedValue == "1")
        {
            pnlAdditionalService.Visible = true;
            pnlStudent.Visible = false;
            pnlInvoice.Visible = false;
        }
        else if (rblType.SelectedValue == "2")
        {
            pnlAdditionalService.Visible = false;
            pnlStudent.Visible = false;
            pnlInvoice.Visible = true;
        }
    }
    protected void tbCustomerSearch_TextChanged(object sender, EventArgs e)
    {
        ddlCustomers.Items.Clear();
        Functions.FillCombo(@"SELECT CustomerID, FirstName+' '+LastName as Name FROM Customer WHERE 
                            (FirstName LIKE N'%" + tbCustomerSearch.Text.Replace("'", "''") + "%' OR LastName LIKE N'%" + tbCustomerSearch.Text.Replace("'", "''") + "%')", ddlCustomers, "Name", "CustomerID");
    }
    protected void tbAddGroupSearch_TextChanged(object sender, EventArgs e)
    {
        ddlAddGroup.Items.Clear();
        Functions.FillCombo(@"SELECT g.GroupID, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as Description
                            FROM [Group] g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                            WHERE g.Invoice=0 AND g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription LIKE N'%" + tbAddGroupSearch.Text + "%'", ddlAddGroup, "GroupId", "Description");
    }
    protected void ddlStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlAddGroup.Items.Clear();
        Functions.FillCombo(@"SELECT gs.GroupID, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as Description
                            FROM GroupStudent gs LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID 
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID WHERE g.Invoice=0 AND gs.StudentID=" +
                            ddlStudents.SelectedValue, ddlAddGroup, "Description", "GroupId");
        if (ddlAddGroup.Items.Count > 0)
        {
            ddlAddGroup.SelectedIndex = 0;
            ddlAddGroup_SelectedIndexChanged(sender, e);
        }
    }
    protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlService.Items.Clear();
        Functions.FillCombo(@"SELECT s.ServiceID, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as ServiceName
                            FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                            WHERE s.CustomerID=" + ddlCustomers.SelectedValue, ddlService, "ServiceName", "ServiceID");
        CalculatePaymentsService();
    }
    protected void ddlAddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculatePayments();
    }
    protected void btnAddPayment_Click(object sender, EventArgs e)
    {
        if (rblType.SelectedValue == "0")
        {
            if (Convert.ToInt32(tbAddAmmount.Text) <= Convert.ToInt32(tbRemainingCosts.Text))
            {
                //if (Convert.ToInt32(tbRemainingPayments.Text) == 1 &&
                //    Convert.ToInt32(tbRemainingCosts.Text) > Convert.ToInt32(tbAddAmmount.Text))
                //{
                //    lblInfo.Text = "It's your last payment! You have to pay all ammount of "+tbRemainingCosts.Text+" denars!";
                //    lblInfo.Visible = true;
                //}
                //else //ako e se vo red izvrsi uplata
                //{
                String GroupStudentID = Functions.ExecuteScalar("SELECT GroupStudentID FROM GroupStudent WHERE GroupID=" + ddlAddGroup.SelectedValue + " AND StudentID=" + ddlStudents.SelectedValue);
                String SQL = @"INSERT INTO Payment (PaymentNumber,Ammount,AmmountWords,AccountNumber,DateOfPayment,
                                GroupStudentID,UserId,CreatedBy) VALUES('" + tbAddPaymentNumber.Text.Replace("'", "''") +
                            "'," + tbAddAmmount.Text + ",N'" + tbAddAmmountWords.Text.Replace("'", "''") + "',N'" + tbAccountNumber.Text.Replace("'", "''") +
                            "',N'" + tbAddDateOfPayment.Text.Replace("'", "''") + "'," + GroupStudentID + "," + Session["UserID"] + "," + Session["UserID"] + "); SELECT SCOPE_IDENTITY()";
                String ID = Functions.ExecuteScalar(SQL);
                //CalculatePayments();
                Fill_Payment(ID);
                lblInfo.Text = "The payments is inserted!";
                lblInfo.Visible = true;
                btnSearch_Click(sender, e);
                //}
            }
            else
            {
                lblInfo.Text = "You are trying to insert more ammount that the remaining cost!";
                lblInfo.Visible = true;
            }
        }
        else if (rblType.SelectedValue == "1")
        {
            if (Convert.ToInt32(tbAddAmmount.Text) <= Convert.ToInt32(tbCustomerRemaining.Text))
            {
                String SQL = @"INSERT INTO Payment (PaymentNumber,Ammount,AmmountWords,AccountNumber,DateOfPayment,
                                ServiceID,UserId,CreatedBy) VALUES('" + tbAddPaymentNumber.Text.Replace("'", "''") +
                            "'," + tbAddAmmount.Text + ",N'" + tbAddAmmountWords.Text.Replace("'", "''") + "',N'" + tbAccountNumber.Text.Replace("'", "''") +
                            "',N'" + tbAddDateOfPayment.Text.Replace("'", "''") + "'," + ddlService.SelectedValue + "," + Session["UserID"] + "," + Session["UserID"] + "); SELECT SCOPE_IDENTITY()";
                String ID = Functions.ExecuteScalar(SQL);
                Fill_Payment(ID);
                lblInfo.Text = "The payments is inserted!";
                lblInfo.Visible = true;
                btnSearch_Click(sender, e);
            }
            else
            {
                lblInfo.Text = "You are trying to insert more ammount that the remaining cost!";
                lblInfo.Visible = true;
            }
        }

        else if (rblType.SelectedValue == "2")
        {
            if (Convert.ToInt32(tbAddAmmount.Text) <= Convert.ToInt32(tbInvoiceRemainingCosts.Text))
            {
                String SQL = @"INSERT INTO Payment (PaymentNumber,Ammount,AmmountWords,AccountNumber,DateOfPayment,
                                InvoiceID,UserId,CreatedBy) VALUES('" + tbAddPaymentNumber.Text.Replace("'", "''") +
                            "'," + tbAddAmmount.Text + ",N'" + tbAddAmmountWords.Text.Replace("'", "''") + "',N'" + tbAccountNumber.Text.Replace("'", "''") +
                            "',N'" + tbAddDateOfPayment.Text.Replace("'", "''") + "'," + ddlInvoiceGroup.SelectedValue + "," + Session["UserID"] + "," + Session["UserID"] + "); SELECT SCOPE_IDENTITY()";
                String ID = Functions.ExecuteScalar(SQL);
                Fill_Payment(ID);
                lblInfo.Text = "The payments is inserted!";
                lblInfo.Visible = true;
                btnSearch_Click(sender, e);
            }
            else
            {
                lblInfo.Text = "You are trying to insert more ammount that the remaining cost!";
                lblInfo.Visible = true;
            }
        }
    }
    protected void btnRecalculate_Click(object sender, EventArgs e)
    {
        CalculatePayments();
    }
    protected void gvMain_PageIndexChanged(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    protected void btnInserNewPayment_Click(object sender, EventArgs e)
    {
        rblType.Enabled = true;
        tbPaymentID.Enabled = true;
        //tbAddPaymentNumber.Enabled = true;
        tbAddAmmount.Enabled = true;
        tbAddAmmountWords.Enabled = true;
        tbAccountNumber.Enabled = true;
        tbAddDateOfPayment.Enabled = true;

        pnlButtonsAdd.Visible = true;
        pnlButtonsNew.Visible = false;
        //btnAddPayment.Visible = true;
        //btnRecalculate.Visible = true;
        //btnInserNewPayment.Visible = false;
        //btnPrintPayment.Visible = false;

        tbStudentsSearch.Enabled = true;
        ddlStudents.Enabled = true;
        ddlAddGroup.Enabled = true;

        tbPaymentID.Text = "";
        tbAddPaymentNumber.Text = "";
        tbAddAmmount.Text = "";
        tbAddAmmountWords.Text = "";
        tbAccountNumber.Text = "";
        tbAddDateOfPayment.Text = Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy");
        tbAddPaymentNumber.Text = Get_PaymentNumber();

        tbCustomerSearch.Enabled = true;
        ddlCustomers.Enabled = true;
        ddlService.Enabled = true;

        ddlInvoiceGroup.Enabled = true;
    }
    protected void btnPrintPayment_Click(object sender, EventArgs e)
    {
        PrintPayment(tbPaymentID.Text);
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        try
        {
            string Id = gvMain.SelectedValue.ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Preview payment details', 'Preview');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void tbAddAmmount_TextChanged(object sender, EventArgs e)
    {
        tbAddAmmountWords.Text = NumberToWords(Convert.ToInt32(tbAddAmmount.Text), "") + " денари";
    }
    protected void ddlService_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculatePaymentsService();
    }
    protected void ddlInvoiceGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculatePaymentsInvoice();
    }
    protected void btnRefreshInvoices_Click(object sender, ImageClickEventArgs e)
    {
        CalculatePaymentsInvoice();
    }
    protected void btnCustomersRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ddlCustomers_SelectedIndexChanged(sender, e);
        CalculatePaymentsService();
    }
    protected void btnAllStudents_Click(object sender, ImageClickEventArgs e)
    {
        ddlStudents.Items.Clear();
        Functions.FillCombo(@"SELECT StudentID, FirstName+' '+LastName as Name FROM Student", ddlStudents, "Name", "StudentID");
    }
    #endregion
}