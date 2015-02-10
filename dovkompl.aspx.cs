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
   // x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        int mesiac = 2;
        int rok = 2015;
        this.init(rok,mesiac);
        this.loadActivities(rok,mesiac);
    }

    protected void loadActivities(int rok, int mesiac)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [is_dovolenky].[id] AS [dov_id],[is_dovolenky].[user_id],[is_dovolenky].[od], ");
        sb.AppendLine("[is_dovolenky].[do],[is_dovolenky].[type] FROM [is_dovolenky] ");
       // sb.AppendLine("INNER JOIN [is_dovolenky] ON [is_users].[id]=[is_dovolenky].[user_id] ");
        sb.AppendFormat("WHERE (MONTH([is_dovolenky].[od]) = '{0}' OR MONTH([is_dovolenky].[do]) = '{0}') ", mesiac);
        sb.AppendFormat("AND (YEAR([is_dovolenky].[od]) = '{0}' OR YEAR([is_dovolenky].[do]) = '{0}')", rok);
        sb.AppendFormat("AND [is_dovolenky].[clinics]='{0}' ORDER BY [is_dovolenky].[do] ASC", 3);

        Dictionary<int, Hashtable> data = x2Mysql.getTable(sb.ToString());
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

                if (hrsCount > 23)
                {
                    dayCount = 1;
                }
            }
            
            int startDay =  


            for (int den = tmp_od_mes.Day; den <= dayCount; den++)
            {
                if (den <= days)
                {
                    Control crtl = FindControl("stCell_" + den.ToString() + "_" + data[rec]["user_id"].ToString());
                    TableCell stCell = (TableCell)crtl;

                    if (data[rec]["type"].ToString() == "do")
                    {
                        
                        stCell.Text = "D";

                    }
                    if (data[rec]["type"].ToString() == "ci")
                    {
                       
                        stCell.Text = "C";

                    }
                }
                else
                {
                  // break;
                }
            }

        }



    }

    protected void init(int rok, int mesiac)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [id],[name2],[titul_pred],[full_name],[titul_za] FROM [is_users] ");
        sb.AppendLine("WHERE [active] = 1 AND [klinika] = 3 AND [work_group]='doctor' ");
        sb.AppendLine("ORDER BY [name2] ASC");

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

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
            menoCell.ID = "cell_" + row.ToString();
            menoCell.Text = table[row]["full_name"].ToString();
            
            riadok.Controls.Add(menoCell);

            for (int den = 0; den < dni; den++)
            {
                int rDen = den + 1;

                int weekDay = (int)new DateTime(rok, mesiac, rDen).DayOfWeek;

                TableCell statusCell = new TableCell();
                statusCell.ID = "stCell_" + rDen.ToString() + "_" + table[row]["id"].ToString();
                statusCell.Width = Unit.Pixel(30);
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