<%@ Page Language="C#" AutoEventWireup="true" CodeFile="operacky.aspx.cs" Inherits="operaracky_operacky" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Rodn0 cislo:
        <asp:TextBox ID="TextBox1" runat="server" Width="123px"></asp:TextBox>
        <br />
        Meno:
        <asp:TextBox ID="TextBox2" runat="server" Width="133px"></asp:TextBox>
        <br />
        Priezvisko&nbsp; <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <br />
        Poistovna:
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        <br />
        Diagnoza MKCH kod:
        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
        <br />
        Diagnoza slovom:
        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        <br />
        Operacny vykon stat:
        <br />
        Operacny vykon slovom&nbsp;
        <asp:ImageButton ID="ImageButton1" runat="server" />
        <br />
        Kodove oznacenie:<br />
        Trauma:<br />
        Planovany<br />
        Prenos<br />
        Sluzba<br />
        Sala<br />
        Operacny tym<br />
        Instrumantarka<br />
        <br />




    
    </div>
    </form>
</body>
</html>
