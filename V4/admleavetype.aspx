﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="admleavetype.aspx.vb" Inherits="admleavetype" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%   If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
       End If
    Dim keyp As String = ""
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
    If Request.QueryString.HasKeys = True And Request.QueryString("task") <> "" Then
        If Request.QueryString("dox") = "" Then
            keyval = Request.QueryString("keyval")
            If Request.QueryString("task") = "update" Then
                   'e("<script type='text/javascript'>alert('updating....');</script>")
                sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                   flg = dbx.save(sql, session("con"),session("path"))
                   '  Response.Write(sql)
                If flg = 1 Then
                       msg = "Data Updated"
                End If
            ElseIf Request.QueryString("task") = "delete" Then
                   Response.Write("<script type='text/javascript'>//alert('deleting....');</script>")
                   'sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                   sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                   flg = dbx.save(sql, session("con"),session("path"))
                   
                ' Response.Write(sql)
                If flg = 1 Then
                    msg = "Data deleted"
                End If
            ElseIf Request.QueryString("task") = "save" Then
                   ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
                ' Response.Write(sql)
                flg = dbx.save(sql, session("con"),session("path"))
                If flg = 1 Then
                    msg = "Data Saved"
                End If
            End If
            'MsgBox(rd)
         
            ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            If flg <> 1 Then
                ' Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
            End If
          
   
        End If
    End If
       Dim fm As New formMaker
   %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
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

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/script.js" type="text/javascript"></script>

	<script type="text/javascript" src="scripts/form.js"></script>

  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>

	</head>
<%  Dim dbc As New dbclass
   
    Dim listids As String
    listids = dbc.idlist_jqry("tbl_leave_type", "leave_type", session("con"))
    
    dbc = Nothing
    %>
   <script type="text/javascript">
   var listx=[<% response.write(listids) %>];
var prv;
  prv="";
var id;
var focused="";
var requf=["leave_type","active","x"];
var fieldlist=["leave_type","no_days_allowed","who_reg","date_reg","active","x"];
function validation1(){if ($('#leave_type').val() == '') {showMessage('leave_type cannot be empty','leave_type');$('#leave_type').focus();return false;}
if ($('#active').val() == '') {showMessage('active cannot be empty','active');$('#active').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmtbl_leave_type").formSerialize();
   $("#frmtbl_leave_type").attr("action","?tbl=tbl_leave_type&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmtbl_leave_type").submit();
  return true;}
  }
} </script>

<body style="height:auto;">
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>
 <div id="formouterbox" style="width:600px;">
    <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" title="Close" id="clikclose" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
 <span id="messagebox">
 <% if msg<>"" then
 response.write(msg)
 end if %>
 </span>
 <form method='post' id='frmtbl_leave_type' name='frmtbl_leave_type' action=""> 
<table><tr><td>leave_type<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='leave_type' name='leave_type' value='' /><br /><label class='lblsmall'></label></td></tr>
<tr>
<td>no_days_allowed<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='no_days_allowed' name='no_days_allowed' value='' /><br /><label class='lblsmall'></label>
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString 
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" /></td></tr><tr><td>active<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='active' name='active' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td colspan='4'>
<input type='button' name='btnSave' id='btnSave' value='Save' /><input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from tbl_leave_type where id=" & Request.QueryString("id"), session("con"))
            If dt.HasRows = True Then
                dt.Read()
                Response.Write("<script type='text/javascript'>")
                For k As Integer = 0 To dt.FieldCount - 1
                    %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k) & "');")%>
                    <%
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
            Dim sqlx As String = "select id,leave_type,no_days_allowed,active from tbl_leave_type order by id desc"
            row = mk.edit_del_list("tbl_leave_type", sqlx, "Leave type,No. Days Allow,Active", session("con"), loc)
            Response.Write(row)
            ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
                con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
                "<img src='images/gif/btn_yes.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','yes','del123');" & Chr(34) & "> " & _
                "<img src='images/gif/cancel.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','no','del123');" & Chr(34) & "></span>"
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
		resizable: true
			
		});
           </script>
           <%
           
       End If  
        %>
 </div>
<script type="text/javascript" language="javascript">
//hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Add Leave type");
    //showobjar("formx","titlet",22,2);
</script>
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=tbl_leave_type");
        $('#frmx').submit();
            }
            else
            {
                 $('#frmx').attr("target","_self");
        $('#frmx').attr("action","?task=&id="+val+"&tbl=tbl_leave_type");
        $('#frmx').submit();

            }
       }
       
   </script>
    <form id="frmx" action="" method="post">
    </form>
    <script type="text/javascript">
  $(document).ready(function() {

  
$( "#leave_type" ).autocomplete({
			source: listx
		});
  });
  </script>

</body>
</html>

