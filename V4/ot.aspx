<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ot.aspx.vb" Inherits="ot" %>


<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  Dim dtime As New datetimecal
    Dim fact(2) As String
    Dim rate(4) As Double
    Dim salary() As String
    Dim hr As Double
    Dim hr1(4) As Date
    Dim date1 As Date
    Dim date2 As Date
    Dim hr_st(2) As String
    Dim hr_end(2) As String
    Dim timedif(2) As Double
    Dim rm As Integer
    Dim t As Integer
    Dim keyp As String = ""
    Dim fm As New formMaker
    Dim emp_id, emptid As String
    If Request.QueryString("dox") = "edit" Then
        keyp = "update"
    ElseIf Request.QueryString("dox") = "delete" Then
        keyp = "delete"
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
                ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")
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
                If sql <> "" Then
                    dtt = dbx.dtmake("tblstatic", sql, session("con"))
               
                    If dtt.HasRows Then
                        dtt.Read()
                        emp_id = dtt.Item("emp_id")
                        emptid = CInt(fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and end_date is null order by id desc", session("con")))
                        salary = dbx.getsal(emptid, session("con"))
                        If salary(0) = "Sorry This employee salary info is not setted!" Then
                            Response.Write(salary(1))
                        Else
                            hr = CDbl(salary(0)) / 200.67
                        End If
                       
                        
                       
                    End If
                    date1 = Request.QueryString("ot_date")
                    Dim obj As Object
                    obj = dtime.isPublic(date1, session("con"))
                    hr1(0) = "#" & Request.QueryString("ot_date") & " " & Request.QueryString("ot_time") & "#"
                    hr1(1) = "#" & Request.QueryString("ot_date") & " " & Request.QueryString("ot_end") & "#"
                    'Response.Write(hr1(0).Hour & ":" & hr1(0).Minute & "<br>")
                    'Response.Write(hr1(1).Hour & ":" & hr1(1).Minute & "<br>")
                    'Response.Write("time differ:" & (hr1(1).Subtract(hr1(0)).TotalMinutes).ToString)
                    Dim endtime As Double = (hr1(1).Hour) * 60 + hr1(1).Minute
                    Dim sttime As Double = (hr1(0).Hour) * 60 + hr1(0).Minute
                    If obj.ToString = "True" Then
                        fact(0) = "HD"
                        rate(0) = 2.5
                        timedif(0) = endtime - sttime
                        hr_st(0) = (CInt(sttime / 60)).ToString & ":" & (sttime Mod 60).ToString
                        hr_end(0) = (CInt(endtime / 60)).ToString & ":" & (endtime Mod 60).ToString
                        t = 1
                    ElseIf dtime.isWeekEnd(date1, emp_id, session("con")).ToString = "True" Then
                        fact(0) = "WE"
                        rate(0) = 2
                        timedif(0) = endtime - sttime
                        hr_st(0) = (CInt(sttime / 60)).ToString & ":" & (sttime Mod 60).ToString
                        hr_end(0) = (CInt(endtime / 60)).ToString & ":" & (endtime Mod 60).ToString
                        t = 1
                    Else
                       
                       
                       
                        If endtime = 0 Then
                            endtime = (24 * 60)
                        End If
                        If endtime > (22 * 60) And endtime <= (24 * 60) Then
                            If sttime > (22 * 60) Then
                                rate(0) = 1.5
                                fact(0) = "Nig"
                                timedif(0) = CDbl(endtime - sttime)
                                Response.Write(timedif(0).ToString & "<br>")
                                t = 1
                                hr_st(0) = (CInt(sttime / 60)).ToString & ":" & (sttime Mod 60).ToString
                                hr_end(0) = (CInt(endtime / 60)).ToString & ":" & (endtime Mod 60).ToString
                            Else
                                rate(0) = 1.25
                                timedif(0) = CDbl((22 * 60) - sttime)
                                hr_st(0) = (CInt(sttime / 60)).ToString & ":" & (sttime Mod 60).ToString
                                hr_end(0) = "22:00"
                                fact(0) = "Rg"
                                rate(1) = 1.5
                                timedif(1) = CDbl(endtime - (22 * 60))
                                fact(1) = "Nig"
                                hr_st(1) = ("22:00")
                                hr_end(1) = (CInt(endtime / 60)).ToString & ":" & (endtime Mod 60).ToString
                                t = 2
                            End If
                    
                        ElseIf endtime > (6 * 60) Then
                            If sttime < (6 * 60) Then
                                fact(0) = "Nig"
                                rate(0) = 1.5
                                timedif(0) = CDbl((6 * 60) - sttime)
                                hr_st(0) = (CInt(sttime / 60)).ToString & ":" & (sttime Mod 60).ToString
                                hr_end(0) = "6:00"
                                fact(1) = "Rg"
                                rate(1) = 1.25
                                timedif(1) = CDbl(endtime - (6 * 60))
                                hr_st(1) = "6:00"
                                hr_end(1) = (CInt(endtime / 60)).ToString & ":" & (endtime Mod 60).ToString
                                t = 2
                            Else
                                fact(0) = "Rg"
                                rate(0) = 1.25
                                timedif(0) = CDbl(endtime - sttime)
                                hr_st(0) = (CInt(sttime / 60)).ToString & ":" & (sttime Mod 60).ToString
                                hr_end(0) = (CInt(endtime / 60)).ToString & ":" & (endtime Mod 60).ToString
                                t = 1
                            End If
                       
                        ElseIf endtime < 22 * 60 Then
                            If endtime - sttime > 0 Then
                                fact(0) = "Rg"
                                rate(0) = 1.25
                                timedif(0) = endtime - sttime
                                hr_st(0) = (CInt(sttime / 60)).ToString & ":" & (sttime Mod 60).ToString
                                hr_end(0) = (CInt(endtime / 60)).ToString & ":" & (endtime Mod 60).ToString
                                t = 1
                            End If
                        
                        End If
                        
                    End If
                    dtt.Close()
                    ' Response.Write("<br>factor:" & fact & "<br>rate:" & rate(0).ToString)
                End If
                    
                For k As Integer = 0 To t - 1
                    Response.Write("fact:" & fact(k) & "<br>")
                    Response.Write("rate:" & rate(k) & "<br>")
                    Response.Write("timedif:" & timedif(k) & "<br>")
                    Response.Write("hour rate: " & hr & "<br>")
                    Response.Write("amout: " & (hr * (timedif(k) / 60) * rate(k)) & "<br>")
                    Response.Write("time 1: " & hr_st(k) & " time 2: " & hr_end(k) & "<br>")
                    sql = "insert into emp_ot(emp_id,emptid,ot_date,ot_time,ot_end,time_diff,rate,factored,amt) " & _
                    "values('" & emp_id & "','" & emptid & "','" & date1 & "','" & hr_st(k) & "','" & _
                    hr_end(k) & "','" & timedif(k) & "','" & rate(k) & "','" & fact(k) & "','" & (hr * (timedif(k) / 60) * rate(k)) & "')"
                    Response.Write("<br>" & sql)
                    flg = dbx.save(sql, session("con"),session("path"))
                Next
                '   Response.Write(sql)
                Response.Write(flg)
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

<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
</script><script type="text/javascript" src="jq/ui/jquery.ui.autocomplete.js"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
</head>

<body style="height:auto;">
<div>
 </div>
  <%  
      Dim namelist As String = ""
      namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", session("con"), " ")

'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))
If Request.Form.HasKeys = True Then
    'Dim db As New dbclass
    ' Dim sql As String
    ' sql = db.makest("tblworkexp", Request.Form, session("con"), "")
    'Response.Write(sql)
    'db.save(sql, session("con"),session("path"))
End If
For Each p As String In Request.Form
    'Response.Write(" <br />" & p & "=>" & Request.Form(p))
 
Next
For Each k As String In Request.ServerVariables
    ' Response.Write("<br />" & k & "=>" & Request.ServerVariables(k))
Next
'Response.Write("<br />" & Request.Form("do"))

 %>

 <div id="formouterbox_small">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messageboxx"></span>
 <script type="text/javascript">
var prv;
  prv="";
var id;
var focused="";
var requf=["ot_date","ot_time","ot_end","x"];
var fieldlist=["emp_id","emptid","ot_date","ot_time","ot_end","description","who_reg","date_reg","x"];
function validation1(){
if ($('#vname').val() == '') {showMessage('vname cannot be empty','vname');$('#vname').focus();return false;}
if ($('#ot_date').val() == '') {showMessage('ot_date cannot be empty','ot_date');$('#ot_date').focus();return false;}
if ($('#ot_time').val() == '') {showMessage('ot_time cannot be empty','ot_time');$('#ot_time').focus();return false;}
if ($('#ot_end').val() == '') {showMessage('ot_End cannot be empty','ot_end');$('#ot_end').focus();return false;}
if ($('#description').val() == '') {showMessage('description cannot be empty','description');$('#description').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_ot").formSerialize();
   $("#frmemp_ot").attr("action","?tbl=emp_ot&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_ot").submit();
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
 </script>
<form method='post' id='frmemp_ot' name='frmemp_ot' action=""> 
<table><tr>
<td>Search by Name</td><td>:</td><td><input type='text' name='vname' id='vname' style='font-size:9pt;' ></td></tr><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<%response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='ot_date' name='ot_date' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'> $(function() {$( "#ot_date").datepicker({changeMonth: true,changeYear: true	}); $( "#ot_date" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
</tr><tr><td>Start Time<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id="h1" name="h1" onchange="javascript:hrch('h1');">
    <%
        Dim kout As String
        For i As Integer = 0 To 23
            If i.ToString.Length = 1 Then
                kout = "0" & i.ToString
            Else
                kout = i.ToString
            End If
            %>
            <option value="<%response.write(kout) %>"><% Response.Write(kout)%></option>
            <%
        Next
     %>
     
</select>:
<select id="m1" name="m1" onchange="javascript:hrch('m1');">
    <%
        For i As Integer = 0 To 55 Step 5
            If i.ToString.Length = 1 Then
                kout = "0" & i.ToString
            Else
                kout = i.ToString
            End If
            %>
            <option value="<%response.write(kout) %>"><% Response.Write(kout)%></option>
            <%
        Next
     %>
     
</select>
<br /><label class='lblsmall' id="txtot_time">HH:mm</label>
<input type='hidden' id='ot_time' name='ot_time' value='' /></td>
<td>End Time<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id="h2" name="h2" onchange="javascript:hrch('h2');">
    <%
        
        For i As Integer = 0 To 23
            If i.ToString.Length = 1 Then
                kout = "0" & i.ToString
            Else
                kout = i.ToString
            End If
            %>
            <option value="<%response.write(kout) %>"><% Response.Write(kout)%></option>
            <%
        Next
     %>
     
</select>:
<select id="m2" name="m2" onchange="javascript:hrch('m2');">
    <%
        For i As Integer = 0 To 55 Step 5
            If i.ToString.Length = 1 Then
                kout = "0" & i.ToString
            Else
                kout = i.ToString
            End If
            %>
            <option value="<%response.write(kout) %>"><% Response.Write(kout)%></option>
            <%
        Next
     %>
     
</select>
<input type='hidden' id='ot_end' name='ot_end' value='' /><br /><label class='lblsmall' id="txtot_end">HH:hh</label></td>
</tr><tr><td>Description<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='description' name='description' value='-' /><br /><label class='lblsmall'></label>
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
 
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_ot where id=" & Request.QueryString("id"), session("con"))
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
        Dim mk As New formMaker
            Dim row As String
            Dim loc As String
            loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,ot_date,ot_time,ot_end,description from emp_ot where emptid='" & emptid & "' order by id desc"
        row = mk.edit_del_list("emp_ot", sqlx, "Overtime date,Start time,End time, Description", Session("con"), loc)
            Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Overtime Reg.");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_ot");
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
   $('#basic_example_2').timepicker();
});
</script>
