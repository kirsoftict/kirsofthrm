<%@ Page Language="VB" AutoEventWireup="false" CodeFile="hrmworkeasy.aspx.vb" Inherits="hrmworkeasy" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  If session("con").ToString = "" Then
       
                %>
                   <script type="text/javascript">
                       window.location = "logout.aspx";
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
                $("#form1").attr("action", "dataallview.aspx?sql=<% response.write(sql) %>");
                $("#form1").attr("target", "workarea");
                $("#form1").submit();
    </script>
  <%
      End Select
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
     <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
         <link rel="stylesheet" href="css/bootstrap.css">
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
      var idxp;
      idxp="";
      function searchedit()
      {
        idxp="1";
        searchv("byname");
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
               //alert(str);

               if(idxp=="1")
               {
                $("#frmsbyname").attr("action","empcontener.aspx?task=byname&" + str +"&datatake=name" );
               }
               else{
   $("#frmsbyname").attr("action","dataallview.aspx?task=byname&" + str);
   }
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
        function opensearch()
        { 
            var arr=["row11","row12","row21","row22","row23","row24","row25","row32"];
    if($("#listsel").val()==""){
        $("#listsel").val("byname");
    }
            switch($("#listsel").val())
            {
                case "byname":
            //  showonly("row11");
     $("#row11").css({display:'inline'});
     
     closefrm(arr,"row11");
   hmgt("outcont","row11");    
                break;
                case "rcday":
                 showonly("row21");
     $("#row21").css({display:'inline'});
     
     closefrm(arr,"row21");
      hmgt("outcont","row21");
                break;
                case "dep":
                     showonly("row12");
               
          $("#row12").css({display:'inline'});
     
     closefrm(arr,"row12");
      hmgt("outcont","row12");
                break;
                 case "projday":
                showonly("row22");
     $("#row22").css({display:'inline'});
     
     closefrm(arr,"row22");
      hmgt("outcont","row22");
                break;
                case "pos":
                 showonly("row32");
     $("#row32").css({display:'inline'});
     
     closefrm(arr,"row32");
      hmgt("outcont","row32");
                break;
case "qual":
 showonly("row24");
     $("#row24").css({display:'inline'});
     
     closefrm(arr,"row24");
      hmgt("outcont","row24");
     break;
        case "fld":
showonly("row23");
     $("#row23").css({display:'inline'});
     
     closefrm(arr,"row23");
      hmgt("outcont","row23");
        break; 
        case "project":
        showonly("row25");
     $("#row25").css({display:'inline'});
     
     closefrm(arr,"row25");
      hmgt("outcont","row25");
        break;       
            }
        }
  function closefrm(arr,me)
  {
    for(i=0;i<arr.length;i++)
    {
        if(me!=arr[i])
        {
            $("#" +arr[i]).css({display:'none'});
        }
    }
  }
  function hmgt(obj,obj2)
  {
   // $("#" + obj).css({width:'300',height:100});
   // $("#" + obj).css({width:findw(document.getElementById(obj2))*1.5,height:findh(document.getElementById(obj2))*1.1});
  }
    </script>
</head>
<body>
<div class="container">
<div class="row">
<div style="display:none;" >
 <form id="searchbox" action="">

    <input id="search" type="text">
    <input id="submit" type="button" value="Search" onclick="javascript:searchv('alls');">
</form>
</div>
</div>

<div class="row">
<div class='col-lg-3'>
<form id='search' name='search' method="post" action=''> <table><tr><td>
Search By: <select id='listsel' name='listsel' onchange="javascript:opensearch();">
<option value='byname'>Name</option>
<option value='project'>Project</option>
<option value='dep'>Department</option>
<option value='rcday'>Recruitment Date</option>
<option value='projday' title="join to the project">Project Date</option>
<option value='qual' title="">Qualification</option>
<option value='pos' title="">Position</option>
<option value='fld' title="">Field of Study</option>
</select></td></tr></table></form></div>


<div id='outcont' class="col-sm-7" style="border:1px red solid">


<% 
    Dim cont As String = ""
    cont = "<form id='frmsbyname' method='post' action=''>" & _
    "<table class=table'><tr><td>" & _
    "<input type='text' name='vname' id='vname' style='font-size:9pt;' ><br><label class='lblsmall'>write name,father name or grand father</label></td>" & _
    "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "javascript:searchv('byname');" & Chr(34) & "><input type='button' class='searchx' id='searcheditx' value='Edit' name='searcheditx' onclick=" & Chr(34) & "javascript:searchedit();" & Chr(34) & ">" & _
    "</td></tr></table></form>"
    Response.Write(formmake("row11", cont, 300, 100, "Search By Name"))

%>


<% 
    'Dim cont As String
    cont = "<form id='frmdep' method='post' action=''>" & _
    "<table class='table'><tr><td>" & _
    "<input type='text' name='department' id='department' style='font-size:9pt;' ><br><label class='lblsmall'>write Department name</label></td>" & _
    "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('department');" & Chr(34) & ">" & _
    "</td></tr><td>Selct <select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row12", cont, 300, 100, "Search By Department"))

