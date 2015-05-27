using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Text.RegularExpressions;




public partial class sluzby3: System.Web.UI.Page
{
    //my_db x_db = new my_db();
    //x2_var my_x2 = new x2_var();
    //mysql_db x2MySQL = new mysql_db();
    public string gKlinika;

    // protected System.Web.UI.HtmlControls.HtmlGenericControl hlavicka;
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.gKlinika = Session["klinika"].ToString().ToLower();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        switch (this.gKlinika)
        {
            case "kdch":
                ctpl.Controls.Add(Page.LoadControl("~/controls/shifts/kdch_shifts.ascx"));
                break;
            //case "2dk":
            //    ctpl.Controls.Add(Page.LoadControl("~/controls/druhadk_hlasko.ascx"));
            //    break;
        }

       
    }

    


    



}
