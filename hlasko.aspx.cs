using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class hlasko : System.Web.UI.Page
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();
    mysql_db x2MySQL = new mysql_db();

    // protected System.Web.UI.HtmlControls.HtmlGenericControl hlavicka;
    protected void Page_Init(object sender, EventArgs e)
    {
        //hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript' src='tinymce/jscripts/tiny_mce/tiny_mce.js'></script>"));
        // hlavicka.Controls.Add(new LiteralControl("<script type='text/javascript'>tinyMCE.init({mode : 'textareas',        force_br_newlines : true,        force_p_newlines : false});</script>"));
    }


    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        Page.Header.DataBind();

        this.msg_lbl.Text = "";
        this.hlasko_pl.Visible = false;
        this.kompl_work_time.Text = "";
       // Response.AppendHeader("Refresh", 6000 + "; URL=hlasko.aspx");


        if (IsPostBack == false)
        {
            // Calendar1.SelectedDate = DateTime.Today;
            this.setMyDate();
            
            this.loadHlasko();
            this.setEPC_init();

        }
        else
        {
            //this.setMyDate();
            // this.setEPC_init();
            this.loadEPCData();
        }

        /*if (IsCallback)
        {
            this.msg_lbl1.Text="test";
        }*/



    }

    protected void setEPC_init()
    {
        
        this.hl_datum_cb.Items.Clear();

        ListItem[] datum = new ListItem[3];

        DateTime now = Convert.ToDateTime(this.Calendar1.SelectedDate);
        int hour = now.Hour;
        int mint =now.Minute;
       
        
        datum[0] = new ListItem(now.ToShortDateString(),now.ToShortDateString());
        datum[1] = new ListItem(now.AddDays(-1).ToShortDateString(), now.AddDays(-1).ToShortDateString());
        datum[2] = new ListItem(now.AddDays(1).ToShortDateString(), now.AddDays(1).ToShortDateString());
        this.hl_datum_cb.Items.AddRange(datum);

       this.jsWorkstarttxt.Text = DateTime.Now.ToString("HH:mm");

       this.loadEPCData();
       

    
    }

    protected void uploadData_fnc(object sender, EventArgs e)
    {

        if (this.loadFile_fup.HasFile)
        {
            try
            {
                SortedList dataFile = new SortedList();
                string fileEx = System.IO.Path.GetExtension(this.loadFile_fup.FileName);
                byte[] dataB =  new byte[this.loadFile_fup.PostedFile.InputStream.Length];
                this.loadFile_fup.PostedFile.InputStream.Read(dataB,0,this.loadFile_fup.PostedFile.ContentLength);
                dataFile.Add("file-name", this.loadFile_fup.FileName.ToString());
                dataFile.Add("file-size", this.loadFile_fup.PostedFile.InputStream.Length);
                dataFile.Add("file-type", fileEx);
                dataFile.Add("file-content", Convert.ToBase64String(dataB));
                
                SortedList res = x2MySQL.mysql_insert("is_data", dataFile);

                if (!Convert.ToBoolean(res["status"]))
                {
                    this.msg_lbl.Text = this.loadFile_fup.PostedFile.FileName + "<br><br>" + res["msg"].ToString();
                }
                else
                {

                    this.loadFile_fup.Visible = false;
                    this.upLoadFile_btn.Visible = false;

                    this.lfId_hidden.Value = res["last_id"].ToString();
                    this.upLoadedFile_lbl.Text = "<a href='lf.aspx?id=" + res["last_id"].ToString() + "' target='_blank'>" + this.loadFile_fup.FileName.ToString() + "</a>";
                }
            }
            catch (Exception ex)
            {
                
                this.msg_lbl.Text = this.loadFile_fup.PostedFile.FileName + "<br><br>" + ex.ToString();
            }

        }

        
    }

    //protected void checkCorrectTime_fnc(object sender, EventArgs e)
    //{
    //    //this.msg_lbl.Text = this.workstart_txt.Text;

    //    string timeStr = this.workstart_txt.Text;
    //    DateTime time;

    //    if (!DateTime.TryParse(timeStr, out time))
    //    {
    //        this.time_valid_msg.Visible = true;
    //        this.activitysave_btn.Enabled = false;
    //    }
    //    else
    //    {
    //        this.time_valid_msg.Visible = false;
    //        this.activitysave_btn.Enabled = true;
    //        this.loadEPCData();
    //    }


    //}

    protected void checkCorrectMinutes_fnc(object sender, EventArgs e)
    {
        string minuteStr = this.jsWorktimetxt.Text;
        int minutes;

        if (!Int32.TryParse(minuteStr, out minutes))
        {
            this.minute_valid_msg.Visible = true;
            this.activitysave_btn.Enabled = false;
        }
        else
        {
            this.minute_valid_msg.Visible = false;
            this.activitysave_btn.Enabled = true;
            this.loadEPCData();
        }
    }

    protected void saveActivity_fnc(object sender, EventArgs e)
    {

        SortedList data = new SortedList();
        data.Add("user_id", Session["user_id"].ToString());
        data.Add("hlasko_id", Session["akt_hlasenie"].ToString());

        DateTime dateTmp = Convert.ToDateTime(this.hl_datum_cb.SelectedValue.ToString());

        data.Add("work_start", my_x2.unixDate(dateTmp) + " " + this.jsWorkstarttxt.Text.ToString());
        data.Add("work_time", this.jsWorktimetxt.Text.ToString());
        data.Add("work_type", this.worktype_cb.SelectedValue.ToString());
        data.Add("patient_name", this.patientname_txt.Text.ToString());
        data.Add("work_text", my_x2.EncryptString(this.activity_txt.Text.ToString(),Session["passphrase"].ToString()));
        data.Add("osirix", this.check_osirix.Checked);

        if (lfId_hidden.Value.ToString() != "0")
        {
            data.Add("lf_id", this.lfId_hidden.Value.ToString());
        }

        if (Session["epc_id"] == null)
        {

            SortedList res = x2MySQL.mysql_insert("is_hlasko_epc", data);

            Boolean status = Convert.ToBoolean(res["status"].ToString());

            if (!status)
            {
                this.msg_lbl.Text = res["msg"].ToString() + "<br>" + res["sql"].ToString();
            }
            else
            {
                this.loadEPCData();
                this.clearEpcData();
                this._generateHlasko();
            }
        }
        else
        {
            SortedList resUp = x2MySQL.mysql_update("is_hlasko_epc", data, Session["epc_id"].ToString());

            if (Convert.ToBoolean(resUp["status"]) == true)
            {
                Session.Remove("epc_id");
                this.loadEPCData();
                this.clearEpcData();
                this._generateHlasko();
                
            }
            else
            {
                this.msg_lbl.Text = resUp["msg"].ToString();
            }
        }
    }

    protected void clearEpcData()
    {
        this.jsWorkstarttxt.Text = DateTime.Now.ToString("HH:mm");
        this.jsWorktimetxt.Text = "15";
        this.patientname_txt.Text = "";
        this.activity_txt.Text = "";
        this.activitysave_btn.Text = Resources.Resource.add;
        this.lfId_hidden.Value = "0";
        
    }

    protected int allWorkTime()
    {
        int result = 0;
        SortedList res = x2MySQL.getRow("SELECT SUM([work_time]) AS [workminutes] FROM [is_hlasko_epc] WHERE [hlasko_id]='" + Session["akt_hlasenie"].ToString() + "'");

        result = Convert.ToInt32(res["workminutes"]);
        
        return result;
    }

    protected void loadEPCData()
    {
      

        this.activity_tbl.Controls.Clear();
        this.activity_tbl.Visible = true;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_hlasko_epc] WHERE [hlasko_id] ='{0}' ORDER BY [work_start] ASC", Session["akt_hlasenie"]);
        Dictionary<int, Hashtable> table = x2MySQL.getTable(sb.ToString());



        int tableLn = table.Count;
        //this.msg_lbl.Text = Session["akt_hlasenie"].ToString();

        TableHeaderRow headRow = new TableHeaderRow();
        TableHeaderCell headCell1 = new TableHeaderCell();
        headCell1.Text = "Akcia";
        headRow.Controls.Add(headCell1);

        TableHeaderCell headCell2 = new TableHeaderCell();
        headCell2.Text = "Typ prace";
        headRow.Controls.Add(headCell2);

        TableHeaderCell headCell3 = new TableHeaderCell();
        headCell3.Text = "Zaciatok prace";
        headRow.Controls.Add(headCell3);

        TableHeaderCell headCell4 = new TableHeaderCell();
        headCell4.Text = "Trvanie prace";
        headRow.Controls.Add(headCell4);

        TableHeaderCell headCell5 = new TableHeaderCell();
        headCell5.Text = "Priezvisko pacienta";
        headRow.Controls.Add(headCell5);

        TableHeaderCell headCell6 = new TableHeaderCell();
        headCell6.Text = "Popis prace";
        headRow.Controls.Add(headCell6);

        TableHeaderCell headCell7 = new TableHeaderCell();
        headCell7.Text = "OSIRIX";
        headRow.Controls.Add(headCell7);

        this.activity_tbl.Controls.Add(headRow);


        for (int i = 0; i < tableLn; i++)
        {
            TableRow riadok = new TableRow();
           

            TableCell celaAction = new TableCell();
            celaAction.ID = "celaAction_" + table[i]["id"].ToString();
            //celaTyp.Text = table[i]["work_type"].ToString();
            Button editBtn = new Button();
            editBtn.ID = "editBtn_" + table[i]["id"].ToString();
            editBtn.Click += new EventHandler(clickEPC_fnc);
            editBtn.CssClass = "button green";
            editBtn.Text = "Edituj";
            celaAction.Controls.Add(editBtn);

            Button delBtn = new Button();
            delBtn.ID = "delBtn_" + table[i]["id"].ToString();
            delBtn.Click += new EventHandler(clickEPC_fnc);
            delBtn.CssClass = "button red";
            delBtn.Text = "Zmaz";
            celaAction.Controls.Add(delBtn);

            riadok.Controls.Add(celaAction);

            TableCell celaTyp = new TableCell();
            celaTyp.Text = table[i]["work_type"].ToString();
            riadok.Controls.Add(celaTyp);

            TableCell celaCas = new TableCell();
            celaCas.Text = my_x2.UnixToMsDateTime(table[i]["work_start"].ToString()).ToString();
            riadok.Controls.Add(celaCas);

            TableCell celaCas2 = new TableCell();
            celaCas2.Text = table[i]["work_time"].ToString();
            riadok.Controls.Add(celaCas2);

            TableCell celaMeno = new TableCell();
            celaMeno.Text = table[i]["patient_name"].ToString();
            riadok.Controls.Add(celaMeno);

            TableCell celaPopis = new TableCell();
            celaPopis.Text = my_x2.DecryptString(table[i]["work_text"].ToString(), Session["passphrase"].ToString());
            riadok.Controls.Add(celaPopis);

            TableCell osirixCell = new TableCell();
            CheckBox ch_osirix = new CheckBox();
            ch_osirix.Checked = Convert.ToBoolean(table[i]["osirix"]);
            ch_osirix.Enabled = false;
            osirixCell.Controls.Add(ch_osirix);

            riadok.Controls.Add(osirixCell);

            this.activity_tbl.Controls.Add(riadok);

            if (Session["hlasko_status"] != null)
            {
                if (Session["hlasko_status"].ToString() == "generated")
                {
                    this.hlasko_pl.Visible = true;
                }
                else
                {
                    this.hlasko_pl.Visible = true;
                }
            }

            int workmin = this.allWorkTime();
            if (workmin > -1)
            {
                this.kompl_work_time.Text = "Cas prace v minutach: " + workmin.ToString() + "(min), v hodinach: " + (workmin / 60).ToString()+"(hod)";
            }

        }

       // this.clearEpcData();
    }

    protected void clickEPC_fnc(object sender, EventArgs e)
    {
        Button epcBtn = (Button)sender;
       // string id = epcBtn.ID.ToString();
       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        

        string[] objId = epcBtn.ID.ToString().Split('_');
        //Control controlList = ctpl.FindControl("ddl_" + tmp[1] + "_" + tmp[2]);
        this.msg_lbl.Text = epcBtn.ID.ToString();

        StringBuilder sb = new StringBuilder();
       // this.msg_lbl.Text ="...."+ objId[0];

        if (objId[0] == "editBtn")
        {
            sb.Length = 0;
            sb.AppendFormat("SELECT * FROM [is_hlasko_epc] WHERE [id] = '{0}'",objId[1]);
            SortedList row = x2MySQL.getRow(sb.ToString());

            //this.msg_lbl.Text = my_x2.UnixToMsDateTime(row["work_start"].ToString()).ToString("d.M.yyyy");
            this.hl_datum_cb.SelectedValue = my_x2.UnixToMsDateTime(row["work_start"].ToString()).ToString("d. M. yyyy");
            this.jsWorkstarttxt.Text = my_x2.UnixToMsDateTime(row["work_start"].ToString()).ToString("HH:mm");
            this.jsWorktimetxt.Text = row["work_time"].ToString();
            this.worktype_cb.SelectedValue = row["work_type"].ToString();
            this.patientname_txt.Text = row["patient_name"].ToString();
            this.activity_txt.Text = my_x2.DecryptString(row["work_text"].ToString(),Session["passphrase"].ToString());
            this.check_osirix.Checked = Convert.ToBoolean(row["osirix"].ToString());

            if (row["lf_id"].ToString() != "0")
            {
                this.lfId_hidden.Value = row["lf_id"].ToString();
            }
            else
            {
                this.lfId_hidden.Value = "0";
            }

            this.activitysave_btn.Text = Resources.Resource.save;

            Session.Add("epc_id", row["id"].ToString());
           
            this.loadEPCData();
            //this._generateHlasko();
            
            
        }
        if (objId[0] == "delBtn")
        {
            sb.Length = 0;
            sb.AppendFormat("DELETE FROM [is_hlasko_epc] WHERE [id] = '{0}'", objId[1]);
            x2MySQL.execute(sb.ToString());

            this.loadEPCData();
            //this._generateHlasko();
        }


    }

    protected void setMyDate()
    {
        DateTime now = DateTime.Now;

        int hour = now.Hour;
        //msg_lbl.Text = "hod:" + hour.ToString();

        if (hour >= 9)
        {
            Calendar1.SelectedDate = DateTime.Today;


        }
        else
        {
            Calendar1.SelectedDate = DateTime.Today.AddDays(-1);
            //  msg_lbl.Text += "tu smr";
        }

    }

    protected void generateHlasko_fnc(object sender, EventArgs e)
    {
        this._generateHlasko(); 
    }

    protected void _generateHlasko()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_hlasko_epc] WHERE [hlasko_id]='{0}' ORDER BY [work_type] ASC", Session["akt_hlasenie"]);

        Dictionary<int, Hashtable> table = x2MySQL.getTable(sb.ToString());

        //Dictionary<SortedList, ArrayList> report;

        int tableLn = table.Count;
        string prijem = "<p><strong>Prijem</strong><ul>";
        string operacie = "<p><strong>Operovani</strong><ul>";
        string konzilia = "<p><strong>Konzilia</strong><ul>";
        string sledovanie = "<p><strong>Sledovani</strong><ul>";
        string dekurz = "<p><strong>Dekurzovanie</strong><ul>";
        string vizita = "<p><strong>Vizita</strong><ul>";
        string osirix = "";

        for (int i = 0; i < tableLn; i++)
        {
            string acText = my_x2.DecryptString(table[i]["work_text"].ToString(), Session["passphrase"].ToString());
            if (table[i]["work_type"].ToString() == "prijem")
            {
                prijem += "<li><strong>" + table[i]["patient_name"].ToString() + "</strong>," + acText + "</li>";
            }
            if (table[i]["work_type"].ToString() == "operac")
            {
                operacie += "<li><strong>" + table[i]["patient_name"].ToString() + "</strong>," + acText + "</li>";
            }
            if (table[i]["work_type"].ToString() == "konzil")
            {
                konzilia += "<li><strong>" + table[i]["patient_name"].ToString() + "</strong>," + acText + "</li>";
            }
            if (table[i]["work_type"].ToString() == "sledov")
            {
                sledovanie += "<li><strong>" + table[i]["patient_name"].ToString() + "</strong>," + acText + "</li>";
            }
            if (table[i]["work_type"].ToString() == "dekurz")
            {
                dekurz += "<li><strong>" + table[i]["patient_name"].ToString() + "</strong>," + acText + "</li>";
            }
            if (table[i]["work_type"].ToString() == "vizita")
            {
                vizita += "<li><strong>" + table[i]["patient_name"].ToString() + "</strong>," + acText + "</li>";
            }

            if (Convert.ToBoolean(table[i]["osirix"]))
            {
                osirix += table[i]["patient_name"].ToString() + "\r\n";
            }
        }
        prijem += "</ul></p>";
        operacie += "</ul></p>";
        konzilia += "</ul></p>";
        sledovanie += "</ul></p>";
        dekurz += "</ul></p>";
        vizita += "</ul></p>";

        this.hlasko_pl.Visible = true;
        this.hlasenie.Text = prijem + operacie + konzilia + sledovanie+dekurz+vizita;
        this.osirix_txt.Text = osirix;

        this.saveGenerated();
        this.clearEpcData();
    }


    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        //user.Text = Calendar1.SelectedDate.ToString();
        //last_user.Text = Request.Cookies["user_id"].Value.ToString();
        //this.Calendar1.SelectedDate = this.Calendar1.SelectedDate;
        this.loadHlasko();
        this.setEPC_init();

    }

    protected void loadHlasko()
    {
        //msg_lbl.Text = Calendar1.SelectedDate.ToString();

        SortedList data = x_db.getHlasko(this.Calendar1.SelectedDate, this.hlas_type.SelectedValue.ToString(), Session["user_id"].ToString());



        if (Convert.ToInt32(data["id"]) != 0)
        {
            if (data["status"].ToString() == "normal")
            {

                this.hlasko_pl.Visible = true;
            }
            else
            {
                this.hlasko_pl.Visible = true;
            }
            this.osirix_txt.Text = data["osirix"].ToString();

            if (data["uzavri"].ToString() == "1")
            {
                this.send.Visible = false;
                this.hlasenie.Visible = false;
                this.dodatok.Visible = true;
                this.def_lock_btn.Visible = false;
                this.def_locl_w_btn.Visible = false;
                this.addInfo_btn.Enabled = true;
                this.view_hlasko.Visible = true;
                this.hlasko_lbl.Visible = true;
                this.epc_pl.Visible = false;
                if (data["encrypt"].ToString() == "yes")
                {
                    this.view_hlasko.Text = my_x2.DecryptString(data["text"].ToString(), Session["passphrase"].ToString());
                }
                else
                {
                    this.view_hlasko.Text = data["text"].ToString();
                }

                // osirix_txt = data["osirix"].ToString();
                this.send.Enabled = false;
            }
            else
            {
                this.send.Visible = true;
                this.hlasenie.Visible = true;
                this.dodatok.Visible = false;
                this.def_lock_btn.Visible = true;
                this.def_locl_w_btn.Visible = true;
                this.view_hlasko.Visible = false;
                this.addInfo_btn.Enabled = false;
                this.send.Enabled = true;
                this.hlasko_lbl.Visible = false;
                this.epc_pl.Visible = true;
                if (data["encrypt"].ToString() == "yes")
                {
                    this.view_hlasko.Text = my_x2.DecryptString(data["text"].ToString(), Session["passphrase"].ToString());
                }
                else
                {
                    this.view_hlasko.Text = data["text"].ToString();
                }
            }
            //SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString();
            this.user.Text = Session["fullname"].ToString();

           

            if (data["encrypt"].ToString() == "no")
            {
                this.hlasenie.Text = data["text"].ToString();
            }
            else
            {
                this.hlasenie.Text = my_x2.DecryptString(data["text"].ToString(), Session["passphrase"].ToString());
            }
            SortedList my_last_user = new SortedList();
            my_last_user = x_db.getUserInfoByID("is_users", data["last_user"].ToString());
            this.last_user.Text = my_last_user["full_name"].ToString();
            Session.Add("akt_hlasenie", data["id"].ToString());

            Session.Add("akt_hlasenie_creat_user", data["creat_user"].ToString());
            Session.Add("akt_hlasenie_last_user", data["last_user"].ToString());
            Session.Add("hlasko_status",data["status"].ToString());
        }
        else
        {
            SortedList newData = new SortedList();
            newData.Add("type", this.hlas_type.SelectedValue.ToString());
            newData.Add("dat_hlas", my_x2.unixDate(this.Calendar1.SelectedDate));
            newData.Add("text", my_x2.EncryptString(Resources.Resource.odd_hlasko_html.ToString(),Session["passphrase"].ToString()));
            newData.Add("creat_user", Session["user_id"].ToString());
            newData.Add("last_user", Session["user_id"].ToString());
            newData.Add("encrypt","yes");
            SortedList res = x2MySQL.mysql_insert("is_hlasko", newData);

            Boolean status = Convert.ToBoolean(res["status"]);

            if (status == true)
            {
                Session.Add("akt_hlasenie", res["last_id"].ToString());
                this.hlasenie.Text = my_x2.DecryptString(newData["text"].ToString(),Session["passphrase"].ToString());
                this.user.Text = Session["fullname"].ToString();
                this.last_user.Text = Session["fullname"].ToString();
                Session.Add("hlasko_status", "normal");
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }

        }


        this.generateOsirix();

    }
    /// <summary>
    /// Ulozi hlasenie, ak sa fnc zavola s 1 ulozi a uzavrie akt. hlasenie potom je mozne pisat len dodatky.. Nula urobi len save
    /// </summary>
    /// <param name="uzavri"></param>
    protected void saveData(bool uzavri, bool callBack)
    {
        SortedList data = new SortedList();
        data.Add("dat_hlas", my_x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("text", my_x2.EncryptString(hlasenie.Text.ToString(),Session["passphrase"].ToString()));
        data.Add("last_user", Session["user_id"].ToString());
        data.Add("creat_user", 0);
        data.Add("type", this.hlas_type.SelectedValue.ToString());
        data.Add("encrypt","yes");
        if (uzavri == true)
        {
            data.Add("uzavri", "1");
        }

        //string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());

        SortedList res = x2MySQL.mysql_insert("is_hlasko", data);

        SortedList my_last_user = new SortedList();
        my_last_user = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);

        if (status == true)
        {
            //msg_lbl.Text = res;
            last_user.Text = my_last_user["full_name"].ToString();
            this.generateOsirix();
        }
        else
        {
            msg_lbl.Text = res["msg"].ToString();
        }
    }

    protected void saveGenerated()
    {
        SortedList data = new SortedList();
        data.Add("dat_hlas", my_x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("text", my_x2.EncryptString(hlasenie.Text.ToString(),Session["passphrase"].ToString()));
        data.Add("last_user", Session["user_id"].ToString());
        data.Add("creat_user", 0);
        data.Add("type", this.hlas_type.SelectedValue.ToString());
        data.Add("status", "generated");
        data.Add("encrypt","yes");
        //if (uzavri == true)
        //{
        //    data.Add("uzavri", "1");
        //}

        //string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());

        SortedList res = x2MySQL.mysql_insert("is_hlasko", data);

        SortedList my_last_user = new SortedList();
        my_last_user = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);

        if (status == true)
        {
            //msg_lbl.Text = res;
            last_user.Text = my_last_user["full_name"].ToString();
            this.generateOsirix();
            
        }
        else
        {
            msg_lbl.Text = res["msg"].ToString();
        }
    }

    protected void saveDodatok(string my_dodatok)
    {
        SortedList data = new SortedList();
        SortedList my_last_user = new SortedList();
        data.Add("text", my_x2.EncryptString(my_dodatok,Session["passphrase"].ToString()));
        data.Add("last_user", 0);
        data.Add("dat_hlas", my_x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("creat_user", 0);
        data.Add("type", this.hlas_type.SelectedValue.ToString());

        SortedList res = x2MySQL.mysql_insert("is_hlasko", data);
        my_last_user = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);


        if (status == true)
        {
            //msg_lbl.Text = res;
            //last_user.Text = my_last_user["full_name"].ToString();
        }
        else
        {
            msg_lbl.Text = res["msg"].ToString();
        }
    }



    protected void send_Click(object sender, EventArgs e)
    {

        this.saveData(false, false);

    }

    /*protected SortedList getSluzbyByDen(int den)
    {
        SortedList result = new SortedList();
        DateTime dnesJe = DateTime.Today;

        SortedList tmp = x_db.loadSluzbaMonthYear("is_sluzby", dnesJe.Month.ToString(), dnesJe.Year.ToString());

        string[][] data = my_x2.parseSluzba(tmp["rozpis"].ToString());

        // int den = dnesJe.Day;

        result.Add("OUP", data[den - 1][1].ToString());
        result.Add("OddA", data[den - 1][2].ToString());
        result.Add("OddB", data[den - 1][3].ToString());
        result.Add("OP", data[den - 1][4].ToString());
        // result.Add("TRP", data[den - 1][5].ToString());

        return result;

    }*/


    protected void hlas_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.loadHlasko();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.send_Click(sender, e);
        Session.Add("hlasko_date", this.Calendar1.SelectedDate);
        Session.Add("hlasko_toWord", false);
        Response.Redirect("print.aspx");
    }

    protected void toWord_Click(object sender, EventArgs e)
    {
        this.send_Click(sender, e);
        Session.Add("hlasko_date", this.Calendar1.SelectedDate);
        Session.Add("hlasko_toWord", true);
        Response.Redirect("print.aspx");
    }

    protected void pdfCretae_btn_Click(object sender, EventArgs e)
    {
       // Session.Add("pdf", "print");
       // Response.Redirect("print.aspx?den=" + Calendar1.SelectedDate.Day.ToString() + "&datum=" + Calendar1.SelectedDate.ToLongDateString() + "&m=" + Calendar1.SelectedDate.Month.ToString());
    }

    protected void def_lock_btn_Click(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (hlasenie.Visible == true)
        {
            this.saveData(true, false);
            Session.Add("hlasko_date", this.Calendar1.SelectedDate);
            Session.Add("hlasko_toWord", false);
            Response.Redirect("print.aspx");
        }
        else
        {
            string tmp_hlasko = view_hlasko.Text;
            tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
            tmp_hlasko += "<br>" + dodatok.Text;
            this.saveDodatok(tmp_hlasko);

            Session.Add("hlasko_date", this.Calendar1.SelectedDate);
            Session.Add("hlasko_toWord", false);

            Response.Redirect("print.aspx");
        }

    }

    protected void def_lock_btn_w_Click(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        if (hlasenie.Visible == true)
        {
            this.saveData(true, false);
            Session.Add("hlasko_date", this.Calendar1.SelectedDate);
            Session.Add("hlasko_toWord", true);

            Response.Redirect("print.aspx");
        }
        else
        {
            string tmp_hlasko = view_hlasko.Text;
            tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
            tmp_hlasko += "<br>" + dodatok.Text;
            this.saveDodatok(tmp_hlasko);
            Session.Add("hlasko_date", this.Calendar1.SelectedDate);
            Session.Add("hlasko_toWord", true);
            Response.Redirect("print.aspx");
        }

    }

    

    protected void addInfo_btn_Click(object sender, EventArgs e)
    {
        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());

        string tmp_hlasko = view_hlasko.Text;
        tmp_hlasko += "***********Dodatok*******" + DateTime.Now.ToLongDateString() + "...." + akt_user_info["full_name"].ToString();
        tmp_hlasko += "<br>" + dodatok.Text;
        this.saveDodatok(tmp_hlasko);
        this.loadHlasko();

    }
    protected void generateOsirix()
    {
        string text = this.osirix_txt.Text.ToString();



        string asciiTxt = x2_var.UTFtoASCII(text.Trim());


        SortedList data = new SortedList();

        if (asciiTxt.Length > 0)
        {
            data.Add("osirix", asciiTxt);
            string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());
            string html = "";
            if (res == "ok")
            {
                //this.osirix_url.Text = asciiTxt.ToString();

                string[] lines = this.returnStrArray(asciiTxt.ToString());
                foreach (string line in lines)
                {
                    html += "<p class='align-center'><a class='blue button' href='http://10.10.2.49:3333/studyList?search=" + line + "' target='_blank' >" + line.ToUpper() + "</a></p>";
                }

                this.osirix_url.Text = html.ToString();

            }
            else
            {
                this.msg_lbl.Text = res.ToString();

            }
        }
        else
        {
            this.osirix_url.Text = "";
        }
    }

    protected void osirix_btn_Click(object sender, EventArgs e)
    {
        this.generateOsirix();
    }

    public string[] returnStrArray(string str)
    {
        string[] result = Regex.Split(str, "\r\n");
        return result;
    }
}
