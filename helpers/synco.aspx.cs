using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class helpers_synco : System.Web.UI.Page
{
    omegadb dbOmega = new omegadb();
    log x2log = new log();
    mysql_db x2Mysql = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void doSyncFnc(object sender, EventArgs e)
    {
        string value = this.kvValue_txt.Text.ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [st2_ms_kv] WHERE [key]='iesko' AND [value]='{0}'", value);

        Dictionary<int, Hashtable> table = dbOmega.getTable(sb.ToString());

        x2log.logData(table,"","omega kv lookup");
        if (table.Count >0)this.syncData(table);
        
         
    }

    protected void syncData(Dictionary<int, Hashtable> table)
    {
        int tableCn = table.Count;

        StringBuilder sb = new StringBuilder();
        Dictionary<int, Hashtable> dataIns = new Dictionary<int, Hashtable>();
        for (int i = 0; i < tableCn; i++)
        {
            int msViewId = Convert.ToInt32(table[i]["ms_id"]);

            sb.AppendLine("SELECT [views_data].[view_ms_id] AS [view_id], [views_data].[item_ms_id] AS [ms_item_id],"); 
            sb.AppendLine("[t_resources].[item_name],[t_accounts].[account_login] AS [login] ");
            sb.AppendLine("FROM [st2_msp_views_data] AS [views_data] ");
            sb.AppendLine("LEFT JOIN [st2_msp_resources] AS [t_resources] ON [t_resources].[item_id] = [views_data].[item_ms_id] ");
            sb.AppendLine("LEFT JOIN [st2_accounts] AS [t_accounts] ON [t_accounts].[account_resource_id] = [views_data].[item_ms_id] ");
            sb.AppendFormat("WHERE [views_data].[view_ms_id]='{0}'",msViewId);

            Dictionary<int, Hashtable> res = dbOmega.getTable(sb.ToString());
            int resCnt = res.Count;
            x2log.logData(res, "", "omega sql");
            //Dictionary<int,Hashtable> data

            if (resCnt >0 )
            {
                for (int row=0; row<resCnt; row++)
                {
                    Hashtable tmp = new Hashtable();
                    tmp.Add("view_id",res[row]["view_id"]);
                    tmp["ms_item_id"] = res[row]["ms_item_id"];
                    tmp["full_name"] = res[row]["item_name"];
                    tmp["name"] = this.makeName(res[row]["item_name"].ToString().Trim());
                    tmp["login"] = res[row]["login"];
                    tmp["clinic"] = 4;

                    dataIns.Add(row, tmp);

                }

                SortedList res2 = x2Mysql.mysql_insert_arr("is_omega_doctors",dataIns);
            }
        }
    }
    
    protected string makeName(string name)
    {
        int pos = name.IndexOf(" ");

        string firstL = name.Substring(0, 1);

        return name.Substring(pos, name.Length-pos)+", "+firstL+"."; 
    }

}