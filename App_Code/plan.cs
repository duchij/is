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
/// Summary description for plan
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class plan : System.Web.Services.WebService
{

    mysql_db x2Mysql = new mysql_db();
    log x2log = new log();
    x2_var x2 = new x2_var();

    public plan()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    public string deleteNurseVacation(string data)
    {

        JavaScriptSerializer js = new JavaScriptSerializer();
        Dictionary<string, string> obj = js.Deserialize<Dictionary<string, string>>(data);

        SortedList saveData = new SortedList();
        saveData.Add("user_id", obj["user_id"]);

        string startDate = x2.sprintf("{0} 00:00:00", new string[] { obj["date"] });

        string endDate = x2.sprintf("{0} 23:59:00", new string[] { obj["date"] });
        saveData.Add("od", startDate);
        saveData.Add("do", endDate);

        saveData.Add("type", "do");
        saveData.Add("comment", "-");
        saveData.Add("clinics", Session["klinika_id"]);
        Dictionary<string, string> rtData = new Dictionary<string, string>();

        string sql = @"
                        DELETE FROM [is_dovolenky_sestr] WHERE [user_id]={0} AND [od]='{1}' AND [do]='{2}'
                        ";
        sql = x2Mysql.buildSql(sql, new string[] { obj["user_id"], startDate, endDate });

        SortedList res = x2Mysql.execute(sql);

        if (!(Boolean)res["status"])
        {
            rtData["status"] = "false";
            rtData["msg"] = res["msg"].ToString();
        }
        else
        {
            rtData["status"] = "true";
        }

        JavaScriptSerializer jsOut = new JavaScriptSerializer();

        return jsOut.Serialize(rtData);



    }

    [WebMethod(EnableSession = true)]
    public string saveNurseVacation(string data)
    {
        //this is a test
        JavaScriptSerializer js = new JavaScriptSerializer();
        Dictionary<string, string> obj = js.Deserialize<Dictionary<string, string>>(data);

        SortedList saveData = new SortedList();
        saveData.Add("user_id", obj["user_id"]);

        string startDate = x2.sprintf("{0} 00:00:00", new string[] { obj["date"] });

        string endDate = x2.sprintf("{0} 23:59:00", new string[] { obj["date"] });
        saveData.Add("od", startDate);
        saveData.Add("do", endDate);

        saveData.Add("type", "do");
        saveData.Add("comment", "-");
        saveData.Add("clinics", Session["klinika_id"]);
        Dictionary<string, string> rtData = new Dictionary<string, string>();
        SortedList res = x2Mysql.mysql_insert("is_dovolenky_sestr", saveData);

        if (!(Boolean)res["status"])
        {
            rtData["status"] = "false";
            rtData["msg"] = res["msg"].ToString();
        }else
        {
            rtData["status"] = "true";
        }

        JavaScriptSerializer jsOut = new JavaScriptSerializer();

        return jsOut.Serialize(rtData);



    }

    [WebMethod(EnableSession = true)]
    public string saveNurseDay(string data)
    {
        Dictionary<string,string> rtData  = new Dictionary<string, string>();
        JavaScriptSerializer js = new JavaScriptSerializer();

        Dictionary<string, string> obj = js.Deserialize<Dictionary<string, string>>(data);
        SortedList res = new SortedList();

        DateTime dt = x2.UnixToMsDateTime(obj["date"]);
        string type = obj["type"];
        int user_id = Convert.ToInt32(obj["user_id"]);

        if (type != "0")
        {
            SortedList saveData = new SortedList();

            saveData.Add("user_id", user_id);
            saveData.Add("datum", obj["date"]);
            saveData.Add("typ", obj["type"]);

           

            int dtGrp = x2.makeDateGroup(dt.Year, dt.Month);

            saveData.Add("date_group", dtGrp);
            saveData.Add("comment", "-");
            saveData.Add("deps", obj["depIdf"]);

            res = x2Mysql.insert_row_old("is_sluzby_2_sestr", saveData);

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
            string query = "DELETE FROM [is_sluzby_2_sestr] WHERE [datum]='{0}' AND [user_id]='{1}' AND [deps]='{2}'";
            query = x2Mysql.buildSql(query, new string[] { x2.unixDate(dt), user_id.ToString(), obj["depIdf"].ToString() });
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

        JavaScriptSerializer jsSer = new JavaScriptSerializer();

        string rtStr = jsSer.Serialize(rtData);

        return rtStr;

    }



}
