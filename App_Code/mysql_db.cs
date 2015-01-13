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
        my_con.Close();
        return result;
    }

    public SortedList getLfData(int id)
    {
        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [file-name],[file-size],[file-type],[file-content] FROM [is_data] WHERE [id]='{0}'", id);
        my_con.Open();
        OdbcCommand my_com = new OdbcCommand(this.parseQuery(sb.ToString()), my_con);
        OdbcDataReader reader = my_com.ExecuteReader();

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
        my_con.Close();
        return result;

    }

    public byte[] getLfContent(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [file-content] FROM [is_data] WHERE [id]='{0}'", id);
        my_con.Open();
        OdbcCommand my_com = new OdbcCommand(this.parseQuery(sb.ToString()), my_con);
       
        byte[] result = (byte[])my_com.ExecuteScalar();
        my_con.Close();
        return result;

    }

    public SortedList callStoredProcWithoutParam(string stored_proc)
    {
        SortedList result = new SortedList();

        OdbcCommand cmd = new OdbcCommand();
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
            result.Add("status", false);
            result.Add("msg", e.ToString());
        }

        my_con.Close();

        return result;
    }

    public int fillDocShifts(int dategroup, int days, int mesiac, int rok)
    {
       
        int result = 0;
        //OdbcTransaction trans1 = null;
        //my_con.Open();
        //trans1 = my_con.BeginTransaction();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "{CALL FILL_DOC_SHIFTS(?,?,?,?,@res)}";

        OdbcParameter vstup = new OdbcParameter();
        vstup.ParameterName = "dateGroup";
        vstup.Direction = ParameterDirection.Input;
        vstup.OdbcType = OdbcType.Int;
        vstup.Value = dategroup;
        cmd.Parameters.Add(vstup);

        OdbcParameter vstup1 = new OdbcParameter();
        vstup1.ParameterName = "days";
        vstup1.Direction = ParameterDirection.Input;
        vstup1.OdbcType = OdbcType.Int;
        vstup1.Value = days;
        cmd.Parameters.Add(vstup1);

        OdbcParameter vstup2 = new OdbcParameter();
        vstup2.ParameterName = "mesiac";
        vstup2.Direction = ParameterDirection.Input;
        vstup2.OdbcType = OdbcType.Int;
        vstup2.Value = mesiac;
        cmd.Parameters.Add(vstup2);

        OdbcParameter vstup3 = new OdbcParameter();
        vstup3.ParameterName = "rok";
        vstup3.Direction = ParameterDirection.Input;
        vstup3.OdbcType = OdbcType.Int;
        vstup3.Value = rok;
        cmd.Parameters.Add(vstup3);

        OdbcParameter vstup4 = new OdbcParameter();
        vstup4.ParameterName = "res";
        vstup4.Direction = ParameterDirection.Output;
        vstup4.OdbcType = OdbcType.Int;
        //vstup3.Value = 2015;
        cmd.Parameters.Add(vstup4);
        my_con.Open();
        result = (int)cmd.ExecuteNonQuery();

        my_con.Close();

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
            
            columns[i] = "`"+row.Key.ToString()+"`";
            if (row.Value == null)
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
            result.Add("sql", query);
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
        //try
        //{
            OdbcDataReader reader = my_com.ExecuteReader();


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
           
        //}
        //catch (Exception e)
        //{
        //    result.Add("status", false);
        //    result.Add("msg", e.ToString());
        //}
        my_con.Close();


        return result;
    }


    public Dictionary<int, Hashtable> getTable(string query)
    {

        Dictionary<int, Hashtable> result = new Dictionary<int, Hashtable>();

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();
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

                    tmp.Add(reader.GetName(i).ToString(), reader.GetString(i));
                }
                result.Add(row, tmp);
                row++;
            }
        }
        my_con.Close();


        return result;

    }

    public Dictionary<int, SortedList> getTableSL(string query)
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


                    //string inData = reader.GetValue(i).ToString();
                    // byte[] buffer = Encoding.UTF8.GetBytes(inData);
                    //string outData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                    tmp.Add(reader.GetName(i).ToString(), reader.GetString(i));
                }
                result.Add(row, tmp);
                row++;
            }
        }
        my_con.Close();


        return result;

    }


}
