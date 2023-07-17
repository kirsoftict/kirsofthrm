<%@ Page Language="VB" AutoEventWireup="false" CodeFile="emplogincreate.aspx.vb" Inherits="emplogincreate" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>

<%  If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
       End If
       
    %>
    <%  Dim keyp As String = ""
    If Request.QueryString("dox") = "edit" Then
        keyp = "update"
    ElseIf Request.QueryString("dox") = "delete" Then
        keyp = "delete"
    ElseIf Request.QueryString("dox") = "upload" Then
    Else
        keyp = "save"
    End If
    Dim idx As String = ""
        idx = Request.QueryString("username")
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
                If LCase(Request.QueryString("task")) = "update" Then
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    flg = dbx.save(sql, session("con"),session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                        'Response.Write("<script type='text/javascript'>alert('updating...." & keyval & key & "');</script>")

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
                ElseIf LCase(Request.QueryString("task")) = "save" Then
                    ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                    sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
                    ' Response.Write(sql)
                    flg = dbx.save(sql, session("con"),session("path"))
                    If flg = 1 Then
                        msg = "Data Saved"
                    End If
                End If
                'MsgBox(rd)
         
            ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
                If flg <> 1 Then
                    Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
                End If
          
   
   End If
    End If
   

   %>
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
<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<link rel="stylesheet" href="../demos.css" />
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
 <%
  Dim dbc As New dbclass
    Response.Write("<script type='text/javascript'>var listname=")
     Response.Write(dbc.listnames(session("con"), "username", "login"))
    Response.Write("</script>")
     Dim listids, listuname As String
     listids = dbc.idlist_jqry("login", "emp_id", session("con"))
     listuname = dbc.idlist_jqry("login", "username", session("con"))
    dbc = Nothing
    %>
   <script language="javascript" type="text/javascript">
   function close(id)
   {
    $( "#dialog" ).dialog('close')
   }
   function showdialog()
   {
  
   $( "#dialog" ).dialog({
		resizable: true,
			modal: true
		});
		 $( "#dialog" ).css({'visibility':'visible'});
   }
var av = [<% response.write(listuname)%>];
var prv;
  prv="";
var id;
var focused="";
var requf=["emp_id","username","password","auth","active","confirm","x"];
var fieldlist=["emp_id","username","password","auth","oldpass","datechanged","active","squestion1","answer1","squestion2","answer2","x"];
function validation1(){if ($('#emp_id').val() == '') {showMessage('emp_id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#username').val() == '') {showMessage('username cannot be empty','username');$('#username').focus();return false;}
if ($('#password').val() == '') {showMessage('password cannot be empty','password');$('#password').focus();return false;}
if ($('#confirm').val() == '') {showMessage('Confirmation cannot be empty','confirm');$('#confirmT').focus();return false;}

if ($('#auth').val() == '') {showMessage('auth cannot be empty','auth');$('#auth').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmlogin").formSerialize();
  // alert(str);
  
   $("#frmlogin").attr("action","?tbl=login&keyval=" + $('#username').val() + "&task=" + document.getElementById("btnSave").value + "&lrd=empcontener&key=username&rd=empcontener.aspx&" + str);
    $("#frmlogin").submit();
  return true;
  }
  }

} </script>

</head>

<body style="height:auto;">
<div></div>

 <div id="formouterbox_small">
    <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" title="Close" id="clikclose" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
 <span id="messagebox"> <%  If msg <> "" Then
                                Response.Write(msg)
                            End If%></span>

