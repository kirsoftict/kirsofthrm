<%@ Page Language="VB" AutoEventWireup="false" CodeFile="newappresult.aspx.vb" Inherits="newappresult" %>
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
                'Response.Write(sql)
                    flg = dbx.save(sql, session("con"),session("path"))
                    If flg = 1 Then
                    msg = "Data Saved"
                    End If
                End If
                'MsgBox(rd)
         
                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            If flg < 1 And flg <> 0 Then
                Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change " & sql & "</span>")
            End If
          
   
   End If
End If
    Dim sec As New k_security
    Dim fld, keyfld, hdrlist As String
    fld = sec.StrToHex("id,app_date,year_app, app_result,who_app")
    keyfld = sec.StrToHex("emp_id")
    
    hdrlist = sec.StrToHex("Appraisal date,Description,Result,Who Assess ")
    
   %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css"/>
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
    <script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
    	<script type="text/javascript" src="jqq/ui/jquery.ui.progressbar.js"></script>
        	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
            	<script type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script type="text/javascript">
    var prv;
    prv = "";
    var id;
    var focused = "";
    var requf = ["emp_id", "emptid", "app_date", "year_app", "app_result", "who_app", "x"];
    var fieldlist = ["emp_id", "emptid", "app_date", "year_app", "app_result", "who_app", "who_empid", "who_reg", "date_reg", "x"];
    function validation1() {
        if ($('#emp_id').val() == '') { showMessage('emp_id cannot be empty', 'emp_id'); $('#emp_id').focus(); return false; }
        if ($('#emptid').val() == '') { showMessage('emptid cannot be empty', 'emptid'); $('#emptid').focus(); return false; }
        if ($('#app_date').val() == '') { showMessage('app_date cannot be empty', 'app_date'); $('#app_date').focus(); return false; }
        if ($('#year_app').val() == '') { showMessage('year_app cannot be empty', 'year_app'); $('#year_app').focus(); return false; }
        if ($('#app_result').val() == '') { showMessage('app_result cannot be empty', 'app_result'); $('#app_result').focus(); return false; }
        else {
             if (!isNaN($('#app_result').val())) {
                 
                if($('#app_result').val()>100 || $('#app_result').val()<0) {
alert($('#app_result').val());
                showMessage('Number should be > 0 and <= 100 ', 'app_result'); 
                $('#app_result').val('');
                $('#app_result').focus();
                $('#app_result').css({ 'border': '1px solid red' });
                return false;
                }
             }
        }
        if ($('#who_app').val() == '') { showMessage('who_app cannot be empty', 'who_app'); $('#who_app').focus(); return false; }
        else if (focused == "") {
            var ans;
            ans = checkblur();
            if (ans != true) {
                $("#" + ans).focus();
            } else {
                var str = $("#frmemp_apprisal").formSerialize();
                $("#frmemp_apprisal").attr("action", "?tbl=emp_apprisal&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
                $("#frmemp_apprisal").submit();
                return true;
            }
        }
    }
    var hdrlist = '<%=hdrlist %>';
    var keyfld = '<%=keyfld %>';
    var fld = '<%=fld %>';

 </script>
 <%  Dim namelist As String = ""
     Dim fm As New formMaker
     Dim empid As String = ""
     Dim sqxl As String
     sqxl = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    "  ORDER BY emp_static_info.first_name, emprec.id DESC"
     namelist = fm.getjavalist22(sqxl, "emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")%>



     <script language="javascript" type="text/javascript">
    var namelist=[<% response.write(namelist) %>];
    </script>
     <link rel="stylesheet" href="css/bootstrap.css" />
<script type="text/javascript">
    //auto expand textarea
    function adjust_textarea(h) {
        h.style.height = "20px";
        h.style.height = (h.scrollHeight) + "px";
    }
    function getdata(val, fld1, fld2) {
        // alert(pid);

        var title = "";
        var xmlhttp1 = new XMLHttpRequest();
        var xmlhttp2 = new XMLHttpRequest();
        xmlhttp1.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                //alert(this.responseText);
                $("#" + fld1).val(this.responseText);
                title = this.responseText;
                xmlhttp2.onreadystatechange = function () {
                    if (this.readyState == 4 && this.status == 200) {
                        //alert(this.responseText);
                        $("#" + fld2).val(this.responseText);
                        title = title +" - " + this.responseText;
                        $("#apprtime").attr("title", title);
                    }

                };
                sql = "select period_end from apr_period where period='" + val + "'";
                xmlhttp2.open("GET", "getval.aspx?sql=" + sql+"&gettype=single", true);
                xmlhttp2.send();
                $("#apprtime").attr("title", title);
            }

        };
        var sql = "select period_start from apr_period where period='" + val + "'";
        xmlhttp1.open("GET", "getval.aspx?sql=" + sql + "&gettype=single", true);
        xmlhttp1.send();
        

       
        /* $('#frmpay').attr("target", "crt");
        $('#frmpay').attr("action", "addcart.php?id=" + pid);
        $('#frmpay').submit();
        */

    }

    function getdata2(fldx, fld1, fld2) {
                   // alert(pid);
                var val=$("#"+fldx).val();

           var empid;
                var xmlhttp1 = new XMLHttpRequest();

                xmlhttp1.onreadystatechange = function () {
                    if (this.readyState == 4 && this.status == 200) {
                        //alert(this.responseText);
                        $("#" + fld1).val(this.responseText);
                        empid = this.responseText;
                        mklist(empid);
                        sql = "select id from emprec where emp_id='" + $("#" + fld1).val() + "' order by id desc";

                        // alert(sql)
                        var xmlhttp2 = new XMLHttpRequest();
                        xmlhttp2.onreadystatechange = function () {
                            if (this.readyState == 4 && this.status == 200) {
                                //alert(this.responseText);
                                //alert(empid);

                                $("#" + fld2).val(this.responseText);
                                $("#" + fldx).attr("title", $("#" + fld1).val() + " " + this.responseText);

                            }

                        };

                        xmlhttp2.open("GET", "getval.aspx?sql=" + sql + "&gettype=single", true);
                        xmlhttp2.send();

                    }

                };
                var name = val.split(" ");
                var sql = "select emp_id from emp_static_info where first_name like '" + name[0] + "' and middle_name like '" + name[1] + "' and last_name like '" + name[2] + "'";
                xmlhttp1.open("GET", "getval.aspx?sql=" + sql + "&gettype=single", true);
                xmlhttp1.send();

               
           
            /* $('#frmpay').attr("target", "crt");
            $('#frmpay').attr("action", "addcart.php?id=" + pid);
            $('#frmpay').submit();
            */

            }
            function getdataat(fldx, fld1, fld2) {
                // alert(pid);
                var val = $("#" + fldx).val();

                var empid;
                var xmlhttp1 = new XMLHttpRequest();

                xmlhttp1.onreadystatechange = function () {
                    if (this.readyState == 4 && this.status == 200) {
                        //alert(this.responseText);
                        $("#" + fld1).val(this.responseText);
                        empid = this.responseText;
                        mklist(empid);
                        sql = "select  from emprec where emp_id='" + $("#" + fld1).val() + "' order by id desc";

                        // alert(sql)
                        var xmlhttp2 = new XMLHttpRequest();
                        xmlhttp2.onreadystatechange = function () {
                            if (this.readyState == 4 && this.status == 200) {
                                //alert(this.responseText);
                                //alert(empid);

                                $("#" + fld2).val(this.responseText);
                                $("#" + fldx).attr("title", $("#" + fld1).val() + " " + this.responseText);

                            }

                        };

                        xmlhttp2.open("GET", "getval.aspx?sql=" + sql + "&gettype=single", true);
                        xmlhttp2.send();

                    }

                };
                var name = val.split(" ");
                var sql = "select emp_id from emp_static_info where first_name like '" + name[0] + "' and middle_name like '" + name[1] + "' and last_name like '" + name[2] + "'";
                xmlhttp1.open("GET", "getval.aspx?sql=" + sql + "&gettype=single", true);
                xmlhttp1.send();



                /* $('#frmpay').attr("target", "crt");
                $('#frmpay').attr("action", "addcart.php?id=" + pid);
                $('#frmpay').submit();
                */

            }
            function mklist(val) {
                var xmlhttp1 = new XMLHttpRequest();
               
                xmlhttp1.onreadystatechange = function () {
                    if (this.readyState == 4 && this.status == 200) {
                       // alert(this.responseText);
                        document.getElementById("listx").innerHTML=(this.responseText);

                        // alert(sql)

                    }
                    else
                    { }
                };

               
                //var sql = "select emp_id from emp_static_info where first_name like '" + name[0] + "' and middle_name like '" + name[1] + "' and last_name like '" + name[2] + "'";
                xmlhttp1.open("GET", "getval.aspx?gettype=div_list&fld=" + fld + "&tbl=emp_apprisal&hrdlist=" + hdrlist + "&keyfLd=" + keyfld +"&empid=" + val, true);
              // alert("getval.aspx?gettype=div_list&fld=" + fld + "&tbl=emp_apprisal&hrdlist=" + hdrlist + "&keyfld=" + keyfld +"&empid=" + val);
                xmlhttp1.send();
            
            
            }
    
