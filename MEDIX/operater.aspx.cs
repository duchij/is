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

public partial class MEDIX_operater : System.Web.UI.Page
{
    medix x_db = new medix();
    x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["medixUser"] == null)
        {
            Response.Redirect(@"../error.html");
        }

        if (!IsPostBack)
        {
            this.loadOdbornosti();
            this.loadAllData();
        }
        else
        {
            string odbornost = this.s_odbornost_dl.SelectedValue.ToString();

            if (odbornost == "none")
            {
                this.loadAllData();
            }
            else
            {
                this.loadDataByOdbornost(odbornost);
            }
        }

    }

    protected void loadOdbornosti()
    {
        this.odbornost_dl.Items.Add(new ListItem("ziadna", "none"));
        this.s_odbornost_dl.Items.Add(new ListItem("ziadna", "none"));
        // zamen_zoz.Items.Add(new ListItem(tmp.Value.ToString(), tmp.Key.ToString()));


        SortedList odbData = x_db.getOdbornosti();

        foreach (DictionaryEntry tmp in odbData)
        {
            this.odbornost_dl.Items.Add(new ListItem(tmp.Key.ToString() + "-" + tmp.Value.ToString(), tmp.Key.ToString()));
            this.s_odbornost_dl.Items.Add(new ListItem(tmp.Key.ToString() + "-" + tmp.Value.ToString(), tmp.Key.ToString()));
            // zamen_zoz.Items.Add(new ListItem(tmp.Value.ToString(), tmp.Key.ToString()));
        }

    }

    protected void loadAllData()
    {
        //this.s_query_txt.Text = "";
        //this.odbor1_db.SelectedValue = "none";
        this.operater_dg.DataSource = x_db.getData_Operateri();
        this.operater_dg.DataBind();
    }

    protected void saveRow_btn_Click(object sender, EventArgs e)
    {
        if (this.operater_dg.SelectedRow != null)
        {
            int id = Convert.ToInt32(this.operater_dg.SelectedRow.Cells[1].Text.ToString());
           // this.debug_lbl.Text = id.ToString();

            SortedList data = new SortedList();

            /* this.typop_txt.Text = result["typop"].ToString();
             this.popis_txt.Text = result["popis"].ToString();
             this.timeop_txt.Text = result["dlzka"].ToString();
             this.odbornost_txt.Text = result["odbornost"].ToString();*/

            data.Add("osobcisl", this.osobcisl_txt.Text.ToString());
            data.Add("meno", this.meno_txt.Text.ToString());
            data.Add("priezvisko", this.priezvisko_txt.Text.ToString());
            data.Add("titul", this.titul_txt.Text.ToString());
            data.Add("skratka", this.skratka_txt.Text.ToString());

            string funkcia = this.funkcia_dl.SelectedValue.ToString();

            if (funkcia == "sestra")
            {
                data.Add("funkcia", "sestra");
            }
            else
            {
                data.Add("funkcia","lekar");
            }

            string odbornost = this.odbornost_txt.Text.ToString();

            data.Add("odbornost", odbornost);


            string result = x_db.update_row("medix_operateri", data, id.ToString());

            if (result != "ok")
            {
                this.debug_lbl.Text = result;
            }
            else
            {
                this.clearFields();

                string _odbornost = this.s_odbornost_dl.SelectedValue.ToString();

                if (_odbornost == "none")
                {
                    this.loadAllData();
                }
                else
                {
                    this.loadDataByOdbornost(odbornost);
                }
            }



        }
        else
        {
            string meno = this.meno_txt.Text.ToString();
            string priezvisko = this.priezvisko_txt.ToString();
            meno = meno.Trim();
            priezvisko = priezvisko.Trim();

            if (meno.Length > 0 && priezvisko.Length > 0)
            {


                SortedList data = new SortedList();

                /* this.typop_txt.Text = result["typop"].ToString();
                 this.popis_txt.Text = result["popis"].ToString();
                 this.timeop_txt.Text = result["dlzka"].ToString();
                 this.odbornost_txt.Text = result["odbornost"].ToString();*/

                data.Add("osobcisl", this.osobcisl_txt.Text.ToString());
                data.Add("meno", this.meno_txt.Text.ToString());
                data.Add("priezvisko", this.priezvisko_txt.Text.ToString());
                data.Add("titul", this.titul_txt.Text.ToString());
                data.Add("skratka", this.skratka_txt.Text.ToString());

                string funkcia = this.funkcia_dl.SelectedValue.ToString();

                if (funkcia == "sestra")
                {
                    data.Add("funkcia", "sestra");
                }
                else
                {
                    data.Add("funkcia", "lekar");
                }

                string odbornost = this.odbornost_txt.Text.ToString();

                data.Add("odbornost", odbornost);

                SortedList result = x_db.insert_rows("medix_operateri", data);

                if (result["status"] != "ok")
                {
                    this.debug_lbl.Text = result["message"].ToString();
                }
                else
                {
                    this.clearFields();

                    string _odbornost = this.s_odbornost_dl.SelectedValue.ToString();

                    if (_odbornost == "none")
                    {
                        this.loadAllData();
                    }
                    else
                    {
                        this.loadDataByOdbornost(odbornost);
                    }
                }
            }
            else
            {
                this.debug_lbl.Text = "<font color='red'>Meno a priezvisko je povinné !!!</font>";
            }
        }
    }


    protected void clearFields()
    {
        this.operater_dg.SelectedIndex = -1;
        this.osobcisl_txt.Text = "";
        this.meno_txt.Text = "";
        this.priezvisko_txt.Text= "";
        this.titul_txt.Text= "";
        this.skratka_txt.Text = "";
        this.odbornost_txt.Text = "";

        this.operater_dg.SelectedIndex = -1;
    }

    protected void operater_dg_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.operater_dg.PageIndex = e.NewPageIndex;
        this.operater_dg.DataBind();
    }

    protected void operater_dg_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
       this.clearFields();

        int id = Convert.ToInt32(this.operater_dg.Rows[e.NewSelectedIndex].Cells[1].Text.ToString());
        //string id = this.operater_dg.SelectedIndex.ToString();
        this.debug_lbl.Text = id.ToString();
      
        SortedList result = x_db.getInfoOperaterData(id);

        this.osobcisl_txt.Text =    result["osobcisl"].ToString();
        this.meno_txt.Text =        result["meno"].ToString();
        this.priezvisko_txt.Text =  result["priezvisko"].ToString();
        this.titul_txt.Text =       result["titul"].ToString();
        this.skratka_txt.Text =     result["skratka"].ToString();
        this.odbornost_txt.Text =   result["odbornost"].ToString();

        if (result["funkcia"].ToString() == "lekar")
        {
            this.funkcia_dl.SelectedValue = "lekar";
        }
        else
        {
            this.funkcia_dl.SelectedValue = "sestra";
        }
        

    }

    protected void odbAdd_btn_Click(object sender, EventArgs e)
    {
        string odbTmp = this.odbornost_dl.SelectedValue.ToString();

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

    protected void odbDel_btn_Click(object sender, EventArgs e)
    {
        char[] delimiterChars = { ',' };

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

    protected void s_osobcislo_btn_Click(object sender, EventArgs e)
    {
        string osobcislo = this.s_osobcislo_txt.Text;
        this.debug_lbl.Text = "";

        try
        {
            int hodnota = Convert.ToInt32(osobcislo);

            this.operater_dg.DataSource = x_db.getDataByOsobCisl(hodnota);
            this.operater_dg.DataBind();
        }   
        catch (Exception er)
        {
            this.debug_lbl.Text = "Ososne cislo je cele cislo z MEDEI";
        }

        
    }
    protected void s_priezvisko_btn_Click(object sender, EventArgs e)
    {
        string priezvisko = this.s_priezvisko_txt.Text;

        this.operater_dg.DataSource = x_db.getDataByPriezvisko(priezvisko);
        this.operater_dg.DataBind();

    }

    protected void odbornostFnc(object sender, EventArgs e)
    {
        this.clearFields();
        Button item = sender as Button;
        string id = item.ID.ToString();

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

        this.s_odbornost_dl.SelectedValue = odbornost;
        this.loadDataByOdbornost(odbornost);



    }

    protected void s_odbornost_dl_SelectedIndexChanged(object sender, EventArgs e)
    {
        string odbornost = this.s_odbornost_dl.SelectedValue.ToString();

        string odbornostKod = this.s_odbornost_dl.SelectedItem.ToString();

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
            this.odbornostLabel.Text = ".."+odbornostKod.ToString();
            this.loadDataByOdbornost(odbornost);
        }
    }

    protected void loadDataByOdbornost(string odbornost)
    {
        this.operater_dg.DataSource = x_db.getOperaterByOdbornost(odbornost);
        this.operater_dg.DataBind();
    }
    protected void operater_dg_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = Convert.ToInt32(this.operater_dg.Rows[e.RowIndex].Cells[1].Text.ToString());
        debug_lbl.Text = id.ToString();
        string res = x_db.delete_row("medix_operateri", id);

        if (res != "ok")
        {
            this.debug_lbl.Text = res;
        }
        else
        {

            this.Page_Load(sender, e);
        }
    }
    protected void newRow_btn_Click(object sender, EventArgs e)
    {
        this.clearFields();

        this.operater_dg.SelectedIndex = -1;
    }
}
