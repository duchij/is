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
    sluzbyclass x_db = new sluzbyclass();
    //string tabulka = "";
    string user_rights = "";




    protected void Page_Init(object sender, EventArgs e)
    {

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        user_rights = Session["rights"].ToString();

        if ((user_rights == "admin") || (user_rights == "poweruser"))
        {
            Button1.Visible = true;
            publish_ck.Visible = true;
        }
        else
        {
            Button1.Visible = false;
            publish_ck.Visible = false;
        }

        this.msg_lbl.Visible = false;

        if (IsPostBack == false)
        {
            DateTime akt_datum = DateTime.Today;

            string mesiac = akt_datum.Month.ToString();
            string rok = akt_datum.Year.ToString();
            //vypis_lbl.Text = rok;

            this.drawSluzby(mesiac, rok);
        }
        else
        {
            string rok = rok_cb.SelectedValue.ToString();
            string mesiac = mesiac_cb.SelectedValue.ToString();

            int mesTmp = Convert.ToInt32(mesiac);
            int rokTmp = Convert.ToInt32(rok);

            //this.Table1.Controls.Clear();
            //this.drawSluzby(mesiac, rok);

            this.drawSluzby(mesiac, rok);
            /*if (mesTmp > 6 && rokTmp >= 2012)
            {
                //this.Table1.Controls.Clear();
                this.drawTable2(rok, mesiac);
            }
            else
            {
                
                this.drawTable(rok, mesiac);
            }*/

            // Response.Cookies["mesiac"].Value = mesiac_cb.SelectedValue.ToString();
            // Response.Cookies["rok"].Value = rok_cb.SelectedValue.ToString();
            //vypis_lbl.Text = rok_cb.SelectedValue.ToString();
            // this.drawSluzby(Request.Cookies["mesiac"].Value.ToString(), Request.Cookies["rok"].Value.ToString());
            //this.saveSluzby();
            //vypis_lbl.Text = Session.Count.ToString();

        }
    }


    private void drawTable(string rok, string mesiac)
    {

        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        days_lbl.Text = days.ToString();
        SortedList doctorList = x_db.getDoctorsForVykaz();
        for (int i = 0; i < days; i++)
        {
            TableRow riadok = new TableRow();

            Table1.Controls.Add(riadok);
            for (int j = 0; j < 6; j++)
            {
                TableCell my_cell = new TableCell();
                my_cell.BorderWidth = 1;
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
                my_text_box.Width = 100;

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

    private void drawTable2(string rok, string mesiac)
    {

        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        days_lbl.Text = days.ToString();
        ArrayList doctorList = x_db.getDoctorsForDl();
        for (int i = 0; i < days; i++)
        {
            TableRow riadok = new TableRow();
            Table1.Controls.Add(riadok);

            for (int j = 0; j < 6; j++)
            {
                TableCell my_cell = new TableCell();

                my_cell.BorderWidth = 1;
                my_cell.BorderColor = System.Drawing.Color.FromArgb(0x990000);

                if (j > 0)
                {
                    DropDownList my_text_box = new DropDownList();

                    ListItem[] newItem = new ListItem[doctorList.Count];


                    for (int po = 0; po < doctorList.Count; po++)
                    {

                        char[] del = { '|' };
                        string strTmp = doctorList[po].ToString();

                        string[] docId = strTmp.Split(del);

                        newItem[po] = new ListItem(docId[1].ToString(), docId[0].ToString());


                    }

                    my_text_box.Items.AddRange(newItem);


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
                    my_text_box.Width = 100;
                    my_cell.Controls.Add(my_text_box);

                }
                else
                {
                    Label my_text_box = new Label();
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
                    my_text_box.Text = den.ToString() + "." + nazov.Substring(0, 2);
                    my_text_box.Width = 100;
                    my_cell.Controls.Add(my_text_box);
                }
                //my_text_box.AutoPostBack = true;





                //form1.Controls.Add(my_text_box);
                riadok.Controls.Add(my_cell);
            }

        }
    }

    protected void __drawSluzby(SortedList data_info, string mesiac, string rok, int days)
    {
        if (data_info["id"] != null)
        {
            Session.Add("akt_sluzba", data_info["id"].ToString());


            String[][] data = my_x2.parseSluzba(data_info["rozpis"].ToString());
            //ArrayList my_list = new ArrayList();

            mesiac_cb.SelectedValue = data_info["mesiac"].ToString();

            rok_cb.SelectedValue = data_info["rok"].ToString();

            if (Convert.ToInt32(data_info["publish"].ToString()) == 1)
            {

                publish_ck.Checked = true;
            }
            else
            {
                publish_ck.Checked = false;
            }


            days_lbl.Text = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac)).ToString();

            for (int i = 0; i < days; i++)
            {
                TableRow riadok = new TableRow();

                Table1.Controls.Add(riadok);
                for (int j = 0; j < data[i].Length; j++)
                {
                    TableCell my_cell = new TableCell();
                    my_cell.BorderWidth = 1;
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


                    my_text_box.Text = data[i][j];
                    my_text_box.Width = 100;
                    if ((user_rights == "admin") || (user_rights == "poweruser"))
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
            Session.Add("akt_sluzba", "0");
            for (int i = 0; i < days; i++)
            {
                TableRow riadok = new TableRow();

                Table1.Controls.Add(riadok);
                for (int j = 0; j < 6; j++)
                {
                    TableCell my_cell = new TableCell();
                    my_cell.BorderWidth = 1;
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


                    if (j == 0)
                    {

                        my_text_box.Text = den.ToString() + "." + nazov.Substring(0, 2); ;
                    }
                    else
                    {
                        my_text_box.Text = "-";
                    }


                    my_text_box.Width = 100;
                    if ((user_rights == "admin") || (user_rights == "poweruser"))
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

    protected void __drawSluzby2(SortedList data_info, string mesiac, string rok, int days)
    {
        ArrayList doctorList = x_db.getDoctorsForDl();
        this.msg_lbl.Text = "dl " + doctorList.Count.ToString();

        if (data_info["id"] != null)
        {

            if (data_info["publish"].ToString() == "1" && IsPostBack == false)
            {

                this.publish_ck.Checked = true;
            }

            if (data_info["publish"].ToString() == "0" && IsPostBack == false)
            {
                this.publish_ck.Checked = false;
            }


            Session.Add("akt_sluzba", data_info["id"].ToString());


            String[][] data = my_x2.parseSluzba(data_info["rozpis"].ToString());
            //ArrayList my_list = new ArrayList();

            mesiac_cb.SelectedValue = data_info["mesiac"].ToString();

            rok_cb.SelectedValue = data_info["rok"].ToString();



            days_lbl.Text = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac)).ToString();

            for (int i = 0; i < days; i++)
            {
                TableRow riadok = new TableRow();
                Table1.Controls.Add(riadok);

               

                for (int j = 0; j < data[i].Length; j++)
                {

                    TableCell my_cell = new TableCell();

                    my_cell.BorderWidth = 1;
                   // my_cell.Width = System.Math.
                    my_cell.BorderColor = System.Drawing.Color.FromArgb(0x990000);
                    //if (j == 0) my_cell.Width = Unit.Percentage(10);

                    if (j > 0)
                    {

                        DropDownList my_text_box = new DropDownList();
                        my_text_box.BorderStyle = BorderStyle.None;

                        my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();
                        my_text_box.CssClass = "no-pad-mobile no-gap-mobile";
                        ListItem[] newItem = new ListItem[doctorList.Count];


                        for (int po = 0; po < doctorList.Count; po++)
                        {

                            char[] del = { '|' };

                            string strTmp = doctorList[po].ToString();

                            string[] docId = strTmp.Split(del);

                            newItem[po] = new ListItem(docId[1].ToString(), docId[0].ToString());


                        }

                        my_text_box.Items.AddRange(newItem);

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



                        if ((user_rights == "admin") || (user_rights == "poweruser"))
                        {
                            // my_text_box.ReadOnly = false;
                            my_text_box.Enabled = true;
                        }
                        else
                        {
                            // my_text_box.ReadOnly = true;
                            my_text_box.Enabled = false;
                            // my_text_box.BackColor = System.Drawing.Color.Black;
                            //my_text_box.ForeColor = System.Drawing.Color.Black;
                        }

                        //my_text_box.ReadOnly = true;
                        //my_cell.Text = "textBox_" + i.ToString() + "_" + j.ToString();



                        my_cell.Controls.Add(my_text_box);

                        string _mm = data[i][j].ToString();

                        char[] delTmp = { '|' };

                        string[] oup = _mm.Split(delTmp);


                        if (j == 1)
                        {
                            TextBox noteTxt = new TextBox();
                            noteTxt.ID = "noteTxt_" + i.ToString();
                            noteTxt.Text = oup[1];
                           // noteTxt.Width = 100;
                            my_cell.Controls.Add(noteTxt);
                        }




                        my_text_box.SelectedValue = oup[0];
                       // my_text_box.Width = 100;

                        //form1.Controls.Add(my_text_box);
                        riadok.Controls.Add(my_cell);


                    }
                    else
                    {
                        Label my_text_box = new Label();
                        my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();
                        //my_text_box.Width = 30;
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
                        my_text_box.Text = den.ToString() + "." + nazov.Substring(0, 2);
                        
                        my_cell.Controls.Add(my_text_box);

                        //form1.Controls.Add(my_text_box);
                        riadok.Controls.Add(my_cell);

                    }

                }

            }
        }
        else
        {
            days_lbl.Text = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac)).ToString();
            mesiac_cb.SelectedValue = mesiac;
            rok_cb.SelectedValue = rok;
            // Response.Cookies["akt_sluzba"].Expires = DateTime.Now.AddDays(-1);
            Session.Add("akt_sluzba", "0");
            for (int i = 0; i < days; i++)
            {
                TableRow riadok = new TableRow();
                Table1.Controls.Add(riadok);


                for (int j = 0; j < 6; j++)
                {

                    TableCell my_cell = new TableCell();
                    if (j == 0) my_cell.Width = Unit.Percentage(10);
                    //my_cell.Width = 100;

                    if (j > 0)
                    {

                        my_cell.BorderWidth = 1;
                        my_cell.BorderColor = System.Drawing.Color.FromArgb(0x990000);


                        DropDownList my_text_box = new DropDownList();
                        my_text_box.BorderStyle = BorderStyle.None;
                        my_text_box.CssClass = "no-pad-mobile no-gap-mobile";

                        my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();

                        ListItem[] newItem = new ListItem[doctorList.Count];
                        int nl = 0;

                        for (int po = 0; po < doctorList.Count; po++)
                        {

                            char[] del = { '|' };

                            string strTmp = doctorList[po].ToString();

                            string[] docId = strTmp.Split(del);

                            newItem[po] = new ListItem(docId[1].ToString(), docId[0].ToString());


                        }
                        my_text_box.Items.AddRange(newItem);

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


                        string _mm = "";
                        my_text_box.SelectedValue = _mm;



                       // my_text_box.Width = 100;
                        if ((user_rights == "admin") || (user_rights == "poweruser"))
                        {
                            my_text_box.Enabled = true;
                        }
                        else
                        {
                            my_text_box.Enabled = false;
                            //my_text_box.BackColor = System.Drawing.Color.Black;
                            //my_text_box.ForeColor = System.Drawing.Color.Black;
                        }
                        //my_text_box.ReadOnly = true;
                        //my_cell.Text = "textBox_" + i.ToString() + "_" + j.ToString();
                        my_cell.Controls.Add(my_text_box);

                        if (j == 1)
                        {
                            TextBox noteTxt = new TextBox();
                            noteTxt.ID = "noteTxt_" + i.ToString();
                           // noteTxt.Width = 100;
                            noteTxt.Text = "-";
                            my_cell.Controls.Add(noteTxt);
                        }
                    }
                    else
                    {
                        Label my_text_box = new Label();

                        my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();
                      //  my_text_box.Width = 100;
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
                        my_text_box.Text = den.ToString() + "." + nazov.Substring(0, 2);

                        my_cell.Controls.Add(my_text_box);
                    }




                    //form1.Controls.Add(my_text_box);
                    riadok.Controls.Add(my_cell);
                }
                //Response.Write("<br>");

            }
        }
    }




    protected void drawSluzby(string mesiac, string rok)
    {
        SortedList data_info = x_db.loadSluzbaMonthYear("is_sluzby", mesiac, rok);
        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        vypis_lbl.Text = data_info.Count.ToString();

        string rights = Session["rights"].ToString();
        int mesTmp = Convert.ToInt32(mesiac);
        int rokTmp = Convert.ToInt32(rok);

        DateTime pDate = Convert.ToDateTime("01.07.2012");
        DateTime oDate = Convert.ToDateTime("01." + mesiac + "." + rok);

        if (data_info["rozpis"] != null)
        {

            if (((rights == "users") || (rights == "sestra")) && (data_info["publish"].ToString() == "0"))
            {
                vypis_lbl.Text = "<font style='color:red'>Služby, ešte nie sú dokončené!</font> ";
            }
            else
            {
                if (oDate >= pDate)
                {
                    this.__drawSluzby2(data_info, mesiac, rok, days);
                }
                else
                {
                    this.__drawSluzby(data_info, mesiac, rok, days);
                }

            }
        }
        else
        {
            if (oDate >= pDate)
            {
                this.__drawSluzby2(data_info, mesiac, rok, days);
            }
            else
            {
                this.__drawSluzby(data_info, mesiac, rok, days);
            }
        }



        /*LinkButton my_link_btn = new LinkButton();
        my_link_btn.Text = "ulozit";
        my_link_btn.PostBackUrl = "sluzby.aspx?mesiac=" + mesiac + "&rok=" + rok;
        form1.Controls.Add(my_link_btn);*/

    }

    protected string getSluzby()
    {
        int pocet_dni = Convert.ToInt32(days_lbl.Text);
        string[] month = new string[pocet_dni];
        string def = "";

        int mesTmp = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int rokTmp = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());

        DateTime pDate = Convert.ToDateTime("01.07.2012");
        DateTime oDate = Convert.ToDateTime("01." + mesTmp + "." + rokTmp);

        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        if (oDate >= pDate)
        {
          

            for (int i = 0; i < pocet_dni; i++)
            {
                for (int j = 1; j <= 6; j++)
                {

                    Control tbox = ctpl.FindControl("textBox_" + i.ToString() + "_" + j.ToString());

                    if (tbox != null)
                    {
                        DropDownList my_box = (DropDownList)tbox;
                        string mtext = my_box.SelectedValue.ToString();

                        if (j == 1)
                        {


                            Control myTxt = ctpl.FindControl("noteTxt_" + i.ToString());
                            TextBox myNote = (TextBox)myTxt;
                            string note = myNote.Text.ToString();
                            note = note.Replace(",", ";");
                            month[i] = "," + mtext + "|" + note;

                        }
                        else
                        {
                            month[i] = month[i] + "," + mtext;
                        }
                    }

                }
            }
        }
        else
        {
            

            for (int i = 0; i < pocet_dni; i++)
            {
                for (int j = 0; j < 6; j++)
                {

                    Control tbox = ctpl.FindControl("textBox_" + i.ToString() + "_" + j.ToString());


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
                            month[i] = month[i] + "," + mtext;
                        }
                    }

                }
            }





        }
        def = String.Join("\r", month);
        return def;
    }




    protected void saveSluzby()
    {
        SortedList data = new SortedList();

        if (Session["akt_sluzba"].ToString() != "0")
        {
            //data.Add("id", Request.Cookies["akt_sluzba"].Value.ToString);
            data.Add("rozpis", this.getSluzby());
            if (publish_ck.Checked)
            {
                data.Add("publish", "1");
            }
            else
            {
                data.Add("publish", "0");
            }

            string res = x_db.updateSluzby("is_sluzby", data, Session["akt_sluzba"].ToString());

            if (res == "ok")
            {
                vypis_lbl.Text = "Aktuálne služby boli update-tnuté....";
            }
            else
            {
                vypis_lbl.Text = "upNastala chyba: " + res + "  " + Session["akt_sluzba"].ToString();
            }

        }
        else
        {
            data.Add("mesiac", this.mesiac_cb.SelectedValue.ToString());
            data.Add("rok", this.rok_cb.SelectedValue.ToString());
            data.Add("rozpis", this.getSluzby().ToString());
            if (publish_ck.Checked)
            {
                data.Add("publish", "1");
            }
            else
            {
                data.Add("publish", "0");
            }

            SortedList ins_data = x_db.insertSluzby("is_sluzby", data);

            if (ins_data["status"].ToString() == "ok")
            {
                Session.Add("akt_sluzba", ins_data["last_id"].ToString());
                vypis_lbl.Text = "Aktuálne sluzby boli uložené v poriadku....." + Session["akt_sluzba"].ToString();
            }
            else if (ins_data["status"].ToString() == "error")
            {
                vypis_lbl.Text = "Nastala chyba:" + ins_data["message"].ToString();
            }
        }



    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //this.drawSluzby(mesiac_cb.SelectedValue.ToString(), rok_cb.SelectedValue.ToString());
        this.saveSluzby();
        // tabulka = "pokus";
        //Session.Add("moje", "lila");
    }

    protected void changeSluzba(object sender, EventArgs e)
    {
        this.Table1.Controls.Clear();

        //vypis_lbl.Text = mesiac_cb.SelectedValue.ToString();
        // Response.Cookies["rok"].Value = rok_cb.SelectedValue.ToString();

        this.drawSluzby(this.mesiac_cb.SelectedValue.ToString(), this.rok_cb.SelectedValue.ToString());
    }



    protected void toWord_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("sltoword.aspx?rok=" + rok_cb.SelectedValue.ToString() + "&mesiac=" + mesiac_cb.SelectedValue.ToString() + "&mes=" + mesiac_cb.SelectedItem.ToString());
    }
    protected void print_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("sltoword.aspx?rok=" + rok_cb.SelectedValue.ToString() + "&mesiac=" + mesiac_cb.SelectedValue.ToString() + "&mes=" + mesiac_cb.SelectedItem.ToString() + "&print=1");
    }
}
