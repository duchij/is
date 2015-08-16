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

public partial class controls_shifts_dk_shifts : System.Web.UI.UserControl
{
    public mysql_db x2Mysql = new mysql_db();
    public x2_var x2 = new x2_var();
    public sluzbyclass x2Sluzby = new sluzbyclass();
    log x2log = new log();
    public string rights = "";
    public string[] shiftType;
    public string wgroup = "";
    private string clinicIdf = "";

    protected void Page_Init(object sender, EventArgs e)
    {
         if (Session["tuisegumdrum"] == null)
        {
            Response.Redirect("error.html");
        }
    }
    

    protected void Page_Load(object sender, EventArgs e)
    {
       

        this.msg_lbl.Text = "";

        this.rights = Session["rights"].ToString();
        this.wgroup = Session["workgroup"].ToString();
        this.clinicIdf = Session["klinika"].ToString();
        //this.gKlinika = Session["klinika"].ToString().ToLower();

        this.initLabels();

        this.druhadk_pl.Visible = true;

        if (this.rights == "poweruser" || this.rights == "sadmin")
        {
            this.setup_btn.Visible = true;
            this.avaible_btn.Visible = true;
            this.weekState_tbl.Visible = true;
        }
        else
        {
            this.weekState_tbl.Visible = false;
            this.setup_btn.Visible = false;
            this.avaible_btn.Visible = false;
        }

        if (IsPostBack == false)
        {
            this.setMonthYear();
            

                string type = this.getShiftState();
                if (type == "active")
                {
                    this.edit_chk.Checked = false;
                }
                else
                {
                    this.edit_chk.Checked = true;
                }
                this.generateDKShiftTableForm();
                this.generateWeekStatus();
            
        }
        else
        {
                this.generateDKShiftTableForm();
                this.generateWeekStatus();
            
        }
    }

    protected ArrayList loadOmegaDoctors()
    {

        ArrayList result = new ArrayList();

        if (Session["active_doctors"] != null)
        {
            result = (ArrayList)Session["active_doctors"];
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            /*tc deaktivacia omegy ID budeme tahat
             * s tabulky is_users.... preto druhy sql
            sb.Append("SELECT [is_omega_doctors].[ms_item_id],[is_omega_doctors].[name],[is_clinics].[idf] AS [idf] FROM [is_omega_doctors] ");
            sb.AppendLine("INNER JOIN [is_clinics] ON [is_clinics].[id] = [is_omega_doctors].[clinic]");
            sb.AppendLine("WHERE [clinic]='4' ORDER BY [name]");*/

            // TODO: opravit a zuniverzalit tent select zatial je na presne IDecka co sa ale moze zmenit
            sb.Append("SELECT [is_users].[name3] AS [name], [is_users].[id] AS [users_id], [is_clinics].[idf] AS [idf] ");
            sb.AppendLine("FROM [is_users]");
            sb.AppendLine("INNER JOIN [is_clinics] ON [is_clinics].[id] = [is_users].[klinika]");
            sb.AppendFormat("WHERE [is_users].[klinika]='{0}' OR [is_users].[klinika]=5 OR [is_users].[klinika]=6 ORDER BY [is_users].[name3]", Session["klinika_id"]);

            Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());
            int dataLn = table.Count;
            result.Add("0|-");

            for (int i = 1; i <= dataLn; i++)
            {
                result.Add(table[i - 1]["users_id"].ToString() + "|" + table[i - 1]["name"].ToString() + " (" + table[i - 1]["idf"].ToString() + ")");
            }

            Session["active_doctors"] = result;
        }

