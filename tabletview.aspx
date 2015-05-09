<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tabletview.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="tabletview" ValidateRequest="False" Culture="sk-SK"%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <asp:Label ID="msg_lbl" runat="server" Text=""></asp:Label>
<div class="row">
<div class="two thirds">
<asp:TextBox ID="osirix_search_txt" runat="server"></asp:TextBox>
</div>
<div class="one third">
      <asp:Button ID="Button1" runat="server" Text="Nájdi štúdiu" onClick="search_fnc" OnClientClick="aspnetForm.target ='_blank';" CssClass="medium button green" />
      </div>
</div>
           
<div class="row">

  <div class="one half one-up-ipad padded">
      
       <h1 class="pink box"> Pacienti so služby:</h1><hr />
            <asp:Calendar ID="Calendar1" runat="server" BackColor="White" 
                BorderColor="#d9edf7" DayNameFormat="Shortest" 
                ForeColor="Black" 
          
                onselectionchanged="Calendar1_SelectionChanged" 
          WeekendDayStyle-HorizontalAlign="Center" CellPadding="4" CssClass="responsive" data-max="15">
                <SelectedDayStyle BackColor="#46627f" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#46627f" />
                        <WeekendDayStyle BackColor="#dff0d8" />
                        <TodayDayStyle BackColor="#f0e6f4" ForeColor="black" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#ffe5c7" Font-Bold="True" />
                        <TitleStyle BackColor="#d9edf7" BorderColor="Black" Font-Bold="True" />
            </asp:Calendar>
           <hr />
            <asp:PlaceHolder ID="hlasenie" runat="server">
                <asp:Table ID="osirixShift_tbl" runat="server"></asp:Table>
            </asp:PlaceHolder>
</div>
  <div class="one half one-up-ipad padded">
                  <h1 class="turquoise box"> RDG diagnostika:</h1><hr />
                  <div class="align-center">
                    <asp:Calendar ID="Calendar2" runat="server" BackColor="White" 
                        BorderColor="#46627f" CellPadding="4" 
                        DayNameFormat="Shortest" 
                        ForeColor="Black" 
                          OnSelectionChanged="Calendar2_SelectionChanged" CssClass="responsive" data-max="15">
                        <SelectedDayStyle BackColor="#46627f" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#46627f" />
                        <WeekendDayStyle BackColor="#dff0d8" />
                        <TodayDayStyle BackColor="#f0e6f4" ForeColor="black" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#ffe5c7" Font-Bold="True" />
                        <TitleStyle BackColor="#d9edf7" BorderColor="Black" Font-Bold="True" />
                    </asp:Calendar>
                  </div>
                <br />  
                <hr />
      <asp:Table ID="depsRdg_tbl" runat="server" EnableViewState="false"></asp:Table>
                <hr />

               <div class="green box"> <h2><asp:Label ID="kojenci_title" runat="server" Text="Kojenci" ></asp:Label></h2></div>
               
               <asp:Table ID="kojenci_tbl" runat="server" BackColor="#FFAD10" Width="100%">
               </asp:Table>
             
               <table class="responsive" data-max="15">
               <tr>
               <td colspan="2"> <hr /></td>
               </tr>
               <tr>
               <td> Priezvisko:</td>
               <td><asp:TextBox ID="kojenci_diag" runat="server" TextMode="SingleLine" /></asp:TextBox></td>
               </tr>
               <tr>
               <td>
               Poznámka:
               </td>
               <td>
               <asp:TextBox ID="kojenci_note" runat="server" class="responsive" data-max="15" ></asp:TextBox>
               </td>
               </tr>
               <tr>
               <td colspan="2" align="right">  <asp:Button ID="kojenci_diag_btn" runat="server" 
                        Text="<%$ Resources:Resource,add %>" onclick="kojenci_diag_btn_Click"  CssClass="button" /> </td>
               </tr>
               </table>
                <hr />  
                     
                  
         <div class="red box"><h2><asp:Label ID="dievcata_titel" runat="server" Text="Dievčatá" ></asp:Label></h2></div>
               
               <asp:Table ID="dievcata_tbl" runat="server" class="responsive" data-max="15"  >
               </asp:Table>
               
               <table class="responsive" data-max="15">
                  <tr>
               <td colspan="2"> <hr /></td>
               </tr>
               <tr>
               <td> Priezvisko:</td>
               <td><asp:TextBox ID="dievcata_diag" runat="server" TextMode="SingleLine" ></asp:TextBox></td>
               </tr>
               <tr>
               <td>
               Poznámka:
               </td>
               <td>
               <asp:TextBox ID="dievcata_note" runat="server" ></asp:TextBox>
               </td>
               </tr>
               <tr>
               <td colspan="2" align="right">  <asp:Button ID="dievcata_diag_btn" runat="server" 
                        Text="<%$ Resources:Resource,add %>" onclick="dievcata_diag_btn_Click"  CssClass="button" /> </td>
               </tr>
               </table>
            
            <hr />
                   
                  
          <div class="orange box"><h2><asp:Label ID="chlapci_titel" runat="server" Text="Chlapci" ></asp:Label></h2></div>
               
               <asp:Table ID="chlapci_tbl" runat="server" class="responsive" data-max="15" >
               </asp:Table>
             
               <table class="responsive" data-max="15" >
                  <tr>
               <td colspan="2"> <hr /></td>
               </tr>
               <tr>
               <td> Priezvisko:</td>
               <td><asp:TextBox ID="chlapci_diag" runat="server" TextMode="SingleLine"/> </asp:TextBox></td>
               </tr>
               <tr>
               <td>
               Poznamka:
               </td>
               <td>
               <asp:TextBox ID="chlapci_note" runat="server" ></asp:TextBox>
               </td>
               </tr>
               <tr>
               <td colspan="2" align="right">  
               <asp:Button ID="chlapci_diag_btn" runat="server" Text="<%$ Resources:Resource,add %>" onclick="chlapci_diag_btn_Click"  CssClass="button" /> </td>
               </tr>
               </table>
            
        
        </div>
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </div>
     
</asp:Content>

