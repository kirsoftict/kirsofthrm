<%@ Page Language="VB" AutoEventWireup="false" CodeFile="homepage.aspx.vb" Inherits="homepage" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" id='welcomp'>
<head>
    <title>Untitled Page</title>
 
   <link rel="stylesheet" href="css/tabcontent.css" />
   <link rel="stylesheet" href="css/templatemo_style.css" />
   <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

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
    <script src="scripts/kirsoft.java.js"></script>
    <link rel="stylesheet" href="jqq/demos/demos.css">
    
    <script>
    function disabletext(e){
return false
}

function reEnable(){
return true
}

//if the browser is IE4+
document.onselectstart=new Function ("return false")

//if the browser is NS6
if (window.sidebar){
document.onmousedown=disabletext
document.onclick=reEnable
}
</script>

      <script>
          function showHideSubMenu(link, id) {

              var uldisplay = "";
              var newClass = "";
              var idlink = "";
              var parent;
              parent = link.parentNode;
              var spanx;
              
              spanx = parent.getElementsByTagName('span');
             
              if (link.className == 'expanded') {

                  // Need to hide

                  uldisplay = 'none';
                  newClass = 'collapsed';

                  $(spanx[1]).css({ background: 'url(images/gif/collapsed.gif) no-repeat' });

              } else {
                  // Need to show
                  uldisplay = 'block';
                  newClass = 'expanded';

                  $(spanx[1]).css({ background: 'url(images/gif/expanded.gif) no-repeat' });
              }


              $("#" + id).css({ 'display': uldisplay });
              link.className = newClass;
              
          }
     
          function closedx(id) {
              var sql;
              sql = "update emp_resign set active='n' where id=" + id;
              $('#frmx').attr("target", "fpay");
              $("#fpay").attr("frameborder", "0");

              $('#frmx').attr("action", "frmdatasaveonly.aspx?sql=" + sql + "&tar=accouting.aspx");
              $('#frmx').submit();
              // $('#post').attr("disabled", "disabled");
              $('#pay').css({ top: '0px', left: '0px' });
              $("#pay").remove("display");
              $("#pay").dialog({
                  title: 'Data Saving',
                  height: 300,
                  width: 600,
                  modal: true
              });


          }
          function balancep(id) {

          }
</script>
<script type="text/javascript">
    function seenon(id, bywho, datex) 
    {
        var sql;
        sql = "update rptdataupdate set seen=1, seendate='" + datex + "',seenby='" + bywho + "' where id=" + id;
        $("#fpay").attr("frameborder", "0");
        $('#frmx').attr("target", "fpay");
        $('#frmx').attr("action", "frmdatasaveonly.aspx?sql=" + sql);
        $('#frmx').submit();
        $('#pay').css({ top: '0px', left: '0px' });
        $("#pay").remove("display");
        $("#pay").dialog({
            top: 30,
            title: 'Bank St.',
            height: 500,
            width: 800,
            modal: true
        });

        $("#fpay").css({ width: '800px' });
    }
   
   
    function openwin(whr,title) {
        var sql;
      // alert(whr);
       // sql = "update rptdataupdate set seen=1, seendate='" + datex + "',seenby='" + bywho + "' where id=" + id;
        $("#fpay").attr("frameborder", "0");
        $('#frmx').attr("target", "fpay");
        $('#frmx').attr("action", whr);
        $('#frmx').submit();
        $('#pay').css({ top: '0px', left: '0px' });
        $("#pay").remove("display");
        $("#pay").dialog({
            top: 30,
            title: title,
            height: 500,
            width: 800,
            modal: true
        });

        $("#fpay").css({ width: '800px' });
    }
   /* function openwin(whr, title,tf,di) {
        var sql;
       
        // sql = "update rptdataupdate set seen=1, seendate='" + datex + "',seenby='" + bywho + "' where id=" + id;
        $("#"+tf).attr("frameborder", "0");
        $('#frmx').attr("target", tf);
        $('#frmx').attr("action", whr);
        $('#frmx').submit();
        $('#' +di).css({ top: '0px', left: '0px' });
        $("#"+di).remove("display");
        $("#"+di).dialog({
            top: 30,
            title: title,
            height: 500,
            width: 800,
            modal: true
        });

        $("#"+tf).css({ width: '800px' });
    }
    */
   if("4.0 (compatible; MSIE 7.0; Windows NT 6.1; Win64; x64; Trident/5.0; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)" !=window.navigator.appVersion)
   {
       // alert("browser is not supported");
       //alert(window.navigator.appVersion);
   }
  // alert(window.navigator.appVersion); 
   $(function () {
       $("#tabs").tabs();
   });
   $(function () {
       $("#tabs2").tabs();
   });
   function showdis(id, nclass, ths) {

      
       $("." + nclass).css({ display: "none" });

       $("#" + id).css({ display: "inline" });

   }
</script>
<style>
    .lft-1
    {
    	
    	}
</style>
<script language="javascript" type="text/javascript">
    function orderpass(id) {
        //alert();
        //window.location="empcontener.aspx?id=" + id.toString();
        $("#datatake").val(id);

        $("#fpay").attr("frameborder", "0");
        $("#fpay").css({width:'1300px',height:'800px'});
         $("#fpay").attr("scrolling", "auto");
        $('#frmemplist').attr("target", "fpay");
        $('#frmemplist').attr("action", "empcontener.aspx");
        $('#frmemplist').submit();
        $('#pay').css({ top: '0px', left: '0px' });
        $("#pay").remove("display");
        $("#pay").dialog({
            top: 30,
            title: 'Error display',
            width: 1300,
            modal: true
        });
    }
</script>
</head>
<body >
<%  Session("emptid")=""
    Dim fl As New file_list
    Dim rtx() As String
    Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
    Response.Write(fl.dialog("conte", "", "<iframe name='gg' id='gg' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))

    %>
    <div id='lod'>Loading...</div>
    <div id='lodfinsh' style='display:none'>
    <asp:Literal ID='pagecont' runat='server'></asp:Literal>
   </div>
   <script language="javascript" type="text/javascript">
   $(document).ready(function() {
   $("#lod").css({display:"none"});
   $("#lodfinsh").css({ display: "inline" });
   <% if passno=1 then
 
                   response.write("openwin('msgout.aspx',  'Warrning!','gg','conte');")
                   end if
    %>
   });
   </script>
         
</body>
</html>

