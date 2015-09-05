<%@ Page Language="C#" AutoEventWireup="true" CodeFile="vzp.aspx.cs" Inherits="sklad_vzp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	 <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1">

         <script src="../gdw/js/libs/modernizr-2.6.2.min.js"></script>
	<%--   <script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>--%>
	 
	    <!-- jQuery-->
	  
	    <!-- framework css --><!--[if gt IE 9]><!-->
	    <link type="text/css" rel="stylesheet" href="../gdw/css/groundwork.css" /><!--<![endif]--><!--[if lte IE 9]>
	    <link type="text/css" rel="stylesheet" href="css/groundwork-core.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-type.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ui.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-anim.css">
	    <link type="text/css" rel="stylesheet" href="css/groundwork-ie.css"><![endif]-->
    <title>Databaza VZP</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>Praca s VZP </h1>
        <asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>
    </div>
        <div class="tabs">
               <ul role="tablist">
                   <li role="tab" aria-controls="#tab1">Práca s PDF</li>
                   <li role="tab" aria-controls="#tab2">Hľadanie VZP</li>
                    
               </ul>
            <div id="tab1" role="tabpanel">
                <div class="row">

                    <div class="one third">
                        <h3 class="blue">Dátum VZP</h3>
                        <p>Vybraný je aktuálny deň, ak si želáte uložiť faktúru s iným dátom vyberte si z kalendára....</p>
                        <asp:Calendar ID="Calendar1" runat="server" Width="400px" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="10pt" ForeColor="Black" Height="180px">
                            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                            <NextPrevStyle VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#808080" />
                            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                            <SelectorStyle BackColor="#CCCCCC" />
                            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <WeekendDayStyle BackColor="#FFFFCC" />
                        </asp:Calendar>
                        <hr />
                        
                    
                    </div>
                    <div class="one third">
                        <h3 class="green">Kód VZP:</h3>
                        <p class="blue">Najprv si vyberte dátum, štandardne je aktuálny deň. Potom napíšte číslo VZP a vyberte si firmu, ku ktorej chcete priložiť zmluvu.
                            A ako posledné si vyberte pdf súbor/zmluvu faktúry. Potom sa Vám vygeneruje nová faktúra aj s vloženým VZP a táto sa Vám uloží...
                            A stiahne sa Vám upravená zmluva....
                             </p>
                        
                        <asp:TextBox ID="vzp_txt" runat="server" Width="400px" Size="100px"></asp:TextBox>
                         Firma:<asp:DropDownList ID="firma_dl" runat="server" Width="400px" OnSelectedIndexChanged="firma_dl_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                         <h2>Nahratie a spracovanie súboru</h2>
                        <asp:FileUpload ID="upload_fv" runat="server" CssClas="box asphalt"  Width="400px" Size="100px" EnableViewState="true" />
                        <asp:Button ID="save_vzp_btn" runat="server" CssClass="button green" Text="Spracuj....." OnClick="processPdfFileFnc" />
                    </div>

                    <div class="one third">
                        <h1>Pridaj firmu</h1>
                        <p>Tu si môžete upraviť alebo vytvoriť existujúcu alebo novú firmu..... Pri výbere firmy v strednom stlpci sa Vám umožní zmena údajov firmy</p>
                        Názov firmy:<asp:TextBox ID="firm_name_txt" runat="server"></asp:TextBox>
                        Adresa firmy:<asp:TextBox ID="firm_address_txt" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>
                        <asp:Button ID="save_firm_btn" runat="server" Text="Uloz" CssClass="button asphalt" OnClick="save_firm_btn_Click" />
                        <asp:Button ID="delete_firm_btn" runat="server" Text="Zmaz" CssClass="button red" OnClick="delete_firm_btn_Click" />
                        <p class="red">Pozor zmazaním firmy sa zmažú aj jej vsetky zmluvy !!!!!</p>
                    </div>
                    
                    </div>

            </div>
            <div id="tab2" role="tabpanel">
                <div class="row">
                     <div class="one third">
                         <div class="box blue">
                    Vyber rok:<asp:DropDownList ID="year_dl" runat="server">
                                <asp:ListItem Value="2010">Rok 2014</asp:ListItem>
                                <asp:ListItem Value="2015">Rok 2015</asp:ListItem>
                                <asp:ListItem Value="2016">Rok 2016</asp:ListItem>
                                <asp:ListItem Value="2017">Rok 2017</asp:ListItem>
					            <asp:ListItem Value="2018">Rok 2018</asp:ListItem>
					            <asp:ListItem Value="2019">Rok 2019</asp:ListItem>
					            <asp:ListItem Value="2020">Rok 2020</asp:ListItem>

                              </asp:DropDownList>
                         </div>
                    </div>
                    <div class="one third">
                        <div class="box yellow">
                        <strong>Hladaj dla firmy:</strong><asp:DropDownList ID="firmsSearch_dl" runat="server"></asp:DropDownList><asp:Button ID="byFirm_btn" runat="server" Text="Hladaj..." OnClick="searchBy" />
                        </div>
                        </div>
                    <div class="one third">
                        <div class="box pink">
                            Hladaj dla kodu VZP:<asp:TextBox ID="vzp_search_txt" runat="server"></asp:TextBox><asp:Button ID="byVzp_btn" runat="server" Text="Hladaj.." OnClick="searchBy" />
                        </div>
                    </div>
                   
                </div>
            </div>

           
            
        </div>
<asp:GridView ID="vzp_gv" runat="server" EnableModelValidation="True" 
        AutoGenerateColumns="False" 
        OnSelectedIndexChanged="vzp_gv_SelectedIndexChanged" OnRowDeleting="vzp_gv_RowDeleting" AllowPaging="True" PageSize="30" OnPageIndexChanged="vzp_gv_PageIndexChanged" >
                    <Columns>
                        <asp:BoundField DataField="lf_id" HeaderText="id" Visible="true" />
                        <asp:BoundField DataField="vzp" HeaderText="vzp" />
                        <asp:BoundField DataField="item_date" HeaderText="Datum" />
                        <asp:BoundField DataField="user_name" HeaderText="Vlozil" />
                        <asp:BoundField DataField="firm_name" HeaderText="Firma" />
                        <asp:CommandField ButtonType="Button" SelectText="Zobraz" ShowSelectButton="True" ControlStyle-CssClass="button green" >
<ControlStyle CssClass="button green"></ControlStyle>
                        </asp:CommandField>
                        <asp:CommandField ButtonType="Button" DeleteText="Zmaz" ShowDeleteButton="True" ControlStyle-CssClass="button red" />
                    </Columns>
                </asp:GridView>

    </form>

     <script type="text/javascript" src="../gdw/js/libs/jquery-1.10.2.min.js"></script>
	<script type="text/javascript" src="../gdw/js/groundwork.all.js"></script>
    <script type="text/javascript" src="../js/isesko.js"></script> 
</body>
</html>
