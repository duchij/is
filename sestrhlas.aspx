<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sestrhlas.aspx.cs" Inherits="sestrhlas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
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
               <%-- <asp:TextBox ID="dodatok" runat="server" Width="90%" Rows="10" Height="100" TextMode="MultiLine" Visible="False">dodatok</asp:TextBox><br />--%>
                    <asp:Button ID="send" runat="server" Text="Ulož zmenu"  OnClick="save_fnc" />
                    
                    <asp:Button ID="copyYesterday_btn" runat="server" Text="<%$Resources:Resource,odd_vloz_clip %>" onclick="copyYesterday_btn_Click" />
                <asp:Button ID="print_btn" runat="server"  Text="Vytlač" onClick = "print_fnc" />
                <asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                     BackColor="#990000" ForeColor="Yellow" OnClick="def_loc_fnc" />
                <%--<asp:Button ID="addInfo_btn" runat="server" Text="Ulož dodatok" Enabled="False" onclick="addInfo_btn_Click"/>--%>
</asp:Content>

