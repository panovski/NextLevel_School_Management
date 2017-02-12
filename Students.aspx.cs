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

public partial class Students : System.Web.UI.Page
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
            Functions.FillCombo("SELECT -1 as Status, ' ' as Description UNION SELECT 1 as Status, 'Active' as Description UNION SELECT 0 as Status, 'Deactivated' as Description", ddlStatus, "Description", "Status");
            Functions.FillCombo("SELECT TemplateName, TemplateFile FROM Template WHERE TemplateType=2 ORDER BY CreatedDate DESC", ddlTemplateCertificate, "TemplateName", "TemplateFile");

            if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
                Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
            {
                Functions.FillCombo("SELECT g.GroupID, g.GroupName + ' - ' + gt.Language + ' - ' + gt.LevelDescription as Course FROM[Group] as g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID = g.GroupTypeID LEFT OUTER JOIN Employee e ON e.EmployeeID=g.EmployeeID WHERE g.Status=1 AND e.UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value), ddlCourse, "Course", "GroupID");
            }
            else
                Functions.FillCombo("SELECT g.GroupID, g.GroupName + ' - ' + gt.Language + ' - ' + gt.LevelDescription as Course FROM[Group] as g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID = g.GroupTypeID WHERE g.Status=1", ddlCourse, "Course", "GroupID");
                        
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
            pnlAddPermission.Visible = false;
            btnPrintPayment.Visible = false;
            btnTransfer.Visible = false;
        }

        if (PermLevel == ConfigurationManager.AppSettings["Admin"].ToString())
        {
            btnRemove.Visible = true;
            btnEdit.Visible = true;
        }
        else
        {
            btnRemove.Visible = false;
            btnEdit.Visible = false;
        }
    }
    public void FillDataGrid(String WherePart)
    {
        dsStudents.SelectCommand = @"SELECT *, CASE WHEN Status=1 THEN 'Active' WHEN Status=0 THEN 'Deactivated' END as StatusDesc FROM Student " + WherePart + " ORDER BY LastName";
    }
    private string FillWherePart()
    {
        String WherePart = "";

        WherePart = Functions.VratiWherePart(tbSSN, "SocialNumber", WherePart);
        WherePart = Functions.VratiWherePart(tbFirstName, "FirstName", WherePart);
        WherePart = Functions.VratiWherePart(tbLastName, "LastName", WherePart);
        WherePart = Functions.VratiWherePartDDL(ddlStatus, "Status", WherePart);

        String WhereEmployee = "";
        if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
                Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
        {
            WhereEmployee = @" AND StudentID IN 
                        (SELECT gs.StudentID FROM GroupStudent gs 
                        LEFT OUTER JOIN [Group] g ON g.GroupID = gs.GroupID
                        LEFT OUTER JOIN Employee e ON e.EmployeeID=g.EmployeeID
                        WHERE e.UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value)+ ") ";
        }

        if (WherePart.Length > 0)
            WherePart = " WHERE " + WherePart + WhereEmployee;
        else if (WhereEmployee.Length>0)
            WherePart = " WHERE " + WhereEmployee.Replace(" AND ","");

        return WherePart;
    }
    private void Fill_Details()
    {
        if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Edit"].ToString() ||
               Functions.Decrypt(Request.Cookies["PermLevel"].Value) == ConfigurationManager.AppSettings["Readonly"].ToString())
        {
            dsDetails.SelectCommand = @"SELECT gs.GroupStudentID as ID, g.GroupName + ' - ' + gt.Language+' - '+gt.LevelDescription as Grupa, gs.Status, gs.Discount, gs.TotalCost, g.GroupID, gs.Transfered
                                    FROM Student as s LEFT OUTER JOIN GroupStudent as gs ON gs.StudentID=s.StudentID 
                                    LEFT OUTER JOIN [Group] as g on g.GroupID=gs.GroupID
                                    LEFT OUTER JOIN GroupType as gt ON gt.GroupTypeID=g.GroupTypeID
                                    LEFT OUTER JOIN Employee e ON e.EmployeeID=g.EmployeeID
                                    WHERE s.StudentID=" + gvStudents.SelectedValue.ToString() + " AND e.UserID=" + Functions.Decrypt(Request.Cookies["UserID"].Value);
        }
        else
        {
            dsDetails.SelectCommand = @"SELECT gs.GroupStudentID as ID, g.GroupName + ' - ' + gt.Language+' - '+gt.LevelDescription as Grupa, gs.Status, gs.Discount, gs.TotalCost, g.GroupID, gs.Transfered
                                    FROM Student as s LEFT OUTER JOIN GroupStudent as gs ON gs.StudentID=s.StudentID 
                                    LEFT OUTER JOIN [Group] as g on g.GroupID=gs.GroupID
                                    LEFT OUTER JOIN GroupType as gt ON gt.GroupTypeID=g.GroupTypeID
                                    WHERE s.StudentID=" + gvStudents.SelectedValue.ToString() ;
        }

        gvDetails.DataBind();
        if (gvDetails.Rows.Count > 0)
        {
            lblNoDetails.Visible = false;
        }
        else
        {
            lblNoDetails.Visible = true;
        }

        dsPayments.SelectCommand = "";
        gvPayments.DataBind();

        Fill_Payments();
        FillClasses();
    }
    private void FillClasses()
    {
        if (gvDetails.SelectedIndex != null)
        {
            String[] Classes = Functions.ReturnIntoArray("SELECT ClassesAttended,PassedFinalTest,ReceivedCertificate FROM GroupStudent WHERE GroupStudentID=" + gvDetails.SelectedValue, 3);
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
                                    WHERE gs.GroupStudentID='" + gvDetails.SelectedValue + "'";
        gvPayments.DataBind();

        String Invoice = Functions.ExecuteScalar("SELECT g.Invoice FROM GroupStudent gs LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID WHERE gs.GroupStudentID=" + gvDetails.SelectedValue);
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
    protected void Fill_Certificates(String GroupID = "")
    {
        if (gvStudents.SelectedRow != null)
        {
            dsCertificates.SelectCommand = @"SELECT c.*, g.GroupName + ' ' + gt.Language + ' ' + gt.LevelDescription as Course
                            FROM [Certificate] c LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID WHERE c.StudentID=" + gvStudents.SelectedValue;// +
            //" AND c.GroupID="+gvDetails.SelectedValue;
            if (GroupID != "")
            {
                dsCertificates.SelectCommand += " AND c.GroupID=" + GroupID;
            }
            gvCertificates.DataBind();
        }
    }
    protected void Fill_Contracts(String GroupID = "")
    {
        if (gvStudents.SelectedRow != null)
        {
            //dsContracts.SelectCommand = @"SELECT ContractID, StartDate, EndDate FROM [Contract] WHERE StudentID=" + gvStudents.SelectedValue;
            dsContracts.SelectCommand = @"SELECT c.*, g.GroupName + '-' + gt.Language+'-'+gt.Level as Course
                            FROM [Contract] c LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=c.GroupStudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                            WHERE gs.StudentID=" + gvStudents.SelectedValue;

             if (GroupID != "")
            {
                dsContracts.SelectCommand += " AND g.GroupID=" + GroupID;
            }
            
            gvContracts.DataBind();
        }
    }
    protected void CalculatePayments()
    {
        String SQL = @"SELECT COUNT(p.PaymentID) as NumberOfPayments, g.NumberOfPayments-COUNT(p.PaymentID) as RemainPayments,
                    (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalPaid, gs.TotalCost - (SELECT CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END) as TotalRemain
                    FROM GroupStudent gs LEFT OUTER JOIN Payment p ON p.GroupStudentID=gs.GroupStudentID
                    LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                    WHERE p.Transfered=0 AND gs.GroupStudentID=" + gvDetails.SelectedValue +
                    " GROUP BY g.NumberOfPayments,gs.TotalCost";
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        String WherePart = FillWherePart();
        FillDataGrid(WherePart);
        gvStudents.DataBind();
        if (gvStudents.Rows.Count < 1)
            lblNoRows.Visible = true;
        else
            lblNoRows.Visible = false;

        gvStudents.SelectedIndex = -1;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (!Edited)
        {
            try
            {
                string Id = gvStudents.SelectedValue.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Edit Student','');", true);
                Edited = true;
            }
            catch { }
        }
        btnSearch_Click(sender, e);
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage('', 'Insert Student','');", true);
        }
        catch { }

        btnSearch_Click(sender, e);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvStudents.SelectedRow != null)
        {
            //String SQL = @"UPDATE Employee SET UserID=null WHERE UserID=" + gvStudents.SelectedValue.ToString();
            //Functions.ExecuteCommand(SQL);
            //SQL = @"DELETE FROM [UserAccess] WHERE UserID=" + gvStudents.SelectedValue.ToString();
            //Functions.ExecuteCommand(SQL);
            //SQL = @"DELETE FROM [User] WHERE UserID=" + gvStudents.SelectedValue.ToString();
            //Functions.ExecuteCommand(SQL);
            //btnSearch_Click(sender, e);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (gvStudents.SelectedRow != null)
        {
            Int32 Exists = Convert.ToInt32(Functions.ExecuteScalar("SELECT COUNT(*) FROM GroupStudent WHERE GroupID=" + ddlCourse.SelectedValue + " AND StudentID=" + gvStudents.SelectedValue.ToString()));

            if (Exists == 0)
            {

                if (tbDiscount.Text == "") tbDiscount.Text = "0";
                decimal TotalCost = Convert.ToDecimal(Functions.ExecuteScalar("SELECT Cost FROM [Group] WHERE GroupID=" + ddlCourse.SelectedValue));
                if (Convert.ToInt32(tbDiscount.Text) > 0)
                    TotalCost = TotalCost - (TotalCost * Convert.ToInt32(tbDiscount.Text) / 100);
                String SQL = "INSERT INTO GroupStudent (GroupID, StudentID, Status, Discount, TotalCost, CreatedBy) VALUES (" +
                ddlCourse.SelectedValue + "," + gvStudents.SelectedValue.ToString() + ",0," + tbDiscount.Text.Replace("'", "''") + "," + TotalCost.ToString().Replace(",", ".") + "," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
                Functions.ExecuteCommand(SQL);
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
            String SQL = "DELETE FROM GroupStudent WHERE GroupStudentID =" + gvDetails.SelectedValue + " AND StudentID=" + gvStudents.SelectedValue;
            Functions.ExecuteCommand(SQL);
            Fill_Details();
        }
    }
    protected void btnDeactivate_Click(object sender, EventArgs e)
    {

    }
    protected void btnActivate_Click(object sender, EventArgs e)
    {

    }
    protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvStudents, "Select$" + e.Row.RowIndex);

            LinkButton _doubleClickButton = (LinkButton)lnkDoubleClick;
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            e.Row.Attributes["ondblclick"] = _jsDouble;
        }
    }
    protected void gvStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        Edited = false;
        foreach (GridViewRow row in gvStudents.Rows)
        {
            if (row.RowIndex == gvStudents.SelectedIndex)
            {
                tbClassesAttended.Text = "";
                cbReceivedCertificate.Checked = false;
                cbPassedFinalTest.Checked = false;
                lblAlreadyAdded.Visible = false;
                row.ToolTip = string.Empty;
                Fill_Details();
                pnlAddPermission.Visible = true;
                Login_Redirect();
                //Fill_Certificates();
                //Fill_Contracts();

                if (gvStudents.SelectedRow.Cells[5].Text == "Active")
                {
                    //btnDeactivate.Visible = true;
                    //btnActivate.Visible = false;
                }
                else
                {
                    //btnDeactivate.Visible = false;
                    //btnActivate.Visible = true;
                }
                break;
            }
            else
            {
                row.ToolTip = "Click to select this row.";
            }
        }
    }
    protected void gvStudents_Sorted(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
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
    protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void lnkDoubleClick_Click(object sender, EventArgs e)
    {
        try
        {
            string Id = gvStudents.SelectedValue.ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNewPage(" + Id + ", 'Preview student details', 'Preview');", true);
            Edited = true;
        }
        catch { }
    }
    protected void lnkSingleClick_Click(object sender, EventArgs e)
    {

    }
    protected void gvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    protected void btnTermsDetails_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedRow != null)
        {
            try
            {
                String GroupID = Functions.ExecuteScalar("SELECT GroupID FROM [GroupStudent] WHERE GroupStudentID="+gvDetails.SelectedValue);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowTerm('Group Term dates'," + GroupID + ",'Preview');", true);
                //Edited = true;
            }
            catch { }
        }
    }
    protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
    {        
        Fill_Payments();        
        String SQL = "SELECT GroupID FROM GroupStudent WHERE GroupStudentID=" + gvDetails.SelectedValue;
        String GrId = Functions.ExecuteScalar(SQL);
        Fill_Certificates(GrId);
        Fill_Contracts(GrId);

        FillClasses();
    }
    protected void btnPrintPayment_Click(object sender, EventArgs e)
    {
        if (gvPayments.SelectedRow != null)
        {
            string SQLPrint = @"SELECT p.AccountNumber,p.Ammount, p.AmmountWords, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as PaymentGroup,
                        s.FirstName+' '+s.LastName as PaymentName, p.PaymentNumber, s.Place as PaymentPlace,
                        convert(varchar,p.DateOfPayment,104) as DateOfPayment, gs.TotalCost, 
						gs.TotalCost-(SELECT SUM(p2.Ammount) FROM Payment p2 where p2.GroupStudentID=gs.GroupStudentID AND p2.DateOfPayment<=p.DateOfPayment) as RemainingCost,
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
    protected void gvCertificates_Sorted(object sender, EventArgs e)
    {
        if(gvDetails.SelectedValue!=null)
            Fill_Certificates(gvDetails.SelectedValue.ToString());
    }
    protected void gvCertificates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCertificates, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvContracts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvContracts, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvContracts_Sorted(object sender, EventArgs e)
    {
        if(gvDetails.SelectedValue!=null)
            Fill_Contracts(gvDetails.SelectedValue.ToString());
    }
    protected void btnPrintSelectedContract_Click(object sender, EventArgs e)
    {
        if (gvContracts.SelectedRow != null)
        {
            string SQLPrint = @"SELECT FirstName + ' ' + LastName as StudentName, Address as StudentAddress,
                        HouseNumber as StudentHouseNumber, Place as StudentPlace, SocialNumber as StudentSN,
                        Place as StudentGrad, convert(varchar,c.StartDate,104) as StartDate,
						CASE WHEN DATEDIFF(DAY,s.DateOfBirth, GETDATE())<6570 THEN 
						N' (малолетен преку својот законски родител-старател) ' + Parent_FirstName + ' ' + Parent_LastName +
						N' од ' + Parent_Place + N', со стан на ул.' + Parent_Address + N' бр.' + Parent_HouseNumber + N' во ' + 
						Parent_Place + N' и ЕМБГ:' + Parent_SocialNumber +'.' ELSE '' END as MaloletenInfo,
						gt.Language, g.NumberOfClasses, g.NumberOfPayments  as BrojMeseci, 
						(g.NumberOfClasses/g.NumberOfPayments)*(SELECT TOP 1 DATEDIFF(MINUTE, t.TimeStart, t.TimeEnd)/60 FROM Termin t WHERE t.GroupID=g.GroupID)  as RabotniChasa, 
						(SELECT TOP 1 DATEDIFF(MINUTE, t.TimeStart, t.TimeEnd) FROM Termin t WHERE t.GroupID=g.GroupID) as ChasMinuti,
						CEILING(gs.TotalCost/g.NumberOfPayments) as RataIznos, gs.TotalCost						
						FROM [Contract] c LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=c.GroupStudentID
						LEFT OUTER JOIN Student s ON s.StudentID = gs.StudentID
						LEFT OUTER JOIN [Group] g ON g.GroupID = gs.GroupID
						LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID = g.GroupTypeID
						WHERE c.ContractID=" + gvContracts.SelectedValue;

            String PaymentPath = Functions.ExecuteScalar("SELECT TOP 1 TemplateFile FROM Template WHERE TemplateType=1 ORDER BY CreatedDate DESC");
            String PathDoc = Server.MapPath(PaymentPath.Replace(".dotx", ".docx"));
            Functions.PrintWord(PathDoc, SQLPrint);
            PrintPDF(PathDoc);
        }
    }
    protected void btnPrintSelectedCertificate_Click(object sender, EventArgs e)
    {
        if (gvCertificates.SelectedRow != null)
        {
            string SQLPrint = @"SELECT c.RegNo, s.FirstName + ' ' + s.LastName as StudentName, convert(varchar, s.DateOfBirth, 104) as DateOfBirth,
                            s.Place, gt.Language, gt.LevelDescription, gt.Level, gt.Program, g.NumberOfClasses, 
							convert(varchar, g.StartDate, 104) as StartDate, convert(varchar, g.EndDate, 104) as EndDate, 
							convert(varchar, g.EndDate, 104) as DateOfPrint, e.FirstName + ' ' + e.LastName as Teacher
                            FROM [Certificate] c LEFT OUTER JOIN Student s ON s.StudentID=c.StudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
							LEFT OUTER JOIN Employee e ON e.EmployeeID = g.EmployeeID
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

            SQLPrint = @"SELECT c.RegNo, '" + StudentName + @"' as StudentName, convert(varchar,s.DateOfBirth, 104) as DateOfBirth,
                            '" + Place + @"' as Place, '"+ Language + @"' as Language, gt.LevelDescription, gt.Level, gt.Program, g.NumberOfClasses, 
							convert(varchar,g.StartDate,104) as StartDate, convert(varchar,g.EndDate,104) as EndDate, 
							convert(varchar,g.EndDate,104) as DateOfPrint, '" + Teacher + @"' as Teacher,
                            (CASE WHEN s.Gender = 1 THEN '' ELSE 'a' END) as Gender
                            FROM [Certificate] c LEFT OUTER JOIN Student s ON s.StudentID=c.StudentID
                            LEFT OUTER JOIN [Group] g ON g.GroupID=c.GroupID
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
							LEFT OUTER JOIN Employee e ON e.EmployeeID = g.EmployeeID
                            WHERE c.CertificateID=" + gvCertificates.SelectedValue;

            String PathDoc = Server.MapPath(ddlTemplateCertificate.SelectedValue.Replace(".dotx", ""));            
            Functions.PrintWord(PathDoc, SQLPrint);
            PrintPDF(PathDoc);
        }
    }
    protected void btnCreateContract_Click(object sender, EventArgs e)
    {
        if (gvStudents.SelectedRow != null)
        {
            string Id = gvStudents.SelectedValue.ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowContract(" + Id + ", 'Student Contracts', '');", true);
        }
    }
    protected void imgbtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        if (gvDetails.SelectedValue != null)
        {
            Fill_Certificates(gvDetails.SelectedValue.ToString());
            Fill_Contracts(gvDetails.SelectedValue.ToString());
        }
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedRow != null && gvStudents.SelectedRow!=null)
        {
            Boolean Transfered = Convert.ToBoolean(Functions.ExecuteScalar("SELECT Transfered FROM GroupStudent WHERE GroupStudentID=" + gvDetails.SelectedValue));
            if (!Transfered)
            {
                String StudentID = gvStudents.SelectedValue.ToString();
                String GroupID = Functions.ExecuteScalar("SELECT GroupID FROM GroupStudent WHERE GroupStudentID=" + gvDetails.SelectedValue);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowTransfer(" + StudentID + "," + GroupID + ",'Transfer student to another group');", true);
            }
        }
    }
    protected void btnTransferDetails_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedRow != null && gvStudents.SelectedRow != null)
        {
            Boolean Transfered = Convert.ToBoolean(Functions.ExecuteScalar("SELECT Transfered FROM GroupStudent WHERE GroupStudentID=" + gvDetails.SelectedValue));
            if (Transfered)
            {
                String StudentID = gvStudents.SelectedValue.ToString();
                String GroupID = Functions.ExecuteScalar("SELECT GroupID FROM GroupStudent WHERE GroupStudentID=" + gvDetails.SelectedValue);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowTransferDetails(" + StudentID + "," + GroupID + ",'Transfer details');", true);
            }
        }
    }
    #endregion
    protected void btnSaveAdd_Click(object sender, EventArgs e)
    {
        if (gvDetails.SelectedIndex != null)
        {
            Functions.ExecuteCommand(@"UPDATE GroupStudent SET ClassesAttended=N'"+tbClassesAttended.Text+
            @"', PassedFinalTest='"+cbPassedFinalTest.Checked+"', ReceivedCertificate='"+cbReceivedCertificate.Checked+
            @"' WHERE GroupStudentID=" + gvDetails.SelectedValue);            
        }
    }
}