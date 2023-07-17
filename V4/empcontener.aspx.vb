Imports Kirsoft.hrm
Imports System.Web
Imports System.Net
Imports System.IO
Partial Class empcontener
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim client As New WebClient()
        ' client.DownloadFile("https://kirsoft-pc:4433/reportnonactive.aspx?val=all", Session("path") & "\reportnon.html")
        ' client.DownloadFile("https://kirsoft-pc:4433/viewreport1.aspx?val=all&active=y", Session("path") & "\reportactive.html")

        'Response.Write(Session("path"))

    End Sub
End Class
