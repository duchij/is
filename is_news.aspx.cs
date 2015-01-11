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
    mysql_db x2Mysql = new mysql_db();
    // x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        // SortedList akt_user_info = x_db.getUserInfoByID("is_users", Request.Cookies["user_id"].Value.ToString());
        //  user.Text = akt_user_info["full_name"].ToString();

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        // full_text.ImageGalleryPath = "img/";


        if (!IsPostBack)
        {
            news_gv.DataSource = x_db.getData_News();
            news_gv.DataBind();
        }
        else
        {
            news_gv.DataSource = x_db.getData_News();
            news_gv.DataBind();
        }

    }


    protected void saveMessage_Click(object sender, EventArgs e)
    {
        

        SortedList data = new SortedList();
        if (Session["news_edit_id"] == null)
        {
            data.Add("kratka_sprava", small_text.Text.ToString());
            data.Add("cela_sprava", full_text.Text.ToString());
            data.Add("datum_txt", DateTime.Today.ToShortDateString());
            data.Add("user", Session["user_id"].ToString());
            data.Add("cielova-skupina", this.targetGroup_dl.SelectedValue.ToString());

            SortedList res = x2Mysql.mysql_insert("is_news", data);

            if (Convert.ToBoolean(res["status"]))
            {
                Response.Redirect("is_news.aspx");
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }
        }
        else
        {
            data.Add("kratka_sprava", this.small_text.Text.ToString());
            data.Add("cela_sprava", this.full_text.Text.ToString());
            data.Add("cielova-skupina", this.targetGroup_dl.SelectedValue.ToString());

            SortedList res = x2Mysql.mysql_update("is_news", data, Session["news_edit_id"].ToString());
            if (Convert.ToBoolean(res["status"]))
            {
               Session.Remove("news_edit_id");
               Response.Redirect("is_news.aspx");
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }
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
            Session["news_edit_id"] = data["id"];

        }

    }

}