<form method='post' id='frmlogin' name='frmlogin' action=""> 
<table><tr><td><input type='hidden' id='emp_id' name='emp_id' value='<% response.write(session("emp_id")) %>' />
Username<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='username' name='username' value='' onblur="javascript:onlost('username',av);"/><br />
<label class='lblsmall'></label></td>
</tr><tr><td>password<sup style='color:red;'>*</sup></td><td>:</td><td colspan="2">
<input type='password' id='password' name='password' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Confirm<sup style='color:red;'>*</sup></td><td>:</td><td colspan="2">
<input type='password' id='confirm' name='confirm' value='' />
<img src="images/loading.gif" alt="checking..." id="rot" style="visibility:hidden;"/><br /><label class='lblsmall'></label></td>
</tr><tr>
<td>Authentications<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='auth' name='auth' value='' /><br />
<label class='lblsmall'>Put numbers with separted by ';' eg. 1;2;3 </label><a id="tips" href="javascript:showdialog();hr('tips');">Tip</a></td>
</tr><tr><td>active<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='active' name='active' value='y' /><br /><label class='lblsmall'></label></td>
</tr><tr><td colspan="5"><fieldset class="fset" style="border:solid 1px blue; background:white;">
<legend><b>Secret Question</b>(optional)</legend>
<table><tr>
<td>Question1<sup style='color:red;'></sup></td><td>:</td><td>
<input type='text' id='squestion1' name='squestion1' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Aanswer1<sup style='color:red;'></sup></td><td>:</td><td>
<input type='text' id='answer1' name='answer1' value='' /><br /><label class='lblsmall'></label></td></tr><tr>
<td>Question2<sup style='color:red;'></sup></td><td>:</td><td><input type='text' id='squestion2' name='squestion2' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>answer2<sup style='color:red;'></sup></td><td>:</td><td>
<input type='text' id='answer2' name='answer2' value='' /><br /><label class='lblsmall'></label></td></tr></table></fieldset></td>
</tr><tr><td colspan='4'>

<input type='hidden' id='date_reg' name='date_reg' value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" />
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>"/>
<input type="button" name="btnSave" id="btnSave" value="Save" />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>

<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Create Login");
    //showobjar("formx","titlet",22,2);
</script>
<%
Dim db As New dbclass
        
    Dim dt As DataTableReader
        
    dt = db.dtmake("new" & Today.ToLocalTime, "select * from login where emp_id='" & Session("emp_id") & "'", session("con"))
    If dt.HasRows = True Then
        dt.Read()
        Response.Write("<script type='text/javascript'>")
        For k As Integer = 0 To dt.FieldCount - 4
            If dt.GetDataTypeName(k).ToLower = "string" And dt.IsDBNull(k) = False Then
                        %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k).trim & "');")%>
                    <% 
                    Else
                         %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k) & "');")%>
                    <%
                    End If
                   
                Next
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
            End If
                dt.Close()
        
            db = Nothing
            dt = Nothing
            %>
            <div id="dialog" title="Tips" style="visibility:hidden;">
                <ul><strong>Authentications</strong>
                <li>1- Administrator</li>
                <li>2- Human Resource</li>
                <li>3- Accounting</li>
                <li>4- Manangment</li>
                <li>5- Users</li>
                </ul>
                <a href="javascript:close('dialog');">Close</a>
                
            </div>
</body>
</html>
<script type="text/javascript">
$(document).ready(function() {
loc=0;
 $("#confirm").focus(function(){
    if( $("#password").val()=="")
    {
        showMessage("*Required","password");
        $("#password").focus();
    }
    else if( $("#password").val().length<=5)
    { 
    $("#password").focus();
    showMessage("The value should 6 or more letters","password");
       
    }
 });
    $("#confirm").keyup( function()
    {
    
        if( $("#confirm").val().length>1)
        {
        if(loc==0){
        $("#rot").attr("src","images/loading.gif");
        $("#rot").css({'visibility':'visible'});
        loc=1;
        }
        }
        else
        {loc=0;
         $("#rot").css({'visibility':'hidden'});
         }
     
    
   } );
   $("#confirm").blur(function(){ 
    if($("#password").val()==$("#confirm").val())
    {
        $("#rot").attr("src","images/gif/tick.gif");
       $("#rot").css({'width':'20px','height':'20px'});
        
        }
        else
        {
        
      
         //$("#rot").attr("src","images/gif/x.gif"); $("#rot").css({'width':'20px','height':'20px'});
         if( $("#confirm").val()!=""){
         showMessage("Password Doesnt much" ,"confirm");
         $("#confirm").focus();
        
         } loc=0;
         $("#rot").css({'visibility':'hidden'});
        }
    
    });
    
});
</script>
