var _this = this;

var galThumbUrl = "http://10.10.2.83/gallery3/index.php/rest/data/{path}?size=thumb";
var galPicUrl = "http://10.10.2.83/gallery3/index.php/rest/data/{path}?size=full";

var galHeaders = {

    "X-Gallery-Request-Method":"get",
    "X-Gallery-Request-Key":"de1ef9f8557883c3b7b012211c635518",
    "Content-type":"Image/JPG"
};




    


_this.opKnihaInit();


function opKnihaTabs() {
    $("#opkniha_tabs").tabs();
    // var tabs = $("#opkniha_tabs").tabs("option","active");

    //console.log(["lo",tabs]);

    try {

        $("#opkniha_tabs").tabs({
            activate: function (event) {

                var tab = event.currentTarget.hash;
                //console.log(tab);

                sendData("opknihaSelectedTab", { selTab: tab }, "afterOpKnihaSelectTab");
            }
        });

        var selTab = $("input[id$=opknihaTab_hv]").val();
        // console.log(selTab);
        if (selTab != undefined && selTab.indexOf("opkniha_tab") != -1)
            switch (selTab) {
                case "#opkniha_tab1":
                    $("#opkniha_tabs").tabs({ active: 0 });
                    break;
                case "#opkniha_tab2":
                    $("#opkniha_tabs").tabs({ active: 1 });
                    break;
                case "#opkniha_tab3":
                    $("#opkniha_tabs").tabs({ active: 2 });
                    break;
                case "#opkniha_tab4":
                    $("#opkniha_tabs").tabs({ active: 3 });
                    break;
            }


    }
    catch (ex) {
        avh(ex.message);
    }
}


function getGalleryData() {

    //console.log("Sending data");

    xhrRawGetData("GET", "http://192.168.56.1/dapp/index.php?d=03032009","TEXT_DATA",{
    
        headers: {},
        responseType: "text",
        source: _this,
        callback: "afterGetData",
        args: [],
        contentType:"image/jpeg"
    
    
    });

   

}





function afterGetData(status,result,args) {

    console.log(["odpoved", status, result, args]);


    if (!status) {
        messabeBox(result, "error");
        return;
    }

    for (var row in result) {

        this.getThumbData(result[row]);

    }

}

function getThumbData(data)
{
   // console.log(data);

    var url = sprintf(this.galThumbUrl, { path: data.id });
   // return;

    xhrRawGetData("GET", url, "BINARY_DATA", {

        headers: this.galHeaders,
        responseType: "arraybuffer",
        source: _this,
        callback: "appendThumb",
        args: data,
        contentType: "image/jpeg"


    });
}

function appendThumb(status,urlObj,data)
{
    var thumbData = `
        <div style="border:1px solid lightgrey;width:auto;padding:3px;float:left;">
           <p> <span class ="blue medium">{imageName}</span></p>
          <a href="javascript:loadPicture('{picId}');">  <img src="{path}" alt="{imageName}"/></a>
        </div>

    `;
    var img = sprintf(thumbData, { path: urlObj,imageName:data.path,picId:data.id });

    $("#contentGalPics").append(img);
}

function loadPicture(id) {
    console.log(id);


    var url = sprintf(this.galPicUrl, { path: id});
    // return;

    xhrRawGetData("GET", url, "BINARY_DATA", {

        headers: this.galHeaders,
        responseType: "arraybuffer",
        source: _this,
        callback: "showPicture",
        args: [],
        contentType: "image/jpeg"


    });

}
function showPicture(status, result, args) {

   //window.tar
    //window.location = result;
   // window.document.write("Naspat");

    var picUrl = sprintf("<p class='asphalt'>Kliknúť pravým tlačidlo myši na obrázok a následne uložiť obrázok</p><img src='{url}' alt='fullpicture'/>", { url: result });


   // window.document.write(picUrl);

    $("#gDialog").dialog("open");
    
   
    $("#gDialog").html(picUrl);
   


}

function afterOpKnihaSelectTab(data) {
    var tab = data.selTab;
    $("input[id$=opknihaTab_hv]").val(tab);
}


function checkUrl()
{
    var url = window.location.href;
    var tmp = url.split("?");

    if (tmp.length > 1) {
        var args = tmp[1].split("&");


        if (args[0] == "m=loadGalleryData") {
            $("#opkniha_tabs").tabs({ active: 3 });
        }
    }
}


function opKnihaInit() {
  
    console.log("INIT OP KNIHA");


   
    $("#gDialog").dialog({
        heigth: 600,
        width: 800,
        modal: true,
        draggable: true,
        autoOpen:false
        
        // position: {my:"left top",at:"left top",of:"gDialogStart"}
    });
    
    $("#gDialog").dialog();

    

    _this.opKnihaTabs();

    _this.checkUrl();

   

    var dt = $("input[id$=opFrom_txt]").val();
    $("input[id$=opFrom_txt]").datepicker();
    $("input[id$=opFrom_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=opFrom_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=opFrom_txt]").val(dt);


    var dt = $("input[id$=opTo_txt]").val();
    $("input[id$=opTo_txt]").datepicker();
    $("input[id$=opTo_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=opTo_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=opTo_txt]").val(dt);

    var dt = $("input[id$=dgFrom_txt]").val();
    $("input[id$=dgFrom_txt]").datepicker();
    $("input[id$=dgFrom_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=dgFrom_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=dgFrom_txt]").val(dt);

    var dt = $("input[id$=dgTo_txt]").val();
    $("input[id$=dgTo_txt]").datepicker();
    $("input[id$=dgTo_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=dgTo_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=dgTo_txt]").val(dt);

    var dt = $("input[id$=myFrom_txt]").val();
    $("input[id$=myFrom_txt]").datepicker();
    $("input[id$=myFrom_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=myFrom_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=myFrom_txt]").val(dt);

    var dt = $("input[id$=myTo_txt]").val();
    $("input[id$=myTo_txt]").datepicker();
    $("input[id$=myTo_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=myTo_txt]").datepicker("option", "dateFormat", "yy-mm-dd");
    $("input[id$=myTo_txt]").val(dt);

    var dt = $("input[id$=galOpDate_txt]").val();
    $("input[id$=galOpDate_txt]").datepicker();
    $("input[id$=galOpDate_txt]").datepicker("option", "dateFormat", "ddmmyy");
    $("input[id$=galOpDate_txt]").datepicker("option", "dateFormat", "ddmmyy");
    $("input[id$=galOpDate_txt]").val(dt);

   
   
}


