<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header.ascx.cs" Inherits="header" EnableViewState="true" %>


<head>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .headTd {
    width:150px;
    }
    
    </style>
</head>

<table width="100%" border="0">
    <tr>
    <td style="width:80px;">Dnes slúži:</td>
    <td class="headTd"><strong>OUP: </strong>
        <asp:Label ID="oup_lbl" runat="server" Text="Label" ForeColor="Red"></asp:Label></td>
    <td><strong>Odd A:</strong> 
        <asp:Label ID="odda_lbl" runat="server" Text="Label" ForeColor="Red"></asp:Label></td>
    <td><strong>Odd B: </strong>
        <asp:Label ID="oddb_lbl" runat="server" Text="Label" ForeColor="Red"></asp:Label></td>
    <td><strong>Op.pohot.: </strong><asp:Label ID="op_lbl" runat="server" Text="Label" ForeColor="Red"></asp:Label></td>
    <td><strong>Prijm.amb.:</strong> <asp:Label ID="trp_lbl" runat="server" Text="Label" ForeColor="Red" ></asp:Label></td>
    <td align="right">Dnes je:<strong>
        <asp:Label ID="date_lbl" runat="server" Text="Label"></asp:Label></strong></td>
    </tr>
    </table>
    <table width="100%">
   <tr>
        <td>Dnes je poslužbe: </td>
        <td colspan="5">
            <asp:Label ID="po_lbl" runat="server" Text="Label" ForeColor="green"></asp:Label>  </td>
            
            <td align="right"><asp:Label ID="lock_info_lbl" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_info_short %>"></asp:Label>
                    <strong><asp:Label ID="poziadav_lbl" runat="server" Text=""></asp:Label><asp:Button ID="Button1" runat="server" Text="Odhlásenie" BorderStyle="Groove" 
                    onclick="log_out_Click" BorderColor="#990000" BackColor="#990000" ForeColor="yellow" /></td>
                   
        </tr></table>
        <table>
        <tr>
        <td class="tabs">
            <asp:HyperLink ID="news_link" runat="server" NavigateUrl="is_news.aspx" CssClass="tabs_a">
                <asp:Localize runat="server" ID="localize3" Text="<%$ Resources:Resource, is_news_title %>">
                </asp:Localize>
            </asp:HyperLink></td>
        
        <td class="tabs">
            <asp:HyperLink ID="odd_link" runat="server" NavigateUrl="is_news.aspx" CssClass="tabs_a">
                <asp:Localize runat="server" ID="localize1" Text="<%$ Resources:Resource,tabs_odd %>">
                </asp:Localize>
           </asp:HyperLink></td>
           <td class="tabs">
            <asp:HyperLink ID="rozpislek_link" runat="server" NavigateUrl="rozpislekar.aspx" CssClass="tabs_a">
                <asp:Localize runat="server" ID="localize2" Text="<%$ Resources:Resource,tabs_rozpis_lekar %>">
                </asp:Localize>
           </asp:HyperLink></td>
           <td class="tabs">
            <asp:HyperLink ID="opprogram_link" runat="server" NavigateUrl="opprogram.aspx" CssClass="tabs_a">
                <asp:Localize runat="server" ID="localize4" Text="<%$ Resources:Resource,tabs_op_program %>">
                </asp:Localize>
           </asp:HyperLink></td>
           <%-- <td class="tabs"><a href="opkniha.aspx" target="_self" class="tabs_a">
                <asp:Localize runat="server" ID="localize2" Text="<%$ Resources:Resource,tabs_operacky %>">
                </asp:Localize>
            </a></td>--%></tr>
    </table>
    