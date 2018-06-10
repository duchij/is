using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;


public partial class _Default : System.Web.UI.Page
{
    my_db db_obj = new my_db();
    //mysql_db db_obj = new mysql_db();
    x2_var x2 = new x2_var();
    log x2log = new log();
    mysql_db x2Mysql = new mysql_db();
   
    

    protected void Page_Load(object sender, EventArgs e)
    {
        string param = Request["__EVENTARGUMENT"];
    }

    protected Boolean personalNumber(string login)
    {
        int inOut;

        return Int32.TryParse(login, out inOut);
    }
    /// <summary>
    /// Forces to change user password using the password decrypted
    /// </summary>
    /// <param name="userName">unexcrypted username</param>
    /// <param name="passwd">unencrypted password</param>
    /// <param name="type">old hashing mechanism</param>
    /// <returns></returns>

    protected int checkChangePassword(string userName, string passwd, string type)
    {
        string hPasswd = null;

        switch (type)
        {
            case "MD5":
                MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

                byte[] arr = Encoding.UTF8.GetBytes(passwd);

                arr = hasher.ComputeHash(arr);
                hPasswd = Convert.ToBase64String(arr);

                break;

        }


        string sql = @"SELECT [force_change],[passwd] FROM [is_users] WHERE [name]='{0}'";

        sql = x2Mysql.buildSql(sql, new string[] { userName});

        SortedList row = x2Mysql.getRow(sql);

        int result = 0; 

        if (row.ContainsKey("force_change"))
        {
            if (row["force_change"].ToString() == "0" && row["passwd"].ToString() == hPasswd)
            {
                result = 1; // no need to change password
            }
            if (row["force_change"].ToString() == "1" && row["passwd"].ToString() == hPasswd)
            {
                result = 2; //need to force change password
            }
            if (row["force_change"].ToString() == "1" && row["passwd"].ToString() != hPasswd)
            {
                result = 4; // Passwords are not the same, contact admin
            }
            if (row["force_change"].ToString() == "1" && row["passwd"].ToString() == "NULL")
            {
                result = 7;
            }
        }
        else
        {
            result = 3; //skip force change
        }
          




        return result;
    }

    protected void runLogin(object sender, EventArgs e)
    {
        SortedList wnameSL = Rijndael.decryptJsAes(this.name_hf.Value.ToString(), Session.SessionID.ToString());

        SortedList passwdSL = Rijndael.decryptJsAes(this.passwd_hf.Value.ToString(), Session.SessionID.ToString());

       

            if (!(Boolean)wnameSL["status"])
            {
                this.info_txt.Text = "Bad user or not active session" + wnameSL["result"].ToString();
            }

            else if (!(Boolean)passwdSL["status"])
            {
                this.info_txt.Text = "Bad password or not active session, "+passwdSL["result"].ToString();
            }
            else
            {
                this.runLogin_phase2(wnameSL["result"].ToString(), passwdSL["result"].ToString());
            }
           

       

    }


