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
    public log x2log = new log();
    x2_var x2 = new x2_var();
    public OdbcConnection my_con = new OdbcConnection();
    //public log x2log = new log();  

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
            if (tmp.Value.ToString() == "NULL" || tmp.Value.ToString().Trim().Length == 0)
            {
                strArr[i] = "[" + tmp.Key.ToString() + "]=NULL";
            }
            else
            {
                strArr[i] = "[" + tmp.Key.ToString() + "] ='" + tmp.Value.ToString() + "'";
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
        my_con.Close();
        return result;
    }

    public string[] getFreeDays()
    {
        string query = "SELECT * FROM [is_settings] WHERE [name]='free_days'";
        SortedList data = this.getRow(this.parseQuery(query));

        string[] result = data["data"].ToString().Split(',');

        return result;
    }

    public SortedList getLfData2(int id)
    {
        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [file-name],[file-size],[file-type] FROM [is_data_2] WHERE [id]='{0}'", id);
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
            x2log.logData(cmd.CommandText.ToString(), e.ToString(), "error wrong sql in callStoredProcWithoutParam()");
            result.Add("status", false);
            result.Add("msg", e.ToString());
        }

        my_con.Close();

        return result;
    }

    public string buildSql(string inQuery, string[] args)
    {
        string result = "";
        result = x2.sprintf(inQuery, args);
        result = parseQuery(result);
        return result;
    }

    public SortedList fillNurseShifts(int dategroup, int days, int mesiac, int rok, string odd)
    {

        SortedList result = new SortedList();
        //OdbcTransaction trans1 = null;
        //my_con.Open();
        //trans1 = my_con.BeginTransaction();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "{CALL FILL_NURSE_SHIFTS(?,?,?,?,?,@res)}";

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
        vstup4.ParameterName = "odd";
        vstup4.Direction = ParameterDirection.Input;
        vstup4.OdbcType = OdbcType.VarChar;
        vstup4.Value = odd;
        cmd.Parameters.Add(vstup4);


        OdbcParameter vstup5 = new OdbcParameter();
        vstup5.ParameterName = "res";
        vstup5.Direction = ParameterDirection.Output;
        vstup5.OdbcType = OdbcType.Int;
        //vstup3.Value = 2015;
        cmd.Parameters.Add(vstup5);
        my_con.Open();
        try
        {
            int res = (int)cmd.ExecuteNonQuery();
            result.Add("status",true);
            result.Add("res",res);
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error wrong sql in fillNurseShifts()");

            result.Add("status",false);
            result.Add("msg", ex.ToString());
        }

        my_con.Close();

        return result;
    }

    public SortedList calcNightWork(int user_id, int mesiac, int rok, int pocetDni)
    {
        SortedList result = new SortedList();

        OdbcTransaction trans1 = null;
        my_con.Open();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
        cmd.CommandText = "CALL CALC_NOCNA_PRACA(?,?,?,?);";

        cmd.Parameters.Add("userID", OdbcType.BigInt).Value = user_id;
        cmd.Parameters.Add("mesiac", OdbcType.Int).Value = mesiac;
        cmd.Parameters.Add("rok", OdbcType.Int).Value = rok;
        cmd.Parameters.Add("pocetDni", OdbcType.Int).Value = pocetDni;
        //cmd.Parameters.Add("id", OdbcType.BigInt).Value = Convert.ToInt32(lfData["id"]);
        cmd.CommandText.ToString();
        try
        {
            cmd.ExecuteNonQuery();
            //cmd.CommandText = "SELECT LAST_INSERT_ID();";
            //int id = Convert.ToInt32(cmd.ExecuteScalar());
            result.Add("status", true);
            //result.Add("last_id",  Convert.ToInt32(lfData["id"]));
            cmd.Transaction.Commit();
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error wrong sql in calcNightWork ");

            result.Add("status",false);
            result.Add("msg",ex.ToString());
            cmd.Transaction.Rollback();
        }
        my_con.Close();



        return result;
    }

    public int fillNkimShifts(int dategroup, int days, int mesiac, int rok, int clinic)
    {

        int result = 0;
        //OdbcTransaction trans1 = null;
        //my_con.Open();
        //trans1 = my_con.BeginTransaction();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "{CALL FILL_NKIM_SHIFTS(?,?,?,?,?,@res)}";

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
        vstup4.ParameterName = "clinic";
        vstup4.Direction = ParameterDirection.Input;
        vstup4.OdbcType = OdbcType.Int;
        vstup4.Value = clinic;
        cmd.Parameters.Add(vstup4);

        OdbcParameter vstup5 = new OdbcParameter();
        vstup5.ParameterName = "res";
        vstup5.Direction = ParameterDirection.Output;
        vstup5.OdbcType = OdbcType.Int;
        //vstup3.Value = 2015;
        cmd.Parameters.Add(vstup5);
        my_con.Open();
        try
        {
            result = (int)cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error fill nkim shifts");
        }


        my_con.Close();

        return result;
    }

    public int fillKdhaoShifts(int dategroup, int days, int mesiac, int rok, int clinic)
    {

        int result = 0;
        //OdbcTransaction trans1 = null;
        //my_con.Open();
        //trans1 = my_con.BeginTransaction();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "{CALL FILL_NKIM_SHIFTS(?,?,?,?,?,@res)}";

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
        vstup4.ParameterName = "clinic";
        vstup4.Direction = ParameterDirection.Input;
        vstup4.OdbcType = OdbcType.Int;
        vstup4.Value = clinic;
        cmd.Parameters.Add(vstup4);

        OdbcParameter vstup5 = new OdbcParameter();
        vstup5.ParameterName = "res";
        vstup5.Direction = ParameterDirection.Output;
        vstup5.OdbcType = OdbcType.Int;
        //vstup3.Value = 2015;
        cmd.Parameters.Add(vstup5);
        my_con.Open();
        try
        {
            result = (int)cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error fill kdhao shifts");
        }


        my_con.Close();

        return result;
    }


    public int fillDocShifts(int dategroup, int days, int mesiac, int rok,int clinic)
    {
       
        int result = 0;
        //OdbcTransaction trans1 = null;
        //my_con.Open();
        //trans1 = my_con.BeginTransaction();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "{CALL FILL_DOC_SHIFTS(?,?,?,?,?,@res)}";

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
        vstup4.ParameterName = "clinic";
        vstup4.Direction = ParameterDirection.Input;
        vstup4.OdbcType = OdbcType.Int;
        vstup4.Value = clinic;
        cmd.Parameters.Add(vstup4);

        OdbcParameter vstup5 = new OdbcParameter();
        vstup5.ParameterName = "res";
        vstup5.Direction = ParameterDirection.Output;
        vstup5.OdbcType = OdbcType.Int;
        //vstup3.Value = 2015;
        cmd.Parameters.Add(vstup5);
        my_con.Open();
        try
        {
            result = (int)cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.ToString(), ex.ToString(), "error fill doc shifts");
        }
        

        my_con.Close();

        return result;
    }

    public byte[] lfStoredData(int id, int size)
    {
        byte[] result = new byte[size];
        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandText = "SELECT `file-content` FROM `is_data_2` WHERE `id` = ?";

        cmd.Parameters.Add("id", OdbcType.BigInt).Value = id;

        my_con.Open();

        OdbcDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                reader.GetBytes(0,0,result,0, size);
            }
        }

        my_con.Close();

        return result;
        
    }

    public SortedList lfUpdateData(byte[] data, SortedList lfData)
    {
       SortedList result = new SortedList();
        OdbcTransaction trans1 = null;
        my_con.Open();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
        cmd.CommandText = "UPDATE `is_data_2` SET `file-name`=?,`file-size`=?,`file-type`=?,`file-content`=? WHERE `id`=?";

        cmd.Parameters.Add("filename", OdbcType.Text).Value = lfData["file-name"].ToString();
        cmd.Parameters.Add("filesize", OdbcType.BigInt).Value = Convert.ToInt32(lfData["file-size"]);
        cmd.Parameters.Add("filetype", OdbcType.VarChar).Value = lfData["file-type"].ToString();
        cmd.Parameters.Add("filecontent", OdbcType.Binary).Value = data;
        cmd.Parameters.Add("id", OdbcType.BigInt).Value = Convert.ToInt32(lfData["id"]);
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
        my_con.Close();
        return result;
    }

    public SortedList lfInsertData(byte[] data, SortedList lfData)     
    {
        SortedList result = new SortedList();
        OdbcTransaction trans1 = null;
        my_con.Open();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
        cmd.CommandText = "INSERT INTO `is_data_2`(`file-name`,`file-size`,`file-type`,`file-content`, `user_id`, `clinic_id`) VALUES (?,?,?,?,?,?)";

        cmd.Parameters.Add("filename", OdbcType.Text).Value = lfData["file-name"].ToString();
        cmd.Parameters.Add("filesize", OdbcType.BigInt).Value = Convert.ToInt32(lfData["file-size"]);
        cmd.Parameters.Add("filetype", OdbcType.VarChar).Value = lfData["file-type"].ToString();
        cmd.Parameters.Add("filecontent", OdbcType.Binary).Value = data;
        cmd.Parameters.Add("userid",OdbcType.BigInt).Value=Convert.ToInt32(lfData["user_id"]);
        cmd.Parameters.Add("clinicid", OdbcType.BigInt).Value = Convert.ToInt32(lfData["clinic_id"]);
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
        my_con.Close();
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

        OdbcTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        OdbcCommand cmdtrans = new OdbcCommand();
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
        my_con.Close();



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
    /// <param name="trans">OdbcTransaction v ktorej to je </param>
    /// <returns>SortedList kyes: status (true/false), msg(error),last_id(on succes),sql(sql in error)</returns>

    public SortedList mysql_insert_nt(string table, SortedList data, OdbcConnection myCon, OdbcTransaction trans1)
    {
        SortedList result = new SortedList();
       // my_con.Open();

        OdbcCommand cmdtrans = new OdbcCommand();
        cmdtrans.Connection = myCon;
        cmdtrans.Transaction = trans1;

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
            cmdtrans.CommandText = query;
            cmdtrans.ExecuteNonQuery();
            cmdtrans.CommandText = "SELECT last_insert_id();";
            id = Convert.ToInt32(cmdtrans.ExecuteScalar());
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
        //my_con.Close();
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
        my_con.Close();
        return result;

    }

    public SortedList executeArr(string[] queries)
    {
        SortedList result = new SortedList();
        OdbcTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();
        string loging = "";
        
        OdbcCommand cmdtrans = new OdbcCommand();
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
        my_con.Close();

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
        OdbcTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        OdbcCommand cmdtrans = new OdbcCommand();
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
        my_con.Close();

        return result;

    }
    

    public int fillDKShifts(int dategroup, int rok, int mesiac, int days, int clinic)
    {
        int result = 0;
        //OdbcTransaction trans1 = null;
        //my_con.Open();
        //trans1 = my_con.BeginTransaction();

        OdbcCommand cmd = new OdbcCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "{CALL FILL_DK_SHIFTS(?,?,?,?,?,@res)}";

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
        vstup4.ParameterName = "clinic";
        vstup4.Direction = ParameterDirection.Input;
        vstup4.OdbcType = OdbcType.Int;
        vstup4.Value = clinic;
        cmd.Parameters.Add(vstup4);

        OdbcParameter vstup5 = new OdbcParameter();
        vstup5.ParameterName = "res";
        vstup5.Direction = ParameterDirection.Output;
        vstup5.OdbcType = OdbcType.Int;
        //vstup3.Value = 2015;
        cmd.Parameters.Add(vstup5);

        try
        {
            my_con.Open();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT @res AS res ";

            object value = cmd.ExecuteScalar();
            int resTmp;

            if (Int32.TryParse(value.ToString(),out resTmp) )
            {
                result = resTmp;
            }

            x2log.logData(value.ToString(), "", "fill_dk_result");
        }
        catch (Exception e)
        {
            x2log.logData(cmd.CommandText.ToString(), e.ToString(), "error in dk shifts");
        }
        my_con.Close();

        return result;
    }

    /// <summary>
    /// returns row of SQL request in current Connection and transaction as SortedList if error the result[status],result[msg],result[sql]
    /// </summary>
    /// <param name="query">SQL Format string</param>
    /// <param name="my_con">Current ODBC Connection</param>
    /// <param name="trans">Current ODBC transaction</param>
  
    public SortedList getRowInCon(string query, OdbcConnection my_con, OdbcTransaction trans)
    {
        if (query.IndexOf("LIMIT 1") == -1)
        {
            query += " LIMIT 1";
        }
        SortedList result = new SortedList();
        //my_con.Open();


        try
        {
            OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con, trans);
            x2log.logData(this.parseQuery(query.ToString()), "", "mysql getrow");
            OdbcDataReader reader = my_com.ExecuteReader();
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
            x2log.logData(this.parseQuery(query.ToString()), e.ToString(), "error wrong sql in getRow()");
            result.Add("status", false);
            result.Add("msg", e.ToString());
            result.Add("sql", this.parseQuery(query.ToString()));
        }
        //my_con.Close();


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
            OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con);
            x2log.logData(this.parseQuery(query.ToString()),"","mysql getrow");
            OdbcDataReader reader = my_com.ExecuteReader();
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
        my_con.Close();


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
            OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con);
            x2log.logData(this.parseQuery(query.ToString()),"","mysql getTable");
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
        my_con.Close();


        return result;

    }

    public SortedList getLabels(string clinic)
    {
        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [idf],[label] FROM [is_labels] WHERE [clinic]='{0}'", clinic);
        my_con.Open();
        string query = this.parseQuery(sb.ToString());
        OdbcCommand my_command = new OdbcCommand(query, my_con);
        try
        {
            OdbcDataReader reader = my_command.ExecuteReader();

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
        return result;
    }

    public Dictionary<int, SortedList> getTableSL(string query)
    {

        Dictionary<int, SortedList> result = new Dictionary<int, SortedList>();

        my_con.Open();
        //x2log.logData(this.parseQuery(query.ToString()), "", "SQL from getTableSL");
        OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();
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
        my_con.Close();


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
            OdbcCommand my_com = new OdbcCommand(this.parseQuery(query.ToString()), my_con);
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
        my_con.Close();
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

}
