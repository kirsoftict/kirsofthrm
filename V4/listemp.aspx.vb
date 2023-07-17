Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Partial Class listemp
    Inherits System.Web.UI.Page

    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("con").state = Data.ConnectionState.Closed Then
            Response.Write("database is closed")
            Session("con").open()
        End If
    End Sub
End Class
