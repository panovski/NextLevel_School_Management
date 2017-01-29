<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payments_Edit.aspx.cs" Inherits="Payments_Edit"  EnableEventValidation="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Next Level - School Management Software</title>
    <link href="Styles/Styles.css" rel="stylesheet" />
    <link href="Styles/component.css" rel="stylesheet" />
    <link href="Styles/GridView.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/ModalPopupWindow.js"></script>
        <script type="text/javascript">
            function CloseDialog() {
                window.alert('The changes are saved!');
                parent.CloseFunction();
            }
    </script>
</head>
<body>
<form id="form1" runat="server">
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager> 
    <table class="EditForma">                       
        <tr>
                            <td>
                                <br />
                                <asp:TextBox ID="tbPaymentID" runat="server" Visible="false"></asp:TextBox>
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                Payment Number:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddPaymentNumber" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="1" ValidationGroup="2"></asp:TextBox>                                        
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbAddPaymentNumber" runat="server" ErrorMessage="!" ControlToValidate="tbAddPaymentNumber" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                            </td>
                            <td rowspan="8" style="vertical-align:top; padding-left:1vw">
                                <table>
                                     <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Student</asp:ListItem>
                                    <asp:ListItem Value="1">Additional Service</asp:ListItem>
                                    <asp:ListItem Value="2">Invoice</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlStudent" runat="server" Visible="true">
                                    <table>
                                        <tr>
                                            <td>
                                                Student:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbStudentsSearch" runat="server" CssClass="TextBoxSearchIconEdit" AutoPostBack="True" OnTextChanged="tbStudentsSearch_TextChanged" TabIndex="6" ValidationGroup="2"></asp:TextBox><br />
                                                <asp:DropDownList ID="ddlStudents" runat="server" CssClass="TextBoxRoundedEdit" AutoPostBack="True" OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged" OnTextChanged="ddlStudents_SelectedIndexChanged" TabIndex="7" ValidationGroup="2"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlStudents" runat="server" ErrorMessage="!" ControlToValidate="ddlStudents" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Group:
                                            </td>
                                            <td>                                               
                                                <asp:DropDownList ID="ddlAddGroup" runat="server" CssClass="TextBoxRoundedEdit" AutoPostBack="True" OnSelectedIndexChanged="ddlAddGroup_SelectedIndexChanged" TabIndex="9" ValidationGroup="2"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlAddGroup" runat="server" ErrorMessage="!" ControlToValidate="ddlAddGroup" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlAdditionalService" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                Customer:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCustomerSearch" runat="server" CssClass="TextBoxSearchIconEdit" AutoPostBack="True" OnTextChanged="tbCustomerSearch_TextChanged" ValidationGroup="2" ></asp:TextBox><br />
                                                <asp:DropDownList ID="ddlCustomers" runat="server" CssClass="TextBoxRoundedEdit" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged" ValidationGroup="2"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlCustomers" runat="server" ErrorMessage="!" ControlToValidate="ddlCustomers" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Service:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlService" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="2" OnSelectedIndexChanged="ddlService_SelectedIndexChanged"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlService" runat="server" ErrorMessage="!" ControlToValidate="ddlService" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlInvoice" runat="server" Visible="false">
                                    <table>                                       
                                        <tr>
                                            <td>
                                                Group:                                                                                 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbInvoiceGroup" runat="server" CssClass="TextBoxSearchIconEdit" AutoPostBack="True" OnTextChanged="tbInvoiceGroup_TextChanged" Visible="false" TabIndex="8" ValidationGroup="2"></asp:TextBox><br />
                                                <asp:DropDownList ID="ddlInvoiceGroup" runat="server" CssClass="TextBoxRoundedEdit" AutoPostBack="True" OnSelectedIndexChanged="ddlInvoiceGroup_SelectedIndexChanged" TabIndex="9" ValidationGroup="2"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlInvoiceGroup" runat="server" ErrorMessage="!" ControlToValidate="ddlInvoiceGroup" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Label ID="lblInfo" runat="server" Text="" Visible="false" CssClass="RequredFieldSmall"></asp:Label>
                                <asp:Button ID="btnSavePayment" runat="server" Text="Save Payment" CssClass="EditFormButton" TabIndex="10" ValidationGroup="2" OnClick="btnSavePayment_Click" /><br />
                                <br />                                
                            </td>
                        </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ammount:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddAmmount" runat="server" CssClass="TextBoxRoundedEdit" TextMode="Number" TabIndex="2" ValidationGroup="2"></asp:TextBox>                                        
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbAddAmmount" runat="server" ErrorMessage="!" ControlToValidate="tbAddAmmount" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                in words:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddAmmountWords" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="3" ValidationGroup="2" ></asp:TextBox>                                        
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbAddAmmountWords" runat="server" ErrorMessage="!" ControlToValidate="tbAddAmmountWords" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                Date of Payment:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddDateOfPayment" runat="server" CssClass="TextBoxRoundedEdit" TextMode="Date" TabIndex="5" ValidationGroup="2"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbAddDateOfPayment" runat="server" ErrorMessage="!" ControlToValidate="tbAddDateOfPayment" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator><br />
                                <asp:CompareValidator ID="cvtbAddDateOfPayment" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbAddDateOfPayment"  CssClass="RequredField" ValidationGroup="2" ></asp:CompareValidator>                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Processed by:
                            </td>
                            <td>
                                <asp:TextBox ID="tbProcessedBy" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="5" ValidationGroup="2" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
                       <tr>
                            <td>
                                Created Date:
                            </td>
                            <td>
                                <asp:TextBox ID="tbCreatedDate" runat="server" CssClass="TextBoxRoundedEdit" TextMode="Date" TabIndex="5" ValidationGroup="2" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CompareValidator ID="cvtbCreatedDate" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbCreatedDate"  CssClass="RequredField" ValidationGroup="2" ></asp:CompareValidator>                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Created by:
                            </td>
                            <td>
                                <asp:TextBox ID="tbCreatedBy" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="5" ValidationGroup="2" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
    </table>  
    </div>
    </form>
</body>
</html>
