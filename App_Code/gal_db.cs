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
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for mysql_db
/// </summary>
public class gal_db
{
    public log x2log = new log();
    x2_var x2 = new x2_var();
   // public OdbcConnection my_con = new OdbcConnection();
    public MySql.Data.MySqlClient.MySqlConnection my_con = new MySql.Data.MySqlClient.MySqlConnection();
    //public log x2log = new log();  

    public gal_db()
	{
		//
		// TODO: Add constructor logic here
		//
        //Configuration myConfig = WebConfigurationManager.OpenWebConfiguration("/is");
        //ConnectionStringSettings connString;

        MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        conn_string.Server = "10.10.2.83";
        conn_string.Port = 3306;
        conn_string.UserID = "galuser";
        conn_string.Password = "st161009";
        conn_string.Database = "gallery3";


        // connString = myConfig.ConnectionStrings.ConnectionStrings["gallery3"];
        my_con.ConnectionString = conn_string.ToString();

	}

    public Boolean offline()
    {
        SortedList res = this.getRow("SELECT [name],[data] FROM [is_settings] WHERE [name]='webstatus'");
        Boolean status = Convert.ToBoolean(res["data"]);
        return status;
    }


    public string parseQuery(string query)
    {
        string pattern = @"\[([a-zA-Z_0-9-]+)\.([a-zA-Z_0-9]+)\]";
        Regex reg = new Regex(pattern);

        query = reg.Replace(query, @"`$1`.`$2`");

        pattern = @"\[([a-zA-Z_0-9-]+)\]";
        reg = new Regex(pattern);
        query = reg.Replace(query, @"`$1`");

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
        MySqlTransaction trans1 = null;

        my_con.Open();

        trans1 = my_con.BeginTransaction();

        // MySqlCommand cmdtrans = new MySqlCommand();
        MySqlCommand cmdtrans = new MySqlCommand();

        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;

        //StringBuilder sb = sb.AppendFormat("UPDATE [{0}] SET {1} WHERE [id]={2}");

       
        string[] strArr = new String[data.Count];
        int i = 0;

        foreach (DictionaryEntry tmp in data)
        {
            //cols = cols + tmp.Key + ",";
            if (tmp.Value.ToString() == "NULL" || tmp.Value.ToString().Trim().Length == 0)
            {
                strArr[i] = "[" + tmp.Key.ToString() + "]=NULL";
            }
            else
            {
                strArr[i] = "[" + tmp.Key.ToString() + "] ='" + tmp.Value.ToString().Trim() + "'";
            }
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
            x2log.logData(query, "", "mysql update");
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
            x2log.logData(query, e.ToString(), "error db wrong sql in mysql_update()");

            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("last_id", 0);
            result.Add("sql", query);
            trans1.Rollback();

        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }
        
        return result;
    }

    

