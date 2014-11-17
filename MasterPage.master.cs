using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
   /* public string PageName
    {
       get
        {
            return lblpageName.Text;
        }
        set
        {
            lblpageName.Text = value;
        }
    }*/
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }


    protected void log_out_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        //Session.Clear();
       // Response.Cookies["tuisegumdrum"].Expires = DateTime.Now.AddDays(-1);
        //Response.Cookies.Clear();
        Response.Redirect("Default.aspx");
    }
}
