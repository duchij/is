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
       

        string rights = Session["rights"].ToString();
        string wgroup = Session["workgroup"].ToString();

        if (rights == "admin")
        {
                
            this.dev_pl.Visible = true;
            this.operacky.Visible = true;
            this.doctors.Visible = true;
            this.admin.Visible = true;
            this.sestra.Visible = true;
        }

        if (wgroup == "op")
        {
            this.operacky.Visible = true;
        }
       

        if (wgroup == "doctor" && rights!="admin")
        {
            this.doctors.Visible = true;
            this.admin.Visible = false;
            this.sestra.Visible = false;
        }
        
        if (wgroup == "nurse") 
        {
            this.doctors.Visible = false;
            this.admin.Visible = false;
            this.sestra.Visible = true;
        }

        

        

    }
}
