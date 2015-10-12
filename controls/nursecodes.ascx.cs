using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_nursecodes : System.Web.UI.UserControl
{

    mysql_db x2Mysql = new mysql_db();
    x2_var x2 = new x2_var();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("../error.html");
        }
        this.loadGroups();
    }


    protected void loadGroups()
    {
        string query = @"
                            SELECT [item_label],[item_letter],[item_id] FROM [is_cn_nurse_codes]
                            WHERE [item_type]='group'
                        ";
        this.groups_dl.Items.Clear();
        query = x2Mysql.buildSql(query, new string[] { });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        int tblLn = table.Count;
        //ListItem[] newItem = new ListItem[tblLn];
        // this.deps_dl.Items.Clear();
        this.groups_dl.Items.Add(new ListItem("-", "0"));
        for (int i = 0; i < tblLn; i++)
        {
            this.groups_dl.Items.Add(new ListItem(table[i]["item_letter"].ToString().Trim()+" "+table[i]["item_label"].ToString(), table[i]["item_letter"].ToString().Trim() + "-" + table[i]["item_id"].ToString().Trim()));
        }

    }


    protected void loadItemsFnc(object sender,EventArgs e)
    {
        string id = this.groups_dl.SelectedValue.ToString();
        if (id != "0")
        {
            string grp = this.groups_dl.SelectedItem.ToString();

            string[] _tmp = grp.Split(',');

            string[] letter = id.Split('-');

            string[] grps = _tmp[0].Split('-');

            string query = @"
                            SELECT [item_letter],[item_number],[item_label] FROM [is_cn_nurse_codes]
                                WHERE [item_type]='item' 
                                        AND [item_number]>='{0}' AND [item_number]<='{1}'
                                        AND [item_letter]='{2}'
                                ORDER BY [item_letter]
                        ";

            query = x2Mysql.buildSql(query, new string[] { grps[0], grps[1], letter[0]});

            Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

            int tblLn = table.Count;

            for (int i = 0; i < tblLn; i++)
            {
                TableRow row = new TableRow();

                TableCell dataCell = new TableCell();
                dataCell.ForeColor = System.Drawing.Color.Black;
                dataCell.Text = table[i]["item_letter"].ToString() + table[i]["item_number"].ToString() + " " + table[i]["item_label"].ToString();
                row.Controls.Add(dataCell);

                code_list_tbl.Controls.Add(row);
            }
        }
        




    }
    

}