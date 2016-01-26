<%@ Control Language="C#" AutoEventWireup="true" CodeFile="nkim_shifts.ascx.cs" Inherits="controls_nkim_shifts" %>
<asp:Literal ID="msg_lbl" runat="server" Text=""></asp:Literal>
    <h2> Plán služieb</h2><hr />
<div id="msg_dialog"></div>
    <asp:PlaceHolder ID="editShiftView_pl" runat="server">
        <div class="row">
            <div class="one half">
                <div class="blue box"">
                    <asp:CheckBox ID="editShift_chk" runat="server" AutoPostBack="true" Checked="false" EnableViewState="true" />
                

                    <asp:Label ID="Label3" runat="server" Text=" Editácia služieb" CssClass="mojInline" AssociatedControlID="editShift_chk"></asp:Label>
                    </div>
                </div>
            <div class="one half">
        
                </div>
        <hr />

        </div>
                
            </asp:PlaceHolder>
        <div class="row">
            
            
            <div class="one third">
             <div style="display:none;">Oddelenie: </div>         
                <asp:DropDownList ID="deps_dl" runat="server" OnSelectedIndexChanged="changeDeps_fnc" EnableViewState="true" AutoPostBack="true" Visible="false"></asp:DropDownList>

            </div>
            <div class="one third">
                 Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" 
                        AutoPostBack="True" onselectedindexchanged="changeSluzba" EnableViewState="true" >
                        
                    </asp:DropDownList> 
                    </div>
                    <div class="one third">
                    Rok: 
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="changeSluzba" EnableViewState="true" >
                    
                </asp:DropDownList>
                </div> 
            </div>
    
 Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text="Label"></asp:Label><hr />
                <asp:Label ID="Label1" runat="server" Text="Víkend" CssClass="red box" Width="130"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="Štátny sviatok" CssClass="yellow box" Width="130"></asp:Label>
    <asp:Label ID="shiftState_lbl" runat="server" Text=""></asp:Label>  
    <div class="row">
   <%-- <asp:Button ID="Button1" runat="server" Text="Plán služieb" OnClick="generate_nurse_plan_fnc" CssClass="button green large"/>
    <p><small>Po kliknutí sa Vám zobrazí Váš klasický plán služieb vo formáte pdf a môžete si ho vytlačiť...</small></p>--%>
        </div>          
    <asp:Table ID="shiftTable" EnableViewState="false" runat="server" CssClass="responsive" data-max="14">                 
                </asp:Table>
                <div class="row">
                    <div class="one whole padded">
                    <asp:Button ID="publish_btn" runat="server" Text="Sprístupniť všetkým" CssClass="green button" OnClick="publishOnFnc"  />
                     <asp:Button ID="unpublish_btn" runat="server" Text="Zblokovať prístup všetkým" CssClass="red button"  OnClick="publishOffFnc"/>
                        </div>
                    <asp:Button ID="toWord_btn" runat="server" Text="do Wordu" OnClick="publishSluzby"/>
                
                    <asp:Button ID="print_btn" runat="server"  Text="Tlačiť" OnClick="publishSluzby" />
                    
                </div>