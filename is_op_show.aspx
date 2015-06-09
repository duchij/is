<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_op_show.aspx.cs" Inherits="is_op_show" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>
        <asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, is_news_title %>"></asp:Label>
    </h2>
    <asp:HiddenField ID="opprogid_hf" runat="server" Value="0" />
    <asp:Button ID="print_btn" runat="server" Text="<%$ Resources:Resource,print %>" CssClass="button blue" OnClick="printDocFnc" />            
    <hr />
                               
    <%--<asp:Label ID="cela_sprava" CssClass="unstyled" runat="server" Text="" style="font-size:12px;font-family:Verdana;"></asp:Label>--%>
    <asp:Literal ID="cela_sprava" runat="server"></asp:Literal>
    <hr />
                
                
</asp:Content>


