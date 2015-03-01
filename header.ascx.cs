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
    public string gKlinika = "";
    //sluzbyclass mySluz = new sluzbyclass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
           /* if (Session["tuisegumdrum"] == null)
            {
                Response.Redirect("error.html");
            }*/
        }
        this.wgroup = Session["workgroup"].ToString();
        this.rights = Session["rights"].ToString();
        this.deps = Session["oddelenie"].ToString();
        this.gKlinika = Session["klinika"].ToString().ToLower();

        if (this.gKlinika == "kdch")
        {
            this.makeHeader();
        }

        if (this.gKlinika == "2dk")
        {
            this.makeHeaderDK();
        }
               
    }

    protected void makeHeaderDK()
    {
        DateTime dnes = DateTime.Today;
        DateTime vcera = DateTime.Today.AddDays(-1);

        string dnesStr = dnes.ToShortDateString();
        string vceraStr = vcera.ToShortDateString();

        StringBuilder sb = new StringBuilder();

        sb.Append("SELECT GROUP_CONCAT([t_s_dk].[typ] ORDER BY [t_s_dk].[ordering] SEPARATOR ';') AS [shift_type],");
        sb.AppendLine("GROUP_CONCAT(IFNULL([t_users].[name3],'-') ORDER BY [t_s_dk].[ordering] SEPARATOR ';') AS [doc_names],");
        sb.AppendLine("GROUP_CONCAT([t_s_dk].[comment] ORDER BY [t_s_dk].[ordering] SEPARATOR '|') AS [doc_comments]");
        sb.AppendLine("FROM [is_sluzby_dk] AS [t_s_dk]");
        sb.AppendLine("LEFT JOIN [is_users] AS [t_users] ON [t_users].[omega_ms_item_id] = [t_s_dk].[user_id]");
        sb.AppendFormat("WHERE [t_s_dk].[datum] ='{0}' OR [t_s_dk].[datum]='{1}'", my_x2.unixDate(dnes), my_x2.unixDate(vcera));
        sb.AppendFormat("AND [t_s_dk].[clinic]='{0}' GROUP BY [t_s_dk].[datum] ORDER BY [t_s_dk].[datum] DESC", Session["klinika_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        if (table.Count == 2)
        {
            string[] shiftType = table[0]["shift_type"].ToString().Split(';');
            string[] docBefore = table[0]["doc_names"].ToString().Split(';');
            string[] comments = table[0]["doc_comments"].ToString().Split('|');
            this.head1_lbl.Text = "";
            this.head2_lbl.Text = "";
            this.head3_lbl.Text = "";
            this.head4_lbl.Text = "";
            this.head5_lbl.Text = "";

            if (shiftType.Length == 6)
            {
                this.oup_lbl.Text = docBefore[0] + "(" + shiftType[0] + ") <br>" + comments[0];
                this.oup_lbl.Text += "<br>" + docBefore[1] + "(" + shiftType[1] + ") <br>" + comments[1];

                this.odda_lbl.Text = docBefore[2] + "(" + shiftType[2] + ") <br>" + comments[2];
                this.odda_lbl.Text += "<br>" + docBefore[3] + "(" + shiftType[3] + ") <br>" + comments[3];

                this.oddb_lbl.Text = docBefore[4] + "(" + shiftType[4] + ") <br>" + comments[4];

                this.op_lbl.Text = docBefore[5] + "(" + shiftType[5] + ") <br>" + comments[5];

                this.trp_lbl.Text = "...";
            }

            if (shiftType.Length == 5)
            {
                this.oup_lbl.Text = docBefore[0] + "(" + shiftType[0] + ") <br>" + comments[0];
                //this.oup_lbl.Text += "<br>" + docBefore[1] + "(" + shiftType[1] + ") <br>" + comments[1];

                this.odda_lbl.Text = docBefore[1] + "(" + shiftType[1] + ") <br>" + comments[1];
                // this.odda_lbl.Text += "<br>" + docBefore[3] + "(" + shiftType[3] + ") <br>" + comments[3];

                this.oddb_lbl.Text = docBefore[2] + "(" + shiftType[2] + ") <br>" + comments[2];

                this.op_lbl.Text = docBefore[3] + "(" + shiftType[3] + ") <br>" + comments[3];

                this.trp_lbl.Text = docBefore[4] + "(" + shiftType[4] + ") <br>" + comments[4];
            }



            this.po_lbl.Text = table[1]["doc_names"].ToString();

            date_lbl.Text = DateTime.Today.ToLongDateString();
            //this.trp_lbl.Text = docBefore[4].ToString() + "<br>" + comments[4].ToString();

            //this.po_lbl.Text = table[1]["users_names"].ToString();

            //date_lbl.Text = DateTime.Today.ToLongDateString();

            //            SELECT 
            //GROUP_CONCAT(`t_s_dk`.`typ` ORDER BY `t_s_dk`.`ordering` SEPARATOR ';') AS `shift_type`, 
            //GROUP_CONCAT(IFNULL(`t_users`.`name3`,'-') ORDER BY `t_s_dk`.`ordering` SEPARATOR ';') AS `doc_names`
            //FROM `is_sluzby_dk` AS `t_s_dk`
            //LEFT JOIN `is_users` AS `t_users` ON `t_users`.`omega_ms_item_id` = `t_s_dk`.`user_id`
            //WHERE `t_s_dk`.`datum`='2015-2-25' OR `t_s_dk`.`datum`='2015-2-24 23:59:00'
            //AND `t_s_dk`.`clinic`=4 
            //GROUP BY `t_s_dk`.`datum` 
        }
        else
        {
            this.msg_lbl.Text = Resources.Resource.shifts_not_done; 
        }


    }

    protected void makeHeader()
    {

        DateTime dnes = DateTime.Today;
        DateTime vcera = DateTime.Today.AddDays(-1);

        string dnesStr = dnes.ToShortDateString();
        string vceraStr = vcera.ToShortDateString();

        StringBuilder sb = new StringBuilder();
        if (this.wgroup == "doctor" || this.wgroup == "op" || this.wgroup == "other")
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
        if (this.wgroup == "nurse" || this.wgroup=="assistent")
        {
            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_2_sestr] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE ([t_sluzb].[datum]='{0}' OR [t_sluzb].[datum]='{1}')", my_x2.unixDate(vcera), my_x2.unixDate(dnes));
            sb.AppendFormat("AND [t_sluzb].[deps]='{0}'", this.deps);
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum] DESC");
        }

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        if (table.Count == 2)
        {
            string[] docBefore = table[0]["users_names"].ToString().Split(';');
            string[] comments = table[0]["comment"].ToString().Split('|');
            if (this.wgroup == "doctor" || this.wgroup=="other")
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

            if (this.wgroup == "nurse" || this.wgroup == "assistent")
            {
                this.head1_lbl.Text = "Den:<br>";
                this.oup_lbl.Text = docBefore[0].ToString() + "<div style='font-size:8px;'><em>(" + comments[0].ToString() + ")</em></div><br>";
                this.oup_lbl.Text += docBefore[1].ToString() + "<div style='font-size:8px;'><em>(" + comments[1].ToString() + ")</em></div><br>";
                this.oup_lbl.Text += docBefore[2].ToString() + "<div style='font-size:8px;'><em>(" + comments[2].ToString() + ")</em></div><br>";

                this.head2_lbl.Text = "Ranka.";
                this.odda_lbl.Text = docBefore[3].ToString() + "<div style='font-size:8px;'><em>(" + comments[3].ToString()+ ")</em></div>";

                this.head3_lbl.Text = "Sanit.1";
                this.oddb_lbl.Text = docBefore[4].ToString() + "<div style='font-size:8px;'><em>(" + comments[4].ToString() + ")</em></div>";

                this.head4_lbl.Text = "Sanit.2";
                this.op_lbl.Text = docBefore[5].ToString() + "<div style='font-size:8px;'><em>(" + comments[5].ToString() + ")</em></div>";

                this.head5_lbl.Text = "Noc:";
                this.trp_lbl.Text = docBefore[6].ToString() + "<div style='font-size:8px;'><em>(" + comments[6].ToString() + ")</em></div><br>";
                this.trp_lbl.Text += docBefore[7].ToString() + "<div style='font-size:8px;'><em>(" + comments[7].ToString() + ")</em></div><br>";
                this.trp_lbl.Text += docBefore[8].ToString() + "<div style='font-size:8px;'><em>(" + comments[8].ToString() + ")</em></div><br>";
              
                if (table[1]["users_names"] != null)
                {
                    this.po_lbl.Text = table[1]["users_names"].ToString();
                }
                else
                {
                    this.po_lbl.Text = "";
                }
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
