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

public partial class dov_stat : System.Web.UI.Page
{
    my_db x_db = new my_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

       // this.drawTable();



    }

    

}
