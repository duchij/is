<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sestrhlas.aspx.cs" Inherits="sestrhlas"  MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
<h2>Hlásenie sestier KDCH</h2>
                <hr />
                   Prihlasený: <strong><asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                   Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />
                <div class="row">
                    <div class="one sixth">
                    <asp:DropDownList ID="deps_dl" runat="server" 
                        AutoPostBack="True" OnSelectedIndexChanged="Calendar1_SelectionChanged" >
                    </asp:DropDownList>
                    </div>
                    <div class="one sixth">
                    <asp:DropDownList ID="predZad_cb" runat="server"  AutoPostBack="True" 
                            OnSelectedIndexChanged="Calendar1_SelectionChanged">
                        <asp:ListItem Value="pred">Predné hlásenie</asp:ListItem>
                        <asp:ListItem Value="zad">Zadné hlásenie</asp:ListItem>                       
                    </asp:DropDownList>
                    </div>
                    
                    <div class="one sixth">
                    <asp:DropDownList ID="time_cb" runat="server"  AutoPostBack="True" 
                            OnSelectedIndexChanged="Calendar1_SelectionChanged">
                        <asp:ListItem Value="n">Nočné hlásenie</asp:ListItem>
                        <asp:ListItem Value="d">Denné hlásenie</asp:ListItem>                       
                    </asp:DropDownList>
                    </div>
                    
                    <div class="three sixth">
                    <asp:Calendar CssClass="responsive" data-max="15" ID="Calendar1" runat="server" BackColor="White" OnSelectionChanged="Calendar1_SelectionChanged" 
                            BorderColor="#999999" DayNameFormat="Shortest" 
                            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"  
                           CellPadding="4">
                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#CCCCCC" />
                        <WeekendDayStyle BackColor="#FFFFCC" />
                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                        <TitleStyle BackColor="#999999" Font-Bold="True" BorderColor="Black" />
                        </asp:Calendar>
                    </div>
                </div>
                    
                   <asp:TextBox ID="hlasenie" CssClass="dtextbox" runat="server" Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox> 
                    
                <%--<FTB:FreeTextBox ID="hlasenie"  Height="500" Width="100%" toolbarlayout="Bold, Italic, Underline,RemoveFormat, Redo, Undo|FontFacesMenu,FontSizesMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell"
runat="Server"></FTB:FreeTextBox>--%>
   
                 <asp:Label ID="hlasko_lbl" runat="server" Visible="False" Font-Size="Small" >Hlasenie:</asp:Label>
                <asp:Label ID="view_hlasko" runat="server" Visible="False" Font-Size="Small"></asp:Label>
               <%-- <asp:TextBox ID="dodatok" runat="server" Width="90%" Rows="10" Height="100" TextMode="MultiLine" Visible="False">dodatok</asp:TextBox><br />--%>
                    <asp:Button ID="send" runat="server" Text="Ulož zmenu"  OnClick="save_fnc" />
                    
                    <asp:Button ID="copyYesterday_btn" runat="server" Text="<%$Resources:Resource,odd_vloz_clip %>" onclick="copyYesterday_btn_Click" />
                <asp:Button ID="print_btn" runat="server"  Text="Vytlač" onClick = "print_fnc" />
                <asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                     BackColor="#990000" ForeColor="Yellow" OnClick="def_loc_fnc" />
                <%--<asp:Button ID="addInfo_btn" runat="server" Text="Ulož dodatok" Enabled="False" onclick="addInfo_btn_Click"/>--%>
</asp:Content>

