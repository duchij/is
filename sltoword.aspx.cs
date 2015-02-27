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

        if (this.gKlinika == "kdch")
        {
            this.loadSluzby();
        }

        if (this.gKlinika == "2dk")
        {
            this.generate2DK();
        }

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

        sb.AppendLine("SELECT [is_sluzby_dk].*, [is_omega_doctors].[name] FROM [is_sluzby_dk]");
        sb.AppendLine("LEFT JOIN [is_omega_doctors] ON [is_omega_doctors].[ms_item_id] = [is_sluzby_dk].[user_id] ");

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

        SortedList res = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name] = 'shift_doctors'");

        // Boolean status = Convert.ToBoolean(res["status"].ToString());

        string[] shifts = res["data"].ToString().Split(',');
        this.shiftType = shifts;

        StringBuilder sb = new StringBuilder();

        sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
        sb.Append("[t_sluzb].[state] AS [state],");
        sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
        sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
        sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
        sb.Append("[t_sluzb].[date_group] AS [dategroup]");
        sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
        sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
        sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}' AND [t_sluzb].[state]='active'", dateGroup);
        sb.Append("GROUP BY [t_sluzb].[datum]");
        sb.Append("ORDER BY [t_sluzb].[datum]");
    

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());




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
                tblRow.Controls.Add(cellDate);
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
                    comment.Text = "<div style='font-size:10px;'>"+ comments[cols]+"</div>";
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



}
