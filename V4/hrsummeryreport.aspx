<%@ Page Language="VB" AutoEventWireup="false" CodeFile="hrsummeryreport.aspx.vb" Inherits="hrsummeryreport" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="Microsoft.VisualBasic"%>
<%
    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim fl As New file_list
    Dim sec As New k_security
    Dim t1, t2 As Date
    t1 = Now
    
    ' Response.AppendHeader("Content-Disposition", "attachment; filename=Error.xls")
    'Response.ContentType = "application/ms-excel"
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
   <script type="text/javascript" src="scripts/kirsoft.payrol.js"></script>
            
            <link href="css/iso.css" media="screen" rel="Stylesheet" type="text/css" />
            <%
          
 
		   
	   ' namelist = getjavalist2("tblproject", "project_name,project_id", Session("con"), "|", "$")
	

 %>
 <script type="text/javascript">
 var month;
 var year;
 var isotype;
 isotype="";
 pd1="";
 pd2="";
 month="";
 year="";
// var stime=Date.now();
<% if isdate(Request.QueryString("pd1")) then
response.write("var pd1=" & chr(34) & request.querystring("pd1") & chr(34) & ";")
response.write("var pd2=" & chr(34) & request.querystring("pd2") & chr(34) & ";")
response.write("var isotype=" & chr(34) & (request.querystring("isotype")) & chr(34) & ";")
response.write(" month=" & chr(34) & cdate(request.querystring("pd1")).month & chr(34) & ";")
response.write(" year=" & chr(34) & cdate(request.querystring("pd1")).year & chr(34) & ";")
else if isdate(Request.form("pd1")) then
response.write("var pd1=" & chr(34) & request.form("pd1") & chr(34) & ";")
response.write("var pd2=" & chr(34) & request.form("pd2") & chr(34) & ";")
response.write("var isotype=" & chr(34) & (request.form("isotype")) & chr(34) & ";")
response.write("month=" & chr(34) & cdate(request.form("pd1")).month & chr(34) & ";")
response.write("year=" & chr(34) & cdate(request.form("pd1")).year & chr(34) & ";")
end if 


%>

  
   
  
 function checkedc(lac) {

            if ($("#" + lac).is(':checked')) 
                {
                    //$("#chkall").text("Checked All");

                   // alert("chhhh");
                    $(".csssign").text(" ");

                }
               
               
            }
 
        function submitiso()
        {
         $('#frmlistout').attr("action", "?isotype="+isotype+"&pd1=" + pd1 + "&pd2=" + pd2 );
                  
         $("#frmlistout").submit();
       
        }
    </script>
   
    

	 
    <style type="text/css">

 
    </style>
    
	  <script src="scripts/taxrpt.js" type="text/javascript"></script> 
</head>

<body>

 <% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

       
    
         <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
<div id='tim' style='left:0px;top:0px; height:20px; position:inherit;'></div>
<form id='frmx' name='frmx' method="post" style='display:none'><input type="text" name="txtpass" /></form>
  
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px;">
HRM ISO Report
    <form name="frmlistout" id="frmlistout" action="" method="post" style="font-size:12pt;">
        Date Gap: <input type="text" id='pd1' name='pd1' value='<% response.write(today.addmonths(-1).toshortdatestring) %>' /> to 
        <input type="text" id='pd2' name='pd2' value='<% response.write(today.toshortdatestring) %>' />
        <script>
            dateafter('pd1', 'pd2');
            $(function () {

                $('#pd1').datepicker('option', 'dateFormat', 'mm/dd/yy');
            });

            $(function () {

                $('#pd2').datepicker('option', 'dateFormat', 'mm/dd/yy');
            });


        </script>
      
        <%  If Request.Item("pd1") <> "" Then
                Response.Write(Request.Item("pd1"))
            End If%>
        <input type="hidden" name="projname" id="projname" value='<%=namelist %>' />
          Report Type<select id="isotype" name="isotype">
                 <option value="Hired">Hired</option>
       <option value="Termination">Termination</option>
      <option value="Promotion"> Promotion</option>
      <option value="Transfer">Transfer</option>
 <option value="Vacancy">Vacancy</option>
         </select>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submitiso();" />
    </form>
    </div>
   
                     
      <div style="width:5px; float:left;"></div> 
     
                   <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form> 

        <%  If Request.Form("pd1") <> "" Then
                init()
            End If%>
    
         
 <%        
   
    t2 = Now
    Dim timout As String = ""
    timout = t2.Subtract(t1).Minutes & "Mins " & t2.Subtract(t1).Seconds & "Secs"
   
    %>
<script type="text/jscript">
$("#tim").text("<% response.write("Loading Time: " & timout) %>");
$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();

</script>
<div id="coverx" style="background:">

 
  
 
    </div>

</body>
</html>


 <script language="javascript" type="text/javascript">
     $(document).ready(function () {
  // alert("<% response.write(request.querystring("chkpen")) %>");
  //alert(month + 'and' +year);
  alert($(".page").outerHeight());
  var www= $("#termx").outerHeight();
    alert("www");
     // $("#ppaytop").css({display:'inline'});
   //  showobj("bigprint","cover",0,0);
  //   $("#cover").css({hieght:findh(document.getElementById("bigprint"))+"px",width:"100px"});
    // alert(findh(document.getElementById("bigprint")));
  
</script>

