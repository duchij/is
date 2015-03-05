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
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
        
        if (IsPostBack)
        { 


            string prefix = this.prefix_txt.Text.ToString().Trim();

            if (prefix.Length > 0)
            {
                this._searchPrefix();
            }
            else
            {
                this.loadClinicLabels();
            }
        }
        else
        {
            this.loadClinics();
        }
    }

    protected void changePrefix(object sender, EventArgs e)
    {
        string[] id = this.clinics_dl.SelectedValue.ToString().Split('|');
        this.prefix_txt.Text = id[1].ToLower();

        this._searchPrefix();
    }

    protected void loadClinicLabels()
    {
        string[] id = this.clinics_dl.SelectedValue.ToString().Split('|');
        this.prefix_txt.Text = id[1].ToLower();

        this._searchPrefix();
    }

    protected void loadClinics()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [id],[idf],[full_name] FROM [is_clinics]");

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());
        
        this.clinics_dl.Items.Add(new ListItem("-","0"));

        int tableCn = table.Count;

        for (int i=0; i<tableCn; i++)
        {
            this.clinics_dl.Items.Add(new ListItem(table[i]["full_name"].ToString(), table[i]["id"].ToString() + "|" + table[i]["idf"].ToString()));
        }
    }

    protected void _searchPrefix()
    {
        this.listTable_tbl.Controls.Clear();
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
                
                saveBtn.ID = "saveLabel_" + table[i]["id"].ToString();
                saveBtn.Click += new EventHandler(changeHandlerFnc);
                saveBtn.CssClass = "button green";
                saveBtn.Text = Resources.Resource.save;
                actionCell.Controls.Add(saveBtn);


                Button deleteBtn = new Button();
                deleteBtn.ID = "deleteLabel_" + table[i]["id"].ToString();
                
                deleteBtn.Click += new EventHandler(changeHandlerFnc);
                deleteBtn.CssClass = "button red";
                deleteBtn.Text = Resources.Resource.delete;
                actionCell.Controls.Add(deleteBtn);

                riadok.Controls.Add(actionCell);
            }
        }
    }

    protected void searchPrefixFnc(object sender, EventArgs e)
    {
        this._searchPrefix();
    }

    protected void clearFields()
    {
        this.labelIdf_txt.Text = "";
        this.labelTxt_txt.Text = "";
    }

    protected void changeHandlerFnc(object sender, EventArgs e)
    {
        Button delBtn = (Button)sender;
        StringBuilder sb = new StringBuilder();
        string[] tmp = delBtn.ID.ToString().Split('_');
        if (tmp[0] == "deleteLabel")
        {
            sb.AppendFormat("DELETE FROM [is_labels] WHERE [id]='{0}'", tmp[1]);

            SortedList res = x2Mysql.execute(sb.ToString());
            if (Convert.ToBoolean(res["status"]))
            {
                this.clearFields();
                this._searchPrefix();
            }
        }
        if (tmp[0] == "saveLabel")
        {
            SortedList data = new SortedList();
            data.Add("idf", this.labelIdf_txt.Text.ToString());
            data.Add("label", this.labelTxt_txt.Text.ToString());

            SortedList res1 = x2Mysql.mysql_update("is_labels", data, tmp[1]);
            if (Convert.ToBoolean(res1["statu"]))
            {
                this.clearFields();
                this._searchPrefix();
            }
        }

    }

    protected void saveLabelFnc(object sender, EventArgs e)
    {
        SortedList data = new SortedList();
        data.Add("idf", this.labelIdf_txt.Text.ToString());
        data.Add("label", this.labelTxt_txt.Text.ToString());

        string[] id = this.clinics_dl.SelectedValue.ToString().Split('|');
        if (id[0] != "0")
        {
            data.Add("clinic", id[0]);

            SortedList res = x2Mysql.mysql_insert("is_labels", data);
            if (Convert.ToBoolean(res["status"]))
            {

                this.clearFields();
                this._searchPrefix();
            }
        }
        else
        {
            this.msg_lbl.Text = "Nie je vybrana klinika...";
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