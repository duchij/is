<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ransed.aspx.cs" Inherits="ransed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>

 <h2> Ranné sedenie</h2><hr />
                
                 Vloz pacienta:<br />
                <asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" 
                         BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" 
                         Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399" Height="200px" 
                         ShowGridLines="True" Width="220px" OnSelectionChanged="date_changed_fnc">
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
</asp:Content>

