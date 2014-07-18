<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printtses.aspx.cs" Inherits="hlasenia_print" %>

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
    <h2> Hlásenie sestier KDCH - <asp:Label ID="odd_lbl" runat="server" Text="Label"></asp:Label>, <asp:Label ID="datum_lbl" runat="server" Text="Label"></asp:Label></h2>
    <!--<table>
    <tr>
     <td><strong>OUP: </strong>
        <asp:Label ID="oup_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    <td><strong>Odd A:</strong> 
        <asp:Label ID="odda_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    <td><strong>Odd B: </strong>
        <asp:Label ID="oddb_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    <td><strong>Op.pohotovost: </strong><asp:Label ID="op_lbl" runat="server" Text="Label" ForeColor="black"></asp:Label></td>
    </tr>
    
    </table>!-->
    
    Vytlačil: <asp:Label ID="user_lbl" runat="server" Text="Label"></asp:Label><hr />
    
    <!--Služba:<strong><asp:Label ID="type_lbl" runat="server" Text=""></asp:Label></strong>!--> <br />
    
    Hlásenie:
    <div style="font-size:12px;font-family:Verdana;"><asp:Label ID="hlas_lbl" runat="server" Text="" ></asp:Label></div><br />
    <hr /><!--<br /><br />
    ...........................................<br />
    Pečiatka a podpis<br /><br />!-->
     <div class="nonprint">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
   <a href="" onclick="window.print();" style="font-size:12px;" />Tlacit </a> &nbsp
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="sestrhlas.aspx" Font-Size="12px">Naspäť</asp:HyperLink></div> 
    </form>
    
</body>
</html>
