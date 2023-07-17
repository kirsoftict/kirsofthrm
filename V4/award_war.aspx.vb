Imports Kirsoft.hrm
Imports System.Data
Imports System.IO

Partial Class award_war
    Inherits System.Web.UI.Page
    Public rm As Integer
    Public idx As String = ""
    Public keyp As String = ""
    Public fm As New formMaker
    Public emp_id, emptid As String
    Public sssql As String
    Public floc As String = ""


    Function firstlines()

        floc = Server.MapPath("employee") & "/" & Session("emp_id") & "/Letter"
        If Directory.Exists(floc) = False Then
            MkDir(floc)
        End If
        emptid = ""
        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        ElseIf Request.QueryString("dox") = "clear" Then
            keyp = "Clear"

        Else
            keyp = "save"
        End If
    End Function
    Function secondlines()

        idx = Request.QueryString("id")
        Dim msg As String = ""
        Dim dbx As New dbclass
        Dim sql As String = ""
        Dim flg As Integer = 0
        Dim flg2 As Integer = 0
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
                If Request.QueryString("task") = "save" Then
                    ' Response.Write(Request.QueryString("task"))
                    Dim arrname() As String
                    sql = ""

                    Dim dtt As DataTableReader
                    emptid = Request.QueryString("emptid")
                    emp_id = Request.QueryString("emp_id")

                    If emp_id <> "" And emptid <> "" Then
                        'sql = dbx.makest(tbl, Request.QueryString, Session("con"), key)

                        sql = "insert into emp_aw_wr(emptid,emp_id,wtgiving,giving_date,letter_no,description,attachfile,who_reg,date_reg) " & _
                            "Values(" & _
                         emptid & ",'" & emp_id & "','" & _
                         Replace(Request.QueryString("wtgiving"), "'", Chr(34)) & "','" & _
                         Request.QueryString("giving_date") & "','" & Request.QueryString("letter_no") & "','" & _
                         Replace(Request.QueryString("description"), "'", Chr(34)) & "','" & _
                         Request.QueryString("attachfile") & "','" & _
                         Request.QueryString("who_reg") & "','" & _
                         Request.QueryString("date_reg") & "')"
                        Response.Write(sql)
                        Dim sqls As String
                        sqls = "BEGIN TRANSACTION award" & Chr(13)
                        sqls &= sql & Chr(13)

                        Try
                            flg = dbx.excutes(sqls, Session("con"), Session("path"))
                            Response.Write(flg)
                        Catch ex As Exception
                            flg = "-4"
                            Response.Write(ex.ToString & sqls)
                        End Try

                        If flg > 0 Then
                            Dim idinter As String
                            idinter = fm.getinfo2("select id from emp_aw_wr order by id desc", Session("con"))
                            Response.Write("Id NO will be;" & idinter.ToCharArray)
                            flg = dbx.excutes("COMMIT TRANSACTION award", Session("con"), Session("path"))


                            If flg = -1 Then
                                msg = "data Saved"
                            End If
                        Else
                            Try
                                flg = dbx.excutes("ROLLBACK TRANSACTION award", Session("con"), Session("path"))

                            Catch ex As Exception
                                Response.Write(ex.ToString)
                            End Try
                            If flg = -1 Then
                                msg = "data Not saved Saved"
                            Else
                                msg = "data isnot saved"
                            End If
                        End If
                    Else
                        msg = "Sorry! data is not saved!  " & msg
                    End If
                    ' sql = dbx.makest(tbl, Request.QueryString, session("con"), key)

                    '
                ElseIf LCase(Request.QueryString("task")) = "update" Then
                    keyval = Request.QueryString("keyval")

                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                    Response.Write(sql)
                    ' flg = dbx.save(sql, Session("con"), Session("path"))
                ElseIf LCase(Request.QueryString("task")) = "delete" Then
                    sql = "BEGIN TRANSACTION delawa" & Chr(13) & "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.excutes(sql, Session("con"), Session("path"))

                    Response.Write(sql)
                    If flg > 0 Then
                        dbx.excutes("COMMIT TRANSACTION delawa", Session("con"), Session("path"))
                    Else
                        dbx.excutes("ROLLBACK TRANSACTION delawa", Session("con"), Session("path"))

                        msg = "Deletion is not preform"
                    End If
                End If

                'MsgBox(rd)

                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)




            End If
        End If
        fm = Nothing
        dbx = Nothing
        Response.Write(msg)
        Return msg
    End Function


End Class
