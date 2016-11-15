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

public partial class is_user : System.Web.UI.Page
{
    user x_db = new user();
    x2_var my_x2 = new x2_var();
    mysql_db x2MySql = new mysql_db();
    log x2log = new log();
    
    //public string[] vykazHeader;
    public string rights;
    public string gKlinika;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        this.msg_lbl.Text = "";
        this.gKlinika = Session["klinika"].ToString().ToLower();
        this.rights = Session["rights"].ToString();

        if (!IsPostBack)
        {
            this.loadClinics();
        }
            
        //this.__loadDeps();

        if (this.rights.IndexOf("admin") != -1)
        {
            this.admin_plh.Visible = true;
            this.user_plh.Visible = true;
            this.resetUserPsswd_btn.Visible = false;

            //if (!IsPostBack)
            //{
            if (this.search_txt.Text.ToString().Trim().Length > 0)
            {
                this.searchByNameFnc(sender, e);
            }
            else
            {
                this.loadGridViewData();
            }
                
           // }
            
        }
        else
        {
            this.resetUserPsswd_btn.Visible = true;
            this.admin_plh.Visible = false;
            this.user_plh.Visible = true;
            this.newUser_btn.Enabled = false;
            this.updateUser_btn.Enabled = true;
            this.updateUser_btn.CssClass = "green button";

            if (!IsPostBack)
            {
               
                this.loadUsersData();

            }
        }
    }

    protected void loadUsersData()
    {
        string userId = Session["user_id"].ToString();
        //string query = my_x2.sprintf("SELECT * FROM [is_users] WHERE [id]='{0}'",new string[] {userId});
        this.selectedUser_hf.Value = Session["user_id"].ToString();
        SortedList result = x_db.getUserInfoByID(userId);

        this.name_txt.Text = result["full_name"].ToString();
        //this.login_txt.Text = result["name"].ToString();
        this.email_txt.Text = result["email"].ToString();
       // this.passwd_txt.Text = result["passwd"].ToString();
       // this.active_txt.Text = result["active"].ToString();
        this.titul_pred.Text = result["titul_pred"].ToString();
        this.titul_za.Text = result["titul_za"].ToString();
        this.zaradenie_txt.Text = result["zaradenie"].ToString();

        //this.name2_txt.Text = result["name2"].ToString();
       // this.name3_txt.Text = result["name3"].ToString();

        //this.clinics_dl.SelectedValue = result["klinika"].ToString();

        //this.__loadDeps();


        //this.oddelenie_dl.SelectedValue = my_x2.getStr(result["oddelenie"].ToString());


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
       // this.rights_cb.SelectedValue = result["prava"].ToString();


    }

    protected void loadGridViewData()
    {


        if (this.rights == "admin")
        {
            this.users_gv.DataSource = x_db.getAllUsersList(0);
        }
        if (this.rights == "sadmin")
        {
            int clinic = Convert.ToInt32(Session["klinika_id"]);
            this.users_gv.DataSource = x_db.getAllUsersList(clinic);
        }
        this.users_gv.DataBind();
    }

    protected void users_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //this.clearFields();
        this.users_gv.PageIndex = e.NewPageIndex;
        if (this.rights == "admin")
        {
            this.users_gv.DataSource = x_db.getAllUsersList(0);
        }
        if (this.rights == "sadmin")
        {
            int clinic = Convert.ToInt32(Session["klinika_id"]);
            this.users_gv.DataSource = x_db.getAllUsersList(clinic);
        }
        this.users_gv.DataBind();
    }

    protected void searchByNameFnc(object sender, EventArgs e)
    {
        string name = this.search_txt.Text.ToString().Trim();
        this.listSearchByNameFnc(name);
    }

    protected void listSearchByNameFnc(string name)
    {
        this.users_gv.DataSource = x_db.searchUsersByName(name, Session["klinika_id"].ToString());
        this.users_gv.DataBind();
    }

    protected void loadClinics()
    {
        //this.clinics_dl.Items.Clear();
        StringBuilder sb = new StringBuilder();

        if (this.rights == "admin")
        {
            sb.AppendLine("SELECT [id],[full_name] FROM [is_clinics] ORDER BY [idf]");
        }
        if (this.rights == "sadmin")
        {
            sb.AppendFormat("SELECT [id],[full_name] FROM [is_clinics] WHERE [idf]='{0}'", Session["klinika"].ToString().ToLower());
        }

        Dictionary<int, Hashtable> table = x2MySql.getTable(sb.ToString());

        int tableLn = table.Count;
        this.clinics_dl.Items.Add(new ListItem("", ""));
        for (int i = 0; i < tableLn; i++)
        {
            this.clinics_dl.Items.Add(new ListItem(table[i]["full_name"].ToString(), table[i]["id"].ToString()));
        }
    }

    protected void loadDeps(object sender, EventArgs e)
    {
        this.__loadDeps();

    }

    protected void __loadDeps()
    {
        this.oddelenie_dl.Items.Clear();
        if (this.clinics_dl.SelectedValue.ToString().Length > 0 )
        {
            int id = Convert.ToInt32(this.clinics_dl.SelectedValue);

            // Session.Add("userSelectedClinic", this.clinics_dl.SelectedValue);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT [id],[label] FROM [is_deps] WHERE [clinic_id]='{0}'", id);
            Dictionary<int, Hashtable> table = x2MySql.getTable(sb.ToString());
            this.oddelenie_dl.Items.Add(new ListItem("", ""));
            int tableLn = table.Count;
            for (int i = 0; i < tableLn; i++)
            {
                this.oddelenie_dl.Items.Add(new ListItem(table[i]["label"].ToString(), table[i]["id"].ToString()));
            }
        }
        
    }

    protected void users_gv_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        string id = this.users_gv.Rows[e.NewSelectedIndex].Cells[1].Text.ToString();
        
        this.updateUser_btn.Enabled = true;
        this.updateUser_btn.CssClass = "green button";
        this.newUser_btn.Enabled = false;
        this.selectedUser_hf.Value = id.ToString();

        SortedList result = x_db.getUserInfoByID(id);

        this.name_txt.Text = result["full_name"].ToString();
        this.login_txt.Text = result["name"].ToString();
        this.email_txt.Text = result["email"].ToString();
        this.passwd_txt.Text = result["passwd"].ToString();
        this.active_dl.SelectedValue = result["active"].ToString();
        this.titul_pred.Text = result["titul_pred"].ToString();
        this.titul_za.Text = result["titul_za"].ToString();
        this.zaradenie_txt.Text = result["zaradenie"].ToString();

        this.name2_txt.Text = result["name2"].ToString();
        this.name3_txt.Text = result["name3"].ToString();

        this.clinics_dl.SelectedValue =my_x2.getStr(result["klinika"].ToString());
        this.__loadDeps();
        this.oddelenie_dl.SelectedValue =my_x2.getStr(result["oddelenie"].ToString());
        

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
        this.workgroup_dl.SelectedValue = result["work_group"].ToString();
    }

    protected void newUserFnc(object sender, EventArgs e)
    {
     
        SortedList data = new SortedList();
        if (this.rights.IndexOf("admin") != -1)
        {
            data.Add("prava", this.rights_cb.SelectedValue);
            data.Add("work_group", this.workgroup_dl.SelectedValue);
            data.Add("active", this.active_dl.SelectedValue);
            if (this.oddelenie_dl.SelectedValue.ToString().Length > 0)
            {
                data.Add("oddelenie", this.oddelenie_dl.SelectedValue);
            }
            else
            {
                data.Add("oddelenie", null);
            }

            if (this.clinics_dl.SelectedValue.ToString().Length > 0)
            {
                data.Add("klinika", this.clinics_dl.SelectedValue);
            }
            else
            {
                data.Add("klinika", null);
            }

            data.Add("name", this.login_txt.Text.ToString());
            data.Add("name2", this.name2_txt.Text.ToString());
            data.Add("name3", this.name3_txt.Text.ToString());
            
        }
        data.Add("full_name", this.name_txt.Text.ToString());
        data.Add("titul_pred", this.titul_pred.Text.ToString());
        data.Add("titul_za", this.titul_za.Text.ToString());
        data.Add("email", this.email_txt.Text.ToString());
        data.Add("zaradenie", this.zaradenie_txt.Text.ToString());
        data.Add("pracdoba", this.pracdoba_txt.Text.ToString());
        data.Add("tyzdoba", this.tyzdoba_txt.Text.ToString());
        data.Add("osobcisl", this.osobcisl_txt.Text.ToString());
        data.Add("klinika_label", this.klinika_txt.Text.ToString());

        SortedList res = x2MySql.mysql_insert("is_users", data);
        if (Convert.ToBoolean(res["status"]))
        {
            this.msg_lbl.Text = "<h3>" + Resources.Resource.sys_insert + "</h3>";
            this.loadGridViewData();

            this.clearFields();
            this.updateUser_btn.CssClass = "button";
            this.updateUser_btn.Enabled = false;
            this.newUser_btn.Enabled = true;
        }
        else
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }

        
    }

    protected void updateUserFnc(object sender, EventArgs e)
    {
        int userId = 0;
        try
        {
            userId = Convert.ToInt32(this.selectedUser_hf.Value.ToString());
        }
        catch (Exception ex)
        {
            x2log.logData(sender, ex.ToString(), "int32 conversion error");
        }

        if (userId!=0)
        {
            SortedList data = new SortedList();

            if (this.rights.IndexOf("admin") != -1)
            {
                data.Add("prava", this.rights_cb.SelectedValue);
                data.Add("work_group", this.workgroup_dl.SelectedValue);

                if (this.oddelenie_dl.SelectedValue.ToString().Length > 0)
                {
                    data.Add("oddelenie", this.oddelenie_dl.SelectedValue);
                }
                else
                {
                    data.Add("oddelenie", "NULL");
                }

                if (this.clinics_dl.SelectedValue.ToString().Length > 0)
                {
                    data.Add("klinika", this.clinics_dl.SelectedValue);
                }
                else
                {
                    data.Add("klinika", "NULL");
                }

                data.Add("active", this.active_dl.SelectedValue);   
                data.Add("name",this.login_txt.Text.ToString());
                data.Add("name2", this.name2_txt.Text.ToString());
                data.Add("name3", this.name3_txt.Text.ToString());
            }

            data.Add("full_name", this.name_txt.Text.ToString());
            data.Add("titul_pred", this.titul_pred.Text.ToString());
            data.Add("titul_za", this.titul_za.Text.ToString());
            data.Add("email", this.email_txt.Text.ToString());
            data.Add("zaradenie", this.zaradenie_txt.Text.ToString());
            data.Add("pracdoba", this.pracdoba_txt.Text.ToString());
            data.Add("tyzdoba", this.tyzdoba_txt.Text.ToString());
            data.Add("osobcisl", this.osobcisl_txt.Text.ToString());
            data.Add("klinika_label", this.klinika_txt.Text.ToString());

            SortedList res = x2MySql.mysql_update("is_users", data, userId.ToString());

            if (Convert.ToBoolean(res["status"]))
            {
                this.msg_lbl.Text = "<h3>"+Resources.Resource.sys_update+"</h3>";

                if (this.rights.IndexOf("admin") != -1)
                {
                    this.loadGridViewData();
                    this.clearFields();
                    this.updateUser_btn.CssClass = "button";
                    this.updateUser_btn.Enabled = false;
                    this.newUser_btn.Enabled = true;
                }
                else
                {
                    this.loadUsersData();
                }
                
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }

        }
    }

    protected void clearFields()
    {
        this.users_gv.SelectedIndex = -1;
        this.name_txt.Text = "";
        this.login_txt.Text = "";
        this.email_txt.Text = "";
        this.active_dl.SelectedValue = "1";
        this.passwd_txt.Text = "";
        this.pracdoba_txt.Text = "";
        this.tyzdoba_txt.Text = "";
        this.osobcisl_txt.Text = "";
        this.titul_pred.Text = "";
        this.titul_za.Text = "";
        this.zaradenie_txt.Text = "";
        this.klinika_txt.Text = "";
        this.selectedUser_hf.Value = "0";
        this.name2_txt.Text = "";
        this.name3_txt.Text = "";
        this.clinics_dl.SelectedValue = "";
        this.oddelenie_dl.Items.Clear();
        this.rights_cb.SelectedValue = "";
        this.workgroup_dl.SelectedValue = "";

    }

    protected void createNamesFnc(object sender, EventArgs e)
    {
        int userId = 0;
        try
        {
            userId = Convert.ToInt32(this.selectedUser_hf.Value.ToString());
        }
        catch (Exception ex)
        {
            x2log.logData(sender, ex.ToString(), "int32 conversion error");
        }


        if (this.rights.IndexOf("admin")!=-1 && userId == 0)
        {
            string fullName = this.name_txt.Text.ToString();
            string fullNameAsci = x2_var.UTFtoASCII(fullName);

            string[] nameUtfArr = fullName.Split(' ');
            string[] nameAsciArr = fullNameAsci.ToLower().Split(' ');

            string login = nameAsciArr[0].Substring(0, 1) + nameAsciArr[1];

            int loginsCnt = x_db.checkIfLoginExists(login);

            if (loginsCnt == 0)
            {
                this.login_txt.Text = login;
            }
            if (loginsCnt > 0)
            {
                this.login_txt.Text = login + "_" + loginsCnt;
            }
            if (loginsCnt == -1)
            {
                this.msg_lbl.Text = "<h1 class='red'>Error in geting rows of logins</h1>";
            }
            this.name2_txt.Text = nameAsciArr[1] + ", " + nameAsciArr[0].Substring(0, 1).ToUpper();
            this.name3_txt.Text = nameUtfArr[1] + ", " + nameUtfArr[0].Substring(0, 1);
        }
        

    }

    protected void resetPasswordFnc(object sender, EventArgs e)
    {
        
        string userId = this.selectedUser_hf.Value.ToString();
        if (userId != "0")
        {
            string sql = my_x2.sprintf("UPDATE [is_users] SET [passwd]=NULL WHERE [id]='{0}'", new string[] { userId });

            SortedList res = x2MySql.execute(sql);

            if (Convert.ToBoolean(res["status"]))
            {
                this.passwd_txt.Text = "";
            }
            else
            {
                this.msg_lbl.Text = "Chyba: " + res["msg"].ToString();
            }
        }
        
    }

    protected void resetPasswdUser(object sender, EventArgs e)
    {
        string userId = Session["user_id"].ToString();
        if (userId != "0")
        {
            string sql = my_x2.sprintf("UPDATE [is_users] SET [passwd]=NULL WHERE [id]='{0}'", new string[] { userId });

            SortedList res = x2MySql.execute(sql);

            if (Convert.ToBoolean(res["status"]))
            {
                Response.Redirect("passch2.aspx");
            }
            else
            {
                this.msg_lbl.Text = "Chyba: " + res["msg"].ToString();
            }
        }
    }

    protected void checkLoginValidtyFnc(object sender, EventArgs e)
    {
        string login = this.login_txt.Text;

        if (!my_x2.isAlfaNum(login))
        {
            this.msg_lbl.Text = "<h2 class='red'>Nesprávny login, môže obsahovať len písmená, čísla a podtrhovátko....</h3>";
        }

    }
   

}