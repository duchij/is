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
        sb.Append("SELECT [patient_name] AS [name], [work_place] AS [place], [work_text] AS [text] FROM [is_hlasko_epc] AS [hlasko_epc]");
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
            osirixLn.NavigateUrl = Resources.Resource.osirix_url + x2_var.UTFtoASCII(data[i]["name"].ToString());
           
            dataCell.Controls.Add(osirixLn);

            Literal workText = new Literal();
            workText.Text = "<p><strong>Odd - (" + data[i]["place"].ToString() + ")</strong>, " + my_x2.DecryptString(data[i]["text"].ToString(), Session["passphrase"].ToString()) + "</p>";
            dataCell.Controls.Add(workText);

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
            osirixComment.Text = "&nbsp;&nbsp;&nbsp;" + my_x2.getStr(data[i]["poznamka"].ToString()) + "&nbsp;&nbsp;&nbsp;";
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
            osirixLink.NavigateUrl = Resources.Resource.osirix_url + x2_var.UTFtoASCII(name_txt.Text.ToString());
            osirixLink.Target = "_blank";
            dataCell.Controls.Add(osirixLink);

            Label osirixComment = new Label();
            osirixComment.Text = "&nbsp;&nbsp;&nbsp;" + my_x2.getStr(note_txt.Text.ToString()) + "&nbsp;&nbsp;&nbsp;";
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
        sb.Append("SELECT [patient_name] AS [name], [work_text] AS [text], [work_place] AS [place] FROM [is_hlasko_epc] AS [hlasko_epc]");
        //sb.Append("");
        sb.AppendLine("INNER JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id] = [hlasko_epc].[hlasko_id] ");
        sb.AppendFormat("WHERE [hlasko].[dat_hlas]='{0}'", my_x2.unixDate(datum));
        sb.AppendFormat("AND [hlasko].[clinic]='{0}' AND [hlasko_epc.osirix]='true'", Session["klinika_id"]);

        Dictionary<int, Hashtable> data = x2db.getTable(sb.ToString());

        int dataLn = data.Count;

        for (int i = 0; i < dataLn; i++)
        {
          
                TableRow riadok = new TableRow();
                this.osirixShift_tbl.Controls.Add(riadok);
                TableCell dataCell = new TableCell();
                dataCell.HorizontalAlign = HorizontalAlign.Center;

                HyperLink meno_lnk = new HyperLink();
                string[] tmpArr = data[i]["name"].ToString().Split(' ');


                meno_lnk.Text = tmpArr[0].ToUpper();
                meno_lnk.NavigateUrl = Resources.Resource.osirix_url +x2_var.UTFtoASCII(tmpArr[0]);
                meno_lnk.Target = "_blank";
                meno_lnk.CssClass = "large button green align-center";
                dataCell.Controls.Add(meno_lnk);

                Literal workText = new Literal();
                workText.Text = "<p><strong>Odd - (" + data[i]["place"].ToString() + ")</strong>, " + my_x2.DecryptString(data[i]["text"].ToString(), Session["passphrase"].ToString()) + "</p>";
                dataCell.Controls.Add(workText);

                
                riadok.Controls.Add(dataCell);
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

    

    protected void search_fnc(object sender, EventArgs e)
    {
        string searchName = x2_var.UTFtoASCII(this.osirix_search_txt.Text.ToString().Trim());

        if (searchName.Length > 0)
        {
            Response.Redirect("http://10.10.2.49:3333/studyList?search=" + searchName);
            //Response.Write("<script>window.open('http://10.10.2.49:3333/studyList?search=" + searchName + "','_blank','');</script>");
        }
        else
        {
            Response.Write("<script>alert('Prazdny retazec....')</script>");
        }
    }

}
