<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empatt.aspx.vb" Inherits="empatt" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<%  Dim fm As New formMaker
    Dim namelist As String
    Dim t1, t2 As Date
    t1 = Now
    %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
	
	<script type="text/javascript" src="scripts/form.js"></script>
<style type="text/css">
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
 <script type="text/javascript">
 $("#showst").css("display","inline");
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
 </script>
 <script type="text/javascript" language="javascript">
    function print(loc,title,head,footer)
    {
    var sty;
    if(footer=="A4")
    {
        sty="<link rel='stylesheet' type='text/css' href='css/printattA4.css' /> "
    }
    else if(footer=="A3")
    {
     sty="<link rel='stylesheet' type='text/css' href='css/printA3.css' /> "
    }
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("print.htm",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<html><head><title>" + head + "</title>" + sty +"</head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
   // printWin.close();
    }
   
    </script>
    <style type='text/css'>#tb1{ border:1px solid black;font-size:10pt;}#tb1 td {border-top: 1px solid black;border-left:1px solid black;}</style>
<link rel='stylesheet' type='text/css' href='css/printattA4.css' />
</head>

<body>
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>
<div id='tim' style='font-size:8px; color:Gray;'></div>
<iframe id="showst" src="_app_offline.htm" style="width:100px; display:none;"></iframe>

<form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form>
<div style="width:100%; height:45px; background:#6879aa; text-align:center;color:White; font-size:19pt;">Attendance Viewer</div>

    <form name="frmlistout" action="" method="post">
        <select id="month" name="month">
            <%  For i As Integer = 1 To 12
                    If Today.Month = i Then
                        Response.Write("<option value='" & i.ToString & "' selected='selected'>" & MonthName(i) & "</option>")
                    Else
                        Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                    End If
                    
                Next%>
        </select>
         <select id="year" name="year">
            <%  For i As Integer = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
         <input type="text" name="projname" id="projname" />
         
         <input type="submit" value="submit" />
    </form>
    <%  Dim sec As New k_security
        Dim outpx As String = "" 'Response.Write("<div id=" & Chr(34) & "outer" & Chr(34) & " style=" & Chr(34) & "border:2px black solid;width:1500px; text-align:center;font-size:12;" & Chr(34) & ">")
       
           
        ' outpx = pageviewonlyx()
        ' Response.Write(outpx)
                
                
                %>
                <div>
                 <div style="width: 5px; float: left;">
       <%
           Dim obj As Object
           
           Dim loc As String = Server.MapPath("download") & "\operatorview.txt"
           loc = loc.Replace("\", "/")
           'Response.Write(loc)
    
           %>
          
        </div> 
        

  </div>
                <%
                    If Request.Form("month") <> "" Then
                        outpx = attendance2() 'pageviewonly()
                        obj = outpx
                        obj = "1;2;3" & Chr(13) & obj
                        File.WriteAllText(loc, obj)
                %>
                 <div id="print" style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;float:left" onclick="javascirpt:print('listatt','Report_print','<%  response.write(Session("company_name").tostring) %>','A4');">
   <img src='images/ico/print.ico' alt="print"/>print A4 </div>  
      <div id="Div1" style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;float:left" onclick="javascirpt:print('listatt','Report_print','<%  response.write(Session("company_name").tostring) %>','A3');">
   <img src='images/ico/print.ico' alt="print"/>print A3 </div> 
   <div class="clickexp" style=" float:left; border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick='javascript:exportx("attendance(<%response.write(now.day.tostring & now.month.tostring & now.year.tostring) %>)","xls","<%response.write(loc) %>","export","1;2;3");' >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="excel" /> Export to Excel</div>
 <div style='clear:both;'/>
   
                <%
                    Response.Write(outpx)
            End If
            'Response.Write("</div>")
           
      
            t2 = Now
            
            
    %>
<script type="text/jscript">
$("#tim").text("<% response.write("Loading Time: " & t2.Subtract(t1).Minutes.toString & "Mins " & t2.Subtract(t1).Seconds & "secs") %>");
   </script>      
  
</body>
</html>

