<%@ Page Language="VB" AutoEventWireup="false" ValidateRequest="false" Debug="true"  CodeFile="helpentry.aspx.vb" Inherits="helpentry" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%   If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           window.location = "logout.aspx";
</script>
       <%
       End If
       idx = Request.QueryString("id")
       Dim fm As New formMaker
   %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
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

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/script.js" type="text/javascript"></script>

	<script type="text/javascript" src="scripts/form.js"></script>

  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script type="text/javascript" src="jscripts/tiny_mce/tiny_mce.js"></script>
	
<%  Dim dbc As New dbclass
   
    Dim listids As String
    '/listids = dbc.idlist_jqry("tbl_leave_type", "leave_type", session("con"))
    
    dbc = Nothing
    %>
   <script type="text/javascript">
   tinyMCE.init({
		// General options
		mode : "textareas",
		theme : "advanced",
		skin : "o2k7",
		plugins : "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,inlinepopups,autosave",

		// Theme options
		theme_advanced_buttons1 : "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
		theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
		//theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
		//theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft",
		theme_advanced_toolbar_location : "top",
		theme_advanced_toolbar_align : "left",
		theme_advanced_statusbar_location : "bottom",
		theme_advanced_resizing : true,

		// Example word content CSS (should be your site CSS) this one removes paragraph margins
		content_css : "css/word.css",

		// Drop lists for link/image/media/template dialogs
		template_external_list_url : "lists/template_list.js",
		external_link_list_url : "lists/link_list.js",
		external_image_list_url : "lists/image_list.js",
		media_external_list_url : "lists/media_list.js",

		// Replace values for the template plugin
		template_replace_values : {
			username : "Some User",
			staffid : "991234"
		}
	});
   var listx=[<% response.write(listids) %>];
var prv;
  prv="";
var id;
var focused="";
var requf=["x"];
var fieldlist=["help","helptext","heading_help,order_help","x"];
function validation1(){
if ($('#heading_help').val() == '') {showMessage('Heading cannot be empty','heading_help');$('#heading_help').focus();return false;}
//if ($('#active').val() == '') {showMessage('active cannot be empty','active');$('#active').focus();return false;}
 if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmtbl_leave_type").formSerialize();
  // alert(str);
   $("#frmtbl_leave_type").attr("action","?tbl=tblhelp&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" );
    $("#frmtbl_leave_type").submit();
  return true;}
  }
} </script>
<style>
		
		#button { padding: .5em 1em; text-decoration: none; }
		#effect { width:900px; height: auto; padding: 0.4em; position: relative; }
		#effect h3 { margin: 0; padding: 0.4em; text-align: left; }
		.ui-effects-transfer { border: 2px dotted gray; }

	</style>
</head>
<body style="height:auto;">
<div id="helpx"></div>
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

 <div id="effect" class="ui-widget-content ui-corner-all">
		<h3 class="ui-widget-header ui-corner-all">Effect</h3>
		<p><span id="messagebox">
 <% if msg<>"" then
 response.write(msg)
 end if %>
 </span>
			
<% whydoc()%>
		</p>
	</div>

 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from tblhelp where id=" & Request.QueryString("id"), Session("con"))
            If dt.HasRows = True Then
                dt.Read()
                Dim sec As New k_security
                Response.Write("<script type='text/javascript'>")
                %>
                    $('#<% Response.Write("help_order').val('" & dt.Item("help_order") & "');")%>
                     $('#<% Response.Write("heading_help').val('" & dt.Item("heading_help") & "');")%>
                      $('#<% Response.Write("helptext').val('" & dt.Item("helptext") & "');")%>
                      
                   
                    <%
               
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
            Dim sqlx As String = "select id,heading_help,helptext,help from tblhelp order by id desc"
                row = edit_del_list_hlp("tblhelp", sqlx, "Heading,Link Page,help text", Session("con"), loc)
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

               $("#dialog-modal").dialog({
                   resizable: true

               });
           </script>
           <%
           
       End If  
        %>
 </div>
<script type="text/javascript" language="javascript">
    //hform=findh(document.getElementById("middle_bar"));
    $('h3').text("Add Help");
    //showobjar("formx","titlet",22,2);
</script>
    <script type="text/javascript">
        function del(val, ans, hd) {

            if (ans == "yes") {
               // alert("<% response.write(loc) %>?task=delete&id=" + val + "&tbl=tblhelp");
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=tblhelp");
                $('#frmx').submit();
            }
            else {
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "?task=&id=" + val + "&tbl=tblhelp");
                $('#frmx').submit();

            }
        }
       
   </script>
    <form id="frmx" action="" method="post">
    </form>
    <script type="text/javascript">
        $(document).ready(function () {


            $("#helptext").autocomplete({
                source: listx
            });
        });
  </script>

</body>
</html>

