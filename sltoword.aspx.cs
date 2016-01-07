using System;
using System.Text;
using System.IO;
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

public partial class sltoword : System.Web.UI.Page
{
    public my_db x_db = new my_db();
    public x2_var my_x2 = new x2_var();
    public mysql_db x2Mysql = new mysql_db();
    public string[] shiftType;
    public string gKlinika;
    

    sluzbyclass x2Sluzby = new sluzbyclass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.gKlinika = Session["klinika"].ToString().ToLower();
        this.setLabels();
        
        this.mesiac_lbl.Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[Convert.ToInt32(Session["aktSluzMesiac"].ToString()) - 1];
        this.rok_lbl.Text = Session["aktSluzRok"].ToString();
        string mes  = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[Convert.ToInt32(Session["aktSluzMesiac"].ToString())-1];

        switch (this.gKlinika)
        {
            case "kdch":
                this.drawTable("kdch");
                break;
            case "2dk":
                this.generate2DK();
                break;
            case "nkim":
                this.loadSluzby();
                break;
            case "kdhao":
                this.drawTable("kdhao");
                break;
        }

/*
        if (this.gKlinika == "kdch" )
        {
            this.loadSluzby();
        }

        if (this.gKlinika == "2dk")
        {
            this.generate2DK();
        }*/

