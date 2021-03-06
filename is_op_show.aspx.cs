﻿using System;
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
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Request.Cookies["user_id"].Value.ToString());
        user.Text = akt_user_info["full_name"].ToString();


        int id = Convert.ToInt32(Request.QueryString["id"].ToString());

        SortedList op_text = x_db.getOpProgramByID(id);
        SortedList passPhrase = x_db.getPassPhrase();
        string _op = my_x2.DecryptString(op_text["full_text"].ToString(), passPhrase["data"].ToString());
        cela_sprava.Text = "<h2>" + op_text["short_text"].ToString() + "</h2><hr />";
        cela_sprava.Text += _op;
    }
}
