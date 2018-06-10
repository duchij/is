using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class vykaz2 : System.Web.UI.Page
{
    public  x2_var my_x2 = new x2_var();
   // vykazdb x_db = new vykazdb();
    public log x2log = new log();
    public mysql_db x2Mysql = new mysql_db();
    public my_db x_db = new my_db();
    public string[] vykazHeader;
    public string rights;

    public SortedList gData = new SortedList();
    public string gKlinika;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.gKlinika = Session["klinika"].ToString().ToLower();
        if (this.gKlinika == "2dk") this.setVykazTypesforDK();
        if (!IsPostBack)
        {
            my_x2.fillYearMonth(ref this.mesiac_cb, ref this.rok_cb, Session["month_dl"].ToString(), Session["years_dl"].ToString());

            //this.msg_lbl.Text = ViewState["head_tbox_4"].ToString();
           //this.createVykaz(false);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        

       
        if (Session["pracdoba"].ToString().Trim().Length == 0 || Session["tyzdoba"].ToString().Trim().Length == 0 || Session["osobcisl"].ToString().Trim().Length == 0)
        {

            // Page page = HttpContext.Current.CurrentHandler as Page;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('" + Resources.Resource.vykaz_error + "');", true);

            //Response.Redirect("adduser.aspx");
        }

        this.rights = Session["rights"].ToString();


        //this.zaMesiac_lbl.Text = "Maj,2012";

       // this.msg_lbl.Visible = false;

        if (!IsPostBack)
        {
            DateTime dnesJe = DateTime.Today;
            this.mesiac_cb.SelectedValue = dnesJe.Month.ToString();
            this.rok_cb.SelectedValue = dnesJe.Year.ToString();

           // Session["vykaz_akt_month"] = dnesJe.Month;
         //   Session["vykaz_akt_year"] = dnesJe.Year;
            
            if ((this.rights == "poweruser" || Session["login"].ToString() == "lsykora") || this.rights == "admin")
            {
                this.anotherUser_pl.Visible = true;
            }
            else
            {
                this.anotherUser_pl.Visible = false;
            }
            //this.createVykaz(true);
        }
        else
        {
           this.createVykaz(true);
            //this.createVykaz(false);
            //string mes = this.mesiac_cb.SelectedValue.ToString();
            //string rok = this.rok_cb.SelectedValue.ToString();

            //if (Session["vykaz_akt_month"].ToString() == mes && Session["vykaz_akt_year"].ToString() == rok)
            //{
            //    //this.msg_lbl.Text = sender.ToString();
            //    this.createVykaz(false);
            //}
            //else
            //{
            //    Session["vykaz_akt_month"] = mes;
            //    Session["vykaz_akt_year"] = rok;
            //    this.vykaz_tbl.Controls.Clear();
            //    this.createVykaz(true);
                
            //}
        }

       

        //this.generateVykaz(Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString()),Convert.ToInt32(this.rok_cb.SelectedValue.ToString()));


    }

    protected void generateVykaz_fnc(object sender, EventArgs e)
    {
        this.mesiac_cb.Enabled = false;
        this.rok_cb.Enabled = false;
        this.vykazInfoHours_pl.Visible = true;
        this.generateVykaz_btn.Enabled = false;
        this.newVykaz_btn.Enabled = true;

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        this.getPrenos(mesiac, rok);
       // this.createVykaz(true);

    }

    protected void newVykaz_fnc(object sender, EventArgs e)
    {
        Response.Redirect("vykaz2.aspx",false);
    }

    protected void getPrenos(int mesiac, int rok)
    {

        if (mesiac == 12)
        {
            mesiac = 1;
            rok--;
        }
        else
        {
            mesiac--;
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("SELECT [prenos] FROM [is_vykaz] WHERE [user_id] ='{0}' AND [mesiac] = '{1}' AND [rok] = '{2}'", Session["user_id"], mesiac, rok);

        SortedList row = x2Mysql.getRow(sb.ToString());
        
        x2log.logData(row,"","pokus");

        if (row.Count > 0)
        {
            string tmp =my_x2.getStr(row["prenos"].ToString());

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

    protected void createVykaz(Boolean writeText)
    {
        
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_vykaz] WHERE [mesiac]='{0}' AND [rok]='{1}' AND [user_id]='{2}'",mesiac,rok,Session["user_id"].ToString());
        SortedList row = x2Mysql.getRow(sb.ToString());
        if (row.Count > 0)
        {
            this.reCreateVykaz(row,mesiac,rok,writeText);
        }
        else
        {
            if (this.gKlinika == "kdch")
            {
                this.generateVykaz(mesiac, rok, writeText);
            }
            if (this.gKlinika == "2dk")
            {
                this.generateVykazDK(mesiac, rok, writeText);
            }
            
        }

        //this._calcData();
    }

    protected void reCreateVykaz(SortedList data, int mesiac, int rok, Boolean writeText)
    {
        this.vykaz_tbl.Controls.Clear();

        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM [is_settings]  WHERE [name]='vykaz_doctors'");

        SortedList row = x2Mysql.getRow(sb.ToString());
        this.vykazHeader = row["data"].ToString().Split(',');

        int cols = this.vykazHeader.Length;
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
            headLabel.ID = this.vykazHeader[col] + "_lbl<br>";
            headLabel.Font.Size = FontUnit.Point(8);
            headLabel.Text = this.vykazHeader[col];

            headCell.Controls.Add(headLabel);

            TextBox tBox = new TextBox();
            tBox.ID = "head_tbox_" + col.ToString();
            tBox.Text = "";
           
            headCell.Controls.Add(tBox);


            headerRow.Controls.Add(headCell);
        }

        int dniMes = DateTime.DaysInMonth(rok, mesiac);
        string[] sviatky = x_db.getFreeDays();

        string vykazStr = data["vykaz"].ToString();
        string[] vykazRiadok = vykazStr.Split('|');
        Boolean[] docShifts = this.getShifts(rok, mesiac);

        

        for (int den = 0; den < dniMes; den++)
        {
            DateTime my_date = new DateTime(rok, mesiac, den + 1);
            int dnesJe = (int)my_date.DayOfWeek;
            string[] rowData = vykazRiadok[den].Split('~');

            string dentmp = (den+1).ToString() + "." + mesiac.ToString();

            int res = Array.IndexOf(sviatky, dentmp);

            if (docShifts[den])
            {
               
                if (res == -1)
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, false, true,writeText);
                }
                else
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, true, true, writeText);
                }
            }
            else
            {
                if (res == -1)
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                }
                else
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, true, false, writeText);
                }
            }
        }
    }

    protected void makeRow(int den, int cols, string[] rowData, int rok, int mesiac, Boolean sviatok, Boolean shift, Boolean writeText)
    {
        //den = den + 1;
      
        try
        {
            DateTime my_date = new DateTime(rok, mesiac, den+1);
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



    protected void generateVykaz(int mesiac, int rok, Boolean writeText)
    {
        this.vykaz_tbl.Controls.Clear();

        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM [is_settings]  WHERE [name]='vykaz_doctors'");

        SortedList row = x2Mysql.getRow(sb.ToString());
        this.vykazHeader = row["data"].ToString().Split(',');

        int cols = this.vykazHeader.Length;
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
            headLabel.ID = this.vykazHeader[col] + "_lbl<br>";
            headLabel.Font.Size = FontUnit.Point(8);
            headLabel.Text = this.vykazHeader[col];
          
            headCell.Controls.Add(headLabel);

            TextBox tBox = new TextBox();
            tBox.ID = "head_tbox_" + col.ToString();
            tBox.Text = "";
            
            headCell.Controls.Add(tBox);


            headerRow.Controls.Add(headCell);
        }

        int dniMes = DateTime.DaysInMonth(rok, mesiac);
        string[] sviatky = x_db.getFreeDays();

        Boolean[] docShifts = this.getShifts(rok, mesiac);

        SortedList vykazVypis = this.getValueFromSluzba();

        for (int den = 0; den < dniMes; den++)
        {
           // 
            DateTime my_date = new DateTime(rok,mesiac, den+1);
            int dnesJe = (int)my_date.DayOfWeek;

            string[] rowData;

             if (docShifts[den])
             {
                 string dentmp = (den + 1).ToString() + "." + mesiac.ToString();

                 int res = Array.IndexOf(sviatky, dentmp);

                 if (dnesJe == 0)
                 {
                     if (res == -1)
                     {
                         rowData = vykazVypis["velkaSluzba"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);
                     }
                     else
                     {
                         rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, true, true, writeText);
                     }

                     den++;
                     rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                   //  TableRow riadok1 = new TableRow();
                     this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                     
                     //den++;

                 }
                 else if (dnesJe == 6)
                 {
                     if (res == -1)
                     {
                         rowData = vykazVypis["velkaSluzba"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);
                     }
                     else
                     {
                         rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, true, true, writeText);
                     }

                     if (den + 1 < dniMes)
                     {
                         den++;
                         rowData = vykazVypis["velkaSluzba2"].ToString().Split(',');
                         //TableRow riadok1 = new TableRow();
                         this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);
                     }
                     if (den + 2 < dniMes)
                     {
                         den++;
                         rowData = vykazVypis["exday"].ToString().Split(',');
                         //TableRow riadok2 = new TableRow();
                         this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                     }
                     
                 }
                 else
                 {
                     if (res == -1)
                     {
                         rowData = vykazVypis["malaSluzba"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);
                        
                     }
                     else
                     {
                         rowData = vykazVypis["sviatok"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, true, true, writeText);
                         
                     }
                     if (den + 1 < dniMes)
                     {
                         den++;
                         rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                     }
                 }
             }
             else
             {
                 string dentmp = (den+1).ToString() + "." + mesiac.ToString();

                 int res = Array.IndexOf(sviatky, dentmp);
                 if (dnesJe != 0 && dnesJe != 6 && res == -1)
                 {
                     rowData = vykazVypis["normDen"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                 }
                 else if (res != -1 && dnesJe != 0 && dnesJe != 6)
                 {
                     rowData = vykazVypis["sviatokNieVikend"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac, true, false, writeText);
                 }
                 else
                 {
                     rowData = vykazVypis["exday"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                 }
             }
        }
        this.fillInVacations(mesiac, rok, Session["user_id"].ToString());

        this.fillEpcData(mesiac, rok, Session["user_id"].ToString());
    }

    protected void generateVykazDK(int mesiac, int rok, Boolean writeText)
    {
        this.vykaz_tbl.Controls.Clear();

        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM [is_settings]  WHERE [name]='vykaz_doctors'");

        SortedList row = x2Mysql.getRow(sb.ToString());
        this.vykazHeader = row["data"].ToString().Split(',');

        int cols = this.vykazHeader.Length;
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
            headLabel.ID = this.vykazHeader[col] + "_lbl<br>";
            headLabel.Font.Size = FontUnit.Point(8);
            headLabel.Text = this.vykazHeader[col];

            headCell.Controls.Add(headLabel);

            TextBox tBox = new TextBox();
            tBox.ID = "head_tbox_" + col.ToString();
            tBox.Text = "";
            headCell.Controls.Add(tBox);


            headerRow.Controls.Add(headCell);
        }

        int dniMes = DateTime.DaysInMonth(rok, mesiac);
        string[] sviatky = Session["freedays"].ToString().Split(',');

        Dictionary<int, Hashtable> docShifts = this.getShiftsDK(rok, mesiac);

        this.setVykazTypesforDK();

        SortedList vykazVypis = (SortedList)Session["2dk_typy_hodin"];

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

            string[] rowdata = (vikend)?vykazVypis["ExDay"].ToString().Split(','):vykazVypis["normDen"].ToString().Split(',');

            this.makeRow(den, cols, rowdata, rok, mesiac, sviatok, false, true);

        }

        this.fillShiftsForDK(docShifts);
        this.fillInVacations(mesiac,rok,Session["user_id"].ToString());
               
    }

    protected void fillShiftsForDK(Dictionary<int, Hashtable> data)
    {
        int dataCn = data.Count;

        for (int row=0; row<dataCn; row++)
        {
            DateTime dt = Convert.ToDateTime(my_x2.UnixToMsDateTime(data[row]["datum"].ToString()));

            int dw = (int)dt.DayOfWeek;
            Boolean vikend = false;
            if (dw == 6 || dw == 0) vikend = true;

            string[] freeD = Session["freedays"].ToString().Split(',');
            int res = Array.IndexOf(freeD,dt.Day.ToString()+"."+dt.Month.ToString());
            Boolean sviatok = (res != -1) ? true : false;

            string week = data[row]["tyzden"].ToString();
            string typ = data[row]["typ"].ToString();

            if (typ != "KlAmb") this.calcRowDK(sviatok, vikend, week, typ, dt);

        }

    }

    protected void setVykazTypesforDK()
    {
        if (Session["2dk_typy_hodin"] == null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT [data],[name] FROM [is_settings] WHERE [name]='2dk_typ_vykaz'");

            SortedList res = x2Mysql.getRow(sb.ToString());

            string[] lines = res["data"].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            int linesCn = lines.Length;

            SortedList vykazTypes = new SortedList();

            for (int i = 0; i < linesCn; i++)
            {
                string[] tmp = lines[i].Split('|');
                vykazTypes.Add(tmp[0], tmp[1]);

            }
            Session.Add("2dk_typy_hodin", vykazTypes);
        }
    }

    protected void calcRowDK(Boolean sviatok, Boolean vikend, string week, string typ, DateTime dt)
    {
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder  ctpl = (ContentPlaceHolder)tmpControl;
        int day = dt.Day;
        int row = day - 1;

        SortedList vykazVypis = (SortedList)Session["2dk_typy_hodin"];

        DateTime dtOneAfter = dt.AddDays(1);

        string zacDt = my_x2.unixDate(dt);
        string koncDt = my_x2.unixDate(dtOneAfter);

        int cols = this.vykazHeader.Length;

        string endHour = "";
        Boolean epcYes = false;

        int daysAfter = 0;

        string[] rowData = new string[cols];

        string[] day1 = new string[cols];
        string[] day2 = new string[cols];
        
        if (typ == "Odd1")
        {
            rowData = vykazVypis["Odd1_vik"].ToString().Split(',');
            endHour = "07:00:00";
            epcYes = true;
            daysAfter = 2;
            day1 = vykazVypis["ExDay"].ToString().Split(',');
            day2 = vykazVypis["ExDay"].ToString().Split(',');
        }

        if (typ == "Odd2")
        {
            rowData = (week == "konz") ? vykazVypis["Odd2_8h"].ToString().Split(',') : vykazVypis["Odd1"].ToString().Split(',');
            if (week=="prijm") endHour="07:00:00";
            epcYes = (week == "konz") ? false : true; 
            if (week=="prijm")
            {
                daysAfter = 2;
                day1 = vykazVypis["ExDay"].ToString().Split(',');
                day2 = vykazVypis["ExDay"].ToString().Split(',');
            }
        }

        if (typ == "Odd")
        {
            rowData = vykazVypis["Odd_norm"].ToString().Split(',');
            endHour = "07:00:00";
            epcYes =true;
            daysAfter = 1;
            day1 = vykazVypis["ExDay"].ToString().Split(',');
        }

        if (typ == "OupA")
        {
            rowData = vykazVypis["OupA_normden"].ToString().Split(',');
            
        }
        if (typ == "OupB")
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
        if (typ == "OupA1" || typ=="OupA2")
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
        }

        if (epcYes)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT [hlasko].[dat_hlas] AS [datum],[hlasko].[type] AS [sluzba_typ],[hlasko_epc].[user_id], SUM([hlasko_epc].[work_time]) AS [worktime], SUM([hlasko_epc].[work_night]) AS [worknight]");
            sb.AppendLine("FROM [is_hlasko_epc] as [hlasko_epc]");
            sb.AppendLine("LEFT JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id]=[hlasko_epc].[hlasko_id]");
            sb.AppendFormat("WHERE [hlasko_epc].[work_start] BETWEEN '{0} 00:00:00' AND '{1} {2}'", zacDt, koncDt, endHour);
            sb.AppendFormat("AND [user_id]='{0}'", Session["user_id"]);
            sb.AppendLine("GROUP BY [hlasko_epc].[hlasko_id]");
            sb.AppendLine("ORDER BY [hlasko_epc].[work_start] ASC");

            Dictionary<int, Hashtable> epc = x2Mysql.getTable(sb.ToString());
            x2log.logData(epc, "", "epc_data");

            for (int dd = 0; dd < epc.Count; dd++)
            {

                Control nightbox = ctpl.FindControl("textBox_" + day.ToString() + "_5");
                TextBox nightTBOX = (TextBox)nightbox;
                decimal night_work = Convert.ToDecimal(epc[dd]["worknight"]);

                nightTBOX.Text = (Math.Round(night_work / 60, 1)).ToString();


                int aktivna = Convert.ToInt32(epc[dd]["worktime"]);
                decimal hodiny = aktivna / 60;
                decimal neaktivna = 12 - hodiny;

                if (neaktivna < 0)
                {
                    hodiny = 12;
                    neaktivna = 0;
                }

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

                    decimal zuctHodinyFLOAT = Convert.ToDecimal(zucHodTxt.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    decimal defZuc = zuctHodinyFLOAT + hodiny;
                    mzvyhTxt.Text = defZuc.ToString();
                    mzvyhTxtSv.Text = defZuc.ToString();

                    mTBox1.Text = hodiny.ToString();
                    mTBox2.Text = neaktivna.ToString();
                }
                else
                {
                    Control tbox1 = ctpl.FindControl("textBox_" + day.ToString() + "_8");
                    TextBox mTBox1 = (TextBox)tbox1;
                    Control tbox2 = ctpl.FindControl("textBox_" + day.ToString() + "_10");
                    TextBox mTBox2 = (TextBox)tbox2;

                    mTBox1.Text = hodiny.ToString();
                    mTBox2.Text = neaktivna.ToString();
                }
            }
        }

        if (daysAfter >0)
        {
            for (int eDay= 0; eDay<daysAfter; eDay++)
            {
                day = dt.AddDays(eDay+1).Day;
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
                    txtB.Text = rowData[tt].ToString();
                    //txtB.BackColor = System.Drawing.Color.LightGray;
                    //txtB.Font.Bold = true;
                }


            }
        }
        
    }



    protected SortedList getUserVykazData()
    {
        SortedList result = new SortedList();
        SortedList typSluziebVykaz = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name]='typ_vykaz'");
        SortedList userVykazData = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name]='" + Session["login"].ToString() + "_vykaz'");
       
        if (typSluziebVykaz.Count > 0 && userVykazData.Count > 0)
        {
            string[] typArr = typSluziebVykaz["data"].ToString().Split(',');
            string[] userData = userVykazData["data"].ToString().Split('|');
            int dataLn = typArr.Length;

            if (dataLn == userData.Length)
            {
                for (int i = 0; i < dataLn; i++)
                {
                    result.Add(typArr[i], userData[i]);
                }
            }
        }

        return result;
    }



    protected SortedList getValueFromSluzba()
    {
        SortedList result = this.getUserVykazData();

        if (result.Count == 0)
        {
            result.Clear();
            StringBuilder sb = new StringBuilder();
            if (Session["pracdoba"].ToString().Length == 0)
            {
                Session["pracdoba"] = 0;
            }

            string dStr = Session["pracdoba"].ToString().Replace(',','.');
            //string dStr = Session["pracdoba"].ToString();

            float pracDoba = Convert.ToSingle(dStr,CultureInfo.InvariantCulture.NumberFormat);

            int zacPrac = 7;
            float polHod = (float)0.5;

            float dlzkaPrace = zacPrac+pracDoba+polHod;

            string pracDobaTmp = pracDoba.ToString().Replace(',', '.');

            sb.AppendFormat("7,12:30,13:00,{0},{1},0,0,0,0,0,0,0,0", dlzkaPrace, pracDobaTmp);
            result["normDen"] = sb.ToString();
            sb.Length = 0;
            decimal sluzbaCas = 15 + 4;
            dlzkaPrace = pracDoba + 4;
            string dlzkaPraceStr = dlzkaPrace.ToString();
            dlzkaPraceStr = dlzkaPraceStr.Replace(',', '.');
            sb.AppendFormat("7,12:30,13:00,{0},{1},5,0,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
            result["malaSluzba"] = sb.ToString();
            result["malaSluzba2"] = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            sb.Length = 0;
            sb.AppendFormat("7,12:30,13:30,{0},{1},5,16.5,0,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
            result["velkaSluzba"] = sb.ToString();
            result["velkaSluzba2"] = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            result["velkaSluzba2a"] = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            sb.Length = 0;
            sb.AppendFormat("7,12:30,13:00,{0},{1},5,16.5,16.5,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
            result["sviatokVikend"] = sb.ToString();
            sb.Length = 0;
            sb.AppendFormat("7,12:30,13:00,{0},{1},5,0,16.5,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
            result["sviatok"] = sb.ToString();
            result["exday"] = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            sb.Length = 0;
            sb.AppendFormat("0,0,0,0,{0},0,0,0,0,0,0,0,0,0", pracDobaTmp);
            result["sviatokNieVikend"] = sb.ToString();
        }


        return result;
    }


    protected Boolean[] getShifts(int rok, int mesiac)
    {
        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }

        StringBuilder query = new StringBuilder();
        int dateGroup = my_x2.makeDateGroup(rok, mesiac);
        //query.AppendFormat("SELECT [datum] FROM [is_sluzby_2] WHERE [user_id] = '{0}' AND [date_group]='{1}' AND ([typ]<>'Prijm' OR [typ]<>'Uraz' OR [typ]<>'Vseob') ORDER BY [datum] ASC", Session["user_id"].ToString(), dateGroup);
        query.AppendFormat("SELECT [datum] FROM [is_sluzby_2] WHERE [user_id] = '{0}' AND [date_group]='{1}' AND [typ] NOT IN('Prijm','Uraz','Vseob','GFS','Uraz2') ORDER BY [datum] ASC", Session["user_id"].ToString(), dateGroup);
        Dictionary<int, Hashtable> table = x2Mysql.getTable(query.ToString());

        
        int tmpLen = table.Count;
        DateTime[] tmpData = new DateTime[tmpLen];
        for (int j = 0; j < tmpLen; j++)
        {
            tmpData[j] = my_x2.UnixToMsDateTime(table[j]["datum"].ToString());
        }


        int dni = DateTime.DaysInMonth(rok, mesiac);
        Boolean[] result = new Boolean[dni];

        StringBuilder dat = new StringBuilder();
        for (int den = 0; den < dni; den++)
        {
            int mDen = den + 1;
            string denStr = mDen.ToString();
            if (denStr.Length == 1)
            {
                denStr = "0" + denStr;
            }

            DateTime dt = new DateTime(rok, mesiac, mDen);
            if (Array.IndexOf(tmpData,dt) != -1)
            {
                result[den] = true;
            }
            else
            {
                result[den] = false;
            }
            dat.Length = 0;
        }
        return result;
    }

    protected Dictionary<int, Hashtable> getShiftsDK(int rok, int mesiac)
    {
        
        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }

        StringBuilder query = new StringBuilder();
        int dateGroup = my_x2.makeDateGroup(rok, mesiac);
        query.AppendFormat("SELECT [datum],[typ],[tyzden] FROM [is_sluzby_dk] WHERE [user_id] = '{0}' AND [date_group]='{1}' ORDER BY [datum] ASC", Session["user_id"].ToString(), dateGroup);
        Dictionary<int, Hashtable> result = x2Mysql.getTable(query.ToString());

        return result;
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
    protected void calcData_Click(object sender, EventArgs e)
    {
        this._calcData();
    }
   

    protected void _calcData()
    {
       //this.createVykaz(false);

        int cols = this.vykazHeader.Length;

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

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

        int pocetVolnychDni = my_x2.pocetVolnychDniBezSviatkov(od_date, do_date);

        int pocetPracdni = days - pocetVolnychDni;

        string ineDni = this.ine_p_dni_txt.Text.ToString();

        if (ineDni.Trim().Length > 0)
        {
            try {
                pocetPracdni = Convert.ToInt32(ineDni);
            }
            catch (Exception ex)
            {

            }
            
        }

        decimal pocetPracHod = 0;

        if (Session["pracdoba"].ToString().Length != 0)
        {
            Session["pracdoba"] = Session["pracdoba"].ToString().Replace(".", ",");
            pocetPracHod = pocetPracdni * Convert.ToDecimal(Session["pracdoba"]);
        }
        else
        {
            pocetPracHod = pocetPracdni * Convert.ToDecimal("7,5");
        }

        this.pocetHod_txt.Text = pocetPracHod.ToString();
        Control resTbox_roz = ctpl.FindControl("head_tbox_" + "4");
        TextBox resTxt_roz = (TextBox)resTbox_roz;

        decimal real = Convert.ToDecimal(resTxt_roz.Text.ToString());

        string prenosStr = this.predMes_txt.Text.ToString();
        prenosStr = prenosStr.Replace('.', ',');
        decimal prenos = Convert.ToDecimal(prenosStr);

        this.rozdiel_lbl.Text = (real-pocetPracHod).ToString();
       
        //this.
       this.saveData();

    }

    protected void calcRealNight(int mesiac, int rok, string id)
    {

        string dateGroup = my_x2.makeDateGroup(rok, mesiac).ToString();
        int dni = DateTime.DaysInMonth(rok, mesiac);

        string mesStr = dateGroup.Substring(4, 2);




        string zacDt = rok.ToString() + "-" + mesStr.ToString() + "-" + "01";
        string koncDt = rok.ToString() + "-" + mesStr.ToString() + "-" + dni.ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [hlasko].[dat_hlas] AS [datum],[hlasko_epc].[user_id] AS [user_id],[hlasko_epc].[user_id]");
        sb.AppendLine("FROM [is_hlasko_epc] as [hlasko_epc]");
        sb.AppendLine("LEFT JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id]=[hlasko_epc].[hlasko_id]");
        sb.AppendFormat("WHERE [hlasko_epc].[work_start] BETWEEN '{0}' AND '{1}'", zacDt, koncDt);
        sb.AppendFormat("AND [user_id]='{0}'", id);
        sb.AppendLine("GROUP BY [hlasko].[hlasko_id]");

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

    }

    protected void fillEpcData(int mesiac, int rok, string id)
    {
        

        ContentPlaceHolder ctpl = new ContentPlaceHolder();

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        int dni = DateTime.DaysInMonth(rok, mesiac);
        SortedList res = x2Mysql.calcNightWork(Convert.ToInt32(id), mesiac, rok, dni);

        if (Convert .ToBoolean(res["status"]))
        {

            string[] freeDays = x_db.getFreeDays();
            string dateGroup = my_x2.makeDateGroup(rok, mesiac).ToString();


            DateTime denPO = new DateTime(rok, mesiac, dni, 7, 0, 0);
            denPO = denPO.AddDays(1);

            string mesStr = dateGroup.Substring(4, 2);

            string zacDt = rok.ToString() + "-" + mesStr.ToString() + "-" + "01";
            //string koncDt = rok.ToString() + "-" + mesStr.ToString() + "-" + dni.ToString();
            string koncDt = my_x2.unixDate(denPO);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT [hlasko].[dat_hlas] AS [datum],[hlasko].[type] AS [sluzba_typ],[hlasko_epc].[user_id], SUM([hlasko_epc].[work_time]) AS [worktime], SUM([hlasko_epc].[work_night]) AS [worknight]");
            sb.AppendLine("FROM [is_hlasko_epc] as [hlasko_epc]");
            sb.AppendLine("LEFT JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id]=[hlasko_epc].[hlasko_id]");
            sb.AppendFormat("WHERE [hlasko_epc].[work_start] BETWEEN '{0} 00:00:00' AND '{1} 07:00:00'", zacDt, koncDt);
            sb.AppendFormat("AND [user_id]='{0}'", id);
            sb.AppendLine("GROUP BY [hlasko_epc].[hlasko_id]");
            sb.AppendLine("ORDER BY [hlasko_epc].[work_start] ASC");

            Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

            int tableLn = table.Count;
            // DateTime[] epcDate = new DateTime[tableLn];

            for (int i = 0; i < tableLn; i++)
            {
                DateTime dt = Convert.ToDateTime(my_x2.MSDate(table[i]["datum"].ToString()));

                if (dt.Month == mesiac)
                {

                    int den = dt.Day;
                    Control nightbox = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_5");

                    TextBox nightTBOX = (TextBox)nightbox;

                    decimal night_work = Convert.ToDecimal(table[i]["worknight"]); ;



                    nightTBOX.Text = (Math.Round(night_work / 60, 1)).ToString();



                    string mesDen = den.ToString() + "." + mesiac.ToString();

                    //int epc_tmp = Array.IndexOf(epcDate, dtTmp);

                    int rs_tmp = Array.IndexOf(freeDays, mesDen);
                    Boolean sviatok = false;

                    if (Array.IndexOf(freeDays, mesDen) != -1)
                    {
                        sviatok = true;
                    }

                    int vikInt = (int)dt.DayOfWeek;
                    Boolean vikend = false;
                    if (vikInt == 0 || vikInt == 6)
                    {
                        vikend = true;
                    }

                    int aktivna = Convert.ToInt32(table[i]["worktime"]);
                    decimal hodiny = aktivna / 60;
                    decimal neaktivna = 12 - hodiny;

                    if (neaktivna < 0)
                    {
                        hodiny = 12;
                        neaktivna = 0;
                    }

                    if (vikend || sviatok)
                    {
                        Control tbox1 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_9");
                        TextBox mTBox1 = (TextBox)tbox1;
                        Control tbox2 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_11");
                        TextBox mTBox2 = (TextBox)tbox2;

                        Control hodTmp = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_4");
                        TextBox zucHodTxt = (TextBox)hodTmp;

                        Control mzvyhTmp = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_7");
                        TextBox mzvyhTxt = (TextBox)mzvyhTmp;

                        Control mzvyhTmpSv = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_6");
                        TextBox mzvyhTxtSv = (TextBox)mzvyhTmpSv;

                        decimal zuctHodinyFLOAT = Convert.ToDecimal(zucHodTxt.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        decimal defZuc = zuctHodinyFLOAT + hodiny;
                        mzvyhTxt.Text = defZuc.ToString();
                        mzvyhTxtSv.Text = defZuc.ToString();

                        mTBox1.Text = hodiny.ToString();
                        mTBox2.Text = neaktivna.ToString();
                    }
                    else
                    {
                        Control tbox1 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_8");
                        TextBox mTBox1 = (TextBox)tbox1;
                        Control tbox2 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_10");
                        TextBox mTBox2 = (TextBox)tbox2;

                        mTBox1.Text = hodiny.ToString();
                        mTBox2.Text = neaktivna.ToString();
                    }
                }
                else
                {
                    int den = dni;
                    Control nightbox = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_5");

                    TextBox nightTBOX = (TextBox)nightbox;

                    decimal night_work = Convert.ToDecimal(table[i]["worknight"]); ;

                    decimal tmpNW = 0;

                    try{

                        tmpNW = Convert.ToDecimal(nightTBOX.Text.ToString());
                    }
                    catch (Exception ex)
                    {
                        tmpNW = 0;
                    }
                    nightTBOX.Text = (tmpNW + night_work).ToString();
                    string mesDen = den.ToString() + "." + mesiac.ToString();

                    //int epc_tmp = Array.IndexOf(epcDate, dtTmp);

                    int rs_tmp = Array.IndexOf(freeDays, mesDen);
                    Boolean sviatok = false;

                    if (Array.IndexOf(freeDays, mesDen) != -1)
                    {
                        sviatok = true;
                    }

                    int vikInt = (int)dt.DayOfWeek;
                    Boolean vikend = false;
                    if (vikInt == 0 || vikInt == 6)
                    {
                        vikend = true;
                    }

                    int aktivna = Convert.ToInt32(table[i]["worktime"]);
                    decimal hodiny = aktivna / 60;
                    decimal neaktivna = 12 - hodiny;

                    if (neaktivna < 0)
                    {
                        hodiny = 12;
                        neaktivna = 0;
                    }

                    if (vikend || sviatok)
                    {
                        Control tbox1 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_9");
                        TextBox mTBox1 = (TextBox)tbox1;
                        Control tbox2 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_11");
                        TextBox mTBox2 = (TextBox)tbox2;

                        Control hodTmp = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_4");
                        TextBox zucHodTxt = (TextBox)hodTmp;

                        Control mzvyhTmp = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_7");
                        TextBox mzvyhTxt = (TextBox)mzvyhTmp;

                        Control mzvyhTmpSv = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_6");
                        TextBox mzvyhTxtSv = (TextBox)mzvyhTmpSv;

                        decimal zuctHodinyFLOAT = Convert.ToDecimal(zucHodTxt.Text.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        decimal defZuc = zuctHodinyFLOAT + hodiny;
                        mzvyhTxt.Text = defZuc.ToString();
                        mzvyhTxtSv.Text = defZuc.ToString();

                        mTBox1.Text = hodiny.ToString();
                        mTBox2.Text = neaktivna.ToString();
                    }
                    else
                    {
                        Control tbox1 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_8");
                        TextBox mTBox1 = (TextBox)tbox1;
                        Control tbox2 = ctpl.FindControl("textBox_" + (den - 1).ToString() + "_10");
                        TextBox mTBox2 = (TextBox)tbox2;

                        mTBox1.Text = hodiny.ToString();
                        mTBox2.Text = neaktivna.ToString();
                    }
                }

            }
        }
        else
        {
            this.msg_lbl.Visible = true;
            this.msg_lbl.Text = res["msg"].ToString();
        }
    }

    protected Dictionary<int, Hashtable> getActivities(int mesiac, int rok, string id)
    {
        //Dictionary<int, Hashtable> result = 
        int days = DateTime.DaysInMonth(rok,mesiac);

         StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [is_dovolenky].[id] AS [dov_id],[is_dovolenky].[user_id],[is_dovolenky].[od], ");
        sb.AppendLine("[is_dovolenky].[do],[is_dovolenky].[type] FROM [is_dovolenky] ");
         sb.AppendFormat("WHERE [is_dovolenky].[od] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00'",rok,mesiac,days);
        sb.AppendFormat(" AND [is_dovolenky].[user_id]='{0}' ORDER BY [is_dovolenky].[do] ASC",id);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

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

            DateTime odDov = Convert.ToDateTime(my_x2.UnixToMsDateTime(dovolenky[i]["od"].ToString()));
            DateTime doDov = Convert.ToDateTime(my_x2.UnixToMsDateTime(dovolenky[i]["do"].ToString()));
            string[] freeDays = x_db.getFreeDays();

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

    protected void createPdf_btn_fnc(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        //this.createVykaz();
        
        //this.calcData_Click(sender, e);
        //this.createEmptyVykaz();
        this.createPdf(rok, mesiac);
    }

    /*protected void createEmptyVykaz()
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        this.vykaz_tbl.Controls.Clear();
        string mesStr = mesiac.ToString();
        if (mesStr.Length == 1)
        {
            mesStr = "0" + mesStr;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM [is_settings]  WHERE [name]='vykaz_doctors'");

        SortedList row = x2Mysql.getRow(sb.ToString());
        this.vykazHeader = row["data"].ToString().Split(',');

        int cols = this.vykazHeader.Length;
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

            Label headLabel = new Label();
            headLabel.ID = this.vykazHeader[col] + "_lbl<br>";
            headLabel.Font.Size = FontUnit.Point(8);
            headLabel.Text = this.vykazHeader[col];

            headCell.Controls.Add(headLabel);

            TextBox tBox = new TextBox();
            tBox.ID = "head_tbox_" + col.ToString();
            tBox.Text = "";
            headCell.Controls.Add(tBox);


            headerRow.Controls.Add(headCell);
        }

        int dniMes = DateTime.DaysInMonth(rok, mesiac);
        string[] sviatky = x_db.getFreeDays();

        Boolean[] docShifts = this.getShifts(rok, mesiac);

        SortedList vykazVypis = this.getValueFromSluzba();

        for (int den = 0; den < dniMes; den++)
        {
            // 
            DateTime my_date = new DateTime(rok, mesiac, den + 1);
            int dnesJe = (int)my_date.DayOfWeek;

            string[] rowData;

            if (docShifts[den])
            {
                string dentmp = (den + 1).ToString() + "." + mesiac.ToString();

                int res = Array.IndexOf(sviatky, dentmp);

                if (dnesJe == 0)
                {
                    if (res == -1)
                    {
                        rowData = vykazVypis["velkaSluzba"].ToString().Split(',');
                        this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);
                    }
                    else
                    {
                        rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                        this.makeRow(den, cols, rowData, rok, mesiac, true, true, writeText);
                    }

                    den++;
                    rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                    //  TableRow riadok1 = new TableRow();
                    this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);

                    //den++;

                }
                else if (dnesJe == 6)
                {
                    if (res == -1)
                    {
                        rowData = vykazVypis["velkaSluzba"].ToString().Split(',');
                        this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);
                    }
                    else
                    {
                        rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                        this.makeRow(den, cols, rowData, rok, mesiac, true, true, writeText);
                    }

                    if (den + 1 < dniMes)
                    {
                        den++;
                        rowData = vykazVypis["velkaSluzba2"].ToString().Split(',');
                        //TableRow riadok1 = new TableRow();
                        this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);
                    }
                    if (den + 2 < dniMes)
                    {
                        den++;
                        rowData = vykazVypis["exday"].ToString().Split(',');
                        //TableRow riadok2 = new TableRow();
                        this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                    }

                }
                else
                {
                    if (res == -1)
                    {
                        rowData = vykazVypis["malaSluzba"].ToString().Split(',');
                        this.makeRow(den, cols, rowData, rok, mesiac, false, true, writeText);

                    }
                    else
                    {
                        rowData = vykazVypis["sviatok"].ToString().Split(',');
                        this.makeRow(den, cols, rowData, rok, mesiac, true, true, writeText);

                    }
                    if (den + 1 < dniMes)
                    {
                        den++;
                        rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                        this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                    }
                }
            }
            else
            {
                string dentmp = (den + 1).ToString() + "." + mesiac.ToString();

                int res = Array.IndexOf(sviatky, dentmp);
                if (dnesJe != 0 && dnesJe != 6 && res == -1)
                {
                    rowData = vykazVypis["normDen"].ToString().Split(',');
                    this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                }
                else if (res != -1 && dnesJe != 0 && dnesJe != 6)
                {
                    rowData = vykazVypis["sviatokNieVikend"].ToString().Split(',');
                    this.makeRow(den, cols, rowData, rok, mesiac, true, false, writeText);
                }
                else
                {
                    rowData = vykazVypis["exday"].ToString().Split(',');
                    this.makeRow(den, cols, rowData, rok, mesiac, false, false, writeText);
                }
            }
        }



        //this.generateVykaz(mesiac, rok, false);
    }*/

    protected void onMonthChangedFnc(object sender, EventArgs e)
    {
        this.predMes_txt.Text = "";
        this.pocetHod_txt.Text = "";
       // this.hodiny_lbl.Text = "0";
        this.rozdiel_lbl.Text = "0";
       // string mesiac = this.mesiac_cb.SelectedValue.ToString();
       // string rok = this.rok_cb.SelectedValue.ToString();

        //this.vykaz_tbl.Controls.Clear();
        //Session.Remove("vykaz_id");

       // this.runGenerate(Convert.ToInt32(mesiac), Convert.ToInt32(rok));
        //this.createVykaz(true);
    }

    protected void onYearChangedFnc(object sender, EventArgs e)
    {
        this.predMes_txt.Text = "";
        this.pocetHod_txt.Text = "";
       // this.hodiny_lbl.Text = "0";
        this.rozdiel_lbl.Text = "0";
        this.createVykaz(true);
        //string mesiac = this.mesiac_cb.SelectedValue.ToString();
       // string rok = this.rok_cb.SelectedValue.ToString();
       // Session.Remove("vykaz_id");

       // this.vykaz_tbl.Controls.Clear();

      //  this.runGenerate(Convert.ToInt32(mesiac), Convert.ToInt32(rok));
    }


    protected void saveData()
    {

        int cols = this.vykazHeader.Length;

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
        data.Add("user_id", Session["user_id"].ToString());
        data.Add("mesiac",mesiac.ToString());
        data.Add("rok",rok.ToString());
        data.Add("vykaz", finalStr);
        data.Add("prenos", this.rozdiel_lbl.Text);

        SortedList res = x2Mysql.mysql_insert("is_vykaz", data);

        Boolean status = Convert.ToBoolean(res["status"].ToString());

        if (!status)
        {
            this.msg_lbl.Visible = true;
            this.msg_lbl.Text = res["msg"].ToString();
        }
        else
        {
            this.createPdf_btn.Enabled = true;
        }



    }
    protected void generateEpc_fnc(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);
        Session["epc_date_group"] = my_x2.makeDateGroup(rok, mesiac);
        Session["epc_mesiac"] = mesiac;
        Session["epc_rok"] = rok;

        Response.Redirect("is_epc.aspx",false);
    }

    protected void createPdf(int rok,int mesiac)
    {
        //int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        //int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int milis = DateTime.Now.Millisecond;
        string path = Server.MapPath("App_Data");
        string imagepath = Server.MapPath("App_Data");
        string oldFile = @path + "\\vykaz1.pdf";
        string hash = my_x2.makeFileHash(Session["login"].ToString() + milis.ToString());
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
		
		
		PdfImportedPage page = writer.GetImportedPage(reader, 1);
        cb.AddTemplate(page, 0, 0);

        double[] koor = new double[13];
        koor[0] = 69; //prichod
        koor[1] = 100; //obed zaciatok
        koor[2] = 130; //obed koniec
        koor[3] = 167; //odchod
        koor[4] = 199; //zuctovac hodiny
        koor[5] = 300; //nocna praca
        koor[6] = 329.55; //mzdove zvyhod
        koor[7] = 356.84; //sviatok
        koor[8] = 390.75; //aktivna1
        koor[9] = 420.42; //aktivna2
        koor[10] = 451.26; //neaktivna1
        koor[11] = 482.09; //neaktivna2
        koor[12] = 511.76; //neaktivna4
       // koor[13] = 507.37; //neaktivna3

        double odHora = 218;


        //string lila = "hura";
        BaseFont mojFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
        cb.SetFontAndSize(mojFont, 10);

        //cb.SetColorStroke(new CMYKColor(1f, 0f, 0f, 0f));
        //cb.SetColorFill(new CMYKColor(0f, 0f, 1f, 0f));

        cb.SetColorStroke(BaseColor.LIGHT_GRAY);
        cb.SetColorFill(BaseColor.LIGHT_GRAY);

        string[] freeDays = x_db.getFreeDays();



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

                cb.Rectangle(34, recYY, 25, 11);
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
        if (Session["klinika_label"].ToString().Length > 0)
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
        cb.ShowText(Session["titul_pred"].ToString() + Session["fullname"].ToString() + " " + Session["titul_za"].ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(388, size.Height - 97);
        cb.ShowText(Session["zaradenie"].ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(166, size.Height - 203);
        cb.ShowText(this.predMes_txt.Text.ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText((float)koor[4], size.Height - 628);
        cb.ShowText(this.rozdiel_lbl.Text.ToString());
        cb.EndText();

        cb.BeginText();
        string osobcisl = Session["osobcisl"].ToString();
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
        String tyzdoba = Session["tyzdoba"].ToString();
        if (tyzdoba.Length > 0)
        {
            tyzdoba = tyzdoba.Replace(',', '.');
            cb.ShowText(tyzdoba);
        }
        else
        {
            cb.ShowText("37.5");
        }

        cb.EndText();

        //int days = DateTime.DaysInMonth(rok, mesiac);
        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        int cols = this.vykazHeader.Length;

        for (int col = 4; col < cols; col++)
        {
            Control THbox = ctpl.FindControl("head_tbox_" + col.ToString());
            TextBox hBox = (TextBox)THbox;
            cb.BeginText();
            string num = hBox.Text.ToString();
            cb.MoveText((float)koor[col], size.Height - 604);
            cb.ShowText(num);

           
            cb.EndText();
        }
        kof = 12.3;
        for (int den = 0; den < days; den++)
        {
            for (int col = 0; col < cols; col++)
            {
                Control Tbox = ctpl.FindControl("textbox_" +den.ToString()+"_"+ col.ToString());
                TextBox mBox = (TextBox)Tbox;
                cb.BeginText();
                string num = mBox.Text.ToString();
                if (num == "0") num = "";
                double recY = (size.Height - (odHora + 0)) - (kof * den);

                float recYY = (float)recY;

                cb.MoveText((float)koor[col], recYY);
                cb.ShowText(num);


                cb.EndText();
            }
        }

     


        myDoc.Close();
        fs.Close();
        writer.Close();
        reader.Close();

        //Response.Redirect(@path + "\\vykaz_new.pdf");
        SortedList res = x_db.registerTempFile("vykaz_" + hash + ".pdf", 5);


        if (res["status"].ToString() == "ok")
        {

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=vykaz_" + hash + ".pdf");
            Response.TransmitFile(@path + "\\vykaz_" + hash + ".pdf");
            Response.End();
        }
        else
        {
            this.msg_lbl.Text = ",,,,,,=" + res["msg"].ToString();
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