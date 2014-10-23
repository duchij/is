using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class poziadavky : System.Web.UI.Page
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {

       
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        SortedList data = x_db.getNextPozDatum(DateTime.Today);

        if (Request.QueryString["a"] != null)
        {

           Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('"+Resources.Resource.odd_poziadavky_alert+"');", true);
            
        }



        string userRights = Session["rights"].ToString();

        if (userRights == "admin")
        {
            admin_section.Visible = true;
            user_section.Visible = false;
        }
        if (userRights.IndexOf("users") != -1)
        {
            admin_section.Visible = false;
            user_section.Visible = true;
        }
        if (userRights.IndexOf("power") != -1)
        {
            admin_section.Visible = true;
            user_section.Visible = true;
        }


        

        poziadav_lbl.Text = data["datum"].ToString();
        poziadav2_lbl.Text = data["datum"].ToString();

        if ((DateTime.Today > Convert.ToDateTime(data["datum"].ToString())) && (mesiac_cb.SelectedValue == DateTime.Today.AddMonths(1).Month.ToString()))
        {
            poziadavky_txt.ReadOnly = true;
        }
        else
        {
            poziadavky_txt.ReadOnly = false;
        }

        
        //this.lock_date.SelectedDate = Convert.ToDateTime(data["datum"].ToString());
      // this.lock_date.SelectMonthText = DateTime.Today.AddMonths(1).ToString();

        //this.lock_date.SelectedDate = DateTime.Today.AddMonths(1);

        if (!IsPostBack)
        {
            //this.lock_date.SelectMonthText = DateTime.Today.AddMonths(1).ToString();

            //this.lock_date.SelectedDate = DateTime.Today.AddMonths(1);
            this.mesiac_cb.SelectedValue = Convert.ToString(DateTime.Today.AddMonths(1).Month);
            
            //this.msg_lbl.Text = this.mesiac_cb.SelectedIndex.ToString();
            
            
            int mes_tmp = DateTime.Today.Month;
            if (mes_tmp == 12)
            {
                this.rok_cb.SelectedValue = Convert.ToString(DateTime.Today.AddYears(1).Year);

            }
            else
            {
                this.rok_cb.SelectedValue = Convert.ToString(DateTime.Today.Year);
            }
                //this.msg_lbl.Text = this.mesiac_cb.SelectedIndex.ToString(); 
            


            SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());
            user.Text = akt_user_info["full_name"].ToString();
            this.loadPoziadavky();
            //this.lock_date.SelectMonthText = DateTime.Today.AddMonths(1).ToString();
        }
        else
        {
            
              //  this.lock_date.SelectedDate = Convert.ToDateTime(data["datum"].ToString());
                //this.lock_date.SelectedDate = DateTime.Today.AddMonths(+1);
        }




    }

    protected void loadPoziadavky()
    {
        SortedList data = x_db.getUserPoziadavky(Session["user_id"].ToString(), DateTime.Today);

        if (data["info"] != null)
        {
            this.poziadavky_txt.Text = data["info"].ToString();
        }

       // this.lock_date.SelectedDate = DateTime.Now.AddMonths(1);
       // this.lock_date.SelectMonthText = DateTime.Today.AddMonths(1).ToString();
    }

    protected void savePoziadavka_fnc_Click(object sender, EventArgs e)
    {
       
        SortedList data = new SortedList();
        DateTime datum = this.lock_date.SelectedDate;



        data.Add("mesiac", datum.Month.ToString());
        data.Add("rok", datum.Year.ToString());
        data.Add("datum",datum.ToShortDateString());

        msg_lbl.Text = datum.ToShortDateString();
        SortedList result =  x_db.savePoziadavky("is_poziadavky_data", data);
        //msg_lbl.Text = data["datum"].ToString();
        if (result["status"] != null)
        {
            if (result["status"].ToString() == "error")
            {
               msg_lbl.Text = result["message"].ToString();
            }
            
        }
        else
        {
           // msg_lbl.Text = result["datum"].ToString();
            poziadav_lbl.Text = result["datum"].ToString();
            poziadav2_lbl.Text = data["datum"].ToString();

            this.lock_date.SelectedDate = Convert.ToDateTime(result["datum"].ToString());
            //Session.Add("lock_date",
            Response.Redirect("poziadavky.aspx");
        }
    }


    protected void saveUserPoziadav_Click(object sender, EventArgs e)
    {
        SortedList data = new SortedList();
        data.Add("user_id",Session["user_id"].ToString());
        data.Add("info",poziadavky_txt.Text.ToString());
        data.Add("mesiac", mesiac_cb.SelectedValue.ToString());
        data.Add("rok", rok_cb.SelectedValue.ToString());

        SortedList result = x_db.saveUserPoziadavka(mesiac_cb.SelectedValue.ToString(), rok_cb.SelectedValue.ToString(), data);

        if (result["status"] != null)
        {
            if (result["status"].ToString() == "error")
            {
                msg_lbl.Text = result["message"].ToString();
            }

        }
        else
        {
            //msg_lbl.Text = result["info"].ToString();
        }
    }

    protected void getPoziadavky_fnc(object sender, EventArgs e)
    {
        SortedList result = x_db.getPoziadavky(mesiac_cb.SelectedValue.ToString(), rok_cb.SelectedValue.ToString(), Session["user_id"].ToString());
        if (result["status"] == null)
        {
            this.poziadavky_txt.Text = result["info"].ToString();
        }
        else
        {
            this.poziadavky_txt.Text = "";
        }
    }

    protected void printPoziadavka_fnc_Click(object sender, EventArgs e)
    {
        Session.Add("rozpis_rok", this.rok_cb.SelectedValue.ToString());
        Session.Add("rozpis_mesiac", this.mesiac_cb.SelectedValue.ToString());

        Response.Redirect(@"printpoz.aspx");

    }

}