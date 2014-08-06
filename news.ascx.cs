using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class news : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        this.loadData();
    }

    protected void loadData()
    {
        my_db x_db = new my_db();
        x2_var my_x2 = new x2_var();

       List<string> opprog = x_db.getOpProgram();

       int cntO = opprog.Count;
       if (cntO != 0)
       {
            for (int i=0; i<cntO; i++)
            {
                TableRow riadok = new TableRow();

                TableCell cela = new TableCell();
                cela.BorderWidth = 0;
                StringBuilder sb = new StringBuilder();

                string[] strTmp = opprog[i].Split('|');
                if (i == 0)
                {
                    sb.AppendFormat("<div style='background-color:#FACE0A;padding:3px;'<strong>{0}</strong> <a href='is_op_show.aspx?id={1}' target='_self'> >> </a><hr/></div>", strTmp[1], Convert.ToInt32(strTmp[0]));
                }
                else
                {
                    sb.AppendFormat("{0} <a href='is_op_show.aspx?id={1}' target='_self'> >> </a><hr/>", strTmp[1], Convert.ToInt32(strTmp[0]));
                }

                cela.Text = sb.ToString();
                riadok.Controls.Add(cela);
                op_table.Controls.Add(riadok);
            }
        }
        
            
         List<String> data = x_db.getNews();
         int cnt = data.Count;

        if (cnt > 0)
        {

            for (int i = 0; i < cnt; i++) 
            {
                TableRow riadok = new TableRow();

                TableCell cela = new TableCell();
                cela.BorderWidth = 0;
                StringBuilder sb = new StringBuilder();

                string[] str = data[i].Split(new char[] { '|' });

                if (i == 0)
                {
                    sb.AppendFormat("<div style='background-color:#FACE0A;padding:3px;'<strong>{0} <a href='is_news_show.aspx?id={1}' target='_self'> >> </a><hr/></div>", str[1], str[0]);
                }
                else
                {
                    sb.AppendFormat("{0} <a href='is_news_show.aspx?id={1}' target='_self'> >> </a><hr/>", str[1], str[0]);
                }
                cela.Text = sb.ToString();
                riadok.Controls.Add(cela);
                news_tbl.Controls.Add(riadok);
            }
        }


    }
}
