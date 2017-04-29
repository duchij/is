using System;
using System.Text;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;

using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;



class ClassesObjPrint
{
    private string _rights = "";
    private int _gKlinika = 0;
    private mysql_db _mysql;
    private x2_var _x2;
    private log _log;
    private string _clinicIdf;

    //private Boolean _editable;
    private string[] _kruzky;

    public Boolean editable
    {
        get
        {
            if (_rights.IndexOf("admin") != -1 || _rights == "poweruser")
            {
                return true;
            }
            else { return false; }

        }
    }

    public string[] kruzky
    {
        get { return _kruzky; }
        set { _kruzky = value; }
    }


    public string clinicIdf
    {
        get { return _clinicIdf; }
        set { _clinicIdf = value; }
    }

    public string rights
    {
        get { return _rights; }
        set { _rights = value; }
    }

    public mysql_db mysql
    {
        get { return _mysql; }
        set { _mysql = value; }
    }



    public int gKlinika
    {
        get { return _gKlinika; }
        set { _gKlinika = value; }
    }

    public x2_var x2
    {
        get { return _x2; }
        set { _x2 = value; }


    }

    public log x2log
    {
        get { return _log; }
        set { _log = value; }
    }

}

public partial class is_p_staze : System.Web.UI.Page
{
    ClassesObjPrint Clasess = new ClassesObjPrint();
    int month = 0;
    int year = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.month = Convert.ToInt32(Request.QueryString["m"]);
            this.year = Convert.ToInt32(Request.QueryString["y"]);

            this.title_lbl.Text = Session["klinika"].ToString() +", "+this.month+"/"+this.year;
           


            Clasess.gKlinika = Convert.ToInt32(Session["klinika_id"]);
            Clasess.rights = Session["rights"].ToString();
            Clasess.clinicIdf = Session["klinika"].ToString();
            Clasess.mysql = new mysql_db();
            Clasess.x2 = new x2_var();
            Clasess.x2log = new log();

            // Clasess.x2.fillYearMonth(ref this.month_dl, ref this.year_dl, Session["month_dl"].ToString(), Session["years_dl"].ToString());

            this.loadRocniky();
            this.loadSchedule();

            string p = Request.QueryString["p"].ToString();

