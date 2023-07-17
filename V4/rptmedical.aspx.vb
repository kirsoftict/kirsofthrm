Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports System.Security.AccessControl
Imports Kirsoft.hrm
Partial Class rptmedical
    Inherits System.Web.UI.Page
    Function mkform()
        Dim spl() As String
        Dim projid As String = ""
        Dim pdate1, pdate2 As Date
        Dim mm, yy As String
        Dim sspl() As String
        Dim nod As String
        Dim dbs As New dbclass
        Dim fm As New formMaker
        If Request.Form("projname") <> "" Then
            spl = Request.Form("projname").Split("|")

            If spl.Length > 1 Then
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            Else
                projid = ""
            End If
        End If

        If projid <> "" Then
            sspl = Request.Form("year").Split("-")
            If sspl.Length > 1 Then
                nod = Date.DaysInMonth(CDate(sspl(1)).Year, CDate(sspl(1)).Month)
                pdate1 = sspl(1)
                pdate2 = pdate1.Month & "/" & nod & "/" & pdate1.Year
            ElseIf sspl.Length > 0 Then
                pdate1 = sspl(0)
                pdate2 = pdate1.Month & "/" & nod & "/" & pdate1.Year
            Else

                pdate1 = "#1/1/0001#"
                pdate2 = "#1/1/0001#"

            End If

            Dim rs As DataTableReader
            Try
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                '  rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                 "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                "and emprec.id in(select emptid from emp_job_assign " & _
                '                                               "where project_id='" & projid.ToString & "' " & _
                '                                              "and ('" & pdate2 & "' >= date_from and '" & pdate2 & "' <= isnull(date_end,'" & Today.ToShortDateString & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                '                                             " ORDER BY emp_static_info.first_name,emprec.id desc "

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "
                'Response.Write(rssqlnew & "<br>")
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                                    "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                                  "and emprec.id in(" & rtnvalue & ") ORDER BY emp_static_info.first_name,emprec.id desc ")
                'Session("payrolllist") = rssqlnew
                'Response.Write(rtnvalue)
                rs = dbs.dtmake("selectemp", rssqlnew, Session("con"))
            Catch ex As Exception
                '  Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '  "where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '  "and (emprec.id in(select emptid from emp_job_assign " & _
                ' "where (project_id='" & projid.ToString & "') " & _
                '"and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "' or date_from between '" & pdate1 & "'  and isnull(date_end,'" & pdate2 & "'))))" & _
                '"ORDER BY emp_static_info.first_name,emprec.id desc ")

                Response.Write(ex.ToString)
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '"where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) " & _
                '"and (emprec.id in(select emptid from emp_job_assign " & _
                '"where (project_id='" & projid.ToString & "') " & _
                '"and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))))" & _
                '"ORDER BY emp_static_info.first_name,emprec.id desc ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))

            End Try
            Dim outp, body As String
            If rs.HasRows Then
                outp = "<table id='tblapp' cellspacing='0' cellpadding='3' >" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:12pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='5' >" & Session("company_name") & _
                "<br> Project Name:"

                outp &= spl(0).ToString

                outp &= "<br> Medical Report Period: " & MonthName(CDate(sspl(0)).Month, True) & " " & CDate(sspl(0)).Year.ToString & " - End of " & _
                  MonthName(CDate(sspl(1)).Month, True) & " " & CDate(sspl(0)).Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)
                outp &= "<tr  style='font-weight:bold;'>"
                outp &= "<td class='headp'>No.</td>"
                outp &= "<td class='headp'>Employee's Name</td>"
                outp &= "<td class='headp'>Year Budgeted</td>"
                outp &= "<td class='headp'>Used</td>"
                outp &= "<td class='headp'>Balance</td>"

                outp &= "<tr>"
                Dim rt() As String
                Dim emptid As String
                Dim rs2 As DataTableReader
                Dim cnt As Integer = 1
                Dim t_amt, u_amt, bal As Double
                Dim uamt As String
                While rs.Read
                    emptid = rs.Item(0)
                   
                            rs2 = dbs.dtmake("appris", "select * from emp_medical_all where emptid=" & emptid & " and date_from='" & sspl(0) & "' and date_exp='" & sspl(1) & "'", Session("con"))
                            t_amt = 0
                            u_amt = 0
                            bal = 0
                            If rs2.HasRows Then
                                rs2.Read()
                                t_amt = rs2.Item("mallwance")
                                uamt = fm.getinfo2("select sum(amt_used) from emp_medical_take where m_id='" & rs2("id") & "'", Session("con"))
                                If uamt = "None" Then
                                    u_amt = 0
                                ElseIf IsNumeric(uamt) Then
                                    u_amt = CDbl(uamt)
                                Else
                                    u_amt = 0
                                    Response.Write(uamt.ToString)
                                End If
                                bal = t_amt - u_amt
                                outp &= "<tr>"
                                outp &= "<td>" & cnt & "</td>"
                                outp &= "<td>" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td>"
                                outp &= "<td>" & FormatNumber(t_amt, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"
                                outp &= "<td>" & FormatNumber(u_amt, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"
                                outp &= "<td>" & FormatNumber(bal, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"

                                outp &= "<tr>"
                            End If
                            rs2.Close()
                        
                End While
                outp &= "</table>"
                Response.Write(outp)

            Else
                Response.Write("Sorry there is no data in the database")
            End If
            rs.Close()
        End If
    End Function
    Function mkform_all()
        Dim spl() As String
        Dim projid As String = ""
        Dim pdate1, pdate2 As Date
        Dim mm, yy As String
        Dim sspl() As String
        Dim nod As String
        Dim dbs As New dbclass
        Dim fm As New formMaker
        If Request.Form("projname") <> "" Then
            spl = Request.Form("projname").Split("|")

            If spl.Length > 1 Then
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            Else
                projid = ""
            End If
        End If

        If Request.Form("year") <> "" Then
            sspl = Request.Form("year").Split("-")
            If sspl.Length > 1 Then
                nod = Date.DaysInMonth(CDate(sspl(1)).Year, CDate(sspl(1)).Month)
                pdate1 = sspl(1)
                pdate2 = pdate1.Month & "/" & nod & "/" & pdate1.Year
            ElseIf sspl.Length > 0 Then
                pdate1 = sspl(0)
                pdate2 = pdate1.Month & "/" & nod & "/" & pdate1.Year
            Else

                pdate1 = "#1/1/0001#"
                pdate2 = "#1/1/0001#"

            End If

            Dim rs As DataTableReader
            Try
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
            Catch ex As Exception
                '  Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '  "where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '  "and (emprec.id in(select emptid from emp_job_assign " & _
                ' "where (project_id='" & projid.ToString & "') " & _
                '"and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "' or date_from between '" & pdate1 & "'  and isnull(date_end,'" & pdate2 & "'))))" & _
                '"ORDER BY emp_static_info.first_name,emprec.id desc ")

                Response.Write(ex.ToString)
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '"where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) " & _
                '"and (emprec.id in(select emptid from emp_job_assign " & _
                '"where (project_id='" & projid.ToString & "') " & _
                '"and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))))" & _
                '"ORDER BY emp_static_info.first_name,emprec.id desc ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))

            End Try
            Dim outp, body As String
            If rs.HasRows Then
                outp = "<table id='tblapp' cellspacing='0' cellpadding='3' >" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:12pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='5' >" & Session("company_name") & _
                "<br> Project Name:"

                outp &= "All Projects"

                outp &= "<br> Medical Report Period: " & MonthName(CDate(sspl(0)).Month, True) & " " & CDate(sspl(0)).Year.ToString & " - End of " & _
                  MonthName(CDate(sspl(1)).Month, True) & " " & CDate(sspl(0)).Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)
                outp &= "<tr  style='font-weight:bold;'>"
                outp &= "<td class='headp'>No.</td>"
                outp &= "<td class='headp'>Employee's Name</td>"
                outp &= "<td class='headp'>Year Budgeted</td>"
                outp &= "<td class='headp'>Used</td>"
                outp &= "<td class='headp'>Balance</td>"

                outp &= "<tr>"
                Dim rt() As String
                Dim emptid As String
                Dim rs2 As DataTableReader
                Dim cnt As Integer = 1
                Dim t_amt, u_amt, bal As Double
                Dim uamt As String
                While rs.Read
                    emptid = rs.Item(0)
                   
                            rs2 = dbs.dtmake("appris", "select * from emp_medical_all where emptid=" & emptid & " and date_from='" & sspl(0) & "' and date_exp='" & sspl(1) & "'", Session("con"))
                            t_amt = 0
                            u_amt = 0
                            bal = 0
                            If rs2.HasRows Then
                                rs2.Read()
                                t_amt = rs2.Item("mallwance")
                                uamt = fm.getinfo2("select sum(amt_used) from emp_medical_take where m_id='" & rs2("id") & "'", Session("con"))
                                If uamt = "None" Then
                                    u_amt = 0
                                ElseIf IsNumeric(uamt) Then
                                    u_amt = CDbl(uamt)
                                Else
                                    u_amt = 0
                                    Response.Write(uamt.ToString)
                                End If
                                bal = t_amt - u_amt
                                outp &= "<tr>"
                                outp &= "<td>" & cnt & "</td>"
                                outp &= "<td>" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td>"
                                outp &= "<td>" & FormatNumber(t_amt, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"
                                outp &= "<td>" & FormatNumber(u_amt, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"
                                outp &= "<td>" & FormatNumber(bal, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"

                                outp &= "<tr>"
                            End If
                            rs2.Close()
                       
                End While
                outp &= "</table>"
                Response.Write(outp)

            Else
                Response.Write("Sorry there is no data in the database")
            End If
            rs.Close()
        End If
    End Function

End Class

