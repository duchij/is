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

        this.rights = Session["rights"].ToString();
        this.wgroup = Session["workgroup"].ToString();


        if (!IsPostBack)
        {
            this.mesiac_cb.SelectedValue = DateTime.Today.Month.ToString();
            this.rok_cb.SelectedValue = DateTime.Today.Year.ToString();

            this.dovOd_user.SelectedDate = DateTime.Today;
            this.dovDo_user.SelectedDate = DateTime.Today.AddDays(1);

            this.drawDovolenTab();
            this.drawUserActDovolenky();

        }
        else
        {
            this.dovolenky_tab.Controls.Clear();
            this.drawDovolenTab();
            this.drawUserActDovolenky();
        
        }

    }


    protected void saveDovStatus()
    {
    }

    protected void changeDovStatus_fnc(object sender, EventArgs e)
    {
        //this.actStatusDovol();
    }


    

    protected void loadKomplViewFnc(object sender, EventArgs e)
    {
        Session.Add("dov_mesiac", this.mesiac_cb.SelectedValue);
        Session.Add("dov_rok", this.rok_cb.SelectedValue);
        Response.Redirect("dovkompl.aspx");
    }

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
        string typ = this.activies_dl.SelectedValue.ToString();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [is_users].[id],[is_users].[full_name],[is_dovolenky].[id] AS [dov_id],[is_dovolenky].[user_id],[is_dovolenky].[od], ");
        sb.AppendLine("[is_dovolenky].[do] FROM [is_users] ");
        sb.AppendLine("INNER JOIN [is_dovolenky] ON [is_users].[id]=[is_dovolenky].[user_id] ");
        sb.AppendFormat("WHERE [is_dovolenky].[od] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00'",tc_rok,tc_month,pocetDni);
        //sb.AppendFormat("WHERE (MONTH([is_dovolenky].[od]) = '{0}' OR MONTH([is_dovolenky].[do]) = '{0}') ", tc_month); 
        //sb.AppendFormat("AND (YEAR([is_dovolenky].[od]) = '{0}' OR YEAR([is_dovolenky].[do]) = '{0}')", tc_rok);
        sb.AppendFormat(" AND [is_dovolenky].[type]='{0}' AND [is_dovolenky].[clinics]='{1}' ORDER BY [is_dovolenky].[do] ASC",typ,Session["klinika_id"]);

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

    protected Boolean checkConflict(DateTime date)
    {
        Boolean st = false;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT COUNT(*) AS [pocet] FROM [is_dovolenky] WHERE '{0} 00:00:00' ", my_x2.unixDate(date));
        sb.AppendLine("BETWEEN [od] AND [do] ");
        sb.AppendFormat("AND [clinics]='{0}' AND [user_id]='{1}'", Session["klinika_id"], Session["user_id"]);

        SortedList result = x2Mysql.getRow(sb.ToString());

        
        if (Convert.ToInt32(result["pocet"]) > 0)
        {
            st = true;
        }
       
        else
        {
            st = false;
        }

        return st;
        // sb.AppendLine("do

    }

    protected void save_user_btn_Click(object sender, EventArgs e)
    {
        if (dovOd_user.SelectedDate > dovDo_user.SelectedDate)
        {
            this.msg_lbl.Text = "Do Datum nemoze byt mensi ako Od datum...";
        }
        else
        {

           // if (!this.checkConflict(dovOd_user.SelectedDate))
           // {

                SortedList data = new SortedList();
                data.Add("user_id", Session["user_id"].ToString());
                data.Add("od", my_x2.unixDate(this.dovOd_user.SelectedDate) + " 00:00:00");
                // if (do_cal.s
                data.Add("do", my_x2.unixDate(this.dovDo_user.SelectedDate) + " 23:59:00");
                data.Add("type", this.freeType_dl.SelectedValue);
                data.Add("clinics", Session["klinika_id"]);
                data.Add("comment", this.comment_txt.Text.ToString().Trim());
                //ata.Add("rok",

                SortedList res = x2Mysql.mysql_insert("is_dovolenky", data);

                if (!Convert.ToBoolean(res["status"]))
                {
                    msg_lbl.Text = res["msg"].ToString();
                    x2log.logData(res, res["msg"].ToString(), "query error");
                }
                else
                {
                    dovolenky_tab.Controls.Clear();
                    this.drawDovolenTab();
                    this.drawUserActDovolenky();
                }
            //}
            //else
            //{
            //    this.statusInfo_lbl.Text = Resources.Resource.dov_konflikt;
            //}
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
            case "le":
                result = Resources.Resource.free_le;
                break;
        }

        return result;
    }
    
    protected void deleteDovolenka(object sender, EventArgs e)
    {
        Button data = (Button)sender;
        string name = data.ID.ToString();
        string[] tmp = name.Split('_');

        string query = my_x2.sprintf("DELETE FROM [is_dovolenky] WHERE [id]='{0}'", new string[] {tmp[1]});
        SortedList res = x2Mysql.execute(query);

        if (Convert.ToBoolean(res["status"]))
        {
            //this.msg_lbl.Text += "bolo zmazane...";
            Response.Redirect("dovolenky.aspx");
        }
        else
        {
            this.msg_lbl.Visible = true;
            this.msg_lbl.Text = res["msg"].ToString();
        }
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

        string query = @"
                            SELECT  [is_users].[id],[is_users].[full_name] AS [full_name],
                                    [is_dovolenky].[id] AS [dov_id], [is_dovolenky].[user_id],[is_dovolenky].[od],[is_dovolenky].[do], [is_dovolenky].[type],[is_dovolenky].[comment]
                                FROM [is_users] INNER JOIN [is_dovolenky] ON [is_users].[id] = [is_dovolenky].[user_id]
                            WHERE [is_dovolenky].[od] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00'
                                    AND [is_dovolenky].[user_id] = '{3}'
                            ORDER BY [is_dovolenky].[od]
                        ";
        query = x2Mysql.buildSql(query, new string[] { tc_year.ToString(), tc_month.ToString(), pocetDni.ToString(), Session["user_id"].ToString() });

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
            info.Text = data[i]["full_name"].ToString() + "  <strong>OD:</strong> " + data[i]["od"].ToString();
            info.Text += "   <strong>DO:</strong> " + data[i]["do"].ToString() + "<br>  <strong>Typ: </strong>";
            info.Text += this.getFreeType(data[i]["type"].ToString());
            string comment = my_x2.getStr(data[i]["comment"].ToString());
            if (comment.Length >0) info.Text += " <strong>Poznámka:</strong> "+ my_x2.getStr(data[i]["comment"].ToString());
            mojRiadok.Controls.Add(info);

            TableCell buttonCell = new TableCell();
            buttonCell.Width = Unit.Percentage(20);
            Button mojeTlac = new Button();
            mojeTlac.CssClass = "button red width-300";

            //mojeTlac.Command += new CommandEventHandler(this.deleteDovolenka);
            
           
            mojeTlac.ID = "Button_" + data[i]["dov_id"].ToString();
            mojeTlac.Text = Resources.Resource.erase.ToString();

            buttonCell.Controls.Add(mojeTlac);
            //mojeTlac.Click += new EventHandler(this.deleteDovolenka);
            mojRiadok.Controls.Add(buttonCell);

            Button printPdf = new Button();
            

        }

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        
        for (int b=0; b<pocetDov; b++)
        {
            Control ctrl = ctpl.FindControl("Button_" + data[b]["dov_id"].ToString());
            Button btn = (Button)ctrl;
            btn.Click += new EventHandler(this.deleteDovolenka);
        }
        
    }

}
