using System;
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

public partial class sluzby : System.Web.UI.Page
{
    x2_var my_x2 = new x2_var();
    my_db x_db = new my_db();
    //string tabulka = "";
    string user_rights= "";
    string u_name = "";
    
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {

            Response.Redirect("error.html");
            
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {

            Response.Redirect("error.html");
        }


       // this.drawTable();
        user_rights = Session["rights"].ToString();
        u_name = Session["login"].ToString();
 
        if ((user_rights == "admin") || (user_rights == "poweruser") )
        {
            Button1.Visible = true;
            Button2.Visible = true;
            CheckBox1.Visible = true;
            publish_ck.Visible = true;
            Button5.Visible = true;
            Button6.Visible = true;
        }
        else
        {
            Button1.Visible = false;
            Button2.Visible = false;
            CheckBox1.Visible = false;
            publish_ck.Visible = false;
            Button5.Visible = false;
            Button6.Visible = false;
        }

        
        if (IsPostBack == false)
        {
            DateTime akt_datum = DateTime.Today;
            this.rok_cb.SelectedValue = akt_datum.Year.ToString();
            this.mesiac_cb.SelectedValue = akt_datum.Month.ToString();
           //string mesiac = akt_datum.Month.ToString();
           //string rok = akt_datum.Year.ToString();
           //vypis_lbl.Text = rok;
           // this.drawStaze(mesiac, rok);

            string rok = rok_cb.SelectedValue.ToString();
            string mesiac = mesiac_cb.SelectedValue.ToString();
            this.drawRozpis(mesiac, rok);
        }
        else
        {

            string rok = rok_cb.SelectedValue.ToString();
            string mesiac = mesiac_cb.SelectedValue.ToString();
            //this.drawRozpis(mesiac, rok);
            this.drawTable(rok, mesiac);
            
           // Response.Cookies["mesiac"].Value = mesiac_cb.SelectedValue.ToString();
           // Response.Cookies["rok"].Value = rok_cb.SelectedValue.ToString();
            //vypis_lbl.Text = rok_cb.SelectedValue.ToString();
           // this.drawStaze(Request.Cookies["mesiac"].Value.ToString(), Request.Cookies["rok"].Value.ToString());
            //this.saveSluzby();
            //vypis_lbl.Text = Session.Count.ToString();
            
        }
    }

    
    private void drawTable(string rok, string mesiac)
    {

        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        days_lbl.Text = days.ToString();

        for (int i = 0; i < days; i++)
        {
            TableRow riadok = new TableRow();
            Table1.Controls.Add(riadok);
            for (int j = 0; j < 7; j++)
            {
                TableCell my_cell = new TableCell();
                my_cell.BorderWidth = 1;
                if (j == 0)
                {
                    my_cell.Width = 40;
                }
                else
                {
                    my_cell.Width = 130;
                }
                my_cell.Height = 60;
                my_cell.VerticalAlign = VerticalAlign.Top;
                my_cell.BorderColor = System.Drawing.Color.FromArgb(0x990000);
                TextBox my_text_box = new TextBox();
                my_text_box.AutoPostBack = true;
                my_text_box.BorderStyle = BorderStyle.None;
                int den = i + 1;
                DateTime my_date = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), den);
                int dnesJe = (int)my_date.DayOfWeek;
                string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                // pokus.Text += nazov;

                if ((nazov == "sobota") || (nazov == "nedeľa"))
                {
                    my_text_box.BackColor = System.Drawing.Color.FromArgb(0x990000);
                    my_text_box.ForeColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    my_text_box.BackColor = System.Drawing.Color.White;
                }

                string[] freeDays = x_db.getFreeDays();

                string mesDen = den.ToString() + "." + mesiac;

                int rs_tmp = Array.IndexOf(freeDays, mesDen);

                if ((rs_tmp != -1) && (nazov != "sobota") && (nazov != "nedeľa"))
                {
                    my_text_box.BackColor = System.Drawing.Color.Yellow;
                    my_text_box.ForeColor = System.Drawing.Color.FromArgb(0x990000);
                }

                //my_text_box.TextChanged += new System.EventHandler(this.my_text_box_TextChanged);
                my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();
                if (j == 0)
                {
                    my_text_box.Width = 40;
                }
                else
                {
                    my_text_box.Width = 100;
                }

                my_text_box.Height = 80;
                my_text_box.TextMode = TextBoxMode.MultiLine;
                my_text_box.Font.Size = 8;

                if ((user_rights == "admin") || (user_rights == "poweruser"))
                {
                    my_text_box.ReadOnly = false;
                }
                else
                {
                    my_text_box.ReadOnly = true;
                }

                my_cell.Controls.Add(my_text_box);

                //form1.Controls.Add(my_text_box);
                riadok.Controls.Add(my_cell);
            }

        }
    }

    protected void drawRozpisPrev(string mesiac, string rok)
    {

        SortedList data_info = x_db.loadRozpisMonthYear(Convert.ToInt32(mesiac), Convert.ToInt32(rok));
        
        DateTime dnesDatum = DateTime.Today;
        int days = DateTime.DaysInMonth(dnesDatum.Year, dnesDatum.Month);

        if (data_info["id"] != null)
        {
            String[][] data = my_x2.parseRozpis(data_info["rozpis"].ToString());
           
            for (int i = 0; i < days; i++)
            {
                string[] row_tmp = new string[7];

                if ((data.Length < days) && (i > data.Length))
                {
                    row_tmp[0] = "31";
                    row_tmp[1] = "-";
                    row_tmp[2] = "-";
                    row_tmp[3] = "-";
                    row_tmp[4] = "-";
                    row_tmp[5] = "-";
                    row_tmp[6] = "-";

                }
                else
                {
                    row_tmp = data[i];
                }
                for (int j = 0; j < row_tmp.Length; j++)
                {
                    
                    Control tbox = FindControl("textBox_"+i.ToString()+"_"+j.ToString());
                    TextBox my_text = (TextBox)tbox;
                    my_text.Text = row_tmp[j].ToString();


                }
            }
        }
    }

    protected void __drawStaze(SortedList data_info, string mesiac, string rok, int days)
    {
        if (data_info["id"] != null)
        {
            Session.Add("akt_rozpis",data_info["id"].ToString());


            String[][] data = my_x2.parseRozpis(data_info["rozpis"].ToString());
            //ArrayList my_list = new ArrayList();

            mesiac_cb.SelectedValue = data_info["mesiac"].ToString();

            rok_cb.SelectedValue = data_info["rok"].ToString();

           /* if (Convert.ToInt32(data_info["publish"]) == 1)
            {

                publish_ck.Checked = true;
            }
            else
            {
                publish_ck.Checked = false;
            }*/


            days_lbl.Text = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac)).ToString();

            for (int i = 0; i < days; i++)
            {
                TableRow riadok = new TableRow();

                Table1.Controls.Add(riadok);
                for (int j = 0; j < data[i].Length; j++)
                {
                    TableCell my_cell = new TableCell();
                    my_cell.BorderWidth = 1;
                    if (j == 0)
                    {
                        my_cell.Width = 40;
                    }
                    else
                    {
                        my_cell.Width = 130;
                    }
                    my_cell.Height = 60;
                    my_cell.VerticalAlign = VerticalAlign.Top;
                    my_cell.BorderColor = System.Drawing.Color.FromArgb(0x990000);
                    TextBox my_text_box = new TextBox();
                    my_text_box.BorderStyle = BorderStyle.None;

                    my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();

                    int den = i + 1;
                    DateTime my_date = new DateTime(Convert.ToInt32(data_info["rok"].ToString()), Convert.ToInt32(data_info["mesiac"].ToString()), den);
                    int dnesJe = (int)my_date.DayOfWeek;
                    string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                    // pokus.Text += nazov;

                    if ((nazov == "sobota") || (nazov == "nedeľa"))
                    {
                        my_text_box.BackColor = System.Drawing.Color.FromArgb(0x990000);
                        my_text_box.ForeColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        my_text_box.BackColor = System.Drawing.Color.White;
                    }

                    string[] freeDays = x_db.getFreeDays();

                    string mesDen = den.ToString() + "." + mesiac;

                    int rs_tmp = Array.IndexOf(freeDays, mesDen);

                    if ((rs_tmp != -1) && (nazov != "sobota") && (nazov != "nedeľa"))
                    {
                        my_text_box.BackColor = System.Drawing.Color.Yellow;
                        my_text_box.ForeColor = System.Drawing.Color.FromArgb(0x990000);
                    }
                    if (j == 0)
                    {
                        my_text_box.Width = 40;
                    }
                    else
                    {
                        my_text_box.Width = 100;
                    }
                    my_text_box.Height = 80;
                    my_text_box.TextMode = TextBoxMode.MultiLine;
                    my_text_box.Text = data[i][j];
                    
                    if ((user_rights == "admin") || (user_rights == "poweruser") )
                    {
                        my_text_box.ReadOnly = false;
                    }
                    else
                    {
                        my_text_box.ReadOnly = true;
                    }

                    //my_text_box.ReadOnly = true;
                    //my_cell.Text = "textBox_" + i.ToString() + "_" + j.ToString();

                    my_cell.Controls.Add(my_text_box);

                    //form1.Controls.Add(my_text_box);
                    riadok.Controls.Add(my_cell);
                }

            }
        }
        else
        {
            days_lbl.Text = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac)).ToString();
            mesiac_cb.SelectedValue = mesiac;
            rok_cb.SelectedValue = rok;
            // Response.Cookies["akt_sluzba"].Expires = DateTime.Now.AddDays(-1);
            Session.Add("akt_rozpis","0");
            for (int i = 0; i < days; i++)
            {
                TableRow riadok = new TableRow();

                Table1.Controls.Add(riadok);
                for (int j = 0; j < 7; j++)
                {
                    TableCell my_cell = new TableCell();
                    my_cell.BorderWidth = 1;
                    if (j == 0)
                    {
                        my_cell.Width = 40;
                    }
                    else
                    {
                        my_cell.Width = 130;
                    }
                    my_cell.VerticalAlign = VerticalAlign.Top;
                    my_cell.BorderColor = System.Drawing.Color.FromArgb(0x990000);
                    TextBox my_text_box = new TextBox();
                    my_text_box.BorderStyle = BorderStyle.None;


                    my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();
                    int den = i + 1;
                    DateTime my_date = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), den);
                    int dnesJe = (int)my_date.DayOfWeek;
                    string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                    // pokus.Text += nazov;

                    if ((nazov == "sobota") || (nazov == "nedeľa"))
                    {
                        my_text_box.BackColor = System.Drawing.Color.FromArgb(0x990000);
                        my_text_box.ForeColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        my_text_box.BackColor = System.Drawing.Color.White;
                    }

                    string[] freeDays = x_db.getFreeDays();

                    string mesDen = den.ToString() + "." + mesiac;

                    int rs_tmp = Array.IndexOf(freeDays, mesDen);

                    if ((rs_tmp != -1) && (nazov != "sobota") && (nazov != "nedeľa"))
                    {
                        my_text_box.BackColor = System.Drawing.Color.Yellow;
                        my_text_box.ForeColor = System.Drawing.Color.FromArgb(0x990000);
                    }

                    my_text_box.TextMode = TextBoxMode.MultiLine;
                    if (j == 0)
                    {
                        my_text_box.Width = 40;
                    }
                    else
                    {
                        my_text_box.Width = 100;
                    }
                    my_text_box.Height = 80;
                    if (j == 0)
                    {

                        my_text_box.Text = den.ToString();
                    }
                    else
                    {
                        my_text_box.Text = "-";
                    }


                   
                    if ((user_rights == "admin") || (user_rights == "poweruser") )
                    {
                        my_text_box.ReadOnly = false;
                    }
                    else
                    {
                        my_text_box.ReadOnly = true;
                    }
                    //my_text_box.ReadOnly = true;
                    //my_cell.Text = "textBox_" + i.ToString() + "_" + j.ToString();

                    my_cell.Controls.Add(my_text_box);


                    //form1.Controls.Add(my_text_box);
                    riadok.Controls.Add(my_cell);
                }
                //Response.Write("<br>");

            }
        }
    }

    


    protected void drawRozpis(string mesiac, string rok)
    {
        SortedList data_info = x_db.loadRozpisMonthYear(Convert.ToInt32(mesiac.ToString()), Convert.ToInt32(rok.ToString()));

        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        //vypis_lbl.Text = data_info.Count.ToString();

        string rights = Session["rights"].ToString();

        if (data_info["rozpis"] != null)
        {

            if (((rights.IndexOf("users") != -1) || (rights.IndexOf("sestra") != -1))  && (data_info["publish"].ToString() == "0"))
            {
                vypis_lbl.Text = "<font style='color:red'>Staze, ešte nie sú dokončené!</font> ";
                vypis1_lbl.Text = "<font style='color:red'>Staze, ešte nie sú dokončené!</font> ";
            }
            else
            {

                this.__drawStaze(data_info, mesiac, rok, days);
            }
        }
        else
        {
            this.__drawStaze(data_info, mesiac, rok, days);
        }

        

        /*LinkButton my_link_btn = new LinkButton();
        my_link_btn.Text = "ulozit";
        my_link_btn.PostBackUrl = "sluzby.aspx?mesiac=" + mesiac + "&rok=" + rok;
        form1.Controls.Add(my_link_btn);*/

    }

    protected string getStaze()
    {
        int pocet_dni = Convert.ToInt32(days_lbl.Text);
        string[] month = new string[pocet_dni];
        string def ="";

        for (int i = 0; i < pocet_dni; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                
                    Control tbox = FindControl("textBox_"+i.ToString()+"_"+j.ToString());

                    if (tbox != null)
                    {
                        TextBox my_box = (TextBox)tbox;
                        string mtext = my_box.Text.ToString();

                        if (j == 0)
                        {
                            month[i] = month[i] + mtext;
                        }
                        else
                        {
                            month[i] = month[i] + "|" + mtext;
                        }
                    }
               
            }

            


            
        }
        def = String.Join("~", month);
        return def;
    }


   

    protected void saveStaze()
    {
        SortedList data = new SortedList();
               
        if (Session["akt_rozpis"].ToString() != "0")
        {
            //data.Add("id", Request.Cookies["akt_sluzba"].Value.ToString);
            data.Add("rozpis", this.getStaze());
            //if (publish_ck.Checked)
            //{
            //    data.Add("publish", "1");
            //}
            //else
            //{
            //    data.Add("publish", "0");
            //}

            string res = x_db.updateRozpis("is_rozpis", data, Session["akt_rozpis"].ToString());

            if (res == "ok")
            {
                vypis_lbl.Text = "Aktuálne staze boli update-tnuté....";
                vypis1_lbl.Text = "Aktuálne staze boli update-tnuté....";
            }
            else
            {
                vypis_lbl.Text = "Nastala chyba: " + res + "  " + Session["akt_rozpis"].ToString();
                vypis1_lbl.Text = "Nastala chyba: " + res + "  " + Session["akt_rozpis"].ToString();
            }

        }
        else
        {
            data.Add("mesiac", mesiac_cb.SelectedValue.ToString());
            data.Add("rok", rok_cb.SelectedValue.ToString());
            data.Add("rozpis", this.getStaze().ToString());
            //if (publish_ck.Checked)
            //{
            //    data.Add("publish", "1");
            //}
            //else
            //{
            //    data.Add("publish", "0");
            //}

            SortedList ins_data = x_db.insertStaze("is_rozpis", data);

            if (ins_data["status"].ToString() == "ok")
            {
                Session.Add("akt_ropis",ins_data["last_id"].ToString());
                vypis_lbl.Text = "Aktuálne staze boli uložené v poriadku....." + Session["akt_rozpis"].ToString();
                vypis1_lbl.Text = "Aktuálne staze boli uložené v poriadku....." + Session["akt_rozpis"].ToString();
            }
            else if (ins_data["status"].ToString() == "error")
            {
                vypis_lbl.Text = "Nastala chyba:" + ins_data["message"].ToString();
                vypis1_lbl.Text = "Nastala chyba:" + ins_data["message"].ToString();
            }
        }



    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //this.drawStaze(mesiac_cb.SelectedValue.ToString(), rok_cb.SelectedValue.ToString());
        this.saveStaze();
       // tabulka = "pokus";
        //Session.Add("moje", "lila");
    }

    protected void changeSluzba(object sender, EventArgs e)
    {
        Table1.Controls.Clear();

        //vypis_lbl.Text = mesiac_cb.SelectedValue.ToString();
       // Response.Cookies["rok"].Value = rok_cb.SelectedValue.ToString();
       this.drawRozpis(mesiac_cb.SelectedValue.ToString(), rok_cb.SelectedValue.ToString());
    }

    protected void prevMonth_Click(object sender, EventArgs e)
    {
        //Table1.Controls.Clear();
        int mesiac = Convert.ToInt32(mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(rok_cb.SelectedValue);
        if (mesiac == 12)
        {
            mesiac = 1;
            rok--;
        }
        else
        {
            mesiac--;
        }
        this.drawRozpisPrev(mesiac.ToString(), rok.ToString());

    }


   

    protected void toWord_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("rozpisword.aspx?rok=" + rok_cb.SelectedValue.ToString()+"&mesiac="+mesiac_cb.SelectedValue.ToString()+"&mes="+mesiac_cb.SelectedItem.ToString());
    }
    protected void print_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("rozpisword.aspx?rok=" + rok_cb.SelectedValue.ToString() + "&mesiac=" + mesiac_cb.SelectedValue.ToString() + "&mes=" + mesiac_cb.SelectedItem.ToString()+"&print=1");
    }
}
