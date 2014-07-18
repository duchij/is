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


public partial class abstr : System.Web.UI.Page
{
    my_reg my_db = new my_reg();
    x2_var my_x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
	//form1.visible = false;
	//Button1.visible = false;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        check2.Text = "<strong> Prebieha registrácia, čakajte prosím...</strong>";
        SortedList my_list = new SortedList();
        my_list.Add(titel1.ID.ToString(), titel1.Text.ToString());
        my_list.Add(name.ID.ToString(), name.Text.ToString());

        my_list.Add(surname.ID.ToString(), surname.Text.ToString());
        my_list.Add(titel2.ID.ToString(), titel2.Text.ToString());
       
        my_list.Add("email", email.Text.ToString());
        my_list.Add(adresa_prac.ID.ToString(), adresa_prac.Text.ToString());
        if (active_status.Checked == true)
        {
            my_list.Add("active_status", "A");
        }
        else
        {
            my_list.Add("active_status", "P");
        }

        if (active_type.Checked == true)
        {
            my_list.Add("active_type", "S");
        }
        else
        {
            my_list.Add("active_type", "A");
        }

        my_list.Add(nazov_pred.ID.ToString(), nazov_pred.Text.ToString());
        my_list.Add(autory_pred.ID.ToString(), autory_pred.Text.ToString());
        my_list.Add(praco_pred.ID.ToString(), praco_pred.Text.ToString());
        my_list.Add(sumar_sk.ID.ToString(), sumar_sk.Text.ToString());

        string sumary = sumar_en.Text.ToString();
        sumary = sumary.Replace("'", "*");

        my_list.Add(sumar_en.ID.ToString(), sumary);
        my_list.Add(abstrakt.ID.ToString(), abstrakt.Text.ToString());

        bool status = true;
        

        if (name.Text.ToString().Length == 0)
        {
            status = false;
        }
        if (surname.Text.ToString().Length == 0)
        {
            status = false;
        }

        if (!my_x2.isEmail(email.Text.ToString()))
        {
            status = false;
        }

        if (adresa_prac.Text.ToString().Length == 0)
        {
            status = false;
        }

        if (status == false)
        {
            check1.Text = "Nezadali ste meno, priezvisko, adresu pracoviska alebo emailova adresa nie je spravna!!!";

        }

        if (active_status.Checked == true && active_type.Checked == false)
        {
            if (nazov_pred.ToString().Length == 0 || autory_pred.ToString().Length == 0 || abstrakt.ToString().Length == 0)
            {
                status = false;
                check2.Text = "Pri aktívnej úšasti je nutné vyplniť aj informácie o prednáške !!!!!";
            }                       
        }
       
        if (status)
        {

            string res = my_db.insert_rows("jasna2011", my_list);

            if (res == "ok")
            {
                SortedList mailData = new SortedList();

                mailData.Add("from", "jasna@kdch.sk");
                mailData.Add("cc", my_list["email"].ToString());
                mailData.Add("to", "jasna@kdch.sk");
                
                mailData.Add("subject", "Registrácia - 57. kongres slovenských a českých detských chirurgov");

                string reg_sprava;

                reg_sprava = "<h1>Registrácia - 57. kongres slovenských a českých detských chirurgov, Jasná,  9. - 12.3.2011</h1><br><br>";
                reg_sprava += "Informácia o správne prebehnutej registracii, prosím na tento mail neodpovedajte<br><br><hr>";
                reg_sprava += "<hr>";
                reg_sprava += "Vážená pani/Váženy pán " + my_list["titel1"].ToString() + " " + my_list["name"].ToString() + " " + my_list["surname"].ToString() + " ," + my_list["titel2"].ToString() + "<br>";
                reg_sprava += "<br>";
                // reg_sprava += "Adresa bydliska: " + adr_bydl.Text + "<br>";
                reg_sprava += "Adresa pracoviska: " + my_list["adresa_prac"].ToString() + "<br><hr>";
                //reg_sprava += "Email:" + email.Text + "<br>";

                if (my_list["active_type"].ToString() == "A")
                {
                    reg_sprava += "Aktivna ucast: ANO<br>";
                }
                else
                {
                    reg_sprava += "Aktivna ucast: NIE<br>";
                }

                reg_sprava += "<u>Názov prednášky:</u> <br>" + my_list["nazov_pred"] + "<br>";
                reg_sprava += "<u>Autori:</u> <br>" + my_list["autory_pred"] + "<br>";
                reg_sprava += "<u>Abstrakt:</u><br> " + my_list["abstrakt"] + "<br>";
                reg_sprava += "<hr>";
                reg_sprava += "Ďakujeme za registráciu, na tento email prosim neodpovedajte !!!!<br><br>";
                reg_sprava += "Organizačný výbor.";

                mailData.Add("message", reg_sprava);

                string m_res = my_x2.mySendMail("mail3.webglobe.sk", mailData, true);

                if (m_res == "ok")
                {
                    Response.Redirect("potvr.aspx");
                }
                else
                {
                    check1.Text = "<strong>CHYBA (mail) !!!:</strong> " + m_res;
                    Response.Redirect("potvr.aspx");
                    
                }
                
            }
            else
            {
                check1.Text = "<strong>CHYBA (db) !!!:</strong> "+res;
            }
        }


    }

    
}
