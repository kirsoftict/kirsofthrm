<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reportaprisal.aspx.vb" Inherits="reportaprisal" %>
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
	
	<link rel="stylesheet" href="jqq/demos/demos.css">
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
 $(function() {
		$( "#radio" ).buttonset();
	});
        function showapp(vax)
        {
       
            $(".g1").css('display','none');
            $("#"+vax).css("display","inline");
        
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

  
   	   function exportxx(fname, ftype, floc, frmname, rigt) {
            //alert(floc);
            $("#" + frmname).attr("action", "print.aspx?exporttax=on");
            $("#filetype").val(ftype);
            $("#fileloc").val(floc);
            $("#filename").val(fname);
            $("#rigt").val(rigt)
            $("#" + frmname).submit();
            //alert(floc);
        }
       
    </script>
    <script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>
    <style>
     .year .p_group label{
	display: block;
	float: left;
	margin-top: -19px;
	background: #FFFFFF;
	height: 14px;
	padding: 2px 5px 2px 5px;
	color: #B9B9B9;
	font-size: 12px;
	overflow: hidden;
	font-family: Arial, Helvetica, sans-serif;
}

    </style>

</head>
<body>
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

       
    
         <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
  <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form>
    <div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:20px;">
   Appraisal Report
    <form name="frmlistout" action="" method="post" style="font-size:12pt;">
           
      
        <script type="text/javascript">
            $("#month").val("<% response.write(today.month) %>");
        </script>
       <!--  Appraisal Period:<select id="year" name="year">
         <% Dim ds As New dbclass
             Dim dtf As String
                Dim rs As DataTableReader
               ' rs = ds.dtmake("dtmk", "select distinct per_from,per_end from emp_apprisal", Session("con"))
            ' If rs.HasRows Then
            '     Response.Write("<option value=''>Select Period</option>")
             '    While rs.Read
              '       dtf = MonthName(CDate(rs(0)).Month, True) & " " & CDate(rs(0)).Year.ToString & " - " & MonthName(CDate(rs(1)).Month, True) & " " & CDate(rs(1)).Year.ToString
               '      Response.Write("<option value='" & CDate(rs(0)).ToShortDateString & "-" & CDate(rs(1)).ToShortDateString & "'>" & dtf & "</option>")
               '  End While
           '  Else
           '      Response.Write("<option value=''>No Data</option>")
           '  End If
           '     rs.Close()
              '  ds = Nothing
               
                
                %>
           
        </select-->
        <div id="radio">
     <input type="radio" id="Radio1" name="selectb" value="aperiod" checked="checked" onclick="javascript:showapp(this.value);" />
     <label for="Radio1"> Single Period</label>
      <input type="radio" id="Radio2" name="selectb" value="p_gg" onclick="javascript:showapp(this.value);" />
      <label for='Radio2'>Summery Period</label></div>
            <span class='g1' id='aperiod'> <label for="year"> Appraisal Period:</label><select id="year" name="year">
         <%  Dim title As String = ""
             Dim appd As String = ""
             rs = ds.dtmake("dtmkx", "select distinct per_end from emp_apprisal order by per_end desc", Session("con"))
             If rs.HasRows Then
                 Response.Write("<option value=''>Select Period</option>")
                 While rs.Read
                     appd = fm.getinfo2("select period from apr_period where period_end='" & rs.Item("per_end") & "'", Session("con"))
                     If appd <> "None" Then
                         title = Format(rs.Item("per_end"), "MMM d, yyyy") & "-" & rs.Item("per_end")
                         Response.Write("<option title='" & title & "' value='" & CDate(rs(0)).ToShortDateString & "'>" & appd & "</option>")
              
                     End If
                 '    dtf = MonthName(CDate(rs(0)).Month, True) & " " & CDate(rs(0)).Year.ToString & " - " & MonthName(CDate(rs(1)).Month, True) & " " & CDate(rs(1)).Year.ToString
                     '  Response.Write("<option value='" & CDate(rs(0)).ToShortDateString & "-" & CDate(rs(1)).ToShortDateString & "'>" & dtf & "</option>")
                 End While
             Else
                 Response.Write("<option value=''>No Data</option>")
             End If
                rs.Close()
            
               
                
                %>
           
        </select></span>
        <span class='g1' id='p_gg' style='display:none'>
           <label for="p_group"> Appraisal group:</label><select id="p_group" name="p_group"> 
         <%  title = ""
             appd = ""
             rs = ds.dtmake("dtmkx", "select distinct p_group from apr_period where p_group is not Null order by p_group desc", Session("con"))
             If rs.HasRows Then
                 Response.Write("<option value=''>Select group</option>")
                 While rs.Read
                    
                         
                     Response.Write("<option title='" & rs.Item("p_group") & "' value='" & rs.Item("p_group") & "'>" & rs.Item("p_group") & "</option>")
              
                   
                     '    dtf = MonthName(CDate(rs(0)).Month, True) & " " & CDate(rs(0)).Year.ToString & " - " & MonthName(CDate(rs(1)).Month, True) & " " & CDate(rs(1)).Year.ToString
                     '  Response.Write("<option value='" & CDate(rs(0)).ToShortDateString & "-" & CDate(rs(1)).ToShortDateString & "'>" & dtf & "</option>")
                 End While
             Else
                 Response.Write("<option value=''>No Data</option>")
             End If
             rs.Close()
             ds = Nothing
               
                
                %>
           
        </select></span>
         Enter Project: <input type="text" name="projname" id="projname" />
          <select id="res" name="res" style='visibility:hidden;'>
         <option value="avtive">Active</option>
         <option value="resign">Resign</option>
         </select> 
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submittopapp();" />
    </form>
    </div>
    <%  If Request.Form("year") <> "" Then
            If Request.Form("projname") <> "" Then
                
           %>
               <div id='bigprint'><%
                                         mkform()%>
                                         </div> 
                                         <%
 
                                                End If
                                     ElseIf Request.Form("p_group") <> "" Then
                                         If Request.Form("projname") <> "" Then
                
           %>
               <div id='bigprint'><%
                                      mkform2()%>
                                         </div> 
                                         <%
 
                                         End If
                                     End If
            %>
             <!--div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4app');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div-->
          
</body>
</html>
