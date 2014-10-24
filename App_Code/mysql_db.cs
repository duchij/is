
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Odbc;
using System.Text;
/*
 * try
        {
            OdbcTransaction trans1 = null;

            String sConString = "DRIVER={MySQL ODBC 3.51 Driver}; SERVER=192.168.10.69;DATABASE=wms; UID=root; PASSWORD=rmysql; OPTION=3";
            OdbcConnection oConnection = new OdbcConnection(sConString);
            oConnection.Open();
             trans1 = oConnection.BeginTransaction();
           try
            {
              
           
                OdbcCommand cmdtrans = new OdbcCommand();
                cmdtrans.Connection = oConnection;
                cmdtrans.Transaction = trans1;
                try
                {
                    cmdtrans.CommandText = "update dynamos set fname='chandrak1' where dl_no ='DL000371'";
                  int rowsaffected = cmdtrans.ExecuteNonQuery();
                  if (rowsaffected > 0)
                  {
                    //-- trans1.Commit();
                      trans1.Rollback();
                  }
                }

catch(Excption ex)*/




/// <summary>
/// Summary description for mysql_db
/// </summary>
public class mysql_db
{
    public OdbcConnection my_con = new OdbcConnection();

	public mysql_db()
	{
		//
		// TODO: Add constructor logic here
		//
        Configuration myConfig = WebConfigurationManager.OpenWebConfiguration("/is");
        ConnectionStringSettings connString;
        connString = myConfig.ConnectionStrings.ConnectionStrings["kdch_sk"];
        my_con.ConnectionString = connString.ToString();

	}

    public SortedList mysql_insert(string table, SortedList data)
    {



        SortedList result = new SortedList();
        OdbcTransaction trans1 = null;
        my_con.Open();
        trans1 = my_con.BeginTransaction();

        OdbcCommand cmdtrans = new OdbcCommand();
        cmdtrans.Connection = my_con;
        cmdtrans.Transaction = trans1;

        StringBuilder sb = new StringBuilder();

        ArrayList columns = new ArrayList();

        foreach (



        sb.AppendFormat("INSERT INTO [{0}] ({1}) VALUES ({2}) ON DUPLICATE KEY UPDATE {3}");



        return result;

    }
}
