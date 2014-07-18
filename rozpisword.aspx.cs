using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class sltoword : System.Web.UI.Page
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        string rok = Request["rok"].ToString();
        string mesiac = Request["mesiac"].ToString();
        string mes = Request["mes"].ToString();
        mesiac_lbl.Text = Request["mes"].ToString();
        rok_lbl.Text = rok;
        this.drawTable(mesiac, rok);
        if (Request["print"] == null)
        {
            Response.Clear();
            Response.Buffer = true;

            Response.ContentType = "application/msword; charset=Windows-1250";

           
            Response.AddHeader("content-disposition", "attachment;filename=rozpis.doc");

            Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
           	Response.Charset = "Windows-1250";

            StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used

            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            this.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        else
        {
            print_lbl.Visible = true;
            print_lbl.Text ="<a href='' onClick='window.print();'>Tlacit</a>";
            back_lbl.Visible = true;
            back_lbl.Text = "<a href='rozpislekar.aspx' target='_self'>Naspat</a>";

        }






    }

    protected void drawTable(string mesiac, string rok)
    {
        SortedList data_info = x_db.loadRozpisMonthYear(Convert.ToInt32(mesiac), Convert.ToInt32(rok));
        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        //vypis_lbl.Text = data_info.Count.ToString();



        String[][] data = my_x2.parseRozpis(data_info["rozpis"].ToString());
        //ArrayList my_list = new ArrayList();





       // days_lbl.Text = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac)).ToString();

        for (int i = 0; i < days; i++)
        {
            TableRow riadok = new TableRow();

            Table1.Controls.Add(riadok);
            for (int j = 0; j < data[i].Length; j++)
            {
                TableCell my_cell = new TableCell();
                //TextBox my_text_box = new TextBox();


                my_cell.ID = "cellBox_" + i.ToString() + "_" + j.ToString();
                

                int den = i + 1;
                DateTime my_date = new DateTime(Convert.ToInt32(data_info["rok"].ToString()), Convert.ToInt32(data_info["mesiac"].ToString()), den);
                int dnesJe = (int)my_date.DayOfWeek;
                string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                // pokus.Text += nazov;

                if ((nazov == "sobota") || (nazov == "nedeľa"))
                {
                    my_cell.BackColor = System.Drawing.Color.FromArgb(0xcc3300);
                }
                else 
                {
                    my_cell.BackColor = System.Drawing.Color.White;
                }

                string[] freeDays = x_db.getFreeDays();

                string mesDen = den.ToString() + "." + mesiac;

                int rs_tmp = Array.IndexOf(freeDays, mesDen);

                if ((rs_tmp != -1) && (nazov != "sobota") && (nazov != "nedeľa"))
                {
                    my_cell.BackColor = System.Drawing.Color.FromArgb(0xcc3300);
                }

                my_cell.BorderColor = System.Drawing.Color.Black;
                my_cell.BorderWidth = 1;
                my_cell.VerticalAlign = VerticalAlign.Top;
                if (j == 0)
                {
                    my_cell.Width = 40;
                }
                else
                {
                    my_cell.Width = 130;
                }
               // my_cell.Height = 80;
                my_cell.Text = "<div style='font-family:Arial;font-size:12px;'>"+ data[i][j]+"</div>";

                
                
                //my_text_box.ReadOnly = true;
                

                //my_text_box.ReadOnly = true;
                //my_cell.Text = "textBox_" + i.ToString() + "_" + j.ToString();

                //my_cell.Controls.Add(my_text_box);

                //form1.Controls.Add(my_text_box);
                riadok.Controls.Add(my_cell);
            }


        }
    }
}
