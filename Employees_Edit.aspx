<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employees_Edit.aspx.cs" Inherits="Employees_Edit" EnableEventValidation="false" %>

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
    <table class="EditForma">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <tr>
            <td>
                First Name:
            </td>
            <td>
                <asp:TextBox ID="tbFirstName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbFirstName" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
            <td style="padding-left:5vw"></td>
            <td rowspan="5" style="padding-left:1vw; padding-top:1vw;">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="1" Visible="false" TabIndex="6"/>               
                <asp:Button ID="btnInsert" runat="server" Text="Insert New Employee" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" Visible="false" TabIndex="7"/>
                <br />
                <asp:Label ID="lblInfo" runat="server" Text="The username is already assigned to another employee!" CssClass="RequredField" Visible="false"></asp:Label>
                <asp:Label ID="lblAlreadyExist" runat="server" Text="The username allready exists!" CssClass="RequredField" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Last Name:
            </td>
            <td>
                <asp:TextBox ID="tbLastName" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="1"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbLastName" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <td>
                Contact Phone:
            </td>
            <td>
                <asp:TextBox ID="tbContactPhone" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="2"></asp:TextBox>
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
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="3"></asp:DropDownList>                      
            </td>
        </tr>
         <tr>
            <td>
                Username:
            </td>
            <td>
                <asp:DropDownList ID="ddlUserName" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="4"></asp:DropDownList>                      
            </td>
        </tr>
         <tr>
            <td>
                Start Date:
            </td>
            <td>
                <asp:TextBox ID="tbStartDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Date" AutoPostBack="True" OnTextChanged="tbStartDate_TextChanged"></asp:TextBox>                               
            </td>
             <td>
                <asp:RequiredFieldValidator ID="rfvDateOfBirth" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbStartDate" ValidationGroup="1"></asp:RequiredFieldValidator><br />
                <asp:CompareValidator ID="cvtbDateOfBirth" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbStartDate"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                    
            </td>
        </tr>
         <tr>
            <td>
                Email:
            </td>
            <td>
                <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBoxRoundedEdit" TabIndex="5" ValidationGroup="1"></asp:TextBox>                
            </td>
        </tr>
        <tr>
            <td>
                
            </td>
            <td>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Wrong e-mail format!" CssClass="RequredField" ControlToValidate="tbEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="1"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbUserName" runat="server" Checked="False" Text="Create Login (username)" Visible="False" TabIndex="5" />
            </td>
        </tr>
    </table>
    </div>
    </form>

</body>
</html>
