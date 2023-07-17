<%@ Page Language="VB" AutoEventWireup="false" CodeFile="hrmwork.aspx.vb" Inherits="hrmwork" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  If session("con").ToString = "" Then
       
                %>
                   <script type="text/javascript">
                        window.location="logout.aspx";
                   </script>
                <%
                End If
                'Dim fname, sname, lname As String
                Dim rt, sql As String
                sql = ""
                Select Case Request.QueryString("task")
                    Case "byname"
                        rt = Request.QueryString("vname")
                        Dim rts() As String
                        rts = rt.Split(" ")
                        sql = "select * from emp_static_info where first_name='" & rts(0) & "' and middle_name='" & rts(1) & "' and last_name='" & rts(2) & "'"
            %> 
            <script type='text/javascript'>
            $("#form1").attr("action","dataallview.aspx?sql=<% response.write(sql) %>");
             $("#form1").attr("target","workarea");
    $("#form1").submit();
    </script>
  <%
      End Select
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>

    <script src="jqq/ui/jquery.ui.button.js"></script>

    <script src="jqq/ui/jquery.ui.dialog.js"></script>
    <script type="text/javascript" src="scripts/form.js"></script>
		<script src="jqq/ui/jquery.ui.datepicker.js"></script>


<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>

	<script type="text/javascript" src="scripts/form.js"></script>


