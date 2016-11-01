function fileSelected() {

    var count = document.getElementById('fileToUpload').files.length;

    //document.getElementById('details').innerHTML = "";
    var html = "<div class='info message'";
    // console.log(document.getElementById('fileToUpload').files);

    for (var index = 0; index < count; index++) {

        var file = document.getElementById('fileToUpload').files[index];
        var fileSize = 0;

        if (file.size > 1024 * 1024) {

            fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';

        } else {
            fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
        }

        html += '<strong>Názov:</strong> ' + file.name + '<br><strong>Veľkosť:</strong> ' + fileSize + '<br><strong>Typ:</strong> ' + file.type;
    }
    html += "</div>";
    $("#details").html(html);

}

function uploadFile() {

    var patientName = $("input[id$=patientName]").val().trim();
    var photoDate = $("input[id$=photoDate]").val().trim();
    var diagnose = $("input[id$=diagnose_txt]").val().trim();
    var note = $("input[id$=note_txt]").val().trim();
    var binNum = $("input[id$=patientBinNum]").val().trim();

    var reg = new RegExp(/[a-zA-Z][0-9]{2,4}/gi);

    if (!reg.test(diagnose)) {
        alert("Prosím zadajte diagnózu vo formáte napr. K36...");
        return;
    }


    if (patientName.length == 0 || photoDate.length == 0 || diagnose == 0)
    {
        alert("Meno, dátum a diagnóza musia byť vyplnené...");
        return;
    }


    var fd = new FormData();
    var count = document.getElementById('fileToUpload').files.length;

    for (var index = 0; index < count; index++) {

        var file = document.getElementById('fileToUpload').files[index];

        fd.append(file.name, file);
        

    }

    fd.append("patientName", patientName);
    fd.append("diagnose", diagnose);
    fd.append("photoDate", photoDate);
    fd.append("note", note);
    fd.append("binNum", binNum);

   

   // return;
    try {

        var xhr;

        if (window.XMLHttpRequest) {
            xhr = new XMLHttpRequest();
        }
        else if (window.ActiveXObject) {
            try {
                xhr = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    xhr = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e) {
                }
            }
        }

        if (!xhr) {
            alert("Error no XMLHttpRequest or ActiveXObject possibility");
            return false;
        }

        xhr.upload.addEventListener("progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, false);
        xhr.open("POST", "upload.aspx");
        xhr.send(fd);

        
    } catch (exc) {

        throw new Error("No XMLHttpRequest or MS ActiveXMLHttp Possible");

    }
    

}

function uploadProgress(evt) {

    if (evt.lengthComputable) {

        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
        // document.getElementById('progress').innerHTML = percentComplete.toString() + '%';
        $("#progress").progressbar({
            value: percentComplete,
            change: function () {
                $("#progress-label").text("Stiahnuté: " + percentComplete.toString());
            },
            complete: function () {
                $("#progress-label").text("Hotovo...")
            }
        });

    }

    else {
        document.getElementById('progress').innerHTML = 'Nedá sa stanoviť....';

    }

}

function uploadComplete(evt) {

    /* This event is raised when the server send back a response */

    alert(evt.target.responseText);
    $("#details").html("");
    $("#progress-label").text("0");
    $("#progress").progressbar({ value: 0 });
   // console.log(evt);

}

function uploadFailed(evt) {

    alert("Vyskytla sa chyba pri nahrávaní súboru.");

}

function uploadCanceled(evt) {

    alert("Nahrávanie bolo prerušené, alebo vypadlo spojenie....");

}

function init()
{
    var dt = new Date();

    $("input[id$=photoDate]").datepicker();
    $("input[id$=photoDate]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=photoDate]").datepicker("setDate", dt);


    $("#progress").progressbar();
    $("#progress").progressbar("option", "max", 100);
}


$(document).ready(function () {
    
    init();

});