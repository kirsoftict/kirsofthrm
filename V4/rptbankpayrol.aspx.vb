Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class rptbankpayrol
    Inherits System.Web.UI.Page
    Public Function makeform() 'Summerised 

        Dim outp As String = ""

        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim coll(5) As String
        Dim spl() As String
        Dim projid As String = ""
        Dim rs As DataTableReader
        Dim copy As String = ""
        Dim i As Integer = 1
        Dim sumt, sump, sumpd As Double
        Dim sumpd1 As Double
        Dim sump1 As String
        Dim pdate As Date
        Dim amtot As String
        Dim nod As Integer
        Dim paiddate As Date
        Dim ref As String = ""
        nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
        Dim pdate1 As Date
        pdate = Request.QueryString("month") & "/" & nod.ToString & "/" & Request.QueryString("year")
        pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
        sumpd1 = 0
        sump1 = 0
        sumt = sump = sumpd = 0
        spl = Request.Form("projname").Split("|")
        If spl.Length <= 1 Then
            ReDim spl(2)
            spl(0) = Request.Form("projname")
            spl(1) = ""
        End If
        outp = ""
        projid = spl(1)

        Dim sql As String
        Dim idclose As String

        If Request.QueryString("ref") <> "" Then
            sql = "select emptid,ref,date_paid from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where Payrollx.ref='" & Request.QueryString("ref") & "' and payrollx.pay_mathd='Bank' and payrollx.bank='" & Request.QueryString("bname") & "' order by emp_static_info.first_name"
        Else
            sql = "select emptid,ref from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where emptid in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate & "') or date_from between '" & pdate1 & "' and '" & pdate & "')) and date_paid='" & pdate & "' order by emp_static_info.first_name"

        End If
        '   Response.Write(sql)
        rs = ds.dtmake("vwapp", sql, Session("con"))


        If rs.HasRows = True Then

            outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
            outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
            outp &= "<td style='text-align:center;font-weight:bold;' colspan='7' >" & Session("company_name") & _
            "<br> Project Name:"
            If projid <> "" Then
                outp &= spl(0).ToString
            Else
                outp &= "All Projects"
            End If
            outp &= "<br>List of Account Number for month: " & MonthName(Request.QueryString("month")) & " " & Request.QueryString("year") & _
                "<br>Paid From: " & fm.getinfo2("select bank_name from tblbanks where abr='" & Request.QueryString("bname") & "'", Session("con")) & "</td>" & Chr(13)

            outp &= "</tr>" & Chr(13)

            outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
            "<td style='width:10px;'>No.</td><td class='fxname'  style='width:200px;'>Employee Name</td><td class='fitx'  style='width:50px;'>Total Income</td><td class='fitx'  style='width:100px;'>Account No.</td><td class='fitx'  style='width:80px;'>Bank</td><td class='fitx'  style='width:80px;'>Branch</td></tr>"
            Dim prd As String
            Dim emptid As String
            Dim arr() As String = {""}
            Dim binfo() As String = {"", "", ""}
            While rs.Read
                coll(0) = ""
                coll(1) = ""
                coll(2) = ""
                coll(3) = ""
                coll(4) = ""
                coll(5) = ""
                emptid = rs.Item("emptid")
                ref = rs.Item("ref")
                If fm.searcharray(arr, emptid) = False Then
                    ReDim Preserve arr(UBound(arr) + 1)
                    arr(UBound(arr) - 1) = emptid


                    coll(0) = i.ToString
                    coll(1) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con")), Session("con"))

                    If ref <> "" Then

                        sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                        paiddate = fm.getinfo2("select pddate from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                        '  Response.Write(paiddate.ToShortDateString & "<br>")
                        prd = fm.getinfo2("select total from pardim_sum where emptid=" & emptid & " and paid_date='" & paiddate.ToShortDateString & "'  and mthd='Bank'", Session("con"))
                        ' Response.Write(prd & " select total from pardim_sum where emptid=" & emptid & " and paid_date='" & paiddate.ToShortDateString & "'  and mthd='Bank'<br>")

                        If IsNumeric(prd) = True Then
                            sumpd1 = CDbl(prd)
                        Else
                            sumpd1 = 0
                        End If
                        'Response.Write(sumpd1.ToString & "<br>")
                        ' Response.Write("select total from pardim_sum where emptid=" & emptid & " and paid_date='" & paiddate.ToShortDateString & "'  and mthd='Bank'<br>")
                    Else
                        sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and date_paid='" & pdate & "'", Session("con"))
                        prd = fm.getinfo2("select total from pardim_sum where emptid=" & emptid & " and paid_date='" & pdate.ToShortDateString & "'", Session("con"))
                        If IsNumeric(prd) = True Then
                            sumpd1 = CDbl(prd)
                        Else
                            sumpd1 = 0
                        End If
                    End If

                    ' Response.Write(sump1 & "<br>select sum(netpay) as exp1 from paryrol where emptid=" & rs.Item("emptid") & " and pay_date='" & pdate & "'")
                    If IsNumeric(sump1) = False Then
                        'Response.Write(sump1.ToString)
                        sump1 = "0"

                    End If


                    coll(2) = fm.numdigit(CDbl((sumpd1 + CDbl(sump1)).ToString), 2).ToString
                    amtot = "0"
                    If Request.QueryString("ot") = "on" Then
                        Dim epid As String
                        epid = empids(emptid)
                        'Response.Write(epid & "<br>")
                        amtot = fm.getinfo2("select sum(netp) from payrollx where emptid in(" & epid & ") and pddate='" & paiddate & "' and remark='OT-Payment'", Session("con"))
                        ' Response.Write(amtot.ToString)
                    End If
                    If IsNumeric(amtot) = True Then
                        coll(2) = CDbl(coll(2)) + CDbl(amtot)

                    End If
                    Dim empidx As String
                    empidx = checkbankacc(emptid)
                    If empidx = "" Then
                        empidx = emptid
                    End If
                    binfo(0) = ""
                    binfo(1) = ""
                    binfo(2) = ""
                    binfo = getbankinf(empidx, pdate)
                    'Response.Write(binfo(0) & "<br>")
                    'coll(3) = fm.getinfo2("select accountno from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))
                    coll(3) &= binfo(0)

                    'coll(4) = fm.getinfo2("select bankname from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con")) & "......"
                    coll(4) &= binfo(1)
                    'coll(5) = fm.getinfo2("select branch from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))
                    coll(5) &= binfo(2)
                    If Request.QueryString("acc") = "None" Then

                        If coll(3) = "None" Then
                            i += 1
                            sumt += sumpd1 + sump1
                            sumpd += sumpd1
                            sump += sump1
                            If IsNumeric(amtot) = True Then
                                sumt += CDbl(amtot)

                            End If
                            outp &= "<tr>"
                            For j As Integer = 0 To coll.Length - 1
                                If j = 2 Then
                                    outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                Else
                                    outp &= "<td>" & coll(j) & "</td>"
                                End If
                            Next
                            outp &= "</tr>"
                        End If
                    ElseIf Request.QueryString("acc") = "acc" Then
                        '  Response.Write(coll(3) & "<br>")
                        If coll(3) <> "None" Then
                            sumt += sumpd1 + sump1
                            sumpd += sumpd1
                            sump += sump1
                            If IsNumeric(amtot) = True Then
                                sumt += CDbl(amtot)

                            End If
                            i = i + 1

                            outp &= "<tr>"
                            For j As Integer = 0 To coll.Length - 1
                                If j = 2 Then

                                    outp &= "<td style='text-align:right;'>" & FormatNumber(coll(j), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                Else
                                    outp &= "<td>" & coll(j) & "</td>"
                                End If
                            Next
                            outp &= "</tr>"

                        End If
                    Else

                        sumt += sumpd1 + sump1
                        sumpd += sumpd1
                        sump += sump1
                        If IsNumeric(amtot) = True Then
                            sumt += CDbl(amtot)

                        End If
                        i += 1
                        outp &= "<tr>"
                        For j As Integer = 0 To coll.Length - 1
                            If j = 2 Then
                                outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                            Else
                                outp &= "<td>" & coll(j) & "</td>"
                            End If
                        Next
                        outp &= "</tr>"

                    End If

                End If
            End While
            outp &= "<tr><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;font-weight:bold;'>&nbsp;" & fm.numdigit(CDbl(sumt), 2).ToString & "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
            outp &= "</table>"
            Response.Write(outp)
        Else
            Response.Write("Sorry, Data is not Existed" & ref)
        End If
        rs.Close()
        fm = Nothing
        ds = Nothing

        Return outp

    End Function
    Public Function makeform2() As String 'Detail part
        Dim outp As String = ""

        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim coll(9) As String
        Dim spl() As String
        Dim projid As String = ""
        Dim rs As DataTableReader
        Dim copy As String = ""
        Dim i As Integer = 1
        Dim sumt, sump, sumpd As Double
        Dim sumpd1, sumot, sump1 As Double
        Dim pdate, ppdate As Date
        Dim nod As Integer
        Dim paiddate As Date
        nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
        pdate = Request.QueryString("month") & "/" & nod.ToString & "/" & Request.QueryString("year")
        Dim pdate1 As Date = Request.QueryString("month") & "/1/" & Request.QueryString("year")

        sumpd1 = 0
        sump1 = 0
        sumt = sump = sumpd = 0
        Dim ref As String = Request.QueryString("ref")
        spl = Request.Form("projname").Split("|")
        If spl.Length <= 1 Then
            ReDim spl(2)
            spl(0) = Request.Form("projname")
            spl(1) = ""
        End If
        outp = ""
        projid = spl(1)
        Dim sql As String
        If Request.QueryString("ref") <> "" Then
            'Response.Write("Go with")
            sql = "select emptid from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where Payrollx.ref='" & Request.QueryString("ref") & "' and payrollx.pay_mathd='Bank' order by emp_static_info.first_name"
        Else
            sql = "select emptid from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where emptid in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate & "') or date_from between '" & pdate1 & "' and '" & pdate & "')) and pddate='" & pdate & "' order by emp_static_info.first_name"

        End If
        '  Response.Write(sql)
        Try
            Dim col As Integer = 0

            rs = ds.dtmake("vwapp", sql, Session("con"))

            If rs.HasRows = True Then

                outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='" & coll.Length & "' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br>List of Account Number for month: " & MonthName(Request.QueryString("month")) & " " & Request.QueryString("year") & _
                "<br>Paid From: " & fm.getinfo2("select bank_name from tblbanks where abr='" & Request.QueryString("bname") & "'", Session("con")) & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
                "<td>No.</td><td>Employee Name</td><td>Payroll Income</td><td>Perdiem</td>"

                If Request.QueryString("ot") = "on" Then
                    outp &= "<td>Over Time</td>"
                    col = 1
                End If
                outp &= "<td>Total</td><td>Account No.</td><td>Bank</td><td>Branch</td></tr>"
                Dim prd As String
                Dim emptid As String
                Dim arr() As String = {""}
                Dim binfo() As String = {"", "", ""}
                While rs.Read
                    coll(0) = ""
                    coll(1) = ""
                    coll(2) = ""
                    coll(3) = ""
                    coll(4) = ""
                    coll(5) = ""
                    coll(6) = ""
                    coll(7) = ""
                    coll(8) = ""
                    emptid = rs.Item("emptid")

                    If fm.searcharray(arr, emptid) = False Then
                        ReDim Preserve arr(UBound(arr) + 1)
                        arr(UBound(arr) - 1) = emptid


                        coll(0) = i.ToString
                        coll(1) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con")), Session("con"))
                        ' prd = fm.getinfo2("select total from pardim_sum where emptid=" & emptid & " and paid_date='" & pdate & "'", Session("con"))
                        ' Response.Write(prd.ToString & "<br>")

                        If ref <> "" Then
                            sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            paiddate = fm.getinfo2("select pddate from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))

                        Else
                            sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and pddate='" & pdate & "'", Session("con"))
                            paiddate = fm.getinfo2("select date_paid from payrollx where emptid=" & emptid, Session("con"))
                        End If
                        ' Response.Write(paiddate.ToShortDateString)
                        prd = fm.getinfo2("select total from pardim_sum where emptid=" & emptid & " and paid_date='" & paiddate.ToShortDateString & "' and mthd='Bank'", Session("con"))
                        '  Response.Write("<br>select total from pardim_sum where emptid=" & emptid & " and paid_date='" & pdate.ToShortDateString & "' and mthd='Bank'")
                        'Response.Write(prd.ToString)
                        If IsNumeric(prd) = True Then
                            sumpd1 = prd
                        Else
                            sumpd1 = 0
                        End If
                        If IsNumeric(sump1) = False Then
                            ' Response.Write(sump1.ToString)
                            sump1 = "0"

                        End If



                        '  Response.Write(sump1 & "<br>select sum(netpay) as exp1 from paryrol where emptid=" & rs.Item("emptid") & " and pay_date='" & pdate & "'")

                        coll(2) = fm.numdigit(CDbl((sump1).ToString), 2).ToString
                        coll(3) = fm.numdigit(CDbl((sumpd1).ToString), 2).ToString
                        coll(8) = ""
                        If Request.QueryString("ot") = "on" Then
                            Dim epid As String
                            epid = empids(emptid)
                            'Response.Write(epid & "<br>")
                            coll(8) = fm.getinfo2("select sum(netp) from payrollx where emptid in(" & epid & ") and pddate='" & paiddate & "' and remark='OT-Payment'", Session("con"))
                            If String.IsNullOrEmpty(coll(8).ToString) = True Then
                                coll(8) = "0"
                            End If
                            '  Response.Write(coll(8).ToString & "<br>")
                        End If
                        If coll(8) = "None" Then
                            coll(8) = "0"
                        End If
                        coll(4) = fm.numdigit(CDbl((sumpd1 + sump1).ToString), 2).ToString
                        Dim empidx As String
                        empidx = checkbankacc(emptid)
                        If empidx = "" Then
                            empidx = emptid
                        End If
                        binfo(0) = ""
                        binfo(1) = ""
                        binfo(2) = ""
                        binfo = getbankinf(empidx, pdate)
                        coll(5) = binfo(0) 'fm.getinfo2("select accountno from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))
                        coll(6) = binfo(1) 'fm.getinfo2("select bankname from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))

                        coll(7) = binfo(2) ' fm.getinfo2("select branch from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))
                        If Request.QueryString("acc") = "None" Then

                            If coll(5) = "None" Then
                                i += 1
                                sumt += sumpd1 + sump1
                                sumpd += sumpd1
                                sump += sump1
                                outp &= "<tr>"
                                For j As Integer = 0 To coll.Length - 2
                                    If j = 2 Or j = 3 Then
                                        outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                    ElseIf j = 4 Then
                                        If IsNumeric(coll(8)) = True Then
                                            outp &= "<td style='text-align:right;'>" & coll(8) & "</td>"
                                            coll(j) += CDbl(coll(8))
                                            sumot += CDbl(coll(8))
                                            sumt += CDbl(coll(8))
                                        End If

                                        outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                    Else
                                        outp &= "<td>" & coll(j) & "</td>"
                                    End If
                                Next
                                outp &= "<td>" & coll(8).ToString & "</td></tr>"
                            End If
                        ElseIf Request.QueryString("acc") = "acc" Then


                            If coll(5) <> "None" Then
                                sumt += sumpd1 + sump1
                                sumpd += sumpd1
                                sump += sump1
                                i = i + 1

                                outp &= "<tr>"
                                For j As Integer = 0 To coll.Length - 3
                                    If j = 2 Or j = 3 Then

                                        outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                    ElseIf j = 4 Then
                                        If IsNumeric(coll(8)) = True Then
                                            outp &= "<td style='text-align:right;'>" & FormatNumber(coll(8), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                            coll(j) = CDbl(coll(j)) + CDbl(coll(8))
                                            sumot += CDbl(coll(8))
                                            sumt += CDbl(coll(8))
                                        End If

                                        outp &= "<td style='text-align:right;'>" & FormatNumber(coll(j), 2, TriState.True, TriState.True, TriState.True) & "</td>"

                                    Else

                                        outp &= "<td>" & coll(j) & "</td>"
                                    End If
                                Next
                                outp &= "</tr>"

                            End If
                        Else
                            sumt += sumpd1 + sump1
                            sumpd += sumpd1
                            sump += sump1
                            i += 1
                            outp &= "<tr>"
                            For j As Integer = 0 To coll.Length - (2 + 1)
                                If j = 2 Or j = 3 Then
                                    outp &= "<td style='text-align:right;'>" & fm.numdigit(CDbl(coll(j)), 2).ToString & "</td>"
                                ElseIf j = 4 Then
                                    If IsNumeric(coll(8)) = True Then
                                        outp &= "<td style='text-align:right;'>" & FormatNumber(coll(8), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                        coll(j) = CDbl(coll(j)) + CDbl(coll(8))
                                        sumot += CDbl(coll(8))
                                        sumt += CDbl(coll(8))
                                    End If

                                    outp &= "<td style='text-align:right;'>" & FormatNumber(coll(j), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                Else
                                    outp &= "<td>" & coll(j) & "</td>"
                                End If
                            Next
                            outp &= "</tr>"

                        End If
                    End If
                End While
                outp &= "<tr><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;font-weight:bold;'>&nbsp;" & fm.numdigit(CDbl(sump), 2).ToString & "</td><td style='text-align:right;font-weight:bold;'>&nbsp;" & fm.numdigit(CDbl(sumpd), 2).ToString & "</td>"
                If Request.QueryString("ot") = "on" Then
                    outp &= "<td style='text-align:right;font-weight:bold;'>&nbsp;" & fm.numdigit(CDbl(sumot), 2).ToString & "</td>"
                End If
                outp &= "<td style='text-align:right;font-weight:bold;'>&nbsp;" & fm.numdigit(CDbl(sumt), 2).ToString & "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                outp &= "</table>"
                Response.Write(outp)
            Else
                Response.Write("Sorry, Data is not Existed" & ref)
            End If
            rs.Close()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


        fm = Nothing
        ds = Nothing

        Return outp
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
    Function checkbankacc(ByVal emptid As Integer)
        Dim rs As DataTableReader
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim emp_id As String
        Dim bkacc As String
        emp_id = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))

        rs = ds.dtmake("getid", "select id from emprec where emp_id='" & emp_id & "' order by id desc", Session("con"))
        If rs.HasRows = True Then
            While rs.Read
                bkacc = fm.getinfo2("select accountno from empbank where emptid=" & rs.Item(0), Session("con"))
                If bkacc = "None" Then
                Else
                    Return rs.Item(0)

                End If
            End While
        End If
        rs = Nothing
        ds = Nothing
        fm = Nothing
    End Function
    Function makeformot()
        Dim outp As String = ""
        If Request.Form("projname") <> "" Then
            Dim ds As New dbclass
            Dim fm As New formMaker
            Dim coll(5) As String
            Dim spl() As String
            Dim projid As String = ""
            Dim rs As DataTableReader
            Dim copy As String = ""
            Dim i As Integer = 1
            Dim sumt, sump, sumpd As Double
            Dim sumpd1 As Double
            Dim sump1 As String
            Dim pdate As Date
            Dim amtot As String
            Dim nod As Integer
            Dim paiddate As Date
            Dim ref As String = ""
            nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
            Dim pdate1 As Date
            pdate = Request.QueryString("month") & "/" & nod.ToString & "/" & Request.QueryString("year")
            pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
            sumpd1 = 0
            sump1 = 0
            sumt = sump = sumpd = 0
            spl = Request.Form("projname").Split("|")
            If spl.Length <= 1 Then
                ReDim spl(2)
                spl(0) = Request.Form("projname")
                spl(1) = ""
            End If
            outp = ""
            projid = spl(1)

            Dim sql As String

            If Request.QueryString("ref") <> "" Then

                sql = "select emptid,ref,date_paid from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where Payrollx.ref='" & Request.QueryString("ref") & "' and payrollx.pay_mathd='Bank' order by emp_static_info.first_name"
            Else
                sql = "select emptid,ref from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where emptid in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate & "') or date_from between '" & pdate1 & "' and '" & pdate & "')) and date_paid='" & pdate & "' order by emp_static_info.first_name"

            End If
            ' Response.Write(sql)
            rs = ds.dtmake("vwapp", sql, Session("con"))

            If rs.HasRows = True Then

                outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='7' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br>List of Account Number for month: " & MonthName(Request.QueryString("month")) & " " & Request.QueryString("year") & _
                "<br>Paid From: " & fm.getinfo2("select bank_name from tblbanks where abr='" & Request.QueryString("bname") & "'", Session("con")) & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
                "<td style='width:10px;'>No.</td><td class='fxname'  style='width:200px;'>Employee Name</td><td class='fitx'  style='width:50px;'>Total OT Income</td><td class='fitx'  style='width:100px;'>Account No.</td><td class='fitx'  style='width:80px;'>Bank</td><td class='fitx'  style='width:80px;'>Branch</td></tr>"
                Dim prd As String
                Dim emptid As String
                Dim arr() As String = {""}
                Dim binfo() As String = {"", "", ""}
                While rs.Read
                    coll(0) = ""
                    coll(1) = ""
                    coll(2) = ""
                    coll(3) = ""
                    coll(4) = ""
                    coll(5) = ""
                    emptid = rs.Item("emptid")
                    ref = rs.Item("ref")
                    If fm.searcharray(arr, emptid) = False Then
                        ReDim Preserve arr(UBound(arr) + 1)
                        arr(UBound(arr) - 1) = emptid


                        coll(0) = i.ToString
                        coll(1) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con")), Session("con"))

                        If ref <> "" Then

                            sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            paiddate = fm.getinfo2("select date_paid from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                           
                            sumpd = 0
                            ' Response.Write("select total from pardim_sum where emptid=" & emptid & " and paid_date='" & paiddate.ToShortDateString & "'  and mthd='Bank'<br>")
                        Else
                            sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and date_paid='" & pdate & "'", Session("con"))
                           
                        End If

                        ' Response.Write(sump1 & "<br>select sum(netpay) as exp1 from paryrol where emptid=" & rs.Item("emptid") & " and pay_date='" & pdate & "'")
                        If IsNumeric(sump1) = False Then
                            Response.Write(sump1.ToString)
                            sump1 = "0"

                        End If


                        coll(2) = fm.numdigit(CDbl(sump1).ToString, 2).ToString
                        amtot = "0"
                       
                        If Request.QueryString("ot") = "on" Then
                            amtot = fm.getinfo2("select sum(netp) from payrollx where emptid =" & emptid & " and pddate='" & paiddate & "' and remark='OT-Payment'", Session("con"))
                            ' Response.Write("in" & emptid)
                        End If
                        If IsNumeric(amtot) = True Then
                            coll(2) = CDbl(coll(2)) + CDbl(amtot)

                        End If
                        Dim empidx As String
                        empidx = checkbankacc(emptid)
                        If empidx = "" Then
                            empidx = emptid
                        End If
                        binfo(0) = ""
                        binfo(1) = ""
                        binfo(2) = ""
                        binfo = getbankinf(empidx, pdate)
                        coll(3) = binfo(0) ' fm.getinfo2("select accountno from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))

                        coll(4) = binfo(1) 'fm.getinfo2("select bankname from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))

                        coll(5) = binfo(2) 'fm.getinfo2("select branch from empbank where emptid=" & empidx & " and active='y' order by id desc", Session("con"))
                        If Request.QueryString("acc") = "None" Then

                            If coll(3) = "None" Then
                                i += 1
                                sumt += sumpd1 + sump1
                                sumpd += sumpd1
                                sump += sump1
                                If IsNumeric(amtot) = True Then
                                    sumt += CDbl(amtot)

                                End If
                                outp &= "<tr>"
                                For j As Integer = 0 To coll.Length - 1
                                    If j = 2 Then
                                        outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                    Else
                                        outp &= "<td>" & coll(j) & "</td>"
                                    End If
                                Next
                                outp &= "</tr>"
                            End If
                        ElseIf Request.QueryString("acc") = "acc" Then
                            sumt += sumpd1 + sump1
                            sumpd += sumpd1
                            sump += sump1
                            If IsNumeric(amtot) = True Then
                                sumt += CDbl(amtot)

                            End If
                            If coll(3) <> "None" Then
                                i = i + 1

                                outp &= "<tr>"
                                For j As Integer = 0 To coll.Length - 1
                                    If j = 2 Then

                                        outp &= "<td style='text-align:right;'>" & FormatNumber(coll(j), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                    Else
                                        outp &= "<td>" & coll(j) & "</td>"
                                    End If
                                Next
                                outp &= "</tr>"

                            End If
                        Else
                            sumt += sumpd1 + sump1
                            sumpd += sumpd1
                            sump += sump1
                            If IsNumeric(amtot) = True Then
                                sumt += CDbl(amtot)

                            End If
                            i += 1
                            outp &= "<tr>"
                            For j As Integer = 0 To coll.Length - 1
                                If j = 2 Then
                                    outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                Else
                                    outp &= "<td>" & coll(j) & "</td>"
                                End If
                            Next
                            outp &= "</tr>"

                        End If

                    End If
                End While
                outp &= "<tr><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;font-weight:bold;'>&nbsp;" & fm.numdigit(CDbl(sumt), 2).ToString & "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                outp &= "</table>"
                Response.Write(outp)
            Else
                Response.Write("Sorry, Data is not Existed" & ref)
            End If
            rs.Close()
            fm = Nothing
            ds = Nothing
        End If
        Return outp
    End Function
    Function makeinc()
        Dim outp As String = ""
        If Request.Form("projname") <> "" Then
            Dim ds As New dbclass
            Dim fm As New formMaker
            Dim coll(5) As String
            Dim spl() As String
            Dim projid As String = ""
            Dim rs As DataTableReader
            Dim copy As String = ""
            Dim i As Integer = 1
            Dim sumt, sump, sumpd As Double
            Dim sumpd1 As Double
            Dim sump1 As String
            Dim pdate As Date
            Dim amtot As String
            Dim nod As Integer
            Dim paiddate As Date
            Dim ref As String = ""
            nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
            Dim pdate1 As Date
            pdate = Request.QueryString("month") & "/" & nod.ToString & "/" & Request.QueryString("year")
            pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
            sumpd1 = 0
            sump1 = 0
            sumt = sump = sumpd = 0
            spl = Request.Form("projname").Split("|")
            If spl.Length <= 1 Then
                ReDim spl(2)
                spl(0) = Request.Form("projname")
                spl(1) = ""
            End If
            outp = ""
            projid = spl(1)

            Dim sql As String

            If Request.QueryString("ref") <> "" Then

                sql = "select emptid,ref,date_paid from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where Payrollx.ref='" & Request.QueryString("ref") & "' and payrollx.pay_mathd='Bank' order by emp_static_info.first_name"
            Else
                sql = "select emptid,ref from payrollx inner join emprec on emprec.id=payrollx.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where emptid in(select emptid from emp_job_assign where project_id='" & projid & "' and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate & "') or date_from between '" & pdate1 & "' and '" & pdate & "')) and date_paid='" & pdate & "' order by emp_static_info.first_name"

            End If
            ' Response.Write(sql)
            rs = ds.dtmake("vwapp", sql, Session("con"))

            If rs.HasRows = True Then

                outp = "<table id='tb1' cellspacing='0' cellpadding='3' border='0'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='7' >" & Session("company_name") & _
                "<br> Project Name:"
                If projid <> "" Then
                    outp &= spl(0).ToString
                Else
                    outp &= "All Projects"
                End If
                outp &= "<br>List of Account Number for month: " & MonthName(Request.QueryString("month")) & " " & Request.QueryString("year") & _
                "<br>Paid From: " & fm.getinfo2("select bank_name from tblbanks where abr='" & Request.QueryString("bname") & "'", Session("con")) & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
                "<td style='width:10px;'>No.</td><td class='fxname'  style='width:200px;'>Employee Name</td><td class='fitx'  style='width:50px;'>Total Net Income</td><td class='fitx'  style='width:100px;'>Account No.</td><td class='fitx'  style='width:80px;'>Bank</td><td class='fitx'  style='width:80px;'>Branch</td></tr>"
                Dim prd As String
                Dim emptid As String
                Dim arr() As String = {""}
                Dim binfo() As String = {"", "", ""}
                While rs.Read
                    coll(0) = ""
                    coll(1) = ""
                    coll(2) = ""
                    coll(3) = ""
                    coll(4) = ""
                    coll(5) = ""
                    emptid = rs.Item("emptid")
                    ref = rs.Item("ref")
                    If fm.searcharray(arr, emptid) = False Then
                        ReDim Preserve arr(UBound(arr) + 1)
                        arr(UBound(arr) - 1) = emptid


                        coll(0) = i.ToString
                        coll(1) = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con")), Session("con"))

                        If ref <> "" Then

                            sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))
                            paiddate = fm.getinfo2("select date_paid from payrollx where emptid=" & emptid & " and ref='" & ref & "'", Session("con"))

                            sumpd = 0
                            ' Response.Write("select total from pardim_sum where emptid=" & emptid & " and paid_date='" & paiddate.ToShortDateString & "'  and mthd='Bank'<br>")
                        Else
                            sump1 = fm.getinfo2("select sum(netp) as exp1 from payrollx where emptid=" & emptid & " and date_paid='" & pdate & "'", Session("con"))

                        End If

                        ' Response.Write(sump1 & "<br>select sum(netpay) as exp1 from paryrol where emptid=" & rs.Item("emptid") & " and pay_date='" & pdate & "'")
                        If IsNumeric(sump1) = False Then
                            Response.Write(sump1.ToString)
                            sump1 = "0"

                        End If


                        coll(2) = fm.numdigit(CDbl(sump1).ToString, 2).ToString
                        amtot = "0"
                        If Request.QueryString("ot") = "on" Then
                            amtot = fm.getinfo2("select sum(netp) from payrollx where emptid=" & emptid & " and pddate='" & pdate & "' and remark='OT-Payment'", Session("con"))
                            'Response.Write("in")
                        End If
                        If IsNumeric(amtot) = True Then
                            coll(2) = CDbl(coll(2)) + CDbl(amtot)

                        End If
                        Dim empidx As String
                        empidx = checkbankacc(emptid)
                        binfo(0) = ""
                        binfo(1) = ""
                        binfo(2) = ""
                        binfo = getbankinf(empidx, pdate)
                        coll(3) = binfo(0) 'fm.getinfo2("select accountno from empbank where emptid=" & empidx & " and active='y'", Session("con"))

                        coll(4) = binfo(1) ' fm.getinfo2("select bankname from empbank where emptid=" & empidx & " and active='y'", Session("con"))

                        coll(5) = binfo(2) 'fm.getinfo2("select branch from empbank where emptid=" & empidx & " and active='y'", Session("con"))
                        If Request.QueryString("acc") = "None" Then

                            If coll(3) = "None" Then
                                i += 1
                                sumt += sumpd1 + sump1
                                sumpd += sumpd1
                                sump += sump1
                                If IsNumeric(amtot) = True Then
                                    sumt += CDbl(amtot)

                                End If
                                outp &= "<tr>"
                                For j As Integer = 0 To coll.Length - 1
                                    If j = 2 Then
                                        outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                    Else
                                        outp &= "<td>" & coll(j) & "</td>"
                                    End If
                                Next
                                outp &= "</tr>"
                            End If
                        ElseIf Request.QueryString("acc") = "acc" Then
                            sumt += sumpd1 + sump1
                            sumpd += sumpd1
                            sump += sump1
                            If IsNumeric(amtot) = True Then
                                sumt += CDbl(amtot)

                            End If
                            If coll(3) <> "None" Then
                                i = i + 1

                                outp &= "<tr>"
                                For j As Integer = 0 To coll.Length - 1
                                    If j = 2 Then

                                        outp &= "<td style='text-align:right;'>" & FormatNumber(coll(j), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                    Else
                                        outp &= "<td>" & coll(j) & "</td>"
                                    End If
                                Next
                                outp &= "</tr>"

                            End If
                        Else
                            sumt += sumpd1 + sump1
                            sumpd += sumpd1
                            sump += sump1
                            If IsNumeric(amtot) = True Then
                                sumt += CDbl(amtot)

                            End If
                            i += 1
                            outp &= "<tr>"
                            For j As Integer = 0 To coll.Length - 1
                                If j = 2 Then
                                    outp &= "<td style='text-align:right;'>" & coll(j) & "</td>"
                                Else
                                    outp &= "<td>" & coll(j) & "</td>"
                                End If
                            Next
                            outp &= "</tr>"

                        End If

                    End If
                End While
                outp &= "<tr><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;font-weight:bold;'>&nbsp;" & fm.numdigit(CDbl(sumt), 2).ToString & "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                outp &= "</table>"
                Response.Write(outp)
            Else
                Response.Write("Sorry, Data is not Existed" & ref)
            End If
            rs.Close()
            fm = Nothing
            ds = Nothing
        End If
        Return outp
    End Function
    Function empids(ByVal empp As String)
        Dim rs As DataTableReader
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim rtn As String = ""
        Try



            rs = ds.dtmake("idcol", "select id from emprec where emp_id='" & fm.getinfo2("select emp_id from emprec where id=" & empp, Session("con")) & "'", Session("con"))
            If rs.HasRows Then
                While rs.Read
                    rtn &= rs.Item(0) & ","
                End While
                If rtn.Length > 0 Then
                    rtn = rtn.Substring(0, rtn.Length - 1)

                End If
            End If
            rs.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
        fm = Nothing
        ds = Nothing
        Return rtn
    End Function

    Function getbankinf(ByVal empid As String, ByVal datepaid As String) As Array
        Dim rs As DataTableReader
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim mail As New mail_system
        Dim frmdate As String
        Dim sql As String = ""
        Dim idclose As String
        Dim emp_id As String = fm.getinfo2("select emp_id from emprec where id=" & empid, Session("con"))

        '  Response.Write(empid & "<br><br>")
        idclose = fm.getjavanum("emprec where emp_id='" & emp_id & "'", "id", Session("con"), ",")
        ' Response.Write("<br><br>xxxxxxxxxxxxxxxxxxx" & idclose & "<br>")
        Dim rtn() As String = {"None", "None", "None"}
        Try
            ' Response.Write(datepaid & "<br>")
            sql = "select * from empbank where active='y' and emptid in(" & idclose & ") and bankname='" & Request.QueryString("bname") & "' order by date_from asc,date_reg asc"
            Response.Write("select * from empbank where active='y' and emptid in(" & idclose & ") and bankname='" & Request.QueryString("bname") & "' order by date_from asc,date_reg asc")
            'Response.Write("<br>" & sql & "<br>")
            rs = dbx.dtmake("dbbank", sql, Session("con"))

            If rs.HasRows Then
                While rs.Read
                    rtn(0) = "None"
                    rtn(1) = "None"
                    rtn(2) = "None"
                   
                    If rs.IsDBNull(8) Then
                        If DateDiff("d", datepaid, rs.Item("date_reg")) <= 0 Then
                            rtn(0) = rs.Item("accountno")
                            rtn(1) = rs.Item("bankname")
                            rtn(2) = rs.Item("branch")
                            'Response.Write(rs.Item("emptid") & "...." & DateDiff("d", datepaid, rs.Item("date_reg")) & "......" & rs.Item("accountno") & "<br>")

                        End If
                    Else
                        If DateDiff("d", datepaid, rs.Item("date_from")) <= 0 Then
                            rtn(0) = rs.Item("accountno")
                            rtn(1) = rs.Item("bankname")
                            rtn(2) = rs.Item("branch")
                            'Response.Write(rs.Item("emptid") & "...." & DateDiff("d", datepaid, rs.Item("date_reg")) & "......" & rs.Item("accountno") & "<br>")

                        End If
                    End If
                End While
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & sql)
            ' mail.sendemail(ex.ToString & sql, Session("epwd"), Session("efrom"), Session("eto"), Session("company_name") & "-Error", Session("smtp"), Session("eport"))
            fm.exception_hand(ex.ToString)
        End Try
        Return rtn
    End Function
End Class
