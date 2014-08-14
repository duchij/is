<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tabletview.aspx.cs" Inherits="tabletview" ValidateRequest="False"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ranna vizita</title>
    <style type="text/css">
    body {
    font-family:Arial, Verdana;
    }
    #wrapper {
    width:720px;
    height:1010px;
    padding:5px;
    margin:0 auto;
    }
   .content {
    overflow:auto;}
    
  
            
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div class="content">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="default.aspx" Font-Size="X-Large">Odhlásiť....</asp:HyperLink>
            <asp:Calendar ID="Calendar1" runat="server" BackColor="#FFFFCC" 
                BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" 
                Font-Names="Verdana" Font-Size="XX-Large" ForeColor="#663399" Height="400px" 
                ShowGridLines="True" Width="440px" 
                onselectionchanged="Calendar1_SelectionChanged" WeekendDayStyle-HorizontalAlign="Center">
                <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                <SelectorStyle BackColor="#FFCC66" />
                <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                <OtherMonthDayStyle ForeColor="#CC9966" />
                <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                    ForeColor="#FFFFCC" />
            </asp:Calendar>
        
            <asp:Label ID="OddA" runat="server" Text="Oddelenie A" BackColor="Aqua" Font-Size="X-Large"></asp:Label>
            <hr />
            <table border=1>
            <tr>
                <td> <asp:Label ID="OddA_txt" runat="server" Text="" CssClass="links"></asp:Label> </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Width="200" Height="200"></asp:TextBox>  </td>
           
            </tr>
            </table>
            <asp:Label ID="OddB" runat="server" Text="Oddelenie B" BackColor="Beige" Font-Size="X-Large"></asp:Label>
            <hr />
            <asp:Label ID="OddB_txt" runat="server" Text="" CssClass="links"></asp:Label>
            <asp:Label ID="Pohovost" runat="server" Text="Op. pohotovost" BackColor="Brown" ForeColor="White" Font-Size="X-Large"></asp:Label>
            <hr />
            <asp:Label ID="Pohotovost_txt" runat="server" Text="" CssClass="links"></asp:Label>
            
        
        </div>
    
    </div>
    </form>
</body>
</html>