    protected void runLogin_phase2(string userName, string passwd)
    { 

        string g_pass = "";

        SortedList data = new SortedList();

        string l_pass = x2.makeSHA1String(passwd);


        //string userName = res["result"].ToString();
        try
        {


            if (x2.isAlfaNum(userName))
            {

                int forceChangeStatus = this.checkChangePassword(userName, passwd, "MD5");

                if (forceChangeStatus == 2 || forceChangeStatus == 7)
                {
                    Session["force_change"] = 1;
                    Response.Redirect("fpassch.aspx?uname=" + userName);
                }
                else if (forceChangeStatus == 4)
                {
                    throw new System.InvalidProgramException("Zle zadane stare heslo, nemozno zmenit na nove kontaktujte admina...");
                }


                Boolean persNum = this.personalNumber(userName);

                if (persNum)
                {
                    userName = "d" + userName;
                }

                data = db_obj.getUserPasswd(userName);



                if (data.Count != 0 && data["active"].ToString() == "1")
                {

                    g_pass = data["passwd"].ToString();


                    data.Remove("passwd");

                    if (persNum)
                    {
                        //g_pass = "d" + g_pass;
                        passwd = "d" + passwd;
                    }

                    if (userName == passwd && g_pass == "NULL" && data["name"].ToString() == passwd)
                    {

                        x2log.logData(data, "", "first user login");
                        this.loadYearMonthData();
                        Session.Add("sid", Session.SessionID.ToString());
                        Session.Add("tuisegumdrum", "activado");
                        Session.Add("user_id", data["id"].ToString());
                        Session.Add("rights", data["prava"].ToString());
                        Session.Add("workgroup", data["work_group"].ToString());
                        Session.Add("fullname", data["full_name"].ToString());
                        Session.Add("login", data["name"].ToString());
                        Session.Add("email", x2.getStr(data["email"].ToString()));

                        Session.Add("klinika", x2.getStr(data["clinics_idf"].ToString()));
                        Session.Add("oddelenie", x2.getStr(data["deps_idf"].ToString()));

                        Session.Add("klinika_id", data["klinika"]);
                        if (x2.getStr(data["omega_ms_item_id"].ToString()).Length > 0)
                        {
                            Session["omega_ms_item_id"] = x2.getStr(data["omega_ms_item_id"].ToString());
                        }
                        else
                        {
                            Session["omega_ms_item_id"] = null;
                        }

                        Session.Add("oddelenie_id", x2.getStr(data["oddelenie"].ToString()));

                        Session.Add("pracdoba", x2.getStr(data["pracdoba"].ToString()));
                        Session.Add("tyzdoba", x2.getStr(data["tyzdoba"].ToString()));
                        Session.Add("osobcisl", x2.getStr(data["osobcisl"].ToString()));

                        Session.Add("titul_pred", x2.getStr(data["titul_pred"].ToString()));
                        Session.Add("titul_za", x2.getStr(data["titul_za"].ToString()));
                        Session.Add("klinika_label", x2.getStr(data["klinika_label"].ToString()));
                        Session.Add("clinic_label", x2.getStr(data["clinic_label"].ToString()));
                        Session.Add("zaradenie", x2.getStr(data["zaradenie"].ToString()));

                        string[] fd = x2Mysql.getFreeDays();
                        Session.Add("freedays", String.Join(",", fd));

                        SortedList passPhrase = db_obj.getPassPhrase();
                        Session.Add("passphrase", passPhrase["data"].ToString());

                        Session.Add("LABELS", this.loadLabels(data["klinika"].ToString()));


                        /* Response.Cookies["tuisegumdrum"].Value = "activado";
                         Response.Cookies["user_id"].Value = data["id"].ToString();
                         Response.Cookies["rights"].Value = data["group"].ToString();
                         Response.Cookies["fullname"].Value = data["full_name"].ToString();
                         Response.Cookies["login"].Value = data["name"].ToString();
                         Response.Cookies["email"].Value = data["email"].ToString(); */

                        Response.Redirect("passch2.aspx");
                    }

                    if (l_pass == g_pass)
                    {

                        //this.Login1_Authenticate.

                        //  e.Authenticated = true;

                        x2log.logData(data, "", "user login:" + data["full_name"]);

                        this.deleteFilesPerDays();
                        Session.Add("sid", Session.SessionID.ToString());
                        Session.Add("tuisegumdrum", "activado");
                        this.loadYearMonthData();
                        Session.Add("user_id", data["id"].ToString());
                        Session.Add("rights", data["prava"].ToString());
                        Session.Add("workgroup", data["work_group"].ToString());
                        Session.Add("fullname", data["full_name"].ToString());
                        Session.Add("login", data["name"].ToString());
                        Session.Add("email", data["email"].ToString());

                        Session.Add("pracdoba", x2.getStr(data["pracdoba"].ToString()));
                        Session.Add("tyzdoba", x2.getStr(data["tyzdoba"].ToString()));
                        Session.Add("osobcisl", x2.getStr(data["osobcisl"].ToString()));

                        Session.Add("klinika", x2.getStr(data["clinics_idf"].ToString()));
                        Session.Add("oddelenie", x2.getStr(data["deps_idf"].ToString()));

                        Session.Add("klinika_id", data["klinika"]);
                        Session.Add("oddelenie_id", x2.getStr(data["oddelenie"].ToString()));

                        Session.Add("titul_pred", x2.getStr(data["titul_pred"].ToString()));
                        Session.Add("titul_za", x2.getStr(data["titul_za"].ToString()));

                        Session.Add("zaradenie", x2.getStr(data["zaradenie"].ToString()));
                        Session.Add("klinika_label", x2.getStr(data["klinika_label"].ToString()));
                        Session.Add("clinic_label", x2.getStr(data["clinic_label"].ToString()));

                        if (data["name"].ToString() == "sklad")
                        {
                            Response.Redirect("sklad/hladanie.aspx");
                        }


                        string[] fd = x2Mysql.getFreeDays();
                        Session.Add("freedays", String.Join(",", fd));

                        if (x2.getStr(data["omega_ms_item_id"].ToString()).Length > 0)
                        {
                            Session["omega_ms_item_id"] = x2.getStr(data["omega_ms_item_id"].ToString());
                        }
                        else
                        {
                            Session["omega_ms_item_id"] = null;
                        }

                        SortedList passPhrase = db_obj.getPassPhrase();
                        Session.Add("passphrase", passPhrase["data"].ToString());
                        Session.Add("LABELS", this.loadLabels(data["klinika"].ToString()));

                        /*Response.Cookies["tuisegumdrum"].Value = " activado";
                        Response.Cookies["user_id"].Value = data["id"].ToString();
                        Response.Cookies["rights"].Value = data["group"].ToString();
                        Response.Cookies["fullname"].Value = data["full_name"].ToString();
                        Response.Cookies["login"].Value = data["name"].ToString();
                        Response.Cookies["email"].Value = data["email"].ToString(); */


                        List<string> news = db_obj.getLastNews(Convert.ToInt32(Session["klinika_id"].ToString()));



                        if (news.Count > 0)
                        {
                            Session["newsToShow"] = news[0];
                            Session["newsToShowDialog"] = news[0];
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNews", "alert('trara');", true);
                        }
                        else
                        {
                            Session["newsToShow"] = "";
                            Session["newsToShowDialog"] = "";
                        }

                        if (Session["klinika"].ToString() == "skladzm")
                        {
                            SortedList vzpData = new SortedList();
                            vzpData.Add("search", "all");
                            vzpData.Add("tab", "1");
                            Session.Add("sklad_vzp", vzpData);
                            Response.Redirect(@"sklad/vzp.aspx");
                        }


                        if (data["name"].ToString().IndexOf("tablet") != -1)
                        {
                            Response.Redirect("tabletview.aspx");
                        }


                        if (Session["workgroup"].ToString() == "doctor")
                        {
                            SortedList poz_data = db_obj.getNextPozDatum(DateTime.Today);

                            if (this.maVyplnPoziadavky(Session["user_id"].ToString()) == true)
                            {
                                Response.Redirect(@"hlasko.aspx");
                            }
                            else
                            {
                                if (DateTime.Today < Convert.ToDateTime(poz_data["datum"].ToString()))
                                {
                                    if (Session["klinika"].ToString().ToLower() == "kdch" && Session["login"].ToString() != "admin")
                                    {
                                        Response.Redirect(@"poziadavky.aspx?a=1");
                                    }
                                    else
                                    {
                                        Response.Redirect(@"hlasko.aspx");
                                    }

                                }
                                else
                                {
                                    Response.Redirect(@"hlasko.aspx");
                                }
                            }
                        }
                        /*else if (group.IndexOf("medix") != -1)
                        {
                            Session.Add("medixUser", "medixUser");
                            Response.Redirect(@"MEDIX/opdg.aspx");
                        }*/
                        else if (Session["workgroup"].ToString() == "assistent")
                        {
                            Response.Redirect(@"sestrhlas.aspx");
                        }
                        else if (Session["workgroup"].ToString() == "nurse")
                        {
                            //SortedList result = db_obj.getNextPozDatum(DateTime.Today);
                            Response.Redirect(@"sestrhlas.aspx");
                        }
                        else if (Session["workgroup"].ToString() == "other")
                        {
                            Response.Redirect(@"sluzby3.aspx");
                        }
                        else if (Session["workgroup"].ToString() == "op")
                        {
                            Response.Redirect(@"opprogram.aspx");
                        }

                        //Response.Write("hura");

                    }

                    else
                    {
                        // e.Authenticated = false;
                        throw new System.InvalidProgramException("Zle heslo.... ");
                       // this.info_txt.Text = "Bad user or password";
                       // x2log.logData(data, "bad user or password", "error bad user login:" + data["full_name"]);
                    }
                }
                else
                {
                    //e.Authenticated = false;
                    throw new System.InvalidProgramException("Zle meno alebo heslo");
                    //this.info_txt.Text = "Bad user or password";
                    //x2log.logData(data, "bad user or password", "error bad user login:" + data["full_name"]);
                }
            }
        }
        catch (Exception ex)
        {
            this.info_txt.Text = ex.Message.ToString();
            x2log.logData(data, "bad user or password", "error bad user login:" + data["full_name"]);
        }

    }

