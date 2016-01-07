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




public partial class dovolenky_sestr : System.Web.UI.Page
{
    private class Deps
    {
        private string _rights;
        private string _wgroup;
        private string _clinic;

        public string rights
        {
            get { return _rights; }
            set { _rights = value; }
        }

        public string wgroup
        {
            get { return _wgroup; }
            set { _wgroup = value; }
        }

        public string clinic
        {
            get { return _clinic; }
            set { _clinic = value; }
        }
    }
    //my_db x_db = new my_db();
    mysql_db x2Mysql = new mysql_db();
    x2_var my_x2 = new x2_var();
    log x2log = new log();

    public string rights = "";
    public string wgroup = "";

    public string department = "";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.msg_lbl.Text = "";

        

        this.rights = Session["rights"].ToString();
        this.wgroup = Session["workgroup"].ToString();
        this.department = Session["oddelenie"].ToString();


        if (this.rights == "users")
        {
            this.uziv_dovolenka.Visible = false;
        }

        my_x2.fillYearMonth(ref this.mesiac_cb,ref this.rok_cb, Session["month_dl"].ToString(), Session["years_dl"].ToString());


        if (!IsPostBack)
        {
            this.mesiac_cb.SelectedValue = DateTime.Today.Month.ToString();
            this.rok_cb.SelectedValue = DateTime.Today.Year.ToString();

            this.dovOd_user.SelectedDate = DateTime.Today;
            this.dovDo_user.SelectedDate = DateTime.Today.AddDays(1);

            this.drawDovolenTab();
           // this.drawUserActDovolenky();
            this.loadNurses();
            

        }
        else
        {
            this.dovolenky_tab.Controls.Clear();
            // this.loadNurses();
            
            this.drawDovolenTab();
            this.drawUserActDovolenky(Convert.ToInt32(this.nurses_dl.SelectedValue.ToString()));
        }

    }


    protected void saveDovStatus()
    {
    }

    protected void changeDovStatus_fnc(object sender, EventArgs e)
    {
        //this.actStatusDovol();
    }

    protected void loadNurses()
    {
        string query = @"SELECT [id],[name3] FROM [kdch_nurse] WHERE [idf]='{0}'";

        query = x2Mysql.buildSql(query, new string[] { this.department });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);
        this.nurses_dl.Items.Add(new ListItem("-", "0"));
        int tblLn = table.Count;
        //this.nurses_dl.Items.Add
        for (int i = 0; i < tblLn; i++)
        {
            this.nurses_dl.Items.Add(new ListItem(table[i]["name3"].ToString(), table[i]["id"].ToString()));
        }

    }
    

    protected void loadKomplViewFnc(object sender, EventArgs e)
    {
        Session.Add("dov_mesiac", this.mesiac_cb.SelectedValue);
        Session.Add("dov_rok", this.rok_cb.SelectedValue);
        Response.Redirect("dovkompl.aspx");
    }

    protected void loadDovByIdFnc(object sender, EventArgs e)
    {
        int id=Convert.ToInt32(this.nurses_dl.SelectedValue.ToString());

        this.drawUserActDovolenky(id);
    }

    protected void drawDovolenTab()
    {
        int tc_month = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int tc_rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int pocetDni = DateTime.DaysInMonth(tc_rok, tc_month);

        string typ = this.activies_dl.SelectedValue.ToString();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [is_users].[id],[is_users].[full_name],[t_dov].[id] AS [dov_id],[t_dov].[user_id],[t_dov].[od], ");
        sb.AppendLine("[t_dov].[do] FROM [is_users] ");
        sb.AppendLine("INNER JOIN [is_dovolenky_sestr] AS [t_dov] ON [is_users].[id]=[t_dov].[user_id] ");
        sb.AppendFormat("WHERE ([t_dov].[od] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00'",tc_rok,tc_month,pocetDni);
        sb.AppendFormat("OR [t_dov].[do] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00') ", tc_rok, tc_month, pocetDni);
        sb.AppendFormat(" AND [t_dov].[type]='{0}' AND [t_dov].[clinics]='{1}' ORDER BY [t_dov].[do] ASC",typ,Session["klinika_id"]);

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
        sb.AppendFormat("SELECT COUNT(*) AS [pocet] FROM [is_dovolenky_sestr] WHERE '{0} 00:00:00' ", my_x2.unixDate(date));
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
            int id = Convert.ToInt32(this.nurses_dl.SelectedValue.ToString());

            if (id > 0)
            {
                SortedList data = new SortedList();
                data.Add("od", my_x2.unixDate(this.dovOd_user.SelectedDate) + " 00:00:00");
                // if (do_cal.s
                data.Add("do", my_x2.unixDate(this.dovDo_user.SelectedDate) + " 23:59:00");
                data.Add("type", this.freeType_dl.SelectedValue);
                data.Add("clinics", Session["klinika_id"]);
                data.Add("comment", this.comment_txt.Text.ToString().Trim());
                data.Add("user_id", id);

                SortedList res = x2Mysql.mysql_insert("is_dovolenky_sestr", data);

                if (!Convert.ToBoolean(res["status"]))
                {
                    my_x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
                    x2log.logData(res, res["msg"].ToString(), "query error");
                }
                else
                {
                    dovolenky_tab.Controls.Clear();
                    this.drawDovolenTab();
                    this.drawUserActDovolenky(id);
                }
            }
            else
            {
                my_x2.errorMessage2(ref this.msg_lbl, "Pozor nie je vybrana sestricka !!!");
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

        string query = my_x2.sprintf("DELETE FROM [is_dovolenky_sestr] WHERE [id]='{0}'", new string[] {tmp[1]});
        SortedList res = x2Mysql.execute(query);

        if (Convert.ToBoolean(res["status"]))
        {
            //Response.Redirect("dovolenky_sestr.aspx");
        }
        else
        {
            my_x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
        }
    }

    public void drawUserActDovolenky(int id)
    {
        this.zoznamUser_tbl.Controls.Clear();

        int tc_month = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int tc_year = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int pocetDni = DateTime.DaysInMonth(tc_year, tc_month);
        string tmp_od = my_x2.unixDate(new DateTime(tc_year, tc_month, 1));

        StringBuilder query = new StringBuilder();
        query.AppendLine("SELECT [is_users].[id],[is_users].[full_name] AS [full_name],");
        query.AppendLine("[t_dov].[id] AS [dov_id], [t_dov].[user_id],[t_dov].[od],[t_dov].[do], [t_dov].[type],[t_dov].[comment] ");
        query.AppendLine("FROM [is_users] INNER JOIN [is_dovolenky_sestr] AS [t_dov] ON [is_users].[id] = [t_dov].[user_id] ");
        query.AppendFormat("WHERE ([t_dov].[od] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00' ", tc_year, tc_month, pocetDni);
        query.AppendFormat("OR [t_dov].[do] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00') ", tc_year, tc_month, pocetDni);
        query.AppendFormat("AND [t_dov].[user_id] = '{0}' ",id);
        query.AppendLine("ORDER BY [t_dov].[od]");

        Dictionary<int, Hashtable> data = x2Mysql.getTable(query.ToString());

        int pocetDov = data.Count;
        this.zoznamUser_tbl.Width = Unit.Percentage(100);
        for (int i = 0; i < pocetDov; i++)
        {
           TableRow mojRiadok = new TableRow();
           mojRiadok.ID = "dovriadok_" + data[i]["dov_id"].ToString();
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

            mojeTlac.Click += new EventHandler(this.deleteDovolenka);
            //comment.Attributes.Add("onChange", "saveNurseShiftComment('" + comment.ID.ToString() + "');");
            //mojeTlac.OnClientClick = "deleteNurseActivity('" + data[i]["dov_id"].ToString() + "'); return false;";

            mojeTlac.OnClientClick = "confirm('Zmazat danu dovoleku?');";
           // mojeTlac.Attributes.Add("onClick", "deleteNurseActivity('" + data[i]["dov_id"].ToString() + "'");
            mojeTlac.ID = "Button_" + data[i]["dov_id"].ToString();
            mojeTlac.Text = Resources.Resource.erase.ToString();

            buttonCell.Controls.Add(mojeTlac);
            mojRiadok.Controls.Add(buttonCell);
        }
        
    }

}
