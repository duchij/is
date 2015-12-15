<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sestrhlas.aspx.cs" Inherits="sestrhlas"  MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
<h2>Hlásenie sestier KDCH</h2>
                <hr />
                   Prihlasený: <strong><asp:Label ID="user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                   Poslednú zmenu vykonal:<strong> <asp:Label ID="last_user" runat="server" Text="Label" ForeColor="#990000"></asp:Label></strong><br />
                <hr />
    <div class="yellow box">
    <a href="lf.aspx?id=323" target="_blank" role="author" class="large asphalt">Sesterská prekladová správa</a>
        <p class="small black">Kliknutím sa otvorí prekladová správa vo Worde, poznm. nutnosť mať nainštalovaný Word 2007 a vyšší
            následne sa dá do "chlievikov" vpisovať normálne vo Worde text. Ak tam niečo nie je stačí dokument len normálne vytlačiť a dopísať rukou.
            <strong>Pozor</strong> v hlavičke je nutné si vybrať oddelenie.
        </p>
    </div>
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
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" OnSelectionChanged="Calendar1_SelectionChanged"  CssClass="responsive" data-max="12"
                            BorderColor="#d9edf7" DayNameFormat="Shortest"  
                              ForeColor="Black" 
                             CellPadding="4" >
                        <SelectedDayStyle BackColor="#46627f" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#46627f" />
                        <WeekendDayStyle BackColor="#dff0d8" />
                        <TodayDayStyle BackColor="#f0e6f4" ForeColor="black" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#ffe5c7" Font-Bold="True" />
                        <TitleStyle BackColor="#d9edf7" BorderColor="Black" Font-Bold="True" />
                        </asp:Calendar>
                    </div>
                </div>
                    <p>
                        <h2 style="color:red;">Dôležité.....</h2>
                        Ak vložíte niečo priamo z MEDEI alebo z Wordu do editora, môže Vám to rozhádzať hlásenie a nemusí sa Vám ani Vašej kolegyni potom text správne uložiť.
                        Preto.....
                        <ul>
                            <li><strong>Nevkladajte veci z MEDEI priamo</strong> do hlásenia ak chcete vložiť niečo z MEDEI tak v menu editora si dajte <b>Upraviť</b> a <b>Vložiť ako Text</b>. 
                                Potom sa Vám nerozhádže ani text ani fonty. To isté platí aj o prenášaní z Wordu. Síce príjdete o formátovanie ale nestratíte text... To je na Vás :) </li>
                            <li><strong>Aby Vám editor nerobil veľké medzery medzi riadkami</strong> použite miesto Enteru kombináciu <strong>SHIFT+Enter</strong>, kurzor Vám skočí do ďalšieho riadku bez medzere.</li>
                        </ul>

                    </p>
            <div class="blue box">
                <asp:CheckBox ID="listCodes_chk" runat="server" AutoPostBack="true" Checked="false" /> <asp:Label AssociatedControlID="listCodes_chk" runat="server" CssClass="mojInline">Zobraz sesterské diagnózy</asp:Label>
               <p><small>Umožňuje vybrať sesterskú diagnózu podľa skupiny. Zobrazenie vypnete odkvačnutím kvačky</small></p>
                
                 </div>
                <asp:PlaceHolder ID="nursecodes_plh" runat="server"></asp:PlaceHolder>
                   <asp:TextBox ID="hlasenie" CssClass="dtextbox" runat="server" Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox> 
                    
                <%--<FTB:FreeTextBox ID="hlasenie"  Height="500" Width="100%" toolbarlayout="Bold, Italic, Underline,RemoveFormat, Redo, Undo|FontFacesMenu,FontSizesMenu|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent|WordClean,NetSpell"
runat="Server"></FTB:FreeTextBox>--%>
   
                 <asp:Label ID="hlasko_lbl" runat="server" Visible="False" Font-Size="Small" >Hlasenie:</asp:Label>
                <asp:Label ID="view_hlasko" runat="server" Visible="False" Font-Size="Small"></asp:Label>
               <%-- <asp:TextBox ID="dodatok" runat="server" Width="90%" Rows="10" Height="100" TextMode="MultiLine" Visible="False">dodatok</asp:TextBox><br />--%>
                    <asp:Button ID="send" runat="server" Text="Ulož zmenu"  OnClick="save_fnc" />
                    
                    <asp:Button ID="copyYesterday_btn" runat="server" Text="<%$Resources:Resource,odd_vloz_clip %>" onclick="copyYesterday_btn_Click" />
                <asp:Button ID="print_btn" runat="server"  Text="Vytlač" onClick = "print_fnc" />
                <%--<asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                     BackColor="#990000" ForeColor="Yellow" OnClick="def_loc_fnc" />--%>
                <%--<asp:Button ID="addInfo_btn" runat="server" Text="Ulož dodatok" Enabled="False" onclick="addInfo_btn_Click"/>--%>
</asp:Content>

