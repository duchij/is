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


        this.generateVykaz(Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString()),Convert.ToInt32(this.rok_cb.SelectedValue.ToString()));


    }

    protected void makeRow(int den, int cols, string[] rowData, int rok, int mesiac, Boolean sviatok)
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
        if (dnesJe != 6 && dnesJe != 0 && sviatok)
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
            tBox.Text = rowData[col];
            dataCell.Controls.Add(tBox);
            riadok.Controls.Add(dataCell);
        }
    }

    protected void generateVykaz(int mesiac, int rok)
    {
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
                         this.makeRow(den, cols, rowData, rok, mesiac, false);
                     }
                     else
                     {
                         rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                         this.makeRow(den, cols,rowData, rok, mesiac, true);
                     }

                     den++;
                     rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                   //  TableRow riadok1 = new TableRow();
                     this.makeRow(den, cols, rowData, rok, mesiac,false);
                     
                     //den++;

                 }
                 else if (dnesJe == 6)
                 {
                     if (res == -1)
                     {
                         rowData = vykazVypis["velkaSluzba"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac,false);
                     }
                     else
                     {
                         rowData = vykazVypis["sviatokVikend"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac,true);
                     }

                    den++;
                    rowData = vykazVypis["velkaSluzba2"].ToString().Split(',');
                    //TableRow riadok1 = new TableRow();
                    this.makeRow(den, cols, rowData, rok, mesiac, false);

                    den++;
                    rowData = vykazVypis["exday"].ToString().Split(',');
                    //TableRow riadok2 = new TableRow();
                    this.makeRow(den, cols, rowData, rok, mesiac,false);
                     
                 }
                 else
                 {
                     if (res == -1)
                     {
                         rowData = vykazVypis["malaSluzba"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, false);
                        
                     }
                     else
                     {
                         rowData = vykazVypis["sviatok"].ToString().Split(',');
                         this.makeRow(den, cols, rowData, rok, mesiac, true);
                         
                     }

                     den++;
                     rowData = vykazVypis["malaSluzba2"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac, false);
                 }
             }
             else
             {
                 string dentmp = (den+1).ToString() + "." + mesiac;

                 int res = Array.IndexOf(sviatky, dentmp);
                 if (dnesJe != 0 && dnesJe != 6 && res == -1)
                 {
                     rowData = vykazVypis["normDen"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac, false);
                 }
                 else if (res != -1 && dnesJe != 0 && dnesJe != 6)
                 {
                     rowData = vykazVypis["sviatokNieVikend"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac, true);
                 }
                 else
                 {
                     rowData = vykazVypis["exday"].ToString().Split(',');
                     this.makeRow(den, cols, rowData, rok, mesiac,false);
                 }
             }
        }
    }



    protected SortedList getValueFromSluzba()
    {
        SortedList result = new SortedList();

        StringBuilder sb = new StringBuilder();

        double pracDoba = Convert.ToDouble(Session["pracdoba"]);
        double dlzkaPrace = 7 + pracDoba + 0.5;

        string pracDobaTmp = pracDoba.ToString().Replace(',', '.');

        sb.AppendFormat("7,{0},{1},0,0,0,0,0,0,0,0", dlzkaPrace, pracDobaTmp);
        result["normDen"] = sb.ToString();

        sb.Length = 0;

        double sluzbaCas = 15 + 4;
        dlzkaPrace = pracDoba + 4;

        string dlzkaPraceStr = dlzkaPrace.ToString();

        dlzkaPraceStr = dlzkaPraceStr.Replace(',', '.');

        sb.AppendFormat("7,{0},{1},5,0,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["malaSluzba"] = sb.ToString();

        result["malaSluzba2"] = "0,0,0,0,0,0,0,0,0,0,0";

        sb.Length = 0;
        sb.AppendFormat("7,{0},{1},5,16.5,0,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["velkaSluzba"] = sb.ToString();
        result["velkaSluzba2"] = "0,0,0,0,0,0,0,0,0,0,0,0";
        result["velkaSluzba2a"] = "0,0,0,0,0,0,0,0,0,0,0,0";

        sb.Length = 0;
        sb.AppendFormat("7,{0},{1},5,16.5,16.5,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["sviatokVikend"] = sb.ToString();
        sb.Length = 0;

        sb.AppendFormat("7,{0},{1},5,0,16.5,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["sviatok"] = sb.ToString();
        result["exday"] = "0,0,0,0,0,0,0,0,0,0,0,0";
        sb.Length = 0;
        sb.AppendFormat("0,0,{0},0,0,0,0,0,0,0,0,0", pracDobaTmp);
        result["sviatokNieVikend"] = sb.ToString();


        return result;
    }


    protected Boolean[] getShifts(int rok, int mesiac)
    {
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
            dat.AppendFormat("{0}-{1}-{2}", rok, mesiac, den + 1);
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
        for (int col = 2; col < cols; col++)
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



        this.saveData();

    }

    protected void createPdf_btn_fnc(object sender, EventArgs e)
    {

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
        this.msg_lbl.Visible = true;
        this.msg_lbl.Text = finalStr;

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



    }


}