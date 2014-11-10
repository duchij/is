<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox"  %>!-->
<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hlasko.aspx.cs" Inherits="hlasko" ValidateRequest="False" Culture="sk-Sk"  %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="hlavicka" runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />

    <title>IS- Hlasenie sluzieb</title>
    <script type="text/javascript" src="tinymce/js/tinymce/tinymce.min.js"></script>
   <%-- <script type="text/javascript" src="js/jquery.js"></script>--%>
    

<script type="text/javascript">

//$(document).ready(function(){

//setInterval(function (){
//            
//            $.ajax( {
//                        url:'hlasko.aspx',
//                        type:'POST',
//                        data:{'saveHlasko':'1'},
//                        
//                        success:function(result){
//                            alert(result);
//                            },
//                            
//                        error:function(xhr,desc,err){
//                        
//                             console.log(xhr);
//		                    console.log("Details: " + desc + "\nError:" + err);
//		                    }
//			    } );  
//            },300);

//});



tinymce.init({
    selector: ".dtextbox",
    //theme:"advanced",
    toolbar:"undo redo | alignleft aligncenter alignright | bold italic | bullist numlist | fontsizeselect | fontselect | forecolor",
    
    fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
    font_formats: "Arial=arial,helvetica,sans-serif;Courier New=courier new,courier,monospace;Verdana=verdana",
    menubar:true,
    plugins : 'paste textcolor code',
    paste_word_valid_elements: "b,strong,i,em,h1,h2",
    paste_as_text: true,
    force_p_newlines: false,
   // forced_root_block : 'div',
    force_br_newlines : true,

    autosave_retention: "30m"

 });


function myAlert()
{
    window.alert("Pozor!!! Zmena dátumu dňa až po 9.00 hod. \n\r \n\rTo znamená, že službe zostáva jej dátum aktuálne vybraný až do 9.00 hod nasledujúceho dňa !!!!");
} 

/*tinyMCE.init({
        language :"sk",
        theme : "advanced",
        plugins : "paste",
        theme_advanced_buttons3_add : "pastetext,pasteword,selectall,pastetext",
        mode : "textareas",
        force_br_newlines : true,
        force_p_newlines : false,
        
        paste_use_dialog : false,
        paste_auto_cleanup_on_paste : true,
        theme_advanced_toolbar_location : "top",
        
       paste_preprocess : function(pl, o) {
            // Content string containing the HTML from the clipboard
           // alert(o.content);
            o.content = "-: CLEANED :-\n" + o.content;
        },
       paste_postprocess : function(pl, o) {
            // Content DOM node containing the DOM structure of the clipboard
           // alert(o.node.innerHTML);
            o.node.innerHTML = o.node.innerHTML + "\n-: CLEANED :-";
        }
        
});*/
</script>

</head>

