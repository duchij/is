<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="dovolenky_sestr.aspx.cs" Inherits="dovolenky_sestr" Culture="sk-Sk" MaintainScrollPositionOnPostback="true" UICulture="sk-SK" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
    <h1>Aktivity na mesiac:</h1>
    <div class="row">
        <div class="one fourth">
            <asp:DropDownList ID="mesiac_cb" runat="server" AutoPostBack="True" >
                <asp:ListItem Value="1" Text="<%$ Resources:Resource,date_januar %>"></asp:ListItem>
                <asp:ListItem Value="2" Text="<%$ Resources:Resource,date_februar %>">Február</asp:ListItem>
                <asp:ListItem Value="3" Text="<%$ Resources:Resource,date_march %>">Marec</asp:ListItem>
                <asp:ListItem Value="4" Text="<%$ Resources:Resource,date_april %>">Apríl</asp:ListItem>
                <asp:ListItem Value="5" Text="<%$ Resources:Resource,date_maj %>">Máj</asp:ListItem>
                <asp:ListItem Value="6" Text="<%$ Resources:Resource,date_june %>">Jún</asp:ListItem>
                <asp:ListItem Value="7" Text="<%$ Resources:Resource,date_july %>">Júl</asp:ListItem>
                <asp:ListItem Value="8" Text="<%$ Resources:Resource,date_august %>">August</asp:ListItem>
                <asp:ListItem Value="9" Text="<%$ Resources:Resource,date_september %>">September</asp:ListItem>
                <asp:ListItem Value="10" Text="<%$ Resources:Resource,date_october %>">Október</asp:ListItem>
                <asp:ListItem Value="11" Text="<%$ Resources:Resource,date_november %>">November</asp:ListItem>
                <asp:ListItem Value="12" Text="<%$ Resources:Resource,date_december %>">December</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="one fourth">

            <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True">
                <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
                <asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                <asp:ListItem Value="2015">Rok 2015</asp:ListItem>
                <asp:ListItem Value="2016">Rok 2016</asp:ListItem>
                <asp:ListItem Value="2017">Rok 2017</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="one fourth">
         Typ:<asp:DropDownList ID="activies_dl" runat="server" CssClass="mojInline" AutoPostBack="True" Width="150">
                         <asp:ListItem Value="do" Text="<%$ Resources:Resource,free_do %>"></asp:ListItem>
                          <asp:ListItem Value="pn" Text="<%$ Resources:Resource,free_pn %>"></asp:ListItem>
                        <asp:ListItem Value="sk" Text="<%$ Resources:Resource,free_sk %>"></asp:ListItem>
                        <asp:ListItem Value="ci" Text="<%$ Resources:Resource,free_ci %>"></asp:ListItem>
                        <asp:ListItem Value="ko" Text="<%$ Resources:Resource,free_ko %>"></asp:ListItem>
                        <asp:ListItem Value="le" Text="<%$ Resources:Resource,free_le %>"></asp:ListItem>
                        </asp:DropDownList>
            </div>
         <div class="one fourth">
        <asp:Button ID="kompl_btn" runat="server" Text="Kompletný prehlad" CssClass="green button" OnClick="loadKomplViewFnc" />
             </div>

    </div>
    <hr />
    <asp:Label ID="Lab" runat="server" Text="" Visible="false"></asp:Label>

    <asp:Table ID="dovolenky_tab" runat="server" CssClass="responsive" data-max="15"></asp:Table>
    <br />

    <asp:PlaceHolder ID="uziv_dovolenka" runat="server">

        <hr />
        <div class="row">
            <div class="one">
                <h2>
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, vacation_select %>"></asp:Label></h2>
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, date_info_lbl %>"></asp:Label>
            </div>
        </div>

        <div class="row">
            <div class="one half">

                <h2 class="green">OD:</h2>
                <asp:Calendar ID="dovOd_user" runat="server" BackColor="White"
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
            </div>


            <div class="one half">
                <h2 class="green">DO:</h2>

                <asp:Calendar ID="dovDo_user" runat="server" BackColor="White"
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
            </div>
        </div>
               <br /><br />

                <div class="row">
                    <div class="one fourth">
                        Sestra:<asp:DropDownList ID="nurses_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="loadDovByIdFnc" Width="150" CssClass="mojInline"></asp:DropDownList></div>
                    <div class="one fourth">
                    <asp:DropDownList ID="freeType_dl" runat="server">
                         <asp:ListItem Value="do" Text="<%$ Resources:Resource,free_do %>"></asp:ListItem>
                          <asp:ListItem Value="pn" Text="<%$ Resources:Resource,free_pn %>"></asp:ListItem>
                        <asp:ListItem Value="sk" Text="<%$ Resources:Resource,free_sk %>"></asp:ListItem>
                        <asp:ListItem Value="ci" Text="<%$ Resources:Resource,free_ci %>"></asp:ListItem>
                        <asp:ListItem Value="ko" Text="<%$ Resources:Resource,free_ko %>"></asp:ListItem>
                         <asp:ListItem Value="le" Text="<%$ Resources:Resource,free_le %>"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                    <div class="one fourth"><asp:TextBox ID="comment_txt" runat="server" CssClass="mojInline" Width="250" placeholder="Poznamka"></asp:TextBox></div>
                    <div class="one fourth align-right">
                    <asp:Button ID="Button2" runat="server" CssClass="large button blue" Text="<%$ Resources:Resource, save %>" OnClick="save_user_btn_Click" />
                        </div>         <asp:Label ID="statusInfo_lbl" runat="server" Visible="true" Text=""></asp:Label>
        <hr />

                         </div>
        <h2>
            <asp:Label ID="holiday_for_user" runat="server" Text="<%$ Resources:Resource, holiday_for_user_month %>"></asp:Label>
            <asp:Label ID="monthUser_lbl" runat="server" Text=""></asp:Label></h2>
        <asp:Table ID="zoznamUser_tbl" runat="server" style="font-size:large;">
        </asp:Table>
        <hr />
    </asp:PlaceHolder>

</asp:Content>