<link rel="stylesheet" type="text/css" media="screen" href="css/jquery/jquery.autocomplete.css" />
    <style type="text/css">
    #searchbox
{
	background: #eaf8fc;
	background-image: -moz-linear-gradient(#fff, #d4e8ec);
	background-image: -webkit-gradient(linear,left bottom,left top,color-stop(0, #d4e8ec),color-stop(1, #fff));
	
	-moz-border-radius: 35px;
	border-radius: 35px;
	
	border-width: 1px;
	border-style: solid;
	border-color: #c4d9df #a4c3ca #83afb7;            
	width: 500px;
	height: 35px;
	padding: 10px;
	margin 10px auto 50px;
	overflow: hidden; /* Clear floats */
}
#search, #submit
{
	float: left;
}

#search
{
	padding: 5px 9px;
	height: 23px;
	width: 380px;
	border: 1px solid #a4c3ca;
	font: normal 13px 'trebuchet MS', arial, helvetica;
	background: #f1f1f1;
	
	-moz-border-radius: 50px 3px 3px 50px;
	 border-radius: 50px 3px 3px 50px;
	 -moz-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.25) inset, 0 1px 0 rgba(255, 255, 255, 1);
	 -webkit-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.25) inset, 0 1px 0 rgba(255, 255, 255, 1);
	 box-shadow: 0 1px 3px rgba(0, 0, 0, 0.25) inset, 0 1px 0 rgba(255, 255, 255, 1);            
}

/* ----------------------- */

#submit
{		
	background: #87cefa;
	background-image: -moz-linear-gradient(#95d788, #6cbb6b);
	background-image: -webkit-gradient(linear,left bottom,left top,color-stop(0, #6cbb6b),color-stop(1, #95d788));
	
	-moz-border-radius: 3px 50px 50px 3px;
	border-radius: 3px 50px 50px 3px;
	
	border-width: 1px;
	border-style: solid;
	border-color: #7eba7c #578e57 #447d43;
	
	 -moz-box-shadow: 0 0 1px rgba(0, 0, 0, 0.3), 0 1px 0 rgba(255, 255, 255, 0.3) inset;
	 -webkit-box-shadow: 0 0 1px rgba(0, 0, 0, 0.3), 0 1px 0 rgba(255, 255, 255, 0.3) inset;
	 box-shadow: 0 0 1px rgba(0, 0, 0, 0.3), 0 1px 0 rgba(255, 255, 255, 0.3) inset;   		

	height: 35px;
	margin: 0 0 0 10px;
        padding: 0;
	width: 90px;
	cursor: pointer;
	font: bold 14px Arial, Helvetica;
	color: #23441e;
	
	text-shadow: 0 1px 0 rgba(255,255,255,0.5);
}

#submit:hover
{		
	background: #95d788;
	background-image: -moz-linear-gradient(#6cbb6b, #95d788);
	background-image: -webkit-gradient(linear,left bottom,left top,color-stop(0, #95d788),color-stop(1, #6cbb6b));
}	

#submit:active
{		
	background: #95d788;
	outline: none;
   
	 -moz-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.5) inset;
	 -webkit-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.5) inset;
	 box-shadow: 0 1px 4px rgba(0, 0, 0, 0.5) inset;		
}

#submit::-moz-focus-inner
{
       border: 0;  /* Small centering fix for Firefox */
}		
.searchx
{		
	background: #87cefa;
	background-image: -moz-linear-gradient(#95d788, #6cbb6b);
	background-image: -webkit-gradient(linear,left bottom,left top,color-stop(0, #6cbb6b),color-stop(1, #95d788));
	
	-moz-border-radius: 3px 15px 15px 3px;
	border-radius: 3px 15px 15px 3px;
	
	border-width: 1px;
	border-style: solid;
	border-color: #7eba7c #578e57 #447d43;
	
	 -moz-box-shadow: 0 0 1px rgba(0, 0, 0, 0.3), 0 1px 0 rgba(255, 255, 255, 0.3) inset;
	 -webkit-box-shadow: 0 0 1px rgba(0, 0, 0, 0.3), 0 1px 0 rgba(255, 255, 255, 0.3) inset;
	 box-shadow: 0 0 1px rgba(0, 0, 0, 0.3), 0 1px 0 rgba(255, 255, 255, 0.3) inset;   		
	height: 20px;
	margin: 0 0 0 10px;
        padding: 0;
	width: 40px;
	cursor: pointer;
	font: bold 14px Arial, Helvetica;
	color: #23441e;
	text-align:left;
	text-shadow: 0 1px 0 rgba(255,255,255,0.5);
}

.searchx:hover
{		
	background: #95d788;
	background-image: -moz-linear-gradient(#6cbb6b, #95d788);
	background-image: -webkit-gradient(linear,left bottom,left top,color-stop(0, #95d788),color-stop(1, #6cbb6b));
}	

.searchx:active
{		
	background: #95d788;
	outline: none;
   
	 -moz-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.5) inset;
	 -webkit-box-shadow: 0 1px 4px rgba(0, 0, 0, 0.5) inset;
	 box-shadow: 0 1px 4px rgba(0, 0, 0, 0.5) inset;		
}

.searchx::-moz-focus-inner
{
       border: 0;  /* Small centering fix for Firefox */
}		
    </style>
    <%  Dim fm As New formMaker
        
        Dim namelist As String = ""
        Dim dept As String = ""
        Dim proj As String
        dept = getjavalist("tbldepartment", "dep_id,dep_name", session("con"), "| ")
        namelist = getjavalist("emp_static_info", "first_name,middle_name,last_name", session("con"), " ")
        proj = getjavalist("tblproject", "project_id,project_name", session("con"), "| ")
        Dim disc As String = getjavalist("tbldiscipline", "discipline", session("con"), "")
        Dim qual As String = getjavalist("tblqualification", "qualification", session("con"), "")
        Dim post As String = getjavalist("tblposition", "job_position", session("con"), "")
        
        %>
    <script language="javascript" type="text/javascript">
    var namelist=[<% response.write(namelist) %>];
    var deplist=[<% response.write(dept) %>];
    var proj=[<% response.write(proj) %>];
     var disc=[<% response.write(disc) %>];
      var qual=[<% response.write(qual) %>];
      var posx=[<% response.write(post) %>];
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
               //alert(str);
   $("#frmsbyname").attr("action","dataallview.aspx?task=byname&" + str);
    $("#frmsbyname").attr("target","frm_tar");
    $("#frmsbyname").submit();
                break;
                case "department":
                str=$("#frmdep").formSerialize();
   $("#frmdep").attr("action","viewreport1.aspx?val=bydep&" + str);
    $("#frmdep").submit();
               // alert(val);
                break;
                case "fromtorec": 
               str=$("#fromtorec").formSerialize();
                 $("#fromtorec").attr("target","frm_tar");
   $("#fromtorec").attr("action","viewreport1.aspx?val=byrectime&" + str);
    $("#fromtorec").submit();
               // alert(val);
                break;
                case "fromtoproj":
                str=$("#fromtoproj").formSerialize();
   $("#fromtoproj").attr("action","viewreport1.aspx?val=byprojdate&" + str);
    $("#fromtoproj").submit();
              // alert(str);
                break;
                 case "proj":
                str=$("#proj").formSerialize();
   $("#proj").attr("action","viewreport1.aspx?val=byproj&" + str);
    $("#proj").submit();
             // alert(str);
                break;
                  case "disc":
                str=$("#frmdisc").formSerialize();
   $("#frmdisc").attr("action","viewreport1.aspx?val=bydis&" + str);
    $("#frmdisc").submit();
               // alert(str);
                break;
                  case "qual":
                str=$("#frmqual").formSerialize();
   $("#frmqual").attr("action","viewreport1.aspx?val=byqual&" + str);
    $("#frmqual").submit();
                //alert(str);
                break;
                 case "post":
                str=$("#frmpost").formSerialize();
   $("#frmpost").attr("action","viewreport1.aspx?val=bypost&" + str);
    $("#frmpost").submit();
                //alert(str);
                break;
                default:
               // alert(val + "errror");
                break
            }
        
        }
  
    </script>
</head>
<body>

<div style="display:none;">
 <form id="searchbox" action="">

    <input id="search" type="text">
    <input id="submit" type="button" value="Search" onclick="javascript:searchv('alls');">
</form>
</div>
<div id="topfram">
<div id="col11" style="float:left;">

<% 
    Dim cont As String
    cont = "<form id='frmsbyname' method='post' action=''>" & _
    "<table cellspacing='0px' cellpadding='0px'><tr><td>" & _
    "<input type='text' name='vname' id='vname' style='font-size:9pt;' ><br><label class='lblsmall'>write name,father name or grand father</label></td>" & _
    "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('byname');" & Chr(34) & ">" & _
    "</td></tr></table></form>"
    Response.Write(formmake("row11", cont, 300, 100, "Search By Name"))

%>

</div><div style="width:10px;float:left;">&nbsp;</div>
<div id="col21" style="float:left;">
<% 
    'Dim cont As String
    cont = "<form id='frmdep' method='post' action=''>" & _
    "<table cellspacing='0px' cellpadding='0px'><tr><td>" & _
    "<input type='text' name='department' id='department' style='font-size:9pt;' ><br><label class='lblsmall'>write Department name</label></td>" & _
    "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('department');" & Chr(34) & ">" & _
    "</td></tr><td>Selct <select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row12", cont, 300, 100, "Search By Department"))

%></div>
<div style="width:10px;float:left;">&nbsp;</div>
<div id="Div1" style="float:left;"><% 
    'Dim cont As String
                                       cont = "<form id='fromtorec' method='post' action=''>" & _
                                       "<table cellspacing='0px' cellpadding='0px'><tr><td colspan='2'>Date Between</td></tr><tr><td>" & _
                                       "From:<input type='text' name='recdate' id='recdate' style='font-size:9pt;' ><br><label class='lblsmall'>select Date</label></td>" & _
                                       "<td>To:<input type='text' name='recdateto' id='recdateto' style='font-size:9pt;' ><br><label class='lblsmall'>select Date</label></td>" & _
                                       "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('fromtorec');" & Chr(34) & ">" & _
                                       "</td></tr><td>select <select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
                                       Response.Write(formmake("row21", cont, 400, 100, "Search By Recruitment Date"))

%>
<script type="text/javascript">
	$(function() {
		$( "#recdate" ).datepicker({
			defaultDate: "+1w",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			
			minDate: "-70Y", maxDate: "-1d"	
		});
		$( "#recdate" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#recdateto" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#recdateto" ).datepicker({
			defaultDate: "+1W",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			maxDate: "1d"
		});
	});
	</script></div>

<div style="clear:both;"><br /></div>
<div id="Div2" style="float:left;">
<%
    cont = "<form id='fromtoproj' method='post' action=''>" & _
                                       "<table cellspacing='0px' cellpadding='0px'><tr><td>Employee's Join the <br>project Starts Between" & _
                                       "</td></tr><tr><td><input type='text' name='projdate' id='projdate' style='font-size:9pt;' ><br><label class='lblsmall' align='middle'>Select Date</label></td>" & _
                                       "<td>and <input type='text' name='projdateto' id='projdateto' style='font-size:9pt;' ><br><label class='lblsmall'>select Date</label></td>" & _
                                       "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('fromtoproj');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select>Workers</td></tr></table></form>"
    Response.Write(formmake("row22", cont, 400, 100, "Search By Project date"))

%>
<script type="text/javascript">
	$(function() {
		$( "#projdate" ).datepicker({
			defaultDate: "+1w",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			minDate: "-70Y", maxDate: "-1d"	
		});
		$( "#projdate" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#projdateto" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#projdateto" ).datepicker({
			defaultDate: "+1W",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			maxDate: "1d"
		});
	});
	</script>
</div>
<div style="width:10px;float:left;">&nbsp;</div>
<div id="Div6" style="float:left;">
<%
    cont = "<form id='proj' method='post' action=''>" & _
                                       "<table cellspacing='0px' cellpadding='0px'><tr><td colspan='2' align='left'>" & _
                                       "Project:<input type='text' id='projx' name='projx'><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label class='lblsmall'>Enter Project Name</label></td></tr><tr><td>" & _
                                       "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('proj');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row25", cont, 400, 100, "Search By Project"))

%>
</div>
<div style="width:10px;float:left;">&nbsp;</div>
<div id="Div4" style="float:left;">
<%
    cont = "<form id='frmdisc' method='post' action=''>" & _
                                       "<table cellspacing='0px' cellpadding='0px'><tr><td>" & _
                                       "<input type='text' id='discipline' name='discipline'><br><label class='lblsmall'>Enter Field of Study</label></td>" & _
                                       "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('disc');" & Chr(34) & ">" & _
                                       "</td></tr><td>select <select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row23", cont, 300, 100, "Search by Field of Study"))

