﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empbank.aspx.vb" Inherits="empbank" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
   pageon()

   %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<style>
    #messagebox
    {
        height:34px;
        color:red;
    }
</style>

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.button.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="scripts/kirsoft.java.js"></script>

  <%  Dim fm As New formMaker
      'Response.Write(fm.headerjq(9))
      Response.Write(fm.headerkirsoft)
      Response.Write(fm.headerkirsoftreq)
      Dim namelist As String = ""
      namelist = gnamelist()
       
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
 
var prv;
  prv="";
var id;
var focused="";
var requf=["vname","bankname","accountno","x"];
var fieldlist=["id","emptid","bankname","accountno","who_reg","date_reg","x"];
function validation1(){
if ($('#vname').val() == '') {showMessage('vname cannot be empty','vname');$('#vname').focus();return false;}
if ($('#bankname').val() == '') {showMessage('Bank name cannot be empty','bankname');$('#bankname').focus();return false;}
if ($('#accountno').val() == '') {showMessage('Account no. cannot be empty','accountno');$('#accountno').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{//alert("intovalid");

   var str=$("#frmempbank").formSerialize();
 //  alert(str)
   $("#frmempbank").attr("action","?tbl=empbank&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmempbank").submit();
  return true;}
  }
} 
    var namelist=[<% response.write(namelist) %>];
    
 
	
   
 </script>
</head>

<body style="height:auto;">


 <div id="formouterbox_small" style="width:600px;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messagebox"></span>
    <div id='dialogx'>
<form method='post' id='frmempbank' name='frmempbank' action=""> 
<table width="600px"><tr>
<td>Search by Name</td><td>:</td><td>
<input type='text' name='vname' id='vname' style='font-size:9pt;'   onkeyup="javascript:startwith('vname',namelist);" /></td></tr><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<%response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
Bank Name<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='bankname' name='bankname' value='CBE' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Branch.</td><td>:</td><td>
<input type="text" name="branch" id="branch" value="" />
</td></tr>
<tr><td>Account No.</td><td>:</td><td>
<input type="text" name="accountno" id="accountno" value="" />
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/></td>
</tr><tr><td>Active:<input type='text' name='active' id='active' value='y' size='2' /></td></tr><tr>

<td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr>
</table></form></div>
</div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
 
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from empbank where id=" & Request.QueryString("id"), session("con"))
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
                Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');")
            End If
            Response.Write("$('#vname').val('" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & dt.Item("emptid"), Session("con")), Session("con")) & "');</script>")
                    
                dt.Close()
            End If
            db = Nothing
            dt = Nothing
        Dim mk As New formMaker
            Dim row As String
            Dim loc As String
            loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "SELECT id, dbo.empbank.bankname,branch, dbo.empbank.accountno,emptid FROM empbank order by id desc"
        row = mk.edit_del_list_wname("empbank", sqlx, "Emp. Name,Bank Name,Branch,Acc. No.", Session("con"), loc)
            Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Bank Acc.");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=empbank");
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
          if msg<>"" then
               Response.Write("<script type='text/javascript'>$('#messagebox').text('" & msg & "');</script>")
          end if
       
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
    $(document).ready(function () {
   
      
    });

</script>