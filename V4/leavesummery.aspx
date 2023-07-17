<%@ Page Language="VB" AutoEventWireup="false" CodeFile="leavesummery.aspx.vb" Inherits="scripts_leavesummery" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  
    If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
       End If
       If Session("emp_id") = "" Then
       %>

<script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="empcontener.aspx";
</script>

<%
ElseIf Session("emptid").ToString = "" Then
      
    Session("emptid") = 0
     %>

<script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="empcontener.aspx";
</script>

<%
    
Else%>
 

<script type="text/javascript">
//alert('<% response.write(session("emp_id")) %>');
</script>

<%    
End If
       %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
<link rel="stylesheet" type="text/css" media="print, handheld" href="css/media.css"/>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css"/>
	<script type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.dialog.js"></script>
<script type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
    <script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>

<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
	<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
	 <script language="javascript" type="text/javascript">
        function approved(id,approved,timeapp,take) {
            var link;
            switch(take)
            {
                case "calculate":
                    link = '?rd=leavesummery&what_task=calc&tbl=emp_leave_budget&emp_id=<% response.write(session("emp_id")) %>'
                    $("#messagebox").css({ 'width': '25px', 'hieght': '25px', 'backgound-image': 'url(images/loading.gif) no-repeat' });
                    window.open(link,"leav calc","menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px")
                   // $("#messagebox").load(link);
                    //alert(link);            
                    break;
                case "calculateview":
                  link='jqueryupdate.aspx?what_task=leaveview&emp_id=<% response.write(session("emp_id")) %>'
                   
                    $("#messagebox").load(link);
                 //alert(link);            
                break;
                case "approved":
                     var str="who_approved=" + approved + "&approved=approved";
                    if(id!=""){
                // alert("jqueryupdate.aspx?tbl=emp_leave_budget&task=update&key=id&keyval=" + id + "&" + str);
                   link="jqueryupdate.aspx?rd=leavesummery&what_task=approved&tbl=emp_leave_budget&task=update&key=id&keyval=" + id + "&" + str
                   
                     $("#messagebox").load(link);
                     // $('#frmemp_static_info').submit();           
                       // return true;
                        }
                break;
                case "available":
                break;
                default:
                
            }
       
        }
        
        </script>

</head>

     
<body>

 <div id="div1" style="width:900px; display:block;">
          <% ' Response.Write(Session("emp_path"))%>
          <div id="pimsubtopmenu">  
          <ul class="l2">
                <li class="l2">
                        <a href="javascript:approved('','','','calculate');" id="A1" class="personal" accesskey="A">
                            Calculate Leave Budget</a></li>
                <li class="l2">
                     <a href="javascript:approved('','','','calculateview');" id="A2" class="personal" accesskey="c">
                       view calculation</a></li>
             
                     </ul>   
                       </div></div>
