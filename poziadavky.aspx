<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="poziadavky.aspx.cs" Inherits="poziadavky" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="row">  
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>

<h2><asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_title %>"></asp:Label></h2>

                <hr />
                Prihlasený: <strong>
                    <asp:Label ID="user" runat="server" Text="" ForeColor="#990000"></asp:Label></strong><br />
                <%-- Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />--%>
                <hr />
                <asp:PlaceHolder ID="admin_section" runat="server">
                    <asp:Label ID="lock_info_lbl" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_info %>"></asp:Label>
                    <strong><asp:Label ID="poziadav_lbl" runat="server" Text=""></asp:Label></strong>
                    <br />
                    <br />
                    <strong><asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_next %>"></asp:Label></strong>
                   
                     <asp:Calendar ID="lock_date" runat="server" BackColor="White"
                    BorderColor="#d9edf7" CellPadding="4" DayNameFormat="Shortest"
                    ForeColor="Black" CssClass="responsive" data-max="15">
                    <SelectedDayStyle BackColor="#46627f" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#46627f" />
                        <WeekendDayStyle BackColor="#dff0d8" />
                        <TodayDayStyle BackColor="#f0e6f4" ForeColor="black" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#ffe5c7" Font-Bold="True" />
                        <TitleStyle BackColor="#d9edf7" BorderColor="Black" Font-Bold="True" />
                </asp:Calendar>



                    <asp:Button ID="lock_date_btn" runat="server" Text="<%$ Resources:Resource, odd_lock_btn %>"
                        OnClick="savePoziadavka_fnc_Click" CssClass="blue button" /><hr />
                    <asp:Button ID="poziadavky_btn" runat="server" Text="<%$ Resources:Resource, odd_print_poz_btn %>"
                        OnClick="printPoziadavka_fnc_Click" CssClass="yellow button" />
                </asp:PlaceHolder>

                <hr />
    
                <asp:PlaceHolder ID="user_section" runat="server">
                    <div class="row">
                        <div class="one third">
                    <!--<asp:Label ID="title_info" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_title %>"> </asp:Label>-->
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_info %>"></asp:Label>
                            <strong><asp:Label ID="poziadav2_lbl" runat="server" Text=""></asp:Label></strong><br /><br />

                         
                            <!--<asp:Label ID="title_info_date" runat="server" Text=" mesiac   "></asp:Label>-->

                        </div>
                        <div class="one third">
                            <asp:DropDownList ID="mesiac_cb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="getPoziadavky_fnc">
                        
                            </asp:DropDownList>
                            <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="getPoziadavky_fnc">
                       
                            </asp:DropDownList>
                        </div>
                        <div class="one third">
                            <asp:TextBox ID="poziadavky_txt" runat="server" TextMode="MultiLine" Rows="10" Width="400px"></asp:TextBox><br />
                            <asp:Button ID="save_btn" runat="server" Text="<%$ Resources:Resource, odd_save %>"
                            OnClick="saveUserPoziadav_Click" CssClass="button green" PostBackUrl="poziadavky.aspx" />
                        </div>
                    </div>
                </asp:PlaceHolder>
        <hr />
</div> 
</asp:Content>
 

