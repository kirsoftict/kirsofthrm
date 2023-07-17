<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empcontener.aspx.vb" Inherits="empcontener" %>

<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.data.sqlclient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Microsoft.VisualBasic" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%Dim fm As New formMaker%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title></title>
    
    <style type="text/css">
    
</style>
<link rel="stylesheet" href="css/kir.login.css" />
    <link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
<script type="text/javascript" src="scripts/style.js"></script>
    <script src="scripts/script.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="css/kir.login.css" />
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>

<link rel="stylesheet" href="css/kir.login.css" />
 
 <script type="text/javascript"><!--     //--><![CDATA[//><!--
     function formsubmit(where) {

         $("#frmsub").attr("target", "workarea");
         $("#frmsub").attr("action", where);
         $("#frmsub").submit();
         // alert($("#workarea").attr("height"));
         $("#contfrm").load(where);

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
         for (var i = 0; i < uls.length; i++) {
             ul = uls[i].style.display = uldisplay;
         }

         link.className = newClass;
     }

     tableDisplayStyle = "table";
     //--><!]]></script>
    <style>
    .menu_a
    {
    	color:White;
    	}
    </style>
</head>
<body>

<% 
    Response.Write(fm.helpback(False,True,False,False,"",""))
    Dim dbc As New file_list
    Dim dbt As New dbclass
    Dim dt As DataTableReader
            
    Dim flg As String = ""
    Dim sql As String = ""
    'Response.Write(Session("emp_id"))
    ' MsgBox(Request.Form("datatake"))
    If Session("emp_id") = "" Then
        If Request.Form("datatake") = "" Then
                %>
                   <script type="text/javascript">
                       window.location = "emplist.aspx";
                   </script>
                <%
                Else
                    Dim valx As String
                    valx = Request.Form("datatake")
                    ' Response.Write(Request.Form("datatake"))
                    Session("lid") = valx
          
                    If IsNumeric(valx) = True Then
                        flg = "Id"
                        Session("lid") = Request.Form("datatake")
                    Else
                        flg = "emp_id"
                        Session("emp_id") = valx
                    End If
                    dt = dbt.dtmake("emp_info", "select * from emprec where emp_id='" & valx & "' and active='y' order by id desc", session("con"))
                    If dt.HasRows = True Then
                        dt.Read()
                        If dt.IsDBNull(8) = True Then
                            Session("status_w") = True
                        Else
                            If dt.Item("end_date").ToString = "" Then
                                Session("status_w") = True
                            Else
                                Session("status_w") = False
                            End If
                        End If
                        Session("emptid") = dt.Item("id")
                    Else
                        Session("emptid") = fm.getinfo2("select id from emprec where emp_id='" & valx & "'  order by hire_date desc", Session("con"))

                    End If
                    dt.Close()
                    
                End If
            
                Select Case flg
                    Case "Id"
                        sql = "select * from emp_static_info where id=" & Session("lid")
          
                    Case "emp_id"
                        sql = "select * from emp_static_info where emp_id='" & Session("emp_id") & "'"

                End Select
                If sql <> "" Then
        
                    dt = dbt.dtmake("emp_static_info" & Today.ToLocalTime, sql, session("con"))
                    If dt.HasRows Then
                        dt.Read()
                        Session("emp_id") = dt.Item("emp_id").trim
                        Session("fullempname") = dt.Item("first_name") & " " & dt.Item("middle_name") & " " & dt.Item("last_name")
                        Session("emp_path") = Server.MapPath("employee") & "\" & Session("emp_id")
                        If dt.IsDBNull(12) = True Then
                            Session("imglink") = "images/gif/default_employee_image.gif"
                        ElseIf dt.Item("imglink") = " " Or dt.Item("imglink") = "" Then
                            Session("imglink") = "images/gif/default_employee_image.gif"
                        Else
                            Session("imglink") = dt.Item("imglink")
                        End If
                    End If
                    If Session("imglink") = "" Then
                        Session("imglink") = "images/gif/default_employee_image.gif"
                    End If
                    dbt = Nothing
                End If
    
        %>
