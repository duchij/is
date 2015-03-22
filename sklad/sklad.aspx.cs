using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class sklad_hladanie : System.Web.UI.Page
{

    private sklad_db x2sklad = new sklad_db();
    private x2_var x2 = new x2_var();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        /*if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }*/

       // 

        if (!IsPostBack)
        {
            this.tovarDetail_pl.Visible = false;
        }
        else
        {
            this._searchFnc();
        }
    }

    protected void _searchFnc()
    {
        this.tovarDetail_pl.Visible = false;
        string searchIn = this.searchIn_dl.SelectedValue.ToString();
        string phrase = this.phrase_txt.Text.ToString().Trim();
        this.result_tbl.Controls.Clear();


        if (phrase.Length > 0)
        {
            switch (searchIn)
            {
                case "nazov":
                    this.searchInNazov(phrase);
                    break;

                case "sukl":
                    this.searchInSukl(phrase);
                    break;
            }
        }
        else
        {
            this.tovarDetail_pl.Visible = true;
        }
    }

    protected void searchFnc(object sender, EventArgs e)
    {
        this._searchFnc(); 
    }

    protected void searchInSukl(string sukl)
    {
        StringBuilder sb = new StringBuilder();

        string suklComp = sukl.ToString().Trim();
        

        sb.AppendFormat("SELECT [nazov],[code_sukl],[id] FROM [tovar] WHERE [code_sukl]='{0}'", suklComp);
        //this.msg_lbl.Text = sb.ToString();
        
        Dictionary<int, Hashtable> table = x2sklad.getTable(sb.ToString());

        this.generateSearchTableNazov(table);
    }
    protected void searchInNazov(string phrase)
    {
        StringBuilder sb = new StringBuilder();
        string[] phrArr = phrase.Trim().Split(' ');
        
        string finalPartQuery = "";
        int phrLn = phrArr.Length;

        if (phrLn > 0)
        {
            string[] qArr = new string[phrLn];
            for (int i=0; i<phrLn; i++)
            {
                qArr[i] = "'%" + phrArr[i] + "%'";
            }

            finalPartQuery = string.Join("OR", qArr);

        }
        else
        {
            finalPartQuery = "'%" + phrase + "%'";
        }

        sb.AppendFormat("SELECT [nazov],[code_sukl],[id] FROM [tovar] WHERE [nazov] LIKE {0}", finalPartQuery);
       // this.msg_lbl.Text = sb.ToString();
        Dictionary<int, Hashtable> table = x2sklad.getTable(sb.ToString());

        this.generateSearchTableNazov(table);


    }

    protected void generateSearchTableNazov(Dictionary<int, Hashtable> data)
    {
        int dataCn = data.Count;
        this.resultTitle_lbl.Text="Pocet najdenych: "+dataCn.ToString();

        for (int i=0;i<dataCn;i++)
        {
            TableRow riadok = new TableRow();
            this.result_tbl.Controls.Add(riadok);

            TableCell nazovCell = new TableCell();
            nazovCell.Text = "<strong>"+data[i]["nazov"].ToString()+"</strong>";
            riadok.Controls.Add(nazovCell);

            TableCell sukl_kod= new TableCell();
            sukl_kod.Text = "<em>"+data[i]["code_sukl"].ToString()+"</em>";
            riadok.Controls.Add(sukl_kod);

            TableCell akciaCell = new TableCell();
            Button akcia_btn = new Button();
            akciaCell.Controls.Add(akcia_btn);
            
            akcia_btn.Click += new EventHandler(loadTovarDetail);
           // akcia_btn.
            akcia_btn.Text = "Otvor detail tovaru";
            akcia_btn.ID = "tovar_"+data[i]["id"].ToString();
            

            riadok.Controls.Add(akciaCell);
        }
        //this.phrase_txt.Text = "";
    }


    protected void loadTovarDetail(object sender, EventArgs e)
    {
        this.tovarDetail_pl.Visible = true;
        this.clearEans();
        this.resultTitle_lbl.Text = "Detail tovaru:";
        Button btn = (Button)sender;
        string[] idArr = btn.ID.ToString().Split('_');

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [nazov],[code_sukl],[id] FROM [tovar] WHERE [id]='{0}'", idArr[1]);

        //this.msg_lbl.Text = sb.ToString();

        SortedList row = x2sklad.getRow(sb.ToString());
        
        if (row["status"] == null)
        {

            sb.Length = 0;
            sb.AppendFormat("SELECT * FROM [ean_kody] WHERE [tovar_id] = '{0}'", row["id"]);
            SortedList eans = x2sklad.getRow(sb.ToString());

            this.result_tbl.Controls.Clear();
            this.tovarDetail_pl.Visible = true;
            this.nazov_lbl.Text = row["nazov"].ToString();
            this.sukl_lbl.Text = row["code_sukl"].ToString();
            this.tovarId_hf.Value = row["id"].ToString();

            this.phrase_txt.Text = "";

            if (eans["status"] == null && eans.Count >0)
            {
                this.ean1_txt.Text = x2.getStr(eans["ean1"].ToString());
                this.ean2_txt.Text = x2.getStr(eans["ean2"].ToString());
                this.ean3_txt.Text = x2.getStr(eans["ean3"].ToString());
                this.ean4_txt.Text = x2.getStr(eans["ean4"].ToString()); 
                this.eanGen_txt.Text = x2.getStr(eans["eanv"].ToString());
                this.expiry_txt.Text = x2.getStr(eans["expiracia"].ToString());
                this.ean128_txt.Text = x2.getStr(eans["ean1_128"].ToString());
                this.lot_txt.Text = x2.getStr(eans["lot"].ToString());
            }



        }
    }
    protected void clearEans()
    {
        this.ean1_txt.Text = "";
        this.ean2_txt.Text = "";
        this.ean3_txt.Text = "";
        this.ean4_txt.Text = "";
        this.eanGen_txt.Text = "";
        this.ean128_txt.Text = "";
        this.expiry_txt.Text = "";
        this.lot_txt.Text = "";
    }

    protected void saveEanTovarFnc(object sender, EventArgs e)
    {
        SortedList data = new SortedList();
        data["ean1"] = this.ean1_txt.Text.ToString();
        data["ean2"] = this.ean2_txt.Text.ToString();
        data["ean3"] = this.ean3_txt.Text.ToString();
        data["ean4"] = this.ean4_txt.Text.ToString();
        data["tovar_id"] = this.tovarId_hf.Value.ToString();
        data["sukl_kod"] = this.sukl_lbl.Text.ToString();
        data["eanv"] = this.eanGen_txt.Text.ToString().Trim();
        data["expiracia"] = this.expiry_txt.Text.ToString().Trim();
        data["ean1_128"] = this.ean128_txt.Text.ToString().Trim();
        data["lot"] = this.lot_txt.Text.ToString().Trim();

        SortedList res = x2sklad.mysql_insert("ean_kody", data);
        if (Convert.ToBoolean(res["status"]))
        {
            this.phrase_txt.Text = "";
            this.result_tbl.Controls.Clear();
            this.resultTitle_lbl.Text = "";
            this.msg_lbl.Text = "Eany ulozene v poriadku....";
            
            
        }
        else
        {
            this.msg_lbl.Text = res["msg"].ToString();
        }

        this.tovarDetail_pl.Visible = false;


    }

    protected string replaceDiac(string eanIn)
    {
        //string result = "";

        string original = eanIn;

        string firstL = "";
        string lastL = "";
        string ean = "";

        if (eanIn.Substring(0,1) == "ˇ")
        {
            firstL = "+";
            ean = eanIn.Substring(1, eanIn.Length - 1);
        }

        if (eanIn.Substring(eanIn.Length-1,1) == "ˇ")
        {
            lastL = "+";

            if (ean.Length > 0)
            {
                ean = ean.Substring(0, ean.Length - 1);
            }
            else
            {
                ean = eanIn.Substring(0, eanIn.Length - 1);
            }
        }
        
        if (firstL != "+" && lastL !="+")
        {
            ean = original;
        }

        //ean = ean.Replace("ˇ", "+");
        if (ean.Substring(0,1)!="+" && ean.Substring(ean.Length-1,1)!="+")
        {
            ean = ean.Replace("+", "1");
        }
        
        ean = ean.Replace("ľ", "2");
        ean = ean.Replace("š", "3");
        ean = ean.Replace("č", "4");
        ean = ean.Replace("ť", "5");
        ean = ean.Replace("ž", "6");
        ean = ean.Replace("ý", "7");
        ean = ean.Replace("á", "8");
        ean = ean.Replace("í", "9");
        ean = ean.Replace("é", "0");

        return firstL+ean+lastL;
    }

    protected void ean_txt_TextChanged(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;

        string tBoxId = tbox.ID.ToString();

       // string ean = "";
        //string expiration = "";
        //string lot = "";
        TextBox txt = (TextBox)sender;
        txt.Text = this.replaceDiac(txt.Text.ToString());

        string txtToParse = txt.Text.ToString().Trim();

        if (txtToParse.Length >49)
        {
            this.ean1_txt.Text = txtToParse.Substring(0, 50);
            this.ean1_info.Text = "Pozor specifikacia ean128 dovoluje maximalne dlzku 48, bude len 48 znakov akceptovanych....";
        }

        Regex myReg = new Regex(@"^[0-9]*$");

        string startCode = txtToParse.Substring(0, 2);

        if (myReg.IsMatch(txtToParse) && (startCode == "01" || startCode=="17"))
        {
            this.parseData(txt, tBoxId, txtToParse);
        }
        else
        {
            if (startCode == "01" || startCode == "17")
            {
                this.parseData(txt, tBoxId, txtToParse);
            }
            else
            {
                if (tBoxId == "ean1_txt") this.eanGen_txt.Text = txtToParse;
            }
            
        }
        string control = this.eanGen_txt.Text.ToString();

        if (control.Length > 14)
        {
            this.ean13_msg.Text = "Pozor kod je vacsi ako 14 a a bude akceptovanych len 14 znakov";
            this.eanGen_txt.Text = control.Substring(0, 14);
            
        }
        tbox.Focus(); 
        

        //+ľščťžýáíé


    }

    protected void parseData(TextBox tbox, string tBoxId, string txtToParse)
    {
        string ean = "";
        string startCode = txtToParse.Substring(0, 2);
        string expiration = "";
        
        if (tBoxId == "ean1_txt")
        {
            if (startCode == "01")
            {
                if (txtToParse.Substring(2, 1) == "0")
                {
                    ean = txtToParse.Substring(3, 13);
                    this.ean128_txt.Text = txtToParse.Substring(2, 14);
                }
                else
                {
                    ean = txtToParse.Substring(2, 14);
                    this.ean128_txt.Text = txtToParse.Substring(2, 14);
                }

                if (txtToParse.Length >= 18)
                {
                    string code2 = txtToParse.Substring(16, 2);

                    if (code2 == "17")
                    {
                        expiration = txtToParse.Substring(18, 6);
                    }

                    
                }
            }

            if (ean.Length == 0)
            {
                ean = txtToParse;
            }
        }

        if (tBoxId == "ean2_txt")
        {
            if (startCode == "17")
            {
                expiration = txtToParse.Substring(2, 6);

                if (txtToParse.Length>=12)
                {
                    string code3 = txtToParse.Substring(8, 2);
                    if (code3=="10")
                    {
                        this.lot_txt.Text = txtToParse.Substring(10, txtToParse.Length - 10);
                    }
                }
            }
        }


        if (tBoxId == "ean1_txt")
        {
            this.eanGen_txt.Text = ean;
        }


        if (expiration.Length == 6)
        {
            string year = expiration.Substring(0, 2);
            string month = expiration.Substring(2, 2);
            string day = expiration.Substring(4, 2);
            if (day == "00") day = "31";
            this.expiry_txt.Text = "20" + year + "-" + month + "-" + day;
        }
    }
}