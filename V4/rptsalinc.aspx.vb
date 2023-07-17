Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient

Partial Class rptsalinc
    Inherits System.Web.UI.Page
    Function makeinc()
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim sql As String = ""
        Dim projname, projid, count As String
        projid = ""
        projname = ""
        Dim jva As String = ""
        Dim rows As Integer = 2
        Try
            If Request.Form("projname") <> "" Then
                Dim spl() As String
                spl = Request.Form("projname").Split("|")
                If spl.Length > 0 Then
                    projname = spl(0)
                    projid = spl(1)
                End If
            End If
            Dim head As String = "<div id='bigprint'><style> #tb1 { border:1px black solid; font-size:9pt;}#tb1 td { border-right:1px black solid; border-bottom:1px black solid;} #tb1 .tfield{ border:1px black solid; font-weight:bold; }#tb1 .thead{ font-size:14pt; font-weight:bold;}</style><table id='tb1' cellpading='0' cellspacing='0' style=''><tr ><td colspan='8' style='text-align:center;' class='thead'>" & Application("company_name") & "<br>Increments Report<br>Project: " & projname & "</td></tr>"
            head &= "<tr><td class='tfield'>SNo.</td><td class='tfield'>Name of Employee</td><td class='tfield'>Position</td><td class='tfield'>Hired date</td><td class='tfield'>Salary/Benefits</td><td class='tfield'>Previous</td><td class='tfield'>Current </td><td class='tfield'>Date of Increment</td></tr>"
            Dim body As String = ""
            Dim bodyc As String = ""
            Dim allbody As String = ""
           
            If projid <> "" Then
                Dim rtnvalue As String
                ' Response.Write(projid & pdate2.ToShortDateString)
                rtnvalue = fm.getprojemp(projid.ToString, Today.ToShortDateString, Session("con"))

                sql = "select * form emp_sal_info where emptid in(" & rtnvalue & ") and emptid not in(select emptid from emp_resign) order by emptid,date_start"
                rs = dbs.dtmake("salinc", "select * from emp_sal_info inner join emp_static_info as esi on esi.emp_id=emp_sal_info.emp_id where emptid in(" & rtnvalue & ") and emptid not in(select emptid from emp_resign) order by first_name", Session("con"))
                If rs.HasRows Then
                    Dim rtn As String = ""
                    Dim conid As String = ""
                    Dim i As Integer = 0
                    Dim allwo() As String
                    Dim color As String = "white"
                    Dim arrid(1) As String
                    arrid(0) = ""
                    While rs.Read
                        body = ""
                        bodyc = ""
                        rtn = ""
                        rows = 0

                        If fm.searcharray(arrid, rs.Item("emptid").ToString) = False Then
                            ReDim Preserve arrid(arrid.Length + 1)
                            arrid(arrid.Length - 2) = rs.Item("emptid")
                            arrid(arrid.Length - 1) = ""
                            rows = 1
                            i = i + 1
                            
                            If color = "#e8f1fa" Then
                                color = "#ffffff"
                            Else
                                color = "#e8f1fa"
                            End If
                            Try
                                rtn = pardim(rs.Item("emptid")).ToString
                            Catch ex As Exception
                                'Response.Write(rs.Item("emptid") & "===>" & ex.ToString)
                            End Try


                            If rtn <> "" Then
                                body = " <tr  style='background:" & color & ";'><td class='y-" & rs.Item("emptid") & "'> Per-diem</td>" & rtn & "</tr>" & Chr(13)
                                rows = rows + 1
                            End If
                            Try


                                allwo = allw(rs.Item("emptid"), color)
                                If allwo(0) <> "0" Then
                                    rows = rows + CInt(allwo(0))
                                    body &= allwo(1)
                                End If
                                bodyc = "<tr style='background-color:" & color & ";'><td style='background-color:" & color & ";' class='x-" & rs.Item("emptid") & "' rowspan='" & rows.ToString & "'> " & i & ".</td>" & _
                               " <td  style='background-color:" & color & ";' class='x-" & rs.Item("emptid") & "' rowspan='" & rows.ToString & "'> " & fm.getfullname(rs.Item("emp_id"), Session("con")) & ".</td>" & _
                              " <td class='x-" & rs.Item("emptid") & "' rowspan='" & rows.ToString & "'> " & fm.getinfo2("select position from emp_job_assign where emp_id='" & rs.Item("emp_id") & "' order by id desc", Session("con")) & ".</td>" & _
                              "<td class='x-" & rs.Item("emptid") & "' rowspan='" & rows.ToString & "'> " & CDate(fm.getinfo2("select hire_date from emprec where id='" & rs.Item("emptid") & "'", Session("con"))).ToShortDateString & ".</td>" & Chr(13)
                                bodyc &= " <td class='y-" & rs.Item("emptid") & "'> Salary</td>" & salary(rs.Item("emptid")) & "</tr>" & Chr(13)
                                allbody &= bodyc & body
                            Catch ex As Exception
                                Response.Write(ex.ToString & "<br>")
                            End Try
                            'outp &= "</tr>"
                            jva &= " $('.x-" & rs.Item("emptid") & "').attr('rowspan','" & rows & "');" & Chr(13)

                            conid = rs.Item("emptid")

                        End If



                    End While
                End If

                allbody = head & allbody & "</table><br>NT-None taxable<br>T- Taxable</div>"
                ' outp = "<script language='javascript' type='text/javascript'> $(document).ready(function() {" & jva & "});</script>" & outp
                Return allbody

            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Function
        ' Dim keyp As String = ""

    Function salary(ByVal emptid As String)
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim cc As Integer = 0
        Dim cp As Integer = 0
        Dim oup(2) As String
        Dim oup2(2) As Double
        Dim dtinc As String
        Dim outp As String = ""
        oup(0) = ""
        oup(1) = ""
        cc = fm.getinfo2("select count(id) as exp1 from emp_sal_info where emptid=" & emptid, Session("con"))
        If cc > 1 Then
            rs = dbs.dtmake("salex", "select * from emp_sal_info where emptid=" & emptid & " order by date_start desc", Session("con"))

            While rs.Read
                If cp < 2 Then
                    oup2(cp) = CDbl(rs.Item("basic_salary").ToString)

                End If


                cp += 1
            End While
        Else
            oup2(0) = fm.getinfo2("select basic_salary from emp_sal_info where emptid=" & emptid, Session("con")).ToString
            oup2(1) = 0
        End If
        outp = "<td style='text-align:right;'>" & FormatNumber(oup2(1), 2, TriState.True, TriState.True, TriState.True) & "</td><td style='text-align:right;'>" & FormatNumber(oup2(0), 2, TriState.True, TriState.True, TriState.True) & "</td><td>" & CDate(fm.getinfo2("select date_start from emp_sal_info where emptid=" & emptid & " order by date_start desc", Session("con"))).ToShortDateString & "</td>"
        dbs = Nothing
        Return outp
    End Function
    Function pardim(ByVal emptid As String)
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim cc As Integer = 0
        Dim cp As Integer = 0
        Dim oup(2) As String
        Dim oup2(2) As Double
        Dim dtinc As String
        Dim outp As String = ""
        oup2(0) = 0
        oup2(1) = 0
        Try

        
        cc = fm.getinfo2("select count(id) as exp1 from emp_pardime where emptid=" & emptid, Session("con"))
        If cc > 1 Then
            rs = dbs.dtmake("salex", "select * from emp_pardime where emptid=" & emptid & " order by from_date desc", Session("con"))
            While rs.Read
                If cp < 2 Then
                    oup2(cp) = CDbl(rs.Item("pardime"))
                End If
                cp += 1
            End While
            rs.Close()
        Else
            If IsNumeric(fm.getinfo2("select pardime from emp_pardime where emptid=" & emptid & " and to_date is null", Session("con"))) Then
                    oup(0) = fm.getinfo2("select pardime from emp_pardime where emptid=" & emptid, Session("con")).ToString
                    oup(1) = 0
            Else
                    oup(1) = fm.getinfo2("select pardime from emp_pardime where emptid=" & emptid, Session("con")).ToString
                    oup(0) = 0
                End If
                If IsNumeric(oup(0)) Then
                    oup2(0) = oup(0)
                Else
                    oup2(0) = 0
                End If
                If IsNumeric(oup(1)) Then
                    oup2(1) = oup(1)
                Else
                    oup2(1) = 0
                End If

        End If
        If oup2(0) > 0 Or oup2(1) > 0 Then
            outp = "<td style='text-align:right;'>" & FormatNumber(oup2(1), 2, TriState.True, TriState.True, TriState.True) & "</td><td style='text-align:right;'>" & FormatNumber(oup2(0), 2, TriState.True, TriState.True, TriState.True) & "</td><td>" & CDate(fm.getinfo2("select from_date from emp_pardime where emptid=" & emptid & " order by from_date desc", Session("con"))).ToShortDateString & "</td>"

            End If
        Catch ex As Exception
            outp = ""
            Response.Write(ex.ToString & "<br>")
        End Try
        dbs = Nothing
        'Response.Write("<b>" & outp & "</b></br>")
        Return outp
    End Function
    Function allw(ByVal emptid As String, ByVal color As String)
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim cc As Integer = 0
        Dim cp As Integer = 0
        Dim oup(2) As String
        Dim dtinc As String
        Dim outp As String = ""
        Dim typex As String = ""
        Dim outp1 As String = ""

        Dim ccrow As Integer = 0
        Dim rn(2) As String
        rn(0) = "0"
        rn(1) = ""
        Try


            cc = fm.getinfo2("select count(id) as exp1 from emp_alloance_rec where emptid=" & emptid & " and to_date is null", Session("con"))
            If cc > 0 Then
                rs = dbs.dtmake("salex", "select * from emp_alloance_rec where emptid=" & emptid & " and to_date is null order by from_date desc", Session("con"))
                While rs.Read
                    If typex <> rs.Item("allownace_type") Then
                        typex = rs.Item("allownace_type")
                        outp1 = all_type(emptid, typex, color)
                        If outp1 <> "" Then
                            ccrow = ccrow + 1
                            outp &= outp1
                        End If

                    End If
                End While
                rs.Close()
            End If
        Catch ex As Exception
            Response.Write("<br><b>" & ex.ToString & "allw</b>")

        End Try
        If ccrow > 0 Then
            rn(0) = ccrow.ToString
            rn(1) = outp
        End If
        dbs = Nothing
        Return rn
    End Function
    Function all_type(ByVal emptid As String, ByVal typex As String, ByVal color As String)
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim cc As Integer = 0
        Dim cp As Integer = 0
        Dim oup(2) As String
        Dim oup2(2) As Double
        Dim dtinc As String
        Dim outp As String = ""
        Dim sql As String = ""
        oup2(0) = 0
        oup2(1) = 0
        oup(0) = ""
        oup(1) = ""
        Try
            sql = "select count(id) as exp1 from emp_alloance_rec where emptid=" & emptid & " and allownace_type='" & typex & "'"
            cc = fm.getinfo2(sql, Session("con"))
            If cc > 1 Then
                rs = dbs.dtmake("salex", "select * from emp_alloance_rec where emptid=" & emptid & " and allownace_type='" & typex & "' order by from_date desc", Session("con"))
                While rs.Read
                    If cp < 2 Then
                        oup2(cp) = CDbl(rs.Item("amount"))
                        If rs.Item("istaxable") = "n" Then
                            oup(cp) &= "<sup>NT</sup>"
                        Else
                            oup(cp) &= "<sup>T</sup>"
                        End If
                    End If
                    cp += 1
                End While
                rs.Close()
            ElseIf cc = 1 Then
                If IsNumeric(fm.getinfo2("select amount from emp_alloance_rec where emptid=" & emptid & "  and allownace_type='" & typex & "' and to_date is null", Session("con"))) Then
                    oup2(0) = fm.getinfo2("select amount from emp_alloance_rec where emptid=" & emptid & "  and allownace_type='" & typex & "' and to_date is null", Session("con"))
                    oup2(1) = 0
                    If fm.getinfo2("select istaxable from emp_alloance_rec where emptid=" & emptid & "  and allownace_type='" & typex & "' and to_date is null", Session("con")) = "n" Then
                        oup(0) &= "<sup>NT</sup>"
                    Else
                        oup(0) &= "<sup>T</sup>"
                    End If
                Else
                    oup2(1) = fm.getinfo2("select amount from emp_alloance_rec where emptid=" & emptid & "  and allownace_type='" & typex & "'", Session("con"))
                    oup2(0) = 0
                    If fm.getinfo2("select istaxable from emp_alloance_rec where emptid=" & emptid & "  and allownace_type='" & typex & "'", Session("con")) = "n" Then
                        oup(1) &= "<sup>NT</sup>"
                    Else
                        oup(1) &= "<sup>T</sup>"
                    End If
                End If


            End If
            If oup2(0) > 0 Or oup2(1) > 0 Then
                outp = "<tr  style='background:" & color & ";'><td>" & typex & "<td style='text-align:right;'>" & FormatNumber(oup2(1), 2, TriState.True, TriState.True, TriState.True) & oup(1) & "</td><td style='text-align:right;'>" & FormatNumber(oup2(0), 2, TriState.True, TriState.True, TriState.True) & oup(0) & "</td><td>" & CDate(fm.getinfo2("select from_date from emp_alloance_rec where emptid=" & emptid & "  and allownace_type='" & typex & "' order by from_date desc", Session("con"))).ToShortDateString & "</td></tr>"

            End If
            dbs = Nothing

            Return outp
        Catch ex As Exception
            Response.Write(ex.ToString & "nowwww 313<br>")
        End Try

    End Function
End Class


