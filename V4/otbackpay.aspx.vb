Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class otbackpay
    Inherits System.Web.UI.Page
    Public pdate1g As Date
    Function getsal(ByVal emptid As Integer, ByVal d1 As Date, ByVal conx As SqlConnection)
        Dim dds As New dbclass
        Dim dt As DataTableReader
        Dim rt(2) As String
        Dim d2 As Date
        d2 = CDate(d1.Month.ToString & "/" & Date.DaysInMonth(CInt(d1.Year), CInt(d1.Month)).ToString & "/" & d1.Year.ToString)
        dt = dds.dtmake("mmm", "select basic_salary,currency,date_start from emp_sal_info where emptid=" & emptid & " and (date_start between '" & d1 & "' and '" & d2 & "' or '" & d1 & "' between date_start and isnull(date_end,'" & d2 & "')) order by id desc", conx)
        ' dt = Me.dtmake("mmm", "select basic_salary,currency,date_start from emp_sal_info where emptid=" & emptid & " order by id desc", conx)
        'Response.Write("<br>select basic_salary,currency,date_start from emp_sal_info where emptid=" & emptid & " and (date_start between '" & d1 & "' and '" & d2 & "' or '" & d1 & "' between date_start and isnull(date_end,'" & d2 & "')) order by id desc")
        If dt.HasRows Then
            While dt.Read
                'If d1.Subtract(CDate(dt.Item("date_start"))).Days >= 0 Then
                rt(0) = dt.Item(0)
                rt(1) = dt.Item(1)
                ' Exit While
                ' End If
            End While
        Else
            rt(0) = "0"
            rt(1) = "empsalary.aspx"
        End If
        dt.Close()
        Return rt
    End Function
    Public Function makeformpx()

        Session.Timeout = 60
        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim fl As New file_list
        Dim sec As New k_security
        Dim namelist As String = ""
        Dim cell(17) As Object

        Dim rrr As DataTableReader
        Dim sum(15) As Double
        Dim avalue As String = ""

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

        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker

        Dim outp As String = ""
        ' Response.Write(Request.Form("projname"))

        Dim damt As Double = 0
        If Request.QueryString("prid") <> "" Then
            Response.Write(Request.QueryString("prid"))
        Else
            ' Response.Write(Request.QueryString("month"))
            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                Response.Write(Request.QueryString("paidst"))
                Dim spl() As String
                Dim projid As String = ""
                Dim projename As String = ""
                If Request.Form("projname") <> "" Or Request.QueryString("projname") <> "" Then
                    If Request.Form("projname") <> "" Then
                        'If String.IsNullOrEmpty(Request.Form("projname")) = False Then
                        spl = Request.Form("projname").Split("|")
                        projename = Request.Form("projname")
                    Else

                        projename = sec.dbHexToStr(Request.QueryString("projname"))
                        spl = projename.Split("|")
                    End If

                    If spl.Length > 1 Then
                        projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                    Else
                        projid = ""
                    End If

                End If
                ' Response.Write(Request.QueryString("projname"))
                If Request.Form("month") <> "" Then
                    nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
                Else
                    nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                    pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
                    pdate2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
                End If

                Dim paid As String
                Dim j As Integer

                Dim paypaid As Integer = 0
                ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
                If paypaid <> 0 Then
                    ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

                End If
                paid = ""
                paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in(select pr from payrollx where ref_inc is null or ref_inc='0' )", Session("con"))
                If paid = "None" Then
                    Response.Write("There is No Payroll list")
                Else
                    rrr = dbs.dtmake("payrol", "select distinct ref,date_paid,bank from payrollx where pr='" & paid & "' and remark='OT-Payment'", Session("con"))
                    If rrr.HasRows Then
                        Response.Write("<div id='viewlistx'><b>Project: " & projename & "<br>Payroll in the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</b><table>")
                        Dim ccout As String = "0"
                        Dim emid, rtnvalue, eml() As String
                        rtnvalue = getprojemp(projid.ToString, pdate2, Session("con"))
                        While rrr.Read
                            '   projename = sec.dbStrToHex(projename)
                            '  Response.Write(rrr.Item(0) & "<br>")
                            ' Response.Write(projid)
                            If projid.ToString <> "" Then
                                'Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')", Session("con")))
                                ' ccout = "0"
                                'ccout = fm.getinfo2("select count(id) from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))


                                'If fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "')and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))", Session("con")).ToString = projid.ToString Then
                                ' If projid.ToString <> "" Then
                                'Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')", Session("con")))
                                ccout = "0"
                                ccout = fm.getinfo2("select count(id) from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                               
                                ' Response.Write(rrr.Item("ref"))
                                emid = fm.getinfo2("select emptid from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                                'Response.Write(emid & "===" & rtnvalue & "----" & rrr.Item("ref") & pdate2.ToShortDateString & "<br>")
                                eml = rtnvalue.Split(",")
                                If fm.searcharray(eml, "'" & emid & "'") Then
                                    Response.Write("<tr><td class='listcont'>" & rrr.Item(0) & "<br><span style='color:gray;font-size:10pt;'>(No. List:" & ccout & ")</span>" & _
                                    "</td><td class='v1'>")
                                    Dim fx() As String = {""}
                                    If String.IsNullOrEmpty(Session("right")) = False Then
                                        fx = Session("right").split(";")
                                        ReDim Preserve fx(UBound(fx) + 1)
                                        fx(UBound(fx) - 1) = ""
                                    End If
                                    If fm.searcharray(fx, "1") Or fm.searcharray(fx, "9") Then
                                        Response.Write("<div class='deletepayrol' onclick=" & Chr(34) & _
                                    "javascript:gotocheckot('del','" & rrr.Item(0) & "');" & Chr(34) & _
                                    " ></div>")
                                    Else
                                        Response.Write("<div class='deletepayrol'></div>")
                                    End If
                                    Response.Write("</td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='viewpayrol' onclick=" & _
                                        Chr(34) & "javascript:gotocheckot('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "');" & _
                                        Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='viewdelpayrol' onclick=" & Chr(34) & _
                                        "javascript:gotocheckot('viewdel','?prdel=" & rrr.Item(0) & "&pd=" & rrr.Item(1) & "');" & _
                                        Chr(34) & "></div></td><td> &nbsp;|&nbsp;</td><td class='v1'><div class='editpayrol' onclick=" & Chr(34) & _
                                        "javascript:gotocheckot('Edit','" & rrr.Item(0) & "');" & _
                                        Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='bankpayrol' onclick=" & Chr(34) & _
                                        "javascript:gotocheckot('Bank','?ref=" & rrr.Item(0) & _
                                        "&ppd=" & pdate1 & "&bname=" & rrr.Item("bank") & "'); " & Chr(34) & _
                                        "></div></td></tr><tr><td colspan=8><hr style='width:600px;' align=left></td></tr>")
                                End If
                                End If

                        End While
                        Response.Write("</table></div>")
                    End If
                    'Response.Write(paid)
                End If
            End If
            dbs = Nothing
            fm = Nothing
        End If

    End Function
    
    Public Function makeformpaidx()
        Session.Timeout = 60

        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String
        Dim avalue As String = ""
        Dim sum(15) As Double
        Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
        Dim dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
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
        Dim sal() As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        Dim sec As New k_security
        ' Response.Write(Request.QueryString("month"))
        Dim ref As String = ""
        If Request.QueryString("month") <> "" Then
            Dim spl() As String
            Dim projid As String = ""
            If Request.QueryString("projname") <> "" Then
                spl = sec.dbHexToStr(Request.QueryString("projname")).Split("|")
                If spl.Length > 1 Then
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                Else
                    projid = ""
                End If
            End If
            ref = Request.QueryString("prid")
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
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate2.Month & "' and year='" & pdate2.Year & "' and id in (select payroll_id from paryrol ref_inc is null)", Session("con"))
            If paid = "None" Then
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            Else
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            End If
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim stylew(4) As String
            Dim tcell As Integer
            tcell = ccol + 14
            Dim ratiow As Double
            Dim wdthpay As Integer = 1250
            If Request.QueryString("widthpay") <> "" Then
                wdthpay = Request.QueryString("widthpay")
            End If



            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
            End If
            'Response.Write("ddd")
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='15' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Over Time Back Pay Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dwot' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp. Id.</td>"
                outp &= "<td class='fxnameot' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitxot' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitxot'   colspan='1'><center>Back Pay</center></td>"
                outp &= "<td class='fitxot' rowspan='2'>Total Payment</td>"
                outp &= "<td class='fitxot' rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitxot'  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='fitxot' rowspan='2'> Tax Paid</td>"
                outp &= "<td class='fitxot' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td class='fitxot'  rowspan='2'>Net pay</td>"
                ' outp &= "<td class='dw'  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall2();'>Clear all</td>" & Chr(13)
                outp &= "<td class='signx' rowspan='2'>Signature</td></tr>" & Chr(13)

                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)

                outp &= "<td class='fitxot' >Over Time</td>"


                outp &= "</tr>" & Chr(13)

                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date
                Dim otamt As Double


                While rs.Read
                    'check id in the list2 and make statement again 
                    emptid = rs.Item("id")
                                   ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                    ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                    otamt = 0
                    color = fm.getinfo2("select ot as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con")).ToString
                    'Response.Write(color & "<br>")
                    If IsNumeric(color) = True Then
                        otamt = CDbl(color)
                    Else
                        otamt = 0
                    End If
                    Dim otamtpaid As Double
                    otamtpaid = getotpaidin(pdate1, pdate2, emptid, Session("con"))
                    Dim idg As String
                    idg = fm.getinfo2("select id as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con")).ToString

                    Dim mtx As String = "0"
                    ' mtx = fm.getinfo2("select sum(ot) as exp1 from payrollx where emptid=" & emptid & " and id<" & idg & " and remark='OT-Payment'", Session("con")).ToString

                    If IsNumeric(mtx) = False Then
                        mtx = 0
                    End If
                    Dim strot As String
                    strot = fm.getinfo2("select sum(isnull(ot,0)) as exp1 from payrollx where (emptid=" & emptid & ") and ((pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))) and (ref<>'" & ref & "') and (id<" & idg & ") and remark='OT-Payment'", Session("con")).ToString
                    If IsNumeric(strot) Then
                        otamtpaid = strot
                    Else
                        otamtpaid = 0
                    End If

                    'otamtpaid = otamtpaid - mtx
                    ' Response.Write(otamtpaid & "===>" & mtx & "<br>")
                    ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                    If otamt > 0 Then 'question..........solved
                        ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                        cell(0) = k
                        cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                        cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                        ' cell(3)=fm.getinfo2("select sum(gross_earn) as Exp2 from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month=" 
                        cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "'))) and id<" & idg, Session("con")).ToString
                        '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")

                        'cell(3) = "0"
                        If cell(3) = "None" Or cell(3).ToString.Length > 10 Or String.IsNullOrEmpty(cell(3).ToString) = True Then
                            cell(3) = 0
                        End If
                        cell(3) = CDbl(cell(3)) + otamtpaid
                        cell(4) = otamt
                        cell(5) = cell(3) + cell(4)
                        ' Response.Write(cell(5).ToString & "===" & cell(3).ToString & "<br>")
                        cell(6) = CDbl(cell(5))
                        'cell(3) = 0
                        'Response.Write(()
                        
                        cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString

                        cell(8) = fm.getinfo2("select sum(tax) from payrollx where emptid=" & emptid & " and ref<>'" & ref & "'  and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "'))) and id<=" & idg, Session("con"))

                        If cell(8) = "None" Or cell(8) = "" Then
                            cell(8) = 0
                        End If

                        cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                        Dim netp As Double
                        netp = fm.getinfo2("select netp from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                        cell(10) = CDbl(cell(4)) - CDbl(cell(9))
                        ' Response.Write(cell(10) & "===" & netp & "<br>")
                        If netp <> cell(10) Then
                            cell(10) = netp
                            'cell(8) = cell(8) - (cell(10) - netp)
                        End If
                        If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                            sumbsal += CDbl(cell(3))
                            sumot += cell(4)
                            suminc += CDbl(cell(5))
                            sumtaxinc += CDbl(cell(6))
                            sumtax += CDbl(cell(7))
                            sumptax += CDbl(cell(8))
                            sumtaxpay += CDbl(cell(9))
                            sumnet += CDbl(cell(10))

                        End If
                        outp &= "<tr>"
                        For i As Integer = 0 To 10
                            If i = 0 Or i = 1 Or i = 2 Then
                                outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                            Else
                                outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                            End If


                        Next


                        outp &= "<td style='border-right:1px solid black;'>&nbsp;</td></tr>" & Chr(13)
                        k += 1

                    End If

                End While
                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
               "<td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td>" & _
               "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumot, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                 "<td class='cooo' style='border-right:1px solid black;'>&nbsp</td>"


                rs.Close()
            End If

            outp &= "</tr>"
            outp &= signpart()
            Dim pdate As Date
            pdate = CDate(fm.getinfo2("select pddate from payrollx where  ref='" & ref & "'", Session("con")))
            outp &= "<tr><td colspan='12'>Paid on: " & MonthName(pdate.Month).ToString & " " & pdate.Day & ", " & pdate.Year.ToString & "</td></tr>"

            outp &= "</table>"
            outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

            fm = Nothing
            dbs = Nothing
            Response.Write(outp)



            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))

            Return outp

        End If
    End Function
    Public Function makeformpaidxdel()
        Session.Timeout = 60
        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String
        Dim avalue As String = ""
        Dim sum(15) As Double
        Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
        ' Dim dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
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
        Dim sal() As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        Dim sec As New k_security
        ' Response.Write(Request.QueryString("month"))
        Dim ref As String = ""
        If Request.QueryString("month") <> "" Then
            Dim spl() As String
            Dim projid As String = ""
            If Request.QueryString("projname") <> "" Then
                spl = sec.dbHexToStr(Request.QueryString("projname")).Split("|")
                If spl.Length > 1 Then
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                Else
                    projid = ""
                End If
            End If
            ref = Request.QueryString("prdel")
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
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate2.Month & "' and year='" & pdate2.Year & "' and id in (select payroll_id from paryrol ref_inc is null)", Session("con"))
            If paid = "None" Then
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            Else
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            End If
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim ottext As String = ""
            Dim stylew(4) As String
            Dim tcell As Integer
            tcell = ccol + 14
            Dim ratiow As Double
            Dim wdthpay As Integer = 1250
            If Request.QueryString("widthpay") <> "" Then
                wdthpay = Request.QueryString("widthpay")
            End If



            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
            End If
            'Response.Write("ddd")
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='15' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Over Time Back Pay Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td rowspan='2'>del</td><td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp. Id.</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitx'  ><center>Back Pay</center></td>"
                outp &= "<td class='fitx' rowspan='2'>Total Payment</td>"
                outp &= "<td class='fitx' rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitx'  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Tax Paid</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td class='fitx'  rowspan='2'>Net pay</td>"
                ' outp &= "<td class='dw'  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall2();'>Clear all</td>" & Chr(13)
                outp &= "<td class='fxname' rowspan='2'>Signature</td></tr>" & Chr(13)

                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)

                outp &= "<td class='fitx' >Over Time</td>"


                outp &= "</tr>" & Chr(13)

                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date
                Dim otamt As Double
                While rs.Read
                    'check id in the list2 and make statement again 
                    emptid = rs.Item("id")
                    ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")


                    ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                    otamt = 0
                    ottext = ""
                    ottext = fm.getinfo2("select ot as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con")).ToString
                    Dim otamtpaid As Double
                    otamtpaid = getotpaidin(pdate1, pdate2, emptid, Session("con"))
                    Dim idg As String
                    idg = fm.getinfo2("select id as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con")).ToString

                    Dim mtx As String
                    mtx = fm.getinfo2("select sum(ot) as exp1 from payrollx where emptid=" & emptid & " and id>" & idg & " and remark='OT-Payment'", Session("con")).ToString
                    'Response.Write(mtx.ToString & "<br>")
                    If IsNumeric(mtx) = False Then
                        mtx = 0
                    End If
                    ' Response.Write(otamt.ToString)
                    ' Response.Write("===>" & otamtpaid.ToString & "<br>")
                    otamtpaid = otamtpaid - mtx
                    If IsNumeric(ottext) = True Then
                        otamt = CDbl(ottext)
                    Else
                        otamt = 0
                    End If

                    ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                    If otamt > 0 Then 'question..........solved
                        ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                        cell(0) = k
                        cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                        cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                        ' cell(3)=fm.getinfo2("select sum(gross_earn) as Exp2 from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month=" 
                        cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "'))) and id<=" & idg, Session("con")).ToString
                        '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")
                        '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")

                        'cell(3) = "0"
                        If cell(3) = "None" Or cell(3).ToString.Length > 10 Or String.IsNullOrEmpty(cell(3).ToString) = True Then
                            cell(3) = 0
                        End If
                        cell(3) = CDbl(cell(3))

                        cell(4) = otamt
                        cell(5) = cell(3) + cell(4)
                        ' Response.Write(cell(5).ToString & "===" & cell(3).ToString & "<br>")
                        cell(6) = CDbl(cell(5))
                        'cell(3) = 0
                        'Response.Write(()
                        cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString

                        cell(8) = fm.getinfo2("select sum(tax) from payrollx where emptid=" & emptid & " and ref<>'" & ref & "'  and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "'))) and id<=" & idg, Session("con"))

                        If cell(8) = "None" Or cell(8) = "" Then
                            cell(8) = 0
                        End If

                        cell(9) = CDbl(cell(7)) - CDbl(cell(8))


                        Dim netp As Double
                        netp = fm.getinfo2("select netp from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                        cell(10) = CDbl(cell(4)) - CDbl(cell(9))
                        If netp <> cell(10) Then
                            cell(10) = netp
                            cell(8) = cell(8) - (cell(10) - netp)
                        End If
                        If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                            sumbsal += CDbl(cell(3))
                            sumot += cell(4)
                            suminc += CDbl(cell(5))
                            sumtaxinc += CDbl(cell(6))
                            sumtax += CDbl(cell(7))
                            sumptax += CDbl(cell(8))
                            sumtaxpay += CDbl(cell(9))
                            sumnet += CDbl(cell(10))

                        End If
                        outp &= "<tr><td><span class='iddel' onclick=" & Chr(34) & "javascript:delsingle('emptid=" & emptid & "&ref=" & ref & "');" & Chr(34) & ">del</span></td>"
                        For i As Integer = 0 To 10
                            If i = 0 Or i = 1 Or i = 2 Then
                                outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                            Else
                                outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                            End If


                        Next


                        outp &= "<td style='border-right:1px solid black;'>&nbsp;</td></tr>" & Chr(13)
                        k += 1

                    End If

                End While
                outp &= "<tr style='text-weight:bold;text-align:right;'><td>&nbsp</td>" & _
               "<td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td>" & _
               "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumot, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                 "<td class='cooo' style='border-right:1px solid black;'>&nbsp</td>"


                rs.Close()
            End If

            outp &= "</tr>"
            outp &= signpart()
            outp &= "</table>"
            outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

            fm = Nothing
            dbs = Nothing
            sec = Nothing
            Response.Write(outp)



            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))

            Return outp

        End If
        Return outp
    End Function
    Public Function makeformunpaid()
        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(11) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double

        Dim dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
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
        Dim sal() As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0

        If Request.Form("month") <> "" Then
            'Response.Write(fl.msgboxt("onfram", "Progression", "Progression shown"))
            'Response.Write("<script>showobj('progressbar');</script>")
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
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where ref_inc is null or ref_inc='0')", Session("con"))


            ' Response.Write(ccol)
            ccol = 1
            'Response.Write(ccol)
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
                Dim rssqlnew As String
                Dim rtnvalue As String
                rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "

                '  rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                   "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                 "and emprec.id in(select emptid from emp_job_assign " & _
                '                                                "where project_id='" & projid.ToString & "' " & _
                '                                               "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                '                                              " ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                rs = dbs.dtmake("selectemp", rssqlnew, Session("con"))
            End If
            'Response.Write("ddd")
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='15' >" & Session("company_name") & _
            "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Over Time Back Pay Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp. Id.</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitx'   colspan='" & (CInt(ccol)).ToString & "'><center>Back Pay</center></td>"
                outp &= "<td class='fitx' rowspan='2'>Total Payment</td>"
                outp &= "<td class='fitx' rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitx'  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Tax Paid</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td class='fitx'  rowspan='2'>After tax</td>"
                outp &= "<td class='fitx'  rowspan='2'>Deduction</td>"
                outp &= "<td class='fitx'  rowspan='2'>Net Pay</td>"
                outp &= "<td class='dw'  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall2();'>Clear all</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Signature</td></tr>" & Chr(13)

                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)

                outp &= "<td class='fitx' >Over Time</td>"


                outp &= "</tr>" & Chr(13)

                Dim k As Integer = 1
                Dim color As String = ""

                Dim otamt As Double = 0
                While rs.Read
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))

                    emptid = rs.Item("id")
                    paid = "None"
                    ' paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where emptid='" & emptid & "' and ref_inc='0' or ref_inc is null)", Session("con"))
                    'Response.Write(paid.ToString & "<br>...")
                    If paid.ToString = "None" Then
                        ' resing = "#1/1/1900#"
                        If color <> "#aabbcc" Then
                            color = "#aabbcc"
                        Else
                            color = "white"
                        End If



                        otamt = 0

                        otamt = fm.getotunp(pdate1, pdate2, emptid, Session("con"))
                        Dim idg As Integer
                        'idg=fm.getinfo2("select id from payroll 
                        Dim otamtpaid As Double
                        otamtpaid = getotpaidin(pdate1, pdate2, emptid, Session("con"))
                        If otamtpaid > 0 Then
                            ' otamt += otamtpaid
                            '  Response.Write(otamtpaid.ToString & "emptid;;;" & emptid & "<br>")
                        End If
                        ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                        If otamt > 0 Then 'question..........solved
                            ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                            cell(0) = k
                            cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                            cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                            ' cell(3)=fm.getinfo2("select sum(gross_earn) as Exp2 from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month=" 
                            cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString
                            '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")
                            'Response.Write("select sum(txinco) as exp1 from payrollx where tax<>'0' and emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))" & "<br>")
                            'cell(3) = "0"
                            Dim otp As String
                            '  Response.Write(cell(1).ToString & emptid & "<br>")
                            otp = fm.getinfo2("select sum(ot) as exp1 from payrollx where tax<>'0' and (b_sal is null or b_sal='0')  and emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString

                            If IsNumeric(otp) Then
                                otamtpaid = otp
                            Else
                                otamtpaid = 0
                            End If
                            If IsNumeric(cell(3)) = False Then
                                cell(3) = 0
                            End If

                            cell(3) = CDbl(cell(3)) + otamtpaid
                            cell(4) = otamt
                            cell(5) = cell(3) + cell(4)
                            ' Response.Write(cell(5).ToString & "===" & cell(3).ToString & "<br>")
                            cell(6) = CDbl(cell(5))
                            'cell(3) = 0
                            'Response.Write(()
                            cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString

                            cell(8) = fm.getinfo2("select sum(tax) from payrollx where emptid=" & emptid & " and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "')", Session("con"))

                            If cell(8) = "None" Or cell(8) = "" Then
                                cell(8) = 0
                            End If

                            cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                            cell(10) = CDbl(cell(4)) - CDbl(cell(9))
                            If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                sumbsal += CDbl(cell(3))
                                sumot += cell(4)
                                suminc += CDbl(cell(5))
                                sumtaxinc += CDbl(cell(6))
                                sumtax += CDbl(cell(7))
                                sumptax += CDbl(cell(8))
                                sumtaxpay += CDbl(cell(9))
                                sumnet += CDbl(cell(10))

                            End If
                            '  sql = "select distinct reason,id,bal,nopay,amt from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and deduction ='ot_back' and "

                            ' Response.Write(sql & "<br>")
                            ' rs2 = dbs.dtmake("loan", sql, Session("con"))
                            Dim title As String = ""
                            Dim ck As String = ""
                            If cell(3) = 0 Then
                                color = "#abcdef"
                                title = "Salary amount is not mentioned! Please first make a payroll."
                                ck = "Disabled='disabled'"
                            End If
                            color = "white"

                            outp &= "<tr style='background-color:" & color & "' title='" & title & "'>"

                            For i As Integer = 0 To 10
                                If CDbl(cell(3)) = 0 And i = 3 Then
                                    color = "red"
                                ElseIf CDbl(cell(8)) = 0 And i = 8 Then
                                    color = "red"
                                Else

                                    color = "white"
                                End If

                                If i = 0 Or i = 1 Or i = 2 Then
                                    outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                Else
                                    outp &= ("<td  style='background:" & color & ";text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                End If


                            Next

                            ' Response.Write(paid)
                            If paid.ToString = "None" Then
                                outp &= "<td> <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' "

                                outp &= "checked='checked'"


                                outp &= " onclick='javascript:sumcolx2();' "
                                If ck <> "" Then
                                    outp &= " " & ck
                                End If
                                outp &= "></td>"
                            End If
                            outp &= "<td style='border-right:1px solid black;'>&nbsp;</td></tr>" & Chr(13)
                            k += 1

                        End If
                    End If
                End While
                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
               "<td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td>" & _
               "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumot, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>&nbsp</td>" & _
                 "<td class='cooo' style='border-right:1px solid black;'>&nbsp</td>"


                rs.Close()
            End If

            outp &= "</tr>"
            outp &= fm.signpart2()
            outp &= "</table>"
            outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

            fm = Nothing
            dbs = Nothing
            Response.Write(outp)



            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))

            Return outp

        End If
        Return outp
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
        Dim avalue As String = ""
        Dim sum(15) As Double
        Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
        Dim dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
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
        Dim sal() As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        Dim sec As New k_security
        'Response.Write(Request.QueryString("month"))

        If Request.QueryString("month") <> "" Then
            Dim spl() As String
            Dim projid As String = ""
           ' Response.Write(Request.QueryString("projname"))
            If Request.QueryString("projname") <> "" Then
                spl = sec.dbHexToStr(Request.QueryString("projname")).Split("|")
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
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate2.Month & "' and year='" & pdate2.Year & "' and id in (select payroll_id from paryrol ref_inc is null)", Session("con"))
            If paid = "None" Then
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            Else
                ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))

            End If
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim stylew(4) As String
            Dim tcell As Integer
            tcell = ccol + 14
            Dim ratiow As Double
            Dim wdthpay As Integer = 1250
            If Request.QueryString("widthpay") <> "" Then
                wdthpay = Request.QueryString("widthpay")
            End If

            ratiow = (wdthpay - 350 - 30 - 60) / tcell
            stylew(0) = "30px"
            stylew(1) = "60px"
            stylew(2) = "350px"
            stylew(3) = Math.Round(ratiow, 0).ToString & "px"

            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                'rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
                Dim rssqlnew As String
                Dim rtnvalue As String
                rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "

                '  rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                   "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                 "and emprec.id in(select emptid from emp_job_assign " & _
                '                                                "where project_id='" & projid.ToString & "' " & _
                '                                               "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                '                                              " ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                rs = dbs.dtmake("selectemp", rssqlnew, Session("con"))
            End If
            'Response.Write("ddd")
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='15' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Over Time Back Pay Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "<br><span class='ref'></span></td>" & Chr(13)
                pdate1g = pdate2
                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp. Id.</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitx'   colspan='1'><center>Back Pay</center></td>"
                outp &= "<td class='fitx' rowspan='2'>Total Payment</td>"
                outp &= "<td class='fitx' rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitx'  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Tax Paid</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td class='fitx'  rowspan='2'>Net pay</td>"
                ' outp &= "<td class='dw'  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall2();'>Clear all</td>" & Chr(13)
                outp &= "<td class='fxname' rowspan='2'>Signature</td></tr>" & Chr(13)

                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)

                outp &= "<td class='fitx' >Over Time</td>"


                outp &= "</tr>" & Chr(13)

                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date
                Dim otamt As Double
                While rs.Read
                    'check id in the list2 and make statement again 
                    emptid = rs.Item("id")
                    ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                    If fm.searcharray(vx, emptid.ToString) Then

                        ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                        otamt = 0

                        otamt = fm.getotunp(pdate1, pdate2, emptid, Session("con"))
                        Dim otamtpaid As Double
                        otamtpaid = getotpaidin(pdate1, pdate2, emptid, Session("con"))

                        ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                        If otamt > 0 Then 'question..........solved
                            ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                            cell(0) = k
                            cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                            cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                            ' cell(3)=fm.getinfo2("select sum(gross_earn) as Exp2 from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month=" 
                            cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString
                            '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")

                            'cell(3) = "0"
                            Dim otp As String
                            otp = fm.getinfo2("select sum(ot) as exp1 from payrollx where tax<>'0' and (b_sal is null or b_sal='0')  and emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString
                            If IsNumeric(otp) Then
                                otamtpaid = otp
                            Else
                                otamtpaid = 0
                            End If
                            If IsNumeric(cell(3)) = False Then
                                cell(3) = 0
                            End If
                            cell(3) = CDbl(cell(3)) + otamtpaid
                            cell(4) = otamt
                            cell(5) = cell(3) + cell(4)
                            ' Response.Write(cell(5).ToString & "===" & cell(3).ToString & "<br>")
                            cell(6) = CDbl(cell(5))
                            'cell(3) = 0
                            'Response.Write(()
                            cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString

                            cell(8) = fm.getinfo2("select sum(tax) from payrollx where emptid=" & emptid & " and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "')", Session("con"))

                            If cell(8) = "None" Or cell(8) = "" Then
                                cell(8) = 0
                            End If

                            cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                            cell(10) = CDbl(cell(4)) - CDbl(cell(9))
                            If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                sumbsal += CDbl(cell(3))
                                sumot += cell(4)
                                suminc += CDbl(cell(5))
                                sumtaxinc += CDbl(cell(6))
                                sumtax += CDbl(cell(7))
                                sumptax += CDbl(cell(8))
                                sumtaxpay += CDbl(cell(9))
                                sumnet += CDbl(cell(10))

                            End If
                            outp &= "<tr>"
                            For i As Integer = 0 To 10
                                If i = 0 Or i = 1 Or i = 2 Then
                                    outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                Else
                                    outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                End If


                            Next


                            outp &= "<td style='border-right:1px solid black;'>&nbsp;</td></tr>" & Chr(13)
                            k += 1

                        End If
                    End If
                End While
                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
               "<td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td>" & _
               "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumot, 2).ToString & "</td>"

                outp &= "<td class='cooo' style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                 "<td class='cooo' style='border-right:1px solid black;'>&nbsp</td>"


                rs.Close()
            End If

            outp &= "</tr>"
            outp &= signpart()
            outp &= "</table>"
            outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

            fm = Nothing
            dbs = Nothing
            sec=Nothing
            Response.Write(outp)



            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))

            Return outp

        End If



    End Function
    Public Function makeform2v(ByVal vx() As String)
        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String
        Dim avalue As String = ""
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
        Dim sal() As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        ' Response.Write(Request.QueryString("month"))

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
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate2.Month & "' and year='" & pdate2.Year & "' and id in (select pr from payrollx ref_inc is null or ref_inc='0')", Session("con"))
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
            Dim stylew(4) As String
            Dim tcell As Integer
            tcell = ccol + 14
            Dim ratiow As Double
            Dim wdthpay As Integer = 1250
            If Request.QueryString("widthpay") <> "" Then
                wdthpay = Request.QueryString("widthpay")
            End If

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
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
            End If

            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='18' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td rowspan='2' class='dw' style='width:" & stylew(0).ToString & "'>No.</td><td class='dw' style='width:" & stylew(1).ToString & ";' rowspan='2'>Emp. Id</td>"
                outp &= "<td class='fxname' style='width:" & stylew(2) & ";' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                outp &= "<td  class='dw' style='width:30px;' rowspan='2'>Days Worked</td>"
                outp &= "<td  class='fitx' rowspan='2'>Basic Earning</td>"
                outp &= "<td  class='fitx' rowspan='2'>Taxable Allowance</td>"
                outp &= "<td  class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Overtime</td>"
                outp &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='dedct' style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Net Pay</td>"

                outp &= "<td rowspan='2' class='signpart'>Signature</td>"
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
                                outp &= "<td class='dedctx'>Other</td>"
                            Else
                                outp &= "<td class='dedctx'>" & rs2.Item("reason").ToString & "</td>"
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
                        If (resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing And incwhen = 0 Then 'question..........
                            ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            ' Response.Write(sql & "<br>")
                            sal = Me.getsal(emptid, pdate1, Session("con"))
                            cell(2) = sal(0)
                            If cell(2) = "Sorry This employee salary info is not setted!" Then
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

                            cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
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
                            cell(6) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='n') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            'Response.Write(cell(6) & "<br>")
                            ' cell(6) = "0"
                            If cell(6).ToString = "None" Then
                                cell(6) = "0"
                            ElseIf IsNumeric(cell(6)) = False Then
                                cell(6) = "0"
                            End If
                            If CDbl(cell(6)) > 0 Then
                                cell(6) = (CDbl(cell(6)) / nod) * calc
                                ' cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                            End If
                            cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            'cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
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
                            Dim dhr As Date
                            dhr = rs.Item("hire_date")
                            ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            Dim penss As String
                            penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", Session("con"))
                            ' Response.Write(pdate2.Subtract(dhr).Days.ToString & "<br>")
                            'Response.Write(penss.ToString & "<br>")
                            If pdate2.Subtract(dhr).Days >= 45 Then
                                cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                            Else
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If
                            If penss <> "None" Then
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If                           ' Response.Write(cell(3).ToString & "===")
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
                                'Response.Write(emptid & "<br>")
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
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                        End If
                                    Else
                                        For j = 0 To ccol - 1
                                            If String.IsNullOrEmpty(cellbval(j)) = False Then
                                                outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>&nbsp;" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                            Else
                                                outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>&nbsp;0.00</td>")
                                            End If
                                        Next
                                        If ccol = 0 Then
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                        End If
                                    End If
                                Next
                                netincom = CDbl(cell(8)) - CDbl(cell(14))
                                If color <> "#ccaa99" Then
                                    sumnet += netincom
                                End If
                                ' Response.Write(netincom.ToString & "<----<br>")
                                outp &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"

                                outp &= "<td colspan='2' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If ' end of selected array
                End While


                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                "<td class='cooo' style=''>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
                "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td>" & _
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
                    outp &= ("<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td>")
                End If
                outp &= "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & Chr(13) & _
                "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & Chr(13) & _
                 "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & Chr(13) & _
                  "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & Chr(13) & _
                  "<td class='cooo' style='' colspan='2'>..&nbsp;" & _
     "</td></tr>"
                'outp &= "</table><br>"
                'outp &= "<table cellspacing='0' cellpadding='0' width='800px' style='font-size:9.5pt;'>"
                outp &= fm.signpart()
                outp &= "</table>" & Chr(13)
                outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
            End If

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
    
    

    Function deleteallx(ByVal ref As String)

        Dim dbs As New dbclass
        Dim flg As String = ""
        If Session("username") <> "" Then


            Dim sqlst As String = "BEGIN TRANSACTION " & Session("username") & Chr(13)


            sqlst &= "delete from payrollx where ref='" & ref & "'" & Chr(13)

            sqlst &= "delete from emp_loan_settlement where ref='" & ref & "'" & Chr(13)

            sqlst &= "Update emp_ot set paidstatus='n',ref=NULL where ref='" & ref & "'" & Chr(13)


            'Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
            flg = dbs.excutes(sqlst, Session("con"), Session("path"))
            If IsNumeric(flg) = True Then
                ' Response.Write(flg)
                If CInt(flg) <= 0 Then
                    dbs.excutes("RollBack TRANSACTION " & Session("username"), Session("con"), Session("path"))
                    Response.Write("data is not saved")
                Else
                    dbs.excutes("COMMIT TRANSACTION " & Session("username"), Session("con"), Session("path"))
                    Response.Write("Data Saved")
                End If


            Else
                dbs.excutes("RollBack TRANSACTION " & Session("username"), Session("con"), Session("path"))
                Response.Write("data is not saved")
            End If
        Else
            Response.Write("<script>window.parent.location.href ='logout.aspx?msg=session expired';</script>")
            Response.Redirect("logout.aspx")
        End If
        dbs = Nothing
        Return Nothing
    End Function
   
    Public Function signpart()
        Dim outp As String = ""
        outp &= "<tr class='sss'><td style='height:15px;border-style:none none none none;' colspan='12'>&nbsp;</td></tr>"
        outp &= "<tr class='sss'>"
        outp &= "<td class='sss' style='height:50px;border-style:none none;' colspan='12' align='center'>" & _
