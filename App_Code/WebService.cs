﻿using System;
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

    private Dictionary<string,string> parseDate(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();
        return js1.Deserialize<Dictionary<string, string>>(data);
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

        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id", Convert.ToInt32(obj["user_id"]));
        shiftsData.Add("typ", obj["type"].ToString());
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
            rtData["msg"] = res["msg"].ToString();
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

        if (user_id == 0)
        {
            shiftsData.Add("user_id", null);
        }
        else
        {
            shiftsData.Add("user_id", user_id);
        }
        
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("comment", obj["comment"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));

        SortedList res = x2Mysql.mysql_insert("is_sluzby_2", shiftsData);

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
    public string saveDocShifts(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);

        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id", Convert.ToInt32(obj["user_id"]));
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));
        SortedList res = new SortedList();
        if (Session["rights"].ToString() == "users" && (Session["user_id"].ToString() != obj["user_id"]))
        {
            res["status"] = false;
            res["msg"] = "Nemozete druhemu naplanovat sluzbu nemate pristupove pravo...";
        }
        else
        {
            res = x2Mysql.mysql_insert("is_sluzby_all", shiftsData);
        }

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
    public string saveKDCHDocShifts(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();

        Dictionary<string, string> obj = js1.Deserialize<Dictionary<string, string>>(data);

        SortedList shiftsData = new SortedList();
        shiftsData.Add("clinic", Session["klinika_id"]);

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);

        shiftsData.Add("datum", x2.unixDate(dt));
        shiftsData.Add("user_id", Convert.ToInt32(obj["user_id"]));
        shiftsData.Add("typ", obj["type"].ToString());
        shiftsData.Add("date_group", x2.makeDateGroup(dt.Year, dt.Month));
        SortedList res = new SortedList();
       
        res = x2Mysql.mysql_insert("is_sluzby_2", shiftsData);
        

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