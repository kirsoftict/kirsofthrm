Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class payroll2
    Inherits System.Web.UI.Page
    Function proces()
        Dim refx As String = ""
        refx = Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & Now.Millisecond.ToString
        Dim spl() As String
        Dim spl2() As String
        Dim con As String = ""
        Dim tr As String = ""
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim pid As String
        Dim sql As String
        Dim flg1 As String
        Dim sq() As String
        Dim sqq() As String
        Dim arr() As String = {"9", "10", "12", "13", "14", "15", ""}
        Dim sq2() As String
        Dim sqlloan() As String
        Dim sqlot() As String

        ' Response.Write(refx & "<br>")
        Dim arr2() As String = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "12", "13", "14", "15", ""}
        If Request.QueryString("task") = "" Then

            pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
            If pid.ToString = "None" Then
                sql = "insert into payrol_reg values('" & Request.QueryString("month") & "','" & Request.QueryString("year") & "')"
                flg1 = ds.save(sql, session("con"), session("path"))
                If flg1.ToString = "1" Then
                    pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
                Else
                    pid = "unknown"
                End If

            End If
            Dim i As Integer = -1
            Dim j As Integer = 0
            Dim outf As String
            If pid <> "unknown" Then
                For Each k As String In Request.QueryString
                    ReDim spl(1)
                    ReDim spl(2)
                    spl = k.Split("_")
                    ' Response.Write("<br>" & con & "===" & spl(0).Trim & "====" & spl.Length)

                    If spl.Length > 1 Then
                        spl2 = spl(1).Split("-")
                        If con <> spl(0).Trim Then

                            If i <> -1 Then
                                sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                                sq2(i) &= ",'" & Request.Form("paydx") & "','" & Request.Form("paymth") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                            End If
                            i = i + 1
                            ReDim Preserve sq(i + 1)
                            ReDim Preserve sq2(i + 1)
                            sq(i) = "insert into paryrol(payroll_id,emptid,gross_earn,tax,pension,pension_co,tdedact,netpay,pay_date,who_reg,date_reg) values(" & pid & ",'"
                            sq2(i) = "insert into paryrolx(ref,pr,emptid,b_sal,no_day,b_e,talw,alw,ot,gross_earnings,txinco,tax,pen_e,pen_c,tded,netp,date_paid,who_reg,date_reg) values(" & refx & "," & pid & ",'"

                            sq(i) &= spl(0).ToString & "'"
                            sq2(i) &= spl(0).ToString & "'"
                            con = spl(0)

                        End If


                        If fm.searcharray(arr, spl(1)) Then
                            outf = Request.QueryString(k).Trim
                            outf = outf.Replace(",", "")
                            sq(i) &= "," & outf
                            '  Response.Write(spl(1) & "===" & outf & "<=<br>")
                        End If
                        If fm.searcharray(arr2, spl(1)) Then
                            outf = Request.QueryString(k).Trim
                            outf = outf.Replace(",", "")
                            sq2(i) &= "," & outf
                            ' Response.Write(spl(1) & "===" & outf & "<br>")
                        End If
                        ' Response.Write("<br>" & spl(0) & "///" & spl(1))
                        If spl2(0).ToString = "11" Then
                            outf = Request.QueryString(k).Trim
                            outf = outf.Replace(",", "")
                            If outf.ToString <> "0.00" And spl2.Length >= 2 Then
                                If j = -1 Then
                                    ReDim sqq(1)
                                    ReDim sqlloan(1)
                                    j = 0
                                Else
                                    ReDim Preserve sqq(j + 1)
                                    ReDim Preserve sqlloan(j + 1)
                                End If
                                sqq(j) = "insert into emp_loan_settlement(loan_no,ref,date_payment,amount,who_reg,date_reg) values(" & _
                                 spl2(2) & "," & pid & ",'" & Request.Form("paydx") & "'," & outf & ",'" & Session("username") & "','" & Today.ToShortDateString & "')"
                                ' Response.Write("<br>" & spl2(0))
                                sqlloan(j) = "insert into emp_loan_settlement(loan_no,ref,date_payment,amount,who_reg,date_reg) values(" & _
                               spl2(2) & "," & pid & ",'" & Request.Form("paydx") & "'," & outf & ",'" & Session("username") & "','" & Today.ToShortDateString & "')"

                                j = j + 1
                            End If
                        End If
                    End If

                    ' Response.Write(k & "==>" & Request.QueryString(k) & "<br>")

                Next
                flg1 = "0"
                sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                'ds.save("begin transaction", Session("con"),session("path"))
                For k As Integer = 0 To i
                    'Response.Write("<br>" & sq(k))
                    flg1 = ds.save(sq(k), Session("con"), Session("path"))
                    If flg1 <> "1" Then
                        Response.Write("<br> Error: " & flg1)
                    End If
                Next

                For k As Integer = 0 To sqq.Length - 1
                    If String.IsNullOrEmpty(sqq(k)) = False Then
                        ' Response.Write("<br> E" & sqq(k))
                        flg1 = ds.save(sqq(k), Session("con"), Session("path"))
                    End If
                    If flg1 <> "1" Then
                        Response.Write("<br>Error: " & flg1)
                    End If
                Next

                flg1 = ds.save("commit", Session("con"), Session("path"))
                If flg1.ToString <> "-1" Then
                    Response.Write(flg1)
                    '   ds.save("rollback", Session("con"))
                    Response.Write("rollback")
                Else
                    Response.Write("Data(s) is/are saved")
                End If
            End If
            ' Response.Write(pid.ToString)
            Response.Write("<table>")
            Response.Write(Request.Form("header"))


            For Each k As String In Request.QueryString
                ReDim spl(1)
                spl = k.Split("_")
                ' Response.Write("<br>" & con & "===" & spl(0).Trim & "====" & spl.Length)
                If spl.Length > 1 Then
                    If con <> spl(0).Trim Then
                        If tr = "" Then
                            tr = "<tr>"
                        ElseIf tr = "<tr>" Then
                            tr = "</tr><tr>"

                        Else
                            tr = "</tr><tr>"
                        End If
                        con = spl(0).Trim
                        Response.Write(tr)
                        Response.Write("<td>" & spl(0).ToString & "</td>")
                    End If


                    If fm.searcharray(arr, spl(1)) Then
                        Response.Write("<td>" & Request.QueryString(k) & "</td>")
                    End If
                End If
                ' 
            Next

            Response.Write("</tr></table>")
        ElseIf Request.QueryString("task") = "two" Then
            Dim arrs() As String = {"4", "5", "9", ""}
            ' Response.Write("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"))
            pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
            ' Response.Write(pid)
            Dim i As Integer = -1
            Dim j As Integer = -1
            Dim outf As String
            con = ""
            Dim ref() As String
            Dim con2 As String = ""

            Dim a As Integer = 0
            For Each k As String In Request.QueryString
                spl = k.Split("_")


                If spl.Length > 1 Then
                    spl2 = spl(1).Split("-")

                    If con2 <> spl(0) Then
                        ' Response.Write(con2 & "==" & spl(0) & "<br>")
                        con2 = spl(0)
                        'Response.Write("<br>")
                        a = a + 1
                        ReDim Preserve ref(a)

                    Else

                        If spl2.Length > 1 Then
                            ' Response.Write(con2 & "==" & spl(0) & "<br>")
                            If spl2(0) = "4" Then
                                ref(a - 1) &= spl2(1) & ","
                                'Response.Write(ref(a - 1))
                            End If
                        End If
                    End If




                End If
            Next

            For Each k As String In Request.QueryString

                spl = k.Split("_")
                ' Response.Write("<br>" & con & "===" & spl(0).Trim & "====" & spl.Length)

                If spl.Length > 1 Then
                    spl2 = spl(1).Split("-")
                    If spl2.Length > 1 Then
                        For p As Integer = 0 To spl2.Length - 1

                            '    Response.Write(spl2(1) & ",<br>")
                        Next
                    End If
                    If con <> spl(0).Trim Then


                        If i <> -1 Then
                            sq(i) &= ",'" & Request.Form("payd") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                        End If
                        i = i + 1


                        ReDim Preserve sq(i + 1)
                        sq(i) = "insert into paryrol(payroll_id,ref_inc,emptid,gross_earn,tax,pay_date,who_reg,date_reg) values(" & pid & ",'"
                        sq(i) &= ref(i) & "','" & spl(0).ToString & "'"
                        con = spl(0)

                        '  Response.Write(k & "==>" & Request.QueryString(k) & "<br>")
                    End If



                    If fm.searcharray(arrs, spl(1)) = True Then


                        outf = Request.QueryString(k).Trim
                        outf = outf.Replace(",", "")
                        'Response.Write(spl(1) & "sp<br>")
                        sq(i) &= "," & outf


                    End If
                End If
            Next
            If i <> -1 Then
                sq(i) &= ",'" & Request.Form("payd") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
            End If
            i = sq.Length - 1
            For a = 0 To ref.Length - 2



                spl = ref(a).Split(",")
                If spl.Length > 1 Then
                    For j = 0 To spl.Length - 1
                        ReDim Preserve sq(i + 1)

                        If String.IsNullOrEmpty(spl(j)) = False Then
                            If spl(j) <> "0" Then

                                sq(i) = "update emp_inc set paidref='" & pid & "' where id=" & spl(j)
                                i += 1
                            End If
                        End If
                    Next

                End If
                'Response.Write(ref(a) & "<br>")
            Next
            ds.save("begin transaction", Session("con"), Session("path"))
            For j = 0 To sq.Length - 2

                If String.IsNullOrEmpty(sq(j)) = False Then
                    flg1 = ds.save(sq(j), Session("con"), Session("path"))
                    If flg1.ToString <> "1" Then
                        'Response.Write(flg1 & "<br>")
                        ds.save("rollback", Session("con"), Session("path"))
                        Response.Write("data is not saved")
                        Exit For
                    End If
                End If


            Next

            flg1 = ds.save("commit", Session("con"), Session("path"))
            ' Response.Write(flg1)
            If flg1 = "-1" Or flg1 = "1" Then
                Response.Write("Data is Saved")
            End If

        End If
        ds = Nothing
        fm = Nothing

    End Function
    Function proces2()
        Dim refx As String = ""
        refx = Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & Now.Millisecond.ToString
        Dim spl() As String
        Dim spl2() As String
        Dim con As String = ""
        Dim tr As String = ""
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim pid As String
        Dim sql As String
        Dim flg1 As String
        Dim sq() As String
        Dim sqq() As String
        Dim arr() As String = {"9", "10", "12", "13", "14", "15", ""}
        Dim sq2() As String
        Dim updatesql(1) As String
        Dim uc As Integer = 0
        ' Response.Write(refx & "<br>")
        Dim arr2() As String = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "12", "13", "14", "15", ""}
        If Request.QueryString("task") = "" Then
            Dim mm As String
            Dim yy As String
            Dim ddt As String
            Dim mthtd As String
            Dim sqlloan() As String
            Dim sqlot() As String
            mthtd = Request.QueryString("paymthd")
            mm = Request.QueryString("month")
            yy = Request.QueryString("year")
            ddt = Date.DaysInMonth(CInt(yy), CInt(mm)).ToString
            Dim datepaid As Date
            datepaid = mm & "/" & ddt & "/" & yy
            pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
            If pid.ToString = "None" Then
                sql = "insert into payrol_reg values('" & Request.QueryString("month") & "','" & Request.QueryString("year") & "')"
                flg1 = ds.save(sql, session("con"), session("path"))
                If flg1.ToString = "1" Then
                    pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
                Else
                    pid = "unknown"
                End If

            End If
            Dim i As Integer = -1
            Dim j As Integer = -1
            Dim outf As String = ""
            If pid <> "unknown" Then
                Dim nextp() As String
                Dim val() As String
                nextp = Request.Form("nextpage").Split("&")
                For k As Integer = 0 To UBound(nextp) - 1
                    ' ReDim spl(1)
                    ReDim spl(2)
                    val = nextp(k).Split("=")
                    spl = val(0).Split("_")
                    ' Response.Write("<br>" & con & "===" & spl(0).Trim & "====" & spl.Length)

                    If spl.Length > 1 Then
                        spl2 = spl(1).Split("-")
                        If con <> spl(0).Trim Then

                            If i <> -1 Then
                                sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                                sq2(i) &= "','" & Request.Form("paydx") & "','" & Request.Form("paymth") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                            End If
                            i = i + 1
                            ReDim Preserve sq(i + 1)
                            ReDim Preserve sq2(i + 1)
                            sq(i) = "insert into paryrol(payroll_id,emptid,gross_earn,tax,pension,pension_co,tdedact,netpay,pay_date,who_reg,date_reg) values(" & pid & ",'"

                            sq2(i) = "insert into payrollx(ref,pr,emptid,b_sal,no_day,b_e,talw,alw,ot,gross_earnings,txinco,tax,pen_e,pen_c,tded,netp,date_paid,pay_mathd,who_reg,date_reg) values('" & refx & "'," & pid & ",'"

                            sq(i) &= spl(0).ToString & "'"
                            sq2(i) &= spl(0).ToString & ""
                            con = spl(0)
                            'Response.Write(sq(i) & "<br>")
                        End If


                        If fm.searcharray(arr, spl(1)) Then
                            outf = val(1).Trim
                            outf = outf.Replace(",", "")
                            sq(i) &= "," & outf

                            ' Response.Write(spl(1) & "===" & outf & "<=<br>")
                        End If
                        If fm.searcharray(arr2, spl(1)) Then
                            outf = val(1).Trim
                            outf = outf.Replace(",", "")
                            sq2(i) &= "','" & outf
                            ' Response.Write(spl(1) & "===" & outf & "<br>")
                            If CInt(spl(1)) = 7 And CDbl(outf) > 0 Then
                                ReDim Preserve updatesql(uc + 1)
                                updatesql(uc) = "update emp_ot set paidstatus='y' where emptid=" & spl(0) & " and ot_date='" & datepaid & "'"
                                uc = uc + 1
                            End If
                        End If
                        ' Response.Write("<br>" & spl(0) & "///" & spl(1))
                        If spl2(0).ToString = "11" Then
                            outf = val(1).Trim
                            outf = outf.Replace(",", "")
                            If outf.ToString <> "0.00" And spl2.Length >= 2 Then
                                If j = -1 Then
                                    ReDim sqq(1)
                                    ReDim sqlloan(1)
                                    j = 0
                                Else
                                    ReDim Preserve sqq(j + 1)
                                    ReDim Preserve sqlloan(j + 1)
                                End If

                                sqq(j) = "insert into emp_loan_settlement(loan_no,ref,date_payment,amount,who_reg,date_reg) values(" & _
                                 spl2(2) & "," & pid & ",'" & Request.Form("paydx") & "'," & outf & ",'" & Session("username") & "','" & Today.ToShortDateString & "')"
                                ' Response.Write("<br>" & spl2(0))

                                j = j + 1
                            End If
                        End If
                    End If

                    ' Response.Write(k & "==>" & Request.QueryString(k) & "<br>")
                    '  Response.Write(sq(i) & "<br>")
                Next
                flg1 = "0"
                sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                sq2(i) &= "','" & Request.Form("paydx") & "','" & Request.Form("paymth") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"

                Dim wrt As String = ""
                Dim wrt2 As String = ""
                wrt = "BEGIN TRANSACTION" & Chr(13)
                wrt2 = "BEGIN TRANSACTION" & Chr(13)
                'ds.save("begin", Session("con"))
                For k As Integer = 0 To i
                    'Response.Write("<br>" & sq(k))
                    ' flg1 = ds.save(sq(k), Session("con"))
                    If sq(k) <> "" Then
                        wrt &= sq(k) & Chr(13)
                    End If
                    If sq2(k) <> "" Then
                        wrt2 &= sq2(k) & Chr(13)
                    End If
                    If flg1 <> "1" Then
                        ' Response.Write("<br> Error: " & flg1 & sq(k))
                    End If
                Next
                For k As Integer = 0 To j
                    If String.IsNullOrEmpty(sqq(k)) = False Then
                        ' Response.Write("<br> E" & sqq(k))
                        ' flg1 = ds.save(sqq(k), Session("con"))
                        wrt &= sqq(k) & Chr(13)
                        wrt2 &= sqq(k) & Chr(13)

                    End If

                Next
                For k As Integer = 0 To uc
                    If String.IsNullOrEmpty(updatesql(k)) = False Then
                        wrt &= updatesql(k) & Chr(13)
                        wrt2 &= updatesql(k) & Chr(13)
                    End If
                Next
                wrt &= "COMMIT" & Chr(13)
                wrt2 &= "COMMIT" & Chr(13)

                flg1 = ds.excutes(wrt, Session("con"), Session("path"))
                'Response.Write(flg1)
                'flg1 = ds.save("commit", Session("con"))
                If IsNumeric(flg1) = True Then
                    ' Response.Write(flg1)
                    If flg1 <= 0 Then
                        ds.excutes("rollback", Session("con"), Session("path"))
                        Response.Write("data is not saved")

                    Else

                        Response.Write("Data Saved")
                    End If

                Else
                    If flg1.ToString <> "-1" Then
                        ds.excutes("rollback", Session("con"), Session("path"))
                        Response.Write("data is not saved")
                    Else
                        Response.Write("Data(s) is/are saved")
                    End If

                End If
                ' ds.excutes(wrt2, Session("con"))
                'Response.Write("<textarea cols='100' rows='30'>" & wrt & "</textarea>")
                ' Response.Write("<textarea cols='100' rows='30'>" & wrt2 & "</textarea>")

            End If

            ' Response.Write(pid.ToString)
            Response.Write("<table>")
            Response.Write(Request.Form("header"))


            For Each k As String In Request.QueryString
                ReDim spl(1)
                spl = k.Split("_")
                ' Response.Write("<br>" & con & "===" & spl(0).Trim & "====" & spl.Length)
                If spl.Length > 1 Then
                    If con <> spl(0).Trim Then
                        If tr = "" Then
                            tr = "<tr>"
                        ElseIf tr = "<tr>" Then
                            tr = "</tr><tr>"

                        Else
                            tr = "</tr><tr>"
                        End If
                        con = spl(0).Trim
                        Response.Write(tr)
                        Response.Write("<td>" & spl(0).ToString & "</td>")
                    End If


                    If fm.searcharray(arr, spl(1)) Then
                        Response.Write("<td>" & Request.QueryString(k) & "</td>")
                    End If
                End If
                ' 
            Next

            Response.Write("</tr></table>")
        ElseIf Request.QueryString("task") = "two" Then
            Dim arrs() As String = {"4", "5", "9", "10", ""}
            Response.Write("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"))
            pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
            ' Response.Write(pid)
            Dim i As Integer = -1
            Dim j As Integer = -1
            Dim outf As String
            con = ""
            Dim ref() As String
            Dim con2 As String = ""

            Dim a As Integer = 0
            For Each k As String In Request.QueryString
                spl = k.Split("_")


                If spl.Length > 1 Then
                    spl2 = spl(1).Split("-")

                    If con2 <> spl(0) Then
                        ' Response.Write(con2 & "==" & spl(0) & "<br>")
                        con2 = spl(0)
                        'Response.Write("<br>")
                        a = a + 1
                        ReDim Preserve ref(a)

                    Else

                        If spl2.Length > 1 Then
                            ' Response.Write(con2 & "==" & spl(0) & "<br>")
                            If spl2(0) = "4" Then
                                ref(a - 1) &= spl2(1) & ","
                                'Response.Write(ref(a - 1))
                            End If
                        End If
                    End If




                End If
            Next

            For Each k As String In Request.QueryString

                spl = k.Split("_")
                ' Response.Write("<br>" & con & "===" & spl(0).Trim & "====" & spl.Length)

                If spl.Length > 1 Then
                    spl2 = spl(1).Split("-")
                    If spl2.Length > 1 Then
                        For p As Integer = 0 To spl2.Length - 1

                            '    Response.Write(spl2(1) & ",<br>")
                        Next
                    End If
                    If con <> spl(0).Trim Then


                        If i <> -1 Then
                            sq(i) &= ",'" & Request.Form("payd") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                        End If
                        i = i + 1


                        ReDim Preserve sq(i + 1)
                        sq(i) = "insert into paryrol(payroll_id,ref_inc,emptid,gross_earn,tax,netpay,pay_date,who_reg,date_reg) values(" & pid & ",'"
                        sq(i) &= ref(i) & "','" & spl(0).ToString & "'"
                        con = spl(0)

                        '  Response.Write(k & "==>" & Request.QueryString(k) & "<br>")
                    End If



                    If fm.searcharray(arrs, spl(1)) = True Then


                        outf = Request.QueryString(k).Trim
                        outf = outf.Replace(",", "")
                        'Response.Write(spl(1) & "sp<br>")
                        sq(i) &= "," & outf


                    End If
                End If
            Next
            If i <> -1 Then
                sq(i) &= ",'" & Request.Form("payd") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
            End If
            i = sq.Length - 1
            For a = 0 To ref.Length - 2



                spl = ref(a).Split(",")
                If spl.Length > 1 Then
                    For j = 0 To spl.Length - 1
                        ReDim Preserve sq(i + 1)

                        If String.IsNullOrEmpty(spl(j)) = False Then
                            If spl(j) <> "0" Then

                                sq(i) = "update emp_inc set paidref='" & pid & "' where id=" & spl(j)
                                i += 1
                            End If
                        End If
                    Next

                End If
                'Response.Write(ref(a) & "<br>")
            Next
            ds.save("begin transaction", Session("con"), session("path"))
            For j = 0 To sq.Length - 2

                If String.IsNullOrEmpty(sq(j)) = False Then
                    flg1 = ds.save(sq(j), Session("con"), Session("path"))
                    'Response.Write(sq(j))
                    If flg1.ToString <> "1" Then
                        'Response.Write(flg1 & "<br>")
                        ds.save("rollback", Session("con"), Session("path"))
                        Response.Write("data is not saved")
                        Exit For
                    End If
                End If


            Next

            flg1 = ds.save("commit", Session("con"), Session("path"))
            '  Response.Write(flg1)
            If flg1 = "-1" Or flg1 = "1" Then
                Response.Write("Data is Saved")
            End If

        End If
        ds = Nothing
        fm = Nothing

    End Function
    Function processnew()
        Dim refx As String = ""
        ' refx = Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & Now.Millisecond.ToString
        Dim spl() As String
        Dim spl2() As String
        Dim con As String = ""
        Dim tr As String = ""
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim pid As String
        Dim sql As String
        Dim flg1 As String
        Dim sq() As String
        Dim sqq() As String
        Dim arr() As String = {"9", "10", "12", "13", "14", "15", ""}
        Dim sq2() As String
        Dim updatesql(1) As String
        Dim uc As Integer = 0
        ' Response.Write(refx & "<br>")
        Dim arr2() As String = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "12", "13", "14", "15", ""}
        If Request.QueryString("task") = "" Then
            Dim mm As String
            Dim yy As String
            Dim ddt As String
            Dim mthtd As String
            Dim sqlloan() As String
            Dim sqlot() As String
            mthtd = Request.QueryString("paymthd")
            mm = Request.QueryString("month")
            yy = Request.QueryString("year")
            ddt = Date.DaysInMonth(CInt(yy), CInt(mm)).ToString
            Dim datepaid As Date
            datepaid = mm & "/" & ddt & "/" & yy
            pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
            If pid.ToString = "None" Then
                sql = "insert into payrol_reg values('" & Request.QueryString("month") & "','" & Request.QueryString("year") & "')"
                flg1 = ds.save(sql, Session("con"), Session("path"))
                If flg1.ToString = "1" Then
                    pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
                Else
                    pid = "unknown"
                End If

            End If
            Dim i As Integer = -1
            Dim j As Integer = -1
            Dim outf As String = ""
            Dim ref As String = ""
            Dim remark As String = ""
            If pid <> "unknown" Then
                Dim nextp() As String
                Dim val() As String

                Dim getid As String
                ' Response.Write(Now.ToString("yyMMddHHmmss") & "<br>")
                ' Response.Write(Now.Ticks & "<br>")


                     ' Response.Write(Now.TimeOfDay.Seconds)
                ref = Request.Form("ref")
