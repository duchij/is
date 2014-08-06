<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dovolenky.aspx.cs" Inherits="dovolenky" Culture="sk-Sk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>IS-Dovolenky</title>
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
           
                 <h1>Dovolenky na mesiac: <asp:DropDownList ID="mesiac_cb" runat="server" AutoPostBack="True" >
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
                    </asp:DropDownList><asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" >
                        <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                        <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
						<asp:ListItem Value="2013">Rok 2013</asp:ListItem>
						<asp:ListItem Value="2014">Rok 2014</asp:ListItem>
						<asp:ListItem Value="2015">Rok 2015</asp:ListItem>
                    </asp:DropDownList></h1><hr />
                <asp:Label ID="Lab" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:Table ID="dovolenky_tab" runat="server" CellSpacing="2" BorderWidth="1" Width="100%" BorderColor="#990000" ></asp:Table> 
                <br /> 
                
                
                 <asp:PlaceHolder ID="vkladanie_dov" runat="server">
                 
                 <hr />
                 
                 
                 
                 
                 <table width="99%">
                 
                 <td valign="top"> Pracovník</td>
                 
                 
                 </tr>
                 <tr>
                 <td valign="top">
                     <asp:DropDownList ID="zamestnanci" runat="server" AutoPostBack="True" OnSelectedIndexChanged="changeDovStatus_fnc" >
                     </asp:DropDownList>
                 </td>
                 <td>
                 Právo na dovolenku:<br />
                 <asp:TextBox ID="dovolenkaPravo_txt" runat="server" Width="60px"></asp:TextBox>
                 </td>
                 <td>
                 Zostatok:<br /><asp:TextBox ID="dovolenkaZost_txt" runat="server" ReadOnly="true"  Width="60px"></asp:TextBox>
                 <asp:Button ID="check_btn" runat="server" Text="<%$ Resources:Resource, dov_prepoc %>" OnClick="checkDovStatusFnc" />
                 </td>
                 </tr>
                 <tr>
                 <td></td>
                 <td valign="top">Od:</td>
                 <td valign="top">Do:</td>
                 </tr>
                 <tr>
                 <td></td>
                 <td valign="top">
                     <asp:Calendar ID="od_cal" runat="server" BackColor="White" 
                         BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
                         Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
                         Width="200px">
                         <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                        <SelectorStyle BackColor="#FFCC66" />
                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#CC9966" />
                        <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                        <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                        <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                            ForeColor="#FFFFCC" />
                     </asp:Calendar>
                 </td>
                 <td valign="top">
                     <asp:Calendar ID="do_cal" runat="server" BackColor="White" 
                         BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
                         Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
                         Width="200px">
                        <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                        <SelectorStyle BackColor="#FFCC66" />
                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#CC9966" />
                        <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                        <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                        <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                            ForeColor="#FFFFCC" />
                     </asp:Calendar>
                 </td>
                 <td></td>
                 </tr>
                 
                 <tr>
                 <td colspan="3" align="right">
                     <asp:Button ID="save_btn" runat="server" Text="Uloz" onclick="save_btn_Click"  /> </td>
                 </tr>
                 
                 </table>
                  <asp:Label ID="warning_lbl" runat="server" Visible="false"></asp:Label>
                 <hr />
                 <h2> 
                     <asp:Label ID="all_vacations" runat="server" Text="<%$ Resources:Resource, vacation_all %>"></asp:Label><asp:Label ID="month_lbl" runat="server" Text=""></asp:Label></h2>
                     <asp:Table ID="zoznam_tbl" runat="server">
                     </asp:Table>
                     <hr />
                 </asp:PlaceHolder>
                
                
                <asp:PlaceHolder ID="uziv_dovolenka" runat="server">
                 
                 <hr />
                 
                 
                 
                 
                 <table width="99%">
                 
                 
                 
                 <tr>
                 <td colspan="3">
                   <h2> <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, vacation_select %>"></asp:Label></h2> </td></tr>
                   <tr>
                   <td></td>
                 <td valign="top"><strong>OD:</strong></td>
                 <td valign="top"><strong>DO:</strong></td>
                 </tr>
                 <tr>
                <td></td>
                 <td valign="top">
                     <asp:Calendar ID="dovOd_user" runat="server" BackColor="White" 
                         BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
                         Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
                         Width="200px">
                         <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                        <SelectorStyle BackColor="#FFCC66" />
                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#CC9966" />
                        <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                        <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                        <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                            ForeColor="#FFFFCC" />
                     </asp:Calendar>
                 </td>
                 <td valign="top">
                     <asp:Calendar ID="dovDo_user" runat="server" BackColor="White" 
                         BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
                         Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
                         Width="200px">
                        <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                        <SelectorStyle BackColor="#FFCC66" />
                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#CC9966" />
                        <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                        <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                        <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                            ForeColor="#FFFFCC" />
                     </asp:Calendar>
                 </td>
                 <td></td>
                 </tr>
                 
                 <tr>
                 <td colspan="3" align="right">
                     <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Resource, save %>" onclick="save_user_btn_Click"  /> </td>
                 </tr>
                 
                 </table>
                  <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                 <hr />
                <h2>Zoznam dovoleniek aktuálneho užívateľa pre mesiac <asp:Label ID="monthUser_lbl" runat="server" Text=""></asp:Label></h2>
                     <asp:Table ID="zoznamUser_tbl" runat="server">
                     </asp:Table>
                     <hr />
                 </asp:PlaceHolder>
                
                
                
                 
                  
                 
                
            
            
            
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