</script>

<style>
body {
  background: #bcb;
}

.form-style-7{
	min-width:400px;
	margin:auto auto;
	background:#fff;
	border-radius:2px;
	padding:20px;
	font-family: Georgia, "Times New Roman", Times, serif;
}
.form-style-7 h1{
	display: block;
	text-align: center;
	padding: 0;
	margin: 0px 0px 20px 0px;
	color: #5C5C5C;
	font-size:x-large;
}
.form-style-7 ul{
	list-style:none;
	padding:0;
	margin:0;	
}
.form-style-7 li{
	display: block;
	padding: 9px;
	border:1px solid #DDDDDD;
	margin-bottom: 20px;
	border-radius: 3px;
}
.form-style-7 li:last-child{
	border:none;
	margin-bottom: 0px;
	text-align: center;
}
.form-style-7 li > label{
	display: block;
	float: left;
	margin-top: -19px;
	background: #FFFFFF;
	height: 17px;
	padding: 2px 5px 2px 5px;
	color: #B9B9B9;
	font-size: 11px;
	overflow: hidden;
	font-family: Arial, Helvetica, sans-serif;
}
.form-style-7 input[type="text"],
.form-style-7 input[type="date"],
.form-style-7 input[type="datetime"],
.form-style-7 input[type="email"],
.form-style-7 input[type="number"],
.form-style-7 input[type="search"],
.form-style-7 input[type="time"],
.form-style-7 input[type="url"],
.form-style-7 input[type="password"],
.form-style-7 textarea,
.form-style-7 select 
{
	box-sizing: border-box;
	-webkit-box-sizing: border-box;
	-moz-box-sizing: border-box;
	width: 100%;
	display:inline-block;
	outline: none;
	border: 1px solid #ccc;
	height: 25px;
	line-height: 25px;
	font-size: 16px;
	padding: 0;
	font-family: Georgia, "Times New Roman", Times, serif;
}
.form-style-7 input[type="text"]:focus,
.form-style-7 input[type="date"]:focus,
.form-style-7 input[type="datetime"]:focus,
.form-style-7 input[type="email"]:focus,
.form-style-7 input[type="number"]:focus,
.form-style-7 input[type="search"]:focus,
.form-style-7 input[type="time"]:focus,
.form-style-7 input[type="url"]:focus,
.form-style-7 input[type="password"]:focus,
.form-style-7 textarea:focus,
.form-style-7 select:focus 
{
}
.form-style-7 li > span{
	background: #F3F3F3;
	display: block;
	padding: 3px;
	margin: 0 -9px -9px -9px;
	text-align: center;
	color: #C0C0C0;
	font-family: Arial, Helvetica, sans-serif;
	font-size: 11px;
}
.form-style-7 textarea{
	resize:auto;
	height:50px;
}
.form-style-7 input[type="submit"],
.form-style-7 input[type="button"],
.form-style-7 input[type="reset"]{
	background: #2471FF;
	border: none;
	padding: 10px 20px 10px 20px;
	border-bottom: 3px solid #5994FF;
	border-radius: 3px;
	color: #D2E2FF;
}
.form-style-7 button{
	background: #2471FF;
	border: none;
	padding: 10px 20px 10px 20px;
	border-bottom: 3px solid #5994FF;
	border-radius: 3px;
	color: #D2E2FF;
}
.form-style-7 input[type="submit"]:hover,
.form-style-7 input[type="button"]:hover,
.form-style-7 input[type="reset"]:hover{
	background: #6B9FFF;
	color:#fff;
}
.hiddenx
{
    
    }
