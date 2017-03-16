<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="true" EnableViewStateMac="true"  Culture="sk-Sk" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
     <title>IESKO KDCH</title>
     <link rel="icon" type="image/png" href="img/logo.png" />   
      <!-- jQuery-->
	  
	    <!-- framework css --><!--[if gt IE 9]><!-->
	    <link type="text/css" rel="stylesheet" href="gdw/css/groundwork.css" /><!--<![endif]--><!--[if lte IE 9]>
	    <link type="text/css" rel="stylesheet" href="css/groundwork-core.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-type.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ui.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-anim.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ie.css"><![endif]-->
	   	<!--<link type="text/css" rel="stylesheet" href="gdw/css/duch.css" />-->
</head>
<body onload="getData();">
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="name_hf" Value="" />
    <asp:HiddenField runat="server" ID="passwd_hf" Value="" />
    <div class="row">
     
        <div class="one third centered">
            <table style="width:300px;">
                <tr>
                    <td colspan="2" class="responsive" data-max="15">
                        
                        <div class="box blue"><h1>Prihlásenie do IS.KDCH</h1></div>
                        
                    </td>
                </tr>
                <tr>
                    <td><h3>Meno:</h3></td>
                    <!--<td><asp:TextBox ID="meno_txt" runat="server" TextMode="SingleLine" ClientIDMode="static"></asp:TextBox></td>-->

                   <td><input type="text" id="meno_txt"></td>
                </tr>
                <tr>
                    <td><h3>Heslo:</h3></td>
                    <!--<td><asp:TextBox TextMode="Password" runat="server" ID ="passwd_txt" ClientIDMode="static"></asp:TextBox></td>-->
                    <td><input type="password" id="passwd_txt"></td>
                </tr>
                <tr>
                    <td colspan="2">
                                             
                        <asp:Button ID="login_btn" runat="server" OnClick="runLogin" onClientClick="return runLogin();" Text="Prihlásiť sa" Visible="true" CssClass="button green"></asp:Button></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="info_txt" runat="server" CssClass="red" Text=""></asp:Label></td>
                </tr>
            </table>
    </div>      
   </div>
    </form>
    <script type="text/javascript" src="gdw/js/libs/jquery-1.10.2.min.js"></script>
	<script type="text/javascript" src="gdw/js/groundwork.all.js"></script>
    <script type="text/javascript" src="js/aes.js"></script> 
    <script type="text/javascript" src="js/login.js"></script>
    
</body>
</html>
