<%@ Page Title="" Language="C#" MasterPageFile="~/MP_NextLevel.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="DefaultForm_Tabela1"> 
        <tr>
            <td colspan="3">Login</td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td> </td>
        </tr>
        <tr>
            <td style="padding-right:1vw; text-align:right; font-size:1.5vw">Username:</td>
            <td style="width:20%; text-align:left">
                <asp:TextBox ID="tbUserName" runat="server" CssClass="TextBoxRounded"></asp:TextBox>
            </td>
            <td style="padding-left:1vw; text-align:left;">
                <asp:RequiredFieldValidator ID="rqvValidateUserName" runat="server" ControlToValidate="tbUserName" CssClass="RequredField" ErrorMessage="Required field!" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="padding-right:1vw; text-align:right; font-size:1.5vw">Password:</td>
            <td>
                <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" CssClass="TextBoxRounded"></asp:TextBox>
            </td>
            <td style="padding-left:1vw; text-align:left">
                <asp:RequiredFieldValidator ID="rqvValidatePassword" runat="server" ControlToValidate="tbPassword" CssClass="RequredField" ErrorMessage="Required field!" ValidationGroup="1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Label ID="lblInfoNajava" runat="server" class="RequredField"></asp:Label>
            </td>
            <td style="padding-left:1vw">
                <asp:Button ID="btnLogin" runat="server" class="FormatedButton" Text="Login" ValidationGroup="1" OnClick="btnLogin_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

