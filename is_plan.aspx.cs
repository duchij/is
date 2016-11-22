﻿using System;
using System.IO;
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


class planClass
{
    private string _rights = "";
    private int _gKlinika = 0;
    private mysql_db _mysql;
    private x2_var _x2;
    private log _log;
    private string _clinicIdf;

    private string _department;

    private string _userId;

    private Boolean _editable;
    private string[] _vykazHeader;

    private SortedList _userData;

    private string[] _workTypes;

    public string[] workTypes
    {
        get { return _workTypes; }
        set { _workTypes = value; }
    }

    public SortedList userData
    {
        get { return _userData; }
        set { _userData = value; }
    }

    public string userId
    {
        get { return _userId; }
        set { _userId = value; }
    }

    public string department
    {
        get { return _department; }
        set { _department = value; }
    }

    public Boolean editable
    {
        get
        {
            if (_rights.IndexOf("admin") != -1 || _rights == "poweruser")
            {
                return true;
            }
            else { return false; }

        }
    }

    public string[] vykazHeader
    {
        get { return _vykazHeader; }
        set { _vykazHeader = value; }
    }


    public string clinicIdf
    {
        get { return _clinicIdf; }
        set { _clinicIdf = value; }
    }

    public string rights
    {
        get { return _rights; }
        set { _rights = value; }
    }

    public mysql_db mysql
    {
        get { return _mysql; }
        set { _mysql = value; }
    }



    public int gKlinika
    {
        get { return _gKlinika; }
        set { _gKlinika = value; }
    }

    public x2_var x2
    {
        get { return _x2; }
        set { _x2 = value; }


    }

    public log x2log
    {
        get { return _log; }
        set { _log = value; }
    }

   

}

