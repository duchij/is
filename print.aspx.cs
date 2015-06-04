using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html;
//using iTextSharp.text.html.simpleparser;

public partial class hlasenia_print : System.Web.UI.Page
{
    // sluzbyclass x_db = new sluzbyclass();
    x2_var my_x2 = new x2_var();
    mysql_db x2MySql = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.datum_lbl.Text = Convert.ToDateTime(Session["hlasko_date"]).ToLongDateString();
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_hlasko] WHERE [id] ='{0}'", Session["akt_hlasenie"]);

        SortedList row = x2MySql.getRow(sb.ToString());

        this.type_lbl.Text = row["type"].ToString();
        this.hlas_lbl.Text = my_x2.DecryptString(row["text"].ToString(), Session["passphrase"].ToString()); 
        this.user_lbl.Text = Session["fullname"].ToString();

        DateTime datum = Convert.ToDateTime(Session["hlasko_date"]);
        sb.Length = 0;

        string klinika = Session["klinika"].ToString().ToLower();

        switch (klinika)
        {
            case "kdch":
                sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
                sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names]");
                sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
                sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
                sb.AppendFormat("WHERE [t_sluzb].[datum]='{0}'", my_x2.unixDate(datum));
                sb.Append("GROUP BY [t_sluzb].[datum]");
                break;
            case "2dk":
                sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
                sb.Append("GROUP_CONCAT(IFNULL([t_users].[name3],'-') ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names]");
                sb.Append("FROM [is_sluzby_dk] AS [t_sluzb]");
                sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
                sb.AppendFormat("WHERE [t_sluzb].[datum]='{0}'", my_x2.unixDate(datum));
                sb.Append("GROUP BY [t_sluzb].[datum]");
                break;
            case "nkim":
                sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
                sb.Append("GROUP_CONCAT(IFNULL([t_users].[name3],'-') ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names]");
                sb.Append("FROM [is_sluzby_all] AS [t_sluzb]");
                sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
                sb.AppendFormat("WHERE [t_sluzb].[datum]='{0}'", my_x2.unixDate(datum));
                sb.Append("GROUP BY [t_sluzb].[datum]");
                break;
                
        }

        row = x2MySql.getRow(sb.ToString());

       // SortedList lekari = this.getSluzbyByDen(Convert.ToInt32(Request["den"].ToString()));
        string[] names = row["users_names"].ToString().Split(';');
        string[] types = row["type1"].ToString().Split(';');

        int colsLn = names.Length;

        TableRow riadok = new TableRow();
        this.report_tbl.Controls.Add(riadok);

        for (int col = 0; col < colsLn; col++ )
        {
            TableCell dataCell = new TableCell();
            dataCell.Text = "<strong>" + types[col] + "</strong>: " + names[col];
            riadok.Controls.Add(dataCell);
        }


            /*this.oup_lbl.Text = names[0];
            this.odda_lbl.Text = names[1];
            this.oddb_lbl.Text = names[2];
            this.op_lbl.Text = names[3];*/

            if (Convert.ToBoolean(Session["hlasko_toWord"]) == true)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/msword";
                Response.AddHeader("content-disposition", "attachment;filename=hlasko.doc");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                Response.Charset = "windows-1250";
                StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

                this.RenderControl(htmlTextWriter);
                Response.Write(stringWriter.ToString());
                Response.End();
            }

        //if (Session["pdf"] == "print")
        //{
        //    Document myDoc = new Document(PageSize.A4);
        //    MemoryStream ms = new MemoryStream();

        //    PdfWriter writer = PdfWriter.GetInstance(myDoc, ms);

        //    StringWriter stringWriter = new StringWriter();

        //    HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
        //    //form1.re

        //    int contr = form1.Controls.Count;

        //    //for (int i = 0; i < contr; i++)
        //    //{
        //        form1.Controls[1].RenderControl(htmlTextWriter);
        //    //}

        //   string html = stringWriter.ToString();
        //    //msg_lbl.Text = "hus"+html;

        //    StringReader se = new StringReader(html);

        //    HTMLWorker obj = new HTMLWorker(myDoc);
           
        //    myDoc.Open();
        //    myDoc.NewPage();

        //    obj.Parse(se);
            
        //    myDoc.Close();

        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=report.pdf");
        //    Response.ContentType = "application/pdf";

        //    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        //    Response.OutputStream.Flush();
        //}
    }

   
}
