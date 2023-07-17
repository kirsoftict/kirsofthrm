Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class rptpayroll
    Inherits System.Web.UI.Page
    Function goseach(ByVal ref As String)
        Dim fm As New formMaker
        Dim remark As String = ""
        Dim emptid As String
        Dim sec As New k_security

        remark = fm.getinfo2("select remark from payrollx where ref='" & ref & "'", Session("con"))
        Dim dtp As String
        Dim xdtp As Date
        dtp = fm.getinfo2("select date_paid from payrollx where ref='" & ref & "'", Session("con"))
        Dim proj As String
        emptid = fm.getinfo2("select emptid from payrollx where ref='" & ref & "'", Session("con"))
        Dim ccout As String = fm.getinfo2("select count(id) from payrollx where ref='" & ref & "'", Session("con"))
        Dim rtn As String = ""
        Dim projx() As String
        Try
            ' Response.Write(dtp)
            If dtp <> "" Then
                If IsDate(dtp) = True Then

                    xdtp = CDate(dtp).ToShortDateString
                    projx = fm.getproj_on_date(emptid, CDate(xdtp), Session("con"))
                Else

                End If
            End If
            proj = projx(1) & "|" & projx(0) & "|"
            Dim m, y As String
            dtp = CDate(dtp).ToShortDateString
            m = dtp.Split("/")(0)
            y = dtp.Split("/")(2)
            Response.Write(dtp)
            proj = sec.dbStrToHex(proj)

            If remark <> "None" Then
                '======================================================================
                Response.Write("<div>" & ref & "<br><span style='color:gray;font-size:10pt;'>(No. List:" & ccout & ") " & remark & "</span>" & _
                                      "</div>")


                '========================================================================
                Select Case LCase(remark)
                    Case "monthly"
                        rtn = "  <div class='viewpayrol' style='float:left;' onclick=" & _
                                        Chr(34) & "javascript:gotocheckprpt('view','?prid=" & ref & "&pd=" & CDate(dtp).ToShortDateString & "&month=" & m & "&year=" & y & "');" & _
                                        Chr(34) & "></div>"
                        rtn &= "<div class='bankpayrol' onclick=" & Chr(34) & _
                                       "javascript:gotocheckp('Bank','?ref=" & ref & _
                                       "&ppd=" & CDate(dtp).ToShortDateString & "&month=" & m & "&year=" & y & "&projname=" & proj & "');" & Chr(34) & _
                                       " style='float:left;></div>"

                    Case "increament"
                        rtn = "  <div class='viewpayrol' style='float:left;' onclick=" & _
                                        Chr(34) & "javascript:gotocheckinc('view','?prid=" & ref & "&pd=" & CDate(dtp).ToShortDateString & "');" & _
                                        Chr(34) & "></div><div class='bankpayrol'  style='float:left;' onclick=" & Chr(34) & _
                                        "javascript:gotocheckinc('Bank','?ref=" & ref & _
                                        "&ppd=" & CDate(dtp).ToShortDateString & "');" & Chr(34) & _
                                        "></div>"
                    Case "pay_inc_middle"
                        rtn = "  <div class='viewpayrol' onclick=" & _
                                            Chr(34) & "javascript:gotocheckpx('view','payrollmiddv2.aspx?prid=" & ref & "&pd=" & CDate(dtp).ToShortDateString & "&month=" & m & "&year=" & y & "&projname=" & proj & "');" & _
                                            Chr(34) & "></div>"
                    Case "ot-payment"
                        rtn = "<div class='viewpayrol' onclick=" & _
                                        Chr(34) & "javascript:gotocheckot('view','?prid=" & ref & "&pd=" & CDate(dtp).ToShortDateString & "&month=" & m & "&year=" & y & "&projname=" & proj & "');" & _
                                        Chr(34) & ">........</div>"
                    Case Else
                        rtn = "Sorry "


                End Select
            End If
        Catch ex As Exception
            rtn = ex.ToString & "<br>" & emptid & "====" & dtp & "=========" & remark
        End Try

        ' Response.Write("<code>" & rtn & "</code>")
        Return rtn
    End Function

    Function identifytime()
        Dim remark As String = ""
        Dim fm As New formMaker
        Dim cod As String
        If Request.QueryString("prid") <> "" Then
            remark = fm.getinfo2("select remark from payrollx where ref='" & Request.QueryString("prid") & "'", Session("con"))
        End If
        ' Response.Write(remark)
        Dim loc As String = Server.MapPath("download") & "\rpt" & remark & "-" & Now.Ticks & ".txt"
        loc = loc.Replace("\", "/")
        'Response.Write(remark)
        Select Case remark
            Case "monthly"
                Response.Write("<div class=" & Chr(34) & "clickexp" & Chr(34) & " style=" & Chr(34) & " float:left; border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & Chr(34) & " onclick=" & Chr(34) & "javascript:exportx('payrol-rpt" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & "','xls','" & loc & "','export','2;3');" & Chr(34) & ">  <img src=" & Chr(34) & "images/png/excel.png" & Chr(34) & " height=" & Chr(34) & "28px" & Chr(34) & " style=" & Chr(34) & "float:left;" & Chr(34) & " alt=" & Chr(34) & "excel" & Chr(34) & " /> Export to Excel</div>")

                cod = makeformpaidx_payroll()

                Dim obj As Object
                If String.IsNullOrEmpty(cod) = False Then
                    ' cod = sec.StrToHex(cod)
                    ' Response.Write(cod)
                    obj = cod
                    obj = "1;2;3" & Chr(13) & obj




                    File.WriteAllText(loc, obj)
                End If

                'Response.Write(loc)


            Case "OT-Payment"
                rptot()
            Case "pay_inc_middle()"
                rptpayrollmid()
            Case "Increament"
                ' Response.Write("<br>>>>>ref=" & Request.QueryString("prid") & "<br>")
                Increment(Request.QueryString("prid"))
                rptpayment()

        End Select
      
    End Function
    Public Function rptpayroll1()
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
        Dim spl() As String
        Dim projid As String = ""
        Dim projename As String = ""

        Dim damt As Double = 0
        If Request.QueryString("prid") <> "" Then
            'Response.Write(Request.QueryString("prid"))

        Else
            ' Response.Write(Request.QueryString("month"))
            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                ' Response.Write(Request.QueryString("paidst"))

                If Request.Form("projname") <> "" Or Request.Form("pname") <> "" Then
                    If Request.Form("projname") <> "" Then
                        'If String.IsNullOrEmpty(Request.Form("projname")) = False Then
                        spl = Request.Form("projname").Split("|")
                        projename = Request.Form("projname")
                    Else

                        projename = sec.dbHexToStr(Request.Form("pname"))
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
                    rrr = dbs.dtmake("payrol", "select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='monthly'", Session("con"))
                    ' Response.Write("select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='monthly'")
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
                                   "</td><td class='v1'><div class='viewpayrol' onclick=" & _
                                   Chr(34) & "javascript:payrollview1('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "&ptype=payroll');" & _
                                   Chr(34) & "></div></td><td> &nbsp;|&nbsp;</td><td class='v1'><div class='editpayrol' onclick=" & Chr(34) & _
                                   "javascript:payrollview1('Edit','" & rrr.Item(0) & "');" & _
                                   Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='bankpayrol' onclick=" & Chr(34) & _
                                   "javascript:payrollview1('Bank','?ref=" & rrr.Item(0) & _
                                   "&ppd=" & pdate1 & "');" & Chr(34) & _
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
    Public Function rptpayrollmid()
        Session.Timeout = 60
        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim fl As New file_list
        Dim sec As New k_security
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
        If Request.QueryString("prid") <> "" Then
            '   Response.Write(Request.QueryString("prid"))

        Else
            Response.Write(Request.QueryString("month"))

            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                'Response.Write(fl.msgboxt("onfram", "Progression", "Progression shown"))
                'Response.Write("<script>showobj('progressbar');</script>") 
                Response.Write(Request.QueryString("paidst"))
                Dim spl() As String
                Dim projid As String = ""
                If Request.Form("projname") <> "" Or Request.Form("pname") <> "" Then
                    If String.IsNullOrEmpty(Request.Form("projname")) = False Then
                        spl = Request.Form("projname").Split("|")
                    Else
                        spl = Request.Form("pname").Split("|")
                    End If

                    If spl.Length > 1 Then
                        projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                    Else
                        projid = ""
                    End If

                End If
                If String.IsNullOrEmpty(Request.Form("month")) = False Then
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
                Dim ccol2 As Integer
                Dim reasonname() As String
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
                    'Response.Write("<br>ref:" & paid)
                    rrr = dbs.dtmake("payrol", "select distinct ref,date_paid,remark from payrollx where pr='" & paid & "' and remark='pay_inc_middle'", Session("con"))
                    If rrr.HasRows Then
                        Response.Write("<div id='viewlistx'>Payroll salary increment in the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "<table>")
                        Response.Write(Request.Form("projname"))
                        Dim ccout As String
                        While rrr.Read
                            'Response.Write(rrr.Item(0))
                            ' Response.Write(projid)
                            If projid.ToString <> "" Then
                                'Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')", Session("con")))
                                ccout = "0"
                                ccout = fm.getinfo2("select count(id) from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                                ' Response.Write(ccout)
                                If rrr.IsDBNull(2) = False Then
                                    If LCase(rrr.Item("remark")) = "pay_inc_middle" Then
                                        ' Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "')and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "') )", Session("con")).ToString & " = " & projid.ToString)
                                        ' If fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "')and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "') )", Session("con")).ToString = projid.ToString Then
                                        Response.Write("<tr><td class='listcont'>" & rrr.Item(0) & "<span style='color:gray;font-size:10pt;'><br>(No. List:" & ccout & ")</span>" & _
                                        "</td><td class='v1'><div class='viewpayrol' onclick=" & _
                                        Chr(34) & "javascript: payrollmid('view','?prid=" & rrr.Item(0) & "&pd=" & rrr.Item(1) & "');" & _
                                        Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='editpayrol' onclick=" & Chr(34) & _
                                        "javascript:gotocheckpx('Edit','" & rrr.Item(0) & "');" & _
                                        Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='bankpayrol' onclick=" & Chr(34) & _
                                        "javascript:gotocheckpx('Bank','?ref=" & rrr.Item(0) & _
                                        "&ppd=" & pdate1 & "');" & Chr(34) & _
                                        "></div></td></tr><tr><td colspan=8><hr style='width:600px;' align=left></td></tr>")
                                        ' End If
                                    End If
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
    Public Function rptpayment()
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
                    If Request.Form("projname") <> "" Or Request.Form("pname") <> "" Then
                        If String.IsNullOrEmpty(Request.Form("projname")) = False Then
                            spl = Request.Form("projname").Split("|")
                            projename = Request.Form("projname")
                        Else
                            spl = Request.Form("pname").Split("|")
                            projename = Request.Form("pname")
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
                        rrr = dbs.dtmake("payrol", "select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='Increament'", Session("con"))
                        ' Response.Write("select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='monthly'")
                        If rrr.HasRows Then

                            Response.Write("<div id='viewlistx'><b>Project: " & projename & "<br>Other Payments in the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</b><table>")
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
                                    "</td><td class='v1'><div class='viewpayrol' onclick=" & _
                                    Chr(34) & "javascript:otherpayment('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='editpayrol' onclick=" & Chr(34) & _
                                    "javascript:otherpayment('Edit','" & rrr.Item(0) & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='bankpayrol' onclick=" & Chr(34) & _
                                    "javascript:otherpayment('Bank','?ref=" & rrr.Item(0) & _
                                    "&ppd=" & pdate1 & "');" & Chr(34) & _
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
    Public Function rptot()
        ' Response.Write("itisin")
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
            otback(Request.QueryString("prid"))
        Else
            ' Response.Write(Request.QueryString("month"))
            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                Response.Write(Request.QueryString("paidst"))
                Dim spl() As String
                Dim projid As String = ""
                Dim projename As String = ""
                If Request.Form("projname") <> "" Or Request.Form("pname") <> "" Then
                    If Request.Form("projname") <> "" Then
                        'If String.IsNullOrEmpty(Request.Form("projname")) = False Then
                        spl = Request.Form("projname").Split("|")
                        projename = Request.Form("projname")
                    Else

                        projename = sec.dbHexToStr(Request.Form("pname"))
                        spl = projename.Split("|")
                    End If

                    If spl.Length > 1 Then
                        projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                    Else
                        projid = ""
                    End If

                End If
                ' Response.Write(Request.Form("pname"))
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
                    rrr = dbs.dtmake("payrol", "select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='OT-Payment'", Session("con"))
                    If rrr.HasRows Then
                        Response.Write("<div id='viewlistx'><b>Project: " & projename & "<br>OT backpayments in the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</b><table>")
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
                                    "</td><td class='v1'><div class='viewpayrol' onclick=" & _
                                    Chr(34) & "javascript:otview('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='editpayrol' onclick=" & Chr(34) & _
                                    "javascript:otview('Edit','" & rrr.Item(0) & "');" & _
                                    Chr(34) & "></div></td><td>&nbsp;|&nbsp;</td><td class='v1'><div class='bankpayrol' onclick=" & Chr(34) & _
                                    "javascript:otview('Bank','?ref=" & rrr.Item(0) & _
                                    "&ppd=" & pdate1 & "');" & Chr(34) & _
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
    Public Function makeformpaidx_payroll() 'View
        'view paid employees
        ' Response.Write(Request.QueryString("prid"))
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
                '  Response.Write(ref)
                pdate2 = fm.getinfo2("select date_paid from payrollx where ref='" & ref & "'", Session("con"))

                Dim spl() As String
                '  nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                '  pdate2 = Request.QueryString("pd")
                nod = Date.DaysInMonth(pdate2.Year, pdate2.Month)
                pdate1 = pdate2.Month & "/1/" & pdate2.Year
                Dim rs As DataTableReader
                Dim rs2 As DataTableReader
                Dim ccol As Integer = 0
                Dim paid As String
                Dim j As Integer
                Dim ccol2 As Integer
                Dim reasonname() As String
                Dim paypaid As Integer = 0
                Dim xemp As String = ""
                xemp = fm.getinfo2("select emptid from payrollx where ref='" & ref & "'", Session("con"))

                spl = fm.getproj_on_date(xemp, pdate2, Session("con"))

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
                    'Response.Write(Request.Form("pname"))
                    outp &= spl(1)
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
                    outp &= fm.projtrans(spl(1), pdate1, Session("con"))
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

    Public Function viewlist()
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

            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                ' Response.Write(Request.QueryString("paidst"))
                Dim spl() As String
                Dim projid As String = ""
                Dim projename As String = ""
                If Request.Form("projname") <> "" Or Request.Form("pname") <> "" Then
                    If Request.Form("projname") <> "" Then
                        'If String.IsNullOrEmpty(Request.Form("projname")) = False Then
                        spl = Request.Form("projname").Split("|")
                        projename = Request.Form("projname")
                    Else

                        projename = sec.dbHexToStr(Request.Form("pname"))
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
                    rrr = dbs.dtmake("payrol", "select distinct ref,date_paid,remark from payrollx where pr='" & paid & "'", Session("con"))
                    ' Response.Write("select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='monthly'")
                    Dim fx() As String = {""}
                    If rrr.HasRows Then
                        Dim remark As String
                        Dim pname As String
                        pname = Request.Form("projname")

                        Response.Write("<div id='viewlistx'><b>Project: " & projename & "<br>Payroll in the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</b><table>")
                        Dim ccout As String = "0"
                        While rrr.Read
                            ' Response.Write(rrr.Item(0))
                            remark = fm.getinfo2("select remark from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
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
                                   "</td>")

                                    If String.IsNullOrEmpty(Session("right")) = False Then
                                        fx = Session("right").split(";")
                                        ReDim Preserve fx(UBound(fx) + 1)
                                        fx(UBound(fx) - 1) = ""
                                    End If
                                   
                                    Response.Write("<td class='v1'><div class='viewpayrol' onclick=" & _
                                    Chr(34) & "javascript:rptviewx('" & rrr.Item(0) & "','" & pname & "','" & remark & "');" & _
                                    Chr(34) & "></div></td></tr><tr><td colspan='8'><hr style='width:600px;' align=left></td></tr>")
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
    Function gotomake(ByVal rmk As String)
        Select Case rmk
            Case "Increament"
                Increment(Request.Form("ref"))
            Case "pay_inc_middle"
                payrollmid(Request.Form("ref"))
            Case "monthly"
                payroll(Request.Form("ref"))
            Case "OT-Payment"
                otback(Request.Form("ref"))
        End Select

    End Function
    Public Function Increment(ByVal ref As String) 'view
        'Dim ref As String = ""
        Dim xoutp As String = ""

        If ref <> "" Then
           
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
            ' CDate(pd).ToShortDateString()
            ' Response.Write("<br>Pd=>" & pd & "<br>")
            If pd = "None" Then
                Response.Write("<br>" & ref & "<<<<+ref<br>")
            End If
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
            Dim getpro() As String
            Try
                getpro = fm.getproj_on_date(idlist, CDate(pd).ToShortDateString, Session("con"))
            Catch ex00 As Exception
                Response.Write(emptid & "==>" & CDate(pd).ToShortDateString & "<br>" & ex00.ToString)
            End Try
            ' Response.Write(getpro(1))
            Dim projid As String = ""
            If getpro(0) <> "" Then
                Dim spl() As String

                If Request.Form("pname") <> "" Then
                    spl = sec.dbHexToStr(Request.Form("pname")).Split("|")
                    If spl(1) = getpro(1) Then
                        Response.Write("Pass")
                    End If
                    If spl.Length <= 1 Then
                        ReDim spl(2)
                        spl(0) = sec.dbHexToStr(Request.Form("pname"))
                        spl(1) = ""
                    End If
                    projid = spl(0) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                End If
                projid = getpro(0)
                Try


                    nod = Date.DaysInMonth(CDate(pd).Year, CDate(pd).Month)
                    pdate3 = CDate(pd).Month & "/1/" & CDate(pd).Year
                    pdate4 = CDate(pd).Month & "/" & nod & "/" & CDate(pd).Year
                    pdate1 = pdate3.AddMonths(-1)
                    pdate2 = pdate1.Month & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year
                    Dim rs As DataTableReader
                    Dim rs2 As DataTableReader
                    ' Response.Write(pdate1 & "=====>" & pdate2 & "=====>" & pdate3 & "=====>" & pdate4 & "<br>")
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
                        "<br> Project Name:" & getpro(1)
                        
                        outp &= "<br><label id='spnid'></label> payment Sheet for the month: " & MonthName(pdate3.Month) & " " & pdate3.Year & Chr(13)
                        outp &= "<br><label id='spnid'></label> Ref: " & ref & "</td>" & Chr(13)

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
                    outp &= "</tr>"
                    Dim resss As String = ""
                    pd = fm.getinfo2("select pddate from payrollx where ref='" & ref & "'", Session("con"))

                    outp &= "<tr><td colspan=4 style='color:blue'>Paid Date: " & CDate(pd).ToShortDateString
                    pd = fm.getinfo2("select who_reg from payrollx where ref='" & ref & "'", Session("con"))

                    outp &= " Registerd by: " & pd & "</td></tr></table>"
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
    Public Function payroll(ByVal ref As String) 'View
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
        'Dim ref As String

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


            If ref <> "" Then
                'Response.Write(fl.msgboxt("onfram", "Progression", "Progression shown"))
                'Response.Write("<script>showobj('progressbar');</script>")
                'ref = Request.QueryString("prid")
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
                    'Response.Write(Request.Form("pname"))
                    outp &= (sec.dbHexToStr(Request.Form("pname")))
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
                    outp &= fm.projtrans(sec.dbHexToStr(Request.Form("pname")), pdate1, Session("con"))
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
    Public Function payrollmid(ByVal ref As String)
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
        ' Dim ref As String
        Dim sec As New k_security
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
        Dim sal As String
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        Dim sql As String
        Dim outp As String = ""
        Dim calc, ca, clwp, newemp As Double
        Dim damt As Double = 0
        'For Each k As String In Request.ServerVariables
        'Response.Write(k & "=" & Request.ServerVariables(k) & "<br>")
        '  Next
        If ref <> "" Then
            'Response.Write(fl.msgboxt("onfram", "Progression", "Progression shown"))
            'Response.Write("<script>showobj('progressbar');</script>")
            'ref = Request.QueryString("prid")
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
            If ref <> "" Then
                rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from payrollx where ref='" & ref.ToString & "') ORDER BY emp_static_info.first_name", Session("con"))

            End If
            outp = "Sorry Can't Process"
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                outp &= "<td class='toptitle' style='text-align:center;font-weight:bold;' colspan='22' >" & Session("company_name") & _
                "<br> Project Name:"
                'rs.Read()
                outp &= sec.dbHexToStr(Request.Form("pname"))
                outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & _
                "<br>" & ref & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td class='dw' rowspan='2' >No.</td><td class='dw' rowspan='2'>Emp. Id</td>"
                outp &= "<td class='fxname'  rowspan='2'>Full Name</td>"
                outp &= "<td class='dedct' colspan='3'>Basic Salary</td>" & Chr(13)
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


                outp &= "<td class='dw' id='chkall' style='cursor:pointer;width:30px;' rowspan='2' onclick='javascript:checkall();'>&nbsp;</td>" & Chr(13)

                outp &= "<td rowspan='2' class='signpart'>Signature</td></tr>" & Chr(13)

                rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanref where ref='" & ref & "' order by reason", Session("con"))


                ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", Session("con"))
                outp &= "<tr>" & Chr(13)
                outp &= "<td class='dedctx'>Prev.<br>Salary</td><td class='dedctx'>New Salary</td><td class='dedctx'>Salary</td>"
                If ccol = 0 Then
                    outp &= "<td class='dedctx'>&nbsp;</td>"
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
                    Else
                        outp &= "<td class='dedctx'> &nbsp;</td>"
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
                Dim dateincr As Date
                Dim csal(2) As String
                While rs.Read
                    ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))

                    emptid = rs.Item("id")
                    paid = ""
                    paid = fm.getinfo2("select id from payrollx where ref='" & ref & "'", Session("con"))
                    '  Response.Write(paid.ToString & "<br>...")
                    Dim incwhen As Integer = 0
                    avalue = ""
                    avalue = fm.getinfo2("select date_start from emp_sal_info where emptid=" & emptid & " order by id desc", Session("con"))
                    If avalue <> "None" Then
                        dateincr = avalue
                    End If
                    Dim dhr As Date
                    dhr = rs.Item("hire_date")
                    If dateincr.Month = pdate1.Month And dateincr.Year = pdate1.Year And DateDiff("m", dhr, pdate1) <> 0 Then
                        'Response.Write(rs.Item("emp_id") & "===>" & dateincr.Subtract(pdate1).Days.ToString & "<br>")
                        incwhen = dateincr.Subtract(pdate1).Days
                    End If

                    ' Response.Write("<br>" & rs.Item("emp_id") & resing.ToString)

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
                                    outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                End If
                            ElseIf i = 2 Then
                                outp &= ("<td style='text-align:right;width:70px;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-1'>" & fm.numdigit(csal(0).ToString, 2) & "<br>(" & incwhen.ToString & " days)</td>")

                                outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "-2'>" & fm.numdigit(csal(1).ToString, 2) & "<br>(" & (nod - incwhen).ToString & " days)</td>")

                                outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

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
                outp &= fm.projtrans(Request.Form("pname"), pdate1, Session("con"))

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
    Public Function otback(ByVal ref As String)
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
        Dim spl() As String
        Dim projid As String = ""
        ' Response.Write(Request.QueryString("month"))
        ' Dim ref As String = ""
        If ref <> "" Then

            If Request.Form("pname") <> "" Then
                spl = sec.dbHexToStr(Request.Form("pname")).Split("|")
                If spl.Length > 1 Then
                    projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
                Else
                    projid = ""
                End If


            End If
            If ref <> "" Then
                pdate2 = fm.getinfo2("select date_paid from payrollx where ref='" & ref & "'", Session("con"))
                nod = Date.DaysInMonth(pdate2.Year, pdate2.Month)
                pdate1 = pdate2.Month & "/1/" & pdate2.Year
                emptid = fm.getinfo2("select emptid from payrollx where ref='" & ref & "'", Session("con"))
                spl = fm.getproj(emptid, pdate1, pdate2, Session("con"))
                projid = spl(0)
                For i As Integer = 0 To UBound(spl)
                    Response.Write("<br>" & spl(i) & "</br>")
                Next
            End If
            ' ref = Request.QueryString("prid")

            ' pdate2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
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
            paid = fm.getinfo2("select id from payrol_reg where month='" & pdate2.Month & "' and year='" & pdate2.Year & "' and id in (select payroll_id from paryrollx ref_inc is null)", Session("con"))
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
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
            End If
            'Response.Write("ddd")
            If rs.HasRows Then
                outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:1pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='15' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(1).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br> Over Time Back Pay Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & "<br> Ref:" & ref & "</td>" & Chr(13)

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
            outp &= fm.projtrans(Request.Form("pname"), pdate1, Session("con"))

            fm = Nothing
            dbs = Nothing
            Response.Write(outp)



            Dim xprint As String = ""
            ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, session("con"))

            Return outp

        End If
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
End Class
