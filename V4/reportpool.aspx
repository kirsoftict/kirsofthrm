<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reportpool.aspx.vb" Inherits="reportpool" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="Microsoft.VisualBasic"%>
<%  If Session("username") = "" Then
     %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           window.location = "logout.aspx";
</script>
       <% End If
       Session.Timeout = "60"
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><link href="images/kir.ico" rel="shortcut icon" />
    <title></title>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.dialog.js"></script>
	 
       
   
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>


    <link href="css/payrol.css" media="screen" rel="stylesheet" type="text/css" />
    <style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>


   
  
 
 
 
    <script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <%  Dim rmk As String = Request.QueryString("remark")
        
        Select Case rmk
            Case "monthly"
                Response.Write(rptview_payroll())
                
        End Select
        
        %>
</body>
</html>
