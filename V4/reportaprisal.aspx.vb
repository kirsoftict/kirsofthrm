Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports System.Security.AccessControl
Imports Kirsoft.hrm
Partial Class reportaprisal
    Inherits System.Web.UI.Page
    Function mkform()
        Dim spl() As String
        Dim projid As String = ""
        Dim pdate1, pdate2 As Date
        Dim mm, yy As String
        Dim sspl() As String
        Dim nod As String
        Dim dbs As New dbclass
        Dim fm As New formMaker

        If Request.Form("projname") <> "" Then
            spl = Request.Form("projname").Split("|")

            If spl.Length > 1 Then
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            Else
                projid = ""
            End If
        End If

        If projid <> "" Then
            pdate2 = Request.Form("year")
            If IsDate(pdate2) = True Then
                ' nod = Date.DaysInMonth(CDate(sspl(0)).Year, CDate(sspl(0)).Month)
                pdate1 = fm.getinfo2("select period_start from apr_period where period_end='" & pdate2 & "'", Session("con"))

                '  pdate2 = pdate1.Month & "/" & nod & "/" & pdate1.Year
            Else
                pdate1 = "#1/1/0001#"
                pdate2 = "#1/1/0001#"

            End If
            ' Response.Write(pdate1.ToShortDateString & "======>" & pdate2.ToShortDateString)
            Dim rs As DataTableReader
            Try
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = fm.getprojemp(projid.ToString, pdate2.ToShortDateString, Session("con"))

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                   "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                   "and emprec.id in(select emptid from emp_job_assign " & _
                                                   "where project_id='" & projid.ToString & "' " & _
                                                   "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & "))" & _
                                                   " ORDER BY emp_static_info.first_name,emprec.id desc "

              
                Response.Write(projid & "_____<br>" & rssqlnew & "<br>")
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                                    "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                                  "and emprec.id in(" & rtnvalue & ") ORDER BY emp_static_info.first_name,emprec.id desc ")
                'Session("payrolllist") = rssqlnew
                'Response.Write(rtnvalue)
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
            Dim outp, body As String
            outp = ""
            If rs.HasRows Then
                ' Response.Write(rs.FieldCount() & "<br>")
                outp &= "<table id='tblapp' cellspacing='0' cellpadding='3' >" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:12pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='6' >" & Session("company_name") & _
                "<br> Project Name:"

                outp &= spl(0).ToString

                outp &= "<br> Appraisal Report Period: " & MonthName(CDate(pdate1).Month, True) & " " & CDate(pdate1).Year.ToString & " - End of " & _
                  MonthName(CDate(pdate2).Month, True) & " " & CDate(pdate2).Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)
                outp &= "<tr  style='font-weight:bold;'>"
                outp &= "<td class='headp'>No.</td>"
                outp &= "<td class='headp'>Employee's Name</td>"
                outp &= "<td class='headp'>Appraisal date</td>"
                outp &= "<td class='headp'>Who Assess</td>"
                outp &= "<td class='headp'>Description</td>"
                outp &= "<td class='headp'>Result</td>"
                outp &= "</tr>"
                Dim rt() As String
                Dim emptid As String
                Dim rs2 As DataTableReader
                Dim cnt As Integer = 1
                While rs.Read
                    emptid = rs.Item(0)

                    rs2 = dbs.dtmake("appris", "select * from emp_apprisal where emptid=" & emptid & " and per_end='" & pdate2 & "'", Session("con"))
                    ' Response.Write("select * from emp_apprisal where emptid=" & emptid & " and per_end='" & pdate2 & "'<br>")
                    If rs2.HasRows Then
                        Response.Write("has row emp_apprisal")
                        rs2.Read()
                        outp &= "<tr>"
                        outp &= "<td>" & cnt & "</td>"
                        outp &= "<td>" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td>"
                        outp &= "<td>" & CDate(rs2.Item("app_date")).ToShortDateString & "</td>"
                        outp &= "<td>" & fm.getfullname(rs2.Item("who_app"), Session("con")) & "</td>"
                        outp &= "<td>" & rs2.Item("year_app") & "</td>"
                        outp &= "<td>" & rs2.Item("app_result") & "</td>"
                        outp &= "</tr>"
                        cnt = cnt + 1

                    End If
                    rs2.Close()

                End While
                outp &= "</table>"

                Dim loc As String
                loc = Session("path")
                loc = loc.Replace("\", "/")
                loc &= "/download/aprs" & Now.Ticks.ToString & ".txt"
                '  Response.Write(loc)
                File.WriteAllText(loc, outp)
                Dim outp2 As String
                outp2 = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                            "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                           " display:block;' onclick=" & Chr(34) & "javascript:exportxx('apprisal(apr-" & Now.Ticks.ToString & ")','xls','" & loc & "','export','2;3');" & Chr(34) & " >" & _
                           "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                           "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                     " onclick=" & Chr(34) & "javascirpt:print('apprpt','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printA4app');" & Chr(34) & ">" & _
                                "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                               "<br>"

                Response.Write(outp2 & "<div id='apprpt'>" & outp & "</div>")

            Else
                Response.Write("Sorry there is no data in the database")
            End If
            rs.Close()
        End If
    End Function
    Function mkform2()

        Dim spl() As String
        Dim projid As String = ""
        Dim pdate1, pdate2, pdate11, pdate22 As Date
        Dim mm, yy As String
        Dim sspl() As String
        Dim nod As String
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim group As String
        Dim groupsd() As Date
        Dim groupse() As Date
        If Request.Form("projname") <> "" Then
            spl = Request.Form("projname").Split("|")

            If spl.Length > 1 Then
                projid = spl(1) 'fm.getinfo2("select project_id from tblproject where project_name='" & Request.Form("projname") & "' order by Project_end", session("con"))
            Else
                projid = ""
            End If
        End If
        Dim dfid() As String
        If projid <> "" Then
            group = ""
            group = Request.Form("p_group")
            Dim countx As String
            countx = fm.getinfo2("select count(id) from apr_period where p_group='" & group & "'", Session("con"))

            If IsNumeric(countx) Then
                Dim rsp As DataTableReader
                rsp = dbs.dtmake("strapr", "select id,period_start,period_end from apr_period where p_group='" & group & "' order by period_start", Session("con"))
                If rsp.HasRows Then
                    ReDim dfid(countx)
                    ReDim groupsd(countx)
                    ReDim groupse(countx)
                    Dim c As Integer = 0
                    While rsp.Read

                        dfid(c) = rsp.Item(0)
                        groupsd(c) = rsp.Item(1)
                        groupse(c) = rsp.Item(2)
                        If (c = 0) Then
                            pdate1 = rsp.Item(1)
                        ElseIf (c = countx - 1) Then
                            pdate2 = rsp.Item(2)
                        End If

                        c = c + 1
                    End While
                End If
                rsp.Close()

            End If
            
            If IsDate(pdate2) = False Then

                pdate1 = "#1/1/0001#"
                pdate2 = "#1/1/0001#"

            End If
            ' Response.Write(pdate1.ToShortDateString & "======>" & pdate2.ToShortDateString)
            Dim rs As DataTableReader
            Try
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = fm.getprojemp(projid.ToString, pdate1.ToShortDateString, Session("con"))

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "


                '  Response.Write(projid & "_____<br>" & rssqlnew & "<br>")
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                                    "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                                  "and emprec.id in(" & rtnvalue & ") ORDER BY emp_static_info.first_name,emprec.id desc ")
                'Session("payrolllist") = rssqlnew
                'Response.Write(rtnvalue)
                '   Response.Write(rssqlnew)
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
            Dim outp, body As String
            If rs.HasRows Then
                outp = "<table id='tblapp' cellspacing='0' cellpadding='3' >" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:12pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='6' >" & Session("company_name") & _
                "<br> Project Name:"

                outp &= spl(0).ToString

                outp &= "<br> Appraisal Report Period: " & MonthName(CDate(pdate1).Month, True) & " " & CDate(pdate1).Year.ToString & " - End of " & _
                  MonthName(CDate(pdate2).Month, True) & " " & CDate(pdate2).Year & "</td>" & Chr(13)

                outp &= "</tr>" & Chr(13)
                outp &= "<tr  style='font-weight:bold;'>"
                outp &= "<td class='headp'>No.</td>"
                outp &= "<td class='headp'>Employee's Name</td>"
                outp &= "<td class='headp'>Department</td>"
                outp &= "<td class='headp'>Appraisal date</td>"
                outp &= "<td class='headp'>Who Assess</td>"
                outp &= "<td class='headp'>Description</td>"
                outp &= "<td class='headp'>Result</td>"
                outp &= "<td class='headp'>Summery Result</td>"
                outp &= "</tr>"
                Dim rt() As String
                Dim emptid As String
                Dim rs2 As DataTableReader
                Dim cnt As Integer = 1
                Dim cntx As Integer = 0
                Dim innerc As Integer
                Dim sumresult As String
                Dim sumpoint As Double
                While rs.Read
                    emptid = rs.Item(0)
                    cntx = fm.getinfo2("select count(id) from emp_apprisal where emptid=" & emptid & " and (per_from>='" & pdate1 & "' and per_end<='" & pdate2 & "')", Session("con"))
                    If cntx > 0 Then
                        '    Response.Write("<br>" & emptid & "===" & cntx & "<br>")
                        Dim joinx As String
                        If IsArray(dfid) Then
                            joinx = String.Join(",", dfid)
                        End If



                        rs2 = dbs.dtmake("appris", "select * from emp_apprisal where emptid=" & emptid & " and  (per_from>='" & pdate1 & "' and per_end<='" & pdate2 & "') order by per_from", Session("con"))
                        If rs2.HasRows Then
                            innerc = 0
                            sumpoint = 0
                            sumresult = ""
                            While rs2.Read


                                If cntx > 1 And innerc = 0 Then
                                    outp &= "<tr>"
                                    outp &= "<td rowspan='" & cntx & "'>" & cnt & "</td>"
                                    outp &= "<td rowspan='" & cntx & "'>" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td>"
                                    outp &= "<td rowspan='" & cntx & "'>" & fm.getinfo2("select department from emp_job_assign where emp_id='" & rs.Item("emp_id") & "'", Session("con")) & "</td>"
                                    outp &= "<td>" & CDate(rs2.Item("app_date")).ToShortDateString & "</td>"
                                    outp &= "<td >" & fm.getfullname(rs2.Item("who_app"), Session("con")) & "</td>"

                                    outp &= "<td>" & rs2.Item("year_app") & "</td>"
                                    If IsNumeric(rs2.Item("app_result")) Then
                                        sumpoint += CDbl(rs2.Item("app_result"))
                                    End If
                                    outp &= "<td>" & rs2.Item("app_result") & "</td>"
                                    outp &= "<td rowspan='" & cntx & "' id='sumr" & emptid & "'>" & rs2.Item("app_result") & "</td></tr>"
                                    innerc = innerc + 1
                                    cnt = cnt + 1
                                ElseIf cntx > 1 And innerc > 0 Then
                                    outp &= "<tr>"
                                    outp &= "<td>" & CDate(rs2.Item("app_date")).ToShortDateString & "</td>"
                                    outp &= "<td >" & fm.getfullname(rs2.Item("who_app"), Session("con")) & "</td>"
                                    outp &= "<td>" & rs2.Item("year_app") & "</td>"
                                    If sumpoint = 0 Then
                                      
                                        If IsNumeric(rs2.Item("app_result")) Then

                                            sumresult = "uncalculated"
                                            ' sumpoint += rs2.Item("app_result")
                                        Else
                                            sumpoint = 0
                                            sumresult = rs2.Item("app_result")
                                        End If
                                    Else
                                        sumpoint += rs2.Item("app_result")

                                    End If
                                    outp &= "<td>" & rs2.Item("app_result") & "</td>"
                                    outp &= "</tr>"

                                    innerc = innerc + 1
                                Else
                                    outp &= "<tr><td >" & cnt & "</td>"
                                    outp &= "<td >" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td>"
                                    outp &= "<td>" & fm.getinfo2("select department from emp_job_assign where emp_id='" & rs.Item("emp_id") & "'", Session("con")) & "</td>"
                                    outp &= "<td>" & CDate(rs2.Item("app_date")).ToShortDateString & "</td>"
                                    outp &= "<td >" & fm.getfullname(rs2.Item("who_app"), Session("con")) & "</td>"
                                    outp &= "<td>" & rs2.Item("year_app") & "</td>"
                                    outp &= "<td >" & rs2.Item("app_result") & "</td>"
                                    outp &= "<td id='sumr" & emptid & "'>" & rs2.Item("app_result") & "</td></tr>" & Chr(13)
                                    cnt = cnt + 1
                                End If

                                If cntx > 1 And innerc = cntx Then
                                    If IsNumeric(sumpoint) And sumpoint > 0 Then
                                        sumresult = (sumpoint / cntx).ToString
                                    End If
                                    outp &= "<script> $('#sumr" & emptid & "').text('" & sumresult & "');</script>"
                                    ' Response.Write(cntx & "------" & innerc & "=======" & sumresult & "<br>")
                                End If

                            End While


                        End If
                        rs2.Close()
                    End If


                End While
                outp &= "</table>"

                Dim loc As String
                loc = Session("path")
                loc = loc.Replace("\", "/")
                loc &= "/download/aprs" & Now.Ticks.ToString & ".txt"
                '  Response.Write(loc)
                File.WriteAllText(loc, outp)
                Dim outp2 As String
                outp2 = Chr(13) & "<div id='clickexp' class='clickexp' style=' float:left; border:none; width:150px;height:28px; " & _
                            "background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer; " & _
                           " display:block;' onclick=" & Chr(34) & "javascript:exportxx('apprisal(apr-" & Now.Ticks.ToString & ")','xls','" & loc & "','export','2;3');" & Chr(34) & " >" & _
                           "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>" & _
                           "<div id='pinta4'  style='display:block;width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;'" & _
                                     " onclick=" & Chr(34) & "javascirpt:print('apprpt','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "','printA4app');" & Chr(34) & ">" & _
                                "<img src='images/png/printer2.png' alt='print' height='28px' style='float:left;'/>print A4 </div>" & _
                               "<br>"

                Response.Write(outp2 & "<div id='apprpt'>" & outp & "</div>")

            Else
                Response.Write("Sorry there is no data in the database")
            End If
            rs.Close()
        End If
    End Function
End Class
