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
/// Summary description for lf
/// </summary>
public class lf : mysql_db
{
	public lf()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public byte[] lf_getLfContent(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [file-content] FROM [is_data] WHERE [id]='{0}'", id);
        my_con.Open();
        MySqlCommand my_com = new MySqlCommand(this.parseQuery(sb.ToString()), my_con);

        byte[] result = (byte[])my_com.ExecuteScalar();
        my_con.Close();
        return result;

    }

    public SortedList lf_getLfData2(int id) 
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
        my_con.Close();
        return result;

    }

    public SortedList lf_lfUpdateData(byte[] data, SortedList lfData)
    {
        SortedList result = new SortedList();
        MySqlTransaction trans1 = null;
        my_con.Open();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
        cmd.CommandText = "UPDATE `is_data_2` SET `file-name`=?,`file-size`=?,`file-type`=?,`file-content`=? WHERE `id`=?";

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
            result.Add("last_id", Convert.ToInt32(lfData["id"]));
            cmd.Transaction.Commit();
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error wrong sql in lfUpdateData()");
            result.Add("status", false);
            result.Add("msg", ex.ToString());
            cmd.Transaction.Rollback();
        }
        my_con.Close();
        return result;
    }

    public byte[] lf_lfStoredData(int id, int size)
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
                reader.GetBytes(0, 0, result, 0, size);
            }
        }

        my_con.Close();

        return result;

    }

    public SortedList lf_lfInsertData(byte[] data, SortedList lfData)
    {
        SortedList result = new SortedList();
        MySqlTransaction trans1 = null;
        
        my_con.Open();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
        cmd.CommandText = "INSERT INTO `is_data_2`(`file-name`,`file-size`,`file-type`,`file-content`, `user_id`, `clinic_id`) VALUES (?,?,?,?,?,?)";

        cmd.Parameters.Add("filename", MySqlDbType.Text).Value = lfData["file-name"].ToString();
        cmd.Parameters.Add("filesize", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["file-size"]);
        cmd.Parameters.Add("filetype", MySqlDbType.VarChar).Value = lfData["file-type"].ToString();
        cmd.Parameters.Add("filecontent", MySqlDbType.Binary).Value = data;
        cmd.Parameters.Add("userid", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["user_id"]);
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
            result.Add("status", false);
            result.Add("msg", ex.ToString());
            cmd.Transaction.Rollback();
        }
        my_con.Close();
        return result;
    }

    


    public Dictionary<int,Hashtable> getFiles(int clinic, int folder, int userId)
    {

        string query = "";
        if (folder== 0)
        {
            query = @"  SELECT [t_struct.item_lf_id] AS [item_lf_id],[t_struct.item_name] AS [item_name],[t_struct.item_comment] AS [item_comment],
                          [t_struct.item_id] AS [item_id], [t_struct.user_id] AS [user_id]
                        FROM [is_structure] AS [t_struct]
                       WHERE [t_struct.clinic_id]='{0}'
                        AND [t_struct.item_parent_id]='{1}'
                        ";

        }
        else
        {
            query = @"SELECT [t_struct.item_lf_id] AS [item_lf_id],[t_struct.item_name] AS [item_name],[t_struct.item_comment] AS [item_comment],
                             [t_struct.item_id] AS [item_id], [t_struct.user_id] AS [user_id]
                            FROM [is_structure] AS [t_struct]
                        INNER JOIN [is_data_2] AS [t_data] ON [t_data.id] = [t_struct.item_lf_id]
                            WHERE [t_struct.clinic_id]='{0}'
                                AND [t_struct.item_parent_id]='{1}'
                        ";
        }
        query = this.buildSql(query, new string[] { clinic.ToString(),folder.ToString(),userId.ToString() });
        return this.getTable(query);
    }


    public SortedList deleteFolder(int folderId)
    {
        SortedList result = new SortedList();
        MySqlTransaction trans1 = null;

        Boolean status = true;
        my_con.Open();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;
        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;

        string query = "UPDATE [is_structure] SET [item_parent_id]=0 WHERE [item_parent_id]={0}";

        query = this.buildSql(query, new string[] { folderId.ToString() });

        try
        {
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            

            cmd.Transaction.Rollback();
            my_con.Close();
            x2log.logData(query, ex.ToString(), "error to delete folder step1");
            status = false;
            result.Add("status", false);
            result.Add("msg", ex.ToString());
        }

        if (status)
        {
            query = "DELETE FROM [is_structure] WHERE [item_id]={0}";
            query = this.buildSql(query, new string[] { folderId.ToString() });

            try
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();  
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                my_con.Close();
                status = false;
                x2log.logData(query, ex.ToString(), "error to delete folder step2");
                result.Add("status", false);
                result.Add("msg", ex.ToString());
            }
        }
       

        if (status)
        {
            cmd.Transaction.Commit();
            my_con.Close();
            result.Add("status", true);
            
        }

        return result;
    }

    //public int sameContent(string hash,MySqlConnection myCon, MySqlTransaction trans)
    public int sameContent(string hash)
    {
        int result = 0;

        string sql = this.buildSql("SELECT [id] FROM [is_data_2] WHERE [hash]='{0}'", new string[] { hash });

        SortedList res = this.getRowInCon(sql);
        
        if (res["id"] != null)
        {
            result = Convert.ToInt32(res["id"]);  
        }

        return result;

    }

    public SortedList storeLfDataInTable(byte[] content, SortedList lfData, string table,SortedList isStruct)
    {
        SortedList result = new SortedList();
        int tmp = this.sameContent(isStruct["item_hash"].ToString());
        MySqlTransaction trans1 = null;
        my_con.Open();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;

        // cmd.CommandText.ToString();

        try
        {

            int id = 0;

            
            

            if (tmp == 0)
            {
                cmd.CommandText = "INSERT INTO `is_data_2`(`file-name`,`file-size`,`file-type`,`file-content`, `user_id`, `clinic_id`, `hash`) VALUES (?,?,?,?,?,?,?)";

                cmd.Parameters.Add("filename", MySqlDbType.Text).Value = lfData["file-name"].ToString();
                cmd.Parameters.Add("filesize", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["file-size"]);
                cmd.Parameters.Add("filetype", MySqlDbType.VarChar).Value = lfData["file-type"].ToString();
                cmd.Parameters.Add("filecontent", MySqlDbType.Binary).Value = content;
                cmd.Parameters.Add("userid", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["user_id"]);
                cmd.Parameters.Add("clinicid", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["clinic_id"]);
                cmd.Parameters.Add("hash", MySqlDbType.VarChar).Value = isStruct["item_hash"].ToString();


                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT LAST_INSERT_ID();";

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else
            {
                id = tmp;
            }



            isStruct.Add("item_lf_id", id);

           // SortedList res = this.mysql_insert_nt(table, isStruct, ref my_con, trans1);
            SortedList res = this.mysql_insert_nt(table, isStruct, ref cmd);

            if (Convert.ToBoolean(res["status"]))
            {
                result.Add("status", true);

                cmd.Transaction.Commit();
            }
            else
            {
                x2log.logData(res["sql"].ToString(), res["msg"].ToString(), "error wrong sql in storeLfDataInTable() insert in structure");
                x2log.logData(lfData, "chyba vystup lf data", "error in lfinsert");
                result.Add("status", false);
                result.Add("msg", res["msg"].ToString());

                cmd.Transaction.Rollback();
            }
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error wrong sql in lfInsertData()");
            //x2log.logData(lfData, "chyba vystup lf data", "error in lfinsert");
            result.Add("status", false);
            result.Add("msg", ex.ToString());
            cmd.Transaction.Rollback();
        }
        my_con.Close();

        return result;
    }


    public SortedList storeLfData(byte[] content, SortedList lfData, SortedList isStruct)
    {
        SortedList result = new SortedList();

        int tmp = this.sameContent(isStruct["item_hash"].ToString());
        
        MySqlTransaction trans1 = null;
        my_con.Open();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = my_con;
        cmd.CommandType = CommandType.Text;

        trans1 = my_con.BeginTransaction();

        cmd.Transaction = trans1;
       
       // cmd.CommandText.ToString();

        try
        {

            int id = 0;

            if (tmp == 0)
            {
                cmd.CommandText = @"INSERT INTO `is_data_2`(`file-name`,`file-size`,`file-type`,`file-content`, `user_id`, `clinic_id`, `hash`)
                                        VALUES (@filename,@filesize,@filetype,@filecontent,@userid,@clinicid,@hash)";

                cmd.Parameters.Add("filename", MySqlDbType.Text).Value = lfData["file-name"].ToString();
                cmd.Parameters.Add("filesize", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["file-size"]);
                cmd.Parameters.Add("filetype", MySqlDbType.VarChar).Value = lfData["file-type"].ToString();
                cmd.Parameters.Add("filecontent", MySqlDbType.Binary).Value = content;
                cmd.Parameters.Add("userid", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["user_id"]);
                cmd.Parameters.Add("clinicid", MySqlDbType.Int32).Value = Convert.ToInt32(lfData["clinic_id"]);
                cmd.Parameters.Add("hash", MySqlDbType.VarChar).Value = isStruct["item_hash"].ToString();


                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT LAST_INSERT_ID();";

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else
            {
                id = tmp;
            }

            

            isStruct.Add("item_lf_id", id);
             
            SortedList res = this.mysql_insert_nt("is_structure", isStruct, ref cmd); 

            if (Convert.ToBoolean(res["status"]))
            {
                result.Add("status", true);

                cmd.Transaction.Commit();
            }
            else
            {
                x2log.logData(res["sql"].ToString(), res["msg"].ToString(), "error wrong sql in storeLfData() insert in structure");
               // x2log.logData(lfData, "chyba vystup lf data", "error in lfinsert");
                result.Add("status", false);
                result.Add("msg", res["msg"].ToString());

                cmd.Transaction.Rollback();
            }
        }
        catch (Exception ex)
        {
            x2log.logData(cmd.CommandText.ToString(), ex.ToString(), "error wrong sql in lfInsertData()");
            x2log.logData(lfData, "chyba vystup lf data", "error in lfinsert");
            result.Add("status", false);
            result.Add("msg", ex.ToString());
            cmd.Transaction.Rollback();
        }
        my_con.Close();

        return result;
    }

    



}