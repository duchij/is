function sendData(fnc, data, callBack, args) {
    $("#commBall").css("display", "block");
    /// <summary>Funkcia na poslanie jQuery Ajaxu smerom na server</summary>
    /// <param name="fnc" type="String">Funkcia na strane serveru vo WebService.asmx</param>
    /// <param name="data" type="Object">JSON Objekt co sa ma poslat</param>
    /// <param name="callBack" type="String">Javascript funkcia kam sa to ma vratit</param>
    /// <param name="args" type="Mixed">Moze by objekt, pole, hocico co sa preposle do callback funkcie</param>
    $.ajax({
        url: "plan.asmx/" + fnc,
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


function saveDayOfNurse(dlId)
{
    console.log(dlId);
    var shiftType = $("[id$=" + dlId + "]").val();

    switch (shiftType) {
        case "doex":
            deleteNurseVacation(dlId, shiftType);
            break;
        case "do":
            saveNurseVacation(dlId,shiftType);
            break;
            default:
                saveNurseShifts(dlId,shiftType);
            break;
    }
}

function deleteNurseVacation(dlId,vacType)
{
    var tmp = dlId.split("_");

    var year = $("[id$=years_dl]").val();
    var month = $("[id$=month_dl]").val();

    // var depData = $("[id$=deps_dl]").val();

    // var depIdf = depData.substring(0, depData.indexOf("_"));

    var data = { user_id: tmp[1], date: year + "-" + month + "-" + tmp[2], type: vacType };
    // console.log(data);
    this.sendData("deleteNurseVacation", data, "afterDeleteNurseVacation", dlId);
}

function afterDeleteNurseVacation(result,dlId)
{
    if (result.status == "true") {
        $("[id$=" + dlId + "] option[value='0']").prop("selected", true);
    }
    else {
        messageBox(result.msg, "error");
    }
}

function saveNurseVacation(dlId,vacType)
{
    var tmp = dlId.split("_");

    var year = $("[id$=years_dl]").val();
    var month = $("[id$=month_dl]").val();

   // var depData = $("[id$=deps_dl]").val();

   // var depIdf = depData.substring(0, depData.indexOf("_"));

    var data = { user_id: tmp[1], date: year + "-" + month + "-" + tmp[2], type: vacType};
    // console.log(data);
    this.sendData("saveNurseVacation", data, "afterSaveNurseVacation", dlId);
}

function afterSaveNurseVacation(result)
{
    if (result.status == "false") {
        messageBox(result.msg, "error");
    }
}


function  saveNurseShifts(dlId,shiftType)
{
    var tmp = dlId.split("_");

    var year = $("[id$=years_dl]").val();
    var month = $("[id$=month_dl]").val();

    var depData = $("[id$=deps_dl]").val();

    var depIdf = depData.substring(0, depData.indexOf("_"));

    var data = { user_id: tmp[1], date: year + "-" + month + "-" + tmp[2], type: shiftType, depIdf: depIdf };
    // console.log(data);
    this.sendData("saveNurseDay", data, "afterSaveNurseDay", dlId);
}

function afterSaveNurseDay(result,dlId) {

    if (result.status === "false") {
        messageBox(result.msg, "error");
    }
}

$(function () {
    $(document).tooltip();
});

$(document).ready(function () {

   
});