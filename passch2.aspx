<%@ Page Language="C#" AutoEventWireup="true" CodeFile="passch2.aspx.cs" Inherits="passch2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Zmena hesla do is.kdch.sk:</h2>
                <hr />
         <asp:Label ID="info_lbl" runat="server" Text="Label" Visible="False"></asp:Label><br /><br />
        <asp:PlaceHolder ID="changePassw" runat="server">
          
                
                
                <table border="0">
                <tr><td>Nové heslo:</td><td><asp:TextBox ID="passwd1" runat="server" TextMode="Password">

                                            </asp:TextBox></td></tr>
               <tr>
               <td>Kontrola hesla:</td>
               <td><asp:TextBox ID="passwd2" runat="server" TextMode="Password"></asp:TextBox></td>
              </tr>
                 </table>
                <asp:Button ID="change_btn" runat="server" Text="Zmeň" 
                    onclick="change_btn_Click" />
               </asp:PlaceHolder>    
                   

    
    </div>
    </form>
</body>
</html>
