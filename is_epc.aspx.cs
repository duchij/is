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

        string dateGroup = Session["epc_date_group"].ToString();


        int rok = Convert.ToInt32(Session["epc_rok"]);
        int mesiac = Convert.ToInt32(Session["epc_mesiac"]);

        int dni = DateTime.DaysInMonth(rok, mesiac);

        string mesStr = dateGroup.Substring(4, 2);

        string zacDt = rok.ToString()+"-"+mesStr.ToString()+"-"+"01";
        string koncDt = rok.ToString()+"-"+mesStr.ToString()+"-"+dni.ToString();
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("SELECT [hlasko_epc].*,[hlasko].[dat_hlas] AS [datum_hlasenia], [hlasko].[type] AS [typ_sluzby]");
        sb.AppendLine("FROM [is_hlasko_epc] AS [hlasko_epc]");
        sb.AppendLine("INNER JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id] = [hlasko_epc].[hlasko_id]");
        sb.AppendFormat("WHERE [hlasko_epc].[user_id] = '{0}'",Convert.ToInt32(Session["user_id"]));
        sb.AppendFormat("AND [hlasko].[dat_hlas] BETWEEN '{0}' AND '{1}' ORDER BY [hlasko_epc].[work_start] ASC",zacDt,koncDt);

        Dictionary<int, SortedList> table = x2Mysql.getTableSL(sb.ToString());
        
        int tableLn = table.Count;

        TableHeaderRow headerRow = new TableHeaderRow();
        this.epc_tbl.Controls.Add(headerRow);

        TableHeaderCell headcell1 = new TableHeaderCell();
        headcell1.Text = "Zaciatok prace";
        headcell1.HorizontalAlign = HorizontalAlign.Left;
        headcell1.Width = Unit.Pixel(120);
        headerRow.Controls.Add(headcell1);

        TableHeaderCell headcell2 = new TableHeaderCell();
        headcell2.Text = "Koniec prace";
        headcell2.HorizontalAlign = HorizontalAlign.Left;
        headcell2.Width = Unit.Pixel(120);
        headerRow.Controls.Add(headcell2);

        TableHeaderCell headcell3 = new TableHeaderCell();
        headcell3.Text = "Meno pacienta";
        headcell3.HorizontalAlign = HorizontalAlign.Left;
        headcell3.Width = Unit.Pixel(120);
        headerRow.Controls.Add(headcell3);

        TableHeaderCell headcell4 = new TableHeaderCell();
        headcell4.Text = "Popis prace";
        headcell4.Width = Unit.Pixel(300);
        headcell4.HorizontalAlign = HorizontalAlign.Left;
        headerRow.Controls.Add(headcell4);

        if (tableLn > 0)
        {
            for (int row = 0; row < tableLn; row++)
            {
                string date = table[row]["datum_hlasenia"].ToString();

                if (row == 0)
                {
                    this.newRowData(row, table);
                }
                else
                {
                    if (table[row]["datum_hlasenia"].ToString() == table[row - 1]["datum_hlasenia"].ToString())
                    {
                        this.newData(row, table); 
                    }
                    else
                    {
                        this.newRowData(row, table);
                    }
                }
            }
        }
    }

    protected void newRowData(int row, Dictionary<int, SortedList> table)
    {
        TableRow riadok = new TableRow();
        riadok.BorderWidth = Unit.Point(2);
        riadok.BorderStyle = BorderStyle.Solid;
        riadok.BorderColor = System.Drawing.Color.Black;
        this.epc_tbl.Controls.Add(riadok);

        TableCell kompl = new TableCell();
       
        kompl.ColumnSpan = 4;
        kompl.Font.Bold = true;
        kompl.Text = "Sluzba: " + table[row]["typ_sluzby"].ToString() + " Dna:" + table[row]["datum_hlasenia"].ToString();
        riadok.Controls.Add(kompl);


        TableRow riadok2 = new TableRow();
        riadok2.BorderWidth = Unit.Point(1);
        riadok2.BorderStyle = BorderStyle.Solid;
        riadok2.BorderColor = System.Drawing.Color.Black;
        this.epc_tbl.Controls.Add(riadok2);
        TableCell celldata1 = new TableCell();
        DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["work_start"].ToString()));
        celldata1.Text = dt.ToString();
        riadok2.Controls.Add(celldata1);

        TableCell celldata2 = new TableCell();
        DateTime dt2 = dt.AddMinutes(Convert.ToInt32(table[row]["work_time"]));
        celldata2.Text = dt2.ToString();
        riadok2.Controls.Add(celldata2);

        TableCell celldata3 = new TableCell();
        celldata3.Text = table[row]["patient_name"].ToString();
        riadok2.Controls.Add(celldata3);

        TableCell celldata4 = new TableCell();
        celldata4.Text = x2.DecryptString(table[row]["work_text"].ToString(), Session["passphrase"].ToString());
        celldata4.Font.Size = FontUnit.Point(8);
        riadok2.Controls.Add(celldata4);
    }

    protected void newData(int row, Dictionary<int, SortedList> table)
    {
        TableRow riadok = new TableRow();
        this.epc_tbl.Controls.Add(riadok);

        TableCell celldata1 = new TableCell();
        DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["work_start"].ToString()));
        celldata1.Text = dt.ToString();
        riadok.Controls.Add(celldata1);

        TableCell celldata2 = new TableCell();
        DateTime dt2 = dt.AddMinutes(Convert.ToInt32(table[row]["work_time"]));
        celldata2.Text =dt2.ToString();
        riadok.Controls.Add(celldata2);

        TableCell celldata3 = new TableCell();
        celldata3.Text = table[row]["patient_name"].ToString();
        riadok.Controls.Add(celldata3);

        TableCell celldata4 = new TableCell();
        celldata4.Text = x2.DecryptString(table[row]["work_text"].ToString(), Session["passphrase"].ToString());
        celldata4.Font.Size = FontUnit.Point(8);
        riadok.Controls.Add(celldata4);
    }

}