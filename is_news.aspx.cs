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


class NewsObj
{
    private mysql_db _mysql;
    private int _gKlinika;

    public int gKlinika
    {
        get { return _gKlinika; }
        set { _gKlinika = value; }
    }

    public mysql_db mysql
    {
        get { return _mysql; }
        set { _mysql = value; }
    }

} 

public partial class is_news : System.Web.UI.Page
{

    my_db x_db = new my_db();
    mysql_db x2Mysql = new mysql_db();
    // x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        int klinikaId = Convert.ToInt32(Session["klinika_id"].ToString());
       
        news_gv.DataSource = x_db.getData_News(klinikaId);
        news_gv.DataBind();
       

    }


    protected void saveMessage_Click(object sender, EventArgs e)
    {

        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
        byte[] text64Arr = UTF8.GetBytes(this.full_text.Text.ToString());

        string text64 = Convert.ToBase64String(text64Arr);

        SortedList data = new SortedList();
        if (Session["news_edit_id"] == null)
        {
            data.Add("kratka_sprava", small_text.Text.ToString());
            data.Add("cela_sprava", text64);
            data.Add("datum_txt", DateTime.Today.ToShortDateString());
            data.Add("user", Session["user_id"].ToString());
            data.Add("cielova_skupina", this.targetGroup_dl.SelectedValue.ToString());
            data.Add("klinika", Session["klinika_id"].ToString());

            SortedList res = x2Mysql.mysql_insert("is_news", data);

            if (Convert.ToBoolean(res["status"]))
            {
                Response.Redirect("is_news.aspx",false);
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }
        }
        else
        {
            data.Add("kratka_sprava", this.small_text.Text.ToString());
            data.Add("cela_sprava", text64);
            data.Add("cielova_skupina", this.targetGroup_dl.SelectedValue.ToString());

            SortedList res = x2Mysql.mysql_update("is_news", data, Session["news_edit_id"].ToString());

            if (Convert.ToBoolean(res["status"]))
            {
               Session.Remove("news_edit_id");
               Response.Redirect("is_news.aspx",false);
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
            Response.Redirect("is_news.aspx",false);
        }
        else
        {
            msg_lbl.Text = result;
        }

    }

    protected void news_gv_selectRow(object sender, GridViewSelectEventArgs e)
    {
        SortedList data = x_db.getInfoNewsData(Convert.ToInt32(news_gv.Rows[e.NewSelectedIndex].Cells[1].Text.ToString()));
        string text = "";
        if (data["status"] == null)
        {
            this.small_text.Text = data["kratka_sprava"].ToString();

            try
            {
                byte[] tmpArr = Convert.FromBase64String(data["cela_sprava"].ToString());
                text = System.Text.Encoding.UTF8.GetString(tmpArr);

            }
            catch (Exception error)
            {
                text = data["kratka_sprava"].ToString();
            }
            this.full_text.Text = text;
            Session["news_edit_id"] = data["id"];
        }
    }

   
}
