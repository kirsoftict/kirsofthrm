Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.FileInfo
Imports System.Security.AccessControl
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm



Partial Class login
    Inherits System.Web.UI.Page


    Public Function checklogin(ByVal uname As String, ByVal pwd As String, ByVal urlpath As String, ByVal imgpath As String) As String


        Dim rs As DataTableReader
        Dim obj As Object
        Dim dt As New dbclass
        Dim msg As String = ""
        Dim fm As New formMaker
        Dim sql As String
        Dim dpt As String = ""
        Dim ishead As String = ""
        Dim sumitedto As String = ""
        Dim listunder As String = ""


        If uname <> "" And pwd <> "" Then
            Try
                Response.Write(Session("think"))

                sql = "select * from login where username='" & uname & "' and password='" & pwd & "' and active='y'"
                Try
                    If Session("con").state = ConnectionState.Closed Then
                        Session("con").open()
                    End If
                    rs = dt.dtmake("loginx", sql, Session("con"))

                Catch ex2 As Exception
                    Response.Write(ex2.ToString & "<<<<<<<<<<<<<<<<0000000" & sql & "<br>")
                End Try

                'rs = obj
                If rs.HasRows = True Then
                    rs.Read()
                    Response.Write("<br>iiiiiinnnnnnnnnnnnn</br>")
                    If rs.Item("password") <> pwd Then
                        msg = "Password is incorrect"
                    Else


                        Session("right") = rs.Item("auth")
                        Session("who") = rs.Item("Id")
                        Session("password") = rs.Item("password")
                        Session("username") = rs.Item("username")
                        Session("uright") = rs.Item("auth").split(";")
                        Session("supordinate") = ""
                        Session("work_load") = ""


                        If rs.IsDBNull(1) = True Then
                            Session("emp_iid") = "Admin"
                        ElseIf rs.Item(1) = "" Then

                            Session("emp_iid") = "Admin"
                        Else
                            Dim rtn As Object

                            Session("emp_iid") = rs.Item("emp_id")

                            Dim mgpath As String = imgpath & rs.Item("emp_id").trim & "/idpic/"
                            mgpath = mgpath.Replace("/", "\").Trim()
                            Response.Write(imgpath & fm.getinfo2("select imglink from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con")))
                            If File.Exists(imgpath & fm.getinfo2("select imglink from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con"))) = True Then
                                Response.Write("exists")
                            End If

                            If File.Exists(imgpath & fm.getinfo2("select imglink from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con"))) Then
                                Session("imglink") = urlpath & "/" & fm.getinfo2("select imglink from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con"))
                            Else
                                Session("imglink") = urlpath & "/images/gif/default_employee_image.gif"
                            End If
                            dpt = fm.getinfo2("select department from emp_job_assign where emp_id='" & Session("emp_iid") & "' order by id desc", Session("con"))
                            sumitedto = fm.getinfo2("select submited_to from emp_job_assign where emp_id='" & Session("emp_iid") & "' order by id desc", Session("con"))
                            Application(dpt) &= Session("emp_iid") & ","
                            sql = "insert into chatroom(dept,emp_id,ssid,logtime,active) values('" & dpt & "',"
                            rtn = dt.excutes("update chatroom set active='n' where emp_id='" & rs.Item("emp_id") & "'", Session("conx"), Session("path"))
                            If (IsNumeric(rtn)) Then
                                '  Response.Write(rtn.ToString)
                            End If
                            listunder = fm.getjavalist2("emp_job_assign where submited_to='" & Session("emp_iid") & "' order by id desc", "submited_to", Session("con"), ",")
                            sql &= "'" & rs.Item("emp_id") & "','" & Session.SessionID & "','" & Format(Now, "M/d/yyyy H:mm:ss") & "','y')"

                            rtn = dt.excutes(sql, Session("conx"), Session("path"))
                            If IsNumeric(rtn) Then
                                '  Response.Write("<br>" & rtn.ToString)
                            End If
                            Session("emp_dep") = dpt
                            ' Response.Write(sql & rtn)
                            Session("dpt") &= dpt & ","
                        End If
                        If Session("emp_iid") <> "Admin" Then
                            Session("userimage") = "employees"
                        End If

                    End If
                    Response.Redirect("index.aspx")

                Else
                    Dim keys As NameObjectCollectionBase.KeysCollection
                    Dim key As String
                    ' Dim sessionstr As String
                    keys = Session.Keys

                    msg = "Username or Passward is incorrect"
                    sql = "select * from login where username='" & uname & "'"
                    rs = dt.dtmake("login" & Today.ToLongDateString, sql, Session("con"))
                    If rs.HasRows = True Then
                        rs.Read()
                        If rs.Item("password") <> pwd Then
                            msg = "Password is incorrect"
                        End If
                    End If
                    rs.Close()
                    sql = "select * from login where username='" & uname & "' and password='" & pwd & "'"
                    rs = dt.dtmake("login" & Today.ToLongDateString, sql, Session("con"))
                    If rs.HasRows = True Then
                        rs.Read()
                        If rs.Item("active") <> "n" Then
                            msg = "User is not Active"
                        End If
                    End If
                    rs.Close()
                   

                        'msg = (copytable("login", "cm_login", Session("con"), Session("conx"), rs.Item("id"), Session("path")))
                        'Response.Write(msg)
                        'Session("right") = rs.Item("auth")
                        'Session("who") = rs.Item("Id")
                        'Session("password") = rs.Item("password")
                        'Session("username") = rs.Item("username")
                        'Session("uright") = rrig
                        'If rs.IsDBNull(1) = True Then
                        ' Session("emp_iid") = "Admin"
                        'ElseIf rs.Item(1) = "" Then

                        'Session("emp_iid") = "Admin"
                        'Else
                        'Session("emp_iid") = rs.Item("emp_id")

                        ' End If
                        'If Session("emp_iid") <> "Admin" Then
                        'Session("userimage") = "employees"
                        'End If
                        'Dim fx() As String = {""}
                        'If String.IsNullOrEmpty(Session("right")) = False Then
                        'fx = Session("right").split(";")
                        'ReDim Preserve fx(UBound(fx) + 1)
                        'fx(UBound(fx) - 1) = ""
                        'End If
                        'If (fm.searcharray(fx, "1") Or fm.searcharray(fx, "2")) Then
                        'If File.Exists("c:\temp\backup\passs.kst") Then
                        'Dim i As Integer = 0
                        'Dim line() As String = File.ReadAllLines("c:\temp\backup\passs.kst")
                        'For i = 0 To line.Length - 1
                        ' If i = 0 Then
                        'Session("fromk") = line(i)
                        'ElseIf i = 1 Then
                        'Session("passk") = line(i)
                        'Else
                        'Session("tosd") &= line(i) & ","

                        'End If

                        '    Next
                        'End If
                        'Response.Redirect("init.aspx?next=on")
                        'Else
                        'Response.Redirect("homenew.aspx")
                        ' End If
                        '    Else
                        '   msg = "not authenticated!"
                        ' End If
                        'End If


                        '    Else
                        msg = "Username or Passward is incorrect"
                        'End If
                    End If
            Catch ex As Exception
                msg = "Unknown Error"
                Response.Write(ex.ToString & "<br>,,,,,,,................")
            End Try
        End If
        ' Dim sqlx As String = "delete from cm_address where emp_no not in(select emp_no from cm_empreg)"
        ' dt.excutes(sqlx, Session("conx"), Session("path"))

        Return msg
    End Function
    Public Function CheckBrowserCaps()

        Dim labelText As String = ""
        Dim myBrowserCaps As System.Web.HttpBrowserCapabilities = Request.Browser
        If (CType(myBrowserCaps, System.Web.Configuration.HttpCapabilitiesBase)).IsMobileDevice Then
            labelText = "Browser is a mobile device."
        Else
            labelText = "Browser is not a mobile device."
        End If

        Return labelText

    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Session("con").open()

    End Sub

    

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

        'Session("con").close()
    End Sub


    Function logcopy()
        Dim p2, pathx As String



        p2 = Session("path") & "\log\"
        pathx = Session("path") & "\log\logkir.txt"
        Try
            If File.Exists(pathx) Then
                Dim flinf As New FileInfo(pathx)
                '  Response.Write(flinf.Length & "bitye")
            Else
                ' Response.Write(pathx)
            End If

        Catch ex As Exception
            Response.Write(ex.ToString & "<br>")
        End Try


    End Function
    Public Function copytable(ByVal stbl As String, ByVal dtbl As String, ByVal scon As String, ByVal dcon As SqlConnection, ByVal id As String, ByVal pth As String)
        Dim rs As DataTableReader
        Dim dc As New dbclass
        Dim fm As New formMaker
        rs = dc.dtmake("xxx", "select * from cm_login", dcon)
        Dim sql As String = ""
        Dim c As Integer
        c = rs.FieldCount
        sql = "insert into " & dtbl & "("
        Dim f, v, r As String
        f = ""
        v = ""
        Dim flds() As String
        flds = dc.getdatafields("login", dcon)
        If flds.Length > 0 Then
            For j As Integer = 0 To UBound(flds)
                Response.Write(flds(j) & "<br>")
            Next
        End If
        For i As Integer = 1 To c - 1
            If fm.searcharray(flds, rs.GetName(i)) = True Then
                r = fm.getinfo2("select " & rs.GetName(i) & " from " & stbl & " where id=" & id, dcon)

                If r <> "None" Then

                    If i < c - 1 Then
                        f &= rs.GetName(i) & ","
                        v &= "'" & r & "',"
                    Else
                        f &= rs.GetName(i)
                        v &= "'" & r & "'"
                    End If

                End If
            Else
                Response.Write(rs.GetName(i) & "<br>")
            End If
        Next
        Dim flg As String = ""
        sql &= f & ") values(" & v & ")"
        flg = dc.excutes(sql, dcon, pth)

        Return flg & "<br>" & sql
    End Function
End Class
