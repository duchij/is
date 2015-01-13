using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class lf : System.Web.UI.Page
{
    mysql_db x2Mysql = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(Request.QueryString["id"]);
        this.downloadFile(id);
    }

    protected void downloadFile(int id)
    {
        SortedList lfData = x2Mysql.getLfData(id);
       // byte[] lfContent = Convert.FromBase64String(x2Mysql.getLfContent(id).ToString());
        byte[] lfContent = Convert.FromBase64String(lfData["file-content"].ToString());
        

        Response.Clear();
        Response.Buffer = true;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.ContentType =lfData["file-type"].ToString();
        
        Response.AddHeader("content-disposition", "attachment;filename=" + lfData["file-name"].ToString());
        Response.AddHeader("content-length", lfData["file-size"].ToString());
        //Stream dat = data["file-content"];
        //byte[] mData = Encoding.Default.GetBytes(data["file-content"].ToString(),0,Convert.ToInt32(data["file-size"]));
        Response.BinaryWrite(lfContent);
       // Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1250");
       // Response.Charset = "Windows-1250";
        //StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used

        //HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
        //this.RenderControl(htmlTextWriter);
        //Response.Write(stringWriter.ToString());
        Response.Flush();
        Response.End();
    }
}