Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm

Partial Class errorfind
    Inherits System.Web.UI.Page
    Function salary()
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        rs = ds.dtmake("ds", "SELECT emptid, COUNT(emptid) AS noo FROM emp_sal_info WHERE date_end Is NULL  GROUP BY emptid", Session("con"))
        If rs.HasRows = True Then
            Response.Write("<br><b> <u>Error on Salary</u></b>")
            While rs.Read
                If rs.Item("noo") >= 2 Then
                    Response.Write("<br>" & rs.Item("emptid"))
                    Response.Write(fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con")), Session("con")))

                End If
                ' Response.Write(rs.Item("noo") & "<br>")
            End While
        End If
        ds = Nothing
    End Function
    Function leaveerror()
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim rq, used, loan As Double
        rs = ds.dtmake("lerror", "select * from emp_leave_take where leave_type='Annual Leave'", Session("con"))
        If rs.HasRows Then
            Response.Write("<table><tr><td>Emp. Name</td><td>Req.id</td><td>Req. day</td><td>Used</td><td>Loan</td></tr>")
            While rs.Read
                rq = rs.Item("no_days")
                used = fm.getinfo2("select isnull(sum(used),0) from empleavapp where req_id=" & rs.Item("id"), Session("con"))
                If CDbl(used) - CInt(used) >= 0.5 Then
                    '   used = CInt(used) + 0.5

                ElseIf CDbl(used) - CInt(used) < 0.5 Then
                    '  used = CInt(used)
                End If
                loan = rq - used
                If loan > 0 Then
                    Response.Write("<tr><td>" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td><td>" & rs.Item("id") & "</td><td>" & rs.Item("no_days") & "</td><td>" & FormatNumber(used, 3).ToString & "</td><td>" & FormatNumber(loan, 2).ToString & "</td></tr>")
                End If
            End While
            Response.Write("</table>")
        End If
        rs.Close()
        fm = Nothing
        ds = Nothing

        leaver()
    End Function
    Function leaver()
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim used As Double
        Dim sql As String
        rs = ds.dtmake("lever", "select * from empleavapp order by id", Session("con"))
        If rs.HasRows Then
            While rs.Read
                used = rs.Item("used")
                sql = ""
                If rs.Item("used") = 0 Then
                    sql = "delete from empleavapp where id=" & rs.Item("id")
                    ds.save(sql, session("con"), session("path"))
                ElseIf CDbl(used) - CInt(used) >= 0.5 Then
                    used = CInt(used) + 0.5
                    sql = "update empleavapp set used=" & used & " where id=" & rs.Item("id")
                    ds.save(sql, session("con"), session("path"))

                ElseIf CDbl(used) - CInt(used) < 0.5 And CDbl(used) - CInt(used) > 0 Then

                    used = CInt(used)
                    sql = "update empleavapp set used=" & used & " where id=" & rs.Item("id")
                End If
                ' Response.Write("<br>" & sql)
            End While
        End If
        rs.Close()
        fm = Nothing
        ds = Nothing

    End Function
    Function leave_b_c()
        Dim rs, rs2 As DataTableReader
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim endyr As String = ""
        Dim ys, ye As String
        Dim result As String = ""
        Dim ks As New kirsoftsystem
        Dim sql As String = ""
        Dim noapp As String
        Dim notopend As String = ""
        Dim empid As String
        Dim rtn As String = ""
        Dim rtn2 As String = ""
        Dim flgapp As Boolean = False
        Dim flgbgt As Boolean = False
        Dim fx() As String = {""}
        If String.IsNullOrEmpty(Session("right")) = False Then
            fx = Session("right").split(";")
            ReDim Preserve fx(UBound(fx) + 1)
            fx(UBound(fx) - 1) = ""
        End If
        If (fm.searcharray(fx, "1") Or fm.searcharray(fx, "2")) Then

            rs = dbs.dtmake("activee", "select * from emprec where active='y'", Session("con"))
            If Session("passk") = "" Then
                Session("passk") = "493 15kir"
                Session("tosd") = "z.kirubel@gmail.com,kirsoftet@gmail.com"
            End If
            If rs.HasRows Then
                While rs.Read
                    ys = fm.getinfo2("select l_s_year from emp_leave_budget where emptid=" & rs.Item("id") & " order by l_s_year desc", Session("con"))
                    ye = fm.getinfo2("select l_e_year from emp_leave_budget where emptid=" & rs.Item("id") & " order by l_s_year desc", Session("con"))
                    empid = rs.Item("emp_id")
                    If ys = "None" Or ye = "None" Then
                        ' Response.Write(fm.getfullname("select emp_id from emprec where id=" & rs.Item("id"), Session("con")) & "select emp_id from emprec where id=" & rs.Item("id") & "<br>")
                        notopend &= "<tr><td>" & fm.getfullname(empid, Session("con")) & "</td><td>computer id:" & rs.Item("id") & "</td><td>emp.Id:" & empid & "</td><td>" & rs.Item("hire_date") & "</td><td>" & rs.Item("type_recuritment") & "</td><td>" & ys & "</td><td>" & ye & "</td></tr>"
                    Else

                        If CDate(ys).Subtract(Today).Days < 0 And CDate(ye).Subtract(Today).Days > 0 Then
                            Response.Write(rs.Item("id") & "===>" & ys & "==" & Today & "==>" & ye & "<br>")
                        Else
                            rtn &= "<tr><td>" & (empid & "Leave need new budget! its over since" & ye & "</td><td>")
                            rtn &= (fm.calc2(Session("con"), empid, rs.Item("id"), Session("path")) & "</td></tr><tr><td colspan=2>-------------------------------------</td></tr>")
                            flgbgt = True
                        End If
                    End If

                    sql = "select id from emp_leave_budget where approved is null and emptid='" & rs.Item("id") & "'"
                    result = fm.getinfo2(sql, Session("con"))
                    ' Response.Write(result & ">>>>" & sql & "<br>")
                    If IsNumeric(result) Then
                        noapp = fm.getjavalist2(" emp_leave_budget where approved is null and emptid=" & rs.Item("id"), "id", Session("con"), "!")

                        Response.Write("Not approved" & rs.Item("emp_id") & "(" & rs.Item("id") & ") line:" & noapp & "<br>")
                        flgapp = True

                    End If
                End While
                Response.Write("<b>No Leave information for this list</b><br>" & rtn & "<br>" & notopend)
                If rtn <> "" Or notopend <> "" Then
                    Dim msend As New mail_system

                    msend.sendemail("<table><tr><td clospan=4><b>The leave budget is not set</b></td></tr>" & notopend & "</table><br><table><tr><td colspan=2><b>Leave budget</b></td></tr>" & rtn & "</table>", Session("passk"), "no-replay@gmail.com", Session("eto"), "Leave information about budget")
                    If flgapp = True Or flgbgt = True Then
                        ' approvedall()''automatically approved disabled
                    End If
                End If

            End If
        End If
        dbs = Nothing
        fm = Nothing
    End Function
    Function approvedall()
        Dim sql As String
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs2 As DataTableReader
        sql = "select * from emprec where active='y' and end_date is null"
        rs = dbs.dtmake("vwallactive", sql, Session("con"))
        Dim msend As New mail_system
        Dim rtn As String = ""
        Dim empid As String = ""
        If rs.HasRows Then
            While rs.Read
                empid = rs.Item("emp_id")
                Try
                    sql = "select * from emp_leave_budget where emptid=" & rs.Item("id") & " and approved is null"
                    rs2 = dbs.dtmake("vwunapproved", sql, Session("con"))
                    If rs2.HasRows Then
                        While rs2.Read
                            sql = "update emp_leave_budget set approved='" & Now.ToString & "', who_approve='system' where id=" & rs2.Item("id")

                            '    Response.Write(sql)"
                            If dbs.excutes(sql, Session("con"), Session("path")) = 1 Then
                                rtn &= "<tr><td>" & fm.getfullname(empid, Session("con")) & "</td><td>computer id:" & rs.Item("id") & "</td><td>emp.Id:" & empid & "</td><td>buget id:" & rs2.Item("id") & "</td><td>dated: " & Now.ToString & "</td><td> APPROVED by the system</td></tr>"

                            Else
                                rtn &= "<tr style='color:red;'><td>" & fm.getfullname(empid, Session("con")) & "</td><td>computer id:" & rs.Item("id") & "</td><td>emp.Id:" & empid & "</td><td>buget id:" & rs2.Item("id") & "</td><td>dated: " & Now.ToString & "</td><td> APPROVED by the system</td></tr>"
                            End If
                            sql = ""
                        End While
                    End If
                    rs2.Close()
                Catch ex As Exception
                    Response.Write(ex.ToString & "<br>" & sql & "<br>")
                    ' msend.sendemail("<table><tr><td>" & ex.ToString & "<br>" & sql & "</td></tr></table>", Session("passk"), "no-reply@gmail.com", "z.kirubel@gmail.com", "Error msg")
                End Try



            End While
            If rtn <> "" Then
                msend.sendemail("<table>" & rtn & "</table>", Session("epwd"), Session("efrom"), Session("eto"), "Error on Leave budget", Session("smtp"), Session("eport"))

            End If
        End If
    End Function
    Function isrenewbgt(ByVal emptid As String)
        If IsNumeric(emptid) Then
            Dim fm As New formMaker

            Try

            Catch ex As Exception

            End Try
        End If
    End Function
End Class
