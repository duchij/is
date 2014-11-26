﻿using System;

using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class helpers_convsluz : System.Web.UI.Page
{
    public mysql_db x2MySql = new mysql_db();
    public x2_var x2 = new x2_var();
    public my_db x_db = new my_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void runConversion_fnc(object sender, EventArgs e)
    {
        int rok = Convert.ToInt32(this.rok_txt.Text.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_txt.Text.ToString());


        SortedList data_info = x_db.loadSluzbaMonthYear("is_sluzby", this.mesiac_txt.Text.ToString(), this.rok_txt.Text.ToString());

        String[][] data = x2.parseSluzba(data_info["rozpis"].ToString());

        int rows = data.Length;
        int cols = data[0].Length;

        string[] tmpVal = new string[rows*5];
        

        StringBuilder sb = new StringBuilder();
        StringBuilder sbDatum = new StringBuilder();

        int x = 0;

        for (int den = 0; den < rows; den++)
        {
            sbDatum.Length = 0;
            sbDatum.AppendFormat("{0}-{1}-{2}",rok,mesiac,den+1);
            string typ = "";
            string userId = "";
            string comment = "";
            for (int one = 0; one < cols; one++)
            {
                if (one == 1)
                {
                    typ = "OUP";
                    string[] tmp = data[den][one].Split('|');
                    userId = tmp[0].ToString();
                    comment = tmp[1].ToString();

                }

                if (one == 2)
                {
                    typ = "OddA";
                    userId = data[den][one];
                    comment = "";
                }

                if (one == 3)
                {
                    typ = "OddB";
                    userId = data[den][one];
                    comment = "";
                }

                if (one == 4)
                {
                    typ = "OP";
                    userId = data[den][one];
                    comment = "";
                }

                if (one == 5)
                {
                    typ = "Prijm";
                    userId = data[den][one];
                    comment = "";
                }
                if (one > 0)
                {
                    sb.AppendFormat("('{0}','{1}','{2}','{3}')", sbDatum.ToString(), typ, userId, comment);

                    tmpVal[x] = sb.ToString();

                    sb.Length = 0;
                    x++;
                }
            }
        }

        string tmpRes = String.Join(",", tmpVal);

        StringBuilder query = new StringBuilder();
        query.AppendFormat("INSERT INTO [is_sluzby_2] ([datum],[typ],[user_id],[comment]) VALUES {0} ON DUPLICATE KEY UPDATE", tmpRes);
        query.Append(" [datum]=values([datum]), [typ]=values([typ]), [user_id]=values([user_id]), [comment]=values([comment])");

        SortedList res = x2MySql.execute(query.ToString());

        Boolean status = Convert.ToBoolean(res["status"]);
        if (status == false)
        {
            this.result.Text = res["query"].ToString()+"<br>"+res["msg"].ToString();
        }



        //data


    }
}