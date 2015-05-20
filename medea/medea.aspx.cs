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
        this.loadDataRDG();
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
        //this.loadDataRDG();
    }

    protected void loadDataRDG()
    {

        SortedList lastSyncRow = mdb.getRowSync("SELECT MAX(id) AS [last_id],[last_date],[last_time],[succes] FROM [rdg_view_log]");

        DateTime dt = new DateTime();
        DateTime date = new DateTime();

       if (lastSyncRow["last_id"].ToString() != "NULL")
       {
           if (lastSyncRow["succes"].ToString() == "yes")
            {
                dt = DateTime.Now.AddMinutes(-1);
                date = DateTime.Today;
            }
            else
            {
                dt = Convert.ToDateTime(lastSyncRow["last_time"]);
                date = Convert.ToDateTime(x2.UnixToMsDateTime(lastSyncRow["last_date"].ToString()));
            } 
       }
       else
       {
           dt = DateTime.Now.AddMinutes(-240);
           date = DateTime.Today;
       }

        


        //DateTime dt = DateTime.Now.AddMinutes(-1);
        string time = String.Format("{0:HH}",dt) + String.Format("{0:mm}",dt); //+dt.Second.ToString();
        string uDate = x2.unixDate(date); 
        //string queryIn = "SELECT * FROM ADMINSQL.klinlog_view ";
        //queryIn += "WHERE datum = '2015-05-20' and cas > {0}00 ";
        //queryIn += "AND scpac <> 0";
        string queryIn = "SELECT * FROM ADMINSQL.rdg_view ";
        queryIn += "WHERE datum = '{0}' AND cas >= {1} ";
        queryIn += "ORDER BY cas ASC";
        //string queryIn = "SELECT * FROM ADMINSQL.uzivatel_view ";
      //  queryIn += "WHERE datum = '2015-05-20' and cas > {0}00 ";
       // queryIn += "AND scpac <> 0";
        
        string query = x2.sprintf(queryIn, new string[] { uDate, time });

        this.msg_lbl.Text = query;

       // string query = "SELECT name, snapshot_isolation_state_desc, is_read_committed_snapshot_on FROM sys.databases";

        //SortedList res = mdb.execute(query);
        //x2log.logData(res, "", "sp_who mssql");


        Dictionary<int, Hashtable> data = mdb.getTable(query);

        Dictionary<int, Hashtable> saveData = new Dictionary<int, Hashtable>();

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
                saveData[i] = new Hashtable();
                foreach (DictionaryEntry row in data[i])
                {
                    

                   switch (row.Key.ToString())
                    {
                        case "cas":
                            saveData[i]["cas"] = row.Value.ToString().Substring(0, 2) + ":" + row.Value.ToString().Substring(2, 2)+":00";
                            break;
                        case "datum":
                            saveData[i]["datum"] = x2.unixDate(Convert.ToDateTime(data[i]["datum"].ToString()));
                            break;
                        case "B":
                            if (row.Value == null || row.Value.ToString().Trim().Length ==0) 
                            {
                                saveData[i].Add("B","NULL");
                            }
                            else
                            {
                                saveData[i]["B"]=row.Value;
                            }
                                    
                            break;
                        case "P":
                            if (row.Value == null)
                            {
                                saveData[i]["P"] = "NULL";
                            }
                            else
                            {
                                saveData[i]["P"] = row.Value;
                            }
                            break;
                        case "uzelzkr":
                            saveData[i]["uzol_kratko"] = row.Value;
                            break;
                        case "id_prac":
                            saveData[i]["id_pracoviska_uzol"] = row.Value;
                            break;
                        case "uzelnazov":
                            saveData[i]["uzol_nazov"] = row.Value;
                            break;
                        case "scpac":
                            saveData[i]["scpac"] = row.Value;
                            break;
                        case "sczad":
                            saveData[i]["sczad"] = row.Value;
                            break;
                        case"K":
                            if (row.Value == null || row.Value.ToString().Trim().Length == 0)
                            {
                                saveData[i]["K"] = "NULL";
                            }
                            else
                            {
                                saveData[i]["K"] = row.Value;
                            }
                            break;
                        case "N":
                            if (row.Value == null || row.Value.ToString().Trim().Length == 0)
                            {
                                saveData[i]["N"] = "NULL";
                            }
                            else
                            {
                                saveData[i]["N"] = row.Value;
                            }
                            break;
                        case "A":
                            if (row.Value == null || row.Value.ToString().Trim().Length == 0)
                            {
                                saveData[i]["A"] = "NULL";
                            }
                            else
                            {
                                saveData[i]["A"] = row.Value;
                            }
                            break;


                    }
                        
                    TableCell dataCell = new TableCell();
                    dataCell.Text = row.Value.ToString();
                    riadok.Controls.Add(dataCell);
                }

            }

           SortedList res = mdb.mysql_insert_arr("rdg_view_sync",saveData);

            if (Convert.ToBoolean(res["status"]))
            {

                SortedList lastRow = mdb.getRowSync("SELECT MAX(id) AS [last_id] FROM [rdg_view_sync]");

                SortedList logData = new SortedList();
                logData.Add("rdg_view_id", lastRow["last_id"]);
                logData.Add("last_date", saveData[dataLn-1]["datum"]);
                logData.Add("last_time", saveData[dataLn-1]["cas"]);
                logData.Add("succes", "yes");

                SortedList res1 = mdb.mysql_insert("rdg_view_log", logData);
            }
            else
            {
                SortedList logData = new SortedList();
                logData.Add("rdg_view_id", "NULL");
                logData.Add("last_date", saveData[dataLn-1]["datum"]);
                logData.Add("last_time", saveData[dataLn-1]["cas"]);
                logData.Add("succes", "no");
                logData.Add("log_msg", res["msg"].ToString());

                SortedList res1 = mdb.mysql_insert("rdg_view_log", logData);
            }


        }
        else
        {
            this.msg_lbl.Text = query+"<br>"+"<br> "+"Asi lock skus refreshnut stranku <br>";
        }
        

        

        


    }

    protected void saveRdgData()
    {

    }
}