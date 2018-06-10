<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_news.aspx.cs" Inherits="is_news" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
    <h2><asp:Label ID="title_lbl" runat="server" Text="<%$ Resources:Resource, is_news_title %>"></asp:Label></h2>
    <div class="row">
        <div class="one half">
                    
            
                <%--Prihlasený: <strong>
                    <asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <%-- Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />--%>
            <h3 class="blue">Cieľová skupina:</h3>
    <asp:DropDownList ID="targetGroup_dl" runat="server">
        <asp:ListItem Value="doctors" Text="Pre doktorov"></asp:ListItem>
        <asp:ListItem Value="nurses" Text="Pre sestry"></asp:ListItem>
        <asp:ListItem Value="all" Text="Pre vsetkych"></asp:ListItem>
    </asp:DropDownList>
            </div>
        <div class="one half">
            <h3 class="blue">Zoznam aktualít:</h3>
                <asp:GridView ID="news_gv" runat="server" AutoGenerateColumns="False"  CssClass="responsive" data-max="15"
                    AllowPaging="True" 
                    onrowdeleting="news_gv_RowDeleting" PageSize="5" Width="99%" OnSelectedIndexChanging="news_gv_selectRow" EnableModelValidation="True" OnPageIndexChanging="news_gv_PageIndexChanging" >
                    <Columns>
                        <asp:CommandField SelectText="Upraviť" ShowSelectButton="True">
                        <ControlStyle Font-Bold="True" ForeColor="#006600" />
                        </asp:CommandField>
                        <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id">
                            <ControlStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="datum_txt" HeaderText="Dátum" 
                            SortExpression="datum_txt">
                            <HeaderStyle Width="100px" Font-Bold="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="kratka_sprava" HeaderText="Správa" 
                            SortExpression="kratka_sprava" >
                        <HeaderStyle Font-Bold="True" />
                        </asp:BoundField>
                        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-CssClass="button yellow" DeleteText="Zmazať" SelectText="Editovať">
                            <ControlStyle Width="100px" Font-Bold="True" ForeColor="#CC0000" />
                        </asp:CommandField>
                    </Columns>
                    <PagerStyle Font-Bold="True" Font-Size="Large" ForeColor="Black"  />
                </asp:GridView>
 </div>
        </div>
    <div class="row">            
                <div class="box blue">
                <h2><asp:Label ID="label1" runat="server" Text="<%$ Resources:Resource, is_news_short %>"></asp:Label></h2>
                <asp:TextBox ID="small_text" runat="server" Width="99%" Height="50px" TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                    </div>
        <div class="box green">
                <h2><asp:Label ID="label2" runat="server" Text="<%$ Resources:Resource, is_news_full %>"></asp:Label></h2><br />
                 <%-- <FTB:FreeTextBox ID="full_text" runat="server" Width="100%" Height="400"  toolbarlayout="Bold, Italic, Underline,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull| Redo,Undo ,BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell|InsertImage,InsertImageFromGallery"></FTB:FreeTextBox>--%>
                <asp:TextBox ID="full_text" runat="server" Width="100%" Height="400" CssClass="dtextbox"></asp:TextBox>
            </div>
    </div>   
                 
                <asp:Button ID="save_btn" runat="server" Text="<%$ Resources:Resource, is_news_button_save %>" CssClass="button asphalt" OnClick="saveMessage_Click" />
</asp:Content>

