<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_seminar.aspx.cs" Inherits="is_seminar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Semináre</h1>
    <asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>
    <asp:HiddenField ID="isSeminarsPage" runat="server" Value="1" />
    <asp:PlaceHolder ID="editSection" runat="server">

         <div class="blue box"><!--Zaciatok modreho boxu--> 
             <h2 class="black">Pridanie seminára</h2>
        <div class="row">
               
                <div class="one fourth">
                    Dátum:<asp:TextBox ID="date_txt" runat="server" Text="" Width="100" CssClass="mojInline"></asp:TextBox>
                </div>
                <div class="one fourth">
                    Lekár:<asp:DropDownList ID="doctors_dl" runat="server" Width="100"  CssClass="mojInline"></asp:DropDownList>
                </div>
                <div class="one fourth">
                    Názov:<asp:TextBox runat="server" ID="tema_txt" Text="" Width="150" CssClass="mojInline"></asp:TextBox>
                    
                </div>
                <div class="one fourth">
                    <asp:Button runat="server" ID="save_button_btn" CssClass="green button" Text="<%$ Resources:Resource,save %>" OnClick="saveDataFnc" />
                </div>
               

            </div>
              </div> <!--koniec modreho boxu-->
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="showSection" runat="server">
        <hr />
        Vyber mesiac a rok:<asp:DropDownList ID="month_dl" runat="server" CssClass="mojInline" Width="100" OnSelectedIndexChanged="refreshPageFnc" AutoPostBack="true" EnableViewState="true"></asp:DropDownList>
        <asp:DropDownList ID="year_dl" runat="server" Width="100" OnSelectedIndexChanged="refreshPageFnc" AutoPostBack="true" CssClass="mojInline" EnableViewState="true"></asp:DropDownList>
        <asp:Literal runat="server" ID="select_date_lit" Text=""></asp:Literal> 
        <h2 class="red">Všetky semináre pre vybraný mesiac</h2>
        <hr /> 
        <asp:Table ID="seminars_tbl" runat="server">

        </asp:Table>

    </asp:PlaceHolder>
</asp:Content>

