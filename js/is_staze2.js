function setClasses() {
    var dt = $("input[id$=classes_date_txt]").val();
    //alert(dt);
    $("input[id$=classes_date_txt]").datepicker();
    $("input[id$=classes_date_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=classes_date_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    //$("input[id$=classes_date_txt]").datepicker("setDate");

    $("input[id$=classes_date_txt]").datepicker("setDate", dt);


   
}

function checkHourFormat(id)
{
    var time = $("input[id$=" + id + "]").val();

    var re = new RegExp("^([01]?[0-9]|2[0-3]):[0-5][0-9]$", "ig");
    if (!re.test(time)) {
        var dt = new Date();
        var min = dt.getMinutes();
        var minStr = "";
        if (min >= 0 || min <= 9) {
            minStr = "0" + min;
        }
        else {
            minStr = min.toString();
        }

        $("input[id$="+id+"]").val(dt.getHours() + ":" + dt.getMinutes());
        alert("Chyba!!! toto nie je spravny format casu!!!! Nastaveny bude aktualny");
    }
    
}

$(document).ready(function () {

    if ($("#dypage").val() == "staze2") {

        setClasses();

    }
});

