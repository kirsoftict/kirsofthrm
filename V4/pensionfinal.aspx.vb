Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class pensionfinal
    Inherits System.Web.UI.Page
    Public Function makepension_rows() 'pension rws
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
                    ' Response.Write(rssqlnew)
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

End Class
