<%@ Page Language="VB" AutoEventWireup="false" CodeFile="iframe2.aspx.vb" Inherits="iframe" %>
<%@ Import Namespace="kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Untitled Document</title>
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
	<link rel="stylesheet" href="../demos.css" />


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
        Response.Write(" <br />" & p & "=>" & Request.Form(p))
 
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
            <div id="formouterbox">
    <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" title="Close" id="clikclose" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
 <span id="messagebox"></span>
<%  Response.Write(form(Session("con"), Request.Form("tblname")))
    fm = Nothing
    %>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
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
