<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="Students.aspx.cs" Inherits="Students" EnableEventValidation="false"%>
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
            modalWin.ShowURL('Students_Edit.aspx?ID=' + vlezID + '&Type='+vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowTerm(vlezCaption, vlezId, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('GroupTerm_Edit.aspx?ID=' + vlezId + '&Type=' + vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowContract(vlezId,vlezCaption, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('StudentsContract_Edit.aspx?ID=' + vlezId + '&Type=' + vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowTransfer(StudentID, GroupID, vlezCaption) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('StudentsTransfer_Edit.aspx?StudentID=' + StudentID + '&GroupID=' + GroupID, 300, 300, vlezCaption, null, callbackFunctionArray);
        }

        function ShowTransferDetails(StudentID, GroupID, vlezCaption) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('TransferDetails.aspx?StudentID=' + StudentID + '&GroupID=' + GroupID, 300, 300, vlezCaption, null, callbackFunctionArray);
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
                <asp:Button ID="btnEdit" runat="server" Text="Edit selected Student" CssClass="FilterButton" ValidationGroup="1" OnClick="btnEdit_Click" />
                <asp:Button ID="btnCreate" runat="server" Text="Add new Student" CssClass="FilterButton" ValidationGroup="1" OnClick="btnCreate_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete the selected Student" CssClass="FilterButton" ValidationGroup="1" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected Student completely?');" Visible="false"/>
                <asp:Button ID="btnCreateContract" runat="server" Text="Create Contract" CssClass="FilterButton" ValidationGroup="1" OnClick="btnCreateContract_Click" />
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
                            <td style="padding-left:1vw">
                            </td>
                            <td>
                                SSN:
                            </td>
                            <td>
                                <asp:TextBox ID="tbSSN" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
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
                   <asp:GridView ID="gvStudents" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="StudentID" DataSourceID="dsStudents" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="20" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvStudents_RowDataBound" OnSelectedIndexChanged="gvStudents_SelectedIndexChanged" OnSorted="gvStudents_Sorted" OnRowCommand="gvStudents_RowCommand" OnPageIndexChanging="gvStudents_PageIndexChanging">
<AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="StudentID" HeaderText="StudentID" InsertVisible="False" ReadOnly="True" SortExpression="StudentID" Visible="false" />
                           <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                           <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                           <asp:BoundField DataField="SocialNumber" HeaderText="SSN" SortExpression="SocialNumber" Visible="false" />
                           <asp:BoundField DataField="ContactPhone" HeaderText="Contact Phone" SortExpression="ContactPhone" />
                           <asp:BoundField DataField="StatusDesc" HeaderText="Status" SortExpression="StatusDesc" />
                           <asp:BoundField DataField="DateOfBirth" HeaderText="Date of birth" SortExpression="DateOfBirth" DataFormatString="{0:d}"/>
                           <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" Visible="false"/>
                           <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" Visible="false" />
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
                    <asp:SqlDataSource ID="dsStudents" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>
                   </div>
                   </div>             
            </td>
            <td style="vertical-align:top;">
                <div class="TabelaDetailsGrid">
                    <table>
                        <tr>
                            <td style="width:20vw; vertical-align:top;">
                                 <table class="EditDetails">
                                    <tr>
                                        <td>
                                        Member of group:
                                            <asp:linkbutton runat="server" ID="lnkDoubleClick" OnClick="lnkDoubleClick_Click" Visible="False">Double</asp:linkbutton>
                                            <asp:linkbutton runat="server" ID="lnkSingleClick" Visible="False" OnClick="lnkSingleClick_Click">Single</asp:linkbutton>
                                        </td>
                                    </tr>
                                </table> 
                                <div style="width:23.9vw; height:11.5vw; overflow: scroll">
                                <asp:GridView ID="gvDetails" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="dsDetails" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDetails_RowDataBound" OnSorted="gvDetails_Sorted" OnSelectedIndexChanged="gvDetails_SelectedIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="false" />
                                    <asp:BoundField DataField="Grupa" HeaderText="Group" SortExpression="Grupa" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="false" />
                                    <asp:BoundField DataField="Discount" HeaderText="Discount" SortExpression="Discount" DataFormatString="{0}%"/>
                                    <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="TotalCost" DataFormatString="{0}ден" />
                                    <asp:CheckBoxField DataField="Transfered" HeaderText="Transfered" SortExpression="Transfered" ItemStyle-HorizontalAlign="Center"/>
                                    </Columns>
                                </asp:GridView>
                                </div>
                                <asp:Label ID="lblNoDetails" runat="server" Text="There is no access for the selected user!" Visible="false" CssClass="RequredField"></asp:Label>
                                <asp:SqlDataSource ID="dsDetails" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                            </td>
                            <td style="vertical-align:top">
                                <asp:Panel ID="pnlAddPermission" runat="server" Visible="false">
                                <table class="EditDetails">
                                    <tr>
                                        <td>
                                            Include into Group:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlCourse" runat="server" CssClass="TextBoxRoundedFilters"></asp:DropDownList>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Discount:<br />
                                            <asp:TextBox ID="tbDiscount" runat="server" TextMode="Number" CssClass="TextBoxNumber"></asp:TextBox>%
                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="NormalButton" OnClick="btnAdd_Click" onclientclick="javascript:return confirm('Are you sure to add the group for the student?');" />
                                            <asp:Button ID="btnRemove" runat="server" Text="Remove selected" CssClass="NormalButton" OnClick="btnRemove_Click" onclientclick="javascript:return confirm('Are you sure to remove the student from selected group?');" />
                                            <br /><br />
                                            <asp:Label ID="lblAlreadyAdded" runat="server" Text="The student is already member of the group!" CssClass="InfoMessage" Visible="False"></asp:Label>
                                            <asp:Button ID="btnDeactivate" runat="server" Text="Change the Student to passive" CssClass="NormalButton"  onclientclick="javascript:return confirm('Are you sure to deactivate the selected user?');" OnClick="btnDeactivate_Click" Visible="false" />
                                            <asp:Button ID="btnActivate" runat="server" Text="Change the Student to active" CssClass="NormalButton"  onclientclick="javascript:return confirm('Are you sure to active the selected user?');" OnClick="btnActivate_Click" Visible="false"/>
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                                <asp:Button ID="btnPreviewDetails" runat="server" Text="Selected Student Details" CssClass="NormalButton" OnClick="lnkDoubleClick_Click" />
                                <asp:Button ID="btnTermsDetails" runat="server" Text="View Group Terms" CssClass="NormalButton" OnClick="btnTermsDetails_Click"  />
                                <asp:Button ID="btnTransfer" runat="server" Text="Transfer to another group" CssClass="NormalButton" OnClick="btnTransfer_Click"/>
                                <asp:Button ID="btnTransferDetails" runat="server" Text="View Transfer Details" CssClass="NormalButton" OnClick="btnTransferDetails_Click"/>
                            </td>
                        </tr>
                    </table>
                
                </div>

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
                                    <asp:CheckBoxField DataField="Transfered" HeaderText="Transfered" SortExpression="Transfered" ItemStyle-HorizontalAlign="Center"/>
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsPayments" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                                <asp:Label ID="lblNoPayments" runat="server" Text="" Visible="false" CssClass="RequredFieldSmall"></asp:Label>
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
                                            Remaining payments:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbRemainingPayments" runat="server" Enabled="false" CssClass="TextBoxNumber"></asp:TextBox><br />
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

                <div class="TabelaDetailsGrid">
                    <table >
                        <tr>
                            <td class="EditDetails">
                                Certificates and Contracts:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                                <div style="width:16vw; height:11.5vw; overflow: scroll">
                                <asp:GridView ID="gvCertificates" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="CertificateID" DataSourceID="dsCertificates" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvCertificates_RowDataBound" OnSorted="gvCertificates_Sorted">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="CertificateID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="CertificateID" Visible="false" />
                                        <asp:BoundField DataField="RegNo" HeaderText="Reg.Number" SortExpression="RegNo" />
                                        <asp:BoundField DataField="Course" HeaderText="Course" SortExpression="Course" />
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsCertificates" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                            </td> 
                            <td>
                                <div style="width:16vw; height:11.5vw; overflow: scroll">
                                <asp:GridView ID="gvContracts" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ContractID" DataSourceID="dsContracts" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvContracts_RowDataBound" OnSorted="gvContracts_Sorted">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="ContractID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ContractID" Visible="false" />
                                        <asp:BoundField DataField="Course" HeaderText="Course" SortExpression="Course"/>
                                        <asp:BoundField DataField="StartDate" HeaderText="Contract Start Date" SortExpression="StartDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="EndDate" HeaderText="Contract End Date" SortExpression="EndDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center"/>
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsContracts" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                            </td>
                            <td style="vertical-align:top" class="EditDetails">
                                <asp:ImageButton ID="imgbtnRefresh" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" OnClick="imgbtnRefresh_Click" CssClass="RefreshButton"/>
                                <br /><br />
                                    Cert. Template:<br /></br><asp:DropDownList ID="ddlTemplateCertificate" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="2" style="width:10vw"></asp:DropDownList>                               
                                    
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
                <asp:Button ID="btnPrintSelectedCertificate" runat="server" Text="Print selected Certificate" CssClass="PrintButton" OnClick="btnPrintSelectedCertificate_Click" />
                <asp:Button ID="btnPrintSelectedContract" runat="server" Text="Print selected Contract" CssClass="PrintButton" OnClick="btnPrintSelectedContract_Click" />                                    
            </td>
        </tr>
    </table>
    <script type="text/javascript">
    function ShowDialog() {
        var rtvalue = window.showModalDialog("Students_Edit.aspx");
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

