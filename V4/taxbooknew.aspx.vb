
Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Imports System.Drawing

Partial Class taxbooknew

    Inherits System.Web.UI.Page
    Dim exe
    Function createpage(ByVal ref As String, ByVal tax As String, ByVal state As String) As String
        Dim rtn As String = ""
        Dim loc As String = Server.MapPath("download")
        loc = loc.Replace("\", "/")
        Select Case tax

            Case "tax"
                Select Case state
                    Case "view"

                        rtn = makepage_all_tax(ref, Request.Item("pd1"), Request.Item("pd2"))

                        Response.Write("single<br>" & rtn)
                    Case "viewall"

                        rtn = makepage_sec_tax(ref, Request.Item("pd1"), Request.Item("pd2"))
                        loc &= "/" & Now.Ticks.ToString & ".txt"
                        '  Response.Write(loc)
                        File.WriteAllText(loc, rtn.Replace("<br/>", Chr(13)))
                        rtn = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                        "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                       " display:block;' onclick=" & Chr(34) & "javascript:exportxx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc & "','export','2;3');" & Chr(34) & " >" & _
                       "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div> "
                        rtn &= "<div id='pinta4'  style='display:none;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
           " onclick=" & Chr(34) & "javascirpt:printtax('bigprint','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printA4');'>" & _
      "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>    <div id='bigprint' style='font-size:8pt;'>" & rtn & "</div>"
                        Response.Write(rtn)
                    Case "viewtotal"
                        '  Response.Write("viewtototal")
                        ' rtn = makepage_tot_tax_excel(ref, Request.Item("pd1"), Request.Item("pd2"))
                        ' rtn = makepage_sec_tax(ref, Request.Item("pd1"), Request.Item("pd2"))
                        'loc &= "/" & Now.Ticks.ToString & ".txt"
                        '  Response.Write(loc)
                        ' File.WriteAllText(loc, rtn)
                        rtn = makepage_tot_tax(ref, Request.Item("pd1"), Request.Item("pd2"))
                        Dim loc2 As String = loc
                        loc2 &= "/" & Now.Ticks & ".htm"

                        loc &= "/" & Now.Ticks.ToString & ".txt"
                        '  Response.Write(loc)
                        File.WriteAllText(loc, rtn)
                        File.WriteAllText(loc2, "<link href='../css/taxold.css' media='print' rel='Stylesheet' type='text/css' />" & rtn)
                        rtn = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                         "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                        " display:block;' onclick=" & Chr(34) & "javascript:exportxx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc & "','export','2;3');" & Chr(34) & " >" & _
                        "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                        "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                  " onclick=" & Chr(34) & "javascirpt:printtax('bigprint','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printA4');" & Chr(34) & ">" & _
                             "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                            " <div style='float:left;'><input type='checkbox' id='signc' name='signc' value='ok' onclick=" & Chr(34) & "javascript:checkedc('signc');" & Chr(34) & "/> Clear signature Part</div>" & _
