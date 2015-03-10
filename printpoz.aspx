<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printpoz.aspx.cs" Inherits="printpoz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IS - Poziadavky</title>
    
    <link href="css/style.css" rel="stylesheet" type="text/css" />
<link href="css/print.css" rel="stylesheet" type="text/css" media="print"/>
</head>
<body style="margin:10px;">
    <form id="form1" runat="server">
    <div>
    <h1><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_title_print %>"></asp:Label><asp:Label ID="poziadavMes_lbl" runat="server"></asp:Label></h1>
    
     Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="1">Január</asp:ListItem>
                        <asp:ListItem Value="2">Február</asp:ListItem>
                        <asp:ListItem Value="3">Marec</asp:ListItem>
                        <asp:ListItem Value="4">Apríl</asp:ListItem>
                        <asp:ListItem Value="5">Máj</asp:ListItem>
                        <asp:ListItem Value="6">Jún</asp:ListItem>
                        <asp:ListItem Value="7">Júl</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>   
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">Október</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>  
                    Rok: 
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" >
                    <asp:ListItem Value="2010">Rok 2010</asp:ListItem>
                    <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                    <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                    <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
                    <asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                    <asp:ListItem Value="2015">Rok 2015</asp:ListItem>
                    <asp:ListItem Value="2016">Rok 2016</asp:ListItem>
                </asp:DropDownList>
    
    <hr />
    <asp:Table ID="zoznam_tbl" runat="server"></asp:Table>
    <hr />
    <div class="nonprint">
        <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
        <a href="" onclick="window.print();" style="font-size:12px;" />Tlacit </a> &nbsp
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="poziadavky.aspx" Font-Size="12px">Naspäť</asp:HyperLink>
     </div> 
    </div>
    </form>
</body>
</html>
