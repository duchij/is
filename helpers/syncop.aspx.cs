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
    protected void Page_Load(object sender, EventArgs e)
    {
        this.loadData();
    }


    protected void loadData()
    {
        string url = "http://is.kdch.sk/App_Data/op.op";
        string strResponse = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        StreamReader stIn = new StreamReader(response.GetResponseStream());
        strResponse = stIn.ReadToEnd();
        stIn.Close();

        byte[] data = Convert.FromBase64String(strResponse);
        string dataStr = Encoding.UTF8.GetString(data);

        string[] strArr = dataStr.Split(new string[] { "\r\n","\n"}, StringSplitOptions.None);

        if (strArr.Length == 5)
        {
            SortedList insData = new SortedList();
        }


    }
}