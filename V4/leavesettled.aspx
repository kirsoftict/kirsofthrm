<%@ Page Language="VB" AutoEventWireup="false" CodeFile="leavesettled.aspx.vb" Inherits="scripts_leavesettled" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%
    Session.Timeout = "60"
    
    Dim fm As New formMaker
    Dim fl As New file_list
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

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
	
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>

<script src="scripts/script.js" type="text/javascript"></script>
<% topscript()
    If Request.QueryString("type") = "edit" Then
        Response.Write("<script type='text/javascript'>var action; action='action=update';</script>")
    ElseIf Request.QueryString("type") = "new" Then
        Response.Write("<script type='text/javascript'>var action; action='action=save';</script>")
    End If
    %>
<script type="text/javascript">
    var focused = "";
    var requf = ["ref", "bno", "amt", "x"];
    var fieldlist = ["bno", "ref", "remark", "amt", "who_reg", "date_reg", "x"];
    function validation1() {
        if ($('#ref').val() == '') { showMessage('ref cannot be empty', 'ref'); $('#ref').focus(); return false; }
        if ($('#bno').val() == '') { showMessage('Balance days cannot be empty', 'bno'); $('#bno').focus(); return false; }
        if ($('#amt').val() == '') { showMessage('Amount cannot be empty', 'amt'); $('#amt').focus(); return false; }
        else if (focused == "") {
            var ans;
            ans = checkblur();
            if (ans != true) {
                $("#" + ans).focus();
            } else {//alert("intovalid");

                var str = $("#form1").formSerialize();
                //  alert(str)
               
                
                $("#form1").attr("action", "?"+action+"&tbl=leav_settled&"+str);
                $("#form1").submit();
                return true;
            }
        }
    }
    function calldisable(val) {
        $("#" + val).attr('disabled', 'disabled');
    }
</script>
    <title></title>
</head>
<body>
 

<form id='frmx' name='frmx' method="post"></form>
    <form id="form1" name="from1" action="" method="post">
    <div><table width="400px"><tr>
       <td>Emp. Computer id:</td> <td>
        <span id="emptid"></span><input name="emptidd" type="text" id="emptidd" style=" display:none" /></td>
       <td> Budget Id:</td><td><span id="bgtid"></span><input name="bgtidd" type="text"  id="bgtidd" style=" display:none" /></td></tr>
       <tr><td> Balance No. days:</td><td><input name="bno" type="text" value="" id="bno"  readonly="readonly"/></td></tr>
       <tr><td> Amount:</td><td><input name="amt" type="text" id="amt" /></td></tr>
       <tr><td> Ref.</td><td><input name="ref" type="text" id="ref" /></td></tr>
      <tr><td> Reamrk:</td><td><textarea name="remark" rows="7" cols="15" id="remark"></textarea></td></tr> 
        <tr><td>
        <input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>

        <input type="button" name="btnSave" id="btnSave" value='Save' onclick="calldisable('btnSave')"  /></td></tr>
 <%  If Request.QueryString("type") = "edit" Then
         Response.Write("<script>$('#btnSave').attr('title','Update');" & "$('#btnSave').attr('value','Update');</script>" & Chr(13))
     End If%>
     </table>

    </div>
    </form>
</body>
</html>
