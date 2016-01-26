<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sltoword.aspx.cs" Inherits="sltoword" Culture ="sk-SK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1250">
    <title>IS - to Word</title>
    <%--<link href="http://is.kdch.sk/css/style.css" rel="stylesheet" type="text/css" />--%>
<link href="http://is.kdch.sk/css/print.css" rel="stylesheet" type="text/css" media="print"/>
    <style type="text/css">
        @page 
        {
            size: 21cm 29.7cm;
            margin: 15mm 15mm 15mm 15mm; /* change the margins as you want them to be. */
        }
        @media print
        {    
            .no-print, .no-print *
            {
                display: none !important;
            }
        }

        body {
            font-family:Arial, Helvetica, sans-serif;
            font-size:10px;
        }
        .nonprint 
        {
	        display:none;
            /*margin:0px !important;*/
        }

        .menoLbl {
            font-size: 11px;
        }
        .commentLbl {
            font-size: 9px;
        }
        .small {
            font-size:smaller;
        }

        td {
            font-size:9px;
            height:auto;
            text-align:center;
        }
        th { 
            font-size:10px;

        }
        h1 {
            font-size:12px;
            line-height:normal;
        }


    </style>
</head>
<body>
    <form id="form1" runat="server">
  
    <h1><asp:Label ID="shiftPrintTitel_lbl" runat="server" Text=""></asp:Label>, <asp:Label ID="mesiac_lbl" runat="server" Text="Label"></asp:Label>, <asp:Label
        ID="rok_lbl" runat="server" Text="Label"></asp:Label>   </h1><hr />
        <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
       <%-- <table border="0" cellpadding="0" cellspacing="0" style="font-family:Arial CE;">
                <tr>
                    <td width="80" align="center"><b><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, is_den %>"></asp:Label></b></td>
                    <td width="160" align="center"><b>OUP</b></td>
                    <td width="130" align="center"><b>A</b></td>
                    <td width="130" align="center"><b>B</b></td>
                    <td width="130" align="center"><b><asp:Label ID="label2" runat="server" Text="<%$ Resources:Resource, slu_pohotovost %>"></asp:Label></b></td>
                    <td width="130" align="center"><b>Prijmova AMB</b></td>
                
                </tr>
                </table>--%>
         <div class="no-print">
    <hr />
        <a href="javascript:window.print();" style="font-weight:bolder;font-size:x-large;" />
         <asp:Label ID="print_lbl" runat="server" Text="<%$ Resources:Resource, print %>"></asp:Label> </a> &nbsp;
        <a href="javascript:history.back();" style="font-weight:bolder;font-size:x-large;" />
             <asp:literal id="test" runat="server" Text="<%$ Resources:Resource, back %>"></asp:literal></a>

         </div> 

        <asp:Table ID="shiftTable" runat="server" BorderColor="Black" BorderWidth="1" cellpadding="0" cellspacing="0" Font-Size="Large" Width="100%"  >
        </asp:Table>   
        <br />
        <asp:Literal ID="shift_sign" runat="server" Text=""></asp:Literal>
    
   
    </form>
</body>
</html>
