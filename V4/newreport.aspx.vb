Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm

Partial Class newreport
    Inherits System.Web.UI.Page


    Public Function pageaddnew() As String

        Dim sql As String
        sql = "select * from " & Request.QueryString("dtable")
        If Request.QueryString("dtable") <> "" Then
            Dim cm As New menubuilder
            Dim rs As SqlDataReader
            Dim formx As String = ""
            rs = cm.cmdx(session("con"), sql)
            Dim icount, i As Integer
            icount = rs.FieldCount
            formx = "<form method='post' id='frmpage_" & Request.QueryString("dtable") & "' name='frmpage_" & Request.QueryString("dtable") & "' enctype='multipart/form-data' action='dbadmin.aspx?updated=on&dtable=" & Request.QueryString("dtable") & "'>"
            formx = formx & "<table>"
            For i = 0 To icount - 1
                If i = 0 Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                    "<input type='hidden' name='" & rs.GetName(i) & "' value=''><b></b></td></tr>"
                ElseIf LCase(rs.GetDataTypeName(i)) = "text" Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                         "<textarea id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & "'>"
                    formx = formx & "</textarea></td></tr>"


                ElseIf LCase(rs.GetDataTypeName(i)) = "datetime" Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                                              "<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                                              "' value='" & Date.Today() & "'>" & Date.Today() & _
                                                        "<script type='text/javascript'>" & _
     "$(function () {" & _
        " $(" & Chr(34) & "#" & rs.GetName(i) & Chr(34) & ").datepicker({ minDate: " & Chr(34) & "-20Y" & Chr(34) & ", maxDate: " & Chr(34) & "+20Y" & Chr(34) & " });" & _
        "   });" & _
 "</script>" & _
                                                             " </td></tr "
                Else
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                                            "<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                                       "' value=''></td></tr>"


                End If
            Next
            formx = formx & "<tr><td><button name='btnupdate' value='update'>Save</button></table></form>"
            formx = formx & "<br />Uploads: images video"
            rs.Close()
            cm = Nothing
            Return formx
        End If
        Return ""
    End Function
End Class

