using System;
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

public partial class is_lf : System.Web.UI.Page
{

    x2_var x2 = new x2_var();
   lf x2lf = new lf();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loadStructData();
    }

    protected void myTest()
    {

        TreeNode nieco = new TreeNode();
        nieco.Text = "pokus";

        TreeNode nieco1 = new TreeNode();
        nieco1.Text = "lolo";
       
        nieco.ChildNodes.Add(nieco1);
        files_tv.Nodes.Add(nieco);
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
                data.Add("vibility", "all");
            }
            data.Add("item_comment", this.folder_comment_txt.Text);

            SortedList res = x2lf.mysql_insert("is_structure", data); 
            

        }
    }


    protected void loadStructData()
    {
        this.files_tv.Nodes.Clear();
        string query = x2.sprintf("SELECT * FROM [is_structure] WHERE [clinic_id]={0}", new string[] {Session["klinika_id"].ToString()});

       

        Dictionary<int, Hashtable> table = x2lf.getTable(query);

       int tableLn = table.Count;

       for (int i=0; i<tableLn; i++)
       {
           
            if (table[i]["item_parent_id"].ToString() == "NULL" && table[i]["item_lf_id"].ToString() =="NULL")
            {
                this.createMainNode(table[i]);
            }
           
       }
    }

    protected void createMainNode(Hashtable data)
    {
        Control ctl = FindControl("node_" + data["item_id"].ToString());
        
        
            TreeNode node = new TreeNode("node_"+data["item_id"].ToString());
            node.Text = data["item_name"].ToString();
            this.files_tv.Nodes.Add(node);
       
    }
}