<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news.ascx.cs" Inherits="news" %>
<head>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
</head>
<h2><asp:Label ID="label2" runat="server" Text="<%$ Resources:Resource, is_op_title_small %>"></asp:Label></h2>
<hr />
<asp:Table ID="op_table" runat="server" Width="100%" ></asp:Table>
<hr />
<h2><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, is_news_title_small %>"></asp:Label></h2>
<hr />
<asp:Table ID="news_tbl" runat="server" Width="100%" ></asp:Table>

