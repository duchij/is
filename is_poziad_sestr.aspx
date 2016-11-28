<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_poziad_sestr.aspx.cs" Inherits="is_poziad_sestr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Požiadavky na služby...</h1>
    <hr />
    <div class="info box">
        Pre každý deň, sa Vám zobrazí možnosť výberu <strong> Áno/Nie/Dovolenka alebo Áno/Nie deňka, Áno/Nie nočná</strong>. Ak nezadáte nič berie sa to, že nemáte na tento deň požiadavku.
        Vaše požiadavky sa presunú do plánovania staničných sestier, t.j. staničná uvidí pre každý deň Vašu požiadavku.
        Je len teda nanej ako sa tejto požiadavke postaví....<br />

        <span class="red">
            Dátum odovzdania požiadaviek si stanovuje staničná sestra, ak ich nahodíte potom čo už spravila plán nemusí Vám  ich akceptovať...
        </span>



    </div>
    <div class="row">
        <div class="one half">
            <strong> Rok:</strong> <asp:DropDownList ID="month_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="redrawTable" ></asp:DropDownList>
        </div>
        <div class="one half inline">
            <strong> Mesiac:</strong><asp:DropDownList ID="years_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="redrawTable"></asp:DropDownList>
        </div>
  
    </div>
    <div class="row">
        <asp:Table ID="poziadTable_tbl" runat="server" CssClass="responsive" data-max="15"></asp:Table>
    </div>
</asp:Content>

