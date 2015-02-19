<%@ Page Language="C#" AutoEventWireup="true" CodeFile="synco.aspx.cs" Inherits="helpers_synco" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Sync:<asp:TextBox ID="kvValue_txt" runat="server"></asp:TextBox><asp:Button ID="sync_btn" runat="server" Text="Sync" OnClick="doSyncFnc" />
    </div>
    </form>
</body>
</html>
