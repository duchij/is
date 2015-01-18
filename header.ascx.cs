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

public partial class header : System.Web.UI.UserControl
{
    my_db x_db = new my_db();
    x2_var my_x2 = new x2_var();
    public mysql_db x2Mysql = new mysql_db();

    public string deps = "";
    public string rights = "";
    public string wgroup = "";
    sluzbyclass mySluz = new sluzbyclass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            if (Session["tuisegumdrum"] == null)
            {
                Response.Redirect("error.html");
            }
        }
        this.wgroup = Session["workgroup"].ToString();
        this.rights = Session["rights"].ToString();
        this.deps = Session["oddelenie"].ToString();

        this.makeHeader();
               
    }

    protected void makeHeader()
    {

        DateTime dnes = DateTime.Today;
        DateTime vcera = DateTime.Today.AddDays(-1);

        string dnesStr = dnes.ToShortDateString();
        string vceraStr = vcera.ToShortDateString();

        StringBuilder sb = new StringBuilder();
        if (this.wgroup == "doctor")
        {

            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[datum]='{0}' OR [t_sluzb].[datum]='{1}'", my_x2.unixDate(vcera), my_x2.unixDate(dnes));
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum] DESC");
        }
        if (this.wgroup == "nurse")
        {
            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_2_sestr] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[datum]='{0}' OR [t_sluzb].[datum]='{1}'", my_x2.unixDate(vcera), my_x2.unixDate(dnes));
            sb.AppendFormat("AND [t_sluzb].[deps]='{0}'", this.deps);
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum] DESC");
        }

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        if (table.Count > 0)
        {
            string[] docBefore = table[0]["users_names"].ToString().Split(';');
            string[] comments = table[0]["comment"].ToString().Split('|');
            if (this.wgroup == "doctor")
            {
                
                // string[] docAfter = table[1]["users_names"].ToString().Split(';');

                this.oup_lbl.Text = docBefore[0].ToString() + "<br>" + comments[0].ToString();
                this.odda_lbl.Text = docBefore[1].ToString() + "<br>" + comments[1].ToString();
                this.oddb_lbl.Text = docBefore[2].ToString() + "<br>" + comments[2].ToString();
                this.op_lbl.Text = docBefore[3].ToString() + "<br>" + comments[3].ToString();
                this.trp_lbl.Text = docBefore[4].ToString() + "<br>" + comments[4].ToString();

                this.po_lbl.Text = table[1]["users_names"].ToString();

                date_lbl.Text = DateTime.Today.ToLongDateString();

                SortedList data = x_db.getNextPozDatum(DateTime.Today);
                poziadav_lbl.Text = data["datum"].ToString();
            }
            if (this.wgroup == "nurse")
            {
                this.head1_lbl.Text = "Den:";
                this.oup_lbl.Text = docBefore[0].ToString() + "<br>" + comments[0].ToString() + "<br>";
                this.oup_lbl.Text += docBefore[1].ToString() + "<br>" + comments[1].ToString() + "<br>";
                this.oup_lbl.Text += docBefore[2].ToString() + "<br>" + comments[2].ToString() + "<br>";

                this.head2_lbl.Text = "Ranka.";
                this.odda_lbl.Text = docBefore[3].ToString() + "<br>" + comments[3].ToString();

                this.head3_lbl.Text = "Sanit.1";
                this.oddb_lbl.Text = docBefore[4].ToString() + "<br>" + comments[4].ToString();

                this.head4_lbl.Text = "Sanit.2";
                this.op_lbl.Text = docBefore[5].ToString() + "<br>" + comments[5].ToString();

                this.head5_lbl.Text = "Noc:";
                this.trp_lbl.Text = docBefore[6].ToString() + "<br>" + comments[6].ToString() + "<br>";
                this.trp_lbl.Text += docBefore[7].ToString() + "<br>" + comments[7].ToString() + "<br>";
                this.trp_lbl.Text += docBefore[8].ToString() + "<br>" + comments[8].ToString() + "<br>";

                this.po_lbl.Text = table[1]["users_names"].ToString();
                this.poziadav_lbl.Text = "";

                this.date_lbl.Text = DateTime.Today.ToLongDateString();

            }
        }
        else
        {
            this.msg_lbl.Text = Resources.Resource.shifts_not_done;           
            //int res =  x2Mysql.fillDocShifts(my_x2.makeDateGroup(rok, mesiac), dni, mesiac, rok);
            //this.makeHeader();
        }

    }

   



}
