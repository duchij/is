using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tabletview : System.Web.UI.Page
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            this.setMyDate();
            this.loadData();
        }
        else
        {
            this.loadData();
        }

    }
    protected void setMyDate()
    {
        DateTime tc = DateTime.Now;
        //DateTime datum = new DateTime();

        if (tc.Hour > 9)
        {
            Calendar1.SelectedDate = DateTime.Today;
            //datum = DateTime.Today;
        }
        else
        {
            Calendar1.SelectedDate = DateTime.Today.AddDays(-1);
            // datum = DateTime.Today.AddDays(-1);
        }
    }

    protected void loadData()
    {
        /*DateTime tc = DateTime.Now;
        DateTime datum = new DateTime();

        if (tc.Hour > 9)
        {
            Calendar1.SelectedDate = DateTime.Today;
            datum = DateTime.Today;
        }
        else
        {
            Calendar1.SelectedDate = DateTime.Today.AddDays(-1);
            datum = DateTime.Today.AddDays(-1);
        }*/

        SortedList data = x_db.getHlaskoByDatum(this.Calendar1.SelectedDate);

        foreach (DictionaryEntry type in data)
        {
            string odd = type.Key.ToString();
            string[] tmp = odd.Split('|');
            if (odd.IndexOf("A") != -1)
            {
               this.OddA_txt.Text =  this.makeLink(type.Value.ToString());

               Session["oddA"] = tmp[1];
            }
            if (odd.IndexOf("B") != -1)
            {
                this.OddB_txt.Text = this.makeLink(type.Value.ToString());
                Session["oddB"] = tmp[1];
            }

            if (odd.IndexOf("OP") != -1)
            {
                this.Pohotovost_txt.Text = this.makeLink(type.Value.ToString());
                Session["OP"] = tmp[1];
            }
        }

        

    }

    public string makeLink(string str)
    {
        string result = "";

        string Odd = x2_var.UTFtoASCII(str);
        // string def = "";
        string[] tmp = this.returnStrArray(Odd);
        int cnt = tmp.Length;

        for (int i = 0; i < cnt; i++)
        {
            result += "<p><a href='http://10.10.2.49:3333/studyList?search=" + tmp[i] + "' target='_blank' style='font-size:xx-large;font-weight:bolder;'>" + tmp[i].ToUpper() + "</a></p>";
        }

        return result;
    }

    public string[] returnStrArray(string str)
    {
        string[] result = Regex.Split(str, "\r\n");
        return result;
    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        this.loadData();
    }

    protected void saveData(string id, string text)
    {
        SortedList data = new SortedList();
        data["osirix"] = text;


    }

}
