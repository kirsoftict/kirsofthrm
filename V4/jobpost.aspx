<%@ Page Language="VB" AutoEventWireup="false" ValidateRequest="false" CodeFile="jobpost.aspx.vb" Inherits="jobpost" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="x-ua-compatible" content="IE=edge" />

<title></title>
<%  Dim cat, position As String
    cat = ""
    position = ""
   
    If File.Exists(Session("path") & "\text\jobcat.txt") = True Then
       
        Dim fxl() As String
        fxl = File.ReadAllLines(Session("path") & "/text/jobcat.txt")
        For i As Integer = 0 To UBound(fxl)
            cat &= Chr(34) & fxl(i) & Chr(34) & ","
            
        Next
        cat &= Chr(34) & "xx" & Chr(34)
    End If
    Dim fm As New formMaker
    ' position = getjavalist("tblposition", "Distinct job_position", Session("con"))
     
    '   position = "var position=[" & position & "];"
    cat = "var cat=[" & cat & "];"
    ' File.WriteAllText(Session("path") & "\scripts\position.kirsoft.js", position)
    File.WriteAllText(Session("path") & "\scripts\jobcat.kirsoft.js", cat)

    %>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="jqq/jquery-1.9.1.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
                  <script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
        <script type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
    <script type="text/javascript" src="jqq/ui/jquery.ui.dialog.js"></script>
    <script type="text/javascript" src="scripts/kirsoft.java.js"></script>
    <script type="text/javascript" src="scripts/form.js"></script>
    <script type="text/javascript" src="scripts/jobcat.kirsoft.js"></script>
       <script type="text/javascript" src="scripts/position.kirsoft.js"></script>
    <link rel="stylesheet" href="jqq/demos/demos.css">  
      <style type="text/css">