%>
</div>
<div style=" clear:both; height:10px;"></div>
<div id="Div5" style="float:left;">
<%
    cont = "<form id='frmqual' method='post' action=''>" & _
                                       "<table cellspacing='0px' cellpadding='0px'><tr><td>" & _
                                       "<input type='text' id='qualification' name='qualification'><br><label class='lblsmall'>Enter Qualification</label></td>" & _
                                       "<td><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('qual');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row24", cont, 300, 100, "Search By Qualification"))

%>
</div>
<div style=" height:10px; width:5px; float:left;"></div>
<div id="Div7" style="float:left;">
<%
    cont = "<form id='frmpost' method='post' action=''>" & _
                                       "<table cellspacing='0px' cellpadding='0px'><tr><td>" & _
                                       "<input type='text' id='position' name='position'><br><label class='lblsmall'>Enter Qualification</label></td>" & _
                                       "<td><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('post');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row32", cont, 300, 100, "Search By Position"))

%>
</div>
<div style="width:10px; float:left">&nbsp;</div>
<div id="Div3" style="float:left;"></div><div style="clear:both;"></div>
</div>
    <form id="form1" name="form1" method="post" action="">
    <div>
    
    </div>
    </form>
 <a href="viewreport1.aspx?val=all" target="frm_tar">View All</a>
</body>
</html>
<script type="text/javascript">
$(document).ready(function() {

$("#state").autocomplete({
   
});
 //$('#row11').css({top:'0px',left:'0px',width:'300px',height:'150px',background:'#ffffff',border:'1px solid blue'});
showonly("row11");
showonly("row12");

 showonly("row25");
 showonly("row21");
 showonly("row22");showonly("row23");showonly("row24");
 showonly("row32");
   
 $( "#vname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  namelist, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		
		$( "#department" ).autocomplete({
			source: deplist	
						
		});
		$( "#proj" ).autocomplete({
			source: proj,	
			options:
			{
			    color:'blue'
			}
		});
		$( "#projx" ).autocomplete({
			source: proj,	
			options:
			{
			    color:'blue'
			}
		});
		   $( "#discipline" ).autocomplete({
			source: disc,	
			options:
			{
			    color:'blue'
			}
		});   
		 $( "#qualification" ).autocomplete({
			source: qual,	
			options:
			{
			    color:'blue'
			}
		}); 
		 $( "#position" ).autocomplete({
			source: posx,	
			options:
			{
			    color:'blue'
			}
		});    //   //showobj("row11");
       });
</script>