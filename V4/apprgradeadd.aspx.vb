Imports Kirsoft.hrm

Partial Class apprgradeadd
    Inherits System.Web.UI.Page
    Public flg As Integer

    Public flg2 As Integer
    Public keyp As String
    Public idx As String
    Function bee() As String
        keyp = ""
        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        Else
            keyp = "save"
        End If
        idx = ""
        idx = Request.QueryString("id")
        Dim msg As String = ""
        Dim dbx As New dbclass
        Dim sql As String = ""
        flg = 0
        flg2 = 0
        ' Response.Write(sc.d_encryption("zewde@123"))
        Dim rd As String = ""

        Dim tbl As String = ""
        Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                    ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)

                    flg = dbx.save(sql, Session("con"), Session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                        msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                    'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, Session("con"), Session("path"))

                    ' Response.Write(sql)
                    If flg = 1 Then
                        msg = "Data deleted"
                    End If
                ElseIf Request.QueryString("task") = "save" Then
                    ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                    sql = dbx.makest(tbl, Request.QueryString, Session("con"), key)
                    'Response.Write(sql)
                    flg = dbx.save(sql, Session("con"), Session("path"))
                    If flg = 1 Then
                        msg = "Data Saved"
                    End If
                End If
                'MsgBox(rd)

                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)

                If flg < 1 And flg <> 0 Then
                    Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
                End If


            End If
        End If
    End Function
End Class
