console.log("INIT HLASKO JS MODULE");
var _this = this;
var module = "controls/js_hlasko.asmx";

_this.initHlasko();

function hlaskoTabs() {

    try{
        $("#hlasko_tabs").tabs();


        $("#hlasko_tabs").tabs({
            activate: function (event) {

                var tab = event.currentTarget.hash;
                // console.log(tab);

                sendData("hlaskoSelectedTab", { selTab: tab }, "afterSelectTab");
            }
        });

        var selTab = $("input[id$=hlaskoSelectedTab]").val();

        if (selTab != undefined && selTab.indexOf("hlasko_tab") != -1) {
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
    catch (err) {
        avh(err);
    }

   
}


function afterSelectTab(data) {
    var tab = data.selTab;
    $("input[id$=hlaskoSelectedTab]").val(tab);
}


function loadOddHlasko() {
    return false;
}

function hlaskoButtons() {

    $("[id$=showOddA_btn]").css("cursor", "pointer");
    $("[id$=showOddB_btn]").css("cursor", "pointer");
    $("[id$=showOup_btn]").css("cursor", "pointer");
    $("[id$=showOp_btn]").css("cursor", "pointer");

    $("[id$=showOddA_btn]").click(function (e) {

        var dt = $("[id$=date_hv]").val();
        sendData2(_this.module, "loadHlasenie", { date: dt,dep:"OddA" }, _this, "showHlasenie");

    });

    

    $("[id$=showOddB_btn]").click(function (e) {

        var dt = $("[id$=date_hv]").val();
        sendData2(_this.module, "loadHlasenie", { date: dt, dep: "OddB" }, _this, "showHlasenie");

    });


    $("[id$=showOup_btn]").click(function (e) {

        var dt = $("[id$=date_hv]").val();
        sendData2(_this.module, "loadHlasenie", { date: dt, dep: "OUP" }, _this, "showHlasenie");

    });

    $("[id$=showOp_btn]").click(function (e) {

        var dt = $("[id$=date_hv]").val();
        sendData2(_this.module, "loadHlasenie", { date: dt, dep: "OP" }, _this, "showHlasenie");

    });

}


function showHlasenie(result) {
    
    if (result.status == "true") {
        tinymce.activeEditor.execCommand('mceSetContent', false, result.result);
    }
    else {
        $("[id$=msg_lbl]").val("<p class='res'>Chyba: "+result.result+"</p>");
    }
    

}



function initHlasko() {


    _this.hlaskoTabs();

    hlaskoButtons();



   


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
        var re = new RegExp("^([01]?[0-9]|2[0-3]):[0-5][0-9]$", "ig");
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

    
}