"<table width='100%' style='border-style:none none none none;'><tr>" & _
"<td style='width:15%;border-style:none none none none;' colspan='2' align='center'></td>" & _
    "<td style='width:15%;border-style:none none none none;' colspan='2'  align='center'>________________<br>Prepared By</td>" & _
"<td style='width:15%;border-style:none none none none;' colspan='2' align='center'>________________<br>Checked By</td>" & _
"<td style='width:15%;border-style:none none none none;' colspan='2'  align='center'><br></td>" & _
"<td style='width:15%;border-style:none none none none;' colspan='2'  align='center'>________________<br>Approved By</td></tr></table></td>"
        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= "<td class='sss' style='height:15px;border-style:none none;' colspan='12' align='center'>" & _
            "</td>"
        outp &= "</tr>" & Chr(13)
       
        outp &= "<tr>"

        outp &= "<td class='sss' style='height:50px;border-style:none none;' colspan='12' align='center'>" & _
            "<table width='100%' style='border-style:none none none none;'><tr><td style='width:15%;border-style:none none none none;' colspan='2' align='center'></td>" & _
    "<td style='width:15%;border-style:none none none none;' colspan='2' align='center'>________________<br>Date</td>" & _
"<td style='width:15%;border-style:none none none none;' colspan='2'  align='center'>________________<br>Date</td>" & _
"<td style='width:15%;border-style:none none none none;' colspan='2'  align='center'><br></td>" & _
"<td style='width:15%;border-style:none none none none;' colspan='2'  align='center'>________________<br>Date</td></tr></table></td>"
        outp &= " </tr>" & Chr(13)
       
       
       
        Return outp
    End Function
    Public Function getotpaidin(ByVal date1 As Date, ByVal date2 As Date, ByVal emptid As Integer, ByVal con As SqlConnection)
        Dim dbx As New dbclass
        Dim rtamt As Double
        Dim rs As DataTableReader
        rs = dbx.dtmake("selectdb", "select sum(ot) as amt from payrollx where date_paid between '" & date1 & "' and '" & date2 & "' and emptid=" & emptid & " and ref is Not null and remark='OT-Payment' group by emptid", con)
        If rs.HasRows Then
            rs.Read()

            rtamt = rs.Item("amt")
            ' Response.Write(rtamt)
        Else

            rtamt = 0
        End If
        rs.Close()
        Return rtamt
    End Function

    Public Function getprojemp(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim did As String
        rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "'  order by id", con)
        Dim d1, d2, de, ds As Date
        'd1 = Nothing
        d2 = Nothing

        Dim rtn As String = ""
        Dim rtn2 As String = ""
        If rs.HasRows Then

            Try

                'Response.Write(" start    ====     requested     ====    Date end<br>")
                While rs.Read

                    d1 = sdate
                    ds = "#1/1/1900#"
                    de = "#1/1/1900#"
                    ds = rs.Item("date_from")
                    If rs.IsDBNull(3) Then
                        'ds = sdate
                        ds = rs.Item("date_from")
                        de = sdate
                        '  Response.Write("<br>" & rs.Item(2) & "===>" & rs.Item(0) & "<br>")
                        If ishear(projid, sdate, rs.Item(0), Session("con")) Then
                               rtn2 &= "'" & rs.Item(0) & "',"
                        End If
                    Else
                        'Response.Write(ishear(projid, sdate, rs.Item(0), Session("con")).ToString & rs.Item(0) & "<br>")
                        Dim obj() As Object
                        obj = fm.isResign(rs.Item(0), Session("con"))
                        ' Response.Write("<br>resign date===>" & obj(1).ToString)
                        If IsDate(obj(1)) Then
                            ' Response.Write("<br>resign date===>" & obj(1).ToString & rs.Item(0) & "<============<br>")
                            If CDate(obj(1)).Month = sdate.Month And CDate(obj(1)).Year = sdate.Year Then
                                d1 = obj(1)
                            End If
                        Else
                            If CDate(rs.Item(3)).Month = sdate.Month And CDate(rs.Item(3)).Year = sdate.Year Then
                                d1 = sdate
                            End If
                            '   did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)
                        End If
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)
                        'Response.Write(.ToString & rs.Item(0) & "<====<br>")
                        If ishear(projid, d1, rs.Item(0), Session("con")) Then
                            If rs.Item(0) = 367 Then
                                '  Response.Write(rs.Item(2).ToString & "----" & rs.Item(3))
                            End If
                            rtn2 &= "'" & rs.Item(0) & "',"
                        End If
                        If IsDate(did) Then
                            If CDate(did) <> rs.Item("date_end") Then
                                de = rs.Item("date_end")
                            Else

                                If CDate(did).Month = sdate.Month And CDate(did).Year = sdate.Year Then
                                    de = sdate
                                Else

                                    de = CDate(did)
                                End If
                                '  Response.Write("<br>" & rs.Item(0) & did.ToString & ">.........<br>")


                            End If


                        Else
                            ' Response.Write("<br>wwwwwwwwwwwwwwwwwwwwwwwww" & rs.Item(0) & rs.Item(2).ToString & " <= " & sdate.ToShortDateString & " And  <= " & rs.Item(3).ToString & "<br>")
                            ' If rs.Item(2) <= sdate And sdate <= rs.Item(3) Then
                            de = rs.Item("date_end")
                            ' End If


                        End If
                    End If

                    '  Response.Write(ds.ToShortDateString)
                    ' Response.Write("     ====     ")
                    'Response.Write(sdate.ToShortDateString)
                    'Response.Write("     ====      ")

                    '                    Response.Write(de.ToShortDateString)
                    '                   Response.Write("  ====         " & rs.Item("emptid") & fm.getfullname(rs.Item("emp_id"), Session("con")) & "<br>")
                    If ds <= sdate And sdate.Subtract(de).Days <= 0 Then
                        rtn &= "'" & rs.Item("emptid") & "',"


                    Else
                        If rs.Item(0) = 367 Then
                            '          Response.Write(sdate.Subtract(de).Days.ToString & "<<<<<<" & ds.ToShortDateString)
                        End If
                    End If


                End While
            Catch ex As Exception
                '              Response.Write(ex.ToString)
                rtn = "'',"
            End Try

        End If
        If rtn = "" Then
            rtn = "'',"
        End If
        rs.Close()
        dbs = Nothing
        rtn &= "''"
        Dim sp() As String = rtn.Split(",")
        'Response.Write(sp.Length)
        ' Response.Write(rtn & "<===>" & rtn2 & "<br>")
        Return rtn
    End Function
    Function ishear(ByVal projid As String, ByVal dd As Date, ByVal empid As String, ByVal con As SqlConnection)
        Dim sql As String
        sql = "select emptid from emp_job_assign where project_id='" & projid & "' and ('" & dd & "' between date_from and isnull(date_end,'" & dd & "')) and emptid=" & empid
        Dim fm As New formMaker
        Dim st As String = fm.getinfo2(sql, Session("con"))
        If IsNumeric(st) Then
            Return True
        Else
            ' Response.Write(st)
            Return False
        End If

    End Function
End Class
