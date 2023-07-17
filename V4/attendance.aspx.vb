Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class attendance
    Inherits System.Web.UI.Page
    Public Function edit_del_list_wname_att(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        Dim arr() As String = {""}
        '   Response.Write("<br>-----------------------------------------<br> " & sql & "<br>--------------------------------------------------------------<br>")
        Try

        
            rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString() + '&" & Request.ServerVariables("QUERY_STRING") & ");$('#frms').submit();}</script>"
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        If dt.HasRows = True Then
            Dim color As String = "E3EAEB"
            Dim empid As String
            Dim datex As Boolean = False
            rtstr &= "<form id='frms' method='post' name='frms' action=''>"
            While dt.Read
                If fm.searcharray(arr, sec.Str2ToHex(dt.Item("att_date"))) = False Then
                    Dim arrbound As Integer = UBound(arr)
                    ReDim Preserve arr(arrbound + 1)
                    arr(arrbound) = sec.Str2ToHex(dt.Item("att_date"))

                    If datex = True Then
                        rtstr &= "</table></div>" & Chr(13)
                        datex = False
                    End If
                    If datex = False Then
                        rtstr &= "<br><div id='" & sec.Str2ToHex(dt.Item("att_date")) & "' class='collapsed' style='height:25px;width:900px; background-color:blue; cursor:pointer;color:white; font-weight:bold;' onclick=" & Chr(34) & "javascript:showHideSubMenu(this,'tbl" & _
                                                  sec.Str2ToHex(dt.Item("att_date")) & "')" & Chr(34) & _
                                                  "><span style='float:left'>" & dt.Item("att_date") & "</span><img src='images/gif/collapsed_.gif' style='float:right;top:10px;'></div>" & Chr(13) & "<div id='tbl" & sec.Str2ToHex(dt.Item("att_date")) & "' style='display:none;'><table  cellspacing='0' cellpadding='0'>" & _
                                      "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
                        If heading <> "" Then
                            For i = 0 To hdr.Length - 1
                                rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                            Next
                        Else
                            For i = 1 To dt.FieldCount - 1
                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"
                            Next
                        End If
                        rtstr = rtstr & "</tr>" & Chr(13)
                        datex = True

                    End If
                End If


                empid = dt.Item("emptid")
                If color <> "#E3EAEB" Then
                    color = "#E3EAEB"
                Else
                    color = "#fefefe"
                End If
                rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>"
                rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "(" & empid & ")</td>"
                For k As Integer = 1 To dt.FieldCount - 2
                    If dt.Item(k).ToString = "y" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                    ElseIf dt.Item(k).ToString = "n" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                    ElseIf dt.GetName(k) = "department" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                        fm.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                        dt.Item(k).ToString & "'", con) & "</td>"
                    ElseIf dt.GetName(k) = "project_id" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                        fm.getinfo2("select project_name from tblproject where project_id='" & _
                        dt.Item(k).ToString & "'", con) & "</td>"
                    Else

                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                    End If

                Next
                rtstr = rtstr & "</tr>"
            End While
        Else
            rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
        End If
        rtstr = rtstr & "</table></div></form>"
        dt.Close()
            dc = Nothing
        Catch ex As Exception
            Response.Write(ex.ToString & "<<<<<")
        End Try
        Return rtstr

    End Function

    Public Function edit_del_list_wname_new(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        Dim arr() As String = {""}
        '   Response.Write("<br>-----------------------------------------<br> " & sql & "<br>--------------------------------------------------------------<br>")
        Try


            rtstr = ""
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                Dim empid As String
                Dim datex As Boolean = False
                rtstr &= "<form id='frms' method='post' name='frms' action=''>"

                rtstr &= "<table  cellspacing='0' cellpadding='0'><tr><td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "selectDel" & Chr(34) & "," & Chr(34) & Chr(34) & "," & Chr(34) & Request.ServerVariables("QUERY_STRING") & Chr(34) & "," & Chr(34) & loc & Chr(34) & "," & Chr(34) & tbl & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' />Delete</td>" & _
                                         "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"

                If heading <> "" Then
                    For i = 0 To hdr.Length - 1
                        rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                    Next
                Else
                    For i = 1 To dt.FieldCount - 1
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"
                    Next
                End If
                rtstr = rtstr & "</tr>" & Chr(13)
                While dt.Read

                    empid = dt.Item("emptid")
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & "," & Chr(34) & Request.ServerVariables("QUERY_STRING") & Chr(34) & "," & Chr(34) & loc & Chr(34) & "," & Chr(34) & tbl & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & _
                    "<td  style='padding-right:20px;cursor:pointer;'><input type='checkbox' id='del" & dt.Item(0) & "'  name='del" & dt.Item(0) & "' value='" & dt.Item("id") & "'></td>"
                    rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "(" & empid & ")</td>"
                    For k As Integer = 1 To dt.FieldCount - 2
                        If dt.Item(k).ToString = "y" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                        ElseIf dt.Item(k).ToString = "n" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                        ElseIf dt.GetName(k) = "department" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            fm.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        ElseIf dt.GetName(k) = "project_id" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            fm.getinfo2("select project_name from tblproject where project_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        Else

                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                        End If

                    Next
                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table></div></form>"
            dt.Close()
            dc = Nothing
        Catch ex As Exception
            Response.Write(ex.ToString & "<<<<<")
        End Try
        Return rtstr

    End Function

    Function makerows()

        Dim row As String
        Dim loc As String
        Dim sqlx As String
        Dim pd1 As String
        Dim empid() As String
        Dim fm As New formMaker

        loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        If Request.QueryString("which") = "month" Then
            pd1 = Request.QueryString("m") & "/1/" & Request.QueryString("y")
            sqlx = "select id,att_date,status,daypartition,emptid from emp_att where month(att_date)=" & Request.QueryString("m") & " and year(att_date)=" & Request.QueryString("y") & " order by att_date desc"

        ElseIf Request.QueryString("which") = "byname" Then
            empid = fm.getempid(Request.QueryString("vname"), Session("con"))
            ' Response.Write("<br><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<br>" & empid(2) & "<br>_______________________________________________________________</br>")
            sqlx = "select id,att_date,status,daypartition,emptid from emp_att where emptid='" & empid(1) & "' order by att_date desc"
        Else
            pd1 = Today.Month & "/1/" & Today.Year
            sqlx = "select id,att_date,status,daypartition,emptid from emp_att where month(att_date)=" & Today.Month & " and year(att_date)=" & Today.Year & " order by att_date desc"

        End If
        ' Response.Write(sqlx)
        row = edit_del_list_wname_new("emp_att", sqlx, "Full Name,Attendance Date,Status,Day Partition", Session("con"), loc)
        Response.Write(row)


    End Function

    Function makerows2()

        Dim row As String
        Dim loc As String
        Dim sqlx As String
        Dim pd1 As String
        Dim empid() As String
        Dim fm As New formMaker

        loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        If Session("mmm") <> "" Then

            sqlx = "select id,att_date,status,daypartition,emptid from emp_att  where month(att_date)=" & Session("mmm") & " and year(att_date)=" & Session("yyr") & " order by emptid, att_date desc"
        Else

            sqlx = "select id,att_date,status,daypartition,emptid from emp_att  where month(att_date)=" & Today.Month & " and year(att_date)=" & Today.Year & " order by emptid, att_date desc"


        End If
        ' Response.Write(sqlx)
        row = edit_del_list_wname_new("emp_att", sqlx, "Full Name,Attendance Date,Status,Day Partition", Session("con"), loc)
        Response.Write(row)


    End Function

    Public Function edit_del_list_wname_att2(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As Array
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        ' Dim sec As New k_security
        Dim arr() As String = {""}
        Dim tbs() As String = {"", ""}
        Dim pats As String
        Dim pg As Integer = 1
        Dim cc As Integer = 1
        Dim script As String = ""
        ' Response.Write(sql)
        rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        If dt.HasRows = True Then

            Dim color As String = "E3EAEB"
            Dim empid As String
            Dim datex As Boolean = False
            Dim colorx As String = ""
            rtstr &= "<form id='frms' method='post' name='frms' action=''>" & Chr(13)
            While dt.Read
                If fm.searcharray(arr, sec.Str2ToHex(dt.Item("att_date"))) = False Then
                    Dim arrbound As Integer = UBound(arr)
                    ReDim Preserve arr(arrbound + 1)
                    arr(arrbound) = sec.Str2ToHex(dt.Item("att_date"))

                    If pg = 1 Then
                        tbs(pg) = "<div id='pg" & pg & "'><div id='tabs-" & pg & "'><ul>" & Chr(13)
                        script &= "$(function(){$(' #tabs-" & pg & "').tabs();});" & Chr(13)
                        pg = 2

                    ElseIf cc Mod 30 = 0 Then
                        ReDim Preserve tbs(pg + 1)
                        colorx = RGB(Rnd(254), Rnd(253), Rnd(242))
                        'rtstr = rtstr & "</table>" & Chr(13) & "</div>"
                        tbs(pg) = "</ul>" & Chr(13) & rtstr & Chr(13) & "</div>" & Chr(13) & "</div>"
                        tbs(pg) &= "<div id='pg" & pg & "' style='background:" & colorx & "'>" & Chr(13) & "<div id='tabs-" & pg & "'>" & Chr(13) & "<ul>" & Chr(13)
                        script &= "$(function(){$(' #tabs-" & pg & "').tabs();});"
                        rtstr = ""
                        pg += 1

                        End If


                    If datex = True Then
                        rtstr &= "</table>" & Chr(13) & "</div>" & Chr(13)
                        datex = False
                    End If

                    cc += 1
                    If datex = False Then
                        tbs(pg - 1) &= "<li><a href='#" & sec.Str2ToHex(dt.Item("att_date")) & "'>" & dt.Item("att_date") & "</a></li>" & Chr(13)

                        rtstr &= "<div id='" & sec.Str2ToHex(dt.Item("att_date")) & "' style='display:none;'>" & Chr(13) & "<table  cellspacing='0' cellpadding='0'>" & Chr(13) & _
                                      "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>" & Chr(13) & "<td  style='padding-right:20px;'>Edit</td>" & Chr(13) & "<td  style='padding-right:20px;'>delete</td>" & Chr(13)
                        If heading <> "" Then
                            For i = 0 To hdr.Length - 1
                                rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>" & Chr(13)
                            Next
                        Else
                            For i = 1 To dt.FieldCount - 1
                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>" & Chr(13)
                            Next
                        End If
                        rtstr = rtstr & "</tr>" & Chr(13)
                        datex = True

                    End If
                End If


        empid = dt.Item("emptid")
        If color <> "#E3EAEB" Then
            color = "#E3EAEB"
        Else
            color = "#fefefe"
        End If
        rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & Chr(13) & _
        "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & Chr(13) & _
        "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>" & Chr(13)
        rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "(" & empid & ")</td>" & Chr(13)
        For k As Integer = 1 To dt.FieldCount - 2
            If dt.Item(k).ToString = "y" Then
                rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>" & Chr(13)
            ElseIf dt.Item(k).ToString = "n" Then
                rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>" & Chr(13)
            ElseIf dt.GetName(k) = "department" Then
                rtstr = rtstr & "<td  style='padding-right:20px;'>" & Chr(13) & _
                fm.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                dt.Item(k).ToString & "'", con) & "</td>" & Chr(13)
            ElseIf dt.GetName(k) = "project_id" Then
                rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                fm.getinfo2("select project_name from tblproject where project_id='" & _
                dt.Item(k).ToString & "'", con) & "</td>" & Chr(13)
            Else

                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>" & Chr(13)
            End If

        Next
        rtstr = rtstr & "</tr>" & Chr(13)
            End While
        Else
            rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>" & Chr(13)
        End If
        rtstr = rtstr & "</table>" & Chr(13) & "</div>" & Chr(13)
        tbs(pg) &= "</ul>" & Chr(13) & rtstr & "</div>" & Chr(13) & "</form>" & Chr(13)
        '  tbs(0) = "<script>$(function(){" & script & "});</script>"
        dt.Close()
        dc = Nothing
        'Return tbs
        Response.Write("<script>" & script & "</script>")
        For i = 0 To pg - 1
            Response.Write(tbs(i))
        Next
    End Function
    Public Function edit_del_list_wname_att3(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As Array
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        ' Dim sec As New k_security
        Dim arr() As String = {""}
        Dim tbs() As String = {"", ""}
        Dim pats As String
        Dim pg As Integer = 1
        Dim cc As Integer = 1
        Dim script As String = ""
        ' Response.Write(sql)
        rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        If dt.HasRows = True Then

            Dim color As String = "E3EAEB"
            Dim empid As String
            Dim datex As Boolean = False
            Dim colorx As String = ""
            rtstr &= "<form id='frms' method='post' name='frms' action=''>" & Chr(13)
            While dt.Read
                If fm.searcharray(arr, sec.Str2ToHex(dt.Item("att_date"))) = False Then
                    Dim arrbound As Integer = UBound(arr)
                    ReDim Preserve arr(arrbound + 1)
                    arr(arrbound) = sec.Str2ToHex(dt.Item("att_date"))

                    If pg = 1 Then
                        tbs(pg) = "<div id='pg" & pg & "'><div id='tabs-" & pg & "'><ul>" & Chr(13)
                        script &= "$(function(){$(' #tabs-" & pg & "').tabs();});" & Chr(13)
                        pg = 2

                    ElseIf cc Mod 30 = 0 Then
                        ReDim Preserve tbs(pg + 1)
                        colorx = RGB(Rnd(254), Rnd(253), Rnd(242))
                        'rtstr = rtstr & "</table>" & Chr(13) & "</div>"
                        tbs(pg) = "</ul>" & Chr(13) & rtstr & Chr(13) & "</div>" & Chr(13) & "</div>"
                        tbs(pg) &= "<div id='pg" & pg & "' style='background:" & colorx & "'>" & Chr(13) & "<div id='tabs-" & pg & "'>" & Chr(13) & "<ul>" & Chr(13)
                        script &= "$(function(){$(' #tabs-" & pg & "').tabs();});"
                        rtstr = ""
                        pg += 1

                    End If


                    If datex = True Then
                        rtstr &= "</table>" & Chr(13) & "</div>" & Chr(13)
                        datex = False
                    End If

                    cc += 1
                    If datex = False Then
                        tbs(pg - 1) &= "<li><a href='#" & sec.Str2ToHex(dt.Item("att_date")) & "'>" & dt.Item("att_date") & "</a></li>" & Chr(13)

                        rtstr &= "<div id='" & sec.Str2ToHex(dt.Item("att_date")) & "' style='display:none;'>" & Chr(13) & "<table  cellspacing='0' cellpadding='0'>" & Chr(13) & _
                                      "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>" & Chr(13) & "<td  style='padding-right:20px;'>Edit</td>" & Chr(13) & "<td  style='padding-right:20px;'>delete</td>" & Chr(13)
                        If heading <> "" Then
                            For i = 0 To hdr.Length - 1
                                rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>" & Chr(13)
                            Next
                        Else
                            For i = 1 To dt.FieldCount - 1
                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>" & Chr(13)
                            Next
                        End If
                        rtstr = rtstr & "</tr>" & Chr(13)
                        datex = True

                    End If
                End If


                empid = dt.Item("emptid")
                If color <> "#E3EAEB" Then
                    color = "#E3EAEB"
                Else
                    color = "#fefefe"
                End If
                rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & Chr(13) & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & Chr(13) & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>" & Chr(13)
                rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "(" & empid & ")</td>" & Chr(13)
                For k As Integer = 1 To dt.FieldCount - 2
                    If dt.Item(k).ToString = "y" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>" & Chr(13)
                    ElseIf dt.Item(k).ToString = "n" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>" & Chr(13)
                    ElseIf dt.GetName(k) = "department" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & Chr(13) & _
                        fm.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                        dt.Item(k).ToString & "'", con) & "</td>" & Chr(13)
                    ElseIf dt.GetName(k) = "project_id" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                        fm.getinfo2("select project_name from tblproject where project_id='" & _
                        dt.Item(k).ToString & "'", con) & "</td>" & Chr(13)
                    Else

                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>" & Chr(13)
                    End If

                Next
                rtstr = rtstr & "</tr>" & Chr(13)
            End While
        Else
            rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>" & Chr(13)
        End If
        rtstr = rtstr & "</table>" & Chr(13) & "</div>" & Chr(13)
        tbs(pg) &= "</ul>" & Chr(13) & rtstr & "</div>" & Chr(13) & "</form>" & Chr(13)
        '  tbs(0) = "<script>$(function(){" & script & "});</script>"
        dt.Close()
        dc = Nothing
        'Return tbs
        Response.Write("<script>" & script & "</script>")
        For i = 0 To pg - 1
            Response.Write(tbs(i))
        Next
    End Function
    Private Function getPageSQL2005(ByVal TableOrViewName As String, _
ByVal Columns As String, ByVal OrderColumn As String, _
ByVal OrderDirection As String, ByVal PageSize As Integer, _
ByVal SelectedPage As Integer, ByVal WhereClause As String) As String
        Return "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " & _
          OrderColumn & " " & OrderDirection & ") AS rownum, " & _
          Columns & " FROM " & TableOrViewName & _
          ") AS tmp WHERE rownum >= " & _
          ((SelectedPage - 1) * PageSize + 1).ToString() & _
          " AND rownum <= " & (SelectedPage * PageSize).ToString() & _
          IIf(WhereClause.Trim() <> "", " AND " & WhereClause, "")
    End Function
End Class
