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
    log x2log = new log();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        this.makeHeader();
        this.loadData();
    }

    protected void makeHeader()
    {
        this.menoTitul_lbl.Text = Session["titul_pred"].ToString() + Session["fullname"].ToString() + " " + Session["titul_za"].ToString();
        this.pracovisko_lbl.Text = Resources.Resource.pracovisko;
        this.zaradenie_lbl.Text = Session["zaradenie"].ToString();
        this.osobne_lbl.Text = Session["osobcisl"].ToString();
    }

    protected void loadData()
    {

        string dateGroup = Session["epc_date_group"].ToString();


        int rok = Convert.ToInt32(Session["epc_rok"]);
        int mesiac = Convert.ToInt32(Session["epc_mesiac"]);

        int dni = DateTime.DaysInMonth(rok, mesiac);

        string mesStr = dateGroup.Substring(4, 2);

        DateTime denPO = new DateTime(rok,mesiac,dni,7,59,0);
        denPO = denPO.AddDays(1);
        string zacDt = rok.ToString()+"-"+mesStr.ToString()+"-"+"01";
      //  string koncDt = rok.ToString()+"-"+mesStr.ToString()+"-"+dni.ToString();
        string koncDt = x2.unixDate(denPO);

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("SELECT `hlasko_epc`.*,[hlasko.dat_hlas] AS [datum_hlasenia], [hlasko.type] AS [typ_sluzby]");
        sb.AppendLine("FROM [is_hlasko_epc] AS [hlasko_epc]");
        sb.AppendLine("INNER JOIN [is_hlasko] AS [hlasko] ON [hlasko.id] = [hlasko_epc.hlasko_id]");
        sb.AppendFormat("WHERE [hlasko_epc.user_id] = '{0}'",Convert.ToInt32(Session["user_id"]));
        sb.AppendFormat("AND [hlasko_epc.work_start] BETWEEN '{0} 00:00:00' AND '{1} 07:59:00' ORDER BY [hlasko_epc.work_start] ASC",zacDt,koncDt);

        Dictionary<int, SortedList> table = x2Mysql.getTableSL(sb.ToString());
        //x2log.logData(table, "", "tabulka sluzieb");


        sb.Length = 0;
        sb.AppendLine("SELECT [hlasko.dat_hlas] AS [datum],[hlasko.type] AS [sluzba_typ],[hlasko_epc.user_id], SUM([work_time]) AS [worktime]");
        sb.AppendLine("FROM [is_hlasko_epc] as [hlasko_epc]");
        sb.AppendLine("LEFT JOIN [is_hlasko] AS [hlasko] ON [hlasko.id]=[hlasko_epc.hlasko_id]");
        sb.AppendFormat("WHERE [hlasko_epc.work_start] BETWEEN '{0} 00:00:00' AND '{1} 07:59:00'", zacDt, koncDt);
        sb.AppendFormat("AND [user_id]='{0}'", Session["user_id"].ToString());
        sb.AppendLine("GROUP BY [hlasko_epc.hlasko_id]");
        sb.AppendLine("ORDER BY [hlasko.dat_hlas]");

        Dictionary<int, Hashtable> statTable = x2Mysql.getTable(sb.ToString());


        
        int tableLn = table.Count;

        TableHeaderRow headerRow = new TableHeaderRow();
        this.epc_tbl.Controls.Add(headerRow);

        TableHeaderCell headcell1 = new TableHeaderCell();
        headcell1.Text = Resources.Resource.epc_work_start;
        headcell1.HorizontalAlign = HorizontalAlign.Left;
        headcell1.Width = Unit.Pixel(120);
        headerRow.Controls.Add(headcell1);

        TableHeaderCell headcell2 = new TableHeaderCell();
        headcell2.Text = Resources.Resource.epc_work_end;
        headcell2.HorizontalAlign = HorizontalAlign.Left;
        headcell2.Width = Unit.Pixel(120);
        headerRow.Controls.Add(headcell2);

        TableHeaderCell headcell3 = new TableHeaderCell();
        headcell3.Text = Resources.Resource.epc_patient_name;
        headcell3.HorizontalAlign = HorizontalAlign.Left;
        headcell3.Width = Unit.Pixel(120);
        headerRow.Controls.Add(headcell3);

        TableHeaderCell headcell4 = new TableHeaderCell();
        headcell4.Text = Resources.Resource.epc_work_time;
        headcell4.Width = Unit.Pixel(50);
        headcell4.HorizontalAlign = HorizontalAlign.Left;
        headerRow.Controls.Add(headcell4);

        TableHeaderCell headcell5 = new TableHeaderCell();
        headcell5.Text = Resources.Resource.epc_work_text;
        headcell5.Width = Unit.Pixel(300);
        headcell5.HorizontalAlign = HorizontalAlign.Left;
        headerRow.Controls.Add(headcell5);
        int stat = 0;
        if (tableLn > 0)
        {
            for (int row = 0; row < tableLn; row++)
            {
                string date = table[row]["datum_hlasenia"].ToString();

                if (row == 0)
                {
                    this.newRowData(row, table,statTable,stat);
                    stat++;
                }
                else
                {
                    if (table[row]["datum_hlasenia"].ToString() == table[row - 1]["datum_hlasenia"].ToString())
                    {
                        this.newData(row, table); 
                    }
                    else
                    {
                        this.newRowData(row, table,statTable,stat);
                        stat++;
                    }
                }
            }
        }
       // this.finalStat(zacDt, koncDt);
    }

    //protected void finalStat(string zacDt, string koncDt)
    //{

    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendLine("SELECT [hlasko].[dat_hlas] AS [datum],[hlasko].[type] AS [sluzba_typ],[hlasko_epc].[user_id], SUM([work_time]) AS [worktime]"); 
    //    sb.AppendLine("FROM [is_hlasko_epc] as [hlasko_epc]");
    //    sb.AppendLine("LEFT JOIN [is_hlasko] AS [hlasko] ON [hlasko].[id]=[hlasko_epc].[hlasko_id]");
    //    sb.AppendFormat("WHERE [hlasko_epc].[work_start] BETWEEN '{0}' AND '{1}'",zacDt,koncDt);
    //    sb.AppendFormat("AND [user_id]='{0}'",Session["user_id"].ToString());
    //    sb.AppendLine("GROUP BY [hlasko_epc].[hlasko_id]");

    //    Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

    //    string result = "";

    //    int tableLn = table.Count;
    //    for (int i = 0; i < tableLn; i++)
    //    {
    //        int min = Convert.ToInt32(table[i]["worktime"]);

    //        decimal celkHod = min / 60;
    //        result += "<p><strong>Dátum:</strong> " + x2.MSDate(table[i]["datum"].ToString()) + " <strong>Služba:</strong> " + table[i]["sluzba_typ"].ToString() + " <strong>Odpracované hodiny: </strong> " + celkHod.ToString()+"</p>";

    //    }
    //    this.finalStat_lbl.Text = result;

    //}

    protected void newRowData(int row, Dictionary<int, SortedList> table, Dictionary<int,Hashtable>statDat,int stat)
    {
        TableRow riadok = new TableRow();
        this.epc_tbl.Controls.Add(riadok);
        riadok.BorderWidth = Unit.Pixel(2);
        riadok.BorderStyle = BorderStyle.Solid;
        riadok.BorderColor = System.Drawing.Color.Black;
        

        TableCell kompl = new TableCell();
       
        
        kompl.BorderWidth = Unit.Point(1);
        kompl.BorderStyle = BorderStyle.Solid;
        kompl.BorderColor = System.Drawing.Color.Black;
       
        kompl.ColumnSpan = 4;
        kompl.Style.Add("padding", "5px");
        kompl.Font.Bold = true;
        kompl.Text = "Služba: " + table[row]["typ_sluzby"].ToString() + " Dňa: " + x2.MSDate(table[row]["datum_hlasenia"].ToString());
        int min = 0;

        try
        {
            min = Convert.ToInt32(statDat[stat]["worktime"]);
        }
        catch (Exception e)
        {
            
            min = 0;
            x2log.logData(statDat, e.ToString(), "error in statDat");
        }

        decimal celkHod = min / 60;
        
        kompl.Text += " <strong>Odpracované hodiny: </strong> " + celkHod.ToString() + "</p>";
        riadok.Controls.Add(kompl);


        TableRow riadok2 = new TableRow();
        riadok2.BorderWidth = Unit.Point(1);
        riadok2.BorderStyle = BorderStyle.Solid;
        riadok2.BorderColor = System.Drawing.Color.Black;
        this.epc_tbl.Controls.Add(riadok2);

        TableCell celldata1 = new TableCell();
        celldata1.Style.Add("padding", "3px");
        celldata1.BorderWidth = Unit.Point(1);
        celldata1.BorderStyle = BorderStyle.Solid;
        celldata1.BorderColor = System.Drawing.Color.Black;
        celldata1.VerticalAlign = VerticalAlign.Top;
        DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["work_start"].ToString()));
        celldata1.Font.Size = FontUnit.Point(8);
        celldata1.Text = dt.ToString();
        riadok2.Controls.Add(celldata1);

        TableCell celldata2 = new TableCell();
        celldata2.Style.Add("padding", "3px");
        celldata2.BorderWidth = Unit.Point(1);
        celldata2.BorderStyle = BorderStyle.Solid;
        celldata2.BorderColor = System.Drawing.Color.Black;
        celldata2.VerticalAlign = VerticalAlign.Top;
        DateTime dt2 = dt.AddMinutes(Convert.ToInt32(table[row]["work_time"]));
        celldata2.Font.Size = FontUnit.Point(8);
        celldata2.Text = dt2.ToString();
        riadok2.Controls.Add(celldata2);

        TableCell celldata3 = new TableCell();
        celldata3.Style.Add("padding", "3px");
        celldata3.BorderWidth = Unit.Point(1);
        celldata3.BorderStyle = BorderStyle.Solid;
        celldata3.BorderColor = System.Drawing.Color.Black;
        celldata3.Font.Size = FontUnit.Point(8);
        celldata3.VerticalAlign = VerticalAlign.Top;
        celldata3.Text = table[row]["patient_name"].ToString();
        riadok2.Controls.Add(celldata3);

        TableCell celldata4 = new TableCell();
        celldata4.Style.Add("padding", "3px");
        celldata4.BorderWidth = Unit.Point(1);
        celldata4.BorderStyle = BorderStyle.Solid;
        celldata4.BorderColor = System.Drawing.Color.Black;
        celldata4.Font.Size = FontUnit.Point(8);
        celldata4.VerticalAlign = VerticalAlign.Top;
        celldata4.Text = table[row]["work_time"].ToString();
        celldata4.Font.Size = FontUnit.Point(8);
        riadok2.Controls.Add(celldata4);

        TableCell celldata5 = new TableCell();
        celldata5.Style.Add("padding", "3px");
        celldata5.BorderWidth = Unit.Point(1);
        celldata5.BorderStyle = BorderStyle.Solid;
        celldata5.VerticalAlign = VerticalAlign.Top;
        celldata5.BorderColor = System.Drawing.Color.Black;
        
        celldata5.Text = x2.DecryptString(table[row]["work_text"].ToString(), Session["passphrase"].ToString());

        if (table[row]["lf_id"].ToString() != "NULL")
        {
            celldata5.Text += "<br><br><a href='lf.aspx?id=" + table[row]["lf_id"].ToString() + "' target='_blank' style='font-size:large;'>Priloha....</a>";
        }

        celldata5.Font.Size = FontUnit.Point(8);
        riadok2.Controls.Add(celldata5);
    }

    protected void newData(int row, Dictionary<int, SortedList> table)
    {
        TableRow riadok = new TableRow();
        this.epc_tbl.Controls.Add(riadok);

        TableCell celldata1 = new TableCell();
        celldata1.Style.Add("padding", "3px");
        celldata1.BorderWidth = Unit.Point(1);
        celldata1.BorderStyle = BorderStyle.Solid;
        celldata1.BorderColor = System.Drawing.Color.Black;
        celldata1.VerticalAlign = VerticalAlign.Top;
        celldata1.Font.Size = FontUnit.Point(8);
        DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[row]["work_start"].ToString()));
        celldata1.Text = dt.ToString();
        riadok.Controls.Add(celldata1);

        TableCell celldata2 = new TableCell();
        celldata2.Style.Add("padding", "3px");
        celldata2.BorderWidth = Unit.Point(1);
        celldata2.BorderStyle = BorderStyle.Solid;
        celldata2.Font.Size = FontUnit.Point(8);
        celldata2.BorderColor = System.Drawing.Color.Black;
        celldata2.VerticalAlign = VerticalAlign.Top;
        DateTime dt2 = dt.AddMinutes(Convert.ToInt32(table[row]["work_time"]));
        celldata2.Text =dt2.ToString();
        riadok.Controls.Add(celldata2);

        TableCell celldata3 = new TableCell();
        celldata3.Style.Add("padding", "3px");
        celldata3.BorderWidth = Unit.Point(1);
        celldata3.BorderStyle = BorderStyle.Solid;
        celldata3.Font.Size = FontUnit.Point(8);
        celldata3.BorderColor = System.Drawing.Color.Black;
        celldata3.VerticalAlign = VerticalAlign.Top;
        celldata3.Text = table[row]["patient_name"].ToString();
        riadok.Controls.Add(celldata3);

        TableCell celldata4 = new TableCell();
        celldata4.Style.Add("padding", "3px");
        celldata4.VerticalAlign = VerticalAlign.Top;
        celldata4.BorderWidth = Unit.Point(1);
        celldata4.BorderStyle = BorderStyle.Solid;
        celldata4.BorderColor = System.Drawing.Color.Black;
        celldata4.Font.Size = FontUnit.Point(8);
        celldata4.Text = table[row]["work_time"].ToString();
        celldata4.Font.Size = FontUnit.Point(8);
        riadok.Controls.Add(celldata4);


        TableCell celldata5 = new TableCell();
        celldata5.Style.Add("padding", "3px");
        celldata5.VerticalAlign = VerticalAlign.Top;
        celldata5.BorderWidth = Unit.Point(1);
        celldata5.BorderStyle = BorderStyle.Solid;
        celldata5.BorderColor = System.Drawing.Color.Black;
        celldata5.Font.Size = FontUnit.Point(8);
        celldata5.Text = x2.DecryptString(table[row]["work_text"].ToString(), Session["passphrase"].ToString());
        if (table[row]["lf_id"].ToString() != "NULL" )
        {
            celldata5.Text += "<br><br><a href='lf.aspx?id=" + table[row]["lf_id"].ToString() + "' target='_blank' style='font-size:large;'>Priloha....</a>";
        }
        celldata5.Font.Size = FontUnit.Point(8);
        riadok.Controls.Add(celldata5);

    }

}