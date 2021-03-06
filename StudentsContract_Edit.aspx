﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentsContract_Edit.aspx.cs" Inherits="StudentsContract_Edit" EnableEventValidation="false" %>
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
                parent.CloseFunction(); }
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
                   <div class="GridViewTabelaMalaGrid">       
                       All Contracts:           
                   <div style="width:44vw; height:40vw; overflow: scroll;"> 
                   <asp:GridView ID="gvMain" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="ContractID" DataSourceID="dsMain" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="20" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvMain_RowDataBound" OnSelectedIndexChanged="gvMain_SelectedIndexChanged" OnSorted="gvMain_Sorted" >
                    <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="ContractID" HeaderText="ContractID" InsertVisible="False" ReadOnly="True" SortExpression="ContractID" Visible="false" />
                           <asp:BoundField DataField="Course" HeaderText="Course" SortExpression="Course"/>
                           <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" DataFormatString="{0:d}" />
                           <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate" DataFormatString="{0:d}"/>
                           <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" Visible="false"/>
                           <asp:BoundField DataField="CreatedByUser" HeaderText="Created By" SortExpression="CreatedByUser" ReadOnly="true" Visible="false"/>
                       </Columns>
<EditRowStyle CssClass="GridViewSelectedRow"></EditRowStyle>
                    <HeaderStyle CssClass="GridViewHeader"></HeaderStyle>
                    <PagerStyle CssClass="GridViewPager"></PagerStyle>
                    <RowStyle CssClass="GridViewRows"></RowStyle>
<SelectedRowStyle CssClass="GridViewSelectedRow"></SelectedRowStyle>
                   </asp:GridView> 
                      </div>
                       <asp:Label ID="lblNoRows" runat="server" Text="There are no results for the search!" Visible="false" CssClass="RequredField"></asp:Label>
                    <asp:SqlDataSource ID="dsMain" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>
                   </div>             
            </td>

            <td style="vertical-align:top; text-align:left">
                <table style="margin-left:3vw;">
                    <tr>
                        <td>
                            Course:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCourse" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvddlCourse" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="ddlCourse" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Start Date:
                        </td>
                        <td>
                            <asp:TextBox ID="tbStartDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" onkeyup="javascript:Date('#tbStartDate')" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvtbStartDate" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbStartDate" ValidationGroup="1" ></asp:RequiredFieldValidator><br />
                            <asp:RegularExpressionValidator ID="revtbStartDate" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02./((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="tbStartDate"  CssClass="RequredField" ValidationGroup="1" ></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           End Date:
                        </td>
                        <td>
                            <asp:TextBox ID="tbEndDate" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" onkeyup="javascript:Date('#tbEndDate')" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="revtbEndDate" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02./((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="tbEndDate"  CssClass="RequredField" ValidationGroup="1" ></asp:RegularExpressionValidator>
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
                            <asp:RegularExpressionValidator ID="revtbCreatedDate" runat="server" ErrorMessage="Wrong date! (dd.MM.yyyy)" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02./((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="tbCreatedDate"  CssClass="RequredField" ValidationGroup="1" ></asp:RegularExpressionValidator>
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
                            <asp:Button ID="btnSave" runat="server" Text="Save changes" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="1" Visible="False"  />               
                            <asp:Button ID="btnInsert" runat="server" Text="Create New Contract" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" />                
                            <asp:Button ID="btnDelete" runat="server" Text="Delete the Contract" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected contract?');" />                
                        </td>
                    </tr>  
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblInfo" runat="server" Text="" CssClass="RequredField" Visible="False"></asp:Label>
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
