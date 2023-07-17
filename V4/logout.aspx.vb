
Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Abandon()
        Response.Redirect("login.aspx?" & Request.ServerVariables("QUERY_STRING"), True)
        Response.RedirectLocation = ("login.aspx?" & Request.ServerVariables("QUERY_STRING"))
    End Sub
End Class
