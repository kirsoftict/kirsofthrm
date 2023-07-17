Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class empinproj
    Inherits System.Web.UI.Page
    Function maketable(ByVal y As String, ByVal m As String)
        Dim nod As Integer = 0
        nod = Date.DaysInMonth(y, m)
        Dim dp As Date
        dp = "#" & m & "/" & nod & "/" & y & "#"
        Dim rs As DataTableReader
        Dim db As New dbclass
        Dim fm As New formMaker
        Dim list() As String
        Dim rt() As String
        Dim empid As String
        rs = db.dtmake("dbproj", "select * from tblproject", Application("con"))
        Dim projid As String
        If rs.HasRows Then
            While rs.Read
                projid = rs.Item("project_id")
                rt = fm.getprojemp_no(projid, dp, Session("con"))
                If IsNumeric(rt(1)) Then
                    If CInt(rt(1)) > 0 Then

                        Response.Write("Project Name:" & rs.Item("project_name") & " No. Emp:" & rt(1) & "<br>")
                        list = rt(0).Split(",")
                        For k As Integer = 0 To list.Length - 1

                            Response.Write((k + 1).ToString & "." & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & list(k), Session("con")), Session("con")) & " Active==" & fm.getinfo2("select active from emprec where id=" & list(k), Session("con")) & "<br>")

                        Next

                    End If
                End If
            End While
        End If

    End Function
End Class