.form-style-2{
	max-width: 750px;
	padding: 10px 6px 5px 10px;
	font: 11px Arial, Helvetica, sans-serif;
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
	width: 110px;
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
<style>
    .col1 { float:left;
            padding-right:3px;}
    .col2{ float:left;}
    .col3{ clear:left;}
    .colfree{clear:left;}
</style><% 
    iniit()
    
   
    Dim fl As New file_list

   %>         <script type="text/javascript" src="scripts/tiny_mce/tiny_mce.js"></script>
<script type="text/javascript">
    tinyMCE.init({
        mode: "textareas",
        theme: "simple"
    });
</script>
      <script type="text/javascript">
    
          var prv;
          prv = "";
          var id;
          var focused = "";
          var requf = ["job_no", "job_title", "catagories", "placework", "needno", "position", "qualification", "yearexp", "salary",  "job_summery", "job_description", "start_date", "end_date", "who_reg", "date_reg", "publish", "x"];
          var fieldlist = ["job_no", "job_title", "catagories", "placework", "needno", "position", "qualification", "yearexp", "salary", "Benefites", "job_summery", "job_description", "offered_specialization", "start_date", "end_date", "who_reg", "date_reg", "publish", "x"];
          function validation1() {
            
              if ($('#job_no').val() == '') { showMessage('job_no cannot be empty', 'job_no'); $('#job_no').focus(); return false; }
              if ($('#job_title').val() == '') { showMessage('job_title cannot be empty', 'job_title'); $('#job_title').focus(); return false; }
              if ($('#catagories').val() == '') { showMessage('catagories cannot be empty', 'catagories'); $('#catagories').focus(); return false; }
              if ($('#placework').val() == '') { showMessage('placework cannot be empty', 'placework'); $('#placework').focus(); return false; }
              if ($('#needno').val() == '') { showMessage('needno cannot be empty', 'needno'); $('#needno').focus(); return false; }
              if ($('#position').val() == '') { showMessage('position cannot be empty', 'position'); $('#position').focus(); return false; }
              if ($('#qualification').val() == '') { showMessage('qualification cannot be empty', 'qualification'); $('#qualification').focus(); return false; }
              if ($('#yearexp').val() == '') { showMessage('yearexp cannot be empty', 'yearexp'); $('#yearexp').focus(); return false; }
              if ($('#salary').val() == '') { showMessage('salary cannot be empty', 'salary'); $('#salary').focus(); return false; }
              if ($('#Benefites').val() == '') { showMessage('Benefites cannot be empty', 'Benefites'); $('#Benefites').focus(); return false; }
              if (tinyMCE.get('job_summery').getContent() == '') { showMessage('job_summery cannot be empty', 'job_summery'); $('#job_summery').focus(); return false; }
              if (tinyMCE.get('job_description').getContent() == '') { showMessage('job_description cannot be empty', 'job_description'); $('#job_description').focus(); return false; }
              if ($('#start_date').val() == '') { showMessage('start_date cannot be empty', 'start_date'); $('#start_date').focus(); return false; }
              if ($('#end_date').val() == '') { showMessage('end_date cannot be empty', 'end_date'); $('#end_date').focus(); return false; }
             if ($('#publish').val() == '') { showMessage('publish cannot be empty', 'publish'); $('#publish').focus(); return false; }
              else if (focused == "") {
                  var ans;
                  ans = checkblur();
                  if (ans != true) {
                      $("#" + ans).focus();
                  } else {
                      var str = $("#frmtblhrjobs").formSerialize();
                      var hxstr = kirconvertToHex(str);
                      $("#hidpass").val(hxstr);
                     // alert(hxstr);
                      $("#frmtblhrjobs").attr("action", "?tbl=tblhrjobs&task=<% response.write(keyp) %>&lrd=empcontener&key=job_no&keyval=<%=Request.QueryString("job_no") %>&rd=empcontener.aspx");
                      $("#frmtblhrjobs").submit();
                      return true;
                  }
              }
          } </script>  
 
     
</head>

<body style="height:auto;">
<% Dim db As New dbclass
    Dim dt As DataTableReader
    Dim scr As String=""
        If keyp = "update" Then
           ' Response.Write("Editing")
        Dim ssql As String = "select * from tblhrjobs where id='" & Request.QueryString("id") & "'"
            Response.Write(ssql)
        dt = db.dtmake("new" & Today.ToLocalTime, ssql, Session("con"))
      If dt.HasRows = True Then
          dt.Read()
            scr = ("<script type='text/javascript'>")
            Dim k As Integer
            For k = 0 To dt.FieldCount - 3
                             
                 If LCase(dt.GetName(k)) = "job_summery" Or LCase(dt.GetName(k)) = "job_description" Or LCase(dt.GetName(k)) = "offered_specialization" Then
                    If IsDBNull(dt.Item(k)) = False Then
                        Session(dt.GetName(k)) = dt.Item(k).trim
                    End If
                   
                                 
                ElseIf LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                       
                    scr &= "$('#" & dt.GetName(k) & "').val('" & dt.Item(k).trim & "');"
                 
                ElseIf LCase(dt.GetDataTypeName(k)) = "datetime" And dt.IsDBNull(k) = False Then
                    Dim sdatex As Date = dt.Item(k)
                    Dim d As String = sdatex.ToShortDateString
                    Dim da As String = sdatex.Day
                    Dim mm As String = sdatex.Month
                    Dim yy As String = sdatex.Year
                    d = mm & "/" & da & "/" & yy
                    scr &= "$('#" & dt.GetName(k) & "').val('" & d & "');"
                Else
                        
                    scr &= "$('#" & dt.GetName(k) & "').val('" & dt.Item(k) & "');"
                  
                End If
                   
Next
If LCase(dt.GetName(k + 1)) = "publish" Then
    If dt.Item(k + 1) = 1 Then
        scr &= "$('#" & dt.GetName(k+1) & "').attr('checked','checked');"
    End If
End If
scr &= "$('#hiddate').css({display:'inline'});"
scr &= "$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>"
End If
dt.Close()
End If
                    %>
<div style="color:Green; font-size:14px;"><%  Response.Write(msgout)
         
      response.Write(webpost)   %>

</div>

<%  
    Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
<div>
 </div>
    <% If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           window.location = "logout.aspx";
</script>
       <%
       End If
       
       
'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))
'Response.Write("<br />" & Request.Form("do"))



 %>

 <div id="loadotherpage"></div>

 <div class='form-style-2'><div class='form-style-2-heading'>Provide your information</div><form method='post' id='frmtblhrjobs' name='frmtblhrjobs' action=""> 
<div class='col1'><label for='job_no'><span>job_no <span class='required'>*</span></span><input type="hidden" id="hidpass" name="hidpass" /><input type='text' class='input-field' name='job_no' id='job_no' value='job-<%=GenerateString(6) %>' /></label>
<label for='job_title'><span>job_title <span class='required'>*</span></span><input type='text' class='input-field' name='job_title' id='job_title' value='' onkeyup="javascript:startwith('job_title',position);" /></label>
<label for='catagories'><span>catagories <span class='required'>*</span></span><input type='text' class='input-field' name='catagories' id='catagories' value='' onkeyup="javascript:startwith('catagories',cat);" /></label>
<label for='placework'><span>placework <span class='required'>*</span></span><input type='text' class='input-field' name='placework' id='placework' value='' /></label>
<label for='needno'><span>Required no <span class='required'>*</span></span><input type='text' class='input-field' name='needno' id='needno' value='' /></label>
</div>
<div class='col2'>

<label for='qualification'><span>qualification <span class='required'>*</span></span><input type='text' class='input-field' name='qualification' id='qualification' value='' /></label>
<label for='yearexp'><span>yearexp <span class='required'>*</span></span><input type='text' class='input-field' name='yearexp' id='yearexp' value='' /></label>
<label for='salary'><span>salary <span class='required'>*</span></span><input type='text' class='input-field' name='salary' id='salary' value='Negotiable' /></label>
<label for='Benefites'><span>Benefites <span class='required'>*</span></span><input type='text' class='input-field' name='Benefites' id='Benefites' value='' /></label>
<label for='position'><span>Term <span class='required'>*</span></span><select class='input-field' name='position' id='position'>
<option value="Full Time">Full Time</option>
<option value="Contract">Contract</option>
<option value="Freelancer">Freelancer</option>
</select>

</label>
</div><div class='colfree'></div>
<div class='col3'><label for='job_summery'><span>job_summery <span class='required'>*</span></span>
<textarea class='input-field' name='job_summery' id='job_summery' cols='100' rows='10'>
<%  If keyp = "update" Then
      
        Response.Write(Session("job_summery"))

    End If%>


</textarea></label></div>
<div class='col3'><label for='job_description'><span>job_description <span class='required'>*</span></span>
<textarea class='input-field' name='job_description' id='job_description' cols='100' rows='10'>
<%  If keyp = "update" Then
      
        Response.Write(Session("job_description"))

    End If%>

</textarea></label></div>
<div class='col3'><label for='offered_specialization'><span>offered_specialization </span>
<textarea class='input-field' name='offered_specialization' id='offered_specialization' cols='100' rows='10'>
<%  If keyp = "update" Then
      
        Response.Write(Session("offered_specialization"))

    End If%>
</textarea></label></div>
<div class='colfree'></div>
<div class='col1'><label for='start_date'><span>start_date <span class='required'>*</span></span><input type='text' class='input-field' name='start_date' id='start_date' value='' /></label></div>
<script language='javascript' type='text/javascript'>    $(function () { $("#start_date").datepicker({ changeMonth: true, changeYear: true }); $("#start_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
<div class='col2'><label for='end_date'><span>end_date <span class='required'>*</span></span><input type='text' class='input-field' name='end_date' id='end_date' value='' /></label></div>
<script language='javascript' type='text/javascript'>    $(function () { $("#end_date").datepicker({ changeMonth: true, changeYear: true }); $("#end_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
<input type='hidden' class='input-field' name='who_reg' id='who_reg' value='<%=session("emp_iid") %>' />
<input type='hidden' class='input-field' name='date_reg' id='date_reg' value='<%=now %>' />
<div class='col1'><label for='publish'><span>publish <span class='required'>*</span></span><input type='checkbox' name='publish' id='publish' value='1' style='float:left;' /></label></div>
<div class='colfree'></div>
<div class='col3'><label><span>&nbsp;</span><input type='button' name='btnSave' id='btnSave' value='Save' /></label></div></form></div>



 




 
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <% 
    If keyp = "update" Then
      
        Response.Write(scr)

    End If
    Dim mk As New formMaker
    Dim row As String
    Dim loc As String
    loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
    Dim sqlx As String = "select id,	job_no,	job_title,	catagories,	placework,	needno,	position,	qualification,	yearexp,	salary,	Benefites,	job_summery,	job_description,	offered_specialization,	start_date,	end_date, publish from tblhrjobs order by id desc"
    row = mk.edit_del_list2("tblhrjobs", sqlx, "job_no,	job_title,	catagories,	placework,	needno,	position,	qualification,	yearexp,	salary,	Benefites,	job_summery,	job_description,	offered_specialization,	start_date,	end_date, publish ", Session("con"), loc, "", True, True, False, False)
    Response.Write(row & "<div class='hspace'>&nbsp;</div>")
        
    %>
 </div>

  
    <script type="text/javascript">
        function del(val, ans, hd) {

            if (ans == "yes") {
                // alert(val + ans);
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=tblhrjobs");
                $('#frmx').submit();
            }
            else {
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
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & Request.QueryString("id") & "','yes','del123');" & Chr(34) & "></span>"
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
               function job_description_onclick() {

               }

           </script>
           <%
           
           End If
           If msgout = "can't save this please resave it" Then
               dt = db.dtmake("new" & Today.ToLocalTime, "select * from tblhrjobs", Session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
                   For k As Integer = 0 To dt.FieldCount - 1
                       'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                      
                      
                       Response.Write("$('#" & dt.GetName(k) & "').val('" & Request.Form(dt.GetName(k)) & "');")
                   
                   
                Next
                Response.Write(Chr(13) & "$('#hiddate').css({display:'inline'});" & Chr(13))
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
                End If
                dt.Close()
            End If
  
        db = Nothing
        dt = Nothing
       %>
       
         
   <script src="scripts/kirsoft.required.js" type="text/javascript"></script>

   
</body>
</html>
  