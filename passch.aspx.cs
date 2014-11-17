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

public partial class passch : System.Web.UI.Page
{
    x2_var my_x2 = new x2_var();
    my_db x_db = new my_db();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void change_btn_Click(object sender, EventArgs e)
    {
        string my_passwd1 = passwd1.Text.ToString();
        string my_passwd2 = passwd2.Text.ToString();

        if ((my_passwd1 != my_passwd2) || (my_passwd1.Length == 0) || (my_passwd2.Length == 0))
        {
            info_lbl.Visible = true;
            info_lbl.Text = "Hesla nie sú rovnaké, alebo heslo nezadané !!!!!";
        }
        else
        {
            SortedList data = new SortedList();
            //data.Add("id",Request.Cookies["user_id"].Value.ToString());
            data.Add("passwd", my_x2.make_hash(my_passwd1));
            info_lbl.Visible = true;
            string res = x_db.update_row("is_users", data, Request.Cookies["user_id"].Value.ToString());
            string prava = Request.Cookies["rights"].Value.ToString();

            if (res == "ok")
            {
                if (prava.IndexOf("sestra") == -1)
                {
                    info_lbl.Text = "Heslo bolo úspešne zmenené !!! <a href='hlasko.aspx' target='_self'> Začať pracovať</a>";
                }
                else
                {
                    info_lbl.Text = "Heslo bolo úspešne zmenené !!! <a href='sestrhlas.aspx' target='_self'> Začať pracovať</a>";
                }

                changePassw.Visible = false;
            }
            else
            {
                info_lbl.Text = "Chyba:" + res;
            }

        }

    }
}
