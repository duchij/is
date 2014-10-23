<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" Culture="sk-Sk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<style type="text/css">
.duchs {font-size:1em;}
.duchsB td {font-size:1.3em;height:50px;}
.duchsB input {font-size:1.2em;}


</style>
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title></title>
 <link rel="icon" type="image/png" href="http://is.kdch.sk/kdch.png" />   
    
</head>
<body>
    <form id="form1" runat="server">
   
        <div class="login">
        <asp:Login ID="Login1"  runat="server" BorderStyle="Solid" 
            FailureText="Nesprávne meno,heslo! Alebo zablokovaný účet..." onauthenticate="Login1_Authenticate" 
            PasswordLabelText="Heslo:" PasswordRequiredErrorMessage="Nutné zadať heslo!" 
            TitleText="Prihlásenie do IS KDCH" UserNameLabelText="Meno:" 
            UserNameRequiredErrorMessage="Nutné zadať meno!" DisplayRememberMe="False" 
            RememberMeText="" BackColor="#FFFBD6" BorderColor="#FFDFAD" 
            BorderPadding="4" BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" 
            ForeColor="#333333" Height="136px" LoginButtonText="Vstúpiť" 
            TextLayout="TextOnTop" Width="218px">
            <TextBoxStyle Font-Size="1em" />
            <LoginButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" ForeColor="#990000" />
            <LayoutTemplate>
                <table border="0" cellpadding="4" cellspacing="0" 
                    style="border-collapse:collapse;width:100%;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" style="width:100%;">
                                <tr>
                                    <td align="center" 
                                        style="color:White;background-color:#990000;font-weight:bold;">
                                        Prihlásenie do IS KDCH</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Meno:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="Nutné zadať meno!" 
                                            ToolTip="Nutné zadať meno!" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Heslo:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server"  TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Nutné zadať heslo!" 
                                            ToolTip="Nutné zadať heslo!" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="LoginButton" runat="server" BackColor="White" 
                                            BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" 
                                            Font-Names="Verdana"  ForeColor="#990000" Text="Vstúpiť" 
                                            ValidationGroup="Login1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#990000" Font-Bold="True" Font-Size="0.9em" 
                ForeColor="White" />
        </asp:Login>
    
        <br />
        <asp:Label ID="info_txt" runat="server"></asp:Label>
            
    </div>
    </form>
</body>
</html>
