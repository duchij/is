using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class tabletview : System.Web.UI.Page
{
   my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();
    mysql_db x2db = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {

       /* this.kojenci_diag_btn.Enabled = false;
        this.OddB_diag_btn.Enabled = false;
        this.Pohotovost_diag_btn.Enabled = false;*/

        if (IsPostBack == false)
        {
          this.setMyDate();
          this.loadPostData();
          //  this.loadData();
        }
        else
        {
            this.setMyDate();
            this.loadPostData();
            
          // this.setData();
        }

    }

    protected void loadPostData()
    {
        this.loadSluzby();

        this.loadKojenciData();
        this.loadDievcataData();
        this.loadChlapciData();
    }

    protected void loadSluzby()
    {

        this.hlasenie.Controls.Clear();

        DateTime datum = this.Calendar1.SelectedDate;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT GROUP_CONCAT([osirix] SEPARATOR ' ') AS [osirix] FROM [is_hlasko] WHERE [dat_hlas] = '{0}'", my_x2.unixDate(datum));
        SortedList result = x2db.getRow(sb.ToString());

        string tmp = result["osirix"].ToString().Replace((char)13,' ');

        string[] str = tmp.Split(' ');

        for (int i = 0; i < str.Length; i++)
        {
            HyperLink meno_lnk = new HyperLink();
            meno_lnk.ID = "sluzba_" + i.ToString();
            meno_lnk.Text = "<center>"+str[i].ToUpper()+"</center><br>";
            meno_lnk.NavigateUrl = "http://10.10.2.49:3333/studyList?search=" + str[i];
            meno_lnk.Target = "_blank";
            meno_lnk.Style.Add("text-align", "center");
            meno_lnk.Style.Add("font-size", "x-large");
            meno_lnk.Style.Add("line-height", "120%");
                   
            
          
            this.hlasenie.Controls.Add(meno_lnk);
        }

    }


    protected void loadKojenciData()
    {
        this.kojenci_tbl.Controls.Clear();

        StringBuilder sb = new StringBuilder();
        DateTime datum = this.Calendar2.SelectedDate;

        sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "KOJ");

        Dictionary<int, SortedList> result = x2db.getTable(sb.ToString());

        int rows = result.Count;

        if (rows > 0)
        {
            for (int i = 0; i < rows; i++)
            {
                this.makeDynamicTable(result[i], "kojenci", "kojenci_tbl");

            }
        }
    }

    protected void loadChlapciData()
    {
        this.chlapci_tbl.Controls.Clear();

        StringBuilder sb = new StringBuilder();
        DateTime datum = this.Calendar2.SelectedDate;

        sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "VD");

        Dictionary<int, SortedList> result = x2db.getTable(sb.ToString());

        int rows = result.Count;

        if (rows > 0)
        {
            for (int i = 0; i < rows; i++)
            {
                this.makeDynamicTable(result[i], "chlapci", "chlapci_tbl");

            }
        }
    }

    protected void loadDievcataData()
    {
        this.dievcata_tbl.Controls.Clear();

        StringBuilder sb = new StringBuilder();
        DateTime datum = this.Calendar2.SelectedDate;

        sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "MSV");

        Dictionary<int, SortedList> result = x2db.getTable(sb.ToString());

        int rows = result.Count;

        if (rows > 0)
        {
            for (int i = 0; i < rows; i++)
            {
                this.makeDynamicTable(result[i], "dievcata", "dievcata_tbl");

            }
        }
    }



    protected void setMyDate()
    {
        DateTime tc = DateTime.Now;
        //DateTime datum = new DateTime();
        this.Calendar2.SelectedDate = DateTime.Today;
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
        this.loadSluzby();
    }

    protected void Calendar2_SelectionChanged(object sender, EventArgs e)
    {
        this.loadKojenciData();
        this.loadDievcataData();
        this.loadChlapciData();
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

    protected void makeDynamicTable(SortedList data,string prefix, string _tabulka)
    {
        string id = "";
        if (data["item_id"] == null)
        {
            id = data["last_id"].ToString();
        }
        else
        {
            id = data["item_id"].ToString();
        }
        //row_tbl.ID = prefix+"_row_" + id.ToString();

        Control tbl = FindControl(_tabulka);
        Table tabulka = (Table)tbl;

        TableRow row_tbl = new TableRow();
        
        tabulka.Controls.Add(row_tbl);



        TableCell cell_tbl = new TableCell();
        //cell_tbl.ID = prefix+"_cell_" + id.ToString();


        HyperLink meno_lnk = new HyperLink();
        meno_lnk.ID = prefix+"_meno_" + id.ToString();
        meno_lnk.Style.Add("font-size", "x-large");
        meno_lnk.Style.Add("padding", "5px");
        meno_lnk.Style.Add("float", "left");
        if (data["name"] != null)
        {
            meno_lnk.Text = data["name"].ToString(); 
            
            meno_lnk.NavigateUrl = "http://10.10.2.49:3333/studyList?search=" + data["name"].ToString(); 
        }
        else
        {
            Control txt = FindControl(prefix + "_diag");
            TextBox text_tmp = (TextBox)txt;
            meno_lnk.Text = text_tmp.Text.ToString();
            meno_lnk.NavigateUrl = "http://10.10.2.49:3333/studyList?search=" + meno_lnk.Text.ToString();
        }

        
        meno_lnk.Target = "_blank";
        cell_tbl.Controls.Add(meno_lnk);

        Label note_lbl = new Label();
        note_lbl.ID = prefix + "_lbl_" + id;
        note_lbl.Style.Add("float", "left");
        if (data["poznamka"] != null)
        {
            note_lbl.Text = "<div style='font-size:small;'>" + data["poznamka"].ToString() + "</div>";
        }
        else
        {
            Control txt1 = FindControl(prefix + "_note");
            TextBox text_tmp1 = (TextBox)txt1;
            note_lbl.Text = "<div style='font-size:small;'>" + text_tmp1.Text.ToString() + "</div>";
        }
        cell_tbl.Controls.Add(note_lbl);



        

        Button delete_btn = new Button();
        delete_btn.ID = prefix+"_delete_" + id.ToString();
        delete_btn.Text = "Zmaz";
        delete_btn.Style.Add("font-size", "large");
        delete_btn.Style.Add("background-color", "red");
        delete_btn.Style.Add("color", "yellow");
        delete_btn.Style.Add("float", "right");
        delete_btn.Click += new EventHandler(delete_btn_fnc);

        cell_tbl.Controls.Add(delete_btn);

        row_tbl.Controls.Add(cell_tbl);
    }

    protected void delete_btn_fnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] str = btn.ID.ToString().Split('_');

        SortedList res = x2db.execute("DELETE FROM [is_osirix] WHERE [item_id]=" + str[2]);

        Boolean result = Convert.ToBoolean(res["status"]);
        if (!result)
        {
            Label1.Text = res["msg"].ToString();
        }
        else
        {
            this.loadPostData();
        }
        
     
       
    }

    protected void kojenci_diag_btn_Click(object sender, EventArgs e)
    {
        //string tmp = x_db.getOsirixData(Session["kojneci"].ToString();

       // string defRes = x2_var.UTFtoASCII(this.kojenci_diag.Text.ToString());

        //this.saveData(Session["kojneci"].ToString(), defRes);
        SortedList data = new SortedList();

        string name = this.kojenci_diag.Text.ToString();
        name = name.Trim();
        name = x2_var.UTFtoASCII(name);

        if (name.Length > 0)
        {

            data.Add("name", name);
            data.Add("poznamka", this.kojenci_note.Text.ToString());
            DateTime datum = this.Calendar2.SelectedDate;
            data.Add("date", my_x2.unixDate(datum));
            data.Add("odd", "KOJ");

            SortedList result = x2db.mysql_insert("is_osirix", data);
            bool res = Convert.ToBoolean(result["status"]);
            int id = Convert.ToInt32(result["last_id"]);
            if (res)
            {
                if (id > 0)
                {
                    this.makeDynamicTable(result, "kojenci", "kojenci_tbl");
                    this.kojenci_diag.Text = "";
                    this.kojenci_note.Text = "";

                }
            }
            else
            {
                Response.Write("<script>alert('" + result["msg"].ToString() + "')</script>");
            }
        }
        else
        {
            this.alert("Meno musi byt vypisane !!!!!");
        }

        
       // this.saveData()
    }

    protected void dievcata_diag_btn_Click(object sender, EventArgs e)
    {
        //string tmp = x_db.getOsirixData(Session["kojneci"].ToString();

        // string defRes = x2_var.UTFtoASCII(this.kojenci_diag.Text.ToString());

        //this.saveData(Session["kojneci"].ToString(), defRes);
        SortedList data = new SortedList();

        string name = this.dievcata_diag.Text.ToString();
        name = name.Trim();
        name = x2_var.UTFtoASCII(name);

        if (name.Length > 0)
        {

            data.Add("name", name);
            data.Add("poznamka", this.dievcata_note.Text.ToString());
            DateTime datum = this.Calendar2.SelectedDate;
            data.Add("date", my_x2.unixDate(datum));
            data.Add("odd", "MSV");

            SortedList result = x2db.mysql_insert("is_osirix", data);
            bool res = Convert.ToBoolean(result["status"]);
            int id = Convert.ToInt32(result["last_id"]);
            if (res)
            {
                if (id > 0)
                {
                    this.makeDynamicTable(result, "dievcata", "dievcata_tbl");
                    this.dievcata_diag.Text = "";
                    this.dievcata_note.Text = "";
                }
            }
            else
            {
                Response.Write("<script>alert('" + result["msg"].ToString() + "')</script>");
            }
        }
        else
        {
            this.alert("Meno musi byt vypisane !!!!!");
        }


        // this.saveData()
    }


    protected void chlapci_diag_btn_Click(object sender, EventArgs e)
    {
        //string tmp = x_db.getOsirixData(Session["kojneci"].ToString();

        // string defRes = x2_var.UTFtoASCII(this.kojenci_diag.Text.ToString());

        //this.saveData(Session["kojneci"].ToString(), defRes);
        SortedList data = new SortedList();

        string name = this.chlapci_diag.Text.ToString();
        name = name.Trim();
        name = x2_var.UTFtoASCII(name);

        if (name.Length > 0)
        {

            data.Add("name", name);
            data.Add("poznamka", this.chlapci_note.Text.ToString());
            DateTime datum = this.Calendar2.SelectedDate;
            data.Add("date", my_x2.unixDate(datum));
            data.Add("odd", "VD");

            SortedList result = x2db.mysql_insert("is_osirix", data);
            bool res = Convert.ToBoolean(result["status"]);
            int id = Convert.ToInt32(result["last_id"]);
            if (res)
            {
                if (id > 0)
                {
                    this.makeDynamicTable(result, "chlapci", "chlapci_tbl");
                    this.chlapci_diag.Text = "";
                    this.chlapci_note.Text = "";

                }
            }
            else
            {
                Response.Write("<script>alert('" + result["msg"].ToString() + "')</script>");
            }
        }
        else
        {
            this.alert("Meno musi byt vypisane !!!!!");
        }

        // this.saveData()
    }

    protected void alert(string message)
    {
        Response.Write("<script>alert('" + message + "');</script>");
    }

    protected void search_fnc(object sender, EventArgs e)
    {
        string searchName = x2_var.UTFtoASCII(this.osirix_search_txt.Text.ToString().Trim());

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
