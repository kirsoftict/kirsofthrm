Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class pensioncont
    Inherits System.Web.UI.Page
   

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
        Dim ulist, tulist As String
        tulist = ""
        Dim rrr As DataTableReader
        Dim sum(15) As Double
        Dim avalue As String = ""
        Dim pcheck() As String
        '  Dim sumbsal, sumbearn, sumtalw, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double

        Dim spension As String = ""
        Dim headcop As String = "none"
        Dim dbs As New dbclass
        Dim strc As Object = "0"
        Dim fm As New formMaker
        'Dim sec As New k_security
        Dim tpaidlist As String = ""
        Dim rtnallp As String
        Dim divsum As String = ""
        Dim damt As Double = 0
        Dim projename As String = ""
        Dim totalemp As String = ""
        Dim ttpx As String = ""
        If Request.QueryString("prid") <> "" Then
            'Response.Write(Request.QueryString("prid"))
        Else
            ' Response.Write(Request.QueryString("month"))
            If Request.Form("month") <> "" Or Request.QueryString("month") <> "" Then
                If Request.Form("month") <> "" Then
                    nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                    pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
                    projename = Request.Form("projname")
                Else
                    nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
                    pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
                    pdate2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
                    projename = Request.QueryString("projname")
                End If
                '  Response.Write(projename)
                ' projename = Request.Form("projname")
                If (projename = "") Then
                    projename = Request.Form("projnamex")
                End If
                ' Response.Write(Request.QueryString("paidst"))
                Dim spl() As String
                Dim projid As String = ""

                Dim pjn() As String
                Dim refxx As String = "'xx'"

                '  Response.Write(projename & "<br>")
                If projename<>"" then
                Try
                    pjn = projename.Split("$")
                    Catch ex2 As Exception
                        Response.Write(ex2.ToString)
                End Try

                Dim rtn() As String
                Dim intno As Integer = 0
                'Dim outp As String
                Dim outp As String = ""
                Dim rtnvalue As String = ""
                Dim outputall As String = ""
                Dim ppname As String
                Dim valx() As String
                rtnvalue = ""
                Dim pce, pcc, ppce, ppcc, tpce, tpcc As Double
                Dim tsum As Double = 0
                pce = 0
                pcc = 0
                ppce = 0
                ppcc = 0
                tpcc = 0
                tpce = 0
                Dim divsum2 As String = ""

                Dim sql As String = ""
                    totalemp = ""
                ppname = projename.Replace("""", "")
                paidlist = ""

                ' Response.Write(ppname & "<br>")
                'spl = ppname.Split("|")
                Dim m, y As String
                m = pdate1.Month
                y = pdate1.Year
                Dim proj, rssqlnew As String
                    Session("rtnvalue") = ""

                    ttpx = getemp_data("select distinct emptid from payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "' and pen_e>0", ",", True)
                    'rtnvalue &= "</td></tr><tr><td>...." & getemp_data("select distinct id from emprec where hire_date<='" & pdate2 & "' and (end_date is null or '" & pdate2 & "' between hire_date and isnull(end_date,'" & Today.ToShortDateString & "'))", "</td><td>", True)
                    'rtnvalue &= "</td></tr><tr><td>intercetion...." & getemp_data("select distinct id from emprec where hire_date<='" & pdate2 & "' and (end_date is null or '" & pdate2 & "' between hire_date and isnull(end_date,'" & Today.ToShortDateString & "')) intersect select distinct id from emprec where hire_date<='" & pdate2 & "' and (end_date is null or '" & pdate2 & "' between hire_date and isnull(end_date,'" & Today.ToShortDateString & "'))", "</td><td>", True)

                    ' Response.Write(ttpx & "<<<<<<<<<<<<<<")
                If pjn.Length > 0 Then
                    divsum = "<div id='bigsum'><table class='vwpen'><tr><td>Project Name</td><td>Payroll Ref</td><td>Employee Cont</td><td>Em'r Cont. </td><td> total cont.</td><td>&nbsp;<tr>"
                    For i As Integer = 0 To pjn.Length - 1
                        'Response.Write("------------------------------------------------------------------<br>")
                        spl = (pjn(i).Replace("""", "")).Split("|")
                        proj = spl(0)
                        '   rtnvalue = fm.getprojemp(spl(1), pdate2, Session("con"))
                        ' Response.Write(rtnvalue & "<br>")
                            'fm.getprojemp(projid.ToString, pdate2, Session("con")) 

                            rtnvalue = fm.getprojemp(spl(1).ToString, pdate2, Session("con")).replace("'", "")
                            '  Response.Write("<br>" & rtnvalue)
                            '  rtnvalue = getemp_data("select distinct emptid from payrollx where pddate between '" & pdate1 & "' and '" & pdate2 & "'", ",", True)

                            ' Response.Write("<br>" & rtnvalue)select pa.emptid from emp_job_assign where project_id='" & spl(1).tostring & "' and 
                            ' Session("rtnvalue") &= rtnvalue
                        If rtnvalue.Length > 1 Then
                            'Response.Write("<br>" & spl(0) & "<br>________________________________________<br>" & rtnvalue & "<br>")
                                Session("rtnvalue") &= rtnvalue
                                totalemp &= rtnvalue
                                rtnvalue = rtnvalue.Substring(0, rtnvalue.Length - 1)

                            divsum &= "<tr><td colspan='6'>" & pjn(i) & "</td></tr>"
                            ' rtnvalue = rtnvalue.Substring(0, rtnvalue.Length - 1)
                                sql = "select distinct ref from payrollx where pddate<='" & pdate2 & "' and  pddate>='" & pdate1 & "' and emptid in(" & rtnvalue & ") and (remark='monthly' or remark='pay_inc_middle')"

                                '  Response.Write("<br>..............................................<br>" & sql & "<br>" & rtnvalue & "<br>" & spl(0) & "<br>.................end...........................<br>")

                            rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                   "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                   "and emprec.id in(" & rtnvalue & ")" & _
                                                                   " ORDER BY emp_static_info.first_name,emprec.id desc "
                                Try

                                    ' Response.Write(sql & "<br>")

                                    rrr = dbs.dtmake("vwpaid", sql, Session("con"))
                                    If rrr.HasRows Then


                                        If spl(1) = "01" Then
                                            'Dim cot() As Object = rrr.GetValues\\\\\(0)
                                            'Dim cs As Integer = rrr.GetValues(cot)

                                            ' Response.Write("<br>"  & rtnvalueqa & "</br>")
                                            ' Response.Write(cs)
                                        End If

                                        paidlist = ""
                                        ppcc = 0
                                        ppce = 0
                                        Dim remark As String
                                        While rrr.Read
                                            If refxx.Length = 0 Then
                                                refxx &= "'" & rrr.Item(0) & "'"
                                            Else
                                                refxx &= ",'" & rrr.Item(0) & "'"
                                            End If

                                            '   Response.Write(rrr.Item(0) & "<br>" & salarypaidlist(rrr.Item(0)) & "<br>")

                                            paidlist &= salarypaidlist(rrr.Item(0)) & ","
                                            ' Response.Write(rrr.Item(0) & "<<<<<<<<<<<<<<<<<br>")
                                            pcc = fm.getinfo2("select sum(pen_c) from payrollx where ref='" & rrr.Item(0) & "'", Session("con"))
                                            pce = fm.getinfo2("select sum(pen_e) from payrollx where ref='" & rrr.Item(0) & "'", Session("con"))
                                            ' rt = fm.getinfo2("select sum(b_e) from payrollx where ref='" & rrr.Item(0) & "'", Session("con"))
                                            remark = fm.getinfo2("select remark from payrollx where ref='" & rrr.Item(0) & "'", Session("con"))
                                            ' Response.Write(remark & "<br>")
                                            proj = sec.dbStrToHex(proj)
                                            If remark <> "None" Then
                                                Select Case LCase(remark)
                                                    Case "monthly"
                                                        divsum2 = "  <div class='viewtax' onclick=" & _
                                                       Chr(34) & "javascript:payrollview1('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "&ptype=payroll&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                                       Chr(34) & ">" & rrr.Item(0) & "</div>"
                                                    Case "increament"
                                                        divsum2 = " <div class='viewtax' onclick=" & _
                                                        Chr(34) & "javascript:otherpayment('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                                        Chr(34) & ">" & rrr.Item(0) & "</div>"
                                                    Case "pay_inc_middle"
                                                        divsum2 = " <div class='viewtax' onclick=" & _
                                                            Chr(34) & "javascript: payrollmid('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                                            Chr(34) & ">" & rrr.Item(0) & "</div>"
                                                    Case "ot-payment"
                                                        divsum2 = "  <div class='viewtax' onclick=" & _
                                                        Chr(34) & "javascript:otview('view','?prid=" & rrr.Item(0) & "&pd=" & pdate1 & "&month=" & m & "&year=" & y & "&&projname=" & proj & "');" & _
                                                        Chr(34) & ">" & rrr.Item(0) & "</div>"
                                                    Case Else
                                                        divsum2 = "Sorry "


                                                End Select
                                            End If
                                            divsum &= "<tr><td> &nbsp;</td><td>" & divsum2 & "</td><td style='text-align:right;'>" & FormatNumber(pce, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                            divsum &= "<td style='text-align:right;'>" & FormatNumber(pcc, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                            divsum &= "<td style='text-align:right;'>" & FormatNumber(pce + pcc, 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                            ppcc += pcc
                                            ppce += pce



                                        End While
                                        tpaidlist &= paidlist

                                        divsum &= "<tr style='font-weight:bold;'><td> &nbsp;</td><td> &nbsp;</td><td style='text-align:right;'>" & FormatNumber(ppce, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                        divsum &= "<td style='text-align:right;'>" & FormatNumber(ppcc, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                                        divsum &= "<td style='text-align:right;'>" & FormatNumber(ppce + ppcc, 2, TriState.True, TriState.True, TriState.True) & "</td><td style='text-align:right;'>" & FormatNumber(ppce + ppcc, 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                        '  divsum &= "<tr style='font-weight:bold;color:red'><td colspan='7'>All: " & rtnvalue & "</td></tr>"
                                        ' divsum &= "<tr style='font-weight:bold;color:red'><td colspan='7'>paidlist: " & paidlist & "</td></tr>"
                                        tpcc += ppcc
                                        tpce += ppce
                                        '  paidlist = paidlist.Substring(0, paidlist.Length - 1)
                                        ' divsum &= "<tr style='font-weight:bold;color:red'><td colspan='7'>pdl: " & paidlist & "</td></tr>"
                                        ' Response.Write(paidlist & "<br>" & rtnvalue & "<br>")
                                        ulist = unpaidlist(paidlist, rtnvalue, pdate1, pdate2)
                                        If ulist <> "" Then
                                            divsum &= "<tr style='font-weight:bold;color:red'><td colspan='7'>Unpaid : " & unpaidlist(paidlist, rtnvalue, pdate1, pdate2) & "</td></tr>"
                                            tulist &= ulist
                                            '  Response.Write("<br>unpaid................." & ulist & ".................</br>")
                                        End If

                                    Else
                                        paidlist = ""
                                        ulist = unpaidlist(paidlist, rtnvalue, pdate1, pdate2)
                                        If ulist <> "" Then
                                            divsum &= "<tr style='font-weight:bold;color:red'><td colspan='7'>Unpaid : " & unpaidlist(paidlist, rtnvalue, pdate1, pdate2) & "</td></tr>"
                                            tulist &= ulist
                                        End If
                                    End If
                                Catch ex As Exception
                                    Response.Write(ex.ToString & "<br>" & sql & "<br>")
                                End Try

                        End If
                    Next
                    '  divsum &= "<tr style='font-weight:bold;color:red; max-width:800px;'><td colspan='7'>paidlist: " & tpaidlist & "</td></tr>"

                    divsum &= "<tr style='font-weight:bold;font-size:14pt;'><td> &nbsp;</td><td> &nbsp;</td><td> &nbsp;</td><td style='text-align:right;'>" & FormatNumber(tpce, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    divsum &= "<td style='text-align:right;'>" & FormatNumber(tpcc, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                    divsum &= "<td style='text-align:right;'>" & FormatNumber(tpce + tpcc, 2, TriState.True, TriState.True, TriState.True) & "</td></tr></table></div>"
                End If
            End If
                Dim loc As String = Server.MapPath("download")
                Dim divsum4() As String
                Dim loc1, loc2, loc3 As String
            Dim divsum3 As String
                If Request.Form("paidst") = "unpaid" Then
                    divsum3 = "<div id='pensionpaid'>" & pensionall_c_new(tpaidlist, tulist, pdate1.Month, pdate1.Year) & "</div>"

                    loc1 = loc.Replace("\", "/")
                    loc1 &= "/" & Now.Ticks.ToString & ".txt"
                    '  Response.Write(loc)
                    File.WriteAllText(loc1, divsum3)
                    divsum3 = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                                 "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                                " display:block;' onclick=" & Chr(34) & "javascript:exportxx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc1 & "','export','2;3');" & Chr(34) & " >" & _
                                "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                                "<div id='pinta4x'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                          " onclick=" & Chr(34) & "javascirpt:print('pensionpaid','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','pension');" & Chr(34) & ">" & _
                                     "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                                    " <div style='float:left;'><input type='checkbox' id='signc' name='signc' value='ok' onclick=" & Chr(34) & "javascript:checkedc('signc');" & Chr(34) & "/> Clear signature Part</div>" & _
        "<div style='float:left;'><input type='checkbox' id='tinp' name='tinp' value='ok' onclick=" & Chr(34) & "javascript:checkedc('tinp');" & Chr(34) & "/> Clear Tin</div><div style='clear:left;'></div><br><div style='height:50px;'>..........................</div>" & _
        "<div id='bigprint'>" & divsum3 & "</div>"
                    '/loc = Session("path")
                    loc2 = loc.Replace("\", "/")
                    loc2 &= "/s" & Now.Ticks.ToString & ".txt"
                    '  Response.Write(loc)
                    File.WriteAllText(loc2, divsum)
                    divsum = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                                "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                               " display:block;' onclick=" & Chr(34) & "javascript:exportxx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc2 & "','export','2;3');" & Chr(34) & " >" & _
                               "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                               "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                         " onclick=" & Chr(34) & "javascirpt:printtax('bigsum','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printA4');" & Chr(34) & ">" & _
                                    "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                                   "<br>" & _
         divsum
                    Response.Write(divsum)
                    Response.Write("<br>................................................................................<br>" & divsum3)

                Else

                    Dim kar As Array = Split(totalemp, ",")
                    Array.Sort(kar)

                    Dim arrx As String = arrayto(kar, ",")



                    Response.Write(arrx & "=======" & kar.Length.ToString & "========" & Split(arrx, ",").Length & "<br>")
                    Response.Write(totalemp & "======totalemp=========" & Split(totalemp, ",").Length & "<br>")
                    Response.Write(ttpx & "=========ttpx======" & Split(ttpx, ",").Length & "<br>")
                    Response.Write(tpaidlist & "======tpaidlist=========" & Split(tpaidlist, ",").Length & "<br>")
                    divsum4 = pensionall_b_new(ttpx, "", pdate1.Month, pdate1.Year)
                    Dim rtn1, rtn2 As String
                    rtn1 = "<div id='pensionpaid'>" & divsum4(0) & "</div>"
                    rtn2 = "<div id='pensum'>" & divsum4(2) & "</div>"

                    loc1 = loc.Replace("\", "/")
                    loc1 &= "/detail" & Now.Ticks.ToString & ".txt"
                    '  Response.Write(loc)
                    File.WriteAllText(loc1, divsum4(0))
                    divsum3 = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                                 "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                                " display:block;' onclick=" & Chr(34) & "javascript:exportxx('pension(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc1 & "','export','2;3');" & Chr(34) & " >" & _
                                "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                                "<div id='pinta4x'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                          " onclick=" & Chr(34) & "javascirpt:print('pensionpaid','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','pension');" & Chr(34) & ">" & _
                                     "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                                    " <div style='float:left;'><input type='checkbox' id='signc' name='signc' value='ok' onclick=" & Chr(34) & "javascript:checkedc('signc');" & Chr(34) & "/> Clear signature Part</div>" & _
        "<div style='float:left;'><input type='checkbox' id='tinp' name='tinp' value='ok' onclick=" & Chr(34) & "javascript:checkedc('tinp');" & Chr(34) & "/> Clear Tin</div><div style='clear:left;'></div><br><div style='height:50px;'>..........................</div>" & _
        "<div id='bigprint'>" & rtn1 & "</div>"
                    loc2 = loc.Replace("\", "/")
                    loc2 &= "/sum" & Now.Ticks.ToString & ".txt"
                    '  Response.Write(loc)
                    File.WriteAllText(loc2, divsum4(1))
                    divsum3 &= Chr(13) & "<div id='clickexp2' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                                 "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                                " display:block;' onclick=" & Chr(34) & "javascript:exportxx('penssum(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc2 & "','export','2;3');" & Chr(34) & " >" & _
                                "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                                "<div id='pinta4x'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                          " onclick=" & Chr(34) & "javascirpt:print('pensum','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','pension');" & Chr(34) & ">" & _
                                     "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                                    " <br><div style='height:50px;'>..........................</div>" & _
        "<div id='bigprint'>" & rtn2 & "</div>"
                    ' loc = Session("path")
                    loc3 = loc.Replace("\", "/")
                    loc3 &= "/s" & Now.Ticks.ToString & ".txt"
                    '  Response.Write(loc)
                    File.WriteAllText(loc3, divsum)
                    divsum = Chr(13) & "<div id='clickexp3' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                                "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                               " display:block;' onclick=" & Chr(34) & "javascript:exportxx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc3 & "','export','2;3');" & Chr(34) & " >" & _
                               "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                               "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                         " onclick=" & Chr(34) & "javascirpt:printtax('bigsum','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printA4');" & Chr(34) & ">" & _
                                    "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                                   "<br>" & _
         divsum
                    Response.Write(divsum)
                    Response.Write("<br>................................................................................<br>" & divsum3)


                End If
            Else
                Response.Write(Request.Form("projnamex"))
            End If

        End If

        Response.Write("<br>" & totalemp)

    End Function

    Function pensionall_b_new(ByVal ref1 As String, ByVal ref2 As String, ByRef month As Integer, ByRef year As Integer)
        ' Response.Write(ref2 & "<br>dsfsdfsd")
        Dim outp(3) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        '   Response.Write("<br>............kir.............<br>")
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        Dim cell(9) As String
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim sumpen As Double
        Dim rs, rs2 As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        Dim j As Integer
        Dim d1 As String = month & "/1/" & year
        Dim d2 As String = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        ' Response.Write(ref1 & "=====" & ref2 & "<br>")
        Dim ref() As String
        Dim count As Integer
        Dim fixed As Integer
        Dim title As String
        Dim nod As Integer
        Dim rlist As String = ""
        Dim r_count As Integer = 0
        Dim color As String = "white"
        Dim pgof As String = ""
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 19
        End If
        count = 0

        Dim pageno As Integer = 1
        Dim refx, refy(), pch() As String
        Dim t As Integer
        Dim sql As String = ""
        Dim refk() As String
        ' pgof = fm.getinfo2("select count(id) from emprec where '" & d1 & "' between hire_date and isnull(end_date,'" & d2 & "')", Session("con"))
        nod = Date.DaysInMonth(CDate(d1).Year, CDate(d2).Month)
        pch = payroll_checklist()

        '  refx = fm.getjavalist2("payrollx where pddate between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle') order by ref", " distinct ref", Session("con"), "")

        ' Response.Write(px)


        ref1 = ref1 & ref2
        refk = ref1.Split(",")
        pgof = refk.Length
        ' Response.Write(ref1.Replace(",", "<br>"))
        '  Response.Write("<br>" & pgof & "<br>")
        '  Response.Write(refk.Length & "<br>" & ref1)
        'ref = refx.Split("$")
        Dim empid As String
        ' Response.Write(pgof & "<br>" & fixed & "<br>" & pgof Mod fixed & "<br>")

        If IsNumeric(pgof) Then
            pgof = Math.Ceiling((CInt(pgof) / CInt(fixed))).ToString
        End If


        For p As Integer = 0 To UBound(refk)
            Try


                If refk(p) <> "" Then
                    If refk(p) = "571" Then
                        Response.Write("1)IN571<br>")
                    End If
                    ' Response.Write(refk(p) & "<br>")
                    sql = "select * from payrollx where emptid ='" & refk(p) & "' and date_paid between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle')"
                    rs = dbs.dtmake("vwpayroll", sql, Session("con"))
                    If rs.HasRows Then
                        ' outp(0) = headerx(month.ToString & "/1/" & year.ToString, pc, pe)

                        While rs.Read
                            empid = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                            'Response.Write(empid)
                            If i = 1 Then
                                outp(1) &= "<div class='page'><div class='subpage'><div style='font-size:6pt; height:5px;'>page" & pageno.ToString & "/" & pgof.ToString & "</div> " & header2(month.ToString & " / 1 / " & year.ToString, pageno, pgof)
                            Else
                                If i Mod fixed = 0 Then
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3' >&nbsp;</td><td>&nbsp</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td style='" & Chr(13) & _
                  "border:1px solid black'>ከአባሪ ቅጾች የመጣ</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & _
                  "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    For t = 0 To 7
                                        coltotal(t) = colsum(t) + coltra(t)
                                    Next
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3' >&nbsp;</td><td style='" & Chr(13) & _
                  "border:1px solid black'>ድምር</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                    outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center' style='" & Chr(13) & _
                  "border:1px solid black'>" & በታች() & "</td></tr></tbody></table></div></div></div>"
                                    pageno += 1
                                    outp(1) &= "<div class='page'><div class='subpage'><div style='font-size:6pt; height:5px;'>page" & pageno.ToString & "/" & pgof.ToString & "</div> " & header2(month.ToString & " / 1 / " & year.ToString, pageno, pgof)
                                    For t = 0 To colsum.Length - 1
                                        coltra(t) = coltotal(t)
                                        colsum(t) = 0
                                    Next

                                End If
                            End If
                            cell(0) = i.ToString
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

                            cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                            cell(4) = rs.Item("b_e")
                            cell(5) = rs.Item("pen_e")
                            cell(6) = rs.Item("pen_c")
                            sumpen = CDbl(cell(5)) + CDbl(cell(6))
                            cell(7) = sumpen.ToString
                            cell(8) = "&nbsp"
                            color = ""
                            title = ""
                            If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                                If CDate(d2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                    color = "red"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                    r_count += 1
                                    rlist &= rs.Item("emptid") & ","
                                Else
                                    'color = "green"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                End If

                            End If
                            outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
                            For j = 0 To 8

                                If j = 0 Then
                                    outp(1) &= " <td" & Chr(13) & _
                  " style='" & Chr(13) & _
                  "border:1px solid black'>" & cell(j) & " </td>" & Chr(13)
                                ElseIf j = 2 Then
                                    outp(1) &= " <td style='" & Chr(13) & _
                  "border:1px solid black;width:220px;'>" & cell(j) & " </td>" & Chr(13)
                                ElseIf j = 1 Then
                                    outp(1) &= " <td class='tin' style='" & Chr(13) & _
                 "border:1px solid black;width:40px;'>" & cell(j) & " </td>" & Chr(13)
                                ElseIf j = 8 Then

                                    outp(1) &= "  <td class='csssign'  style='" & Chr(13) & _
                  "border:1px solid black'>&nbsp;" & _
 fm.getproj(refk(p), d1, d2, Session("con"))(0) & "(" & refk(p) & ")" & rs.Item("ref") & "</td>" & Chr(13)
                                ElseIf j <= 3 Then
                                    outp(1) &= "<td class='nnrow'  style='" & Chr(13) & _
                  "border:1px solid black'>" & cell(j) & "&nbsp;</td>" & Chr(13)

                                Else
                                    outp(1) &= "<td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & Chr(13) & _
                                 FormatNumber(cell(j), 2, TriState.True, TriState.True, TriState.True) & "          </td>" & Chr(13)
                                    colsum(j) += CDbl(cell(j))
                                End If
                            Next
                            outp(1) &= "</tr>"
                            i = i + 1
                        End While
                    Else
                        rs2 = dbs.dtmake("vwpayroll", "select * from payrollx where emptid ='" & refk(p) & "' and date_paid between '" & CDate(d1).AddMonths(-1).ToShortDateString & "' and '" & CDate(d2).AddMonths(-1).ToShortDateString & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
                        If refk(p) = "571" Then
                            Response.Write("2)IN571<br>")
                        End If
                        If rs2.HasRows Then
                            rs2.Read()
                            empid = fm.getinfo2("select emp_id from emprec where id='" & rs2.Item("emptid") & "'", Session("con"))

                            If i = 1 Then
                                outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                            Else
                                If i Mod fixed = 0 Then
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3' >&nbsp;</td><td  style='" & Chr(13) & _
                  "border:1px solid black'>&nbsp</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    For t = 0 To 7
                                        coltotal(t) = colsum(t) + coltra(t)
                                    Next
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td style='" & Chr(13) & _
                  "border:1px solid black'>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                    outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center' style='" & Chr(13) & _
                  "border:1px solid black'>" & በታች() & "</td></tr></table></div></div>"
                                    pageno += 1
                                    outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                    For t = 0 To colsum.Length - 1
                                        coltra(t) = coltotal(t)
                                        colsum(t) = 0
                                    Next

                                End If
                            End If
                            Dim nods As Integer = 0
                            color = ""
                            title = ""
                            If IsDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)) Then
                                If CDate(d2).Subtract(CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                    color = "#abcdef"
                                    title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                                    r_count += 1
                                    rlist &= rs2.Item("emptid") & ","
                                Else

                                    title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                                End If
                                nods = CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)).Subtract(CDate(d1)).Days

                                ' Response.Write(nods.ToString)
                            Else
                                nods = nod
                                color = "#abcdef"
                            End If

                            cell(0) = i.ToString
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

                            cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs2.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                            cell(4) = (CDbl(rs2.Item("b_e")) / nod) * nods
                            cell(5) = (CDbl(rs2.Item("pen_e")) / nod) * nods
                            cell(6) = (CDbl(rs2.Item("pen_c")) / nod) * nods
                            sumpen = CDbl(cell(5)) + CDbl(cell(6))
                            cell(7) = sumpen.ToString
                            cell(8) = "&nbsp(" & nods & ")"


                            outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
                   "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;" & _
                   fm.getproj(refk(p), d1, d2, Session("con"))(0) & "(" & refk(p) & ")" & "</td>" & Chr(13)
                                ElseIf j <= 3 Then
                                    outp(1) &= "<td class='nnrow'>" & cell(j) & "&nbsp;</td>" & Chr(13)

                                Else
                                    outp(1) &= "<td class='numb' style='" & Chr(13) & _
                  "border:1px solid black;'>" & Chr(13) & _
                                 FormatNumber(cell(j), 2, TriState.True, TriState.True, TriState.True) & "          </td>" & Chr(13)
                                    colsum(j) += CDbl(cell(j))
                                End If
                            Next
                            outp(1) &= "</tr>"

                            i = i + 1
                        Else
                            If refk(p) = "571" Then
                                Response.Write("4)IN571<br>")
                            End If
                        End If
                        rs2.Close()

                    End If
                    rs.Close()

                End If
            Catch ex As Exception
                Response.Write(ex.ToString & "<br>" & sql)
            End Try
        Next
        If pageno > 0 Then
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td style='" & Chr(13) & _
                  "border:1px solid black'>&nbsp</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td  style='" & Chr(13) & _
                  "border:1px solid black'>ከአባሪ ቅጾች የመጣ</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            For t = 0 To 7
                coltotal(t) = colsum(t) + coltra(t)
            Next
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td style='" & Chr(13) & _
                  "border:1px solid black'>ድምር</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'  style='" & Chr(13) & _
                  "border:1px solid black'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr>"

            outp(1) &= "</tbody></table></div></div></div>"
            outp(2) = "<div id='summery'><div class='page'><div class='subpage'><div style='font-size:6pt; height:5px;'>page Summery</div> " & header2(month.ToString & " / 1 / " & year.ToString, pgof, pgof)
            outp(2) &= summerypension(i - 1, coltotal(4), coltotal(5), coltotal(6), coltotal(7), rlist)

        End If
        outp(2) &= "</tbody></table></div>" & Chr(13) & "</div></div>" & Chr(13)
        outp(0) &= outp(1)
        outp(1) = outp(2)
        ' Response.Write(i.ToString & "===>" & pgof.ToString & "==" & fixed.ToString)
        Session("page") = outp(0)
        Return outp
    End Function
    Function pensionall_c_new(ByVal ref1 As String, ByVal ref2 As String, ByRef month As Integer, ByRef year As Integer)
        '   Response.Write(ref2 & "<br>dsfsdfsd")
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))

        Dim cell(9) As String
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim sumpen As Double
        Dim rs, rs2 As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        Dim j As Integer
        Dim d1 As String = month & "/1/" & year
        Dim d2 As String = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        ' Response.Write(ref)
        Dim ref() As String
        Dim count As Integer
        Dim fixed As Integer
        Dim title As String
        Dim nod As Integer
        Dim rlist As String = ""
        Dim r_count As Integer = 0
        Dim color As String = "white"
        Dim pgof As String = ""
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        count = 0
        Dim pageno As Integer = 1
        Dim refx, refy(), pch() As String
        Dim t As Integer
        Dim sql As String = ""
        Dim refk() As String
        ' pgof = fm.getinfo2("select count(id) from emprec where '" & d1 & "' between hire_date and isnull(end_date,'" & d2 & "')", Session("con"))
        nod = Date.DaysInMonth(CDate(d1).Year, CDate(d2).Month)
        pch = payroll_checklist()

        '  refx = fm.getjavalist2("payrollx where pddate between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle') order by ref", " distinct ref", Session("con"), "")

        ' Response.Write(px)
        ' Response.Write(ref1 & "<br>" & ref2 & "<br>")

        ref1 = ref1 & ref2
        refk = ref1.Split(",")
        pgof = refk.Length
        ' Response.Write(ref1.Replace(",", "<br>"))
        '  Response.Write("<br>" & pgof & "<br>")
        '  Response.Write(refk.Length & "<br>" & ref1)
        'ref = refx.Split("$")
        Dim empid As String
        ' Response.Write(pgof & "<br>" & fixed & "<br>" & pgof Mod fixed & "<br>")
        While pgof Mod fixed > 5
            fixed -= 1
            Response.Write("<br>" & fixed)
        End While
        If IsNumeric(pgof) Then
            pgof = Math.Ceiling((CInt(pgof) / CInt(fixed))).ToString
        End If


        For p As Integer = 0 To UBound(refk)
            Try


                If refk(p) <> "" Then
                    If refk(p) = "571" Then
                        Response.Write("1)IN571<br>")
                    End If
                    ' Response.Write(refk(p) & "<br>")
                    sql = "select * from payrollx where emptid ='" & refk(p) & "' and date_paid between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle')"
                    rs = dbs.dtmake("vwpayroll", sql, Session("con"))
                    If rs.HasRows Then
                        ' outp(0) = headerx(month.ToString & "/1/" & year.ToString, pc, pe)

                        While rs.Read
                            empid = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                            'Response.Write(empid)
                            If i = 1 Then
                                outp(1) &= "<div class='page'><div class='subpage'><div style='font-size:6pt; height:5px;'>page" & pageno.ToString & "/" & pgof.ToString & "</div> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                            Else
                                If i Mod fixed = 0 Then
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    For t = 0 To 7
                                        coltotal(t) = colsum(t) + coltra(t)
                                    Next
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                    outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr></table></div></div>"
                                    pageno += 1
                                    outp(1) &= "<div class='page'><div class='subpage'><div style='font-size:6pt; height:5px;'>page" & pageno.ToString & "/" & pgof.ToString & "</div> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                    For t = 0 To colsum.Length - 1
                                        coltra(t) = coltotal(t)
                                        colsum(t) = 0
                                    Next

                                End If
                            End If
                            cell(0) = i.ToString
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

                            cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                            cell(4) = rs.Item("b_e")
                            cell(5) = rs.Item("pen_e")
                            cell(6) = rs.Item("pen_c")
                            sumpen = CDbl(cell(5)) + CDbl(cell(6))
                            cell(7) = sumpen.ToString
                            cell(8) = "&nbsp"
                            color = ""
                            title = ""
                            If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                                If CDate(d2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                    color = "red"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                    r_count += 1
                                    rlist &= rs.Item("emptid") & ","
                                Else
                                    'color = "green"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                End If

                            End If
                            outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
                                ElseIf j = 1 Then
                                    outp(1) &= " <td class='tin' style='" & Chr(13) & _
                 "border:1px solid black;width:220px;'>" & cell(j) & " </td>" & Chr(13)
                                ElseIf j = 8 Then

                                    outp(1) &= "  <td class='csssign' style='" & Chr(13) & _
                   "border-right:double 2.25pt black;border-left:solid 1px black;border-bottom:solid 1.0pt black;" & Chr(13) & _
                   "mso-border-bottom-alt:solid .5pt;mso-border-right-alt:solid .5pt;" & Chr(13) & _
                   "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px;font-size:7px;'>&nbsp;" & _
 fm.getproj(refk(p), d1, d2, Session("con"))(0) & "(" & refk(p) & ")" & rs.Item("ref") & "</td>" & Chr(13)
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
                    Else
                        rs2 = dbs.dtmake("vwpayroll", "select * from payrollx where emptid ='" & refk(p) & "' and date_paid between '" & CDate(d1).AddMonths(-1).ToShortDateString & "' and '" & CDate(d2).AddMonths(-1).ToShortDateString & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
                        If refk(p) = "571" Then
                            Response.Write("2)IN571<br>")
                        End If
                        If rs2.HasRows Then
                            rs2.Read()
                            empid = fm.getinfo2("select emp_id from emprec where id='" & rs2.Item("emptid") & "'", Session("con"))

                            If i = 1 Then
                                outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                            Else
                                If i Mod fixed = 0 Then
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    For t = 0 To 7
                                        coltotal(t) = colsum(t) + coltra(t)
                                    Next
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                    outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr></table></div></div>"
                                    pageno += 1
                                    outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                    For t = 0 To colsum.Length - 1
                                        coltra(t) = coltotal(t)
                                        colsum(t) = 0
                                    Next

                                End If
                            End If
                            Dim nods As Integer = 0
                            color = ""
                            title = ""
                            If IsDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)) Then
                                If CDate(d2).Subtract(CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                    color = "#abcdef"
                                    title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                                    r_count += 1
                                    rlist &= rs2.Item("emptid") & ","
                                Else

                                    title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                                End If
                                nods = CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)).Subtract(CDate(d1)).Days

                                ' Response.Write(nods.ToString)
                            Else
                                nods = nod
                                color = "#abcdef"
                            End If

                            cell(0) = i.ToString
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

                            cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs2.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                            cell(4) = (CDbl(rs2.Item("b_e")) / nod) * nods
                            cell(5) = (CDbl(rs2.Item("pen_e")) / nod) * nods
                            cell(6) = (CDbl(rs2.Item("pen_c")) / nod) * nods
                            sumpen = CDbl(cell(5)) + CDbl(cell(6))
                            cell(7) = sumpen.ToString
                            cell(8) = "&nbsp(" & nods & ")"


                            outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
                   "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;" & _
                   fm.getproj(refk(p), d1, d2, Session("con"))(0) & "(" & refk(p) & ")" & "</td>" & Chr(13)
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
                        Else
                            If refk(p) = "571" Then
                                Response.Write("4)IN571<br>")
                            End If
                        End If
                        rs2.Close()

                    End If
                    rs.Close()

                End If
            Catch ex As Exception
                Response.Write(ex.ToString & "<br>" & sql)
            End Try
        Next
        If pageno > 0 Then
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            For t = 0 To 7
                coltotal(t) = colsum(t) + coltra(t)
            Next
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr>"
            outp(1) &= "<tr><td colspan='9'>" & summerypension(i - 1, coltotal(4), coltotal(5), coltotal(6), coltotal(7), rlist) & "</td></tr>"

        End If
        outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
        ' Response.Write(i.ToString & "===>" & pgof.ToString & "==" & fixed.ToString)
        Session("page") = outp(0)
        Return outp(0)
    End Function
    Public Function headerx(ByVal datef As Date, ByVal pc As String, ByVal pe As String) As String
        Dim rtn As String
        Dim dtm As New dtimex
        rtn = " <table border='5' bordercolor='black' cellpadding='0' cellspacing='0' style='border-collapse:" & Chr(13) & _
 "collapse;width:960px;'>" & Chr(13)

        rtn &= "<tr height='21' style='height:15.75px'>" & Chr(13) & _
         "   <td colspan='9' style='font-size:10pt; font-weight:bold; border:4px red solid; height:20px'>" & _
"<img src='images/gif/pensiontop.gif' />" & Chr(13) & _
                 "</td>" & _
                 "</tr>" & Chr(13)
        rtn &= "<tr height='22' style='height:16.5px'>" & Chr(13) & _
       "   <td class='style11' colspan='9' height='22'  style='font-size:10pt; font-weight:bold;'> " & Chr(13) & Chr(13) & _
        "     ክፍል -1- " & Chr(13) & _
          "        የጡረታ መዋጮውን የሚከፍለው ድርጅት ዝርዝር መረጃ" & Chr(13) & _
          "</td>" & Chr(13) & Chr(13) & _
      "</tr><tr><td colspan='9'>" & _
      "<table style='width:960px;' border=1 cellspacing=0 cellpadding=0 >" & Chr(13) & Chr(13)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        rtn &= "<tr>" & _
           " <td  colspan='6'>" & _
             "   <b>  1. የግብር ከፋይ ስም :" & Chr(12) & "</b>" & Session("company_name_amharic") & " </td>" & _
            "<td colspan='3'>" & _
                "<b> 3.የግብር ከፋይመለያ ቁጥር </b><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & Chr(12) & Session("tin") & "</td>" & _
            "<td colspan='2'>" & _
               " <b>4. የግብር ሂሳብ ቁጥር</b></td>" & _
            "<td colspan='2'> "
        Dim dtc As String = dtm.convert_to_ethx(datef.AddDays(30))

        rtn &= " <b>5. የክፍያ ጊዜ:</b><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " & dtm.getmonth(dtc.Split(".")(1), "amh") & "," & dtc.Split(".")(2) & "</td>" & _
             "<td colspan='1' > " & _
              "  page <u>" & pc & " </u> of <u>" & pe & "</u>" & _
              "</td>" & _
        "</tr>" & _
       " <tr >" & _
            "<td colspan=2>" & _
               " <b>2. ክልል</b> አ.አ</td>" & _
                          "<td colspan=4> <b>2b. ዞነ ክ/ከተማ</b><br>" & Session("scity") & _
              "</td>" & _
                        "<td colspan='5'>" & _
                "<b>5. ግብር ሰብሳቢ መ/ቤትስም</b></td>" & _
                 "<td style='border-right:none; vertical-align:top;' rowspan=2 colspan=3> <b>               የሰነድ ቁጥር(ለቢሮ አገልግሎት)</b></td>" & _
                             "</tr>" & _
        "<tr style='height:10px;'>" & _
           " <td colspan='2' style='height:10px;'>" & _
              " <b> 2c. ወረዳ</b><br>" & Session("w") & "</td> " & _
           " <td colspan=2 ><b>2d. ቀበሌ/ገ/ማህበር </b></td>" & _
            "<td colspan=2><b>2e. የቤት ቁጥር</b><br>" & Session("hno") & "</td>" & _
            "<td colspan='2'><b>6. ሰልክ ቁጥር</b><br>" & Session("tel") & "</td>" & _
            "<td colspan='3'> <b> 7. የፋክስ ቁጥር</b></td>" & _
                    "</tr> </table></td></tr>   "
        '   rtn &= "<tr height='22' style='height:16.5px'>" & Chr(13) & _
        '    "   <td class='style11' colspan='9' height='22'  style='font-size:10pt; font-weight:bold;'> " & Chr(13) & Chr(13) & _
        '    "     ክፍል -1- " & Chr(13) & _
        '     "        የጡረታ መዋጮውን የሚከፍለው ድርጅት ዝርዝር መረጃ" & Chr(13) & _
        '    "</td>" & Chr(13) & Chr(13) & _
        '    "</tr>" & Chr(13) & Chr(13) & _
        '   "<tr class='cinfo' height='21' style='height:15.75px'  style='font-size:9pt; font-weight:bold;'>" & Chr(13) & Chr(13) & _
        '   "   <td class='style14' colspan='9' height='42' style='font-size:9pt; font-weight:bold;'>..." & Chr(13) & _
        '   "</td>" & Chr(13) & _
        ' "</tr>" & Chr(13) & Chr(13) & _
        '   Chr(13)
        rtn &= "<tr  style='font-size:10pt; font-weight:bold;'>" & Chr(13) & Chr(13) & _
              "                    <td class='style11' colspan='9' >" & Chr(13) & _
          "                    ሠንጠረዥ - 2 ማስወቂያ ዝርዝር መረጃ" & Chr(13) & _
           "                 </td>" & Chr(13) & Chr(13) & _
           "       </tr>" & "      <tr  style='font-size:8pt; font-weight:bold;height:15.75px'>" & Chr(13) & Chr(13) & _
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
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ስም" & Chr(13) & _
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
    Public Function header2(ByVal datef As Date, ByVal pc As String, ByVal pe As String) As String
        Dim rtn As String
        Dim dtm As New dtimex
        rtn = " <div class='top1'><table border='5' bordercolor='black' cellpadding='0' cellspacing='0' style='border-collapse:" & Chr(13) & _
 "collapse;width:960px;'>" & Chr(13)

        rtn &= "<tr height='21' style='height:15.75px'>" & Chr(13) & _
         "   <td colspan='9' style='font-size:10pt; font-weight:bold; border:4px red solid; height:20px'>" & _
"<img src='images/gif/pensiontop.gif' />" & Chr(13) & _
                 "</td>" & _
                 "</tr>" & Chr(13)
        rtn &= "<tr height='22' style='height:16.5px'>" & Chr(13) & _
       "   <td class='style11' colspan='9' height='22'  style='font-size:10pt; font-weight:bold;'> " & Chr(13) & Chr(13) & _
        "     ክፍል -1- " & Chr(13) & _
          "        የጡረታ መዋጮውን የሚከፍለው ድርጅት ዝርዝር መረጃ" & Chr(13) & _
          "</td>" & Chr(13) & Chr(13) & _
      "</tr></table></div>" & _
      "<div class='top2'><table style='width:960px;' border=1 cellspacing=0 cellpadding=0 >" & Chr(13) & Chr(13)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        rtn &= "<tr>" & _
           " <td  colspan='3'>" & _
             "   <b>  1. የግብር ከፋይ ስም :" & Chr(12) & "</b>" & Session("company_name_amharic") & " </td>" & _
            "<td colspan='2'>" & _
                "<b> 3.የግብር ከፋይመለያ ቁጥር </b><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & Chr(12) & Session("tin") & "</td>" & _
            "<td colspan='1'>" & _
               " <b>4. የግብር ሂሳብ ቁጥር</b></td>" & _
            "<td colspan='2'> "
        Dim dtc As String = dtm.convert_to_ethx(datef.AddDays(30))

        rtn &= " <b>5. የክፍያ ጊዜ:</b><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " & dtm.getmonth(dtc.Split(".")(1), "amh") & "," & dtc.Split(".")(2) & "</td>" & _
             "<td colspan='1' > " & _
              "  page <u>" & pc & " </u> of <u>" & pe & "</u>" & _
              "</td>" & _
        "</tr>" & _
       " <tr >" & _
            "<td colspan=2>" & _
               " <b>2. ክልል</b> አ.አ</td>" & _
                          "<td colspan=2> <b>2b. ዞነ ክ/ከተማ</b><br>" & Session("scity") & _
              "</td>" & _
                        "<td colspan='3'>" & _
                "<b>5. ግብር ሰብሳቢ መ/ቤትስም</b></td>" & _
                 "<td style='border-right:none; vertical-align:top;' rowspan=2 colspan=3> <b>               የሰነድ ቁጥር(ለቢሮ አገልግሎት)</b></td>" & _
                             "</tr>" & _
        "<tr style='height:10px;'>" & _
           " <td colspan='2' style='height:10px;'>" & _
              " <b> 2c. ወረዳ</b><br>" & Session("w") & "</td> " & _
           " <td colspan=2 ><b>2d. ቀበሌ/ገ/ማህበር </b></td>" & _
            "<td colspan=2><b>2e. የቤት ቁጥር</b><br>" & Session("hno") & "</td>" & _
            "<td colspan='2'><b>6. ሰልክ ቁጥር</b><br>" & Session("tel") & "</td>" & _
            "<td colspan='3'> <b> 7. የፋክስ ቁጥር</b></td>" & _
                    "</tr> </table></div><div class='top3'><table>   "
        '   rtn &= "<tr height='22' style='height:16.5px'>" & Chr(13) & _
        '    "   <td class='style11' colspan='9' height='22'  style='font-size:10pt; font-weight:bold;'> " & Chr(13) & Chr(13) & _
        '    "     ክፍል -1- " & Chr(13) & _
        '     "        የጡረታ መዋጮውን የሚከፍለው ድርጅት ዝርዝር መረጃ" & Chr(13) & _
        '    "</td>" & Chr(13) & Chr(13) & _
        '    "</tr>" & Chr(13) & Chr(13) & _
        '   "<tr class='cinfo' height='21' style='height:15.75px'  style='font-size:9pt; font-weight:bold;'>" & Chr(13) & Chr(13) & _
        '   "   <td class='style14' colspan='9' height='42' style='font-size:9pt; font-weight:bold;'>..." & Chr(13) & _
        '   "</td>" & Chr(13) & _
        ' "</tr>" & Chr(13) & Chr(13) & _
        '   Chr(13)
        rtn &= "<tr  style='font-size:10pt; font-weight:bold;'>" & Chr(13) & Chr(13) & _
              "                    <td class='style11' colspan='9' >" & Chr(13) & _
          "                    ሠንጠረዥ - 2 ማስወቂያ ዝርዝር መረጃ" & Chr(13) & _
           "                 </td>" & Chr(13) & Chr(13) & _
           "       </tr></table></div><div class='top4'><table style='width:960px;' border='1' cellspacing=0 cellpadding=0><thead>" & "      <tr>" & Chr(13) & Chr(13) & _
                "         <td>" & Chr(13) & Chr(13) & _
        "ሀ) ተ.ቁ" & Chr(13) & _
        "                 </td><td class='stylex' style=''>" & Chr(13) & Chr(13) & _
        "                   ለ)የሠራተኛው የግብር <br>ከፋይ መለያ ቁጥር(TIN)" & Chr(13) & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style='width:220px;' >" & Chr(13) & _
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ስም" & Chr(13) & _
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" & Chr(13) & _
         "                <td  class='stylex' style=''>" & Chr(13) & _
        "                    መ)የተቀጠሩበት<br> ቀን ቀን/ወር/ዓ.ም" & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style=''>" & Chr(13) & _
        "                    ሠ)የወር ደመወዝ<br>/ብር/" & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style='' >" & Chr(13) & _
        "                    ረ) የሰራተኛው መዋጮ <br>መጠን <br>" & pe & "%/ብር/" & Chr(13) & _
        "                 </td>" & Chr(13) & _
         "                <td class='stylex' style=''>" & Chr(13) & _
        "                     ሰ)የአሰሪው መዋጮ <br>መጠን <br>" & pc & "% /ብር" & Chr(13) & _
           "                 </td>" & Chr(13) & _
            "                <td class='stylex' style=''>"

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
             "              <td class='style2' style=''>" & Chr(13) & _
           "                  የሠራተኛ ፊርማ  " & Chr(13) & _
           "                 </td>" & Chr(13) & _
            "      </tr></thead><tbody>"

        Return rtn
    End Function
    Function unpaidlist(ByVal arrx As String, ByVal arry As String, ByVal pd1 As Date, ByVal pd2 As Date)
        Dim fm As New formMaker
        Dim rdate As Date
        Dim arrpd(), arrall() As String
        Dim rtn As String = ""
        arrpd = arrx.Split(",")
        arrall = arry.Split(",")

        For i As Integer = 0 To arrall.Length - 1
            If fm.searcharray(arrpd, arrall(i)) Then

            Else
                If fm.isResign(arrall(i), Session("con"))(0) = "n" Then
                    rdate = fm.isResign(arrall(i), Session("con"))(1)
                    If pd1.Subtract(rdate).Days < 0 Then
                        rtn &= arrall(i) & ","

                        '   Response.Write(rdate & "====>" & arrall(i))
                    End If
                Else
                    rtn &= arrall(i) & ","
                End If

            End If
        Next
        Return rtn
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
    Function getvewlist(ByVal projname As String, ByVal pdate1 As Date, ByVal pdate2 As Date) 'pension
        ' Dim pdate1, pdate2 As Date
        Dim nod As Integer
        Dim paidlist As String = ""
        Dim fl As New file_list
        Dim sec As New k_security
        Dim namelist As String = ""
        Dim chklist As String
        ' Dim cell(17) As Object
        Dim rtn(8) As String
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
            rrr = dbs.dtmake("payrol", "select distinct ref from payrollx where date_paid between '" & pdate1 & "' and '" & pdate2 & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))

            Dim emid, rtnvalue, eml() As String
            ' Response.Write(pdate2)
            rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
            rtn(6) = rtnvalue

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
                            rtn(7) = rrr.Item("ref")
                            If String.IsNullOrEmpty(Session("right")) = False Then
                                fx = Session("right").split(";")
                                ReDim Preserve fx(UBound(fx) + 1)
                                fx(UBound(fx) - 1) = ""
                            End If

                            'rtn(1) = "on"


                            If fm.searcharray(fx, "1") Or fm.searcharray(fx, "9") Then
                                If emid <> "''" Then
                                    cct += CInt(ccout)
                                    paidlist = paidlist.Replace("""", "")
                                    paidlist = paidlist.Replace(Chr(13), "")
                                    rtn(1) = "on"
                                    rtn(0) &= ("onclick = " & Chr(34) & Chr(13) & _
                                        "javascript:gotopension('view','" & paidlist & "');" & Chr(34))
                                    rtn(3) &= paidlist 'rrr.Item(0) & "|"
                                End If

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
        Dim nod As Integer
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


                        If fm.searcharray(fx, "1") Or fm.searcharray(fx, "9") Then
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


    Function pensionall_excel(ByRef ref2 As String, ByRef month As Integer, ByRef year As Integer)

        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        Dim cell(9) As String
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim sumpen As Double
        Dim rs, rs2 As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        Dim j As Integer
        Dim d1 As String = month & "/1/" & year
        Dim d2 As String = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        ' Response.Write(ref)
        Dim ref() As String
        Dim count As Integer
        Dim fixed As Integer
        Dim title As String
        Dim nod As Integer
        Dim rlist As String = ""
        Dim r_count As Integer = 0
        Dim color As String = "white"
        Dim pgof As String = ""
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        count = 0
        Dim pageno As Integer = 1
        Dim refx As String
        Dim t As Integer

        pgof = fm.getinfo2("select count(id) from emprec where '" & d1 & "' between hire_date and isnull(end_date,'" & d2 & "')", Session("con"))
        'outp(1) &= "<div class='page'>"
        refx = fm.getjavalist2("payrollx where pddate between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle') order by ref", " distinct ref", Session("con"), "")
        refx = getjavalist2("emprec where '" & d1 & "' between hire_date and isnull(end_date,'" & d2 & "')", "id", Session("con"), "", "$")
        ' Response.Write(refx.Replace(Chr(34), ""))
        refx = refx.Replace("""", "")
        ref = refx.Split("$")
        ' Response.Write(refx)
        Dim empid As String
        ' Response.Write(pgof & "<br>" & fixed & "<br>" & pgof Mod fixed & "<br>")
        While pgof Mod fixed > 7
            fixed -= 1
        End While
        If IsNumeric(pgof) Then
            pgof = Math.Ceiling((CInt(pgof) / CInt(fixed))).ToString
        End If
        nod = Date.DaysInMonth(CDate(d1).Year, CDate(d2).Month)
        outp(1) = ""

        For p As Integer = 0 To UBound(ref) - 2

            If ref(p) <> "" Then
                ' Response.Write(ref(p) & "<br>")
                rs = dbs.dtmake("vwpayroll", "select * from payrollx where emptid ='" & ref(p) & "' and date_paid between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
                If rs.HasRows Then
                    ' outp(0) = headerx(month.ToString & "/1/" & year.ToString, pc, pe)

                    While rs.Read
                        empid = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                        'Response.Write(empid)
                        If i = 1 Then
                            outp(1) &= headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                        Else
                            If i Mod fixed = 0 Then
                                outp(1) &= "<tr style='font-weight:bold'><td colspan='3' style='border:1pt black solid;'>&nbsp;</td><td style='border:1px black solid;'>&nbsp</td><td class='numb' style='border:1px black solid;'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                outp(1) &= "<tr  style='font-weight:bold'><td colspan='3' style='border:1pt black solid;'>&nbsp;</td><td style='border:1px black solid;'>ከአባሪ ቅጾች የመጣ</td><td class='numb' style='border:1px black solid;'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                For t = 0 To 7
                                    coltotal(t) = colsum(t) + coltra(t)
                                Next
                                outp(1) &= "<tr style='font-weight:bold'><td colspan='3' style='border:1pt black solid;'>&nbsp;</td><td>ድምር</td><td class='numb' style='border:1pt black solid;'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr></table><br><br>"
                                pageno += 1
                                outp(1) &= headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                For t = 0 To colsum.Length - 1
                                    coltra(t) = coltotal(t)
                                    colsum(t) = 0
                                Next

                            End If
                        End If
                        cell(0) = i.ToString
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

                        cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                        cell(4) = rs.Item("b_e")
                        cell(5) = rs.Item("pen_e")
                        cell(6) = rs.Item("pen_c")
                        sumpen = CDbl(cell(5)) + CDbl(cell(6))
                        cell(7) = sumpen.ToString
                        cell(8) = "&nbsp"
                        color = ""
                        title = ""
                        If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                            If CDate(d2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                color = "red"
                                title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                r_count += 1
                                rlist &= rs.Item("emptid") & ","
                            Else
                                'color = "green"
                                title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                            End If

                        End If
                        outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
               "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;" & fm.getproj(ref(p), d1, d2, Session("con"))(0) & "(" & ref(p) & ")" & "</td>" & Chr(13)
                            ElseIf j <= 3 Then
                                outp(1) &= "<td class='nnrow' style='border:1px black solid;'>" & cell(j) & "&nbsp;</td>" & Chr(13)

                            Else
                                outp(1) &= "<td class='numb' style='border:1px black solid;'>" & Chr(13) & _
                             FormatNumber(cell(j), 2, TriState.True, TriState.True, TriState.True) & "          </td>" & Chr(13)
                                colsum(j) += CDbl(cell(j))
                            End If
                        Next
                        outp(1) &= "</tr>"
                        i = i + 1
                    End While
                Else
                    rs2 = dbs.dtmake("vwpayroll", "select * from payrollx where emptid ='" & ref(p) & "' and date_paid between '" & CDate(d1).AddMonths(-1).ToShortDateString & "' and '" & CDate(d2).AddMonths(-1).ToShortDateString & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
                    If rs2.HasRows Then
                        rs2.Read()
                        empid = fm.getinfo2("select emp_id from emprec where id='" & rs2.Item("emptid") & "'", Session("con"))

                        If i = 1 Then
                            outp(1) &= headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                        Else
                            If i Mod fixed = 0 Then
                                outp(1) &= "<tr style='font-weight:bold'><td colspan='3' style='border:1pt black solid;'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border:1px solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                outp(1) &= "<tr  style='font-weight:bold'><td colspan='3' style='border:1pt black solid;'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1px solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                For t = 0 To 7
                                    coltotal(t) = colsum(t) + coltra(t)
                                Next
                                outp(1) &= "<tr style='font-weight:bold'><td colspan='3' style='border:1px black solid;'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1px solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr></table>"
                                pageno += 1
                                outp(1) &= headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                For t = 0 To colsum.Length - 1
                                    coltra(t) = coltotal(t)
                                    colsum(t) = 0
                                Next

                            End If
                        End If
                        Dim nods As Integer = 0
                        color = ""
                        title = ""
                        If IsDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)) Then
                            If CDate(d2).Subtract(CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                color = "#abcdef"
                                title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                                r_count += 1
                                rlist &= rs2.Item("emptid") & ","
                            Else

                                title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                            End If
                            nods = CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)).Subtract(CDate(d1)).Days

                            ' Response.Write(nods.ToString)
                        Else
                            nods = nod
                            color = "#abcdef"
                        End If

                        cell(0) = i.ToString
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

                        cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs2.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                        cell(4) = (CDbl(rs2.Item("b_e")) / nod) * nods
                        cell(5) = (CDbl(rs2.Item("pen_e")) / nod) * nods
                        cell(6) = (CDbl(rs2.Item("pen_c")) / nod) * nods
                        sumpen = CDbl(cell(5)) + CDbl(cell(6))
                        cell(7) = sumpen.ToString
                        cell(8) = "&nbsp(" & nods & ")"


                        outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
               "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;" & fm.getproj(ref(p), d1, d2, Session("con"))(0) & "(" & ref(p) & ")" & "</td>" & Chr(13)
                            ElseIf j <= 3 Then
                                outp(1) &= "<td class='nnrow'>" & cell(j) & "&nbsp;</td>" & Chr(13)

                            Else
                                outp(1) &= "<td class='numb'>" & Chr(13) & _
                             FormatNumber(cell(j), 2, TriState.True, TriState.True, TriState.True) & "          </td>" & Chr(13)
                                colsum(j) += CDbl(cell(j))
                            End If
                        Next
                        outp(1) &= "</tr>" & Chr(13)
                        i = i + 1
                    End If
                    rs2.Close()

                End If
                rs.Close()
            End If

        Next
        If pageno > 0 Then
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb' style='border:1px solid black;'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border:1px solid black;'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>" & Chr(13)
            outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb' style='border:1px solid black;'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border:1px solid black;'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>" & Chr(13)
            For t = 0 To 7
                coltotal(t) = colsum(t) + coltra(t)
            Next
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>" & Chr(13)
            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr>" & Chr(13)
            ' outp(1) &= "<tr><td colspan='9'>" & summerypension(i - 1, coltotal(4), coltotal(5), coltotal(6), coltotal(7), rlist) & "</td></tr>"

        End If
        outp(0) &= outp(1) & "</table>" & summerypension_excel(i - 1, coltotal(4), coltotal(5), coltotal(6), coltotal(7), rlist) & Chr(13)
        ' Response.Write(i.ToString & "===>" & pgof.ToString & "==" & fixed.ToString)
        Session("page") = outp(0)
        Return outp(0)
    End Function

    Function makepage_sec_tax(ByVal ref As String, ByVal pd1 As Date, ByVal pd2 As Date)
        Response.Write(pd1 & "---" & pd2 & "<br>")
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        Dim pstamt, psumtax, tpstamt, tpsumtax As Double
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
    Function makepage_tot_tax(ByVal ref As String, ByVal pd1 As Date, ByVal pd2 As Date)
        ' Response.Write(ref)
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        Dim pstamt, psumtax, tpstamt, tpsumtax As Double
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
            rs = dbs.dtmake("vwpayroll", "select * from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))
            '  Response.Write("select distnict emptid from payrollx where emptid in (" & ref & ") and pddate between '" & pd1 & "' and '" & pd2 & "' and remark in('monthly','pay_inc_middle')")
            ccc = 0
            Dim pdm As String = ""
            If rs.HasRows Then

                pno = 1
                ' outp(0) = headertax(month.ToString & "/1/" & year.ToString, pno.ToString, tpg.ToString)
                '  outp(1) &= "<div class='page'>"
                Dim inx As Integer = 0
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
                            title = getjavalist2("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'", "ref,txinco,tax", Session("con"), "=>", "|")
                            ' Response.Write("payrollx where emptid=" & rs.Item("emptid") & " and pddate between '" & pd1 & "' and '" & pd2 & "'<br>")

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
                   "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px;font-size:8px;' title='(" & rs.Item("emptid") & ")" & Chr(13) & rs.Item("ref") & "'>&nbsp;</td>" & Chr(13)

                    outp(1) &= "</tr>"
                    i = i + 1
                End While

                outp(1) &= "<tr style='font-weight:bold'><td colspan='8'>&nbsp;</td><td>&nbsp</td><td class='cssamt'>" & FormatNumber(colsum(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(colsum(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                outp(1) &= "<tr  style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ከአባሪ ቅጾች የመጣ</td><td class='cssamt'>" & FormatNumber(coltra(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt' style='border-right:1px solid black;'>" & FormatNumber(coltra(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                For t = 0 To 9
                    coltotal(t) = colsum(t) + coltra(t)
                Next
                outp(1) &= "<tr style='font-weight:bold'><td colspan='7'>&nbsp;</td><td colspan='2'>ድምር</td><td class='cssamt'>" & FormatNumber(coltotal(8), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='cssamt'  style='border-right:1px solid black;'>" & FormatNumber(coltotal(9), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr></table></div></div>"
                pageno += 1
                outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br><table>"
                outp(1) &= "<tr><td colspan='12'>" & summerytax(i - 1, coltotal(8), coltotal(9), rlist) & "</td></tr>"
                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='12' align='center'>" & በታች() & "</td></tr>"


                outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try

        Return outp(0)
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
        Response.Write(outp)
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
        Dim dbs As New dbclass
        ' dbs.pathserver = Server.MapPath(" ")
        ' Response.Write(dbs.pathserver.ToString)

        ' Dim rtn As Object
        '  rtn = dbs.Backup(Session("con"), dbs.pathserver & "/app_data/hrmp.mdf")
        '  Response.Write(rtn.ToString)
        dbs = Nothing
        Session("con").close()
        Session("con").open()
    End Sub
    Public Function getprojemp(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim did As String
        ' Response.Write("<br>select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')  order by id<br>")
        rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')  order by id", con)
        Dim d1, d2, de, ds As Date
        d2 = Nothing
        d1 = "#" & sdate.Month.ToString & "/1/" & sdate.Year.ToString & "#"
        Dim listid() As String = {""}
        Dim rtn As String = ""
        Dim rtn2 As String
        If rs.HasRows Then
            ' While rs.Read
            '   Response.Write("'" & rs.Item("emptid") & "',")
            'End While
            'Response.Write("<br>")
            'rs.Close()
            ' rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "'  order by id", con)
            rtn = ""
            Try
                'Response.Write(" start    ====     requested     ====    Date end<br>")
                While rs.Read
                    If rtn = "" Then
                        rtn = "'" & rs.Item("emptid") & "'"
                    Else
                        rtn &= ",'" & rs.Item("emptid") & "'"
                    End If


                End While
            Catch ex As Exception
                Response.Write(ex.ToString)
                '  rtn = "'',"
            End Try

        End If

        rs.Close()
        If rtn <> "" Then
            rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & d1 & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "') and emptid not in(" & rtn & ")  order by id", con)
        Else
            rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & d1 & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')   order by id", con)

        End If
        rtn2 = ""
        If rs.HasRows Then
            Try
                'Response.Write(" start    ====     requested     ====    Date end<br>")
                While rs.Read
                    If rtn2 = "" Then
                        If IsDate(fm.isResign(rs.Item(0), Session("con"))(1)) Then
                            Response.Write(sdate.Subtract(CDate(fm.isResign(rs.Item(0), Session("con"))(1))).Days.ToString & ",,,,,: :" & d1 & "....." & fm.isResign(rs.Item(0), Session("con"))(1) & "..." & rs.Item(0) & " <br>")
                            If sdate.Subtract(CDate(fm.isResign(rs.Item(0), Session("con"))(1))).Days > 0 And sdate.Subtract(CDate(fm.isResign(rs.Item(0), Session("con"))(1))).Days <= Date.DaysInMonth(sdate.Year, sdate.Month) Then
                                If IsNumeric(rs.Item(0)) Then
                                    rtn2 = "'" & rs.Item("emptid") & "'"
                                End If
                            End If
                        End If

                    Else
                        If IsDate(fm.isResign(rs.Item(0), Session("con"))(1)) Then
                            If sdate.Subtract(CDate(fm.isResign(rs.Item(0), Session("con"))(1))).Days > 0 And sdate.Subtract(CDate(fm.isResign(rs.Item(0), Session("con"))(1))).Days <= Date.DaysInMonth(sdate.Year, sdate.Month) Then
                                If rs.Item("emptid") <> "" Then
                                    rtn2 = ",'" & rs.Item("emptid") & "'"
                                End If
                            End If
                        End If
                    End If


                End While
            Catch ex As Exception
                Response.Write(ex.ToString)

            End Try
        End If
        dbs = Nothing

        If rtn <> "" And rtn2 <> "" Then
            rtn &= "," & rtn2
            ' Response.Write("<br>R1:" & rtn & ".....R2:" & rtn2.Length & "<br>")
        ElseIf rtn = "" Then
            rtn = rtn2

        End If
        'Response.Write(rtn & "<br>")
        Dim sp() As String = rtn.Split(",")
        'Response.Write(sp.Length)

        Return rtn
    End Function
    Public Function getprojempactive(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim did As String
        Response.Write("<br>select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')  order by id<br>")
        rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' and  '" & sdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "')  order by id", con)
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
    Function getemp_data(ByVal sql As String, ByVal sparater As String, ByVal dataformat As Boolean)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim rtn As String = ""
        Try

       
        rs = dbs.dtmake("mkx", sql, Session("con"))
        If rs.HasRows Then
            While rs.Read
                Select Case dataformat
                    Case True
                        If IsNumeric(rs.Item(0)) Then
                            rtn &= rs.Item(0) & sparater
                        End If

                    Case False
                        rtn &= "'" & rs.Item(0) & "'" & sparater
                    Case Else
                End Select
            End While


            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try
        If rtn <> "" Then
            rtn = rtn.Substring(0, rtn.Length - 1)

        End If
        Return rtn
    End Function
    Public Function headertax(ByVal datef As Date, ByVal pno As String, ByVal tp As String) As Array
        Dim rtn() As String = {"", ""}
        Dim dtm As New dtimex
        rtn(0) = "  <table border='0' cellpadding='0' cellspacing='0' style='font-size:8pt,width:29.7cm' border='1' bordercolor='black'>" & Chr(13) & _
            " <tr style='height:10px;'>" & Chr(13) & _
            "<td class='style41' style='height:10px;' >" & Chr(13) & _
              "  <img src='images\jpg\evel.jpg' width='65pt'/></td>" & Chr(13) & _
            "  <td class='style2' colspan='4' width='454' style='font-size:10pt;font-weight:bold;'>" & Chr(13) & _
               " የኢትዮጵያ ፌድራላዊ ዲሞክራሲ ሪሊክ  <br/> የኢትዮጵያ ገቢዎችና ጉሙሩክ</td>" & Chr(13) & _
            "<td class='style3' colspan='3' width='736' style='font-size:10pt;font-weight:bold;'>" & Chr(13) & _
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
        rtn(1) &= "<table cellpadding='0' cellspacing='0'>" & Chr(13) & _
                    " <tr style='height:15px;'>" & Chr(13) & _
                     "    <td class='style24' rowspan='2' style='width:2%'> (ሀ) ተ.ቁ</td>" & Chr(13) & _
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
        Dim dtm As New dtimex
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


    Function makepage(ByRef ref As String, ByRef month As Integer, ByRef year As Integer)
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        Dim cell(9) As String
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim sumpen As Double
        Dim rs, rs2 As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        Dim j As Integer
        Dim d1 As String = month & "/1/" & year
        Dim d2 As String = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        ' Response.Write(ref)

        Dim count As Integer
        Dim fixed As Integer
        Dim title As String
        Dim nod As Integer
        Dim rlist As String = ""
        Dim r_count As Integer = 0
        Dim color As String = "white"

        ' Response.Write(ref)

        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        count = 0
        Dim pageno As Integer = 1
        Dim sql As String = ""
        Dim empid As String
        Dim t As Integer = 0
        Dim pgof As Integer
        pgof = fm.getinfo2("select count(id) from payrollx where emptid in (" & ref.Substring(0, ref.Length - 1) & ") and pddate between '" & d1 & "' and '" & d2 & "' and remark in('monthly','pay_inc_middle')", Session("con"))



        While pgof Mod fixed > 7
            fixed -= 1
        End While
        If pgof Mod fixed = 0 Then
            '    Response.Write(pgof Mod fixed)
            fixed -= 1

        End If

        If IsNumeric(pgof) Then
            ' Response.Write(pgof / fixed & "<br>")
            pgof = Math.Ceiling((CInt(pgof) / CInt(fixed))).ToString
        End If

        nod = Date.DaysInMonth(CDate(d1).Year, CDate(d2).Month)

        ' Response.Write(pgof & "<br>" & fixed & "<br>" & pgof Mod fixed & "<br>")
        Try
            sql = "select * from payrollx where emptid in (" & ref.Substring(0, ref.Length - 1) & ") and pddate between '" & d1 & "' and '" & d2 & "' and remark in('monthly','pay_inc_middle')"
            rs = dbs.dtmake("vwpayroll", sql, Session("con"))
            If rs.HasRows Then
                ' outp(0) = headerx(month.ToString & "/1/" & year.ToString, pc, pe)
                outp(1) &= "<div class='page'>"
                While rs.Read
                    empid = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                    'Response.Write(empid)
                    If i = 1 Then
                        outp(1) &= "<div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                    Else
                        If i Mod fixed = 0 Then
                            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                            For t = 0 To 7
                                coltotal(t) = colsum(t) + coltra(t)
                            Next
                            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                            outp(1) &= "<tr style='text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr></table></div>"
                            pageno += 1
                            outp(1) &= "<div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                            For t = 0 To colsum.Length - 1
                                coltra(t) = coltotal(t)
                                colsum(t) = 0
                            Next

                        End If
                    End If
                    cell(0) = i.ToString
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

                    cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                    cell(4) = rs.Item("b_e")
                    cell(5) = rs.Item("pen_e")
                    cell(6) = rs.Item("pen_c")
                    sumpen = CDbl(cell(5)) + CDbl(cell(6))
                    cell(7) = sumpen.ToString
                    cell(8) = "&nbsp"
                    color = ""
                    title = ""
                    If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                        If CDate(d2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                            color = "red"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                            r_count += 1
                            rlist &= rs.Item("emptid") & ","
                        Else
                            'color = "green"
                            title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                        End If

                    End If
                    outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
           "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;" & fm.getproj(rs.Item("emptid"), d1, d2, Session("con"))(0) & "(" & rs.Item("emptid") & ")" & "</td>" & Chr(13)
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
            End If
        Catch ex As Exception
        End Try
        If pageno > 0 Then
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            For t = 0 To 7
                coltotal(t) = colsum(t) + coltra(t)
            Next
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr>"
            outp(1) &= "<tr><td colspan='9'>" & summerypension(i - 1, coltotal(4), coltotal(5), coltotal(6), coltotal(7), rlist) & "</td></tr>"

        End If
        outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
        ' Response.Write(i.ToString & "===>" & pgof.ToString & "==" & fixed.ToString)
        Session("page") = outp(0)
        Return outp(0)

    End Function
    Function makepage_all_tax(ByRef ref As String, ByRef pd1 As Date, ByRef pd2 As Date)
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        Dim pstamt, psumtax, tpstamt, tpsumtax As Double

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
        Return outp(0)
    End Function
    Function makepage_secx_tax(ByRef ref As String, ByRef pd1 As Date, ByRef pd2 As Date)
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        Dim pstamt, psumtax, tpstamt, tpsumtax As Double

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
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        Dim cell(9) As String
        Dim pstamt, psumtax, tpstamt, tpsumtax As Double
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
    Function pensionall(ByRef ref2() As String, ByRef month As Integer, ByRef year As Integer)

        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
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
        Dim d1 As String = month & "/1/" & year
        Dim d2 As String = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        ' Response.Write(ref)
        Dim ref() As String
        Dim count As Integer
        Dim fixed As Integer
        Dim title As String
        Dim r_count As Integer = 0
        Dim color As String = "white"
        Dim pgof As String = ""
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        count = 0
        Dim pageno As Integer = 1
        Dim refx As String
        Dim t As Integer

        pgof = fm.getinfo2("select count(id) from payrollx where pddate between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
        outp(1) &= "<div class='page'>"
        refx = fm.getjavalist2("payrollx where pddate between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle')", " distinct ref", Session("con"), "")
        refx = refx.Replace("""", "")
        ref = refx.Split(",")
        If IsNumeric(pgof) Then
            pgof = Math.Ceiling((CInt(pgof) / CInt(fixed))).ToString
        End If
        For p As Integer = 0 To UBound(ref) - 1

            If ref(p) <> "" Then
                ' Response.Write(ref(p) & "<br>")
                rs = dbs.dtmake("vwpayroll", "select * from payrollx where ref='" & ref(p) & "'", Session("con"))
                If rs.HasRows Then
                    ' outp(0) = headerx(month.ToString & "/1/" & year.ToString, pc, pe)

                    While rs.Read
                        Dim empid As String = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                        'Response.Write(empid)
                        If i = 1 Then
                            outp(1) &= "<div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                        Else
                            If i Mod fixed = 0 Then
                                outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                For t = 0 To 7
                                    coltotal(t) = colsum(t) + coltra(t)
                                Next
                                outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan=9>" & በታች() & "</td></tr></table></div>"
                                pageno += 1
                                outp(1) &= "<div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                For t = 0 To colsum.Length - 1
                                    coltra(t) = coltotal(t)
                                    colsum(t) = 0
                                Next

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

                        cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                        cell(4) = rs.Item("b_e")
                        cell(5) = rs.Item("pen_e")
                        cell(6) = rs.Item("pen_c")
                        sumpen = CDbl(cell(5)) + CDbl(cell(6))
                        cell(7) = sumpen.ToString
                        cell(8) = "&nbsp"
                        color = ""
                        title = ""
                        If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                            If CDate(d2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                color = "red"
                                title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                r_count += 1
                            Else
                                color = "green"
                                title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                            End If

                        End If
                        outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
               "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;" & ref(p) & "</td>" & Chr(13)
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

                End If

            End If

        Next
        outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
        Session("page") = outp(0)
        Return outp(0)
    End Function
    Function summerypension(ByVal nx As Integer, ByVal tsal As Double, ByVal tec As Double, ByVal tcc As Double, ByVal ttcc As Double, ByVal res As String)
        Dim rtn As String = ""
        Dim fm As New formMaker
        rtn = " <table Style='font-size:8pt;'><tr><td>" & _
        "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse;font-size:8pt;'>" & _
                     " <tr>" & _
            "    <td class='tn' colspan='3'>" & _
             "       ክፍል -3- የወሩ የተነቃለለ ሂሳብ</td>" & _
                        " </tr>" & _
            "<tr>" & _
             "   <td class='ldbl' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              "      10</td>" & _
               " <td class='nnrowx' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                "    በዚህ ወር ደመወዝ የሚከፈላቸው የሠራተኞች ብዛት</td>" & _
                "<td class='numbc' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                 nx.ToString & "</td>" & _
            "</tr>" & _
        "            <tr>" & _
         "       <td  class='ldbl' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
          "          20</td>" & _
           "     <td class='nnrowx' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
            "        የወሩ ጠቅላላ የሠራተኞች ደመወዝ ከሠንጠረዥ (ሠ)</td>" & _
             "   <td class='numbc' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              FormatNumber(tsal, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
            "<tr>" & _
             "   <td  class='ldbl' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              "      30</td>" & _
               " <td class='nnrowx' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                "    የወሩ ጠቅላላ የሠራተኞች መዋጮ መጠን ከሠንጠረዥ (ረ)</td>" & _
                "<td class='numbc' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                 FormatNumber(tec, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
            "<tr>" & _
             "   <td class='ldbl' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              "      40</td>" & _
               " <td class='nnrowx' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                "    የወሩ ጠቅላላ የአሰሪው መዋጮ መጠን ከሠንጠረዥ (ሰ)</td>" & _
                "<td class='numbc' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                 FormatNumber(tcc, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
            "<tr>" & _
             "   <td class='ldbl' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              "      50</td>" & _
               " <td class='nnrowx' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                "    የወሩ ጠቅላላ ከአሰሪው የሚገባ ጥቅል መዋጮ መጠን ከሠንጠረዥ (ሸ)</td>" & _
                "<td class='numbc' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                 FormatNumber(ttcc, 2, TriState.True, TriState.True, TriState.True) & "   </td>" & _
            "</tr>" & _
        "</table>" & _
        "</td><td style='width:20px;' >&nbsp;</td><td>" & _
         "   <table border='0' cellpadding='0' cellspacing='0' style='border-collapse:" & _
 "collapse; width:400px;font-size:8pt'>" & _
  "                     <tr height='21'>" & _
        "            <td class='tn' colspan=3>" & _
         "               ክፍል -4- በዚህ ወር የሥራ ውላቸው የተቋረጠ ሠራተኞች ዝርዝር መረጃ</td>" & _
          "         </tr>" & _
               " <tr>" & _
                "    <td class='lheads' style='" & Chr(13) & _
                  "border:1px solid black;width:7%;'>" & _
                 "       ተ.ቁ</td>" & _
                  "  <td class='heads' style='" & Chr(13) & _
                  "border:1px solid black;width:23%;'>" & _
                   "    የሠራተኛው የግብር <br>ከፋይ መለያ ቁጥር(TIN)" & _
                  "  </td>" & _
                   " <td class='headsn' style='" & Chr(13) & _
                  "border:1px solid black;width:69%;'>" & _
        "                        የሠራተኛው ስም:የአባት ስም፡ የአያት ስም" & _
        "                    </td>" & _
         "       </tr>"
        Dim empid As String = ""
        Dim emptid() As String = res.Split(",")
        Dim cell() As String = {"", "", ""}
        For j As Integer = 0 To emptid.Length - 1
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
                "    <td class='ldbl' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                (j + 1) & "</td>" & _
                  "  <td  class='nnrowx' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                            cell(1) & " </td>" & _
                    "<td  class='fname' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
                    cell(2) & "  &nbsp;</td>" & _
               " </tr>"
        Next


        rtn &= "</table>" & _
        "</td><td style='" & Chr(13) & _
                  "border:1px solid black;width:20px;'>&nbsp;</td><td>" & _
        "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:" & _
 "collapse; border:2pt double black; font-size:8pt;'>" & _
  "                 <tr >" & _
      "          <td class='nnrow' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
       "             የተከፈለበት ቀን</td><td class='nnrow' style='" & Chr(13) & _
                  "border:1px solid black;'>&nbsp;</td>" & _
        "    </tr>" & _
         "   <tr >" & _
          "      <td class='nnrow' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
           "         የደረሰኝ ቁጥር</td><td class='nnrow'>&nbsp;</td>" & _
            "</tr>" & _
            "<tr >" & _
             "   <td class='nnrow' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              "      የገንዘብ ልክ</td><td class='nnrow' style='" & Chr(13) & _
                  "border:1px solid black;'>&nbsp;</td>" & _
            "</tr>" & _
            "<tr height='20'>" & _
             "   <td class='nnrow' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              "      ቼክ</td><td class='nnrow'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" & _
            "</tr>" & _
            "<tr >" & _
             "   <td class='nnrow' style='" & Chr(13) & _
                  "border:1px solid black;'>" & _
              "      የገንዘብ ተቀባይ ፊርማ</td><td class='nnrow'>&nbsp;</td>" & _
            "</tr>" & _
        "</table>" & _
        "</td></tr></table><br> " & _
         "   <table  cellpadding='0' cellspacing='0' style='border-collapse:" & _
" collapse;width:100%;border:2pt double black;'>" & _
  "                      <tr >" & _
                                "    <td style='text-align:center;font-size:11px;'>" & _
              " <img src='images/gif/pensiondown.gif' /></td>" & _
               "</tr>" & _
           "</table>"
        Return rtn
    End Function
    Function summerypension_excel(ByVal nx As Integer, ByVal tsal As Double, ByVal tec As Double, ByVal tcc As Double, ByVal ttcc As Double, ByVal res As String)
        Dim rtn As String = ""
        Dim fm As New formMaker
        rtn = " <table ><tr><td>" & _
        "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse;border:2pt double black;'>" & _
                     " <tr>" & _
            "    <td class='tn' colspan='3'>" & _
             "       ክፍል -3- የወሩ የተነቃለለ ሂሳብ</td>" & _
                        " </tr>" & _
            "<tr>" & _
             "   <td class='ldbl'>" & _
              "      10</td>" & _
               " <td class='nnrowx' style='width:21px;height:31px;'>" & _
                "   <span style='width:21px;height:31px;'> በዚህ ወር ደመወዝ የሚከፈላቸው የሠራተኞች ብዛት</span></td>" & _
                "<td class='numbc'>" & _
                 nx.ToString & "</td>" & _
            "</tr>" & _
        "            <tr>" & _
         "       <td  class='ldbl' >" & _
          "          20</td>" & _
           "     <td class='nnrowx' style='width:21px;height:31px;' >" & _
            "       <span style='width:21px;height:31px;'> የወሩ ጠቅላላ የሠራተኞች ደመወዝ ከሠንጠረዥ (ሠ)</span></td>" & _
             "   <td class='numbc'>" & _
              FormatNumber(tsal, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
            "<tr>" & _
             "   <td  class='ldbl'>" & _
              "      30</td>" & _
               " <td class='nnrowx' style='width:21px;height:31px;' >" & _
                "   <span style='width:21px;height:31px;'> የወሩ ጠቅላላ የሠራተኞች መዋጮ መጠን ከሠንጠረዥ (ረ)</span></td>" & _
                "<td class='numbc'>" & _
                 FormatNumber(tec, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
            "<tr>" & _
             "   <td class='ldbl'>" & _
              "      40</td>" & _
               " <td class='nnrowx' style='width:21px;height:31px;'>" & _
                "  <span style='width:21px;height:31px;'>  የወሩ ጠቅላላ የአሰሪው መዋጮ መጠን ከሠንጠረዥ (ሰ)</span></td>" & _
                "<td class='numbc'>" & _
                 FormatNumber(tcc, 2, TriState.True, TriState.True, TriState.True) & "</td>" & _
            "</tr>" & _
            "<tr>" & _
             "   <td class='ldbl'>" & _
              "      50</td>" & _
               " <td class='nnrowx' style='width:21px;height:31px;'>" & _
                " <span style='width:21px;height:31px;'>   የወሩ ጠቅላላ ከአሰሪው የሚገባ ጥቅል መዋጮ መጠን ከሠንጠረዥ (ሸ)</span></td>" & _
                "<td class='numbc'>" & _
                 FormatNumber(ttcc, 2, TriState.True, TriState.True, TriState.True) & "   </td>" & _
            "</tr>" & _
        "</table>" & _
        "</td><td>" & _
         "   <table border='0' cellpadding='0' cellspacing='0' style='border-collapse:" & _
 "collapse; width:400px;border:2pt double black;'>" & _
  "                     <tr height='21'>" & _
        "            <td class='tn' colspan=3>" & _
         "               ክፍል -4- በዚህ ወር የሥራ ውላቸው የተቋረጠ ሠራተኞች ዝርዝር መረጃ</td>" & _
          "         </tr>" & _
               " <tr>" & _
                "    <td class='lheads' style=''>" & _
                 "       ተ.ቁ</td>" & _
                  "  <td class='heads' style=''>" & _
                   "    የሠራተኛው የግብር <br>ከፋይ መለያ ቁጥር(TIN)" & _
                  "  </td>" & _
                   " <td class='headsn' style='height:40px;width:50px;' colspan='2'>" & _
        "                        የሠራተኛው ስም:" & Chr(13) & "የአባት ስም፡" & Chr(13) & " የአያት ስም" & _
        "                    </td>" & _
         "       </tr>"
        Dim empid As String = ""
        Dim emptid() As String = res.Split(",")
        Dim cell() As String = {"", "", ""}
        For j As Integer = 0 To emptid.Length - 1
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
                (j + 1) & "</td>" & _
                  "  <td  class='nnrowx'>&nbsp;" & _
                            cell(1) & " </td>" & _
                    "<td  class='fname' colspan='2'>" & _
                    cell(2) & "  &nbsp;</td>" & _
               " </tr>"
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
         "   <table  cellpadding='0' cellspacing='0' style='border-collapse:" & _
" collapse;width:100%;border:2pt double black;'>" & _
  "                      <tr >" & _
                   "<td class='style11' colspan='9' height='22' >" & _
                "ክፍል -5- የትክክለኛነት ማረጋገጫ</td>" & _
               "               </tr>" & _
              " <tr>" & _
               "    <td style='text-align:center;font-size:11pt;' colspan='5'>" & _
                "በላይ የተገለጸው መስወቂያና የተሰጠው መረጃ በሙሉ የተሞላና ትከከለኛ መሆኑን አረጋግጣለሁ፡፡<br> ትክክለኛ ያልሆነ መረጃ መቅረብ " & _
                "መግብር ሕጎችም ሆነ በወንጀለኛ መቅጫ ሕግ የሚስቀጣ መሆኑን ገነዘባለሁ፡፡</td>" & _
                 "  <td colspan='4'>" & _
                "የግብር ከፋይ/ህጋዊ ወኪል ስም<br>&nbsp;<br>&nbsp;<br>&nbsp;<br>&nbsp;<br></td>" & _
               "</tr>" & _
           "</table>"
        Return rtn
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
        Dim emptid() As String = res.Split(",")
        Dim cell() As String = {"", "", ""}
        For j As Integer = 0 To emptid.Length - 1
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
                (j + 1) & "</td>" & _
                  "  <td  class='nnrowx'>" & _
                            cell(1) & " </td>" & _
                    "<td  class='fname'>" & _
                    cell(2) & "  &nbsp;</td>" & _
               " </tr>"
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
         "   <table  cellpadding='0' cellspacing='0' style='border-collapse:" & _
" collapse;width:100%;border:2pt double black;'>" & _
  "                      <tr >" & _
                   "<td class='style11' colspan='2' height='22' >" & _
                "ክፍል -5- የትክክለኛነት ማረጋገጫ</td>" & _
               "               </tr>" & _
              " <tr>" & _
               "    <td style='text-align:center;font-size:11px;'>" & _
                "በላይ የተገለጸው መስወቂያና የተሰጠው መረጃ በሙሉ የተሞላና ትከከለኛ መሆኑን አረጋግጣለሁ፡፡<br> ትክክለኛ ያልሆነ መረጃ መቅረብ " & _
                "መግብር ሕጎችም ሆነ በወንጀለኛ መቅጫ ሕግ የሚስቀጣ መሆኑን ገነዘባለሁ፡፡</td>" & _
                 "  <td>" & _
                "የግብር ከፋይ/ህጋዊ ወኪል ስም<br>&nbsp;<br>&nbsp;<br>&nbsp;<br>&nbsp;<br></td>" & _
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
        Dim dtm As New dtimex
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
    Function makelist_sum()
        Dim nod As Integer
        Dim pdate1, pdate2 As Date
        Dim ref, empidactive, salarypaid, proj, empid As String
        Dim rs As DataTableReader
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim sql As String = ""

        If Request.Form("month") <> "" Then
            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
        Else
            nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))
            pdate1 = Request.QueryString("month") & "/1/" & Request.QueryString("year")
            pdate2 = Request.QueryString("month") & "/" & nod & "/" & Request.QueryString("year")
        End If
        If IsDate(pdate1) = True Then
            sql = "select distinct ref from payrollx where date_paid between '" & pdate1 & "' and '" & pdate2 & "' and (remark='monthly' or remark='pay_inc_middle')"
            rs = dbx.dtmake("vwpension", sql, Session("con"))
            If rs.HasRows Then
                ref = ""
                Dim color As String = ""
                color = "white"
                While rs.Read
                    ref &= rs.Item(0) & ","

                    Session(rs.Item(0)) = getproject(rs.Item(0), pdate1, pdate2)
                    If color = "white" Then
                        color = "#236498"
                    Else
                        color = "white"
                    End If
                    Response.Write("<div style='background:" & color & ";'><table><tr><td>" & rs.Item(0) & "</td></tr><tr><td>" & Session(rs.Item(0))(0) & "</td></tr><tr><td>" & _
                                   Session(rs.Item(0))(1) & "</td></tr><tr><td>" & Session(rs.Item(0))(5) & "</td></tr><tr><td>" & Session(rs.Item(0))(2) & "</td></tr><tr><td>" & Session(rs.Item(0))(3) & _
                                   "</td></tr><tr><td>" & Session(rs.Item(0))(4) & "</td></tr></table></div>")

                End While
                Response.Write(ref)
            End If

        End If
    End Function
    Function getproject(ByVal ref As String, ByVal pd1 As Date, ByVal pd2 As Date)
        Dim fm As New formMaker
        Dim rtn(6) As String
        Dim spl() As String
        'Response.Write(ref & pd1 & pd2 & "<br>")
        rtn(0) = fm.getinfo2("select emptid from payrollx where ref='" & ref & "'", Session("con"))
        If IsNumeric(rtn(0)) Then
            rtn(0) = fm.getproj(rtn(0), pd1, pd2, Session("con"))(0)
            ' Response.Write(rtn(0) & "<====proj<br>")


            rtn(1) = fm.getprojemp(rtn(0), pd2, Session("con"))
            rtn(2) = salarypaidlist(ref)
            rtn(3) = fm.getinfo2("select sum(pen_c) from payrollx where ref='" & ref & "'", Session("con"))
            rtn(4) = fm.getinfo2("select sum(pen_e) from payrollx where ref='" & ref & "'", Session("con"))
            rtn(5) = fm.getinfo2("select sum(b_e) from payrollx where ref='" & ref & "'", Session("con"))

        End If
        Return rtn
    End Function
    Function salarypaidlist(ByVal ref As String)
        Dim rs As DataTableReader
        Dim dbx As New dbclass
        Dim rtn As String = ""
        rs = dbx.dtmake("getpaid", "select emptid from payrollx where ref='" & ref & "'", Session("con"))
        If rs.HasRows Then
            While rs.Read
                rtn &= rs.Item(0) & ","

            End While
        End If
        If rtn <> "" Then
            rtn = rtn.Substring(0, (rtn.Length - 1))
        End If
        Return rtn
    End Function

    '''''''''''''''''''''''''''''''''''''Delete finally''''''''''''''''''''''
    '  Function createpage(ByVal ref As String, ByVal tax As String, ByVal state As String) '
    'Dim rtn As String = ""
    'Dim loc As String = Server.MapPath("download")
    '   loc = loc.Replace("\", "/")
    '
    '   Select Case tax
    ''      Case "pension"
    '        Select Case state
    '           Case "view"
    '              rtn = makepage(ref, Request.Item("month"), Request.Item("year"))
    ''''             Response.Write(rtn)
    '     Case "viewall"
    '        rtn = makepage(ref, Request.Item("month"), Request.Item("year"))
    '       Response.Write(rtn)
    '  Case "viewtotal"

    'Response.Write("ppp" & ref & "," & Request.Item("month"))
    '                    rtn = pensionall_excel(ref, Request.Item("month"), Request.Item("year"))
    '                   loc &= "/" & Now.Ticks.ToString & ".txt"
    '
    '                   File.WriteAllText(loc, rtn)
    '                  rtn = pensionall_b(ref, Request.Item("month"), Request.Item("year"))
    '                 rtn = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
    '   "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
    '      " display:block;' onclick=" & Chr(34) & "javascript:exportxx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc & "','export','2;3');" & Chr(34) & " >" & _
    '"<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div><div style=float:left;'><input type='checkbox' id='signc' name='signc' value='ok' onclick=" & Chr(34) & "javascript:checkedc('signc');" & Chr(34) & " /> Clear signature Part</div><div style='clear:left'></div><div id='print'>" & rtn & "</div>"
    '  ' " exportx('tax(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','xls','" & loc & "','export','2;3');" 
    '                     Response.Write("chk<br>" & rtn)
    '            End Select


    '   End Select
    'End Function
    Function pensionall_b(ByVal ref2 As String, ByRef month As Integer, ByRef year As Integer)
        ' Response.Write(ref2 & "<br>")
        Dim outp(2) As String
        Dim pc, pe As String
        Dim dtm As New dtimex
        Dim fm As New formMaker
        ' Response.Write("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by desc<br>")
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & month.ToString & "/1/" & year.ToString & "' order by id desc", Session("con"))
        Dim cell(9) As String
        Dim colsum() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltra() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim coltotal() As Double = {0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim sumpen As Double
        Dim rs, rs2 As DataTableReader
        Dim dbs As New dbclass
        Dim i As Integer = 1
        Dim j As Integer
        Dim d1 As String = month & "/1/" & year
        Dim d2 As String = month & "/" & Date.DaysInMonth(year, month) & "/" & year
        ' Response.Write(ref)
        Dim ref() As String
        Dim count As Integer
        Dim fixed As Integer
        Dim title As String
        Dim nod As Integer
        Dim rlist As String = ""
        Dim r_count As Integer = 0
        Dim color As String = "white"
        Dim pgof As String = ""
        If File.Exists("c:\temp\constline.kst") Then
            fixed = File.ReadAllText("c:\temp\constline.kst")
        Else
            fixed = 17
        End If
        count = 0
        Dim pageno As Integer = 1
        Dim refx, refy(), pch() As String
        Dim t As Integer
        Dim sql As String = ""
        Dim refk() As String
        ' pgof = fm.getinfo2("select count(id) from emprec where '" & d1 & "' between hire_date and isnull(end_date,'" & d2 & "')", Session("con"))
        nod = Date.DaysInMonth(CDate(d1).Year, CDate(d2).Month)
        pch = payroll_checklist()
        '  Response.Write(ref2)
        '  refx = fm.getjavalist2("payrollx where pddate between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle') order by ref", " distinct ref", Session("con"), "")
        refx = getjavalist2("emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where '" & d1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & d2 & "') order by emp_static_info.first_name", "emprec.id", Session("con"), "", "$")
        '  Response.Write(refx.Replace(Chr(34), ""))
        refx = refx.Replace("""", "")
        refx = refx.Replace("xx", "")
        refx = refx.Substring(0, refx.Length - 1)
        ref = refx.Split("$")
        refy = ref2.Split(",")
        sql = "payrollx where emptid in(" & refx.Replace("$", ",") & ") and (remark='monthly' or remark='pay_inc_middle') order by ref"
        ' Response.Write(sql)
        ' Response.Write(refx.Replace("$", ",") & "<br>")
        Dim px As String = ""
        px = getjavalist2("payrollx where emptid in(" & refx.Replace("$", ",") & ") and (remark='monthly' or remark='pay_inc_middle') and date_paid between '" & d1 & "' and '" & d2 & "'  order by id", "emptid", Session("con"), "", "$")
        px = px.Replace("""", "")
        px = px.Replace("xx", "")
        px = px.Replace("$", ",")
        If pch(3).Length > 0 Then
            '  Response.Write("unpaid emp:" & pch(3) & "<br>")
            pch(3) = pch(3).Replace("'", "")
            px = px & pch(3)
        End If
        ' Response.Write(px)

        refk = px.Split(",")
        px = getjavalist2("payrollx where emptid Not in(" & refx.Replace("$", ",") & ") and (remark='monthly' or remark='pay_inc_middle') and date_paid between '" & d1 & "' and '" & d2 & "'  order by id", "emptid", Session("con"), "", "$")
        '  Response.Write(px)
        pgof = refk.Length
        ' Response.Write("<br>" & pgof & "<br>")
        'ref = refx.Split("$")
        Dim empid As String
        ' Response.Write(pgof & "<br>" & fixed & "<br>" & pgof Mod fixed & "<br>")
        While pgof Mod fixed > 5
            fixed -= 1
            'Response.Write("<br>" & fixed)
        End While
        If IsNumeric(pgof) Then
            pgof = Math.Ceiling((CInt(pgof) / CInt(fixed))).ToString
        End If


        For p As Integer = 0 To refk.Length - 1
            Try


                If refk(p) <> "" Then
                    '  Response.Write(refk(p) & "<br>")
                    sql = "select * from payrollx where emptid ='" & refk(p) & "' and date_paid between '" & d1 & "' and '" & d2 & "' and (remark='monthly' or remark='pay_inc_middle')"
                    rs = dbs.dtmake("vwpayroll", sql, Session("con"))
                    If rs.HasRows Then
                        ' outp(0) = headerx(month.ToString & "/1/" & year.ToString, pc, pe)

                        While rs.Read
                            empid = fm.getinfo2("select emp_id from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                            'Response.Write(empid)
                            If i = 1 Then
                                outp(1) &= "<div class='page'><div class='subpage'><div style='font-size:6pt; height:5px;'>page" & pageno.ToString & "/" & pgof.ToString & "</div> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                            Else
                                If i Mod fixed = 0 Then
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    For t = 0 To 7
                                        coltotal(t) = colsum(t) + coltra(t)
                                    Next
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                    outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr></table></div></div>"
                                    pageno += 1
                                    outp(1) &= "<div class='page'><div class='subpage'><div style='font-size:6pt; height:5px;'>page" & pageno.ToString & "/" & pgof.ToString & "</div> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                    For t = 0 To colsum.Length - 1
                                        coltra(t) = coltotal(t)
                                        colsum(t) = 0
                                    Next

                                End If
                            End If
                            cell(0) = i.ToString
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

                            cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                            cell(4) = rs.Item("b_e")
                            cell(5) = rs.Item("pen_e")
                            cell(6) = rs.Item("pen_c")
                            sumpen = CDbl(cell(5)) + CDbl(cell(6))
                            cell(7) = sumpen.ToString
                            cell(8) = "&nbsp"
                            color = ""
                            title = ""
                            If IsDate(fm.isResign(rs.Item("emptid"), Session("con"))(1)) Then
                                If CDate(d2).Subtract(CDate(fm.isResign(rs.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                    color = "red"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                    r_count += 1
                                    rlist &= rs.Item("emptid") & ","
                                Else
                                    'color = "green"
                                    title = fm.isResign(rs.Item("emptid"), Session("con"))(1)
                                End If

                            End If
                            If cell(5) > 0 Then
                                outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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

                                        outp(1) &= "  <td class='sign' style='" & Chr(13) & _
                       "border-right:double 2.25pt black;border-left:solid 1px black;border-bottom:solid 1.0pt black;" & Chr(13) & _
                       "mso-border-bottom-alt:solid .5pt;mso-border-right-alt:solid .5pt;" & Chr(13) & _
                       "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px;font-size:7px;'>&nbsp;" & _
     fm.getproj(refk(p), d1, d2, Session("con"))(0) & "(" & refk(p) & ")" & rs.Item("ref") & "</td>" & Chr(13)
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
                            End If
                        End While
                    Else
                        rs2 = dbs.dtmake("vwpayroll", "select * from payrollx where emptid ='" & refk(p) & "' and date_paid between '" & CDate(d1).AddMonths(-1).ToShortDateString & "' and '" & CDate(d2).AddMonths(-1).ToShortDateString & "' and (remark='monthly' or remark='pay_inc_middle')", Session("con"))
                        If rs2.HasRows Then
                            rs2.Read()
                            empid = fm.getinfo2("select emp_id from emprec where id='" & rs2.Item("emptid") & "'", Session("con"))

                            If i = 1 Then
                                outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof.ToString & "<br> " & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                            Else
                                If i Mod fixed = 0 Then
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
                                    For t = 0 To 7
                                        coltotal(t) = colsum(t) + coltra(t)
                                    Next
                                    outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"

                                    outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr></table></div></div>"
                                    pageno += 1
                                    outp(1) &= "<div class='page'><div class='subpage'>page" & pageno.ToString & "/" & pgof & "<br>" & headerx(month.ToString & " / 1 / " & year.ToString, pc, pe)
                                    For t = 0 To colsum.Length - 1
                                        coltra(t) = coltotal(t)
                                        colsum(t) = 0
                                    Next

                                End If
                            End If
                            Dim nods As Integer = 0
                            color = ""
                            title = ""
                            If IsDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)) Then
                                If CDate(d2).Subtract(CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1))).Days >= 0 Then
                                    color = "#abcdef"
                                    title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                                    r_count += 1
                                    rlist &= rs2.Item("emptid") & ","
                                Else

                                    title = fm.isResign(rs2.Item("emptid"), Session("con"))(1)
                                End If
                                nods = CDate(fm.isResign(rs2.Item("emptid"), Session("con"))(1)).Subtract(CDate(d1)).Days

                                ' Response.Write(nods.ToString)
                            Else
                                nods = nod
                                color = "#abcdef"
                            End If

                            cell(0) = i.ToString
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

                            cell(3) = dtm.convert_to_ethx(CDate(fm.getinfo2("select hire_date from emprec where id=" & rs2.Item("emptid") & " order by hire_date desc", Session("con"))).ToShortDateString)
                            cell(4) = (CDbl(rs2.Item("b_e")) / nod) * nods
                            cell(5) = (CDbl(rs2.Item("pen_e")) / nod) * nods
                            cell(6) = (CDbl(rs2.Item("pen_c")) / nod) * nods
                            sumpen = CDbl(cell(5)) + CDbl(cell(6))
                            cell(7) = sumpen.ToString
                            cell(8) = "&nbsp(" & nods & ")"


                            outp(1) &= " <tr style='font-size:8pt;background:" & color & "' title='" & title & "'>" & Chr(13)
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
                   "mso-border-color-alt:windowtext;padding:0cm 5.4pt 0cm 5.4pt;height:15.75px'>&nbsp;" & _
                   fm.getproj(refk(p), d1, d2, Session("con"))(0) & "(" & refk(p) & ")" & "</td>" & Chr(13)
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
                        End If
                        rs2.Close()

                    End If
                    rs.Close()

                End If
            Catch ex As Exception
                Response.Write(ex.ToString & "<br>" & sql)
            End Try
        Next
        If pageno > 0 Then
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>&nbsp</td><td class='numb'>" & FormatNumber(colsum(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(colsum(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(colsum(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr  style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ከአባሪ ቅጾች የመጣ</td><td class='numb'>" & FormatNumber(coltra(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltra(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltra(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            For t = 0 To 7
                coltotal(t) = colsum(t) + coltra(t)
            Next
            outp(1) &= "<tr style='font-weight:bold'><td colspan='3'>&nbsp;</td><td>ድምር</td><td class='numb'>" & FormatNumber(coltotal(4), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(5), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb'>" & FormatNumber(coltotal(6), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='numb' style='border-right:1pt solid black;'>" & FormatNumber(coltotal(7), 2, TriState.True, TriState.True, TriState.True) & "</td></tr>"
            outp(1) &= "<tr style='height:37px;text-alight:bottom;'><td colspan='9' align='center'>" & በታች() & "</td></tr>"
            outp(1) &= "<tr><td colspan='9'>" & summerypension(i - 1, coltotal(4), coltotal(5), coltotal(6), coltotal(7), rlist) & "</td></tr>"

        End If
        outp(0) &= outp(1) & "</table>" & Chr(13) & "</div></div>" & Chr(13)
        ' Response.Write(i.ToString & "===>" & pgof.ToString & "==" & fixed.ToString)
        Session("page") = outp(0)
        Return outp(0)
    End Function
    Function arrayto(ByVal arrx As Array, ByVal spr As String)
        Dim rtn As String = ""
        If arrx.Length > 0 Then
            For i As Integer = 0 To arrx.Length - 1
                If arrx(i) <> "" Then
                    rtn &= arrx(i) & spr
                End If


            Next
            rtn = rtn.Substring(0, rtn.Length - 1)

        End If
        Return rtn

    End Function

End Class
Public Class dtimex
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



