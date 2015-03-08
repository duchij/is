using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class helpers_update : System.Web.UI.Page
{
    //private updateStatus;
    private mysql_db x2Mysql = new mysql_db();

    protected void Page_Init(object sender, EventArgs e)
    {
        /*if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }*/
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void doUpdateFnc(object sender, EventArgs e)
    {
        string serverUrl = Session["serverUrl"].ToString();
        string komplPath = serverUrl+@"\App_Data\update.lo";

        string passw = this.passwd_txt.Text.ToString();

        if (passw == "I3sKO")
        {

            if (File.Exists(komplPath))
            {
                this.info_lbl.Text = "Another Update in progress....WAIT TO END.........";
            }
            else
            {
                StreamWriter sw = new StreamWriter(komplPath);
                sw.Write("1");
                sw.Close();

                this._doUpdate();

            }
        }
        else
        {
            this.info_lbl.Text = "Wrong password.....";
        }
    }

    protected void _doUpdate()
    {
        Boolean result = false;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_settings] WHERE [name]='update_status'");

        SortedList res = x2Mysql.getRow(sb.ToString());
        if (res.Count > 0)
        {
            int updateNum = Convert.ToInt32(res["data"]);

            int nextUpdate = updateNum+1;

            string serverUrl = Session["serverUrl"].ToString();

            string fileName = serverUrl + @"\update\update_" + nextUpdate.ToString() + ".txt";

            if (File.Exists(fileName))
            {
                Boolean status = this._runUpdateFile(fileName);

                if (status)
                {
                    sb.Length = 0;
                    sb.AppendFormat("UPDATE [is_settings] SET [data] = '{0}' WHERE [name]='update_status'", nextUpdate);
                    SortedList tmpRes = x2Mysql.execute(sb.ToString());
                    if (Convert.ToBoolean(tmpRes["status"]))
                    {
                        this._doUpdate();
                    }
                    else
                    {
                        this.info_lbl.Text += "ERROR TO WRITE TO DB...... " + tmpRes["msg"].ToString();
                        result = false;
                    }
                    
                }
            }
            else
            {
                result = true;
            }
        }
        this._afterUpdate(result);        
    }

    protected void _afterUpdate(Boolean status)
    {
        if (status)
        {
            this.info_lbl.Text += "**************UPDATE SUCCESSFULL************** ";
            string serverUrl = Session["serverUrl"].ToString();
            string komplPath = serverUrl + @"\App_Data\update.lo";
            File.Delete(komplPath);

        }
        else
        {
            this.info_lbl.Text += "**************UPDATE FAILER************** ";
            string serverUrl = Session["serverUrl"].ToString();
            string komplPath = serverUrl + @"\App_Data\update.lo";
            File.Delete(komplPath);
        }
    }

    protected Boolean _runUpdateFile(string fileName)
    {
        Boolean result = false;
        StreamReader sr = new StreamReader(fileName);
        string content = sr.ReadToEnd();
        sr.Close();

        string[] queries = content.Split(new string[] { "--nextquery--" }, StringSplitOptions.None);

        int queryNum = queries.Length;

        if (queryNum == 0)
        {
            queries[0] = content;
        }

        SortedList res = x2Mysql.executeArr(queries);

        if (Convert.ToBoolean(res["status"]))
        {
            this.info_lbl.Text += "UPDATED.....<br>" + res["loging"].ToString()+ "<br>";
            result = true;
        }
        else
        {
            this.info_lbl.Text +="UPDATED OF "+fileName+"FAILED</br>";
            this.info_lbl.Text += res["log"].ToString()+"<br>";
            this.info_lbl.Text += res["msg"].ToString();
            this.info_lbl.Text +="..................................................";
            this.info_lbl.Text +="<br>CHECK UPDATE QUERIES AND DATABASE....<br>";
            result = false;
        }
        return result;
    }
}