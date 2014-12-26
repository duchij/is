<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header.ascx.cs" Inherits="header" EnableViewState="true" %>
Dnes slúži:
<div class="row">
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
    <div class="one fifth">
        <div class="box red">
            <strong>OUP: </strong>
            <asp:Label ID="oup_lbl" runat="server" Text="..." ></asp:Label>
            </div>
    </div>
    <div class="one fifth">
            <div class="box blue">
                <strong>Odd A:</strong> 
                <asp:Label ID="odda_lbl" runat="server" Text="..." ></asp:Label>
            </div>
    </div>
    <div class="one fifth">
        <div class="box green">
            <strong>Odd B: </strong>
            <asp:Label ID="oddb_lbl" runat="server" Text="..."></asp:Label>
            </div>
    </div>
    <div class="one fifth">
        <div class="box asphalt">
        <strong>Op.pohot.: </strong>
        <asp:Label ID="op_lbl" runat="server" Text="..."></asp:Label>
        </div>
    </div>
    <div class="one fifth">
     <div class="box purple">
        <strong>Prijm.amb.:</strong> 
        <asp:Label ID="trp_lbl" runat="server" Text="..." ></asp:Label>
        </div>
    </div>
</div>
<div class="row">
    <div class="three fifths half-padded">
        Dnes je poslužbe:     
           <strong> <asp:Label ID="po_lbl" runat="server" Text="..." ForeColor="green"></asp:Label></strong>
    </div>
    <div class="two fifths half-padded">
        Dnes je:
         <strong><asp:Label ID="date_lbl" runat="server" Text="..."></asp:Label></strong><br />
        
       <asp:Label ID="lock_info_lbl" runat="server" Text="<%$ Resources:Resource, odd_poziadavky_info_short %>">
       </asp:Label>
       <strong>
                    <asp:Label ID="poziadav_lbl" runat="server" Text=""></asp:Label></strong>
        
    </div>

</div>


      
    