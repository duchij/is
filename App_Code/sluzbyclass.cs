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
using MySql.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for sluzbyclass
/// </summary>
public class sluzbyclass : my_db
{
    x2_var myx2 = new x2_var();

	public sluzbyclass()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public ArrayList getDoctorsForDl()
    {
        string query = "SELECT * FROM `is_users` WHERE (`group` = 'users' OR `group` = 'poweruser') AND `active` = 1  ORDER BY substr(`name` ,2) ";

        my_con.Open();

        MySqlCommand my_com = new MySqlCommand(query, my_con);
        MySqlDataReader reader = my_com.ExecuteReader();

        ArrayList result = new ArrayList();
        result.Add("0|-");

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result.Add(reader["id"].ToString()+"|"+myx2.getVykazName(reader["full_name"].ToString()));
            }
        }
        else
        {
            result.Add("nodata");
        }
        my_con.Close();
        return result;
    }

	 public SortedList getDoctorsForVykaz()
    {
        string query = "SELECT * FROM `is_users` WHERE (`group` = 'users' OR `group` = 'poweruser') AND `active` = 1  ORDER BY substr(`name` ,2) ";

        my_con.Open();

        MySqlCommand my_com = new MySqlCommand(query, my_con);
        MySqlDataReader reader = my_com.ExecuteReader();

        SortedList result = new SortedList();
        result.Add("0", "-");

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result.Add(reader["id"].ToString(), myx2.getVykazName(reader["full_name"].ToString()));
            }
        }
        else
        {
            result["nodata"] = "1";
        }
        my_con.Close();
        return result;
    }


    


}
