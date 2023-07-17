<%@ Page Language="VB" AutoEventWireup="false" CodeFile="chartemp.aspx.vb" Inherits="chartemp" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<script type="text/javascript" src="chart/sources/jscharts.js"></script>

</head>
<body style="background:#ededed;">
<form id='frms' runat="server" >
<asp:Literal ID='ltrtext' runat="server"></asp:Literal></form>
 <%
    ' Response.Write(Request.QueryString("chart"))
     If Request.QueryString("chart") <> "" Then
         chartmk(Request.QueryString("chart"))
     End If%>
</body>
</html>