</style>


</head>

<body style="height:auto;">

    <% If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           window.location = "logout.aspx";
</script>
       <%
       End If
       
       If Session("emp_id") = "" Then
       %>

<script type="text/javascript">
    //document.location.href="admin_home.php"
    //window.location = "empcontener.aspx";
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

'Response.Write("<br />" & Request.Form("do"))

'Response.Write(Request.QueryString("vew"))
'Response.Write(Session("emp_id"))
 %>


   <div class="content-lg container">

 <div class="masonry-grid-item col-xs-12 col-sm-6 col-md-6 sm-margin-b-30">
                        <div class="margin-b-60">
                         <span id="messagebox"></span>
    <form class='form-style-7' method='post' id='frmemp_apprisal' name='frmemp_apprisal'> 
    <h1>Appriasal Report Entry</h1>
<ul><li><label for="vname emp_id emptid">Employee Name</label>
 <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" onblur="javascript:if(this.value!==''){getdata2('vname','emp_id','emptid');}" />
<input type='hidden' id='emp_id' name='emp_id' value="<%response.write(session("emp_id")) %>" />
<input type='hidden' id='emptid' name='emptid' value="<%response.write(session("emptid")) %>" /></li>
<li><label for='apprtime'>Period</label><select id='apprtime' name='apprtime' class='optionlist'  onchange="javascript:getdata(this.value,'per_from','per_end');" >
<option value="">Appresal Time</option>
<%  'Dim dt As DataTableReader
    '  dt = dbx.dtmake("static" & Today.ToLongDateString, "select * from emp_static_info", session("con"))
   
    Response.Write(fm.getoption("apr_period", "period", "Period_start,period_end", Session("con")))%>
