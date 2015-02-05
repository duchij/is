using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Web;

/// <summary>
/// Summary description for log
/// </summary>
public class log
{
    x2_var x2 = new x2_var(); 
	public log()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private StreamWriter openFile()
    {
        string serverPath = HttpContext.Current.Server.MapPath("~");

        DateTime dt = DateTime.Today;
        
        string shortDate = x2.unixDate(dt);
        //string path = @"..\App_Data\";

        string complFile = serverPath+@"\App_Data\"+shortDate+".log";

        if (!File.Exists(complFile))
        {
            File.Create(complFile);
        }

        StreamWriter sfw = new StreamWriter(@complFile,true);
        

        return sfw;
    }

    public void logData(object data, string error, string idf)
    {

        
        string dt = DateTime.Today.ToShortDateString();
        string dh = DateTime.Now.ToLongTimeString();
        StringBuilder sb = new StringBuilder();
        if (error.Length > 0)
        {
            sb.AppendFormat("ERROR   {0} {1} -- {2} ERROR:\r\n {3} ", dt, dh, idf, error);
            sb.AppendFormat("Stack trace: {0} \r\n", Environment.StackTrace.ToString());
            sb.AppendLine("\r\n-----------------------------------------------------------END OF ERROR\r\n");
        }
        //sw.WriteLine(sb.ToString());
       // sb.Length = 0;
        if (data.GetType() == typeof(SortedList))
        {
            SortedList sl = (SortedList)data;
            sb.AppendFormat("{0} {1} -- {2} -- SortedList:\r\n",dt,dh,idf);
            foreach (DictionaryEntry row in sl)
            {
                sb.AppendFormat("       ['{0}'] = {1} \r\n", row.Key.ToString(), row.Value.ToString());
            }
            sb.AppendLine("\r\n-----------------------------------------------------------END OF  SortedList\r\n");
        }

        if (data.GetType() == typeof(Dictionary<int, Hashtable>))
        {
            Dictionary<int, Hashtable> table = (Dictionary<int, Hashtable>)data;
            sb.AppendFormat("{0} {1} -- {2} -- Dictionary<int, Hashtable>:\r\n", dt, dh, idf);
            int cnt = table.Count;
            for (int row = 0; row < cnt; row++)
            {
                foreach (DictionaryEntry riad in table[row])
                {
                    sb.AppendFormat("        [{0}]['{1}'] = {2} \r\n", row, riad.Key.ToString(), riad.Value.ToString());
                }
            }
            sb.AppendLine("\r\n-----------------------------------------------------------END OF  Dictionary<int, Hashtable>\r\n");
        }
        if (data.GetType() == typeof(string))
        {
            sb.AppendFormat("{0} {1} -- {2} -- string data:\r\n", dt, dh, idf); 
            sb.AppendFormat("        string = {0} \r\n", data.ToString());
            sb.AppendLine("\r\n-----------------------------------------------------------END OF  string\r\n");
        }


        StreamWriter sw = this.openFile();
        sw.WriteLine(sb.ToString());
        sw.Close();
    }

   

}