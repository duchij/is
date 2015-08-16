<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_opkniha.aspx.cs" Inherits="is_opkniha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1> Hľadanie v operančnej knihe</h1>
    <hr /> Roky 2000-2014 spolu - <asp:Label ID="row_counts" runat="server" Text=""></asp:Label>
    <hr />
    <div class="row">
        <p>Hľadaj podľa časti slova/slov (oddelene medzerami) v diagnozach: <asp:TextBox ID="queryDg_txt" runat="server"></asp:TextBox><asp:Button ID="search_dg_btn" runat="server" Text="<%$ Resources:Resource,search %>" OnClick="searchInDgFnc" /></p>
        <p>Hľadaj podľa časti slova/slov (oddelene medzerami) vo vykonoch: <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><%--<asp:Button ID="search_op_btn" runat="server" Text="<%$ Resources:Resource,search %>" OnClick="searchInOpFnc" />--%></p>
     <%--   <p>Hľadaj podľa štatistikého kódu operačiek:<asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList><asp:Button ID="Button3" runat="server" Text="<%$ Resources:Resource,search %>" /></p>
        <p>Hľadaj podľa operatér/asistent: <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><asp:Button ID="Button4" runat="server" Text="<%$ Resources:Resource,search %>" /></p>--%>
    </div>
    <hr />
    <asp:GridView ID="GridView1" runat="server" ></asp:GridView>

</asp:Content>

