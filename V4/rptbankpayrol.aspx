<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptbankpayrol.aspx.vb" Inherits="rptbankpayrol" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
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

<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
<script src="scripts/script.js" type="text/javascript"></script>

<script type="text/javascript" src="jqq/demo.js"></script>
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
      
       $('#frmlistout').attr("action","rptbankpayrol.aspx?"+str);
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
    var printstyle=document.getElementById(st);
    var printFriendly = document.getElementById(loc);
    var printWin = window.open('print.htm','_self');
    printWin.document.write("<html><head><link href='images/kir.ico' rel='shortcut icon'><title>" + head + "</title>" + style + "</head><body>" + printFriendly.innerHTML + "</body></html>");
   
 printWin.document.close();
    printWin.window.print(); 
  //printWin.document.close();
   //printWin.close();
    }
  
	$(function() {
		$( "#radio" ).buttonset();
	});
	
    </script>
</head>

<body style="height:auto;">
<div id="st"></div>
<%
   
    '  For Each k As String In Request.ServerVariables
    'Response.Write(k & "==" & Request.ServerVariables(k) & "<br>")
    ' Next

    
    %>
    <form id='frmx' name='frmx' method="post" style='display:none'></form>
  <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form>
<div style="width:100%; height:125px; background:#6879aa; text-align:center;color:White; font-size:13pt;">Payroll Bank Statement Print Form

   

    
    <form name="frmlistout" id="frmlistout" action="" method="post">
        <select id="month" name="month">
            <%  For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
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
         <input type='hidden' id='bname' name='bname' value='<%=Request.QueryString("bname") %>' />
         <% Response.Write(Request.QueryString("bname"))%>
         <br />
        <% If Request.QueryString("bank") <> "ot" and Request.QueryString("incr")<>"on" Then%> 
         <fieldset title="" style="width:320px;">
         <div id="radio" style='height:20px; font-size:11px;'>
         <input type="radio" id='Radio2' name='summery' value="summery" /><label for="Radio2">Summery</label>
         <input type="radio" id="Radio1" name="summery" value="details" /><label for="Radio1">Details</label></div>
         <div>
        Include Back Pay OT <input type="checkbox" name="ot" id="ot" value="on" />
        <%End If%>
        <% If Request.QueryString("bank") = "ot" Then%>
        <input type="hidden" id="bank" name="bank" value="<% response.write(request.querystring("bank")) %>" />
        <%End If%>
         <% If Request.QueryString("incr") = "on" Then%>
        <input type="hidden" id="incr" name="incr" value="<% response.write(request.querystring("incr")) %>" />
        <%End If%>
        <input type="hidden" id='ref' name='ref' />
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:findidpp();" />
 </div> </fieldset>   </form></div>
     <div id="print4"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:printx('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>       
      <div style="width: 5px; float: left;">
        </div> 
     <div style="width: 5px; float: left;">
       <%
           Dim obj As Object
           
           Dim loc As String = Server.MapPath("download") & "\rptbank.txt"
           loc = loc.Replace("\", "/")
           'Response.Write(loc)
    
           %>
          
        </div> 
        <div class="clickexp" style=" float:left; border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; display:none;" onclick='javascript:exportx("bank-report(<%response.write(now.day.tostring & now.month.tostring & now.year.tostring) %>)","xls","<%response.write(loc) %>","export","2;3");' >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="excel" /> Export to Excel</div>

     <form name="frmpay" id="frmpay" action="" method="post">
     <input type="hidden" name="nextpage" id="nextpage" />
   <div style="width:790px; overflow:scroll; height:330px;"> 
   <div id="bigprint" style='width:650px;'>
            
    
     <input type="hidden" id="Hidden1" name="nextpage" />
     
   <%  ' Response.Write(Request.QueryString("Summery"))
       For Each k As String In Request.ServerVariables
           'Response.Write(k & "==" & Request.ServerVariables(k) & "<br>")
       Next
       ' Response.Write(Request.QueryString("incr"))
       Dim pd As Date
           If Request.QueryString("ppd") <> "" Then
           
               pd = Request.QueryString("ppd")
           ' pd = pd.AddMonths(-1)
       Else
           pd = Request.Form("month") & "/1/" & Request.Form("year")
       End If
           Dim sec As New k_security
           Dim cod As String
           cod = ""
           If Request.QueryString("bank") = "ot" Then
           cod = makeformot()
       ElseIf Request.QueryString("incr") = "on" Then
           cod = makeinc()
          ' Response.Write(Request.QueryString("incr"))
       ElseIf Request.QueryString("Summery") = "summery" Then
           cod = makeform()
           ElseIf Request.QueryString("Summery") = "details" Then
           
               cod = makeform2()
           End If
       If String.IsNullOrEmpty(cod) = False Then
           ' cod = sec.StrToHex(cod)
           ' Response.Write(cod)
           obj = cod
           obj = "1;2;3" & Chr(13) & obj
           File.WriteAllText(loc, obj)
       End If
       
       
        %></div>
        </div>
        </form>
       <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascrippt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');"><img src='images/ico/print.ico' alt="print"/>print</div>        
<% if cod<>"" then%>

<script>
    $(".clickexp").css({ display: 'inline' });
    $("#tb1").remove("display");
    
       
      
</script>
<% End If
    
    If Request.QueryString("projname") <> "" Then
        
    %>
<script type="text/jscript">
<%if request.form("year")<>"" then%>
$("#projname").val("<% response.write(request.querystring("projname")) %>");
<% else%>
$("#projname").val("<% response.write(sec.dbHexToStr(request.querystring("projname"))) %>");
<%end if %>

$("#month").val("<% response.write(pd.month.tostring) %>");
$("#year").val("<% response.write(pd.year.tostring) %>");
$("#ref").val("<% response.write(request.querystring("ref")) %>");
$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();


</script>
<% ElseIf Request.Form("projname") <> "" Then
    %>
    <script type="text/jscript">

$("#projname").val("<% response.write(request.form("projname")) %>");
$("#month").val("<% response.write(pd.month.tostring) %>");
$("#year").val("<% response.write(pd.year.tostring) %>");
$("#ref").val("<% response.write(request.querystring("ref")) %>");
$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();


</script>

    <% End If
    %>

</body>
</html>


