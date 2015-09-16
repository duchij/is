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
            this.loadDeps();
            if (this.deps.Length > 0)
            {
                this.deps_dl.SelectedValue = this.deps;
            }
            this.loadSluzby();
        }
        else
        {
           // this.deps_dl.Items.Clear();
            this.shiftTable.Controls.Clear();
           // this.loadDeps();
            this.loadSluzby();
        }

        
    }

    protected void changeSluzba(object sender, EventArgs e)
    {
        this.loadSluzby();
    }

    protected void changeDeps_fnc(object sender, EventArgs e)
    {
        this.editShift_chk.Checked = false;
        this.loadSluzby();
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

    protected void loadSluzby()
    {

        this.shiftTable.Controls.Clear();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        
        int daysMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        string dateGroup = rok+mesiac;
        Session.Add("aktDateGroup", dateGroup);

        SortedList res = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name] = '"+Session["klinika"].ToString()+"_shifts_nurse'");
        
        // Boolean status = Convert.ToBoolean(res["status"].ToString());

        string[] shifts = res["data"].ToString().Split(',');
        this.shiftType = shifts;

        string depsIdf = this.deps_dl.SelectedValue.ToString();

        if (this.deps.Length == 0)
        {
            this.deps = depsIdf;
        }
        
        StringBuilder sb = new StringBuilder();

        if (this.rights == "admin" || this.rights == "poweruser")
        {

            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_2_sestr] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}{1}'", rok, mesiac);
            sb.AppendFormat("AND [t_sluzb].[deps]='{0}'", depsIdf); 
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum]");
        }
        else
        {
            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_2_sestr] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}{1}' AND [t_sluzb].[state]='active'", rok, mesiac);
            sb.AppendFormat("AND [t_sluzb].[deps]='{0}'", depsIdf);
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum]");
        }

      //  this.msg_lbl.Text = sb.ToString();

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

       
        if (table.Count == daysMonth)
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

            string[] header = shifts;
         
            int days = table.Count;
            int colsNum = header.Length;

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
                headCell1.Text = "<strong><center>" + header[head].ToString() + "</center></strong>";
                // headCell1.
                headRow.Controls.Add(headCell1);
            }
            /* TableHeaderCell headCellSave = new TableHeaderCell();
             headCellSave.ID = "headCellSave_" + colsNum;
             headCellSave.Text = "<strong>Ulozit</strong>";
             headRow.Controls.Add(headCellSave);*/

            shiftTable.Controls.Add(headRow);

            string[] freeDays = x2Sluzby.getFreeDays();

            ArrayList doctorList = this.loadDoctors(this.deps_dl.SelectedValue.ToString());

            int aktDenMesiac = DateTime.Today.Day;
            Boolean editShifts = this.editShift_chk.Checked;

            for (int row = 0; row < days; row++)
            {
                TableRow tblRow = new TableRow();
                shiftTable.Controls.Add(tblRow);

                string[] names = table[row]["users_names"].ToString().Split(';');
                string[] userId = table[row]["users_ids"].ToString().Split('|');
                string[] comments = table[row]["comment"].ToString().Split('|');

                DateTime myDate = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), row + 1);
                int dnesJe = (int)myDate.DayOfWeek;
                string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                string sviatok = (row + 1).ToString() + "." + myDate.Month.ToString();
                int jeSviatok = Array.IndexOf(freeDays, sviatok);
                TableCell cellDate = new TableCell();
                tblRow.Controls.Add(cellDate);
                // cellDate.ID = "cellDate_" + row;
                if (dnesJe == 0 || dnesJe == 6)
                {
                    cellDate.CssClass = "box red";
                }

                if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                {
                    cellDate.CssClass = "box yellow";
                }
                string text = (row + 1).ToString();
                cellDate.Text = text + ". " + nazov;


                for (int cols = 0; cols < colsNum; cols++)
                {
                    TableCell dataCell = new TableCell();
                    // tblRow.Controls.Add(dataCell);
                    // dataCell.ID = "dataCell_" + row.ToString() + "_" + cols.ToString();

                    // if (cols < colsNum)
                    //  {


                   // if ((this.rights == "admin" || this.rights == "poweruser") && editShifts)
                    if (this.rights == "admin" || this.rights == "poweruser")
                    {
                        DropDownList doctors_lb = new DropDownList();
                        doctors_lb.ID = "ddl_" + row.ToString() + "_" + cols.ToString();
                        //doctors_lb.CssClass = "no-pad-mobile no-gap-mobile";
                        
                        int listLn = doctorList.Count;
                        System.Web.UI.WebControls.ListItem[] newItem =new System.Web.UI.WebControls.ListItem[listLn];

                        for (int doc = 0; doc < listLn; doc++)
                        {
                            string[] tmp = doctorList[doc].ToString().Split('|');
                            newItem[doc] = new System.Web.UI.WebControls.ListItem(tmp[1].ToString(), tmp[0].ToString());
                        }
                        doctors_lb.Items.AddRange(newItem);

                        if (cols >= 0 && cols < names.Length )
                        {
                            string dd = userId[cols].ToString();

                            doctors_lb.SelectedValue = dd; // userId[cols].ToString();
                            doctors_lb.ToolTip = names[cols].ToString();
                            dataCell.Controls.Add(doctors_lb);
                            //doctors_lb.SelectedIndex = doctors_lb.Items.IndexOf(doctors_lb.Items.FindByValue(userId[cols]));

                            doctors_lb.AutoPostBack = true;
                            doctors_lb.SelectedIndexChanged += new EventHandler(dItemChanged);
                            //doctors_lb.SelectedValue = "-";

                            TextBox textBox = new TextBox();
                            dataCell.Controls.Add(textBox);
                            textBox.ID = "textBox_" + row.ToString() + "_" + cols.ToString();
                            textBox.Text = comments[cols];
                            textBox.AutoPostBack = true;
                            textBox.TextChanged += new EventHandler(commentChanged);
                        }
                        else
                        {
                            doctors_lb.SelectedValue = "-"; // userId[cols].ToString();
                            doctors_lb.ToolTip = "-";
                            dataCell.Controls.Add(doctors_lb);
                            //doctors_lb.SelectedIndex = doctors_lb.Items.IndexOf(doctors_lb.Items.FindByValue(userId[cols]));

                            doctors_lb.AutoPostBack = true;
                            doctors_lb.SelectedIndexChanged += new EventHandler(dItemChanged);
                            //doctors_lb.SelectedValue = "-";

                            TextBox textBox = new TextBox();
                            dataCell.Controls.Add(textBox);
                            textBox.ID = "textBox_" + row.ToString() + "_" + cols.ToString();
                            textBox.Text = "-";
                            textBox.AutoPostBack = true;
                            textBox.TextChanged += new EventHandler(commentChanged);
                        }
                        
                    }
                    else
                    {
                        Label name = new Label();
                        dataCell.Controls.Add(name);
                        name.ID = "name_" + row.ToString() + "_" + cols.ToString();
                        name.Text = names[cols] + "<br>";
                        name.ToolTip = names[cols] + " / " + comments[cols];

                        Label comment = new Label();
                        dataCell.Controls.Add(comment);
                        comment.ID = "label_" + row.ToString() + "_" + cols.ToString();
                        comment.Font.Italic = true;
                        comment.Text = comments[cols];

                    }
                    if (dnesJe == 0 || dnesJe == 6)
                    {
                        dataCell.CssClass = "box red";
                    }
                    if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                    {
                        dataCell.CssClass = "box yellow";
                    }
                    if (aktDenMesiac == (row + 1))
                    {
                        dataCell.BorderColor = System.Drawing.Color.Red;
                        dataCell.BorderWidth = Unit.Point(5);
                    }
                    tblRow.Controls.Add(dataCell);
                }
            }
        }
        else
        {
            if (table.Count == 0)
            {
                if (this.rights != "admin" || this.rights != "poweruser")
                {
                    this.msg_lbl.Text = Resources.Resource.shifts_not_done;
                   // this.publish_cb.Visible = false;
                }
                if (this.rights == "admin" || this.rights == "poweruser")
                {
                    SortedList myres  = x2Mysql.fillNurseShifts(Convert.ToInt32(dateGroup), Convert.ToInt32(daysMonth), Convert.ToInt32(mesiac), Convert.ToInt32(rok), depsIdf);
                    
                    if (Convert.ToBoolean(myres["status"]))
                    {
                        this.shiftTable.Controls.Clear();
                        //this.publish_cb.Visible = true;
                    //this.msg_lbl.Text = daysTmp.ToString();
                    //ViewState.Clear();
                        this.loadSluzby();
                    }
                    else
                    {
                        this.msg_lbl.Text = myres["msg"].ToString();
                    }
                }
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

    protected void commentChanged(object sender, EventArgs e)
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
            this.msg_lbl.Text = result["msg"].ToString();
        }



       // this.msg_lbl.Text = e.ToString() + "..." + tBoxF.Text.ToString();
    }

    protected void dItemChanged(object sender, EventArgs e)
    {
       // this.msg_lbl.Text = e.ToString();

        DropDownList ddl = (DropDownList)sender;

        string[] tmp = ddl.ID.ToString().Split('_');


        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        Control controlList = ctpl.FindControl("ddl_"+tmp[1]+"_"+tmp[2]);

        DropDownList doctor_lb = (DropDownList)controlList;

        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", this.shiftType[col]);
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

        Boolean res = Convert.ToBoolean(result["status"].ToString());
        if (!res)
        {
            this.msg_lbl.Text = result["msg"].ToString();
        }
        
    }

    protected void publishOnFnc(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        string rok = this.rok_cb.SelectedValue.ToString();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        sb.AppendFormat("UPDATE [is_sluzby_2_sestr] SET [state]='active' WHERE [date_group]='{0}{1}'", rok, mesiac);
        SortedList res = x2Mysql.execute(sb.ToString());

        Boolean result = Convert.ToBoolean(res["status"].ToString());

        if (!result)
        {
            this.msg_lbl.Text = res["msg"].ToString();
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

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        sb.AppendFormat("UPDATE [is_sluzby_2_sestr] SET [state]='draft' WHERE [date_group]='{0}{1}'", rok, mesiac);
        SortedList res = x2Mysql.execute(sb.ToString());

        Boolean result = Convert.ToBoolean(res["status"].ToString());

        if (!result)
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }
        else
        {
            if (this.rights == "admin" || this.rights == "poweruser")
            {
                this.shiftState_lbl.Text = Resources.Resource.shifts_see_limited;
            }
        }
    }

    //protected void changePublishStatus(object sender, EventArgs e)
    //{
    //    StringBuilder sb = new StringBuilder();

    //   // CheckBox publ = new CheckBox();

    //    //CheckBox publ = (CheckBox)sender;


    //    string rok = this.rok_cb.SelectedValue.ToString();
    //    string mesiac = this.mesiac_cb.SelectedValue.ToString();

    //    if (mesiac.Length == 1)
    //    {
    //        mesiac = "0" + mesiac;
    //    }



    //    if (this.publish_cb.Checked == true)
    //    {
    //        sb.AppendFormat("UPDATE [is_sluzby_2_sestr] SET [state]='active' WHERE [date_group]='{0}{1}'", rok, mesiac);
    //        //this.publish_cb.Checked = false;
    //    }
    //    else
    //    {
    //        sb.AppendFormat("UPDATE [is_sluzby_2_sestr] SET [state]='draft' WHERE [date_group]='{0}{1}'", rok, mesiac);
    //        //this.publish_cb.Checked = true;
    //    }

    //   // this.msg_lbl.Text = sb.ToString();
    //    SortedList res =  x2Mysql.execute(sb.ToString());

    //    Boolean result = Convert.ToBoolean(res["status"].ToString());

    //    if (!result)
    //    {
    //        this.msg_lbl.Text = res["msg"].ToString();
    //    }

    //}

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
        string query = @"
                        SELECT [t_users.full_name] AS [name]
                           ,GROUP_CONCAT([t_sluzb.typ] ORDER BY [t_sluzb.datum] SEPARATOR ';' ) AS [plan] 
                           ,GROUP_CONCAT(DATE_FORMAT([t_sluzb.datum],'%Y-%c-%e') ORDER BY [t_sluzb.datum] SEPARATOR ';' ) AS [datum]
                        FROM [is_sluzby_2_sestr] AS [t_sluzb]
                            INNER JOIN [is_users] AS [t_users] ON [t_users.id] = [t_sluzb.user_id]
                        WHERE [t_sluzb.date_group]='201509' AND [deps]='MSV' 
                        GROUP BY [t_sluzb.user_id]
                        ORDER BY [t_sluzb.datum] 
                        ";

        query = x2Mysql.buildSql(query,new string[] {});

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);
        
        
        this.makePdf(table);

    }

    protected void makePdf(Dictionary<int, Hashtable> table)
    {
        

        
        int milis = DateTime.Now.Millisecond;
        string path = Server.MapPath("App_Data");
        string imagepath = Server.MapPath("App_Data");
        string oldFile = @path + @"\nurse\plan10.pdf";
        string hash = x2.makeFileHash(Session["login"].ToString() + milis.ToString());
        string newFile = @path + @"\nurse\plan\plan_" + hash + ".pdf";
        this.msg_lbl.Text = oldFile;
        // open the reader
        PdfReader reader = new PdfReader(oldFile);
        
               
        Rectangle size = reader.GetPageSizeWithRotation(1);
        Document myDoc = new Document(size,0,0,0,0);
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
        cb.AddTemplate(page, 0, 0);
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
        

        float startRowY = size.Height- 189; //126 start
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
        cb.ShowText(mesiac.ToString()+" / "+rok.ToString());
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
                double recY = (size.Height - (startRowY+10)) + (planDistance * i);

                float recYY = (float)recY;

                cb.Rectangle(startPlanX+(i*planDistance), (float)179.40, (float)13.96, (float)235.97);
                //cb.Stroke();
                cb.Fill();
            }


        }
        cb.SetColorStroke(BaseColor.BLACK);
        cb.SetColorFill(BaseColor.BLACK);

        cb.SetFontAndSize(mojFont, 9);

        int tbLn = table.Count;

       // int modulo = 8 % 10;



        for (int i=0; i<tbLn; i++)
        {
            if (i <10)
            {
                string[] nm = table[i]["name"].ToString().Split(' ');
                cb.BeginText();
                cb.MoveText(startNameX, startRowY - (i * nameDistance));
                          
                cb.ShowText(nm[0]);
                cb.EndText();

                cb.BeginText();
                cb.MoveText(startNameX, startRowY - (i * nameDistance)-9);

                cb.ShowText(nm[1]);
                cb.EndText();

                string[] sluzby = table[i]["plan"].ToString().Split(';');
                string[] dni = table[i]["datum"].ToString().Split(';');

                //int slLn = sluzby.Length;
                for (int p=0;p<days;p++)
                {

                    string unixDate = rok.ToString() + "-" + mesiac.ToString() +"-"+ (p + 1).ToString();

                    int pos = Array.IndexOf(dni,unixDate);

                    if (pos !=-1)
                    {
                        cb.BeginText();
                        cb.MoveText(startPlanX + (p * planDistance), startRowY - (i * nameDistance));
                        cb.ShowText(sluzby[pos]);
                        cb.EndText();
                    }

                    
                }

            }
            
        }
        myDoc.NewPage();
        cb.AddTemplate(page, 0,0);
        
        

        myDoc.Close();
        fs.Close();
        writer.Close();
        reader.Close();

    }

    protected void writeData(PdfContentByte cb, SortedList data)
    {

    }
}