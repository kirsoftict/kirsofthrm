<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rpttinpen.aspx.vb" Inherits="rpttinpen" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
	<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
    function print(loc,title,head,footer)
    { 
   
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<html><head><title>" + head +"</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
  
   
    
    }
    function searchv(val)
        {var str=""
            switch(val)
            {
                case "alls":
                     alert("all called");
                break;
                case "byname":
                // alert("byname");
               str=$("#frmsbyname").formSerialize();
              // alert(str);
   $("#frmsbyname").attr("action","?val=namex&" + str);
    $("#frmsbyname").attr("target","frm_tar");
    $("#frmsbyname").submit();
                break;
                }
              }
    </script>
</head>
<body>
	<% dim outp() as string
	    outp = getout()
	    ' Response.Write(outp(1))
    Dim fm As New formMaker
        
        Dim namelist As String = ""
        Dim dept As String = ""
        Dim proj As String
	    'dept = fm.getjavalist2("tbldepartment", "dep_id,dep_name", Session("con"), "| ")
	    'Response.Write(outp(2))
	    If outp(2) <> "" Then
	        namelist = fm.getjavalist22(outp(2), "emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	    Else
	        namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	    End If
	    
	    'namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	    'Response.Write("kkkkk")

	   
	    proj = fm.getjavalist2("tblproject", "project_id,project_name", Session("con"), "| ")
	    Dim disc As String = fm.getjavalist2("tbldiscipline", "discipline", Session("con"), "")
	    Dim qual As String = fm.getjavalist2("tblqualification", "qualification", Session("con"), "")
	    Dim post As String = fm.getjavalist2("tblposition", "job_position", Session("con"), "")
        
        %>
    <script language="javascript" type="text/javascript">
    var namelist=[<% response.write(namelist) %>];
    var deplist=[<% response.write(dept) %>];
    var proj=[<% response.write(proj) %>];
     var disc=[<% response.write(disc) %>];
      var qual=[<% response.write(qual) %>];
      var posx=[<% response.write(post) %>];
</script>
<div style="float:left;">
<form id='frmsbyname' method='post' action=''>
    <table cellspacing='0px' cellpadding='0px'><tr><td>
    <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" /><br/>
    <label class='lblsmall'>write name</label></td>
    <td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick="searchv('byname')" />
    </td></tr></table></form>
    
    </div>
     <div style="float:left; width:5px;"></div>

  <div id="print"  style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;float:left" onclick="javascirpt:print('allList','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico' alt="print"/>print</div>        
  <div style="float:left; width:5px;"></div>
   <%
        
    Dim obj As Object
       obj = outp(1)
       Dim loc As String = Server.MapPath("download") & "\rpttin.txt"
    obj = "1;2;3" & Chr(13) & obj
    File.WriteAllText(loc, obj)
    
    
       If outp(1) <> "" Then%>
        <div id='expxls' style="float:left; width:250px;">
<form id="exportexcel" name="exprtexcel" action="print.aspx?pagename=viewrpt-<% response.write(request.querystring("val")) %>" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="from file" />
    <input type="hidden" name="loc" id="loc" value="<% response.write(loc) %>" />
    <input type="hidden" name="right" id="right" value="<% response.write(session("right")) %>" />
    <div style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick="javascript:$('#exportexcel').submit();" >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="Excel" /> Export to Excel</div>
</form></div> <div style="clear:both"></div>
  <% End If  %> 
  <div id="allList">
        
           
           <% Response.Write(outp(1))%>
        
    <table><tr><td width="15" style="height:14px; background-color:Red">&nbsp;</td><td>None Active Employees</td></tr></table>
    </div>
        
  
</body>
</html>
