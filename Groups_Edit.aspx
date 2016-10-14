<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Groups_Edit.aspx.cs" Inherits="Groups_Edit" EnableEventValidation="true" %>
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
                <asp:Button ID="btnInsert" runat="server" Text="Insert New Group" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" Visible="false"/>                
            </td>
        </tr>
        <tr>
            <td>
                Group Type:
            </td>
            <td>
                <asp:DropDownList ID="ddlCourse" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
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
                <asp:TextBox ID="tbStartDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Date" AutoPostBack="True"></asp:TextBox>                
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbStartDate" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbStartDate" ValidationGroup="1"></asp:RequiredFieldValidator><br />
                <asp:CompareValidator ID="cvtbStartDate" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbStartDate"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                
            </td>
        </tr>
        <tr>
            <td>
                End Date:
            </td>
            <td>
                <asp:TextBox ID="tbEndDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Date" AutoPostBack="True"></asp:TextBox>                
            </td>
            <td>
                <asp:CompareValidator ID="cvtbEndDate" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbEndDate"  CssClass="RequredField" ValidationGroup="1" ></asp:CompareValidator>                
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
                <asp:RequiredFieldValidator ID="rfvtbNoClasses" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbNoClasses" ValidationGroup="1"></asp:RequiredFieldValidator>                
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
                <asp:RequiredFieldValidator ID="rfvtbNoPayments" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbNoPayments" ValidationGroup="1"></asp:RequiredFieldValidator>                
            </td>
        </tr>
         <tr>
            <td>
                Cost:
            </td>
            <td>
                <asp:TextBox ID="tbCost" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Number"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvtbCost" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbCost" ValidationGroup="1"></asp:RequiredFieldValidator>                
            </td>
        </tr>
        <tr>
            <td>
                Teacher:
            </td>
            <td>
                <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
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
