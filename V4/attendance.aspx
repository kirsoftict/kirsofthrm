<%@ Page Language="VB" AutoEventWireup="false" CodeFile="attendance.aspx.vb" Inherits="attendance" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
    
    ' For Each t As String In Request.ServerVariables
    ' Response.Write(t & "===>" & Request.ServerVariables(t) & "<br>")
    ' Next
    If Session("emp_iid") = "" Then
        Response.Write(" <script>window.location='logout.aspx?msg=session timeout';</script>")
    End If
    Dim keyp As String = ""
    Dim fm As New formMaker
    Dim emp_id, emptid As String
    emptid = ""
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
   ' Response.Write(Request.QueryString("dox"))
    
    If Request.QueryString.HasKeys = True Then
        If Request.QueryString("dox") = "" Then
           ' Response.Write("dox===""" &  Request.QueryString("task"))
            keyval = Request.QueryString("keyval")
            If Request.QueryString("task") = "update" Then
                ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                flg = dbx.save(sql, Session("con"), Session("path"))
                ' Response.Write(sql)
                If flg = 1 Then
                    msg = "Data Updated"
                End If
            ElseIf Request.QueryString("dox") = "delete" Then
                'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                flg = dbx.save(sql, Session("con"), Session("path"))
                   
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
                If Request.QueryString("to_date") <> "" Then
                    
                    If sql <> "" Then
                        
                        dtt = dbx.dtmake("tblstatic", sql, Session("con"))
                        Dim d1, d2 As Date
                        d1 = Request.QueryString("att_date")
                        d2 = Request.QueryString("to_date")
                        If dtt.HasRows Then
                            dtt.Read()
                            '   Response.Write(sql)
                            emp_id = dtt.Item("emp_id")
                            sql = "select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and '" & d1.ToShortDateString & "' between hire_date and isnull(end_date,'" & Now.ToShortDateString & "') order by id desc"
                            emptid = (fm.getinfo2(sql, Session("con")))
                            '   Response.Write("emptid:==><br>" & sql & "<br>+=========" & emp_id)
                        End If
                        
                        Dim sysd As New datetimecal
                       
                        If d2.Subtract(d1).Days >= 0 And emptid <> "" Then
                            
                            '  Response.Write(emptid)
                            sql = ""
                            While d1 <= d2
                                ' Response.Write(emptid)
                                ' Response.Write(sysd.isWeekEnd(d1, emp_id, Session("con")).ToString & "<br>")
                                ' If (sysd.isWeekEnd(d1, emp_id, Session("con"))).ToString = "False" Then
                                If sysd.isPublic(d1, Session("con")) = False Or sysd.isWeekEnd(d1, emptid, Session("con")).ToString = "False" Then
                                    sql &= "insert into emp_att(emptid,att_date,status,daypartition,who_reg,date_reg) " & _
                               "values('" & emptid & "','" & d1.ToShortDateString & "','" & _
                               Request.QueryString("st") & "','" & _
                               Request.QueryString("daypartition") & "','" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')" & Chr(13)
                                End If
                               
                                '  End If
                                d1 = d1.AddDays(1)
                            End While
                            '  Response.Write(sql)
                            If sql <> "" And Session("emp_iid") <> "" Then
                                sql = "Begin Trans " & Session("emp_iid") & Chr(13) & sql
                                Dim rtv As String
                                rtv = dbx.excutes(sql, Session("con"), Session("path"))
                                If IsNumeric(rtv) = True Then
                                    If CInt(rtv) > 0 Then
                                        dbx.excutes("Commit Trans " & Session("emp_iid"), Session("con"), Session("path"))
                                        msg = "Data Saved"
                                    Else
                                        dbx.excutes("RollBack Trans " & Session("emp_iid"), Session("con"), Session("path"))
                                        msg = "Data is Not Saved"
                                    End If
                                Else
                                    dbx.excutes("RollBack Trans " & Session("emp_iid"), Session("con"), Session("path"))
                                    msg = "Data is Not Saved"
                                End If
                            End If
                        Else
                            msg = "Sorry Date From is greater than to Date to"
                        End If
                        
                        dtt.Close()
                    End If
                Else
                  
               
                
                   
                    If sql <> "" Then
                        dtt = dbx.dtmake("tblstatic", sql, Session("con"))
               
                        If dtt.HasRows Then
                            dtt.Read()
                            emp_id = dtt.Item("emp_id")
                            emptid = CInt(fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and end_date is null order by id desc", Session("con")))
                        
                        
                       
                        End If
                        ' Response.Write(Request.QueryString("st"))
                        sql = "insert into emp_att(emptid,att_date,status,who_reg,date_reg) " & _
                        "values('" & emptid & "','" & Request.QueryString("att_date") & "','" & _
                        Request.QueryString("st") & "','" & _
                                   Request.QueryString("daypartition") & "','" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')"
                        'Response.Write(sql)
                    
                        flg = dbx.save(sql, Session("con"), Session("path"))
                        ' Response.Write(flg)
                        If flg = 1 Then
                            msg = "Data Saved"
                        End If
                    End If
                End If
        
            
                'MsgBox(rd)
         
                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            End If
            
          
   
        End If
    End If
    If Request.QueryString("dox") = "delete" Then
        ' Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
        ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
        sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
        Dim conf As String
        If IsError(Session("con")) Then
            Response.Write("<br>--------xxxxxxxxxxxxxxxxxx------------------<br>Error Coooooonnnneection <br>...................................<br>")
        ElseIf Session("con").State = ConnectionState.Closed Then
            Response.Write("<br>--------xxxxxxxxxxxxxxxxxx------------------<br>Error Coooooonnnneection closed <br>...................................<br>")
    
        End If
        conf = dbx.excutes(sql, Session("con"), Session("path"))
                   
       ' Response.Write(sql)
        If IsNumeric(conf) Then
            If flg = 1 Then
                msg = "Data deleted"
            End If
        Else
            Response.Write(conf.ToString)
        End If
        
    End If
    If Request.QueryString("dox") = "selectDel" Then
        ' Response.Write("sql del")
        ' Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
        ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
        sql = ""
        Try
            
        
        For Each k As String In Request.QueryString
            ' Response.Write(k & "--->" & Request.QueryString(k) & "<br>")
                If k <> "" Then
                    If k.Length >= 3 Then
                        '  Response.Write(k & "--->" & Request.QueryString(k) & "<br>")
                        If k.Substring(0, 3) = "del" Then
                            sql &= "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString(k) & Chr(13)
                        End If
                    End If
                    
                End If
            Next
            'sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
            
        sql = "Begin Trans " & Session("emp_iid") & Chr(13) & sql
        ' Response.Write(sql)
        Dim rtv As String
        rtv = dbx.excutes(sql, Session("con"), Session("path"))
       
        If IsNumeric(rtv) = True Then
            If CInt(rtv) > 0 Then
                '   Response.Write(rtv.ToString & "<<<<<<<<<<<<<<<br>")
                    rtv = (dbx.excutes("Commit Trans " & Session("emp_iid"), Session("con"), Session("path")))
                '    Response.Write(rtv.ToString & "At Commited")
                If IsNumeric(rtv) = False Then
                        dbx.excutes("RollBack Trans " & Session("emp_iid"), Session("con"), Session("path"))
                    msg = "Data is Not deleted"
                Else
                    msg = "Data Deleted"
                End If
                
            Else
                dbx.excutes("RollBack", Session("con"), Session("path"))
                msg = "Data is Not deleted " & rtv
                '   Response.Write("roled back")
            End If
        Else
            dbx.excutes("RollBack", Session("con"), Session("path"))
            ' Response.Write("mmmmmmmmmmm what!!!")
            msg = "Data is Not deleted "
            End If
        Catch exx As Exception
            Dim em As New mail_system
            'Dim dbx As New dbclass
            dbx.writeerro(exx.ToString & "<br>Delete attendance Entry") 
            
            Response.Write(exx.ToString & "<br>")
            For Each k As String In Request.QueryString
                Response.Write(k & "--->" & Request.QueryString(k) & "<br>")
               
            Next
            
        End Try
        '  conf = dbx.excutes(sql, Session("con"), Session("path"))
                   
        '  Response.Write(",,,,,,,,,,,,,,," & sql)
        
    End If

   %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" type="text/css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />
	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
    <script src="jqq/ui/jquery.ui.tabs.js"></script>
<script src="jqq/ui/jquery.ui.button.js"></script>
	
	

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
	
	<script type="text/javascript" src="scripts/form.js"></script>
	
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
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

'Response.Write("<br />" & Request.Form("do"))

 %>
 <script type="text/javascript">
 var likeid;
  function showHideSubMenu(link,id) {

        var uldisplay="";
        var newClass="";
        
        
        if (link.className == 'expanded') {

            // Need to hide
            
            uldisplay = 'none';
            newClass = 'collapsed';

        } else {
            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
        }

       
        $("#"+id).css({'display':  uldisplay});
        link.className = newClass;
    }
 
 
var prv;
  prv="";
var id;
var focused="";
var requf=["vname","att_date","status","x"];
var fieldlist=["emptid","att_date","status","who_reg","date_reg","x"];
function validation1(){
if ($('#vname').val() == '') {showMessage('Employee name cannot be empty','vname');$('#vname').focus();return false;}
if ($('#att_date').val() == '') {showMessage('att_date cannot be empty','att_date');$('#att_date').focus();return false;}
if ($('#status').val() == '') {showMessage('Status cannot be empty','status');$('#status').focus();return false;}
if ($('#to_date').val()==''){$('#to_date').val($('#att_date').val());showMessage('To date cannot be empty','to_date');$('#to_date').focus();return false;}
if($('#att_date').val()>$('#to_date').val()){alert('error! the from date should be smaller than the second');$('#to_date').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_att").formSerialize();
   $("#frmemp_att").attr("action","?tbl=emp_att&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_att").submit();
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
		
		$(function() {
		$( "#tabs" ).tabs();
	});
    function ajaxsumit(wc)
    {
            var str;
            if(wc=="month"){
             str=$("#frmlistout").formSerialize();}
            else
            {
                 str=$("#frmemp_att").formSerialize();
                // alert(str);
            }
    //alert(wc+str);
     $("#listx").load("attendance.aspx?which="+wc+"&"+str);
        
    }
 </script>
 <script type='text/javascript'>

     function ssub() {

         str = $("#frmattendance").formSerialize();
         $("#frmattendance").attr("action", "attendance.aspx?" + str);
         //alert(str);
         $("#frmattendance").submit();
     }
     function goclicked(whr, id, str, loc,tbl) {
         if (whr == "delete") {
             $("#dialog-confirm").css({ "display": "none", "font-size": "12pt" });
             $("#dialog-confirm").dialog({
                 resizable: true,
                 modal: true,
                 height: 140,
                 buttons: {
                     "Delete ": function () {
                         $("#listx").load(loc +'?dox=' + whr + '&tbl=' + tbl + '&id=' + id.toString() + '&' + str);
                         //  $('#frms').attr('action', loc + '?dox=' + whr + '&tbl=' + tbl + '&id=' + id.toString() + '&' + str);
                         
                       //  $('#frms').submit();
                     },
                     Cancel: function () {
                         $(this).dialog("close");
                     }
                 }
             });

         }
         else if (whr == "godelete") {

             $('#listx').load(loc + '?dox=' + whr + '&id=' + id.toString() + '&' + str);
            // $('#frms').submit();
         }
         else if (whr == "edit") {
                 
             $('#frms').attr('action', loc + '?dox=' + whr + '&tbl=' + tbl + '&id=' + id.toString() + '&' + str);
             $('#frms').submit();
         }
         else if (whr == "selectDel") {
         
             $("#dialog-confirm").css({ "display": "none", "font-size": "12pt" });
             $("#dialog-confirm").dialog({
                 modal: true,
                 height: 240,
                 buttons: {
                     "Delete ": function () {
                         //alert(loc);

                         str = $("#frms").formSerialize();
                         //alert(str);
                         str += "&" + $("#frmemp_att").formSerialize() + "&" + $("#frmlistout").formSerialize();
                        // alert(loc + '?dox=' + whr + '&tbl=' + tbl + '&id=' + id.toString() + '&' + str);
                         $("#frms").attr("action", loc + '?dox=' + whr + '&tbl=' + tbl + '&id=' + id.toString() + '&' + str);
                         $("#frms").submit();
                         // $('#listx').load(loc + '?which=<%=request.querystring("which") %>&dox=' + whr + '&tbl=' + tbl + '&id=' + id.toString() + '&' + str);
                         // $('#frms').submit();
                         $(this).dialog("close");
                        // $('#listx').text() += '<%=msg %>'

                         //  ajaxsumit('<%=request.querystring("which") %>');
                         // $("#listx").text($("#listx").text() + ajaxsumit('<%=request.querystring("which") %>'));

                     },
                     Cancel: function () {
                         $(this).dialog("close");
                     }
                 }
             });
         }
             }</script>
 <style>#tabs{font-size:10px;}</style>
</head>

<body style="height:auto;">
<%  ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))
    If Session("mmm") <> "" Then
        If Request.QueryString("monthx") <> "" Then
            Session("mmm") = Request.QueryString("monthx")
            Session("yyr") = Request.QueryString("yearx")
        End If
    ElseIf Request.QueryString("monthx") <> "" Then
        Session("mmm") = Request.QueryString("monthx")
        Session("yyr") = Request.QueryString("yearx")
    End If
    Response.Write(msg)
    ' Response.Write("<br>" & Request.QueryString("dox") & "clikediiiiiiiii<br>")
    If Request.QueryString("which") = "" Or keyp = "update" Then
    %>
    <div id="dialog-confirm" title="Delete item?" style='display:none;'>
	<p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
    These items will be permanently deleted and cannot be recovered. Are you sure?</p>
</div>
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px;">
Attendance Entry

    <form name="frmattendance" id="frmattendance" action="" method="post" style="font-size:12pt;">
         <select id="monthx" name="monthx">
            <%  For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
       
         <select id="yearx" name="yearx">
            <%  For i As Integer = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
         <script type="text/javascript">
        <% if session("mmm")<>"" then %>
         $("#monthx").val("<% response.write(session("mmm")) %>");
         $("#yearx").val("<% response.write(session("yyr")) %>");
        <% else %>
 $("#monthx").val("<% response.write(today.month) %>");
 $("#yearx").val("<% response.write(today.year) %>");
        <% end if %>
                   
        </script>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:ssub();" />
    </form>
    </div>
    <div id='ppaytop' style="display:none; height:28px; font-size:9pt;">
     <div id='del' style="display:none;float:left;">
                <form id='frmdel' name='frmdel' action="" method="post">
    <input type="hidden" id='delpass' name='delpass' value='' />
    <input type="button" value='Delete' id='btndel' name='btndel' onclick="javascript: if(confirm('are you sure, Do you want delete all the Payroll data')==true){ deleted();}" style="height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; font-size:9px;" />
</form>
                </div>
                </div>
<div style="float:left">
 <div id="formouterbox_small" style="width:600px;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="msgbox"></span>

<form method='post' id='frmemp_att' name='frmemp_att' action=""> 
<table width="600px"><tr>
<td>Search by Name</td><td>:</td><td>
<input type='text' name='vname' id='vname' style='font-size:9pt;' onblur="javascript:ajaxsumit('byname');" /></td></tr><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<%response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
From Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='att_date' name='att_date' value='' /><br /><label class='lblsmall'></label></td>

<script language='javascript' type='text/javascript'> $(function() {$( "#att_date").datepicker({changeMonth: true,changeYear: true	}); $( "#att_date" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>

<td>To Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='to_date' name='to_date' value='' /><br /><label class='lblsmall'></label></td>

<script language='javascript' type='text/javascript'>    $(function () { $("#to_date").datepicker({ changeMonth: true, changeYear: true }); $("#to_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
</tr><tr><td>Attendance Status</td><td>:</td><td>
<select id="st" name="st">
    <option value="A">Absent</option>
 </select></td>
 <td>Duration</td><td>:</td><td>
<select id="daypartition" name="daypartition">
    <option value="F">Full Day</option>
    <option value="M">Morning</option>
    <option value="A">Afternoon</option>
 </select>

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
 
 
 <div id="listx" style=" width:50%; padding:0px 7px 0px 0px; font-size:10px;float:left">
   <% makerows2() %>
 </div>
 
    <div id="attsum" style="float:left">
   </div>
</div>
   <%  End If
       Dim db As New dbclass
       Dim dt As DataTableReader
       
       Dim mk As New formMaker
        If keyp = "update" Then
          
           dt = db.dtmake("new" & Today.ToLocalTime, "select * from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id"), Session("con"))
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
                Response.Write("$('#vname').val('" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & dt.Item("emptid"), Session("con")), Session("con")) & "');")
                Response.Write("$('#id').val('" & Request.QueryString("id") & "');")
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
                End If
                dt.Close()
          
            db = Nothing
            dt = Nothing
        End If
        Dim loc As String
        Loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
         
       %> 
  
   
 
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Attendance Entery Form.");
    //showobjar("formx","titlet",22,2);
   
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
       

           <%
           
           End If
           
       
           If Request.QueryString("takename") <> "" Then
               Response.Write("<script>$('#vname').val('" & Request.QueryString("vname") & "');</script>")

           End If
   
           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","_parent");
        $("#msgbox").val('<%=msg %>');
        //$('#frmx').attr("action","<% response.write(rd) %>");
        // $('#frmx').submit();
       </script>
   <%  End If
       
       
       %>
   <script>
    $(document).ready(function () {
        var link;
        link="month=<%=session("mmm") %>&year=<%=session("yyr") %>";
        alert(link);
        $("#attsum").load("attendancesum.aspx?"+link);
        }
        );
    </script>
</body>
</html>

