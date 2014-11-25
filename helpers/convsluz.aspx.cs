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
        int rok = Convert.ToInt32(this.rok_txt.Text.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_txt.ToString());


        SortedList data_info = x_db.loadSluzbaMonthYear("is_sluzby", this.mesiac_txt.Text.ToString(), this.rok_txt.Text.ToString());

        String[][] data = x2.parseSluzba(data_info["rozpis"].ToString());

        int rows = data.Length;
        int cols = data[0].Length;

        ArrayList tmpValues = new ArrayList();
        StringBuilder sb = new StringBuilder();
        StringBuilder sbDatum = new StringBuilder();

        for (int den = 0; den < rows; den++)
        {
            sbDatum.AppendFormat("{0}-{1}-{2}",rok,mesiac,den+1);
            string typ = "";
            string userId = "";
            for (int one=0; one<cols; one++)
            {
                if (one == 1)
                {
                    typ="OUP";
                    string[] tmp= data[den][one].Split('|');
                    userId = tmp[0].ToString();

                }
                if (one == 2)
                {
                    typ="OddA";
                    userId = tmpValues[one];
                }
                sb.AppendFormat("([{0}],[{1}],[{2}],[{3}])",sbDatum.ToString(),typ,userId,"");
        }



    }
}