<body onload="">
<%--<asp:Label ID="msg_lbl1" runat="server" Text="Label"></asp:Label>--%>
    
    <form id="form1" runat="server">
    <asp:Label ID="msg_lbl" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="wrapper">   
    <div id="header">
        <h1>Informačný systém Kliniky detskej chirurgie LF UK a DFNsP</h1> 
        <duch:my_header runat="server"></duch:my_header>
    </div>
        
        <div id="content">
         <div id="menu2">
           <info:info_bar ID="info_bar" runat="server" />
            <info:info_bar />
        </div>
            <div id="cont_text">
            
                <h2>Hlásenie službieb KDCH, DOrK a KPU</h2>
                <hr />
                   Prihlasený: <strong><asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                   Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />
                <asp:Label ID="news_lbl" runat="server" Text=""></asp:Label>
                <table>
                <tr>
                <td valign="top">
                    <asp:DropDownList ID="hlas_type" runat="server" Height="21px" Width="187px" 
                        AutoPostBack="True" onselectedindexchanged="Calendar1_SelectionChanged" 
                        ontextchanged="hlas_type_SelectedIndexChanged">
                        <asp:ListItem Value="A">Oddelenie A</asp:ListItem>
                        <asp:ListItem Value="B">Oddelenie B</asp:ListItem>
                        <asp:ListItem Value="OP">Op.pohotovosť</asp:ListItem>
                    </asp:DropDownList></td>
                    <td valign="top"><asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" 
                            BorderColor="#FFCC66" DayNameFormat="Shortest" 
                            Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399" Height="200px" 
                            onselectionchanged="Calendar1_SelectionChanged" Width="220px" 
                            BorderWidth="1px" ShowGridLines="True">
                        <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                        <SelectorStyle BackColor="#FFCC66" />
                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#CC9966" />
                        <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                        <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                        <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                            ForeColor="#FFFFCC" />
                        </asp:Calendar></td>
                    </tr>
                    </table>
                    <br />
                    <br />
                    <hr />
                    <table width="95%" border="0">
                    <tr>
                    <td style="width:60%;">
                    <p>Sem <strong style="color:Maroon;">copy/paste priezvisko</strong> z hlásenia následne po jeho vložení za ním stlačiť klávesu enter, tak aby každé priezvisko začínalo na novom riadku, pre uloženie/vygenerovanie stlačiť <strong> uložiť</strong>, nie je nutné dodržiavať veľké malé písmená. Automaticky sa vygeneruje odkaz na OSIRIX</p>
                    <hr />
                    </td>
                    <td><p><strong style="color:Maroon;">Klikni na link, pre otvorenie v OSIRIXe</strong><br /><br />Tento link funguje len vrámci DFNsP!!!!!</p><hr /></td>
                    </tr>
                    <tr>
                    <td>
                        <asp:TextBox ID="osirix_txt" runat="server" Width="60%" Height="150px" TextMode="MultiLine" ></asp:TextBox>
                        <asp:Button ID="osirix_btn"
                            runat="server" Text="Ulož a Generuj" onclick="osirix_btn_Click" BackColor="#990000" ForeColor="Yellow"  /></td>
                        <td valign="top">
                        
                            <div style="text-align:center;"><asp:Label ID="osirix_url" runat="server" Text="Label"></asp:Label></div></td>
                        </tr>
                    </table>
                    <h1>Hlásenie služby:</h1>
                    <hr />
                    <asp:TextBox ID="hlasenie" CssClass="dtextbox" runat="server"  Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox>
                    <%--<FTB:FreeTextBox ID="hlasenie" runat="server" Width="100%" Height="500"  toolbarlayout="Bold, Italic, Underline,RemoveFormat|FontFacesMenu,FontSizesMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull; Redo,Undo ,BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell"></FTB:FreeTextBox>--%>
                    <hr />
                 <asp:Label ID="hlasko_lbl" runat="server" Visible="False" Font-Size="Small" >Hlasenie:</asp:Label><br />
                <asp:Label ID="view_hlasko" runat="server" Visible="False" Font-Size="Small"></asp:Label>
                <asp:TextBox ID="dodatok"  CssClass="dtextbox" runat="server" Width="90%" Rows="10" Height="100" TextMode="MultiLine" Visible="False">dodatok</asp:TextBox><br />
                    <asp:Button ID="send" runat="server" Text="Ulož zmenu" onclick="send_Click" />
                
               
                <asp:Button ID="print_btn" runat="server" onclick="Button1_Click" Text="Vytlač" />
                
                <asp:Button ID="toWord_btn" runat="server" onclick="toWord_Click" Text="Tlač/Word" />
                
                <asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                    onclick="def_lock_btn_Click" BackColor="#990000" ForeColor="Yellow" />
                
                 <asp:Button ID="def_locl_w_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko/Word" 
                    onclick="def_lock_btn_w_Click" BackColor="#990000" ForeColor="Yellow" />    
                    
                <asp:Button ID="addInfo_btn" runat="server" Text="Ulož dodatok" Enabled="False" 
                    onclick="addInfo_btn_Click" />
                    
                   
            </div>
         </div>
            <div id="menu">
            
                <menu:left_menu ID="Left_menu1" runat="server" /><menu:left_menu />
            
            </div>
            
            
            
        
       
        <div id="footer">Design by Boris Duchaj</div>
    
    
    </div>
 
    
    
 
    
    </form>
</body>
</html>
