<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Users" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="Scripts/ModalPopupWindow.js"></script>
    <script>
        var modalWin = new CreateModalPopUpObject();
        modalWin.SetLoadingImagePath("Images/Icons/loading.gif");
        modalWin.SetCloseButtonImagePath("Images/Icons/Close.png");
        //Uncomment below line to make look buttons as link
        modalWin.SetButtonStyle("background:none;border:none;textDecoration:underline;cursor:pointer");

        function ShowNewPage(vlezID, vlezCaption) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('Users_Edit.aspx?ID=' + vlezID, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowMessage() {
            modalWin.ShowMessage('This Modal Popup Window using Javascript', 200, 400, 'User Information');
        }

        function ShowMessageWithAction() {
            //ShowConfirmationMessage(message, height, width, title,onCloseCallBack, firstButtonText, onFirstButtonClick, secondButtonText, onSecondButtonClick);
            modalWin.ShowConfirmationMessage('This is confirmation window using Javascript', 200, 400, 'User Confirmation', null, 'Agree', Action1, 'Disagree', Action2);
        }

        function ShowMessageNoDragging() {
            modalWin.Draggable = false;
            modalWin.ShowMessage('You can not drag this window', 200, 400, 'User Information');

        }

        function Action1() {
            alert('Action1 is excuted');
            modalWin.HideModalPopUp();
        }

        function Action2() {
            alert('Action2 is excuted');
            modalWin.HideModalPopUp();
        }

        function EnrollNow(msg) {
            modalWin.HideModalPopUp();
            modalWin.ShowMessage(msg, 200, 400, 'User Information', null, null);
        }

        function EnrollLater() {
            modalWin.HideModalPopUp();
            modalWin.ShowMessage(msg, 200, 400, 'User Information', null, null);
        }

        function HideModalWindow() {
            modalWin.HideModalPopUp();
        }

        function ShowChildWindowValues(name, email, address, phone, zip) {
            var displayString = "<b>Values Of Child Window</b> <br><br>Name : " + name;
            displayString += "<br><br>Email : " + email;
            displayString += "<br><br>Address : " + address;
            displayString += "<br><br>Phone : " + phone;
            displayString += "<br><br>Zip : " + zip;
            var div = document.getElementById("divShowChildWindowValues");
            div.style.display = "";
            div.innerHTML = displayString;
        }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlTopMeni" runat="server" Visible="true">
    <table id="TabelaMeni" class="MeniTop">
        <tr>
            <td>
                <asp:Button ID="btnEdit" runat="server" Text="Edit selected account" CssClass="FilterButton" ValidationGroup="1" OnClick="btnEdit_Click" />
                <asp:Button ID="btnCreate" runat="server" Text="Create User account" CssClass="FilterButton" ValidationGroup="1" OnClick="btnCreate_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete the selected account" CssClass="FilterButton" ValidationGroup="1" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected user account?');"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table id="TabelaGlavna" class="GlavnaStandard">
        <tr>
            <td colspan="2">
                <div id="Filters" class="TabelaFilters">
                    <asp:Panel ID="pnlFilters" runat="server" DefaultButton="btnSearch">
                    <table id="TabFilters">                        
                        <tr>
                            <td>
                                Username:
                            </td>
                            <td>
                                <asp:TextBox ID="tbUserName" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td>
                                First Name:
                            </td>
                            <td>
                                <asp:TextBox ID="tbFirstName" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td>
                                Last Name:
                            </td>
                            <td>
                                <asp:TextBox ID="tbLastName" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td style="text-align:right">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="FilterButton" ValidationGroup="1" OnClick="btnSearch_Click" />                                
                            </td>
                            </tr>
                            <tr>
                            <td>
                                Status:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:DropDownList>                                
                            </td>
                            <td style="padding-left:1vw"></td>
                            
                        </tr>                    
                    </table>
                   </asp:Panel>
                </div>
            </td>
        </tr>
        <tr>
            <td>             
                   <div class="TabelaMalaGrid">                   
                   <asp:GridView ID="gvUsers" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="UserID" DataSourceID="dsUsers" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="15" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvUsers_RowDataBound" OnSelectedIndexChanged="gvUsers_SelectedIndexChanged" OnSorted="gvUsers_Sorted" OnPageIndexChanging="gvUsers_PageIndexChanging">
<AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="UserID" HeaderText="UserID" InsertVisible="False" ReadOnly="True" SortExpression="UserID" Visible="false" />
                           <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                           <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                           <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                           <asp:BoundField DataField="ContactPhone" HeaderText="Contact Phone" SortExpression="ContactPhone" Visible="false" />
                           <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                           <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" Visible="false"/>
                           <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" Visible="false"/>
                       </Columns>
<EditRowStyle CssClass="GridViewSelectedRow"></EditRowStyle>
                    <HeaderStyle CssClass="GridViewHeader"></HeaderStyle>
                    <PagerStyle CssClass="GridViewPager"></PagerStyle>
                    <RowStyle CssClass="GridViewRows"></RowStyle>
<SelectedRowStyle CssClass="GridViewSelectedRow"></SelectedRowStyle>
                   </asp:GridView> 
                       <asp:Label ID="lblNoRows" runat="server" Text="There are no results for the search!" Visible="false" CssClass="RequredField"></asp:Label>
                    <asp:SqlDataSource ID="dsUsers" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>
                   </div>             
            </td>
            <td style="vertical-align:top;">
                <div class="TabelaDetailsGrid">
                    <table>
                        <tr>
                            <td style="width:20vw">
                                <asp:GridView ID="gvDetails" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="dsDetails" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDetails_RowDataBound" OnSorted="gvDetails_Sorted">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="false" />
                                    <asp:BoundField DataField="UserName" HeaderText="Username" SortExpression="Username" />
                                    <asp:BoundField DataField="Type" HeaderText="Access Level" SortExpression="Type" />
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="lblNoDetails" runat="server" Text="There is no access for the selected user!" Visible="false" CssClass="RequredField"></asp:Label>
                                <asp:SqlDataSource ID="dsDetails" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>
                            </td>
                            <td>
                                <asp:Panel ID="pnlAddPermission" runat="server" Visible="false">
                                <table class="EditDetails">
                                    <tr>
                                        <td>
                                            Add Permission:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="TextBoxRoundedFilters"></asp:DropDownList>
                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="NormalButton" OnClick="btnAdd_Click" onclientclick="javascript:return confirm('Are you sure to add the permission?');" />
                                            <asp:Button ID="btnRemove" runat="server" Text="Remove selected" CssClass="NormalButton" OnClick="btnRemove_Click" onclientclick="javascript:return confirm('Are you sure to remove the selected permission?');" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate the selected user account" CssClass="NormalButton"  onclientclick="javascript:return confirm('Are you sure to deactivate the selected user?');" OnClick="btnDeactivate_Click" Visible="false" />
                                            <asp:Button ID="btnActivate" runat="server" Text="Activate the selected user account" CssClass="NormalButton"  onclientclick="javascript:return confirm('Are you sure to active the selected user?');" OnClick="btnActivate_Click" Visible="false"/>
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
    function ShowDialog() {
        var rtvalue = window.showModalDialog("Users_Edit.aspx");
        for (i = 0; i < rtvalue.length; i++) {
            alert(rtvalue[i]);
        }
    }
    function CloseFunction()
    {
        window.location.reload();
    }
    </script>
</asp:Content>

