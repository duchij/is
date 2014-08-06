<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="is_news.aspx.cs" Inherits="is_news" ValidateRequest="False" Culture="sk-SK" %>
<%--<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>--%>


<%@ Register TagPrefix="duch" TagName="my_header" Src="header.ascx" %>
<%@ Register TagPrefix="menu" TagName="left_menu" Src="left_menu.ascx" %>
<%@ Register TagPrefix="info" TagName="info_bar" Src="news.ascx" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IS- Interné aktuality KDCH</title>
     <link href="css/style.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="tinymce/js/tinymce/tinymce.min.js"></script>
      
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

          });
      
      </script>
</head>
<body>
    <form id="form1" runat="server">
    
     <div id="wrapper">
        <div id="header">
            <h1>
                Informačný systém Kliniky detskej chirurgie LF UK a DFNsP</h1>
            <duch:my_header runat="server"></duch:my_header>
        </div>
        <div id="content">
        <div id="menu2">
           <info:info_bar ID="info_bar" runat="server" />
            <info:info_bar />
        </div>
        
            <div id="cont_text">
                <h2>
                    <asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, is_news_title %>"></asp:Label></h2>
                <hr />
                <%--Prihlasený: <strong>
                    <asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <%-- Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />--%>
                <asp:GridView ID="news_gv" runat="server" AutoGenerateColumns="False" 
                    AllowPaging="True" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
                    BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" 
                    onrowdeleting="news_gv_RowDeleting" PageSize="5" Width="99%" SelectedIndex="-1" AutoGenerateSelectButton="true" OnSelectedIndexChanging="news_gv_selectRow" >
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id">
                            <ControlStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="datum_txt" HeaderText="Datum" 
                            SortExpression="datum_txt">
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="kratka_sprava" HeaderText="Sprava" 
                            SortExpression="kratka_sprava" />
                        <asp:CommandField ShowDeleteButton="True">
                            <ControlStyle Width="100px" />
                        </asp:CommandField>
                    </Columns>
                    <FooterStyle BackColor="Tan" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                        HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                </asp:GridView>
                
                <hr />
                <h2><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, is_news_short %>"></asp:Label></h2><br />
                <asp:TextBox ID="small_text" runat="server" Width="99%" Height="50px" TextMode="MultiLine" MaxLength="250"></asp:TextBox><br />
                <h2><asp:Label ID="label2" runat="server" Text="<%$ Resources:Resource, is_news_full %>"></asp:Label></h2><br />
                 <%-- <FTB:FreeTextBox ID="full_text" runat="server" Width="100%" Height="400"  toolbarlayout="Bold, Italic, Underline,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull| Redo,Undo ,BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell|InsertImage,InsertImageFromGallery"></FTB:FreeTextBox>--%>
                <asp:TextBox ID="full_text" runat="server" Width="100%" Height="400" CssClass="dtextbox"></asp:TextBox>
                 
                 
                <asp:Button ID="save_btn" runat="server" Text="<%$ Resources:Resource, is_news_button_save %>" OnClick="saveMessage_Click" />
                
            </div>
        </div>
        
        <div id="menu">
            <menu:left_menu ID="Left_menu1" runat="server" />
            <menu:left_menu />
        </div>
        
        
        <div id="footer">
            Design by Boris Duchaj</div>
    </div>
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
    
    
    
    </form>
</body>
</html>
