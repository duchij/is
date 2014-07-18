<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dov_stat.aspx.cs" Inherits="dov_stat" Culture="sk-Sk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>IS - Dovolenky stav </title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
    <div id="header">
        
     <h1>Informačný systém Kliniky detskej chirurgie LF UK a DFNsP</h1> 
     <duch:my_header ID="My_header1" runat="server"></duch:my_header>
    
    </div>
        
        <div id="content">
           
             <div id="menu2">
           <info:info_bar ID="info_bar" runat="server" />
            <info:info_bar />
        </div>
            <div id="cont_text">
                 <h1>Dovolenky - Zostatok</h1><hr />
                
               
                 
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                            BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="4" DataSourceID="ObjectDataSource1" Width="505px">
                    <RowStyle BackColor="White" ForeColor="#330099" />
                    <Columns>
                        <asp:BoundField DataField="full_name" HeaderText="Meno" />
                        <asp:BoundField DataField="zostatok" HeaderText="Zostatok" />
                        <asp:BoundField DataField="narok" HeaderText="Nárok" />
                    </Columns>
                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                </asp:GridView>
                 
                
            
            
            
        
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                            SelectMethod="getAllStatusDov" TypeName="my_db"></asp:ObjectDataSource>
                 
                
            
            
            
        <br /><br />
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
