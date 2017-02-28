<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fpassch.aspx.cs" Inherits="passch2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/style.css" />
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/sha3.js" type="text/javascript"></script>
    <script src="js/login.js" type="text/javascript"></script>
</head>
<body onload="readHeaders();">
    <form id="form1" runat="server">
    <div>
        <h2>Vynútená zmena hesla do systému....</h2>
        <hr />
        Prosím o zmenu na nové heslo, vzhľadom na bezpečnosť... Obe heslá musia byť rovnaké...<hr />

            <asp:Label ID="info_lbl" runat="server" Text="Label" Visible="False"></asp:Label><br /><br />
                <table border="0">
                    <tr>
                        <td>Meno:</td>
                        <td><input type="text" name="uname" id="uname_txt" value="" readonly="readonly"/></td>
                    </tr>
                <tr>
                    <td>Nové heslo:</td>
                    <td><input type="password" id="passwd1_txt" name="pass1" /></td>

                </tr>
               <tr>
               <td>Nové heslo2:</td>
               <td><input type="password" id="passwd2_txt" name="pass2" /></td>
              </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" name="changePass" value="Zmen" onclick="changePasswordFnc();" />

                    </td>
                </tr>
                     </table>
    
    </div>
    </form>
</body>
</html>
