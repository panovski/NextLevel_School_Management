﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Groups_Edit.aspx.cs" Inherits="Groups_Edit" EnableEventValidation="true"  %>
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
            if (lenght == 2 && input.indexOf(".")<1)
                jQuery(textboxinput).val(input + ".");
            if (lenght == 5 && input.charAt(4)!=='.')
                jQuery(textboxinput).val(input + ".");
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            height: 31px;
        }
    </style>

</head>
<body>
 <form id="form1" runat="server">
    <div>
    <table class="EditForma">        
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>        
        <tr style="vertical-align:top; text-align:left">
            <td>
            </td>
            <td>
               <asp:CheckBox ID="cbIndividual" runat="server" Text="Individual group" />
            </td>
            <td>
                
            </td>
            
            <td>                
            </td>
        </tr>
        <tr style="vertical-align:top; text-align:left">
            <td>
                Group Name:
            </td>
            <td>
                <asp:TextBox ID="tbGroupName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" Enabled="false"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbGroupName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbGroupName" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>

            <td rowspan="10" style="vertical-align:top; text-align:left">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="1" Visible="false"/>               
                <br />
                <asp:Button ID="btnInsert" runat="server" Text="Insert New Group" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" Visible="false"/>                
            </td>
        </tr>
        <tr>
            <td>
                Group Type:
            </td>
            <td>
                <asp:DropDownList ID="ddlCourse" runat="server" CssClass="TextBoxRoundedEdit" ></asp:DropDownList>             
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvddlCourse" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlCourse" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Start Date:
            </td>
            <td>
                <asp:TextBox ID="tbStartDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True" onkeyup="javascript:Date('#tbStartDate')"></asp:TextBox>                
            </td>
            <td>                
                <asp:RequiredFieldValidator ID="rfvtbStartDate" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbStartDate" ValidationGroup="1"></asp:RequiredFieldValidator><br />
                <asp:RegularExpressionValidator ID="revStartDate" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02./((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="tbStartDate"  CssClass="RequredField" ValidationGroup="1" ></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                End Date:
            </td>
            <td>
                <asp:TextBox ID="tbEndDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" AutoPostBack="True" onkeyup="javascript:Date('#tbEndDate')"></asp:TextBox>                
            </td>
            <td>
                <asp:RegularExpressionValidator ID="revEndDate" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02./((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="tbEndDate"  CssClass="RequredField" ValidationGroup="1" ></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                Number of classes:
            </td>
            <td>
                <asp:TextBox ID="tbNoClasses" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Number"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbNoClasses" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbNoClasses" ValidationGroup="2"></asp:RequiredFieldValidator>                
            </td>
        </tr>
        <tr>
            <td>
                Number of payments:
            </td>
            <td>
                <asp:TextBox ID="tbNoPayments" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Number"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbNoPayments" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbNoPayments" ValidationGroup="2"></asp:RequiredFieldValidator>                
            </td>
        </tr>
         <tr>
            <td class="auto-style1">
                Cost:
            </td>
            <td class="auto-style1">
                <asp:TextBox ID="tbCost" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Number"></asp:TextBox>
            </td>
            <td class="auto-style1">
                <asp:RequiredFieldValidator ID="rfvtbCost" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbCost" ValidationGroup="2"></asp:RequiredFieldValidator>                
            </td>
        </tr>
        <tr>
            <td>
                Teacher:
            </td>
            <td>
                <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" ></asp:DropDownList>                
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvddlTeacher" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlTeacher" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr> 
         <tr>
            <td>
                Teacher Percentage:
            </td>
            <td>
                <asp:TextBox ID="tbTeacherPercentage" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Number"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbTeacherPercentage" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbTeacherPercentage" ValidationGroup="1"></asp:RequiredFieldValidator>
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
                Paid with invoice:
            </td>
            <td>
                <asp:CheckBox ID="cbInvoice" runat="server" />
            </td>
            <td>
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
