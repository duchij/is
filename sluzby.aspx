<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sluzby.aspx.cs" Inherits="sluzby" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
 <h2> Plán služieb</h2><hr />
 <div class="row">
 <div class="one half">
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
                    </div>
                    <div class="one half">
                    Rok: 
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="changeSluzba" >
                    <asp:ListItem Value="2010">Rok 2010</asp:ListItem>
                    <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                    <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                    <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
					<asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                </asp:DropDownList>
                </div> 
</div>
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
</asp:Content>

