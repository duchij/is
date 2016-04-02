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

        switch (this.gKlinika)
        {
            case "kdch":
                this.makeHeader();
                break;
            case "2dk":
                this.makeHeaderDK();
                break;
            case "1dk":
                this.makeHeaderDK();
                break;
            case "nkim":
                this.makeHeaderNkim();
                break;
            case "kdhao":
                this.makeHeaderKdhao();
                break;
        }
               
    }

    protected void makeHeaderDK()
    {
        DateTime dnes = DateTime.Today;
        DateTime vcera = DateTime.Today.AddDays(-1);

        string dnesStr = dnes.ToShortDateString();
        string vceraStr = vcera.ToShortDateString();
        this.date_lbl.Text = DateTime.Today.ToLongDateString();
        string query = "";
        if (this.wgroup == "doctor")
        {

            query = @"
                        SELECT 
                                GROUP_CONCAT(CONCAT_WS(';',[t_sluz.typ],[t_user.name3],[t_sluz.comment]) SEPARATOR '|') AS [sluzba]
                                FROM [is_sluzby_all] AS [t_sluz] 
                            INNER JOIN [is_users] AS [t_user] ON [t_user.id] = [t_sluz.user_id]
                                WHERE [t_sluz.datum] BETWEEN '{0}' AND '{1}'
                                    AND [clinic]={2}
                                    GROUP BY [t_sluz.datum]
                                    ORDER BY [t_sluz.datum] DESC";
        }

        query = x2Mysql.buildSql(query, new string[] { my_x2.unixDate(vcera), my_x2.unixDate(dnes), Session["klinika_id"].ToString() });
       

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);
        this.head1_lbl.Text = "Odd: ";
        this.head2_lbl.Text = "OupA:";
        this.head3_lbl.Text = "OupB:";
        this.head4_lbl.Text = "Expe:";
        this.head5_lbl.Text = "KlAmb:";


        if (table.Count >0 )
        {
            this.oup_lbl.Text = "";
            this.odda_lbl.Text = "";
            this.oddb_lbl.Text = "";
            this.op_lbl.Text = "";
            this.trp_lbl.Text = "";

            string[] sluzbDnes = table[0]["sluzba"].ToString().Split('|');

            int sluzLn = sluzbDnes.Length;

            SortedList sluzba = new SortedList();
            SortedList notes = new SortedList();
            for (int s = 0; s < sluzLn; s++)
            {
                string[] tmp = sluzbDnes[s].Split(';');
                sluzba.Add(tmp[0], tmp[1]);
                notes.Add(tmp[0], tmp[2]);
            }

            if (sluzba["Odd"] != null)
            {
                this.oup_lbl.Text = "Odd: "+sluzba["Odd"].ToString() + "<br>" + notes["Odd"].ToString();
            }

            if (sluzba["Odd2"] != null)
            {
                this.oup_lbl.Text += "<br>Odd2"+sluzba["Odd2"].ToString() + "<br>" + notes["Odd2"].ToString();
            }

            if (sluzba["OupA"] != null)
            {
                this.odda_lbl.Text += sluzba["OupA"].ToString() + "<br>" + notes["OupA"].ToString();
            }

            if (sluzba["OupB"] != null)
            {
                this.oddb_lbl.Text += sluzba["OupB"].ToString() + "<br>" + notes["OupB"].ToString();
            }

            if (sluzba["Expe"] != null)
            {
                this.op_lbl.Text += sluzba["Expe"].ToString() + "<br>" + notes["Expe"].ToString();
            }

            if (sluzba["KlAmb"] != null)
            {
                this.trp_lbl.Text += sluzba["KlAmb"].ToString() + "<br>" + notes["KlAmb"].ToString();
            }
            if (table.Count>1)
            {
                string[] sluzbaVcera = table[1]["sluzba"].ToString().Split('|');
                int vceraLn = sluzbaVcera.Length;

                sluzba.Clear();

                for (int v = 0; v < vceraLn; v++)
                {
                    string[] tmp = sluzbaVcera[v].Split(';');
                    sluzba.Add(tmp[0], tmp[1] + "<em> /" + tmp[2] + "/</em>");
                }
                this.po_lbl.Text = "<p class='small'>";
                foreach (DictionaryEntry row in sluzba)
                {
                    this.po_lbl.Text += row.Value.ToString() + "(" + row.Key.ToString() + "), ";
                }
                this.po_lbl.Text += "</p>";
            }
            

            date_lbl.Text = DateTime.Today.ToLongDateString();

            SortedList data = x_db.getNextPozDatum(DateTime.Today);
            poziadav_lbl.Text = data["datum"].ToString();

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

        string query ="";
        this.date_lbl.Text = DateTime.Today.ToLongDateString();
        if (this.wgroup == "doctor" || this.wgroup == "op" || this.wgroup == "other")
        {

            query = @"
                        SELECT 
                                GROUP_CONCAT(CONCAT_WS(';',[t_sluz.typ],[t_user.name3],[t_sluz.comment]) SEPARATOR '|') AS [sluzba]
                                FROM [is_sluzby_2] AS [t_sluz] 
                            INNER JOIN [is_users] AS [t_user] ON [t_user.id] = [t_sluz.user_id]
                                WHERE [t_sluz.datum]='{0}' OR  [t_sluz.datum]='{1}'
                                    GROUP BY [t_sluz.datum]
                                    ORDER BY [t_sluz.datum] DESC";

            query = x2Mysql.buildSql(query, new string[] { my_x2.unixDate(vcera), my_x2.unixDate(dnes) });


        }
        if (this.wgroup == "nurse" || this.wgroup=="assistent")
        {
                query = @"SELECT 
                                GROUP_CONCAT(CONCAT_WS(';',[t_sluzb.typ],[t_users.name3]) SEPARATOR '|') AS [sluzba], 
                                [t_sluzb.datum] AS [datum] 
                            FROM [is_sluzby_2_sestr] AS [t_sluzb] 
                                INNER JOIN [is_users] AS [t_users] ON [t_users.id] = [t_sluzb.user_id]
                            WHERE [datum] = '{0}' OR [datum]='{1}'
                            AND [deps] = '{2}'
                            GROUP BY [datum] DESC
                            ";
            query = x2Mysql.buildSql(query, new string[] { my_x2.unixDate(vcera), my_x2.unixDate(dnes), this.deps });
        }

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        if (table.Count == 2)
        {
            if (this.wgroup == "doctor" || this.wgroup=="other")
            {
                
                this.oup_lbl.Text = "";
                this.odda_lbl.Text = "";
                this.oddb_lbl.Text = "";
                this.op_lbl.Text = "";
                this.trp_lbl.Text = "";
                string[] sluzbDnes = table[0]["sluzba"].ToString().Split('|');

                int sluzLn = sluzbDnes.Length;

                SortedList sluzba = new SortedList();
                SortedList notes = new SortedList();
                for (int s=0; s< sluzLn; s++)
                {
                    string[] tmp = sluzbDnes[s].Split(';');
                    sluzba.Add(tmp[0], tmp[1]);
                    notes.Add(tmp[0], tmp[2]);
                }

                if (sluzba["OUP"] != null)
                {
                    this.oup_lbl.Text = sluzba["OUP"].ToString() + "<br>" + notes["OUP"].ToString();
                }
                if (sluzba["OddA"] != null)
                {
                    this.odda_lbl.Text = sluzba["OddA"].ToString() + "<br>" + notes["OddA"].ToString();
                }
               
                if (sluzba["OddB"] != null)
                {
                    this.oddb_lbl.Text = sluzba["OddB"].ToString() + "<br>" + notes["OddB"].ToString();
                }
                    
                if (sluzba["OP"] != null)
                {
                    this.op_lbl.Text = sluzba["OP"].ToString() + "<br>" + notes["OP"].ToString();
                }

                if (sluzba["Prijm"]!=null)
                {
                    this.trp_lbl.Text = "Prijm:"+sluzba["Prijm"].ToString() + "<br>" + notes["Prijm"].ToString();
                }
                if (sluzba["Vseob"] != null)
                {
                    this.trp_lbl.Text +="<br>Vseob:"+ sluzba["Vseob"].ToString() + "<br>" + notes["Vseob"].ToString();
                }
                if (sluzba["Uraz"] != null)
                {
                    this.trp_lbl.Text += "<br>Uraz:" + sluzba["Uraz"].ToString() + "<br>" + notes["Uraz"].ToString();
                }

                string[] sluzbaVcera = table[1]["sluzba"].ToString().Split('|');
                int vceraLn = sluzbaVcera.Length;

                sluzba.Clear();
                
                for (int v=0; v<vceraLn; v++)
                {
                    string[] tmp = sluzbaVcera[v].Split(';');
                    sluzba.Add(tmp[0], tmp[1] + "<em> /" + tmp[2] + "/</em>");
                }
                this.po_lbl.Text = "<p class='small'>";
                foreach (DictionaryEntry row in sluzba)
                {
                    this.po_lbl.Text += row.Value.ToString()+"("+row.Key.ToString()+"), ";
                }
                this.po_lbl.Text += "</p>";

                date_lbl.Text = DateTime.Today.ToLongDateString();

                SortedList data = x_db.getNextPozDatum(DateTime.Today);
                poziadav_lbl.Text = data["datum"].ToString();
            }

            if (this.wgroup == "nurse" || this.wgroup == "assistent")
            {
               // string 
                this.head1_lbl.Text = "Den:<br>";
                string[] sluzbDnes = table[0]["sluzba"].ToString().Split('|');

                SortedList data = new SortedList();

                int slLn = sluzbDnes.Length;

                for (int i = 0; i < slLn; i++ )
                {
                    string[] tmp = sluzbDnes[i].Split(';');
                    data.Add(tmp[0], tmp[1]);
                }

                if (data["D1"] != null)
                {
                    this.oup_lbl.Text = data["D1"].ToString()+"<br>";// +"<div style='font-size:8px;'><em>(" + comments[0].ToString() + ")</em></div><br>";
                }
                if (data["D2"] != null)
                {
                    this.oup_lbl.Text += data["D2"].ToString()+"<br>"; // + "<div style='font-size:8px;'><em>(" + comments[1].ToString() + ")</em></div><br>";
                }
                if (data["A1"] != null)
                {
                    this.oup_lbl.Text += data["A1"].ToString();// + "<div style='font-size:8px;'><em>(" + comments[2].ToString() + ")</em></div><br>";
                }
                this.head2_lbl.Text = "Ranka.";
                if (data["RA"] != null)
                {
                    this.odda_lbl.Text = data["RA"].ToString()+"<br>";// +"<div style='font-size:8px;'><em>(" + comments[3].ToString() + ")</em></div>";
                }
                if (data["RA2"] != null)
                {
                    this.odda_lbl.Text += data["RA2"].ToString();// +"<div style='font-size:8px;'><em>(" + comments[3].ToString() + ")</em></div>";
                }

                this.head3_lbl.Text = "Sanit.1";
                if (data["S1"] != null)
                {
                    this.oddb_lbl.Text = data["S1"].ToString();// +"<div style='font-size:8px;'><em>(" + comments[4].ToString() + ")</em></div>";
                }

                this.head4_lbl.Text = "Sanit.2";
                if (data["S2"] != null)
                {
                    this.op_lbl.Text = data["S2"].ToString();// +"<div style='font-size:8px;'><em>(" + comments[5].ToString() + ")</em></div>";
                }

                this.head5_lbl.Text = "Noc:";
                if (data["N1"] != null)
                {
                    this.trp_lbl.Text = data["N1"].ToString()+"<br>";// + "<div style='font-size:8px;'><em>(" + comments[6].ToString() + ")</em></div><br>";
                }
                if (data["N2"] != null)
                {
                    this.trp_lbl.Text += data["N2"].ToString()+"<br>";// + "<div style='font-size:8px;'><em>(" + comments[7].ToString() + ")</em></div><br>";
                }
                if (data["A2"] !=null)
                {
                    this.trp_lbl.Text += data["A2"].ToString();// + "<div style='font-size:8px;'><em>(" + comments[8].ToString() + ")</em></div><br>";
                }

                string poSluzbe = table[1]["sluzba"].ToString();
              
                poSluzbe = poSluzbe.Replace('|',',');
                poSluzbe = poSluzbe.Replace(';', '-');

                if (table[1]["users_names"] != null)
                {
                    this.po_lbl.Text = "<p><small>"+poSluzbe+"</small></p>";
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

    protected void makeHeaderNkim()
    {

        DateTime dnes = DateTime.Today;
        DateTime vcera = DateTime.Today.AddDays(-1);

        string dnesStr = dnes.ToShortDateString();
        string vceraStr = vcera.ToShortDateString();

      
        this.date_lbl.Text = DateTime.Today.ToLongDateString();
        string query="";

        if (this.wgroup == "doctor")
        {

            query = @"
                        SELECT 
                                GROUP_CONCAT(CONCAT_WS(';',[t_sluz.typ],[t_user.name3],[t_sluz.comment]) SEPARATOR '|') AS [sluzba]
                                FROM [is_sluzby_all] AS [t_sluz] 
                            INNER JOIN [is_users] AS [t_user] ON [t_user.id] = [t_sluz.user_id]
                                WHERE ([t_sluz.datum]='{0}' OR  [t_sluz.datum]='{1}')
                                    AND [clinic]={2}
                                    GROUP BY [t_sluz.datum]
                                    ORDER BY [t_sluz.datum] DESC";

            


        }
        query = x2Mysql.buildSql(query, new string[] { my_x2.unixDate(vcera), my_x2.unixDate(dnes), Session["klinika_id"].ToString() });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        this.head1_lbl.Text = "JVSN: ";
        this.head2_lbl.Text = "InterMed:";
        this.head3_lbl.Text = "Transport: ";
        this.head4_lbl.Text = "-";
        this.head5_lbl.Text = "-";

        if (table.Count == 2)
        {
           
            if (this.wgroup == "doctor" || this.wgroup == "other")
            {

                this.oup_lbl.Text = "";
                this.odda_lbl.Text = "";
                this.oddb_lbl.Text = "";
                this.op_lbl.Text = "";
                this.trp_lbl.Text = "";
                string[] sluzbDnes = table[0]["sluzba"].ToString().Split('|');

                int sluzLn = sluzbDnes.Length;

                SortedList sluzba = new SortedList();
                SortedList notes = new SortedList();
                for (int s = 0; s < sluzLn; s++)
                {
                    string[] tmp = sluzbDnes[s].Split(';');
                    sluzba.Add(tmp[0], tmp[1]);
                    notes.Add(tmp[0], tmp[2]);
                }

                if (sluzba["JVSN"] != null)
                {
                    this.oup_lbl.Text = sluzba["JVSN"].ToString() + "<br>" + notes["JVSN"].ToString();
                }
                if (sluzba["InterMed"] != null)
                {
                    this.odda_lbl.Text = sluzba["InterMed"].ToString() + "<br>" + notes["InterMed"].ToString();
                }

                if (sluzba["Transport"] != null)
                {
                    this.oddb_lbl.Text = sluzba["Transport"].ToString() + "<br>" + notes["Transport"].ToString();
                }

                string[] sluzbaVcera = table[1]["sluzba"].ToString().Split('|');
                int vceraLn = sluzbaVcera.Length;

                sluzba.Clear();

                for (int v = 0; v < vceraLn; v++)
                {
                    string[] tmp = sluzbaVcera[v].Split(';');
                    sluzba.Add(tmp[0], tmp[1] + "<em> /" + tmp[2] + "/</em>");
                }
                this.po_lbl.Text = "<p class='small'>";
                foreach (DictionaryEntry row in sluzba)
                {
                    this.po_lbl.Text += row.Value.ToString() + "(" + row.Key.ToString() + "), ";
                }
                this.po_lbl.Text += "</p>";

                date_lbl.Text = DateTime.Today.ToLongDateString();

                SortedList data = x_db.getNextPozDatum(DateTime.Today);
                poziadav_lbl.Text = data["datum"].ToString();

            }

          
        }
        else
        {
            this.msg_lbl.Text = Resources.Resource.shifts_not_done;
            //int res =  x2Mysql.fillDocShifts(my_x2.makeDateGroup(rok, mesiac), dni, mesiac, rok);
            //this.makeHeader();
        }

    }

    protected void makeHeaderKdhao()
    {

        DateTime dnes = DateTime.Today;
        DateTime vcera = DateTime.Today.AddDays(-1);

        string dnesStr = dnes.ToShortDateString();
        string vceraStr = vcera.ToShortDateString();

        string query = "";
        this.date_lbl.Text = DateTime.Today.ToLongDateString();

        if (this.wgroup == "doctor")
        {

            query = @"
                        SELECT 
                                GROUP_CONCAT(CONCAT_WS(';',[t_sluz.typ],[t_user.name3],[t_sluz.comment]) SEPARATOR '|') AS [sluzba]
                                FROM [is_sluzby_all] AS [t_sluz] 
                            INNER JOIN [is_users] AS [t_user] ON [t_user.id] = [t_sluz.user_id]
                                WHERE ([t_sluz.datum]='{0}' OR  [t_sluz.datum]='{1}')
                                    AND [clinic]={2}
                                    GROUP BY [t_sluz.datum]
                                    ORDER BY [t_sluz.datum] DESC";
        }

        query = x2Mysql.buildSql(query, new string[] { my_x2.unixDate(vcera), my_x2.unixDate(dnes), Session["klinika_id"].ToString() });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);
        this.head1_lbl.Text = "OddAB: ";
        this.head2_lbl.Text = "OddABVik:";
        this.head3_lbl.Text = "Transplant: ";
        this.head4_lbl.Text = "-";
        this.head5_lbl.Text = "-";

        if (table.Count == 2)
        {
           

            if (this.wgroup == "doctor" || this.wgroup == "other")
            {
                 
                this.oup_lbl.Text = "";
                this.odda_lbl.Text = "";
                this.oddb_lbl.Text = "";
                this.op_lbl.Text = "";
                this.trp_lbl.Text = "";
                string[] sluzbDnes = table[0]["sluzba"].ToString().Split('|');

                int sluzLn = sluzbDnes.Length;

                SortedList sluzba = new SortedList();
                SortedList notes = new SortedList();
                for (int s = 0; s < sluzLn; s++)
                {
                    string[] tmp = sluzbDnes[s].Split(';');
                    sluzba.Add(tmp[0], tmp[1]);
                    notes.Add(tmp[0], tmp[2]);
                }

                if (sluzba["OddAB"] != null)
                {
                    this.oup_lbl.Text = sluzba["OddAB"].ToString() + "<br>" + notes["OddAB"].ToString();
                }
                if (sluzba["OddABVik"] != null)
                {
                    this.odda_lbl.Text = sluzba["OddABVik"].ToString() + "<br>" + notes["OddABVik"].ToString();
                }

                if (sluzba["Transplant"] != null)
                {
                    this.oddb_lbl.Text = sluzba["Transplant"].ToString() + "<br>" + notes["Transplant"].ToString();
                }

                string[] sluzbaVcera = table[1]["sluzba"].ToString().Split('|');
                int vceraLn = sluzbaVcera.Length;

                sluzba.Clear();

                for (int v = 0; v < vceraLn; v++)
                {
                    string[] tmp = sluzbaVcera[v].Split(';');
                    sluzba.Add(tmp[0], tmp[1] + "<em> /" + tmp[2] + "/</em>");
                }
                this.po_lbl.Text = "<p class='small'>";
                foreach (DictionaryEntry row in sluzba)
                {
                    this.po_lbl.Text += row.Value.ToString() + "(" + row.Key.ToString() + "), ";
                }
                this.po_lbl.Text += "</p>";

                date_lbl.Text = DateTime.Today.ToLongDateString();

                SortedList data = x_db.getNextPozDatum(DateTime.Today);
                poziadav_lbl.Text = data["datum"].ToString();
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
