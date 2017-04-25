#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Groups : System.Web.UI.Page
{
    #region Variables
    private static bool Edited = false;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Functions.FillCombo("SELECT '' as Language UNION SELECT DISTINCT(Language) FROM GroupType", ddlLanguage, "Language", "Language");
            Functions.FillCombo("SELECT '' as LevelDescription UNION SELECT DISTINCT(LevelDescription) FROM GroupType", ddlLevel, "LevelDescription", "LevelDescription");
            Functions.FillCombo("SELECT StudentID, FirstName+' '+LastName + ' - ' + convert(varchar(20), DateOfBirth, 104) as Name FROM Student WHERE Status=1", ddlStudents, "Name", "StudentID");
            Functions.FillCombo(@"SELECT -1 as EmployeeID,'' as Name UNION 
                                SELECT e.EmployeeID, e.FirstName+' '+e.LastName as Name 
                                FROM Employee e LEFT OUTER JOIN [User] u ON e.UserID=u.UserID LEFT OUTER JOIN UserAccess ua ON ua.UserID=u.UserID
                                WHERE e.Status=1 AND ua.UserTypeID NOT IN ("+ ConfigurationManager.AppSettings["Admin"].ToString() +
                                ","+ ConfigurationManager.AppSettings["Advanced"].ToString() + ") GROUP BY e.EmployeeID, e.FirstName, e.LastName", ddlTeacher, "Name", "EmployeeID");
            
            Functions.FillCombo(@"SELECT 1 as Value, 'Progress Test' as Description UNION " +
                                " SELECT 2 as Value, 'Final-Oral' as Description UNION SELECT 3 as Value, 'Final-Written' as Description", ddlTestType, "Description", "Value");

            if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
                Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
            {
                String Employee = Functions.ExecuteScalar("SELECT EmployeeID FROM Employee WHERE UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value));
                ddlTeacher.SelectedValue = Employee;
                ddlTeacher.Enabled = false;
            }

            Functions.FillCombo("SELECT TemplateName, TemplateFile FROM Template WHERE TemplateType=2 ORDER BY CreatedDate DESC", ddlTemplateCertificate, "TemplateName", "TemplateFile");
            Functions.FillCombo("SELECT -1 as Enable, '' as Description UNION SELECT 1 as Enable, 'Active' as Description UNION SELECT 0 as Enable, 'Finished' as Description", ddlStatus, "Description", "Enable");
            ddlStatus.SelectedValue = "1";
            Login_Redirect();
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
            btnCreateAllCertificates.Visible = true;
            btnEdit.Visible = true;
        }
        else
        {
            pnlTopMeni.Visible = false;
            pnlAddPermission.Visible = false;
            btnEdit.Visible = false;
        }

        if (PermLevel == ConfigurationManager.AppSettings["Admin"].ToString())
        {
            bthEditGrType.Visible = true;
            btnEditClassroom.Visible = true;
            btnEditTemplates.Visible = true;
            btnDeleteAllCertificates.Visible = true;
            btnRemove.Visible = true;
            btnDelete.Visible = true;
            btnDeleteTest.Visible = true;
            btnChangeDiscount.Visible = true;
            btnClearCost.Visible = true;
        }
        else
        {
            //btnEdit.Visible = false;
            btnRemove.Visible = false;
            btnCreateAllCertificates.Visible = false;
        }
    }
    public void FillDataGrid(String WherePart)
    {
        dsMain.SelectCommand = @"SELECT g.*,gt.*,g.GroupID as ID, CASE WHEN Status=1 THEN 'Active' WHEN Status=0 THEN 'Finished' END as StatusDesc, 
                               (SELECT COUNT(*) FROM GroupStudent as gs LEFT OUTER JOIN Student s ON s.StudentID=gs.StudentID WHERE gs.GroupID=g.GroupID AND gs.Transfered=0 AND s.Status=1 )as StudentsNo
                               FROM [Group] as g LEFT OUTER JOIN GroupType as gt ON gt.GroupTypeID=g.GroupTypeID " + WherePart + " ORDER BY g.GroupID DESC";
    }
    private string FillWherePart()
    {
        String WherePart = "";

        WherePart = Functions.VratiWherePartEqual(tbGroupName, "g.GroupName", WherePart);
        WherePart = Functions.VratiWherePartDDL(ddlLanguage, "gt.Language", WherePart);
        WherePart = Functions.VratiWherePartDDL(ddlLevel, "gt.LevelDescription", WherePart);
        WherePart = Functions.VratiWherePartDDL_ID(ddlTeacher, "g.EmployeeID", WherePart);
        WherePart = Functions.VratiWherePartDDL(ddlStatus, "g.Status", WherePart);

        if (WherePart.Length > 0) WherePart = " WHERE " + WherePart;
        return WherePart;
    }
    private void Fill_Details()
    {
        dsDetails.SelectCommand = @"SELECT s.StudentID as ID, s.FirstName + ' ' + s.LastName as Name,gs.Status, gs.Discount, gs.TotalCost, SUM(CASE WHEN p.Transfered=0 THEN p.Ammount END) as TotalPaid, CASE WHEN gs.TotalCost - SUM(CASE WHEN p.Transfered=0 THEN p.Ammount END) <0 THEN 0 
                                ELSE gs.TotalCost - SUM(CASE WHEN p.Transfered=0 THEN p.Ammount END) END as TotalRemain, gs.Transfered
                                FROM GroupStudent as gs LEFT OUTER JOIN Student as s ON s.StudentID=gs.StudentID LEFT OUTER JOIN Payment p ON p.GroupStudentID=gs.GroupStudentID
                                WHERE gs.GroupID=" + gvMain.SelectedValue.ToString() + " AND s.Status=1 GROUP BY s.StudentID , s.FirstName + ' ' + s.LastName,gs.Status, gs.Discount, gs.TotalCost,gs.Transfered";
        gvDetails.DataBind();
    }
    private void FillClasses()
    {
        if (gvDetails.SelectedIndex != null)
        {
            String[] Classes = Functions.ReturnIntoArray("SELECT ClassesAttended,PassedFinalTest,ReceivedCertificate FROM GroupStudent WHERE StudentID=" + gvDetails.SelectedValue +" AND GroupID="+gvMain.SelectedValue, 3);
            if (Classes.Length > 0)
            {
                tbClassesAttended.Text = Classes[0];
                if (Classes[1] != "")
                    cbPassedFinalTest.Checked = Convert.ToBoolean(Classes[1]);
                if (Classes[2] != "")
                    cbReceivedCertificate.Checked = Convert.ToBoolean(Classes[2]);
            }
        }
    }
    protected void Fill_Payments()
    {
        dsPayments.SelectCommand = @"SELECT * FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                                    WHERE gs.GroupID='" + gvMain.SelectedValue + "' AND gs.StudentID='" + gvDetails.SelectedValue + "'";
        gvPayments.DataBind();

        String Invoice = Functions.ExecuteScalar("SELECT Invoice FROM [Group] WHERE GroupID=" + gvMain.SelectedValue);
        Boolean ExistInvoice = false;
        if (Invoice != "") ExistInvoice = Convert.ToBoolean(Invoice);

        if (ExistInvoice)
        {
            lblNoPayments.Text = "This group is paid with invoice!";
            lblNoPayments.Visible = true;
        }
        else
        {
            lblNoPayments.Visible = false;
            CalculatePayments();
        }
    }
    protected void CalculatePayments()
    {
        String SQL = @"SELECT COUNT(p.PaymentID) as NumberOfPayments, g.NumberOfPayments-COUNT(p.PaymentID) as RemainPayments,
                    (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalPaid, gs.TotalCost - (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalRemain
                    FROM GroupStudent gs LEFT OUTER JOIN Payment p ON p.GroupStudentID=gs.GroupStudentID
                    LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                    WHERE p.Transfered=0 AND gs.GroupID='" + gvMain.SelectedValue + "' AND gs.StudentID='" + gvDetails.SelectedValue + "' " +//gs.GroupStudentID=" + gvDetails.SelectedValue +
                    "GROUP BY g.NumberOfPayments,gs.TotalCost,gs.Discount";
        string[] Payments = Functions.ReturnIntoArray(SQL, 4);

        if (Payments.Count() > 0)
        {
            tbNumberOfPayments.Text = Payments[0];
            tbRemainingPayments.Text = Payments[1];
            tbTotalPaid.Text = Payments[2];
            tbRemainingCosts.Text = Payments[3];

            String IndividualGroup = Functions.ExecuteScalar("SELECT IndividualGroup FROM [Group] WHERE GroupID=" + gvMain.SelectedValue);
            if (Convert.ToBoolean(IndividualGroup))
                tbRemainingCosts.Text = "0";

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
    protected void Fill_Certificates(String GroupID = "")
    {
        if (gvMain.SelectedRow != null)
        {
            dsCertificates.SelectCommand = @"SELECT c.*, s.FirstName + ' ' + s.LastName as Student
                            FROM [Certificate] c LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN Student s ON s.StudentID=c.StudentID WHERE c.GroupID=" + gvMain.SelectedValue;

            if (GroupID != "")
            {
                dsCertificates.SelectCommand += " AND c.GroupID=" + GroupID;
            }
            gvCertificates.DataBind();
        }
    }
    protected void Fill_Tests()
    {
        if (gvMain.SelectedRow != null)
        {
            dsUsedTests.SelectCommand = @"SELECT t.*, u.FirstName + ' ' + u.LastName as CreatedByUser,
                            CASE WHEN t.TestType=1 THEN 'Progress Test' WHEN t.TestType=2 THEN 'Final-Oral' WHEN t.TestType=3 THEN 'Final-Written' END as TypeOpis
                            FROM [Test] t LEFT OUTER JOIN [Group] g ON g.GroupID=t.GroupID 
                            LEFT OUTER JOIN [User] u ON u.UserID=t.CreatedBy
                            WHERE t.GroupID=" + gvMain.SelectedValue;
            gvUsedTests.DataBind();
            lblInfo.Visible = false;
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
                if (gvMain.SelectedValue != null)
                {
                    string Id = gvMain.SelectedValue.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Edit Group','');", true);
                    Edited = true;
                }
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
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage('', 'Insert Group','');", true);
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
            String SQL = @"DELETE FROM [Certificate] WHERE GroupID=" + gvMain.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            SQL = @"DELETE FROM [Termin] WHERE GroupID=" + gvMain.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            SQL = @"DELETE FROM GroupStudent WHERE GroupID=" + gvMain.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            SQL = @"DELETE FROM [Test] WHERE GroupID=" + gvMain.SelectedValue.ToString();
            Functions.ExecuteCommand(SQL);
            SQL = @"DELETE FROM [Group] WHERE GroupID=" + gvMain.SelectedValue.ToString();
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
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            Int32 SelectedIndex = gvMain.SelectedIndex;
            Int32 Exists = Convert.ToInt32(Functions.ExecuteScalar("SELECT COUNT(*) FROM GroupStudent WHERE GroupID=" + gvMain.SelectedValue + " AND StudentID=" + ddlStudents.SelectedValue));
            String IndividualGroup = Functions.ExecuteScalar("SELECT IndividualGroup FROM [Group] WHERE GroupID=" + gvMain.SelectedValue);

            if (Exists == 0)
            {
                string SQL = "";
                if (!Convert.ToBoolean(IndividualGroup))
                {
                    if (tbDiscount.Text == "") tbDiscount.Text = "0";
                    decimal TotalCost = Convert.ToDecimal(Functions.ExecuteScalar("SELECT Cost FROM [Group] WHERE GroupID=" + gvMain.SelectedValue));
                    if (Convert.ToInt32(tbDiscount.Text) > 0)
                        TotalCost = TotalCost - (TotalCost * Convert.ToInt32(tbDiscount.Text) / 100);
                    SQL = "INSERT INTO GroupStudent (GroupID, StudentID, Status, Discount, TotalCost, CreatedBy) VALUES (" +
                    gvMain.SelectedValue + "," + ddlStudents.SelectedValue + ",0," + tbDiscount.Text.Replace("'", "''") + "," + TotalCost.ToString().Replace(",", ".") + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
                }
                else if (Convert.ToBoolean(IndividualGroup))
                {
                    SQL = "INSERT INTO GroupStudent (GroupID, StudentID, Status, Discount, TotalCost, CreatedBy) VALUES (" +
                        gvMain.SelectedValue + "," + ddlStudents.SelectedValue + ",0,0,0," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
                }
                Functions.ExecuteCommand(SQL);
                btnSearch_Click(sender, e);
                gvMain.SelectedIndex = SelectedIndex;
                Fill_Details();
            }
            else
                lblAlreadyAdded.Visible = true;
        }
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedRow != null)
        {
            Int32 SelectedIndex = gvMain.SelectedIndex;
            String SQL = "DELETE FROM [Payment] WHERE GroupStudentID IN (SELECT g.GroupStudentID FROM GroupStudent g WHERE g.StudentID=" + gvDetails.SelectedValue + " AND g.GroupID=" + gvMain.SelectedValue+")";
            Functions.ExecuteCommand(SQL);
            SQL = "DELETE FROM GroupStudent WHERE StudentId=" + gvDetails.SelectedValue + " AND GroupID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            btnSearch_Click(sender, e);
            gvMain.SelectedIndex = SelectedIndex;
            Fill_Details();
        }
    }
    protected void bthEditGrType_Click(object sender, EventArgs e)
    {
        //if (!Edited)
        // {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowGrTypes('Group Types','');", true);
            //Edited = true;
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
        //}
        //btnSearch_Click(sender, e);

    }
    protected void btnAddTerm_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowTerm('Group Term dates'," + gvMain.SelectedValue + ",'');", true);
                //Edited = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
                if (HttpContext.Current != null)
                    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            }
        }
    }
    protected void btnTermsDetails_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowTerm('Group Term dates'," + gvMain.SelectedValue + ",'Preview');", true);
                //Edited = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
                if (HttpContext.Current != null)
                    HttpContext.Current.Response.Redirect(@"~\Error.aspx");
            }
        }

    }
    protected void btnPrintPayment_Click(object sender, EventArgs e)
    {
        if (gvPayments.SelectedRow != null)
        {
            string SQLPrint = @"SELECT p.AccountNumber,p.Ammount, p.AmmountWords, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as PaymentGroup,
                        s.FirstName+' '+s.LastName as PaymentName, p.PaymentNumber, s.Place as PaymentPlace,
                        convert(varchar,p.DateOfPayment,104) as DateOfPayment, gs.TotalCost, 
						CASE WHEN gs.TotalCost-(SELECT SUM(p2.Ammount)						
						FROM Payment p2 where p2.GroupStudentID=gs.GroupStudentID AND p2.DateOfPayment<=p.DateOfPayment) < 0 THEN 0
						ELSE gs.TotalCost-(SELECT SUM(p2.Ammount) FROM Payment p2 where p2.GroupStudentID=gs.GroupStudentID AND p2.DateOfPayment<=p.DateOfPayment) END as RemainingCost,
						(SELECT COUNT(*) FROM Payment p2 where p2.GroupStudentID=gs.GroupStudentID AND p2.DateOfPayment<=p.DateOfPayment) as NoPayments, g.NumberOfPayments as TotalNoPayment
                        FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                        LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                        LEFT OUTER JOIN Student s ON s.StudentID=gs.StudentID WHERE p.PaymentID=" + gvPayments.SelectedValue;

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
    protected void btnEditClassroom_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowClassRoom('Classrooms','','');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void btnPrintSelectedCertificate_Click(object sender, EventArgs e)
    {
        if (gvCertificates.SelectedRow != null)
        {
            String Text_1 = "g.NumberOfClasses";
            String Text_2 = "";
            String IndividualGroup = Functions.ExecuteScalar("SELECT IndividualGroup FROM [Group] WHERE GroupID=" + gvMain.SelectedValue);
            if (Convert.ToBoolean(IndividualGroup))
            {
                Text_1 = "gs.ClassesAttended as NumberOfClasses";
                Text_2 = "LEFT OUTER JOIN GroupStudent gs ON gs.GroupID=c.GroupID AND gs.StudentID=c.StudentID";
            }

            string SQLPrint = @"SELECT c.RegNo, s.FirstName + ' ' + s.LastName as StudentName, convert(varchar,s.DateOfBirth,104) as DateOfBirth,
                            s.Place, gt.Language, gt.LevelDescription, gt.Level, gt.Program, "+Text_1+@", 
							convert(varchar,g.StartDate,104) as StartDate, convert(varchar,g.EndDate,104) as EndDate, 
							convert(varchar,g.EndDate,104) as DateOfPrint, e.FirstName + ' ' + e.LastName as Teacher
                            FROM [Certificate] c LEFT OUTER JOIN Student s ON s.StudentID=c.StudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
							LEFT OUTER JOIN Employee e ON e.EmployeeID = g.EmployeeID " + Text_2 + @" 
                            WHERE c.CertificateID=" + gvCertificates.SelectedValue;            

            String[] Detali = Functions.ReturnIntoArray(SQLPrint, 13);
            String StudentName = Detali[1];
            String Place = Detali[3];
            String Language = Detali[4];
            String Teacher = Detali[12];
            StudentName = Functions.ReturnLatin(StudentName);
            Place = Functions.ReturnLatin(Place);
            Language = Functions.ReturnLatin(Language);
            Teacher = Functions.ReturnLatin(Teacher);

            SQLPrint = @"SELECT c.RegNo, '" + StudentName + @"' as StudentName, convert(varchar,s.DateOfBirth,104) as DateOfBirth,
                            '" + Place + @"' as Place, '" + Language + @"' as Language, gt.LevelDescription, gt.Level, gt.Program, "+Text_1+@", 
							convert(varchar,g.StartDate,104) as StartDate, convert(varchar,g.EndDate,104) as EndDate, 
							convert(varchar,g.EndDate,104) as DateOfPrint, '" + Teacher + @"' as Teacher,
                            (CASE WHEN s.Gender = 1 THEN '' ELSE 'a' END) as Gender
                            FROM [Certificate] c LEFT OUTER JOIN Student s ON s.StudentID=c.StudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
							LEFT OUTER JOIN Employee e ON e.EmployeeID = g.EmployeeID " + Text_2 + @"
                            WHERE c.CertificateID=" + gvCertificates.SelectedValue;

            //String PathDoc = Server.MapPath("~/Templates/Certificate2016");
            String PathDoc = Server.MapPath(ddlTemplateCertificate.SelectedValue.Replace(".dotx", ""));
            Functions.PrintWord(PathDoc, SQLPrint);
            PrintPDF(PathDoc);
        }

    }
    protected void btnPrintAllCertificates_Click(object sender, EventArgs e)
    {
        int br = 1;
        foreach (GridViewRow gr in gvCertificates.Rows)
        {
            if (gr.RowIndex > -1)
            {
                gvCertificates.SelectRow(gr.RowIndex);

                String Text_1 = "g.NumberOfClasses";
                String Text_2 = "";
                String IndividualGroup = Functions.ExecuteScalar("SELECT IndividualGroup FROM [Group] WHERE GroupID=" + gvMain.SelectedValue);
                if (Convert.ToBoolean(IndividualGroup))
                {
                    Text_1 = "gs.ClassesAttended as NumberOfClasses";
                    Text_2 = "LEFT OUTER JOIN GroupStudent gs ON gs.GroupID=c.GroupID AND gs.StudentID=c.StudentID";
                }


                string SQLPrint = @"SELECT c.RegNo, s.FirstName + ' ' + s.LastName as StudentName, convert(varchar,s.DateOfBirth,104) as DateOfBirth,
                            s.Place, gt.Language, gt.LevelDescription, gt.Level, gt.Program, "+Text_1+@", 
							convert(varchar,g.StartDate,104) as StartDate, convert(varchar,g.EndDate,104) as EndDate, 
							convert(varchar,g.EndDate,104) as DateOfPrint, e.FirstName + ' ' + e.LastName as Teacher
                            FROM [Certificate] c LEFT OUTER JOIN Student s ON s.StudentID=c.StudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
							LEFT OUTER JOIN Employee e ON e.EmployeeID = g.EmployeeID " + Text_2 + @"
                            WHERE c.CertificateID=" + gvCertificates.SelectedValue;

                String[] Detali = Functions.ReturnIntoArray(SQLPrint, 13);
                String StudentName = Detali[1];
                String Place = Detali[3];
                String Language = Detali[4];
                String Teacher = Detali[12];
                StudentName = Functions.ReturnLatin(StudentName);
                Place = Functions.ReturnLatin(Place);
                Language = Functions.ReturnLatin(Language);
                Teacher = Functions.ReturnLatin(Teacher);

                SQLPrint = @"SELECT c.RegNo, '" + StudentName + @"' as StudentName, convert(varchar,s.DateOfBirth,104) as DateOfBirth,
                            '" + Place + @"' as Place, '" + Language + @"' as Language, gt.LevelDescription, gt.Level, gt.Program, "+Text_1+@", 
							convert(varchar,g.StartDate,104) as StartDate, convert(varchar,g.EndDate,104) as EndDate, 
							convert(varchar,g.EndDate,104) as DateOfPrint, '" + Teacher + @"' as Teacher,
                            (CASE WHEN s.Gender = 1 THEN '' ELSE 'a' END) as Gender
                            FROM [Certificate] c LEFT OUTER JOIN Student s ON s.StudentID=c.StudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
							LEFT OUTER JOIN Employee e ON e.EmployeeID = g.EmployeeID " + Text_2 + @"
                            WHERE c.CertificateID=" + gvCertificates.SelectedValue;

                //String PathDoc = Server.MapPath("~/Templates/");
                String PathDoc = Server.MapPath(ddlTemplateCertificate.SelectedValue);
                Functions.PrintWordMulti(PathDoc, Server.MapPath("~/Templates/Multi/"), SQLPrint, br);
                br++;
            }
        }

        String PathExport = Server.MapPath("~/Templates/MultiExport.docx");
        String PathCurrent = Server.MapPath("~/Templates/Multi/");

        for (int i = 1; i < br; i++)
        {
            if (i == 1)
                Functions.MergeDocs(PathExport, PathCurrent + i.ToString() + ".docx", true, false);
            else if (i == br - 1)
                Functions.MergeDocs(PathExport, PathCurrent + i.ToString() + ".docx", false, true);
            else
                Functions.MergeDocs(PathExport, PathCurrent + i.ToString() + ".docx", false, false);
        }
        PrintPDF(PathExport.Replace(".docx", ""));
    }
    protected void btnCreateAllCertificates_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"INSERT INTO [Certificate] (StudentID,GroupID, CreatedBy) 
                            SELECT StudentID, GroupID, " + Functions.Decrypt(Request.Cookies["UserID"].Value) + " FROM GroupStudent gs WHERE GroupID=" + gvMain.SelectedValue +
                            " AND gs.Transfered=0 AND StudentID NOT IN (SELECT StudentID FROM [Certificate] WHERE GroupId=gs.GroupID AND StudentID=gs.StudentID)";

            Functions.ExecuteCommand(SQL);
            Fill_Certificates();
        }
    }
    protected void btnDeleteAllCertificates_Click(object sender, EventArgs e)
    {
        if (gvMain.SelectedRow != null)
        {
            String SQL = @"DELETE FROM [Certificate] WHERE GroupID=" + gvMain.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Certificates();
        }
    }
    protected void btnEditTemplates_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowTemplates('Edit Templates');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void imgbtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Certificates();
    }
    protected void lnkDoubleClick_Click(object sender, EventArgs e)
    {
        try
        {
            string Id = gvMain.SelectedValue.ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Preview group details', 'Preview');", true);
            Edited = true;
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void tbStudentsSearch_TextChanged(object sender, EventArgs e)
    {
        ddlStudents.Items.Clear();
        Functions.FillCombo(@"SELECT StudentID, FirstName+' '+LastName + ' - ' + convert(varchar(20), DateOfBirth, 104) as Name FROM Student WHERE Status=1 AND
                            (FirstName LIKE N'%" + tbStudentsSearch.Text.Replace("'", "''") + "%' OR LastName LIKE N'%" + tbStudentsSearch.Text.Replace("'", "''") + "%' OR FirstName + ' ' + LastName LIKE N'%" + tbStudentsSearch.Text.Replace("'", "''") + "%' OR LastName + ' ' + FirstName LIKE N'%" + tbStudentsSearch.Text.Replace("'", "''") + "%')", ddlStudents, "Name", "StudentID");
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvDetails, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }
    protected void gvDetails_Sorted(object sender, EventArgs e)
    {
        Fill_Details();
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
                lblAlreadyAdded.Visible = false;
                row.ToolTip = string.Empty;
                Fill_Details();
                Fill_Certificates(gvMain.SelectedValue.ToString());
                Fill_Tests();
                pnlAddPermission.Visible = true;
                Login_Redirect();
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
    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPayments, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvPayments_Sorted(object sender, EventArgs e)
    {
        Fill_Payments();
    }
    protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Payments();
        FillClasses();
    }
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
    protected void gvMain_PageIndexChanged(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    protected void gvDetails_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Details();
    }
    #endregion
    protected void btnSaveAdd_Click(object sender, EventArgs e)
    {
        if((gvDetails.SelectedIndex != null) && (gvMain.SelectedIndex!=null))
        {
            Functions.ExecuteCommand(@"UPDATE GroupStudent SET ClassesAttended=N'" + tbClassesAttended.Text +
            @"', PassedFinalTest='" + cbPassedFinalTest.Checked + "', ReceivedCertificate='" + cbReceivedCertificate.Checked +
            @"' WHERE StudentID=" + gvDetails.SelectedValue + " AND GroupID=" + gvMain.SelectedValue);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvMain.SelectedRow != null)
            {
                string fileName = Path.GetFileName(fuFile.PostedFile.FileName);
                if (fileName != "")
                {
                    fuFile.PostedFile.SaveAs(Server.MapPath("~/Templates/Templates/") + gvMain.SelectedValue + "-" + fileName);
                    String SQL = "INSERT INTO Test (TestName,TestFile,TestType,GroupID,CreatedBy) VALUES (N'" + fileName +
                                "','~/Templates/Templates/" + gvMain.SelectedValue + "-" + fileName + "'," + ddlTestType.SelectedValue + "," + gvMain.SelectedValue + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
                    Functions.ExecuteCommand(SQL);
                    Fill_Tests();
                    lblInfo.Text = "The test is Uploaded!";
                    lblInfo.Visible = true;
                }
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
        if (gvUsedTests.SelectedRow != null && gvMain.SelectedRow != null)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            String Path = "~/Templates/Templates/" + gvMain.SelectedValue + "-" + gvUsedTests.SelectedRow.Cells[1].Text.ToString();
            string[] FileName = Path.Split('/');
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + FileName[3] + ";");
            response.TransmitFile(Server.MapPath(Path));
            response.Flush();
            response.End();
        }
    }
    protected void btnEditChanges_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvMain.SelectedRow != null && gvUsedTests.SelectedRow != null)
            {
                String SQL = "";
                string fileName = Path.GetFileName(fuFile.PostedFile.FileName);
                if (fileName != "")
                {
                    fuFile.PostedFile.SaveAs(Server.MapPath("~/Templates/Templates/") + gvMain.SelectedValue + "-" + fileName);

                    SQL = "UPDATE Template SET TemplateName=N'" + fileName +
                            "',TemplateFile='~/Templates/Templates/" + gvMain.SelectedValue + "-" + fileName + "',TemplateType=" + ddlTestType.SelectedValue + ", CreatedDate=GetDate() WHERE TestID=" + gvUsedTests.SelectedValue;
                }
                else
                {
                    SQL = "UPDATE Template SET TemplateName=N'" + fileName + "',TemplateType=" + ddlTestType.SelectedValue + ", CreatedDate=GetDate() WHERE TestID=" + gvUsedTests.SelectedValue;
                }

                Functions.ExecuteCommand(SQL);
                Fill_Tests();
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
    protected void gvUsedTests_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUsedTests, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }
    protected void btnDeleteTest_Click(object sender, EventArgs e)
    {
        if (gvUsedTests.SelectedRow != null)
        {
            String SQL = @"DELETE FROM [Test] WHERE TestID=" + gvUsedTests.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Tests();
            lblInfo.Text = "The test is deleted!";
            lblInfo.Visible = true;
        }
    }
    protected void gvUsedTests_Sorted(object sender, EventArgs e)
    {
        Fill_Tests();
    }
    protected void btnChangeDiscount_Click(object sender, EventArgs e)
    {
        if ((gvDetails.SelectedIndex != null) && (gvMain.SelectedIndex != null))
        {
            Int32 SelectedIndex = gvMain.SelectedIndex;

            if (tbDiscount.Text == "") tbDiscount.Text = "0";
            decimal TotalCost = Convert.ToDecimal(Functions.ExecuteScalar("SELECT Cost FROM [Group] WHERE GroupID=" + gvMain.SelectedValue));
            if (Convert.ToInt32(tbDiscount.Text) > 0)
                TotalCost = TotalCost - (TotalCost * Convert.ToInt32(tbDiscount.Text) / 100);

            Functions.ExecuteCommand(@"UPDATE GroupStudent SET TotalCost=N'" + TotalCost.ToString().Replace(",", ".") + "', Discount=N'" + tbDiscount.Text +
            @"' WHERE StudentID=" + gvDetails.SelectedValue + " AND GroupID=" + gvMain.SelectedValue);

            btnSearch_Click(sender, e);
            gvMain.SelectedIndex = SelectedIndex;
            Fill_Details();
        }
    }
    protected void btnClearCost_Click(object sender, EventArgs e)
    {
        if ((gvDetails.SelectedIndex != null) && (gvMain.SelectedIndex != null))
        {
            Int32 SelectedIndex = gvMain.SelectedIndex;
            decimal TotalPaid = 0;
            if (gvDetails.SelectedRow.Cells[5].Text!="")  TotalPaid = Convert.ToDecimal(gvDetails.SelectedRow.Cells[5].Text.Replace("ден",""));

            Functions.ExecuteCommand(@"UPDATE GroupStudent SET TotalCost=N'" + TotalPaid.ToString().Replace(",", ".") + 
            @"' WHERE StudentID=" + gvDetails.SelectedValue + " AND GroupID=" + gvMain.SelectedValue);

            btnSearch_Click(sender, e);
            gvMain.SelectedIndex = SelectedIndex;
            Fill_Details();
            Fill_Payments();
        }
    }
}