<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_gallery.aspx.cs" Inherits="is_gallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Literal runat="server" ID="msg_lit"></asp:Literal>
<h1>Kamera gallery</h1>
    <p class="blue">
        V mobilných zariadeniach (smartfone, tablet) máte dostupné nové tlačidlo - <a href="utils/camera.html" target="_self" class="button green"><i class="icon-camera"></i>Kamera</a>.<br /> Po stlačení budete presmerovaní, do malej aplikácie , ktorá Vám umožní
        zadať dáta o pacientovi, jeho diagnózu a následne môžete pomocou mobilného zariadenia odfotiť operačný nález, ev iné ktorý sa následne uloží do systému.<br />
        <strong>Meno pacienta, dátum a diagnóza</strong> sú povinné, a to preto aby ste mohli následne danú vec vyhľadávať....<br />
        Na tomto mieste nájdete následne posledných 10 odfotených snímok, ale máte možnosť aj vyhľadávania...
        <strong>Pozor !!!!</strong> Ak nejdete cez WiFi, tak sa Vám používajú dáta z Vášho mobilného paušálu......
    </p>
    <hr />
    <table class="responsive" data-max="15">
        <tr>
            <td>Hľadaj podľa:</td>
            <td>
                    <asp:DropDownList runat="server" ID="searchBy" EnableViewState="true">
                    <asp:ListItem Text="Mena" Value="ten">-</asp:ListItem>
                    <asp:ListItem Text="Mena" Value="name"></asp:ListItem>
                    <asp:ListItem Text="Rodné číslo" Value="bin_num"></asp:ListItem>
                    <asp:ListItem Text="Diagnóza" Value="diagnose"></asp:ListItem>
                   
                </asp:DropDownList>
 <!-- <asp:ListItem Text="Dátum (rrrr-mm-dd)" Value="photoDate"></asp:ListItem> -->
            </td>

            <td>
                <asp:TextBox runat="server" ID="queryString" Text="" EnableViewState="true"></asp:TextBox>

            </td>
            <td><asp:Button runat="server" ID="searchExec_btn"  CssClass="green button" Text="Hladaj" OnClick="searchGalleryFnc" EnableViewState="true"/></td>


        </tr>

    </table>
     
    
   
    <hr />
    <h3><asp:Label runat="server" ID="searchTitle_lbl" Text="Posledných 10 záznamov"></asp:Label></h3>
    <asp:table runat="server" ID="files_tbl" CssClass="responsive" data-max="12"></asp:table>
    

</asp:Content>

