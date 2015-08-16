<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_lf.aspx.cs" Inherits="is_lf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Priložené súbory</h1>
    <hr />
    <asp:FileUpload ID="FileUpload1"  Width="400" Height="36" size="40px"  runat="server" />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

    <div class="row">
    
        <div class="one third">
            Folder: <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
        </div>

        <div class="one third">
            Vytvorit novy folder: <asp:TextBox ID="folder_name_txt" runat="server"></asp:TextBox><br />
            Popis foldera: <asp:TextBox id="folder_comment_txt" runat="server"></asp:TextBox>
            <asp:Button ID="create_folder_btn" runat="server" Text="Vytvor" OnClick="create_folder_fnc" />
        </div>

        <div class="one third vertical bottom"> 
            <asp:checkbox ID="see_me_chk" runat="server" Checked="false" /> Viditelnost pre mna <br />
            <asp:checkbox ID="see_all_chk" runat="server" Checked="true" /> pre vsetkych
        </div>
    
    </div>
    <asp:Button runat="server" ID="upload_btn" text="nahraj"/>
    <hr />
    <asp:TreeView ID="files_tv" runat="server" CssClass="responsive" data-max="15" ></asp:TreeView>
    
</asp:Content>


