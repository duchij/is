<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sklad1.aspx.cs" Inherits="sklad_hladanie" EnableViewState="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="../css/sklad.css" />
    <title>Sklad hladanie</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
        <div id="left_panel">
        <h1>Hladanie v sklade</h1>
        <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
        <hr />
        <asp:DropDownList ID="searchIn_dl" runat="server">
                <asp:ListItem Value="nazov">V nazve</asp:ListItem>
                <asp:ListItem Value="sukl">Dla sukl kodu</asp:ListItem>
                <asp:ListItem Value="ean">Podla eanu</asp:ListItem>

        </asp:DropDownList>
        <asp:TextBox ID="phrase_txt" runat="server" Width="200px" AutoPostBack="true" OnTextChanged="parseSearchPhrase_fnc"></asp:TextBox>
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
                    <td><asp:TextBox ID="ean1_txt" runat="server" Width="200px" OnTextChanged="ean_txt_TextChanged" AutoPostBack="true"></asp:TextBox><asp:Label ID="ean1_info" runat="server" Text="" ForeColor="red"></asp:Label></td>
                    
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
                    <td><asp:TextBox ID="eanGen_txt" runat="server" ReadOnly="true"></asp:TextBox><asp:Label ID="ean13_msg" runat="server" Text="" ForeColor="red"></asp:Label></td>
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

        <div id="right_panel">

            <h1>Zoznam nahodenych tovarov</h1>
            <asp:GridView ID="goodslist_gv" runat="server"  BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" AllowPaging="True"  AutoGenerateColumns="False" OnPageIndexChanging="goodslist_gv_PageIndexChanging" OnSelectedIndexChanging="goodslist_gv_SelectedIndexChanging" >
                <AlternatingRowStyle BackColor="White" />
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <Columns>

                    <asp:CommandField EditText="Edituj" ShowSelectButton="True" SelectText="Vyber" />
                    <asp:BoundField DataField="tovar_id" HeaderText="tovarId" />
                    <asp:BoundField DataField="sukl_kod" HeaderText="SUKL kod" />
                    <asp:BoundField DataField="nazov" HeaderText="Nazov" />
                    <asp:BoundField DataField="eanv" HeaderText="EAN" />

                </Columns>

            </asp:GridView>
        </div>

    </div>
    </form>
    <script src="../js/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src="../js/sklad.js" type="text/javascript"></script>
</body>
</html>
