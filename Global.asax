<%@ Application Language="C#" %>

<script runat="server">
    
    
    void Application_Start(object sender, EventArgs e) 
    {
     
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        //Server.Transfer("error.html");
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {

        //Session.Timeout = 10;
        // Code that runs when a new session is started
        

    }

    void Session_End(object sender, EventArgs e) 
    {
        Server.Transfer("Default.aspx");
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        x2_var x2 = new x2_var();
        Boolean status = x2.offline();

        if (status == false)
        {
            try
            {
                if (Session != null)
                {
                    Session.Abandon();

                }
                //Response.Redirect("offline.html");
                Server.Transfer("offline.html");
            }
            catch (Exception error)
            {
                Server.Transfer("offline.html");
            }
            //Session.Abandon();
        }

        
    }
       
</script>
