using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_opkniha_gal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        if (Request.QueryString["galId"] != null)
        {
            this.loadPicture(Request.QueryString["galId"]);
        }


    }


    private void loadPicture(string id)
    {
        this.galPicture.ImageUrl = "lf_view.ashx?galId=" + id;
    }
}