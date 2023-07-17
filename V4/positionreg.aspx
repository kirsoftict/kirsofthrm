<%@ Page Language="VB" AutoEventWireup="false" CodeFile="positionreg.aspx.vb" Inherits="positionreg" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="cache-control" content="no-cache" />
       <meta http-equiv="pragma" content="no-cache" />
<title></title>
<%   If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           window.location = "logout.aspx";
</script>
       <% End If
           Dim t1 As Date = Now.ToLongTimeString
           Response.Write(Request.QueryString("task"))
           If Request.QueryString("task") = "delete" Then
               Dim dbb As New dbclass
               Dim flg As Object
               flg = dbb.excutes("delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id"), Session("con"), Session("path"))
               If IsNumeric(flg) Then
                   If flg > 0 Then
                       Response.Write("Deleted")
                   End If
               End If
               'Response.Write(flg.ToString & "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") )
               dbb=Nothing
           End If
   

   %>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<link rel="stylesheet" type="text/css" media="screen" href="css/pagger.css" /> 
<script src="scripts/script.js" type="text/javascript"></script>
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
    <script src="jqq/ui/jquery.ui.dialog.js"></script>
    <script src="jqq/ui/jquery.ui.button.js"></script>
 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="scripts/pager.js"></script>

<%  Dim namelist As String = ""
    Dim fm As New formMaker
    namelist = fm.getjavalist2("tblposition", "job_position", Session("con"), "")
   
 %>
<script type="text/javascript">
namelist=[<% response.write(namelist) %>];
var prv;
  prv="";
var id;
var focused="";
var requf=["job_position","job_description","x"];
var fieldlist=["job_position","job_grade","job_description","who_reg","date_reg","stat","abr","x"];
function validation1(){if ($('#job_position').val() == '') {showMessage('position cannot be empty','job_position');$('#job_position').focus();return false;}
if ($('#job_description').val() == '') {showMessage('description cannot be empty','job_description');$('#job_description').focus();return false;}
if ($('#stat').val() == '') {showMessage('stat cannot be empty','stat');$('#stat').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmtblposition").formSerialize();
//alert("save.aspx?tbl=tblposition&task=" + $("#btnSave").val() +"&lrd=empcontener&key=id&keyval=id&rd=empcontener.aspx&" + str);
 var mmsg;
 
   $("#messagebox").load("save.aspx?tbl=tblposition&task=" + $("#btnSave").val() +"&lrd=empcontener&key=id&keyval=id&rd=empcontener.aspx&" + str);

mmsg=$("#messagebox").text();
alert(mmsg);
   // $("#frmtblposition").submit();
 // return true;
 }
  }
} </script>
</head>
<body>


    <div id='bodyx' style='padding-left:15px; height:max-height:760px;'>
 
 <div id="formouterbox_small" style="width:600px; float:left;" >
    <div id="formheader" onclick="javascript:showMenu('forminner')">
    
    <span class="titlet">
This is Title</span>

        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div>
         </div>
    <div id="forminner">
 <span id="messagebox"></span>
 <form method='post' id='frmtblposition' name='frmtblposition' action=""> 
 <input type="hidden" id="id" name="id" />
<table><tr><td>Position<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='job_position' name='job_position' />Abr.<input type="text" name="abr" id="abr" /></td></tr>
<tr>
<td>Description<sup style='color:red;'></sup></td><td>:</td><td colspan="4">
<textarea id='job_description' name='job_description' cols="30" rows="12" ></textarea></td></tr>
<tr>
<td><input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'   value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString 
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
stat<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='stat' name='stat' value='y' /></td>
</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' /><input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    
    <sup style="color:Red;">*</sup>Required Fields
 </div></div>

 
    
 <%
     Dim loc As String
     loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
     Dim sqlx As String = "select id,job_position,abr,job_description from tblposition as tb order by job_position"
     'row = mk.edit_del_list("tblposition", sqlx, "Position,Abrivation,Description", session("con"), loc)
     %>
     <div style=' float:left; width:30px;  '></div>
     <div style='float:left;width:650px;'>
     <div style="width:200px; ">
     <form id='frmsbyname' method='post' action=''>
    <table cellspacing='0px' cellpadding='0px' width="100px"><tr><td valign="top">
   Search:</td><td>  <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" /><br/>
    <label class='lblsmall'>Write Job Position</label></td>
    <td valign='top'>
    </td></tr></table></form></div>
  
  <div id="listx" style="  padding:0px 7px 0px 0px; width:650px;">
  <div id="maxnum" style=""><input type="text" id="maxrow" value="10" onkeyup="javascript:if(event.keyCode==13) changepg($('#pgnum').val());" />
  <input type="hidden" id="pgnum" value="1" />
  <input type="hidden" id="sql" value="select id,job_position,abr,job_description from (select *,row_number() over (order by job_position) as row from tblposition) tbl1" />
  <input type="hidden" id="heeader" value="Position,Abrivation,Description" />
   <input type="hidden" id="loc" value="<%=loc %>" />
   <input type="hidden" id="tbl" value="tblposition" />
  </div>
  <div id="outputfram" style='background-color:Yellow; width:700px;' >
  <div id="listtablex" style=' background-color:Lime'><img src="images/gif/loading.gif" /></div>
  <div id="pagingx" style=' background-color:Gray'><img src="images/gif/loading.gif" /></div>
  </div>
  
 </div>  </div>
<script type="text/javascript" language="javascript">
//hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Add & Edit Job Position");
    //showobjar("formx","titlet",22,2);
</script>
    <script type="text/javascript">
        function delx(val, ans, hd) {
            pgnum = $("#pgnum").val();
            tsize = $("#maxrow").val();
            // alert("inini");
           // alert('?dox=' + whr + '&id=' + id + '&pgnum=' + pgnum + '&tsize=' + tsize);
          
            if (ans == "yes") {
                // alert(val + ans);
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=tblposition&pgnum=" + pgnum + "&tsize=" + tsize);
                $('#frmx').submit();
            }
        }
      
   </script>
    <form id="frmx" action="" method="post">
    </form>
   <%  Dim t2 As Date
       t2 = Now.ToLongTimeString
       'If msg = "" Then
       'msg = (FormatNumber(t2.Subtract(t1).Minutes, 2).ToString & ":" & FormatNumber(t2.Subtract(t1).Seconds, 2).ToString & ":" & FormatNumber(t2.Subtract(t1).Milliseconds, 2).ToString)
       'Else
       'msg &= (FormatNumber(t2.Subtract(t1).Minutes, 2).ToString & ":" & FormatNumber(t2.Subtract(t1).Seconds, 2).ToString & ":" & FormatNumber(t2.Subtract(t1).Milliseconds, 2).ToString)
     
       '  End If
       
       ' If msg <> "" Then
            %>
            <script type="text/javascript">
             // $("#messagebox").text(msg);

            </script>
            <%
                '     End If
            
           
            %>
            <script type="text/javascript">
                //changetsize();
               <% if request.querystring("pgnum")<>"" then %>
                 $("#pgnum").val('<%=request.Querystring("pgnum") %>');
  $("#maxrow").val('<%=request.Querystring("tsize") %>');
               <%end if %>
                changepg($("#pgnum").val());
            </script>
            </div>
            <% 
                If Request.QueryString("dox") = "delete" Then
                Response.Write("<script>                                    var kx = confirm('are you sure want delete!'); if (kx == true) { delx('" & Request.QueryString("id") & "', 'yes', ''); }</script>")
                Response.Write("delete" & Request.QueryString("id") & "table")
            End If%>
             <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
</body>
</html>



