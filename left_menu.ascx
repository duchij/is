﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="left_menu.ascx.cs" Inherits="left_menu" %>

                 <div id="menu_small_title"><asp:label ID="Label2" runat="server" Text="<%$ Resources:Resource,lmenu_rozpis %>"></asp:label></div>
                <ul>
                    
                    <li><a href="sluzby.aspx" target="_self"><asp:Localize runat="server" ID="localize1" Text="<%$ Resources:Resource,odd_akt_sluz %>"></asp:Localize></a></li>
                    <li><a href="staze.aspx" target="_self"><asp:Localize runat="server" ID="localize2" Text="<%$ Resources:Resource,odd_staze %>"></asp:Localize></a></li> 
                    </ul>
                        
                    <asp:PlaceHolder ID="users" runat="server"> 
                     <div id="menu_small_title"><asp:label ID="Label3" runat="server" Text="<%$ Resources:Resource,lmenu_oddelenie %>"></asp:label></div>
                      <ul>               
                        <li><a href="hlasko.aspx" target="_self">Hlásenie služieb</a></li>
                        <li><a href="vykaz.aspx" target="_self">Mesačný výkaz</a></li>
                        <li><a href="dovolenky.aspx" target="_self">Dovolenky</a></li> 
                        <li><a href="poziadavky.aspx" target="_self"><asp:label ID="poziadavky_lbl" runat="server" Text="<%$ Resources:Resource,odd_poziadavky %>"></asp:label></a></li>
                     </ul>                    
                    </asp:PlaceHolder>
                    
                    <asp:PlaceHolder ID="admin" runat="server">
                    <ul>
                    <li><a href="dov_stat.aspx" target="_self">Dovolenky - stav</a></li>
                    </ul>
                    </asp:PlaceHolder>
                    
                    <asp:PlaceHolder ID="sestra" runat="server">
                    <div id="menu_small_title"><asp:label ID="Label1" runat="server" Text="<%$ Resources:Resource,lmenu_sestry %>"></asp:label></div>
                    <ul>
                    <li><a href="sestrhlas.aspx" target="_self">Hlásenie sestier</a></li> 
                    </ul>                   
                    </asp:PlaceHolder>
                    
                    <div id="menu_small_title"><asp:label ID="Label4" runat="server" Text="<%$ Resources:Resource,lmenu_user %>"></asp:label></div>
                    <ul>
                    <li><a href="adduser.aspx" target="_self">Užívateľ</a> </li>  
                    </ul>  
                    <!--<li><a href="opkniha.aspx" target="_self">Operačná kniha</a> </li>!-->          
                    
                    
                </ul>
                
