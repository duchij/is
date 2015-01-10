//function testik() {
//    alert("pokus");
//}

$(document).ready(function () {

    //alert("hura");

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