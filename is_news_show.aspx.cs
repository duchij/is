using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class is_news_show : System.Web.UI.Page
{
    my_db x_db = new my_db();
    mysql_db x2Mysql = new mysql_db();
    x2_var X2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
        string query = @"SELECT [cela_sprava],[cielova_skupina] FROM [is_news] WHERE [id]={0}";

        query = x2Mysql.buildSql(query, new string[] { id.ToString() });

        SortedList row = x2Mysql.getRow(query);

        if (row["status"]==null)
        {
            this.cela_sprava.Text = X2.stringFrom64(row["cela_sprava"].ToString());
        }
        else
        {
            X2.errorMessage2(ref this.msg_lbl, row["msg"].ToString());
        }
        
    }
}
