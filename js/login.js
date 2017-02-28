var sid = null;

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
    window.sid = result.sid;
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
    
    var passHash = CryptoJS.SHA3(pss);
    passHash = passHash.toString();


    $("[id$=passwd_txt]").val(passHash+sid);
    __doPostBack("login_btn", "login");
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
    
}

$(document).ready(function () {
    $("[id$=passwd_txt]").on("keypress", function (e) {
        if (e.keyCode == 13) {
            runLogin();
        }
       
    });

});