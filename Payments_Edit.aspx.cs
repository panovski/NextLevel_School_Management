#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Payments_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {           
            if (Request.QueryString["ID"] != "")
            {
                Fill_Payment(Request.QueryString["ID"]);
                btnSavePayment.Visible = true;
            }

            if (Request.QueryString["Type"] == "Preview")
            {
                //Fill_Payment(Request.QueryString["ID"]);
                DisableControls(this, false);
                btnSavePayment.Visible = false;
            }
        }
    }
    #endregion

    #region Functions
    public void Login_Redirect()
    {
        if (Session["PermLevel"] == null) Response.Redirect("Default.aspx");
        if (Session["PermLevel"].ToString() != ConfigurationManager.AppSettings["Admin"].ToString() &&
          Session["PermLevel"].ToString() != ConfigurationManager.AppSettings["Advanced"].ToString() &&
          Session["PermLevel"].ToString() != ConfigurationManager.AppSettings["Edit"].ToString())
        {
            Response.Redirect("Default.aspx");
        }
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
            else if (c is RadioButtonList)
                ((RadioButtonList)(c)).Enabled = State;

            DisableControls(c, State);
        }
    }
    protected void Fill_Payment(String PaymentID)
    {
        lblInfo.Visible = false;
        String[] Payment = Functions.ReturnIntoArray(@"SELECT p.*,u1.FirstName+' '+u1.LastName as ProcessedBy, u2.FirstName+' '+u2.LastName as CreatedByUser FROM Payment p 
        LEFT OUTER JOIN [User] u1 ON u1.UserID = p.UserID LEFT OUTER JOIN [User] u2 ON u2.UserID=p.CreatedBy 
        WHERE PaymentID=" + PaymentID, 17);
        tbPaymentID.Text = Payment[0];
        tbAddPaymentNumber.Text = Payment[1];
        tbAddAmmount.Text = Payment[2];
        tbAddAmmountWords.Text = Payment[3];
        tbAccountNumber.Text = Payment[4];
        tbAddDateOfPayment.Text = Convert.ToDateTime(Payment[5]).ToString("yyyy-MM-dd");
        tbProcessedBy.Text = Payment[15];
        tbCreatedDate.Text = Convert.ToDateTime(Payment[9]).ToString("yyyy-MM-dd");
        tbCreatedBy.Text = Payment[16];

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

            ddlStudents.SelectedIndex = 0;
            ddlStudents_SelectedIndexChanged(null, null);
            ddlAddGroup.SelectedValue = GroupStudent[1];
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
        }

    }
    #endregion

    #region Handled Events
    protected void btnSave_Click(object sender, EventArgs e)
    {

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
    protected void btnSavePayment_Click(object sender, EventArgs e)
    {
        String SQL = "";
        if (rblType.SelectedValue == "0")
        {
            String GroupStudentID = Functions.ExecuteScalar("SELECT GroupStudentID FROM GroupStudent WHERE GroupID=" + ddlAddGroup.SelectedValue + " AND StudentID=" + ddlStudents.SelectedValue);
            SQL = @"UPDATE Payment SET PaymentNumber = N'" + tbAddPaymentNumber.Text.Replace("'", "''") +
             "', Ammount=" + tbAddAmmount.Text.Replace("'", "''") +
             ", AmmountWords=N'" + tbAddAmmountWords.Text.Replace("'", "''") +
             "', AccountNumber=N'" + tbAccountNumber.Text.Replace("'", "''") +
             "', DateOfPayment='" + tbAddDateOfPayment.Text.Replace("'", "''") +
             "', GroupStudentId=" + GroupStudentID +
             " WHERE PaymentID=" + Request.QueryString["ID"];
        }

        else if (rblType.SelectedValue == "1")
        {
            SQL = @"UPDATE Payment SET PaymentNumber = N'" + tbAddPaymentNumber.Text.Replace("'", "''") +
              "', Ammount=" + tbAddAmmount.Text.Replace("'", "''") +
              ", AmmountWords=N'" + tbAddAmmountWords.Text.Replace("'", "''") +
              "', AccountNumber=N'" + tbAccountNumber.Text.Replace("'", "''") +
              "', DateOfPayment='" + tbAddDateOfPayment.Text.Replace("'", "''") +
              "', ServiceID=" + ddlService.SelectedValue +
              " WHERE PaymentID=" + Request.QueryString["ID"];

        }

        else if (rblType.SelectedValue == "2")
        {
            SQL = @"UPDATE Payment SET PaymentNumber = N'" + tbAddPaymentNumber.Text.Replace("'", "''") +
             "', Ammount=" + tbAddAmmount.Text.Replace("'", "''") +
             ", AmmountWords=N'" + tbAddAmmountWords.Text.Replace("'", "''") +
             "', AccountNumber=N'" + tbAccountNumber.Text.Replace("'", "''") +
             "', DateOfPayment='" + tbAddDateOfPayment.Text.Replace("'", "''") +
             "', InvoiceID=" + ddlInvoiceGroup.SelectedValue +
             " WHERE PaymentID=" + Request.QueryString["ID"];
        }

        Functions.ExecuteCommand(SQL);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);

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
    protected void ddlStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlAddGroup.Items.Clear();
        Functions.FillCombo(@"SELECT gs.GroupID, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as Description
                            FROM GroupStudent gs LEFT OUTER JOIN [Group] g ON g.GroupID=gs.GroupID 
                            LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID WHERE gs.StudentID=" +
                            ddlStudents.SelectedValue, ddlAddGroup, "Description", "GroupId");
        if (ddlAddGroup.Items.Count > 0)
        {
            ddlAddGroup.SelectedIndex = 0;
            ddlAddGroup_SelectedIndexChanged(sender, e);
        }
    }
    protected void ddlAddGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void tbCustomerSearch_TextChanged(object sender, EventArgs e)
    {
        ddlCustomers.Items.Clear();
        Functions.FillCombo(@"SELECT CustomerID, FirstName+' '+LastName as Name FROM Customer WHERE 
                            (FirstName LIKE N'%" + tbCustomerSearch.Text.Replace("'", "''") + "%' OR LastName LIKE N'%" + tbCustomerSearch.Text.Replace("'", "''") + "%')", ddlCustomers, "Name", "CustomerID");
    }
    protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlService.Items.Clear();
        Functions.FillCombo(@"SELECT s.ServiceID, CAST(s.ServiceID AS VARCHAR(16)) + '-' + st.ServiceName as ServiceName
                            FROM [Service] s LEFT OUTER JOIN ServiceType st ON st.ServiceTypeID=s.ServiceTypeID
                            WHERE s.CustomerID=" + ddlCustomers.SelectedValue, ddlService, "ServiceName", "ServiceID");
    }
    protected void ddlService_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlInvoiceGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void tbInvoiceGroup_TextChanged(object sender, EventArgs e)
    {
        ddlAddGroup.Items.Clear();
        Functions.FillCombo(@"SELECT g.GroupID, g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription as Description
                            FROM [Group] g LEFT OUTER JOIN GroupType gt ON gt.GroupTypeID=g.GroupTypeID
                            WHERE g.Invoice=0 AND g.GroupName+'-'+gt.Language+'-'+gt.LevelDescription LIKE N'%" + tbInvoiceGroup.Text + "%'", ddlAddGroup, "GroupId", "Description");
    }
    #endregion    
}