    public SortedList getLfData2(int id)
    {
        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [file-name],[file-size],[file-type] FROM [is_data_2] WHERE [id]='{0}'", id);
        my_con.Open();
        MySqlCommand my_com = new MySqlCommand(this.parseQuery(sb.ToString()), my_con);
        MySqlDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetValue(i) == null)
                    {
                        result.Add(reader.GetName(i).ToString(), "0");
                    }
                    else
                    {
                        result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                    }
                }

            }
        }
        reader.Close();
        my_con.Close();
        return result;

    }

    public byte[] getLfContent(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [file-content] FROM [is_data] WHERE [id]='{0}'", id);
        my_con.Open();
        MySqlCommand my_com = new MySqlCommand(this.parseQuery(sb.ToString()), my_con);
       
        byte[] result = (byte[])my_com.ExecuteScalar();
        my_con.Close();
        return result;

    }

    public SortedList callStoredProcWithoutParam(string stored_proc)
    {
        SortedList result = new SortedList();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "{CALL "+stored_proc+"()}";

        my_con.Open();
        try
        {
            cmd.ExecuteNonQuery();
            result.Add("status", true);
        }
        catch (Exception e)
        {
            x2log.logData(cmd.CommandText.ToString(), e.ToString(), "error wrong sql in callStoredProcWithoutParam()");
            result.Add("status", false);
            result.Add("msg", e.ToString());
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
            
        }
       

        return result;
    }

    public string buildSql(string inQuery, string[] args)
    {
        string result = "";
        result = x2.sprintf(inQuery, args);
        result = parseQuery(result);
        return result;
    }

   

    public byte[] lfStoredData(int id, int size)
    {
        byte[] result = new byte[size];
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandText = "SELECT `file-content` FROM `is_data_2` WHERE `id` = ?";

        cmd.Parameters.Add("id", MySqlDbType.Int32).Value = id;

        my_con.Open();

        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                reader.GetBytes(0,0,result,0, size);
            }
        }
        reader.Close();
        my_con.Close();

        return result;
        
    }

    public SortedList lfUpdateData(byte[] data, SortedList lfData)
    {
       SortedList result = new SortedList();
        MySqlTransaction trans1 = null;
        my_con.Open();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
        cmd.CommandText = "UPDATE `is_data_2` SET `file-name`=@filename,`file-size`=@filesize,`file-type`=@filetype,`file-content`=@filecontent WHERE `id`=@id";

        cmd.Parameters.Add("filename", MySqlDbType.Text).Value = lfData["file-name"].ToString();
        cmd.Parameters.Add("filesize", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["file-size"]);
        cmd.Parameters.Add("filetype", MySqlDbType.VarChar).Value = lfData["file-type"].ToString();
        cmd.Parameters.Add("filecontent", MySqlDbType.Binary).Value = data;
        cmd.Parameters.Add("id", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["id"]);
        cmd.CommandText.ToString();
        try
        {
            cmd.ExecuteNonQuery();
            //cmd.CommandText = "SELECT LAST_INSERT_ID();";
            //int id = Convert.ToInt32(cmd.ExecuteScalar());
            result.Add("status", true);
            result.Add("last_id",  Convert.ToInt32(lfData["id"]));
            cmd.Transaction.Commit();
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error wrong sql in lfUpdateData()");
            result.Add("status",false);
            result.Add("msg",ex.ToString());
            cmd.Transaction.Rollback();
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }
        return result;
    }

    public SortedList lfInsertData(byte[] data, SortedList lfData)     
    {
        SortedList result = new SortedList();
        MySqlTransaction trans1 = null;
        my_con.Open();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
        cmd.CommandText = @"INSERT INTO `is_data_2`(`file-name`,`file-size`,`file-type`,`file-content`, `user_id`, `clinic_id`) 
                        VALUES (@filename,@filesize,@filetype,@filecontent,@userid,@clinicid)";

        cmd.Parameters.Add("filename", MySqlDbType.Text).Value = lfData["file-name"].ToString();
        cmd.Parameters.Add("filesize", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["file-size"]);
        cmd.Parameters.Add("filetype", MySqlDbType.VarChar).Value = lfData["file-type"].ToString();
        cmd.Parameters.Add("filecontent", MySqlDbType.Binary).Value = data;
        cmd.Parameters.Add("userid",MySqlDbType.Int32).Value=Convert.ToInt32(lfData["user_id"]);
        cmd.Parameters.Add("clinicid", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["clinic_id"]);
        cmd.CommandText.ToString();
        try
        {
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID();";
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            result.Add("status", true);
            result.Add("last_id", id);
            cmd.Transaction.Commit();
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error wrong sql in lfInsertData()");
            x2log.logData(lfData, "chyba vystup lf data", "error in lfinsert");
            result.Add("status",false);
            result.Add("msg",ex.ToString());
            cmd.Transaction.Rollback();
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }
        return result;
    }

    public SortedList mysql_insert_arr(string table, Dictionary<int,Hashtable> data)
    {
        SortedList result = new SortedList();

        int dataCnt = data.Count;

        string[] arr = new string[dataCnt];
        string[] columns = new string[data[0].Count];
        string[] col_vals = new string[data[0].Count];

        for (int i = 0; i < dataCnt; i++)
        {
            string[] _tmp = new string[data[i].Count];
            int j = 0;
            foreach (DictionaryEntry _row in data[i])
            {
                if (i == 0)
                {
                    columns[j] = "`" + _row.Key.ToString() + "`";
                    col_vals[j] = "`" + _row.Key.ToString() + "` =  values(`" + _row.Key.ToString() + "`)";
                }

                if (_row.Value == null)
                {
                    _tmp[j] = "NULL";
                }
                else if (_row.Value.ToString().Trim().Length == 0)
                {
                    _tmp[j] = "NULL";
                }
                else
                {
                    _tmp[j] = "'" + _row.Value.ToString() + "'";
                }
                j++;
            }
            arr[i] = "("+string.Join(",",_tmp)+")";
        }

        string t_cols = string.Join(",", columns);
        string t_cols_vals = string.Join(",", arr);
        string t_vals = string.Join(",", col_vals);

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("INSERT INTO `{0}` ({1}) VALUES {2} ON DUPLICATE KEY UPDATE {3};", table, t_cols, t_cols_vals, t_vals);
        string query = sb.ToString();

        MySqlTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        MySqlCommand cmdtrans = new MySqlCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;

        try
        {
            x2log.logData(query, "", "mysql insert");
            cmdtrans.CommandText = query;
            cmdtrans.ExecuteNonQuery();
           // cmdtrans.CommandText = "SELECT last_insert_id();";
           // id = Convert.ToInt32(cmdtrans.ExecuteScalar());
            trans1.Commit();
            result.Add("status", true);
            //result.Add("last_id", id);
        }
        catch (Exception e)
        {
            x2log.logData(query, e.ToString(), "error wrong sql in mysql_insert_arr()");
            result.Add("status", false);
            result.Add("msg", e.ToString());
            //result.Add("last_id", 0);
            result.Add("sql", query);
            trans1.Rollback();

        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }



        return result;
    }

    

    protected void myEscape(string text)
    {
        //string pattern = @""
    }

    //TODO je nutne parsovanie pola pre insert dat do samostanej fnc...
 

    /// <summary>
    /// Vlozi novy riadok do db a vrati posledne ID, bez transakcie, avsak toto je v transakci nutne je trans lebo ked sa to robi v transakic
    /// </summary>
    /// <param name="table">string table name</param>
    /// <param name="data">SortedList data to insert</param>
    /// <param name="myCon">OdbcConnection v ktorej to je </param>
    /// <param name="trans">MySqlTransaction v ktorej to je </param>
    /// <returns>SortedList kyes: status (true/false), msg(error),last_id(on succes),sql(sql in error)</returns>

    public SortedList mysql_insert_nt(string table, SortedList data, ref MySqlCommand cmd)
    {
        SortedList result = new SortedList();
       // my_con.Open();

      //  MySqlCommand cmdtrans = new MySqlCommand();
        //cmdtrans.Connection = myCon;
        //cmdtrans.Transaction = trans1;

        StringBuilder sb = new StringBuilder();

        string[] columns = new string[data.Count];
        string[] values = new string[data.Count];
        string[] col_val = new string[data.Count];


        int i = 0;
        foreach (DictionaryEntry row in data)
        {

            columns[i] = "`" + row.Key.ToString() + "`";
            if (row.Value == null)
            {
                values[i] = "NULL";
            }
            else if (row.Value.ToString().Trim().Length == 0)
            {
                values[i] = "NULL";
            }
            else
            {
                values[i] = "'" + row.Value.ToString() + "'";
            }
            col_val[i] = "`" + row.Key + "` =  values(`" + row.Key + "`)";
            i++;
        }

        string t_cols = string.Join(",", columns);
        string t_values = string.Join(",", values);
        string col_val_str = string.Join(",", col_val);

        sb.AppendFormat("INSERT INTO `{0}` ({1}) VALUES ({2}) ON DUPLICATE KEY UPDATE {3};", table, t_cols, t_values, col_val_str);

        string query = sb.ToString();

        int id = 0;
        try
        {
            x2log.logData(query, "", "mysql insert_nt");
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT last_insert_id();";
            id = Convert.ToInt32(cmd.ExecuteScalar());
            result.Add("status", true);
            result.Add("last_id", id);
        }
        catch (Exception e)
        {
            x2log.logData(query, e.ToString(), "error wrong sql in mysql_insert_nt()");
            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("last_id", 0);
            result.Add("sql", query);

        }
        
        return result;

    }

    /// <summary>
    /// Vlozi novy riadok no db a vrati posledne ID
    /// </summary>
    /// <param name="table">string table name</param>
    /// <param name="data">SortedList data to insert</param>
    /// <returns>SortedList kyes: status (true/false), msg(error),last_id(on succes),sql(sql in error)</returns>

    public SortedList mysql_insert(string table, SortedList data)
    {
        SortedList result = new SortedList();
        MySqlTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        MySqlCommand cmdtrans = new MySqlCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;

        StringBuilder sb = new StringBuilder();

        string[] columns = new string[data.Count];
        string[] values = new string[data.Count];
        string[] col_val = new string[data.Count];


        int i = 0;
        foreach (DictionaryEntry row in data)
        {
            
            columns[i] = "`"+row.Key.ToString()+"`";
            if (row.Value == null)
            {
                values[i] = "NULL";
            }
            else if (row.Value.ToString().Trim().Length == 0)
            {
                values[i] = "NULL";
            }
            else
            {
                values[i] = "'" + row.Value.ToString() + "'";
            }
            col_val[i] = "`" + row.Key + "` =  values(`" + row.Key + "`)";
            i++;
        }

        string t_cols = string.Join(",", columns);
        string t_values = string.Join(",", values);
        string col_val_str = string.Join(",", col_val);

        sb.AppendFormat("INSERT INTO `{0}` ({1}) VALUES ({2}) ON DUPLICATE KEY UPDATE {3};", table, t_cols, t_values, col_val_str);

        string query = sb.ToString();
        
        int id = 0;
        try
        {
            x2log.logData(query, "", "mysql insert");
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
            x2log.logData(query, e.ToString(), "error wrong sql in mysql_insert()");
            result.Add("status",false);
            result.Add("msg",e.ToString());
            result.Add("last_id", 0);
            result.Add("sql", query);
            trans1.Rollback();
          
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }
        return result;

    }

    public SortedList executeArr(string[] queries)
    {
        SortedList result = new SortedList();
        MySqlTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();
        string loging = "";
        
        MySqlCommand cmdtrans = new MySqlCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;
        try
        {
            for (int q = 0; q < queries.Length; q++ )
            {
                loging += queries[q] + "<br>";
                loging += ".......................................<br>";
                //x2log.logData(queries[q], "", "mysql executeArr");
                cmdtrans.CommandText = queries[q];
                cmdtrans.ExecuteNonQuery();
            }
                
            trans1.Commit();
            result.Add("status", true);
            result.Add("loging", loging);
        }
        catch (Exception e)
        {
            loging += "ERROR:" + e.ToString() + "<br>";
            trans1.Rollback();
            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("log", loging);
            //result.Add("query", query);
            x2log.logData(result, e.ToString(), "error wrong sql in executeArr()");
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }

        return result;
    }
    
    /// <summary>
    /// Vykona sql nonquery napr.DELETE alebo UPDATE 
    /// Returns SortedList (status, msg, query)
    /// </summary>
    /// <param name="query">formatovany SQL retazec typu string</param>
    public SortedList execute(string query)
    {
        query = this.parseQuery(query);

        SortedList result = new SortedList();
        MySqlTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        MySqlCommand cmdtrans = new MySqlCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;
        try
        {
            x2log.logData(query, "", "mysql execute");
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
            x2log.logData(result, e.ToString(), "error wrong sql in execute()");
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }

        return result;

    }
    

   

    /// <summary>
    /// returns row of SQL request in current Connection and transaction as SortedList if error the result[status],result[msg],result[sql]
    /// </summary>
    /// <param name="query">SQL string</param>
   
  
    public SortedList getRowInCon(string query)
    {
        if (query.IndexOf("LIMIT 1") == -1)
        {
            query += " LIMIT 1";
        }
        SortedList result = new SortedList();

        my_con.Open();

        MySqlCommand icmd = new MySqlCommand(query,my_con);

        try
        {
            // MySqlCommand my_com = new MySqlCommand(this.parseQuery(query.ToString()), my_con, trans);
            //icmd.CommandText = query;


            x2log.logData(query, "", "mysql getrow");
            MySqlDataReader reader = icmd.ExecuteReader();
            // result.Add("status" , true);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetValue(i).ToString() == "NULL")
                        {
                            result.Add(reader.GetName(i).ToString(), "NULL");
                        }
                        else
                        {
                            string tf = reader.GetFieldType(i).ToString();
                            if (tf == "System.Byte[]")
                            {
                                byte[] dl = (byte[])reader.GetValue(i);
                                string rr = System.Text.Encoding.UTF8.GetString(dl);
                                result.Add(reader.GetName(i).ToString(), rr);
                            }
                            else
                            {
                                result.Add(reader.GetName(i).ToString(), reader.GetValue(i));
                               
                            }
                        }
                    }

                }
                reader.Close();
            }

        }
        catch (Exception e)
        {
            x2log.logData(this.parseQuery(query.ToString()), e.ToString(), "error wrong sql in getRow()");
            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("sql", this.parseQuery(query.ToString()));
        }
        finally
        {
            
            my_con.Close();
            my_con.Dispose();
        }
        //cmd.

        return result;
    }


    /// <summary>
    /// returns row of SQL request
    /// </summary>
    /// <param name="query"></param>
    /// <returns>
    /// SortedList if error the result[status],result[msg],result[sql]
    /// </returns>
    public SortedList getRow(string query)
    {
        if (query.IndexOf("LIMIT 1") == -1)
        {
            query += " LIMIT 1";
        }
        SortedList result = new SortedList();
        my_con.Open();

        
        try
        {
            MySqlCommand my_com = new MySqlCommand(this.parseQuery(query.ToString()), my_con);
            x2log.logData(this.parseQuery(query.ToString()),"","mysql getrow");
            MySqlDataReader reader = my_com.ExecuteReader();
           // result.Add("status", true);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            result.Add(reader.GetName(i).ToString(), "NULL");
                        }
                        else
                        {
                            string tf = reader.GetFieldType(i).ToString();
                            if (tf == "System.Byte[]")
                            {
                                byte[] dl = (byte[])reader.GetValue(i);
                                string rr = System.Text.Encoding.UTF8.GetString(dl);
                                result.Add(reader.GetName(i).ToString(), rr);
                            }
                            else
                            {
                                result.Add(reader.GetName(i).ToString(), reader.GetValue(i));
                            }
                        }
                    }

                }
            }

        }
        catch (Exception e)
        {
            x2log.logData(this.parseQuery(query.ToString()),e.ToString(),"error wrong sql in getRow()");
            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("sql", this.parseQuery(query.ToString()));
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }


        return result;
    }


    public Dictionary<int, Hashtable> getTable(string query)
    {

        Dictionary<int, Hashtable> result = new Dictionary<int, Hashtable>();

        //if (query.IndexOf(""))

        try
        {

            if (query.Trim().Length==0)
            {
                throw new System.Exception("NO VALID OR EMPTY SQL String...");
            }

            my_con.Open();
            MySqlCommand my_com = new MySqlCommand(this.parseQuery(query.ToString()), my_con);
            x2log.logData(this.parseQuery(query.ToString()),"","mysql getTable");
            MySqlDataReader reader = my_com.ExecuteReader();
            int row = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Hashtable tmp = new Hashtable();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {


                        //string inData = reader.GetValue(i).ToString();
                        // byte[] buffer = Encoding.UTF8.GetBytes(inData);
                        //string outData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            tmp.Add(reader.GetName(i).ToString(), "NULL");
                        }
                        else
                        {
                            string tf = reader.GetFieldType(i).ToString();

                            if (tf == "System.Byte[]")
                            {
                                byte[] dl = (byte[])reader.GetValue(i);
                                string rr = System.Text.Encoding.UTF8.GetString(dl);

                                // tmp.Add(reader.GetName(i).ToString(), reader.GetString(i).ToString());
                                tmp.Add(reader.GetName(i).ToString(), rr);
                            }
                            else
                            {
                                tmp.Add(reader.GetName(i).ToString(), reader.GetValue(i));
                            }
                        }
                    }
                    result.Add(row, tmp);
                    row++;
                }
            }
        }
        catch (Exception ex)
        {
            x2log.logData(this.parseQuery(query.ToString()), ex.ToString(), "error wrong sql in getTable()");
        }
        finally
        {
            
            my_con.Close();
            my_con.Dispose();
        }


        return result;

    }

    public SortedList getLabels(string clinic)
    {
        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [idf],[label] FROM [is_labels] WHERE [clinic]='{0}'", clinic);
        my_con.Open();
        string query = this.parseQuery(sb.ToString());
        MySqlCommand my_command = new MySqlCommand(query, my_con);
        try
        {
            MySqlDataReader reader = my_command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add(reader["idf"].ToString(), reader["label"].ToString());
                }
            }
            
        }
        catch (Exception e)
        {
            x2log.logData(query, e.ToString(), "getLabels error");
            result.Add("status", false);
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }
        return result;
    }

    public Dictionary<int, SortedList> getTableSL(string query)
    {

        Dictionary<int, SortedList> result = new Dictionary<int, SortedList>();

        my_con.Open();
        //x2log.logData(this.parseQuery(query.ToString()), "", "SQL from getTableSL");
        MySqlCommand my_com = new MySqlCommand(this.parseQuery(query.ToString()), my_con);

        MySqlDataReader reader = my_com.ExecuteReader();
        int row = 0;
        try
        {

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    SortedList tmp = new SortedList();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {


                        //string inData = reader.GetValue(i).ToString();
                        // byte[] buffer = Encoding.UTF8.GetBytes(inData);
                        //string outData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                        if (reader.GetValue(i) == DBNull.Value)
                        {
                            tmp.Add(reader.GetName(i).ToString(), "NULL");
                        }
                        else
                        {
                            string tf = reader.GetFieldType(i).ToString();

                            if (tf == "System.Byte[]")
                            {
                                byte[] dl = (byte[])reader.GetValue(i);
                                string rr = System.Text.Encoding.UTF8.GetString(dl);

                                // tmp.Add(reader.GetName(i).ToString(), reader.GetString(i).ToString());
                                tmp.Add(reader.GetName(i).ToString(), rr);
                            }
                            else
                            {
                                tmp.Add(reader.GetName(i).ToString(), reader.GetValue(i));
                            }
                        }


                        //  tmp.Add(reader.GetName(i).ToString(), reader.GetString(i));
                    }
                    result.Add(row, tmp);
                    row++;
                }
            }
        }
        catch (Exception ex)
        {
            x2log.logData(this.parseQuery(query.ToString()), ex.ToString(), "error wrong sql in getTableSL()");
        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }


        return result;

    }

    public Boolean mysql_insert_log(SortedList data)
    {
        Boolean result = true;
        

        StringBuilder sb = new StringBuilder();

        string[] columns = new string[data.Count];
        string[] values = new string[data.Count];
        string[] col_val = new string[data.Count];


        int i = 0;
        foreach (DictionaryEntry row in data)
        {

            columns[i] = "`" + row.Key.ToString() + "`";
            if (row.Value == null)
            {
                values[i] = "NULL";
            }
            else if (row.Value.ToString().Trim().Length == 0)
            {
                values[i] = "NULL";
            }
            else
            {
                values[i] = "'" + row.Value.ToString() + "'";
            }
            col_val[i] = "`" + row.Key + "` =  values(`" + row.Key + "`)";
            i++;
        }

        string t_cols = string.Join(",", columns);
        string t_values = string.Join(",", values);
        string col_val_str = string.Join(",", col_val);

        sb.AppendFormat("INSERT INTO `{0}` ({1}) VALUES ({2}) ON DUPLICATE KEY UPDATE {3};", "is_register_temp", t_cols, t_values, col_val_str);

        string query = sb.ToString();

        int id = 0;
        try
        {
            my_con.Open();
            MySqlCommand my_com = new MySqlCommand(this.parseQuery(query.ToString()), my_con);
            my_com.CommandText = query;
            my_com.ExecuteNonQuery();
            // cmdtrans.CommandText = "SELECT last_insert_id();";
            // id = Convert.ToInt32(cmdtrans.ExecuteScalar());
            
            // result.Add("last_id", id);
        }
        catch (Exception e)
        {
            result = false;
            //result.Add("msg", e.ToString());
            //result.Add("last_id", 0);
            //result.Add("sql", query);

        }
        finally
        {
            my_con.Close();
            my_con.Dispose();
        }
        return result;

    }

    public SortedList registerTempFile(string filename, Int32 days,string folder)
    {
        SortedList data = new SortedList();

        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        Int32 tolivetime = unixTimestamp + days * 24 * 60 * 60;

        data.Add("file_name", filename);
        data.Add("time_in", unixTimestamp);
        data.Add("time_out", tolivetime);
        data.Add("folder", folder);

        SortedList result = this.mysql_insert("is_register_temp", data);

        return result;
    }


    /// <summary>
    /// Klasicky mysql insert Vlozi riadok, a vrati status true a posledne IDecko, inak vrati chybove hlasenie
    /// </summary>
    /// <param name="table"></param>
    /// <param name="data"></param>
    /// <returns></returns>

    public SortedList insert_row_old(string table, SortedList data)
    {
        string cols = "(";
        string values = "('";
        string parse_str;
        SortedList result = new SortedList();
        foreach (DictionaryEntry tmp in data)
        {
            cols = cols + tmp.Key + ",";
            parse_str = tmp.Value.ToString();
            values = values + parse_str.Replace("'", "*") + "','";
        }
        cols = cols.Substring(0, cols.Length - 1);
        values = values.Substring(0, values.Length - 2);
        cols = cols + ")";
        values = values + ")";

        string query = "INSERT INTO `" + table + "` " + cols + " VALUES " + values;

        //return query;
        MySqlCommand cmd = new MySqlCommand();
        try
        {
            my_con.Open();

           // MySqlCommand my_insert = new MySqlCommand(query, my_con);

            

           

            cmd.Connection = my_con;
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            int id = Convert.ToInt32(cmd.ExecuteScalar());

            result.Add("status", true);
            result.Add("last_id", id);
            //return true;
            my_con.Close();
            cmd.Dispose();
        }
        catch (Exception e)
        {
            my_con.Close();
            cmd.Dispose();
            result.Add("status", false);
            string msg = e.ToString();
            
            if (msg.IndexOf("Duplicate entry") != -1)
            {
                msg = "DE";
            }

            result.Add("msg", msg);
            
        }

        return result;

    }

}
