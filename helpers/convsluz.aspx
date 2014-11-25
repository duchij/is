<%@ Page Language="C#" AutoEventWireup="true" CodeFile="convsluz.aspx.cs" Inherits="helpers_convsluz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>Mesiac:<asp:TextBox ID="mesiac_txt" runat="server"></asp:TextBox></p>
        <p>Rok:<asp:TextBox ID="rok_txt" runat="server"></asp:TextBox></p>
        <asp:Button ID="run" runat="server" Text="Convert" OnClick="runConversion_fnc" />
    </div>
    </form>
</body>
</html>
