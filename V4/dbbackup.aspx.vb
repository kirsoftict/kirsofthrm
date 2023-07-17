Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Imports System.Diagnostics
'Imports SMS3ASuiteLib
Partial Class dbbackup
    Inherits System.Web.UI.Page

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("backup") = "on" Then
            '  Process.Start("C:\Users\kirubelz\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\kirsofthrmbkp") '"C:\Users\kirubelz\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\backupapp\backupapp.appref-ms")
        End If
        

    End Sub
End Class
