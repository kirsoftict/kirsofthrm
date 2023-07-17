<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptpayroll.aspx.vb" Inherits="rptpayroll" %>

<%@ Import Namespace="Kirsoft.hrm" %>
 <%Dim fm As New formMaker
    Dim fl As New file_list
    Dim sec As New k_security
    Dim t1, t2 As Date
    t1 = Now%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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
	<script type="text/javascript" src="scripts/kirsoft.payrol.js"></script>
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>

<%  If  Session("iid") <>"" Then
        %>
        <script>            window.location = "logout.aspx";</script>
        <%
    End If%>
    <link href="css/payrol.css" media="screen" rel="stylesheet" type="text/css" />
    <style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>
	<%	     	    Dim namelist As String
	    Dim reflist As String
	    reflist = fm.getjavalist2("payrollx", "distinct ref", Session("con"), "")
	    
	    
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
	

 %>
 <script type="text/javascript">
// var stime=Date.now();
<% if Request.QueryString("month")<>"" then
response.write("var month=" & chr(34) & request.querystring("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.querystring("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.querystring("projname")) & chr(34) & ";")

else
response.write("var month=" & chr(34) & request.form("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.form("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.form("projname")) & chr(34) & ";")

end if 
response.write("var ref=" & chr(34) & request.querystring("ref") & chr(34) & ";")

%>

  
 var paymth="<% response.write(request.querystring("paymth")) %>";
 //alert(document.referrer.toString())
 var ppname="<% response.write(sec.dbStrToHex(request.querystring("projname"))) %>";
 
   var reflsit=[<%=reflist %>];
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

        function searchpayroll()
        {
            if($("#txtreflist").val()!=="")
            {
              $("#frmsearch").attr("action","?search=on");
                $("#frmsearch").submit();
            }
        }

    </script>
    <script src="scripts/kirsoft.payrol2.js" type="text/javascript"></script>
    <script>
       function rptviewx(ref,proj,rmk)
   {
        $("#ref").val(ref);
        $("#pname").val(proj);
        $("#rmk").val(rmk);
        alert(ref);
        $("#rptview").submit();

   }
    </script>
</head>
<body>
  <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")
      Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='1000px' height='500' frameborder='0' src='' scrolling='auto'></iframe>"))
%><form id='frmx' name='frmx' method="post" style='display:none'></form>
   <div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px;">Payment Report
    <form name="frmlistout" id="frmlisout" action="" method="post" style="font-size:12pt;">
        Payment Calendar: <select id="month" name="month">
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
         Enter Project: <input type="text" name="projname" id="projname" />
          <select id="res" name="res" style='visibility:hidden;'>
         <option value="avtive">Active</option>
         <option value="resign">Resign</option>
         </select> 
         <select id="paidst" name="paidst" style=" visibility:hidden;">
         <option value="unpaid">Un-paid</option>
         <option value="paid" selected="selected">Paid</option>
         </select>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submit('frmlisout');" />
    </form>
    </div>
    <div>
    <form method="post" id='frmsearch' name='frmsearch'>Search By Payroll No.<input type="text" name="txtreflist" id="txtreflist" onkeyup="javascript:startautoc('txtreflist',reflsit);" />
    <input type="button" value="Get" onclick="Javascript:searchpayroll();" />
    </form></div>
    <form method="post" action="" id="rptview" name="rptview">
            <input type="hidden" id="ref" name="ref" />
            <input type="hidden" id="rmk" name="rmk" />
            <input type="hidden" id="pname"  name="pname" />

    </form>
    <div>
        <div style="width:5px; float:left;"></div> 
      
     <div style="width: 5px; float: left;">
       <%
           Dim obj As Object
           
           Dim loc As String = Server.MapPath("download") & "\operatorview.txt"
           loc = loc.Replace("\", "/")
           'Response.Write(loc)
    
           %>
          
        </div> 
        <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form>
        <div class="clickexp" style=" float:left; border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; display:none;" onclick='javascript:exportx("payrol(<%response.write(now.day.tostring & now.month.tostring & now.year.tostring) %>)","xls","<%response.write(loc) %>","export","2;3");' >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="excel" /> Export to Excel</div>
 
   </div>
    </div>
   
    <div><%  If Request.QueryString("ref") <> "" Then
                 Response.Write(goseach(Request.QueryString("ref")))
             ElseIf Request.QueryString("search") = "on" Then
                 ' Response.Write(Request.Form("txtreflist"))
                 Response.Write(goseach(Request.Form("txtreflist")))
             ElseIf Request.Form("rmk") <> "" Then
                 gotomake(Request.Form("rmk"))
             ElseIf Request.Form("projname") <> "" Then
                 Response.Write(Request.Form("projname"))
                 viewlist()
             ElseIf Request.QueryString("prid") <> "" Then
                 'Response.Write("viewing....123")
                 identifytime()
                 Response.Write(Now.ToString)
                 Response.Write("<br>today" & Today.ToString)
             End If
             
            
             
             %></div>
    </body>
    </html>