﻿<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adduser.aspx.cs" Inherits="adduser" Culture="sk-SK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>IS-Užívateľ</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />

    
    <script type="text/javascript" src="tinymce/jscripts/tiny_mce/tiny_mce.js"></script>

<script type="text/javascript">
tinyMCE.init({
        mode : "textareas",
        force_br_newlines : true,
        force_p_newlines : false
        
});
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
    <div id="header"><h1>Informačný systém Kliniky detskej chirurgie LF UK a DFNsP</h1> 
    <duch:my_header ID="My_header1" runat="server"></duch:my_header>
    
    </div>
        
        <div id="content">
             <div id="menu2">
           <info:info_bar ID="info_bar" runat="server" />
            <info:info_bar />
        </div>
            
            <div id="cont_text">
                
            
            
                    <h2>Informácie o užívateľovi:</h2><hr />
                    <asp:PlaceHolder ID="adminsectionPlace" runat="server">
                        <asp:TextBox ID="search_txt" runat="server"></asp:TextBox>
                        <asp:Button ID="search_btn" runat="server" Text="Hľadaj" OnClick="searchByNameFnc" />
                    
                     <asp:GridView ID="users_gv" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                        GridLines="None" onpageindexchanging="users_gv_PageIndexChanging" 
                        onselectedindexchanging="users_gv_SelectedIndexChanging" Width="100%" 
                        onrowdeleting="users_gv_RowDeleting">
                         <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                         <Columns>
                             <asp:CommandField ShowSelectButton="True" />
                             <asp:BoundField DataField="id" HeaderText="id">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField DataField="full_name" HeaderText="Cele meno">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField DataField="name" HeaderText="login">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField ConvertEmptyStringToNull="False" DataField="group" 
                                 HeaderText="Skupina">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:CommandField ShowDeleteButton="True" />
                         </Columns>
                         <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                         <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                         <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                         <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                         <AlternatingRowStyle BackColor="White" />
                    
                    </asp:GridView>
                    
                    <table>
                     <tr>
                        <td>Heslo:</td>
                        <td><asp:TextBox ID="passwd_txt" runat="server" Width="300px"></asp:TextBox></td>
                    
                    </tr>
                     <tr>
                        <td>Aktivny:</td>
                        <td><asp:TextBox ID="active_txt" runat="server" Width="30px"></asp:TextBox></td>
                    
                    </tr>
                    
                    </table>
                
               </asp:PlaceHolder>
                    
                    <table>
                    <tr>
                        <td>Meno a priezvisko:</td>
                        <td><asp:TextBox ID="name_txt" runat="server"></asp:TextBox></td>
                    
                    </tr>
                    <tr>
                        <td>Login:</td>
                        <td><asp:TextBox ID="login_txt" runat="server"></asp:TextBox></td> 
                    </tr>
                    <tr>
                        <td>E-mail:</td>
                        <td><asp:TextBox ID="email_txt" runat="server"></asp:TextBox></td>
                    
                    </tr>
                    <tr>
                        <td>Pracovna doba:</td>
                        <td><asp:TextBox ID="pracdoba_txt" runat="server" Text="7,5"></asp:TextBox></td>
                    
                    </tr>
                    
                     <tr>
                        <td>Tyzdenna pracovna doba:</td>
                        <td><asp:TextBox ID="tyzdoba_txt" runat="server" Text="37,5"></asp:TextBox></td>
                    
                    </tr>
                    
                     <tr>
                        <td>Osobne cislo:</td>
                        <td><asp:TextBox ID="osobcisl_txt" runat="server" Text=""></asp:TextBox></td>
                    
                    </tr>
                    
                    
                    
                    <tr>
                        <td>
                            <asp:Label ID="rights_lbl" runat="server" Text="Práva"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="rights_cb" runat="server">
                                <asp:ListItem Value="poweruser">PowerUser</asp:ListItem>
                                <asp:ListItem Value="admin">Admin</asp:ListItem>
                                <asp:ListItem Value="users">Users</asp:ListItem>
                                <asp:ListItem Value="sestra">Sestra</asp:ListItem>
                                <asp:ListItem Value="sestra_vd">Sestra_vd</asp:ListItem>
                                <asp:ListItem Value="medix">medix</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    
                    </tr>
                    
                    
                    
                    </table>
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource,adduser_relogin %>" 
                            Font-Size="14pt" ForeColor="#FF3300"></asp:Label>
             <hr />
                
                
                <asp:Button ID="send_btn" runat="server" Text="Vlož" onclick="send_btn_Click"/>
                <asp:Button ID="uprav_btn" runat="server" Text="Uprav" onclick="uprav_btn_Click"/>
                   
            </div>
            
            
            
        
        </div>
        <div id="menu">
                 <menu:left_menu ID="Left_menu1" runat="server" /><menu:left_menu />
            
            </div>
        <div id="footer">Design by Boris Duchaj</div>
    
    
    </div>
 
    
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
