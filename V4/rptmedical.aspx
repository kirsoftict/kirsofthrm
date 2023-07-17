<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptmedical.aspx.vb" Inherits="rptmedical" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
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
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script src="jqq/ui/jquery.ui.datepicker.js"></script>
	<script src="jqq/ui/jquery.ui.button.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
	
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>


    <link href="css/printA4app.css" media="screen" rel="stylesheet" type="text/css" />
    <style type="text/css">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>
	<%  	   
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
	

 %>
 <script type="text/javascript">
// var stime=Date.now();
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
    <script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>

	 
</head>
<body>
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>


       
    
         <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
    <div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:20px;">
   Medical Report
    <form name="frmlistout" action="" method="post" style="font-size:12pt;">
           
      
        <script type="text/javascript">
            $("#month").val("<% response.write(today.month) %>");
        </script>
         Budget Period:<select id="year" name="year">
         <% Dim ds As New dbclass
             Dim dtf As String
                Dim rs As DataTableReader
             rs = ds.dtmake("dtmk", "select distinct date_from from emp_medical_all", Session("con"))
             If rs.HasRows Then
                 Response.Write("<option value=''>Select Period</option>")
                 While rs.Read
                     dtf = MonthName(CDate(rs(0)).Month, True) & " " & CDate(rs(0)).Year.ToString & " - " & MonthName(CDate(fm.getinfo2("select date_exp from emp_medical_all where date_from='" & rs(0) & "'", Session("con"))).Month, True) & " " & CDate(fm.getinfo2("select date_exp from emp_medical_all where date_from='" & rs(0) & "'", Session("con"))).Year.ToString
                     Response.Write("<option value='" & CDate(rs(0)).ToShortDateString & "-" & CDate(fm.getinfo2("select date_exp from emp_medical_all where date_from='" & rs(0) & "'", Session("con"))).ToShortDateString & "'>" & dtf & "</option>")
                 End While
             Else
                 Response.Write("<option value=''>No Data</option>")
             End If
                rs.Close()
                ds = Nothing
               
                
                %>
           
        </select>
         Enter Project: <input type="text" name="projname" id="projname" />
          <select id="res" name="res" style='visibility:hidden;'>
         <option value="avtive">Active</option>
         <option value="resign">Resign</option>
         </select> 
         Get All<input type="checkbox" value="all" id="chkall" name="chkall" />
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submittopmed();" />
    </form>
    </div>
    <% If (Request.Form("year") = "" Or Request.Form("projname") = "") And Request.Form("chkall") = "" Then
            If Request.Form("chkall") = "" Then
                Response.Write("Please Enter Period and Project name")
            Else
                %>
             <div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4app');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>
           <div id='bigprint'><%
                         If Request.Form("chkall") <> "" Then
                             mkform_all()
                         Else
                             mkform()
                         End If
                                        %>
                                         </div> <%
            End If
           
        Else
            %>
             <div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4app');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>
           <div id='bigprint'><%
                                  If Request.Form("chkall") <> "" Then
                                      mkform_all()
                                  Else
                                      mkform()
                                  End If
                                        %>
                                         </div> <%
 
        End If%>
</body>
</html>
