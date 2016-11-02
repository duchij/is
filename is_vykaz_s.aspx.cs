using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
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

class VykazClass
{
    private string _rights = "";
    private int _gKlinika = 0;
    private mysql_db _mysql;
    private x2_var _x2;
    private log _log;
    private string _clinicIdf;

    private string _department;

    private Boolean _editable;
    private string[] _vykazHeader;

    public string department
    {
        get { return _department; }
        set { _department = value; }
    }

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

    public string[] vykazHeader
    {
        get { return _vykazHeader; }
        set { _vykazHeader = value; }
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


public partial class is_vykaz_s : System.Web.UI.Page
{
    private VykazClass vykaz = new VykazClass(); 


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        

        vykaz.gKlinika = Convert.ToInt32(Session["klinika_id"]);
        vykaz.rights = Session["rights"].ToString();
        vykaz.clinicIdf = Session["klinika"].ToString();
        vykaz.mysql = new mysql_db();
        vykaz.x2 = new x2_var();
        vykaz.x2log = new log();
        vykaz.department = Session["oddelenie"].ToString();

        vykaz.x2.fillYearMonth(ref this.mesiac_cb, ref this.rok_cb, Session["month_dl"].ToString(), Session["years_dl"].ToString());

        if (vykaz.editable)
        {
            this.anotherUser_pl.Visible = true;
        }
        else
        {
            this.anotherUser_pl.Visible = false;
        }



        if (!IsPostBack)
        {
            DateTime dnesJe = DateTime.Today;
            this.mesiac_cb.SelectedValue = dnesJe.Month.ToString();
            this.rok_cb.SelectedValue = dnesJe.Year.ToString();

            if (vykaz.editable)
            {
                this.loadDeps();
            }
            
        }

       
    }

    

    protected void loadNurses_fnc(object sender, EventArgs e)
    {
        string dep = this.deps_dl.SelectedValue.ToString();

        string query = "SELECT [id], [name3] FROM [kdch_nurse] WHERE [idf]='{0}'";

        query = vykaz.mysql.buildSql(query, new string[] { dep });

        Dictionary<int, Hashtable> table = vykaz.mysql.getTable(query);

        int nurseLn = table.Count;
        this.nurses_dl.Items.Clear();
        for (int nurse = 0; nurse < nurseLn; nurse++)
        {

            this.nurses_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[nurse]["name3"].ToString(), table[nurse]["id"].ToString()));

        }


    }

    protected void loadDeps()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_deps] WHERE [clinic_id]='{0}'", Session["klinika_id"]);

        Dictionary<int, Hashtable> table = vykaz.mysql.getTable(sb.ToString());


        int depsLn = table.Count;
       // this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "-"));
        for (int dep = 0; dep < depsLn; dep++)
        {
            
            this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[dep]["label"].ToString(), table[dep]["idf"].ToString()));
            
        }
    }

    protected void generateVykazNurse(int mesiac, int rok, Boolean writeText)
    {
        this.vykaz_tbl.Controls.Clear();

        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }
        
        string query = "SELECT * FROM `is_settings`  WHERE `name`='vykaz_nurse_header'";
       // query = vykaz.mysql.buildSql(query, new string[] { });
        SortedList row = vykaz.mysql.getRow(query);
        vykaz.vykazHeader = row["data"].ToString().Split(',');

        int cols = vykaz.vykazHeader.Length;
        TableHeaderRow headerRow = new TableHeaderRow();

        TableHeaderCell headCellDate = new TableHeaderCell();
        headCellDate.ID = "datum_cell";
        headCellDate.Text = "Datum";
        headerRow.Controls.Add(headCellDate);

        this.vykaz_tbl.Controls.Add(headerRow);

        for (int col = 0; col < cols; col++)
        {
            TableHeaderCell headCell = new TableHeaderCell();
            headCell.ID = "headCell_" + col.ToString();
            headCell.BackColor = System.Drawing.Color.Gold;
            Label headLabel = new Label();
            headLabel.ID = vykaz.vykazHeader[col] + "_lbl<br>";
            headLabel.Font.Size = FontUnit.Point(8);
            headLabel.Text = vykaz.vykazHeader[col];

            headCell.Controls.Add(headLabel);

            TextBox tBox = new TextBox();
            tBox.ID = "head_tbox_" + col.ToString();
            tBox.Text = "";
            headCell.Controls.Add(tBox);


            headerRow.Controls.Add(headCell);
        }

        int dniMes = DateTime.DaysInMonth(rok, mesiac);
        string[] sviatky = Session["freedays"].ToString().Split(',');

        Dictionary<int, Hashtable> nurseShifts = this.getShiftsNurse(rok, mesiac);

        this.setVykazTypesforNurses();

        SortedList vykazVypis = (SortedList)Session["KDCH_nurse_hours_type"];

        for (int den = 0; den < dniMes; den++)
        {
            // 
            DateTime my_date = new DateTime(rok, mesiac, den + 1);
            int dnesJe = (int)my_date.DayOfWeek;
            Boolean vikend = false;
            if (dnesJe == 6 || dnesJe == 0) vikend = true;

            string dentmp = (den + 1).ToString() + "." + mesiac.ToString();

            int res = Array.IndexOf(sviatky, dentmp);
            Boolean sviatok = (res != -1) ? true : false;

           string[] rowdata = (vikend) ? vykazVypis["ExDay"].ToString().Split(',') : vykazVypis["NormDay"].ToString().Split(',');

           this.makeRow(den, cols, rowdata, rok, mesiac, sviatok, false, true);

        }

        this.fillShiftsForNurses(nurseShifts);
        this.fillInVacations(mesiac, rok, Session["user_id"].ToString());

    }


    protected void fillShiftsForNurses(Dictionary<int, Hashtable> data)
    {
        int dataCn = data.Count;

        for (int row = 0; row < dataCn; row++)
        {
            DateTime dt = Convert.ToDateTime(vykaz.x2.UnixToMsDateTime(data[row]["datum"].ToString()));

            int dw = (int)dt.DayOfWeek;
            Boolean vikend = false;
            if (dw == 6 || dw == 0) vikend = true;

            string[] freeD = Session["freedays"].ToString().Split(',');
            int res = Array.IndexOf(freeD, dt.Day.ToString() + "." + dt.Month.ToString());
            Boolean sviatok = (res != -1) ? true : false;

            //string week = data[row]["tyzden"].ToString();
            string typ = data[row]["typ"].ToString();
            if (typ != "KlAmb") this.calcRow(sviatok, vikend, typ, dt);

        }

    }



    protected void calcRow(Boolean sviatok, Boolean vikend, string typ, DateTime dt)
    {
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        int day = dt.Day;
        int row = day - 1;

        SortedList vykazVypis = (SortedList)Session["KDCH_nurse_hours_type"];

        DateTime dtOneAfter = dt.AddDays(1);

        string zacDt = vykaz.x2.unixDate(dt);
        string koncDt = vykaz.x2.unixDate(dtOneAfter);

        int cols = vykaz.vykazHeader.Length;

        string endHour = "";
        Boolean epcYes = true;

        int daysAfter = 0;

        string[] rowData = new string[cols];

        string[] day1 = new string[cols];
        string[] day2 = new string[cols];

        if (typ.IndexOf("N") != -1)
        {
            rowData = vykazVypis["N"].ToString().Split(',');
            //endHour = "07:00:00";
            //epcYes = true;
            daysAfter = 1;
            day1 = vykazVypis["Nc"].ToString().Split(',');
            //day2 = vykazVypis["ExDay"].ToString().Split(',');
        }

        if (typ.IndexOf("D") != -1)
        {
            rowData = vykazVypis["D"].ToString().Split(',');
            
            daysAfter = 0;
            //day1 = vykazVypis["ExDay"].ToString().Split(',');
            //day2 = vykazVypis["ExDay"].ToString().Split(',');
          
        }

        if (typ.IndexOf("R") != -1)
        {
            rowData = vykazVypis["R"].ToString().Split(',');
            //endHour = "07:00:00";
           // epcYes = true;
            daysAfter = 0;
           // day1 = vykazVypis["ExDay"].ToString().Split(',');
        }

        if (typ.IndexOf("S") != -1)
        {
            rowData = vykazVypis["S"].ToString().Split(',');

        }
        if (typ == "A1")
        {
            rowData = vykazVypis["A1"].ToString().Split(',');
            daysAfter = 0;
        }

        if (typ == "A2")
        {
            rowData = vykazVypis["A2"].ToString().Split(',');
            daysAfter = 1;
            day1 = vykazVypis["A2c"].ToString().Split(',');
        }

        /*if (typ == "OupB")
        {
            rowData = vykazVypis["OupB_normden"].ToString().Split(',');
            epcYes = true;
            daysAfter = 1;
            day1 = vykazVypis["OupB_normden_po"].ToString().Split(',');

        }
        if (typ == "Expe")
        {
            rowData = (vikend) ? vykazVypis["Expe_vik"].ToString().Split(',') : vykazVypis["Expe_normden"].ToString().Split(',');
            endHour = (vikend) ? "08:00:00" : "07:00:00";
            epcYes = true;
            if (!vikend)
            {
                daysAfter = 1;
                day1 = vykazVypis["Expe_normden_po"].ToString().Split(',');
            }
        }
        if (typ == "OupA1" || typ == "OupA2")
        {
            rowData = vykazVypis["OupA12_vik"].ToString().Split(',');
        }

        if (typ == "OupB1")
        {
            rowData = vykazVypis["OupB1_vik"].ToString().Split(',');
            epcYes = true;
            daysAfter = 1;
            day1 = vykazVypis["OupB1_vik_po"].ToString().Split(',');
        }

        for (int tt = 0; tt < cols; tt++)
        {
            Control crtl = ctpl.FindControl("textBox_" + row.ToString() + "_" + tt.ToString());
            TextBox txtB = (TextBox)crtl;
            txtB.Text = rowData[tt].ToString();
            txtB.BackColor = System.Drawing.Color.LightGray;
            txtB.Font.Bold = true;
        }*/

        Control prichod = ctpl.FindControl("textBox_" + row.ToString() + "_0");
        TextBox prichod_txt = (TextBox)prichod;
        prichod_txt.Text = rowData[0];


        Control obed_zac = ctpl.FindControl("textBox_" + row.ToString() + "_1");
        TextBox obed_zac_txt = (TextBox)obed_zac;
        obed_zac_txt.Text = rowData[1];

        Control obed_konc = ctpl.FindControl("textBox_" + row.ToString() + "_2");
        TextBox obed_konc_txt = (TextBox)obed_konc;
        obed_konc_txt.Text = rowData[2];

        Control odchod = ctpl.FindControl("textBox_" + row.ToString() + "_3");
        TextBox odchod_txt = (TextBox)odchod;
        odchod_txt.Text = rowData[3];

        Control hodiny = ctpl.FindControl("textBox_" + row.ToString() + "_4");
        TextBox hodiny_txt = (TextBox)hodiny;
        hodiny_txt.Text = rowData[4];


        if (vikend || sviatok)
        {


            Control tbox1 = ctpl.FindControl("textBox_" + day.ToString() + "_9");
            TextBox mTBox1 = (TextBox)tbox1;


            Control tbox2 = ctpl.FindControl("textBox_" + day.ToString() + "_11");
            TextBox mTBox2 = (TextBox)tbox2;

            Control hodTmp = ctpl.FindControl("textBox_" + day.ToString() + "_4");
            TextBox zucHodTxt = (TextBox)hodTmp;

            Control mzvyhTmp = ctpl.FindControl("textBox_" + day.ToString() + "_7");
            TextBox mzvyhTxt = (TextBox)mzvyhTmp;

            Control mzvyhTmpSv = ctpl.FindControl("textBox_" + day.ToString() + "_6");
            TextBox mzvyhTxtSv = (TextBox)mzvyhTmpSv;

           // decimal zuctHodinyFLOAT = Convert.ToDecimal(zucHodTxt.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
           // decimal defZuc = zuctHodinyFLOAT + hodiny;
           // mzvyhTxt.Text = defZuc.ToString();
            //mzvyhTxtSv.Text = defZuc.ToString();

            //mTBox1.Text = hodiny.ToString();
            //mTBox2.Text = neaktivna.ToString();
        }
        else
        {



            Control tbox1 = ctpl.FindControl("textBox_" + day.ToString() + "_8");
            TextBox mTBox1 = (TextBox)tbox1;
            Control tbox2 = ctpl.FindControl("textBox_" + day.ToString() + "_10");
            TextBox mTBox2 = (TextBox)tbox2;

        //    mTBox1.Text = hodiny.ToString();
        //    mTBox2.Text = neaktivna.ToString();
        }
            
        

        if (daysAfter > 0)
        {
            for (int eDay = 0; eDay < daysAfter; eDay++)
            {
                day = dt.AddDays(eDay + 1).Day;
                row = day - 1;
                if (eDay == 0)
                {
                    rowData = day1;
                }
                else
                {
                    rowData = day2;
                }

                for (int tt = 0; tt < cols; tt++)
                {
                    Control crtl = ctpl.FindControl("textBox_" + row.ToString() + "_" + tt.ToString());
                    TextBox txtB = (TextBox)crtl;
                    if (tt == 3)
                    {
                        if (txtB.Text.ToString().Trim() == "0")
                        {
                            txtB.Text = rowData[tt].ToString();
                        }
                        
                    }

                    
                    //txtB.BackColor = System.Drawing.Color.LightGray;
                    //txtB.Font.Bold = true;
                }


            }
        }

    }

    protected void makeRow(int den, int cols, string[] rowData, int rok, int mesiac, Boolean sviatok, Boolean shift, Boolean writeText)
    {
        //den = den + 1;

        try
        {
            DateTime my_date = new DateTime(rok, mesiac, den + 1);
            int dnesJe = (int)my_date.DayOfWeek;

            TableRow riadok = new TableRow();
            this.vykaz_tbl.Controls.Add(riadok);

            TableCell dateCell = new TableCell();
            dateCell.ID = "dateCell_" + den.ToString();

            if (dnesJe == 6 || dnesJe == 0)
            {
                dateCell.CssClass = "box red";
            }
            if (dnesJe != 6 && dnesJe != 0 && sviatok == true)
            {
                dateCell.CssClass = "box yellow";
            }


            dateCell.Text = (den + 1).ToString();

            riadok.Controls.Add(dateCell);

            for (int col = 0; col < cols; col++)
            {
                TableCell dataCell = new TableCell();
                dataCell.ID = "dataCell_" + den.ToString() + "_" + col.ToString();

                if (dnesJe == 6 || dnesJe == 0)
                {
                    dataCell.CssClass = "box red";
                }

                if (dnesJe != 6 && dnesJe != 0 && sviatok)
                {
                    dataCell.CssClass = "box yellow";
                }

                //  dataCell.CssClass = "box red";
                TextBox tBox = new TextBox();
                tBox.ID = "textBox_" + den.ToString() + "_" + col.ToString();
                if (shift)
                {
                    tBox.Font.Bold = true;
                    tBox.BackColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    tBox.Font.Bold = false;
                }
                if (writeText) tBox.Text = rowData[col];
                dataCell.Controls.Add(tBox);
                riadok.Controls.Add(dataCell);
            }

        }
        catch (Exception ex)
        {
            //den = den - 1;
            // my_date = my_date.AddDays(1);
        }
    }

    protected void makeEmptyRow(int den, int cols, string[] rowData, int rok, int mesiac, Boolean sviatok, Boolean shift, Boolean writeText)
    {
        DateTime my_date = new DateTime(rok, mesiac, den + 1);

        int dnesJe = (int)my_date.DayOfWeek;

        TableRow riadok = new TableRow();
        this.vykaz_tbl.Controls.Add(riadok);

        TableCell dateCell = new TableCell();
        dateCell.ID = "dateCell_" + den.ToString();

        if (dnesJe == 6 || dnesJe == 0)
        {
            dateCell.CssClass = "box red";
        }
        if (dnesJe != 6 && dnesJe != 0 && sviatok == true)
        {
            dateCell.CssClass = "box yellow";
        }


        dateCell.Text = (den + 1).ToString();

        riadok.Controls.Add(dateCell);

        for (int col = 0; col < cols; col++)
        {
            TableCell dataCell = new TableCell();
            dataCell.ID = "dataCell_" + den.ToString() + "_" + col.ToString();

            if (dnesJe == 6 || dnesJe == 0)
            {
                dataCell.CssClass = "box red";
            }

            if (dnesJe != 6 && dnesJe != 0 && sviatok)
            {
                dataCell.CssClass = "box yellow";
            }

            //  dataCell.CssClass = "box red";
            TextBox tBox = new TextBox();
            tBox.ID = "textBox_" + den.ToString() + "_" + col.ToString();
            if (shift)
            {
                tBox.Font.Bold = true;
                tBox.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                tBox.Font.Bold = false;
            }
            if (writeText) tBox.Text = rowData[col];
            dataCell.Controls.Add(tBox);
            riadok.Controls.Add(dataCell);
        }
    }



    // TODO toto spravit univerzalne pre hoc ake oddelenie
    protected void setVykazTypesforNurses()
    {
        if (Session["KDCH_nurse_hours_type"] == null)
        {
            
            string query = "SELECT [data],[name] FROM [is_settings] WHERE [name]='KDCH_nurse_hours_type'";
            query = vykaz.mysql.buildSql(query, new string[] { vykaz.clinicIdf.ToString() });

            SortedList res = vykaz.mysql.getRow(query);

            string[] lines = res["data"].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            int linesCn = lines.Length;

            SortedList vykazTypes = new SortedList();

            for (int i = 0; i < linesCn; i++)
            {
                string[] tmp = lines[i].Split('|');
                vykazTypes.Add(tmp[0], tmp[1]);

            }
            Session.Add("KDCH_nurse_hours_type", vykazTypes);
        }
    }


    protected Dictionary<int, Hashtable> getShiftsNurse(int rok, int mesiac)
    {

        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }

       
        int dateGroup = vykaz.x2.makeDateGroup(rok, mesiac);
        string query = @"SELECT 
                                [datum],
                                [typ]
                                
                            FROM [is_sluzby_2_sestr] 
                        WHERE [user_id] = '{0}' 
                        AND [date_group]='{1}'
                        AND [deps] = '{2}' 
                        ORDER BY [datum] ASC";

        string dep = Session["oddelenie"].ToString();

        if (string.IsNullOrEmpty(dep))
        {
            dep = this.deps_dl.SelectedValue.ToString();
        }


        query = vykaz.mysql.buildSql(query, new string[] { Session["user_id"].ToString(), dateGroup.ToString(),dep });              
                        
                       
        Dictionary<int, Hashtable> result = vykaz.mysql.getTable(query);

        return result;
    }

    protected Dictionary<int, Hashtable> getActivities(int mesiac, int rok, string id)
    {
        //Dictionary<int, Hashtable> result = 
        int days = DateTime.DaysInMonth(rok, mesiac);

        string query = @"
                        SELECT  [t_dov.id] AS [dov_id], [t_dov.user_id] AS [user_id], [t_dov.od] AS [od],
                                [t_dov.do] AS [do], [t_dov.type] AS [type] FROM [is_dovolenky_sestr] AS [t_dov]
                            WHERE [t_dov.od] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00'
                            AND [t_dov.user_id]={3}
                            ORDER BY [t_dov.do] ASC";

        query = vykaz.mysql.buildSql(query, new string[] { rok.ToString(), mesiac.ToString(), days.ToString(), id.ToString() });

        Dictionary<int, Hashtable> table = vykaz.mysql.getTable(query);

        return table;
    }



    protected void fillInVacations(int mesiac, int rok, string id)
    {
        //ArrayList dovolenky = x_db.getDovolenkyByID(mesiac, rok, Convert.ToInt32(id));
        Dictionary<int, Hashtable> dovolenky = this.getActivities(mesiac, rok, id);
        int dovCnt = dovolenky.Count;

        ContentPlaceHolder ctpl = new ContentPlaceHolder();

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        for (int i = 0; i < dovCnt; i++)
        {
            //string[] data = dovolenky[i].ToString().Split(';');

            //string dd1 = my_x2.MSDate(data[1].ToString());
            //string dd2 = my_x2.MSDate(data[2].ToString());

            DateTime odDov = Convert.ToDateTime(vykaz.x2.UnixToMsDateTime(dovolenky[i]["od"].ToString()));
            DateTime doDov = Convert.ToDateTime(vykaz.x2.UnixToMsDateTime(dovolenky[i]["do"].ToString()));
            string[] freeDays = Session["freedays"].ToString().Split(',');

            for (DateTime ddStart = odDov; ddStart <= doDov; ddStart += TimeSpan.FromDays(1))
            {
                if (ddStart.Month == mesiac && ddStart.Year == rok)
                {
                    int vikend = (int)ddStart.DayOfWeek;


                    string mesDen = ddStart.Day.ToString() + "." + mesiac;

                    int rs_tmp = Array.IndexOf(freeDays, mesDen);

                    if (vikend != 0 && vikend != 6 && rs_tmp == -1)
                    {

                        int ddTemp = ddStart.Day - 1;
                        Control tbox = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_0");
                        Control tbox1 = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_3");

                        Control tbox2 = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_1");
                        Control tbox3 = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_2");


                        TextBox my_text_box = (TextBox)tbox;
                        TextBox my_text_box1 = (TextBox)tbox1;

                        TextBox my_text_box2 = (TextBox)tbox2;
                        TextBox my_text_box3 = (TextBox)tbox3;

                        if (dovolenky[i]["type"].ToString() == "do") { my_text_box.Text = "D"; my_text_box1.Text = "D"; my_text_box2.Text = "0"; my_text_box3.Text = "0"; }
                        if (dovolenky[i]["type"].ToString() == "pn") { my_text_box.Text = "PN"; my_text_box1.Text = "PN"; my_text_box2.Text = "0"; my_text_box3.Text = "0"; }
                        if (dovolenky[i]["type"].ToString() == "sk") { my_text_box.Text = "SK"; my_text_box1.Text = "SK"; my_text_box2.Text = "0"; my_text_box3.Text = "0"; }
                        if (dovolenky[i]["type"].ToString() == "le") { my_text_box.Text = "SK"; my_text_box1.Text = "Le"; my_text_box2.Text = "0"; my_text_box3.Text = "0"; }


                    }
                }
            }
        }
    }



    protected void newVykaz_fnc(object sender, EventArgs e)
    {

    }

    protected void generateVykaz_fnc(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        this.generateVykazNurse(mesiac,rok,true);
    }

    protected void generateEpc_fnc(object sender, EventArgs e)
    {

    }


    protected void calcData_Click(object sender, EventArgs e)
    {

    }

    protected void createPdf_btn_fnc(object sender, EventArgs e)
    {

    }

}