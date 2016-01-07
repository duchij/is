<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_staze2.aspx.cs" Inherits="is_staze2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <input type="hidden" id="dypage" value="staze2" />
    <div class="row">
        <div class="one fifth">
            Vyucujuci:<asp:DropDownList ID="lecturer_dl" runat="server"></asp:DropDownList>
        </div>

        <div class="one fifth">
            Skupina:<asp:DropDownList ID="group_dl" runat="server"></asp:DropDownList>
        </div>
        
        
       <div class="one fifth">
            Zaciatok: <asp:TextBox ID="starttime_txt" runat="server"></asp:TextBox> (hh:mm)
        </div>
        <div class="one fifth">
            Koniec:<asp:TextBox id="endtime_txt" runat="server"></asp:TextBox> (hh:mm)

        </div>
        <div class="one fifth">
             Poznamka:<asp:TextBox ID="comment_txt" runat="server"></asp:TextBox> (max 100)
        </div>
       


    </div>


</asp:Content>

