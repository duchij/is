<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" Culture="sk-Sk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" ForeColor="#990000" />
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
