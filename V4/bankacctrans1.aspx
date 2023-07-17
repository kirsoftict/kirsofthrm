<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bankacctrans1.aspx.vb" Inherits="bankacctrans1" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%
    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim fl As New file_list
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><title></title>
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
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

<script type="text/javascript" src="jq/ui/jquery.ui.autocomplete.js"></script>
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>

<style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>
	<%  	   
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", session("con"), "|")

 %>
 <script type="text/javascript">
 
 $("#st").text(window.status);
 function checkall()
 {
    if($(".chkbox").is(':checked'))
    {
        $("#chkall").text("Checked All");
        
        $(".chkbox").attr("checked",false);
       
    }
    else
    {
        $(".chkbox").attr("checked",true);
       $("#chkall").text("clear All");
    }
  sumcolx();
 }
  
 //alert(document.referrer.toString())

function submittop()
{
document.frmlistout.action="payroll.aspx";
document.frmlistout.submit();
}
  
    function print(loc,title,head,footer)
    {
    
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("print.htm",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<html><head><meta http-equiv='content-type' content='application/winword' /><meta name='content-disposition' content='attachment; filename=payrol.doc' /><title>" + head + "</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>" + footer + "</label></body></html>");
    printWin.document.close();
   printWin.window.print();   
  // printWin.close();
    }
   
    </script>
</head>

<body style="height:auto;">
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>
<div style="width:100%; height:45px; background:#6879aa; text-align:center;color:White; font-size:19pt;">Payroll Maker</div>

   <%  Response.Write(fl.msgboxt("pay", "Confirmation", "<iframe name='fpay' id='fpay' width='900' height='500' frameborder='0' src='' scrolling='yes'></iframe>"))
%>
<div id="st"></div>
<%  If Request.QueryString.HasKeys = True Then
        For Each k As String In Request.QueryString
            '  Response.Write(k & "==" & Request.QueryString(k) & "<br>")
        Next
    End If
    If Request.Form.HasKeys = True Then
        For Each k As String In Request.Form
            ' Response.Write(k & "==" & Request.Form(k) & "<br>")
        Next
    End If
    
    %>
    <form name="frmlistout" action="" method="post">
     <table><tr><td>payment Date</td><td>:</td><td><input type='text' id='to_date' name='to_date' /></td>
                    <script language='javascript' type='text/javascript'> 
         $(function() {$( '#to_date').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	}); $( '#to_date' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>
         <td>
         Enter Project<input type="text" name="projname" id="projname" /></td><td>
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submittop();" />
         </td></tr></table>
    </form>
     <form name="frmpay" id="frmpay" action="" method="post">
    <div id="bigprint">
             <% makebank() %>
        </div>
        </form>
       <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico' alt="print"/>print</div>        
<script type="text/jscript">

$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();

</script>

</body>
</html>
