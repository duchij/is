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
using System.Web.Services;

public partial class is_lf : System.Web.UI.Page
{

    x2_var x2 = new x2_var();
    public lf x2lf = new lf();
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
        if (Session["lfSelTab"] != null)
        {
            //this.msg_lbl.Text = Session["hlaskoSelTab"].ToString();
            this.setlftab_hv.Value = Session["lfSelTab"].ToString();
        }

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
    }

    protected void deleteFile(int id)
    {
        string query = @"DELETE FROM [is_structure] WHERE [item_lf_id] = {0} ";

        query = x2lf.buildSql(query, new string[] { id.ToString() });

        SortedList res = x2lf.execute(query);
        if (!(Boolean)res["status"])
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }
        else
        {
            this.loadFiles();
        }

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
                data.Add("visibility", Session["user_id"]);
            }
            else
            {
                data.Add("visibility", 0);
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
        this.files_tbl.Controls.Clear();

        int clinic = Convert.ToInt32(Session["klinika_id"]);
        int folder = Convert.ToInt32(this.folders_dl.SelectedValue.ToString());

        this.actual_folder_lbl.Text = this.folders_dl.SelectedItem.ToString();

        int userId = Convert.ToInt32(Session["user_id"].ToString());

        Dictionary<int, Hashtable> files = x2lf.getFiles(clinic, folder,userId);

        TableHeaderRow headRow = new TableHeaderRow();
        TableHeaderCell headCellName = new TableHeaderCell();
        headCellName.Text = "Nazov";
        headRow.Controls.Add(headCellName);

        TableHeaderCell headCellNote = new TableHeaderCell();
        headCellNote.Text = "Popis";
        headRow.Controls.Add(headCellNote);

        TableHeaderCell headCellActions = new TableHeaderCell();
        headCellActions.Text = "...";
        headRow.Controls.Add(headCellActions);

        this.files_tbl.Controls.Add(headRow);

        int tblLn = files.Count;

        for (int i=0; i< tblLn; i++)
        {
            TableRow riadok = new TableRow();

            TableCell nameCell = new TableCell();
            nameCell.Text = files[i]["item_name"].ToString();
            riadok.Controls.Add(nameCell);

            TableCell noteCell = new TableCell();
            noteCell.Text = x2.getStr(files[i]["item_comment"].ToString());
            riadok.Controls.Add(noteCell);

            TableCell actionCell = new TableCell();
            Button downBtn = new Button();
            downBtn.Text = "Zobraz/Stiahni...";
            downBtn.ID = "downBtn_" + files[i]["item_id"].ToString();
            downBtn.Click += new EventHandler(download_fnc);
            downBtn.CssClass = "button green";

            actionCell.Controls.Add(downBtn);
            if (files[i]["user_id"].ToString() == Session["user_id"].ToString())
            {
                Button delBtn = new Button();
                delBtn.Text = "Zmaz";
                delBtn.ID = "delBtn_" + files[i]["item_id"].ToString();
                delBtn.OnClientClick = "return confirm('Zmazať " + files[i]["item_name"].ToString() + "?');";
                delBtn.Click += new EventHandler(deleteFile_fnc);
                delBtn.CssClass = "button red";

                actionCell.Controls.Add(delBtn);
            }
            

            riadok.Controls.Add(actionCell);

            this.files_tbl.Controls.Add(riadok);

        }

    }

    protected void onPickUp( object sender, EventArgs e)
    {
    }

    protected void loadFolders()
    {
        this.folders_dl.Items.Clear();
        this.del_folders_dl.Items.Clear();
        string query = @"SELECT [item_name],[item_id] 
                            FROM [is_structure] 
                        WHERE [item_parent_id] IS NULL 
                            AND [clinic_id]='{0}'
                               AND ([visibility]={1} OR [visibility]=0)
                            ";

        string sql = x2lf.buildSql(query, new string[] { Session["klinika_id"].ToString(),Session["user_id"].ToString() });

        Dictionary<int, Hashtable> table = x2lf.getTable(sql);

        int tableLn = table.Count;
        this.folders_dl.Items.Add(new ListItem("-", "0"));
        for (int i=0; i<tableLn;i++)
        {
            this.folders_dl.Items.Add(new ListItem(table[i]["item_name"].ToString(), table[i]["item_id"].ToString()));
            this.del_folders_dl.Items.Add(new ListItem(table[i]["item_name"].ToString(), table[i]["item_id"].ToString()));
        }

        

    }

    protected void changeFolderFnc(object sender, EventArgs e)
    {
        this.loadFiles();
    }

   

    protected void download_fnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] idStr = btn.ID.ToString().Split('_');
        SortedList row = x2lf.getRow("SELECT [item_lf_id] AS [id] FROM [is_structure] WHERE [item_id]=" + idStr[1]);
        int id = Convert.ToInt32(row["id"].ToString());
        Response.Redirect("lf.aspx?id=" + id.ToString());
    }

    protected void deleteFile_fnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] idStr = btn.ID.ToString().Split('_');
        int id = Convert.ToInt32(idStr[1]);
        SortedList res = x2lf.execute("DELETE FROM [is_structure] WHERE [item_id]=" + id);
        if ((Boolean)(res["status"]))
        {
            this.loadFiles();
        }
        else
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }

        
    }

    protected void deleteFolderFnc(object sender, EventArgs e)
    {
        int folderId = Convert.ToInt32(this.del_folders_dl.SelectedValue.ToString());

        if (folderId > 0)
        {
            SortedList res = x2lf.deleteFolder(folderId);

            if ((Boolean)res["status"])
            {
                this.loadFolders();
                this.loadFiles();
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
            }
        }
        
    }

   

    protected void uploadFileFnc(object sender, EventArgs e)
    {
        if (this.lf_upf.HasFile)
        {
            try
            {
                long size = this.lf_upf.PostedFile.InputStream.Length;
                size = size / 1024000;

                //this.msg_lbl.Text = size.ToString();

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
                this.msg_lbl.Text = x2.errorMessage("Súbor sa nenahral...!!!!");
               // this.ctrl_msg_lbl.Text = this.loadFile_fup.PostedFile.FileName + "<br><br>" + ex.ToString();
            }
        }
    }

    protected void editVisibility(string st)
    {
        string fId = this.del_folders_dl.SelectedValue.ToString();

        string visibility = "0";
        if (st=="me")
        {
            visibility = Session["user_id"].ToString();
        }
        string query = "UPDATE [is_structure] SET [visibility]={0} WHERE [item_id]={1}";

        query = x2lf.buildSql(query, new string[] { visibility, fId });

        SortedList res = x2lf.execute(query);
        if ((Boolean)res["status"])
        {
            //this.loadFolders();

        }
        else
        {
            this.msg_lbl.Text = x2.errorMessage(res["msg"].ToString());
        }
        
    }

    protected void setVisibilityFnc(object sender, EventArgs e)
    {
        RadioButton rb = (RadioButton)sender;

        string id = rb.ID.ToString();

        if (id == "edit_visibility_me")
        {
            this.editVisibility("me");
        }
        if (id == "edit_visibility_all")
        {
            this.editVisibility("all");
        }
    }
}