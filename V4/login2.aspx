<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login2.aspx.vb" Inherits="login2" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>Untitled Page</title>
    <link href="css/style.css" rel="stylesheet" type="text/css"/>
        <link href="css/layout.css" rel="stylesheet" type="text/css"/>
	<link href="css/message.css" rel="stylesheet" type="text/css"/>
	<!--[if lte IE 6]>
	<link href="themes/orange/css/IE6_style.css" rel="stylesheet" type="text/css"/>
	<![endif]-->
	<!--[if IE]>
	<link href="themes/orange/css/IE_style.css" rel="stylesheet" type="text/css"/>
	<![endif]-->
    <!--[if IE 9]>
        <link href="themes/orange/css/IE9_style.css" rel="stylesheet" type="text/css"/>
    <![endif]-->
    <!--[if IE 8]>
        <link href="themes/orange/css/IE8_style.css" rel="stylesheet" type="text/css"/>
    <![endif]-->
	<script type="text/javascript" src="scripts/style.js"></script>
	<script src="scripts/script.js" type="text/javascript"></script>
	<script type="text/javascript" src="scripts/archive.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.validate.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.form.js"></script>
    <script type="text/javascript" src="scripts/jquery/jquery.tablesorter.js"></script>
    <script type="text/javascript" src="js/validate.js"></script>

            
    <script type="text/javascript" src="js/core.js"></script>
<script type="text/javascript" src="js/admin.js"></script>

    <link rel="stylesheet" type="text/css" media="screen" href="css/main.css" />
<link rel="stylesheet" type="text/css" media="screen" href="css/module.report.css" />
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>

</head>
<body>
  <%  Dim str() As String
      If File.Exists("F:\kirold\WebSite2\Employee\allempx.txt") = True Then
          Response.Write("datais found")
             
          str = File.ReadAllLines("F:\kirold\WebSite2\Employee\allempx.txt")
          
             
          For i As Integer = 0 To str.Length - 1
              Response.Write(str(i) & "<br>")
          Next
          
      End If
      %>
      </body>

</html>
