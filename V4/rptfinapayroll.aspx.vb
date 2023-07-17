Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient

Partial Class rptfinapayroll
    Inherits System.Web.UI.Page

    Protected Sub rptfinapayroll_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Function enddate(ByVal d1 As Date) As Date
        d1 = d1.Month & "/" & Date.DaysInMonth(d1.Year, d1.Month) & "/" & d1.Year
        Return d1
    End Function
    Function body()
        Dim d1, d2 As Date
        Dim proj As String
        Dim sqlx, row As String
        Dim mk As New formMaker

        If Request.Item("month") <> "" Then
            d1 = Request.Item("month") & "/1/" & Request.Item("year")
            proj = Request.Item("projname")
            Dim projid() As String
            projid = proj.Split("|")
            Dim rtnvalue As String = ""
            d2 = enddate(d1)
            Response.Write(d1 & "====" & d2 & "====" & proj)
            rtnvalue = mk.getprojemp(projid(1).ToString, d2, Session("con"))
            sqlx = "select id,date_paid,pddate,b_sal,b_e,talw,alw,ot,gross_earnings,txinco,tax,pen_e,pen_c,netp,ref,emptid,remark from payrollx where emptid in(" & rtnvalue & ") and pddate between '" & d1 & "' and '" & d2 & "' order by pddate,ref,remark"
            row = tableviewsalpaidx("Paid Salary Information", sqlx, "Payroll Date,paid Date,Basic Salary,Earnings,Taxable Allowance,Allowance,OT,Gross Earning,Taxable income,Tax,Pension Contribution,Pension C.co.,Net Pay,Payroll Ref.", Session("con"), "")
            Response.Write(row)
        End If
    End Function
    Public Function tableviewsalpaidx(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim sumgross, sumtaxincome, sumtax, sumpc, sumpcc, netpay, sumot As Double
        Dim arrref() As String = {""}

        sumgross = 0
        sumtaxincome = 0
        sumtax = 0
        sumpc = 0
        sumpcc = 0
        netpay = 0
        sumot = 0
        Dim fm As New formMaker
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        rtstr = "<div id='rptpayroll'><table  cellspacing='0' cellpadding='7' width='800' border=1>" & _
        "<th colspan='14'>" & tbl & "</th>" & _
        "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
        Try
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        Catch ex As Exception
            Return "Error: copy the following error and mail it to the developer<br>" & sql & "<br>System shows:" & ex.ToString
        End Try

        If dt.HasRows = True Then
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & hdr(i) & "</td>"
                    If i = 1 Then
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>Name of Emp</td>"

                    End If
                Next
            Else
                For i = 1 To dt.FieldCount - 3

                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"


                Next
            End If

            rtstr = rtstr & "</tr>"
            Dim color As String = "E3EAEB"
            Dim fname As String
            Dim kx As Integer = 1
            While dt.Read
                fname = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & dt.Item("emptid"), Session("con")), Session("con"))
                If fm.searcharray(arrref, dt.Item("ref")) = False Then


                    ReDim Preserve arrref(kx)

                    arrref(kx - 1) = dt.Item("ref")
                    kx += 1
                End If
                If color <> "#E3EAEB" Then
                    color = "#E3EAEB"
                Else
                    color = "#fefefe"
                End If
                If LCase(dt.Item("remark")) = "monthly" Then
                    color = "blue"
                ElseIf LCase(dt.Item("remark")) = "increament" Then
                    color = "yellow"
                ElseIf LCase(dt.Item("remark")) = "pay_inc_middle" Then
                    color = "gray"
                ElseIf LCase(dt.Item("remark")) = "ot-payment" Then
                    color = "green"
                End If
                rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                ' Dim outp As String
                For k As Integer = 1 To dt.FieldCount - 3
                    If dt.IsDBNull(k) = False Then
                        If dt.Item(k).ToString = "y" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"

                        ElseIf dt.GetName(k) = "date_paid" Then


                            rtstr = rtstr & "<td  style='padding-right:20px;'> " & MonthName(CDate(dt.Item(k)).Month, True) & " " & CDate(dt.Item(k)).Year.ToString & "</td>"

                        Else

                            rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>"
                            If IsNumeric(dt.Item(k)) = True Then
                                rtstr &= FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True)
                            Else
                                rtstr &= dt.Item(k)
                            End If
                            rtstr &= "</td>"

                        End If
                        If k = 2 Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & fname & " </td>"
                        End If
                        If LCase(dt.GetName(k)) = "ot" Then
                            If IsNumeric(dt.Item(k)) Then
                                sumot += CDbl(dt.Item(k))
                            Else
                                sumot += 0
                            End If
                        ElseIf LCase(dt.GetName(k)) = "gross_earnings" Then
                            If IsNumeric(dt.Item(k)) Then
                                sumgross += CDbl(dt.Item(k))
                            Else
                                sumgross += 0
                            End If
                        ElseIf LCase(dt.GetName(k)) = "txinco" Then
                            If IsNumeric(dt.Item(k)) Then
                                sumtaxincome += CDbl(dt.Item(k))
                            Else
                                sumtaxincome += 0
                            End If
                        ElseIf LCase(dt.GetName(k)) = "tax" Then
                            If IsNumeric(dt.Item(k)) Then
                                sumtax += CDbl(dt.Item(k))
                            Else
                                sumtax += 0
                            End If
                        ElseIf LCase(dt.GetName(k)) = "pen_e" Then
                            If IsNumeric(dt.Item(k)) Then
                                sumpc += CDbl(dt.Item(k))
                            Else
                                sumpc += 0
                            End If
                        ElseIf LCase(dt.GetName(k)) = "pen_c" Then
                            If IsNumeric(dt.Item(k)) Then
                                sumpcc += CDbl(dt.Item(k))
                            Else
                                sumpcc += 0
                            End If
                        ElseIf LCase(dt.GetName(k)) = "netp" Then
                            If IsNumeric(dt.Item(k)) Then
                                netpay += CDbl(dt.Item(k))
                            Else
                                netpay += 0
                            End If
                            'ot, gross_earnings, txinco, tax, pen_e, pen_c, netp
                        End If
                        ' rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>" & outp & "</td>"
                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>-</td>"
                    End If
                Next


                rtstr = rtstr & "</tr>"
            End While
        Else
            rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
        End If
        If heading <> "" Then
            For i = 0 To hdr.Length - 1
                Select Case i
                    Case 6
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & FormatNumber(sumot, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    Case 7
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & FormatNumber(sumgross, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    Case 8
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & FormatNumber(sumtaxincome, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    Case 9
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & FormatNumber(sumtax, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    Case 10
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & FormatNumber(sumpc, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    Case 11
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & FormatNumber(sumpcc, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    Case 12
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & FormatNumber(netpay, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    Case Else
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>-</td>"
                End Select
                'rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>" & hdr(i) & "</td>"
                If i = 1 Then
                    rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'>-</td>"

                End If
            Next
        Else
            For i = 1 To dt.FieldCount - 2

                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"


            Next
        End If

        Dim sum() As Double = {sumot, sumgross, sumtaxincome, sumtax, sumpc, sumpcc, netpay}
        rtstr = rtstr & "</table>"
        dt.Close()
        dc = Nothing
        dt = Nothing
        dataaudit(arrref, sum)
        Return rtstr

    End Function
    Function dataaudit(ByVal ref() As String, ByVal sum() As Double)
        Dim sumh(6) As Double
        Dim name() As String = {"OT", "Gross", "Taxable Income", "Tax", "Pension Employee Cont.", "Pension Company Cont.", "Net pay"}
        Dim fm As New formMaker
        For i As Integer = 0 To 5
            sumh(i) = 0
        Next

        If ref.Length > 0 Then
            For i As Integer = 0 To ref.Length - 1
                If String.IsNullOrEmpty(ref(i)) Then
                    '  Response.Write("<br>Nulll")
                Else
                    Response.Write("<br>" & ref(i))
                    getnamelist(ref(i))
                    If IsNumeric(fm.getinfo2("select sum(ot) from payrollx where ref='" & ref(i) & "'", Session("con"))) Then
                        sumh(0) += CDbl(fm.getinfo2("select sum(ot) from payrollx where ref='" & ref(i) & "'", Session("con")))
                    End If
                    If IsNumeric(fm.getinfo2("select sum(gross_earnings) from payrollx where ref='" & ref(i) & "'", Session("con"))) Then
                        sumh(1) += CDbl(fm.getinfo2("select sum(gross_earnings) from payrollx where ref='" & ref(i) & "'", Session("con")))

                    End If
                    If IsNumeric(fm.getinfo2("select sum(txinco) from payrollx where ref='" & ref(i) & "'", Session("con"))) Then
                        sumh(2) += CDbl(fm.getinfo2("select sum(txinco) from payrollx where ref='" & ref(i) & "'", Session("con")))
                    End If
                    If IsNumeric(fm.getinfo2("select sum(tax) from payrollx where ref='" & ref(i) & "'", Session("con"))) Then
                        sumh(3) += CDbl(fm.getinfo2("select sum(tax) from payrollx where ref='" & ref(i) & "'", Session("con")))
                    End If
                    If IsNumeric(fm.getinfo2("select sum(pen_e) from payrollx where ref='" & ref(i) & "'", Session("con"))) Then
                        sumh(4) += CDbl(fm.getinfo2("select sum(pen_e) from payrollx where ref='" & ref(i) & "'", Session("con")))
                    End If
                    If IsNumeric(fm.getinfo2("select sum(pen_c) from payrollx where ref='" & ref(i) & "'", Session("con"))) Then
                        sumh(5) += CDbl(fm.getinfo2("select sum(pen_c) from payrollx where ref='" & ref(i) & "'", Session("con")))
                    End If
                    If IsNumeric(fm.getinfo2("select sum(netp) from payrollx where ref='" & ref(i) & "'", Session("con"))) Then
                        sumh(6) += CDbl(fm.getinfo2("select sum(netp) from payrollx where ref='" & ref(i) & "'", Session("con")))
                    End If


                    'ot, gross_earnings, txinco, tax, pen_e, pen_c, netp

                    End If
                    'Response.Write("<br>" & ref(i))
            Next
            Dim dblr As Double = 0
            For j As Integer = 0 To sumh.Length - 1
                If IsNumeric(sumh(j)) Then
                    dblr = 0
                    dblr = sumh(j) - sum(j)
                End If
                Response.Write("<br>" & dblr.ToString & name(j) & ":" & CDbl(sumh(j)).ToString & "-" & CDbl(sum(j)).ToString & "=" & (CDbl(sumh(j)) - CDbl(sum(j))).ToString)
            Next
            'Response.Write("ot sum:" & sumh(0))
        End If

    End Function
    Function getnamelist(ByVal ref As String)
        Dim dt As New dbclass
        Dim rs As DataTableReader
        rs = dt.dtmake("namelist", "select emptid from payrollx where ref='" & ref & "'", Session("con"))
        Dim fname As String = ""
        Dim fm As New formMaker
        Response.Write("<br>..............................." & ref & ".....................................")
        Dim i As Integer = 1
        If rs.HasRows Then
            While rs.Read()
                fname &= "<br>" & i & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con")), Session("con"))
                i += 1
            End While

        End If
        Response.Write(fname)
    End Function
End Class
