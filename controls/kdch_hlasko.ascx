<%@ Control Language="C#" AutoEventWireup="true" CodeFile="kdch_hlasko.ascx.cs" Inherits="controls_druhadk_hlasko" EnableViewState="true" %>
<asp:HiddenField ID="hlaskoSelectedTab" runat="server" />
<h1 class="black">Hlásenie služby</h1>
<hr />
Vytvoril:<asp:Label ID="creatUser_lbl" runat="server" Text="" Font-Bold="true"></asp:Label><br />
    Posledná zmena:<asp:Label ID="lastUser_lbl" runat="server" Text="" Font-Bold="true"></asp:Label>
<hr />
  <asp:literal ID="msg_lbl" runat="server"></asp:literal>
<div class="row">
<div class="dismissible info message">
    <h3 class="red">Nové hlásko</h3>
    Hlásenie bolo rozdelené, na tri taby. V prvom píšete Vaše EPČ, v druhom tab-e sa Vám generuje EPČ zoznam a v treťom hlásenie služby.
    Stačí sa len kliknúť do daného tabu a uvidíte čo ste vložili.<br />
    T.č. pribudlo aj miesto kde píšete EPČ, je to len kvôli sprehľadneniu.<br />
    Samozrejme kliknutím na túto správu táto zmizne.....
</div>
</div>
<div id="hlasko_tabs">
    <ul>
        <li><a href="#hlasko_tab1">Písanie EPČ</a></li>
        <li><a href="#hlasko_tab2">Zoznam EPČ</a></li>
        <li><a href="#hlasko_tab3">Hlásenie služby</a></li>
    </ul>

    <div id="hlasko_tab1"> <!--zaciatok tab1-->
        <div class="row">
            <div class="one half">
            <%--OnSelectionChanged="Calendar1_SelectionChanged"--%>
            <h3 class="green">Služba:</h3>
                <asp:DropDownList ID="shiftType_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Calendar1_SelectionChanged"></asp:DropDownList>
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" OnSelectionChanged="Calendar1_SelectionChanged"  CssClass="responsive" data-max="15"
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

            <div class="one half padded">
                Zobraz v OSIRIXe
                <hr />
                <asp:Table ID="osirix_tbl" runat="server"></asp:Table>
         
            </div> 
        
        
      </div>

        <div class="row">
        <asp:Label ID="epc_titel" runat="server" Text="<%$ Resources:Resource,epc_titel %>" CssClass="green large"></asp:Label>
</div>
<div class="box blue">  
<div class="row">
    
      
            <div class="one fifth">
                Dátum:
                <asp:DropDownList ID="hl_datum_cb" runat="server"></asp:DropDownList>
            </div>
            <div class="one fifth">
                <asp:Label ID="time_valid_msg" runat="server" Text="<%$ Resources:Resource,no_valid_time %>" Visible="false" CssClass="red"></asp:Label>
                Začiatok (formát hh:mm)   <asp:TextBox ID="jsWorkstarttxt" runat="server" Text=""></asp:TextBox>
            </div>
            <div class="one fifth">
                Čas trvania (minuty) <asp:TextBox ID="jsWorktimetxt" runat="server" Text="15"></asp:TextBox>
                <asp:Label ID="minute_valid_msg" runat="server" Text="<%$ Resources:Resource,no_valid_int %>" Visible="false" CssClass="red"></asp:Label>
            </div>
             <div class="one fifth">
                 Oddelenie:
                 <asp:DropDownList ID="clinicDep_dl" runat="server"></asp:DropDownList>

             </div>
            <div class="one fifth">
                Typ:<asp:DropDownList ID="worktype_cb" runat="server">
                    <asp:ListItem Value="prijem">Príjem</asp:ListItem>
                <asp:ListItem Value="operac">Operácia/Asistencia/Výkon</asp:ListItem>
                <asp:ListItem Value="sledov">Sledovanie</asp:ListItem>
                <asp:ListItem Value="konzil">Konzílium</asp:ListItem>
                <asp:ListItem Value="vizita">Vizita</asp:ListItem>
                <asp:ListItem Value="dekurz">Dekurzovanie</asp:ListItem>
                <asp:ListItem Value="urgent">Urgent</asp:ListItem>
                    </asp:DropDownList>
            </div> 
