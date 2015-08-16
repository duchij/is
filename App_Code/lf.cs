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
        OdbcCommand my_com = new OdbcCommand(this.parseQuery(sb.ToString()), my_con);

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

    public SortedList lf_lfUpdateData(byte[] data, SortedList lfData)
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
                reader.GetBytes(0, 0, result, 0, size);
            }
        }

        my_con.Close();

        return result;

    }

    public SortedList lf_lfInsertData(byte[] data, SortedList lfData)
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
        cmd.Parameters.Add("userid", OdbcType.BigInt).Value = Convert.ToInt32(lfData["user_id"]);
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
            result.Add("status", false);
            result.Add("msg", ex.ToString());
            cmd.Transaction.Rollback();
        }
        my_con.Close();
        return result;
    }



}