<form name="frmapproved" id="frmapproved" method="post" action=""></form>


    <%  'get emp. info
        'Response.Write(fm.leavebugetcal(session("con"), Session("emp_id")))
        ' Response.Write(Date.Now.ToLocalTime.TimeOfDay..ToString)
        Dim dt3 As DataTableReader
        Dim conx As SqlConnection
        conx = session("con")
        Dim empid As String = Session("emp_id")
        Dim strout As String = ""
        
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim xmsg As String = Request.QueryString("msg")
        If Request.QueryString("calc") = "called" Then
            ' Response.Write(Session("emptid"))
            
            strout = calc3(Session("con"), Session("emp_id"), Session("emptid"), Session("path"))
        Else
            strout = calc3(Session("con"), Session("emp_id"), Session("emptid"), Session("path"))
        End If
      '  Response.Write(strout)
        
        %>
        
        <div id="messagebox"></div>
        <%
        Dim i As Integer = 0
            Dim color As String
            try
            dt3 = dbx.dtmake("new3", "select id,l_s_year,l_e_year,no_days_with_period,approved,isexp from emp_leave_budget where emp_id='" & empid & "' and emptid=" & Session("emptid") & " order by l_e_year", conx)
        If dt3.HasRows = True Then
            Dim fc As Integer
                fc = Math.Round(900 / dt3.FieldCount)
           
            If i = 0 Then
                    Dim arrheader() As String = {"id", "Year Alloted Start", "Year End", "No Days", "Approved", "Expire"}
                    strout &= "<div style='background:#7595f7; display:inline; width:900px;height:24px' >" & _
                "<div style='backgroud-color:#7595f7; width:900px; height:24px; font-size:14pt; color:white;'>&nbsp;" & _
                "Employee Id:<font style='color:black;'>" & Session("emp_id") & _
                "</font> Full Name:<font style='color:black;'>" & Session("fullempname") & _
                "</div>" & _
                "<div style='background-color:#7595cc; width:900px;height:20px; display:block;'> "
                    For j As Integer = 1 To dt3.FieldCount - 1
                        strout &= "<div style='width:" & fc & "px;float:left;'><b>" & arrheader(j) & "</b>&nbsp;</div>"
                    Next

                    strout &= "</div><div style='clear:both;'></div>"

            End If
               
            While dt3.Read
                If i Mod 2 Then
                        color = "#E3EAEB"
                Else
                    color = "#fefefe"
                    End If
                    strout &= "<div style='background:" & color & ";width:900px;'>"
                    For j As Integer = 1 To dt3.FieldCount - 1
                       
                        If dt3.GetName(j) = "approved" Then
                         If dt3.IsDBNull(j) = True Then
                                    strout &= "<div class='listrow' style='width:" & fc & "px;float:left; cursor:pointer;font-style:italic; " & _
                                    "color:blue;background:" & color & "' onclick=" & Chr(34) & "javascript:approved('" & _
                                            dt3.Item("id") & "','" & Session("emp_iid") & "','" & Today.ToShortDateString.ToString & "','approved');" & _
                                            Chr(34) & _
                                            " onmouseover = " & Chr(34) & _
                         "javascript: this.style.background='#00ff00';" & Chr(34) & " onmouseout=" & Chr(34) & _
                         "javascript: this.style.background='';" & Chr(34) & " title='Click To Approve'>Not Approved&nbsp;</div>"
                                ElseIf dt3.Item(j).ToString = "" Then
                                    strout &= "<div class='listrow' style='width:" & fc & "px;float:left; cursor:pointer;font-style:italic; color:blue;' onclick=" & Chr(34) & "javascript:approved('" & _
                                    dt3.Item("id") & "','" & Session("emp_iid") & "','" & Today.ToShortDateString & "','approved');" & Chr(34) & _
                                    " onmouseover = " & Chr(34) & _
                                                  "javascript: this.style.background='#00ff00';" & Chr(34) & " onmouseout=" & Chr(34) & _
                                                  "javascript: this.style.background='';" & Chr(34) & " title='Click To Approve'>Not Approved&nbsp;</div>"
                                Else
                                    strout &= "<div class='listrow' style='width:" & fc & "px; float:left; display:inline;'>Approved&nbsp;</div>"
                                End If
                        ElseIf LCase(dt3.GetName(j)) = "isexp" Then
                            If dt3.IsDBNull(j) = True Then
                                If fm.isexp(Today.ToShortDateString, dt3.Item("l_e_year"), 2, "y") Then
                                    strout &= "<div class='listrow' style='width:" & fc & "px;float:left; cursor:pointer;font-style:italic; color:blue;' onclick=" & Chr(34) & "javascript:approved('" & _
                       dt3.Item("id") & "','" & Session("emp_iid") & "','" & Today.ToShortDateString.ToString & "','updateexp');" & Chr(34) & _
                       " onmouseover = " & Chr(34) & _
                                     "javascript: this.style.background='#00ff00';" & Chr(34) & " onmouseout=" & Chr(34) & _
                                     "javascript: this.style.background='';" & Chr(34) & " title='Click To update: this leave is expired'>Expired&nbsp;</div>"
                                End If
                            Else
                                strout &= "<div class='listrow' style='width:" & fc & "px;float:left; display:inline'>Expired&nbsp;</div>"
  
                            End If
                        Else
                            strout &= "<div class='listrow' style='width:" & fc & "px;float:left; display:inline'>" & dt3.Item(j) & "&nbsp;</div>"
  
                        End If
                    Next
                    strout &= "</div><div style='clear:both;'></div>"
                    i = i + 1
                End While
                strout &= "</div>"
                Else
                    Dim emptid As Integer
                    emptid = Session("emptid")
                    Response.Write("data is not found click on calculate ==>")
                    'Response.Write("select hire_date from emprec where emp_id='" & empid & "' and emptid=" & emptid.ToString & " and end_date is Null order by id desc")
                    '  strout = fm.calc2(Session("con"), Session("emp_id"), emptid, Session("path"))
            
                %>
                <script type="text/javascript">
             //  approved('','','',"calculate");
                </script>
                <%
            End If
       
            'Response.Write("<table><tr bgcolor='blue'><td>Expired leave days</td><td>" & intexp.ToString & "</td></tr>")
            'Response.Write("<tr bgcolor=''><td>Available leave days</td><td>" & intavl.ToString & "</td></tr></table><br>")
            'Response.Write(strout.Length - 3)
            If strout.Length > 3 Then
                    If strout.Substring(strout.Length - 3) = "spx" Then
                        Response.Redirect(strout)
                    Else
                        Response.Write(strout)
                    End If
                End If
                
                dt3.Close()
            Catch ex As Exception
                
                Response.Write(ex.ToString & "==sorry")
                
            End Try
            fm = Nothing
            dbx = Nothing
            dt3 = Nothing
            
        %>  
       <script language="javascript" type="text/javascript">
       $("#messagebox").text('<% response.write(xmsg) %>');
       </script>  
      
       </body>
</html>
