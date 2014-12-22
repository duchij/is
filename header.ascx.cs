using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
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
    public mysql_db x2Mysql = new mysql_db();
    sluzbyclass mySluz = new sluzbyclass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            if (Session["tuisegumdrum"] == null)
            {
                Response.Redirect("error.html");
            }
        }
        string userRights = Session["rights"].ToString();


        DateTime dnes = DateTime.Today;
        DateTime vcera = DateTime.Today.AddDays(-1);

        string dnesStr = dnes.ToShortDateString();
        string vceraStr = vcera.ToShortDateString();

        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
        sb.Append("[t_sluzb].[state] AS [state],");
        sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
        sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
        sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
        sb.Append("[t_sluzb].[date_group] AS [dategroup]");
        sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
        sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
        sb.AppendFormat("WHERE [t_sluzb].[datum]='{0}' OR [t_sluzb].[datum]='{1}'", my_x2.unixDate(vcera), my_x2.unixDate(dnes));
        sb.Append("GROUP BY [t_sluzb].[datum]");
        sb.Append("ORDER BY [t_sluzb].[datum] DESC");

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());


        if (table.Count > 0)
        {
            string[] docBefore = table[0]["users_names"].ToString().Split(';');
            string[] comments = table[0]["comment"].ToString().Split('|');
           // string[] docAfter = table[1]["users_names"].ToString().Split(';');

            this.oup_lbl.Text = docBefore[0].ToString()+"<br>"+comments[0].ToString();
            this.odda_lbl.Text = docBefore[1].ToString() + "<br>" + comments[1].ToString();
            this.oddb_lbl.Text = docBefore[2].ToString() + "<br>" + comments[2].ToString();
            this.op_lbl.Text = docBefore[3].ToString() + "<br>" + comments[3].ToString();
            this.trp_lbl.Text = docBefore[4].ToString() + "<br>" + comments[4].ToString();

            this.po_lbl.Text = table[1]["users_names"].ToString();

            date_lbl.Text = DateTime.Today.ToLongDateString();

            SortedList data = x_db.getNextPozDatum(DateTime.Today);
            poziadav_lbl.Text = data["datum"].ToString();


        }



        //SortedList data = x_db.getNextPozDatum(DateTime.Today);
        //poziadav_lbl.Text = data["datum"].ToString();

      /*  if (userRights == "admin" || userRights == "poweruser")
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
        }*/

        //SortedList aktSluz = this.getDnesSluzby();

        
       
        
        //oup_lbl.Text = aktSluz["OUP"].ToString();
        //odda_lbl.Text = aktSluz["OddA"].ToString();
        //oddb_lbl.Text = aktSluz["OddB"].ToString();
        //op_lbl.Text = aktSluz["OP"].ToString();
        //trp_lbl.Text = aktSluz["TRP"].ToString();

        //SortedList vcera = this.getVceraSluzby();
        //po_lbl.Text = vcera["OUP"] + ", " + vcera["OddA"] + ", " + vcera["OddB"] + ", " + vcera["OP"] + ", ";

        //date_lbl.Text = DateTime.Today.ToLongDateString();

        

    }

    //protected SortedList getDnesSluzby()
    //{
    //    SortedList result = new SortedList();
    //    DateTime dnesJe = DateTime.Now;

    //    SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", dnesJe.Month.ToString(), dnesJe.Year.ToString());

    //    SortedList docList = mySluz.getDoctorsForVykaz();

    //     string [][] data;
    //     int den = dnesJe.Day;

    //     if (tmp["ziadna_sluzba"] != "true")
    //     {
    //         DateTime pDate = Convert.ToDateTime("01.07.2012");

    //         DateTime oDate = Convert.ToDateTime(dnesJe.Day.ToString()+"." + dnesJe.Month.ToString() + "." + dnesJe.Year.ToString());

    //         data = my_x2.parseSluzba(tmp["rozpis"].ToString());
			 
    //         if (oDate > pDate)
    //         {

    //             string _mm = data[den - 1][1].ToString();

    //             char[] del = { '|' };

    //             string[] oup = _mm.Split(del);

    //             result.Add("OUP", docList[oup[0]]+oup[1]);
    //             result.Add("OddA", docList[data[den - 1][2].ToString()]);
    //             result.Add("OddB", docList[data[den - 1][3].ToString()]);
    //             result.Add("OP", docList[data[den - 1][4].ToString()]);
    //             result.Add("TRP", docList[data[den - 1][5].ToString()]);
    //         }
    //         else
    //         {
    //             result.Add("OUP", data[den - 1][1].ToString());
    //             result.Add("OddA", data[den - 1][2].ToString());
    //             result.Add("OddB", data[den - 1][3].ToString());
    //             result.Add("OP", data[den - 1][4].ToString());
    //             result.Add("TRP", data[den - 1][5].ToString());
    //         }
    //     }
    //     else
    //     {

    //         result.Add("OUP", '-');
    //         result.Add("OddA", '-');
    //         result.Add("OddB", '-');
    //         result.Add("OP", '-');
    //         result.Add("TRP", '-');
    //     }
      

        

    //    return result;

    //}

    //protected SortedList getVceraSluzby()
    //{
    //    SortedList result = new SortedList();
    //    DateTime dnesJe = DateTime.Today;

    //    int my_den = dnesJe.Day;
    //    int my_month = dnesJe.Month;
    //    int my_year = dnesJe.Year;

    //DateTime pDate = Convert.ToDateTime("01.07.2012");

    //    DateTime oDate = Convert.ToDateTime(dnesJe.Day.ToString()+"." + dnesJe.Month.ToString() + "." + dnesJe.Year.ToString());

    //    if (dnesJe.Day == 1)
    //    {


    //        if (dnesJe.Month == 1)
    //        {
    //            my_month = 12;
    //            my_year = dnesJe.Year - 1;

    //        }
    //        else
    //        {
    //            my_month = dnesJe.Month - 1;
    //            my_year = dnesJe.Year;
    //        }

    //    }


    //    SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", my_month.ToString(), my_year.ToString());

    //    string[][] data;

    //    SortedList docList = mySluz.getDoctorsForVykaz();

    //    if (tmp["ziadna_sluzba"] != "true")
    //    {

    //        data = my_x2.parseSluzba(tmp["rozpis"].ToString());

    //        int last_day = data.Length;

    //        if (dnesJe.Day == 1)
    //        {
    //            my_den = last_day;
    //        }
    //        else
    //        {
    //            my_den = my_den - 1;
    //        }

    //        if (oDate > pDate)
    //        {

    //            string _mm = data[my_den - 1][1].ToString();

    //            char[] del = { '|' };

    //            string[] oup = _mm.Split(del);

    //            result.Add("OUP", docList[oup[0]]+oup[1]);
    //            result.Add("OddA", docList[data[my_den - 1][2].ToString()]);
    //            result.Add("OddB", docList[data[my_den - 1][3].ToString()]);
    //            result.Add("OP", docList[data[my_den - 1][4].ToString()]);
    //            result.Add("TRP", docList[data[my_den - 1][5].ToString()]);
    //        }
    //        else
    //        {
    //            result.Add("OUP", data[my_den - 1][1].ToString());
    //            result.Add("OddA", data[my_den - 1][2].ToString());
    //            result.Add("OddB", data[my_den - 1][3].ToString());
    //            result.Add("OP", data[my_den - 1][4].ToString());
    //            result.Add("TRP", data[my_den - 1][5].ToString());
    //        }
    //    }
    //    else
    //    {
    //        result.Add("OUP", '-');
    //        result.Add("OddA", '-');
    //        result.Add("OddB", '-');
    //        result.Add("OP", '-');
    //        result.Add("TRP", '-');
    //    }




    //    return result;
    //}

   

}
