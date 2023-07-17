<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empsalary.aspx.vb" Inherits="empsalary" %>

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
    Dim msg2 As String = ""
    Dim dbx As New dbclass
    Dim fm As New formMaker
    Dim locid As Object
    Dim fl As New file_list
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
                sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
               ' Response.Write(sql)
                flg = dbx.save(sql, session("con"),session("path"))
                ' locid = fm.getinfo2("select id from emp_sal_info where date_end is null and emptid=" & Session("emptid") & " order by id desc", Session("con"))
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
                locid = fm.getinfo2("select id from emp_sal_info where date_end is null and emptid=" & Session("emptid") & " order by id desc", Session("con"))
                sql = "Begin Transaction" & Chr(13)
                sql &= dbx.makest(tbl, Request.QueryString, Session("con"), key) & Chr(13)
                'Response.Write(sql)
                'flg = dbx.save(sql, session("con"),session("path"))
                'Response.Write(flg)
                If sql <> "" Then
                    Dim dd As Date
                    dd = Request.QueryString("date_start")
                    
                    dd = dd.AddDays(-1)
                    ' Response.Write(dd.ToString)
                    'Response.Write(locid.ToString)
                    If locid.ToString <> "None" Then
                        sql &= "update emp_sal_info set date_end='" & dd.ToShortDateString & "', active='n' where id=" & locid.ToString & Chr(13)
                        ' Response.Write(sql)
                        
                        ' dbx.save(sql, session("con"),session("path"))
                    End If
                    flg = dbx.excutes(sql, Session("con"),session("path"))
                    Dim rpt() As String = {""}
                    If CInt(flg) > 0 Then
                        dbx.excutes("Commit", Session("con"),session("path"))
                        msg = "Data is Saved"
                        thisinc()
                        flg = 1
                        End If
                    
                    
                    End If
                End If
            End If
            'MsgBox(rd)
         
            ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            If flg <> 1 Then
                Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
            End If
          
   
   End If


   %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>

<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript">
	
	</script>
<script type="text/javascript">
var prv;
  prv="";
var id;
var focused="";
var requf=["emp_id","currency,basic_salary","x"];
var fieldlist=["emp_id","basic_salary","date_start","who_reg","date_reg","x"];
function validation1(){if ($('#emp_id').val() == '') {showMessage('emp_id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#basic_salary').val() == '') {showMessage('basic_salary cannot be empty','basic_salary');$('#basic_salary').focus();return false;}
if ($('#currency').val() == '') {showMessage('Currency cannot be empty','currency');$('#currency').focus();return false;}
else if(focused=="") { 
var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_sal_info").formSerialize();
   //alert(str);
   $("#frmemp_sal_info").attr("action","?tbl=emp_sal_info&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_sal_info").submit();
  return true;}
  }
} </script>


</head>

<body style="height:auto;">
<div></div>

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
'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))
'Response.Write("<br />" & Request.Form("do"))


 %>

 <div id="formouterbox_small">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messageboxx"></span>
<form method='post' id='frmemp_sal_info' name='frmemp_sal_info' action=""> 
<table><tr><td>
<input type='hidden' id='emp_id' name='emp_id' value="<%response.write(session("emp_id")) %>" />
<input type='hidden' id='emptid' name='emptid' value="<%response.write(session("emptid")) %>" />

Currency<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id='currency' name='currency' > 
<option value="">--Currency--
    </option>
<%  
    Response.Write(fm.getoption("tblcountry where currency<>''", "currency", "currency", session("con")))%>
    </select>
<br />
<label class='lblsmall'></label></td></tr>
<tr><td>
Basic Salary<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='basic_salary' name='basic_salary' value='' style="text-align:right;" /><br />
<label class='lblsmall'></label></td>
</tr>
<tr><td colspan="6" ><hr /><br /></td></tr>
<tr>
<td>Date Action<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='date_start' name='date_start' value=''  />
<br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'> $(function() {$( "#date_start").datepicker({changeMonth: true,changeYear: true	}); $( "#date_start" ).datepicker( "option","dateFormat","mm/dd/yy");});</script></tr>
<tr id='hiddate' style="display:none;">
<td>Date End<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='date_end' name='date_end' value=''  />
<br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>    $(function () { $("#date_end").datepicker({ changeMonth: true, changeYear: true }); $("#date_end").datepicker("option", "dateFormat", "mm/dd/yy"); });</script></tr>
<tr><td>Remark</td><td>:</td><td><textarea id='remark' name='remark' cols='20' rows='6'></textarea></td></tr>
<tr><td><input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/><input type='button' name='btnSave' id='btnSave' value='Save' /><input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" />
</td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
 
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_sal_info where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
          For k As Integer = 0 To dt.FieldCount - 4
                    'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                    If LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                        %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k).trim & "');" & Chr(13))%>
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
                Response.Write(Chr(13) & "$('#hiddate').css({display:'inline'});" & Chr(13))
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
        Dim sqlx As String = "select id,Basic_salary,currency,date_start,date_end from emp_sal_info where emp_id='" & Session("emp_id") & "' order by id desc"
        row = mk.edit_del_list("emp_sal_info", sqlx, "Basic Salary,Currency,Action date,End Date", Session("con"), loc)
            Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Salary Detail");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_sal_info");
        $('#frmx').submit();
            }
            else
            {
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
	
		$( "#dialog-modal" ).dialog({
		resizable: true,
			modal: true
		});
           </script>
           <%
           
       End If
           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","_parent");
       
        //$('#frmx').attr("action","<% response.write(rd) %>");
       // $('#frmx').submit();
    </script>
   <%  End If%>
   
   
</body>
</html>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
