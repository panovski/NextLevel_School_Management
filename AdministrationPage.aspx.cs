#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class AdministrationPage : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        Login_Redirect();
        if (!Page.IsPostBack)
        {
            Fill_Dashboard_1_1();
            Fill_Dashboard_1_2();
            Fill_Dashboard_2_0();
            Fill_Dashboard_2_1();
            Fill_Dashboard_2_2();
            Fill_Dashboard_2_3();
            Fill_Dashboard_3_1();
            Fill_Dashboard_3_2();
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        if (Request.Cookies["PermLevel"] == null) Response.Redirect("Default.aspx");
        else if (Request.Cookies["PermLevel"].Value == "") Response.Redirect("Default.aspx");
        
        if (Functions.Decrypt(Request.Cookies["PermLevel"].Value) != ConfigurationManager.AppSettings["Admin"].ToString())
            Response.Redirect("Default.aspx");

        //if (Session["PermLevel"] == null) Response.Redirect("Default.aspx");
        //if (Session["PermLevel"].ToString() != ConfigurationManager.AppSettings["Admin"].ToString())
        //{
        //    Response.Redirect("Default.aspx");
        //}
    }
    protected void Fill_Dashboard_1_1()
    {
        dsDashboard1_1.SelectCommand = @"SELECT p.PaymentID,p.PaymentNumber, p.Ammount, 
									(SELECT CASE WHEN p.GroupStudentID IS NOT NULL THEN s.FirstName+' ' +s.LastName WHEN p.InvoiceID IS NOT NULL THEN 'Group Invoice' END) as Student,
									(SELECT CASE WHEN p.GroupStudentID IS NOT NULL THEN e.FirstName+' '+e.LastName WHEN p.InvoiceID IS NOT NULL THEN e2.FirstName+' '+e2.LastName END) as Employee,
									p.Collected, p.PaidToEmployee, p.DateOfPayment, u.FirstName+' '+u.LastName as ProcessedBy,
									(SELECT CASE WHEN p.GroupStudentID IS NOT NULL THEN  CEILING(g.TeacherPercentage*p.Ammount/100) WHEN p.InvoiceID IS NOT NULL THEN  CEILING(g2.TeacherPercentage*p.Ammount/100) END) as ForEmployee
									FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                                    LEFT OUTER JOIN Student s ON s.StudentID=gs.StudentID
                                    LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
									LEFT OUTER JOIN [Group] g2 ON g2.GroupID=p.InvoiceID
                                    LEFT OUTER JOIN Employee e ON e.EmployeeID=g.EmployeeID 
									LEFT OUTER JOIN Employee e2 ON e2.EmployeeID=g2.EmployeeID
									LEFT OUTER JOIN [User] u ON u.UserID=p.CreatedBy
									WHERE p.Transfered=0 AND (p.GroupStudentID>0 or p.InvoiceID>0) AND (PaidToEmployee=0 OR p.Collected=0)";

        lblInfo1_1.Text = gvDashboard1_1.Rows.Count.ToString();// Functions.ExecuteScalar("SELECT COUNT(*) FROM Payment WHERE PaidToEmployee=0");
        String[] Sums = Functions.ReturnIntoArray(@"SELECT SUM(Ammount), SUM(CASE WHEN p.GroupStudentID IS NOT NULL THEN CEILING(g.TeacherPercentage*Ammount/100) WHEN p.InvoiceID IS NOT NULL THEN CEILING(g2.TeacherPercentage*Ammount/100) END)
                                            FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                                            LEFT OUTER JOIN Student s ON s.StudentID=gs.StudentID
                                            LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
											LEFT OUTER JOIN [Group] g2 ON g2.GroupID=p.InvoiceID
                                            WHERE p.Transfered=0 AND (p.GroupStudentID>0 OR p.InvoiceID>0) AND (PaidToEmployee=0 OR p.Collected=0)", 2);
        String[] Sums2 = Functions.ReturnIntoArray(@"SELECT SUM(Ammount), SUM(CASE WHEN p.GroupStudentID IS NOT NULL THEN CEILING(g.TeacherPercentage*Ammount/100) WHEN p.InvoiceID IS NOT NULL THEN CEILING(g2.TeacherPercentage*Ammount/100) END)
                                            FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                                            LEFT OUTER JOIN Student s ON s.StudentID=gs.StudentID
                                            LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
											LEFT OUTER JOIN [Group] g2 ON g2.GroupID=p.InvoiceID
                                            WHERE p.Transfered=0 AND (p.GroupStudentID>0 OR p.InvoiceID>0) AND PaidToEmployee=0 AND p.Collected=1", 2);

        lblTotalPaid1_1.Text = Sums[0];// Functions.ExecuteScalar("SELECT SUM(Ammount) FROM Payment WHERE PaidToEmployee=0");
        lblTotalCollected1_1.Text = Sums2[0];// Functions.ExecuteScalar("SELECT SUM(Ammount) FROM Payment WHERE PaidToEmployee=0 AND Collected=1");
        lblTotalForEmployee1_1.Text = Sums[1];
        lblTotalFromCollected1_1.Text = Sums2[1];

        if (lblTotalCollected1_1.Text == "")
            lblTotalCollected1_1.Text = "0";
        if (lblTotalFromCollected1_1.Text == "")
            lblTotalFromCollected1_1.Text = "0";

        Fill_Dashboard_2_1();
    }
    protected void Fill_Dashboard_1_2()
    {
        dsDashboard1_2.SelectCommand = @"SELECT p.PaymentID,p.PaymentNumber, p.Ammount, c.FirstName+' '+c.LastName as Customer, e.FirstName+' '+e.LastName as Employee, p.Collected, p.PaidToEmployee, p.DateOfPayment, u.FirstName+' '+u.LastName as ProcessedBy,
                                    CEILING((st.EmployeePercentage/st.Cost)*p.Ammount) as ForEmployee
                                    FROM Payment p LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID
									LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
									LEFT OUTER JOIN Customer c ON c.CustomerID=s.CustomerID
                                    LEFT OUTER JOIN Employee e ON e.EmployeeID=s.EmployeeID 
									LEFT OUTER JOIN [User] u ON u.UserID=p.CreatedBy
									WHERE p.Transfered=0 AND p.ServiceID>0 AND (p.PaidToEmployee=0 OR p.Collected=0)";

        lblInfo1_2.Text = gvDashboard1_2.Rows.Count.ToString();// Functions.ExecuteScalar("SELECT COUNT(*) FROM Payment WHERE PaidToEmployee=0");
        String[] Sums = Functions.ReturnIntoArray(@"SELECT SUM(Ammount), SUM(CEILING(EmployeePercentage*Ammount/100)) 
                                            FROM Payment p LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID
									        LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                                            WHERE p.ServiceID>0 AND (p.PaidToEmployee=0 OR p.Collected=0)", 2);
        String[] Sums2 = Functions.ReturnIntoArray(@"SELECT SUM(Ammount), SUM(CEILING(EmployeePercentage*Ammount/100)) 
                                            FROM Payment p LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID
									        LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                                            WHERE p.Transfered=0 AND p.ServiceID>0 AND PaidToEmployee=0 AND Collected=1", 2);

        lblTotalPaid1_2.Text = Sums[0];// Functions.ExecuteScalar("SELECT SUM(Ammount) FROM Payment WHERE PaidToEmployee=0");
        lblTotalCollected1_2.Text = Sums2[0];// Functions.ExecuteScalar("SELECT SUM(Ammount) FROM Payment WHERE PaidToEmployee=0 AND Collected=1");
        lblTotalForEmployee1_2.Text = Sums[1];
        lblTotalFromCollected1_2.Text = Sums2[1];

        if (lblTotalCollected1_2.Text == "")
            lblTotalCollected1_2.Text = "0";
        if (lblTotalFromCollected1_2.Text == "")
            lblTotalFromCollected1_2.Text = "0";

        Fill_Dashboard_2_1();
    }
    protected void Fill_Dashboard_2_0()
    {
        dsDashboard2_0.SelectCommand = @"SELECT e.UserID,e.FirstName+' '+e.LastName as Employee, SUM(p.Ammount) as Ammount, p.Collected
                        FROM Payment p 
                        LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                        LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
						LEFT OUTER JOIN [Group] g2 ON g2.GroupID=p.InvoiceID
                        LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID
                        LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                        LEFT OUTER JOIN [User] e ON e.UserID=p.CreatedBy
                        WHERE p.Transfered=0 AND p.Collected=0
                        GROUP BY e.UserID,e.FirstName,e.LastName,p.Collected";
    }
    protected void Fill_Dashboard_2_1()
    {
        dsDashboard2_1.SelectCommand = @"SELECT e.EmployeeID,e.FirstName+' '+e.LastName as Employee,
                        CEILING(SUM(p.Ammount/100* CASE WHEN p.ServiceID>0 THEN (st.EmployeePercentage/st.Cost)*100 WHEN p.GroupStudentID>0 THEN g.TeacherPercentage WHEN p.InvoiceID>0 THEN g2.TeacherPercentage END)) as TotalForEmployee,
                        CEILING(SUM(CASE WHEN p.Collected=1 THEN p.Ammount ELSE 0 END /100 * CASE WHEN p.ServiceID>0 THEN (st.EmployeePercentage/st.Cost)*100 WHEN p.GroupStudentID>0 THEN g.TeacherPercentage WHEN p.InvoiceID>0 THEN g2.TeacherPercentage END)) as TotalFromCollected,p.PaidToEmployee
                        FROM Payment p 
                        LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID
                        LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
						LEFT OUTER JOIN [Group] g2 ON g2.GroupID=p.InvoiceID
                        LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID
                        LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                        LEFT OUTER JOIN Employee e ON (e.EmployeeID=g.EmployeeID OR e.EmployeeID=s.EmployeeID OR e.EmployeeID=g2.EmployeeID)
                        WHERE p.Transfered=0 AND p.PaidToEmployee=0
                        GROUP BY e.EmployeeID,e.FirstName,e.LastName,p.PaidToEmployee";
    }
    protected void Fill_Dashboard_2_2()
    {
        dsDashboard2_2.SelectCommand = @"SELECT s.ServiceID, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as ServiceName, c.FirstName+' '+c.LastName as Customer, s.TotalCost as Cost, SUM(p.Ammount) as Paid
                                        FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                                        LEFT OUTER JOIN Payment p ON p.ServiceID=s.ServiceID
										LEFT OUTER JOIN Customer c ON c.CustomerID=s.CustomerID
                                        group by s.ServiceID,st.ServiceName,c.FirstName,c.LastName,s.TotalCost
                                        HAVING SUM(p.Ammount)<s.TotalCost
                                        UNION SELECT s.ServiceID, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as ServiceName, c.FirstName+' '+c.LastName as Customer, s.TotalCost, 0 as Paid
                                        FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
										LEFT OUTER JOIN Customer c ON c.CustomerID=s.CustomerID
                                        WHERE s.ServiceID NOT IN (SELECT distinct(p.ServiceID) FROM Payment p WHERE p.ServiceID>0)";
    }
    protected void Fill_Dashboard_2_3()
    {
        dsDashboard2_3.SelectCommand = @"SELECT gs.GroupStudentID, s.FirstName+' '+s.LastName as Student, g.GroupName + '-' +gt.Language+'-'+gt.LevelDescription+'-'+gt.Level as Course
                                    FROM Student s LEFT OUTER JOIN GroupStudent gs ON gs.StudentID=s.StudentID
                                    LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                                    LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                                    WHERE gs.GroupStudentID NOT IN (SELECT GroupStudentID FROM [Contract]) AND g.IndividualGroup=0";
    }
    protected void Fill_Dashboard_3_1()
    {
        dsDashboard3_1.SelectCommand = @"SELECT t.ClassRoomID, c.Name, CASE WHEN Day = 1 THEN 'Monday'
                                    WHEN Day = 2 THEN 'Tuesday'
                                    WHEN Day = 3 THEN 'Wednesday'
                                    WHEN Day = 4 THEN 'Thursday'
                                    WHEN Day = 5 THEN 'Friday'
                                    WHEN Day = 6 THEN 'Saturday'
                                    WHEN Day = 7 THEN 'Sunday'
                                    END as Day, 
                                    t.TimeStart, t.TimeEnd, g.GroupName
                                    FROM Termin t LEFT OUTER JOIN [Group] g ON g.GroupID=t.GroupID
                                    LEFT OUTER JOIN ClassRoom c ON c.ClassRoomID=t.ClassRoomID
                                    WHERE g.Status=1
                                    ORDER BY ClassRoomID, t.Day";
    }

    protected void Fill_Dashboard_3_2()
    {
        dsDashboard3_2.SelectCommand = @"SELECT gs.GroupStudentID, g.GroupName, st.FirstName+' '+st.LastName as Student, gs.TotalCost as Cost, CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END as Paid
                                        FROM [GroupStudent] gs LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                                        LEFT OUTER JOIN Payment p ON p.GroupStudentID=gs.GroupStudentID
                                        LEFT OUTER JOIN Student st ON st.StudentID=gs.StudentID
                                        WHERE g.Status=0
                                        GROUP BY gs.GroupStudentID, g.GroupName, st.FirstName,st.LastName, gs.TotalCost
                                        HAVING CASE WHEN SUM(p.Ammount) IS NULL THEN 0 ELSE SUM(p.Ammount) END <gs.TotalCost";
    }
    #endregion

    #region Handled Events
    protected void ibtnLogout_Click(object sender, ImageClickEventArgs e)
    {
        //System.Web.Security.FormsAuthentication.SetAuthCookie("", true);
        Response.Cookies.Remove("UserID");
        Response.Cookies.Remove("PermLevel");
        Response.Cookies["UserID"].Value = "";
        Response.Cookies["PermLevel"].Expires.AddMilliseconds(1);
        Response.Cookies["UserID"].Expires.AddMilliseconds(1);

        HttpCookie Loaded = new HttpCookie("Loaded");
        Loaded.Value = "false";
        Loaded.Expires = DateTime.Now.AddHours(8);
        Response.SetCookie(Loaded);

        Session["Loaded"] = false;
        //Session["najaven"] = false;
        Session["PermLevel"] = null;

        Response.Redirect("Default.aspx");
    }
    protected void ibtnGroupTypes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowGrTypes('Group Types','');", true);
        }
        catch (Exception err)
        {
            HttpContext.Current.Session["ErrorMessage"] = err.Message.ToString();
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Redirect(@"~\Error.aspx");
        }
    }
    protected void ibtnServiceTypes_Click(object sender, ImageClickEventArgs e)
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
    protected void itbnClassrooms_Click(object sender, ImageClickEventArgs e)
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
    protected void ibtnTemplates_Click(object sender, ImageClickEventArgs e)
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
    protected void btnSave1_1_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridViewRow gr in gvDashboard1_1.Rows)
        {
            CheckBox checkBox = gr.Cells[5].Controls[0] as CheckBox;
            if (checkBox.Checked && checkBox.Enabled)
            {
                Functions.ExecuteCommand("UPDATE Payment SET Collected=1 WHERE PaymentNumber='" + gr.Cells[1].Text + "'");
            }

            CheckBox checkBox2 = gr.Cells[6].Controls[0] as CheckBox;
            if (checkBox2.Checked && checkBox2.Enabled)
            {
                Functions.ExecuteCommand("UPDATE Payment SET PaidToEmployee=1 WHERE PaymentNumber='" + gr.Cells[1].Text + "'");
            }
        }

        Fill_Dashboard_1_1();
    }
    protected void btnSave1_2_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridViewRow gr in gvDashboard1_2.Rows)
        {
            CheckBox checkBox = gr.Cells[5].Controls[0] as CheckBox;
            if (checkBox.Checked && checkBox.Enabled)
            {
                Functions.ExecuteCommand("UPDATE Payment SET Collected=1 WHERE PaymentNumber='" + gr.Cells[1].Text + "'");
            }

            CheckBox checkBox2 = gr.Cells[6].Controls[0] as CheckBox;
            if (checkBox2.Checked && checkBox2.Enabled)
            {
                Functions.ExecuteCommand("UPDATE Payment SET PaidToEmployee=1 WHERE PaymentNumber='" + gr.Cells[1].Text + "'");
            }
        }

        Fill_Dashboard_1_2();
    }
    protected void btnSave2_0_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridViewRow gr in gvDashboard2_0.Rows)
        {
            CheckBox checkBox = gr.Cells[3].Controls[0] as CheckBox;
            if (checkBox.Checked && checkBox.Enabled)
            {
                Functions.ExecuteCommand(@"UPDATE Payment SET Collected=1 WHERE Collected=0 AND Transfered=0 AND CreatedBy=" + gr.Cells[0].Text);
            }
        }
        Fill_Dashboard_1_1();
        Fill_Dashboard_1_2();
        Fill_Dashboard_2_0();
        Fill_Dashboard_2_1();
    }
    protected void btnSave2_1_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridViewRow gr in gvDashboard2_1.Rows)
        {
            CheckBox checkBox = gr.Cells[4].Controls[0] as CheckBox;
            if (checkBox.Checked && checkBox.Enabled)
            {
                Functions.ExecuteCommand(@"UPDATE Payment SET PaidToEmployee=1 WHERE PaymentID IN
                                        (SELECT PaymentID FROM Payment p LEFT OUTER JOIN GroupStudent gs ON gs.GroupStudentID=p.GroupStudentID 
                                        LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID
                                        WHERE g.EmployeeID=" + gr.Cells[0].Text + @" AND 
                                        p.PaidToEmployee=0 AND p.Transfered=0
                                        UNION 
                                        SELECT PaymentID FROM Payment p LEFT OUTER JOIN [Service] s ON s.ServiceID=p.ServiceID 
                                        WHERE s.EmployeeID=" + gr.Cells[0].Text + @" AND 
                                        p.PaidToEmployee=0 AND p.Transfered=0
                                        UNION
                                        SELECT PaymentID FROM Payment p LEFT OUTER JOIN [Group] g ON g.GroupID=p.InvoiceID 
                                        WHERE g.EmployeeID=" + gr.Cells[0].Text + @" AND 
                                        p.PaidToEmployee=0 AND p.Transfered=0)");
            }
        }
        Fill_Dashboard_1_1();
        Fill_Dashboard_1_2();
        Fill_Dashboard_2_0();
        Fill_Dashboard_2_1();
    }
    protected void btnRefresh1_1_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Dashboard_1_1();
    }
    protected void btnRefresh1_2_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Dashboard_1_2();
    }
    protected void btnRefresh2_0_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Dashboard_2_0();
    }
    protected void btnRefresh2_1_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Dashboard_2_1();
    }
    protected void btnRefresh2_3_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Dashboard_2_3();
    }
    protected void btnRefresh3_1_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Dashboard_3_1();
    }
    protected void gvDashboard1_1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox checkBox = e.Row.Cells[5].Controls[0] as CheckBox;
            if (!checkBox.Checked)
                checkBox.Enabled = true;

            CheckBox checkBox2 = e.Row.Cells[6].Controls[0] as CheckBox;
            if (!checkBox2.Checked)
                checkBox2.Enabled = true;
        }
    }
    protected void gvDashboard1_1_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_1_1();
    }
    protected void gvDashboard1_1_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_1_1();
    }
    protected void gvDashboard1_2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox checkBox = e.Row.Cells[5].Controls[0] as CheckBox;
            if (!checkBox.Checked)
                checkBox.Enabled = true;

            CheckBox checkBox2 = e.Row.Cells[6].Controls[0] as CheckBox;
            if (!checkBox2.Checked)
                checkBox2.Enabled = true;
        }
    }
    protected void gvDashboard1_2_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_1_2();
    }
    protected void gvDashboard1_2_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_1_2();
    }
    protected void gvDashboard2_0_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_2_0();
    }
    protected void gvDashboard2_0_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox checkBox = e.Row.Cells[3].Controls[0] as CheckBox;
            if (!checkBox.Checked)
                checkBox.Enabled = true;
        }
    }
    protected void gvDashboard2_0_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_2_0();
    }
    protected void gvDashboard2_1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox checkBox = e.Row.Cells[4].Controls[0] as CheckBox;
            if (!checkBox.Checked)
                checkBox.Enabled = true;
        }
    }
    protected void gvDashboard2_1_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_2_1();
    }
    protected void gvDashboard2_1_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_2_1();
    }
    protected void gvDashboard2_2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvDashboard2_2_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_2_2();
    }
    protected void gvDashboard2_2_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_2_2();
    }
    protected void gvDashboard2_3_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvDashboard2_3_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_2_3();
    }
    protected void gvDashboard2_3_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_2_3();
    }
    protected void gvDashboard3_1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvDashboard3_1_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_3_1();
    }
    protected void gvDashboard3_1_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_3_1();
    }
    protected void btnRefresh3_2_Click(object sender, ImageClickEventArgs e)
    {
        Fill_Dashboard_3_2();
    }
    protected void gvDashboard3_2_PageIndexChanged(object sender, EventArgs e)
    {
        Fill_Dashboard_3_2();
    }
    protected void gvDashboard3_2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvDashboard3_2_Sorted(object sender, EventArgs e)
    {
        Fill_Dashboard_3_2();
    }
    #endregion

}