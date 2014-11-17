<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="poziadavky.aspx.cs" Inherits="poziadavky" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>

<h2>
                    <asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_title %>"></asp:Label></h2>
                <hr />
                Prihlasený: <strong>
                    <asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <%-- Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />--%>
                <hr />
                <asp:PlaceHolder ID="admin_section" runat="server">
                    <asp:Label ID="lock_info_lbl" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_info %>"></asp:Label>
                    <strong><asp:Label ID="poziadav_lbl" runat="server" Text=""></asp:Label></strong>
                    <br />
                    <br />
                    <strong><asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_next %>"></asp:Label></strong>
                   
                    <asp:Calendar runat="server" ID="lock_date" BackColor="#FFFFCC" BorderColor="#FFCC66"
                        DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399"
                        Height="200px" Width="220px" BorderWidth="1px" ShowGridLines="True" TitleFormat="MonthYear">
                        <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                        <SelectorStyle BackColor="#FFCC66" />
                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#CC9966" />
                        <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                        <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                        <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
                    </asp:Calendar>
                    <asp:Button ID="lock_date_btn" runat="server" Text="<%$ Resources:Resource, odd_lock_btn %>"
                        OnClick="savePoziadavka_fnc_Click" /><hr />
                    <asp:Button ID="poziadavky_btn" runat="server" Text="<%$ Resources:Resource, odd_print_poz_btn %>"
                        OnClick="printPoziadavka_fnc_Click" />
                </asp:PlaceHolder>
                <hr />
                <asp:PlaceHolder ID="user_section" runat="server">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_info %>"></asp:Label>
                    <strong><asp:Label ID="poziadav2_lbl" runat="server" Text=""></asp:Label></strong><br /><br />
                    <asp:Label ID="title_info" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_title %>"> </asp:Label>
                    <asp:Label ID="title_info_date" runat="server" Text=" mesiac"></asp:Label>
                    <asp:DropDownList ID="mesiac_cb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="getPoziadavky_fnc">
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
                    <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="getPoziadavky_fnc">
                        <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                        <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                         <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
                          <asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:TextBox ID="poziadavky_txt" runat="server" TextMode="MultiLine" Rows="10" Width="400px"></asp:TextBox><br />
                    <asp:Button ID="save_btn" runat="server" Text="<%$ Resources:Resource, odd_save %>"
                        OnClick="saveUserPoziadav_Click" PostBackUrl="poziadavky.aspx" />
                </asp:PlaceHolder>
</asp:Content>

