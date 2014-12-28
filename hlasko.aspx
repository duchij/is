<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hlasko.aspx.cs" Inherits="hlasko"  %>

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
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White"  CssClass="responsive" data-max="15"
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
                        <h3>
                        <asp:Label ID="osirix_info" runat="server" Text="<%$ Resources:Resource, osirix_info %>"></asp:Label>
                        </h3>
                        
                         <asp:TextBox ID="osirix_txt" runat="server" Height="150px" TextMode="MultiLine" ></asp:TextBox>
                        <%--<asp:Button ID="osirix_btn" runat="server" Text="Ulož a Generuj" onclick="osirix_btn_Click" BackColor="#990000" ForeColor="Yellow" CssClass="button" />--%>
                        </div>
                        
                        <div class="one half">
                            <p><strong style="color:Maroon;">Klikni na link, pre otvorenie v OSIRIXe</strong><br /><br />Tento link funguje len vrámci DFNsP!!!!!</p>
                            <asp:Label ID="osirix_url" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                   
                    <h1>Hlásenie služby:</h1>
    Datum:
    <asp:DropDownList ID="hl_datum_cb" runat="server"></asp:DropDownList>
    Zaciatok (format hh:mm)   <asp:TextBox ID="workstart_txt" runat="server" ></asp:TextBox> 
   Cas trvanie (minuty) <asp:TextBox ID="worktime_txt" runat="server" Text="15"></asp:TextBox>
    Typ:<asp:DropDownList ID="worktype_cb" runat="server">
            <asp:ListItem Value="prijem">Prijem</asp:ListItem>
         <asp:ListItem Value="operac">Operacia</asp:ListItem>
         <asp:ListItem Value="sledov">Slodovani</asp:ListItem>
         <asp:ListItem Value="konzil">Konziulum</asp:ListItem>
        </asp:DropDownList>
    Meno pacienta:
    <asp:TextBox ID="patientname_txt" runat="server"></asp:TextBox>
    Popis aktivity:<asp:TextBox ID="activity_txt" runat="server" TextMode="MultiLine"></asp:TextBox>
    <asp:Button ID="activitysave_btn" runat="server" Text="Pridaj" OnClick="saveActivity_fnc" />
    <asp:Button ID="generate_btn" runat="server" Text="Generuj hlasko" />
    <asp:PlaceHolder ID="ativitylist_pl" runat="server">

    </asp:PlaceHolder>
    <asp:PlaceHolder ID="hlasko_pl" runat="server">
                    
                    <asp:TextBox ID="hlasenie" CssClass="dtextbox" runat="server"  Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox>
                    <%--<FTB:FreeTextBox ID="hlasenie" runat="server" Width="100%" Height="500"  toolbarlayout="Bold, Italic, Underline,RemoveFormat|FontFacesMenu,FontSizesMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull; Redo,Undo ,BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell"></FTB:FreeTextBox>--%>
                    <hr />
                 <asp:Label ID="hlasko_lbl" runat="server" Visible="False" Font-Size="Small" >Hlasenie:</asp:Label><br />
                <asp:Label ID="view_hlasko" runat="server" Visible="False" Font-Size="Small"></asp:Label>
                <asp:TextBox ID="dodatok"  CssClass="dtextbox" runat="server" Width="90%" Rows="10" Height="100" TextMode="MultiLine" Visible="False">dodatok</asp:TextBox><br />
                    <asp:Button ID="send" runat="server" Text="Ulož zmenu" onclick="send_Click" CssClass="button green" />
                
               
                <asp:Button ID="print_btn" runat="server" onclick="Button1_Click" Text="Vytlač" CssClass="button blue" />
                
                <asp:Button ID="toWord_btn" runat="server" onclick="toWord_Click" Text="Tlač/Word"  />
                
                <asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                    onclick="def_lock_btn_Click" CssClass="button red" />
                
                 <asp:Button ID="def_locl_w_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko/Word" 
                    onclick="def_lock_btn_w_Click" CssClass="button red" />    
                    
                <asp:Button ID="addInfo_btn" runat="server" Text="Ulož dodatok" Enabled="False" 
                    onclick="addInfo_btn_Click" CssClass="button asphalt" />
        </asp:PlaceHolder>
</asp:Content>

