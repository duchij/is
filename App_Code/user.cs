using System;

using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Odbc;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for user
/// </summary>
public class user: mysql_db
{

    x2_var x2 = new x2_var();
	public user()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public SortedList getUserInfoByID(string id)
    {
        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_users] WHERE [id]='{0}'", id);
        //string query = "SELECT * FROM [is_users] WHERE [id]=" + id;
        my_con.Open();

        MySqlCommand my_com = new MySqlCommand(this.parseQuery(sb.ToString()), my_con);

        MySqlDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetValue(i) == DBNull.Value)
                    {
                        result.Add(reader.GetName(i).ToString(), "");
                    }
                    else
                    {
                        result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                    }
                }
            }

        }
        else
        {
            result.Add("result", "none");
        }
        my_con.Close();


        return result;
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

        MySqlDataAdapter da = new MySqlDataAdapter(this.parseQuery(query), my_con);
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

        MySqlDataAdapter da = new MySqlDataAdapter(sb.ToString(), my_con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }

    public int checkIfLoginExists(string login)
    {
        string query = x2.sprintf("SELECT COUNT(*) AS [logins] FROM [is_users] WHERE [name] LIKE '{0}%'",new string[]{login});

        SortedList row = this.getRow(query);
        int result = 0;

        if (row["status"] == null)
        {
            result = Convert.ToInt32(row["logins"]);
        }
        else
        {
            result = -1;
            this.x2log.logData(row, row["msg"].ToString(), "error get row in checking login counts");
        }

        return result;
    }


}
