Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class hrsummeryreport
    Inherits System.Web.UI.Page
    Public pd1, pd2 As String
    Function init()
        If IsDate(Request.Form("pd1")) Then
            pd1 = Request.Form("pd1")
            pd2 = Request.Form("pd2")
        ElseIf IsDate(Request.QueryString("pd1")) Then
            pd1 = Request.QueryString("pd1")
            pd2 = Request.QueryString("pd2")
        End If
        If IsDate(pd1) Then

            Select Case LCase(Request.Form("isotype"))
                Case "hired"
                    hired()
                Case "termination"
                    termination()
                Case "transfer"
                    transfer()
                Case "promotion"
                    promotion()
                Case "vacancy"
                Case Else


            End Select
        Else
            Response.Write(Request.Form("pd1"))
        End If
        If IsPostBack = True Then
            Response.Write("postback")
        End If
    End Function
    Function hired()

        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim sql As String
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim seck As String
        Dim nrows As Integer
        Dim ccount As Integer = 0
        Dim tpgno As Integer = 15
        Dim outp As String = ""
        Dim nperp As Integer = 15
        Dim page_count As Integer = 1
        Dim projid As String = ""
        Dim proj As String = ""
        Dim fname, hrdate, post, sal, allow, allow2, allow_, sex As String
        Dim fd(,) As String
        Dim edu, emp_status As String
        Dim csex, cterm As String
        Dim poost As String = ""
        csex = 0
        cterm = 0
        Dim max As Integer = 20
        Dim remarkx As String = ""

        sql = "select * from emprec where hire_date between '" & pd1 & "' and '" & pd2 & "' order by hire_date"
        Dim poostx As String = ""
        rs = dbs.dtmake("vwhire", sql, Session("con"))
        If rs.HasRows Then
            nrows = fm.getinfo2("select count(id) from emprec where hire_date between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
            If nrows > 15 Then
                tpgno = Math.Ceiling((nrows - 15) / max) + 1
            Else
                tpgno = 1
            End If
            ' tpgno = Math.Ceiling(nrows / nperp)
            ' Response.Write(nrows & "<<<\norows" & tpgno & "<<</total pages<br>")
            poost = ""
            Dim pgcont As Integer = 0
            While rs.Read
                If ccount = 0 Then
                    outp &= "<div id='p1' class='page'><div class='subpage' >" & header_first(1, tpgno, "Hired") & header_all(1, tpgno, "Hired")
                    outp &= table_header("hired")
                ElseIf pgcont Mod nperp = 0 Then
                    page_count = page_count + 1
                    outp &= "</table><br>" & footerpages() & "</div></div><div id='p" & page_count & "' class='page'><div class='subpage' >" & header_all(page_count, tpgno, "Hired")
                    outp &= table_header("hired")
                    pgcont = 0
                    nperp = max
                End If
                pgcont = pgcont + 1
                proj = ""
                fname = ""
                sex = ""
                hrdate = ""
                emp_status = ""
                remarkx = ""
                edu = ""
                projid = fm.getinfo2("select project_id from emp_job_assign where emp_id='" & rs.Item("emp_id") & "' order by id desc", Session("con"))
                proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                fname = fm.getfullname(rs.Item("emp_id"), Session("con"))
                sex = fm.getinfo2("select sex from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con"))
                fd = fm.getqualification(rs.Item("emp_id"), Session("con"))
                hrdate = rs.Item("hire_date")

                post = fm.getinfo2("select position from emp_job_assign where emp_id='" & rs.Item("emp_id") & "' order by id desc", Session("con"))
                If post <> "" And post <> "None" Then
                    seck = sec.Kir_StrToHex(post)
                    ' Response.Write("<br>" & seck & "===" & sec.Kir_HexToStr(seck) & "<br>")
                End If
                '  Response.Write(InStr(post, poostx) & "<br>")
                If InStr(poostx, post) > 0 Then
                    ' Response.Write(post & "existed<br>")
                    Session(seck) = Session(seck) + 1
                Else
                    Session(seck) = 1
                    poostx &= post & "|"
                    poost &= seck & "|"
                    'Response.Write("Reg" & poostx)
                End If


                If IsNumeric(Session(sex)) Then
                    Session(sex) = Session(sex) + 1
                Else
                    Session(sex) = 1
                    csex &= csex & "|"
                End If
                emp_status = fm.getinfo2("select active from emprec where id='" & rs.Item("id") & "'", Session("con"))
                If emp_status = "n" Then
                    emp_status = "Inactive"
                ElseIf emp_status = "y" Then
                    emp_status = "active"
                End If
                sal = fm.getinfo2("select basic_salary from emp_sal_info where emptid=" & rs.Item("id") & " order by date_start", Session("con"))
                edu = fd(4, 0)

                allow = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("id") & " and istaxable='n' and from_date='" & rs.Item("hire_date") & "' is null group by istaxable", Session("con"))
                If IsNumeric(allow) = False Then
                    allow = 0
                End If
                allow2 = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("id") & " and to_date is null and istaxable='y' and from_date='" & rs.Item("hire_date") & "' group by istaxable", Session("con"))
                If IsNumeric(allow2) = False Then
                    allow2 = 0
                End If

                If emp_status = "Inactive" Then
                    remarkx = fm.getinfo2("select reason from emp_resign where emptid=" & rs.Item("id"), Session("con"))
                End If

                allow_ = (CDbl(allow) + CDbl(allow2)).ToString
                outp &= " <tr>" & Chr(13) & _
           " <td nowrap width='35'>          <p class='ename'><span>" & (ccount + 1).ToString & "<o:p></o:p></span></p></td>" & Chr(13) & _
          "<td nowrap width='150'>    <p class='ename'><span>" & fname & "<o:p>&nbsp;</o:p></span></p></td>" & Chr(13) & _
          "<td nowrap width='110'> <p  class='ename '> <span>" & post & " <o:p>&nbsp;</o:p></span></p> </td>" & Chr(13) & _
            "<td nowrap width='45'><p  class='ename '><span>" & hrdate & "<o:p>&nbsp;</o:p></span></p></td>" & Chr(13) & _
           " <td nowrap width='22'><p class='ename '><span>" & sex & "<o:p>&nbsp;</o:p></span></p></td>" & Chr(13) & _
          "  <td nowrap width='150'><p class='ename'> <span>" & edu & "<o:p>&nbsp;</o:p></span></p> </td>" & Chr(13) & _
          "  <td nowrap width='198'> <p class='ename'><span>" & proj & "<o:p>&nbsp;</o:p></span></p>       </td>" & Chr(13) & _
           " <td nowrap width='22'> <p class='ename '><span>" & emp_status & "<o:p>&nbsp;</o:p></span></p></td>" & Chr(13) & _
            "<td nowrap width='71'><p class='cssamt'><span>" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.True) & "<o:p>&nbsp;</o:p></span></p></td>" & Chr(13) & _
            "<td nowrap width='71'><p class='cssamt'><span>" & FormatNumber(allow_, 2, TriState.True, TriState.True, TriState.True) & "<o:p>&nbsp;</o:p></span></p></td>" & Chr(13) & _
            "<td nowrap width='71'><p align='center' class='MsoNormal'><span>&nbsp;" & remarkx & "<o:p></o:p></span></p></td>" & Chr(13) & _
        " </tr>"
                ccount = ccount + 1
            End While
            outp &= "</table><br>" & footerpages() & "</div></div>"
            outp &= "<div class='page'><div class='subpage' ><table cellspacing=0 cellpadding=0><tr><td colspan='3'>Summery: Gender</td></tr><tr><td>Male</td><td>:</td><td>" & Session("male") & "</td></tr><tr><td>Female</td><td>:</td><td>" & Session("female") & "</td></tr></table><br>"
            Dim spl() As String
            spl = poost.Split("|")
            Array.Sort(spl)
            ' Response.Write(poost)
            If spl.Length > 0 Then
                outp &= "<table cellspacing=0 cellpadding=0><tr><td colspan=4>Summery by Position</td></tr><tr><td>no.</td><td>Position</td><td></td><td>No. Emp.</td></tr>"
                Dim j As Integer = 1
                For i As Integer = 0 To UBound(spl)
                    '   Response.Write("<br>" & sec.Kir_HexToStr(spl(i)))
                    If spl(i) <> "" Then
                        outp &= "<tr><td> " & j & "</td><td>" & sec.Kir_HexToStr(spl(i)) & "</td><td>:</td><td> " & Session(spl(i)) & "</td></tr>"
                        Session.Remove(spl(i))
                        j = j + 1
                    End If
                Next
                outp &= "</table>"
            End If
            outp &= "</div></div>"
            Session("male") = 0
            Session("female") = 0
            Dim loc As String = Session("path")
            loc &= "/download/iso" & Now.Ticks & ".txt"
            File.WriteAllText(loc, outp)
            outp = "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                         "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                        " display:block;' onclick=" & Chr(34) & "javascript:exportx('isohrm(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','doc,'" & loc & "','export','2;3');" & Chr(34) & " >" & _
                        "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                        "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                  " onclick=" & Chr(34) & "javascirpt:print('bigprintx','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printisoA4');" & Chr(34) & ">" & _
                             "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                            "<div style='clear:left;'></div><br>" & _
