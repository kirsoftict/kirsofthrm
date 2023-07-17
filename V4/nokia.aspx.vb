Imports System.Net.Mail
Imports System.Web.Mail
Imports Kirsoft.hrm
Imports SMS3ASuiteLib
Imports System.IO.Ports


Partial Class nokia
    Inherits System.Web.UI.Page
    Function mailthis(ByVal fromc As String, ByVal toc As String, ByVal subj As String, ByVal msg As String)
        Dim email As New mail_system
        Dim msgs As Object
        msgs = email.mailprep(toc, fromc, msg, subj)
        msgs = email.mailsend(msgs)
        Response.Write(msgs.ToString)
    End Function
    Function nokia_sms()
        Dim SMSPort = New SerialPort

        With SMSPort
            .PortName = "COM24"
            .BaudRate = 9600
            .Parity = Parity.None
            .DataBits = 8
            .StopBits = StopBits.One
            .Handshake = Handshake.None
            .DtrEnable = True
            .RtsEnable = True

            
        End With

        SMSPort.Open()

        SMSPort.ReadLine("AT" & Chr(13))
       
        SMSPort.ReadLine("AT+CMGF=1" & Chr(13))
        
        SMSPort.ReadLine("AT+CMGS=" & Chr(34) & textbox1.Text & Chr(34))
        ' SMSPort.WriteLine( & Chr(26))
       
        Response.Write(SMSPort.ReadExisting())

        SMSPort.Close()
    End Function
End Class
