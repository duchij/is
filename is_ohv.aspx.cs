using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_ohv : System.Web.UI.Page
{
    mysql_db x2Mysql = new mysql_db();
    x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.init();
            this.loadCodes(false);
        }
        else
        {
           // this.loadCodes(false);
        }
    }

    protected void loadCodecsFnc(object sender, EventArgs e)
    {
        this.loadCodes(false);
    }

    protected void init()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [odbkod],[text] FROM [medix_odbornost]");

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableLn = table.Count;
        // ListItem[] newItem = new ListItem[tableLn + 1];
        // newItem[0] = new ListItem("", "0");
        for (int i = 0; i < tableLn; i++)
        {
            this.clgroup_dl.Items.Add(new ListItem(table[i]["text"].ToString(), table[i]["odbkod"].ToString()));
        }

        this.clgroup_dl.SelectedValue = "107";
    }

    protected void loadCodes(Boolean all)
    {
        string insurance = this.insurance_dl.SelectedValue.ToString();
        string clgroup = this.clgroup_dl.SelectedValue.ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT [code],[label],[comment] FROM [is_ohv]");

        if (all == false)
        {
            if (insurance == "24")
            {
                sb.AppendFormat(" WHERE [insurance] = '{0}'", insurance);
            }
            else
            {
                sb.AppendFormat(" WHERE [cl_group] LIKE '%{0}%' AND [insurance] = '{1}'", clgroup, insurance);
            }
        }
        else
        {
            sb.AppendFormat(" WHERE [insurance] = '{0}'", insurance);
        }

        sb.AppendLine(" ORDER BY [code]");

        Dictionary<int, Hashtable> data = x2Mysql.getTable(sb.ToString());

        int dataCn = data.Count;
        this.ohvCode_tbl.Controls.Clear();
        for (int i = 0; i < dataCn; i++)
        {
            TableRow riadok = new TableRow();
            this.ohvCode_tbl.Controls.Add(riadok);

            TableCell cellCode = new TableCell();
            cellCode.Text = data[i]["code"].ToString();
            riadok.Controls.Add(cellCode);


            TableCell cellLabel = new TableCell();
            cellLabel.Text = data[i]["label"].ToString();
            riadok.Controls.Add(cellLabel);

            TableCell cellComment = new TableCell();
            cellComment.Text = x2.getStr(data[i]["comment"].ToString());
            riadok.Controls.Add(cellComment);
        }
    }

    protected void loadAllCodesFnc(object sender, EventArgs e)
    {
        this.loadCodes(true);
    }

}