using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class helpers_callsp : System.Web.UI.Page
{
    public mysql_db x2Mysql = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

       // if (Session["rights"].ToString() == "admin")

    }

    protected void call_fnc(object sender, EventArgs e)
    {
        SortedList result = x2Mysql.callStoredProcWithoutParam(this.stored_txt.Text.ToString());

        if (!Convert.ToBoolean(result["status"]))
        {
            this.msg_lbl.Text = result["msg"].ToString();
        }
        else
        {
            this.msg_lbl.Text = "OK";
        }

    }
}