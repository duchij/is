<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="vykaz.aspx.cs" Inherits="vykaz"  Culture="Sk-sk" ValidateRequest="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />


    <title>IS-Vykaz</title>
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
                <h1> Mesacny vykaz</h1>
                <asp:Label ID="zaMesiac_lbl" runat="server"></asp:Label>
                
                 Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" 
                        AutoPostBack="True" OnSelectedIndexChanged = "onMonthChangedFnc">
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
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" OnSelectedIndexChanged = "onYearChangedFnc">
                    <asp:ListItem Value="2010">Rok 2010</asp:ListItem>
                    <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                    <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                    <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
                     <asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                </asp:DropDownList>
                
                
                <hr />
                    <asp:Label ID="predchMes_lbl" runat="server" Text="Prenos z predchadzajuceho mesiaca:"></asp:Label><asp:TextBox ID="predMes_txt"
                        runat="server" Width="50"></asp:TextBox>
                        <asp:Button ID="calcData_btn" runat="server" onclick="calcData_Click" Text="Vypocitaj" />
                <asp:Button ID="createPdf_btn" runat="server" Text="Tlac" 
                    OnClick="createPdf_btn_fnc" Enabled="False" />
                    <hr />
                    Pocet hodin podla dni:<asp:TextBox ID="pocetHod_txt" runat="server" Width="50"> </asp:TextBox><%--<asp:Label ID="pocetHod_lbl" runat="server"></asp:Label> --%>Rozdiel medzi: <strong><asp:Label ID="rozdiel_lbl" runat="server"></asp:Label></strong><br /><hr />
                      <table border="1" style="border:none;">
                        <tr>
                            <td width="45" style="font-size:9px;" ></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="prichod_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="odchod_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="hodiny_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="nocpraca_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="mzdovzvyh_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="sviatok_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="a1_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="a2_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="nea1_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="nea2_lbl" runat="server"></asp:label></td>
                            <td width="45" style="font-size:9px;"><asp:label ID="nea3_lbl" runat="server"></asp:label></td>
                        </tr>
                      
                      
                      </table> 
                      
                    
                
                <table border="1" style="border:none;">
                <tr>
                    <td width="45" style="font-size:9px;" >den</td>
                    <td width="45" style="font-size:9px;">prichod</td>
                    <td width="45" style="font-size:9px;">odchod</td>
                    <td width="45" style="font-size:9px;">hodiny</td>
                    <td width="45" style="font-size:9px;">nocna praca</td>
                    <td width="45" style="font-size:9px;">Mzdove zvyhod</td>
                    <td width="45" style="font-size:9px;">sviatok</td>
                    <td width="45" style="font-size:9px;">AI</td>
                    <td width="45" style="font-size:9px;">AII</td>
                    <td width="45" style="font-size:9px;">NeA I</td>
                    <td width="45" style="font-size:9px;">NeA II</td>
                    <td width="45" style="font-size:9px;">NeA III</td>
                
                </tr>
                </table>
                
                <asp:Table ID="vykaz_tbl" runat="server"></asp:Table>
                
                    
                    
                
                </div>
        </div>
        
        <div id="menu">
                <menu:left_menu ID="Left_menu1" runat="server" /><menu:left_menu />
            
            </div>
        <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
        
        <div id="footer">Design by Boris Duchaj</div>
    
    
    </div>
    </form>
</body>
</html>
