<%@ Page Language="C#" AutoEventWireup="true" CodeFile="abstr.aspx.cs" Inherits="abstr" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <link href="styly.css" rel="stylesheet" type="text/css" />
    <title>Registracia</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="wrapper">
        
            <div id="header">
                 <table width="100%" cellspacing="2">
                    <tr>
                    <td align="left">
                    <a href="default_j.aspx" target="_self"><img src="img/sk.jpg" alt="Slovenska verzia"  /></a>
                     <a href="default_en.aspx" target="_self"><img src="img/en.jpg" alt="English version" /></a>
                    </td>
                        <td align="right">
                        Slovenská lekárska spoločnosť<br />
                        Slovenská spoločnosť detskej chirurgie pri SLS<br />
                        Česká pediatricko chirurgická společnost ČLS JEP<br />
                        Klinika detskej chirurgie LF UK a DFNsP<br />
                        Občianske združenie Detská chirurgia Slovenska<br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="m_title">57. kongres slovenských a českých detských chirurgov <br /></div>                            
                            <div class="s_title">s medzinárodnou účasťou</div>

                        </td>
                    
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                           <div class="m_date">Hotel Wellness Chopok, Jasná - Nízke Tatry,<br /> Slovakia 9. - 12. 3. 2011</div> 
     <div class="info">
               
                <strong>Organizačný výbor:</strong><br />
                <b>tel.: </b>++421/2/59371 342, 343, 173<b> fax</b>.: ++421/2/59371 866<br />
                <b>mobil.</b>: ++ 421/2/903 775 252, jasna.kdch.sk<br />
vladimir.cingel@gmail.com<br />
                
                </div>

                        </td>
                    
                    </tr>
                    
                    
 </table>  
            

            
            </div>
            
            <div id="content">
                <div id="menu">
                <ul class="main_menu">
                    <li><a href="default_j.aspx" target="_self">Hlavná stránka</a></li>
                    <li><a href="org.aspx" target="_self">Organizačné informácie</a></li>
                    <li><a href="prog.aspx" target="_self">Program</a></li>
                     <li></li>
                    <li><a href="abstr.aspx" target="_self" style="color:red;">Prihlásenie abstraktu</a></li>
                    <li></li>
                   
                    <li><a href="http://www.investito.sk" target="_blank">Investito.sk</a></li>
                    <li><a href="http://www.hotelchopok.sk" target="_blank">hotelchopok.sk</a></li>
                    <li><a href="http://www.kdch.sk/kongresy/jasna/jasna_1_info_final_sk_half.pdf" target="_blank">Prvá informácia (pdf) <img src="img/sk.jpg" /></a></li>
                    <li><a href="http://www.kdch.sk/kongresy/jasna/jasna_1_info_final_en_half.pdf" target="_blank">1st information (pdf) <img src="img/en.jpg" /></a></li>
                </ul>
                <br /><br /><br />
                
                
                
                </div>
                
                <div id="text">
                
                    <div class="main_text">
                    <strong class="title1">Registrácia</strong><hr />
                    <strong>Meno, email a adresa pracoviska je povinnné vyplniť. V prípade aktívnej účasti kliknite na aktívnu účasť, v tomto prípade je povinné vyplniť aj údaje o prednáške. V prípade, že ste len spoluautorom, nie je potrebné tieto údaje vyplniť. V prípade pasívnej účasti, stačí vyplniť len kontaktné údaje.</strong><hr />
                        <asp:Label ID="check1" runat="server" Text="" ></asp:Label>
                    
                    <table width="100%" style="color:Black;font-size:13px;font-weight:bolder;">
                        <tr>
                        <td>Titul pred menom:</td>
                        <td><asp:TextBox ID="titel1" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                        <tr>
                        <td>Meno</td>
                        <td><asp:TextBox ID="name" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                       
                    
                        <tr>
                        <td>Priezvisko</td>
                        <td><asp:TextBox ID="surname" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                        
                         <tr>
                        <td>Titul za menom:</td>
                        <td><asp:TextBox ID="titel2" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                        
                        <tr>
                        <td>e-mail</td>
                        <td><asp:TextBox ID="email" runat="server" Width="400px" ></asp:TextBox></td> 
                        
                        </tr>
                        
                        <tr>
                        <td valign="top">Adresa pracoviska</td>
                        <td>
                            <asp:TextBox ID="adresa_prac" runat="server" Width="400" TextMode="MultiLine" Rows="5"></asp:TextBox></td> 
                        
                        </tr>
                        
                        <tr>
                        <td valign="top">Účasť</td>
                        <td>
                            
                            <asp:CheckBox ID="active_status" runat="server" Text="Aktivna účasť"/><asp:CheckBox
                                ID="active_type" Text="Spoluautor" runat="server" />
                            </td> 
                        
                        </tr>                    
                    
                        
                    </table>
                
             <hr />
             <table style="color:Black;font-size:13px;font-weight:bolder;">
                <tr>
                    <td valign="top">Názov prednášky</td>
                    <td>
                        <asp:TextBox ID="nazov_pred" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                
                <tr>
                    <td valign="top">Autori prednášky</td>
                    <td>
                        <asp:TextBox ID="autory_pred" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                
                <tr>
                    <td valign="top">Jednotlivé pracoviská</td>
                    <td>
                        <asp:TextBox ID="praco_pred" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td valign="top">Sumár</td>
                    <td>
                        <asp:TextBox ID="sumar_sk" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td valign="top">Sumary in english</td>
                    <td>
                        <asp:TextBox ID="sumar_en" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td valign="top">Štrukturovaný abstrakt</td>
                    <td>
                        <asp:TextBox ID="abstrakt" runat="server" Rows="10" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                <td colspan="2">
                    <asp:Button ID="Button1" runat="server" Text="Odoslat" 
                        onclick="Button1_Click" Enabled = "false" /></td>
                </tr>
                
             
             </table>   
                        <asp:Label ID="check2" runat="server" Text=""></asp:Label>          
                
                
                </div>  <!-- main text !-->
            </div> <!-- content !-->
            <div id="footer">
            Klinika detskej chirurgie LF UK a DFNsP, 2010<br />
            Design by B.Duchaj
            </div>
            
        </div>
    
    
    
    </div>
    </form>
</body>
</html>