%><% 
    'Dim cont As String
      cont = "<form id='fromtorec' method='post' action=''>" & _
      "<table class='table'><tr><td colspan='2'>Date Between</td></tr><tr><td>" & _
      "From:<input type='text' name='recdate' id='recdate' style='font-size:9pt;' ><br><label class='lblsmall'>select Date</label></td>" & _
      "<td>To:<input type='text' name='recdateto' id='recdateto' style='font-size:9pt;' ><br><label class='lblsmall'>select Date</label></td>" & _
      "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('fromtorec');" & Chr(34) & ">" & _
      "</td></tr><td>select <select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
                                       Response.Write(formmake("row21", cont, 400, 100, "Search By Recruitment Date"))

%>
<script type="text/javascript">
    $(function () {
        $("#recdate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 2,

            minDate: "-70Y", maxDate: "-1d"
        });
        $("#recdate").datepicker("option", "dateFormat", "mm/dd/yy");
        $("#recdateto").datepicker("option", "dateFormat", "mm/dd/yy");
        $("#recdateto").datepicker({
            defaultDate: "+1W",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 2,
            maxDate: "1d"
        });
    });
	</script>


<%
    cont = "<form id='fromtoproj' method='post' action=''>" & _
                                       "<table class='table'><tr><td>Employee's Join the <br>project Starts Between" & _
                                       "</td></tr><tr><td><input type='text' name='projdate' id='projdate' style='font-size:9pt;' ><br><label class='lblsmall' align='middle'>Select Date</label></td>" & _
                                       "<td>and <input type='text' name='projdateto' id='projdateto' style='font-size:9pt;' ><br><label class='lblsmall'>select Date</label></td>" & _
                                       "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('fromtoproj');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select>Workers</td></tr></table></form>"
    Response.Write(formmake("row22", cont, 400, 100, "Search By Project date"))

%>
<script type="text/javascript">
    $(function () {
        $("#projdate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 2,
            minDate: "-70Y", maxDate: "-1d"
        });
        $("#projdate").datepicker("option", "dateFormat", "mm/dd/yy");
        $("#projdateto").datepicker("option", "dateFormat", "mm/dd/yy");
        $("#projdateto").datepicker({
            defaultDate: "+1W",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 2,
            maxDate: "1d"
        });
    });
	</script>

<%
    cont = "<form id='proj' method='post' action=''>" & _
                                       "<table  class='table'><tr><td colspan='2' align='left'>" & _
                                       "Project:<input type='text' id='projx' name='projx'><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label class='lblsmall'>Enter Project Name</label></td></tr><tr><td>" & _
                                       "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('proj');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row25", cont, 400, 100, "Search By Project"))

%>

<%
    cont = "<form id='frmdisc' method='post' action=''>" & _
                                       "<table  class='table'><tr><td>" & _
                                       "<input type='text' id='discipline' name='discipline'><br><label class='lblsmall'>Enter Field of Study</label></td>" & _
                                       "<td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('disc');" & Chr(34) & ">" & _
                                       "</td></tr><td>select <select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row23", cont, 300, 100, "Search by Field of Study"))

%>

<%
    cont = "<form id='frmqual' method='post' action=''>" & _
                                       "<table  class='table'><tr><td>" & _
                                       "<input type='text' id='qualification' name='qualification'><br><label class='lblsmall'>Enter Qualification</label></td>" & _
                                       "<td><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('qual');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row24", cont, 300, 100, "Search By Qualification"))

%>

<%
    cont = "<form id='frmpost' method='post' action=''>" & _
                                       "<table  class='table'><tr><td>" & _
                                       "<input type='text' id='position' name='position'><br><label class='lblsmall'>Enter Position</label></td>" & _
                                       "<td><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick=" & Chr(34) & "searchv('post');" & Chr(34) & ">" & _
                                       "</td></tr><td>Select:<select id='active' name='active'><option value='y'>Active only</option><option value='n'>Not active</option><option value=''>all</option></select></td></tr></table></form>"
    Response.Write(formmake("row32", cont, 300, 100, "Search By Position"))

%>


</div>
<div id='butn' class="col-sm-2" style="border:1px blue solid"> <a href="viewreport1.aspx?val=all" target="frm_tar" class="btn-block">View All</a></div>
</div>

</div>


    <form id="form1" name="form1" method="post" action="">
   
    </form>

</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        opensearch();
        $("#state").autocomplete({

    });
    //$('#row11').css({top:'0px',left:'0px',width:'300px',height:'150px',background:'#ffffff',border:'1px solid blue'});
    //showonly("row11");
    // showonly("row12");

    //showonly("row25");
    // showonly("row21");
    // showonly("row22"); showonly("row23"); showonly("row24");
    //showonly("row32");

    $("#vname").autocomplete({
        source: function (req, response) {
            var re = $.ui.autocomplete.escapeRegex(req.term);
            var matcher = new RegExp("^" + re, "i");
            response($.grep(namelist, function (item) {
                return matcher.test(item);
            }));
        }
    });

    $("#department").autocomplete({
        source: deplist

    });
    $("#proj").autocomplete({
        source: proj,
        options:
			{
			    color: 'blue'
			}
    });
    $("#projx").autocomplete({
        source: proj,
        options:
			{
			    color: 'blue'
			}
    });
    $("#discipline").autocomplete({
        source: disc,
        options:
			{
			    color: 'blue'
			}
    });
    $("#qualification").autocomplete({
        source: qual,
        options:
			{
			    color: 'blue'
			}
    });
    $("#position").autocomplete({
        source: posx,
        options:
			{
			    color: 'blue'
			}
    });    //   //showobj("row11");
});

</script>