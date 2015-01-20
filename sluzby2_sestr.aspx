<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  CodeFile="sluzby2_sestr.aspx.cs" Inherits="sluzby2_sestr" MaintainScrollPositionOnPostback="true" Culture="sk-SK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
    <h2> Plán služieb</h2><hr />
        <div class="row">
            <div class="one third">
             Oddelenie:          
                <asp:DropDownList ID="deps_dl" runat="server" OnSelectedIndexChanged="changeDeps_fnc"></asp:DropDownList>

            </div>
            <div class="one third">
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
                    <div class="one third">
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
                </div> 
            </div>
 Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text="Label"></asp:Label><hr />
                <asp:Label ID="Label1" runat="server" Text="Víkend" CssClass="red box" Width="130"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="Štátny sviatok" CssClass="yellow box" Width="130"></asp:Label>
                <asp:Table ID="shiftTable" runat="server" EnableViewState="true" CssClass="responsive" data-max="14">                 
                </asp:Table>
                <div class="row">
                    <div class="one whole padded">
                    <asp:CheckBox ID="publish_cb" runat="server" OnCheckedChanged="changePublishStatus" AutoPostBack="true" CssClass="mojInline"  />
                        <asp:Label ID="publish_lbl" runat="server" Text="Uvernejniť" CssClass="mojInline"></asp:Label>
                        </div>
                    <asp:Button ID="toWord_btn" runat="server" Text="do Wordu" OnClick="publishSluzby"/>
                
                    <asp:Button ID="print_btn" runat="server"  Text="Tlačiť" OnClick="publishSluzby" />
                </div>
</asp:Content>

