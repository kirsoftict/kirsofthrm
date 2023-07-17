<%@ page language="vb" ValidateRequest="false" Debug="true" CodeFile="edit.aspx.vb" Inherits="_edit" %>
<%@ Import Namespace="system.IO" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.data.sqlclient" %>
<%@ Import Namespace="system.web.configration" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>
</title>
  <script type="text/javascript" src="jscripts/tiny_mce/tiny_mce.js"></script>
<script type="text/javascript">
function close()
{
alert("close1");
            window.setTimeout("close2();",1000);
           
           
}
function close2()
{
    alert("close2");
 window.close();
}
	tinyMCE.init({
		// General options
		mode : "textareas",
		theme : "advanced",
		skin : "o2k7",
		plugins : "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,inlinepopups,autosave",

		// Theme options
		theme_advanced_buttons1 : "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
		theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
		theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
		theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft",
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
</script>
</head>
<body>
<div class="logox" style="opacity:0.7;filter:alpha(opacity=60); width:290px; height:150px; background-color:#c79958;">
			
			<span style="margin-top:20px;"><img src="../images/logo_label.gif" alt="logo" width="290px" />
			</span>
			</div>
			<div>
			
               
    
    <%   
        ' Response.Write(makeform("menu") & "<br>")
        If Request.QueryString("id") <> "" And Request.QueryString("menu") <> "" And Request.QueryString("what") <> "" Then
            Response.Write(edit(Request.QueryString("menu"), Request.QueryString("what"), Request.QueryString("id")))
        ElseIf Request.Form("btnupdate") <> "" Then
            ' Context.Response.Write(updateon())
            Response.Write(updateon())
            %><%
            
        End If
       
        %>
   
    
    </div>
   


</body>

</html>