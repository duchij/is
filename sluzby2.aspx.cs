using System;
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

public partial class sluzby2 : System.Web.UI.Page
{
    public mysql_db x2Mysql = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loadSluzby();
    }

    protected void changeSluzba(object sender, EventArgs e)
    {
    }

    protected void loadSluzby()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [t_sluzb].[datum], GROUP_CONCAT([typ] SEPARATOR ';') AS [type1],"); 
        sb.Append("GROUP_CONCAT([user_id] SEPARATOR ';') AS [users_ids],");
        sb.Append("GROUP_CONCAT(IF([user_id]=0,'-',[t_users].[full_name]) SEPARATOR ';') AS [users_names],");
        sb.Append("[t_sluzb].[date_group] AS [date-group]");
        sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
        sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
        sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}'","201411");
        sb.Append("GROUP BY [t_sluzb].[datum]");
        sb.Append("ORDER BY [t_sluzb].[datum]");

        Dictionary<int, SortedList> table = x2Mysql.getTable(sb.ToString());

        string[] header = table[0]["type1"].ToString().Split(';');

        int days = table.Count;
        int colsNum = header.Length;

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell headCell = new TableHeaderCell();
        headCell.ID = "headCell_date";
        headCell.Text = "Datum";
        headRow.Controls.Add(headCell);

        for (int head = 0; head < colsNum; head++)
        {
            TableHeaderCell headCell1 = new TableHeaderCell();
            headCell1.ID = "headCell_" + head;
            headCell1.Text = header[head].ToString();
            headRow.Controls.Add(headCell1);
        }
        this.shiftTable.Controls.Add(headRow);

        for (int row = 0; row < days; row++)
        {
            TableRow tblRow = new TableRow();
            string[] names = table[row]["users_names"].ToString().Split(';');

            
            TableCell cellDate = new TableCell();
            cellDate.ID = "cellDate_" + row;
            string text = (row + 1).ToString();
            cellDate.Text = text;
            tblRow.Controls.Add(cellDate);
            for (int cols = 0; cols < colsNum; cols++)
            {
                TableCell dataCell = new TableCell();
                dataCell.ID = "dataCell_" + row.ToString() + cols.ToString();
                dataCell.Text = names[cols];
                
                tblRow.Controls.Add(dataCell);
            }
            this.shiftTable.Controls.Add(tblRow);

        }


    }


}