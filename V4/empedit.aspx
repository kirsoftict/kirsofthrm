<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empedit.aspx.vb" Inherits="empedit" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>


<%  Dim sc As New k_security
    Dim db As New dbclass
    Dim sql As String = ""
    Dim flg As Integer = 0
    Dim flg2 As Integer = 0
    ' Response.Write(sc.d_encryption("zewde@123"))
    Dim rd As String = ""
      
    Dim tbl As String = ""
    Dim key As String = ""
    Dim keyval As String = ""
    If Request.QueryString.HasKeys = True Then
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        keyval = Request.QueryString("keyval")
        If Request.QueryString("task") = "update" Then
            Response.Write("<script type='text/javascript'>alert('updating....');</script>")
            sql = db.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
            flg = db.save(sql, Session("con"), Session("path"))
            Response.Write(sql)
            If flg = 1 Then
                Response.Write("<span style='font-size:15pt; color:Green;'> Data Updated </span>")
            End If
        Else
            Response.Write("<script type='text/javascript'>//alert('saving....');</script>")

            sql = db.makest(tbl, Request.QueryString, Session("con"), key)
            ' Response.Write(sql)
            flg = db.save(sql, Session("con"), Session("path"))
            If flg = 1 Then
                If Request.QueryString("emp_id") <> "" Then
                    Session("emp_id") = Request.QueryString("emp_id")
                End If
                Response.Write("<span style='font-size:15pt; color:Green;'> Data Saved </span>")
            End If
        End If
        'MsgBox(rd)
         
        ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
    End If
    Dim dbc As New dbclass
    Response.Write("<script type='text/javascript'>var listname=")
    Response.Write(dbc.listnames(session("con"), "first_name,middle_name,last_name", "emp_static_info"))
    Response.Write("</script>")
    dbc = Nothing
    Dim empid As String = ""
    empid = Session("emp_id")
    If Session("username") = "" Then
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
    %>
   <script language="javascript" type="text/javascript">

 var requf=["emp_id","first_name","middle_name","last_name","sex","dob","nationality"];
 var fieldlist=["emp_id","file_no","first_name","middle_name","last_name","sex","dob","nationality","marital_status","emp_tin","x"];
 var prv;
  prv="";
      var id;
  var focused="";
function validation1(){ $("#fullname").css({'border':'none'});
if ($('#emp_id').val() == '') {showMessage('emp_id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#first_name').val() == '') {showMessage('first_name cannot be empty','first_name');$('#first_name').focus();return false;}
if ($('#middle_name').val() == '') {showMessage('middle_name cannot be empty','middle_name');$('#middle_name').focus();return false;}
if ($('#last_name').val() == '') {showMessage('last_name cannot be empty','last_name');$('#last_name').focus();return false;}
if ($('#sex').val() == '') {showMessage('sex cannot be empty','sex');$('#sex').focus();return false;}
if ($('#dob').val() == '') {showMessage('dob cannot be empty','dob');$('#dob').focus();return false;}
if ($('#nationality').val() == '') {showMessage('nationality cannot be empty','nationality');$('#nationality').focus();return false;}
 else if(focused=="")
  { 
   
    var ans
    ans=checkblur();
   // alert(ans);
    if(ans!=true){
     $("#" + ans).focus();
     alert(ans);
    }
    
    else{
    str=$("#frmemp_static_info").formSerialize();
        alert(str);
        $("#frmemp_static_info").attr("action", "?tbl=emp_static_info&task=update&lrd=empcontener&key=emp_id&keyval=<% response.write(empid) %>&rd=empcontener.aspx&" + str);
        alert("?tbl=emp_static_info&task=update&lrd=empcontener&key=emp_id&keyval=<% response.write(empid) %>&rd=empcontener.aspx&" + str);
        $("#frmemp_static_info").submit();
  return true;  }
    }
    //else
    //alert(focused);
}

 
   </script>

</head>

<body style="height:auto;">