<div id="empoutbox" >
      <%  
          If File.Exists(Session("emp_path")) = False Then
              dbc.makedir(Session("emp_path"))
          End If
         
          %>
          <form id="frmsub" action="" method="post"></form>
       <div id="div1">
          <% ' Response.Write(Session("emp_path"))%>
          <div id="pimsubtopmenu">  
          <ul class="l2" style="display:inline;">
                <li class="l2" style="display:inline;">
                        <a href="javascript:formsubmit('attachcv.aspx');" id="A1" class="personal" accesskey="A">
                            <span>Attachments</span></a></li>
                <li class="l2" style="display:inline;">
                <a id="menuemp" style="background-color:#243682; cursor:pointer;" 
                 onclick="javascript:showobjar('menuemp','emprec',0,18);" onmouseout=" 
                 javascript:ha('emprec');"  target='frm_tar'>
                 Career Information</a>
                    
                       </li>  
                          <li class="l2" style="display:inline;">
                <a id="menumed" style="background-color:#243682; cursor:pointer;" 
                 onclick="javascript:showobjar('menumed','empmed',0,18);" onmouseout=" 
                 javascript:ha('empmed');"  target='frm_tar'>
                 Medical Information</a>
                    
                       </li>        
                       <li class="l2" style="display:inline;">
                                         <a href="javascript:formsubmit('empappr.aspx');" id="A3" class="personal"  accesskey="e">
                        <span>Appraisal</span></a></li>       
                        <li class="l2" style="display:inline;">
                                         <a href="javascript:formsubmit('emplogincreate.aspx');" id="A8" class="personal"  accesskey="e">
                        <span>Create User Account</span></a></li>
                        
                       
                     </ul>  </div></div>
       <div id="div2">
             <div id="pimempleft" style="float:left;">
               <div id="picture" style='height:270px'>
                    <div id="currentImage" style="width:150px;height:260px;">
    <center>
                <a href="javascript:formsubmit('uploadpic.aspx');"> change picture </a>
            <img alt="Employee Photo" src="<%response.write(session("imglink")) %>" id="empPic" 
                 width="143.000000" height="170" style="border:0px 0px 0px 0px;"/>
                  </center>
                  <%  If String.IsNullOrEmpty(Session("emptid")) = False Then
                          If Session("emptid").ToString <> "" Then%>
    <center><span class="smallHelpText" style=" font-size:12px;"><strong>
  <a href="dataallview.aspx?empid=<% response.write(session("emp_id"))%>" target="workarea">  <% Response.Write(Session("fullempname"))%></a><br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Emp id: &nbsp;&nbsp;&nbsp;<%  
                                                      Response.Write(Session("emp_id"))%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                     <br /> File No:<% Response.Write(fm.getinfo2("select file_no from emp_static_info where emp_id='" & Session("emp_id") & "'", Session("con")).ToString)%></strong>(<% response.write(session("emptid"))%> )</span></center>
            <%  
if fm.getinfo2("select file_no from emp_static_info where emp_id='" & Session("emp_id") & "'", Session("con")).ToString="None" then
session("emptid")=""
end if
End If
            End If%>
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

        var propHeight = Math.floor((imgHeight / imgWidth) * 150);
        var propWidth = Math.floor((imgWidth / imgHeight) * 180);

        if (isNaN(propHeight) || (propHeight <= 180)) {
            newHeight = propHeight;
            newWidth = 150;
        }

        if (isNaN(propWidth) || (propWidth <= 150)) {
            newWidth = propWidth;
            newHeight = 180;
        }

        if (fileModified == 1) {
            newWidth = newImgWidth;
            newHeight = newImgHeight;
        }

        $("#empPic").attr("height", newHeight);
        $("#empPic").attr("width", newWidth);
        $("#empPic").attr("visibility", "visible");
    }

    $(document).ready(function () {
        //imageResize();
    });
    
    //]]>
