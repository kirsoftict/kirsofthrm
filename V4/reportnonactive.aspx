<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reportnonactive.aspx.vb" Inherits="reportnonactive" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" id='html' >
<head >
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css"/>
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
	
    
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
    function print(loc,title,head,footer)
    { 
    var ysno=confirm("please change the paper layout to landscape");
    
    if(ysno==true){
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<html><head><title>" + head +"</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
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
                }
              }
    </script>
</head>
<body>
	<% dim outp() as string
	    outp = getout()

    Dim fm As New formMaker
        
        Dim namelist As String = ""
        Dim dept As String = ""
        Dim proj As String
	    dept = fm.getjavalist2("tbldepartment", "dep_id,dep_name", Session("con"), "| ")
	    If outp(1) <> "" Then
	        namelist = fm.getjavalist22(outp(2), "emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	    Else
	        namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	    End If
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
<div style='float:left;'>
<form id='frmsbyname' method='post' action=''>
    <table cellspacing='0px' cellpadding='0px'><tr><td>
    <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" /><br/>
    <label class='lblsmall'>write name</label></td>
    <td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick="searchv('byname')" />
    </td></tr></table></form></div>
   <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer;float:left" onclick="javascirpt:print('allList','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico' alt="print"/>print</div>        
   <% Dim hd As String = ""
       
       hd &= " <div id='allList'>"
       hd &= " <table border='0' cellpadding='0' cellspacing='0' style='width: 990pt; border-collapse: collapse; height: 76px;'>"
       hd &= "<tr><td colspan='15' style='text-align:center;font-size:16pt; color:Blue'>" & outp(0) & "</td></tr>"
       hd &= "<tr  style='height: 15.75pt; font-size:12pt; color:#020202; font-weight:bold;background-color: #367898'>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid;"
       hd &= "border-top: windowtext 1pt solid; border-left: windowtext 1pt solid; width: 16pt;"
       hd &= "border-bottom: black 1pt solid; height: 47.25pt;'>"
       hd &= "No"
       hd &= "</td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 231pt;"
       hd &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>                   "
       hd &= "Full name</td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 34pt;"
       hd &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
       hd &= "<span style='font-size: 10pt'>"
       hd &= "sex</span></td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 74pt;"
       hd &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
       hd &= "Employment date</td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 64pt;"
       hd &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
       hd &= "Date of Resign</td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 111pt;"
       hd &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
       hd &= "Qualification</td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= " font-weight: bold;  border-left: windowtext 1pt solid; width: 54pt;"
       hd &= " border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
       hd &= "Award</td>"
       hd &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext; width: 47pt;  border-bottom: black 1pt solid;"
       hd &= "font-family:Times New Roman; ' rowspan='2'>"
       hd &= "&nbsp;Year of Graduation</td>"
       hd &= "<td class='xl67' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext; width: 123pt; border-bottom: black 1pt solid;"
       hd &= "font-family:Times New Roman; ' rowspan='2'>"
       hd &= "&nbsp;Institution</td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 79pt;"
       hd &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
       hd &= "Position</td>"
       hd &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 50pt;"
       hd &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
       hd &= "Salary</td>"
       hd &= "<td class='xl69' colspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext; width: 66pt; border-bottom: windowtext 1pt solid;"
       hd &= "font-family:Times New Roman; ' width='88'>"
       hd &= "Allowance</td>"
       hd &= "<td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 35pt;"
       hd &= "border-bottom: windowtext 1pt solid;  font-family:Times New Roman; '"
       hd &= "width='46'>"
       hd &= "Perdiem</td>"
       hd &= "<td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 98pt;"
       hd &= "border-bottom: windowtext 1pt solid; font-family:Times New Roman; '>"
       hd &= "project</td>"
       hd &= "<td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
       hd &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 90pt;"
       hd &= "border-bottom: windowtext 1pt solid;  font-family:Times New Roman; '>"
       hd &= "Tel/Mob</td>"
       hd &= " </tr>"
       hd &= "<tr style='font-weight: bold;   font-family:Times New Roman; height: 31.5pt;"
       hd &= "font-size:12pt; color:#020202; font-weight:bold;background-color: #367898'>"
       hd &= "<td class='xl64' style='border-right: windowtext 1pt solid; border-top: windowtext;"
       hd &= "border-left: windowtext; width: 42pt; border-bottom: windowtext 1pt solid; height: 31pt;'>"
       hd &= " Non"
       hd &= "                    <br />"
       hd &= "      Taxable</td>"
       hd &= "<td class='xl64' style='border-right: windowtext 1pt solid; border-top: windowtext;"
       hd &= "border-left: windowtext; width: 44pt; border-bottom: windowtext 1pt solid; height: 31pt;"
       hd &= "'>"
       hd &= "Taxable</td>"
       hd &= "</tr>"
       hd &= (outp(1))
       hd &= " </table>"
       hd &= "</div>"
         
        
       Dim obj As Object
       obj = hd
       Dim loc As String = Server.MapPath("download") & "\reportnonactive.txt"
       obj = "1;2;3" & Chr(13) & obj
       File.WriteAllText(loc, obj)
    
    
       If hd <> "" Then%>
        <div id='expxls' style="float:left; width:250px;">
<form id="exportexcel" name="exprtexcel" action="print.aspx?pagename=viewrpt-<% response.write(request.querystring("val")) %>" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="from file" />
    <input type="hidden" name="loc" id="loc" value="<% response.write(loc) %>" />
     <input type="hidden" name="rigt" id="rigt" value="1;2;3" />
    <div style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick="javascript:$('#exportexcel').submit();" >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="Excel" /> Export to excel</div>
</form></div> 
  <% End If
      
      Response.Write(hd)
      
      %>
  
</body>
</html>
<%  %>