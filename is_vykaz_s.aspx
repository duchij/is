<%@ Page Language="C#" AutoEventWireup="true" CodeFile="is_vykaz_s.aspx.cs" Inherits="is_vykaz_s" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>

 
  
 <h1> Mesačný výkaz sestry</h1>
    <hr />
    <p class="red">
        
        <strong>
            Skontrolujte si Vaše osobné informácie, pretože ak ich nebudete mať správne vyplnené Váš výkaz bude neúplne vytlačený. Vaše údaje si može skontrolovať v 
            <a href="is_user.aspx" target="_self">nastaveniach užívateľa</a>. Po atktualizácii je nutné sa odhlásiť a nanovo prihlásiť...
        </strong>
        <hr />
    </p>
    <div class="dismissible info message">Ako prvé je nutné si vybrať mesiac a rok, následne je nutné stlačiť tlačidlo <u>Vytvor výkaz</u>. Vygeneruje sa už výkaz aj s vypočítanými hodnotami. 
        Hodnoty si vyždz skontrolujte a upravte a následne a stlačte tlačidlo <u>Vypočítaj</u>... a až potom je možné výkaz vytlačiť pomocou tlačidla Tlač...<br />
        Ak chcete vytvoriť nový výkaz tak je nutné stlačiť tlačidlo <u>Nový výkaz</u>
        <br />
        <p class="red">
            Ak si zmeníte služby, dovolenky a pod Váš uložený výkaz sa vygeneruje nanovo a vy prídete o všetky zmeny. Preto si generujte výkaz ako úplne posledný...
        </p>
    </div>
     <hr />
    <asp:PlaceHolder ID="anotherUser_pl" runat="server">
        <div class="row">
            <div class=" dismissible warning message block">
                Pozor!!!!! Ak vyberiete oddelenie a sestru pre dané oddelenie, bude sa zobrazovať a prepočítavať výkaz pre danú sestru a nie
                pre prihláseného uživateľa, pre návrat do normálneho režimu stlačte tlačidlo <u>Nový výkaz</u>

            </div>
         <div class="one third">
             <h2>Výkaz pre uživateľa:</h2>
            
        </div>

        <div class="one third"> <div align="left"><asp:DropDownList ID="deps_dl" runat="server" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="loadNurses_fnc"></asp:DropDownList><asp:DropDownList ID="nurses_dl" runat="server"></asp:DropDownList></div></div>
        <div class="one third"><div align="left"><asp:Button ID="insertForUser_btn" EnableViewState="true" runat="server" CssClass="button red" Text="Načítaj vykaz pre sestru" OnClick="generateVykaz_fnc" /></div></div>
            
     <hr /> 
    </div>  
    </asp:PlaceHolder>
    <div class="row">
        <div class="one third"><%--OnSelectedIndexChanged = "onMonthChangedFnc"--%>
            <asp:Button ID="newVykaz_btn" runat="server" CssClass="button asphalt"  Text="Nový výkaz" Enabled="true" OnClick="newVykaz_fnc" />
             
                <asp:Label ID="zaMesiac_lbl" runat="server"></asp:Label>
                 Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" Width="100"  CssClass="mojInline">
                        
                    </asp:DropDownList>  
        </div>
        <div class="one third"> 
            
                    Rok: <%--OnSelectedIndexChanged = "onYearChangedFnc"--%>
                <asp:DropDownList ID="rok_cb" runat="server" CssClass="mojInline" Width="100">
                </asp:DropDownList>
                
            </div>
        <div class="one third">
            
         <asp:Button ID="generateVykaz_btn" runat="server" CssClass="button green"  Text="Vytvor výkaz" OnClick="generateVykaz_fnc" />    
           <!-- <asp:Button ID="generateEPC_btn" runat="server" CssClass="button blue"  Text="<%$ Resources:Resource,generate_epc %>" OnClick="generateEpc_fnc" /> -->
        </div>
        <asp:PlaceHolder ID="vykazInfoHours_pl" runat="server" Visible="false">
             
         <div class="row">
            <div class="one">
                    <hr />
                        <asp:Label ID="predchMes_lbl" runat="server" Text="Prenos z predchadzajuceho mesiaca:" ></asp:Label><asp:TextBox ID="predMes_txt" runat="server" Width="50" CssClass="mojInline" Text="0"></asp:TextBox>
                        <asp:Button ID="calcData_btn" runat="server" onclick="calcData_Click" Text="Vypočítaj" CssClass="mojInline" />
                        <asp:Button ID="createPdf_btn" runat="server" Text="Tlač" OnClick="createPdf_btn_fnc" Enabled="false" CssClass="mojInline" />
                    <hr />
            </div>
        </div>

        <div class="row">
           <div class="one">
                        <!--Pocet prac. dni:<asp:TextBox ID="ine_p_dni_txt" runat="server" Width="50"></asp:TextBox>-->
                        Počet hodín podľa dní:<asp:TextBox ID="pocetHod_txt" runat="server" Width="50" CssClass="mojInline"></asp:TextBox>
                        <asp:Label ID="pocetHod_lbl" runat="server"></asp:Label>
                        Rozdiel medzi: <strong><asp:Label ID="rozdiel_lbl" runat="server" CssClass="mojInline"></asp:Label></strong>
              
                <hr />
            </div>  
        </div>
        </asp:PlaceHolder>
        <div class="row">

            <asp:Table ID="vykaz_tbl" runat="server" CssClass="responsive" data-max="13" EnableViewState="true"></asp:Table>

        </div>


    </div>



</asp:Content>