733:
                getid = pid & "-" & Now.ToString("yyMMddHHmmss")
                If ref = "" Then
                    ref = "NT-pl-" & getid & "-" & Session("who") 'Now.Ticks ' sec.StrToHex3(CStr(Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString))

                End If

                Dim refcheck As String = fm.getinfo2("select id from payrollx where ref='" & ref & "'", Session("con"))
                If refcheck <> "None" Then
                    ref = ""
                    GoTo 733
                End If
                If Request.QueryString("remark") <> "" Then
                    remark = Request.QueryString("remark")

                Else
                    remark = "monthly"
                End If
                nextp = Request.Form("nextpage").Split("&")
                For k As Integer = 0 To UBound(nextp) - 1
                    ' ReDim spl(1)
                    ReDim spl(2)
                    val = nextp(k).Split("=")
                    spl = val(0).Split("_")
                    ' Response.Write("<br>" & val(0) & "===" & spl(0).Trim & "====" & spl(1))

                    If spl.Length > 1 Then

                        spl2 = spl(1).Split("-")
                        If con <> spl(0).Trim Then

                            If i <> -1 Then
                                ' sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                                sq2(i) &= "','" & Request.Form("paydx") & "','" & Request.Form("paymth") & "','" & Request.Form("bankname") & "','" & Session("username") & "','" & Today.ToShortDateString & "','" & Request.Form("ppdate") & "','" & remark & "')"
                            End If
                            i = i + 1
                            ' ReDim Preserve sq(i + 1)
                            ReDim Preserve sq2(i + 1)
                            '   sq(i) = "insert into paryrol(payroll_id,emptid,gross_earn,tax,pension,pension_co,tdedact,netpay,pay_date,who_reg,date_reg) values(" & pid & ",'"

                            sq2(i) = "insert into payrollx(ref,pr,emptid,b_sal,no_day,b_e,talw,alw,ot,gross_earnings,txinco,tax,pen_e,pen_c,tded,netp,date_paid,pay_mathd,bank,who_reg,date_reg,pddate,remark) values('" & ref & "'," & pid & ",'"

                            'sq(i) &= spl(0).ToString & "'"
                            sq2(i) &= spl(0).ToString & ""
                            con = spl(0)
                            'Response.Write(sq(i) & "<br>")
                        End If



                        If fm.searcharray(arr2, spl(1)) Then
                            outf = val(1).Trim
                            outf = outf.Replace(",", "")
                            sq2(i) &= "','" & outf
                            ' Response.Write(spl(1) & "===" & outf & "<br>")
                            If CInt(spl(1)) = 7 And CDbl(outf) > 0 Then
                                ReDim Preserve updatesql(uc + 1)
                                updatesql(uc) = "update emp_ot set paidstatus='y',ref='" & ref & "',date_reg='" & Now.ToString & "' where emptid=" & spl(0) & " and datepart(month,datepaid)='" & CDate(Request.Form("paydx")).Month & "' and datepart(year,datepaid)='" & CDate(Request.Form("paydx")).Year & "'"
                                uc = uc + 1
                            End If
                        End If
                        ' Response.Write("<br>" & spl(0) & "///" & spl(1))
                        If spl2(0).ToString = "2" Then
                            ' Response.Write(outf & "<br>")
                        End If
                        If spl2(0).ToString = "11" Then
                            outf = val(1).Trim
                            outf = outf.Replace(",", "")
                            If outf.ToString <> "0.00" And spl2.Length >= 2 Then
                                If j = -1 Then
                                    ReDim sqq(1)

                                    j = 0
                                Else
                                    ReDim Preserve sqq(j + 1)

                                End If

                                sqq(j) = "insert into emp_loan_settlement(loan_no,ref,date_payment,amount,who_reg,date_reg) values(" & _
                                 spl2(2) & ",'" & ref & "','" & Request.Form("paydx") & "'," & outf & ",'" & Session("username") & "','" & Now.ToString & "')"
                                ' Response.Write("<br>" & spl2(0))

                                j = j + 1
                            End If
                        End If
                    End If

                    ' Response.Write(k & "==>" & Request.QueryString(k) & "<br>")
                    '  Response.Write(sq(i) & "<br>")
                Next
                flg1 = "0"
                '  sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                sq2(i) &= "','" & Request.Form("paydx") & "','" & Request.Form("paymth") & "','" & Request.Form("bankname") & "','" & Session("username") & "','" & Now.ToString & "','" & Request.Form("ppdate") & "','" & remark & "')"

                Dim wrt As String = ""
                Dim wrt2 As String = ""

                ' wrt = "BEGIN TRANSACTION" & Chr(13)
                wrt2 = "BEGIN TRANSACTION " & Session("username") & Chr(13)
                'ds.save("begin", Session("con"))
                For k As Integer = 0 To i
                    'Response.Write("<br>" & sq(k))
                    ' flg1 = ds.save(sq(k), Session("con"))

                    If sq2(k) <> "" Then
                        wrt2 &= sq2(k) & Chr(13)
                    End If

                Next
                For k As Integer = 0 To j
                    If String.IsNullOrEmpty(sqq(k)) = False Then
                        wrt2 &= sqq(k) & Chr(13)

                    End If

                Next
                For k As Integer = 0 To uc
                    If String.IsNullOrEmpty(updatesql(k)) = False Then

                        wrt2 &= updatesql(k) & Chr(13)

                    End If
                Next

                ' wrt2 &= "COMMIT" & Chr(13)
                Try
                        flg1 = ds.excutes(wrt2, Session("con"), Session("path"))
                    ' Response.Write("<br>Flag..............<br>" & flg1 & "<br>............<br>")
                Catch exx As Exception
                    Response.Write(exx.ToString & "<br>" & wrt2 & "<br>")
                    Dim lm As New leavemgt
                    lm.writeerro(exx.ToString, Session("path"))
                End Try

                '  Response.Write(wrt2)
                ' flg1 = "-1"
                Response.Write(flg1)
                'flg1 = ds.save("commit", Session("con"))
                If IsNumeric(flg1) = True Then
                    'Response.Write(flg1)
                    If flg1 <= 0 Then
                        ds.excutes("RollBack TRANSACTION " & Session("username"), Session("con"), Session("path"))
                        Response.Write("data is not saved")
                        File.WriteAllText("c:\temp\payroll\notsavedw2" & Now.ToString("yyMMddHHmmss") & ".sql", wrt2)

                    Else
                        File.WriteAllText("c:\temp\payroll\w2" & Now.ToString("yyMMddHHmmss") & ".sql", wrt2)

                        ds.excutes("Commit TRANSACTION " & Session("username"), Session("con"), Session("path"))
                        Response.Write("Data Saved")
                    End If

                Else
                    If flg1.ToString <> "-1" Then
                        '  ds.save("rollback", Session("con"))
                        Response.Write("data is not saved")
                    Else
                        Response.Write("Data(s) is/are saved")
                    End If

                End If
                'ds.excutes(wrt2, Session("con"))

                'Response.Write("<textarea cols='100' rows='30'>" & wrt2.ToString & "</textarea>")

            End If
            ' Response.Write(Session("fired").ToString)

            ' Response.Write(pid.ToString)
        ElseIf Request.QueryString("task") = "two" Then
            gototasktwo() 'increament
        ElseIf Request.QueryString("task") = "three" Then
            Dim mm As String
            Dim ref_inc As String = ""
            Dim yy As String
            Dim ddt As String
            Dim mthtd As String
            Dim sqlloan() As String
            Dim sqlot() As String
            Dim arrx() As String = {"4", "9", "10", ""}
            mthtd = Request.QueryString("paymthd")
            mm = Request.QueryString("month")
            yy = Request.QueryString("year")
            ddt = Date.DaysInMonth(CInt(yy), CInt(mm)).ToString
            Dim datepaid, ppdate As Date
            datepaid = Request.Form("PPdate")
            ppdate = Request.Form("paydx")
            Dim d1, d2 As Date
            d1 = mm & "/1/" & yy
            d2 = mm & "/" & ddt & "/" & yy
            pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
            If pid.ToString = "None" Then
                sql = "insert into payrol_reg values('" & Request.QueryString("month") & "','" & Request.QueryString("year") & "')"
                flg1 = ds.excutes(sql, Session("con"), Session("path"))
                If flg1.ToString = "1" Then
                    pid = fm.getinfo2("select id from payrol_reg where month='" & Request.QueryString("month") & "' and year=" & Request.QueryString("year"), Session("con"))
                Else
                    pid = "unknown"
                End If

            End If
            Dim i As Integer = -1
            Dim j As Integer = -1
            Dim outf As String = ""
            Dim ref As String = ""
            If pid <> "unknown" Then
                Dim nextp() As String
                Dim val() As String

                Dim getid As String
                ' Response.Write(Now.ToString("yyMMddHHmmss") & "<br>")
                ' Response.Write(Now.Ticks & "<br>")

                ' Response.Write(Now.TimeOfDay.Seconds)
                ref = Request.Form("ref")
