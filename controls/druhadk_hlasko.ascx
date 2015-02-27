<%@ Control Language="C#" AutoEventWireup="true" CodeFile="druhadk_hlasko.ascx.cs" Inherits="controls_druhadk_hlasko" %>

<div class="row">
    <div class="one third">
        <%--OnSelectionChanged="Calendar1_SelectionChanged"--%>
         <asp:Calendar ID="Calendar1" runat="server" BackColor="White" 
                            BorderColor="#999999" DayNameFormat="Shortest" 
                              ForeColor="Black" 
                             CellPadding="4" >
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
    <div class="two tirds">
        aktivity list

    </div>
</div>
    <br />
<div class="row">
        <asp:Label ID="epc_titel" runat="server" Text="<%$ Resources:Resource,epc_titel %>" CssClass="green"></asp:Label>
</div>
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
                 <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>

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

        </div>
        <div class="one half">
            Popis aktivity:<asp:TextBox ID="activity_txt" runat="server" TextMode="MultiLine"></asp:TextBox>
        </div> 
</div>
<div class="row">
        <asp:CheckBox ID="check_osirix" runat="server" Text=""/>  
        <asp:Label ID="Label2" runat="server" Text="   Zobraz v OSIRIXe" CssClass="mojInline"></asp:Label>
       
</div>
<div class="row">
       <asp:PlaceHolder ID="fileupload_pl" runat="server" >
           <div class="box blue align-left">
               Externý súbor....<br />
                <asp:FileUpload ID="loadFile_fup" runat="server"  Width="400" Height="36" size="40px" CssClass="duch" />
               <asp:Button ID="upLoadFile_btn" runat="server" Text="Nahraj...Max.200MB" /> <br /><br />
               <asp:Label ID="upLoadedFile_lbl" runat="server" Text="" CssClass="red"></asp:Label>
               <asp:HiddenField ID="lfId_hidden" runat="server" Value="0" />
            </div>
        </asp:PlaceHolder>
</div>
<div class="row">
    <div class="align-right">
          <asp:Button ID="activitysave_btn" runat="server" Text="Pridaj do EPC"  CssClass="button asphalt"  />
    </div>
</div>
          
       







