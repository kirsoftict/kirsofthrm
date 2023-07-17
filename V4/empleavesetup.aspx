<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empleavesetup.aspx.vb" Inherits="empleavesetup" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  Dim fm As New formMaker
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
Dim sav As String
      ' Response.Write(sc.d_encryption("zewde@123"))
      Dim rd As String = ""
Dim xflg As String = ""
      Dim tbl As String = ""
      Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
    If Request.QueryString.HasKeys = True And Request.QueryString("task") <> "" Then
        If Request.QueryString("dox") = "" Then
            keyval = Request.QueryString("keyval")
        If Request.QueryString("task") = "update" Then
            Response.Write(Request.QueryString("user_rec_date"))
            Response.Write("<script type='text/javascript'>alert('updating...." & Request.QueryString("user_rec_date") & "');</script>")
            If Request.QueryString("user_rec_date") = "y" Then
                Response.Write("user date is the end date....")
                sql = "update emp_leave_info set no_days='" & Request.QueryString("no_days") & "'," & _
                    "year_end='"
                    
                Dim gethr As String = fm.getinfo2("select hire_date from emprec where id=" & Request.QueryString("emptid"), Session("con"))
                Dim ddd As Date
                If IsDate(gethr) = True Then
                    ddd = CDate(gethr)
                    gethr = MonthName(ddd.Month, ddd.Year)
                    gethr &= " " & ddd.Day
                Else
                    gethr = ""
                End If
               
                sql &= gethr & "',"
                sql &= "user_rec_date='y',"
                       
              
                sql &= "who_reg='" & Request.QueryString("who_reg") & "',"
                sql &= "date_reg='" & Request.QueryString("date_reg") & "'"
                sql &= " where emptid=" & Request.QueryString("emptid")
                Response.Write(sql)
                Try
                    If gethr <> "" Then
                       ' xflg = dbx.excutes(sql, Session("con"), Session("path"))
                        Response.Write("<br>" & xflg)
                        If IsNumeric(xflg) Then
                            msg = "Data Saved"
                        End If
                    End If
                Catch ex As Exception
                    Response.Write(ex.ToString)
                    fm.exception_hand(ex.ToString,"empleavesetup")
                End Try
                
            Else
                
                sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                sav = dbx.save(sql, Session("con"), Session("path"))
                '  Response.Write(sql)
                If IsNumeric(sav) Then
                    If sav > 0 Then
                         msg = "Data Updated"
                    End If
                   
                Else
                    Response.Write(sav & sql & "<br>")
                End If
            End If
        ElseIf Request.QueryString("task") = "delete" Then
            Response.Write("<script type='text/javascript'>//alert('deleting....');</script>")
            'sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
            sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
            sav = dbx.excutes(sql, Session("con"), Session("path"))
                   
            Response.Write(sql)
            If IsNumeric(sav) Then
                msg = "Data deleted"
            Else
                Response.Write(sav)
            End If
        ElseIf Request.QueryString("task") = "save" Then
            ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")
            If Request.QueryString("user_rec_date") = "y" Then
                Response.Write("user date is the end date....")
                sql = "insert into emp_leave_info(emptid,emp_id,no_days,year_end,user_rec_date,who_reg,date_reg,state) values"
                sql &= "('" & Request.QueryString("emptid") & "',"
                sql &= "'" & Request.QueryString("emp_id") & "',"
                sql &= "'" & Request.QueryString("no_days") & "',"
                Dim gethr As String = fm.getinfo2("select hire_date from emprec where id=" & Request.QueryString("emptid"), Session("con"))
                Dim ddd As Date
                If IsDate(gethr) = True Then
                    ddd = CDate(gethr)
                    gethr = MonthName(ddd.Month, ddd.Year)
                    gethr &= " " & ddd.Day
                Else
                    gethr = ""
                End If
               
                sql &= "'" & gethr & "',"
                sql &= "'y',"
                sql &= "'" & Request.QueryString("who_reg") & "',"
                sql &= "'" & Request.QueryString("date_reg") & "',"
                sql &= "'y')"
                Response.Write(sql)
                If gethr <> "" Then
                    flg = dbx.save(sql, Session("con"), Session("path"))
                    If flg = 1 Then
                        msg = "Data Saved"
                    End If
                End If
            Else
                sql = dbx.makest(tbl, Request.QueryString, Session("con"), key)
                ' Response.Write(sql)
                flg = dbx.save(sql, Session("con"), Session("path"))
                If flg = 1 Then
                    msg = "Data Saved"
                End If
                
            End If
                
        End If
            'MsgBox(rd)
         
            ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            If flg <> 1 Then
                ' Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
            End If
          
   
        End If
    End If
     
   %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">

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


<script type="text/javascript" src="scripts/form.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>

	

   <script language="javascript" type="text/javascript">
var prv;
  prv="";
var id;
var focused="";
var requf=["emp_id","no_days","year_end","who_reg","data_reg","month","dates","x"];
var fieldlist=["emp_id","leave_no_days","year_end","who_reg","data_reg","active","x"];
function validation1() {
   
if ($('#emp_id').val() == '') {showMessage('Employee Id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#no_days').val() == '') {showMessage('no_days cannot be empty','no_days');$('#no_days').focus();return false;}
if ($('#month').val() == '') {showMessage('Month cannot be empty','month');$('#month').focus();return false;}
if ($('#dates').val() == '') {showMessage('Date cannot be empty','dates');$('#dates').focus();return false;}
if (chkno('no_days')==false){$('#no_days').focus();  return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
    var str = $("#frmemp_leave_info").formSerialize();
    alert(str);
   $("#frmemp_leave_info").attr("action","?tbl=emp_leave_info&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_leave_info").submit();
  return true;}
  }
}
function checkst(id)
 {
     alert($("#" + id).fieldValue());
     if ($("#" + id).fieldValue() !== "")
         $("#" + id).val("y");
     //var str = $("#frmemp_leave_info").formSerialize();
     
     //alert(str);
}
</script>
	</head>
