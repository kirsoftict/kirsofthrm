Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class payrollkirxx

    Inherits System.Web.UI.Page

    Public Function makeformunpaid_payroll() 'unpaid list
        Dim mx As New mail_system
        Dim fm As New formMaker
        Dim sec As New k_security
        Session("payrolllist") = ""
        'Response.Write("Start time:" & Now.Minute.ToString & ":" & Now.Second.ToString & ":" & Now.Millisecond.ToString)
        Session("companyck") = "Net Consult P.L.C"

        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        If Session("company_name") <> Session("companyck") Then

            Dim emailx As New mail_system
            Dim k As Object
            k = emailx.sendemail("Payroll Copyright checkpoint error from " & Session("companyck") & "<br>hrm-System", Session("pwd"), Session("efrom").ToString, Session("eto"), "Payroll:Copyright issue" & Session("company_name"))
            If LCase(k) <> "msg sent" Then
                File.WriteAllText("c:\temp\email\outbox\msg" & Now.Ticks & ".msg", k.ToString)

            End If
            Response.Redirect("logout.aspx?msg=copyright issue&msgtype=Caution&titlex=Caution")



        End If
        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String
        Dim header, body, footer As String
        Dim sum(15) As Double
        Dim avalue As String = ""
        ' Dim bsal, dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(),
        Dim pemp, pco, netincom As Double
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
        ' Dim fm As New formMaker
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
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where ref_inc is null or ref_inc='0')", Session("con"))
            Dim rtnvalue As String = ""
            rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
            ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM " & _
                                    "(SELECT distinct vwloanbal.reason as cnt FROM vwloanbal where vwloanbal.bal>0 and vwloanbal.emptid in" & _
                                        "(" & rtnvalue & ") and vwloanbal.dstart<='" & _
                                    pdate1 & "') AS derivedtbl_1", Session("con")))

          

            If IsNumeric(paid) = False Then
                paid = "0"
            End If

            'Response.Write(ccol)

            Dim pass As Integer = 0
            If ccol = 0 Then
                ccol = 1
            ElseIf ccol = 1 Then
                ccol = 1
                pass = 1
            End If

            'Response.Write(ccol.ToString & "<br>")
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim paidpayrol As String
            Dim stylew(4) As String
            Dim tcell As Integer
            Dim colded As String = ""
            ' Dim rtnvalue As String = ""

            rs2 = dbs.dtmake("dbpenx", "SELECT distinct reason FROM vwloanbal where bal>0 and emptid in" & _
                                        "(" & rtnvalue & ") and dstart<='" & _
                                    pdate1 & "'", Session("con"))
            '  Response.Write("SELECT distinct reason froM (SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
            '  "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
            '  "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
            '   pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1")
            ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

            colded &= "<tr>" & Chr(13)
            If rs2.HasRows Then
                Dim i As Integer = 0
                While rs2.Read
                    ' Response.Write(rs2.Item("reason"))
                    If rs2.Item("reason") = "-" Then
                        colded &= "<td class='dedctx'>Other</td>"
                    Else
                        colded &= "<td class='dedctx'>" & rs2.Item("reason").ToString & "</td>"

                    End If
                    reasonname(i) = rs2.Item("reason")
                    i += 1
                End While
            Else

                colded &= "<td class='dedctx'>&nbsp;</td>"
            End If
            colded &= "</tr>" & Chr(13)
            Dim wdthpay As Integer = 1250
            tcell = ccol + 19
            If Request.QueryString("widthpay") <> "" Then
                wdthpay = Request.QueryString("widthpay")
            End If
            Dim ratiow As Double
            ratiow = (wdthpay - 350 - 30 - 60) / tcell

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
            Session("pstop") = False
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                Try
                    Dim rssql, rssqlnew As String

                    ' Response.Write(projid & pdate2.ToShortDateString)
                    rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                    ' Response.Write(rtnvalue)
                    rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                      "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                      "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                      "and emprec.id in(select emptid from emp_job_assign " & _
                                                                      "where project_id='" & projid.ToString & "' " & _
                                                                      "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                                                                      " ORDER BY emp_static_info.first_name,emprec.id desc "
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
                    Session("payrolllist") = rssqlnew
                    rs = dbs.dtmake("selectemp", rssqlnew, Session("con"))
                    'Response.Write(rssqlnew)
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
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc <br>")
            End If
            'Response.Write("ddd")
            ' Response.Write(rtnvalue)
            If rs.HasRows Then
                header = "<div id='pensionstop' style='display:none;'>Pension stop for terminated:<input type='checkbox' name='chkpensionstop' id='chkpensionstop' value='checked' /></div><table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                header &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                header &= "<td style='text-align:center;font-weight:bold;' colspan='" & tcell & "' >" & Session("company_name") & _
            "<br> Project Name:"
                If projid <> "" Then
                    header &= spl(0).ToString
                Else
                    header &= "All Projects"
                End If
                header &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                header &= "</tr>" & Chr(13)

                header &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                header &= "<td rowspan='2' class='dw'>No.</td><td class='dw' rowspan='2'>Emp. Id</td>"
                header &= "<td class='fxname' rowspan='2'>Full Name</td>"
                header &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                header &= "<td class='dw'  style='width:30px;' rowspan='2'>Days Worked</td>"
                header &= "<td  class='fitx' rowspan='2'>Basic Earning</td>"
                header &= "<td  class='fitx' rowspan='2'>Taxable Allowance</td>"
                header &= "<td  class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                header &= "<td class='fitx' rowspan='2'> Overtime</td>"
                header &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                header &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                header &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                header &= "<td class='dedct' style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                header &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                header &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                header &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                header &= "<td class='fitx' rowspan='2'>Net Pay</td>"
                'header &= "<td class='fitx' rowspan='2'>Leave Bal</td>"
                header &= "<td class='dw'  id='chkall'  rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                header &= "<td rowspan='2' class='signpart'>Signature</td></tr>" & Chr(13)
                If paid = "None" Then
                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                    rs2 = dbs.dtmake("dbpenx", "SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                        "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                        "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                        pdate1 & "'", Session("con"))
                    'Response.Write(ccol)
                Else
                    rs2 = dbs.dtmake("dbpenx", "SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                       "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                       "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                       pdate1 & "'  and emp_job_assign.project_id='" & projid & "'", Session("con"))
                    rs2 = dbs.dtmake("dbpenx", "SELECT reason froM (SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                       "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                       "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                       pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con"))
                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0 and dstart < ='" & pdate1 & "'", Session("con"))
                    ' Response.Write(ccol)
                End If

                rs2.Close()
                header &= colded
                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date
                Dim did As String
                Dim rt() As String
                body = ""
                While rs.Read

                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))

                    emptid = rs.Item("id")
                    If emptid = 620 Then

                    End If

                    paid = ""
                    paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where emptid='" & emptid & "' and (ref_inc='0' or ref_inc is null))", Session("con"))

                    '  Response.Write("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where emptid='" & emptid & "' and ref_inc='0' or ref_inc is null)<br>")
                    If paid.ToString = "None" Then
                        resing = "#1/1/1900#"
                        If color <> "#aabbcc" Then
                            color = "#aabbcc"
                        Else
                            color = "white"
                        End If
                      

                        did = ""
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "' and resign_date>='" & pdate1 & "'", Session("con"))
                        
                        ' Response.Write(did & "<br>select resign_date from emp_resign where emptid='" & emptid & "' and resign_date>='" & pdate1 & "'")
                        If (rs.Item("active") = "n") Then
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "' and resign_date>='" & pdate1 & "'", Session("con"))
                            ' Response.Write("yyy")
                        End If

                        If did <> "None" And did <> "" Then
                            resing = did
                            dbs.excutes("update emprec set active='n' where id=" & emptid, Session("con"), Session("path"))

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
                        If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year Then
                            'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                            incwhen = dateincr.Subtract(pdate1).Days
                        End If

                        If dhr.Month = pdate1.Month And dhr.Year = pdate1.Year Then

                            incwhen = 0
                        End If
                        ' Response.Write(resing.ToShortDateString & "<br>")
                        ' Response.Write(fm.getfullname(rs.Item("emp_id"), Session("con")) & "<br>")
                        ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                        If ((resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing) And incwhen = 0 Then 'question..........solved
                            'Response.Write(emptid & "<br>")
                            ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                            'Response.Write(resing.ToShortDateString & "<br>")
                           
                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            'Response.Write(sql & "<br>")
                            sal = Me.getsalhrm(emptid, pdate1, Session("con"))
                            'Response.Write(emptid & "===>" & sal(0) & "<br>")
                            cell(2) = sal(0)
                            If IsNumeric(sal(0)) = False Then
                                'Response.Write(sal(0))
                                cell(2) = "0"
                            End If
                            ca = 0
                            clwp = 0

                            If cell(2) = "Sorry This employee salary info is not setted!" Then
                                cell(2) = "0"
                                '  color = "#ccaa99"
                            End If
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
                                    fired = pdate2.Subtract(resing).Days + 1
                                    ' Response.Write(resing.ToShortDateString)
                                Else
                                    fired = 0
                                End If

                            End If

                            calc = 0
                            calc = nod - ca - clwp - newemp - fired
                           

                            cell(3) = Math.Round(calc, 2).ToString
                            ' Response.Write("<br>===>cl3:" & cell(3))
                            Dim dbl As Double
                            dbl = (CDbl(cell(2)) / nod) * CDbl(calc)

                            ' Response.Write(dbl.ToString & rs.Item("emp_id") & "<br>")

                            'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                            cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString ' God does this
                            'cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                            ' Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)
                            ' Response.Write(cell(4).ToString & "<br>")

                            'cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
                            Dim allz() As String
                            'allz = getallowancex(emptid, "y", pdate1, calc, Session("con"))

                            allz = getallowancexp(emptid, "y", pdate1, calc, Session("con"))
                            'Response.Write(allz(1).ToString & "<br>")
                            cell(5) = allz(0)
                            ' Response.Write(allz(0).ToString)
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            ElseIf IsNumeric(cell(5)) = False Then
                                ' Response.Write(cell(5).ToString & "<br>")
                                cell(5) = "0"

                            End If

                            If CDbl(cell(5)) > 0 Then
                                'cell(5) = ((CDbl(cell(5)) / nod) * calc).ToString
                                ' Response.Write(cell(5))
                                'cell(5) = (CDbl(cell(5)) / nod) * (nod - fired)
                            End If
                            '  cell(6) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='n') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            'allz = getallowancex(emptid, "n", pdate1, calc, Session("con"))
                            allz = getallowancexp(emptid, "n", pdate1, calc, Session("con"))
                            'Response.Write(allz(1).ToString & "<br>")
                            If emptid = 49 Then
                                '  Response.Write(pdate1 & "=" & allz(1) & "===" & calc & "<br>")
                            End If
                            cell(6) = allz(0)
                            If cell(6).ToString = "None" Then
                                cell(6) = "0"
                            ElseIf IsNumeric(cell(6)) = False Then
                                cell(6) = "0"
                            End If
                            If CDbl(cell(6)) > 0 Then
                                'cell(6) = (CDbl(cell(6)) / nod) * calc
                                ' cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                            End If

                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            cell(10) = Math.Round(pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString
                            ' Response.Write(cell(10) & "<br>")
                            ' pay_tax(cell(9), pdate1)
                            If projid <> "" Then
                                'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                'Response.Write(sql)
                                sql = "select distinct reason,id,bal,nopay,amt from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(" & rtnvalue & ") order by reason"

                                ' Response.Write(sql & "<br>")
                                rs2 = dbs.dtmake("loan", sql, Session("con"))
                            Else
                                rs2 = dbs.dtmake("loan", "select distinct reason,id,bal,nopay,amt from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            End If

                            If rs2.HasRows Then
                                Dim emp1() As String = {""}
                                Dim list() As String = {""}
                                For j = 0 To reasonname.Length - 2
                                    cellb(j) = 0
                                Next
                                While rs2.Read

                                    damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))
                                    '   Response.Write(reasonname.Length)
                                    ' Array.Sort(reasonname)

                                    For j = 0 To reasonname.Length - 2
                                        Try

                                            ' Response.Write("<br>...." & rs2.Item("reason") & ".......<br>")
                                            If LCase(reasonname(j)).Trim = LCase(rs2.Item("reason")).ToString.Trim Then

                                                If damt > rs2.Item("bal") Then
                                                    damt = rs2.Item("bal")
                                                End If

                                                cellb(j) = damt.ToString

                                                cellbval(j) = rs2.Item("id").ToString

                                                sumloan(j) += CDbl(cellb(j))

                                            Else

                                                If cellb(j) > 0 Then
                                                Else
                                                    cellb(j) = "0.00"
                                                    cellbval(j) = "0"
                                                    sumloan(j) += CDbl(cellb(j))
                                                End If

                                            End If
                                        Catch ex As Exception
                                            Response.Write(ex.ToString & cell(1))
                                        End Try
                                    Next

                                End While

                            Else

                                For j = 0 To reasonname.Length - 2
                                    cellbval(j) = "0"
                                    cellb(j) = "0.00"
                                    sumloan(j) += CDbl(cellb(j))
                                Next
                            End If
                            rs2.Close()





                            ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            Dim penss As String
                            penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart <='" & pdate2 & "' order by id desc", Session("con"))
                            ' Response.Write(pdate2.Subtract(dhr).Days.ToString & "<br>")
                            ' Response.Write(penss.ToString & "<br>")
                            If pdate2.Subtract(dhr).Days >= 1 Then
                                cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                            Else
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If
                            Dim penss2 As String = ""
                            penss2 = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and pensstop<='" & pdate1 & "' order by id desc", Session("con"))

                            If penss <> "None" Then
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                                If penss2 <> "None" Then
                                    If CInt(penss2) > CInt(penss) Then
                                        cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                        cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                                    End If
                                End If
                            Else
                                If penss2 <> "None" Then

                                    cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                    cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                                End If
                            End If
                            ' Response.Write(cell(3).ToString & "===")
                            If color <> "#ccaa99" And cell(3) > 0 Then
                                If fired = 0 Then
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
                            If cell(2).ToString <> "0" And CDbl(cell(3)) > 0 Then
                                ' Response.Write(resing.ToShortDateString)
                                body &= "<tr "
                                If fired <> 0 Or resing.ToShortDateString <> "1/1/1900" Then
                                    body &= "style='background-color:red;'"
                                    Session("pstop") = True
                                End If
                                body &= "><td id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

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
                                            body &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                        Else
                                            body &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                        End If
                                    Else
                                        For j = 0 To reasonname.Length - 2
                                            If String.IsNullOrEmpty(cellbval(j)) = False Then
                                                body &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                            Else
                                                body &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
                                            End If
                                        Next
                                        If ccol = 0 Then
                                            body &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                        End If
                                    End If
                                Next
                                netincom = CDbl(cell(8)) - CDbl(cell(14))
                                If color <> "#ccaa99" Then
                                    sumnet += netincom
                                End If
                                ' Response.Write(netincom.ToString & "<----<br>")
                                body &= "<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"
                                'body &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_16'>" & getleaveinfobal(emptid, Session("con")) & "</td>"
                                body &= "<td  style='text-align:right;'>"
                                If paid.ToString = "None" And CDbl(cell(2)) <> 0 Then
                                    body &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' "
                                    If fired = 0 Then
                                        body &= "checked='checked'"

                                    End If
                                    body &= " onclick='javascript:sumcolx();'></td>"
                                Else
                                    If CDbl(cell(2)) <> 0 Then
                                        body &= " Paid</td>"

                                        '  body &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' style='visibility:hidden;'>Paid</td>"
                                    Else
                                        body &= " None</td>"

                                        'body &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' ></td>"

                                    End If


                                End If
                                body &= "<td style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If

                End While


                body &= "<tr style='text-weight:bold;text-align:right;'>" & _
            "<td class='cooo'>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
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
                    body &= ("<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='lon-" & j & "'>" & fm.numdigit(sumloan(j).ToString, 2) & "</td>")
                Next
                If ccol = 0 Then
                    body &= ("<td class='cooo' style='text-align:right;'>&nbsp;</td>")
                End If
                body &= "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
            "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
             "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
              "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
              "<td class='cooo' style='' colspan='2'>..&nbsp;" & _
 "</td></tr><tr id='result'><td>...</td></tr>"
                footer = fm.signpart()
                footer &= "</table>"
                outp = header & body & footer
                outp &= fm.projtrans(Request.Form("projname"), pdate1, Session("con")).ToString

                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)


            End If
            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
            '  Response.Write("End time:" & Now.Minute.ToString & ":" & Now.Second.ToString & ":" & Now.Millisecond.ToString)
            Return outp

        End If
        Return outp

    End Function
    Public Function makeform2_payroll(ByVal vx() As String) ' make statement
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
        Dim sec As New k_security
        ' Response.Write(Request.QueryString("month"))
        Dim rtnvalue As String
       
        Try
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
                rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))

                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM " & _
                                   "(SELECT distinct vwloanbal.reason as cnt FROM vwloanbal where vwloanbal.bal>0 and vwloanbal.emptid in" & _
                                       "(" & rtnvalue & ") and vwloanbal.dstart<='" & _
                                   pdate1 & "') AS derivedtbl_1", Session("con")))
                Dim pass As Integer = 0
                If ccol = 0 Then
                    ccol = 1
                Else
                    pass = 1
                End If
                ' Response.Write(ccol & "<br>")
                'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ReDim cellb(ccol)
                ReDim cellbval(ccol)
                ReDim sumloan(ccol)

                ReDim reasonname(ccol)
                'Response.Write(reasonname.Length & ">>><br>")
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
                    'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
                    ' rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                    Try
                        Dim rssqlnew As String
                       
                        rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                          "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                          "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                          "and emprec.id in(" & rtnvalue & ")" & _
                                                                          " ORDER BY emp_static_info.first_name,emprec.id desc "
                        If Session("payrolllist") <> "" Then
                            rssqlnew = Session("payrolllist")
                        End If
                        '  rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                        '                                                   "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                        '                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                        '                                                 "and emprec.id in(select emptid from emp_job_assign " & _
                        '                                                "where project_id='" & projid.ToString & "' " & _
                        '                                               "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                        '                                              " ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
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
                End If

                If rs.HasRows Then
                    outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                    outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                    outp &= "<td style='text-align:center;font-weight:bold;' colspan='20' >" & Session("company_name") & _
                    "<br> Project Name:"
                    If projid <> "" Then
                        outp &= spl(0).ToString
                    Else
                        outp &= "All Projects"
                    End If
                    outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "<br><span class='ref'></span></td>" & Chr(13)

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
                    'outp &= "<td class='fitx' rowspan='2'>Leave Bal</td>"

                    outp &= "<td rowspan='2' class='signpart'>Signature</td>"
                    If paid = "None" Then
                        ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                        rs2 = dbs.dtmake("dbpenx", "SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                            "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and ('" & pdate2 & _
                                            "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') or " & _
                                            "emp_job_assign.date_from between '" & pdate1 & "'  and isnull(emp_job_assign.date_end,'" & pdate2 & "') )and vwloanbal.dstart<='" & _
                                            pdate1 & "'", Session("con"))
                        'Response.Write(ccol)
                    Else
                        rs2 = dbs.dtmake("dbpenx", "SELECT distinct reason froM (SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                     "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate2 & _
                                     "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                     pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con"))
                      
                    End If
                    rs2 = dbs.dtmake("dbpenx", " SELECT distinct reason FROM vwloanbal where vwloanbal.bal>0 and vwloanbal.emptid in" & _
                                        "(" & rtnvalue & ") and vwloanbal.dstart<='" & _
                                    pdate1 & "'", Session("con"))
                   

                    outp &= "<tr>" & Chr(13)
                    If pass = 0 Then
                        outp &= "<td class='dedctx'>&nbsp;</td>"
                    Else
                        If rs2.HasRows Then
                            Dim i As Integer = 0
                            While rs2.Read
                                    '  Response.Write(rs2.Item("reason") & "<br>")
                                If rs2.Item("reason") = "-" Then
                                    outp &= "<td class='dedctx'>Other</td>"
                                Else
                                    outp &= "<td class='dedctx'>" & rs2.Item("reason").ToString & "</td>"
                                    End If
                                    ' Response.Write(reasonname.Length & "<br>")
                                If reasonname.Length > i Then
                                    reasonname(i) = rs2.Item("reason")
                                    End If
                                i += 1
                                End While
                            End If
                        End If

                    outp &= "</tr>" & Chr(13)
                    rs2.Close()
                    Dim k As Integer = 1
                    Dim color As String = ""
                    Dim resing As Date
                    Dim pstop As Boolean = False
                    While rs.Read
                        pstop = False
                            'check id in the list2 and make statement again 
                        emptid = rs.Item("id")
                        ' Response.Write(emptid)
                            'Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                        If fm.searcharray(vx, emptid.ToString) Then

                                ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                                End If

                            ' Response.Write(emptid & rs.Item("active"))
                            If (rs.Item("active") = "n") Then
                                    ' Response.Write(rs.Item("emp_id"))
                                resing = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))
                            Else

                            End If
                            ' Response.Write(resing)
                            Dim dateincr As Date
                            Dim incwhen As Integer = 0
                            avalue = ""
                            avalue = fm.getinfo2("select date_start from emp_sal_info where emptid=" & emptid & " order by id desc", Session("con"))
                            If avalue <> "None" Then
                                dateincr = avalue
                                End If
                            Dim dhr As Date
                            dhr = rs.Item("hire_date")
                            If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year Then
                                    'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                                incwhen = dateincr.Subtract(pdate1).Days
                                End If

                            If dhr.Month = pdate1.Month And dhr.Year = pdate1.Year Then

                                incwhen = 0
                                End If
                                ' Response.Write(resing & "====" & pdate1 & "=====" & pdate2)

                            If ((resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing) And incwhen = 0 Then 'question..........solved
                                    '  Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                                    ' Response.Write(Request.QueryString("chkpen") & "<br>")

                                If Request.Form("chkpensionstop") <> "" Then
                                    Session("pstop") = False

                                    If IsDate(resing) = True Then
                                        If resing <> "#1/1/1900#" Then
                                            pstop = True
                                                ' Response.Write("stoppppiiiinnnggg")
                                            End If

                                        End If
                                    End If
                                cell(0) = rs.Item("emp_id")
                                cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                                    'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                                    ' Response.Write(sql & "<br>")
                                sal = Me.getsalhrm(emptid, pdate1, Session("con"))
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
                                        fired = pdate2.Subtract(resing).Days + 1
                                    Else
                                        fired = 0
                                        End If

                                    End If
                                calc = nod - ca - clwp - newemp - fired
                                ' Response.Write(nod & "==>" & ca & "==>" & clwp & "++>" & newemp & "===>" & fired)
                                cell(3) = Math.Round(calc, 2).ToString
                                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                                cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                                    'Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)

                                    ' cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                                    '  Response.Write(cell(5).ToString)
                                    ' cell(5) = "0"
                                Dim allz() As String
                                allz = getallowancexp(emptid, "y", pdate1, calc, Session("con"))
                                    ' Response.Write(allz(2).ToString & "<br>")
                                cell(5) = allz(0)
                                If cell(5).ToString = "None" Then
                                    cell(5) = "0"
                                ElseIf cell(5).ToString = "" Then
                                    cell(5) = "0"
                                    End If

                                If CDbl(cell(5)) > 0 Then
                                        ' cell(5) = ((CDbl(cell(5)) / nod) * calc).ToString
                                        ' Response.Write(cell(5))
                                        'cell(5) = (CDbl(cell(5)) / nod) * (nod - fired)
                                    End If
                                    'cell(6) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='n') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                                    'Response.Write(cell(6) & "<br>")
                                    ' cell(6) = "0"
                                    ' Dim allz() As String
                                allz = getallowancexp(emptid, "n", pdate1, calc, Session("con"))
                                    ' Response.Write(allz(2).ToString & "<br>")
                                cell(6) = allz(0)
                                If cell(6).ToString = "None" Then
                                    cell(6) = "0"
                                ElseIf IsNumeric(cell(6)) = False Then
                                    cell(6) = "0"
                                    End If
                                If CDbl(cell(6)) > 0 Then
                                        'cell(6) = (CDbl(cell(6)) / nod) * calc
                                        ' cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                                    End If
                                cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                                    ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                                    '   Response.Write(cell(7).ToString & "<br>")
                                cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                                    'Response.Write(cell(8).ToString & "<br>")
                                cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                                cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString


                                If projid <> "" Then
                                        'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                        'Response.Write(sql)
                                    sql = ""
                                    rs2 = dbs.dtmake("loan", "select distinct id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' ) order by id", Session("con"))

                                Else
                                    rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                    End If
                                For j = 0 To reasonname.Length - 2
                                    cellb(j) = "0"
                                Next
                                If rs2.HasRows Then
                                    Dim emp1() As String = {""}
                                    Dim list() As String = {""}
                                    For j = 0 To reasonname.Length - 2
                                        cellb(j) = 0
                                    Next
                                    While rs2.Read

                                        damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))
                                            '   Response.Write(reasonname.Length)
                                            ' Array.Sort(reasonname)

                                        For j = 0 To reasonname.Length - 2

                                            If LCase(reasonname(j)).Trim = LCase(rs2.Item("reason")).ToString.Trim Then

                                                If damt > rs2.Item("bal") Then
                                                    damt = rs2.Item("bal")
                                                    End If

                                                cellb(j) = damt.ToString

                                                cellbval(j) = rs2.Item("id").ToString

                                                sumloan(j) += CDbl(cellb(j))

                                            Else

                                                If cellb(j) > 0 Then
                                                Else
                                                    cellb(j) = "0.00"
                                                    cellbval(j) = "0"
                                                    sumloan(j) += CDbl(cellb(j))
                                                    End If

                                                End If
                                        Next

                                        End While

                                Else

                                    For j = 0 To reasonname.Length - 2
                                        cellbval(j) = "0"
                                        cellb(j) = "0.00"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next
                                    End If
                                rs2.Close()




                                    ' Dim dhr As Date
                                dhr = rs.Item("hire_date")
                                    ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                Dim penss As String
                                penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", Session("con"))
                                    ' Response.Write(pdate2.Subtract(dhr).Days.ToString & "<br>")
                                    'Response.Write(penss.ToString & "<br>")
                                If pdate2.Subtract(dhr).Days >= 1 Then
                                    cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                    cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                                Else
                                    cell(12) = "0.00"
                                    cell(13) = "0.00"
                                    End If
                                Dim penss2 As String
                                penss2 = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and pensstop<='" & pdate1 & "' order by id desc", Session("con"))
                                If pstop = True Then
                                        ' Response.Write("ssssssssssttttopppppp")
                                    penss2 = "None"
                                    cell(12) = "0.00"
                                    cell(13) = "0.00"
                                    End If
                                If penss <> "None" Then
                                    cell(12) = "0.00"
                                    cell(13) = "0.00"
                                    If penss2 <> "None" Then
                                        If CInt(penss2) > CInt(penss) Then
                                            cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                            cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                                            End If
                                        End If
                                Else
                                    If penss2 <> "None" Then

                                        cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                        cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                                    Else

                                        End If
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
                                        'outp &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_16'>" & getleaveinfobal(emptid, Session("con")) & "</td>" & Chr(13)

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
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & Chr(13)
                        '  outp &= "<td  class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='lv'>..</td>" & Chr(13) & _
                    outp &= "<td class='cooo' style='' colspan='2'>..&nbsp;" & _
         "</td></tr>"
                        'outp &= "</table><br>"
                        'outp &= "<table cellspacing='0' cellpadding='0' width='800px' style='font-size:9.5pt;'>"
                    outp &= fm.signpart()
                    outp &= "</table>" & Chr(13)
                        ' outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

                    rs.Close()
                    fm = Nothing
                    dbs = Nothing
                    Response.Write(outp)
                End If

                Return outp



            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Function

    Public Function makeformpx_payroll()
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
        'Dim sec As New k_security
        Dim outp As String = ""


        Dim damt As Double = 0
        If Request.QueryString("prid") <> "" Then
            'Response.Write(Request.QueryString("prid"))
        Else
            ' Response.Write(Request.QueryString("month"))
            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                ' Response.Write(Request.QueryString("paidst"))
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
                    paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in(select pr from payrollx where ref_inc is null or ref_inc='0')", Session("con"))
                    If paid = "None" Then
                        Response.Write("There is No Payroll list")
                    Else
                    rrr = dbs.dtmake("payrol", "select distinct ref,date_paid,bank from payrollx where pr='" & paid & "' and remark='monthly'", Session("con"))
                        ' Response.Write("select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='monthly'")
                    Dim fx() As String = {""}
                    If rrr.HasRows Then
                        Response.Write("<div id='viewlistx'><b>Project: " & projename & "<br>Payroll in the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</b><table>")
                        Dim ccout As String = "0"
                        While rrr.Read
                            ' Response.Write(rrr.Item(0))
                            ' Response.Write(projid)
                            If projid.ToString <> "" Then
                                'Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')", Session("con")))
                                ccout = "0"
                                ccout = fm.getinfo2("select count(id) from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                                Dim emid, rtnvalue, eml() As String
                                rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                                ' Response.Write(rrr.Item("ref"))
                                emid = fm.getinfo2("select emptid from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                                'Response.Write(emid & "===" & rtnvalue & "----" & rrr.Item("ref") & pdate2.ToShortDateString & "<br>")
                                eml = rtnvalue.Split(",")
                                If fm.searcharray(eml, "'" & emid & "'") Then
                                    ' Response.Write(rrr.Item("ref"))
                                    Response.Write("<tr><td class='listcont'>" & rrr.Item(0) & "<br><span style='color:gray;font-size:10pt;'>(No. List:" & ccout & ")</span>" & _
                                   "</td><td class='v1'><div class='deletepayrol'")

                                    If String.IsNullOrEmpty(Session("right")) = False Then
                                        fx = Session("right").split(";")
                                        ReDim Preserve fx(UBound(fx) + 1)
                                        fx(UBound(fx) - 1) = ""
                                    End If
                                    If fm.searcharray(fx, "1") Or fm.searcharray(fx, "9") Then
                                        Response.Write(" onclick = " & Chr(34) & _
                                    "javascript:gotocheckp('del','" & rrr.Item(0) & "');" & Chr(34))
                                    End If
                                    Response.Write(" ></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='viewpayrol' onclick=" & _
                                    Chr(34) & "javascript:gotocheckp('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='viewdelpayrol' onclick=" & Chr(34) & _
                                    "javascript:gotocheckp('viewdel','?prdel=" & rrr.Item(0) & "&pd=" & rrr.Item(1) & "');" & _
                                    Chr(34) & "></div></td><td> &nbsp;|&nbsp;</td><td class='v1'><div class='editpayrol' onclick=" & Chr(34) & _
                                    "javascript:gotocheckp('Edit','" & rrr.Item(0) & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'>.." & rrr.Item("bank") & "<div class='bankpayrol' onclick=" & Chr(34) & _
                                    "javascript:gotocheckp('Bank','?ref=" & rrr.Item(0) & _
                                    "&ppd=" & pdate1 & "&bname=" & rrr.Item("bank") & "');" & Chr(34) & _
                                    "></div></td></tr><tr><td colspan='8'><hr style='width:600px;' align=left></td></tr>")
                                End If
                            End If
                           
                        End While
                        Response.Write("</table></div>")
                  
                    End If
                        'Response.Write(paid)
                    End If
                End If
            End If
    End Function

    Public Function makeform()

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
            Dim ccol2 As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If
            paid = ""
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select payroll_id from paryrol where ref_inc is null or ref_inc=0)", Session("con"))
            If paid = "None" Then
                ' ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vw_loan WHERE  (dstart <= '" & pdate1 & "') and ref='0' GROUP BY reason) AS derivedtbl_1", Session("con")))
                'Response.Write("ref=0")
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

            Else
                'Response.Write(paid)
                If IsNumeric(paid) = False Then
                    paid = "0"
                End If

                ' ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') and ref='" & paid & "' GROUP BY reason) AS derivedtbl_1", Session("con")))
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

                ' ccol2 = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vw_loan WHERE Bal > 0 AND ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                '  Response.Write("ref=0")
            End If
            If ccol = 0 Then
                ccol = 1
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
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                ' rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                Try
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                   "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                   "and emprec.id in(select emptid from emp_job_assign " & _
                                                   "where project_id='" & projid.ToString & "' " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
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
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))

                End Try
            End If
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                outp &= "<td class='toptitle' style='text-align:center;font-weight:bold;' colspan='19' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

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

                rs2 = dbs.dtmake("dbpen", "SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                   "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                   "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                   pdate1 & "'  and emp_job_assign.project_id='" & projid & "'", Session("con"))


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
                                outp &= "<td  class='dedctx'>" & rs2.Item("reason").ToString & "</td>"
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
                Dim did As String
                Dim loanid As String = ""
                Dim otid As String = ""

                While rs.Read
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    emptid = rs.Item("id")
                    paid = ""
                    paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select payroll_id from paryrol where emptid='" & emptid & "' and (ref_inc='0' or ref_inc is null))", Session("con"))
                    '  Response.Write(paid.ToString & "<br>...")
                    If paid.ToString <> "None" Then
                        paidlist &= fm.getinfo2("select id from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "') and (ref_inc='0' or ref_inc IS NULL)", Session("con")) & ","
                        '  Response.Write(paidlist & "<br>")
                        resing = "#1/1/1900#"
                        If color <> "#aabbcc" Then
                            color = "#aabbcc"
                        Else
                            color = "white"
                        End If

                        did = ""
                        If (rs.Item("active") = "n") Then
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "' order by id desc", Session("con"))

                        End If
                        If did <> "None" And did <> "" Then
                            resing = did
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


                        If ((resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing) And incwhen = 0 Then 'question..........solved
                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            ' sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            'Response.Write(sql & "<br>")
                            sal = dbs.getsal(emptid, pdate1, Session("con"))
                            cell(2) = sal(0)
                            If cell(2) = "Sorry This employee salary info is not setted!" Then
                                cell(2) = "0"
                                '  color = "#ccaa99"
                            End If
                            'Response.Write(paid)
                            ca = fm.catt(emptid, Session("con"), pdate1, pdate2)
                            'Response.Write(ca.ToString & "<==a")
                            clwp = Math.Round(CDbl(fm.lwpinmonth(pdate1, pdate2, emptid, Session("con"))), 2)
                            'Response.Write("lwp==>" & clwp.ToString & "<br>")
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
                            '  cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
                            Dim allz() As String
                            allz = getallowancex(emptid, "y", pdate1, calc, Session("con"))
                            ' Response.Write(allz(2).ToString & "<br>")
                            cell(5) = allz(0)
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            ElseIf IsNumeric(cell(5)) = False Then
                                Response.Write(cell(5).ToString & "<br>")
                                cell(5) = "0"

                            End If

                            If CDbl(cell(5)) > 0 Then
                                ' cell(5) = ((CDbl(cell(5)) / nod) * calc).ToString
                                ' Response.Write(cell(5))
                                'cell(5) = (CDbl(cell(5)) / nod) * (nod - fired)
                            End If
                            ' cell(6) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='n') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            'Response.Write(cell(6) & "<br>")
                            ' cell(6) = "0"

                            allz = getallowancex(emptid, "n", pdate1, calc, Session("con"))
                            ' Response.Write(allz(2).ToString & "<br>")
                            cell(6) = allz(0)
                            If cell(6).ToString = "None" Then
                                cell(6) = "0"
                            ElseIf IsNumeric(cell(6)) = False Then
                                cell(6) = "0"
                            End If
                            If CDbl(cell(6)) > 0 Then
                                'cell(6) = (CDbl(cell(6)) / nod) * calc
                                ' cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                            End If
                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString

                            'otid &= fm.getinfo2("select id from emp_ot where emptid=" & emptid & " and ot_date='" & pdate2 & "' and paidstatus='y'", Session("con")) & ","
                            rrr = dbs.dtmake("sss", "select id from emp_ot where emptid=" & emptid & " and ot_date='" & pdate2 & "' and paidstatus='y'", Session("con"))
                            If rrr.HasRows Then
                                While rrr.Read
                                    otid &= rrr.Item("id") & ","
                                End While
                            End If
                            rrr.Close()
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString

                            ' Response.Write(cell(10) & "<br>")
                            If paid = "None" Then
                                If projid <> "" Then
                                    'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                    'Response.Write(sql)
                                    sql = ""
                                    rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')) order by id", Session("con"))
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
                                'Response.Write("select id,no_month_settle as nopay,amt,reason from emp_loan_req where deduction_starts<br> < ='" & pdate1 & "' and emptid=" & emptid.ToString & " and id in<br>(select loan_no from emp_loan_settlement where ref=" & paid.ToString & ")<br>")
                                rs2 = dbs.dtmake("loan", "select id,no_month_settle as nopay,amt,reason from emp_loan_req where deduction_starts < ='" & pdate1 & "' and emptid=" & emptid.ToString & " and id in(select loan_no from emp_loan_settlement where ref='" & paid.ToString & "')", Session("con"))
                                ' If projid <> "" Then
                                ' sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"
                                ' Response.Write(sql)
                                ' sql = ""
                                '  rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) order by id", Session("con"))
                                'Else
                                ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                ' End If
                                If rs2.HasRows Then
                                    j = 0

                                    While rs2.Read
                                        loanid &= fm.getinfo2("select id from emp_loan_settlement where loan_no=" & rs2.Item("id") & " and date_payment='" & pdate2 & "' and ref='" & paid.ToString, Session("con")) & "',"

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
                            Dim penss As String
                            penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", Session("con"))
                            ' Response.Write(penss & "<br>")
                            If pdate2.Subtract(dhr).Days >= 1 Then
                                cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                            Else
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If
                            If penss <> "None" Then
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If
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
                        End If
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
                outp &= "</table>"
                rs.Close()
                outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
                Response.Write("<input type=hidden id='delpaid' name='delpaid' value='" & paidlist & "⌡" & loanid & "⌡" & otid & "'>")
                ' Response.Write("payrol no. list:" & paidlist & "<br>loan settled list: " & loanid & "<br>OT List:" & otid)
            End If
            'Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))


            Return outp



        End If
    End Function

    Public Function makeformpaidx_payroll() 'View
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

    Public Function makeformpaidxdel_payroll() 'view for del
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
        Dim sql As String
        Dim outp As String = ""
        Dim sec As New k_security
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0

        If Request.QueryString("prdel") <> "" Then
            'Response.Write(fl.msgboxt("onfram", "Progression", "Progression shown"))
            'Response.Write("<script>showobj('progressbar');</script>")
            ref = Request.QueryString("prdel")
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
                ' Response.Write("SELECT COUNT(cnt) AS Expr1 froM (SELECT distinct reason AS cnt FROM vwloanref WHERE  ref='" & ref & "')  AS derivedtbl_1")
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

            End If
            outp = "Sorry Can't Process"
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                outp &= "<td class='toptitle' style='text-align:center;font-weight:bold;' colspan='19' >" & Session("company_name") & _
                "<br> Project Name:"
                'rs.Read()
                outp &= sec.dbHexToStr(Request.QueryString("projname"))
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & _
                "<br>" & ref & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2'>Del</td>"
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
                    outp &= "<td ></td>"
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


                outp &= "</tr>" & Chr(13)
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
                        'paidlist &= fm.getinfo2("select id from paryrollx where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "') and (ref_inc='0' or ref_inc IS NULL)", Session("con")) & ","
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
                        'Response.Write(cell(6) & "<br>")
                        ' cell(6) = "0"

                        cell(7) = fm.getinfo2("select ot from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))

                        'otid &= fm.getinfo2("select id from emp_ot where emptid=" & emptid & " and ot_date='" & pdate2 & "' and paidstatus='y'", Session("con")) & ","

                        cell(8) = fm.getinfo2("select gross_earnings from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                        'Response.Write(cell(8).ToString & "<br>")
                        cell(9) = fm.getinfo2("select txinco from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                        cell(10) = fm.getinfo2("select tax from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                        ' Response.Write(cell(10) & "<br>")
                        rs2.Close()
                        j = 0
                        If fm.getinfo2("select ref from emp_loan_settlement where ref='" & ref & "'", Session("con")) <> "None" Then
                            rs2 = dbs.dtmake("loan", "select id,loan_no,reason from vwloanref where ref='" & ref & "' and emptid=" & emptid & " order by reason", Session("con"))

                            If rs2.HasRows Then
                                j = 0
                                While rs2.Read
                                    'Response.Write(rs2.Item("loan_no") & "=" & emptid & "<br>")
                                    If reasonname(j Mod ccol) = rs2.Item("reason") Then
                                        cellb(j) = fm.getinfo2("select amount from emp_loan_settlement where id='" & rs2.Item("id") & "'", Session("con"))
                                        cellbval(j) = "0"
                                        sumloan(j) += CDbl(cellb(j))
                                        j += 1




                                    ElseIf reasonname(j Mod ccol) <> rs2.Item("reason") Then
                                        cellb(j) = "0.00"
                                        cellbval(j) = "0"
                                        sumloan(j) += CDbl(cellb(j))
                                        j += 1
                                    End If



                                End While
                            Else
                                cellb(j) = "0.00"
                                cellbval(j) = "0"
                                sumloan(j) += CDbl(cellb(j))
                                j += 1
                            End If
                        Else
                            For j = 0 To ccol - 1
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
                        outp &= "<tr><td><span class='iddel' onclick=" & Chr(34) & "javascript:delsingle('emptid=" & emptid & "&ref=" & ref & "');" & Chr(34) & ">del</span></td><td id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

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
                "<td class='cooo'>&nbsp;</td><td class='cooo'>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
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
                outp &= "</table>"
                rs.Close()
                outp &= fm.projtrans(sec.dbHexToStr(Request.QueryString("projname")), pdate1, Session("con"))

                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
                ' Response.Write("<input type=hidden id='delpaid' name='delpaid' value='" & paidlist & "⌡" & loanid & "⌡" & otid & "'>")
                ' Response.Write("payrol no. list:" & paidlist & "<br>loan settled list: " & loanid & "<br>OT List:" & otid)
            End If
            'Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))






        End If
        Return outp
    End Function
    Public Function getids_payroll()
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
    Function deleteallx_payroll(ByVal ref As String)
        ' Response.Write(ref & " gonna delete")
        Dim dbs As New dbclass
        Dim flg As String = ""
        Dim sqlst As String = "BEGIN TRANSACTION" & Chr(13)


        sqlst &= "delete from payrollx where ref='" & ref & "'" & Chr(13)

        sqlst &= "delete from emp_loan_settlement where ref='" & ref & "'" & Chr(13)

        sqlst &= "Update emp_ot set paidstatus='n',ref=NULL where ref='" & ref & "'" & Chr(13)
        sqlst &= "Update emp_inc set paidref=NULL where paidref='" & ref & "'" & Chr(13)
        sqlst &= "COMMIT" & Chr(13)
        ' Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
        flg = dbs.excutes(sqlst, Session("con"), Session("path"))
        ' Response.Write(flg.ToString)
        If IsNumeric(flg) = True Then
            ' Response.Write(flg)
            If CInt(flg) <= 0 Then
                dbs.excutes("RollBack", Session("con"), Session("path"))
                Response.Write("data is not saved")
            Else
                'dbs.save("COMMIT", Session("con"),session("path"))
                File.AppendAllText(Session("path") & "/log/dellog.txt", sqlst & "==>" & Session("emp_iid") & "=>" & Now.ToString & Chr(13))
                Response.Write("Data Saved")
            End If



        End If

        dbs = Nothing
        Return Nothing
    End Function
    Public Function projtrans_payroll(ByVal proj As String, ByVal d1 As Date)
        Dim rt(2) As String
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim spl() As String
        Dim nod As Integer
        Dim cc As Integer = 1
        rt(0) = ""
        nod = Date.DaysInMonth(Year(d1), Month(d1))
        spl = proj.Split("|")
        rs = dbx.dtmake("trans", "select * from emp_job_assign where date_end between '" & d1.ToShortDateString & "' and '" & d1.AddDays(nod - 1) & "'", Session("con"))
        If rs.HasRows = True Then
            While rs.Read
                'Response.Write(proj & "=-==" & fm.getinfo2("select project_name from tblproject where project_id='" & rs.Item("project_id") & "'", Session("con")) & "<br>")
                If spl(0) = fm.getinfo2("select project_name from tblproject where project_id='" & rs.Item("project_id") & "'", Session("con")) Then
                    rt(0) = cc.ToString & ". " & fm.getfullname(rs.Item("emp_id"), Session("con")) & " has transfer from " & proj & " to " & fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where date_from between '" & d1.ToShortDateString & "' and '" & d1.AddDays(nod - 1) & "'", Session("con")) & "'", Session("con")) & "<br>"
                    cc += 1
                End If

            End While
        End If
        rs.Close()


        Return rt(0)
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dbs As New dbclass
        ' dbs.pathserver = Server.MapPath(" ")
        ' Response.Write(dbs.pathserver.ToString)
        Session.Timeout = 60
        ' Dim rtn As Object
        '  rtn = dbs.Backup(Session("con"), dbs.pathserver & "/app_data/hrmp.mdf")
        '  Response.Write(rtn.ToString)
        dbs = Nothing
        If Session("emp_iid") = "" Then
            Response.Redirect("login.aspx?msg=session expired")
        End If
        If Session("con").state = Data.ConnectionState.Closed Then
            'Response.Write("database is closed")
            Session("con").open()

        End If
    End Sub
    Function getallowancex(ByVal emptid As String, ByVal altype As String, ByVal pdate1 As Date, ByVal calc As Double, ByVal con As SqlConnection)
        Dim nod As Integer
        Dim rtn(3) As String
        Dim rpt As String = ""
        Dim fm As New formMaker
        Dim ca, clwp As Double
        Dim d2, d3 As Date
        Dim amt As Double
        nod = Date.DaysInMonth(pdate1.Year, pdate1.Month)
        d2 = pdate1.Month & "/" & nod.ToString & "/" & pdate1.Year
        Dim dvar1, dvar2 As Integer
        Dim rt As String = ""
        Dim rs As DataTableReader
        Dim dbx As New dbclass
        amt = 0
        rpt = ""
        'Response.Write("<br>.." & emptid)
        rs = dbx.dtmake(altype, "select * from emp_alloance_rec where to_date between '" & pdate1 & "' and '" & d2 & "' and istaxable='" & altype & "' and emptid='" & emptid & "'", con)
        If rs.HasRows Then
            'rpt &= "has rows"
            While rs.Read
                dvar1 = rs.Item("to_date").Subtract(pdate1).days + 1

                ' Response.Write(dvar1.ToString)
               If dvar1 > 0 Then
                    rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allownace_type") & " has been closed on:" & rs.Item("from_date") & " and emptid='" & emptid & "'<br>"
                    amt += (CDbl(rs.Item("amount")) / nod) * dvar1
                End If
            End While
        End If

        rs.Close()
        ' Response.Write(rpt)

        Dim skip As Boolean = False
        rs = dbx.dtmake(altype, "select * from emp_alloance_rec where  (from_date between '" & pdate1 & "' and isnull(to_date,'" & d2 & "') or '" & pdate1 & "' between from_date and isnull(to_date,'" & d2 & "')) and  (emptid=" & emptid & ") and (istaxable='" & altype & "')", con)
        ' Response.Write("select * from emp_alloance_rec where to_date is null and (from_date between '" & pdate1 & "' and isnull(to_date,'" & d2 & "') or '" & pdate1 & "' between from_date and isnull(to_date,'" & d2 & "')) and  (emptid=" & emptid & ") and (istaxable='" & altype & "')")
        If rs.HasRows Then
            While rs.Read
                ' Response.Write("<br>" & rs.Item("from_date") & " ==" & rs.Item("to_date") & "===>" & rs.Item("amount"))
                ' Response.Write("<br>" & IsDBNull(7).ToString)
                If IsDBNull(7) = False Then
                    If rs.Item(7).ToString <> "" Then
                        If CDate(rs.Item("to_date")).Month = pdate1.Month And CDate(rs.Item("to_date")).Year = pdate1.Year Then
                            skip = True
                        End If
                    End If
                End If
                If skip = False Then
                    dvar1 = CDate(rs.Item("from_date")).Subtract(d2).Days
                    ca = CDbl(fm.getinfo2("select count(id) from emp_att where emptid=" & rs.Item("id") & " and att_date between '" & pdate1 & "' and '" & d2 & "' and status='A'", Session("con")))
                    ' Response.Write(ca.ToString & "<==a")
                    clwp = Math.Round(CDbl(fm.lwpinmonth(pdate1, d2, emptid, Session("con"))), 2)
                    If CDate(rs.Item("from_date")).Month = pdate1.Month And CDate(rs.Item("from_date")).Year = pdate1.Year Then
                        'if some one resign should be consider here......................
                        If dvar1 < 0 Then
                            dvar1 = (dvar1 * -1) + 1

                            dvar2 = dvar1 - ca - clwp

                        End If
                        rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allownace_type") & " has been started on:" & rs.Item("from_date") & " and emptid='" & emptid & "'<br>"
                        'Response.Write(dvar1.ToString)
                    Else
                        dvar1 = nod
                        dvar2 = nod - ca - clwp
                    End If

                    If dvar1 > 0 Then
                        amt += (CDbl(rs.Item("amount")) / nod) * dvar2
                        ' Response.Write(dvar2.ToString)

                    End If
                End If
            End While
        End If

        rs.Close()

        dbx = Nothing
        ' Response.Write(amt)
        rtn(0) = amt
        rtn(1) = rpt
        rtn(2) = dvar1
        Return rtn

    End Function
    Function getallowancexp(ByVal emptid As String, ByVal altype As String, ByVal pdate1 As Date, ByVal calc As Double, ByVal con As SqlConnection)
       
        Dim nod As Integer
        Dim rtn(3) As String
        Dim rpt As String = ""
        Dim fm As New formMaker
        Dim ca, clwp As Double
        Dim pdate2, ds, de As Date
        Dim amt As Double
        Dim sumall As Double
        Dim did As String
        Dim sql As String = ""
        nod = Date.DaysInMonth(pdate1.Year, pdate1.Month)
        pdate2 = pdate1.Month & "/" & nod.ToString & "/" & pdate1.Year
        Dim dvar1 As Integer
        Dim rt As String = ""
        Dim rs As DataTableReader
        Dim dbx As New dbclass
        amt = 0
        rpt = ""
        Try

            sql = "select * from emp_alloance_rec where istaxable='" & altype & "' and emptid='" & emptid & "'"
            rs = dbx.dtmake(altype, sql, con)
            sql = ""
            If rs.HasRows Then
                sumall = 0
                Dim skip As Boolean = False
                While rs.Read
                    ds = "#1/1/1900#"
                    de = "#1/1/1900#"
                    skip = False
                    If rs.IsDBNull(7) Then
                        If rs.Item("from_date") <= pdate1 Then
                            ds = pdate1
                        Else
                            ds = rs.Item("from_date")
                        End If
                        de = pdate2.AddDays(1)
                    Else
                        If rs.Item("to_date") < pdate1 Then
                            skip = True
                        Else
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "' order by id desc", con)
                            If IsDate(did) Then
                                If pdate1.Month = CDate(did).Month And pdate1.Year = CDate(did).Year Then
                                    de = CDate(did)
                                    If CDate(rs.Item("from_date")) <= pdate1 Then
                                        ds = pdate1
                                    Else
                                        If CDate(rs.Item("from_date")) <= pdate2 Then
                                            ds = rs.Item("from_date")
                                        End If
                                    End If
                                Else
                                    If CDate(rs.Item("from_date")) <= pdate1 Then
                                        ds = pdate1
                                    ElseIf CDate(rs.Item("from_date")) <= pdate2 Then
                                        ds = rs.Item("from_date")
                                    End If
                                    If CDate(rs.Item("to_date")) <= pdate2 Then
                                        de = CDate(rs.Item("to_date")).AddDays(1)
                                    Else
                                        de = pdate2.AddDays(1)
                                    End If
                                End If
                            Else
                                If CDate(rs.Item("from_date")) <= pdate1 Then
                                    ds = pdate1
                                ElseIf CDate(rs.Item("from_date")) <= pdate2 Then
                                    ds = rs.Item("from_date")
                                End If
                                If CDate(rs.Item("to_date")) <= pdate2 Then
                                    de = CDate(rs.Item("to_date")).AddDays(1)
                                Else
                                    de = pdate2.AddDays(1)
                                End If
                            End If
                        End If
                    End If
                    If skip = False Then
                        ca = CDbl(fm.getinfo2("select count(id) from emp_att where emptid=" & emptid & " and att_date between '" & ds & "' and '" & de & "' and status='A'", Session("con")))
                        
                        If IsNumeric(ca) = False Then
                            ca = 0
                        End If
                        clwp = Math.Round(CDbl(fm.lwpinmonth(ds, de, emptid, Session("con"))), 2)
                        If IsNumeric(clwp) = False Then
                            clwp = 0
                        End If
                        If de = ds Then
                            ' Response.Write("error date calculation" & emptid)
                        Else
                            dvar1 = 0
                            If de > ds Then
                                dvar1 = de.Subtract(ds).Days
                                dvar1 = dvar1 - ca - clwp
                            Else
                                dvar1 = 0
                            End If
                            sumall += (rs.Item("amount") / nod) * dvar1
                        End If
                    End If
                    If dvar1 > 0 Then
                        rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allownace_type") & " has been started on:" & rs.Item("from_date") & " and end on:" & de.ToString & " Absent: " & ca.ToString & " Lwp: " & clwp.ToString & " Total Date: " & dvar1 & " emptid='" & emptid & "'<br>"
                    End If
                End While
                rs.Close()
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try
        dbx = Nothing
        rtn(0) = sumall
        rtn(1) = rpt
        rtn(2) = dvar1
        Return rtn

    End Function
    Function getsalhrm(ByVal emptid As Integer, ByVal d1 As Date, ByVal conx As SqlConnection)
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
    Public Function getprojemp(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim did As String
        ' rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "'>=date_from and '" & sdate & "'<= isnull(date_end,'" & sdate & "')  order by id", con)
        Dim d1, d2, de, ds As Date
        '  Response.Write("<br>select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "'>=date_from and '" & sdate & "'<= isnull(date_end,'" & sdate & "')  order by id")
        d2 = Nothing
        d1 = "#" & sdate.Month.ToString & "/1/" & sdate.Year.ToString & "#"
        rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "'>=date_from and '" & d1 & "'<= isnull(date_end,'" & sdate & "')  order by id", con)

        Dim listid() As String = {""}
        Dim rtn As String = ""
        If rs.HasRows Then
            ' While rs.Read
            '   Response.Write("'" & rs.Item("emptid") & "',")
            'End While
            'Response.Write("<br>")
            'rs.Close()
            ' rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "'  order by id", con)

            Try
                'Response.Write(" start    ====     requested     ====    Date end<br>")
                While rs.Read
                    ds = "#1/1/1900#"
                    de = "#1/1/1900#"
                    ' ds = rs.Item("date_from")
                    ' Response.Write(rs.Item("emptid") & "<br>")
                    If rs.IsDBNull(3) Then
                        'ds = sdate

                        ds = rs.Item("date_from")
                        de = sdate

                    Else
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)
                        If IsDate(did) Then
                            If rs.Item("emptid") = 620 Then
                                ' Response.Write(did)
                            End If
                            If CDate(rs.Item("date_end")) <> sdate Then
                                If CDate(did).Month = sdate.Month And CDate(did).Year = sdate.Year Then
                                    de = sdate
                                    ds = rs.Item("date_from")
                                Else
                                    de = rs.Item("date_end")
                                    ds = rs.Item("date_from")
                                End If

                            Else
                                de = sdate
                                ds = rs.Item("date_from")

                            End If
                        Else
                            de = rs.Item("date_end")
                            ds = rs.Item("date_from")



                        End If

                        '  Response.Write(ds.ToShortDateString)
                        ' Response.Write("     ====     ")
                        'Response.Write(sdate.ToShortDateString)
                        'Response.Write("     ====      ")

                        '                    Response.Write(de.ToShortDateString)
                        '                    Response.Write("  ====         " & rs.Item("emptid") & fm.getfullname(rs.Item("emp_id"), Session("con")) & "<br>")
                    End If
                    If ds <= sdate And sdate <= de Then
                        rtn &= "'" & rs.Item("emptid") & "',"

                    Else
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
        'Response.Write(rtn & "<br>")
        Dim sp() As String = rtn.Split(",")
        'Response.Write(sp.Length)

        Return rtn
    End Function
    Function checkused_p(ByVal ref As String, ByVal d1 As Date, ByVal emi() As String, ByVal pd As String)
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim id As String
        Dim fm As New formMaker
        id = fm.getinfo2("select id from payrollx where ref='" & ref & "'", Session("con"))
        Dim c As Integer

    End Function

    Public Function getleaveinfobal(ByVal emptid As Integer, ByVal con As SqlConnection)
        Dim output As String = ""
    
        Dim bgtid As Integer
        Dim dbs As New dbclass
        'Dim rqid As Integer
        Dim bgtno As Double = 0
        Dim ndav As Double = 0
        Dim used As Double = 0
        Dim tused As Double = 0
        Dim tndav As Double = 0
        Dim dt As DataTableReader
        Dim color, ref, rmk As String
        Dim fm As New formMaker
        Dim i As Integer
        color = "blue"
        Dim dt2 As DataTableReader
        Dim amt As Double = 0

        Dim otp As String = ""
        'Dim emptid As Integer
        ' Dim fm As New formMaker
        Try
            dt = dbs.dtmake("leavesee", "select * from show_leave_bal where emptid=" & emptid & " ORDER BY ID", con)


            If dt.HasRows Then
              
                tndav = 0
                Dim url As String = ""
                Dim sttl As String
                While dt.Read
                    used = 0
                    ndav = 0
                    sttl = 0
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fdafda"
                    End If
                    If fm.isexp(Today.ToShortDateString, dt.Item("Year End"), 2, "y") Then
                        color = "red"
                    End If


                    bgtid = dt.Item("id")
                    'sttl = (fm.getinfo2("select bal from leav_settled where bgtid=" & bgtid, con))
                    If Not IsNumeric(sttl) Then
                        sttl = "0"
                    End If

                    Dim isdr As String = fm.getinfo2("select resign_date from emp_resign where emptid=" & emptid, con)
                    If IsDate(isdr) Then
                        ndav = fm.showavdate_rev(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"), isdr)
                    Else
                        ndav = fm.showavdate_rev(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"), "")
                    End If
                    'ndav = fm.showavdate(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"))
                   


                    If color <> "red" Then
                        output = FormatNumber(ndav, 2) - CDbl(dt.Item(6))
                        tndav += output
                    End If

                
                End While

            End If
            'Response.Write(output)
            ' Return output
        Catch ex As Exception
            
        End Try
        dt = Nothing
        dbs = Nothing

        Return CInt(tndav).ToString
    End Function

    Public Function makeformunpaid_payroll_new() 'unpaid list
        Dim mx As New mail_system
        Dim fm As New formMaker
        Dim sec As New k_security
        Session("payrolllist") = ""
        'Response.Write("Start time:" & Now.Minute.ToString & ":" & Now.Second.ToString & ":" & Now.Millisecond.ToString)
        Session("companyck") = "Net Consult P.L.C"

        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        If Session("company_name") <> Session("companyck") Then
            Response.Redirect("logout.aspx?msg=copyright issue&msgtype=Caution&titlex=Caution")

        End If
        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String
        Dim header, body, footer As String
        Dim sum(15) As Double
        Dim avalue As String = ""
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
        ' Dim fm As New formMaker
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


            ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM " & _
                                    "(SELECT distinct vwloanbal.reason as cnt FROM vwloanbal where vwloanbal.bal>0 and vwloanbal.emptid in" & _
                                        "(Select emptid from emp_job_assign where '" & pdate1 & "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "')  and emp_job_assign.project_id='" & projid & "') and vwloanbal.dstart<='" & _
                                    pdate1 & "') AS derivedtbl_1", Session("con")))
            ' Response.Write("SELEC'T COUNT(cnt) AS Expr1 froM " & _
            '  "(SELECT distinct vwloanbal.reason as cnt FROM vwloanbal where vwloanbal.bal>0 and vwloanbal.emptid in" & _
            '    "(Select emptid from emp_job_assign where '" & pdate1 & "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "')  and emp_job_assign.project_id='" & projid & "') and vwloanbal.dstart<='" & _
            '  pdate1 & "') AS derivedtbl_1")
            'ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vwloanbal WHERE  bal>0 and (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            'Response.Write("ref=0")

            If IsNumeric(paid) = False Then
                paid = "0"
            End If

            'Response.Write(ccol)

            Dim pass As Integer = 0
            If ccol = 0 Then
                ccol = 1
            ElseIf ccol = 1 Then
                ccol = 1
                pass = 1
            End If

            'Response.Write(ccol.ToString & "<br>")
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim paidpayrol As String
            Dim stylew(4) As String
            Dim tcell As Integer
            Dim colded As String = ""
            Dim rtnvalue As String = ""

            rs2 = dbs.dtmake("dbpenx", "SELECT distinct reason froM (SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                               "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                               "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                               pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con"))

            ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

            colded &= "<tr>" & Chr(13)
            If rs2.HasRows Then
                Dim i As Integer = 0
                While rs2.Read
                    ' Response.Write(rs2.Item("reason"))
                    If rs2.Item("reason") = "-" Then
                        colded &= "<td class='dedctx'>Other</td>"
                    Else
                        colded &= "<td class='dedctx'>" & rs2.Item("reason").ToString & "</td>"

                    End If
                    reasonname(i) = rs2.Item("reason")
                    i += 1
                End While
            Else

                colded &= "<td class='dedctx'>&nbsp;</td>"
            End If
            colded &= "</tr>" & Chr(13)
            Dim wdthpay As Integer = 1250
            tcell = ccol + 13
            If Request.QueryString("widthpay") <> "" Then
                wdthpay = Request.QueryString("widthpay")
            End If
            Dim ratiow As Double
            ratiow = (wdthpay - 350 - 30 - 60) / tcell

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
            Session("pstop") = False
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                Try
                    Dim rssql, rssqlnew As String

                    ' Response.Write(projid & pdate2.ToShortDateString)
                    rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                    rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                      "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                      "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                      "and emprec.id in(select emptid from emp_job_assign " & _
                                                                      "where project_id='" & projid.ToString & "' " & _
                                                                      "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                                                                      " ORDER BY emp_static_info.first_name,emprec.id desc "
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
                    Session("payrolllist") = rssqlnew
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
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc <br>")
            End If
            'Response.Write("ddd")
            If rs.HasRows Then
                header = "<div id='pensionstop' style='display:none;'>Pension stop for terminated:<input type='checkbox' name='chkpensionstop' id='chkpensionstop' value='checked' /></div><table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                header &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                header &= "<td style='text-align:center;font-weight:bold;' colspan='20' >" & Session("company_name") & _
            "<br> Project Name:"
                If projid <> "" Then
                    header &= spl(0).ToString
                Else
                    header &= "All Projects"
                End If
                header &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                header &= "</tr>" & Chr(13)

                header &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                header &= "<td rowspan='2' class='dw'>No.</td><td class='dw' rowspan='2'>Emp. Id</td>"
                header &= "<td class='fxname' rowspan='2'>Full Name</td>"
                header &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                header &= "<td class='dw'  style='width:30px;' rowspan='2'>Days Worked</td>"
                header &= "<td  class='fitx' rowspan='2'>Basic Earning</td>"
                header &= "<td  class='fitx' rowspan='2'>Taxable Allowance</td>"
                header &= "<td  class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                header &= "<td class='fitx' rowspan='2'> Overtime</td>"
                header &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                header &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                header &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                header &= "<td class='dedct' style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                header &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                header &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                header &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                header &= "<td class='fitx' rowspan='2'>Net Pay</td>"
                header &= "<td class='fitx' rowspan='2'>Leave Bal</td>"
                header &= "<td class='dw'  id='chkall'  rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                header &= "<td rowspan='2' class='signpart'>Signature</td></tr>" & Chr(13)
                If paid = "None" Then
                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                    rs2 = dbs.dtmake("dbpenx", "SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                        "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                        "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                        pdate1 & "'", Session("con"))
                    'Response.Write(ccol)
                Else
                    rs2 = dbs.dtmake("dbpenx", "SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                       "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                       "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                       pdate1 & "'  and emp_job_assign.project_id='" & projid & "'", Session("con"))
                    rs2 = dbs.dtmake("dbpenx", "SELECT reason froM (SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                       "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                       "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                       pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con"))
                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0 and dstart < ='" & pdate1 & "'", Session("con"))
                    ' Response.Write(ccol)
                End If

                rs2.Close()
                header &= colded
                Dim k As Integer = 1
                Dim color As String = ""
                Dim resing As Date
                Dim did As String
                Dim rt() As String
                body = ""
                While rs.Read

                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))

                    emptid = rs.Item("id")
                    ' Response.Write(rs.Item("id"))
                    paid = ""
                    paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where emptid='" & emptid & "' and (ref_inc='0' or ref_inc is null))", Session("con"))

                    '  Response.Write("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where emptid='" & emptid & "' and ref_inc='0' or ref_inc is null)<br>")
                    If paid.ToString = "None" Then
                        resing = "#1/1/1900#"
                        If color <> "#aabbcc" Then
                            color = "#aabbcc"
                        Else
                            color = "white"
                        End If

                        did = ""
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "' and resign_date>='" & pdate1 & "'", Session("con"))
                        'Response.Write(did & "<br>")
                        If (rs.Item("active") = "n") Then
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "' and resign_date>='" & pdate1 & "'", Session("con"))
                            ' Response.Write("yyy")
                        End If

                        If did <> "None" And did <> "" Then
                            resing = did

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
                        If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year Then
                            'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                            incwhen = dateincr.Subtract(pdate1).Days
                        End If

                        If dhr.Month = pdate1.Month And dhr.Year = pdate1.Year Then

                            incwhen = 0
                        End If
                        ' Response.Write(resing.ToShortDateString & "<br>")
                        ' Response.Write(fm.getfullname(rs.Item("emp_id"), Session("con")) & "<br>")
                        ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                        If ((resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing) And incwhen = 0 Then 'question..........solved
                            'Response.Write(emptid & "<br>")
                            ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                            'Response.Write(resing.ToShortDateString & "<br>")
                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            'Response.Write(sql & "<br>")
                            sal = Me.getsalhrm(emptid, pdate1, Session("con"))
                            'Response.Write(emptid & "===>" & sal(0) & "<br>")
                            cell(2) = sal(0)
                            If IsNumeric(sal(0)) = False Then
                                'Response.Write(sal(0))
                                cell(2) = "0"
                            End If
                            ca = 0
                            clwp = 0

                            If cell(2) = "Sorry This employee salary info is not setted!" Then
                                cell(2) = "0"
                                '  color = "#ccaa99"
                            End If
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
                                    fired = pdate2.Subtract(resing).Days + 1
                                    ' Response.Write(resing.ToShortDateString)
                                Else
                                    fired = 0
                                End If

                            End If
                            'Response.Write(nod.ToString & "---" & fired.ToString & "----" & ca & "----" & clwp & "----" & newemp & "<br>")
                            calc = 0
                            calc = nod - ca - clwp - newemp - fired

                            cell(3) = Math.Round(calc, 2).ToString
                            ' Response.Write("<br>===>cl3:" & cell(3))
                            Dim dbl As Double
                            dbl = (CDbl(cell(2)) / nod) * CDbl(calc)

                            ' Response.Write(dbl.ToString & rs.Item("emp_id") & "<br>")

                            'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                            cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString ' God does this
                            'cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                            ' Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)
                            ' Response.Write(cell(4).ToString & "<br>")

                            'cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
                            Dim allz() As String
                            'allz = getallowancex(emptid, "y", pdate1, calc, Session("con"))

                            allz = getallowancexp(emptid, "y", pdate1, calc, Session("con"))
                            'Response.Write(allz(1).ToString & "<br>")
                            cell(5) = allz(0)
                            ' Response.Write(allz(0).ToString)
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            ElseIf IsNumeric(cell(5)) = False Then
                                ' Response.Write(cell(5).ToString & "<br>")
                                cell(5) = "0"

                            End If

                            If CDbl(cell(5)) > 0 Then
                                'cell(5) = ((CDbl(cell(5)) / nod) * calc).ToString
                                ' Response.Write(cell(5))
                                'cell(5) = (CDbl(cell(5)) / nod) * (nod - fired)
                            End If
                            '  cell(6) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='n') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            'allz = getallowancex(emptid, "n", pdate1, calc, Session("con"))
                            allz = getallowancexp(emptid, "n", pdate1, calc, Session("con"))
                            'Response.Write(allz(1).ToString & "<br>")
                            If emptid = 49 Then
                                '  Response.Write(pdate1 & "=" & allz(1) & "===" & calc & "<br>")
                            End If
                            cell(6) = allz(0)
                            If cell(6).ToString = "None" Then
                                cell(6) = "0"
                            ElseIf IsNumeric(cell(6)) = False Then
                                cell(6) = "0"
                            End If
                            If CDbl(cell(6)) > 0 Then
                                'cell(6) = (CDbl(cell(6)) / nod) * calc
                                ' cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                            End If

                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString
                            ' Response.Write(cell(10) & "<br>")

                            If projid <> "" Then
                                'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                'Response.Write(sql)
                                sql = "select distinct reason,id,bal,nopay,amt from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(" & rtnvalue & ") order by reason"

                                ' Response.Write(sql & "<br>")
                                rs2 = dbs.dtmake("loan", sql, Session("con"))
                            Else
                                rs2 = dbs.dtmake("loan", "select distinct reason,id,bal,nopay,amt from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            End If

                            If rs2.HasRows Then
                                Dim emp1() As String = {""}
                                Dim list() As String = {""}
                                For j = 0 To reasonname.Length - 2
                                    cellb(j) = 0
                                Next
                                While rs2.Read

                                    damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))
                                    '   Response.Write(reasonname.Length)
                                    ' Array.Sort(reasonname)

                                    For j = 0 To reasonname.Length - 2

                                        If LCase(reasonname(j)).Trim = LCase(rs2.Item("reason")).ToString.Trim Then

                                            If damt > rs2.Item("bal") Then
                                                damt = rs2.Item("bal")
                                            End If

                                            cellb(j) = damt.ToString

                                            cellbval(j) = rs2.Item("id").ToString

                                            sumloan(j) += CDbl(cellb(j))

                                        Else

                                            If cellb(j) > 0 Then
                                            Else
                                                cellb(j) = "0.00"
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                            End If

                                        End If
                                    Next

                                End While

                            Else

                                For j = 0 To reasonname.Length - 2
                                    cellbval(j) = "0"
                                    cellb(j) = "0.00"
                                    sumloan(j) += CDbl(cellb(j))
                                Next
                            End If
                            rs2.Close()





                            ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            Dim penss As String
                            penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart <='" & pdate2 & "' order by id desc", Session("con"))
                            ' Response.Write(pdate2.Subtract(dhr).Days.ToString & "<br>")
                            ' Response.Write(penss.ToString & "<br>")
                            If pdate2.Subtract(dhr).Days >= 1 Then
                                cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                            Else
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If
                            Dim penss2 As String = ""
                            penss2 = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and pensstop<='" & pdate1 & "' order by id desc", Session("con"))

                            If penss <> "None" Then
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                                If penss2 <> "None" Then
                                    If CInt(penss2) > CInt(penss) Then
                                        cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                        cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                                    End If
                                End If
                            Else
                                If penss2 <> "None" Then

                                    cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                    cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                                End If
                            End If
                            ' Response.Write(cell(3).ToString & "===")
                            If color <> "#ccaa99" And cell(3) > 0 Then
                                If fired = 0 Then
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
                            If cell(2).ToString <> "0" And CDbl(cell(3)) > 0 Then
                                ' Response.Write(resing.ToShortDateString)
                                body &= "<tr "
                                If fired <> 0 Or resing.ToShortDateString <> "1/1/1900" Then
                                    body &= "style='background-color:red;'"
                                    Session("pstop") = True
                                End If
                                body &= "><td id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

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
                                            body &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                        Else
                                            body &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                        End If
                                    Else
                                        For j = 0 To reasonname.Length - 2
                                            If String.IsNullOrEmpty(cellbval(j)) = False Then
                                                body &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                            Else
                                                body &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
                                            End If
                                        Next
                                        If ccol = 0 Then
                                            body &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                        End If
                                    End If
                                Next
                                netincom = CDbl(cell(8)) - CDbl(cell(14))
                                If color <> "#ccaa99" Then
                                    sumnet += netincom
                                End If
                                ' Response.Write(netincom.ToString & "<----<br>")
                                body &= "<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"
                                body &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_16'>" & getleaveinfobal(emptid, Session("con")) & "</td>"
                                body &= "<td  style='text-align:right;'>"
                                If paid.ToString = "None" And CDbl(cell(2)) <> 0 Then
                                    body &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' "
                                    If fired = 0 Then
                                        body &= "checked='checked'"

                                    End If
                                    body &= " onclick='javascript:sumcolx();'></td>"
                                Else
                                    If CDbl(cell(2)) <> 0 Then
                                        body &= " Paid</td>"

                                        '  body &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' style='visibility:hidden;'>Paid</td>"
                                    Else
                                        body &= " None</td>"

                                        'body &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' ></td>"

                                    End If


                                End If
                                body &= "<td style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If

                End While


                body &= "<tr style='text-weight:bold;text-align:right;'>" & _
            "<td class='cooo'>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
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
                    body &= ("<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='lon-" & j & "'>" & fm.numdigit(sumloan(j).ToString, 2) & "</td>")
                Next
                If ccol = 0 Then
                    body &= ("<td class='cooo' style='text-align:right;'>&nbsp;</td>")
                End If
                body &= "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
            "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
             "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
              "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
              "<td class='cooo' style='' colspan='2'>..&nbsp;" & _
 "</td></tr><tr id='result'></tr>"
                footer = fm.signpart()
                footer &= "</table>"
                outp = header & body & footer
                outp &= fm.projtrans(Request.Form("projname"), pdate1, Session("con")).ToString

                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)


            End If
            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
            '  Response.Write("End time:" & Now.Minute.ToString & ":" & Now.Second.ToString & ":" & Now.Millisecond.ToString)
            Return outp

        End If
        Return outp

    End Function







    '-------------------------------------------
    Public Function makeform2_payroll_new(ByVal vx() As String)
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
        Dim sec As New k_security
        ' Response.Write(Request.QueryString("month"))


        Try
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
                    ' ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                    ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT distinct vwloanbal.reason as cnt FROM vwloanbal INNER JOIN " & _
                                          "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                          "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                          pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

                Else
                    'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                    ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT distinct vwloanbal.reason as cnt FROM vwloanbal INNER JOIN " & _
                                          "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                          "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                          pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

                End If
                Dim pass As Integer = 0
                If ccol = 0 Then
                    ccol = 1
                Else
                    pass = 1
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
                    'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
                    ' rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                    Try
                        Dim rssqlnew As String
                        Dim rtnvalue As String
                        rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                        rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                          "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                          "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                          "and emprec.id in(" & rtnvalue & ")" & _
                                                                          " ORDER BY emp_static_info.first_name,emprec.id desc "
                        If Session("payrolllist") <> "" Then
                            rssqlnew = Session("payrolllist")
                        End If
                        '  rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                        '                                                   "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                        '                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                        '                                                 "and emprec.id in(select emptid from emp_job_assign " & _
                        '                                                "where project_id='" & projid.ToString & "' " & _
                        '                                               "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                        '                                              " ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
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
                End If

                If rs.HasRows Then
                    outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                    outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                    outp &= "<td style='text-align:center;font-weight:bold;' colspan='21' >" & Session("company_name") & _
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
                    'outp &= "<td class='fitx' rowspan='2'>Leave Bal</td>"

                    outp &= "<td rowspan='2' class='signpart'>Signature</td>"
                    If paid = "None" Then
                        ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                        rs2 = dbs.dtmake("dbpenx", "SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                            "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and ('" & pdate1 & _
                                            "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') or " & _
                                            "emp_job_assign.date_from between '" & pdate1 & "'  and isnull(emp_job_assign.date_end,'" & pdate2 & "') )and vwloanbal.dstart<='" & _
                                            pdate1 & "'", Session("con"))
                        'Response.Write(ccol)
                    Else
                        rs2 = dbs.dtmake("dbpenx", "SELECT vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                           "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and ('" & pdate1 & _
                                           "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') or " & _
                                           "emp_job_assign.date_from between '" & pdate1 & "'  and isnull(emp_job_assign.date_end,'" & pdate2 & "') )and vwloanbal.dstart<='" & _
                                           pdate1 & "'", Session("con"))  ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0 and dstart < ='" & pdate1 & "'", Session("con"))
                        ' Response.Write(ccol)
                    End If
                    rs2 = dbs.dtmake("dbpenx", "SELECT reason froM (SELECT distinct vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                       "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                       "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                       pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con"))


                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                    outp &= "<tr>" & Chr(13)
                    If pass = 0 Then
                        outp &= "<td class='dedctx'>&nbsp;</td>"
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
                    Dim pstop As Boolean = False
                    While rs.Read
                        pstop = False
                        'check id in the list2 and make statement again 
                        emptid = rs.Item("id")
                        'Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                        If fm.searcharray(vx, emptid.ToString) Then

                            ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                            End If


                            If (rs.Item("active") = "n") Then
                                Response.Write(rs.Item("emp_id"))
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
                            If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year Then
                                'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                                incwhen = dateincr.Subtract(pdate1).Days
                            End If

                            If dhr.Month = pdate1.Month And dhr.Year = pdate1.Year Then

                                incwhen = 0
                            End If
                            ' Response.Write(resing & "====" & pdate1 & "=====" & pdate2)

                            If ((resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing) And incwhen = 0 Then 'question..........solved
                                '  Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                                ' Response.Write(Request.QueryString("chkpen") & "<br>")

                                If Request.Form("chkpensionstop") <> "" Then
                                    Session("pstop") = False

                                    If IsDate(resing) = True Then
                                        If resing <> "#1/1/1900#" Then
                                            pstop = True
                                            ' Response.Write("stoppppiiiinnnggg")
                                        End If

                                    End If
                                End If
                                cell(0) = rs.Item("emp_id")
                                cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                                'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                                ' Response.Write(sql & "<br>")
                                sal = Me.getsalhrm(emptid, pdate1, Session("con"))
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
                                        fired = pdate2.Subtract(resing).Days + 1
                                    Else
                                        fired = 0
                                    End If

                                End If
                                calc = nod - ca - clwp - newemp - fired

                                cell(3) = Math.Round(calc, 2).ToString
                                ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                                cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                                'Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)

                                ' cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                                '  Response.Write(cell(5).ToString)
                                ' cell(5) = "0"
                                Dim allz() As String
                                allz = getallowancexp(emptid, "y", pdate1, calc, Session("con"))
                                ' Response.Write(allz(2).ToString & "<br>")
                                cell(5) = allz(0)
                                If cell(5).ToString = "None" Then
                                    cell(5) = "0"
                                ElseIf cell(5).ToString = "" Then
                                    cell(5) = "0"
                                End If

                                If CDbl(cell(5)) > 0 Then
                                    ' cell(5) = ((CDbl(cell(5)) / nod) * calc).ToString
                                    ' Response.Write(cell(5))
                                    'cell(5) = (CDbl(cell(5)) / nod) * (nod - fired)
                                End If
                                'cell(6) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='n') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                                'Response.Write(cell(6) & "<br>")
                                ' cell(6) = "0"
                                ' Dim allz() As String
                                allz = getallowancexp(emptid, "n", pdate1, calc, Session("con"))
                                ' Response.Write(allz(2).ToString & "<br>")
                                cell(6) = allz(0)
                                If cell(6).ToString = "None" Then
                                    cell(6) = "0"
                                ElseIf IsNumeric(cell(6)) = False Then
                                    cell(6) = "0"
                                End If
                                If CDbl(cell(6)) > 0 Then
                                    'cell(6) = (CDbl(cell(6)) / nod) * calc
                                    ' cell(6) = (CDbl(cell(6)) / nod) * (nod - fired)
                                End If
                                cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                                ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                                '   Response.Write(cell(7).ToString & "<br>")
                                cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                                'Response.Write(cell(8).ToString & "<br>")
                                cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                                cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9)), pdate1, Session("con")), 2).ToString

                                If projid <> "" Then
                                    'sql = "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid exist(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and end_date is null) order by id"

                                    'Response.Write(sql)
                                    sql = ""
                                    rs2 = dbs.dtmake("loan", "select distinct id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " and emptid in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' ) order by id", Session("con"))

                                Else
                                    rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                End If
                                For j = 0 To reasonname.Length - 2
                                    cellb(j) = "0"
                                Next
                                If rs2.HasRows Then
                                    j = 0

                                    While rs2.Read
                                        damt = (CDbl(rs2.Item("amt")) / CDbl(rs2.Item("nopay")))
                                        For j = 0 To reasonname.Length - 2
                                            ' Response.Write(reasonname(j) & "=" & rs2.Item("reason") & "<br>")
                                            If reasonname(j) = rs2.Item("reason") Then

                                                cellb(j) = damt.ToString
                                                cellbval(j) = rs2.Item("id").ToString
                                                sumloan(j) += CDbl(cellb(j))
                                            Else
                                                If CInt(cellb(j)) > 0 Then
                                                Else
                                                    cellb(j) = "0.00"
                                                    cellbval(j) = "0"
                                                    sumloan(j) += CDbl(cellb(j))
                                                End If

                                                'End If
                                            End If

                                        Next

                                    End While

                                Else
                                    For j = 0 To reasonname.Length - 2
                                        cellbval(j) = "0"
                                        cellb(j) = "0.00"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next
                                End If
                                rs2.Close()

                                ' Dim dhr As Date
                                dhr = rs.Item("hire_date")
                                ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                                Dim penss As String
                                penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", Session("con"))
                                ' Response.Write(pdate2.Subtract(dhr).Days.ToString & "<br>")
                                'Response.Write(penss.ToString & "<br>")
                                If pdate2.Subtract(dhr).Days >= 1 Then
                                    cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                    cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString

                                Else
                                    cell(12) = "0.00"
                                    cell(13) = "0.00"
                                End If
                                Dim penss2 As String
                                penss2 = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and pensstop<='" & pdate1 & "' order by id desc", Session("con"))
                                If pstop = True Then
                                    ' Response.Write("ssssssssssttttopppppp")
                                    penss2 = "None"
                                    cell(12) = "0.00"
                                    cell(13) = "0.00"
                                End If
                                If penss <> "None" Then
                                    cell(12) = "0.00"
                                    cell(13) = "0.00"
                                    If penss2 <> "None" Then
                                        If CInt(penss2) > CInt(penss) Then
                                            cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                            cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                                        End If
                                    End If
                                Else
                                    If penss2 <> "None" Then

                                        cell(12) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pemp) / 100)), 2).ToString
                                        cell(13) = Math.Round((fm.pension(CDbl(cell(4)), CDbl(pco) / 100)), 2).ToString
                                    Else

                                    End If
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
                                    'outp &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_16'>" & getleaveinfobal(emptid, Session("con")) & "</td>" & Chr(13)

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
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & Chr(13)
                    outp &= "<td  class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='lv'>..</td>" & Chr(13) & _
  "<td class='cooo' style='' colspan='2'>..&nbsp;" & _
         "</td></tr>"
                    'outp &= "</table><br>"
                    'outp &= "<table cellspacing='0' cellpadding='0' width='800px' style='font-size:9.5pt;'>"
                    outp &= fm.signpart()
                    outp &= "</table>" & Chr(13)
                    ' outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

                    rs.Close()
                    fm = Nothing
                    dbs = Nothing
                    Response.Write(outp)
                End If

                Return outp



            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Function
    Function pay_taxx(ByVal amt As Double, ByVal pd As Date)
        Dim fm As New formMaker
        Dim l() As String
        Dim px() As String
        Dim rtn As String = ""
        Dim tax As Double = 0
        rtn = fm.getinfo2("select path from tbltax where pdate<'" & pd & "' order by pdate desc", Session("con"))
        If rtn <> "" Then
            '  Response.Write(amt & "xxxxxxxxxx" & rtn & "<br>")
            Try


                l = File.ReadAllLines(rtn)

                For i As Integer = 0 To l.Length - 1
                    'Response.Write(l(i) & "<br>")
                    px = l(i).Split(",")
                    If CDbl(amt) > CDbl(px(0)) Then
                        tax = CDbl(amt) * CDbl(px(2)) - px(1)
                        ' Response.Write(amt & "====" & px(2) & " ===" & px(1) & "<br>")

                        Exit For
                    End If
                Next
            Catch ex As Exception
                'Response.Write(ex.ToString)
                ' Response.Write(rtn & "<br>")
                For i As Integer = 0 To l.Length - 1
                    px = l(i).Split(",")
                    '  Response.Write(px(0) & "===" & px(1) & "====" & px(2) & "<br>")
                Next
                tax = 0
            End Try

        End If
        ' Response.Write("tax===" & tax & "<br>")
        Return tax



    End Function
    Function pay_tax(ByVal amt As Double, ByVal pd As Date, ByVal con As SqlConnection)
        Dim fm As New formMaker
        Dim l() As String
        Dim px() As String
        Dim rtn As String = ""
        Dim tax As Double = 0
        rtn = fm.getinfo2("select path from tbltax where pdate<='" & pd & "' order by pdate desc", con)
        If rtn <> "" Then
            '  Response.Write(amt & "xxxxxxxxxx" & rtn & "<br>")
            Try


                l = File.ReadAllLines(rtn)

                For i As Integer = 0 To l.Length - 1
                    'Response.Write(l(i) & "<br>")
                    px = l(i).Split(",")
                    If CDbl(amt) > CDbl(px(0)) Then
                        tax = CDbl(amt) * CDbl(px(2)) - px(1)
                        'Response.Write(amt & "====" & px(2) & " ===" & px(1) & "<br>")

                        Exit For
                    End If
                Next
            Catch ex As Exception
                'Response.Write(ex.ToString)
                ' Response.Write(rtn & "<br>")
                For i As Integer = 0 To l.Length - 1
                    px = l(i).Split(",")
                    Response.Write(px(0) & "errrrrr===" & px(1) & "====" & px(2) & "<br>")
                Next
                tax = 0
            End Try

        End If
        ' Response.Write("tax===" & tax & "<br>")
        Return tax



    End Function
    '   -----------------------------------------------------------payroll wieth leave------------------------------------------------------------------------------------------

End Class

