//Main Iesko JS script
/* options ={ 
                headers:key/value of headers to be set for the call
                responseType: string arraybuffer, blob, json, text
              
                source: the source which called the method
                callback: the method, to which the result is to be loaded
                args: arguments, in form of array or single parameter
                contentType: content type to be loaded eg image/jpeg
    method = GET,POST
    url = url of the REST call
                
*/

//options{headers,source,callback,responseType, args}
function xhrRawGetData(method, url, type, options)
{
    ///<summary> Gets Data from REST call </summary>
        

    // Attempt to creat the XHR2 object
    var xhr;

    try {
        xhr = new XMLHttpRequest();
    } catch (e) {
        try {
            xhr = new XDomainRequest();
        } catch (e) {
            try {
                xhr = new ActiveXObject('Msxml2.XMLHTTP');
            } catch (e) {
                try {
                    xhr = new ActiveXObject('Microsoft.XMLHTTP');
                } catch (e) {
                    alert('\nYour browser is not' +
                        ' compatible with XHR2');
                }
            }
        }
    }

    xhr.open(method, url, true);
    xhr.responseType = options.responseType;
    //xhr.
    // xhr.overrideMimeType("text\/plain; charset=x-user-defined");

    // xhr.setRequestHeader("Access-Control-Allow-Origin","*");
    var headers = options.headers;

    for (var key in headers) {
        xhr.setRequestHeader(key, headers[key]);
    }

    var _source = options.source;

   // xhr.setRequestHeader("X-Gallery-Request-Method", "get");
  //  xhr.setRequestHeader("X-Gallery-Request-Key", "de1ef9f8557883c3b7b012211c635518");
   // xhr.setRequestHeader("Content-type", "Image/JPG");
    //console.log(["type", type]);
    switch (type) {

        case "BINARY_DATA":
            xhr.onload = function () {

                if (this.status == 200) {

                    var blob = new Blob([xhr.response, { type: options.contentType }]);

                    var objectUrl = URL.createObjectURL(blob);

                   _source[options.callback](true, objectUrl, options.args);
                    //window.open(objectUrl);
                }
                else {
                   // console.log(xhr.responseText);
                    _source[options.callback](false, xhr.responseText, options.args);

                }
            }
            break;

        case "TEXT_DATA":
            
            xhr.onreadystatechange = function (e) {

                if (xhr.readyState == 4 && xhr.status == 200) {

                    var response = xhr.responseText;
                    var data;
                    try {

                        data = JSON.parse(response);

                    } catch (e) {

                        data = e.message;
                    }

                    _source[options.callback](true,data, options.args);
                }
                if (xhr.status != 200)
                {
                    _source[options.callback](false, xhr.responseText, options.args);
                }
            }


            break;
    

    
    }

    /* xhr.onreadystatechange = function (e) {
 
         if (xhr.readyState == 4 && xhr.status == 200) {
             // console.log(e.target);
 
             var dataLn = e.target.responseText.length;
             var array = [];
             for (var i = 0; i < dataLn; i++) {
                 array[i] = e.target.responseText.charCodeAt(1) & 0xff;
             }
 
            // var arrayBuffer = e.target.responseText.charCodeAt(x) & 0xff;
 
             console.log(["pole", atob(array)]);
 
            // var arrayBuffer = new Uint8Array(e.target.response);
             //var byteArray = new Uint8Array(arrayBuffer);
 
 
             var response = $.base64('encode', array);
             window[callback](response, args);
         }
     };*/

    xhr.send(null);
    // numberOfBLObsSent++;
};

