<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="Payments.aspx.cs" Inherits="Payments" EnableEventValidation="false" %>
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
            modalWin.ShowURL('Payments_Edit.aspx?ID=' + vlezID + '&Type='+vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
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
                <asp:Button ID="btnEdit" runat="server" Text="Edit selected payment" CssClass="FilterButton" ValidationGroup="1" OnClick="btnEdit_Click" />
                <asp:Button ID="btnPreview" runat="server" Text="Preview the payment" CssClass="FilterButton" ValidationGroup="1" OnClick="btnPreview_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete the selected payment" CssClass="FilterButton" ValidationGroup="1" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected payment completely?');"/>                
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
                                Payment Number:
                            </td>
                            <td>
                                <asp:TextBox ID="tbPaymentNumber" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>                  
                            <td>
                                Student:
                            </td>
                            <td>
                                <asp:TextBox ID="tbStudent" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td>
                                Group:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:DropDownList>                               
                            </td>
                            <td style="padding-left:1vw">
                            </td>
                            
                            <td style="padding-left:1vw"></td>
                            <td style="text-align:right">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="FilterButton" ValidationGroup="1" OnClick="btnSearch_Click" />                                                                                                
                            </td>
                            </tr>
                            <tr>
                            <td>
                               Date From:
                            </td>
                            <td>
                                <asp:TextBox ID="tbDateFrom" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1" TextMode="Date" AutoPostBack="True"></asp:TextBox><br />
                                <asp:CompareValidator ID="cvtbDateFrom" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbDateFrom"  CssClass="RequredFieldSmall" ValidationGroup="1" ></asp:CompareValidator>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td>
                               Date To:
                            </td>
                            <td>
                                <asp:TextBox ID="tbDateTo" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1" TextMode="Date" AutoPostBack="True"></asp:TextBox><br />
                                <asp:CompareValidator ID="cvtbDateTo" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbDateTo"  CssClass="RequredFieldSmall" ValidationGroup="1" ></asp:CompareValidator>
                            </td>
                            <td style="padding-left:1vw"></td>
                            
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
                   <asp:GridView ID="gvMain" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="PaymentID" DataSourceID="dsMain" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="13" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvMain_RowDataBound" OnSelectedIndexChanged="gvMain_SelectedIndexChanged" OnSorted="gvMain_Sorted" OnPageIndexChanging="gvMain_PageIndexChanged" >
                    <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="PaymentID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="PaymentID" Visible="false" />
                           <asp:BoundField DataField="PaymentNumber" HeaderText="Number" SortExpression="PaymentNumber" ItemStyle-HorizontalAlign="Center" />                           
                           <asp:BoundField DataField="Ammount" HeaderText="Amount" SortExpression="Ammount" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                           <asp:BoundField DataField="StudentClient" HeaderText="Student/Client" SortExpression="StudentClient"/>
                           <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" Visible="false"/>
                           <asp:BoundField DataField="Teacher" HeaderText="Employee" SortExpression="Teacher" visible="false"/>
                           <asp:BoundField DataField="PaymentType" HeaderText="Type" SortExpression="PaymentType" ItemStyle-HorizontalAlign="Center"/>
                           <asp:CheckBoxField DataField="Transfered" HeaderText="Transfered" SortExpression="Transfered" ItemStyle-HorizontalAlign="Center"/>

                       </Columns>
<EditRowStyle CssClass="GridViewSelectedRow"></EditRowStyle>
                    <HeaderStyle CssClass="GridViewHeader"></HeaderStyle>
                    <PagerStyle CssClass="GridViewPager"></PagerStyle>
                    <RowStyle CssClass="GridViewRows"></RowStyle>
<SelectedRowStyle CssClass="GridViewSelectedRow"></SelectedRowStyle>
                   </asp:GridView> 
                       <asp:Label ID="lblNoRows" runat="server" Text="There are no results for the search!" Visible="false" CssClass="RequredField"></asp:Label>
                       <asp:linkbutton runat="server" ID="lnkDoubleClick" OnClick="lnkDoubleClick_Click" Visible="False">Double</asp:linkbutton>
                    <asp:SqlDataSource ID="dsMain" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>
                   </div>
                   </div>             
            </td>
            <td style="vertical-align:top;">  
                <div class="TabelaDetailsStandard">
                    <asp:Panel ID="pnlPayment" runat="server" Visible="False">
                    <table class="EditDetailsStandard" style="text-align:right; vertical-align:top;">
                        <tr>
                            <td>
                                Add Payment:<br />
                                <asp:TextBox ID="tbPaymentID" runat="server" Visible="false"></asp:TextBox>
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                Payment Number:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddPaymentNumber" runat="server" CssClass="TextBoxRoundedFilters" TabIndex="1" ValidationGroup="2" Enabled="False"></asp:TextBox>                                        
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvtbAddPaymentNumber" runat="server" ErrorMessage="!" ControlToValidate="tbAddPaymentNumber" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                            </td>
                            <td rowspan="8" style="vertical-align:top; padding-left:1vw">
                                <table>
                                     <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Student</asp:ListItem>
                                    <asp:ListItem Value="1">Additional Service</asp:ListItem>
                                    <asp:ListItem Value="2">Invoice</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlStudent" runat="server" Visible="true">
                                    <table>
                                        <tr>
                                            <td>
                                                Student:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbStudentsSearch" runat="server" CssClass="TextBoxSearchIcon" AutoPostBack="True" OnTextChanged="tbStudentsSearch_TextChanged" TabIndex="6" ValidationGroup="2"></asp:TextBox><br />
                                                <asp:DropDownList ID="ddlStudents" runat="server" CssClass="TextBoxRoundedFilters" AutoPostBack="True" OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged" OnTextChanged="ddlStudents_SelectedIndexChanged" TabIndex="7" ValidationGroup="2"></asp:DropDownList>                                            
                                                <asp:ImageButton ID="btnAllStudents" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnAllStudents_Click"/>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlStudents" runat="server" ErrorMessage="!" ControlToValidate="ddlStudents" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Group:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbAddGroupSearch" runat="server" CssClass="TextBoxSearchIcon" AutoPostBack="True" OnTextChanged="tbAddGroupSearch_TextChanged" Visible="false" TabIndex="8" ValidationGroup="2"></asp:TextBox><br />
                                                <asp:DropDownList ID="ddlAddGroup" runat="server" CssClass="TextBoxRoundedFilters" AutoPostBack="True" OnSelectedIndexChanged="ddlAddGroup_SelectedIndexChanged" TabIndex="9" ValidationGroup="2"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlAddGroup" runat="server" ErrorMessage="!" ControlToValidate="ddlAddGroup" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                Number of payments:<asp:TextBox ID="tbNumberOfPayments" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Remaining payments:<asp:TextBox ID="tbRemainingPayments" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Total Paid:<asp:TextBox ID="tbTotalPaid" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Remaining costs:<asp:TextBox ID="tbRemainingCosts" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlAdditionalService" runat="server" Visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                Customer:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCustomerSearch" runat="server" CssClass="TextBoxSearchIcon" AutoPostBack="True" OnTextChanged="tbCustomerSearch_TextChanged" ValidationGroup="2" ></asp:TextBox><br />
                                                <asp:DropDownList ID="ddlCustomers" runat="server" CssClass="TextBoxRoundedFilters" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged" ValidationGroup="2"></asp:DropDownList>                                            
                                                <asp:ImageButton ID="btnCustomersRefresh" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnCustomersRefresh_Click"/>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlCustomers" runat="server" ErrorMessage="!" ControlToValidate="ddlCustomers" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Service:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlService" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="2" OnSelectedIndexChanged="ddlService_SelectedIndexChanged"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlService" runat="server" ErrorMessage="!" ControlToValidate="ddlService" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                Number of payments:<asp:TextBox ID="tbCustomerPayments" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Total Paid:<asp:TextBox ID="tbCustomerPaid" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Remaining costs:<asp:TextBox ID="tbCustomerRemaining" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlInvoice" runat="server" Visible="false">
                                    <table>                                       
                                        <tr>
                                            <td>
                                                Group:                                                                                 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbInvoiceGroup" runat="server" CssClass="TextBoxSearchIcon" AutoPostBack="True" OnTextChanged="tbAddGroupSearch_TextChanged" Visible="false" TabIndex="8" ValidationGroup="2"></asp:TextBox><br />
                                                <asp:DropDownList ID="ddlInvoiceGroup" runat="server" CssClass="TextBoxRoundedFilters" AutoPostBack="True" OnSelectedIndexChanged="ddlInvoiceGroup_SelectedIndexChanged" TabIndex="9" ValidationGroup="2"></asp:DropDownList>                                            
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvddlInvoiceGroup" runat="server" ErrorMessage="!" ControlToValidate="ddlInvoiceGroup" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                                                <asp:ImageButton ID="btnRefreshInvoices" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefreshInvoices_Click"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                Number of payments:<asp:TextBox ID="tbInvoicePayments" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Remaining payments:<asp:TextBox ID="tbInvoiceRemainingPayments" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Total Paid:<asp:TextBox ID="tbInvoicePaid" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                                Remaining costs:<asp:TextBox ID="tbInvoiceRemainingCosts" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Label ID="lblInfo" runat="server" Text="" Visible="false" CssClass="RequredFieldSmall"></asp:Label>
                                <asp:Panel ID="pnlButtonsAdd" runat="server">
                                <asp:Button ID="btnAddPayment" runat="server" Text="Add Payment" CssClass="NormalButton" OnClick="btnAddPayment_Click" TabIndex="10" ValidationGroup="2" /><br />
                                <asp:Button ID="btnRecalculate" runat="server" Text="Recalculate" CssClass="NormalButton" OnClick="btnRecalculate_Click" TabIndex="11" ValidationGroup="2" Visible="False" /><br />                                
                                </asp:Panel>
                                <asp:Panel ID="pnlButtonsNew" runat="server">
                                <br />
                                <asp:Button ID="btnInserNewPayment" runat="server" Text="Create new Payment" CssClass="EditButton" OnClick="btnInserNewPayment_Click"  />
                                <br />
                                
                                </asp:Panel>
                            </td>
                        </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ammount:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddAmmount" runat="server" CssClass="TextBoxRoundedFilters" TextMode="Number" TabIndex="2" ValidationGroup="2" AutoPostBack="True" OnTextChanged="tbAddAmmount_TextChanged"></asp:TextBox>                                        
                            </td>
                            <td style="text-align:left">
                                <asp:RequiredFieldValidator ID="rfvtbAddAmmount" runat="server" ErrorMessage="!" ControlToValidate="tbAddAmmount" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                in words:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddAmmountWords" runat="server" CssClass="TextBoxRoundedFilters" TabIndex="3" ValidationGroup="2" ></asp:TextBox>                                        
                            </td>
                            <td style="text-align:left">
                                <asp:RequiredFieldValidator ID="rfvtbAddAmmountWords" runat="server" ErrorMessage="!" ControlToValidate="tbAddAmmountWords" CssClass="RequredField" ValidationGroup="2" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Account Number:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAccountNumber" runat="server" CssClass="TextBoxRoundedFilters" TabIndex="4" ValidationGroup="2" ></asp:TextBox>                                        
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Date of Payment:
                            </td>
                            <td>
                                <asp:TextBox ID="tbAddDateOfPayment" runat="server" CssClass="TextBoxRoundedFilters" TextMode="Date" TabIndex="5" ValidationGroup="2"></asp:TextBox>
                                
                            </td>
                            <td style="text-align:left">
                                <asp:RequiredFieldValidator ID="rfvtbAddDateOfPayment" runat="server" ErrorMessage="!" ControlToValidate="tbAddDateOfPayment" CssClass="RequredField" ValidationGroup="2"></asp:RequiredFieldValidator><br />                                
                                <asp:CompareValidator ID="cvtbAddDateOfPayment" runat="server" ErrorMessage="Wrong date!" Operator="DataTypeCheck" Type="Date" ControlToValidate="tbAddDateOfPayment"  CssClass="RequredFieldSmall" ></asp:CompareValidator>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td><br />                                
                                
                            </td><td></td><td></td>
                        </tr>
                       <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        <tr>
                            <td><br /></td><td></td><td></td>
                        </tr>
                        </table>
                        </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
    </ContentTemplate>    
    </asp:UpdatePanel>
    <table id="TabelaBottomn" class="MeniBottom">
        <tr>
            <td>                
                <asp:Button ID="btnPrintPayment" runat="server" Text="Print payment" CssClass="PrintButton" OnClick="btnPrintPayment_Click" ValidationGroup="3"   />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
    function ShowDialog() {
        var rtvalue = window.showModalDialog("Payments_Edit.aspx");
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

