<%@ Page Language="C#" AutoEventWireup="true" CodeFile="toexcel.aspx.cs" Inherits="toexcel" Culture="sk-SK"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal runat="server" ID="msg_lbl" Text=""></asp:Literal>
    <asp:Table ID="result_tbl" runat="server"></asp:Table>
    </div>
    </form>
</body>
</html>
