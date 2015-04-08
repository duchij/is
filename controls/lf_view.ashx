<%@ WebHandler Language="C#" Class="lf_view" %>

using System;
using System.Web;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Generic;

public class lf_view : IHttpHandler {

    mysql_db x2Mysql = new mysql_db();
    
    public void ProcessRequest (HttpContext context) {
        
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
                    
            }
            byte[] lfData = x2Mysql.lfStoredData(picId, Convert.ToInt32(picData["file-size"]));
            context.Response.BinaryWrite(lfData); 
            
            
        }
        
       
        
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}