</script>
</div>  <div id="pimleftmenu" style="padding-top:-2em;">
                    <ul class="pimleftmenu">
        <li class="l1 parent">
            <a class="expanded" onclick="showHideSubMenu(this)" style='cursor:pointer' >
                <span class="parent personal">Personal</span></a>
            <ul class="l2">
                <li class="l2">
                        <a href="javascript:formsubmit('empedit.aspx');" id="personalLink" class="personal" accesskey="p">
                            <span>Personal Details</span></a></li>
                <li class="l2">
                     <a href="javascript:formsubmit('nocolpage.aspx');" id="contactsLink" class="personal" accesskey="c">
                        <span>Contact Details</span></a></li>
                                        <li class="l2">
                                         <a href="javascript:formsubmit('emgcontact.aspx');" id="emgcontactsLink" class="personal"  accesskey="e">
                        <span>Emergency Contacts</span></a></li>
                    
                <li class="l2">
                                         <a href="javascript:formsubmit('dependant.aspx');" id="A2" class="personal"  accesskey="e">
                        <span>Dependents</span></a></li>
                                                
                <li class="l2">
                                        <a href="javascript:formsubmit('education.aspx');" id="immigrationLink" class="personal" accesskey="i" >
                        <span>Education Qualification</span></a></li>
                                                
                <li class="l2">
                                            <a href="javascript:formsubmit('empworkexp.aspx');" id="jobLink" accesskey="j" class="employment"  >

                            <span>Work Experiences</span></a></li>
                                             
                <li class="l2">
                                        <a href="javascript:formsubmit('emplanguage.aspx');" id="paymentsLink" class="employment" accesskey="s" >
                          <span>Language</span></a></li>
                <li class="l2">
                                        <a href="javascript:formsubmit('empreferance.aspx');" id="A5" class="employment" accesskey="s" >
                        <span>Reference</span></a></li>
                
                <li class="l2">
                                        <a href="javascript:formsubmit('empskill.aspx');" id="A6" class="employment" accesskey="s" >
                        <span>Skills</span></a></li>
            </ul>
        </li>
         <!-- start of leave section -->
                <li class="l1 parent">
            <a class="expanded" onclick="showHideSubMenu(this);" style='cursor:pointer'><span>Leave</span></a>
            <ul class="l2">
            <li class="l2">
					<a href="javascript:formsubmit('empleavesetup.aspx');">
						<span>Leave Setup</span>
					</a>
				</li>
                <li class="l2">
					<a href="javascript:formsubmit('leavesummery.aspx');">
						<span>Leave Budgeting</span>
					</a>
				</li>
                <li class="l2">
					<a href="javascript:formsubmit('leavetake.aspx');">
						<span>Leave take</span>
					</a>
				</li>
            </ul>
        </li>
        
    </ul>
</div>
             </div><div style="width:5px; float:left">&nbsp;</div>
            <div id="empright" style=" vertical-align:top; float:left;width:85%; height:auto;">
            
             <iframe name="workarea" id="workarea" src="hrmwork.htm" enableviewstate="false" frameborder="0" style=" vertical-align:top; width:100%; height:600px; border:none;"></iframe>
                </div>
       </div> 
</div>
<%  Else
        '  Response.Write("comming here")
      '  Response.Write(Session("emp_id"))
        dt = dbt.dtmake("emplist", "select * from emprec where emp_id='" & Session("emp_id") & "' order by id desc", Session("con"))
        If dt.HasRows Then
            While dt.Read
   %>           
