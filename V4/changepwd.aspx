<%@ Page Language="VB" AutoEventWireup="false" CodeFile="changepwd.aspx.vb" Inherits="changepwd" %>
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
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	<link rel="stylesheet" href="jq/demos.css" />
 <script language="javascript" type="text/javascript">
 var pwd='<% response.write(session("password")) %>';
var requf=["old_password","password","cpassword"];
var fieldlist=["old_password","password","cpassword"];
  var focused="";
function validation1(){ 
if ($('#old_password').val() == '') {showMessage('Old_password cannot be empty','old_password');$('#old_password').focus();return false;}
if ($('#password').val() == '') {showMessage('New Password cannot be empty','password');$('#password').focus();return false;}
if ($('#cpassword').val() == '') {showMessage('confirm Password cannot be empty','cpassword');$('#cpassword').focus();return false;}
if ($('#cpassword').val() != $('#password').val()) {showMessage('confirm Password not same the password','cpassword');$('#cpassword').focus();return false;}

 else if(focused=="")
  { 
  
    var ans;
    ans=checkblur();
 //  alert(ans);
    if(ans!=true){
     $("#" + ans).focus();
   
    }
       else{
    str=$("#frmpwdchg").formSerialize();
 //alert(str);
//$('#frmemp_static_info').attr("target","_self");
$('#frmpwdchg').attr("action","?task=update&username=<% response.write(session("username")) %>");
 $('#frmpwdchg').submit();
    return true;}
    }
    //else
    //alert(focused);
}

 
 </script>

</head>

<body style="height:auto;">
<% Dim fm As New formMaker
    
    'Response.Write(fm.helpback(False, True, False, False, "", ""))%>
 <div id="formouterbox" style="width:400px;">
    <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" title="Close" id="clickclose" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
 <span id="messagebox"></span>
  <form name="frmpwdchg" id="frmpwdchg" action="" method="post">
    <table>
    	<tr><td style=" vertical-align:text-top; width:25%;">Old Password</td>
        		<td style=" vertical-align:text-top; width:5%;">:</td>
        		<td style=" vertical-align:text-top; width:70%;"><input type="password" name="old_password" id="old_password" />
        		</td></tr><tr><td>
        		New Password</td><td>:</td><td><input type="password" name="password" id="password" /></td>
        		</tr>
        		<tr><td>
        		Confirm Password</td><td>:</td><td><input type="password" name="cpassword" id="cpassword" /></td>
        		</tr>
        		<tr><td>
        		</td><td colspan="3" align="center"><input type="button" id="btnSave" name="btnSave" value="Change" />&nbsp;
        		<input type="button" id="btncancel" name="btncancel" value="Cancel" />&nbsp;</td>
        		</tr>
    </table>
    </form>
    </div>
    <sup style="color:Red;"></sup>All Fields are Required 
 </div>

<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Change Password");
  </script>
  <%  Dim flg As Integer = 0
      Dim db As New dbclass
      If Request.QueryString.HasKeys = True Then
          If Request.QueryString("task") = "update" Then
              Dim sql As String
              sql = "update login set oldpass='" & Request.Form("old_password") & "', password='" & Request.Form("password") & _
              "',datechanged='" & Now & "' where username='" & Session("username") & "'"
              If Session("emp_id") <> "" Then
                  sql = "update login set oldpass='" & Request.Form("old_password") & "', password='" & Request.Form("password") & _
               "',datechanged='" & Now & "' where username='" & Session("emp_id") & "'"
              End If
              If sql <> "" Then
                  flg = db.save(sql, session("con"),session("path"))
              End If
              If flg = 1 Then
                   %>
              <script type="text/javascript">
                $('#messagebox').text("Password is changed");
              </script>
              <%     
              Else   
                  %>
    <script type="text/javascript">
                $('#messagebox').text("Sorry password is not changed try again");
    </script>
    <%     
              End If
             
          End If
      End If
  %>
</body>
</html>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>



