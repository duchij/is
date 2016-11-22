<%@ Page Language="C#" AutoEventWireup="true" CodeFile="is_plan.aspx.cs" Inherits="is_plan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link type="text/css" rel="stylesheet" href="css/plan.css">
        <link type="text/css" rel="stylesheet" href="css/jquery-ui.min.css" />
        <link type="text/css" rel="stylesheet" href="css/jquery-ui.structure.min.css" />
        <link type="text/css" rel="stylesheet" href="css/jquery-ui.theme.min.css" />
        <link type="text/css" rel="stylesheet" href="css/n_style.css" />


    <title>Plan pre sestry</title>
</head>
<body>
    <div id="msg_dialog"></div>
        <div id="commBall"><img src="img/ajax-loader.gif" ></div>
    <form id="form1" runat="server">
 
        <h1>Plánovanie pre sestry</h1>
        <a href="sluzby2_sestr.aspx" target="_self">Naspäť do pôvodného plánovania</a>
        <div>
            <div>Rok:
                <asp:DropDownList runat="server" ID="month_dl" AutoPostBack="true"></asp:DropDownList>

            </div>
            <div>Mesiac:
                <asp:DropDownList runat="server" ID="years_dl" AutoPostBack="true"></asp:DropDownList>

            </div>
            <div>Oddelenie:
                <asp:DropDownList runat="server" ID="deps_dl" AutoPostBack="true" OnSelectedIndexChanged="setPlanForDepartmentFnc"></asp:DropDownList>
            </div>
        </div>
        <div class="planTable">
            <hr />
            <asp:Table runat="server" ID="planTable_tbl" style="width:100%;border:1px solid"></asp:Table>
        </div>
    
    </form>
    <script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jquery-ui.min.js"></script>
  <script src="js/plan.js" type="text/javascript"></script>

</body>
</html>
