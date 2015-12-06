using System;
using System.Web.DynamicData;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Serialization;


/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class data
{
    private string _test;
    private string _pokus;

    public string test
    {
        get { return _test; }
        set { _test = value; }
    }

    public string pokus
    {
        get { return _pokus; }
        set { _pokus = value; }
    }
   
   
}

public class news
{
    private string _newsId;
    private string _newsText;

    public string newsId
    {
        get { return _newsId; }
        set { _newsId = value; }
    }
    public string newsText
    {
        get { return _newsText; }
        set { _newsText = value; }
    }
}

public class resultData
{
    //public int row { get; set; }

    public string key { get; set; }
    public string value { get; set; }
}

[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    mysql_db x2Mysql = new mysql_db();
    log x2log = new log();

    //mysql_db db = new mysql_db();

    public WebService () {
         
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
            

    }

    

    //[WebMethod]
    ////[System.Web.Script.Services.ScriptMethod(UseHttpGet = true)]
    //public string HelloWorld(List<data> kilo) {
       
    //    foreach (var row in kilo){
                
    //    }

    //    JavaScriptSerializer js = new JavaScriptSerializer();
    //    data ds = new data();
    //    ds.test = "23";
    //    ds.pokus = "test";
        
    //   // return "{'lalal':'test'}"; 
    //    return js.Serialize(ds).ToString();
    //}

    [WebMethod]
    public string loadNews(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();
        Dictionary<string, string> ds = js1.Deserialize<Dictionary<string, string>>(data);
        news inp = new news();
        news nv = new news();
        nv.newsText = ds["pokus"].ToString();
        nv.newsId = "20";
       // JavaScriptSerializer js = new JavaScriptSerializer();
        // return js.Serialize(nv).ToString();
        return data;
        //return newsId+"sprava";

    }
    [WebMethod]
    public string loadData(string text)
    {
        

        string result;
        string query = "SELECT [stat_vykon] FROM [is_opkniha] WHERE [stat_vykon] LIKE '%{0}%'";

        query = x2Mysql.buildSql(query, new string[] { text });

        Dictionary<int, Hashtable> table = x2Mysql.getTable(query);

        int tblLn = table.Count;
        string[] arr = new string[tblLn];
         JavaScriptSerializer js = new JavaScriptSerializer();
         resultData response = new resultData();
        for (int i = 0; i < tblLn; i++ )
        {
            foreach (DictionaryEntry row in table[i])
            {
                response.key = row.Key.ToString();
                response.value = row.Value.ToString();

                arr[i] = "\""+i.ToString()+"\":"+js.Serialize(response).ToString();
            }
        }

        string res = String.Join(",", arr);

        res = "{" + res + "}";
        x2log.logData(table, "", "result nieco");
       

        try
        {
            //result = js.Serialize(table).ToString();
        }
        catch (Exception ex)
        {
            result = ex.ToString();
            x2log.logData(table, result, "eroro serializer");
        }

        return res;

    }
    
}
