<%@ Page Language="VB" AutoEventWireup="false" CodeFile="leaveform.aspx.vb" Inherits="leaveform" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  If Session("username") = "" Then
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
ElseIf Session("emptid") = 0 Then

     %>

<script type="text/javascript">
alert('<% response.write(session("emptid")) %>');
</script>

<%    
End If
    Dim fullname As String = ""
    Dim leavetype As String = ""
    Dim leavsdate As Date
    Dim leavedate As Date
    Dim leavrdate As Date
    Dim rdays As Double = 0
    Dim fm As New formMaker
    fullname = fm.getfullname(Session("emp_id"), session("con"))
    Dim dbx As New dbclass
    Dim dt As DataTableReader
    Dim position As String
    Dim hiredate As String
position = fm.getinfo2("select position from emp_job_assign where emptid=" & Session("emptid") & " order by id desc", session("con"))
    hiredate = fm.getinfo("hire_date", "emprec", " id=" & Session("emptid"), session("con"))
    If Request.QueryString("id") <> "" Then
        
    'Dim dt1, dt2 As DataTableReader
    If Request.QueryString("id") <> "None" Then
        dt = dbx.dtmake("dt1", "select *from emp_leave_take where id=" & Request.QueryString("id"), session("con"))
        If dt.HasRows Then
            dt.Read()
            leavsdate = dt.Item("date_taken_from")
            leavrdate = dt.Item("date_return")
            leavedate = leavrdate.AddDays(-1)
            rdays = dt.Item("no_days")
            leavetype = dt.Item("leave_type")
        End If
        dt.Close()
        dt = dbx.dtmake("leaveinfo", "Select * from empleavapp  where req_id=" & Request.QueryString("id"), session("con"))
        If dt.HasRows Then
        End If
        dt.Close()
    End If
