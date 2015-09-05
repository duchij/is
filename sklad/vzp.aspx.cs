using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class sklad_vzp : System.Web.UI.Page
{
    x2_var x2 = new x2_var();
    vzp x2mysql = new vzp();
    lf x2lf = new lf();
    public SortedList vzpData = new SortedList();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("../error.html");
        }

        vzpData = (SortedList)Session["sklad_vzp"];

        if (Request.QueryString["dr"] != null)
        {
            try {
                this.deleteFirmFnc(Convert.ToInt32(Request.QueryString["dr"].ToString()));
            }
            catch (Exception ex)
            {
                Response.Redirect("vzp.aspx");
            }
            
        }
        this.msg_lbl.Text = "";

        if (!IsPostBack)
        {
            this.year_dl.SelectedValue = DateTime.Today.Year.ToString();
            this.Calendar1.SelectedDate = DateTime.Today;
            this.loadFirms();
        }

        if (this.vzpData["search"].ToString() == "all")
        {
            this.loadData();
        }
        
    }

    protected void loadFirms()
    {
        this.firma_dl.Items.Clear();
        this.firmsSearch_dl.Items.Clear();
        string query = "SELECT [item_id],[item_label] FROM [is_sklad_firms] ORDER BY [item_label]";

        Dictionary<int, Hashtable> table = x2mysql.getTable(query);

        int tableLn = table.Count;
        this.firma_dl.Items.Add(new System.Web.UI.WebControls.ListItem("-", "0"));
        for (int i=0; i<tableLn; i++)
        {
            this.firma_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[i]["item_label"].ToString(), table[i]["item_id"].ToString()));
            this.firmsSearch_dl.Items.Add(new System.Web.UI.WebControls.ListItem(table[i]["item_label"].ToString(), table[i]["item_id"].ToString()));
        }
        

    }

    protected void deleteFirmFnc(int id)
    {
        string query = x2mysql.buildSql("DELETE FROM [is_sklad_firms] WHERE [item_id]='{0}'", new string[] { id.ToString() });

        SortedList res = x2mysql.execute(query);

        if (Convert.ToBoolean(res["status"]))
        {
            Response.Redirect("vzp.aspx");
        }
        else
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }

    }

    protected void loadData()
    {
        string year = this.year_dl.SelectedValue.ToString();
        this.vzp_gv.DataSource = x2mysql.fillVzpDataSet("all", "0", year);
        this.vzp_gv.DataBind();
    }
    

    protected void searchBy(object sender,EventArgs e)
    {
        Button btn = (Button)sender;
        string id = btn.ID.ToString();
        string year = this.year_dl.SelectedValue.ToString();
       
        switch (id)
        {
            case "byFirm_btn":
                this.vzpData["search"] = "firm";
                this.vzp_gv.DataSource =  x2mysql.fillVzpDataSet("firm", this.firmsSearch_dl.SelectedValue.ToString(),year);
                break;
            case "byVzp_btn":
                this.vzpData["search"] = "vzp";
                this.vzp_gv.DataSource = x2mysql.fillVzpDataSet("vzp", this.vzp_search_txt.Text.ToString().Trim(),year);
                break;
        }

        this.vzp_gv.DataBind();
       
    }

    protected void showFileFnc(object sender, EventHandler e)
    {

    }

    protected void processPdfFileFnc(object sender, EventArgs e)
    {
        if (this.upload_fv.HasFile)
        {
           
            DateTime dt = DateTime.UtcNow;
            string str = dt.ToLongTimeString() + "subor";
            string hash = x2.makeHashString(str);
            string location = Server.MapPath("../App_Data/") + @hash+".pdf";

            string milis = DateTime.UtcNow.Millisecond.ToString();

            string fileNameNew = x2.makeHashString(milis + "pdf") + ".pdf";

            string newFileLocation = Server.MapPath("../App_Data") + "//"; 



            this.upload_fv.SaveAs(location);

            PdfReader reader = new PdfReader(location);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document myDoc = new Document(PageSize.A4);

            FileStream fs = new FileStream(newFileLocation+fileNameNew, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(myDoc, fs);
            myDoc.Open();

            BaseFont mojFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            

            // the pdf content
            //PdfWriter pw = writer.DirectContent;
            PdfContentByte cb = writer.DirectContent;
            cb.SetFontAndSize(mojFont, 10);
            cb.BeginText();
            cb.MoveText(470, size.Height - 65);
            cb.ShowText(this.vzp_txt.Text.ToString());
            cb.EndText();

            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);


            myDoc.Close();
            fs.Close();
            writer.Close();
            reader.Close();

            SortedList res = this.uploadData(newFileLocation + fileNameNew, this.vzp_txt.Text.ToString());

            if (Convert.ToBoolean(res["status"]))
            {
                System.IO.File.Delete(location);
                

                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileNameNew);
                Response.TransmitFile(newFileLocation + fileNameNew);
                Response.End();

                System.IO.File.Delete(newFileLocation + fileNameNew);

                this.changeKeyInSession("search", "all");
                this.loadData();
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }

            
        }
    }

    protected void changeKeyInSession(String key,String value)
    {
        SortedList st = (SortedList)Session["sklad_vzp"];
        st[key] = value;
        Session["sklad_vzp"] = st;
    }

    protected SortedList uploadData(string fileName, string vzp)
    {
       
        string fileEx = System.IO.Path.GetExtension(fileName);
        string file = System.IO.Path.GetFileName(fileName);


        byte[] dataB = System.IO.File.ReadAllBytes(fileName);

        //this.upload_fv.PostedFile.InputStream.Read(dataB, 0, this.upload_fv.PostedFile.ContentLength);
        SortedList dataFile = new SortedList();
        dataFile.Add("file-name", file);
        dataFile.Add("file-size", dataB.Length);
        dataFile.Add("file-type", fileEx);
        dataFile.Add("user_id", Session["user_id"]);
        dataFile.Add("clinic_id", Session["klinika_id"]);

        SortedList storeData = new SortedList();

        storeData.Add("vzp", vzp);
        storeData.Add("item_firm", this.firma_dl.SelectedValue.ToString());
        storeData.Add("item_date", x2.unixDate(this.Calendar1.SelectedDate));
        storeData.Add("user_id", Session["user_id"]);
        storeData.Add("item_hash", x2.makeByteHash(dataB));

        return x2lf.storeLfDataInTable(dataB, dataFile, "is_sklad_vzp", storeData);
      //  if (res["status"])

        //dataFile.Add("clinic_id", null);
    }

    protected void vzp_gv_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = this.vzp_gv.SelectedRow.Cells[0].Text.ToString();
        this.msg_lbl.Text = id;

        Response.Redirect(@"../lf.aspx?id=" + id.ToString());
    }
    protected void vzp_gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int row = e.RowIndex;

        GridView gv = (GridView)sender;

        int id = Convert.ToInt32(gv.Rows[row].Cells[0].Text.ToString());

        string query = "DELETE FROM [is_data_2] WHERE [id]={0}";

        query = x2mysql.buildSql(query, new string[] { id.ToString() });

        SortedList res = x2mysql.execute(query);

        if (Convert.ToBoolean(res["status"]))
        {
            this.changeKeyInSession("search", "all");
            this.loadData();
        }
        else
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }

    }
    protected void firma_dl_SelectedIndexChanged(object sender, EventArgs e)
    {
        string query = @"SELECT [item_label],[item_adress] FROM [is_sklad_firms] WHERE [item_id]='{0}'";

        string id = this.firma_dl.SelectedValue.ToString();

        if (id != "0")
        {
            query = x2mysql.buildSql(query, new string[] { id });

            SortedList row = x2mysql.getRow(query);

            if (row["item_label"] != null)
            {
                this.firm_name_txt.Text = row["item_label"].ToString();
                this.firm_address_txt.Text = x2.getStr(row["item_adress"].ToString());
            }
        }
        else
        {
            this.firm_name_txt.Text = "";
            this.firm_address_txt.Text = "";
        }
        


    }
    protected void save_firm_btn_Click(object sender, EventArgs e)
    {
        string id = this.firma_dl.SelectedValue.ToString();

        if (id != "0")
        {
            SortedList data = new SortedList();
            data.Add("item_label", this.firm_name_txt.Text.ToString());
            data.Add("item_adress", this.firm_address_txt.Text.ToString());



            SortedList res = x2mysql.mysql_update("is_sklad_firms", data, "WHERE `item_id`='"+id+"'");

            if (Convert.ToBoolean(res["status"]))
            {
                this.firm_name_txt.Text = "";
                this.firm_address_txt.Text = "";
                this.loadFirms();
            }
            else
            {
                this.msg_lbl.Text = res["msg"].ToString();
            }


        }
        else
        {
            string firmName = this.firm_name_txt.Text.ToString().Trim();
            string firmAdress = this.firm_address_txt.Text.ToString().Trim();

            if (firmName.Length >0  && firmAdress.Length>0)
            {
                SortedList dataIns = new SortedList();
                dataIns.Add("item_label", firmName);
                dataIns.Add("item_adress", firmAdress);

                SortedList res1 = x2mysql.mysql_insert("is_sklad_firms", dataIns);

                if (Convert.ToBoolean(res1["status"]))
                {
                    this.firm_name_txt.Text = "";
                    this.firm_address_txt.Text = "";
                    this.loadFirms();
                }
                else
                {
                    this.msg_lbl.Text = res1["msg"].ToString();
                }
            }
            else
            {
                this.msg_lbl.Text = "<h3 class='red'>Firma a adresa musia byt vyplnene !!!!</h3>";
            }
        }

    }
    protected void delete_firm_btn_Click(object sender, EventArgs e)
    {
        string id = this.firma_dl.SelectedValue.ToString();

        if (id!="0")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='warning message'> Naozaj chcete vymazat? Pozor zmazu sa vsetky zmluvy !!!!");
            sb.AppendFormat("<a href='vzp.aspx?dr={0}' target='_self'>ANO</a>/<a href='vzp.aspx' target='_self'>NIE</a> </div>", id);
            this.msg_lbl.Text = sb.ToString();
        }
    }
    protected void vzp_gv_PageIndexChanged(object sender, EventArgs e)
    {
        string ss = this.vzpData["search"].ToString();
        string year = this.year_dl.SelectedValue.ToString();
        switch (ss)
        {
            case "firm":
                this.vzp_gv.DataSource = x2mysql.fillVzpDataSet("firm", this.firmsSearch_dl.SelectedValue.ToString(), year);
                break;
            case "vzp":
                this.vzp_gv.DataSource = x2mysql.fillVzpDataSet("vzp", this.vzp_search_txt.Text.ToString().Trim(), year);
                break;

        }

        this.vzp_gv.DataBind();
    }
}