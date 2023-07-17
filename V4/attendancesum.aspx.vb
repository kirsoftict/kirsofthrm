Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class attendancesum
    Inherits System.Web.UI.Page
    Function attendance()
        Dim date1 As Date
        Dim emptid As Integer
        Dim fm As New formMaker
        Dim projid As String = ""
        Dim name, project As String
        Dim spl(3) As String

        Dim cp As Double = 0
        Dim ca As Double = 0
        Dim clwp As Double = 0
        Dim newe As Integer = 0
        Dim cell As String = ""
        Dim lf() As String
        Dim flg As Integer = 0
        Dim sun(32) As String
        Dim outpx As String = ""

        If Request.QueryString("month") <> "" Or Request.Form("month") <> "" Then
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim sys As New datetimecal

            Dim nod As Integer = 0
            Dim datename As String = ""
            Dim ethdate As String
            nod = 30 ' Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            Dim d1 As Date
            Dim mm, yy As String
            If IsNumeric(Request.Form("month")) Then
                mm = Request.Form("month")
                yy = Request.Form("year")
                d1 = mm & "/1/" & yy
                nod = Date.DaysInMonth(yy, mm)
            Else
                mm = Request.QueryString("month")
                yy = Request.QueryString("year")
                d1 = mm & "/1/" & yy
                nod = Date.DaysInMonth(yy, mm)
            End If


            Dim date3 As Date = mm & "/" & nod.ToString & "/" & yy
            ' Response.Write(d1.ToShortDateString)
            'outpx & =(projid)

            'rs = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id ORDER BY emp_static_info.first_name", Session("con"))
            Dim rtnvalue, rssqlnew As String
            '   rtnvalue = fm.getprojemp(projid.ToString, date3, Session("con"))
            ' Response.Write(getprojemp(projid.ToString, date3, Session("con")))
            ' 
            Try
                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                             "where ('" & d1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & date3 & _
                                                                             "') or emprec.hire_date between '" & d1 & "' and isnull(emprec.end_date,'" & date3 & "' )) " & _
                                                                               " ORDER BY emp_static_info.first_name,emprec.id desc "
           
            'outpx & =(rssqlnew & "<br>")
            ' outpx & =("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
            '                                                                    "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
            '                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
            '                                                                  "and emprec.id in(" & rtnvalue & ") ORDER BY emp_static_info.first_name,emprec.id desc ")
            'Session("payrolllist") = rssqlnew
            'outpx & =(rtnvalue)
            rs = dbx.dtmake("selectemp", rssqlnew, Session("con"))
            flg = 1

            Dim empid As String

            Dim datetoday As Date
            Dim clss As String = ""
            Dim attstar As String = ""
            'Response.Write(d1 & ">>>>><br>")

            If rs.HasRows = False Then
                Response.Write("No Data found")
                End If

            If rs.HasRows Then
                outpx &= ("<div id='listatt'>")

                outpx &= ("<table id='tb1' cellspacing='0' cellpadding='5' align='center' style='border: 1px black solid;text-align:left;'>")
                outpx &= ("<tr><td colspan='" & (nod + 10).ToString & "' style='text-align:center;'>")
                outpx &= ("<label style=" & Chr(34) & "font-weight:bold;" & Chr(34) & "> " & Session("company_name") & "<br />")
                outpx &= (" Project Name : ")

                outpx &= ("ALL Projects")

                outpx &= ("<br />")
                    outpx &= ("Attendance for the Month of " & MonthName(mm) & ")" & ", " & yy)
                    outpx &= "<br>(" & d1 & "===" & date3 & ")"
                outpx &= ("</label>")

                outpx &= ("</td>")
                outpx &= ("<tr><td class='' style='border:solid 1px black;'>No.</td><td class='toppx' >Emp. Name</td>" & _
                          "<td class='toppx'>Position</td>")
                Dim pp As Integer = 1

                outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >A</td>" & _
                          "<td style='border-top: 1px solid black;border-left:1px solid black;' >LWP</td>" & _
                          "<td style='border-top: 1px solid black;border-left:1px solid black;' >AL</td>" & _
                          "<td style='border-top: 1px solid black;border-left:1px solid black;' >New</td>" & _
                           "<td style='border-top: 1px solid black;border-left:1px solid black;' >res</td>" & _
                          "<td style='border-top: 1px solid black;border-left:1px solid black;' >Total Days on Job(" & nod.ToString & ")</td>")
                outpx &= ("</tr>")
                Dim color As String = "E3EAEB"
                Dim rdate As Date
                Dim j As Integer
                Dim date2 As Date
                Dim cal As Double
                Dim dateres As Date
                Dim stat As String

              
                ' date1 = d1

                ' outpx & =(fm.getfullname(empid, Session("con")) & "<br>")
                ' Dim rt() As String
                While rs.Read
                    ' Response.Write("rrrrsssreeeddd")
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    'Response.Write("xxxxxxxxxxxxxxxx")
                    emptid = rs.Item("id")

                    empid = fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con"))
                        ' Response.Write(datetoday & "====" & d1.AddDays(-1) & "<br>")
                    Try


                            
                            ' outp & =(rs.Item("emp_id") & "  ===" & rdate.ToLongDateString & "   " & date2.Subtract(rdate).Days.ToString & "<br>")


                            If color <> "#E3EAEB" Then
                                color = "#E3EAEB"
                            Else
                                color = "#fefefe"
                            End If
                            newe = 0
                            ca = 0
                            clwp = 0
                            cp = 0
                            cal = 0
                            Dim sv As Integer = 0
                            Dim r As Integer = 0
                            Dim cell2 As String = ""
                            'Response.Write(nod)
                            ca = fm.catt(emptid, Session("con"), d1, date3)
                            clwp = fm.lwpinmonth(d1, date3, emptid, Session("con"))
                            'getinfo2("select count(id) from emp_att where status='A' and  daypartition='F' and att_date between '" & d1 & "' and '" & date3 & "' and emptid='d 
                            If d1 < rs.Item("hire_date") Then
                                newe = nod - (date3.Subtract(rs.Item("hire_date")).days) - 1

                            End If
                            Dim did As String = ""
                            If (rs.Item("active") = "n") Then
                                did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "' and resign_date>'" & d1 & "'", Session("con"))
                                ' Response.Write("yyy")
                            End If
                            If IsDate(did) Then
                                If CDate(did).Subtract(CDate(date3)).Days < 0 Then
                                    r = date3.Subtract(CDate(did)).Days + 1

                                End If

                            End If
                            If ca > 0 Or newe > 0 Or r > 0 Or clwp > 0 Then
                                outpx &= ("<tr style='background-color:" & color & "'>")

                                ' emptid = rs.Item("id")


                                ' empid = fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con"))
                                ' outpx & =(fm.getfullname(empid, Session("con")) & "<br>")
                                outpx &= ("<td class='nopx'>" & pp.ToString & "</td>")
                                '<td style='border-top: 1px solid black;border-left:1px solid black;' >" & empid & "</td>")
                                outpx &= ("<td style='' class='fname' >" & fm.getfullname(empid, Session("con")) & "</td>")
                                Dim position As String
                                position = fm.getinfo2("select position from emp_job_assign where emptid=" & emptid & " order by id desc", Session("con"))
                                outpx &= ("<td style='' class='post' >" & position & "</td>")



                                outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & ca.ToString & "</td>")
                                outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & clwp.ToString & "</td>")
                                outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & cal.ToString & "</td>")
                                outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & newe.ToString & "</td>")
                                outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & r.ToString & "</td>")
                                outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & (nod - (ca + clwp + newe + r)).ToString & "</td>")
                                outpx &= ("</tr>")
                                pp += 1
                            End If

                        Catch ex As Exception
                Response.Write(ex.ToString)
                        End Try

                End While

                outpx &= ("</table></div>")


                End If
            Catch ex2 As Exception

                Response.Write(ex2.ToString & "==>")
                Response.Write(d1.ToString & "=====> " & date3 & "<br>")
                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                                " ORDER BY emp_static_info.first_name,emprec.id desc "
            End Try

        End if


        Return outpx
        'Return Nothing
    End Function
    Public Function isOnleave2(ByVal emptid As Integer, ByVal stm As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim bol As Object = False
        Dim ret(3) As String
        Dim sql As String
        Dim fm As New formMaker
        ret(0) = ""
        ret(1) = ""
        ret(2) = ""
        Dim dt As DataTableReader
        sql = "select * from emp_leave_take where '" & stm & "' between date_taken_from and date_return and emptid='" & emptid & "' and approved_date is not null"
        dt = dbs.dtmake("attseek", sql, con)
        If dt.HasRows Then
            bol = True
            dt.Read()
            ret(1) = dt.Item("leave_type")
            'Response.Write("<br>" & emptid & "==>" & DateDiff("d", stm, dt.Item("date_return")).ToString)
            If dt.Item("byhalfday") = "y" Then
                If DateDiff("d", stm, dt.Item("date_return")) > 0 Then
                    ret(2) = "0.5"
                Else
                    ret(2) = "0"
                End If

            ElseIf DateDiff("d", stm, dt.Item("date_return")) > 0 Then
                ' Response.Write("<br>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con")), Session("con")) & "==>" & DateDiff("d", stm, dt.Item("date_return")).ToString)
                ret(2) = "1"
            Else
                ret(2) = "0"
            End If
        Else
            ret(1) = sql
        End If
        ' ret(2) = sql
        dt.Close()
        dt = Nothing
        dbs = Nothing
        ret(0) = bol.ToString
        Return ret
    End Function
End Class
