using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class helpers_mapPath : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.mapPath_lbl.Text = ""+ System.Web.HttpContext.Current.Server.MapPath("")+"<br>";
        this.mapPath_lbl.Text += @"/" + System.Web.HttpContext.Current.Server.MapPath("/") + "<br>";
        this.mapPath_lbl.Text += "." + System.Web.HttpContext.Current.Server.MapPath(".") + "<br>";
        this.mapPath_lbl.Text += "~" + System.Web.HttpContext.Current.Server.MapPath("~") + "<br>";
        this.mapPath_lbl.Text += ".." + System.Web.HttpContext.Current.Server.MapPath("..") + "<br>";
    }
}