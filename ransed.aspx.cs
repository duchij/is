using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;

public partial class ransed : System.Web.UI.Page
{
    x2_var my_x2 = new x2_var();
    mysql_db x2db = new mysql_db();

    protected void Page_Load(object sender, EventArgs e)
    {   
        this.setMyDate();
        this.loadSluzby();
    }

     protected void setMyDate()
    {
        DateTime tc = DateTime.Now;
        //DateTime datum = new DateTime();

        if (tc.Hour > 9)
        {
            Calendar1.SelectedDate = DateTime.Today;
            //datum = DateTime.Today;
        }
        else
        {
            Calendar1.SelectedDate = DateTime.Today.AddDays(-1);
            // datum = DateTime.Today.AddDays(-1);
        }
    }

    protected void loadSluzby()
    {
         DateTime datum = this.Calendar1.SelectedDate;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT GROUP_CONCAT([osirix] SEPARATOR ' ') AS [osirix] FROM [is_hlasko] WHERE [dat_hlas] = '{0}/{1}/{2}'", datum.Month, datum.Day, datum.Year);
        SortedList result = x2db.getRow(sb.ToString());

        string tmp = result["osirix"].ToString().Replace((char)13,' ');

        string[] str = tmp.Split(' ');

        for (int i = 0; i < str.Length; i++)
        {
            HyperLink meno_lnk = new HyperLink();
            meno_lnk.ID = "sluzba_" + i.ToString();
            meno_lnk.Text = "<li><strong>"+str[i].ToUpper()+" >>></strong></li>";
            meno_lnk.NavigateUrl = "http://10.10.2.49:3333/studyList?search=" + str[i];
            meno_lnk.Target = "_blank";
            this.sluzba_pl.Controls.Add(meno_lnk);
        }

    }

    protected void loadKojenci()
    {
        DateTime datum = this.Calendar1.SelectedDate;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT * FROM [is_osirix] WHERE [date] = '{0}' AND [odd] = '{1}'", my_x2.unixDate(datum), "KOJ");

        Dictionary<int, SortedList> result = x2db.getTable(sb.ToString());



    }
 

}
