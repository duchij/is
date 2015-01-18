using System;
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

public partial class MasterPage : System.Web.UI.MasterPage
{
    public x2_var X2 = new x2_var();
    public my_db oldDb = new my_db();
    public mysql_db x2Mysql = new mysql_db();
    public string rights="";
    public string wgroup = "";
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
    protected void Page_Load(object sender, EventArgs e)
    {
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

        if (this.wgroup == "nurse")
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
        this.shiftsOfCurrentUser();


    }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.info_plh.Visible = false;
    }



    protected void log_out_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        //Session.Clear();
       // Response.Cookies["tuisegumdrum"].Expires = DateTime.Now.AddDays(-1);
        //Response.Cookies.Clear();
        Response.Redirect("Default.aspx");
    }

    protected void showInfoMessage()
    {
        
            this.info_plh.Visible = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT [cela_sprava],[cielova-skupina] FROM [is_news] WHERE [id]={0}", Convert.ToInt32(Session["newsToShow"]));

            SortedList row = x2Mysql.getRow(sb.ToString());

            if (row["cielova-skupina"].ToString() == "doctors" && (this.rights == "users" || this.rights == "poweruser"))
            {
                this.info_message_lbl.Text = row["cela_sprava"].ToString();
            }
            else if (row["cielova-skupina"].ToString() == "nurses" && this.rights.IndexOf("sestra") != -1 )
            {
                this.info_message_lbl.Text = row["cela_sprava"].ToString();
            }
            else if (row["cielova-skupina"].ToString() == "all")
            {
                this.info_message_lbl.Text = row["cela_sprava"].ToString();
            }
            else
            {
                this.info_plh.Visible = false;
            }

            
       
        Session["newsToShow"]="";
    }


    protected void shiftsOfCurrentUser()
    {
        int dateGroup = X2.makeDateGroup(DateTime.Today.Year, DateTime.Today.Month);
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [datum] FROM [is_sluzby_2] WHERE [date_group] ='{0}' AND [user_id] = '{1}' AND [typ]<>'prijm' ORDER BY [datum]", dateGroup, Session["user_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());
        string[] shifts = new string[table.Count];
        if (table.Count > 0)
        {
            int tblLen = table.Count;
            for (int i = 0; i < tblLen; i++)
            {
                shifts[i] = X2.UnixToMsDateTime(table[i]["datum"].ToString()).ToString("d");
            }
            this.currentShifts_lbl.Text = String.Join(", ",shifts);

        }

    }
}
