<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sluzby.aspx.cs" Inherits="sluzby" Culture="sk-SK" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>IS-Plan sluzieb</title>
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
    
                <h2> Plán služieb</h2><hr />
                Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" 
                        AutoPostBack="True" onselectedindexchanged="changeSluzba" >
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
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="changeSluzba" >
                    <asp:ListItem Value="2010">Rok 2010</asp:ListItem>
                    <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                    <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                    <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
					<asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                </asp:DropDownList>
                Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text="Label"></asp:Label><hr />
                <asp:Label ID="Label1" runat="server" Text="Víkend" BackColor="#990000" ForeColor="Yellow" Width="130"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="Štátny sviatok" BackColor="Yellow" ForeColor="#990000" Width="130"></asp:Label>
                <table border="0" cellpadding="0" >
                <tr>
                    <td width="130" align="center"><b>Den</b></td>
                    <td width="130" align="center"><b>OUP</b></td>
                    <td width="130" align="center"><b>A</b></td>
                    <td width="130" align="center"><b>B</b></td>
                    <td width="130" align="center"><b>Pohotovost</b></td>
                    <td width="130" align="center"><b>Prijmova AMB</b></td>
                
                </tr>
                </table>
    
                <asp:Table ID="Table1" runat="server" BorderWidth="1" BorderColor="#990000" CellPadding="0" CellSpacing="0" Width="100%"></asp:Table>
                <asp:Button ID="Button1" runat="server" Text="Uložiť" onclick="Button1_Click" />
                <asp:CheckBox ID="publish_ck" runat="server" Text="Publikovať" />
                <asp:Button ID="toWord_btn" runat="server" Text="do Wordu" 
                        onclick="toWord_btn_Click" />
                
                    <asp:Button ID="print_btn" runat="server" onclick="print_btn_Click" 
                        Text="Tlačiť" />
                
                <br />
                <br />
                        <asp:Label ID="vypis_lbl" runat="server" Text="" ></asp:Label>
                        
                        
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
