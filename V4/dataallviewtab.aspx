<%@ Page Language="VB" AutoEventWireup="false" CodeFile="dataallviewtab.aspx.vb" Inherits="dataallviewtab" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import Namespace="System.Char" %>
<%
   
Dim reqdata As String
    reqdata = Request.QueryString("name")
    
 %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
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
    <link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.accordion.js"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    $(function () {
        $("#tabx").tabs();
    });
    function showHideSubMenu(link) {

        var uldisplay;
        var newClass;

        if (link.className == 'expanded') {

            // Need to hide
            uldisplay = 'none';
            newClass = 'collapsed';

        } else {

            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
        }

        var parent = link.parentNode;
        uls = parent.getElementsByTagName('ul');
        for (var i = 0; i < uls.length; i++) {
            ul = uls[i].style.display = uldisplay;
        }

        link.className = newClass;
    }

    tableDisplayStyle = "table";
    
    
</script>
 <style type="text/css">
 ul.error_list {
        color: #ff0000;
    }

    :disabled:not([type="image"]) {
        background-color:#FFFFFF;
        color:#444444;
    }

    /*input[type=text] {
        border-top: 0px;
        border-left: 0px;
        border-right: 0px;
        border-bottom: 1px solid #888888;
    }*/

    table.historyTable th {
        border-width: 0px;
        padding: 3px 3px 3px 5px;
        text-align: left;
    }
    table.historyTable td {
        border-width: 0px;
        padding: 3px 3px 3px 5px;
        text-align: left;
    }

    .locationDeleteChkBox {
        padding:2px 4px 2px 4px;
        border-style: solid;
        border-width: thin;
        display:block;
    }

    .pimpanel {
        position:absolute;
        left:-9999px;
    }
   
    #empname {
        display:block;
        color: black;
    }

    #personalIcons,
    #employmentIcons,
    #qualificationIcons {
        display:block;
        position:absolute;
        left:-999px;
        width:400px;
        text-align:center;
        padding-left:100px;
        padding-right:100px;
    }

   
    #pimleftmenu {
        display:block;
        float: left;
       /* background: #FFFBED;*/
        padding: 2px 2px 2px 2px;
        margin: 0px 0px 0px 5px;
    }
    #pimleftmenu ul {
        list-style-type: none;
        padding-left: 0;
        margin-left: 0;
        width: 1024px;
        
    }

    #pimleftmenu ul.pimleftmenu li {
        list-style-type:none;
        margin-left: 0;
        margin-bottom: 1px;
       
    }

    #pimleftmenu ul li.parent {
        padding-left: 0px;
        padding-top:4px;
        font-weight: bold;
        text-decoration:none;
       
    }

    #pimleftmenu ul.pimleftmenu li a {
        display:block;
        outline: 0;
        padding: 2px 2px 2px 4px;
        text-decoration: none;
        background:#446699 none repeat scroll 0 0;
        border-color:#CD85ff #8B5Aff #8B5Aff #CD85ff;
        border-style:solid;
        border-width:1px;
        color:#ffffff;
        font-size: 14pt;
        font-weight:bold;
        text-align: left;
    }
    #pimleftmenu ul.pimleftmenu li a:hover {
        color: #ffffff;
        background-color: #122468;
    }

    #pimleftmenu ul.pimleftmenu li a.current {
        color: #ffffff;
        background-color: #446699;
    }
#pimleftmenu td
{
     font-weight:normal; text-decoration:none;
}
#pimleftmenu label
{
     font-weight:bold; text-decoration:none;
}
    #pimleftmenu ul.pimleftmenu li a.collapsed,
    #pimleftmenu ul.pimleftmenu li a.expanded {
        display:block;
        outline: 0;
        padding: 2px 2px 2px 4px;
        text-decoration: none;
        border: 0 ;
        color: #ffffff;
        font-size: 14pt;
        font-weight:bold;
        text-align: left;
        text-decoration:none;
        
        
    }

    #pimleftmenu ul.pimleftmenu li span.expanded {
        background: #446699 url(images/gif/expanded.gif) no-repeat center right;
        color:#ffffff;
        text-decoration:none;
        cursor:pointer;
        text-decoration:none;
    }

    #pimleftmenu ul.pimleftmenu li span.collapsed {
        background: #4466aa url(images/gif/collapsed.gif) no-repeat center right;
        border-bottom: 1px solid #d87415;
        color:ffffff;
        text-decoration:none;
        cursor:pointer;
        
    }

    #pimleftmenu ul.pimleftmenu li a.collapsed:hover span,
    #pimleftmenu ul.pimleftmenu li a.expanded:hover span {
        color: #cccccc;
        
    }


    #pimleftmenu ul span {
        display:block;
        font-size:14pt;
    }
 
 
