<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeFile="staze.aspx.cs" Inherits="staze" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
<h2> Plán stáží</h2><hr />
    <div class="row">
        <div class="one half">
                Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" 
                        AutoPostBack="True" OnSelectedIndexChanged="changeStazeFnc" >
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
        <div class="one half" >
                    Rok: 
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="changeStazeFnc" >
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
    <div class="row">
                Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text="Label"></asp:Label><hr />
                <asp:Label ID="vikend_lbl" runat="server" Text="Víkend" CssClass="red box"></asp:Label>
                <asp:Label ID="stat_lbl" runat="server" Text="Štátny sviatok" CssClass="yellow box"></asp:Label>
    </div>
    <div class="row">       
    
               <p> <asp:Table ID="stazeTable_tbl" runat="server" CssClass="responsive" data-max="15"></asp:Table></p>
   </div>    
    <div class="row">
        <asp:PlaceHolder ID="setState_pl" runat="server" Visible="false">
                <asp:Button ID="publish_btn" runat="server" Text="Sprístupniť všetkým" CssClass="green button" OnClick="publishFnc"  />
                <asp:Button ID="unpublish_btn" runat="server" Text="Zblokovať prístup všetkým" CssClass="red button" OnClick="publishFnc"/>
        </asp:PlaceHolder>        
                <asp:Button ID="word_btn" runat="server" Text="do Wordu" OnClick="printFnc" />
                
               <asp:Button ID="print_btn" runat="server" Text="Tlačiť" OnClick="printFnc"/>
              <%-- <asp:Label ID="vypis_lbl" runat="server" Text="" ></asp:Label>--%>
        </div>
</asp:Content>

