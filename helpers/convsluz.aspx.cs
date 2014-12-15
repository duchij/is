using System;

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
        this.result.Text = "";


        int rok = Convert.ToInt32(this.rok_txt.Text.ToString());

        string mesiacStr = this.mesiac_txt.Text.ToString();

        if (mesiacStr.Length == 1)
        {
            mesiacStr = "0" + mesiacStr;
        }
        string dateGroup = this.rok_txt.Text.ToString() + mesiacStr;

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
            int ordering=0;
            for (int one = 0; one < cols; one++)
            {
                if (one == 1)
                {
                    typ = "OUP";
                    string[] tmp = data[den][one].Split('|');
                    userId = tmp[0].ToString();
                    comment = tmp[1].ToString();
                    ordering = 1;

                }

                if (one == 2)
                {
                    typ = "OddA";
                    userId = data[den][one];
                    comment = "";
                    ordering = 2;
                }

                if (one == 3)
                {
                    typ = "OddB";
                    userId = data[den][one];
                    comment = "";
                    ordering = 3;
                }

                if (one == 4)
                {
                    typ = "OP";
                    userId = data[den][one];
                    comment = "";
                    ordering = 4;
                }

                if (one == 5)
                {
                    typ = "Prijm";
                    userId = data[den][one];
                    comment = "";
                    ordering = 5;
                }
                if (one > 0)
                {
                    sb.AppendFormat("('{0}','{1}','{2}','{3}','0','{4}')", sbDatum.ToString(), typ, userId, comment,ordering);

                    tmpVal[x] = sb.ToString();

                    sb.Length = 0;
                    x++;
                }
            }
        }

        string tmpRes = String.Join(",", tmpVal);

        StringBuilder query = new StringBuilder();
        query.AppendFormat("INSERT INTO [is_sluzby_2] ([datum],[typ],[user_id],[comment],[date_group],[ordering]) VALUES {0} ON DUPLICATE KEY UPDATE", tmpRes);
        query.Append(" [datum]=values([datum]), [typ]=values([typ]), [user_id]=values([user_id]), [comment]=values([comment]),[date_group]=values([date_group]),[ordering]=values([ordering])");

        SortedList res = x2MySql.execute(query.ToString());

        Boolean status = Convert.ToBoolean(res["status"]);
        if (status == false)
        {
            this.result.Text = res["query"].ToString() + "<br>" + res["msg"].ToString();
        }
        else
        {
            this.result.Text = "OK";
        }



        //data


    }
}
