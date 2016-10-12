<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_staze3.aspx.cs" Inherits="is_staze3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <input type="hidden" id="dypage" value="staze2" />
    <h1>Plán stáží...</h1>
    <asp:Literal runat="server" ID="msg_lbl" Text=""></asp:Literal>
    <hr />
    <asp:PlaceHolder ID="insertSection_plh" runat="server">
    <div class="row blue box">
        <div class="one third ">
            Vyučujúci:<asp:DropDownList ID="lecturer_dl" runat="server" EnableViewState="true"></asp:DropDownList>
        </div>

        <div class="one third">
            Ročník:<asp:DropDownList ID="yearclass_dl" runat="server" EnableViewState="true" ></asp:DropDownList>
        </div>

        <div class="one third">
            Skupina:<asp:TextBox ID="group_txt" runat="server"></asp:TextBox>(max 10)
        </div>
        
    </div>
    <div class="row green box">
        <div class="one fourth">
            Dátum: <asp:TextBox ID="classes_date_txt" runat="server" EnableViewState="true" ></asp:TextBox> (rrrr-mm-dd)
        </div>
       <div class="one fourth">
            Začiatok: <asp:TextBox ID="starttime_txt" runat="server" onchange="checkHourFormat('starttime_txt');" Text="08:00"></asp:TextBox> (hh:mm)
        </div>
        <div class="one fourth">
            Koniec:<asp:TextBox id="endtime_txt" runat="server" onchange="checkHourFormat('endtime_txt');" Text="13:00"></asp:TextBox> (hh:mm)

        </div>
        <div class="one fourth">
             Poznámka:<asp:TextBox ID="comment_txt" runat="server"></asp:TextBox> (max 100)
        </div>
        <asp:Button ID="saveData_btn" OnClick="saveData" runat="server" CssClass="button green" Text="<%$ Resources:Resource,save %>"/>
    </div>
        <div class="row"></div>
    </asp:PlaceHolder>
    <hr />
    <div class="row">
        <div class="one half"><asp:DropDownList ID="month_dl" runat="server" AutoPostBack="True"></asp:DropDownList></div>
        <div class="one half"><asp:DropDownList ID="year_dl" runat="server" AutoPostBack="True"></asp:DropDownList></div>

    </div> 
    <div class="row">
        <asp:PlaceHolder ID="listSection_plh" runat="server">
            <asp:table runat="server" ID="classes_tbl"></asp:table>

        </asp:PlaceHolder>
         </div>


</asp:Content>

