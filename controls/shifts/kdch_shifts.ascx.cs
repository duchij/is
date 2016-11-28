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

public partial class controls_kdch_shifts : System.Web.UI.UserControl
{
    public mysql_db x2Mysql = new mysql_db();
    public x2_var x2 = new x2_var();
    public sluzbyclass x2Sluzby = new sluzbyclass();
    log x2log = new log();

    public string  rights = "";
    public string deps = "";
    public string[] shiftType;
    public string gKlinika = "";
    public string clinicIdf="kdch";

    private string state = "";
    Boolean userControl = false;


    protected void Page_Init(object sender, EventArgs e)
    {
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        if (!IsPostBack)
        {
            x2.fillYearMonth(ref this.mesiac_cb, ref this.rok_cb, Session["month_dl"].ToString(), Session["years_dl"].ToString());
        }
       

        this.msg_lbl.Text = "";

        this.deps = Session["oddelenie"].ToString();
        this.rights = Session["rights"].ToString();
        this.gKlinika = Session["klinika_id"].ToString().ToLower();
       // this.editShiftView_pl.Visible = true;

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
        
       

        if (IsPostBack == false)
        {
            this.setMonthYear();
            this.state = this.getState();

            if (this.state == "active")
            {
                this.editShift_chk.Checked = false;
            }
            else
            {
                this.editShift_chk.Checked = true;
            }



            this.drawTable();
        }
        else
        {
           

            this.shiftTable.Controls.Clear();
            this.drawTable();
        }

        
    }

    protected string getState()
    {
        string query = @"SELECT [state] FROM [is_sluzby_2] WHERE [date_group]='{0}' AND [clinic]='{1}' GROUP BY [state]";

        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());

        int dateGroup = x2.makeDateGroup(rok, mesiac);


        query = x2Mysql.buildSql(query, new string[] {dateGroup.ToString(),this.gKlinika.ToString()});

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
        string state = this.getState();
        if (state == "active")
        {
            this.editShift_chk.Checked = true;
        }
        else
        {
            this.editShift_chk.Checked = false;
        }
		
		int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
		int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
		int days = DateTime.DaysInMonth(rok, mesiac);
		this.days_lbl.Text = days.ToString();
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
        this.drawTable();
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

        string idf = this.clinicIdf + "_shift_doctors";

        query = x2Mysql.buildSql(query, new string[] { idf });

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
            string headLbl = groups[i];
            if (headLbl.IndexOf(",")!=-1)
            {
                   headLbl = headLbl.Replace(",", "<br>");
            }
            headCell.Text = "<center><strong>" + headLbl + "</center></strong>";
            
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

            if (dnesJe == 0 || dnesJe == 6)
            {
                dayCell.CssClass = "box red";
            }