function sendData(fnc,data,callBack,args)
{
    $("#commBall").css("display", "block");
    /// <summary>Funkcia na poslanie jQuery Ajaxu smerom na server</summary>
    /// <param name="fnc" type="String">Funkcia na strane serveru vo WebService.asmx</param>
    /// <param name="data" type="Object">JSON Objekt co sa ma poslat</param>
    /// <param name="callBack" type="String">Javascript funkcia kam sa to ma vratit</param>
    /// <param name="args" type="Mixed">Moze by objekt, pole, hocico co sa preposle do callback funkcie</param>
    $.ajax({
        url: "WebService.asmx/"+fnc,
        method: "POST",
        dataType: "text",
       // contentType: "application/json; charset=utf-8",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        // contentType: "application/x-www-form-urlencoded",
        //contentType: "text/html",
        data: "data=" + JSON.stringify(data),
        error: function (jqXHR,errorText) {
            messageBox(errorText, "error");
        },
        success: function (data) {
            //console.log(data);
            //showNewsText(data);
            var xml = $.parseXML(data);
            //console.log(xml);
            var dtJson = xml.childNodes[0].textContent;
            var obj = JSON.parse(dtJson);
            $("#commBall").css("display", "none");
            window[callBack](obj,args);
        }

    });
}


function sendData2(scriptFile, fnc, data, source, callBack, args) {
    $("#commBall").css("display", "block");
    /// <summary>Funkcia na poslanie jQuery Ajaxu smerom na server</summary>
    /// <param name="fnc" type="String">Funkcia na strane serveru vo WebService.asmx</param>
    /// <param name="data" type="Object">JSON Objekt co sa ma poslat</param>
    /// <param name="callBack" type="String">Javascript funkcia kam sa to ma vratit</param>
    /// <param name="args" type="Mixed">Moze by objekt, pole, hocico co sa preposle do callback funkcie</param>
    $.ajax({
        url: scriptFile+"/" + fnc,
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
            //console.log(data);
            //showNewsText(data);
            var xml = $.parseXML(data);
            //console.log(xml);
            var dtJson = xml.childNodes[0].textContent;
            var obj = JSON.parse(dtJson);
            $("#commBall").css("display", "none");
            window[callBack](obj, args);
        }

    });
}


function getDataFromUrl(url, options, data, source, callBack, args) {

    $.ajaxSetup({
        beforeSend: function (request) {
            request.setRequestHeader("X-Gallery-Request-Method", "get");
            request.setRequestHeader("X-Gallery-Request-Key", "de1ef9f8557883c3b7b012211c635518");
            request.setRequestHeader("Content-type", "Image/JPG");


        }
    });

    $("#commBall").css("display", "block");
    /// <summary>Funkcia na poslanie jQuery Ajaxu smerom na server</summary>
    /// <param name="fnc" type="String">Funkcia na strane serveru vo WebService.asmx</param>
    /// <param name="data" type="Object">JSON Objekt co sa ma poslat</param>
    /// <param name="callBack" type="String">Javascript funkcia kam sa to ma vratit</param>
    /// <param name="args" type="Mixed">Moze by objekt, pole, hocico co sa preposle do callback funkcie</param>
    $.ajax({
        url: url,
        method: options.method,
        dataType: options.dataType,
        headers: options.headers,
       /* beforeSend: function (request) {
            request.setRequestHeader("X-Gallery-Request-Method", "get");
            request.setRequestHeader("X-Gallery-Request-Key", "de1ef9f8557883c3b7b012211c635518");
            request.setRequestHeader("Content-type", "Image/JPG");


        },*/
        
       
        contentType: options.contentType,
        //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        // contentType: "application/x-www-form-urlencoded",
        //contentType: "text/html",
        data: "data=" + JSON.stringify(data),
        error: function (jqXHR, errorText) {
            messageBox(errorText, "error");
        },
        success: function (data) {

            //console.log(data);
            //console.log(data);
            //showNewsText(data);
            //var xml = $.parseXML(data);
            //console.log(xml);
            //var dtJson = xml.childNodes[0].textContent;

            switch (this.dataType) {

                case "text":
                    var obj = JSON.parse(data);
                    $("#commBall").css("display", "none");
                    window[callBack](obj, args);
                    break;
                case "binary":
                    var inData = data.base64.encode(data);
                    window[callBack](inData, args);


            }

            
        }

    });
}


