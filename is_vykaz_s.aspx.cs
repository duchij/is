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

    private string _userId;

    private Boolean _editable;
    private string[] _vykazHeader;

    private SortedList _userData;

    public SortedList userData
    {
        get { return _userData; }
        set { _userData = value; }
    }

    public string userId
    {
        get { return _userId; }
        set { _userId = value; }
    }

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
    VykazClass vykaz = new VykazClass(); 


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
        vykaz.userData = new SortedList();

        vykaz.department = Session["oddelenie"].ToString();

        this.setDepsNurse();

        

        if (vykaz.editable)
        {
            this.anotherUser_pl.Visible = true;
            this.vykazInfoHours_pl.Visible = true;
        }
        else
        {
            this.anotherUser_pl.Visible = false;
            this.vykazInfoHours_pl.Visible = true;
        }

        if (vykaz.editable)
        {
            this.loadDeps();
           // this.loadNurses_fnc(sender, e);
        }


        if (!IsPostBack)
        {
            vykaz.x2.fillYearMonth(ref this.mesiac_cb, ref this.rok_cb, Session["month_dl"].ToString(), Session["years_dl"].ToString());

            DateTime dnesJe = DateTime.Today;
            this.mesiac_cb.SelectedValue = dnesJe.Month.ToString();
            this.rok_cb.SelectedValue = dnesJe.Year.ToString();

            this.loadDeps();

            
            
        }
        else
        {
            //this.vykaz_tbl.Controls.Clear();
            int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
            int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
            
            this.generateVykazNurse(mesiac, rok, true);
        }

       
    }

    protected void setNurseData_fnc(object sender, EventArgs e)
    {
        this.setDepsNurse();
        this.generateVykaz_btn.Enabled = false;
    }



    protected void setDepsNurse()
    {
        string deps = this.deps_dl.SelectedValue.ToString();
        string nurse = this.nurses_dl.SelectedValue.ToString();

        if (deps!= "0" && nurse != "0" && !string.IsNullOrEmpty(nurse))
        {
            vykaz.department = deps;
            vykaz.userId = nurse;

            SortedList res = this.getUserData(nurse);

            if (res["status"] != null)
            {
                vykaz.x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
            }
            else
            {
                vykaz.userData = res;
                this.generateVykaz_btn.Enabled = false;

            }

        }
        else
        {
            vykaz.department = Session["oddelenie"].ToString();
            vykaz.userId = Session["user_id"].ToString();
        }

    }

    protected void saveData()
    {

        int cols = vykaz.vykazHeader.Length;

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        int days = DateTime.DaysInMonth(rok, mesiac);
        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;
        string[] riadok = new string[days];

        for (int den = 0; den < days; den++)
        {
            string[] tmp = new string[cols];
            for (int col = 0; col < cols; col++)
            {
                Control Tbox = ctpl.FindControl("textBox_" + den.ToString() + "_" + col.ToString());
                TextBox sumBox = (TextBox)Tbox;
                string sum = sumBox.Text.ToString();
                sum = sum.Replace('.', ',');

                tmp[col] = sum;
            }
            string tmpStr = string.Join("~", tmp);
            riadok[den] = tmpStr;

        }
        string finalStr = string.Join("|", riadok);
        //this.msg_lbl.Visible = true;
        // this.msg_lbl.Text = finalStr;

        SortedList data = new SortedList();
        if (vykaz.userData.Count > 0)
        {
            data.Add("user_id", vykaz.userId);
        }
        else
        {
            data.Add("user_id", Session["user_id"].ToString());
        }
        
        data.Add("mesiac", mesiac.ToString());
        data.Add("rok", rok.ToString());
        data.Add("vykaz", finalStr);
        data.Add("prenos", this.rozdiel_lbl.Text);

        SortedList res = vykaz.mysql.mysql_insert("is_vykaz", data);

        Boolean status = Convert.ToBoolean(res["status"].ToString());

        if (!(Boolean)res["status"])
        {
            vykaz.x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
        }
        else
        {
            this.createPdf_btn.Enabled = true;
        }



    }




    protected SortedList getUserData(string id)
    {
        string sql = "SELECT [titul_pred],[titul_za],[full_name],[pracdoba],[tyzdoba],[zaradenie],[osobcisl] FROM [is_users] WHERE [id]={0} ";

        sql = vykaz.mysql.buildSql(sql, new string[] { id });

        SortedList res = vykaz.mysql.getRow(sql);

        return res;

    }

    protected void loadNurses_fnc(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string dep = this.deps_dl.SelectedValue.ToString();

            string query = "SELECT [id], [name3] FROM [kdch_nurse] WHERE [idf]='{0}'";

            query = vykaz.mysql.buildSql(query, new string[] { dep });

            Dictionary<int, Hashtable> table = vykaz.mysql.getTable(query);

            int nurseLn = table.Count;
            this.nurses_dl.Items.Clear();
            this.nurses_dl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
            for (int nurse = 0; nurse < nurseLn; nurse++)
            {

                this.nurses_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[nurse]["name3"].ToString(), table[nurse]["id"].ToString()));

            }
        }
        


    }

    protected void loadDeps()
    {
        

        
        if (!IsPostBack)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM [is_deps] WHERE [clinic_id]='{0}'", Session["klinika_id"]);

            Dictionary<int, Hashtable> table = vykaz.mysql.getTable(sb.ToString());


            int depsLn = table.Count;

            this.deps_dl.Items.Clear();
            this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
            for (int dep = 0; dep < depsLn; dep++)
            {
            
                this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[dep]["label"].ToString(), table[dep]["idf"].ToString()));
            
            }
        }
        
    }

    protected void generateVykazNurse(int mesiac, int rok, Boolean writeText)
    {
        // this.vykaz_tbl.Controls.Clear();

        this.getPrenos(mesiac, rok);

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
            this.fillInVacations(mesiac, rok, vykaz.userId);

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

      //  string endHour = "";
      //  Boolean epcYes = true;

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

        Control workType = ctpl.FindControl("workType_" + row.ToString());
        Label workType_lbl = (Label)workType;
        workType_lbl.Text = typ;



        Control prichod = ctpl.FindControl("textBox_" + row.ToString() + "_0");
        TextBox prichod_txt = (TextBox)prichod;
        prichod_txt.Text = rowData[0];


        Control obed_zac = ctpl.FindControl("textBox_" + row.ToString() + "_1");
        TextBox obed_zac_txt = (TextBox)obed_zac;
        obed_zac_txt.Text = rowData[1];

        Control obed_konc = ctpl.FindControl("textBox_" + row.ToString() + "_2");
        TextBox obed_konc_txt = (TextBox)obed_konc;
        obed_konc_txt.Text = rowData[2];

        Control koncPrac = ctpl.FindControl("textBox_" + row.ToString() + "_3");
        TextBox koncPrac_txt = (TextBox)koncPrac;
        koncPrac_txt.Text = rowData[3];


        Control hodiny = ctpl.FindControl("textBox_" + row.ToString() + "_4");
        TextBox hodiny_txt = (TextBox)hodiny;
        hodiny_txt.Text = rowData[4];

        Control nocPrac = ctpl.FindControl("textBox_" + row.ToString() + "_5");
        TextBox nocPrac_txt = (TextBox)nocPrac;
        nocPrac_txt.Text = rowData[5];

        if (vikend && sviatok)
        {
            Control mzvyhTmp = ctpl.FindControl("textBox_" + row.ToString() + "_7");
            TextBox mzvyhTxt = (TextBox)mzvyhTmp;
            mzvyhTxt.Text = rowData[4];

            Control mzvyhTmpSv = ctpl.FindControl("textBox_" + row.ToString() + "_6");
            TextBox mzvyhTxtSv = (TextBox)mzvyhTmpSv;
            mzvyhTxtSv.Text = rowData[4];
        }
        else if (vikend && !sviatok)
        {
            Control mzvyhTmp = ctpl.FindControl("textBox_" + row.ToString() + "_6");
            TextBox mzvyhTxt = (TextBox)mzvyhTmp;
            mzvyhTxt.Text = rowData[4];

        } else if (!vikend && sviatok)
        {
            Control mzvyhTmpSv = ctpl.FindControl("textBox_" + row.ToString() + "_7");
            TextBox mzvyhTxtSv = (TextBox)mzvyhTmpSv;
            mzvyhTxtSv.Text = rowData[4];
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
                    if (tt ==3)
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


            //dateCell.Text = (den + 1).ToString();

            Label day_lbl = new Label();
            day_lbl.Text = "<strong>"+(den + 1).ToString()+"</strong> ";
            dateCell.Controls.Add(day_lbl);

            Label workType_lbl = new Label();
            workType_lbl.ID = "workType_" + den.ToString();
            dateCell.Controls.Add(workType_lbl);

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

        query = vykaz.mysql.buildSql(query, new string[] { vykaz.userId, dateGroup.ToString(),vykaz.department });              
                        
                       
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

                        

                        //    Control 


                        TextBox my_text_box = (TextBox)tbox;
                        TextBox my_text_box1 = (TextBox)tbox1;

                        TextBox my_text_box2 = (TextBox)tbox2;
                        TextBox my_text_box3 = (TextBox)tbox3;

                       

                        if (dovolenky[i]["type"].ToString() == "do")
                        {
                            my_text_box.Text = "0";
                            my_text_box1.Text = "Dov";
                            my_text_box2.Text = "0";
                            my_text_box3.Text = "0";

                            
                            
                            if (vykaz.userData.ContainsKey("pracdoba") != false)
                            {
                                Control pracDoba = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_4");
                                TextBox pracDoba_txt = (TextBox)pracDoba;
                                pracDoba_txt.Text = vykaz.x2.getStr(vykaz.userData["pracdoba"].ToString());
                               // pracDoba_txt.Text = "tr";
                                //vykaz.x2log.logData(vykaz.userData["pracdoba"], "", "oble");
                            }
                            else
                            {
                                //pracDoba_txt.Text = vykaz.x2.getStr(Session["pracdoba"].ToString());
                            }
                            //pracDoba_txt.Text = "g";
                            // pracDoba_txt.Text = "T";

                        }
                        if (dovolenky[i]["type"].ToString() == "pn")
                        {
                            my_text_box.Text = "PN";
                            my_text_box1.Text = "PN";
                            my_text_box2.Text = "0";
                            my_text_box3.Text = "0";
                            Control pracDoba = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_4");
                            TextBox pracDoba_txt = (TextBox)pracDoba;
                            if (vykaz.userData.Count > 0)
                            {
                                pracDoba_txt.Text = vykaz.x2.getStr(vykaz.userData["pracdoba"].ToString());
                            }
                            else
                            {
                                pracDoba_txt.Text = vykaz.x2.getStr(Session["pracdoba"].ToString());
                            }
                        }

                        if (dovolenky[i]["type"].ToString() == "sk")
                        {
                            my_text_box.Text = "SK";
                            my_text_box1.Text = "SK";
                            my_text_box2.Text = "0";
                            my_text_box3.Text = "0";
                            Control pracDoba = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_4");
                            TextBox pracDoba_txt = (TextBox)pracDoba;
                            if (vykaz.userData.Count > 0)
                            {
                                pracDoba_txt.Text = vykaz.x2.getStr(vykaz.userData["pracdoba"].ToString());
                            }
                            else
                            {
                                pracDoba_txt.Text = vykaz.x2.getStr(Session["pracdoba"].ToString());
                            }
                        }

                        if (dovolenky[i]["type"].ToString() == "le")
                        {
                            my_text_box.Text = "SK";
                            my_text_box1.Text = "SK";
                            my_text_box2.Text = "0";
                            my_text_box3.Text = "0";
                            Control pracDoba = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_4");
                            TextBox pracDoba_txt = (TextBox)pracDoba;
                            if (vykaz.userData.Count > 0)
                            {
                                pracDoba_txt.Text = vykaz.x2.getStr(vykaz.userData["pracdoba"].ToString());
                            }
                            else
                            {
                                pracDoba_txt.Text = vykaz.x2.getStr(Session["pracdoba"].ToString());
                            }
                        }
                    }
                }
            }
        }
    }



    protected void newVykaz_fnc(object sender, EventArgs e)
    {
        Response.Redirect("is_vykaz_s.aspx");
    }

    protected void generateVykaz_fnc(object sender, EventArgs e)
    {
        Button sendBtn = (Button)sender;

        if (sendBtn.ID == "generateVykaz_btn")
        {
            vykaz.userData.Clear();
        }


       // vykaz.userData.Clear();
       
       // this.generateVykaz_btn.Enabled = false;

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        this.vykaz_tbl.Controls.Clear();
        this.generateVykazNurse(mesiac,rok,true);
    }

    protected void generateEpc_fnc(object sender, EventArgs e)
    {

    }


    protected void calcData_Click(object sender, EventArgs e)
    {
        this._calcData();
    }


    protected void getPrenos(int mesiac, int rok)
    {

        DateTime dt = new DateTime(rok, mesiac,1);

        DateTime lastMonth = dt.AddMonths(-1);

        string query = @"SELECT [prenos] FROM [is_vykaz] WHERE [user_id] ={0} AND [mesiac] = {1} AND [rok] = {2}";

        string userId = "";

        if (vykaz.userData.Count > 0)
        {
            userId = vykaz.userId;
        }
        else
        {
            userId = Session["user_id"].ToString();
        }

        query = vykaz.mysql.buildSql(query, new string[] { userId, lastMonth.Month.ToString(), lastMonth.Year.ToString() });

        SortedList row = vykaz.mysql.getRow(query);

        vykaz.x2log.logData(row, "", "pokus");

        if (row.Count > 0)
        {
            string tmp = vykaz.x2.getStr(row["prenos"].ToString());

            if (tmp.Length == 0)
            {
                this.predMes_txt.Text = "0";
            }
            else
            {
                this.predMes_txt.Text = tmp;
            }
        }
        else
        {
            this.predMes_txt.Text = "0";
        }
    }

    protected decimal verifyNumber(string number)
    {
        decimal num;
        bool status = Decimal.TryParse(number, out num);
        if (!status)
        {
            num = 0;
        }
        return num;
    }


    protected void _calcData()
    {
        //this.createVykaz(false);

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        //this.generateVykazNurse(mesiac, rok, false);


        int cols = vykaz.vykazHeader.Length;



        int days = DateTime.DaysInMonth(rok, mesiac);
        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;
        decimal suma = 0;
        for (int col = 4; col < cols; col++)
        {
            suma = 0;
            for (int den = 0; den < days; den++)
            {
                Control Tbox = ctpl.FindControl("textBox_" + den.ToString() + "_" + col.ToString());
                TextBox sumBox = (TextBox)Tbox;
                string sum = sumBox.Text.ToString();
                sum = sum.Replace('.', ',');


                suma += this.verifyNumber(sum);
            }
            Control resTbox = ctpl.FindControl("head_tbox_" + col.ToString());
            TextBox resTxt = (TextBox)resTbox;

            if (col == 4)
            {
                float prn = Convert.ToSingle(this.predMes_txt.Text.ToString().Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);

                suma = suma + (decimal)prn;
                resTxt.Text = suma.ToString();

            }
            else
            {
                resTxt.Text = suma.ToString();
            }

        }
        DateTime od_date = new DateTime(rok, mesiac, 1);
        DateTime do_date = new DateTime(rok, mesiac, days);

        int pocetVolnychDni = vykaz.x2.pocetVolnychDniBezSviatkov(od_date, do_date);

        int pocetPracdni = days - pocetVolnychDni;

        string ineDni = this.ine_p_dni_txt.Text.ToString();

        if (ineDni.Trim().Length > 0)
        {
            try
            {
                pocetPracdni = Convert.ToInt32(ineDni);
            }
            catch (Exception ex)
            {

            }

        }

        decimal pocetPracHod = 0;

        if (Session["pracdoba"].ToString().Length != 0 && this.nurses_dl.SelectedValue.ToString().Length == 0)
        {
            Session["pracdoba"] = Session["pracdoba"].ToString().Replace(".", ",");

            

        }else
        {
            //toto je fallback ak nic nieje nastavene....
            pocetPracHod = pocetPracdni * Convert.ToDecimal("7,5");
        }


        if (vykaz.userData.Count > 0)
        {
            vykaz.userData["pracdoba"] = vykaz.userData["pracdoba"].ToString().Replace(".", ",");
            pocetPracHod = pocetPracdni * Convert.ToDecimal(vykaz.userData["pracdoba"]);

        }
        else
        {
            //toto je fallback ak nic nieje nastavene....
            pocetPracHod = pocetPracdni * Convert.ToDecimal("7,5");
        }



        //vypocet nadcasov
        decimal sumNadcas = 0;
        try
        {

            Control nadcas1 = ctpl.FindControl("head_tbox_8");
            TextBox nadcas1_txt = (TextBox)nadcas1;

            Control nadcas2 = ctpl.FindControl("head_tbox_9");
            TextBox nadcas2_txt = (TextBox)nadcas2;

            decimal nad1 = Convert.ToDecimal(nadcas1_txt.Text.ToString());
            decimal nad2 = Convert.ToDecimal(nadcas2_txt.Text.ToString());

            sumNadcas = nad1 + nad2;




        }catch(Exception ex)
        {
            vykaz.x2.errorMessage2(ref this.msg_lbl, "Zle hodnoty v nadcasoch: " + ex.ToString());
        }


        this.pocetHod_txt.Text = pocetPracHod.ToString();
        Control resTbox_roz = ctpl.FindControl("head_tbox_" + "4");
        TextBox resTxt_roz = (TextBox)resTbox_roz;

        decimal real = Convert.ToDecimal(resTxt_roz.Text.ToString());
        real = real - sumNadcas;
        resTxt_roz.Text = real.ToString();
        string prenosStr = this.predMes_txt.Text.ToString();
        prenosStr = prenosStr.Replace('.', ',');
        decimal prenos = Convert.ToDecimal(prenosStr);

        
        

        this.rozdiel_lbl.Text = (real - pocetPracHod).ToString();





        
        
        



        this.createPdf_btn.Enabled = true;
        //this.
        this.saveData();

    }

    protected void createPdf_btn_fnc(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        //this.createVykaz();

        this.calcData_Click(sender, e);
        //this.createEmptyVykaz();
        this.createPdf(rok, mesiac);
    }


    protected void createPdf(int rok, int mesiac)
    {
        //int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        //int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int milis = DateTime.Now.Millisecond;
        string path = Server.MapPath("App_Data");
        string imagepath = Server.MapPath("App_Data");
        string oldFile = @path + "\\vykaz.pdf";
        string hash = vykaz.x2.makeFileHash(Session["login"].ToString() + milis.ToString());
        string newFile = @path + "\\vykaz_" + hash + ".pdf";
        this.msg_lbl.Text = oldFile;
        // open the reader
        PdfReader reader = new PdfReader(oldFile);
        Rectangle size = reader.GetPageSizeWithRotation(1);
        Document myDoc = new Document(PageSize.A4);

        //1cm == 28.3pt



        // open the writer
        FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
        PdfWriter writer = PdfWriter.GetInstance(myDoc, fs);
        myDoc.Open();

        // the pdf content
        //PdfWriter pw = writer.DirectContent;
        PdfContentByte cb = writer.DirectContent;

        double[] koor = new double[14];
        koor[0] = 34; //typ
        koor[1] = 69; //prichod
        koor[2] = 100; //obed zaciatok
        koor[3] = 130; //obed koniec
        koor[4] = 167; //odchod
        koor[5] = 199; //zuctovac hodiny
        koor[6] = 300; //nocna praca
        koor[7] = 329.55; //mzdove zvyhod
        koor[8] = 356.84; //sviatok
        koor[9] = 390.75; //aktivna1
        koor[10] = 420.42; //aktivna2
        koor[11] = 451.26; //neaktivna1
        koor[12] = 482.09; //neaktivna2
        koor[13] = 511.76; //neaktivna4
                           // koor[13] = 507.37; //neaktivna3

        double odHora = 218;


        //string lila = "hura";
        BaseFont mojFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
        cb.SetFontAndSize(mojFont, 10);

        //cb.SetColorStroke(new CMYKColor(1f, 0f, 0f, 0f));
        //cb.SetColorFill(new CMYKColor(0f, 0f, 1f, 0f));

        cb.SetColorStroke(BaseColor.LIGHT_GRAY);
        cb.SetColorFill(BaseColor.LIGHT_GRAY);

        string[] freeDays = Session["freedays"].ToString().Split(',');



        int days = DateTime.DaysInMonth(rok, mesiac);
        double kof = 12.4;
        for (int i = 0; i < days; i++)
        {
            int den = i + 1;
            /*if (i == 1)
            {
                cb.Rectangle(46, size.Height-(float)odHora, 10, 10);
                cb.Fill();
            }*/

            string mesDen = den.ToString() + "." + mesiac.ToString();

            int rs_tmp = Array.IndexOf(freeDays, mesDen);

            DateTime my_date = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), den);
            int dnesJe = (int)my_date.DayOfWeek;

            if (dnesJe == 0 || dnesJe == 6 || rs_tmp != -1)
            {
                //173.22
                //vyska stlpca je 11
                //od lava 46
                //dlzka je 423.7

                /* cb.MoveTo(46, size-odHora +(11*i));
                 cb.LineTo(469, size - odHora + (11 * i));
                 cb.LineTo(469, size - odHora + (11 * i)-11);
                 cb.LineTo(46, size - odHora + (11 * i) - 11);
                 //Path closed, stroked and filled
                 cb.ClosePathFillStroke();*/
                double recY = (size.Height - (odHora + 0)) - (kof * i);

                float recYY = (float)recY;

                cb.Rectangle(34, recYY, 540, 11);
                //cb.Stroke();
                cb.Fill();
            }


        }
        cb.SetColorStroke(BaseColor.BLACK);
        cb.SetColorFill(BaseColor.BLACK);

        cb.BeginText();
        cb.MoveText(291, size.Height - 68);
        cb.ShowText(this.mesiac_cb.SelectedItem.ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(416, size.Height - 68);
        cb.ShowText(rok.ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(233, size.Height - 97);
        if (vykaz.x2.getStr(Session["klinika_label"].ToString()).Length > 0)
        {
            cb.ShowText(Session["klinika_label"].ToString());
        }
        else
        {
            cb.ShowText("");
        }
        cb.EndText();

        cb.BeginText();
        cb.MoveText(26, size.Height - 97);
        
        if (vykaz.userData.Count > 0)
        {
            cb.ShowText(vykaz.x2.getStr(vykaz.userData["titul_pred"].ToString()) + vykaz.userData["full_name"].ToString() + " " + vykaz.x2.getStr(vykaz.userData["titul_za"].ToString()));
        }else
        {
            cb.ShowText(Session["titul_pred"].ToString() + Session["fullname"].ToString() + " " + Session["titul_za"].ToString());
        }
        cb.EndText();


        cb.BeginText();
        cb.MoveText(388, size.Height - 97);
        if (vykaz.userData.Count > 0)
        {
            cb.ShowText(vykaz.x2.getStr(vykaz.userData["zaradenie"].ToString()));
        }
        else
        {
            cb.ShowText(Session["zaradenie"].ToString());
        }
        

        cb.EndText();

        cb.BeginText();
        cb.MoveText(166, size.Height - 203);
        cb.ShowText(this.predMes_txt.Text.ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText((float)koor[5], size.Height - 628);
        cb.ShowText(this.rozdiel_lbl.Text.ToString());
        cb.EndText();

        cb.BeginText();

        string osobcisl = "";

        if (vykaz.userData.Count > 0)
        {
            osobcisl = vykaz.x2.getStr(vykaz.userData["osobcisl"].ToString());
        }
        else
        {
            osobcisl = Session["osobcisl"].ToString();
        }
        
        cb.MoveText(480, size.Height - 68);

        if (osobcisl.Length > 0)
        {
            cb.ShowText(osobcisl);
        }
        else
        {
            cb.ShowText("");
        }
        cb.EndText();

        cb.BeginText();
        cb.MoveText(388, size.Height - 127);
        cb.ShowText(this.pocetHod_txt.Text.ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(130, size.Height - 127);
        String tyzdoba = "";
        if (vykaz.userData.Count > 0)
        {
            tyzdoba = vykaz.x2.getStr(vykaz.userData["tyzdoba"].ToString());
        }
        else
        {
            tyzdoba = Session["tyzdoba"].ToString();
        }
        if (tyzdoba.Length > 0)
        {
            tyzdoba = tyzdoba.Replace(',', '.');
            cb.ShowText(tyzdoba);
        }
        else
        {
            cb.ShowText("0");
        }

        cb.EndText();

        //int days = DateTime.DaysInMonth(rok, mesiac);
        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        int cols = vykaz.vykazHeader.Length;

        for (int col = 4; col < cols; col++)
        {
            Control THbox = ctpl.FindControl("head_tbox_" + col.ToString());
            TextBox hBox = (TextBox)THbox;
            cb.BeginText();
            string num = hBox.Text.ToString();
            cb.MoveText((float)koor[col+1], size.Height - 604);
            cb.ShowText(num);


            cb.EndText();
        }
        kof = 12.3;
        for (int den = 0; den < days; den++)
        {
            for (int col = 0; col < cols; col++)
            {

                double recY = (size.Height - (odHora + 0)) - (kof * den);
                float recYY = (float)recY;

                if (col == 0)
                {
                    Control workType = ctpl.FindControl("workType_" + den.ToString());
                    Label workType_lbl = (Label)workType;

                    string typ = workType_lbl.Text.ToString();
                    cb.BeginText();
                    cb.MoveText((float)koor[col], recYY);

                    if (typ.Length > 0)
                    {
                        if (typ.IndexOf("S") !=-1)
                        {
                            typ = "R";
                            
                        }
                        else if (typ == "A1")
                        {
                            typ = "R";
                        }
                        else if (typ == "A2")
                        {
                            typ = "N";
                        }
                        else if (typ == "ZD")
                        {
                            typ = "R";
                        }
                        else if (typ == "ZN")
                        {
                            typ = "N";
                        }
                        else
                        {
                            typ = typ.Substring(0, 1);
                        }
                       
                    }
                    

                    cb.ShowText(typ);
                    cb.EndText();
                }
                else
                {
                    Control Tbox = ctpl.FindControl("textbox_" + den.ToString() + "_" +(col-1).ToString());
                    TextBox mBox = (TextBox)Tbox;
                    cb.BeginText();
                    string num = mBox.Text.ToString();
                    if (num == "0") num = "";

                    cb.MoveText((float)koor[col], recYY);
                    cb.ShowText(num);

                    cb.EndText();
                }
                
            }
        }

        PdfImportedPage page = writer.GetImportedPage(reader, 1);
        cb.AddTemplate(page, 0, 0);


        myDoc.Close();
        fs.Close();
        writer.Close();
        reader.Close();

        //Response.Redirect(@path + "\\vykaz_new.pdf");
        SortedList res = vykaz.mysql.registerTempFile("vykaz_" + hash + ".pdf", 5,@"~/App_Data/");


        if ((Boolean)res["status"])
        {

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=vykaz_" + hash + ".pdf");
            Response.TransmitFile(@path + "\\vykaz_" + hash + ".pdf");
            Response.End();
        }
        else
        {
            vykaz.x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
        }


    }

    protected float cmPt(double number)
    {
        string unit = "28,3464";
        double res = number * Convert.ToDouble(unit);

        float result = (float)res;

        return result;
    }

}