using System;
using System.IO;
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

public partial class opprogram : System.Web.UI.Page
{

    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();
    mysql_db x2Mysql = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        if (!IsPostBack)
        {
            news_gv.DataSource = x_db.getData_OpProgram();
            news_gv.DataBind();
        }
        else
        {
            news_gv.DataSource = x_db.getData_OpProgram();
            news_gv.DataBind();
        }
    }

    public void saveFile(string id)
    {
        SortedList data = x2Mysql.getRow("SELECT * FROM [is_opprogram] WHERE [id]='" + id + "'");

        string str = data["kratka_sprava"].ToString() + "\r\n";
        str +=data["cela_sprava"].ToString()+"\r\n";
        str += data["datum_txt"].ToString() + "\r\n";
        str += data["user"].ToString() + "\r\n";
        str += data["datum"].ToString() + "\r\n";

        byte[] datato64 = Encoding.UTF8.GetBytes(str);
        string txt64 = Convert.ToBase64String(datato64);

        StreamWriter sw = new StreamWriter(Session["serverUrl"].ToString() + @"\img\op.txt",false);
        sw.Write(txt64);
        sw.Close();
    }


    protected void saveMessage_Click(object sender, EventArgs e)
    {
        int id = 0;

        SortedList passPhrase = x_db.getPassPhrase();

        if (news_gv.SelectedIndex != -1)
        {
            id = Convert.ToInt32(news_gv.Rows[news_gv.SelectedIndex].Cells[1].Text.ToString());
        }

        SortedList data = new SortedList();
        if (id <= 0)
        {
            string _op = my_x2.EncryptString(full_text.Text.ToString(), passPhrase["data"].ToString());
            DateTime __dnes = DateTime.Now;
            data.Add("kratka_sprava", small_text.Text.ToString());
            data.Add("cela_sprava", _op);
            data.Add("datum_txt", DateTime.Today.ToShortDateString());
            data.Add("user", Session["user_id"].ToString());
            data.Add("datum", my_x2.unixDate(new DateTime(__dnes.Year, __dnes.Month, __dnes.Day)));
            data = x_db.saveOpProgram(data);

            if (data["status"].ToString() == "ok")
            {
                this.saveFile(data["last_id"].ToString());
                Response.Redirect("opprogram.aspx");
            }
            else
            {
                msg_lbl.Text = data["message"].ToString();
            }
        }
        else
        {
            string _op = my_x2.EncryptString(full_text.Text.ToString(), passPhrase["data"].ToString());
            data.Add("kratka_sprava", small_text.Text.ToString());
            data.Add("cela_sprava", _op);

            x_db.update_row("is_opprogram", data, id.ToString());
            this.saveFile(id.ToString());
            Response.Redirect("opprogram.aspx");
        }
    }

    protected void news_gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string result = x_db.DeleteOpProgramRow(Convert.ToInt32(news_gv.Rows[e.RowIndex].Cells[1].Text.ToString()));

        if (result == "ok")
        {
            Response.Redirect("opprogram.aspx");
        }
        else
        {
            msg_lbl.Text = result;
        }
    }

    protected void news_gv_selectRow(object sender, GridViewSelectEventArgs e)
    {
        SortedList passPhrase = x_db.getPassPhrase();
        SortedList data = x_db.getInfoOpProgramData(Convert.ToInt32(news_gv.Rows[e.NewSelectedIndex].Cells[1].Text.ToString()));

        if (data["status"] == null)
        {
            string _op = my_x2.DecryptString(data["cela_sprava"].ToString(), passPhrase["data"].ToString());

            this.small_text.Text = data["kratka_sprava"].ToString();
            this.full_text.Text = _op;
        }
    }

    protected void news_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        news_gv.PageIndex = e.NewPageIndex;
        news_gv.DataBind();
    }

    

}
