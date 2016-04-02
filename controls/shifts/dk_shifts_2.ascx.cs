using System;
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

class ShiftsObjDK
{
    private string _rights = "";
    private int _gKlinika = 0;
    private mysql_db _mysql;
    private x2_var _x2;
    private log _log;
    private string _clinicIdf;
    private Boolean _userControl;


    private string _state;

    public string state
    {
        get { return _state; }
        set { _state = value; }
    }



    public Boolean userControl
    {
        get { return _userControl; }
        set { _userControl = value; }
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



public partial class controls_shifts_dk_shifts_2 : System.Web.UI.UserControl
{
    ShiftsObjDK Shifts = new ShiftsObjDK();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.initObj();
        
        
        
        if (!IsPostBack)
        {
            Shifts.x2.fillYearMonth(ref this.mesiac_cb, ref this.rok_cb, Session["month_dl"].ToString(), Session["years_dl"].ToString());
            this.setMonthYear();
        }

      
        
        this.drawTable();
        this.generateWeekStatus();
    }

    protected void initObj()
    {
        Shifts.gKlinika = Convert.ToInt32(Session["klinika_id"]);
        Shifts.rights = Session["rights"].ToString();
        Shifts.clinicIdf = Session["klinika"].ToString();
        Shifts.mysql = new mysql_db();
        Shifts.x2 = new x2_var();
        Shifts.x2log = new log();
        Shifts.userControl = true;

    }

    protected void setMonthYear()
    {
        DateTime dnes = DateTime.Today;

        //int days = Date

        int mesiac = dnes.Month;
        int rok = dnes.Year;
        int days = DateTime.DaysInMonth(rok, mesiac);
        this.days_lbl.Text = days.ToString();
        this.mesiac_cb.SelectedValue = mesiac.ToString();
        this.rok_cb.SelectedValue = rok.ToString();
    }

    protected void drawTable()
    {
        string query = @"SELECT [name],[data] FROM [is_settings] WHERE [name]='{0}'";

        string idf = Shifts.clinicIdf + "_shift_doctors";

        query = Shifts.mysql.buildSql(query, new string[] { idf });

        SortedList row = Shifts.mysql.getRow(query);

        string data = row["data"].ToString();

        this.parseMultiTable(data);

    }

    protected void changeSluzba(object sender, EventArgs e)
    {
        //string state = this.getState();

        //if (state == "active")
        //{
        //    this.editShift_chk.Checked = true;
        //}
        //else
        //{
        //    this.editShift_chk.Checked = false;
        //}

        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int days = DateTime.DaysInMonth(rok, mesiac);
        this.days_lbl.Text = days.ToString();

        this.generateWeekStatus();
        this.shiftTable.Controls.Clear();
        this.drawTable();

    }

    protected void parseMultiTable(string data)
    {
        string[] freeDays = Session["freedays"].ToString().Split(',');

        this.shiftTable.Controls.Clear();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();


        int daysMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        string dateGroup = rok + mesiac;
        Session.Add("aktDateGroup", dateGroup);


        string[] groups = data.Split(';');
        ArrayList doctorList = this.loadOmegaDoctors();
        int grps = groups.Length;
        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dayHeadCell = new TableHeaderCell();
        dayHeadCell.Text = "Datum";
        headRow.Controls.Add(dayHeadCell);

        for (int i = 0; i < grps; i++)
        {
            //string[] head = groups[i].Split('_');

            TableHeaderCell headCell = new TableHeaderCell();
            headCell.HorizontalAlign = HorizontalAlign.Center;
            string headLbl = groups[i];
            if (headLbl.IndexOf(",") != -1)
            {
                headLbl = headLbl.Replace(",", "<br>");
            }
            headCell.Text = "<center><strong>" + headLbl + "</center></strong>";

            headRow.Controls.Add(headCell);
        }
        this.shiftTable.Controls.Add(headRow);

        for (int day = 0; day < daysMonth; day++)
        {

            TableRow dataRow = new TableRow();

            TableCell dayCell = new TableCell();
            int rDay = day + 1;

            DateTime myDate = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), rDay);
            int dnesJe = (int)myDate.DayOfWeek;
            string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
            string sviatok = rDay.ToString() + "." + myDate.Month.ToString();
            int jeSviatok = Array.IndexOf(freeDays, sviatok);



            dayCell.Text = rDay.ToString() + ". " + nazov;
            dayCell.ID = "dateCell_" + rDay.ToString();

            /*if (dnesJe == 0 || dnesJe == 6)
            {
                dayCell.CssClass = "box red";
            }

            if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
            {
                dayCell.CssClass = "box yellow";
            }*/

            dataRow.Controls.Add(dayCell);

            for (int col = 0; col < grps; col++)
            {
                TableCell dataCell = new TableCell();

                if (dnesJe == 0 || dnesJe == 6)
                {
                    dataCell.CssClass = "box red";
                }

                if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                {
                    dataCell.CssClass = "box yellow";
                }


                string[] ddls = groups[col].Split(',');
                if (ddls.Length > 0)
                {
                    int dCount = ddls.Length;
                    for (int cnt = 0; cnt < dCount; cnt++)
                    {

                        if (this.editShift_chk.Checked)
                        {
                            DropDownList dl = new DropDownList();
                            dl.ID = "ddl_" + rDay.ToString() + "_" + ddls[cnt];

                            int listLn = doctorList.Count;
                            System.Web.UI.WebControls.ListItem[] newItem = new System.Web.UI.WebControls.ListItem[listLn];

                            for (int doc = 0; doc < listLn; doc++)
                            {
                                string[] tmp = doctorList[doc].ToString().Split('|');
                                newItem[doc] = new System.Web.UI.WebControls.ListItem(tmp[1].ToString(), tmp[0].ToString());
                            }
                            dl.ToolTip = ddls[cnt];
                            dl.Items.AddRange(newItem);

                            //dl.AutoPostBack = true;
                            // dl.SelectedIndexChanged += new EventHandler(dItemChanged_2);
                            dl.Attributes.Add("onChange", "saveAllDocShifts('" + dl.ID.ToString() + "');");
                            dataCell.Controls.Add(dl);

                            TextBox comment = new TextBox();
                            comment.ID = "textBox_" + rDay.ToString() + "_" + ddls[cnt];
                            comment.Text = "-";
                            comment.Attributes.Add("onChange", "saveAllDocShiftComment('" + comment.ID.ToString() + "');");
                            // comment.AutoPostBack = true;
                            // comment.TextChanged += new EventHandler(commentChanged_2);


                            dataCell.Controls.Add(comment);

                            dataRow.Controls.Add(dataCell);
                        }
                        else
                        {
                            Label dl = new Label();
                            dl.ID = "name_" + rDay.ToString() + "_" + ddls[cnt];
                            dl.Text = "-";

                            dataCell.Controls.Add(dl);

                            Label comment = new Label();
                            comment.ID = "comment_" + rDay.ToString() + "_" + ddls[cnt];
                            comment.Text = "-";
                            dataCell.Controls.Add(comment);

                            dataRow.Controls.Add(dataCell);
                        }


                    }
                }
                else
                {
                    if (this.editShift_chk.Checked)
                    {
                        DropDownList dl1 = new DropDownList();
                        dl1.ID = "ddl_" + rDay.ToString() + "_" + groups[col];

                        int listLn = doctorList.Count;
                        System.Web.UI.WebControls.ListItem[] newItem = new System.Web.UI.WebControls.ListItem[listLn];

                        for (int doc = 0; doc < listLn; doc++)
                        {
                            string[] tmp = doctorList[doc].ToString().Split('|');
                            newItem[doc] = new System.Web.UI.WebControls.ListItem(tmp[1].ToString(), tmp[0].ToString());
                        }
                        dl1.Items.AddRange(newItem);
                        //dl1.AutoPostBack = true;
                        //dl1.SelectedIndexChanged += new EventHandler(dItemChanged_2);
                        dl1.Attributes.Add("onChange", "saveAllDocShifts('" + dl1.ID.ToString() + "');");
                        dataCell.Controls.Add(dl1);

                        TextBox comment = new TextBox();
                        comment.ID = "textBox_" + rDay.ToString() + "_" + groups[col];
                        comment.Text = "-";
                        //comment.AutoPostBack = true;
                        //comment.TextChanged += new EventHandler(commentChanged_2);
                        comment.Attributes.Add("onChange", "saveAllDocShiftComment('" + comment.ID.ToString() + "');");
                        dataCell.Controls.Add(comment);
                        dataRow.Controls.Add(dataCell);
                    }
                    else
                    {
                        Label dl = new Label();
                        dl.ID = "name_" + rDay.ToString() + "_" + groups[col];
                        dl.Text = "-";

                        dataCell.Controls.Add(dl);

                        Label comment = new Label();
                        comment.ID = "comment_" + rDay.ToString() + "_" + groups[col];
                        comment.Text = "-";
                        dataCell.Controls.Add(comment);

                        dataRow.Controls.Add(dataCell);
                    }
                }
            }
            int month = Convert.ToInt32(mesiac);
            if (rDay == DateTime.Today.Day && month == DateTime.Today.Month)
            {
                dataRow.BorderWidth = Unit.Pixel(3);
                dataRow.BorderStyle = BorderStyle.Solid;
                dataRow.BorderColor = System.Drawing.Color.Red;
            }

            this.shiftTable.Controls.Add(dataRow);
        }
        this.setShifts();
    }


    protected void setShifts()
    {
        string query = "";
        //string depsIdf = this.deps_dl.SelectedValue.ToString();

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        int dateGroup = Shifts.x2.makeDateGroup(rok, mesiac);

        // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        if (Shifts.editable)
        {

            query = @"SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],
                            [t_sluzb].[state] AS [state],
                            GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],
                            GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],
            GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],
            [t_sluzb].[date_group] AS [dategroup] 
            FROM [is_sluzby_all] AS [t_sluzb]
            LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id] 
            WHERE [t_sluzb].[date_group] = '{0}' 
            AND ([t_sluzb].[clinic] = '{1}' OR [t_sluzb].[clinic] = '{2}' OR [t_sluzb].[clinic] = '{3}') 
            GROUP BY [t_sluzb].[datum]
            ORDER BY [t_sluzb].[datum]";
        }
        else
        {
            query = @"SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],
                [t_sluzb].[state] AS [state],
                GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],
                GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],
                GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],
                [t_sluzb].[date_group] AS [dategroup]
                FROM [is_sluzby_all] AS [t_sluzb]
                LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]
                    WHERE [t_sluzb].[date_group] = '{0}' 
                -- AND [t_sluzb].[state]='active'
                AND ([t_sluzb].[clinic] = '{1}' OR [t_sluzb].[clinic] = '{2}' OR [t_sluzb].[clinic] = '{3}')  
                GROUP BY [t_sluzb].[datum]
                ORDER BY [t_sluzb].[datum]";
        }

        int daysMonth = DateTime.DaysInMonth(rok, mesiac);


        query = Shifts.mysql.buildSql(query, new string[] { dateGroup.ToString(), Shifts.gKlinika.ToString(),"5","6" });

        Dictionary<int, Hashtable> table = Shifts.mysql.getTable(query);

        int tblCnt = table.Count;
        if (tblCnt > 0)
        {


            if (Shifts.editable)
            {
                string state = table[0]["state"].ToString();

                if (state == "active")
                {
                    this.shiftState_lbl.Text = Resources.Resource.shifts_see_all;
                    //this.publish_cb.ch
                }
                else
                {
                    this.shiftState_lbl.Text = Resources.Resource.shifts_see_limited;
                }
            }

            for (int day = 0; day < tblCnt; day++)
            {

                int rDay = Convert.ToDateTime(Shifts.x2.UnixToMsDateTime(table[day]["datum"].ToString())).Day;
                string[] names = table[day]["users_names"].ToString().Split(';');
                string[] userId = table[day]["users_ids"].ToString().Split('|');
                string[] comments = table[day]["comment"].ToString().Split('|');
                string[] type = table[day]["type1"].ToString().Split(';');

                int tpLn = type.Length;

                if (names.Length != userId.Length)
                {
                    Shifts.x2.errorMessage2(ref this.msg_lbl, "Inkonzistencia v datach prosim opravit.....");
                }
                if (tpLn == 0)
                {
                    if (this.editShift_chk.Checked)
                    {
                        Control crtl = FindControl("ddl_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        DropDownList ddl = (DropDownList)crtl;

                        ddl.SelectedValue = table[day]["users_ids"].ToString();
                        // ddl.SelectedIndexChanged += new EventHandler(dItemChanged_2);
                        ddl.Attributes.Add("onChange", "saveAllDocShifts('" + ddl.ID.ToString() + "');");

                        if (Shifts.userControl)
                        {
                            if (Shifts.rights == "users" && (Session["user_id"].ToString() != table[day]["users_ids"].ToString()))
                            {
                                ddl.Enabled = false;
                            }
                            else
                            {
                                if (Shifts.rights == "users")
                                {
                                    System.Web.UI.WebControls.ListItem data = ddl.Items.FindByValue(Session["user_id"].ToString());
                                    ddl.Items.Clear();
                                    ddl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
                                    ddl.Items.Add(data);
                                }

                            }
                        }


                        Control crtl1 = FindControl("textBox_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        TextBox textBox = (TextBox)crtl1;
                        textBox.Text = table[day]["comment"].ToString();
                        textBox.Attributes.Add("onChange", "saveAllDocShiftComment('" + textBox.ID.ToString() + "');");

                        if (Shifts.userControl)
                        {
                            if (Shifts.rights == "users" && (Session["user_id"].ToString() != table[day]["users_ids"].ToString()))
                            {
                                textBox.ReadOnly = true;
                            }
                        }


                        //textBox.TextChanged += new EventHandler(commentChanged_2);
                    }
                    else
                    {
                        Control crtl = FindControl("name_" + rDay.ToString() + "_" + table[day]["type1"].ToString());

                        Label ddl = (Label)crtl;

                        try
                        {
                            ddl.Text = table[day]["user_names"].ToString() + "<br>";
                        }
                        catch (Exception ex)
                        {
                            ddl.Text = "-<br>";
                        }

                        Control crtl1 = FindControl("comment_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        Label textBox = (Label)crtl1;
                        try
                        {
                            textBox.Text = "<p class='small'>" + table[day]["comment"].ToString() + "</p>";
                        }
                        catch (Exception ex)
                        {
                            textBox.Text = "<p class='small'>-</p>";
                        }



                    }
                }
                else
                {
                    for (int col = 0; col < tpLn; col++)
                    {
                        if (this.editShift_chk.Checked)
                        {
                            Control crtl = FindControl("ddl_" + rDay.ToString() + "_" + type[col]);
                            DropDownList ddl = (DropDownList)crtl;

                            ddl.SelectedValue = userId[col];
                            if (Shifts.userControl)
                            {
                                if (Shifts.rights == "users" && (Session["user_id"].ToString() != userId[col].ToString()))
                                {
                                    ddl.Enabled = false;
                                }
                                else
                                {
                                    if (Shifts.rights == "users")
                                    {
                                        System.Web.UI.WebControls.ListItem data = ddl.Items.FindByValue(Session["user_id"].ToString());
                                        ddl.Items.Clear();
                                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
                                        ddl.Items.Add(data);
                                    }

                                }
                            }



                            //ddl.SelectedIndexChanged += new EventHandler(dItemChanged_2);

                            ddl.Attributes.Add("onChange", "saveAllDocShifts('" + ddl.ID.ToString() + "');");


                            Control crtl1 = FindControl("textBox_" + rDay.ToString() + "_" + type[col]);
                            TextBox textBox = (TextBox)crtl1;
                            textBox.Text = comments[col];
                            if (Shifts.userControl)
                            {
                                if (Shifts.rights == "users" && (Session["user_id"].ToString() != userId[col]))
                                {
                                    textBox.ReadOnly = true;
                                }
                            }

                            // textBox.TextChanged += new EventHandler(commentChanged_2);
                            textBox.Attributes.Add("onChange", "saveAllDocShiftComment('" + textBox.ID.ToString() + "');");
                        }
                        else
                        {
                            Control crtl = FindControl("name_" + rDay.ToString() + "_" + type[col]);
                            Label ddl = (Label)crtl;

                            try
                            {
                                ddl.Text = names[col] + "<br><em>(" + type[col] + ")</em><br>";
                            }
                            catch (Exception ex)
                            {
                                ddl.Text = "-<br>";
                            }

                            Control crtl1 = FindControl("comment_" + rDay.ToString() + "_" + type[col]);
                            Label textBox = (Label)crtl1;
                            textBox.Text = comments[col] + "<br>";
                        }
                    }
                }

            }
        }
        else
        {
            if (Shifts.editable)
            {
                this.msg_lbl.Text = Resources.Resource.shifts_not_done;
                // this.publish_cb.Visible = false;
            }
        }
    }


    protected ArrayList loadOmegaDoctors()
    {

        ArrayList result = new ArrayList();

        if (Session["active_doctors"] != null)
        {
            result = (ArrayList)Session["active_doctors"];
        }
        else
        {
            
            string query = @"SELECT [is_users].[name3] AS [name], [is_users].[id] AS [users_id], [is_clinics].[idf] AS [idf]
                                FROM [is_users]
                            INNER JOIN [is_clinics] ON [is_clinics].[id] = [is_users].[klinika]
                            WHERE [is_users].[klinika]='{0}' OR [is_users].[klinika]={1} OR [is_users].[klinika]={2} ORDER BY [is_users].[name3]";
            
            query = Shifts.mysql.buildSql(query,new string[] {Shifts.gKlinika.ToString(),"5","6"});
            Dictionary<int, Hashtable> table = Shifts.mysql.getTable(query);
            int dataLn = table.Count;
            result.Add("0|-");

            for (int i = 1; i <= dataLn; i++)
            {
                result.Add(table[i - 1]["users_id"].ToString() + "|" + table[i - 1]["name"].ToString() + " (" + table[i - 1]["idf"].ToString() + ")");
            }

            Session["active_doctors"] = result;
        }

        return result;

    }

    protected int getFirstMonday(int rok, int mesiac)
    {
        int firstM = 1;
        for (int den = 1; den <= 7; den++)
        {
            DateTime dt = new DateTime(rok, mesiac, den);
            if (dt.DayOfWeek == DayOfWeek.Monday)
            {
                firstM = den;
                break;
            }
        }
        return firstM;
    }

    protected void generateWeekStatus()
    {
        this.weekState_tbl.Controls.Clear();
        

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        int days = DateTime.DaysInMonth(rok, mesiac);

        TableHeaderRow headRow = new TableHeaderRow();
        this.weekState_tbl.Controls.Add(headRow);
        int startDay = this.getFirstMonday(rok, mesiac);
        int endDay = startDay + 6;

        // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        // ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        for (int week = 0; week <= 5; week++)
        {
            Control tmpC = FindControl("tempWeek_" + week.ToString());
            HiddenField tempWeek = (HiddenField)tmpC;

            TableHeaderCell headCell = new TableHeaderCell();

            if (week < 5)
            {
                if (week == 0)
                {
                    if (startDay > 1)
                    {
                        int eTmp = startDay - 1;
                        headCell.Text = "1. - " + eTmp.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                        tempWeek.Value = "1_" + eTmp.ToString();
                    }
                    else
                    {
                        headCell.Text = startDay.ToString() + ". - " + endDay.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                        tempWeek.Value = startDay.ToString() + "_" + endDay.ToString();
                        endDay = endDay + 7;
                        startDay = startDay + 7;
                    }
                }

                if (week > 0)
                {
                    if (endDay > days) endDay = days;
                    headCell.Text = startDay.ToString() + ". - " + endDay.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                    tempWeek.Value = startDay.ToString() + "_" + endDay.ToString();
                    endDay = endDay + 7;
                    // if (endDay > days) endDay = days;
                    startDay = startDay + 7;

                }
            }
            if (week == 5)
            {
                if (startDay < days)
                {
                    startDay = endDay - 6;
                    //int lendDay = startDay + 6;
                    headCell.Text = startDay.ToString() + ". - " + days.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                    tempWeek.Value = startDay.ToString() + "_" + endDay.ToString();
                }
                // weekStatus["week_" + week.ToString()] = tempWeek.Value.ToString();
            }
            headRow.Controls.Add(headCell);

            // startDay = endDay + 1;
        }
        TableRow riadok = new TableRow();
        this.weekState_tbl.Controls.Add(riadok);

        string query = "SELECT [date_start],[date_end],[state] FROM [is_dk_week_helper] WHERE [date_start] BETWEEN '{0}' AND '{1}' ORDER BY [date_start] ASC";

        string dtStart = rok.ToString() + "-" + mesiac.ToString() + "-1";
        string dtEnd = rok.ToString() + "-" + mesiac.ToString() +"-"+ days.ToString();
        query = Shifts.mysql.buildSql(query, new string[] {dtStart,dtEnd});
        Dictionary<int, Hashtable> table = Shifts.mysql.getTable(query);
        int tbLn = table.Count;

        for (int week = 0; week < 5; week++)
        {
            TableCell cellData = new TableCell();
            if (Shifts.editable) 
            {
                DropDownList state_dl = new DropDownList();
                state_dl.Items.Add(new ListItem("-", "0"));
                state_dl.Items.Add(new ListItem("Konziliarny", "konz"));
                state_dl.Items.Add(new ListItem("Prijmovy", "prijm"));
                state_dl.AutoPostBack = true;
                state_dl.ID = "stateweek_" + week.ToString();
                state_dl.SelectedIndexChanged += new EventHandler(saveWeekStatus);

                if (tbLn > 0 && week < tbLn)
                {
                    if (table[week]["state"].ToString() == "prijm")
                    {
                        cellData.CssClass = "info box";
                    }
                    else
                    {
                        cellData.CssClass = "success box";
                    }
         

                    state_dl.SelectedValue = table[week]["state"].ToString();
                    this.highlightWeek(table[week]);
                }
                cellData.Controls.Add(state_dl);
            }
            else
            {
                if (tbLn > 0 && week < tbLn)
                {
                    if (table[week]["state"].ToString() == "prijm")
                    {
                        cellData.CssClass = "info box";
                    }
                    else
                    {
                        cellData.CssClass = "success box";
                    }

                    cellData.Text = table[week]["state"].ToString();
                    this.highlightWeek(table[week]);
                }
            }

            
            riadok.Controls.Add(cellData);
            
            
        }
        this.weekState_tbl.Controls.Add(riadok);

    }

    protected void highlightWeek(Hashtable data)
    {
        DateTime dtStart = Shifts.x2.UnixToMsDateTime(data["date_start"].ToString());
        DateTime dtEnd = Shifts.x2.UnixToMsDateTime(data["date_end"].ToString());

        Control ctrl = new Control();


        for (DateTime dateTime = dtStart; dateTime <= dtEnd; dateTime += TimeSpan.FromDays(1))
        {
            int day = dateTime.Day;
            ctrl = FindControl("dateCell_" + day.ToString());
            TableCell cell = (TableCell)ctrl;
            if (data["state"].ToString() == "prijm")
            {
                cell.CssClass = "info box";
                cell.ToolTip = "Prijmovy";
            }
            else
            {
                cell.CssClass = "success box";
                cell.ToolTip = "Konziliarny";
            }
                
        }

    }

    protected void saveWeekStatus(object sender, EventArgs e)
    {
        SortedList data = new SortedList();

        DropDownList dlWeek = (DropDownList)sender;
        if (dlWeek.SelectedValue != "0")
        {
            string[] weekTmp = dlWeek.ID.ToString().Split('_');

            int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
            int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

            //int days = DateTime.DaysInMonth(rok, mesiac);

            Control ctrl = new Control();
            ctrl = FindControl("tempWeek_" + weekTmp[1]);
            HiddenField hidden = (HiddenField)ctrl;

            string[] dtArr = hidden.Value.ToString().Split('_');

            string dateStart = rok.ToString() + "-" + mesiac.ToString() + "-" + dtArr[0];
            string dateEnd = rok.ToString() + "-" + mesiac.ToString() + "-" + dtArr[1];
            data.Add("date_start", dateStart);
            data.Add("date_end", dateEnd);
            data.Add("state", dlWeek.SelectedValue.ToString());
            data.Add("clinic", Shifts.gKlinika);

            SortedList res = Shifts.mysql.mysql_insert("is_dk_week_helper", data);

            if (!(Boolean)res["status"])
            {
                Shifts.x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
            }
            else
            {
                this.generateWeekStatus();
            }
        }
    }


    protected void publishStateFnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string id = btn.ID.ToString();
        string state = "draft";
        if (id=="publish_btn")
        {
            state = "active";
        }

        string rok = this.rok_cb.SelectedValue.ToString();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();

        int dateGroup = Shifts.x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        string query = "UPDATE [is_sluzby_all] SET [state]='{2}' WHERE [date_group]='{0}' AND [clinic]='{1}'";
        query = Shifts.mysql.buildSql(query, new string[] { dateGroup.ToString(), Shifts.gKlinika.ToString(),state });
        
        SortedList res = Shifts.mysql.execute(query);

        if (!(Boolean)res["status"])
        {
            Shifts.x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString());
        }
        else
        {
            if (Shifts.editable)
            {
                this.shiftState_lbl.Text = Resources.Resource.shifts_see_all;
            }
        }
    }

    protected void publishSluzby(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string ID = btn.ID.ToString();

        int dnesJe = DateTime.Today.Day;

        Session.Add("aktSluzMesiac", this.mesiac_cb.SelectedValue.ToString());
        Session.Add("aktSluzRok", this.rok_cb.SelectedValue.ToString());
        Session.Add("aktSluzClinic", Shifts.gKlinika);

        // Session.Add)"aktSluzMesLbl", DateTime.Today.m

        if (ID == "toWord_btn")
        {
            Session.Add("toWord", 1);
            Response.Redirect("sltoword.aspx");
        }
        if (ID == "print_btn")
        {
            Session.Add("toWord", 0);
            Response.Redirect("sltoword.aspx");
        }

    }
    

}