<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" Culture="sk-Sk" %>


<!DOCTYPE html >

<!--[if lt IE 7]><html class="no-js ie ie6 lt-ie7 lt-ie8 lt-ie9 lt-ie10"><![endif]-->
<!--[if IE 7]>   <html class="no-js ie ie7 lt-ie8 lt-ie9 lt-ie10"><![endif]-->
<!--[if IE 8]>   <html class="no-js ie ie8 lt-ie9 lt-ie10"><![endif]-->
<!--[if IE 9]>   <html class="no-js ie ie9 lt-ie10"><![endif]-->
<!--[if gt IE 9]><html class="no-js ie ie10"><![endif]-->
<!--[if !IE]><!-->

<html class="no-js" xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	 <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1">
	 
	   <script src="gdw/js/libs/modernizr-2.6.2.min.js"></script>
        <title></title>
    <link rel="icon" type="image/png" href="img/logo.png" />   
      <!-- jQuery-->
	  
	    <!-- framework css --><!--[if gt IE 9]><!-->
	    <link type="text/css" rel="stylesheet" href="gdw/css/groundwork.css" /><!--<![endif]--><!--[if lte IE 9]>
	    <link type="text/css" rel="stylesheet" href="css/groundwork-core.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-type.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ui.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-anim.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ie.css"><![endif]-->
	   	<link type="text/css" rel="stylesheet" href="gdw/css/duch.css" />
    
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="row">
        
      <div class="one third centered">
        <asp:Login ID="Login1"  runat="server"  
            FailureText="Nesprávne meno,heslo! Alebo zablokovaný účet..." onauthenticate="Login1_Authenticate" 
            PasswordLabelText="Heslo:" PasswordRequiredErrorMessage="Nutné zadať heslo!" 
            TitleText="Prihlásenie do IS KDCH" UserNameLabelText="Meno:" 
            UserNameRequiredErrorMessage="Nutné zadať meno!" DisplayRememberMe="False" 
            RememberMeText=""  LoginButtonText="Vstúpiť">
            <LayoutTemplate >
              
                            <table border="0" cellpadding="0" class="responsive" data-max="15">
                                <tr>
                                    <td align="center" colspan="2">
                                        <div class="box blue centered"><h1 class="responsive half-padded" data-compression="12">Prihlásenie do IS KDCH</h1></div></td>
                                </tr>
                                <tr>
                                    <td align="right" >
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName"><h3>Meno:</h3></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" Font-Size="Medium" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="Nutné zadať meno!" 
                                            ToolTip="Nutné zadať meno!" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="20%">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password"><h3>Heslo:</h3></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Font-Size="Medium"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Nutné zadať heslo!" 
                                            ToolTip="Nutné zadať heslo!" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                    
                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Vstúpiť" 
                                            ValidationGroup="Login1" BackColor="#009933" Font-Bold="True" 
                                            Font-Size="15px" ForeColor="White"  />
                                    </td>
                                </tr>
                            </table>
                  
            </LayoutTemplate>
        </asp:Login>
    
        
        <asp:Label ID="info_txt" runat="server"></asp:Label>
      </div>      
    </div>
    </form>
        <script type="text/javascript" src="gdw/js/libs/jquery-1.10.2.min.js"></script>
	<script type="text/javascript" src="gdw/js/groundwork.all.js"></script>
</body>
</html>
