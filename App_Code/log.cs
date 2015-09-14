using System;
using System.Configuration;
using System.Web.Configuration;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Data;
using System.Security.Cryptography;

using System.Diagnostics;

//public static class Logger
//{

//    private static object locker = new object();

//    public static void Log(string source, string message)
//    {
//        lock (locker) {
//            string logFilePath = @"C:\LogFile\log.txt";

//            using (FileStream file = new FileStream(logFilePath,FileMode.Append,FileAccess.Write,FileShare.None) 
//            {
//                StreamWriter writer = new StreamWriter(file);

//                writer.write(source + ": : " + message);   
//                writer.Flush();

//                file.Close();
//            }
//        }
//    }

//}

/// <summary>
/// Summary description for log
/// </summary>
public class log
{
    public string serverPath = "";
//    public OdbcConnection my_con = new OdbcConnection();
    
	public log()
	{
        //this.ServerPath = path;
		//
		// TODO: Add constructor logic here
		//
    //    Configuration myConfig = WebConfigurationManager.OpenWebConfiguration("/is");
    //    ConnectionStringSettings connString;
    //    connString = myConfig.ConnectionStrings.ConnectionStrings["kdch_sk"];
    //    my_con.ConnectionString = connString.ToString();
       }

    private string Ip()
    {
        string ipAddress = "unknown";
        try
        {

            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {

                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                /* if (ipAddress == null || ipAddress == "unknown")
                 {
                     ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                 }*/
            }
            else
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        catch (Exception e)
        {

        }
        return ipAddress;
    }

    private string makeHashString(string text)
    {
        MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        byte[] data = enc.GetBytes(text);
        data = hasher.ComputeHash(data);
        //Convert.
        return Convert.ToBase64String(data).Replace('/', 'x');

    }

    private string unixDate(DateTime datum)
    {
        string mesiac = datum.Month.ToString();
        string den = datum.Day.ToString();
        string rok = datum.Year.ToString();

        return rok + "-" + mesiac + "-" + den;
    }

    private void sendMail(SortedList data)
    {
        MailAddress from = new MailAddress("kdch@kdch.sk");
        MailAddress to = new MailAddress("bduchaj@gmail.com");

        MailMessage sprava = new MailMessage();
        sprava.From = from;
        sprava.To.Add(to);
        sprava.Subject = "Error is.kdch.sk - " + DateTime.Now.ToLongDateString();
        sprava.IsBodyHtml = true;
        sprava.BodyEncoding = Encoding.UTF8;

        string regSprava = "";
        regSprava += "<h3>Error in is.kdch.sk - "+DateTime.Now.ToLongDateString()+"</h3>";
        regSprava += "<hr>";
        regSprava += "<p>"+data["stack"].ToString()+"</p>";
        regSprava += "<p>" + data["error"].ToString() + "</p>";
        regSprava += "<p>"+data["data"].ToString()+"</p>";

        sprava.Body = regSprava;
        SmtpClient mail_klient = new SmtpClient("mail3.webglobe.sk");
        try
        {
            mail_klient.Send(sprava);
        }
        catch (Exception e)
        {
            
        }


    }

    public void checkIfLogExists(string serverPath)
    {
       /* string serverPath = "";
        try
        {
        
            serverPath = HttpContext.Current.Server.MapPath("~");
        }
        catch (Exception e)
        {
             serverPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
        }*/

        DateTime dt = DateTime.Today;
        string shortDate = this.unixDate(dt);
        //string path = @"..\App_Data\";

        string complFile = serverPath+"\\App_Data\\"+shortDate+".log";

        if (!File.Exists(complFile))
        {
            File.Create(complFile,1,FileOptions.Asynchronous);
        }

        if (dt.Hour >= 22)
        {
            dt.AddDays(1);
            shortDate = this.unixDate(dt);
            complFile = serverPath + @"\App_Data\" + shortDate + ".log";

            if (!File.Exists(complFile))
            {
                File.Create(complFile,1,FileOptions.Asynchronous);
            }

        }
    }

    private SortedList openFile()
    {
        try
        {
            this.serverPath = System.Web.HttpContext.Current.Session["serverUrl"].ToString();
        }
        catch (Exception e)
        {
            this.serverPath = "end";
        }
      //  System.Web.HttpContext.Current.Server.
       /* try
        {
            serverPath = HttpContext.Current.Server.MapPath("~");
        }
        catch(Exception e)
        {
            serverPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            
        }*/
        StreamWriter sfw =null;
        string complFile = "";
        string fileName = "";

        if (serverPath != "end")
        {
            DateTime dt = DateTime.Today;

            string shortDate = this.unixDate(dt);
            //string path = @"..\App_Data\";

            complFile = this.serverPath + @"\App_Data\" + shortDate + ".log";
            fileName = shortDate + ".log";

            /*if (!File.Exists(complFile))
            {
                File.Create(complFile);
            } */

            int counter = 0;

            while (FileInUse(@complFile)&&counter<10)
            {
                System.Threading.Thread.Sleep(100);
                counter++;
            }
            if (counter<10)
            {
                sfw = new StreamWriter(@complFile, true);
            }
            else
            {
                sfw = null;
            }
        }

        SortedList result = new SortedList();
        result.Add("sfw", sfw);
        result.Add("fileName", fileName);

        return result;
    }

    public void logData(object data, string error, string idf)
    {
        string logIp = this.Ip();
        
        string dt = DateTime.Today.ToShortDateString();
        string dh = DateTime.Now.ToLongTimeString();
        StringBuilder sb = new StringBuilder();
        SortedList errorDt = new SortedList();

        Boolean sendMail = false;

        string strToWrite="";

        if (error.Length > 0)
        {
            sb.AppendFormat("ERROR   {0} {1} -- {2}, IP:{3} ERROR:\r\n {4} ", dt, dh, idf,logIp, error);
            sb.AppendFormat("Stack trace: {0} \r\n", Environment.StackTrace.ToString());
            sb.AppendLine("\r\n-----------------------------------------------------------END OF ERROR\r\n");

            sendMail = true;
            
            errorDt.Add("stack", Environment.StackTrace.ToString());
            errorDt.Add("error", error);
        }
        //sw.WriteLine(sb.ToString());
       // sb.Length = 0;
        if (data.GetType() == typeof(SortedList))
        {
            SortedList sl = (SortedList)data;
            sb.AppendFormat("{0} {1} -- {2} --IP:{3} ---- SortedList:\r\n",dt,dh,idf,logIp);
           
            foreach (DictionaryEntry row in sl)
            {
                sb.AppendFormat("       ['{0}'] = {1} \r\n", row.Key.ToString(), row.Value.ToString());
            }
            sb.AppendLine("\r\n-----------------------------------------------------------END OF  SortedList\r\n");
            
#if DEBUG
            strToWrite = sb.ToString();
#else
            strToWrite = sb.ToString();
            if (error.Length == 0)
            {
                if (strToWrite.Length > 500)
                {
                    strToWrite = strToWrite.Substring(0, 500);
                    strToWrite += ".....data truncated......";
                    strToWrite += "\r\n-----------------------------------------------------------END OF  SortedList\r\n";
                }
            }
#endif
            if (sendMail) errorDt.Add("data", strToWrite);
            
        }

        if (data.GetType() == typeof(Dictionary<int, Hashtable>))
        {
            Dictionary<int, Hashtable> table = (Dictionary<int, Hashtable>)data;
            sb.AppendFormat("{0} {1} -- {2} --IP:{3} -- Dictionary<int, Hashtable>:\r\n", dt, dh, idf,logIp);
            int cnt = table.Count;
            for (int row = 0; row < cnt; row++)
            {
                foreach (DictionaryEntry riad in table[row])
                {
                    sb.AppendFormat("        [{0}]['{1}'] = {2} \r\n", row, riad.Key.ToString(), riad.Value.ToString());
                }
            }
            sb.AppendLine("\r\n-----------------------------------------------------------END OF  Dictionary<int, Hashtable>\r\n");
            strToWrite = sb.ToString();
#if DEBUG
            strToWrite = sb.ToString();
#else
            strToWrite = sb.ToString();
            if (error.Length ==0)
           {
               if (strToWrite.Length > 500)
               {
                   strToWrite = strToWrite.Substring(0, 500);
                   strToWrite += ".....data truncated......";
                   strToWrite += "\r\n-----------------------------------------------------------END OF  Dictionary<int, Hashtable>\r\n";
               }
           }
#endif
            if (sendMail) errorDt.Add("data", strToWrite);
        }

        if (data.GetType() == typeof(Dictionary<int, SortedList>))
        {
            Dictionary<int, SortedList> table = (Dictionary<int, SortedList>)data;
            sb.AppendFormat("{0} {1} -- {2} -- IP:{3} -- Dictionary<int, SortedList>:\r\n", dt, dh, idf,logIp);
            int cnt = table.Count;
            for (int row = 0; row < cnt; row++)
            {
                foreach (DictionaryEntry riad in table[row])
                {
                    sb.AppendFormat("        [{0}]['{1}'] = {2} \r\n", row, riad.Key.ToString(), riad.Value.ToString());
                }
            }
            sb.AppendLine("\r\n-----------------------------------------------------------END OF  Dictionary<int, SortedList>\r\n");
            strToWrite = sb.ToString();
#if DEBUG
            strToWrite = sb.ToString();
#else
            strToWrite = sb.ToString();
            if (error.Length ==0 )
            {
                if (strToWrite.Length > 500)
                {
                    strToWrite = strToWrite.Substring(0, 500);
                    strToWrite += ".....data truncated......";
                    strToWrite += "\r\n-----------------------------------------------------------END OF  Dictionary<int, SortedList>\r\n";
                }
            }
#endif
            if (sendMail) errorDt.Add("data", strToWrite);
        }


        if (data.GetType() == typeof(string))
        {
            sb.AppendFormat("{0} {1} -- {2} -- IP:{3} -- string data:\r\n", dt, dh, idf,logIp); 
            sb.AppendFormat("        string = {0} \r\n", data.ToString());
            sb.AppendLine("\r\n-----------------------------------------------------------END OF  string\r\n");

#if DEBUG
            strToWrite = sb.ToString();
#else
            strToWrite = sb.ToString();
            if (error.Length ==0 )
            {
               if (strToWrite.Length > 500)
               {
                   strToWrite = strToWrite.Substring(0, 500);
                   strToWrite += ".....data truncated......";
                   strToWrite += "\r\n-----------------------------------------------------------END OF  string\r\n";
               }
            }
#endif
            {
		 
	    }

            if (sendMail) errorDt.Add("data", strToWrite);
        }

        StreamWriter sw = null;

        try
        {
            SortedList res = this.openFile();
            sw = (StreamWriter)res["sfw"];
            string fileName = res["fileName"].ToString();

            if (sw != null)
            {
               
                sw.WriteLine(strToWrite);
                sw.Flush();
                sw.Close();
                this.registerTempFile(fileName, 7);
                sw.Dispose();
                
                //reg.registerTempFile(sw.)
            }
            
            
           // if (sendMail) this.sendMail(errorDt);
            
        }
        catch (Exception ex)
        {
           if (this.serverPath != "end")
           {

               string edt = DateTime.Today.ToShortDateString();
               string edh = DateTime.Now.ToLongTimeString();

               string elogIp = this.Ip();

               DateTime eDt = DateTime.Today;

               string shortDate = this.unixDate(eDt);
               string fileName = this.serverPath+@"\App_Data\global_error_" + shortDate + ".log";
               string shortName = "global_error_" + shortDate + ".log";
               StringBuilder esb = new StringBuilder();
               esb.AppendFormat("\r\n Global error..........{0} {1} {2}", elogIp, edt, edh);
               esb.Append("_________________________________________________________________________\r\n");
               esb.AppendFormat("{0}\r\n", ex.ToString());
               esb.Append("_________________________________________________________________________\r\n");


               StreamWriter sfw = new StreamWriter(fileName, true);
               sfw.WriteLine(esb.ToString());
               sfw.Close();
               sfw.Dispose();
               //this.registerTempFile(shortName, 7);
               

           }
          
            //sw.Flush();
            //sw.Close();
        }
        
    }

    private void registerTempFile(string filename, Int32 days)
    {
        SortedList data = new SortedList();

        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        Int32 tolivetime = unixTimestamp + days * 24 * 60 * 60;


        data.Add("file_name", filename);
        data.Add("time_in", unixTimestamp);
        data.Add("time_out", tolivetime);
        data.Add("hash", this.makeHashString(filename));

        mysql_db db = new mysql_db();
        db.mysql_insert_log(data);
        
      
    }


	

    static bool FileInUse(string path)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
              
                if (fs.CanWrite)
                {
                    fs.Dispose();
                    fs.Close();
                }
            }
            return false;
        }
        catch (IOException ex)
        {
            return true;
        }
    }


  
   

}

