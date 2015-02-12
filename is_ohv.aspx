<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_ohv.aspx.cs" Inherits="is_ohv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>OHV Kody</h1>
    <hr />
    <div class="row">
        <div class="one third">
             <asp:DropDownList ID="insurance_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="loadCodecsFnc">
        <asp:ListItem Value="24" Text="24 Dovera"></asp:ListItem>
        <asp:ListItem Value="25" Text="25 Vseobecna zdrav. poistovna"></asp:ListItem>
        <asp:ListItem Value="27" Text="27 Union"></asp:ListItem>
    </asp:DropDownList>

        </div>
        <div class="one third">
            <asp:DropDownList ID="clgroup_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="loadCodecsFnc"></asp:DropDownList>

        </div>
        <div class="one third">
            <asp:Button ID="allCodes_btn" runat="server" OnClick="loadAllCodesFnc" CssClass="green button" Text="Nahraj vsetky kody" />

        </div>


    </div>
   
    
    
    <hr />
    <asp:Table ID="ohvCode_tbl" runat="server" CssClass="responsive" data-max="15"></asp:Table>

</asp:Content>

