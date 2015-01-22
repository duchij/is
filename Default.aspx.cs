﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;

public partial class _Default : System.Web.UI.Page
{
    my_db db_obj = new my_db();
    x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        //System.Web.HttpBrowserCapabilities browser = Request.Browser;
        /*Boolean status = x2.offline();

        if (status == false)
        {
            Session.Clear();
            Response.Redirect("offline.html");
        }*/

        


        //Response.Cookies["akt_sluzba"].Expires = DateTime.Now.AddDays(-1);
        //Response.Cookies["akt_hlasenie "].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["tuisegumdrum"].Expires = DateTime.Now.AddDays(-1);

        if (Request.Browser.Cookies == false)
        {
            Login1.Visible = false;
            info_txt.Text = "Máte deaktiované cookies, na to aby tento formulár fungoval, ich musíte zapnúť!!!!";
        }
        else
        {

        }

    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {

        x2_var my_x2 = new x2_var();
        string passwd = Login1.Password.ToString().Trim();
        string l_pass = my_x2.make_hash(passwd);
        string g_pass = "";

        SortedList data = new SortedList();
        string userName = Login1.UserName.ToString().Trim();
        
        if (my_x2.isAlfaNum(userName))
        {

            data = db_obj.getUserPasswd(userName);

            if (data.Count != 0 && data["active"].ToString() == "1")
            {

                g_pass = data["passwd"].ToString();

                if (Login1.UserName == passwd && g_pass == "NULL" && data["name"].ToString() == passwd)
                {


                    Session.Add("tuisegumdrum", "activado");
                    Session.Add("user_id", data["id"].ToString());
                    Session.Add("rights", data["prava"].ToString());
                    Session.Add("workgroup", data["work_group"].ToString());
                    Session.Add("fullname", data["full_name"].ToString());
                    Session.Add("login", data["name"].ToString());
                    Session.Add("email", data["email"].ToString());

                    Session.Add("klinika", data["clinics_idf"].ToString());
                    Session.Add("oddelenie", data["deps_idf"].ToString()); 

                    Session.Add("pracdoba", data["pracdoba"].ToString());
                    Session.Add("tyzdoba", data["tyzdoba"].ToString());
                    Session.Add("osobcisl", data["osobcisl"].ToString());

                    Session.Add("titul_pred", data["titul_pred"].ToString());
                    Session.Add("titul_za", data["titul_za"].ToString());



                   /* Response.Cookies["tuisegumdrum"].Value = "activado";
                    Response.Cookies["user_id"].Value = data["id"].ToString();
                    Response.Cookies["rights"].Value = data["group"].ToString();
                    Response.Cookies["fullname"].Value = data["full_name"].ToString();
                    Response.Cookies["login"].Value = data["name"].ToString();
                    Response.Cookies["email"].Value = data["email"].ToString(); */

                    Response.Redirect("passch.aspx");
                }

                if (l_pass == g_pass)
                {
                    e.Authenticated = true;
                    this.deleteFilesPerDays();
                    Session.Add("tuisegumdrum", "activado");
                    Session.Add("user_id", data["id"].ToString());
                    Session.Add("rights", data["prava"].ToString());
                    Session.Add("workgroup", data["work_group"].ToString());
                    Session.Add("fullname", data["full_name"].ToString());
                    Session.Add("login", data["name"].ToString());
                    Session.Add("email", data["email"].ToString());

                    Session.Add("pracdoba", data["pracdoba"].ToString());
                    Session.Add("tyzdoba", data["tyzdoba"].ToString());
                    Session.Add("osobcisl", data["osobcisl"].ToString());

                    Session.Add("klinika", data["clinics_idf"].ToString());
                    Session.Add("oddelenie", data["deps_idf"].ToString());

                    Session.Add("titul_pred", data["titul_pred"].ToString());
                    Session.Add("titul_za", data["titul_za"].ToString());

                    Session.Add("zaradenie", data["zaradenie"].ToString());

                    SortedList passPhrase = db_obj.getPassPhrase();
                    Session.Add("passphrase", passPhrase["data"].ToString());


                    /*Response.Cookies["tuisegumdrum"].Value = " activado";
                    Response.Cookies["user_id"].Value = data["id"].ToString();
                    Response.Cookies["rights"].Value = data["group"].ToString();
                    Response.Cookies["fullname"].Value = data["full_name"].ToString();
                    Response.Cookies["login"].Value = data["name"].ToString();
                    Response.Cookies["email"].Value = data["email"].ToString(); */

                    List<string> news = db_obj.getLastNews();

                    if (news.Count > 0)
                    {
                        Session["newsToShow"] = news[0];
                    }


                    if (data["name"].ToString() == "vtablet")
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
                                Response.Redirect(@"poziadavky.aspx?a=1");
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
                    else if(Session["workgroup"].ToString() == "nurse")
                    {
                        //SortedList result = db_obj.getNextPozDatum(DateTime.Today);
                        Response.Redirect(@"sestrhlas.aspx");
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
                }
            }
            else
            {
                e.Authenticated = false;
            }
        }
    }

    protected void deleteFilesPerDays()
    {
        List<string> filesToDelete = db_obj.loadTmpFilesToDelete();

        if (filesToDelete.Count > 0)
        {
            for (int i = 0; i < filesToDelete.Count; i++)
            {
                if (File.Exists(@Server.MapPath("App_Data") + @"\" + filesToDelete[i])) ;
                {
                    File.Delete(@Server.MapPath("App_Data") + @"\" + filesToDelete[i]);
                }
            }
            db_obj.deleteFilesInDb();
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
