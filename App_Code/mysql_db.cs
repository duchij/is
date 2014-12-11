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
/*
 * try
        {
            OdbcTransaction trans1 = null;

            String sConString = "DRIVER={MySQL ODBC 3.51 Driver}; SERVER=192.168.10.69;DATABASE=wms; UID=root; PASSWORD=rmysql; OPTION=3";
            OdbcConnection oConnection = new OdbcConnection(sConString);
            oConnection.Open();
             trans1 = oConnection.BeginTransaction();
           try
            {
              
           
                OdbcCommand cmdtrans = new OdbcCommand();
                cmdtrans.Connection = oConnection;
                cmdtrans.Transaction = trans1;
                try
                {
                    cmdtrans.CommandText = "update dynamos set fname='chandrak1' where dl_no ='DL000371'";
                  int rowsaffected = cmdtrans.ExecuteNonQuery();
                  if (rowsaffected > 0)
                  {
                    //-- trans1.Commit();
                      trans1.Rollback();
                  }
                }

catch(Excption ex)*/




/// <summary>
/// Summary description for mysql_db
/// </summary>
public class mysql_db
{
    public OdbcConnection my_con = new OdbcConnection();

	public mysql_db()
	{
		//
		// TODO: Add constructor logic here
		//
        Configuration myConfig = WebConfigurationManager.OpenWebConfiguration("/is");
        ConnectionStringSettings connString;
        connString = myConfig.ConnectionStrings.ConnectionStrings["kdch_sk"];
        my_con.ConnectionString = connString.ToString();

	}

    private string parseQuery(string query)
    {
        query = query.Replace('[', '`');
        query = query.Replace(']', '`');

        return query;
    }
    /// <summary>
    /// Funkcia update single table, single row nejde cez foreign keys funguje na transakcii
    /// </summary>
    /// <param name="table">Nazov tabulky kam to dat</param>
    /// <param name="data">Data format SortedList nazvy columns</param>
    /// <param name="id">1. WHERE [nejake_id]=idecko, alebo len idecko</param>
    /// <returns></returns>

    public SortedList mysql_update(string table, SortedList data, string id)
    {
        OdbcTransaction trans1 = null;
        my_con.Open();

        trans1 = my_con.BeginTransaction();

        OdbcCommand cmdtrans = new OdbcCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;

        //StringBuilder sb = sb.AppendFormat("UPDATE [{0}] SET {1} WHERE [id]={2}");

       
        string[] strArr = new String[data.Count];
        int i = 0;

        foreach (DictionaryEntry tmp in data)
        {
            //cols = cols + tmp.Key + ",";
            strArr[i] = "["+tmp.Key.ToString()+"] ='"+tmp.Value.ToString()+"'";
            i++;
           // parse_str = parse_str.Replace("'", "*");
        }
        string setStr = String.Join(",", strArr);
        
        StringBuilder sb =  new StringBuilder();

        if (id.IndexOf("WHERE") != -1)
        {
            sb.AppendFormat("UPDATE [{0}] SET {1} {2}", table, setStr, id);
        }
        else
        {
            sb.AppendFormat("UPDATE [{0}] SET {1} WHERE [id] = {2}", table, setStr, id);
        }

         //= sb.AppendFormat("UPDATE [{0}] SET {1} WHERE [id]={2}", table, setStr, id);
        string query = sb.ToString();
        query = parseQuery(query);
        //return query;

       // int id = 0;

        SortedList result = new SortedList();
        try
        {
            cmdtrans.CommandText = query;
            cmdtrans.ExecuteNonQuery();
            //cmdtrans.CommandText = "SELECT last_insert_id();";
            //id = Convert.ToInt32(cmdtrans.ExecuteScalar());
            trans1.Commit();
            result.Add("status", true);
            result.Add("last_id", id);
        }
        catch (Exception e)
        {
            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("last_id", 0);
            trans1.Rollback();

        } 

        return result;
    }


    public SortedList mysql_insert(string table, SortedList data)
    {
        SortedList result = new SortedList();
        OdbcTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        OdbcCommand cmdtrans = new OdbcCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;

        StringBuilder sb = new StringBuilder();

        string[] columns = new string[data.Count];
        string[] values = new string[data.Count];
        string[] col_val = new string[data.Count];


        int i = 0;
        foreach (DictionaryEntry row in data)
        {
            
            columns[i] = "["+row.Key.ToString()+"]";
            if (row.Value == null)
            {
                values[i] = "NULL";
            }
            else
            {
                values[i] = "'" + row.Value.ToString() + "'";
            }
            col_val[i] = "[" + row.Key + "] =  values([" + row.Key + "])";
            i++;
        }

        string t_cols = string.Join(",", columns);
        string t_values = string.Join(",", values);
        string col_val_str = string.Join(",", col_val);

        sb.AppendFormat("INSERT INTO [{0}] ({1}) VALUES ({2}) ON DUPLICATE KEY UPDATE {3};", table, t_cols, t_values, col_val_str);

        string query = this.parseQuery(sb.ToString());
        
        int id = 0;
        try
        {
            cmdtrans.CommandText = query;
            cmdtrans.ExecuteNonQuery();
            cmdtrans.CommandText = "SELECT last_insert_id();";
            id = Convert.ToInt32(cmdtrans.ExecuteScalar());
            trans1.Commit();
            result.Add("status", true);
            result.Add("last_id", id);
        }
        catch (Exception e)
        {
            result.Add("status",false);
            result.Add("msg",e.ToString());
            result.Add("last_id", 0);
            trans1.Rollback();
          
        } 
        my_con.Close();
        return result;

    }

    public SortedList execute(string query)
    {
        query = this.parseQuery(query);

        SortedList result = new SortedList();
        OdbcTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        OdbcCommand cmdtrans = new OdbcCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;
        try
        {
            cmdtrans.CommandText = query;
            cmdtrans.ExecuteNonQuery();
            trans1.Commit();
            result.Add("status", true);
        }
        catch (Exception e)
        {
            trans1.Rollback();
            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("query", query);
        }
        my_con.Close();

        return result;

    }
    public SortedList getRow(string query)
    {
        SortedList result = new SortedList();
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();
        int row = 0;

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
              
            }
        }
        my_con.Close();


        return result;
    }


    public Dictionary<int, SortedList> getTable(string query)
    {

        Dictionary<int, SortedList> result = new Dictionary<int, SortedList>();

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();
        int row = 0;

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                SortedList tmp = new SortedList();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    tmp.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
                result.Add(row, tmp);
                row++;
            }
        }
        my_con.Close();


        return result;

    }

}
