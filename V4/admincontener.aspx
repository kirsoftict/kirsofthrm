<%@ Page Language="VB" AutoEventWireup="false" CodeFile="admincontener.aspx.vb" Inherits="admincontener" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.data.sqlclient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Microsoft.VisualBasic" %>
<%@ Import Namespace="kirsoft.hrm" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title></title>
    
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
    .currentpanel {
        margin-top: 10px;
        margin-left: 190px;
    }
    #photodiv {
        margin-top:19px;
        float:left;
        text-align:center;
        margin-left: 650px;
        padding: 2px;
        border: 1px solid #FAD163;
    }
    #photodiv span {
        color: black;
        font-weight: bold;
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

    #icons div a {
        display:block;
        float:left;
        height: 50px;
        width: 54px;
        text-decoration:none;
        text-align:center;
        vertial-align:bottom;
        padding-top: 45px;
        outline: 0;
        background-position: top center;
        margin-left:8px;
        margin-right:8px;
    }

    #icons div a:hover {
        color: black;
        text-decoration: underline;
    }

    #icons div a.current {
        font-weight: bold;
        color:black;
        cursor:default;
    }

    #icons div a.current:hover {
        color:black;
        text-decoration:none;
    }

    #icons {
        display:block;
        clear:both;
        margin-left: 130px;
        margin-top: 5px;
        margin-bottom: 2px;
        width:500px;
        height: 60px;
    }
    #pimleftmenu {
        display:block;
        float: left;
        background: #FFFBED
        padding: 2px 2px 2px 2px;
        margin: 0px 0px 0px 5px;
    }
    #pimleftmenu ul {
        list-style-type: none;
        padding-left: 0;
        margin-left: 0;
        width: 156px;
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
        font-size: 11px;
        font-weight:bold;
        text-align: left;
    }
    #pimleftmenu ul.pimleftmenu li a:hover {
        color: #FFFBED;
        background-color: #e88d1e;
    }

    #pimleftmenu ul.pimleftmenu li a.current {
        color: #FFFBED;
        background-color: #446699;
    }

    #pimleftmenu ul.pimleftmenu li a.collapsed,
    #pimleftmenu ul.pimleftmenu li a.expanded {
        display:block;
        outline: 0;
        padding: 2px 2px 2px 4px;
        text-decoration: none;
        border: 0 ;
        color: #336699;
        font-size: 11px;
        font-weight:bold;
        text-align: left;
    }

    #pimleftmenu ul.pimleftmenu li a.expanded {
        background: #FFFBED url(images/gif/expanded.gif) no-repeat center right;
    }

    #pimleftmenu ul.pimleftmenu li a.collapsed {
        background: #FFFBED url(images/gif/collapsed.gif) no-repeat center right;
        border-bottom: 1px solid #d87415;
    }

    #pimleftmenu ul.pimleftmenu li a.collapsed:hover span,
    #pimleftmenu ul.pimleftmenu li a.expanded:hover span {
        color: #336699;
    }


    #pimleftmenu ul span {
        display:block;
    }

    #pimleftmenu li.parent span.parent {
        color: #336699;
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

    div#editPaneMemberships label {
        width: 200px;
    }

    div#editPaneMemberships br {
        clear:left;
    }

    div#editPaneMemberships input {
        display:block;
        margin: 2px 2px 2px 2px;
        float:left;
    }

    div#editPaneMemberships #membershipTypeLabel,
    div#editPaneMemberships #membershipLabel {
        display:inline;
        font-weight:bold;
        padding-left:2px;
    }

    /* photo */
    #currentImage {
        padding: 2px;
        margin: 5px 4px 14px 2px;
        border: 1px solid #FAD163;
        cursor:pointer;
    }

    #imageSizeRule {
        width:200px;
    }

    #imageHint {
        font-size:10px;
        color:#999999;
        padding-left:8px;
    }
</style>
    <link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>

<link rel="stylesheet" href="css/kir.login.css" />
 <script type="text/javascript"><!--//--><![CDATA[//><!--
 function formsubmit(where)
{
   
  $("#frmsub").attr("target","workarea");
    $("#frmsub").attr("action",where);
    $("#frmsub").submit();
  //$("#contfrm").load(where);
  
  }

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
    //--><!]]></script>
</head>


<body>
<%
        
        Dim flg As String = ""
        Dim sql As String = ""
    ' MsgBox(Request.Form("datatake"))
       
    
    Select Case flg
        Case "Id"
            sql = "select * from emp_static_info where id=" & Session("lid")
          
        Case "emp_id"
            sql = "select * from emp_static_info where emp_id='" & Session("emp_id") & "'"

    End Select
   
    ' Response.Write(Session("imglink"))
        %>
