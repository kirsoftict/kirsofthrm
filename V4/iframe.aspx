<%@ Page Language="VB" AutoEventWireup="false" CodeFile="iframe.aspx.vb" Inherits="iframe" %>
<%@ Import Namespace="kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Untitled Document</title>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jqq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
	<link rel="stylesheet" href="css/demos.css" />

    <style type="text/css">
.form-style-2{
	max-width: 500px;
	padding: 20px 12px 10px 20px;
	font: 13px Arial, Helvetica, sans-serif;
}
.form-style-2-heading{
	font-weight: bold;
	font-style: italic;
	border-bottom: 2px solid #ddd;
	margin-bottom: 20px;
	font-size: 15px;
	padding-bottom: 3px;
}
.form-style-2 label{
	display: block;
	margin: 0px 0px 15px 0px;
}
.form-style-2 label > span{
	width: 100px;
	font-weight: bold;
	float: left;
	padding-top: 8px;
	padding-right: 5px;
}
.form-style-2 span.required{
	color:red;
}
.form-style-2 .tel-number-field{
	width: 40px;
	text-align: center;
}
.form-style-2 input.input-field{
	width: 48%;
	
}

.form-style-2 input.input-field, 
.form-style-2 .tel-number-field, 
.form-style-2 .textarea-field, 
 .form-style-2 .select-field{
	box-sizing: border-box;
	-webkit-box-sizing: border-box;
	-moz-box-sizing: border-box;
	border: 1px solid #C2C2C2;
	box-shadow: 1px 1px 4px #EBEBEB;
	-moz-box-shadow: 1px 1px 4px #EBEBEB;
	-webkit-box-shadow: 1px 1px 4px #EBEBEB;
	border-radius: 3px;
	-webkit-border-radius: 3px;
	-moz-border-radius: 3px;
	padding: 7px;
	outline: none;
}
.form-style-2 .input-field:focus, 
.form-style-2 .tel-number-field:focus, 
.form-style-2 .textarea-field:focus,  
.form-style-2 .select-field:focus{
	border: 1px solid #0C0;
}
.form-style-2 .textarea-field{
	height:100px;
	width: 55%;
}
.form-style-2 input[type=submit],
.form-style-2 input[type=button]{
	border: none;
	padding: 8px 15px 8px 15px;
	background: #FF8500;
	color: #fff;
	box-shadow: 1px 1px 4px #DADADA;
	-moz-box-shadow: 1px 1px 4px #DADADA;
	-webkit-box-shadow: 1px 1px 4px #DADADA;
	border-radius: 3px;
	-webkit-border-radius: 3px;
	-moz-border-radius: 3px;
}
.form-style-2 input[type=submit]:hover,
.form-style-2 input[type=button]:hover{
	background: #EA7B00;
	color: #fff;
}

</style>

<body style="height:auto;">

<%  Response.Write(Request.QueryString("what"))
    Dim sc As New k_security
    Response.Write(sc.d_encryption("zewde@123"))
    If Request.Form.HasKeys = True Then
        Dim db As New dbclass
        Dim sql As String = ""
        'sql = db.makest("emp_address", Request.Form, session("con"))
        ' Response.Write(sql)
        'db.save(sql, session("con"),session("path"))
    End If
    For Each p As String In Request.Form
      '  Response.Write(" <br />" & p & "=>" & Request.Form(p))
 
    Next
    For Each k As String In Request.ServerVariables
        ' Response.Write("new <br />" & k & "=>" & Request.ServerVariables(k))
    Next
    Dim fm As New formMaker
    'fm.fdb("F:\kirold\WebSite2\diff data\listposition.txt", session("con"))
    If Request.Form("tblname") = "" Then
        %>
            <form method="post" action=""><input type='text' name='tblname' /><input type="submit" value='go' /></form>
        <%
        Else
            %>
            
<% 
    Dim rtn As String = Me.form(Session("con"), Request.Form("tblname"))
    ' Response.Write("<code>" & rtn & "</code>")
    Response.Write(rtn)
    fm = Nothing
    %>
  
            <%
        End If
 %>
 
 

<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("New Jobs entry");
    //showobjar("formx","titlet",22,2);
</script>
</body>
</html>
