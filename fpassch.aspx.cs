﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fpassch : System.Web.UI.Page
{
    x2_var my_x2 = new x2_var();
    my_db x_db = new my_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["sid"] == null)
        {
            Response.Redirect("error.html");
        }
        if (Session["force_change"] == null)
        {
            Response.Redirect("error.html");
        }

    }

  

}