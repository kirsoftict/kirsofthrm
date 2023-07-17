<%@ Page Language="VB" AutoEventWireup="false" CodeFile="dbbackup.aspx.vb" Inherits="dbbackup" %>
<%@ Import Namespace="system.IO" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.data.sqlclient" %>
<%@ Import Namespace="Kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
     <title>Untitled Page</title>
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

	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<link rel="stylesheet" href="jq/demos.css" />
	
        
</head>
<body  style="overflow:auto;">
<%Dim fp As String = Server.MapPath("")
    Dim sec As New k_security
    %>
<script>
    function summm() {
        //alert("wt is going on");
       // $('#frmx').attr("target", "frm_tar");

       // $('#frmx').attr("action", "?backup=on");
        // $('#frmx').submit();
        $('#frmx').attr("target", "_blank");

        $('#frmx').attr("action", "http://kirsoft:8090/dbbackup.aspx");
        $('#frmx').submit();
    }
              
             
       </script>
         
       <div id="clock"></div>
       <form id='frmx' name='frmx' method="post">
            <input type='hidden' id='fileloc' name='fileloc' value='<%=fp %>'/>
       </form>
      <div> <a href="javascript:summm();">Backup Database</a></div>
       <div><iframe width="100%" height="100%" frameborder=0 id="filesystem" name="filesystem" src="filesystem.aspx?root=<%=sec.dbstrtohex(server.mappath("") & "/dbbackup") %>"></iframe></div>

       <script language="javascript" type="text/javascript">
           var frm;
           frm = document.getElementById("filesystem");
           //alert(frm.innerHTML);
           
       
       </script>
</body>
</html>


  