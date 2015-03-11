using System;
using System.Text;
using System.Globalization;
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

public partial class staze : System.Web.UI.Page
{
    x2_var x2 = new x2_var();
    mysql_db x2Mysql = new mysql_db();
    log x2log = new log();
    //string tabulka = "";
    public string rights = "";
    public string wgroup = "";
    public string gKlinika;

    protected void Page_Init(object sender, EventArgs e)
    {

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

       

        this.msg_lbl.Text = "";
        this.rights = Session["rights"].ToString();
        this.wgroup = Session["workgroup"].ToString();
        this.gKlinika = Session["klinika"].ToString().ToLower();
        this.setLabels();

        if (this.rights == "poweruser" || this.rights=="admin" || this.rights=="sadmin")
        {
            this.edit_box_pl.Visible = true;
            this.setState_pl.Visible = true;
        }
        else
        {
            this.edit_box_pl.Visible = false;
        }

        

        
        if (IsPostBack == false)
        {
            this.setActDate();

            int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
            int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

            string state = this.getState(x2.makeDateGroup(rok, mesiac));

            if (this.rights.IndexOf("admin") != -1 || this.rights=="poweruser")
            {
                this.drawTable();
            }
            else if (this.rights == "users" && state=="yes")
            {
                this.drawTable();
            }
            else if (this.rights == "users" && state == "no")
            {
                this.word_btn.Visible = false;
                this.print_btn.Visible = false;
                this.msg_lbl.Text = Resources.Resource.staze_not_done;
            }

            
        }
        else
        {
            int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
            int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

            string state = this.getState(x2.makeDateGroup(rok, mesiac));

            if (this.rights.IndexOf("admin")!=-1 || this.rights == "poweruser" || Session["login"].ToString() == "jbabala" )
            {
                this.word_btn.Visible = true;
                this.print_btn.Visible = true;
                this.drawTable();
            }
            else if (this.rights == "users" && state == "yes")
            {
                this.word_btn.Visible = true;
                this.print_btn.Visible = true;
                this.drawTable();
            }
            else if (this.rights == "users" && state == "no")
            {
                this.stazeTable_tbl.Controls.Clear();
                this.word_btn.Visible = false;
                this.print_btn.Visible = false;
                this.msg_lbl.Text = Resources.Resource.staze_not_done;
            }
        }
    }

    protected void setLabels()
    {
        this.staze_titel_lbl.Text = x2.setLabel(this.gKlinika + "_staze_titel");
    }

    protected void setActDate()
    {
        DateTime dt = DateTime.Today;

        this.mesiac_cb.SelectedValue = dt.Month.ToString();
        this.rok_cb.SelectedValue = dt.Year.ToString();

    }

    protected void changeStazeFnc(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        string state = this.getState(x2.makeDateGroup(rok, mesiac));
        if (state == "yes")
        {
            this.drawTable();
            this.loadStaze();
        }
        
    }

    protected string getState(int dateGrp)
    {
        string result="no";
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("SELECT [public] FROM [is_staze_2] WHERE [date_group]='{0}' GROUP BY [public]", dateGrp);

        SortedList res = x2Mysql.getRow(sb.ToString());
        if (res.Count > 0)
        {
            if (res["status"] == null)
            {
                result = res["public"].ToString();
            }
            else
            {
                x2log.logData(res, "error", "error in getting state of staze");

            }
        }
        else
        {
            this.stazeTable_tbl.Controls.Clear();
            this.msg_lbl.Text = Resources.Resource.staze_not_done;
        }

        

        return result;

    }


    protected void drawTable()
    {
        this.stazeTable_tbl.Controls.Clear();

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);



        int daysInMonth = DateTime.DaysInMonth(rok, mesiac);

