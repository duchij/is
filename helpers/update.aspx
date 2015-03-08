<%@ Page Language="C#" AutoEventWireup="true" CodeFile="update.aspx.cs" Inherits="helpers_update" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body { font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;}
        .update_proc {
            font-family:'Courier New',Courier New, Courier, monospace;
            font-size:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <h1>IESKO UPDATE</h1>
        <hr />

        Update passwd:<asp:TextBox ID="passwd_txt" TextMode="Password" runat="server"></asp:TextBox>
        <asp:Button ID="update_btn" runat="server"  OnClick="doUpdateFnc" Text="update...." />
        <br /><br />
        <asp:Label ID="info_lbl" runat="server" Text="" CssClass="update_proc"></asp:Label>
        
    </div>
    </form>
</body>
</html>
