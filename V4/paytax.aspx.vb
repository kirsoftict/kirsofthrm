Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm

Partial Class paytax
    Inherits System.Web.UI.Page
    Public Function edit_del_list(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim dt As DataTableReader
        Dim hdr() As String
        Dim fm As New formMaker
        hdr = heading.Split(",")
        Dim i As Integer
        rtstr = "<script type='text/javascript'>" & Chr(13) & _
        "function goclicked(whr,id)" & Chr(13) & _
        "{  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());" & Chr(13) & _
        "$('#frms').submit();}</script>" & Chr(13)
        rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & Chr(13) & _
        "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>" & Chr(13) & _
        "<td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>" & Chr(13)
        Try

        
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
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

        If dt.HasRows = True Then
            Dim color As String = "E3EAEB"
            While dt.Read
                If color <> "#E3EAEB" Then
                    color = "#E3EAEB"
                Else
                    color = "#fefefe"
                End If
                rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & Chr(13) & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & Chr(13) & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & Chr(13) & _
                "<img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>" & Chr(13)
                For k As Integer = 1 To dt.FieldCount - 1
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
                rtstr = rtstr & "</tr>" & Chr(13)
            End While
        Else
            rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>" & Chr(13)
            End If
            rtstr = rtstr & "</table></form>"
            dt.Close()
            dc = Nothing
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try

        Return rtstr

    End Function
End Class
