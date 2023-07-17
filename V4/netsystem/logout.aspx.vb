Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports System.Security.AccessControl
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim db As New dbclass

        Dim sql As String
        Try
            Dim pathext As String = "c:\temp\sessionexp.txt"
            Dim tictime As Object
            Dim flinf = New FileInfo(pathext)
            If CInt(flinf.Length) / 1024 > 2 Then
                tictime = Now.Ticks
                If File.Exists("c:\temp\" & tictime & "session.log") Then
                    File.WriteAllText("c:\temp\" & tictime & "session.log", File.ReadAllText(pathext))
                    File.WriteAllText(pathext, "")
                Else
                    File.AppendAllText("c:\temp\sessionexp.txt", Session.SessionID & "<========" & Now & vbNewLine)

                End If


            End If

            sql = "update chatroom set active='n' where ssid='" & Session.SessionID & "'"
            db.excutes(sql, Session("conx"), Session("path"))
            Session.Abandon()
         

        Catch ex As Exception
            If Session("path") <> "" Then
                db.writeerro(ex.ToString & vbNewLine & sql & vbNewLine)
                Response.Write("<script>window.location=login.aspx;</script>")
            Else
                Dim flinf As FileInfo
                flinf = New FileInfo("c:\temp\sessionout.txt")
                If CInt(flinf.Length) / 1024 > 2 Then
                    db.writeerro(ex.ToString & vbNewLine & sql & vbNewLine)
                Else
                    db.writeerro(ex.ToString & vbNewLine & sql & vbNewLine)
                End If
                Response.Write("<script>window.location=login.aspx;</script>")
            End If

        End Try




        Response.Redirect("../login.aspx?" & Request.ServerVariables("QUERY_STRING"), False)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Response.Redirect("../login.aspx")
        ' Response.Write("<script>window.location='login.aspx';</script>")
    End Sub
End Class
