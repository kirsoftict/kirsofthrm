<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editpayrolref.aspx.vb" Inherits="editpayrolref" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%  If String.IsNullOrEmpty(Session("chgref")) = True Then
        Session("chgref") = Request.ServerVariables("HTTP_REFERER")& "?paidst=paid&" & Request.ServerVariables("QUERY_STRING")
    End If
    %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
    <title>Untitled Page</title>
    	<script src="jqq/jquery-1.9.1.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function goclick()
        {
            $('#frmx').attr("action","?editref=save");
            $('#frmx').submit();
        }
        function submitx()
        {
            alert("Data is changed");
             $('#fm').attr("target","frm_tar");
             $('#fm').attr("action","<%Response.Write(session("chgref")) %>");
              $('#fm').submit();
        }
    </script>
</head>
<body>

<form id='fm' name='fm' method="post" action=""></form>
<div>

<%  
    ' Response.Write(Request.ServerVariables(""))
    'Response.Write("<BR>" & Request.ServerVariables("QUERY_STRING"))
    If Request.QueryString("delref") <> "" Then
        
        Dim msg As String = deleteallx()
        If msg = "Data Saved" Then
           ' Response.Write(Request.ServerVariables("HTTP_REFERER"))
       %><script>
            //delayx(500);
            $('#fm').attr("target","frm_tar");
             $('#fm').attr("action","<% Response.Write(Request.ServerVariables("HTTP_REFERER"))%>");
            $('#fm').submit();
            </script>
       <%
       Else
           Response.Write(msg)
       End If
   ElseIf Request.QueryString("deltype") = "inc" Then
        Dim msg As String = deleteallxinc()
        If msg = "Data Saved" Then
       %><script>
            //delayx(500);
            $('#fm').attr("target","frm_tar");
             $('#fm').attr("action","<% Response.Write(Request.ServerVariables("HTTP_REFERER"))%>?<% Response.Write(Request.ServerVariables("QUERY_STRING"))%>");
            $('#fm').submit();
            </script>
       <%
       Else
         '  Response.Write(msg)
       End If
   Else
       Session("chgref") = (Request.ServerVariables("HTTP_REFERER")) & "?paidst=paid&" & (Request.ServerVariables("QUERY_STRING"))
       comex()
       Dim sq() As String
       sq = Session("chgref").split("?")
      ' Response.Write(sq.Length)
       If sq.Length > 2 Then
           Session("chgref") = sq(0) & "?" & sq(1)
           'Response.Write(Session("chgref"))
       Else
           ' Response.Write("whttt")
       End If
       
   End If
 
   %>
    
    </div>
</body>
</html>
