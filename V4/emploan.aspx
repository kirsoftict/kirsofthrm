<%@ Page Language="VB" AutoEventWireup="false" CodeFile="emploan.aspx.vb" Inherits="emploan" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%     Dim rm As Integer
    Dim t As Integer
    Dim keyp As String = ""
    Dim fm As New formMaker
    Dim emp_id, emptid As String
    emptid = ""
    If emptid.ToString = "" Then
        emptid = "0"
    End If
    Dim projid() As String
    Dim pdate1, pdate2 As Date
    Dim idsin As String
    If Request.Item("projname") <> "" Then
        ' Response.Write("IIIIIIIIIIIIN")
        If Request.QueryString("projname") <> Session("projname") Or Request.QueryString("month") <> Session("month") Or Request.QueryString("year") <> Session("year") Then
            Session("projname") = Request.Item("projname")
            Session("month") = Request.Item("month")
            Session("year") = Request.Item("year")
        End If
    End If
    If Session("projname") <> "" Then
        projid = Session("projname").split("|")
        pdate1 = Session("month") & "/1/" & Session("year")
        pdate2 = Session("month") & "/" & Date.DaysInMonth(Session("year"), Session("month")).ToString & "/" & Session("year")
        ' projid = Session("projname").split("|")
        idsin = fm.getprojemp(projid(1).ToString, pdate2, Session("con"))
    End If
   
    If Request.QueryString("dox") = "edit" Then
        keyp = "update"
    ElseIf Request.QueryString("dox") = "delete" Then
        keyp = "delete"
    ElseIf Request.QueryString("dox") = "clear" Then
        keyp = "Clear"
        
    Else
        keyp = "save"
    End If
    Dim idx As String = ""
    idx = Request.QueryString("id")
    Dim msg As String = ""
    Dim dbx As New dbclass
      Dim sql As String = ""
      Dim flg As Integer = 0
      Dim flg2 As Integer = 0
      ' Response.Write(sc.d_encryption("zewde@123"))
      Dim rd As String = ""
   
      Dim tbl As String = ""
      Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    flg = dbx.save(sql, session("con"),session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, session("con"),session("path"))
                   
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data deleted"
                End If
                ElseIf Request.QueryString("task") = "save" Then
                Dim arrname() As String
                sql = ""
                If Request.QueryString.HasKeys = True Then
                    arrname = Request.QueryString("vname").Split(" ")
                    'Response.Write(arrname.Length.ToString)
                    If arrname.Length >= 3 Then
                        sql = "Select * from emp_static_info where first_name='" & arrname(0) & "' and middle_name='" & arrname(1) & "' and last_name='" & arrname(2) & "'"
               
                    End If
                End If
                Dim dtt As DataTableReader
                emptid = ""
                emp_id = ""
                If sql <> "" Then
                    dtt = dbx.dtmake("tblstatic", sql, session("con"))
               
                    If dtt.HasRows Then
                        dtt.Read()
                        emp_id = dtt.Item("emp_id")
                        '  Response.Write("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and end_date is null order by id desc")
                        emptid = fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and end_date is null order by id desc", Session("con")).ToString
                      
                        emptid = fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and '" & Request.QueryString("loan_date") & "' between hire_date and isnull(end_date,'" & pdate2 & "') order by id desc", Session("con")).ToString
                        If emptid = "None" Then
                            msg = "This Employee is not an Employee to this company"
                            emptid = ""
                        Else
                            '  emptid = fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and end_date is null order by id desc", Session("con")).ToString
                    
                        End If
                        Dim dblrec As String = fm.getinfo2("select id from emp_loan_req where loan_date='" & Request.QueryString("loan_date") & "' and amt='" & Request.QueryString("amt") & "' and deduction_starts='" & Request.QueryString("deduction_starts") & "' and no_month_settle=" & Request.QueryString("no_month_settle") & " and reason='" & Request.QueryString("reason") & "'", Session("con"))
                        Response.Write(dblrec)
                        If dblrec <> "None" Then
                            msg = "Data is duplucated!!!!!!!!!"
                            emptid = ""
                        End If
                    End If
                End If
                If emp_id <> "" And emptid <> "" Then
                    sql = "insert into emp_loan_req(emptid,emp_id,loan_date,reason,amt,deduction_starts,no_month_settle,who_reg,date_reg) Values('" & emptid & "','" & emp_id & "','" & Request.QueryString("loan_date") & _
                    "','" & Request.QueryString("reason") & "','" & Request.QueryString("amt") & "','" & _
                    Request.QueryString("deduction_starts") & "','" & Request.QueryString("no_month_settle") & _
                    "','" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')"
                    Response.Write(sql)
                    Try
                        flg = dbx.excutes(sql, Session("con"), Session("path"))
                    Catch ex As Exception
                          Response.Write(ex.ToString)
                    End Try
                   
                Else
                    msg = "Sorry! data is not saved! check the name." & " or " & msg
                End If
                ' sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
               
                '
                If flg = 1 Then
                    msg = "Data Saved"
                End If
                End If
            
                'MsgBox(rd)
         
                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
             
          
   
   End If
