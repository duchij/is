<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sklad.aspx.cs" Inherits="sklad_hladanie" %>

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
        <h3><asp:Label ID="resultTitle_lbl" runat="server" Text=""></asp:Label></h3>
        <asp:Table ID="result_tbl" runat="server"></asp:Table>

        <asp:PlaceHolder ID="tovarDetail_pl" runat="server" Visible="true" >
            <table>
                <tr>
                    <td>Nazov tovaru:</td>
                    <td>
                        <asp:Label ID="nazov_lbl" runat="server" Text=""></asp:Label> </td>

                    <asp:HiddenField ID="tovarId_hf" runat="server" Value="" />
                </tr>
                <tr>
                    <td>Sukl kod:</td>
                    <td>
                    <asp:Label ID="sukl_lbl" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td>Ean 1</td>
                    <td><asp:TextBox ID="ean1_txt" runat="server" Width="200px" OnTextChanged="ean_txt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                    
                </tr>
                
                 <tr>
                    <td>Ean 2</td>
                    <td><asp:TextBox ID="ean2_txt" runat="server" Width="200px" OnTextChanged="ean_txt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                    
                </tr>

                 <tr>
                    <td>Ean 3</td>
                    <td><asp:TextBox ID="ean3_txt" runat="server" Width="200px" OnTextChanged="ean_txt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td>Ean 4</td>
                    <td><asp:TextBox ID="ean4_txt" runat="server" Width="200px" OnTextChanged="ean_txt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td>EAN V</td>
                    <td><asp:TextBox ID="eanGen_txt" runat="server" ReadOnly="true"></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>EAN 1_128</td>
                    <td><asp:TextBox ID="ean128_txt" runat="server" ReadOnly="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Expiracia</td>
                    <td><asp:TextBox ID="expiry_txt" runat="server" ReadOnly="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>LOT</td>
                    <td><asp:TextBox ID="lot_txt" runat="server" ReadOnly="true"></asp:TextBox></td>
                </tr>


                <tr>
                    <td colspan="2"><asp:Button ID="saveDetail_btn" runat="server" Text="Uloz tovar" OnClick="saveEanTovarFnc" /></td>
                </tr>



            </table>

        </asp:PlaceHolder>
    </div>
    </form>
    <script src="../js/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src="../js/sklad.js" type="text/javascript"></script>
</body>
</html>
