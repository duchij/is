using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_druhadk_hlasko : System.Web.UI.UserControl
{
    mysql_db x2Mysql = new mysql_db();
    x2_var x2 = new x2_var();
    log x2log = new log();
    my_db x2dbold = new my_db();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["hlaskoSelTab"]!=null)
        {
            //this.msg_lbl.Text = Session["hlaskoSelTab"].ToString();
            this.hlaskoSelectedTab.Value = Session["hlaskoSelTab"].ToString();
        }

        this.tags_lit.Text = "";
       if (!IsPostBack)
        {
           
            this.setMyDate();
            if (this.setShiftTypes())
            {
                this.loadclinicDeps();
                this.setShiftForDoctor();
                this.setEPC_init();
                this.loadHlasko();
                this.loadEPCData(false);
            }
            else
            {
                this.msg_lbl.Text = "<h3 class='red'>"+Resources.Resource.no_sfihts_generated+"</h3>";
                    
            }
            
        }
        else
        {
           
            this.loadEPCData(false);
            
            
            //this.loadHlasko();
        }

        DateTime dt = Convert.ToDateTime(this.Calendar1.SelectedDate);
       
        this.date_hv.Value = x2.unixDate(dt);

        //this.setShiftTypes();
    }

    protected void setMyDate()
    {
        DateTime now = DateTime.Now;

        int hour = now.Hour;
        if (hour >= 9)
        {
            this.Calendar1.SelectedDate = DateTime.Today;
            this.hlaskoCal_cal.SelectedDate = DateTime.Today;
        }
        else
        {
            this.Calendar1.SelectedDate = DateTime.Today.AddDays(-1);
            this.hlaskoCal_cal.SelectedDate = DateTime.Today.AddDays(-1);
        }

    }
    protected void hlasko_SelectionChanged(object sender, EventArgs e)
    {
        this.Calendar1.SelectedDate = this.hlaskoCal_cal.SelectedDate;

        if (sender.GetType() == typeof(Calendar))
        {

            //this.setShiftTypes();
            this.loadHlasko();
            this.loadEPCData(false);
            this.setEPC_init();
        }
        if (sender.GetType() == typeof(DropDownList))
        {
            this.loadHlasko();
            this.loadEPCData(false);
        }


    }

    protected void showHlasko_fnc(object sender, EventArgs e)
    {

        Button btn = (Button)sender;

        string buttonId = btn.ID.ToString();

       
        switch(buttonId)
        {
            case "showOup_btn":
                this.shiftType_dl.SelectedValue = "OUP";
                this.Calendar1_SelectionChanged(this.shiftType_dl, e);
                break;
            case "showOddA_btn":
                this.shiftType_dl.SelectedValue = "OddA";
                this.Calendar1_SelectionChanged(this.shiftType_dl, e);
                break;
            case "showOddB_btn":
                this.shiftType_dl.SelectedValue = "OddB";
                this.Calendar1_SelectionChanged(this.shiftType_dl, e);
                break;
            case "showOp_btn":
                this.shiftType_dl.SelectedValue = "OP";
                this.Calendar1_SelectionChanged(this.shiftType_dl, e);
                break;
        }
        

    }

    protected void setShiftForDoctor()
    {
        DateTime dt = Convert.ToDateTime(this.Calendar1.SelectedDate);

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [typ] FROM [is_sluzby_2] WHERE [user_id] = '{0}' AND [datum]='{1}' AND [typ]!='Prijm'", Session["user_id"].ToString(), x2.unixDate(dt).ToString());

        SortedList row = x2Mysql.getRow(sb.ToString());

        if (row.Count > 0)
        {
            string type = row["typ"].ToString();

            this.shiftType_dl.SelectedValue = type;
            
        }
       
    }


    protected int calcAfter19()
    {
        int result = 0;
        DateTime dateZac = Convert.ToDateTime(this.hl_datum_cb.SelectedValue.ToString() + " " + this.jsWorkstarttxt.Text.ToString());

        DateTime dateKonc = dateZac.AddMinutes(Convert.ToInt32(this.jsWorktimetxt.Text));

        DateTime dateMore19 = Convert.ToDateTime(this.hl_datum_cb.SelectedValue.ToString() + " " + "19:00");

        int h1 = dateZac.Hour;
        int m1 = dateZac.Minute;

        int h2 = dateKonc.Hour;
        int m2 = dateKonc.Minute;

        if (h1 < 19 && h1 > 7)
        {
            if (h2 >= 19 && m2 > 0)
            {
                result = (dateKonc - dateMore19).Minutes;
            }
            if (h2 < 19)
            {
                result = 0;
            }
        }
        else if (h1 >= 0 || h1 < 8)
        {
            result = Convert.ToInt32(this.jsWorktimetxt.Text);
        }


        return result;
    }

    protected void clearEpcData()
    {
        this.jsWorkstarttxt.Text = DateTime.Now.ToString("HH:mm");
        this.jsWorktimetxt.Text = "15";
        this.patientname_txt.Text = "";
        this.activity_txt.Text = "";
        this.activitysave_btn.Text = Resources.Resource.add;
        this.lfId_hidden.Value = "0";
        this.check_osirix.Checked = false;

    }

    protected void uploadData_fnc(object sender, EventArgs e)
    {

        if (this.loadFile_fup.HasFile)
        {
            try
            {
                SortedList dataFile = new SortedList();
                string fileEx = System.IO.Path.GetExtension(this.loadFile_fup.FileName);
                byte[] dataB = new byte[this.loadFile_fup.PostedFile.InputStream.Length];
                this.loadFile_fup.PostedFile.InputStream.Read(dataB, 0, this.loadFile_fup.PostedFile.ContentLength);
                dataFile.Add("file-name", this.loadFile_fup.FileName.ToString());
                dataFile.Add("file-size", this.loadFile_fup.PostedFile.InputStream.Length);
                dataFile.Add("file-type", fileEx);
                dataFile.Add("user_id", Session["user_id"]);
                dataFile.Add("clinic_id", Session["klinika_id"]);
                // dataFile.Add("file-content", Convert.ToBase64String(dataB));

                if (this.lfId_hidden.Value.ToString() == "0")
                {
                    SortedList res = x2Mysql.lfInsertData(dataB, dataFile);

                    if (Convert.ToBoolean(res["status"]))
                    {
                        //this.msg_lbl.Text = res["last_id"].ToString();

                        this.lfId_hidden.Value = res["last_id"].ToString();
                        this.loadFile_fup.Visible = false;
                        this.upLoadFile_btn.Visible = false;
                        this.upLoadedFile_lbl.Text = "<a href='lf.aspx?id=" + res["last_id"].ToString() + "' target='_blank'>" + this.loadFile_fup.FileName.ToString() + "</a>";
                    }
                    else
                    {
                        this.msg_lbl.Text = res["msg"].ToString();
                    }
                }
                else
                {
                    dataFile.Add("id", Convert.ToInt32(this.lfId_hidden.Value));
                    SortedList res = x2Mysql.lfUpdateData(dataB, dataFile);

                    if (Convert.ToBoolean(res["status"]))
                    {
                        //this.msg_lbl.Text = res["last_id"].ToString();

                        this.lfId_hidden.Value = res["last_id"].ToString();
                        this.loadFile_fup.Visible = false;
                        this.upLoadFile_btn.Visible = false;
                        this.upLoadedFile_lbl.Text = "<a href='lf.aspx?id=" + res["last_id"].ToString() + "' target='_blank'>" + this.loadFile_fup.FileName.ToString() + "</a>";
                    }
                    else
                    {
                        this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                this.msg_lbl.Text = x2.errorMessage(this.loadFile_fup.PostedFile.FileName + "<br><br>" + ex.ToString());
            }
        }


    }

    protected void saveActivity_fnc(object sender, EventArgs e)
    {
        //this.hlaskoSelectedTab.Value = "1";

        int realTime = this.calcAfter19();
        this.jsWorktimetxt.Text = realTime.ToString();
        SortedList data = new SortedList();
        data.Add("user_id", Session["user_id"].ToString());
        data.Add("hlasko_id", Session["akt_hlasenie"].ToString());

        DateTime dateTmp = Convert.ToDateTime(this.hl_datum_cb.SelectedValue.ToString());

        string tmpDt = x2.unixDate(dateTmp) + " " + this.jsWorkstarttxt.Text.ToString();

        data.Add("work_start",tmpDt.TrimEnd());
        data.Add("work_time", this.jsWorktimetxt.Text.ToString());
        // data.Add("work_time", this.calcAfter19());
        data.Add("work_type", this.worktype_cb.SelectedValue.ToString());

        string meno = this.patientname_txt.Text.ToString().Trim();

        if (meno.Length == 0)
        {
            meno = "pacient";
        }
        
        data.Add("patient_name", meno);

        data.Add("work_text", x2.EncryptString(this.activity_txt.Text.ToString(), Session["passphrase"].ToString()));
        data.Add("osirix", this.check_osirix.Checked);
        data.Add("work_place", this.clinicDep_dl.SelectedValue);

        if (this.lfId_hidden.Value.ToString() != "0")
        {
            data.Add("lf_id", this.lfId_hidden.Value.ToString());
        }

        if (Session["epc_id"] == null)
        {

            SortedList res = x2Mysql.mysql_insert("is_hlasko_epc", data);

            Boolean status = Convert.ToBoolean(res["status"].ToString());

            if (!status)
            {
                this.msg_lbl.Text = res["msg"].ToString() + "<br>" + res["sql"].ToString();
            }
            else
            {
                this.loadEPCData(true);
                this.clearEpcData();
                this._generateHlasko();

                this.loadFile_fup.Visible = true;
                this.upLoadFile_btn.Visible = true;
                this.upLoadedFile_lbl.Text = "";

               // this.hlaskoSelectedTab.Value = "#hlasko_tab1";

            }
        }
        else
        {
            SortedList resUp = x2Mysql.mysql_update("is_hlasko_epc", data, Session["epc_id"].ToString());

            if (Convert.ToBoolean(resUp["status"]) == true)
            {
                Session.Remove("epc_id");
                this.loadEPCData(true);
                this.clearEpcData();
                this._generateHlasko();

                this.loadFile_fup.Visible = true;
                this.upLoadFile_btn.Visible = true;
                this.upLoadedFile_lbl.Text = "";

              //  this.hlaskoSelectedTab.Value = "#hlasko_tab1";

            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage(resUp["msg"].ToString());
            }
        }
    }

    protected void _generateHlasko()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [is_hlasko_epc].*,[is_deps].[label] FROM [is_hlasko_epc] ");
        sb.AppendLine("LEFT JOIN [is_deps] ON [is_deps].[idf] = [is_hlasko_epc].[work_place]");
        sb.AppendFormat("WHERE [hlasko_id]='{0}' ORDER BY [work_type] ASC", Session["akt_hlasenie"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        //Dictionary<SortedList, ArrayList> report;
        int typeCn = this.worktype_cb.Items.Count;

        string[] workType = new string[typeCn];
        string[] workIdf = new string[typeCn];
        SortedList works = new SortedList();
        for (int i = 0; i < typeCn; i++)
        {
            workType[i] = this.worktype_cb.Items[i].Text.ToString()+"<ul>||instr||</ul><br>";
            workIdf[i] = this.worktype_cb.Items[i].Value.ToString();
            
            works[workIdf[i]]="";
        }

        int tableLn = table.Count;
        /*string prijem = "<p><strong>Prijem</strong><ul>";
        string operacie = "<p><strong>Operovani</strong><ul>";
        string konzilia = "<p><strong>Konzilia</strong><ul>";
        string sledovanie = "<p><strong>Sledovani</strong><ul>";
        string dekurz = "<p><strong>Dekurzovanie</strong><ul>";
        string vizita = "<p><strong>Vizita</strong><ul>";
        string osirix = ""; */
        string osirix = "";
        for (int i = 0; i < tableLn; i++)
        {
            string acText = x2.DecryptString(table[i]["work_text"].ToString(), Session["passphrase"].ToString());
            string tmp = works[table[i]["work_type"].ToString()].ToString();
            tmp += "<li><strong>" + table[i]["patient_name"].ToString() + "</strong>, <i>(" + table[i]["label"].ToString() + ")</i> " + acText + "</li>";
            works[table[i]["work_type"].ToString()] = tmp;            

            if (Convert.ToBoolean(table[i]["osirix"]))
            {
                osirix += table[i]["patient_name"].ToString() + "\r\n";
            } 
        }

        for (int i = 0; i < typeCn; i++)
        {
            string tmp = works[workIdf[i]].ToString();
            string tmp1 = workType[i];
            tmp1 = tmp1.Replace("||instr||", tmp);
            workType[i] = tmp1;
        }


        //this.hlasko_pl.Visible = true;
        this.hlasenie.Text = string.Join("", workType);
        //this.osirix_txt.Text = osirix;

        this.saveGenerated();
        //this.clearEpcData();
    }

    protected void saveGenerated()
    {
        SortedList data = new SortedList();
        //data.Add("dat_hlas", x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("text", x2.EncryptString(this.hlasenie.Text.ToString(), Session["passphrase"].ToString()));
        data.Add("last_user", Session["user_id"].ToString());
        //data.Add("creat_user", 0);
        data.Add("type", this.shiftType_dl.SelectedValue.ToString());
        data.Add("status", "generated");
        data.Add("encrypt", "yes");
        

        SortedList res = x2Mysql.mysql_update("is_hlasko", data,Session["akt_hlasenie"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);

        if (status == true)
        {
            this.loadHlasko();
        }
        else
        {
           this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }
    }

    protected void loadHlasko()
    {
        DateTime dt = Convert.ToDateTime(this.Calendar1.SelectedDate);

        this.date_hv.Value = x2.unixDate(dt);

        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [is_hlasko].*, [users1].[full_name] AS [creatUser], [users2].[full_name] AS [lastUser] FROM [is_hlasko]");
        sb.AppendLine("INNER JOIN [is_users] AS [users1] ON [users1].[id] = [is_hlasko].[creat_user]");
        sb.AppendLine("INNER JOIN [is_users] AS [users2] ON [users2].[id] = [is_hlasko].[last_user]");
        sb.AppendFormat("WHERE [clinic]='{0}' AND [dat_hlas]='{1}' AND [type]='{2}'", Session["klinika_id"], x2.unixDate(dt), this.shiftType_dl.SelectedValue);

        SortedList row = x2Mysql.getRow(sb.ToString());

        if (row["id"] == null)
        {
            SortedList data = new SortedList();
            data.Add("type", this.shiftType_dl.SelectedValue);
            data.Add("dat_hlas", x2.unixDate(dt));
            data.Add("text", x2.EncryptString(Resources.Resource.odd_hlasko_html.ToString(), Session["passphrase"].ToString()));
            data.Add("creat_user", Session["user_id"]);
            data.Add("last_user", Session["user_id"]);
            data.Add("clinic", Session["klinika_id"]);
            data.Add("encrypt", "yes");

            SortedList result = x2Mysql.mysql_insert("is_hlasko", data);

            if (Convert.ToBoolean(result["status"]))
            {
                Session["akt_hlasenie"] = Convert.ToInt32(result["last_id"]);
                this.creatUser_lbl.Text = Session["fullname"].ToString();
                this.lastUser_lbl.Text = Session["fullname"].ToString();
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage(result["msg"].ToString());
            }

        }
        else
        {
            Session["akt_hlasenie"] = Convert.ToInt32(row["id"]);
            this.creatUser_lbl.Text = row["creatUser"].ToString();
            this.lastUser_lbl.Text = row["lastUser"].ToString();
            this.hlasenie.Text = x2.DecryptString(row["text"].ToString(), Session["passphrase"].ToString());
            //this.creatUser_lbl.Text = x2dbold.getUserInfoByID
        }

    }

    protected void setEPC_init()
    {

        this.hl_datum_cb.Items.Clear();

        ListItem[] datum = new ListItem[3]; 

        DateTime now = Convert.ToDateTime(this.Calendar1.SelectedDate);
        int hour = now.Hour;
        int mint = now.Minute;


        datum[0] = new ListItem(now.ToShortDateString(), now.ToShortDateString());
        datum[1] = new ListItem(now.AddDays(-1).ToShortDateString(), now.AddDays(-1).ToShortDateString());
        datum[2] = new ListItem(now.AddDays(1).ToShortDateString(), now.AddDays(1).ToShortDateString());
        this.hl_datum_cb.Items.AddRange(datum);

        this.jsWorkstarttxt.Text = DateTime.Now.ToString("HH:mm");

        //this.loadEPCData();



    }

    protected Boolean setShiftTypes()
    {
        Boolean res = true;
        //this.shiftType_dl.Items.Clear();
        StringBuilder sb = new StringBuilder();

        DateTime dt = this.Calendar1.SelectedDate;

        sb.AppendFormat("SELECT [typ] FROM [is_sluzby_2] WHERE [datum]='{0}' ORDER BY [ordering] ASC", x2.unixDate(dt));

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());
        int tableCn = table.Count;

        if (tableCn == 0)
        {
            res = false;
        }

        for (int i = 0; i < tableCn; i++)
        {
            this.shiftType_dl.Items.Add(new ListItem(table[i]["typ"].ToString(), table[i]["typ"].ToString()));
        }

        return res;

    }

   // protected void shiftType_SelectionChanged(object)

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        this.hlaskoCal_cal.SelectedDate = this.Calendar1.SelectedDate;
      
        if (sender.GetType() == typeof(Calendar))
        {

            //this.setShiftTypes();
            this.loadHlasko();
            this.loadEPCData(false);
            this.setEPC_init();
        }
        if (sender.GetType() == typeof(DropDownList))
        {
            this.loadHlasko();
            this.loadEPCData(false);
        }
    }

    protected void loadclinicDeps()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_deps] WHERE [clinic_id]='{0}'", Session["klinika_id"]);

        Dictionary<int, Hashtable> data = x2Mysql.getTable(sb.ToString());

        int dataCn = data.Count;
        

        for (int i = 0; i < dataCn; i++)
        {
            this.clinicDep_dl.Items.Add(new ListItem(data[i]["label"].ToString(),data[i]["idf"].ToString()));
        }
        //this.clinicDep_dl.Items.Add(new ListItem("Mimo kliniky", "mimo"));
    }

    protected void clickEPC_fnc(object sender, EventArgs e)
    {
        Button epcBtn = (Button)sender;
        // string id = epcBtn.ID.ToString();
        // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;


        string[] objId = epcBtn.ID.ToString().Split('_');
        //Control controlList = ctpl.FindControl("ddl_" + tmp[1] + "_" + tmp[2]);
        //this.msg_lbl.Text = epcBtn.ID.ToString();

        StringBuilder sb = new StringBuilder();
       

        if (objId[0] == "editBtn")
        {
           // this.msg_lbl.Text = objId[1] + "...." + objId[0];
            sb.Length = 0;
            sb.AppendFormat("SELECT * FROM [is_hlasko_epc] WHERE [id] = '{0}'", objId[1]);
            SortedList row = x2Mysql.getRow(sb.ToString());
            x2log.logData(row, "", "epc row");
            //this.msg_lbl.Text = my_x2.UnixToMsDateTime(row["work_start"].ToString()).ToString("d.M.yyyy");
            this.hl_datum_cb.SelectedValue = x2.UnixToMsDateTime(row["work_start"].ToString()).ToString("d. M. yyyy");
            this.jsWorkstarttxt.Text = x2.UnixToMsDateTime(row["work_start"].ToString()).ToString("HH:mm");
            this.jsWorktimetxt.Text = row["work_time"].ToString();
            this.worktype_cb.SelectedValue = row["work_type"].ToString();
            this.patientname_txt.Text = row["patient_name"].ToString();
            this.activity_txt.Text = x2.DecryptString(row["work_text"].ToString(), Session["passphrase"].ToString());
            this.check_osirix.Checked = Convert.ToBoolean(row["osirix"].ToString());

            if (row["lf_id"].ToString() != "NULL")
            {
                this.lfId_hidden.Value = row["lf_id"].ToString();


                this.loadFile_fup.Visible = true;
                this.upLoadFile_btn.Visible = true;

                this.upLoadedFile_lbl.Text = "<a href='lf.aspx?id=" + row["lf_id"].ToString() + "' target='_blank'>Priloha....</a>";


            }
            else
            {
                this.lfId_hidden.Value = "0";
                this.loadFile_fup.Visible = true;
                this.upLoadFile_btn.Visible = true;
            }

            this.activitysave_btn.Text = Resources.Resource.save;

            Session.Add("epc_id", row["id"].ToString());

            //this.loadEPCData();
            //this._generateHlasko();
            this.hlaskoSelectedTab.Value = "#hlasko_tab1";

        }
        if (objId[0] == "delBtn")
        {
            //this.msg_lbl.Text = objId[1].ToString();
            sb.Length = 0;
            sb.AppendFormat("DELETE FROM [is_hlasko_epc] WHERE [id] = '{0}'", objId[1]);
            SortedList res = x2Mysql.execute(sb.ToString());
            if (!Convert.ToBoolean(res["status"]))
            {
                this.msg_lbl.Text = x2.errorMessage( res["msg"].ToString() + "<br><br>" + res["query"].ToString());
            }
            this.loadEPCData(true);
            //this._generateHlasko();
        }


    }

    protected void loadEPCData(Boolean generate)
    {


        this.activity_tbl.Controls.Clear();
        this.osirix_tbl.Controls.Clear();

        this.activity_tbl.Visible = true;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_hlasko_epc] WHERE [hlasko_id] ='{0}' ORDER BY [work_start] ASC", Session["akt_hlasenie"]);
        //sb.AppendFormat("SELECT * FROM [is_hlasko_epc] WHERE [user_id] ='{0}' ORDER BY [work_start] ASC", Session["user_id"]);
        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());



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
        headCell4.Text = "Trvanie prace<br><font style='font-size:smaller;'>počíta sa čas od 19.00</font> ";
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

        TableHeaderCell headCell8 = new TableHeaderCell();
        headCell8.Text = "Subor";
        headRow.Controls.Add(headCell8);

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
            celaCas.Text = x2.UnixToMsDateTime(table[i]["work_start"].ToString()).ToString();
            riadok.Controls.Add(celaCas);

            TableCell celaCas2 = new TableCell();
            celaCas2.Text = table[i]["work_time"].ToString();
            riadok.Controls.Add(celaCas2);

            TableCell celaMeno = new TableCell();
            celaMeno.Text = table[i]["patient_name"].ToString();
            riadok.Controls.Add(celaMeno);

            TableCell celaPopis = new TableCell();
            celaPopis.Text = x2.DecryptString(table[i]["work_text"].ToString(), Session["passphrase"].ToString());
            riadok.Controls.Add(celaPopis);

            TableCell osirixCell = new TableCell();
            CheckBox ch_osirix = new CheckBox();
            ch_osirix.Checked = Convert.ToBoolean(table[i]["osirix"]);
            ch_osirix.Enabled = false;
            osirixCell.Controls.Add(ch_osirix);

            if (Convert.ToBoolean(table[i]["osirix"]))
            {
                TableRow osirixRow = new TableRow();
                this.osirix_tbl.Controls.Add(osirixRow);

                TableCell dataCell = new TableCell();
                dataCell.Text = "<p class='align-center'><a class='blue button' href='"+Resources.Resource.osirix_url + x2_var.UTFtoASCII(table[i]["patient_name"].ToString()) + "*' target='_blank' >" + table[i]["patient_name"].ToString().ToUpper() + "</a></p>";
                tags_lit.Text += "<a class='asphalt button' href='"+Resources.Resource.osirix_url + x2_var.UTFtoASCII(table[i]["patient_name"].ToString()) + "*' target='_blank' >" + table[i]["patient_name"].ToString().ToUpper() + "</a>";

                osirixRow.Controls.Add(dataCell);
            }


            riadok.Controls.Add(osirixCell);

            TableCell fileCell = new TableCell();

            if (table[i]["lf_id"].ToString() != "NULL")
            {
                Label url_lbl = new Label();
                url_lbl.Text = "<a href='lf.aspx?id=" + table[i]["lf_id"].ToString() + "' target='_blank'>Subor...</a>";
                fileCell.Controls.Add(url_lbl);
                Button delLf_btn = new Button();
                delLf_btn.ID = "delLF_" + table[i]["lf_id"].ToString();
                delLf_btn.Text = Resources.Resource.delete + " subor";
                delLf_btn.CssClass = "button red";
                delLf_btn.Click += new EventHandler(deleteLFByID);
                fileCell.Controls.Add(delLf_btn);
            }
            riadok.Controls.Add(fileCell);

            this.activity_tbl.Controls.Add(riadok);

            //if (Session["hlasko_status"] != null)
            //{
            //    if (Session["hlasko_status"].ToString() == "generated")
            //    {
            //        this.hlasko_pl.Visible = true;
            //    }
            //    else
            //    {
            //        this.hlasko_pl.Visible = true;
            //    }
            //}

            int workmin = this.allWorkTime();
            if (workmin > -1)
            {
                this.kompl_work_time.Text = "Cas prace v minutach: " + workmin.ToString() + "(min), v hodinach: " + (workmin / 60).ToString() + "(hod)";
            }

        }
        if (generate) this._generateHlasko();
        // this.clearEpcData();
    }

    protected void saveHlaskoFnc(object sender, EventArgs e)
    {
        this.saveData(false, true);
    }

    protected void saveData(bool uzavri, bool callBack)
    {
        SortedList data = new SortedList();
      //  data.Add("dat_hlas", x2.unixDate(this.Calendar1.SelectedDate));
        data.Add("text", x2.EncryptString(hlasenie.Text.ToString(), Session["passphrase"].ToString()));
        data.Add("last_user", Session["user_id"].ToString());
        //data.Add("creat_user", 0);
        data.Add("type", this.shiftType_dl.SelectedValue.ToString());
        data.Add("encrypt", "yes");

        if (uzavri == true)
        {
            data.Add("uzavri", "1");
        }

        //string res = x_db.update_row("is_hlasko", data, Session["akt_hlasenie"].ToString());

        SortedList res = x2Mysql.mysql_update("is_hlasko", data, Session["akt_hlasenie"].ToString());

       // SortedList my_last_user = new SortedList();
        //my_last_user = x_db.getUserInfoByID(Session["user_id"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);

        if (status == true)
        {
            this.loadHlasko();
            //msg_lbl.Text = res;
           // last_user.Text = my_last_user["full_name"].ToString();
           // this.generateOsirix();
        }
        else
        {
            this.msg_lbl.Text =x2.errorMessage(res["msg"].ToString());
        }
    }

    protected void deleteLFByID(object sender, EventArgs e)
    {
        Button delBtn = (Button)sender;

        string id = delBtn.ID.ToString();
        string[] tmp = id.Split('_');

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("DELETE FROM [is_data_2] WHERE [id]='{0}'", tmp[1]);

        SortedList res = x2Mysql.execute(sb.ToString());

        if (!Convert.ToBoolean(res["status"]))
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }
        else
        {
            this.loadEPCData(false);
        }

    }

    protected int allWorkTime()
    {
        int result = 0;
        SortedList res = x2Mysql.getRow("SELECT SUM([work_time]) AS [workminutes] FROM [is_hlasko_epc] WHERE [hlasko_id]='" + Session["akt_hlasenie"].ToString() + "'");

        result = Convert.ToInt32(res["workminutes"]);

        return result;
    }

    protected void printFnc(object sender, EventArgs e)
    {
        this.saveData(false,true);
        Session.Add("hlasko_date", this.Calendar1.SelectedDate);
        Session.Add("hlasko_toWord", false);
        Response.Redirect("print.aspx",false);
    }

}