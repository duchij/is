<%@ Page Language="C#" AutoEventWireup="true" CodeFile="synco.aspx.cs" Inherits="helpers_synco" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
   <h1>Sync users from omega:</h1>
        <hr />
        
        <asp:DropDownList ID="clinics_dl" runat="server" OnSelectedIndexChanged="setClinicData" AutoPostBack="true"></asp:DropDownList>
        Clinic_id:<asp:TextBox ID="clinic_id" runat="server" ReadOnly="true"></asp:TextBox>
        Idf:<asp:TextBox ID="kvValue_txt" runat="server" ReadOnly="true"></asp:TextBox>
        
        <asp:Button ID="sync_btn" runat="server" Text="Sync" OnClick="doSyncFnc" />
    </div>
    </form>
</body>
</html>
