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

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

public partial class hlasenia_print : System.Web.UI.Page
{
    sluzbyclass x_db = new sluzbyclass();
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            
            Response.Redirect("error.html");
        }
        datum_lbl.Text = Request["datum"].ToString();
       // msg_lbl.Visible = false;
        my_db x_db = new my_db();

        SortedList data = x_db.getDataByID("is_hlasko",Session["akt_hlasenie"].ToString());

        SortedList user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (data.Count > 1)
        {
            type_lbl.Text = data["type"].ToString();
            hlas_lbl.Text = data["text"].ToString();
            
        }
           

        user_lbl.Text = user_info["full_name"].ToString();
        SortedList lekari = this.getSluzbyByDen(Convert.ToInt32(Request["den"].ToString()));
        
        oup_lbl.Text = lekari["OUP"].ToString();
        odda_lbl.Text = lekari["OddA"].ToString();
        oddb_lbl.Text = lekari["OddB"].ToString();
        op_lbl.Text = lekari["OP"].ToString();

        if (Request["w"] == "1")
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

        if (Session["pdf"] == "print")
        {
          

            Document myDoc = new Document(PageSize.A4);
            MemoryStream ms = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(myDoc, ms);

            StringWriter stringWriter = new StringWriter();

            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
            //form1.re

            int contr = form1.Controls.Count;

            //for (int i = 0; i < contr; i++)
            //{
                form1.Controls[1].RenderControl(htmlTextWriter);
            //}

           string html = stringWriter.ToString();
            //msg_lbl.Text = "hus"+html;

            StringReader se = new StringReader(html);

            HTMLWorker obj = new HTMLWorker(myDoc);
           
            myDoc.Open();
            myDoc.NewPage();

            obj.Parse(se);
            
            myDoc.Close();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=report.pdf");
            Response.ContentType = "application/pdf";

            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();

           // msg_lbl.Text = se.
          
            /*
            //TextReader te = new TextReader(stringWriter.ToString());

            HTMLWorker obj = new HTMLWorker(myDoc);

            

           /* Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=report.pdf");
            Response.ContentType = "application/pdf";*/





            /*Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition", "attachment; filename=report.pdf");
            Response.ContentType = "application/pdf";(*/

        }

        /*if (Session["pdf"] == "print")
        {
            string datum = Request["datum"].ToString();
            SortedList data = new SortedList();
            StringBuilder itext = new StringBuilder();
            
            itext.AppendFormat("Hlásenie služby: {0}",Session["hlasko_datum"]);
            data.Add("title",itext);
            itext.Remove(0,itext.Length);
            
            itext.AppendFormat("<b>OUP:</b>{0}, <b>Odd.A:</b>{1}, <b>Odd.B:</b>{2}, <b>Op.pohotovost:</b>{3}",lekari["OUP"].ToString(),lekari["OddA"].ToString(),lekari["OddB"].ToString(),lekari["OP"].ToString());
            
            itext.AppendFormat("<br><b>Vytlacil:</b>{0}<br>",user_info["full_name"].ToString());
            itext.Append("<hr>");
            itext.AppendFormat("<b>Sluzba:</a>{0}",data["type"].ToString());
            data.Add("info",itext);
            itext.Remove(0,itext.Length);
            
            itext.AppendFormat("<b>Hlasenie:</b><br>{0}",data["text"].ToString());
            data.Add("hlasenie",itext);
            itext.Remove(0,itext.Length);

            itext.AppendLine("<br><hr><br><br><br>");
            itext.AppendLine("..............................");
            itext.AppendLine("Peciatka a podpis");

            data.Add("podpis",itext);
            itext.Remove(0,itext.Length);

              


            Response.Clear();
            Response.Buffer = true;
            Response.BufferOutput = true;

            Response.ContentType = "application/octet-stream";



            Response.AddHeader("content-disposition", "attachment;filename=hlasko.pdf");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            Response.Charset = "windows-1250";

            StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used

            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            this.RenderControl(htmlTextWriter);
            MemoryStream _result = my_x2.createPDF(my_x2.createHlaskoPDF(data));    
 

            //my_x2.createPDF(stringWriter.ToString());
            //Response.Write

            //StreamReader sr = new StreamReader(_result);
            //String myStr = sr.ReadToEnd();
            //Console.WriteLine(myStr);

            //Response.
            Response.BinaryWrite(_result.ToArray());
            Response.End();

        }*/

      
    }

    protected SortedList getSluzbyByDen(int xden)
    {
        SortedList result = new SortedList();
        DateTime dnesJe = DateTime.Today;
        int den = Convert.ToInt32(Request["den"].ToString());
        int mesiac = Convert.ToInt32(Request["m"].ToString());

        SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", mesiac.ToString(), dnesJe.Year.ToString());

        SortedList docList = x_db.getDoctorsForVykaz();

        if (tmp["ziadna_sluzba"] != "true")
        {
            string[][] data = my_x2.parseSluzba(tmp["rozpis"].ToString());

            DateTime pDate = Convert.ToDateTime("01.07.2012");
            DateTime oDate = Convert.ToDateTime("01." + mesiac.ToString() + "." + DateTime.Now.Year.ToString());

            // int den = dnesJe.Day;
            if (oDate >= pDate)
            {

                string _mm = data[den - 1][1].ToString();

                char[] del = { '|' };

                string[] oup = _mm.Split(del);

                result.Add("OUP", docList[oup[0]].ToString()+""+oup[1]);

                result.Add("OddA", docList[data[den - 1][2].ToString()]);
                result.Add("OddB", docList[data[den - 1][3].ToString()]);
                result.Add("OP", docList[data[den - 1][4].ToString()]);
            }
            else
            {
                result.Add("OUP", data[den - 1][1].ToString());
                result.Add("OddA", data[den - 1][2].ToString());
                result.Add("OddB", data[den - 1][3].ToString());
                result.Add("OP", data[den - 1][4].ToString());
            }
            // result.Add("TRP", data[den - 1][5].ToString());
        }
        else
        {
            result.Add("OUP", "-");
            result.Add("OddA", "-");
            result.Add("OddB", "-");
            result.Add("OP", "-");
        }

        return result;

    }

}
