<%@ Control Language="C#" AutoEventWireup="true" CodeFile="dk_shifts.ascx.cs" Inherits="controls_shifts_dk_shifts"  %>
<asp:Label runat="server" ID="msg_lbl" Text=""></asp:Label>

<asp:HiddenField ID="tempWeek_0" runat="server" />
    <asp:HiddenField ID="tempWeek_1" runat="server" />
    <asp:HiddenField ID="tempWeek_2" runat="server" />
    <asp:HiddenField ID="tempWeek_3" runat="server" />
    <asp:HiddenField ID="tempWeek_4" runat="server" />
    <asp:HiddenField ID="tempWeek_5" runat="server" />

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
<asp:PlaceHolder ID="druhadk_pl" runat="server" Visible ="false">
          <asp:Table ID="weekState_tbl" runat="server" CssClass="responsive" data-max="14"></asp:Table>
                            <asp:Button ID="setup_btn" runat="server" Text="" OnClick="makeShiftsDraftDKFnc" CssClass="button green" />
                            <asp:Button ID="avaible_btn" runat="server" Text="" OnClick="makeShiftsActiveDKFnc"  CssClass="button blue"/>
                            
                       
           </asp:PlaceHolder>

<div class="info box">
    <asp:CheckBox ID="edit_chk" runat="server" Text="" AutoPostBack="true" EnableViewState="false" />&nbsp; <asp:Label ID="editChk_lbl" runat="server" Text=""></asp:Label>
</div>
Počet dní v mesiaci: <asp:Label ID="days_lbl" runat="server" Text=""></asp:Label><hr />
<asp:Label ID="Label1" runat="server" Text="Víkend" CssClass="red box" Width="130"></asp:Label>
<asp:Label ID="Label2" runat="server" Text="Štátny sviatok" CssClass="yellow box" Width="130"></asp:Label>
<asp:Label ID="shiftState_lbl" runat="server" Text=""></asp:Label>
<asp:Table ID="shiftTable" runat="server" EnableViewState="false" CssClass="responsive" data-max="14"></asp:Table>
<div class="row">
        
                    <asp:Button ID="toWord_btn" runat="server" Text="do Wordu" OnClick="publishSluzby"/>
                
                    <asp:Button ID="print_btn" runat="server"  Text="Tlačiť" OnClick="publishSluzby" />
</div>
