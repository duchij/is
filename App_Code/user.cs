using System;

using System.Configuration;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Odbc;
using System.Text;

/// <summary>
/// Summary description for user
/// </summary>
public class user: my_db
{
	public user()
	{
		//
		// TODO: Add constructor logic here
		//
	}

   

    public DataSet getAllUsersList(int clinic)
    {
        StringBuilder sb = new StringBuilder();
        if (clinic == 0)
        {
            sb.Append("SELECT [id],[full_name],[name],[prava] FROM [is_users]");
        }
        else
        {
            sb.AppendFormat("SELECT [id],[full_name],[name],[prava] FROM [is_users] WHERE [klinika] ='{0}'",clinic);
        }
        string query = sb.ToString();
        my_con.Open();

        OdbcDataAdapter da = new OdbcDataAdapter(this.parseQuery(query), my_con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }

    public DataSet searchUsersByName(string name,string klinika_id)
    {
        string query = "SELECT [id],[full_name],[name],[prava] FROM [is_users] WHERE [full_name] LIKE '%{0}%' AND [klinika]='{1}'";

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(this.parseQuery(query.ToString()), name, klinika_id);
        my_con.Open();

        OdbcDataAdapter da = new OdbcDataAdapter(sb.ToString(), my_con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }


}
