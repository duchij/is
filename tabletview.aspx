<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tabletview.aspx.cs" Inherits="tabletview" ValidateRequest="False"   %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Ranna vizita - iPad</title>
    <link href="css/tabletstyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
   /* body {
    font-family:Arial, Verdana;
    }
    #wrapper {
    width:720px;
    height:1010px;
    padding:5px;
    margin:0 auto;
    }
   .content {
   padding:5px;
    overflow:auto;}
    */
  
            
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
     <div style="text-align:right;"><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="default.aspx" Font-Size="X-Large"  >Odhlásiť....</asp:HyperLink></div>
        <div id="left">
           
            <br />
            <asp:TextBox ID="osirix_search_txt" runat="server" Font-Size="x-Large" Width="70%"></asp:TextBox>
            
            <asp:Button ID="Button1" runat="server" Text="Najdi studiu" onClick="search_fnc" OnClientClick="aspnetForm.target ='_blank';" Font-Size="X-Large"/>
           
            <asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" 
                BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" 
                Font-Names="Verdana" Font-Size="Large" ForeColor="#663399" Height="350px" 
                ShowGridLines="True" Width="400px" 
                onselectionchanged="Calendar1_SelectionChanged" WeekendDayStyle-HorizontalAlign="Center">
                <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                <SelectorStyle BackColor="#FFCC66" />
                <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                <OtherMonthDayStyle ForeColor="#CC9966" />
                <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="16pt" 
                    ForeColor="#FFFFCC" />
            </asp:Calendar>
            <br /> <br />
            </div>
              <div id="right">
            <table border="1" width="100%" style="border:1px solid #FF7A00; border-collapse:collapse;">
            <tr>
            <td colspan="2" valign="middle" style="background-color:#FF7A00;height:40px;"  ><div style="font-size:x-large;color:black;"> Kojenci</div></td></tr>
            <tr>
                <td width="50%"> <asp:Label ID="kojenci_txt" runat="server" Text="" CssClass="links"></asp:Label> </td>
                <td width="50%">
                    Priezvisko:<asp:TextBox ID="kojenci_diag" runat="server" Width="200px" Height="23px" 
                        TextMode="SingleLine" Font-Size="X-Large"></asp:TextBox><br />
                        Poznamka:<asp:TextBox ID="kojenci_note" runat="server" Width="200px"></asp:TextBox>
                    <asp:Button ID="kojenci_diag_btn" runat="server" 
                        Text="<%$ Resources:Resource,add %>" onclick="kojenci_diag_btn_Click"  Font-Size="X-Large" />  </td>
           
            </tr>
            </table>
          
          
           
            <table border="1" width="100%" style="border:1px solid #A65000; border-collapse:collapse;">
            <tr>
            <td colspan="2" valign="middle" style="background-color:#A65000;height:40px;" >
            <div style="font-size:x-large;color:black;">Dievcata - MSV</div>
            </td>
            </tr>
            <tr>
                <td width="50%"><asp:Label ID="OddB_txt" runat="server" Text="" CssClass="links"></asp:Label></td>
                <td width="50%">
                <asp:TextBox ID="OddB_diag" runat="server" Width="337px" Height="200px" 
                        TextMode="MultiLine" Font-Size="X-Large"></asp:TextBox><br />
                    <asp:Button ID="OddB_diag_btn" runat="server" Text="<%$ Resources:Resource,save %>" Font-Size="X-Large" />  </td>
            
           
            
            </tr>
            </table>
            <hr /><br />
            
            <table border="1" width="100%" style="border:1px solid #FF9b40; border-collapse:collapse;">
            <tr>
            <td colspan="2" valign="middle" style="background-color:#FF9b40;height:40px;">
            <div style="font-size:x-large;color:black;">Velke deti </div></td>
            </tr>
            <tr>
                <td width="50%"><asp:Label ID="Pohotovost_txt" runat="server" Text="" CssClass="links"></asp:Label></td>
                <td>
                    <asp:TextBox ID="Pohotovost_diag" runat="server" Width="340px" Height="200px" 
                        TextMode="MultiLine" Font-Size="X-Large"></asp:TextBox><br />
                    <asp:Button ID="Pohotovost_diag_btn" runat="server" Text="<%$ Resources:Resource,save %>" Font-Size="X-Large" /> 
                
                
                </td>
                
                
            </tr>
            </table>
        
        </div>
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </div>
     
    </form>
</body>
</html>
