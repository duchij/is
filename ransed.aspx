<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ransed.aspx.cs" Inherits="ransed"  Culture="sk-SK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>Ranna diagnostika KDCH LF UK a DFNsP</title>
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
                     <h2> Ranné sedenie</h2><hr />
                
                 Vloz pacienta:<br />
                <asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" 
                         BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" 
                         Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399" Height="200px" 
                         ShowGridLines="True" Width="220px">
                    <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                    <SelectorStyle BackColor="#FFCC66" />
                    <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                    <OtherMonthDayStyle ForeColor="#CC9966" />
                    <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                    <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                    <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                        ForeColor="#FFFFCC" />
                     </asp:Calendar>
                 Priezvisko: 
                <asp:TextBox ID="name_txt" runat="server"></asp:TextBox>  
                Poznámka:<asp:TextBox ID="note_txt" runat="server"></asp:TextBox>
                Oddelenie
                <asp:DropDownList ID="odd_dl" runat="server">
                        <asp:ListItem Value="MSV">Dievčatá</asp:ListItem>
                        <asp:ListItem Value="KOJ">Kojenci</asp:ListItem>
                        <asp:ListItem Value="VD">Chlapci</asp:ListItem>
                </asp:DropDownList>    
                <br />
                <asp:Button ID="pacient_add_btn" runat="server" Text="Pridaj" OnClick="add_patient_click_fnc" />
                <hr />
                <h2>Služba</h2><hr />
                <asp:PlaceHolder ID="sluzba_pl" runat="server"></asp:PlaceHolder>
                <h2>Kojenci</h2><hr />
                <asp:PlaceHolder ID="kojenci_pl" runat="server"></asp:PlaceHolder>
                <h2>Dievčatá</h2><hr />
                <asp:PlaceHolder ID="dievcata_pl" runat="server"></asp:PlaceHolder>
                <h2>Chlapci</h2><hr />
                <asp:PlaceHolder ID="chlapci_pl" runat="server"></asp:PlaceHolder>
                
                        
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
