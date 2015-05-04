﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_user.aspx.cs" Inherits="is_user" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
    <asp:PlaceHolder ID="admin_plh" runat="server">

        <div class="row">
            <div class="one half">
                <div class="row"><asp:TextBox ID="search_txt" runat="server"></asp:TextBox>
                        <asp:Button ID="search_btn" runat="server" Text="Hľadaj" OnClick="searchByNameFnc" />
                </div>
                <asp:GridView ID="users_gv" runat="server" data-max="15" CssClass="responsive" AllowPaging="True" 
                    OnPageIndexChanging="users_gv_PageIndexChanging" 
                    OnSelectedIndexChanging="users_gv_SelectedIndexChanging"
                    CaptionAlign="Left" Caption="Zoznam užívateľov" AutoGenerateColumns="False" EnableModelValidation="True" SelectedRowStyle-BackColor="#66FF33">
                         <Columns>
                             <asp:CommandField ShowSelectButton="True" />
                             <asp:BoundField DataField="id" HeaderText="id">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField DataField="full_name" HeaderText="Celé meno">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField DataField="name" HeaderText="login">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:BoundField ConvertEmptyStringToNull="False" DataField="prava" 
                                 HeaderText="Skupina">
                                 <HeaderStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                             <asp:CommandField ShowDeleteButton="True" />
                         </Columns>
                </asp:GridView> 
               
            </div>
            <div class="one half">
                <asp:HiddenField ID="selectedUser_hf" runat="server" Value="0" />
                <table>
                    <tr>
                        <td>
                        Heslo: <asp:TextBox ID="passwd_txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        Aktívny: <asp:TextBox ID="active_txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        Login: <asp:TextBox ID="login_txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        Práva:<asp:DropDownList ID="rights_cb" runat="server">
                                <asp:ListItem Value="admin">Admin</asp:ListItem>
                                <asp:ListItem Value="sadmin">SubAdmin</asp:ListItem>
                                <asp:ListItem Value="poweruser">PowerUser</asp:ListItem>
                                <asp:ListItem Value="users">Users</asp:ListItem>
                                <%--<asp:ListItem Value="sestra">Sestra</asp:ListItem>
                                <asp:ListItem Value="sestra_vd">Sestra_vd</asp:ListItem>
                                <asp:ListItem Value="medix">medix</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        Zaradenie:<asp:DropDownList ID="workgroup_dl" runat="server">
                                    <asp:ListItem Value="doctor">Lekár</asp:ListItem>
                                    <asp:ListItem Value="nurse">Sestra</asp:ListItem>
                                    <asp:ListItem Value="assistent">Asistent</asp:ListItem>
                                    <asp:ListItem Value="op">Operačky</asp:ListItem>
                                    <asp:ListItem Value="other">Ostatné</asp:ListItem>
                                <%--<asp:ListItem Value="sestra">Sestra</asp:ListItem>
                                <asp:ListItem Value="sestra_vd">Sestra_vd</asp:ListItem>
                                <asp:ListItem Value="medix">medix</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td>
                           <%-- OnSelectedIndexChanged="loadDeps" AutoPostBack="true"--%>
                        Klinika:<asp:DropDownList ID="clinics_dl" runat="server" >
                                </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td>
                        Oddelenie:<asp:DropDownList ID="oddelenie_dl" runat="server">
                                  </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:PlaceHolder>
    <hr />
    <asp:PlaceHolder ID="user_plh" runat="server">
        <div class="row">
            <div class="one half">
                 <table>
                    <tr>
                        <td>Titul pred menom:<asp:TextBox ID="titul_pred" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td>Meno a priezvisko:<asp:TextBox ID="name_txt" runat="server"></asp:TextBox></td>
                    </tr>
                      <tr>
                        <td>Titul za menom:<asp:TextBox ID="titul_za" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Email:<asp:TextBox ID="email_txt" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Pracovné zaradenie:<asp:TextBox ID="zaradenie_txt" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
            <div class="one half">
                <table>
                    <tr>
                        <td>Pracovná doba:<asp:TextBox ID="pracdoba_txt" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Týždenná pracovná doba:<asp:TextBox ID="tyzdoba_txt" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Osobné číslo:<asp:TextBox ID="osobcisl_txt" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Klinika (skratka, napr. výkaz):<asp:TextBox ID="klinika_txt" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
            
        </div>
        <asp:Button ID="newUser_btn" runat="server" Text="Vlož nového užívateľa" CssClass="button red" OnClick="newUserFnc" />
        <asp:Button ID="updateUser_btn" runat="server" Text="Aktualizuj užívateľa" Enabled="false" OnClick="updateUserFnc" />
    </asp:PlaceHolder>

    

</asp:Content>