        return result;

    }

    protected void initLabels()
    {
        this.editChk_lbl.Text = "Editovat";
        this.setup_btn.Text = x2.setLabel("2dk_shifts_setup");
        this.avaible_btn.Text = x2.setLabel("2dk_shifts_active");
    }

    protected void generateDKShiftTableForm()
    {
        this.shiftTable.Controls.Clear();

        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();
        int daysInMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));

        SortedList res = x2Mysql.getRow("SELECT * FROM [is_settings] WHERE [name] = '2dk_shift_doctors'");

        string[] shifts = res["data"].ToString().Split(',');
        this.shiftType = shifts;

        int colsNum = shifts.Length;
        string[] header = shifts;

        ArrayList doctors = this.loadOmegaDoctors();

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
            headCell1.Text = "<strong><center>" + x2.setLabel(header[head].ToString()) + "</center></strong>";
            // headCell1.
            headRow.Controls.Add(headCell1);
        }
        /* TableHeaderCell headCellSave = new TableHeaderCell();
         headCellSave.ID = "headCellSave_" + colsNum;
         headCellSave.Text = "<strong>Ulozit</strong>";
         headRow.Controls.Add(headCellSave);*/

        this.shiftTable.Controls.Add(headRow);

        string[] freeDays = Session["freedays"].ToString().Split(',');
        Boolean editSt = this.edit_chk.Checked;

        for (int row = 0; row < daysInMonth; row++)
        {
            TableRow tblRow = new TableRow();
            shiftTable.Controls.Add(tblRow);

            DateTime myDate = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), row + 1);
            int dnesJe = (int)myDate.DayOfWeek;
            string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
            string sviatok = (row + 1).ToString() + "." + myDate.Month.ToString();
            int jeSviatok = Array.IndexOf(freeDays, sviatok);

            TableCell cellDate = new TableCell();
            tblRow.Controls.Add(cellDate);
            // cellDate.ID = "cellDate_" + row;
            if (dnesJe == 0 || dnesJe == 6)
            {
                cellDate.CssClass = "box red";
            }

            if (jeSviatok != -1 && (dnesJe != 0 && dnesJe != 6))
            {
                cellDate.CssClass = "box yellow";
            }
            string text = (row + 1).ToString();
            cellDate.Text = text + ". " + nazov;

            string weekState = "konz";
            for (int cell = 0; cell < colsNum; cell++)
            {
                if (cell == 0)
                {
                    TableCell konzCell = new TableCell();
                    konzCell.ID = "konzWeek_" + (row + 1).ToString();

                    string tmp = konzCell.Text.ToString().Trim();
                    if (tmp.Length == 0) konzCell.Text = "konz";

                    if (konzCell.Text.ToString() == "konz") konzCell.CssClass = "info box";
                    if (konzCell.Text.ToString() == "prijm") konzCell.CssClass = "success box";


                    weekState = konzCell.Text.ToString();
                    tblRow.Controls.Add(konzCell);
                }


                TableCell dataCell = new TableCell();
                if ((dnesJe == 0 || dnesJe == 6) && jeSviatok != -1 ) //vikend + sviatok
                {
                    dataCell.CssClass = "box yellow";
                    if (cell == 1)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Odd1_" + (row + 1).ToString(), doctors);
                            dl.ToolTip = "Ates.lek./24h:";

                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Odd1_" + (row + 1).ToString());
                            txtDoc.ToolTip = "Ates.lek./24h.";
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB = this.makeCellComment("Odd1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);


                        if (editSt)
                        {
                            DropDownList dl1 = this.makeDoctorList("Odd2_" + (row + 1).ToString(), doctors);
                            dl1.ToolTip = "Prijm.tyzdem 24h \r\n Konz.tyzden 8h";
                            dataCell.Controls.Add(dl1);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Odd2_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB1 = this.makeCellComment("Odd2_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB1);

                        tblRow.Controls.Add(dataCell);

                    }
                    if (cell == 2)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupA1_" + (row + 1).ToString(), doctors);

                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupA1_" + (row + 1).ToString());

                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("OupA1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        if (editSt)
                        {
                            DropDownList dl1 = this.makeDoctorList("OupA2_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl1);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupA2_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB1 = this.makeCellComment("OupA2_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB1);

                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 3)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupB1_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupB1_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB = this.makeCellComment("OupB1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 4)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Expe_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Expe_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("Expe_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 5)
                    {
                        tblRow.Controls.Add(dataCell);
                    }
                }


                if ((dnesJe == 0 || dnesJe == 6) && jeSviatok == -1)  //vikend
                {
                    dataCell.CssClass = "box red";
                    if (cell == 1)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Odd1_" + (row + 1).ToString(), doctors);
                            dl.ToolTip = "Ates.lek./24h:";

                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Odd1_" + (row + 1).ToString());
                            txtDoc.ToolTip = "Ates.lek./24h.";
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB = this.makeCellComment("Odd1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);


                        if (editSt)
                        {
                            DropDownList dl1 = this.makeDoctorList("Odd2_" + (row + 1).ToString(), doctors);
                            dl1.ToolTip = "Prijm.tyzdem 24h \r\n Konz.tyzden 8h";
                            dataCell.Controls.Add(dl1);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Odd2_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB1 = this.makeCellComment("Odd2_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB1);

                        tblRow.Controls.Add(dataCell);

                    }
                    if (cell == 2)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupA1_" + (row + 1).ToString(), doctors);

                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupA1_" + (row + 1).ToString());

                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("OupA1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        if (editSt)
                        {
                            DropDownList dl1 = this.makeDoctorList("OupA2_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl1);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupA2_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB1 = this.makeCellComment("OupA2_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB1);

                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 3)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupB1_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupB1_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB = this.makeCellComment("OupB1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 4)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Expe_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Expe_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("Expe_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 5)
                    {
                        tblRow.Controls.Add(dataCell);
                    }
                }
                if (jeSviatok != -1 && (dnesJe != 0 && dnesJe != 6)) //je sviatok
                {
                    dataCell.CssClass = "box yellow";

                    if (cell == 1)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Odd1_" + (row + 1).ToString(), doctors);
                            dl.ToolTip = "Atest./24h";

                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Odd1_" + (row + 1).ToString());
                            txtDoc.ToolTip = "Ates./24h";
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("Odd1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        if (editSt)
                        {
                            DropDownList dl1 = this.makeDoctorList("Odd2_" + (row + 1).ToString(), doctors);
                            dl1.ToolTip = "Prijm.tyzden 24h \r\n Konz.tyzden 8h";
                            dataCell.Controls.Add(dl1);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Odd2_" + (row + 1).ToString());
                            txtDoc.ToolTip = "Prijm.tyzden 24h \r\n Konz.tyzden 8h";

                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB1 = this.makeCellComment("Odd2_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB1);

                        tblRow.Controls.Add(dataCell);

                    }
                    if (cell == 2)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupA1_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupA1_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }
                        TextBox txtB = this.makeCellComment("OupA1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        if (editSt)
                        {
                            DropDownList dl1 = this.makeDoctorList("OupA2_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl1);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupA2_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB1 = this.makeCellComment("OupA2_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB1);

                        tblRow.Controls.Add(dataCell); ;

                    }
                    if (cell == 3)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupB1_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupB1_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("OupB1_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 4)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Expe_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Expe_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("Expe_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }

                    if (cell == 5)
                    {
                        tblRow.Controls.Add(dataCell);
                    }
                }

                if ((dnesJe != 0 && dnesJe != 6) && jeSviatok == -1) //normDen
                {
                    //dataCell.ID = "Odd1_" + (row + 1).ToString();
                    if (cell == 1)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Odd_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Odd_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("Odd_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }

                    if (cell == 2)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupA_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupA_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }


                        TextBox txtB = this.makeCellComment("OupA_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);


                        tblRow.Controls.Add(dataCell);
                    }

                    if (cell == 3)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("OupB_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("OupB_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB = this.makeCellComment("OupB_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);

                        tblRow.Controls.Add(dataCell);
                    }

                    if (cell == 4)
                    {
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("Expe_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("Expe_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB = this.makeCellComment("Expe_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);


                        tblRow.Controls.Add(dataCell);
                    }
                    if (cell == 5)
                    {
                        tblRow.Controls.Add(dataCell);
                        if (editSt)
                        {
                            DropDownList dl = this.makeDoctorList("KlAmb_" + (row + 1).ToString(), doctors);
                            dataCell.Controls.Add(dl);
                        }
                        else
                        {
                            Label txtDoc = this.makeDoctorText("KlAmb_" + (row + 1).ToString());
                            dataCell.Controls.Add(txtDoc);
                        }

                        TextBox txtB = this.makeCellComment("KlAmb_txt_" + (row + 1).ToString());
                        dataCell.Controls.Add(txtB);


                        tblRow.Controls.Add(dataCell);
                    }
                }
            }
        }

        //if (!IsPostBack)
        //{
        this.setShiftsDK();
        // }
    }


    protected Label makeDoctorText(string ID)
    {
        Label txtBox = new Label();
        txtBox.ID = ID;
        txtBox.Text = "-";
        txtBox.EnableViewState = false;
        return txtBox;
    }

    protected DropDownList makeDoctorList(string ID, ArrayList doctors)
    {
        DropDownList dl = new DropDownList();

        dl.ID = ID;
        dl.SelectedIndexChanged += new EventHandler(dlItemChangedDK);
        dl.AutoPostBack = true;
        dl.EnableViewState = false;
        ListItem[] item = new ListItem[doctors.Count];

        for (int doc = 0; doc < doctors.Count; doc++)
        {
            string[] tmp = doctors[doc].ToString().Split('|');
            item[doc] = new ListItem(tmp[1].ToString(), tmp[0].ToString());
        }

        dl.Items.AddRange(item);

        return dl;
    }

    protected TextBox makeCellComment(string ID)
    {
        TextBox comment_txt = new TextBox();
        comment_txt.ID = ID;
        comment_txt.AutoPostBack = true;
        comment_txt.TextChanged += new EventHandler(commentChangedDK);
        comment_txt.Text = "-";
        comment_txt.BackColor = System.Drawing.Color.LightGray;

        return comment_txt;
    }

    protected void commentChangedDK(object sender, EventArgs e)
    {
        TextBox txtB = (TextBox)sender;
        string[] txtId = txtB.ID.ToString().Split('_');

        string comment = txtB.Text.ToString();

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        string datum = rok.ToString() + "-" + mesiac.ToString() + "-" + txtId[2].ToString();

        SortedList data = new SortedList();
        data.Add("datum", datum);
        data.Add("typ", txtId[0]);
        data.Add("comment", comment);
        data.Add("clinic", 4);

        SortedList res = x2Mysql.mysql_insert("is_sluzby_dk", data);


    }

    protected void dlItemChangedDK(object sender, EventArgs e)
    {
        DropDownList dl = (DropDownList)sender;

        dl.ToolTip = dl.SelectedItem.ToString();

        string[] dlId = dl.ID.ToString().Split('_');

        int userId = Convert.ToInt32(dl.SelectedValue);



        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        string datum = rok.ToString() + "-" + mesiac + "-" + dlId[1].ToString();
        string shiftAv = "";

        if (this.rights.IndexOf("admin") == -1 && this.rights != "poweruser")
        {
            shiftAv = this.shiftAvaibalityDK(dlId[0], datum);
        }


        if (shiftAv.Length == 0 || shiftAv == userId.ToString())
        {
            SortedList data = new SortedList();
            data.Add("datum", datum);
            data.Add("typ", dlId[0]);
            if (userId == 0)
            {
                data.Add("user_id", null);
            }
            else
            {
                data.Add("user_id", userId);
            }
            data.Add("clinic", Session["klinika_id"]);
            //data.Add("clinic", 4);
            //data.Add("date_group", x2.makeDateGroup(rok, mesiac));

            SortedList res = x2Mysql.mysql_insert("is_sluzby_dk", data);
        }
        else
        {
            this.msg_lbl.Text = x2.setLabel("2dk_shifts_notavaible");

            ArrayList doctors = this.loadOmegaDoctors();
            dl.Items.Clear();

            for (int doc = 0; doc < doctors.Count; doc++)
            {
                string[] docArr = doctors[doc].ToString().Split('|');
                if (docArr[0] == shiftAv)
                {
                    dl.Items.Add(new ListItem(docArr[1], docArr[0]));
                    dl.SelectedValue = shiftAv;
                    dl.ToolTip = docArr[1].ToString();
                    dl.Enabled = false;
                    break;
                }
            }
        }
    }

    protected string shiftAvaibalityDK(string typ, string datum)
    {
        string result = "";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [user_id] FROM [is_sluzby_dk] WHERE [typ]='{0}' AND [datum]='{1}' AND [clinic]='{2}'", typ, datum, Session["klinika_id"].ToString());

        SortedList row = x2Mysql.getRow(sb.ToString());

        string tmp = x2.getStr(row["user_id"].ToString());

        if (tmp.Length > 0)
        {
            result = tmp;
        }

        return result;
    }
    

    protected void setShiftsDK()
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        Boolean chk_st = this.edit_chk.Checked;
        int dateGroup = x2.makeDateGroup(rok, mesiac);

        StringBuilder sb = new StringBuilder();

        //sb.AppendLine("SELECT [is_sluzby_dk].*, [is_omega_doctors].[name] FROM [is_sluzby_dk]");
        //sb.AppendLine("LEFT JOIN [is_omega_doctors] ON [is_omega_doctors].[ms_item_id] = [is_sluzby_dk].[user_id] ");
        //spojenie s is_users
        sb.AppendLine("SELECT [is_sluzby_dk].*, [is_users].[name3] AS [name] FROM [is_sluzby_dk]");
        sb.AppendLine("LEFT JOIN [is_users] ON [is_users].[id] = [is_sluzby_dk].[user_id] ");
        sb.AppendFormat("WHERE [is_sluzby_dk].[date_group] ='{0}' AND [is_sluzby_dk].[clinic]='{1}' ORDER BY [is_sluzby_dk].[datum] ASC", dateGroup, Session["klinika_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int tableCn = table.Count;

        if (tableCn > 0 && table[0]["state"].ToString() != "setup")
        {
            this.shiftTable.Visible = true;
           // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
           // ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

            for (int doc = 0; doc < tableCn; doc++)
            {
                string type = table[doc]["typ"].ToString();

                DateTime dt = Convert.ToDateTime(x2.UnixToMsDateTime(table[doc]["datum"].ToString()));
                string userId = table[doc]["user_id"].ToString();
                //string loggedUser = Session["omega_ms_item_id"].ToString();
                string loggedUser = Session["user_id"].ToString();
                if (userId == "NULL") userId = "0";

                if (chk_st)
                {
                    Control cl = FindControl(type + "_" + dt.Day.ToString());
                    DropDownList dl = (DropDownList)cl;

                    if (this.rights == "users")
                    {
                        if (userId != loggedUser && userId != "0")
                        {
                            dl.Enabled = false;
                        }
                        //if (userId == "0" || userId == Session["omega_ms_item_id"].ToString())
                        if (userId == "0" || userId == Session["user_id"].ToString())
                        {

                            // ListItem data = dl.Items.FindByValue(Session["omega_ms_item_id"].ToString());
                            ListItem data = dl.Items.FindByValue(Session["user_id"].ToString());
                            dl.Items.Clear();
                            dl.Items.Add(new ListItem("-", "0"));
                            dl.Items.Add(data);
                        }
                    }

                    if (dl != null)
                    {
                        dl.SelectedValue = userId;
                        dl.ToolTip += "\r\n" + dl.SelectedItem.ToString();
                    }
                }
                else
                {
                    Control tcl = FindControl(type + "_" + dt.Day.ToString());
                    Label txtDocName = (Label)tcl;

                    // this.shiftTable.Controls.Remove(dl);

                    txtDocName.Text = "<p>" + x2.getStr(table[doc]["name"].ToString()) + "</p>";
                }



                string comment = table[doc]["comment"].ToString().Trim();
                Control tCl = FindControl(type + "_" + "txt_" + dt.Day.ToString());
                TextBox txtB = (TextBox)tCl;
                if ((comment.Length == 0 || comment == "-") && this.edit_chk.Checked == false)
                {
                    txtB.Visible = false;
                }
                else
                {
                    if (txtB != null)
                    {
                        txtB.Visible = true;
                        txtB.Text = comment;
                    }
                }
            }


        }
        else
        {
            if (tableCn == 0)
            {
                //int dateGroup = x2.makeDateGroup(rok,mesiac);
                int days = DateTime.DaysInMonth(rok, mesiac);
                int res = x2Mysql.fillDKShifts(dateGroup, rok, mesiac, days, Convert.ToInt32(Session["klinika_id"]));
                x2log.logData(res.ToString(), "", "fill_dk_shifts");

                if (res > 0)
                {
                    this.generateDKShiftTableForm();
                    this.generateWeekStatus();
                }
            }
            else if (table[0]["state"].ToString() == "setup")
            {
                this.msg_lbl.Text = Resources.Resource.shifts_not_done;
                this.shiftTable.Visible = false;
            }
        }


    }

    protected int getFirstMonday(int rok, int mesiac)
    {
        int firstM = 1;
        for (int den = 1; den <= 7; den++)
        {
            DateTime dt = new DateTime(rok, mesiac, den);
            if (dt.DayOfWeek == DayOfWeek.Monday)
            {
                firstM = den;
                break;
            }
        }
        return firstM;
    }

    protected void generateWeekStatus()
    {
        this.weekState_tbl.Controls.Clear();
        this.tempWeek_0.Value = "";
        this.tempWeek_1.Value = "";
        this.tempWeek_2.Value = "";
        this.tempWeek_3.Value = "";
        this.tempWeek_4.Value = "";
        this.tempWeek_5.Value = "";

        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        int days = DateTime.DaysInMonth(rok, mesiac);

        TableHeaderRow headRow = new TableHeaderRow();
        this.weekState_tbl.Controls.Add(headRow);
        int startDay = this.getFirstMonday(rok, mesiac);
        int endDay = startDay + 6;

       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
       // ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        for (int week = 0; week <= 5; week++)
        {

            TableHeaderCell headCell = new TableHeaderCell();
            Control tmpC = FindControl("tempWeek_" + week.ToString());

            HiddenField tempWeek = (HiddenField)tmpC;

            // SortedList weekStatus = new SortedList();

            if (week < 5)
            {
                if (week == 0)
                {
                    if (startDay > 1)
                    {
                        int eTmp = startDay - 1;
                        headCell.Text = "1. - " + eTmp.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                        tempWeek.Value = "1_" + eTmp.ToString();
                    }
                    else
                    {
                        headCell.Text = startDay.ToString() + ". - " + endDay.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                        tempWeek.Value = startDay.ToString() + "_" + endDay.ToString();
                        endDay = endDay + 7;
                        startDay = startDay + 7;
                    }
                }

                if (week > 0)
                {
                    if (endDay > days) endDay = days;
                    headCell.Text = startDay.ToString() + ". - " + endDay.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                    tempWeek.Value = startDay.ToString() + "_" + endDay.ToString();
                    endDay = endDay + 7;
                    // if (endDay > days) endDay = days;
                    startDay = startDay + 7;

                }
            }
            if (week == 5)
            {
                if (startDay < days)
                {
                    startDay = endDay - 6;
                    //int lendDay = startDay + 6;
                    headCell.Text = startDay.ToString() + ". - " + days.ToString() + ". " + mesiac.ToString() + ". " + rok.ToString();
                    tempWeek.Value = startDay.ToString() + "_" + days.ToString();
                }
                // weekStatus["week_" + week.ToString()] = tempWeek.Value.ToString();
            }
            headRow.Controls.Add(headCell);

            // startDay = endDay + 1;
        }
        TableRow riadok = new TableRow();
        this.weekState_tbl.Controls.Add(riadok);

        for (int week = 0; week <= 5; week++)
        {
            Control cl = FindControl("tempWeek_" + week.ToString());


            HiddenField tempData = (HiddenField)cl;
            string[] hdValue = tempData.Value.ToString().Split('_');
            if (cl != null && hdValue.Length == 2)
            {
                string startD = rok.ToString() + "-" + mesiac.ToString() + "-" + hdValue[0];
                string endD = rok.ToString() + "-" + mesiac.ToString() + "-" + hdValue[1];

                TableCell cellData = new TableCell();

                DropDownList dl = new DropDownList();
                dl.ID = "week_" + week.ToString() + "_dl";
                dl.Items.Add(new ListItem("Konziliarny", "konz"));
                dl.Items.Add(new ListItem("Prijmovy", "prijm"));

                dl.SelectedIndexChanged += new EventHandler(setWeekStatus);
                dl.AutoPostBack = true;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT [tyzden] FROM [is_sluzby_dk] WHERE [datum] BETWEEN '{0}' AND '{1}' GROUP BY [tyzden]", startD, endD);

                SortedList row = x2Mysql.getRow(sb.ToString());
                if (row["status"] == null)
                {
                    if (row.Count > 0)
                    {
                        dl.SelectedValue = row["tyzden"].ToString();

                        if (row["tyzden"].ToString() == "konz")
                        {
                            dl.BackColor = System.Drawing.Color.FromArgb(0xd9edf7);
                        }
                        else
                        {
                            dl.BackColor = System.Drawing.Color.FromArgb(0xdff0d8);
                        }

                        int start = Convert.ToInt32(hdValue[0]);
                        int end = Convert.ToInt32(hdValue[1]);

                        for (int iWeek = start; iWeek <= end; iWeek++)
                        {
                            Control txtCl = FindControl("konzWeek_" + iWeek.ToString());
                            TableCell txtB = (TableCell)txtCl;

                            txtB.Text = row["tyzden"].ToString();

                            if (this.rights != "poweruser" && this.rights.IndexOf("admin") == -1)
                            {
                                if (this.edit_chk.Checked == true) this.blockDayInShiftDK(rok, mesiac, iWeek, row["tyzden"].ToString());
                            }


                            if (row["tyzden"].ToString() == "konz")
                            {
                                txtB.CssClass = "info box";

                            }
                            else
                            {
                                txtB.CssClass = "success box";
                            }
                        }
                    }
                }
                cellData.Controls.Add(dl);
                riadok.Controls.Add(cellData);
            }

        }

    }

    protected void blockDayInShiftDK(int rok, int mesiac, int den, string week)
    {
        DateTime dt = new DateTime(rok, mesiac, den);

        string[] freeDays = Session["freedays"].ToString().Split(',');

        int dayOfWeek = (int)dt.DayOfWeek;

       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        int sviatok = Array.IndexOf(freeDays, den + "." + mesiac);
        Boolean vikend = false;

        if (dayOfWeek == 6 || dayOfWeek == 0) vikend = true;


        if (sviatok != -1 || vikend)
        {
            if (week == "konz")
            {
                /*Control cl1 = ctpl.FindControl("Odd1_" + den);
                DropDownList dl1 = (DropDownList)cl1;
                dl1.Enabled = false;

                Control cl2 = ctpl.FindControl("Odd2_" + den);
                DropDownList dl2 = (DropDownList)cl2;
                dl2.Enabled = false;*/

                Control cl3 = FindControl("OupA1_" + den);
                DropDownList dl3 = (DropDownList)cl3;
                dl3.Enabled = false;

                Control cl4 = FindControl("OupA2_" + den);
                DropDownList dl4 = (DropDownList)cl4;
                dl4.Enabled = false;

                Control cl5 = FindControl("OupB1_" + den);
                DropDownList dl5 = (DropDownList)cl5;
                dl5.Enabled = false;
            }
            if (week == "prijm")
            {
                Control cl1 = FindControl("Expe_" + den);
                DropDownList dl1 = (DropDownList)cl1;
                dl1.Enabled = false;
            }
        }

        if (sviatok == -1 && !vikend)
        {
            if (week == "konz")
            {
                if (dt.DayOfWeek == DayOfWeek.Monday || dt.DayOfWeek == DayOfWeek.Wednesday || dt.DayOfWeek == DayOfWeek.Friday)
                {
                    Control cl1 = FindControl("OupA_" + den);
                    DropDownList dl1 = (DropDownList)cl1;
                    dl1.Enabled = false;

                    Control cl2 = FindControl("OupB_" + den);
                    DropDownList dl2 = (DropDownList)cl2;
                    dl2.Enabled = false;
                }
                else
                {
                    Control cl3 = FindControl("Expe_" + den);
                    DropDownList dl3 = (DropDownList)cl3;
                    dl3.Enabled = false;
                }

            }
            if (week == "prijm")
            {
                if (dt.DayOfWeek == DayOfWeek.Tuesday || dt.DayOfWeek == DayOfWeek.Thursday)
                {
                    Control cl1 = FindControl("OupA_" + den);
                    DropDownList dl1 = (DropDownList)cl1;
                    dl1.Enabled = false;

                    Control cl2 = FindControl("OupB_" + den);
                    DropDownList dl2 = (DropDownList)cl2;
                    dl2.Enabled = false;
                }
                else
                {
                    Control cl3 = FindControl("Expe_" + den);
                    DropDownList dl3 = (DropDownList)cl3;
                    dl3.Enabled = false;
                }
            }
        }
    }

    protected void setWeekStatus(object sender, EventArgs e)
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);
        DropDownList dl = (DropDownList)sender;
        string[] id = dl.ID.ToString().Split('_');

        string week = dl.SelectedValue.ToString();
        if (week == "konz")
        {
            dl.BackColor = System.Drawing.Color.FromArgb(0xd9edf7);
        }
        else
        {
            dl.BackColor = System.Drawing.Color.FromArgb(0xdff0d8);
        }

       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");
       // ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        Control cl = FindControl("tempWeek_" + id[1].ToString());

        HiddenField tempData = (HiddenField)cl;



        string[] hdValue = tempData.Value.ToString().Split('_');
        if (cl != null & hdValue.Length == 2)
        {

            string startD = rok.ToString() + "-" + mesiac.ToString() + "-" + hdValue[0];
            string endD = rok.ToString() + "-" + mesiac.ToString() + "-" + hdValue[1];
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE [is_sluzby_dk] SET [tyzden]='{0}' WHERE [datum] BETWEEN '{1}' AND '{2}' ", week, startD, endD);

            SortedList res = x2Mysql.execute(sb.ToString());
            if (Convert.ToBoolean(res["status"]))
            {

                int start = Convert.ToInt32(hdValue[0]);
                int end = Convert.ToInt32(hdValue[1]);

                for (int iWeek = start; iWeek <= end; iWeek++)
                {
                    Control txtCl = FindControl("konzWeek_" + iWeek.ToString());
                    TableCell txtB = (TableCell)txtCl;

                    txtB.Text = week;

                    if (week == "prijm") txtB.CssClass = "success box";
                    if (week == "konz") txtB.CssClass = "info box";
                }
            }
        }

    }

    protected string getShiftState()
    {
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        int dateGroup = x2.makeDateGroup(rok, mesiac);

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [state] FROM [is_sluzby_dk] WHERE [date_group]='{0}' AND [clinic]='{1}' GROUP BY [state]", dateGroup, Session["klinika_id"]);

        SortedList row = x2Mysql.getRow(sb.ToString());
        if (row.Count == 0)
        {
            row["state"] = "setup";
        }

        return row["state"].ToString();

    }

    protected void makeShiftsActiveDKFnc(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        int dateGroup = x2.makeDateGroup(rok, mesiac);

        sb.AppendFormat("UPDATE [is_sluzby_dk] SET [state]='active' WHERE [state] ='draft' AND [date_group] ='{0}' AND [clinic]='{1}'", dateGroup, Session["klinika_id"]);

        SortedList res = x2Mysql.execute(sb.ToString());

        if (Convert.ToBoolean(res["status"]))
        {
            this.edit_chk.Checked = false;
            this.generateDKShiftTableForm();

        }
    }

    protected void makeShiftsDraftDKFnc(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        int mesiac = Convert.ToInt32(this.mesiac_cb.SelectedValue);
        int rok = Convert.ToInt32(this.rok_cb.SelectedValue);

        int dateGroup = x2.makeDateGroup(rok, mesiac);

        sb.AppendFormat("UPDATE [is_sluzby_dk] SET [state]='draft' WHERE [state] ='setup' AND [date_group] ='{0}' AND [clinic]='{1}'", dateGroup, Session["klinika_id"]);

        SortedList res = x2Mysql.execute(sb.ToString());

        if (Convert.ToBoolean(res["status"]))
        {
            this.shiftTable.Visible = true;
        }

    }

    protected void changeSluzba(object sender, EventArgs e)
    {
        this.generateDKShiftTableForm();
        this.generateWeekStatus();
    }

    protected void setMonthYear()
    {
        DateTime dnes = DateTime.Today;
        int mesiac = dnes.Month;
        int rok = dnes.Year;

        this.mesiac_cb.SelectedValue = mesiac.ToString();
        this.rok_cb.SelectedValue = rok.ToString();
    }
   

   /* protected void generateShiftTable()
    {
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        int daysMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        this.days_lbl.Text = daysMonth.ToString();

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }

        string dateGroup = rok + mesiac;
        Session.Add("aktDateGroup", dateGroup);
        string query = x2.sprintf("SELECT * FROM [is_settings] WHERE [name] = '{0}_shift_doctors'", new string[] { this.clinicIdf });
        SortedList res = x2Mysql.getRow(query);

        // Boolean status = Convert.ToBoolean(res["status"].ToString());

        string[] shifts = res["data"].ToString().Split(',');
        this.shiftType = shifts;


       
        if (this.rights.IndexOf("admin") != -1 || this.rights == "poweruser")
        {
            string state = this.getShiftStateKDCH();

            if (state == "active")
            {
                this.shiftState_lbl.Text = Resources.Resource.shifts_see_all;
                //this.publish_cb.ch
            }
            else
            {
                this.shiftState_lbl.Text = Resources.Resource.shifts_see_limited;
            }
        }

        string[] header = shifts;

            int days = daysMonth;
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
                headCell1.Text = "<strong><center>" + x2.setLabel(header[head].ToString()) + "</center></strong>";
                // headCell1.
                headRow.Controls.Add(headCell1);
            }
            

            shiftTable.Controls.Add(headRow);

            string[] freeDays = x2Sluzby.getFreeDays();
            ArrayList doctorList = this.loadDoctors();

            int aktDenMesiac = DateTime.Today.Day;


            for (int row = 0; row < days; row++)
            {
                TableRow tblRow = new TableRow();
                shiftTable.Controls.Add(tblRow);


                DateTime myDate = new DateTime(Convert.ToInt32(rok), Convert.ToInt32(mesiac), row + 1);
                int dnesJe = (int)myDate.DayOfWeek;
                string nazov = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[dnesJe];
                string sviatok = (row + 1).ToString() + "." + myDate.Month.ToString();
                int jeSviatok = Array.IndexOf(freeDays, sviatok);



                TableCell cellDate = new TableCell();
                tblRow.Controls.Add(cellDate);
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
                cellDate.Text = text + ". " + nazov;


                for (int cols = 0; cols < colsNum; cols++)
                {
                    TableCell dataCell = new TableCell();
                    // tblRow.Controls.Add(dataCell);
                    // dataCell.ID = "dataCell_" + row.ToString() + "_" + cols.ToString();

                    // if (cols < colsNum)
                    //  {


                    if ((this.rights.IndexOf("admin") != -1 || this.rights == "poweruser") && this.wgroup == "doctor" && this.edit_chk.Checked == true)
                    {
                        DropDownList doctors_lb = new DropDownList();
                        doctors_lb.ID = "ddl_" + row.ToString() + "_" + cols.ToString();
                        //doctors_lb.CssClass = "no-pad-mobile no-gap-mobile";

                        int listLn = doctorList.Count;
                        ListItem[] newItem = new ListItem[listLn];

                        for (int doc = 0; doc < listLn; doc++)
                        {
                            string[] tmp = doctorList[doc].ToString().Split('|');
                            newItem[doc] = new ListItem(tmp[1].ToString(), tmp[0].ToString());
                        }
                        doctors_lb.Items.AddRange(newItem);

                        //string dd = userId[cols].ToString();

                       // doctors_lb.SelectedValue = dd; // userId[cols].ToString();
                        dataCell.Controls.Add(doctors_lb);
                        //doctors_lb.SelectedIndex = doctors_lb.Items.IndexOf(doctors_lb.Items.FindByValue(userId[cols]));

                        doctors_lb.AutoPostBack = true;
                        doctors_lb.SelectedIndexChanged += new EventHandler(dItemChanged);
                        //doctors_lb.SelectedValue = "-";

                        TextBox textBox = new TextBox();
                        dataCell.Controls.Add(textBox);
                        textBox.ID = "textBox_" + row.ToString() + "_" + cols.ToString();
                       // textBox.Text = comments[cols];
                        textBox.AutoPostBack = true;
                        textBox.TextChanged += new EventHandler(commentChanged);
                    }
                    else
                    {
                        Label name = new Label();
                        dataCell.Controls.Add(name);
                        name.ID = "name_" + row.ToString() + "_" + cols.ToString();
                        //name.Text = names[cols] + "<br>";

                        Label comment = new Label();
                        dataCell.Controls.Add(comment);
                        comment.ID = "label_" + row.ToString() + "_" + cols.ToString();
                        comment.Font.Italic = true;
                        //comment.Text = comments[cols];

                    }
                    if (dnesJe == 0 || dnesJe == 6)
                    {
                        dataCell.CssClass = "box red";
                    }
                    if (jeSviatok != -1 && dnesJe != 0 && dnesJe != 6)
                    {
                        dataCell.CssClass = "box yellow";
                    }
                    if (aktDenMesiac == (row + 1))
                    {
                        dataCell.BorderColor = System.Drawing.Color.Red;
                        dataCell.BorderWidth = Unit.Point(5);
                    }
                    tblRow.Controls.Add(dataCell);
                }
            }
            this.loadSluzby();
    }*/

   /* protected void loadSluzby()
    {

        //this.shiftTable.Controls.Clear();
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        int daysMonth = DateTime.DaysInMonth(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        //this.days_lbl.Text = daysMonth.ToString();
        string query = x2.sprintf("SELECT * FROM [is_settings] WHERE [name] = '{0}_shift_doctors'", new string[] { this.clinicIdf });
        SortedList res = x2Mysql.getRow(query);
        StringBuilder sb = new StringBuilder();
        int dateGroup = x2.makeDateGroup(Convert.ToInt32(rok), Convert.ToInt32(mesiac));
        
        if ((this.rights.IndexOf("admin")!=-1 || this.rights == "poweruser") && this.wgroup == "doctor")
        {

            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_all] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}'",dateGroup );
            sb.AppendFormat("WHERE [t_sluzb].[clinic] = '{0}'", Session["klinika_id"]);
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum]");
        }
        else
        {
            sb.Append("SELECT [t_sluzb].[datum] , GROUP_CONCAT([typ] ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [type1],");
            sb.Append("[t_sluzb].[state] AS [state],");
            sb.Append("GROUP_CONCAT([t_sluzb].[user_id] ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [users_ids],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[user_id]=0,'-',[t_users].[name3]) ORDER BY [t_sluzb].[ordering] SEPARATOR ';') AS [users_names],");
            sb.Append("GROUP_CONCAT(IF([t_sluzb].[comment]=NULL,'-',[t_sluzb].[comment]) ORDER BY [t_sluzb].[ordering] SEPARATOR '|') AS [comment],");
            sb.Append("[t_sluzb].[date_group] AS [dategroup]");
            sb.Append("FROM [is_sluzby_all] AS [t_sluzb]");
            sb.Append("LEFT JOIN [is_users] AS [t_users] ON [t_users].[id] = [t_sluzb].[user_id]");
            sb.AppendFormat("WHERE [t_sluzb].[date_group] = '{0}' AND [t_sluzb].[state]='active'", dateGroup);
            sb.AppendFormat("WHERE [t_sluzb].[clinic] = '{0}'", Session["klinika_id"]);
            sb.Append("GROUP BY [t_sluzb].[datum]");
            sb.Append("ORDER BY [t_sluzb].[datum]");
        }

        //  this.msg_lbl.Text = sb.ToString();

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());
        int cols = this.shiftType.Length;
        if (table.Count == daysMonth)
        {
            for (int row = 0; row < daysMonth; row++)
            {
                //TableRow tblRow = new TableRow();
                //shiftTable.Controls.Add(tblRow);

                string[] names = table[row]["users_names"].ToString().Split(';');
                string[] userId = table[row]["users_ids"].ToString().Split('|');
                string[] comments = table[row]["comment"].ToString().Split('|');

                for (int col=0;col<cols;col++)
                {
                    if (this.edit_chk.Checked == true)
                    {
                        Control __textBox = FindControl("textBox_" + row + "_" + col);
                        Control __ddlBox = FindControl("ddl_" + row + "_" + col);

                        TextBox txtBox = (TextBox)__textBox;
                        DropDownList ddlBox = (DropDownList)__ddlBox;


                        txtBox.Text = comments[col].ToString();
                        ddlBox.SelectedValue = userId[col];
                    }
                    else
                    {
                        Control __comment = FindControl("label_" + row + "_" + col);
                        Control __name = FindControl("name_" + row + "_" + col);

                        Label name = (Label)__name;
                        Label comment = (Label)__comment;


                        name.Text = names[col]+"</br>";
                        comment.Text = comments[col];
                    }
                    
                }
            }
        }
        else
        {
            if (table.Count == 0)
            {
                if (this.rights.IndexOf("admin") == -1 || this.rights != "poweruser")
                {
                    this.msg_lbl.Text = Resources.Resource.shifts_not_done;
                    // this.publish_cb.Visible = false;
                }
                if ((this.rights.IndexOf("admin") != -1 || this.rights == "poweruser") && this.wgroup == "doctor")
                {
                    int daysTmp = x2Mysql.fillNkimShifts(Convert.ToInt32(dateGroup), Convert.ToInt32(daysMonth), Convert.ToInt32(mesiac), Convert.ToInt32(rok),Convert.ToInt32(Session["klinika_id"]));
                    this.shiftTable.Controls.Clear();
                    
                    this.loadSluzby();
                }
            }
        }
    }
    */
    protected ArrayList loadDoctors()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SELECT [id],[name3] FROM [is_users] WHERE ([work_group]='doctor') AND [active]='1' AND [klinika]='{0}'  ORDER BY [name2]",Session["klinika_id"]);

        Dictionary<int, Hashtable> table = x2Mysql.getTable(sb.ToString());

        int dataLn = table.Count;

        ArrayList result = new ArrayList();
        //result.Add("-", "-");
        result.Add("0|-");
        for (int i = 1; i <= dataLn; i++)
        {
            result.Add(table[i - 1]["id"].ToString() + "|" + table[i - 1]["name3"].ToString());
        }

        return result;

    }

    protected void commentChanged(object sender, EventArgs e)
    {

        TextBox tbox = (TextBox)sender;

        string[] tmp = tbox.ID.ToString().Split('_');

        //Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;

        Control controlList1 = FindControl("textBox_" + tmp[1] + "_" + tmp[2]);
        Control controlList2 = FindControl("ddl_" + tmp[1] + "_" + tmp[2]);

        TextBox tBoxF = (TextBox)controlList1;
        DropDownList doctor_lb = (DropDownList)controlList2;


        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", this.shiftType[col]);
        data.Add("ordering", col + 1);

        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }


        data.Add("date_group", rok + mesiac);
        int den = Convert.ToInt32(tmp[1]);
        den = den + 1;

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());
        data.Add("comment", tBoxF.Text.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_all", data);

        Boolean res = Convert.ToBoolean(result["status"].ToString());

        if (!res)
        {
            this.msg_lbl.Text = result["msg"].ToString();
        }



        // this.msg_lbl.Text = e.ToString() + "..." + tBoxF.Text.ToString();
    }


    protected void dItemChanged(object sender, EventArgs e)
    {
        // this.msg_lbl.Text = e.ToString();

        DropDownList ddl = (DropDownList)sender;

        string[] tmp = ddl.ID.ToString().Split('_');


       // Control tmpControl = Page.Master.FindControl("ContentPlaceHolder1");

        //ContentPlaceHolder ctpl = (ContentPlaceHolder)tmpControl;
        Control controlList = FindControl("ddl_" + tmp[1] + "_" + tmp[2]);

        DropDownList doctor_lb = (DropDownList)controlList;

        SortedList data = new SortedList();
        data.Add("user_id", doctor_lb.SelectedValue.ToString());

        int col = Convert.ToInt32(tmp[2]);
        data.Add("typ", this.shiftType[col]);
        data.Add("ordering", col + 1);
        data.Add("clinic", Session["klinika_id"].ToString());
        string mesiac = this.mesiac_cb.SelectedValue.ToString();
        string rok = this.rok_cb.SelectedValue.ToString();

        if (mesiac.Length == 1)
        {
            mesiac = "0" + mesiac;
        }


        data.Add("date_group", rok + mesiac);
        int den = Convert.ToInt32(tmp[1]);
        den = den + 1;

        data.Add("datum", this.rok_cb.SelectedValue.ToString() + "-" + this.mesiac_cb.SelectedValue.ToString() + "-" + den.ToString());

        SortedList result = x2Mysql.mysql_insert("is_sluzby_all", data);

        Boolean res = Convert.ToBoolean(result["status"].ToString());

        if (!res)
        {
            this.msg_lbl.Text = result["msg"].ToString();
        }

    }

    protected void publishSluzby(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string ID = btn.ID.ToString();

        int dnesJe = DateTime.Today.Day;

        Session.Add("aktSluzMesiac", this.mesiac_cb.SelectedValue.ToString());
        Session.Add("aktSluzRok", this.rok_cb.SelectedValue.ToString());
        // Session.Add)"aktSluzMesLbl", DateTime.Today.m

        if (ID == "toWord_btn")
        {
            Session.Add("toWord", 1);
            Response.Redirect("sltoword.aspx");
        }
        if (ID == "print_btn")
        {
            Session.Add("toWord", 0);
            Response.Redirect("sltoword.aspx");
        }

    }


}