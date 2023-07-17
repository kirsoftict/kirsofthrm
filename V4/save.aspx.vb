Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO

Partial Class save
    Inherits System.Web.UI.Page
    Function saveas()
        Dim fm As New formMaker
        Dim keyp As String = ""
        If Request.QueryString("dox") = "edit" Then
            keyp = "update"

        ElseIf Request.QueryString("dox") = "delete" Then
            'Response.Write(keyp & "<br>")
            keyp = "delete"
        Else
            keyp = "save"
        End If
        'Response.Write(keyp)


        Dim idx As String = ""
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
        Try
            If Request.QueryString.HasKeys = True And Request.QueryString("task") <> "" Then
                If Request.QueryString("dox") = "" Then
                    keyval = Request.QueryString("keyval")
                    ' Response.Write("<br>" & keyval)
                    If LCase(Request.QueryString("task")) = "update" Then
                        'e("<script type='text/javascript'>alert('updating....');</script>")
                        sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                        flg = dbx.excutes(sql, Session("con"), Session("path"))
                        ' Response.Write(sql)
                        If flg = 1 Then
                            msg = "Data Updated"
                        Else
                            msg = "Sorry Data is not Updated"
                        End If
                    ElseIf LCase(Request.QueryString("task")) = "delete" Then
                        '  Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                        'sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                        sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                        flg = dbx.save(sql, Session("con"), Session("path"))
                        '  Response.Write(flg)
                        'Response.Write(sql)
                        If flg = 1 Then
                            msg = "Data deleted"
                        Else
                            msg = "Sorry Data is not deleted"
                        End If
                    ElseIf LCase(Request.QueryString("task")) = "save" Then
                        '  Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                        sql = dbx.makest(tbl, Request.QueryString, Session("con"), key)
                        File.AppendAllText("c:\temp\saved\save.sql", sql & "<br>")
                        ' Response.Write(sql)
                        Try
                            flg = dbx.excutes(sql, Session("con"), Session("path"))
                            File.AppendAllText("c:\temp\saved\save.sql", sql & "<br>")
                        Catch ex As Exception
                            '  Response.Write(ex.ToString)
                            flg = 0
                            msg = ex.ToString & " error on saving"
                            File.AppendAllText("c:\temp\saved\errsave.sql", sql & "<br>" & msg)
                        End Try

                        If flg = 1 Then
                            msg = "Data Saved"
                            Dim position As String = ""
                            Dim rdxx() As String
                            If msg = "Data Saved" Then


                                position = fm.getjavalist2("tblposition", "Distinct job_position", Session("con"), "")

                                position = "var position=[" & position & "];"

                                File.WriteAllText(Session("path") & "\scripts\position.kirsoft.js", position)
                            Else
                                If Not File.Exists(Session("path") & "\scripts\position.kirsoft.js") Then
                                    position = fm.getjavalist2("tblposition", "Distinct job_position", Session("con"), "")

                                    position = "var position=[" & position & "];"

                                    File.WriteAllText(Session("path") & "\scripts\position.kirsoft.js", position)
                                Else
                                    rdxx = File.ReadAllLines(Session("path") & "\scripts\position.kirsoft.js")
                                    If rdxx.Length < 1 Then
                                        position = fm.getjavalist2("tblposition", "Distinct job_position", Session("con"), "")

                                        position = "var position=[" & position & "];"

                                        File.WriteAllText(Session("path") & "\scripts\position.kirsoft.js", position)
                                    End If
                                End If
                            End If
                        Else
                            msg = "Sorry Data is not Save"
                        End If
                    End If
                        'MsgBox(rd)

                        ' sql = db.makest(tbl, Request.QueryString, session("con"), key)

                        If flg <> 1 Then
                            ' Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
                        End If


                    End If
                End If
        Catch ex2 As Exception
            msg = ex2.ToString & " error on saving"
        End Try
        Response.Write(msg)
    End Function
End Class
