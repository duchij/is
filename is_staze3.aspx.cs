using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


class ClassesObj
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

    public string[] kruzky{
        get {return _kruzky;}
        set { _kruzky =value;}
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

public partial class is_staze3 : System.Web.UI.Page
{
    private ClassesObj Clasess = new ClassesObj();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        

        Clasess.gKlinika = Convert.ToInt32(Session["klinika_id"]);
        Clasess.rights = Session["rights"].ToString();
        Clasess.clinicIdf = Session["klinika"].ToString();
        Clasess.mysql = new mysql_db();
        Clasess.x2 = new x2_var();
        Clasess.x2log = new log();

        Clasess.x2.fillYearMonth(ref this.month_dl, ref this.year_dl, Session["month_dl"].ToString(), Session["years_dl"].ToString());

        this.insertSection_plh.Visible = Clasess.editable;


        this.loadRocniky();

        if (!IsPostBack)
        {

            if (Clasess.editable)
            {
                this.loadLecturers();
                this.loadStudentClasses();
            }
            this.setMonthYear();
            
        }

        this.loadSchedule();
        
    }

    protected void setMonthYear()
    {
        int mesiac = DateTime.Today.Month;
        int rok = DateTime.Today.Year;
        this.month_dl.SelectedValue = mesiac.ToString();
        this.year_dl.SelectedValue = rok.ToString();
    }


    protected void loadLecturers()
    {
        string query = @"SELECT [id],[name3] FROM [is_users] WHERE [work_group]='doctor' AND [active]='1' AND [klinika]='{0}' ORDER BY [name2]";
        query = Clasess.mysql.buildSql(query,new string[] {Clasess.gKlinika.ToString()});

        Dictionary<int, Hashtable> table = Clasess.mysql.getTable(query);

        int dataLn = table.Count;

        for (int i = 0; i < dataLn; i++)
        {
            this.lecturer_dl.Items.Add(new ListItem(table[i]["name3"].ToString(), table[i]["id"].ToString()));
        }
    }

    protected void loadRocniky()
    {
            string query = "SELECT [data] FROM [is_settings] WHERE [name]='{0}_staze'";

            query = Clasess.mysql.buildSql(query, new string[] { Clasess.clinicIdf });
            SortedList row = Clasess.mysql.getRow(query);
            //string[] cl =
            Clasess.kruzky = row["data"].ToString().Split(',');
    } 


    protected void loadStudentClasses()
    {

        int cln = Clasess.kruzky.Length;

        this.yearclass_dl.Items.Clear();

        for (int i=0; i< cln; i++){
            this.yearclass_dl.Items.Add(new ListItem(Clasess.kruzky[i],Clasess.kruzky[i]));
        }
    }

    protected void saveData(Object sender, EventArgs e)
    {

        string group = this.group_txt.Text.ToString().Trim();

        Boolean status = true;

        if (group.Length == 0)
        {
            status = false;
            Clasess.x2.errorMessage2(ref this.msg_lbl, "Musi byt zadana skupina....");
        }

        string startTime = this.starttime_txt.Text.ToString().Trim();
        string endTime = this.endtime_txt.Text.ToString().Trim();

        if (startTime.Length == 0 || endTime.Length == 0)
        {
            status = false;
            Clasess.x2.errorMessage2(ref this.msg_lbl, "Nie ja zadany zaciatok, alebo koniec");
        }

        string classDate = this.classes_date_txt.Text.ToString().Trim();

        if (classDate.Length == 0)
        {
            status = false;
            Clasess.x2.errorMessage2(ref this.msg_lbl, "Nie ja zadany datum");
        }
       

        if (status)
        {
            SortedList saveData = new SortedList();
            saveData.Add("user_id", this.lecturer_dl.SelectedValue.ToString());
            saveData.Add("class_year", this.yearclass_dl.SelectedValue.ToString());

            saveData.Add("group", group);

            string startDate = classDate + " " +startTime + ":00";
            string endDate = classDate + " " + endTime + ":00";

            saveData.Add("date_start", startDate);
            saveData.Add("date_end", endDate);
            saveData.Add("clinic", Clasess.gKlinika);
            string note = this.comment_txt.Text.ToString().Trim();
            if (note.Length == 0)
            {
                saveData.Add("note", "-");
            }
            else
            {
                saveData.Add("note", note );
            }
           


            SortedList result = Clasess.mysql.mysql_insert("is_staze3", saveData);

            if (!(Boolean)result["status"])
            {
                Clasess.x2.errorMessage2(ref this.msg_lbl, result["msg"].ToString());
            }
            else
            {
                this.loadSchedule();
            }
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

        int month = Convert.ToInt32(this.month_dl.SelectedValue.ToString());
        int year = Convert.ToInt32(this.year_dl.SelectedValue.ToString());

        int days = DateTime.DaysInMonth(year, month);

        TableHeaderRow headRow = new TableHeaderRow();

        int kruzkLn = Clasess.kruzky.Length;

        TableHeaderCell headDate = new TableHeaderCell();
        headDate.Text = "Datum";
        headRow.Controls.Add(headDate);

        for (int k = 0; k < kruzkLn; k++)
        {
            TableHeaderCell headCell = new TableHeaderCell();
            headCell.Text = Clasess.kruzky[k];
            headRow.Controls.Add(headCell);
        }

        this.classes_tbl.Controls.Add(headRow);



        for (int day = 1; day <= days; day++)
        {
            TableRow newRow = new TableRow();

            TableCell cellDate = new TableCell();
            DateTime myDate = new DateTime(year, month,day);
            int den = (int)myDate.DayOfWeek;
            string nazov =CultureInfo.CurrentCulture.DateTimeFormat.DayNames[den];
            cellDate.Text = day.ToString() + ". " + nazov + "," + year.ToString();

            int jeSviatok = Array.IndexOf(freeDays, day.ToString() + "." + month.ToString());
            
            string classDay = "";

            if (den == 0 || den == 6)
            {
                classDay = "box red";
            }

            if (jeSviatok != -1 && den != 0 && den != 6)
            {
                classDay = "box yellow";
            }

            cellDate.CssClass = classDay;

            newRow.Controls.Add(cellDate);
            for (int cell = 0; cell < clLn; cell++)
            {
                TableCell cellData = new TableCell();
                cellData.ID = "cell_" + day.ToString() + "_" + cl[cell];
                cellData.CssClass = classDay;
                //cellData.Text = cl[cell];
                newRow.Controls.Add(cellData);
            }
            this.classes_tbl.Controls.Add(newRow);
        }
        this.loadLectureData(year, month, 3);
    }



    protected void loadLectureData(int year,int month,int clinic)
    {
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;


        string startDate = year.ToString() + "-" + month.ToString() + "-01";
        int days = DateTime.DaysInMonth(year,month);
        string endDate = year.ToString() + "-" + month.ToString() + "-"+days.ToString();

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

        query = Clasess.mysql.buildSql(query, new string[]{startDate,endDate,Clasess.gKlinika.ToString()});

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
                        Control ctl = ctpl.FindControl("cell_" + day.ToString() + "_" + year_class);

                        TableCell dataCell = (TableCell)ctl;



                        string[] dtTmp = kurzy[k].Split(',');
                        Literal lt = new Literal();
                        string startTime = dtTmp[2].Substring(0, 5);
                        string endTime = dtTmp[3].Substring(0, 5);
                        lt.Text = "<p class='alert box'><span class='blue'>" + startTime + "-" + endTime + "</span><br><strong>" + year_class+"/"+dtTmp[1] + "</strong>, <span class='red'>" + dtTmp[0] + "</span> " ;
                        if (notes[k] != null) {
                            lt.Text += "<br><span class='small'>" + notes[k] + "</span></p>";
                        }
                        
                        dataCell.Controls.Add(lt);

                        if (Clasess.editable)
                        {
                            Button editBtn = new Button();
                            editBtn.Text = "E";
                            editBtn.CssClass = "small button green";
                            editBtn.ID = "editBtn_" + dtTmp[4];
                            editBtn.Click += new EventHandler(buttonAction);
                            editBtn.ToolTip = "Edituje aktuálny záznam...";
                            dataCell.Controls.Add(editBtn);

                            Button delBtn = new Button();
                            delBtn.Text = "D";
                            delBtn.ID = "delBtn_" + dtTmp[4];
                            delBtn.OnClientClick = "return confirm('Naozaj zmazat...');";
                            delBtn.CssClass = "small button red";
                            delBtn.ToolTip = "Zmaže aktuálny záznam...";
                            delBtn.Click += new EventHandler(buttonAction);
                            dataCell.Controls.Add(delBtn);

                            Literal pasik = new Literal();
                            pasik.Text = "<hr>";
                            dataCell.Controls.Add(pasik);
                        }
                        
                    }

                }
                else
                {
                    DateTime dt = Convert.ToDateTime(table[row]["den"].ToString());
                    int day = dt.Day;
                    Control ctl = ctpl.FindControl("cell_" + day.ToString() + "_" + year_class);



                    TableCell dataCell = (TableCell)ctl;

                    string[] dtTmp = kruzTmp.Split(',');
                    Literal lt = new Literal();
                    string startTime = dtTmp[2].Substring(0, 5);
                    string endTime = dtTmp[3].Substring(0, 5);
                    lt.Text = " <p class='alert box'><span class='blue'>" + startTime + "-" + endTime + "</span><br><strong>" + year_class+"/"+dtTmp[1] + "</strong>, <span class='red'>" + dtTmp[0] + "</span> " ;
                    lt.Text += "<br><span class='small'>" + table[row]["note"].ToString() + "</span></p>";
                    dataCell.Controls.Add(lt);
                    if (Clasess.editable)
                    {
                        Button editBtn = new Button();
                        editBtn.Text = "E";
                        editBtn.ID = "editBtn_" + dtTmp[4];
                        editBtn.Click += new EventHandler(buttonAction);
                        editBtn.CssClass = "small button green";
                        editBtn.ToolTip = "Edituje aktuálny záznam...";
                        dataCell.Controls.Add(editBtn);

                        Button delBtn = new Button();
                        delBtn.Text = "D";
                        delBtn.ID = "delBtn_" + dtTmp[4];
                        delBtn.Click += new EventHandler(buttonAction);
                        delBtn.ToolTip = "Zmaže aktuálny záznam...";
                        delBtn.OnClientClick = "return confirm('Naozaj zmazat...');";
                        delBtn.CssClass = "small button red";
                        dataCell.Controls.Add(delBtn);

                        Literal pasik = new Literal();
                        pasik.Text = "<hr>";
                        dataCell.Controls.Add(pasik);
                    }
                    


                }
                // string[] kruz = .split
            }
    }


    protected void buttonAction(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] tmp = btn.ID.ToString().Split('_');
         
        if (tmp[0]=="delBtn")
        {
            this.deleteLecture(tmp[1]);
        }
        if (tmp[0]=="editBtn")
        {
            this.loadDataForUpdate(tmp[1]);
        }

    }

    protected void loadDataForUpdate(string id)
    {
        string query = "SELECT [user_id],[class_year],[group],[date_start],[date_end],[note] FROM [is_staze3] WHERE [item_id]={0}";
        query = Clasess.mysql.buildSql(query,new string[]{id});
        SortedList row = Clasess.mysql.getRow(query);

        if (row["status"] != null)
        {
            Clasess.x2.errorMessage2(ref this.msg_lbl, "Chyba pri nacitani riadku " + row["msg"].ToString());
        }
        else
        {
            this.lecturer_dl.SelectedValue = row["user_id"].ToString();
            this.comment_txt.Text = row["note"].ToString();
            this.group_txt.Text = row["group"].ToString();


            DateTime startDate = Convert.ToDateTime(row["date_start"].ToString());
            DateTime endDate = Convert.ToDateTime(row["date_end"].ToString());

            string startDateStr = Clasess.x2.unixDate(startDate);
            string endDateStr = Clasess.x2.unixDate(endDate);

            //string uStartDate = Clasess.x2.dat

            this.classes_date_txt.Text = startDateStr;

            this.starttime_txt.Text = startDate.ToShortTimeString();
            this.endtime_txt.Text = endDate.ToShortTimeString();
            this.yearclass_dl.SelectedValue = row["class_year"].ToString();
        }
        

    }

    protected void deleteLecture(string id)
    {
        string query = "DELETE FROM [is_staze3] WHERE [item_id]='{0}'";

        query = Clasess.mysql.buildSql(query,new string[] {id});

        SortedList result = Clasess.mysql.execute(query);

        if (!(Boolean)result["status"])
        {
            Clasess.x2.errorMessage2(ref this.msg_lbl, "Chyba pri mazani " + result["msg"].ToString());
        }
        else
        {
            this.loadSchedule();
        }

    }
}