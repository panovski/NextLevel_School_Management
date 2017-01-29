#region Using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Services_Edit : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Login_Redirect();
            Functions.FillCombo("SELECT ServiceName + ' - '+ Description as ServiceName, ServiceTypeID FROM ServiceType", ddlServiceType, "ServiceName", "ServiceTypeID");
            Functions.FillCombo("SELECT FirstName+' '+LastName as Customer, CustomerID FROM Customer", ddlCustomer, "Customer", "CustomerID");
            Functions.FillCombo("SELECT FirstName + ' ' + LastName as Employee, EmployeeID FROM Employee WHERE Status=1", ddlEmployee, "Employee", "EmployeeID");
            Functions.FillCombo("SELECT 1 as StatusID, 'Active' as Description UNION SELECT 0 as StatusID, 'Done' as Description", ddlStatus, "Description", "StatusID");

            if (Request.QueryString["ID"] != "")
            {
                FillDetailsEdit();
                btnSave.Visible = true;
            }
            else
            {
                btnInsert.Visible = true;
            }

            if (Request.QueryString["Type"] == "Preview")
            {
                FillDetailsEdit();
                DisableControls(this, false);
                btnSave.Visible = false;
                btnInsert.Visible = false;
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
    public void FillDetailsEdit()
    {
        String[] Service = Functions.ReturnIntoArray("SELECT s.*, u.FirstName+' '+u.LastName FROM [Service] s LEFT OUTER JOIN [User] u ON u.UserID=s.CreatedBy WHERE ServiceID=" + Request.QueryString["ID"], 11);
        ddlServiceType.SelectedValue = Service[1];
        ddlCustomer.SelectedValue = Service[2];
        ddlEmployee.SelectedValue = Service[3];
        ddlStatus.SelectedValue = Service[4];
        tbToDate.Text = Convert.ToDateTime(Service[5]).ToString("yyyy-MM-dd");
        tbQuantity.Text = Service[6];
        tbCreatedDate.Text = Service[8];
        tbCreatedBy.Text = Service[10];
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        String CustomerID = ddlCustomer.SelectedValue;
        if (cbNewCustomer.Checked)
        {
            String SQLInsert = @"INSERT INTO [Customer] (FirstName,LastName,Contact,Place,CreatedBy) " +
                             "VALUES(N'" + tbFirstName.Text.Replace("'", "''") + "',N'" + tbLastName.Text.Replace("'", "''") + "',N'" + tbContact.Text.Replace("'", "''") + "',N'" + tbPlace.Text.Replace("'", "''") + "'," +
                             Functions.Decrypt(Request.Cookies["UserID"].Value) + "); SELECT SCOPE_IDENTITY()";
            CustomerID = Functions.ExecuteScalar(SQLInsert);
        }

        String Cost = Functions.ExecuteScalar("SELECT Cost FROM ServiceType WHERE ServiceTypeID=" + ddlServiceType.SelectedValue);
        String SQL = @"UPDATE [Service] SET ServiceTypeID=" + ddlServiceType.SelectedValue +
                  ", CustomerID= " + CustomerID +
                  ", EmployeeID= " + ddlEmployee.SelectedValue +
                  ", Status= '" + ddlStatus.SelectedValue +
                  "', ToDate='" + tbToDate.Text.Replace("'", "''") +
                  "', Quantity=" + tbQuantity.Text.Replace("'", "''") +
                  ", TotalCost='" + Convert.ToInt32(tbQuantity.Text) * Convert.ToDecimal(Cost) + 
                  "' WHERE ServiceID=" + Request.QueryString["ID"];
        Functions.ExecuteCommand(SQL);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        String CustomerID = ddlCustomer.SelectedValue;
        if (cbNewCustomer.Checked)
        {
            String SQLInsert = @"INSERT INTO [Customer] (FirstName,LastName,Contact,Place,CreatedBy) " +
                            "VALUES(N'" + tbFirstName.Text.Replace("'", "''") + "',N'" + tbLastName.Text.Replace("'", "''") + "',N'" + tbContact.Text.Replace("'", "''") + "',N'" + tbPlace.Text.Replace("'", "''") + "'," +
                            Functions.Decrypt(Request.Cookies["UserID"].Value) + "); SELECT SCOPE_IDENTITY()";
            CustomerID = Functions.ExecuteScalar(SQLInsert);
        }

        String Cost = Functions.ExecuteScalar("SELECT Cost FROM ServiceType WHERE ServiceTypeID=" + ddlServiceType.SelectedValue);
        String SQL = @"INSERT INTO [Service] (ServiceTypeID,CustomerID,EmployeeID,Status,ToDate,Quantity,TotalCost,CreatedBy)
                   VALUES(" + ddlServiceType.SelectedValue + "," + CustomerID + "," + ddlEmployee.SelectedValue + "," + ddlStatus.SelectedValue +
                  ",N'" + tbToDate.Text.Replace("'", "''") + "'," + tbQuantity.Text.Replace("'", "''") + ",'" + Convert.ToInt32(tbQuantity.Text) * Convert.ToDecimal(Cost) + "'," + Functions.Decrypt(Request.Cookies["UserID"].Value) + ")";
        Functions.ExecuteCommand(SQL);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "CloseDialog()", true);
    }
    protected void cbNewCustomer_CheckedChanged(object sender, EventArgs e)
    {
        pnlNewCustomer.Visible = cbNewCustomer.Checked;
        ddlCustomer.Enabled = !cbNewCustomer.Checked;

        rfvddlCustomer.Enabled = !cbNewCustomer.Checked;
    }
    protected void tbCustomerSearch_TextChanged(object sender, EventArgs e)
    {
        ddlCustomer.Items.Clear();
        Functions.FillCombo(@"SELECT CustomerID, FirstName+' '+LastName as Name FROM Customer WHERE 
                            (FirstName LIKE N'%" + tbCustomerSearch.Text.Replace("'", "''") + "%' OR LastName LIKE N'%" + tbCustomerSearch.Text.Replace("'", "''") + "%')", ddlCustomer, "Name", "CustomerID");
    }

    #endregion
}