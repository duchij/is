<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="adduser.aspx.cs" Inherits="adduser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="msg_lbl" runat="server" Text="Label"></asp:Label>
<h2>Informácie o užívateľovi:</h2><hr />
                    <asp:PlaceHolder ID="adminsectionPlace" runat="server">
                        <asp:TextBox ID="search_txt" runat="server"></asp:TextBox>
                        <asp:Button ID="search_btn" runat="server" Text="Hľadaj" OnClick="searchByNameFnc" />
                    
                     <asp:GridView ID="users_gv" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                        GridLines="None" onpageindexchanging="users_gv_PageIndexChanging" 
                        onselectedindexchanging="users_gv_SelectedIndexChanging" Width="100%" 
                        onrowdeleting="users_gv_RowDeleting">
                         <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                         <Columns>
                             <asp:CommandField ShowSelectButton="True" />
                             <asp:BoundField DataField="id" HeaderText="id">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField DataField="full_name" HeaderText="Cele meno">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField DataField="name" HeaderText="login">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField ConvertEmptyStringToNull="False" DataField="group" 
                                 HeaderText="Skupina">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:CommandField ShowDeleteButton="True" />
                         </Columns>
                         <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                         <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                         <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                         <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                         <AlternatingRowStyle BackColor="White" />
                    
                    </asp:GridView>
                    
                    <table>
                     <tr>
                        <td>Heslo:</td>
                        <td><asp:TextBox ID="passwd_txt" runat="server" Width="300px"></asp:TextBox></td>
                    
                    </tr>
                     <tr>
                        <td>Aktivny:</td>
                        <td><asp:TextBox ID="active_txt" runat="server" Width="30px"></asp:TextBox></td>
                    
                    </tr>
                    
                    </table>
                
               </asp:PlaceHolder>
                    
                    <table>
                    <tr>
                    <td>Titul pred menom:</td>
                    <td>
                        <asp:TextBox ID="titul_pred" runat="server"></asp:TextBox></td>
                    </tr>
                                       
                    <tr>
                        <td>Meno a priezvisko:</td>
                        <td><asp:TextBox ID="name_txt" runat="server"></asp:TextBox></td>
                    
                    </tr>
                    <tr>
                    <td>Titul za menom:</td>
                    <td>
                        <asp:TextBox ID="titul_za" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Login:</td>
                        <td><asp:TextBox ID="login_txt" runat="server"></asp:TextBox></td> 
                    </tr>
                    <tr>
                        <td>E-mail:</td>
                        <td><asp:TextBox ID="email_txt" runat="server"></asp:TextBox></td>
                    
                    </tr>
                    
                     <tr>
                        <td>Pracovné zaradenie:</td>
                        <td><asp:TextBox ID="zaradenie_txt" runat="server"></asp:TextBox></td>
                    
                    </tr>
                    <tr>
                        <td>Pracovná doba:</td>
                        <td><asp:TextBox ID="pracdoba_txt" runat="server" Text=""></asp:TextBox></td>
                    
                    </tr>
                    
                     <tr>
                        <td>Týždenná pracovná doba:</td>
                        <td><asp:TextBox ID="tyzdoba_txt" runat="server" Text=""></asp:TextBox></td>
                    
                    </tr>
                    
                     <tr>
                        <td>Osobné číslo:</td>
                        <td><asp:TextBox ID="osobcisl_txt" runat="server" Text=""></asp:TextBox></td>
                    
                    </tr>
                    
                    
                    
                    <tr>
                        <td>
                            <asp:Label ID="rights_lbl" runat="server" Text="Práva"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="rights_cb" runat="server">
                                <asp:ListItem Value="poweruser">PowerUser</asp:ListItem>
                                <asp:ListItem Value="admin">Admin</asp:ListItem>
                                <asp:ListItem Value="users">Users</asp:ListItem>
                                <asp:ListItem Value="sestra">Sestra</asp:ListItem>
                                <asp:ListItem Value="sestra_vd">Sestra_vd</asp:ListItem>
                                <asp:ListItem Value="medix">medix</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    
                    </tr>
                    
                    
                    
                    </table>
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource,adduser_relogin %>" 
                            Font-Size="14pt" ForeColor="#FF3300"></asp:Label>
             <hr />
                
                
                <asp:Button ID="send_btn" runat="server" Text="Vlož" onclick="send_btn_Click"/>
                <asp:Button ID="uprav_btn" runat="server" Text="Uprav" onclick="uprav_btn_Click"/>
    <asp:PlaceHolder ID="vykaz_pl" runat="server">
    <h1>Nastavenia pre výkaz</h1>
        <asp:Table ID="vykazSetup_tbl" runat="server" CssClass="responsive" data-max="13"></asp:Table>
        <asp:Button ID="saveVykaz_btn" runat="server" Text="Ulož nastavenie" OnClick="saveVykaz_fnc" CssClass="button green"  />
        <asp:Button ID="resetVykaz_btn" runat="server" Text="Reset nastavenia" OnClick="resetVykaz_fnc" CssClass="button red"  />
        </asp:PlaceHolder>
</asp:Content>

