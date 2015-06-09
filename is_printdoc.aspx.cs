using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_printdoc : System.Web.UI.Page
{
    mysql_db x2Mysql = new mysql_db();
    x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        string what = Request.QueryString["w"].ToString();
        int id = Convert.ToInt32(Request.QueryString["id"]);

        this.loadData(what,id);
    }


    protected void loadData(string what, int id)
    {
        switch (what)
        {
            case "op":
                this.loadOperacnyProgram(id);
                break;

        }

        
    }

    protected void loadOperacnyProgram(int id)
    {
        string query = x2.sprintf("SELECT * FROM [is_opprogram] WHERE [id]={0}", new string[] { id.ToString() });
        SortedList data = x2Mysql.getRow(query);

        this.titel_lbl.Text = data["kratka_sprava"].ToString();

        this.message_lit.Text = x2.DecryptString(data["cela_sprava"].ToString(), Session["passphrase"].ToString());
        
    }
}