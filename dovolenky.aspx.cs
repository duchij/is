using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class dovolenky : System.Web.UI.Page
{
    //my_db x_db = new my_db();
    mysql_db x2Mysql = new mysql_db();
    x2_var my_x2 = new x2_var();
    log x2log = new log();

    public string rights = "";
    public string wgroup = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        //this.warning_lbl.Visible = false;
        // Lab.Text = sender.GetType().TypeHandle.ToString();
       /* if (Request["del"] != null)
        {
            // this.deleteDovolenka();
        }*/

        this.rights = Session["rights"].ToString();
        this.wgroup = Session["workgroup"].ToString();

        //if (this.rights == "users")
        //{
        //   // this.vkladanie_dov.Visible = false;
        //    this.uziv_dovolenka.Visible = true;

        //}
        //else if (this.rights == "admin" || this.rights == "poweruser")
        //{
        //   // this.vkladanie_dov.Visible = false;
        //    this.uziv_dovolenka.Visible = true;
        //}

        if (!IsPostBack)
        {
            this.mesiac_cb.SelectedValue = DateTime.Today.Month.ToString();
            this.rok_cb.SelectedValue = DateTime.Today.Year.ToString();

            this.drawDovolenTab();
            this.drawUserActDovolenky();

           /*( if (this.rights != "users")
            {
                od_cal.SelectedDate = DateTime.Today;
                do_cal.SelectedDate = DateTime.Today;

                List<string> data = x_db.getAllUsers("is_users", "users");
                int cnt = data.Count;
                for (int i = 0; i < cnt; i++)
                {
                    string[] strTmp = data[i].Split('|');
                    if (strTmp[0] != "Administrator")
                    {
                        zamestnanci.Items.Add(new ListItem(strTmp[1].ToString(), strTmp[0].ToString()));
                        // zamen_zoz.Items.Add(new ListItem(tmp.Value.ToString(), tmp.Key.ToString()));
                    }
                }


                this.month_lbl.Text = mesiac_cb.SelectedItem.ToString();
                this.monthUser_lbl.Text = mesiac_cb.SelectedItem.ToString();

                dovolenky_tab.Controls.Clear();

                this.drawDovolenTab();
                //this.drawActDovolenky();
                //this.actStatusDovol();
            }
            else
            {
                this.drawDovolenTab();
                this.drawUserActDovolenky();
                //this.actStatusDovol();
            }*/
        }
        else
        {
            dovolenky_tab.Controls.Clear();

            if (this.rights != "users")
            {

                this.drawDovolenTab();
                //this.drawActDovolenky();

            }
            else
            {
                this.drawDovolenTab();
                this.drawUserActDovolenky();
            }
        }


    }

    //protected void deleteDovolenka(string dov_id)
    //{
    //    int _id = Convert.ToInt32(dov_id);

    //    SortedList dov_data = x_db.getDovolenkaByID(_id.ToString());
    //    DateTime do_date = Convert.ToDateTime(dov_data["do"].ToString());
    //    DateTime od_date = Convert.ToDateTime(dov_data["od"].ToString());

    //    //DateTime 


    //    int pocetVolnychDni = my_x2.pocetVolnychDni(od_date, do_date, x_db.getFreeDays());

    //    int pocetDni = (do_date - od_date).Days + 1;

    //    int pocetPracDni = pocetDni - pocetVolnychDni;

    //    SortedList data = x_db.getDovolStatus("is_dovolen_zost", dov_data["user_id"].ToString());
    //    string res = x_db.eraseRowByID("is_dovolenky", _id.ToString());

    //    if (res != "ok")
    //    {
    //        msg_lbl.Text = res;
    //    }

    //    this.dovolenky_tab.Controls.Clear();
    //   // this.zoznam_tbl.Controls.Clear();
    //    this.drawDovolenTab();


    //    if (this.rights.IndexOf("users") == -1)
    //    {
    //        //this.drawActDovolenky();
    //    }
    //    else
    //    {
    //        this.drawUserActDovolenky();
    //    }
    //}


    protected void saveDovStatus()
    {
    }

    protected void changeDovStatus_fnc(object sender, EventArgs e)
    {
        //this.actStatusDovol();
    }


    //protected void actStatusDovol()
    //{
    //    // msg_lbl.Text = "tu dom";

    //    SortedList dov_data = x_db.getDovolStatus("is_dovolen_zost", zamestnanci.SelectedValue.ToString());
    //    if (dov_data["error"] == null)
    //    {
    //        if (dov_data.Count != 0)
    //        {
    //            dovolenkaPravo_txt.Text = dov_data["narok"].ToString();
    //            dovolenkaZost_txt.Text = dov_data["zostatok"].ToString();

    //        }
    //        else
    //        {
    //            SortedList my_data = new SortedList();
    //            my_data.Add("user_id", this.zamestnanci.SelectedValue.ToString());
    //            my_data.Add("narok", "35");
    //            my_data.Add("zostatok", "35");

    //            x_db.insert_rows("is_dovolen_zost", my_data);

    //            dovolenkaPravo_txt.Text = "35";
    //            dovolenkaZost_txt.Text = "35";

    //        }

    //    }
    //    else if (dov_data["error"] != null)
    //    {
    //        msg_lbl.Text = dov_data["error"].ToString();
    //    }

    //}


    protected void drawDovolenTab()
    {
        //DateTime datum = DateTime.Today;
        int tc_month = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int tc_rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        //int pocet_dni = datum.day 
        int pocetDni = DateTime.DaysInMonth(tc_rok, tc_month);



        //string tmp_od = my_x2.unixDate(new DateTime(datum.Year, datum.Month, 1));

        //string tmp_do = my_x2.unixDate(new DateTime(datum.Year, datum.Month, pocetDni));
        //ArrayList data = x_db.getDovolenky(tc_month, tc_rok);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [is_users].[id],[is_users].[full_name],[is_dovolenky].[id] AS [dov_id],[is_dovolenky].[user_id],[is_dovolenky].[od], ");
        sb.AppendLine("[is_dovolenky].[do] FROM [is_users]");
        sb.AppendLine("INNER JOIN [is_dovolenky] ON [is_users].[id]=[is_dovolenky].[user_id]");
        sb.AppendFormat("WHERE (MONTH([is_dovolenky].[od]) = '{0}' OR MONTH([is_dovolenky].[do]) = '{0}')", tc_month); 
        sb.AppendFormat("AND (YEAR([is_dovolenky].[od]) = '{0}' OR YEAR([is_dovolenky].[do]) = '{0}')", tc_rok);



        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());
        int tableCnt = table.Count;
        StringBuilder names = new StringBuilder();
        string[] mes_dov = new string[pocetDni];
        for (int o = 0; o < pocetDni; o++)
        {
            //lila += o.ToString() + "..";
            mes_dov[o] = "<ul>";
            int posCnt = 0;
            for (int i = 0; i < tableCnt; i++)
            {

                //lila += data[i].ToString();
                //string muf = data[i].ToString();
                //tmp = muf.Split(';');

                DateTime tmp_od_mes = Convert.ToDateTime(table[i]["od"]);
                DateTime tmp_do_mes = Convert.ToDateTime(table[i]["do"]);

                DateTime tmp_den_mes = new DateTime(tc_rok, tc_month, o + 1);

                if (tmp_den_mes >= tmp_od_mes && tmp_den_mes <= tmp_do_mes)
                {
                    string lo_name = table[i]["full_name"].ToString();

                    if (mes_dov[o].ToString().IndexOf(lo_name) == -1)
                    {
                        if (posCnt % 2 == 0)
                        {
                            mes_dov[o] += "<li style='font-size:smaller;'><strong>" + table[i]["full_name"].ToString() + "</strong></li>";
                        }
                        else
                        {
                            mes_dov[o] += "<li style='font-size:smaller;'>" + table[i]["full_name"].ToString() + "</li>";
                        }
                        posCnt++;
                    }
                }
                
            }
            mes_dov[o] += "</ul>";

        }
        // int riadkov = 5;
        // int counter = 0;
        int my_den = 0;

        for (int i = 0; i < 5; i++)
        {

            TableRow mojRiadok = new TableRow();
            for (int x = 0; x < 7; x++)
            {

                TableCell mojaCela = new TableCell();
                mojaCela.ID = "mojaCelb_" + i.ToString() + "_" + x.ToString();
                mojaCela.VerticalAlign = VerticalAlign.Top;
                
               // mojaCela.CssClass = "duch";
                // mojaCela.Width = 100;
                my_den++;

                //DateTime tmp_den_mes = new DateTime(tc_rok, tc_month, o + 1);
                if (DateTime.Today.Day == my_den)
                {
                    mojaCela.CssClass = "box yellow";
                    //mojaCela.BackColor = System.Drawing.Color.Yellow;
                   // mojaCela.ForeColor = System.Drawing.Color.FromArgb(0x990000);
                }


                mojaCela.BorderWidth = 1;
                mojaCela.BorderColor = System.Drawing.Color.LightGray;


                if (my_den <= pocetDni)
                {
                    DateTime my_date = new DateTime(tc_rok, tc_month, my_den);
                    int dnesJe = (int)my_date.DayOfWeek;
                    string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];

                    if ((nazov == "sobota") || (nazov == "nedeľa"))
                    {
                        //mojaCela.BackColor = System.Drawing.Color.FromArgb(0x990000);
                       // mojaCela.ForeColor = System.Drawing.Color.Yellow;
                        mojaCela.CssClass = "box red";
                    }

                    if (DateTime.Today.Day == my_den)
                    {
                        //mojaCela.BackColor = System.Drawing.Color.Yellow;
                       // mojaCela.ForeColor = System.Drawing.Color.FromArgb(0x990000);
                        mojaCela.CssClass = "box yellow";
                    }

                    mojaCela.Text = "<strong>" + my_den.ToString() + ".</strong><font> " + nazov.Substring(0, 3) + "</font><br><br/>";
                    //mojaCela.Text += "<font style='font-size:10px;'>"+mes_dov[my_den-1].ToString()+"</font>";
                    mojaCela.Text += mes_dov[my_den - 1].ToString();
                }
                mojRiadok.Controls.Add(mojaCela);
            }
            dovolenky_tab.Controls.Add(mojRiadok);
        }
    }

    protected void saveZoz_fnc_Click(object sender, EventArgs e)
    {

    }

    //protected void zmenaNarok_fnc(object sender, EventArgs e)
    //{
    //    //msg_lbl.Text = "jijo";

    //    SortedList dov_data = x_db.getDovolStatus("is_dovolen_zost", zamestnanci.SelectedValue.ToString());

    //    int _novyNarok = Convert.ToInt32(this.dovolenkaPravo_txt.Text);

    //    int _povNarok = Convert.ToInt32(dov_data["narok"].ToString());

    //    int _zost = Convert.ToInt32(dovolenkaZost_txt.Text);

    //    int rozdiel = 0;
    //    int rozdielZostatok = 0;

    //    if (_novyNarok >= _povNarok)
    //    {
    //        rozdiel =_novyNarok - _povNarok ;
    //        rozdielZostatok = _zost + rozdiel;
    //    }
    //    else
    //    {
    //        rozdiel =_povNarok - _novyNarok;
    //        rozdielZostatok = _zost - rozdiel;
    //    }       

    //    dovolenkaZost_txt.Text = rozdielZostatok.ToString();

    //    SortedList data = new SortedList();
    //    data.Add("narok", _novyNarok.ToString());
    //    data.Add("zostatok", rozdielZostatok.ToString());

    //    msg_lbl.Text = x_db.update_row("is_dovolen_zost",data, "WHERE user_id ="+ this.zamestnanci.SelectedValue.ToString()).ToString();

    //   //msg_lbl.Text = rozdiel.ToString();

    //}

    protected void save_user_btn_Click(object sender, EventArgs e)
    {
        if (dovOd_user.SelectedDate > dovDo_user.SelectedDate)
        {
            this.msg_lbl.Text = "Do Datum nemoze byt mensi ako Od datum...";
        }
        else
        {
            SortedList data = new SortedList();
            data.Add("user_id", Session["user_id"].ToString());
            data.Add("od", my_x2.unixDate(dovOd_user.SelectedDate));
            // if (do_cal.s
            data.Add("do", my_x2.unixDate(dovDo_user.SelectedDate));
            data.Add("type" , this.freeType_dl.SelectedValue);
            data.Add("clinics", Session["klinika_id"]);
            //ata.Add("rok",

            SortedList res = x2Mysql.mysql_insert("is_dovolenky", data);

            if (!Convert.ToBoolean(res["status"]))
            {
                msg_lbl.Text = res["msg"].ToString();
            }
            else
            {
                dovolenky_tab.Controls.Clear();
                this.drawDovolenTab();
                this.drawUserActDovolenky();
            }
          }
    }

    protected string getFreeType(string idf)
    {
        string result = "";

        switch (idf) {
            case "pn":
                result = Resources.Resource.free_pn;
                break;
            case "do":
                result = Resources.Resource.free_do;
                break;
            case "ci":
                result = Resources.Resource.free_ci;
                break;
            case "ko":
                result = Resources.Resource.free_ko;
                break;
            case "sk":
                result = Resources.Resource.free_sk;
                break;
        }

        return result;
    }




    //protected void save_btn_Click(object sender, EventArgs e)
    //{

    //    if (od_cal.SelectedDate > do_cal.SelectedDate)
    //    {
    //        msg_lbl.Text = "Do Datum nemoze byt mensi ako Od datum...";
    //    }
    //    else
    //    {
    //        //int tc_month = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
    //        //int tc_rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

    //        if (x_db.checkDovExists(zamestnanci.SelectedValue.ToString(), my_x2.unixDate(od_cal.SelectedDate), my_x2.unixDate(do_cal.SelectedDate)) == false)
    //        {

    //            SortedList data = new SortedList();
    //            data.Add("user_id", zamestnanci.SelectedValue.ToString());
    //            data.Add("od", my_x2.unixDate(od_cal.SelectedDate));
    //            // if (do_cal.s
    //            data.Add("do", my_x2.unixDate(do_cal.SelectedDate));
    //            //ata.Add("rok",




    //            SortedList tmp = x_db.insert_rows("is_dovolenky", data);

    //            if (tmp["status"].ToString() == "error")
    //            {
    //                msg_lbl.Text = tmp["message"].ToString();
    //            }
    //            else
    //            {


    //                int pocetVolnychDni = my_x2.pocetVolnychDni(od_cal.SelectedDate, do_cal.SelectedDate, x_db.getFreeDays());

    //                int pocetDni = (do_cal.SelectedDate - od_cal.SelectedDate).Days + 1;

    //                //msg_lbl.Text = "volne:" + pocetVolnychDni.ToString() + "...." + pocetDni.ToString();

    //                int pocetPracDni = pocetDni - pocetVolnychDni;

    //                int zostatok = Convert.ToInt32(dovolenkaZost_txt.Text);

    //                int _zost = zostatok - pocetPracDni;

    //                dovolenkaZost_txt.Text = _zost.ToString();

    //                SortedList dov_data = new SortedList();
    //                dov_data.Add("zostatok", _zost.ToString());


    //                x_db.update_row("is_dovolen_zost", dov_data, "WHERE user_id =" + zamestnanci.SelectedValue.ToString());

    //                dovolenky_tab.Controls.Clear();
    //                this.drawDovolenTab();
    //                this.drawActDovolenky();


    //            }
    //        }
    //        else
    //        {
    //            warning_lbl.Visible = true;
    //            warning_lbl.Text = Resources.Resource.info_dov_status;
    //        }
    //    }






    //}

    protected void deleteDovolenka(object sender, EventArgs e)
    {
        Control data = (Control)sender;

        string name = data.ID.ToString();
        string[] tmp = name.Split(new char[] { '_' });

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("DELETE FROM [is_dovolenky] WHERE [id]='{0}'", tmp[1]);
        x2Mysql.execute(sb.ToString());

        //this.deleteDovolenka(tmp[1].ToString());
    }

    public void drawUserActDovolenky()
    {
        this.zoznamUser_tbl.Controls.Clear();
        // DateTime datum = DateTime.Now;


        int tc_month = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int tc_year = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int pocetDni = DateTime.DaysInMonth(tc_year, tc_month);
        string tmp_od = my_x2.unixDate(new DateTime(tc_year, tc_month, 1));

        // string tmp_do = my_x2.unixDate(new DateTime(datum.Year, datum.Month, pocetDni));

        StringBuilder query = new StringBuilder();
        query.AppendLine("SELECT [is_users].[id],[is_users].[full_name] AS [full_name],");
        query.AppendLine("[is_dovolenky].[id] AS [dov_id], [is_dovolenky].[user_id],[is_dovolenky].[od],[is_dovolenky].[do], [is_dovolenky].[type]");
        query.AppendLine("FROM [is_users] INNER JOIN [is_dovolenky] ON [is_users].[id] = [is_dovolenky].[user_id]");
        query.AppendFormat("WHERE (MONTH([is_dovolenky].[od]) = '{0}' OR MONTH([is_dovolenky].[do]) = '{0}')", tc_month);
        query.AppendFormat("AND (YEAR([is_dovolenky].[od]) = '{0}' OR YEAR([is_dovolenky].[do]) = '{0}') AND [is_users].[id] = {1} ", tc_year, Session["user_id"]);
        query.AppendLine(" ORDER BY [is_dovolenky].[od]");

        Dictionary<int, Hashtable> data = x2Mysql.getTable(query.ToString());
        

        int pocetDov = data.Count;
        this.zoznamUser_tbl.Width = Unit.Percentage(100);
        for (int i = 0; i < pocetDov; i++)
        {
           TableRow mojRiadok = new TableRow();
           mojRiadok.Width = Unit.Pixel(300);
           this.zoznamUser_tbl.Controls.Add(mojRiadok);
           TableCell info = new TableCell();
           // mojaCela.ID = "mojaCela_" +data[i]["id"].ToString();
            //mojaCela.Width = 150;
            info.Width = Unit.Pixel(330);
            info.Text = data[i]["full_name"].ToString() + "  <strong>OD:</strong> "+data[i]["od"].ToString()+ "   <strong>DO:</strong> "+data[i]["do"].ToString()+ "<br>  <strong>Typ: </strong>"+this.getFreeType(data[i]["type"].ToString()); 
            mojRiadok.Controls.Add(info);

            TableCell buttonCell = new TableCell();
            buttonCell.Width = Unit.Percentage(20);
            Button mojeTlac = new Button();
            mojeTlac.CssClass = "button red width-300";

            mojeTlac.Command += new CommandEventHandler(this.deleteDovolenka);
            mojeTlac.ID = "Button_" + data[i]["dov_id"].ToString();
            mojeTlac.Text = Resources.Resource.erase.ToString();

            buttonCell.Controls.Add(mojeTlac);
            mojRiadok.Controls.Add(buttonCell);
        }
            //zoznamUser_tbl.Controls.Add(mojRiadok);
        
    }



    //public void drawActDovolenky()
    //{
    //    zoznam_tbl.Controls.Clear();
    //    // DateTime datum = DateTime.Now;


    //    int tc_month = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
    //    int tc_year = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
    //    int pocetDni = DateTime.DaysInMonth(tc_year, tc_month);
    //    string tmp_od = my_x2.unixDate(new DateTime(tc_year, tc_month, 1));

    //    // string tmp_do = my_x2.unixDate(new DateTime(datum.Year, datum.Month, pocetDni));
    //    ArrayList data = x_db.getDovolenkyWithID(tc_month, tc_year);

    //    int pocetDov = data.Count;

    //    for (int i = 0; i < pocetDov; i++)
    //    {

    //        TableRow mojRiadok = new TableRow();
    //        string[] tmp = new string[4];

    //        tmp = data[i].ToString().Split(new char[] { ';' });

    //        for (int j = 0; j < 5; j++)
    //        {
    //            TableCell mojaCela = new TableCell();
    //            mojaCela.ID = "mojaCela_" + i.ToString() + "_" + j.ToString();
    //            // mojaCela.Width = 150;
    //            if (j < 3)
    //            {
    //                mojaCela.Text = tmp[j].ToString();
    //            }
    //            if (j == 4)
    //            {
    //                Button mojeTlac = new Button();
    //                mojeTlac.Command += new CommandEventHandler(this.deleteDovolenka);
    //                mojeTlac.ID = "Button_" + tmp[3].ToString();
    //                mojeTlac.Text = "Zmaz";


    //                //mojaCela.Text = "<a href='dovolenky.aspx?del="+tmp[3].ToString()+"' style='color:red;'>Zmaž"+"</a>";
    //                mojaCela.Controls.Add(mojeTlac);
    //            }
    //            mojRiadok.Controls.Add(mojaCela);
    //        }
    //        zoznam_tbl.Controls.Add(mojRiadok);
    //    }
    //}

    //protected void checkDovStatusFnc(object sender, EventArgs e)
    //{
    //    string user_id = this.zamestnanci.SelectedValue.ToString();
    //    StringBuilder query = new StringBuilder();
    //    query.AppendFormat("SELECT * FROM `is_dovolenky` WHERE `user_id`='{0}'", user_id);
    //    Dictionary<int, SortedList> data = x_db.getTable(query.ToString());

    //    int data_len = data.Count;
    //    int dovCount = 0;
    //    int defDni = 0;
    //    for (int i = 0; i < data_len; i++)
    //    {
    //        foreach (DictionaryEntry row_data in data[i])
    //        {
    //            // msg_lbl.Text += row_data.Key.ToString() + "..." + row_data.Value.ToString() + "<br/>";

    //            DateTime zaciatok = Convert.ToDateTime(data[i]["od"].ToString());
    //            DateTime koniec = Convert.ToDateTime(data[i]["do"].ToString());

    //            // int celk_dov = Convert.ToInt32(this.dovolenkaPravo_txt.ToString());

    //            int pocetVolnychDni = my_x2.pocetVolnychDni(zaciatok, koniec, x_db.getFreeDays());

    //            int pocetDni = (koniec - zaciatok).Days + 1;

    //            defDni = pocetDni - pocetVolnychDni;




    //        }
    //        dovCount += defDni;
    //    }
    //    int _pravo = 0;
    //    int _zost = 0;

    //    try
    //    {

    //        _pravo = Convert.ToInt32(dovolenkaPravo_txt.Text.ToString());
    //        _zost = Convert.ToInt32(dovolenkaZost_txt.Text.ToString());
    //        int __tmp = _pravo - dovCount;

    //        dovolenkaZost_txt.Text = __tmp.ToString();


    //        SortedList dov_data = new SortedList();
    //        dov_data.Add("zostatok", __tmp.ToString());
    //        dov_data.Add("narok", dovolenkaPravo_txt.Text.ToString());


    //        x_db.update_row("is_dovolen_zost", dov_data, "WHERE user_id =" + zamestnanci.SelectedValue.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        this.warning_lbl.Text = ex.Message.ToString();
    //    }
    //}


}
