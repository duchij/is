using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_opkniha : System.Web.UI.Page
{
    x2_var x2 = new x2_var();
    mysql_db x2Mysql = new mysql_db();
    log x2Log = new log();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void searchInDgFnc(object sender, EventArgs e)
    {
        string queryStr = this.queryDg_txt.Text.ToString().Trim();
        string[] queryArr = queryStr.Split(' ');

        int strLn = queryArr.Length;

        string finalLike = "";
        
        if (strLn > 0)
        {
            string[] arrTmp = new String[strLn];

            for (int i=0; i<strLn;i++)
            {
                arrTmp[i] = "'%" + queryArr[i] + "%'";
            }

            finalLike = string.Join(" OR ", arrTmp);
        }
        else
        {
            finalLike = "'%" + queryStr + "%'";
        }

        x2Log.logData(finalLike, "", "final query");

        string query = x2.sprintf("SELECT * FROM [is_opkniha] WHERE [diagnoza] LIKE {0} ORDER BY [datum] ", new String[] { finalLike });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        x2Log.logData(table, "", "query result");



    }
}