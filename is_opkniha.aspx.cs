using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_opkniha : System.Web.UI.Page
{
    x2_var x2 = new x2_var();
    mysql_db x2Mysql = new mysql_db();
    log x2Log = new log();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        this.msg_lbl.Text = "";
        this.loadCount();
    }

    protected void loadCount()
    {
        string query = "SELECT COUNT(*) AS [rows] FROM [is_opkniha]";
        SortedList row = x2Mysql.getRow(query);

        if (row["status"]==null)
        {
            this.row_counts.Text = row["rows"].ToString();
        }


    }

    protected void searchInDgFnc(object sender, EventArgs e)
    {
        int fromYear = 0;
        int toYear = 0;

        try
        {
            fromYear = Convert.ToInt32(this.fromYear_txt.Text.ToString());
            toYear = Convert.ToInt32(this.toYear_txt.Text.ToString());
            if (this.queryDg_txt.Text.ToString().Trim().Length>0)
            {
                this.searchData(this.queryDg_txt.Text.ToString().Trim(), fromYear, toYear);
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage("Nie čo hľadať !!!!!!");
            }
            

        }
        catch (Exception ex)
        {
           this.msg_lbl.Text = x2.errorMessage("Roky nie sú čísla:  " + ex);
        }
    }

    protected void searchInOPFnc(object sender, EventArgs e)
    {
        int fromYear = 0;
        int toYear = 0;

        try
        {
            fromYear = Convert.ToInt32(this.fromYearOP_txt.Text.ToString());
            toYear = Convert.ToInt32(this.toYearOP_txt.Text.ToString());

            if (this.queryOp_txt.Text.ToString().Trim().Length >0)
            {
                this.searchDataOP(this.queryOp_txt.Text.ToString().Trim(), fromYear, toYear);
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage("Nie čo hľadať !!!!!!");
            }
           

        }
        catch (Exception ex)
        {
            this.msg_lbl.Text = x2.errorMessage("Roky nie sú čísla:  " + ex);
        }
    }

    protected void searchDataOP(string queryStr, int fromYear, int toYear)
    {

        //string queryStr = this.queryDg_txt.Text.ToString().Trim();
        string finalLike = this.parseQuery(queryStr);

        

        string query = x2.sprintf("SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] FROM [is_opkniha] WHERE ([vykon] LIKE {0} AND [datum] BETWEEN '{1}-01-01 00:00:01' AND '{2}-12-31 23:59:59' ORDER BY [datum] ", new String[] { finalLike, fromYear.ToString(), toYear.ToString() });
        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);


        int resLn = table.Count;
        this.foundRows_lbl.Text = "Nájdených:" + resLn.ToString();

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dateHeadCell = new TableHeaderCell();
        dateHeadCell.Text = Resources.Resource.date;

        dateHeadCell.Font.Bold = true;
        dateHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dateHeadCell);

        TableHeaderCell nameHeadCell = new TableHeaderCell();
        nameHeadCell.Text = "Meno";
        nameHeadCell.Font.Bold = true;
        nameHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(nameHeadCell);

        TableHeaderCell rcHeadCell = new TableHeaderCell();
        rcHeadCell.Text = Resources.Resource.birth_num;
        rcHeadCell.Font.Bold = true;
        rcHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(rcHeadCell);

        TableHeaderCell dgHeadCell = new TableHeaderCell();
        dgHeadCell.Text = Resources.Resource.diagnose;
        dgHeadCell.Font.Bold = true;
        dgHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dgHeadCell);

        TableHeaderCell opHeadCell = new TableHeaderCell();
        opHeadCell.Text = "Výkon";
        opHeadCell.Font.Bold = true;
        opHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(opHeadCell);

        TableHeaderCell teamHeadCell = new TableHeaderCell();
        teamHeadCell.Text = "Team";
        teamHeadCell.Font.Bold = true;
        teamHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(teamHeadCell);

        this.result_tbl.Controls.Add(headRow);



        for (int row = 0; row < resLn; row++)
        {
            TableRow resRow = new TableRow();

            TableCell dateCell = new TableCell();
            dateCell.Text = table[row]["datum"].ToString();
            resRow.Controls.Add(dateCell);

            TableCell nameCell = new TableCell();
            nameCell.Text = table[row]["priezvisko"].ToString();
            resRow.Controls.Add(nameCell);

            TableCell rcCell = new TableCell();
            rcCell.Text = table[row]["rodne_cislo"].ToString();
            resRow.Controls.Add(rcCell);

            TableCell dgCell = new TableCell();
            dgCell.Text = table[row]["diagnoza"].ToString();
            resRow.Controls.Add(dgCell);

            TableCell opCell = new TableCell();
            opCell.Text = table[row]["vykon"].ToString();
            resRow.Controls.Add(opCell);

            TableCell teamCell = new TableCell();
            teamCell.Text = table[row]["operater"].ToString();
            resRow.Controls.Add(teamCell);

            this.result_tbl.Controls.Add(resRow);

        }

    }

    protected void searchData(string queryStr, int fromYear, int toYear)
    {

        //string queryStr = this.queryDg_txt.Text.ToString().Trim();
        string finalLike = this.parseQuery(this.queryDg_txt.Text);

        string query = x2.sprintf("SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] FROM [is_opkniha] WHERE ([diagnoza] LIKE {0} AND [datum] BETWEEN '{1}-01-01 00:00:01' AND '{2}-12-31 23:59:59' ORDER BY [datum] ", new String[] { finalLike,fromYear.ToString(),toYear.ToString() });
        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);


        int resLn = table.Count;
        this.foundRows_lbl.Text = "Nájdených:" + resLn.ToString() + " záznamov."; ;

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dateHeadCell = new TableHeaderCell();
        dateHeadCell.Text = "Datum";
        dateHeadCell.Font.Bold = true;
        dateHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dateHeadCell);

        TableHeaderCell nameHeadCell = new TableHeaderCell();
        nameHeadCell.Text = "Meno";
        nameHeadCell.Font.Bold = true;
        nameHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(nameHeadCell);

        TableHeaderCell rcHeadCell = new TableHeaderCell();
        rcHeadCell.Text = "Rodne cislo";
        rcHeadCell.Font.Bold = true;
        rcHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(rcHeadCell);

        TableHeaderCell dgHeadCell = new TableHeaderCell();
        dgHeadCell.Text = "Diagnoza";
        dgHeadCell.Font.Bold = true;
        dgHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dgHeadCell);

        TableHeaderCell opHeadCell = new TableHeaderCell();
        opHeadCell.Text = "Vykon";
        opHeadCell.Font.Bold = true;
        opHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(opHeadCell);

        TableHeaderCell teamHeadCell = new TableHeaderCell();
        teamHeadCell.Text = "Team";
        teamHeadCell.Font.Bold = true;
        teamHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(teamHeadCell);

        this.result_tbl.Controls.Add(headRow);



        for (int row = 0; row < resLn; row++)
        {
            TableRow resRow = new TableRow();

            TableCell dateCell = new TableCell();
            dateCell.Text = table[row]["datum"].ToString();
            resRow.Controls.Add(dateCell);

            TableCell nameCell = new TableCell();
            nameCell.Text = table[row]["priezvisko"].ToString();
            resRow.Controls.Add(nameCell);

            TableCell rcCell = new TableCell();
            rcCell.Text = table[row]["rodne_cislo"].ToString();
            resRow.Controls.Add(rcCell);

            TableCell dgCell = new TableCell();
            dgCell.Text = table[row]["diagnoza"].ToString();
            resRow.Controls.Add(dgCell);

            TableCell opCell = new TableCell();
            opCell.Text = table[row]["vykon"].ToString();
            resRow.Controls.Add(opCell);

            TableCell teamCell = new TableCell();
            teamCell.Text = table[row]["operater"].ToString();
            resRow.Controls.Add(teamCell);

            this.result_tbl.Controls.Add(resRow);

        }

    }

    private string parseQuery(string query)
    {
        string queryStr = query.ToString().Trim();
        string[] queryArr = queryStr.Split(' ');

        int strLn = queryArr.Length;

        string finalLike = "";

        if (strLn > 0)
        {
            string[] arrTmp = new String[strLn];

            for (int i = 0; i < strLn; i++)
            {
                arrTmp[i] = "'%" + queryArr[i] + "%'";
            }

            finalLike = string.Join(" OR ", arrTmp);
            finalLike = finalLike + ")";
        }
        else
        {
            finalLike = "'%" + queryStr + "%')";
        }

        return finalLike;
    }

    protected void searchToExcelFnc(object sender, EventArgs e)
    {

        string finalLike = this.parseQuery(this.queryDg_txt.Text);
        //x2Log.logData(finalLike, "", "final query");

        string query = x2.sprintf("SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] FROM [is_opkniha] WHERE ([diagnoza] LIKE {0} ORDER BY [datum] ", new String[] { finalLike });

        Session["toExcelQuery"] = query;

        Response.Redirect("toexcel.aspx?a=opres");
    }

    protected void searchOPToExcelFnc(object sender, EventArgs e)
    {

        string finalLike = this.parseQuery(this.queryDg_txt.Text);
        //x2Log.logData(finalLike, "", "final query");

        string query = x2.sprintf("SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] FROM [is_opkniha] WHERE ([vykon] LIKE {0} ORDER BY [datum] ", new String[] { finalLike });

        Session["toExcelQuery"] = query;

        Response.Redirect("toexcel.aspx?a=opres");
    }
}