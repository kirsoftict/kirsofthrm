<%@ Page Language="VB" AutoEventWireup="false" CodeFile="checksession.aspx.vb" Inherits="checksession" %>

<%@ Import Namespace="Kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
<link rel="stylesheet" type="text/css" media="print, handheld" href="css/media.css"/>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css"/>
	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
<meta http-equiv="refresh" content="15" />

    <title>Untitled Page</title>
</head>
<body>
     <%  If Session("username") = "" Then
             'Response.Redirect("logout.aspx?session=timeout")
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
           window.parent.location.href = "logout.aspx?msg=session expired";
</script>
       <%
       Else
           Response.Write(Session("username"))
       End If
       If IsError(Session("con")) = True Then
             %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
           window.parent.location.href = "logout.aspx?msg=session expired";
</script>
       <%
       End If
       Dim fp As String = Server.MapPath("")
       Dim subm As Boolean = False
       '  Response.Write(Now.Hour.ToString & ":" & Now.Minute.ToString)
       Dim fx() As String = {""}
       If String.IsNullOrEmpty(Session("right")) = False Then
           fx = Session("right").split(";")
           ReDim Preserve fx(UBound(fx) + 1)
           fx(UBound(fx) - 1) = ""
       End If
       Dim fm As New formmaker
       

       'Response.Write(fm.searcharray(fx, "1"))
      %>
      <script>
         // alert("referesh");
      </script>
      
      <%
            If (fm.searcharray(fx, "1")) then
           If Now.Hour.ToString = "13" And Now.Minute.ToString = "0" Then
               subm = True
          %>

          <script>
          
              function summm() {
              //alert("wt is going on");
               //   $('#frmx').attr("target", "_blank");

               //  $('#frmx').attr("action", "http://kirubel:8009/dbbackup.aspx");
               //  $('#frmx').submit();
              }
              
             
       </script>
          <%
          End If
      End If
       %>
       <div id="clock"></div>
       <form id='frmx' name='frmx' method="post">
            <input type='text' id='fileloc' name='fileloc' value='<%=fp %>'/>
       </form>
       <% If subm = True Then%>
       <script>
           summm();
       </script>
       <%End If%>
</body>
</html>

