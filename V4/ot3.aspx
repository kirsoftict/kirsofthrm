<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ot3.aspx.vb" Inherits="ot3" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  
    If Session("con").state = ConnectionState.Open Then
        ' Response.Write("it is open!!!!!")
      
    Else
        If Session("username") = "" Then
            Response.Redirect("login.aspx")
        Else
            Response.Write("was closed")
            Session("con").open()
        End If
       
        
        
    End If
  
    Session.Timeout = "60"
        Dim namelist As String
        Dim fm As New formMaker
        Dim fl As New file_list
        Dim sec As New k_security
        Dim projname As String
        Dim projid As String
        Dim spl() As String
        Dim t1, t2 As Date
        Dim pdate1, pdate2 As Date
        pdate1 = "#1/1/1900#"
        pdate2 = "#1/1/1900#"
        t1 = Now
    '  Response.Write("start: " & t1.ToString)
        ' Response.Write(String.IsNullOrEmpty(Session("pprroojj")))
        If String.IsNullOrEmpty(Session("pprroojj")) Then
        
            Session("pprroojj") = ""
            If Request.Form("projname") <> "" Then
                If Request.Form("projname") <> Session("pprroojj") And Request.Form("month") & "/1/" & Request.Form("year") <> Session("whn") Then
                    Session("pprroojj") = Request.Form("projname")
                    Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate1 = Session("whn")
                    pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
           
                End If
            End If
        
        Else
            If Request.QueryString("sub") = "" Then
            
                If Request.Form("projname") <> Session("pprroojj") And Request.QueryString("task") = "" Then
                    If Request.Form("projname") <> "" Then
                        Session("pprroojj") = Request.Form("projname")
                        Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                        pdate1 = Session("whn")
                        pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
                    End If
                ElseIf Request.Form("month") <> "" Then
                    If Request.Form("month") & "/1/" & Request.Form("year") <> Session("whn") Then
                        Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                        pdate1 = Session("whn")
                        pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
                    End If
                Else
                
                    pdate1 = Session("whn")
                    pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
    
            End If
            Else
                ' Response.Write(Request.QueryString("sub"))
                Session("pprroojj") = ""
                Session("whn") = "#1/1/0001#"
            End If
        End If
        If IsError(Session("whn")) = False And Session("whn") <> "" Then
            ' Session("pprroojj") = ""
            ' Session("whn") = "#1/1/0001#"
            pdate1 = Session("whn")
            pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
    
        
    End If
   
    ' If pdate1.ToShortDateString <> CDate(Session("whn")).ToShortDateString Then
       
    ' End If
   
    ' Response.AppendHeader("Content-Disposition", "attachment; filename=Error.xls")
    'Response.ContentType = "application/ms-excel"
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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
    <script src="jqq/ui/jquery.ui.tabs.js"></script>
	
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>


    <link href="css/payrol.css" media="screen" rel="stylesheet" type="text/css" />
    <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
    <script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>
   <% 
      Dim keyp As String = ""
      If Request.QueryString("dox") = "edit" Then
          keyp = "update"
      ElseIf Request.QueryString("dox") = "delete" Then
          keyp = "delete"
      Else
          keyp = "save"
      End If
 
      Dim emp_id, emptid As String
      Dim arrx() As String = {"Reg", "Nig", "WE", "HD"}
      bee()
      Dim idx As String = ""
      idx = Request.QueryString("id")
      Dim msg As String = ""
      Dim flg As Integer = 0
      Dim flg2 As Integer = 0
      
      Dim rd As String = ""

      Dim tbl As String = ""
      Dim key As String = ""
      Dim keyval As String = ""
      tbl = Request.QueryString("tbl")
      key = Request.QueryString("key")
      rd = Request.QueryString("rd")
       ' namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", session("con"), " ")


 %>
    <style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}
        .style1
        {
            width: 163px;
        }
        .style2
        {
            width: 44px;
        }
    </style>
    <title></title>
    	<%  	   
    	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
	
    	    
 %>
 <script type="text/javascript">
// var stime=Date.now();
<% 
if Request.QueryString("month")<>"" then
response.write("var month=" & chr(34) & request.querystring("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.querystring("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.querystring("projname")) & chr(34) & ";")

else
response.write("var month=" & chr(34) & request.form("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.form("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.form("projname")) & chr(34) & ";")

