using System;
using System.IO;
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

public partial class toexcel : System.Web.UI.Page
{
    mysql_db x2Mysql = new mysql_db();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        if (Request.QueryString["a"] != null)
        {
            string action = Request.QueryString["a"].ToString().Trim();

            switch (action){

                case "opres":
                    this.opToExcel();
                    break;

            }
        }
        else
        {
            this.msg_lbl.Text = "Žiadna valídna akcia.....";
        }

    }

    protected void opToExcel()
    {
        if (Session["toExcelQuery"] != null)
        {
            string query = Session["toExcelQuery"].ToString();
            Session.Remove("toExcelQuery");
            Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

            int resLn = table.Count;

            if (resLn > 0)
            {
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


                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/msexcel";
                Response.AddHeader("content-disposition", "attachment;filename=result.xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                Response.Charset = "windows-1250";
                StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

                this.RenderControl(htmlTextWriter);
                Response.Write(stringWriter.ToString());
                Response.End();
            }
            else
            {

                this.msg_lbl.Text = "Nič na zobrazenie......";
            }
            


        }
        else
        {
            Response.Redirect("error.html");
        }
    }
}