End If
   
   %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>




<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">

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
		<script type="text/javascript" src="scripts/form.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
   <script src="jqq/demo.js"></script>
<script>    
$(function () {
        $("#tabs").tabs();
    });
     <% Dim sec As New k_security
      Dim namelist As String
     namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
   %>
    var nameproj=[<% response.write(namelist) %>]; 
   <%
  if Request.QueryString("month")<>"" then
response.write("var month=" & chr(34) & request.querystring("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.querystring("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.querystring("projname")) & chr(34) & ";")

else
response.write("var month=" & chr(34) & request.form("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.form("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.form("projname")) & chr(34) & ";")

end if %>
   
    </script>

    <%  'Response.Write(Session("projname"))
        Dim rson As String
        rson = fm.getjavalist2("emp_loan_req", "reason", Session("con"), "")
        Dim listinproj As String
        If Session("projname") <> "" Then
            listinproj = fm.getprojemp(projid(1), pdate2, Session("con"))
            'Dim sql As String
            ' response.write("//" & session("pprroojj"))
            sql = " emp_static_info as esi inner join emprec on esi.emp_id=emprec.emp_id " & _
              "where emprec.id " & _
              "in" & _
              "(" & listinproj & ")"
            namelist = fm.getjavalist2(sql, " esi.first_name,esi.middle_name,esi.last_name", Session("con"), " ")
           ' Response.Write(sql)
        Else
            
            namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
        End If%>
     <script type="text/javascript">
var prv;
  prv="";
