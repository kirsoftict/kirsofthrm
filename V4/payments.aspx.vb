Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class payments
    Inherits System.Web.UI.Page
    Public payrdate As Date

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
    Public Function makeformpx_payroll()
        Try
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
                    If String.IsNullOrEmpty(Request.Form("projname")) = False Then
                        spl = Request.Form("projname").Split("|")
                        projename = Request.Form("projname")
                    Else
                        spl = Request.QueryString("projname").Split("|")
                        projename = Request.QueryString("projname")
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
                Dim pdate3, pdate4 As Date
                pdate3 = pdate1.AddMonths(-1)
                pdate4 = pdate2.AddMonths(-1)
                Dim paid As String
                Dim j As Integer

                Dim paypaid As Integer = 0
                ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
                If paypaid <> 0 Then
                    ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

                End If
                paid = ""
                    paid = fm.getinfo2("select id from payrol_reg where month='" & pdate3.Month & "' and year='" & pdate3.Year & "' and id in(select pr from payrollx where ref_inc is not null and remark='Increament')", Session("con"))
                    ' Response.Write("select id from payrol_reg where month='" & pdate3.Month & "' and year='" & pdate3.Year & "' and id in(select pr from payrollx where ref_inc is not null and remark='Increament' and date_paid between '" & pdate3 & "' and '" & pdate4 & "')")
                    ' Response.Write(paid.ToString)
                    If paid = "None" Then
                        Response.Write("There is No Payroll list")
                    Else
                        ' Response.Write("select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='Increament'")

                        rrr = dbs.dtmake("payrol", "select distinct ref,date_paid,bank from payrollx where pr='" & paid & "' and remark='Increament'", Session("con"))
                        If rrr.HasRows Then

                            Response.Write("<div id='viewlistx'><b>Project: " & projename & "<br>Payroll in the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</b><table>")
                            Dim ccout As String = "0"
                            While rrr.Read
                                '  Response.Write(rrr.Item(0) & "<br>")

                                If projid.ToString <> "" Then
                                    'Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')", Session("con")))
                                    ccout = "0"
                                    ' Response.Write(rrr.Item("ref"))
                                    ccout = fm.getinfo2("select count(id) from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                                    ' Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate3 & "'between  date_from and isnull(date_end,'" & pdate4 & "')", Session("con")).ToString & "=" & projid.ToString)
                                    If fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "' and remark='Increament') and '" & pdate3 & "'between  date_from and isnull(date_end,'" & pdate4 & "')", Session("con")).ToString = projid.ToString Then
                                        '  Response.Write("wooooo")
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
                                    "javascript:gotocheckinc('del','" & rrr.Item(0) & "');" & Chr(34) & _
                                    " ></div>")
                                        Else
                                            Response.Write("<div class='deletepayrol'></div>")
                                        End If
                                        Response.Write("</td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='viewpayrol' onclick=" & _
                                    Chr(34) & "javascript:gotocheckinc('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='viewdelpayrol' onclick=" & Chr(34) & _
                                    "javascript:gotocheckinc('viewdel','?prdel=" & rrr.Item(0) & "&pd=" & rrr.Item(1) & "');" & _
                                    Chr(34) & "></div></td><td> &nbsp;|&nbsp;</td><td class='v1'><div class='editpayrol' onclick=" & Chr(34) & _
                                    "javascript:gotocheckinc('Edit','" & rrr.Item(0) & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='bankpayrol' onclick=" & Chr(34) & _
                                    "javascript:gotocheckinc('Bank','?ref=" & rrr.Item(0) & _
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
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Function

    Public Function makeformunpaid_payroll()
        Session("companyck") = "Net Consult P.L.C"
        Dim fm As New formMaker
        Dim nod As Integer
        If Session("company_name") <> Session("companyck") Then
            Try
                fm.mailsender("Copy right issue from client:" & Session("companyck"), "z.kirubel@gmail.com;kirzed@yahoo.co.uk", "z.kirubel@gmail.com", "Copywrite")
            Catch ex As Exception
                fm.exception_hand(ex)
            End Try
            Response.Redirect("logout.aspx?msg=copyright issue&msgtype=Caution&titlex=Caution")
        End If
        Dim pdate1, pdate2, pdate3, pdate4 As Date


        Dim fl As New file_list
        'Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double

        Dim suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
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

        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        Dim ccol As Integer = 0
        'Response.Write(Request.Form("projname"))
        If Request.Form("month") <> "" Then
            Dim spl() As String
            Dim projid As String
            If Request.Form("projname") <> "" Then
                spl = Request.Form("projname").Split("|")
                If spl.Length <= 1 Then
                    ReDim spl(2)
                    spl(0) = Request.Form("projname")
                    spl(1) = ""
                    Response.Write("Sorry! the Project is not selected Properly")
                End If
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            End If
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate3 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate4 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            pdate1 = pdate3.AddMonths(-1)
            'pdate2 = pdate4.AddMonths(-1)
            pdate2 = pdate1.Month & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year

            Dim rs As DataTableReader
            Dim rs2 As DataTableReader

            Dim ccol2 As Integer = 0
            Dim paid, paid2 As String
            Dim j As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            ' rs = dbs.dtmake("nuemp", "select * from emp_inc where paid_date between '" & pdate3 & "' and '" & pdate4 & "'", Session("con"))

            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If

            paid = ""
            paid = fm.getinfo2("select id from emp_inc where paidref is null", Session("con"))
            'Response.Write(projid.ToString)

            ccol = CInt(fm.getinfo2("SELECT COUNT(countx) AS Expr1 froM (SELECT distinct reason AS countx FROM emp_inc WHERE (amt+amt2) > 0 AND " & _
                                    "(inc_date between '" & pdate3 & "' and '" & pdate4 & "' and paidref is null) and " & _
                                    "emptid in(select emptid from emp_job_assign " & _
                                                   "where project_id='" & projid.ToString & "' " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')))" & _
                                                   "GROUP BY reason) AS derivedtbl_1", Session("con")))
            If IsNumeric(ccol) = False Then
                ccol = 1
            End If
            ReDim cellb(ccol + 1)
            ReDim cellbval(ccol + 1)
            ReDim sumloan(ccol + 1)
            ReDim reasonname(ccol + 1)
            Dim tit As String = ""
            Dim toptit As String = ""
            strc = "0"
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                Try
                    '  rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                    '                                "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                    '                               "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                    '                              "and emprec.id in(select emptid from emp_job_assign " & _
                    '                             "where project_id='" & projid.ToString & "' " & _
                    '                            "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                    '                           " ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                    '  rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                    'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
                    Dim rssqlnew As String
                    Dim rtnvalue As String
                    rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                    '  Response.Write(rtnvalue)
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
                    '  Response.Write(rssqlnew)
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
            rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE (amt+amt2) > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and (paidref is null or paidref='') and " & _
                                " emptid in(select emptid from emp_job_assign " & _
                                                  "where project_id='" & projid.ToString & "' " & _
                                                  " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                                                  " GROUP BY reason", Session("con"))
            '  Response.Write("SELECT distinct reason  FROM emp_inc WHERE (amt+amt2) > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and (paidref is null or paidref='') and " & _
            '                     " emptid in(select emptid from emp_job_assign " & _
            '                                      "where project_id='" & projid.ToString & "' " & _
            '                                     " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
            '                                    " GROUP BY reason")

            ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
            tit &= "<tr>" & Chr(13)
            ' rs.Close()
            If rs2.HasRows Then
                Dim i As Integer = 0
                While rs2.Read

                    If rs2.Item("reason") = "-" Then
                        tit &= "<td >Other</td>"
                    Else
                        tit &= "<td >" & rs2.Item("reason").ToString & "</td>"
                    End If
                    reasonname(i) = rs2.Item("reason")
                    ' Response.Write(reasonname(i) & "i<br>")
                    i += 1

                End While

                reasonname(i) = "Non-Taxable"
                tit &= "<td >Non-Taxable</td>"
            End If

            For l As Integer = 0 To ccol - 1
                If l = ccol - 1 Then
                    If toptit = "" Then
                        toptit &= reasonname(l)
                    Else
                        toptit = toptit.Substring(0, toptit.Length - 2) & " and " & reasonname(l)
                    End If

                Else
                    toptit &= reasonname(l) & ", "
                End If

            Next
            'Response.Write("....")
            If rs.HasRows Then
                outp = "<style>" & _
             "#tb1 " & _
             "{" & _
                 "border:1px solid black;" & _
                 "font-size:9pt;" & _
             "}" & _
           " #tb1 td" & _
             "{" & _
              "border-top: 1px solid black;" & _
             " border-left:1px solid black;" & _
                 "font-size:9pt;" & _
             "}" & _
            " </style>"
                outp &= "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='" & (13 + CInt(ccol)).ToString & "' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br>" & toptit & " payment Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp. Id.</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitx'   colspan='" & (CInt(ccol) + 1).ToString & "'><center>Increament</center></td>"
                outp &= "<td class='fitx' rowspan='2'>Total Increament</td>"
                outp &= "<td class=''fitx' rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitx'  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Tax Paid</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td class='fitx'  rowspan='2'>Net pay</td>"
                outp &= "<td class='dw'  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Signature</td></tr>" & Chr(13)

                outp &= tit
                outp &= "</tr>" & Chr(13)
                rs.Close()
                'Response.Write(toptit)
                rs2.Close()
                rs2 = dbs.dtmake("dbpen", "SELECT *  FROM emp_inc inner join emprec on emprec.id=emp_inc.emptid inner join emp_static_info as esi " & _
                                 " on esi.emp_id=emprec.emp_id WHERE (amt+amt2) > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & _
                                 "') and paidref is null order by esi.first_name", Session("con"))
                ' Response.Write("SELECT *  FROM emp_inc inner join emprec on emprec.id=emp_inc.emptid inner join emp_static_info as esi " & _
                '                 " on esi.emp_id=emprec.emp_id WHERE (amt+amt2) > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & _
                '                "') and paidref is null order by esi.first_name")
                Dim arremp(1) As String
                arremp(0) = ""
                If rs2.HasRows Then
                    Dim k As Integer = 1
                    Dim color As String = ""
                    Dim resing As Date
                    While rs2.Read

                        emptid = rs2.Item("emptid")
                       
                        If rs2.IsDBNull(7) = True Then
                            paid = "None"

                        Else
                            paid = rs2.Item("paidref")
                        End If
                        Dim prj As String
                        ' Response.Write("select project_id from emp_job_assign where emptid=" & emptid & " and '" & pdate2 & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')")
                        prj = fm.getinfo2("select project_id from emp_job_assign where emptid=" & emptid & " and '" & pdate1 & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')", Session("con"))

                        '"select project_id from emp_job_assign where emptid=" & emptid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))", Session("con"))
                        'Response.Write(prj.ToString & "<br>" & projid.ToString)
                        '   Response.Write("<br>select project_id from emp_job_assign where emptid=" & emptid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))<br>")
                        '  Response.Write(prj & "--------" & spl(1) & "==============" & spl(0) & "<br>")

                        If fm.searcharray(arremp, emptid.ToString) = False And prj = spl(1) Then
                            ' Response.Write(emptid & "<br>")
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                            End If

                            cell(0) = k
                            cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                            cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                            ' cell(3)=fm.getinfo2("select sum(gross_earn) as Exp2 from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month=" 
                            cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString
                            '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")
                            ' cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString
                            '   Response.Write("<br>select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))")
                            'cell(3) = "0"
                            If cell(3) = "None" Or cell(3).ToString.Length > 10 Or cell(3) = "" Then
                                cell(3) = 0
                            End If
                            Dim otamtpaid As Double
                            otamtpaid = getotpaidin(pdate1, pdate2, emptid, Session("con"))
                            ' Response.Write(otamtpaid & "<br>")
                            ' otamtpaid=fm.getinfo2("select sum(ot) from payrollx where remark='OT-Payment'
                            'Response.Write(mtx.ToString)
                            'Response.Write(pdate1.ToShortDateString & "---" & pdate2.ToShortDateString & otamtpaid.ToString)
                            cell(3) = CDbl(cell(3)) + otamtpaid
                            Dim rsp As DataTableReader
                            '  Response.Write("<br>SELECT *  FROM emp_inc WHERE emptid=" & emptid & " and (amt+amt2) > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is null")
                            rsp = dbs.dtmake("dbx", "SELECT *  FROM emp_inc WHERE emptid=" & emptid & " and (amt+amt2) > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is null", Session("con"))
                            j = 0
                            inc = 0
                            For p As Integer = 0 To reasonname.Length - 1
                                cellb(p) = 0
                                cellbval(p) = 0
                            Next
                            Dim max As Integer = 0
                            If rsp.HasRows Then
                                While rsp.Read
                                    '  Response.Write("llllooopppp" & rs.Item("emptid"))
                                    For p As Integer = 0 To reasonname.Length - 1
                                        ' Response.Write(reasonname(p) & " <=====" & rsp.Item("reason") & "<br>")

                                        If LCase(Trim(reasonname(p))) = LCase(Trim(rsp.Item("reason"))) Then
                                            cellb(p) = rsp.Item("amt").ToString
                                            ' Response.Write(cellb(p))
                                            cellbval(p) = rsp.Item("id").ToString
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))
                                            max = p
                                            '  Response.Write(emptid & "=>" & reasonname(p) & "=" & rsp.Item("reason") & p & "<br>")
                                        ElseIf cellb(p) = 0 Then
                                            cellb(p) = 0
                                            cellbval(p) = 0
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))
                                            max = p
                                        End If

                                    Next

                                    ' Response.Write(sumloan(max + 1).ToString & "<br>")
                                    'cellb(j) = rs.Item("amt").ToString
                                    'cellbval(j) = rs.Item("id").ToString
                                    'sumloan(j) += CDbl(cellb(j))
                                    'inc = inc + CDbl(cellb(j))
                                    ' j += 1
                                End While
                                max = UBound(reasonname)
                                cellb(max) = fm.getinfo2("select sum(amt2) as exp2 from emp_inc WHERE emptid=" & emptid & " and amt2 > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "')", Session("con"))
                                ' Response.Write(cellb(max + 1).ToString & rsp.Item("emptid"))
                                If IsNumeric(cellb(max)) = False Then
                                    cellb(max) = 0
                                End If
                                cellbval(max) = rsp.Item("id").ToString
                                sumloan(max) += CDbl(cellb(max))
                                rsp.Close()
                            End If
                            cell(5) = inc + cellb(max)

                            ' Response.Write(cell(5).ToString & "===" & cell(3).ToString & "<br>")
                            cell(6) = CDbl(cell(5)) + CDbl(cell(3)) - cellb(max)
                            'cell(3) = 0
                            'Response.Write(()
                            'cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6))), 2).ToString
                            cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString
                            cell(8) = fm.getinfo2("select sum(tax) from payrollx where emptid=" & emptid & " and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "')", Session("con"))

                            If cell(8) = "None" Or cell(8) = "" Then
                                cell(8) = 0
                            End If

                            cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                            cell(10) = CDbl(cell(5)) - CDbl(cell(9))
                            If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                sumbsal += CDbl(cell(3))
                                'sumbearn += CDbl(cell(4))
                                suminc += CDbl(cell(5))
                                sumtaxinc += CDbl(cell(6))
                                sumtax += CDbl(cell(7))
                                sumptax += CDbl(cell(8))
                                sumtaxpay += CDbl(cell(9))
                                sumnet += CDbl(cell(10))

                            End If
                            color = "white"
                            outp &= "<tr>"
                            For i As Integer = 0 To 10
                                If CDbl(cell(3)) = 0 And i = 3 Then
                                    color = "red"
                                ElseIf CDbl(cell(8)) = 0 And i = 8 Then
                                    color = "red"
                                    color = "white"
                                End If


                                If i <> 4 Then
                                    If i >= 0 And i < 3 Then
                                        outp &= ("<td class='cell' style=''  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i) & "</td>")

                                    Else
                                        outp &= ("<td class='cell' style='background:" & color & ";text-align:right;'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    End If

                                Else
                                    For j = 0 To ccol - 1
                                        ' Response.Write(cellb(j) & "<><br>")
                                        outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-" & cellbval(j) & "'>&nbsp;" & FormatNumber(cellb(j).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    Next

                                    outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-inc-" & j + 1 & "'>&nbsp;" & FormatNumber(cellb(j + 1).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")


                                End If
                            Next
                            ' Response.Write(paid)
                            If paid = "None" Then
                                outp &= " <td class='cell'><input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked'></td>"
                            Else
                                If CDbl(cell(4)) <> 0 Then
                                    outp &= " <td class='cell'><input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked' disabled='disabled' style='visibility:hidden;'>Paid</td>"
                                Else
                                    outp &= " <td class='cell'><input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked' disabled='disabled' ></td>"

                                End If


                            End If
                            outp &= "<td class='cell'>&nbsp;</td></tr>" & Chr(13)
                            k += 1
                            If paid = "None" Then
                                Dim arrl As Integer
                                arrl = arremp.Length
                                ReDim Preserve arremp(arrl + 1)
                                arremp(arrl) = emptid.ToString
                            End If
                        End If
                    End While
                    outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                   "<td class='cell'>&nbsp;</td><td class='cell'>&nbsp;</td><td class='cell'>&nbsp;</td>" & _
                   "<td class='cell' style='text-align:right;'>" & FormatNumber(sumbsal.ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"
                    For j = 0 To ccol - 1
                        outp &= "<td class='cell' style='text-align:right;'>" & FormatNumber(sumloan(j), 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"
                    Next
                    outp &= "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumloan(UBound(sumloan)), 2).ToString & "</td>"

                    outp &= ("<td class='cell' style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>&nbsp</td></tr>")

                End If
            End If
            outp &= fm.signpart2()
            outp &= "</table>"
            outp &= fm.projtrans(Request.Form("projname"), pdate1, Session("con"))
        End If
        Response.Write(outp)
        ' Response.Write(ccol)

        Dim outpx As String = ""

        Return outp
    End Function

    Public Function makeform2_payroll(ByVal vx() As String) 'make statement

        Session("companyck") = "Net Consult P.L.C"

        Dim nod As Integer
        If Session("company_name") <> Session("companyck") Then
            Response.Redirect("logout.aspx?msg=copyright issue&msgtype=Caution&titlex=Caution")

        End If
        Dim pdate1, pdate2, pdate3, pdate4 As Date


        Dim fl As New file_list
        'Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double

        Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
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
        Dim ccol As Integer = 0
        If Request.QueryString("month") <> "" Then
            Dim spl() As String
            Dim projid As String
            If Request.QueryString("projname") <> "" Then
                ' Response.Write(Request.QueryString("projname"))
                spl = getprojname(Request.QueryString("projname")).Split("|")
                If spl.Length <= 1 Then
                    ReDim spl(2)
                    spl(0) = Request.QueryString("projname")
                    spl(1) = ""
                    Response.Write("Sorry! the Project is not selected Properly")
                End If
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            End If
            nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
            pdate3 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
            pdate4 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
            pdate1 = pdate3.AddMonths(-1)
            pdate2 = pdate1.Month & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year

            Dim rs As DataTableReader
            Dim rs2 As DataTableReader

            Dim ccol2 As Integer = 0
            Dim paid, paid2 As String
            Dim j As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            ' rs = dbs.dtmake("nuemp", "select * from emp_inc where paid_date between '" & pdate3 & "' and '" & pdate4 & "'", Session("con"))

            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If

            paid = ""
            paid = fm.getinfo2("select id from emp_inc where paidref is null", Session("con"))


            ccol = CInt(fm.getinfo2("SELECT COUNT(countx) AS Expr1 froM (SELECT distinct reason AS countx FROM emp_inc WHERE (amt + amt2)>0 AND " & _
                                    "(inc_date between '" & pdate3 & "' and '" & pdate4 & "' and paidref is null) and " & _
                                    "emptid in(select emptid from emp_job_assign " & _
                                                   "where project_id='" & projid.ToString & "' " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                                                   "GROUP BY reason) AS derivedtbl_1", Session("con")))
            ' Response.Write(ccol.ToString)
            If IsNumeric(ccol) = False Then
                ccol = 1
            End If
            ReDim cellb(ccol + 1)
            ReDim cellbval(ccol + 1)
            ReDim sumloan(ccol + 1)
            ReDim reasonname(ccol + 1)
            Dim tit As String = ""
            Dim toptit As String = ""
            rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE (amt + amt2)>0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and (paidref is null or paidref='') and " & _
                                 " emptid in(select emptid from emp_job_assign " & _
                                                   "where project_id='" & projid.ToString & "' " & _
                                                   " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                                                   " GROUP BY reason", Session("con"))

            tit &= "<tr>" & Chr(13)
            ' rs.Close()
            If rs2.HasRows Then
                Dim i As Integer = 0
                While rs2.Read

                    If rs2.Item("reason") = "-" Then
                        tit &= "<td >Other</td>"
                    Else
                        tit &= "<td >" & rs2.Item("reason").ToString & "</td>"
                    End If
                    reasonname(i) = rs2.Item("reason")
                    ' Response.Write(reasonname(i) & "i<br>")
                    i += 1

                End While

                reasonname(i) = "Non-Taxable"
                tit &= "<td >Non-Taxable</td>"
            End If

            For l As Integer = 0 To ccol - 1
                If l = ccol - 1 Then
                    If toptit = "" Then
                        toptit &= reasonname(l)
                    Else
                        toptit = toptit.Substring(0, toptit.Length - 1) & " and " & reasonname(l)
                    End If

                Else
                    toptit &= reasonname(l) & ", "
                End If

            Next
            strc = "0"
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                Try
                    ' rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                    '                               "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                    '                              "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                    '                             "and emprec.id in(select emptid from emp_job_assign " & _
                    ''                            "where project_id='" & projid.ToString & "' " & _
                    ''                          "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                    '                        " ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                    ' rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
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
            'Response.Write("....")
            If rs.HasRows Then
                outp = "<style>" & _
             "#tb1 " & _
             "{" & _
                 "border:1px solid black;" & _
                 "font-size:9pt;" & _
             "}" & _
           " #tb1 td" & _
             "{" & _
              "border-top: 1px solid black;" & _
             " border-left:1px solid black;" & _
                 "font-size:9pt;" & _
             "}" & _
            " </style>"
                outp &= "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='" & (12 + CInt(ccol)).ToString & " ' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br>" & toptit & " payment Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)
                payrdate = pdate2
                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp. Id.</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitx'   colspan='" & (CInt(ccol) + 1).ToString & "'><center>Increament</center></td>"
                outp &= "<td class='fitx' rowspan='2'>Total Increament</td>"
                outp &= "<td class=''fitx' rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitx'  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Tax Paid</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td class='fitx'  rowspan='2'>Net pay</td>"
                outp &= "<td class='fitx' rowspan='2'>Signature</td></tr>" & Chr(13)
                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

                rs.Close()
                outp &= tit

                outp &= "</tr>" & Chr(13)
                rs2.Close()
                rs2 = dbs.dtmake("dbpen", "SELECT *  FROM emp_inc inner join emprec on emprec.id=emp_inc.emptid inner join emp_static_info as esi " & _
                                                " on esi.emp_id=emprec.emp_id WHERE (amt+amt2) > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & _
                                                "') and paidref is null order by esi.first_name", Session("con"))
                Dim arremp(1) As String
                arremp(0) = ""
                If rs2.HasRows Then
                    Dim k As Integer = 1
                    Dim color As String = ""
                    Dim resing As Date
                    While rs2.Read
                        emptid = rs2.Item("emptid")
                        If rs2.IsDBNull(7) = True Then
                            paid = "None"

                        Else
                            paid = rs2.Item("paidref")
                        End If
                        Dim prj As String
                        ' prj = fm.getinfo2("select project_id from emp_job_assign where emptid=" & emptid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))", Session("con"))
                        ' Response.Write(fm.searcharray(vx, emptid.ToString) & "<br>")
                        prj = fm.getinfo2("select project_id from emp_job_assign where emptid=" & emptid & " and '" & pdate1 & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')", Session("con"))

                        If fm.searcharray(arremp, emptid.ToString) = False And fm.searcharray(vx, emptid.ToString) And prj = projid Then
                            'Response.Write(fm.searcharray(vx, emptid.ToString) & "<br>")
                            'Response.Write(emptid & "<br>")
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                            End If

                            cell(0) = k
                            cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                            cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                            ' cell(3)=fm.getinfo2("select sum(gross_earn) as Exp2 from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month=" 
                            cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString
                            '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")

                            'cell(3) = "0"
                            If cell(3) = "None" Or cell(3).ToString.Length > 10 Or cell(3) = "" Then
                                Response.Write(cell(3))
                                cell(3) = 0
                            End If
                            Dim otamtpaid As Double
                            otamtpaid = getotpaidin(pdate1, pdate2, emptid, Session("con"))
                            'Response.Write(mtx.ToString)

                            cell(3) = CDbl(cell(3)) + otamtpaid

                            Dim rsp As DataTableReader
                            rsp = dbs.dtmake("dbx", "SELECT *  FROM emp_inc WHERE emptid=" & emptid & " and (amt + amt2)>0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is null", Session("con"))
                            j = 0
                            inc = 0
                            For p As Integer = 0 To reasonname.Length - 1
                                cellb(p) = 0
                                cellbval(p) = 0
                            Next
                            Dim max As Integer = 0
                            If rsp.HasRows Then
                                While rsp.Read
                                    '  Response.Write("llllooopppp" & rs.Item("emptid"))
                                    For p As Integer = 0 To reasonname.Length - 1

                                        If LCase(Trim(reasonname(p))) = LCase(Trim(rsp.Item("reason"))) Then
                                            cellb(p) = rsp.Item("amt").ToString
                                            ' Response.Write(cellb(p))
                                            cellbval(p) = rsp.Item("id").ToString
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))
                                            max = p
                                            ' Response.Write(rs.Item("emptid") & "=>" & reasonname(p) & "=" & rs.Item("reason") & p & "<br>")
                                        ElseIf cellb(p) = 0 Then
                                            cellb(p) = 0
                                            cellbval(p) = 0
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))
                                            max = p
                                        End If

                                    Next

                                    ' Response.Write(sumloan(max + 1).ToString & "<br>")
                                    'cellb(j) = rs.Item("amt").ToString
                                    'cellbval(j) = rs.Item("id").ToString
                                    'sumloan(j) += CDbl(cellb(j))
                                    'inc = inc + CDbl(cellb(j))
                                    ' j += 1
                                End While
                                max = UBound(reasonname)
                                cellb(max) = fm.getinfo2("select sum(amt2) as exp2 from emp_inc WHERE emptid=" & emptid & " and amt2 > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "')", Session("con"))
                                ' Response.Write(cellb(max + 1).ToString & rsp.Item("emptid"))
                                If IsNumeric(cellb(max)) = False Then
                                    cellb(max) = 0
                                End If
                                cellbval(max) = rsp.Item("id").ToString
                                sumloan(max) += CDbl(cellb(max))
                                rsp.Close()
                            End If
                            cell(5) = inc + cellb(max)

                            ' Response.Write(cell(5).ToString & "===" & cell(3).ToString & "<br>")
                            cell(6) = CDbl(cell(5)) + CDbl(cell(3)) - cellb(max)
                            'cell(3) = 0
                            'Response.Write(()
                            ' cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6))), 2).ToString
                            cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString
                            cell(8) = fm.getinfo2("select sum(tax) from payrollx where emptid=" & emptid & " and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "')", Session("con"))

                            If cell(8) = "None" Or cell(8) = "" Then
                                cell(8) = 0
                            End If

                            cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                            cell(10) = CDbl(cell(5)) - CDbl(cell(9))
                            If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                sumbsal += CDbl(cell(3))
                                'sumbearn += CDbl(cell(4))
                                suminc += CDbl(cell(5))
                                sumtaxinc += CDbl(cell(6))
                                sumtax += CDbl(cell(7))
                                sumptax += CDbl(cell(8))
                                sumtaxpay += CDbl(cell(9))
                                sumnet += CDbl(cell(10))

                            End If
                            outp &= "<tr>"
                            For i As Integer = 0 To 10

                                If i <> 4 Then
                                    If i >= 0 And i < 3 Then
                                        outp &= ("<td class='cell' style=''  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i) & "</td>")

                                    Else
                                        outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    End If

                                Else
                                    For j = 0 To ccol - 1
                                        ' Response.Write(cellb(j) & "<><br>")
                                        outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-" & cellbval(j) & "'>" & FormatNumber(cellb(j).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    Next

                                    outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-inc-" & j + 1 & "'>&nbsp;" & FormatNumber(cellb(j + 1).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")


                                End If
                            Next
                            ' Response.Write(paid)

                            outp &= "<td class='cell'>&nbsp;</td></tr>" & Chr(13)
                            k += 1
                            If paid = "None" Then
                                Dim arrl As Integer
                                arrl = arremp.Length
                                ReDim Preserve arremp(arrl + 1)
                                arremp(arrl) = emptid.ToString
                            End If
                        End If
                    End While
                    outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                   "<td class='cell'>&nbsp;</td><td class='cell'>&nbsp;</td><td class='cell'>&nbsp;</td>" & _
                   "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"
                    For j = 0 To ccol - 1
                        outp &= "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumloan(j), 2).ToString & "</td>"
                    Next
                    outp &= "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumloan(UBound(sumloan)), 2).ToString & "</td>"

                    outp &= ("<td class='cell' style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>&nbsp</td></tr>")

                End If
            End If
            outp &= fm.signpart2()
            outp &= "</table>"
            outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))
        End If
        Response.Write(outp)
        ' Response.Write(ccol)

        Dim outpx As String = ""

        Return outp

    End Function

    Public Function makeform2v_payroll(ByVal vx() As String)
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
                ' ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vwloanbal WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

            Else
                ' ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vwloanbal WHERE bal>0 and ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

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
                'rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                Try
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                   "where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                   "and (emprec.id in(select emptid from emp_job_assign " & _
                                                   "where (project_id='" & projid.ToString & "') " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))))" & _
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
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

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
    Public Function makeform3_payroll(ByVal vx() As String)
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
                ' ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

            Else
                ' ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

            End If
            ' Response.Write(ccol)
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim stylew(4) As String
            Dim tcell As Integer
            tcell = ccol + 14

            Dim wdthpay As Integer = 1250
            tcell = ccol + 14
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
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                ' rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                Try
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                   "where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                   "and (emprec.id in(select emptid from emp_job_assign " & _
                                                   "where (project_id='" & projid.ToString & "') " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))))" & _
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
                outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
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
                outp &= "<td rowspan='2' style='width:" & stylew(0).ToString & "'>No.</td><td style='width:" & stylew(1).ToString & ";' rowspan='2'>Emp. Id</td>"
                outp &= "<td style='width:" & stylew(2) & ";' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                outp &= "<td  style='width:30px;' rowspan='2'>Days Worked</td>"
                outp &= "<td  class='fitx' rowspan='2'>Basic Earning</td>"
                outp &= "<td  class='fitx' rowspan='2'>Taxable Allowance</td>"
                outp &= "<td  class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Overtime</td>"
                outp &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Net Pay</td>"


                outp &= "<td  id='chkall' style='cursor:pointer;width:" & stylew(3).ToString & ";' rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

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
                            ' sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            ' Response.Write(sql & "<br>")
                            sal = dbs.getsal(emptid, pdate1, Session("con"))
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

                            cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            ElseIf IsNumeric(cell(5)) = False Then
                                Response.Write(cell(5).ToString & "<br>")
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
                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            'cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
                            ' Response.Write(cell(10) & "<br>")
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
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

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
                                outp &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'>"
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
                                outp &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If
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
                outp &= "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
                "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
                  "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                  "<td class='cooo' colspan='2'>..&nbsp;" & _
     "</td></tr><tr id='result'></tr>"
                outp &= "</table>"
                outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

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
           "$(function() {$( '#paydx').datepicker({changeMonth: true,changeYear: true	});" & Chr(13) & " $( '#paydx' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                    "  <input type='button' id='post' onclick='javascript:findid()' name='post' value='Paid' />"

            Response.Write(xprint)


            Return outp

        End If
    End Function
    Public Function makeform3_payroll_()

        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

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
                'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

            Else
                'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT vwloanbal.emptid as cnt, vwloanbal.reason FROM vwloanbal INNER JOIN " & _
                                      "emp_job_assign ON vwloanbal.emptid = emp_job_assign.emptid where vwloanbal.bal>0 and '" & pdate1 & _
                                      "' between emp_job_assign.date_from and isnull(emp_job_assign.date_end,'" & pdate2 & "') and vwloanbal.dstart<='" & _
                                      pdate1 & "'  and emp_job_assign.project_id='" & projid & "') AS derivedtbl_1", Session("con")))

            End If
            'Response.Write(ccol)
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim stylew(4) As String
            Dim tcell As Integer
            Dim wdthpay As Integer = 1250
            tcell = ccol + 14
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
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.end_date between '" & pdate2 & "' and '" & pdate1 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                ' rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.end_date between '" & pdate1 & "' and '" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                Try
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                   "where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                   "and (emprec.id in(select emptid from emp_job_assign " & _
                                                   "where (project_id='" & projid.ToString & "') " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))))" & _
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
                outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
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
                outp &= "<td rowspan='2' style='width:" & stylew(0).ToString & "'>No.</td><td style='width:" & stylew(1).ToString & ";' rowspan='2'>Emp. Id</td>"
                outp &= "<td style='width:" & stylew(2) & ";' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                outp &= "<td  style='width:30px;' rowspan='2'>Days Worked</td>"
                outp &= "<td  class='fitx' rowspan='2'>Basic Earning</td>"
                outp &= "<td  class='fitx' rowspan='2'>Taxable Allowance</td>"
                outp &= "<td  class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Overtime</td>"
                outp &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Net Pay</td>"


                outp &= "<td  id='chkall' style='cursor:pointer;width:" & stylew(3).ToString & ";' rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                outp &= "<td rowspan='2' class='signpart'>Signature</td>" & Chr(13)

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
                Dim did As String
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
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))

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
                    If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year Then
                        'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                        incwhen = dateincr.Subtract(pdate1).Days
                    End If
                    If ((resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing) And incwhen = 0 Then 'question..........solved
                        cell(0) = rs.Item("emp_id")
                        cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                        'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                        ' Response.Write(sql & "<br>")
                        sal = dbs.getsal(emptid, pdate1, Session("con"))
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

                        'Response.Write(calc.ToString)

                        cell(3) = Math.Round(calc, 2).ToString
                        'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                        cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString ' God does this
                        'cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                        ' Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)


                        cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                        '  Response.Write(cell(5).ToString)
                        ' cell(5) = "0"
                        If cell(5).ToString = "None" Then
                            cell(5) = "0"
                        ElseIf cell(5).ToString = "" Then
                            cell(5) = "0"
                        ElseIf IsNumeric(cell(5)) = False Then
                            Response.Write(cell(5).ToString & "<br>")
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
                        cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                        ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                        '   Response.Write(cell(7).ToString & "<br>")
                        cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                        'Response.Write(cell(8).ToString & "<br>")
                        cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                        'cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
                        ' Response.Write(cell(10) & "<br>")
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
                                        outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                    Else
                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    End If
                                Else
                                    For j = 0 To ccol - 1
                                        If String.IsNullOrEmpty(cellbval(j)) = False Then
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                        Else
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
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
                            outp &= "<td style='text-align:right;'>"
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
                End While


                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                "<td class='cooo' style=''>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
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
                  "<td class='cooo' colspan='2'>..&nbsp;" & _
     "</td></tr><tr id='result'></tr>"
                outp &= "</table>"
                outp &= fm.projtrans(Request.QueryString("projname"), pdate1, Session("con"))

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
    Function deleteall_payroll()
        Dim spl() As String = Request.Form("delpass").Split("⌡")
        Dim spl2() As String
        Dim dbs As New dbclass
        Dim flg As String = ""
        Dim sqlst As String = "BEGIN TRANSACTION" & Chr(13)
        For i As Integer = 0 To spl.Length - 1
            If String.IsNullOrEmpty(spl(i)) = False Then
                spl2 = spl(i).Split(",")
                For j As Integer = 0 To spl2.Length - 1
                    If String.IsNullOrEmpty(spl2(j)) = False Then
                        If i = 0 Then
                            sqlst &= "delete from paryrol where id=" & spl2(j) & Chr(13)
                        ElseIf i = 1 Then
                            sqlst &= "delete from emp_loan_settlement where id=" & spl2(j) & Chr(13)
                        ElseIf i = 2 And spl2(j) <> "None" Then
                            sqlst &= "Update emp_ot set paidstatus='n',ref=NULL where id=" & spl2(j) & Chr(13)
                        End If

                    End If
                Next
            End If
        Next
        ' sqlst &= "COMMIT" & Chr(13)
        'Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
        flg = dbs.excutes(sqlst, Session("con"), session("path"))
        If IsNumeric(flg) = True Then
            ' Response.Write(flg)
            If CInt(flg) <= 0 Then
                dbs.save("rollback", Session("con"), session("path"))
                Response.Write("data is not saved")
            Else
                dbs.save("COMMIT", Session("con"), session("path"))
                Response.Write("Data Saved")
            End If



        End If


    End Function
    Function deleteallx_payroll(ByVal ref As String)

        Dim dbs As New dbclass
        Dim flg As String = ""
        Dim sqlst As String = "BEGIN TRANSACTION" & Chr(13)


        sqlst &= "delete from payrollx where ref='" & ref & "'" & Chr(13)

        sqlst &= "delete from emp_loan_settlement where ref='" & ref & "'" & Chr(13)

        sqlst &= "Update emp_ot set paidstatus='n',ref=NULL where ref='" & ref & "'" & Chr(13)
        sqlst &= "Update emp_inc set paidref=NULL where paidref='" & ref & "'" & Chr(13)
        sqlst &= "COMMIT" & Chr(13)
        'Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
        flg = dbs.excutes(sqlst, Session("con"), session("path"))
        If IsNumeric(flg) = True Then
            ' Response.Write(flg)
            If CInt(flg) <= 0 Then
                dbs.excutes("RollBack", Session("con"), session("path"))
                Response.Write("data is not saved")
            Else
                'dbs.save("COMMIT", Session("con"),session("path"))
                Response.Write("Data Saved")
            End If



        End If

        dbs = Nothing
        Return Nothing
    End Function
    Public Function getallowance(ByVal emptid As String, ByVal altype As String, ByVal pdate1 As Date, ByVal con As SqlConnection)
        Dim nod As Integer
        Dim rtn(2) As String
        Dim rpt As String = ""
        Dim d2 As Date
        Dim amt As Double
        nod = Date.DaysInMonth(pdate1.Year, pdate1.Month)
        d2 = pdate1.Month & "/" & nod.ToString & "/" & pdate1.Year
        Dim dvar1, dvar2 As Integer
        Dim rt As String = ""
        Dim rs As DataTableReader
        Dim dbx As New dbclass
        amt = 0
        rpt = ""
        rs = dbx.dtmake(altype, "select * from emp_alloance_rec where to_date between '" & pdate1 & "' and '" & d2 & "' and istaxable='" & altype & "' and emptid='" & emptid & "'", con)
        If rs.HasRows Then
            'rpt &= "has rows"
            While rs.Read
                dvar1 = rs.Item("to_date").Subtract(pdate1).days + 1
                ' rpt &= "has rows" & dvar1.ToString & "<br>"
                If dvar1 > 0 Then
                    rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allownace_type") & " has been closed on:" & rs.Item("from_date") & " and emptid='" & emptid & "'<br>"
                    amt += (CDbl(rs.Item("amount")) / nod) * dvar1
                End If
            End While
        End If
        rs.Close()
        Response.Write(rpt)
        If rpt = "" Then
            rs = dbx.dtmake(altype, "select * from emp_alloance_rec where to_date is null and (from_date between '" & pdate1 & "' and isnull(to_date,'" & d2 & "') or '" & pdate1 & "' between from_date and isnull(to_date,'" & pdate1 & "')) and  (emptid=" & emptid & ") and (istaxable='" & altype & "')", con)
            If rs.HasRows Then
                While rs.Read
                    If CDate(rs.Item("from_date")).Month = pdate1.Month And CDate(rs.Item("from_date")).Year = pdate1.Year Then
                        dvar1 = CDate(rs.Item("from_date")).Subtract(d2).Days
                        If dvar1 < 0 Then
                            dvar1 = (dvar1 * -1) + 1
                        End If
                        rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allownace_type") & " has been started on:" & rs.Item("from_date") & " and emptid='" & emptid & "'<br>"
                        Response.Write(dvar1.ToString)
                    Else
                        dvar1 = nod
                    End If

                    If dvar1 > 0 Then
                        amt += (CDbl(rs.Item("amount")) / nod) * dvar1
                    End If
                End While
            End If

            rs.Close()
        End If
        dbx = Nothing
        rtn(0) = amt
        rtn(1) = rpt
        Return rtn
    End Function
    Public Function getallowance(ByVal emptid As String, ByVal altype As String, ByVal pdate1 As Date, ByVal calc As Double, ByVal con As SqlConnection)
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
        rs = dbx.dtmake(altype, "select * from emp_alloance_rec where to_date between '" & pdate1 & "' and '" & d2 & "' and istaxable='" & altype & "' and emptid='" & emptid & "'", con)
        If rs.HasRows Then
            'rpt &= "has rows"
            While rs.Read
                d3 = rs.Item("to_date")
                dvar1 = rs.Item("to_date").Subtract(pdate1).days + 1
                ca = CDbl(fm.getinfo2("select count(id) from emp_att where emptid=" & rs.Item("id") & " and att_date between '" & pdate1 & "' and '" & d3 & "' and status='A'", Session("con")))
                'Response.Write(ca.ToString & "<==a")
                clwp = Math.Round(CDbl(fm.lwpinmonth(pdate1, d3, emptid, Session("con"))), 2)
                dvar2 = dvar1 - ca - clwp
                ' rpt &= "has rows" & dvar1.ToString & "<br>"
                If dvar1 > 0 Then
                    rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allownace_type") & " has been closed on:" & rs.Item("from_date") & " and emptid='" & emptid & "'<br>"
                    amt += (CDbl(rs.Item("amount")) / nod) * dvar2
                End If
            End While
        End If
        rs.Close()
        ' Response.Write(rpt)

        rs = dbx.dtmake(altype, "select * from emp_alloance_rec where to_date is null and (from_date between '" & pdate1 & "' and isnull(to_date,'" & d2 & "') or '" & pdate1 & "' between from_date and isnull(to_date,'" & pdate1 & "')) and  (emptid=" & emptid & ") and (istaxable='" & altype & "')", con)

        If rs.HasRows Then
            While rs.Read
                dvar1 = CDate(rs.Item("from_date")).Subtract(d2).Days
                ca = CDbl(fm.getinfo2("select count(id) from emp_att where emptid=" & rs.Item("id") & " and att_date between '" & pdate1 & "' and '" & d2 & "' and status='A'", Session("con")))
                'Response.Write(ca.ToString & "<==a")
                clwp = Math.Round(CDbl(fm.lwpinmonth(pdate1, d2, emptid, Session("con"))), 2)
                If CDate(rs.Item("from_date")).Month = pdate1.Month And CDate(rs.Item("from_date")).Year = pdate1.Year Then

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
                End If
            End While
        End If

        rs.Close()

        dbx = Nothing
        rtn(0) = amt
        rtn(1) = rpt
        rtn(2) = dvar1
        Return rtn
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
        Session("con").close()
        Session("con").open()
    End Sub
    Function getprojname(ByVal pname As String)
        Dim sec As New k_security
        pname = sec.dbHexToStr(pname)
        Return pname
    End Function
    '----------------------------------------------///////Down old version-------------------------------------------------------
    Public namelsit As String
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
    Function unpaidlist()
        Dim pdate1, pdate2, pdate3, pdate4 As Date
        Dim nod As Integer

        Dim fl As New file_list
        'Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double

        Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
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
        Dim ccol As Integer = 0
        If Request.Form("month") <> "" Then
            Dim spl() As String
            Dim projid As String
            If Request.Form("projname") <> "" Then
                spl = Request.Form("projname").Split("|")
                If spl.Length <= 1 Then
                    ReDim spl(2)
                    spl(0) = Request.Form("projname")
                    spl(1) = ""
                    Response.Write("Sorry! the Project is not selected Properly")
                End If
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            End If
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate3 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate4 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            pdate1 = pdate3.AddMonths(-1)
            pdate2 = pdate4.AddMonths(-1)
            Dim rs As DataTableReader
            Dim rs2 As DataTableReader

            Dim ccol2 As Integer = 0
            Dim paid, paid2 As String
            Dim j As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            ' rs = dbs.dtmake("nuemp", "select * from emp_inc where paid_date between '" & pdate3 & "' and '" & pdate4 & "'", Session("con"))

            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If

            paid = ""
            paid = fm.getinfo2("select id from emp_inc where paidref is null", Session("con"))


            ccol = CInt(fm.getinfo2("SELECT COUNT(countx) AS Expr1 froM (SELECT distinct reason AS countx FROM emp_inc WHERE (amt + amt2)>0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "' and paidref is null) GROUP BY reason) AS derivedtbl_1", Session("con")))
            ' Response.Write(ccol.ToString)
            If IsNumeric(ccol) = False Then
                ccol = 1
            End If
            ReDim cellb(ccol + 1)
            ReDim cellbval(ccol + 1)
            ReDim sumloan(ccol + 1)
            ReDim reasonname(ccol + 1)

            strc = "0"
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                Try
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                   "where ( '" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                   "and (emprec.id in(select emptid from emp_job_assign " & _
                                                   "where (project_id='" & projid.ToString & "') " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))))" & _
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
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc <br>")
            End If
            'Response.Write("....")
            If rs.HasRows Then
                outp = "<style>" & _
             "#tb1 " & _
             "{" & _
                 "border:1px solid black;" & _
                 "font-size:9pt;" & _
             "}" & _
           " #tb1 td" & _
             "{" & _
              "border-top: 1px solid black;" & _
             " border-left:1px solid black;" & _
                 "font-size:9pt;" & _
             "}" & _
            " </style>"
                outp &= "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='13' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br>Incentive(s) payment Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp. Id.</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitx'   colspan='" & (CInt(ccol) + 1).ToString & "'><center>Increament</center></td>"
                outp &= "<td class='fitx' rowspan='2'>Total Increament</td>"
                outp &= "<td class=''fitx' rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitx'  rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Tax Paid</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td class='fitx'  rowspan='2'>Net pay</td>"
                outp &= "<td class='dw'  id='chkall' style='cursor:pointer'rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Signature</td></tr>" & Chr(13)
                rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE (amt + amt2)>0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and (paidref is null or paidref='')", Session("con"))

                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)
                rs.Close()
                If rs2.HasRows Then
                    Dim i As Integer = 0
                    While rs2.Read

                        If rs2.Item("reason") = "-" Then
                            outp &= "<td >Other</td>"
                        Else
                            outp &= "<td >" & rs2.Item("reason").ToString & "</td>"
                        End If
                        reasonname(i) = rs2.Item("reason")
                        ' Response.Write(reasonname(i) & "i<br>")
                        i += 1

                    End While
                    reasonname(i) = "Non-Taxable"
                    outp &= "<td >Non-Taxable</td>"
                End If

                outp &= "</tr>" & Chr(13)
                rs2.Close()
                rs2 = dbs.dtmake("dbpen", "SELECT *  FROM emp_inc WHERE (amt + amt2)>0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is null", Session("con"))
                Dim arremp(1) As String
                arremp(0) = ""
                If rs2.HasRows Then
                    Dim k As Integer = 1
                    Dim color As String = ""
                    Dim resing As Date
                    While rs2.Read

                        emptid = rs2.Item("emptid")
                        If rs2.IsDBNull(7) = True Then
                            paid = "None"

                        Else
                            paid = rs2.Item("paidref")
                        End If
                        If fm.searcharray(arremp, emptid.ToString) = False Then
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                            End If

                            cell(0) = k
                            cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                            cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                            ' cell(3)=fm.getinfo2("select sum(gross_earn) as Exp2 from paryrol where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month=" 
                            cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))", Session("con")).ToString
                            '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")

                            'cell(3) = "0"
                            If cell(3) = "None" Or cell(3).ToString.Length > 10 Or cell(3) = "" Then
                                cell(3) = 0
                            End If
                            Dim rsp As DataTableReader
                            rsp = dbs.dtmake("dbx", "SELECT *  FROM emp_inc WHERE emptid=" & emptid & " and (amt + amt2)>0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "')", Session("con"))
                            j = 0
                            inc = 0
                            For p As Integer = 0 To reasonname.Length - 1
                                cellb(p) = 0
                                cellbval(p) = 0
                            Next
                            Dim max As Integer = 0
                            If rsp.HasRows Then
                                While rsp.Read
                                    '  Response.Write("llllooopppp" & rs.Item("emptid"))
                                    For p As Integer = 0 To reasonname.Length - 1

                                        If reasonname(p) = rsp.Item("reason") Then
                                            cellb(p) = rsp.Item("amt").ToString
                                            ' Response.Write(cellb(p))
                                            cellbval(p) = rsp.Item("id").ToString
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))
                                            max = p
                                            ' Response.Write(rs.Item("emptid") & "=>" & reasonname(p) & "=" & rs.Item("reason") & p & "<br>")
                                        ElseIf cellb(p) = 0 Then
                                            cellb(p) = 0
                                            cellbval(p) = 0
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))
                                            max = p
                                        End If

                                    Next

                                    ' Response.Write(sumloan(max + 1).ToString & "<br>")
                                    'cellb(j) = rs.Item("amt").ToString
                                    'cellbval(j) = rs.Item("id").ToString
                                    'sumloan(j) += CDbl(cellb(j))
                                    'inc = inc + CDbl(cellb(j))
                                    ' j += 1
                                End While
                                max = UBound(reasonname)
                                cellb(max) = fm.getinfo2("select sum(amt2) as exp2 from emp_inc WHERE emptid=" & emptid & " and amt2 > 0 AND (inc_date between '" & pdate3 & "' and '" & pdate4 & "')", Session("con"))
                                ' Response.Write(cellb(max + 1).ToString & rsp.Item("emptid"))
                                If IsNumeric(cellb(max)) = False Then
                                    cellb(max) = 0
                                End If
                                cellbval(max) = rsp.Item("id").ToString
                                sumloan(max) += CDbl(cellb(max))
                                rsp.Close()
                            End If
                            cell(5) = inc + cellb(max)

                            ' Response.Write(cell(5).ToString & "===" & cell(3).ToString & "<br>")
                            cell(6) = CDbl(cell(5)) + CDbl(cell(3)) - cellb(max)
                            'cell(3) = 0
                            'Response.Write(()
                            ' cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6))), 2).ToString
                            cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString
                            cell(8) = fm.getinfo2("select sum(tax) from payrollx where emptid=" & emptid & " and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "')", Session("con"))

                            If cell(8) = "None" Or cell(8) = "" Then
                                cell(8) = 0
                            End If

                            cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                            cell(10) = CDbl(cell(5)) - CDbl(cell(9))
                            If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                sumbsal += CDbl(cell(3))
                                'sumbearn += CDbl(cell(4))
                                suminc += CDbl(cell(5))
                                sumtaxinc += CDbl(cell(6))
                                sumtax += CDbl(cell(7))
                                sumptax += CDbl(cell(8))
                                sumtaxpay += CDbl(cell(9))
                                sumnet += CDbl(cell(10))

                            End If
                            outp &= "<tr>"
                            For i As Integer = 0 To 10

                                If i <> 4 Then
                                    If i >= 0 And i < 3 Then
                                        outp &= ("<td class='cell' style=''  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i) & "</td>")

                                    Else
                                        outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    End If

                                Else
                                    For j = 0 To ccol - 1
                                        ' Response.Write(cellb(j) & "<><br>")
                                        outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-" & cellbval(j) & "'>&nbsp;" & FormatNumber(cellb(j).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    Next

                                    outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-inc-" & j + 1 & "'>&nbsp;" & FormatNumber(cellb(j + 1).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")


                                End If
                            Next
                            ' Response.Write(paid)
                            If paid = "None" Then
                                outp &= " <td class='cell'><input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked'></td>"
                            Else
                                If CDbl(cell(4)) <> 0 Then
                                    outp &= " <td class='cell'><input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked' disabled='disabled' style='visibility:hidden;'>Paid</td>"
                                Else
                                    outp &= " <td class='cell'><input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked' disabled='disabled' ></td>"

                                End If


                            End If
                            outp &= "<td class='cell'>&nbsp;</td></tr>" & Chr(13)
                            k += 1
                            If paid = "None" Then
                                Dim arrl As Integer
                                arrl = arremp.Length
                                ReDim Preserve arremp(arrl + 1)
                                arremp(arrl) = emptid.ToString
                            End If
                        End If
                    End While
                    outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                   "<td class='cell'>&nbsp;</td><td class='cell'>&nbsp;</td><td class='cell'>&nbsp;</td>" & _
                   "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"
                    For j = 0 To ccol - 1
                        outp &= "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumloan(j), 2).ToString & "</td>"
                    Next
                    outp &= "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumloan(UBound(sumloan)), 2).ToString & "</td>"

                    outp &= ("<td class='cell' style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                     "<td class='cell' style='text-align:right;'>&nbsp</td><td class='cell' style='text-align:right;'>&nbsp</td>")

                End If
            End If
            outp &= signpart()
            outp &= "</tr></table>"
            outp &= fm.projtrans(Request.Form("projname"), pdate1, Session("con"))
        End If
        Response.Write(outp)
        ' Response.Write(ccol)

        Dim outpx As String = ""
        outpx = "payroll Date<input type='text' name='payd' id='payd' value='" & Today.ToShortDateString & "' /> Pay Method: <input type='text' name='paymth' id='paymth' value='Bank' />" & Chr(13) & _
          "<script language='javascript' type='text/javascript'>//sumcolx(); " & Chr(13) & _
          "$(function() {$( '#payd').datepicker({changeMonth: true,changeYear: true,maxDate:'+1M'	});" & Chr(13) & " $( '#payd' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                   "Paid Date:<input type='text' name='ppdate' id='ppdate' value='" & _
                   Today.ToShortDateString & "'>" & _
                              "<script language='javascript' type='text/javascript'>//sumcolx(); " & Chr(13) & _
          "$(function() {$( '#ppdate').datepicker({changeMonth: true,changeYear: true,maxDate:'+1M'	});" & Chr(13) & " $( '#ppdate' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                   "<input type='button' id='post' onclick='javascript:findidinc()' name='post' value='Paid/Payroll Date' />"

        ' outpx = "<input type='text' name='payd' id='payd' value='" & Today.ToShortDateString & "' />Pay Method: <input type='text' name='paymth' id='paymth' value='Cheque' />" & _
        '"<script language='javascript' type='text/javascript'> " & _
        '"$(function() {$( '#payd').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	}); $( '#payd' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
        '    "  <input type='button' id='post' onclick='javascript:findidinc()' name='post' value='Paid' /></form>"

        Response.Write(outpx)
        Return outp
    End Function

    Function paidlist()
        Dim pdate1, pdate2, pdate3, pdate4 As Date
        Dim nod As Integer

        Dim fl As New file_list
        'Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double

        Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
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
        Dim ccol As Integer = 0
        outp = ""
        'Dim ccol As Integer = 0
        ccol = 0
        sumbsal = 0

        suminc = 0
        sumtaxinc = 0
        sumtax = 0

        sumptax = 0
        sumtaxpay = 0
        sumnet = 0
        If Request.Form("month") <> "" Then
            Dim spl() As String
            Dim projid As String
            If Request.Form("projname") <> "" Then
                spl = Request.Form("projname").Split("|")
                If spl.Length <= 1 Then
                    ReDim spl(2)
                    spl(0) = Request.Form("projname")
                    spl(1) = ""
                End If
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            End If
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate3 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate4 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            pdate1 = pdate3.AddMonths(-1)
            pdate2 = pdate4.AddMonths(-1)
            Dim rs As DataTableReader
            Dim rs2 As DataTableReader

            Dim ccol2 As Integer = 0
            Dim paid, paid2 As String
            Dim j As Integer
            Dim reasonname() As String
            Dim paypaid As Integer = 0
            ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
            rs = dbs.dtmake("nuemp", "select * from emp_inc where paid_date between '" & pdate3 & "' and '" & pdate4 & "'", Session("con"))

            If paypaid <> 0 Then
                ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

            End If

            paid = ""
            paid = fm.getinfo2("select id from emp_inc where paidref is null", Session("con"))


            ccol = CInt(fm.getinfo2("SELECT COUNT(countx) AS Expr1 froM (SELECT distinct reason AS countx FROM emp_inc WHERE (amt + amt2)>0 AND (paid_date between '" & pdate3 & "' and '" & pdate4 & "' and paidref is not null) GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)

            strc = "0"
            If Request.Form("projname") = "" Then
                rs = dbs.dtmake("db", "select * from emprec where hire_date<='" & pdate4 & "' order by id desc", Session("con"))
            Else
                rs = dbs.dtmake("selectemp", "select * from emprec where hire_date<='" & pdate4 & "' and id in(select emptid from emp_job_assign where project_id='" & projid & "' and end_date is null) and end_date is null ", Session("con"))
            End If

            If rs.HasRows Then

                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='12' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br>Incentive(s) payment Sheet for the month: " & MonthName(pdate4.Month) & " " & pdate4.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr class='dw' style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2'>No.</td><td style='width:60px;' rowspan='2'>Emp.id</td>"
                outp &= "<td class='fxname' style='width:250px;' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                outp &= "<td class='fitx'   colspan='" & ccol.ToString & "'>Increament</td>"
                outp &= "<td class='fitx'  rowspan='2'>Total Increament</td>"
                outp &= "<td class='fitx'  rowspan='2'>Taxable Income</td>"
                outp &= "<td class='fitx'   rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td  class='fitx' rowspan='2'> Tax Paid</td>"
                outp &= "<td  class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                outp &= "<td  class='fitx' rowspan='2'>Net pay</td>"
                outp &= "<td  class='fitx' rowspan='2'>Signature</td></tr>" & Chr(13)
                rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE (amt + amt2)>0 AND (paid_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is Not null", Session("con"))

                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)
                rs.Close()
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

                outp &= "</tr>" & Chr(13)
                rs2.Close()
                rs2 = dbs.dtmake("dbpen", "SELECT *  FROM emp_inc WHERE (amt + amt2)>0 AND (paid_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is not null", Session("con"))
                Dim arremp(1) As String
                arremp(0) = ""
                If rs2.HasRows Then
                    Dim k As Integer = 1
                    Dim color As String = ""
                    Dim resing As Date
                    While rs2.Read

                        emptid = rs2.Item("emptid")
                        If rs2.IsDBNull(7) = True Then
                            paid = "None"

                        Else
                            paid = rs2.Item("paidref")
                        End If
                        If fm.searcharray(arremp, emptid.ToString) = False Then
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                            End If

                            cell(0) = k
                            cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                            cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                            cell(3) = fm.getinfo2("select gross_earn from paryrol where emptid=" & emptid & " and payroll_id in(select id from payrol_reg where month='" & pdate3.Month & "' and year='" & pdate3.Year & "')", Session("con"))
                            If cell(3) = "None" Then
                                cell(3) = 0
                            End If
                            rs = dbs.dtmake("dbx", "SELECT *  FROM emp_inc WHERE emptid=" & emptid & " and (amt + amt2)>0 AND (paid_date between '" & pdate3 & "' and '" & pdate4 & "')", Session("con"))
                            j = 0
                            inc = 0
                            For p As Integer = 0 To reasonname.Length - 1
                                cellb(p) = 0
                                cellbval(p) = 0
                            Next
                            If rs.HasRows Then
                                While rs.Read
                                    '  Response.Write("llllooopppp" & rs.Item("emptid"))
                                    For p As Integer = 0 To reasonname.Length - 1

                                        If reasonname(p) = rs.Item("reason") Then
                                            cellb(p) = rs.Item("amt").ToString
                                            ' Response.Write(cellb(p))
                                            cellbval(p) = rs.Item("id").ToString
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))
                                            '  Response.Write(rs.Item("emptid") & "=>" & reasonname(p) & "=" & rs.Item("reason") & p & "<br>")
                                        ElseIf cellb(p) = 0 Then
                                            cellb(p) = 0
                                            cellbval(p) = 0
                                            sumloan(p) += CDbl(cellb(p))
                                            inc = inc + CDbl(cellb(p))

                                        End If

                                    Next
                                    'cellb(j) = rs.Item("amt").ToString
                                    'cellbval(j) = rs.Item("id").ToString
                                    'sumloan(j) += CDbl(cellb(j))
                                    'inc = inc + CDbl(cellb(j))
                                    ' j += 1
                                End While
                                ' While j < ccol
                                'cellb(j) = 0
                                'cellbval(j) = 0
                                'sumloan(j) += CDbl(cellb(j))
                                'inc = inc + CDbl(cellb(j))
                                'j += 1
                                'End While
                            End If
                            cell(5) = inc
                            cell(6) = CDbl(cell(5)) + CDbl(cell(3))
                            ' cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6))), 2).ToString
                            cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString
                            cell(8) = fm.getinfo2("select tax from paryrol where emptid=" & emptid & " and payroll_id in(select id from payrol_reg where month='" & pdate3.Month & "' and year='" & pdate3.Year & "') and (ref_inc='0' or ref_inc IS NULL)", Session("con"))
                            If cell(8) = "None" Then
                                cell(8) = 0
                            End If
                            cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                            cell(10) = CDbl(cell(5)) - CDbl(cell(9))
                            If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                sumbsal += CDbl(cell(3))
                                ' sumbearn += CDbl(cell(4))
                                suminc += CDbl(cell(5))
                                sumtaxinc += CDbl(cell(6))
                                sumtax += CDbl(cell(7))
                                sumptax += CDbl(cell(8))
                                sumtaxpay += CDbl(cell(9))
                                sumnet += CDbl(cell(10))

                            End If
                            outp &= "<tr>"
                            For i As Integer = 0 To 10

                                If i <> 4 Then
                                    If i >= 0 And i < 3 Then
                                        outp &= ("<td class='cell' style=''  id='p" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i) & "</td>")

                                    Else
                                        outp &= ("<td class='cell' style='text-align:right;'  id='p" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    End If

                                Else
                                    For j = 0 To ccol - 1
                                        ' Response.Write(cellb(j) & "<><br>")
                                        outp &= ("<td style='text-align:right;'  id='p" & emptid.ToString & "_" & i & "-" & cellbval(j) & "'>&nbsp;" & FormatNumber(cellb(j).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    Next


                                End If
                            Next
                            ' Response.Write(paid)

                            outp &= "<td width='300px' style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                            k += 1

                            Dim arrl As Integer
                            arrl = arremp.Length
                            ReDim Preserve arremp(arrl + 1)
                            arremp(arrl) = emptid.ToString

                        End If
                    End While
                    outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                   "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
                   "<td style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"
                    For j = 0 To ccol - 1
                        outp &= "<td style='text-align:right;'>" & fm.numdigit(sumloan(j), 2).ToString & "</td>"
                    Next
                    outp &= ("<td style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                     "<td style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                     "<td style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                     "<td style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                     "<td style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                     "<td style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                     "<td style='text-align:right;'>&nbsp</td>")

                End If
            End If
            outp &= signpart()
            outp &= "</tr></table>"
            outp &= fm.projtrans(Request.Form("projname"), pdate1, Session("con"))
        End If
        Response.Write(outp)
        ' Response.Write(ccol)



    End Function
    Function signpart()
        Dim outp As String = ""
        outp &= "<tr><td style='height:10px' colspan='10' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>&nbsp;</td></tr>"
        outp &= "<tr style='border:0px 0px 0px 0px;'>"
        outp &= "<td colspan='3' style='width:150px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Receiver's Signature</label></td>"

        outp &= "<td  colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Prepared&nbsp; By</label></td>"

        outp &= "<td  colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Checked By</label></td>"

        outp &= "<td  colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Verified By</label></td>"

        outp &= " <td colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Approved By</label></td>"

        outp &= "</tr>" & Chr(13)

        ' outp &= "<tr><td style='height:50px' colspan='15' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>&nbsp;</td></tr>" & Chr(13)

        outp &= "<tr>"
        outp &= "<td colspan='3' style='width:150px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Date</label></td>"

        outp &= "<td colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Date</label></td>"

        outp &= "<td colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Date</label></td>"

        outp &= "<td colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Date</label></td>"

        outp &= "<td colspan='2' style='width:100px;text-align:center;border:1px 1px 1px 1px white solid;'>________________<br><label style='top-padding:-3px;'>Date</label></td>"
        outp &= "</tr>" & Chr(13)

        Return outp
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
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vw_loan WHERE  (dstart <= '" & pdate1 & "') and ref='0' GROUP BY reason) AS derivedtbl_1", Session("con")))
                'Response.Write("ref=0")
            Else
                'Response.Write(paid)
                If IsNumeric(paid) = False Then
                    paid = "0"
                End If

                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vw_loan WHERE ( dstart <= '" & pdate1 & "') and ref='" & paid & "' GROUP BY reason) AS derivedtbl_1", Session("con")))
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
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
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
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))

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
                            cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            ElseIf IsNumeric(cell(5)) = False Then
                                Response.Write(cell(5).ToString & "<br>")
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
                            ' cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
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
                                'Response.Write("select id,no_month_settle as nopay,amt,reason from emp_loan_req where deduction_starts<br> < ='" & pdate1 & "' and emptid=" & emptid.ToString & " and id in<br>(select loan_no from emp_loan_settlement where ref=" & paid.ToString & ")<br>")
                                rs2 = dbs.dtmake("loan", "select id,no_month_settle as nopay,amt,reason from emp_loan_req where deduction_starts < ='" & pdate1 & "' and emptid=" & emptid.ToString & " and id in(select loan_no from emp_loan_settlement where ref=" & paid.ToString & ")", Session("con"))
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
                                        loanid &= fm.getinfo2("select id from emp_loan_settlement where loan_no=" & rs2.Item("id") & " and date_payment='" & pdate2 & "' and ref=" & paid.ToString, Session("con")) & ","

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
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

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
    Public Function makeformpaidx_payroll() 'view
        Dim ref As String = ""
        Dim xoutp As String = ""

        If Request.QueryString("prid") <> "" Then
            ref = Request.QueryString("prid")
            ' Response.Write(ref)
            Dim sec As New k_security
            Dim pdate1, pdate2, pdate3, pdate4 As Date
            Dim nod As Integer

            Dim fl As New file_list
            'Dim namelist As String = ""
            Dim emptid, empname As String
            Dim cell(17) As Object
            Dim cellb() As String
            Dim cellbval() As String

            Dim sum(15) As Double
            Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
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
            Dim ccol As Integer = 0
            outp = ""
            'Dim ccol As Integer = 0
            ccol = 0
            sumbsal = 0
            Dim idg As String = fm.getinfo2("select id from payrollx where ref='" & ref & "' order by id desc", Session("con"))
            Dim pd As String = fm.getinfo2("select date_paid from payrollx where ref='" & ref & "' order by id desc", Session("con"))
            ' Response.Write(ref & "<br>")
            Dim idlist As String = ""
            idlist = fm.getinfo2("select emptid from payrollx where ref='" & ref & "'", Session("con"))
            ' Response.Write("<br>" & idlist)
            suminc = 0
            sumtaxinc = 0
            sumtax = 0

            sumptax = 0
            sumtaxpay = 0
            sumnet = 0
            Dim getpro() As String = getproj_on_date(emptid, pd)
            Dim projid As String = ""
            If Request.QueryString("month") <> "" Then
                Dim spl() As String

                If Request.QueryString("projname") <> "" Then
                    spl = sec.dbHexToStr(Request.QueryString("projname")).Split("|")
                    If spl(1) = getpro(1) Then
                        Response.Write("Pass")
                    End If
                    If spl.Length <= 1 Then
                        ReDim spl(2)
                        spl(0) = sec.dbHexToStr(Request.QueryString("projname"))
                        spl(1) = ""
                    End If
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                End If

                Try

               
                nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                pdate3 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
                pdate4 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
                pdate1 = pdate3.AddMonths(-1)
                pdate2 = pdate1.Month & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year
                Dim rs As DataTableReader
                Dim rs2 As DataTableReader
                    Response.Write(pdate1 & "=====>" & pdate2 & "=====>" & pdate3 & "=====>" & pdate4 & "<br>")
                Dim ccol2 As Integer = 0
                Dim paid, paid2 As String
                Dim j As Integer
                Dim reasonname() As String
                Dim paypaid As Integer = 0
                ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
                rs = dbs.dtmake("nuemp", "select * from emp_inc where paidref='" & ref & "'", Session("con"))

                If paypaid <> 0 Then
                    ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

                End If

                paid = ""
                paid = fm.getinfo2("select id from emp_inc where paidref is null", Session("con"))


                ccol = CInt(fm.getinfo2("SELECT COUNT(countx) AS Expr1 froM (SELECT distinct reason AS countx FROM emp_inc WHERE paidref='" & ref & "' GROUP BY reason) AS derivedtbl_1", Session("con")))
                ReDim cellb(ccol + 1)
                ReDim cellbval(ccol + 1)
                ReDim sumloan(ccol + 1)
                ReDim reasonname(ccol + 1)

                    strc = "0"
                    projid = ""
                If projid = "" Then
                    rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
                Else
                    'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                        rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate3 & "' and '" & pdate4 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))

                        ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
                End If
                If rs.HasRows Then

                    outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                    outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                    outp &= "<td style='text-align:center;font-weight:bold;' colspan='14' >" & Session("company_name") & _
                    "<br> Project Name:"
                    If projid <> "" Then
                        outp &= spl(0).ToString
                    Else
                        outp &= "All Projects"
                    End If
                    outp &= "<br><label id='spnid'></label> payment Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate2.Year & "</td>" & Chr(13)

                    outp &= "</tr>" & Chr(13)

                    outp &= "<tr class='dw' style='text-align:center;font-weight:bold;' >" & Chr(13)
                    outp &= "<td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp.id</td>"
                    outp &= "<td class='fxname' style='width:250px;' rowspan='2'>Full Name</td>"
                    outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                    outp &= "<td class='fitx'   colspan='" & (ccol + 1).ToString & "'>Increament</td>"
                    outp &= "<td class='fitx'  rowspan='2'>Total Increament</td>"
                    outp &= "<td class='fitx'  rowspan='2'>Taxable Income</td>"
                    outp &= "<td class='fitx'   rowspan='2'>Tax</td>" & Chr(13)
                    outp &= "<td  class='fitx' rowspan='2'> Tax Paid</td>"
                    outp &= "<td  class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                    outp &= "<td  class='fitx' rowspan='2'>Net pay</td>"
                    outp &= "<td  class='fitx' rowspan='2'>Signature</td></tr>" & Chr(13)
                    rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE paidref ='" & ref & "'", Session("con"))

                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                    outp &= "<tr>" & Chr(13)
                    rs.Close()
                    Dim i As Integer = 0
                    If rs2.HasRows Then

                        While rs2.Read

                            If rs2.Item("reason") = "-" Then
                                outp &= "<td class='dw' >Other</td>"
                            Else
                                outp &= "<td class='dw'>" & rs2.Item("reason").ToString & "</td>"
                            End If
                            reasonname(i) = rs2.Item("reason")
                            i += 1
                        End While
                        reasonname(i) = "Non-Taxable"
                        outp &= "<td class='dw'>Non-Taxable</td>"
                    End If

                    outp &= "</tr>" & Chr(13)
                    rs2.Close()
                    rs2 = dbs.dtmake("dbpen", "SELECT *  FROM emp_inc inner join emp_static_info as esi on esi.emp_id=emp_inc.emp_id WHERE paidref='" & ref & "' order by esi.first_name", Session("con"))
                    Dim arremp(1) As String
                    arremp(0) = ""
                    If rs2.HasRows Then
                        Dim k As Integer = 1
                        Dim color As String = ""
                        Dim resing As Date
                        While rs2.Read

                            emptid = rs2.Item("emptid")
                            If rs2.IsDBNull(7) = True Then
                                paid = "None"

                            Else
                                paid = rs2.Item("paidref")
                            End If
                            If fm.searcharray(arremp, emptid.ToString) = False Then
                                resing = "#1/1/1900#"
                                If color <> "#aabbcc" Then
                                    color = "#aabbcc"
                                Else
                                    color = "white"
                                End If
                                Try
                                    idg = fm.getinfo2("select id from payrollx where ref='" & ref & "' and emptid=" & emptid, Session("con"))

                                    cell(0) = k
                                    cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                                    cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                                    cell(3) = fm.getinfo2("select sum(isnull(txinco,0)) as exp1 from payrollx where (emptid=" & emptid & ") and ((pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))) and (ref<>'" & ref & "') and (id<" & idg & ")", Session("con")).ToString
                                    ' Response.Write("<br>select sum(isnull(txinco,0)) as exp1 from payrollx where (emptid=" & emptid & ") and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "'))) and ref<>'" & ref & "' and id<" & idg)

                                    'cell(3) = "0"
                                    If IsNumeric(cell(3)) = False Then
                                        Response.Write(cell(3))
                                        cell(3) = 0
                                    End If
                                    Dim otamtpaid As Double
                                    Dim strot As String
                                    strot = fm.getinfo2("select sum(isnull(ot,0)) as exp1 from payrollx where (emptid=" & emptid & ") and ((pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "')))) and (ref<>'" & ref & "') and (id<" & idg & ") and remark='OT-Payment'", Session("con")).ToString
                                    If IsNumeric(strot) Then
                                        otamtpaid = strot
                                    Else
                                        otamtpaid = 0
                                    End If
                                    'getotpaidin(pdate1, pdate2, emptid, Session("con"))
                                    'Response.Write(mtx.ToString)
                                    'Response.Write
                                    cell(3) = CDbl(cell(3)) + otamtpaid
                                    Dim rsp As DataTableReader
                                    rsp = dbs.dtmake("dbx", "SELECT *  FROM emp_inc WHERE emptid=" & emptid & " and paidref='" & ref & "'", Session("con"))
                                    j = 0
                                    inc = 0
                                    For p As Integer = 0 To reasonname.Length - 1
                                        cellb(p) = 0
                                        cellbval(p) = 0
                                    Next
                                    Dim max As Integer = 0
                                    If rsp.HasRows Then
                                        While rsp.Read
                                                '  Response.Write("llllooopppp" & rs.Item("emptid"))

                                            For p As Integer = 0 To reasonname.Length - 1
                                                    ' Response.Write("<br>" & rsp.Item("emptid") & "====>" & reasonname(p) & "=============>" & rsp.Item("reason") & "<br>")
                                                    If Trim(LCase(reasonname(p))) = Trim(LCase(rsp.Item("reason"))) Then
                                                        cellb(p) = rsp.Item("amt").ToString
                                                        ' Response.Write(cellb(p))
                                                        cellbval(p) = rsp.Item("id").ToString
                                                        sumloan(p) += CDbl(cellb(p))
                                                        inc = inc + CDbl(cellb(p))
                                                        max = p
                                                        ' Response.Write(rs.Item("emptid") & "=>" & reasonname(p) & "=" & rs.Item("reason") & p & "<br>")
                                                    ElseIf cellb(p) = 0 Then
                                                        cellb(p) = 0
                                                        cellbval(p) = 0
                                                        sumloan(p) += CDbl(cellb(p))
                                                        inc = inc + CDbl(cellb(p))
                                                        max = p
                                                    End If

                                            Next

                                            ' Response.Write(sumloan(max + 1).ToString & "<br>")
                                            'cellb(j) = rs.Item("amt").ToString
                                            'cellbval(j) = rs.Item("id").ToString
                                            'sumloan(j) += CDbl(cellb(j))
                                            'inc = inc + CDbl(cellb(j))
                                            ' j += 1
                                        End While
                                        max = UBound(reasonname)
                                        cellb(max) = fm.getinfo2("select sum(amt2) as exp2 from emp_inc WHERE emptid=" & emptid & " and paidref='" & ref & "'", Session("con"))


                                        ' Response.Write(cellb(max + 1).ToString & rsp.Item("emptid"))
                                        If IsNumeric(cellb(max)) = False Then
                                            cellb(max) = 0
                                        End If
                                        cellbval(max) = rsp.Item("id").ToString
                                        sumloan(max) += CDbl(cellb(max))
                                        rsp.Close()
                                    End If
                                    cell(5) = inc + cellb(max)
                                    cell(6) = CDbl(cell(5)) + CDbl(cell(3)) - cellb(max)

                                    '  cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6))), 2).ToString

                                    cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString
                                    cell(8) = fm.getinfo2("select sum(isnull(tax,0)) from payrollx where emptid=" & emptid & " and id<" & idg & "and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "') and ref<>'" & ref & "' ", Session("con"))
                                    ' cell(8) = fm.getinfo2("select tax from payrollx where emptid=" & emptid & " and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "') and ref<>'" & ref & "' ", Session("con"))
                                    If IsNumeric(cell(8)) = False Then
                                        Response.Write(cell(8))
                                        cell(8) = 0
                                    End If
                                    cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                                    cell(10) = CDbl(cell(5)) - CDbl(cell(9))
                                    If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                        sumbsal += CDbl(cell(3))
                                        ' sumbearn += CDbl(cell(4))
                                        suminc += CDbl(cell(5))
                                        sumtaxinc += CDbl(cell(6))
                                        sumtax += CDbl(cell(7))
                                        sumptax += CDbl(cell(8))
                                        sumtaxpay += CDbl(cell(9))
                                        sumnet += CDbl(cell(10))

                                    End If

                                    outp &= "<tr >"
                                    For i = 0 To 10
                                        If CDbl(cell(3)) = 0 And i = 3 Then
                                            color = "red"
                                        Else
                                            color = "white"
                                        End If
                                        If i <> 4 Then
                                            If i >= 0 And i < 3 Then
                                                outp &= ("<td class='cell' style=''  id='p" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i) & "</td>")

                                            Else
                                                outp &= ("<td class='cell' style='background:" & color & ";text-align:right;'  id='p" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                            End If

                                        Else
                                            For j = 0 To ccol - 1
                                                ' Response.Write(cellb(j) & "<><br>")
                                                outp &= ("<td style='text-align:right;'  id='p" & emptid.ToString & "_" & i & "-" & cellbval(j) & "'>&nbsp;" & FormatNumber(cellb(j).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                            Next

                                            outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-inc-" & j + 1 & "'>&nbsp;" & FormatNumber(cellb(j + 1).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                        End If
                                    Next
                                    ' Response.Write(paid)

                                    outp &= "<td width='300px' style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                                    k += 1

                                    Dim arrl As Integer
                                    arrl = arremp.Length
                                    ReDim Preserve arremp(arrl + 1)
                                    arremp(arrl) = emptid.ToString
                                Catch ex As Exception
                                    'cell(7) = 0
                                    Response.Write(cell(9).ToString)
                                    Response.Write("<br>" & cell(5))
                                End Try
                            End If
                        End While
                        outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                       "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
                       "<td style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"
                        For j = 0 To ccol - 1
                            outp &= "<td style='text-align:right;'>" & fm.numdigit(sumloan(j), 2).ToString & "</td>"
                        Next
                        outp &= "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumloan(UBound(sumloan)), 2).ToString & "</td>"

                        outp &= ("<td style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>&nbsp</td>")

                    End If
                End If
                outp &= signpart()
                outp &= "</tr></table>"
                Dim resss As String = ""

                For pp As Integer = 0 To UBound(reasonname) - 2
                    If pp <> UBound(reasonname) - 2 Then
                        resss &= reasonname(pp) & ", "
                    Else
                        If resss.Length > 2 Then
                            resss = resss.Substring(0, resss.Length - 2)
                            resss &= " And " & reasonname(pp) & " "
                        Else
                            resss &= reasonname(pp) & " "
                        End If
                    End If
                Next
                Session("rrss") = resss

                xoutp &= "<script language='javascript'> $('#spnid').text('" & resss

                xoutp &= "');</script>"
                Catch ex As Exception
                    Response.Write(Request.QueryString("month") & "=====" & Request.QueryString("year"))
                End Try
            End If

            Response.Write(outp)
            Return outp
        End If
        ' Response.Write(ccol)
    End Function
    Function makeformpaidxdel_payroll()
        Dim ref As String = ""
        If Request.QueryString("prdel") <> "" Then
            ref = Request.QueryString("prdel")
            'Response.Write(ref)

            Dim pdate1, pdate2, pdate3, pdate4 As Date
            Dim nod As Integer

            Dim fl As New file_list
            'Dim namelist As String = ""
            Dim emptid, empname As String
            Dim cell(17) As Object
            Dim cellb() As String
            Dim cellbval() As String

            Dim sum(15) As Double

            Dim bsal, suminc, sumtaxinc, sumptax, sumtaxpay, inc As Double
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
            Dim ccol As Integer = 0
            outp = ""
            'Dim ccol As Integer = 0
            ccol = 0
            sumbsal = 0

            suminc = 0
            sumtaxinc = 0
            sumtax = 0

            sumptax = 0
            sumtaxpay = 0
            sumnet = 0
            Dim secx As New k_security
            Dim xv As String
            xv = Request.QueryString("projname")

            Try
                xv = secx.dbHexToStr(xv.ToString)
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            '  Response.Write(xv)
            Response.Write(xv)
            If Request.QueryString("month") <> "" Then
                Dim spl() As String
                Dim projid As String
                If xv <> "" Then
                    spl = xv.Split("|")
                    If spl.Length <= 1 Then
                        ReDim spl(2)
                        spl(0) = xv
                        spl(1) = ""
                    End If
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                End If
                nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                pdate3 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
                pdate4 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
                pdate1 = pdate3.AddMonths(-1)
                pdate2 = pdate4.AddMonths(-1)
                Dim rs As DataTableReader
                Dim rs2 As DataTableReader

                Dim ccol2 As Integer = 0
                Dim paid, paid2 As String
                Dim j As Integer
                Dim reasonname() As String
                Dim paypaid As Integer = 0
                ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
                rs = dbs.dtmake("nuemp", "select * from emp_inc where paidref='" & ref & "'", Session("con"))

                If paypaid <> 0 Then
                    ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

                End If

                paid = ""
                paid = fm.getinfo2("select id from emp_inc where paidref is null", Session("con"))


                ccol = CInt(fm.getinfo2("SELECT COUNT(countx) AS Expr1 froM (SELECT distinct reason AS countx FROM emp_inc WHERE paidref='" & ref & "' GROUP BY reason) AS derivedtbl_1", Session("con")))
                ReDim cellb(ccol + 1)
                ReDim cellbval(ccol + 1)
                ReDim sumloan(ccol + 1)
                ReDim reasonname(ccol)

                strc = "0"
                If Request.Form("projname") = "" Then
                    rs = dbs.dtmake("db", "select * from emprec where hire_date<='" & pdate4 & "' order by id desc", Session("con"))
                Else
                    rs = dbs.dtmake("selectemp", "select * from emprec where hire_date<='" & pdate4 & "' and id in(select emptid from emp_job_assign where project_id='" & projid & "' and end_date is null) and end_date is null ", Session("con"))
                End If

                If rs.HasRows Then

                    outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                    outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                    outp &= "<td style='text-align:center;font-weight:bold;' colspan='14' >" & Session("company_name") & _
                    "<br> Project Name:"
                    If projid <> "" Then
                        outp &= spl(0).ToString
                    Else
                        outp &= "All Projects"
                    End If
                    outp &= "<br><label id='spnid'></label> payment Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate2.Year & "</td>" & Chr(13)

                    outp &= "</tr>" & Chr(13)

                    outp &= "<tr class='dw' style='text-align:center;font-weight:bold;' >" & Chr(13)
                    outp &= "<td class='dw' rowspan='2'>Del</td><td class='dw' rowspan='2'>No.</td><td class='dw' style='width:60px;' rowspan='2'>Emp.id</td>"
                    outp &= "<td class='fxname' style='width:250px;' rowspan='2'>Full Name</td>"
                    outp &= "<td class='fitx' rowspan='2'>Prev. Taxable Income</td>" & Chr(13)
                    outp &= "<td class='fitx'   colspan='" & (ccol + 1).ToString & "'>Increament</td>"
                    outp &= "<td class='fitx'  rowspan='2'>Total Increament</td>"
                    outp &= "<td class='fitx'  rowspan='2'>Taxable Income</td>"
                    outp &= "<td class='fitx'   rowspan='2'>Tax</td>" & Chr(13)
                    outp &= "<td  class='fitx' rowspan='2'> Tax Paid</td>"
                    outp &= "<td  class='fitx' rowspan='2'>Tax payable</td>" & Chr(13)
                    outp &= "<td  class='fitx' rowspan='2'>Net pay</td>"
                    outp &= "<td  class='fitx' rowspan='2'>Signature</td></tr>" & Chr(13)
                    rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE paidref ='" & ref & "'", Session("con"))

                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                    outp &= "<tr>" & Chr(13)
                    rs.Close()
                    Dim i As Integer = 0
                    If rs2.HasRows Then

                        While rs2.Read

                            If rs2.Item("reason") = "-" Then
                                outp &= "<td >Other</td>"
                            Else
                                outp &= "<td >" & rs2.Item("reason").ToString & "</td>"
                            End If
                            reasonname(i) = rs2.Item("reason")
                            i += 1
                        End While
                        outp &= "<td >Non-Taxable</td>"
                    End If

                    outp &= "</tr>" & Chr(13)
                    rs2.Close()
                    rs2 = dbs.dtmake("dbpen", "SELECT *  FROM emp_inc WHERE paidref='" & ref & "'", Session("con"))
                    Dim arremp(1) As String
                    arremp(0) = ""
                    If rs2.HasRows Then
                        Dim k As Integer = 1
                        Dim color As String = ""
                        Dim resing As Date
                        While rs2.Read

                            emptid = rs2.Item("emptid")
                            If rs2.IsDBNull(7) = True Then
                                paid = "None"

                            Else
                                paid = rs2.Item("paidref")
                            End If
                            If fm.searcharray(arremp, emptid.ToString) = False Then
                                resing = "#1/1/1900#"
                                If color <> "#aabbcc" Then
                                    color = "#aabbcc"
                                Else
                                    color = "white"
                                End If

                                cell(0) = k
                                cell(1) = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                                cell(2) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con"))
                                cell(3) = fm.getinfo2("select sum(txinco) as exp1 from payrollx where emptid=" & emptid & " and (pr in(select id from payrol_reg where (month='" & pdate1.Month & "') and (year='" & pdate2.Year & "'))) and ref<>'" & ref & "'", Session("con")).ToString
                                '  Response.Write("select sum(Gross_earn) as exp1 from paryrol where emptid=" & emptid & " and (payroll_id in(select id from payrol_reg where (month='" & pdate3.Month & "') and (year='" & pdate3.Year & "')))" & cell(3).ToString & "<br>")

                                'cell(3) = "0"
                                If cell(3) = "None" Or cell(3).ToString.Length > 10 Then
                                    cell(3) = 0
                                End If
                                Dim rsp As DataTableReader
                                rsp = dbs.dtmake("dbx", "SELECT *  FROM emp_inc WHERE emptid=" & emptid & " and paidref='" & ref & "'", Session("con"))
                                j = 0
                                inc = 0
                                For p As Integer = 0 To reasonname.Length - 1
                                    cellb(p) = 0
                                    cellbval(p) = 0
                                Next
                                Dim max As Integer = 0
                                If rsp.HasRows Then
                                    While rsp.Read
                                        '  Response.Write("llllooopppp" & rs.Item("emptid"))
                                        For p As Integer = 0 To reasonname.Length - 1

                                            If reasonname(p) = rsp.Item("reason") Then
                                                cellb(p) = rsp.Item("amt").ToString
                                                ' Response.Write(cellb(p))
                                                cellbval(p) = rsp.Item("id").ToString
                                                sumloan(p) += CDbl(cellb(p))
                                                inc = inc + CDbl(cellb(p))
                                                max = p
                                                ' Response.Write(rs.Item("emptid") & "=>" & reasonname(p) & "=" & rs.Item("reason") & p & "<br>")
                                            ElseIf cellb(p) = 0 Then
                                                cellb(p) = 0
                                                cellbval(p) = 0
                                                sumloan(p) += CDbl(cellb(p))
                                                inc = inc + CDbl(cellb(p))
                                                max = p
                                            End If

                                        Next

                                        ' Response.Write(sumloan(max + 1).ToString & "<br>")
                                        'cellb(j) = rs.Item("amt").ToString
                                        'cellbval(j) = rs.Item("id").ToString
                                        'sumloan(j) += CDbl(cellb(j))
                                        'inc = inc + CDbl(cellb(j))
                                        ' j += 1
                                    End While
                                    max = UBound(reasonname)
                                    cellb(max + 1) = fm.getinfo2("select sum(amt2) as exp2 from emp_inc WHERE emptid=" & emptid & " and paidref='" & ref & "'", Session("con"))
                                    ' Response.Write(cellb(max + 1).ToString & rsp.Item("emptid"))
                                    If IsNumeric(cellb(max + 1)) = False Then
                                        cellb(max + 1) = 0
                                    End If
                                    cellbval(max + 1) = rsp.Item("id").ToString
                                    sumloan(max + 1) += CDbl(cellb(max + 1))
                                    rsp.Close()
                                End If
                                cell(5) = inc
                                cell(6) = CDbl(cell(5)) + CDbl(cell(3))
                                ' cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6))), 2).ToString
                                cell(7) = Math.Round(fm.pay_tax(CDbl(cell(6)), pdate1, Session("con")), 2).ToString
                                cell(8) = fm.getinfo2("select tax from payrollx where emptid=" & emptid & " and pr in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "') and ref<>'" & ref & "' ", Session("con"))
                                If cell(8) = "None" Then
                                    cell(8) = 0
                                End If
                                cell(9) = CDbl(cell(7)) - CDbl(cell(8))
                                cell(10) = CDbl(cell(5)) - CDbl(cell(9))
                                If color <> "#ccaa99" And cell(5).ToString <> "0" Then
                                    sumbsal += CDbl(cell(3))
                                    ' sumbearn += CDbl(cell(4))
                                    suminc += CDbl(cell(5))
                                    sumtaxinc += CDbl(cell(6))
                                    sumtax += CDbl(cell(7))
                                    sumptax += CDbl(cell(8))
                                    sumtaxpay += CDbl(cell(9))
                                    sumnet += CDbl(cell(10))

                                End If
                                outp &= "<tr><td><span class='iddel' onclick=" & Chr(34) & "javascript:delsingleinc('emptid=" & emptid & "&ref=" & ref & "');" & Chr(34) & ">del</span></td>"
                                For i = 0 To 10

                                    If i <> 4 Then
                                        If i >= 0 And i < 3 Then
                                            outp &= ("<td class='cell' style=''  id='p" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i) & "</td>")

                                        Else
                                            outp &= ("<td class='cell' style='text-align:right;'  id='p" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                        End If

                                    Else
                                        For j = 0 To ccol - 1
                                            ' Response.Write(cellb(j) & "<><br>")
                                            outp &= ("<td style='text-align:right;'  id='p" & emptid.ToString & "_" & i & "-" & cellbval(j) & "'>&nbsp;" & FormatNumber(cellb(j).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                        Next

                                        outp &= ("<td class='cell' style='text-align:right;'  id='" & emptid.ToString & "_" & i & "-inc-" & j + 1 & "'>&nbsp;" & FormatNumber(cellb(j + 1).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    End If
                                Next
                                ' Response.Write(paid)

                                outp &= "<td width='300px' style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                                k += 1

                                Dim arrl As Integer
                                arrl = arremp.Length
                                ReDim Preserve arremp(arrl + 1)
                                arremp(arrl) = emptid.ToString

                            End If
                        End While
                        outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                       "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
                       "<td style='text-align:right;'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>"
                        For j = 0 To ccol - 1
                            outp &= "<td style='text-align:right;'>" & fm.numdigit(sumloan(j), 2).ToString & "</td>"
                        Next
                        outp &= "<td class='cell' style='text-align:right;'>" & fm.numdigit(sumloan(UBound(sumloan)), 2).ToString & "</td>"

                        outp &= ("<td style='text-align:right;'>" & fm.numdigit(suminc, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumtaxinc, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumtax.ToString, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumptax, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumtaxpay, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                         "<td style='text-align:right;'>&nbsp</td>")

                    End If
                End If
                outp &= signpart()
                outp &= "</tr></table>"
                Dim resss As String = ""

                For pp As Integer = 0 To UBound(reasonname) - 2
                    If pp <> UBound(reasonname) - 2 Then
                        resss &= reasonname(pp) & ", "
                    Else
                        If resss.Length > 2 Then
                            resss = resss.Substring(0, resss.Length - 2)
                            resss &= " And " & reasonname(pp) & " "
                        End If
                    End If
                Next

                outp &= "<script language='javascript'> $('#spnid').text('" & resss

                outp &= "');</script>"

            End If

            Response.Write(outp)
            Return outp
        End If
    End Function
    Public Function makeformunpaid()
        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim bsal, dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
        Dim sumbsal, sumfsal, sumssal, sumbearn, sumtalw, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double
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
        sumfsal = 0
        sumssal = 0
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

            If paid = "None" Then
                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vwloanbal WHERE  bal>0 and (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                'Response.Write("ref=0")
            Else
                'Response.Write(paid)
                If IsNumeric(paid) = False Then
                    paid = "0"
                End If

                ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vwloanbal WHERE bal>0 and ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                ' ccol2 = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT reason AS cnt FROM vw_loan WHERE Bal > 0 AND ( dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
                '  Response.Write("ref=0")
            End If
            ' Response.Write(ccol)
            If ccol = 0 Then
                ccol = 1
            End If
            'Response.Write(ccol)
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim paidpayrol As String
            Dim stylew(4) As String
            Dim tcell As Integer
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
            If projid = "" Then
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
            End If
            'Response.Write("ddd")
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='22' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td rowspan='2' class='dw'>No.</td><td class='dw' rowspan='2'>Emp. Id</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='dedct' colspan='3'>Basic Salary</td>" & Chr(13)
                outp &= "<td class='dw'  style='width:30px;' rowspan='2'>Days Worked</td>"
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
                outp &= "<td class='dw'  id='chkall'  rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                outp &= "<td rowspan='2' class='signpart'>Signature</td></tr>" & Chr(13)
                If paid = "None" Then
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

                Else
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0 and dstart < ='" & pdate1 & "'", Session("con"))

                End If
                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)
                outp &= "<td class='dedctx' style='width:70px;'>Prev. Salary</td><td class='dedctx'>New Salary</td><td class='dedctx'>Salary</td>"
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
                Dim did As String
                While rs.Read
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))

                    emptid = rs.Item("id")
                    paid = ""
                    paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in (select pr from payrollx where emptid='" & emptid & "' and ref_inc='0' or ref_inc is null)", Session("con"))
                    'Response.Write(paid.ToString & "<br>...")
                    If paid.ToString = "None" Then
                        resing = "#1/1/1900#"
                        If color <> "#aabbcc" Then
                            color = "#aabbcc"
                        Else
                            color = "white"
                        End If

                        did = ""
                        If (rs.Item("active") = "n") Then
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))

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

                        ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                        If incwhen > 0 Then '....
                            Dim csal(2) As String
                            ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)
                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            'Response.Write(sql & "<br>")
                            sal = Me.getsal(emptid, pdate1, Session("con"))
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
                            Dim salpen As Double
                            salpen = CDbl(csal(1))
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

                            cell(3) = Math.Round(calc, 2).ToString
                            Dim dbl As Double
                            dbl = (CDbl(cell(2)) / nod) * CDbl(calc)

                            ' Response.Write(dbl.ToString & rs.Item("emp_id") & "<br>")

                            'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                            cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString ' God does this
                            'cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                            ' Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)
                            ' Response.Write(cell(4).ToString & "<br>")

                            cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            ElseIf IsNumeric(cell(5)) = False Then
                                Response.Write(cell(5).ToString & "<br>")
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

                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            'cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
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
                            Dim penss As String
                            penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", Session("con"))
                            ' Response.Write(pdate2.Subtract(dhr).Days.ToString & "<br>")
                            If pdate2.Subtract(dhr).Days >= 45 Then
                                cell(12) = Math.Round((fm.pension(CDbl(salpen), CDbl(pemp) / 100)), 2).ToString
                                cell(13) = Math.Round((fm.pension(CDbl(salpen), CDbl(pco) / 100)), 2).ToString

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
                                outp &= "<tr><td id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

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
                                            outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                        Else
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                        End If
                                    ElseIf i = 2 Then
                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-1'>" & fm.numdigit(csal(0).ToString, 2) & "<br>(" & incwhen.ToString & " days)</td>")

                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-2'>" & fm.numdigit(csal(1).ToString, 2) & "<br>(" & (nod - incwhen).ToString & " days)</td>")

                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    ElseIf i = 11 Then
                                        For j = 0 To ccol - 1
                                            If String.IsNullOrEmpty(cellbval(j)) = False Then
                                                outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                            Else
                                                outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
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
                           "<td class='cooo' style='border-bottom:1px black solid;border-top:1px solid black;'>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
                           "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bfsal'>" & fm.numdigit(sumfsal.ToString, 2).ToString & "</td>" & _
                            "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bssal'>" & fm.numdigit(sumssal.ToString, 2).ToString & "</td>" & _
                             "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                            "<td class='cooo' style='text-align:right;border-top:1px solid black;'>&nbsp;</td>" & _
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
                  "<td class='cooo' colspan='2'>..&nbsp;" & _
     "</td></tr><tr id='result'></tr>"
                outp &= fm.signpart()
                outp &= "</table>"

                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
            End If
            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
            ' xprint = "<script language='javascript' type='text/javascript'>//sumcolx(); " & _
            '"$(function() {$( '#payd').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	}); $( '#payd' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
            ' "  <input type='button' id='post' onclick='javascript:findid2()' name='post' value='Make Statment' />"



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
        Dim avalue As String = ""
        Dim sum(15) As Double

        Dim bsal, dwork, basicearning, tallw, allw, ot, grossearn, taxincome, tax, loan(), pemp, pco, netincom As Double
        Dim sumbsal, sumfsal, sumssal, sumbearn, sumtalw, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double
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
        sumfsal = 0
        sumssal = 0
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
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='22' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td rowspan='2' class='dw'>No.</td><td class='dw' rowspan='2'>Emp. Id</td>"
                outp &= "<td class='fxname' rowspan='2'>Full Name</td>"
                outp &= "<td class='dedct' colspan='3'>Basic Salary</td>" & Chr(13)
                outp &= "<td class='dw'  style='width:30px;' rowspan='2'>Days Worked</td>"
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
                'outp &= "<td class='dw'  id='chkall'  rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                outp &= "<td rowspan='2' class='signpart'>Signature</td></tr>" & Chr(13)
                If paid = "None" Then
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))

                Else
                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanbal where bal>0 and dstart < ='" & pdate1 & "'", Session("con"))

                End If
                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)
                outp &= "<td class='dedctx' style='width:70px;'>Prev. Salary</td><td class='dedctx'>New Salary</td><td class='dedctx'>Salary</td>"
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
                        If incwhen > 0 Then 'question..........
                            ' Response.Write(emptid & "==" & fm.searcharray(vx, emptid.ToString) & "<br>")
                            Dim csal(2) As String
                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            ' Response.Write(sql & "<br>")
                            sal = Me.getsal(emptid, pdate1, Session("con"))
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
                            Dim salpen As Double
                            salpen = CDbl(csal(1))
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
                            Dim dhr As Date
                            dhr = rs.Item("hire_date")
                            ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", Session("con"))
                            Dim penss As String
                            penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", Session("con"))
                            ' Response.Write(pdate2.Subtract(dhr).Days.ToString & "<br>")
                            'Response.Write(penss.ToString & "<br>")
                            If pdate2.Subtract(dhr).Days >= 45 Then
                                cell(12) = Math.Round((fm.pension(CDbl(salpen), CDbl(pemp) / 100)), 2).ToString
                                cell(13) = Math.Round((fm.pension(CDbl(salpen), CDbl(pco) / 100)), 2).ToString

                            Else
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If
                            If penss <> "None" Then
                                cell(12) = "0.00"
                                cell(13) = "0.00"
                            End If                           ' Response.Write(cell(3).ToString & "===")
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
                                outp &= "<tr><td id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)


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
                                            outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                        Else
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                        End If
                                    ElseIf i = 2 Then
                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-1'>" & fm.numdigit(csal(0).ToString, 2) & "<br>(" & incwhen.ToString & " days)</td>")

                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-2'>" & fm.numdigit(csal(1).ToString, 2) & "<br>(" & (nod - incwhen).ToString & " days)</td>")

                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    ElseIf i = 11 Then
                                        For j = 0 To ccol - 1
                                            If String.IsNullOrEmpty(cellbval(j)) = False Then
                                                outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                            Else
                                                outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
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

                                outp &= "<td style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If
                End While


                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                           "<td class='cooo' style='border-bottom:1px black solid;border-top:1px solid black;'>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
                           "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bfsal'>" & fm.numdigit(sumfsal.ToString, 2).ToString & "</td>" & _
                            "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bssal'>" & fm.numdigit(sumssal.ToString, 2).ToString & "</td>" & _
                             "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                            "<td class='cooo' style='text-align:right;border-top:1px solid black;'>&nbsp;</td>" & _
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
                  "<td class='cooo' colspan='2'>..&nbsp;" & _
     "</td></tr><tr id='result'></tr>"
                outp &= fm.signpart()
                outp &= "</table>"

                rs.Close()
                fm = Nothing
                dbs = Nothing
                Response.Write(outp)
            End If
            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))
            ' xprint = "<script language='javascript' type='text/javascript'>//sumcolx(); " & _
            '"$(function() {$( '#payd').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	}); $( '#payd' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
            ' "  <input type='button' id='post' onclick='javascript:findid2()' name='post' value='Make Statment' />"



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
            Dim stylew(4) As String
            Dim tcell As Integer
            tcell = ccol + 14

            Dim wdthpay As Integer = 1250
            tcell = ccol + 14
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
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
            End If

            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
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
                outp &= "<td rowspan='2' style='width:" & stylew(0).ToString & "'>No.</td><td style='width:" & stylew(1).ToString & ";' rowspan='2'>Emp. Id</td>"
                outp &= "<td style='width:" & stylew(2) & ";' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                outp &= "<td  style='width:30px;' rowspan='2'>Days Worked</td>"
                outp &= "<td  class='fitx' rowspan='2'>Basic Earning</td>"
                outp &= "<td  class='fitx' rowspan='2'>Taxable Allowance</td>"
                outp &= "<td  class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Overtime</td>"
                outp &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Net Pay</td>"


                outp &= "<td  id='chkall' style='cursor:pointer;width:" & stylew(3).ToString & ";' rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

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
                            ' sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            ' Response.Write(sql & "<br>")
                            sal = dbs.getsal(emptid, pdate1, Session("con"))
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

                            cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                            '  Response.Write(cell(5).ToString)
                            ' cell(5) = "0"
                            If cell(5).ToString = "None" Then
                                cell(5) = "0"
                            ElseIf cell(5).ToString = "" Then
                                cell(5) = "0"
                            ElseIf IsNumeric(cell(5)) = False Then
                                Response.Write(cell(5).ToString & "<br>")
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
                            cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                            ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                            '   Response.Write(cell(7).ToString & "<br>")
                            cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                            ' cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
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
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

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
                                outp &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'>"
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
                                outp &= "<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'>&nbsp;</td></tr>" & Chr(13)
                                k += 1
                            End If
                        End If
                    End If
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
                outp &= "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
                "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
                 "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
                  "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                  "<td class='cooo' colspan='2'>..&nbsp;" & _
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
           "$(function() {$( '#paydx').datepicker({changeMonth: true,changeYear: true	});" & Chr(13) & " $( '#paydx' ).datepicker( 'option','dateFormat','mm/dd/yy');});" & Chr(13) & "</script>" & Chr(13) & _
                    "  <input type='button' id='post' onclick='javascript:findid()' name='post' value='Paid' />"

            Response.Write(xprint)


            Return outp

        End If
    End Function
    Public Function makeform3()

        Dim pdate1, pdate2 As Date
        Dim nod As Integer

        Dim fl As New file_list
        Dim namelist As String = ""
        Dim emptid, empname As String
        Dim cell(17) As Object
        Dim cellb() As String
        Dim cellbval() As String

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
            'Response.Write(ccol)
            'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", Session("con")))
            ReDim cellb(ccol)
            ReDim cellbval(ccol)
            ReDim sumloan(ccol)
            ReDim reasonname(ccol)
            Dim stylew(4) As String
            Dim tcell As Integer
            Dim wdthpay As Integer = 1250
            tcell = ccol + 14
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
                rs = dbs.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.end_date between '" & pdate2 & "' and '" & pdate1 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.end_date between '" & pdate1 & "' and '" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
            End If

            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
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
                outp &= "<td rowspan='2' style='width:" & stylew(0).ToString & "'>No.</td><td style='width:" & stylew(1).ToString & ";' rowspan='2'>Emp. Id</td>"
                outp &= "<td style='width:" & stylew(2) & ";' rowspan='2'>Full Name</td>"
                outp &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                outp &= "<td  style='width:30px;' rowspan='2'>Days Worked</td>"
                outp &= "<td  class='fitx' rowspan='2'>Basic Earning</td>"
                outp &= "<td  class='fitx' rowspan='2'>Taxable Allowance</td>"
                outp &= "<td  class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'> Overtime</td>"
                outp &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                outp &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                outp &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                outp &= "<td style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                outp &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                outp &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                outp &= "<td class='fitx' rowspan='2'>Net Pay</td>"


                outp &= "<td  id='chkall' style='cursor:pointer;width:" & stylew(3).ToString & ";' rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                outp &= "<td rowspan='2' class='signpart'>Signature</td>" & Chr(13)

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
                Dim did As String
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
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", Session("con"))

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
                    If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year Then
                        'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                        incwhen = dateincr.Subtract(pdate1).Days
                    End If
                    If ((resing >= pdate1 And resing <= pdate2) Or resing = "#1/1/1900#" Or pdate1 < resing) And incwhen = 0 Then 'question..........solved
                        cell(0) = rs.Item("emp_id")
                        cell(1) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                        'sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                        ' Response.Write(sql & "<br>")
                        sal = dbs.getsal(emptid, pdate1, Session("con"))
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

                        'Response.Write(calc.ToString)

                        cell(3) = Math.Round(calc, 2).ToString
                        'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                        cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString ' God does this
                        'cell(4) = Math.Round(((CDbl(cell(2)) / nod) * CDbl(calc)), 2).ToString
                        ' Response.Write("<br>Math.Round(((" & CDbl(cell(2)).ToString & "/" & nod.ToString & "*" & CDbl(calc).ToString & ", 2)=" & cell(4).ToString)


                        cell(5) = fm.getinfo2("select sum(amount) as exp1 from emp_alloance_rec where (emptid=" & emptid & ") and (istaxable='y') and ('" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "'))", Session("con"))
                        '  Response.Write(cell(5).ToString)
                        ' cell(5) = "0"
                        If cell(5).ToString = "None" Then
                            cell(5) = "0"
                        ElseIf cell(5).ToString = "" Then
                            cell(5) = "0"
                        ElseIf IsNumeric(cell(5)) = False Then
                            Response.Write(cell(5).ToString & "<br>")
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
                        cell(7) = fm.getot(pdate1, pdate2, emptid, Session("con")).ToString
                        ' Response.Write(cell(4).ToString & "..4<br>" & cell(5).ToString & "..5<br>" & cell(6).ToString & "..6<br>" & cell(7).ToString & "..7<br>________<br>")
                        '   Response.Write(cell(7).ToString & "<br>")
                        cell(8) = CDbl(cell(4)) + CDbl(cell(5)) + CDbl(cell(6)) + CDbl(cell(7))
                        'Response.Write(cell(8).ToString & "<br>")
                        cell(9) = Math.Round((CDbl(cell(8)) - CDbl(cell(6))), 2).ToString
                        ' cell(10) = Math.Round(fm.pay_tax(CDbl(cell(9))), 2).ToString
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
                                        outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                    Else
                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & FormatNumber(cell(i).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</td>")

                                    End If
                                Else
                                    For j = 0 To ccol - 1
                                        If String.IsNullOrEmpty(cellbval(j)) = False Then
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                        Else
                                            outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
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
                            outp &= "<td style='text-align:right;'>"
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
                End While


                outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                "<td class='cooo' style=''>&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
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
                  "<td class='cooo' colspan='2'>..&nbsp;" & _
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
    Function deleteall()
        Dim spl() As String = Request.Form("delpass").Split("⌡")
        Dim spl2() As String
        Dim dbs As New dbclass
        Dim flg As String = ""
        Dim sqlst As String = "BEGIN TRANSACTION" & Chr(13)
        For i As Integer = 0 To spl.Length - 1
            If String.IsNullOrEmpty(spl(i)) = False Then
                spl2 = spl(i).Split(",")
                For j As Integer = 0 To spl2.Length - 1
                    If String.IsNullOrEmpty(spl2(j)) = False Then
                        If i = 0 Then
                            sqlst &= "delete from paryrol where id=" & spl2(j) & Chr(13)
                        ElseIf i = 1 Then
                            sqlst &= "delete from emp_loan_settlement where id=" & spl2(j) & Chr(13)
                        ElseIf i = 2 And spl2(j) <> "None" Then
                            sqlst &= "Update emp_ot set paidstatus='n',ref=NULL where id=" & spl2(j) & Chr(13)
                        End If

                    End If
                Next
            End If
        Next
        ' sqlst &= "COMMIT" & Chr(13)
        'Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
        flg = dbs.excutes(sqlst, Session("con"), session("path"))
        If IsNumeric(flg) = True Then
            ' Response.Write(flg)
            If CInt(flg) <= 0 Then
                dbs.save("rollback", Session("con"), session("path"))
                Response.Write("data is not saved")
            Else
                dbs.save("COMMIT", Session("con"), session("path"))
                Response.Write("Data Saved")
            End If



        End If


    End Function
    Function deleteallx(ByVal ref As String)

        Dim dbs As New dbclass
        Dim flg As String = ""
        Dim sqlst As String = "BEGIN TRANSACTION" & Chr(13)


        sqlst &= "delete from payrollx where ref='" & ref & "'" & Chr(13)

        'sqlst &= "delete from emp_loan_settlement where ref='" & ref & "'" & Chr(13)

        sqlst &= "Update emp_inc set paidref=Null where paidref='" & ref & "'" & Chr(13)
        ' Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
        flg = dbs.excutes(sqlst, Session("con"), session("path"))
        If IsNumeric(flg) = True Then
            ' Response.Write(flg)
            If CInt(flg) <= 0 Then
                dbs.excutes("RollBack", Session("con"), session("path"))
                Response.Write("data is not saved")
            Else
                dbs.excutes("Commit", Session("con"), session("path"))
                Response.Write("Data Saved")

            End If



        End If

        dbs = Nothing
        Return Nothing
    End Function
    Public Function getotpaidin(ByVal date1 As Date, ByVal date2 As Date, ByVal emptid As Integer, ByVal con As SqlConnection)
        Dim dbx As New dbclass
        Dim rtamt As Double
        Dim rs As DataTableReader
        rs = dbx.dtmake("selectdb", "select sum(ot) as amt from payrollx where date_paid='" & date2 & "' and emptid=" & emptid & " and ref is Not null and remark='OT-Payment' group by emptid", con)
        'Response.Write("select sum(amt) as amt from emp_ot where ot_date between '" & date1 & "' and '" & date2 & "' and emptid=" & emptid & " and ref is Not null group by emptid")
        If rs.HasRows Then
            rs.Read()

            rtamt = rs.Item("amt")

        Else

            rtamt = 0
        End If
        rs.Close()
        Return rtamt
    End Function
    Public Function getproj_on_date(ByVal emptid As Integer, ByVal pd1 As Date)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim rt(3) As String
        rt(0) = ""
        rt(1) = ""
        rt(2) = ""
        Dim hrd As String
        Dim sql As String = "select project_id, from emp_job_assign where " & _
                            "emptid=" & emptid & " and ('" & pd1 & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "'))"

        Try



            hrd = fm.getinfo2("select hire_date from emprec where id=" & emptid, Session("con"))

            rs = dbs.dtmake("dbsprojx", sql, Session("con"))
            If rs.HasRows Then

                While rs.Read()

                    ' Response.Write("<br>" & emptid.ToString & "===" & rs.Item("date_from"))

                    rt(0) = rs.Item("project_id")
                    rt(1) = dbs.getprojectname(rt(0), Session("con"))
                    rt(2) &= rt(1) & ","
                End While

            End If
        Catch ex As Exception
            Return rt
        End Try
        rt(2) &= "<br> " & sql & "--------------------------------------------------------------------------<br>"
        rs.Close()
        dbs = Nothing
        Return rt
    End Function
End Class
