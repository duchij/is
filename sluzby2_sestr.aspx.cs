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
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class sluzby2_sestr : System.Web.UI.Page
{
    public mysql_db x2Mysql = new mysql_db();
    public x2_var x2 = new x2_var();
    public sluzbyclass x2Sluzby = new sluzbyclass();
    log x2log = new log();

    public string  rights = "";
    public string deps = "";
    public string[] shiftType;



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

        this.deps = Session["oddelenie"].ToString();
        this.rights = Session["rights"].ToString();
       
        if (this.rights.IndexOf("admin") != -1 || this.rights == "poweruser")
        {
            this.editShiftView_pl.Visible = true;
            this.publish_btn.Visible = true;
            this.unpublish_btn.Visible = true;
        }
        else
        {
            this.editShiftView_pl.Visible = false;
            this.publish_btn.Visible = false;
            this.unpublish_btn.Visible = false;
        }

        /*if (Session["oddelenie"].ToString().Length == 0)
        {
            this.deps = this.deps_dl.SelectedValue.ToString();
        }*/
        
        if (IsPostBack == false)
        {
            this.setMonthYear();


            //DateTime dnes = DateTime.Today;

            this.loadDeps();
            if (this.deps.Length > 0)
            {
                this.deps_dl.SelectedValue = this.deps;
            }

            string state = this.getState();
            
            if (state == "active")
            {
                this.editShift_chk.Checked = false;
            }
            else
            {
                this.editShift_chk.Checked = true;
            }

            this.drawTable();
            
            
            //this.loadSluzby();
        }
        else
        {
           // this.deps_dl.Items.Clear();
            this.shiftTable.Controls.Clear();

            /*string state = this.getState();
            if (state == "active")
            {
                this.editShift_chk.Checked = true;
            }
            else
            {
                this.editShift_chk.Checked = false;
            }*/
            
            
            this.drawTable();
            
           // this.loadDeps();
            //this.loadSluzby();
        }

        
    }

    protected string getState()
    {
        string query = @"SELECT [state] FROM [is_sluzby_2_sestr] WHERE [date_group]='{0}' AND [deps]='{1}' GROUP BY [state]";

        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());

        int dateGroup = x2.makeDateGroup(rok, mesiac);


        query = x2Mysql.buildSql(query, new string[] {dateGroup.ToString(),this.deps.ToString()});

        SortedList row = x2Mysql.getRow(query);
        string result;
        if (row["state"] != null)
        {
            result = row["state"].ToString();

        }
        else
        {
            result = "draft";
        }

        return result;
    }

    protected void changeSluzba(object sender, EventArgs e)
    {
        //this.loadSluzby();
        string state = this.getState();
        if (state == "active")
        {
            this.editShift_chk.Checked = true;
        }
        else
        {
            this.editShift_chk.Checked = false;
        }
        this.shiftTable.Controls.Clear();
        this.drawTable();
        
    }

    protected void changeDeps_fnc(object sender, EventArgs e)
    {
        string state = this.getState();
        if (state == "active")
        {
            this.editShift_chk.Checked = true;
        }
        else
        {
            this.editShift_chk.Checked = false;
        }
        this.editShift_chk.Checked = false;
        //this.loadSluzby();
            this.drawTable();
    }

    protected void setMonthYear()
    {
        DateTime dnes = DateTime.Today;
        int mesiac = dnes.Month;
        int rok = dnes.Year;

        this.mesiac_cb.SelectedValue = mesiac.ToString();
        this.rok_cb.SelectedValue = rok.ToString();
    }

    protected void loadDeps()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_deps] WHERE [clinic_id]='{0}'", Session["klinika_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        this.deps_dl.Items.Clear();
        int depsLn = table.Count;
        System.Web.UI.WebControls.ListItem[] newItem = new System.Web.UI.WebControls.ListItem[depsLn];
       // this.deps_dl.Items.Clear();
        for (int dep = 0; dep < depsLn; dep++)
        {
            if (this.rights == "admin" || this.rights == "poweruser")
            {
                this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[dep]["label"].ToString(), table[dep]["idf"].ToString()));
            }
            else
            {
                if (this.deps == table[dep]["idf"].ToString() && this.rights == "users")
                {
                    this.deps_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[dep]["label"].ToString(), table[dep]["idf"].ToString()));
                }
            }
        }
       // this.deps_dl.Items.AddRange(newItem);


    }

    protected void drawTable()
    {
        string query = @"SELECT [name],[data] FROM [is_settings] WHERE [name]='{0}'";

        query = x2Mysql.buildSql(query, new string[] {"kdch_shifts_nurse_2"});

        SortedList row = x2Mysql.getRow(query);

        string data = row["data"].ToString();
        
        this.parseMultiTable(data);
        
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
        ArrayList doctorList = this.loadDoctors(this.deps_dl.SelectedValue.ToString());
        int grps = groups.Length;
        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dayHeadCell = new TableHeaderCell();
        dayHeadCell.Text = "Datum";
        headRow.Controls.Add(dayHeadCell);

        for (int i=0; i<grps;i++)
        {
            //string[] head = groups[i].Split('_');

            TableHeaderCell headCell = new TableHeaderCell();
            headCell.HorizontalAlign = HorizontalAlign.Center;
            headCell.Text = "<center><strong>" + groups[i] + "</center></strong>";
            
            headRow.Controls.Add(headCell);
        }
        this.shiftTable.Controls.Add(headRow);

        for (int day=0; day<daysMonth; day++)
        {

            TableRow dataRow = new TableRow();

            TableCell dayCell = new TableCell();
            int rDay = day+1;

            DateTime myDate = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), rDay);
            int dnesJe = (int)myDate.DayOfWeek;
            string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
            string sviatok = rDay.ToString() + "." + myDate.Month.ToString();
            int jeSviatok = Array.IndexOf(freeDays, sviatok);



            dayCell.Text = rDay.ToString()+". "+nazov;
            dataRow.Controls.Add(dayCell);

            for (int col=0; col<grps; col++)
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
                if (ddls.Length >0)
                {
                    int dCount = ddls.Length;
                    for (int cnt=0; cnt<dCount; cnt++)
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
                            dl.Items.AddRange(newItem);
                            //dl.AutoPostBack = true;
                            //dl.SelectedIndexChanged += new EventHandler(dItemChanged_2);

                            dl.Attributes.Add("onChange", "saveNurseShifts('" + dl.ID.ToString() + "');");

                            dataCell.Controls.Add(dl);

                            TextBox comment = new TextBox();
                            comment.ID = "textBox_" + rDay.ToString() + "_" + ddls[cnt];
                            comment.Text = "-";
                           // comment.AutoPostBack = true;
                           // comment.TextChanged += new EventHandler(commentChanged_2);

                            comment.Attributes.Add("onChange", "saveNurseShiftComment('" + comment.ID.ToString() + "');");

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
                        dl1.Attributes.Add("onChange", "saveNurseShifts('" + dl1.ID.ToString() + "');");
                        dataCell.Controls.Add(dl1);

                        TextBox comment = new TextBox();
                        comment.ID = "textBox_" + rDay.ToString() + "_" + groups[col];
                        comment.Text = "-";
                        //comment.AutoPostBack = true;
                        //comment.TextChanged += new EventHandler(commentChanged_2);
                        comment.Attributes.Add("onChange", "saveNurseShiftComment('" + comment.ID.ToString() + "');");

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
            this.shiftTable.Controls.Add(dataRow);
        }
       this.setShifts();
    }

    protected void setShifts()
    {
        string query = "";
        string depsIdf = this.deps_dl.SelectedValue.ToString();

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        int dateGroup = x2.makeDateGroup(rok,mesiac);

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        if (this.rights == "admin" || this.rights == "poweruser")
        {

            query = @"   SELECT 
                            [t_sluzb].[datum], 
                            GROUP_CONCAT([typ] SEPARATOR ';') AS [type1],
                            [t_sluzb].[state] AS [state],
                            GROUP_CONCAT([t_sluzb].[user_id] SEPARATOR '|') AS [users_ids],
                            GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) SEPARATOR ';') AS [users_names],
                            GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) SEPARATOR '|') AS [comment],
                            [t_sluzb].[date_group] AS [dategroup]
                        FROM [is_sluzby_2_sestr] AS [t_sluzb]
                        LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]
                            WHERE [t_sluzb].[date_group] = '{0}'
                            AND [t_sluzb].[deps]='{1}'
                        GROUP BY [t_sluzb].[datum]
                        ORDER BY [t_sluzb].[datum]";
        }
        else
        {
            query = @"   SELECT 
                                [t_sluzb].[datum], 
                                GROUP_CONCAT([typ] SEPARATOR ';') AS [type1],
                                [t_sluzb].[state] AS [state],
                                GROUP_CONCAT([t_sluzb].[user_id] SEPARATOR '|') AS [users_ids],
                                GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) SEPARATOR ';') AS [users_names],
                                GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) SEPARATOR '|') AS [comment],
                                [t_sluzb].[date_group] AS [dategroup]
                            FROM [is_sluzby_2_sestr] AS [t_sluzb]
                        LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]
                            WHERE [t_sluzb].[date_group] = '{0}' AND [t_sluzb].[state]='active'
                            AND [t_sluzb].[deps]='{1}'
                        GROUP BY [t_sluzb].[datum]
                        ORDER BY [t_sluzb].[datum]";
        }

        int daysMonth = DateTime.DaysInMonth(rok, mesiac);


        query = x2Mysql.buildSql(query,new string[] {dateGroup.ToString(), depsIdf});

        Dictionary<int,Hashtable> table = x2Mysql.getTable(query);

        int tblCnt = table.Count;
        if (tblCnt > 0)
        {

        
            if (this.rights == "admin" || this.rights == "poweruser")
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

            for (int day=0; day<tblCnt; day++)
            {
                
                int rDay = Convert.ToDateTime(x2.UnixToMsDateTime(table[day]["datum"].ToString())).Day;
                string[] names = table[day]["users_names"].ToString().Split(';');
                string[] userId = table[day]["users_ids"].ToString().Split('|');
                string[] comments = table[day]["comment"].ToString().Split('|');
                string[] type = table[day]["type1"].ToString().Split(';');

                int tpLn = type.Length;

                if (names.Length != userId.Length)
                {
                    this.msg_lbl.Text = x2.errorMessage("Inkonzistencia v datach prosim opravit.....");
                }
                if (tpLn == 0)
                {
                    if (this.editShift_chk.Checked)
                    {
                        Control crtl = ctpl.FindControl("ddl_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        DropDownList ddl = (DropDownList)crtl;

                        try
                        {
                            ddl.SelectedValue = table[day]["users_ids"].ToString();
                        }
                        catch (Exception ex)
                        {
                            ddl.SelectedValue = "0";
                        }
                        
                        //ddl.SelectedIndexChanged += new EventHandler(dItemChanged_2);

                        Control crtl1 = ctpl.FindControl("textBox_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        TextBox textBox = (TextBox)crtl1;
                        textBox.Text = table[day]["comment"].ToString();
                        //textBox.TextChanged += new EventHandler(commentChanged_2);
                    }
                    else
                    {
                        Control crtl = ctpl.FindControl("name_" + rDay.ToString() + "_" + table[day]["type1"].ToString());

                        Label ddl = (Label)crtl;

                        try
                        {
                            ddl.Text = table[day]["user_names"].ToString() +"<br>";
                        }
                        catch (Exception ex)
                        {
                            ddl.Text = "-<br>";
                        }

                        Control crtl1 = ctpl.FindControl("comment_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        Label textBox = (Label)crtl1;
                        textBox.Text = table[day]["comment"].ToString() + "<br>";
                    }
                }
                else
                {
                    for (int col = 0; col < tpLn; col++)
                    {
                        if (this.editShift_chk.Checked)
                        {
                            Control crtl = ctpl.FindControl("ddl_" + rDay.ToString() + "_" + type[col]);
                            DropDownList ddl = (DropDownList)crtl;

                            try
                            {
                                ddl.SelectedValue = userId[col];
                            }
                            catch (Exception ex)
                            {
                                ddl.SelectedValue = "0";
                            }
                            
                            //ddl.SelectedIndexChanged += new EventHandler(dItemChanged_2);

                            Control crtl1 = ctpl.FindControl("textBox_" + rDay.ToString() + "_" + type[col]);
                            TextBox textBox = (TextBox)crtl1;
                            textBox.Text = comments[col];
                            //textBox.TextChanged += new EventHandler(commentChanged_2);
                        }
                        else
                        {
                            Control crtl = ctpl.FindControl("name_" + rDay.ToString() + "_" + type[col]);
                            Label ddl = (Label)crtl;

                            try
                            {
                                ddl.Text = names[col] + "<br>";
                            }
                            catch (Exception ex)
                            {
                                ddl.Text = "-<br>";
                            }

                            Control crtl1 = ctpl.FindControl("comment_" + rDay.ToString() + "_" + type[col]);
                            Label textBox = (Label)crtl1;
                            textBox.Text = comments[col] + "<br>";
                        }
                    }
                }
                
            }
        } 
        else
        {
            if (this.rights != "admin" || this.rights != "poweruser")
            {
                this.msg_lbl.Text = Resources.Resource.shifts_not_done;
                // this.publish_cb.Visible = false;
            }
        }
    }

    protected void selectDoctors(Dictionary<int, Hashtable>table, int days, int colsNum)
    {
        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;
        for (int row = 0; row < days; row++)
        {
            string[] userId = table[row]["users_ids"].ToString().Split('|');

            for (int col=0; col < colsNum; col++)
            {
                Control controlList = new Control();
                controlList = ctpl.FindControl("dlistBox_"+row.ToString()+"_"+col.ToString());

                DropDownList doctors_lb = new DropDownList();

                doctors_lb = (DropDownList)controlList;
               // ListItem selectedListItem = doctors_lb.Items.FindByValue(userId[col].ToString());
              //  selectedListItem.Selected = true;
               // doctors_lb.SelectedIndex = doctors_lb.Items.IndexOf(doctors_lb.Items.FindByValue(userId[col]));
               doctors_lb.SelectedValue = userId[col];

              //  doctors_lb.s

            }
        }
    }

    protected string returnString(string data)
    {
        string result = "";
        
        if (data.GetType() == typeof(System.Byte[]))
        {
           // result = reader.GetValue(i).ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
        else
        {
            result = data;
        }

        return result;        
    }

    protected void reDrawTableFnc(object sender, EventArgs e)
    {
    }

    protected ArrayList loadDoctors(string sDeps)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [kdch_nurse] WHERE ([idf]='{0}')  ORDER BY [name2]",sDeps);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int dataLn = table.Count;

        ArrayList result = new ArrayList();
        //result.Add("-", "-");
        result.Add("0|-");
        for (int i=1; i <= dataLn; i++)
        {
            result.Add(table[i-1]["id"].ToString()+"|"+table[i-1]["name3"].ToString());
        }

        return result;

    }

    /*protected void commentChanged(object sender, EventArgs e)
    {

        TextBox tbox = (TextBox)sender;

        string[] tmp = tbox.ID.ToString().Split('_');

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        Control controlList1 = ctpl.FindControl("textBox_"+tmp[1]+"_"+tmp[2]);
        Control controlList2 = ctpl.FindControl("ddl_" + tmp[1] + "_" + tmp[2]);

        TextBox tBoxF = (TextBox)controlList1;
        DropDownList doctor_lb = (DropDownList)controlList2;


        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", this.shiftType[col]);
        data.Add("ordering", col + 1);
        data.Add("deps", this.deps);
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }


        data.Add("date_group", rok + mesiac);
        int den = Convert.ToInt32(tmp[1]);
        den = den + 1;

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());
        data.Add("comment", tBoxF.Text.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_2_sestr", data);

        Boolean res = Convert.ToBoolean(result["status"].ToString());

        if (!res)
        {
            this.msg_lbl.Text = x2.errorMessage(result["msg"].ToString());
        }



       // this.msg_lbl.Text = e.ToString() + "..." + tBoxF.Text.ToString();
    }*/

    protected void commentChanged_2(object sender, EventArgs e)
    {

        TextBox tbox = (TextBox)sender;

        string[] tmp = tbox.ID.ToString().Split('_');

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        Control controlList1 = ctpl.FindControl("textBox_" + tmp[1] + "_" + tmp[2]);
        Control controlList2 = ctpl.FindControl("ddl_" + tmp[1] + "_" + tmp[2]);

        TextBox tBoxF = (TextBox)controlList1;
        DropDownList doctor_lb = (DropDownList)controlList2;


        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        //int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", tmp[2]);
        //data.Add("ordering", col + 1);
        data.Add("deps", this.deps);
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();
        int dateGroup = x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        data.Add("date_group", dateGroup);
        int den = Convert.ToInt32(tmp[1]);

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());

        string com = tBoxF.Text.ToString().Trim();

        if (com.Length == 0) com = "-";
        
        data.Add("comment", tBoxF.Text.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_2_sestr", data);

        if (!(Boolean)result["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(result["msg"].ToString());
        }
    }

    /*protected void dItemChanged(object sender, EventArgs e)
    {
        this.msg_lbl.Text = e.ToString();

        DropDownList ddl = (DropDownList)sender;

        string[] tmp = ddl.ID.ToString().Split('_');


        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        Control controlList = ctpl.FindControl("ddl_"+tmp[1]+"_"+tmp[2]);

        DropDownList doctor_lb = (DropDownList)controlList;

        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", tmp[2]);
        data.Add("ordering", col+1);

        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        data.Add("deps", this.deps);
        data.Add("date_group", rok+mesiac);
        int den = Convert.ToInt32(tmp[1]);
        den = den +1;

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_2_sestr", data);

        
        if (!(Boolean)result["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(result["msg"].ToString());
        }
        
        
    }*/

    protected void dItemChanged_2(object sender, EventArgs e)
    {

        DropDownList ddl = (DropDownList)sender;

        string[] tmp = ddl.ID.ToString().Split('_');


        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        Control controlList = ctpl.FindControl("ddl_" + tmp[1] + "_" + tmp[2]);

        DropDownList doctor_lb = (DropDownList)controlList;

        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        //int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", tmp[2]);
        //data.Add("ordering", col + 1);

        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        int dateGroup = x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        data.Add("deps", this.deps);
        data.Add("date_group", dateGroup);
        int den = Convert.ToInt32(tmp[1]);
        //den = den + 1;

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_2_sestr", data);

        if (!(Boolean)result["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(result["msg"].ToString());
        }


    }



    protected void publishOnFnc(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        string rok = this.rok_cb.SelectedValue.ToString();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();

        int dateGroup = x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        sb.AppendFormat("UPDATE [is_sluzby_2_sestr] SET [state]='active' WHERE [date_group]='{0}' AND [deps]='{1}'", dateGroup,this.deps);
        SortedList res = x2Mysql.execute(sb.ToString());

        if (!(Boolean)res["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }
        else
        {
            if (this.rights == "admin" || this.rights == "poweruser")
            {
                this.shiftState_lbl.Text = Resources.Resource.shifts_see_all;
            }
        }
    }

    protected void publishOffFnc(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        string rok = this.rok_cb.SelectedValue.ToString();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();

        int dateGroup = x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        sb.AppendFormat("UPDATE [is_sluzby_2_sestr] SET [state]='draft' WHERE [date_group]='{0}' AND [deps]='{1}'", dateGroup, this.deps);
        SortedList res = x2Mysql.execute(sb.ToString());

        if (!(Boolean)res["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }
        else
        {
            if (this.rights == "admin" || this.rights == "poweruser")
            {
                this.shiftState_lbl.Text = Resources.Resource.shifts_see_limited;
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
    
    protected void generate_nurse_plan_fnc(object sender, EventArgs e)
    {
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());

        string odd = this.deps_dl.SelectedValue.ToString();
        
        string query = @"
                        SELECT [t_users.full_name] AS [name], [t_sluzb.user_id] AS [user_id]
                           ,GROUP_CONCAT([t_sluzb.typ] ORDER BY [t_sluzb.datum] SEPARATOR ';' ) AS [plan] 
                           ,GROUP_CONCAT(DATE_FORMAT([t_sluzb.datum],'%Y-%c-%e') ORDER BY [t_sluzb.datum] SEPARATOR ';' ) AS [datum]
                        FROM [is_sluzby_2_sestr] AS [t_sluzb]
                            INNER JOIN [is_users] AS [t_users] ON [t_users.id] = [t_sluzb.user_id]
                        WHERE [t_sluzb.date_group]='{0}' AND [deps]='{1}' 
                        GROUP BY [t_sluzb.user_id]
                        ORDER BY [t_users.work_group] 
                        ";

        query = x2Mysql.buildSql(query,new string[] {x2.makeDateGroup(rok,mesiac).ToString(),odd});

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        this.makePdf(table);

    }

    protected Dictionary<string, ArrayList> getVacations(int mesiac, int rok)
    {
        int days = DateTime.DaysInMonth(rok, mesiac);

        string query = @"SELECT [user_id],[od],
                            [do],[type] FROM [is_dovolenky_sestr]
                            WHERE [od] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00'
                            OR [do] BETWEEN '{0}-{1}-01 00:00:00' AND '{0}-{1}-{2} 23:59:00'
                            ORDER BY [do] ASC";

        query = x2Mysql.buildSql(query, new string[] { rok.ToString(), mesiac.ToString(), days.ToString()});

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);
        Dictionary<string, ArrayList> result = new Dictionary<string,ArrayList>();

        int tblLn = table.Count;

        for (int i=0; i< tblLn; i++)
        {
            if (result.ContainsKey(table[i]["user_id"].ToString()) == false)
            {
                result.Add(table[i]["user_id"].ToString(), new ArrayList());
            }

           
           string tmp = table[i]["od"].ToString() + ";" + table[i]["do"].ToString() + ";" + table[i]["type"].ToString();


            result[table[i]["user_id"].ToString()].Add(tmp);


        }

        return result;

    }

    protected void writePage(Rectangle size,ref PdfContentByte cb, Dictionary<int, Hashtable> table, int pageNum)
    {

        
        //rotate 270 degree hardcoded
        //cb.AddTemplate(page, -1f, 0, 0, -1f, reader.GetPageSizeWithRotation(1).Width,  reader.GetPageSizeWithRotation(1).Height);
        //cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, page.Width, 0);
        //cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);

        //PdfDictionary pdfDic = new PdfDictionary();
        //pdfDic = reader.GetPageN(1);
        //pdfDic.Put(PdfName.ROTATE, new PdfNumber(rot + 90));
        //PdfStamper stamp = new PdfStamper(reader,new )
        BaseFont mojFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
        cb.SetFontAndSize(mojFont, 9);


        float startRowY = size.Height - 189; //126 start
        float startNameX = (float)28.51;
        float startPlanX = (float)173.8;
        float nameDistance = (float)23.66;
        float planDistance = (float)15.38;
        //float planEleDistance = (float)7.27;


        float dateX = (float)632.16;
        float dateY = size.Height - (float)30.53;


        float madeByX = (float)613.81;
        float madeByY = size.Height - (float)56.66;

        float depX = (float)59.43;
        float depY = size.Height - (float)42.65;

        float clinicX = (float)88.25;
        float clinicY = size.Height - (float)53.09;


        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);



        //rok a mesiac
        cb.BeginText();
        cb.MoveText(dateX, dateY);
        cb.ShowText(mesiac.ToString() + " / " + rok.ToString());
        cb.EndText();
        //kto spravil
        cb.BeginText();
        cb.MoveText(madeByX, madeByY);
        cb.ShowText(Session["fullname"].ToString());
        cb.EndText();
        //oddelenie
        cb.BeginText();
        cb.MoveText(depX, depY);
        cb.ShowText(this.deps_dl.SelectedItem.ToString());
        cb.EndText();
        //Klinika
        cb.BeginText();
        cb.MoveText(clinicX, clinicY);
        cb.ShowText(Session["clinic_label"].ToString()); ;
        cb.EndText();

        Dictionary<string, ArrayList> vacations = this.getVacations(mesiac, rok);


        int days = DateTime.DaysInMonth(rok, mesiac);

        cb.SetColorStroke(BaseColor.LIGHT_GRAY);
        cb.SetColorFill(BaseColor.LIGHT_GRAY);

        string[] freeDays = Session["freedays"].ToString().Split(',');

        double kof = 12.4;
        for (int i = 0; i < days; i++)
        {
            int den = i + 1;
            /*if (i == 1)
            {
                cb.Rectangle(46, size.Height-(float)odHora, 10, 10);
                cb.Fill();
            }*/

            string mesDen = den.ToString() + "." + mesiac.ToString();

            int rs_tmp = Array.IndexOf(freeDays, mesDen);

            DateTime my_date = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), den);
            int dnesJe = (int)my_date.DayOfWeek;

            if (dnesJe == 0 || dnesJe == 6 || rs_tmp != -1)
            {
                //173.22
                //vyska stlpca je 11
                //od lava 46
                //dlzka je 423.7

                /* cb.MoveTo(46, size-odHora +(11*i));
                 cb.LineTo(469, size - odHora + (11 * i));
                 cb.LineTo(469, size - odHora + (11 * i)-11);
                 cb.LineTo(46, size - odHora + (11 * i) - 11);
                 //Path closed, strokend filled
                 cb.ClosePathFillStroke();*/
                double recY = (size.Height - (startRowY + 10)) + (planDistance * i);

                float recYY = (float)recY;

                cb.Rectangle(startPlanX + (i * planDistance), (float)179.40, (float)13.96, (float)235.97);
                //cb.Stroke();
                cb.Fill();
            }
        }
        cb.SetColorStroke(BaseColor.BLACK);
        cb.SetColorFill(BaseColor.BLACK);

        cb.SetFontAndSize(mojFont, 9);

        int tbLn = table.Count;

        //int st = pageNum + 10;
        int from = pageNum*10;

        int to = from+10;
        if (to > tbLn)
        {
            to = tbLn;
        }

        int position = 0;
        for (int j = from; j < to; j++)
        {
            
            string[] nm = table[j]["name"].ToString().Split(' ');
            cb.BeginText();
            cb.MoveText(startNameX, startRowY - (position * nameDistance));

            cb.ShowText(nm[0]);
            cb.EndText();

            cb.BeginText();
            cb.MoveText(startNameX, startRowY - (position * nameDistance) - 9);

            cb.ShowText(nm[1]);
            cb.EndText();

            string[] sluzby = table[j]["plan"].ToString().Split(';');
            string[] dni = table[j]["datum"].ToString().Split(';');

            //int slLn = sluzby.Length;
            for (int p = 0; p < days; p++)
            {

                string unixDate = rok.ToString() + "-" + mesiac.ToString() + "-" + (p + 1).ToString();
                int pos = -1;
                int counts = table[j]["datum"].ToString().Length;
                if (counts == 1)
                {
                    pos = 1;
                }
                else
                { 
                    pos = Array.IndexOf(dni, unixDate);
                }


                if (pos != -1)
                {
                    cb.BeginText();
                    cb.MoveText(startPlanX + (p * planDistance), startRowY - (position * nameDistance));

                    string dat;

                    if (counts == 1)
                    {
                        dat = table[j]["datum"].ToString();
                    }
                    else
                    {
                        dat = sluzby[pos];
                    }

                    switch (dat)
                    {
                        case "A1":
                            cb.ShowText("D");
                            break;
                        case "A2":
                            cb.ShowText("N");
                            break;
                        case "S1":
                            cb.ShowText("R");
                            break;
                        case "S2":
                            cb.ShowText("R");
                            break;
                        case "RA":
                            cb.ShowText("R1");
                            break;
                        case "RA2":
                            cb.ShowText("R2");
                            break;
                        default:
                            cb.ShowText(sluzby[pos].Substring(0, 1));
                            break;

                    }
                    cb.EndText();
                }
            }
            if (vacations.ContainsKey(table[j]["user_id"].ToString()))
            {
                ArrayList userVacations = vacations[table[j]["user_id"].ToString()];

                int uVLn = userVacations.Count;

                for (int v = 0; v < uVLn; v++)
                {
                    string[] tmp = userVacations[v].ToString().Split(';');

                    DateTime dtStart = Convert.ToDateTime(tmp[0]);
                    DateTime dtStop = Convert.ToDateTime(tmp[1]);

                    int mStart = dtStart.Month;
                    int mStop = dtStop.Month;

                    if (mStart != mStop)
                    {
                        if (mStart < mesiac)
                        {
                            int daysInMonth = DateTime.DaysInMonth(rok, mesiac);
                            //string endDt = days.ToString() + "." + mesiac.ToString() + "." + rok.ToString();
                            string DtStr = "1." + mesiac.ToString() + "." + rok.ToString();
                            dtStart = Convert.ToDateTime(DtStr);
                        }
                        if (mStop > mesiac )
                        {
                            int daysInMonth = DateTime.DaysInMonth(rok, mesiac);
                            string DtStr = days.ToString() + "." + mesiac.ToString() + "." + rok.ToString();
                            dtStop = Convert.ToDateTime(DtStr);
                        }
                    }

                   

                    while (dtStart <= dtStop)
                    {
                        
                        int day = dtStart.Day;

                        int dW = (int)dtStart.DayOfWeek;

                        string denStr = day.ToString() + "." + mesiac.ToString();
                        if (Array.IndexOf(freeDays, denStr) == -1 && dW != 0 && dW != 6)
                        {
                            cb.BeginText();
                            cb.MoveText(startPlanX + ((day - 1) * planDistance), startRowY - (position * nameDistance));
                            switch (tmp[2])
                            {
                                case "do":
                                    cb.ShowText("Dv");
                                    break;
                                case "pn":
                                    cb.ShowText("PN");
                                    break;
                                case "le":
                                    cb.ShowText("Le");
                                    break;
                                default:
                                    cb.ShowText(tmp[2]);
                                    break;

                            }

                            cb.EndText();
                        }
                        dtStart = dtStart.AddDays(1);
                    }
                }
            }
            position++;   
        }
        //return cb;
        
    }

    protected void makePdf(Dictionary<int, Hashtable> table)
    {
        int milis = DateTime.Now.Millisecond;
        string path = Server.MapPath("App_Data");
        string imagepath = Server.MapPath("App_Data");
        string oldFile = @path + @"\templates\nurse\plan10.pdf";
        string hash = x2.makeFileHash(Session["login"].ToString() + milis.ToString());
        string newFileName = "plan_" + hash + ".pdf";
        string newFile = @path + @"\" + newFileName;
        this.msg_lbl.Text = oldFile;
        // open the reader
        PdfReader reader = new PdfReader(oldFile);


        Rectangle size = reader.GetPageSizeWithRotation(1);
        Document myDoc = new Document(size, 0, 0, 0, 0);
        myDoc.SetPageSize(size);
        float rot = reader.GetPageRotation(1);
        //reader.Pa


        //1cm == 28.3pt



        // open the writer
        FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
        PdfWriter writer = PdfWriter.GetInstance(myDoc, fs);
        myDoc.Open();
        PdfImportedPage page = writer.GetImportedPage(reader, 1);
        // the pdf content
        //PdfWriter pw = writer.DirectContent;
        PdfContentByte cb = writer.DirectContent;
        


        int tbLn = table.Count;
        double parts = tbLn / 10;
        int modulo = tbLn % 10;
         
        int pages = 0;

        if (parts <= 0)
        {
            pages = 1;
        }
        if (parts >0 && modulo == 0)
        {
            pages = Convert.ToInt32(parts);
        }
        if (parts >0 && modulo > 0)
        {
            pages = Convert.ToInt32(parts) + 1;
        }

        
        int cnt = 0;
        int namePos = 0;
        while (cnt < pages)
        {
            if (cnt == 0)
            {
                cb.AddTemplate(page, 0, 0);
                this.writePage(size, ref cb, table,cnt);
                
            }
            else
            {
               myDoc.NewPage();
               cb.AddTemplate(page, 0, 0);
               this.writePage(size, ref cb, table,cnt);
            }
            cnt++;
        }


        myDoc.Close();
        fs.Close();
        writer.Close();
        reader.Close();

        SortedList res = x2Mysql.registerTempFile(newFileName, 5,"");


        if ((Boolean)res["status"])
        {

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=plan.pdf");
            Response.TransmitFile(newFile);
            Response.End();
        }
        else
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }

    }

    protected void writeData(PdfContentByte cb, SortedList data)
    {

    }
}