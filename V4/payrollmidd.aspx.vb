Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class payrollx
    Inherits System.Web.UI.Page
    Public Function makeform()
        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim csal(2) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim bsal, dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
        Dim sumbsal, sumbearn, sumtalw, sumfsal, sumssal, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double
        sumbsal = 0
        sumbearn = 0
        sumtalw = 0
        sumalw = 0
        sumot = 0
        sumgross = 0
        sumtincome = 0
        sumtax = 0
        sumpemp = 0
        sumpco = 0
        sumtd = 0
        sumnet = 0
        Dim sal As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        If Request.Form("month") <> "" Then
            Dim spl() As String
            Dim projid As String = ""
            If Request.Form("projname") <> "" Then
                spl = Request.Form("projname").Split("|")

                If spl.Length > 1 Then
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                Else
                    projid = ""
                End If
            End If
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            Dim rs As DataTableReader
            Dim rs2 As DataTableReader
            Dim ccol As Integer = 0
            Dim paid As String
            Dim j As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If
            paid = ""
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx ref_inc is null or ref_inf='0')", Session("con"))
            If paid = "None" Then
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vwloanbal WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            Else
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vwloanbal WHERE bal>0 ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            End If
            'Response.Write(ccol)
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            strc = (fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", Session("con")))
            If strc.ToString.Length > 3 Then
                pemp = 0
            Else
                pemp = CDbl(strc)
            End If
            strc = "0"
            strc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", Session("con"))
            If strc.Length > 3 Then
                pco = "0"
            Else
                pco = CDbl(strc)
            End If
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ", Session("con"))
            End If

            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3' style='width:1300px;font-size:9pt;' border='0'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='16' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td rowspan='2'>No.</td><td style='width:90px;' rowspan='2'>Emp. Id</td>"
                outp &= "<td style='width:250px;' rowspan='2'>Full Name</td>"
                outp &= "<td   colspan='3'>Basic Salary</td>" & Chr(13)
                outp &= "<td  rowspan='2'>Days Worked</td>"
                outp &= "<td  rowspan='2'>Basic Earning</td>"
                outp &= "<td  rowspan='2'>Taxable Allowance</td>"
                outp &= "<td   rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td  rowspan='2'> Overtime</td>"
                outp &= "<td  rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  rowspan='2'>Taxable Incom</td>"
                outp &= "<td  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td rowspan='2'>Net Pay</td>"
                outp &= "<td  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                outp &= "<td rowspan='2'>Signature</td></tr>" & Chr(13)
                If paid = "None" Then
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

                Else
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0 and dstart < ='" & pdate1 & "'", Session("con"))

                End If
                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr><td>Prev.<br>Salary</td><td>New Salary</td><td>Salary</td>"
                outp &= "" & Chr(13)
                If ccol = 0 Then
                    outp &= "<td ></td>"
                Else
                    If rs2.HasRows Then
                        Dim i As Integer = 0
                        While rs2.Read

                            If rs2.Item("reason") = "-" Then
                                outp &= "<td >Other</td>"
                            Else
                                outp &= "<td >" & rs2.Item("reason").ToString & "</td>"
                            End If
                            reasonname(i) = rs2.Item("reason")
                            i += 1
                        End While
                    End If
                End If


                outp &= "</tr>" & Chr(13)
                rs2.Close()
                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date
                While rs.Read
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    resing = "#1/1/1900#"
                    If color <> "#aabbcc" Then
                        color = "#aabbcc"
                    Else
                        color = "white"
                    End If
                    emptid = rs.Item("id")

                    If (rs.Item("active") = "n") Then

                        resing = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))
                    End If
                    Dim dateincr As Date
                    Dim incwhen As Integer = 0
                    avalue = ""
                    avalue = fm.getinfo2("select date_start from emp_sal_info where emptid=" & emptid & " order by id desc", Session("con"))
                    If avalue <> "None" Then
                        dateincr = avalue
                    End If
                    Dim dhr As Date
                    dhr = rs.Item("hire_date")
                    If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year And dhr.Month <> pdate1.Month And dhr.Year <> pdate1.Year Then
                        'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                        incwhen = dateincr.Subtract(pdate1).Days
                    End If
                    If incwhen > 0 Then 'question..........solved
                        cell(0) = rs.Item("emp_id")
                        cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                        sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                        ' Response.Write(sql & "<br>")
                        'cell(2) = fm.getinfo2(sql, Session("con")).ToString
                        csal(0) = fm.getinfo2(sql, Session("con")).ToString

                        If csal(0) = "None" Then
                            csal(0) = "0"
                        End If
                        sql = "select basic_salary from emp_sal_info where date_end is null and emptid=" & rs.Item("id").ToString
                        csal(1) = fm.getinfo2(sql, Session("con")).ToString
                        If csal(1).ToString = "None" Then
                            csal(1) = "0"
                        End If
                        csal(0) = (CDbl(csal(0)) / nod) * incwhen
                        csal(1) = (CDbl(csal(1)) / nod) * (nod - incwhen)


                        Dim dblv As Double = CDbl(csal(0)) + CDbl(csal(1))
                        cell(2) = dblv.ToString
                        'Response.Write(csal(0))
                        'cell(2) = "0"
                        If cell(2) = "None" Then
                            cell(2) = "0"
                            '  color = "#ccaa99"
                        End If
                        paid = ""
                        paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select payroll_id from paryrol where emptid='" & emptid & "')", Session("con"))
                        'Response.Write(paid)
                        ca = fm.catt(emptid, Session("con"), pdate1, pdate2)
                        'Response.Write(ca.ToString & "<==a")
                        clwp = Math.Round(CDbl(fm.lwpinmonth(pdate1, pdate2, emptid, Session("con"))), 2)
                        ' Response.Write("lwp==>" & clwp.ToString & "<br>")
                        newemp = 0
                        If pdate1 < rs.Item("hire_date") Then
                            newemp = nod - (pdate2.Subtract(rs.Item("hire_date")).days) - 1
                        End If
                        Dim fired As Double = 0


                        'nod = Today.Day
                        If resing <> "#1/1/1900#" Then
                            If pdate1.Month = resing.Month And pdate1.Year = resing.Year Then
                                fired = pdate2.Subtract(resing).Days
                            Else
                                fired = 0
                            End If

                        End If
                        calc = nod - ca - clwp - newemp - fired

                        'Response.Write(calc.ToString)

                        cell(3) = Math.Round(calc, 2).ToString
                        'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                        cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString ' God does this
                        'cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                        ' Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)


                        cell(5) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and to_date is null and istaxable='y'", Session("con"))
                        If cell(5).ToString = "None" Then
                            cell(5) = "0"
                        ElseIf cell(5).ToString = "" Then
                            cell(5) = "0"
                        End If

                        If CDbl(cell(5)) > 0 Then
                            cell(5) = ((CDbl(cell(5)) / nod) * calc).ToString
                            ' Response.Write(cell(5))
                            'cell(5) = (CDbl(cell(5)) / nod) * (nod - fired)
                        End If
                        cell(6) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and to_date is null and istaxable='n'", Session("con"))
                        'Response.Write(cell(6) & "<br>")
                        If cell(6).ToString = "None" Then
                            cell(6) = "0"
                        ElseIf IsNumeric(cell(6)) = False Then
                            cell(6) = "0"
                        End If
                        If CDbl(cell(6)) > 0 Then
                            cell(6) = (CDbl(cell(6)) / nod) * calc
                            ' cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                        End If
                        cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                        ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                        '   Response.Write(cell(7).ToString & "<br>")
                        cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                        'Response.Write(cell(8).ToString & "<br>")
                        cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                        '  cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
                        cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString
                        ' Response.Write(cell(10) & "<br>")
                        If paid = "None" Then
                            If projid <> "" Then
                                'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                'Response.Write(sql)
                                sql = ""
                                rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) order by id", Session("con"))
                            Else
                                rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            End If
                            If rs2.HasRows Then
                                j = 0

                                While rs2.Read
                                    damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))

                                    If damt < CDbl(rs2.Item("bal")) And j < ccol Then
                                        If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                            cellb(j) = "0.00"
                                            cellbval(j) = "0"
                                            sumloan(j) += CDbl(cellb(j))
                                            j += 1
                                        End If
                                        cellb(j) = damt.ToString
                                        cellbval(j) = rs2.Item("id").ToString
                                        sumloan(j) += CDbl(cellb(j))

                                        j = j + 1

                                    Else
                                        If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                            cellb(j) = "0.00"
                                            cellbval(j) = "0"
                                            sumloan(j) += CDbl(cellb(j))
                                            j += 1
                                        End If
                                        cellbval(j) = rs2.Item("id").ToString
                                        cellb(j) = rs2.Item("bal").ToString
                                        sumloan(j) += CDbl(cellb(j))
                                        j = j + 1

                                    End If
                                End While
                                If j < ccol Then
                                    For p As Integer = j To ccol
                                        cellbval(p) = "0"
                                        cellb(p) = "0.00"
                                        sumloan(p) += CDbl(cellb(p))
                                    Next
                                End If
                            Else
                                For j = 0 To cellb.Length - 1
                                    cellbval(j) = "0"
                                    cellb(j) = "0.00"
                                    sumloan(j) += CDbl(cellb(j))
                                Next
                            End If
                            rs2.Close()

                        Else
                            rs2 = dbs.dtmake("loan", "select id,no_month_settle as nopay,amt,reason from emp_loan_req where deduction_starts < ='" & pdate1 & "' and emptid=" & emptid.ToString & " and id in(select loan_no from emp_loan_settlement where ref=" & paid.ToString & ")", Session("con"))
                            If projid <> "" Then
                                ' sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"
                                ' Response.Write(sql)
                                ' sql = ""
                                '  rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) order by id", Session("con"))
                            Else
                                'rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            End If
                            If rs2.HasRows Then
                                j = 0

                                While rs2.Read
                                    damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))

                                    If j < ccol Then
                                        If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                            cellb(j) = "0.00"
                                            cellbval(j) = "0"
                                            sumloan(j) += CDbl(cellb(j))
                                            j += 1
                                        End If
                                        cellb(j) = damt.ToString
                                        cellbval(j) = rs2.Item("id").ToString
                                        sumloan(j) += CDbl(cellb(j))

                                        j = j + 1

                                    Else
                                        If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                            cellb(j) = "0.00"
                                            cellbval(j) = "0"
                                            sumloan(j) += CDbl(cellb(j))
                                            j += 1
                                        End If
                                        cellbval(j) = rs2.Item("id").ToString
                                        cellb(j) = rs2.Item("bal").ToString
                                        sumloan(j) += CDbl(cellb(j))
                                        j = j + 1

                                    End If
                                End While
                            Else
                                For j = 0 To cellb.Length - 1
                                    cellbval(j) = "0"
                                    cellb(j) = "0.00"
                                    sumloan(j) += CDbl(cellb(j))
                                Next
                            End If
                            rs2.Close()

                        End If
                        ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                        cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                        cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                        ' Response.Write(cell(3).ToString & "===")
                        If color <> "#ccaa99" And cell(3).ToString <> "0" Then
                            sumfsal += CDbl(csal(0))
                            sumssal += CDbl(csal(1))
                            sumbsal += CDbl(cell(2))
                            sumbearn += CDbl(cell(4))
                            sumtalw += CDbl(cell(5))
                            sumalw += CDbl(cell(6))
                            sumot += CDbl(cell(7))
                            sumgross += CDbl(cell(8))
                            sumtincome += CDbl(cell(9))
                            sumtax += CDbl(cell(10))
                            sumpemp += CDbl(cell(12))
                            sumpco += CDbl(cell(13))

                        End If
                        Dim tdd As Double = 0
                        For i As Integer = 0 To ccol - 1
                            If color <> "#ccaa99" Then
                                tdd += CDbl(cellb(i))
                            End If
                        Next
                        cell(14) = (CDbl(cell(12)) + tdd + CDbl(cell(10))).ToString
                        If CDbl(cell(14)) > 0 Then
                            sumtd += CDbl(cell(14))
                        End If
                        If cell(2).ToString <> "0" And cell(3) <> "0" Then
                            outp &= "<tr style='background-color:" & color & "'>" & _
