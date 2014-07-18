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

public partial class header : System.Web.UI.UserControl
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();
    sluzbyclass mySluz = new sluzbyclass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        string userRights = Session["rights"].ToString();
       

        SortedList data = x_db.getNextPozDatum(DateTime.Today);
        poziadav_lbl.Text = data["datum"].ToString();

        if (userRights == "admin" || userRights == "poweruser")
        {
            this.news_link.NavigateUrl = "is_news.aspx";
        }
        else
        {
            this.news_link.NavigateUrl = "";
        }

        if (userRights.IndexOf("sestra") != -1)
        {
            this.odd_link.NavigateUrl = "sestrhlas.aspx";
        }
        else
        {
            this.odd_link.NavigateUrl = "hlasko.aspx";
        }

        if (userRights.IndexOf("users_op") != -1 || userRights == "admin")
        {
            this.opprogram_link.NavigateUrl = "opprogram.aspx";
        }
        else
        {
            this.opprogram_link.NavigateUrl = "";
        }

        SortedList aktSluz = this.getDnesSluzby();

        
       
        
        oup_lbl.Text = aktSluz["OUP"].ToString();
        odda_lbl.Text = aktSluz["OddA"].ToString();
        oddb_lbl.Text = aktSluz["OddB"].ToString();
        op_lbl.Text = aktSluz["OP"].ToString();
        trp_lbl.Text = aktSluz["TRP"].ToString();

        SortedList vcera = this.getVceraSluzby();
        po_lbl.Text = vcera["OUP"] + ", " + vcera["OddA"] + ", " + vcera["OddB"] + ", " + vcera["OP"] + ", ";

        date_lbl.Text = DateTime.Today.ToLongDateString();

        

    }

    protected SortedList getDnesSluzby()
    {
        SortedList result = new SortedList();
        DateTime dnesJe = DateTime.Now;

        SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", dnesJe.Month.ToString(), dnesJe.Year.ToString());

        SortedList docList = mySluz.getDoctorsForVykaz();

         string [][] data;
         int den = dnesJe.Day;

         if (tmp["ziadna_sluzba"] != "true")
         {
             DateTime pDate = Convert.ToDateTime("01.07.2012");

             DateTime oDate = Convert.ToDateTime(dnesJe.Day.ToString()+"." + dnesJe.Month.ToString() + "." + dnesJe.Year.ToString());

             data = my_x2.parseSluzba(tmp["rozpis"].ToString());
			 
             if (oDate > pDate)
             {

                 string _mm = data[den - 1][1].ToString();

                 char[] del = { '|' };

                 string[] oup = _mm.Split(del);

                 result.Add("OUP", docList[oup[0]]+oup[1]);
                 result.Add("OddA", docList[data[den - 1][2].ToString()]);
                 result.Add("OddB", docList[data[den - 1][3].ToString()]);
                 result.Add("OP", docList[data[den - 1][4].ToString()]);
                 result.Add("TRP", docList[data[den - 1][5].ToString()]);
             }
             else
             {
                 result.Add("OUP", data[den - 1][1].ToString());
                 result.Add("OddA", data[den - 1][2].ToString());
                 result.Add("OddB", data[den - 1][3].ToString());
                 result.Add("OP", data[den - 1][4].ToString());
                 result.Add("TRP", data[den - 1][5].ToString());
             }
         }
         else
         {

             result.Add("OUP", '-');
             result.Add("OddA", '-');
             result.Add("OddB", '-');
             result.Add("OP", '-');
             result.Add("TRP", '-');
         }
      

        

        return result;

    }

    protected SortedList getVceraSluzby()
    {
        SortedList result = new SortedList();
        DateTime dnesJe = DateTime.Today;

        int my_den = dnesJe.Day;
        int my_month = dnesJe.Month;
        int my_year = dnesJe.Year;

	DateTime pDate = Convert.ToDateTime("01.07.2012");

        DateTime oDate = Convert.ToDateTime(dnesJe.Day.ToString()+"." + dnesJe.Month.ToString() + "." + dnesJe.Year.ToString());

        if (dnesJe.Day == 1)
        {


            if (dnesJe.Month == 1)
            {
                my_month = 12;
                my_year = dnesJe.Year - 1;

            }
            else
            {
                my_month = dnesJe.Month - 1;
                my_year = dnesJe.Year;
            }

        }


        SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", my_month.ToString(), my_year.ToString());

        string[][] data;

        SortedList docList = mySluz.getDoctorsForVykaz();

        if (tmp["ziadna_sluzba"] != "true")
        {

            data = my_x2.parseSluzba(tmp["rozpis"].ToString());

            int last_day = data.Length;

            if (dnesJe.Day == 1)
            {
                my_den = last_day;
            }
            else
            {
                my_den = my_den - 1;
            }

            if (oDate > pDate)
            {

                string _mm = data[my_den - 1][1].ToString();

                char[] del = { '|' };

                string[] oup = _mm.Split(del);

                result.Add("OUP", docList[oup[0]]+oup[1]);
                result.Add("OddA", docList[data[my_den - 1][2].ToString()]);
                result.Add("OddB", docList[data[my_den - 1][3].ToString()]);
                result.Add("OP", docList[data[my_den - 1][4].ToString()]);
                result.Add("TRP", docList[data[my_den - 1][5].ToString()]);
            }
            else
            {
                result.Add("OUP", data[my_den - 1][1].ToString());
                result.Add("OddA", data[my_den - 1][2].ToString());
                result.Add("OddB", data[my_den - 1][3].ToString());
                result.Add("OP", data[my_den - 1][4].ToString());
                result.Add("TRP", data[my_den - 1][5].ToString());
            }
        }
        else
        {
            result.Add("OUP", '-');
            result.Add("OddA", '-');
            result.Add("OddB", '-');
            result.Add("OP", '-');
            result.Add("TRP", '-');
        }




        return result;
    }

    protected void log_out_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
       // Response.Cookies["tuisegumdrum"].Expires = DateTime.Now.AddDays(-1);
        //Response.Cookies.Clear();
        Response.Redirect("default.aspx");
    }

}
