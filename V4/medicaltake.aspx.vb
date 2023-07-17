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
Imports Kirsoft.hrm
Partial Class medicaltake
    Inherits System.Web.UI.Page
    Function filesview(ByVal path As String, ByVal empid As String, ByVal folder As String, ByVal root As String) As String
        Dim loc As String = path & "\" & empid & "\" & folder & "\"
        'Dim f As Directory
        Dim up As New file_list
        Dim rtstr As String = ""
        root &= "/" & empid & "/" & folder
        If Directory.Exists(loc) = True Then

            Dim ext As String = ""
            Dim fname As String = ""
            'rtstr = "what  what..." & loc
            For Each d As String In Directory.GetDirectories(loc)
                rtstr &= "<div title='' style='display block; clear:both;'>"
                Dim fld() As String
                fld = d.Split("\")
                rtstr &= "<span style='background:#243772;font-size:15pt; color:white;'>Directory: " & fld(fld.Length - 1) & "</span><br>"
                For Each k As String In Directory.GetFiles(d)
                    rtstr = rtstr & "<div style='display:inline; float:left; width:100px;'>" & _
                "<span style=' display:block'>"
                    Select Case up.file_ext(k).ToLower
                        Case ".doc", ".docx"
                            fname = "msword"
                        Case ".pdf"
                            fname = "pdf_icon"
                        Case Else
                            fname = "unknown"
                    End Select
                    Dim ff As String

                    ff = k.Replace("\", "~")
                    rtstr = rtstr & " <img src='images/gif/" & fname & ".gif' height='40px' width='40px' alt='" & up.findfilename(k) & "' title='" & up.findfilename(k) & "' />" & _
                        " <br /><span()>"
                    Dim fn As String = up.findfilename(k)
                    If fn.Length > 8 Then
                        fn = fn.Substring(0, 5) & "~." & up.file_ext(k)
                    End If
                    rtstr = rtstr & fn & "</span>" & _
           " <span><a href=" & Chr(34) & "javascript:del('" & ff & "','1st');" & Chr(34) & ">delete</a></span>&nbsp;&nbsp;&nbsp;<span>" & _
           "<a href=" & Chr(34) & root & "/" & fld(fld.Length - 1) & "/" & up.findfilename(k) & Chr(34) & ">View</a></span>" & _
        "</span></div>" & _
        "<div style='width:15px; float:left;'>&nbsp;</div>"
                Next
                rtstr &= "</div><div style='clear:both'>"
            Next
        Else
            rtstr = "<div style='height:75px;'>file doesnt found<br></div>"
        End If
        If rtstr = "" Then
            rtstr = "is empty"
        End If
        Return rtstr
    End Function
End Class
