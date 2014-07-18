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

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        check2.Text = "<strong> Registration in progress, please wait...</strong>";
        SortedList my_list = new SortedList();
        my_list.Add(titel1.ID.ToString(), titel1.Text.ToString());
        my_list.Add(name.ID.ToString(), name.Text.ToString());

        my_list.Add(surname.ID.ToString(), surname.Text.ToString());
        my_list.Add(titel2.ID.ToString(), titel2.Text.ToString());
       
        my_list.Add(email.ID.ToString(), email.Text.ToString());
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
       // my_list.Add(sumar_sk.ID.ToString(), sumar_sk.Text.ToString());

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
            check1.Text = "Name, adress are obligatory or your  email address is not correct !!!";

        }

        if (active_status.Checked == true && active_type.Checked == false)
        {
            if (nazov_pred.ToString().Length == 0 || autory_pred.ToString().Length == 0 || abstrakt.ToString().Length == 0)
            {
                status = false;
                check2.Text = "In case of active participation the information about the lecture is obligatory !!!!!";
            }                       
        }
       
        if (status)
        {

            string res = my_db.insert_rows("jasna2011", my_list);

            if (res == "ok")
            {
                SortedList mailData = new SortedList();

                mailData.Add("from", "jasna@kdch.sk");
                mailData.Add("to", my_list["email"]);
                mailData.Add("cc", "jasna@kdch.sk");
                mailData.Add("subject", "Registration -The 57th Congress of Slovak and Czech Paediatric Surgeons");

                string reg_sprava;

                reg_sprava = "<h1>Registration -The 57th Congress of Slovak and Czech Paediatric Surgeons, Jasná,  9. - 12.3.2011, Slovakia</h1><br><br>";
                reg_sprava += "Information about correct registration, Please do not respond to this email<br><br><hr>";
                reg_sprava += "<hr>";
                reg_sprava += "Dear Mrs, Ms., " + my_list["titel1"].ToString() + " " + my_list["name"].ToString() + " " + my_list["surname"].ToString() + " ," + my_list["titel2"].ToString() + "<br>";
                reg_sprava += "<br>";
                // reg_sprava += "Adresa bydliska: " + adr_bydl.Text + "<br>";
                reg_sprava += "Adress: " + my_list["adresa_prac"].ToString() + "<br><hr>";
                //reg_sprava += "Email:" + email.Text + "<br>";

                if (my_list["active_type"].ToString() == "A")
                {
                    reg_sprava += "Active participation: YES<br>";
                }
                else
                {
                    reg_sprava += "Active participation: NO<br>";
                }

                reg_sprava += "<u>Lecture title:</u> <br>" + my_list["nazov_pred"] + "<br>";
                reg_sprava += "<u>Authors:</u> <br>" + my_list["autory_pred"] + "<br>";
                reg_sprava += "<u>Abstract:</u><br> " + my_list["abstrakt"] + "<br>";
                reg_sprava += "<hr>";
                reg_sprava += "Thank you very much for registration, do not respond to this email !!!!<br><br>";
                reg_sprava += "Organisational committee.";

                mailData.Add("message", reg_sprava);

                string m_res = my_x2.mySendMail("mail3.webglobe.sk", mailData, true);

                if (m_res == "ok")
                {
                    Response.Redirect("potvr_en.aspx");
                }
                else
                {
                    check1.Text = "<strong>Error (mail) !!!:</strong> " + m_res;
                    Response.Redirect("potvr_en.aspx");
                }
                
            }
            else
            {
                check1.Text = "<strong>Error (db) !!!:</strong> "+res;
            }
        }


    }

    
}
