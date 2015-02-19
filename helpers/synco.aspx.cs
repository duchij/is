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
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void doSyncFnc(object sender, EventArgs e)
    {
        string value = this.kvValue_txt.Text.ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [st2_ms_kv] WHERE [key]='iesko_sync' AND [value]='{0}'", value);

        Dictionary<int, Hashtable> table = dbOmega.getTable(sb.ToString());

        //x2log.logData(table,"","omega kv lookup");
        if (table.Count >0)this.syncData(table);
        
         
    }

    protected void syncData(Dictionary<int, Hashtable> table)
    {
        int tableCn = table.Count;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < tableCn; i++)
        {
            int msViewId = Convert.ToInt32(table[i]["ms_id"]);

            sb.AppendFormat("SELECT ");
        }

    }


}