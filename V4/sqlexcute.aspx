<%@ Page Language="VB" AutoEventWireup="false" CodeFile="sqlexcute.aspx.vb" Inherits="sqlexcute" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <div>
    <form id="sql" name='sql' method="post" action="">
        <textarea rows="30" cols="100" id="sqlst" name="sqlst"></textarea>
        <br /><input type="submit" value="EXC" name="exc" />
    </form>
    </div>
</body>
</html>
<%
    Try
        Session("con").open()
    Catch ex As Exception

    End Try
    If Request.Item("sqlst") <> "" Then
        Dim dbx As New dbclass
        Dim fl As New file_list
        Dim res As String = ""
        Dim Sql As String = "drop TABLE emp_test_one"
        Dim fsql As String
        'Response.Write(Server.MapPath(""))
        fsql = Request.Item("sqlst")
        'Response.Write(fsql)
        ' Dim arr() As String
        'fsql = fl.getfilecontx(Server.MapPath("") & "\emp_job_assign_final.sql")
        'arr = fl.getfcontline(Server.MapPath("") & "\emp_job_assign_final.sql")
        'For i As Integer = 0 To arr.Length - 1
        'Response.Write(arr(i) & "<br>")
        ' Next
        Response.Write(fsql)
        
        res = dbx.excutes(fsql, Session("con"), Session("path"))
        Response.Write(res.ToString)
    End If
 %>