<%@ Control Language="C#" AutoEventWireup="true" CodeFile="left_menu.ascx.cs" Inherits="left_menu"  %>
<%--<%@ Reference Control="~/MasterPage.master" %>--%>
 <asp:PlaceHolder ID="dev_pl" runat="server" Visible="false">
    Vyvojar
        <ul>
              <li><asp:HyperLink ID="call_sp" runat="server" NavigateUrl="~/helpers/callsp.aspx">Call Stored Procedure</asp:HyperLink></li>
              <li><asp:HyperLink ID="call_consluz" runat="server" NavigateUrl="~/helpers/convsluz.aspx">Konvertor sluzieb</asp:HyperLink></li> 
             <li><asp:HyperLink ID="labels" runat="server" NavigateUrl="labels.aspx">Labels</asp:HyperLink></li> 
            <li><asp:HyperLink ID="update" runat="server" NavigateUrl="~/helpers/update.aspx">UPDATE</asp:HyperLink></li> 
            <li><asp:Button ID="offline_btn" runat="server" Text="Set Web Offline" CssClass="button red" OnClick="setState" /></li>
        <li><asp:Button ID="online_btn" runat="server" Text="Set Web Online" CssClass="button green"  OnClick="setState"/></li>

             <li><asp:HyperLink ID="newUserVykaz" runat="server" NavigateUrl="is_user_vyk.aspx">!!!! newUserVyk</asp:HyperLink></li>

        </ul>
  </asp:PlaceHolder>

<asp:PlaceHolder ID="sadmin_menu" runat="server" Visible="false">
    <div class="alert box">
    <h3>SubAdmin</h3>
        </div>
    <ul>
        <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="labels.aspx">Labels</asp:HyperLink></li>
    </ul>

</asp:PlaceHolder>

 <asp:PlaceHolder ID="operacky" runat="server"  Visible="false">
        <h3>Operacky</h3>
        <ul>
            <li class="box orange"><a href="opprogram.aspx" target="_self">Operacny program</a></li>
            <li><a href="sluzby2.aspx" target="_self"><asp:Localize runat="server" ID="localize1" Text="<%$ Resources:Resource,odd_akt_sluz %>"></asp:Localize></a></li>
        </ul>
  </asp:PlaceHolder>     
    
 <%--<div class="box info half-padded">
   <h3><asp:label ID="Label2" runat="server" Text="<%$ Resources:Resource,lmenu_rozpis %>"></asp:label></h3></div>
        <ul>
                    
            <li><a href="sluzby2.aspx" target="_self"><asp:Localize runat="server" ID="localize1" Text="<%$ Resources:Resource,odd_akt_sluz %>"></asp:Localize></a></li>
            <li><a href="staze.aspx" target="_self"><asp:Localize runat="server" ID="localize2" Text="<%$ Resources:Resource,odd_staze %>"></asp:Localize></a></li> 
            <li><a href="ransed.aspx" target="_self"><asp:Localize runat="server" ID="localize3" Text="<%$ Resources:Resource,ranne_sed %>"></asp:Localize></a></li>
         </ul> --%>  
       
                        
  <asp:PlaceHolder ID="doctors" runat="server"  Visible="false"> 
     <div class="box info half-padded"> 
         <h3><asp:label ID="Label3" runat="server" Text="<%$ Resources:Resource,lmenu_oddelenie %>"></asp:label></h3>
     </div>
     <ul>               
        <li><a href="hlasko.aspx" target="_self">Hlásenie služieb</a></li>
         <li><a href="tabletview.aspx" target="_self">RDG - Tablet/Mobil</a></li>
        <li><a href="vykaz2.aspx" target="_self">Mesačný výkaz</a></li>
        <li><a href="dovolenky.aspx" target="_self">Dovolenky, Voľná</a></li> 
        <li><a href="poziadavky.aspx" target="_self"><asp:label ID="poziadavky_lbl" runat="server" Text="<%$ Resources:Resource,odd_poziadavky %>"></asp:label></a></li>
     </ul>
     <br />
     <ul>
        <li><a href="sluzby2.aspx" target="_self"><asp:Localize runat="server" ID="localize5" Text="<%$ Resources:Resource,odd_akt_sluz %>"></asp:Localize></a></li>
        <li><a href="staze.aspx" target="_self"><asp:Localize runat="server" ID="localize6" Text="<%$ Resources:Resource,odd_staze %>"></asp:Localize></a></li> 
        <li><a href="ransed.aspx" target="_self"><asp:Localize runat="server" ID="localize7" Text="<%$ Resources:Resource,ranne_sed %>"></asp:Localize></a></li>
     <li><a href="is_ohv.aspx" target="_self">OHV Kódy</a></li>
     </ul>
  </asp:PlaceHolder>
                    
  <asp:PlaceHolder ID="admin" runat="server"  Visible="false">
    <ul>
        <%--<li><a href="dov_stat.aspx" target="_self">Dovolenky - stav</a></li>--%>
        <li><a href="is_news.aspx" target="_self"><asp:Localize runat="server" ID="localize4" Text="Novinky"></asp:Localize></a></li>
    </ul>
   </asp:PlaceHolder>
                    
   <asp:PlaceHolder ID="sestra" runat="server"  Visible="false">
    <div class="box info half-padded"> 
        <asp:label ID="Label1" runat="server" Text="<%$ Resources:Resource,lmenu_sestry %>"></asp:label>
    </div>
    <ul>
        <li><a href="sestrhlas.aspx" target="_self">Hlásenie sestier</a></li>
        <li><a href="sluzby2_sestr.aspx" target="_top">Služby</a></li>
        <li><a href="sluzby2.aspx" target="_self">Služby lekári<%--<asp:Localize runat="server" ID="localize2" Text="<%$ Resources:Resource,odd_akt_sluz %>"></asp:Localize>--%></a></li>

        <li>Mesačný výkaz</li> 
    </ul>                   
   </asp:PlaceHolder>
                    
    <div class="box info half-padded"> <h3><asp:label ID="Label4" runat="server" Text="<%$ Resources:Resource,lmenu_user %>"></asp:label></h3></div>
    <ul>
        <li><a href="is_user.aspx" target="_self">Užívateľ</a> </li> 
        <li><a href="is_user_vyk.aspx" target="_self">Užívateľský výkaz</a> </li> 
    </ul> 
                    
    <div class="box info half-padded"><h3><asp:label ID="Label5" runat="server" Text="<%$ Resources:Resource,lmenu_dfnsp %>"></asp:label></h3></div>
    <div style="font-size:10px;text-align:left;"><asp:label ID="Label6" runat="server" Text="<%$ Resources:Resource,lmenu_dfnsp_note %>"></asp:label></div>
    <ul>
        <li><a href="http://10.10.2.83/gallery3" target="_blank">Galéria KDCH</a> </li>  
        <li><a href="http://10.10.2.83/kdch" target="_blank">KDCH Intranet-Navody</a> </li>
        <li><a href="http://10.10.2.49:3333" target="_blank">OSIRIX portál</a> </li> 
        
    </ul>
                     
<!--<li><a href="opkniha.aspx" target="_self">Operačná kniha</a> </li>!-->          
                    
                    
                
                