</select></li>
<li>
<input type='text' id='app_date' name='app_date' value='' placeholder="Appriasal Date" /></li>
<script language='javascript' type='text/javascript'>
    $(function () {
        $("#app_date").datepicker({ changeMonth: true, changeYear: true });
        $("#app_date").datepicker("option", "dateFormat", "mm/dd/yy"); 
    });

</script><li style='display:none'>
<input type='text' id='per_from' name='per_from' value='' placeholder='Appresal Period From'/>-<input type='text' id='per_end' name='per_end' value='' placeholder='Upto' />
</li><li>
<label for='year_app'>Year and Comment from Appraisee</label><sup style='color:red;'>*</sup>
<textarea id='year_app' name='year_app' cols="30" rows="8"></textarea></li><li>
<label for='comment_emp'>Employee comment</label><sup style='color:red;'>*</sup>
<textarea id='comment_emp' name='comment_emp' cols="30" rows="8"></textarea></li>
<li><label for='app_result'>Appriasal Result in % <sup style='color:red;'>*</sup></label>
<input type='text' id='app_result' name='app_result' value='' Placeholder="Enter only number 0-90" onkeyup="javascript:chkno('app_result');this.style.border='1px solid #ccc';"/></li>
<li>

<select id='who_app' name='who_app' >
<option value="">--Who appriased--</option>
<%  'Dim dt As DataTableReader
    '  dt = dbx.dtmake("static" & Today.ToLongDateString, "select * from emp_static_info", session("con"))
   
    Response.Write(fm.getoption("viewishead where ishead='y'", "emp_id", "first_name,middle_name", Session("con")))%>
</select>
</li><li><input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/><input type='button' name='btnSave' id='btnSave' value='Save' /><input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" />
</li></ul></form>
    
    <sup style="color:Red;">*</sup>Required Fields
    </div>
    </div>
    <div class="masonry-grid-item col-xs-12 col-sm-6 col-md-4 sm-margin-b-30">
                        <div class="margin-b-60">
 
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_apprisal where id=" & Request.QueryString("id"), session("con"))
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
        '  Dim sqlx As String = "select id,app_date,year_app, app_result,who_app from emp_apprisal where emp_id='" & Session("emp_id") & "' order by id desc"
        ' row = mk.edit_del_list("emp_apprisal", sqlx, "Appraisal date,Description,Result,Who Assess ", Session("con"), loc)
        '  Response.Write(row)
                    
        
    %>
 </div></div></div></div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
    hform = findh(document.getElementById("middle_bar"));
    $('.titlet').text("Appraisal");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
        function del(val, ans, hd) {

            if (ans == "yes") {
                // alert(val + ans);
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=emp_apprisal");
                $('#frmx').submit();
            }
            else {
                ha(hd);
            }
        }
   </script>
    <form id="frmx" action="" method="post">
    </form>
   <form id="frms" action="" method="post">
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

               $("#dialog-modal").dialog({
                   resizable: true,
                   modal: true
               });
           </script>
           <%
           
       End If
           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target", "_parent");

        //$('#frmx').attr("action","<% response.write(rd) %>");
        // $('#frmx').submit();
    
    </script>
   <%  End If%>
   
   <script type="text/javascript">
       $(document).ready(function () {
           $("#per_from").datepicker({
               defaultDate: "+1w",
               changeMonth: true,
               changeYear: true,
               numberOfMonths: 1,
               maxDate: "+2Y", minDate: "-2y",
               onSelect: function (selectedDate) {
                   $("#per_end").datepicker("option", "minDate", selectedDate);

               }
           });
           $("#per_end").datepicker("option", "dateFormat", "mm/dd/yy");
           $("#per_from").datepicker("option", "dateFormat", "mm/dd/yy");
           $("#per_end").datepicker({
               defaultDate: "+1w",
               changeMonth: true,
               changeYear: true,
               numberOfMonths: 2,
               maxDate: "2Y",
               onSelect: function (selectedDate) {
                   $("#per_from").datepicker("option", "MaxDate", selectedDate);
               }
           });
       });
    </script>
    <script type='text/javascript'>
        function goclicked(whr, id) {
            $('#frms').attr('action', '<%=loc %>?dox=' + whr + '&id=' + id.toString());
            $('#frms').submit();
        }
        $("#messagebox").text('<%=msg %>');
            </script>
</body>
</html>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>

