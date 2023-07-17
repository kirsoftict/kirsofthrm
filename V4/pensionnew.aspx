<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pensionnew.aspx.vb" Inherits="pensionnew" %>

<%@ Import Namespace="Kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv='catch-control' content='no-catch' />
    <meta http-equiv='expires' content='0' />
    <meta http-equiv='pragma' content='no-catch' />



	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
   
	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
    <link rel="stylesheet" href="Pension_files/fonts.css" type="text/css" media="all">
     <link rel="stylesheet" href="css/bootstrap.css" type="text/css" media="all">
<link rel="stylesheet" href="Pension_files/style.css" type="text/css" media="all">
<link rel="stylesheet" id="screen_css-css" href="Pension_files/pension_screen.css" type="text/css" media="screen">
<!--link rel="stylesheet" id="print_css-css" href="Pension_files/pension_print.css" type="text/css" media="print"-->
<link rel="stylesheet" href="css/pensionnew.css" type="text/css" media="all">
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
		<style type="text/css">

.numberx
 {
  text-align:left}
</style>




 <script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>

<%  Dim namelist As String = ""
	    namelist = getjavalist2("tblproject", "project_name,project_id", Session("con"), "|", "$")
	

 %>

       
                        
 <script>
     if (window.XMLHttpRequest) {
         XMLHttpRequestObject = new XMLHttpRequest();
     } else if (window.ActiveXObject) {
         XMLHttpRequestObject = new 
ActiveXObject("Microsoft.XMLHTTP");
     }
     function getData(dataSource, divID) {
              if (XMLHttpRequestObject) {
             var obj = document.getElementById(divID);
             XMLHttpRequestObject.open("GET", dataSource);
             XMLHttpRequestObject.onreadystatechange = function () {
                 if (XMLHttpRequestObject.readyState == 4 &&
XMLHttpRequestObject.status == 200) {
                     obj.innerHTML = XMLHttpRequestObject.responseText;
                 }
             }
             XMLHttpRequestObject.send(null);
         }
     }
     $(document).ready(function () {
         $("#projname").autocomplete({
             source: function (req, response) {
                 var re = $.ui.autocomplete.escapeRegex(req.term);
                 var matcher = new RegExp("^" + re, "i");
                 response($.grep(namelist, function (item) {
                     return matcher.test(item);
                 }));
             }
         });

         $("#2019523").mouseover(function () { alert("i over"); });
     });
     function exportexcel(id) { 
     var filxe=new Bulb([$("#"+id).html()],{type:"application/vnd.ms-excel"});
     var url=URL.createObjectURL(filxe);

     let a=$("<a />",{
     href:url,
     download:"pensiondown.xls"}).appendTo("body").get(0).click();

     
     }
     function subtaxp() {
         //alert("in");
         
        $("#frmlistout").attr("action", "pensionnew.aspx");
        // prog2(5500);
        // showobj('progressbar');

         $("#frmlistout").submit();
     }
 </script>
 
</head>
<body>
<div id=disp></div>
<form id='frmx' name='frmx' method="post" style='display:none'></form>
  
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px;">
Pension Report 
    <form name="frmlistout" id="frmlistout" action="" method="post" style="font-size:12pt;color:black;">
    <div>
     Payroll Calendar: From: <input type="text" id='pd1' name='pd1' value='' style='color:black;' /> 
     to <input type="text" id='pd2' name='pd2' value='' style='color:black'/>
        <script>
            $(function () {
                $('#pd1').datepicker({ changeMonth: true, changeYear: true, maxDate: '+1d' });
                $('#pd1').datepicker('option', 'dateFormat', 'mm/dd/yy');
                $('#pd1').datepicker('option', 'color', 'black');
            });
            $(function () {
                $('#pd2').datepicker({ changeMonth: true, changeYear: true, maxDate: '+1d' });
                $('#pd2').datepicker('option', 'dateFormat', 'mm/dd/yy');
            });

        </script></div>
        <div style="display:none;">
       <input type="checkbox" id='ethcal' name='ethcal' value='ethcal' checked="checked" />use Eth calander range  <br />
       Payroll Calendar: <select id="month" name="month">
            <%  For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
        <select id="ethmonth" name="ethmonth"  style="display:none;">
            <%  Dim dt As New dtime
                For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & dt.getmonth(i, "amheng") & "</option>")
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
      <select id="ethyr" name="ethyr" onchange='javascript:getData("default4.aspx?ethyear=" + this.value + "&ethmonth=" + $("#ethmonth").val(),"disp");' style="display:none;">

            <% 
                Dim ethyr As Integer
                
                
               
                Response.Write(ethyr)
                For i As Integer = ethyr To ethyr - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
        </div>
          Pension <select id="paidst" name="paidst">
         <option value="pension">Collected</option>
          <option value="unpaid">Include Uncollected</option>
         </select>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:subtaxp();" />
    </form>
    </div>
 <% 
  Dim whr As String = Request.Form("paidst")
        ' Response.Write(whr)
           If Request.Form("pd1") <> "" Then
         '  Response.Write("<div class='blue_btn' id='print' onclick=" & Chr(34) & "javascript:print('bigprint','" & Session("conmpany_name") & "','','','pension')" & Chr(34) & ");'>Print</div>")
         Dim t1, t2 As Date
         t1 = Now
           
            Select Case whr
                Case "pension"
                    collectiononly()
                Case "unpaid"
                    allincludeunpaid()

         End Select
         t2 = Now
         Dim timout As String = ""
         timout = t2.Subtract(t1).Minutes & "Mins " & t2.Subtract(t1).Seconds & "Secs"
            Response.Write("<br>" & timout & "<br>")
        Else

        End If%>

  <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form>
<script>
   
</script>
</body>
</html>
