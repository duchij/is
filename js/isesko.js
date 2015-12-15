

function sendData(fnc,data,callBack)
{
    $.ajax({
        url: "WebService.asmx/"+fnc,
        method: "POST",
        dataType: "text",
       // contentType: "application/json; charset=utf-8",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        // contentType: "application/x-www-form-urlencoded",
        //contentType: "text/html",
        data: "data=" + JSON.stringify(data),
        success: function (data) {
            console.log(data);
            //showNewsText(data);
            var xml = $.parseXML(data);
            var dtJson = xml.childNodes[0].innerHTML;
            var obj = JSON.parse(dtJson);
            window[callBack](obj);
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



function hlaskoTabs()
{
    $("#hlasko_tabs").tabs();


    $("#hlasko_tabs").tabs({
        activate: function (event) {

            var tab = event.currentTarget.hash;
           // console.log(tab);

            sendData("hlaskoSelectedTab", { selTab: tab }, "afterSelectTab");
        }
    });

    var selTab = $("input[id$=hlaskoSelectedTab]").val();
    
    if (selTab!= undefined && selTab.indexOf("hlasko_tab" != -1)) {
        switch (selTab) {
            case "#hlasko_tab1":
                $("#hlasko_tabs").tabs({ active: 0 });
                break;
            case "#hlasko_tab2":
                $("#hlasko_tabs").tabs({ active: 1 });
                break;
            case "#hlasko_tab3":
                $("#hlasko_tabs").tabs({ active: 2 });
                break;
        }
    }
}

function opKnihaTabs()
{
    
    $("#opkniha_tabs").tabs();

    $("#opkniha_tabs").tabs({
        activate: function (event) {

            var tab = event.currentTarget.hash;
           // console.log(tab);

            sendData("opknihaSelectedTab", { selTab: tab }, "afterOpKnihaSelectTab");
        }
    });


}

function lfTabs() {

    $("#lf_tabs").tabs();

    $("#lf_tabs").tabs({
        activate: function (event) {

            var tab = event.currentTarget.hash;
            console.log(tab);

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

function afterLfSelectTab(data) {
    var tab = data.selTab;
    $("input[id$=setlftab_hv]").val(tab);
}


function afterSelectTab(data)
{
    var tab = data.selTab;
    $("input[id$=hlaskoSelectedTab]").val(tab);
}

function setSeminar()
{
        var dt = $("input[id$=date_txt]").val();
    
        $("input[id$=date_txt]").datepicker();
        $("input[id$=date_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
        $("input[id$=date_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
        $("input[id$=date_txt]").datepicker("setDate", dt);
   // $("input[id$=date_txt]").datepicker("setDate", "yy-mm-dd");
}

$(document).ready(function () {

    hlaskoTabs();
    opKnihaTabs();
    lfTabs();


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


});