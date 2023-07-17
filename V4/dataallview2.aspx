<%@ Page Language="VB" AutoEventWireup="false" CodeFile="dataallview2.aspx.vb" Inherits="dataallview2" %>
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
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.sortable.js"></script>
	<script src="jq/ui/jquery.ui.accordion.js"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
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
        for(var i=0; i<uls.length; i++) {
            ul = uls[i].style.display = uldisplay;
        }

        link.className = newClass;
    }

    tableDisplayStyle = "table";
    
    $(function() {
		$( "#accordion" )
			.accordion({
				header: "> div > h3"
			})
			.sortable({
				axis: "y",
				handle: "h3",
				stop: function( event, ui ) {
					// IE doesn't register the blur when sorting
					// so trigger focusout handlers to remove .ui-state-focus
					ui.item.children( "h3" ).triggerHandler( "focusout" );
				}
			});
	});
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



    </style>
    <script type="text/javascript" language="javascript">
    function print(loc,title,head,footer)
    {
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<html><head><title></title></head><body style='font-size:12pt;'><h1 style='font-size:15pt; color:blue;'>"+head+"</h1>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
    printWin.close();
    }
   
    </script>
	</head>



    
<body style="height:auto;">
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
        If Request.QueryString.HasKeys = True Then
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
        dt = dbs.dtmake("tblstatic", sql, session("con"))
    End If
    
    If dt.HasRows Then
        dt.Read()
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
                    strout &= "document.getElementById(" & dt.GetName(k) & ").src='http://localhost:8002/images/gif/default_employee_image.gif';" & Chr(13)
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
        dt = dbs.dtmake("emprec his", "select * from emprec where emp_id='" & emp_id & "'", session("con"))
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
                dptname = dbs.getdepname(dt.Item("department"), session("con"))
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
       
        <div id="pimleftmenu" style="padding-top:-12px;">
                    <ul class="pimleftmenu" style="padding-top:-12px;">
        <li class="l1 parent">
            <span class="collapsed" onclick="showHideSubMenu(this)" style="">
                <span class="parent" style="color:White; text-decoration:none;">Personal Information</span></span>
            <ul class="l2" style="display:none;">
                <li class="l2">                        
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
                        	
                         <%  Dim outp As String = "<div id=x" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('personalinfo','Report_print','" & Session("company_name") & "<BR> Personal Details','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                        
                       </li>  
                        	</ul>
	
                       
            
        </li>
        </ul>
            <ul class="pimleftmenu">
                <li class="l1 parent">
                <span class="collapsed"  onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Work Experience</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="workexp" style="">
                          <% 
                            
        Dim sqlx As String = "select id,Organization,from_d,to_d,position,last_salary,cur from tblworkexp where emp_id='" & emp_id & "' order by id desc"
        
                              row = mk.tableview_wexp("tblworkexp", sqlx, "Organization,From,Upto,Postition,Last Salary,Currency", Session("con"), "")
        
                           
        Response.Write(row)
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('workexp','Report_print','" & Session("company_name") & "<BR> Employee Work Experience','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                                &nbsp;</div>
                        </li>
                    </ul>
                </li>
            </ul> 
            <ul class="pimleftmenu">
                <li class="l1 parent"><span class="collapsed"  onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Educational Background</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="edu" style="">
                          <% 
                              row = ""
                              sqlx = ""
                              sqlx = "select id,Qualification,diciplin,school,country,year_g,score from emp_education where emp_id='" & emp_id & "' order by id desc"
                              row = mk.tableview("emp_education", sqlx, "Qualification,Field of Study,Institution,Country,Year of Graduation,Score", session("con"), "")
        Response.Write(row)
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('edu','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                </li>
            </ul>
            <ul class="pimleftmenu">
                <li class="l1 parent"><span class="collapsed"  onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Skills</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="Skill" style="">
                          <% 
                              row = ""
                              sqlx = ""
                              sqlx = "select id,Skilles from empskill where emp_id='" & emp_id & "' order by id desc"
                              row = mk.tableview("empskill", sqlx, "Skills", session("con"), "")
        Response.Write(row)
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('skill','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                </li>
            </ul>
            <ul class="pimleftmenu">
                <li class="l1 parent">
                <span class="collapsed" onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Reference</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="ref" style="">
                          <% 
                              row = ""
                              sqlx = ""
                              sqlx = "select id,refname,position,email,mob,tel from empreferance where emp_id='" & emp_id & "' order by id desc"
                              row = mk.tableview("empreferance", sqlx, "Ref. Name, Position, Email, Mob, Tel", Session("con"), "")
        Response.Write(row)
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('ref','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                </li>
            </ul>
            <ul class="pimleftmenu">
                <li class="l1 parent"><span class="collapsed"  onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Employment History</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
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
                                          <% end if %>
                                        <span >Recruitment Information</span>
                                    <%
                              row = ""
                              sqlx = ""
                                        sqlx = "select id,type_recuritment,hire_date,end_date from emprec where emp_id='" & emp_id & "' order by id desc"
                                        row = mk.tableview("emp_job_assign", sqlx, "Type of recruitment,Hire Date, End  Date", session("con"), "")
        Response.Write(row)
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
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('ref','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                </li>
            </ul>
            <ul class="pimleftmenu">
                <li class="l1 parent"><span class="collapsed"  onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Salary and Payroll information</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="salpay" style="">
                                     <%     
                             Dim allowance() As Double
                             Dim allowancestr() As String
                             Dim basicsal() As String
                             Dim allw(,) As String
                                         Dim perdim As String
                                        
                             allw = dbs.getallow(emptid, session("con"))
                                         basicsal = dbs.getsal(emptid, session("con"))
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
                                        <td><% Response.Write(basicsal(0) & " " & basicsal(1))%>
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
                                             sqlx = "select id,ot_date,ot_time,ot_end,time_diff,rate,factored,amt from emp_ot where emptid=" & emptid & " order by id desc"
                                             If emptid <> "" Then
                                                 row = mk.tableview("emp_ot", sqlx, "Date,Time Start, Time End,Different,Rate,Overtime Hours,Amount", Session("con"), "").ToString
                                                 Response.Write(row)
                                             End If
                                          %>
                                         
                                         </td></tr>
                                        
                                        </table>
                                        <%
                                            ' End If
                             
                         
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('ref','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                 
                </li>
            </ul>
            <ul class="pimleftmenu">
                <li class="l1 parent"><span class="collapsed" onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Loan Information</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="divloan" style="">
                         <%    Dim fm As New formMaker
                             Response.Write(fm.loan(emptid, Session("con")))
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('divloan','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                    
                </li>
            </ul>
            <ul class="pimleftmenu">
                <li class="l1 parent"><span class="collapsed" onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Leave Information</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="divleave" style="">
                         <%  
                          
                             'Dim emptid As Integer
                           
                             Response.Write(fm.getleaveinfo2(emptid, Session("con")))
                            
                            
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('divleave','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                    
                </li>
            </ul>
             <ul class="pimleftmenu">
                <li class="l1 parent"><span class="collapsed" onclick="showHideSubMenu(this)">
                    <span class="parent" style="color: White;text-decoration:none;">Perdiem</span></span>
                    <ul class="l2" style="display: none;">
                        <li class="l2">
                        <div id="pardim" style="">
                         <%  
                          
                             'Dim emptid As Integer
                           
                               row = ""
                             sqlx = "select id,pardim,reason,no_days,from_date,to_date,(no_days*pardim) as Total,paid_state from pardimpay where emptid=" & emptid & " order by paid_date desc,paid_state"
                                             If emptid <> "" Then
                                 row = mk.tableview("pardimpay", sqlx, "Par-diem,Reason,No. Days,Date From, Date End,Total,Paid", Session("con"), "").ToString
                                                 Response.Write(row)
                                             End If
                                          
                            
                            
        %>
                        </div>
                            <div>
                                <%  outp = "<div id=y" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('pardim','Report_print','" & Session("company_name") & "<BR> Employee Educational Background','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)%>
                            </div>
                        </li>
                    </ul>
                    
                </li>
            </ul>
            
             </div>
        <div style="clear:both; "></div>

         <%  outp = "<div id=" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:blue;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('pimleftmenu','Report_print','" & Session("company_name") & "<BR> Personal Details','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico'/>print</div>"
                                Response.Write(outp)               
     
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


