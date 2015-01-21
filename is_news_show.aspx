<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_news_show.aspx.cs" Inherits="is_news_show" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>
                    <asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, is_news_title %>"></asp:Label></h2>
                <hr />
               <%-- Prihlasený: <strong>
                    <asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000" ></asp:Label></strong><br />--%>
                <%-- Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />--%>
                <hr />
                
                <asp:Label ID="cela_sprava" runat="server" Text="" style="font-size:12px;font-family:Verdana;"></asp:Label></h2>
</asp:Content>

