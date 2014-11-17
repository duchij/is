<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hlasko.aspx.cs" Inherits="hlasko" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>

 <h2>Hlásenie službieb KDCH, DOrK a KPU</h2>
                <hr />
                   Prihlasený: <strong><asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                   Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />
                <asp:Label ID="news_lbl" runat="server" Text=""></asp:Label>
               <div class="row">
               <div class="one half">
                    <asp:DropDownList ID="hlas_type" runat="server"  
                        AutoPostBack="True" OnSelectedIndexChanged="Calendar1_SelectionChanged" 
                        ontextchanged="hlas_type_SelectedIndexChanged">
                        <asp:ListItem Value="A">Oddelenie A</asp:ListItem>
                        <asp:ListItem Value="B">Oddelenie B</asp:ListItem>
                        <asp:ListItem Value="OP">Op.pohotovosť</asp:ListItem>
                    </asp:DropDownList>
                    </div>
                    <div class="one half">
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White"  Font-Size="Medium"
                            BorderColor="#999999" DayNameFormat="Shortest" 
                              ForeColor="Black" 
                            OnSelectionChanged="Calendar1_SelectionChanged" CellPadding="4" 
                            >
                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#CCCCCC" />
                        <WeekendDayStyle BackColor="#FFFFCC" />
                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" />
                        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                        </asp:Calendar>
                    </div>
                    </div>
                    
                    <div class="row">
                        <div class="one half">
                        <p>Sem <strong style="color:Maroon;">copy/paste priezvisko</strong> z hlásenia následne po jeho vložení za ním stlačiť klávesu enter, tak aby každé priezvisko začínalo na novom riadku, pre uloženie/vygenerovanie stlačiť <strong> uložiť</strong>, nie je nutné dodržiavať veľké malé písmená. Automaticky sa vygeneruje odkaz na OSIRIX</p>
                        
                         <asp:TextBox ID="osirix_txt" runat="server" Height="150px" TextMode="MultiLine" ></asp:TextBox>
                        <%--<asp:Button ID="osirix_btn" runat="server" Text="Ulož a Generuj" onclick="osirix_btn_Click" BackColor="#990000" ForeColor="Yellow" CssClass="button" />--%>
                        </div>
                        
                        <div class="one half">
                            <p><strong style="color:Maroon;">Klikni na link, pre otvorenie v OSIRIXe</strong><br /><br />Tento link funguje len vrámci DFNsP!!!!!</p>
                            <asp:Label ID="osirix_url" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                   
                    <h1>Hlásenie služby:</h1>
                    
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
</asp:Content>