function sprintf (text, objArgs) {
    //var text = this;
    for (var key in objArgs) {
        var reg = new RegExp('{' + key + '}', 'gm');
        text = text.replace(reg, objArgs[key]);
    }
    return text;
}




function down() {
    var pos = $(this).offset(); // remember position
    $("#test_dl").css("position", "static");
    $("#test_dl").offset(pos);   // reset position
    $("#test_dl").attr("size", "15"); // open dropdown
    $("#test_dl").unbind("focus", down);
}
function up() {
    $("#test_dl").css("position", "static");
    $("#test_dl").attr("size", "1");  // close dropdown
    $("#test_dl").unbind("change", up);
}
function openDropdown(elementId) {
    $('#' + elementId).focus(down).blur(up).focus();
}



function saveNurseShifts(ddl) {
    var user_id = $("[id$=" + ddl + "]").val();
    var tmp = ddl.split("_");
    var deps = $("[id$=deps_dl]").val();
    var year = $("[id$=rok_cb]").val();
    var month = $("[id$=mesiac_cb]").val();

    var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2],deps:deps };

    this.sendData("saveNurseShifts", data, "afterSaveNurseShifts", ddl);
    
}



function saveDocShifts(ddl)
{
    var user_id = $("[id$=" + ddl + "]").val();
    var tmp = ddl.split("_");

    var year = $("[id$=rok_cb]").val();
    var month = $("[id$=mesiac_cb]").val();

    var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2] };

    this.sendData("saveDocShifts", data, "afterSaveKdhaoShifts");
}

function saveKDCHDocShifts(ddl) {
    var user_id = $("[id$=" + ddl + "]").val();
    var tmp = ddl.split("_");

    var year = $("[id$=rok_cb]").val();
    var month = $("[id$=mesiac_cb]").val();

    var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2] };

    this.sendData("saveKDCHDocShifts", data, "afterSaveKDCHShifts");
}

function saveAllDocShifts(ddl) {
    var user_id = $("[id$=" + ddl + "]").val();
    var tmp = ddl.split("_");

    var year = $("[id$=rok_cb]").val();
    var month = $("[id$=mesiac_cb]").val();

    var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2] };
    console.log(data);
    this.sendData("saveDocShifts", data, "afterSaveAllShifts",ddl);
}




function saveAllDocShiftComment(comment)
{
    var note =  $("[id$=" + comment + "]").val();
    var tmp = comment.split("_");
    var user_id = $("[id$=ddl_" + tmp[1]+"_"+tmp[2] + "]").val();

    var year = $("[id$=rok_cb]").val();
    var month = $("[id$=mesiac_cb]").val();
   
    var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2],comment:note };

    this.sendData("saveDocShiftsComment", data, "afterSaveKdhaoShifts");
}


function saveKDCHDocShiftComment(comment) {
    var note = $("[id$=" + comment + "]").val();
    if (note.trim().length == 0)
    {
        note = "-";
    }
    var tmp = comment.split("_");

    var user_id = $("[id$=ddl_" + tmp[1] + "_" + tmp[2] + "]").val();
    
    var month = $("[id$=mesiac_cb]").val();
    var year = $("[id$=rok_cb]").val();
    var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2], comment: note };

    this.sendData("saveKDCHDocShiftsComment", data, "afterSaveKDCHShifts");
    
}


function saveAllDocShiftComment(comment) {
    var note = $("[id$=" + comment + "]").val();
    if (note.trim().length == 0) {
        note = "-";
    }
    var tmp = comment.split("_");

    var user_id = $("[id$=ddl_" + tmp[1] + "_" + tmp[2] + "]").val();

    var month = $("[id$=mesiac_cb]").val();
    var year = $("[id$=rok_cb]").val();
    var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2], comment: note };

    this.sendData("saveDocShiftsComment", data, "afterSaveAllShiftsComment",comment);

}



