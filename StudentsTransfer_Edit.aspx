<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentsTransfer_Edit.aspx.cs" Inherits="StudentsTransfer_Edit" EnableEventValidation="false"%>

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
            <td>
                From Group:
            </td>
            <td>
                <asp:TextBox ID="tbGroupName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" Enabled="false"></asp:TextBox>
            </td>
            <td>
               
            </td>

            <td rowspan="10" style="vertical-align:top; text-align:left">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="1"/>               
            </td>
        </tr>
        <tr>
            <td>
                Transfer to Group:</td>
            <td>
                <asp:DropDownList ID="ddlCourse" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvddlCourse" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlCourse" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Transfer Date:
            </td>
            <td>
                <asp:TextBox ID="tbTransferDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True" onkeyup="javascript:Date('#tbTransferDate')"></asp:TextBox>                
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbTransferDate" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbTransferDate" ValidationGroup="1"></asp:RequiredFieldValidator><br />
                <asp:RegularExpressionValidator ID="revtbTransferDate" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02./((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="tbTransferDate"  CssClass="RequredField" ValidationGroup="1" ></asp:RegularExpressionValidator>
            </td>
        </tr>       
        <tr>
            <td colspan="3">
                <asp:Label ID="lblInfo" runat="server" Text="" Visible="false" CssClass="RequredField"></asp:Label>
            </td>
        </tr>            
    </table>  
    </div>
    </form>
</body>
</html>
