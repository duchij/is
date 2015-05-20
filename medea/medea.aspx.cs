using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class helpers_medea : System.Web.UI.Page
{
    x2_var x2 = new x2_var();
    medea mdb = new medea();
    log x2log = new log();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loadData(10);
    }

    protected void runSqlFnc(object sender, EventArgs e)
    {
        string rowStr = this.rows_txt.Text.ToString();
        int rows = 0;
        try
        {
            rows = Convert.ToInt32(rowStr);
        }
        catch(Exception ex)
        {
            this.msg_lbl.Text = ex.ToString();
            rows = 10;
        }
        this.loadData(rows);
    }

    protected void loadData(int rows)
    {
        DateTime dt = DateTime.Now.AddMinutes(-1);

        string time = dt.Hour.ToString() + dt.Minute.ToString()+dt.Second.ToString();

        string queryIn = "SELECT * FROM ADMINSQL.klinlog_view ";
        queryIn += "WHERE datum = '2015-05-20' and cas > {0}00 ";
        queryIn += "AND scpac <> 0";
        
        string query = x2.sprintf(queryIn, new string[] { time });

        this.msg_lbl.Text = query;

       // string query = "SELECT name, snapshot_isolation_state_desc, is_read_committed_snapshot_on FROM sys.databases";

        //SortedList res = mdb.execute(query);
        //x2log.logData(res, "", "sp_who mssql");


        Dictionary<int, Hashtable> data = mdb.getTable(query);


        int dataLn = data.Count;

        if (dataLn > 0)
        {
            Table dataTbl = new Table();
            dataTbl.Width = Unit.Percentage(100);

            this.data_plh.Controls.Add(dataTbl);

            TableHeaderRow headerRow = new TableHeaderRow();
            headerRow.BackColor = System.Drawing.Color.Gray;
            dataTbl.Controls.Add(headerRow);

            int headerLn = data[0].Count;

            foreach (DictionaryEntry head in data[0])
            {
                TableHeaderCell datCell = new TableHeaderCell();
                datCell.Text = head.Key.ToString();
                headerRow.Controls.Add(datCell);
            }

            for (int i = 0; i < dataLn; i++)
            {
                TableRow riadok = new TableRow();
                dataTbl.Controls.Add(riadok);
                foreach (DictionaryEntry row in data[i])
                {
                    TableCell dataCell = new TableCell();
                    dataCell.Text = row.Value.ToString();
                    riadok.Controls.Add(dataCell);
                }

            }
        }
        else
        {
            this.msg_lbl.Text = query+"<br>"+"<br> "+"Asi lock skus refreshnut stranku <br>";
        }
        

        

        


    }
}