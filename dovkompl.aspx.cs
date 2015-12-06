using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dovkompl : System.Web.UI.Page
{
    mysql_db x2Mysql = new mysql_db();
    x2_var x2 = new x2_var();
    public string gKlinika;

    protected void Page_Load(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(Session["dov_mesiac"]);
        int rok = Convert.ToInt32(Session["dov_rok"]);

        this.gKlinika = Session["klinika"].ToString().ToLower();
        DateTime dt = DateTime.Today;
        switch (this.gKlinika)
        {
            case "kdch":
                this.dovkomplTitel_lbl.Text = "Prehľad prítomností a neprítomností KDCH";
                this.init(rok, mesiac);

                this.loadActivities(rok, mesiac);
                this.loadShifts(rok, mesiac);
                break;
            case "nkim":
                
                this.dovkomplTitel_lbl.Text = x2.setLabel("nkim_dovkompl_titel") + ", " + dt.ToLongDateString();
                this.init(rok, mesiac);

                this.loadActivities(rok, mesiac);
                this.loadShifts(rok, mesiac);
                break;
            case "kdhao":

                this.dovkomplTitel_lbl.Text = x2.setLabel("kdhao_dovkompl_titel") + ", " + dt.ToLongDateString();
                this.init(rok, mesiac);

                this.loadActivities(rok, mesiac);
                this.loadShifts(rok, mesiac);
                break;
            case "2dk":
                this.dovkomplTitel_lbl.Text = x2.setLabel("2dk_dovkompl_titel") + ", " + dt.ToLongDateString();
                this.init(rok, mesiac);
                this.loadActivities(rok, mesiac);
                this.loadShiftsDK(rok, mesiac);
                break;


        }

      
    }

    protected void loadShiftsDK(int rok, int mesiac)
    {
        int dateGroup = x2.makeDateGroup(rok, mesiac);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [datum], GROUP_CONCAT([typ] ORDER BY [ordering]) AS [type], ");
        sb.AppendLine("GROUP_CONCAT(IFNULL([user_id],'0') ORDER BY [ordering]) AS [user_ids] FROM [is_sluzby_dk]");
        sb.AppendFormat("WHERE [date_group] = '{0}' AND [state]='active' AND [clinic]='{1}' ",dateGroup,Session["klinika_id"]);
        sb.AppendLine("GROUP BY [datum]");

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableCn = table.Count;
        int days = DateTime.DaysInMonth(rok, mesiac);
        if (tableCn > 0)
        {
            for (int row = 0; row < tableCn; row++)
            {
                string[] typArr = table[row]["type"].ToString().Split(',');
                string[] idArr = table[row]["user_ids"].ToString().Split(',');

                DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["datum"].ToString()));

                int den = dt.Day;
                int dayOfWeek = (int)dt.DayOfWeek;

                int cnUsers = idArr.Length;
                for (int i = 0; i < cnUsers; i++)
                {
                    if (idArr[i] != "0")
                    {
                        Control crtl = FindControl("stCell_" + den.ToString() + "_" + idArr[i]);
                        if (crtl != null)
                        {
                            TableCell stCell = (TableCell)crtl;

                            switch (typArr[i])
                            {
                                case "OupA":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0xed7669);
                                    stCell.ToolTip += "Urgent (norm.den)" + "\r\n";
                                    break;
                                case "OupA1":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0xed7669);
                                    stCell.ToolTip += "Urgent (sviatok/vikend)" + "\r\n";
                                    break;
                                case "OupA2":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0xed7669);
                                    stCell.ToolTip += "Urgent (sviatok/vikend)" + "\r\n";
                                    break;
                                case "OupB":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0xed7669);
                                    stCell.ToolTip += "Urgent (norm.den)" + "\r\n";
                                    break;
                                case "OupB1":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0xed7669);
                                    stCell.ToolTip += "Urgent (sviatok/vikend)" + "\r\n";
                                    break;
                                case "Odd":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0x5faee3);
                                    stCell.ToolTip += "Oddelenie (norm.den)" + "\r\n";
                                    break;
                                case "Odd1":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0x5faee3);
                                    stCell.ToolTip += "Oddelenie (sviatok/vikend)" + "\r\n";
                                    break;
                                case "Odd2":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0x5faee3);
                                    stCell.ToolTip += "Oddelenie (sviatok/vikend)" + "\r\n";
                                    break;
                                case "OddB":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0x54d98c);
                                    stCell.ToolTip += "Oddelenie B" + "\r\n";
                                    break;
                                case "Expe":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0x46627f);
                                    stCell.ToolTip += "Expektacne" + "\r\n";
                                    break;
                                case "KlAmb":
                                    stCell.BackColor = System.Drawing.Color.FromArgb(0xb07cc6);
                                    stCell.ToolTip += "Klinicka ambulancia" + "\r\n";
                                    break;
                            }
                            string tmpStCell = stCell.Text.ToString();
                            if (tmpStCell.Length > 0)
                            {
                                stCell.Text += "\r\n" + typArr[i];
                            }
                            else
                            {
                                stCell.Text = typArr[i];
                            }
                        }

                        
                    }

                    if (typArr[i] != "KlAmb")
                    {
                        if (dayOfWeek == 0 || dayOfWeek == 6)
                        {

                            Control crtl1 = FindControl("stCell_" + (den + 1).ToString() + "_" + idArr[i]);
                            if (crtl1 != null)
                            {
                                TableCell stCell1 = (TableCell)crtl1;
                                stCell1.BackColor = System.Drawing.Color.LightGray;
                                stCell1.ToolTip += "Po sluzbe \r\n";
                            }
                            Control crtl2 = FindControl("stCell_" + (den + 2).ToString() + "_" + idArr[i]);
                            if (crtl2 != null)
                            {
                                TableCell stCell2 = (TableCell)crtl2;
                                stCell2.BackColor = System.Drawing.Color.LightGray;
                                stCell2.ToolTip += "Po sluzbe \r\n";
                            }
                        }
                        else
                        {
                            Control crtl3 = FindControl("stCell_" + (den + 1).ToString() + "_" + idArr[i]);
                            if (crtl3 != null)
                            {
                                TableCell stCell3 = (TableCell)crtl3;
                                stCell3.BackColor = System.Drawing.Color.LightGray;
                                stCell3.ToolTip += "Po sluzbe \r\n";
                            }
                        }
                    }
                }
            }
        }

    }


    protected void loadShifts(int rok, int mesiac)
    {
        int dateGroup = x2.makeDateGroup(rok, mesiac);

        string query = @"   SELECT  [t_sluz.datum] AS [datum], 
                                    GROUP_CONCAT([t_sluz.typ] ORDER BY [t_sluz.ordering]) AS [type], 
                                    GROUP_CONCAT([t_sluz.user_id] ORDER BY [t_sluz.ordering]) AS [user_ids] 
                            FROM [is_sluzby_2] AS [t_sluz]
                            INNER JOIN [is_users] AS [t_users] ON [t_users.id] = [t_sluz.user_id]
                            WHERE [t_sluz.date_group] = '{0}' AND [t_sluz.state]='active' AND [t_users.worker]='int'  
                                  GROUP BY [datum] 
                        ";

        query = x2Mysql.buildSql(query, new string[] { dateGroup.ToString() });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        int tableCn = table.Count;
        int days = DateTime.DaysInMonth(rok, mesiac);
        if (tableCn > 0)
        {
            for (int row = 0; row < tableCn; row++)
            {
                string[] typArr = table[row]["type"].ToString().Split(',');
                string[] idArr = table[row]["user_ids"].ToString().Split(',');

                DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["datum"].ToString()));

                int den = dt.Day;
                int dayOfWeek = (int)dt.DayOfWeek;

                int cnUsers = idArr.Length;
                for (int i = 0; i < cnUsers; i++)
                {
                    if (idArr[i] != "0")
                    {
                        Control crtl = FindControl("stCell_" + den.ToString() + "_" + idArr[i]);
                        TableCell stCell = (TableCell)crtl;
                        switch (typArr[i])
                        {
                            case "OUP":
                                stCell.BackColor = System.Drawing.Color.FromArgb(0xed7669);
                                stCell.ToolTip += "Urgent"+"\r\n";
                                break;
                            case "OddA":
                                stCell.BackColor = System.Drawing.Color.FromArgb(0x5faee3);
                                stCell.ToolTip += "Oddelenie A" + "\r\n";
                                break;
                            case "OddB":
                                stCell.BackColor = System.Drawing.Color.FromArgb(0x54d98c);
                                stCell.ToolTip += "Oddelenie B" + "\r\n";
                                break;
                            case "OP":
                                stCell.BackColor = System.Drawing.Color.FromArgb(0x46627f);
                                stCell.ToolTip += "Operačná pohotovosť" + "\r\n";
                                break;
                            case "Prijm":
                                stCell.BackColor = System.Drawing.Color.FromArgb(0xb07cc6);
                                stCell.ToolTip += "Príjmová ambulancia" + "\r\n";
                                break;
                        }
                        string tmpStCell = stCell.Text.ToString();
                        if (tmpStCell.Length > 0)
                        {
                            stCell.Text += "\r\n" + typArr[i];
                        }
                        else
                        {
                            stCell.Text = typArr[i];
                        }
                    }

                    if (typArr[i] != "Prijm")
                    {
                        if (dayOfWeek == 0 || dayOfWeek == 6)
                        {

                            Control crtl1 = FindControl("stCell_" + (den + 1).ToString() + "_" + idArr[i]);
                            if (crtl1 != null)
                            {
                                TableCell stCell1 = (TableCell)crtl1;
                                stCell1.BackColor = System.Drawing.Color.LightGray;
                                stCell1.ToolTip += "Po sluzbe \r\n";
                            }
                            Control crtl2 = FindControl("stCell_" + (den + 2).ToString() + "_" + idArr[i]);
                            if (crtl2 != null)
                            {
                                TableCell stCell2 = (TableCell)crtl2;
                                stCell2.BackColor = System.Drawing.Color.LightGray;
                                stCell2.ToolTip += "Po sluzbe \r\n";
                            }
                        }
                        else
                        {
                            Control crtl3 = FindControl("stCell_" + (den + 1).ToString() + "_" + idArr[i]);
                            if (crtl3 != null)
                            {
                                TableCell stCell3 = (TableCell)crtl3;
                                stCell3.BackColor = System.Drawing.Color.LightGray;
                                stCell3.ToolTip += "Po sluzbe \r\n";
                            }
                        }
                    }
                }

               
                


            }
        }
    }

    protected void loadActivities(int rok, int mesiac)
    {

        string query = @"SELECT [is_dovolenky.id] AS [dov_id] ,[is_dovolenky.user_id],[is_dovolenky.od],
                                [is_dovolenky.do],[is_dovolenky.type], [is_dovolenky.comment] FROM [is_dovolenky]
                            WHERE (MONTH([is_dovolenky.od]) = '{0}' OR MONTH([is_dovolenky.do]) = '{0}') 
                                AND (YEAR([is_dovolenky.od]) = '{1}' OR YEAR([is_dovolenky.do]) = '{1}')
                                AND [is_dovolenky.clinics]='{2}' ORDER BY [is_dovolenky.do] ASC
                        ";
        query = x2Mysql.buildSql(query, new string[] { mesiac.ToString(), rok.ToString(), Session["klinika_id"].ToString() });

        Dictionary<int, Hashtable> data = x2Mysql.getTable(query);
        int dataCn = data.Count;

        int days = DateTime.DaysInMonth(rok, mesiac);

        for (int rec = 0; rec < dataCn; rec++)
        {
            DateTime tmp_od_mes = Convert.ToDateTime(data[rec]["od"]);
            DateTime tmp_do_mes = Convert.ToDateTime(data[rec]["do"]);

            int dayCount = tmp_do_mes.Subtract(tmp_od_mes).Days;

            if (dayCount == 0)
            {
                int hrsCount = tmp_do_mes.Subtract(tmp_od_mes).Hours;

                if (hrsCount >= 23)
                {
                    dayCount = 1;
                }
            }

            int startDay = tmp_od_mes.Day;
            int endDay = tmp_do_mes.Day;
            // for (int den = 0 ; den <= dayCount; den++)
           // int den = 0;
            int dDen = 0;
            for (int den = 0; den <= dayCount; den++)
            {
                if (den == dayCount && dayCount == 1)
                {
                    break;
                }

                if (dayCount > 1)
                {
                    dDen = startDay + den;
                  //  den++;
                }
                else
                {
                    dDen = startDay;
                    
                }

                if (dDen <= days)
                {
                    Control crtl = FindControl("stCell_" + dDen.ToString() + "_" + data[rec]["user_id"].ToString());
                    TableCell stCell = (TableCell)crtl;
                    stCell.HorizontalAlign = HorizontalAlign.Center;

                    SortedList cellData = this.getActivityType(data[rec]["type"].ToString(),x2.getStr(data[rec]["comment"].ToString()));
                    stCell.ForeColor = System.Drawing.Color.White;

                    string state = stCell.Text.ToString().Trim();

                    if (state.Length > 0)
                    {
                        stCell.Text += cellData["code"].ToString();
                        stCell.ToolTip += "\r\n"+cellData["label"].ToString();
                        if (cellData["comment"].ToString().Length > 0) stCell.ToolTip += " (" + cellData["comment"].ToString() + ")";
                        stCell.BackColor = System.Drawing.Color.FromArgb(0xFFAAAA);
                    }
                    else
                    {
                        stCell.Text = cellData["code"].ToString();
                        stCell.ToolTip += cellData["label"].ToString();
                        if (cellData["comment"].ToString().Length > 0) stCell.ToolTip += " (" + cellData["comment"].ToString() + ")";
                        stCell.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(cellData["color"]));
                    }
                }
                else
                {
                  break;
                }
            }
        }
    }

    protected SortedList getActivityType(string type,string comment)
    {
        SortedList result = new SortedList();
        result.Add("comment", comment);
        switch (type)
        {
            case "do":
                result.Add("code", "D");
                result.Add("color", 0x261758);
                result.Add("label", Resources.Resource.free_do);
               
                break;
            case "ci":
                result.Add("code", "C");
                result.Add("color", 0x804515);
                result.Add("label", Resources.Resource.free_ci);
                break;
            case "ko":
                result.Add("code", "K");
                result.Add("color", 0x801815);
                result.Add("label", Resources.Resource.free_ko);
                break;
            case "pn":
                result.Add("code", "P");
                result.Add("color", 0x201858);
                result.Add("label", Resources.Resource.free_pn);
                break;
            case "sk":
                result.Add("code", "S");
                result.Add("color", 0x550000);
                result.Add("label", Resources.Resource.free_sk);
                break;
            case "le":
                result.Add("code", "L");
                result.Add("color", 0x260339);
                result.Add("label", Resources.Resource.free_le);
                break;

        }

        return result;
    }

    protected void init(int rok, int mesiac)
    {

        string query = @"SELECT 
                                [id],[name],[name2],[name3],[titul_pred],[full_name],[titul_za] FROM [is_users] 
                            WHERE ([active]='1' AND [klinika] = '{0}' AND [work_group]='doctor' AND [worker]='int') 
                                AND [name] <> 'admin' 
                                AND [name] <> 'vtablet'
                            ORDER BY [name2] ASC
                        ";
        query = x2Mysql.buildSql(query, new string[] { Session["klinika_id"].ToString() });
        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        int tableCnt = table.Count;

        //int mesiac = 1;
       // int rok = 2015;

        int dni = DateTime.DaysInMonth(rok, mesiac);

        TableHeaderRow headRow = new TableHeaderRow();
        this.komplview_tbl.Controls.Add(headRow);

        TableHeaderCell headMeno = new TableHeaderCell();
        headMeno.Text = "Lekar";
        headMeno.Width = Unit.Pixel(200);
        headRow.Controls.Add(headMeno);

        string[] freeDays = x2Mysql.getFreeDays();

        for (int den = 0; den<dni; den++)
        {
            TableCell denCell = new TableCell();
            int rDen = den+1;
            denCell.Text = rDen.ToString();
            denCell.HorizontalAlign = HorizontalAlign.Center;
            headRow.Controls.Add(denCell);
        }

       

        for (int row = 0; row < tableCnt; row++)
        {
            TableRow riadok = new TableRow();
            this.komplview_tbl.Controls.Add(riadok);

            TableCell menoCell = new TableCell();
            menoCell.ID = "dovNameCell_" + row.ToString();
            menoCell.Text = table[row]["name3"].ToString().TrimStart();

            if (row % 2 == 0)
            {
                menoCell.BackColor = System.Drawing.Color.LightGray;
                menoCell.BorderStyle = BorderStyle.Solid;
                menoCell.BorderWidth = Unit.Pixel(3);
                menoCell.BorderColor = System.Drawing.Color.LightGray;
                
            }
            

            riadok.Controls.Add(menoCell);

            for (int den = 0; den < dni; den++)
            {
                int rDen = den + 1;

                int weekDay = (int)new DateTime(rok, mesiac, rDen).DayOfWeek;

                int dnesJe = DateTime.Today.Day;

                TableCell statusCell = new TableCell();
                statusCell.ID = "stCell_" + rDen.ToString() + "_" + table[row]["id"].ToString();
                statusCell.Width = Unit.Pixel(30);
                statusCell.ToolTip = table[row]["full_name"].ToString().Trim();
                statusCell.ToolTip += "\r\n";

                

                if (dnesJe == rDen)
                {
                    statusCell.BorderColor = System.Drawing.Color.Black;
                    statusCell.BorderStyle = BorderStyle.Dotted;
                    
                }

                if (weekDay == 0 || weekDay == 6)
                {
                    statusCell.BackColor = System.Drawing.Color.FromArgb(0xD46A6A);
                }
                else
                {
                    statusCell.BackColor = System.Drawing.Color.FromArgb(0x80BA5D);
                }

                string denMes = rDen.ToString() + "." + mesiac.ToString();
                if (Array.IndexOf(freeDays, denMes) != -1)
                {
                    statusCell.BackColor = System.Drawing.Color.FromArgb(0xD4BF6A);
                }
                riadok.Controls.Add(statusCell);
            }
        }
    }
}