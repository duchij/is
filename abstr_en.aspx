<%@ Page Language="C#" AutoEventWireup="true" CodeFile="abstr.aspx_en.cs" Inherits="abstr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <link href="styly.css" rel="stylesheet" type="text/css" />
    <title>Registration</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="wrapper">
        
             <div id="header">
                <table width="100%" cellspacing="2" border="0">
                    <tr><td align="left">
                    <a href="default_j.aspx" target="_self"><img src="img/sk.jpg" alt="Slovenska verzia"  /></a>
                     <a href="default_en.aspx" target="_self"><img src="img/en.jpg" alt="English version" /></a>
                    </td>
                        <td align="right">
                        Slovak Medical Association<br />
                        Slovak Association of Paediatric Surgery<br />
                        Czech Association of Paediatric Surgery<br />
                        Department of Paediatric Surgery, Children‘s University Hospital and Faculty of
Medicine, Comenius University in Bratislava<br />
                        Slovak Civic Association of Paediatric Surgery<br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="m_title">The 57th
Congress of Slovak and Czech
Paediatric Surgeons<br /></div>                            
                            <div class="s_title">with international participation</div>

                        </td>
                    
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                           <div class="m_date">Hotel Wellness Chopok, Jasná - Nízke Tatry,<br /> Slovakia 9. - 12. 3. 2011</div> 
     <div class="info">
               
                <strong>Organizing Committee:</strong><br />
                <b>Phone: </b>++421/2/59371 342, 343, 173<b> Fax</b>.: ++421/2/59371 866<br />
                <b>Mobile phone:</b>: ++ 421/2/903 775 252, jasna.kdch.sk<br />
vladimir.cingel@gmail.com<br />
                
                </div>

                        </td>
                    
                    </tr>
                    
                    
                </table>
            
            

            
            </div>
            <div id="content">
                <div id="menu">
                <ul class="main_menu">
                    <li><a href="default_en.aspx" target="_self">Main page</a></li>
                    <li><a href="org_en.aspx" target="_self">Organisational information</a></li>
                    <li><a href="prog_en.aspx" target="_self">Program</a></li>
		    <li></li>
                    <li><a href="abstr_en.aspx" target="_self" style="color:red;">Abstract registration</a></li>
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
                    <strong class="title1">Registration</strong><hr />
                    <strong>Name, email and adress of  the employer are obligatory. In case of active participation, check the active participation checkbox, in this case the information about the lecture is also obligatory, co-authors have to fill the registration form, but are not obligated to fill out the lecture informtion, but it is neccessary to check the co-author checkbox. In case of passice participation fill out only the personal data. </strong><hr />
                        <asp:Label ID="check1" runat="server" Text="" ></asp:Label>
                    
                    <table width="100%" style="color:Black;font-size:13px;font-weight:bolder;">
                        <tr>
                        <td>Titul before name</td>
                        <td><asp:TextBox ID="titel1" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                        <tr>
                        <td>Name</td>
                        <td><asp:TextBox ID="name" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                       
                    
                        <tr>
                        <td>Surname</td>
                        <td><asp:TextBox ID="surname" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                        
                         <tr>
                        <td>Titul afeter name</td>
                        <td><asp:TextBox ID="titel2" runat="server" Width="400px" ></asp:TextBox></td>                                                
                        </tr>
                        
                        <tr>
                        <td>e-mail</td>
                        <td><asp:TextBox ID="email" runat="server" Width="400px" ></asp:TextBox></td> 
                        
                        </tr>
                        
                        <tr>
                        <td valign="top">Adress of emoployer</td>
                        <td>
                            <asp:TextBox ID="adresa_prac" runat="server" Width="400" TextMode="MultiLine" Rows="5"></asp:TextBox></td> 
                        
                        </tr>
                        
                        <tr>
                        <td valign="top">Participation</td>
                        <td>
                            
                            <asp:CheckBox ID="active_status" runat="server" Text="Active participation"/><asp:CheckBox
                                ID="active_type" Text="Co-author" runat="server" />
                            </td> 
                        
                        </tr>                    
                    
                        
                    </table>
                
             <hr />
             <table style="color:Black;font-size:13px;font-weight:bolder;">
                <tr>
                    <td valign="top">Lecture title</td>
                    <td>
                        <asp:TextBox ID="nazov_pred" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                
                <tr>
                    <td valign="top">Lecture authors (all)</td>
                    <td>
                        <asp:TextBox ID="autory_pred" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                
                <tr>
                    <td valign="top">Address of all authors</td>
                    <td>
                        <asp:TextBox ID="praco_pred" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                <!--<tr>
                    <td valign="top">Sumary in english</td>
                    <td>
                        <asp:TextBox ID="sumar_sk" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>!-->
                <tr>
                    <td valign="top">Sumary in english</td>
                    <td>
                        <asp:TextBox ID="sumar_en" runat="server" Rows="4" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td valign="top">Structured abstract</td>
                    <td>
                        <asp:TextBox ID="abstrakt" runat="server" Rows="10" TextMode="MultiLine" 
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                <td colspan="2">
                    <asp:Button ID="Button1" runat="server" Text="Submit" 
                        onclick="Button1_Click" enabled = "false" /></td>
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
