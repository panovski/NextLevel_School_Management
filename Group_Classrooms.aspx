﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Group_Classrooms.aspx.cs" Inherits="Group_Classrooms" EnableEventValidation="false" %>
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
                   <div class="GridViewTabelaMalaGrid">       
                       All active classrooms:           
                   <div style="width:44vw; height:40vw; overflow: scroll;"> 
                   <asp:GridView ID="gvMain" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="ClassroomID" DataSourceID="dsMain" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="20" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvMain_RowDataBound" OnSelectedIndexChanged="gvMain_SelectedIndexChanged" OnSorted="gvMain_Sorted" OnPageIndexChanged="gvMain_PageIndexChanged" >
                    <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="ClassroomID" HeaderText="ClassroomID" InsertVisible="False" ReadOnly="True" SortExpression="ClassroomID" Visible="false" />
                           <asp:BoundField DataField="Name" HeaderText="Classroom name" SortExpression="Name" />
                           <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
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
                            Classroom name:
                        </td>
                        <td>
                            <asp:TextBox ID="tbClassroomName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvtbClassroomName" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbClassroomName" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description:
                        </td>
                        <td>
                            <asp:TextBox ID="tbDescription" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" ></asp:TextBox>
                        </td>
                        <td>
                            
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
                            <asp:Button ID="btnInsert" runat="server" Text="Insert new Classroom" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" />                
                            <asp:Button ID="btnDelete" runat="server" Text="Delete the Classroom" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected classroom?');" /> <br />

                            <asp:Label ID="lblInfo" runat="server" Text="" Visible="false" CssClass="RequredField"></asp:Label>
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
