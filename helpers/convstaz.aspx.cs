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

        SortedList data_info = x_db.loadStazeMonthYear("is_staze", mesiac.ToString(), rok.ToString());
        Dictionary<int, Hashtable> insData = new Dictionary<int,Hashtable>();
        if (data_info["ziadne_staze"] == null)
        {
            string data = data_info["rozpis"].ToString();

            string[][] rozpis = x2.parseStaz(data);

            int days = rozpis.Length;
            int ins = 0;
            for (int row=0; row< days;row++)
            {
                int cols = rozpis[row].Length;
                 
                for (int col = 0; col < 6; col++)
                {
                    Hashtable tmp = new Hashtable(); 
                   // if (col == 0)
                    //{
                     tmp.Add("datum", rok + "-" + mesiac + "-" + rozpis[row][0].TrimStart());
                    //}
                    if (col == 1)
                    {
                        tmp.Add("type", "3roc");
                    }
                    if (col == 2)
                    {
                        tmp.Add("type", "4roc");
                    }
                    if (col == 3)
                    {
                        tmp.Add("type", "5roc");
                    }
                    if (col == 4)
                    {
                        tmp.Add("type", "6roc");
                    }
                    if (col == 5)
                    {
                        tmp.Add("type", "Cudz");
                    }

                    tmp.Add("clinic", 3);
                    tmp.Add("text", rozpis[row][col]);
                    tmp.Add("date_group", dateGroup);
                    if (col > 0)
                    {
                        insData.Add(ins, tmp);
                        ins++;
                    }
                    
                    
                }
                

            }

        }


       /// string tmpRes = String.Join(",", tmpVal);


        SortedList res = x2MySql.mysql_insert_arr("is_staze_2", insData);

        Boolean status = Convert.ToBoolean(res["status"]);
        if (status == false)
        {
            this.result.Text = res["sql"].ToString() + "<br>" + res["msg"].ToString();
        }
        else
        {
            this.result.Text = "OK";
        }



        //data


    }
}
