<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupTerm_Edit.aspx.cs" Inherits="GroupTerm_Edit" EnableEventValidation="false"%>

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
                       All terms that selected group have:           
                   <div style="width:44vw; height:40vw; overflow: scroll;"> 
                   <asp:GridView ID="gvMain" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="TerminID" DataSourceID="dsMain" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="20" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvMain_RowDataBound" OnSelectedIndexChanged="gvMain_SelectedIndexChanged" OnSorted="gvMain_Sorted" >
                    <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="TerminID" HeaderText="TerminID" SortExpression="TerminID" InsertVisible="False" ReadOnly="True" Visible="false" />
                           <asp:BoundField DataField="DayDesc" HeaderText="Day" SortExpression="DayDesc" />
                           <asp:BoundField DataField="TimeStart" HeaderText="Time From" SortExpression="TimeStart" ItemStyle-HorizontalAlign="Center"/>
                           <asp:BoundField DataField="TimeEnd" HeaderText="Time To" SortExpression="TimeEnd" ItemStyle-HorizontalAlign="Center"/>
                           <asp:BoundField DataField="Classroom" HeaderText="Classroom" SortExpression="Classroom" />
                           <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" Visible="false"/>
                           <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" Visible="false"/>
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
            <td>
                                <table class="EditForma">
                    <tr>
                        <td>
                            Termin:<asp:TextBox ID="tbTerminId" runat="server" Visible="false"></asp:TextBox>
                        </td>
                        <td></td><td></td>
                    </tr>
                    <tr>
                        <td>
                            Day:
                        </td>
                        <td>                            
                            <asp:DropDownList ID="ddlTerminDay" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            From:
                        </td>
                        <td>
                            <asp:TextBox ID="tbTerminFrom" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Time"></asp:TextBox>                            
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvtbTerminFrom" runat="server" ErrorMessage="!" CssClass="RequredField" ControlToValidate="tbTerminFrom" ValidationGroup="1"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revtbTerminFrom" runat="server" ErrorMessage="00:00-23:59" CssClass="RequredField" ControlToValidate="tbTerminFrom" ValidationGroup="1" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            To:
                        </td>
                        <td>
                           <asp:TextBox ID="tbTerminTo" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" TextMode="Time"></asp:TextBox>                            
                        </td>
                        <td>
                           <asp:RequiredFieldValidator ID="rfvtbTerminTo" runat="server" ErrorMessage="!" CssClass="RequredField" ControlToValidate="tbTerminTo" ValidationGroup="1"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revrtbTerminTo" runat="server" ErrorMessage="00:00-23:59" CssClass="RequredField" ControlToValidate="tbTerminTo" ValidationGroup="1" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Classroom:
                        </td>
                        <td>                            
                            <asp:DropDownList ID="ddlClassroom" runat="server" CssClass="TextBoxRoundedEdit"></asp:DropDownList>                
                        </td>
                        <td></td>
                    </tr>
                   <tr>
                       <td>
                       </td>
                       <td></td>
                       <td>
                           <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnAdd_Click"/>               
                          
                       </td>
                   </tr>
                   <tr>
                       <td>
                       </td>
                       <td></td>
                       <td>
                           <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnRemove_Click" />             
                       </td>
                   </tr>
                   <tr>
                       <td colspan="3">
                           <asp:Label ID="lblInfo" runat="server" Text="" Visible="false" CssClass="RequredField"></asp:Label>
                       </td>
                   </tr>
                </table>
            </td>
         </tr>
        <tr>
            <td></td>
            <td>
                
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