<div id="empoutbox" >
      <%  Dim dbc As New file_list
          %>
          <form id="frmsub" action="" method="post"></form>
       <div id="div1">
          <% ' Response.Write(Session("emp_path"))%>
           uper menu</div>
       <div id="div2">
             <div id="pimempleft" style="float:left;">
               <div id="picture">
                    <div id="currentImage" style="width:150px;height:210px;overflow:hidden;">
    <center>
                <a href="javascript:formsubmit('uploadpic.aspx');"> change picture </a>
            <img alt="Employee Photo" src="<% response.write(session("imglink")) %>" id="empPic" 
                 width="143.000000" height="160" style="border:0px 0px 0px 0px;"/>
      
            </center>
    <center><span class="smallHelpText" style=" font-size:12px;"><strong><% Response.Write(Session("fullempname"))%></strong></span></center>
</div>
<script type="text/javascript">
    //<![CDATA[
    function imageResize() {
        var imgHeight = $("#empPic").attr("height");
        var imgWidth = $("#empPic").attr("width");
        var newHeight = 0;
        var newWidth = 0;
        
        $('#currentImage').css('height', 'auto');

        //algorithm for image resizing
        //resizing by width - assuming width = 150,
        //resizing by height - assuming height = 180

        var propHeight = Math.floor((imgHeight/imgWidth) * 150);
        var propWidth = Math.floor((imgWidth/imgHeight) * 180);

        if (isNaN(propHeight) || (propHeight <= 180)) {
            newHeight = propHeight;
            newWidth = 150;
        }

        if (isNaN(propWidth) || (propWidth <= 150)) {
            newWidth = propWidth;
            newHeight = 180;
        }

        if(fileModified == 1) {
            newWidth = newImgWidth;
            newHeight = newImgHeight;
        }

        $("#empPic").attr("height", newHeight);
        $("#empPic").attr("width", newWidth);
        $("#empPic").attr("visibility", "visible");
    }
    
    $(document).ready(function() {
        //imageResize();
    });
    
    //]]>
</script>

</div>
               <div id="pimleftmenu">
                    <ul class="pimleftmenu">
        <li class="l1 parent">
            <a href="#" class="expanded" onclick="showHideSubMenu(this);">
                <span class="parent personal">Personal</span></a>
            <ul class="l2">
                <li class="l2">
                        <a href="javascript:formsubmit('empedit.aspx');" id="personalLink" class="personal" accesskey="p">
                            <span>Personal Details</span></a></li>
                <li class="l2">
                     <a href="javascript:formsubmit('nocolpage.aspx');" id="contactsLink" class="personal" accesskey="c">
                        <span>Contact Details</span></a></li>
                                        <li class="l2">
                                         <a href="javascript:formsubmit(/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewEmergencyContacts/empNumber/1" id="emgcontactsLink" class="personal"  accesskey="e">
                        <span>Emergency Contacts</span></a></li>
                    
                <li class="l2">
                                        <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewDependents/empNumber/1" id="dependentsLink" class="personal"  accesskey="d">
                        <span>Dependents</span></a></li>
                                                
                <li class="l2">
                                        <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewImmigration/empNumber/1" id="immigrationLink" class="personal" accesskey="i" >
                        <span>Immigration</span></a></li>
                                                
                <li class="l2">
                                            <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewJobDetails/empNumber/1" id="jobLink" accesskey="j" class="employment"  >

                            <span>Job</span></a></li>
                                             
                <li class="l2">
                                        <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewSalaryList/empNumber/1" id="paymentsLink" class="employment" accesskey="s" >
                        <span>Salary</span></a></li>
                                                
                <li class="l2">
                    <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewUsTaxExemptions/empNumber/1" id="taxLink" class="employment" accesskey="t" >
                        <span>Tax Exemptions</span></a></li>
                    <li class="l2">
                      
                    <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewReportToDetails/empNumber/1" id="report-toLink" class="employment" accesskey="r" >
                        <span>Report-to</span></a></li> 
                        
                                        <li class="l2">
                      
                    <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewQualifications/empNumber/1" id="A1" class="personal" accesskey="p">
                        <span>Qualifications</span></a></li>
                                   <li class="l2">
                                        <a href="/hrm/orangehrm-2.7.1/symfony/web/index.php/pim/viewMemberships/empNumber/1" id="membershipsLink" class="pimmemberships" accesskey="m">
                        <span>Membership</span>
                    </a>
                                    </li>                        
            </ul>
        </li>

        <!-- start of leave section -->
                <li class="l1 parent">
            <a href="#" class="expanded" onclick="showHideSubMenu(this);"><span>Leave</span></a>
            <ul class="l2">
                <li class="l2">
					<a href="javascript:leaveFormSubmission('leaveSummary');">
						<span>Leave Summary</span>
					</a>
				</li>
                <li class="l2">
					<a href="javascript:leaveFormSubmission('leaveList');">
						<span>Leave List</span>
					</a>
				</li>
            </ul>
        </li>
        
    </ul>
</div>
             </div><div style="width:5px; float:left">&nbsp;</div>
            <div id="empright" style=" vertical-align:top; float:left;width:85%">
                <iframe id="workarea" height="70%" scrolling="auto" src="hrmwork.htm" style="vertical-align:top; width:100%"></iframe>
                </div>
       </div> 
</div>
</body>
</html>
<script language="javascript" type="text/javascript">
$(document).ready(function(){

});
</script>