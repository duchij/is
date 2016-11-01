<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lf.aspx.cs" Inherits="lf" Culture="sk-SK" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .imgS {
            width:100%;
            height:auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="javascript:window.history.back();" target="_self" style="font-family:Arial, Helvetica, sans-serif;font-size:x-large">Naspäť</a>
       <asp:Image ID="lfImage" ImageUrl=""; runat="server" CssClass="imgS" />
    </div>
    </form>
</body>
</html>
