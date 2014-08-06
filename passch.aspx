<%@ Page Language="C#" AutoEventWireup="true" CodeFile="passch.aspx.cs" Inherits="passch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>Password change</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
    <div id="header"><h1>Informačný systém Kliniky detskej chirurgie LF UK a DFNsP</h1> </div>
        
        <div id="content">
            
            
            <div id="cont_text">
            
                <h2>Zmena hesla:</h2>
                <hr />
                
                <asp:Label ID="info_lbl" runat="server" Text="Label" Visible="False"></asp:Label><br /><br />
                
                <asp:PlaceHolder ID="changePassw" runat="server">
                <table border="0">
                <tr><td>Nové heslo:</td><td><asp:TextBox ID="passwd1" runat="server" TextMode="Password"></asp:TextBox></td></tr>
               <tr>
               <td>Kontrola hesla:</td>
               <td><asp:TextBox ID="passwd2" runat="server" TextMode="Password"></asp:TextBox></td>
              </tr>
                 </table>
                <asp:Button ID="change_btn" runat="server" Text="Zmeň" 
                    onclick="change_btn_Click" />
                   
                    </asp:PlaceHolder>
                   
            </div>
            
            
            
        
        </div>
        <div id="menu">
                <div class="menu">Menu</div>
                <ul>
                    
                </ul>
            
            </div>
        <div id="footer">Design by Boris Duchaj</div>
    
    
    </div>
 
    
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
 
    
    </form>
</body>
</html>