<body style="height:auto;">
<div></div>

 <div id="formouterbox_small">
    <div id="formheader" style="width: 59%">
    <span class="titlet">
This is Title</span>
<span class="close" title="Close" id="clikclose" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2" style="width: 84%">&nbsp;</div>
        <div class="head3" style="width: 12%; height: 24px">
            <span style="color: #ffffff"></span></div>
        </div>
    <div id="forminner" style="width: 57%">
 <span id="messagebox"><% if msg<>"" then
 response.write(msg)
 end if %></span>
 <form method='post' id='frmemp_leave_info' name='frmemp_leave_info' action=""> 
<table style="width: 97%"><tr><td style="width: 131px">Leave No Days<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='no_days' name='no_days' value='14' /><br /><label class='lblsmall'>Number Only</label>
<input type='hidden' id='emp_id' name='emp_id' value="<%response.write(session("emp_id")) %>" />
<input type='hidden' id='emptid' name='emptid' value="<%response.write(session("emptid")) %>" />
</td>
</tr><tr>
<td style="width: 131px">Fisical Year End<sup style='color:red;'>*</sup></td><td>:</td><td>
<script language="javascript" type="text/javascript">
function enabledate(val)
{var dayy=0;
 $('#spanMsgmonth').css('display', 'none');
    switch(val)
    {
        case "January": case "March": case "May": case "July": case "August": case "October": case "December":
            dayy=31;
        break;
        case "February":
            if(<% response.write((today.year Mod 4)) %> == 0)
            {
                dayy=29;
                
            }
            else
                dayy=28;
        break;
        case  "April": case "June": case "September": case "November":
            dayy=30;
        break;
    }
    $("#dates").empty();
    $("#dates").append('<option value="" style="color:Gray;">Date</option>');
    for(j=1;j<=dayy;j++){
   // document.getElementById("dates").option[j]=new Option(j.toString(),j.toString());
    //document.getElementById("dates").option[j-1].value=j;
    $("#dates").append('<option value="'+j+'">'+j+'</option>');
    }
   if($("#month").val()!="")
    document.getElementById("dates").disabled=false;
   else
   document.getElementById("dates").disabled="disabled";

}
function ondatechange()
{
     $('#spanMsgdates').css('display', 'none');
    $("#year_end").val( $("#month").val() + " " + $("#dates").val());
}
</script>

<select id="month" name="month" onchange="javascript: enabledate(this.value);">
        <option value="" style="color:Gray;">Months</option>
        <%
            for i as integer=1 to 12
                Response.Write("<option value=" & Chr(34) & MonthName(i).ToString & Chr(34) & ">")
                Response.Write(MonthName(i).ToString)
                Response.Write("</option>")
            next
         %>
</select>
<select id="dates" name="dates" disabled="disabled" onchange="Javascript: ondatechange();">
        <option value="" style="color:Gray;">Date</option>
 </select></td></tr>
<tr>
<td style="" colspan="4">User The Hireing date :
<input type="checkbox" name="user_rec_date" id="user_rec_date" value='y'  onclick="javascript:checkst('user_rec_date');"/> </td></tr>
<tr><td style="width: 131px"><br /><label class='lblsmall'></label>
<input type='hidden' id='year_end' name='year_end' value='' />
<input type='hidden' id='state' name='state' value='y' />
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString 
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" /></td>
</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>

<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Leave Setup");
    //showobjar("formx","titlet",22,2);
</script>

 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_leave_info where id=" & Request.QueryString("id"), session("con"))
            If dt.HasRows = True Then
                dt.Read()
                Response.Write("<script type='text/javascript'>")
                For k As Integer = 0 To dt.FieldCount - 3
                    %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k) & "');")%>
                    <%
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
            Dim sqlx As String = "select id,no_days,year_end,user_rec_date from emp_leave_info where emp_id='" & Session("emp_id") & "' order by id desc"
            row = mk.edit_del_list("emp_leave_info", sqlx, "Leave No Days,Fiscal Year End,Use Hiring date As Fiscal Year", session("con"), loc)
            Response.Write(row)
            ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
                con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
                "<img src='images/gif/btn_yes.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','yes','del123');" & Chr(34) & "> " & _
                "<img src='images/gif/cancel.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','no','del123');" & Chr(34) & "></span>"
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
		resizable: true
			
		});
           </script>
           <%
           
       End If  
        %>
 </div>

    <script type="text/javascript">
      function del(val,ans,hd)
       {
          if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_leave_info");
        $('#frmx').submit();
            }
            else
            {
                 $('#frmx').attr("target","_self");
        $('#frmx').attr("action","?task=&id="+val+"&tbl=emp_leave_info");
        $('#frmx').submit();
            }
       }
       $("#month").val("June");
       enabledate("June");
       $("#dates").val("30");
       $("#dates").attr("disabled","false");
       ondatechange();
    </script>
    <form id="frmx" action="" method="post">
    </form>
   <script type="text/javascript" src="scripts/kirsoft.required.js"></script>
</body>
</html>