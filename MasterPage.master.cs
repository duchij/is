using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//public delegate void setWebState (object sender,System.EventArgs e);

public partial class MasterPage : System.Web.UI.MasterPage
{
    //public event setWebState doSetWebState;

    public x2_var X2 = new x2_var();
    public my_db oldDb = new my_db();
    public mysql_db x2Mysql = new mysql_db();
    public string rights="";
    public string wgroup = "";
    public string gKlinika;

    /* public string PageName
    {
       get
        {
            return lblpageName.Text;
        }
        set
        {
            lblpageName.Text = value;
        }
    }*/
    //protected virtual void _setEx(object sender,EventArgs e)
   // {
       // doSetWebState(sender, e);
    //}

    protected void Page_PreInit(object sender, EventArgs e)
    {
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        
        this.gKlinika = Session["klinika"].ToString().Trim().ToLower();
        this.web_titel.Text = X2.setLabel(Session["klinika"].ToString().ToLower()+"_web_titel");

        this.current_user_lbl.Text = Session["fullname"].ToString();
        this.rights = Session["rights"].ToString();
        this.wgroup = Session["workgroup"].ToString();

        if (Session["newsToShow"] != null && !IsPostBack)
        {
            if (Session["newsToShow"].ToString().Length != 0)
            {
               this.showInfoMessage();
            }
            else
            {
                Session["newsToShow"] = "";
            }
        }
        else
        {            
            this.info_plh.Visible = false;
        }

        if (this.wgroup == "nurse" || this.wgroup == "assistent")
        {
            this.hlas_lekar_plh.Visible = false;
            this.hlas_sestier_plh.Visible = true;
        }

        if (this.wgroup =="doctor")
        {
            this.hlas_lekar_plh.Visible = true;
            this.hlas_sestier_plh.Visible = false;
        }

        if (this.rights == "admin")
        {
            this.hlas_lekar_plh.Visible = true;
            this.hlas_sestier_plh.Visible = true;
        }
        //this.loadLogo();
        //this.master_lbl.Text = this.gKlinika;
        this.shiftsOfCurrentUser();
        this.seminarsOCurrentUser();

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        left_menu.setStateButton += new EventHandler(stateIt);
        this.info_plh.Visible = false;
        
        
    }

    //protected void loadLogo()
    //{
    //    SortedList row = x2Mysql.getRow("SELECT [data] FROM [is_settings] WHERE [name] = 'logo'");
    //    if (row["data"]!=null)
    //    {
    //        string serverUrl = Session["serverUrl"].ToString();

    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendFormat(@"<img src='App_Data/logo/{0}'>",row["data"].ToString());

    //        this.logo.Text = sb.ToString();
    //    }
    //}

    protected void stateIt(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string id = btn.ID.ToString();

        if (id == "offline_btn")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE [is_settings] SET [data]='false' WHERE [name]='webstatus'");

            x2Mysql.execute(sb.ToString());
            string serverUrl = Session["serverUrl"].ToString();

            if (File.Exists(serverUrl+ @"\app_offline.ina"))
            {
                try
                {
                    File.Move(serverUrl + @"\app_offline.ina", serverUrl + @"\app_offline.htm");
                    Session.Abandon();
                }
                catch (Exception exc)
                {
                    Session.Abandon();
                }
                
            }
        }

        if (id == "online_btn")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE [is_settings] SET [data]='true' WHERE [name]='webstatus'");
            x2Mysql.execute(sb.ToString());
            //string serverUrl = Session["serverUrl"].ToString();

