<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reportbank1.aspx.vb" Inherits="reportbank1" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%
    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim fl As New file_list
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><title></title>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/script.js" type="text/javascript"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
<script src="jqq/ui/jquery.ui.button.js"></script>
    <script src="jqq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
<link rel="stylesheet" href="jq/demos.css">
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
<script src="scripts/script.js" type="text/javascript"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>


    <link href="css/payrol.css" media="screen" rel="stylesheet" type="text/css" />
      <link href="css/bankst.css" media="screen" rel="stylesheet" type="text/css" />  
          

<style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>
	<%  	   
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", session("con"), "|")

 %>
 <link rel="stylesheet" href="css/printA4.css">

 <script type="text/javascript">
 
 function findidpp()
 {
  
        
       var str=$("#frmlistout").formSerialize();
   // alert(str);
      
       $('#frmlistout').attr("action","reportbank1.aspx?"+str);
       $('#frmlistout').submit();
    
       
 //  $("#nextpage").val(output);
  // $("#frmpay").attr("action","?" );
// $("#frmpay").submit();
 
 }

 var namelist=[<% response.write(namelist) %>];
    $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  namelist, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});



    function printx(loc,title,head,footer,st)
    {
    
   if(st=="printA4")
    {
        style="<link rel='stylesheet' href='css/bankst.css' />";
    }
    else if(st=="printA3")
    {
        style="<link rel='stylesheet' href='css/printA3.css' />";
    }
    else
    {
        style="";
    }
    var printstyle=document.getElementById(st)
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("print.htm",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<html><head><link href='images/kir.ico' rel='shortcut icon'><title>" + head + "</title>" + style + "</head><body>" + printFriendly.innerHTML + "</body></html>");
   
 printWin.window.print(); 
  printWin.document.close();
   //printWin.close();
    }
   
    </script>
           <style>.Highlighted a{

   background-color : Green !important;

   background-image :none !important;

   color: White !important;

   font-weight:bold !important;

   font-size: 8pt;

}</style>
</head>

<body style="height:auto;">
<div id="st"></div>
 <%  
         '    Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
     Response.Write(fl.dialog("pay", "Bank St. Notice", "Please! Make sure you want show all date wise"))
%>
<div style="width:100%; height:105px; background:#6879aa; text-align:center;color:White; font-size:13pt;">Payroll Bank Statement Print Form

   
<%
   mkjvdate("Perd") 
    For Each k As String In Request.ServerVariables
        ' Response.Write(k & "==" & Request.ServerVariables(k) & "<br>")
    Next

    
    %>
    
    <form name="frmlistout" id="frmlistout" action="" method="post">
        <select id="month" name="month">
            <%  For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "' title='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
        <script type="text/javascript">
            $("#month").val("<% response.write(today.month) %>");
        </script>
         <select id="year" name="year">
            <%  For i As Integer = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
         Enter Project<input type="text" name="projname" id="projname" /> 
         View Bank Acc. Status: "<select id='acc' name='acc'>
         <option value="acc">Who has account</option>
            <option value="None">Who has't Account</option>
             <option value="">All</option>
         </select><br />
         <fieldset title="Select one" style="width:320px;">
          Summery Only:<input type="radio" id='summery' name='summery' value="summery" />
         Details: <input type="radio" id="Radio1" name="summery" value="details" /><br />
        Include Back Pay OT <input type="checkbox" name="ot" id="ot" value="on" /> </fieldset>
        <input type="hidden" id='ref' name='ref' />
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:findidpp();" />
    </form></div>
     <div id="print4"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:printx('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>       
      <div style="width: 5px; float: left;">
        </div> <div id='expxlsx' style="display:none;float:left;">
<form id="exprtexcel" name="exprtexcel" action="print.aspx?pagename=bankst(<% response.write(request.querystring("ref")) %>)" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="" />
    <div style=" border:none; width:180px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick="javascript:$('#exprtexcel').submit();" >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="excel" /> Export to Excel</div>
</form></div>
     <form name="frmpay" id="frmpay" action="" method="post">
     <input type="hidden" name="nextpage" id="nextpage" />
   
   <div id="bigprint" style='width:auto;'>
            
    
     <input type="hidden" id="Hidden1" name="nextpage" />
     <input type='text' id='bkcalander' name='bkcalander' />
   <%  ' Response.Write(Request.QueryString("Summery"))
       Dim sec As New k_security
       Dim cod As String
       cod = ""
       If Request.QueryString("Summery") = "summery" Then
           cod = makeform()
       ElseIf Request.QueryString("Summery") = "details" Then
           
           cod = makeform2()
       End If
       If String.IsNullOrEmpty(cod) = False Then
           cod = sec.StrToHex(cod)
       End If
     
       
       
        %></div>
      
        </form>
   
       <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascrippt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');"><img src='images/ico/print.ico' alt="print"/>print</div>        

<%
   
    If cod <> "" Then%>

<script>
    $("#tb1").remove("display");
    $("#nnp").val("<%Response.Write(cod) %>");

    $("#expxlsx").css({ display: 'inline' });
</script>
<% end if %>
<script type="text/jscript">

$("#projname").val("<% response.write(request.querystring("projname")) %>");
$("#month").val("<% response.write(request.querystring("month")) %>");
$("#year").val("<% response.write(request.querystring("year")) %>");
$("#ref").val("<% response.write(request.querystring("ref")) %>");
$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();


</script>

</body>
</html>


