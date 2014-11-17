<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rozpislekar.aspx.cs" Inherits="rozpislekar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>

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
</asp:Content>

