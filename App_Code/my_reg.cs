using System;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Configuration;
using System.Collections;
using System.Collections.Generic;

using System.Data.Odbc;


/// <summary>
/// Summary description for my_reg
/// </summary>
public class my_reg
{
    private OdbcConnection my_con = new OdbcConnection();

	public my_reg()
	{
		//
		// TODO: Add constructor logic here
		//
        //my_con.ConnectionString = "Driver={MySQL ODBC 3.51 Driver};Server=192.168.1.95;port=3306;uid=root;password=aa;Option=3;Database=kdch_sk;pooling=false";
     
        //my_con.ConnectionString = @"Driver={MySQL ODBC 5.1 Driver};Server=neptun;uid=kdch_sk;password=st160305;Option=3;Database=kdch_sk;";



        Configuration myConfig = WebConfigurationManager.OpenWebConfiguration("/is");
        ConnectionStringSettings connString;
        connString = myConfig.ConnectionStrings.ConnectionStrings["kdch_sk"];

        my_con.ConnectionString = connString.ToString();
    
    }

    public string insert_rows(string table, SortedList data)
    {
        string cols = "(";
        string values = "('";
        string parse_str;
        foreach (DictionaryEntry tmp in data)
        {
            cols = cols + tmp.Key + ",";
            parse_str = tmp.Value.ToString();
            values = values + parse_str.Replace("'","*") + "','";
        }
        cols = cols.Substring(0, cols.Length-1);
        values = values.Substring(0, values.Length-2);
        cols = cols + ")";
        values = values + ")";

        string query = "INSERT INTO "+table+" " + cols + " VALUES " + values;

        //return query;
        try
        {
            my_con.Open();

            OdbcCommand my_insert = new OdbcCommand(query, my_con);

            my_insert.ExecuteNonQuery();

            my_con.Close();
            //return true;
            return "ok";
        }
         catch (Exception e)
       {
           return e.ToString();
       }
   

    }

    


}
