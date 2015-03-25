$(document).ready(function () {

    $("input[id$=ean1_txt]").focusout(function () {
        //alert($("input[id$=ean1_txt]").val());
        var txt = $("input[id$=ean1_txt]").val();

        txt = txt.replace("´", "+");
        txt = txt.replace("ˇ", "+");

        $("input[id$=ean1_txt]").val(txt);
    });
    $("input[id$=ean2_txt]").focusout(function () {
        //alert($("input[id$=ean1_txt]").val());
        var txt = $("input[id$=ean1_txt]").val();

        txt = txt.replace("´", "+");
        txt = txt.replace("ˇ", "+");

        $("input[id$=ean1_txt]").val(txt);
    });
    $("input[id$=ean3_txt]").focusout(function () {
        //alert($("input[id$=ean1_txt]").val());
        var txt = $("input[id$=ean1_txt]").val();

        txt = txt.replace("´", "+");
        txt = txt.replace("ˇ", "+");

        $("input[id$=ean1_txt]").val(txt);
    });
    $("input[id$=ean4_txt]").focusout(function () {
        //alert($("input[id$=ean1_txt]").val());
        var txt = $("input[id$=ean1_txt]").val();

        txt = txt.replace("´", "+");
        txt = txt.replace("ˇ", "+");

        $("input[id$=ean1_txt]").val(txt);
    });


    $("input[id$=phrase_txt]").keyup(function (e) {

        var sv = $("select[id$=searchIn_dl]").children("option").filter(":selected").val();
        if (sv == "ean")
        {
            var txt = $("input[id$=phrase_txt]").val();
            //txt = txt.trim();
            // alert(txt);
            var keynum;

            if (window.event) { // IE					
                keynum = e.keyCode;
            } else
                if (e.which) { // Netscape/Firefox/Opera					
                    keynum = e.which;
                }

            var keyP = String.fromCharCode(keynum);


            if (keynum == 223) {
                // alert(keyP + "   " + keynum);
                txt = txt.replace("´", "+");
                txt = txt.replace("ˇ", "+");
                $("input[id$=phrase_txt]").val(txt);
            }

            if (keyP == "1") {

                txt = txt.replace("+", "1")
                $("input[id$=phrase_txt]").val(txt);
            }

            if (keyP == "2") {

                txt = txt.replace("ľ", "2")
                $("input[id$=phrase_txt]").val(txt);
            }

            if (keyP == "3") {

                txt = txt.replace("š", "3")
                $("input[id$=phrase_txt]").val(txt);
            }
            if (keyP == "4") {

                txt = txt.replace("č", "4")
                $("input[id$=phrase_txt]").val(txt);
            }
            if (keyP == "5") {

                txt = txt.replace("ť", "5")
                $("input[id$=phrase_txt]").val(txt);
            }
            if (keyP == "6") {

                txt = txt.replace("ž", "6")
                $("input[id$=phrase_txt]").val(txt);
            }
            if (keyP == "7") {

                txt = txt.replace("ý", "7")
                $("input[id$=phrase_txt]").val(txt);
            }
            if (keyP == "8") {

                txt = txt.replace("á", "8")
                $("input[id$=phrase_txt]").val(txt);
            }
            if (keyP == "9") {

                txt = txt.replace("í", "9")
                $("input[id$=phrase_txt]").val(txt);
            }
            if (keyP == "0") {

                txt = txt.replace("é", "0")
                $("input[id$=phrase_txt]").val(txt);
            }
        }

        
    });


    $("input[id$=ean1_txt]").keyup(function (e) {

        var txt = $("input[id$=ean1_txt]").val();
        //txt = txt.trim();
      // alert(txt);
        var keynum;

        if (window.event) { // IE					
            keynum = e.keyCode;
        } else
            if (e.which) { // Netscape/Firefox/Opera					
                keynum = e.which;
            }

        var keyP = String.fromCharCode(keynum);
      

       if (keynum == 223)
        {
           // alert(keyP + "   " + keynum);
            txt = txt.replace("´", "+");
            txt = txt.replace("ˇ", "+");
            $("input[id$=ean1_txt]").val(txt);
        }

        if (keyP == "1") {

            txt = txt.replace("+", "1")
            $("input[id$=ean1_txt]").val(txt);
        }
        
        if (keyP == "2") {
          
            txt = txt.replace("ľ","2")
            $("input[id$=ean1_txt]").val(txt);
        }

        if (keyP == "3") {

            txt = txt.replace("š", "3")
            $("input[id$=ean1_txt]").val(txt);
        }
        if (keyP == "4") {

            txt = txt.replace("č", "4")
            $("input[id$=ean1_txt]").val(txt);
        }
        if (keyP == "5") {

            txt = txt.replace("ť", "5")
            $("input[id$=ean1_txt]").val(txt);
        }
        if (keyP == "6") {

            txt = txt.replace("ž", "6")
            $("input[id$=ean1_txt]").val(txt);
        }
        if (keyP == "7") {

            txt = txt.replace("ý", "7")
            $("input[id$=ean1_txt]").val(txt);
        }
        if (keyP == "8") {

            txt = txt.replace("á", "8")
            $("input[id$=ean1_txt]").val(txt);
        }
        if (keyP == "9") {

            txt = txt.replace("í", "9")
            $("input[id$=ean1_txt]").val(txt);
        }
        if (keyP == "0") {

            txt = txt.replace("é", "0")
            $("input[id$=ean1_txt]").val(txt);
        }
    });

    $("input[id$=ean2_txt]").keyup(function (e) {

        var txt = $("input[id$=ean2_txt]").val();
        //txt = txt.trim();
        // alert(txt);
        var keynum;

        if (window.event) { // IE					
            keynum = e.keyCode;
        } else
            if (e.which) { // Netscape/Firefox/Opera					
                keynum = e.which;
            }

        var keyP = String.fromCharCode(keynum);


        if (keynum == 223) {
            // alert(keyP + "   " + keynum);
            txt = txt.replace("´", "+");
            txt = txt.replace("ˇ", "+");
            $("input[id$=ean2_txt]").val(txt);
        }

        if (keyP == "1") {

            txt = txt.replace("+", "1")
            $("input[id$=ean2_txt]").val(txt);
        }

        if (keyP == "2") {

            txt = txt.replace("ľ", "2")
            $("input[id$=ean2_txt]").val(txt);
        }

        if (keyP == "3") {

            txt = txt.replace("š", "3")
            $("input[id$=ean2_txt]").val(txt);
        }
        if (keyP == "4") {

            txt = txt.replace("č", "4")
            $("input[id$=ean2_txt]").val(txt);
        }
        if (keyP == "5") {

            txt = txt.replace("ť", "5")
            $("input[id$=ean2_txt]").val(txt);
        }
        if (keyP == "6") {

            txt = txt.replace("ž", "6")
            $("input[id$=ean2_txt]").val(txt);
        }
        if (keyP == "7") {

            txt = txt.replace("ý", "7")
            $("input[id$=ean2_txt]").val(txt);
        }
        if (keyP == "8") {

            txt = txt.replace("á", "8")
            $("input[id$=ean2_txt]").val(txt);
        }
        if (keyP == "9") {

            txt = txt.replace("í", "9")
            $("input[id$=ean2_txt]").val(txt);
        }
        if (keyP == "0") {

            txt = txt.replace("é", "0")
            $("input[id$=ean2_txt]").val(txt);
        }
    });


    $("input[id$=ean3_txt]").keyup(function (e) {

        var txt = $("input[id$=ean3_txt]").val();
        //txt = txt.trim();
        // alert(txt);
        var keynum;

        if (window.event) { // IE					
            keynum = e.keyCode;
        } else
            if (e.which) { // Netscape/Firefox/Opera					
                keynum = e.which;
            }

        var keyP = String.fromCharCode(keynum);


        if (keynum == 223) {
            // alert(keyP + "   " + keynum);
            txt = txt.replace("´", "+");
            txt = txt.replace("ˇ", "+");
            $("input[id$=ean3_txt]").val(txt);
        }

        if (keyP == "1") {

            txt = txt.replace("+", "1")
            $("input[id$=ean3_txt]").val(txt);
        }

        if (keyP == "2") {

            txt = txt.replace("ľ", "2")
            $("input[id$=ean3_txt]").val(txt);
        }

        if (keyP == "3") {

            txt = txt.replace("š", "3")
            $("input[id$=ean3_txt]").val(txt);
        }
        if (keyP == "4") {

            txt = txt.replace("č", "4")
            $("input[id$=ean3_txt]").val(txt);
        }
        if (keyP == "5") {

            txt = txt.replace("ť", "5")
            $("input[id$=ean3_txt]").val(txt);
        }
        if (keyP == "6") {

            txt = txt.replace("ž", "6")
            $("input[id$=ean3_txt]").val(txt);
        }
        if (keyP == "7") {

            txt = txt.replace("ý", "7")
            $("input[id$=ean3_txt]").val(txt);
        }
        if (keyP == "8") {

            txt = txt.replace("á", "8")
            $("input[id$=ean3_txt]").val(txt);
        }
        if (keyP == "9") {

            txt = txt.replace("í", "9")
            $("input[id$=ean3_txt]").val(txt);
        }
        if (keyP == "0") {

            txt = txt.replace("é", "0")
            $("input[id$=ean3_txt]").val(txt);
        }
    });

    $("input[id$=ean4_txt]").keyup(function (e) {

        var txt = $("input[id$=ean3_txt]").val();
        //txt = txt.trim();
        // alert(txt);
        var keynum;

        if (window.event) { // IE					
            keynum = e.keyCode;
        } else
            if (e.which) { // Netscape/Firefox/Opera					
                keynum = e.which;
            }

        var keyP = String.fromCharCode(keynum);


        if (keynum == 223) {
            // alert(keyP + "   " + keynum);
            txt = txt.replace("´", "+");
            txt = txt.replace("ˇ", "+");
            $("input[id$=ean4_txt]").val(txt);
        }

        if (keyP == "1") {

            txt = txt.replace("+", "1")
            $("input[id$=ean4_txt]").val(txt);
        }

        if (keyP == "2") {

            txt = txt.replace("ľ", "2")
            $("input[id$=ean4_txt]").val(txt);
        }

        if (keyP == "3") {

            txt = txt.replace("š", "3")
            $("input[id$=ean4_txt]").val(txt);
        }
        if (keyP == "4") {

            txt = txt.replace("č", "4")
            $("input[id$=ean4_txt]").val(txt);
        }
        if (keyP == "5") {

            txt = txt.replace("ť", "5")
            $("input[id$=ean4_txt]").val(txt);
        }
        if (keyP == "6") {

            txt = txt.replace("ž", "6")
            $("input[id$=ean4_txt]").val(txt);
        }
        if (keyP == "7") {

            txt = txt.replace("ý", "7")
            $("input[id$=ean4_txt]").val(txt);
        }
        if (keyP == "8") {

            txt = txt.replace("á", "8")
            $("input[id$=ean4_txt]").val(txt);
        }
        if (keyP == "9") {

            txt = txt.replace("í", "9")
            $("input[id$=ean4_txt]").val(txt);
        }
        if (keyP == "0") {

            txt = txt.replace("é", "0")
            $("input[id$=ean4_txt]").val(txt);
        }
    });

});
