<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hlasko.aspx.cs" Inherits="hlasko" MaintainScrollPositionOnPostback="true"  %>

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
                        <asp:ListItem Value="OUP">OÚP</asp:ListItem>
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
                    
                   
                   
                    <h1>Hlásenie služby:</h1>
    <asp:PlaceHolder ID="epc_pl" runat="server"  Visible="true">
        <div class="info box">
      <asp:Label ID="epc_titel" runat="server" Text="<%$ Resources:Resource,epc_titel %>" CssClass="green"></asp:Label>
    <div class="row">
      
        <div class="one fourth">
         
           
    Dátum:
    <asp:DropDownList ID="hl_datum_cb" runat="server"></asp:DropDownList>
            </div>
        <div class="one fourth">
            
            <asp:Label ID="time_valid_msg" runat="server" Text="<%$ Resources:Resource,no_valid_time %>" Visible="false" CssClass="red"></asp:Label>
    Začiatok (formát hh:mm)   <asp:TextBox ID="jsWorkstarttxt" runat="server" Text=""></asp:TextBox>
  </div>
        <div class="one fourth">
             Čas trvania (minuty) <asp:TextBox ID="jsWorktimetxt" runat="server" Text="15"></asp:TextBox>
              <asp:Label ID="minute_valid_msg" runat="server" Text="<%$ Resources:Resource,no_valid_int %>" Visible="false" CssClass="red"></asp:Label>
            </div>
        <div class="one fourth">
    Typ:<asp:DropDownList ID="worktype_cb" runat="server">
            <asp:ListItem Value="prijem">Príjem</asp:ListItem>
         <asp:ListItem Value="operac">Operácia/Asistencia/Výkon</asp:ListItem>
         <asp:ListItem Value="sledov">Sledovanie</asp:ListItem>
         <asp:ListItem Value="konzil">Konzílium</asp:ListItem>
        <asp:ListItem Value="vizita">Vizita</asp:ListItem>
        <asp:ListItem Value="dekurz">Dekurzovanie</asp:ListItem>
        </asp:DropDownList>
            </div> 
    </div>
    <div class="row">
        <div class="one half">
            
    Priezvisko pacienta (len priezvisko!!!!):
    <asp:TextBox ID="patientname_txt" runat="server"></asp:TextBox></div>
        <div class="one half">
    Popis aktivity:<asp:TextBox ID="activity_txt" runat="server" TextMode="MultiLine"></asp:TextBox>
            </div> 
        </div>
    <div class="row">
    <asp:CheckBox ID="check_osirix" runat="server" Text=""/>  <asp:Label ID="Label2" runat="server" Text="   Zobraz v OSIRIXe" CssClass="mojInline"></asp:Label>
       
        </div>
   <div class="row">
       <asp:PlaceHolder ID="fileupload_pl" runat="server" >
           <div class="box blue align-left">
               Externý súbor....<br />
           <asp:FileUpload ID="loadFile_fup" runat="server" Width="500" Height="36"  />
               <asp:Button ID="upLoadFile_btn" runat="server" Text="Nahraj...Max.200MB" OnClick="uploadData_fnc" />
               <asp:Label ID="upLoadedFile_lbl" runat="server" Text="" CssClass="red"></asp:Label>
               <asp:HiddenField ID="lfId_hidden" runat="server" Value="0" />
            </div>
        </asp:PlaceHolder>
   </div>
   <div class="row">
       <div class="align-right">
           

   <asp:Button ID="activitysave_btn" runat="server" Text="Pridaj do EPC" OnClick="saveActivity_fnc" CssClass="button asphalt"  />
           </div>
       </div>
             </div>
        </asp:PlaceHolder>
   
        
    <asp:PlaceHolder ID="ativitylist_pl" runat="server">
        <div class="success box">
        <asp:Table ID="activity_tbl" runat="server"></asp:Table>
            <asp:Label ID="kompl_work_time" runat="server" Text=""></asp:Label><br />
        <asp:Button ID="generate_btn" runat="server" Text="Generuj hlasko" CssClass="button green"  OnClick="generateHlasko_fnc" /> 
            </div>
            </asp:PlaceHolder>
   
    <%--<asp:Button ID="generateEPC_btn" runat="server" CssClass="button blue"  Text="<%$ Resources:Resource,generate_epc %>" OnClick="generateEpc_fnc" />--%>
    <hr />
    <asp:PlaceHolder ID="hlasko_pl" runat="server">

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
                    <h3><asp:Label ID="Label1" runat="server" CssClass="red" Text="<%$ Resources:Resource, hlasko_info %>"></asp:Label></h3>
        
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

