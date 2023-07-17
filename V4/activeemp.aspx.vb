Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class activeemp
    Inherits System.Web.UI.Page
    Public pd1, pd2, isotype As String
    Public pgno As Integer

  
    Function init()
        If IsDate(Request.Form("pd2")) Then
            pd2 = Request.Form("pd2")

        ElseIf IsDate(Request.QueryString("pd2")) Then
            pd2 = Request.QueryString("pd2")

        End If

        If IsDate(pd2) Then

            

                    activeworker()
               
        Else
            Response.Write(Request.Form("pd2"))
        End If
        If IsPostBack = True Then
            Response.Write("postback")
        End If
    End Function
    Function activeworker()
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
        Dim nperp As Integer
        Dim page_count As Integer = 1
        Dim projid As String = ""
        Dim proj As String = ""
        Dim fname, hrdate, post, sal, allow, allow2, allow_, sex As String
        Dim fd(,) As String
        Dim edu, emp_status As String
        Dim csex, cterm As String
        Dim poost As String = ""
        Dim k As String = ""
        If File.Exists(Session("path") & "\log\pass.kst") Then
            Dim r As String
            r = File.ReadAllText(Session("path") & "\log\pass.kst")
            k = sec.Kir_HexToStr(r)
            ' Response.Write(k)
        End If

        csex = 0
        cterm = 0
        Dim max As Integer = 20
        Dim remarkx As String = ""
        nperp = max
        Dim rssqlnew As String
        rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                     "where emprec.hire_date<='" & pd2 & "' and ('" & pd2 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pd2 & _
                                                                     "') or emprec.hire_date between '" & pd2 & "' and isnull(emprec.end_date,'" & pd2 & "' )) " & _
                                                                                                                                         " ORDER BY emp_static_info.first_name,emp_static_info.middle_name,emp_static_info.last_name,emprec.id desc "
        ' Response.Write(rssqlnew & "<br>")
        ' sql = "select * from emprec  where hire_date < '" & pd2 & "' and (end_date is null or end_date>'" & pd2 & "')  order by hire_date"
        Dim poostx As String = ""
        ' Response.Write(sql)
        sql = rssqlnew
        Try
            rs = dbs.dtmake("vwhire", sql, Session("con"))



            If rs.HasRows Then
                nrows = fm.getinfo2("SELECT count(emprec.id) FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                     "where emprec.hire_date<='" & pd2 & "' and ('" & pd2 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pd2 & _
                                                                     "') or emprec.hire_date between '" & pd2 & "' and isnull(emprec.end_date,'" & pd2 & "' )) ", Session("con"))
                If nrows > nperp Then


                    tpgno = Math.Floor(nrows / nperp) + 1


                Else
                    tpgno = 1
                End If
                ' tpgno = Math.Ceiling(nrows / nperp)
                ' Response.Write(nrows & "<<<\norows" & tpgno & "<<</total pages<br>")
                poost = ""
                Dim pgcont As Integer = 0
               
                While rs.Read
                    If ccount = 0 Then
                        outp &= "<div class='page'><div class='subpage' >" & header_first(1, tpgno, "Hired") & header_all(1, tpgno, "Hired")
                        outp &= table_header("hired")
                    ElseIf pgcont Mod (nperp) = 0 Then
                        page_count = page_count + 1
                        outp &= "</table><br>" & footerpages() & "</div></div><div class='page'><div class='subpage' >" & header_all(page_count, tpgno, "Hired")
                        outp &= table_header("hired")
                        pgcont = 0

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
                    sal = IIf(IsNumeric(sal), sal, 0)

                    edu = fd(4, 0)
                    edu = IIf(edu = "", "-", edu)
                    allow = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("id") & " and istaxable='n' and from_date='" & rs.Item("hire_date") & "' is null group by istaxable", Session("con"))
                    If IsNumeric(allow) = False Then
                        allow = 0
                    End If
                    allow2 = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & rs.Item("id") & " and to_date is null and istaxable='y' and from_date='" & rs.Item("hire_date") & "' group by istaxable", Session("con"))
                    If IsNumeric(allow2) = False Then
                        allow2 = 0
                    End If

                    If emp_status = "Inactive" Then
                        '  Response.Write(rs.Item("id") & "<br>")
                        remarkx = fm.getinfo2("select reason from emp_resign where emptid=" & rs.Item("id"), Session("con"))
                    End If

                    allow_ = (CDbl(allow) + CDbl(allow2)).ToString
                    outp &= " <tr  class='repeatable-data-row'>" & Chr(13) & _
               " <td nowrap width='35' class='ename'><span>" & (ccount + 1).ToString & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='150' class='ename'><span>" & fname & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='110'  class='ename '> <span>" & post.Substring(0, IIf(post.Length < 22, post.Length, 22)) & " </span> </td>" & Chr(13)
                    outp &= "<td nowrap width='45' class='ename '><span>" & hrdate & "</span></td>" & Chr(13)
                    outp &= " <td nowrap width='22' class='ename '><span>" & sex & "</span></td>" & Chr(13)
                    outp &= "  <td nowrap width='150' class='ename' title='" & edu & "'> <span>" & edu.Substring(0, IIf(edu.Length < 25, edu.Length, 25)) & "</span> </td>" & Chr(13)
                    outp &= "  <td nowrap width='168' class='ename'><span title='" & proj & "' >" & proj.Substring(0, IIf(proj.Length < 32, proj.Length, 32)) & "</span>      </td>" & Chr(13)
                    outp &= " <td nowrap width='22' class='ename '><span>" & emp_status & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='71' class='cssamt'><span>" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.True) & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='71' class='cssamt'><span>" & FormatNumber(allow_, 2, TriState.True, TriState.True, TriState.True) & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='71' align='center' ><span>&nbsp;" & remarkx & "</span></td>" & Chr(13) & _
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
                    outp &= "<table cellspacing=0 cellpadding=0><tr><td colspan=4>Summery by Position</td></tr><tr><td>no.</td><td>Position</td><td></td><td>No. Emp.</td></tr>" & Chr(13)
                    Dim j As Integer = 1
                    For i As Integer = 0 To UBound(spl)
                        '   Response.Write("<br>" & sec.Kir_HexToStr(spl(i)))
                        If spl(i) <> "" Then
                            outp &= "<tr><td> " & j & "</td><td>" & sec.Kir_HexToStr(spl(i)) & "</td><td>:</td><td> " & Session(spl(i)) & "</td></tr>" & Chr(13)
                            Session.Remove(spl(i))
                            j = j + 1
                        End If
                    Next
                    outp &= "</table>" & Chr(13)
                End If
                outp &= "</div></div>" & Chr(13)
                Session("male") = 0
                Session("female") = 0
                Dim loc As String = Session("path")
                loc = Replace(loc, "\", "/")
                loc &= "/download/iso" & Now.Ticks & ".txt"
                File.WriteAllText(loc, outp)

                outp = "<div id='pageprint' style=''>" & outp & "</div><div style='clear:left;'></div>"

                Response.Write(printexport("doc", "pageprint", "A4", loc))
                Response.Write(outp)
                Response.Write(csex)
                'Response.Write("<script>alert($('#p1').height());</script>")
                pgno = page_count
            End If
        Catch ex As Exception
            Dim ml As New mail_system
            Response.Write(ex.ToString)
            fm.exception_hand(ex, "Erro on Active emp")
        End Try
    End Function
    Function makechart(ByVal d1 As String)
        If (IsDate(d1)) Then
            Dim y As Integer = CDate(d1).Year
            Dim year() As Integer = {y}
            Dim yrdate() As Date = {"12/31/" & y}
            Dim noemp() As Integer = {0}
            Dim ky As Integer = 0
            Dim jv As String = ""
            Dim tblx As String = "<tr><td>Year</td>"
            Dim tbly As String = "<tr><td>No Emp.</td>"
            Dim sql As String = ""
            Dim dp As Date
            Dim rand As New Random
            Dim countx, cnt As Integer
            Dim legend, legcolor, clr As String
            legcolor = "["
            legend = ""
            cnt = 0
            ' y = Today.Year - 9
            For i As Integer = y To Today.Year
                ky = year.Length
                'Response.Write(year.Length & "<br>")
                ReDim Preserve year(year.Length + 1)
                ReDim Preserve yrdate(ky + 1)
                ReDim Preserve noemp(ky + 1)

                dp = "12/31/" & i
                If IsDate(dp) Then
                    sql = "SELECT count(emprec.id) FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                         "where emprec.hire_date<='" & dp & "' and ('" & dp & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & dp & _
                                                "') or emprec.hire_date between '" & dp & "' and isnull(emprec.end_date,'" & dp & "' )) "
                    '  Response.Write(sql & "<br>")
                    cnt = cnt + 1
                    countx = numbercount(sql)
                    clr = "#" & Conversion.Hex(rand.Next(CInt(255))) & Conversion.Hex(rand.Next(CInt(255))) & Conversion.Hex(rand.Next(CInt(255)))
                    jv &= "['" & i & "', " & countx & "],"
                    If clr.Length < 7 Then
                        For k As Integer = 0 To 7 - clr.Length - 1
                            clr &= "0"
                        Next
                    End If
                    legend &= "myChart.setLegendForBar(" & cnt & ",'" & i & "');" & Chr(13)
                    legcolor &= "'" & clr & "',"
                    year(ky - 1) = i
                    tblx &= "<td>" & i & "</td>"
                    tbly &= "<td>" & countx & "</td>"

                    yrdate(ky - 1) = dp
                    noemp(ky - 1) = countx


                End If



            Next
            jv = jv.Substring(0, jv.Length - 1)
            legcolor = legcolor.Substring(0, legcolor.Length - 1)
            legcolor &= "];"
            Return mkchart(jv, "Employee Every Year", "Year", "No.Emp.", legend, legcolor) & "<table>" & tblx & "</tr>" & tbly & "</tr></table>"

        End If
    End Function
    Function activeworker2()
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
        Dim nperp As Integer
        Dim page_count As Integer = 1
        Dim projid As String = ""
        Dim proj As String = ""
        Dim fname, hrdate, post, sal, allow, allow2, allow_, sex As String
        Dim fd(,) As String
        Dim edu, emp_status As String
        Dim csex, cterm As String
        Dim poost As String = ""
        Dim k As String = ""
        If File.Exists(Session("path") & "\log\pass.kst") Then
            Dim r As String
            r = File.ReadAllText(Session("path") & "\log\pass.kst")
            k = sec.Kir_HexToStr(r)
            ' Response.Write(k)
        End If

        csex = 0
        cterm = 0
        Dim max As Integer = 17
        Dim remarkx As String = ""
        nperp = max
        Dim rssqlnew As String
        rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                     "where ('" & pd2 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pd2 & _
                                                                     "') or emprec.hire_date between '" & pd2 & "' and isnull(emprec.end_date,'" & pd2 & "' )) " & _
                                                                                                                                         " ORDER BY emp_static_info.first_name,emp_static_info.middle_name,emp_static_info.last_name,emprec.id desc "
        ' Response.Write(rssqlnew & "<br>")
        ' sql = "select * from emprec  where hire_date < '" & pd2 & "' and (end_date is null or end_date>'" & pd2 & "')  order by hire_date"
        Dim poostx As String = ""
        ' Response.Write(sql)
        sql = rssqlnew
        Try
            rs = dbs.dtmake("vwhire", sql, Session("con"))



            If rs.HasRows Then
                nrows = fm.getinfo2("SELECT count(emprec.id) FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                     "where ('" & pd2 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pd2 & _
                                                                     "') or emprec.hire_date between '" & pd2 & "' and isnull(emprec.end_date,'" & pd2 & "' )) ", Session("con"))
                If nrows > nperp Then


                    tpgno = Math.Floor(nrows / nperp) + 1


                Else
                    tpgno = 1
                End If
                ' tpgno = Math.Ceiling(nrows / nperp)
                ' Response.Write(nrows & "<<<\norows" & tpgno & "<<</total pages<br>")
                poost = ""
                Dim pgcont As Integer = 0
                While rs.Read
                    If ccount = 0 Then
                        outp &= "<div id='wrapper' class='landscape'><div class='a4_landscape'><header>" & header_first(1, tpgno, "Hired") & header_all(1, tpgno, "Hired") & "</header>"
                        outp &= "<div> " & table_header("hired")
                    ElseIf pgcont Mod (nperp) = 0 Then
                        page_count = page_count + 1
                        outp &= "<tbody></table>" & footerpages() & "</div><div><div> <!--One page containing div-->" & _
"<div id='page_separator'></div>" & _
"<a class='close_btn' id='1'>close</a><div class='a4_landscape' ><header>" & header_all(page_count, tpgno, "Hired") & "</header>"
                        outp &= "<div>" & table_header("hired")
                        pgcont = 0

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
                    sal = IIf(IsNumeric(sal), sal, 0)

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
                        '  Response.Write(rs.Item("id") & "<br>")
                        remarkx = fm.getinfo2("select reason from emp_resign where emptid=" & rs.Item("id"), Session("con"))
                    End If

                    allow_ = (CDbl(allow) + CDbl(allow2)).ToString
                    outp &= " <tr  class='repeatable-data-row'>" & Chr(13) & _
               " <td nowrap width='35' class='ename'>" & (ccount + 1).ToString & "</td>" & Chr(13)
                    outp &= "<td nowrap width='150' class='ename'><span>" & fname & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='110'  class='ename '> <span>" & post.Substring(0, IIf(post.Length < 22, post.Length, 22)) & " </span> </td>" & Chr(13)
                    outp &= "<td nowrap width='45' class='ename '><span>" & hrdate & "</span></td>" & Chr(13)
                    outp &= " <td nowrap width='22' class='ename '><span>" & sex & "</span></td>" & Chr(13)
                    outp &= "  <td nowrap width='150' class='ename'> <span>" & edu & "</span> </td>" & Chr(13)
                    outp &= "  <td nowrap width='198' class='ename'><span title='" & proj & "' >" & proj.Substring(0, IIf(proj.Length < 32, proj.Length, 32)) & "</span>      </td>" & Chr(13)
                    outp &= " <td nowrap width='22' class='ename '><span>" & emp_status & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='71' class='cssamt'><span>" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.True) & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='71' class='cssamt'><span>" & FormatNumber(allow_, 2, TriState.True, TriState.True, TriState.True) & "</span></td>" & Chr(13)
                    outp &= "<td nowrap width='71' align='center' ><span>&nbsp;" & remarkx & "</span></td>" & Chr(13) & _
                 " </tr>"
                    ccount = ccount + 1
                End While
                outp &= "</table><br>" & footerpages() & "</div>"
                outp &= "<div class='a4_landscape'><table ><tr><td colspan='3'>Summery: Gender</td></tr><tr><td>Male</td><td>:</td><td>" & Session("male") & "</td></tr><tr><td>Female</td><td>:</td><td>" & Session("female") & "</td></tr></table><br>"
                Dim spl() As String
                spl = poost.Split("|")
                Array.Sort(spl)
                ' Response.Write(poost)
                If spl.Length > 0 Then
                    outp &= "<table width='100%' class='data table4' border='1'><tr><td colspan=4>Summery by Position</td></tr><tr><td>no.</td><td>Position</td><td></td><td>No. Emp.</td></tr>" & Chr(13)
                    Dim j As Integer = 1
                    For i As Integer = 0 To UBound(spl)
                        '   Response.Write("<br>" & sec.Kir_HexToStr(spl(i)))
                        If spl(i) <> "" Then
                            outp &= "<tr><td> " & j & "</td><td>" & sec.Kir_HexToStr(spl(i)) & "</td><td>:</td><td> " & Session(spl(i)) & "</td></tr>" & Chr(13)
                            Session.Remove(spl(i))
                            j = j + 1
                        End If
                    Next
                    outp &= "</table>" & Chr(13)
                End If
                outp &= "</div></div>" & Chr(13)
                Session("male") = 0
                Session("female") = 0
                Dim loc As String = Session("path")
                loc = Replace(loc, "\", "/")
                loc &= "/download/iso" & Now.Ticks & ".txt"
                File.WriteAllText(loc, outp)

                outp = "<div id='pageprint' style=''>" & outp & "</div><div style='clear:left;'></div>"

                Response.Write(printexport("doc", "pageprint", "A4", loc))
                Response.Write(outp)
                Response.Write(csex)
                'Response.Write("<script>alert($('#p1').height());</script>")
                pgno = page_count
            End If
        Catch ex As Exception
            Dim ml As New mail_system
            Response.Write(ex.ToString)
            fm.exception_hand(ex, "active Emp Erro")
           end Try
    End Function
    Function viewall()
        Dim ccount As Integer = 0
        Dim outp As String = ""
        Dim pgcont, page_count, nperp, tpgno As Integer

        If ccount = 0 Then
            outp &= "<div class='page'><div class='subpage'>" & header_first(1, tpgno, "Vacancy") & header_all(1, tpgno, "Vacancy")
            outp &= table_header("vacancy")
        ElseIf pgcont Mod nperp = 0 Then
            page_count = page_count + 1
            outp &= "</table><br>" & footerpages() & "</div></div><div  class='page'><div class='subpage' >" & header_all(page_count, tpgno, "Vacancy")
            outp &= table_header("vacancy")
            pgcont = 0

        End If
        pgcont = pgcont + 1
    End Function

    Function allr()

        Dim listreport(5) As String '
        Dim colllist() As String = {"Month", "Hired", "termination", "transfer", "promotion", "vacancy"}
        Dim arrojb(,) As Object
        Dim fm As New formMaker
        Dim m, y, i As Integer
        Dim rtn As String
        Dim outp As String = ""
        outp &= "<div class='page'><div class='subpage' >"
        Dim hso As Date = fm.getinfo2("select hire_date from emprec order by hire_date", Session("con"))
        outp &= makechart(hso) & "</div></div>"
        Response.Write(outp)
        If pd1 <> "" Then
            rtn = "<div class='page'><div class='subpage' >" & header_first(1, 1, "Summery") & header_all(1, 1, "Summery")
            rtn &= "<table align='center' cellspacing=0 cellpadding=0 style='font-size:14pt;'><tr style='font-size:16pt;'><td>Report Type</td><td>No</td></tr>"

            listreport(0) = fm.getinfo2("select count(id) from emprec where hire_date between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
            listreport(1) = fm.getinfo2("select count(id) from emprec where end_date between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
            listreport(2) = fm.getinfo2("select count(id) from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='Job Assignment'", Session("con"))
            listreport(3) = fm.getinfo2("select count(id) from emp_job_assign where date_from between '" & pd1 & "' and '" & pd2 & "' and ass_for='promotion'", Session("con"))
            listreport(4) = fm.getinfo2("select count(id) from tblhrjobs where start_date between '" & pd1 & "' and '" & pd2 & "'", Session("con"))
            rtn &= "<tr><td class='label'>Hired</td><td><a href='?isotype=hired&pd1=" & pd1 & "&pd2=" & pd2 & "&pagesize=25'>" & listreport(0) & "</a></td></tr>"
            rtn &= "<tr><td class='label'>termination</td><td><a href='?isotype=termination&pd1=" & pd1 & "&pd2=" & pd2 & "&pagesize=25'>" & listreport(1) & "</a></td></tr>"
            rtn &= "<tr><td class='label'>transfer</td><td><a href='?isotype=transfer&pd1=" & pd1 & "&pd2=" & pd2 & "&pagesize=25'>" & listreport(2) & "</a></td></tr>"
            rtn &= "<tr><td class='label'>Promotion</td><td><a href='?isotype=promotion&pd1=" & pd1 & "&pd2=" & pd2 & "&pagesize=25'>" & listreport(3) & "</a></td>"
            rtn &= "<tr><td class='label'>Vacancy</td><td><a href='?isotype=vacancy&pd1=" & pd1 & "&pd2=" & pd2 & "&pagesize=25'>" & listreport(4) & "</a></td></tr>"
            rtn &= "</table>"
            Dim jvarr, ldgcolor, ldnd As String
            jvarr = "['hired'," & listreport(0) & "],"
            jvarr &= "['Termination'," & listreport(1) & "],"
            jvarr &= "['Transfer'," & listreport(2) & "],"
            jvarr &= "['Promotion'," & listreport(3) & "],"
            jvarr &= "['Vacancy'," & listreport(4) & "]"


            ldgcolor = "" '"myChart.setBarColor('#0000ff', 1);"

            ldnd = "" ' "myChart.setLegendForBar(" & k + 1 & ", '" & colllist(k + 1) & "');"

            '  Response.Write(rtn) ' & "<br>" & jvarr & "<br>" & ldgcolor & "<br>" & ldnd)
            Dim xrep As String
            xrep = "  var myData = new Array(" & jvarr & ");" & Chr(13)
            xrep &= "var myChart = new JSChart('graph', 'bar');" & Chr(13)
            xrep &= "myChart.setDataArray(myData);" & Chr(13)
            xrep &= "myChart.setTitle('HR Report Summery(" & pd1 & "-" & pd2 & ")');" & Chr(13)
            xrep &= "myChart.setTitleColor('#8E8E8E');" & Chr(13)
            xrep &= "myChart.setAxisNameX('Report Type');" & Chr(13)
            xrep &= "myChart.setAxisNameY('No. Employees');" & Chr(13)
            xrep &= "myChart.setAxisNameFontSize(16);" & Chr(13)
            xrep &= "myChart.setAxisNameColor('#999');" & Chr(13)
            xrep &= "myChart.setAxisValuesAngle(30);" & Chr(13)
            xrep &= "myChart.setAxisValuesColor('#777');" & Chr(13)
            xrep &= "myChart.setAxisColor('#B5B5B5');" & Chr(13)
            xrep &= "myChart.setAxisWidth(1);" & Chr(13)
            xrep &= "myChart.setBarValuesColor('#2F6D99');" & Chr(13)
            xrep &= "myChart.setAxisPaddingTop(60);" & Chr(13)
            xrep &= "myChart.setAxisPaddingBottom(60);" & Chr(13)
            xrep &= "myChart.setAxisPaddingLeft(45);" & Chr(13)
            xrep &= "myChart.setTitleFontSize(11);" & Chr(13)
            ' xrep &= "myChart.setBarColor('#2D6B96', 1);" & Chr(13)
            'xrep &= "myChart.setBarColor('#9CCEF0', 2);" & Chr(13)
            xrep &= ldgcolor
            xrep &= "myChart.setBarBorderWidth(0);" & Chr(13)
            xrep &= "myChart.setBarSpacingRatio(50);" & Chr(13)
            xrep &= "myChart.setBarOpacity(0.9);" & Chr(13)
            xrep &= "myChart.setFlagRadius(6);" & Chr(13)
            'xrep &= "myChart.setTooltip(['North America', 'Click me', 1], callback);" & Chr(13)
            xrep &= "myChart.setTooltipPosition('nw');" & Chr(13)
            xrep &= "myChart.setTooltipOffset(3);" & Chr(13)
            xrep &= "myChart.setLegendShow(false);" & Chr(13)
            xrep &= "myChart.setLegendPosition('right top');" & Chr(13)
            ' xrep &= ldnd & Chr(13)
            ' xrep &= "myChart.setLegendForBar(1, '2005');" & Chr(13)
            ' xrep &= "myChart.setLegendForBar(2, '2010');" & Chr(13)
            xrep &= "myChart.setSize(616, 321);" & Chr(13)
            xrep &= "myChart.setGridColor('#C6C6C6');" & Chr(13)
            xrep &= "myChart.draw();" & Chr(13)
            rtn = "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                 " onclick=" & Chr(34) & "javascirpt:print('bigprintx','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printisoA4');" & Chr(34) & ">" & _
                            "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                           "<div style='clear:left;'></div><br>" & _
"<div id='bigprintx' style=''>" & rtn & "<div id='graph'>Loading...</div></div></div><script >" & xrep & "</script></div><div style='clear:left;'></div>"
            Response.Write(rtn)


        Else
            Dim datex As Date
            datex = Today.AddMonths(-1)
            m = datex.Month
            y = datex.Year

            pd1 = "1/1/" & Today.Year
            pd2 = Today.AddMonths(-1)
            rtn = "<div class='page'><div class='subpage' >" & header_first(1, 1, "Summery") & header_all(1, 1, "Summery")
            rtn &= "<table align='center' cellspacing=0 cellpadding=0><tr style='font-weight:bold;font-size:14px;'><td>Month</td><td>Hired</td><td>termination</td><td>transfer</td><td>Promotion</td><td>Vacancy</td></tr>"


            For i = 1 To m
                listreport(0) = fm.getinfo2("select count(id) from emprec where month(hire_date) =" & i & " and year(hire_date)=" & y, Session("con"))
                listreport(1) = fm.getinfo2("select count(id) from emprec where month(end_date) =" & i & " and year(end_date)=" & y, Session("con"))
                listreport(2) = fm.getinfo2("select count(id) from emp_job_assign where month(date_from)=" & i & " and year(date_from)=" & y & " and ass_for='Job Assignment'", Session("con"))
                listreport(3) = fm.getinfo2("select count(id) from emp_job_assign where month(date_from)=" & i & " and year(date_from)=" & y & " and ass_for='promotion'", Session("con"))
                listreport(4) = fm.getinfo2("select count(id) from tblhrjobs where  month(start_date)=" & i & " and year(start_date)=" & y, Session("con"))
                ReDim Preserve arrojb(6, i + 1)
                For p As Integer = 0 To 4
                    arrojb(p, i) = listreport(p)
                Next
            Next
            Dim colorar() As String = {"#2d6b15", "#2d0044", "#f06b96", "#2d1200", "#210b90", "#1d6b92", "#216b92", "#2d1b02", "#2d6112"}
            Dim j, k As Integer
            Dim jvarr As String = ""
            Dim ldnd, ldgcolor As String
            ldgcolor = ""
            ldnd = ""
            For j = 1 To i - 1
                rtn &= "<tr><td>" & MonthName(j) & "</td>"
                jvarr &= "['" & MonthName(j) & "',"
                For k = 0 To 4
                    rtn &= "<td class='ndata'>" & arrojb(k, j) & "</td>"
                    If j = 1 Then
                        ldgcolor &= "myChart.setBarColor('" & colorar(k) & "', " & k + 1 & ");"

                        ldnd &= "myChart.setLegendForBar(" & k + 1 & ", '" & colllist(k + 1) & "');"
                    End If

                    jvarr &= arrojb(k, j) & ","
                Next
                jvarr = jvarr.Substring(0, jvarr.Length - 1)
                jvarr &= "],"
                rtn &= "</tr>"
            Next
            jvarr = jvarr.Substring(0, jvarr.Length - 1)
            rtn &= "</table>"
            Response.Write(rtn) ' & "<br>" & jvarr & "<br>" & ldgcolor & "<br>" & ldnd)
            Dim xrep As String
            xrep = "  var myData = new Array(" & jvarr & ");" & Chr(13)
            xrep &= "var myChart = new JSChart('graph', 'bar');" & Chr(13)
            xrep &= "myChart.setDataArray(myData);" & Chr(13)
            xrep &= "myChart.setTitle('HR Report Summery');" & Chr(13)
            xrep &= "myChart.setTitleColor('#8E8E8E');" & Chr(13)
            xrep &= "myChart.setAxisNameX('Month');" & Chr(13)
            xrep &= "myChart.setAxisNameY('No. Employees');" & Chr(13)
            xrep &= "myChart.setAxisNameFontSize(16);" & Chr(13)
            xrep &= "myChart.setAxisNameColor('#999');" & Chr(13)
            xrep &= "myChart.setAxisValuesAngle(30);" & Chr(13)
            xrep &= "myChart.setAxisValuesColor('#777');" & Chr(13)
            xrep &= "myChart.setAxisColor('#B5B5B5');" & Chr(13)
            xrep &= "myChart.setAxisWidth(1);" & Chr(13)
            xrep &= "myChart.setBarValuesColor('#2F6D99');" & Chr(13)
            xrep &= "myChart.setAxisPaddingTop(60);" & Chr(13)
            xrep &= "myChart.setAxisPaddingBottom(60);" & Chr(13)
            xrep &= "myChart.setAxisPaddingLeft(45);" & Chr(13)
            xrep &= "myChart.setTitleFontSize(11);" & Chr(13)
            ' xrep &= "myChart.setBarColor('#2D6B96', 1);" & Chr(13)
            'xrep &= "myChart.setBarColor('#9CCEF0', 2);" & Chr(13)
            xrep &= ldgcolor
            xrep &= "myChart.setBarBorderWidth(0);" & Chr(13)
            xrep &= "myChart.setBarSpacingRatio(50);" & Chr(13)
            xrep &= "myChart.setBarOpacity(0.9);" & Chr(13)
            xrep &= "myChart.setFlagRadius(6);" & Chr(13)
            'xrep &= "myChart.setTooltip(['North America', 'Click me', 1], callback);" & Chr(13)
            xrep &= "myChart.setTooltipPosition('nw');" & Chr(13)
            xrep &= "myChart.setTooltipOffset(3);" & Chr(13)
            xrep &= "myChart.setLegendShow(true);" & Chr(13)
            xrep &= "myChart.setLegendPosition('right top');" & Chr(13)
            xrep &= ldnd & Chr(13)
            ' xrep &= "myChart.setLegendForBar(1, '2005');" & Chr(13)
            ' xrep &= "myChart.setLegendForBar(2, '2010');" & Chr(13)
            xrep &= "myChart.setSize(616, 321);" & Chr(13)
            xrep &= "myChart.setGridColor('#C6C6C6');" & Chr(13)
            xrep &= "myChart.draw();" & Chr(13)

            Response.Write("<div id='graph'>Loading...</div></div>" & Chr(13) & "<script >" & xrep & "</script>")



        End If



    End Function
   
    Function numbercount(ByVal sql As String)
        Dim fm As New formMaker
        Dim rslt As String
        rslt = fm.getinfo2(sql, Session("con"))
        If IsNumeric(rslt) Then
            Return CInt(rslt)
        Else
            Return 0
        End If
    End Function
    Function header_first(ByVal p As Integer, ByVal q As Integer, ByVal rtype As String)
        Dim rtn As String = ""
        rtn &= "   <table width='100%' class='data' border='1'>" & Chr(13) & _
        "<tr>" & Chr(13) & _
            "<td width='116' height='60'>" & Chr(13) & _
               " <p class='MsoHeader'>" & Chr(13) & _
                 "<img align='left' height='41' hspace='12' " & Chr(13) & _
       " src = 'images\netlog.png'v:shapes='Picture_x0020_9' width='69' /><span></span></p>" & Chr(13) & _
          "  </td><td valign='top' width='527'>" & Chr(13) & _
             "  <p class='MsoHeader'>" & Chr(13) & _
                 "   <b><span>&nbsp;</span></b><span>Company Name:</span></p>" & Chr(13) & _
               " <p align='center'  style='text-align:center'><span>" & Session("company_name_amharic") & "</span></p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'><b><span>" & Session("company_name") & "</span></b><span></span></p>" & Chr(13) & _
            "</td><td colspan='2' valign='top' width='218'>" & Chr(13) & _
            "    <p class='MsoHeader'>" & Chr(13) & _
                   " <span>Document No.:</span></p>" & Chr(13) & _
               " <p class='MsoHeader'>" & Chr(13) & _
                    "<span></span></p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                 "   <span>OF/NET/______</span></p>" & Chr(13) & _
           " </td>        </tr>" & Chr(13) & _
        "<tr>         <td colspan='2' valign='top' width='643'>" & Chr(13) & _
          "      <p class='MsoHeader'>" & Chr(13) & _
                    "<span>Title:&nbsp; </span>" & Chr(13) & _
                "</p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                   " <b><span>Human Resource Report</span></b><span></span></p>" & Chr(13) & _
                                               "  </td>" & Chr(13) & _
            "<td valign='top' width='79'>" & Chr(13) & _
              "  <p class='MsoHeader'>" & Chr(13) & _
                  "  <span>Issue No.:</span></p>" & Chr(13) & _
              "  <p class='MsoHeader'>" & Chr(13) & _
                 "   <span></span></p>" & Chr(13) & _
               " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                  "  <span>1</span></p>" & Chr(13) & _
              "  <p class='MsoHeader'>" & Chr(13) & _
                  "  <span></span></p>" & Chr(13) & _
          "  </td>" & Chr(13) & _
           " <td valign='top' width='140'>" & Chr(13) & _
          "  </td>" & Chr(13) & _
        "</tr>" & Chr(13) & _
    "</table>"

        Return rtn
    End Function
    Function header_all(ByVal p As String, ByVal q As String, ByVal rtype As String)
        Dim rtn As String = ""
        rtn = "<table width='100%' class='data' border='1'><tbody><tr><td>AS Date of " & Format(CDate(pd2), "MMM dd, yyyy") & _
            "</td><td> &nbsp;</td></tr><tr><td>" & "<p>Report type:" & rtype & "</p>" & Chr(13) & "</td><td>" & Chr(13) & " <p class='MsoHeader'>" & Chr(13) & _
               "   <span>Page No.:</span></p>" & Chr(13) & _
             Chr(13) & " <p align='center' class='MsoHeader' style='text-align:center'>" & Chr(13) & _
                " <span>Page </span><b><span>" & p.ToString & "</span></b><span> " & Chr(13) & _
                  "of </span><span><span><b>" & q & "</b></span></p></td></tr></tbody></table>"
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


        outp = " <table width='100%' class='sub_data table3' border='1'><tbody>" & _
        "<tr> " & _
         "   <td width='306'> " & _
          "      <p>" & _
           "         <b><span>&nbsp;Prepared by ____________________</span></b></p> " & _
            "</td> " & _
            "<td width='286'> " & _
             "   <p> " & _
              "      <b><span>Approved By: _____________________</span></b></p> " & _
            "</td> " & _
            "<td width='62'> " & _
             "   <p> " & _
              "      &nbsp;</p> " & _
            "</td> " & _
        "</tr> " & _
        "<tr> " & _
         "   <td width='306'> " & _
          "      <p > " & _
           "         <b><span>&nbsp;Date _________________</span></b></p> " & _
            "</td> " & _
            "<td colspan='2' width='348'> " & _
             "   <p > " & _
              "      <b><span>Date ____________________</span></b></p> " & _
            "</td> " & _
        "</tr></tbody> " & _
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

    Function mkjava(ByVal yr As Integer, ByVal title As String, ByVal collist() As String)
        Dim dbx As New dbclass
        Dim rs As DataTableReader
        Dim rtn(5) As String
        Dim rand As New Random
        Dim crclr As String = ""
        Dim cc As String = ""
        Dim mkch As Boolean = False

        rs = dbx.dtmake("chrt", "SELECT MONTH(hire_date) AS month, COUNT(id) AS noemp FROM emprec WHERE (YEAR(hire_date) = '" & yr & "') GROUP BY MONTH(hire_date)", Session("con"))
        Dim id As Integer = 1
        If rs.HasRows Then
            mkch = True
            rtn(0) &= "['Months',"
            While rs.Read
                rtn(0) &= rs.Item(1) & ","
                crclr = "'#"
                For j As Integer = 1 To 3
                    cc = Conversion.Hex(rand.Next(CInt(255)))


                    If cc.Length < 2 Then
                        cc = "0" & cc
                    End If
                    crclr &= cc
                Next
                ' crclr = "'#" & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & "'"
                crclr &= "'"
                rtn(1) &= "myChart.setLegendForBar(" & id & ",'" & MonthName(rs.Item(0), True) & "');" & Chr(13)
                rtn(2) &= "myChart.setBarColor( " & crclr & ", " & id & ");" & Chr(13)
                id = id + 1
                rand.Next(CInt(255))
                rand.Next(CInt(255))
                ' rgb(rand.Next(255), rand.Next(255), rand.Next(255))
            End While
            rtn(3) = title & " " & yr.ToString
            rtn(0) = rtn(0).Substring(0, rtn(0).Length - 1) & "]"
            ' rtn(1) = rtn(1).Substring(0, rtn(1).
        Else
            rs.Close()
            rs = dbx.dtmake("chrt", "SELECT MONTH(hire_date) AS month, COUNT(id) AS noemp FROM emprec WHERE (YEAR(hire_date) = '" & (yr - 1) & "') GROUP BY MONTH(hire_date)", Session("con"))
            id = 1
            yr = yr - 1
            If rs.HasRows Then
                mkch = True
                rtn(0) &= "['Months',"
                While rs.Read
                    rtn(0) &= rs.Item(1) & ","
                    crclr = "'#"
                    For j As Integer = 1 To 3
                        cc = Conversion.Hex(rand.Next(CInt(255)))


                        If cc.Length < 2 Then
                            cc = "0" & cc
                        End If
                        crclr &= cc
                    Next
                    ' crclr = "'#" & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & "'"
                    crclr &= "'"
                    rtn(1) &= "myChart.setLegendForBar(" & id & ",'" & MonthName(rs.Item(0), True) & "');" & Chr(13)
                    rtn(2) &= "myChart.setBarColor( " & crclr & ", " & id & ");" & Chr(13)
                    id = id + 1
                    rand.Next(CInt(255))
                    rand.Next(CInt(255))
                    ' rgb(rand.Next(255), rand.Next(255), rand.Next(255))
                End While
                rtn(3) = title & " " & yr.ToString
                rtn(0) = rtn(0).Substring(0, rtn(0).Length - 1) & "]"
            End If
        End If
        rs.Close()
        rtn(4) = mkch
        Return rtn
    End Function
    Function printexport(ByVal expto As String, ByVal pageprint As String, ByVal psize As String, ByVal loc As String)
        Dim outpx As String
        outpx = "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & Chr(13) & _
                            "background:url(images/blue_banner-760x147.jpg) #224488;color:White; cursor:pointer; " & Chr(13) & _
                           " display:block;' onclick=" & Chr(34) & "javascript:exportisoo('isohrm" & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & "','" & expto & "','" & loc & "','export','2;3');" & Chr(34) & " >" & Chr(13) & _
                           "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & Chr(13) & _
                           "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147.jpg) #224488;color:White;cursor:pointer;float:left;'" & Chr(13) & _
                                     " onclick=" & Chr(34) & "javascirpt:printiso('" & pageprint & "','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','" & psize & "');" & Chr(34) & ">" & Chr(13) & _
                                "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>"
        Return outpx
    End Function
    Function mkchart(ByVal arg As String, ByVal title As String, ByVal xaxis As String, ByVal yaxis As String, ByVal ledgend As String, ByVal ledcolor As String)
        Dim xrep, rtn As String
        rtn = ""
        xrep = "  var myData = new Array(" & arg & ");" & Chr(13)
        xrep &= "var colors=" & ledcolor
        xrep &= "var myChart = new JSChart('graph2', 'bar');" & Chr(13)

        xrep &= "myChart.setDataArray(myData);" & Chr(13)
        xrep &= "myChart.colorizeBars(colors);" & Chr(13)
        xrep &= "myChart.setTitle('" & title & "');" & Chr(13)
        xrep &= "myChart.setTitleColor('#8E8E8E');" & Chr(13)
        xrep &= "myChart.setAxisNameX('" & xaxis & "');" & Chr(13)
        xrep &= "myChart.setAxisNameY('" & yaxis & " ');" & Chr(13)
        xrep &= "myChart.setAxisNameFontSize(16);" & Chr(13)
        xrep &= "myChart.setAxisNameColor('#999');" & Chr(13)
        xrep &= "myChart.setAxisValuesAngle(30);" & Chr(13)
        xrep &= "myChart.setAxisValuesColor('#777');" & Chr(13)
        xrep &= "myChart.setAxisColor('#B5B5B5');" & Chr(13)
        xrep &= "myChart.setAxisWidth(1);" & Chr(13)
        xrep &= "myChart.setBarValuesColor('#2F6D99');" & Chr(13)
        xrep &= "myChart.setAxisPaddingTop(60);" & Chr(13)
        xrep &= "myChart.setAxisPaddingBottom(60);" & Chr(13)
        xrep &= "myChart.setAxisPaddingLeft(45);" & Chr(13)
        xrep &= "myChart.setTitleFontSize(11);" & Chr(13)
        ' xrep &= "myChart.setBarColor('#2D6B96', 1);" & Chr(13)
        'xrep &= "myChart.setBarColor('#9CCEF0', 2);" & Chr(13)

        'xrep &= ledcolor
        xrep &= "myChart.setBarBorderWidth(0);" & Chr(13)
        xrep &= "myChart.setBarSpacingRatio(50);" & Chr(13)
        xrep &= "myChart.setBarOpacity(0.9);" & Chr(13)
        xrep &= "myChart.setFlagRadius(6);" & Chr(13)
        'xrep &= "myChart.setTooltip(['North America', 'Click me', 1], callback);" & Chr(13)
        xrep &= "myChart.setTooltipPosition('nw');" & Chr(13)
        xrep &= "myChart.setTooltipOffset(3);" & Chr(13)
        xrep &= "myChart.setLegendShow(false);" & Chr(13)
        xrep &= "myChart.setLegendPosition('right top');" & Chr(13)
        '  xrep &= ledgend
        ' xrep &= ldnd & Chr(13)
        ' xrep &= "myChart.setLegendForBar(1, '2005');" & Chr(13)
        ' xrep &= "myChart.setLegendForBar(2, '2010');" & Chr(13)
        xrep &= "myChart.setSize(700, 500);" & Chr(13)
        xrep &= "myChart.setGridColor('#C6C6C6');" & Chr(13)
        xrep &= "myChart.draw();" & Chr(13)
        rtn = "<div id='graph2'>Loading...x</div><script >" & xrep & "</script>"
        Return rtn
    End Function
End Class
