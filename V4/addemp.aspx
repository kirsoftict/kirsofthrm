<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addemp.aspx.vb" Inherits="addemp" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%  If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
       End If
       If Session("emp_id") <> "" Then
           Session("emp_id") = ""
       End If
       Session("emptid") = ""
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<script src="scripts/script.js" type="text/javascript"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>

<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	</script><script type="text/javascript" src="jq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>

<link rel="stylesheet" href="jq/demos.css" />
<%  Dim dbc As New dbclass
    Response.Write("<script type='text/javascript'>var listname=")
    Response.Write(dbc.listnames(session("con"), "first_name,middle_name,last_name", "emp_static_info"))
    Response.Write("</script>")
    Dim listids As String
    listids = dbc.idlist_jqry("emp_static_info", "emp_id", session("con"))
    
    dbc = Nothing
    %>
   <script language="javascript" type="text/javascript">
var av = [<% response.write(listids)%>];
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
    else if(keycheck()==true)
    {
        $("#messagebox").text("Sorry! Full Name value is Duplicate");
        $("#fullname").css({'border':'1px solid red'});
        $("#first_name").focus();

    }
    else{
    str=$("#frmemp_static_info").formSerialize();
 //alert(str);
 $("#messagebox").load("open.aspx?tbl=emp_static_info&lrd=empcontener&key=emp_id&rd=empcontener.aspx&" + str);
 //   $('#frmemp_static_info').submit();
    return true;}
    }
    //else
    //alert(focused);
}
   </script>

</head>

<body style="height:450px;">

<%  ' Response.Write(Request.QueryString("what"))
    
    Dim sc As New k_security
   
    ' Response.Write(sc.d_encryption("zewde@123"))
    If Request.Form.HasKeys = True Then
        Dim db As New dbclass
        Dim sql As String
        sql = db.makest("emp_static_info", Request.Form, session("con"), "emp_id")
        '  Response.Write(sql)
        ' flg = db.save(sql, session("con"),session("path"))
        '  Response.Write(flg)
    End If
    For Each p As String In Request.Form
        ' Response.Write(" <br />" & p & "=>" & Request.Form(p))
 
    Next
    For Each k As String In Request.ServerVariables
        ' Response.Write("new <br />" & k & "=>" & Request.ServerVariables(k))
    Next
    Dim fm As New formMaker
 %>
 <% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

 <div id="formouterbox" style="width:750px;">
    <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" title="Close" id="clickclose" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div>
        <div class="head2">&nbsp;</div>
        <div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
 <span id="messagebox"></span>
  <form method='post' id='frmemp_static_info' action="" enctype="multipart/form-data"> 
<table cellpadding="5px"><tr><td colspan="4">
<table><tr>
<td> Employee Id<sup style='color:red;'>*</sup></td><td>:</td>
<td class="ui-widget">
<input type='text' id='emp_id' name='emp_id' onblur="javascript:onlost('emp_id',av);" /> </td><td> File No</td><td>:</td>
<td><input type='text' id='file_no' name='file_no' /></td>
<td>Tin (optional)</td><td>:</td>
<td><input type='text' id='emp_tin' name='emp_tin' maxlength="11" />
</td></tr>
<tr><td colspan='6'>&nbsp;</td><td style="" >Pen No.(optional)</td><td>:</td>
<td style="">
<input type='text' id='emp_pen' name='emp_pen' maxlength="20" /></td></tr>
</table>
</td></tr>
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
<tr><td>
<table cellspacing="5px"><tr>
<td>sex<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id='sex' name='sex'>
<option value="Male">Male</option>
<option value="Female">Female</option>
</select>
</td>
<td>Date of Birth<sup style='color:red;'>*</sup>&nbsp;:&nbsp;
</td><td>:</td><td><input type='text' id='dob' name='dob' />
<script type="text/javascript" language='javascript'> 
$(function() {$( "#dob").datepicker({changeMonth: true,changeYear: true,minDate: "-70Y", maxDate: "-18Y"}); 
$( "#dob" ).datepicker( "option","dateFormat","mm/dd/yy");});
</script></td>
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
<input type="button" name="btnSave" id="btnSave" value="Save" />
          <input type="reset" /></td></tr>
          </table>
          </form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>

<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Add New Employee");
    //showobjar("formx","titlet",22,2);
   $('#nationality').val("Ethiopian");
  </script>
  <% 
   If Session("emp_id") <> "" Then
          Response.Write("<script type='text/javascript'>" & _
              "$('#messagebox').val('" & Session("emp_id") & "')</script>")
        
    End If%>
    <form id='frmx' name='frmx' action='' method="post"></form>
</body>
</html>


