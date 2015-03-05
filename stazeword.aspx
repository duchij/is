<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stazeword.aspx.cs" Inherits="sltoword" Culture ="sk-SK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1250">
    <title>Print staze</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/print.css" rel="stylesheet" type="text/css" media="print"/>
</head>
<body style="font-family:Arial CE;">
<form id="form1" runat="server">
    <h1><asp:Label ID ="printStaze_titel" runat="server" Text="titel...."></asp:Label></h1>
    <hr />
    
    <div class="nonprint">
        <hr />
        <asp:Label ID="print_lbl" runat="server" Text="" Font-Size="X-Large"></asp:Label> /
        <asp:Label ID="back_lbl" runat="server" Text=""  Font-Size="X-Large"></asp:Label>
    </div>
    <asp:Table ID="stazeTable_tbl" runat="server" Width="100%"></asp:Table>
    <hr />
    <asp:Label ID="sign_lbl" runat="server"></asp:Label>
</form>
</body>
</html>
