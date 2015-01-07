<%@ Control Language="C#" AutoEventWireup="true" CodeFile="left_menu.ascx.cs" Inherits="left_menu" %>

 <asp:PlaceHolder ID="dev_pl" runat="server">
Vyvojar
<ul>
    <li>
    <asp:HyperLink ID="call_sp" runat="server" NavigateUrl="~/helpers/callsp.aspx">Call Stored Procedure</asp:HyperLink></li>
      <li><asp:HyperLink ID="call_consluz" runat="server" NavigateUrl="~/helpers/convsluz.aspx ">Konvertor sluzieb</asp:HyperLink></li> 
        
    </ul>
  </asp:PlaceHolder>

 <asp:PlaceHolder ID="operacky" runat="server">
        <div class="box red half-padded"><h3>Operacky</h3></div>
        <ul>
            <li class="box orange"><a href="opprogram.aspx" target="_self">Operacny program</a></li>
        </ul>
  </asp:PlaceHolder>     
    
<div class="box info half-padded">
    <h3><asp:label ID="Label2" runat="server" Text="<%$ Resources:Resource,lmenu_rozpis %>"></asp:label></h3></div>
        <ul>
                    
            <li><a href="sluzby2.aspx" target="_self"><asp:Localize runat="server" ID="localize1" Text="<%$ Resources:Resource,odd_akt_sluz %>"></asp:Localize></a></li>
            <li><a href="staze.aspx" target="_self"><asp:Localize runat="server" ID="localize2" Text="<%$ Resources:Resource,odd_staze %>"></asp:Localize></a></li> 
            <li><a href="ransed.aspx" target="_self"><asp:Localize runat="server" ID="localize3" Text="<%$ Resources:Resource,ranne_sed %>"></asp:Localize></a></li>
         </ul>   
       
                        
        <asp:PlaceHolder ID="users" runat="server"> 
                    <div class="box info half-padded"> 
                        <h3><asp:label ID="Label3" runat="server" Text="<%$ Resources:Resource,lmenu_oddelenie %>"></asp:label></h3>
                    </div>
                      <ul>               
                        <li><a href="hlasko.aspx" target="_self">Hlásenie služieb</a></li>
                        <li><a href="vykaz2.aspx" target="_self">Mesačný výkaz</a></li>
                        <li><a href="dovolenky.aspx" target="_self">Dovolenky</a></li> 
                        <li><a href="poziadavky.aspx" target="_self"><asp:label ID="poziadavky_lbl" runat="server" Text="<%$ Resources:Resource,odd_poziadavky %>"></asp:label></a></li>
                     </ul>                    
                    </asp:PlaceHolder>
                    
                    <asp:PlaceHolder ID="admin" runat="server">
                    <ul>
                        <li><a href="dov_stat.aspx" target="_self">Dovolenky - stav</a></li>
                        <li><a href="is_news.aspx" target="_self"><asp:Localize runat="server" ID="localize4" Text="Novinky"></asp:Localize></a></li>
                    </ul>
                    </asp:PlaceHolder>
                    
                    <asp:PlaceHolder ID="sestra" runat="server">
                    <div class="box info half-padded"> 
                        <asp:label ID="Label1" runat="server" Text="<%$ Resources:Resource,lmenu_sestry %>"></asp:label>
                    </div>
                    <ul>
                    <li><a href="sestrhlas.aspx" target="_self">Hlásenie sestier</a></li> 
                    </ul>                   
                    </asp:PlaceHolder>
                    
                   <div class="box info half-padded"> <h3><asp:label ID="Label4" runat="server" Text="<%$ Resources:Resource,lmenu_user %>"></asp:label></h3></div>
                    <ul>
                    <li><a href="adduser.aspx" target="_self">Užívateľ</a> </li>  
                    </ul> 
                    
                    <div class="box info half-padded"><h3><asp:label ID="Label5" runat="server" Text="<%$ Resources:Resource,lmenu_dfnsp %>"></asp:label></h3></div>
                    <div style="font-size:10px;text-align:left;"><asp:label ID="Label6" runat="server" Text="<%$ Resources:Resource,lmenu_dfnsp_note %>"></asp:label></div>
                    <ul>
                    <li><a href="http://10.10.2.83/gallery3" target="_blank">Galéria KDCH</a> </li>  
                     <li><a href="http://10.10.2.83/kdch" target="_blank">KDCH Intranet</a> </li>
                     <li><a href="http://10.10.2.49:3333" target="_blank">OSIRIX portál</a> </li> 
                    
                    </ul>
                     
                    <!--<li><a href="opkniha.aspx" target="_self">Operačná kniha</a> </li>!-->          
                    
                    
                </ul>
                
