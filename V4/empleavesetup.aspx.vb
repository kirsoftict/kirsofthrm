
Partial Class empleavesetup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("con").state = Data.ConnectionState.Closed Then
            Response.Write("database is closed")
            Session("con").open()
        End If
    End Sub
End Class
