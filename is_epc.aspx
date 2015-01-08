<%@ Page Language="C#" AutoEventWireup="true" CodeFile="is_epc.aspx.cs" Inherits="is_epc" Culture="sk-Sk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tlacenie EPC</title>
    <link href="css/print.css" rel="stylesheet" type="text/css" media="print"/>
</head>
<body style="font-family:Arial; font-size:12px;margin:10px">
    <form id="form1" runat="server">

         
    <div>
        <asp:Image ID="Image1" runat="server" ImageUrl="img/dfnsp.jpg" Width="80" Height="80" ImageAlign="Left" />
        Detská fakultná nemocnica s poliklinikou Bratislava, Limbová 1, 833 40  Bratislava
        <hr />
        <h1> EVIDENCIA PRACOVNÉHO ČASU V ÚPS</h1>
        
        <hr />
        <div class="nonprint">
        <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
            <a href="javascript:window.print();" style="font-weight:bolder;font-size:x-large;" />
            <asp:Label ID="print_lbl" runat="server" Text="<%$ Resources:Resource, print %>"></asp:Label> </a> &nbsp;
               <asp:HyperLink ID="HyperLink1" runat="server" 
        NavigateUrl="vykaz2.aspx" Font-Bold="true" Font-Size="X-Large" Text="<%$ Resources:Resource, back %>"></asp:HyperLink>

        </div>
        <%--<table style="width:100%">
            <tr>
                <td><asp:Label ID="username_lbl" runat="server" Text="Label"></asp:Label></td>
                <td>KDCH</td>
                <td><asp:Label ID="zaradenie_lbl" runat="server" Text="Label"></asp:Label></td>
                <td><asp:Label ID="osobcislo_lbl" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>--%>

        


        <hr />
        <asp:Table ID="epc_tbl" runat="server" BorderColor="Black" Width="100%" CellPadding="0" CellSpacing="0" ></asp:Table>
    </div>
        

    </form>
</body>
</html>
