using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class passch2 : System.Web.UI.Page
{
    x2_var my_x2 = new x2_var();
    my_db x_db = new my_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

       
    }

    protected void change_btn_Click(object sender, EventArgs e)
    {
        string my_passwd1 = this.passwd1.Text.ToString();
        string my_passwd2 = this.passwd2.Text.ToString();

        if ((my_passwd1 != my_passwd2) || (my_passwd1.Length == 0) || (my_passwd2.Length == 0))
        {
            this.info_lbl.Visible = true;
            this.info_lbl.Text = "Hesla nie sú rovnaké, alebo heslo nezadané !!!!!";
        }
        else
        {
            SortedList data = new SortedList();
            //data.Add("id",Request.Cookies["user_id"].Value.ToString());
            data.Add("passwd", my_x2.make_hash(my_passwd1));
            this.info_lbl.Visible = true;
            string res = x_db.update_row("is_users", data, Session["user_id"].ToString());
            string wgroup = Session["workgroup"].ToString();

            if (res == "ok")
            {
                if (wgroup == "doctor")
                {
                    this.info_lbl.Text = "Heslo bolo úspešne zmenené !!! <a href='hlasko.aspx' target='_self'> Začať pracovať</a>";
                }
                else
                {
                    this.info_lbl.Text = "Heslo bolo úspešne zmenené !!! <a href='sestrhlas.aspx' target='_self'> Začať pracovať</a>";
                }

                this.changePassw.Visible = false;
            }
            else
            {
                this.info_lbl.Text = "Chyba:" + res;
            }

        }

    }

}