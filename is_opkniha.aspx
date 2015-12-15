<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_opkniha.aspx.cs" Inherits="is_opkniha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <h1> Hľadanie v operačnej knihe</h1>
    <asp:HiddenField ID="opknihaTab_hv" runat="server" Value="" />
    <asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>
    <div class="dismissible info message">
    <p class="asphalt">Prinášam Vám možnosť hľadania v operačných knihách tak ako sú vedené od roku 2000. Hľadanie v nich je kostrbaté, pretože databáza nebola
        navrhnutá tak aby kontrolova vstupy, čo sa do nej zadávajú, preto je tam raz <q>Fraktura a raz fractura</q>. Takže zatiaľ máte spustené hľadanie 
        podľa diagnóz, alebo podľa výkonov. Hľadanie je jednoduché chcem najsť napr. operácie appendixov, tak skúsim zadať <q>append apend</q> a následne len
        roky od kedy do kedy. Jednotlivé časti slov oddeľujte medzerou, inak do bude brať ako jedno slovo... Export je možný aj do excel tabuľky, na to slúži dlačidlo 
        do Excelu, pred otvorením súboru Vás Excel upozorní, že sa nejedná oficiálny excel, stlačte áno a hotovo.
            
    </p>
        <p class="asphalt"> Vzhľadom na to, že dáta uložené v tejto databáze sú chaotické musíte vyskúšať viacero možností hľadania. Časom pribudne aj trošku komplikovanejšie
            hľadanie pre náročných...
        </p>
        <p class="asphalt">Toto  môžete využiť aj pre galériu obrázkov, keď viete dátum operácie. Stačí ísť na <a href="http://10.10.2.83/gallery3" target="_blank">Galériu</a>
            Prihlásiť sa tam (meno a heslo Vám poskytnem) a skúsiť vyhľadať podľa dátumu operácie, či tam nie sú nejaké obrázky. Pozor, galéria funguje len vrámci DFNSP. Keď budem, mať čas
            tak Vám to sem nejak vyspájam...
        </p>
         </div>
    <hr /> Roky 2000-2014 spolu -<strong> <asp:Label ID="row_counts" runat="server" Text=""></asp:Label> </strong>Operácii
    <hr />
     <div class="row">
    <div id="opkniha_tabs">
   
        <ul>
            <li><a href="#opkniha_tab1">podľa Diagnóz</a></li>
            <li><a href="#opkniha_tab2">podľa Výkonov</a></li>
            <li><a href="#opkniha_tab3">Moje výkony</a></li>
        </ul>
        <div id="opkniha_tab1">
        <h3 class="blue"> Hľadanie v diagnózach</h3>
        <p class="small">Umožňuje, hľadanie v diagnózach, tieto sú uložené slovne, nie podľa MKCH 10, a nie vždy je dodržaný správny pravopis, preto môžete zadať viacero slov, oddelené len <b>medzerou.</b> <br />Zdávajte len spoločné základy slov. Napr. appen apen<br />
        Ak chcete veci len zobraziť stlačte len <b>Hľadaj</b>, v prípade exportu do excelu je tlačidlo <b>do Excelu</b> (musíte po prepnutí sa do excelu súhlásiť, že chcete daný súbor otvoriť) </p>
        
            <div class="one fourth">
        <b>Hľadaj v diagnózach: </b><asp:TextBox ID="queryDg_txt" runat="server" EnableViewState="true"></asp:TextBox>
            </div>
        <div class="one fourth">
            <strong>Od roku:</strong>  <asp:TextBox ID="fromYear_txt" runat="server" ></asp:TextBox>
        </div>
        <div class="one fourth">
            <strong>Do roku:</strong> <asp:TextBox ID="toYear_txt" runat="server"></asp:TextBox>
        </div>
        <div class="one fourth">
            <br />
            <asp:Button ID="search_dg_btn" runat="server" Text="<%$ Resources:Resource,search %>" CssClass="blue button" OnClick="searchInDgFnc" />
             <asp:Button ID="to_excel_btn" runat="server" Text="Do excelu" CssClass="green button" OnClick="searchToExcelFnc" />
       </div>
            </div> <!-- koniec tabu1 -->
        <div id="opkniha_tab2"> <!--zaciatok tabu2 -->
            <h3 class="blue"> Hľadanie vo výkonoch</h3>
        <p class="small">Umožňuje, hľadanie v operačných výkonoch, tieto sú uložené slovne, a nie vždy je dodržaný správny pravopis, preto môžete zadať viacero slov, oddelené len <b> medzerou.</b> <br />Zdávajte len spoločné základy slov. Napr. LSK apend append<br />
        Ak chcete veci len zobraziť stlačte len <b>Hľadaj</b>, v prípade exportu do excelu je tlačidlo <b>do Excelu</b> (musíte po prepnutí sa do excelu súhlásiť, že chcete daný súbor otvoriť) </p>
        
            <div class="one fourth">
       <b> Hľadaj v výkonoch:</b> <asp:TextBox ID="queryOp_txt" runat="server" EnableViewState="true"></asp:TextBox>
            </div>
        <div class="one fourth">
            <strong>Od roku:</strong>  <asp:TextBox ID="fromYearOP_txt" runat="server" EnableViewState="true" ></asp:TextBox>
        </div>
        <div class="one fourth">
            <strong>Do roku:</strong> <asp:TextBox ID="toYearOP_txt" runat="server" EnableViewState="true"></asp:TextBox>
        </div>
        <div class="one fourth">
             <br />
            <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Resource,search %>" CssClass="blue button" OnClick="searchInOPFnc" />
             <asp:Button ID="Button2" runat="server" Text="Do excelu" CssClass="green button" OnClick="searchOPToExcelFnc" />
       </div>
        </div> <!--zaciatok tabu3 -->
        <div class="row">
            <div id="opkniha_tab3"> <!--zaciatok tabu3 -->
                    <h3 class="asphalt"> Výpis mojich výkonov </h3>
                    <div class="one fourth">Meno:<asp:textbox id="menoMyOP_txt" runat="server"></asp:textbox></div>
                    <div class="one fourth">Od roku:<asp:textbox id="fromYearMyOP_txt" runat="server"></asp:textbox></div>
                    <div class="one fourth">Do roku:<asp:textbox id="toYearMyOP_txt" runat="server"></asp:textbox></div>
                    <div class="one fourth">
                        <asp:Button ID="Button3" runat="server" Text="<%$ Resources:Resource,search %>" CssClass="blue button" OnClick="searchInMyOPFnc" />
             <asp:Button ID="Button4" runat="server" Text="Do excelu" CssClass="green button" OnClick="searchInMyExcelOPFnc" /></div>
            
            </div>
        </div><!--koniec tabu3 -->


         </div>
    </div>
        <%-- <p>Hľadaj podľa časti slova/slov (oddelene medzerami) vo vykonoch: <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><%--<asp:Button ID="search_op_btn" runat="server" Text="<%$ Resources:Resource,search %>" OnClick="searchInOpFnc" />--%>
     <%--   <p>Hľadaj podľa štatistikého kódu operačiek:<asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList><asp:Button ID="Button3" runat="server" Text="<%$ Resources:Resource,search %>" /></p>
        <p>Hľadaj podľa operatér/asistent: <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><asp:Button ID="Button4" runat="server" Text="<%$ Resources:Resource,search %>" /></p>--%>
   
    <hr />
    <%--<asp:GridView ID="GridView1" runat="server" ></asp:GridView>--%>
    <asp:Label ID="foundRows_lbl" Text="" runat="server"></asp:Label>
    <asp:Table ID="result_tbl" runat="server" CssClass="responsive" data-max="15"></asp:Table>
</asp:Content>

