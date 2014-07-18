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

public partial class is_news : System.Web.UI.Page
{

    my_db x_db = new my_db();
   // x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
       // SortedList akt_user_info = x_db.getUserInfoByID("is_users", Request.Cookies["user_id"].Value.ToString());
      //  user.Text = akt_user_info["full_name"].ToString();

        if (Request.Cookies["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        full_text.ImageGalleryPath = "img/";
        

        if (!IsPostBack)
        {
            //SortedList akt_user_info = x_db.getUserInfoByID("is_users", Request.Cookies["user_id"].Value.ToString());
           // user.Text = akt_user_info["full_name"].ToString();

            //GridView1.Data

            news_gv.DataSource = x_db.getData_News();


            news_gv.DataBind();

            //opkniha.DataSource = x_db.getData_operacie();
            //opkniha.DataBind();
        }
        else
        {
            news_gv.DataSource = x_db.getData_News();
            news_gv.DataBind();
        }

    }


    protected void saveMessage_Click(object sender, EventArgs e)
    {
        int id = 0;

        if (news_gv.SelectedIndex != -1)
        {
            id = Convert.ToInt32(news_gv.Rows[news_gv.SelectedIndex].Cells[1].Text.ToString());
        }
        
        SortedList data = new SortedList();
        if (id <= 0)
        {

            

            data.Add("kratka_sprava", small_text.Text.ToString());
            data.Add("cela_sprava", full_text.Text.ToString());
            data.Add("datum_txt", DateTime.Today.ToShortDateString());
            data.Add("user", Request.Cookies["user_id"].Value.ToString());

            data = x_db.saveNews(data);

            if (data["status"].ToString() == "ok")
            {
                Response.Redirect("is_news.aspx");
            }
            else
            {
                msg_lbl.Text = data["message"].ToString();
            }
        }
        else
        {
            data.Add("kratka_sprava", small_text.Text.ToString());
            data.Add("cela_sprava", full_text.Text.ToString());

            x_db.update_row("is_news", data, id.ToString());
            Response.Redirect("is_news.aspx");
        }


    }

    protected void news_gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string result = x_db.DeleteNewsRow(Convert.ToInt32(news_gv.Rows[e.RowIndex].Cells[1].Text.ToString()));

        if (result == "ok")
        {
            Response.Redirect("is_news.aspx");
        }
        else
        {
            msg_lbl.Text = result;
        }
        //msg_lbl.Text = Convert.ToString(news_gv.SelectedRow.Cells[0].Text);
        
        //news_gv.DataBind();

    }

    protected void news_gv_selectRow(object sender, GridViewSelectEventArgs e)
    {
        //msg_lbl.Text = "tu";
       // msg_lbl.Text += news_gv.Rows[e.NewSelectedIndex].Cells[1].Text.ToString();

        SortedList data = x_db.getInfoNewsData(Convert.ToInt32(news_gv.Rows[e.NewSelectedIndex].Cells[1].Text.ToString()));

        if (data["status"] == null)
        {
            this.small_text.Text = data["kratka_sprava"].ToString();
            this.full_text.Text = data["cela_sprava"].ToString();
        }

    }

}
