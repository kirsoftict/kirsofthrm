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
Partial Class hrmworkeasy
    Inherits System.Web.UI.Page
    Function formmake(ByVal id As String, ByVal cont As String, ByVal width As Integer, ByVal height As Integer, ByVal Title As String)
        Dim str As String = ""
        ' // Response.Write(id & "<br>")
        str = "<span id=" & Chr(34) & CStr(id) & Chr(34) & " > " & _
            "<b>" & CStr(Title) & "</b>&nbsp;&nbsp;" & CStr(cont) & "</span>" & _
        Chr(13)
        Return str
    End Function
    Function getjavalist(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection, ByVal sp As String) As String
        Dim db As New dbclass
        Dim sql As String = "select " & dis & " from " & dbtabl
        Dim dt As DataTableReader
        Dim retstr As String = ""
        dt = db.dtmake("dbtbl" & Today.ToLongTimeString, sql, conx)
        Dim disp() As String
        Dim dispn As Integer = 0
        disp = dis.Split(",")
        Dim optdis As String = ""
        If disp.Length > 1 Then
            dispn = disp.Length
        End If
        If dt.HasRows = True Then
            While dt.Read
                retstr &= Chr(34)

                For i As Integer = 0 To disp.Length - 1
                    If dt.IsDBNull(i) = False Then
                        If LCase(dt.GetDataTypeName(i)) = "string" Then
                            retstr &= dt.Item(i).trim & sp
                        Else
                            retstr &= dt.Item(i) & sp
                        End If
                    End If

                Next
                retstr &= Chr(34)
                retstr &= ","
                'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
            End While
            retstr &= Chr(34) & "xx" & Chr(34)
        End If
        Return retstr
    End Function
End Class


