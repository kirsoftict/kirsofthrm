
Partial Class Default3
    Inherits System.Web.UI.Page
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.AppendHeader("Content-Type", "application/msword")
        Response.AppendHeader("Content-disposition", _
                               "attachment; filename=sample.doc")
    End Sub
End Class
