<%@ Page Language="VB" AutoEventWireup="false" CodeFile="jqueryupdate.aspx.vb" Inherits="jqueryupdate" %>
<%@ Import Namespace="kirsoft.hrm" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<script type="text/javascript">
function redirect(where)
{

}
</script>
</head>
<body>
    <%  Dim flg As Integer = 0
        Dim msg As String = ""
        Dim what_task As String = Request.QueryString("what_task")
        Dim rd As String = ""
        Dim fl As New file_list
        'Response.Write(what_task) 
        Dim fm As New formMaker
        Dim db As New dbclass
        Select Case what_task
            Case "calc"
                ' msg = fm.calc2(Session("con"), Session("emp_id"), Session("emptid"), Session("path"))
                ' If msg.Substring(msg.Length - 3) = "spx" Then
                'rd = msg + (Session("emptid"))
                ' End If
            Case "leaveview"
                Dim vew(3, 3) As String
                
                vew = fm.view_leave_bal(Session("companyname"), Session("emptid"), Session("emp_id"), Session("con"))
                msg = vew(0, 0)
                
            Case "approved"
              
                If Request.QueryString("tbl") = "emp_leave_budget" Then
                    Dim tbl As String = Request.QueryString("tbl")
                    Dim key As String = Request.QueryString("key")
                    Dim keyval As String = Request.QueryString("keyval")
                    rd = Request.QueryString("rd") & ".aspx"
                    Dim sql As String = "update " & tbl & " set approved='" & Today.Month.ToString & "/" & Today.Day.ToString & "/" & Today.Year.ToString & " " & Now.Hour.ToString & ":" & Now.Minute.ToString & ":" & Now.Second.ToString & "', who_approve='" & Request.QueryString("who_approved") & "' where " & key & "=" & keyval
                    '    Response.Write(sql)
                    If db.save(sql, Session("con"), Session("path")) = 1 Then
                        msg = "success"
                        flg = 1
                    Else
                        msg = ("Sorry no change")
                    End If
                End If
            Case "upload"
                Response.Write("we in")
                Dim floc, fttype(), emp_id, re As String
                floc = Request.QueryString("dest")
                emp_id = Request.QueryString("empid")
                fttype = Request.QueryString("ftype").Split(",")
                Response.Write(Server.MapPath("employee") & "\" & emp_id & "\" & floc & "\")
                re = fl.fupload(Request.Files("flupload"), Server.MapPath("employee") & "\" & emp_id & "\" & floc & "\", 200000, fttype)
                msg = re
            Case Else
                msg = "do nothing"
        End Select
        Response.Write("<div style='width:300px;'>" & msg & "</div>")
    
        fm = Nothing
        db = Nothing
        fl = Nothing
     %>
      <form id="frmx" name="frmx" action="" method="post">
    </form>
  <%
      If rd <> "" Then
          %>
          <script language="javascript" type="text/javascript">
                $("#frmx").attr("target","workarea");
                 $("#frmx").attr("action","<% response.write(rd & "?msg=" & msg) %>");
                  $("#frmx").submit()
                
          </script>
          <%
      End If
   %>  
   
</body>
</html>

<script language="javascript" type="text/javascript">
$(document).ready(function() {

// $('#frmx').delay(4000);
       
       
window.close();
});
</script>

