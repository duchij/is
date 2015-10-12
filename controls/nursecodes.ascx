<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nursecodes.ascx.cs" Inherits="controls_nursecodes" %>
<div class="box green">
    <h2>Sesterské Diagnózy</h2>
    <hr />
    <div class="row">
        <div class="one half">
            <h3>Skupiny Diagnóz:</h3>
            <p>Po výbere skupiny sa Vám vedľa zobrazia jednotlivé diagnózy pre danú skupinu.</p>
            <asp:DropDownList ID="groups_dl" runat="server" OnSelectedIndexChanged="loadItemsFnc" AutoPostBack="true"></asp:DropDownList>
        </div>

        <div class="one half">
            <h3>Zoznam kódov:</h3>
            <p>Označením a Copy/Paste si môže diagnózu hocikam preniesť....</p>
           <asp:Table ID="code_list_tbl" runat="server" data-max="15" CssClass="responsive"></asp:Table>
        </div>
        
    </div>
</div>