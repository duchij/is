//function testik() {
//    alert("pokus");
//}

function onSuccess(data) {
    alert(data);
}

function test() {
    //alert("lolo");
   // var st = $("input[id$=testButton").val();
    var st = "test";
    $.ajax({
        type: "POST",
        url: "../WebService.asmx/setData",
        data: "{data:'text'}",
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

$(document).ready(function () {

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
            $("input[id$=_jsWorkstarttxt]").val(dt.getHours()+":"+dt.getMinutes()   );
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