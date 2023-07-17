<%@ Page Language="VB" AutoEventWireup="false" CodeFile="leavecalc.aspx.vb" Inherits="leavecalc" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	<script type="text/javascript" language="javascript">
    function print(loc,title,head,footer)
    {
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<html><head><title><% response.write(session("company_name")) %></title></head><body><h1 style='font-size:14pt;'>"+head+"</h1>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
    printWin.close();
    }
    function approvednow(val)
    {
        var str=$("#appvd").formSerialize();
   $("#appvd").attr("action", val +"&" + str);
   //alert(val + "&" + str);
    $("#appvd").submit();
 return true;
    }
     </script>
</head>
<body>
<div>
<%    
    outpage()
                    
                   %>
                  
   
<%
       
     %>
    <form id="form1" name="form1" method="Post" action="">
    </form>
   <p><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong><strong>Date: &hellip;&hellip;&hellip;&hellip;&hellip;.&hellip;&hellip;&hellip;&hellip;&hellip;</strong><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong><strong>&nbsp;</strong><strong>&nbsp;</strong><strong>Ref. No ..&hellip;&hellip;&hellip;..&hellip;&hellip;...&hellip;&hellip;&hellip;</strong><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong></p>
<p align="center"><strong><span style="text-decoration: underline;">LEAVE REQUEST FORM</span></strong></p>
<p><strong>I hereby request to be granted annual leave specified below:- </strong></p>
<ol>
<li><strong>1.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>Name of employee -----------------------------------------&nbsp;&nbsp; position-----------------------------</strong></li>
</ol>
<p><strong>ID NO.-----------------------------</strong></p>
<ol>
<li><strong>2.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>Type of leave requested :-</strong></li>
</ol>
<p><strong>2.1 Annual Leave&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.4&nbsp;&nbsp; Marriage Leave&nbsp; </strong></p>
<p><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.2 Maternity Leave &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.5&nbsp;&nbsp; Leave without Pay&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong></p>
<p><strong>2.3&nbsp;&nbsp;&nbsp; </strong><strong>Sick Leave &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.6&nbsp;&nbsp;&nbsp; Other </strong></p>
<ol>
<li><strong>3.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>Number of working days leave requested: ----------------------------------------------</strong></li>
</ol>
<p><strong>Starting date of leave ------------------------------- </strong></p>
<p><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;Tel. No. while on leave -------------------------</strong></p>
<p><strong>Signature of applicant ------------------------&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date &nbsp;------------------------</strong></p>
<ol>
<li><strong>4.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong><span style="text-decoration: underline;">Approval of employee&rsquo;s immediate supervisor </span></strong></li>
</ol>
<p><strong>Supervisor&rsquo;s Comment -------------------------------------------------------------------</strong></p>
<p><strong>Supervisor&rsquo;s Name --------------------------------------- Signature &amp; Date ------------------------</strong></p>
<ol>
<li><strong>5.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong><span style="text-decoration: underline;">To be completed by Human Resource Adm. Head </span></strong></li>
</ol>
<p><strong>Employment date ------------------------------------------------&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong></p>
<p><strong>No of leave days Accrued &amp; transferred with approval: year ending June 30, ------------------</strong></p>
<p><strong>No of leave days Accrued during the month ending ---------------- 30, ------------------</strong></p>
<p><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total entitlement&nbsp; ------------------------------- days.</strong></p>
<p><strong>Less number of leave days approved; - -------------------------------------------</strong></p>
<p><strong>Current Annual leave balance:- -----------------------------------------------------</strong></p>
<p><strong>Starting date of leave ------------------------------&nbsp; end date of leave -----------------------------</strong></p>
<p><strong>From date&nbsp; -------------------------------------------- employee has to be on duty.</strong></p>
<p><strong>Checked by Head, HRA Sec. -&nbsp; ---------------------------------- -Signature &amp; Date:&nbsp; ---------------------</strong></p>
<p><strong><span style="text-decoration: underline;">CC.</span></strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>TO Mr/miss &nbsp;---------------------------------------------------------------------</strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>To General Manager</strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>To Administration &amp; Finance</strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>To Personal File </strong></p>
<p>&nbsp;</p>
</div>
</body>
</html>
