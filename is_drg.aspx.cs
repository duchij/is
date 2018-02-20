using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

class DrgObj
{
    private string _rights = "";
    private int _gKlinika = 0;
    private mysql_db _mysql;
    private x2_var _x2;
    private log _log;
    private string _clinicIdf;

    //private Boolean _editable;
    private string[] _kruzky;

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

    public string[] kruzky
    {
        get { return _kruzky; }
        set { _kruzky = value; }
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

public partial class is_drg : System.Web.UI.Page
{
    private DrgObj Drg = new DrgObj();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        Page.ClientScript.RegisterClientScriptInclude("selective", ResolveUrl(@"js\drg.js"));

        /*if (!Master.Page.ClientScript.IsStartupScriptRegistered("alert"))
        {
            Master.Page.ClientScript.RegisterStartupScript
                (this.GetType(), "alert", "insideJS();", true);
        }*/

        Drg.gKlinika = Convert.ToInt32(Session["klinika_id"]);
        Drg.rights = Session["rights"].ToString();
        Drg.clinicIdf = Session["klinika"].ToString();
        Drg.mysql = new mysql_db();
        Drg.x2 = new x2_var();
        Drg.x2log = new log();


        if (!IsPostBack)
        {
            this.initData();
        }


    }

    protected void initData()
    {

        this.loadDrgGroups();
    }

    protected void loadDrgGroups()
    {

        string sql = @"SELECT [drg_group_label],  [item_id] FROM [is_drg_groups]";

        sql = Drg.mysql.buildSql(sql, new string[] { });

        Dictionary<int, Hashtable> resTable = Drg.mysql.getTable(sql);

        int tblLn = resTable.Count();

        this.drg_group_dl.Items.Clear();
        this.drg_group_search_dl.Items.Clear();

        if (tblLn > 0)
        {

            this.drg_group_search_dl.Items.Add(new ListItem("-", "0"));

            for (var i=0; i< tblLn; i++)
            {

                this.drg_group_dl.Items.Add(new ListItem(resTable[i]["drg_group_label"].ToString(), resTable[i]["item_id"].ToString()));
                this.drg_group_search_dl.Items.Add(new ListItem(resTable[i]["drg_group_label"].ToString(), resTable[i]["item_id"].ToString()));

            }

        }

    }

    



    protected void saveDataFnc(object sender, EventArgs e)
    {
        SortedList saveData = new SortedList();
        try
        {
            saveData.Add("drg_group", Convert.ToInt32(this.drg_group_dl.SelectedValue.ToString()));
            saveData.Add("main_dg", this.main_dg_txt.Text.ToString().ToUpper().Replace(".",""));
            saveData.Add("second_dg", this.second_dg_txt.Text.ToString().ToUpper().Replace(".", ""));
            saveData.Add("drg_code", this.drg_code_txt.Text.ToString());
            saveData.Add("note", this.note_txt.Text.ToString());
            saveData.Add("other_dg", this.other_dg_txt.Text.ToString().ToUpper().Replace(".", ""));
            saveData.Add("user_id", Session["user_id"].ToString());

            SortedList res = Drg.mysql.mysql_insert("is_drg", saveData);

            if (!(Boolean)res["status"])
            {
                if (res["msg"].ToString().IndexOf("cannot be null") != -1)
                {
                    Drg.x2.errorMessage2(ref this.msg_lbl,"Nezadali ste povinnu hodnotu");
                }
                else
                {
                    Drg.x2.errorMessage2(ref this.msg_lbl, res["msg"].ToString() + "<br>" + res["sql"].ToString());
                }


               
            }else
            {

                Drg.x2.succesMessage(ref this.msg_lbl, "Data zapisane OK...");
                //Drg.x2.errorMessage2(ref this.msg_lbl, "Data zapisane...");
                //Drg.x2.warningMessage()

            }
            

        }
        catch (Exception ex)
        {
            Drg.x2.errorMessage2(ref this.msg_lbl, " Chyba:" + ex.ToString());

        }
       
        


    }

    protected void searchDataFnc(object sender, EventArgs e)
    {
        try
        {
            string where = this.drg_group_search_dl.SelectedValue.ToString();
            string dg = this.dg_search_txt.Text.ToString().Trim().ToUpper().Replace(".", "");
            string sql = @"";

            if (where != "0" && dg.Length == 0)
            {
                sql = @"SELECT [main_dg], [second_dg], [drg_code], [note], [other_dg] FROM [is_drg] WHERE [drg_group]={0}";
                sql = Drg.mysql.buildSql(sql, new string[] { where});
            }

            if (where != "0" && dg.Length > 0)
            {
                sql = @"SELECT 
                                [main_dg], [second_dg], [drg_code], [note], [other_dg] 
                            FROM [is_drg] WHERE [drg_group]={0}
                            AND [main_dg] LIKE '{1}%';

                        ";

                sql = Drg.mysql.buildSql(sql, new string[] { where,dg });


            }

            Dictionary<int, Hashtable> table = Drg.mysql.getTable(sql);

            int tblLn = table.Count;

            TableHeaderRow headRow = new TableHeaderRow();
            TableHeaderCell mainDg = new TableHeaderCell();
            mainDg.Text = "Hlavna dg";
            headRow.Controls.Add(mainDg);

            TableHeaderCell secDg = new TableHeaderCell();
            secDg.Text = "Vedlajsia dg";
            headRow.Controls.Add(secDg);

            TableHeaderCell vykDg = new TableHeaderCell();
            vykDg.Text = "Kod vykonu";
            headRow.Controls.Add(vykDg);

            TableHeaderCell note = new TableHeaderCell();
            note.Text = "Poznamka";
            headRow.Controls.Add(note);

            TableHeaderCell otherDg = new TableHeaderCell();
            otherDg.Text = "Ostatne Dg";
            headRow.Controls.Add(otherDg);

            this.result_table_tbl.Controls.Add(headRow);

            for (var i=0; i<tblLn; i++)
            {
                TableRow riadok = new TableRow();

                TableCell mainDgCell = new TableCell();
                //Label mainDgLbl = new Label();
                mainDgCell.Text = table[i]["main_dg"].ToString();

                riadok.Controls.Add(mainDgCell);

                TableCell secondDgCell = new TableCell();
                //Label mainDgLbl = new Label();
                secondDgCell.Text = table[i]["second_dg"].ToString();

                riadok.Controls.Add(secondDgCell);

                TableCell drgVykCell = new TableCell();
                //Label mainDgLbl = new Label();
                drgVykCell.Text = table[i]["drg_code"].ToString();

                riadok.Controls.Add(drgVykCell);

                TableCell noteCell = new TableCell();
                //Label mainDgLbl = new Label();
                noteCell.Text = table[i]["note"].ToString();

                riadok.Controls.Add(noteCell);

                TableCell otherDgCell = new TableCell();
                //Label mainDgLbl = new Label();
                otherDgCell.Text = table[i]["other_dg"].ToString();

                riadok.Controls.Add(otherDgCell);

                this.result_table_tbl.Controls.Add(riadok);

            }





        }
        catch (Exception ex)
        {
            Drg.x2.errorMessage2(ref this.msg_lbl, ex.ToString());
        }

    }


}