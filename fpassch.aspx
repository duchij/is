<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fpassch.aspx.cs" Inherits="fpassch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/style.css" />
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/sha1a.js" type="text/javascript"></script>
    <script src="js/enc-base64.js" type="text/javascript"></script>
    <script src="js/login_min.js" type="text/javascript"></script>
</head>
<body onload="readHeaders();">
    <form id="form1" runat="server">
    <div style="width:80%;margin:auto;">
        <h2>Vynútená zmena hesla do systému....</h2>
        <hr />
        <p style="color:darkred; font-size:13px;"> 
            Vzhľadom na smernicu <strong>EU GDRP od 25.5.2018</strong> je nutné najvyššia ochrana hesla, preto je nutné nastaviť Vaše heslo a zmena komunukácie IS.
            Pre Vás sa nič nemení, len tento úkon je nutný na prešifrovanie Vášho hesla..
        </p>
        Prosím o zmenu na nové heslo...<br />
        <h2>Dôležité</h2>
        <ul>
            <li>Obe heslá musia byť rovnaké</li>
            <li>Môžete použiť aj Vaše pôvodné heslo</li>
            <li>Heslo, by ste nikomu nemali dávať ani ho mať uložené na viditeľnom mieste.</li>
            <li>Viac napr. <a href="https://www.podnikajte.sk/pravo-a-legislativa/c/3329/category/zakonne-povinnosti-podnikatela/article/ochrana-osobnych-udajov-25-5-2018-gpdr.xhtml" target="_blank">tu</a> </li>

        </ul>
        
        <p> Po vyplnení oboch hesiel je nutné myšou kliknúť na tlačidlo <strong>Zmeň</strong></p>
        <strong>Po úspešnej zmene hesla budete presmerovaný opätovne na login, kde už použijete Vaše nové heslo...</strong>
        <p> Ak Vám bude prehliadač hlásiť nezašifrovanú komunikáciu, to je v poriadku, pretože dáta sa sú pre tento úkon jednorázovo zakódované....</p>
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
                        <input type="button" style="background-color:darkred;color:yellow;" name="changePass" value="Zmeň" onclick="changePasswordFnc();" />

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
