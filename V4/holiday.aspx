<%@ Page Language="VB" AutoEventWireup="false" CodeFile="holiday.aspx.vb" Inherits="holiday" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%   If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <% End If
      
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
    If Request.QueryString.HasKeys = True And Request.QueryString("task") <> "" Then
        If Request.QueryString("dox") = "" Then
            keyval = Request.QueryString("keyval")
            If Request.QueryString("task") = "update" Then
                'e("<script type='text/javascript'>alert('updating....');</script>")
                sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                   flg = dbx.save(sql, session("con"),session("path"))
                   ' Response.Write(sql)
                If flg = 1 Then
                       msg = "Data Updated"
                   Else
                       msg = "Sorry Data is not Updated"
                End If
            ElseIf Request.QueryString("task") = "delete" Then
                   Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                   'sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                   sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                   flg = dbx.save(sql, session("con"),session("path"))
                   
                ' Response.Write(sql)
                If flg = 1 Then
                       msg = "Data deleted"
                   Else
                       msg = "Sorry Data is not deleted"
                   End If
            ElseIf Request.QueryString("task") = "save" Then
                ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
                ' Response.Write(sql)
                flg = dbx.save(sql, session("con"),session("path"))
                If flg = 1 Then
                       msg = "Data Saved"
                   Else
                       msg = "Sorry Data is not Save"
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
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script-->
	<script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>

  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>

	</head>
<script type="text/javascript">

  var fieldlist=["holiday_name","date_lie","description","x"];
var prv;
  prv="";
      var id;
  var focused="";
  var requf=["holiday_name","date_lie","description","x"];
function validation1(){
if ($('#holiday_name').val() == '') {showMessage('holiday_name cannot be empty','holiday_name');$('#holiday_name').focus();return false;}
if ($('#date_lie').val() == '') {showMessage('date_lie cannot be empty','date_lie');$('#date_lie').focus();return false;}
if ($('#description').val() == '') {showMessage('description cannot be empty','description');$('#description').focus();return false;}
else if(focused=="")
  { 
    var ans
    ans=checkblur();
   // alert(ans);
    if(ans!=true){
     $("#" + ans).focus();
     //alert(ans);
    }
    else{
    str=$("#frmholidays").formSerialize();
  //alert(str);
  //$("#messagebox").load("open.aspx?tbl=holidays&task=save&key=stat&rd=&" + str);
     $("#frmholidays").attr("action","?tbl=holidays&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=admincontener.aspx&" + str);
    
   // $("#messagebox").load("open.aspx?tbl=emp_address&rd=&" + str);
    $('#frmholidays').submit();

    return true;}
    }
}</script>
<body style="height:auto;">

 <div id="formouterbox_small" style="width:750px;">
    <div id="formheader">
    <span class="titlet" style="color:Yellow;">
This is Title</span>
<span class="close" title="Close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
 <span id="messagebox"></span>
<form method='post' id='frmholidays' name='frmholidays' action=""> 
<table><tr><td>Holiday Name<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='holiday_name' name='holiday_name' /></td>
<td>date_lie<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='date_lie' name='date_lie' />
</td>
<script language='javascript' type="text/javascript"> $(function() {
$( "#date_lie").datepicker({changeMonth: true,changeYear: true	}); 
$( "#date_lie" ).datepicker( "option","dateFormat","mm/dd/yy");});</script></tr><tr><td>description<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='description' name='description' /></td>
</tr><tr><td colspan="4">
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString 
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" />
<script language='javascript' type="text/javascript"> 
$(function() {
$( "#date_reg").datepicker({changeMonth: true,changeYear: true	}); 
$( "#date_reg" ).datepicker( "option","dateFormat","mm/dd/yy");});
</script><input type='hidden' id='who_reg' name='who_reg' value="<% response.write(session("username")) %>" />
<input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
     </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        Dim val_up As String = ""
        If keyp = "update" Then
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from holidays where id=" & Request.QueryString("id"), session("con"))
            If dt.HasRows = True Then
                dt.Read()
                Response.Write("<script type='text/javascript'>")
                For k As Integer = 0 To dt.FieldCount - 1
                   ' Response.Write("//" & dt.GetDataTypeName(k))
                    If dt.GetDataTypeName(k).ToLower = "datetime" Then
                        Dim dta() As String
                        Dim strd As String
                        strd = dt.Item(k)
                        dta = strd.Split("/")
                        val_up = dta(1) & "/" & dta(0) & "/" & dta(2)
                    Else
                        val_up = dt.Item(k)
                    End If
                    %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & val_up & "');")%>
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
            Dim sqlx As String = "select id,Holiday_name,date_lie,description from holidays order by id desc"
            row = mk.edit_del_list("holidays", sqlx, "Holiday Name,Date On,Description", session("con"), loc)
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
<script type="text/javascript" language="javascript">
//hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Add holidays");
    //showobjar("formx","titlet",22,2);
</script>
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=holidays");
        $('#frmx').submit();
            }
            else
            {
                 $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=&id="+val+"&tbl=holidays");
        $('#frmx').submit();

            }
       }
      
   </script>
    <form id="frmx" action="" method="post">
    </form>
   <% if msg<>"" then
            %>
            <script type="text/javascript">
              $("#messagebox").load("msgout.aspx?anst=<% response.write(flg) %>&msg=<% response.write(msg) %>");

            </script>
            <%
   end if %>
</body>
</html>


   
