using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sklad_hladanie : System.Web.UI.Page
{

    public sklad_db x2sklad = new sklad_db();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        /*if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }*/

       // this.tovarDetail_pl.Visible = false;
    }

    protected void searchFnc(object sender, EventArgs e)
    {
        string searchIn = this.searchIn_dl.SelectedValue.ToString();
        string phrase = this.phrase_txt.Text.ToString();

        switch (searchIn)
        {
            case "nazov":
                this.searchInNazov(phrase);
            break;
        }


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

        sb.AppendFormat("SELECT [nazov],[sukl_let],[sukl_num],[id] FROM [tovar] WHERE [nazov] LIKE {0}", finalPartQuery);
        this.msg_lbl.Text = sb.ToString();
        Dictionary<int, Hashtable> table = x2sklad.getTable(sb.ToString());

        this.generateSearchTableNazov(table);


    }

    protected void generateSearchTableNazov(Dictionary<int, Hashtable> data)
    {
        int dataCn = data.Count;

        for (int i=0;i<dataCn;i++)
        {
            TableRow riadok = new TableRow();
            this.result_tbl.Controls.Add(riadok);

            TableCell nazovCell = new TableCell();
            nazovCell.Text = data[i]["nazov"].ToString();
            riadok.Controls.Add(nazovCell);

            TableCell sukl_kod= new TableCell();
            sukl_kod.Text = data[i]["sukl_let"].ToString() + "" + data[i]["sukl_num"].ToString();
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
    }


    protected void loadTovarDetail(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] idArr = btn.ID.ToString().Split('_');

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [nazov],[sukl_let],[sukl_num] WHERE [id]='{0}'", idArr[1]);

        this.msg_lbl.Text = sb.ToString();

        SortedList row = x2sklad.getRow(sb.ToString());
        
        if (row["status"] == null)
        {
            this.tovarDetail_pl.Visible = true;
            this.nazov_lbl.Text = row["nazov"].ToString();
            this.sukl_lbl.Text = row["sukl_let"].ToString() + "" + row["sukl_num"];
        }



    }

}