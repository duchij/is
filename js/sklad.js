$(document).ready(function () {

    $("input[id$=ean1_txt]").keypress(function (e) {

        var txt = $("input[id$=ean1_txt]").val();
        alert(txt);
        var keynum;

        if (window.event) { // IE					
            keynum = e.keyCode;
        } else
            if (e.which) { // Netscape/Firefox/Opera					
                keynum = e.which;
            }
        var keyP = String.fromCharCode(keynum);
        var retK = "";
       /* ean = ean.Replace("+", "1");
   
        
        ean = ean.Replace("ľ", "2");
        ean = ean.Replace("š", "3");
        ean = ean.Replace("č", "4");
        ean = ean.Replace("ť", "5");
        ean = ean.Replace("ž", "6");
        ean = ean.Replace("ý", "7");
        ean = ean.Replace("á", "8");
        ean = ean.Replace("í", "9");
        ean = ean.Replace("é", "0");*/


        if (keyP == "+") retK = "1";
        if (keyP == "ľ") retK = "2";
        var txtLn = txt.length;

        txt = txt.substr(txtLn - 1, 1);
        txt = txt + retK;
        $("input[id$=ean1_txt]").val(txt);
       
    });

});
