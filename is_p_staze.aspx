<%@ Page Language="C#" AutoEventWireup="true" CodeFile="is_p_staze.aspx.cs" Inherits="is_p_staze" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plan stazi</title>
    <style>
        body {
            font-family:Arial;
            font-size:11px;
        }
        td{
            padding:2px;
            vertical-align:top;
        }
        .info {
            padding:2px;
            border:solid 0.5px black;
        }

            
            .no-print, .no-print *
            {
                display: none !important;
                
            }
        


    </style>
</head>
<body>
    <form id="form1" runat="server">
    <a href="javascript:history.back();" class="no-print" media="print" style="font-size:16px;text-decoration:none;" > <<< Naspäť</a>
    <h1>Plán stáži: <asp:label ID="title_lbl" runat="server" Text=""></asp:label></h1>
       
    <asp:Literal ID="info_lt" runat="server" Text=""></asp:Literal>
    <asp:Table ID="classes_tbl" runat="server"></asp:Table>
    
    </form>
</body>
</html>
