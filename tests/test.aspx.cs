using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using System.Web.UI.WebControls;

public partial class tests_test : System.Web.UI.Page
{

    x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.cryptoTest();
    }


    protected void cryptoTest()
    {
        string test = "toto_je_test";

        string vector = "8080808080808080";


        //string str64 = x2.stringTo64(test.Trim());

        //this.input_lbl.Text = test+"<br>"+str64;
      


       SortedList res = Rijndael.encryptAesJs(test, vector);


        this.input_lbl.Text = test + ":"+res["result"].ToString();

        if (!(Boolean)res["status"])
        {

            this.output_lbl.Text = res["result"].ToString();
        }else
        {

           
            string code = res["result"].ToString();
                
            SortedList res2 = Rijndael.decryptJsAes(code, vector);

            if (!(Boolean)res2["status"])
            {
                this.output_lbl.Text = "o:"+res2["result"].ToString();
            }else
            {
                

                this.output_lbl.Text = "o:"+res2["result"].ToString();
            }


      }


        










    }

}