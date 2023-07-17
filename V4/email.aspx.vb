Imports System.Data
Imports System.Net.Mail
Imports Kirsoft.hrm
Imports System.Data.SqlClient
Imports System.IO


Partial Class email
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Form("msg") <> "" Then
            Dim mil As New mail_systema
            mil.init(Request.QueryString("msg"), Request.QueryString("subj"), Session("eto"), Session("efrom"), Session("eport"), Session("con"), Session("path"), Session("smpt"), Session("eport"), Session("epwd"))
            Response.Write(Request.QueryString("msg"))
            If mil.isemailexist() = False Then
                mil.sendemail()
            End If
        End If

    End Sub
End Class
Class mail_systema

    Private emsg As String
    Private subj As String
    Private tos As String
    Private froms As String
    Private port As String
    Private con As SqlConnection
    Private pathx As String
    Private smtpemail As String
    Private smtp As String
    Private passwordx As String
    Dim sec As New k_security
    Dim fm As New formMaker
    Dim dbs As New dbclass
    Public Function init(ByVal emsgx, ByVal subjx, ByVal tosx, ByVal fromsx, ByVal portx, ByVal conx, ByVal pathh, ByVal smtpx, ByVal smtpemailx, ByVal passwordxx)
        emsg = emsgx
        subj = subjx
        tos = tosx
        froms = fromsx
        port = portx
        con = conx
        pathx = pathh
        smtp = smtpx
        smtpemail = smtpemailx
        passwordx = passwordxx
    End Function
    Public Function isemailexist()




        If fm.getinfo2("select mailid from tblemail where mailcont='" & sec.StrToHex3(emsg) & "' and senddate='" & Today.ToShortDateString & "'", con) = "None" Then
            Return False

        Else
            Return True
        End If

    End Function
    Function mailreg()
        Dim flname As String

        flname = sec.StrToHex3(Now.ToString)

        Dim sql As String = "insert into tblemail(mailid,maildatetime,sendrpt,subject,mailcont) values('" & flname & _
                   "','" & Now.ToString & _
                   "','unsend" & _
                   "','" & subj & "','" & _
                   sec.StrToHex3(emsg) & "')"
        dbs.excutes(sql, con, pathx)
    End Function



    Function sendemail()
        Try
            Dim Smtp_Server As New SmtpClient(smtp, port)

            Dim e_mail As New MailMessage()
            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential(smtpemail, passwordx)
            ' Smtp_Server.Port = 587

            Smtp_Server.EnableSsl = True
            e_mail = New MailMessage()
            e_mail.From = New MailAddress(froms)
            e_mail.To.Add(tos)
            e_mail.Subject = subj
            e_mail.IsBodyHtml = True
            e_mail.Body = emsg
            Smtp_Server.Send(e_mail)

            Dim sql As String = "update tblemail set sendrpt='sent', senddate='" & Now.ToString & "' where mailcont='" & sec.StrToHex3(emsg) & "'"
            dbs.excutes(sql, con, pathx)
            Return "msg sent"
        Catch error_t As Exception
            If File.Exists("C:\temp\email\email.txt") Then
                Try
                    emsg = Now.ToString & error_t.ToString & ">>>" & emsg & ">>>to:" & tos & ">>>>>>>>>>>>>>>\n"
                    File.AppendAllText("C:\temp\email\email.txt", emsg)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

            End If
            Return "not sent because " & error_t.ToString
        End Try
    End Function
End Class