end if 

%>

  
 var paymth="<% response.write(request.querystring("paymth")) %>";
 //alert(document.referrer.toString())
 var ppname="<% response.write(sec.dbStrToHex(request.querystring("projname"))) %>";
 
   
  
 var nameproj=[<% response.write(namelist) %>];
 
    $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  nameproj, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});

  <% 
 
  if session("pprroojj")<>"" then
    spl=session("pprroojj").split("|")
    if spl.length>1 then
        projname=spl(0)
        projid=spl(1)
        Dim listinproj As String = fm.getprojemp(projid, pdate2, Session("con"))
        dim sql as string
       ' response.write("//" & session("pprroojj"))
       sql=" emp_static_info as esi inner join emprec on esi.emp_id=emprec.emp_id " & _
         "where emprec.id " & _
         "in" & _
         "(" & listinproj & ")"
         namelist = fm.getjavalist2(sql, " esi.first_name,esi.middle_name,esi.last_name", session("con"), " ")
     response.write("/*" & sql & "*/")
    else
    end if
    end if
    if namelist<>"" then
    
%>
var namelist=[<% response.write(namelist) %>];
<% end if %>

 var prv;
  prv="";
var id;
var focused="";
var requf=["ot_date","time_diff","factored","datepaid","x"];
var fieldlist=["emp_id","emptid","ot_time","ot_end","description","datepaid","who_reg","date_reg","x"];
function validation1(){
if ($('#vname').val() == '') {showMessage('vname cannot be empty','vname');$('#vname').focus();return false;}
if ($('#ot_date').val() == '') {showMessage('OT Date cannot be empty','datework');$('#ot_date').focus();return false;}
if ($('#factored').val() == '') {showMessage('factored cannot be empty','factored');$('#factored').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
  $("#btnsave").attr("disabled",true);
   var str=$("#frmemp_ot").formSerialize();
   $("#frmemp_ot").attr("action","?tbl=emp_ot&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_ot").submit();
     $("#btnsave").attr("disabled",false);
  return true;
  }
  }
} 
   function edit(val,date1,date2,fulname)
   {
         $("#pay").css({ width: '900px', height: '500px' });
          $("#fpay").css({ width: '900px', height: '500px' });
         //alert(whr + fn.toString());
         $('#frmx').attr("target", "fpay");
         $("#fpay").attr("frameborder", "0");

         $('#frmx').attr("action", "grid.aspx?task=edit&sql=" + val + "&tbl=emp_ot&page=ot3.aspx&fname="+ fulname);
       // alert("grid.aspx?task=edit&sql" + val + "&tbl=emp_ot");
       
                $('#pay').css({ top: '0px', left: '0px' });
         $("#pay").remove("display"); 
        
         $("#pay").dialog({
             height: 300,
             width: 800,
             modal: true,
             buttons: {

'Refresh and Close': function() {
$('#frmx').attr("target", "frm_tar");
        // $("#fpay").attr("frameborder", "0");
      
         $('#frmx').attr("action", "ot3.aspx?chkupdate=on");
      
         $('#frmx').submit();
       
$(this).dialog('close');
// Update Rating
}
}
         }); 
           $('#frmx').submit();
   
   }
  
	$(function() {
		$( "#tabs" ).tabs();
	});

   
    </script>
    

	 <% 'response.write(namelist) %>
</head>
<body>

<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

<div style='clear:both;'></div>
       
    
         <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' frameborder='0' src='' scrolling='no'></iframe>"))
%>

<form id='frmx' name='frmx' method="post"></form>
  
<div id="st"></div>
<div style="width:100%; height:40px; background:#6879aa; text-align:center;color:White; font-size:13pt; padding-top:10px;">

    <form name="frmlistout" id='frmlistout' action="ot3.aspx" method="post" style="font-size:12pt;">
   OT Registration     Month: <select id="month" name="month">
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
       
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:$('#frmlistout').submit();" />
    </form>
    </div>
    <%
    
    
    If Session("pprroojj") <> "" And Session("whn") <> "" Then%>


 <%   pdate1 = Session("whn")
     pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
     ' Response.Write("<br>" & getprojemp(projid, pdate2, Session("con")))
    %>
       <% If Request.QueryString("chkupdate") = "on" Then
          ' updatedb()
           updatedb(Session("pprroojj"), pdate1, pdate2)
       End If%>
    <div id="tabs">
  <ul>
    <li><a href="#tabs-1">Add Data and Edit</a></li>
    <li><a href="#tabs-2">All paid list in the month</a></li>
    <li><a href="#tabs-3">Paid in the Current month</a></li>
  </ul>
  <div id="tabs-1">

