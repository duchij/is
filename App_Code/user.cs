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

   

    public DataSet getAllUsersList()
    {

        string query = "SELECT * FROM is_users";
         my_con.Open();

        OdbcDataAdapter da = new OdbcDataAdapter(query, my_con.ConnectionString);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }

    public DataSet searchUsersByName(string name)
    {
        string query = "SELECT * FROM `is_users` WHERE `full_name` LIKE '%{0}%'";

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(query, name);
        my_con.Open();

        OdbcDataAdapter da = new OdbcDataAdapter(sb.ToString(), my_con.ConnectionString);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }


}