930:
                getid = pid & "-" & Now.ToString("yyMMddHHmmss")
                If ref = "" Then
                    ref = "OT-" & getid & Session("who") 'Now.Ticks ' sec.StrToHex3(CStr(Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString))

                End If
                Dim refcheck As String = fm.getinfo2("select id from payrollx where ref='" & ref & "'", Session("con"))
                If refcheck <> "None" Then
                    ref = ""
                    GoTo 930

                End If      ' Response.Write(Now.TimeOfDay.Seconds)
                nextp = Request.Form("nextpage").Split("&")

                For k As Integer = 0 To UBound(nextp) - 1
                    ' ReDim spl(1) 

                    ReDim spl(2)
                    val = nextp(k).Split("=")
                    spl = val(0).Split("_")
                    'Response.Write("<br>" & val(0) & "===" & spl(0).Trim & "xxxx" & spl(1) & "****" & val(1))

                    If spl.Length > 1 Then
                        spl2 = spl(1).Split("-")
                        If con <> spl(0).Trim Then

                            If i <> -1 Then
                                ' sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                                sq2(i) &= "','" & Request.Form("paydx") & "','" & Request.Form("ppdate") & "','" & Request.Form("paymth") & "','" & Request.Form("bankname") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                                ref_inc = ""
                            End If
                            i = i + 1
                            ' ReDim Preserve sq(i + 1)
                            ReDim Preserve sq2(i + 1)
                            '   sq(i) = "insert into paryrol(payroll_id,emptid,gross_earn,tax,pension,pension_co,tdedact,netpay,pay_date,who_reg,date_reg) values(" & pid & ",'"

                            sq2(i) = "insert into payrollx(ref,pr,emptid,ot,tax,netp,date_paid,pddate,pay_mathd,bank,who_reg,date_reg) values('" & ref & "'," & pid & ",'"

                            'sq(i) &= spl(0).ToString & "'"
                            sq2(i) &= spl(0).ToString & ""
                            con = spl(0)
                            'Response.Write(sq(i) & "<br>")
                        End If



                        If fm.searcharray(arrx, spl2(0)) Then
                            If CInt(spl2(0)) <> 4 Then
                                outf = val(1).Trim
                                outf = outf.Replace(",", "")
                                sq2(i) &= "','" & outf
                            End If
                            ' Response.Write(spl(1) & "===" & outf & "<br>")
                            If CInt(spl2(0)) = 4 Then
                                ReDim Preserve updatesql(uc + 1)
                                If fm.searcharray(arr2, spl(1)) Then
                                    outf = val(1).Trim
                                    outf = outf.Replace(",", "")
                                    sq2(i) &= "','" & outf
                                    '  Response.Write(spl(1) & "===" & outf & "<br>")
                                    If CInt(spl(1)) = 4 And CDbl(outf) > 0 Then
                                        ReDim Preserve updatesql(uc + 1)
                                        updatesql(uc) = "update emp_ot set paidstatus='y',ref='" & ref & "',datepaid='" & Request.Form("ppdate") & "' where emptid=" & spl(0) & " and ot_date between '" & d1 & "' and '" & d2 & "' and paidstatus='n'"
                                        uc = uc + 1
                                    End If
                                End If
                            End If
                        End If
                        ' Response.Write("<br>" & spl(0) & "///" & spl(1))

                    End If

                    ' Response.Write(k & "==>" & Request.QueryString(k) & "<br>")
                    '  Response.Write(sq(i) & "<br>")
                Next
                flg1 = "0"
                '  sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                '  sq2(i) &= "','" & Request.Form("paydx") & "','" & Request.Form("ppdate") & "','" & Request.Form("paymth") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                sq2(i) &= "','" & Request.Form("paydx") & "','" & Request.Form("ppdate") & "','" & Request.Form("paymth") & "','" & Request.Form("bankname") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"

                Dim wrt As String = ""
                Dim wrt2 As String = ""
                ' wrt = "BEGIN TRANSACTION" & Chr(13)
                wrt2 = "BEGIN  TRANSACTION " & Session("username") & Chr(13)
                'ds.save("begin", Session("con"))
                For k As Integer = 0 To i
                    'Response.Write("<br>" & sq(k))
                    ' flg1 = ds.save(sq(k), Session("con"))

                    If sq2(k) <> "" Then
                        wrt2 &= sq2(k) & Chr(13)
                    End If

                Next
                For k As Integer = 0 To j
                    If String.IsNullOrEmpty(sqq(k)) = False Then

                        wrt2 &= sqq(k) & Chr(13)

                    End If

                Next
                For k As Integer = 0 To uc
                    If String.IsNullOrEmpty(updatesql(k)) = False Then

                        wrt2 &= updatesql(k) & Chr(13)
                    End If
                Next

                ' wrt2 &= "COMMIT" & Chr(13)
                flg1 = ds.excutes(wrt2, Session("con"), Session("path"))

                If IsNumeric(flg1) = True Then
                    'Response.Write(flg1)
                    If flg1 <= 0 Then
                        ds.excutes("RollBack TRANSACTION " & Session("username"), Session("con"), Session("path"))
                        Response.Write("data is not saved")
                        File.WriteAllText("c:\temp\payroll\notsavedw2" & Now.ToString("yyMMddHHmmss") & ".sql", wrt2)
                    Else

                        sql = "update payrollx set remark='OT-Payment' where ref='" & ref & "'"
                        ds.excutes(sql, Session("con"), Session("path"))

                        ds.excutes("Commit TRANSACTION " & Session("username"), Session("con"), Session("path"))
                        Response.Write(flg1.ToString & " Data(s) Saved")
                        File.WriteAllText("c:\temp\payroll\w2" & Now.ToString("yyMMddHHmmss") & ".sql", wrt2)
                    End If

                Else

                    ds.excutes("RollBack TRANSACTION " & Session("username"), Session("con"), Session("path"))
                    Response.Write("data is not saved")

                End If


                ' ds.excutes(wrt2, Session("con"), Session("path"))

                ' Response.Write("<textarea cols='100' rows='30'>" & wrt2.ToString & "</textarea>")

            End If


        End If
        ds = Nothing
        fm = Nothing
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Function gototasktwo()
        Dim mm As String
        Dim ref_inc As String = ""
        Dim yy As String
        Dim ddt As String
        Dim mthtd As String
        Dim sqlloan() As String
        Dim sqlot() As String
        Dim fm As New formMaker
        Dim uc As Integer
        Dim pid As String
        Dim sql As String
        Dim flg1, flg As String
        Dim ds As New dbclass
        Dim sec As New k_security
        Dim arrx() As String = {"4", "5", "9", "10", ""}
        mthtd = Request.QueryString("paymthd")
        mm = Request.QueryString("month")
        yy = Request.QueryString("year")
        ddt = Date.DaysInMonth(CInt(yy), CInt(mm)).ToString
        Dim datepaid As Date
        datepaid = mm & "/" & ddt & "/" & yy
        Dim pdate1 As Date
        pdate1 = datepaid.AddMonths(-1)
        datepaid = Request.Form("ppdate")


        pid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month.ToString & "' and year=" & pdate1.Year.ToString, Session("con"))
        If pid.ToString = "None" Then
            sql = "insert into payrol_reg values('" & pdate1.Month.ToString & "','" & pdate1.Year.ToString & "')"
            flg1 = ds.save(sql, session("con"), session("path"))
            If flg1.ToString = "1" Then
                pid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month.ToString & "' and year=" & pdate1.Year.ToString, Session("con"))
            Else
                pid = "unknown"
            End If

        End If
        Dim i As Integer = -1
        Dim j As Integer = -1
        Dim outf As String = ""
        Dim ref As String = ""
        Dim con As String
        Dim updatesql() As String
        Dim spl(), spl2(), sq2(), sqq() As String
        If pid <> "unknown" Then
            Dim nextp() As String
            Dim val() As String

            Dim getid As String
            ' Response.Write(Now.ToString("yyMMddHHmmss") & "<br>")
            ' Response.Write(Now.Ticks & "<br>")

            ' Response.Write(Now.TimeOfDay.Seconds)
            ref = Request.Form("ref")
