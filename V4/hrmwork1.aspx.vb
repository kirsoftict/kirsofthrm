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
Partial Class hrmwork1
    Inherits System.Web.UI.Page
    Function formmake(ByVal id As String, ByVal cont As String, ByVal width As Integer, ByVal height As Integer, ByVal Title As String)
        Dim str As String
        str = "<div id=" & Chr(34) & CStr(id) & Chr(34) & " class='formmake' style='width:" & width & "px;height:auto;'> " & _
        "<div style=" & Chr(34) & "height:30px; background:url(images/blue_banner-760x147.jpg); vertical-align:top;top:-10px;" & Chr(34) & _
           ">" & Chr(13) & _
           "<div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:75%; float:left; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
           "><b>" & CStr(Title) & "</b></div>" & Chr(13) & "<div style=" & Chr(34) & "cursor:pointer; text-align:left; height:30px; width:22px; background-image:url(images/gif/x.gif); float:left;" & _
           Chr(34) & " title='close'  onclick=" & Chr(34) & "javascript: document.getElementById('" & CStr(id) & _
           "').style.visibility='hidden';" & Chr(34) & _
           ">&nbsp;close </div></div>" & Chr(13) & _
      "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%;height:100%; overflow:no; font-size:10pt; color:blue;" & Chr(34) & _
      ">&nbsp;&nbsp;" & CStr(cont) & "</div></div>" & _
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
