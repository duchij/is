<%@ Page Language="C#" AutoEventWireup="true" CodeFile="is_news_show.aspx.cs" Inherits="is_news_show" %>
<%@ Register TagPrefix="duch" TagName="my_header" Src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IS - Cela sprava</title>
</head>
<body>
    <form id="form1" runat="server">
     <div id="wrapper">
        <div id="header">
            <h1>
                Informačný systém Kliniky detskej chirurgie LF UK a DFNsP</h1>
            <duch:my_header runat="server"></duch:my_header>
        </div>
        <div id="content">
        <div id="menu2">
           <info:info_bar ID="info_bar" runat="server" />
            <info:info_bar />
        </div>
        
            <div id="cont_text">
                <h2>
                    <asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, is_news_title %>"></asp:Label></h2>
                <hr />
                Prihlasený: <strong>
                    <asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000" ></asp:Label></strong><br />
                <%-- Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />--%>
                <hr />
                
                <asp:Label ID="cela_sprava" runat="server" Text="" style="font-size:12px;font-family:Verdana;"></asp:Label></h2>
                
            </div>
        </div>
        
        <div id="menu">
            <menu:left_menu ID="Left_menu1" runat="server" />
            <menu:left_menu />
        </div>
        
        
        <div id="footer">
            Design by Boris Duchaj</div>
    </div>
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
