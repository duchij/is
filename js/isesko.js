//function testik() {
//    alert("pokus");
//}



function onSuccess(data) {
    alert(data);
}

function showNewsInfo(text)
{
    alert(text);
}

function testFnc()
{
    loadNews("test");
}

function test() {
    //alert("lolo");
   // var st = $("input[id$=testButton").val();
    var st = "test";
    $.ajax({
        type: "POST",
        url: "../WebService.asmx/loadNews",
        data: "text=halo",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Content-type",
                                 "application/json; charset=utf-8");
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // success: function (response) { alert("uspech"+ response); },
        failure: function (response) {
            alert("chyba"+response);
        },
        complete: function (res) {alert(res);}
    });
}

function checkTab(tab)
{
    alert(tab);
    
}

function pisanie2(ele)
{
    //var text = $("#pisanie_txt").val(); 
    $("#test_dl").focus(down);
}

function pisanie()
{
    var text = $("#pisanie_txt").val();
    $.ajax({
        url: "WebService.asmx/loadData",
        method: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        // contentType: "application/x-www-form-urlencoded",
        //contentType: "text/html",
        data: "{text:'"+text+"'}",
        success: function (data) {
            //showNewsText(data);
            console.log(data);
        }

    });


}


function printTable() {
    var DocumentContainer = document.getElementById('shiftTable');
    console.log(DocumentContainer.innerHTML);
    var WindowObject = window.open('', 'PrintWindow', 'width=750,height=650,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes');
    var strHtml = "<html>\n<head>\n<link rel='stylesheet' type='text/css' href='print.css'>\n</head><body>\n<div style='testStyle' documentcontainer.innerhtml='' mode='hold' />         WindowObject.document.writeln(strHtml)";
    WindowObject.document.close();
    WindowObject.focus();
    // WindowObject.print();
    // WindowObject.close();
    //alert(DocumentContainer);
}

function loadNews(id)
{
    var news = { newsId: 'lalal', newsText: 'toto je pokus' };
    $.ajax({
        url: "WebService.asmx/loadNews",
        method: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        // contentType: "application/x-www-form-urlencoded",
        //contentType: "text/html",
        data:  "{data:'"+JSON.stringify(news)+"'}",
        success: function (data) {
            //showNewsText(data);
            console.log(data);
        }

    });

    
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

$(document).ready(function () {

    

    //var news = $("input[id$=newsDialogShow]").val().toString();

    //if (news.length >0) {
    //    loadNews(news);
    //}

    $("#tabs").tabs();

    var stab = $("input[id$=_settab_hv]").val();
    if (stab != undefined && stab.length > 0) {
        //alert(stab);
        $("#tabs").tabs({active:1});
    }

    //$("select[id$=test_dl]").on("keyup", function (e) {
    //    pisanie2($(this));
        
    //});

    $("#test_btn").click(function () {
       // alert("hura");
        var request = $.ajax({
            url: "WebService.asmx/loadNews",
            method: "POST",
            dataType: "text",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            //contentType: "application/x-www-form-urlencoded",
            //contentType: "text/plain",
            data:"data="+JSON.stringify({test: "lala", pokus: "husra" }),
            succes:function(data){
                console.log($.parseXML(data));
            },
            error: function (data) {
                console.log(data);
            }
          
        });
        //request.success(function (data) {
        //    console.log(data);
        //});

        request.done(function (data) {
            var xml = $.parseXML(data)
            console.log(xml.childNodes[0].innerHTML);

            
            //alert(data);
        });

        //request.success(function (data) {
        //    console.log(data);
        //    alert(data);
        //});

    });

    //alert("hura");

    //$("input[id$=testButton").click(function (e) {
    //    //var st = false;
    //    //if ($("input[id$=_publish_cb]").attr("checked")) {
    //    //    st = true;
    //    //}
            
    //    var st = $("input[id$=_publish_cb]").is(":checked");

    //    //if (st) {
    //    alert("lolo");


    //    $.ajax({
    //        type: "POST",
    //        url: "hlasko.aspx/setData",
    //        data: '{state: "' +st + '" }',
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (response) { alert(response.d); },

    //        failure: function (response) {
    //            alert(response.d);
    //        }
    //    });
    //    // }


    //});

    //$("input[id$=_dovNameCell_]").mouseover(function (e) {
    //    alert("lolo");
    //});

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

    $("select[id$=_worktype_cb]").change(function (e) {

        var selectedValue = $("select[id$=_worktype_cb]").val();

        if (selectedValue == "urgent") {
            $("input[id$=_patientname_txt]").val("Osetr. pacienti");
            $("input[id$=_jsWorktimetxt]").val("1440");
            $("input[id$=_jsWorkstarttxt]").val("07:00");
        }

        // alert(selectedValue);
    });

    $("input[id$=_jsWorkstarttxt]").change(function (e) {

        var str = $("input[id$=_jsWorkstarttxt]").val();
        var re = new RegExp("^([01]?[0-9]|2[0-3]):[0-5][0-9]","ig");
        //alert(str);
        if (!re.test(str)) {
            var dt = new Date();
            var min = dt.getMinutes();
            var minStr = "";
            if (min >= 0 || min <= 9) {
                minStr = "0" + min;
            }
            else {
                minStr = min.toString();
            }

                $("input[id$=_jsWorkstarttxt]").val(dt.getHours() + ":" + dt.getMinutes());
            alert("Chyba!!! toto nie je spravny format casu!!!! Nastaveny bude aktualny");
        }

    });

    $("input[id$=_jsWorktimetxt]").change(function (e) {
        var str = $("input[id$=_jsWorktimetxt]").val();
        // alert(Number(str));
        if (isNaN(str)) {
            $("input[id$=_jsWorktimetxt]").val("15");
            alert("Toto nie je cele cislo!!!!");
        }
    });


    //$("input[id$=_jsWorskstarttxt]").on('change', ':input', function () {
    //    alert("lo");
    //});


    //$("input[id$=_jsTextBox1]").change(function (e) {
    //    var str = $("input[id$=_jsTextBox1]").val();
    //    alert(str);
    //});

});