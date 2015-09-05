using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Data;
using System.Data.Odbc;

/// <summary>
/// Summary description for vzp
/// </summary>
public class vzp : mysql_db
{
	public vzp()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataSet fillVzpDataSet(string what, string data, string year)
    {
        //DataSet result = 
        string query = "";

        switch (what)
        {
            case "firm":
                query = @"      SELECT [t_vzp.item_lf_id] AS [lf_id], [t_vzp.vzp] AS [vzp],[t_vzp.item_date] AS [item_date],
                                    [t_users.full_name] AS [user_name], [t_firms.item_label] AS [firm_name]
                                    FROM [is_sklad_vzp] AS [t_vzp]
                                INNER JOIN [is_sklad_firms] AS [t_firms] ON [t_firms.item_id] = [t_vzp.item_firm]
                                INNER JOIN [is_users] AS [t_users] ON [t_users.id] = [t_vzp.user_id] 
                                    WHERE [t_vzp.item_firm] = {0} AND YEAR([item_date])='{1}'
                        ";
                break;
            case "vzp":
                query = @"      SELECT [t_vzp.item_lf_id] AS [lf_id], [t_vzp.vzp] AS [vzp],[t_vzp.item_date] AS [item_date],
                                    [t_users.full_name] AS [user_name], [t_firms.item_label] AS [firm_name]
                                    FROM [is_sklad_vzp] AS [t_vzp]
                                INNER JOIN [is_sklad_firms] AS [t_firms] ON [t_firms.item_id] = [t_vzp.item_firm]
                                INNER JOIN [is_users] AS [t_users] ON [t_users.id] = [t_vzp.user_id] 
                                    WHERE [t_vzp.vzp] = {0} AND YEAR([item_date])='{1}'
                        ";
                break;
            case "all":
                query = @"      SELECT [t_vzp.item_lf_id] AS [lf_id], [t_vzp.vzp] AS [vzp],[t_vzp.item_date] AS [item_date],
                                    [t_users.full_name] AS [user_name], [t_firms.item_label] AS [firm_name]
                                    FROM [is_sklad_vzp] AS [t_vzp]
                                INNER JOIN [is_sklad_firms] AS [t_firms] ON [t_firms.item_id] = [t_vzp.item_firm]
                                INNER JOIN [is_users] AS [t_users] ON [t_users.id] = [t_vzp.user_id] 
                                    ORDER BY [t_vzp.item_date] LIMIT 100;
                        ";
                break;
        }

        my_con.Open();

        string sql = this.buildSql(query, new string[] { data,year });

        OdbcDataAdapter da = new OdbcDataAdapter(sql, this.my_con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }

}