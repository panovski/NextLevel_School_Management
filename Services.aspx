<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="Services.aspx.cs" Inherits="Services" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script src="Scripts/ModalPopupWindow.js"></script>
    <script>
        var modalWin = new CreateModalPopUpObject();
        modalWin.SetLoadingImagePath("Images/Icons/loading.gif");
        modalWin.SetCloseButtonImagePath("Images/Icons/Close.png");
        //Uncomment below line to make look buttons as link
        modalWin.SetButtonStyle("background:none;border:none;textDecoration:underline;cursor:pointer");

        function ShowNewPage(vlezID, vlezCaption, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('Services_Edit.aspx?ID=' + vlezID + '&Type=' + vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowServiceTypes(vlezCaption) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('ServiceType_Edit.aspx?', 500, 800, vlezCaption, null, callbackFunctionArray);
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
                <asp:Button ID="btnEdit" runat="server" Text="Edit selected service" CssClass="FilterButton" ValidationGroup="1" OnClick="btnEdit_Click" />
                <asp:Button ID="btnCreate" runat="server" Text="Add new service" CssClass="FilterButton" ValidationGroup="1" OnClick="btnCreate_Click" />
                <asp:Button ID="btnServiceType" runat="server" Text="Service Types" CssClass="FilterButton" ValidationGroup="1" OnClick="btnServiceType_Click" Visible="false" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete the selected service" CssClass="FilterButton" ValidationGroup="1" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected service completely?');" Visible="false"/>                
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" id="upData">
    <ContentTemplate>
    <table id="TabelaGlavna" class="GlavnaStandard">
        <tr>
            <td colspan="2">
                <div id="Filters" class="TabelaFilters">
                    <asp:Panel ID="pnlFilters" runat="server" DefaultButton="btnSearch">
                    <table id="TabFilters">                        
                        <tr>                            
                            <td>
                                Service Name:
                            </td>
                            <td>
                                <asp:TextBox ID="tbServiceName" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td>
                                Customer:
                            </td>
                            <td>
                                <asp:TextBox ID="tbCustomer" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw">
                            </td>
                            <td>
                                Employee:
                            </td>
                            <td>
                                <asp:TextBox ID="tbEmployee" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td style="text-align:right">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="FilterButton" ValidationGroup="1" OnClick="btnSearch_Click" />                                                                                                
                            </td>
                            </tr>
                            <tr>
                            <td>                               
                            </td>
                            <td>                                
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td colspan="7" style="text-align:right">                                
                            </td>
                        </tr>                    
                    </table>
                   </asp:Panel>
                </div>
            </td>
        </tr>
        <tr style="vertical-align:top">
            <td>             
                   <div class="TabelaMalaGrid">                   
                   <div style="width:25.5vw; overflow: scroll">   
                   <asp:GridView ID="gvMain" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="ServiceID" DataSourceID="dsMain" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="13" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvMain_RowDataBound" OnSelectedIndexChanged="gvMain_SelectedIndexChanged" OnSorted="gvMain_Sorted" OnRowCommand="gvMain_RowCommand" OnPageIndexChanging="gvMain_PageIndexChanging">
<AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="ServiceID" HeaderText="ServiceID" InsertVisible="False" ReadOnly="True" SortExpression="ServiceID" Visible="false" />
                           <asp:BoundField DataField="ServiceName" HeaderText="Service" SortExpression="ServiceName" />
                           <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                           <asp:BoundField DataField="Employee" HeaderText="Employee" SortExpression="Employee" />
                           <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                       </Columns>
<EditRowStyle CssClass="GridViewSelectedRow"></EditRowStyle>
                    <HeaderStyle CssClass="GridViewHeader"></HeaderStyle>
                    <PagerStyle CssClass="GridViewPager"></PagerStyle>
                    <RowStyle CssClass="GridViewRows"></RowStyle>
<SelectedRowStyle CssClass="GridViewSelectedRow"></SelectedRowStyle>
                   </asp:GridView> 
                       <asp:Label ID="lblNoRows" runat="server" Text="There are no results for the search!" Visible="false" CssClass="RequredField"></asp:Label>
                    <asp:SqlDataSource ID="dsMain" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>
                   </div>  
                   </div>           
            </td>
            <td style="vertical-align:top;">
                <div class="TabelaDetailsGrid">
                    <table >
                        <tr>
                            <td class="EditDetails">
                                Payments:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                                <div style="width:23.9vw; height:11.5vw; overflow: scroll">
                                <asp:GridView ID="gvPayments" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="PaymentID" DataSourceID="dsPayments" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvPayments_RowDataBound" OnSorted="gvPayments_Sorted">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                    <asp:BoundField DataField="PaymentID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="PaymentID" Visible="false" />
                                    <asp:BoundField DataField="PaymentNumber" HeaderText="Number" SortExpression="PaymentNumber" />
                                    <asp:BoundField DataField="Ammount" HeaderText="Amount" SortExpression="Ammount" DataFormatString="{0}ден" />
                                    <asp:BoundField DataField="DateOfPayment" HeaderText="Date" SortExpression="DateOfPayment" />
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsPayments" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                            </td> 
                            <td>
                                <table style="text-align:right" class="EditDetails">
                                    <tr>
                                        <td>
                                            Number of payments:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbNumberOfPayments" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td>
                                            Total Paid:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbTotalPaid" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Remaining costs:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbRemainingCosts" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>                           
                        </tr>
                        </table>
                </div>                

            </td>
        </tr>
    </table>
    </ContentTemplate>    
    </asp:UpdatePanel>
    <table id="TabelaBottomn" class="MeniBottom">
        <tr>
            <td>                
                <asp:Button ID="btnPrintPayment" runat="server" Text="Print payment" CssClass="PrintButton" OnClick="btnPrintPayment_Click"/>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function ShowDialog() {
            var rtvalue = window.showModalDialog("Services_Edit.aspx");
            for (i = 0; i < rtvalue.length; i++) {
                alert(rtvalue[i]);
            }
        }
        function CloseFunction() {
            window.location.reload();
        }
    </script>
</asp:Content>

