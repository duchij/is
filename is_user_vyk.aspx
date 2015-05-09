<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_user_vyk.aspx.cs" Inherits="is_user_vyk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
     <h1>Nastavenia pre výkaz</h1>
        <asp:Table ID="vykazSetup_tbl" runat="server" CssClass="responsive" data-max="13"></asp:Table>
        <asp:Button ID="saveVykaz_btn" runat="server" Text="Ulož nastavenie" OnClick="saveVykaz_fnc" CssClass="button green"  />
        <asp:Button ID="resetVykaz_btn" runat="server" Text="Reset nastavenia" OnClick="resetVykaz_fnc" CssClass="button red"  />


</asp:Content>

