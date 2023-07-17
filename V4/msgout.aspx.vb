Imports Kirsoft.hrm
Partial Class _msgout
    Inherits System.Web.UI.Page
    Function msgout()
        Dim sec As New k_security
        If Request.QueryString("msg") <> "" Then
            Response.Write(Request.QueryString("msg"))
        ElseIf Request.Form("msg") <> "" Then
            Response.Write(Request.Form("msg"))
        ElseIf Request.Form("hidpass") <> "" Then
            ' Response.Write("passed")
            Response.Write(sec.dbHexToStr(Request.Form("hidpass")))

        End If
    End Function
End Class
