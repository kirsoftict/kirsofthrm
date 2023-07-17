<%@ Page Language="VB" AutoEventWireup="false" CodeFile="payroll.aspx.vb" Inherits="payrollkirxx" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="Microsoft.VisualBasic"%>
<%  If Session("username") = "" Then
     %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
response.redirect("logout.aspx")
       <%  End If
           
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
    <meta http-equiv='catch-control' content='no-catch' />
    <meta http-equiv='expires' content='0' />
    <meta http-equiv='pragma' content='no-catch' />
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
	 
       
   
	<link rel="stylesheet" href="jq/demos.css" />
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
#bkname
{
    display:none;
    
    }
    
</style>
	<%  	   
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
	 %>
 <script type="text/javascript">
 function bankchg()
 {
 if($('#paymth').val()=='Bank'){ 
 $('#bkname').css({'display':'inline'});}
 else
 { 
 $('#bkname').css({'display':'none'});
 $("#bankname").val('');}
 }
// var stime=Date.now();
<% 
if Request.QueryString("month") <> "" then
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
 <%    Dim whomade As String
  If Request.QueryString("prid") <> "" Then
          
     whomade   = fm.getinfo2("select who_reg from payrollx where ref='" & Request.QueryString("prid") & "'", Session("con"))
           %>
           var accname="<%=whomade %>";
           <%
      else
      %>
      var accname="<%=session("username") %>";
      <%
      end if %>
   
  
 
 
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

  function datechk()
  {
    if($("#paydx").val()!==$("#ppdate").val())
    {
        alert("Payroll date and Pay date is not similar");
    }
  }
   
    </script>
    <script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>

	 
</head>

<body>
<%   
         Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")
           Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
<div id='tim' style='left:0px;top:0px; height:20px; position:inherit;'></div>
<form id='frmx' name='frmx' method="post" style='display:none' action=""></form>
  
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px;">Payroll Maker
    <form name="frmlistout" action="" method="post" style="font-size:12pt;">
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
       
          <select id="res" name="res" style='visibility:hidden;'>
         <option value="avtive">Active</option>
         <option value="resign">Resign</option>
         </select> Paid Status<select id="paidst" name="paidst">
         <option value="unpaid">Un-paid</option>
         <option value="paid">Paid</option>
         </select>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submittop();" />
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
      <div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg)
           #224488;color:White;cursor:pointer;float:left;display:none;"
            onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>   
       <div id="spanview"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg)
           #224488;color:White;cursor:pointer;float:left;" title='Please select the date and click on paid button'>
      <img src='images/png/printer2dis.png' alt="print" height="28px" style="float:left;"/>print A4 </div>   
    <div style="width: 5px; float: left;">
    </div>
        <!--div id="Span2"  style="width:100px; height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA3');"><img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A3</div-->        
        <div style="width: 5px; float: left;">
        </div>
        
