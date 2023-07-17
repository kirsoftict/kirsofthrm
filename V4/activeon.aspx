<%@ Page Language="VB" AutoEventWireup="false" CodeFile="activeon.aspx.vb" Inherits="activeon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link rel="stylesheet" href="./Employment Tax_files/style.css" type="text/css" media="all">
<link rel="stylesheet" href="./Employment Tax_files/style.css" type="text/css" media="all">
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
    	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.dialog.js"></script>
	 
       
   
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>



<script src="scripts/script.js" type="text/javascript"></script>


   <script language="javascript" type="text/javascript" src="scripts/kirsoft.iso.js"></script> 
            <script type="text/javascript" src="chart/sources/jscharts.js"></script>

 <script type="text/javascript">
 var month;
 var year;
 var isotype;
 isotype="";
 pd1="";
 pd2="";
 month="";
 year="";
// var stime=Date.now();
<% if isdate(Request.QueryString("pd2")) then
response.write("var pd2=" & chr(34) & request.querystring("pd2") & chr(34) & ";")
else if isdate(Request.form("pd2")) then
response.write("var pd2=" & chr(34) & request.form("pd2") & chr(34) & ";")
end if 


%>
         

                function submitiso() {
                    $('#frmlistout').attr("action", "?pd2=" + pd2);

                    $("#frmlistout").submit();

                }
   
            </script>

</head>
<body>
    <form id='frmx' name='frmx' method="post" style='display:none'><input type="text" name="txtpass" /></form>
  
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px; z-index:99999; position:relative;">
HRM ISO Active Employees 
    <form name="frmlistout" id="frmlistout" action="" method="post" style="font-size:12pt;">
        as Date of: 
       
        <input type="text" id='pd2' name='pd2' value='<% response.write(today.toshortdatestring) %>' />
        <script language="javascript" type="text/javascript">

            dateonly('pd2');
           // dateafteronly('pd1', 'pd2');
        </script>
   
        <%  ' If Request.Item("pd1") <> "" Then
            'Response.Write(Request.Item("pd1"))
            '  End If %>
      
         
         <input type="hidden" id="pagesize" name="pagesize" value="25" />
         <input type="hidden" id="pper" name="pper" value="P1:" />
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submitiso();" />
    </form>
    </div>
   
                     
      <div style="width:5px; float:left;"></div> 
     
                   <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form> 
<%  If Request.Item("pd2") <> "" Then
        activeworker()
    End If%>

</body>
</html>
