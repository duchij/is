<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="vykaz2.aspx.cs" Inherits="vykaz2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
  
 <h1> Mesacny vykaz</h1>
    <div class="row">
        <div class="one half">
                <asp:Label ID="zaMesiac_lbl" runat="server"></asp:Label>
                 Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" 
                        AutoPostBack="True" OnSelectedIndexChanged = "onMonthChangedFnc">
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
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" OnSelectedIndexChanged = "onYearChangedFnc">
                    <asp:ListItem Value="2010">Rok 2010</asp:ListItem>
                    <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                    <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                    <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
                    <asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                     <asp:ListItem Value="2015">Rok 2015</asp:ListItem>
                      <asp:ListItem Value="2016">Rok 2016</asp:ListItem>
                </asp:DropDownList>
        </div>

         <div class="row">
            <div class="one">
                    <hr />
                        <asp:Label ID="predchMes_lbl" runat="server" Text="Prenos z predchadzajuceho mesiaca:"></asp:Label><asp:TextBox ID="predMes_txt" runat="server" Width="50" ></asp:TextBox>
                        <asp:Button ID="calcData_btn" runat="server" onclick="calcData_Click" Text="Vypocitaj" />
                        <asp:Button ID="createPdf_btn" runat="server" Text="Tlac" OnClick="createPdf_btn_fnc" Enabled="False" />
                    <hr />
            </div>
        </div>

        <div class="row">
            <div class="one">
                        Pocet hodin podla dni:<asp:TextBox ID="pocetHod_txt" runat="server" Width="50"></asp:TextBox>
                        <%--<asp:Label ID="pocetHod_lbl" runat="server"></asp:Label> --%>
                        Rozdiel medzi: <strong><asp:Label ID="rozdiel_lbl" runat="server"></asp:Label></strong><hr />
            </div>
        </div>

        <div class="row">

            <asp:Table ID="vykaz_tbl" runat="server" CssClass="responsive" data-max="13"></asp:Table>

        </div>


    </div>



</asp:Content>

