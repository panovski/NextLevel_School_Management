<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferDetails.aspx.cs" Inherits="TransferDetails" EnableEventValidation="false" %>

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
                Student:
            </td>
            <td>
                <asp:TextBox ID="tbStudent" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" Enabled="false"></asp:TextBox>
            </td>
            <td>
               
            </td>

            <td rowspan="10" style="vertical-align:top; text-align:left">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                From Group:</td>
            <td>
                <asp:TextBox ID="tbFromGroup" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" Enabled="false"></asp:TextBox>
            </td>
            <td>
                
            </td>
        </tr>
        <tr>
            <td>
                To Group:
            </td>
            <td>
                <asp:TextBox ID="tbToGroup" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True"></asp:TextBox>                
            </td>
            <td>                
            </td>
        </tr>

         <tr>
            <td>
                Transfer date:
            </td>
            <td>
                <asp:TextBox ID="tbTransferDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True" onkeyup="javascript:Date('#tbTransferDate')"></asp:TextBox>                
            </td>
            <td>
                <asp:CompareValidator ID="cvtbTransferDate" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbTransferDate"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                                                         
            </td>
        </tr>

         <tr>
            <td>
                Created date:
            </td>
            <td>
                <asp:TextBox ID="tbCreatedDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Date" AutoPostBack="True"></asp:TextBox>                
            </td>
            <td>       
                <asp:CompareValidator ID="cvtbCreatedDate" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbCreatedDate"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                                                                  
            </td>
        </tr>

         <tr>
            <td>
                Created by:
            </td>
            <td>
                <asp:TextBox ID="tbCreatedBy" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True"></asp:TextBox>                
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
