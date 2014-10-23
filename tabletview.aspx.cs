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

        this.kojenci_diag_btn.Enabled = false;
        this.OddB_diag_btn.Enabled = false;
        this.Pohotovost_diag_btn.Enabled = false;

        if (IsPostBack == false)
        {
            this.setMyDate();
            this.loadData();
        }
        else
        {
           this.setData();
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

    protected void setData()
    {
        this.kojenci_diag_btn.Enabled = true;
        this.OddB_diag_btn.Enabled = true;
        this.Pohotovost_diag_btn.Enabled = true;
        this.kojenci_txt.Text = this.makeLink(this.kojenci_diag.Text.ToString());
        this.OddB_txt.Text = this.makeLink(this.OddB_diag.Text.ToString());
        this.Pohotovost_txt.Text = this.makeLink(this.Pohotovost_diag.Text.ToString());
    }

    protected void loadData()
    {
        this.OddB_txt.Text = "";
        this.kojenci_txt.Text = "";
        this.Pohotovost_txt.Text = "";

        this.OddB_diag.Text = "";
        this.kojenci_diag.Text = "";
        this.Pohotovost_diag.Text = "";

        this.kojenci_diag_btn.Enabled = false;
        this.OddB_diag_btn.Enabled = false;
        this.Pohotovost_diag_btn.Enabled = false;

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
                this.kojenci_txt.Text = this.makeLink(type.Value.ToString());
                this.kojenci_diag.Text = type.Value.ToString();
                
                Session["kojneci"] = tmp[1];
                String strTmp = type.Value.ToString().Trim();

                this.kojenci_diag_btn.Enabled = true;

                /*if (strTmp.Length > 0)
                {
                    this.kojneci_diag_btn.Enabled = true;
                }
                else
                {
                    this.kojneci_diag_btn.Enabled = false;
                }*/
            }
            
            if (odd.IndexOf("B") != -1)
            {
                this.OddB_txt.Text = this.makeLink(type.Value.ToString());
                this.OddB_diag.Text = type.Value.ToString();
                Session["oddB"] = tmp[1];
                this.OddB_diag_btn.Enabled = true;
            }

            if (odd.IndexOf("OP") != -1)
            {
                this.Pohotovost_txt.Text = this.makeLink(type.Value.ToString());
                this.Pohotovost_diag.Text = type.Value.ToString();
                Session["OP"] = tmp[1];
                this.Pohotovost_diag_btn.Enabled = true;
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
        string res = x_db.update_row("is_hlasko", data, id);
        if (res != "ok")
        {
            Label1.Text = res;
        }
        else
        {
            Label1.Text = res;
        }
    }

    protected void kojenci_diag_btn_Click(object sender, EventArgs e)
    {
        //string tmp = x_db.getOsirixData(Session["kojneci"].ToString();

        string defRes = x2_var.UTFtoASCII(this.kojenci_diag.Text.ToString());

        this.saveData(Session["kojneci"].ToString(), defRes);
       // this.saveData()
    }

    protected void search_fnc(object sender, EventArgs e)
    {
        string searchName = this.osirix_search_txt.Text.ToString().Trim();

        if (searchName.Length > 0)
        {
            Response.Write("<script>window.open('http://10.10.2.49:3333/studyList?search=" + searchName + "','_blank','');</script>");
        }
        else
        {
            Response.Write("<script>alert('Prazdny retazec....')</script>");
        }
    }

}
