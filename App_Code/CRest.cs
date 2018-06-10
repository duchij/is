using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Net;
using System.Web.ClientServices;
using System.IO;

/// <summary>
/// Summary description for CRest
/// </summary>
public class CRest
{

    private x2_var x2 = new x2_var();
    private log Log = new log();

    public CRest()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public string _csCurl(string url, string method, Dictionary<string, string> data)
    {
        string result = null;
        switch (method)
        {
            case "POST":
                result = this._cPOST(url, data);
                break;
            case "GET_BIN":
                result = this._cGETBin(url, data);
                break;

            case "GET_TXT":
                result = this._cGETTxt(url, data);
                break;

            default:
                return "Error: no method provided";
                break;
        }

        return result;
    }

    private string _cGETTxt(string url, Dictionary<string, string> data)
    {
        string result = null;

        string args = this.dictionaryJoin(data);


        WebClient myClient = new WebClient();
        // myClient.
        foreach (var item in data)
        {
            myClient.Headers.Set(item.Key, item.Value);
        }


        try
        {

            Stream mStream = myClient.OpenRead(url);
            StreamReader sr = new StreamReader(mStream);
            result = sr.ReadToEnd();


        }
        catch (Exception ex)
        {
            result = "Error: " + ex.ToString();
        }


        return result;
    }

    private string _cGETBin(string url, Dictionary<string, string> data)
    {
        string result = null;

        string args = this.dictionaryJoin(data);


        WebClient myClient = new WebClient();
      // myClient.
        foreach (var item in data)
        {
            myClient.Headers.Set(item.Key, item.Value);
        }
        

        try
        {

           // string test = myClient.OpenRead(url);

            byte[] picData = myClient.DownloadData(url);

             result = Convert.ToBase64String(picData);

            //MemoryStream ms = new MemoryStream(picDdata);

          /*  Stream mStream = myClient.OpenRead(url);
            StreamReader sr = new StreamReader(mStream);
            
                
            result = sr.ReadToEnd();*/
                
            
        }catch (Exception ex)
        {

            Log.logData(ex.ToString(), "Error fetch data from Gallery", "CRest class");
            result = "Error: " + ex.ToString();
        }
       

        //myClient.OpenRead(url);
        return result;
        
    }


   


    private string _cPOST(string url, Dictionary<string, string> data)
    {
        string result = null;

        string args = this.dictionaryJoin(data);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = args.Length;

        using (Stream webStream = request.GetRequestStream())
        {

            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
            {
                requestWriter.Write(args);
            }
        }

        try
        {
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        result = responseReader.ReadToEnd();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result = "Error:" + ex.Message;
        }



        return result;
    }


    private string dictionaryJoin(Dictionary<string, string> data)
    {
        string result = null;

        foreach (var item in data)
        {
            result += string.Format("&{0}={1}", item.Key, item.Value);
        }


        return result;
    }
}