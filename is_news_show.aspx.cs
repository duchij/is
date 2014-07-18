using System;
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
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.Cookies["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Request.Cookies["user_id"].Value.ToString());
        user.Text = akt_user_info["full_name"].ToString();


        int id = Convert.ToInt32(Request.QueryString["id"].ToString());

        cela_sprava.Text = x_db.getNewsByID(id);
    }
}
