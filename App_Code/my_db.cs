﻿using System;

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
/// Summary description for my_db
/// </summary>
public class my_db
{
    public  OdbcConnection my_con = new OdbcConnection();

	public my_db()
	{
		//
		// TODO: Add constructor logic here
		//
     
        Configuration myConfig = WebConfigurationManager.OpenWebConfiguration("/is");
        ConnectionStringSettings connString;
        connString = myConfig.ConnectionStrings.ConnectionStrings["kdch_sk"];

        my_con.ConnectionString = connString.ToString();
    
    }

    public DataSet getData_opvykon(string druh)
    {
        string query = "SELECT * FROM is_opvykon WHERE Oddelenie='" + druh + "'";
        string conn = my_con.ConnectionString.ToString();
        my_con.Open();
        OdbcDataAdapter da = new OdbcDataAdapter(query, conn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }
    public DataSet getData_operacie()
    {
        string query = "SELECT * FROM is_opkniha ORDER BY DESC";
        string conn = my_con.ConnectionString.ToString();
        my_con.Open();
        OdbcDataAdapter da = new OdbcDataAdapter(query, conn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }

    public SortedList getAllUsers(string table,string skupina)
    {
        SortedList result = new SortedList();
        StringBuilder query = new StringBuilder();
        query.AppendFormat("SELECT id, full_name,`group` FROM `{0}` WHERE `group` = '{1}' OR `group`= 'poweruser' ORDER BY full_name DESC",table,skupina);
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result.Add(reader[0].ToString(), reader[1].ToString());
            }
        }
        my_con.Close();

        return result;




    }

    public SortedList getUserInfoByID(string table, string id)
    {
        SortedList result = new SortedList();

        string query = "SELECT * FROM " + table + " WHERE id=" + id;
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            result.Add("result", "none");
            my_con.Close();
        }



