<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ransed.aspx.cs" Inherits="ransed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>

 <h2> Ranné sedenie</h2><hr />
        <div class="row">
            <div class="one half">
                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999" DayNameFormat="Shortest" 
                          ForeColor="Black" CssClass="responsive" data-max="15" 
        Height="180px" Width="200px" OnSelectionChanged="date_changed_fnc" 
        CellPadding="4">
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True"  />
                    <TitleStyle BackColor="#999999" Font-Bold="True" BorderColor="Black" />
                     </asp:Calendar>
            </div>
            <div class="one half  half-padded">

                Priezvisko: <asp:TextBox ID="name_txt" runat="server"></asp:TextBox> 
                Poznámka:<asp:TextBox ID="note_txt" runat="server"></asp:TextBox>
                Oddelenie

                <asp:DropDownList ID="odd_dl" runat="server">
                       <%-- <asp:ListItem Value="MSV">Dievčatá</asp:ListItem>
                        <asp:ListItem Value="KOJ">Kojenci</asp:ListItem>
                        <asp:ListItem Value="VD">Chlapci</asp:ListItem>--%>
                </asp:DropDownList>  
                   
                
                <asp:Button ID="pacient_add_btn" runat="server" Text="Pridaj" OnClick="add_patient_click_fnc" CssClass="button green" />
            </div>
        </div>
    
        <div class="row">
            <div class="one half-padded">
            <asp:PlaceHolder ID="osirixData_plh" runat="server">

            </asp:PlaceHolder>


<%--                <h2>Služba</h2><hr />
                <asp:PlaceHolder ID="sluzba_pl" runat="server"></asp:PlaceHolder>
                <h2>Kojenci</h2><hr />
                <asp:PlaceHolder ID="kojenci_pl" runat="server"></asp:PlaceHolder>
                <h2>Dievčatá</h2><hr />
                <asp:PlaceHolder ID="dievcata_pl" runat="server"></asp:PlaceHolder>
                <h2>Chlapci</h2><hr />
                <asp:PlaceHolder ID="chlapci_pl" runat="server"></asp:PlaceHolder>--%>
            </div>
        </div>
</asp:Content>

