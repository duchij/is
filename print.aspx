﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="print.aspx.cs" Inherits="hlasenia_print"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script type="text/javascript">
function varitext(text)
{
    text = document
    print(text);

}
</script>
<link href="css/style.css" rel="stylesheet" type="text/css" />
<link href="css/print.css" rel="stylesheet" type="text/css" media="print"/>
    <title>Vytlačenie hlásenia</title>
</head>
<body style="font-size:11px;margin:10px">
       
    <form id="form1" runat="server">
    <h2> Hlásenie služby: <asp:Label ID="datum_lbl" runat="server" Text="Label"></asp:Label></h2>
    <table width="70%">
    <tr>
     <td style="font-size:12px;"><strong>OUP: </strong>
        <asp:Label ID="oup_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    <td style="font-size:12px;"><strong>Odd A:</strong> 
        <asp:Label ID="odda_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    <td style="font-size:12px;"><strong>Odd B: </strong>
        <asp:Label ID="oddb_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    <td style="font-size:12px;"><strong>Op.pohotovost: </strong><asp:Label ID="op_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    </tr>
    <tr>
    <td colspan="4" style="font-size:12px;">
         Vytlačil: <asp:Label ID="user_lbl" runat="server" Text="Label"></asp:Label><hr />
    
    Služba:<strong><asp:Label ID="type_lbl" runat="server" Text=""></asp:Label></strong> <br />
    
    Hlásenie:<asp:Label ID="hlas_lbl" runat="server" Text=""></asp:Label><br />
    <hr /><br /><br />
    ...........................................<br />
    Pečiatka a podpis<br /><br />
    </td>
    
    </tr>
    
   
    </table>
     <div class="nonprint">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
   <a href="" onclick="window.print();" style="font-weight:bolder;font-size:x-large;" /><asp:Label ID="print_lbl" runat="server" Text="<%$ Resources:Resource, print %>"></asp:Label> </a> &nbsp
    <asp:HyperLink ID="HyperLink1" runat="server" 
        NavigateUrl="hlasko.aspx" Font-Bold="true" Font-Size="X-Large" Text="<%$ Resources:Resource, back %>"></asp:HyperLink></div> 
    </form>
    
</body>
</html>
