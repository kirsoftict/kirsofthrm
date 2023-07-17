Imports System.Data
Imports Kirsoft.hrm

Partial Class reportpool
    Inherits System.Web.UI.Page
    Public Function rptview_payroll() 'View
        'view paid employees

        Session.Timeout = 60
        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String
        Dim rrr As DataTableReader
        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim pemp, pco, netincom As Double
        Dim ref As String

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
        Dim sec As New k_security
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        'For Each k As String In Request.ServerVariables
        'Response.Write(k & "=" & Request.ServerVariables(k) & "<br>")
        '  Next
        Try


            If Request.QueryString("prid") <> "" Then
                'Response.Write(fl.msgboxt("onfram", "Progression", "Progression shown"))
                'Response.Write("<script>showobj('progressbar');</script>")
                ref = Request.QueryString("prid")
                ' Response.Write(ref)
                Dim spl() As String
                nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                pdate2 = Request.QueryString("pd")
                pdate1 = pdate2.Month & "/1/" & pdate2.Year
                Dim rs As DataTableReader
                Dim rs2 As DataTableReader
                Dim ccol As Integer = 0
                Dim paid As String
                Dim j As Integer
                Dim ccol2 As Integer
                Dim reasonname() As String
                Dim paypaid As Integer = 0
                ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
                If paypaid <> 0 Then
                    ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

                End If
                paid = ""
                paid = fm.getinfo2("select ref from payrollx where ref='" & ref & "'", Session("con"))
                ccol = 0
                If paid <> "None" Then
                    ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT distinct reason AS cnt FROM vwloanref WHERE  ref='" & ref & "')  AS derivedtbl_1", Session("con")))
                    ' ' Response.Write("SELECT COUNT(cnt) AS Expr1 froM (SELECT distinct reason AS cnt FROM vwloanref WHERE  ref='" & ref & "')  AS derivedtbl_1")

                End If

                'Response.Write(ccol)
                'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ReDim cellb(ccol)
                ReDim cellbval(ccol)
                ReDim sumloan(ccol)
                ReDim reasonname(ccol)
                Dim stylew(4) As String
                Dim tcell As Integer
                Dim wdthpay As Integer = 1400
                tcell = ccol + 13
                If Request.QueryString("widthpay") <> "" Then
                    wdthpay = Request.QueryString("widthpay")
                End If
                Dim ratiow As Double
                ratiow = (wdthpay - 350 - 30 - 60) / tcell
                stylew(0) = "30px"
                stylew(1) = "60px"
                stylew(2) = "350px"
                stylew(3) = Math.Round(ratiow, 0).ToString & "px"

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
                If ref <> "" Then
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from payrollx where ref='" & ref.ToString & "') ORDER BY emp_static_info.first_name", Session("con"))
                Else
                    rs = Nothing
                End If
                outp = "Sorry Can't Process"
                If rs.HasRows Then
                    outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                    outp &= "<thead>"
                    outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                    outp &= "<td class='toptitle' style='text-align:center;font-weight:bold;' colspan='" & (19 + ccol).ToString & "' >" & Session("company_name") & _
                    "<br> Project Name:"
                    'rs.Read()
                    ''Session("chgref") = (Request.ServerVariables("HTTP_REFERER")) & "?paidst=paid&" & (Request.ServerVariables("QUERY_STRING"))

                    ' Response.Write(Session("chgref"))
                    'Response.Write(Request.QueryString("projname"))
                    outp &= (sec.dbHexToStr(Request.QueryString("projname")))
                    outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & _
                    "<br>" & ref & "</td>" & Chr(13)

                    outp &= "</tr>" & Chr(13)

                    outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                    outp &= "<td class='dw' rowspan='2' >No.</td><td class='dw' rowspan='2'>Emp. Id</td>"
                    outp &= "<td class='fxname'  rowspan='2'>Full Name</td>"
                    outp &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                    outp &= "<td class='dw' rowspan='2'>Days Worked</td>"
                    outp &= "<td class='fitx' rowspan='2'>Basic Earning</td>"
                    outp &= "<td class='fitx' rowspan='2'>Taxable Allowance</td>"
                    outp &= "<td class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                    outp &= "<td class='fitx' rowspan='2'> Overtime</td>"
                    outp &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                    outp &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                    outp &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                    outp &= "<td class='dedct' style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                    outp &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                    outp &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                    outp &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                    outp &= "<td class='fitx' rowspan='2'>Net Pay</td>"


                    outp &= "<td class='dw' id='chkall' style='cursor:pointer;width:30px;' rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                    outp &= "<td rowspan='2' class='signpart'>Signature</td></tr>" & Chr(13)

                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanref where ref='" & ref & "' order by reason", Session("con"))


                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                    outp &= "<tr>" & Chr(13)
                    If ccol = 0 Then
                        outp &= "<td class='dedctx' >&nbsp;</td>"
                    Else
                        If rs2.HasRows Then
                            Dim i As Integer = 0
                            While rs2.Read

                                If rs2.Item("reason") = "-" Then
                                    outp &= "<td class='dedctx'>Other</td>"
                                Else
                                    outp &= "<td  class='dedctx'>" & rs2.Item("reason").ToString & "</td>"
                                End If
                                reasonname(i) = rs2.Item("reason")
                                i += 1
                            End While
                        End If
                    End If


                    outp &= "</tr> </thead> <tbody>" & Chr(13)
                    rs2.Close()
                    Dim k As Integer = 1
                    Dim color As String = ""
                    Dim resing As Date
                    Dim did As String
                    Dim loanid As String = ""
                    Dim otid As String = ""

                    While rs.Read
                        ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))

                        emptid = rs.Item("id")
                        paid = ""
                        paid = fm.getinfo2("select id from payrollx where ref='" & ref & "'", Session("con"))
                        '  Response.Write(paid.ToString & "<br>...")
                        If paid.ToString <> "None" Then
                            ' paidlist &= fm.getinfo2("select id from paryrollx where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "') and (ref_inc='0' or ref_inc IS NULL)", Session("con")) & ","
                            '  Response.Write(paidlist & "<br>")
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                            End If

                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            ' sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            'Response.Write(sql & "<br>")
                            sal = fm.getinfo2("select b_sal from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            cell(2) = sal
                            If cell(2) = "Sorry This employee salary info is not setted!" Then
                                cell(2) = "0"
                                '  color = "#ccaa99"
                            End If
                            'Response.Write(paid)

                            'Response.Write(calc.ToString)

                            cell(3) = fm.getinfo2("select no_day from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                            cell(4) = fm.getinfo2("select b_e from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            cell(5) = fm.getinfo2("select talw from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            '  Response.Write(cell(5).ToString)

                            cell(6) = fm.getinfo2("select alw from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            cell(7) = fm.getinfo2("select ot from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))

                            'otid &= fm.getinfo2("select id from emp_ot where emptid=" & emptid & " and ot_date='" & pdate2 & "' and paidstatus='y'", Session("con")) & ","

                            cell(8) = fm.getinfo2("select gross_earnings from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = fm.getinfo2("select txinco from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            cell(10) = fm.getinfo2("select tax from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            ' Response.Write(cell(10) & "<br>")
                            rs2.Close()
                            j = 0
                            Dim loaninf As String
                            loaninf = fm.getinfo2("select ref from emp_loan_settlement where ref='" & ref & "'", Session("con"))

                            If loaninf <> "None" Then
                                rs2 = dbs.dtmake("loan", "select id,loan_no,reason from vwloanref where ref='" & ref & "' and emptid=" & emptid & " order by reason", Session("con"))

                                If rs2.HasRows Then
                                    j = 0
                                    While rs2.Read
                                        'Response.Write(reasonname.Length.ToString)
                                        For j = 0 To reasonname.Length - 1
                                            ' Response.Write(rs2.Item("loan_no") & "=" & emptid & "<br>")
                                            cellb(j) = "0"
                                            cellbval(j) = "0"
                                            ' Response.Write(reasonname(j) & "=" & rs2.Item("reason") & "<br>")
                                            If reasonname(j) = rs2.Item("reason") Then
                                                cellb(j) = fm.getinfo2("select amount from emp_loan_settlement where id='" & rs2.Item("id") & "'", Session("con"))
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                ' j += 1

                                            ElseIf reasonname(j) <> rs2.Item("reason") Then
                                                If cellbval(j) = "" Then
                                                    cellb(j) = "0.00"
                                                    cellbval(j) = "0"
                                                    sumloan(j) += CDbl(cellb(j))
                                                End If
                                                ' j += 1
                                            End If

                                        Next

                                    End While
                                Else
                                    For j = 0 To reasonname.Length - 1
                                        cellb(j) = "0.00"
                                        cellbval(j) = "0"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next

                                End If
                            Else

                                For j = 0 To reasonname.Length - 1
                                    cellb(j) = "0.00"
                                    cellbval(j) = "0"
                                    sumloan(j) += CDbl(cellb(j))
                                Next

                            End If
                            rs2.Close()

                        End If
                        ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                        Dim penss As String
                        penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", Session("con"))
                        ' Response.Write(penss & "<br>")

                        cell(12) = fm.getinfo2("select pen_e from payrollx where ref='" & ref & "' and emptid=" & rs.Item("id"), Session("con"))
                        If cell(12) = "None" Then
                            cell(12) = "0"
                        End If



                        cell(13) = fm.getinfo2("select pen_c from payrollx where ref='" & ref & "' and emptid=" & rs.Item("id"), Session("con"))
                        If cell(13) = "None" Then
                            cell(13) = "0"
                        End If




                        ' Response.Write(cell(3).ToString & "===")

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
                            outp &= "<tr><td id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

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
                                        outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                    Else
                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                    End If
                                Else
                                    For j = 0 To ccol - 1
                                        If String.IsNullOrEmpty(cellbval(j)) = False Then
                                            outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                        Else
                                            outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
                                        End If
                                    Next
                                    If ccol = 0 Then
                                        outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                    End If
                                End If
                            Next
                            netincom = CDbl(cell(8)) - CDbl(cell(14))
                            If color <> "#ccaa99" Then
                                sumnet += netincom
                            End If
                            ' Response.Write(netincom.ToString & "<----<br>")
                            outp &= "<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"
                            outp &= "<td  style='text-align:right;'>"
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
                            outp &= "<td style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                            k += 1
                        End If

                    End While


                    outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                    "<td class='cooo' >&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
                    "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                     "<td class='cooo' style='text-align:right;'>&nbsp;</td>" & _
                     "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bearn'>" & fm.numdigit(sumbearn, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtalw'>" & fm.numdigit(sumtalw, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumalw'>" & fm.numdigit(sumalw.ToString, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumot'>" & fm.numdigit(sumot, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumgross'>" & fm.numdigit(sumgross, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumti'>" & fm.numdigit(sumtincome, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumt'>" & fm.numdigit(sumtax, 2).ToString & "</td>"


                    For j = 0 To ccol - 1
                        outp &= ("<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='lon-" & j & "'>" & fm.numdigit(sumloan(j).ToString, 2) & "</td>")
                    Next
                    If ccol = 0 Then
                        outp &= ("<td class='cooo' style='text-align:right;'>&nbsp;</td>")
                    End If
                    outp &= "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
                    "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
                     "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                      "<td class='cooo'  colspan='2'>..&nbsp;" & _
            "</td></tr><tr id='result'></tr>"
                    outp &= fm.signpart()
                    outp &= "</tbody></table>"
                    rs.Close()
                    outp &= fm.projtrans(sec.dbHexToStr(Request.QueryString("projname")), pdate1, Session("con"))
                    fm = Nothing
                    dbs = Nothing
                    Response.Write("<div>" & outp & "</div>")
                    ' Response.Write("<input type=hidden id='delpaid' name='delpaid' value='" & paidlist & "⌡" & loanid & "⌡" & otid & "'>")
                    ' Response.Write("payrol no. list:" & paidlist & "<br>loan settled list: " & loanid & "<br>OT List:" & otid)
                End If
                'Dim xprint As String = ""
                ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))






            End If
        Catch ex As Exception
            Response.Write("<div style='width:600px;color:blue'>" & ex.ToString & " Sorry data is not coming out</div>")
        End Try
        Return outp
    End Function
End Class
