<%@ WebHandler Language="C#" Class="lf_view" %>

using System;
using System.Web;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Generic;

public class lf_view : IHttpHandler {

    private mysql_db x2Mysql = new mysql_db();
    private x2_var X2 = new x2_var();

    public void ProcessRequest (HttpContext context) {


        if (context.Request.QueryString["galId"] != null)
        {
            this.loadDataFromGallery(context.Request.QueryString["galId"].ToString(), context);
        }


        if (context.Request.QueryString["id"] != null)
        {
            int picId = Convert.ToInt32(context.Request.QueryString["id"]);

            SortedList picData = x2Mysql.getLfData2(picId);
            switch (picData["file-type"].ToString().ToLower())
            {
                case "jpg":
                    context.Response.ContentType = "image/jpg";
                    break;
                case "png":
                    context.Response.ContentType = "image/png";
                    break;
                case "tiff":
                    context.Response.ContentType = "image/tiff";
                    break;
                case "gif":
                    context.Response.ContentType = "image/gif";
                    break;

            }
            byte[] lfData = x2Mysql.lfStoredData(picId, Convert.ToInt32(picData["file-size"]));
            context.Response.BinaryWrite(lfData);


        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private void loadDataFromGallery(string id,HttpContext context)
    {

        Dictionary<string, string> headerData = new Dictionary<string, string>();

        headerData["X-Gallery-Request-Method"] = "get";
        headerData["X-Gallery-Request-Key"] = "de1ef9f8557883c3b7b012211c635518";
        headerData["Content_type"] = "Image/JPG";

        CRest myCurl = new CRest();
        string url = string.Format(Resources.Resource.opkniha_gallery_url_picture, id);
        string data = myCurl._csCurl(url, "GET_BIN", headerData);

        byte[] arr = Convert.FromBase64String(data);

        context.Response.BinaryWrite(arr);




    }




}