<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Students_Edit.aspx.cs" Inherits="Students_Edit" EnableEventValidation="true"%>

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
            <td>
                First Name:
            </td>
            <td>
                <asp:TextBox ID="tbFirstName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbFirstName" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>

            <td rowspan="7" style="vertical-align:top; text-align:left">
                Ask for parrent:
                <asp:RadioButtonList ID="rblParrent" runat="server" RepeatDirection="Horizontal" ToolTip="Parrent" AutoPostBack="True" OnSelectedIndexChanged="rblParrent_SelectedIndexChanged">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="1" Visible="false"/>               
                <asp:Button ID="btnInsert" runat="server" Text="Insert New Student" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" Visible="false"/>                
                <asp:Panel ID="pnlParrents" runat="server" Visible="false">
                    <br />Parents:<br />
                    <table>
                        <tr>
                            <td>
                                First Name:
                            </td>
                            <td>
                                <asp:TextBox ID="tbParrentFirstName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvParrentFirstName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbParrentFirstName" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last Name:
                            </td>
                            <td>
                                <asp:TextBox ID="tbParrentLastName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvParrentLastName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbParrentLastName" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address:
                            </td>
                            <td>
                                <asp:TextBox ID="tbParrentAddress" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbParrentAddress" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbParrentAddress" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                House number:
                            </td>
                            <td>
                                <asp:TextBox ID="tbParrentHouseNumber" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbParrentHouseNumber" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbParrentHouseNumber" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Place:
                            </td>
                            <td>
                                <asp:TextBox ID="tbParrentPlace" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbParrentPlace" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbParrentPlace" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Phone Number / Mobile:
                            </td>
                            <td>
                                <asp:TextBox ID="tbParentPhone" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>                            
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
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbLastName" ValidationGroup="1"></asp:RequiredFieldValidator>
                <asp:Label ID="lblAlreadyExist" runat="server" Text="The student allready exists!" CssClass="RequredField" Visible="False"></asp:Label>
            </td>
        </tr>
       
         <tr>
            <td>
                Contact Phone:
            </td>
            <td>
                <asp:TextBox ID="tbContactPhone" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvContactPhone" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbContactPhone" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Status:
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                 
            </td>
        </tr>   
        <tr>
            <td>
                Date of Birth:
            </td>
            <td>
                <asp:TextBox ID="tbDateOfBirth" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True" onkeyup="javascript:Date('#tbDateOfBirth')" OnTextChanged="tbDateOfBirth_TextChanged" ></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvDateOfBirth" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbDateOfBirth" ValidationGroup="1"></asp:RequiredFieldValidator><br />
                <asp:RegularExpressionValidator ID="revtbDateOfBirth" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02./((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="tbDateOfBirth"  CssClass="RequredField" ValidationGroup="1" ></asp:RegularExpressionValidator>                                          
            </td>
        </tr>
        <tr>
            <td>
                Place of Birth:
            </td>
            <td>
                <asp:TextBox ID="tbPlaceOfBirth" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True" ></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbPlaceOfBirth" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbPlaceOfBirth" ValidationGroup="1"></asp:RequiredFieldValidator><br />
            </td>
        </tr>
        <tr>
            <td>
                Gender:
            </td>
            <td>
                <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="False">Female</asp:ListItem>
                    <asp:ListItem Value="True">Male</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>    
                <asp:RequiredFieldValidator ID="rfvrblGender" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="rblGender" ValidationGroup="1"></asp:RequiredFieldValidator>            
            </td>
        </tr>
        <tr>
            <td>
                Address:
            </td>
            <td>
                <asp:TextBox ID="tbAddress" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" ></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbAddress" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <td>
                House number:
            </td>
            <td>
                <asp:TextBox ID="tbHouseNumber" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" ></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvHouseNumber" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbHouseNumber" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Place:
            </td>
            <td>
                <asp:TextBox ID="tbPlace" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" ></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvPlace" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbPlace" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                E-mail:
            </td>
            <td>
                <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Email" ></asp:TextBox>                
            </td>
            <td>                
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid e-mail address!" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="1"></asp:RegularExpressionValidator>
            </td>
        </tr>        
         <tr>
            <td>
                Created Date:
            </td>
            <td>
                <asp:TextBox ID="tbCreatedDate" runat="server" CssClass="TextBoxRoundedEdit" Enabled="False"></asp:TextBox>
            </td>
            <td>
              
            </td>
        </tr>
         <tr>
            <td>
                Created By:
            </td>
            <td>
                <asp:TextBox ID="tbCreatedBy" runat="server" CssClass="TextBoxRoundedEdit" Enabled="False"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>  
        <tr>            
            <td>                

            </td>
            <td colspan="2">
                
            </td>
        </tr>            
    </table>  
    </div>
    </form>
</body>
</html>
