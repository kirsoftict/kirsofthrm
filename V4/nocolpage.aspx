﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="nocolpage.aspx.vb" Inherits="nocolpage" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
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
                    Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
                End If
          
   
   End If
End If

   %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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

   <script language="javascript" type="text/javascript">
   var requf=["street1","mob","pemail","hno","kebele","worda","subcity"];
   var fieldlist=["street1","street2","htel","wtel","mob","pemail","wemail","postal","subcity","worda","kebele","hno","date_reg","who_reg","x"];
  var prv;
  prv="";
      var id;
  var focused="";
   

 function validation1(){

 if ($('#street1').val() == '') {showMessage('*Required','street1');$('#street1').focus();return false;}
 if ($('#mob').val() == '') {showMessage('*Required','mob');$('#mob').focus();return false;}
 if ($('#pemail').val() == '') {showMessage('*Required','pemail');$('#pemail').focus();return false;}
 if ($('#subcity').val() == '') {showMessage('*Required','subcity');$('#subcity').focus();return false;}
  if ($('#worda').val() == '') {showMessage('*Required','worda');$('#worda').focus();return false;}
 if ($('#kebele').val() == '') {showMessage('*Required','kebele');$('#kebele').focus();return false;}
 if ($('#hno').val() == '') {showMessage('*Required','hno');$('#hno').focus();return false;}
 else if(focused=="")
 { //alert("called validation");
    var ans
    ans=checkblur();
    if(ans!=true){ 
     // alert("called validation");
     $("#" + ans).focus();
  
    }
    else{
   // alert("called validation2");
   var str=$("#frmemp_address").formSerialize();
 //alert(str);
  //$("#messagebox").load("open.aspx?tbl=emp_static_info&task=update&lrd=empcontener&key=emp_id&rd=empcontener.aspx&" + str);
   // $('#frmemp_static_info').submit();
 
 // alert("open.aspx?tbl=emp_address&task=save&lrd=empcontener&key=emp_id&rd=empcontenener.aspx&" + str);
    $("#frmemp_address").attr("action","?tbl=emp_address&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    
   // $("#messagebox").load("open.aspx?tbl=emp_address&rd=&" + str);
    $('#frmemp_address').submit();
  
    return true;}
    }
 }
   </script>

</head>

<body style="height:auto;">
<div>
 </div>
    <% If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
       End If
       
       If Session("emp_id") = "" Then
       %>

<script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="empcontener.aspx";
</script>

<%
Else
     %>

<script type="text/javascript">
//alert('<% response.write(session("emp_id")) %>');
</script>

<%    
End If
'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))
If Request.Form.HasKeys = True Then
    'Dim db As New dbclass
    ' Dim sql As String
    ' sql = db.makest("emp_address", Request.Form, session("con"), "")
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
 <form method='post' id='frmemp_address' name="frmemp_address" action=''>
  <table width="591" cellpadding='2' cellspacing='2'><tr>
    <td>Street1<sup style='color:red;'>*</sup></td><td>:</td><td><input type="hidden" id="emp_id" name="emp_id" value="<% response.write(session("emp_id")) %>" /><input type='text' id='street1' name='street1' /></td></tr>
    <tr>
      <td>Street2</td><td>:</td><td><input type='text' id='street2' name='street2' /></td></tr>
      <tr>
  <td>Mobile<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='mob' name='mob' /></td>
  </tr><tr><td>Home phone</td><td>:</td><td><input type='text' id='htel' name='htel' /></td>
    <td>Work phone</td><td>:</td><td><input type='text' id='wtel' name='wtel' /></td>
    </tr>
    <tr>
        <td colspan='8'> <hr /></td></tr><tr>
    
      <td>Personal email<sup style='color:red;'>*</sup></td><td>:</td><td>
      <input type='text' id='pemail' name='pemail'  /></td>
    <td>Work email</td><td>:</td><td><input type='text' id='wemail' name='wemail'  /></td></tr>
    <tr>
         <td>Postal Code</td><td>:</td><td><input type='text' id='postal' name='postal' /></td></tr><tr>
        <td colspan='8'> <hr /></td></tr><tr>
         <td>Sub City<sup style='color:red;'>*</sup></td><td>:</td><td>
         <input type='text' id='subcity' name='subcity' />
         </td>
         <td>worda<sup style='color:red;'>*</sup></td><td>:</td><td>
         <input type='text' id='worda' name='worda' />
         </td></tr><tr>
         <td>kebele<sup style='color:red;'>*</sup></td><td>:</td><td>
         <input type='text' id='kebele' name='kebele' /></td>
         <td>hno<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='hno' name='hno' /></td></tr>
          <tr><td><input type='hidden' id='date_reg' name='date_reg' value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" /></td><td>
          <input type='hidden' id='who_reg' name='who_reg' value="<% response.write(session("username"))%>" /></td></tr>
          <tr><td colspan="6" align="center">
          <input type="hidden" id="btnv" name="btnv" value="save" />
          <input type="button" name="btnSave" id="btnSave" value="Save" />
          <input type="reset" /></td></tr></table>
          </form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_address where id=" & Request.QueryString("id"), session("con"))
            If dt.HasRows = True Then
                dt.Read()
                Response.Write("<script type='text/javascript'>")
                For k As Integer = 0 To dt.FieldCount - 4
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
            Dim sqlx As String = "select id,street1,street2,htel,wtel,mob,pemail,postal,subcity,worda,kebele,hno from emp_address where emp_id='" & Session("emp_id") & "' order by id desc"
            row = mk.edit_del_list("emp_address", sqlx, "Street1,Street2,Home Tel,Work Tel,Mob,Email,P.O.Box,Sub-city,Worda,Kebele,HNo.", session("con"), loc)
            Response.Write(row)
        
        %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Contact Details");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
            alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_address");
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
   <%  End If%>
   
   
</body>
</html>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
