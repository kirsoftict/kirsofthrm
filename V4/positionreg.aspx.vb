Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class positionreg
    Inherits System.Web.UI.Page
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
                rtstr &= "<form id='frms' method='post' name='frms' action='' style='width:650px;'>"

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
    Function makerows2()

        Dim row As String
        Dim loc As String
        Dim sqlx As String
        Dim pd1 As String
        Dim empid() As String
        Dim fm As New formMaker
        If Session("posnumrows") = "" Then
            Session("posnumrows") = 0
        End If

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

    Function paging(ByVal no As Integer)

    End Function
End Class
