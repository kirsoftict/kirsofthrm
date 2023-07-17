<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="Microsoft.VisualBasic"%>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">

<title>   Pension</title>

    <title></title>
    <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
   
	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
    <link rel="stylesheet" href="./Pension_files/fonts.css" type="text/css" media="all">
<link rel="stylesheet" href="./Pension_files/style.css" type="text/css" media="all">
<link rel="stylesheet" id="screen_css-css" href="./Pension_files/pension_screen.css" type="text/css" media="screen">
<link rel="stylesheet" id="print_css-css" href="./Pension_files/pension_print.css" type="text/css" media="print">



		<style type="text/css">
img.wp-smiley,
img.emoji {
	display: inline !important;
	border: none !important;
	box-shadow: none !important;
	height: 1em !important;
	width: 1em !important;
	margin: 0 .07em !important;
	vertical-align: -0.1em !important;
	background: none !important;
	padding: 0 !important;
}
.numberx
 {
  text-align:left}
</style>




 <script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>


			</head>


<body id="subpage">



    <% 
        ' Response.Write(email())
        'Response.Write(Session("epwd") & "===?" & Session("efrom") & "<br>")
        ' Dim kiemail As New mail_system
        ' Response.Write(kiemail.sendemail("test", Session("epwd"), Session("efrom"), "z.kirubel@gmail.com", "test2").ToString)
        '  makeformunpaid()
        selectout()
        %>
</div></div></body></html>