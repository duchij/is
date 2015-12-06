<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_news_show.aspx.cs" Inherits="is_news_show" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>
<h2><asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, is_news_title %>"></asp:Label></h2>
    <div style="background-color:#efeeea;padding:3px;">
<asp:Literal ID="cela_sprava" runat="server" Text=""></asp:Literal>
        </div>

</asp:Content>

