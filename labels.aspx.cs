using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class labels_labels : System.Web.UI.Page
{
    mysql_db x2Mysql = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void searchPrefixFnc(object sender, EventArgs e)
    {
        string prefix = this.prefix_txt.Text.ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_labels] WHERE [idf] LIKE '{0}_%'", prefix);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableCn = table.Count;

        if (tableCn > 0)
        {
            for (int i = 0; i < tableCn; i++)
            {
                TableRow riadok = new TableRow();
                this.listTable_tbl.Controls.Add(riadok);

                TableCell cellIdf = new TableCell();
                //cellIdf.ID = "cellIdf_" + table[i]["id"].ToString();

                Label labelIdf = new Label();
                labelIdf.ID = "labelIdf_" + table[i]["id"].ToString();
                labelIdf.Text = table[i]["idf"].ToString();
                cellIdf.Controls.Add(labelIdf);
                riadok.Controls.Add(cellIdf);

                TableCell cellLabel = new TableCell();
                TextBox textLabel = new TextBox();
                textLabel.ID = "tboxLabel_" + table[i]["id"].ToString();
                textLabel.Width = Unit.Pixel(400);
                textLabel.Text = table[i]["label"].ToString();
                cellLabel.Controls.Add(textLabel);
                riadok.Controls.Add(cellLabel);

                TableCell actionCell = new TableCell();
                Button saveBtn = new Button();
                saveBtn.CssClass = "button green";
                saveBtn.ID = "saveLabel_" + table[i]["id"].ToString();
                saveBtn.Text = Resources.Resource.save;
                actionCell.Controls.Add(saveBtn);

                
                Button deleteBtn = new Button();
                deleteBtn.ID = "deleteLabel_" + table[i]["id"].ToString();
                deleteBtn.CssClass = "button red";
                deleteBtn.Text = Resources.Resource.delete;
                actionCell.Controls.Add(deleteBtn);

                riadok.Controls.Add(actionCell);


            }
        }
        else
        {
            /*TableRow riadok = new TableRow();
            this.listTable_tbl.Controls.Add(riadok);

            TableCell cellIdf = new TableCell();
            TextBox textIdf = new TextBox();
            textIdf.ID = "textIdfTmp_0";
            cellIdf.Controls.Add(textIdf);

            riadok.Controls.Add(cellIdf);


            TableCell cellLabel = new TableCell();
            TextBox textLabel = new TextBox();
            textLabel.ID = "textLabelTmp_0";
            cellLabel.Controls.Add(textLabel);
            riadok.Controls.Add(cellLabel);

            TableCell cellButtons = new TableCell();
            Button addNew = new Button();
            addNew.Click += new EventHandler(newRow_fnc);
            addNew.Text = "Pridaj";
            cellButtons.Controls.Add(addNew);

            riadok.Controls.Add(cellButtons);*/
        }
    }

    protected void newRow_fnc(object sender, EventArgs e)
    {
        TableRow riadok = new TableRow();
        this.listTable_tbl.Controls.Add(riadok);

        TableCell cellIdf = new TableCell();
        TextBox textIdf = new TextBox();
        cellIdf.Controls.Add(textIdf);

        riadok.Controls.Add(cellIdf);


        TableCell cellLabel = new TableCell();
        TextBox textLabel = new TextBox();
        cellLabel.Controls.Add(textLabel);
        riadok.Controls.Add(cellLabel);

        TableCell cellButtons = new TableCell();
        Button addNew = new Button();
        addNew.Click += new EventHandler(newRow_fnc);
        addNew.Text = "Pridaj";
        cellButtons.Controls.Add(addNew);

        riadok.Controls.Add(cellButtons);


    }
}