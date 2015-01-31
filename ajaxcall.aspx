<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ajaxcall.aspx.cs"  Inherits="callExt.ajaxcall" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src ="js/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src ="js/isesko.js" type="text/javascript"></script>
    <%--<script src ="js/json2.js" type="text/javascript"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" id="testButton" value="test" onclick="test()" />
    </div>
    </form>
</body>
</html>
