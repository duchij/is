<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="opprogram.aspx.cs" Inherits="opprogram" %>
<%--<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
 <h2>
                    <asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, is_opprogram_title %>"></asp:Label></h2>
                <hr />
                <%--Prihlasený: <strong>
                    <asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <%-- Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />--%>

                <asp:GridView ID="news_gv" runat="server" AutoGenerateColumns="false" 
                    AllowPaging="True" BackColor="LightGoldenrodYellow" BorderColor="Tan" 
                    BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" 
                    onrowdeleting="news_gv_RowDeleting" PageSize="5" Width="99%" SelectedIndex="-1" AutoGenerateSelectButton="true" OnSelectedIndexChanging="news_gv_selectRow" OnPageIndexChanging="news_gv_PageIndexChanging">
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
                <h2><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, is_opprogram_short %>"></asp:Label></h2><br />
                <asp:TextBox ID="small_text" runat="server" Width="99%" Height="50px" TextMode="MultiLine" MaxLength="250"></asp:TextBox><br />
                <h2><asp:Label ID="label2" runat="server" Text="<%$ Resources:Resource, is_news_full %>"></asp:Label></h2><br />
                
                  
    
                <%--<FTB:FreeTextBox ID="full_text" runat="server" Width="100%" Height="400"  toolbarlayout="Bold, Italic, Underline,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull| Redo,Undo ,BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell|InsertImage,InsertImageFromGallery"></FTB:FreeTextBox>--%>
                 <asp:TextBox ID="full_text" CssClass="dtextbox" runat="server"  Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox>            
    
    <asp:Button ID="save_btn" runat="server" Text="<%$ Resources:Resource, is_news_button_save %>" OnClick="saveMessage_Click" />
    <asp:Button ID="printButton_btn" runat="server" Text="<%$ Resources:Resource, print %>" />
</asp:Content>

