Imports System.Data
Imports Kirsoft.hrm
Imports System.Web.Mail
Imports System.IO
Partial Class emailcreater
    Inherits System.Web.UI.Page
    Function createMail()
        Dim db As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim sql As String = "select distinct inf.emp_id,eja.department from emp_static_info as inf left join emprec as ec on inf.emp_id=ec.emp_id left join emp_job_assign as eja on eja.emptid=ec.id where ec.end_date is null order by eja.department, inf.emp_id"
        'Response.Write(sql)
        Dim msgque() As String
        Dim tbl, tbl2, email, password As String
        Response.Write("<table border=1>")
        ' tbl2 = "<table border=1>"
        Dim pemail As String
        Dim insertwemail As String
        Dim i As Integer = 0
        rs = db.dtmake("email", sql, Session("con"))
        Dim dup, name As String
        dup = ""
        name = ""
        Dim pushemail() As String = {""}
        Dim fname As String
        If rs.HasRows Then
            While rs.Read
                fname = ""
                'email = rs.Item(1) & "." & rs.Item(2) & "@netconsult.com.et"
                fname = fm.getfullname(rs.Item(0), Session("con"))
                email = fname.Split(" ")(0) & "." & fname.Split(" ")(1)
                email = email.Replace("/", ".")
                email = email & "@netconsult.com.et"
                ' Response.Write(Array.BinarySearch(pushemail, email) & "<br>")
                If fm.searcharray(pushemail, email) = False Then

                    ReDim Preserve pushemail(i + 1)
                    pushemail(i) = email
pass:               password = getpassword()
                    If dup <> password Then
                        dup = password
                    Else
                        GoTo pass
                    End If
                    Response.Write("<tr><td>" & rs.Item(0) & "</td><td>" & email & "</td><td>" & password & "</td><td>100</td><td>" & rs.Item(1) & "</td><td><a href='http://neteth.ddns.net/email/outbox/" & password & ".htm' target='blank'>click print</a></td></tr>")
                    ' tbl2 &= "<tr><td>" & email & " </td><td>" & Request.Form("forward") & "</td>"
                    ReDim Preserve msgque(i + 1)
                    msgque(i) = createmsg(fname.Split(" ")(0), fname.Split(" ")(1), email, password)
                    pemail = fm.getinfo2("select pemail from emp_address where emp_id='" & rs.Item(0) & "'", Session("con"))
                    File.WriteAllText(Session("path") & "/email/outbox/" & password & ".htm", pemail.ToString & vbNewLine & msgque(i))

                    i = i + 1
                End If
            End While
            Response.Write("</table>")
            ' tbl2 &= "</tble>"
            Response.Write("<br> " & tbl2)

        End If
    End Function
    Function getpassword()
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 8
            Dim idx As Integer = r.Next(0, 35)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString
    End Function
    Function createmsg(ByVal fname As String, ByVal sname As String, ByVal email As String, ByVal pwd As String)
        Dim msg As String
        msg = "<div id='msg' style='max-width:21cm;border:1px solid #0fa;'><p>Dear " & fname & vbNewLine & "</p>"
        msg &= "<p> We are created an offical email address that helps you and us for a formal communication entire the organization and behalf of the company to other venders and customers.</p>"
        msg &= "<table><tr><td>Your Email</td><td>" & email & "</td></tr><tr><td>Password</td><td>" & pwd & "</td></tr></table></p>"
        msg &= "<p>Thank you for using the services<br>ICT Team</p>"
        msg &= "<h3>How to configure your phone and outlook</h3>"
        msg &= "Direct accessing link <a href='https://netconsult.com.et:2096/' target='blank'>Click</a><br> "
        msg &= configuration(fname & "." & sname) & "</div>"
        Return msg
    End Function
    Function configuration(ByVal em As String)

        Dim rtn As String = ""
        If File.Exists(Session("path") & "/text/emailconf.htm") Then
            rtn = File.ReadAllText(Session("path") & "/text/emailconf.htm")
            rtn = rtn.Replace("administrator", em)
        End If
        Return rtn
    End Function
    Function sendemail()
        Dim rtn As String = ""
        Dim fs As New file_list
        Dim ml As New mail_system
        Response.Write(Request.Item("send") & "<br>" & Session("path") & "\email\outbox ===> File size: " & fs.fileno(Session("path") & "\email\outbox"))
        ' Response.Write(Directory.GetFiles((Session("path") & "/email/outbox/") )
        If fs.fileno(Session("path") & "\email\outbox") > 0 Then
            If Request.Item("send") = "true" Then
                Response.Write("send is heare but let me check")
                ' rtn = (fs.filelist(Session("path") & "\email\outbox/"))

            Else
                Dim arrx() As String
                arrx = fs.listfile(Session("path") & "\email\outbox/")

                rtn = "<button onclick='javascript:send();'>snd</button>"
                For j As Integer = 0 To UBound(arrx) - 1 Step 1
                    'Response.Write("===>" & arrx(j) & "<br>")
                    Response.Write("<div class='page' style='overflow:hide;'><div class='subpage'>" & File.ReadAllText(arrx(j)) & "</div></div>")
                   
                Next
            End If
        End If
        Response.Write(ml.sendemail("msg", "@netpass#@!4", "info@netconsult.com.et", "z.kirubel@gmail.com", "test", "mail.netconsult.com.et", 465))
        Return rtn
    End Function
End Class
