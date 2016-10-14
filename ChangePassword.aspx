<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="TabelaDetailsGrid">
        <tr>
            <td style="text-align:right">
                Username:
            </td>
            <td>
                <asp:TextBox ID="tbUserName" runat="server" CssClass="TextBoxRoundedFilters" ReadOnly="true" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align:right">
                Old Password:
            </td>
            <td>
                <asp:TextBox ID="tbOldPassword" runat="server" CssClass="TextBoxRoundedFilters" TextMode="Password"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbOldPassword" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align:right">
                New Password:
            </td>
            <td>
                <asp:TextBox ID="tbNewPassword" runat="server" CssClass="TextBoxRoundedFilters" TextMode="Password"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbNewPassword" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align:right">
                Confirm New Password:
            </td>
            <td>
                <asp:TextBox ID="tbConfirmPassword" runat="server" CssClass="TextBoxRoundedFilters" TextMode="Password"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ErrorMessage="Required!" CssClass="RequredField" ControlToValidate="tbConfirmPassword" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
                <tr>
            <td style="text-align:left">
                <asp:label id="lblInfoMessage" runat="server" text="" CssClass="RequredField"></asp:label>
            </td>
            <td style="text-align:left">
                <asp:Button ID="btnChange" runat="server" Text="Change Password" CssClass="FormatedButton" OnClick="btnChange_Click" ValidationGroup="1" />
            </td>
            <td></td>
        </tr>
    </table>
</asp:Content>

