<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_drg.aspx.cs" Inherits="is_drg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>
    <div class="row">
        <div class="blue box">
            <div class="row">
                <h3>Zadaj diagnozy pre  DRG Skupinu:</h3>
                <p class="red">Červeným sú označené povinné položky</p>
                <div class="one half">
                   
                    Drg Skupina:<asp:DropDownList runat="server" ID="drg_group_dl" width="200px"></asp:DropDownList>
                </div>
                <div class="one half">
                <span class="red">* Hlavna diagnoza:</span><asp:TextBox runat="server" ID="main_dg_txt" width="100px"></asp:TextBox>
                <span class="red">* Vedlajsia diagnoza:</span><asp:TextBox runat="server" ID="second_dg_txt"  width="100px"></asp:TextBox>
                    </div>
                <div class="one half">
                <span class="red">* Vykon DRG kodu:</span><asp:TextBox runat="server" ID="drg_code_txt"  width="100px"></asp:TextBox>
                Poznamka:<asp:TextBox runat="server" ID="note_txt"></asp:TextBox>
                </div>

                <div class="one half">
                Ostatne dg:<asp:TextBox runat="server" ID="other_dg_txt"></asp:TextBox>
                    <p class="small">Oddelit ciarkou</p>
                    </div>
                
                <p><asp:Button runat="server" ID="save_drg_row_btn" text="Ulozit" onClick="saveDataFnc" cssClass="button green" />
                </p>
                <hr />

            </div>
        </div>
        <div class="green box">
            <div class="row"><h3>Hladanie</h3>
                <div class="one third">
                    Vyber skupinu:<asp:DropDownList runat="server" id="drg_group_search_dl" cssClass="inline"></asp:DropDownList> 

                </div>
                <div class="one third">
                   
                    Diagnoza:<asp:TextBox runat="server" id="dg_search_txt" cssClass="inline"></asp:TextBox>
                <p class="small red">Nemusi byt cela staci cast</p>
                </div>

                <div class="one third">
                    <asp:Button runat="server" ID="search_btn" Text="Hladaj" cssClass="red button" onClick="searchDataFnc"/>
                </div>
            </div>
        </div>
        
        <hr />

        <div class="row">

            <asp:table runat="server" id="result_table_tbl"></asp:table>
        </div>
    </div>
</asp:Content>

