Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class hrmjobs
    Inherits System.Web.UI.Page
    Function makeform(ByVal tblname)
        Dim formx As String = ""
        Dim rs As SqlDataReader
        Dim obj As New menubuilder
        Dim icount, i As Integer
        rs = obj.cmdx(Session("con"), "select * from " & tblname)
        icount = rs.FieldCount
        formx = "<form method='post' id='frm" & tblname & "' name='frm" & tblname & "'>" & Chr(13)
        formx = formx & "<table>"
        For i = 0 To icount - 1
            If i = 0 Then
                formx = formx & "<tr style='display:none'><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                "<input type='hidden' name='" & rs.GetName(i) & "' value=''><b></b></td></tr>" & Chr(13)
            ElseIf LCase(rs.GetDataTypeName(i)) = "text" Then
                formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                     "<textarea id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & "'>"
                formx = formx & "</textarea></td></tr>" & Chr(13)


            ElseIf LCase(rs.GetDataTypeName(i)) = "datetime" Then
                formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                                          "<input type='text' class='input-field' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                                          "' value='" & Today.Month & "/" & Today.Day & "/" & Today.Year & "' />" & Chr(13) & _
                                                    "<script type='text/javascript'>" & Chr(13) & _
 "$(function () {" & Chr(13) & _
    " $(" & Chr(34) & "#" & rs.GetName(i) & Chr(34) & ").datepicker({ minDate: " & Chr(34) & "-20Y" & Chr(34) & ", maxDate: " & Chr(34) & "+20Y" & Chr(34) & " });" & Chr(13) & _
    "   });" & _
    "$('#" & rs.GetName(i) & "').datepicker( 'option','dateFormat','mm/dd/yy');" & Chr(13) & _
