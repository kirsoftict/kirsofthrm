Imports Kirsoft.hrm

Partial Class getval
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        If Request.QueryString("gettype") = "single" Then
            get_single_data()
        ElseIf Request.QueryString("gettype") = "div_list" Then

            get_div_list()
        ElseIf Request.QueryString("gettype") = "page" Then
            get_single_data()
        Else
            Response.Write("it is else")

        End If

    End Sub
    Function get_single_data()
        Dim fild As String = ""
        Dim sql As String = ""
        Dim fm As New formMaker
        sql = Request.QueryString("sql")
        Response.Write(fm.getinfo2(sql, Session("con")))

    End Function

    Function get_div_list()

        Dim fm As New formMaker
        Dim row As String
        Dim loc As String
        Dim sec As New k_security

        Dim fld As String = sec.HexToStr(Request.QueryString("fld"))
        Dim header As String = sec.HexToStr(Request.QueryString("hdrlist"))
        Dim empid As String = (Request.QueryString("empid"))
        Dim keyfld As String = sec.HexToStr(Request.QueryString("keyfld"))
        loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select " & fld & " from " & Request.QueryString("tbl") & " where " & keyfld & "='" & empid & "' order by id desc"
        row = fm.edit_del_list(Request.QueryString("tbl"), sqlx, header, Session("con"), loc)
        Response.Write(row)
        
    End Function

End Class
