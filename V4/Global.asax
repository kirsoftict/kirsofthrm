<%@ Application Language="VB" %>
<%@ Import Namespace = "system.data"%>
<%@ import Namespace="system.data.sqlclient" %>


<script runat="server">
  Public headerfiletypex As String
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Dim con As Object
        Application("path") = Server.MapPath("")
        '  Application("constr") = System.Configuration.ConfigurationManager.ConnectionStrings("hrmpConnectionString1").ToString ' "Data Source=.\SQLEXPRESS;AttachDbFilename=" & Session("path") & "\app_data\hrmp.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True"
        'con = New SqlConnection(Application("constr"))
        ' Application("con") = con
        '  Application("con").open()
        ' Code that runs on application startup<A HREF="Global.asax">Global.asax</A>
        Application("baddress") = "+251 911 479143, P.o.Box:23932"
        Application("logo") = "images/netlog.gif"
        Application("company_name") = "Net Consult P.L.C"
        Application("company_name_amharic") = "ኔት ኮንሰልት ሃ/የተ/የግ/ማ."
        Application("hour") = Now
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        'MsgBox(Application("path"))
        Application("con").close()
        Application.RemoveAll()
        
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        ' MsgBox(Application("path"))
        Dim con As Object
        ' Dim con As Object

        ' Dim sqlcmd As New SqlCommand
        Session("path") = Server.MapPath("")
        Session("constr") = System.Configuration.ConfigurationManager.ConnectionStrings("hrmpConnectionString1").ToString ' "Data Source=.\SQLEXPRESS;AttachDbFilename=" & Session("path") & "\app_data\hrmp.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True"
        con = New SqlConnection(Session("constr"))
        Session("con") = con
        Session("con").open()
        Session("company_name") = "Net Consult P.L.C"
        Session("company_name_amharic") = "ኔት ኮንሰልት ሃ/የተ/የግ/ማ."
        Session("tin")="0000041336"
        Session("systemon") = "12/1/2013"
        Session("logo") = "images/netlog.png"
        Session("region") = "A.A"
        Session("s_city") = "Bole"
        Session("ctel") = "+251 11 6654654"
        Session("w") = " "
        Session("hno")=" "
        'session("cmd")=sqlcmd
        'Response.Write("connection opend" & "<br />")
       
        ' MsgBox(Session("path"))
       
 
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        session("con").close()
        
        session("con") = Nothing
        
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
  
   
</script>