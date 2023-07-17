Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class view_report1
    Inherits System.Web.UI.Page
    Public Function makeformx(ByVal sql As String)
        Dim fullname, position, sal(), proj As String
        ' Dim sql As String = ""
        Dim nrow As Integer = 0
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim dt As DataTableReader
        Dim out As String = ""
        Dim fld(,) As String
        Dim empid(1) As String
        Dim active As String = ""
        Dim rowspan As Integer = 0
        Dim col(15) As String
        Dim color As String = ""
        empid(0) = ""
        Dim i As Integer = 0
        dt = dbs.dtmake("md", sql, session("con"))
        If dt.HasRows Then
            Response.Write(sql)
            out = ""
            While dt.Read
                rowspan = 0
                'Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)

                If fm.searcharray(empid, dt.Item("emp_id")) = False Then
                    ' Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)
                    ReDim Preserve empid(i + 1)
                    empid(i) = dt.Item("emp_id")

                    For j As Integer = 0 To 15
                        col(j) = ""
                    Next
                    col(0) = (i + 1).ToString
                    'Response.Write("<br>" & col(0))
                    fullname = fm.getfullname(dt.Item("emp_id"), Session("con"))
                    col(1) = "<a href='dataallview.aspx?vname=" & fullname & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fullname & "</a>"
                    col(2) = fm.getinfo2("select sex from emp_static_info where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    fld = fm.getqualification(dt.Item("emp_id"), Session("con"))
                    col(3) = dt.Item("hire_date")
                    position = fm.getinfo2("select position from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' order by id desc", Session("con"))
                    col(9) = "<a href='?val=bypost&position=" & position & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & position & "</a>"
                    active = dt.Item("active")
                    sal = dbs.getsal(dt.Item("id"), Session("con"))
                    col(10) = sal(0)
                    col(11) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & dt.Item("id") & " and istaxable='n' and to_date is null group by istaxable", Session("con"))
                    col(12) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & dt.Item("id") & " and to_date is null and istaxable='y' group by istaxable", Session("con"))
                    col(13) = fm.getinfo2("select pardime from emp_pardime where emptid=" & dt.Item("id") & " and to_date is null order by id desc", Session("con"))
                    col(15) = fm.getinfo2("select mob from emp_address where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    Dim projid As String = ""
                    projid = fm.getinfo2("select project_id from tblproject where project_id in(select project_id from tblproj_assign where emptid=" & dt.Item("id") & " and end_date is null)", Session("con"))
                    proj = fm.getinfo2("select project_name from tblproject where project_id in(select project_id from tblproj_assign where emptid=" & dt.Item("id") & " and end_date is null)", Session("con"))
                    col(14) = "<a href='?val=byproj&projx=" & projid & "|" & proj & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & proj & "</a>"

                    'col(13) = ""
                    'Response.Write(dt.Item("emp_id") & "data " & fld(0, 0) & "<br>")
                    rowspan = CInt(fld(0, 0))
                    If color = "#e8f1fa" Then
                        color = "white"
                    Else
                        color = "#e8f1fa"
                    End If
                    If active.Trim = "n" Then
                        color = "red"
                    End If
                    out &= "<tr style=' font-family:Times New Roman; height: 31.5pt;font-size:9pt; mso-height-source: userset;background-color:" & color & "'>" & Chr(13)
                    ' Response.Write(active & "<br>")

                    '  Response.Write(color)
                    For p As Integer = 0 To 15

                        If rowspan > 0 And (p >= 8 Or p <= 3) Then
                            If p > 9 And p <= 13 Then
                                out &= "<td rowspan='" & (rowspan).ToString & "' class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt; text-align:right; '>" & col(p) & "</td>" & Chr(13)
                            Else
                                out &= "<td rowspan='" & (rowspan).ToString & "' class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
              "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt; '>" & col(p) & "</td>" & Chr(13)

                            End If
                        ElseIf p > 3 And p < 8 Then
                            If rowspan = 0 Then
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(2, 0) & "</td>"
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt;'>" & fld(3, 0) & "</td>"
                                p = 8
                            Else
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(2, 0) & "</td>"
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt;'>" & fld(3, 0) & "</td>"
                                p = 8
                            End If
                        Else
                            If p > 9 And p <= 13 Then
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
             "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt;text-align:right;'>" & col(p) & "</td>"

                            Else
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
              "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt;'>" & col(p) & "</td>"

                            End If

                        End If
                    Next
                    out &= "</tr>"
                    If rowspan > 1 Then
                        For k As Integer = 1 To CInt(fld(0, 0)) - 1
                            out &= "<tr style=' font-family:Times New Roman; height: 31.5pt;font-size:9pt;background-color:" & color & "'><td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='?val=bydis&discipline=" & fld(4, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, k) & "</a></td>"
                            out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='?val=byqual&qualification=" & fld(1, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, k) & "</a></td>"
                            out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(2, k) & "</td>"
                            out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(3, k) & "</td></tr>"

                        Next

                    End If
                    i = i + 1
                End If

            End While

            'Response.Write("<table cellpading='2' cellspacing='2' bordercolor='blue'>" & out & "</table>")
        End If
        If out = "" Then
            out = "<tr><td colspan='15' style='color:red;font-size:30pt;'>Sorry! Data are not Found</td></tr>"
        End If
        Return out
    End Function
End Class
