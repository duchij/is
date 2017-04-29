<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fpassch.aspx.cs" Inherits="fpassch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/style.css" />
    <script src="js/jquery.js" type="text/javascript"></script>
    
    <script src="js/sha1a.js" type="text/javascript"></script>

    <script src="js/enc-base64.js" type="text/javascript"></script>
    
    <script src="js/login.js" type="text/javascript"></script>
</head>
<body onload="readHeaders();">
    <form id="form1" runat="server">
    <div>
        <h2>Vynútená zmena hesla do systému....</h2>
        <hr />
        Prosím o zmenu na nové heslo...<br /> <strong>Obe heslá musia byť rovnaké... </strong><br />Po vyplnení oboch hesiel je nutné myšou kliknúť na tlačidlo <strong>Zmen</strong><br /><br />
        <strong>Po úspešnej zmene hesla budete presmerovaný opätovne na login, kde už použijete Vaše nové heslo...</strong>
        <hr />

            <asp:Label ID="info_lbl" runat="server" Text="Label" Visible="False"></asp:Label><br /><br />
                <table border="0">
                    <tr>
                        <td>Meno:</td>
                        <td><input type="text" id="uname_txt" value="" readonly="readonly"/></td>
                    </tr>
                <tr>
                    <td>Nové heslo:</td>
                    <td><input type="password" id="passwd1_txt"  /></td>

                </tr>
               <tr>
               <td>Nové heslo2:</td>
               <td><input type="password" id="passwd2_txt"  /></td>
              </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" name="changePass" value="Zmen" onclick="changePasswordFnc();" />

                    </td>
                </tr>
                     </table>
    
    </div>
        <hr />
        <div id="info_txt"></div>
        <hr />
    </form>
</body>
</html>
