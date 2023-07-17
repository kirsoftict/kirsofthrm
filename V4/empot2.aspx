<%@ Page Language="VB" AutoEventWireup="false" CodeFile="~/empot2.aspx.vb" Inherits="empot2" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
   
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
                Dim salary() As String
                Dim hr As Double
                
                Dim dtt As DataTableReader
                If sql <> "" Then
                    dtt = dbx.dtmake("tblstatic", sql, session("con"))
               
                    If dtt.HasRows Then
                        dtt.Read()
                        emp_id = dtt.Item("emp_id")
                        emptid = CInt(fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and end_date is null order by id desc", session("con")))
                        salary = dbx.getsal(emptid, session("con"))
                        hr = CDbl(salary(0)) / 200.67
                        
                       
                    End If
                    Dim amt, rate As Double
                    Dim date1, date2 As Date
                    Dim factored As String
                    date1 = Request.QueryString("ot_date")
                    date2 = Request.QueryString("datework")
                    rate = CDbl(Request.QueryString("rate"))
                    Dim timedif As Double
                    timedif = Request.QueryString("time_diff")
                   
                    amt = hr * rate * timedif
                    
                    dtt.Close()
                    Select Case rate
                        Case 1.25
                            factored = "Reg"
                        Case 1.5
                            factored = "Nig"
                        Case 2
                            factored = "WE"
                        Case 2.5
                            factored = "HD"
                            
                    End Select
                        
                    sql = "insert into emp_ot(emp_id,emptid,ot_date,time_diff,rate,factored,amt,datework,who_reg,date_reg) " & _
                    "values('" & emp_id & "','" & emptid & "','" & date1.ToShortDateString & "','" & timedif & "','" & rate & _
                    "','" & factored & "','" & Math.Round(amt, 2).ToString & "','" & date2 & "','" & Request.QueryString("who_reg") & _
                    "','" & Request.QueryString("date_reg") & "')"
                    ' Response.Write(sql)
                    
                    flg = dbx.save(sql, session("con"),session("path"))
                    ' Response.Write(flg)
                    If flg = 1 Then
                        msg = "Data Saved"
                    Else
                        msg = "Data is not saved, try again. If this continue for long please contact your IT officer"
                    End If
                End If
            
                'MsgBox(rd)
         
                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            End If
            
          
   
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
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
<script src="jqq/ui/jquery.ui.button.js"></script>
    <script src="jqq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>	

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
 <script type="text/javascript">
 
var prv;
  prv="";
var id;
var focused="";
var requf=["ot_date","time_diff","factored","datework","x"];
var fieldlist=["emp_id","emptid","ot_date","ot_time","ot_end","description","datework","who_reg","date_reg","x"];
function validation1(){
if ($('#vname').val() == '') {showMessage('vname cannot be empty','vname');$('#vname').focus();return false;}
if ($('#time_diff').val() == '') {showMessage('Houre cannot be empty','time_diff');$('#time_diff').focus();return false;}
if ($('#ot_date').val() == '') {showMessage('OT_date cannot be empty','ot_date');$('#ot_date').focus();return false;}
if ($('#factored').val() == '') {showMessage('factored cannot be empty','factored');$('#factored').focus();return false;}
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
		
		
 </script>
</head>

<body style="height:auto;">
<div></div>


 <div id="formouterbox_small" style="width:600px;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messageboxx"></span>

<form method='post' id='frmemp_ot' name='frmemp_ot' action=""> 
<table width="600px"><tr>
<td>Search by Name</td><td>:</td><td><input type='text' name='vname' id='vname' style='font-size:9pt;' ></td>
</tr><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<%response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
Date Paid<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='ot_date' name='ot_date' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'> $(function() {$( "#ot_date").datepicker({changeMonth: true,changeYear: true	}); $( "#ot_date" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
<td>Date OT<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='datework' name='datework' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'> $(function() {$( "#datework").datepicker({changeMonth: true,changeYear: true	}); $( "#datework" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
</tr><tr><td>Factored Hours</td><td>:</td><td>
<select id="rate" name="rate">
    <option value="1.25">Regular Hrs.</option>
     <option value="1.5">Night Hrs.</option>
    <option value="2">Sunday(Weekends) Hrs.</option>
    <option value="2.5">Public Holiday Hrs.</option>
 </select>
</td><td>Hours:</td><td>:</td><td>
<input type='text' id='time_diff' name='time_diff' value='' size="2" /><br />
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
        Dim sqlx As String = "select id,ot_date,datework,factored,time_diff,rate,amt from emp_ot order by id desc"
        row = mk.edit_del_list2("emp_ot", sqlx, "Overtime Paid date,Overtime Date,Factored-hour,HRs,Rate,Total Amount", Session("con"), loc, "", False, True, False, False)
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
