using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Collections.Specialized;


public partial class utils_upload : System.Web.UI.Page
{
    lf x2lf = new lf();
    x2_var x2 = new x2_var();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["tuisegumdrum"] == null)
        {
            this.resultLit.Text = "Nie ste prihlaseny, prihlaste sa. Subor nebude nahraty do systemu.....";
        }else
        {
           this.uploadData();
        }

        

        

       // HttpContext.Current.Response.Write("Upload successfully!");



    }

    protected void uploadData()
    {
        HttpFileCollection files = HttpContext.Current.Request.Files;

        NameValueCollection formData = HttpContext.Current.Request.Form;

        string patient_name = formData["patientName"].ToString();
        string bin_num = formData["binNum"].ToString();
        string diagnose = formData["diagnose"].ToString();
        string note = formData["note"].ToString();
        string photo_date = formData["photoDate"].ToString();

        if (string.IsNullOrEmpty(patient_name))
        {
            patient_name = "pacient";
        }

        if (string.IsNullOrEmpty(bin_num))
        {
            bin_num = "NULL";
        }

        if (string.IsNullOrEmpty(note))
        {
            note = "NULL";
        }

        for (int i = 0; i < files.Count; i++)
        {

            HttpPostedFile uploadfile = files[i];

            // You must create “upload” sub folder under the wwwroot.

            byte[] data = new byte[uploadfile.ContentLength];
            int picLength = uploadfile.ContentLength;
            uploadfile.InputStream.Read(data, 0, picLength);

            string fileEx = System.IO.Path.GetExtension(uploadfile.FileName);

            SortedList lfData = new SortedList();
            lfData.Add("file-size", picLength);
            lfData.Add("file-name", uploadfile.FileName.ToString());
            lfData.Add("file-type", fileEx);
            lfData.Add("user_id", Session["user_id"]);
            lfData.Add("clinic_id", Session["klinika_id"]);
           

            SortedList photoData = new SortedList();
            photoData.Add("patient_name", patient_name);
            photoData.Add("bin_num", bin_num);
            photoData.Add("photo_date", photo_date);
            photoData.Add("diagnose", diagnose);
            photoData.Add("note", note);
            photoData.Add("item_hash", x2.makeByteHash(data));

            SortedList res = x2lf.cameraLfData(data, lfData, photoData);

            if (!(Boolean)res["status"])
            {
                this.resultLit.Text = "Chyba:"+res["msg"].ToString();
            }
            else
            {
                this.resultLit.Text = "Subor sa nahral v poriadku....";
            }



            // uploadfile.SaveAs(Server.MapPath(".") + "\\upload\\" + uploadfile.FileName);
        }
    }

    
}