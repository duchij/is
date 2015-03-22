$(document).ready(function () {

    $("input[id$=ean1_txt]").keyup(function (e) {

        var txt = $("input[id$=ean1_txt]").val();
      // alert(txt);
        var keynum;

        if (window.event) { // IE					
            keynum = e.keyCode;
        } else
            if (e.which) { // Netscape/Firefox/Opera					
                keynum = e.which;
            }

        //var keyP = String.fromCharCode(keynum);

        //alert(",,,,,,,"+keyP);

        var keyP = String.fromCharCode(keynum);
        alert("..."+keyP);
        if (txt.length == 0)
        {
            $("input[id$=ean1_txt]").val(keyP)
        }
        else {
            var tmp = txt.substring(txt.length-1,1)
            $("input[id$=ean1_txt]").val(tmp+keyP)
        }
       
    });

});
