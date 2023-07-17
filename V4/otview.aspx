<%@ Page Language="VB" AutoEventWireup="false" CodeFile="otview.aspx.vb" Inherits="otview" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%
    dim fm as new formMaker
    dim namelist as string
     namelist = fm.getjavalist2("tblproject", "project_name,project_id", session("con"), "|")
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>


<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
<script src="jqq/ui/jquery.ui.button.js"></script>
    <script src="jqq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>

<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
	<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
<script type="text/javascript" src="scripts/form.js"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
 <script type="text/javascript" language="javascript">
var namelist=[<% response.write(namelist) %>];
    $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  namelist, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});
 
    function print(loc,title,head,footer)
    {
    
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<html><head><meta http-equiv='content-type' content='application/winword'><meta name='content-disposition' content='attachment; filename=payrol.doc'><title>" + head + "</title></head><body>" + printFriendly.innerHTML + "<br><label class='smalllbl'>" + footer + "</label></body></html>");
    printWin.document.close();
   printWin.window.print();   
    printWin.close();
    }
   
    </script>
  
    <title>overtime</title>
</head>
<body>
<div style="width:100%;  background:#6879aa; text-align:center;color:White; font-size:19pt;">Overtime viewer

    <form name="frmlistout" action="" method="post" style="font-size:12pt;">
        <select id="month" name="month">
            <%  For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next%>
        </select>
         <select id="year" name="year">
            <%  For i As Integer = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
         Enter Project:<input type="text" name="projname" id="projname" />
         <script type="text/javascript">$("#month").val("<% response.write(today.month) %>");</script>
         Paid in this month <input type="checkbox" id="paidlist" name="paidlist" value="list paid" />
         <input type="submit" value="submit" />
    </form></div>
 
      <asp:Literal ID="outp" runat="server"></asp:Literal>
       <asp:Literal ID="outp2" runat="server"></asp:Literal>
        
      <form id="ckk" name="ckk" method="post"></form>
<script type="text/jscript">

$("#ckk").attr("target","chksess");
$("#ckk").attr("action","checksession.aspx");
$("#ckk").submit();
</script>
</body>
</html>