<%  'Response.Write(Request.QueryString("what"))
    
    'Response.Write(sc.d_encryption("zewde@123"))
    If Request.Form.HasKeys = True Then
        ' Dim db As New dbclass
        ' Dim sql As String
        'sql = db.makest("emp_address", Request.Form, session("con"))
        '  Response.Write(sql)
        'db.save(sql, session("con"),session("path"))
    End If
    ' For Each p As String In Request.Form
    ' Response.Write(" <br />" & p & "=>" & Request.Form(p))
 
    '  Next
    '  Response.Write(" <br />=>" & Request.Form(Session("emp_id")))

    For Each k As String In Request.ServerVariables
        ' Response.Write("new <br />" & k & "=>" & Request.ServerVariables(k))
    Next
    Dim fm As New formMaker
 %>

 <div id="formouterbox_small">
    <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" title="Close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;..</div>
        </div>
    <div id="forminner">
 <span id="messagebox"></span>
  <form method='post' id='frmemp_static_info' action="" enctype="multipart/form-data"> 
<table cellpadding="5px"><tr><td colspan="4">
<table><tr><td> Employee Id<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='hidden' id='emp_id' name='emp_id' />
<input type='hidden' id='imglink' name='imglink' /><input type='text' id='idview' name='idview' /></td><td> File No</td><td>:</td>
<td><input type='text' id='file_no' name='file_no' /></td>
<td>Tin (optional)</td><td>:</td>
<td><input type='text' id='emp_tin' name='emp_tin' maxlength="11" />
</td></tr>
<tr>
<td colspan='6'>&nbsp;</td><td style="" >Pen No.(optional)</td><td>:</td>
<td style="">
<input type='text' id='emp_pen' name='emp_pen' maxlength="20" /></td></tr>
</table>
<tr><td colspan="4"><hr /></td></tr>
<tr><td colspan="4">
<table id="fullname"><tr><td>Full Name</td><td>:</td><td>
<input type='text' id='first_name' name='first_name'/><br /><label class="lblsmall"> First Name<sup style='color:red;'>*</sup></label></td>
<td><input type='text' id='middle_name' name='middle_name' /><br /><label class="lblsmall">Middle Name<sup style='color:red;'>*</sup></label>
</td>
<td><input type='text' id='last_name' name='last_name' /><br />
<label class="lblsmall"> Last Name<sup style='color:red;'>*</sup></label></td></tr>
</table>
</td></tr>
<tr><td colspan="4">
<hr /></td></tr>
<tr><td style="width: 256px">
<table cellspacing="5px"><tr>
<td>sex<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id='sex' name='sex'>
<option value="Male">Male</option>
<option value="Female">Female</option>
</select>
</td>
<td>Date of Birth<sup style='color:red;'>*</sup>&nbsp;:&nbsp;
</td><td>:</td><td><input type='text' id='dob' name='dob' />
</td>
</tr><tr>
<td>Nationality<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id='nationality' name='nationality'>
<option value="">Select Country</option>
<%Response.Write(fm.getoption("nationality", "country", "country", session("con")))%></select></td>
<td>Marital Status<sup style='color:red;'></sup></td><td>:</td><td>
<select id='marital_status' name='marital_status'>
<option value="">Select</option>
<%  Response.Write(fm.getoption("martial_status", "stat", "stat", session("con")))%>

</select>
</td>
</tr>
</table>
</td></tr>

<tr><td colspan="4">
<input type='hidden' id='date_of_reg' name='date_of_reg' value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" />
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>"/>
<input type="button" name="btnsave" id="btnsave" value="Edit" />
          </td></tr>
          </table>
          </form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
<%  Dim dbx As New dbclass
      Dim arr() As String
    arr = dbx.edited_field("emp_static_info", session("con"), "emp_id", Session("emp_id"))
    Dim strfil As String = arr(0)
      Dim enable As String = arr(1)
  
      
      
      %>
    
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Edit Personal Information");
    //showobjar("formx","titlet",22,2);
   $('#nationality').val("Ethiopian");
   <% response.write(strfil) %>
  </script>
  
  
</body>
</html>

<script type="text/javascript">
 $('#idview').val($('#emp_id').val());
$("#frmemp_static_info").data('edit',1); 


                $('#btnsave').click(function() {
                    var editMode = $("#frmemp_static_info").data('edit');
                    if (editMode == 1) { // ha('formouterbox');
                        <% response.write(enable) %>
                      $('#idview').val($('#emp_id').val());
                       document.getElementById('idview').disabled="disabled";
                        $("#frmemp_static_info").data('edit', 0);
                        this.value = "Update";
                        this.title = "Update";
                      //showobj('formouterbox');
                    } else {
                           validation1();
                 }});
                 
               
</script>
