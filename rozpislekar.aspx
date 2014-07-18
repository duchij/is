<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rozpislekar.aspx.cs" Inherits="sluzby" Culture="sk-SK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>IS-Rozpis lekarov</title>
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
    
                <h2> Rozpis lekárov</h2><hr />
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
                </asp:DropDownList>
                Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text="Label"></asp:Label><hr />
                <asp:Label ID="Label1" runat="server" Text="Víkend" BackColor="#990000" ForeColor="Yellow" Width="130"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="Štátny sviatok" BackColor="Yellow" ForeColor="#990000" Width="130"></asp:Label><br />
      <asp:Button ID="Button2" runat="server" Text="Uložiť" onclick="Button1_Click" />
                <asp:CheckBox ID="CheckBox1" runat="server" Text="Publikovať"  Checked="True" />
                <asp:Button ID="Button3" runat="server" Text="do Wordu" 
                        onclick="toWord_btn_Click" />
                
                    <asp:Button ID="Button4" runat="server" onclick="print_btn_Click" 
                        Text="Tlačiť" /><asp:Button ID="Button5" runat="server" onclick="prevMonth_Click" 
                        Text="<%$ Resources:Resource, odd_rozpis_prev %>" /><hr />
                        <asp:Label ID="vypis1_lbl" runat="server" Text="" ></asp:Label>
                        <hr />
                <table border="0" cellpadding="0">
                <tr>
                    <td width="40" align="center"><b>Deň</b></td>
                    <td width="130" align="center"><b>OUP</b></td>
                    <td width="130" align="center"><b>Všeob.amb.</b></td>
                    <td width="130" align="center"><b>Trauma.amb</b></td>
                    <td width="130" align="center"><b>Kojenci</b></td>
                    <td width="130" align="center"><b>Dievčatá</b></td>
                    <td width="130" align="center"><b>Veľké deti</b></td>
                
                </tr>
                </table>
    
                <asp:Table ID="Table1" runat="server" BorderWidth="1" BorderColor="#990000" CellPadding="0" CellSpacing="0" Width="100%"></asp:Table>
                <asp:Button ID="Button1" runat="server" Text="Uložiť" onclick="Button1_Click" />
                <asp:CheckBox ID="publish_ck" runat="server" Text="Publikovať" Checked="true" />
                <asp:Button ID="toWord_btn" runat="server" Text="do Wordu" 
                        onclick="toWord_btn_Click" />
                
                    <asp:Button ID="print_btn" runat="server" onclick="print_btn_Click" 
                        Text="Tlačiť" />
                        
                        <asp:Button ID="Button6" runat="server" onclick="prevMonth_Click" 
                        Text="<%$ Resources:Resource, odd_rozpis_prev %>" />
                
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