        return result;
    }


    public Dictionary<int, SortedList> getTable(string query)
    {

        Dictionary<int, SortedList> result = new Dictionary<int, SortedList>();

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query.ToString(), my_con);

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

         

    public SortedList insertHoliday(string table, SortedList data)
    {
        SortedList result = new SortedList();
        
        SortedList tmp = this.insert_rows(table, data);

        if (tmp["status"].ToString() == "ok")
        {
            result.Add("status", "ok");
            result.Add("last_id", tmp["last_id"].ToString());
        }
        if (tmp["status"].ToString() == "error")
        {
            result.Add("status", "error");
            result.Add("message", tmp["message"].ToString());
        }


        return result;
    }



    public SortedList lastInsertID(string table)
    {
        //string query = "SELECT LAST_INSERT_ID() FROM `{0}` LIMIT 1;";
        string query = "SELECT MAX(id) FROM `{0}`";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(query, table);

        //my_con.Open();
        
        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);       

        OdbcDataReader reader = my_com.ExecuteReader();
        SortedList result = new SortedList();

        while (reader.Read())
        {

            for (int i = 0; i < reader.FieldCount; i++)
            {
                result.Add("last_id", reader.GetValue(i).ToString());
            }
        }
        
        
      // my_con.Close();

        return result;

    }



    public SortedList getUserPasswd(string name)
    {
        SortedList result = new SortedList();
        
        string query = "SELECT * FROM is_users WHERE name='"+name+"'";
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);
        

        OdbcDataReader reader = my_com.ExecuteReader();
       
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
    /// <summary>
    /// vlozi riadky, a vrati status=ok a posledne IDecko, inak vrati chybove hlasenie, vrati status=ok  alebo status=error a polozke message je error
    /// </summary>
    /// <param name="table"></param>
    /// <param name="data"></param>
    /// <returns></returns>

    public SortedList insert_rows(string table, SortedList data)
    {
        string cols = "(";
        string values = "('";
        string parse_str;
        SortedList result = new SortedList();
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

        string query = "INSERT INTO `"+table+"` " + cols + " VALUES " + values;
       
        //return query;
        try
        {
            my_con.Open();

            OdbcCommand my_insert = new OdbcCommand(query, my_con);

            my_insert.ExecuteNonQuery();
          

            

           

            SortedList my_res = this.lastInsertID(table);

            my_con.Close();

            result.Add("status", "ok");
            result.Add("last_id", my_res["last_id"]);
            //return true;
            return result;
        }
         catch (Exception e)
       {
           my_con.Close();
           result.Clear();
           result.Add("status", "error");
           result.Add("message", e.ToString()+"...."+query);
           return result;
       }
   

    }

    
    /// <summary>
    /// updatuje riadky v db ak je id len cislo, plati klauzula id=cislo, ak sa to id vlozi WHERE nejake_id = id, tak sa pouzije tento zapi id...
    /// </summary>
    /// <param name="table"></param>
    /// <param name="data"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string update_row(string table, SortedList data, string id)
    {
        string my_set = "SET ";
        string parse_str;
        foreach (DictionaryEntry tmp in data)
        {
            //cols = cols + tmp.Key + ",";
            parse_str = tmp.Value.ToString();
            parse_str = parse_str.Replace("'", "*");
            my_set += tmp.Key.ToString() + "='" + parse_str + "', ";
        }
        my_set = my_set.Substring(0, my_set.Length - 2);
        //values = values.Substring(0, values.Length - 2);
        //cols = cols + ")";
        //values = values + ")";
        string query = "";
        if (id.IndexOf("WHERE") != -1)
        {
            query = "UPDATE " + table + " " + my_set + " " + id;
        }
        else
        {
            query = "UPDATE " + table + " " + my_set + " WHERE id=" + id;
        }

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
            my_con.Close();
            return e.ToString();
        }

        //return query;
    }


    public string delete_row(string table, int id)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("DELETE FROM {0} WHERE id={1}", table, id);
        string query = sb.ToString();

        try
        {
            my_con.Open();
            OdbcCommand sqlDelete = new OdbcCommand(query, my_con);

            sqlDelete.ExecuteNonQuery();
            my_con.Close();
            return "ok";
        }
        catch (Exception e)
        {
            my_con.Close();
            return e.ToString();
        }


    }


    public SortedList getDataByID(string table, string id)
    {
        SortedList result = new SortedList();
        string query = "SELECT * FROM " + table + " WHERE id=" + id;
        my_con.Open();
        OdbcCommand my_com = new OdbcCommand(query, my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

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
        else
        {
            result.Add("empty", "true");
        }
        my_con.Close();

        return result;
        
    }

    public SortedList getSestrHlasko(DateTime dat_hlas, string oddelenie, string lokalita, string hlas_text, string user_id, string cas)
    {
        SortedList result = new SortedList();
        string mesiac = dat_hlas.Month.ToString();
        string den = dat_hlas.Day.ToString();
        string rok = dat_hlas.Year.ToString();
        string unix_date = rok + "-" + mesiac + "-" + den;

        string query = "SELECT * FROM `is_hlasko_sestry` WHERE `dat_hlas` = '" + unix_date + "' AND `oddelenie` ='" + oddelenie + "'" + " AND `lokalita` = '" + lokalita + "' AND `cas`='" + cas + "'";

        string query1 = "SELECT * FROM `is_hlasko_sestry` WHERE (`dat_hlas` = '{0}' AND `oddelenie` ='{1}') AND (`lokalita` = '{2}' AND `cas`='{3}')";
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat(query1, unix_date, oddelenie, lokalita, cas);

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

        string lbl_hlaskoSestr = Resources.Resource.odd_hlasko_sestry.ToString();

        if (reader.HasRows)
        {
            result.Add("update", "1");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
            SortedList data = new SortedList();
            data.Add("oddelenie", oddelenie);
            data.Add("lokalita", lokalita);
            data.Add("dat_hlas", unix_date);
            data.Add("hlasko", lbl_hlaskoSestr);
           
            data.Add("creat_user", user_id);
            data.Add("last_user", user_id);
            data.Add("uzavri", "0");
            data.Add("cas", cas);


            SortedList res = this.insert_rows("is_hlasko_sestry", data);
            my_con.Close();
            if (res["status"] == "ok")
            {
                result.Add("new_ins", "true");
                result.Add("hlasko", lbl_hlaskoSestr);
                result.Add("akt_hlasenie", res["last_id"]);
                result.Add("last_user", user_id);
                result.Add("uzavri", "0");
                result.Add("cas", cas);
            }
            else
            {
                result.Add("error", res["message"]);
            }
        }
        return result;
    }



    public SortedList getHlasko(DateTime dat_hlas, string hlas_type, string hlas_text, string user_id)
    {
        SortedList result = new SortedList();
        string mesiac = dat_hlas.Month.ToString();
        string den = dat_hlas.Day.ToString();
        string rok = dat_hlas.Year.ToString();
        string unix_date = mesiac + "/" + den + "/" + rok;

        string query = "SELECT * FROM is_hlasko WHERE dat_hlas = '" + unix_date+"' AND type ='"+hlas_type+"'";
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);


        OdbcDataReader reader = my_com.ExecuteReader();


        string lbl_hlasko = Resources.Resource.odd_hlasko_html.ToString();
        if (reader.HasRows)
        {
            result.Add("update", "1");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
            SortedList data = new SortedList();
            data.Add("type", hlas_type);
            data.Add("dat_hlas", unix_date);
            data.Add("text", lbl_hlasko);
            data.Add("date", dat_hlas.ToString());
            data.Add("creat_user", user_id);
            data.Add("last_user", user_id);
            data.Add("uzavri", "0");
            data.Add("osirix", "");

            SortedList res = this.insert_rows("is_hlasko", data);
            my_con.Close();
            if (res["status"] == "ok")
            {
                result.Add("new_ins", "true");
                result.Add("text", lbl_hlasko);
                result.Add("akt_hlasenie", res["last_id"]);
                result.Add("last_user", user_id);
                result.Add("uzavri", "0");
                result.Add("osirix", "");
            }
            else
            {
                result.Add("error", res["message"]);
            }
        }
        return result;
    }

    public SortedList insertSluzby(string table, SortedList data)
    {
        SortedList result = this.insert_rows(table, data);

        return result;
    }

    public string updateSluzby(string table, SortedList data, string id)
    {
        string result = this.update_row(table, data, id);

        return result;
    }
    /// <summary>
    /// Funkcia nacita sluzby dla mesiaca a roku
    /// </summary>
    /// <param name="table"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>

    public SortedList loadSluzbaMonthYear(string table, string month, string year)
    {
        string query = "SELECT * FROM " + table + " WHERE mesiac='" + month + "' AND rok='" + year+"'";
        SortedList result = new SortedList();

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

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
        else
        {
            result.Add("ziadna_sluzba", "true");

        }
        
        my_con.Close();

       


        return result;
    }

    public string[] getFreeDays()
    {
        string query = "SELECT * FROM is_settings WHERE name='free_days'";

        my_con.Open();
        //my_con.Open();
        OdbcCommand my_com = new OdbcCommand(query, my_con);

        OdbcDataReader reader = my_com.ExecuteReader();
        string tmp = "";

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tmp = reader["data"].ToString();
            }
        }
        my_con.Close();

        string[] result = tmp.Split(new char[] { ',' });

        return result;

    }

    public ArrayList getDovolenky(int month, int year)
    {
        ArrayList result = new ArrayList();

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT is_users.id,is_users.full_name,is_dovolenky.id AS dov_id,is_dovolenky.user_id,is_dovolenky.od,is_dovolenky.do FROM is_users INNER JOIN is_dovolenky on is_users.id = is_dovolenky.user_id WHERE (MONTH(is_dovolenky.od) = '{0}' OR MONTH(is_dovolenky.do) = '{0}') AND (YEAR(is_dovolenky.od) = '{1}' OR YEAR(is_dovolenky.do) = '{1}')", month,year);

       // string query = "SELECT is_users.id,is_users.full_name,is_dovolenky.id,is_dovolenky.user_id,is_dovolenky.od,is_dovolenky.do FROM is_users INNER JOIN is_dovolenky on is_users.id = is_dovolenky.user_id WHERE is_dovolenky.od >= '"+od_datum+"' AND is_dovolenky.do <= '"+do_datum+"'";

        my_con.Open();
        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();
        
        //int j = 0;
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result.Add(reader["full_name"].ToString() + ";" + reader["od"].ToString() + ";" + reader["do"].ToString());
            }
            
        }
        my_con.Close();


        return result;
    }

    public OdbcDataReader getAllStatusDov()
    {

        string query = "SELECT is_users.id, is_users.full_name, is_dovolen_zost.id AS dovzost_id, is_dovolen_zost.user_id, is_dovolen_zost.zostatok, is_dovolen_zost.narok FROM is_users INNER JOIN is_dovolen_zost ON is_users.id = is_dovolen_zost.user_id";
        //SortedList result = new SortedList();
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);
        OdbcDataReader reader = my_com.ExecuteReader();
        //my_con.Close();

        //OdbcDataReader result = reader;
        //my_con.Close();

        return reader;
        my_con.Close();
    }

    public SortedList getDovolStatus(string table, string id)
    {
        SortedList result = new SortedList();
        
        string query = "SELECT is_users.id, is_users.full_name, is_dovolen_zost.id AS dovzost_id, is_dovolen_zost.user_id, is_dovolen_zost.zostatok, is_dovolen_zost.narok FROM is_users INNER JOIN is_dovolen_zost ON is_users.id = is_dovolen_zost.user_id WHERE is_dovolen_zost.user_id = " + id;
        my_con.Open();
       
        try
        {

            OdbcCommand my_com = new OdbcCommand(query, my_con);
            OdbcDataReader reader = my_com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add("fullname", reader["full_name"].ToString());
                    result.Add("zostatok", reader["zostatok"].ToString());
                    result.Add("narok", reader["narok"].ToString());
                    result.Add("id", reader["dovzost_id"].ToString());

                }

            }
            my_con.Close();
        }
        catch (Exception e)
        {
            my_con.Close();
            result.Add("error", e.ToString());
        }


      

        return result;
    }

    public bool checkDovExists(string user_id, string datOd, string datDo)
    {
        bool result = false;

        StringBuilder query = new StringBuilder();

        //query.AppendFormat("SELECT is_users.id,is_users.full_name,is_dovolenky.id AS dov_id,is_dovolenky.user_id,is_dovolenky.od,is_dovolenky.do FROM is_users INNER JOIN is_dovolenky on is_users.id = is_dovolenky.user_id WHERE is_dovolenky.user_id = {0} AND ((MONTH(is_dovolenky.od) = '{1}' OR MONTH(is_dovolenky.do) = '{1}') AND (YEAR(is_dovolenky.od) = '{2}' OR YEAR(is_dovolenky.do)) = '{2}')", user_id,month,year);

        query.AppendFormat("SELECT * FROM is_dovolenky WHERE user_id = {0} AND ((od >= DATE('{1}') AND do <= DATE('{2}')) OR (od <= DATE('{2}') AND do >= DATE('{1}')))", user_id, datOd, datDo);

        my_con.Open();  
        OdbcCommand my_com = new OdbcCommand(query.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            result = true;
        }
        my_con.Close();


        return  result;

    }

    public ArrayList getDovolenkyWithID(int month,int year)
    {
        ArrayList result = new ArrayList();

        StringBuilder query = new StringBuilder();
        query.AppendFormat("SELECT is_users.id,is_users.full_name,is_dovolenky.id AS dov_id,is_dovolenky.user_id,is_dovolenky.od,is_dovolenky.do FROM is_users INNER JOIN is_dovolenky on is_users.id = is_dovolenky.user_id WHERE (MONTH(is_dovolenky.od) = '{0}' OR MONTH(is_dovolenky.do) = '{0}') AND (YEAR(is_dovolenky.od) = '{1}' OR YEAR(is_dovolenky.do) = '{1}')", month,year);

        //string query = "SELECT is_users.id,is_users.full_name,is_dovolenky.id AS dov_id,is_dovolenky.user_id,is_dovolenky.od,is_dovolenky.do FROM is_users INNER JOIN is_dovolenky on is_users.id = is_dovolenky.user_id WHERE is_dovolenky.od >= '" + od_datum + "' AND is_dovolenky.do <= '" + do_datum+"'";

        my_con.Open();
        OdbcCommand my_com = new OdbcCommand(query.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        //int j = 0;
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result.Add(reader["full_name"].ToString() + ";" + reader["od"].ToString() + ";" + reader["do"].ToString()+";"+reader["dov_id"].ToString());
            }

        }
        my_con.Close();


        return result;
    }

    public string eraseRowByID(string table, string id)
    {
        //string result = "";

        string query = "DELETE FROM " + table + " WHERE id=" + id;
        try
        {


            my_con.Open();

            OdbcCommand my_com = new OdbcCommand(query, my_con);

            my_com.ExecuteNonQuery();
            my_con.Close();
            return "ok";
        }
        catch (Exception e)
        {
            return e.ToString();
        }

        //return result;
    }

    public SortedList getDovolenkaByID(string id)
    {
        string query = "SELECT * FROM is_dovolenky WHERE id = "+id;

        my_con.Open();
        OdbcCommand my_com = new OdbcCommand(query, my_con);
        OdbcDataReader reader = my_com.ExecuteReader();
        SortedList result = new SortedList();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result.Add("od",reader["od"].ToString());
                result.Add("do", reader["do"].ToString());
                result.Add("user_id", reader["user_id"].ToString());
            }
        }
        my_con.Close();

        return result;
    }

    
    //cast urcena pre staze

    public SortedList loadStazeMonthYear(string table, string month, string year)
    {
        string query = "SELECT * FROM " + table + " WHERE `mesiac`='" + month + "' AND `rok`='" + year + "'";
        SortedList result = new SortedList();

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

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
        else
        {
            result.Add("ziadna_staze", "true");

        }

        my_con.Close();




        return result;
    }

    public string updateStaze(string table, SortedList data, string id)
    {
        string result = this.update_row(table, data, id);

        return result;
    }

    public SortedList insertStaze(string table, SortedList data)
    {
        SortedList result = this.insert_rows(table, data);

        return result;
    }

    /* cast pre poziadavky */

    public SortedList savePoziadavky(string table, SortedList data)
    {

        
        StringBuilder sb = new StringBuilder();        
        sb.AppendFormat("SELECT * FROM {0} WHERE mesiac='{1}' AND rok='{2}'", table.ToString(), data["mesiac"].ToString(), data["rok"].ToString());

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();
        SortedList result = new SortedList();

        if (reader.HasRows)
        {

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
           result =  this.insert_rows(table, data);
        }

       

        if (result["id"] != null)
        {
            this.update_row("is_poziadavky_data", data, result["id"].ToString());
           // result.Clear();
            result["datum"] = data["datum"].ToString();
        }
        else
        {
            result["datum"] = data["datum"].ToString();
        }

       

        return result;

    }

    public SortedList saveUserPoziadavka(string mesiac, string rok, SortedList data)
    {
        SortedList result = new SortedList();

        StringBuilder sb = new StringBuilder();

        sb.Append("SELECT is_users.id, is_users.full_name, is_poziadavky_info.id AS info_id, is_poziadavky_info.user_id, is_poziadavky_info.mesiac , is_poziadavky_info.rok, is_poziadavky_info.info ");         
        
        sb.Append("FROM is_users INNER JOIN is_poziadavky_info ON is_poziadavky_info.user_id = is_users.id ");
        sb.AppendFormat("WHERE is_poziadavky_info.mesiac = {0} AND is_poziadavky_info.rok = {1} AND is_users.id = {2}", mesiac, rok,data["user_id"].ToString());

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
            result = this.insert_rows("is_poziadavky_info", data);
        }

        if (result["info_id"] != null)
        {
            this.update_row("is_poziadavky_info", data, result["info_id"].ToString());
            // result.Clear();
            result["info"] = data["info"].ToString();
        }
        else
        {
            result["info"] = data["info"].ToString();
        }

        return result; 

    }

    public SortedList getAllPoziadavky(DateTime datum)
    {

        int mesiac = datum.AddMonths(1).Month;
        int rok = datum.Year;

        if (datum.Month == 12)
        {
            rok += 1;
        }

        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT is_users.id, is_users.full_name, is_poziadavky_info.id AS info_id, is_poziadavky_info.user_id, is_poziadavky_info.mesiac , is_poziadavky_info.rok, is_poziadavky_info.info ");

        sb.Append("FROM is_users INNER JOIN is_poziadavky_info ON is_poziadavky_info.user_id = is_users.id ");
        sb.AppendFormat("WHERE is_poziadavky_info.mesiac = {0} AND is_poziadavky_info.rok = {1}", mesiac, rok);

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                
                result.Add(reader["full_name"].ToString(), reader["info"].ToString());
                
            }
            my_con.Close();
        }
        else
        {
            result.Add("status", "empty");
        }

        return result;
    }

    public SortedList getAllPoziadavkySel(string month, string year)
    {

        int mesiac = Convert.ToInt32(month);
        int rok = Convert.ToInt32(year);

       // if (datum.Month == 12)
       // {
       //     rok += 1;
       // }

        SortedList result = new SortedList();
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT is_users.id, is_users.full_name, is_poziadavky_info.id AS info_id, is_poziadavky_info.user_id, is_poziadavky_info.mesiac , is_poziadavky_info.rok, is_poziadavky_info.info ");

        sb.Append("FROM is_users INNER JOIN is_poziadavky_info ON is_poziadavky_info.user_id = is_users.id ");
        sb.AppendFormat("WHERE is_poziadavky_info.mesiac = {0} AND is_poziadavky_info.rok = {1}", mesiac, rok);

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {

                result.Add(reader["full_name"].ToString(), reader["info"].ToString());

            }
            my_con.Close();
        }
        else
        {
            result.Add("status", "empty");
        }

        return result;
    }

    public SortedList getPoziadavky(string mesiac, string rok, string user_id)
    {
        SortedList result = new SortedList();

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM is_poziadavky_info WHERE `user_id` = {0} AND `mesiac` = {1} AND `rok` = {2}", user_id, mesiac, rok);

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            result.Add("status", "empty");
        }



        return result;
    }

    public SortedList getNextPozDatum(DateTime datum)
    {
        SortedList result = new SortedList();

        int mesiac = datum.Month;
        int rok = datum.Year;

        
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("SELECT * FROM is_poziadavky_data WHERE `mesiac` = {0} AND `rok` = {1}", mesiac, rok);

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
            SortedList data = new SortedList();
            data.Add("mesiac", mesiac.ToString());
            data.Add("rok", rok.ToString());
            data.Add("datum","18."+mesiac.ToString()+"."+rok.ToString());

            result = this.insert_rows("is_poziadavky_data", data);

            result.Add("datum", data["datum"].ToString());


        }


        return result;
    }

    public SortedList getUserPoziadavky(string user_id, DateTime datum)
    {
        SortedList result = new SortedList();
        int mesiac = datum.AddMonths(1).Month;
        int rok = datum.Year;

        if (datum.Month == 12)
        {
            rok += 1;
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("SELECT * FROM is_poziadavky_info WHERE `user_id`={0} AND `mesiac`={1} AND `rok` = {2}", user_id, mesiac, rok);

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
          
          

            result.Add("status", "empty");


        }

        return result;
    }

    /* ohladom is_news */

    public SortedList saveNews(SortedList data)
    {
        SortedList result = new SortedList();

       // StringBuilder sb = new StringBuilder();
        //sb.AppendFormat("SELECT * FROM is_news WHERE `user_id` = {0} AND `mesiac` = {1} AND `rok` = {2}", user_id, mesiac, rok);

        result = this.insert_rows("is_news", data);

        return result;

    }

    public SortedList getNews()
    {
        string query = "SELECT *, DATE(`datum`) as `n_d` FROM `is_news` ORDER BY `n_d` DESC LIMIT 5";

        SortedList result = new SortedList();
        //int i = 0;

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {

                result.Add(reader["id"].ToString(), "<strong>" + reader["n_d"].ToString() + "</strong><br/>" + reader["kratka_sprava"].ToString());
                //i++;  

            }
            my_con.Close();
        }
        else
        {
            my_con.Close();


            result.Add("status", "empty");


        }

        return result;
    }

    public DataSet getData_News()
    {
       
        string query = "SELECT * FROM `is_news` ORDER BY DATE(datum) DESC";
        string conn = my_con.ConnectionString.ToString();
        //string conn = @"Driver={MySQL ODBC 3.51 Driver};uid=root;password=aa;Server=127.0.0.1;Option=3;Database=kdch_sk;";
        my_con.Open();
       
        OdbcDataAdapter da = new OdbcDataAdapter(query, conn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }

    public string DeleteNewsRow(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("DELETE FROM `is_news` WHERE `id` = {0} ",id);
        string result = "";
        try
        {


            my_con.Open();

            OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

            my_com.ExecuteNonQuery();
            my_con.Close();
            return "ok";
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    public string getNewsByID(int id)
    {
        StringBuilder sb = new StringBuilder();
        string result = "";
        sb.AppendFormat("SELECT * FROM `is_news` WHERE `id`={0}", id);
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result = reader["cela_sprava"].ToString();

            }
            my_con.Close();
        }
        else
        {
            my_con.Close();


            result = "empty";


        }

        return result;
    }

    public SortedList getInfoNewsData (int id)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM `is_news` WHERE `id`={0}",id);

        my_con.Open();
        SortedList result = new SortedList();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                 for (int i = 0; i < reader.FieldCount; i++)
                 {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                 }

            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
            result.Add("status", "empty");

        }

        return result;
    }

    /* urcene pre rozpis*/
    public SortedList loadRozpisMonthYear(int month, int year)
    {
        //string query = "SELECT * FROM " + table + " WHERE `mesiac`='" + month + "' AND `rok`='" + year + "'";

        StringBuilder query = new StringBuilder();
        query.AppendFormat("SELECT * FROM is_rozpis WHERE `mesiac`='{0}' AND `rok`='{1}'", month, year);

        SortedList result = new SortedList();

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

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
        else
        {
            result.Add("ziaden_rozpis", "true");

        }

        my_con.Close();




        return result;
    }

    public string updateRozpis(string table, SortedList data, string id)
    {
        string result = this.update_row(table, data, id);

        return result;
    }

    /* ohladom op_programu */

    public SortedList getPassPhrase()
    {
        SortedList result = new SortedList();
        string query = "SELECT * FROM is_settings WHERE `name`='opprogram'";

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query.ToString(), my_con);


        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }
            }
            x2_var my_x2 = new x2_var();

            result["data"] = my_x2.make_hash(result["data"].ToString());
        }
        else
        {
            result.Add("ziaden_program", "true");

        }

        my_con.Close();




        return result;
    }


    public SortedList saveOpProgram(SortedList data)
    {
        SortedList result = new SortedList();

        // StringBuilder sb = new StringBuilder();
        //sb.AppendFormat("SELECT * FROM is_news WHERE `user_id` = {0} AND `mesiac` = {1} AND `rok` = {2}", user_id, mesiac, rok);

        result = this.insert_rows("is_opprogram", data);

        return result;

    }

    public SortedList getOpProgram()
    {
        string query = "SELECT * FROM is_opprogram ORDER BY DATE(datum) DESC LIMIT 5";

        SortedList result = new SortedList();
        //int i = 0;

        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(query, my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {

                result.Add(reader["id"].ToString(), "<strong>" + reader["datum_txt"].ToString() + "</strong><br/>" + reader["kratka_sprava"].ToString());
                //i++;  

            }
            my_con.Close();
        }
        else
        {
            my_con.Close();


            result.Add("status", "empty");


        }

        return result;
    }

    public DataSet getData_OpProgram()
    {

        string query = "SELECT * FROM is_opprogram ORDER BY DATE(datum) DESC LIMIT 15 ";
        string conn = my_con.ConnectionString.ToString();
        //string conn = @"Driver={MySQL ODBC 3.51 Driver};uid=root;password=aa;Server=127.0.0.1;Option=3;Database=kdch_sk;";
        my_con.Open();

        OdbcDataAdapter da = new OdbcDataAdapter(query, conn);
        DataSet ds = new DataSet();
        da.Fill(ds);
        my_con.Close();

        return ds;
    }

    public string DeleteOpProgramRow(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("DELETE FROM `is_opprogram` WHERE `id` = {0} ", id);
        string result = "";
        try
        {


            my_con.Open();

            OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

            my_com.ExecuteNonQuery();
            my_con.Close();
            return "ok";
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    public SortedList getOpProgramByID(int id)
    {
        StringBuilder sb = new StringBuilder();

        SortedList result = new SortedList();
        sb.AppendFormat("SELECT * FROM `is_opprogram` WHERE `id`={0}", id);
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                result.Add("full_text", reader["cela_sprava"].ToString());
                result.Add("short_text", reader["kratka_sprava"].ToString());

            }
            my_con.Close();
        }
        else
        {
            my_con.Close();


            result.Add("status", "empty");


        }

        return result;
    }

    

    public SortedList getInfoOpProgramData(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM `is_opprogram` WHERE `id`={0}", id);

        my_con.Open();
        SortedList result = new SortedList();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());
                }

            }
            my_con.Close();
        }
        else
        {
            my_con.Close();
            result.Add("status", "empty");

        }

        return result;
    }
}