<div id="formouterbox" style="width:600px; float:left">
 
    
    <div id="forminner">
    <span id="messageboxx"></span>
   <b> This Entry for: <% Response.Write(Session("pprroojj") & " and Month of :" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString) %></b>
<form method='post' id='frmemp_ot' name='frmemp_ot' action=""> 
<table width="700px"><tr>
<td>Search by Name<sup style='color:red;'>*</sup></td><td>:</td><td colspan='5'>
<input type="hidden" name="id" id="id" />
<input type='text' name='vname' id='vname' style='font-size:9pt;' onkeyup="javascript:startwith('vname',namelist);" /><!--input type="checkbox" value='hasname' id='takename' name='takename' />Take name to the next Entery--></td>
</tr><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<%response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
OT Date<sup style='color:red;'>*</sup></td><td>:</td>
<td class="style1"><input type='text' id='ot_date' name='ot_date' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>    $(function () { $("#ot_date").datepicker({ changeMonth: true, changeYear: true }); $("#ot_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
<td class="style2">OT Paid</td><td>:</td>
<td><input type='text' id='datepaid' name='datepaid' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>    $(function () { $("#datepaid").datepicker({ changeMonth: true, changeYear: true }); $("#datepaid").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
</tr>

<tr><td colspan="4">
   
    <fieldset style="border:1px gray solid; width: 275px;">
<table style="width:250px;"><tr><td>
  Regular Hrs.</td><td>:</td><td><input type='text' id='Reg' name='Reg' value='' size="2" /></td></tr>
  <tr><td>
  Night Hrs.</td><td>:</td><td><input type='text' id='Nig' name='Nig' value='' size="2" /></td></tr>
  <tr><td>
  Sunday(Weekends) Hrs.</td><td>:</td><td><input type='text' id='WE' name='WE' value='' size="2" /></td></tr>
  <tr><td>
  Public Holiday Hrs.</td><td>:</td><td><input type='text' id='HD' name='HD' value='' size="2" /></td></tr>
</table></fieldset>
</td><td colspan='4'><textarea id='remark' name='remark' cols='40' onfocus="javascript:if($('#remark').text()=='Remark' ||$('#remark').text()=='remark' ) $('#remark').text('');" onblur="javascript:if($('#remark').text()=='') $('#remark').text('Remark');" rows='10' style='overflow:hidden;'>Remark</textarea></td></tr>
    
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
<tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr>
</table>
</form>
</div>
    <sup style="color:Red;">*</sup>Required Fields
    
 </div> 
 <% viewforedit(Session("pprroojj"),pdate1,pdate2) %>

    <div style='float:left;'>
    <asp:Literal ID='outp' runat="server"></asp:Literal>
    </div>
 <%' end of data entry %>
   </div>
  <div id="tabs-2">
<%
     viewpaid(Session("pprroojj"), pdate1, pdate2)%>
     
    <div>
    <asp:Literal ID='outp2' runat="server"></asp:Literal></div>  </div>
  <div id="tabs-3">
<% paidlistp(Session("pprroojj"), pdate1, pdate2)%>
    <div>
    <asp:Literal ID='outp3' runat="server"></asp:Literal></div>
    </div>  </div>

    <% ' Registration form %>

<% Else
    Response.Write("<b>Please select Project and date</b>")
    End If%>


<%  ' end registration 
    If Session("con").state = Data.ConnectionState.Closed Then
        'Response.Write("database is closed")
        Session("con").open()
    End If
    If Session("con").state = ConnectionState.Open Then
        ' Response.Write("<br>end of it!!!!")
        ' Session("con").close()
        
    End If
    t2 = Now
    'Response.Write(" End time:" & t2.ToString)
    Dim timout As String = ""
    timout = t2.Subtract(t1).Minutes & "Mins " & t2.Subtract(t1).Seconds & "Secs"
    Response.Write(timout)
    %>

<div style='float:left;width:1%;'>&nbsp;</div>


    
    

</body>
</html>
<script> 
    </script>