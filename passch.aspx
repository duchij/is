<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="passch.aspx.cs" Inherits="passch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>Zmena hesla:</h2>
                <hr />
                
                <asp:Label ID="info_lbl" runat="server" Text="Label" Visible="False"></asp:Label><br /><br />
                
                <asp:PlaceHolder ID="changePassw" runat="server">
                <table border="0">
                <tr><td>Nové heslo:</td><td><asp:TextBox ID="passwd1" runat="server" TextMode="Password"></asp:TextBox></td></tr>
               <tr>
               <td>Kontrola hesla:</td>
               <td><asp:TextBox ID="passwd2" runat="server" TextMode="Password"></asp:TextBox></td>
              </tr>
                 </table>
                <asp:Button ID="change_btn" runat="server" Text="Zmeň" 
                    onclick="change_btn_Click" />
                   
                    </asp:PlaceHolder>
</asp:Content>

