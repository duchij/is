using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class hlasko : System.Web.UI.Page
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();
    mysql_db x2MySQL = new mysql_db();

    // protected System.Web.UI.HtmlControls.HtmlGenericControl hlavicka;
    protected void Page_Init(object sender, EventArgs e)
    {
        //hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript' src='tinymce/jscripts/tiny_mce/tiny_mce.js'></script>"));
        // hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript'>tinyMCE.init({mode : 'textareas',        force_br_newlines : true,        force_p_newlines : false});</script>"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.msg_lbl.Text = "";
        this.hlasko_pl.Visible = false;        

        Response.AppendHeader("Refresh", 6000 + "; URL=hlasko.aspx");


        if (IsPostBack == false)
        {
            // Calendar1.SelectedDate = DateTime.Today;
            this.setMyDate();
            this.setEPC_init();
            this.loadHlasko();

        }
        else
        {
            //this.setMyDate();
        }

        /*if (IsCallback)
        {
            this.msg_lbl1.Text="test";
        }*/



    }

    protected void setEPC_init()
    {
        ListItem[] datum = new ListItem[2];
        DateTime now = DateTime.Now;
        int hour = now.Hour;
        int mint =now.Minute;
       
        datum[1] = new ListItem(DateTime.Today.AddDays(-1).ToShortDateString(), DateTime.Today.AddDays(-1).ToShortDateString());
        datum[0] = new ListItem(DateTime.Today.ToShortDateString(),DateTime.Today.ToShortDateString());
        this.hl_datum_cb.Items.AddRange(datum);

        this.workstart_txt.Text = DateTime.Now.ToString("HH:mm");
           

       

    
    }

    protected void saveActivity_fnc(object sender, EventArgs e)
    {
        SortedList data = new SortedList();
        data.Add("user_id", Session["user_id"].ToString());
        data.Add("hlasko_id", Session["akt_hlasenie"].ToString());

        DateTime dateTmp = Convert.ToDateTime(this.hl_datum_cb.SelectedValue.ToString());

        data.Add("work_start",my_x2.unixDate(dateTmp)+" "+this.workstart_txt.Text.ToString());
        data.Add("work_time", this.worktime_txt.Text.ToString());
        data.Add("work_type", this.worktype_cb.SelectedValue.ToString());
        data.Add("patient_name", this.patientname_txt.Text.ToString());
        data.Add("work_text", this.activity_txt.Text.ToString());

        SortedList res = x2MySQL.mysql_insert("is_hlasko_epc", data);

        Boolean status = Convert.ToBoolean(res["status"].ToString());

        if (!status)
        {
            this.msg_lbl.Text = res["msg"].ToString() + "<br>" + res["sql"].ToString();
        }
        else
        {

        }

    }

    protected void loadEPCData()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_hlasko_epc] WHERE [hlasko_id] = {1} ORDER BY [work_start] ASC", Session["akt_hlas"].ToString());
        x
    }

    protected void setMyDate()
    {
        DateTime now = DateTime.Now;

        int hour = now.Hour;
        //msg_lbl.Text = "hod:" + hour.ToString();

        if (hour >= 9)
        {
            Calendar1.SelectedDate = DateTime.Today;


        }
        else
        {
            Calendar1.SelectedDate = DateTime.Today.AddDays(-1);
            //  msg_lbl.Text += "tu smr";
        }

    }



    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        //user.Text = Calendar1.SelectedDate.ToString();
        //last_user.Text = Request.Cookies["user_id"].Value.ToString();
        //this.Calendar1.SelectedDate = this.Calendar1.SelectedDate;
        this.loadHlasko();

    }

    protected void loadHlasko()
    {
        //msg_lbl.Text = Calendar1.SelectedDate.ToString();

        SortedList data = x_db.getHlasko(this.Calendar1.SelectedDate, this.hlas_type.SelectedValue.ToString(), Session["user_id"].ToString());

        if (Convert.ToInt32(data["id"]) != 0)
        {
            this.osirix_txt.Text = data["osirix"].ToString();

            if (data["uzavri"].ToString() == "1")
            {
                this.send.Visible = false;
                this.hlasenie.Visible = false;
                this.dodatok.Visible = true;
                this.def_lock_btn.Visible = false;
                this.def_locl_w_btn.Visible = false;
                this.addInfo_btn.Enabled = true;
                this.view_hlasko.Visible = true;
                this.hlasko_lbl.Visible = true;
                this.view_hlasko.Text = data["text"].ToString();
                // osirix_txt = data["osirix"].ToString();
                this.send.Enabled = false;
            }
            else
            {
                this.send.Visible = true;
                this.hlasenie.Visible = true;
                this.dodatok.Visible = false;
                this.def_lock_btn.Visible = true;
                this.def_locl_w_btn.Visible = true;
                this.view_hlasko.Visible = false;
                this.addInfo_btn.Enabled = false;
                this.send.Enabled = true;
                this.hlasko_lbl.Visible = false;
                this.view_hlasko.Text = data["text"].ToString();
            }
            //SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString();
            this.user.Text = Session["fullname"].ToString();

            SortedList my_last_user = new SortedList();
            hlasenie.Text = data["text"].ToString();
            my_last_user = x_db.getUserInfoByID("is_users", data["last_user"].ToString());
            last_user.Text = my_last_user["full_name"].ToString();
            Session.Add("akt_hlasenie", data["id"].ToString());

            Session.Add("akt_hlasenie_creat_user", data["creat_user"].ToString());
            Session.Add("akt_hlasenie_last_user", data["last_user"].ToString());
        }
        else
        {
            SortedList newData = new SortedList();
            newData.Add("type", this.hlas_type.SelectedValue.ToString());
            newData.Add("dat_hlas", my_x2.unixDate(this.Calendar1.SelectedDate));
            newData.Add("text", Resources.Resource.odd_hlasko_html.ToString());
            newData.Add("creat_user", Session["user_id"].ToString());
            newData.Add("last_user", Session["user_id"].ToString());

            SortedList res = x2MySQL.mysql_insert("is_hlasko", newData);

            Boolean status = Convert.ToBoolean(res["status"]);

            if (status == true)
            {
                Session.Add("akt_hlasenie", res["last_id"].ToString());
                this.hlasenie.Text = newData["text"].ToString();
                this.user.Text = Session["fullname"].ToString();
                this.last_user.Text = Session["fullname"].ToString();
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }

        }


        this.generateOsirix();

    }
    /// <summary>
    /// Ulozi hlasenie, ak sa fnc zavola s 1 ulozi a uzavrie akt. hlasenie potom je mozne pisat len dodatky.. Nula urobi len save
    /// </summary>
    /// <param name="uzavri"></param>
    protected void saveData(bool uzavri, bool callBack)
    {
        SortedList data = new SortedList();
        data.Add("dat_hlas", my_x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("text", hlasenie.Text.ToString());
        data.Add("last_user", Session["user_id"].ToString());
        data.Add("creat_user", 0);
        data.Add("type", this.hlas_type.SelectedValue.ToString());

        if (uzavri == true)
        {
            data.Add("uzavri", "1");
        }

        //string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());

        SortedList res = x2MySQL.mysql_insert("is_hlasko", data);

        SortedList my_last_user = new SortedList();
        my_last_user = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);

        if (status == true)
        {
            //msg_lbl.Text = res;
            last_user.Text = my_last_user["full_name"].ToString();
            this.generateOsirix();
        }
        else
        {
            msg_lbl.Text = res["msg"].ToString();
        }
    }

    protected void saveDodatok(string my_dodatok)
    {
        SortedList data = new SortedList();
        SortedList my_last_user = new SortedList();
        data.Add("text", my_dodatok);
        data.Add("last_user", 0);
        data.Add("dat_hlas", my_x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("creat_user", 0);
        data.Add("type", this.hlas_type.SelectedValue.ToString());

        SortedList res = x2MySQL.mysql_insert("is_hlasko", data);
        my_last_user = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);


        if (status == true)
        {
            //msg_lbl.Text = res;
            //last_user.Text = my_last_user["full_name"].ToString();
        }
        else
        {
            msg_lbl.Text = res["msg"].ToString();
        }
    }



    protected void send_Click(object sender, EventArgs e)
    {

        this.saveData(false, false);

    }

    protected SortedList getSluzbyByDen(int den)
    {
        SortedList result = new SortedList();
        DateTime dnesJe = DateTime.Today;

        SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", dnesJe.Month.ToString(), dnesJe.Year.ToString());

        string[][] data = my_x2.parseSluzba(tmp["rozpis"].ToString());

        // int den = dnesJe.Day;

        result.Add("OUP", data[den - 1][1].ToString());
        result.Add("OddA", data[den - 1][2].ToString());
        result.Add("OddB", data[den - 1][3].ToString());
        result.Add("OP", data[den - 1][4].ToString());
        // result.Add("TRP", data[den - 1][5].ToString());

        return result;

    }


    protected void hlas_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.loadHlasko();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.send_Click(sender, e);
        Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
    }

    protected void toWord_Click(object sender, EventArgs e)
    {
        this.send_Click(sender, e);
        Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString() + "&w=1");
    }

    protected void pdfCretae_btn_Click(object sender, EventArgs e)
    {
        Session.Add("pdf", "print");
        Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
    }

    protected void def_lock_btn_Click(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (hlasenie.Visible == true)
        {
            this.saveData(true, false);
            Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
        }
        else
        {
            string tmp_hlasko = view_hlasko.Text;
            tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
            tmp_hlasko += "<br>" + dodatok.Text;
            this.saveDodatok(tmp_hlasko);
            Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
        }

    }

    protected void def_lock_btn_w_Click(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (hlasenie.Visible == true)
        {
            this.saveData(true, false);
            Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString() + "&w=1");
        }
        else
        {
            string tmp_hlasko = view_hlasko.Text;
            tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
            tmp_hlasko += "<br>" + dodatok.Text;
            this.saveDodatok(tmp_hlasko);
            Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString() + "&w=1");
        }

    }

    protected void addInfo_btn_Click(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        string tmp_hlasko = view_hlasko.Text;
        tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
        tmp_hlasko += "<br>" + dodatok.Text;
        this.saveDodatok(tmp_hlasko);
        this.loadHlasko();

    }
    protected void generateOsirix()
    {
        string text = this.osirix_txt.Text.ToString();



        string asciiTxt = x2_var.UTFtoASCII(text.Trim());


        SortedList data = new SortedList();

        if (asciiTxt.Length > 0)
        {
            data.Add("osirix", asciiTxt);
            string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());
            string html = "";
            if (res == "ok")
            {
                //this.osirix_url.Text = asciiTxt.ToString();

                string[] lines = this.returnStrArray(asciiTxt.ToString());
                foreach (string line in lines)
                {
                    html += "<p class='align-center'><a class='blue button' href='http://10.10.2.49:3333/studyList?search=" + line + "' target='_blank' >" + line.ToUpper() + "</a></p>";
                }

                this.osirix_url.Text = html.ToString();

            }
            else
            {
                this.msg_lbl.Text = res.ToString();

            }
        }
        else
        {
            this.osirix_url.Text = "";
        }
    }

    protected void osirix_btn_Click(object sender, EventArgs e)
    {
        this.generateOsirix();
    }

    public string[] returnStrArray(string str)
    {
        string[] result = Regex.Split(str, "\r\n");
        return result;
    }
}
