<%@ Page Language="VB" AutoEventWireup="false" CodeFile="emailcreater.aspx.vb" Inherits="emailcreater" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title></title>
    <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css"/>
  
	<script src="jqq/jquery-1.9.1.js"></script>
  <script>
      function send() {
          alert("sending....");
          $("#frm").attr("action", "?send=true");
          $("#frm").submit();
         
      }
  </script>
  <link href="css/print600.css" media="screen" rel="stylesheet" type="text/css" />
    
</head>
<body>  
<%  If Request.Form("forward") <> "" Then
            'Response.Write("IIIN")
            createMail()
        End If%>
       <form id='frm' method="post"></form>
<form id=frmx action="?post=1" method=post>
    Forword email<input type=text name="forward" />
    <input type=submit value=submit />
</form>
        <% 'Response.Write(sendemail)%>

        
    </body>
</html>
