<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="staze.aspx.cs" Inherits="staze" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
<h2> Plán stáží</h2><hr />
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
					<asp:ListItem Value="2015">Rok 2015</asp:ListItem>
					<asp:ListItem Value="2016">Rok 2016</asp:ListItem>
                </asp:DropDownList>
                Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text="Label"></asp:Label><hr />
                <asp:Label ID="vikend_lbl" runat="server" Text="Víkend" BackColor="#990000" ForeColor="Yellow" Width="130"></asp:Label>
                <asp:Label ID="stat_lbl" runat="server" Text="Štátny sviatok" BackColor="Yellow" ForeColor="#990000" Width="130"></asp:Label>
                <table border="0" cellpadding="0">
                <tr>
                    <td width="130" align="center"><b><asp:Label ID="Label1" Text="<%$ Resources:Resource, staze_den %>" runat="server"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, staze_3 %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, staze_4 %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, staze_5 %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, staze_6 %>"></asp:Label></b></td>
                    <td width="130" align="center"><b><asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, staze_note %>"></asp:Label></b></td>
                
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
</asp:Content>

