Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports System.Security.AccessControl
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Partial Class loginx
    Inherits System.Web.UI.Page
    Public Function checklogin(ByVal uname As String, ByVal pwd As String) As String
        Dim rs As DataTableReader
        Dim dt As New dbclass
        Dim msg As String = ""
        Dim fm As New formMaker

        Try

            If Session("con").state = ConnectionState.Closed Then
                Session("con").open()
            End If
            If uname <> "" And pwd <> "" Then
                Dim sql As String = "select * from login where username='" & uname & "' and password='" & pwd & "'"
                rs = dt.dtmake("login" & Today.ToLongDateString, sql, Session("con"))
                If rs.HasRows = True Then
                    rs.Read()
                    If rs.Item("password") <> pwd Then
                        msg = "Password is incorrect"
                    Else
                        Session("right") = rs.Item("auth")
                        Session("who") = rs.Item("Id")
                        Session("password") = rs.Item("password")
                        Session("username") = rs.Item("username")
                        If rs.IsDBNull(1) = True Then
                            Session("emp_iid") = "Admin"
                        ElseIf rs.Item(1) = "" Then

                            Session("emp_iid") = "Admin"
                        Else
                            Session("emp_iid") = rs.Item("emp_id")

                        End If
                        If IsError(Application("count")) = True Then
                            Application("count") = 1
                        ElseIf IsNumeric(Application("count")) = False Then
                            Application("count") = 1
                        Else
                            Application("count") = Application("count") + 1

                        End If
                        Response.Redirect("home.aspx")
                    End If


                Else
                    msg = "Username or Passward is incorrect"
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
            Try
                If Session("con").state = ConnectionState.Broken Then
                    Response.Write("<br>connection BRoken")
                ElseIf Session("con").state = ConnectionState.Closed Then
                    Response.Write("<br>Connection Closed")
                ElseIf Not Session("con") Then
                    Response.Write("<br>session Is not set")

                End If
            Catch ex2 As Exception
                Response.Write("<br>Error2" & ex2.ToString)
            End Try
        End Try
        Return msg
    End Function
    Public Function CheckBrowserCaps()

        Dim labelText As String = ""
        Dim myBrowserCaps As System.Web.HttpBrowserCapabilities = Request.Browser
        If (CType(myBrowserCaps, System.Web.Configuration.HttpCapabilitiesBase)).IsMobileDevice Then
            labelText = "Browser is a mobile device."
        Else
            labelText = "Browser is not a mobile device."
        End If

        Return labelText

    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Session("con").open()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Session("url") = "kirsoft:8015"
        'Application("") = "+251 911 479143, P.o.Box:23932"
        Application("logo") = "images/netlog.gif"
        Application("company_name") = "Net Consult Consulting Engineers and Architects P.L.C"
        Application("company_name_amharic") = "ኔት ኮንሰልት አማካሪ መሃንዲሶችና አርክቴክቶች ሃ/የተ/የግ/ማ."
        Application("worda") = ""
        Application("kebele") = ""
        Application("hno") = ""
        Application("tin") = "00000"

        Me.ccx.Text = "<a href='http://www.kirsoftet.com' target='_blank'>kirSoft</a> ver 1 &copy; kirSoft cp. " & Today.Year.ToString & " All rights reserved."
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete


    End Sub
End Class
