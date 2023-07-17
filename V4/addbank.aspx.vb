Imports Kirsoft.hrm

Partial Class addbank
    Inherits System.Web.UI.Page
    Public fm As New formMaker
    Public keyp As String = ""
    Public idx As String = ""
    Public flg As String = ""
    Public rd As String = ""
    Function lineone()

        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        Else
            keyp = "save"
        End If

        idx = Request.QueryString("id")
        Dim msg As String = ""
        Dim dbx As New dbclass
        Dim sql As String = ""

        Dim flg2 As Integer = 0
        Dim saverr As String = ""
        ' Response.Write(sc.d_encryption("zewde@123"))


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
                    flg = dbx.excutes(sql, Session("con"), Session("path"))
                    ' Response.Write(sql)
                    If IsNumeric(flg) = True Then
                        msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                    Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    'Response.Write(sql)
                    flg = dbx.excutes(sql, Session("con"), Session("path"))
                    ' Response.Write("<br>" & saverr & "<br>")
                    ' Response.Write(sql)
                    If IsNumeric(flg) = True Then

                        msg = "Data deleted"
                    Else
                        fm.exception_hand(saverr, Session("company_name") & " addbank.aspx")
                    End If
                ElseIf Request.QueryString("task") = "save" Then
                    ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")
                    Try


                        sql = dbx.makest(tbl, Request.QueryString, Session("con"), key)
                        ' Response.Write(sql)
                        saverr = dbx.excutes(sql, Session("con"), Session("path"))
                        If IsNumeric(saverr) Then
                            flg = saverr
                            msg = "Data Saved"
                            Response.Write(msg)
                        Else
                            Response.Write(flg)
                        End If

                    Catch ex As Exception
                        Response.Write(ex.ToString & "<br>" & sql & "<br>" & tbl & "<br>")
                        fm.exception_hand(ex, "addbank Erro :")
                    End Try
                End If
                'MsgBox(rd)

                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)

                If flg <> "" And IsNumeric(flg) = False Then
                    Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
                End If


            End If
        End If

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("con").state = Data.ConnectionState.Closed Then
            Response.Write("database is closed")
            Session("con").open()
        End If
    End Sub
End Class
