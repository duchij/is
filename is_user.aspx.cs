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

        this.gKlinika = Session["klinika"].ToString().ToLower();
        this.rights = Session["rights"].ToString();

        this.loadGridViewData();
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

        this.loadClinics();
        this.__loadDeps();

        this.users_gv.DataBind();
    }

    protected void users_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //this.clearFields();
        this.users_gv.PageIndex = e.NewPageIndex;
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
        this.clinics_dl.Items.Clear();
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
        int id = Convert.ToInt32(this.clinics_dl.SelectedValue);
        Session.Add("userSelectedClinic", this.clinics_dl.SelectedValue);
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
        this.active_txt.Text = result["active"].ToString();
        this.titul_pred.Text = result["titul_pred"].ToString();
        this.titul_za.Text = result["titul_za"].ToString();
        this.zaradenie_txt.Text = result["zaradenie"].ToString();

        this.clinics_dl.SelectedValue = result["klinika"].ToString();

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
    }

    protected void newUserFnc(object sender, EventArgs e)
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

            data.Add("name", this.login_txt.Text.ToString());
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
                   
                data.Add("name",this.login_txt.Text.ToString());
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
    }

    protected void clearFields()
    {
        this.users_gv.SelectedIndex = -1;
        this.name_txt.Text = "";
        this.login_txt.Text = "";
        this.email_txt.Text = "";
        this.active_txt.Text = "1";
        this.passwd_txt.Text = "";
        this.pracdoba_txt.Text = "";
        this.tyzdoba_txt.Text = "";
        this.osobcisl_txt.Text = "";
        this.titul_pred.Text = "";
        this.titul_za.Text = "";
        this.zaradenie_txt.Text = "";
        this.klinika_txt.Text = "";
        this.selectedUser_hf.Value = "0";


    }
}