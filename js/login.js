var __sid = null;

function messageBox(text, type) {
    switch (type) {
        case "error":
            $("[id$=msg_dialog]").html("<h2 class='red'>CHYBA:</h2><p class='red'>" + text + "</p>");
            $("[id$=msg_dialog]").dialog({

                closeOnEscape: true

            });


            $("[id$=msg_dialog]").dialog();
            break;
        case "warning":
            $("[id$=msg_dialog]").html("<h2 class='green'>Upozornenie:</h2><p class='green'>" + text + "</p>");
            $("[id$=msg_dialog]").dialog({

                closeOnEscape: true

            });
            $("[id$=msg_dialog]").dialog();
            break;
        default:
            $("[id$=msg_dialog]").html("<h2 class='blue'>Info:</h2><p class='blue'>" + text + "</p>");
            $("[id$=msg_dialog]").dialog({

                closeOnEscape: true

            });
            $("[id$=msg_dialog]").dialog();
            break;
    }


}


function sendData(fnc, data, callBack, args) {
    $("#commBall").css("display", "block");
    /// <summary>Funkcia na poslanie jQuery Ajaxu smerom na server</summary>
    /// <param name="fnc" type="String">Funkcia na strane serveru vo WebService.asmx</param>
    /// <param name="data" type="Object">JSON Objekt co sa ma poslat</param>
    /// <param name="callBack" type="String">Javascript funkcia kam sa to ma vratit</param>
    /// <param name="args" type="Mixed">Moze by objekt, pole, hocico co sa preposle do callback funkcie</param>
    $.ajax({
        url: "WebService.asmx/" + fnc,
        method: "POST",
        dataType: "text",
        // contentType: "application/json; charset=utf-8",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        // contentType: "application/x-www-form-urlencoded",
        //contentType: "text/html",
        data: "data=" + JSON.stringify(data),
        error: function (jqXHR, errorText) {
            messageBox(errorText, "error");
        },
        success: function (data) {
            console.log(data);
            //showNewsText(data);
            var xml = $.parseXML(data);
            console.log(xml);
            var dtJson = xml.childNodes[0].textContent;
            var obj = JSON.parse(dtJson);
            $("#commBall").css("display", "none");
            window[callBack](obj, args);
        }

    });
}

function getData()
{
    sendData("getSessionID", {}, "setSessionData");
}

function setSessionData(result)
{
    __sid = result.sid;

  
}

function getURLParameter(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [null, ''])[1].replace(/\+/g, '%20')) || null;
}

function readHeaders()
{
   
    var xhtp = new XMLHttpRequest();
    xhtp.open('GET', document.location, false);
    xhtp.send(null);

    var headers = xhtp.getAllResponseHeaders().toString();

    var uname = getURLParameter('uname');

    if (uname === null) {
        alert("Nebolo zadane meno. Budete presmerovani, spat na login");
        window.location.href = "Default.aspx";

    } else {
        $("#uname_txt").val(uname);
    }
    

}

function runLogin() {


    var name = $("[id$=meno_txt]").val();
    if (name.trim().length == 0) {
        
        $("[id$=info_txt]").html("Nie je zadane meno");
        return false;
    }

    var pss = $("[id$=passwd_txt]").val();
    if (pss.trim().length == 0) {
        $("[id$=info_txt]").html("Nie je zadane heslo");
        return false;
    }

    // alert("huera");
    // console.log(__sid);

    var kP = __sid.substr(0, 16);

    console.log(kP.length);
    
    var key = CryptoJS.enc.Utf8.parse(kP.trim());
    var iv = CryptoJS.enc.Utf8.parse(kP.trim());
    //console.log([key, iv]);

   var encryptedName = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(name.trim()),key,{
        keySize:128/8,
        iv:iv,
        mode:CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    var encryptedPasswd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(pss.trim()), key, {
        keySize: 128/8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    var nm = encryptedName;
    var np = encryptedPasswd;

    
    $("[id$=name_hf]").val(nm);
    $("[id$=passwd_hf]").val(np);
    
   // __doPostBack("login_btn", "login");
}


function changePasswordFnc()
{
   

    var p1 = $("#passwd1_txt").val().toString();
    var p2 = $("#passwd2_txt").val().toString();

    if (p1 === p2) {

        var passwd = CryptoJS.SHA3(p1);
        var pStr = passwd.toString();

        var data = {
                uname: $("#uname_txt").val(),
                passwd: pStr
            };

        //console.log(data);

        sendData("forceChangePasswd", data, "afterPasswdChange");

    }

}

function afterPasswdChange(result)
{
    console.log(result);

    if (result.status === "True") {
        window.location.href = "Default.aspx";
    }
    else {
        $("[id$=info_lbl]").val("Nastala chyba: " + result.result+"Kontaktujte admina...");
    }
    
}

$(document).ready(function () {
    //getData();
    $("[id$=passwd_txt]").on("keypress", function (e) {
        if (e.keyCode == 13) {
            //runLogin();
        }
       
    });

});