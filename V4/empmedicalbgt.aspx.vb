Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Web.UI.Page
Imports System.Security
'Imports System.Web.SessionState.HttpSessionState
Imports System.IO
Imports System.Web.HttpServerUtility
Imports System.Web.HttpException
Imports System.Web.HttpRuntime
Imports System.Net.Mail
Imports System.Collections.Generic
Imports System.Text
Imports System
Imports Kirsoft.hrm
Partial Class empmedicalbgt
    Inherits System.Web.UI.Page
    Function dactive() As Boolean
        Dim rs As DataTableReader
        Dim db As New dbclass
        Dim fm As New formMaker
        Dim flg As Integer = 0
        Dim sql As String = "select * from emp_medical_all where emp_id='" & Session("emp_id") & "' and active='y' order by id desc"
        rs = db.dtmake("emp_medical", sql, session("con"))
        If rs.HasRows = True Then
            Dim dates As Date
            While rs.Read
                dates = rs.Item("date_exp")
                'Response.Write("====>" & fm.isexp(Today.ToShortDateString, dates, 1, "d").ToString)
                If fm.isexpforward(Today.ToShortDateString, dates, 1, "d") Then
                    sql = "update emp_medical_all set active='n' where id=" & rs.Item("id")
                    db.save(sql, session("con"), session("path"))
                    flg = 1
                End If
            End While
        End If
        rs.Close()
        rs = Nothing
        db = Nothing
        If flg = 1 Then
            Return True
        End If
        Return False
    End Function
End Class