   /* protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {

        x2_var my_x2 = new x2_var();
        string passwd = Login1.Password.ToString().Trim();
        string l_pass = my_x2.make_hash(passwd);
        string g_pass = "";

        SortedList data = new SortedList();
        string userName = Login1.UserName.ToString().Trim();
        
        if (my_x2.isAlfaNum(userName))
        {
            Boolean persNum = this.personalNumber(userName);

            if (persNum)
            {
                userName = "d" + userName;
            }

            data = db_obj.getUserPasswd(userName);

            

            if (data.Count != 0 && data["active"].ToString() == "1")
            {

                g_pass = data["passwd"].ToString();

                if (persNum)
                {
                    //g_pass = "d" + g_pass;
                    passwd = "d" + passwd;
                }

                if (userName == passwd && g_pass == "NULL" && data["name"].ToString() == passwd)
                {

                    x2log.logData(data,"","first user login");
                    this.loadYearMonthData();
                    Session.Add("tuisegumdrum", "activado");
                    Session.Add("user_id", data["id"].ToString());
                    Session.Add("rights", data["prava"].ToString());
                    Session.Add("workgroup", data["work_group"].ToString());
                    Session.Add("fullname", data["full_name"].ToString());
                    Session.Add("login", data["name"].ToString());
                    Session.Add("email", x2.getStr(data["email"].ToString()));

                    Session.Add("klinika", x2.getStr(data["clinics_idf"].ToString()));
                    Session.Add("oddelenie", x2.getStr(data["deps_idf"].ToString()));

                    Session.Add("klinika_id", data["klinika"]);
                    if (x2.getStr(data["omega_ms_item_id"].ToString()).Length > 0)
                    {
                        Session["omega_ms_item_id"] = x2.getStr(data["omega_ms_item_id"].ToString());
                    }
                    else
                    {
                        Session["omega_ms_item_id"] = null;
                    }

                    Session.Add("oddelenie_id", x2.getStr(data["oddelenie"].ToString()));

                    Session.Add("pracdoba", x2.getStr(data["pracdoba"].ToString()));
                    Session.Add("tyzdoba", x2.getStr(data["tyzdoba"].ToString()));
                    Session.Add("osobcisl", x2.getStr(data["osobcisl"].ToString()));

                    Session.Add("titul_pred", x2.getStr(data["titul_pred"].ToString()));
                    Session.Add("titul_za", x2.getStr(data["titul_za"].ToString()));
                    Session.Add("klinika_label", x2.getStr(data["klinika_label"].ToString()));
                    Session.Add("clinic_label", x2.getStr(data["clinic_label"].ToString()));
                    Session.Add("zaradenie", x2.getStr(data["zaradenie"].ToString()));

                    string[] fd = x2Mysql.getFreeDays();
                    Session.Add("freedays",String.Join(",",fd));

                    SortedList passPhrase = db_obj.getPassPhrase();
                    Session.Add("passphrase", passPhrase["data"].ToString());

                    Session.Add("LABELS",this.loadLabels(data["klinika"].ToString()));

        

                    Response.Redirect("passch2.aspx");
                }

                if (l_pass == g_pass)
                {

                    e.Authenticated = true;

                    x2log.logData(data, "", "user login:"+data["full_name"]);

                    this.deleteFilesPerDays();
                    
                    Session.Add("tuisegumdrum", "activado");
                    this.loadYearMonthData();
                    Session.Add("user_id", data["id"].ToString());
                    Session.Add("rights", data["prava"].ToString());
                    Session.Add("workgroup", data["work_group"].ToString());
                    Session.Add("fullname", data["full_name"].ToString());
                    Session.Add("login", data["name"].ToString());
                    Session.Add("email", data["email"].ToString());

                    Session.Add("pracdoba", x2.getStr(data["pracdoba"].ToString()));
                    Session.Add("tyzdoba", x2.getStr(data["tyzdoba"].ToString()));
                    Session.Add("osobcisl", x2.getStr(data["osobcisl"].ToString()));
                                                                 
                    Session.Add("klinika", x2.getStr(data["clinics_idf"].ToString()));
                    Session.Add("oddelenie",x2.getStr(data["deps_idf"].ToString()));

                    Session.Add("klinika_id", data["klinika"]);
                    Session.Add("oddelenie_id", x2.getStr(data["oddelenie"].ToString()));
                    
                    Session.Add("titul_pred", x2.getStr(data["titul_pred"].ToString()));
                    Session.Add("titul_za",  x2.getStr(data["titul_za"].ToString()));

                    Session.Add("zaradenie",  x2.getStr(data["zaradenie"].ToString()));
                    Session.Add("klinika_label", x2.getStr(data["klinika_label"].ToString()));
                    Session.Add("clinic_label", x2.getStr(data["clinic_label"].ToString()));

                    if (data["name"].ToString() == "sklad")
                    {
                        Response.Redirect("sklad/hladanie.aspx");
                    }


                    string[] fd = x2Mysql.getFreeDays();
                    Session.Add("freedays", String.Join(",", fd));

                    if (x2.getStr(data["omega_ms_item_id"].ToString()).Length > 0)
                    {
                        Session["omega_ms_item_id"] = x2.getStr(data["omega_ms_item_id"].ToString());
                    }
                    else
                    {
                        Session["omega_ms_item_id"] = null;
                    }

                    SortedList passPhrase = db_obj.getPassPhrase();
                    Session.Add("passphrase", passPhrase["data"].ToString());
                    Session.Add("LABELS", this.loadLabels(data["klinika"].ToString()));

                  
                  
                    List<string> news = db_obj.getLastNews(Convert.ToInt32(Session["klinika_id"].ToString()));

                   

                    if (news.Count > 0)
                    {
                        Session["newsToShow"] = news[0];
                        Session["newsToShowDialog"] = news[0];
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNews", "alert('trara');", true);
                    }
                    else
                    {
                        Session["newsToShow"] = "";
                        Session["newsToShowDialog"] = "";
                    }

                    if (Session["klinika"].ToString() == "skladzm")
                    {
                        SortedList vzpData = new SortedList();
                        vzpData.Add("search", "all");
                        vzpData.Add("tab", "1");
                        Session.Add("sklad_vzp",vzpData);
                        Response.Redirect(@"sklad/vzp.aspx");
                    }


                    if (data["name"].ToString().IndexOf("tablet")!=-1)
                    {
                        Response.Redirect("tabletview.aspx");
                    }
                

                    if (Session["workgroup"].ToString() == "doctor")
                    {
                       SortedList poz_data = db_obj.getNextPozDatum(DateTime.Today);

                        if (this.maVyplnPoziadavky(Session["user_id"].ToString()) == true)
                        {
                                Response.Redirect(@"hlasko.aspx");
                        }
                        else
                        {
                            if (DateTime.Today < Convert.ToDateTime(poz_data["datum"].ToString()))
                            {
                                if (Session["klinika"].ToString().ToLower() == "kdch" && Session["login"].ToString()!="admin")
                                {
                                    Response.Redirect(@"poziadavky.aspx?a=1");
                                }
                                else
                                {
                                    Response.Redirect(@"hlasko.aspx");
                                }
                               
                            }
                            else
                            {
                                Response.Redirect(@"hlasko.aspx");
                            }
                        }
                    }
                    
                    else if (Session["workgroup"].ToString() == "assistent")
                    {
                        Response.Redirect(@"sestrhlas.aspx");
                    }
                    else if (Session["workgroup"].ToString() == "nurse")
                    {
                        //SortedList result = db_obj.getNextPozDatum(DateTime.Today);
                        Response.Redirect(@"sestrhlas.aspx");
                    }
                    else if (Session["workgroup"].ToString() == "other")
                    {
                        Response.Redirect(@"sluzby3.aspx");
                    }
                    else if (Session["workgroup"].ToString() == "op")
                    {
                        Response.Redirect(@"opprogram.aspx");
                    }

                    //Response.Write("hura");

                }

                else
                {
                    e.Authenticated = false;
                    x2log.logData(data, "bad user or password", "error bad user login:" + data["full_name"]);
                }
            }
            else
            {
                e.Authenticated = false;
                x2log.logData(data, "bad user or password", "error bad user login:" + data["full_name"]);
            }
        }
    }
*/
    protected SortedList loadLabels(string cId)
    {
        SortedList labels = new SortedList();

        SortedList res = x2Mysql.getLabels(cId);
        if (res["status"] == null)
        {
            labels = res;
        }

        return labels;

        
    }