"</script>" & _
                                                         " </td></tr> " & Chr(13)
            Else
                formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & Chr(13) & _
                                                        "<input type='text' class='input-field' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                                   "' value=''></td></tr>" & Chr(13)


            End If

        Next
        formx = formx & "</table>"
        rs.Close()
        Return formx
    End Function
    Function edit(ByVal menu_id, ByVal editwhat, ByVal id)
        Dim str As String
        Dim rs As SqlDataReader
        Dim cm As New menubuilder
        Dim f As New file_list
        Dim formx As String = ""
        If checkdir(menu_id, id) Then

        End If

        rs = cm.cmdx(Session("con"), "select * from page_containt where menu_id=" & menu_id & " and id=" & id)
        str = ""

        Select Case editwhat
            Case "Menu"

            Case "pages"
                If rs.HasRows = True Then
                    Dim icount, i As Integer

                    icount = rs.FieldCount
                    rs.Read()
                    formx = "<form method='post' id='frmpage_containt' name='frmpage_containt' enctype='multipart/form-data' action='edit.aspx?updated=on'>"
                    formx = formx & "<table>"
                    For i = 0 To icount - 1
                        If i = 0 Or i = 1 Then
                            formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                            "<input type='hidden' name='" & rs.GetName(i) & "' value='" & rs.Item(i) & "'><b>" & CStr(rs.Item(rs.GetName(i))) & "</b></td></tr>"
                        ElseIf LCase(rs.GetDataTypeName(i)) = "text" Then
                            formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                                 "<textarea id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & "'>"
                            If rs.GetName(i) = "detail" Then
                                If rs.IsDBNull(i) = False Then
                                    If File.Exists(rs.Item(i)) = True Then
                                        str = f.getfilecontx(CStr(rs.Item(i)))
                                    Else
                                        str = ""
                                    End If
                                    formx = formx & str
                                End If

                            Else
                                If rs.IsDBNull(i) = False Then
                                    formx = formx & CStr(rs.Item(i))
                                End If
                            End If

                            formx = formx & "</textarea></td></tr>"
                        ElseIf rs.GetName(i) = "small_image" Then
                            formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                   "<input type='file' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                   "'>"
                            If rs.IsDBNull(i) = False Then
                                formx = formx & "<br><img src='../" & rs.Item(i) & "' alt='small image' width='170'>"
                            Else
                                formx = formx & "<br>No image"
                            End If
                            formx = formx & "</td></tr>"
                        ElseIf rs.GetName(i) <> "publish" Then
                            formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                       "<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                       "' value='"
                            If rs.isdbnull(i) = False Then


                                If CStr(rs.Item(i)) <> "" Then
                                    formx = formx & CStr(rs.Item(i))
                                End If

                            End If
                            formx = formx & "'></td></tr>"
                        End If
                    Next
                    formx = formx & "<tr><td><button name='btnupdate' value='update'>Update</button></table></form>"
                    formx = formx & "<script> document.getElementById('sessionid').value='" & session.sessionid & "'</script>"
                    cm = Nothing


                End If


        End Select
        rs.Close()
        Return formx
    End Function
    Function updateon()
        Dim str, strx As String
        str = ""
        strx = ""
        Dim rs As SqlDataReader
        Dim cm As New menubuilder
        Dim row As String
        Dim k As String
        Dim f As New file_list
        k = (f.findpath(Server.MapPath("")))
        Dim path, path2 As String
        Dim httppath As String = ""
        Dim i As Integer
        Dim datatype As String
        Dim strcode As String
        Dim fname, ufname, ufext As String
        Dim ufallow() As String = {".jpg", ".jpeg", ".png", ".gif"}
        Dim fileupload1 As HttpPostedFile
        Dim fileok, fsize As Boolean
        If Request.Form("id") <> "" Then
            rs = cm.cmdx(Session("con"), "select * from page_containt where id=" & Request.Form("id"))
            str = str & "update page_containt set "
            If Request.QueryString("updated") = "on" Then
                path2 = Server.MapPath("~/")
                path = k & "pages\" & Request.Form("menu_id") & "\" & Request.Form("id") & "\"
                ' path2 = "~/pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/"
                If Not Directory.Exists(path & "images") Then
                    f.makedir(path & "images")
                End If
                If Not Directory.Exists(path & "text") Then
                    f.makedir(path & "text")
                End If
                fileok = False
                fsize = False
                Dim sstr As String = (Request.Form("short_story")).Replace("'", "''")

                strcode = (Request.Form("detail")).Replace("'", "''")
                fileupload1 = Request.Files("small_image")
                If fileupload1.FileName <> "" Then
                    ufname = fileupload1.FileName
                    ufext = LCase(System.IO.Path.GetExtension(ufname))

                    For i = 0 To ufallow.Length - 1
                        If ufext = ufallow(i) Then
                            fileok = True
                        End If
                    Next

                    If fileupload1.ContentLength < 100000 Then
                        fsize = True
                    End If
                    If fileok And fsize Then
                        Try
                            ufname = f.findfilename(ufname)
                            fileupload1.SaveAs(path & "images/" & ufname)
                            httppath = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                            str = str & " small_image" & "='" & httppath & "', "
                        Catch ex As Exception
                            response.write("File could not be uploaded." & ex.ToString)
                        End Try
                    ElseIf fileok = False Then
                        response.write("Cannot accept files of this type.")
                    ElseIf fsize = False Then
                        response.write("File size is morethan acceptable; max size=100kb")
                    End If


                End If

                For i = 2 To rs.FieldCount - 1
                    datatype = LCase(rs.GetDataTypeName(i))

                    If Request.Form(rs.GetName(i)) <> "" Then
                        If datatype = "text" Or datatype = "varchar" Or datatype = "char" Then
                            If rs.GetName(i) = "detail" Then

                                fname = "detail_" & Request.Form("id") & ".rtf"

                                If File.Exists(path & "text\" & fname) = False Then
                                    File.CreateText(path & "text\" & fname)

                                End If
                                Try
                                    ' response.write(path & "<br>")
                                    File.WriteAllText(path & "text\" & fname, Request.Form("detail"))
                                Catch ex As Exception
                                    response.write(ex.ToString & "line 198")
                                End Try

                                str = str & rs.GetName(i) & "='" & path & "text\" & fname & "'"
                            Else
                                strcode = (Request.Form(rs.getname(i))).Replace("'", "''")

                                str = str & rs.GetName(i) & "='" & strcode & "'"
                            End If

                        Else
                            str = str & rs.GetName(i) & "=" & Request.Form(rs.GetName(i))
                        End If
                        If (rs.FieldCount - 1) <> i Then
                            str = str & ", "
                        End If
                    End If
                Next

                str = str & " where id=" & Request.Form("id")
                'response.write(str)

                Dim cmx As New SqlCommand
                With cmx
                    .Connection = Session("con")
                    Try
                        rs.Close()
                    Catch ex As Exception
                        response.write(ex.Message)
                    End Try
                    .CommandType = CommandType.Text
                    .CommandText = str
                    Try
                        ' response.write(str)
                        row = .ExecuteNonQuery()
                    Catch ex As Exception
                        response.write(ex.ToString)
                        str = ""
                    End Try

                End With
            End If
            rs.Close()
            cm = Nothing

        End If

        Return str
    End Function
    Function checkdir(ByVal mid As String, ByVal id As String)
        Dim path As String
        Dim arr() As String
        Dim i As Integer
        Dim npath As String
        path = Server.MapPath("")
        arr = path.Split("\")
        npath = ""
        For i = 0 To arr.Length - 2
            npath = npath & arr(i) & "\"
        Next
        Try
            Dim returnValue As DirectoryInfo
            If Not Directory.Exists(npath & "pages\" & mid & "\" & id) Then
                returnValue = Directory.CreateDirectory(npath & "pages\" & mid & "\" & id)
                ' response.write(returnValue.FullName)
            End If
        Catch ex As Exception
            response.write(ex.ToString)
        End Try

    End Function
End Class