"<td class='cell' id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

                            For i As Integer = 0 To 14
                                If (cell(3).ToString = "0" And IsNumeric(cell(i)) = True) Then

                                    cell(i) = 0
                                End If
                                If color = "#ccaa99" And IsNumeric(cell(i)) = True Then
                                    cell(i) = 0
                                End If
                                If i <> 11 And i <> 2 Then
                                    'Response.Write("isNUmeric: " & cell(i) & "=" & IsNumeric(cell(i)).ToString & "***")

                                    '   Response.Write(cell(i).ToString)
                                    If i = 0 Or i = 1 Or i = 3 Then
                                        outp &= ("<td class='cell' style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                    Else
                                        outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                    End If
                                ElseIf i = 2 Then
                                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-1'>" & fm.numdigit(csal(0).ToString, 2) & "<br>(" & incwhen.ToString & " days)</td>")

                                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-2'>" & fm.numdigit(csal(1).ToString, 2) & "<br>(" & (nod - incwhen).ToString & " days)</td>")

                                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                ElseIf i = 11 Then
                                    For j = 0 To ccol - 1
                                        If String.IsNullOrEmpty(cellbval(j)) = False Then
                                            outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                        Else
                                            outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
                                        End If
                                    Next
                                    If ccol = 0 Then
                                        outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                    End If
                                End If
                            Next
                            netincom = CDbl(cell(8)) - CDbl(cell(14))
                            If color <> "#ccaa99" Then
                                sumnet += netincom
                            End If
                            ' Response.Write(netincom.ToString & "<----<br>")
                            outp &= "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"
                            outp &= "<td class='cell' style='text-align:right;'>"
                            If paid.ToString = "None" And CDbl(cell(2)) <> 0 Then
                                outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked' onclick='javascript:sumcolx();'></td>"
                            Else
                                If CDbl(cell(2)) <> 0 Then
                                    outp &= " Paid</td>"

                                    '  outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' style='visibility:hidden;'>Paid</td>"
                                Else
                                    outp &= " None</td>"

                                    'outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' ></td>"

                                End If


                            End If
                            outp &= "<td class='cell' width='300px' style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                            k += 1
                        End If
                    End If
                End While


                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                "<td class='cell' style='border-bottom:1px black solid;border-top:1px solid black;'>&nbsp;</td><td class='cell'>..&nbsp;</td><td class='cell'>..&nbsp;</td>" & _
                "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bfsal'>" & fm.numdigit(sumfsal.ToString, 2).ToString & "</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bssal'>" & fm.numdigit(sumssal.ToString, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                 "<td class='cell' style='text-align:right;border-top:1px solid black;'>&nbsp;</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bearn'>" & fm.numdigit(sumbearn, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtalw'>" & fm.numdigit(sumtalw, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumalw'>" & fm.numdigit(sumalw.ToString, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumot'>" & fm.numdigit(sumot, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumgross'>" & fm.numdigit(sumgross, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumti'>" & fm.numdigit(sumtincome, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumt'>" & fm.numdigit(sumtax, 2).ToString & "</td>"


                For j = 0 To ccol - 1
                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='lon-" & j & "'>" & fm.numdigit(sumloan(j).ToString, 2) & "</td>")
                Next
                If ccol = 0 Then
                    outp &= ("<td class='cell' style='text-align:right;'>&nbsp;</td>")
                End If
                outp &= "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
                "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                  "<td class='cell' colspan='2'>..&nbsp;" & _
     "</td></tr><tr id='result'></tr>"
                outp &= "</table>"
                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
            End If
            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
            xprint = "<script language='javascript' type='text/javascript'>//sumcolx(); " & _
       "$(function() {$( '#payd').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	}); $( '#payd' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
                "  <input type='button' id='post' onclick='javascript:findid2()' name='post' value='Make Statment' />"



            Response.Write(xprint)
            Return outp



        End If
    End Function
    Public Function makeform2(ByVal vx() As String)
        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String
        Dim csal(2) As Object
        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim bsal, dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
        Dim sumbsal, sumbearn, sumtalw, sumfsal, sumssal, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double
        sumbsal = 0
        sumbearn = 0
        sumtalw = 0
        sumalw = 0
        sumot = 0
        sumgross = 0
        sumtincome = 0
        sumtax = 0
        sumpemp = 0
        sumpco = 0
        sumtd = 0
        sumnet = 0
        Dim sal As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        ' Response.Write(Request.QueryString("month"))
        For Each kkk As String In vx
            ' Response.Write("<tr><td>" & kkk & "</td></tr>")
        Next
        If Request.QueryString("month") <> "" Then
            Dim spl() As String
            Dim projid As String = ""
            If Request.QueryString("projname") <> "" Then
                spl = Request.QueryString("projname").Split("|")
                If spl.Length > 1 Then
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                Else
                    projid = ""
                End If
            End If

            nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))

            pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
            pdate2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
            Dim rs As DataTableReader
            Dim rs2 As DataTableReader
            Dim ccol As Integer = 0
            Dim paid As String
            Dim j As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If
            paid = ""
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx (ref_inc is null or ref_inc='0'))", Session("con"))
            If paid = "None" Then
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vwloanbal WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            Else
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vwloanbal WHERE bal>0 and ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            End If
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            strc = (fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", Session("con")))
            If strc.ToString.Length > 3 Then
                pemp = 0
            Else
                pemp = CDbl(strc)
            End If
            strc = "0"
            strc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", Session("con"))
            If strc.Length > 3 Then
                pco = "0"
            Else
                pco = CDbl(strc)
            End If
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name", Session("con"))
            Else
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
            End If

            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3' style='width:1300px;font-size:9pt;' border='0'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='16' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td rowspan='2'>No.</td><td style='width:90px;' rowspan='2'>Emp. Id</td>"
                outp &= "<td style='width:250px;' rowspan='2'>Full Name</td>"
                outp &= "<td colspan='3'>Basic Salary</td>" & Chr(13)
                outp &= "<td  rowspan='2'>Days Worked</td>"
                outp &= "<td  rowspan='2'>Basic Earning</td>"
                outp &= "<td  rowspan='2'>Taxable Allowance</td>"
                outp &= "<td   rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td  rowspan='2'> Overtime</td>"
                outp &= "<td  rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  rowspan='2'>Taxable Incom</td>"
                outp &= "<td  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td rowspan='2'>Net Pay</td>"

                outp &= "<td rowspan='2'>Signature</td></tr>" & Chr(13)
                If paid = "None" Then
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

                Else
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where  dstart < ='" & pdate1 & "'", Session("con"))

                End If
                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr><td>Prev.<br>Salary </td><td>New Salary</td><td>Salary</td>" & Chr(13)
                If ccol = 0 Then
                    outp &= "<td ></td>"
                Else
                    If rs2.HasRows Then
                        Dim i As Integer = 0
                        While rs2.Read

                            If rs2.Item("reason") = "-" Then
                                outp &= "<td >Other</td>"
                            Else
                                outp &= "<td >" & rs2.Item("reason").ToString & "</td>"
                            End If
                            reasonname(i) = rs2.Item("reason")
                            i += 1
                        End While
                    End If
                End If

                outp &= "</tr>" & Chr(13)
                rs2.Close()
                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date

                While rs.Read
                    'check id in the list2 and make statement again 
                    emptid = rs.Item("id")
                    ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                    If fm.searcharray(vx, emptid.ToString) Then
                        ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                        resing = "#1/1/1900#"
                        If color <> "#aabbcc" Then
                            color = "#aabbcc"
                        Else
                            color = "white"
                        End If


                        If (rs.Item("active") = "n") Then

                            resing = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))
                        End If
                        Dim dateincr As Date
                        Dim incwhen As Integer = 0
                        avalue = ""
                        avalue = fm.getinfo2("select date_start from emp_sal_info where emptid=" & emptid & " order by id desc", Session("con"))
                        If avalue <> "None" Then
                            dateincr = avalue
                        End If
                        If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year Then
                            'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                            incwhen = dateincr.Subtract(pdate1).Days
                        End If
                        If incwhen > 0 Then 'question..........no question it is solved
                            ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            ' Response.Write(sql & "<br>")
                            'cell(2) = fm.getinfo2(sql, Session("con")).ToString
                            csal(0) = fm.getinfo2(sql, Session("con")).ToString

                            If csal(0) = "None" Then
                                csal(0) = "0"
                            End If
                            sql = "select basic_salary from emp_sal_info where date_end is null and emptid=" & rs.Item("id").ToString
                            csal(1) = fm.getinfo2(sql, Session("con")).ToString
                            If csal(1).ToString = "None" Then
                                csal(1) = "0"
                            End If
                            csal(0) = (CDbl(csal(0)) / nod) * incwhen
                            csal(1) = (CDbl(csal(1)) / nod) * (nod - incwhen)


                            Dim dblv As Double = CDbl(csal(0)) + CDbl(csal(1))
                            cell(2) = dblv.ToString
                            'Response.Write(csal(0))
                            'cell(2) = "0"
                            If cell(2) = "None" Then
                                cell(2) = "0"
                                '  color = "#ccaa99"
                            End If
                            paid = ""
                            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select payroll_id from paryrol where emptid='" & emptid & "')", Session("con"))
                            'Response.Write(paid)
                            ca = fm.catt(emptid, Session("con"), pdate1, pdate2)
                            'Response.Write(ca.ToString & "<==a")
                            clwp = Math.Round(CDbl(fm.lwpinmonth(pdate1, pdate2, emptid, Session("con"))), 2)
                            ' Response.Write("lwp==>" & clwp.ToString & "<br>")
                            newemp = 0
                            If pdate1 < rs.Item("hire_date") Then
                                newemp = nod - (pdate2.Subtract(rs.Item("hire_date")).days) - 1
                            End If
                            Dim fired As Double = 0


                            'nod = Today.Day
                            If resing <> "#1/1/1900#" Then
                                If pdate1.Month = resing.Month And pdate1.Year = resing.Year Then
                                    fired = pdate2.Subtract(resing).Days
                                Else
                                    fired = 0
                                End If

                            End If
                            calc = nod - ca - clwp - newemp - fired

                            cell(3) = Math.Round(calc, 2).ToString
                            ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                            cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                            'Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)

                            cell(5) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and to_date is null and istaxable='y'", Session("con"))
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            End If

                            If CDbl(cell(5)) > 0 Then
                                cell(5) = (CDbl(cell(5)) / nod) * calc
                            End If
                            cell(6) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and to_date is null and istaxable='n'", Session("con"))
                            'Response.Write(cell(6) & "<br>")
                            If cell(6).ToString = "None" Then
                                cell(6) = "0"
                            ElseIf IsNumeric(cell(6)) = False Then
                                cell(6) = "0"
                            End If
                            If CDbl(cell(6)) > 0 Then
                                cell(6) = (CDbl(cell(6)) / nod) * calc
                            End If
                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            ' cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
                            cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString
                            If paid = "None" Then
                                If projid <> "" Then
                                    'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                    'Response.Write(sql)
                                    sql = ""
                                    rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) order by id", Session("con"))
                                Else
                                    rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                End If
                                If rs2.HasRows Then
                                    j = 0

                                    While rs2.Read
                                        damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))

                                        If damt < CDbl(rs2.Item("bal")) And j < ccol Then
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellb(j) = damt.ToString
                                            cellbval(j) = rs2.Item("id").ToString
                                            sumloan(j) += CDbl(cellb(j))

                                            j = j + 1

                                        Else
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellbval(j) = rs2.Item("id").ToString
                                            cellb(j) = rs2.Item("bal").ToString
                                            sumloan(j) += CDbl(cellb(j))
                                            j = j + 1

                                        End If
                                    End While
                                    If j < ccol Then
                                        For p As Integer = j To ccol
                                            cellbval(p) = "0"
                                            cellb(p) = "0.00"
                                            sumloan(p) += CDbl(cellb(p))
                                        Next
                                    End If
                                Else
                                    For j = 0 To cellb.Length - 1
                                        cellbval(j) = "0"
                                        cellb(j) = "0.00"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next
                                End If
                                rs2.Close()

                            Else
                                rs2 = dbs.dtmake("loan", "select id,no_month_settle as nopay,amt,reason from emp_loan_req where deduction_starts < ='" & pdate1 & "' and emptid=" & emptid.ToString & " and id in(select loan_no from emp_loan_settlement where ref=" & paid.ToString & ")", Session("con"))
                                If projid <> "" Then
                                    ' sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"
                                    ' Response.Write(sql)
                                    ' sql = ""
                                    '  rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) order by id", Session("con"))
                                Else
                                    'rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                End If
                                If rs2.HasRows Then
                                    j = 0

                                    While rs2.Read
                                        damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))

                                        If j < ccol Then
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellb(j) = damt.ToString
                                            cellbval(j) = rs2.Item("id").ToString
                                            sumloan(j) += CDbl(cellb(j))

                                            j = j + 1

                                        Else
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellbval(j) = rs2.Item("id").ToString
                                            cellb(j) = rs2.Item("bal").ToString
                                            sumloan(j) += CDbl(cellb(j))
                                            j = j + 1

                                        End If
                                    End While
                                Else
                                    For j = 0 To cellb.Length - 1
                                        cellbval(j) = "0"
                                        cellb(j) = "0.00"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next
                                End If
                                rs2.Close()

                            End If
                            ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                            cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                            ' Response.Write(cell(3).ToString & "===")
                            If color <> "#ccaa99" And cell(3).ToString <> "0" Then
                                sumfsal += CDbl(csal(0))
                                sumssal += CDbl(csal(1))
                                sumbsal += CDbl(cell(2))
                                sumbearn += CDbl(cell(4))
                                sumtalw += CDbl(cell(5))
                                sumalw += CDbl(cell(6))
                                sumot += CDbl(cell(7))
                                sumgross += CDbl(cell(8))
                                sumtincome += CDbl(cell(9))
                                sumtax += CDbl(cell(10))
                                sumpemp += CDbl(cell(12))
                                sumpco += CDbl(cell(13))

                            End If
                            Dim tdd As Double = 0
                            For i As Integer = 0 To ccol - 1
                                If color <> "#ccaa99" Then
                                    tdd += CDbl(cellb(i))
                                End If
                            Next
                            cell(14) = (CDbl(cell(12)) + tdd + CDbl(cell(10))).ToString
                            If CDbl(cell(14)) > 0 Then
                                sumtd += CDbl(cell(14))
                            End If

                            If cell(2).ToString <> "0" And cell(3) <> "0" Then
                                'Response.Write(emptid & "<br>")
                                outp &= "<tr style='background-color:" & color & "'><td class='cell' id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

                                For i As Integer = 0 To 14
                                    If (cell(3).ToString = "0" And IsNumeric(cell(i)) = True) Then

                                        cell(i) = 0
                                    End If
                                    If color = "#ccaa99" And IsNumeric(cell(i)) = True Then
                                        cell(i) = 0
                                    End If
                                    If i <> 11 And i <> 2 Then
                                        'Response.Write("isNUmeric: " & cell(i) & "=" & IsNumeric(cell(i)).ToString & "***")

                                        '   Response.Write(cell(i).ToString)
                                        If i = 0 Or i = 1 Or i = 3 Then
                                            outp &= ("<td class='cell' style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                        Else
                                            outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                        End If
                                    ElseIf i = 2 Then
                                        outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-1'>" & fm.numdigit(csal(0).ToString, 2) & "<br>(" & incwhen.ToString & " days)</td>")

                                        outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-2'>" & fm.numdigit(csal(1).ToString, 2) & "<br>(" & (nod - incwhen).ToString & " days)</td>")

                                        outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                    ElseIf i = 11 Then
                                        For j = 0 To ccol - 1
                                            If String.IsNullOrEmpty(cellbval(j)) = False Then
                                                outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>&nbsp;" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                            Else
                                                outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>&nbsp;0.00</td>")
                                            End If
                                        Next
                                        If ccol = 0 Then
                                            outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                        End If
                                    End If
                                Next
                                netincom = CDbl(cell(8)) - CDbl(cell(14))
                                If color <> "#ccaa99" Then
                                    sumnet += netincom
                                End If
                                ' Response.Write(netincom.ToString & "<----<br>")
                                outp &= "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"

                                outp &= "<td class='cell' width='300px' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If ' end of selected array
                End While


                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                "<td class='cell' style='border-bottom:1px black solid;border-top:1px solid black;'>&nbsp;</td><td class='cell'>..&nbsp;</td><td class='cell'>..&nbsp;</td>" & _
                "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bfsal'>" & fm.numdigit(sumfsal.ToString, 2).ToString & "</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bssal'>" & fm.numdigit(sumssal.ToString, 2).ToString & "</td>" & _
 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00border-top:1px solid black;'>&nbsp;</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bearn'>" & fm.numdigit(sumbearn, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtalw'>" & fm.numdigit(sumtalw, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumalw'>" & fm.numdigit(sumalw.ToString, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumot'>" & fm.numdigit(sumot, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumgross'>" & fm.numdigit(sumgross, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumti'>" & fm.numdigit(sumtincome, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumt'>" & fm.numdigit(sumtax, 2).ToString & "</td>"


                For j = 0 To ccol - 1
                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='lon-" & j & "'>" & fm.numdigit(sumloan(j).ToString, 2) & "</td>")
                Next
                If ccol = 0 Then
                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td>")
                End If
                outp &= "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & Chr(13) & _
                "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & Chr(13) & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & Chr(13) & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & Chr(13) & _
                  "<td class='cell' colspan='2'>..&nbsp;" & _
     "</td></tr><tr id='result'></tr>"
                outp &= "</table>"
                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
            End If
            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
            xprint &= "<input type='text' name='paydx' id='paydx' value='" & Today.ToShortDateString & "' />" & Chr(13) & _
       "<script language='javascript' type='text/javascript'>//sumcolx(); " & Chr(13) & _
       "$(function() {$( '#paydx').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	});" & Chr(13) & " $( '#paydx' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                "  <input type='button' id='post' onclick='javascript:findid()' name='post' value='Paid' />"

            Response.Write(xprint)
            Return outp



        End If
    End Function
    Public Function getids()
        Dim idarr() As String = {"none"}
        Dim spl() As String
        Dim con As String = ""
        Dim i As Integer = 0
        Dim arrkey() As String = Request.Form("nextpage").Split("&")
        For i = 0 To arrkey.Length - 1
            'Response.Write(arrkey(i) & "<br>")
            spl = arrkey(i).Split("=")(0).Split("_")
            If con <> spl(0).Trim Then
                If spl.Length > 1 Then
                    ReDim Preserve idarr(i + 1)
                    idarr(i) = spl(0).ToString
                    con = spl(0)
                    i += 1
                End If
            End If
        Next

        Return idarr
    End Function
    Public Function makeform3(ByVal vx() As String)
        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double

        Dim bsal, dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
        Dim sumbsal, sumbearn, sumtalw, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double
        sumbsal = 0
        sumbearn = 0
        sumtalw = 0
        sumalw = 0
        sumot = 0
        sumgross = 0
        sumtincome = 0
        sumtax = 0
        sumpemp = 0
        sumpco = 0
        sumtd = 0
        sumnet = 0
        Dim sal As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        If Request.Form("month") <> "" Then
            Dim spl() As String
            Dim projid As String = ""
            If Request.Form("projname") <> "" Then
                spl = Request.Form("projname").Split("|")

                If spl.Length > 1 Then
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                Else
                    projid = ""
                End If
            End If
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            Dim rs As DataTableReader
            Dim rs2 As DataTableReader
            Dim ccol As Integer = 0
            Dim paid As String
            Dim j As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If
            paid = ""
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select payroll_id from paryrol ref_inc is null)", Session("con"))
            If paid = "None" Then
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            Else
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            End If
            ' Response.Write(ccol)
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            strc = (fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", Session("con")))
            If strc.ToString.Length > 3 Then
                pemp = 0
            Else
                pemp = CDbl(strc)
            End If
            strc = "0"
            strc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", Session("con"))
            If strc.Length > 3 Then
                pco = "0"
            Else
                pco = CDbl(strc)
            End If
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ", Session("con"))
            End If

            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3' style='width:1300px;font-size:9pt;' border='0'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='16' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td rowspan='2'>No.</td><td style='width:90px;' rowspan='2'>Emp. Id</td>"
                outp &= "<td style='width:250px;' rowspan='2'>Full Name</td>"
                outp &= "<td  rowspan='2'>Basic Salary</td>" & Chr(13)
                outp &= "<td  rowspan='2'>Days Worked</td>"
                outp &= "<td  rowspan='2'>Basic Earning</td>"
                outp &= "<td  rowspan='2'>Taxable Allowance</td>"
                outp &= "<td   rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td  rowspan='2'> Overtime</td>"
                outp &= "<td  rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  rowspan='2'>Taxable Incom</td>"
                outp &= "<td  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td rowspan='2'>Net Pay</td>"
                outp &= "<td  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                outp &= "<td rowspan='2'>Signature</td></tr>" & Chr(13)
                If paid = "None" Then
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

                Else
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where  dstart < ='" & pdate1 & "'", Session("con"))

                End If
                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)
                If ccol = 0 Then
                    outp &= "<td ></td>"
                Else
                    If rs2.HasRows Then
                        Dim i As Integer = 0
                        While rs2.Read

                            If rs2.Item("reason") = "-" Then
                                outp &= "<td >Other</td>"
                            Else
                                outp &= "<td >" & rs2.Item("reason").ToString & "</td>"
                            End If
                            reasonname(i) = rs2.Item("reason")
                            i += 1
                        End While
                    End If
                End If


                outp &= "</tr>" & Chr(13)
                rs2.Close()
                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date

                While rs.Read
                    emptid = rs.Item("id")
                    If fm.searcharray(vx, emptid.ToString) Then
                        resing = "#1/1/1900#"
                        If color <> "#aabbcc" Then
                            color = "#aabbcc"
                        Else
                            color = "white"
                        End If


                        If (rs.Item("active") = "n") Then

                            resing = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))
                        End If
                        If (resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing Then 'question..........

                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            ' Response.Write(sql & "<br>")
                            cell(2) = fm.getinfo2(sql, Session("con")).ToString
                            If cell(2) = "None" Then
                                cell(2) = "0"
                                '  color = "#ccaa99"
                            End If
                            paid = ""
                            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select payroll_id from paryrol where emptid='" & emptid & "')", Session("con"))
                            'Response.Write(paid)
                            ca = fm.catt(emptid, Session("con"), pdate1, pdate2)
                            'Response.Write(ca.ToString & "<==a")
                            clwp = Math.Round(CDbl(fm.lwpinmonth(pdate1, pdate2, emptid, Session("con"))), 2)
                            ' Response.Write("lwp==>" & clwp.ToString & "<br>")
                            newemp = 0
                            If pdate1 < rs.Item("hire_date") Then
                                newemp = nod - (pdate2.Subtract(rs.Item("hire_date")).days) + 1
                            End If
                            Dim fired As Double = 0
                            If resing <> "#1/1/1900#" Then
                                If pdate1.Month = resing.Month And pdate1.Year = resing.Year Then
                                    fired = pdate2.Subtract(resing).Days
                                Else
                                    fired = 0
                                End If

                            End If
                            calc = nod - ca - clwp - newemp - fired
                            cell(3) = Math.Round(calc, 2).ToString

                            cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString

                            cell(5) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and to_date is null and istaxable='y'", Session("con"))
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            End If

                            If CDbl(cell(5)) > 0 And fired > 0 And nod > fired Then
                                cell(5) = (CDbl(cell(5)) / nod) * (nod - fired)
                            End If
                            cell(6) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and to_date is null and istaxable='n'", Session("con"))
                            'Response.Write(cell(6) & "<br>")
                            If cell(6).ToString = "None" Then
                                cell(6) = "0"
                            ElseIf IsNumeric(cell(6)) = False Then
                                cell(6) = "0"
                            End If
                            If CDbl(cell(6)) > 0 And fired > 0 And nod > fired Then
                                cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                            End If
                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            '  cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
                            cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString
                            ' Response.Write(cell(10) & "<br>")
                            If paid = "None" Then
                                If projid <> "" Then
                                    'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                    'Response.Write(sql)
                                    sql = ""
                                    rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) order by id", Session("con"))
                                Else
                                    rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                End If
                                If rs2.HasRows Then
                                    j = 0

                                    While rs2.Read
                                        damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))

                                        If damt < CDbl(rs2.Item("bal")) And j < ccol Then
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellb(j) = damt.ToString
                                            cellbval(j) = rs2.Item("id").ToString
                                            sumloan(j) += CDbl(cellb(j))

                                            j = j + 1

                                        Else
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellbval(j) = rs2.Item("id").ToString
                                            cellb(j) = rs2.Item("bal").ToString
                                            sumloan(j) += CDbl(cellb(j))
                                            j = j + 1

                                        End If
                                    End While
                                    If j < ccol Then
                                        For p As Integer = j To ccol
                                            cellbval(p) = "0"
                                            cellb(p) = "0.00"
                                            sumloan(p) += CDbl(cellb(p))
                                        Next
                                    End If
                                Else
                                    For j = 0 To cellb.Length - 1
                                        cellbval(j) = "0"
                                        cellb(j) = "0.00"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next
                                End If
                                rs2.Close()

                            Else
                                rs2 = dbs.dtmake("loan", "select id,no_month_settle as nopay,amt,reason from emp_loan_req where deduction_starts < ='" & pdate1 & "' and emptid=" & emptid.ToString & " and id in(select loan_no from emp_loan_settlement where ref=" & paid.ToString & ")", Session("con"))
                                If projid <> "" Then
                                    ' sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"
                                    ' Response.Write(sql)
                                    ' sql = ""
                                    '  rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) order by id", Session("con"))
                                Else
                                    'rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                End If
                                If rs2.HasRows Then
                                    j = 0

                                    While rs2.Read
                                        damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))

                                        If j < ccol Then
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellb(j) = damt.ToString
                                            cellbval(j) = rs2.Item("id").ToString
                                            sumloan(j) += CDbl(cellb(j))

                                            j = j + 1

                                        Else
                                            If reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                j += 1
                                            End If
                                            cellbval(j) = rs2.Item("id").ToString
                                            cellb(j) = rs2.Item("bal").ToString
                                            sumloan(j) += CDbl(cellb(j))
                                            j = j + 1

                                        End If
                                    End While
                                Else
                                    For j = 0 To cellb.Length - 1
                                        cellbval(j) = "0"
                                        cellb(j) = "0.00"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next
                                End If
                                rs2.Close()

                            End If
                            ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            cell(12) = Math.Round((fm.pension(CDbl(cell(2)), CDbl(pemp) / 100)), 2).ToString
                            cell(13) = Math.Round((fm.pension(CDbl(cell(2)), CDbl(pco) / 100)), 2).ToString
                            ' Response.Write(cell(3).ToString & "===")
                            If color <> "#ccaa99" And cell(3).ToString <> "0" Then
                                sumbsal += CDbl(cell(2))
                                sumbearn += CDbl(cell(4))
                                sumtalw += CDbl(cell(5))
                                sumalw += CDbl(cell(6))
                                sumot += CDbl(cell(7))
                                sumgross += CDbl(cell(8))
                                sumtincome += CDbl(cell(9))
                                sumtax += CDbl(cell(10))
                                sumpemp += CDbl(cell(12))
                                sumpco += CDbl(cell(13))

                            End If
                            Dim tdd As Double = 0
                            For i As Integer = 0 To ccol - 1
                                If color <> "#ccaa99" Then
                                    tdd += CDbl(cellb(i))
                                End If
                            Next
                            cell(14) = (CDbl(cell(12)) + tdd + CDbl(cell(10))).ToString
                            If CDbl(cell(14)) > 0 Then
                                sumtd += CDbl(cell(14))
                            End If
                            If cell(2).ToString <> "0" And cell(3) <> "0" Then
                                outp &= "<tr style='background-color:" & color & "'><td class='cell' id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

                                For i As Integer = 0 To 14
                                    If (cell(3).ToString = "0" And IsNumeric(cell(i)) = True) Then

                                        cell(i) = 0
                                    End If
                                    If color = "#ccaa99" And IsNumeric(cell(i)) = True Then
                                        cell(i) = 0
                                    End If
                                    If i <> 11 Then
                                        'Response.Write("isNUmeric: " & cell(i) & "=" & IsNumeric(cell(i)).ToString & "***")

                                        '   Response.Write(cell(i).ToString)
                                        If i = 0 Or i = 1 Or i = 3 Then
                                            outp &= ("<td class='cell' style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                        Else
                                            outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                        End If
                                    Else
                                        For j = 0 To ccol - 1
                                            If String.IsNullOrEmpty(cellbval(j)) = False Then
                                                outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>&nbsp;" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                            Else
                                                outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>&nbsp;0.00</td>")
                                            End If
                                        Next
                                        If ccol = 0 Then
                                            outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                        End If
                                    End If
                                Next
                                netincom = CDbl(cell(8)) - CDbl(cell(14))
                                If color <> "#ccaa99" Then
                                    sumnet += netincom
                                End If
                                ' Response.Write(netincom.ToString & "<----<br>")
                                outp &= "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"
                                outp &= "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>"
                                If paid.ToString = "None" And CDbl(cell(2)) <> 0 Then
                                    outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked' onclick='javascript:sumcolx();'></td>"
                                Else
                                    If CDbl(cell(2)) <> 0 Then
                                        outp &= " Paid</td>"

                                        '  outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' style='visibility:hidden;'>Paid</td>"
                                    Else
                                        outp &= " None</td>"

                                        'outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' ></td>"

                                    End If


                                End If
                                outp &= "<td class='cell' width='300px' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If
                End While


                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                "<td class='cell' style='border-bottom:1px black solid;border-top:1px solid black;'>&nbsp;</td><td class='cell'>..&nbsp;</td><td class='cell'>..&nbsp;</td>" & _
                "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00border-top:1px solid black;'>&nbsp;</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bearn'>" & fm.numdigit(sumbearn, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtalw'>" & fm.numdigit(sumtalw, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumalw'>" & fm.numdigit(sumalw.ToString, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumot'>" & fm.numdigit(sumot, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumgross'>" & fm.numdigit(sumgross, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumti'>" & fm.numdigit(sumtincome, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumt'>" & fm.numdigit(sumtax, 2).ToString & "</td>"


                For j = 0 To ccol - 1
                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='lon-" & j & "'>" & fm.numdigit(sumloan(j).ToString, 2) & "</td>")
                Next
                If ccol = 0 Then
                    outp &= ("<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td>")
                End If
                outp &= "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
                "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
                 "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
                  "<td class='cell' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                  "<td class='cell' colspan='2'>..&nbsp;" & _
     "</td></tr><tr id='result'></tr>"
                outp &= "</table>"
                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
                Return outp
            End If
            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
            xprint &= "<input type='text' name='paydx' id='paydx' value='" & Today.ToShortDateString & "' />" & Chr(13) & _
           "<script language='javascript' type='text/javascript'>//sumcolx(); " & Chr(13) & _
           "$(function() {$( '#paydx').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	});" & Chr(13) & " $( '#paydx' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                    "  <input type='button' id='post' onclick='javascript:findid()' name='post' value='Paid' />"

            Response.Write(xprint)


            Return outp

        End If
    End Function

End Class
