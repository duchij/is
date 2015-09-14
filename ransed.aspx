<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ransed.aspx.cs" Inherits="ransed" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>

 <h2> Ranné sedenie</h2><hr />
        <div class="row">
            <div class="one half">
               <asp:Calendar ID="Calendar1" runat="server" BackColor="White" OnSelectionChanged="date_changed_fnc"  CssClass="responsive" data-max="12"
                            BorderColor="#d9edf7" DayNameFormat="Shortest"  
                              ForeColor="Black" 
                             CellPadding="2" NextPrevStyle-Width="10%" DayHeaderStyle-Wrap="False" TitleStyle-HorizontalAlign="Left" NextPrevFormat="CustomText" TitleFormat="MonthYear" TitleStyle-Wrap="True" TitleStyle-VerticalAlign="Top" NextPrevStyle-HorizontalAlign="Left" TitleStyle-Width="100%" SelectorStyle-Wrap="False">
                        <SelectedDayStyle BackColor="#46627f" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#46627f" />
                        <WeekendDayStyle BackColor="#dff0d8" />
                        <TodayDayStyle BackColor="#f0e6f4" ForeColor="black" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#ffe5c7" Font-Bold="True" />
                        <TitleStyle BackColor="#d9edf7" BorderColor="Black" Font-Bold="True" />
                        </asp:Calendar>
            </div>
            <div class="one half  half-padded">
                <h2 class="blue">Pridaj pacienta na rannú diagnostiku...</h2>
                Priezvisko: <asp:TextBox ID="name_txt" runat="server"></asp:TextBox> 
                Poznámka:<asp:TextBox ID="note_txt" runat="server"></asp:TextBox>
                <p>Sem napíšte krátku epikrízu, alebo to čo chcete prebrať na rannom sedení...</p>
                Oddelenie

                <asp:DropDownList ID="odd_dl" runat="server" EnableViewState="true">
                       <%-- <asp:ListItem Value="MSV">Dievčatá</asp:ListItem>
                        <asp:ListItem Value="KOJ">Kojenci</asp:ListItem>
                        <asp:ListItem Value="VD">Chlapci</asp:ListItem>--%>
                </asp:DropDownList>  
                   
                
                <asp:Button ID="pacient_add_btn" runat="server" Text="Pridaj" OnClick="add_patient_click_fnc" CssClass="button green" />
            </div>
        </div>
    
        <div class="row">
            <div class="one half-padded">
            <asp:PlaceHolder ID="osirixData_plh" runat="server" EnableViewState="false">

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

