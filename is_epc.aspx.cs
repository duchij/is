using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_epc : System.Web.UI.Page
{
    public mysql_db x2Mysql = new mysql_db();
    public x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loadData();
    }

    protected void loadData()
    {

        StringBuilder sb = new StringBuilder();
       // sb.AppendFormat("SELECT [datum],[user_id],[typ] FROM [is_sluzby_2] WHERE [date_group]={0} AND [user_id]={1} AND [typ]!='prijm' ORDER BY [datum] ASC", Convert.ToInt32(Session["date_group"]), Convert.ToInt32(Session["user_id"]));
        
        //Dictionary<int,SortedList> tableDatum = x2Mysql.getTableSL(sb.ToString());

       // sb.Length = 0;

        sb.AppendLine("SELECT [hlasko_epc].*,[hlasko].[dat_hlas] AS [datum_hlasenia]");
        sb.AppendLine("FROM [is_hlasko_epc] AS [hlasko_epc]");
        sb.AppendLine("INNER JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id] = [hlasko_epc].[hlasko_id]");
        sb.AppendLine("WHERE [hlasko_epc].[user_id] = 2");
        sb.AppendLine("AND [hlasko].[dat_hlas] BETWEEN '2015-01-01' AND '2015-01-31' ORDER BY [hlasko_epc].[work_start] ASC");

        Dictionary<int, SortedList> table = x2Mysql.getTableSL(sb.ToString());
        
        int tableLn = table.Count;

        if (tableLn > 0)
        {
            for (int row = 0; row < tableLn; row++)
            {
                string date = table[row]["datum_hlasenia"].ToString();

                if (row == 0)
                {
                    TableRow riadok = new TableRow();
                    this.epc_tbl.Controls.Add(riadok);

                    TableCell kompl = new TableCell();
                    kompl.ColumnSpan = 4;
                    kompl.Text = "lolo"+ date;
                    riadok.Controls.Add(kompl);


                    TableRow riadok2 = new TableRow();
                    this.epc_tbl.Controls.Add(riadok2);
                    TableCell celldata1 = new TableCell();
                    celldata1.Text = table[row]["work_start"].ToString();
                    riadok2.Controls.Add(celldata1);

                    TableCell celldata2 = new TableCell();
                    celldata2.Text = table[row]["work_time"].ToString();
                    riadok2.Controls.Add(celldata2);

                    TableCell celldata3 = new TableCell();
                    celldata3.Text = table[row]["patient_name"].ToString();
                    riadok2.Controls.Add(celldata3);

                    TableCell celldata4 = new TableCell();
                    celldata4.Text = x2.DecryptString(table[row]["work_text"].ToString(),Session["passphrase"].ToString());
                    riadok2.Controls.Add(celldata4);

                }
                else
                {
                    if (table[row]["datum_hlasenia"].ToString() == table[row - 1]["datum_hlasenia"].ToString())
                    {
                        TableRow riadok = new TableRow();
                        this.epc_tbl.Controls.Add(riadok);

                        TableCell celldata1 = new TableCell();
                        celldata1.Text = table[row]["work_start"].ToString();
                        riadok.Controls.Add(celldata1);

                        TableCell celldata2 = new TableCell();
                        celldata2.Text = table[row]["work_time"].ToString();
                        riadok.Controls.Add(celldata2);

                        TableCell celldata3 = new TableCell();
                        celldata3.Text = table[row]["patient_name"].ToString();
                        riadok.Controls.Add(celldata3);

                        TableCell celldata4 = new TableCell();
                        celldata4.Text = table[row]["work_text"].ToString();
                        riadok.Controls.Add(celldata4);
                    }
                    else
                    {
                        TableRow riadok = new TableRow();
                        this.epc_tbl.Controls.Add(riadok);

                        TableCell kompl = new TableCell();
                        kompl.ColumnSpan = 4;
                        kompl.Text = "66"+ table[row]["datum_hlasenia"].ToString();
                        riadok.Controls.Add(kompl);

                        TableRow riadok2 = new TableRow();
                        this.epc_tbl.Controls.Add(riadok2);
                        TableCell celldata1 = new TableCell();
                        celldata1.Text = table[row]["work_start"].ToString();
                        riadok2.Controls.Add(celldata1);

                        TableCell celldata2 = new TableCell();
                        celldata2.Text = table[row]["work_time"].ToString();
                        riadok2.Controls.Add(celldata2);

                        TableCell celldata3 = new TableCell();
                        celldata3.Text = table[row]["patient_name"].ToString();
                        riadok2.Controls.Add(celldata3);

                        TableCell celldata4 = new TableCell();
                        celldata4.Text = table[row]["work_text"].ToString();
                        riadok2.Controls.Add(celldata4);

                    }
                }
            }
        }
        //sb.
    }
}