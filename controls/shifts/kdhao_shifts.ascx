<%@ Control Language="C#" AutoEventWireup="true" CodeFile="kdhao_shifts.ascx.cs" Inherits="controls_shifts_kdhao_shifts"  %>
<asp:Label runat="server" ID="msg_lbl" Text=""></asp:Label>
<div class="row">
            
            <div class="one half"> 
                 Mesiac:<asp:DropDownList ID="mesiac_cb" runat="server" 
                        AutoPostBack="True" onselectedindexchanged="changeSluzba" EnableViewState="true">
                        <asp:ListItem Value="1">Január</asp:ListItem>
                        <asp:ListItem Value="2">Február</asp:ListItem>
                        <asp:ListItem Value="3">Marec</asp:ListItem>
                        <asp:ListItem Value="4">Apríl</asp:ListItem>
                        <asp:ListItem Value="5">Máj</asp:ListItem>
                        <asp:ListItem Value="6">Jún</asp:ListItem>
                        <asp:ListItem Value="7">Júl</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>   
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">Október</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList> 
                    </div>
                    <div class="one half">
                    Rok: 
                <asp:DropDownList ID="rok_cb" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="changeSluzba" EnableViewState="true" >
                    <asp:ListItem Value="2010">Rok 2010</asp:ListItem>
                    <asp:ListItem Value="2011">Rok 2011</asp:ListItem>
                    <asp:ListItem Value="2012">Rok 2012</asp:ListItem>
                    <asp:ListItem Value="2013">Rok 2013</asp:ListItem>
					<asp:ListItem Value="2014">Rok 2014</asp:ListItem>
					<asp:ListItem Value="2015">Rok 2015</asp:ListItem>
					<asp:ListItem Value="2016">Rok 2016</asp:ListItem>
                </asp:DropDownList>
                </div> 
</div>

<div class="info box">
    <asp:CheckBox ID="edit_chk" runat="server" Text="" AutoPostBack="true" EnableViewState="true" />&nbsp; <asp:Label ID="editChk_lbl" runat="server" Text=""></asp:Label>
</div>
Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text=""></asp:Label><hr />
<asp:Label ID="Label1" runat="server" Text="Víkend" CssClass="red box" Width="130"></asp:Label>
<asp:Label ID="Label2" runat="server" Text="Štátny sviatok" CssClass="yellow box" Width="130"></asp:Label>
<asp:Label ID="shiftState_lbl" runat="server" Text=""></asp:Label>
<asp:Table ID="shiftTable" runat="server" EnableViewState="false" CssClass="responsive" data-max="14"></asp:Table>
<div class="row">
        <div class="one whole padded">

            <asp:PlaceHolder ID="kdch_pl" runat="server" Visible="false">
                <asp:Button ID="publish_btn" runat="server" Text="Sprístupniť všetkým" CssClass="green button" OnClick="publishOnFnc"  />
                <asp:Button ID="unpublish_btn" runat="server" Text="Zblokovať prístup všetkým" CssClass="red button"  OnClick="publishOffFnc"/>
                            
                            
            </asp:PlaceHolder>
                      

                        <%--<asp:Label ID="publish_lbl" runat="server" Text="Uvernejniť" CssClass="mojInline"></asp:Label>--%>
        </div>
                    <asp:Button ID="toWord_btn" runat="server" Text="do Wordu" OnClick="publishSluzby"/>
                
                    <asp:Button ID="print_btn" runat="server"  Text="Tlačiť" OnClick="publishSluzby" />
</div>
