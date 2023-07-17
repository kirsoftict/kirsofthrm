<%@ Page Language="VB" AutoEventWireup="false" CodeFile="sqlselectview.aspx.vb" Inherits="sqlselectview" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <div>
    <form id="sql" name='sql' method="post" action="">
   
        <textarea rows="10" cols="100" id="sqlst" name="sqlst"></textarea>
        <br /> <input type="text" id="con" name="con" /><input type="submit" value="EXC" name="exc" />
    </form>
    </div>
</body>
</html>
<% Dim con As String = "con"
    If Request.Item("con") <> "" Then
        con = Request.Item("con")
        
    End If
    If Request.Item("sqlst") <> "" Then
        Dim dbx As New dbclass
        Dim fl As New file_list
        Dim res As String = ""
        Dim rs As DataTableReader
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
        Try
            If con = "con" Then
                Try
                    rs = dbx.dtmake("viewst", fsql, Session("con"))
                    If rs.HasRows Then
                        getout(rs)
                    End If
                Catch ex As Exception
                    Response.Write(ex.ToString & fsql)
                End Try
              
                
            ElseIf con = "conr" Then
                Try
                    rs = dbx.dtmake("viewst", fsql, Session("conr"))
                    If rs.HasRows Then
                        getout(rs)
                    End If
                Catch ex As Exception
                    Response.Write(ex.ToString & fsql)
                End Try
            End If
            'Response.Write(res.ToString)
        Catch ex As Exception
            Response.Write("connection already open<br>")
        End Try
        Response.Write("<br>" & res.ToString)
       
    End If
 %>
