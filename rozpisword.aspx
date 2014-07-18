<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rozpisword.aspx.cs" Inherits="sltoword" Culture ="sk-SK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1250">
    <title>IS - to Word</title>
    <link href="http://is.kdch.sk/css/style.css" rel="stylesheet" type="text/css" />
<link href="http://is.kdch.sk/css/print.css" rel="stylesheet" type="text/css" media="print"/>
</head>
<body style="font-family:Arial CE;">
    <form id="form1" runat="server">
    <div>
    <h1><asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, is_rozp_title %>"></asp:Label>, <asp:Label ID="mesiac_lbl" runat="server" Text="Label"></asp:Label>, <asp:Label
        ID="rok_lbl" runat="server" Text="Label"></asp:Label>   </h1><hr />
        <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                     <td width="40" align="center"><b><asp:Label ID="label2" runat="server" Text="<%$ Resources:Resource, is_den %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, is_oup %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="label3" runat="server" Text="<%$ Resources:Resource, is_vseamb %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="label4" runat="server" Text="<%$ Resources:Resource, is_traumamb %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="label5" runat="server" Text="<%$ Resources:Resource, is_koj %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="label6" runat="server" Text="<%$ Resources:Resource, is_dievcat %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="label7" runat="server" Text="<%$ Resources:Resource, is_veldeti %>"></asp:Label></b></td>
                
                </tr>
                </table>
        <asp:Table ID="Table1" runat="server" BorderColor="Black" BorderWidth="1" cellpadding="0" cellspacing="0" Font-Size="Small">
        </asp:Table>   
        <br />
        Schvalil:<br />
        MUDr. Vladimir Cingel,<br />
        primar KDCH
    </div>
    <div class="nonprint">
    <hr />
    <asp:Label ID="print_lbl" runat="server" Text="" Visible="false"></asp:Label> 
    <asp:Label ID="back_lbl" runat="server" Text="" Visible="false"></asp:Label>
    </div>
    </form>
</body>
</html>
