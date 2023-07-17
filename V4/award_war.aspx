<%@ Page Language="VB" AutoEventWireup="false" CodeFile="award_war.aspx.vb" Inherits="award_war" %>

<%@ Import Namespace="System.IO" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<% 
    firstlines()
   
    secondlines()
    Dim flg As String
    Dim msg As String
     Dim loc As String=   Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
    %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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
    <script src="jqq/ui/jquery.ui.tabs.js"></script>
		<script type="text/javascript" src="scripts/form.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
   <script src="jqq/demo.js"></script>
    <title></title>
     <script type="text/javascript">
         var prv;
         prv = "";
         var id;
         var focused = "";
         var requf = ["emptid", "emp_id", "wtgiving", "giving_date", "letter_no", "x"];
         var fieldlist = ["emptid", "emp_id", "wtgiving", "giving_date", "letter_no", "description", "attachfile", "who_reg", "date_reg", "x"];
         function validation1() {
             if ($('#emptid').val() == '') { showMessage('emptid cannot be empty', 'emptid'); $('#emptid').focus(); return false; }
             if ($('#emp_id').val() == '') { showMessage('emp_id cannot be empty', 'emp_id'); $('#emp_id').focus(); return false; }
             if ($('#wtgiving').val() == '') { showMessage('wtgiving cannot be empty', 'wtgiving'); $('#wtgiving').focus(); return false; }
             if ($('#giving_date').val() == '') { showMessage('giving_date cannot be empty', 'giving_date'); $('#giving_date').focus(); return false; }
             if ($('#letter_no').val() == '') { showMessage('letter_no cannot be empty', 'letter_no'); $('#letter_no').focus(); return false; }
             else if (focused == "") {
                 var ans;
                 ans = checkblur();
                 if (ans != true) {
                     $("#" + ans).focus();
                 } else {
                     var str = $("#frmemp_aw_wr").formSerialize();
                     $("#frmemp_aw_wr").attr("action", "?tbl=emp_aw_wr&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
                     $("#frmemp_aw_wr").submit();
                     return true;
                 }
             }
         } 
         
         function goupload(whr,fn)
{
    var ftype=".doc,.docx,.pdf,.jpg";
    var size=1000000;
    var loct="<%response.write(session("emp_id")) %>/leave/" +fn+"/";
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;
   // alert(whr + fn.toString());
     $('#frmx').attr("target","uploadf");
       
       $('#frmx').attr("action","allupload.aspx?upload=phase1&ftype="+ftype+"&size="+size+"&loct="+loct);
       $('#frmx').submit();
       $('#upload').css({top:'0px',left:'0px'});
       showobj("upload");
 // $( '#upload').dialog('destroy');
//$( '#upload' ).dialog({resizable: true,modal: true});
  //   $('#upload').css({'visibility':'visible','display':'inline'});
}
function del(val,ans)
       { 
        if( ans=="1st")
          { // alert(val);
                $('#frmx').attr("target","_self");
                $('#frmx').attr("action","<% response.write(loc) %>?task=deletefile&id="+val+"&tbl=file");
                $('#frmx').submit();
            }
        else
        {
       // str=$("#frmemp_static_info").formSerialize();
        //alert("deleted");
         //  $("#messagebox").load("deletefile.aspx?fname=" + val + "&tasks=delete");

              $('#frmx').attr("target","_self");
             $('#frmx').attr("action","<% response.write(loc) %>?task=deletecon&id="+val+"&delete=true");
             $('#frmx').submit();
        }
            
       }
         </script>
</head>
<body>
<div id='dialogx'><div style='font-weight:bold;'>Award or Warrning Resignation</div>
     <form method='post' id='frmemp_aw_wr' name='frmemp_aw_wr' enctype="multipart/form-data"> 
<table id='tb1'>
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
<input type='hidden' id='emp_id' name='emp_id' value='<% response.write(session("emp_id")) %>' />
<tr><td>Letter type: <sup style='color:red;'>*</sup></td><td>:</td><td>
<select id='wtgiving' name='wtgiving' >
<option value="Warrning">Warrning</option>
<option value="Award">Award</option>
</select><br /><label class='lblsmall'></label></td>
<td>Date of Issue<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='giving_date' name='giving_date' value='' />
<br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>    $(function () { $("#giving_date").datepicker({ changeMonth: true, changeYear: true }); $("#giving_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script></tr>
<tr><td>Letter No<sup style='color:red;'></sup></td><td>:</td><td><input type='text' id='letter_no' name='letter_no' value='' /><br /><label class='lblsmall'></label></td>
<td>Description<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='description' name='description' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>[Attach File]<sup style='color:red;'></sup></td><td>:</td><td>
    <input type="file" id='attachfile' name='attachfile'/>
<br /><label class='lblsmall'></label></td>
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/></td>
</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
 <sup style="color:Red;">*</sup>Required Fields
 <div id="listx" style=" width:70%; padding:0px 7px 0px 0px; float:left">
    <%  
        '  Dim loc As String
        Dim db As New dbclass
        Dim dt As datatablereader
        
        If keyp = "update" Then
          
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_aw_wr where id=" & Request.QueryString("id"), Session("con"))
      If dt.HasRows = True Then
          dt.Read()
                Response.Write("<script type='text/javascript'>")
                '  Response.Write("$('#date_return').css({visibility:'visible'});")

          For k As Integer = 0 To dt.FieldCount - 4
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
           
        Dim mk As New formMaker
        Dim row As String
       
        
        row = ""
     
      
        Dim sqlx As String = "select id,wtgiving,giving_date,letter_no,description,attachfile,emptid from emp_aw_wr where emp_id='" & Session("emp_id") & "' order by id desc"
        row = mk.edit_del_list3("emp_aw_wr", sqlx, "Letter type,Letter date,Letter no,Description,Attachfile", Session("con"), loc, floc, True, True, True, True)
        Response.Write(row)
                    
        'files uploaded
    
    %>
 </div>
 <%
     Dim f As New file_list
     loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
     Response.Write(f.filelist(floc))%>
 <div></div>
 <div style=" float:left; width:30%;">
 <script type="text/javascript">
    function onchangebb()
    {
        var str=$("#upload").formSerialize();
        //alert("jqueryupdate.aspx?empid=<% response.write(session("emp_id")) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&task=upload");
       $("#newpage").load("jqueryupdate.aspx?empid=<% response.write(session("emp_id")) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&what_task=upload");
      }
 </script>

 </div>
 <div id="print1" style=" float:right; width:59px; height:33px; color:Gray;" onclick="javascirpt:print('listx','','','');">print</div>
 <div style=" clear:both;"></div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Leave Request");
    //showobjar("formx","titlet",22,2);
   $( "#messagebox" ).text("<% response.write(Request.QueryString("msg")) %>");
  </script>
  
    
    <form id="frmx" action="" method="post">
    </form>
   <form id="frmup" action="" method="post" enctype="multipart/form-data">
    </form>
   
   <%  ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
          
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:delx('" & idx & "','yes','del123');" & Chr(34) & "></span>"
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
           <%  fs = Nothing
           End If

           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","workarea");
        $('#frmx').attr("action","<% response.write(Request.ServerVariables("URL")) %>?msg=<% response.write(msg) %>" );
        $('#frmx').submit();
    </script>
   <%  ElseIf flg = 2 Then
       %><script type="text/javascript">
             showMessage('<%response.write(msg) %>', 'date_taken_from');
       </script>
       <%
   End If
       f = Nothing
       
       
       
       %>
   
    <script type="text/javascript">
        function delx(val, ans, hd) {

            if (ans == "yes") {
                // alert(val + ans);
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=emp_aw_wr");
                $('#frmx').submit();
            }
            else {
                ha(hd);
            }
        }
   </script>
<%
  
    'Response.Write(savebtn)
    db = Nothing
    dt = Nothing
        
    
    
    ' loansettle()
    
    
    %>

<script type="text/javascript">
   $(document).ready(function() {
  
   });
</script>
 
</body>
</html>