        SortedList res = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name] = '"+this.gKlinika+"_staze'");

        string[] staze = res["data"].ToString().Split(',');

        int colsNum = staze.Length;
        string[] header = staze;

       // ArrayList doctors = this.loadOmegaDoctors();

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell headCell = new TableHeaderCell();
        headCell.ID = "headCell_date";
        headCell.Text = "<strong>Datum</strong>";
        // headCell.Style
        headRow.Controls.Add(headCell);

        for (int head = 0; head < colsNum; head++)
        {
            TableHeaderCell headCell1 = new TableHeaderCell();
            headCell1.ID = "headCell_" + head;
            headCell1.Text = "<strong><center>" + x2.setLabel(header[head].ToString()) + "</center></strong>";
            // headCell1.
            headRow.Controls.Add(headCell1);
        }
        this.stazeTable_tbl.Controls.Add(headRow);
        string[] freeDays = Session["freedays"].ToString().Split(',');

        for (int row=0;row<daysInMonth; row++)
        {
            TableRow riadok = new TableRow();
            this.stazeTable_tbl.Controls.Add(riadok);
            int rDen = row + 1;

            DateTime dt = new DateTime(rok, mesiac, rDen);
            Boolean vikend = false;
            int dW = (int)dt.DayOfWeek;

            if (dW == 6 || dW == 0) vikend = true;
            int sviatok = Array.IndexOf(freeDays, rDen.ToString() + "." + mesiac.ToString());


            for (int col=0; col<=colsNum; col++)
            {
                
                if (col==0)
                {

                    TableCell dateCell = new TableCell(); 
                    dateCell.Text = dt.ToShortDateString();
                    if (vikend) dateCell.CssClass = "box red";
                    if (sviatok != -1) dateCell.CssClass = "box yellow";
                   riadok.Controls.Add(dateCell);
                }
                else
                {
                    TableCell dataCell = new TableCell();
                    if (vikend) dataCell.CssClass = "box red";
                    if (sviatok != -1) dataCell.CssClass = "box yellow";
                    if ((this.rights.IndexOf("admin")!=-1 || this.rights=="poweruser" || Session["login"].ToString() == "jbabala") && this.edit_chk.Checked)
                    {
                        TextBox txtBox = new TextBox();
                        txtBox.ID = header[col-1] + "_" + rDen.ToString();
                        txtBox.TextMode = TextBoxMode.MultiLine;
                        txtBox.AutoPostBack = true;
                        txtBox.TextChanged += new EventHandler(changedText);
                        txtBox.Wrap = false;
                        txtBox.Rows = 2;
                        dataCell.Controls.Add(txtBox);
                    }
                    else
                    {
                        Label textLbl = new Label();
                        textLbl.ID = header[col-1] + "_" + rDen.ToString();
                        dataCell.Controls.Add(textLbl);
                    }
                    riadok.Controls.Add(dataCell);
                }
            }
        }
        this.loadStaze();
    }

    protected void loadStaze()
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        int dateGroup = x2.makeDateGroup(rok, mesiac);
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [type],[datum],[text] FROM [is_staze_2] WHERE [date_group]='{0}' AND [clinic]='{1}' ORDER BY [datum]", dateGroup,Session["klinika_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableCn = table.Count;

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        for (int row=0; row<tableCn; row++)
        {
            DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["datum"].ToString()));

            int day = dt.Day;

            string type = table[row]["type"].ToString();

            if ((this.rights.IndexOf("admin") != -1 || this.rights == "poweruser" || Session["login"].ToString() == "jbabala") && this.edit_chk.Checked)
            {
                Control cl = ctpl.FindControl(type + "_" + day.ToString());

                TextBox txtBox = (TextBox)cl;

                txtBox.Text = table[row]["text"].ToString();


            }
            else
            {
                Control cl = ctpl.FindControl(type + "_" + day.ToString());

                Label txtBox = (Label)cl;

                txtBox.Text = table[row]["text"].ToString() + "<br>"; 
            }


        }
    }


    protected void changedText(object sender, EventArgs e)
    {
        TextBox txtBox = (TextBox)sender;
        string[] tId = txtBox.ID.ToString().Split('_');

        SortedList data = new SortedList();
        data.Add("text", txtBox.Text.ToString());
        data.Add("type", tId[0]);

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        int den = Convert.ToInt32(tId[1]);

        int dateGroup = x2.makeDateGroup(rok, mesiac);
        data.Add("date_group", dateGroup);
        data.Add("datum", rok.ToString() + "-" + mesiac.ToString() + "-" + den.ToString());
        data.Add("clinic", Session["klinika_id"]);

        SortedList res = x2Mysql.mysql_insert("is_staze_2", data);

        if (Convert.ToBoolean(res["status"]))
        {
            this.drawTable();
            this.loadStaze();
        }
        else
        {
            x2log.logData(res, "error in app", "error in save staze");
            this.msg_lbl.Text = res["msg"].ToString();
        }
    }

    protected void publishFnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string id = btn.ID.ToString();

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);
        int dateGrp = x2.makeDateGroup(rok, mesiac);

        StringBuilder sb = new StringBuilder();

        if (id == "publish_btn")
        {

            sb.AppendFormat("UPDATE [is_staze_2] SET [public]='yes' WHERE [date_group]='{0}'", dateGrp);
        }
        if (id == "unpublish_btn")
        {
            sb.AppendFormat("UPDATE [is_staze_2] SET [public]='no' WHERE [date_group]='{0}'", dateGrp);
        }

        SortedList res = x2Mysql.execute(sb.ToString());

        if (!Convert.ToBoolean(res["status"]))
        {
            x2log.logData(res, "error", "error in setting staze");
        }


    }

    protected void printFnc(object sender, EventArgs e)
    {
        Session.Add("st_akt_month", this.mesiac_cb.SelectedValue);
        Session.Add("st_akt_year", this.rok_cb.SelectedValue);

        Button btn = (Button)sender;
        string id = btn.ID.ToString();

        if (id == "word_btn")
        {
            Session.Add("st_to_word", true);
        }
        else
        {
            Session.Add("st_to_word", false);
        }

        Response.Redirect("stazeword.aspx");
    }

}
