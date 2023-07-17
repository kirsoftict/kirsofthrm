<%@ Page Language="VB" AutoEventWireup="false" CodeFile="aprperiod.aspx.vb" Inherits="aprperiod" %>
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
                    flg = dbx.excutes(sql, session("con"),session("path"))
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
               'Response.Write(sql)
                flg = dbx.excutes(sql, Session("con"), Session("path"))
                    If flg = 1 Then
                    msg = "Data Saved"
                    End If
                End If
                'MsgBox(rd)
         
                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
            '  Response.Write(flg)
                If flg < 1 and flg<>0  Then
                    Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
                End If
          
   
   End If
End If
    Dim fm As New formMaker
    Dim periodx As String = ""
    periodx = fm.getjavalist2("apr_period", " distinct period ", Session("con"), "")
    
   %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Untitled Document</title>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 

<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
    		<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>

<link rel="stylesheet" href="css/bootstrap.css" />
<script type="text/javascript">
 var periodq;
    <%if periodx <>"" then %>
    periodq=[<% response.write(periodx) %>];
    <%else %>
     periodq=[<% response.write(periodx) %>];
    <%end if %>
    var prv;
    prv = "";
    var id;
    var focused = "";
    var requf = ["period", "period_start", "period_end", "x"];
    var fieldlist = ["period", "period_start", "period_end", "who_reg", "date_reg", "x"];
    function validation1() {
        if ($('#period').val() == '') { showMessage('period cannot be empty', 'period'); $('#period').focus(); return false; }
        if ($('#period_start').val() == '') { showMessage('period_start cannot be empty', 'period_start'); $('#period_start').focus(); return false; }
        if ($('#period_end').val() == '') { showMessage('period_end cannot be empty', 'period_end'); $('#period_end').focus(); return false; }
           else if (focused == "") {
            var ans;
            ans = checkblur();
            if (ans != true) {
                $("#" + ans).focus();
            } else {
                var str = $("#frmapr_period").formSerialize();
                $("#frmapr_period").attr("action", "?tbl=apr_period&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
                $("#frmapr_period").submit();
                return true;
            }
        }
    }

   
  
    </script>
    <script src="scripts/kirsoft.java.js" type="text/javascript"></script>
    <style>
    body{
	background: #ccc;
}
.form-style-9{
	max-width:400px;
	margin:auto auto;
	background:#fff;
	border-radius:3px;
	padding:20px;
	font-family: Georgia, "Times New Roman", Times, serif;
	
}
.form-style-9 ul{
	padding:0;
	margin:0;
	list-style:none;
}
.form-style-9 ul li{
	display: block;
	margin-bottom: 10px;
	min-height: 35px;
}
.form-style-9 ul li  .field-style{
	box-sizing: border-box; 
	-webkit-box-sizing: border-box;
	-moz-box-sizing: border-box; 
	padding: 8px;
	outline: none;
	border: 1px solid #B0CFE0;
	-webkit-transition: all 0.30s ease-in-out;
	-moz-transition: all 0.30s ease-in-out;
	-ms-transition: all 0.30s ease-in-out;
	-o-transition: all 0.30s ease-in-out;

}.form-style-9 ul li  .field-style:focus{
	box-shadow: 0 0 5px #B0CFE0;
	border:1px solid #B0CFE0;
}
.form-style-9 ul li .field-split{
	width: 49%;
}
.form-style-9 ul li .field-full{
	width: 100%;
}
.form-style-9 ul li input.align-left{
	float:left;
}
.form-style-9 ul li input.align-right{
	float:right;
}
.form-style-9 ul li textarea{
	width: 100%;
	height: 100px;
}
.form-style-9 ul li input[type="button"], 
.form-style-9 ul li input[type="submit"],
.form-style-9 ul li input[type="reset"] {
	-moz-box-shadow: inset 0px 1px 0px 0px #3985B1;
	-webkit-box-shadow: inset 0px 1px 0px 0px #3985B1;
	box-shadow: inset 0px 1px 0px 0px #3985B1;
	background-color: #216288;
	border: 1px solid #17445E;
	display: inline-block;
	cursor: pointer;
	color: #FFFFFF;
	padding: 8px 18px;
	text-decoration: none;
	font: 12px Arial, Helvetica, sans-serif;
}
.form-style-9 ul li input[type="button"]:hover, 
.form-style-9 ul li input[type="submit"]:hover,
.form-style-9 ul li input[type="reset"]:hover {
	background: linear-gradient(to bottom, #2D77A2 5%, #337DA8 100%);
	background-color: #28739E;
}
.form-style-9 li > span{
	background: #F3F3F3;
	display: block;
	padding: 3px;
	margin: 0 -9px -9px -9px;
	text-align: center;
	color: #a0a0a0;
	font-family: Arial, Helvetica, sans-serif;
	font-size: 11px;
}
</style>

</head>
<body>
           
           <div class="content-lg container">

 <div class="masonry-grid-item col-xs-12 col-sm-6 col-md-6 sm-margin-b-30">
                        <div class="margin-b-60">

<form class='form-style-9' method='post' id='frmapr_period' name='frmapr_period'>
<h1>Appraisal Period</h1> <span id="messagebox"></span> 
<ul><li><sup style='color:red;'>*</sup><input type='text' id='period' name='period' value='' onblur="javascript:onlost('period',periodq);" placeholder="Period" />
</li><li><sup style='color:red;'>*</sup><input type='text' id='period_start' name='period_start' value='' placeholder='Period Start' />

        <sup style='color:red;'>*</sup><input type='text' id='period_end' name='period_end' value='' Placeholder='Period End' />
</li>
<li><sup style='color:red;'>*</sup><input type='text' id='p_group' name='p_group' value='' placeholder="group like 2009" /><span>Please give group value for summery purpose</span>
<script language='javascript' type='text/javascript'>    $(function () { $("#period_start").datepicker({ changeMonth: true, changeYear: true }); $("#period_start").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
       
<script language='javascript' type='text/javascript'>    $(function () { $("#period_end").datepicker({ changeMonth: true, changeYear: true }); $("#period_end").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/></td>
<li><input type='button' name='btnSave' id='btnSave' value='Save' /><input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" />
   
    <sup style="color:Red;">*</sup>Required Fields</li></ul></form>
  
 </div></div>
        <div class="masonry-grid-item col-xs-12 col-sm-6 col-md-6 sm-margin-b-30">
                        <div class="margin-b-60">    
 
  <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from apr_period where id=" & Request.QueryString("id"), Session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
          For k As Integer = 0 To dt.FieldCount - 1
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
        Dim sqlx As String = "select id,period,period_start,period_end,p_group from apr_period order by p_group, id desc"
        row = mk.edit_del_list("apr_period", sqlx, "Period,Period From,Period upto,Group ", Session("con"), loc)
    Response.Write(row)
        
    %>
    </div></div></div></div>
<script type="text/javascript" language="javascript">
    hform = findh(document.getElementById("middle_bar"));
    $('.titlet').text("Appresal Period");
    //showobjar("formx","titlet",22,2);
</script>
</body>
</html>