<div id="empoutbox" >
      <%  
          If File.Exists(Session("emp_path")) = False Then
              dbc.makedir(Session("emp_path"))
          End If
         
          %>
          <form id="frmsub" action="" method="post"></form>
       <div id="div1">
          <% ' Response.Write(Session("emp_path"))%>
          <div id="pimsubtopmenu">  
          <ul class="l2" style="display:inline;">
                <li class="l2" style="display:inline;">
                        <a href="javascript:formsubmit('attachcv.aspx');" id="A1" class="personal" accesskey="A">
                            <span>Attachments</span></a></li>
                <li class="l2" style="display:inline;">
                <a id="menuemp" style="background-color:#243682; cursor:pointer;" 
                 onclick="javascript:showobjar('menuemp','emprec',0,18);" onmouseout=" 
                 javascript:ha('emprec');"  target='frm_tar'>
                 Career Information</a>
                    
                       </li>  
                          <li class="l2" style="display:inline;">
                <a id="menumed" style="background-color:#243682; cursor:pointer;" 
                 onclick="javascript:showobjar('menumed','empmed',0,18);" onmouseout=" 
                 javascript:ha('empmed');"  target='frm_tar'>
                 Medical Information</a>
                    
                       </li>        
                       <li class="l2" style="display:inline;">
                                         <a href="javascript:formsubmit('empappr.aspx');" id="A3" class="personal"  accesskey="e">
                        <span>Appraisal</span></a></li>       
                        <li class="l2" style="display:inline;">
                                         <a href="javascript:formsubmit('emplogincreate.aspx');" id="A8" class="personal"  accesskey="e">
                        <span>Create User Account</span></a></li>
                        
                       
                     </ul>  </div></div>
       <div id="div2">
             <div id="pimempleft" style="float:left;">
               <div id="picture">
                    <div id="currentImage" style="width:150px;height:auto;">
    <center>
                <a href="javascript:formsubmit('uploadpic.aspx');"> change picture </a>
            <img alt="Employee Photo" src="<%response.write(session("imglink")) %>" id="empPic" 
                 width="143.000000" height="170" style="border:0px 0px 0px 0px;"/>
                  </center>
                  <%  If String.IsNullOrEmpty(Session("emptid")) = False Then
                          If Session("emptid").ToString <> "" Then%>
    <center><span class="smallHelpText" style=" font-size:12px;"><strong>
  <a href="dataallview.aspx?empid=<% response.write(session("emp_id"))%>" target="workarea">  <% Response.Write(Session("fullempname"))%></a><br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Emp id: &nbsp;&nbsp;&nbsp;<%  
                                                      Response.Write(Session("emp_id"))%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                     <br /> File No:<% Response.Write(fm.getinfo2("select file_no from emp_static_info where emp_id='" & Session("emp_id") & "'", Session("con")).ToString)%></strong>(<% response.write(session("emptid"))%> )</span></center>
            <%  End If
            End If%>
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

        var propHeight = Math.floor((imgHeight / imgWidth) * 150);
        var propWidth = Math.floor((imgWidth / imgHeight) * 180);

        if (isNaN(propHeight) || (propHeight <= 180)) {
            newHeight = propHeight;
            newWidth = 150;
        }

        if (isNaN(propWidth) || (propWidth <= 150)) {
            newWidth = propWidth;
            newHeight = 180;
        }

        if (fileModified == 1) {
            newWidth = newImgWidth;
            newHeight = newImgHeight;
        }

        $("#empPic").attr("height", newHeight);
        $("#empPic").attr("width", newWidth);
        $("#empPic").attr("visibility", "visible");
    }

    $(document).ready(function () {
        //imageResize();
    });
    
    //]]>
</script>
</div>  <div id="pimleftmenu" style="padding-top:-2em;">
                    <ul class="pimleftmenu">
        <li class="l1 parent">
            <a class="expanded" onclick="showHideSubMenu(this)" style='cursor:pointer' >
                <span class="parent personal">Personal</span></a>
            <ul class="l2">
                <li class="l2">
                        <a href="javascript:formsubmit('empedit.aspx');" id="personalLink" class="personal" accesskey="p">
                            <span>Personal Details</span></a></li>
                <li class="l2">
                     <a href="javascript:formsubmit('nocolpage.aspx');" id="contactsLink" class="personal" accesskey="c">
                        <span>Contact Details</span></a></li>
                                        <li class="l2">
                                         <a href="javascript:formsubmit('emgcontact.aspx');" id="emgcontactsLink" class="personal"  accesskey="e">
                        <span>Emergency Contacts</span></a></li>
                    
                <li class="l2">
                                         <a href="javascript:formsubmit('dependant.aspx');" id="A2" class="personal"  accesskey="e">
                        <span>Dependents</span></a></li>
                                                
                <li class="l2">
                                        <a href="javascript:formsubmit('education.aspx');" id="immigrationLink" class="personal" accesskey="i" >
                        <span>Education Qualification</span></a></li>
                                                
                <li class="l2">
                                            <a href="javascript:formsubmit('empworkexp.aspx');" id="jobLink" accesskey="j" class="employment"  >

                            <span>Work Experiences</span></a></li>
                                             
                <li class="l2">
                                        <a href="javascript:formsubmit('emplanguage.aspx');" id="paymentsLink" class="employment" accesskey="s" >
                          <span>Language</span></a></li>
                <li class="l2">
                                        <a href="javascript:formsubmit('empreferance.aspx');" id="A5" class="employment" accesskey="s" >
                        <span>Reference</span></a></li>
                
                <li class="l2">
                                        <a href="javascript:formsubmit('empskill.aspx');" id="A6" class="employment" accesskey="s" >
                        <span>Skills</span></a></li>
            </ul>
        </li>
         <!-- start of leave section -->
                <li class="l1 parent">
            <a class="expanded" onclick="showHideSubMenu(this);" style='cursor:pointer'><span>Leave</span></a>
            <ul class="l2">
            <li class="l2">
					<a href="javascript:formsubmit('empleavesetup.aspx');">
						<span>Leave Setup</span>
					</a>
				</li>
                <li class="l2">
					<a href="javascript:formsubmit('leavesummery.aspx');">
						<span>Leave Budgeting</span>
					</a>
				</li>
                <li class="l2">
					<a href="javascript:formsubmit('leavetake.aspx');">
						<span>Leave take</span>
					</a>
				</li>
            </ul>
        </li>
        
    </ul>