        if (Session["toWord"].ToString() == "1")
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/msword; charset=Windows-1250";
            Response.AddHeader("content-disposition", "attachment;filename=" + x2_var.UTFtoASCII(mes) + ".doc");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1250");
           	Response.Charset = "Windows-1250";
            StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used

            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
            this.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
       


    }

    protected void setLabels()
    {
        this.shiftPrintTitel_lbl.Text = my_x2.setLabel(this.gKlinika + "_shifts_print");
        this.shift_sign.Text = my_x2.setLabel(this.gKlinika + "_shifts_sign");
    }

    protected void generate2DK()
    {
        this.shiftTable.Controls.Clear();
        string mesiac = Session["aktSluzMesiac"].ToString();
        string rok = Session["aktSluzRok"].ToString();

        int daysMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));


        string dateGroup = my_x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac)).ToString();
            

        SortedList res = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name] = '2dk_shift_doctors'");

        // Boolean status = Convert.ToBoolean(res["status"].ToString());

        string[] shifts = res["data"].ToString().Split(',');
        this.shiftType = shifts;

        int days = daysMonth;
        int colsNum = this.shiftType.Length;

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell headCell = new TableHeaderCell();
        headCell.ID = "headCell_date";
        headCell.Text = "<strong>Datum</strong>";
        headCell.BorderWidth = 1;
        headCell.Width = Unit.Pixel(100);

        // headCell.Style
        headRow.Controls.Add(headCell);

        for (int head = 0; head < colsNum; head++)
        {
            TableHeaderCell headCell1 = new TableHeaderCell();
            headCell1.ID = "headCell_" + head;
            headCell1.Text = "<strong><center>" + shifts[head].ToString() + "</center></strong>";
            headCell1.Width = Unit.Pixel(130);
            headCell1.BorderWidth = 1;
            // headCell1.
            headRow.Controls.Add(headCell1);
        }
        /* TableHeaderCell headCellSave = new TableHeaderCell();
         headCellSave.ID = "headCellSave_" + colsNum;
         headCellSave.Text = "<strong>Ulozit</strong>";
         headRow.Controls.Add(headCellSave);*/

        shiftTable.Controls.Add(headRow);

        string[] freeDays = x2Sluzby.getFreeDays();

        for (int row=0; row<daysMonth; row++)
        {
            int rDen = row + 1;
            TableRow riadok = new TableRow();
            this.shiftTable.Controls.Add(riadok);

            TableCell dateCell = new TableCell();
            dateCell.BorderStyle = BorderStyle.Solid;
            dateCell.BorderWidth = Unit.Pixel(1);
            DateTime dt = Convert.ToDateTime(rDen+"."+mesiac+"."+rok);
            dateCell.Text = dt.ToShortDateString();
            riadok.Controls.Add(dateCell);

            int dayOWeek = (int)dt.DayOfWeek;
            int sviatok = Array.IndexOf(freeDays, rDen + "." + mesiac);
            if (sviatok != -1 || (dayOWeek == 0 || dayOWeek == 6))
            {
                dateCell.BackColor = System.Drawing.Color.LightGray;
            }


            for (int col=0; col<colsNum; col++)
            {

                TableCell dataCell = new TableCell();
                dataCell.ID = shifts[col] + "_" + rDen;
                dataCell.BorderStyle = BorderStyle.Solid;
                dataCell.BorderWidth = Unit.Pixel(1);

                if (sviatok !=-1 || (dayOWeek == 0 || dayOWeek == 6))
                {
                    dataCell.BackColor = System.Drawing.Color.LightGray;
                }

                dataCell.ToolTip = shifts[col] + "_" + rDen;
                riadok.Controls.Add(dataCell);
            }
        }
        this.load2DKShifts(dateGroup);
    }

    protected void load2DKShifts(string dateGroup)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("SELECT [is_sluzby_dk].*, [is_users].[name3] AS [name] FROM [is_sluzby_dk]");
        sb.AppendLine("LEFT JOIN [is_users] ON [is_users].[id] = [is_sluzby_dk].[user_id] ");
        sb.AppendFormat("WHERE [is_sluzby_dk].[date_group] ='{0}' AND [is_sluzby_dk].[clinic]='{1}' ORDER BY [is_sluzby_dk].[datum] ASC", dateGroup, Session["klinika_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableCn = table.Count;

        for (int i=0; i< tableCn; i++)
        {
            DateTime dt = Convert.ToDateTime(my_x2.UnixToMsDateTime(table[i]["datum"].ToString()));

            string type = table[i]["typ"].ToString();
            string week = table[i]["tyzden"].ToString();
            int day = dt.Day;

            if (week == "konz" || week=="prijm")
            {
                Control cl = FindControl("Konz_" + day.ToString());
                TableCell dataCell = (TableCell)cl;
                dataCell.Text = "<center>"+week.ToUpper()+"</center>";
            }

            if (type.IndexOf("Odd")!= -1)
            {
                if (my_x2.getStr(table[i]["name"].ToString()) != "")
                {
                    Control cl = FindControl("Odd_" + day.ToString());
                    TableCell dataCell = (TableCell)cl;
                    Label textLbl = new Label();
                    
                    textLbl.Text = "<p>"+ my_x2.getStr(table[i]["name"].ToString()) + "<br> (" + table[i]["comment"] + ")</p>";
                    dataCell.Controls.Add(textLbl);
                }
            }
            if (type.IndexOf("OupA") != -1)
            {
                if (my_x2.getStr(table[i]["name"].ToString()) != "")
                {
                    Control cl = FindControl("OupA_" + day.ToString());
                    TableCell dataCell = (TableCell)cl;
                    Label textLbl = new Label();

                    textLbl.Text = "<p>" + my_x2.getStr(table[i]["name"].ToString()) + "<br> (" + table[i]["comment"] + ")</p>";
                    dataCell.Controls.Add(textLbl);
                }
            }
            if (type.IndexOf("OupB") != -1)
            {
                if (my_x2.getStr(table[i]["name"].ToString()) != "")
                {
                    Control cl = FindControl("OupB_" + day.ToString());
                    TableCell dataCell = (TableCell)cl;
                    Label textLbl = new Label();

                    textLbl.Text = "<p>" + my_x2.getStr(table[i]["name"].ToString()) + "<br> (" + table[i]["comment"] + ")</p>";
                    dataCell.Controls.Add(textLbl);
                }
            }
            if (type.IndexOf("Expe") != -1)
            {
                if (my_x2.getStr(table[i]["name"].ToString()) != "")
                {
                    Control cl = FindControl("Expe_" + day.ToString());
                    TableCell dataCell = (TableCell)cl;
                    Label textLbl = new Label();

                    textLbl.Text = "<p>" + my_x2.getStr(table[i]["name"].ToString()) + "<br> (" + table[i]["comment"] + ")</p>";
                    dataCell.Controls.Add(textLbl);
                }
            }
            if (type.IndexOf("KlAmb") != -1)
            {
                if (my_x2.getStr(table[i]["name"].ToString()) != "")
                {
                    Control cl = FindControl("KlAmb_" + day.ToString());
                    TableCell dataCell = (TableCell)cl;
                    Label textLbl = new Label();

                    textLbl.Text = "<p>" + my_x2.getStr(table[i]["name"].ToString()) + "<br> (" + table[i]["comment"] + ")</p>";
                    dataCell.Controls.Add(textLbl);
                }
            }

        }




    }


    protected void loadSluzby()
    {

        this.shiftTable.Controls.Clear();

        string mesiac = Session["aktSluzMesiac"].ToString();
        string rok = Session["aktSluzRok"].ToString();

        int daysMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

       
        string dateGroup = Session["aktDateGroup"].ToString();

        string settings = "";
        string shiftQuery = "";

        switch(this.gKlinika)
        {
            case "kdch":
                settings = "SELECT * FROM [is_settings] WHERE [name] = 'kdch_shift_doctors'";
                
                shiftQuery = "SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],";
                shiftQuery +=" [t_sluzb].[state] AS [state],";
                shiftQuery +=" GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],";
                shiftQuery +=" GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],";
                shiftQuery +=" GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],";
                shiftQuery +=" [t_sluzb].[date_group] AS [dategroup]";
                shiftQuery +=" FROM [is_sluzby_2] AS [t_sluzb]";
                shiftQuery +=" LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]";
                shiftQuery +=" WHERE [t_sluzb].[date_group] = '{0}' AND [t_sluzb].[state]='active'";
                shiftQuery +=" GROUP BY [t_sluzb].[datum]";
                shiftQuery +=" ORDER BY [t_sluzb].[datum]";

                shiftQuery = my_x2.sprintf(shiftQuery, new string[] { dateGroup });
                break;

            case "nkim":
                settings = "SELECT * FROM [is_settings] WHERE [name] = 'nkim_shift_doctors'";

                shiftQuery = "SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],";
                shiftQuery +=" [t_sluzb].[state] AS [state],";
                shiftQuery +=" GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],";
                shiftQuery +=" GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],";
                shiftQuery +=" GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],";
                shiftQuery +=" [t_sluzb].[date_group] AS [dategroup]";
                shiftQuery +=" FROM [is_sluzby_all] AS [t_sluzb]";
                shiftQuery +=" LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]";
                shiftQuery +=" WHERE [t_sluzb].[date_group] = '{0}' AND [t_sluzb].[state]='active'";
                shiftQuery += " AND [t_sluzb].[clinic] = {1}";
                shiftQuery +=" GROUP BY [t_sluzb].[datum]";
                shiftQuery +=" ORDER BY [t_sluzb].[datum]";

                shiftQuery = my_x2.sprintf(shiftQuery, new string[] { dateGroup,Session["klinika_id"].ToString() });
                break;
            case "kdhao":
                settings = "SELECT * FROM [is_settings] WHERE [name] = 'kdhao_shift_doctors'";

                shiftQuery = "SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],";
                shiftQuery += " [t_sluzb].[state] AS [state],";
                shiftQuery += " GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],";
                shiftQuery += " GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],";
                shiftQuery += " GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],";
                shiftQuery += " [t_sluzb].[date_group] AS [dategroup]";
                shiftQuery += " FROM [is_sluzby_all] AS [t_sluzb]";
                shiftQuery += " LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]";
                shiftQuery += " WHERE [t_sluzb].[date_group] = '{0}' AND [t_sluzb].[state]='active'";
                shiftQuery += " AND [t_sluzb].[clinic] = {1}";
                shiftQuery += " GROUP BY [t_sluzb].[datum]";
                shiftQuery += " ORDER BY [t_sluzb].[datum]";

                shiftQuery = my_x2.sprintf(shiftQuery, new string[] { dateGroup, Session["klinika_id"].ToString() });
                break;


        }

        SortedList res = x2Mysql.getRow(settings);

        // Boolean status = Convert.ToBoolean(res["status"].ToString());

        string[] shifts = res["data"].ToString().Split(',');
        this.shiftType = shifts;

        Dictionary<int, Hashtable> table = x2Mysql.getTable(shiftQuery);




        if (table.Count == daysMonth)
        {
      

            string[] header = shifts;

            int days = table.Count;
            int colsNum = header.Length;

            TableHeaderRow headRow = new TableHeaderRow();

            TableHeaderCell headCell = new TableHeaderCell();
            headCell.ID = "headCell_date";
            headCell.Text = "<strong>Datum</strong>";
            headCell.BorderWidth = 1;
            headCell.Width = Unit.Pixel(100);
            
            // headCell.Style
            headRow.Controls.Add(headCell);

            for (int head = 0; head < colsNum; head++)
            {
                TableHeaderCell headCell1 = new TableHeaderCell();
                headCell1.ID = "headCell_" + head;
                headCell1.Text = "<strong><center>" + header[head].ToString() + "</center></strong>";
                headCell1.Width = Unit.Pixel(130);
                headCell1.BorderWidth = 1;
                // headCell1.
                headRow.Controls.Add(headCell1);
            }
            /* TableHeaderCell headCellSave = new TableHeaderCell();
             headCellSave.ID = "headCellSave_" + colsNum;
             headCellSave.Text = "<strong>Ulozit</strong>";
             headRow.Controls.Add(headCellSave);*/

            shiftTable.Controls.Add(headRow);

            string[] freeDays = x2Sluzby.getFreeDays();
          //  ArrayList doctorList = this.loadDoctors();

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
                string sviatok = (row + 1).ToString() + "." + mesiac;
                int jeSviatok = Array.IndexOf(freeDays, sviatok);



                TableCell cellDate = new TableCell();
                
                // cellDate.ID = "cellDate_" + row;
                if (dnesJe == 0 || dnesJe == 6)
                {
                   // cellDate.CssClass = "box red";
                    cellDate.BackColor = System.Drawing.Color.Gray;
                }

                if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                {
                    //cellDate.CssClass = "box yellow";
                    cellDate.BackColor = System.Drawing.Color.LightGray;
                }
                string text = (row + 1).ToString();
                cellDate.Text = text + ". " + nazov;
                cellDate.BorderWidth = 1;
                tblRow.Controls.Add(cellDate);

                for (int cols = 0; cols < colsNum; cols++)
                {
                    TableCell dataCell = new TableCell();
                   
                    Label name = new Label();
                    dataCell.Controls.Add(name);
                    name.ID = "name_" + row.ToString() + "_" + cols.ToString();
                    if (Session["toWord"].ToString() == "1")
                    {
                        name.Text = "<strong>" + names[cols] + "</strong>";
                        name.Font.Size = FontUnit.Point(11);
                    }
                    else
                    {
                        name.Text = "<strong>" + names[cols] + "</strong><br>";
                    }
                    name.CssClass = "menoLbl";
                    Label comment = new Label();
                    dataCell.Controls.Add(comment);
                    comment.ID = "label_" + row.ToString() + "_" + cols.ToString();
                    comment.Text = comments[cols];
                    if (dnesJe == 0 || dnesJe == 6)
                    {
                        dataCell.BackColor = System.Drawing.Color.Gray;
                    }
                    if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                    {
                        dataCell.BackColor = System.Drawing.Color.LightGray;
                    }
                
                    dataCell.BorderWidth = 1;
                    tblRow.Controls.Add(dataCell);
                }
            }
        }
        else
        {
            if (table.Count == 0)
            {
                
                    this.msg_lbl.Text = Resources.Resource.shifts_not_done;
            }
        }
    }


    protected void drawTable(string dep)
    {
        string query = @"SELECT [name],[data] FROM [is_settings] WHERE [name]='{0}'";
        
        query = x2Mysql.buildSql(query, new string[] { dep+"_shift_doctors" });

        SortedList row = x2Mysql.getRow(query);

        string data = row["data"].ToString();

        this.parseMultiTable(data);

    }

    protected void parseMultiTable(string data)
    {
        string[] freeDays = Session["freedays"].ToString().Split(',');

        this.shiftTable.Controls.Clear();
        string mesiac = Session["aktSluzMesiac"].ToString();
        string rok = Session["aktSluzRok"].ToString();


        int daysMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        string dateGroup = rok + mesiac;
        Session.Add("aktDateGroup", dateGroup);


        string[] groups = data.Split(';');
        int grps = groups.Length;
        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dayHeadCell = new TableHeaderCell();
        dayHeadCell.Text = "Datum";
        //dayHeadCell.Width = 140;
        headRow.Controls.Add(dayHeadCell);

        for (int i = 0; i < grps; i++)
        {
            //string[] head = groups[i].Split('_');

            TableHeaderCell headCell = new TableHeaderCell();
            headCell.HorizontalAlign = HorizontalAlign.Center;
            headCell.Text = "<center><strong>" + groups[i] + "</center></strong>";

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

            if (dnesJe == 0 || dnesJe == 6)
            {
                dayCell.BackColor = System.Drawing.Color.LightGray;
            }

            if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
            {
                dayCell.BackColor = System.Drawing.Color.LightGray;
            }



            dayCell.Text = rDay.ToString() + ". " + nazov;
            dataRow.Controls.Add(dayCell);

            for (int col = 0; col < grps; col++)
            {
                TableCell dataCell = new TableCell();

                if (dnesJe == 0 || dnesJe == 6)
                {
                    dataCell.BackColor = System.Drawing.Color.LightGray;
                }

                if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                {
                    dataCell.BackColor = System.Drawing.Color.LightGray;
                }


                string[] ddls = groups[col].Split(',');
                if (ddls.Length > 0)
                {
                    int dCount = ddls.Length;
                    for (int cnt = 0; cnt < dCount; cnt++)
                    {
                        Label dl = new Label();
                        dl.ID = "name_" + rDay.ToString() + "_" + ddls[cnt];
                     //   dl.Text = "-";

                        dataCell.Controls.Add(dl);

                        Label comment = new Label();
                        comment.ID = "comment_" + rDay.ToString() + "_" + ddls[cnt];
                      //  comment.Text = "-";
                        dataCell.Controls.Add(comment);

                        dataRow.Controls.Add(dataCell);
                    }
                }
                else
                {
                    
                    Label dl = new Label();
                    dl.ID = "name_" + rDay.ToString() + "_" + groups[col];
                  //  dl.Text = "-";

                    dataCell.Controls.Add(dl);

                    Label comment = new Label();
                    comment.ID = "comment_" + rDay.ToString() + "_" + groups[col];
                   // comment.Text = "-";
                    dataCell.Controls.Add(comment);

                    dataRow.Controls.Add(dataCell);
                }
            }
            this.shiftTable.Controls.Add(dataRow);
        }
        this.setShifts();
    }

    protected void setShifts()
    {
        string query = "";

        int mesiac = Convert.ToInt32(Session["aktSluzMesiac"].ToString());
        int rok = Convert.ToInt32(Session["aktSluzRok"].ToString());

        int dateGroup = my_x2.makeDateGroup(rok, mesiac);

        if (this.gKlinika == "kdhao")
        {
            query = @"SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],
                [t_sluzb].[state] AS [state],
                GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],
                GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],
                GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],
                [t_sluzb].[date_group] AS [dategroup]
                FROM [is_sluzby_all] AS [t_sluzb]
                LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]
                    WHERE [t_sluzb].[date_group] = '{0}' AND [t_sluzb].[state]='active'
                AND [t_sluzb].[clinic] = '{1}' 
                GROUP BY [t_sluzb].[datum]
                ORDER BY [t_sluzb].[datum]";
        }
        if (this.gKlinika == "kdch")
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


        query = x2Mysql.buildSql(query, new string[] { dateGroup.ToString(), Session["aktSluzClinic"].ToString() });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        int tblCnt = table.Count;
        if (tblCnt > 0)
        {
            for (int day = 0; day < tblCnt; day++)
            {

                int rDay = Convert.ToDateTime(my_x2.UnixToMsDateTime(table[day]["datum"].ToString())).Day;
                string[] names = table[day]["users_names"].ToString().Split(';');
                string[] userId = table[day]["users_ids"].ToString().Split('|');
                string[] comments = table[day]["comment"].ToString().Split('|');
                string[] type = table[day]["type1"].ToString().Split(';');

                int tpLn = type.Length;

                if (names.Length != userId.Length)
                {
                    this.msg_lbl.Text = my_x2.errorMessage("Inkonzistencia v datach prosim opravit.....");
                }
                if (tpLn == 0)
                {
                    
                        Control crtl = FindControl("name_" + rDay.ToString() + "_" + table[day]["type1"].ToString());

                        Label ddl = (Label)crtl;

                        try
                        {
                            ddl.Text = table[day]["user_names"].ToString() + "<span class='small'>("+ table[day]["type1"].ToString()+")</span><br>";
                        }
                        catch (Exception ex)
                        {
                            //ddl.Text = "-";
                        }

                        if (table[day]["comment"]!=null && table[day]["comment"].ToString() != "-")
                        {
                            Control crtl1 = FindControl("comment_" + rDay.ToString() + "_" + table[day]["type1"].ToString());
                            Label textBox = (Label)crtl1;


                            textBox.Text = "<i>" + table[day]["comment"].ToString() + "</i>";
                    
                        }    
                        
                }
                else
                {
                    for (int col = 0; col < tpLn; col++)
                    {
                       
                        Control crtl = FindControl("name_" + rDay.ToString() + "_" + type[col]);
                        Label ddl = (Label)crtl;

                        try
                        {
                            ddl.Text = names[col] + "<span class='small'>("+ type[col]+")</span><br>";
                        }
                        catch (Exception ex)
                        {
                           // ddl.Text = "-";
                        }

                        if (comments[col]!=null && comments[col]!="-")
                        {
                            Control crtl1 = FindControl("comment_" + rDay.ToString() + "_" + type[col]);
                            Label textBox = (Label)crtl1;

                            textBox.Text = "<i>" + comments[col] + "</i>";
                        }

                        
                    }
                }

            }
        }
        else
        {
           this.msg_lbl.Text = Resources.Resource.shifts_not_done;
        }
    }





}
