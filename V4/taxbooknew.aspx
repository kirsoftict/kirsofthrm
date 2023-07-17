<%@ Page Language="VB" AutoEventWireup="false" CodeFile="taxbooknew.aspx.vb" Inherits="taxbooknew" %>

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
   
            
            <link href="css/taxold.css" media="screen" rel="Stylesheet" type="text/css" />
            <%
          
 
		   
	    namelist = getjavalist2("tblproject", "project_name,project_id", Session("con"), "|", "$")
	

 %>
 <script type="text/javascript">
 var month;
 var year;
 month="";
 year="";
// var stime=Date.now();
<% if Request.QueryString("pd1")<>"" then
response.write("var pd1=" & chr(34) & request.querystring("pd1") & chr(34) & ";")
response.write("var pd2=" & chr(34) & request.querystring("pd2") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.querystring("projname")) & chr(34) & ";")
response.write(" month=" & chr(34) & cdate(request.querystring("pd1")).month & chr(34) & ";")
response.write(" year=" & chr(34) & cdate(request.querystring("pd1")).year & chr(34) & ";")
else if Request.form("pd1")<>"" then
response.write("var pd1=" & chr(34) & request.form("pd1") & chr(34) & ";")
response.write("var pd2=" & chr(34) & request.form("pd2") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.form("projname")) & chr(34) & ";")
response.write("month=" & chr(34) & cdate(request.form("pd1")).month & chr(34) & ";")
response.write("year=" & chr(34) & cdate(request.form("pd1")).year & chr(34) & ";")
end if 
response.write("var ref=" & chr(34) & request.querystring("ref") & chr(34) & ";")