</div>
             </div><div style="width:5px; float:left">&nbsp;</div>
            <div id="empright" style=" vertical-align:top; float:left;width:85%; height:auto;">
            
             <iframe name="workarea" id="workarea" src="hrmwork.htm" enableviewstate="false" frameborder="0" style=" vertical-align:top; width:100%; height:600px; border:none;"></iframe>
                </div>
       </div> 
</div><%
      End While
  Else
      If Request.Form("datatake") = "" Then
                %>
                   <script type="text/javascript">
                       window.location = "emplist.aspx";
                   </script>
                <%
                Else
                    Dim valx As String
                    valx = Request.Form("datatake")
                    Session("lid") = valx
          
                    If IsNumeric(valx) = True Then
                        flg = "Id"
                        Session("lid") = Request.Form("datatake")
                    Else
                        flg = "emp_id"
                        Session("emp_id") = valx
                    End If
                    dt = dbt.dtmake("emp_info", "select * from emprec where emp_id='" & valx & "' order by id desc", session("con"))
                    If dt.HasRows = True Then
                        dt.Read()
                        If dt.IsDBNull(8) = True Then
                            Session("status_w") = True
                        Else
                            If dt.Item("end_date").ToString = "" Then
                                Session("status_w") = True
                            Else
                                Session("status_w") = False
                            End If
                        End If
                        Session("emptid") = dt.Item("id")
                    End If
                    dt.Close()
                    
                End If
            
                Select Case flg
                    Case "Id"
                        sql = "select * from emp_static_info where id=" & Session("lid")
          
                    Case "emp_id"
                        sql = "select * from emp_static_info where emp_id='" & Session("emp_id") & "'"

                End Select
                If sql <> "" Then
        
                    dt = dbt.dtmake("emp_static_info" & Today.ToLocalTime, sql, session("con"))
                    If dt.HasRows Then
                        dt.Read()
                        Session("emp_id") = dt.Item("emp_id").trim
                        Session("fullempname") = dt.Item("first_name") & " " & dt.Item("middle_name") & " " & dt.Item("last_name")
                        Session("emp_path") = Server.MapPath("employee") & "\" & Session("emp_id")
                        If dt.IsDBNull(12) = True Then
                            Session("imglink") = "images/gif/default_employee_image.gif"
                        ElseIf dt.Item("imglink") = " " Or dt.Item("imglink") = "" Then
                            Session("imglink") = "images/gif/default_employee_image.gif"
                        Else
                            Session("imglink") = dt.Item("imglink")
                        End If
                    End If
                    If Session("imglink") = "" Then
                        Session("imglink") = "images/gif/default_employee_image.gif"
                    End If
                    dbt = Nothing
                End If
    
        %>
