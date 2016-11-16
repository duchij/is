using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_plan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.drawTable();

    }


    protected void drawTable()
    {
        int days = DateTime.DaysInMonth(2016,11);

        TableRow riadok = new TableRow();
        for (int d=1; d <= days; d++)
        {
            TableCell dayCell = new TableCell();
            dayCell.ID = "dayCell_1_" + d;
            DropDownList dl = new DropDownList();
            dayCell.Controls.Add(dl);
            riadok.Controls.Add(dayCell);
        }
        this.planTable_tbl.Controls.Add(riadok);

    }
}