End If
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
    <title>Untitled Page</title>
    <script type="text/javascript" language="javascript">
    function print(loc,title,head,footer)
    {
    var printFriendly = document.getElementById(loc);
    var printstyle=document.getElementById("priv");
    var printWin = window.open("print.aspx",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<meta http-equiv='Content-Type' content='application/msword'><style>" + printstyle.innerHTML + "</style><div class='book'><div class='page'><div class='subpage'>" + printFriendly.innerHTML + "</div></div></div>");
    printWin.document.close();
    printWin.window.print();   
  // printWin.close();
    }
   function printx(loc)
   {
        var frm=document.getElementById("ppr");
        var pf=document.getElementById(loc);
        frm.action="previewpring.aspx?send="+pf;
        frm.submit();
   
   }
    </script>
    
    <style id='priv' type="text/css">
        body {
        margin: 0;
        padding: 0;
        background-color: #FAFAFA;
        font: 9pt;
    }
    * {
        box-sizing: border-box;
        -moz-box-sizing: border-box;
    }
    .page {
        width: 21cm;
        min-height: 27.7cm;
        padding: 0.5cm;
        margin: 0.5cm auto;
        border: 1px #D3D3D3 solid;
        border-radius: 1px;
        background: white;
        box-shadow: 0 0 1px rgba(0, 0, 0, 0.1);
    }
    .subpage {
        padding: 0.5cm;
        border: 1px red solid;
        height: 260mm;
        outline: 1cm #FFEAEA solid;
    }
    
    @page {
        size: A4;
        margin: 0;
    }
    @media print {
        .page {
            margin: 0;
            border: initial;
            border-radius: initial;
            width: initial;
            min-height: initial;
            box-shadow: initial;
            background: initial;
            page-break-after: always;
        }
    }
    </style>
</head>
<body style="font-size:9pt">
    <div id="leaveform" style="height:800px;" >
    <p><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; </strong>
    <strong>Date: </strong><span style="text-decoration:underline;">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
    <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong><strong>&nbsp;</strong><strong>&nbsp;</strong><strong>Ref. No ..</strong><span style=" text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
    <strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong></p>
<p align="center"><strong><span style="text-decoration: underline; font-size:14pt;">
LEAVE REQUEST FORM</span></strong></p>
<strong>I hereby request to be granted annual leave specified below:- </strong>
<br /><br />
<strong>1.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>Name of employee:</strong>
<span style="text-decoration:underline;">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% response.write(fullname) %>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 
&nbsp;&nbsp;<strong> position</strong><span style=" text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% response.write(position) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
<br /><strong>ID NO.</strong>
<span style="text-decoration:underline;">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% response.write(session("emp_id")) %>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
<br /><br />
<strong>2.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>Type of leave requested :-</strong>
<span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<% response.write(leavetype) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
<br /><br /><strong>3.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong>
Number of working days leave requested:</strong> <span style="text-decoration:underline;">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% response.write(rdays) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
<br /><strong>Starting date of leave:</strong>
<span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<% Response.Write(leavsdate.ToShortDateString)%> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
<p><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;Tel. No. while on leave </strong><span style="text-decoration: underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% Response.Write(fm.getinfo2("select mob from emp_address where emp_id='" & Session("emp_id") & "'", session("con")))%>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;</span></p>
<br><strong>Signature of applicant ------------------------&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date &nbsp;------------------------</strong>
<br /><strong>4.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong><span style="text-decoration: underline;">Approval of employee&rsquo;s immediate supervisor </span></strong>
<p><strong>Supervisor&rsquo;s Comment -------------------------------------------------------------------</strong></p>
<p><strong>Supervisor&rsquo;s Name --------------------------------------- Signature &amp; Date ------------------------</strong></p>
<strong>5.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong><strong><span style="text-decoration: underline;">
To be completed by Human Resource Adm. Head </span></strong><%
         Dim vew(3, 3) As String
                                                                vew = fm.view_leave_bal(Session("company_name"), Session("emptid"), Session("emp_id"), Session("con"))
 %>
<br /><strong>Employment date: </strong><span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;<% response.write(hiredate) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 
<br /><strong>No of leave days Accrued &amp; transferred with approval:</strong><span style="text-decoration:underline;"><% response.Write(vew(2,0)) %> &nbsp;&nbsp; <% Response.Write(Math.Round(CDbl(vew(1, 0)), 2))%> </span><br />
<strong>No of leave days Accrued &amp; transferred with approval:</strong><span style="text-decoration:underline;"><% response.Write(vew(2,1)) %> &nbsp;&nbsp; <% Response.Write(Math.Round(CDbl(vew(1, 1)), 2))%> </span><br />
<strong>No of leave days Accrued &amp; transferred with approval:</strong><span style="text-decoration:underline;"><% response.Write(vew(2,2)) %> &nbsp;&nbsp; <% Response.Write(Math.Round(CDbl(vew(1, 2)), 2))%> </span><br />
<% dim caldays as double
    caldays = CInt(vew(1, 0)) + CInt(vew(1, 1)) + CInt(vew(1, 2))
 %>
<p><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total entitlement&nbsp; <span style="text-decoration: underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% Response.Write(caldays.ToString)%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span> days.</strong></p>
<p><strong>Less number of leave days approved; - </strong> 
<span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<% Response.Write(rdays)%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></p>
<p><strong>Current Annual leave balance:- </strong> 
<span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%  Dim calcrdays As Double
    Dim balshow As Double = 0
    'calcrdays = caldays - CInt(vew(1, 0))
    If vew(1, 0) <= vew(0, 2) Then
        balshow = vew(0, 2) - vew(1, 0)
        Response.Write(vew(2, 0) & "- Balance:" & balshow)
       
    Else
        
        balshow = Math.Round((CDbl(vew(0, 2) - CDbl(vew(1, 0)))))
        Response.Write(vew(2, 0) & "- Balance:" & (balshow * -1).ToString)
      
    End If
    Response.Write(", ")
    If balshow >= 0 Then
        If vew(1, 1) <= balshow Then
            balshow = Math.Round(balshow - CDbl(vew(1, 1)), 2)
            Response.Write(vew(2, 1) & "- Balance:" & balshow.ToString)
           
        Else
            balshow = Math.Round(balshow - CDbl(vew(1, 1)))
            Response.Write(vew(2, 1) & "- Balance:" & Math.Round((balshow * -1), 2).ToString)
           
        End If
    Else
        Response.Write(vew(2, 1) & "- Balance:" & Math.Round(CDbl(vew(1, 1)), 2).ToString)
        'Response.Write(" " & balshow.ToString & "<br>")
    End If
    ' Response.Write(", ")
    If balshow > 0 Then
        If vew(1, 2) <= balshow Then
            balshow = balshow - vew(1, 2)
            Response.Write(vew(2, 2) & "- Balance:" & Math.Round(CDbl(balshow), 2).ToString)
            
        Else
            balshow = Math.Round((CDbl(vew(1, 2)) - balshow), 2)
            Response.Write(vew(2, 2) & "- Balance:" & Math.Round(CDbl(balshow), 2).ToString)
           
        End If
    Else
        Response.Write(vew(2, 2) & "- Balance:" & Math.Round(CDbl(vew(1, 2)), 2).ToString)
    End If
   %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></p>
<p><strong>Starting date of leave</strong> <span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% response.write(leavsdate.ToShortDateString) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><strong> end date of leave </strong><span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% Response.Write(leavedate.ToShortDateString)%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></p>
<p><strong>From date</strong><span style="text-decoration:underline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% Response.Write(leavrdate.ToShortDateString)%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><strong> employee has to be on duty.</strong></p>
<p><strong>Checked by Head, HRA Sec. -&nbsp; ---------------------------------- -Signature &amp; Date:&nbsp; ---------------------</strong></p>
<p><strong><span style="text-decoration: underline;">CC.</span></strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>TO Mr/miss &nbsp;---------------------------------------------------------------------</strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>To General Manager</strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>To Administration &amp; Finance</strong></p>
<p>-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>To Personal File </strong></p>
<p>&nbsp;</p>
    </div>
           <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('leaveform','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico' alt="print"/>print</div>        
<form id="ppr" name="ppr" method="post" action="">
    
</form>
            <%    
                If Request.QueryString("id") = "None" Then
                    %>
                   
                        <div style="opacity:0.9;
	filter:alpha(opacity=90); 
	background:#bcd9eb; 
	left:0px; 
	top:0px; 
	width:100%; 
	height:1900px; 
	text-align:center;
	vertical-align:middle; 
	position:absolute; overflow:hidden; font-size:78pt; color:Red;"> Sorry! No Leave information</div>
                    <%
                    End If
                    Response.Write(vew(0, 0))
               
%>

</body>
</html>
