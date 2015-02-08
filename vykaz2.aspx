﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="vykaz2.aspx.cs" Trace="false" Inherits="vykaz2" Culture="sk-SK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
  
 <h1> Mesačný výkaz</h1>
    <div class="red">Ako prvé je nutné si vybrať mesiac a rok, následne je nutné stlačiť tlačidlo <strong>Vytvor výkaz</strong>. Vygeneruje sa už výkaz aj s vypočítanými hodnotami. Ak je nutné 
        upraviť tak si hodnoty a stlačiť tlačidlo <strong>Vypočítaj</strong>... a až potom je možné výkaz vytlačiť pomocou tlačidla Tlač...<br />
        Ak chcete vytvoriť nový výkaz tak je nutné stlačiť tlačidlo <strong>Nový výkaz</strong>
    </div>
     <hr />
    <asp:PlaceHolder ID="anotherUser_pl" runat="server">
        <div class="row">
            <div class="one third">Výkaz pre uživateľa:</div>
        <div class="one third"> <div align="left"><asp:DropDownList ID="doctors_dl" runat="server"></asp:DropDownList></div></div>
        <div class="one third"><div align="left"><asp:Button ID="insertForUser_btn" runat="server" Text="Načítaj" /></div></div>
            </div>
     <hr />   
    </asp:PlaceHolder>
    <div class="row">
        <div class="one third"><%--OnSelectedIndexChanged = "onMonthChangedFnc"--%>
            <asp:Button ID="newVykaz_btn" runat="server" CssClass="button asphalt"  Text="Novy vykaz" Enabled="false" OnClick="newVykaz_fnc" />
             
                <asp:Label ID="zaMesiac_lbl" runat="server"></asp:Label>
                 Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" Width="100"  CssClass="mojInline">
                        <asp:ListItem Value="1">Január</asp:ListItem>
                        <asp:ListItem Value="2">Február</asp:ListItem>
                        <asp:ListItem Value="3">Marec</asp:ListItem>
                        <asp:ListItem Value="4">Apríl</asp:ListItem>
                        <asp:ListItem Value="5">Máj</asp:ListItem>
                        <asp:ListItem Value="6">Jún</asp:ListItem>
                        <asp:ListItem Value="7">Júl</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>   
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">Október</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>  
        </div>
        <div class="one third"> 
            
                    Rok: <%--OnSelectedIndexChanged = "onYearChangedFnc"--%>
                <asp:DropDownList ID="rok_cb" runat="server" CssClass="mojInline" Width="100">
                    <asp:ListItem Value="2010">Rok 2010</asp:ListItem>
                    <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                    <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                    <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
                    <asp:ListItem Value="2014">Rok 2014</asp:ListItem>
                     <asp:ListItem Value="2015">Rok 2015</asp:ListItem>
                      <asp:ListItem Value="2016">Rok 2016</asp:ListItem>
                </asp:DropDownList>
                
            </div>
        <div class="one third">
            
         <asp:Button ID="generateVykaz_btn" runat="server" CssClass="button green"  Text="Vytvor vykaz" OnClick="generateVykaz_fnc" />    
            <asp:Button ID="generateEPC_btn" runat="server" CssClass="button blue"  Text="<%$ Resources:Resource,generate_epc %>" OnClick="generateEpc_fnc" />
        </div>
        <asp:PlaceHolder ID="vykazInfoHours_pl" runat="server" Visible="false">
             
         <div class="row">
            <div class="one">
                    <hr />
                        <asp:Label ID="predchMes_lbl" runat="server" Text="Prenos z predchadzajuceho mesiaca:" ></asp:Label><asp:TextBox ID="predMes_txt" runat="server" Width="50" CssClass="mojInline"></asp:TextBox>
                        <asp:Button ID="calcData_btn" runat="server" onclick="calcData_Click" Text="Vypocitaj" CssClass="mojInline" />
                        <asp:Button ID="createPdf_btn" runat="server" Text="Tlac" OnClick="createPdf_btn_fnc" Enabled="false" CssClass="mojInline" />
                    <hr />
            </div>
        </div>

        <div class="row">
            <div class="one">
                        Pocet hodin podla dni:<asp:TextBox ID="pocetHod_txt" runat="server" Width="50" CssClass="mojInline"></asp:TextBox>
                        <%--<asp:Label ID="pocetHod_lbl" runat="server"></asp:Label> --%>
                        Rozdiel medzi: <strong><asp:Label ID="rozdiel_lbl" runat="server" CssClass="mojInline"></asp:Label></strong>
                
                <hr />
            </div>
        </div>
        </asp:PlaceHolder>
        <div class="row">

            <asp:Table ID="vykaz_tbl" runat="server" CssClass="responsive" data-max="13"></asp:Table>

        </div>


    </div>



</asp:Content>

