using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_seminar : System.Web.UI.Page
{

    x2_var x2 = new x2_var();
    mysql_db x2Mysql = new mysql_db();
    string rights;
    string gKlinika;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        x2.fillYearMonth(ref this.month_dl, ref this.year_dl, Session["month_dl"].ToString(),Session["years_dl"].ToString());



        this.rights = Session["rights"].ToString();
        this.gKlinika = Session["klinika_id"].ToString();

        if (this.rights.IndexOf("admin")!=-1 || this.rights == "poweruser")
        {
            this.editSection.Visible = true;
        }
        else
        {
            this.editSection.Visible = false;
        }


        if (!IsPostBack)
        {
            this.setDate();
            this.loadDoctors();
        }

        this.loadSeminars();
    }

    protected void setDate()
    {
        DateTime dt = DateTime.Today;
        int month = dt.Month;
        int year = dt.Year;

        this.month_dl.SelectedValue = month.ToString();
        this.year_dl.SelectedValue = year.ToString();

    }

    protected void loadDoctors()
    {
        string query = "SELECT [id],[name3] FROM [is_users] WHERE [work_group]='doctor' AND [active]='1' AND [klinika]={0} ORDER BY [name2]";

        query = x2Mysql.buildSql(query, new string[] { this.gKlinika });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        this.doctors_dl.Items.Clear();
        int docsLn = table.Count;
        System.Web.UI.WebControls.ListItem[] newItem = new System.Web.UI.WebControls.ListItem[docsLn];
        // this.deps_dl.Items.Clear();
        for (int dep = 0; dep < docsLn; dep++)
        {
            this.doctors_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[dep]["name3"].ToString(), table[dep]["id"].ToString()));
        }
    }
    protected void saveDataFnc(object sender, EventArgs e)
    {
        SortedList data = new SortedList();
        data.Add("date", this.date_txt.Text.ToString());
        data.Add("user_id", this.doctors_dl.SelectedValue.ToString());

        string tema = this.tema_txt.Text.ToString();

        if (tema.Length == 0)
        {
            tema = "Seminar";
        }
        data.Add("tema", tema);
        data.Add("clinic_id", this.gKlinika);

        SortedList res = x2Mysql.mysql_insert("is_seminars", data); 
        if (!(Boolean)res["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }
        else
        {
            this.loadSeminars(); 
        }
    }

    protected void loadSeminars()
    {
        this.seminars_tbl.Controls.Clear();




        int month = Convert.ToInt32(this.month_dl.SelectedValue); ;
        int year = Convert.ToInt32(this.year_dl.SelectedValue);
        int days = DateTime.DaysInMonth(year, month);

        string query = @"SELECT [is_seminars].*,[users.name3] AS [name] FROM [is_seminars] 
                            INNER JOIN [is_users] AS [users] ON [users.id] = [is_seminars.user_id]
                            WHERE [date] BETWEEN '{0}-{1}-1' AND '{0}-{1}-{2}' AND [clinic_id]={3} ORDER BY [date] ASC";

        query = x2Mysql.buildSql(query, new string[] { year.ToString(), month.ToString(), days.ToString(),this.gKlinika });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        int tbLn = table.Count;

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell headCellDate = new TableHeaderCell();
        headCellDate.Text = "Dátum";
        headCellDate.Font.Bold = true;
        headRow.Controls.Add(headCellDate);

        TableHeaderCell headCellSemin = new TableHeaderCell();
        headCellSemin.Text = "Nazov seminára";
        headCellSemin.Font.Bold = true;
        headRow.Controls.Add(headCellSemin);

        TableHeaderCell nameCellSemin = new TableHeaderCell();
        nameCellSemin.Text = "Meno";
        nameCellSemin.Font.Bold = true;
        headRow.Controls.Add(nameCellSemin);

        TableHeaderCell headCellAction = new TableHeaderCell();
        headCellAction.Text = "Akcia";
        headCellAction.Font.Bold = true;
        headRow.Controls.Add(headCellAction);

        this.seminars_tbl.Controls.Add(headRow);

        for (int i=0; i< tbLn; i++)
        {
            TableRow tblRow = new TableRow();

            TableCell dateCell = new TableCell();
            DateTime dt = Convert.ToDateTime(x2.MSDate(table[i]["date"].ToString()));

            dateCell.Text = dt.ToLongDateString();
            tblRow.Controls.Add(dateCell);

            TableCell seminCell = new TableCell();
            string tema = x2.getStr(table[i]["tema"].ToString());
            if (tema.Length==0)
            {
                tema = "Seminar";
            }
            seminCell.Text = tema;
            tblRow.Controls.Add(seminCell);

            TableCell nameCell = new TableCell();
            nameCell.Text = table[i]["name"].ToString();
            tblRow.Controls.Add(nameCell);


            TableCell actionCell = new TableCell();
            actionCell.Text = "-";
            if (this.rights.IndexOf("admin")!=-1 || this.rights=="poweruser")
            {
                Button editBtn = new Button();
                editBtn.Text = "Edituj";
                editBtn.ID = "edit_" + table[i]["id"].ToString();
                editBtn.CssClass = "button blue";
                editBtn.Click += new EventHandler(seminarWorkFnc);
                actionCell.Controls.Add(editBtn);

                Button deleteBtn = new Button();
                deleteBtn.Text = Resources.Resource.delete;
                deleteBtn.ID = "delete_" + table[i]["id"].ToString();
                deleteBtn.CssClass = "button blue";
                deleteBtn.CssClass = "button red";
                deleteBtn.OnClientClick = "return confirm('Naozaj zmazat?')";
                deleteBtn.Click += new EventHandler(seminarWorkFnc);
                actionCell.Controls.Add(deleteBtn);
            }
            
            tblRow.Controls.Add(actionCell);

            this.seminars_tbl.Controls.Add(tblRow);

        }
    }

    protected void seminarWorkFnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;

        string id = btn.ID.ToString();

        string[] data = id.Split('_');

        if (data[0] == "delete")
        {
            string query = "DELETE FROM [is_seminars] WHERE [id]={0}";
            query = x2Mysql.buildSql(query, new string[] { data[1] });

            SortedList res = x2Mysql.execute(query);
            if (!(Boolean)res["status"])
            {
                x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
            }
            else
            {
                this.loadSeminars();
            }
        }

        if (data[0] == "edit")
        {
            string query = @"SELECT [is_seminars.date] AS [date],[is_seminars.tema] AS [tema] ,[users.name3] AS [name],[users.id] AS [user_id] FROM [is_seminars] 
                            INNER JOIN [is_users] AS [users] ON [users.id] = [is_seminars.user_id]
                            WHERE [is_seminars.id] ={0}";
            query = x2Mysql.buildSql(query, new string[] { data[1] });

            SortedList res = x2Mysql.getRow(query);

            if (res["status"] != null)
            {
                x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
            }
            else
            {
                this.date_txt.Text = x2.unixDate(Convert.ToDateTime(res["date"].ToString()));
                this.doctors_dl.SelectedValue = res["user_id"].ToString();
                this.tema_txt.Text = res["tema"].ToString();
            }

        }


    }


    protected void refreshPageFnc(object sender,EventArgs e)
    {
        this.loadSeminars();
    }
}