<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="Groups.aspx.cs" Inherits="Groups" EnableEventValidation="false" %>
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
            modalWin.ShowURL('Groups_Edit.aspx?ID=' + vlezID + '&Type='+vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowGrTypes(vlezCaption, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('GroupTypes_Edit.aspx?Type=' + vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowTerm(vlezCaption, vlezId, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('GroupTerm_Edit.aspx?ID=' + vlezId+'&Type='+vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowClassRoom(vlezCaption, vlezId, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('Group_Classrooms.aspx?ID=' + vlezId + '&Type=' + vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowTemplates(vlezCaption) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('Templates_Edit.aspx?', 500, 800, vlezCaption, null, callbackFunctionArray);
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
                <asp:Button ID="btnEdit" runat="server" Text="Edit selected group" CssClass="FilterButton" ValidationGroup="1" OnClick="btnEdit_Click" />
                <asp:Button ID="btnCreate" runat="server" Text="Add new group" CssClass="FilterButton" ValidationGroup="1" OnClick="btnCreate_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete the selected group" CssClass="FilterButton" ValidationGroup="1" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected group completely?');" Visible="false"/>
                <asp:Button ID="btnAddTerm" runat="server" Text="Group Terms" CssClass="FilterButton" ValidationGroup="1" OnClick="btnAddTerm_Click"  />                                
                <asp:Button ID="bthEditGrType" runat="server" Text="Edit Group Types" CssClass="FilterButton" ValidationGroup="1" OnClick="bthEditGrType_Click" Visible="False"  />                                
                <asp:Button ID="btnEditClassroom" runat="server" Text="Edit Classrooms" CssClass="FilterButton" ValidationGroup="1" Visible="False" OnClick="btnEditClassroom_Click"  />                                
                <asp:Button ID="btnEditTemplates" runat="server" Text="Edit Templates" CssClass="FilterButton" ValidationGroup="1" Visible="False" OnClick="btnEditTemplates_Click"  />                                
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
                                Group Name:
                            </td>
                            <td>
                                <asp:TextBox ID="tbGroupName" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:TextBox>
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td>
                                Language
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:DropDownList>                               
                            </td>
                            <td style="padding-left:1vw">
                            </td>
                            <td>
                                Level:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLevel" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:DropDownList>                               
                            </td>
                            <td style="padding-left:1vw"></td>
                            <td style="text-align:right">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="FilterButton" ValidationGroup="1" OnClick="btnSearch_Click" />                                                                                                
                            </td>
                            </tr>
                            <tr>
                            <td>
                               Teacher:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="1"></asp:DropDownList>                               
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
                   <asp:GridView ID="gvMain" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="dsMain" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="22" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvMain_RowDataBound" OnSelectedIndexChanged="gvMain_SelectedIndexChanged" OnSorted="gvMain_Sorted" OnPageIndexChanged="gvMain_PageIndexChanged" >
                    <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="false" />
                           <asp:BoundField DataField="GroupName" HeaderText="Group" SortExpression="GroupName" />
                           <asp:BoundField DataField="Language" HeaderText="Language" SortExpression="Language" />
                           <asp:BoundField DataField="LevelDescription" HeaderText="Level" SortExpression="LevelDescription"/>
                           <asp:BoundField DataField="StudentsNo" HeaderText="Students" SortExpression="StudentsNo" ItemStyle-HorizontalAlign="Center"/>
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
                    <asp:SqlDataSource ID="dsMain" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>
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
                                        Students:
                                            <asp:linkbutton runat="server" ID="lnkDoubleClick" OnClick="lnkDoubleClick_Click" Visible="False">Double</asp:linkbutton>
                                            <asp:linkbutton runat="server" ID="lnkSingleClick" Visible="False">Single</asp:linkbutton>
                                        </td>
                                    </tr>
                                </table> 
                                <div style="width:23.9vw; height:11.3vw; overflow: scroll;">
                                <asp:GridView ID="gvDetails" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="dsDetails" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDetails_RowDataBound" OnSorted="gvDetails_Sorted" OnSelectedIndexChanged="gvDetails_SelectedIndexChanged" OnPageIndexChanged="gvDetails_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="false" />
                                    <asp:BoundField DataField="Name" HeaderText="Student" SortExpression="Name" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="false" />
                                    <asp:BoundField DataField="Discount" HeaderText="Discount" SortExpression="Discount" DataFormatString="{0}%"  ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="TotalCost" DataFormatString="{0}ден" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TotalPaid" HeaderText="Total Paid" SortExpression="TotalPaid" DataFormatString="{0}ден" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TotalRemain" HeaderText="Total Remain" SortExpression="TotalRemain" DataFormatString="{0}ден" ItemStyle-HorizontalAlign="Right" />
                                        <asp:CheckBoxField DataField="Transfered" HeaderText="Transfered" SortExpression="Transfered" ItemStyle-HorizontalAlign="Center"/>
                                    </Columns>
                                </asp:GridView>
                                </div>
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
                                            <asp:TextBox ID="tbStudentsSearch" runat="server" CssClass="TextBoxSearchIcon" AutoPostBack="True" OnTextChanged="tbStudentsSearch_TextChanged"></asp:TextBox>
                                            <asp:DropDownList ID="ddlStudents" runat="server" CssClass="TextBoxRoundedFilters"></asp:DropDownList>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Discount:<br />
                                            <asp:TextBox ID="tbDiscount" runat="server" TextMode="Number" CssClass="TextBoxNumber"></asp:TextBox>%
                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="NormalButton" OnClick="btnAdd_Click" onclientclick="javascript:return confirm('Are you sure to add the student into the group?');" />
                                            <asp:Button ID="btnRemove" runat="server" Text="Remove selected" CssClass="NormalButton" OnClick="btnRemove_Click" onclientclick="javascript:return confirm('Are you sure to remove the student from selected group?');" />                                            
                                            <br /><br />
                                            <asp:Label ID="lblAlreadyAdded" runat="server" Text="The student is already member of the group!" CssClass="InfoMessage" Visible="False"></asp:Label>                                            
                                        </td>
                                </table>
                                </asp:Panel>                                
                                <asp:Button ID="btnPreviewDetails" runat="server" Text="Selected Group Details" CssClass="NormalButton" OnClick="lnkDoubleClick_Click" />
                                <asp:Button ID="btnTermsDetails" runat="server" Text="View Group Terms" CssClass="NormalButton" OnClick="btnTermsDetails_Click"  />
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
                                            <asp:Button ID="btnPrintPayment" runat="server" Text="Print payment" CssClass="PrintButton" OnClick="btnPrintPayment_Click"/>                                            
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
                                Certificates:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                                <div style="width:23.9vw; height:11.5vw; overflow: scroll">
                                <asp:GridView ID="gvCertificates" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="CertificateID" DataSourceID="dsCertificates" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvCertificates_RowDataBound" OnSorted="gvCertificates_Sorted">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="CertificateID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="CertificateID" Visible="false" />
                                        <asp:BoundField DataField="RegNo" HeaderText="Reg.Number" SortExpression="RegNo" />
                                        <asp:BoundField DataField="Student" HeaderText="Student" SortExpression="Student" />
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsCertificates" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                            </td>                             
                            <td style="vertical-align:top">
                                <asp:ImageButton ID="imgbtnRefresh" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" OnClick="imgbtnRefresh_Click" CssClass="RefreshButton"/>
                                <br /><br />
                                    Certificate Template:<asp:DropDownList ID="ddlTemplateCertificate" runat="server" CssClass="TextBoxRoundedFilters" ValidationGroup="2"></asp:DropDownList>                               
                                    <asp:Button ID="btnPrintSelectedCertificate" runat="server" Text="Print selected Certificate" CssClass="PrintButton" OnClick="btnPrintSelectedCertificate_Click" /><br />
                                    <asp:Button ID="btnPrintAllCertificates" runat="server" Text="Print all Certificates" CssClass="PrintButton" OnClick="btnPrintAllCertificates_Click" /><br /><br />
                                    <asp:Button ID="btnCreateAllCertificates" runat="server" Text="Create all Certificates" CssClass="NormalButton" OnClick="btnCreateAllCertificates_Click"  onclientclick="javascript:return confirm('Are you sure to create certificates for everyone?');"/>
                                    <asp:Button ID="btnDeleteAllCertificates" runat="server" Text="Delete all Certificates" CssClass="NormalButton" onclientclick="javascript:return confirm('Are you sure to delete all certificates in this group?');" OnClick="btnDeleteAllCertificates_Click" Visible="False"/>
                            </td>                           
                        </tr>
                        </table>
                </div>

            </td>
        </tr>
    </table>
    <script type="text/javascript">
    function ShowDialog() {
        var rtvalue = window.showModalDialog("Groups_Edit.aspx");
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

