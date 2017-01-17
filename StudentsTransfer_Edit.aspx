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
                <asp:TextBox ID="tbTransferDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Date" AutoPostBack="True"></asp:TextBox>                
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbTransferDate" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbTransferDate" ValidationGroup="1"></asp:RequiredFieldValidator><br />
                <asp:CompareValidator ID="cvtbTransferDate" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbTransferDate"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                                         
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
