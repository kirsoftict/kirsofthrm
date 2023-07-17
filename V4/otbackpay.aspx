<%@ Page Language="VB" AutoEventWireup="false" CodeFile="otbackpay.aspx.vb" Inherits="otbackpay" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<%
    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim fl As New file_list
    Dim sec As New k_security
    Dim t1, t2 As Date
    t1 = Now.ToLongTimeString
    
    ' Response.AppendHeader("Content-Disposition", "attachment; filename=Error.xls")
    'Response.ContentType = "application/ms-excel"
    If IsError(Session("con")) = True Then
        Response.Redirect("logout.aspx?msg=session_expire")
    End If
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


    <link href="css/payrol.css" media="screen" rel="stylesheet" type="text/css" />
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

	    Dim projnamex As String
	    Dim secx As New k_security%>
 <script type="text/javascript">
// var stime=Date.now();
<% if Request.QueryString("month")<>"" then
response.write("var month=" & chr(34) & request.querystring("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.querystring("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & secx.dbstrtohex(request.querystring("projname")) & chr(34) & ";")
projnamex=request.querystring("projname")

else
response.write("var month=" & chr(34) & request.form("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.form("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & secx.dbstrtohex(request.form("projname")) & chr(34) & ";")
projnamex=request.form("projname")
end if 
response.write("var ref=" & chr(34) & request.querystring("ref") & chr(34) & ";")

%>

  
 var paymth="<% response.write(request.querystring("paymth")) %>";
 //alert(document.referrer.toString())
 var ppname="<% response.write(sec.dbstrtohex(projnamex)) %>";

   
  
 
 
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

        <div id="tim"></div>
         <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
<form id='frmx' name='frmx' method="post" style='display:none'></form>
  <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="1;2;3" />
    
</form>


  
<div id="st"></div>
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt;">OT Back Payment <form name="frmlistout" action="" method="post" style="font-size:12pt;">
        Payroll Calendar: <select id="month" name="month">
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
          Paid Status<select id="paidst" name="paidst">
         <option value="unpaid">Un-paid</option>
         <option value="paid">Paid</option>
         </select>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submittopot();" />
    </form>
    </div>
    <div id='ppaytop' style="display:none; height:28px; font-size:9pt;">
     <div id='del' style="display:none;float:left;">
                <form id='frmdel' name='frmdel' action="" method="post">
    <input type="hidden" id='delpass' name='delpass' value='' />
    <input type="button" value='Delete' id='btndel' name='btndel' onclick="javascript: if(confirm('are you sure, Do you want delete all the Payroll data')==true){ deleted();}" style="height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; font-size:9px;" />
</form>
                </div>
                     
      <div style="width:5px; float:left;"></div> 
      <div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4ot');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>     
    <div style="width: 5px; float: left;">
    </div>
        <div id="Span2"  style="width:100px; height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA3');"><img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A3</div>        
        <div style="width: 5px; float: left;">
        </div>
        
<div id='mkstx' style=" height:28px; width:130px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; display:none; text-align:center; vertical-align:middle; float:left; margin-bottom:auto;" onclick="javascript:findid2ot('<%  response.write(request.querystring("paymthd")) %>','<%response.write(secx.dbstrtohex(projnamex)) %>')"> 
<span style="margin-top:30px; margin-bottom:auto; vertical-align:middle;">
<img src='images/gif/work.gif' alt="Work" height="28px" style="float:left;"/>
Make Statment</span></div> 
       <div style="width: 5px; float: left;">
        </div> <div id='expxls'  style=" display:none; float:left; width:150px; height:28px;">
        <%
            Dim loc As String = Server.MapPath("download") & "\" & Now.Ticks & ".txt"
           loc = loc.Replace("\", "/")
             %>
          
        
        <div class="clickexp" style=" float:left; border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " onclick='javascript:exportx("payrol-ot-back(<%response.write(now.day.tostring & now.month.tostring & now.year.tostring) %>)","xls","<%response.write(loc) %>","export","1;2;3");' >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="excel" /> Export to Excel</div> 
   </div></div>
     <%                 
                If Request.QueryString("del") = "on" Then
             deleteallx(Request.QueryString("ref"))
                End If%>
                <div style=" clear:left;"></div>
                
     <form name="frmpay" id="frmpay" action="" method="post">
     
     <input type="hidden" name="nextpage" id="nextpage" />

            
           
               <div id="bigprint">
    
             
     
   <% 
       
       Dim cod As String
       Dim javas As String = ""
       cod = ""
       Dim flgdel As Boolean = False
       Dim paidflg As Boolean = False
       If Request.QueryString("prid") <> "" Then
           cod = Me.makeformpaidx()
          
       ElseIf Request.QueryString("prdel") <> "" Then
          
           cod = Me.makeformpaidxdel()
       ElseIf Request.QueryString("first") = "" Then
          
          
           If Request.Form("res") = "resign" Then
              
                %>
                   
                    <script language="javascript" type="text/javascript">

                        $("#mkstx").css({ display: 'inline' });
                        // $("#ppaytop").css({display:'inline'});
</script>
                   <%
           Else
                       If Request.Form("paidst") = "paid" Or Request.QueryString("paidst") = "paid" Then
                         
                           cod = Me.makeformpx 'make paid list menu
                  
                           flgdel = True
                       Else
                           javas = "mkst"
                           cod = Me.makeformunpaid 'unpaid list
                  
                 
                       End If
               End If
          
              
               
         
               Else
                   
                   Dim arrx() As String
                   arrx = Me.getids
                   ReDim Preserve arrx(UBound(arrx) + 1)
                   arrx(UBound(arrx)) = "xxx"
           
                   '  Response.Write("innn")
                   cod = makeform2(arrx)
                   paidflg = True
       End If
   
       If String.IsNullOrEmpty(cod) = False Then
                   ' cod = sec.StrToHex(cod)
                   File.WriteAllText(loc, "1;2;3" & Chr(13) & cod)
               ' Response.Write(cod)
       End If
       ' cod = sec.StrToHex(cod)
        %>
        </div>
       
        <%  If javas = "mkst" Then
                %>
                   
                    <script language="javascript" type="text/javascript">

                        $("#mkstx").css({ display: 'inline' });
                        // $("#ppaytop").css({display:'inline'});
</script>
                   <%
            End If
            
            If paidflg = True Then
                Dim xprint As String = ""
                       Dim pppd As String
                       Dim ds As New dbclass
                       Dim pid, ref, count, projid, sql, flg1, spl() As String
                       pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
                       If pid.ToString = "None" Then
                           sql = "insert into payrol_reg values('" & Request.QueryString("month") & "','" & Request.QueryString("year") & "')"
                           flg1 = ds.save(sql, Session("con"), Session("path"))
                           If flg1.ToString = "1" Then
                               pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
                           Else
                               pid = "unknown" '
                           End If

                       End If
                       If pid <> "unknown" Then
                           count = fm.getinfo2("select count(id) from payrollx where pr=" & pid, Session("con"))
                           If Request.QueryString("projname") <> "" Then
                               spl = sec.dbHexToStr(Request.QueryString("projname")).Split("|")
                               If spl.Length > 1 Then
                                   projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                               Else
                                   projid = ""
                               End If
                           End If
                           ' ref = "NP-" & pid & "-" & projid & "-" & count & "-" & Request.QueryString("month") & Request.QueryString("year") & "-" & Session("userid")
                           ref = "NO-" & projid & "-" & count & "-" & Session("who")
                       Else
                           ref = "NO-" & projid & "-" & Now.ToString("yymmddHms") & "-" & Session("who")  ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
                       End If
                       xprint &= "payroll Date<input type='text' name='paydx' id='paydx' value='" & pdate1g.ToShortDateString & "' /><input type='text' name='ref' id='ref' value='" & ref & "' style='visibility:hidden'/>Pay Method:" & Chr(13) & _
                            "<select name='paymth' id='paymth' onchange=" & Chr(34) & "javascript:bankchg();" & Chr(34) & ">" & _
                                              "<option value='Bank'>Bank</option>" & Chr(13) & Chr(13) & _
                                               "<option value='Cheque'>Cheque</option>" & Chr(13) & Chr(13) & _
                                        "<option value='Cash'>Cash</option></select>" & Chr(13)
                
                       xprint &= "<span id='bkname'><select name='bankname' id='bankname'><option value=''>Bank Name</option>" & fm.getoption("tblbanks", "abr", "bank_name", Session("con")) & "</select></span>"
                       xprint &= "<script language='javascript' type='text/javascript'>$('#bankname').val('CBE');//sumcolx(); " & Chr(13) & _
           "$(function() {$( '#paydx').datepicker({changeMonth: true,changeYear: true,maxDate:'+1M'	});" & Chr(13) & " $( '#paydx' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                    "Paid Date:<input type='text' name='ppdate' id='ppdate' value='" & _
                    Today.ToShortDateString & "'>" & _
                               "<script language='javascript' type='text/javascript'>$('.ref').text('" & ref & "');//sumcolx(); " & Chr(13) & _
           "$(function() {$( '#ppdate').datepicker({changeMonth: true,changeYear: true,maxDate:'+1M'	});" & Chr(13) & " $( '#ppdate' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                    "<input type='button' id='post' onclick='javascript:findidot()' name='post' value='Paid/Payroll Date' />"

                       Response.Write(xprint)
                   End If%>
        </form>
        
       <span id="print"  style="width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');"><img src='images/ico/print.ico' alt="print"/>print A4</span> &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;        
       <span id="Div1"  style="width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA3');"><img src='images/ico/print.ico' alt="print"/>print A3</span>        

<%  If cod <> "" Then%>
        <script language="javascript" type="text/javascript">

$("#ppaytop").css({ display: 'inline' });
            $("#expxls").css({ display: 'inline' });
            
            // $("#ppaytop").css({display:'inline'});
</script>
   <% End If
    %>
<%  If flgdel = True Then%>
<script language="javascript" type="text/javascript">
    $("#del").css({ display: 'inline' });
    $("#bigprint").css({ display: 'inline' });
</script>

<%  End If
    t2 = Now.ToLongTimeString
    
    Dim pdate As Date
    If Request.QueryString("prid") <> "" Then
        pdate = CDate(fm.getinfo2("select pddate from payrollx where  ref='" & Request.QueryString("prid") & "'", Session("con")))
    End If
    %>
<script type="text/jscript">
$("#tim").text("<% response.write("Loading Time: " & t2.subtract(t1).seconds.tostring & "Secs.") %>");
$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();<% If Request.QueryString("prid") <> "" Then%> 
showMessage("<% response.write("Paid on: " & MonthName(pdate.Month).ToString & " " & pdate.Day & ", " & pdate.Year.ToString)%>","tb1");
<%end if %>
</script>

</body>
</html>
