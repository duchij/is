<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="labels.aspx.cs" Inherits="labels_labels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Popisky......</h1>
    <hr />
    Prefix skupiny (napr. nazov_):<asp:TextBox ID="prefix_txt" runat="server"></asp:TextBox><asp:Button ID="search_btn" runat="server" Text="Hladaj" OnClick="searchPrefixFnc" />
    <hr />
    <asp:Table ID="listTable_tbl" runat="server" CssClass="responsive" data-max="15"></asp:Table>
    <hr />
    <asp:Button ID="save_btn" runat="server" CssClass="button green" Text="uloz zmeny"/> 
</asp:Content>

