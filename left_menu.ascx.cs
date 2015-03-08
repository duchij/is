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
    public EventHandler setStateButton;

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
        if (rights == "sadmin")
        {
            this.doctors.Visible = true;
            this.admin.Visible = true;
            this.sestra.Visible = true;
            this.sadmin_menu.Visible = true;
        }
       

        if (wgroup == "op")
        {
            this.operacky.Visible = true;
        }
        else if ((wgroup == "doctor" || wgroup=="other") && (rights.IndexOf("admin")==-1))
        {
            this.doctors.Visible = true;
            this.admin.Visible = false;
            this.sestra.Visible = false;
        }
        else if (wgroup == "nurse" || wgroup =="assistent") 
        {
            this.doctors.Visible = false;
            this.admin.Visible = false;
            this.sestra.Visible = true;
        }
    }

    protected void setState(object sender, EventArgs e)
    {
        if (setStateButton!=null)
        {
            setStateButton(sender, e);
        }
       
        
        //this.offline_btn.Click += new EventHandler(duchmaster.webState_click);
    }
}