    protected void loadYearMonthData()
    {
        string monthDl = @"date_januar,1;date_februar,2;date_march,3;date_april,4;date_maj,5;date_june,6;date_july,7;date_august,8;date_september,9;date_october,10;date_november,11;date_december,12";
        string yearsDl = "2010,2020";

        Session.Add("month_dl", monthDl);
        Session.Add("years_dl", yearsDl);

    }

    protected void deleteFilesPerDays()
    {
        List<string> filesToDelete = db_obj.loadTmpFilesToDelete();

        if (filesToDelete.Count > 0)
        {
            for (int i = 0; i < filesToDelete.Count; i++)
            {
                if (filesToDelete[i].IndexOf("|") == -1)
                {
                    if (File.Exists(@Server.MapPath("App_Data") + @"\" + filesToDelete[i]))
                    {
                        File.Delete(@Server.MapPath("App_Data") + @"\" + filesToDelete[i]);
                    }
                }
                else
                {
                    string[] tmp = filesToDelete[i].Split('|');

                    if (File.Exists(@Server.MapPath("App_Data") + @"\"+tmp[1]+@"\" + tmp[0]))
                    {
                       // File.Delete(@Server.MapPath("App_Data") + @"\"+tmp[1]+@"\" + tmp[0]);
                    }
                }
            }
        }
    }
    
    protected bool maVyplnPoziadavky(string user_id)
    {
        bool result = false;
        DateTime dnes = DateTime.Today;

        SortedList data = db_obj.getUserPoziadavky(user_id, dnes);

        if (data["info"] != null)
        {
            result = true;
        }
        return result;

    }

}