            /*if (File.Exists(serverUrl + @"\app_offline.htm"))
            {
                File.Move(serverUrl + @"\app_offline.htm", serverUrl + @"\app_offline.ina");
            }*/


        }
    }



    protected void log_out_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<script language=javascript>");
        sb.AppendLine("var myWin = window.open('','_parent','');");
        sb.AppendLine("myWin.close();</script>");
        Session.Abandon();
        Response.Redirect("Default.aspx");
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", sb.ToString());
    }




    protected void showInfoMessage()
    {
        
            this.info_plh.Visible = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT [cela_sprava],[cielova_skupina],[id] FROM [is_news] WHERE [id]={0}", Convert.ToInt32(Session["newsToShow"]));

            SortedList row = x2Mysql.getRow(sb.ToString());

            string spravaDialog = row["cela_sprava"].ToString();
            
            string sprava = X2.stringFrom64(row["cela_sprava"].ToString());

            int pos = sprava.IndexOf("-viac-");

            if (pos != -1)
            {

                sprava = sprava.Substring(0, pos);
            }

            string link = " <p> <a href='is_news_show.aspx?id={0}' target='_self'>Celá správa >>>></a> </p>";

            link = X2.sprintf(link, new string[] { row["id"].ToString() });

            sprava = sprava + link;

            if (row["cielova_skupina"].ToString() == "doctors" && this.wgroup == "doctor")
            {
                this.info_message_lbl.Text = sprava;
            }
            else if (row["cielova_skupina"].ToString() == "nurses" && this.wgroup=="nurse" )
            {
                this.info_message_lbl.Text = sprava;
            }
            else if (row["cielova_skupina"].ToString() == "all")
            {
                this.info_message_lbl.Text = sprava;
            }
            else
            {
                this.info_plh.Visible = false;
            }

            
       
        Session["newsToShow"]="";
        Session["newsToShowDialog"] = spravaDialog;
        
    }


    protected void shiftsOfCurrentUser()
    {
        int dateGroup = X2.makeDateGroup(DateTime.Today.Year, DateTime.Today.Month);
        StringBuilder sb = new StringBuilder();

        switch(this.gKlinika)
        {
            case "kdch":
            if (this.wgroup == "doctor")
            {
                sb.AppendFormat("SELECT [datum],[typ] FROM [is_sluzby_2] WHERE [date_group] ='{0}' AND [user_id] = '{1}' ORDER BY [datum]", dateGroup, Session["user_id"]);
            }
            else
            {
                sb.AppendFormat("SELECT [datum],[typ] FROM [is_sluzby_2_sestr] WHERE [date_group] ='{0}' AND [user_id] = '{1}' AND [deps]='{2}' ORDER BY [datum]", dateGroup, Session["user_id"], Session["oddelenie"]);
            }
            break;
        case "2dk":
        
            if (this.wgroup == "doctor")
            {
                sb.AppendFormat("SELECT [datum],[typ] FROM [is_sluzby_dk] WHERE [date_group] ='{0}' AND [user_id] = '{1}' ORDER BY [datum]", dateGroup, Session["user_id"]);
            }
            break;
            case "nkim":
                if (this.wgroup == "doctor")
                {
                    sb.AppendFormat("SELECT [datum],[typ] FROM [is_sluzby_all] WHERE [date_group] ='{0}' AND [user_id] = '{1}' ORDER BY [datum]", dateGroup, Session["user_id"]);
                }
            break;
            case "kdhao":
            if (this.wgroup == "doctor")
            {
                sb.AppendFormat("SELECT [datum],[typ] FROM [is_sluzby_all] WHERE [date_group] ='{0}' AND [user_id] = '{1}' ORDER BY [datum]", dateGroup, Session["user_id"]);
            }
            break;

        }
        string query = sb.ToString();
        query = x2Mysql.buildSql(query, new string[] { });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        string[] shifts = new string[table.Count];

        if (table.Count > 0)
        {
            int tblLen = table.Count;
            for (int i = 0; i < tblLen; i++)
            {
                if (this.wgroup == "doctor")
                {
                    shifts[i] = X2.UnixToMsDateTime(table[i]["datum"].ToString()).ToString("d.M") + " (" + table[i]["typ"].ToString() + ")";
                }
                else
                {
                    shifts[i] = X2.UnixToMsDateTime(table[i]["datum"].ToString()).ToString("d.M") + " (" + table[i]["typ"].ToString() + ")";
                }
            }
            this.currentShifts_lbl.Text =String.Join(", ",shifts);

        }

    }

    protected void seminarsOCurrentUser()
    {
        int month = DateTime.Today.Month;
        int year = DateTime.Today.Year;
        int user_id = Convert.ToInt32(Session["user_id"].ToString());

        string query = @"SELECT [is_seminars.date] AS [date],[is_seminars.tema] AS [tema] ,[users.name3] AS [name] FROM [is_seminars] 
                            INNER JOIN [is_users] AS [users] ON [users.id] = [is_seminars.user_id]
                            WHERE [is_seminars.user_id] = {0} OR [users.name]='admin'
                                AND YEAR([is_seminars.date])={1} AND MONTH([is_seminars.date])={2}";
        query = x2Mysql.buildSql(query, new string[] { user_id.ToString(), year.ToString(), month.ToString() });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        int tbLn = table.Count;

        if (tbLn > 0)
        {
            this.seminars_lbl.Text = "<strong class='green'>Moje seminare: </strong>";
            string sems="";
            for (int s=0; s< tbLn; s++)
            {
                DateTime dt = Convert.ToDateTime(X2.MSDate(table[s]["date"].ToString()));

                sems += " <strong class='red'>"+dt.ToShortDateString() + "</strong>- " + X2.getStr(table[s]["tema"].ToString());
            }
            this.seminars_lbl.Text += sems;
        }






    }
}
