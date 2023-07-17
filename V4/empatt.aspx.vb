Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class empatt
    Inherits System.Web.UI.Page
    Public rema1(), rema2() As String
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
        Dim remark1() As String = {""}
        Dim remark2() As String = {""}
        Dim pgc As Integer

        If Request.Form("month") <> "" Then
            If Request.Form("projname") <> "" Then

                spl = Request.Form("projname").Split("|")
                If spl.Length <= 1 Then
                    ReDim spl(2)
                    spl(0) = Request.Form("projname")
                    spl(1) = ""
                End If

                projid = spl(1)
            End If
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim sys As New datetimecal
            Dim nod As Integer = 0
            Dim datename As String = ""
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            Dim date3 As Date = CDate(Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year"))
            Dim d1 As Date
            d1 = CDate(Request.Form("month") & "/1/" & Request.Form("year"))
            'outpx & =(projid)
            If projid <> "" Then
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = fm.getprojemp(projid.ToString, date3, Session("con"))
                ' getprojemp(projid.ToString, date3, Session("con"))
                'Response.Write()
                rs = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info " & _
                                "ON emprec.emp_id = emp_static_info.emp_id " & _
                                "where (emprec.id in(" & rtnvalue & ")) ORDER BY emp_static_info.first_name", Session("con"))

                rs = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.id in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & Request.Form("month") & "/1/" & Request.Form("year") & "' between date_from and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "') or date_from between '" & Request.Form("month") & "/1/" & Request.Form("year") & "'  and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "')))) ORDER BY emp_static_info.first_name", Session("con"))
                'outpx & =("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.id in(select emptid from emp_job_assign where project_id='" & projid & "' and '" & Request.Form("month") & "/1/" & Request.Form("year") & "' between date_from and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "'))) ORDER BY emp_static_info.first_name")
                pgc = fm.getinfo2("SELECT count(emprec.id) FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.id in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & Request.Form("month") & "/1/" & Request.Form("year") & "' between date_from and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "') or date_from between '" & Request.Form("month") & "/1/" & Request.Form("year") & "'  and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "'))))", Session("con"))
                ' Response.Write(pgc)
            Else
                'rs = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id ORDER BY emp_static_info.first_name", Session("con"))
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = fm.getprojemp(projid.ToString, date3, Session("con"))
                Response.Write(getprojemp(projid.ToString, date3, Session("con")))
                '  rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                 "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                "and emprec.id in(select emptid from emp_job_assign " & _
                '                                               "where project_id='" & projid.ToString & "' " & _
                '                                              "and ('" & pdate2 & "' >= date_from and '" & pdate2 & "' <= isnull(date_end,'" & Today.ToShortDateString & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                '                                             " ORDER BY emp_static_info.first_name,emprec.id desc "

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & d1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & date3 & _
                                                                  "') or emprec.hire_date between '" & d1 & "' and isnull(emprec.end_date,'" & date3 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
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
            End If
            Dim empid As String

            Dim datetoday As Date
            Dim clss As String = ""
            If Today.Day > 25 Then
                datetoday = date3
            Else
                datetoday = Today
            End If
            If Today.Month = Request.Form("month") And Today.Year = Request.Form("year") Then
                date1 = "#" & Request.Form("month") & "/1/" & Request.Form("year") & "#"
                nod = datetoday.Subtract(date1).Days + 1
                If nod > Date.DaysInMonth(Request.Form("year"), Request.Form("month")) Then
                    nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                End If
            Else
                nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            End If

            If rs.HasRows Then
                outpx &= ("<div id='listatt'>")

                outpx &= ("<table id='tb1' cellspacing='0' cellpadding='5' align='center' style='border: 1px black solid;text-align:left;font-size:8pt;'>")
                outpx &= ("<tr><td colspan='" & (nod + 10).ToString & "' style='text-align:center;'>")
                outpx &= ("<label style=" & Chr(34) & "font-weight:bold;" & Chr(34) & "> " & Session("company_name") & "<br />")
                outpx &= (" Project Name : ")
                If spl(0) <> "" Then
                    outpx &= (spl(0))
                Else
                    outpx &= ("ALL Projects")
                End If
                outpx &= ("<br />")
                outpx &= ("Attendance for the Month of " & MonthName(Request.Form("month")) & ", " & Request.Form("year"))
                outpx &= ("</label>")

                outpx &= ("</td>")
               
                Dim pp As Integer = 1

                outpx &= "</tr>"

                Dim px()() As String
                px = header(nod)
                sun = px(1)
                outpx &= px(0)(0)
                'Response.Write(px(0)(0))
                date1 = "#" & Request.Form("month") & "/" & nod & "/" & Request.Form("year") & "#"

                Dim color As String = "E3EAEB"
                Dim rdate As Date
                Dim j As Integer
                Dim date2 As Date
                Dim cal As Double
                Dim dateres As Date
                Dim stat As String

                If Today.Day > 25 Then
                    '     datetoday = date3
                Else
                    '  datetoday = Today
                End If

                Dim title As String = ""
                ' outpx & =(fm.getfullname(empid, Session("con")) & "<br>")
                Dim rt() As String
                Dim c As Integer = 1
                Dim t As Integer = 0
                Dim hc, wc, pc As Integer
                Dim trx As String
                While rs.Read
                    title = ""
                    t = 0
                    hc = 0
                    wc = 0
                    trx = ""
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    stat = ""
                    emptid = rs.Item("id")
                    empid = fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con"))
                    If datetoday > date1.AddDays(-1) Then

                        If rs.IsDBNull(4) = False Then
                            dateres = rs.Item(4)
                        Else
                            dateres = "1/1/1900"
                        End If
                        If dateres.Month = date1.Month And dateres.Year = date1.Year Then
                            stat = "inthis"
                            'Response.Write(emptid & "==>" & dateres.Month & "===" & date1.Month & "<br>")
                        ElseIf dateres = "1/1/1900" Then
                            stat = "active"

                        ElseIf dateres > date1 Then
                            stat = "past"
                        ElseIf dateres < date1 Then
                            stat = "inactive"
                        End If
                        rdate = rs.Item("hire_date")
                        Dim transf_flag As Boolean = False
                        Dim whrtranser As String = ""
                        Dim dtrans As Date = "1/1/1900"
                        Dim dtrasnstr As String
                        If istransfer(emptid, date3, Session("con")) Then
                            transf_flag = True

                        End If

                        date2 = "#" & Request.Form("month") & "/1/" & Request.Form("year") & "#"
                        If rdate.Month = Request.Form("month") And rdate.Year = Request.Form("year") Then
                            j = rdate.Day
                        Else
                            j = 1

                        End If

                        ' outp & =(rs.Item("emp_id") & "  ===" & rdate.ToLongDateString & "   " & date2.Subtract(rdate).Days.ToString & "<br>")
                        If date2.AddDays(j - 1).Subtract(rdate).Days >= 0 And stat <> "inactive" And dateres <> d1 Then
                            If istransfer(emptid, date3, Session("con")) Then
                                If fm.whrtrans(emptid, date3, Session("con")) = projid Then
                                    ' Response.Write("<br>transferd:" & projid.ToString)
                                End If
                            End If
                            If color <> "#E3EAEB" Then
                                color = "#E3EAEB"
                            Else
                                color = "#fefefe"
                            End If
                            outpx &= ("<tr style='background-color:" & color & "'>")

                            ' emptid = rs.Item("id")


                            ' empid = fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con"))
                            ' outpx & =(fm.getfullname(empid, Session("con")) & "<br>")
                            outpx &= ("<td class='nopx'>" & pp.ToString & "</td>")
                            '<td style='border-top: 1px solid black;border-left:1px solid black;' >" & empid & "</td>")
                            outpx &= ("<td style='' class='fname' title='" & emptid & "'>" & fm.getfullname(empid, Session("con")) & "</td>")
                            Dim position As String
                            position = fm.getinfo2("select position from emp_job_assign where emptid=" & emptid & " order by id desc", Session("con"))
                            outpx &= ("<td style='' class='post' >" & position & "</td>")

                            pp += 1
                            newe = 0
                            ca = 0
                            clwp = 0
                            cp = 0
                            cal = 0
                            pc = 0
                            Dim sv As Integer = 0
                            Dim r As Integer = 0
                            Dim cell2 As String = ""
                            Dim titleh As String = ""
                            For i As Integer = 1 To nod
                                titleh = ""
                                Dim colorp As String = ""
                                cell = ""
                                date1 = "#" & Request.Form("month") & "/" & i.ToString & "/" & Request.Form("year") & "#"
                                datename = WeekdayName(Weekday(date1), True)
                                lf = isOnleave2(emptid, date1, Session("con"))
                                Dim hf As String
                                If i >= j Then

                                    If fm.isAbs(emptid, date1, Session("con")) Then
                                        cell = "A"

                                        hf = fm.getinfo2("select daypartition from emp_att where emptid=" & emptid & " and att_date='" & date1 & "'", Session("con"))
                                        If hf = "M" Or hf = "A" Then
                                            cell = "A" & hf
                                            ca += 0.5

                                        Else
                                            ca += 1
                                        End If

                                    ElseIf fm.leaveab(lf(1)) = "lwp" Then
                                        hf = fm.getinfo2("select byhalfday from emp_leave_take where emptid=" & emptid & " and '" & date1 & "' between date_taken_from and date_return", Session("con"))
                                        If hf = "y" Then
                                            Response.Write(emptid & "<br>")
                                            clwp += 0.5
                                            cell = "h" & fm.leaveab(lf(1))
                                        Else
                                            clwp += 1
                                            cell = fm.leaveab(lf(1))
                                        End If

                                    ElseIf LCase(lf(1)) = "site visit" Then
                                        hf = fm.getinfo2("select byhalfday from emp_leave_take where emptid=" & emptid & " and '" & date1 & "' between date_taken_from and date_return", Session("con"))
                                        If hf = "y" Then
                                            sv += 0.5
                                        Else
                                            sv += 1
                                        End If

                                        cell = "SV"
                                    ElseIf sun(i) = "gray" Then
                                        If LCase(lf(0)) = "false" Then
                                            cell = "W"

                                        Else
                                            'outp & =(fm.leaveab(lf(1)))
                                            'cell2 = fm.leaveab(lf(1))

                                            cell = "W"
                                        End If
                                    ElseIf sun(i) = "yellow" Or sun(i) = "brown" Then
                                        If LCase(lf(0)) = "false" Then
                                            cell = "H"
                                            titleh = fm.getinfo2("select holiday_name from holidays where date_lie='" & date1 & "'", Session("con"))

                                        Else
                                            'cell2 = fm.leaveab(lf(1))
                                            cell = "H"
                                            titleh = fm.getinfo2("select holiday_name from holidays where date_lie='" & date1 & "'", Session("con"))

                                        End If
                                        'cell = "H"
                                    Else
                                        'lf = fm.isOnleave(emptid, date1, Session("con"))
                                        If LCase(lf(0)) = "false" Then
                                            cell = "P"
                                            'cell &= lf(1)
                                            cp += 1
                                        Else
                                            'cell2 = fm.leaveab(lf(1))
                                            If CDbl(lf(2)) > 0 Then
                                                cell = fm.leaveab(lf(1))
                                            Else
                                                cell = "P"
                                            End If


                                        End If
                                    End If
                                Else
                                    cell = "-"
                                    newe += 1
                                End If
                                If cell = "AL" Then
                                    ' Response.Write(projid & empid & datename & "===>" & lf(2) & "...<br>")
                                    '  If LCase(datename) = "sat" And projid = "01" Then
                                    'cal += 0.5
                                    'Response.Write(projid & empid & datename & "===>" & lf(2) & "...<br>")
                                    ' Else
                                    cell = fm.getinfo2("select project_id from emp_job_assign where emptid='" & emptid & "' and '" & date1 & "' between date_from and isnull(date_end,'" & Today & "')", Session("con"))

                                    If projid = cell Then
                                        cell = "AL"
                                        title = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                                    Else
                                        cell = "AL"
                                    End If
                                    cal += CDbl(lf(2))
                                    'End If

                                End If
                                If transf_flag = True Then
                                    '  Response.Write("select project_id from emp_job_assign where emptid='" & emptid & "' and '" & date1 & "' between date_from and isnull(date_end,'" & date3 & "')<br>")
                                    Try
                                        dtrasnstr = fm.getinfo2("select date_from from emp_job_assign where emptid=" & emptid & " and date_end is null", Session("con"))

                                    Catch ex As Exception
                                        'Response.Write("select date_from from emp_job_assign where emptid=" & emptid & " and date_end is null")
                                        fm.exception_hand(ex)
                                    End Try
                                    If IsDate(dtrasnstr) Then
                                        dtrans = dtrasnstr
                                        If fm.getinfo2("select project_id from emp_job_assign where emptid='" & emptid & "' and '" & date1 & "' between date_from and isnull(date_end,'" & date3 & "')", Session("con")) = projid Then

                                            cell = "P"
                                            '  Response.Write(fm.getinfo2("select date_from from emp_job_assign where emptid=" & emptid & " and date_end is null", Session("con")))
                                            '   rdate = fm.getinfo2("select date_from from emp_job_assign where emptid=" & emptid & " and date_end is null", Session("con"))
                                        Else
                                            whrtranser = fm.getinfo2("select project_id from emp_job_assign where emptid=" & emptid & " and date_end is null", Session("con"))
                                            ' Response.Write(emptid & "==>" & dtrans.Month & "===" & date1.Month & "====<:" & date3 & "====>" & whrtranser & "++++" & fm.whrtrans(emptid, date3, Session("con")) & "_________________________" & projid & "<br>")
                                            If CDate(date1).Subtract(dtrans).Days >= 0 Then
                                                cell = "T"
                                                cp = cp - 1
                                                t = t + 1
                                            Else
                                                cell = fm.getinfo2("select project_id from emp_job_assign where emptid='" & emptid & "' and '" & date1 & "' between date_from and isnull(date_end,'" & date3 & "')", Session("con"))
                                                title = fm.getinfo2("select project_name from tblproject where project_id='" & cell & "'", Session("con"))

                                                title = fm.getfullname(empid, Session("con")) & " has come from " & title

                                                cell = "T"
                                                t = t + 1
                                                cp = cp - 1
                                                If cell.Trim <> "" Then
                                                    '  Response.Write(UBound(remark1) & "<br>")
                                                    If fm.searcharray(remark1, title) = False Then '
                                                        ReDim Preserve remark1(c + 1)
                                                        remark1(c) = title
                                                        ' ReDim Preserve remark1(UBound(remark1) + 1)
                                                        ' remark1(UBound(remark1)) = ""
                                                        ReDim Preserve remark2(c + 1)
                                                        remark2(c) = c
                                                        ' Response.Write(c & "=>" & remark1(c) & "<br>")
                                                        ' remark1(UBound(remark2)) = ""
                                                        c += 1
                                                    End If
                                                End If
                                            End If
                                            'cell = "-"
                                            '  stat = "inthis"
                                        End If
                                    End If
                                Else
                                    whrtranser = fm.getinfo2("select project_id from emp_job_assign where emptid=" & emptid & " and date_end is null", Session("con"))
                                    ' Response.Write(emptid & "==>" & dtrans.Month & "===" & date1.Month & "====<:" & date3 & "====>" & whrtranser & "++++" & fm.whrtrans(emptid, date3, Session("con")) & "_________________________" & projid & "<br>")
                                    If CDate(date1).Subtract(dtrans).Days >= 0 Then
                                        'cell = "T"
                                        cp = cp - 1
                                        ' t = t + 1
                                    Else
                                        '  cell = fm.getinfo2("select project_id from emp_job_assign where emptid='" & emptid & "' and '" & date1 & "' between date_from and isnull(date_end,'" & date3 & "')", Session("con"))
                                        ' title = fm.getinfo2("select project_name from tblproject where project_id='" & cell & "'", Session("con"))

                                        ' title = fm.getfullname(empid, Session("con")) & " has come from " & title

                                        ' cell = "T"
                                        '   t = t + 1
                                        cp = cp - 1

                                    End If
                                End If
                                Dim temp As String = ""
                                title &= i.ToString
                                ' Response.Write(titleh)
                                If titleh <> "" Then
                                    title = titleh
                                End If
                                If cell = "H" Then
                                    If trx = "P" Then
                                        pc = pc + 1
                                    ElseIf trx = "T" Then
                                        t = t + 1
                                    End If
                                End If
                                If cell = "W" Then
                                    If trx = "P" Then
                                        pc = pc + 1
                                    ElseIf trx = "T" Then
                                        t = t + 1
                                    End If
                                End If
                                trx = cell
                                If cell = "P" Then
                                    pc = pc + 1
                                End If
                                If sun(i) = "" Then
                                    temp = sun(i)
                                    'If stat = "past" Then
                                    'sun(i) = "gray"

                                    'End If
                                    If stat = "inthis" Then
                                        If date1 >= dateres Then
                                            cell = "R"
                                            sun(i) = "red"
                                            r += 1
                                        End If
                                    End If

                                    If sun(i) = "" Then
                                        outpx &= ("<td class='attd' title='" & title & "' >" & cell & "</td>")
                                        title = ""
                                    Else
                                        outpx &= ("<td class='attd" & sun(i) & "' style='background-color:" & sun(i) & ";' title='" & title & "'>" & cell & "</td>")
                                    End If
                                    sun(i) = temp
                                Else
                                    temp = sun(i)
                                    ' If stat = "past" Then
                                    'sun(i) = "gray"
                                    'End If
                                    If stat = "inthis" Then
                                        If transf_flag = True Then
                                            If date1 > dtrans Then
                                                cell = "T"
                                                sun(i) = ""
                                                r += 1
                                            End If
                                            'cell = "T"


                                        ElseIf date1 >= dateres Then
                                            r += 1
                                            sun(i) = "red"
                                            cell = "R"
                                        End If
                                    End If
                                    If sun(i) = "" Then
                                        outpx &= ("<td class='attd' title='" & title & "'>" & cell & "</td>")
                                    Else
                                        outpx &= ("<td class='attd' style='background-color:" & sun(i) & ";' title='" & title & "'>" & cell & "</td>")
                                    End If
                                    sun(i) = temp
                                End If

                            Next
                            outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & ca.ToString & "</td>")
                            outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & clwp.ToString & "</td>")
                            outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & cal.ToString & "</td>")
                            outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & newe.ToString & "</td>")
                            outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & sv.ToString & "</td>")
                            outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & (nod - (ca + clwp + newe + r + t)).ToString & "</td>")
                            outpx &= ("</tr>")

                        End If
                    End If

                End While
                rema1 = remark1
                rema2 = remark2
                outpx &= ("<tr><td colspan='" & (nod + 8).ToString & "'>&nbsp;</td></tr>" & signpart())
                outpx &= "<tr><td style='text-align:left;border:1px 1px 1px 1px white solid;' colspan='30'>Remark:</td></tr>"

                For k As Integer = 1 To UBound(rema1) - 1
                    'Response.Write("<br>" & Request.Form("projname") & "===>" & rema1(k))
                    outpx &= "<tr><td style='text-align:left;border:1px 1px 1px 1px white solid;' colspan='30'>" & rema2(k) & " " & rema1(k) & "</td></tr>"


                Next

                outpx &= ("</table></div>")

             

            End If
        End If



        Return outpx
        'Return Nothing
    End Function

    Function header(ByVal nod As Integer)
        Dim outpx() As String = {""}
        Dim date1 As Date
        Dim datename As String
        Dim sys As New datetimecal
        Dim sun(32) As String
        outpx(0) &= ("<tr><td class='' style='border:1px black solid;'>No.</td><td class='fname' style='border:1px black solid;' >Emp. Name</td>" & _
                         "<td class='pos' style='border:1px black solid;'>Position</td>")
        Dim pp As Integer = 1
        For i As Integer = 1 To nod
            date1 = "#" & Request.Form("month") & "/" & i.ToString & "/" & Request.Form("year") & "#"
            'Weekday("#" & Request.Form("month") & "/" & i.ToString & "/" & Request.Form("year") & "#")

            datename = WeekdayName(Weekday(date1), True)

            If datename = "Sun" Then
                sun(i) = "gray"
            Else
                sun(i) = ""
            End If
            If sys.isPublic(date1, Session("con")) And datename <> "Sun" Then
                sun(i) = "yellow"
            ElseIf sys.isPublic(date1, Session("con")) And datename = "Sun" Then
                sun(i) = "brown"

            End If

            If sun(i) = "" Then
                outpx(0) &= ("<td class='toppx' style='border:1px black solid;'>" & datename.Substring(0, 1) & "<br>" & i.ToString & "</td>")

            Else
                outpx(0) &= ("<td class='toppx' style='background:" & sun(i) & ";border:1px black solid;'>" & datename.Substring(0, 2) & "<br>" & i.ToString & "</td>")

            End If
        Next
        outpx(0) &= ("<td class='toppx' style='border:1px black solid;' >A</td><td class='toppx' style='border:1px black solid;'>LWP</td><td class='toppx' style='border:1px black solid;' >AL</td><td class='toppx' style='border:1px black solid;'>New</td><td class='toppx' style='border:1px black solid;'>SV</td><td class='toppxx' style='border:1px black solid;' >Total Days on Job(" & nod.ToString & ")</td>")
        outpx(0) &= ("</tr>")
        Dim rtn(2)() As String

        rtn(0) = outpx
        rtn(1) = sun
        Return rtn
    End Function

    Function signpart()
        Dim outp As String = ""
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim rf() As String
        Dim count As Integer = 1
        Dim fm As New formMaker
        outp &= "<tr>"
        Try
            rs = dbs.dtmake("vwlty", "select * from tbl_leave_type", Session("con"))
            If rs.HasRows Then
                While rs.Read
                    If count Mod 5 = 1 Then
                        outp &= "</tr><tr>"
                    End If
                    If count = 1 Or count Mod 5 = 1 Then
                        outp &= "<td colspan='2' style='text-align:left;border:1px 1px 1px 1px white solid;'>" & rs.Item("abr") & "=" & rs.Item("leave_type") & "</td>"
                    Else
                        outp &= "<td  colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>" & rs.Item("abr") & "=" & rs.Item("leave_type") & "</td>"
                    End If
                    count += 1
                End While

            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
            fm.exception_hand(ex)
        End Try
        Dim sp() As String
        If File.Exists(Server.MapPath("abr") & "/abr.txt") Then
            rf = File.ReadAllLines(Server.MapPath("abr") & "/abr.txt")
            If rf.Length > 0 Then
                For k As Integer = 0 To rf.Length - 1

                    If IsError(rf(k)) = False And rf(k).Split(",").Length > 1 Then
                        sp = rf(k).Split(",")
                        ' Response.Write(sp(2) & "<br>")
                        If LCase(rf(k).Split(",")(2)) = "attendance" Then
                            If count Mod 5 = 1 Then
                                outp &= "</tr><tr>"
                            End If
                            If count = 1 Or count Mod 5 = 1 Then
                                outp &= "<td colspan='2' style='text-align:left;border:1px 1px 1px 1px white solid;'>" & rf(k).Split(",")(0) & "=" & rf(k).Split(",")(1) & "</td>"
                            Else
                                outp &= "<td  colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>" & rf(k).Split(",")(0) & "=" & rf(k).Split(",")(1) & "</td>"
                            End If
                            count += 1
                        End If
                    End If
                Next
            End If
        End If
        outp &= "</tr>"

        ' outp &= "<td colspan='2' style='width:100px;text-align:left;border:1px 1px 1px 1px white solid;'>L=Annual Leave</td>"

        '        outp &= "<td  colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>A= Absent</td>"

        '        outp &= "<td  colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>AA= Absent in the Afternoon</td>"
        '       outp &= "<td  colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>AM= Absent in the Monrning</td>"
        '      outp &= "<td  colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>P=Present at job</td>"

        '        outp &= "</tr><tr><td  colspan='2' style='width:100px;text-align:left;border:1px 1px 1px 1px white solid;'>SL=Sick Leave</td>"

        '        outp &= " <td colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>LWP=Leave Without Pay</td>"
        '       outp &= " <td colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>EL=Exam Leave</td>"
        '      outp &= " <td colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>SV=Site Visit</td>"
        '     outp &= " <td colspan='8' style='text-align:left;border:1px 1px 1px 1px white solid;'>T=Transfer</td>"
        '    outp &= "</tr>" & Chr(13)
        '   outp &= "<tr><td colspan='30' style='text-align:left;border:1px 1px 1px 1px white solid;'><table border='0' cellspacing='0' cellpadding='0'>"
        ' For k As Integer = 1 To UBound(rema1) - 1

        '    outp &= "<tr><td style='text-align:left;border:1px 1px 1px 1px white solid;'>" & rema2(k) & "</td><td style='text-align:left;border:1px 1px 1px 1px white solid;'>=</td><td style='text-align:left;border:1px 1px 1px 1px white solid;'>" & rema1(k) & "</td></tr>"


        ' Next

        outp &= ""
        outp &= "<tr>"

        outp &= "<td  style='height:20px;border:none;'>&nbsp;</td></tr><tr>"

        outp &= "<td colspan='30' style='text-align:left;border:1px 1px 1px 1px white solid;' >Checked By:-<span style='text-decoration:underline;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>"

        outp &= "</td>"
        outp &= "</tr><tr><td colspan='30' style='text-align:left;border:1px 1px 1px 1px white solid;'>Signature:-<span style='text-decoration:underline;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>"

        outp &= "</td>"

        outp &= "</tr><tr><td colspan='30' style='text-align:left;border:1px 1px 1px 1px white solid;'>Date:-<span style='text-decoration:underline;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>"

        outp &= "</td>"

        outp &= " </tr>" & Chr(13)

        Return outp
    End Function
    Function getprojemp(ByVal projid As String, ByVal d1 As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim did As String
        Dim r() As String
        Response.Write("select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and date_from between date_from and isnull(date_end,'" & d1 & "')  order by emp_id,emptid desc")
        rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and date_from between date_from and isnull(date_end,'" & d1 & "')  order by emp_id,emptid desc", con)
        If rs.HasRows Then
            While rs.Read
                r = fm.isResign(rs.Item("emptid"), con)
                'Response.Write(r(1))
                If IsDate(r(1)) Then
                    If CDate(r(1)).Month = d1.Month And CDate(r(1)).Year = d1.Year Then
                        Response.Write(r(1) & rs.Item("emptid") & ".............<br>")
                    End If

                Else
                    Response.Write(rs.Item("emptid") & ",")
                End If

            End While
        End If
    End Function
    Public Function isOnleave(ByVal emptid As Integer, ByVal stm As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim bol As Object = False
        Dim ret(3) As String
        Dim sql As String
        ret(0) = ""
        ret(1) = ""
        ret(2) = ""
        Dim dt As DataTableReader
        sql = "select * from emp_leave_take where date_taken_from <='" & stm & "' and date_return>='" & stm & "' and emptid='" & emptid & "' and approved_date is not null"
        dt = dbs.dtmake("attseek", sql, con)
        If dt.HasRows Then
            bol = True
            dt.Read()
            ret(1) = dt.Item("leave_type")
            If dt.Item("byhalfday") = "y" Then
                ret(2) = "0.5"
            ElseIf (CDbl(dt.Item("no_days")) - CInt(dt.Item("no_days"))) < 1 Then
                Try
                    If stm.AddDays(ret(2)) = CDate(dt.Item("date_return")) Then
                        ret(2) = "0.5"
                    Else
                        ret(2) = "1"
                    End If
                Catch ex As Exception
                    Response.Write(dt.Item("date_return"))
                    ret(2) = "1"
                End Try


            Else
                ret(2) = "1"
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
        sql = "select * from emp_leave_take where '" & stm & "' between date_taken_from and dateadd(day,-1,date_return) and emptid='" & emptid & "' and approved_date is not null"
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
    Function istransfer(ByVal emptid As Integer, ByVal d1 As Date, ByVal con As SqlConnection) As Boolean
        Dim fm As New formMaker
        Dim frm, hrd, jas As String
        Dim flg As Boolean = False
        frm = fm.getinfo2("select date_from from emp_job_assign where '" & d1.Month & "'=month(date_from) and '" & d1.Year & "'= isnull(year(date_from),0) and emptid=" & emptid, con)
        hrd = fm.getinfo2("select hire_date from emprec where '" & d1.Month & "'=month(hire_date) and '" & d1.Year & "'=year(hire_date) and emptid=" & emptid, con)
        ' Response.Write((hrd).ToString & "===Subtract" & frm & "====" & emptid & "<br>")
        jas = fm.getinfo2("select ass_for from emp_job_assign where '" & d1.Month & "'=month(date_from) and '" & d1.Year & "'= year(date_from) and emptid=" & emptid, con)
        '  Response.Write(hrd & "<br>")
        If IsDate(frm) And jas = "Job Assignment" Then
            flg = True
            '   Response.Write(hrd & emptid & "<br>")

        Else
            flg = False

        End If
        Return flg

    End Function
    Function attendance2()
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
        Dim remark1() As String = {""}
        Dim remark2() As String = {""}
        Dim pgc As Integer
        Session("projcoms") = ""
        Session("projtrans") = ""
        If Request.Form("month") <> "" Then
            If Request.Form("projname") <> "" Then

                spl = Request.Form("projname").Split("|")
                If spl.Length <= 1 Then
                    ReDim spl(2)
                    spl(0) = Request.Form("projname")
                    spl(1) = ""
                End If

                projid = spl(1)
            End If
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim sys As New datetimecal
            Dim nod As Integer = 0
            Dim datename As String = ""
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            Dim date3 As Date = CDate(Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year"))
            Dim d1 As Date
            d1 = CDate(Request.Form("month") & "/1/" & Request.Form("year"))
            'outpx & =(projid)
            If projid <> "" Then
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = fm.getprojemp(projid.ToString, date3, Session("con"))
                ' getprojemp(projid.ToString, date3, Session("con"))
                'Response.Write()
                rs = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info " & _
                                "ON emprec.emp_id = emp_static_info.emp_id " & _
                                "where (emprec.id in(" & rtnvalue & ")) ORDER BY emp_static_info.first_name", Session("con"))

                rs = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.id in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & Request.Form("month") & "/1/" & Request.Form("year") & "' between date_from and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "') or date_from between '" & Request.Form("month") & "/1/" & Request.Form("year") & "'  and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "')))) ORDER BY emp_static_info.first_name", Session("con"))
                'outpx & =("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.id in(select emptid from emp_job_assign where project_id='" & projid & "' and '" & Request.Form("month") & "/1/" & Request.Form("year") & "' between date_from and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "'))) ORDER BY emp_static_info.first_name")
                pgc = fm.getinfo2("SELECT count(emprec.id) FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.id in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & Request.Form("month") & "/1/" & Request.Form("year") & "' between date_from and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "') or date_from between '" & Request.Form("month") & "/1/" & Request.Form("year") & "'  and isnull(date_end,'" & Request.Form("month") & "/" & nod.ToString & "/" & Request.Form("year") & "'))))", Session("con"))
                ' Response.Write(pgc)
            Else
                'rs = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id ORDER BY emp_static_info.first_name", Session("con"))
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = fm.getprojemp(projid.ToString, date3, Session("con"))
                Response.Write(getprojemp(projid.ToString, date3, Session("con")))
                '  rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                 "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                "and emprec.id in(select emptid from emp_job_assign " & _
                '                                               "where project_id='" & projid.ToString & "' " & _
                '                                              "and ('" & pdate2 & "' >= date_from and '" & pdate2 & "' <= isnull(date_end,'" & Today.ToShortDateString & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                '                                             " ORDER BY emp_static_info.first_name,emprec.id desc "

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & d1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & date3 & _
                                                                  "') or emprec.hire_date between '" & d1 & "' and isnull(emprec.end_date,'" & date3 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
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
            End If
            Dim empid As String

            Dim datetoday As Date
            Dim clss As String = ""
            If Today.Day > 25 Then
                datetoday = date3
            Else
                datetoday = Today
            End If
            If Today.Month = Request.Form("month") And Today.Year = Request.Form("year") Then
                date1 = "#" & Request.Form("month") & "/1/" & Request.Form("year") & "#"
                nod = datetoday.Subtract(date1).Days + 1
                If nod > Date.DaysInMonth(Request.Form("year"), Request.Form("month")) Then
                    nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                End If
            Else
                nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            End If

            If rs.HasRows Then
                outpx &= ("<div id='listatt'>")

                outpx &= ("<table id='tb1' cellspacing='0' cellpadding='5' align='center' style='border: 1px black solid;text-align:left;font-size:8pt;'>")
                outpx &= ("<tr><td colspan='" & (nod + 10).ToString & "' style='text-align:center;'>")
                outpx &= ("<label style=" & Chr(34) & "font-weight:bold;" & Chr(34) & "> " & Session("company_name") & "<br />")
                outpx &= (" Project Name : ")
                If spl(0) <> "" Then
                    outpx &= (spl(0))
                Else
                    outpx &= ("ALL Projects")
                End If
                outpx &= ("<br />")
                outpx &= ("Attendance for the Month of " & MonthName(Request.Form("month")) & ", " & Request.Form("year"))
                outpx &= ("</label>")

                outpx &= ("</td>")

                Dim pp As Integer = 1

                outpx &= "</tr>"

                Dim px()() As String
                px = header(nod)
                sun = px(1)
                outpx &= px(0)(0)
                'Response.Write(px(0)(0))
                date1 = "#" & Request.Form("month") & "/" & nod & "/" & Request.Form("year") & "#"

                Dim color As String = "E3EAEB"
                Dim rdate As Date
                Dim j As Integer
                Dim date2 As Date
                Dim cal As Double
                Dim dateres As Date
                Dim stat As String

                If Today.Day > 25 Then
                    '     datetoday = date3
                Else
                    '  datetoday = Today
                End If

                Dim title As String = ""
                ' outpx & =(fm.getfullname(empid, Session("con")) & "<br>")
                Dim rt() As String
                Dim c As Integer = 1
                Dim t As Integer = 0
                Dim hc, wc, pc As Integer
                Dim trx As String
                While rs.Read
                    title = ""
                    t = 0
                    hc = 0
                    wc = 0
                    trx = ""
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    stat = ""
                    emptid = rs.Item("id")
                    empid = fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con"))
                    If datetoday > date1.AddDays(-1) Then

                        If rs.IsDBNull(4) = False Then
                            dateres = rs.Item(4)
                        Else
                            dateres = "1/1/1900"
                        End If
                        If dateres.Month = date1.Month And dateres.Year = date1.Year Then
                            stat = "inthis"
                            'Response.Write(emptid & "==>" & dateres.Month & "===" & date1.Month & "<br>")
                        ElseIf dateres = "1/1/1900" Then
                            stat = "active"

                        ElseIf dateres > date1 Then
                            stat = "past"
                        ElseIf dateres < date1 Then
                            stat = "inactive"
                        End If
                        rdate = rs.Item("hire_date")
                        Dim transf_flag As Boolean = False
                        Dim whrtranser As String = ""
                        Dim dtrans As Date = "1/1/1900"
                        Dim dtrasnstr As String
                        
                        If istransfer(emptid, date3, Session("con")) Then
                            transf_flag = True
                            Response.Write("trasfer: " & emptid & "</br>")
                        End If

                        date2 = "#" & Request.Form("month") & "/1/" & Request.Form("year") & "#"
                        If rdate.Month = Request.Form("month") And rdate.Year = Request.Form("year") Then
                            j = rdate.Day
                        Else
                            j = 1

                        End If

                        ' outp & =(rs.Item("emp_id") & "  ===" & rdate.ToLongDateString & "   " & date2.Subtract(rdate).Days.ToString & "<br>")
                        If date2.AddDays(j - 1).Subtract(rdate).Days >= 0 And stat <> "inactive" And dateres <> d1 Then
                            If istransfer(emptid, date3, Session("con")) Then
                                If fm.whrtrans(emptid, date3, Session("con")) = projid Then
                                    ' Response.Write("<br>transferd:" & projid.ToString)
                                End If
                            End If
                            If color <> "#E3EAEB" Then
                                color = "#E3EAEB"
                            Else
                                color = "#fefefe"
                            End If
                            outpx &= ("<tr style='background-color:" & color & "'>")

                            ' emptid = rs.Item("id")


                            ' empid = fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con"))
                            ' outpx & =(fm.getfullname(empid, Session("con")) & "<br>")
                            outpx &= ("<td class='nopx'>" & pp.ToString & "</td>")
                            Dim fname As String
                            fname = fm.getfullname(empid, Session("con"))
                            '<td style='border-top: 1px solid black;border-left:1px solid black;' >" & empid & "</td>")
                            outpx &= ("<td style='' class='fname' title='" & emptid & "'>" & fname & "</td>")
                            Dim position As String
                            position = fm.getinfo2("select position from emp_job_assign where emptid=" & emptid & " order by id desc", Session("con"))
                            outpx &= ("<td style='' class='post' >" & position & "</td>")

                            pp += 1
                            newe = 0
                            ca = 0
                            clwp = 0
                            cp = 0
                            cal = 0
                            pc = 0
                            Dim sv As Integer = 0
                            Dim r As Integer = 0
                            Dim cell2 As String = ""
                            Dim titleh As String = ""
                            Dim proj() As String

                            For i As Integer = 1 To nod
                                titleh = ""
                                Dim colorp As String = ""
                                cell = ""
                                date1 = "#" & Request.Form("month") & "/" & i.ToString & "/" & Request.Form("year") & "#"
                                datename = WeekdayName(Weekday(date1), True)
                                lf = isOnleave2(emptid, date1, Session("con"))

                               
                                Dim hf As String
                                If i >= j Then

                                    If fm.isAbs(emptid, date1, Session("con")) Then
                                        cell = "A"

                                        hf = fm.getinfo2("select daypartition from emp_att where emptid=" & emptid & " and att_date='" & date1 & "'", Session("con"))
                                        If hf = "M" Or hf = "A" Then
                                            cell = "A" & hf
                                            ca += 0.5

                                        Else
                                            ca += 1
                                        End If

                                    ElseIf fm.leaveab(lf(1)) = "lwp" Then
                                        hf = fm.getinfo2("select byhalfday from emp_leave_take where emptid=" & emptid & " and '" & date1 & "' between date_taken_from and date_return", Session("con"))
                                        If hf = "y" Then
                                            Response.Write(emptid & "<br>")
                                            clwp += 0.5
                                            cell = "h" & fm.leaveab(lf(1))
                                        Else
                                            clwp += 1
                                            cell = fm.leaveab(lf(1))
                                        End If

                                    ElseIf LCase(lf(1)) = "site visit" Then
                                        hf = fm.getinfo2("select byhalfday from emp_leave_take where emptid=" & emptid & " and '" & date1 & "' between date_taken_from and date_return", Session("con"))
                                        If hf = "y" Then
                                            sv += 0.5
                                        Else
                                            sv += 1
                                        End If

                                        cell = "SV"
                                    ElseIf sun(i) = "gray" Then
                                        If LCase(lf(0)) = "false" Then
                                            cell = "W"

                                        Else
                                            'outp & =(fm.leaveab(lf(1)))
                                            'cell2 = fm.leaveab(lf(1))

                                            cell = "W"
                                        End If
                                    ElseIf sun(i) = "yellow" Or sun(i) = "brown" Then
                                        If LCase(lf(0)) = "false" Then
                                            cell = "H"
                                            titleh = fm.getinfo2("select holiday_name from holidays where date_lie='" & date1 & "'", Session("con"))

                                        Else
                                            'cell2 = fm.leaveab(lf(1))
                                            cell = "H"
                                            titleh = fm.getinfo2("select holiday_name from holidays where date_lie='" & date1 & "'", Session("con"))

                                        End If
                                        'cell = "H"
                                    Else
                                        'lf = fm.isOnleave(emptid, date1, Session("con"))
                                        If LCase(lf(0)) = "false" Then
                                            cell = "P"
                                            'cell &= lf(1)
                                            cp += 1
                                        Else
                                            'cell2 = fm.leaveab(lf(1))
                                            If CDbl(lf(2)) > 0 Then
                                                cell = fm.leaveab(lf(1))
                                            Else
                                                cell = "P"
                                            End If


                                        End If
                                    End If
                                Else
                                    cell = "-"
                                    newe += 1
                                End If
                                If cell = "AL" Then
                                    ' Response.Write(projid & empid & datename & "===>" & lf(2) & "...<br>")
                                    '  If LCase(datename) = "sat" And projid = "01" Then
                                    'cal += 0.5
                                    'Response.Write(projid & empid & datename & "===>" & lf(2) & "...<br>")
                                    ' Else
                                    cell = fm.getinfo2("select project_id from emp_job_assign where emptid='" & emptid & "' and '" & date1 & "' between date_from and isnull(date_end,'" & Today & "')", Session("con"))

                                    If projid = cell Then
                                        cell = "all"
                                        title = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                                    Else
                                        cell = "AL"
                                    End If
                                    cal += CDbl(lf(2))
                                    'End If

                                End If
     
                        Dim temp As String = ""
                        title &= i.ToString
                        ' Response.Write(titleh)
                      
                        If cell = "H" Then
                            If trx = "P" Then
                                pc = pc + 1
                           
                            End If
                        End If
                        If cell = "W" Then
                            If trx = "P" Then
                                pc = pc + 1
                            ElseIf trx = "T" Then
                                t = t + 1
                            End If
                        End If
                        trx = cell
                        If cell = "P" Then
                            pc = pc + 1
                        End If
                        If sun(i) = "" Then
                            temp = sun(i)
                            'If stat = "past" Then
                            'sun(i) = "gray"

                            'End If
                            If stat = "inthis" Then
                                If date1 >= dateres Then
                                    cell = "R"
                                    sun(i) = "red"
                                    r += 1
                                End If
                            End If

                          

                        Else
                            temp = sun(i)
                            ' If stat = "past" Then
                            'sun(i) = "gray"
                            'End If
                            If stat = "inthis" Then
                                If date1 >= dateres Then
                                    r += 1
                                    sun(i) = "red"
                                    cell = "R"
                                End If
                                    End If
                                  
                          

                                End If
                                If cell = "P" Then
                                    proj = getproj_on_date(emptid, date1, Session("con"))
                                    If projid <> proj(0) Then
                                        cell = "T"
                                        t = t + 1
                                        cp = cp - 1
                                    End If

                                    Dim pcell, pproj(), fproj() As String
                                    If date1.AddDays(1).Day <= nod Then
                                    Else
                                        Response.Write("end of month")
                                    End If
                                    pproj = getproj_on_date(emptid, date1.AddDays(-1), Session("con"))
                                    fproj = getproj_on_date(emptid, date1.AddDays(1), Session("con"))
                                    If pproj(0) <> projid Then
                                        If Session("projcoms").ToString.Contains(pproj(1)) = False And Session("projcoms").ToString.Contains(fname) = False Then
                                            Session("projcoms") &= fm.getfullname(empid, Session("con")) & "has comes from " & pproj(1) & "<br>"
                                            ' Response.Write(Session("projtrans").ToString.Contains(pproj(1)))
                                        End If

                                    ElseIf fproj(0) <> projid Then
                                        Response.Write(Session("projtrans").ToString.Contains(pproj(1)))
                                        If Session("projtrans").ToString.Contains(fproj(1)) = False Then
                                            Session("projtrans") &= fm.getfullname(empid, Session("con")) & " Trasfer To " & fproj(1) & "<br>"
                                        End If

                                    End If

                                End If
                                If sun(i) = "" Then
                                    outpx &= ("<td class='attd' title='" & title & "'>" & cell & "</td>")
                                Else
                                    outpx &= ("<td class='attd' style='background-color:" & sun(i) & ";' title='" & title & "'>" & cell & "</td>")
                                End If
                                sun(i) = temp
                            Next
                        outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & ca.ToString & "</td>")
                        outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & clwp.ToString & "</td>")
                        outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & cal.ToString & "</td>")
                        outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & newe.ToString & "</td>")
                        outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & sv.ToString & "</td>")
                        outpx &= ("<td style='border-top: 1px solid black;border-left:1px solid black;' >" & (nod - (ca + clwp + newe + r + t)).ToString & "</td>")
                        outpx &= ("</tr>")

                    End If
                    End If

                End While
                rema1 = remark1
                rema2 = remark2
                outpx &= ("<tr><td colspan='" & (nod + 8).ToString & "'>&nbsp;</td></tr>" & signpart())
                outpx &= "<tr><td style='text-align:left;border:1px 1px 1px 1px white solid;' colspan='30'>Remark:</td></tr>"

                For k As Integer = 1 To UBound(rema1) - 1
                    'Response.Write("<br>" & Request.Form("projname") & "===>" & rema1(k))
                    outpx &= "<tr><td style='text-align:left;border:1px 1px 1px 1px white solid;' colspan='30'>" & rema2(k) & " " & rema1(k) & "</td></tr>"


                Next

                outpx &= ("</table></div>")
                outpx &= "<br>" & Session("projcoms") & Session("projtrans")

            End If
        End If



        Return outpx
        'Return Nothing
    End Function
    Public Function getproj_on_date(ByVal emptid As Integer, ByVal pd1 As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim rt(2) As String
        Dim hrd, sql As String
        Dim fm As New formMaker

        hrd = fm.getinfo2("select hire_date from emprec where id=" & emptid & " and hire_date<='" & pd1 & "'", con)
        If IsDate(hrd) Then
            If CDate(hrd).Subtract(pd1).Days <= 1 Then
                Try
                    sql = "select project_id,date_from,date_end from emp_job_assign where " & _
                           "emptid=" & emptid & " and '" & pd1 & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')"

                    rs = dbs.dtmake("dbsprojx", sql, con)
                    If rs.HasRows Then
                        While rs.Read()

                            ' Response.Write("<br>" & emptid.ToString & "===" & rs.Item("date_from"))

                            rt(0) = rs.Item("project_id")
                            rt(1) = dbs.getprojectname(rt(0), con)

                        End While
                    Else
                        rt(0) = "no Project"
                        rt(1) = "no Project"
                    End If
                    rs.Close()
                Catch ex As Exception
                    '   Response.Write(sql)
                    rt(1) = "no project"
                    rt(0) = "no Project"
                End Try
            End If
        Else
            rt(1) = "new"
            rt(0) = "new"
        End If
        '  rs = dbs.dtmake("dbsprojx", "select project_id,date_from,date_end from emp_job_assign where " & _
        '                   "emptid=" & emptid & " and ('" & pd1 & "' between date_from and isnull(date_end,'" & pd1.AddDays(Date.DaysInMonth(pd1.Year, pd1.Month) - 1) & "') or (month(date_from)='" & CDate(hrd).Month & "' and year(date_from)='" & CDate(hrd).Year & "')) order by date_from", con)


        dbs = Nothing
        Return rt
    End Function
End Class