#pimleftmenu  .datadisplay
{
    font-size:11pt;
    border:1px #234599 solid;
    width:100%;
}
    #pimleftmenu ul span span {
        display:inline;
        text-decoration:underline;
    }

    div.requirednotice {
        margin-left: 15px;
    }

    #parentPaneDependents {
        float:left;
        width: 50%;
    }

    #parentPaneChildren {
        float:left;
        width: 50%;
    }

    /** Job */
    h3#locationTitle, table#assignedLocationsTable {
        margin-left:10px;
    }

    #jobSpecDuties {
        width:400px;
    }

    /** Dependents */
    div#addPaneDependents {
        width:100%;
    }

    div#addPaneDependents label {
        width: 100px;
    }

    div#addPaneDependents br {
        clear:left;
    }

    div#addPaneDependents input {
        display:block;
        margin: 2px 2px 2px 2px;
        float:left;
    }

    div.formbuttons {
        text-align:left;
    }

    input.hiddenField {
        display:none;
    }

    /* Children */
    div#addPaneChildren {
        width:100%;
    }

    div#addPaneChildren label {
        width: 100px;
    }

    div#addPaneChildren br {
        clear:left;
    }

    div#addPaneChildren input {
        display:block;
        margin: 2px 2px 2px 2px;
        float:left;
    }

    /* education */
    div#editPaneEducation {
        width:100%;
    }

    div#editPaneEducation label {
        width: 200px;
    }

    div#editPaneEducation br {
        clear:left;
    }

    div#editPaneEducation input {
        display:block;
        margin: 2px 2px 2px 2px;
        float:left;
    }

    div#editPaneEducation #educationLabel {
        display:inline;
        font-weight:bold;
        padding-left:2px;
    }

    div.formbuttons {
        text-align:left;
    }

    /* membership */
    label#membershipLabel,
    label#membershipTypeLabel {
        font-weight:bold;
    }

    div#editPaneMemberships {
        width:100%;
    }
.parent
{
text-decoration:none;
}

#wexp
{
	width:1000px;
	font-size:10pt;
	}
    #tblall
    {
    	width:800px;
    	font-size:10pt;
    	}
    </style>
    <script type="text/javascript" language="javascript">
        function print(loc, title, head, footer) {
            var printFriendly = document.getElementById(loc)
            var printWin = window.open("about:blank", title, "menubar=no;status=no;toolbar=no;");
            printWin.document.write("<html><head><title></title></head><body style='font-size:12pt;'><h1 style='font-size:15pt; color:blue;'>" + head + "</h1>" + printFriendly.innerHTML + "<label class='smalllbl'>" + footer + "</label></body></html>");
            printWin.document.close();
            printWin.window.print();
            printWin.close();
        }
   
    </script>
	</head>



    
<body style="height:auto;">
<div id='helpx'></div>
 <%  Dim fl As New file_list
     Dim fm As New formMaker
     Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

<!--!a href="dataallview-Copy.aspx?empid=<%response.write(Request.QueryString("empid")) %>">CV</a-->
<form id='frmx' name='frmx' method="post"></form>
  
