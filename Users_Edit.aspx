<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users_Edit.aspx.cs" Inherits="Users_Edit"  EnableEventValidation="true"%>

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
                Username:
            </td>
            <td>
                <asp:TextBox ID="tbUserName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbUserName" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <asp:Panel ID="pnlPassword" runat="server" Visible="false">
            <td>
                Password:
            </td>
            <td>
                <asp:TextBox ID="tbPassword" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Password"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbPassword" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
            </asp:Panel>
        </tr>
        <tr>
            <td>
                First Name:
            </td>
            <td>
                <asp:TextBox ID="tbFirstName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbFirstName" ValidationGroup="1"></asp:RequiredFieldValidator>
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
                Enabled:
            </td>
            <td>
                <asp:DropDownList ID="ddlEnable" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                 
            </td>
        </tr>   
        <tr>
            <td>
                <asp:CheckBox ID="cbChangePassword" runat="server" Text="Change Password?" TextAlign="Left" />
            </td>
            <td>
                <asp:Panel ID="pnlChangePassword" runat="server">
                    <asp:TextBox ID="tbChangePassword" runat="server" CssClass="TextBoxRoundedEdit" TextMode="Password"></asp:TextBox>
                </asp:Panel>                
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbContactPhone" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>  
        <tr>
            <td>
                <br />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="1" Visible="false"/>               
                <asp:Button ID="btnInsert" runat="server" Text="Insert New Employee" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" Visible="false"/>                
            </td>
            <td colspan="2">
                <asp:Label ID="lblAlreadyExist" runat="server" Text="The username allready exists!" CssClass="RequredField" Visible="false"></asp:Label>
            </td>
        </tr>            
    </table>        
    </div>
    </form>
</body>
</html>
