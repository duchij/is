﻿using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class adduser : System.Web.UI.Page
{
    user x_db = new user();
    x2_var my_x2 = new x2_var();
    mysql_db x2MySql = new mysql_db();
    public string[] vykazHeader;

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (Request.Cookies["tuisegumdrum"] == null)
        //{
        //    Response.Redirect("error.html");
        //}

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.msg_lbl.Text = "";
        //string rights = Request.Cookies["rights"].Value.ToString();
        string rights = Session["rights"].ToString();

        SortedList akt_user_info = x_db.getUserInfoByID(Session["user_id"].ToString());


        if (rights == "users"  || rights == "poweruser")
        {
            this.adminsectionPlace.Visible = false;
            login_txt.ReadOnly = true;
            send_btn.Visible = false;
            rights_cb.Visible = false;
            rights_lbl.Visible = false;
            this.passwd_txt.ReadOnly = true;
        }

        if (rights == "admin")
        {
            this.adminsectionPlace.Visible = true;

            if (!IsPostBack)
            {
                this.name_txt.Text = "";
                this.login_txt.Text = "";
                this.email_txt.Text = "";
                this.titul_pred.Text = "";
                this.titul_za.Text = "";
                this.zaradenie_txt.Text = "";
                this.passwd_txt.ReadOnly = false;
                this.loadData();
            }
            else
            {
                string name = this.search_txt.Text.ToString();
                name = name.Trim();

                if (name.Length > 0)
                {
                    this.listSearchByNameFnc(name);
                }
                else
                {

                    this.loadData();
                }
            }

            uprav_btn.Visible = false;
            send_btn.Visible = true;
            rights_cb.Visible = true;
            rights_lbl.Visible = true;
        }
        else
        {
            if (!IsPostBack)
            {
                name_txt.Text = akt_user_info["full_name"].ToString();
                login_txt.Text = Session["login"].ToString();
                email_txt.Text = Session["email"].ToString();

                this.titul_pred.Text = Session["titul_pred"].ToString();
                this.titul_za.Text = Session["titul_za"].ToString();

                this.zaradenie_txt.Text = Session["zaradenie"].ToString();


                string pracdoba = my_x2.getStr(Session["pracdoba"].ToString());
                string tyzdoba = my_x2.getStr(Session["tyzdoba"].ToString());
                string osobcisl = my_x2.getStr(Session["osobcisl"].ToString());


                if (pracdoba.Length > 0)
                {
                    this.pracdoba_txt.Text = Session["pracdoba"].ToString();
                }
                if (tyzdoba.Length > 0)
                {
                    this.tyzdoba_txt.Text = Session["tyzdoba"].ToString();
                }

                if (osobcisl.Length > 0)
                {
                    this.osobcisl_txt.Text = Session["osobcisl"].ToString();
                }


            }
        }
        this.generateVykazSettings();

    }

    protected void loadData()
    {
        this.users_gv.DataSource = x_db.getAllUsersList();
        this.users_gv.DataBind();
    }
    protected void send_btn_Click(object sender, EventArgs e)
    {
        string meno = name_txt.Text;
        string login = login_txt.Text;
        string email = email_txt.Text;
        string passwd = this.passwd_txt.Text;
        SortedList data = new SortedList();
        int id = 0;


        try
        {
            id = Convert.ToInt32(this.users_gv.SelectedRow.Cells[1].Text.ToString());

            data.Clear();

            data.Add("full_name", meno);
            data.Add("name", login);
            data.Add("email", email);
            data.Add("passwd", passwd);
            data.Add("active", this.active_txt.Text);
            data.Add("group", rights_cb.SelectedValue.ToString());
            data.Add("pracdoba", this.pracdoba_txt.Text.ToString());
            data.Add("tyzdoba", this.tyzdoba_txt.Text.ToString());
            data.Add("osobcisl", this.osobcisl_txt.Text.ToString());
            data.Add("titul_pred", this.titul_pred.Text.ToString());
            data.Add("titul_za", this.titul_za.Text.ToString());
            data.Add("zaradenie", this.zaradenie_txt.Text.ToString());

            SortedList result = x2MySql.mysql_update("is_users", data, id.ToString());

            Boolean status = Convert.ToBoolean(result["status"]);

            if (status == false)
            {
                msg_lbl.Text = result["msg"].ToString();
            }
            else
            {
                this.name_txt.Text = "";
                this.login_txt.Text = "";
                this.email_txt.Text = "";
                this.users_gv.SelectedIndex = -1;
                this.passwd_txt.Text = "";
                this.titul_pred.Text = "";
                this.titul_za.Text = "";
                this.zaradenie_txt.Text = "";
                this.Page_Load(sender, e);

            }




        }
        catch (Exception error)
        {


            if (meno.Length != 0)
            {
                if ((my_x2.isAlfaNum(login) == true) && (login.Length != 0))
                {
                    //msg_lbl.Text = "tu sme";

                    data.Clear();

                    data.Add("full_name", meno);
                    data.Add("name", login);
                    data.Add("group", rights_cb.SelectedValue.ToString());
                    data.Add("active", this.active_txt.Text);

                    data.Add("pracdoba", this.pracdoba_txt.Text.ToString());
                    data.Add("tyzdoba", this.tyzdoba_txt.Text.ToString());

                    data.Add("osobcisl", this.osobcisl_txt.Text.ToString());
                    data.Add("titul_pred", this.titul_pred.Text.ToString());
                    data.Add("titul_za", this.titul_za.Text.ToString());

                    data.Add("zaradenie", this.zaradenie_txt.Text.ToString());
                    if (email.Length == 0)
                    {
                        email = "x";
                    }
                    data.Add("email", email);
                    SortedList user = x_db.getUserPasswd(login);

                    if ((user.Count != 0) && (user["name"].ToString() == login))
                    {
                        msg_lbl.Text = "Daný užívateľ už v databáze existuje !!!!!";
                    }
                    else
                    {
                        SortedList res = x2MySql.mysql_insert("is_users", data);
                        // SortedList res = x_db.insert_rows("is_users", data);
                        Boolean status = Convert.ToBoolean(res["status"]);

                        if (status == true)
                        {
                            msg_lbl.Text = "Užívateľ bol pridaný !!!!";
                        }
                        else
                        {
                            msg_lbl.Text = "Chyba:" + error.ToString() + "<br>" + res["msg"].ToString();
                        }
                    }
                }
                else
                {
                    msg_lbl.Text = "Login, môže byť tvorený len písmenami a číslami, medzera a iné znaky nie sú dovolené!";
                }
            }
            else
            {
                msg_lbl.Text = "Meno musi byt vypisane";
            }

        }
    }
    protected void uprav_btn_Click(object sender, EventArgs e)
    {
        string meno = name_txt.Text.ToString();
        string email = email_txt.Text.ToString();

        SortedList data = new SortedList();

        data.Add("full_name", meno);
        data.Add("email", email);

        data.Add("pracdoba", this.pracdoba_txt.Text.ToString().Trim());
        data.Add("tyzdoba", this.tyzdoba_txt.Text.ToString().Trim());

        data.Add("osobcisl", this.osobcisl_txt.Text.ToString().Trim());
        data.Add("titul_pred", this.titul_pred.Text.ToString().Trim());
        data.Add("titul_za", this.titul_za.Text.ToString().Trim());
        data.Add("zaradenie", this.zaradenie_txt.Text.ToString().Trim());

        // string res = x_db.update_row("is_users", data, Session["user_id"].ToString());

        SortedList res = x2MySql.mysql_update("is_users", data, Session["user_id"].ToString());

        Boolean status = Convert.ToBoolean(res["status"]);

        if (status == true)
        {
            msg_lbl.Text = "Zmena bola úspešne vykonaná !!!";
        }
        else
        {
            msg_lbl.Text = "Chyba: " + res["msg"].ToString();
        }

    }
    protected void users_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.clearFields();
        this.users_gv.PageIndex = e.NewPageIndex;
        this.users_gv.DataBind();
    }

    protected void clearFields()
    {
        this.users_gv.SelectedIndex = -1;
        this.name_txt.Text = "";
        this.login_txt.Text = "";
        this.email_txt.Text = "";
        this.active_txt.Text = "0";
        this.passwd_txt.Text = "";
        this.pracdoba_txt.Text = "";
        this.tyzdoba_txt.Text = "";
        this.osobcisl_txt.Text = "";
        this.titul_pred.Text = "";
        this.titul_za.Text = "";
        this.zaradenie_txt.Text = "";
    }

    protected void users_gv_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        string id = this.users_gv.Rows[e.NewSelectedIndex].Cells[1].Text.ToString();


        SortedList result = x_db.getUserInfoByID(id);

        this.name_txt.Text = result["full_name"].ToString();
        this.login_txt.Text = result["name"].ToString();
        this.email_txt.Text = result["email"].ToString();
        this.passwd_txt.Text = result["passwd"].ToString();
        this.active_txt.Text = result["active"].ToString();
        this.titul_pred.Text = result["titul_pred"].ToString();
        this.titul_za.Text = result["titul_za"].ToString();
        this.zaradenie_txt.Text = result["zaradenie"].ToString();
        string pracdoba = my_x2.getStr(result["pracdoba"].ToString());
        string tyzdoba = my_x2.getStr(result["tyzdoba"].ToString());

        string osobcisl = my_x2.getStr(result["osobcisl"].ToString());

        if (pracdoba.Length > 0)
        {

            this.pracdoba_txt.Text = pracdoba;
        }
        if (tyzdoba.Length > 0)
        {

            this.tyzdoba_txt.Text = tyzdoba;
        }

        if (osobcisl.Length > 0)
        {

            this.osobcisl_txt.Text = osobcisl;
        }


        this.rights_cb.SelectedValue = result["prava"].ToString();


    }
    protected void users_gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = Convert.ToInt32(this.users_gv.Rows[e.RowIndex].Cells[1].Text.ToString());
        //debug_lbl.Text = id.ToString();
        string res = x_db.delete_row("is_users", id);

        if (res != "ok")
        {
            this.msg_lbl.Text = res;
        }
        else
        {

            this.Page_Load(sender, e);
        }
    }

    protected void searchByNameFnc(object sender, EventArgs e)
    {
        string name = this.search_txt.Text.ToString();
        //this.msg_lbl.Text = name;

        this.listSearchByNameFnc(name);

    }

    protected void listSearchByNameFnc(string name)
    {
        this.users_gv.DataSource = x_db.searchUsersByName(name);
        this.users_gv.DataBind();
    }
    protected string[] getVykazTyp()
    {
        SortedList row = x2MySql.getRow("SELECT * FROM [is_settings] WHERE [name]='typ_vykaz'");
        return row["data"].ToString().Split(',');

    }
    protected string[] getDefaultHours()
    {
        SortedList row = x2MySql.getRow("SELECT * FROM [is_settings] WHERE [name]='hodiny_vykaz'");
        return row["data"].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    }

    protected string getUserHours()
    {
        string result = "";
        string tmp = Session["login"].ToString()+"_vykaz";

        SortedList row = x2MySql.getRow("SELECT * FROM [is_settings] WHERE [name]='" + tmp + "'");
        if (row.Count > 0)
        {
            result = row["data"].ToString();
        }
        return result;
    }

    protected void generateVykazSettings()
    {
        this.vykazSetup_tbl.Controls.Clear();
        string[] typVykaz = this.getVykazTyp();
              

        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM [is_settings]  WHERE [name]='vykaz_doctors'");

        SortedList row = x2MySql.getRow(sb.ToString());
        this.vykazHeader = row["data"].ToString().Split(',');

        string tmpHours = this.getUserHours();
        string[] hoursArr = tmpHours.Split('|');

        string[] defaultHodiny = new string[this.vykazHeader.Length];

        if (hoursArr[0].Split(',').Length == this.vykazHeader.Length)
        {
            defaultHodiny = hoursArr;
        }
        else
        {
            defaultHodiny = this.getDefaultHours(); 
        }

        int cols = this.vykazHeader.Length;
        TableHeaderRow headerRow = new TableHeaderRow();

        TableHeaderCell headCellDate = new TableHeaderCell();
        headCellDate.ID = "sluzba_cell";
        headCellDate.Text = "Sluzba";
        headerRow.Controls.Add(headCellDate);

        this.vykazSetup_tbl.Controls.Add(headerRow);

        for (int col = 0; col < cols; col++)
        {
            TableHeaderCell headCell = new TableHeaderCell();
            headCell.ID = "headCell_" + col.ToString();

            Label headLabel = new Label();
            headLabel.ID = this.vykazHeader[col] + "_lbl<br>";
            headLabel.Font.Size = FontUnit.Point(8);
            headLabel.Text = this.vykazHeader[col];

            headCell.Controls.Add(headLabel);

           /* TextBox tBox = new TextBox();
            tBox.ID = "head_tbox_" + col.ToString();
            tBox.Text = "";
            headCell.Controls.Add(tBox);*/


            headerRow.Controls.Add(headCell);
        }

        int typVykazLn = typVykaz.Length;

        for (int riadok = 0; riadok < typVykazLn; riadok++)
        {
            TableRow riadTyp = new TableRow();
            TableCell typCell = new TableCell();
            typCell.ID = typVykaz[riadok].ToString();
            typCell.Text = typVykaz[riadok].ToString();

            string[] hodiny = defaultHodiny[riadok].Split(',');

            switch (typVykaz[riadok].ToString())
            {
                case "normDen":
                    typCell.ToolTip = "Nastavenia normalneho dna";
                    break;
                case "malaSluzba":
                    typCell.ToolTip = "Nastavenia hodin pri malej sluzbe";
                    break;
                case "malaSluzba2":
                    typCell.ToolTip = "Nastavenia hodin po malej sluzbe";
                    break;
                case "velkaSluzba":
                    typCell.ToolTip = "Nastavenia hodin pri velkej sluzbe";
                    break;
                case "velkaSluzba2":
                    typCell.ToolTip = "Nastavenia hodin den po velkej sluzbe";
                    break;
                case "velkaSluzba2a":
                    typCell.ToolTip = "Nastavenia hodin druhy nasledujuci den po velkej sluzbe (sobota, nedela a pondelok, utorok)";
                    break;
                case "sviatokVikend":
                    typCell.ToolTip = "Nastavenia hodin v sluzbe ak pripadne na vikend aj sviatocny den";
                    break;
                case "sviatok":
                    typCell.ToolTip = "Nastavenie hodin ak je sviatok iny den ako sobota, nedela";
                    break;
                case "exDay":
                    typCell.ToolTip = "Nastavenie hodin pocas nasledujuceho volneho dna";
                    break;
                case "sviatokNieVikend":
                    typCell.ToolTip = "Nastavenie hodin v sviatok, ale nie pocas sluzby";
                    break;

            }
            riadTyp.Controls.Add(typCell);
            for (int col = 0; col < cols; col++)
            {
                TableCell dataCela = new TableCell();
                riadTyp.Controls.Add(dataCela);
                TextBox tBox = new TextBox();
                tBox.ID = "textBox_" + riadok.ToString() + "_" + col.ToString();
                if (!IsPostBack)
                {
                    tBox.Text = hodiny[col].ToString();
                }

               
                dataCela.Controls.Add(tBox);
            }
            this.vykazSetup_tbl.Controls.Add(riadTyp);
        }
    }

    protected void saveVykaz_fnc(object sender, EventArgs e)
    {
        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

       // string[] typVykaz = this.getVykazTyp();

        string[] defaultHodiny = this.getDefaultHours();

        int typVykazLn = defaultHodiny.Length;
        int defaultHodLn = this.vykazHeader.Length;
        string[] riadArr = new string[typVykazLn];
        string[] stlpArr = new string[defaultHodLn];
        for (int riadok = 0; riadok < typVykazLn; riadok++)
        {
            for (int col = 0; col < defaultHodLn; col++)
            {
                Control Tbox = ctpl.FindControl("textBox_" + riadok.ToString() + "_" + col.ToString());
                TextBox tBox = (TextBox)Tbox;
                //TextBox tBox = new TextBox();
                string hodnota = tBox.Text.ToString();
                hodnota = hodnota.Replace(',','.');
                //tBox.ID = "textBox_" + riadok.ToString() + "_" + col.ToString();
                stlpArr[col] = hodnota;

            }
            string tmp = String.Join(",",stlpArr);

            riadArr[riadok] = tmp;

        }
        string defStr = String.Join("|", riadArr);

        SortedList saveData = new SortedList();
        saveData.Add("name", Session["login"].ToString()+"_vykaz");
        saveData.Add("data", defStr);

        SortedList result = x2MySql.mysql_insert("is_settings", saveData);

        if (!Convert.ToBoolean(result["status"]))
        {
            this.msg_lbl.Visible = true;
            this.msg_lbl.Text = result["msg"].ToString();
        }
        else
        {
            this.msg_lbl.Visible = true;
            this.msg_lbl.Text = "Nastavenia vykazu ulozene";
        }



    }

    protected void resetVykaz_fnc(object sender, EventArgs e)
    {
        SortedList res = x2MySql.execute("DELETE FROM [is_settings] WHERE [name]='" + Session["login"].ToString() + "_vykaz'");
        if (!Convert.ToBoolean(res["status"]))
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }
        else
        {
            Response.Redirect("adduser.aspx");
        }

    }


}
