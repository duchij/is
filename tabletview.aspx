<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tabletview.aspx.cs" Inherits="tabletview" ValidateRequest="False"%>

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
            <asp:TextBox ID="osirix_search_txt" runat="server" Font-Size="x-Large" Width="60%"></asp:TextBox>
          
            <asp:Button ID="Button1" runat="server" Text="Nájdi štúdiu" onClick="search_fnc" OnClientClick="aspnetForm.target ='_blank';" Font-Size="X-Large"/>
             <br />
               <br />
                <h1 style="font-size:20px;"> Pacienti so služby:</h1><hr />
            <asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" 
                BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" 
                Font-Names="Verdana" Font-Size="Large" ForeColor="#663399" Height="350px" 
                ShowGridLines="True" Width="90%" 
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
           <hr />
            <asp:PlaceHolder ID="hlasenie" runat="server">
            
            </asp:PlaceHolder>
            <hr />
            </div>
            
              <div id="right">
                  <h1 style="font-size:20px;"> Ranné sedenie</h1><hr />
                  <div style="text-align:center;">
                  <asp:Calendar ID="Calendar2" runat="server" BackColor="White" 
                      BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" 
                      DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" 
                      ForeColor="#003399" Height="200px" Width="90%" OnSelectionChanged="Calendar2_SelectionChanged">
                      <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                      <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                      <WeekendDayStyle BackColor="#CCCCFF" />
                      <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                      <OtherMonthDayStyle ForeColor="#999999" />
                      <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                      <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                      <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" 
                          Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px"   />
                  </asp:Calendar>
                  </div>
               <asp:Label ID="kojenci_title" runat="server" Text="Kojenci" Font-Size ="X-Large" BackColor="#FFAD10" Width="100%" Height="40px"></asp:Label>
               
               <asp:Table ID="kojenci_tbl" runat="server" BackColor="#FFAD10" Width="100%">
               </asp:Table>
             
               <table style="background-color:#FFAD10;width:100%;">
               <tr>
               <td colspan="2"> <hr /></td>
               </tr>
               <tr>
               <td> Priezvisko:</td>
               <td><asp:TextBox ID="kojenci_diag" runat="server" Width="90%" Height="23px" 
                        TextMode="SingleLine" Font-Size="X-Large"></asp:TextBox></td>
               </tr>
               <tr>
               <td>
               Poznámka:
               </td>
               <td>
               <asp:TextBox ID="kojenci_note" runat="server" Width="90%" Height="23px" Font-Size="X-Large"></asp:TextBox>
               </td>
               </tr>
               <tr>
               <td colspan="2" align="right">  <asp:Button ID="kojenci_diag_btn" runat="server" 
                        Text="<%$ Resources:Resource,add %>" onclick="kojenci_diag_btn_Click"  Font-Size="X-Large" /> </td>
               </tr>
               </table>
                <hr />  
                     
                  
         <asp:Label ID="dievcata_titel" runat="server" Text="Dievčatá" Font-Size ="X-Large" BackColor="#B17400" Width="100%" Height="40px" ></asp:Label>
               
               <asp:Table ID="dievcata_tbl" runat="server" BackColor="#B17400" Width="100%" >
               </asp:Table>
               
               <table style="background-color:#B17400;width:100%;">
                  <tr>
               <td colspan="2"> <hr /></td>
               </tr>
               <tr>
               <td> Priezvisko:</td>
               <td><asp:TextBox ID="dievcata_diag" runat="server" Width="90%" Height="23px" 
                        TextMode="SingleLine" Font-Size="X-Large"></asp:TextBox></td>
               </tr>
               <tr>
               <td>
               Poznámka:
               </td>
               <td>
               <asp:TextBox ID="dievcata_note" runat="server" Width="90%" Height="23px" Font-Size="X-Large"></asp:TextBox>
               </td>
               </tr>
               <tr>
               <td colspan="2" align="right">  <asp:Button ID="dievcata_diag_btn" runat="server" 
                        Text="<%$ Resources:Resource,add %>" onclick="dievcata_diag_btn_Click"  Font-Size="X-Large" /> </td>
               </tr>
               </table>
            
            <hr />
                   
                  
         <asp:Label ID="chlapci_titel" runat="server" Text="Chlapci" Font-Size ="X-Large" BackColor="#FF7810" Width="100%" Height="40px" ></asp:Label>
               
               <asp:Table ID="chlapci_tbl" runat="server" BackColor="#FF7810" Width="100%">
               </asp:Table>
             
               <table style="background-color:#FF7810;width:100%">
                  <tr>
               <td colspan="2"> <hr /></td>
               </tr>
               <tr>
               <td> Priezvisko:</td>
               <td><asp:TextBox ID="chlapci_diag" runat="server" Width="90%" Height="23px" 
                        TextMode="SingleLine" Font-Size="X-Large"></asp:TextBox></td>
               </tr>
               <tr>
               <td>
               Poznamka:
               </td>
               <td>
               <asp:TextBox ID="chlapci_note" runat="server" Width="90%" Height="23px" Font-Size="X-Large"></asp:TextBox>
               </td>
               </tr>
               <tr>
               <td colspan="2" align="right">  <asp:Button ID="chlapci_diag_btn" runat="server" 
                        Text="<%$ Resources:Resource,add %>" onclick="chlapci_diag_btn_Click"  Font-Size="X-Large" /> </td>
               </tr>
               </table>
            
        
        </div>
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </div>
     
    </form>
</body>
</html>
