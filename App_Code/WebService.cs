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
    x2_var x2 = new x2_var();
    //mysql_db db = new mysql_db();

    public WebService () {
         
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
            

    }

    protected Dictionary<string,string> deserialize(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();
        return js1.Deserialize<Dictionary<string, string>>(data);
    }


    protected string serialize(Dictionary<string,string> data)
    {
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(data).ToString();
    }

    private Dictionary<string,string> parseDate(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();
        return js1.Deserialize<Dictionary<string, string>>(data);
    }

    [WebMethod(EnableSession = true)]
    public string forceChangePasswd(string data)
    {
        Dictionary<string, string> obj = this.deserialize(data);
        Dictionary<string, string> rtData = new Dictionary<string, string>();

        string name = obj["uname"].ToString();
        string passwd = obj["passwd"].ToString();

        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
       // byte[] dataArr = enc.GetBytes(passwd);

        //string hPasswd = Convert.ToBase64String(dataArr);

        string sql = @"UPDATE [is_users] SET [passwd]='{0}', [force_change]=0  WHERE [name]='{1}'";

        sql = x2Mysql.buildSql(sql, new string[] { passwd, name });

        SortedList res = x2Mysql.execute(sql);

        if (!(Boolean)res["status"])
        {
            rtData["status"] = res["status"].ToString();
            rtData["result"] = res["msg"].ToString();
        }else
        {
            Session.Remove("force_change");
            rtData["status"] = res["status"].ToString();
        }

        

        return this.serialize(rtData);


        
    }

    [WebMethod(EnableSession = true)]
    public string hlaskoSelectedTab(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);
        Session["hlaskoSelTab"] = obj["selTab"].ToString();
        return data;
    }

    [WebMethod(EnableSession = true)]
    public string getSessionID(string data)
    {
        //Session.SessionID
        Dictionary<string, string> rtData = new Dictionary<string, string>();
        rtData["sid"] = System.Web.HttpContext.Current.Session.SessionID;

        return this.serialize(rtData);

    }

    [WebMethod(EnableSession = true)]
    public string saveNursePoziad(string data)
    {
        Dictionary<string, string> obj = this.deserialize(data);
        Dictionary<string, string> rtData = new Dictionary<string, string>();
        if (obj["status"] != "0")
        {
            SortedList saveData = new SortedList();

            saveData.Add("user_id", obj["userId"]);
            saveData.Add("status", obj["status"]);
            saveData.Add("datum", obj["datum"]);
            saveData.Add("clinic_id", Session["klinika_id"].ToString());
            saveData.Add("dep_idf", Session["oddelenie"].ToString());

            SortedList res = x2Mysql.mysql_insert("is_poziad_sestr", saveData);

            if (!(Boolean)res["status"])
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }else
            {
                rtData["status"] = "true";
            }
        }

        if (obj["status"] == "0")
        {
            string sql  = @"DELETE FROM [is_poziad_sestr] WHERE [user_id] = {0} AND [datum] = '{1}'";

            sql = x2Mysql.buildSql(sql, new string[] { obj["userId"], obj["datum"] });

            SortedList res = x2Mysql.execute(sql);

            if (!(Boolean)res["status"])
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }else
            {
                rtData["status"] = "true";
            }



        }
        return this.serialize(rtData);
    }

    [WebMethod(EnableSession = true)]
    public string opknihaSelectedTab(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);
        Session["opKnihaSelTab"] = obj["selTab"].ToString();
        return data;
    }


    [WebMethod(EnableSession = true)]
    public string nkimHlaskoSelectedTab(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);
        Session["nkimHlaskoSelTab"] = obj["selTab"].ToString();
        return data;
    }

    public string deleteNurseActivity(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();
        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        int id = Convert.ToInt32(obj["dovId"]);
        string query = "DELETE FROM [is_dovolenky_sestr] WHERE [id]={0}";
        query = x2Mysql.buildSql(query, new string[] { id.ToString() });

        SortedList res = x2Mysql.execute(query);
        Dictionary<string, string> rtData = new Dictionary<string, string>();

        if ((Boolean)res["status"])
        {
            rtData["status"] = "true";
        }
        else
        {
            rtData["status"] = "false";
            rtData["msg"] = res["msg"].ToString();
        }

        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }

    [WebMethod(EnableSession = true)]
    public string lfSelectedTab(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);
        Session["lfSelTab"] = obj["selTab"].ToString();
        return data;
    }

    [WebMethod(EnableSession = true)]
    public string saveNurseShiftsComment(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
       // shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);

        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id", Convert.ToInt32(obj["user_id"]));
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("comment", obj["comment"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));
        shiftsData.Add("deps", obj["deps"]);
        SortedList res = x2Mysql.mysql_insert("is_sluzby_2_sestr", shiftsData);

        Dictionary<string, string> rtData = new Dictionary<string, string>();

        if ((Boolean)res["status"])
        {
            rtData["status"] = "true";
        }
        else
        {
            rtData["status"] = "false";
            rtData["msg"] = res["msg"].ToString();
        }
        //Session["lfSelTab"] = obj["selTab"].ToString();
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }


    [WebMethod(EnableSession = true)]
    public string saveNurseShifts(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        //shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);
        int userId = Convert.ToInt32(obj["user_id"]);
        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id", userId);
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));
        shiftsData.Add("deps", obj["deps"]);
       

        Dictionary<string, string> rtData = new Dictionary<string, string>();
        SortedList res = new SortedList();

        if (userId >0)
        {
             res = x2Mysql.mysql_insert("is_sluzby_2_sestr", shiftsData);

            if ((Boolean)res["status"])
            {
                rtData["status"] = "true";
            }
            else
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }
        }
        else
        {
            string query = "DELETE FROM [is_sluzby_2_sestr] WHERE [datum]='{0}' AND [typ]='{1}' AND [deps]='{2}'";
            query = x2Mysql.buildSql(query, new string[] { x2.unixDate(dt), obj["type"].ToString(), obj["deps"].ToString() });
            res = x2Mysql.execute(query);

            if ((Boolean)res["status"])
            {
                rtData["status"] = "true";
            }
            else
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }
        }

        
        //Session["lfSelTab"] = obj["selTab"].ToString();
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }




    [WebMethod(EnableSession = true)]
    public string saveDocShiftsComment(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);

        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id", Convert.ToInt32(obj["user_id"]));
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("comment", obj["comment"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));

        SortedList res = x2Mysql.mysql_insert("is_sluzby_all", shiftsData);

        Dictionary<string, string> rtData = new Dictionary<string, string>();

        if ((Boolean)res["status"])
        {
            rtData["status"] = "true";
        }
        else
        {
            rtData["status"] = "false";
            rtData["msg"] = "<b>Nemozete dat poznamku bez zadania mena lekara!!!!</b><p class='small'>"+res["msg"].ToString()+"</p>";
        }
        //Session["lfSelTab"] = obj["selTab"].ToString();
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }

    [WebMethod(EnableSession = true)]
    public string saveKDCHDocShiftsComment(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);

        shiftsData.Add("datum", x2.unixDate(dt));
        int user_id = Convert.ToInt32(obj["user_id"]);
        Dictionary<string, string> rtData = new Dictionary<string, string>();

        if (user_id == 0)
        {
            //shiftsData.Add("user_id", null);
            rtData["status"] = "false";
            rtData["msg"] = "Nie je mozne dat poznamku bez vybraneho lekara pre dany den a typ...";
        }
        else
        {
            shiftsData.Add("user_id", user_id);

            shiftsData.Add("typ", obj["type"].ToString());
            shiftsData.Add("comment", obj["comment"].ToString());
            shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));

            SortedList res = x2Mysql.mysql_insert("is_sluzby_2", shiftsData);



            if ((Boolean)res["status"])
            {
                rtData["status"] = "true";
            }
            else
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }

        }
        
        
        //Session["lfSelTab"] = obj["selTab"].ToString();
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }
        

    [WebMethod(EnableSession = true)]
    public string saveAllDocShiftsComment(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);

        shiftsData.Add("datum", x2.unixDate(dt));
        int user_id = Convert.ToInt32(obj["user_id"]);
        Dictionary<string, string> rtData = new Dictionary<string, string>();

        if (user_id == 0)
        {
            //shiftsData.Add("user_id", null);
            rtData["status"] = "false";
            rtData["msg"] = "Nie je mozne dat poznamku bez vybraneho lekara pre dany den a typ...";
        }
        else
        {
            shiftsData.Add("user_id", user_id);

            shiftsData.Add("typ", obj["type"].ToString());
            shiftsData.Add("comment", obj["comment"].ToString());
            shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));

            SortedList res = x2Mysql.mysql_insert("is_sluzby_all", shiftsData);



            if ((Boolean)res["status"])
            {
                rtData["status"] = "true";
            }
            else
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }

        }


        //Session["lfSelTab"] = obj["selTab"].ToString();
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }




    [WebMethod(EnableSession = true)]
    public string saveDocShifts(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);


        int userId = Convert.ToInt32(obj["user_id"]);

        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id",userId);

        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));
        SortedList res = new SortedList();
        Dictionary<string, string> rtData = new Dictionary<string, string>();
        SortedList free = this.isFree(shiftsData);
        
        if ((Boolean)free["status"])
        {
            if (userId == 0)
            {
                string query = "DELETE FROM [is_sluzby_all] WHERE [datum]='{0}' AND [typ]='{1}' AND [clinic]={2}";
                query = x2Mysql.buildSql(query, new string[] { x2.unixDate(dt).ToString(), obj["type"].ToString(), Session["klinika_id"].ToString() });

                res = x2Mysql.execute(query);

                if ((Boolean)res["status"])
                {
                    rtData["status"] = "true";
                }
                else
                {
                    rtData["status"] = "false";
                    rtData["msg"] = res["msg"].ToString();
                }
            }
            else
            {
                if (Session["rights"].ToString() == "users" && (Session["user_id"].ToString() != obj["user_id"]))
                {
                    rtData["status"] = "false";
                    rtData["msg"] = "Nemozete druhemu naplanovat sluzbu nemate pristupove pravo...";
                }
                else
                {
                    res = x2Mysql.mysql_insert("is_sluzby_all", shiftsData);

                    if ((Boolean)res["status"])
                    {
                        rtData["status"] = "true";
                    }
                    else
                    {
                        rtData["status"] = "false";
                        rtData["msg"] = res["msg"].ToString();
                    }
                }
            }
        }
        else
        {
            rtData["status"] = "false";
            rtData["msg"] = "Miesto je uz obsadene !!!!";
            rtData["user_id"] = free["user_id"].ToString();
        }

        

        

        
        //Session["lfSelTab"] = obj["selTab"].ToString();
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }


    [WebMethod(EnableSession = true)]
    public string saveKDCHDocShifts(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);
        int userId = Convert.ToInt32(obj["user_id"]);
        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id",userId);
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));
        SortedList res = new SortedList();
        Dictionary<string, string> rtData = new Dictionary<string, string>();

        if (userId==0)
        {
            string query = "DELETE FROM [is_sluzby_2] WHERE [datum]='{0}' AND [typ]='{1}' AND [clinic]={2}";
            query = x2Mysql.buildSql(query,new string[] { x2.unixDate(dt).ToString(), obj["type"].ToString(), Session["klinika_id"].ToString() });

            res = x2Mysql.execute(query);

            if ((Boolean)res["status"])
            {
                rtData["status"] = "true";
            }
            else
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }
        }
        else
        {
            res = x2Mysql.mysql_insert("is_sluzby_2", shiftsData);

            if ((Boolean)res["status"])
            {
                rtData["status"] = "true";
            }
            else
            {
                rtData["status"] = "false";
                rtData["msg"] = res["msg"].ToString();
            }
        }
       
        //Session["lfSelTab"] = obj["selTab"].ToString();
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }

    private SortedList isFree(SortedList data)
    {
        string query = "SELECT [user_id] FROM [is_sluzby_all] WHERE [datum]='{0}' AND [typ]='{1}' AND [clinic]='{2}'";

        query = x2Mysql.buildSql(query, new string[] { data["datum"].ToString(), data["typ"].ToString(), Session["klinika_id"].ToString() });
        SortedList row = x2Mysql.getRow(query);

        SortedList result = new SortedList();
        result["status"] = true; 
        if (row["user_id"] != null)
        {
            result["status"] = false;
            result["user_id"] = row["user_id"];
        }

        return result;

    }

    [WebMethod(EnableSession = true)]
    public string saveAllDocShifts(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);
        int userId = Convert.ToInt32(obj["user_id"]);
        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id", userId);
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));

       
        Dictionary<string, string> rtData = new Dictionary<string, string>();

        SortedList free = this.isFree(shiftsData);

        if ((Boolean)free["status"])
        {
            SortedList res = new SortedList();

            if (userId == 0)
            {
                string query = "DELETE FROM [is_sluzby_all] WHERE [datum]='{0}' AND [typ]='{1}' AND [clinic]={2}";
                query = x2Mysql.buildSql(query, new string[] { x2.unixDate(dt).ToString(), obj["type"].ToString(), Session["klinika_id"].ToString() });

                res = x2Mysql.execute(query);

                if ((Boolean)res["status"])
                {
                    rtData["status"] = "true";
                }
                else
                {
                    rtData["status"] = "false";
                    rtData["msg"] = res["msg"].ToString();
                }
            }
            else
            {
                res = x2Mysql.mysql_insert("is_sluzby_all", shiftsData);

                if ((Boolean)res["status"])
                {
                    rtData["status"] = "true";
                }
                else
                {
                    rtData["status"] = "false";
                    rtData["msg"] = res["msg"].ToString();
                }
            }
        }
        else
        {
            rtData["status"] = "false";
            rtData["msg"] = "Miesto je uz obsadene !!!!";
            rtData["user_id"] = free["user_id"].ToString();
        }

        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(rtData).ToString();
    }




    [WebMethod(EnableSession = true)]
    public string OopknihaSelectedTab(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);
        Session["opKnihaSelTab"] = obj["selTab"].ToString();
        return data;
    }



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