            if ( p == "w")
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
        }
        catch (Exception ex)
        {
            this.info_lt.Text = ex.ToString();
        }

        


    }

    protected void loadSchedule()
    {


        this.classes_tbl.Controls.Clear();

        string[] freeDays = Session["freedays"].ToString().Split(',');


        string query = "SELECT [data] FROM [is_settings] WHERE [name]='{0}_staze'";

        query = Clasess.mysql.buildSql(query, new string[] { Clasess.clinicIdf });
        SortedList row = Clasess.mysql.getRow(query);
        string[] cl = row["data"].ToString().Split(',');

        int clLn = cl.Length;


        int days = DateTime.DaysInMonth(this.year, this.month);

        TableHeaderRow headRow = new TableHeaderRow();

        int kruzkLn = Clasess.kruzky.Length;

        TableHeaderCell headDate = new TableHeaderCell();
        headDate.Text = "Datum";
        headRow.Controls.Add(headDate);

        for (int k = 0; k < kruzkLn; k++)
        {
            TableHeaderCell headCell = new TableHeaderCell();
            headCell.Text = Clasess.kruzky[k];
            headCell.Width = Unit.Point(100);
            headRow.Controls.Add(headCell);
        }

        this.classes_tbl.Controls.Add(headRow);



        for (int day = 1; day <= days; day++)
        {
            TableRow newRow = new TableRow();

            TableCell cellDate = new TableCell();
            DateTime myDate = new DateTime(year, month, day);
            int den = (int)myDate.DayOfWeek;
            string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[den];
            cellDate.Text = day.ToString() + ". " + nazov + ", " + year.ToString();

            int jeSviatok = Array.IndexOf(freeDays, day.ToString() + "." + month.ToString());

            int classDay = 0xffffff; 

            if (den == 0 || den == 6)
            {
                classDay = 0xc7ccc1;
            }

            if (jeSviatok != -1 && den != 0 && den != 6)
            {
                classDay = 0xc7ccc1;
            }

            cellDate.BackColor = System.Drawing.Color.FromArgb(classDay);

            newRow.Controls.Add(cellDate);
            for (int cell = 0; cell < clLn; cell++)
            {
                TableCell cellData = new TableCell();
                cellData.ID = "cell_" + day.ToString() + "_" + cl[cell];
                cellData.BackColor = System.Drawing.Color.FromArgb(classDay);
                //cellData.Text = cl[cell];
                newRow.Controls.Add(cellData);
            }
            this.classes_tbl.Controls.Add(newRow);
        }
        this.loadLectureData(year, month, 3);
    }

    protected void loadRocniky()
    {
        string query = "SELECT [data] FROM [is_settings] WHERE [name]='{0}_staze'";

        query = Clasess.mysql.buildSql(query, new string[] { Clasess.clinicIdf });
        SortedList row = Clasess.mysql.getRow(query);
        //string[] cl =
        Clasess.kruzky = row["data"].ToString().Split(',');
    }



    protected void loadLectureData(int year, int month, int clinic)
    {
       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
       // ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;


        string startDate = year.ToString() + "-" + month.ToString() + "-01";
        int days = DateTime.DaysInMonth(year, month);
        string endDate = year.ToString() + "-" + month.ToString() + "-" + days.ToString();

        string query = @"SELECT [t_staze.user_id] ,[t_staze.class_year] AS [class_year], 
                            GROUP_CONCAT(CONCAT_WS(',',[t_users.name3],[t_staze.group],DATE_FORMAT([t_staze.date_start],'%T'),DATE_FORMAT([t_staze.date_end],'%T'),t_staze.item_id) SEPARATOR '|') AS [kruz],
                            GROUP_CONCAT(DATE_FORMAT([t_staze.date_start],'%e.%c.%Y') SEPARATOR '|') AS [den],
                            GROUP_CONCAT([t_staze.note] SEPARATOR '|') AS [note]
                            FROM [is_staze3] as [t_staze]

                        INNER JOIN [is_users] AS [t_users] ON [t_users.id] =  [t_staze.user_id]

                        WHERE 
                            [t_staze.date_start] BETWEEN '{0} 00:00:01' AND '{1} 23:59:59'
                            OR [t_staze.date_end] BETWEEN '{0} 00:00:01' AND '{1} 23:59:59'
                            AND [t_staze.clinic]={2}

                        GROUP BY [t_staze.class_year]
                        ORDER BY [t_staze.date_start]";

        query = Clasess.mysql.buildSql(query, new string[] { startDate, endDate, Clasess.gKlinika.ToString() });

        Dictionary<int, Hashtable> table = Clasess.mysql.getTable(query);

        int tblLn = table.Count;



        for (int row = 0; row < tblLn; row++)
        {
            int cell = row + 1;
            string kruzTmp = table[row]["kruz"].ToString();

            string year_class = table[row]["class_year"].ToString();

            if (kruzTmp.IndexOf("|") != -1)
            {
                string[] kurzy = kruzTmp.Split('|');

                int kLn = kurzy.Length;

                for (int k = 0; k < kLn; k++)
                {
                    string[] kDatum = table[row]["den"].ToString().Split('|');

                    string[] notes = table[row]["note"].ToString().Split('|');

                    DateTime dt = Convert.ToDateTime(kDatum[k]);
                    int day = dt.Day;
                    Control ctl = FindControl("cell_" + day.ToString() + "_" + year_class);

                    TableCell dataCell = (TableCell)ctl;



                    string[] dtTmp = kurzy[k].Split(',');
                    Literal lt = new Literal();
                    string startTime = dtTmp[2].Substring(0, 5);
                    string endTime = dtTmp[3].Substring(0, 5);
                    lt.Text = "<div class='info'><strong>" + startTime + "-" + endTime + "</strong><br><i>" + year_class + " / " + dtTmp[1] + "</i>, " + dtTmp[0];
                    if (notes[k] != null)
                    {
                        lt.Text += "<br>" + notes[k] + "</div>";
                    }

                    dataCell.Controls.Add(lt);

                    

                }

            }
            else
            {
                DateTime dt = Convert.ToDateTime(table[row]["den"].ToString());
                int day = dt.Day;
                Control ctl = FindControl("cell_" + day.ToString() + "_" + year_class);

                TableCell dataCell = (TableCell)ctl;

                string[] dtTmp = kruzTmp.Split(',');
                Literal lt = new Literal();
                string startTime = dtTmp[2].Substring(0, 5);
                string endTime = dtTmp[3].Substring(0, 5);

                 lt.Text = "<div class='info'><strong>" + startTime + "-" + endTime + "</strong><br><i>" + year_class + " / " + dtTmp[1] + "</i>, " + dtTmp[0];
                lt.Text += "<br>" + table[row]["note"].ToString() + "</div>";

                dataCell.Controls.Add(lt);

            }
            // string[] kruz = .split
        }
    }


}