1121:
            getid = pid & "-" & Now.ToString("yyMMddHHmmss")
            If ref = "" Then
                ref = "NT-incr-" & getid & Session("who") 'Now.Ticks ' sec.StrToHex3(CStr(Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString))

            End If

            Dim refcheck As String = fm.getinfo2("select id from payrollx where ref='" & ref & "'", Session("con"))
            If refcheck <> "None" Then
                ref = ""
                GoTo 1121
            End If      ' Response.Write(Now.TimeOfDay.Seconds)
            '  ref = "NT-incr-" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString

            nextp = Request.Form("nextpage").Split("&")
            Dim txall As Double = 0
            For k As Integer = 0 To UBound(nextp) - 1

                ReDim spl(2)
                val = nextp(k).Split("=")
                spl = val(0).Split("_")
                If spl.Length > 1 Then
                    spl2 = spl(1).Split("-")
                    If con <> spl(0).Trim Then

                        If i <> -1 Then
                            ' sq(i) &= ",'" & Request.Form("paydx") & "','" & Session("username") & "','" & Today.ToShortDateString & "')"
                            sq2(i) &= "','" & ref_inc & "','" & Request.Form("paydx") & "','" & Request.Form("paymth") & "','" & Request.Form("bankname") & "','Increament','" & Session("username") & "','" & Today.ToShortDateString & "','" & Request.Form("ppdate") & "')"
                            ref_inc = ""
                        End If
                        i = i + 1
                        ' ReDim Preserve sq(i + 1)
                        ReDim Preserve sq2(i + 1)
                        '   sq(i) = "insert into paryrol(payroll_id,emptid,gross_earn,tax,pension,pension_co,tdedact,netpay,pay_date,who_reg,date_reg) values(" & pid & ",'"

                        sq2(i) = "insert into payrollx(ref,pr,emptid,alw,txinco,tax,netp,ref_inc,date_paid,pay_mathd,bank,remark,who_reg,date_reg,pddate) values('" & ref & "'," & pid & ",'"

                        'sq(i) &= spl(0).ToString & "'"
                        sq2(i) &= spl(0).ToString & ""
                        con = spl(0)

                    End If



                    If fm.searcharray(arrx, spl2(0)) Then
                        If CInt(spl2(0)) <> 4 And CInt(spl2(0)) <> 5 Then
                            outf = val(1).Trim
                            outf = outf.Replace(",", "")
                            sq2(i) &= "','" & outf
                        End If
                        ' Response.Write(spl(1) & "===" & outf & "<br>")
                        If CInt(spl2(0)) = 4 Then
                            ReDim Preserve updatesql(uc + 1)
                            If spl2(1).ToString <> "inc" Then
                                If CInt(spl2(1)) > 0 Then
                                    updatesql(uc) = "update emp_inc set paidref='" & ref & "',paid_date='" & Request.Form("ppdate") & "',date_reg='" & Now.ToShortDateString & "' where emptid=" & spl(0) & " and id=" & spl2(1)
                                    'Response.Write(updatesql(uc) & "<br>")
                                    If ref_inc <> "" Then
                                        ref_inc &= "," & spl2(1)
                                    Else
                                        ref_inc &= spl2(1)

                                    End If

                                    uc = uc + 1

                                End If
                            ElseIf spl2(1).ToString = "inc" Then
                                outf = val(1).Trim
                                outf = outf.Replace(",", "")
                                sq2(i) &= "','" & outf
                                txall = CDbl(outf)

                            End If
                        ElseIf CInt(spl2(0)) = 5 Then
                            outf = val(1).Trim
                            outf = outf.Replace(",", "")
                            sq2(i) &= "','" & (CDbl(outf) - txall).ToString

                        End If
                    End If

                End If

            Next
            flg1 = "0"
            sq2(i) &= "','" & ref_inc & "','" & Request.Form("paydx") & "','" & Request.Form("paymth") & "','" & Request.Form("bankname") & "','Increament','" & Session("username") & "','" & Today.ToShortDateString & "','" & Request.Form("ppdate") & "')"

            Dim wrt As String = ""
            Dim wrt2 As String = ""
            refcheck = fm.getinfo2("select id from payrollx where ref='" & ref & "'", Session("con"))
            If refcheck = "None" Then

                wrt2 = "BEGIN TRANSACTION " & Session("username") & Chr(13)

                For k As Integer = 0 To i

                    If sq2(k) <> "" Then
                        wrt2 &= sq2(k) & Chr(13)
                        Response.Write(sq2(k) & "<br>")
                    End If

                Next
                For k As Integer = 0 To j
                    If String.IsNullOrEmpty(sqq(k)) = False Then
                        wrt2 &= sqq(k) & Chr(13)

                    End If

                Next
                For k As Integer = 0 To uc
                    If String.IsNullOrEmpty(updatesql(k)) = False Then

                        wrt2 &= updatesql(k) & Chr(13)
                        Response.Write("<br> " & updatesql(k) & "<br>")
                    End If
                Next
                flg1 = ds.excutes(wrt2, Session("con"), Session("path"))
                'flg1 = 0
                ' Response.Write(wrt2 & "<br>")
                If IsNumeric(flg1) = True Then

                    If flg1 <= 0 Then
                        ds.excutes("RollBack TRANSACTION " & Session("username"), Session("con"), Session("path"))
                        Response.Write("data is not saved yy")
                        File.WriteAllText("c:\temp\payroll\notsavedw2" & Now.ToString("yyMMddHHmmss") & ".sql", wrt2)
                    Else

                        sql = "update payrollx set remark='Increament',pddate='" & Request.Form("ppdate") & "' where ref='" & ref & "'"
                        ds.excutes(sql, Session("con"), Session("path"))

                        ds.excutes("Commit TRANSACTION " & Session("username"), Session("con"), Session("path"))
                        Response.Write("Data Saved")
                        File.WriteAllText("c:\temp\payroll\w2" & Now.ToString("yyMMddHHmmss") & ".sql", wrt2)
                    End If

                Else
                    If Session("con").state = ConnectionState.Closed Then
                        Response.Write("<br>DB - Connection is Closed!!!!!!<br>")
                    End If
                    ds.excutes("RollBack TRANSACTION " & Session("username"), Session("con"), Session("path"))
                    Response.Write("data is not saved xx" & flg1)

                End If

            Else
                Response.Write("referal Error")

            End If
        End If
    End Function
    Function getkey(ByVal num As Integer)
        Dim keyx As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 0 To num
            Dim idx As String = r.Next(0, keyx.Length)
            sb.Append(keyx.Substring(idx, 1))

        Next
        Return sb.ToString
    End Function

End Class
