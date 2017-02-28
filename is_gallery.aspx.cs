using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class is_gallery : System.Web.UI.Page
{
    x2_var x2 = new x2_var();
    public lf x2lf = new lf();
    log x2log = new log();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        if (!IsPostBack)
        {
            this.loadFirstTen();
        }else
        {
            if (this.searchBy.SelectedValue.ToString() == "ten")
            {
                this.loadFirstTen();
            }
            
        }

    }


    protected void loadFirstTen()
    {
        string sql = @"SELECT   
                                [t_gallery.item_id] AS [item_id],
                                [t_gallery.patient_name] AS [patient_name], 
                                [t_gallery.diagnose] AS [diagnose], 
                                [t_gallery.bin_num] AS [bin_num],
                                [t_gallery.photo_date] AS [photo_date],
                                [t_gallery.note] AS [note], 
                                [t_gallery.lf_id] AS [lf_id],
                                [t_pics.user_id] AS [user_id],
                                [t_pics.clinic_id] AS [clinic_id]
                        FROM [is_gallery] AS [t_gallery]
                        INNER JOIN [is_data_2] as [t_pics] ON [t_pics.id] = [t_gallery.lf_id]
                        WHERE [t_pics.clinic_id] = {0}
                        ORDER BY [t_gallery.photo_date] DESC
                        LIMIT 10
                                ";
        sql = x2.sprintf(sql, new string[] { Session["klinika_id"].ToString() });
        this.showResults(sql);
        
    }

    protected void showResults(string sql)
    {
        this.files_tbl.Controls.Clear();

        sql = x2lf.buildSql(sql, new string[] { });

        Dictionary<int, Hashtable> files = x2lf.getTable(sql);

        if (files.Count > 0)
        {

            TableHeaderRow headRow = new TableHeaderRow();
            TableHeaderCell headCellName = new TableHeaderCell();
            headCellName.Text = "Meno pacienta";
            headRow.Controls.Add(headCellName);

            TableHeaderCell headCellBinNum = new TableHeaderCell();
            headCellBinNum.Text = "Rodné číslo";
            headRow.Controls.Add(headCellBinNum);

            TableHeaderCell headCellDate = new TableHeaderCell();
            headCellDate.Text = "Dátum";
            headRow.Controls.Add(headCellDate);

            TableHeaderCell headCellDg = new TableHeaderCell();
            headCellDg.Text = "Diagnóza";
            headRow.Controls.Add(headCellDg);

            TableHeaderCell headCellNote = new TableHeaderCell();
            headCellNote.Text = "Poznámka";
            headRow.Controls.Add(headCellNote);

            TableHeaderCell headCellActions = new TableHeaderCell();
            headCellActions.Text = "...";
            headRow.Controls.Add(headCellActions);

            this.files_tbl.Controls.Add(headRow);

            int cntLn = files.Count;

            for (int i = 0; i < cntLn; i++)
            {
                TableRow riadok = new TableRow();

                TableCell nameCell = new TableCell();
                nameCell.ColumnSpan = 6;
                Label linkName = new Label();
                linkName.Text = "<span class='info block box'><strong>"+files[i]["patient_name"].ToString()+"</strong></span>";
                linkName.CssClass = "large asphalt";

                nameCell.Controls.Add(linkName);
                 riadok.Controls.Add(nameCell);
                this.files_tbl.Controls.Add(riadok);


                TableRow riadok1 = new TableRow();

                TableCell nameCell1 = new TableCell();

                ImageButton imgBtn = new ImageButton();
                imgBtn.ImageUrl = "http://" + HttpContext.Current.Request.Url.Authority + "/controls/lf_view.ashx?id=" + files[i]["lf_id"].ToString();
                imgBtn.Width = new Unit(100);
                imgBtn.Height = new Unit(100);
                imgBtn.ImageAlign = ImageAlign.Left;
                nameCell1.Controls.Add(imgBtn);
               /* Image img = new Image();

                img.ImageUrl = "http://" + HttpContext.Current.Request.Url.Authority + "/controls/lf_view.ashx?id=" + files[i]["lf_id"].ToString();
                img.ImageAlign = ImageAlign.Left;
                img.Width = new Unit(50);

                nameCell1.Controls.Add(img);*/

               /* HyperLink hlink = new HyperLink();
                hlink.NavigateUrl = "lf.aspx?id=" + files[i]["lf_id"].ToString();
                hlink.Text = "<br>lf.aspx?id=" + files[i]["lf_id"].ToString();
                hlink.CssClass = "blue";
                nameCell1.Controls.Add(hlink);*/

                riadok1.Controls.Add(nameCell1);

                TableCell binNum = new TableCell();
                binNum.Text = x2.getStr(files[i]["bin_num"].ToString());
                riadok1.Controls.Add(binNum);

                TableCell photoDate = new TableCell();
                photoDate.Text = x2.getStr(files[i]["photo_date"].ToString());
                riadok1.Controls.Add(photoDate);

                TableCell diagnose = new TableCell();
                diagnose.Text = x2.getStr(files[i]["diagnose"].ToString());
                riadok1.Controls.Add(diagnose);

                TableCell noteCell = new TableCell();
                noteCell.Text = x2.getStr(files[i]["note"].ToString());
                riadok1.Controls.Add(noteCell);




                TableCell actionCell = new TableCell();
                Button downBtn = new Button();
                downBtn.Text = "Zobraz/Stiahni...";
                downBtn.ID = "downBtn_" + files[i]["lf_id"].ToString();
                downBtn.Click += new EventHandler(this.download_fnc);
                downBtn.CssClass = "button green";

                actionCell.Controls.Add(downBtn);
                if (files[i]["user_id"].ToString() == Session["user_id"].ToString())
                {
                    Button delBtn = new Button();
                    delBtn.Text = Resources.Resource.delete;
                    delBtn.ID = "delBtn_" + files[i]["item_id"].ToString();
                    delBtn.OnClientClick = "return confirm('Zmazať " + files[i]["patient_name"].ToString() + "?');";
                    delBtn.Click += new EventHandler(this.deleteFile_fnc);
                    delBtn.CssClass = "button red";

                    actionCell.Controls.Add(delBtn);

                    /* FileUpload upFile = new FileUpload();
                     upFile.ID = "upFile_" + files[i]["lf_id"].ToString();
                     upFile.Attributes.Add("size", "10px");

                     actionCell.Controls.Add(upFile);

                     Button upDateFileBtn = new Button();
                     upDateFileBtn.Text = "Novšia verzia";
                     upDateFileBtn.CssClass = "button asphalt";
                     upDateFileBtn.ID = "upFileBtn_" + files[i]["lf_id"].ToString();
                     upDateFileBtn.Click += new EventHandler(this.updateFile_fnc);

                     actionCell.Controls.Add(upDateFileBtn);*/

                }


                riadok1.Controls.Add(actionCell);

                this.files_tbl.Controls.Add(riadok1);
            }

        }
    }

    protected void searchGalleryFnc(object sender, EventArgs e)
    {
        string queryBy = this.searchBy.SelectedValue.ToString();
        string key = this.queryString.Text.ToString().Trim();
        string sqlPart = "";
        if (key.Length == 0)
        {
            x2.errorMessage2(ref this.msg_lit, "Nezadany retazec....");
        }
        else
        {
            switch (queryBy)
            {
                case "name":
                    this.searchTitle_lbl.Text = "Hľadanie podľa mena.";
                    sqlPart = x2.sprintf("AND [patient_name] LIKE '%{0}%'", new string[] { key }); 
                     break;
                case "bin_num":
                    this.searchTitle_lbl.Text = "Hľadanie podľa rodného čísla.";
                    sqlPart = x2.sprintf("AND [bin_num] LIKE '{0}%'", new string[] { key });
                    break;
                case "diagnose":
                    this.searchTitle_lbl.Text = "Hľadanie podľa diagnózy.";
                    sqlPart = x2.sprintf("AND [diagnose] LIKE '{0}%'", new string[] { key });
                    break;

            }



          string  sql = @"SELECT   
                                [t_gallery.item_id] AS [item_id],
                                [t_gallery.patient_name] AS [patient_name], 
                                [t_gallery.diagnose] AS [diagnose], 
                                [t_gallery.bin_num] AS [bin_num],
                                [t_gallery.photo_date] AS [photo_date],
                                [t_gallery.note] AS [note], 
                                [t_gallery.lf_id] AS [lf_id],
                                [t_pics.user_id] AS [user_id],
                                [t_pics.clinic_id] AS [clinic_id]
                        FROM [is_gallery] AS [t_gallery]
                        INNER JOIN [is_data_2] as [t_pics] ON [t_pics.id] = [t_gallery.lf_id]
                        WHERE [t_pics.clinic_id] = {0}
                        {1}    
                        ORDER BY [t_gallery.photo_date] DESC";

            sql = x2.sprintf(sql, new string[] { Session["klinika_id"].ToString(), sqlPart });

            this.showResults(sql);

        }



    }

    protected void deleteFile_fnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] idStr = btn.ID.ToString().Split('_');
        int id = Convert.ToInt32(idStr[1]);
        SortedList res = x2lf.execute("DELETE FROM [is_gallery] WHERE [item_id]=" + id);

        if ((Boolean)(res["status"]))
        {
            this.loadFirstTen();
        }
        else
        {
           x2.errorMessage2(ref this.msg_lit,res["msg"].ToString());
        }


    }
    protected void download_fnc(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] idStr = btn.ID.ToString().Split('_');
      //  SortedList row = x2lf.getRow("SELECT [lf_id] AS [id] FROM [is_gallery] WHERE [item_id]=" + idStr[1]);
        int id = Convert.ToInt32(idStr[1].ToString());
        Response.Redirect("lf.aspx?id=" + id.ToString());
    }



}