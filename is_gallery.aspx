<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_gallery.aspx.cs" Inherits="is_gallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Kamera gallery</h1>
    <asp:Literal runat="server" ID="msg_lit"></asp:Literal>
    <hr />
    <table>
        <tr>
            <td>Hľadaj podľa:</td>
            <td>
                    <asp:DropDownList runat="server" ID="searchBy">
                    <asp:ListItem Text="Mena" Value="name"></asp:ListItem>
                    <asp:ListItem Text="Rodné číslo" Value="bin_num"></asp:ListItem>
                    <asp:ListItem Text="Diagnóza" Value="diagnose"></asp:ListItem>
                    <asp:ListItem Text="Dátum (rrrr-mm-dd)" Value="photoDate"></asp:ListItem>
                </asp:DropDownList>

            </td>

            <td>
                <asp:TextBox runat="server" ID="queryString" Text=""></asp:TextBox>

            </td>
            <td><asp:Button runat="server" ID="searchExec_btn"  CssClass="green button" Text="Hladaj"/></td>


        </tr>

    </table>
     
    
   
    <hr />
    <asp:table runat="server" ID="files_tbl" CssClass="responsive" data-max="12"></asp:table>
    

</asp:Content>

