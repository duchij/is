<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sestrhlas.aspx.cs" Inherits="sestrhlas" ValidateRequest="False" Culture="sk-Sk" %>
<%@ Register TagPrefix="duch" TagName="my_header" src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="css/style.css" rel="stylesheet" type="text/css" />
    <title>IS -KDCH Hlásenie sestier</title>
     <script type="text/javascript" src="tinymce/js/tinymce/tinymce.min.js"></script>
     <%--<script type="text/javascript" src="zeroclipboard/ZeroClipboard.js"></script> --%>
     
    <script type="text/javascript">
    tinymce.init({
    selector: ".dtextbox",
    //theme:"advanced",
    toolbar: "undo redo | alignleft aligncenter alignright | bold italic | bullist numlist | fontsizeselect | fontselect | forecolor",

    fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
    font_formats: "Arial=arial,helvetica,sans-serif;Courier New=courier new,courier,monospace;Verdana=verdana",
    menubar: true,
    plugins: 'paste textcolor code',
    paste_word_valid_elements: "b,strong,i,em,h1,h2",
    paste_as_text: true,
    force_p_newlines: false,
    // forced_root_block : 'div',
    force_br_newlines: true,

    autosave_retention: "30m"


 })
   
         
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
            //alert(o.content);
            o.content = "-: CLEANED :-\n" + o.content;
        },
       paste_postprocess : function(pl, o) {
            // Content DOM node containing the DOM structure of the clipboard
            //alert(o.node.innerHTML);
            o.node.innerHTML = o.node.innerHTML + "\n-: CLEANED :-";
        }
        
});*/
</script>
</head>
<body>
    <form id="form1" runat="server">
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
            
                <h2>Hlásenie sestier KDCH</h2>
                <hr />
                   Prihlasený: <strong><asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                   Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />
                <table width="99%">
                <tr>
                <td valign="top">
                    <asp:DropDownList ID="oddType_cb" runat="server" Height="18px" Width="113px" 
                        AutoPostBack="True" OnSelectedIndexChanged="Calendar1_SelectionChanged" >
                       
                    </asp:DropDownList></td>
                    <td valign="top"><asp:DropDownList ID="predZad_cb" runat="server" Height="18px" 
                            Width="146px" AutoPostBack="True" 
                            OnSelectedIndexChanged="Calendar1_SelectionChanged">
                        <asp:ListItem Value="pred">Predné hlásenie</asp:ListItem>
                        <asp:ListItem Value="zad">Zadné hlásenie</asp:ListItem>                       
                    </asp:DropDownList></td>
                    <td valign="top"><asp:DropDownList ID="time_cb" runat="server" Height="19px" 
                            Width="131px" AutoPostBack="True" 
                            OnSelectedIndexChanged="Calendar1_SelectionChanged">
                        <asp:ListItem Value="n">Nočné hlásenie</asp:ListItem>
                        <asp:ListItem Value="d">Denné hlásenie</asp:ListItem>                       
                    </asp:DropDownList></td>
                    <td valign="top"><asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" OnSelectionChanged="Calendar1_SelectionChanged" 
                            BorderColor="#FFCC66" DayNameFormat="Shortest" 
                            Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399" Height="200px" 
                           Width="220px" 
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
                   <asp:TextBox ID="hlasenie" CssClass="dtextbox" runat="server" Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox> 
                    
                <%--<FTB:FreeTextBox ID="hlasenie"  Height="500" Width="100%" toolbarlayout="Bold, Italic, Underline,RemoveFormat, Redo, Undo|FontFacesMenu,FontSizesMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell"
runat="Server"></FTB:FreeTextBox>--%>
   
                 <asp:Label ID="hlasko_lbl" runat="server" Visible="False" Font-Size="Small" >Hlasenie:</asp:Label>
                <asp:Label ID="view_hlasko" runat="server" Visible="False" Font-Size="Small"></asp:Label>
                <asp:TextBox ID="dodatok" runat="server" Width="90%" Rows="10" Height="100" TextMode="MultiLine" Visible="False">dodatok</asp:TextBox><br />
                    <asp:Button ID="send" runat="server" Text="Ulož zmenu"  OnClick="save_fnc" />
                    
                    <asp:Button ID="copyYesterday_btn" runat="server" Text="<%$Resources:Resource,odd_vloz_clip %>" onclick="copyYesterday_btn_Click" />
                <asp:Button ID="print_btn" runat="server"  Text="Vytlač" onClick = "print_fnc" />
                <asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                     BackColor="#990000" ForeColor="Yellow" OnClick="def_loc_fnc" />
                <asp:Button ID="addInfo_btn" runat="server" Text="Ulož dodatok" Enabled="False" onclick="addInfo_btn_Click"
                    />
            </div>
         </div>
            <div id="menu">
            
                <menu:left_menu ID="Left_menu1" runat="server" /><menu:left_menu />
            
            </div>
            
            
            
        
       
        <div id="footer">Design by Boris Duchaj</div>
    
    
    </div>
 
    
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
