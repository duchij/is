<%@ Page Language="C#" AutoEventWireup="true" CodeFile="callsp.aspx.cs" Inherits="helpers_callsp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
    <div>
		<asp:Label ID="Label1" runat="server" Text="Stored Procedure" ToolTip="bez parametrou"></asp:Label><asp:textbox ID="stored_txt" runat="server"></asp:textbox>
        <asp:Button ID="run_btn" runat="server" Text="Call" OnClick="call_fnc" />

        <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
