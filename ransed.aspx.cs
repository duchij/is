using System;
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
using System.Text;

public partial class ransed : System.Web.UI.Page
{
    x2_var my_x2 = new x2_var();
    mysql_db x2db = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {


        if (IsPostBack == false)
        {
            this.setMyDate();
            this.loadData();
        }

    }

    protected void loadData()
    {
        this.name_txt.Text = "";
        this.note_txt.Text = "";

        this.loadSluzby();
        this.loadKojenci();
        this.loadDievcata();
        this.loadChlapci();
    }


    protected void setMyDate()
    {
        DateTime tc = DateTime.Now;
        //DateTime datum = new DateTime();
        this.Calendar1.SelectedDate = DateTime.Today;

        /* if (tc.Hour > 9)
         {
             this.Calendar1.SelectedDate = DateTime.Today;
             //datum = DateTime.Today;
         }
         else
         {
             this.Calendar1.SelectedDate = DateTime.Today.AddDays(-1);
             // datum = DateTime.Today.AddDays(-1);
         }*/
    }

    protected void date_changed_fnc(object sender, EventArgs e)
    {
        this.loadData();
    }

    protected void loadSluzby()
    {
        DateTime datum = this.Calendar1.SelectedDate.AddDays(-1);

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT GROUP_CONCAT([osirix] SEPARATOR ' ') AS [osirix] FROM [is_hlasko] WHERE [dat_hlas] = '{0}'", my_x2.unixDate(datum));
        SortedList result = x2db.getRow(sb.ToString());
        sb.Length = 0;
        string tmp = result["osirix"].ToString().Replace((char)13, ' ');

        string[] str = tmp.Split(' ');
        sb.AppendLine("<ul>");
        Label meno_lnk = new Label();
        for (int i = 0; i < str.Length; i++)
        {
            meno_lnk.ID = "sluzba";
            if (str[i].ToString().Trim().Length > 0)
            {
                sb.AppendFormat("<li><a href='http://10.10.2.49:3333/studyList?search={0}' target='_blank'>{0}</a></li>", str[i].ToString());
            }
        }
        sb.AppendLine("</ul>");
        meno_lnk.Text = sb.ToString();
        this.sluzba_pl.Controls.Add(meno_lnk);

    }

    protected void loadKojenci()
    {
        DateTime datum = this.Calendar1.SelectedDate;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "KOJ");

        Dictionary<int, SortedList> result = x2db.getTable(sb.ToString());
        sb.Length = 0;
        Label meno_lnk = new Label();
        meno_lnk.ID = "kojenci";

        sb.AppendFormat("<ul>");

        for (int i = 0; i < result.Count; i++)
        {
            sb.AppendFormat("<li><a href='http://10.10.2.49:3333/studyList?search={0}' target='_blank'>{0}</a><br>{1}</li>", result[i]["name"].ToString(), result[i]["poznamka"].ToString());

        }
        sb.AppendFormat("</ul>");
        meno_lnk.Text = sb.ToString();
        this.kojenci_pl.Controls.Add(meno_lnk);
    }

    protected void loadDievcata()
    {
        DateTime datum = this.Calendar1.SelectedDate;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "MSV");

        Dictionary<int, SortedList> result = x2db.getTable(sb.ToString());
        sb.Length = 0;
        Label meno_lnk = new Label();
        meno_lnk.ID = "dievacata";

        sb.AppendFormat("<ul>");

        for (int i = 0; i < result.Count; i++)
        {
            sb.AppendFormat("<li><a href='http://10.10.2.49:3333/studyList?search={0}' target='_blank'>{0}</a><br>{1}</li>", result[i]["name"].ToString(), result[i]["poznamka"].ToString());

        }
        sb.AppendFormat("</ul>");
        meno_lnk.Text = sb.ToString();
        this.dievcata_pl.Controls.Add(meno_lnk);
    }


    protected void loadChlapci()
    {
        DateTime datum = this.Calendar1.SelectedDate;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "VD");

        Dictionary<int, SortedList> result = x2db.getTable(sb.ToString());
        sb.Length = 0;
        Label meno_lnk = new Label();
        meno_lnk.ID = "chlapci";

        sb.AppendFormat("<ul>");

        for (int i = 0; i < result.Count; i++)
        {
            sb.AppendFormat("<li><a href='http://10.10.2.49:3333/studyList?search={0}' target='_blank'>{0}</a><br>{1}</li>", result[i]["name"].ToString(), result[i]["poznamka"].ToString());

        }
        sb.AppendFormat("</ul>");
        meno_lnk.Text = sb.ToString();
        this.chlapci_pl.Controls.Add(meno_lnk);
    }

    protected void add_patient_click_fnc(object sender, EventArgs e)
    {
        SortedList data = new SortedList();

        data["name"] = this.name_txt.Text.ToString();
        data["poznamka"] = this.note_txt.Text.ToString();
        data["odd"] = this.odd_dl.SelectedValue.ToString();
        data["date"] = my_x2.unixDate(this.Calendar1.SelectedDate);

        SortedList res = x2db.mysql_insert("is_osirix", data);

        bool status = Convert.ToBoolean(res["status"].ToString());

        if (status == false)
        {
            this.alert("Chyba: " + res["msg"].ToString());
        }
        else
        {
            this.loadData();
        }

    }

    protected void alert(string message)
    {
        Response.Write("<script>alert('" + message + "');</script>");
    }





}
