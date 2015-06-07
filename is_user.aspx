<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="is_user.aspx.cs" Inherits="is_user" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
    <asp:PlaceHolder ID="admin_plh" runat="server">

        <div class="row">
            <h1 class="red">Administrator</h1>
            <div class="one half">
                <div class="row">Časť mena: <asp:TextBox ID="search_txt" runat="server" Width="200px" CssClass="inline" ToolTip="Zadaj časť priezviska s diakritikou"></asp:TextBox>
                        <asp:Button ID="search_btn" runat="server" Text="Hľadaj" OnClick="searchByNameFnc" CssClass="blue button" />
                </div>
                <asp:GridView ID="users_gv" runat="server" data-max="15" CssClass="responsive" AllowPaging="True"  EnableViewState="true"
                    OnPageIndexChanging="users_gv_PageIndexChanging" 
                    OnSelectedIndexChanging="users_gv_SelectedIndexChanging"
                    CaptionAlign="Left" AutoGenerateColumns="False" EnableModelValidation="True" SelectedRowStyle-BackColor="#66FF33">
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
                 <p class="red"> !!! Po zadaní mena a priezviska v nastaveniach užívateľa a opustení textového rámika sa automaticky vegenerujú login a rôzne údaje v admin sekcii, prosím skontrolujte ich.</p>
                <table>
                    <tr>
                        <td>
                        Heslo: <asp:TextBox ID="passwd_txt" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:Button ID="resetpasswd_btn" OnClick="resetPasswordFnc" Text="Reset password" CssClass="red button" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        Aktívny: <asp:DropDownList ID="active_dl" runat="server">
                                     <asp:ListItem Value="1">Aktívny</asp:ListItem>
                                     <asp:ListItem Value="0">Zablokovaný</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td>
                        Name2: <asp:TextBox ID="name2_txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>
                        Name3: <asp:TextBox ID="name3_txt" runat="server"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>
                        Login: <asp:TextBox ID="login_txt" runat="server" AutoPostBack="true" OnTextChanged="checkLoginValidtyFnc"></asp:TextBox>
                            <p>V prípade zadania osobného čísla prosím vložte pred neho d. Napr.dxxxxx. Inak nebude užívateľ nájdený<br />
                                Login može obsahovať len písmená, čísla a podtrhovátko. 

                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        Práva:<asp:DropDownList ID="rights_cb" runat="server">
                                 <asp:ListItem Value=""></asp:ListItem>
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
                                    <asp:ListItem Value=""></asp:ListItem>
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
                        Klinika:<asp:DropDownList ID="clinics_dl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="loadDeps">
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
            <h1 class="green">Nastavenia užívateľa</h1>
            <div class="one half">
                 <table>
                    <tr>
                        <td>Titul pred menom:<asp:TextBox ID="titul_pred" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td>Meno a priezvisko:<asp:TextBox ID="name_txt" runat="server" OnTextChanged="createNamesFnc" AutoPostBack="true"></asp:TextBox>
                           
                        </td>
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
                    <tr>
                        <td><asp:Button ID="resetUserPsswd_btn" runat="server" Text="Reset hesla...." CssClass="red button" OnClick="resetPasswdUser" /><br />
                            <p>Možnosť zmeny hesla daného užívateľa. Po stlačení budete presmerovaný na stránku zmeny hesla... </p></td>
                    </tr>
                </table>
            </div>
            
        </div>
        <asp:Button ID="newUser_btn" runat="server" Text="Vlož nového užívateľa" CssClass="button red" OnClick="newUserFnc" />
        <asp:Button ID="updateUser_btn" runat="server" Text="Aktualizuj užívateľa" Enabled="false" OnClick="updateUserFnc" />
    </asp:PlaceHolder>

    

</asp:Content>

