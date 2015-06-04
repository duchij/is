<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nkim_hlasko.ascx.cs" Inherits="nkim_hlasko" EnableViewState="true" %>
<hr />
Vytvoril:<asp:Label ID="creatUser_lbl" runat="server" Text="" Font-Bold="true"></asp:Label><br />
    Posledná zmena:<asp:Label ID="lastUser_lbl" runat="server" Text="" Font-Bold="true"></asp:Label>


<hr />
  <asp:Label ID="ctrl_msg_lbl" runat="server"></asp:Label>
<div class="row">
    
  
    
    <div class="one half">
        <%--OnSelectionChanged="Calendar1_SelectionChanged"--%>
        Služba:
        <asp:DropDownList ID="shiftType_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Calendar1_SelectionChanged"></asp:DropDownList>
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
    <div class="one half padded">
        RDG v prehliadači....
        <hr />
        <asp:Table ID="osirix_tbl" runat="server"></asp:Table>
         
    </div> 
</div>
    <div class="row">
    
   
        <div class="success box">
            <asp:Table ID="activity_tbl" runat="server"></asp:Table>
            <asp:Label ID="kompl_work_time" runat="server" Text=""></asp:Label><br />
            <asp:Button ID="generate_btn" runat="server" Text="Generuj hlasko" CssClass="button green" /> 
        </div>

    </div>

    <br />
<div class="row">
        <asp:Label ID="epc_titel" runat="server" Text="<%$ Resources:Resource,epc_titel %>" CssClass="green"></asp:Label>
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
                    <asp:ListItem Value="vyhodV">Vyhodnocovanie výsledkov</asp:ListItem>
                    <asp:ListItem Value="upravL">Úprava liečby</asp:ListItem>
                    <asp:ListItem Value="konzul">Konzultácia</asp:ListItem>
                    <asp:ListItem Value="vizita">Vizita</asp:ListItem>
                    <asp:ListItem Value="dekurz">Dekurzovanie</asp:ListItem>
                    <asp:ListItem Value="konzil">Konzílium</asp:ListItem>
                    <%--<asp:ListItem Value="urgent">Urgent</asp:ListItem>--%>
                    </asp:DropDownList>
            </div> 
        </div>

<div class="row">
        <div class="one half">
            
            Priezvisko pacienta (len priezvisko!!!!):
            <asp:TextBox ID="patientname_txt" runat="server"></asp:TextBox>

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
<asp:TextBox ID="hlasenie" CssClass="dtextbox" runat="server"  Width="90%" Rows="30" Height="500" TextMode="MultiLine"> </asp:TextBox>
<asp:Button ID="saveHlasko_btn" runat="server" Text="Ulož zmenu" OnClick="saveHlaskoFnc" CssClass="button green" />
                
               
                <asp:Button ID="print_btn" runat="server" Text="Vytlač" CssClass="button blue" OnClick="printFnc" />
                
                <asp:Button ID="toWord_btn" runat="server" Text="Tlač/Word"  />
                
                <asp:Button ID="def_lock_btn" runat="server" Text="Uzavrieť a vytlačiť hlásko" 
                    CssClass="button red" />          
       







