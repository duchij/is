using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class is_lf : System.Web.UI.Page
{

    x2_var x2 = new x2_var();
    lf x2lf = new lf();
    log x2log = new log();

    protected void Page_Init (object sender, EventArgs e)
   {
       if (Session["tuisegumdrum"] == null)
       {
           Response.Redirect("error.html");
       }
   }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.loadFolders();
            this.loadFiles();
        }
        else
        {
            //this.loadFolders();
            this.loadFiles();
        }
        
        if (Request.QueryString["dr"] != null)
        {
            try
            {
                this.deleteFile(Convert.ToInt32(Request.QueryString["dr"].ToString()));
            }
            catch (Exception ex)
            {
                x2log.logData(Request.QueryString["dr"].ToString(), ex.ToString(), "error in bad request");
                this.msg_lbl.Text = ex.ToString();
            }
            
        }

    }

    protected void deleteFile(int id)
    {
         
    }

    protected void create_folder_fnc(object sender, EventArgs e)
    {
        string folderName = this.folder_name_txt.Text.ToString().Trim();

        if (folderName.Length > 0)
        {
            string folderHash = x2.make_hash(folderName);
            SortedList data = new SortedList();

            data.Add("item_name", folderName);
            data.Add("item_hash", folderHash);
            data.Add("clinic_id", Session["klinika_id"].ToString());
            data.Add("user_id", Session["user_id"]);
            if (this.see_me_chk.Checked)
            {
                data.Add("visibility", "me");
            }
            else
            {
                data.Add("visibility", "all");
            }
            data.Add("item_comment", this.folder_comment_txt.Text);

            SortedList res = x2lf.mysql_insert("is_structure", data);
            if (Convert.ToBoolean(res["status"]))
            {
                this.loadFolders();
            }
            
        }
    }

    protected void loadFiles()
    {
        int clinic = Convert.ToInt32(Session["klinika_id"]);
        int folder = Convert.ToInt32(this.folders_dl.SelectedValue.ToString());

        this.actual_folder_lbl.Text = this.folders_dl.SelectedItem.ToString();

        this.lf_gv.DataSource = x2lf.getFiles(clinic, folder);
        this.lf_gv.DataBind();
    }

    protected void onPickUp( object sender, EventArgs e)
    {
    }

    protected void loadFolders()
    {
        this.folders_dl.Items.Clear();
        string query = "SELECT [item_name],[item_id] FROM [is_structure] WHERE [item_parent_id] IS NULL AND [clinic_id]='{0}'";

        string sql = x2lf.buildSql(query, new string[] { Session["klinika_id"].ToString() });

        Dictionary<int, Hashtable> table = x2lf.getTable(sql);

        int tableLn = table.Count;
        this.folders_dl.Items.Add(new ListItem("-", "0"));
        for (int i=0; i<tableLn;i++)
        {
            this.folders_dl.Items.Add(new ListItem(table[i]["item_name"].ToString(), table[i]["item_id"].ToString()));
        }
    }

    protected void changeFolderFnc(object sender, EventArgs e)
    {
        this.loadFiles();
    }

    protected void grid_menu_fnc(object sender, EventArgs e)
    {
        this.msg_lbl.Text = sender.ToString();
    }

    protected void download_fnc(object sender, EventArgs e)
    {
        int id =Convert.ToInt32(this.lf_gv.SelectedRow.Cells[0].Text.ToString());
        Response.Redirect("lf.aspx?id=" + id.ToString());
    }

    

    protected void uploadFileFnc(object sender, EventArgs e)
    {
        

        if (this.lf_upf.HasFile)
        {
            

            try
            {
                long size = this.lf_upf.PostedFile.InputStream.Length;
                size = size / 1024000;

                this.msg_lbl.Text = size.ToString();

                if (size > 64)
                {
                    throw new System.Exception("Subor ma viac ako 64M !!!!!");
                }

                string fileLabel = this.file_user_name_txt.Text.ToString().Trim();

                if (fileLabel.Length == 0)
                {
                    fileLabel = this.lf_upf.FileName.ToString();
                }

                SortedList dataFile = new SortedList();
                string fileEx = System.IO.Path.GetExtension(this.lf_upf.FileName);

                byte[] dataB = new byte[this.lf_upf.PostedFile.InputStream.Length];

                this.lf_upf.PostedFile.InputStream.Read(dataB, 0, this.lf_upf.PostedFile.ContentLength);

                dataFile.Add("file-name", this.lf_upf.FileName.ToString());
                dataFile.Add("file-size", this.lf_upf.PostedFile.InputStream.Length);
                dataFile.Add("file-type", fileEx);
                dataFile.Add("user_id", Session["user_id"]);
                dataFile.Add("clinic_id", Session["klinika_id"]);

                SortedList isStruct = new SortedList();
                isStruct.Add("item_name", fileLabel);

                isStruct.Add("item_parent_id", this.folders_dl.SelectedValue.ToString());
                isStruct.Add("item_hash", x2.makeByteHash(dataB));
                isStruct.Add("user_id", Session["user_id"]);
                isStruct.Add("clinic_id", Session["klinika_id"]);

                SortedList res = x2lf.storeLfData(dataB, dataFile, isStruct);

                if (Convert.ToBoolean(res["status"]))
                {
                    this.loadFiles();
                }
                else
                {
                    //this.msg_lbl.Text = res["msg"].ToString();
                    throw new System.Exception(res["msg"].ToString());
                }

                // dataFile.Add("file-content", Convert.ToBase64String(dataB));

                
            }
            catch (Exception ex)
            {
                this.msg_lbl.Text = "<div class='dismissible error message'>"+ex.ToString()+"</div>";
               // this.ctrl_msg_lbl.Text = this.loadFile_fup.PostedFile.FileName + "<br><br>" + ex.ToString();
            }
        }
    }


    protected void lf_gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        int row = e.RowIndex;

        int delId = Convert.ToInt32(this.lf_gv.Rows[row].Cells[0].Text.ToString());

        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='warning message'><h1>Naozaj zmazat subor?</h1>");
        sb.AppendFormat("<a href='is_lf.aspx?dr={0}' class='button green' target='_self'>ANO</a>/NIE </div>",delId);
        this.msg_lbl.Text = sb.ToString();
    }
}