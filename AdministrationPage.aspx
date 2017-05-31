<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="AdministrationPage.aspx.cs" Inherits="AdministrationPage" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="Scripts/ModalPopupWindow.js"></script>
    <script>
        var modalWin = new CreateModalPopUpObject();
        modalWin.SetLoadingImagePath("Images/Icons/loading.gif");
        modalWin.SetCloseButtonImagePath("Images/Icons/Close.png");
        //Uncomment below line to make look buttons as link
        modalWin.SetButtonStyle("background:none;border:none;textDecoration:underline;cursor:pointer");

        function ShowGrTypes(vlezCaption, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('GroupTypes_Edit.aspx?Type=' + vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }
        
        function ShowClassRoom(vlezCaption, vlezId, vlezType) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('Group_Classrooms.aspx?ID=' + vlezId + '&Type=' + vlezType, 500, 800, vlezCaption, null, callbackFunctionArray);
        }

        function ShowTemplates(vlezCaption) {
            var callbackFunctionArray = new Array(EnrollNow, EnrollLater);
            modalWin.ShowURL('Templates_Edit.aspx?', 500, 800, vlezCaption, null, callbackFunctionArray);
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
    <asp:ScriptManager ID="scriptmanager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" id="upData">
    <ContentTemplate>
    <table class="AdminPage">
        <tr>
            <td colspan="10">
                Administration Page<br /> 
            </td>
        </tr>
        <tr style="vertical-align:top">
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnAccessPermission" runat="server" ImageUrl="~/Images/Icons/AccessPermission.png" ToolTip="Give access to user" CssClass="imageButton" PostBackUrl="~/Users.aspx"/>
                <br /> 
                User's Access
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnManageEmployee" runat="server" ImageUrl="~/Images/Icons/Employees.png"  ToolTip="Manage Employees" CssClass="imageButton" PostBackUrl="~/Employees.aspx" />
                <br /> 
                Employees
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnManageStudents" runat="server" ImageUrl="~/Images/Icons/Students.png"  ToolTip="Manage Students" CssClass="imageButton" PostBackUrl="~/Students.aspx" />
                <br /> 
                Students
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnManageGroups" runat="server" ImageUrl="~/Images/Icons/StudentsGroup.png"  ToolTip="Manage Student Groups" CssClass="imageButton" PostBackUrl="~/Groups.aspx" />
                <br /> 
                Groups
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnGroupTypes" runat="server" ImageUrl="~/Images/Icons/GroupTypes.png"  ToolTip="Manage Student Groups" CssClass="imageButton" OnClick="ibtnGroupTypes_Click" />
                <br /> 
                Group Types
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnServiceTypes" runat="server" ImageUrl="~/Images/Icons/ServiceTypes.png"  ToolTip="Manage Student Groups" CssClass="imageButton" OnClick="ibtnServiceTypes_Click" />
                <br /> 
                Service Types
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="itbnClassrooms" runat="server" ImageUrl="~/Images/Icons/Classrooms.png"  ToolTip="Manage Student Groups" CssClass="imageButton" OnClick="itbnClassrooms_Click" />
                <br /> 
                Classrooms
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnTemplates" runat="server" ImageUrl="~/Images/Icons/Templates.png"  ToolTip="Manage Student Groups" CssClass="imageButton" OnClick="ibtnTemplates_Click" />
                <br /> 
                Templates
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnChangePassword" runat="server" ImageUrl="~/Images/Icons/ChangePassword.png"  ToolTip="Change Password" CssClass="imageButton" PostBackUrl="~/ChangePassword.aspx" />
                <br /> 
                Change Password
            </td>
            <td style="text-align:center">
                <asp:ImageButton ID="ibtnLogout" runat="server" ImageUrl="~/Images/Icons/Logout.png"  ToolTip="Logout" CssClass="imageButton" OnClick="ibtnLogout_Click" />
                <br /> 
                Logout
            </td>
        </tr>        
    </table>
 
    <table>
        <tr>
            <td>
                <div class="DashboardItem">  
                     <table style="width:100%">
                         <tr>
                             <td class="DashboardDetails">
                                 Not cleared payments from Students: 
                                 <asp:Label ID="lblInfo1_1" runat="server" Text=""></asp:Label>
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnSave1_1" runat="server" ImageUrl="~/Images/Icons/Save.png" CssClass="RefreshButton" OnClick="btnSave1_1_Click" onclientclick="javascript:return confirm('Are you sure to save changes?');"/>
                                 <asp:ImageButton ID="btnRefresh1_1" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh1_1_Click"/>                                 
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:38.5vw; height:15vw; overflow: scroll;">
                                <asp:GridView ID="gvDashboard1_1" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="PaymentID" DataSourceID="dsDashboard1_1" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard1_1_RowDataBound" OnSorted="gvDashboard1_1_Sorted" OnPageIndexChanged="gvDashboard1_1_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="PaymentID" HeaderText="PaymentID" InsertVisible="False" ReadOnly="True" SortExpression="PaymentID" Visible="false" />
                                        <asp:BoundField DataField="PaymentNumber" HeaderText="Payment" SortExpression="PaymentNumber" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="Ammount" HeaderText="Amount" SortExpression="Ammount" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="Student" HeaderText="Student" SortExpression="Student" />
                                        <asp:BoundField DataField="Employee" HeaderText="Employee" SortExpression="Employee" />
                                        <asp:CheckBoxField DataField="Collected" HeaderText="Collected" SortExpression="Collected" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:CheckBoxField DataField="PaidToEmployee" HeaderText="Paid to Employee" SortExpression="PaidToEmployee" ItemStyle-HorizontalAlign="Center"/>                                        
                                        <asp:BoundField DataField="DateOfPayment" HeaderText="Date of Payment" SortExpression="DateOfPayment" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center"/>                                        
                                        <asp:BoundField DataField="ForEmployee" HeaderText="Percentage for Employee" SortExpression="ForEmployee" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="ProcessedBy" HeaderText="ProcessedBy" SortExpression="ProcessedBy" />
                                    </Columns>
                                </asp:GridView>
                                    
                                    <asp:SqlDataSource ID="dsDashboard1_1" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                                 <br />
                                 <div class="TextShadow">
                                    Total paid: <asp:Label ID="lblTotalPaid1_1" runat="server" Text=""></asp:Label>
                                    den.  Total Collected: <asp:Label ID="lblTotalCollected1_1" runat="server" Text=""></asp:Label>den.<br />
                                     Total for Employee: <asp:Label ID="lblTotalForEmployee1_1" runat="server" Text=""/>den.
                                     Total for Employee from collected: <asp:Label ID="lblTotalFromCollected1_1" runat="server" Text=""/>den.
                                 </div>
                             </td>
                         </tr>
                     </table>
                </div>
            </td>
            <td>
                <div class="DashboardItem">  
                     <table  style="width:100%">
                         <tr>
                             <td class="DashboardDetails">
                                 Not cleared payments from Services: 
                                 <asp:Label ID="lblInfo1_2" runat="server" Text=""></asp:Label>
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnSave1_2" runat="server" ImageUrl="~/Images/Icons/Save.png" CssClass="RefreshButton" OnClick="btnSave1_2_Click" onclientclick="javascript:return confirm('Are you sure to save changes?');"/>
                                 <asp:ImageButton ID="btnRefresh1_2" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh1_2_Click"/>
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:38.5vw; height:15vw; overflow: scroll">
                                <asp:GridView ID="gvDashboard1_2" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="PaymentID" DataSourceID="dsDashboard1_2" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard1_2_RowDataBound" OnSorted="gvDashboard1_2_Sorted" OnPageIndexChanged="gvDashboard1_2_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="PaymentID" HeaderText="PaymentID" InsertVisible="False" ReadOnly="True" SortExpression="PaymentID" Visible="false" />
                                        <asp:BoundField DataField="PaymentNumber" HeaderText="Payment" SortExpression="PaymentNumber" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="Ammount" HeaderText="Amount" SortExpression="Ammount" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                        <asp:BoundField DataField="Employee" HeaderText="Employee" SortExpression="Employee" />
                                        <asp:CheckBoxField DataField="Collected" HeaderText="Collected" SortExpression="Collected" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:CheckBoxField DataField="PaidToEmployee" HeaderText="Paid to Employee" SortExpression="PaidToEmployee" ItemStyle-HorizontalAlign="Center"/>                                        
                                        <asp:BoundField DataField="DateOfPayment" HeaderText="Date of Payment" SortExpression="DateOfPayment" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center"/>                                        
                                        <asp:BoundField DataField="ForEmployee" HeaderText="Percentage for Employee" SortExpression="ForEmployee" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="ProcessedBy" HeaderText="ProcessedBy" SortExpression="ProcessedBy" />
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsDashboard1_2" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                                 <br />
                                 <div class="TextShadow">
                                    Total paid: <asp:Label ID="lblTotalPaid1_2" runat="server" Text=""></asp:Label>
                                    den.  Total Collected: <asp:Label ID="lblTotalCollected1_2" runat="server" Text=""></asp:Label>den.<br />
                                     Total for Employee: <asp:Label ID="lblTotalForEmployee1_2" runat="server" Text=""/>den.
                                     Total for Employee from collected: <asp:Label ID="lblTotalFromCollected1_2" runat="server" Text=""/>den.
                                 </div>
                             </td>
                         </tr>
                     </table>
                </div>
            </td>            
        </tr>
        <tr>
           <td colspan="2">
               <table>
                   <tr>
                <td>
                <div class="DashboardItem3">  
                     <table style="width:100%">
                         <tr>
                             <td class="DashboardDetails" >
                                 To be collected from Employees:
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnSave2_0" runat="server" ImageUrl="~/Images/Icons/Save.png" CssClass="RefreshButton" OnClick="btnSave2_0_Click" onclientclick="javascript:return confirm('Are you sure to save changes?');"/>
                                 <asp:ImageButton ID="btnRefresh2_0" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh2_0_Click" />
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:25vw; height:15vw; overflow: scroll">
                                <asp:GridView ID="gvDashboard2_0" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="UserID" DataSourceID="dsDashboard2_0" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard2_0_RowDataBound" OnSorted="gvDashboard2_0_Sorted" OnPageIndexChanged="gvDashboard2_0_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="UserID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="UserID" Visible="true" />
                                        <asp:BoundField DataField="Employee" HeaderText="Employee" SortExpression="Employee" />
                                        <asp:BoundField DataField="Ammount" HeaderText="Total" SortExpression="Ammount" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:CheckBoxField DataField="Collected" HeaderText="Collected" SortExpression="Collected" ItemStyle-HorizontalAlign="Center"/>                                        
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsDashboard2_0" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                             </td>
                         </tr>
                     </table>
                </div>
            </td>

                <td>
                <div class="DashboardItem3">  
                     <table style="width:100%">
                         <tr>
                             <td class="DashboardDetails" >
                                 To be paid to the Employees:
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnSave2_1" runat="server" ImageUrl="~/Images/Icons/Save.png" CssClass="RefreshButton" OnClick="btnSave2_1_Click" onclientclick="javascript:return confirm('Are you sure to save changes?');"/>
                                 <asp:ImageButton ID="btnRefresh2_1" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh2_1_Click" />
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:25vw; height:15vw; overflow: scroll">
                                <asp:GridView ID="gvDashboard2_1" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="EmployeeID" DataSourceID="dsDashboard2_1" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard2_1_RowDataBound" OnSorted="gvDashboard2_1_Sorted" OnPageIndexChanged="gvDashboard2_1_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="EmployeeID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="EmployeeID" Visible="true" />
                                        <asp:BoundField DataField="Employee" HeaderText="Employee" SortExpression="Employee" />
                                        <asp:BoundField DataField="TotalForEmployee" HeaderText="Total" SortExpression="TotalForEmployee" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="TotalFromCollected" HeaderText="From Collected" SortExpression="TotalFromCollected" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:CheckBoxField DataField="PaidToEmployee" HeaderText="Paid to Employee" SortExpression="PaidToEmployee" ItemStyle-HorizontalAlign="Center"/>                                        
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsDashboard2_1" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                             </td>
                         </tr>
                     </table>
                </div>
            </td>
            <td>
            <div class="DashboardItem3">  
                     <table style="width:100%">
                         <tr>
                             <td class="DashboardDetails" >
                                 Not paid Additional Services:
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnRefresh2_2" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh2_1_Click" />
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:25vw; height:15vw; overflow: scroll">
                                <asp:GridView ID="gvDashboard2_2" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ServiceID" DataSourceID="dsDashboard2_2" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard2_2_RowDataBound" OnSorted="gvDashboard2_2_Sorted" OnPageIndexChanged="gvDashboard2_2_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="ServiceID" HeaderText="ServiceID" InsertVisible="False" ReadOnly="True" SortExpression="ServiceID" Visible="false" />
                                        <asp:BoundField DataField="ServiceName" HeaderText="Service" SortExpression="ServiceName" />
                                        <asp:BoundField DataField="Customer" HeaderText="Student" SortExpression="Customer" />
                                        <asp:BoundField DataField="Cost" HeaderText="Cost" SortExpression="Cost" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="Paid" HeaderText="Paid" SortExpression="Paid" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsDashboard2_2" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                             </td>
                         </tr>
                     </table>
                </div>
            </td>        
                   </tr>
                   <tr>
                       <td>
            <div class="DashboardItem3">  
                     <table style="width:100%">
                         <tr>
                             <td class="DashboardDetails" >
                                 Students without created Contract:
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnRefresh2_3" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh2_3_Click" />
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:25vw; height:15vw; overflow: scroll">
                                <asp:GridView ID="gvDashboard2_3" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="GroupStudentID" DataSourceID="dsDashboard2_3" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard2_3_RowDataBound" OnSorted="gvDashboard2_3_Sorted" OnPageIndexChanged="gvDashboard2_3_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="GroupStudentID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="GroupStudentID" Visible="false" />
                                        <asp:BoundField DataField="Student" HeaderText="Student" SortExpression="Student" />
                                        <asp:BoundField DataField="Course" HeaderText="Course" SortExpression="Course" />
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsDashboard2_3" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                             </td>
                         </tr>
                     </table>
                </div>
            </td>
                       <td>
                        <div class="DashboardItem3">  
                     <table style="width:100%">
                         <tr>

                        <td class="DashboardDetails" >
                                 Terms by Classroom:
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnRefresh3_1" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh3_1_Click" />
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:25vw; height:15vw; overflow: scroll">
                                <asp:GridView ID="gvDashboard3_1" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ClassRoomID" DataSourceID="dsDashboard3_1" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard3_1_RowDataBound" OnSorted="gvDashboard3_1_Sorted" OnPageIndexChanged="gvDashboard3_1_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="ClassRoomID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ClassRoomID" Visible="false" />
                                        <asp:BoundField DataField="Name" HeaderText="Classroom" SortExpression="Name" />
                                        <asp:BoundField DataField="Day" HeaderText="Day" SortExpression="Day" />
                                        <asp:BoundField DataField="TimeStart" HeaderText="From" SortExpression="TimeStart" />
                                        <asp:BoundField DataField="TimeEnd" HeaderText="To" SortExpression="TimeEnd" />
                                        <asp:BoundField DataField="GroupName" HeaderText="Group" SortExpression="GroupName" />
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsDashboard3_1" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                             </td>
                             </tr>
                     </table>
                </div>
            </td>

                       <td>
                        <div class="DashboardItem3">  
                     <table style="width:100%">
                         <tr>

                        <td class="DashboardDetails" >
                                 Students - Still not paid (finished groups):
                             </td>
                             <td style="text-align:right">
                                 <asp:ImageButton ID="btnRefresh3_2" runat="server" ImageUrl="~/Images/Icons/Refresh1.png" CssClass="RefreshButton" OnClick="btnRefresh3_2_Click" />
                             </td>
                         </tr>                    
                         <tr>
                             <td colspan="2">
                                <div style="width:25vw; height:15vw; overflow: scroll">
                                <asp:GridView ID="gvDashboard3_2" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="GroupStudentID" DataSourceID="dsDashboard3_2" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvDashboard3_2_RowDataBound" OnSorted="gvDashboard3_2_Sorted" OnPageIndexChanged="gvDashboard3_2_PageIndexChanged">
                                <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="GroupStudentID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="GroupStudentID" Visible="false" />
                                        <asp:BoundField DataField="GroupName" HeaderText="Group" SortExpression="GroupName" />
                                        <asp:BoundField DataField="Student" HeaderText="Student" SortExpression="Student" />
                                        <asp:BoundField DataField="Cost" HeaderText="Cost" SortExpression="Cost" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="Paid" HeaderText="Paid" SortExpression="Paid" DataFormatString="{0} ден." ItemStyle-HorizontalAlign="Right"/>
                                    </Columns>
                                </asp:GridView>
                                    <asp:SqlDataSource ID="dsDashboard3_2" runat="server" ConnectionString="<%$ ConnectionStrings:konekcija %>" SelectCommand=""></asp:SqlDataSource>                                
                                </div>
                             </td>
                             </tr>
                     </table>
                </div>
            </td>


                   </tr>
               </table>
           </td> 
        </tr>
    </table>    
        </ContentTemplate>    
    </asp:UpdatePanel>
</asp:Content>