var id;
var focused="";
var requf=["loan_date","reason","amt","deduction_starts","no_month_settle","x"];
var fieldlist=["emptid","emp_id","loan_date","reason","amt","deduction_starts","no_month_settle","apporved_by","approved_date","who_reg","date_reg","active","x"];
function validation1(){
if ($('#loan_date').val() == '') {showMessage('loan_date cannot be empty','loan_date');$('#loan_date').focus();return false;}
if ($('#reason').val() == '') {showMessage('reason cannot be empty','reason');$('#reason').focus();return false;}
if ($('#amt').val() == '') {showMessage('Amount cannot be empty','amt');$('#amt').focus();return false;}
if ($('#deduction_starts').val() == '') {showMessage('deduction_starts cannot be empty','deduction_starts');$('#deduction_starts').focus();return false;}
if ($('#no_month_settle').val() == '') {showMessage('no_month_settle cannot be empty','no_month_settle');$('#no_month_settle').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_loan_req").formSerialize();
   $("#frmemp_loan_req").attr("action","?tbl=emp_loan_req&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_loan_req").submit();
  return true;}
  }
} 
    var namelist=[<% response.write(namelist) %>];
    $(document).ready(function() {
     $( "#vname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  namelist, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		$( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  nameproj, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		 var rson=[<% response.write(rson) %>];
   
     $( "#reason" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  rson, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});
		
		function hrch(key)
		{ //alert(key);
		var h1,m1;
		var stdate; 
		var spt;
		stdate=$("#ot_time").val();
		var stdate1;
		stdate1=$("#ot_end").val();
		    switch(key)
		    {
		        case "h1":
		        
		            if(stdate!="")
		            {
		                spt=stdate.split(":"); 
		                h1=spt[0];
		                m1=spt[1]; 
		                 $("#ot_time").val($("#h1").val() + ":" + m1);
		            }
		            else
		            {
		                 $("#ot_time").val($("#h1").val() + ":00");
		             }   
		             getshorttime( $("#ot_time").val(),'txtot_time');
		        break;
		        case "m1":
		         if(stdate!="")
		            {
		                spt=stdate.split(":"); 
		                h1=spt[0];
		                m1=spt[1]; 
		                 $("#ot_time").val(h1 + ":" + $("#m1").val());
		            }
		            else
		            {
		                 
		                 $("#ot_time").val("00" + ":" + $("#m1").val());
		            
		            }
		            getshorttime( $("#ot_time").val(),'txtot_time');
		        break;
		        case "h2":
		        
		            if(stdate1!="")
		            {
		                spt=stdate1.split(":"); 
		                h2=spt[0];
		                m2=spt[1]; 
		                 $("#ot_end").val($("#h2").val() + ":" + m2);
		            }
		            else
		            {
		                 $("#ot_end").val($("#h2").val() + ":00");
		             }   
		             getshorttime( $("#ot_end").val(),'txtot_end');
		        break;
		        case "m2":
		         if(stdate1!="")
		            {
		                spt=stdate1.split(":"); 
		                h2=spt[0];
		                m2=spt[1]; 
		                 $("#ot_end").val(h2 + ":" + $("#m2").val());
		            }
		            else
		            {		                 
		                 $("#ot_end").val("00" + ":" + $("#m2").val());
		           
		            }
		            getshorttime( $("#ot_end").val(),'txtot_end');
		        break;
		       
		    }
		}
		function getshorttime(val,id)
		{
		
		    var spl;
		    spl=val.split(":");
		    var intval;
		    var hh="";
		    var mm="";
		    var ap="";
		    //intval=spl[0].parsInt;
		   
		    if(spl[0]>=12)
		    { 
		   
		        if(spl[0]==12)
		        {
		        hh=spl[0];
		        mm=spl[1];
		        ap="PM";
		           //$("#" + id).text( + ":" +  + );
		          
		        }
		        else
		        {
		        intval=spl[0]-12;
		        hh=intval;
		        mm=spl[1];
		        ap="PM";
		        // $("#" + id).text(intval + ":" + spl[1] + "PM");
		        }
		    }
		    else
		    {
		    
		         if(spl[0]==0)
		        {
		        hh=12;
		        mm=spl[1];
		        ap="AM";
		              // alert(spl[0] + ":" + spl[1] + "AMy");
		          // $("#" + id).text("12:" + spl[1] + "AM");
		        }
		        else
		        {
		        hh=spl[0];
		        mm=spl[1];
		        ap="AM";
		          //$("#" + id).text("");
		           // $("#" + id).text( + ":" + spl[1] + "AM");
		            //alert(id+"===>" + spl[0] + ":" + spl[1] + "4Mx");
		        }
		        
		    }
		   // alert(hh.toString() + ":" + mm.toString() + ap);
		   $("#" + id).text( hh.toString() + ":" + mm.toString() + ap);
		}
     function submitfrm(val)
     {
        if (val=="simple")
        $('#frmsub').submit();
        else if(val=="clear")
        {
            $("#frmsub").attr("action","?dox=clear");
 $('#frmsub').submit();
        }
     } 
 </script>
 
</head>

<body style="height:auto; font-size:10px;">

 <div id="tabs">
  <ul>
    <li><a href="#tabs-1">Add Data and Edit</a></li>
    <li><a href="#tabs-2">Payment History</a></li>
    <li style=" padding-left:25px;"><form id="frmsub">
    Month <select id="month" name="month">
            <%
                For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
        <script type="text/javascript">
            $("#month").val("<% response.write(today.month) %>");
        </script>
        Year <select id="year" name="year">
            <%  For i As Integer = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
         Enter Project: <input type="text" name="projname" id="projname"  />
       
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submitfrm('simple');" />
          <input type="button" value="Clear" id="Button1" name="firstform" onclick="javascript:submitfrm('clear');" />
         </form></li>
    
  </ul>
  <div id="tabs-1"> 
  <%''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' %>
  

 <div id="formouterbox_small" style="width:650px; float:left;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messageboxx"></span>
    <% Response.Write(Request.QueryString("month"))
    Response.Write(Request.QueryString("year"))
 %> 
<form method='post' id='frmemp_loan_req' name='frmemp_loan_req' action=''> 
<table id='tb1'><tr><td>Name</td><td>:</td><td>
<input type='text' name='vname' id='vname' />

<input type='hidden' id='emptid' name='emptid' value='' />
<input type='hidden' id='emp_id' name='emp_id' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Loan Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='loan_date' name='loan_date' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>    $(function () { $("#loan_date").datepicker({ changeMonth: true, changeYear: true }); $("#loan_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
<td>Reason<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='reason' name='reason' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Amount<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='amt' name='amt' value='' style="text-align:right;" /><br /><label class='lblsmall'></label></td>
<td>Deduction Starts<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='deduction_starts' name='deduction_starts' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>    $(function () { $("#deduction_starts").datepicker({ changeMonth: true, changeYear: true }); $("#deduction_starts").datepicker("option", "dateFormat", "mm/dd/yy"); });</script></tr><tr>
<td>Settlement divides into<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='no_month_settle' name='no_month_settle' value='' size="3" /><br /><label class='lblsmall'></label>
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/></td>
</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
</div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
      <% Response.Write(Request.QueryString("month"))
    Response.Write(Request.QueryString("year"))
 %> 
     
 <% Dim loc As String
     Dim mk As New formMaker
     Dim row As String
     loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
     'Response.Write(Request.Form("projname"))
     If keyp = "Clear" Then
         Session("projname") = ""
         Session("month") = ""
         Session("year") = ""
         Dim sqlx As String = "select id,loan_date,reason,amt,deduction_starts,no_month_settle,emptid from emp_loan_req order by id desc"
        
         row = mk.edit_del_list_wname("emp_loan_req", sqlx, "Employee Name,Loan Date,Reason,Loan Amount,Action Start,In Months Settled", Session("con"), loc)
         ' Response.Write(row)
     ElseIf Session("projname") <> "" Then
         Dim sqlx As String = "select id,loan_date,reason,amt,deduction_starts,no_month_settle,emptid from emp_loan_req where emptid in (" & idsin & _
             ") and loan_date between '" & pdate1 & "' and '" & pdate2 & "'  order by id desc"
         'Response.Write("<b>" & projid(0) & "   " & MonthName(Session("month")) & " " & Session("year") & "</b>")
         row = mk.edit_del_list_wname("emp_loan_req", sqlx, "Employee Name,Loan Date,Reason,Loan Amount,Action Start,In Months Settled", Session("con"), loc)
         Response.Write(row)
     End If
     
     %>   
 <div id="listx" style=" width:400px; padding:0px 7px 0px 0px; float:left; font-size:12px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_loan_req where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
                For k As Integer = 0 To dt.FieldCount - 3
                    'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                    If LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                        %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k).trim & "');")%>
                    <% 
                    ElseIf LCase(dt.GetDataTypeName(k)) = "datetime" And dt.IsDBNull(k) = False Then
                        Dim sdatex As Date = dt.Item(k)
                        Dim d As String = sdatex.ToShortDateString
                        Dim da As String = sdatex.Day
                        Dim mm As String = sdatex.Month
                        Dim yy As String = sdatex.Year
                        d = mm & "/" & da & "/" & yy
                        Response.Write("$('#" & dt.GetName(k) & "').val('" & d & "');")
                    Else
                         %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k) & "');")%>
                    <%
                    End If
                   
                Next
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
                End If
                dt.Close()
            End If
            db = Nothing
            dt = Nothing
       
        
    %>
 </div>
 <%''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' %>
 </div>
 <div id="tabs-2">
        <div id="divloan" style="">
                         <%  Dim dbs As New dbclass
                             Dim rs As DataTableReader
                             Dim fullname As String
                             If Session("projname") <> "" Then
                                 rs = dbs.dtmake("dbsloan", "select distinct emptid from vw_loan where emptid in (" & idsin & ")", Session("con"))
                                 If rs.HasRows Then
                                     While rs.Read
                                         emptid = rs.Item("emptid")
                                         fullname = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                                         Response.Write("<b>" & fullname & "</b><br>")
                                         Response.Write(fm.loan(emptid, Session("con")))
                                     End While
                                 End If
                             Else
                                 Response.Write("No Loan from this project")
                             End If
                            
                           
                             
        %>
                        </div>
 
 </div>
 </div>

 
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Loan Request!");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_loan_req");
        $('#frmx').submit();
            }
            else
            {
                ha(hd);
            }
       }
   </script>
    <form id="frmx" action="" method="post">
    </form>
   
   
   <%  ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','yes','del123');" & Chr(34) & "></span>"
           fs.msgboxt("del123", "Caution! Deleting", con)
           str = "<div id='del123' style=" & Chr(34) & "opacity:0.9;filter:alpha(opacity=90); background:#9fdfaf; left:400px; top:200px; width:600px; height:400px; text-align:center; vertical-align:middle; position:absolute; content:open-quote;" & Chr(34) & _
            "><div style=" & Chr(34) & "height:30px; background:url(images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            "><div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:120px; position:absolute; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>Warrening</b></div><div style=" & Chr(34) & "cursor:pointer; text-align:right; height:30px; width:22px; color:#CC0000; background:url(../images/xp.gif); background-repeat:no-repeat; right:0px; position:absolute" & Chr(34) & " dir=" & Chr(34) & "rtl" & Chr(34) & " onClick=" & Chr(34) & "javascript: document.getElementById('" & CStr(ID) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp; </div></div><br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(con) & "</div></div>"
           ' Response.Write(str)
           %> 
           <div id="dialog-modal" title="Caution"><% response.write(con) %></div>
           <script type="text/javascript">
          
           //$( "#dialog:ui-dialog" ).dialog( "destroy" );
	
		$( "#dialog-modal" ).dialog({
		resizable: true,
			modal: true
		});
           </script>
           <%
           
       End If
           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","_parent");
       
        //$('#frmx').attr("action","<% response.write(rd) %>");
       // $('#frmx').submit();
    </script>
   <%  End If
      %>
   
   
</body>
</html>
<script>
   $(document).ready(function() {
   $("#tb1").css({'font-size':'8px'});
   $('#basic_example_2').timepicker();
   $("#month").val('<%=session("month") %>');
   $("#year").val('<%=session("year") %>');
   $("#projname").val('<%=session("projname") %>');
});
<% If msg <> "" Then
           %>
           $("#messageboxx").text("<%response.write(msg) %>");
           
           <%
       End If
       
       %>
</script>
