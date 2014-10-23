using System;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        //System.Web.HttpBrowserCapabilities browser = Request.Browser;
        bool isIdevice = false;
        if (Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone"))
        {
            isIdevice = true;
        }
       // bool isIpad = Request.UserAgent.ToLower().Contains("ipad");
        if (isIdevice)
        {
            this.Login1.Width = Unit.Pixel(400);
            this.Login1.CssClass = "duchsB";
            
            
            //this.Login1.TitleTextStyle.Width = Unit.Percentage(200);
        }


        Response.Cookies["akt_sluzba"].Expires = DateTime.Now.AddDays(-1);
        Response.Cookies["akt_hlasenie "].Expires = DateTime.Now.AddDays(-1);
        //Response.Cookies["tuisegumdrum"].Expires = DateTime.Now.AddDays(-1);

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

        string l_pass = my_x2.make_hash(Login1.Password);
        string g_pass = "";

        SortedList data = new SortedList();

        if (my_x2.isAlfaNum(Login1.UserName))
        {
            
            data =  db_obj.getUserPasswd(Login1.UserName);

            if (data.Count != 0 && data["active"].ToString() == "1")
            {

                g_pass = data["passwd"].ToString();

                if (Login1.UserName == Login1.Password && g_pass.Length == 0 && data["name"].ToString() == Login1.Password  )
                {
                    Session.Add("tuisegumdrum", "activado");
                    Session.Add("user_id", data["id"].ToString());
                    Session.Add("rights",data["group"].ToString());
                    Session.Add("fullname", data["full_name"].ToString());
                    Session.Add("login", data["name"].ToString());
                    Session.Add("email", data["email"].ToString());

                    Session.Add("pracdoba", data["pracdoba"].ToString());
                    Session.Add("tyzdoba", data["tyzdoba"].ToString());
                    Session.Add("osobcisl", data["osobcisl"].ToString());



                    Response.Cookies["tuisegumdrum"].Value = "activado";
                    Response.Cookies["user_id"].Value = data["id"].ToString();
                    Response.Cookies["rights"].Value = data["group"].ToString();
                    Response.Cookies["fullname"].Value = data["full_name"].ToString();
                    Response.Cookies["login"].Value = data["name"].ToString();
                    Response.Cookies["email"].Value = data["email"].ToString();

                    Response.Redirect("passch.aspx");
                }

                if (l_pass == g_pass) 
                {
                   


                    e.Authenticated = true;

                    this.deleteFilesPerDays();

                    if (data["name"].ToString() == "vtablet")
                    {
                        Response.Redirect("tabletview.aspx");
                    
                    }


                    Session.Add("tuisegumdrum", "activado");
                    Session.Add("user_id", data["id"].ToString());
                    Session.Add("rights", data["group"].ToString());
                    Session.Add("fullname", data["full_name"].ToString());
                    Session.Add("login", data["name"].ToString());
                    Session.Add("email", data["email"].ToString());

                    Session.Add("pracdoba", data["pracdoba"].ToString());
                    Session.Add("tyzdoba", data["tyzdoba"].ToString());
                    Session.Add("osobcisl",data["osobcisl"].ToString());


                    

                    Response.Cookies["tuisegumdrum"].Value = " activado";
                    Response.Cookies["user_id"].Value = data["id"].ToString();
                    Response.Cookies["rights"].Value = data["group"].ToString();
                    Response.Cookies["fullname"].Value = data["full_name"].ToString();
                    Response.Cookies["login"].Value = data["name"].ToString();
                    Response.Cookies["email"].Value = data["email"].ToString();



                  

                    string group = data["group"].ToString();

                    if (group.IndexOf("sestra") == -1 && group.IndexOf("medix") == -1)
                    {
                        //ideme zistovat poziadavky

                            SortedList poz_data = db_obj.getNextPozDatum(DateTime.Today);

                            if (this.maVyplnPoziadavky(Response.Cookies["user_id"].Value.ToString()) == true)
                            {
                                List<string> news = db_obj.getLastNews();

                                if (news.Count > 0)
                                {
                                    Session["newsToShow"] = news[0];
                                    Response.Redirect(@"hlasko.aspx");
                                }
                                else
                                {
                                    Response.Redirect(@"hlasko.aspx");
                                }
                            }
                            else
                            {
                                List<string> news = db_obj.getLastNews();

                                if (news.Count > 0)
                                {
                                    Response.Redirect(@"hlasko.aspx?news=" + news[0]);
                                }
                                
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
                    else
                    {
                        //SortedList result = db_obj.getNextPozDatum(DateTime.Today);
                        Response.Redirect(@"sestrhlas.aspx");
                    }
                    //Response.Write("hura");

                }
            
                else
                {
                    e.Authenticated = false;
                    //Response.Write("Zle");
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
            for (int i=0; i<filesToDelete.Count; i++)
            {
                if (File.Exists(@Server.MapPath("App_Data")+@"\"+filesToDelete[i]));
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
