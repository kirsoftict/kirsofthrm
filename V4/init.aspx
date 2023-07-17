<%@ Page Language="VB" AutoEventWireup="false" CodeFile="init.aspx.vb" Inherits="init" %>

<%@ Import Namespace="Kirsoft.hrm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
   
    
    <script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
     <script type="text/javascript" src="jqq/ui/jquery.ui.progressbar.js"></script>
    <script type="text/javascript" src="scripts/kirsoft.java.js"></script>
</head>
<body>
<div id="msgout"></div>
   <div id='lod' style="width:50px; height:50px; left:640px;top:340px; position:absolute"><img src='images/loading.gif' /></div>
  
      <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

          'Response.Write(Session("page1"))
          Dim t1, t2 As Date
          t1 = Now
          Dim loadf As Boolean
      If Request.QueryString("next") = "on" Then
              
              Dim s, m As String
              loadf = leave_b_c()%>
              <script>

    var w;
    var h;
    w = window.screen.availWidth;
    h = window.screen.availHeight;
    prog2(10000);
   
        </script><%
              t2 = Now
              s = t2.Subtract(t1).Seconds
              m = t2.Subtract(t1).Minutes
              
              'Response.Write(CDate(t1).Srubtract(t2).Seconds.ToString & "sec")%>
          <script>
   //alert("<% 'Response.Write(CDate(t1).Subtract(t2).Seconds.ToString & "sec") %>");
   <% if loadf=true then %>
              $(document).ready(function () {
             window.location = "home.aspx?timedif=<%response.write("m=" & m & "s=" & s) %>"; 
            $("lod").text("Finsh");
        });
        <%end if %>
         </script><%
      Else
          %><script>                window.location = "init.aspx?next=on"; </script><%
                                                                                    End If
                                                                                    
                                                                                    ' Session("con").close()
                                                                                    
                                                                                 
      %>
   
</body>
</html>
