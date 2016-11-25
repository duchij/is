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
    <asp:Literal runat="server" ID="msg_lbl" Text="0"></asp:Literal>
    <div id="msg_dialog"></div>
        <div id="commBall"><img src="img/ajax-loader.gif" ></div>
    <form id="form1" runat="server">
 
        <h1>Plánovanie pre sestry</h1>
        <div class="legendaPlan">
            <strong>RA</strong> - rannka, <strong>RA2</strong>-rannka 2, <strong>D1</strong> - prvá sestra dennka, <strong>D2</strong> - druhá sestra dennka, <strong>ZD</strong> - začínajúca sestra dennka<br />
            <strong>A1</strong> - assistent rannka, <strong>A2</strong> - asistent nočná<br />
            <strong>N1</strong> - prvá sestra nočná, <strong>N2</strong> - druhá sestra nočná, <strong>ZN</strong> - začínajúca sestra nočná<br />
            <strong>S1</strong> - prvý sanitár, <strong>S2</strong> - druhý sanitár<br />
            Podržaním myši nad dňom sa zobrazí aktuálny dátum pre tento deň
        </div>
        <hr />
        <a href="sluzby2_sestr.aspx" target="_self">Naspäť do pôvodného plánovania</a> | <asp:Button runat="server" ID="linkPlan_btn" OnClick="printPlan_fnc" Text="Vytlač pdf"></asp:Button> | <a href="dovolenky_sestr.aspx" target="_self">Plánovanie dovoleniek a iných aktivít</a>
        <p></p>
        <div class="menu" >
            <div class="selectStyle">Rok:
                <asp:DropDownList runat="server" ID="month_dl" AutoPostBack="true" OnSelectedIndexChanged="setPlanForDepartmentFnc"></asp:DropDownList>

            </div>
            <div class="selectStyle">Mesiac:
                <asp:DropDownList runat="server" ID="years_dl" AutoPostBack="true" OnSelectedIndexChanged="setPlanForDepartmentFnc"></asp:DropDownList>

            </div>
            <div class="selectStyle">Oddelenie:
                <asp:DropDownList runat="server" ID="deps_dl" AutoPostBack="true" OnSelectedIndexChanged="setPlanForDepartmentFnc"></asp:DropDownList>
            </div>
        </div>
        <div class="planTable">
            <hr />
            <asp:Table runat="server" ID="planTable_tbl" style="width:100%;" CellPadding="0" CellSpacing="0"></asp:Table>
        </div>
    
    </form>
    <script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jquery-ui.min.js"></script>
  <script src="js/plan.js" type="text/javascript"></script>

</body>
</html>
