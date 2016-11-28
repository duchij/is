using System;
using System.IO;
using System.Text;
using System.Globalization;
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


class planClass
{
    private string _rights = "";
    private int _gKlinika = 0;
    private mysql_db _mysql;
    private x2_var _x2;
    private log _log;
    private string _clinicIdf;

    private string _department;

    private string _userId;

    private Boolean _editable;
    private string[] _vykazHeader;

    private SortedList _userData;

    private string[] _workTypes;

    public string[] workTypes
    {
        get { return _workTypes; }
        set { _workTypes = value; }
    }

    public SortedList userData
    {
        get { return _userData; }
        set { _userData = value; }
    }

    public string userId
    {
        get { return _userId; }
        set { _userId = value; }
    }

    public string department
    {
        get { return _department; }
        set { _department = value; }
    }

    public Boolean editable
    {
        get
        {
            if (_rights.IndexOf("admin") != -1 || _rights == "poweruser")
            {
                return true;
            }
            else { return false; }

        }
    }

    public string[] vykazHeader
    {
        get { return _vykazHeader; }
        set { _vykazHeader = value; }
    }


    public string clinicIdf
    {
        get { return _clinicIdf; }
        set { _clinicIdf = value; }
    }

    public string rights
    {
        get { return _rights; }
        set { _rights = value; }
    }

    public mysql_db mysql
    {
        get { return _mysql; }
        set { _mysql = value; }
    }



    public int gKlinika
    {
        get { return _gKlinika; }
        set { _gKlinika = value; }
    }

    public x2_var x2
    {
        get { return _x2; }
        set { _x2 = value; }


    }

    public log x2log
    {
        get { return _log; }
        set { _log = value; }
    }



}

public partial class is_poziad_sestr : System.Web.UI.Page
{

    planClass poziad = new planClass();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }


        poziad.gKlinika = Convert.ToInt32(Session["klinika_id"]);
        poziad.rights = Session["rights"].ToString();
        poziad.clinicIdf = Session["klinika"].ToString();
        poziad.mysql = new mysql_db();
        poziad.x2 = new x2_var();
        poziad.x2log = new log();
        poziad.userData = new SortedList();

        poziad.department = Session["oddelenie"].ToString();


        if (!IsPostBack)
        {
            poziad.x2.fillYearMonth(ref this.month_dl, ref this.years_dl, Session["month_dl"].ToString(), Session["years_dl"].ToString());
            DateTime dnesJe = DateTime.Today;
            this.month_dl.SelectedValue = dnesJe.Month.ToString();
            this.years_dl.SelectedValue = dnesJe.Year.ToString();
        }

        

        this.drawTable();
    }


    protected void drawTable()
    {
        this.poziadTable_tbl.Controls.Clear();

        int my_den = 0;
        int pocetDni = DateTime.DaysInMonth(2016, 11);
        for (int i = 0; i < 5; i++)
        {

            TableRow mojRiadok = new TableRow();
            for (int x = 0; x < 7; x++)
            {

                TableCell mojaCela = new TableCell();
                mojaCela.ID = "mojaCelb_" + i.ToString() + "_" + x.ToString();
                mojaCela.VerticalAlign = VerticalAlign.Top;

                // mojaCela.CssClass = "duch";
                // mojaCela.Width = 100;
                my_den++;

                //DateTime tmp_den_mes = new DateTime(tc_rok, tc_month, o + 1);
                if (DateTime.Today.Day == my_den)
                {
                    mojaCela.CssClass = "box yellow";
                    //mojaCela.BackColor = System.Drawing.Color.Yellow;
                    // mojaCela.ForeColor = System.Drawing.Color.FromArgb(0x990000);
                }


                mojaCela.BorderWidth = 1;
                mojaCela.BorderColor = System.Drawing.Color.LightGray;


                if (my_den <= pocetDni)
                {
                    DateTime my_date = new DateTime(2016, 11, my_den);
                    int dnesJe = (int)my_date.DayOfWeek;
                    string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];

                    if ((nazov == "sobota") || (nazov == "nedeľa"))
                    {
                       
                        mojaCela.CssClass = "box red";
                    }

                    if (DateTime.Today.Day == my_den)
                    {
                        
                        mojaCela.CssClass = "box yellow";
                    }
                    Literal dateLabel = new Literal();
                    dateLabel.Text = "<strong>" + my_den.ToString() + ".</strong><font> " + nazov.Substring(0, 3) + "</font><br><br/>";
                    mojaCela.Controls.Add(dateLabel);


                    DropDownList dl = new DropDownList();
                    this.fillDl(dl);
                    dl.ID = "statusDl_" + Session["user_id"].ToString() + "_" + my_date.Day;
                    dl.Attributes.Add("onChange", "savePoziadOfNurse('" + dl.ID.ToString() + "');");
                    
                    mojaCela.Controls.Add(dl);
                    //mojaCela.Text += "<font style='font-size:10px;'>"+mes_dov[my_den-1].ToString()+"</font>";
                   // mojaCela.Text += mes_dov[my_den - 1].ToString();
                }
                mojRiadok.Controls.Add(mojaCela);
            }
            this.poziadTable_tbl.Controls.Add(mojRiadok);
        }

        this.loadPoziad();
    } 


    protected void loadPoziad()
    {
        int month = Convert.ToInt32(this.month_dl.SelectedValue);
        int year = Convert.ToInt32(this.years_dl.SelectedValue);

        int days = DateTime.DaysInMonth(year, month);

        string sql = @"
                        SELECT [datum],[user_id],[status] 
                            FROM [is_poziad_sestr] 
                        WHERE [user_id]={0}
                            AND [datum] BETWEEN '{1}-{2}-01 00:00:00' AND '{1}-{2}-{3} 23:59:59'
                    ";

        sql = poziad.x2.sprintf(sql, new string[] { Session["user_id"].ToString(),year.ToString(),month.ToString(),days.ToString() });

        Dictionary<int, SortedList> table = poziad.mysql.getTableSL(sql);

        int tblLn = table.Count;

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        for (int i=0; i< tblLn; i++)
        {
            DateTime dt = poziad.x2.UnixToMsDateTime(table[i]["datum"].ToString());

            string dlStatus = "statusDl_{0}_{1}";
            dlStatus = poziad.x2.sprintf(dlStatus, new string[] { Session["user_id"].ToString(), dt.Day.ToString() });

            Control dlCtl = ctpl.FindControl(dlStatus);
            DropDownList dl = (DropDownList)dlCtl;
           
            dl.SelectedValue = table[i]["status"].ToString();
            dl.ToolTip = dl.SelectedItem.ToString();

        }
        
         
    }

    protected void redrawTable(object sender, EventArgs e)
    {
        this.drawTable();
    }

    protected void fillDl (DropDownList dl)
    {
        dl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.plan_sestr_yes, "yes"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.plan_sestr_no, "no"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.plan_sestr_do, "do"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.plan_sestr_yes_d, "yes_d"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.plan_sestr_yes_n, "yes_n"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.plan_sestr_no_d, "no_d"));
        dl.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.plan_sestr_no_n, "no_n"));
    }

}