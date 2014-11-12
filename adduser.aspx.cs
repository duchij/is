using System;
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

        SortedList akt_user_info = x_db.getUserInfoByID("is_users", Session["user_id"].ToString());


        if (rights.IndexOf("users") != -1 || rights.IndexOf("sestra") !=-1  || rights.IndexOf("poweruser") != -1 )
        {
            this.adminsectionPlace.Visible = false;
            login_txt.ReadOnly = true;
            send_btn.Visible = false;
            rights_cb.Visible = false;
            rights_lbl.Visible = false;
            this.passwd_txt.ReadOnly = true;
        }

        if ((rights == "admin") && (Session["login"].ToString() != "vcingel"))
        {
            this.adminsectionPlace.Visible = true;

            if (!IsPostBack)
            {
                this.name_txt.Text = "";
                this.login_txt.Text = "";
                this.email_txt.Text = "";
                this.titul_pred.Text = "";
                this.titul_za.Text = "";
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

                string pracdoba = my_x2.getStr(Session["pracdoba"].ToString());
                string tyzdoba = my_x2.getStr(Session["tyzdoba"].ToString());
                string osobcisl = my_x2.getStr(Session["osobcisl"].ToString());
              

                if (pracdoba.Length > 0)
                {
                    this.pracdoba_txt.Text = Session["pracdoba"].ToString();
                }
                if (tyzdoba.Length > 0 )
                {
                    this.tyzdoba_txt.Text = Session["tyzdoba"].ToString();
                }

                if (osobcisl.Length > 0)
                {
                    this.osobcisl_txt.Text = Session["osobcisl"].ToString();
                }
                    

            }
        }      


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

                this.Page_Load(sender, e);
              
            }




        }
        catch (Exception error)
        {


            if (meno.Length != 0)
            {
                if ((my_x2.isAlfaNum(login) == true ) && (login.Length !=0))
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


                    if (email.Length == 0)
                    {
                        email = "x";
                    }
                    data.Add("email", email);
                    SortedList user = x_db.getUserPasswd(login);
                    
                    if ((user.Count != 0 ) && (user["name"].ToString() == login))
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
                            msg_lbl.Text = "Chyba:" + error.ToString()+"<br>"+res["msg"].ToString();
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
    }

    protected void users_gv_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        string id = this.users_gv.Rows[e.NewSelectedIndex].Cells[1].Text.ToString();


        SortedList result = x_db.getUserInfoByID("is_users",id);

        this.name_txt.Text = result["full_name"].ToString();
        this.login_txt.Text = result["name"].ToString();
        this.email_txt.Text = result["email"].ToString();
        this.passwd_txt.Text = result["passwd"].ToString();
        this.active_txt.Text = result["active"].ToString();
        this.titul_pred.Text = result["titul_pred"].ToString();
        this.titul_za.Text = result["titul_za"].ToString();

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


        this.rights_cb.SelectedValue = result["group"].ToString();


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

}