<div id="empoutbox" >
      <%  
          If File.Exists(Session("emp_path")) = False Then
              dbc.makedir(Session("emp_path"))
          End If
         
          %>
          <form id="frmsub" action="" method="post"></form>
       <div id="div1">
          <% ' Response.Write(Session("emp_path"))%>
          <div id="pimsubtopmenu">  
          <ul class="l2" style="display:inline;">
                <li class="l2" style="display:inline;">
                        <a href="javascript:formsubmit('attachcv.aspx');" id="A1" class="personal" accesskey="A">
                            <span>Attachments</span></a></li>
                <li class="l2" style="display:inline;">
                <a id="menuemp" style="background-color:#243682; cursor:pointer;" 
                 onclick="javascript:showobjar('menuemp','emprec',0,18);" onmouseout=" 
                 javascript:ha('emprec');"  target='frm_tar'>
                 Career Information</a>
                    
                       </li>  
                          <li class="l2" style="display:inline;">
                <a id="menumed" style="background-color:#243682; cursor:pointer;" 
                 onclick="javascript:showobjar('menumed','empmed',0,18);" onmouseout=" 
                 javascript:ha('empmed');"  target='frm_tar'>
                 Medical Information</a>
                    
                       </li>        
                       <li class="l2" style="display:inline;">
                                         <a href="javascript:formsubmit('empappr.aspx');" id="A3" class="personal"  accesskey="e">
                        <span>Appraisal</span></a></li>       
                        <li class="l2" style="display:inline;">
                                         <a href="javascript:formsubmit('emplogincreate.aspx');" id="A8" class="personal"  accesskey="e">
                        <span>Create User Account</span></a></li>
                        
                       
                     </ul>  </div></div>
       <div id="div2">
             <div id="pimempleft" style="float:left;">
               <div id="picture">
                    <div id="currentImage" style="width:150px;height:230px;overflow:hidden;">
    <center>
                <a href="javascript:formsubmit('uploadpic.aspx');"> change picture </a>
            <img alt="Employee Photo" src="<%response.write(session("imglink")) %>" id="empPic" 
                 width="143.000000" height="170" style="border:0px 0px 0px 0px;"/>
                  </center>
                  <%  If String.IsNullOrEmpty(Session("emptid")) = False Then
                          If Session("emptid").ToString <> "" Then%>
    <center><span class="smallHelpText" style=" font-size:12px;"><strong>
  <a href="dataallview.aspx?empid=<% response.write(session("emp_id"))%>" target="workarea">  <% Response.Write(Session("fullempname"))%></a><br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Emp id: &nbsp;&nbsp;&nbsp;<%  
                                                      Response.Write(Session("emp_id"))%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                     <br /> File No:<% Response.Write(fm.getinfo2("select file_no from emp_static_info where emp_id='" & Session("emp_id") & "'", Session("con")).ToString)%></strong>(<% response.write(session("emptid"))%> )</span></center>
            <%  End If
            End If%>
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

        var propHeight = Math.floor((imgHeight / imgWidth) * 150);
        var propWidth = Math.floor((imgWidth / imgHeight) * 180);

        if (isNaN(propHeight) || (propHeight <= 180)) {
            newHeight = propHeight;
            newWidth = 150;
        }

        if (isNaN(propWidth) || (propWidth <= 150)) {
            newWidth = propWidth;
            newHeight = 180;
        }

        if (fileModified == 1) {
            newWidth = newImgWidth;
            newHeight = newImgHeight;
        }

        $("#empPic").attr("height", newHeight);
        $("#empPic").attr("width", newWidth);
        $("#empPic").attr("visibility", "visible");
    }

    $(document).ready(function () {
        //imageResize();
    });
    
    //]]>
</script>
</div>  <div id="pimleftmenu" style="padding-top:-2em;">
                    <ul class="pimleftmenu">
        <li class="l1 parent">
            <a class="expanded" onclick="showHideSubMenu(this)" style='cursor:pointer' >
                <span class="parent personal">Personal</span></a>
            <ul class="l2">
                <li class="l2">
                        <a href="javascript:formsubmit('empedit.aspx');" id="personalLink" class="personal" accesskey="p">
                            <span>Personal Details</span></a></li>
                <li class="l2">
                     <a href="javascript:formsubmit('nocolpage.aspx');" id="contactsLink" class="personal" accesskey="c">
                        <span>Contact Details</span></a></li>
                                        <li class="l2">
                                         <a href="javascript:formsubmit('emgcontact.aspx');" id="emgcontactsLink" class="personal"  accesskey="e">
                        <span>Emergency Contacts</span></a></li>
                    
                <li class="l2">
                                         <a href="javascript:formsubmit('dependant.aspx');" id="A2" class="personal"  accesskey="e">
                        <span>Dependents</span></a></li>
                                                
                <li class="l2">
                                        <a href="javascript:formsubmit('education.aspx');" id="immigrationLink" class="personal" accesskey="i" >
                        <span>Education Qualification</span></a></li>
                                                
                <li class="l2">
                                            <a href="javascript:formsubmit('empworkexp.aspx');" id="jobLink" accesskey="j" class="employment"  >

                            <span>Work Experiences</span></a></li>
                                             
                <li class="l2">
                                        <a href="javascript:formsubmit('emplanguage.aspx');" id="paymentsLink" class="employment" accesskey="s" >
                          <span>Language</span></a></li>
                <li class="l2">
                                        <a href="javascript:formsubmit('empreferance.aspx');" id="A5" class="employment" accesskey="s" >
                        <span>Reference</span></a></li>
                
                <li class="l2">
                                        <a href="javascript:formsubmit('empskill.aspx');" id="A6" class="employment" accesskey="s" >
                        <span>Skills</span></a></li>
            </ul>
        </li>
         <!-- start of leave section -->
                <li class="l1 parent">
            <a class="expanded" onclick="showHideSubMenu(this);" style='cursor:pointer'><span>Leave</span></a>
            <ul class="l2">
            <li class="l2">
					<a href="javascript:formsubmit('empleavesetup.aspx');">
						<span>Leave Setup</span>
					</a>
				</li>
                <li class="l2">
					<a href="javascript:formsubmit('leavesummery.aspx');">
						<span>Leave Budgeting</span>
					</a>
				</li>
                <li class="l2">
					<a href="javascript:formsubmit('leavetake.aspx');">
						<span>Leave take</span>
					</a>
				</li>
            </ul>
        </li>
        
    </ul>
