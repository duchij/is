using System;
using System.Collections;
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

    // protected System.Web.UI.HtmlControls.HtmlGenericControl hlavicka;
    protected void Page_Init(object sender, EventArgs e)
    {
        //hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript' src='tinymce/jscripts/tiny_mce/tiny_mce.js'></script>"));
        // hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript'>tinyMCE.init({mode : 'textareas',        force_br_newlines : true,        force_p_newlines : false});</script>"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        

        Response.AppendHeader("Refresh", 300 + "; URL=hlasko.aspx"); 


        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }





        if (IsPostBack == false)
        {
            // Calendar1.SelectedDate = DateTime.Today;
            this.setMyDate();



            this.loadHlasko();
        }




    }


    protected void setMyDate()
    {
        DateTime now = DateTime.Now;

        int hour = now.Hour;
        msg_lbl.Text = "hod:" + hour.ToString();
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

        this.loadHlasko();

    }

    protected void loadHlasko()
    {
        //msg_lbl.Text = Calendar1.SelectedDate.ToString();

        SortedList data = x_db.getHlasko(Calendar1.SelectedDate, hlas_type.SelectedValue.ToString(), hlasenie.Text.ToString(), Session["user_id"].ToString());
        this.osirix_txt.Text = data["osirix"].ToString();
        if (data["uzavri"].ToString() == "1")
        {
            hlasenie.Visible = false;
            dodatok.Visible = true;
            def_lock_btn.Enabled = false;
            addInfo_btn.Enabled = true;
            view_hlasko.Visible = true;
            hlasko_lbl.Visible = true;
            view_hlasko.Text = data["text"].ToString();
           // osirix_txt = data["osirix"].ToString();
            send.Enabled = false;


        }
        else
        {

            hlasenie.Visible = true;
            dodatok.Visible = false;
            def_lock_btn.Enabled = true;
            view_hlasko.Visible = false;
            addInfo_btn.Enabled = false;
            send.Enabled = true;
            hlasko_lbl.Visible = false;
            view_hlasko.Text = data["text"].ToString();
        }
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());
        user.Text = akt_user_info["full_name"].ToString();

        SortedList my_last_user = new SortedList();

        //SortedList lekari = this.getSluzbyByDen(Convert.ToInt32(Calendar1.SelectedDate.Day));

        if (data["new_ins"] == "true")
        {

            hlasenie.Text = data["text"].ToString();
            my_last_user = x_db.getUserInfoByID("is_users", data["last_user"].ToString());
            last_user.Text = my_last_user["full_name"].ToString();
            Session.Add("akt_hlasenie", data["akt_hlasenie"].ToString());
        }

        if (data["update"] != null)
        {
            hlasenie.Text = data["text"].ToString();
            my_last_user = x_db.getUserInfoByID("is_users", data["last_user"].ToString());
            last_user.Text = my_last_user["full_name"].ToString();
            Session.Add("akt_hlasenie", data["id"].ToString());
        }


        if (data.ContainsKey("error"))
        {
            msg_lbl.Text = data["error"].ToString();
        }
        this.generateOsirix();

    }
    /// <summary>
    /// Ulozi hlasenie, ak sa fnc zavola s 1 ulozi a uzavrie akt. hlasenie potom je mozne pisat len dodatky.. Nula urobi len save
    /// </summary>
    /// <param name="uzavri"></param>
    protected void saveData(bool uzavri,bool callBack)
    {
        SortedList data = new SortedList();
        SortedList my_last_user = new SortedList();
        data.Add("text", hlasenie.Text.ToString());
        data.Add("last_user", Session["user_id"].ToString());

        if (uzavri == true)
        {
            data.Add("uzavri", "1");
        }

        string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());
        my_last_user = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (res.IndexOf("ok") != -1)
        {
            //msg_lbl.Text = res;
            last_user.Text = my_last_user["full_name"].ToString();

            if (callBack == true)
                Response.Write("OK");
            

        }
        else
        {
            msg_lbl.Text = res + Session["akt_hlasenie"].ToString();
            if (callBack == true)
                Response.Write("Error: "+res + Session["akt_hlasenie"].ToString());
        }
    }

    protected void saveDodatok(string my_dodatok)
    {
        SortedList data = new SortedList();
        SortedList my_last_user = new SortedList();
        data.Add("text", my_dodatok);
        data.Add("last_user", Session["user_id"].ToString());

        string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());
        my_last_user = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (res.IndexOf("ok") != -1)
        {
            //msg_lbl.Text = res;
            last_user.Text = my_last_user["full_name"].ToString();
        }
        else
        {
            msg_lbl.Text = res + Session["akt_hlasenie"].ToString();
        }
    }



    protected void send_Click(object sender, EventArgs e)
    {

        this.saveData(false,false);

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
            this.saveData(true,false);
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
            this.saveData(true,false);
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

        string asciiTxt = x2_var.UTFtoASCII(text);
      

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
                    html += "<p><a href='http://10.10.2.49:3333/studyList?search=" + line + "' target='_blank' style='font-size:15px;font-weight:bolder;'>" + line.ToUpper() + "</a></p>";
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
        string[] result = Regex.Split(str,"\r\n");
        return result;
    }
}
