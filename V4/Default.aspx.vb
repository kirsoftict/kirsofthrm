Imports System.Diagnostics
Imports System
Imports System.Security
Imports System.IO
Imports Kirsoft.hrm

Partial Class _Default
    Inherits System.Web.UI.Page
    Function promail()
        Dim mail As New mail_system
        Response.Write(mail.sendemail("text", Session("epwd"), Session("efrom"), "z.kirubel@gmail.com", "MMC"))
    End Function
    Function processx()
      
        Dim procStartInfo As New ProcessStartInfo
        Dim procExecuting As New Process
        Dim secstr As New System.Security.SecureString
        secstr.AppendChar("@")
        secstr.AppendChar("m")
        secstr.AppendChar("e")
        secstr.AppendChar("#")
        secstr.AppendChar("1")
        secstr.AppendChar("2")
        secstr.AppendChar("3")

        ' Process.Start("C:\TEMP\backup.bat", "kirubelz", secstr, "KIRUBEL")

        With procStartInfo
            .UseShellExecute = False

            .FileName = "excel.exe"
            .WindowStyle = ProcessWindowStyle.Normal

            'add this to prompt for elevation
            .UserName = "kirubelz"
            .Password = secstr

        End With
        Try
            'procExecuting = Process.Start(procStartInfo)
            '    Response.Write(procExecuting.StandardOutput.ReadLine)
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try




    End Function
End Class
