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
/// Summary description for vykaz
/// </summary>
public class vykazdb : my_db
{
    x2_var myX2 = new x2_var();
    //public mysql_db x2Mysql = new mysql_db();

	public vykazdb()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    

    public Boolean[] getSluzbyOfUserByID(string id, string mesiac, string rok)
    {
        my_db x_db = new my_db();

        SortedList data_info = this.loadSluzbaMonthYear("is_sluzby", mesiac, rok);
        int pocet_dni = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        Boolean[] sluzbyName = new bool[pocet_dni];

        if (data_info["rozpis"] != null)
        {

            String[][] data = myX2.parseSluzba(data_info["rozpis"].ToString());
            int tmp = 0;
            string meno = "";
            for (int i = 0; i < pocet_dni; i++) 
            {
                tmp = data[i].Length-1;

                for (int j = 0; j < tmp; j++)
                {

                    if (j == 1)
                    {
                        string[] tmpId = data[i][j].Split('|');
                        meno = tmpId[0];
                    }
                    else
                    {

                        meno = data[i][j].ToString();
                    }

                   // meno = data[i][j].ToString();

                    if (meno == id )
                    {
                        sluzbyName[i] = true;
                        j=tmp;
                    }
                    else
                    {
                        sluzbyName[i] = false;
                    }
                }
            }
        }
       

            //string result = "";



        return sluzbyName;

    }

    public Boolean[] getSluzbyOfUser(string name,string mesiac, string rok)
        {
            my_db x_db = new my_db();

            SortedList data_info = this.loadSluzbaMonthYear("is_sluzby", mesiac, rok);

            String[][] data = myX2.parseSluzba(data_info["rozpis"].ToString());

            
            int pocet_dni = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
            Boolean[] sluzbyName = new bool[pocet_dni];

            int tmp =0;
            string meno = "";
            for (int i = 0; i < pocet_dni; i++)
            {
                tmp = data[i].Length-1;

                for (int j = 0; j < tmp; j++) //kvoli tomu ze ambulancia sa nerata ako sluzba
                {
                    meno = data[i][j].ToString();

                    if (meno.IndexOf(name) != -1)
                    {
                        sluzbyName[i] = true;
                        break;
                    }
                    else
                    {
                        sluzbyName[i] = false;
                    }
                }
            }


            //string result = "";



            return sluzbyName;

        }

    public SortedList getCurrentVykaz(int user_id, int mesiac, int rok)
    {
        string query = "SELECT * FROM `is_vykaz` WHERE `user_id` = {0} AND `mesiac` = {1} AND `rok` = {2}";
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat(query, user_id, mesiac, rok);
        SortedList result = new SortedList();
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
            
        }
        else
        {
            result.Add("nodata","1");
        }
        my_con.Close();

        return result;
        
    }

    public string getPrevMonthPrenos(int user_id, int mesiac, int rok)
    {
        if (mesiac == 12)
        {
            mesiac = 1;
            rok--;
        }
        else
        {
            mesiac--;
        }

        string query = "SELECT [prenos] FROM [is_vykaz] WHERE [user_id] = {0} AND [mesiac] = {1} AND [rok] = {2}";
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat(query, user_id, mesiac, rok);
        string result = "0";
        my_con.Open();

        OdbcCommand my_com = new OdbcCommand(sb.ToString(), my_con);

        OdbcDataReader reader = my_com.ExecuteReader();
        

        if (reader.HasRows)
        {
            result = reader[0].ToString();
        }
        my_con.Close();
        

        return result;



    }


}
