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
    public x2_var X2 = new x2_var();
    public my_db oldDb = new my_db();
    public string rights;
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

           
        this.showInfoMessage();
        this.current_user_lbl.Text = Session["fullname"].ToString();
        this.rights = Session["rights"].ToString();

        if (this.rights.IndexOf("sestra") != -1)
        {
            this.hlas_lekar_plh.Visible = false;
            this.hlas_sestier_plh.Visible = true;
        }

        if (this.rights.IndexOf("users") != -1)
        {
            this.hlas_lekar_plh.Visible = true;
            this.hlas_sestier_plh.Visible = false;
        }

        if (this.rights == "admin")
        {
            this.hlas_lekar_plh.Visible = true;
            this.hlas_sestier_plh.Visible = true;
        }



    }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.info_plh.Visible = false;
    }



    protected void log_out_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        //Session.Clear();
       // Response.Cookies["tuisegumdrum"].Expires = DateTime.Now.AddDays(-1);
        //Response.Cookies.Clear();
        Response.Redirect("Default.aspx");
    }

    protected void showInfoMessage()
    {
        if (Session["newsToShow"] != null)
        {
            this.info_plh.Visible = true; 
            int id = Convert.ToInt32(Session["newsToShow"].ToString());
            this.info_message_lbl.Text = oldDb.getNewsByID(id);
            Session.Remove("newsToShow");


        }
    }
}
