<%@ Control Language="C#" AutoEventWireup="true" CodeFile="news.ascx.cs" Inherits="news" %>


<h2 class="blue"><asp:Label ID="label2" runat="server" Text="<%$ Resources:Resource, is_op_title_small %>"></asp:Label></h2>
<asp:PlaceHolder ID="op_tbl" runat="server"></asp:PlaceHolder>

<h2><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, is_news_title_small %>" ></asp:Label></h2>
<asp:PlaceHolder ID="news_plh" runat="server"></asp:PlaceHolder>



