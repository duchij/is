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

    public mysql_db x2Mysql = new mysql_db();
    public my_db x_db = new my_db();
    public string[] vykazHeader;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        if (Session["pracdoba"].ToString().Trim().Length == 0 || Session["tyzdoba"].ToString().Trim().Length == 0 || Session["osobcisl"].ToString().Trim().Length == 0)
        {

            // Page page = HttpContext.Current.CurrentHandler as Page;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('" + Resources.Resource.vykaz_error + "');", true);

            //Response.Redirect("adduser.aspx");
        }

       


        //this.zaMesiac_lbl.Text = "Maj,2012";

        this.msg_lbl.Visible = false;

        if (!IsPostBack)
        {
            DateTime dnesJe = DateTime.Today;
            this.mesiac_cb.SelectedValue = dnesJe.Month.ToString();
            this.rok_cb.SelectedValue = dnesJe.Year.ToString();
        }

        this.createVykaz();

        //this.generateVykaz(Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString()),Convert.ToInt32(this.rok_cb.SelectedValue.ToString()));


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

        if (row.Count > 0)
        {
            this.predMes_txt.Text = row["prenos"].ToString();
        }
        else
        {
            this.predMes_txt.Text = "0";
        }
    }

    protected void createVykaz()
    {
        
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        if (IsPostBack == false)
        {
            this.getPrenos(mesiac, rok);
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_vykaz] WHERE [mesiac]='{0}' AND [rok]='{1}' AND [user_id]='{2}'",mesiac,rok,Session["user_id"].ToString());
        SortedList row = x2Mysql.getRow(sb.ToString());
        if (row.Count > 0)
        {
            this.reCreateVykaz(row,mesiac,rok);
        }
        else
        {
            this.generateVykaz(mesiac, rok);
        }
    }

    protected void reCreateVykaz(SortedList data, int mesiac, int rok)
    {
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

        string vykazStr = data["vykaz"].ToString();
        string[] vykazRiadok = vykazStr.Split('|');
        Boolean[] docShifts = this.getShifts(rok, mesiac);

        

        for (int den = 0; den < dniMes; den++)
        {
            DateTime my_date = new DateTime(rok, mesiac, den + 1);
            int dnesJe = (int)my_date.DayOfWeek;
            string[] rowData = vykazRiadok[den].Split('~');

            string dentmp = (den + 1).ToString() + "." + mesiac.ToString();

            int res = Array.IndexOf(sviatky, dentmp);

            if (docShifts[den])
            {
               
                if (res == -1)
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, false, true);
                }
                else
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, true, true);
                }

            }
            else
            {
                if (res == -1)
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, false, false);
                }
                else
                {
                    this.makeRow(den, cols, rowData, rok, mesiac, true, false);
                }
            }

        }

    }

    protected void makeRow(int den, int cols, string[] rowData, int rok, int mesiac, Boolean sviatok, Boolean shift)
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
            tBox.Text = rowData[col];
            dataCell.Controls.Add(tBox);
            riadok.Controls.Add(dataCell);
        }
    }

    protected void generateVykaz(int mesiac, int rok)
    {
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
                         this.makeRow(den, cols, rowData, rok, mesiac, false,true);
                     }
                     else
                     {
                         rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                         this.makeRow(den, cols,rowData, rok, mesiac, true,true);
                     }

                     den++;
                     rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                   //  TableRow riadok1 = new TableRow();
                     this.makeRow(den, cols, rowData, rok, mesiac,false,false);
                     
                     //den++;

                 }
                 else if (dnesJe == 6)
                 {
                     if (res == -1)
                     {
                         rowData = vykazVypis["velkaSluzba"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac,false,true);
                     }
                     else
                     {
                         rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac,true,true);
                     }

                     if (den + 1 < dniMes)
                     {
                         den++;
                         rowData = vykazVypis["velkaSluzba2"].ToString().Split(',');
                         //TableRow riadok1 = new TableRow();
                         this.makeRow(den, cols, rowData, rok, mesiac, false, true);
                     }
                     if (den + 2 < dniMes)
                     {
                         den++;
                         rowData = vykazVypis["exday"].ToString().Split(',');
                         //TableRow riadok2 = new TableRow();
                         this.makeRow(den, cols, rowData, rok, mesiac, false, false);
                     }
                     
                 }
                 else
                 {
                     if (res == -1)
                     {
                         rowData = vykazVypis["malaSluzba"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, false,true);
                        
                     }
                     else
                     {
                         rowData = vykazVypis["sviatok"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, true,true);
                         
                     }
                     if (den + 1 < dniMes)
                     {
                         den++;
                         rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, false, false);
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
                     this.makeRow(den, cols, rowData, rok, mesiac, false,false);
                 }
                 else if (res != -1 && dnesJe != 0 && dnesJe != 6)
                 {
                     rowData = vykazVypis["sviatokNieVikend"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac, true,false);
                 }
                 else
                 {
                     rowData = vykazVypis["exday"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac,false,false);
                 }
             }
        }
        this.fillInVacations(mesiac, rok, Session["user_id"].ToString());
        this.fillEpcData(mesiac, rok, Session["user_id"].ToString());
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
            StringBuilder sb = new StringBuilder();
            if (Session["pracdoba"].ToString().Length == 0)
            {
                Session["pracdoba"] = 0;
            }
            double pracDoba = Convert.ToDouble(Session["pracdoba"]);
            double dlzkaPrace = 7 + pracDoba + 0.5;
            string pracDobaTmp = pracDoba.ToString().Replace(',', '.');
            sb.AppendFormat("7,12:30,13:00,{0},{1},0,0,0,0,0,0,0,0", dlzkaPrace, pracDobaTmp);
            result["normDen"] = sb.ToString();
            sb.Length = 0;
            double sluzbaCas = 15 + 4;
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
        query.AppendFormat("SELECT [datum] FROM [is_sluzby_2] WHERE [user_id] = {0} AND [date_group]={1} AND [typ]<>'Prijm' ORDER BY [datum] ASC", Session["user_id"].ToString(), dateGroup);
        Dictionary<int, Hashtable> table = x2Mysql.getTable(query.ToString());

        ArrayList tmpData = new ArrayList();
        int tmpLen = table.Count;
        for (int j = 0; j < tmpLen; j++)
        {
            tmpData.Add(table[j]["datum"]);
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

            dat.AppendFormat("{0}-{1}-{2}", rok, mesStr, denStr);
            if (tmpData.IndexOf(dat.ToString()) != -1)
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


    protected void calcData_Click(object sender, EventArgs e)
    {
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
                suma += Convert.ToDecimal(sum);
            }
            Control resTbox = ctpl.FindControl("head_tbox_" + col.ToString());
            TextBox resTxt = (TextBox)resTbox;

            resTxt.Text = suma.ToString();

        }
        DateTime od_date = new DateTime(rok, mesiac, 1);
        DateTime do_date = new DateTime(rok, mesiac, days);

        int pocetVolnychDni = my_x2.pocetVolnychDniBezSviatkov(od_date, do_date);

        int pocetPracdni = days - pocetVolnychDni;

        decimal pocetPracHod = 0;

        if (Session["pracdoba"] != null)
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

        this.rozdiel_lbl.Text = (pocetPracHod - real+prenos).ToString();

        

        this.saveData();

    }

    protected void fillEpcData(int mesiac, int rok, string id)
    {
        ContentPlaceHolder ctpl = new ContentPlaceHolder();

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        string[] freeDays = x_db.getFreeDays();
        string dateGroup = my_x2.makeDateGroup(rok, mesiac).ToString();
        int dni = DateTime.DaysInMonth(rok, mesiac);

        string mesStr = dateGroup.Substring(4, 2);

        string zacDt = rok.ToString() + "-" + mesStr.ToString() + "-" + "01";
        string koncDt = rok.ToString() + "-" + mesStr.ToString() + "-" + dni.ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [hlasko].[dat_hlas] AS [datum],[hlasko].[type] AS [sluzba_typ],[hlasko_epc].[user_id], SUM([work_time]) AS [worktime]");
        sb.AppendLine("FROM [is_hlasko_epc] as [hlasko_epc]");
        sb.AppendLine("LEFT JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id]=[hlasko_epc].[hlasko_id]");
        sb.AppendFormat("WHERE [hlasko_epc].[work_start] BETWEEN '{0}' AND '{1}'", zacDt, koncDt);
        sb.AppendFormat("AND [user_id]='{0}'", id);
        sb.AppendLine("GROUP BY [hlasko_epc].[hlasko_id]");

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableLn = table.Count;
        DateTime[] epcDate = new DateTime[tableLn];

        for (int i = 0; i < tableLn; i++)
        {
            epcDate[i] = Convert.ToDateTime(my_x2.MSDate(table[i]["datum"].ToString()));
        }


        for (int row = 0; row < dni; row++)
        {
            int den = row + 1;
            int vikend = (int)new DateTime(rok, mesiac, den).DayOfWeek;
            DateTime dtTmp = new DateTime(rok, mesiac, den);
            string mesDen = den.ToString() + "." + mesiac.ToString();

            int epc_tmp = Array.IndexOf(epcDate, dtTmp);

            int rs_tmp = Array.IndexOf(freeDays, mesDen);

            if (epc_tmp != -1)
            {
                int aktivna = Convert.ToInt32(table[epc_tmp]["worktime"]);
                decimal hodiny = aktivna / 60;
                decimal neaktivna = 12 - hodiny;

                if (neaktivna < 0)
                {
                    hodiny = 12;
                    neaktivna = 0;
                }

                if ((vikend == 0 || vikend == 6) && rs_tmp == -1)
                {

                    Control tbox1 = ctpl.FindControl("textBox_" + row.ToString() + "_9");
                    TextBox mTBox1 = (TextBox)tbox1;
                    Control tbox2 = ctpl.FindControl("textBox_" + row.ToString() + "_11");
                    TextBox mTBox2 = (TextBox)tbox2;

                    mTBox1.Text = hodiny.ToString();
                    mTBox2.Text = neaktivna.ToString();
                }

                if ((vikend == 0 || vikend == 6) && rs_tmp != -1)
                {

                    Control tbox1 = ctpl.FindControl("textBox_" + row.ToString() + "_9");
                    TextBox mTBox1 = (TextBox)tbox1;
                    Control tbox2 = ctpl.FindControl("textBox_" + row.ToString() + "_11");
                    TextBox mTBox2 = (TextBox)tbox2;

                    mTBox1.Text = hodiny.ToString();
                    mTBox2.Text = neaktivna.ToString();
                }

                if (vikend != 0 && vikend != 6 && rs_tmp != -1)
                {
                    Control tbox1 = ctpl.FindControl("textBox_" + row.ToString() + "_9");
                    TextBox mTBox1 = (TextBox)tbox1;
                    Control tbox2 = ctpl.FindControl("textBox_" + row.ToString() + "_11");
                    TextBox mTBox2 = (TextBox)tbox2;

                    mTBox1.Text = hodiny.ToString();
                    mTBox2.Text = neaktivna.ToString();
                }

                if (vikend != 0 && vikend != 6 && rs_tmp == -1)
                {
                    Control tbox1 = ctpl.FindControl("textBox_" + row.ToString() + "_7");
                    TextBox mTBox1 = (TextBox)tbox1;
                    Control tbox2 = ctpl.FindControl("textBox_" + row.ToString() + "_9");
                    TextBox mTBox2 = (TextBox)tbox2;

                    mTBox1.Text = hodiny.ToString();
                    mTBox2.Text = neaktivna.ToString();
                }


            }
        }


    }


    protected void fillInVacations(int mesiac, int rok, string id)
    {
        ArrayList dovolenky = x_db.getDovolenkyByID(mesiac, rok, Convert.ToInt32(id));
        int dovCnt = dovolenky.Count;

        ContentPlaceHolder ctpl = new ContentPlaceHolder();

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        for (int i = 0; i < dovCnt; i++)
        {
            string[] data = dovolenky[i].ToString().Split(';');

            //string dd1 = my_x2.MSDate(data[1].ToString());
            //string dd2 = my_x2.MSDate(data[2].ToString());

            DateTime odDov = Convert.ToDateTime(data[1].ToString());
            DateTime doDov = Convert.ToDateTime(data[2].ToString());
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

                        my_text_box.Text = "D";
                        my_text_box1.Text = "D";
                        my_text_box2.Text = "0";
                        my_text_box3.Text = "0";
                    }
                }
            }
        }
    }

    protected void createPdf_btn_fnc(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        this.createPdf(rok, mesiac);
    }

    protected void onMonthChangedFnc(object sender, EventArgs e)
    {
        this.predMes_txt.Text = "";
        this.pocetHod_txt.Text = "";
       // this.hodiny_lbl.Text = "0";
        this.rozdiel_lbl.Text = "0";
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        this.vykaz_tbl.Controls.Clear();
        //Session.Remove("vykaz_id");

       // this.runGenerate(Convert.ToInt32(mesiac), Convert.ToInt32(rok));
    }

    protected void onYearChangedFnc(object sender, EventArgs e)
    {
        this.predMes_txt.Text = "";
        this.pocetHod_txt.Text = "";
       // this.hodiny_lbl.Text = "0";
        this.rozdiel_lbl.Text = "0";
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();
        Session.Remove("vykaz_id");

        this.vykaz_tbl.Controls.Clear();

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

        Response.Redirect("is_epc.aspx");
    }

    protected void createPdf(int rok,int mesiac)
    {
        //int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        //int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int milis = DateTime.Now.Millisecond;
        string path = Server.MapPath("App_Data");
        string imagepath = Server.MapPath("App_Data");
        string oldFile = @path + "\\vykaz.pdf";
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
        BaseFont mojFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
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
        cb.ShowText("KDCH");
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
            Control Tbox = ctpl.FindControl("head_tbox_" + col.ToString());
            TextBox mBox = (TextBox)Tbox;
            cb.BeginText();
            string num = mBox.Text.ToString();
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

        PdfImportedPage page = writer.GetImportedPage(reader, 1);
        cb.AddTemplate(page, 0, 0);


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
            this.msg_lbl.Text = ",,,,,,=" + res["message"].ToString();
        }


        //



        //my_x2.createVykazPdf(path, imagepath);

    }

    protected float cmPt(double number)
    {
        string unit = "28,3464";
        double res = number * Convert.ToDouble(unit);

        float result = (float)res;

        return result;
    }

}