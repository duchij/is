using System;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class vykaz : System.Web.UI.Page
{
    x2_var my_x2 = new x2_var();
    vykazdb x_db = new vykazdb();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        if (Session["pracdoba"].ToString().Trim().Length == 0 || Session["tyzdoba"].ToString().Trim().Length == 0 || Session["osobcisl"].ToString().Trim().Length == 0)
        {

           // Page page = HttpContext.Current.CurrentHandler as Page;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('"+Resources.Resource.vykaz_error+"');", true);

            //Response.Redirect("adduser.aspx");
        }

        //this.zaMesiac_lbl.Text = "Maj,2012";

        this.msg_lbl.Visible = false;

        if (IsPostBack == false)
        {
            DateTime akt_datum = DateTime.Today;

            int mesiac = akt_datum.Month;
            int rok = akt_datum.Year;

            this.mesiac_cb.SelectedValue = mesiac.ToString();
            this.rok_cb.SelectedValue = rok.ToString();

            this.runGenerate(mesiac, rok);
        }
        else
        {
            string mesiac = this.mesiac_cb.SelectedValue.ToString();
            string rok = this.rok_cb.SelectedValue.ToString();

            this.runGenerate(Convert.ToInt32(mesiac), Convert.ToInt32(rok));

        }



        
    }

    protected void runGenerate(int mesiac,int rok)
    {
        SortedList curVykaz = x_db.getCurrentVykaz(Convert.ToInt32(Session["user_id"].ToString()), mesiac, rok);
        string prenos = x_db.getPrevMonthPrenos(Convert.ToInt32(Session["user_id"].ToString()), mesiac, rok);

        string tmp = this.predMes_txt.Text.ToString();
        tmp = tmp.Trim();
        if (tmp.Length == 0)
        {

            this.predMes_txt.Text = prenos;
        }

        msg_lbl.Text = curVykaz.Count.ToString();

        if (curVykaz["id"] != null)
        {
            Session["vykaz_id"] = curVykaz["id"].ToString();
            this.createVykazFromData(curVykaz, rok, mesiac);

        }
        else
        {
            this.generateVykazTable(rok.ToString(), mesiac.ToString(), true);
        }

        this.fillInVacations(mesiac, rok, Session["user_id"].ToString());
        
    }

    protected void fillInVacations(int mesiac, int rok, string id)
    {
        ArrayList dovolenky = x_db.getDovolenkyByID(mesiac,rok,Convert.ToInt32(id)); 
        int dovCnt = dovolenky.Count;

        for (int i = 0; i < dovCnt; i++)
        {
            string[] data = dovolenky[i].ToString().Split(';');

            //string dd1 = my_x2.MSDate(data[1].ToString());
            //string dd2 = my_x2.MSDate(data[2].ToString());

            DateTime odDov = Convert.ToDateTime(data[1].ToString());
            DateTime doDov = Convert.ToDateTime(data[2].ToString());

            for (DateTime ddStart = odDov; ddStart <= doDov; ddStart += TimeSpan.FromDays(1))
            {
                if (ddStart.Month == mesiac && ddStart.Year == rok)
                {
                    int vikend = (int)ddStart.DayOfWeek;

                    string[] freeDays = x_db.getFreeDays();

                    string mesDen = ddStart.Day.ToString() + "." + mesiac;

                    int rs_tmp = Array.IndexOf(freeDays, mesDen);

                    if (vikend != 0 && vikend != 6 && rs_tmp == -1)
                    {

                        int ddTemp = ddStart.Day - 1;

                        ContentPlaceHolder ctpl = new ContentPlaceHolder();
                        Control tmp = Page.Master.FindControl("ContentPlaceHolder1");

                        ctpl = (ContentPlaceHolder)ctpl;


                        Control tbox = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_2");
                        Control tbox1 = ctpl.FindControl("textBox_" + ddTemp.ToString() + "_1");


                        TextBox my_text_box = (TextBox)tbox;
                        TextBox my_text_box1 = (TextBox)tbox1;

                        my_text_box.Text = "D";
                        my_text_box1.Text = "D";
                    }
                }

            }



        }

    }


    protected void onMonthChangedFnc(object sender, EventArgs e)
    {
        this.predMes_txt.Text = "";
        this.pocetHod_txt.Text = "";
        this.hodiny_lbl.Text = "0";
        this.rozdiel_lbl.Text = "0";
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        this.vykaz_tbl.Controls.Clear();
        Session.Remove("vykaz_id");

        this.runGenerate(Convert.ToInt32(mesiac),Convert.ToInt32(rok));
    }

    protected void onYearChangedFnc(object sender, EventArgs e)
    {
        this.predMes_txt.Text = "";
        this.pocetHod_txt.Text = "";
        this.hodiny_lbl.Text = "0";
        this.rozdiel_lbl.Text = "0";
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();
        Session.Remove("vykaz_id");

        this.vykaz_tbl.Controls.Clear();

        this.runGenerate(Convert.ToInt32(mesiac), Convert.ToInt32(rok));
    }



    protected void createVykazFromData(SortedList data, int rok, int mesiac)
    {
        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        string vykazStr = data["vykaz"].ToString();

        char[] delimiter = {'|'};

        string[] rows = vykazStr.Split(delimiter);

        int rowLen = rows.Length;

        this.generateVykazTable(rok.ToString(), mesiac.ToString(), false);

        for (int i = 0; i < days; i++)
        {
            char[] del1 = { '~' };

            string[] cols = rows[i].Split(del1);

            int colsLen = cols.Length;

            for (int j = 0; j < colsLen; j++)
            {
                ContentPlaceHolder ctp = new ContentPlaceHolder();
                Control tmp = Page.Master.FindControl("ContentPlaceHolder1");
                ctp = (ContentPlaceHolder)tmp;


                Control tbox = ctp.FindControl("textBox_" + i.ToString() + "_" + j.ToString());
                TextBox my_text_box = (TextBox)tbox;

                if (cols[j].ToString() != "0")
                {
                    my_text_box.Text = cols[j].ToString();
                }
            }
        }
    }

    


    protected void generateVykazTable(string rok, string mesiac, Boolean activeCalc)
    {
        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        this.vykaz_tbl.Controls.Clear();

        for (int i = 0; i < days; i++)
        {
            TableRow riadok = new TableRow();
            this.vykaz_tbl.Controls.Add(riadok);

            for (int j = 0; j < 12; j++)
            {
                TableCell my_cell = new TableCell();
                my_cell.BorderWidth = 1;
                TextBox my_text_box = new TextBox();
                my_text_box.BorderStyle = BorderStyle.None;
                my_text_box.Width = 40;
                my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();

                my_cell.Width = 45;

                int den = i + 1;
                DateTime my_date = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), den);
                int dnesJe = (int)my_date.DayOfWeek;
                string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                // pokus.Text += nazov;

                if (j == 0)
                {
                    my_text_box.Text = den.ToString() + "." + nazov.Substring(0, 2);
                }

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


                my_cell.Controls.Add(my_text_box);

                //form1.Controls.Add(my_text_box);
                riadok.Controls.Add(my_cell);

            }

        }

        if (activeCalc)
        {
            //this.enterVykazData(mesiac, rok);
        }
    }

    protected void enterVykazData(string mesiac, string rok)
    {

        int mesTmp = Convert.ToInt32(mesiac);
        int rokTmp = Convert.ToInt32(rok);
        int days = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));


        Boolean[] mySluzby = new Boolean[days];

        DateTime pDate = Convert.ToDateTime("01.07.2012");
        DateTime oDate = Convert.ToDateTime("01." + mesiac+ "." + rok);

        if (oDate >= pDate)
        {
            mySluzby = x_db.getSluzbyOfUserByID(Session["user_id"].ToString(), mesiac, rok);
        }
        else
        {
            mySluzby = x_db.getSluzbyOfUser(my_x2.getVykazName(Session["fullname"].ToString()), mesiac, rok);
        }

        
      //  int prevDay = 0;
      //  int exDay = 0;

        SortedList vykazVypis = this.getValueFromSluzba();
        string[] sviatky = x_db.getFreeDays();
        //Boolean sobota = false;
        //Boolean nedela = false;

        for (int i = 0; i < days; i++)
        {
            int den = i + 1;
            DateTime my_date = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), den);
            int dnesJe = (int)my_date.DayOfWeek;
        //    int pos = 0;
            string[] rozpis = new string[11];

           

            if (mySluzby[i])
            {

                string dentmp = den.ToString() + "." + mesiac;

                int res = Array.IndexOf(sviatky, dentmp);
                /*if (dnesJe == 0 || dnesJe == 6)
                {
                    rozpis = this.denCisla(vykazVypis["velkaSluzba"].ToString());

                    for (int j = 0; j < 12; j++)
                    {
                        if (j > 0)
                        {
                            Control tbox = FindControl("textBox_" + i.ToString() + "_" + j.ToString());
                            TextBox my_text_box = (TextBox)tbox;

                            my_text_box.Text = rozpis[j-1].ToString();
                        }


                    }*/

                    if (dnesJe == 0)
                    {
                        if (res == -1)
                        {
                            this.fillNumbers(this.denCisla(vykazVypis["velkaSluzba"].ToString()), i);
                        }
                        else
                        {
                            this.fillNumbers(this.denCisla(vykazVypis["sviatokVikend"].ToString()), i);
                        }
                        i++;
                        this.fillNumbers(this.denCisla(vykazVypis["malaSluzba2"].ToString()),i);
                    }
                    else if (dnesJe == 6)
                    {
                        if (res == -1)
                        {
                            this.fillNumbers(this.denCisla(vykazVypis["velkaSluzba"].ToString()), i);
                        }
                        else
                        {
                            this.fillNumbers(this.denCisla(vykazVypis["sviatokVikend"].ToString()), i);
                        }

                        i++;
                        this.fillNumbers(this.denCisla(vykazVypis["velkaSluzba2"].ToString()),i);
                        i++; 
                        this.fillNumbers(this.denCisla(vykazVypis["exday"].ToString()),i);
                }
                else
                {
                    if (res == -1)
                    {
                        this.fillNumbers(this.denCisla(vykazVypis["malaSluzba"].ToString()), i);
                    }
                    else
                    {
                        this.fillNumbers(this.denCisla(vykazVypis["sviatok"].ToString()), i);
                    }
                    i++;


                    this.fillNumbers(this.denCisla(vykazVypis["malaSluzba2"].ToString()),i);
                }
            }
            else
            {
                string dentmp = den.ToString()+"."+mesiac;

                int res = Array.IndexOf(sviatky, dentmp);

                

                if (dnesJe != 0 && dnesJe != 6 && res == -1)
                {
                    this.fillNumbers(this.denCisla(vykazVypis["normDen"].ToString()),i);
                }
                else if (res != -1 && dnesJe != 0 && dnesJe != 6)
                {
                    this.fillNumbers(this.denCisla(vykazVypis["sviatokNieVikend"].ToString()), i);
                }
                else
                {
                    this.fillNumbers(this.denCisla(vykazVypis["exday"].ToString()), i);
                }
            }
        }
    }

    protected void fillNumbers(string[] rozpis, int i)
    {
        //rozpis = this.denCisla(vykazVypis["exday"].ToString());

        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        for (int j = 0; j < 11; j++)
        {
            if (j > 0)
            {
                Control tbox = ctpl.FindControl("textBox_" + i.ToString() + "_" + j.ToString());
                TextBox my_text_box = (TextBox)tbox;
                //TextBox my_text_box = new TextBox();

                //my_text_box.ID = "textBox_" + i.ToString() + "_" + j.ToString();

                if (rozpis[j-1] != "0")
                {
                    string tmp = rozpis[j - 1].ToString();

                    my_text_box.Text = tmp;
                }
            }


        }
    }

    protected string[] denCisla(string retaz)
    {
        char[] delimiter = {','};

        return retaz.Split(delimiter);
        
    }

    protected SortedList getValueFromSluzba()
    {
        SortedList result = new SortedList();

        StringBuilder sb = new StringBuilder();

        double pracDoba = Convert.ToDouble(Session["pracdoba"]);
        double dlzkaPrace = 7+pracDoba+0.5;

        string pracDobaTmp = pracDoba.ToString().Replace(',', '.');

        sb.AppendFormat("7,{0},{1},0,0,0,0,0,0,0,0", dlzkaPrace, pracDobaTmp);
        result["normDen"] = sb.ToString();

        sb.Length = 0;

        double sluzbaCas = 15 + 4;
        dlzkaPrace = pracDoba + 4;
        
        string dlzkaPraceStr = dlzkaPrace.ToString();

        dlzkaPraceStr = dlzkaPraceStr.Replace(',', '.');

        sb.AppendFormat("7,{0},{1},5,0,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["malaSluzba"] = sb.ToString();

        result["malaSluzba2"]       = "0,0,0,0,0,0,0,0,0,0,0";

        sb.Length = 0;
        sb.AppendFormat("7,{0},{1},5,16.5,0,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["velkaSluzba"]       = sb.ToString();
        result["velkaSluzba2"]      = "0,0,0,0,0,0,0,0,0,0,0,0";
        result["velkaSluzba2a"]     = "0,0,0,0,0,0,0,0,0,0,0,0";

        sb.Length = 0;
        sb.AppendFormat("7,{0},{1},5,16.5,16.5,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["sviatokVikend"] = sb.ToString();
        sb.Length = 0;

        sb.AppendFormat("7,{0},{1},5,0,16.5,0,5,0,7,0,0", sluzbaCas, dlzkaPraceStr);
        result["sviatok"]           = sb.ToString();
        result["exday"]             = "0,0,0,0,0,0,0,0,0,0,0,0";
        sb.Length = 0;
        sb.AppendFormat("0,0,{0},0,0,0,0,0,0,0,0,0", pracDobaTmp);
        result["sviatokNieVikend"]  = sb.ToString();
        

        return result;
    }

    protected decimal getColumSum(int col,int rows, string textBox)
    {
        decimal result = 0;
        decimal tmp = 0;

        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        for (int i = 0; i < rows; i++)
        {
            Control Tbox = ctpl.FindControl(textBox + i.ToString() + "_" + col.ToString());
            TextBox sumBox = (TextBox)Tbox;
            try
            {
                
                //tmp = tmp + Convert.ToInt32(sumBox.Text.ToString());
                string num = sumBox.Text.ToString();
                num = num.Replace('.', ',');
                
                tmp = tmp + Convert.ToDecimal(num);
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                result = tmp;
            }
        }
        return result;
    }

    
    protected void calcData_Click(object sender, EventArgs e)
    {
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());

            int days = DateTime.DaysInMonth(rok, mesiac);

            decimal odpracHod        = this.getColumSum(3, days, "textBox_");
            
            this.hodiny_lbl.Text    = odpracHod.ToString();
            
            this.nocpraca_lbl.Text  = this.getColumSum(4, days, "textBox_").ToString();
            this.mzdovzvyh_lbl.Text = this.getColumSum(5, days, "textBox_").ToString();
            this.sviatok_lbl.Text   = this.getColumSum(6, days, "textBox_").ToString();
            this.a1_lbl.Text        = this.getColumSum(7, days, "textBox_").ToString();
            this.a2_lbl.Text        = this.getColumSum(8, days, "textBox_").ToString();
            this.nea1_lbl.Text      = this.getColumSum(9, days, "textBox_").ToString();
            this.nea2_lbl.Text      = this.getColumSum(10, days, "textBox_").ToString();
            this.nea3_lbl.Text      = this.getColumSum(11, days, "textBox_").ToString();

            DateTime od_date = new DateTime(rok, mesiac, 1);
            DateTime do_date = new DateTime(rok, mesiac, days);

            int pocetVolnychDni = my_x2.pocetVolnychDniBezSviatkov(od_date, do_date);

            int pocetPracdni = days - pocetVolnychDni;
            //zaMesiac_lbl.Text ="days:"+days.ToString()+"....prac"+ pocetPracdni.ToString();
            string prenosStr = this.predMes_txt.Text.ToString();
        
            prenosStr = prenosStr.Replace(".", ",");
            decimal tmpOdpHod = odpracHod + Convert.ToDecimal(prenosStr);
            this.hodiny_lbl.Text = tmpOdpHod.ToString();
            //decimal prenos = Convert.ToDecimal(prenosStr);
            decimal pocetPracHod = 0;

            if (Session["pracdoba"] != null)
            {
                Session["pracdoba"] = Session["pracdoba"].ToString().Replace(".", ",");
                pocetPracHod = pocetPracdni * Convert.ToDecimal(Session["pracdoba"]);
            }
            else
            {
                pocetPracHod = pocetPracdni * Convert.ToDecimal("7,5");
            }


            decimal rozdiel = odpracHod - pocetPracHod + Convert.ToDecimal(prenosStr);

            

            this.pocetHod_txt.Text = pocetPracHod.ToString();
            this.rozdiel_lbl.Text = rozdiel.ToString();
           //pocetHod_txt.Text += "<br>Prenos do dalsieho mesiaca: " + rozdiel.ToString();

           this.saveData(mesiac, rok);
           this.createPdf_btn.Enabled = true;

            //this.createPdf(); 

    }

    protected void createPdf_btn_fnc(object sender, EventArgs e)
    {
        this.createPdf();
    }

    protected void saveData(int mesiac, int rok)
    {
        string vykazData = "";
        int days = DateTime.DaysInMonth(rok, mesiac);

        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        for (int i = 0; i < days; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                Control Tbox = ctpl.FindControl("textBox_" + i.ToString() + "_" + j.ToString());
                TextBox mBox = (TextBox)Tbox;

                string num = mBox.Text.ToString();

                num = num.Trim();

                if (num.Length == 0)
                {
                    num = "0";
                }

                if (j < 11)
                {
                    
                    vykazData += num + "~";
                }
                else
                {
                    vykazData += num;
                }


            }
            vykazData += "|";
        }
        this.msg_lbl.Text = vykazData;

        SortedList sData = new SortedList();
        sData["mesiac"] = mesiac.ToString();
        sData["rok"] = rok.ToString();
        sData["user_id"] = Session["user_id"].ToString();
        sData["vykaz"] = vykazData;
        string prenosStr = this.rozdiel_lbl.Text.ToString();

        //prenosStr = prenosStr.Replace(",", ".");

        sData["prenos"] = prenosStr;

        if (Session["vykaz_id"] == null)
        {
             SortedList res =  x_db.insert_rows("is_vykaz", sData);
             if (res["status"].ToString() != "error")
             {
                 Session["vykaz_id"] = res["last_id"];
             }
             else
             {
                 this.msg_lbl.Text = res["message"].ToString();
             }
        }
        else
        {
            x_db.update_row("is_vykaz", sData, Session["vykaz_id"].ToString());
        }
    }


    protected void createPdf()
    {
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue.ToString());
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue.ToString());
        int milis = DateTime.Now.Millisecond;

        string path = Server.MapPath("App_Data");
        string imagepath = Server.MapPath("App_Data");

        string oldFile = @path+"\\vykaz.pdf";



        string hash = my_x2.makeFileHash(Session["login"].ToString()+milis.ToString());


        string newFile = @path + "\\vykaz_"+hash+".pdf";

        
        this.msg_lbl.Text = oldFile;
        // open the reader
        PdfReader reader = new PdfReader(oldFile);

        Rectangle size = reader.GetPageSizeWithRotation(1);

        Document myDoc = new Document(PageSize.A4);

        //1cm == 28.3pt
      
      

        // open the writer
        FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);

        PdfWriter writer = PdfWriter.GetInstance(myDoc, fs);
        myDoc.Open();

        // the pdf content
        //PdfWriter pw = writer.DirectContent;
        PdfContentByte cb = writer.DirectContent;

        double[] koor = new double[12];
        koor[0] = 0;
        koor[1] = 79.05;
        koor[2] = 101.25;
        koor[3] = 119.36;
        koor[4] = 190.3;
        koor[5] = 206.7;
        koor[6] = 220.9;
        koor[7] = 275.47;
        koor[8] = 303.11;
        koor[9] = 326.9;
        koor[10] = 352.7;
        koor[11] = 377.02;

        double odHora = 173;


        //string lila = "hura";
        BaseFont mojFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        cb.SetFontAndSize(mojFont,10);

        //cb.SetColorStroke(new CMYKColor(1f, 0f, 0f, 0f));
        //cb.SetColorFill(new CMYKColor(0f, 0f, 1f, 0f));

        cb.SetColorStroke(BaseColor.LIGHT_GRAY);
        cb.SetColorFill(BaseColor.LIGHT_GRAY);

        string[] freeDays = x_db.getFreeDays();

        

        int days = DateTime.DaysInMonth(rok, mesiac);
        double kof = 12.1;
        for (int i = 0; i < days; i++)
        {
            int den = i + 1;
            /*if (i == 1)
            {
                cb.Rectangle(46, size.Height-(float)odHora, 10, 10);
                cb.Fill();
            }*/

            string mesDen = den.ToString() + "." + mesiac.ToString();

            int rs_tmp = Array.IndexOf(freeDays, mesDen);

            DateTime my_date = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), den);
            int dnesJe = (int)my_date.DayOfWeek;

            if (dnesJe == 0 || dnesJe == 6 || rs_tmp != -1)
            {
                //173.22
                //vyska stlpca je 11
                //od lava 46
                //dlzka je 423.7
                
               /* cb.MoveTo(46, size-odHora +(11*i));
                cb.LineTo(469, size - odHora + (11 * i));
                cb.LineTo(469, size - odHora + (11 * i)-11);
                cb.LineTo(46, size - odHora + (11 * i) - 11);
                //Path closed, stroked and filled
                cb.ClosePathFillStroke();*/
                double recY = (size.Height - (odHora+1)) - (kof * i);

                float recYY = (float)recY;

                cb.Rectangle(46, recYY, 405, 11);
                //cb.Stroke();
                cb.Fill();
            }


        }
        cb.SetColorStroke(BaseColor.BLACK);
        cb.SetColorFill(BaseColor.BLACK);

        cb.BeginText();
        cb.MoveText(248, size.Height - 41);
        cb.ShowText(this.mesiac_cb.SelectedItem.ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(362, size.Height - 41);
        cb.ShowText(rok.ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(446, size.Height - 41);
        cb.ShowText("KDCH");
        cb.EndText();
        
        cb.BeginText();
        cb.MoveText(121, size.Height - 60);
        cb.ShowText(Session["titul_pred"].ToString()+Session["fullname"].ToString()+" "+Session["titul_za"].ToString());
        cb.EndText();

        cb.BeginText();
        cb.MoveText(375, size.Height - 60);
        cb.ShowText(Session["zaradenie"].ToString());
        cb.EndText();


        cb.BeginText();

        string osobcisl = my_x2.getStr(Session["osobcisl"].ToString());
        cb.MoveText(517, size.Height - 66);

        if (osobcisl.Length > 0)
        {

            cb.ShowText(osobcisl);
        }
        else
        {
            cb.ShowText("");
        }
        cb.EndText();


        
        cb.BeginText();
        cb.MoveText(121, size.Height - 100);
        String tyzdoba = my_x2.getStr(Session["tyzdoba"].ToString());

        if (tyzdoba.Length > 0)
        {
            tyzdoba = tyzdoba.Replace(',', '.');
            cb.ShowText(tyzdoba);
        }
        else
        {
            cb.ShowText("37.5");
        }
        
        cb.EndText();

        //pocet hodin dla dni
        cb.BeginText();
        cb.MoveText(278, size.Height - 100);
        cb.ShowText(this.pocetHod_txt.Text);
        cb.EndText();
        //pocet odrobenych hodin
        cb.BeginText();
        cb.MoveText(115, size.Height - 564);
        cb.ShowText(this.hodiny_lbl.Text);
        cb.EndText();

        //pocet nocna praca
        cb.BeginText();
        cb.MoveText(184, size.Height - 564);
        cb.ShowText(this.nocpraca_lbl.Text);
        cb.EndText();

        //pocet mzdove zvyhodneie
        cb.BeginText();
        cb.MoveText(206, size.Height - 564);
        cb.ShowText(this.mzdovzvyh_lbl.Text);
        cb.EndText();

        //pocet sviatok
        cb.BeginText();
        cb.MoveText(231, size.Height - 564);
        cb.ShowText(this.sviatok_lbl.Text);
        cb.EndText();

        //mala aktivna
        cb.BeginText();
        cb.MoveText(275, size.Height - 564);
        cb.ShowText(this.a1_lbl.Text);
        cb.EndText();

        //mala neaktivna
        cb.BeginText();
        cb.MoveText(303, size.Height - 564);
        cb.ShowText(this.a2_lbl.Text);
        cb.EndText();

        //velka neaktivna
        cb.BeginText();
        cb.MoveText(303, size.Height - 564);
        cb.ShowText(this.a2_lbl.Text);
        cb.EndText();

        //velka aktivna
        cb.BeginText();
        cb.MoveText(326, size.Height - 564);
        cb.ShowText(this.nea1_lbl.Text);
        cb.EndText();

        //velka aktivna
        cb.BeginText();
        cb.MoveText(352, size.Height - 564);
        cb.ShowText(this.nea2_lbl.Text);
        cb.EndText();

        //velka aktivna
        cb.BeginText();
        cb.MoveText(377, size.Height - 564);
        cb.ShowText(this.nea3_lbl.Text);
        cb.EndText();

        //prevod
        cb.BeginText();
        cb.MoveText(115, size.Height - 584);
        cb.ShowText(this.rozdiel_lbl.Text);
        cb.EndText();

        //prenos
        cb.BeginText();
        cb.MoveText(119, size.Height - 160);
        cb.ShowText(this.predMes_txt.Text);
        cb.EndText();



        /*this.nocpraca_lbl.Text = this.getColumSum(4, days, "textBox_").ToString();
        this.mzdovzvyh_lbl.Text = this.getColumSum(5, days, "textBox_").ToString();
        this.sviatok_lbl.Text = this.getColumSum(6, days, "textBox_").ToString();
        this.a1_lbl.Text = this.getColumSum(7, days, "textBox_").ToString();
        this.a2_lbl.Text = this.getColumSum(8, days, "textBox_").ToString();
        this.nea1_lbl.Text = this.getColumSum(9, days, "textBox_").ToString();
        this.nea2_lbl.Text = this.getColumSum(10, days, "textBox_").ToString();
        this.nea3_lbl.Text = this.getColumSum(11, days, "textBox_").ToString();*/
     

        
        float ypos = 0;
       
       
        for (int i = 0; i < days; i++)
        {
           
           if (i == 0)
           {
                ypos = size.Height - (float)odHora;
           }
           else
           {
                ypos = ypos - (float)kof;
           }

           ContentPlaceHolder ctpl = new ContentPlaceHolder();
           Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

           ctpl = (ContentPlaceHolder)tmpControl;
           
            
            for (int j = 0; j < 12; j++)
            {
                Control Tbox = ctpl.FindControl("textBox_" + i.ToString() + "_" + j.ToString());
                TextBox mBox = (TextBox)Tbox;
                cb.BeginText();
                if (j > 0 )
                {
                    string num = mBox.Text.ToString();
                    cb.MoveText((float)koor[j], ypos);
                    cb.ShowText(num);
                     
                }
                cb.EndText();
            }
        }
        
        PdfImportedPage page = writer.GetImportedPage(reader, 1);
        cb.AddTemplate(page,0,0);
        

        myDoc.Close();
        fs.Close();
        writer.Close();
        reader.Close();

        //Response.Redirect(@path + "\\vykaz_new.pdf");
        SortedList res = x_db.registerTempFile("vykaz_"+hash+".pdf", 5);
        

        if (res["status"].ToString() == "ok")
        {

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=vykaz_" + hash + ".pdf");
            Response.TransmitFile(@path + "\\vykaz_" + hash + ".pdf");
            Response.End();
        }
        else
        {
            this.msg_lbl.Text = ",,,,,,="+ res["message"].ToString();
        }


        //


        
        //my_x2.createVykazPdf(path, imagepath);
 
    }

    protected float cmPt(double number)
    {
        string unit = "28,3464";
        double res = number * Convert.ToDouble(unit);

        float result = (float)res;

        return result;
    }

}
