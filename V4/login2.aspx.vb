Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports System.Security.AccessControl
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Partial Class login2
    Inherits System.Web.UI.Page
    Public Function checklogin(ByVal uname As String, ByVal pwd As String) As String
        Dim rs As DataTableReader
        Dim dt As New dbclass
        Dim msg As String = ""

        If uname <> "" And pwd <> "" Then
            Dim sql As String = "select * from login where username='" & uname & "' and password='" & pwd & "'"
            rs = dt.dtmake("login", sql, session("con"))
            If rs.HasRows = True Then
                rs.Read()
                Session("right") = rs.Item("auth")
                Session("who") = rs.Item("Id")
                Session("password") = rs.Item("password")
                Session("username") = rs.Item("username")
                Session("emp_iid") = rs.Item("emp_id")
                Response.Redirect("home.aspx")

            Else
                msg = "Username or Passward is incorrect"
            End If
        End If
        Return msg
    End Function
End Class
