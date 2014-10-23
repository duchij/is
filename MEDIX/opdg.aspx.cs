using System;
using System.Collections;

using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class MEDIX_opdg : System.Web.UI.Page
{
    medix x_db = new medix();
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["medixUser"] == null)
        {
            Response.Redirect(@"../error.html");
        }


        if (!IsPostBack)
        {
            this.clearFields();
            this.loadAllData();
            this.loadOdbornosti();

        }
        else
        {
          
            string stemp = this.query_txt.Text;
            stemp = stemp.Trim();

            if (stemp.Length > 0)
            {
                this.loadDataQuery(stemp);
            }
            else if (this.odbor1_db.SelectedValue.ToString() != "none")
            {
                this.loadDataByOdbornost(this.odbor1_db.SelectedValue.ToString());
            }
            else
            {
                this.loadAllData();
            }
        }
    }
    protected void opDg_dg_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        opDg_dg.PageIndex = e.NewPageIndex;
        opDg_dg.DataBind();
    }


    protected void opDg_dg_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //this.debug_lbl.Text = opDg_dg.Rows[e.NewSelectedIndex].Cells[1].Text.ToString();
        this.clearFields();
        SortedList result = x_db.getInfoOpDgData(Convert.ToInt32(this.opDg_dg.Rows[e.NewSelectedIndex].Cells[1].Text.ToString()));
        this.skratka_txt.Text = result["skratka"].ToString();
        this.typop_txt.Text = result["typop"].ToString();
        this.popis_txt.Text = result["popis"].ToString();
        this.timeop_txt.Text = result["dlzka"].ToString();
        this.vzpkod_txt.Text = result["vzpkod"].ToString();
        this.odbornost_txt.Text = result["odbornost"].ToString();
    }

    protected void loadAllData()
    {
        this.query_txt.Text = "";
        this.odbor1_db.SelectedValue = "none";
        this.opDg_dg.DataSource = x_db.getData_OpDg();
        this.opDg_dg.DataBind();

        this.odbornostID.Text = "Vsetky odbornosti";
        this.odbornostLabel.Text = "- Aktualna odbornost";
    }

    protected void clearFields()
    {
        this.opDg_dg.SelectedIndex = -1;
        this.typop_txt.Text = "";
        this.popis_txt.Text = "";
        this.timeop_txt.Text = "";
        this.odbornost_txt.Text = "";
        this.skratka_txt.Text = "";
    }

    protected void loadDataQuery(string query)
    {
       
        //debug_lbl.Text = query;
        
       // this.opDg_dg.er
        this.opDg_dg.DataSource = x_db.getData_OpDg_ByQuery(query);
        this.opDg_dg.DataBind();
    }

    protected void allData_btn_Click(object sender, EventArgs e)
    {
        this.opDg_dg.SelectedIndex = -1;
        this.loadAllData();
    }
    protected void query_btn_Click(object sender, EventArgs e)
    {
        this.odbor1_db.SelectedValue = "none";
        this.loadDataQuery(this.query_txt.Text.ToString());
    }

    protected void save_btn_Click(object sender, EventArgs e)
    {
        //int id = Convert.ToInt32(this.opDg_dg.Rows[this.opDg_dg.SelectedIndex].Cells[1].Text.ToString());
      this.debug_lbl.Text = "";
        if (this.opDg_dg.SelectedRow != null)
        {
            int id = Convert.ToInt32(this.opDg_dg.SelectedRow.Cells[1].Text.ToString());
            this.debug_lbl.Text = id.ToString();

            SortedList data = new SortedList();

            /* this.typop_txt.Text = result["typop"].ToString();
             this.popis_txt.Text = result["popis"].ToString();
             this.timeop_txt.Text = result["dlzka"].ToString();
             this.odbornost_txt.Text = result["odbornost"].ToString();*/

            data.Add("typop", this.typop_txt.Text.ToString());
            data.Add("skratka", this.skratka_txt.Text.ToString());
            data.Add("popis", this.popis_txt.Text.ToString());
            data.Add("dlzka", this.timeop_txt.Text.ToString());
            data.Add("vzpkod", this.vzpkod_txt.Text.ToString());
            data.Add("odbornost", this.odbornost_txt.Text.ToString());

            string result = x_db.update_row("medix_opdg", data, id.ToString());
            if (result != "ok")
            {
                this.debug_lbl.Text = result;
            }
            else
            {
                this.clearFields();

                string odbornost = this.odbor1_db.SelectedValue.ToString();

                if (odbornost == "none")
                {
                    this.loadAllData();
                }
                else
                {
                    this.loadDataByOdbornost(odbornost);
                }

                //this.loadAllData();
            }



        }
        else
        {
            string optype = this.typop_txt.Text.ToString();

            optype = optype.Trim();

            if (optype.Length > 0)
            {


                SortedList data = new SortedList();

                /* this.typop_txt.Text = result["typop"].ToString();
                 this.popis_txt.Text = result["popis"].ToString();
                 this.timeop_txt.Text = result["dlzka"].ToString();
                 this.odbornost_txt.Text = result["odbornost"].ToString();*/

                data.Add("typop", this.typop_txt.Text.ToString());
                data.Add("skratka", this.skratka_txt.Text.ToString());
                data.Add("popis", this.popis_txt.Text.ToString());
                data.Add("dlzka", this.timeop_txt.Text.ToString());
                data.Add("odbornost", this.odbornost_txt.Text.ToString());
                data.Add("vzpkod", this.vzpkod_txt.Text.ToString());

                SortedList result = x_db.insert_rows("medix_opdg", data);

                if (result["status"] != "ok")
                {
                    this.debug_lbl.Text = result["message"].ToString();
                }
                else
                {
                    this.clearFields();

                    string odbornost = this.odbor1_db.SelectedValue.ToString();

                    if (odbornost == "none")
                    {
                        this.loadAllData();
                    }
                    else
                    {
                        this.loadDataByOdbornost(odbornost);
                    }
                    //this.loadAllData();
                }
            }
            else
            {
                this.debug_lbl.Text = "<font color='red'>Názov operácie je povinný !!!</font>";
            }
        }
       


    }
    protected void query_txt_TextChanged(object sender, EventArgs e)
    {

    }

    protected void loadOdbornosti()
    {
        this.odbor1_db.Items.Add(new ListItem("ziadna", "none"));
        // zamen_zoz.Items.Add(new ListItem(tmp.Value.ToString(), tmp.Key.ToString()));
        this.odbor2_dl.Items.Add(new ListItem("ziadna", "none"));


        SortedList odbData = x_db.getOdbornosti();

        foreach (DictionaryEntry tmp in odbData)
        {
            this.odbor1_db.Items.Add(new ListItem(tmp.Key.ToString()+"-"+tmp.Value.ToString(),  tmp.Key.ToString()));
                // zamen_zoz.Items.Add(new ListItem(tmp.Value.ToString(), tmp.Key.ToString()));
            this.odbor2_dl.Items.Add(new ListItem(tmp.Key.ToString() + "-" + tmp.Value.ToString(), tmp.Key.ToString()));
        }

    }
    protected void add_btn_Click(object sender, EventArgs e)
    {
        string odbTmp = this.odbor2_dl.SelectedValue.ToString();

        string odbornost_value = odbornost_txt.Text.ToString();
        
        //debug_lbl.Text = odbornost_value;

        if (odbornost_value.Length == 0)
        {
            this.odbornost_txt.Text = odbTmp;
        }
        else
        {
            this.odbornost_txt.Text += "," + odbTmp;
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        char[] delimiterChars = {','};

        string odbor_str = this.odbornost_txt.Text.ToString();
        string[] odb_arr = odbor_str.Split(delimiterChars);

       if (odb_arr.Length == 0)
       {
           this.odbornost_txt.Text = "";
       }
       else
       {
           odbor_str = "";

           for (int i = 0; i < odb_arr.Length - 1; i++)
           {
               if (i == 0)
               {

                   odbor_str += odb_arr[i];
               }
               else
               {
                   odbor_str += "," + odb_arr[i];
               }
           }
           this.odbornost_txt.Text = odbor_str;
       }

        

    }
    protected void typop_txt_TextChanged(object sender, EventArgs e)
    {
       // this.debug_lbl.Text = "";
    }
    protected void odbor1_db_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.clearFields();
        this.query_txt.Text = "";
        string odbornost = this.odbor1_db.SelectedValue.ToString();

        string odbornostKod = this.odbor1_db.SelectedItem.ToString();

        //this.debug_lbl.Text = odbornost.ToString();
        if (odbornost == "none")
        {
            this.loadAllData();
            this.odbornostID.Text = "Všetky odbornosti";
            this.odbornostLabel.Text = "- Aktuálna odbornosť";
        }
        else
        {
            this.odbornostID.Text = odbornost.ToString();
            this.odbornostLabel.Text = ".." + odbornostKod.ToString();
            this.loadDataByOdbornost(odbornost);
        }


        
        

    }

    protected void loadDataByOdbornost(string odbornost)
    {
        this.opDg_dg.DataSource = x_db.getData_OpDg_ByOdbornost(odbornost);
        this.opDg_dg.DataBind();
    }


    protected void newRow_btn_Click(object sender, EventArgs e)
    {
        this.opDg_dg.SelectedIndex = -1;
        this.clearFields();
    }

    protected void odbornostFnc(object sender, EventArgs e)
    {
        Button item = sender as Button;
        string id = item.ID.ToString();
        this.clearFields();

        if (id.IndexOf("kdch") != -1)
        {
            odbornostID.Text = "107";
            odbornostLabel.Text = "- Detska chirurgia";
        }

        if (id.IndexOf("urol") != -1)
        {
            odbornostID.Text = "109";
            odbornostLabel.Text = "- Pediatrická urológia";
        }

        if (id.IndexOf("ortop") != -1)
        {
            odbornostID.Text = "108";
            odbornostLabel.Text = "- Pediatrická ortopédia";
        }

        if (id.IndexOf("orl") != -1)
        {
            odbornostID.Text = "114";
            odbornostLabel.Text = "- Pediatrická otorinolaryngológia";
        }

        if (id.IndexOf("oftal") != -1)
        {
            odbornostID.Text = "336";
            odbornostLabel.Text = "- Pediatrická oftalmológia";
        }
        if (id.IndexOf("onko") != -1)
        {
            odbornostID.Text = "329";
            odbornostLabel.Text = "- Pediatrická onkológia";
        }

        if (id.IndexOf("aro") != -1)
        {
            odbornostID.Text = "323";
            odbornostLabel.Text = "- Pediatrická anestezilógia";
        }

        string odbornost = this.odbornostID.Text.ToString();

        this.odbor1_db.SelectedValue = odbornost;
        this.loadDataByOdbornost(odbornost);
        


    }
    protected void opDg_dg_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
       int  id = Convert.ToInt32(this.opDg_dg.Rows[e.RowIndex].Cells[1].Text.ToString());
       debug_lbl.Text = id.ToString();
       string res = x_db.delete_row("medix_opdg", id);

       if (res != "ok")
       {
           this.debug_lbl.Text = res;
       }
       else
       {

           this.Page_Load(sender,e);
       }
    }
}
