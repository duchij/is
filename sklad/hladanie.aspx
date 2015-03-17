<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hladanie.aspx.cs" Inherits="sklad_hladanie" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sklad hladanie</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Hladanie v sklade</h1>
        <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
        <hr />
        <asp:DropDownList ID="searchIn_dl" runat="server">
                <asp:ListItem Value="nazov">V nazve</asp:ListItem>
                <asp:ListItem Value="sukl">Dla sukl kodu</asp:ListItem>
                <asp:ListItem Value="ean">Podla eanu</asp:ListItem>

        </asp:DropDownList>
        <asp:TextBox ID="phrase_txt" runat="server" Width="200px"></asp:TextBox>
        <asp:Button ID="search_btn" runat="server" Text="Hladaj" OnClick="searchFnc" />
        <hr />
        <asp:Table ID="result_tbl" runat="server"></asp:Table>

        <asp:PlaceHolder ID="tovarDetail_pl" runat="server" Visible="true" >
            <table>
                <tr>
                    <td>Nazov tovaru:</td><td>
                        <asp:Label ID="nazov_lbl" runat="server" Text=""></asp:Label> </td>
                </tr>
                <tr>
                    <td>Sukl kod:</td>
                    <td>
                    <asp:Label ID="sukl_lbl" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td>Ean 1</td>
                    <td><asp:TextBox ID="ean1_txt" runat="server"></asp:TextBox></td>
                    
                </tr>
                 <tr>
                    <td>Ean 2</td>
                    <td><asp:TextBox ID="ean2_txt" runat="server"></asp:TextBox></td>
                    
                </tr>

                 <tr>
                    <td>Ean 3</td>
                    <td><asp:TextBox ID="ean3_txt" runat="server"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td>Ean 4</td>
                    <td><asp:TextBox ID="ean4_txt" runat="server"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="saveDetail_btn" runat="server" Text="Uloz tovar" /></td>
                </tr>



            </table>

        </asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
