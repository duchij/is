using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class sltoword : System.Web.UI.Page
{
    mysql_db x2Mysql = new mysql_db();
    x2_var x2 = new x2_var();
    public string gKlinika;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        this.gKlinika = Session["klinika"].ToString().ToLower();
        this.setLabels();

        this.drawTable();
        if (Convert.ToBoolean(Session["st_to_word"]))
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/msword; charset=Windows-1250";
            Response.AddHeader("content-disposition", "attachment;filename=staze.doc");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1250");
           	Response.Charset = "Windows-1250";

            StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used

            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            this.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        else
        {
            print_lbl.Visible = true;
            print_lbl.Text ="<a href='javascript:window.print();'>"+Resources.Resource.print+"</a>";
            back_lbl.Visible = true;
            back_lbl.Text = "<a href='javascript:window.history.back();' target='_self'>" + Resources.Resource.back + "</a>";

        }
    }

    protected void setLabels()
    {
        DateTime dt = DateTime.Today;

        this.printStaze_titel.Text = x2.setLabel(this.gKlinika+"_staze_titel")+", "+dt.ToLongDateString();
        this.sign_lbl.Text = x2.setLabel(this.gKlinika + "_staze_sign");
    }

    protected void drawTable()
    {
        this.stazeTable_tbl.Controls.Clear();

        int mesiac = Convert.ToInt32(Session["st_akt_month"]);
        int rok = Convert.ToInt32(Session["st_akt_year"]);

        int daysInMonth = DateTime.DaysInMonth(rok, mesiac);
        string query = "SELECT * FROM [is_settings] WHERE [name] = '" + Session["klinika"].ToString().ToLower() + "_staze'";

        SortedList res = x2Mysql.getRow(query);
        string[] staze = res["data"].ToString().Split(',');

        int colsNum = staze.Length;
        string[] header = staze;

        // ArrayList doctors = this.loadOmegaDoctors();

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell headCell = new TableHeaderCell();
        headCell.ID = "headCell_date";
        headCell.BorderStyle = BorderStyle.Solid;
        headCell.BorderWidth = Unit.Pixel(1);
        headCell.Text = "<strong>Datum</strong>";
        // headCell.Style
        headRow.Controls.Add(headCell);

        for (int head = 0; head < colsNum; head++)
        {
            TableHeaderCell headCell1 = new TableHeaderCell();
            headCell1.BorderStyle = BorderStyle.Solid;
            headCell1.BorderWidth = Unit.Pixel(1);
            headCell1.ID = "headCell_" + head;
            headCell1.Text = "<strong><center>" + x2.setLabel(header[head].ToString()) + "</center></strong>";
            // headCell1.
            headRow.Controls.Add(headCell1);
        }
        this.stazeTable_tbl.Controls.Add(headRow);

        string[] freeDays = Session["freedays"].ToString().Split(',');

        for (int row = 0; row < daysInMonth; row++)
        {
            TableRow riadok = new TableRow();
            this.stazeTable_tbl.Controls.Add(riadok);
            int rDen = row + 1;

            DateTime dt = new DateTime(rok, mesiac, rDen);
            Boolean vikend = false;
            int dW = (int)dt.DayOfWeek;

            if (dW == 6 || dW == 0) vikend = true;
            int sviatok = Array.IndexOf(freeDays, rDen.ToString() + "." + mesiac.ToString());


            for (int col = 0; col <= colsNum; col++)
            {

                if (col == 0)
                {

                    TableCell dateCell = new TableCell();
                    dateCell.Text = dt.ToShortDateString();
                    dateCell.BorderStyle = BorderStyle.Solid;
                    dateCell.BorderWidth = Unit.Pixel(1);
                    
                    if (vikend) dateCell.BackColor = System.Drawing.Color.LightGray;
                    if (sviatok != -1) dateCell.BackColor = System.Drawing.Color.LightGray;
                    riadok.Controls.Add(dateCell);
                }
                else
                {
                    TableCell dataCell = new TableCell();
                    dataCell.BorderStyle = BorderStyle.Solid;
                    dataCell.BorderWidth = Unit.Pixel(1);
                    if (vikend) dataCell.BackColor = System.Drawing.Color.LightGray;
                    if (sviatok != -1) dataCell.BackColor = System.Drawing.Color.LightGray;
                   
                    Label textLbl = new Label();
                    textLbl.ID = header[col - 1] + "_" + rDen.ToString();
                    dataCell.Controls.Add(textLbl);
                    
                    riadok.Controls.Add(dataCell);
                }
            }
        }
        this.loadStaze();
    }

    protected void loadStaze()
    {
        int mesiac = Convert.ToInt32(Session["st_akt_month"]);
        int rok = Convert.ToInt32(Session["st_akt_year"]);

        int dateGroup = x2.makeDateGroup(rok, mesiac);
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [type],[datum],[text] FROM [is_staze_2] WHERE [date_group]='{0}' AND [clinic]='{1}' ORDER BY [datum]", dateGroup, Session["klinika_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableCn = table.Count;

       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        for (int row = 0; row < tableCn; row++)
        {
            DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["datum"].ToString()));

            int day = dt.Day;

            string type = table[row]["type"].ToString();
            Control cl = FindControl(type + "_" + day.ToString());
            Label txtBox = (Label)cl;
            txtBox.Text = table[row]["text"].ToString();
         


        }
    }

}
