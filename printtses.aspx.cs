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

public partial class hlasenia_print : System.Web.UI.Page
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {

            Response.Redirect("error.html");
        }

        datum_lbl.Text = Request["datum"].ToString();
        msg_lbl.Visible = false;
        my_db x_db = new my_db();

        SortedList data = x_db.getDataByID("is_hlasko_sestry", Session["akt_hlasenie"].ToString());
        SortedList user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (data.Count > 1)
        {
            string odd = "";
            string lok = "";
            if (data["oddelenie"].ToString() == "vd")
            {
                odd = "Veľkém deti";
            }
            if (data["oddelenie"].ToString() == "msv")
            {
                odd = "MSV";
            }
            if (data["oddelenie"].ToString() == "koj")
            {
                odd = "Kojenci";
            }

            if (data["lokalita"].ToString() == "pred")
            {
                lok = "Predné hlásenie";
            }
            else
            {
                lok = "Zadné hlásenie";
            }
            odd_lbl.Text = odd + ", " + lok;
            //type_lbl.Text = data["type"].ToString();
            hlas_lbl.Text = data["hlasko"].ToString();
            
        }
           

        user_lbl.Text = user_info["full_name"].ToString();
        SortedList lekari = this.getSluzbyByDen(Convert.ToInt32(Request["den"].ToString()));
        
        oup_lbl.Text = lekari["OUP"].ToString();
        odda_lbl.Text = lekari["OddA"].ToString();
        oddb_lbl.Text = lekari["OddB"].ToString();
        op_lbl.Text = lekari["OP"].ToString();
      
    }

    protected SortedList getSluzbyByDen(int xden)
    {
        SortedList result = new SortedList();
        DateTime dnesJe = DateTime.Today;
        int den = Convert.ToInt32(Request["den"].ToString());
        int mesiac = Convert.ToInt32(Request["m"].ToString());

        SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", mesiac.ToString(), dnesJe.Year.ToString());

        

        //SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", dnesJe.Month.ToString(), dnesJe.Year.ToString());

        string[][] data = my_x2.parseSluzba(tmp["rozpis"].ToString());

       // int den = dnesJe.Day;

        result.Add("OUP", data[den - 1][1].ToString());
        result.Add("OddA", data[den - 1][2].ToString());
        result.Add("OddB", data[den - 1][3].ToString());
        result.Add("OP", data[den - 1][4].ToString());
       // result.Add("TRP", data[den - 1][5].ToString());

        return result;

    }

}
