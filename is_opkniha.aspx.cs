using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public class GalData
{
    public string id { get; set; }
    public string name { get; set; }
    public string path { get; set; }
    public string width { get; set; }
    public string height { get; set; }

}


public partial class is_opkniha : System.Web.UI.Page
{
    x2_var x2 = new x2_var();
    mysql_db x2Mysql = new mysql_db();
    log x2Log = new log();
    gal_db GalDb = new gal_db();

    private List<GalData> PicData = new List<GalData>(); 

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }

        if (Session["opKnihaSelTab"] != null)
        {
            //this.msg_lbl.Text = Session["hlaskoSelTab"].ToString();
            this.opknihaTab_hv.Value = Session["opKnihaSelTab"].ToString();
        }

       
        this.msg_lbl.Text = "";
        this.loadCount();

       if (IsPostBack)
        {
            //Button searchBtn = (Button)

            if (sender.GetType().ToString() == "Button")
            {
                Button btn = (Button)sender;

                if (btn.ID == "galSearch_btn")
                {
                    this.loadSearchResults(this.searchData_lbl.Text.Trim());
                }
            }

            if (Request.QueryString["m"] != null)
            {
                this.parseRequestData(Request);
            }

            
        }

    }

    private void parseRequestData(HttpRequest request)
    {
        string method = Request.QueryString["m"].ToString().Trim();

        switch (method)
        {
            case "loadGalleryData":

                DateTime date = x2.UnixToMsDateTime(request.QueryString["d"].ToString().Trim());
                this.loadSearchResults(date.ToString("ddMMyyyy"));
                break;
        }
    }

    protected void searchGalleryFnc(object sender, EventArgs e)
    {
        //if (is)

        

        if (IsPostBack)
        {

           // this.myTest(null);

            string inDate = this.searchData_lbl.Text.Trim();

            if (string.IsNullOrEmpty(inDate))
            {
                this.loadSearchResults(null);
            }else
            {
               this.loadSearchResults(inDate);
            }

        }

        
    }

    private void myTest(string date)
    {
        CRest myCurl = new CRest();

        Dictionary<string, string> headerData = new Dictionary<string, string>();

        headerData["X-Gallery-Request-Method"] = "get";
        headerData["X-Gallery-Request-Key"] = "de1ef9f8557883c3b7b012211c635518";
        headerData["Content_type"] = "Image/JPG";


        string url = "http://10.10.2.83/gallery3/index.php/rest/item/1306";


        string data = myCurl._csCurl(url, "GET_TXT", headerData);

        this.msg_lbl.Text = data;

    }


    private void loadSearchResults(string inDate)
    {

        string date = null;

        if (string.IsNullOrEmpty(inDate))
        {
            date = this.galOpDate_txt.Text.Trim();
        }
        else
        {
            date = inDate;

            this.galOpDate_txt.Text = date;
        }

        this.galOpDate_txt.Text = date;
        this.searchData_lbl.Text = date;


        Dictionary<string, string> headerData = new Dictionary<string, string>();

        headerData["X-Gallery-Request-Method"] = "get";
        headerData["X-Gallery-Request-Key"] = "de1ef9f8557883c3b7b012211c635518";
        headerData["Content_type"] = "Image/JPG";

        CRest myRest = new CRest();

        try
        {

            if (string.IsNullOrEmpty(date))
            {
                throw new Exception("Nebol zadany datum pre hladanie obrazkov");
            }

            /* string query = @"SELECT 
                             [t_gal.id], [t_gal.name], [t_gal.relative_path_cache] AS [path],
                             [t_gal.width], [t_gal.height]

                             FROM [gal3_items] AS [t_gal]

                             WHERE [t_gal.name] LIKE '%{0}%'

                             ";

             query = GalDb.buildSql(query, new string[] { date });
             Dictionary<int, Hashtable> table = GalDb.getTable(query);*/


            Dictionary<int, Hashtable> table = new Dictionary<int, Hashtable>();
            Dictionary<string, string> getData = new Dictionary<string, string>();

            getData.Add("d", "03032009");

            string res = myRest._csCurl("http://192.168.56.1/dapp/index.php?d=03032009", "GET_TXT", getData);


            JavaScriptSerializer sr = new JavaScriptSerializer();

           // GalData gl = new GalData();

            this.PicData = sr.Deserialize<List<GalData>>(res);


           int tblCnt = table.Count;




            this.foundPictures_lbl.CssClass = "green bold medium";


            string tmpDate = string.Format("{0}.{1}.{2}", date.Substring(0, 2), date.Substring(2, 2), date.Substring(4, 4));

            // string strDate = Convert.ToDateTime(date).ToLongDateString();

            this.foundPictures_lbl.Text = string.Format("Najdenych obrazkov predatum {1} : {0}", tblCnt, tmpDate);


            if (tblCnt > 0)
            {

                int rows = tblCnt % 2;


                string url = null;
                string picData = null;

                for (int i = 0; i < tblCnt; i++)
                {

                    TableRow tblRow = new TableRow();

                    TableCell tblCell = new TableCell();

                    Label lblName = new Label();
                    lblName.CssClass = "asphalt";
                    lblName.Text = string.Format("<p class='asphalt bold'>{0}</p>", table[i]["path"].ToString());
                    tblCell.Controls.Add(lblName);

                    Image imgThumb = new Image();
                    url = string.Format(Resources.Resource.opkniha_gallery_url_thumb, table[i]["id"]);

                    picData = myRest._csCurl(url, "GET_BIN", headerData);
                    string _dt = string.Format(@"data:image/jpeg;base64,{0}", picData);

                    imgThumb.ImageUrl = _dt;
                    imgThumb.Style.Add("display","block");
                    tblCell.Controls.Add(imgThumb);


                    HyperLink lnk = new HyperLink();
                    lnk.Text = "Plna velkost";
                    lnk.CssClass = "button yellow medium";
                    lnk.NavigateUrl = "controls/opkniha_gal.aspx?galId=" + table[i]["id"].ToString(); 
                //    lnk.
                    tblCell.Controls.Add(lnk);

                    tblRow.Controls.Add(tblCell);


                    this.picResult_tbl.Controls.Add(tblRow);
                }
            }
        }


        catch (Exception ex)
        {
            this.msg_lbl.Text = x2.errorMessage("Chyba:  " + ex.Message);
        }
    }


    protected void loadPicture(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] tmp = btn.ID.Split('_');

        Response.Redirect("controls/opkniha_gal.aspx?galId=" + tmp[1]); 



    }

    protected void loadCount()
    {
        string query = "SELECT COUNT(*) AS [rows] FROM [is_opkniha]";
        SortedList row = x2Mysql.getRow(query);

        if (row["status"]==null)
        {
            this.row_counts.Text = row["rows"].ToString();
        }


    }

    

    protected void searchInDgFnc(object sender, EventArgs e)
    {
        string fromDate = null;
        string toDate = null;
        string search = null;

        try
        {
            fromDate = this.dgFrom_txt.Text.Trim();
            toDate = this.dgTo_txt.Text.Trim();

            if (string.IsNullOrEmpty(fromDate))
            {
                throw new Exception("Nie je zadany datum OD");
            }

            if (string.IsNullOrEmpty(toDate))
            {
                throw new Exception("Nie je zadany datum DO");
            }

            search = this.queryDg_txt.Text.ToString().Trim();

            if (search.Length > 0)
            {
                this.searchData(search, fromDate, toDate);
            }
            else
            {
                throw new Exception("Nie čo hľadať !!!!!!");
            }
            

        }
        catch (Exception ex)
        {
           this.msg_lbl.Text = x2.errorMessage("Chyba:  " + ex.ToString());
        }
    }

    protected void searchInMyOPFnc(object sender, EventArgs e)
    {
        string fromDate = null;
        string toDate = null;
        string search = null;

        try
        {
            fromDate = this.myFrom_txt.Text.Trim();
            toDate = this.myTo_txt.Text.Trim();
            search = this.menoMyOP_txt.Text.ToString().Trim();


            if (string.IsNullOrEmpty(fromDate))
            {
                throw new Exception("Nie je zadany datum OD");
            }

            if (string.IsNullOrEmpty(toDate))
            {
                throw new Exception("Nie je zadany datum DO");
            }

            if (search.Length > 0 )
            {
               
                this.searchDataMyOP(search, fromDate, toDate);
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage("Nie čo hľadať !!!!!!");
            }


        }
        catch (Exception ex)
        {
            this.msg_lbl.Text = x2.errorMessage("Chyba:  " + ex.ToString());
        }
    }


    protected void searchInOPFnc(object sender, EventArgs e)
    {
        string fromDate = null;
        string toDate = null;
        string search = null;

        try
        {
            fromDate = this.opFrom_txt.Text.Trim();
            toDate = this.opTo_txt.Text.Trim();
            search = this.queryOp_txt.Text.ToString().Trim();


            if (string.IsNullOrEmpty(fromDate))
            {
                throw new Exception("Nie je zadany datum OD");
            }

            if (string.IsNullOrEmpty(toDate))
            {
                throw new Exception("Nie je zadany datum DO");
            }

            if (search.Length >0)
            {
                this.searchDataOP(search, fromDate, toDate);
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage("Nie čo hľadať !!!!!!");
            }
           

        }
        catch (Exception ex)
        {
            this.msg_lbl.Text = x2.errorMessage("Roky nie sú čísla:  " + ex);
        }
    }

    protected void searchDataOP(string queryStr, string fromDate, string toDate)
    {

        //string queryStr = this.queryDg_txt.Text.ToString().Trim();
        string finalLike = this.parseQuery(queryStr);



        string query = x2.sprintf(
                                    @"SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] 
                                        FROM [is_opkniha] 
                                      WHERE ([vykon] LIKE {0} AND [datum] BETWEEN '{1} 00:00:01' AND '{2} 23:59:59' 
                                        ORDER BY [datum] ", 
                                   new String[] { finalLike, fromDate, toDate });
        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);


        int resLn = table.Count;
        this.foundRows_lbl.Text = "Nájdených:" + resLn.ToString();

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dateHeadCell = new TableHeaderCell();
        dateHeadCell.Text = Resources.Resource.date;

        dateHeadCell.Font.Bold = true;
        dateHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dateHeadCell);

        TableHeaderCell nameHeadCell = new TableHeaderCell();
        nameHeadCell.Text = "Meno";
        nameHeadCell.Font.Bold = true;
        nameHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(nameHeadCell);

        TableHeaderCell rcHeadCell = new TableHeaderCell();
        rcHeadCell.Text = Resources.Resource.birth_num;
        rcHeadCell.Font.Bold = true;
        rcHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(rcHeadCell);

        TableHeaderCell dgHeadCell = new TableHeaderCell();
        dgHeadCell.Text = Resources.Resource.diagnose;
        dgHeadCell.Font.Bold = true;
        dgHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dgHeadCell);

        TableHeaderCell opHeadCell = new TableHeaderCell();
        opHeadCell.Text = "Výkon";
        opHeadCell.Font.Bold = true;
        opHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(opHeadCell);

        TableHeaderCell teamHeadCell = new TableHeaderCell();
        teamHeadCell.Text = "Team";
        teamHeadCell.Font.Bold = true;
        teamHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(teamHeadCell);

        this.result_tbl.Controls.Add(headRow);



        for (int row = 0; row < resLn; row++)
        {
            TableRow resRow = new TableRow();

            TableCell dateCell = new TableCell();
            Label dateLbl = new Label();
            dateLbl.Text = table[row]["datum"].ToString()+"</br>";
            dateCell.Controls.Add(dateLbl);

            //dateCell.Text = table[row]["datum"].ToString();


            LinkButton btnLink = new LinkButton();
            btnLink.CssClass = "button blue medium";
            btnLink.PostBackUrl = string.Format("is_opkniha.aspx?m=loadGalleryData&d={0}", table[row]["datum"]);
            btnLink.Text = "Obrazky pre datum";
            dateCell.Controls.Add(btnLink);

            resRow.Controls.Add(dateCell);

            TableCell nameCell = new TableCell();
            nameCell.Text = table[row]["priezvisko"].ToString();
            resRow.Controls.Add(nameCell);

            TableCell rcCell = new TableCell();
            rcCell.Text = table[row]["rodne_cislo"].ToString();
            resRow.Controls.Add(rcCell);

            TableCell dgCell = new TableCell();
            dgCell.Text = table[row]["diagnoza"].ToString();
            resRow.Controls.Add(dgCell);

            TableCell opCell = new TableCell();
            opCell.Text = table[row]["vykon"].ToString();
            resRow.Controls.Add(opCell);

            TableCell teamCell = new TableCell();
            teamCell.Text = table[row]["operater"].ToString();
            resRow.Controls.Add(teamCell);

            this.result_tbl.Controls.Add(resRow);

        }

    }

    protected void searchDataMyOP(string name, string fromDate, string toDate)
    {

        
        string nameAsci = x2_var.UTFtoASCII(name);

        string query = x2.sprintf(
                                    @"SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] 
                                        FROM [is_opkniha] 
                                    WHERE ([operater] LIKE '%{0}%' OR '%{3}%') 
                                    AND [datum] BETWEEN '{1} 00:00:01' AND '{2} 23:59:59' 
                                        ORDER BY [datum] ", 

                                  new String[] { name, fromDate, toDate,nameAsci });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);


        int resLn = table.Count;
        this.foundRows_lbl.Text = "Nájdených:" + resLn.ToString() + " záznamov."; ;

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dateHeadCell = new TableHeaderCell();
        dateHeadCell.Text = "Datum";
        dateHeadCell.Font.Bold = true;
        dateHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dateHeadCell);

        TableHeaderCell nameHeadCell = new TableHeaderCell();
        nameHeadCell.Text = "Meno";
        nameHeadCell.Font.Bold = true;
        nameHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(nameHeadCell);

        TableHeaderCell rcHeadCell = new TableHeaderCell();
        rcHeadCell.Text = "Rodne cislo";
        rcHeadCell.Font.Bold = true;
        rcHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(rcHeadCell);

        TableHeaderCell dgHeadCell = new TableHeaderCell();
        dgHeadCell.Text = "Diagnoza";
        dgHeadCell.Font.Bold = true;
        dgHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dgHeadCell);

        TableHeaderCell opHeadCell = new TableHeaderCell();
        opHeadCell.Text = "Vykon";
        opHeadCell.Font.Bold = true;
        opHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(opHeadCell);

        TableHeaderCell teamHeadCell = new TableHeaderCell();
        teamHeadCell.Text = "Team";
        teamHeadCell.Font.Bold = true;
        teamHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(teamHeadCell);

        this.result_tbl.Controls.Add(headRow);



        for (int row = 0; row < resLn; row++)
        {
            TableRow resRow = new TableRow();

            TableCell dateCell = new TableCell();

            Label dateLbl = new Label();
            dateLbl.Text = table[row]["datum"].ToString()+"</br>";
            dateCell.Controls.Add(dateLbl);
            

            LinkButton btnLink = new LinkButton();
            btnLink.CssClass = "button blue medium";
            btnLink.PostBackUrl = string.Format("is_opkniha.aspx?m=loadGalleryData&d={0}", table[row]["datum"]);
            btnLink.Text = "Obrazky pre datum";
            dateCell.Controls.Add(btnLink);

            resRow.Controls.Add(dateCell);

            TableCell nameCell = new TableCell();
            nameCell.Text = table[row]["priezvisko"].ToString();
            resRow.Controls.Add(nameCell);

            TableCell rcCell = new TableCell();
            rcCell.Text = table[row]["rodne_cislo"].ToString();
            resRow.Controls.Add(rcCell);

            TableCell dgCell = new TableCell();
            dgCell.Text = table[row]["diagnoza"].ToString();
            resRow.Controls.Add(dgCell);

            TableCell opCell = new TableCell();
            opCell.Text = table[row]["vykon"].ToString();
            resRow.Controls.Add(opCell);

            TableCell teamCell = new TableCell();
            teamCell.Text = table[row]["operater"].ToString();
            resRow.Controls.Add(teamCell);

            this.result_tbl.Controls.Add(resRow);

        }

    }



    protected void searchData(string queryStr, string fromDate, string toDate)
    {

        //string queryStr = this.queryDg_txt.Text.ToString().Trim();
        string finalLike = this.parseQuery(this.queryDg_txt.Text);

        string query = x2.sprintf(@"SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] 
                                        FROM [is_opkniha] 
                                    WHERE 
                                    ([diagnoza] LIKE {0} AND [datum] BETWEEN '{1} 00:00:00' AND '{2} 23:59:59' 
                                    ORDER BY [datum] ", new String[] { finalLike,fromDate,toDate});

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);


        int resLn = table.Count;
        this.foundRows_lbl.Text = "Nájdených:" + resLn.ToString() + " záznamov."; ;

        TableHeaderRow headRow = new TableHeaderRow();

        TableHeaderCell dateHeadCell = new TableHeaderCell();
        dateHeadCell.Text = "Datum";
        dateHeadCell.Font.Bold = true;
        dateHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dateHeadCell);

        TableHeaderCell nameHeadCell = new TableHeaderCell();
        nameHeadCell.Text = "Meno";
        nameHeadCell.Font.Bold = true;
        nameHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(nameHeadCell);

        TableHeaderCell rcHeadCell = new TableHeaderCell();
        rcHeadCell.Text = "Rodne cislo";
        rcHeadCell.Font.Bold = true;
        rcHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(rcHeadCell);

        TableHeaderCell dgHeadCell = new TableHeaderCell();
        dgHeadCell.Text = "Diagnoza";
        dgHeadCell.Font.Bold = true;
        dgHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(dgHeadCell);

        TableHeaderCell opHeadCell = new TableHeaderCell();
        opHeadCell.Text = "Vykon";
        opHeadCell.Font.Bold = true;
        opHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(opHeadCell);

        TableHeaderCell teamHeadCell = new TableHeaderCell();
        teamHeadCell.Text = "Team";
        teamHeadCell.Font.Bold = true;
        teamHeadCell.BackColor = System.Drawing.Color.LightGray;
        headRow.Controls.Add(teamHeadCell);

        this.result_tbl.Controls.Add(headRow);



        for (int row = 0; row < resLn; row++)
        {
            TableRow resRow = new TableRow();

            TableCell dateCell = new TableCell();
            Label dateLbl = new Label();
            dateLbl.Text = table[row]["datum"].ToString() + "</br>";
            dateCell.Controls.Add(dateLbl);


            LinkButton btnLink = new LinkButton();
            btnLink.CssClass = "button blue medium";
            btnLink.PostBackUrl = string.Format("is_opkniha.aspx?m=loadGalleryData&d={0}", table[row]["datum"]);
            btnLink.Text = "Obrazky pre datum";
            dateCell.Controls.Add(btnLink);

            resRow.Controls.Add(dateCell);

            TableCell nameCell = new TableCell();
            nameCell.Text = table[row]["priezvisko"].ToString();
            resRow.Controls.Add(nameCell);

            TableCell rcCell = new TableCell();
            rcCell.Text = table[row]["rodne_cislo"].ToString();
            resRow.Controls.Add(rcCell);

            TableCell dgCell = new TableCell();
            dgCell.Text = table[row]["diagnoza"].ToString();
            resRow.Controls.Add(dgCell);

            TableCell opCell = new TableCell();
            opCell.Text = table[row]["vykon"].ToString();
            resRow.Controls.Add(opCell);

            TableCell teamCell = new TableCell();
            teamCell.Text = table[row]["operater"].ToString();
            resRow.Controls.Add(teamCell);

            this.result_tbl.Controls.Add(resRow);

        }

    }

    private string parseQuery(string query)
    {
        string queryStr = query.ToString().Trim();
        string[] queryArr = queryStr.Split(' ');

        int strLn = queryArr.Length;

        string finalLike = "";

        if (strLn > 0)
        {
            string[] arrTmp = new String[strLn];

            for (int i = 0; i < strLn; i++)
            {
                arrTmp[i] = "'%" + queryArr[i] + "%'";
            }

            finalLike = string.Join(" OR ", arrTmp);
            finalLike = finalLike + ")";
        }
        else
        {
            finalLike = "'%" + queryStr + "%')";
        }

        return finalLike;
    }

    protected void searchToExcelFnc(object sender, EventArgs e)
    {
        string fromDate = null;
        string toDate = null;

        try
        {

            string query = "";

            fromDate = this.dgFrom_txt.Text.ToString();
            toDate = this.dgTo_txt.Text.ToString();

            string finalLike = this.parseQuery(this.queryDg_txt.Text);

            if (finalLike.Trim().Length > 0 && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {

                string sql = @" SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] 
                                    FROM [is_opkniha] 
                                WHERE ([diagnoza] LIKE {0} 
                                    AND [datum] BETWEEN '{1} 00:00:01' AND '{2} 23:59:59'
                                ORDER BY [datum]";


                query = x2.sprintf(sql, new string[] { finalLike, fromDate, toDate });
                
                Session["toExcelQuery"] = query;

                Response.Redirect("toexcel.aspx?a=opres");
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage("Nie čo hľadať !!!!!!");
            }

        }
        catch (Exception ex)
        {
            this.msg_lbl.Text = x2.errorMessage("Chyba:  " + ex.ToString());
        }
        
        //x2Log.logData(finalLike, "", "final query");



    }

    protected void searchOPToExcelFnc(object sender, EventArgs e)
    {

        string fromDate = null;
        string toDate = null;
        try
        {
            fromDate = this.opFrom_txt.Text.ToString().Trim();
            toDate = this.opTo_txt.Text.ToString().Trim();

            string finalLike = this.parseQuery(this.queryOp_txt.Text);

            string query = "";
            if (finalLike.Trim().Length >0 && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {

                string sql = @" SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] 
                                    FROM [is_opkniha] 
                                WHERE ([vykon] LIKE {0}  
                                AND [datum] BETWEEN '{1} 00:00:01' AND '{2} 23:59:59' 
                                    ORDER BY [datum]";

                query = x2.sprintf(sql, new String[] { finalLike,fromDate,toDate });

                Session["toExcelQuery"] = query;

                Response.Redirect("toexcel.aspx?a=opres");
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage("Nie čo hľadať !!!!!!");
            }

        }catch (Exception ex)
        {
            this.msg_lbl.Text = x2.errorMessage("Chyba:  " + ex.ToString());
        }

       
        //x2Log.logData(finalLike, "", "final query");

       


    }

    protected void searchInMyExcelOPFnc(object sender, EventArgs e)
    {

        string fromDate = null;
        string toDate = null;

        try
        {
            fromDate = this.myFrom_txt.Text.ToString().Trim();
            toDate = this.myTo_txt.Text.ToString().Trim();

            if (this.menoMyOP_txt.Text.ToString().Trim().Length > 0 && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                string name = this.menoMyOP_txt.Text.ToString().Trim();
                string nameAsci = x2_var.UTFtoASCII(name);
                   
                //this.searchDataMyOP(this.menoMyOP_txt.Text.ToString().Trim(), fromYear, toYear);
                string query = x2.sprintf(@"SELECT [datum],[priezvisko],[rodne_cislo],[diagnoza],[vykon],[operater] 
                                                FROM [is_opkniha] 
                                            WHERE ([operater] LIKE '%{0}%' OR '%{3}%') 
                                                AND [datum] BETWEEN '{1} 00:00:01' AND '{2} 23:59:59' 
                                                ORDER BY [datum] ", new String[] { name, fromDate, toDate, nameAsci });

                Session["toExcelQuery"] = query;

                Response.Redirect("toexcel.aspx?a=opres",false);
            }
            else
            {
                this.msg_lbl.Text = x2.errorMessage("Nie čo hľadať !!!!!!");
            }


        }
        catch (Exception ex)
        {
            this.msg_lbl.Text = x2.errorMessage("Chyba:  " + ex);
        }
    }


}