function afterSaveKDCHShifts(result)
{
    if (result.status == "false") {
        $("[id$=msg_dialog]").html("<h2 class='red'>CHYBA:</h2><p class='red'>" + result.msg + "</p>");
        $("[id$=msg_dialog]").dialog();
        //$("[id$=" + obj + "]").val("0");
    }
}

function afterSaveAllShiftsComment(result, comment) {

    if (result.status == "false") {

        $("[id$=" + comment + "]").val("-");
        $("[id$=msg_dialog]").html("<h2 class='red'>CHYBA:</h2><p class='red'>" + result.msg + "</p>");
        $("[id$=msg_dialog]").dialog();
        //$("[id$=" + obj + "]").val("0");
    }
}

function afterSaveAllShifts(result,ddl) {

    if (result.status == "false") {

        if (result.user_id != undefined)
        {
            $("[id$=" + ddl + "] option[value='"+result.user_id+"']").prop("selected",true);
        }
        else {
            $("[id$=" + ddl + "]").val("0");
        }
        
        $("[id$=msg_dialog]").html("<h2 class='red'>CHYBA:</h2><p class='red'>" + result.msg + "</p>");
        $("[id$=msg_dialog]").dialog();
        //$("[id$=" + obj + "]").val("0");
    }
}


function messageBox(text,type)
{
    switch (type)
    {
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



function deleteNurseActivity(id)
{
    alert(id);
    var st = confirm("Naozaj zmazať danú dovolenku?");
    if (st)
    {
        this.sendData("deleteNurseActivity", { dovId: id }, "afterActivityDelete");
    }

}

function afterActivityDelete(result) 
{
    if (result.status=="false")
    {
        this.messageBox(result.msg, "warning");
    }
    else
    {
        window.location("dovolenky_sestr.aspx");
    }
    
}



function saveNurseShiftComment(comment)
{
    var note = $("[id$=" + comment + "]").val();
    var tmp = comment.split("_");
    var user_id = $("[id$=ddl_" + tmp[1] + "_" + tmp[2] + "]").val();
    var deps = $("[id$=deps_dl]").val();
    var year = $("[id$=rok_cb]").val();
    var month = $("[id$=mesiac_cb]").val();
    note = note.trim();




    if (note.length > 0 && user_id!="0")
    {
        var data = { user_id: user_id, date: year + "-" + month + "-" + tmp[1], type: tmp[2], comment: note, deps: deps };
        this.sendData("saveNurseShiftsComment", data, "afterSaveNurseShifts", comment);
    }
    else
    {
        $("[id$=" + comment + "]").val("-");
        messageBox("Prazdny retazec, alebo sluzba nema priradeneho pracovnika", "warning");
        
    }

    
}


function afterSaveNurseShifts(result,obj)
{
    if (result.status == "false")
    {
        $("[id$=msg_dialog]").html("<h2 class='red'>CHYBA:</h2><p class='red'>"+result.msg+"</p>");
        $("[id$=msg_dialog]").dialog();
        $("[id$=" + obj + "]").val("0");
    }
}


function afterSaveKdhaoShifts(result)
{
    if (result.status=="false")
    {
        alert(result.msg);
    }
}




function lfTabs() {

    $("#lf_tabs").tabs();

    $("#lf_tabs").tabs({
        activate: function (event) {

            var tab = event.currentTarget.hash;
 //           console.log(tab);

            sendData("lfSelectedTab", { selTab: tab }, "afterLfSelectTab");
        }
    });


    var selTab = $("input[id$=setlftab_hv]").val();

    if (selTab != undefined && selTab.indexOf("lf_tab" != -1)) {
        switch (selTab) {
            case "#lf_tab1":    
                $("#lf_tabs").tabs({ active: 0 });
                break;
            case "#lf_tab2":
                $("#lf_tabs").tabs({ active: 1 });
                break;
            case "#lf_tab3":
                $("#lf_tabs").tabs({ active: 2 });
                break;
        }
    }

}


function nkimTabs() {

    $("#nkim_hlasko_tabs").tabs();

    $("#nkim_hlasko_tabs").tabs({
        activate: function (event) {

            var tab = event.currentTarget.hash;
            //           console.log(tab);

            sendData("nkimHlaskoSelectedTab", { selTab: tab }, "afterNkimSelectTab");
        }
    });


    var selTab = $("input[id$=nkimHlasko_hv]").val();

    if (selTab != undefined && selTab.indexOf("nkim_hlasko_tab" != -1)) {
        switch (selTab) {
            case "#nkim_hlasko_tab1":
                $("#nkim_hlasko_tabs").tabs({ active: 0 });
                break;
            case "#nkim_hlasko_tab2":
                $("#nkim_hlasko_tabs").tabs({ active: 1 });
                break;
            case "#nkim_hlasko_tab3":
                $("#nkim_hlasko_tabs").tabs({ active: 2 });
                break;
        }
    }

}

function afterNkimSelectTab(data)
{
    var tab = data.selTab;
    $("input[id$=nkimHlasko_hv]").val(tab);
}


function afterLfSelectTab(data) {
    var tab = data.selTab;
    $("input[id$=setlftab_hv]").val(tab);
}




//Cast pre poziadavky sestier//

function savePoziadOfNurse(dlId)
{
    var dl = $("[id$=" + dlId + "]");
    var status = dl.val();

    switch (status) {

        case "yes":
            dl.attr("title", "Áno, tu chcem slúžiť!");
            break;
        case "no":
            dl.attr("title", "Nie, tu nechcem služiť!");
            break;
        case "do":
            dl.attr("title", "Plánujem dovolenku");
            break;
    }

    
    var year = $("[id$=years_dl]").val();
    var month = $("[id$=month_dl]").val();

    var tmp = dlId.split("_");

    var userId = tmp[1];
    var day = tmp[2];

    var data = {
        userId: userId,
        datum:year+"-"+month+"-"+day,
        status:status

    };
    console.log(data);
    sendData("saveNursePoziad", data, "afterSavePoziadNurse");

    //console.log(data);
}


function afterSavePoziadNurse(result)
{
    console.log(result);
}

//koniec casti pre poziadavky sesstier//

function setSeminar()
{
        var dt = $("input[id$=date_txt]").val();
    
        $("input[id$=date_txt]").datepicker();
        $("input[id$=date_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
        $("input[id$=date_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
        $("input[id$=date_txt]").datepicker("setDate", dt);
   // $("input[id$=date_txt]").datepicker("setDate", "yy-mm-dd");
}


function loadJsFileForClass(myClass) {
    var jsfile = myClass;

    if (jsfile === ".js") {
        return;
    }

    if (!checkClass(jsfile)) {
        var fileRef = document.createElement("script");
        fileRef.setAttribute("type", "text/javascript");
        fileRef.setAttribute("src", "js/" + jsfile);

        document.getElementsByTagName("head")[0].appendChild(fileRef);
    }
}

function checkClass(jsFile) {

    var head = document.getElementsByTagName("head")[0].children;
    for (var ele in head) {
        if (typeof head[ele] === "object") {
            if (head[ele].outerHTML.indexOf("jsFile") != -1) return true;
        }
    }
    return false;
}

function avh(message)
{
    console.log("****************************DEBUG*****************************");
    console.log(message);
    console.log("****************************END DEBUG*****************************")
}


$(document).ready(function () {

 
    lfTabs();
    nkimTabs();


    if ($("input[id$=isSeminarsPage]").val() == "1")
    {
        setSeminar();
    }

    $("#delPasswdBtn").click(function (e) {
        //alert("lal");
        $("input[id$=passwd_txt").val("");

    });

    $("#odd_dl").change(function (e) {
        var selectedValue = $("#odd_dl").val();
        alert(selectedValue);
        if (selectedValue == "indiksem")
        {
            alert("halo");
        }

    });

   

   

   //inicializacia js triedy
    if ($("input[id$=class_hv]").val() != undefined) {

        var loadClass = $("input[id$=class_hv]").val();
        loadJsFileForClass(loadClass+".js");
    }


});