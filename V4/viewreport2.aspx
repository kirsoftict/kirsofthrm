<%@ Page Language='VB' AutoEventWireup='false' CodeFile='viewreport2.aspx.vb' Inherits='viewreportx' %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>

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
    function printxx(loc,title,head,footer)
    { 
   
    var ysno=confirm("please change the paper layout to landscape");
    
    if(ysno==true){
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<html><head><title>" + head +"</title> <link rel='stylesheet' type='text/css' href='css/kir.login.css' /></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
    printWin.close();
    }
    
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
                case "byproj":
                    str=$("#frmsbyproj").formSerialize();
              // alert(str);
   $("#frmsbyproj").attr("action","?val=byproj&" + str);
    $("#frmsbyproj").attr("target","frm_tar");
    $("#frmsbyproj").submit();
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
	    ' Response.Write(outp(2))
	    
	    If outp(1) <> "" Then
	        namelist = fm.getjavalist22(outp(2), "emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	        ' Response.Write(fm.getjavalist22(outp(2), "emp_static_info", "first_name,middle_name,last_name", Session("con"), " ").ToString)
	    Else
	        namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	        'Response.Write("kkkkk")

	    End If
	   	   


	    proj = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
	    '  'Response.Write(fm.helpback(False, True, False, False, "", ""))

        %>
    <script language="javascript" type="text/javascript">
    var namelist=[<% response.write(namelist) %>];
    var proj=[<% response.write(proj) %>];
</script>
<div style="float:left;">
<form id='frmsbyname' method='post' action=''>

    <table cellspacing='0px' cellpadding='0px'><tr><td>
    <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" /><br/>
    <label class='lblsmall'>write name</label></td>
    <td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick="searchv('byname')" />
    </td></tr></table></form></div>
    <div style="float:left;">
    <form id='frmsbyproj' method='post' action=''>
    <table cellspacing='0px' cellpadding='0px'><tr><td>
    <input type='text' name='projx' id='projx' style='font-size:9pt;'  onkeyup="javascript:startwith('projx',proj);" /><br/>
    <label class='lblsmall'>write project</label></td>
    <td valign='top'><input type='button' class='searchx' id='Button1' value='Go' name='searchx' onclick="searchv('byproj')" />
    </td></tr></table></form></div>



  <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:printxx('allList','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico' alt="print"/>print</div>        
  <% Dim outp3 As String = ""%>
  <%  outp3 &= "<div id='allList'>"
      outp3 &= "        <table class='leavec' border='0' cellpadding='0' cellspacing='0' style='width: 800px; border-collapse: collapse; font-size:9pt;'>"
      outp3 &= "<tr><td colspan='15' style='text-align:center;font-size:16pt; color:Blue'>" & outp(0) & "<br />Leave Summery Report</td></tr>"
            outp3 &= "<tr  style='height: 15.75pt; font-size:12pt; color:#020202; font-weight:bold;background-color: #367898'>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid;"
      outp3 &= "border-top: windowtext 1pt solid; border-left: windowtext 1pt solid;"
      outp3 &= "border-bottom: black 1pt solid; height: 47.25pt;'>"
      outp3 &= " No"
      outp3 &= "</td>"
      outp3 &= "<td>Comp. No</td>"
      outp3 &= "<td class='xl67'  style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= "  font-weight: bold;  border-left: windowtext 1pt solid;"
      outp3 &= " border-bottom: black 1pt solid;  font-family:Times New Roman; '>                   "
      outp3 &= "Full name</td>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= "font-weight: bold;  border-left: windowtext 1pt solid; "
      outp3 &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
      outp3 &= "<span style='font-size: 10pt'>"
      outp3 &= "Year</span></td>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= "font-weight: bold;  border-left: windowtext 1pt solid; "
      outp3 &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
      outp3 &= "Budget</td>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= "  font-weight: bold;  border-left: windowtext 1pt solid; "
      outp3 &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
      outp3 &= "Available</td>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= " font-weight: bold;  border-left: windowtext 1pt solid; "
      outp3 &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
      outp3 &= "Used</td>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= "    font-weight: bold;  border-left: windowtext;  border-bottom: black 1pt solid;"
      outp3 &= "font-family:Times New Roman; ' >"
      outp3 &= "&nbsp;Balance</td>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= "font-weight: bold;  border-left: windowtext; border-bottom: black 1pt solid;"
      outp3 &= "font-family:Times New Roman; ' >"
      outp3 &= "&nbsp;Is Expire</td>"
      outp3 &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
      outp3 &= "font-weight: bold;  border-left: windowtext; border-bottom: black 1pt solid;"
      outp3 &= "font-family:Times New Roman; ' >"
      outp3 &= "&nbsp;Paid</td>"
      outp3 &= "     </tr>"
      outp3 &= outp(1)
      outp3 &= " </table>"
      outp3 &= "<table><tr><td width='15' style='height:14px; background-color:Red'>&nbsp;</td><td>Expired Portion</td></tr></table>"
      outp3 &= "</div>"
      
        
      Dim obj As Object
      obj = outp3
      Dim loc As String = Server.MapPath("download") & "\viewreport2.txt"
      obj = "1;2;3" & Chr(13) & obj
      File.WriteAllText(loc, obj)
    
    
      If outp3 <> "" Then%>
        <div id='expxls' style="float:left; width:250px;">
<form id="exportexcel" name="exprtexcel" action="print.aspx?pagename=viewrpt-<% response.write(request.querystring("val")) %>" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="from file" />
    <input type="hidden" name="loc" id="loc" value="<% response.write(loc) %>" />
    <input type="hidden" name="rigt" id="rigt" value="<% response.write(session("right")) %>" />
    <div style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick="javascript:$('#exportexcel').submit();" >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="Excel" /> Export to Excel</div>
</form></div> 
  <% End If
      
      Response.Write(outp3)
      %>
    %>
        
  <script>
   // showobj("outp");
  </script>
</body>
</html>
