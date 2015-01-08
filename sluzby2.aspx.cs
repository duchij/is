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

public partial class sluzby2 : System.Web.UI.Page
{
    public mysql_db x2Mysql = new mysql_db();
    public x2_var x2 = new x2_var();
    public sluzbyclass x2Sluzby = new sluzbyclass();

    public string  rights = "";
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

        this.rights = Session["rights"].ToString();
        if (this.rights == "admin" || this.rights == "poweruser")
        {
            this.publish_cb.Visible = true;
        }
        else
        {
            this.publish_cb.Visible = false;
        }
        if (IsPostBack == false)
        {
            this.setMonthYear();

           this.loadSluzby();
        }
        else
        {
           // string argument = Request["__EVENTARGUMENT"].ToString();

           // this.msg_lbl.Text ="he"+ argument;
            this.shiftTable.Controls.Clear();
            //this.shiftTable.Controls.d
            this.loadSluzby();

            /*ContentPlaceHolder ctpl = new ContentPlaceHolder();
            Control tmpControl = Master.FindControl("ContentPlaceHolder1");

            ctpl = (ContentPlaceHolder)tmpControl;
            Control controlList = ctpl.FindControl("listBox_0_0");

            DropDownList doctor_lb = (DropDownList)controlList;

            this.msg_lbl.Text = doctor_lb.SelectedValue.ToString();*/

           //
        }
    }

    protected void changeSluzba(object sender, EventArgs e)
    {
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

        SortedList res = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name] = 'shift_doctors'");

        // Boolean status = Convert.ToBoolean(res["status"].ToString());

        string[] shifts = res["data"].ToString().Split(',');
        this.shiftType = shifts;
        
        StringBuilder sb = new StringBuilder();

        if (this.rights == "admin" || this.rights == "poweruser")
        {

            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}{1}'", rok, mesiac);
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
            sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}{1}' AND [t_sluzb].[state]='active'", rok, mesiac);
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum]");
        }

      //  this.msg_lbl.Text = sb.ToString();

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());
        if (table.Count == daysMonth)
        {
            if (!IsPostBack)
            {
                string state = table[0]["state"].ToString();

                if (state == "active")
                {
                    this.publish_cb.Checked = true;
                    //this.publish_cb.ch
                }
                else
                {
                    this.publish_cb.Checked = false;
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
            ArrayList doctorList = this.loadDoctors();

            int aktDenMesiac = DateTime.Today.Day;


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


                    if (this.rights == "admin" || this.rights == "poweruser")
                    {
                        DropDownList doctors_lb = new DropDownList();



                        doctors_lb.ID = "ddl_" + row.ToString() + "_" + cols.ToString();
                        //doctors_lb.CssClass = "no-pad-mobile no-gap-mobile";

                        int listLn = doctorList.Count;
                        ListItem[] newItem = new ListItem[listLn];

                        for (int doc = 0; doc < listLn; doc++)
                        {
                            string[] tmp = doctorList[doc].ToString().Split('|');
                            newItem[doc] = new ListItem(tmp[1].ToString(), tmp[0].ToString());
                        }
                        doctors_lb.Items.AddRange(newItem);

                        string dd = userId[cols].ToString();

                        doctors_lb.SelectedValue = dd; // userId[cols].ToString();
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
                        Label name = new Label();
                        dataCell.Controls.Add(name);
                        name.ID = "name_" + row.ToString() + "_" + cols.ToString();
                        name.Text = names[cols] + "<br>";

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


                    /* }
                     else
                     {
                         Button saveBtn = new Button();
                         dataCell.Controls.Add(saveBtn);
                         saveBtn.ID="saveBtn_"+ row.ToString() + "_" + cols.ToString();
                         saveBtn.CssClass = "button green";
                         saveBtn.EnableViewState = true;
                       
                         saveBtn.Text = Resources.Resource.save;
                         saveBtn.Click += new EventHandler(saveShiftRow);
                         saveBtn.EnableViewState = true;
                        
                     }*/

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
                    this.publish_cb.Visible = false;
                }
                if (this.rights == "admin" || this.rights == "poweruser")
                {
                    int daysTmp = x2Mysql.fillDocShifts(Convert.ToInt32(dateGroup), Convert.ToInt32(daysMonth), Convert.ToInt32(mesiac), Convert.ToInt32(rok));
                    this.shiftTable.Controls.Clear();
                    this.publish_cb.Visible = true;
                    //this.msg_lbl.Text = daysTmp.ToString();
                    //ViewState.Clear();
                    this.loadSluzby();
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

    protected ArrayList loadDoctors()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [id],[name3] FROM [is_users] WHERE ([group] = 'users' OR [group] = 'poweruser') AND [active] = 1  ORDER BY [name2]");

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

        SortedList result = x2Mysql.mysql_insert("is_sluzby_2", data);

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


        data.Add("date_group", rok+mesiac);
        int den = Convert.ToInt32(tmp[1]);
        den = den +1;

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_2", data);

        Boolean res = Convert.ToBoolean(result["status"].ToString());
        if (!res)
        {
            this.msg_lbl.Text = result["msg"].ToString();
        }
        
    }

    protected void changePublishStatus(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();

       // CheckBox publ = new CheckBox();

        //CheckBox publ = (CheckBox)sender;


        string rok = this.rok_cb.SelectedValue.ToString();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }



        if (this.publish_cb.Checked == true)
        {
            sb.AppendFormat("UPDATE [is_sluzby_2] SET [state]='active' WHERE [date_group]='{0}{1}'", rok, mesiac);
            //this.publish_cb.Checked = false;
        }
        else
        {
            sb.AppendFormat("UPDATE [is_sluzby_2] SET [state]='draft' WHERE [date_group]='{0}{1}'", rok, mesiac);
            //this.publish_cb.Checked = true;
        }

       // this.msg_lbl.Text = sb.ToString();
        SortedList res =  x2Mysql.execute(sb.ToString());

        Boolean result = Convert.ToBoolean(res["status"].ToString());

        if (!result)
        {
            this.msg_lbl.Text = res["msg"].ToString();
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
    
    


}