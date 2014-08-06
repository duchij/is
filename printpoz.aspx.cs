using System;
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
using System.Text;

public partial class printpoz : System.Web.UI.Page
{
    my_db x_db = new my_db();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {

            Response.Redirect("error.html");
        }
        if (!IsPostBack)
        {

            this.mesiac_cb.SelectedValue = Session["rozpis_mesiac"].ToString();
            this.rok_cb.SelectedValue = DateTime.Today.Year.ToString();
            this.loadData();
        }
        else
        {
            this.loadSelData(this.mesiac_cb.SelectedValue.ToString(), this.rok_cb.SelectedValue.ToString());

        }


        
    }

    protected void loadSelData(string mesiac, string rok)
    {

        List<string> result = x_db.getAllPoziadavkySel(mesiac,rok);

        int year = Convert.ToInt32(rok);
        int month = Convert.ToInt32(mesiac);

        //if (DateTime.Today.Month == 12)
        //{
        //    year += 1;
        //}

        StringBuilder sb = new StringBuilder();

        sb.AppendFormat(" {0} / {1}", month, year);


        poziadavMes_lbl.Text = sb.ToString();
        int dlzka = result.Count;

        if (dlzka != 0)
        {
            for (int i = 0; i < dlzka; i++ )
            {
                string[] strTmp = result[i].Split('|');
                TableRow riadok = new TableRow();
                zoznam_tbl.Controls.Add(riadok);


                TableCell my_cell1 = new TableCell();
                my_cell1.BorderWidth = 1;
                my_cell1.Height = 30;
                my_cell1.Width = 200;
                my_cell1.VerticalAlign = VerticalAlign.Top;
                my_cell1.BorderColor = System.Drawing.Color.FromArgb(0x000000);
                my_cell1.Text = "<strong>" + strTmp[0] + "</strong>";
                riadok.Controls.Add(my_cell1);

                TableCell my_cell2 = new TableCell();
                my_cell2.BorderWidth = 1;
                my_cell2.BorderColor = System.Drawing.Color.FromArgb(0x000000);
                my_cell2.Width = 500;
                my_cell2.VerticalAlign = VerticalAlign.Top;
                my_cell2.Text = strTmp[1].ToString();
                riadok.Controls.Add(my_cell2);


            }

        }

    }
    
    protected void loadData()
    {
       List<string> result = x_db.getAllPoziadavky(DateTime.Today);

        int year = DateTime.Today.Year;
        int month = DateTime.Today.AddMonths(1).Month;

        if (DateTime.Today.Month == 12)
        {
            year +=1;
        }

        StringBuilder sb = new StringBuilder();
            
        sb.AppendFormat(" {0} / {1}",month,year);


        poziadavMes_lbl.Text = sb.ToString();
        int dlzka = result.Count;
        
        if (dlzka != 0)
        {
            for (int i = 0; i < dlzka; i++ )
            {
                string[] strTmp = result[i].Split('|');

                TableRow riadok = new TableRow();
                zoznam_tbl.Controls.Add(riadok);


                TableCell my_cell1 = new TableCell();
                my_cell1.BorderWidth = 1;
                my_cell1.Height = 30;
                my_cell1.Width = 200;
                my_cell1.VerticalAlign = VerticalAlign.Top;
                my_cell1.BorderColor = System.Drawing.Color.FromArgb(0x000000);
                my_cell1.Text = "<strong>" + strTmp[0] + "</strong>";
                riadok.Controls.Add(my_cell1);

                TableCell my_cell2 = new TableCell();
                my_cell2.BorderWidth = 1;
                my_cell2.BorderColor = System.Drawing.Color.FromArgb(0x000000);
                my_cell2.Width = 500;
                my_cell2.VerticalAlign = VerticalAlign.Top;
                my_cell2.Text = strTmp[1].ToString();
                riadok.Controls.Add(my_cell2);


            }

        }

    }

}
