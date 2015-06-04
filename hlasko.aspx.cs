using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;




public partial class hlasko : System.Web.UI.Page
{
    //my_db x_db = new my_db();
    //x2_var my_x2 = new x2_var();
    //mysql_db x2MySQL = new mysql_db();
    public string gKlinika;

    // protected System.Web.UI.HtmlControls.HtmlGenericControl hlavicka;
    protected void Page_Init(object sender, EventArgs e)
    {
       

        //if (this.gKlinika == "kdch")
        //{
        //    this.kdch_pl.Visible = true;
        //}

        //if (this.gKlinika == "2dk")
        //{
        //    this.druhaDK_pl.Visible = true;
        //}
        //hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript' src='tinymce/jscripts/tiny_mce/tiny_mce.js'></script>"));
        // hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript'>tinyMCE.init({mode : 'textareas',        force_br_newlines : true,        force_p_newlines : false});</script>"));
    }


    protected void Page_Load(object sender, EventArgs e)
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
                 ctpl.Controls.Add(Page.LoadControl("~/controls/kdch_hlasko.ascx")); 
                break;
            case "2dk":
                ctpl.Controls.Add(Page.LoadControl("~/controls/druhadk_hlasko.ascx")); 
                break;
            case "nkim":
                ctpl.Controls.Add(Page.LoadControl("~/controls/nkim_hlasko.ascx"));
                break;
        }
    }
        
    

}