"<div style='float:left;'><input type='checkbox' id='tinp' name='tinp' value='ok' onclick=" & Chr(34) & "javascript:checkedc('tinp');" & Chr(34) & "/> Clear Tin</div><div style='clear:left;'></div><br>" & _
"<div id='bigprint'>" & rtn & "</div>"
                        Response.Write(rtn)
                End Select

        End Select
    End Function
    Function makepage_tot_tax(ByVal ref As String, ByVal pd1 As Date, ByVal pd2 As Date)
        'Response.Write(ref)
        pay_jv(pd1, pd2)
        Dim outp(2) As String
        ' Dim pc, pe As String
        Dim dtm As New dtime
        Dim proj As String = ""
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        ' Dim pstamt, psumtax, tpstamt, tpsumtax As Double
        ' Dim pd1, pd2 As String
        ' pd1 = month & "/1/" & year
        'pd2 = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        '  Dim sumpen As Double
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim i As Integer = 1
        Dim j As Integer
        Dim mycolor As New Color
        Dim rndx As New Random
        Dim projlist() As String = {""}
        ' Response.Write(ref)
        Dim increament As String
        Dim fixed As Integer
        Dim txs As Double = 0
        'Dim trow As Integer
        Dim ot_back As String
        Dim taxsum, tincome, talw As String
        Dim basic As String
        Dim pno, tpg As Integer
        Dim sql As String = ""
        Dim color As String = "white"
        Dim bgpay As String = "white"
        Dim cref As String = ""
        Dim pgof As String = ""
        Dim netpay As String = ""
        Dim b_e As String
        If File.Exists("c:\temp\consttax.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        ' count = 0
        Dim pageno As Integer = 1
        ' Dim refx As String
        Dim tempf As Integer
        Dim stax, stinc As String
        Dim otbk As String
        Dim incmonth As String
        stax = fm.getinfo2("select sum(tax) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
        If IsNumeric(stax) = False Then
            Response.Write(stax & " stax<br>")
            stax = 0
        End If
        stinc = fm.getinfo2("select sum(txinco) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))
        If IsNumeric(stinc) = False Then
            Response.Write(stinc & " stax<br>")
            stinc = 0
        End If
        otbk = fm.getinfo2("select sum(ot) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and (b_sal is null)", Session("con"))
        If IsNumeric(otbk) = False Then
            Response.Write(otbk & " stax<br>")
            otbk = 0
        End If
        incmonth = fm.getinfo2("select sum(txinco) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and remark='Increament'", Session("con"))
        If IsNumeric(incmonth) = False Then
            Response.Write(incmonth & " increment error<br>")
            incmonth = 0
        End If
        Response.Write("<table><tr><td><b>Taxable Income:</b></td><td style='text-align:right;'>" & FormatNumber(stinc, 2, TriState.True, TriState.True, TriState.True) & "</td></tr>")
        Response.Write("<tr><td><b>OT Back pay:</b></td><td style='text-align:right;'>" & FormatNumber(otbk, 2, TriState.True, TriState.True, TriState.True) & "</td></tr>")
        Response.Write("<tr><td><b>Increament pay:</b></td><td style='text-align:right;'>" & FormatNumber(incmonth, 2, TriState.True, TriState.True, TriState.True) & "</td></tr>")
        Response.Write("<tr><td><b>Total Taxable income:</b></td><td  style='text-align:right;'>" & FormatNumber((CDbl(stinc) + CDbl(otbk) + CDbl(incmonth)).ToString, 2, TriState.True, TriState.True, TriState.True) & "</td></tr>")
        Response.Write("<tr><td><b>Tax collected:</b></td><td  style='text-align:right;'>" & FormatNumber(stax, 2, TriState.True, TriState.True, TriState.True) & "</td></tr></table><br>")
        Dim lemp As String = ""
        lemp = (fm.getjavalist2("payrollx  where  pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", "emptid", Session("con"), "").Replace(Chr(34), ""))
        lemp = lemp.Replace("xx", "0")
        'Response.Write(lemp)
        Dim t As Integer
        Dim rlist As String = ""
        ' ref = ref.Substring(0, ref.Length - 1)
        sql = "select count(id) from payrollx where emptid in(" & lemp & ") and  pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')"
        Dim title As String = ""

        pgof = fm.getinfo2(sql, Session("con"))
        Dim pginc As Integer
        sql = "select count(id) from payrollx where emptid not in(" & lemp & ") and  pddate between '" & pd1 & "' and '" & pd2 & "' and remark not in('monthly','pay_inc_middle')"

        pginc = fm.getinfo2(sql, Session("con"))
        '  Response.Write(pginc & "<br>" & pgof & "<br>")
        If IsNumeric(pginc) Then
            pgof += pginc + 1
            '    Response.Write(pgof & "...<br>")
        End If
        Dim nod, nods As Integer
        Try


            'Response.Write(fixed)
            nod = Date.DaysInMonth(CDate(pd1).Year, CDate(pd2).Month)

            Dim pgsize(4) As Integer
            Dim pk As Integer = 1
            Dim fp, lsp, pgs, tpgx As Integer
            Dim dblx As Double
            fp = 13
            pgs = 17
            lsp = 7
            dblx = (pgof - 10) / 20
            'dblx = (pgof / 20) - pgs
            tpgx = 0
            Dim pca As Integer
            pca = pgof
            '  Response.Write(pgof & "......" & dblx & "<br>")
            Dim pggno() As Integer = {0}
            Dim ccc As Integer = 0
            If pgof > 17 Then
                While pca > 0
                    ccc = ccc + 1
                    If pca > 13 Then
                        If pggno.Length = 1 Then
                            ReDim Preserve pggno(ccc)
                            pggno(ccc - 1) = 13
                            pca -= 13
                        ElseIf pca > 20 Then
                            pca -= 20
                            ReDim Preserve pggno(ccc)
                            pggno(ccc - 1) = 20
                        Else
                            pca -= 13
                            ReDim Preserve pggno(ccc)
                            pggno(ccc - 1) = 13
                        End If
                    Else
                        ReDim Preserve pggno(ccc)
                        pggno(ccc - 1) = pca
                        pca = 0
                    End If

                End While
            Else
                ccc = ccc + 1
                If pca > 10 Then

                    ReDim pggno(2)
                    pggno(0) = 10
                    pggno(1) = (pca - 10)
                    pca = 0


                Else
                    ReDim Preserve pggno(ccc)
                    pggno(ccc - 1) = pca
                    pca = 0
                End If

            End If
            pgof = pggno.Length
            ' For ccc = 0 To pggno.Length - 1
            '  Response.Write("page " & ccc & ":" & pggno(ccc) & "<br>")
            ' Next
            ' trow = fm.getinfo2("select count(id) from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))
            ' tpg = CInt(trow) / fixed
            rs = dbs.dtmake("vwpayroll", "select * from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle') order by ref,pddate,emptid,id", Session("con"))
            '  Response.Write("select distnict emptid from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')")
            ccc = 0
            Dim pdm As String = ""
            Dim refx(), refx2() As String
            Dim ttitle() As String = {""}
            Dim alwtb As Double = 0
            If rs.HasRows Then

                pno = 1
                ' outp(0) = headertax(month.ToString & "/1/" & year.ToString, pno.ToString, tpg.ToString)
                '  outp(1) &= "<div class='page'>"
                cref = ""
                proj = ""
                Dim inx As Integer = 0
                Dim emptid() As String = {""}
                While rs.Read
                    title = ""
                    color = ""

                    alwtb = 0
                    Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                    ' Response.Write(rs.Item("emptid") & "<br>")
                    inx = inx + 1
                    If i = 1 Then
                        outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headertax(pd1, pno.ToString, pgof.ToString)(0) & headertax(pd1, pno.ToString, pgof.ToString)(1)
                    Else
                        If inx > pggno(ccc) And ccc < pgof Then
                            inx = 1

                            ccc = ccc + 1

                            outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            For t = 0 To 9
                                coltotal(t) = colsum(t) + coltra(t)
                            Next
                            outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr></table></div></div>"
                            pageno += 1
                            outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headertax(pd1, pno.ToString, tpg.ToString)(1)
                            For t = 0 To colsum.Length - 1
                                coltra(t) = coltotal(t)
                                colsum(t) = 0
                            Next

                        End If
                    End If
                    If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                        ' Response.Write("<br>" & rs.Item("emptid") & "====" & fm.isResign(rs.Item("emptid"), Session("con"))(1))
                        If CDate(pd2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                            color = "red"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                            ' Response.Write(title & "<br>")
                            rlist &= rs.Item("emptid") & ","
                        Else
                            'color = "green"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                        End If

                    End If
                    pdm = fm.getinfo2("select count(id) from payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'", Session("con"))

                    If IsNumeric(pdm) Then
                        If CInt(pdm) > 1 Then
                            color = "#ffac43"
                            title = getjavalist2("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "' and ref not in('" & rs.Item("ref") & "')", "ref,txinco,tax,id", Session("con"), "=>", "|")
                            ' Response.Write("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'<br>")
                            ' Response.Write(rs.Item("emptid") & 
                            title = title.Replace(Chr(34), "")
                            refx = title.Split("|")
                            If refx.Length > 1 Then
                                For k As Integer = 0 To refx.Length - 1
                                    refx2 = refx(k).Split("=>")
                                    '  Response.Write(refx2(0) & "xx<br>")
                                    session_off(refx2(0) & "_" & rs.Item("emptid"))
                                Next
                            Else
                                refx2 = refx(0).Split("=>")
                                ' Response.Write(refx2(0) & "<br>")
                                session_off(refx2(0) & "_" & rs.Item("emptid"))
                            End If

                        End If
                    End If
                    session_off(rs.Item("ref") & "_" & rs.Item("emptid"))
                    If fm.searcharray(emptid, rs.Item("emptid")) = False Then
                        emptid(UBound(emptid)) = rs.Item("emptid")
                        ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                        ReDim Preserve emptid(UBound(emptid) + 1)

                        outp(1) &= " <tr style='background:" & color & ";' title='" & title & "'>"

                        ' outp(1) &= " <td class='nox'>" & i.ToString & "</td>" & Chr(13)
                        If CDate(rs.Item("pddate")).Subtract(CDate(rs.Item("date_paid"))).Days <> 0 Then
                            outp(1) &= " <td class='nox' style='background:#0000ff;width:2%;'>" & i.ToString & "</td>"
                        Else

                            outp(1) &= " <td class='nox' style='width:2%;'>" & i.ToString & ")</td>"
                        End If
                        cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))
                        If cell(1) = "None" Then
                            cell(1) = "&nbsp;"
                        End If
                        outp(1) &= "<td class='tin' style='width:5%'>&nbsp;" & cell(1) & "</td>"
                        If empid = "None" Then
                            cell(2) = "&nbsp;"
                        Else
                            cell(2) = fm.getfullname(empid, Session("con"))
                        End If
                        If (IsNumeric(rs.Item("alw"))) = False Then
                            alwtb = 0
                        Else
                            alwtb = rs.Item("alw")
                        End If
                        outp(1) &= " <td class='ename'  style='width:14%'> " & cell(2) & "</td>"
                        outp(1) &= "<td class='edate'  style='width:4%'>" & dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid"), Session("con"))).ToShortDateString) & "</td>"
                        outp(1) &= " <td class='cssamt'  style='width:4%'>" & FormatNumber(rs.Item("b_e"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                        outp(1) &= " <td class='cssamt'  style='width:4%'>" & FormatNumber(alwtb, 2, TriState.True, TriState.True, TriState.True) & " </td>"
                        talw = fm.getinfo2("select sum(talw) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))

                        If IsNumeric(talw) = True Then
                            ' talw = CDbl(rs.Item("talw")).ToString
                        Else
                            talw = "0"
                        End If


                        outp(1) &= "<td class='cssamt'  style='width:4%'> &nbsp;" & FormatNumber(talw, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                        ot_back = fm.getinfo2("select sum(ot) from payrollx where remark='OT-Payment' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))

                        If IsNumeric(ot_back) Then
                            If IsNumeric(rs.Item("ot")) Then
                                ot_back = CDbl(ot_back) + CDbl(rs.Item("ot"))

                            End If
                        Else
                            If IsNumeric(rs.Item("ot")) Then
                                ot_back = CDbl(rs.Item("ot"))
                            Else
                                ot_back = 0
                            End If
                        End If
                        outp(1) &= "<td class='cssamt'  style='width:4%'> " & FormatNumber(ot_back, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                        increament = fm.getinfo2("select sum(txinco) from payrollx where remark='Increament' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                        If IsNumeric(increament) = False Then
                            increament = "0"
                        End If

                        outp(1) &= "<td class='cssamt'  style='width:4%'> " & FormatNumber(increament, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                        basic = fm.getinfo2("select sum(b_e) from payrollx where remark='monthly' and pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))

                        If IsNumeric(basic) Then
                            tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw) + CDbl(basic)).ToString
                        Else
                            basic = 0
                            tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw)).ToString
                        End If
                        colsum(8) += tincome

                        outp(1) &= "<td class='cssamt'  style='width:4%'>" & FormatNumber(tincome, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                        taxsum = fm.getinfo2("select sum(tax) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                        If IsNumeric(taxsum) Then
                            txs = txs + CDbl(taxsum)
                            '  Response.Write("tax==>" & txs.ToString & "<br>")
                        Else
                            taxsum = "0"
                        End If
                        colsum(9) += CDbl(taxsum)
                        outp(1) &= "<td class='cssamt'  style='width:4%'>" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                        outp(1) &= "<td class='cssamt'  style='width:4%'>&nbsp;</td>" & Chr(13)
                        netpay = fm.getinfo2("select sum(netp) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                        If IsNumeric(netpay) = False Then
                            ' Response.Write(taxsum)
                            netpay = "0"
                        End If

                        outp(1) &= "<td class='cssamt'  style='width:4%'>" & FormatNumber(netpay, 2, TriState.True, TriState.True, TriState.True) & "</td>"

                        If cref <> rs.Item("ref") Then

                            proj = fm.getproj_on_date(rs.Item("emptid"), pd2, Session("con"))(1)

                            If proj.Length > 12 Then
                                proj = proj.Substring(0, 12)

                            Else
                                proj = proj
                            End If
                            Session(proj & "ref") &= rs.Item("ref") & "|"

                            If fm.searcharray(projlist, proj) = False Then


                                projlist(UBound(projlist)) = proj
                                ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                ReDim Preserve projlist(UBound(projlist) + 1)



                            Else
                                '      Response.Write("dup=>" & proj)
                            End If
                            If cref = "" Then
                                Session(proj & "inc") = 0
                                Session(proj & "otback") = 0
                                Session(proj & "basic") = 0
                                Session(proj & "talw") = 0
                                Session(proj & "tax") = 0
                            End If
                            cref = rs.Item("ref")

                            bgpay = "#" & Hex(RGB(rndx.Next(0, 255), rndx.Next(0, 255), rndx.Next(0, 255)))
                        End If
                        refx = title.Split("|")
                        If refx.Length > 1 Then
                            For k As Integer = 0 To refx.Length - 1
                                refx2 = refx(k).Split("=>")
                                If refx2.Length > 0 Then
                                    '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                    If refx2(0) <> "" Then
                                        If fm.searcharray(ttitle, refx2(0)) = False Then
                                            ttitle(UBound(ttitle)) = refx2(0)
                                            ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                            ReDim Preserve ttitle(UBound(ttitle) + 1)
                                            '     Response.Write(refx2(0) & "><><>IIIIII<br>")
                                            If IsDate(refx2(0)) = False Then
                                                Session(proj & "ref") &= refx2(0) & "|"
                                            End If

                                        End If
                                    End If
                                Else
                                    ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                                End If
                            Next
                        Else
                            refx2 = refx(0).Split("=>")
                            If refx2.Length > 0 Then
                                '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                If refx2(0) <> "" Then
                                    If fm.searcharray(ttitle, refx2(0)) = False Then
                                        ttitle(UBound(ttitle)) = refx2(0)
                                        ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                        ReDim Preserve ttitle(UBound(ttitle) + 1)
                                        '     Response.Write(refx2(0) & "><><>IIIIII<br>")
                                        If IsDate(refx2(0)) = False Then
                                            Session(proj & "ref") &= refx2(0) & "|"
                                        End If


                                    End If

                                End If
                            Else
                                ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                            End If
                        End If

                        Session(proj & "inc") = CDbl(Session(proj & "inc")) + CDbl(increament)
                        Session(proj & "otback") = CDbl(Session(proj & "otback")) + CDbl(ot_back)
                        Session(proj & "basic") = CDbl(Session(proj & "basic")) + CDbl(basic)
                        Session(proj & "talw") = CDbl(Session(proj & "talw")) + CDbl(talw)
                        Session(proj & "tax") = CDbl(Session(proj & "tax")) + CDbl(taxsum)
                        outp(1) &= "<td class='csssign' style='font-size:8px;background-color:" & bgpay & "' title='(" & rs.Item("emptid") & ")" & Chr(12) & rs.Item("ref") & "'>"

                        outp(1) &= proj & "</td>" & Chr(13)


                        outp(1) &= "</tr>"
                        i = i + 1
                    Else ' Name duplication...............

                        If cref <> rs.Item("ref") Then

                            proj = fm.getproj_on_date(rs.Item("emptid"), pd2, Session("con"))(1)
                            If proj.Length > 12 Then
                                proj = proj.Substring(0, 12)
                                If proj = "Aleta-Wendo" Then
                                    Response.Write(rs.Item("emptid") & "<br>")
                                End If
                            Else
                                proj = proj
                            End If
                            Session(proj & "ref") &= rs.Item("ref") & "|"

                            If fm.searcharray(projlist, proj) = False Then


                                projlist(UBound(projlist)) = proj
                                ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                ReDim Preserve projlist(UBound(projlist) + 1)



                            Else
                                '      Response.Write("dup=>" & proj)
                            End If
                            If cref = "" Then
                                Session(proj & "inc") = 0
                                Session(proj & "otback") = 0
                                Session(proj & "basic") = 0
                                Session(proj & "talw") = 0
                                Session(proj & "tax") = 0
                            End If
                            cref = rs.Item("ref")

                            bgpay = "#" & Hex(RGB(rndx.Next(0, 255), rndx.Next(0, 255), rndx.Next(0, 255)))
                        End If
                        refx = title.Split("|")
                        If refx.Length > 1 Then
                            For k As Integer = 0 To refx.Length - 1
                                refx2 = refx(k).Split("=>")
                                If refx2.Length > 0 Then
                                    '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                    If refx2(0) <> "" Then
                                        If fm.searcharray(ttitle, refx2(0)) = False Then
                                            ttitle(UBound(ttitle)) = refx2(0)
                                            ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                            ReDim Preserve ttitle(UBound(ttitle) + 1)
                                            '     Response.Write(refx2(0) & "><><>IIIIII<br>")
                                            Session(proj & "ref") &= refx2(0) & "|"


                                        End If
                                    End If
                                Else
                                    ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                                End If
                            Next
                        Else
                            refx2 = refx(0).Split("=>")

                            If refx2.Length > 0 Then
                                '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                If refx2(0) <> "" Then
                                    If fm.searcharray(ttitle, refx2(0)) = False Then
                                        ttitle(UBound(ttitle)) = refx2(0)
                                        ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                        ReDim Preserve ttitle(UBound(ttitle) + 1)
                                        '     Response.Write(refx2(0) & "><><>IIIIII<br>")

                                        Session(proj & "ref") &= refx2(0) & "|"



                                    End If
                                End If
                            Else
                                ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                            End If
                        End If
                    End If

                End While
                rs.Close()

                Dim p As Integer
                Dim unsettedp As String = ""
                Try

                    For p = 0 To UBound(Session("payroll")) - 2
                        If Session(Session("payroll")(p)) = 1 Then
                            unsettedp &= "'" & Session("payroll")(p).split("_")(0) & "',"
                        End If
                    Next
                    For p = 0 To Session("ot").Length - 2
                        If Session(Session("ot")(p)) = 1 Then
                            unsettedp &= "'" & Session("ot")(p).split("_")(0) & "',"
                        End If
                    Next
                    For p = 0 To UBound(Session("inc")) - 2
                        If Session(Session("inc")(p)) = 1 Then
                            unsettedp &= "'" & Session("inc")(p).split("_")(0) & "',"
                        End If
                    Next
                Catch ex As Exception
                    Response.Write(ex.ToString & "<br>")
                End Try
                If unsettedp <> "" Then
                    ' Response.Write("<br>select * from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and ref in(" & unsettedp.Substring(0, unsettedp.Length - 1) & ") order by ref,pddate,emptid,id")
                    rs = dbs.dtmake("vwpayroll", "select * from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and ref in(" & unsettedp.Substring(0, unsettedp.Length - 1) & ") order by ref,pddate,emptid,id", Session("con"))

                    If rs.HasRows Then
                        While rs.Read

                            Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                            ' Response.Write(rs.Item("emptid") & "<br>")
                            inx = inx + 1
                            If i = 1 Then
                                outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headertax(pd1, pno.ToString, pgof.ToString)(0) & headertax(pd1, pno.ToString, pgof.ToString)(1)
                            Else
                                If ccc < pgof Then
                                    '  Response.Write(ccc & "====" & rs.Item("emptid") & "===" & pggno(ccc) & "<br>")
                                    If inx > pggno(ccc) And ccc < pgof Then
                                        inx = 1

                                        ccc = ccc + 1

                                        outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                        outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                        For t = 0 To 9
                                            coltotal(t) = colsum(t) + coltra(t)
                                        Next
                                        outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                        outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr></table></div></div>"
                                        pageno += 1
                                        outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headertax(pd1, pno.ToString, tpg.ToString)(1)
                                        For t = 0 To colsum.Length - 1
                                            coltra(t) = coltotal(t)
                                            colsum(t) = 0
                                        Next

                                    End If
                                End If
                            End If
                            If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                                ' Response.Write("<br>" & rs.Item("emptid") & "====" & fm.isResign(rs.Item("emptid"), Session("con"))(1))
                                If CDate(pd2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                    color = "red"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                    ' Response.Write(title & "<br>")
                                    rlist &= rs.Item("emptid") & ","
                                Else
                                    'color = "green"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                End If

                            End If
                            pdm = fm.getinfo2("select count(id) from payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'", Session("con"))

                            If IsNumeric(pdm) Then
                                If CInt(pdm) > 1 Then
                                    color = "#ffac43"
                                    title = getjavalist2("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "' and ref not in('" & rs.Item("ref") & "')", "ref,txinco,tax,id", Session("con"), "=>", "|")
                                    ' Response.Write("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'<br>")
                                    ' Response.Write(rs.Item("emptid") & 
                                    title = title.Replace(Chr(34), "")
                                    refx = title.Split("|")
                                    If refx.Length > 1 Then
                                        For k As Integer = 0 To refx.Length - 1
                                            refx2 = refx(k).Split("=>")
                                            '  Response.Write(refx2(0) & "xx<br>")
                                            session_off(refx2(0) & "_" & rs.Item("emptid"))
                                        Next
                                    Else
                                        refx2 = refx(0).Split("=>")
                                        ' Response.Write(refx2(0) & "<br>")
                                        session_off(refx2(0) & "_" & rs.Item("emptid"))
                                    End If

                                End If
                            End If
                            session_off(rs.Item("ref") & "_" & rs.Item("emptid"))
                            If fm.searcharray(emptid, rs.Item("emptid")) = False Then
                                emptid(UBound(emptid)) = rs.Item("emptid")
                                ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                ReDim Preserve emptid(UBound(emptid) + 1)

                                outp(1) &= " <tr style='background:" & color & ";' title='" & title & "'>"

                                ' outp(1) &= " <td class='nox'>" & i.ToString & "</td>" & Chr(13)
                                If CDate(rs.Item("pddate")).Subtract(CDate(rs.Item("date_paid"))).Days <> 0 Then
                                    outp(1) &= " <td class='nox' style='background:#0000ff'>" & i.ToString & "</td>" & Chr(13)
                                Else

                                    outp(1) &= " <td class='nox'>" & i.ToString & "</td>" & Chr(13)
                                End If
                                cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))
                                If cell(1) = "None" Then
                                    cell(1) = "&nbsp;"
                                End If
                                outp(1) &= "<td class='tin' width='89'>&nbsp;" & cell(1) & "</td>" & Chr(13)
                                If empid = "None" Then
                                    cell(2) = "&nbsp;"
                                Else
                                    cell(2) = fm.getfullname(empid, Session("con"))
                                End If

                                outp(1) &= " <td class='ename'> " & cell(2) & "</td>" & Chr(13)
                                outp(1) &= "<td class='edate'>" & dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid"), Session("con"))).ToShortDateString) & "</td>" & Chr(13)
                                If rs.IsDBNull(6) = True Then
                                    If CDate(rs.Item("date_paid")).Month <> pd1.Month Then
                                        b_e = fm.getinfo2("select sum(b_e) from payrollx where emptid=" & rs.Item("emptid") & " and date_paid='" & rs.Item("date_paid") & "' and remark='monthly'", Session("con"))

                                    End If
                                Else
                                    b_e = rs.Item(6)
                                End If
                                If IsNumeric(b_e) = False Then
                                    b_e = "0"
                                End If
                                If (IsNumeric(rs.Item("alw"))) = False Then
                                    alwtb = 0
                                Else
                                    alwtb = rs.Item("alw")
                                End If
                                outp(1) &= " <td class='cssamt' width='104'>" & FormatNumber(b_e, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                                outp(1) &= " <td class='cssamt'>" & FormatNumber(alwtb, 2, TriState.True, TriState.True, TriState.True) & " </td>"
                                talw = fm.getinfo2("select sum(talw) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))

                                If IsNumeric(talw) = True Then
                                    ' talw = CDbl(rs.Item("talw")).ToString
                                Else
                                    talw = "0"
                                End If


                                outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(talw, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                                ot_back = fm.getinfo2("select sum(ot) from payrollx where remark='OT-Payment' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))

                                If IsNumeric(ot_back) Then
                                    If IsNumeric(rs.Item("ot")) Then
                                        ot_back = CDbl(ot_back) + CDbl(rs.Item("ot"))

                                    End If
                                Else
                                    If IsNumeric(rs.Item("ot")) Then
                                        ot_back = CDbl(rs.Item("ot"))
                                    Else
                                        ot_back = 0
                                    End If
                                End If
                                outp(1) &= "<td class='cssamt'> " & FormatNumber(ot_back, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                                increament = fm.getinfo2("select sum(txinco) from payrollx where remark='Increament' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                                If IsNumeric(increament) = False Then
                                    increament = "0"
                                End If

                                outp(1) &= "<td class='cssamt'> " & FormatNumber(increament, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                                basic = fm.getinfo2("select sum(b_e) from payrollx where remark='monthly' and pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                                If IsNumeric(basic) Then
                                    tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw) + CDbl(basic)).ToString
                                Else
                                    basic = "0"
                                    tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw)).ToString
                                End If
                                colsum(8) += tincome

                                outp(1) &= "<td class='cssamt'>" & FormatNumber(tincome, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                                taxsum = fm.getinfo2("select sum(tax) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                                If IsNumeric(taxsum) Then
                                    txs = txs + CDbl(taxsum)
                                    '  Response.Write("tax==>" & txs.ToString & "<br>")
                                Else
                                    taxsum = "0"
                                End If
                                colsum(9) += CDbl(taxsum)
                                outp(1) &= "<td class='cssamt'>" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                                outp(1) &= "<td class='cssamt'>&nbsp;</td>" & Chr(13)
                                netpay = fm.getinfo2("select sum(netp) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                                If IsNumeric(netpay) = False Then
                                    ' Response.Write(taxsum)
                                    netpay = "0"
                                End If

                                outp(1) &= "<td class='cssamt'>" & FormatNumber(netpay, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)

                                If cref <> rs.Item("ref") Then

                                    proj = fm.getproj_on_date(rs.Item("emptid"), pd2, Session("con"))(1)
                                    If proj.Length > 12 Then
                                        proj = proj.Substring(0, 12)
                                    Else
                                        proj = proj
                                    End If
                                    Session(proj & "ref") &= rs.Item("ref") & "|"

                                    If fm.searcharray(projlist, proj) = False Then


                                        projlist(UBound(projlist)) = proj
                                        ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                        ReDim Preserve projlist(UBound(projlist) + 1)



                                    Else
                                        '      Response.Write("dup=>" & proj)
                                    End If
                                    If cref = "" Then
                                        Session(proj & "inc") = 0
                                        Session(proj & "otback") = 0
                                        Session(proj & "basic") = 0
                                        Session(proj & "talw") = 0
                                        Session(proj & "tax") = 0
                                    End If
                                    cref = rs.Item("ref")

                                    bgpay = "#" & Hex(RGB(rndx.Next(0, 255), rndx.Next(0, 255), rndx.Next(0, 255)))
                                End If
                                refx = title.Split("|")
                                If refx.Length > 1 Then
                                    For k As Integer = 0 To refx.Length - 1
                                        refx2 = refx(k).Split("=>")
                                        If refx2.Length > 0 Then
                                            '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                            If refx2(0) <> "" Then
                                                If fm.searcharray(ttitle, refx2(0)) = False Then
                                                    ttitle(UBound(ttitle)) = refx2(0)
                                                    ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                                    ReDim Preserve ttitle(UBound(ttitle) + 1)
                                                    '     Response.Write(refx2(0) & "><><>IIIIII<br>")
                                                    If IsDate(refx2(0)) = False Then
                                                        Session(proj & "ref") &= refx2(0) & "|"
                                                    End If

                                                End If
                                            End If
                                        Else
                                            ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                                        End If
                                    Next
                                Else
                                    refx2 = refx(0).Split("=>")
                                    If refx2.Length > 0 Then
                                        '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                        If refx2(0) <> "" Then
                                            If fm.searcharray(ttitle, refx2(0)) = False Then
                                                ttitle(UBound(ttitle)) = refx2(0)
                                                ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                                ReDim Preserve ttitle(UBound(ttitle) + 1)
                                                '     Response.Write(refx2(0) & "><><>IIIIII<br>")
                                                If IsDate(refx2(0)) = False Then
                                                    Session(proj & "ref") &= refx2(0) & "|"
                                                End If


                                            End If

                                        End If
                                    Else
                                        ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                                    End If
                                End If

                                Session(proj & "inc") = CDbl(Session(proj & "inc")) + CDbl(increament)
                                Session(proj & "otback") = CDbl(Session(proj & "otback")) + CDbl(ot_back)
                                Session(proj & "basic") = CDbl(Session(proj & "basic")) + CDbl(basic)
                                Session(proj & "talw") = CDbl(Session(proj & "talw")) + CDbl(talw)
                                Session(proj & "tax") = CDbl(Session(proj & "tax")) + CDbl(taxsum)
                                outp(1) &= "<td class='csssign' style='" & Chr(13) & _
                               "border-right:double 2.25pt black;border-left:solid 1px black;border-bottom:solid 1.0pt black;" & Chr(13) & _
                               "mso-border-bottom-alt:solid .5pt;mso-border-right-alt:solid .5pt;" & Chr(13) & _
                               "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px;font-size:8px;background-color:" & bgpay & "' title='(" & rs.Item("emptid") & ")" & Chr(13) & rs.Item("ref") & "'>"

                                outp(1) &= proj & "</td>" & Chr(13)


                                outp(1) &= "</tr>"
                                i = i + 1
                            Else ' Name duplication...............

                                If cref <> rs.Item("ref") Then

                                    proj = fm.getproj_on_date(rs.Item("emptid"), pd2, Session("con"))(1)
                                    If proj.Length > 12 Then
                                        proj = proj.Substring(0, 12)
                                    Else
                                        proj = proj
                                    End If
                                    Session(proj & "ref") &= rs.Item("ref") & "|"

                                    If fm.searcharray(projlist, proj) = False Then


                                        projlist(UBound(projlist)) = proj
                                        ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                        ReDim Preserve projlist(UBound(projlist) + 1)



                                    Else
                                        '      Response.Write("dup=>" & proj)
                                    End If
                                    If cref = "" Then
                                        Session(proj & "inc") = 0
                                        Session(proj & "otback") = 0
                                        Session(proj & "basic") = 0
                                        Session(proj & "talw") = 0
                                        Session(proj & "tax") = 0
                                    End If
                                    cref = rs.Item("ref")

                                    bgpay = "#" & Hex(RGB(rndx.Next(0, 255), rndx.Next(0, 255), rndx.Next(0, 255)))
                                End If
                                refx = title.Split("|")
                                If refx.Length > 1 Then
                                    For k As Integer = 0 To refx.Length - 1
                                        refx2 = refx(k).Split("=>")
                                        If refx2.Length > 0 Then
                                            '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                            If refx2(0) <> "" Then
                                                If fm.searcharray(ttitle, refx2(0)) = False Then
                                                    ttitle(UBound(ttitle)) = refx2(0)
                                                    ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                                    ReDim Preserve ttitle(UBound(ttitle) + 1)
                                                    '     Response.Write(refx2(0) & "><><>IIIIII<br>")
                                                    Session(proj & "ref") &= refx2(0) & "|"


                                                End If
                                            End If
                                        Else
                                            ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                                        End If
                                    Next
                                Else
                                    refx2 = refx(0).Split("=>")

                                    If refx2.Length > 0 Then
                                        '  Response.Write(refx2(0) & refx2(1) & "<br>")
                                        If refx2(0) <> "" Then
                                            If fm.searcharray(ttitle, refx2(0)) = False Then
                                                ttitle(UBound(ttitle)) = refx2(0)
                                                ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                                                ReDim Preserve ttitle(UBound(ttitle) + 1)
                                                '     Response.Write(refx2(0) & "><><>IIIIII<br>")

                                                Session(proj & "ref") &= refx2(0) & "|"



                                            End If
                                        End If
                                    Else
                                        ' Response.Write("Main..." & rs.Item("ref") & "<br>")
                                    End If
                                End If
                            End If

                        End While
                    End If

                End If
                outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                For t = 0 To 9
                    coltotal(t) = colsum(t) + coltra(t)
                Next
                outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr></table></div></div>"
                pageno += 1
                outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br><table>"
                '  Response.Write((i - 1).ToString & "===" & coltotal(8).ToString & "===9.." & coltotal(9).ToString & "===rs=" & rlist.ToString & "===" & emptid.ToString & "====<br>")
                outp(1) &= "<tr><td colspan='12'>" & summerytax(i - 1, coltotal(8), coltotal(9), rlist) & "</td></tr>"
                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr>"


                outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql & "<br>" & proj & "<br>" & rs.Item("emptid"))

        End Try
        Response.Write("<br><div id='pinta4s'  style='width:100px; height:28px;background:#25a;color:White;cursor:pointer;float:left;'" & _
           " onclick=" & Chr(34) & "javascirpt:printtax('summerytax','Summery_print','" & Session("company_name") & "','Tuesday, November 01, 2016','printA4');" & Chr(34) & ">" & _
      "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 Summery </div><div style='clear:left'></div>  " & _
"<div id='summerytax' style='font-size:10px;'><table style='font-size:10px;'><tr><td>NO </td><td>Proj</td><td>Basic</td><td>OT</td><td>Allowance</td><td>Increament/Bones</td><td>taxable Income</td><td>Tax</td></tr>")
        Dim sumtax As Double = 0
        Dim sumtaxi As Double = 0
        Dim taxinc As Double = 0
        Dim tx As Integer
        Dim ccolor As String = "ff00aa"
        If projlist.Length > 1 Then
            Dim screenx As String = ""
            For tx = 0 To projlist.Length - 2
                screenx = ""
                '  Response.Write(projlist(tx) & tx.ToString & "<br>")
                If IsError(projlist(tx)) = False Then
                    If ccolor = "fdfdfd" Then
                        ccolor = "1369fa"
                    Else
                        ccolor = "4599fd"
                    End If
                    taxinc = CDbl(Session(projlist(tx) & "basic")) + CDbl(Session(projlist(tx) & "otback")) + CDbl(Session(projlist(tx) & "talw")) + CDbl(Session(projlist(tx) & "inc"))
                    Response.Write("<tr style='background-color:#" & ccolor & ";'><td>" & tx.ToString & "</td><td>" & projlist(tx).ToString & "</td>" & _
                       "<td class='cssamt'>" & FormatNumber(Session(projlist(tx) & "basic"), 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
                     "<td class='cssamt'>" & FormatNumber(Session(projlist(tx) & "otback"), 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
                     "<td class='cssamt'>" & FormatNumber(Session(projlist(tx) & "talw"), 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
                       "<td class='cssamt'>" & FormatNumber(Session(projlist(tx) & "inc"), 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
                        "<td class='cssamt'>" & FormatNumber(taxinc, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
                    "<td class='cssamt'>" & FormatNumber(Session(projlist(tx) & "tax"), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>")
                    screenx = getdetail(Session(projlist(tx) & "ref"))
                    Response.Write("<tr><td colspan=8><div style='width:600px'>" & screenx & "</div></td></tr>")
                    sumtax = sumtax + CDbl(Session(projlist(tx) & "tax"))
                    sumtaxi = sumtaxi + taxinc
                End If
            Next

            Response.Write("<tr><td colspan=6>&nbsp;</td><td>" & sumtaxi & "</td><td>" & sumtax.ToString & "</td></tr>")
        End If
        Response.Write("</table></div>")
        Response.Write(pd1 & "--->" & pd2)
        For tx = 0 To projlist.Length - 2

            '  Response.Write(projlist(tx) & tx.ToString & "<br>")
            If IsError(projlist(tx)) = False Then

                Session.Remove(projlist(tx) & "basic")
                Session.Remove(projlist(tx) & "otback")
                Session.Remove(projlist(tx) & "talw")
                Session.Remove(projlist(tx) & "inc")
                Session.Remove(projlist(tx) & "tax")
                Session.Remove(projlist(tx) & "ref")
                Session.Remove("payroll")
                Session.Remove("ot")
                Session.Remove("inc")

            End If
        Next
        Return outp(0)

    End Function
    Function getdetail(ByVal valx As String)
        Dim spl() As String
        Dim ref() As String = {""}
        Dim be, talw, ot, inc As String
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim sec As New k_security
        Dim rtn As String = ""
        spl = valx.Split("|")
        Dim ccolor As String = "white"
        Dim dupref() As String = {""}
        Response.Write(valx)

        If spl.Length > 0 Then
            rtn &= " <table border=0 cellspacing=0  style='font-size:10px;'width:800px' ><tr style='font-weight:bold;text-align:center;'><td>P.No</td><td>Basic sal</td><td>Taxable Allowance</td><td>OT</td><td>Taxable Income</td><td> Tax</td></tr>"
            For j As Integer = 0 To UBound(spl) - 1
                rs = dbx.dtmake("vwrow", "select sum(b_e) as B_earn,sum(talw) as t_allowance,sum(ot) As ot,sum(txinco) as tax_income,sum(tax) as Tax  from payrollx where ref='" & spl(j) & "'", Session("con"))
                If rs.HasRows Then
                    rs.Read()
                    If fm.searcharray(dupref, spl(j)) = False Then
                        dupref(UBound(dupref)) = spl(j)
                        ' Response.Write(UBound(projlist) & "Proj len===>" & proj & "<br>")
                        ReDim Preserve dupref(UBound(dupref) + 1)

                        If ccolor = "white" Then
                            ' ccolor = "gray"
                        Else
                            '   ccolor = "white"
                        End If
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim emptid As String = ""
                        Dim remark As String = ""
                        remark = fm.getinfo2("select remark from payrollx where ref='" & spl(j) & "'", Session("con"))
                        Dim dtp As String
                        dtp = CDate(fm.getinfo2("select date_paid from payrollx where ref='" & spl(j) & "'", Session("con"))).ToShortDateString
                        Dim proj As String
                        emptid = fm.getinfo2("select emptid from payrollx where ref='" & spl(j) & "'", Session("con"))
                        Dim projx() As String
                        projx = fm.getproj_on_date(emptid, dtp, Session("con"))
                        proj = projx(1) & "|" & projx(0) & "|"
                        Dim m, y As String
                        m = dtp.Split("/")(0)
                        y = dtp.Split("/")(2)
                        proj = sec.dbStrToHex(proj)
                        Dim rtnx As String = ""
                        If remark <> "None" Then
                            Select Case LCase(remark)
                                Case "monthly"
                                    rtnx = "  <div class='viewtax' onclick=" & _
                                   Chr(34) & "javascript:payrollview1('view','?prid=" & spl(j) & "&pd=" & Request.Form("pd1") & "&ptype=payroll&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                   Chr(34) & ">" & spl(j) & "</div>"
                                Case "increament"
                                    rtnx = " <div class='viewtax' onclick=" & _
                                    Chr(34) & "javascript:otherpayment('view','?prid=" & spl(j) & "&pd=" & Request.Form("pd1") & "&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                    Chr(34) & ">" & spl(j) & "</div>"
                                Case "pay_inc_middle"
                                    rtnx = " <div class='viewtax' onclick=" & _
                                        Chr(34) & "javascript: payrollmid('view','?prid=" & spl(j) & "&pd=" & Request.Form("pd1") & "&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                        Chr(34) & ">" & spl(j) & "</div>"
                                Case "ot-payment"
                                    rtnx = "  <div class='viewtax' onclick=" & _
                                    Chr(34) & "javascript:otview('view','?prid=" & spl(j) & "&pd=" & Request.Form("pd1") & "&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                    Chr(34) & ">" & spl(j) & "</div>"
                                Case Else
                                    rtnx = "Sorry "


                            End Select
                        End If
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        rtn &= "<tr style='background:" & ccolor & "'><td style='width:20%;'>" & rtnx & "</td>"
                        For k As Integer = 0 To rs.FieldCount - 1
                            If rs.IsDBNull(k) = True Then
                                rtn &= "<td class='cssamt' style='width:13%'>-</td>"
                            Else
                                rtn &= "<td class='cssamt'  style='width:13%'>" & FormatNumber(rs.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                            End If
                        Next
                        rtn &= "</tr>"
                    End If
                End If
                rs.Close()
            Next
            rtn &= "</table>"
        End If
        Return rtn

    End Function

    Public Function makeformpx_payroll()
        Session.Timeout = 60
        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim allemp As String = ""
        Dim fl As New file_list
        Dim sec As New k_security
        Dim namelist As String = ""
        Dim cell(17) As Object

        Dim rrr As DataTableReader
        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim pcheck() As String
        '  Dim sumbsal, sumbearn, sumtalw, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double


        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        'Dim sec As New k_security

        Dim rtnallp As String

        Dim damt As Double = 0
        If Request.QueryString("prid") <> "" Then
            'Response.Write(Request.QueryString("prid"))
        Else
            ' Response.Write(Request.QueryString("month"))
            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                If Request.Form("month") <> "" Then
                    nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
                Else
                    nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                    pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
                    pdate2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
                End If
                ' Response.Write(Request.QueryString("paidst"))
                Dim spl() As String
                Dim projid As String = ""
                Dim projename As String = ""
                Dim pjn() As String
                projename = Request.Form("projname")
                '  Response.Write(projename & "<br>")
                pjn = projename.Split("$")
                Dim rtn(6) As String
                Dim intno As Integer = 0
                'Dim outp As String
                Dim outp As String = ""
                Dim rtnvalue As String = ""
                Dim outputall As String = ""
                Dim ppname As String
                Dim valx() As String
                rtnvalue = ""

                ppname = projename.Replace("""", "")
                ' Response.Write(ppname & "<br>")
                'spl = ppname.Split("|")
                If pjn.Length > 0 Then

                    For i As Integer = 0 To pjn.Length - 1
                        'Response.Write("------------------------------------------------------------------<br>")
                        spl = (pjn(i).Replace("""", "")).Split("|")

                        '   rtnvalue = fm.getprojemp(spl(1), pdate2, Session("con"))
                        ' Response.Write(rtnvalue & "<br>")

                        rtn = getvewlist(pjn(i), pdate1, pdate2)
                        '  rtn(4) = rtnvalue
                        'paidlist &= rtn(5)


                        If rtn(1) = "on" Then
                            outp &= "<div>========================cell " & i & "=============================================<br>"
                            intno += CInt(rtn(2))
                            outp &= rtn(2) & "<br>"
                            outp &= rtn(0).ToString
                            'compair(rtn(4), listin)

                            'Response.Write(rtn(0)) 
                            If spl.Length > 1 Then
                                ' rtnvalue = fm.getprojemp(spl(1), pdate2, Session("con"))
                                ' Dim listin As String = getprojempx(spl(1).ToString, pdate2, Session("con"))
                                'outp &= rtnvalue & "<br>"
                                ' outp &= compair(rtn(4), listin)
                                allemp &= rtn(4)
                                valx = rtnvalue.Split(",")
                                ' outp &= "==>" & UBound(valx) & "<br>"
                                For k As Integer = 0 To valx.Length - 1
                                    If valx(k).Replace("'", "") <> "" Then
                                        If IsDate(fm.isResign(valx(k).Replace("'", ""), Session("con"))(1)) Then
                                            ' outp &= "<br>resign " & valx(k) & " " & fm.isResign(valx(k).Replace("'", ""), Session("con"))(1)
                                        End If
                                    End If
                                Next
                            End If

                            outputall &= rtn(3) & "|"

                            outp &= "<div class='' onclick=" & Chr(34) & "javascript:gotopension('Viewall','" & rtn(3) & "')" & Chr(34) & " style='color:blue;'> View Section</div>"
                            ' pcheck = pchecklist(pdate1, pdate2, pjn(i))
                            'outp &= ("list emp2:" & rtn(4) & "<br>")
                            ' outp &= ("unpaid:" & pcheck(1) & "<br>")
                            'outp &= ("Resign:" & pcheck(2) & "<br>")

                        End If
                        ' Response.Write("------------------------------------------------------------------<br>")

                    Next
                    Dim sortarr() As String
                    allemp = allemp.Replace("""", "")
                    allemp = allemp.Replace(Chr(13), "")
                    allemp = allemp.Replace(" ", "")
                    Response.Write("<div class='' onclick=" & Chr(34) & "javascript:gotopension('Viewtotal','" & allemp.Replace("'", "") & "')" & Chr(34) & " style='color:blue;cursor:pointer'> View all</div>")
                    intno = allemp.Split(",").Length

                    Response.Write("Total No. emp.:" & intno.ToString & "<br>")
                    Response.Write(outp)
                    ' Response.Write("paidlist:" & allemp & "<br>")
                    sortarr = allemp.Replace("'", "").Split(",")
                    Array.Sort(sortarr)

                    ' Response.Write("total employee:")
                    For j As Integer = 0 To sortarr.Length - 1
                        '    Response.Write(sortarr(j) & ",")
                    Next


                    ' Response.Write("<br>")
                    'pcheck = pchecklist_x(pdate1, pdate2)
                    'Response.Write(compair(pcheck(0), allemp))

                End If
            End If
        End If
    End Function
    Public Function makeformp_taxx() 'payroll tax
        Session.Timeout = 60
        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim allemp As String = ""
        Dim fl As New file_list
        Dim sec As New k_security
        Dim namelist As String = ""
        Dim cell(17) As Object

        Dim rrr As DataTableReader
        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim pcheck() As String
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

        Dim rtnallp As String

        Dim damt As Double = 0

        ' Response.Write(Request.QueryString("month"))
        If Request.Item("pd1") <> "" Then

            If Request.Form("pd1") <> "" Then
                nod = Date.DaysInMonth(CDate(Request.Form("pd1")).Year, CDate(Request.Form("pd1")).Month)
                pdate1 = Request.Form("pd1")
                pdate2 = Request.Form("pd2")
            Else
                '  nod = Date.DaysInMonth(CDate(Request.QueryString("pd1")).Year, CDate(Request.QueryString("pd1")).Month)
                pdate1 = Request.QueryString("pd1")
                pdate2 = Request.QueryString("pd2")
            End If
            ' Response.Write(Request.QueryString("paidst"))
            Dim spl() As String
            Dim projid As String = ""
            Dim projename As String = ""
            Dim pjn() As String
            projename = Request.Form("projname")
            '  Response.Write(projename & "<br>")
            pjn = projename.Split("$")
            Dim rtn(6) As String
            Dim intno As Integer = 0
            'Dim outp As String
            Dim outp As String = ""
            Dim rtnvalue As String = ""
            Dim outputall As String = ""
            Dim ppname As String
            Dim valx() As String



            ' Response.Write(ppname & "<br>")
            'spl = ppname.Split("|")
            Response.Write(pdate1 & "==" & pdate2)
            '  If pjn.Length > 0 Then
            rtn(4) = getjavalist2("payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "' and (remark='monthly' or remark='pay_inc_middle') order by ref", "distinct emptid,ref", Session("con"), "|", "$")
            '  Response.Write(rtn(4))
            allemp = rtn(4).Replace("""", "")
            allemp = allemp.Replace("$", ",")
            allemp = allemp.Replace("xx", "")
            allemp = allemp.Substring(0, allemp.Length - 1)
            spl = allemp.Split(",")
            allemp = ""
            For i As Integer = 0 To spl.Length - 1
                allemp &= spl(i).Split("|")(0) & ","
            Next
            allemp = allemp.Substring(0, allemp.Length - 1)
            paidlist = allemp.Split(",").Length
            Dim sortarr() As String
            Response.Write("<div class='' onclick=" & Chr(34) & "javascript:gototax('Viewtotal','" & allemp & "')" & Chr(34) & " style='color:blue;cursor:pointer;'> View all....</div>")
            Response.Write("Total No. emp.:" & CInt(paidlist) & "<br>")
            Response.Write(outp)

            sortarr = allemp.Replace("'", "").Split(",")
            Array.Sort(sortarr)

            ' Response.Write("total employee:")

        End If
    End Function
    Public Function makeformp_tax() 'payroll tax
        ' Response.Write("tax")
        Session.Timeout = 60
        Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim allemp As String = ""
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

        Dim rtnallp As String = ""

        Dim damt As Double = 0
        If Request.QueryString("prid") <> "" Then
            ' Response.Write(Request.QueryString("prid"))
        Else
            ' Response.Write(Request.QueryString("month"))
            If Request.Form("pd1") <> "" Or Request.QueryString("pd2") <> "" Then
                ' If Request.Form("month") <> "" Then
                'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                'pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
                'pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
                'Else
                '   nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                '  pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
                ' pdate2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
                'End If
                ' Response.Write(Request.QueryString("paidst"))
                pdate1 = Request.Item("pd1")
                pdate2 = Request.Item("pd2")
                nod = Date.DaysInMonth(pdate1.Year, pdate1.Month)
                Dim spl() As String
                Dim projid As String = ""
                Dim projename As String = ""
                Dim pjn() As String

                projename = Request.Form("projname")
                ' Response.Write(projename & "<br>")
                pjn = projename.Split("$")
                Dim rtn(6) As String
                Dim intno As Integer = 0
                'Dim outp As String
                Dim outp As String = ""
                Dim rtnvalue As String = ""
                Dim outputall As String = ""
                Dim ppname As String
                Dim valx() As String

                Response.Write("From " & pdate1 & " to:" & pdate2 & "<br>")
                ppname = projename.Replace("""", "")

                ' Response.Write(ppname & "<br>")
                'spl = ppname.Split("|")
                If pjn.Length > 0 Then
                    ' Response.Write(pjn)
                    For i As Integer = 0 To pjn.Length - 1
                        'Response.Write("------------------------------------------------------------------<br>")
                        spl = (pjn(i).Replace("""", "")).Split("|")

                        '   rtnvalue = fm.getprojemp(spl(1), pdate2, Session("con"))
                        ' Response.Write(rtnvalue & "<br>")

                        rtn = getvewlist_tax(pjn(i), pdate1, pdate2)
                        '  rtn(4) = rtnvalue
                        'paidlist &= rtn(5)


                        If rtn(1) = "on" Then
                            outp &= "<div>=====================================================================<br>"
                            intno += CInt(rtn(2))
                            outp &= rtn(2) & "<br>"
                            outp &= rtn(0).ToString
                            'compair(rtn(4), listin)

                            'Response.Write(rtn(0)) 
                            If spl.Length > 1 Then
                                rtnvalue = fm.getprojemp(spl(1), pdate2, Session("con"))
                                ' Dim listin As String = getprojempx(spl(1).ToString, pdate2, Session("con"))
                                ' outp &= rtnvalue & "<br>"
                                ' outp &= compair(rtn(4), listin)
                                allemp &= rtn(4)
                                valx = rtnvalue.Split(",")
                                ' outp &= "==>" & UBound(valx) & "<br>"
                                '   Response.Write("wwwwwwwwwwwwwwwwww")
                            End If
                            outp &= "</div>"
                            outputall &= rtn(3) & "|"
                            outp &= "<div class='' onclick=" & Chr(34) & "javascript:gototax('Viewall','" & rtn(3) & "')" & Chr(34) & " style='color:blue;'> View Section</div>"
                        End If
                        ' Response.Write("------------------------------------------------------------------<br>")

                    Next
                    allemp = allemp.Replace(Chr(34), "").Replace(" ", "")
                    allemp = allemp.Substring(0, allemp.Length - 1)
                    '  Response.Write(allemp & "<br>payrollx where emptid Not in(" & allemp & ") and  pddate between '" & pdate1 & "' and '" & pdate2 & "' order by emptid,id")
                    paidlist = fm.getalllist(Session("con"), "payrollx where emptid Not in(" & allemp & ") and  pddate between '" & pdate1 & "' and '" & pdate2 & "'", "ref")
                    rtnallp = getjavalist2("payrollx where ref in(" & paidlist.Replace(Chr(34), "'") & ") ", " distinct emptid", Session("con"), "", ",")
                    paidlist = paidlist.Replace(",", "<br>").Replace(Chr(34), "")
                    Response.Write("<div>=====================================================================<br>Orphan Paidlist<br>" & paidlist & "<br>" & rtnallp)
                    rtnallp = rtnallp.Replace(Chr(34), "").Replace(Chr(13), "")
                    rtnallp &= "," & allemp.Replace(Chr(13), "")
                    outputall &= paidlist
                    Response.Write("<div class='' onclick=" & Chr(34) & "javascript:gototax('Viewtotal','" & rtnallp & "')" & Chr(34) & " style='color:blue; cursor:pointer;'> View all</div>")

                    Response.Write("Total No. emp.:" & intno.ToString & "<br>")
                    Response.Write(outp)
                    'Response.Write("paidlist:" & paidlist & "<br>")
                    '  Response.Write("total employee:" & allemp)



                    '   File.WriteAllText("c:\temp\temmmp.txt", allemp.Replace("$", Chr(13)))

                End If
            End If
        End If
    End Function
    Function pay_jv(ByVal pdate1 As Date, ByVal pdate2 As Date)
        Dim rrr As DataTableReader
        Dim dbs As New dbclass
        Dim arrmonth(), arrot(), arrinc() As String
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim i As Integer = 1
        Session.Remove("payroll")
        Session.Remove("ot")
        Session.Remove("inc")

        rrr = dbs.dtmake("payrol", "select distinct ref,pddate,date_paid from payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
        If rrr.HasRows Then
            While rrr.Read


                rs = dbs.dtmake("pyx", "select emptid from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                If rs.HasRows Then
                    While rs.Read
                        ReDim Preserve arrmonth(i)
                        ssession_on(rrr.Item("ref") & "_" & rs.Item("emptid"))
                        arrmonth(i - 1) = rrr.Item("ref") & "_" & rs.Item("emptid")
                        i = i + 1
                    End While
                End If
                rs.Close()
                'Session(rrr.Item("ref") & "_emp") = fm.getjavalist2("payrollx where ref='" & rrr.Item("ref") & "'", "distinct emptid", Session("con"), "")



            End While
        Else
            Response.Write("error")
        End If
        rrr.Close()
        i = 1
        rrr = dbs.dtmake("payrol", "select distinct ref,pddate,date_paid from payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "' and (remark='OT-Payment' and b_sal is null)", Session("con"))
        If rrr.HasRows Then
            While rrr.Read

                rs = dbs.dtmake("pyx", "select emptid from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                If rs.HasRows Then
                    While rs.Read
                        ReDim Preserve arrot(i)
                        ssession_on(rrr.Item("ref") & "_" & rs.Item("emptid"))
                        arrot(i - 1) = rrr.Item("ref") & "_" & rs.Item("emptid")
                        i = i + 1
                    End While
                End If
                rs.Close()


            End While
        Else
            Response.Write("error")
        End If
        rrr.Close()
        i = 1
        rrr = dbs.dtmake("payrol", "select distinct ref,pddate,date_paid from payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "' and (remark='Increament' and b_sal is null)", Session("con"))
        If rrr.HasRows Then
            While rrr.Read

                rs = dbs.dtmake("pyx", "select emptid from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                If rs.HasRows Then
                    While rs.Read
                        ReDim Preserve arrinc(i)
                        ssession_on(rrr.Item("ref") & "_" & rs.Item("emptid"))
                        arrinc(i - 1) = rrr.Item("ref") & "_" & rs.Item("emptid")
                        i = i + 1
                    End While
                End If
                rs.Close()


            End While
        Else
            Response.Write("error")
        End If
        rrr.Close()

        Session("payroll") = arrmonth
        Session("ot") = arrot
        Session("inc") = arrinc

    End Function
    Function getvewlist(ByVal projname As String, ByVal pdate1 As Date, ByVal pdate2 As Date) 'pension
        ' Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim fl As New file_list
        Dim sec As New k_security
        Dim namelist As String = ""
        Dim chklist As String
        ' Dim cell(17) As Object
        Dim rtn(6) As String
        Dim rrr As DataTableReader
        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim spl() As String
        Dim projid As String = ""
        Dim projename As String = ""
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

        ' Dim paidlist As String = ""

        Dim paid As String
        Dim j As Integer

        'If String.IsNullOrEmpty(Request.Form("projname")) = False Then
        'spl = projname.Split("|")
        projename = projname.Replace("""", "")

        For j = 0 To rtn.Length - 1
            rtn(j) = ""
        Next
        ' projename = sec.dbHexToStr(Request.QueryString("projname"))
        spl = projename.Split("|")
        Dim cct As Integer = 0

        If spl.Length > 1 Then
            projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
        Else
            projid = ""
        End If
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
            rrr = dbs.dtmake("payrol", "select distinct ref,date_paid from payrollx where pr='" & paid & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
            ' Response.Write("select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='monthly'")
            Dim emid, rtnvalue, eml() As String
            ' Response.Write(pdate2)
            rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))

            'rtn(4) = rtnvalue
            Dim listemid As String = ""
            If rrr.HasRows Then
                rtn(0) = ("<div id='viewlistx'><b>Project: " & projname & "</b><table>")
                rtn(1) = "off"
                Dim ccout As String = "0"
                Dim loc As String = Server.MapPath("download") & "\operatorview.txt"
                loc = loc.Replace("\", "/")

                While rrr.Read
                    ' Response.Write(rrr.Item(0))
                    ' Response.Write(projid)
                    If projid.ToString <> "" Then
                        'Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')", Session("con")))
                        ccout = "0"
                        ccout = fm.getinfo2("select count(id) from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                        '  Response.Write(rrr.Item("ref") & "<br>")

                        ' Response.Write(rrr.Item("ref"))
                        emid = fm.getinfo2("select emptid from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                        'Response.Write(emid & "===" & rtnvalue & "----" & rrr.Item("ref") & pdate2.ToShortDateString & "<br>")
                        eml = rtnvalue.Split(",")
                        If fm.searcharray(eml, "'" & emid & "'") Then
                            'Response.Write(rrr.Item("ref"))
                            ' listemid &= "'" & emid & "','"  
                            paidlist = fm.getalllist(Session("con"), "payrollx where ref='" & rrr.Item("ref") & "'", "emptid") & ","
                            ' Response.Write(rrr.Item("ref") & "++++" & paidlist & "<br>")
                            rtn(4) &= paidlist
                            rtn(0) &= "<tr><td class='listcont'>" & rrr.Item(0) & "<br><span style='color:gray;font-size:10pt;'>(No. List:" & ccout & ")</span>" & Chr(13) & _
                           "</td><td class='v1'><div class='viewpayrol' "
                            Dim fx() As String = {""}

                            If String.IsNullOrEmpty(Session("right")) = False Then
                                fx = Session("right").split(";")
                                ReDim Preserve fx(UBound(fx) + 1)
                                fx(UBound(fx) - 1) = ""
                            End If

                            'rtn(1) = "on"


                            If fm.searcharray(fx, "1") Or fm.searcharray(fx, "9") Then
                                If emid <> "''" Then
                                    cct += CInt(ccout)

                                End If
                                paidlist = paidlist.Replace("""", "")
                                paidlist = paidlist.Replace(Chr(13), "")
                                rtn(1) = "on"
                                rtn(0) &= ("onclick = " & Chr(34) & Chr(13) & _
                                    "javascript:gotopension('view','" & paidlist & "');" & Chr(34))
                                rtn(3) &= paidlist 'rrr.Item(0) & "|"
                            End If
                            rtn(0) &= "</div></td></tr>"
                            rtn(0) &= "<tr><td></td></tr>"

                        Else

                            If IsDate(fm.isResign(emid, Session("con"))(1)) Then
                                Dim dtemp As Date
                                dtemp = fm.isResign(emid, Session("con"))(1)
                                '  Response.Write(rrr.Item("ref"))
                                If pdate2.Subtract(dtemp).Days > 0 Then
                                    If pdate2.Month = dtemp.Month And pdate2.Year = dtemp.Year Then
                                        ' Response.Write("resign:" & emid & ": " & fm.isResign(emid, Session("con"))(1) & "<br>")
                                    End If


                                End If

                            Else
                                '  Response.Write(rrr.Item("ref"))
                                '  Response.Write(emid & ">><<<<<<<<<<<br>")
                            End If
                        End If
                    End If

                End While
                rrr.Close()
                rtn(0) &= ("</tr></tr><td></td></tr>")
                rtn(5) = paidlist
                rtn(0) &= ("</table></div>")
                ' Response.Write(listemid & ".......<br>")
            End If

            'Response.Write(paid)
        End If
        If rtn(3) <> "" Then
            rtn(3) = rtn(3).Substring(0, rtn(3).Length - 1)
        End If
        rtn(2) = cct.ToString
        Return rtn
    End Function


    Function getvewlist_tax(ByVal projname As String, ByVal pdate1 As Date, ByVal pdate2 As Date) As Array 'payroll tax
        ' Dim pdate1, pdate2 As Date
        '  Dim nod As Integer
        Dim paidlist As String = ""
        Dim fl As New file_list
        Dim sec As New k_security
        Dim namelist As String = ""
        ' Dim cell(17) As Object
        Dim rtn(6) As String
        Dim rrr As DataTableReader
        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim spl() As String
        Dim projid As String = ""
        Dim projename As String = ""
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

        ' Dim paidlist As String = ""

        Dim paid As String
        Dim j As Integer

        'If String.IsNullOrEmpty(Request.Form("projname")) = False Then
        'spl = projname.Split("|")
        projename = projname.Replace("""", "")

        For j = 0 To rtn.Length - 1
            rtn(j) = ""
        Next
        ' projename = sec.dbHexToStr(Request.QueryString("projname"))
        spl = projename.Split("|")
        Dim cct As Integer = 0

        If spl.Length > 1 Then
            projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
        Else
            projid = ""
        End If
        Dim paypaid As Integer = 0
        ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), Session("con"))
        If paypaid <> 0 Then
            ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", Session("con")))

        End If
        paid = ""
        '  paid = fm.getinfo2("select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "' and id in(select pr from payrollx where ref_inc is null or ref_inc='0')", Session("con"))

        rrr = dbs.dtmake("payrol", "select distinct ref,pddate,date_paid from payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
        ' Response.Write("select distinct ref,date_paid from payrollx where pr='" & paid & "' and remark='monthly'")
        Dim emid, rtnvalue, eml() As String
        ' Response.Write(pdate2)


        Dim listemid As String = ""
        If rrr.HasRows Then
            rtn(0) = ("<div id='viewlistx'><b>Project: " & projname & "</b><table>")
            rtn(1) = "off"
            Dim ccout As String = "0"
            Dim loc As String = Server.MapPath("download") & "\operatorview.txt"
            loc = loc.Replace("\", "/")
            rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
            While rrr.Read
                ' Response.Write(rrr.Item(0))
                ' Response.Write(projid & "<br>")
                ' rtn(4) &= getjavalist2("payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "' and (remark='monthly' or remark='pay_inc_middle')", "emptid", Session("con"), "", "$") & "$"
                ssession_on(rrr.Item("ref"))

                If projid.ToString <> "" Then

                    'Response.Write(fm.getinfo2("select project_id from emp_job_assign where emptid in(select emptid from payrollx where ref='" & rrr.Item(0) & "') and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')", Session("con")))
                    ccout = "0"
                    ccout = fm.getinfo2("select count(id) from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                    '  Response.Write(rrr.Item("ref") & "<br>")

                    ' Response.Write(rrr.Item("ref"))
                    emid = fm.getinfo2("select emptid from payrollx where ref='" & rrr.Item("ref") & "'", Session("con"))
                    'Response.Write(emid & "===" & rtnvalue & "----" & rrr.Item("ref") & pdate2.ToShortDateString & "<br>")
                    eml = rtnvalue.Split(",")
                    paidlist = fm.getalllist(Session("con"), "payrollx where ref='" & rrr.Item("ref") & "'", "emptid") & ","
                    '  Response.Write(rrr.Item("ref") & "++++" & paidlist & "<br>")
                    If fm.searcharray(eml, "'" & emid & "'") Then
                        'Response.Write(rrr.Item("ref"))
                        ' listemid &= "'" & emid & "','"
                        rtn(4) &= paidlist
                        rtn(0) &= "<tr><td class='listcont'>" & rrr.Item(0) & "<br><span style='color:gray;font-size:10pt;'>(No. List:" & ccout & ")</span>" & Chr(13) & _
                       "</td><td class='v1'><div class='viewpayrol' "
                        Dim fx() As String = {""}
                        If String.IsNullOrEmpty(Session("right")) = False Then
                            fx = Session("right").split(";")
                            ReDim Preserve fx(UBound(fx) + 1)
                            fx(UBound(fx) - 1) = ""
                        End If

                        'rtn(1) = "on"


                        If fm.searcharray(fx, "1") Or fm.searcharray(fx, "3") Or fm.searcharray(fx, "9") Then
                            If emid <> "''" Then
                                cct += CInt(ccout)

                            End If
                            rtn(1) = "on"
                            rtn(0) &= ("onclick = " & Chr(34) & Chr(13) & _
                        "javascript:gototax('view','" & rrr.Item(0) & "');" & Chr(34))
                            rtn(3) &= rrr.Item(0) & "|"
                        End If
                        rtn(0) &= "</div></td><tr><td>&nbsp;</td></tr>"
                        rtn(0) &= "<tr><td></td></tr>"

                    Else

                        If IsDate(fm.isResign(emid, Session("con"))(1)) Then
                            Dim dtemp As Date
                            dtemp = fm.isResign(emid, Session("con"))(1)
                            ' Response.Write(rrr.Item("ref") & "<<<<<<<<<<<<" & emid & "====" & dtemp & "<br>")
                            If pdate2.Subtract(dtemp).Days > 0 Then
                                If pdate2.Month = dtemp.Month And pdate2.Year = dtemp.Year Then
                                    ' Response.Write("resign:" & emid & ": " & fm.isResign(emid, Session("con"))(1) & "<br>")
                                End If


                            End If

                        Else
                            '  Response.Write(rrr.Item("ref"))
                            '  Response.Write(emid & ">><<<<<<<<<<<br>")
                        End If
                    End If
                End If

            End While
            rrr.Close()
            rtn(0) &= ("</tr></tr><td></td></tr>")
            rtn(5) = paidlist
            rtn(0) &= ("</table></div>")
            ' Response.Write(listemid & ".......<br>")
        End If


        If rtn(3) <> "" Then
            rtn(3) = rtn(3).Substring(0, rtn(3).Length - 1)
        End If
        rtn(2) = cct.ToString
        Return rtn
    End Function



    Function makepage_sec_tax(ByVal ref As String, ByVal pd1 As Date, ByVal pd2 As Date)
        Response.Write(pd1 & "---" & pd2 & "<br>")
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtime
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        '  Dim pstamt, psumtax, tpstamt, tpsumtax As Double
        ' Dim pd1, pd2 As String
        ' pd1 = month & "/1/" & year
        'pd2 = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        Dim sumpen As Double
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim i As Integer = 1
        Dim j As Integer
        Dim txinc, txot As String
        txinc = ""
        txot = ""
        ' Response.Write(ref)
        Dim increament As String
        Dim fixed As Integer

        Dim trow As Integer
        Dim ot_back As String
        Dim taxsum, tincome, talw As String
        Dim basic As String
        Dim pno, tpg As Integer
        Dim sql As String = ""
        Dim color As String = "white"
        Dim pgof As String = ""
        If File.Exists("c:\temp\consttax.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        ' count = 0
        Dim pageno As Integer = 1
        ' Dim refx As String
        Dim tempf As Integer
        Dim t As Integer
        Dim rlist As String = ""
        Dim reff As String
        Dim refx() As String
        refx = ref.Split("|")
        ref = ref.Replace("|", ",")
        reff = ref
        ref = ""
        If refx.Length > 1 Then
            For p As Integer = 0 To refx.Length - 1
                '  Response.Write("<br>" & refx(p))
                ref &= "'" & refx(p) & "',"
            Next
        Else
            ref = "'" & reff & "',"
        End If
        ' Response.Write("<br>" & ref)
        ref = "ref in(" & ref.Substring(0, ref.Length - 1) & ")"
        ' sql = "select count(id) from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')"
        Dim title As String = ""
        ' pgof = fm.getinfo2(sql, Session("con"))
        pgof = fm.getinfo2("select count(id) from payrollx where " & ref, Session("con"))
        Dim pca As Integer = 0
        Dim nod, nods As Integer
        Dim pgsize(4) As Integer
        Dim pk As Integer = 1
        Dim fp, lsp, pgs, tpgx As Integer
        Dim dblx As Double
        fp = 13
        pgs = 17
        lsp = 7
        dblx = (pgof - 10) / 20
        'dblx = (pgof / 20) - pgs
        tpgx = 0
        pca = pgof
        ' Response.Write(pgof & "......" & dblx & "<br>")
        Dim pggno() As Integer = {0}
        Dim ccc As Integer = 0
        If pgof > 17 Then
            While pca > 0
                ccc = ccc + 1
                If pca > 10 Then
                    If pggno.Length = 1 Then
                        ReDim Preserve pggno(ccc)
                        pggno(ccc - 1) = 10
                        pca -= 10
                    ElseIf pca > 20 Then
                        pca -= 20
                        ReDim Preserve pggno(ccc)
                        pggno(ccc - 1) = 20
                    Else
                        pca -= 10
                        ReDim Preserve pggno(ccc)
                        pggno(ccc - 1) = 10
                    End If
                Else
                    ReDim Preserve pggno(ccc)
                    pggno(ccc - 1) = pca
                    pca = 0
                End If

            End While
        Else
            ccc = ccc + 1
            If pca > 10 Then

                ReDim pggno(2)
                pggno(0) = 10
                pggno(1) = (pca - 10)
                pca = 0


            Else
                ReDim Preserve pggno(ccc)
                pggno(ccc - 1) = pca
                pca = 0
            End If

        End If

        Try
            Dim pdm As String

            'Response.Write(fixed)
            nod = Date.DaysInMonth(CDate(pd1).Year, CDate(pd1).Month)

            ' trow = fm.getinfo2("select count(id) from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))
            ' tpg = CInt(trow) / fixed
            ' rs = dbs.dtmake("vwpayroll", "select * from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))
            '  Response.Write("select distnict emptid from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')")
            '    Response.Write("select * from payrollx where " & ref)
            rs = dbs.dtmake("vwpayroll", "select * from payrollx where " & ref, Session("con"))
            If rs.HasRows Then

                pno = 1
                ' outp(0) = headertax(month.ToString & "/1/" & year.ToString, pno.ToString, tpg.ToString)
                '  outp(1) &= "<div class='page'>"
                pgof = pggno.Length - 1
                Dim inx As Integer = 0
                ccc = 0
                'Response.Write(headertax2(pd1, pno.ToString, pgof.ToString)(0) & headertax(pd1, pno.ToString, pgof.ToString)(1) & "</table>")

                While rs.Read
                    title = ""
                    color = ""
                    Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                    ' Response.Write(rs.Item("emptid") & "<br>")
                    inx = inx + 1

                    If i = 1 Then
                        outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headertax(pd1, pno.ToString, pgof.ToString)(0) & headertax(pd1, pno.ToString, pgof.ToString)(1)
                    Else
                        If inx > pggno(ccc) And ccc < pgof Then
                            inx = 1
                            ccc = ccc + 1

                            outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            For t = 0 To 9
                                coltotal(t) = colsum(t) + coltra(t)
                            Next
                            outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr></table></div></div>"
                            pageno += 1
                            outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headertax(pd1, pno.ToString, tpg.ToString)(1)
                            For t = 0 To colsum.Length - 1
                                coltra(t) = coltotal(t)
                                colsum(t) = 0
                            Next
                            If pageno = 2 Then
                                tempf = fixed + i
                                ' Response.Write("<br>" & tempf & "<br>")
                            Else
                                tempf = tempf + fixed
                            End If
                            'End If
                        End If
                    End If
                    title = ""
                    If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                        ' Response.Write("<br>" & rs.Item("emptid") & "====" & fm.isResign(rs.Item("emptid"), Session("con"))(1))
                        If CDate(pd2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                            color = "red"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                            ' Response.Write(title & "<br>")
                            rlist &= rs.Item("emptid") & ","
                        Else
                            'color = "green"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                        End If

                    End If
                    pdm = fm.getinfo2("select count(id) from payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
                    If pdm > 1 Then
                        color = "#ffac43"
                        title = getjavalist2("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'", "ref,txinco,tax", Session("con"), "=>", "|")
                        ' Response.Write("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'<br>")
                    End If
                    outp(1) &= " <tr style='background:" & color & ";' title='" & title & "'>"
                    outp(1) &= " <td class='nox'>" & i.ToString & "</td>" & Chr(13)
                    cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))
                    If cell(1) = "None" Then
                        cell(1) = "&nbsp;"
                    End If
                    outp(1) &= "<td class='tin' width='89'>&nbsp;" & cell(1) & "</td>" & Chr(13)
                    If empid = "None" Then
                        cell(2) = "&nbsp;"
                    Else
                        cell(2) = fm.getfullname(empid, Session("con"))
                    End If

                    outp(1) &= " <td class='ename'> " & cell(2) & "</td>" & Chr(13)
                    outp(1) &= "<td class='edate'>" & dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid"), Session("con"))).ToShortDateString) & "</td>" & Chr(13)
                    outp(1) &= " <td class='cssamt' width='104'>" & FormatNumber(rs.Item("b_e"), 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= " <td class='cssamt'> &nbsp; " & FormatNumber(rs.Item("alw"), 2, TriState.True, TriState.True, TriState.True) & " </td>"
                    If IsNumeric(rs.Item("talw")) Then
                        talw = CDbl(rs.Item("talw")).ToString
                    Else
                        talw = "0"
                    End If


                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(talw, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    ot_back = fm.getinfo2("select sum(ot) from payrollx where remark='OT-Payment' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))

                    If IsNumeric(ot_back) Then
                        If IsNumeric(rs.Item("ot")) Then
                            ot_back = CDbl(ot_back) + CDbl(rs.Item("ot"))
                            'txot &= getjavalist2("payrollx", " distnict ref", Session("con"), "", "$")

                        End If
                    Else
                        If IsNumeric(rs.Item("ot")) Then
                            ot_back = CDbl(rs.Item("ot"))
                        Else
                            ot_back = 0
                        End If
                    End If

                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(ot_back, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    increament = fm.getinfo2("select sum(txinco) from payrollx where remark='Increament' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(increament) = False Then
                        increament = "0"
                    Else
                        ' txinc &= getjavalist2("payrollx", "ref", Session("con"), "", "$")
                        ' Response.Write(getjavalist2("payrollx", " distinct ref", Session("con"), "", "$"))

                    End If

                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(increament, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    basic = fm.getinfo2("select b_e from payrollx where remark='monthly' and pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(basic) Then
                        tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw) + CDbl(basic)).ToString
                    Else
                        tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw)).ToString
                    End If
                    colsum(8) += tincome

                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(tincome, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    taxsum = fm.getinfo2("select sum(tax) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(taxsum) Then
                    Else
                        taxsum = "0"
                    End If
                    colsum(9) += CDbl(taxsum)
                    outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= "<td class='cssamt'>&nbsp;</td>" & Chr(13)
                    taxsum = fm.getinfo2("select sum(netp) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(taxsum) = False Then
                        Response.Write(taxsum)
                        taxsum = "0"
                    End If

                    outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= "<td class='csssign' style='" & Chr(13) & _
                   "border-right:double 2.25pt black;border-left:solid 1px black;border-bottom:solid 1.0pt black;" & Chr(13) & _
                   "mso-border-bottom-alt:solid .5pt;mso-border-right-alt:solid .5pt;" & Chr(13) & _
                   "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px;font-size:8px;' title='(" & rs.Item("emptid") & ")" & rs.Item("ref") & "'>&nbsp; </td>" & Chr(13)

                    outp(1) &= "</tr>"
                    i = i + 1
                End While

                outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                For t = 0 To 9
                    coltotal(t) = colsum(t) + coltra(t)
                Next
                outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr>"
                outp(1) &= "<tr><td colspan='13'>" & summerytax(i - 1, coltotal(8), coltotal(9), rlist) & "</td></tr>"

                outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try

        Return outp(0) '& "<br>......................................................<br>" & txinc & "<br>......................................................<br>" & txot
    End Function



    Function compair(ByVal rtnv As String, ByVal listin As String)
        Dim a(), ar() As String
        a = rtnv.Replace("'", "").Split(",")
        ar = listin.Replace("'", "").Split(",")

        Dim fm As New formMaker
        Dim id As String
        Dim outp As String = "<table>"
        ' For k As Integer = 0 To ar.Length - 1
        For j As Integer = 0 To a.Length - 2
            '     Response.Write(a(j) & "<br>")
            If fm.searcharray(ar, a(j)) = False Then
                outp &= "<tr><td>" & a(j) & "</td></tr>"
                If IsDate(fm.isResign(a(j).Replace("'", ""), Session("con"))(1)) Then
                    outp &= "<tr><td>....resign" & fm.isResign(a(j).Replace("'", ""), Session("con"))(1) & "</td></tr>"
                End If

            End If
        Next

        'Next
        outp &= "</table>"
        ' Response.Write(outp)
        Return outp

    End Function

    Public Function payroll_checklist()
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim outp As String = ""
        Dim nod As Integer
        Dim pd1, pd2 As String
        Dim pd As String
        Dim rtn() As String = {"", "", "", "", ""}

        Dim count As String
        If Request.Form("month") <> "" Then
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pd1 = Request.Form("month") & "/1/" & Request.Form("year")
            pd2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
        Else
            nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
            pd1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
            pd2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
        End If
        Dim color As String = "#4589fa"
        Dim sql As String = "SELECT emprec.id, esi.first_name,esi.middle_name,emprec.active FROM emprec INNER JOIN emp_static_info as esi ON emprec.emp_id = esi.emp_id where  ('" & pd1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pd2 & "') ) order by esi.first_name,esi.middle_name " '"SELECT emprec.id, esi.first_name,esi.middle_name,emprec.active FROM emprec INNER JOIN emp_static_info as esi ON emprec.emp_id =esi.emp_id  where end_date is null or ('" & CDate(pd1).ToShortDateString & "' between hire_date and isnull(end_date,'" & CDate(pd2).ToShortDateString & "') and emprec.id not in(select emptid from emp_resign where resign_date<'" & CDate(pd1).ToShortDateString & "'))"
        Try
            '   Response.Write(sql)
            Dim unpaid() As String = {""}
            rs = dbs.dtmake("chklst", sql, Session("con"))
            If rs.HasRows Then
                count = fm.getinfo2("SELECT count(emprec.id) FROM emprec right outer JOIN emp_static_info as esi ON emprec.emp_id = esi.emp_id where  ('" & pd1 & "' between hire_date and isnull(end_date,'" & pd2 & "') ) ", Session("con"))
                Response.Write("NO:" & count)
                outp &= pd1
                outp &= "<table  cellpadding='0' cellspacing='1' style='border:1px solid black;width:800px;'><tr><td>no.</td><td>com.id</td><td>Name</td><td>Project</td><td>paid status</td></tr>"
                Dim i As Integer = 1
                Dim isres As String = ""
                While rs.Read
                    isres = ""
                    ' Response.Write("in")
                    ReDim Preserve unpaid(i)
                    unpaid(i) = ""
                    pd = fm.getinfo2("select ref from payrollx where emptid='" & rs.Item("id") & "' and date_paid='" & pd2 & "' and  (remark='monthly' or remark ='pay_inc_middle')", Session("con"))
                    isres = fm.isResign(rs.Item("id"), Session("con"))(1)

                    If color = "#4589fa" Then
                        color = "white"
                    Else
                        color = "#4589fa"
                    End If
                    If IsDate(isres) = False Then
                        isres = "1/1/1900"
                    Else
                        rtn(4) &= "'" & rs.Item("id") & "',"
                    End If
                    If CDate(pd1).Subtract(CDate(isres)).Days <> 0 Then

                        'Response.Write(pd)
                        outp &= "<tr style='background-color:" & color & ";' ><td>" & i & "</td>"
                        outp &= "<td>" & rs.Item("id") & "</td>"
                        rtn(0) &= "'" & rs.Item("id") & "',"
                        outp &= "<td>" & rs.Item("first_name") & " " & rs.Item("middle_name") & "</td>"
                        outp &= "<td>" & fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid='" & rs.Item("id") & "' and '" & pd1 & "' between date_from and isnull(date_end,'" & pd2 & "')", Session("con")) & "'", Session("con")) & "</td>"
                        If pd = "None" Then
                            unpaid(i) = "<tr style='background-color:" & color & ";' ><td>" & i & "</td>"
                            unpaid(i) &= "<td>" & rs.Item("id") & "</td>"
                            unpaid(i) &= "<td>" & rs.Item("first_name") & " " & rs.Item("middle_name") & "</td>"
                            unpaid(i) &= "<td>" & fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid='" & rs.Item("id") & "' and '" & pd1 & "' between date_from and isnull(date_end,'" & pd2 & "')", Session("con")) & "'", Session("con")) & "</td>"
                            unpaid(i) &= "<td style='background:yellow'>" & pd & "===>" & isres & "</td></tr>"
                            outp &= "<td style='background:yellow'>" & pd & " </td>"
                            rtn(3) &= "'" & rs.Item("id") & "',"
                        Else
                            outp &= "<td>" & pd & "</td>"

                        End If
                        outp &= "</tr>"
                        i = i + 1
                    End If
                End While
                outp &= "</table>"
                unpaid(0) = "<div>unpaid list<br><table  cellpadding='0' cellspacing='1' style='border:1px solid black;width:800px;'><tr><td>no.</td><td>com.id</td><td>Name</td><td>Project</td><td>paid status</td></tr>"
                For k As Integer = 1 To UBound(unpaid)
                    If unpaid(k) <> "" Then
                        unpaid(0) &= unpaid(k)
                    End If

                Next
                unpaid(0) &= "</table></div>"
                outp = unpaid(0) & outp
                rtn(1) = outp
                rtn(2) = sql & "<br>"

            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
        Return rtn
    End Function
    Public Function pchecklist(ByVal pd1 As Date, ByVal pd2 As Date, ByVal prjid As String)
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim outp As String = ""
        Dim nod As Integer

        Dim pd As String
        Dim rtn() As String = {"", "", "", "", ""}
        Dim pro() As String
        pro = prjid.Replace("'", "").Split("|")
        nod = Date.DaysInMonth(pd1.Year, pd1.Month)
        Dim color As String = "#4589fa"
        Dim sql As String = "SELECT distinct emprec.id FROM emprec INNER JOIN emp_job_assign as eja ON emprec.id=eja.emptid where  ('" & pd1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pd2 & "')) and eja.project_id='" & pro(1) & "' and '" & pd1 & "'  between eja.date_from and isnull(eja.date_end,'" & pd2 & "')" '"SELECT emprec.id, esi.first_name,esi.middle_name,emprec.active FROM emprec INNER JOIN emp_static_info as esi ON emprec.emp_id =esi.emp_id  where end_date is null or ('" & CDate(pd1).ToShortDateString & "' between hire_date and isnull(end_date,'" & CDate(pd2).ToShortDateString & "') and emprec.id not in(select emptid from emp_resign where resign_date<'" & CDate(pd1).ToShortDateString & "'))"
        Try
            '   Response.Write(sql)
            Dim unpaid() As String = {""}
            rs = dbs.dtmake("chklst", sql, Session("con"))
            If rs.HasRows Then

                outp &= pd1
                Dim i As Integer = 1
                Dim isres As String = ""
                While rs.Read
                    isres = ""
                    ' Response.Write("in")

                    pd = fm.getinfo2("select ref from payrollx where emptid='" & rs.Item("id") & "' and date_paid='" & pd2 & "' and  (remark='monthly' or remark ='pay_inc_middle')", Session("con"))
                    isres = fm.isResign(rs.Item("id"), Session("con"))(1)

                    If color = "#4589fa" Then
                        color = "white"
                    Else
                        color = "#4589fa"
                    End If
                    If IsDate(isres) = False Then
                        isres = "1/1/1900"
                    Else
                        rtn(2) &= "'" & rs.Item("id") & "'" & isres & ","
                    End If
                    If CDate(pd1).Subtract(CDate(isres)).Days <> 0 Then

                        'Response.Write(pd)

                        If pd = "None" Then

                            rtn(1) &= "'" & rs.Item("id") & "',"
                        Else
                            rtn(0) &= "'" & rs.Item("id") & "',"
                        End If


                    End If
                End While



            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try
        Return rtn
    End Function
    Public Function pchecklist_x(ByVal pd1 As Date, ByVal pd2 As Date)
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim outp As String = ""
        Dim nod As Integer

        Dim pd As String
        Dim rtn() As String = {"", "", "", "", ""}
        Dim pro() As String

        nod = Date.DaysInMonth(pd1.Year, pd1.Month)
        Dim color As String = "#4589fa"
        Dim sql As String = "SELECT distinct emprec.id FROM emprec where  ('" & pd1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pd2 & "'))" 'SELECT emprec.id, esi.first_name,esi.middle_name,emprec.active FROM emprec INNER JOIN emp_static_info as esi ON emprec.emp_id =esi.emp_id  where end_date is null or ('" & CDate(pd1).ToShortDateString & "' between hire_date and isnull(end_date,'" & CDate(pd2).ToShortDateString & "') and emprec.id not in(select emptid from emp_resign where resign_date<'" & CDate(pd1).ToShortDateString & "'))"
        Try
            '   Response.Write(sql)
            Dim unpaid() As String = {""}
            rs = dbs.dtmake("chklst", sql, Session("con"))
            If rs.HasRows Then

                outp &= pd1
                Dim i As Integer = 1
                Dim isres As String = ""
                While rs.Read
                    isres = ""
                    ' Response.Write("in")

                    pd = fm.getinfo2("select ref from payrollx where emptid='" & rs.Item("id") & "' and date_paid='" & pd2 & "' and  (remark='monthly' or remark ='pay_inc_middle')", Session("con"))
                    isres = fm.isResign(rs.Item("id"), Session("con"))(1)

                    If color = "#4589fa" Then
                        color = "white"
                    Else
                        color = "#4589fa"
                    End If
                    If IsDate(isres) = False Then
                        isres = "1/1/1900"
                    Else
                        rtn(2) &= "'" & rs.Item("id") & "'" & isres & ","
                    End If
                    If CDate(pd1).Subtract(CDate(isres)).Days <> 0 Then

                        'Response.Write(pd)

                        If pd = "None" Then

                            rtn(1) &= "'" & rs.Item("id") & "',"
                        Else
                            rtn(0) &= "'" & rs.Item("id") & "',"
                        End If


                    End If
                End While



            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try
        Response.Write(rtn(0).Replace("'", ""))
        Return rtn
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
        ' Dim dbs As New dbclass
        ' dbs.pathserver = Server.MapPath(" ")
        ' Response.Write(dbs.pathserver.ToString)

        ' Dim rtn As Object
        '  rtn = dbs.Backup(Session("con"), dbs.pathserver & "/app_data/hrmp.mdf")
        '  Response.Write(rtn.ToString)
        ' dbs = Nothing
        ' Session("con").close()
        ' Session("con").open()
    End Sub
    Public Function getprojemp(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim did As String
        rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "'>=date_from and '" & sdate & "'<= isnull(date_end,'" & sdate & "')  order by id", con)
        Dim d1, d2, de, ds As Date
        d2 = Nothing
        d1 = "#" & sdate.Month.ToString & "/1/" & sdate.Year.ToString & "#"
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

                    If rs.IsDBNull(3) Then
                        'ds = sdate

                        ds = rs.Item("date_from")
                        de = sdate

                    Else
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)
                        If IsDate(did) Then
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

    Public Function headertax(ByVal datef As Date, ByVal pno As String, ByVal tp As String) As Array
        Dim rtn() As String = {"", ""}
        Dim dtm As New dtime
        rtn(0) = "  <table border='1' bordercolor='black' cellpadding='0' cellspacing='0' style='font-size:8pt,width:29.7cm' >" & Chr(13) & _
            " <tr>" & Chr(13) & _
            "<td>" & Chr(13) & _
              "  <img src='images\jpg\evel.jpg' width='65'/></td>" & _
            "  <td colspan='6' style='font-size:13pt;font-weight:bold; text-align:center;' align='center'>" & _
               " የኢትዮጵያ ፌድራላዊ ዲሞክራሲ ሪሊክ  " & Chr(12) & " የኢትዮጵያ ገቢዎችና ጉሙሩክ....</td>" & _
            "<td  colspan='6' style='font-size:13pt;font-weight:bold; text-align:center;' align='center'>" & _
               " የሰንጠረዥ፤ሀ &nbsp;&nbsp;&nbsp;የስራ ግብር ክፍያ ማሳወቂያ ቅጽ (ለቀጣሪዎች)" & Chr(12) & _
                "(የገቢ ግብር ሀዋጅ ቁጥር 286/1996 እና ገቢ ግብር ደንብ ቁጥር 78/1994)</td>" & _
            "<td >" & _
                "<img src='images\jpg\leba.jpg' width='65px'/>" & _
            "</td> </tr>" & _
        "<tr>" & _
           " <td colspan='14' style='font-size:12pt;font-weight:bold; text-align:center;' align='center' >" & _
             "   ክፍል 1. የግብር ከፋይ ዝርዝር መረጃ</td>" & _
       " </tr>" & _
        "<tr>" & _
           " <td  colspan='4'>" & _
             "   <b>  1. የግብር ከፋይ ስም :" & Chr(12) & "</b>ኔት ኮንሰልት ኮንሰልቲንግ ኢንጂነሪንግና አርክቴክት</td>" & _
            "<td colspan='3'>" & _
                "<b> 3.የግብር ከፋይመለያ ቁጥር " & Chr(12) & "0000041336</td>" & _
            "<td colspan='3'>" & _
               " 4. የግብር ሂሳብ ቁጥር<br/></td>" & _
            "<td colspan='4'> "
        Dim dtc As String = dtm.convert_to_ethx(datef.AddDays(30))

        rtn(0) &= " 5. የክፍያ ጊዜ " & dtm.getmonth(dtc.Split(".")(1), "amh") & "," & dtc.Split(".")(2) & "</td>" & _
        "</tr>" & _
       " <tr >" & _
            "<td >" & _
               " 2. ክልል አ.አ</td>" & _
            "<td>" & _
               " 2፣ቀበሌ/ገ/ማህበር</td>" & _
            "<td >" & _
              "  &nbsp;</td>" & _
            "<td colspan='2'>" & _
                "5. ግብር ሰብሳቢ መ/ቤትስም</td>" & _
            "<td> " & _
                "&nbsp;</td>" & _
            "<td '>" & _
                "&nbsp;</td>" & _
                "<td >" & _
                "&nbsp;</td>" & _
            "<td colspan='6' > " & _
              "  page <u>" & pno & " </u> of <u>" & tp & "</u>" & _
              "</td>" & _
        "</tr>" & _
        "<tr style='height:10px;'>" & _
           " <td colspan='2' style='height:10px;'>" & _
              "  2a. ወረዳ</td> " & _
           " <td >                2. ዞነ ክ/ከተማ</td>" & _
            "<td >                2. የቤት ቁጥር</td>" & _
            "<td colspan='3'>6. ሰልክ ቁጥር</td>" & _
            "<td>               7. የፋክስ ቁጥር</td>" & _
            "<td>                የሰነድ ቁጥር(ለቢሮ አገልግሎት)</td>" & _
            "<td colspan='5'>               &nbsp;</td>" & _
        "</tr>        <tr  style='height:10px;'>" & _
                   " <td class='style23' colspan='14' style='height:15px;text-align:center' width='auto'> ሰንጠረዥ-2 ማስታወቂያ ዝርዝር መረጃ </td>" & _
        "</tr></table>        "
        rtn(1) &= "<table cellpadding='0' cellspacing='0' border='1' bordercolor='black'>" & _
                    " <tr style='height:auto;font-size:9pt; font-weight:bold;text-align:center;'>" & _
                     "    <td rowspan='2' style='width:2%'> (ሀ) ተ.ቁ</td>" & _
                        " <td rowspan='2' style='width:5%'> (ለ)የሠራተኛው የግብር<br/> ከፋይ መለያ ቁጥር(TIN)</td>" & _
                        " <td  rowspan='2' style='width:14%'>(ሐ) የሠራተኛው ስም:<br>የአባት ስም፡ የአያት ስም</td>" & _
                        " <td  rowspan='2' style='width:4%'> (መ)የተቀጠሩበት<br/>ቀን<br/> ቀን/ወር/ዓ.ም</td>" & _
                         "<td rowspan='2' style='width:4%'> (ሠ)የወር ደመወዝ<br/> /ብር/</td>" & _
                         "<td rowspan='2' style='width:4%'> (ረ) ጠቅላላ <br/>የትራንስፖርት<br/>አበል /ብር/</td>" & _
                        " <td  colspan='3' style='width:16%'>  ተጨማሪ ክፍያዎች</td>" & _
                        " <td  rowspan='2' style='width:5%'>  (በ) ጠቅላላ ግብር የሚከፈልበት ገቢ /ብር/</td>" & _
                         "<td  rowspan='2' style='width:4%'>  (ተ) የስራ ግብር/ብር/</td>" & _
                        " <td rowspan='2' style='width:2%'>  (ቸ) የትምህርት የወጪ መጋራት ክፍያ/ብር/</td>" & _
                        " <td rowspan='2' style='width:5%'>  (ነ)የተጣራ ክፍያ/ብር/</td>" & _
                         "<td  rowspan='2' style='width:7%'>    የሠራተኛ ፊርማ</td>" & _
                    " </tr><tr>" & _
                         "<td style='width:4%'>(ሰ) የስራግብር የሚከፈልበት</td>" & _
                         "<td  style='width:4%'>(ሸ) የትርፍ ሰዓት<br/>ከፍያ ብር</td>" & _
                         "<td  style='width:4%'>(ቀ) ሌሎች ጥቅማ ጥቅሞች ብር</td>" & _
                     "</tr>"
        '" <tr>"  & _
        '   " <td class='nox'>1</td>"  & _
        '   "<td class='tin' width='89'>0000000000</td>" & Chr(13) & _
        ' " <td class='ename'> Shimelis Tilahun</td>" & Chr(13) & _
        ' "<td class='edate'>&nbsp;</td>" & Chr(13) & _
        '" <td class='cssamt' width='104'>&nbsp; </td>" & Chr(13) & _
        '" <td class='cssamt'> &nbsp; </td>" & Chr(13) & _
        ' "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        ' "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        ' "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        '      "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        '     "<td class='cssamt'>&nbsp;</td>" & Chr(13) & _
        '    "<td class='cssamt'>&nbsp;</td>" & Chr(13) & _
        '   "<td class='cssamt'>&nbsp;</td>" & Chr(13) & _
        '  "<td class='csssign'> &nbsp;</td>" & Chr(13) & _
        '              "</tr>" & Chr(13) & _
        '        " </table>" & Chr(13) & _
        '     " </td></tr>" & Chr(13) & _
        '"</table>"
        Return rtn
    End Function
    Public Function headertax_first(ByVal datef As Date, ByVal pno As String, ByVal tp As String) As Array
        Dim rtn() As String = {"", ""}
        Dim dtm As New dtime
        rtn(0) = "  <table border='1' bordercolor='black' cellpadding='0' cellspacing='0' style='font-size:8pt,width:29.7cm' >" & Chr(13) & _
            " <tr style='height:10px;'>" & Chr(13) & _
            "<td class='' style='height:10px;' >" & Chr(13) & _
              "  <img src='images\jpg\evel.jpg' width='65'/></td>" & Chr(13) & _
            "  <td class='style2' colspan='4' width='454' style='font-size:10pt;font-weight:bold;'>" & Chr(13) & _
               " የኢትዮጵያ ፌድራላዊ ዲሞክራሲ ሪሊክ  <br/> የኢትዮጵያ ገቢዎችና ጉሙሩክ....</td>" & Chr(13) & _
            "<td class='' colspan='3' width='736' style='font-size:10pt;font-weight:bold;'>" & Chr(13) & _
               " የሰንጠረዥ፤ሀ &nbsp;&nbsp;&nbsp;የስራ ግብር ክፍያ ማሳወቂያ ቅጽ (ለቀጣሪዎች)" & Chr(13) & _
                "(የገቢ ግብር ሀዋጅ ቁጥር 286/1996 እና ገቢ ግብር ደንብ ቁጥር 78/1994)</td>" & Chr(13) & _
            "<td class='style1x' align='left' style='width:90pt' valign='top' width='120'>" & Chr(13) & _
                "<img src='images\jpg\leba.jpg' width='65px'/>" & Chr(13) & _
            "</td> </tr>" & Chr(13) & _
        "<tr style='height:10px;'>" & Chr(13) & _
           " <td class='style5' colspan='9' >" & Chr(13) & _
             "   ክፍል 1. የግብር ከፋይ ዝርዝር መረጃ</td>" & Chr(13) & _
       " </tr>" & Chr(13) & _
        "<tr style='height:10px;'>" & Chr(13) & _
           " <td class='style6' colspan='4' width='388'>" & Chr(13) & _
             "   <b>  1. የግብር ከፋይ ስም :</b><br/>ኔት ኮንሰልት ኮንሰልቲንግ ኢንጂነሪንግና አርክቴክት</td>" & Chr(13) & _
            "<td class='style7' colspan='2'>" & Chr(13) & _
                "<b> 3.የግብር ከፋይመለያ ቁጥር </b><br/>0000041336</td>" & Chr(13) & _
            "<td class='style9'>" & Chr(13) & _
               " 4. የግብር ሂሳብ ቁጥር<br/></td>" & Chr(13) & _
            "<td class='style10' colspan='2'> "
        Dim dtc As String = dtm.convert_to_ethx(datef.AddDays(30))

        rtn(0) &= " 5. የክፍያ ጊዜ " & dtm.getmonth(dtc.Split(".")(1), "amh") & "," & dtc.Split(".")(2) & "</td>" & Chr(13) & _
        "</tr>" & Chr(13) & _
       " <tr style='mso-height-source:userset;height:10px'>" & Chr(13) & _
            "<td class='style42'>" & Chr(13) & _
               " 2. ክልል አ.አ</td>" & Chr(13) & _
            "<td class='style14'>" & Chr(13) & _
               " 2፣ቀበሌ/ገ/ማህበር</td>" & Chr(13) & _
            "<td class='style13'>" & Chr(13) & _
              "  &nbsp;</td>" & Chr(13) & _
            "<td class='style7' colspan='2'>" & Chr(13) & _
                "<span style='mso-spacerun:yes'>&nbsp;</span>5. ግብር ሰብሳቢ መ/ቤትስም</td>" & Chr(13) & _
            "<td class='style16'> " & Chr(13) & _
                "&nbsp;</td>" & Chr(13) & _
            "<td class='style16'>" & Chr(13) & _
                "&nbsp;</td>" & Chr(13) & _
            "<td class='style17'> " & Chr(13) & _
              "  page <u>" & pno & " </u> of <u>" & tp & "</u>" & Chr(13) & _
              "</td>" & Chr(13) & _
        "</tr>" & Chr(13) & _
        "<tr style='height:10px;'>" & Chr(13) & _
           " <td class='style18' colspan='2' style='height:10px;'>" & Chr(13) & _
              "  2a. ወረዳ</td> " & Chr(13) & _
           " <td class='style19'>                2. ዞነ ክ/ከተማ</td>" & Chr(13) & _
            "<td class='style19'>                2. የቤት ቁጥር</td>" & Chr(13) & _
            "<td class='style7' colspan='2'>6. ሰልክ ቁጥር</td>" & Chr(13) & _
            "<td class='style21'>               7. የፋክስ ቁጥር</td>" & Chr(13) & _
            "<td class='style16'>                የሰነድ ቁጥር(ለቢሮ አገልግሎት)</td>" & Chr(13) & _
            "<td class='style22'>               &nbsp;</td>" & Chr(13) & _
        "</tr>        <tr  style='height:10px;'>" & Chr(13) & _
                   " <td class='style23' colspan='9' style='height:15px;' width='auto'> ሰንጠረዥ-2 ማስታወቂያ ዝርዝር መረጃ </td>" & Chr(13) & _
        "</tr></table>        "
        rtn(1) &= "<table cellpadding='0' cellspacing='0' border='1' bordercolor='black'>" & Chr(13) & _
                    " <tr style=' font-weight:bold;text-align:center; font-size:9pt;'>" & Chr(13) & _
                     "    <td  rowspan='2' style='width:2%'> (ሀ) ተ.ቁ</td>" & Chr(13) & _
                        " <td class='style25' rowspan='2' style='width:5%'> (ለ)የሠራተኛው የግብር<br/> ከፋይ መለያ ቁጥር(TIN)</td>" & Chr(13) & _
                        " <td class='style26' rowspan='2' style='width:14%'>(ሐ) የሠራተኛው ስም:<br>የአባት ስም፡ የአያት ስም</td>" & Chr(13) & _
                        " <td class='style43' rowspan='2' style='width:4%'> (መ)የተቀጠሩበት<br/>ቀን<br/> ቀን/ወር/ዓ.ም</td>" & Chr(13) & _
                         "<td class='style28' rowspan='2' style='width:4%'> (ሠ)የወር ደመወዝ<br/> /ብር/</td>" & Chr(13) & _
                         "<td class='style45' rowspan='2' style='width:4%'> (ረ) ጠቅላላ <br/>የትራንስፖርት<br/>አበል /ብር/</td>" & Chr(13) & _
                        " <td class='style30' colspan='3' style='width:16%'>  ተጨማሪ ክፍያዎች</td>" & Chr(13) & _
                        " <td class='style53' rowspan='2' style='width:5%'>  (በ) ጠቅላላ ግብር የሚከፈልበት ገቢ /ብር/</td>" & Chr(13) & _
                         "<td class='style32' rowspan='2' style='width:4%'>  (ተ) የስራ ግብር/ብር/</td>" & Chr(13) & _
                        " <td class='style55' rowspan='2' style='width:2%'>  (ቸ) የትምህርት የወጪ መጋራት ክፍያ/ብር/</td>" & Chr(13) & _
                        " <td class='style57' rowspan='2' style='width:5%'>  (ነ)የተጣራ ክፍያ/ብር/</td>" & Chr(13) & _
                         "<td class='style59' rowspan='2' style='width:7%'>    የሠራተኛ ፊርማ</td>" & Chr(13) & _
                    " </tr><tr>" & Chr(13) & _
                         "<td class='style47' style='width:4%'>(ሰ) የስራግብር የሚከፈልበት</td>" & Chr(13) & _
                         "<td class='style49' style='width:4%'>(ሸ) የትርፍ ሰዓት<br/>ከፍያ ብር</td>" & Chr(13) & _
                         "<td class='style51' style='width:4%'>(ቀ) ሌሎች ጥቅማ ጥቅሞች ብር</td>" & Chr(13) & _
                     "</tr>"
        '" <tr>" & Chr(13) & _
        '   " <td class='nox'>1</td>" & Chr(13) & _
        '   "<td class='tin' width='89'>0000000000</td>" & Chr(13) & _
        ' " <td class='ename'> Shimelis Tilahun</td>" & Chr(13) & _
        ' "<td class='edate'>&nbsp;</td>" & Chr(13) & _
        '" <td class='cssamt' width='104'>&nbsp; </td>" & Chr(13) & _
        '" <td class='cssamt'> &nbsp; </td>" & Chr(13) & _
        ' "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        ' "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        ' "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        '      "<td class='cssamt'> &nbsp;</td>" & Chr(13) & _
        '     "<td class='cssamt'>&nbsp;</td>" & Chr(13) & _
        '    "<td class='cssamt'>&nbsp;</td>" & Chr(13) & _
        '   "<td class='cssamt'>&nbsp;</td>" & Chr(13) & _
        '  "<td class='csssign'> &nbsp;</td>" & Chr(13) & _
        '              "</tr>" & Chr(13) & _
        '        " </table>" & Chr(13) & _
        '     " </td></tr>" & Chr(13) & _
        '"</table>"
        Return rtn
    End Function
    Public Function headertax2(ByVal datef As Date, ByVal pno As String, ByVal tp As String) As Array
        Dim rtn() As String = {"", ""}
        Dim dtm As New dtime
        rtn(0) = "  <table border='0' cellpadding='0' cellspacing='0' style='font-size:8pt,width:29.7cm' >" & _
" <tr style='height:10px;'>" & _
 "    <td style='height:10px;'>" & _
  "       <img src='images\jpg\evel.jpg' width='65pt'/></td>" & _
   "  <td class='h1' colspan='5'" & _
    "     style='font-size:10pt;font-weight:bold; text-align: center;'>" & _
 "የኢትዮጵያ ፌድራላዊ ዲሞክራሲ ሪሊክ  <br/> የኢትዮጵያ ገቢዎችና ጉሙሩክ</td>" & _
  "   <td class='h2' colspan='2'" & _
   "      style='font-size:10pt;font-weight:bold; text-align: center;'>" & _
" የሰንጠረዥ፤ሀ &nbsp;&nbsp;&nbsp;የስራ ግብር ክፍያ ማሳወቂያ ቅጽ (ለቀጣሪዎች)" & _
"(የገቢ ግብር ሀዋጅ ቁጥር 286/1996 እና ገቢ ግብር ደንብ ቁጥር 78/1994)</td>" & _
"     <td align='left' style='width:90pt' valign='top'>" & _
   "      <img src='images\jpg\leba.jpg' width='65px'/>" & _
    " </td></tr>" & _
"<tr style='height:10px;'>" & _
 "<td class='h3' colspan='9' >" & _
  " ክፍል 1. የግብር ከፋይ ዝርዝር መረጃ</td>" & _
 "</tr>" & _
"<tr style='height:10px;'>" & _
 "<td class='h4' colspan='4'>" & _
  "   <span class='style1'>1. የግብር ከፋይ ስም :</span><br/>ኔት ኮንሰልት ኮንሰልቲንግ ኢንጂነሪንግና አርክቴክት</td>" & _
"<td class='h5' colspan='2'>" & _
 "   <span class='style1,>3.የግብር ከፋይመለያ ቁጥር </span><br/>0000041336</td>" & _
"<td class='style3' style='vertical-align:top;'>" & _
 "   <strong>4. የግብር ሂሳብ ቁጥር</strong></td>"
        Dim dtc As String = dtm.convert_to_ethx(datef.AddDays(30))
        rtn(0) &= "<td class='h7' colspan='2'>  5. የክፍያ ጊዜ:" & dtm.getmonth(dtc.Split(".")(1), "amh") & "," & dtc.Split(".")(2) & " </td>" & _
"</tr>" & _
 "<tr style='height:10px'>" & _
"<td class='h8'>" & _
"2:      .ክልል" & _
 "   <br />" & _
  "  አ.አ</td>" & _
"<td class='h9'>" & _
"  2፣ቀበሌ/ገ/ማህበር</td>" & _
"<td class='style5'>    &nbsp;</td>" & _
"<td class='style4' colspan='3'>5. ግብር ሰብሳቢ መ/ቤትስም</td>" & _
"<td class='style17' colspan='3'> page " & pno & " </u> of <u>" & tp & "</u>" & Chr(13) & "</td>" & _
"</tr>" & _
"<tr style='height:10px;'> <td class='style18' colspan='2'>  2a. ወረዳ</td> " & _
 "<td class='style6'>                2. ዞነ ክ/ከተማ</td>" & _
"<td class='style19'>                2. የቤት ቁጥር</td>" & _
"<td class='style20'>6. ሰልክ ቁጥር</td>" & _
"<td class='style2'><b><span class='style22'>7. የፋክስ ቁጥር</span>" & _
 "   </b></td>" & _
"<td class='style1' colspan='3'>                የሰነድ ቁጥር(ለቢሮ አገልግሎት)</td>" & _
"</tr>        <tr  style='height:10px;'>" & _
 "<td class='style23' colspan='14'" & _
    " style='height:15px; text-align: center; font-weight: 700;' width='auto'> ሰንጠረዥ-2 ማስታወቂያ ዝርዝር መረጃ </td>" & _
"</tr></table>       "

        rtn(1) = " <table cellpadding='0' cellspacing='0'>" & _
 "<tr style='height:15px;'>" & _
 "   <td class='style24' rowspan='2' style='width:2%'> (ሀ) ተ.ቁ</td>" & _
" <td class='style24' rowspan='2' style='width:5%'><span class='style25'>(ለ)የሠራተኛው የግብር</span>" & _
"<span class='style25'>ከፋይ መለያ ቁጥር(TIN)</span></td>" & _
" <td class='style24' rowspan='2' style='width:14%'><span class='style25'>(ሐ) የሠራተኛው ስም:</span><span class='style25'>የአባት ስም፡ የአያት ስም</span></td>" & _
 "<td class='style26' rowspan='2' style='width:4%'><span class='style25'>(መ)የተቀጠሩበት</span><br" & _
   "  class='style25'/><span class='style25'>ቀን</span><br class='style25/>" & _
     "<span class='style25'>ቀን/ወር/ዓ.ም</span></td>" & _
"<td class='style26' rowspan='2' style='width:4%'><span class='style25'>(ሠ)የወር ደመወዝ</span><br" & _
   " class='style25'/><span class='style25'>/ብር/</span></td>" & _
"<td class='style26' rowspan='2' style='width:4%'><span class='style25'>(ረ) ጠቅላላ " & _
   " </span><br class='style25'/><span class='style25'>የትራንስፖርት</span><br" & _
        "class='style25'/><span class='style25'>አበል /ብር/</span></td>" & _
 "<td class='style24' colspan='3' style='width:16%'>  ተጨማሪ ክፍያዎች</td>" & _
 "<td class='style24' rowspan='2' style='width:5%'>  (በ) ጠቅላላ ግብር የሚከፈልበት ገቢ /ብር/</td>" & _
"<td class='style24' rowspan='2' style='width:4%'>  (ተ) የስራ ግብር/ብር/</td>" & _
 "<td class='style24' rowspan='2' style='width:2%'>  (ቸ) የትምህርት የወጪ መጋራት ክፍያ/ብር/</td>" & _
 "<td class='style24' rowspan='2' style='width:5%'>  (ነ)የተጣራ ክፍያ/ብር/</td>" & _
"<td class='style24' rowspan='2' style='width:7%'>    የሠራተኛ ፊርማ</td>" & _
" </tr><tr>" & _
"<td class='style24' style='width:4%'>(ሰ) የስራግብር የሚከፈልበት</td>" & _
"<td class='style26' style='width:4%'><span class='style25'>(ሸ) የትርፍ ሰዓት</span><br" & _
   " class='style25'/><span class='style25'>ከፍያ ብር</span></td>" & _
"<td class='style24' style='width:4%'>(ቀ) ሌሎች ጥቅማ ጥቅሞች ብር</td>" & _
"</tr>"
        Return rtn
    End Function
    Public Function headerx(ByVal datef As Date, ByVal pc As String, ByVal pe As String) As String
        Dim rtn As String
        Dim dtm As New dtime
        rtn = " <table border='0' cellpadding='0' cellspacing='0' style='border-collapse:" & Chr(13) & _
 "collapse;width:28cm;'>" & Chr(13)

        rtn &= "<tr height='21' style='height:15.75px'>" & Chr(13) & _
         "   <td class='style1' colspan='4' height='21' style='font-size:10pt; font-weight:bold;width:44%'>" & Chr(13) & _
          "      የኢትዮጵያ ፌደራላዊ ዲሞክራሲያዊ ሪፐብሊክ</td>" & Chr(13) & _
           " <td class='style2' colspan='5' style='font-size:10pt; font-weight:bold;width:55%'>" & Chr(13) & Chr(13) & _
            "    የግል ድርጅት የጡረታ መዋጮ ማሳወቂያ ቅጽ</td>" & Chr(13) & _
        "</tr>" & Chr(13) & Chr(13) & _
        "<tr>"
        rtn &= "   <td class='style3' colspan='4' width='467'  style='font-size:10pt; font-weight:bold;'>" & Chr(13) & _
           "      የኢትዮጵያ ገቢዎችና ጉምሩክ ባለስልጣን</td>" & Chr(13) & Chr(13) & _
            " <td class='style4' colspan='5'  style='font-size:10pt; font-weight:bold;'>" & Chr(13) & _
             "    (በግል ድርጅቶች ሠራተኞች ጡረ አዋጅ ቁጥር 715/2003)</td>" & Chr(13) & Chr(13) & _
                 "</tr>" & Chr(13) & _
         "<tr height='21' style='height:15.75px'>" & Chr(13) & Chr(13) & _
          "   <td class='style6' height='21'>" & Chr(13) & _
           "      &nbsp;</td>" & Chr(13) & Chr(13) & _
            " <td class='style7'>" & Chr(13) & Chr(13) & _
             "    &nbsp;</td>" & Chr(13) & Chr(13) & _
             "<td class='style8'>" & Chr(13) & Chr(13) & _
              "   &nbsp;</td>" & Chr(13) & Chr(13) & _
             "<td class='style7'>" & Chr(13) & _
              "   &nbsp;</td>" & Chr(13) & Chr(13) & _
             "<td class='style9' colspan='5'>" & Chr(13) & _
              "   ቅጽ ቁጥር 2/2003 የተጨማሪ ቅጽ</td>" & Chr(13) & Chr(13) & _
         "        </tr>" & Chr(13)
        rtn &= "<tr height='22' style='height:16.5px'>" & Chr(13) & _
          "   <td class='style11' colspan='9' height='22'  style='font-size:10pt; font-weight:bold;'> " & Chr(13) & Chr(13) & _
           "     ክፍል -1- " & Chr(13) & _
             "        የጡረታ መዋጮውን የሚከፍለው ድርጅት ዝርዝር መረጃ" & Chr(13) & _
             "</td>" & Chr(13) & Chr(13) & _
         "</tr>" & Chr(13) & Chr(13) & _
         "<tr class='cinfo' height='21' style='height:15.75px'  style='font-size:9pt; font-weight:bold;'>" & Chr(13) & Chr(13) & _
          "   <td class='style14' colspan='5' height='42' rowspan='2'  style='font-size:9pt; font-weight:bold;'>" & Chr(13) & _
           "     1. የድርጅቱ ስም: " & Session("company_name_amharic") & "</td>" & Chr(13) & _
            " <td class='style16' height='21' colspan='2' rowspan='2' width='246'  style='font-size:10pt; font-weight:bold;'>" & Chr(13) & Chr(13) & _
             "    2. የድርጅቱ የግብር ከፋይ መለያ ቁጥር <br />&nbsp;" & Chr(13) & _
               Session("tin") & "</td>" & Chr(13) & Chr(13) & _
             "<td class='stylex'  style='font-size:9pt; font-weight:bold;'>" & Chr(13) & _
              "   3. የክፍያ ጊዜ </td>" & Chr(13) & Chr(13) & _
             "<td class='style15' rowspan='2'>" & Chr(13) & _
              "   &nbsp;</td>" & Chr(13) & Chr(13) & _
         "</tr>" & Chr(13)
        Dim dtc As String = dtm.convert_to_ethx(datef.AddDays(30))

        rtn &= "<tr height='21' style='height:15.75px;font-size:9pt; font-weight:bold;'>" & Chr(13) & _
          "   <td class='style2x' height='21'>" & Chr(13) & _
           "      ወር&nbsp;&nbsp;" & dtm.getmonth(dtc.Split(".")(1), "amh") & "," & dtc.Split(".")(2) & "</td>" & Chr(13) & _
         "</tr>" & Chr(13) & Chr(13) & _
         "<tr  style='font-size:10pt; font-weight:bold;'>" & Chr(13) & Chr(13) & _
         "                    <td class='style11' colspan='9' >" & Chr(13) & _
    "                    ሠንጠረዥ - 2 ማስወቂያ ዝርዝር መረጃ" & Chr(13) & _
    "                 </td>" & Chr(13) & Chr(13) & _
              "       </tr>" & Chr(13)
        rtn &= "      <tr  style='font-size:8pt; font-weight:bold;height:15.75px'>" & Chr(13) & Chr(13) & _
                "         <td  style='font-size:8pt; font-weight:bold;width:2%;border:1px solid black;border-left:2pt double black;border-top:2pt double black;'>" & Chr(13) & Chr(13) & _
        "ሀ) ተ.ቁ" & Chr(13) & _
        "                 </td><td class='stylex' style='width:4%;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;font-size:8pt;" & Chr(13) & _
       "height:15.75px'>" & Chr(13) & Chr(13) & _
        "                   ለ)የሠራተኛው የግብር <br>ከፋይ መለያ ቁጥር(TIN)" & Chr(13) & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style='width:220px;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;" & Chr(13) & _
       "height:15.75px;font-weight:700;text-align:center' >" & Chr(13) & _
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ስም" & Chr(13) & _
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" & Chr(13) & _
         "                <td  class='stylex' style='width:5%;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;" & Chr(13) & _
       "height:15.75px'>" & Chr(13) & _
        "                    መ)የተቀጠሩበት<br> ቀን ቀን/ወር/ዓ.ም" & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style='width:5%;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;" & Chr(13) & _
       "height:15.75px'>" & Chr(13) & _
        "                    ሠ)የወር ደመወዝ<br>/ብር/" & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style='width:7%;font_size:8pt;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;" & Chr(13) & _
       "height:15.75px' >" & Chr(13) & _
        "                    ረ) የሰራተኛው መዋጮ <br>መጠን <br>" & pe & "%/ብር/" & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style='width:7%;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;" & Chr(13) & _
       "height:15.75px'>" & Chr(13) & _
        "                     ሰ)የአሰሪው መዋጮ <br>መጠን <br>" & pc & "% /ብር" & Chr(13) & _
           "                 </td>" & Chr(13) & _
            "                <td class='stylex' style='width:5%;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;" & Chr(13) & _
       "height:15.75px'>"

        If IsNumeric(pc) = False Then
            pc = 0
        End If
        If IsNumeric(pe) = False Then
            pe = "0"
        End If
        Dim sum As Double
        sum = CDbl(pc) + CDbl(pe)
        rtn &= "ሸ) በአሰሪው የሚገባ <br>ጥቅል መዋጮ </br>" & Chr(13) & _
          (CInt(pc) + CInt(pe)).ToString & "%<br> /ብር/(ረ+ሰ)" & Chr(13) & _
                      "               </td>" & Chr(13) & _
             "              <td class='style2' style='width:15%;font-size:8pt;border-top:double windowtext 2.25pt;" & Chr(13) & _
       "border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;" & Chr(13) & _
       "mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;" & Chr(13) & _
       "mso-border-top-alt:double windowtext 2.25pt;border-right:double windowtext 2.25pt;padding:0cm 5.4pt 0cm 5.4pt;" & Chr(13) & _
       "height:15.75px'>" & Chr(13) & _
           "                  የሠራተኛ ፊርማ  " & Chr(13) & _
           "                 </td>" & Chr(13) & _
            "      </tr>"

        Return rtn
    End Function


    Function makepage_all_tax(ByRef ref As String, ByRef pd1 As Date, ByRef pd2 As Date)
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtime
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        ' Dim pstamt, psumtax, tpstamt, tpsumtax As Double

        Dim sumpen As Double
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        Dim j As Integer
        ' Response.Write(ref)
        Dim increament As String
        Dim fixed As Integer
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        Dim trow As Integer
        Dim ot_back As String
        Dim taxsum, tincome, talw As String
        Dim basic As String
        Dim pno, tpg As Integer
        rs = dbs.dtmake("vwpayroll", "select * from payrollx where ref='" & ref & "'", Session("con"))
        If rs.HasRows Then
            trow = fm.getinfo2("select count(id) from payrollx where ref='" & ref & "'", Session("con"))
            Dim tfixed As Integer = 13
            tpg = Math.Ceiling((CInt(trow) - tfixed) / fixed)
            tpg = tpg + 1
            pno = 1
            ' outp(0) = headertax(month.ToString & "/1/" & year.ToString, pno.ToString, tpg.ToString)
            outp(1) &= "<div class='page'>"


            While rs.Read
                Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                'Response.Write(empid)
                If i = 1 Then
                    outp(1) &= "<div class='subpage'>page" & pno.ToString & "<br>" & headertax(CDate(pd1), pno.ToString, tpg.ToString)(0) & headertax(pd1, pno.ToString, tpg.ToString)(1)
                Else
                    If i Mod tfixed = 0 Then
                        tfixed = fixed
                        outp(1) &= "</table></div>"
                        pno += 1
                        outp(1) &= "<div class='subpage'>" & i & "page" & pno.ToString & "<br>" & headertax(pd1, pno.ToString, tpg.ToString)(1)

                    End If
                End If
                outp(1) &= " <tr>"
                outp(1) &= " <td class='nox'>" & i.ToString & "</td>" & Chr(13)
                cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))
                If cell(1) = "None" Then
                    cell(1) = "&nbsp;"
                End If
                outp(1) &= "<td class='tin' width='89'>&nbsp;" & cell(1) & "</td>" & Chr(13)
                If empid = "None" Then
                    cell(2) = "&nbsp;"
                Else
                    cell(2) = fm.getfullname(empid, Session("con"))
                End If

                outp(1) &= " <td> " & cell(2) & "</td>" & Chr(13)
                outp(1) &= "<td class='edate'>" & dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid"), Session("con"))).ToShortDateString) & "</td>" & Chr(13)
                outp(1) &= " <td class='cssamt' width='104'>" & rs.Item("b_e") & "</td>" & Chr(13)
                outp(1) &= " <td class='cssamt'> &nbsp; " & rs.Item("alw") & " </td>"
                If IsNumeric(rs.Item("talw")) Then
                    talw = CDbl(rs.Item("talw")).ToString
                Else
                    talw = "0"
                End If


                outp(1) &= "<td class='cssamt'> &nbsp;" & talw & "</td>" & Chr(13)
                ot_back = fm.getinfo2("select sum(ot) from payrollx where remark='OT-Payment' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                If IsNumeric(ot_back) Then
                    If IsNumeric(rs.Item("ot")) Then
                        ot_back = CDbl(ot_back) + CDbl(rs.Item("ot"))

                    End If
                Else
                    If IsNumeric(rs.Item("ot")) Then
                        ot_back = CDbl(rs.Item("ot"))
                    Else
                        ot_back = 0
                    End If
                End If
                outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(ot_back, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                increament = fm.getinfo2("select sum(txinco) from payrollx where remark='Increament' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                If IsNumeric(increament) = False Then
                    increament = "0"
                End If

                outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(increament, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                basic = fm.getinfo2("select b_e from payrollx where remark='monthly' and pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                If IsNumeric(basic) Then
                    tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw) + CDbl(basic)).ToString
                Else
                    tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw)).ToString
                End If
                outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(tincome, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                taxsum = fm.getinfo2("select sum(tax) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                If IsNumeric(taxsum) Then
                Else
                    taxsum = "0"
                End If
                outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                outp(1) &= "<td class='cssamt'>&nbsp;</td>" & Chr(13)
                taxsum = fm.getinfo2("select sum(netp) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                If IsNumeric(taxsum) = False Then
                    Response.Write(taxsum)
                    taxsum = "0"
                End If
                outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                outp(1) &= "<td class='csssign'> &nbsp;(" & rs.Item("emptid") & ")</td>" & Chr(13)
                outp(1) &= "</tr>"
                i = i + 1
            End While
            outp(0) &= outp(1) & "</table>" & Chr(13) & "</div>" & Chr(13)
        End If
        Return outp(0)
    End Function
    Function makepage_secx_tax(ByRef ref As String, ByRef pd1 As Date, ByRef pd2 As Date)
        Dim outp(2) As String


        Dim dtm As New dtime
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        Dim pstamt, psumtax, tpstamt, tpsumtax As Double
        psumtax = 0
        pstamt = 0
        tpstamt = pstamt
        tpsumtax = 0

        Dim sumpen As Double
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        '        Dim j As Integer
        ' Response.Write(ref)
        Dim increament As String
        Dim fixed As Integer
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        Dim trow As Integer
        Dim ot_back As String
        Dim taxsum, tincome, talw As String
        Dim basic As String
        Dim pno, tpg As Integer
        Dim reff As String
        Dim refx() As String
        refx = ref.Split("|")
        ref = ref.Replace("|", ",")
        reff = ref
        ref = ""
        If refx.Length > 1 Then
            For p As Integer = 0 To refx.Length - 1
                '  Response.Write("<br>" & refx(p))
                ref &= "'" & refx(p) & "',"
            Next
        Else
            ref = "'" & ref & "',"
        End If
        ' Response.Write("<br>" & ref)
        ref = "ref in(" & ref.Substring(0, ref.Length - 1) & ")"
        Try

            Dim tempf As Integer

            rs = dbs.dtmake("vwpayroll", "select * from payrollx where " & ref, Session("con"))
            If rs.HasRows Then
                trow = fm.getinfo2("select count(id) from payrollx where " & ref, Session("con"))
                Dim tfixed As Integer = 13
                ' tpg = Math.Ceiling((CInt(trow) - tfixed) / fixed)
                tpg = tpg + 1
                pno = 1
                While tpg Mod fixed > 3
                    fixed -= 1
                End While
                If IsNumeric(tpg) Then
                    ' Response.Write(pgof)
                    tpg = Math.Ceiling((CInt(tpg) / CInt(fixed))).ToString

                End If

                If fixed > 17 Then
                    tempf = 15
                Else
                    tempf = fixed
                End If
                ' outp(0) = headertax(month.ToString & "/1/" & year.ToString, pno.ToString, tpg.ToString)
                outp(1) &= "<div class='page'>"


                While rs.Read
                    Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                    'Response.Write(empid)
                    If i = 1 Then
                        outp(1) &= "<div class='subpage'>page" & pno.ToString & "<br>" & headertax(CDate(pd1), pno.ToString, tpg.ToString)(0) & headertax(pd1, pno.ToString, tpg.ToString)(1)
                    Else
                        If i Mod tfixed = 0 Then
                            tfixed = fixed
                            outp(1) &= "</table></div>"
                            pno += 1
                            outp(1) &= "<div class='subpage'>" & i & "page" & pno.ToString & "<br>" & headertax(pd1, pno.ToString, tpg.ToString)(1)

                        End If
                    End If
                    outp(1) &= " <tr>"
                    outp(1) &= " <td class='nox'>" & i.ToString & "</td>" & Chr(13)
                    cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))
                    If cell(1) = "None" Then
                        cell(1) = "&nbsp;"
                    End If
                    outp(1) &= "<td class='tin' width='89'>&nbsp;" & cell(1) & "</td>" & Chr(13)
                    If empid = "None" Then
                        cell(2) = "&nbsp;"
                    Else
                        cell(2) = fm.getfullname(empid, Session("con"))
                    End If

                    outp(1) &= " <td class='ename'> " & cell(2) & "</td>" & Chr(13)
                    outp(1) &= "<td class='edate'>" & dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid"), Session("con"))).ToShortDateString) & "</td>" & Chr(13)
                    outp(1) &= " <td class='cssamt' width='104'>" & rs.Item("b_e") & "</td>" & Chr(13)
                    outp(1) &= " <td class='cssamt'> &nbsp; " & rs.Item("alw") & " </td>"
                    If IsNumeric(rs.Item("talw")) Then
                        talw = CDbl(rs.Item("talw")).ToString
                    Else
                        talw = "0"
                    End If


                    outp(1) &= "<td class='cssamt'> &nbsp;" & talw & "</td>" & Chr(13)
                    ot_back = fm.getinfo2("select sum(ot) from payrollx where remark='OT-Payment' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(ot_back) Then
                        If IsNumeric(rs.Item("ot")) Then
                            ot_back = CDbl(ot_back) + CDbl(rs.Item("ot"))

                        End If
                    Else
                        If IsNumeric(rs.Item("ot")) Then
                            ot_back = CDbl(rs.Item("ot"))
                        Else
                            ot_back = 0
                        End If
                    End If
                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(ot_back, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    increament = fm.getinfo2("select sum(txinco) from payrollx where remark='Increament' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(increament) = False Then
                        increament = "0"
                    End If

                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(increament, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    basic = fm.getinfo2("select b_e from payrollx where remark='monthly' and pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(basic) Then
                        tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw) + CDbl(basic)).ToString
                    Else
                        tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw)).ToString
                    End If
                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(tincome, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    taxsum = fm.getinfo2("select sum(tax) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(taxsum) Then
                    Else
                        taxsum = "0"
                    End If
                    outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= "<td class='cssamt'>&nbsp;</td>" & Chr(13)
                    taxsum = fm.getinfo2("select sum(netp) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(taxsum) = False Then
                        Response.Write(taxsum)
                        taxsum = "0"
                    End If
                    outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= "<td class='csssign'> &nbsp;(" & rs.Item("emptid") & ")</td>" & Chr(13)
                    outp(1) &= "</tr>"
                    i = i + 1
                End While
                outp(0) &= outp(1) & "</table>" & Chr(13) & "</div>" & Chr(13)
            End If
        Catch ex As Exception
            Response.Write(ref)
        End Try
        Return outp(0)
    End Function


    Function makepage_tot_tax_excel(ByVal ref As String, ByVal pd1 As Date, ByVal pd2 As Date) 'export to excel
        ' Response.Write(ref)
        Dim outp(2) As String

        Dim dtm As New dtime
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        '  Dim pstamt, psumtax, tpstamt, tpsumtax As Double
        ' Dim pd1, pd2 As String
        ' pd1 = month & "/1/" & year
        'pd2 = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        Dim sumpen As Double
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim i As Integer = 1
        Dim j As Integer
        ' Response.Write(ref)
        Dim increament As String
        Dim fixed As Integer

        Dim trow As Integer
        Dim ot_back As String
        Dim taxsum, tincome, talw As String
        Dim basic As String
        Dim pno, tpg As Integer
        Dim sql As String = ""
        Dim color As String = "white"
        Dim pgof As String = ""
        If File.Exists("c:\temp\consttax.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        ' count = 0
        Dim pageno As Integer = 1
        ' Dim refx As String
        Dim tempf As Integer
        Dim t As Integer
        Dim rlist As String = ""
        ref = ref.Substring(0, ref.Length - 1)
        sql = "select count(id) from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')"
        Dim title As String = ""
        pgof = fm.getinfo2(sql, Session("con"))
        Dim nod, nods As Integer
        Try

            While pgof Mod fixed > 3
                fixed -= 1
            End While
            If IsNumeric(pgof) Then
                ' Response.Write(pgof)
                pgof = Math.Ceiling((CInt(pgof) / CInt(fixed))).ToString

            End If

            If fixed > 17 Then
                tempf = 15
            Else
                tempf = fixed
            End If
            Response.Write(fixed)
            nod = Date.DaysInMonth(CDate(pd1).Year, CDate(pd2).Month)

            ' trow = fm.getinfo2("select count(id) from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))
            ' tpg = CInt(trow) / fixed
            rs = dbs.dtmake("vwpayroll", "select * from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))
            '  Response.Write("select distnict emptid from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')")

            If rs.HasRows Then

                pno = 1
                ' outp(0) = headertax(month.ToString & "/1/" & year.ToString, pno.ToString, tpg.ToString)
                '  outp(1) &= "<div class='page'>"

                While rs.Read
                    title = ""
                    color = ""
                    Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                    ' Response.Write(rs.Item("emptid") & "<br>")
                    If i = 1 Then
                        outp(1) &= "<table class='subpage'><tr><td>" & headertax(pd1, pno.ToString, pgof.ToString)(0) & headertax(pd1, pno.ToString, pgof.ToString)(1)
                    Else
                        If i Mod tempf = 0 Then




                            '   Response.Write(tempf.ToString & "<br>")

                            outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            For t = 0 To 9
                                coltotal(t) = colsum(t) + coltra(t)
                            Next
                            outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr></table></td></tr></table><br>"
                            pageno += 1
                            outp(1) &= "<table class='subpage'><tr><td>" & headertax(pd1, pno.ToString, tpg.ToString)(1)
                            For t = 0 To colsum.Length - 1
                                coltra(t) = coltotal(t)
                                colsum(t) = 0
                            Next
                            If pageno = 2 Then
                                tempf = fixed + i
                                ' Response.Write("<br>" & tempf & "<br>")
                            Else
                                tempf = tempf + fixed
                            End If
                        End If
                    End If
                    If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                        ' Response.Write("<br>" & rs.Item("emptid") & "====" & fm.isResign(rs.Item("emptid"), Session("con"))(1))
                        If CDate(pd2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                            color = "red"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                            ' Response.Write(title & "<br>")
                            rlist &= rs.Item("emptid") & ","
                        Else
                            'color = "green"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                        End If

                    End If
                    outp(1) &= " <tr style='background:" & color & ";' title='" & title & "'>"
                    outp(1) &= " <td class='nox'>" & i.ToString & "</td>" & Chr(13)
                    cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))
                    If cell(1) = "None" Then
                        cell(1) = "&nbsp;"
                    End If
                    outp(1) &= "<td class='tin' width='89'>&nbsp;" & cell(1) & "</td>" & Chr(13)
                    If empid = "None" Then
                        cell(2) = "&nbsp;"
                    Else
                        cell(2) = fm.getfullname(empid, Session("con"))
                    End If

                    outp(1) &= " <td class='ename'> " & cell(2) & "</td>" & Chr(13)
                    outp(1) &= "<td class='edate'>" & dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid"), Session("con"))).ToShortDateString) & "</td>" & Chr(13)
                    outp(1) &= " <td class='cssamt' width='104'>" & FormatNumber(rs.Item("b_e"), 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= " <td class='cssamt'> &nbsp; " & FormatNumber(rs.Item("alw"), 2, TriState.True, TriState.True, TriState.True) & " </td>"
                    If IsNumeric(rs.Item("talw")) Then
                        talw = CDbl(rs.Item("talw")).ToString
                    Else
                        talw = "0"
                    End If


                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(talw, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    ot_back = fm.getinfo2("select sum(ot) from payrollx where remark='OT-Payment' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(ot_back) Then
                        If IsNumeric(rs.Item("ot")) Then
                            ot_back = CDbl(ot_back) + CDbl(rs.Item("ot"))

                        End If
                    Else
                        If IsNumeric(rs.Item("ot")) Then
                            ot_back = CDbl(rs.Item("ot"))
                        Else
                            ot_back = 0
                        End If
                    End If
                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(ot_back, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    increament = fm.getinfo2("select sum(txinco) from payrollx where remark='Increament' and  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(increament) = False Then
                        increament = "0"
                    End If

                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(increament, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    basic = fm.getinfo2("select b_e from payrollx where remark='monthly' and pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(basic) Then
                        tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw) + CDbl(basic)).ToString
                    Else
                        tincome = (CDbl(increament) + CDbl(ot_back) + CDbl(talw)).ToString
                    End If
                    colsum(8) += tincome

                    outp(1) &= "<td class='cssamt'> &nbsp;" & FormatNumber(tincome, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    taxsum = fm.getinfo2("select sum(tax) from payrollx where  pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(taxsum) Then
                    Else
                        taxsum = "0"
                    End If
                    colsum(9) += CDbl(taxsum)
                    outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= "<td class='cssamt'>&nbsp;</td>" & Chr(13)
                    taxsum = fm.getinfo2("select sum(netp) from payrollx where pddate between '" & pd1 & "' and '" & pd2 & "' and emptid=" & rs.Item("emptid"), Session("con"))
                    If IsNumeric(taxsum) = False Then
                        Response.Write(taxsum)
                        taxsum = "0"
                    End If

                    outp(1) &= "<td class='cssamt'>&nbsp;" & FormatNumber(taxsum, 2, TriState.True, TriState.True, TriState.True) & "</td>" & Chr(13)
                    outp(1) &= "<td class='csssign' style='" & Chr(13) & _
                   "border-right:double 2.25pt black;border-left:solid 1px black;border-bottom:solid 1.0pt black;" & Chr(13) & _
                   "mso-border-bottom-alt:solid .5pt;mso-border-right-alt:solid .5pt;" & Chr(13) & _
                   "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px;font-size:8px;'>(" & rs.Item("emptid") & ")<br>" & rs.Item("ref") & " </td>" & Chr(13)

                    outp(1) &= "</tr>"
                    i = i + 1
                End While

                outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                For t = 0 To 9
                    coltotal(t) = colsum(t) + coltra(t)
                Next
                outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr>"
                outp(1) &= "<tr><td colspan='13'>" & summerytax(i - 1, coltotal(8), coltotal(9), rlist) & "</td></tr>"

                outp(0) &= outp(1) & "</table>" & Chr(13) & "</td></tr></table>" & Chr(13)
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try

        Return outp(0)
    End Function
    Function pensionline(ByVal payroll As String)

    End Function

    Function summerytax(ByVal nx As Integer, ByVal ttxsal As Double, ByVal tax As Double, ByVal res As String)
        Dim rtn As String = ""
        Dim fm As New formMaker
        rtn = " <table ><tr><td>" & _
        "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse;'>" & _
                     " <tr>" & _
            "    <td class='tn' colspan='3'>" & _
             "       ክፍል -3- የወሩ የተነቃለለ ሂሳብ</td>" & _
                        " </tr>" & _
            "<tr>" & _
             "   <td class='ldbl'>" & _
              "      10</td>" & _
               " <td class='nnrowx'>" & _
                "    በዚህ ወር ደመወዝ የሚከፈላቸው የሠራተኞች ብዛት</td>" & _
                "<td class='numbc'>" & _
                 nx.ToString & "</td>" & _
            "</tr>" & _
        "            <tr>" & _
         "       <td  class='ldbl'>" & _
          "          20</td>" & _
           "     <td class='nnrowx'>" & _
            "        የወሩ ጠቅላላ የሥራ ግብር የሚከፈልበት ገቢ(ከሠንጠረዥ በ)</td>" & _
             "   <td class='numbc'>" & _
              FormatNumber(ttxsal, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
            "<tr>" & _
             "   <td  class='ldbl'>" & _
              "      30</td>" & _
               " <td class='nnrowx'>" & _
                "    የወሩ ጠቅላላ መከፈል ያለበት የስራ ግብር መጠን (ከሠንጠረዥ ተ)</td>" & _
                "<td class='numbc'>" & _
                 FormatNumber(tax, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
                    "</table>" & _
        "</td><td style='width:20px;'>&nbsp;</td><td>" & _
         "   <table border='0' cellpadding='0' cellspacing='0' style='border-collapse:" & _
 "collapse; width:400px;'>" & _
  "                     <tr height='21'>" & _
        "            <td class='tn' colspan=3>" & _
         "               ክፍል -4- በዚህ ወር የሥራ ውላቸው የተቋረጠ ሠራተኞች ዝርዝር መረጃ</td>" & _
          "         </tr>" & _
               " <tr>" & _
                "    <td class='lheads' style='width:7%;'>" & _
                 "       ተ.ቁ</td>" & _
                  "  <td class='heads' style='width:23%;'>" & _
                   "    የሠራተኛው የግብር <br>ከፋይ መለያ ቁጥር(TIN)" & _
                  "  </td>" & _
                   " <td class='headsn' style='width:69%;'>" & _
        "                        የሠራተኛው ስም:የአባት ስም፡ የአያት ስም" & _
        "                    </td>" & _
         "       </tr>"
        Dim empid As String = ""
        '  Response.Write(res)
        Dim emptid() As String = res.Split(",")
        Dim cell() As String = {"", "", ""}
        Dim cemptt As String = ""
        Array.Sort(emptid)
        Dim cx As Integer = 1
        Dim rdate As String
        For j As Integer = 0 To emptid.Length - 2
            If cemptt <> emptid(j) Then
                cemptt = emptid(j)
                rdate = fm.isResign(emptid(j), Session("con"))(1)
                If CDate(rdate).Month = CDate(Request.Item("pd1")).Month And CDate(rdate).Year = CDate(Request.Item("pd1")).Year Then
                    empid = fm.getinfo2("select emp_id from emprec where id='" & emptid(j) & "'", Session("con"))

                    cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))

                    If cell(1) = "None" Then
                        cell(1) = ""
                    End If
                    If cell(1) <> "" Then
                        If cell(1).Length <> 10 Then
                            cell(1) &= "(error)"
                        End If
                    End If
                    '  Response.Write(empid)
                    If empid = "None" Then
                        cell(2) = "&nbsp;"
                    Else
                        cell(2) = fm.getfullname(empid, Session("con"))
                    End If
                    rtn &= " <tr>" & _
                        "    <td class='ldbl'>" & _
                        (cx) & ")</td>" & _
                          "  <td  class='nnrowx'>" & _
                                    cell(1) & " </td>" & _
                            "<td  class='fname'>" & _
                            cell(2) & "  &nbsp;</td>" & _
                       " </tr>"
                    cx = cx + 1
                End If
            End If

        Next


        rtn &= "</table>" & _
        "</td><td style='width:20px;'>&nbsp;</td><td>" & _
        "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:" & _
 "collapse; border:2pt double black;'>" & _
  "                 <tr >" & _
      "          <td class='nnrow'>" & _
       "             የተከፈለበት ቀን</td><td class='nnrow'>&nbsp;</td>" & _
        "    </tr>" & _
         "   <tr >" & _
          "      <td class='nnrow'>" & _
           "         የደረሰኝ ቁጥር</td><td class='nnrow'>&nbsp;</td>" & _
            "</tr>" & _
            "<tr >" & _
             "   <td class='nnrow'>" & _
              "      የገንዘብ ልክ</td><td class='nnrow'>&nbsp;</td>" & _
            "</tr>" & _
            "<tr height='20'>" & _
             "   <td class='nnrow'>" & _
              "      ቼክ</td><td class='nnrow'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" & _
            "</tr>" & _
            "<tr >" & _
             "   <td class='nnrow'>" & _
              "      የገንዘብ ተቀባይ ፊርማ</td><td class='nnrow'>&nbsp;</td>" & _
            "</tr>" & _
        "</table>" & _
        "</td></tr></table><br> " & _
         "   <table  cellpadding='0' cellspacing='0' style='width:960px;'>" & _
  "                      <tr >" & _
                   "<td class='style11' colspan='2' height='22' >" & _
                "ክፍል -5- የትክክለኛነት ማረጋገጫ</td>" & _
               "               </tr>" & _
              " <tr>" & _
               "    <td style='text-align:left;font-size:11px;'><table style='width:960px; border:2px balck solid;'><tr><td style='width:33%;border-right:1px solid black;'>" & _
                "በላይ የተገለጸው መስወቂያና የተሰጠው መረጃ በሙሉ የተሞላና ትከከለኛ መሆኑን አረጋግጣለሁ፡፡ ትክክለኛ ያልሆነ መረጃ መቅረብ " & _
                "መግብር ሕጎችም ሆነ በወንጀለኛ መቅጫ ሕግ የሚስቀጣ መሆኑን ገነዘባለሁ፡፡</td>" & _
                 "  <td style='width:30%;border-right:1px solid black;'>የግብር ከፋይ/ህጋዊ ወኪል<br>" & _
       " ስም____________________<br>" & _
       " ፊርማ___________________<br>" & _
       " ቀን____________________<br>" & _
"</td><td style='width:7%;border-right:1px solid black;'>ማኅተም</td><td style='width:30%'>የግብር ባለስልጣን<br> " & _
 " ስም____________________<br>" & _
       " ፊርማ___________________<br>" & _
       " ቀን____________________<br>" & _
       "</td></tr></table></td>" & _
               "</tr>" & _
           "</table>"
        Return rtn
    End Function
    Function printexport()
        Dim rtn As String = ""
        Dim loc As String
        loc = Session("path") & "/taxpens/"
        If Directory.Exists(loc) = False Then
            MkDir(loc)
        End If
        rtn &= " ></div></td></tr><tr><td><hr style='width:600px;' align=left></td></tr>"
        rtn &= "<tr><td><div class='clickexp' style=' float:left; border:none; width:150px;height:28px;" & Chr(13) & _
            "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; display:none;' " & Chr(13) & _
            "onclick='javascript:exportx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & "','xls','" & loc & "','export','2;3');' >" & Chr(13) & _
"<img src='images/gif/exportxls.gif' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>"
    End Function
    Public Function getprojempx(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim rtn As String = ""
        Dim did As String
        Try


            rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and '" & sdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "') order by emp_id,emptid desc", con)
            Dim d1, d2, de, ds As Date
            d1 = Nothing
            d2 = Nothing
            Dim i As Integer = 0

            If rs.HasRows Then

                Try
                    'Response.Write(" start    ====     requested     ====    Date end<br>")
                    While rs.Read
                        rtn &= "'" & rs.Item("emptid") & "',"
                        i = i + 1
                        'Response.Write(rtn)
                    End While

                Catch ex As Exception
                    '              Response.Write(ex.ToString)
                    rtn = "'',"
                    'exception_hand(ex)
                End Try

            End If
            If rtn <> "" Then
                rtn = rtn.Substring(0, rtn.Length - 1)
            End If
            rs.Close()
            dbs = Nothing

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
        'Response.Write(projid & "<br>innnn" & rtn)
        Return rtn
    End Function
    Function በታች()
        Dim rtn As String = "<br>የድርጅቱ ከፋይ/ሕጋዊ ወኪል ስም__________________________________________ ፊርማ_________________ ቀን___________________"
        Return rtn
    End Function
    Public Function getjavalist2(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection, ByVal sp As String, ByVal spx As String) As String
        Dim db As New dbclass
        Dim fm As New formMaker
        Dim sql As String = "select  " & dis & " from " & dbtabl
        '  Response.Write(sql & "<br>")
        Dim dt As DataTableReader
        Dim retstr As String = ""
        Try
            dt = db.dtmake("dbtbl", sql, conx)
        Catch ex As Exception
            Return ex.ToString & "<br>" & sql
        End Try

        Dim disp() As String
        Dim dispn As Integer = 1
        disp = dis.Split(",")
        Dim optdis As String = ""
        If disp.Length > 1 Then
            dispn = disp.Length
        End If
        If dt.HasRows = True Then
            While dt.Read
                retstr &= Chr(34)

                For i As Integer = 0 To disp.Length - 1
                    If dt.IsDBNull(i) = False Then
                        If LCase(dt.GetDataTypeName(i)) = "string" Then
                            retstr &= dt.Item(i).trim & sp
                        Else
                            retstr &= dt.Item(i) & sp

                        End If
                    Else
                        retstr &= fm.getinfo2("select ot from payrollx where id=" & dt.Item(disp.Length - 1), Session("con")) & sp
                    End If

                Next
                retstr &= Chr(34)
                retstr &= spx
                'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
            End While

        End If
        dt.Close()

        retstr = retstr.Substring(0, retstr.Length - 1)
        Return retstr
    End Function

    Function makepage_b(ByRef ref As String, ByRef month As Integer, ByRef year As Integer)
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtime
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        Dim cell(9) As String
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim sumpen As Double
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        Dim j As Integer
        ' Response.Write(ref)
        Dim count As Integer
        Dim fixed As Integer
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        count = 0
        Dim pageno As Integer = 1
        rs = dbs.dtmake("vwpayroll", "select * from payrollx where emptid in (" & ref & ")", Session("con"))
        If rs.HasRows Then
            ' outp(0) = headerx(month.ToString & "/1/" & year.ToString, pc, pe)
            outp(1) &= "<div class='page'>"
            While rs.Read
                Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                'Response.Write(empid)
                If i = 1 Then
                    outp(1) &= "<div class='subpage'>page" & pageno.ToString & "<br>" & headerx(month.ToString & "/1/" & year.ToString, pc, pe)
                Else
                    If i Mod fixed = 0 Then
                        outp(1) &= "</table></div>"
                        pageno += 1
                        outp(1) &= "<div class='subpage'>" & i & "page" & pageno.ToString & "<br>" & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)

                    End If
                End If
                cell(0) = i.ToString
                cell(1) = fm.getinfo2("select emp_tin from emp_static_info where emp_id ='" & empid & "'", Session("con"))
                If cell(1) = "None" Then
                    cell(1) = ""
                End If
                '  Response.Write(empid)
                If empid = "None" Then
                    cell(2) = "&nbsp;"
                Else
                    cell(2) = fm.getfullname(empid, Session("con"))
                End If

                cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid"), Session("con"))).ToShortDateString)
                cell(4) = rs.Item("b_e")
                cell(5) = rs.Item("pen_e")
                cell(6) = rs.Item("pen_c")
                sumpen = CDbl(cell(5)) + CDbl(cell(6))
                cell(7) = sumpen.ToString
                cell(8) = "&nbsp"
                outp(1) &= " <tr style='font-size:8pt;'>" & Chr(13)
                For j = 0 To 8

                    If j = 0 Then
                        outp(1) &= " <td style='" & Chr(13) & _
       "border-left:double 2.25pt;border-bottom:solid 1.0pt;border-right:solid 1.0pt;" & Chr(13) & _
       "border-color:windowtext;mso-border-left-alt:" & Chr(13) & _
       "double 2.25pt;mso-border-bottom-alt:solid .5pt;mso-border-right-alt:solid .5pt;" & Chr(13) & _
       "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>" & cell(j) & " </td>" & Chr(13)
                    ElseIf j = 2 Then
                        outp(1) &= " <td style='" & Chr(13) & _
      "border:1px solid black;width:220px;'>" & cell(j) & " </td>" & Chr(13)
                    ElseIf j = 8 Then
                        outp(1) &= "  <td style='" & Chr(13) & _
       "border-right:double 2.25pt black;border-left:solid 1px black;border-bottom:solid 1.0pt black;" & Chr(13) & _
       "mso-border-bottom-alt:solid .5pt;mso-border-right-alt:solid .5pt;" & Chr(13) & _
       "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;</td>" & Chr(13)
                    ElseIf j <= 3 Then
                        outp(1) &= "<td class='nnrow'>" & cell(j) & "&nbsp;</td>" & Chr(13)

                    Else
                        outp(1) &= "<td class='numb'>" & Chr(13) & _
                     FormatNumber(cell(j), 2, TriState.True, TriState.True, TriState.True) & "          </td>" & Chr(13)
                        colsum(j) += CDbl(cell(j))
                    End If
                Next
                outp(1) &= "</tr>"
                i = i + 1
            End While
            outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
        End If
        Session("page") = outp(0)
        Return outp(0)
    End Function
    Function ssession_on(ByVal item As String)
        Session(item) = 1
        '  Response.Write(Session(item) & "===" & item & "<br>")
    End Function
    Function session_off(ByVal item As String)
        Session(item) = 0
        '  Response.Write(Session(item) & "===" & item & "<br>")
    End Function
End Class
Public Class dtime
    Function convert_to_GC(ByVal da As String)


        Try

            Dim datefx, datef(3) As String
            datefx = da.ToString
            Dim datefxx() As String
            datefxx = da.Split(".")
            'Response.Write(datefxx(0))
            datef(0) = datefxx(2)
            datef(1) = datefxx(1)
            datef(2) = datefxx(0)
            Dim ma As Integer = 0
            Dim daydiff() As Integer
            Dim md As Integer = CInt(8 - 12)
            Dim dayinm() As Integer = {0, 30, 31, 30, 31, 31, 28, 31, 30, 31, 30, 31, 31}
            Dim dd, mm, yy As Integer
            If datef(1) >= 1 And datef(1) < 7 Then
                If CInt(datef(0) - 1) Mod 4 = 3 Then 'leap year date differnce
                    '  Response.Write("leap year<br>")


                    daydiff = New Integer() {0, 11, 11, 10, 10, 9, 8, 8, 8, 8, 8, 7, 7, 5}
                    dayinm = New Integer() {0, 30, 31, 30, 31, 31, 29, 31, 30, 31, 30, 31, 31}
                Else
                    '{0, 9, 8, 9, 8, 8, 7, 7, 10, 10, 10, 9, 9}
                    daydiff = New Integer() {0, 10, 10, 9, 9, 8, 7, 8, 9, 8, 8, 7, 7, 5}
                End If
            Else


                If CInt(datef(0)) Mod 4 = 0 Then 'leap year date differnce

                    daydiff = New Integer() {0, 11, 11, 10, 10, 9, 8, 9, 8, 8, 7, 7, 6, 5}
                    dayinm = New Integer() {0, 30, 31, 30, 31, 31, 29, 31, 30, 31, 30, 31, 31}
                Else
                    daydiff = New Integer() {0, 10, 10, 10, 9, 9, 9, 9, 8, 8, 7, 7, 6, 5}
                End If
            End If
            If datef(1) <> 13 Then
                If (datef(2) + daydiff(datef(1))) > dayinm(datef(1)) Then
                    ma = 1
                    dd = datef(2) + daydiff(datef(1)) - dayinm(datef(1))
                    ' Response.Write(daydiff(datef(1)) & "====" & dayinm(datef(1)) & "<br>")
                Else
                    ma = 0
                    dd = daydiff(datef(1)) + datef(2)

                    'yy = datef(0) + 7
                End If
            Else
                mm = 9
                If (datef(2) + daydiff(datef(1))) >= dayinm(datef(1) - 1) Then
                    ma = 1
                    dd = datef(2) + daydiff(datef(1)) - dayinm(datef(1) - 1)

                Else
                    ma = 0
                    dd = daydiff(datef(1)) + datef(2)
                End If

            End If
            If datef(1) < 5 And mm <> 9 Then
                mm = datef(1) + 8 + ma
            ElseIf mm <> 9 Then
                mm = datef(1) - 4 + ma
            End If
            If mm = 13 Then
                mm = 1
            End If
            If mm > 9 And ma <= 12 Then

                yy = datef(0) + 7


            ElseIf mm >= 1 And mm < 9 Then

                yy = datef(0) + 8

            ElseIf mm = 9 Then
                If datef(1) = 13 Then
                    yy = datef(0) + 8
                Else
                    If daydiff(datef(1)) >= datef(2) Then
                        'Response.Write(daydiff(datef(1)) & " <= " & datef(2) & "<br>")
                        yy = datef(0) + 7
                    Else
                        yy = datef(0) + 8
                    End If
                End If




                ' Response.Write("......" & datef(0) & "<br>")
            End If
            ' yy = 2016
            Return mm.ToString & "/" & dd.ToString & "/" & yy.ToString
        Catch ex As Exception
            Return ex.ToString
        End Try
    End Function
    Public Function convert_to_ethx(ByVal da As Date)
        Dim lp As Boolean = False
        Dim lpd As Integer = 0
        Dim d, m, y As Integer
        d = da.Day
        m = da.Month
        y = da.Year
        Dim etd, etm, ety As Integer
        Dim dx() As Integer
        Dim rtn As String = ""
        If y Mod 4 = 3 Then
            lp = True   '0123456789101112
            dx = New Integer() {0, 8, 7, 9, 8, 8, 7, 7, 6, 11, 11, 10, 10}

        ElseIf y Mod 4 = 0 Then
            dx = New Integer() {0, 9, 8, 9, 8, 8, 7, 7, 6, 10, 10, 9, 9}
        Else

            dx = New Integer() {0, 8, 7, 9, 8, 8, 7, 7, 6, 10, 10, 9, 9}
        End If
        etd = CInt(d) - CInt(dx(m))
        Dim etdd As Integer = CInt(d) - CInt(dx(m))
        Dim yrm As Integer = 0
        Dim dtmx As Integer = 0
        If etd <= 0 Then
            ' rtn &= ">>>>>" & etd.ToString & "<<<<--dmmm-change>>>>"
            dtmx = 1
            If lp = True Then
                lpd = 6
            Else
                lpd = 5
            End If
            If m = 9 Then
                etd = 30 + etd + lpd
                etm = 12
                If etd > 30 Then
                    etm = 13
                    etd = lpd + etdd
                End If

            Else
                If m = 9 Then
                    yrm = 1
                End If
                etd = 30 + etd


            End If
        Else
            If m = 9 Then

            End If
            'rtn &= etd & "grater date>>>>>"

        End If

        If m <= 8 Then
            ety = y - 8
        ElseIf etm = 13 Then

            ety = y - 8


        ElseIf etm <> 0 Then
            ety = y - 8
        Else

            ety = y - 7
            ' rtn &= " >>>>yr<<<< " & ety & "<<<<"

        End If
        If etm = 0 Then
            If etm <> 13 Then
                If m > 9 Then
                    '      rtn &= "mmmm " & m & "<<<<<"
                    etm = m - 8 - dtmx
                ElseIf m = 9 Then
                    If dtmx > 1 Then
                        etm = 12
                    Else
                        etm = m - 8 - dtmx
                    End If
                Else
                    '       rtn &= "mmmm " & m & "<<<<<"
                    etm = m + 4 - dtmx
                End If
            End If
        End If
        Return etd.ToString & "." & etm.ToString & "." & ety.ToString & rtn

    End Function

    Function getmonth(ByVal m As Integer, ByVal rtn As String)
        Dim amhmonthNames() As String = {"", "መስከረም", "ጥቅምት", "ኅዳር", "ታህሣሥ", "ጥር", "የካቲት", "መጋቢት", "ሚያዝያ", "ግንቦት", "ሰኔ", "ሐምሌ", "ነሐሴ", "ጳጉሜ"}
        Dim amhmonthshort() As String = {"", "መስከ", "ጥቅም", "ኅዳር", "ታህሣ", "ጥር", "የካቲ", "መጋቢ", "ሚያዝ", "ግንቦ", "ሰኔ", "ሐምሌ", "ነሐሴ", "ጳጉሜ"}
        Dim ethmonth() As String = {"", "Meskerm", "Tikmet", "Hidar", "Tahisas", "Tir", "Yekatite", "Megabit", "Miazia", "Ginbot", "Sene", "Hamele", "Nehasse", "Pagume"}

        If rtn = "amh" Then
            Return amhmonthNames(m)
        ElseIf rtn = "amh_short" Then
            Return amhmonthshort(m)

        ElseIf rtn = "amheng" Then
            Return ethmonth(m)
        End If
    End Function

End Class



