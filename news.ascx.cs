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
       StringBuilder sb = new StringBuilder();
       int cntO = opprog.Count;
       if (cntO != 0)
       {
            for (int i=0; i<cntO; i++)
            {
                Label myLbl = new Label();
                myLbl.ID = "op_lbl_" + i.ToString();

                string[] strTmp = opprog[i].Split('|');

                if (i == 0)
                {
                    sb.AppendFormat("<p><a class='yellow button'  href='is_op_show.aspx?id={1}' target='_self'>{0}</a></p>", strTmp[1], Convert.ToInt32(strTmp[0]));
                }
                else
                {
                    sb.AppendFormat("<p><a class='button' href='is_op_show.aspx?id={1}' target='_self'> {0}</a></p>", strTmp[1], Convert.ToInt32(strTmp[0]));
                }

                myLbl.Text = sb.ToString();
                this.op_tbl.Controls.Add(myLbl);
                sb.Length = 0;
                
            }
        }
        
            
         List<String> data = x_db.getNews();
         int cnt = data.Count;
         sb.Length = 0;
        if (cnt > 0)
        {

            for (int i = 0; i < cnt; i++) 
            {
                Label nLbl = new Label();
                nLbl.ID = "news_lbl_" + i.ToString();
                

                string[] str = data[i].Split(new char[] { '|' });

                if (i == 0)
                {
                    sb.AppendFormat("<div class='box red'>{0}<a href='is_news_show.aspx?id={1}' target='_self'> <div class='white'> >>>> </div></a> </div>", str[1], str[0]);
                }
                else
                {
                    sb.AppendFormat("<div class='box grey'>{0}<a href='is_news_show.aspx?id={1}' target='_self'> <div class='white'>  >>>>> </div></a></div>", str[1], str[0]);
                }
                nLbl.Text = sb.ToString();
                this.news_plh.Controls.Add(nLbl);
                sb.Length = 0;
            }
        }


    }
}
