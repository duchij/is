using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class helpers_syncop : System.Web.UI.Page
{

    private mysql_db x2Mysql = new mysql_db();
    private log x2log = new log();
    private x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.loadData();
    }


    protected void loadData()
    {
        string url = @"http://is.kdch.sk/img/op.txt";
        string strResponse = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        StreamReader stIn = new StreamReader(response.GetResponseStream());
        strResponse = stIn.ReadToEnd();
        stIn.Close();

        byte[] data = Convert.FromBase64String(strResponse);
        string dataStr = Encoding.UTF8.GetString(data);

        string[] strArr = dataStr.Trim().Split(new string[] { "\r\n","\n"}, StringSplitOptions.None);

        if (strArr.Length >= 5)
        {
            SortedList insData = new SortedList();
            insData["kratka_sprava"] = strArr[0];
            insData["cela_sprava"] = strArr[1];
            insData["datum_txt"] = strArr[2];
            insData["user"] = strArr[3];
            insData["datum"] = x2.unixDate(Convert.ToDateTime(strArr[4]));

            SortedList result = x2Mysql.mysql_insert("is_opprogram", insData);
            x2log.logData(result, "", "sync op with kdch.sk");
        }


    }
}