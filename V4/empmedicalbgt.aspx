<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empmedicalbgt.aspx.vb" Inherits="empmedicalbgt" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  dactive()
    Dim fm As New formMaker
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
                sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                Dim cc As String
                Dim dtv As Boolean
                cc = fm.getinfo2("select sum(amt_used) from emp_medical_all where m_id=" & keyval, Session("con"))
                If IsNumeric(cc) = False Then
                    cc = "0"
               Response.Write(cc.ToString)
                    
                End If
                
                    flg = dbx.save(sql, session("con"),session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                Dim cc As Integer
                cc = fm.getinfo2("select count(id) from emp_medical_take where m_id=" & Request.QueryString("id"), Session("con"))
                If cc > 0 Then
                    msg = "Sorry data Can't be delete, it has a data"
                    'Response.Write("<script>alert('" & msg & "');</script>")
                    flg = 0
                Else
                    flg = dbx.save(sql, session("con"),session("path"))
                End If
               
                   
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data deleted"
                End If
                ElseIf Request.QueryString("task") = "save" Then
                ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                    sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
                'Response.Write(sql)
                    flg = dbx.save(sql, session("con"),session("path"))
                    If flg = 1 Then
                    msg = "Data Saved"
                   
                    End If
                End If
                'MsgBox(rd)
                
                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
                If flg <> 1 and flg<>0 Then
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
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript">
	$(function() {
		$( "#hire_date" ).datepicker({
			defaultDate: "+1w",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			minDate: "-70Y", maxDate: "-1d",
			onSelect: function( selectedDate ) {
				$( "#end_date" ).datepicker( "option", "minDate", selectedDate );
				
			}
		});
		$( "#end_date" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#hire_date" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#end_date" ).datepicker({
			defaultDate: "+1w",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			maxDate: 0,
			onSelect: function( selectedDate ) {
				$( "#hire_date" ).datepicker( "option", "MaxDate", selectedDate );
			}
		});
	});
	</script>
<script type="text/javascript">
var prv;
  prv="";
var id;
var focused="";
var requf=["emptid","emp_id","mallwance","date_from","date_exp","x"];
var fieldlist=["emptid","emp_id","mallwance","date_from","date_exp","who_reg","date_reg","x"];
function validation1(){if ($('#emptid').val() == '') {showMessage('emptid cannot be empty','emptid');$('#emptid').focus();return false;}
if ($('#emp_id').val() == '') {showMessage('emp_id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#mallwance').val() == '') {showMessage('mallwance cannot be empty','mallwance');$('#mallwance').focus();return false;}
if ($('#date_from').val() == '') {showMessage('date_from cannot be empty','date_from');$('#date_from').focus();return false;}
if ($('#date_exp').val() == '') {showMessage('date_exp cannot be empty','date_exp');$('#date_exp').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_medical_all").formSerialize();
   $("#frmemp_medical_all").attr("action","?tbl=emp_medical_all&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&loc=<%response.write(request.querystring("loc")) %>&tar=<%response.write(request.querystring("tar")) %>&" + str);
    $("#frmemp_medical_all").submit();
  return true;}
  }
} </script>

 
</head>

<body style="height:auto;">
<div></div>
<div style="top:0px; width:450px; height:25px; padding:0px 0px 0px 0px; position:fixed; font-size:8pt;"> 

      <div id="mmenu"> 
       &nbsp;&nbsp;
         <span style='color:yellow; cursor:pointer; float:right;' onclick="javascript:gotoinfo('<% Response.Write(request.servervariables("URL")) %>')">Help</span>
        </div> 
        
        </div>

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
//window.location="empcontener.aspx";
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

 <div id="formouterbox_small" style='width:500px;'>
    
    <div id="forminner">
    <span id="messageboxx"></span>
<form method='post' id='frmemp_medical_all' name='frmemp_medical_all' action=""> 
<table><tr><td>
<input type='hidden' id='emp_id' name='emp_id' value="<%response.write(session("emp_id")) %>" />
<input type='hidden' id='emptid' name='emptid' value="<%response.write(session("emptid")) %>" />

Allowance Budgeted<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='mallwance' name='mallwance' value='' style="text-align:right;" /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Date From<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='date_from' name='date_from' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'> $(function() {$( "#date_from").datepicker({changeMonth: true,changeYear: true	}); $( "#date_from" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
<td>End Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='date_exp' name='date_exp' value='' /><br /><label class='lblsmall'></label>
<script language='javascript' type='text/javascript'> $(function() {$( "#date_exp").datepicker({changeMonth: true,changeYear: true	}); $( "#date_exp" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
</td>
</tr><tr><td><input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/><input type='button' name='btnSave' id='btnSave' value='Save' <% 
 if request.querystring("update")="onx" then
 response.write("Disabled='disabled'")
 end if %> /><input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" />
</td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
 
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_medical_all where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
          For k As Integer = 0 To dt.FieldCount - 4
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
        Dim sqlx As String = "select id,mallwance,date_from,date_exp from emp_medical_all where emp_id='" & Session("emp_id") & "' order by id desc"
        row = mk.edit_del_list("emp_medical_all", sqlx, "Allwance Budgeted,From,Upto", session("con"), loc)
            Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Medical Register");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_medical_all");
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
           If msg = "Data Saved" Then%>
           <script>
        //$(document).delay(80000);
        $('#frmx').attr("target","frm_tar");
       
        $('#frmx').attr("action","medicalout1.aspx");
       $('#frmx').submit();
    </script>
   <%     ElseIf msg = "Sorry data Can't be delete, it has a data" Then%>
   
    <script>
        alert("<% response.write(msg) %>");
        $('#frmx').attr("target", "frm_tar");

        $('#frmx').attr("action", "medicalout1.aspx");
        $('#frmx').submit();
    </script>
   <%
       
   End If
           
   
   %>
   
   
</body>
</html>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
