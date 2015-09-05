<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_lf.aspx.cs" Inherits="is_lf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Priložené súbory</h1>
    <hr />
    <asp:Label ID="msg_lbl" runat="server" Text="" CssClass="red"></asp:Label>
    <div class="tabs">
        <ul role="tablist">
            <li role="tab" aria-controls="#tab1">Nahratie súboru</li>
            <li role="tab" aria-controls="#tab2">Praca s adresármi</li>
            <li role="tab" aria-controls="#tab3">Praca so súbormi</li>
        </ul>
        <div id="tab1" role="tabpanel">
            <div class="row">
    
                <div class="one half">
                    <div class="box blue">
                    <h3 class="asphalt">Adresár:</h3>
                        <p>Vyber adresar kam nahrať súbor....</p>
                         <asp:DropDownList ID="folders_dl" runat="server" OnSelectedIndexChanged="changeFolderFnc" AutoPostBack="true" EnableViewState="true"></asp:DropDownList>
                </div>
            </div>
                <div class="one half">
                    <div class="box green">
                        <h3>Nahratie suboru...</h3>
                        <p><div class="red">max 64MB</div> Subor sa nahraje do aktualne vybraneho adresara...</p>
                     <asp:FileUpload  ID="lf_upf"  Width="400px" size="40px" runat="server" CssClass="box asphalt" />
                    
                    Vlastný názov/popis súboru: <asp:TextBox ID="file_user_name_txt" runat="server" Text=""></asp:TextBox>
                    <asp:Button runat="server" ID="upload_btn" text="Nahraj" CssClass="button medium asphalt" OnClick="uploadFileFnc" />
                        </div>
                </div>
                </div>
    <h1>Zoznam suborov v adresari - <strong><asp:Label runat="server" ID="actual_folder_lbl" Text="" ></asp:Label></strong></h1>
            <p><asp:Label runat="server" ID="folder_comment_lbl" Text=""></asp:Label></p> 
    <asp:GridView ID="lf_gv" runat="server" 
        CssClass="responsive" data-max="15"
        EnableModelValidation="True" 
        AutoGenerateColumns="False" OnRowCommand="grid_menu_fnc" OnRowDeleting="lf_gv_RowDeleting">
        <Columns>
            <asp:BoundField DataField="item_id" HeaderText="ID"  />
            <asp:BoundField DataField="item_name" HeaderText="Nazov" />
            <asp:BoundField DataField="item_comment" HeaderText="Poznamka" />
            <asp:CommandField DeleteText="Zmaz" SelectText="Zobraz/Stiahni...." ShowSelectButton="True" ButtonType="Button" ControlStyle-CssClass="button green" />
            <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-CssClass="button red"  />
        </Columns>
    </asp:GridView>
            </div>

        <div id="tab2" role="tabpanel">
           
            <div class="red box">
                Názov nového adresára: <asp:TextBox ID="folder_name_txt" runat="server"></asp:TextBox><br />
                Popis adresára: <asp:TextBox id="folder_comment_txt" runat="server"></asp:TextBox>
                
           
            <p>
                <asp:checkbox ID="see_me_chk" runat="server" Checked="false" /> Viditelnost pre mna <br />
                <asp:checkbox ID="see_all_chk" runat="server" Checked="true" /> pre vsetkych
                </p>
                <asp:Button ID="create_folder_btn" runat="server" Text="Vytvor" OnClick="create_folder_fnc" CssClass="button red" />
            </div>
           
       
        </div>

        <div id="tab3" role="tabpanel">
            nieco
        </div>
</div>
    <%--<asp:TreeView ID="files_tv" runat="server" CssClass="responsive" data-max="15" ></asp:TreeView>--%>
    
</asp:Content>


