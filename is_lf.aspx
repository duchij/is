<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_lf.aspx.cs" Inherits="is_lf" EnableEventValidation="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   <%-- <button id="test_btn" onclick="return false;">lalal</button>--%>
    <h1 class="blue">Priložené súbory</h1>
    <hr />

    <asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>
    <div id="lf_tabs">
        <asp:HiddenField ID="setlftab_hv" runat="server" Value="" />
        <ul>
            <li><a href="#lf_tab1">Nahratie súboru</a></li>
            <li><a href="#lf_tab2">Praca s adresármi</a></li>
            <li><a href="#lf_tab3">Praca so súbormi</a></li>
        </ul>
        <div id="lf_tab1">
            <div class="row">
   
                <div class="one half">
                    <div class="box blue">
                    <h3 class="asphalt">Adresár:</h3>
                        <p>Vyber adresár kam nahrať súbor....</p>
                         <asp:DropDownList ID="folders_dl" runat="server" OnSelectedIndexChanged="changeFolderFnc" AutoPostBack="true" EnableViewState="true"></asp:DropDownList>
                </div>
            </div>
                <div class="one half">
                    <div class="box green">
                        <h3>Nahratie suboru...</h3>
                        <p><div class="red">max 64MB</div> Súbor sa nahraje do aktuálne vybraného adresára...</p>
                     <asp:FileUpload  ID="lf_upf"  Width="400px" size="40px" runat="server" CssClass="box asphalt" />
                    
                    Vlastný názov/popis súboru: <asp:TextBox ID="file_user_name_txt" runat="server" Text=""></asp:TextBox>
                    <asp:Button runat="server" ID="upload_btn" text="Nahraj" CssClass="button medium asphalt" OnClick="uploadFileFnc" />
                        </div>
                </div>
                </div>
    <h1>Zoznam súborov v adresári - <strong><asp:Label runat="server" ID="actual_folder_lbl" Text="" CssClass="asphalt" ></asp:Label></strong></h1>
            <p><asp:Label runat="server" ID="folder_comment_lbl" Text=""></asp:Label></p> 
    <asp:table runat="server" ID="files_tbl"></asp:table>
            </div>

        <div id="lf_tab2">
           
            <div class="red box">
                Názov nového adresára: <asp:TextBox ID="folder_name_txt" runat="server"></asp:TextBox><br />
                Popis adresára: <asp:TextBox id="folder_comment_txt" runat="server"></asp:TextBox>
                
           
            <p>
                <asp:radiobutton ID="see_me_chk" runat="server" GroupName="prava" />Viditeľný pre mňa..  <br />
                <asp:radiobutton ID="see_all_chk" runat="server" GroupName="prava" Checked="true"/> Viditeľný pre všetkých
                </p>
                <asp:Button ID="create_folder_btn" runat="server" Text="Vytvor" OnClick="create_folder_fnc" CssClass="button red" />
                <hr />
                <h2>Mazanie adresára</h2>
                Vymaž adresár: <asp:DropDownList ID="del_folders_dl" runat="server" OnSelectedIndexChanged="changeFolderFnc" EnableViewState="true"></asp:DropDownList>
                <asp:Button ID="delete_folder_btn" runat="server" Text="<%$ Resources:Resource,delete %>" CssClass="button medium yellow" onClick="deleteFolderFnc" OnClientClick="return confirm('Naozaj chcete zmazať? Pozor súbory sa presunú ho hlavného adresára!!!!');"/>
                <hr />
                <h2>Nastavenie viditeľnosti</h2>
                <asp:radiobutton ID="edit_visibility_me" runat="server" GroupName="editPrava" OnCheckedChanged="setVisibilityFnc" AutoPostBack="true" />Viditeľný pre mnňa..  <br />
                <asp:radiobutton ID="edit_visibility_all" runat="server" GroupName="editPrava" OnCheckedChanged="setVisibilityFnc" Checked="true" AutoPostBack="true"/> Viditeľný pre všetkých
            </div>
           
       
        </div>

        <div id="lf_tab3">
            Pripravuje sa....
        </div>
</div>
    <%--<asp:TreeView ID="files_tv" runat="server" CssClass="responsive" data-max="15" ></asp:TreeView>--%>
    
</asp:Content>


