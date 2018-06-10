using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for hlasko
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class hlasko : System.Web.Services.WebService
{

    mysql_db x2Mysql = new mysql_db();
    log x2log = new log();
    x2_var x2 = new x2_var();

    public hlasko()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    protected Dictionary<string, string> deserialize(string data)
    {
        JavaScriptSerializer js1 = new JavaScriptSerializer();
        return js1.Deserialize<Dictionary<string, string>>(data);
    }


    protected string serialize(Dictionary<string, string> data)
    {
        JavaScriptSerializer js2 = new JavaScriptSerializer();
        return js2.Serialize(data).ToString();
    }


    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }


    [WebMethod(EnableSession =true)]
    public string loadHlasenie(string data)
    {
        Dictionary<string, string> inData = this.deserialize(data);

        Dictionary<string, string> rtData = new Dictionary<string, string>();


        string query=string.Format(@"
        SELECT [is_hlasko.text] AS [text]
            -- [users1].[full_name] AS [creatUser], 
            -- [users2].[full_name] AS [lastUser] 
        FROM [is_hlasko]
        -- INNER JOIN [is_users] AS [users1] ON [users1].[id] = [is_hlasko].[creat_user]
        -- INNER JOIN [is_users] AS [users2] ON [users2].[id] = [is_hlasko].[last_user]
        WHERE [clinic]='{0}' 
            AND [dat_hlas]='{1}' 
            AND [type]='{2}'", Session["klinika_id"], inData["date"], inData["dep"]);

      
        query = x2Mysql.parseQuery(query);

        SortedList row = x2Mysql.getRow(query);

        if (row.ContainsKey("status"))
        {
            rtData.Add("status", "false");
            rtData.Add("result", row["msg"].ToString());
        }else
        {
            
            rtData.Add("status", "true");
            if (row.ContainsKey("text"))
            {
                rtData.Add("result", x2.DecryptString(row["text"].ToString(), Session["passphrase"].ToString()));
            }else
            {
                rtData.Add("result", "Nie je zapis");
            }

                
        }

        

        return this.serialize(rtData);
    }

}
