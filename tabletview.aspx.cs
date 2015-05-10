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
        string gKlinika = ""; 

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        this.gKlinika = Session["klinika"].ToString().ToLower();

       /* this.kojenci_diag_btn.Enabled = false;
        this.OddB_diag_btn.Enabled = false;
        this.Pohotovost_diag_btn.Enabled = false;*/

        if (!IsPostBack)
        {
          this.setMyDate();
          this.setDepsView();
          this.loadShifts();
          //this.loadPostData();
          //  this.loadData();
        }
        else
        {
            this.setMyDate();
            this.setDepsView();
            this.loadShifts();
            //this.loadPostData();
            
          // this.setData();
        }

    }

    protected void loadShifts()
    {
        if (this.gKlinika=="2dk")
        {
            this.loadDKOsirixData();
        }
        if (this.gKlinika == "kdch")
        {
            this.loadSluzbyKDCH();
        }
        
    }

    protected void loadDKOsirixData()
    {
        this.osirixShift_tbl.Controls.Clear();
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [patient_name] AS [name] FROM [is_hlasko_epc] AS [hlasko_epc]");
        sb.AppendLine("INNER JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id] = [hlasko_epc].[hlasko_id] ");
        sb.AppendFormat("WHERE [hlasko].[dat_hlas]='{0}'",my_x2.unixDate(this.Calendar1.SelectedDate));
        sb.AppendFormat("AND [hlasko].[clinic]='{0}'", Session["klinika_id"]);

        Dictionary<int, Hashtable> data = x2db.getTable(sb.ToString());

        int dataCn = data.Count;

        for (int i=0;i<dataCn;i++)
        {
            TableRow riadok = new TableRow();
            this.osirixShift_tbl.Controls.Add(riadok);
            TableCell dataCell = new TableCell();
            dataCell.HorizontalAlign = HorizontalAlign.Center;

            HyperLink osirixLn = new HyperLink();
            osirixLn.Text = data[i]["name"].ToString();
            osirixLn.CssClass = "large green button align-center";
            osirixLn.NavigateUrl = Resources.Resource.osirix_url + data[i]["name"];
           
            dataCell.Controls.Add(osirixLn);

            riadok.Controls.Add(dataCell);
        }


       // string query = my_x2.sprintf(" WHERE [osirix]=true AND ");

    }

    protected void loadPostData()
    {
        //this.loadSluzby();

        //this.loadKojenciData();
        //this.loadDievcataData();
        //this.loadChlapciData();
    }

    protected void setDepsView()
    {
        //this.depsRdg_tbl.Controls.Clear();

        string sql = my_x2.sprintf("SELECT [idf],[label] FROM [is_deps] WHERE [clinic_id]='{0}'", new string[] {Session["klinika_id"].ToString()});
        Dictionary<int, Hashtable> data = x2db.getTable(sql);

        int dataCn = data.Count;

       
        for (int i=0; i<dataCn; i++)
        {
            Table rdgTable = new Table();
            rdgTable.ID = "rdgTable_" + data[i]["idf"];

            TableRow riadok = new TableRow();

            TableCell cellDep = new TableCell();
            riadok.Controls.Add(cellDep);
            cellDep.ID = "adding_"+data[i]["idf"].ToString();

            Label title = new Label();
            title.Text = "<h1>"+data[i]["label"].ToString()+"</h1>";
            cellDep.Controls.Add(title);
            rdgTable.Controls.Add(riadok);

            TableRow riadokData = new TableRow();

            TableCell cellData = new TableCell();

            Label name_lbl = new Label();
            name_lbl.Text = "Meno:";
            cellData.Controls.Add(name_lbl);

            TextBox name_txt = new TextBox();
            name_txt.ID = "name_"+data[i]["idf"].ToString();
            cellData.Controls.Add(name_txt);

            Label note_lbl = new Label();
            note_lbl.Text = "Poznamka:";
            cellData.Controls.Add(note_lbl);

            TextBox note_txt = new TextBox();
            note_txt.ID = "note_" + data[i]["idf"].ToString(); ;
            cellData.Controls.Add(note_txt);

            Button add_btn = new Button();
            add_btn.Text = "Pridaj";
            add_btn.ID = "addBtn_" + data[i]["idf"].ToString();
            add_btn.Click += new EventHandler(addPatientFnc);
            cellData.Controls.Add(add_btn);

            riadokData.Controls.Add(cellData);

            rdgTable.Controls.Add(riadokData);
            this.loadOsirixData(data[i]["idf"].ToString(),rdgTable);

            this.rdgDg_pl.Controls.Add(rdgTable);

        }
    }

    protected void loadOsirixData(string dep,Table rdgTable)
    {
        DateTime datum = this.Calendar2.SelectedDate;

        string query = my_x2.sprintf("SELECT * FROM [is_osirix] WHERE [date]='{0}' AND [odd]='{1}' AND [clinic]='{2}'", new string[] { my_x2.unixDate(datum), dep, Session["klinika_id"].ToString() });

        Dictionary<int,Hashtable> data = x2db.getTable(query);

        int dataCn = data.Count;
       
        for (int i=0; i<dataCn; i++)
        {
            TableRow newOsirRow = new TableRow();
            newOsirRow.ID = "osirixRow_" + dep + "_" + data[i]["item_id"].ToString(); 

            rdgTable.Controls.Add(newOsirRow);

            TableCell dataCell = new TableCell();
            dataCell.ID = "osirixCell_" + data[i]["odd"].ToString() + "_" + data[i]["item_id"].ToString();

            HyperLink osirixLink = new HyperLink();
            osirixLink.Text = data[i]["name"].ToString();
            osirixLink.CssClass = "button blue large";
            osirixLink.NavigateUrl = Resources.Resource.osirix_url + data[i]["name"].ToString();
            osirixLink.Target = "_blank";
            dataCell.Controls.Add(osirixLink);

            Label osirixComment = new Label();
            osirixComment.Text = "&nbsp;&nbsp;&nbsp;" + data[i]["poznamka"].ToString() + "&nbsp;&nbsp;&nbsp;";
            dataCell.Controls.Add(osirixComment);

            Button delLink = new Button();
            delLink.ID = "del_" +dep+"_"+ data[i]["item_id"].ToString();
            delLink.Click += new EventHandler(deleteOsirEntry);
            delLink.Text = Resources.Resource.delete;
            delLink.CssClass = "red button";

            dataCell.Controls.Add(delLink);

            newOsirRow.Controls.Add(dataCell);
        }


    }

    protected void addPatientFnc(object sender, EventArgs e)
    {
        Button addBtn = (Button)sender;
        string[] idArr = addBtn.ID.ToString().Split('_');

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        

        TextBox name_txt = (TextBox)ctpl.FindControl("name_" + idArr[1]);
        

        //this.msg_lbl.Text = name_txt.Text.ToString();

        TextBox note_txt = (TextBox)ctpl.FindControl("note_" + idArr[1]);

        SortedList data = new SortedList();

        data.Add("name", name_txt.Text);
        data.Add("poznamka", note_txt.Text);
        data.Add("odd", idArr[1]);
        data.Add("clinic", Session["klinika_id"]);

        DateTime datum = this.Calendar2.SelectedDate;
        data.Add("date", my_x2.unixDate(datum));

        SortedList res = x2db.mysql_insert("is_osirix",data);

        if (Convert.ToBoolean(res["status"]))
        {
            Table rdgTable = (Table)ctpl.FindControl("rdgTable_" + idArr[1]);

            TableRow newOsirRow = new TableRow();
            newOsirRow.ID = "osirixRow_" + idArr[1] + "_" + res["last_id"].ToString();
            rdgTable.Controls.Add(newOsirRow);

            TableCell dataCell = new TableCell();
            dataCell.ID = "osirixCell_" + idArr[1] + "_" + res["last_id"].ToString();

            HyperLink osirixLink = new HyperLink();
            osirixLink.Text = name_txt.Text.ToString();
            osirixLink.CssClass = "large button blue";
            osirixLink.NavigateUrl = Resources.Resource.osirix_url + name_txt.Text.ToString();
            osirixLink.Target = "_blank";
            dataCell.Controls.Add(osirixLink);

            Label osirixComment = new Label();
            osirixComment.Text = "&nbsp;&nbsp;&nbsp;" + note_txt.Text.ToString() + "&nbsp;&nbsp;&nbsp;";
            dataCell.Controls.Add(osirixComment);

            Button delLink = new Button();
            delLink.ID = "del_" + idArr[1]+"_"+res["last_id"].ToString();
            
            delLink.Text = Resources.Resource.delete;
            delLink.Click += new EventHandler(deleteOsirEntry);
            delLink.CssClass = "red button";

            dataCell.Controls.Add(delLink);
            
            newOsirRow.Controls.Add(dataCell);
            name_txt.Text = "";
            note_txt.Text = "";

        }
        else
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }
    }
    
   protected void deleteOsirEntry(object sender, EventArgs e)
    {
       Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
       ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
       Button delBtn = (Button)sender;
       string[] idArr = delBtn.ID.ToString().Split('_');

       string query = my_x2.sprintf("DELETE FROM [is_osirix] WHERE [item_id] = {0}", new string[] { idArr[2] });

       SortedList res = x2db.execute(query);

       if (Convert.ToBoolean(res["status"]))
       {
           Table rdgTable = (Table)ctpl.FindControl("rdgTable_" + idArr[1]);
           TableRow osirRow = (TableRow)ctpl.FindControl("osirixRow_"+idArr[1]+"_"+idArr[2]);
           rdgTable.Controls.Remove(osirRow);
       }
       else
       {
           this.msg_lbl.Text = res["msg"].ToString();
       }

       
    }


    protected void loadSluzbyKDCH()
    {
        this.osirixShift_tbl.Controls.Clear();

        DateTime datum = this.Calendar1.SelectedDate;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT GROUP_CONCAT([osirix] SEPARATOR ' ') AS [osirix] FROM [is_hlasko] WHERE [dat_hlas] = '{0}'", my_x2.unixDate(datum));
        SortedList result = x2db.getRow(sb.ToString());

        string osirix = my_x2.getStr(result["osirix"].ToString());

        string tmp = osirix.Replace((char)13,' ');

        string[] str = tmp.Split(' ');

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Trim().Length > 0)
            {
                TableRow riadok = new TableRow();
                this.osirixShift_tbl.Controls.Add(riadok);
                TableCell dataCell = new TableCell();
                dataCell.HorizontalAlign = HorizontalAlign.Center;

                HyperLink meno_lnk = new HyperLink();
                meno_lnk.Text = str[i].ToUpper();
                meno_lnk.NavigateUrl = Resources.Resource.osirix_url + str[i];
                meno_lnk.Target = "_blank";
                meno_lnk.CssClass = "large button green align-center";

                dataCell.Controls.Add(meno_lnk);
                riadok.Controls.Add(dataCell);
            }
        }

    }


    //protected void loadKojenciData()
    //{
    //    this.kojenci_tbl.Controls.Clear();

    //    StringBuilder sb = new StringBuilder();
    //    DateTime datum = this.Calendar2.SelectedDate;

    //    sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "KOJ");

    //    Dictionary<int, SortedList> result = x2db.getTableSL(sb.ToString());

    //    int rows = result.Count;

    //    if (rows > 0)
    //    {
    //        for (int i = 0; i < rows; i++)
    //        {
    //            this.makeDynamicTable(result[i], "kojenci", "kojenci_tbl");

    //        }
    //    }
    //}

    //protected void loadChlapciData()
    //{
    //    this.chlapci_tbl.Controls.Clear();

    //    StringBuilder sb = new StringBuilder();
    //    DateTime datum = this.Calendar2.SelectedDate;

    //    sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "VD");

    //    Dictionary<int, SortedList> result = x2db.getTableSL(sb.ToString());

    //    int rows = result.Count;

    //    if (rows > 0)
    //    {
    //        for (int i = 0; i < rows; i++)
    //        {
    //            this.makeDynamicTable(result[i], "chlapci", "chlapci_tbl");

    //        }
    //    }
    //}

    //protected void loadDievcataData()
    //{
    //    this.dievcata_tbl.Controls.Clear();

    //    StringBuilder sb = new StringBuilder();
    //    DateTime datum = this.Calendar2.SelectedDate;

    //    sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "MSV");

    //    Dictionary<int, SortedList> result = x2db.getTableSL(sb.ToString());

    //    int rows = result.Count;

    //    if (rows > 0)
    //    {
    //        for (int i = 0; i < rows; i++)
    //        {
    //            this.makeDynamicTable(result[i], "dievcata", "dievcata_tbl");

    //        }
    //    }
    //}



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

   

    //public string makeLink(string str)
    //{
    //    string result = "";

    //    string Odd = x2_var.UTFtoASCII(str);
    //    // string def = "";
    //    string[] tmp = this.returnStrArray(Odd);
    //    int cnt = tmp.Length;

    //    for (int i = 0; i < cnt; i++)
    //    {
    //        result += "<p><a href='http://10.10.2.49:3333/studyList?search=" + tmp[i] + "' target='_blank' style='font-size:xx-large;font-weight:bolder;'>" + tmp[i].ToUpper() + "</a></p>";
    //    }

    //    return result;
    //}

    //public string[] returnStrArray(string str)
    //{
    //    string[] result = Regex.Split(str, "\r\n");
    //    return result;
    //}

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        //this.loadSluzby();
        this.loadShifts();
    }

    protected void Calendar2_SelectionChanged(object sender, EventArgs e)
    {
        this.setDepsView();
        //this.loadKojenciData();
        //this.loadDievcataData();
        //this.loadChlapciData();
    }

    //protected void saveData(string id, string text)
    //{
    //    SortedList data = new SortedList();
    //    data["osirix"] = x2_var.UTFtoASCII(text);
    //    string res = x_db.update_row("is_hlasko", data, id);
    //    if (res != "ok")
    //    {
    //        Label1.Text = res;
    //    }
    //    else
    //    {
    //        Label1.Text = res;
    //    }
    //}

    //protected void makeDynamicTable(SortedList data,string prefix, string _tabulka)
    //{
    //    ContentPlaceHolder ctpl = new ContentPlaceHolder();
    //    Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

    //    ctpl = (ContentPlaceHolder)tmpControl;

    //    string id = "";
    //    if (data["item_id"] == null)
    //    {
    //        id = data["last_id"].ToString();
    //    }
    //    else
    //    {
    //        id = data["item_id"].ToString();
    //    }
    //    //row_tbl.ID = prefix+"_row_" + id.ToString();

    //    Control tbl = ctpl.FindControl(_tabulka);
    //    Table tabulka = (Table)tbl;

    //    TableRow row_tbl = new TableRow();
        
    //    tabulka.Controls.Add(row_tbl);



    //    TableCell cell_tbl = new TableCell();
        
    //    //cell_tbl.ID = prefix+"_cell_" + id.ToString();


    //    HyperLink meno_lnk = new HyperLink();
    //    meno_lnk.ID = prefix+"_meno_" + id.ToString();
    //    meno_lnk.CssClass = "large button blue";
    //    if (data["name"] != null)
    //    {
    //        meno_lnk.Text = data["name"].ToString(); 
            
    //        meno_lnk.NavigateUrl = "http://10.10.2.49:3333/studyList?search=" + data["name"].ToString(); 
    //    }
    //    else
    //    {
    //        Control txt = ctpl.FindControl(prefix + "_diag");
    //        TextBox text_tmp = (TextBox)txt;
    //        meno_lnk.Text = text_tmp.Text.ToString();
    //        meno_lnk.NavigateUrl = "http://10.10.2.49:3333/studyList?search=" + meno_lnk.Text.ToString();
    //    }

        
    //    meno_lnk.Target = "_blank";
    //    cell_tbl.Controls.Add(meno_lnk);

    //    Label note_lbl = new Label();
    //    note_lbl.ID = prefix + "_lbl_" + id;
    //    note_lbl.CssClass = "align-left";
    //    if (data["poznamka"] != null)
    //    {
    //        note_lbl.Text = data["poznamka"].ToString();
    //    }
    //    else
    //    {
    //        Control txt1 = ctpl.FindControl(prefix + "_note");
    //        TextBox text_tmp1 = (TextBox)txt1;
    //        note_lbl.Text =text_tmp1.Text.ToString() ;
    //    }
    //    cell_tbl.Controls.Add(note_lbl);



        

    //    Button delete_btn = new Button();
    //    delete_btn.ID = prefix+"_delete_" + id.ToString();
    //    delete_btn.Text = "Zmaz";
    //    delete_btn.CssClass = "medium button red pull-right";
    //    delete_btn.Click += new EventHandler(delete_btn_fnc);

    //    cell_tbl.Controls.Add(delete_btn);

    //    row_tbl.Controls.Add(cell_tbl);
    //}

    //protected void delete_btn_fnc(object sender, EventArgs e)
    //{
    //    Button btn = (Button)sender;
    //    string[] str = btn.ID.ToString().Split('_');

    //    SortedList res = x2db.execute("DELETE FROM [is_osirix] WHERE [item_id]=" + str[2]);

    //    Boolean result = Convert.ToBoolean(res["status"]);
    //    if (!result)
    //    {
    //        Label1.Text = res["msg"].ToString();
    //    }
    //    else
    //    {
    //        this.loadPostData();
    //    }
        
     
       
    //}

    //protected void kojenci_diag_btn_Click(object sender, EventArgs e)
    //{
    //    //string tmp = x_db.getOsirixData(Session["kojneci"].ToString();

    //   // string defRes = x2_var.UTFtoASCII(this.kojenci_diag.Text.ToString());

    //    //this.saveData(Session["kojneci"].ToString(), defRes);
    //    SortedList data = new SortedList();

    //    string name = this.kojenci_diag.Text.ToString();
    //    name = name.Trim();
    //    name = x2_var.UTFtoASCII(name);

    //    if (name.Length > 0)
    //    {

    //        data.Add("name", name);
    //        data.Add("poznamka", this.kojenci_note.Text.ToString());
    //        DateTime datum = this.Calendar2.SelectedDate;
    //        data.Add("date", my_x2.unixDate(datum));
    //        data.Add("odd", "KOJ");

    //        SortedList result = x2db.mysql_insert("is_osirix", data);
           
    //        bool res = Convert.ToBoolean(result["status"]);
    //        int id = Convert.ToInt32(result["last_id"]);
    //        if (res)
    //        {
    //            if (id > 0)
    //            {
    //                this.makeDynamicTable(result, "kojenci", "kojenci_tbl");
    //                this.kojenci_diag.Text = "";
    //                this.kojenci_note.Text = "";

    //            }
    //        }
    //        else
    //        {
    //            Response.Write("<script>alert('" + result["msg"].ToString() + "')</script>");
    //        }
    //    }
    //    else
    //    {
    //        this.alert("Meno musi byt vypisane !!!!!");
    //    }

        
    //   // this.saveData()
    //}

    //protected void dievcata_diag_btn_Click(object sender, EventArgs e)
    //{
    //    //string tmp = x_db.getOsirixData(Session["kojneci"].ToString();

    //    // string defRes = x2_var.UTFtoASCII(this.kojenci_diag.Text.ToString());

    //    //this.saveData(Session["kojneci"].ToString(), defRes);
    //    SortedList data = new SortedList();

    //    string name = this.dievcata_diag.Text.ToString();
    //    name = name.Trim();
    //    name = x2_var.UTFtoASCII(name);

    //    if (name.Length > 0)
    //    {

    //        data.Add("name", name);
    //        data.Add("poznamka", this.dievcata_note.Text.ToString());
    //        DateTime datum = this.Calendar2.SelectedDate;
    //        data.Add("date", my_x2.unixDate(datum));
    //        data.Add("odd", "MSV");

    //        SortedList result = x2db.mysql_insert("is_osirix", data);
    //        bool res = Convert.ToBoolean(result["status"]);
    //        int id = Convert.ToInt32(result["last_id"]);
    //        if (res)
    //        {
    //            if (id > 0)
    //            {
    //                this.makeDynamicTable(result, "dievcata", "dievcata_tbl");
    //                this.dievcata_diag.Text = "";
    //                this.dievcata_note.Text = "";
    //            }
    //        }
    //        else
    //        {
    //            Response.Write("<script>alert('" + result["msg"].ToString() + "')</script>");
    //        }
    //    }
    //    else
    //    {
    //        this.alert("Meno musi byt vypisane !!!!!");
    //    }


    //    // this.saveData()
    //}


    //protected void chlapci_diag_btn_Click(object sender, EventArgs e)
    //{
    //    //string tmp = x_db.getOsirixData(Session["kojneci"].ToString();

    //    // string defRes = x2_var.UTFtoASCII(this.kojenci_diag.Text.ToString());

    //    //this.saveData(Session["kojneci"].ToString(), defRes);
    //    SortedList data = new SortedList();

    //    string name = this.chlapci_diag.Text.ToString();
    //    name = name.Trim();
    //    name = x2_var.UTFtoASCII(name);

    //    if (name.Length > 0)
    //    {

    //        data.Add("name", name);
    //        data.Add("poznamka", this.chlapci_note.Text.ToString());
    //        DateTime datum = this.Calendar2.SelectedDate;
    //        data.Add("date", my_x2.unixDate(datum));
    //        data.Add("odd", "VD");

    //        SortedList result = x2db.mysql_insert("is_osirix", data);
    //        bool res = Convert.ToBoolean(result["status"]);
    //        int id = Convert.ToInt32(result["last_id"]);
    //        if (res)
    //        {
    //            if (id > 0)
    //            {
    //                this.makeDynamicTable(result, "chlapci", "chlapci_tbl");
    //                this.chlapci_diag.Text = "";
    //                this.chlapci_note.Text = "";

    //            }
    //        }
    //        else
    //        {
    //            Response.Write("<script>alert('" + result["msg"].ToString() + "')</script>");
    //        }
    //    }
    //    else
    //    {
    //        this.alert("Meno musi byt vypisane !!!!!");
    //    }

    //    // this.saveData()
    //}

    //protected void alert(string message)
    //{
    //    Response.Write("<script>alert('" + message + "');</script>");
    //}

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