            if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
            {
                dayCell.CssClass = "box yellow";
            }

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
                if (ddls.Length > 0)
                {
                    int dCount = ddls.Length;
                    for (int cnt = 0; cnt < dCount; cnt++)
                    {

                        if (this.editShift_chk.Checked && (this.rights.IndexOf("admin") != -1 || this.rights == "poweruser"))
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
                            dl.Attributes.Add("onChange", "saveKDCHDocShifts('" + dl.ID.ToString()+"');");
                            dataCell.Controls.Add(dl);

                            TextBox comment = new TextBox();
                            comment.ID = "textBox_" + rDay.ToString() + "_" + ddls[cnt];
                            comment.Text = "-";
                            comment.Attributes.Add("onChange", "saveKDCHDocShiftComment('" + comment.ID.ToString() + "');");
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
                    if (this.editShift_chk.Checked && (this.rights.IndexOf("admin") != -1 || this.rights == "poweruser"))
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
                        dl1.Attributes.Add("onChange", "saveKDCHDocShifts('" + dl1.ID.ToString() + "');");
                        dataCell.Controls.Add(dl1);

                        TextBox comment = new TextBox();
                        comment.ID = "textBox_" + rDay.ToString() + "_" + groups[col];
                        comment.Text = "-";
                        //comment.AutoPostBack = true;
                        //comment.TextChanged += new EventHandler(commentChanged_2);
                        comment.Attributes.Add("onChange", "saveKDCHDocShiftComment('" + comment.ID.ToString() + "');");
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
            if (rDay==DateTime.Today.Day && month == DateTime.Today.Month )
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
        string depsIdf = this.deps_dl.SelectedValue.ToString();

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        int dateGroup = x2.makeDateGroup(rok,mesiac);

       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        if (this.rights.IndexOf("admin")!=-1 || this.rights == "poweruser")
        {

            query =  @"SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],
                            [t_sluzb].[state] AS [state],
                            GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],
                            GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],
            GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],
            [t_sluzb].[date_group] AS [dategroup] 
            FROM [is_sluzby_2] AS [t_sluzb]
            LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id] 
            WHERE [t_sluzb].[date_group] = '{0}' 
            AND [t_sluzb].[clinic] = '{1}' 
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
                FROM [is_sluzby_2] AS [t_sluzb]
                LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]
                    WHERE [t_sluzb].[date_group] = '{0}' 
                -- AND [t_sluzb].[state]='active'
                AND [t_sluzb].[clinic] = '{1}' 
                GROUP BY [t_sluzb].[datum]
                ORDER BY [t_sluzb].[datum]";
        }

        int daysMonth = DateTime.DaysInMonth(rok, mesiac);


        query = x2Mysql.buildSql(query,new string[] {dateGroup.ToString(), this.gKlinika});

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
                    x2.errorMessage2(ref this.msg_lbl,"Inkonzistencia v datach prosim opravit.....");
                }
                if (tpLn == 0)
                {
                    if (this.editShift_chk.Checked && (this.rights.IndexOf("admin") != -1 || this.rights == "poweruser"))
                    {
                        Control crtl = FindControl("ddl_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        DropDownList ddl = (DropDownList)crtl;

                        ddl.SelectedValue = table[day]["users_ids"].ToString();
                       // ddl.SelectedIndexChanged += new EventHandler(dItemChanged_2);
                        ddl.Attributes.Add("onChange", "saveKDCHDocShifts('" + ddl.ID.ToString() + "');");

                        if (this.userControl)
                        {
                            if (this.rights == "users" && (Session["user_id"].ToString() != table[day]["users_ids"].ToString()))
                            {
                                ddl.Enabled = false;
                            }
                            else
                            {
                                if (this.rights == "users")
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
                        textBox.Attributes.Add("onChange", "saveKDCHDocShiftComment('" + textBox .ID.ToString() + "');");

                        if (this.userControl)
                        {
                            if (this.rights == "users" && (Session["user_id"].ToString() != table[day]["users_ids"].ToString()))
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
                            ddl.Text = table[day]["user_names"].ToString() +"<br>";
                        }
                        catch (Exception ex)
                        {
                            ddl.Text = "-<br>";
                        }

                        Control crtl1 = FindControl("comment_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                        Label textBox = (Label)crtl1;
                        try
                        {
                            textBox.Text ="<p class='small'>"+ table[day]["comment"].ToString() + "</p>";
                        }
                        catch(Exception ex)
                        {
                            textBox.Text = "<p class='small'>-</p>";
                        }


                        
                    }
                }
                else
                {
                    for (int col = 0; col < tpLn; col++)
                    {
                        if (this.editShift_chk.Checked && (this.rights.IndexOf("admin") != -1 || this.rights == "poweruser"))
                        {
                            Control crtl = FindControl("ddl_" + rDay.ToString() + "_" + type[col]);
                            DropDownList ddl = (DropDownList)crtl;

                            ddl.SelectedValue = userId[col];
                            if (this.userControl)
                            {
                                if (this.rights == "users" && (Session["user_id"].ToString() != userId[col].ToString()))
                                {
                                    ddl.Enabled = false;
                                }
                                else
                                {
                                    if (this.rights == "users")
                                    {
                                        System.Web.UI.WebControls.ListItem data = ddl.Items.FindByValue(Session["user_id"].ToString());
                                        ddl.Items.Clear();
                                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
                                        ddl.Items.Add(data);
                                    }

                                }
                            }



                            //ddl.SelectedIndexChanged += new EventHandler(dItemChanged_2);

                            ddl.Attributes.Add("onChange", "saveKDCHDocShifts('" + ddl.ID.ToString() + "');");


                            Control crtl1 = FindControl("textBox_" + rDay.ToString() + "_" + type[col]);
                            TextBox textBox = (TextBox)crtl1;
                            textBox.Text = comments[col];
                            if (this.userControl)
                            {
                                if (this.rights == "users" && (Session["user_id"].ToString() != userId[col]))
                                {
                                    textBox.ReadOnly = true;
                                }
                            }

                            // textBox.TextChanged += new EventHandler(commentChanged_2);
                            textBox.Attributes.Add("onChange", "saveKDCHDocShiftComment('" + textBox.ID.ToString() + "');");
                        }
                        else
                        {
                            Control crtl = FindControl("name_" + rDay.ToString() + "_" + type[col]);
                            Label ddl = (Label)crtl;

                            try
                            {
                                ddl.Text = names[col] +"<br><em>("+type[col]+ ")</em><br>";
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
                doctors_lb.SelectedValue = userId[col];
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

        //if (this.rights == "users")
        //{
        //    sb.AppendFormat("SELECT [id],[name3] FROM [is_users] WHERE [id]='{0}'", Session["user_id"].ToString());
       // }
        //else
        //{
            sb.AppendFormat("SELECT [id],[name3] FROM [is_users] WHERE [work_group]='doctor' AND [active]='1' AND [klinika]='{0}' ORDER BY [name2]", this.gKlinika);
        //}
        

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
       

    protected void commentChanged_2(object sender, EventArgs e)
    {

        TextBox tbox = (TextBox)sender;

        string[] tmp = tbox.ID.ToString().Split('_');

       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        //Control controlList1 = ctpl.FindControl("textBox_" + tmp[1] + "_" + tmp[2]);
        Control controlList1 = FindControl("textBox_" + tmp[1] + "_" + tmp[2]);
       // Control controlList2 = ctpl.FindControl("ddl_" + tmp[1] + "_" + tmp[2]);
        Control controlList2 = FindControl("ddl_" + tmp[1] + "_" + tmp[2]);

        TextBox tBoxF = (TextBox)controlList1;
        DropDownList doctor_lb = (DropDownList)controlList2;


        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        //int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", tmp[2]);
        //data.Add("ordering", col + 1);
        //data.Add("deps", this.deps);
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();
        int dateGroup = x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        data.Add("date_group", dateGroup);
        data.Add("clinic", this.gKlinika);
        int den = Convert.ToInt32(tmp[1]);

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());

        string com = tBoxF.Text.ToString().Trim();

        if (com.Length == 0) com = "-";
        
        if (tBoxF.Text.ToString().Trim().Length == 0)
        {
            data.Add("comment", "-");
        }
        else
        {
            data.Add("comment", tBoxF.Text.ToString());
        }
        

        SortedList result = x2Mysql.mysql_insert("is_sluzby_all", data);

        if (!(Boolean)result["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(result["msg"].ToString());
        }
    }

 

    protected void dItemChanged_2(object sender, EventArgs e)
    {

        DropDownList ddl = (DropDownList)sender;

        string[] tmp = ddl.ID.ToString().Split('_');


        //Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

      //  ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        //Control controlList = ctpl.FindControl("ddl_" + tmp[1] + "_" + tmp[2]);
        Control controlList = FindControl("ddl_" + tmp[1] + "_" + tmp[2]);
        DropDownList doctor_lb = (DropDownList)controlList;

        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        //int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", tmp[2]);
        data.Add("clinic", this.gKlinika);
        //data.Add("ordering", col + 1);

        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        int dateGroup = x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        //data.Add("deps", this.deps);
        data.Add("date_group", dateGroup);
        int den = Convert.ToInt32(tmp[1]);
        //den = den + 1;

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_all", data);

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

        sb.AppendFormat("UPDATE [is_sluzby_all] SET [state]='active' WHERE [date_group]='{0}' AND [clinic]='{1}'", dateGroup,this.gKlinika);
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

        sb.AppendFormat("UPDATE [is_sluzby_2] SET [state]='draft' WHERE [date_group]='{0}' AND [clinic]='{1}'", dateGroup, this.gKlinika);
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
        Session.Add("aktSluzClinic", this.gKlinika);

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