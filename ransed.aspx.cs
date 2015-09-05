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
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        this.msg_lbl.Text = "";
        this.loadDeps();
        this.setMyDate();
        this.loadData();

    }

    protected void loadDeps()
    {
        string query = my_x2.sprintf("SELECT * FROM [is_deps] WHERE [clinic_id]='{0}'", new string[] {Session["klinika_id"].ToString()});
        Dictionary<int,Hashtable> table = x2db.getTable(query);

        int tableCn = table.Count;

        if (tableCn > 0)
        {
            for (int i = 0; i < tableCn; i++)
            {
                this.odd_dl.Items.Add(new ListItem(table[i]["label"].ToString(), table[i]["idf"].ToString()));

            }
        }

    }

    protected void loadData()
    {
        if (!IsPostBack)
        {
            this.name_txt.Text = "";
            this.note_txt.Text = "";
        }

        

        //this.loadSluzby();
        this.loadSluzby();
        this.setDepsView();
        //this.loadKojenci();
        //this.loadDievcata();
        //this.loadChlapci();
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

        Table shiftTable = new Table();
        this.osirixData_plh.Controls.Add(shiftTable);

        TableHeaderRow headRow = new TableHeaderRow();
        shiftTable.Controls.Add(headRow);
        TableHeaderCell headCell = new TableHeaderCell();
        headCell.Text = "<h2>Sluzby</h2>";
        headRow.Controls.Add(headCell);

        sb.Append("SELECT [patient_name] AS [name], [work_text] AS [text], [work_place] AS [place] FROM [is_hlasko_epc] AS [hlasko_epc]");
        sb.AppendLine("INNER JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id] = [hlasko_epc].[hlasko_id] ");
        sb.AppendFormat("WHERE [hlasko].[dat_hlas]='{0}'", my_x2.unixDate(datum));
        sb.AppendFormat("AND [hlasko].[clinic]='{0}'", Session["klinika_id"]);

        Dictionary<int, Hashtable> data = x2db.getTable(sb.ToString());

        int dataCn = data.Count;
      //  this.msg_lbl
        for (int i = 0; i < dataCn; i++)
        {
            TableRow riadok = new TableRow();
            shiftTable.Controls.Add(riadok);
            TableCell dataCell = new TableCell();
            dataCell.HorizontalAlign = HorizontalAlign.Center;

            HyperLink osirixLn = new HyperLink();

            string[] tmpArr = data[i]["name"].ToString().Split(' ');
            osirixLn.Text = tmpArr[0];
            osirixLn.CssClass = "large green button align-center";
            osirixLn.NavigateUrl = Resources.Resource.osirix_url + x2_var.UTFtoASCII(data[i]["name"].ToString());
            dataCell.Controls.Add(osirixLn);
            Literal workText = new Literal();
            workText.Text = "<p><strong>Odd - (" + data[i]["place"].ToString() + ")</strong>, " + my_x2.DecryptString(data[i]["text"].ToString(), Session["passphrase"].ToString()) + "</p>";
            dataCell.Controls.Add(workText);




            riadok.Controls.Add(dataCell);
        }
    }

    protected void setDepsView()
    {
        //this.depsRdg_tbl.Controls.Clear();

        string sql = my_x2.sprintf("SELECT [idf],[label] FROM [is_deps] WHERE [clinic_id]='{0}'", new string[] { Session["klinika_id"].ToString() });
        Dictionary<int, Hashtable> data = x2db.getTable(sql);

        int dataCn = data.Count;


        for (int i = 0; i < dataCn; i++)
        {
            Table rdgTable = new Table();
            rdgTable.ID = "rdgTable_" + data[i]["idf"];
            this.osirixData_plh.Controls.Add(rdgTable);

            TableHeaderRow headRow = new TableHeaderRow();
            rdgTable.Controls.Add(headRow);
            
            TableHeaderCell headCell = new TableHeaderCell();
            headCell.Text = "<h2>" + data[i]["label"].ToString() + "</h2>";
            headRow.Controls.Add(headCell);

            this.loadOsirixData(data[i]["idf"].ToString(), rdgTable);

        }
    }

    protected void loadOsirixData(string dep, Table rdgTable)
    {
        DateTime datum = this.Calendar1.SelectedDate;

        string query = my_x2.sprintf("SELECT * FROM [is_osirix] WHERE [date]='{0}' AND [odd]='{1}' AND [clinic]='{2}'", new string[] { my_x2.unixDate(datum), dep, Session["klinika_id"].ToString() });

        Dictionary<int, Hashtable> data = x2db.getTable(query);

        int dataCn = data.Count;

        for (int i = 0; i < dataCn; i++)
        {
            TableRow newOsirRow = new TableRow();
            newOsirRow.ID = "osirixRow_" + dep + "_" + data[i]["item_id"].ToString();

            rdgTable.Controls.Add(newOsirRow);

            TableCell dataCell = new TableCell();
            dataCell.ID = "osirixCell_" + data[i]["odd"].ToString() + "_" + data[i]["item_id"].ToString();

            HyperLink osirixLink = new HyperLink();
            osirixLink.Text = data[i]["name"].ToString();
            osirixLink.CssClass = "button blue large";
            osirixLink.NavigateUrl = Resources.Resource.osirix_url + x2_var.UTFtoASCII(data[i]["name"].ToString());
            osirixLink.Target = "_blank";
            dataCell.Controls.Add(osirixLink);

            Label osirixComment = new Label();
            osirixComment.Text = "&nbsp;&nbsp;&nbsp;" + data[i]["poznamka"].ToString() + "&nbsp;&nbsp;&nbsp;";
            dataCell.Controls.Add(osirixComment);

            Button delLink = new Button();
            delLink.ID = "del_" + dep + "_" + data[i]["item_id"].ToString();
            //delLink.Click += new EventHandler(deleteOsirEntry);
            delLink.Text = Resources.Resource.delete;
            delLink.CssClass = "red button";

            dataCell.Controls.Add(delLink);

            newOsirRow.Controls.Add(dataCell);
        }


    }

    protected void add_patient_click_fnc(object sender, EventArgs e)
    {

        string name = this.name_txt.Text.ToString();
        string note = this.note_txt.Text.ToString();

        this.name_txt.Text = "";
        this.note_txt.Text = "";

        string dep = this.odd_dl.SelectedValue.ToString();

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        Table rdgTable = (Table)ctpl.FindControl("rdgTable_" + dep);

        TableRow riadok = new TableRow();
        rdgTable.Controls.Add(riadok);

        TableCell dataCell = new TableCell();

        HyperLink osirixLn = new HyperLink();
        osirixLn.CssClass = "button large blue";
        osirixLn.NavigateUrl = Resources.Resource.osirix_url + name;
        osirixLn.Text = name;
        dataCell.Controls.Add(osirixLn);

        riadok.Controls.Add(dataCell);

        Label noteLbl = new Label();
        noteLbl.Text = note;
        dataCell.Controls.Add(noteLbl);

        riadok.Controls.Add(dataCell);

        //this.saveToOsirix(name, note,dep);


    }

    protected void saveToOsirix(string name, string note,string dep)
    {
        SortedList data = new SortedList();
        data.Add("name", name.Trim());
        data.Add("note", note.Trim());
        data.Add("clinic", Session["klinika_id"]);
        data.Add("odd", dep.Trim());
    }
    
    
}