"<div id='bigprintx'>" & outp & "</div>"

            Response.Write(outp)
            Response.Write("<script>alert($('#p1').height());</script>")
        End If

    End Function
    Function termination()
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim sec As New k_security
        Dim sql As String
        Dim fm As New formMaker
        Dim nrows As Integer
        Dim ccount As Integer = 0
        Dim tpgno As Integer = 15
        Dim outp As String = ""
        Dim nperp As Integer = 15
        Dim page_count As Integer = 1
        Dim projid As String = ""
        Dim proj As String = ""
        Dim fname, hrdate, post, sal, allow, allow2, allow_, sex As String
        Dim fd(,) As String
        Dim edu, emp_status As String
        Dim csex, cterm As String
        Dim poost As String = ""
        csex = 0
        cterm = 0
        Dim max As Integer = 20
        Dim remarkx As String = ""
        sql = "select * from emprec where end_date between '" & pd1 & "' and '" & pd2 & "' order by end_date"
        '  Response.Write(sql)

        rs = dbs.dtmake("vwhire", sql, Session("con"))
        If rs.HasRows Then
            nrows = fm.getinfo2("select count(id) from emprec where end_date between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
            If nrows > 15 Then
                tpgno = Math.Ceiling((nrows - 15) / max) + 1
            Else
                tpgno = 1
            End If
            ' tpgno = Math.Ceiling(nrows / nperp)
            ' Response.Write(nrows & "<<<\norows" & tpgno & "<<</total pages<br>")
            poost = ""
            Dim seck As String
            Dim pgcont As Integer = 0
            While rs.Read
                If ccount = 0 Then
                    outp &= "<div id='p1' class='page'><div class='subpage' >" & header_first(1, tpgno, "termination") & header_all(1, tpgno, "Termination")
                    outp &= table_header("termination")
                ElseIf pgcont Mod nperp = 0 Then
                    page_count = page_count + 1
                    outp &= "</table><br>" & footerpages() & "</div></div><div id='p" & page_count & "' class='page'><div class='subpage' style='background-color:yellow;' >" & header_all(page_count, tpgno, "Termination")

                    outp &= table_header("termination")
                    pgcont = 0
                    nperp = max
                End If
                pgcont = pgcont + 1
                proj = ""
                fname = ""
                sex = ""
                hrdate = ""
                emp_status = ""
                remarkx = ""
                edu = ""
                projid = fm.getinfo2("select project_id from emp_job_assign where emp_id='" & rs.Item("emp_id") & "' order by id desc", Session("con"))
                proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                fname = fm.getfullname(rs.Item("emp_id"), Session("con"))
                sex = fm.getinfo2("select sex from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con"))
                fd = fm.getqualification(rs.Item("emp_id"), Session("con"))
                hrdate = rs.Item("end_date")

                post = fm.getinfo2("select position from emp_job_assign where emp_id='" & rs.Item("emp_id") & "' order by id desc", Session("con"))
                If post <> "" And post <> "None" Then
                    seck = sec.Kir_StrToHex(post)
                    ' Response.Write("<br>" & seck & "===" & sec.Kir_HexToStr(seck) & "<br>")
                End If

                If IsNumeric(Session(seck)) = True Then

                    Session(seck) = Session(seck) + 1
                    If Session(seck) <= 1 Then
                        poost &= seck & "|"
                    End If
                Else
                    Session(seck) = 1

                    poost &= seck & "|"
                End If
                If IsNumeric(Session(sex)) Then
                    Session(sex) = Session(sex) + 1
                Else
                    Session(sex) = 1
                    csex &= csex & "|"
                End If
                emp_status = fm.getinfo2("select active from emprec where id='" & rs.Item("id") & "'", Session("con"))
                If emp_status = "n" Then
                    emp_status = "Inactive"
                ElseIf emp_status = "y" Then
                    emp_status = "active"
                End If
                sal = fm.getinfo2("select basic_salary from emp_sal_info where emptid=" & rs.Item("id") & " order by date_start", Session("con"))
                edu = fd(4, 0)
                If IsNumeric(sal) = False Then
                    sal = 0
                End If
                allow = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("id") & " and istaxable='n' and from_date='" & rs.Item("hire_date") & "' is null group by istaxable", Session("con"))
                If IsNumeric(allow) = False Then
                    allow = 0
                End If
                allow2 = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("id") & " and to_date is null and istaxable='y' and from_date='" & rs.Item("hire_date") & "' group by istaxable", Session("con"))
                If IsNumeric(allow2) = False Then
                    allow2 = 0
                End If

                If emp_status = "Inactive" Then
                    remarkx = fm.getinfo2("select reason from emp_resign where emptid=" & rs.Item("id"), Session("con"))
                End If

                allow_ = (CDbl(allow) + CDbl(allow2)).ToString
                outp &= " <tr>"
                outp &= " <td nowrap width='35' class='ename'>" & (ccount + 1).ToString & "</td>"
                outp &= "<td nowrap width='190' class='ename'>" & fname & "</td>"
                outp &= "<td nowrap width='190' class='ename'>" & post & "  </td>"
                outp &= "<td nowrap width='45' class='ename'>" & hrdate & "</td>"
                outp &= " <td nowrap width='22' class='ename'>" & sex & "</td>"
                outp &= "<td nowrap width='60' class='cssamt' style='text-align:right;'>" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                outp &= "<td nowrap width='71' class='cssamt' style='text-align:right;'>" & FormatNumber(allow_, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                outp &= "  <td nowrap width='198' class='ename'>" & proj & "      </td>"
                outp &= "<td nowrap width='71' align='left' class='ename'>&nbsp;" & remarkx & "<o:p></o:p></span></p></td>"
                outp &= " </tr>"
                ccount = ccount + 1
            End While
            outp &= "</table><br>" & footerpages() & "</div></div>"
            outp &= "<div class='page'><div class='subpage' ><table cellspacing=0 cellpadding=0><tr><td colspan='3'>Summery: Gender</td></tr><tr><td>Male</td><td>:</td><td>" & Session("male") & "</td></tr><tr><td>Female</td><td>:</td><td>" & Session("female") & "</td></tr></table><br>"
            Dim spl() As String
            spl = poost.Split("|")
            Array.Sort(spl)
            ' Response.Write(poost)
            If spl.Length > 0 Then
                outp &= "<table cellspacing=0 cellpadding=0><tr><td colspan=4>Summery by Position</td></tr><tr><td>no.</td><td>Position</td><td></td><td>No. Emp.</td></tr>"
                Dim j As Integer = 1
                For i As Integer = 0 To UBound(spl)
                    Response.Write("<br>" & sec.Kir_HexToStr(spl(i)))
                    If spl(i) <> "" Then
                        outp &= "<tr><td> " & j & "</td><td>" & sec.Kir_HexToStr(spl(i)) & "</td><td>:</td><td> " & Session(spl(i)) & "</td></tr>"
                        Session.Remove(spl(i))
                        j = j + 1
                    End If
                Next
                outp &= "</table>"
            End If
            outp &= "</div></div>"
            Session("male") = 0
            Session("female") = 0
            Dim loc As String = Session("path")
            loc &= "/download/iso" & Now.Ticks & ".txt"
            File.WriteAllText(loc, outp)
            outp = "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                         "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                        " display:block;' onclick=" & Chr(34) & "javascript:exportx('isohrm(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','doc,'" & loc & "','export','2;3');" & Chr(34) & " >" & _
                        "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                        "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                  " onclick=" & Chr(34) & "javascirpt:print('bigprintx','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printisoA4');" & Chr(34) & ">" & _
                             "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                            "<div style='clear:left;'></div><br>" & _
"<div id='bigprintx'>" & outp & "</div>"
            Response.Write(outp)
            Response.Write("<script>alert($('#p1').height());</script>")
        End If
        For Each k As String In Session.StaticObjects
            '  Response.Write(k & ".................<br>")
        Next
    End Function

    Function transfer()
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim sec As New k_security
        Dim sql As String
        Dim fm As New formMaker
        Dim nrows As Integer
        Dim ccount As Integer = 0
        Dim tpgno As Integer = 15
        Dim outp As String = ""
        Dim nperp As Integer = 15
        Dim page_count As Integer = 1
        Dim projid As String = ""
        Dim proj As String = ""
        Dim fname, hrdate, post, sal, allow, allow2, allow_, sex As String
        Dim fd(,) As String
        Dim edu, emp_status As String
        Dim csex, cterm As String
        Dim poost As String = ""
        csex = 0
        cterm = 0
        Dim max As Integer = 20
        Dim remarkx As String = ""
        Dim transproj As String = ""
        sql = "select * from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Job Assignment' order by date_from"
        '  Response.Write(sql)
        sql = "select emp_job_assign.*,first_name,middle_name,emp_static_info.emp_id from emp_job_assign " & _
            "inner join emprec on emprec.id=emp_job_assign.emptid inner join " & _
            "emp_static_info on emprec.emp_id=emp_static_info.emp_id where " & _
           " date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Job Assignment' order by emp_static_info.first_name"
        rs = dbs.dtmake("vwhire", sql, Session("con"))
        If rs.HasRows Then
            Try
                sql = "select count(id) from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Job Assignment'"
                nrows = fm.getinfo2("select count(id) from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Job Assignment'", Session("con"))

            Catch ex As Exception
                Response.Write(ex.ToString & sql)
                nrows = 0
            End Try
            If nrows > 15 Then
                tpgno = Math.Ceiling((nrows - 15) / max) + 1
            Else
                tpgno = 1
            End If
            ' tpgno = Math.Ceiling(nrows / nperp)
            ' Response.Write(nrows & "<<<\norows" & tpgno & "<<</total pages<br>")
            poost = ""
            Dim seck As String
            Dim pgcont As Integer = 0
            Dim chkname As String = ""
            While rs.Read
                transproj = ""
                If ccount = 0 Then
                    outp &= "<div class='page'><div class='subpage' >" & header_first(1, tpgno, "Transfer") & header_all(1, tpgno, "Transfer")
                    outp &= table_header("transfer")
                ElseIf pgcont Mod nperp = 0 Then
                    page_count = page_count + 1
                    outp &= "</table><br>" & footerpages() & "</div></div><div class='page'><div class='subpage' >" & header_all(page_count, tpgno, "Transfer")

                    outp &= table_header("transfer")
                    pgcont = 0
                    nperp = max
                End If
                pgcont = pgcont + 1
                proj = ""
                fname = ""
                sex = ""
                hrdate = ""
                emp_status = ""
                remarkx = ""
                edu = ""
                projid = fm.getinfo2("select project_id from emp_job_assign where id='" & rs.Item("id") & "' order by id desc", Session("con"))
                ' proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                fname = fm.getfullname(rs.Item("emp_id"), Session("con"))
                sex = fm.getinfo2("select sex from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con"))
                ' fd = fm.getqualification(rs.Item("emp_id"), Session("con"))
                hrdate = rs.Item("date_from")
                ' Response.Write(hrdate & "<br>")
                Try
                    transproj = fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid=" & rs.Item("emptid") & " and  date_from ='" & CDate(hrdate).ToShortDateString & "'", Session("con")) & "'", Session("con"))
                    proj = fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid=" & rs.Item("emptid") & " and  (date_end = '" & CDate(hrdate).AddDays(-1).ToShortDateString & "')", Session("con")) & "'", Session("con"))

                Catch ex As Exception
                    Response.Write("select project_name from tblproject where project_id='" & "select project_id from emp_job_assign where emptid=" & rs.Item("emptid") & " and  date_from ='" & hrdate & "'")
                End Try

                post = fm.getinfo2("select position from emp_job_assign where id='" & rs.Item("id") & "' order by id desc", Session("con"))
                If post <> "" And post <> "None" Then
                    seck = sec.Kir_StrToHex(post)
                    ' Response.Write("<br>" & seck & "===" & sec.Kir_HexToStr(seck) & "<br>")
                End If
                If fname <> chkname Then
                    chkname = fname
                    If IsNumeric(Session(seck)) = True Then

                        Session(seck) = Session(seck) + 1
                        If Session(seck) <= 1 Then
                            poost &= seck & "|"
                        End If
                    Else
                        Session(seck) = 1

                        poost &= seck & "|"
                    End If
                    If IsNumeric(Session(sex)) Then
                        Session(sex) = Session(sex) + 1
                    Else
                        Session(sex) = 1
                        csex &= csex & "|"
                    End If
                End If
                emp_status = fm.getinfo2("select active from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                If emp_status = "n" Then
                    emp_status = "Inactive"
                ElseIf emp_status = "y" Then
                    emp_status = "active"
                End If
                sal = fm.getsal(rs.Item("emp_id"), rs.Item("date_from"), Session("con"))
                'sal = fm.getinfo2("select basic_salary from emp_sal_info where emptid=" & rs.Item("emptid") & " date_start between '" & rs.Item("date_from") & "' and isnull('" & rs.Item("date_end") & "','" & pd2 & "')", Session("con"))
                '  Response.Write("select basic_salary from emp_sal_info where emptid=" & rs.Item("emptid") & " date_start between '" & rs.Item("date_from") & "' and isnull('" & rs.Item("date_end") & "','" & pd2 & "')")
                If IsNumeric(sal) = False Then
                    sal = 0
                End If
                allow = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("emptid") & " and istaxable='n' and from_date between '" & pd1 & "' and '" & pd2 & "' group by istaxable", Session("con"))
                If IsNumeric(allow) = False Then
                    allow = 0
                End If
                allow2 = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("emptid") & " and istaxable='y' and from_date between '" & pd1 & "' and '" & pd2 & "' group by istaxable", Session("con"))
                If IsNumeric(allow2) = False Then
                    allow2 = 0
                End If

                If emp_status = "Inactive" Then
                    remarkx = fm.getinfo2("select reason from emp_resign where emptid=" & rs.Item("emptid"), Session("con"))
                End If

                allow_ = (CDbl(allow) + CDbl(allow2)).ToString
                outp &= " <tr>"
                outp &= " <td nowrap width='35' class='ename'>" & (ccount + 1).ToString & "</td>"
                outp &= "<td nowrap width='190' class='ename'>" & fname & "</td>"
                outp &= "<td nowrap width='120' class='ename'>" & post & "  </td>"
                outp &= "<td nowrap width='90' class='ename'>" & hrdate & "  </td>"
                outp &= "<td nowrap width='190' class='ename'>" & proj & "</td>"
                outp &= " <td nowrap width='190' class='ename'>" & transproj & "</td>"
                outp &= "<td nowrap width='60' class='cssamt' style='text-align:right;'>" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                outp &= "<td nowrap width='71' class='cssamt' style='text-align:right;'>" & FormatNumber(allow_, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                '  outp &= "  <td nowrap width='198' class='ename'>" & proj & "      </td>"
                outp &= "<td nowrap width='71' align='left' class='ename'>&nbsp;" & remarkx & "</td>"
                outp &= " </tr>"
                ccount = ccount + 1
            End While
            outp &= "</table><br>" & footerpages() & "</div></div>"
            outp &= "<div class='page'><div class='subpage' ><table cellspacing=0 cellpadding=0><tr><td colspan='3'>Summery: Gender</td></tr><tr><td>Male</td><td>:</td><td>" & Session("male") & "</td></tr><tr><td>Female</td><td>:</td><td>" & Session("female") & "</td></tr></table><br>"
            Dim spl() As String
            spl = poost.Split("|")
            Array.Sort(spl)
            ' Response.Write(poost)
            If spl.Length > 0 Then
                outp &= "<table cellspacing=0 cellpadding=0><tr><td colspan=4>Summery by Position</td></tr><tr><td>no.</td><td>Position</td><td></td><td>No. Emp.</td></tr>"
                Dim j As Integer = 1
                For i As Integer = 0 To UBound(spl)
                    ' Response.Write("<br>" & sec.Kir_HexToStr(spl(i)))
                    If spl(i) <> "" Then
                        outp &= "<tr><td> " & j & "</td><td>" & sec.Kir_HexToStr(spl(i)) & "</td><td>:</td><td> " & Session(spl(i)) & "</td></tr>"
                        Session.Remove(spl(i))
                        j = j + 1
                    End If
                Next
                outp &= "</table>"
            End If
            outp &= "</div></div>"
            Session("male") = 0
            Session("female") = 0
            Dim loc As String = Session("path")
            loc &= "/download/iso" & Now.Ticks & ".txt"
            File.WriteAllText(loc, outp)
            outp = "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                         "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                        " display:block;' onclick=" & Chr(34) & "javascript:exportx('isohrm(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','doc,'" & loc & "','export','2;3');" & Chr(34) & " >" & _
                        "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                        "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                  " onclick=" & Chr(34) & "javascirpt:print('bigprintx','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printisoA4');" & Chr(34) & ">" & _
                             "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                            "<div style='clear:left;'></div><br>" & _
"<div id='bigprintx'>" & outp & "</div>"
            Response.Write(outp)
        End If
        For Each k As String In Session.StaticObjects
            '  Response.Write(k & ".................<br>")
        Next
    End Function
    Function promotion()
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim sec As New k_security
        Dim sql As String
        Dim fm As New formMaker
        Dim nrows As Integer
        Dim ccount As Integer = 0
        Dim tpgno As Integer = 15
        Dim outp As String = ""
        Dim nperp As Integer = 15
        Dim page_count As Integer = 1
        Dim projid As String = ""
        Dim proj As String = ""
        Dim fname, hrdate, post, sal, allow, allow2, allow_, sex As String
        Dim fd(,) As String
        Dim edu, emp_status As String
        Dim csex, cterm As String
        Dim poost As String = ""
        csex = 0
        cterm = 0
        Dim max As Integer = 20
        Dim remarkx As String = ""
        Dim transproj As String = ""
        sql = "select * from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Job Assignment' order by date_from"
        '  Response.Write(sql)
        sql = "select emp_job_assign.*,first_name,middle_name,emp_static_info.emp_id from emp_job_assign " & _
            "inner join emprec on emprec.id=emp_job_assign.emptid inner join " & _
            "emp_static_info on emprec.emp_id=emp_static_info.emp_id where " & _
           " date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Promotion' order by emp_static_info.first_name"
        rs = dbs.dtmake("vwhire", sql, Session("con"))
        If rs.HasRows Then
            Try
                sql = "select count(id) from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Promotion'"
                nrows = fm.getinfo2("select count(id) from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='promotion'", Session("con"))

            Catch ex As Exception
                Response.Write(ex.ToString & sql)
                nrows = 0
            End Try
            If nrows > 15 Then
                tpgno = Math.Ceiling((nrows - 15) / max) + 1
            Else
                tpgno = 1
            End If
            ' tpgno = Math.Ceiling(nrows / nperp)
            ' Response.Write(nrows & "<<<\norows" & tpgno & "<<</total pages<br>")
            poost = ""
            Dim seck As String
            Dim pgcont As Integer = 0
            Dim chkname As String = ""
            Dim post2, sal2, allow2_, allow_2 As String
            While rs.Read
                transproj = ""
                If ccount = 0 Then
                    outp &= "<div class='page'><div class='subpage' >" & header_first(1, tpgno, "Promotion") & header_all(1, tpgno, "Promotion")
                    outp &= table_header("promotion")
                ElseIf pgcont Mod nperp = 0 Then
                    page_count = page_count + 1
                    outp &= "</table><br>" & footerpages() & "</div></div><div class='page'><div class='subpage' >" & header_all(page_count, tpgno, "Promotion")

                    outp &= table_header("promotion")
                    pgcont = 0
                    nperp = max
                End If
                pgcont = pgcont + 1
                proj = ""
                fname = ""
                sex = ""
                hrdate = ""
                emp_status = ""
                post2 = ""
                sal2 = ""
                allow_2 = ""
                allow2_ = ""
                remarkx = ""
                allow2 = ""
                edu = ""
                projid = fm.getinfo2("select project_id from emp_job_assign where id='" & rs.Item("id") & "' order by id desc", Session("con"))
                ' proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                fname = fm.getfullname(rs.Item("emp_id"), Session("con"))
                sex = fm.getinfo2("select sex from emp_static_info where emp_id='" & rs.Item("emp_id") & "'", Session("con"))
                ' fd = fm.getqualification(rs.Item("emp_id"), Session("con"))
                hrdate = rs.Item("date_from")
                ' Response.Write(hrdate & "<br>")
                Try
                    transproj = fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid=" & rs.Item("emptid") & " and  date_from ='" & CDate(hrdate).ToShortDateString & "'", Session("con")) & "'", Session("con"))
                Catch ex As Exception
                    Response.Write("select project_name from tblproject where project_id='" & "select project_id from emp_job_assign where emptid=" & rs.Item("emptid") & " and  date_from ='" & hrdate & "'")
                End Try

                post = fm.getinfo2("select position from emp_job_assign where id='" & rs.Item("id") & "' order by id desc", Session("con"))
                post2 = fm.getinfo2("select position from emp_job_assign where emptid=" & rs.Item("emptid") & " and date_end is not null order by date_end desc", Session("con"))
                If post <> "" And post <> "None" Then
                    seck = sec.Kir_StrToHex(post)
                    ' Response.Write("<br>" & seck & "===" & sec.Kir_HexToStr(seck) & "<br>")
                End If
                If fname <> chkname Then
                    chkname = fname
                    If IsNumeric(Session(seck)) = True Then

                        Session(seck) = Session(seck) + 1
                        If Session(seck) <= 1 Then
                            poost &= seck & "|"
                        End If
                    Else
                        Session(seck) = 1

                        poost &= seck & "|"
                    End If
                    If IsNumeric(Session(sex)) Then
                        Session(sex) = Session(sex) + 1
                    Else
                        Session(sex) = 1
                        csex &= csex & "|"
                    End If
                End If
                emp_status = fm.getinfo2("select active from emprec where id='" & rs.Item("emptid") & "'", Session("con"))
                If emp_status = "n" Then
                    emp_status = "Inactive"
                ElseIf emp_status = "y" Then
                    emp_status = "active"
                End If
                sal = fm.getsal(rs.Item("emp_id"), rs.Item("date_from"), Session("con"))
                sal2 = fm.getsal(rs.Item("emp_id"), CDate(rs.Item("date_from")).AddDays(-3), Session("con"))
                ' Response.Write(fm.getinfo2("select basic_salary from emp_sal_info where emptid=" & rs.Item("emptid") & " and date_start<'" & CDate(rs.Item("date_from")).AddDays(-3).ToShortDateString & "' order by date_start desc", Session("con")) & "<br>")

                'sal = fm.getinfo2("select basic_salary from emp_sal_info where emptid=" & rs.Item("emptid") & " date_start between '" & rs.Item("date_from") & "' and isnull('" & rs.Item("date_end") & "','" & pd2 & "')", Session("con"))
                '  Response.Write("select basic_salary from emp_sal_info where emptid=" & rs.Item("emptid") & " date_start between '" & rs.Item("date_from") & "' and isnull('" & rs.Item("date_end") & "','" & pd2 & "')")
                If IsNumeric(sal) = False Then
                    sal = 0
                End If
                allow = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("emptid") & " and istaxable='n' and from_date between '" & pd1 & "' and '" & pd2 & "' group by istaxable", Session("con"))
                If IsNumeric(allow) = False Then
                    allow = 0
                End If
                allow2 = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("emptid") & " and istaxable='y' and from_date between '" & pd1 & "' and '" & pd2 & "' group by istaxable", Session("con"))
                If IsNumeric(allow2) = False Then
                    allow2 = 0
                End If

                If emp_status = "Inactive" Then
                    remarkx = fm.getinfo2("select reason from emp_resign where emptid=" & rs.Item("emptid"), Session("con"))
                End If

                allow_ = (CDbl(allow) + CDbl(allow2)).ToString
                If IsNumeric(sal2) = False Then
                    sal = 0
                End If
                allow2_ = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("emptid") & " and istaxable='n' and '" & CDate(pd1).AddDays(-2).ToShortDateString & "'between from_date   and isnull(end_date,'" & Today.ToShortDateString & "') group by istaxable", Session("con"))
                If IsNumeric(allow2_) = False Then
                    allow2_ = 0
                End If
                allow_2 = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("emptid") & " and istaxable='y' and '" & CDate(pd1).AddDays(-2).ToShortDateString & "'between from_date   and isnull(end_date,'" & Today.ToShortDateString & "') group by istaxable", Session("con"))
                If IsNumeric(allow_2) = False Then
                    allow_2 = 0
                End If

                If emp_status = "Inactive" Then
                    remarkx = fm.getinfo2("select reason from emp_resign where emptid=" & rs.Item("emptid"), Session("con"))
                End If

                allow_2 = (CDbl(allow_2) + CDbl(allow2_)).ToString
                outp &= " <tr>"
                outp &= " <td nowrap width='35' class='ename'>" & (ccount + 1).ToString & "</td>"
                outp &= "<td nowrap width='190' class='ename'>" & fname & "</td>"

                outp &= "<td nowrap width='50' class='ename'>" & hrdate & "  </td>"
                outp &= "<td nowrap width='120' class='ename'>" & post2 & "  </td>"
                outp &= "<td nowrap width='60' class='cssamt' style='text-align:right;'>" & FormatNumber(sal2, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                outp &= "<td nowrap width='71' class='cssamt' style='text-align:right;'>" & FormatNumber(allow_2, 2, TriState.True, TriState.True, TriState.True) & "</td>"

                outp &= "<td nowrap width='120' class='ename'>" & post & "  </td>"
                outp &= "<td nowrap width='60' class='cssamt' style='text-align:right;'>" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                outp &= "<td nowrap width='71' class='cssamt' style='text-align:right;'>" & FormatNumber(allow_, 2, TriState.True, TriState.True, TriState.True) & "</td>"
                '  outp &= "  <td nowrap width='198' class='ename'>" & proj & "      </td>"
                outp &= " <td nowrap width='190' class='ename'>" & transproj & "</td>"
                '  outp &= "<td nowrap width='71' align='left' class='ename'>&nbsp;" & remarkx & "</td>"
                outp &= " </tr>"
                ccount = ccount + 1
            End While
            outp &= "</table><br>" & footerpages() & "</div></div>"
            outp &= "<div class='page'><div class='subpage' ><table cellspacing=0 cellpadding=0><tr><td colspan='3'>Summery: Gender</td></tr><tr><td>Male</td><td>:</td><td>" & Session("male") & "</td></tr><tr><td>Female</td><td>:</td><td>" & Session("female") & "</td></tr></table><br>"
            Dim spl() As String
            spl = poost.Split("|")
            Array.Sort(spl)
            ' Response.Write(poost)
            If spl.Length > 0 Then
                outp &= "<table cellspacing=0 cellpadding=0><tr><td colspan=4>Summery by Position</td></tr><tr><td>no.</td><td>Position</td><td></td><td>No. Emp.</td></tr>"
                Dim j As Integer = 1
                For i As Integer = 0 To UBound(spl)
                    ' Response.Write("<br>" & sec.Kir_HexToStr(spl(i)))
                    If spl(i) <> "" Then
                        outp &= "<tr><td> " & j & "</td><td>" & sec.Kir_HexToStr(spl(i)) & "</td><td>:</td><td> " & Session(spl(i)) & "</td></tr>"
                        Session.Remove(spl(i))
                        j = j + 1
                    End If
                Next
                outp &= "</table>"
            End If
            outp &= "</div></div>"
            Session("male") = 0
            Session("female") = 0
            Dim loc As String = Session("path")
            loc &= "/download/iso" & Now.Ticks & ".txt"
            File.WriteAllText(loc, outp)
            outp = "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                         "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                        " display:block;' onclick=" & Chr(34) & "javascript:exportx('isohrm(" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ")','doc,'" & loc & "','export','2;3');" & Chr(34) & " >" & _
                        "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                        "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                  " onclick=" & Chr(34) & "javascirpt:print('bigprintx','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printisoA4');" & Chr(34) & ">" & _
                             "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                            "<div style='clear:left;'></div><br>" & _
"<div id='bigprintx'>" & outp & "</div>"
            Response.Write(outp)
        End If
        For Each k As String In Session.StaticObjects
            '  Response.Write(k & ".................<br>")
        Next
    End Function

    Function header_first(ByVal p As Integer, ByVal q As Integer, ByVal rtype As String)
        Dim rtn As String = ""
        rtn &= "   <table class='top1' border='1' cellpadding='0' cellspacing='0' align='center'>" & Chr(13) & _
        "<tr>" & Chr(13) & _
            "<td width='116' height='60'>" & Chr(13) & _
               " <p class='MsoHeader'>" & Chr(13) & _
                 "<img align='left' height='41' hspace='12' " & Chr(13) & _
       " src = 'images\netlog.png'v:shapes='Picture_x0020_9' width='69' /><span><o:p></o:p></span></p>" & Chr(13) & _
          "  </td><td valign='top' width='527'>" & Chr(13) & _
             "  <p class='MsoHeader'>" & Chr(13) & _
                 "   <b><span>&nbsp;</span></b><span>Company Name:<o:p></o:p></span></p>" & Chr(13) & _
               " <p align='center' class='MsoNormal' style='text-align:center'><span>" & Session("company_name_amharic") & "<o:p></o:p></span></p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'><b><span>" & Session("company_name") & "</span></b><span><o:p></o:p></span></p>" & Chr(13) & _
            "</td><td colspan='2' valign='top' width='218'>" & Chr(13) & _
            "    <p class='MsoHeader'>" & Chr(13) & _
                   " <span>Document No.:<o:p></o:p></span></p>" & Chr(13) & _
               " <p class='MsoHeader'>" & Chr(13) & _
                    "<span><o:p>&nbsp;</o:p></span></p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                 "   <span>OF/NET/______<o:p></o:p></span></p>" & Chr(13) & _
           " </td>        </tr>" & Chr(13) & _
        "<tr>         <td colspan='2' valign='top' width='643'>" & Chr(13) & _
          "      <p class='MsoHeader'>" & Chr(13) & _
                    "<span>Title:&nbsp; <o:p></o:p></span>" & Chr(13) & _
                "</p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                   " <b><span>Human Resource Report</span></b><span><o:p></o:p></span></p>" & Chr(13) & _
                                               "  </td>" & Chr(13) & _
            "<td valign='top' width='79'>" & Chr(13) & _
              "  <p class='MsoHeader'>" & Chr(13) & _
                  "  <span>Issue No.:<o:p></o:p></span></p>" & Chr(13) & _
              "  <p class='MsoHeader'>" & Chr(13) & _
                 "   <span><o:p>&nbsp;</o:p></span></p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                  "  <span>1<o:p></o:p></span></p>" & Chr(13) & _
              "  <p class='MsoHeader'>" & Chr(13) & _
                  "  <span><o:p>&nbsp;</o:p></span></p>" & Chr(13) & _
          "  </td>" & Chr(13) & _
           " <td valign='top' width='140'>" & Chr(13) & _
          "  </td>" & Chr(13) & _
        "</tr>" & Chr(13) & _
    "</table>"

        Return rtn
    End Function
    Function header_all(ByVal p As String, ByVal q As String, ByVal rtype As String)
        Dim rtn As String = ""
        rtn = "<table class='tbltop' align='center'><tr><td>From:" & Format(CDate(pd1), "MMM dd, yyyy") & "-" & Format(CDate(pd2), "MMM dd, yyyy") & _
            "</td><td> &nbsp;</td></tr><tr><td>" & "<p>Report type:" & rtype & "</p>" & Chr(13) & "</td><td>" & Chr(13) & " <p class='MsoHeader'>" & Chr(13) & _
               "   <span>Page No.:<o:p></o:p></span></p>" & Chr(13) & _
             Chr(13) & " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                " <span>Page </span><b><span>" & p.ToString & "</span></b><span> " & Chr(13) & _
                  "of </span><span><span><b>" & q & "</b></span></p></td></tr></table>"
        Return rtn
    End Function
    Function table_header(ByVal required As String)
        Dim rtn As String = ""
        ' Response.Write(required)
        Dim loc As String = Session("path") & "/iso/hr/"
        Select Case required
            Case "hired"
                rtn = File.ReadAllText(loc & "hire.txt")
            Case "termination"
                '   Response.Write("IIIIIIIIINNNNNNN")
                rtn = File.ReadAllText(loc & "termination.txt")
            Case "transfer"
                rtn = File.ReadAllText(loc & "transfer.txt")
            Case "promotion"
                rtn = File.ReadAllText(loc & "promotion.txt")
            Case "vacancy"
                rtn = File.ReadAllText(loc & "vacancy.txt")
            Case Else
        End Select
        Return rtn
    End Function
    Function footerpages()
        Dim outp As String = ""


        outp = " <table border='0' cellpadding='0' cellspacing='0' class='MsoNormalTable' " & _
       " width='653'>" & _
        "<tr> " & _
         "   <td nowrap valign='bottom' width='306'> " & _
          "      <p class='MsoNormal'>" & _
           "         <b><span>&nbsp;Prepared by ____________________<o:p></o:p></span></b></p> " & _
            "</td> " & _
            "<td nowrap valign='bottom' width='286'> " & _
             "   <p class='MsoNormal'> " & _
              "      <b><span>Approved By: _____________________<o:p></o:p></span></b></p> " & _
            "</td> " & _
            "<td width='62'> " & _
             "   <p class='MsoNormal'> " & _
              "      &nbsp;</p> " & _
            "</td> " & _
        "</tr> " & _
        "<tr> " & _
         "   <td nowrap valign='bottom' width='306'> " & _
          "      <p class='MsoNormal'> " & _
           "         <b><span>&nbsp;Date _________________<o:p></o:p></span></b></p> " & _
            "</td> " & _
            "<td colspan='2' nowrap valign='bottom' width='348'> " & _
             "   <p class='MsoNormal'> " & _
              "      <b><span>Date ____________________<o:p></o:p></span></b></p> " & _
            "</td> " & _
        "</tr> " & _
    "</table> "
        Return outp
    End Function
    Function summeryhr(ByVal pd1 As Date, ByVal pd2 As Date)
        Dim fm As New formMaker
        Dim hirer, termr, transr, salincr, promr As String
        hirer = fm.getinfo2("select count(id) from emprec where hire_date between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
        termr = fm.getinfo2("select count(id) from emprec where end_date between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
        transr = fm.getinfo2("select count(id) from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Job Assignment'", Session("con"))

    End Function
    Protected Sub ISOHRM_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub
End Class
