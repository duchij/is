using System;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class sluzby2 : System.Web.UI.Page
{
    public mysql_db x2Mysql = new mysql_db();
    public x2_var x2 = new x2_var();
    public sluzbyclass x2Sluzby = new sluzbyclass();

    public string  rights = "";

    protected void Page_Init(object sender, EventArgs e)
    {
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        this.rights = Session["rights"].ToString();

        if (IsPostBack == false)
        {
            this.setMonthYear();

           this.loadSluzby();
        }
        else
        {
           // string argument = Request["__EVENTARGUMENT"].ToString();

           // this.msg_lbl.Text ="he"+ argument;
            this.loadSluzby();

            /*ContentPlaceHolder ctpl = new ContentPlaceHolder();
            Control tmpControl = Master.FindControl("ContentPlaceHolder1");

            ctpl = (ContentPlaceHolder)tmpControl;
            Control controlList = ctpl.FindControl("listBox_0_0");

            DropDownList doctor_lb = (DropDownList)controlList;

            this.msg_lbl.Text = doctor_lb.SelectedValue.ToString();*/

           //
        }
    }

    protected void changeSluzba(object sender, EventArgs e)
    {
        this.loadSluzby();
    }

    protected void setMonthYear()
    {
        DateTime dnes = DateTime.Today;
        int mesiac = dnes.Month;
        int rok = dnes.Year;

        this.mesiac_cb.SelectedValue = mesiac.ToString();
        this.rok_cb.SelectedValue = rok.ToString();
    }



    protected void loadSluzby()
    {
        //this.shiftTable.Controls.Clear();

        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();
        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
        sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
        sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[full_name]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
        sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
        sb.Append("[t_sluzb].[date_group] AS [dategroup]");
        sb.Append("FROM [is_sluzby_2] AS [t_sluzb]");
        sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
        sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}{1}'",rok,mesiac);
        sb.Append("GROUP BY [t_sluzb].[datum]");
        sb.Append("ORDER BY [t_sluzb].[datum]");

        Dictionary<int, SortedList> table = x2Mysql.getTable(sb.ToString());


        if (table.Count > 0)
        {
            string[] header = table[0]["type1"].ToString().Split(';');

            int days = table.Count;
            int colsNum = header.Length;

            TableHeaderRow headRow = new TableHeaderRow();

            TableHeaderCell headCell = new TableHeaderCell();
            headCell.ID = "headCell_date";
            headCell.Text = "<strong>Datum</strong>";
           // headCell.Style
            headRow.Controls.Add(headCell);

            for (int head = 0; head < colsNum; head++)
            {
                TableHeaderCell headCell1 = new TableHeaderCell();
                headCell1.ID = "headCell_" + head;
                headCell1.Text = "<strong>"+ header[head].ToString()+"</strong>";
                headRow.Controls.Add(headCell1);
            }
            TableHeaderCell headCellSave = new TableHeaderCell();
            headCellSave.ID = "headCellSave_" + colsNum;
            headCellSave.Text = "<strong>Ulozit</strong>";
            headRow.Controls.Add(headCellSave);

            this.shiftTable.Controls.Add(headRow);

            string[] freeDays = x2Sluzby.getFreeDays();

            
            ArrayList doctorList = this.loadDoctors();

            int listLn = doctorList.Count;
            ListItem[] newItem = new ListItem[listLn];

            for (int doc = 0; doc < listLn; doc++)
            {
                string[] tmp = doctorList[doc].ToString().Split('|');
                newItem[doc] = new ListItem(tmp[1].ToString(), tmp[0].ToString());
            }

           

            
            for (int row = 0; row < days; row++)
            {
                TableRow tblRow = new TableRow();

                string[] names = table[row]["users_names"].ToString().Split(';');
                string[] userId = table[row]["users_ids"].ToString().Split('|');
                string[] comments = table[row]["comment"].ToString().Split('|');

                DateTime myDate = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), row + 1);
                int dnesJe = (int)myDate.DayOfWeek;
                string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                string sviatok = (row + 1).ToString() + "." + mesiac;
                int jeSviatok = Array.IndexOf(freeDays, sviatok);
          

                TableCell cellDate = new TableCell();
               // cellDate.ID = "cellDate_" + row;
                if (dnesJe == 0 || dnesJe == 6)
                {
                    cellDate.CssClass = "box red";
                }

                if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                {
                    cellDate.CssClass = "box yellow";
                }
                string text = (row + 1).ToString();
                cellDate.Text = text+". "+nazov;
                tblRow.Controls.Add(cellDate);
                for (int cols = 0; cols <= colsNum; cols++)
                {
                    TableCell dataCell = new TableCell();
                   // dataCell.ID = "dataCell_" + row.ToString() + "_" + cols.ToString();

                    if (cols < colsNum)
                    {
                       

                        if (this.rights == "admin" || this.rights == "poweruser")
                        {
                            DropDownList doctors_lb = new DropDownList();
                            doctors_lb.ID = "dlistBox_" + row.ToString() + "_" + cols.ToString();
                            doctors_lb.Items.AddRange(newItem);

                            doctors_lb.ID = "dlistBox_" + row.ToString() + "_" + cols.ToString();
                           
                           // doctors_lb.EnableViewState = true;
                            //doctors_lb.EnableViewState = true;

                            doctors_lb.SelectedValue = userId[cols];

                            dataCell.Controls.Add(doctors_lb);

                          
                            //doctors_lb.AutoPostBack = true;
                           // doctors_lb.SelectedIndexChanged += new EventHandler(pokus);
                            //doctors_lb.SelectedValue = "-";
                        }
                        else
                        {
                            Label name = new Label();
                            name.ID = "name_" + row.ToString() + "_" + cols.ToString();
                            name.Text = "<p>" + names[cols] + "</p>";
                            dataCell.Controls.Add(name);
                        }

                        Label comment = new Label();
                        comment.ID = "label_" + row.ToString() + "_" + cols.ToString();
                        comment.Text = comments[cols];
                        dataCell.Controls.Add(comment);


                        if (dnesJe == 0 || dnesJe == 6)
                        {
                            dataCell.CssClass = "box red";
                        }
                        if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                        {
                            dataCell.CssClass = "box yellow";
                        }

                        tblRow.Controls.Add(dataCell);
                    }
                    else
                    {
                        Button saveBtn = new Button();
                        saveBtn.ID="saveBtn_"+ row.ToString() + "_" + cols.ToString();
                        saveBtn.CssClass = "button green";
                        saveBtn.EnableViewState = true;
                       
                        saveBtn.Text = Resources.Resource.save;
                        saveBtn.Click += new EventHandler(saveShiftRow);
                        saveBtn.EnableViewState = true;
                        dataCell.Controls.Add(saveBtn);
                        tblRow.Controls.Add(dataCell);
                    

                    }
                    this.shiftTable.Controls.Add(tblRow);
                }

            }
        }
        
    }

    protected ArrayList loadDoctors()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT [id],[full_name] FROM [is_users] WHERE ([group] = 'users' OR [group] = 'poweruser') AND [active] = 1  ORDER BY [name2]");

        Dictionary<int, SortedList> table = x2Mysql.getTable(sb.ToString());

        int dataLn = table.Count;

        ArrayList result = new ArrayList();
       // result.Add("-", "-");
        result.Add("0|-");
        for (int i=1; i <= dataLn; i++)
        {
            result.Add(table[i-1]["id"].ToString()+"|"+table[i-1]["full_name"].ToString());
        }

        return result;

    }

    protected void pokus(object sender, EventArgs e)
    {
       // this.msg_lbl.Text = e.ToString();

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        Control controlList = ctpl.FindControl("listBox_0_");

        DropDownList doctor_lb = (DropDownList)controlList;

       // this.msg_lbl.Text = e.ToString() + "..." + doctor_lb.SelectedValue.ToString();

    }

    protected void saveShiftRow(object sender, EventArgs e)
    {
        Button saveBtn = (Button)sender;

        string idTmp = saveBtn.ID;

        string[] tmp = idTmp.Split('_');

        Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        Control controlList = ctpl.FindControl("dlistBox_0_1" );

        DropDownList doctor_lb = (DropDownList)controlList;

        string mtext = doctor_lb.SelectedValue.ToString();
       // doctor_lb.Text
      //  string lo = Request.Form("listBox_0_0").ToString();
        this.msg_lbl.Text = idTmp + " he:" + mtext + "<br>"+Request.Form.ToString();
       // this.msg_lbl.Text = Request.Form.ToString();

    }
    


}