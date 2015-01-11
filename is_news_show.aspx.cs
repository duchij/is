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
    //string id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Request.Cookies["user_id"].Value.ToString());
        user.Text = akt_user_info["full_name"].ToString();
        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [cela_sprava],[cielova-skupina] FROM [is_news] WHERE [id]={0}", id);

        SortedList row = x2Mysql.getRow(sb.ToString());
        if (row.Count > 0 )
        {
            this.cela_sprava.Text = row["cela_sprava"].ToString();
        }
    }
}
