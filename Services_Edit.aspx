<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Services_Edit.aspx.cs" Inherits="Services_Edit" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Next Level - School Management Software</title>
    <link href="Styles/Styles.css" rel="stylesheet" />
    <link href="Styles/component.css" rel="stylesheet" />
    <link href="Styles/GridViewEditForm.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/ModalPopupWindow.js"></script>
        <script type="text/javascript">
            function CloseDialog() {
                window.alert('The changes are saved!');
                parent.CloseFunction();
            }
    </script>
    <script type="text/javascript">
        function Date(textboxinput) {
            var input = jQuery(textboxinput).val();
            var lenght = input.length;
            if (lenght == 2 && input.indexOf(".") < 1)
                jQuery(textboxinput).val(input + ".");
            if (lenght == 5 && input.charAt(4) !== '.')
                jQuery(textboxinput).val(input + ".");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
 <div>
    <table class="EditForma">        
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>        
        <tr style="vertical-align:top; text-align:left">
           
            <td style="vertical-align:top; text-align:left">
                <table style="margin-left:3vw;">
                    <tr>
                        <td>
                            Service Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvddlServiceType" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlServiceType" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>

                        <td rowspan="8" style="vertical-align:top">
                            <asp:Panel ID="pnlNewCustomer" runat="server" Visible="False">
                                New Customer:
                            <table>
                                <tr>
                                    <td>
                                        First Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbFirstName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvtbFirstName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbFirstName" ValidationGroup="1" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Last Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbLastName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                                    </td>
                                    <td>                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Contact:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbContact" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvtbContact" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbContact" ValidationGroup="1" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Place:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPlace" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvtbPlace" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbPlace" ValidationGroup="1" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                        <td>
                            Date of Birth:
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateOfBirth" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" onkeyup="javascript:Date('#tbDateOfBirth')"></asp:TextBox>
                        </td>
                        <td> 
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbDateOfBirth"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                             
                        </td>
                    </tr>    
                            </table>
                            </asp:Panel>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            Customer:
                        </td>
                        <td>
                            <asp:TextBox ID="tbCustomerSearch" runat="server" CssClass="TextBoxSearchIconEdit" AutoPostBack="True" OnTextChanged="tbCustomerSearch_TextChanged"></asp:TextBox>                            
                        </td>                                                                      
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>  
                        </td>                                                                      
                        <td>
                            <asp:RequiredFieldValidator ID="rfvddlCustomer" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlCustomer" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbNewCustomer" runat="server" CssClass="TextBoxRoundedEdit" AutoPostBack="True" OnCheckedChanged="cbNewCustomer_CheckedChanged" Text="New Customer" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            Employee:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvddlEmployee" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlEmployee" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            Status:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvddlStatus" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlStatus" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>     
                    <tr>
                        <td>
                            Until Date:
                        </td>
                        <td>
                            <asp:TextBox ID="tbToDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" onkeyup="javascript:Date('#tbToDate')"></asp:TextBox>
                        </td>
                        <td> 
                            <asp:CompareValidator ID="cvtbToDate" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbToDate"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                             
                        </td>
                    </tr>     
                    <tr>
                        <td>
                            Quantity:
                        </td>
                        <td>
                            <asp:TextBox ID="tbQuantity" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Number" >1</asp:TextBox>
                        </td>
                        <td> 
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbQuantity" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>                                                      
                    <tr>
                        <td>
                            Created Date:
                        </td>
                        <td>
                            <asp:TextBox ID="tbCreatedDate" runat="server" CssClass="TextBoxRoundedEdit" Enabled="False" ValidationGroup="1" ></asp:TextBox>
                        </td>
                        <td>              
                        </td>
                    </tr>
                     <tr>
                        <td>
                            Created By:
                        </td>
                        <td>
                            <asp:TextBox ID="tbCreatedBy" runat="server" CssClass="TextBoxRoundedEdit" Enabled="False" ValidationGroup="1" ></asp:TextBox>
                        </td>
                        <td>                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Save changes" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="1" Visible="False" />
                            <asp:Button ID="btnInsert" runat="server" Text="Insert new Service" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" Visible="False" />
                        </td>
                    </tr>  
                </table>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
