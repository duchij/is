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

public partial class left_menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        string rights = Request.Cookies["rights"].Value.ToString();

        if (rights.IndexOf("users") != -1)

        {
            users.Visible = true;
            admin.Visible = false;
            sestra.Visible = false;
        }
        if (rights == "admin")
        {
            users.Visible = true;
            admin.Visible = true;
            sestra.Visible = true;
            
        }
        if (rights.IndexOf("sestra") != -1)
        {
            users.Visible = false;
            admin.Visible = false;
            sestra.Visible = true;
        }

        

        

    }
}
