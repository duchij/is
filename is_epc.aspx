﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="is_epc.aspx.cs" Inherits="is_epc" Culture="sk-Sk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-family:Arial; font-size:12px;margin:10px">
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="Image1" runat="server" ImageUrl="img/dfnsp.jpg" Width="80" Height="80" ImageAlign="Left" />
        Detská fakultná nemocnica s poliklinikou Bratislava, Limbová 1, 833 40  Bratislava
        <hr />
        <h1> EVIDENCIA PRACOVNÉHO ČASU V ÚPS</h1>
        <hr />
        <table style="width:100%">
            <tr>
                <td><asp:Label ID="username_lbl" runat="server" Text="Label"></asp:Label></td>
                <td>KDCH</td>
                <td><asp:Label ID="zaradenie_lbl" runat="server" Text="Label"></asp:Label></td>
                <td><asp:Label ID="osobcislo_lbl" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>

        


        <hr />
        <asp:Table ID="epc_tbl" runat="server" BorderColor="Black"></asp:Table>
    </div>
    </form>
</body>
</html>