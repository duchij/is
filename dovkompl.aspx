<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dovkompl.aspx.cs" Inherits="dovkompl" %>

<!DOCTYPE html>
<%--<!--[if lt IE 7]><html class="no-js ie ie6 lt-ie7 lt-ie8 lt-ie9 lt-ie10"><![endif]-->
<!--[if IE 7]>   <html class="no-js ie ie7 lt-ie8 lt-ie9 lt-ie10"><![endif]-->
<!--[if IE 8]>   <html class="no-js ie ie8 lt-ie9 lt-ie10"><![endif]-->
<!--[if IE 9]>   <html class="no-js ie ie9 lt-ie10"><![endif]-->
<!--[if gt IE 9]><html class="no-js ie ie10"><![endif]-->
<!--[if !IE]><!-->--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	 <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1">

         <script src="gdw/js/libs/modernizr-2.6.2.min.js"></script>
	 <script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
	 
	    <!-- jQuery-->
	  
	    <!-- framework css --><!--[if gt IE 9]><!-->
	    <link type="text/css" rel="stylesheet" href="gdw/css/groundwork.css" /><!--<![endif]--><!--[if lte IE 9]>
	    <link type="text/css" rel="stylesheet" href="css/groundwork-core.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-type.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ui.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-anim.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ie.css"><![endif]-->
	   	<link type="text/css" rel="stylesheet" href="gdw/css/duch.css" />--%>

    <link rel="stylesheet" href="css/style.css" type="text/css" />
    <link href="css/print.css" rel="stylesheet" type="text/css" media="print"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="msg_lit" runat="server"></asp:Literal>
        <h1><asp:Label ID="dovkomplTitel_lbl" runat="server" Text=""></asp:Label></h1>
        <hr />
        <div class="nonprint">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
   <a href="javascript:window.print();" style="font-weight:bolder;font-size:x-large;" /><asp:Label ID="print_lbl" runat="server" Text="<%$ Resources:Resource, print %>"></asp:Label> </a> &nbsp;
    <a href="javascript:window.history.back();"><asp:label id="labelid" runat="server" Font-Size="X-Large" Font-Bold="true" Text="<%$ Resources:Resource, back %>"></asp:label></a></div>
        <hr />
    <div>
        <asp:Table ID="komplview_tbl" runat="server" BorderStyle="Solid" Width="100%" ></asp:Table>
    </div>  
    </form>
    <%--<script type="text/javascript" src="gdw/js/libs/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/isesko.js"></script>--%>
    <%-- <script type="text/javascript" src="gdw/js/libs/jquery-1.10.2.min.js"></script>
	<script type="text/javascript" src="gdw/js/groundwork.all.js"></script>--%>
</body>
</html>
