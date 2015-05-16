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

public partial class is_user_vyk : System.Web.UI.Page
{

    user x_db = new user();
    x2_var my_x2 = new x2_var();
    mysql_db x2MySql = new mysql_db();
    log x2log = new log();

    //public string[] vykazHeader;
    public string rights;
    public string gKlinika;
    string[] vykazHeader;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        this.gKlinika = Session["klinika"].ToString().ToLower();
        this.rights = Session["rights"].ToString();

        if (this.gKlinika == "kdch")
        {
            this.generateVykazSettings();
        }

        if (this.gKlinika == "2dk")
        {
            //this.generateVykazSettingsDK();
        }
    }

    protected string[] getVykazTyp()
    {
        SortedList row = x2MySql.getRow("SELECT * FROM [is_settings] WHERE [name]='typ_vykaz'");
        return row["data"].ToString().Split(',');

    }
    protected string[] getDefaultHours()
    {
        SortedList row = x2MySql.getRow("SELECT * FROM [is_settings] WHERE [name]='hodiny_vykaz'");
        return row["data"].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    }

    protected string getUserHours()
    {
        string result = "";
        string tmp = Session["login"].ToString() + "_vykaz";

        SortedList row = x2MySql.getRow("SELECT * FROM [is_settings] WHERE [name]='" + tmp + "'");
        if (row.Count > 0)
        {
            result = row["data"].ToString();
        }
        return result;
    }

    protected void generateVykazSettings()
    {
        this.vykazSetup_tbl.Controls.Clear();
        string[] typVykaz = this.getVykazTyp();


        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT * FROM [is_settings]  WHERE [name]='vykaz_doctors'");

        SortedList row = x2MySql.getRow(sb.ToString());
        this.vykazHeader = row["data"].ToString().Split(',');

        string tmpHours = this.getUserHours();
        string[] hoursArr = tmpHours.Split('|');

        string[] defaultHodiny = new string[this.vykazHeader.Length];

        if (hoursArr[0].Split(',').Length == this.vykazHeader.Length)
        {
            defaultHodiny = hoursArr;
        }
        else
        {
            defaultHodiny = this.getDefaultHours();
        }

        int cols = this.vykazHeader.Length;
        TableHeaderRow headerRow = new TableHeaderRow();

        TableHeaderCell headCellDate = new TableHeaderCell();
        headCellDate.ID = "sluzba_cell";
        headCellDate.Text = "Sluzba";
        headerRow.Controls.Add(headCellDate);

        this.vykazSetup_tbl.Controls.Add(headerRow);

        for (int col = 0; col < cols; col++)
        {
            TableHeaderCell headCell = new TableHeaderCell();
            headCell.ID = "headCell_" + col.ToString();

            Label headLabel = new Label();
            headLabel.ID = this.vykazHeader[col] + "_lbl<br>";
            headLabel.Font.Size = FontUnit.Point(8);
            headLabel.Text = this.vykazHeader[col];

            headCell.Controls.Add(headLabel);

            /* TextBox tBox = new TextBox();
             tBox.ID = "head_tbox_" + col.ToString();
             tBox.Text = "";
             headCell.Controls.Add(tBox);*/


            headerRow.Controls.Add(headCell);
        }

        int typVykazLn = typVykaz.Length;

        for (int riadok = 0; riadok < typVykazLn; riadok++)
        {
            TableRow riadTyp = new TableRow();
            TableCell typCell = new TableCell();
            typCell.ID = typVykaz[riadok].ToString();
            typCell.Text = typVykaz[riadok].ToString();

            string[] hodiny = defaultHodiny[riadok].Split(',');

            switch (typVykaz[riadok].ToString())
            {
                case "normDen":
                    typCell.ToolTip = "Nastavenia normalneho dna";
                    break;
                case "malaSluzba":
                    typCell.ToolTip = "Nastavenia hodin pri malej sluzbe";
                    break;
                case "malaSluzba2":
                    typCell.ToolTip = "Nastavenia hodin po malej sluzbe";
                    break;
                case "velkaSluzba":
                    typCell.ToolTip = "Nastavenia hodin pri velkej sluzbe";
                    break;
                case "velkaSluzba2":
                    typCell.ToolTip = "Nastavenia hodin den po velkej sluzbe";
                    break;
                case "velkaSluzba2a":
                    typCell.ToolTip = "Nastavenia hodin druhy nasledujuci den po velkej sluzbe (sobota, nedela a pondelok, utorok)";
                    break;
                case "sviatokVikend":
                    typCell.ToolTip = "Nastavenia hodin v sluzbe ak pripadne na vikend aj sviatocny den";
                    break;
                case "sviatok":
                    typCell.ToolTip = "Nastavenie hodin ak je sviatok iny den ako sobota, nedela";
                    break;
                case "exDay":
                    typCell.ToolTip = "Nastavenie hodin pocas nasledujuceho volneho dna";
                    break;
                case "sviatokNieVikend":
                    typCell.ToolTip = "Nastavenie hodin v sviatok, ale nie pocas sluzby";
                    break;

            }
            riadTyp.Controls.Add(typCell);
            for (int col = 0; col < cols; col++)
            {
                TableCell dataCela = new TableCell();
                riadTyp.Controls.Add(dataCela);
                TextBox tBox = new TextBox();
                tBox.ID = "textBox_" + riadok.ToString() + "_" + col.ToString();
                if (!IsPostBack)
                {
                    tBox.Text = hodiny[col].ToString();
                }


                dataCela.Controls.Add(tBox);
            }
            this.vykazSetup_tbl.Controls.Add(riadTyp);
        }
    }

    protected void saveVykaz_fnc(object sender, EventArgs e)
    {
        ContentPlaceHolder ctpl = new ContentPlaceHolder();
        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ctpl = (ContentPlaceHolder)tmpControl;

        // string[] typVykaz = this.getVykazTyp();

        string[] defaultHodiny = this.getDefaultHours();

        int typVykazLn = defaultHodiny.Length;
        int defaultHodLn = this.vykazHeader.Length;
        string[] riadArr = new string[typVykazLn];
        string[] stlpArr = new string[defaultHodLn];
        for (int riadok = 0; riadok < typVykazLn; riadok++)
        {
            for (int col = 0; col < defaultHodLn; col++)
            {
                Control Tbox = ctpl.FindControl("textBox_" + riadok.ToString() + "_" + col.ToString());
                TextBox tBox = (TextBox)Tbox;
                //TextBox tBox = new TextBox();
                string hodnota = tBox.Text.ToString();
                hodnota = hodnota.Replace(',', '.');
                //tBox.ID = "textBox_" + riadok.ToString() + "_" + col.ToString();
                stlpArr[col] = hodnota;

            }
            string tmp = String.Join(",", stlpArr);

            riadArr[riadok] = tmp;

        }
        string defStr = String.Join("|", riadArr);

        SortedList saveData = new SortedList();
        saveData.Add("name", Session["login"].ToString() + "_vykaz");
        saveData.Add("data", defStr);

        SortedList result = x2MySql.mysql_insert("is_settings", saveData);

        if (!Convert.ToBoolean(result["status"]))
        {
            this.msg_lbl.Visible = true;
            this.msg_lbl.Text = result["msg"].ToString();
        }
        else
        {
            this.msg_lbl.Visible = true;
            this.msg_lbl.Text = "Nastavenia vykazu ulozene";
        }



    }

    protected void resetVykaz_fnc(object sender, EventArgs e)
    {
        string query = my_x2.sprintf("DELETE FROM [is_settings] WHERE [name]='{0}_vykaz'",new string[] {Session["login"].ToString()});
        SortedList res = x2MySql.execute(query);

        if (!Convert.ToBoolean(res["status"]))
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }
        else
        {
            Response.Redirect("is_user_vyk.aspx");
        }

    }

}