public partial class is_plan : System.Web.UI.Page
{
    planClass plan = new planClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }


        plan.gKlinika = Convert.ToInt32(Session["klinika_id"]);
        plan.rights = Session["rights"].ToString();
        plan.clinicIdf = Session["klinika"].ToString();
        plan.mysql = new mysql_db();
        plan.x2 = new x2_var();
        plan.x2log = new log();
        plan.userData = new SortedList();

        plan.department = Session["oddelenie"].ToString();
        plan.workTypes = this.getWorkTypes();


        if (!IsPostBack)
        {
            plan.x2.fillYearMonth(ref this.month_dl, ref this.years_dl, Session["month_dl"].ToString(), Session["years_dl"].ToString());
            DateTime dnesJe = DateTime.Today;
            this.month_dl.SelectedValue = dnesJe.Month.ToString();
            this.years_dl.SelectedValue = dnesJe.Year.ToString();
            this.loadDeps();
        }

       // this.drawTable();
    }

    protected void drawTable()
    {
        int month = Convert.ToInt32(this.month_dl.SelectedValue);
        int year = Convert.ToInt32(this.years_dl.SelectedValue);

        string[] freeDays = Session["freedays"].ToString().Split(',');

        string depData = this.deps_dl.SelectedValue.ToString();

        string depId = depData.Substring(depData.IndexOf("_")+1, depData.Length - (depData.IndexOf("_")+1));

        string query = @"
                            SELECT [t_nurse.full_name],[t_nurse.id] AS [user_id] 
                                FROM [is_users] AS [t_nurse]
                        WHERE [t_nurse.oddelenie] = {0}
                         AND [t_nurse.work_group] IN ('nurse','sanitar','assistent')
                         AND [t_nurse.active] = 1
                        ORDER BY [t_nurse.work_group]
                        ";

        query = plan.mysql.buildSql(query, new string[] { depId });

        Dictionary<int, SortedList> table = plan.mysql.getTableSL(query);

        int days = DateTime.DaysInMonth(year, month);

        int usersLn = table.Count;
        TableHeaderRow headRow = new TableHeaderRow();
        Boolean vikend = false;
        Boolean sviatok = true;

        for (int dayL=0; dayL < days; dayL++)
        {

            DateTime my_date = new DateTime(year, month, dayL + 1);
            int dnesJe = (int)my_date.DayOfWeek;

            if (dnesJe == 6 || dnesJe == 0)
            {
                vikend = true;
            }else
            {
                vikend = false;
            }

            string den = (dayL + 1) + "." + month.ToString();
            TableHeaderCell dateCell = new TableHeaderCell();
            
            if (vikend)
            {
                dateCell.BackColor = System.Drawing.Color.Red;
            }
            dateCell.Text = (dayL + 1) + "." + month.ToString();
            if (Array.IndexOf(freeDays, den) != -1)
            {
                sviatok = true;
            }else
            {
                sviatok = false;
            }

            if (sviatok)
            {
                dateCell.BackColor = System.Drawing.Color.Yellow;
            }
            dateCell.BorderStyle = BorderStyle.Solid;
            dateCell.BorderColor = System.Drawing.Color.Black;
            dateCell.BorderWidth = Unit.Pixel(1);

            headRow.Controls.Add(dateCell);
        }
        this.planTable_tbl.Controls.Add(headRow);
        for (int u = 0; u < usersLn; u++)
        {
            TableRow nameRow = new TableRow();
            TableCell nameCell = new TableCell();
            nameCell.ColumnSpan = days;
            nameCell.Font.Size = 12;
            nameCell.Text = table[u]["full_name"].ToString();
            nameCell.BackColor = System.Drawing.Color.LightGray;
            nameCell.HorizontalAlign = HorizontalAlign.Left;
            nameRow.Controls.Add(nameCell);
            this.planTable_tbl.Controls.Add(nameRow);
            TableRow riadok = new TableRow();
           
            for (int d = 0; d < days; d++)
            {
                DateTime my_date = new DateTime(year, month, d + 1);
                int dnesJe = (int)my_date.DayOfWeek;

                if (dnesJe == 6 || dnesJe == 0)
                {
                    vikend = true;
                }
                else
                {
                    vikend = false;
                }
                string den = (d + 1) + "." + month.ToString();

                if (Array.IndexOf(freeDays, den) != -1)
                {
                    sviatok = true;
                }
                else
                {
                    sviatok = false;
                }

                


                TableCell dayCell = new TableCell();

                if (vikend)
                {
                    dayCell.BackColor = System.Drawing.Color.Red;
                }

                if (sviatok)
                {
                    dayCell.BackColor = System.Drawing.Color.Yellow;
                }

                dayCell.BorderStyle = BorderStyle.Solid;
                dayCell.BorderColor = System.Drawing.Color.Black;
                dayCell.BorderWidth = Unit.Pixel(1);
                DropDownList dayDl = new DropDownList();
                dayDl.ID = "dayCell_" + table[u]["user_id"].ToString() + "_" + (d + 1);
                dayDl.Width = Unit.Pixel(38);

                dayDl.ToolTip = my_date.ToLongDateString();
               
                this.fillWorkTypes(dayDl);
                dayDl.Attributes.Add("onChange", "saveDayOfNurse('" + dayDl.ID.ToString() + "');");
                dayCell.Controls.Add(dayDl);

                riadok.Controls.Add(dayCell);
            }
            this.planTable_tbl.Controls.Add(riadok);
        }

        this.loadShifts(month, year);
        this.loadActivities(month, year);
    
    }

    protected void fillWorkTypes(DropDownList dl)
    {
        dl.Items.Clear();
        dl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));

        int typeLn = plan.workTypes.Length;
        for (int type = 0; type < typeLn; type++)
        {

            dl.Items.Add(new System.Web.UI.WebControls.ListItem(plan.workTypes[type], plan.workTypes[type]));

        }
        dl.Items.Add(new System.Web.UI.WebControls.ListItem("Dovolenka", "do"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem("Vymaz dovolenku", "doex"));
    }

    protected string[] getWorkTypes()
    {
        string sql = "SELECT [data] FROM [is_settings] WHERE [name]='kdch_shifts_nurse_2'";

        sql = plan.mysql.buildSql(sql, new string[] { });

        SortedList row = plan.mysql.getRow(sql);

        string data = row["data"].ToString().Replace(";", " ").Replace(",", " ");

        string[] dataArr = data.Split(' ');

        return dataArr;

    }

    protected void loadActivities(int mesiac, int rok)
    {
        string query = @"
                        SELECT [user_id],[od],[do], [type]
                            FROM [is_dovolenky_sestr]  
                        WHERE ([od] BETWEEN '{2}-{0}-01 00:00:00' AND '{2}-{0}-{1} 23:59:00'
                            OR [do] BETWEEN '{2}-{0}-01 00:00:00' AND '{2}-{0}-{1} 23:59:00')
                            AND [type]='do' AND [clinics]={3} 
                        ORDER BY [do] ASC
                        ";
        int clinic = Convert.ToInt32(Session["klinika_id"].ToString());
        int days = DateTime.DaysInMonth(rok, mesiac);

        query = plan.mysql.buildSql(query,new string[] { mesiac.ToString(), days.ToString(), rok.ToString(), clinic.ToString() });

        Dictionary<int, Hashtable> table = plan.mysql.getTable(query);

        int tblLn = table.Count;


        for (int i=0; i<tblLn; i++)
        {
            DateTime odDatum = Convert.ToDateTime(table[i]["od"].ToString());
            DateTime doDatum = Convert.ToDateTime(table[i]["do"].ToString());

            for (DateTime ddStart = odDatum; ddStart <= doDatum; ddStart += TimeSpan.FromDays(1))
            {
                int day = ddStart.Day;

                Control ctrl = FindControl("dayCell_" + table[i]["user_id"].ToString() + "_" + day.ToString());
                DropDownList dl = (DropDownList)ctrl;
                if (dl != null)
                {
                    dl.SelectedValue = table[i]["type"].ToString();
                }
                
            }
        }

    }


    protected void loadShifts(int mesiac, int rok)
    {
        int dateGrp = plan.x2.makeDateGroup(rok, mesiac);
        string depData = this.deps_dl.SelectedValue.ToString();

        string depIdf = depData.Substring(0, depData.IndexOf("_"));

        string query = @"
                            SELECT [user_id],[datum],[typ],[deps]
                                FROM [is_sluzby_2_sestr]
                            WHERE [date_group] = {0}
                                AND [deps] = '{1}'
                            ORDER BY [datum]
                        ";
        query = plan.mysql.buildSql(query, new string[] { dateGrp.ToString(), depIdf });

        Dictionary<int, Hashtable> table = plan.mysql.getTable(query);
        int tblLn = table.Count;
        for (int i=0; i < tblLn; i++)
        {
            DateTime dt = plan.x2.UnixToMsDateTime(table[i]["datum"].ToString());

            int day = dt.Day;

            Control crtl = FindControl("dayCell_" + table[i]["user_id"].ToString() + "_" + day.ToString());

            DropDownList dl = (DropDownList)crtl;

            dl.SelectedValue = table[i]["typ"].ToString();

        }

    }


    protected void loadDeps()
    {



        if (!IsPostBack)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT [label],[id],[idf] FROM [is_deps] WHERE [clinic_id]='{0}'", Session["klinika_id"]);

            Dictionary<int, Hashtable> table = plan.mysql.getTable(sb.ToString());


            int depsLn = table.Count;

            this.deps_dl.Items.Clear();
            this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
            for (int dep = 0; dep < depsLn; dep++)
            {

                this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[dep]["label"].ToString(), table[dep]["idf"]+"_"+table[dep]["id"].ToString()));

            }
        }

    }



    protected void setPlanForDepartmentFnc(object sender, EventArgs e)
    {
        this.drawTable();
    }
}