<div id='mkstx' style=" height:28px; width:130px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; display:none; text-align:center; vertical-align:middle; float:left; margin-bottom:auto;" onclick="javascript:findid2('<%  response.write(request.querystring("paymthd")) %>','<%response.write(Request.querystring("projname")) %>')"> 
<span style="margin-top:30px; margin-bottom:auto; vertical-align:middle;">
<img src='images/gif/work.gif' alt="Work" height="28px" style="float:left;"/>
Make Statment</span></div> 
 
       <div style="width: 5px; float: left;">
      
        </div> 
        <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form> <%
           Dim obj As Object
           
           Dim loc As String = Server.MapPath("download") & "\operatorview.txt"
           loc = loc.Replace("\", "/")
           'Response.Write(loc)
    
           %>
          
        <div class="clickexp" style=" float:left; border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; display:none;" onclick='javascript:exportx("payrol(<%response.write(now.day.tostring & now.month.tostring & now.year.tostring) %>)","xls","<%response.write(loc) %>","export","2;3");' >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="excel" /> Export to Excel</div>
 
   </div>
     <%                 
         If Request.QueryString("del") = "on" Then
             Response.Write("del on")
             deleteallx_payroll(Request.QueryString("ref"))
         End If%>
                <div style=" clear:left;"></div>
                
     <form name="frmpay" id="frmpay" action="" method="post">
     
     <input type="hidden" name="nextpage" id="nextpage" />

            
           
               <div id="bigprint" style='font-size:8pt;'>
    
             
     
   <% 
       Dim cod As String
       cod = ""
        Dim mkst As String = ""
       Dim flgdel As Boolean = False
       Dim paidflg As Boolean = False
       If Request.QueryString("prid") <> "" Then
          
           '   Dim whomade As String = fm.getinfo2("select who_reg from payrollx where ref='" & Request.QueryString("prid") & "'", Session("con"))
           
           Response.Write("<script src='jsfixed/demo/demo.js'></script>")
           cod = Me.makeformpaidx_payroll()
           %><script type="text/javascript">
               $("#Span1").css({ display: 'inline' });
         $("#spanview").css({ display: 'none' });
         </script><%
       ElseIf Request.QueryString("prdel") <> "" Then
           'Response.Write("called")
           cod = Me.makeformpaidxdel_payroll()
       ElseIf Request.QueryString("first") = "" Then
            
           
           If Request.Form("res") = "resign" Then
               ' cod = Me.makeform3_payroll
                %>
                   
                    <script language="javascript" type="text/javascript">
        
        $("#mkstx").css({display:'inline'});
       // $("#ppaytop").css({display:'inline'});
</script>
                   <%
           Else
               If Request.Form("paidst") = "paid" Or Request.QueryString("paidst") = "paid" Then
                           cod = Me.makeformpx_payroll 'make paid list menu
                          
                           
                   flgdel = True
               Else
                          
                           cod = Me.makeformunpaid_payroll 'unpaid list
                           mkst = "on"
                   %>
                   
                  
                   <%
                 
               End If
               End If
          
              
            
         
           Else
               
               Dim arrx() As String
               arrx = Me.getids_payroll
               ReDim Preserve arrx(UBound(arrx) + 1)
               arrx(UBound(arrx)) = "xxx"
           
               '  Response.Write("innn")
               cod = makeform2_payroll(arrx)
               paidflg = True
       End If
   
       If String.IsNullOrEmpty(cod) = False Then
              ' cod = sec.StrToHex(cod)
               ' Response.Write(cod)
               obj = cod
               obj = "1;2;3" & Chr(13) & obj
               



               File.WriteAllText(loc, obj)
       End If
       ' cod = sec.StrToHex(cod)
        %>
        </div>
     
        <%  If Request.QueryString("first") = "" Then
               
            End If
           
            If paidflg = True Then
                Dim ds As New dbclass
                Dim pid, ref, count, projid, sql, flg1, spl() As String
                pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
                If pid.ToString = "None" Then
                    sql = "insert into payrol_reg values('" & Request.QueryString("month") & "','" & Request.QueryString("year") & "')"
                    flg1 = ds.excutes(sql, Session("con"), Session("path"))
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
                    ref = "NP-" & projid & "-" & pid & count & "-" & Session("who")
                Else
                    ref = "Np-" & projid & "-" & Now.ToString("yymmddHms") & "-" & Session("who")
                End If
                Dim xprint As String = ""
                Dim m, y, d, pdd As String
                m = Request.QueryString("month")
                y = Request.QueryString("year")
                d = Date.DaysInMonth(y, m)
                If m <> "" Then
                    pdd = m & "/" & d & "/" & y
                Else
                    pdd = ""
                End If
                ' Response.Write(Request.QueryString("month") & "/" & Request.QueryString("year"))
                ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
                xprint &= "payroll Date: " & pdd & "<input type='text' name='paydx' id='paydx' value='" & pdd & "' style='visibility:hidden'/> <input type='text' name='ref' id='ref' value='" & ref & "' style='visibility:hidden'/>Pay Method: " & _
                "<select name='paymth' id='paymth' onchange=" & Chr(34) & "javascript:bankchg();" & Chr(34) & ">" & _
                                              "<option value='Bank'>Bank</option>" & Chr(13) & Chr(13) & _
                                               "<option value='Cheque'>Cheque</option>" & Chr(13) & Chr(13) & _
                                        "<option value='Cash'>Cash</option></select>" & Chr(13)
                
                xprint &= "<span id='bkname'><select name='bankname' id='bankname'><option value=''>Bank Name</option>" & fm.getoption("tblbanks", "abr", "bank_name", Session("con")) & "</select></span>"
                xprint &= "<script language='javascript' type='text/javascript'>$('#bankname').val('CBE');//sumcolx(); " & Chr(13) & _
           "$(function() {$( '#paydx').datepicker({changeMonth: true,changeYear: true,maxDate:'+1M'	});" & Chr(13) & " $( '#paydx' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & " if($('#paymth').val()=='Bank'){ $('#bkname').css({'display':'inline'});}</script>" & Chr(13) & _
                    "Paid Date:<input type='text' name='ppdate' id='ppdate' value='" & _
                    Today.ToShortDateString & "' onchange=" & Chr(34) & "javascript:datechk();" & Chr(34) & ">" & _
                               "<script language='javascript' type='text/javascript'>$('.ref').text('" & ref & "');//sumcolx(); " & Chr(13) & _
           "$(function() {$( '#ppdate').datepicker({changeMonth: true,changeYear: true,maxDate:'+1M'	});" & Chr(13) & " $( '#ppdate' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>"
                xprint &= "<input type='button' id='post' onclick='javascript:findid()' name='post' value='Paid/Payroll Date' />"
         
                If pdd = "" Then
                    xprint &= "<script language='javascript' type='text/javascript'>alert('check payroll date, some error is happend on date. Pls put the end date of the month!');</script>"
                End If
                    

                Response.Write(xprint)
            End If%>
        </form>
        
       <!--span id="print"  style="width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');"><img src='images/ico/print.ico' alt="print"/>print A4</span> &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;        
       <span id="Div1"  style="width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA3');"><img src='images/ico/print.ico' alt="print"/>print A3</span-->        

<%  If cod <> "" Then%>
        <script language="javascript" type="text/javascript">
            $(".clickexp").css({display:"inline"});
        $("#ppaytop").css({display:'inline'});
        $("#expxls").css({display:'inline'});
       // $("#ppaytop").css({display:'inline'});
</script>
   <% End If
    %>
<%  If flgdel = True Then%>
<script language="javascript" type="text/javascript">
    $("#del").css({display:'inline'})
</script>

<%  End If
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
<div id="cover" style="background:#ffccaa;"></div>
<footer><% response.Write(whomade) %></footer>
</body>
</html>


 <script language="javascript" type="text/javascript">
     $(document).ready(function () {
  // alert("<% response.write(request.querystring("chkpen")) %>");
     <% if mkst="on" then %>
         $("#mkstx").css({ display: 'inline' });
         <%end if %>
     });
     // $("#ppaytop").css({display:'inline'});
   //  showobj("bigprint","cover",0,0);
  //   $("#cover").css({hieght:findh(document.getElementById("bigprint"))+"px",width:"100px"});
    // alert(findh(document.getElementById("bigprint")));
    <% if session("pstop")=true then%>
    $("#pensionstop").css({display:'inline'});
    <%end if
     dim fx() as string
     If String.IsNullOrEmpty(Session("right")) = False Then
                            fx = Session("right").split(";")
                            ReDim Preserve fx(UBound(fx) + 1)
                            fx(UBound(fx) - 1) = ""
                        End If
                        If fm.searcharray(fx, "1") = False Or fm.searcharray(fx, "9") = False Then
                            Response.Write(" $('.deletepayrol').attr('disabled',false);$('.viewdelpayrol').attr('disabled',true);")
                            
                        End If
    
    
     %>
</script>