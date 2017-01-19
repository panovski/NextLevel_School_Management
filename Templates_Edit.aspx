<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Templates_Edit.aspx.cs" Inherits="Templates_Edit" EnableEventValidation="false" %>

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
</head>
<body>
    <form id="form1" runat="server">
<table class="EditForma">        
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>        
        <tr style="vertical-align:top; text-align:left">
            <td>           
                   <div class="GridViewTabelaPomala">       
                       All Templates:           
                   <div style="width:34vw; height:40vw; overflow: scroll;"> 
                   <asp:GridView ID="gvMain" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="TemplateID" DataSourceID="dsMain" CssClass="GridView" PagerStyle-CssClass="GridViewPager" HeaderStyle-CssClass="GridViewHeader" RowStyle-CssClass="GridViewRows" AllowPaging="True" AllowSorting="True" PageSize="20" AlternatingRowStyle-CssClass="GridViewRowsAlt" SelectedRowStyle-CssClass="GridViewSelectedRow" EditRowStyle-CssClass="GridViewSelectedRow" OnRowDataBound="gvMain_RowDataBound" OnSelectedIndexChanged="gvMain_SelectedIndexChanged" OnSorted="gvMain_Sorted" >
                    <AlternatingRowStyle CssClass="GridViewRowsAlt"></AlternatingRowStyle>
                       <Columns>
                           <asp:BoundField DataField="TemplateID" HeaderText="TemplateID" InsertVisible="False" ReadOnly="True" SortExpression="TemplateID" Visible="false" />
                           <asp:BoundField DataField="TemplateName" HeaderText="Template Name" SortExpression="TemplateName"/>
                           <asp:BoundField DataField="TemplateFile" HeaderText="Template File" SortExpression="TemplateFile"/>
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
                            Template type:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvddlCourse" runat="server" ErrorMessage="!" CssClass="RequredField" ControlToValidate="ddlType" ValidationGroup="1" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Choose file:
                        </td>
                        <td>
                            <asp:FileUpload ID="fuFile" runat="server" ValidationGroup="1" />                            
                        </td>
                        <td>                            
                            <asp:RequiredFieldValidator ID="rfvfuFile" runat="server" ErrorMessage="!" CssClass="RequredField" ControlToValidate="fuFile" ValidationGroup="1" ></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revfuFile" runat="server" ErrorMessage="Just .docx is allowed!" CssClass="RequredField" ControlToValidate="fuFile" ValidationGroup="1" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.docx)$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Template Name:
                        </td>
                        <td>
                            <asp:TextBox ID="tbTemplateName" runat="server" CssClass="TextBoxRoundedEdit" ValidationGroup="1" ></asp:TextBox>
                        </td>
                        <td>
                           <asp:RequiredFieldValidator ID="rfvtbTemplateName" runat="server" ErrorMessage="!" CssClass="RequredField" ControlToValidate="tbTemplateName" ValidationGroup="1" ></asp:RequiredFieldValidator> 
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
                            <asp:Button ID="btnSave" runat="server" Text="Save changes" CssClass="EditFormButton" OnClick="btnSave_Click"  ValidationGroup="2" Visible="False"  />               
                            <asp:Button ID="btnInsert" runat="server" Text="Create New Template" CssClass="EditFormButton" ValidationGroup="1" OnClick="btnInsert_Click" />                
                            <asp:Button ID="btnDelete" runat="server" Text="Delete the Template" CssClass="EditFormButton" ValidationGroup="2" OnClick="btnDelete_Click" onclientclick="javascript:return confirm('Are you sure to delete the selected contract?');" />                
                            <asp:Button ID="btnDownload" runat="server" Text="Download selected Template" CssClass="EditFormButton" OnClick="btnDownload_Click" />     
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
    </form>
</body>
</html>