%>

  
 var paymth="<% response.write(request.querystring("paymth")) %>";
 //alert(document.referrer.toString())
 var ppname="<% response.write(sec.dbStrToHex(request.querystring("projname"))) %>";
 
   
  
 
 

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
   function gopension(wr, val) {
                 // var year="<% response.write(request.form("year")) %>";
                 //var month="<% response.write(request.form("month")) %>";
                 /*var projname="<% response.write(Request.Form("projname")) %>";*/
                
                     $("#fpay").attr("frameborder", "0");
                     $('#frmx').attr("target", "fpay");
                     $('#frmx').attr("action", "?wr=" + wr + "&month=" + month + "&year=" + year + "&projname=" + projname + "&val="+val);
                     $('#frmx').submit();
                     $('#pay').css({ top: '0px', left: '0px' });
                     $("#pay").remove("display");
                     $("#pay").dialog({
                         height: 300,
                         width: 600,
                         modal: true
                     });
               

             }
             function gotopension(wr,list)
{
                     $("#fpay").attr("frameborder", "0");
                     //$('#frmx').attr("target", "fpay");
                     $('#frmx').attr("action", "?tax=pension&wr=" + wr + "&month=" + month + "&year=" + year + "&val="+list);
                     $('#frmx').submit();
                     $('#pay').css({ top: '0px', left: '0px' });
                     $("#pay").remove("display");
                     $("#pay").dialog({
                         height: 400,
                         width: 800,
                         modal: true
                     });
               

   
  
}
   function gototax(wr,list)
{


                     $("#fpay").attr("frameborder", "0");
                     //$('#frmx').attr("target", "fpay");
                     $('#frmx').attr("action", "?tax=tax&wr=" + wr + "&pd1=" + pd1 + "&pd2=" + pd2 + "&val="+list);
                     $('#frmx').submit();
                     $('#pay').css({ top: '0px', left: '0px' });
                     $("#pay").remove("display");
                     $("#pay").dialog({
                         height: 400,
                         width: 800,
                         modal: true
                     });
               

   
  
}
 function checkedc(lac) {

            if ($("#" + lac).is(':checked')) 
                {
                    //$("#chkall").text("Checked All");

                   // alert("chhhh");
                    $(".csssign").text(" ");

                }
               
               
            }
             function checkedc(lac) {

            if ($("#" + lac).is(':checked')) 
                {
if(lac=="signc"){
                    //$("#chkall").text("Checked All");

                   // alert("chhhh");
                    $(".csssign").text(" ");
}
else if(lac=="tinp"){    $(".tin").text(" ");  }


                }
               
               
            }

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
    <script language="javascript" type="text/javascript">
    
    
    </script>
    

	 
    <style type="text/css">

 table.MsoNormalTable
	{line-height:115%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	}
	.viewtax
	{
	    color:Black;
	    cursor:pointer;
	    
	    }
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
Payroll Tax Report
    <form name="frmlistout" action="" method="post" style="font-size:12pt;">
        Payroll Calendar: From: <input type="text" id='pd1' name='pd1' value='<% response.write(today.addmonths(-1).toshortdatestring) %>' /> to 
        <input type="text" id='pd2' name='pd2' value='<% response.write(today.toshortdatestring) %>' />
        <script>
            $(function () {
                $('#pd1').datepicker({ changeMonth: true, changeYear: true, maxDate: '+1d' });
                $('#pd1').datepicker('option', 'dateFormat', 'mm/dd/yy');
            });
            $(function () {
                $('#pd2').datepicker({ changeMonth: true, changeYear: true, maxDate: '+1d' });
                $('#pd2').datepicker('option', 'dateFormat', 'mm/dd/yy');
            });

        </script>
        <!--select id="month" name="month">
            <%  'For i As Integer = 1 To 12
                 '   Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                'Next
                
                %>
        </select>
        <script type="text/javascript">
            $("#month").val("<% 'response.write(today.month) %>");
        </script>
         <select id="year" name="year">
            <%'  For i As Integer = Today.Year To Today.Year - 9 Step -1
               '     Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                'Next%>
        </select-->
        <input type="hidden" name="projname" id="projname" value='<%=namelist %>' />
          Tax<select id="paidst" name="paidst">
                 <option value="tax">Payroll tax</option>
       
         </select>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:subptax2();" />
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
     
    </div>
              
        <div style="width: 5px; float: left;">
        </div>
    
    <div style="width: 5px; float: left;">     

 
       <div style="width: 5px; float: left; ">
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
<div style=" clear:left"></div>
   
 
   </div>
     
                <div style=" clear:left;"></div>
                
     <form name="frmpay" id="frmpay" action="" method="post">
     
     <input type="hidden" name="nextpage" id="nextpage" />

            
           
             
    
             
     
   <% 
       Dim cod As String
       cod = ""
        Dim mkst As String = ""
       Dim flgdel As Boolean = False
       Dim paidflg As Boolean = False
      
            
       Dim TT() As String
       
       If Request.Form("paidst") = "pension" Then
           cod = Me.makeformpx_payroll 'make paid list menu
       ElseIf Request.Form("paidst") = "tax" Then
           ' Response.Write("hear")
           'pay_jv(Request.Form("pd1"), Request.Form("pd2"))
           createpage("", "tax", "viewtotal")
           Response.Write("<script>$('#pinta4').css({display:'inline'});</script>")
           Response.Write("<script>$('.clickexp').css({display:'inline'});</script>")
           ' cod = Me.makeformp_tax 'unpaid list
       ElseIf Request.Form("paidst") = "chk" Then
           TT = payroll_checklist()
           For p As Integer = 0 To UBound(TT)-1
               If TT(p) <> "" Then
                   Response.Write(TT(p))
               End If
           Next
          
          
       End If
    
       'If Request.Item("tax") = "tax" Then
       'Response.Write(LCase(Request.Item("wr")))
       'createpage(Request.Item("val"), "tax", LCase(Request.Item("wr")))
       ' Response.Write("<script>$('#pinta4').css({display:'inline'});</script>")
       ' Response.Write("<script>$('.clickexp').css({display:'inline'});</script>")
       ' End If
         
         
         
   
       If String.IsNullOrEmpty(cod) = False Then
           ' cod = sec.StrToHex(cod)
           ' Response.Write(cod)
           obj = cod
           obj = "1;2;3" & Chr(13) & obj
               



           File.WriteAllText(loc, obj)
       End If
       ' cod = sec.StrToHex(cod)
        %>
        
     
        <%  If Request.QueryString("first") = "" Then
               
            End If
            
            If paidflg = True Then
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
                xprint &= "payroll Date<input type='text' name='paydx' id='paydx' value='" & pdd & "' /> Pay Method: " & _
                "<select name='paymth' id='paymth'>" & Chr(13) & _
                                              "<option value='Bank'>Bank</option>" & Chr(13) & Chr(13) & _
                                               "<option value='Cheque'>Cheque</option>" & Chr(13) & Chr(13) & _
                                        "<option value='Cash'>Cash</option></select>" & Chr(13) & _
                           "<script language='javascript' type='text/javascript'>//sumcolx(); " & Chr(13) & _
           "$(function() {$( '#paydx').datepicker({changeMonth: true,changeYear: true,maxDate:'+1M'	});" & Chr(13) & " $( '#paydx' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                    "Paid Date:<input type='text' name='ppdate' id='ppdate' value='" & _
                    Today.ToShortDateString & "' onchange=" & Chr(34) & "javascript:datechk();" & Chr(34) & ">" & _
                               "<script language='javascript' type='text/javascript'>//sumcolx(); " & Chr(13) & _
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
            $(".clickexp").css({ display: "inline" });
            $("#ppaytop").css({ display: 'inline' });
            $("#expxls").css({ display: 'inline' });
            // $("#ppaytop").css({display:'inline'});
</script>
   <% End If
    %>
<%  If flgdel = True Then%>
<script language="javascript" type="text/javascript">
    $("#del").css({ display: 'inline' })
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
<div id="cover" style="background:#ffccaa;">
 
    </div>

</body>
</html>


 <script language="javascript" type="text/javascript">
     $(document).ready(function () {
  // alert("<% response.write(request.querystring("chkpen")) %>");
  //alert(month + 'and' +year);
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
     %>
</script>