</div>

<div class="row">
        <div class="one half">
            
            Priezvisko pacienta (len priezvisko!!!!):
            <asp:TextBox ID="patientname_txt" runat="server"></asp:TextBox>
            <p class="yellow small">Ak nevypíšete meno, systém automaticky vloží slovo pacient !!!!</p>

        </div>
        <div class="one half">
            Popis aktivity:<asp:TextBox ID="activity_txt" runat="server" TextMode="MultiLine"></asp:TextBox>
        </div> 
</div>
<div class="row">
        <asp:CheckBox ID="check_osirix" runat="server" Text=""/>  
        <asp:Label ID="Label2" runat="server" Text="   RDG v Prehliadaci" CssClass="mojInline"></asp:Label>
       
</div>

<div class="row">
       <asp:PlaceHolder ID="fileupload_pl" runat="server" >
          <%-- <div class="box blue align-left">--%>
               Externý súbor....<br />
                <asp:FileUpload ID="loadFile_fup" runat="server"  Width="400" Height="36" size="40px" CssClass="duch" />
               <asp:Button ID="upLoadFile_btn" runat="server" Text="Nahraj...Max.200MB" OnClick="uploadData_fnc" /> <br /><br />
               <asp:Label ID="upLoadedFile_lbl" runat="server" Text="" CssClass="red"></asp:Label>
               <asp:HiddenField ID="lfId_hidden" runat="server" Value="0" />
           <%-- </div>--%>
        </asp:PlaceHolder>
</div>
<div class="row">
    <div class="align-right">
          <asp:Button ID="activitysave_btn" runat="server" Text="Pridaj do EPC" OnClick="saveActivity_fnc"  CssClass="button asphalt"  />
    </div>
</div>
     </div> <%-- blue bx--%>




    </div><!--koniec tab1-->
    <div id="hlasko_tab2"><!--zaciatok tab2-->
         <div class="row">
    
   
        <div class="success box">
            <asp:Table ID="activity_tbl" runat="server"></asp:Table>
            <asp:Label ID="kompl_work_time" runat="server" Text=""></asp:Label><br />
            <asp:Button ID="generate_btn" runat="server" Text="Generuj hlasko" CssClass="button green" /> 
        </div>

    </div>
    </div><!--koniec tab2-->
    <div id="hlasko_tab3"><!--zaciatok tab3-->
        <h3>Zobraz hlasenia</h3>
        <div class="row">
            <asp:Button CssClass="medium button red" runat="server" ID="showOup_btn" OnClick="showHlasko_fnc" Text="OUP" />
            <asp:Button CssClass="medium button blue" runat="server" ID="showOddA_btn" OnClick="showHlasko_fnc" Text="OddA" />
            <asp:Button CssClass="medium button green" runat="server" ID="showOddB_btn" OnClick="showHlasko_fnc" Text="OddB" />
            <asp:Button CssClass="medium button asphalt" runat="server" ID="showOp_btn" OnClick="showHlasko_fnc" Text="Op. pohotovost" />
            <asp:TextBox ID="hlasenie" CssClass="dtextbox" runat="server"  Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox>
            <asp:Button ID="saveHlasko_btn" runat="server" Text="Ulož zmenu" OnClick="saveHlaskoFnc" CssClass="button green" />
                <asp:Button ID="print_btn" runat="server" Text="Vytlač" CssClass="button blue" OnClick="printFnc" />
                <asp:Button ID="toWord_btn" runat="server" Text="Tlač/Word"  />
                <asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                    CssClass="button red" />          
        </div>
    </div><!--koniec tab3-->

</div>  <!-- Koniec tabov -->  
  
    

       







