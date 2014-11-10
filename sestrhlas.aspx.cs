using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;


public partial class sestrhlas : System.Web.UI.Page
{
   

    my_db x_db = new my_db();
    mysql_db x2MySql = new mysql_db();
    x2_var x2 = new x2_var();

   string userRights = "";
   
 
    

    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.AppendHeader("Refresh", 300 + "; URL=sestrhlas.aspx"); 

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        userRights = Session["rights"].ToString();
        if (IsPostBack == false)
        {
            Calendar1.SelectedDate = DateTime.Today;

            if (userRights == "sestra")
            {
                oddType_cb.Items.Add(new ListItem("MSV", "msv"));
            }
            if (userRights == "sestra_vd")
            {
                oddType_cb.Items.Add(new ListItem("Velke deti", "vd"));
            }

            if ((userRights == "admin") || (userRights == "poweruser"))
            {
                oddType_cb.Items.Add(new ListItem("MSV", "msv"));
                oddType_cb.Items.Add(new ListItem("Velke deti", "vd"));

            }

            this.loadHlasko();
        }
    }


    protected void loadHlasko()
    {
        this.hlasenie.Text = "";
        this.msg_lbl.Text = "";
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());
        user.Text = akt_user_info["full_name"].ToString();

        SortedList data = x_db.getSestrHlasko(Calendar1.SelectedDate, oddType_cb.SelectedValue.ToString(), predZad_cb.SelectedValue.ToString(), hlasenie.Text.ToString(), Session["user_id"].ToString(), this.time_cb.SelectedValue.ToString());

        if (Convert.ToInt32(data["id"]) != 0)
        {
            if (data["uzavri"].ToString() == "1")
            {
                hlasenie.Visible = false;
                //dodatok.Visible = true;
                def_lock_btn.Enabled = false;
                //addInfo_btn.Enabled = true;
                view_hlasko.Visible = true;
                hlasko_lbl.Visible = true;

                view_hlasko.Text = data["hlasko"].ToString();

                //view_hlasko.Text += Resources.Resource.odd_vloz_clip_info.ToString();
                send.Enabled = false;
            }
            else
            {
                hlasenie.Visible = true;
                //dodatok.Visible = false;
                def_lock_btn.Enabled = true;
                view_hlasko.Visible = false;

                //addInfo_btn.Enabled = false;
                send.Enabled = true;
                hlasko_lbl.Visible = false;
            }

            SortedList my_last_user = new SortedList();

            hlasenie.Text = data["hlasko"].ToString();
            my_last_user = x_db.getUserInfoByID("is_users", data["last_user"].ToString());
            last_user.Text = my_last_user["full_name"].ToString();
            Session.Add("akt_hlasenie", data["id"].ToString());
            Session.Add("hlasko_creat_user",data["creat_user"].ToString());
        }
        else
        {
           /* Calendar1.SelectedDate, oddType_cb.SelectedValue.ToString(), predZad_cb.SelectedValue.ToString(), hlasenie.Text.ToString(), Session["user_id"].ToString(), this.time_cb.SelectedValue.ToString()*/

            this.hlasenie.Visible = true;
            //dodatok.Visible = false;
            this.def_lock_btn.Enabled = true;
            this.view_hlasko.Visible = false;

            //addInfo_btn.Enabled = false;
            this.send.Enabled = true;
            this.hlasko_lbl.Visible = false;

            SortedList newData = new SortedList();
            newData.Add("dat_hlas", x2.unixDate(this.Calendar1.SelectedDate));
            newData.Add("oddelenie",this.oddType_cb.SelectedValue.ToString());
            newData.Add("lokalita",this.predZad_cb.SelectedValue.ToString());
            newData.Add("cas",this.time_cb.SelectedValue.ToString());
            newData.Add("creat_user", Session["user_id"].ToString());
            newData.Add("last_user", Session["user_id"].ToString());
            newData.Add("hlasko", "Hlasenie sestier");
            SortedList result = x2MySql.mysql_insert("is_hlasko_sestry",newData);

            Boolean status = Convert.ToBoolean(result["status"]);
            if (status == true)
            {
                Session.Add("akt_hlasenie", result["last_id"].ToString());
                Session.Add("hlasko_creat_user", Session["user_id"].ToString());
                this.hlasenie.Text = "Hlasenie sestier...";
                last_user.Text = Session["fullname"].ToString();
            }
            else
            {
                msg_lbl.Text = result["msg"].ToString();
                this.myAlert(result["msg"].ToString());
            }
        }
    }

    protected void myAlert(string msg)
    {
        //Response.Write("<script>Alert('" + msg + "');</script>");
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append ("window.onLoad=function() {");
        sb.Append ("alert('");
        sb.Append (msg);
        sb.Append ("')};</script>");
        ClientScript.RegisterClientScriptBlock(this.Page.GetType(),"alert",sb.ToString());

    }

    protected void loadPrevHlasko()
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());
        user.Text = akt_user_info["full_name"].ToString();

        SortedList data = new SortedList();

        if (this.time_cb.SelectedValue.ToString() == "d")
        {
           data = x_db.getSestrHlasko(Calendar1.SelectedDate.AddDays(-1), oddType_cb.SelectedValue.ToString(), predZad_cb.SelectedValue.ToString(), hlasenie.Text.ToString(), Session["user_id"].ToString(), "n");
        }

        if (this.time_cb.SelectedValue.ToString() == "n")
        {
            data = x_db.getSestrHlasko(Calendar1.SelectedDate, oddType_cb.SelectedValue.ToString(), predZad_cb.SelectedValue.ToString(), hlasenie.Text.ToString(), Session["user_id"].ToString(), "d");
        }
        
        hlasenie.Text += Resources.Resource.odd_prev_hlasko + data["hlasko"].ToString();

        if (data.ContainsKey("error"))
        {
            msg_lbl.Text = data["error"].ToString();
        }
    }


    /// <summary>
    /// Ulozi hlasenie, ak sa fnc zavola s 1 ulozi a uzavrie akt. hlasenie potom je mozne pisat len dodatky.. Nula urobi len save
    /// </summary>
    /// <param name="uzavri"></param>
    protected void saveData(bool uzavri)
    {
        SortedList data = new SortedList();
        data.Add("hlasko", hlasenie.Text.ToString());
        data.Add("last_user",Session["user_id"].ToString());
        data.Add("dat_hlas", x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("oddelenie", this.oddType_cb.SelectedValue.ToString());
        data.Add("lokalita", this.predZad_cb.SelectedValue.ToString());
        data.Add("cas", this.time_cb.SelectedValue.ToString());
        data.Add("creat_user", 0);

        if (uzavri == true)
        {
            data.Add("uzavri", "1");
        }

        SortedList res = x2MySql.mysql_insert("is_hlasko_sestry", data);
        Boolean status = Convert.ToBoolean(res["status"]);

        if (status == true)
        {
            last_user.Text = Session["fullname"].ToString();
        }
        else
        {
            msg_lbl.Text = res["msg"].ToString();
        }
    }

    protected void save_fnc(object sender, EventArgs e)
    {

        this.saveData(false);

    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        //user.Text = Calendar1.SelectedDate.ToString();
        //last_user.Text = Request.Cookies["user_id"].Value.ToString();

        this.loadHlasko();

    }

    protected void saveDodatok(string my_dodatok)
    {
        SortedList data = new SortedList();
        SortedList my_last_user = new SortedList();
        data.Add("hlasko", my_dodatok);
        data.Add("last_user",Session["user_id"].ToString());

        string res = x_db.update_row("is_hlasko_sestry", data, Session["akt_hlasenie"].ToString());
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

    protected void print_fnc(object sender, EventArgs e)
    {
        this.save_fnc(sender, e);
        Response.Redirect("printtses.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
    }

    protected void def_loc_fnc(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (hlasenie.Visible == true)
        {
            this.saveData(true);
            Response.Redirect("printtses.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
        }
        /*else
        {
            string tmp_hlasko = view_hlasko.Text;
            tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
            tmp_hlasko += "<br>" + dodatok.Text;
            this.saveDodatok(tmp_hlasko);
            Response.Redirect("printtses.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
        }*/

    }

    //protected void addInfo_btn_Click(object sender, EventArgs e)
    //{
    //    SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

    //    string tmp_hlasko = view_hlasko.Text;
    //    tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
    //    tmp_hlasko += "<br>" + dodatok.Text;
    //    this.saveDodatok(tmp_hlasko);
    //    this.loadHlasko();

    //}

    protected void copyYesterday_btn_Click(object sender, EventArgs e)
    {
       
        this.loadPrevHlasko();

    }
}