<%Dim sql As String = ""
    Dim dbs As New dbclass
    Dim dt As DataTableReader
    Dim i As Integer = 0
    Dim fullname, emp_id, emptid, sex, dob, nationality, imglink, emptin, mstatus, position, dept, proj, status As String
    Dim htel, wtel, mob, pemail, postal, subcity, worda, kebele, hno As String
    Dim arrname() As String
    position = ""
    Dim strout As String = ""
    strout = "<script type='text/javascript'>"
    If Request.QueryString("empid") = "" Then
        If Request.QueryString("vname") <> "" Then
            Dim name As String = ""
            name = Request.QueryString("vname").Trim
           
            arrname = name.Split(" ")
          
            
            If arrname.Length >= 3 Then
                sql = "Select * from emp_static_info where first_name='" & arrname(0).Trim & "' and middle_name='" & arrname(1).Trim & "' and last_name='" & arrname(2).Trim & "'"
               
            End If
        End If
            
    Else
        sql = "Select * from emp_static_info where emp_id='" & Request.QueryString("empid") & "'"
        
    End If
    If sql <> "" Then
        dt = dbs.dtmake("tblstatic", sql, Session("con"))
    End If
    
    If dt.HasRows Then
        dt.Read()
        'emp_id = dt.Item("emp_id")
        For k As Integer = 0 To dt.FieldCount - 1
            If dt.IsDBNull(k) = False Then
                If dt.GetName(k) <> "imglink" Then
                    strout &= "$('#" & dt.GetName(k) & "').css({'font-size':'11pt'});" & Chr(13)
                    strout &= "$('#" & dt.GetName(k) & "').text('" & dt.Item(k) & "');" & Chr(13)
                         
                      
                Else

                    If dt.Item("imglink") <> "" Then
                        Dim xstr As String = dt.Item("imglink")
                        If xstr.Length < 7 Then
                            strout &= "$('#" & dt.GetName(k) & "').attr('src','http://localhost:8002/images/gif/default_employee_image.gif');" & Chr(13)
                        Else
                            strout &= "$('#" & dt.GetName(k) & "').attr('src','" & dt.Item(k) & "');" & Chr(13)
                        End If
                    Else
                        strout &= "$('#" & dt.GetName(k) & "').attr('src','" & dt.Item(k) & "');" & Chr(13)

                    End If
                End If '
                            
            Else
                If dt.GetName(k) <> "imglink" Then
                    strout &= "$('#" & dt.GetName(k) & "').text('-');" & Chr(13)
                Else
                    strout &= "document.getElementById(" & dt.GetName(k) & ").src='" & Request.ServerVariables("HOST") & "/images/gif/default_employee_image.gif';" & Chr(13)
                    ' strout &= "$('#" & dt.GetName(k) & "').css('cursor','pointer');" & Chr(13)

                End If
            End If
        Next
               
        Dim mk As New formMaker
        Dim row As String = ""
        Dim projname, projid As String
        emp_id = dt.Item("emp_id")
        fullname = mk.getfullname(emp_id, Session("con"))
        dt.Close()
        Dim dptname As String = ""
        Dim emptdate As Date
        Dim resdate As Date
        projid = ""
        dt = dbs.dtmake("emprec his", "select * from emprec where emp_id='" & emp_id & "' order by id desc ", Session("con"))
        If dt.HasRows Then
            dt.Read()
            
            emptdate = dt.Item("hire_date")
            If dt.IsDBNull(4) = False Then
                resdate = dt.Item("end_date")
            End If
            emptid = dt.Item("id")
            dt.Close()
            sql = "Select ass_for as 'Assignment Type',position as 'Position',department as 'Department',date_from as 'Start Date',date_end as 'Last Date',project_id as project from emp_job_assign where emptid=" & emptid & " order by id desc"
            dt = dbs.dtmake("jobassign", sql, Session("con"))
            
            If dt.HasRows Then
                dt.Read()
                dptname = dbs.getdepname(dt.Item("department"), Session("con"))
                position = dt.Item("position").ToString
                projid = dt.Item("project").ToString
            End If
            
            
        End If
        dt.Close()
        If projid <> "" Then
            projname = mk.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
        End If
        
        
        dt = dbs.dtmake("emp_address", "select * from emp_address where emp_id='" & emp_id & "' order by id desc", Session("con"))
        If dt.HasRows Then
            dt.Read()
            For i = 2 To dt.FieldCount - 4
                If dt.IsDBNull(i) = False Then
                    Session(dt.GetName(i).ToString) = dt.Item(i)
                Else
                    Session(dt.GetName(i).ToString) = "-"
                End If
            Next
            
            
        End If
        dt.Close()
        %>
       
                        <div id="tabx">
                            <ul>
                            <li><a href="#">1</a></li>
                            <li><a href="#">1</a></li>
                            <li><a href="#">1</a></li>
                            <li><a href="#">1</a></li>
                            </ul>
                        </div>
                       
                        <div id="personalinfo"  class="datadisplay">
                            <table style="width: 595px;">
                                <tr>  
                                <td colspan="3" rowspan="" style="WIDTH: 499px">             
                                        <table style="width: 494px;font-size:12pt;" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td rowspan="" style="width: 250px;">
                                                   <label style=" font-weight:bold"> Full Name</label></td>
                                                <td rowspan="" style="width: 18px;">
                                                    :</td>
                                                <td id="fullname" rowspan="" style="width: 679px; font-weight:normal; text-decoration:none;">
                                                    </td>
                                                <td rowspan="" style="width: 127px;">
                                                  <label style="font-weight:bold">Nationality</label> </td>
                                                <td rowspan="" style="width: 20px;  ">
                                                    :</td>
                                                <td rowspan="" style="width: 358px; ">
                                                   <span id='nationality'></span></td>
                                            </tr>
                                          </table>
                                    </td>
                                    <td style="width: 120px" rowspan="2">
                                        <img id="imglink" src="" style="vertical-align:top; width:120px;" alt="img" />
                                     </td>
                                     <td rowspan="7" style="width:400px; vertical-align:top;">
                                        <table style=" width:350px;"><tr><th colspan="3"></th></tr>
                                        <tr>
                                        <td style="width:100px"><label style="font-weight: bold">Mob</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(Session("mob"))%></td>
                                        </tr>
                                        <tr>
                                        <td><label style="font-weight:bold">Home Tel</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(Session("htel"))%></td>
                                        </tr>
                                         <tr>
                                        <td><label style="font-weight:bold">Work Tel</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(Session("wtel"))%></td>
                                        </tr>
                                         <tr>
                                        <td><label style="font-weight:bold">e-mail</label></td><td style="width:10px">:</td>
                                        <td style="width:200px"><a href="mailto:<%response.write(session("pemail")) %>" target="_blank" style=" clear:both; background-color:White;font-size:12pt; border:none; color:blue; font-weight:normal; font-style:italic; text-decoration:underline "><% Response.Write(Session("pemail"))%></a></td>
                                        </tr>
                                        </table>
                                     </td>
                                </tr>
                                <tr>
                                    <td style="width: 70px; height: 10px;" rowspan="">
                                        <table cellpadding="0" cellspacing="0" style="font-size:12px; width: 250px; height: 47px;">
                                            <tr>
                                                <td style="width:71px;font-size:12pt;">
                                                    <label style="font-weight:bold">Emp ID</label></td>
                                                <td style="width: 10px">
                                                    :</td>
                                                <td style="width: 80px">
                                                    <span id='emp_id'></span></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 71px">
                                                   <label style="font-weight:bold"> <span style="font-size: 12pt">Sex</span></label></td>
                                                <td style="width: 10px">
                                                    :</td>
                                                <td style="width: 80px">
                                                    <span id='sex'></span></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 71px;">
                                                    <label style="font-weight:bold"><span style="font-size: 12pt">
                                                    Birth Date</span></label></td>
                                                <td style="width: 10px; ">
                                                    :</td>
                                                <td style="width: 80px; ">
                                                    <span id='dob'></span></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 53px;" rowspan="1">
                                        <table cellpadding="0" cellspacing="0" style="font-size:12px; width: 242px;">
                                            <tr>
                                                <td style="width: 424px">
                                                    <label style="font-weight:bold"><span style="font-size: 12pt">Marital status</span></label></td>
                                                <td style="width: 57px">
                                                    :</td>
                                                <td style="width: 593px">
                                                    <span id='marital_status'></span></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 424px">
                                                   <label style="font-weight:bold"> <span style="font-size: 12pt">Tin</span></label></td>
                                                <td style="width: 57px">
                                                    :</td>
                                                <td style="width: 593px">
                                                   <span id='emp_tin'></span></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                            </table>  
                            <table style="width: 419px;">
                                <tr>
                                    <td colspan="">
                                        <strong>
                                        Current Status:</strong></td>
                                        <td style="width: 13px">:</td>
                                        <td style="width: 300px">
                                        <span id="cs"></span></td>
                                </tr>
                                <tr>
                                    <td style="width: 180px">
                                        <strong>
                                        Department</strong></td>
                                    <td style="width: 13px">
                                        :</td>
                                    <td id='empdep' style="width: 300px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px">
                                        <strong>
                                        Project</strong></td>
                                    <td style="width: 13px">
                                        :</td>
                                    <td id="empproj" style="width: 300px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px">
                                        <strong>
                                        Position</strong></td>
                                    <td style="width: 13px">
                                        :</td>
                                    <td id="emppost" style="width: 300px">
                                    </td>
                                </tr>
                            </table>
                           
                           </div>
                        	
                        
                    
                        <div id="workexp" style="">
                          <% 
                            
                              Dim sqlx As String = "select id,Organization,from_d,to_d,position,rec_cond,job_description,last_salary,cur from tblworkexp where emp_id='" & emp_id & "' order by id desc"
        
                              row = mk.tableview_wexp("tblworkexp", sqlx, "Organization,From,Upto,Postition,Work as,Job Description,Last Salary,Currency", Session("con"), "")
        
                           
        Response.Write(row)
        %>
                        </div>
                           
                        <div id="edu" style="">
                          <% 
                              row = ""
                              sqlx = ""
                              sqlx = "select id,Qualification,diciplin,school,country,year_g,score from emp_education where emp_id='" & emp_id & "' order by id desc"
                              row = mk.tableview("emp_education", sqlx, "Qualification,Field of Study,School/College/University,Country,Year of Graduation,Score", Session("con"), "")
        Response.Write(row)
        %>
                        </div>
                            
                        <div id="Skill" style="">
                          <% 
                              row = ""
                              sqlx = ""
                              sqlx = "select id,Skilles from empskill where emp_id='" & emp_id & "' order by id desc"
                              row = mk.tableview("empskill", sqlx, "Skills", session("con"), "")
        Response.Write(row)
        %>
                        </div>
                           
                        <div id="ref" style="">
                          <% 
                              row = ""
                              sqlx = ""
                              sqlx = "select id,refname,position,email,mob,tel from empreferance where emp_id='" & emp_id & "' order by id desc"
                              row = mk.tableview("empreferance", sqlx, "Ref. Name, Position, Email, Mob, Tel", session("con"), "")
        Response.Write(row)
        %>
                        </div>
                           
                        <div id="emphist" style="">
                          <br />
                          <%  Dim color As String
                              dt = dbs.dtmake("fselc", "select * from emprec where end_date is null and  emp_id='" & emp_id & "'", Session("con"))
                              If dt.HasRows Then%>
                                    <table style=" width:750px;"><tr><th colspan="3"></th></tr>
                                        <tr>
                                        <td style="width:100px"><label style="font-weight: bold">Hire Date</label></td>
                                        <td style="width:10px">:</td>
                                        <td><% Response.Write(emptdate.ToShortDateString)%></td>
                                        
                                        <td><label style="font-weight:bold">Employment id</label></td><td style="width:10px">:</td>
                                        <td><%  If emptid <> "" Then
                                                    Response.Write(emptid.ToString)
                                                End If
                                                %></td>
                                        </tr>
                                         <tr>
                                        <td><label style="font-weight:bold">Current Position</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(position)%></td>
                                        
                                        <td><label style="font-weight:bold">Department</label></td><td style="width:10px">:</td>
                                        <td style="width:200px"><label style=" font-weight:bold;"><%Response.Write(dptname)%></label></td>
                                        </tr>
                                         <tr>
                                        <td><label style="font-weight:bold">Project</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(projname)%></td>
                                                                               </tr>
                                        </table>
                                          <% 
                                          Else
                                              dt.Close()
                                              dt = dbs.dtmake("fselc", "select * from emprec where emp_id='" & emp_id & "' order by hire_date desc", Session("con"))
                                              If dt.HasRows Then
                                                  dt.Read()
                                                  emptid = dt.Item("id")
                                                  %>
                                                   <table style=" width:750px;"><tr><th colspan="3"></th></tr>
                                        <tr>
                                        <td style="width:100px"><label style="font-weight: bold">Hire Date</label></td>
                                        <td style="width:10px">:</td>
                                        <td><% Response.Write(emptdate.ToShortDateString)%></td>
                                        
                                        <td><label style="font-weight:bold">Employment id</label></td><td style="width:10px">:</td>
                                        <td><%  If dt.Item("id").ToString <> "" Then
                                                    Response.Write("Last computer id:" & dt.Item("id").ToString)
                                                End If
                                                %></td>
                                        </tr>
                                         <tr>
                                        <td><label style="font-weight:bold">Current Position</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(position)%></td>
                                        
                                        <td><label style="font-weight:bold">Department</label></td><td style="width:10px">:</td>
                                        <td style="width:200px"><label style=" font-weight:bold;"><%Response.Write(dptname)%></label></td>
                                        </tr>
                                         <tr>
                                        <td><label style="font-weight:bold">Project</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(projname)%></td>
                                                                               </tr>
                                        </table>
                                          <% 
                                              End If
                                              Response.Write("This Person is x-Staf")
                                          End If%>
                                        <span >Recruitment Information</span>
                                    <%
                              row = ""
                              sqlx = ""
                                        sqlx = "select id,type_recuritment,hire_date,end_date from emprec where emp_id='" & emp_id & "' order by id desc"
                                        row = mk.tableview("emp_job_assign", sqlx, "Type of recruitment,Hire Date, End  Date", session("con"), "")
                                        Dim twexp As Double
                                        ' Dim mt
                                        Dim hdate As Date
                                        hdate = mk.getinfo2("select hire_date from emprec where emp_id='" & emp_id & "' order by id desc", Session("con"))
                                        twexp = Today.Subtract(hdate).Days / 30.4
                                       
                                        If CInt(twexp) > 12 Then
                                          '  expout = Math.Floor(exp / 12).ToString & "Yrs " & CInt(exp Mod 12).ToString & "Ms"
                                        End If
                                        ' Response.Write(twexp.ToString)
                                        Response.Write(row) 'exp in the company
                                        Dim rs As DataTableReader = dbs.dtmake("empr", sqlx, Session("con"))
                                        Dim dds As Integer = 0
                                        If rs.HasRows = True Then
                                            While rs.Read
                                                If rs.IsDBNull(3) = False Then
                                                    dds += CDate(rs.Item("end_date")).Subtract(CDate(rs.Item("hire_date"))).Days
                                                Else
                                                    dds += Now.Subtract(CDate(rs.Item("hire_date"))).Days
                                                    
                                                End If
                                            End While
                                            If dds > 0 Then
                                                Dim exp As Double = 0
                                                exp = dds / 30.41
                                                Response.Write("<span style='font-size:11pt; font-weight:bold;'>Total Experience:" & Math.Floor(exp / 12).ToString & "Yrs and " & (CInt(exp Mod 12)).ToString & "Ms</span>")
                                            End If
                                        End If
                                        rs.Close()
        %>
       
        <br />
                                        <span >Job Assignment Details</span>
                                    <%
                              row = ""
                              sqlx = ""
                                        sqlx = "select id,position,department,project_id,date_from,date_end from emp_job_assign where emp_id='" & emp_id & "' order by id desc"
                                        row = mk.tableview("emp_job_assign", sqlx, "Position, Department,Project,Date From, End Date", Session("con"), "")
        Response.Write(row)
        %>
       
    
                        </div>
                          
                        <div id="salpay" style="">
                                     <%     
                                         
                                         
                                        
                             Dim allowance() As Double
                             Dim allowancestr() As String
                             Dim basicsal() As String
                             Dim allw(,) As String
                                         Dim perdim As String
                                        
                             allw = dbs.getallow(emptid, session("con"))
                                         basicsal = dbs.getsal(emptid, Session("con"))
                                         If IsNumeric(basicsal(0)) = False Then
                                             basicsal(0) = "0"
                                         End If
                                         If basicsal(0).ToString <> "" Then
                                             If IsNumber(basicsal(0)(0)) = True Then
                                                 basicsal(0) = dbs.float(CDbl(basicsal(0)), 2)
                                             Else
                                                 Response.Write(basicsal(0))
                                                 'Response.Redirect(basicsal(1) & "?vew=xxx")
                                             End If
                                            
                                             
                                         End If
                             
                                         perdim = dbs.getperdim(emptid, session("con")).ToString
                                         perdim = Math.Round(Decimal.Add(CDbl(perdim), 0.00001), 2).ToString
                                                        
                                 %>
                                 <table style=" width:750px;"><tr><th colspan="3"></th></tr>
                                        <tr>
                                        <td style="width:150px"><label style="font-weight: bold">
                                        Current Basic Salary</label></td>
                                        <td style="width:10px">:</td>
                                        <td><% Response.Write(FormatNumber(basicsal(0), 2, TriState.True, TriState.True, TriState.True) & " " & basicsal(1))%>
                                        </td>
                                             <td><label style="font-weight:bold">Per-dime</label></td><td style="width:10px">:</td>
                                        <td><% Response.Write(perdim)%></td>
                                        </tr>
                                         <tr>
                                        <td><label style="font-weight:bold">Allowances</label></td><td style="width:10px">:</td></tr><tr>
                                        <td colspan="6">
                                        <%  sqlx = "select id,allownace_type,amount,istaxable,from_date from emp_alloance_rec where emptid=" & emptid & " and to_date is null order by id desc,istaxable"
                                            
                                            row = mk.tableview("emp_alloance_rec", sqlx, "Allowance Type,Amount,Taxable,Date From", session("con"), "")
                                            Response.Write(row)
                                            'Response.Write(allw.Length.ToString & "<br>")
                                            'Response.Write(UBound(allw).ToString & "<br>")
                                            'Response.Write((allw.Length / UBound(allw)).ToString)
                                            ' For i = 0 To UBound(allw)
                                                
                                            ' Response.Write(allw(i, 0).ToString & "<br>")
                                                
                                            ' Next
                                            
                                            %>
                                         </td></tr>
                                         <tr><td colspan="6"><label style=" font-weight:bold">OverTime</label></td></tr>
                                         <tr><td colspan="6">
                                         <%  row = ""
                                             sqlx = "select id,ot_date,datepaid,time_diff,rate,factored,amt from emp_ot where emptid=" & emptid & " order by id desc"
                                             If emptid <> "" Then
                                                 row = mk.tableview("emp_ot", sqlx, "OT Date,Paid Date,Different,Rate,Overtime Hours,Amount", Session("con"), "").ToString
                                                 Response.Write(row)
                                             End If
                                          %>
                                         
                                         </td></tr>
                                        
                                        </table>
                                        <%
                                            row = ""
                                            sqlx = "select id,basic_salary,date_start,date_end,emp_id,emptid from emp_sal_info where emp_id='" & emp_id & "' order by id desc"
                                            row = mk.tableviewsal("Salary Information", sqlx, "Basic Salary,Action Date,End Date,Project", Session("con"), "")
                                            Response.Write(row & "<br>")
                                            row = ""
                                            ' sqlx = "select id,pay_date,gross_earn,tax,pension,pension_co,tdedact,netpay,emptid from paryrol where emptid in(select id from emprec where emp_id='" & emp_id & "') order by id desc"
                                            ' row = mk.tableviewsalpaid("Paid Salary Information", sqlx, "Pay Date,Gross Earning,Tax,Pension Contribution,Pension C.co.,Total Deduction,Net Pay,Computer Emp.No.", Session("con"), "")
                                            sqlx = "select id,date_paid,pddate,b_sal,b_e,talw,alw,ot,gross_earnings,txinco,tax,pen_e,pen_c,netp,ref from payrollx where emptid in(select id from emprec where emp_id='" & emp_id & "') and (ref_inc is null or ref_inc='0') order by date_paid desc"
                                            row = mk.tableviewsalpaidx("Paid Salary Information", sqlx, "Payroll Date,paid Date,Basic Salary,Earnings,Taxable Allowance,Allowance,OT,Gross Earning,Taxable income,Tax,Pension Contribution,Pension C.co.,Net Pay,Payroll Ref.", Session("con"), "")
                                         
                                            Response.Write(row & "<br>")
                                            sqlx = "select id,date_paid,pddate,b_sal,b_e,talw,alw,ot,gross_earnings,txinco,tax,pen_e,pen_c,netp,ref from payrollx where emptid in(select id from emprec where emp_id='" & emp_id & "') and (ref_inc is Not null and ref_inc<>'0') order by date_paid desc"
                                            row = mk.tableviewsalpaidx("Paid Salary Information", sqlx, "Payroll Date,Pay Date,Basic Salary,Earnings,Taxable Allowance,Allowance,OT,Gross Earning,Taxable income,Tax,Pension Contribution,Pension C.co.,Net Pay,Payroll Ref.", Session("con"), "")
                                            Response.Write("Increament and other Payments<br>")
                                            Response.Write(row & "<br>")
                                            ' End If
                             
                         
        %>
                        </div>
                          
                        <div id="divloan" style="">
                         <%    
                             Response.Write(fm.loan(emptid, Session("con")))
        %>
                        </div>
                           
                        <div id="divleave" style="">
                         <%  
                          
                             'Dim emptid As Integer
                             '  Response.Write(emptid)
                             Response.Write(fm.getleaveinfo(emptid, Session("con")))
                             Dim fx() As String
                             fx = fm.rightmk(Session("right"))
                             If fm.searcharray(fx, "1") Or fm.searcharray(fx, "2") Or fm.searcharray(fx, "3") Then%><br />
                        <div><div style="background-color:Blue;"> Un-used leave Balance Settlement</div> 
                         <%
                             Response.Write(fm.getleaveinfobal(emptid, Session("con")))
        %>
                       </div><%End If%> </div>
                           
                        <div id="pardim" style="">
                         <%  
                          
                             'Dim emptid As Integer
                           
                             row = ""
                             sqlx = "select id,pardime,from_date,to_date,emp_id,emptid from emp_pardime where emptid='" & emptid & "' order by id desc"
                             row = mk.tableviewpar("Perdiem Rate", sqlx, "Perdiem,From_date,To-Date,Project", Session("con"), "")
                             Response.Write(row & "<br>")
                             row = ""
                             sqlx = "select id,pardim,reason,no_days,from_date,to_date,(no_days*pardim) as Total,paid_state from pardimpay where emptid='" & emptid & "' order by paid_date desc,paid_state"
                             If emptid <> "" Then
                                 row = mk.tableview("pardimpay", sqlx, "Perdiem Rate,Reason,No. Days,Date From, Date End,Total,Paid", Session("con"), "").ToString
                                 Response.Write(row)
                             End If
                                          
                            
                            
        %>
                        </div>
                      <%              
     
        ' Response.Write(dbs.isactive("select * from emprec where emp_id='" & emp_id & "' order by id desc", session("con")).ToString)
             strout &= "$('#fullname').text('" & fullname & "');"
             If dbs.isactive("select * from emprec where emp_id='" & emp_id & "' order by id desc", Session("con")) Then
                 strout &= "$('#cs').text('Active');"
                 strout &= "$('#empdep').text('" & dptname & "');"
                 strout &= "$('#empproj').text('" & projname & "');"
                 strout &= "$('#emppost').text('" & position & "');"
             Else
                 strout &= "$('#cs').text('Inactive');"
                 strout &= "$('#empdep').text('x-" & dptname & "');"
                 strout &= "$('#empproj').text('x-" & projname & "');"
                 strout &= "$('#emppost').text('x-" & position & "');"
             End If
        strout &= "</script>"
        Response.Write(strout)
        End If
             
       
        'response.write(getpersonalinfo(empid)) %>    
       
                                            

</body>
</html>