</div>
             </div><div style="width:5px; float:left">&nbsp;</div>
            <div id="empright" style=" vertical-align:top; float:left;width:85%; height:auto;">
            
             <iframe name="workarea" id="workarea" src="hrmwork.htm" enableviewstate="false" frameborder="0" style=" vertical-align:top; width:100%; height:600px; border:none;"></iframe>
                </div>
       </div> 
</div><%
  End If
    End If
    
 %>
 <div class='submenu' id='emprec' style='visibility:hidden; left:0px; top:0px; position:absolute;background-image:url(images/menubg.jpg);'
              onmouseover="javascript:showobjar('menuemp','emprec',0,18);"
                          onmouseout="javascript:ha('emprec');">
  <a class="menu_a" href='empemp1.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Hiring Info.</a>    
  <%   Dim hr As String
      Dim contd As String
    
          hr = fm.getinfo2("select type_recuritment from emprec where id=" & Session("emptid") & " and end_date is null order by hire_date desc", Session("con"))
 If hr<>"None" then
          If hr = "Contract" Then
              contd = fm.getinfo2("select dateend from emp_contract where emptid='" & Session("emptid") & "'", Session("con"))
              If contd = "None" Then
                  %>
                  <script>
                      $("#frmx").attr("target", "workarea");
                      $("#frmx").attr("action", "contract_entry.aspx");
                      $("#frmx").submit();
                  </script>
                  
                  <%
              End If
  
              
              %>
  <a class="menu_a" href='contract_entry.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Contract info.</a>    
  <% End If
  End If%>
  <a class="menu_a" href='empemp2.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Job Assignment</a>    
   <a class="menu_a" href='empsalary.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Salary Review</a>    
  <!--a class="menu_a" href='empassignproject.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Project Assignment</a-->    
  <a class="menu_a" href='empallowance.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Allowance</a>  
    <a class="menu_a" href='empperdime.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Per-diem</a>    
  <a class="menu_a" href='empresign.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Termination</a>
   <a class="menu_a" href='pension_start.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('emprec'); " target='workarea'>Pension start</a>

 </div>
 <div class='submenu' id='empmed' style='visibility:hidden; left:0px; top:0px; position:absolute;background-image:url(images/menubg.jpg);'
              onmouseover="javascript:showobjar('menumed','empmed',0,18);"
                          onmouseout="javascript:ha('empmed');">

  <a class="menu_a" href='empmedicalbgt.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('empmed'); " target='workarea'>Medical Bugdeting</a>    
  <a class="menu_a" href='medicaltake.aspx' onmouseover="javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" onmouseout="this.style.background='url(images/blue_banner-760x147x.jpg) #224488';"onclick="javascript:document.getElementById('workarea').src='';ha('empmed'); " target='workarea'>Medical payment</a>    

 </div>
 <form id='frmpay' name='frmpay' method="post" action=''></form>
 <script language="javascript" type="text/javascript">
     $("#frmpay").attr("target", "chksess");
     $("#frmpay").attr("action", "checksession.aspx");
     $("#frmpay").submit();
 </script>
 <%dbc = Nothing
     dbt = Nothing
     fm=Nothing%>
</body>
</html>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        // Set specific variable to represent all iframe tags.
        var iFrames = document.getElementsByTagName('workarea');

        // Resize heights.
        function iResize() {
            // Iterate through all iframes in the page.
            for (var i = 0, j = iFrames.length; i < j; i++) {
                // Set inline style to equal the body height of the iframed content.
                iFrames[i].style.height = iFrames[i].contentWindow.document.body.offsetHeight + 'px';
            }
        }

        // Check if browser is Safari or Opera.

    });

</script>