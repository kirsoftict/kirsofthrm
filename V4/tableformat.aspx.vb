Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class tableformat
    Inherits System.Web.UI.Page
    Dim fm As New formMaker
    Function mkrow()

        Dim rowx As String
        'rowx = Request.Form("tbl") & "<br>" & Request.Form("sql") & "<br>" & Request.Form("tbl") & "<br>" & Request.Form("head") & "<br>" & Request.Form("loc")
        ' Response.Write(rowx)
        rowx = edit_del_list(Request.Form("tbl"), Request.Form("sql"), Request.Form("head"), Session("con"), Request.Form("loc"))
        Response.Write(rowx)
    End Function
    Function pagger()
        Dim rows As String
        Dim tsize, tpage, sh As Integer
        Dim pgn As Integer = Request.Form("pgno")


        Dim rtn As String = ""
        Dim rtn2 As String = "..."
        rows = fm.getinfo2("select count(*) from " & Request.Form("tbl"), Session("con"))
        If IsNumeric(rows) Then
            tsize = Request.Form("maxrows")
            tpage = CInt(rows) / CInt(tsize)
            ' Response.Write("Totalpage: " & tpage)
            rtn = "<div class='pagination'><ul>"
            If tpage > 10 Then
                sh = 10
            Else
                sh = tpage
            End If
            For k As Integer = 1 To sh
                If k = pgn Then
                    rtn &= "<li class='active'>" & k & "</li>"
                Else
                    rtn &= "<li onclick=" & Chr(34) & "javascript:changepg('" & k & "');" & Chr(34) & ">" & k & "</li>"
                End If

            Next
        End If
        rtn = rtn & rtn2 & "<input type='text' id='ppp' name='ppp' size='3' max='" & tpage & "' value='" & pgn & "' onkeyup=" & Chr(34) & "javascript:if(parseInt(this.value)>" & tpage & "){ alert('Maximum is " & tpage & "'); } else if(event.keyCode==13) { changepg(this.value);}" & Chr(34) & "> of " & tpage & " Pages"

        Response.Write(rtn & "</div>")
    End Function
    Function edit_del_list(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim dt As DataTableReader
        Dim fm As New formMaker
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer

        ' rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"

        rtstr &= "<div class='datalist'><form id='frms' method='post' name='frms' action='' ><table cellspacing='0' cellpadding='0' style='width:100%;'>" & Chr(13) & _
        "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>" & Chr(13) & _
        "<td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>" & Chr(13)
        Try


            dt = dc.dtmake(tbl & "x", sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;width:auto'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px; width:auto'>" & dt.GetName(i) & "</td>"

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
                    "<td  style='padding-right:20px;cursor:pointer;width:auto' onclick='javascript: goclickedx(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & Chr(13) & _
                    "<td  style='padding-right:20px;cursor:pointer;width:auto' onclick='javascript: goclickedx(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & Chr(13) & _
                    "<img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>" & Chr(13)
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.Item(k).ToString = "y" Then
                            rtstr = rtstr & "<td  style='padding-right:20px; width:auto'>Yes</td>"
                        ElseIf dt.Item(k).ToString = "n" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;widht:auto'>No</td>"
                        ElseIf dt.GetName(k) = "department" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;width:auto'>" & _
                            fm.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        ElseIf dt.GetName(k) = "project_id" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;width:auto'>" & _
                            fm.getinfo2("select project_name from tblproject where project_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        Else

                            rtstr = rtstr & "<td  style='padding-right:20px; width:auto'>" & dt.Item(k) & "</td>"
                        End If

                    Next
                    rtstr = rtstr & "</tr>" & Chr(13)
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>" & Chr(13)
            End If
            dt.Close()
        Catch ex As Exception
            rtstr &= ex.ToString & "<br>" & sql & tbl
        End Try
        rtstr = rtstr & "</table></form></div>"

        dc = Nothing

        Return rtstr

    End Function
End Class
