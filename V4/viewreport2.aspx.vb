Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class viewreportx
    Inherits System.Web.UI.Page
    Function getout()
        For Each k As String In Request.QueryString
            Session(k) = Request.QueryString(k)
        Next
        Dim title As String = Session("company_name")
        Dim sql As String = ""
        Dim out As String = ""
        Dim active As String = ""
        Dim fm As New formMaker
        Dim rt() As String = {"", "", ""}
        If Request.QueryString.HasKeys = True Then
            active = Request.QueryString("active")

            If active <> "" Then
                If active = "y" Then
                    active = "(Active Employees)"
                Else
                    active = "(Deactive Employess)"
                End If
            Else
                active = "(All Employees)"
            End If
            Select Case (Request.QueryString("val"))
                Case "all"
                    sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.end_date is null ORDER BY emp_static_info.first_name, emprec.id DESC"
                    out = makeformx(sql)
                    title &= "<br>List of Emplyee's "
                Case "bydep"

                    Dim dep() As String
                    dep = Request.QueryString("department").Split("|")
                    title &= "<br>List of Emplyee's Filtered by Department " & dep(1) & " " & active
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) order by id desc"

                    End If

                    ' Response.Write(sql)

                    out = makeformx(sql)
                Case "byprojdate"

                    sql = ""
                    If Request.QueryString("projdateto") = "" And Request.QueryString("projdate") <> "" Then
                        If Request.QueryString("active") <> "" Then
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from >='" & Request.QueryString("projdate") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                        Else
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from >='" & Request.QueryString("projdate") & "') order by id desc"

                        End If
                    ElseIf Request.QueryString("projdateto") <> "" And Request.QueryString("projdate") <> "" Then
                        If Request.QueryString("active") <> "" Then
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                        Else
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') order by id desc"

                        End If
                    Else
                        out = "Sorry date is not selected"
                    End If
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If

                Case "byproj"
                    sql = ""
                    Dim dep() As String
                    Dim rtnvalue As String
                    dep = Request.QueryString("projx").Split("|")
                    'title &= "<br>List of Emplyee's Filtered by Project " & dep(1) & " " & active
                    'project is joined to empjobassign
                    ' Response.Write(dep(0))
                    If dep.Length <= 1 Then

                        dep = Request.QueryString("projx").Split("|")
                        ReDim Preserve dep(2)
                        dep(1) = ""
                        dep(0) = ""

                    End If
                    ' rtnvalue = fm.getprojemp(dep(1).ToString, Today.ToShortDateString, Session("con"))
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where id in(select emptid from emp_job_assign where project_id='" & _
                        dep(1) & "' and date_end is null) and  emprec.active='y'  ORDER BY emp_static_info.first_name, emprec.id DESC"
                    Else
                        sql = "select * from emprec inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where emprec.id in(select emptid from emp_job_assign where project_id='" & _
                         dep(1) & "' and date_end is null) and  emprec.active='y' ORDER BY emp_static_info.first_name, emprec.id DESC"

                    End If
                    title &= "<br>List of Emplyee's Filtered by Project " & dep(0) & " " & active
                    ' Response.Write(sql)
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If
                Case "bydis"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Field of Study " & Request.QueryString("discipline") & " " & active
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "') order by id desc"
                    End If

                    If sql <> "" Then
                        out = makeformx(sql)
                    End If 'Response.Write(sql)

                    ' For Each k As String In Request.QueryString
                    'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                    'Next
                Case "byrectime"
                    ' For Each k As String In Request.QueryString
                    'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                    'Next
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where hire_date between '" & Request.QueryString("recdate") & "' and '" & Request.QueryString("recdateto") & "' and active='" & Request.QueryString("active") & "' order by hire_date, id desc"
                    Else
                        sql = "select * from emprec where hire_date between '" & Request.QueryString("recdate") & "' and '" & Request.QueryString("recdateto") & "' order by hire_date, id desc"


                    End If
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If
                Case "byqual"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Qualification " & Request.QueryString("qualification") & " " & active
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') order by id desc"
                    End If
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If 'Response.Write(sql)

                Case "bypost"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by position " & Request.QueryString("position") & " " & active
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where id in(select emptid from emp_job_assign where position='" & Request.QueryString("position") & "' and date_end is null ) and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where id in(select emptid from emp_job_assign where position='" & Request.QueryString("position") & "' )  order by id desc"
                    End If
                    If sql <> "" Then
                        out = makeformx(sql)
                        'Response.Write(sql)
                    End If
                Case "pp"
                    '45 days workers
                    sql = "select * from emprec where dateadd(d,45,hire_date)>'" & Today.ToShortDateString & "'"
                    ''Response.Write(sql)
                    out = makeformx(sql)
                Case "namex"
                    Dim name() As String
                    name = Request.QueryString("vname").Split(" ")
                    If name.Length > 3 Then
                        If name(0).ToString <> " " Then
                            sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%')  order by id desc"

                        End If
                    ElseIf name.Length = 3 Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%' and middle_name like '%" & name(1) & "%' and last_name like '%" & name(2) & "%')  order by id desc"

                    ElseIf name.Length = 1 Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%')  order by id desc"

                    End If

                    'Response.Write(sql)
                    out = makeformx(sql)

            End Select
        End If
        rt(0) = title
        rt(1) = out
        rt(2) = sql
        Return rt
    End Function
    Public Function makeformx(ByVal sql As String)
        Dim fullname, position, sal(), proj As String
        ' Dim sql As String = ""
        Dim nrow As Integer = 0
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim dt As DataTableReader
        Dim out As String = ""
        Dim fld(,) As String
        Dim empx(1) As String
        Dim active As String = ""
        Dim rowspan As Integer = 0
        Dim col(15) As String
        Dim color As String = ""
        Dim color2 As String = ""
        Dim isexp As Boolean = False
        Dim retu As Boolean
        Dim sec As New k_security
        empx(0) = ""
        Dim i As Integer = 0
        Dim sumon As String = ""
        dt = dbs.dtmake("md", sql, Session("con"))
        If dt.HasRows Then
            out = ""
            While dt.Read
                rowspan = 0
                'Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)
                retu = fm.searcharray(empx, sec.Str2ToHex(dt.Item("emp_id"))).ToString
                ' Response.Write(retu.ToString & "<br>")
                If retu = False Then
                    ' Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)

                    ReDim Preserve empx(i + 1)
                    empx(i) = sec.Str2ToHex(dt.Item("emp_id"))


                    'get leave info here
                    col(0) = (i).ToString
                    Dim oldcolor As String = ""
                    Dim rs As DataTableReader
                    rs = dbs.dtmake("leave", "select * from show_leave_bal where emptid=" & dt.Item("id") & " order by 'year end'", Session("con"))
                    Dim tbgt, usedx, avail, bal, expbal As Double
                    tbgt = 0
                    Dim paidbymony As Double = 0
                    If rs.HasRows = True Then
                        Dim no_row As String = fm.getinfo2("select count(id) from show_leave_bal where emptid=" & dt.Item("id"), Session("con"))
                        ' Response.Write(no_row)
                        If color = "#e8f1fa" Then
                            color = "white"
                        Else
                            color = "#e8f1fa"
                        End If
                        If IsNumeric(no_row) = True Then
                            out &= "<tr style='background-color:" & color & ";'><td rowspan='" & no_row & "'>" & (i + 1).ToString & "</td>"
                            fullname = fm.getfullname(dt.Item("emp_id"), Session("con"))
                            out &= "<td rowspan='" & no_row & "'>" & dt.Item("id") & "</td><td rowspan='" & no_row & "'><a href='dataallview.aspx?empid=" & dt.Item("emp_id") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fullname & "</a></td>"
                            i = i + 1
                        End If
                        Dim flg As Integer = 0
                        Dim ndav As Double

                        tbgt = 0
                        usedx = 0
                        avail = 0
                        bal = 0
                        expbal = 0
                        paidbymony = 0
                        Dim paidam As Double
                        Dim pdst As String

                        While rs.Read
                            pdst = fm.getinfo2("select paidamt from leav_settled where bgtid=" & rs.Item("id"), Session("con"))
                            If IsNumeric(pdst) Then
                                paidam = pdst
                                pdst = "paid"
                            End If

                            isexp = fm.isexp(rs.Item("year End"), Today.ToShortDateString, 2, "y")
                            ndav = fm.showavdate(rs.Item("Year Start"), rs.Item("Year End"), rs.Item("Budget"))
                            ' Response.Write("<br>" & tbgt & "+" & rs.Item("Budget") & "=" & (tbgt + rs.Item("Budget")))
                            If isexp Then
                                If pdst = "paid" Then
                                    paidbymony += Math.Round(CDbl(ndav) - CDbl(rs.Item("Used")), 2)
                                End If
                                expbal += Math.Round(CDbl(ndav) - CDbl(rs.Item("Used")), 2)

                            Else
                                tbgt += CDbl(rs.Item("Budget"))
                                usedx += Math.Round(CDbl(rs.Item("Used")), 2)
                                avail += Math.Round(CDbl(ndav), 2)
                                bal += Math.Round(CDbl(ndav) - CDbl(rs.Item("Used")), 2)
                            End If


                            If flg = 0 Then
                                If pdst = "paid" Then
                                    oldcolor = color
                                    color = "pink"

                                End If
                                If isexp Then
                                    color2 = "red"
                                    If color = "pink" Then
                                        color2 = color
                                    End If
                                    out &= "<td style='background-color:" & color2 & ";'>" & (CDate(rs.Item("Year End")).Year).ToString & "</td>"
                                    out &= "<td style='background-color:" & color2 & ";'>" & rs.Item("Budget").ToString & "</td>"

                                    out &= "<td style='background-color:" & color2 & ";' >" & Math.Round(ndav, 2).ToString & "</td>"
                                    out &= "<td style='background-color:" & color2 & ";'>" & rs.Item("Used").ToString & "</td>"

                                    out &= "<td style='background-color:" & color2 & ";'>" & Math.Round((ndav - CDbl(rs.Item("Used"))), 2).ToString & "</td>"
                                    out &= "<td style='background-color:" & color2 & ";'>yes</td>"
                                    If pdst = "paid" Then
                                        out &= "<td style='background-color:" & color & ";'>Paid</td>"
                                    Else
                                        out &= "<td style='background-color:" & color2 & ";'>-</td>"
                                    End If
                                    out &= "</tr>"
                                Else
                                    out &= "<td>" & (CDate(rs.Item("Year End")).Year).ToString & "</td>"
                                    out &= "<td>" & rs.Item("Budget").ToString & "</td>"
                                    out &= "<td>" & FormatNumber(ndav, 2).ToString & "</td>"
                                    out &= "<td>" & FormatNumber(rs.Item("Used"), 2).ToString & "</td>"

                                    out &= "<td>" & FormatNumber((ndav - CDbl(rs.Item("Used"))), 2).ToString & "</td>"
                                    out &= "<td>-</td>"
                                    If pdst = "paid" Then
                                        out &= "<td>Paid</td>"
                                    Else
                                        out &= "<td>-</td>"
                                    End If

                                    out &= "</tr>"
                                End If

                                flg = 1
                            Else
                                If isexp Then
                                    color = "red"
                                Else
                                    color = ""

                                End If
                                If pdst = "paid" Then
                                    oldcolor = color
                                    color = "pink"

                                End If
                                If Math.Round((ndav - CDbl(rs.Item("Used"))), 2).ToString = 0 Then
                                    color = "gray"
                                End If

                                out &= "<tr  style='background-color:" & color & ";'>"
                                out &= "<td>" & (CDate(rs.Item("Year End")).Year).ToString & "</td>"
                                out &= "<td>" & rs.Item("Budget").ToString & "</td>"
                                out &= "<td>" & Math.Round(ndav, 2).ToString & "</td>"
                                out &= "<td>" & FormatNumber(rs.Item("Used"), 2).ToString & "</td>"

                                out &= "<td>" & Math.Round((ndav - CDbl(rs.Item("Used"))), 2).ToString & "</td>"
                                If color = "red" Then
                                    out &= "<td style='border-top:1px solid black;'>yes</td>"
                                Else
                                    out &= "<td style='border-top:1px solid black;'>-</td>"
                                End If
                                If color = "pink" Then
                                    out &= "<td style='border-top:1px solid black;'>paid</td>"
                                Else

                                    out &= "<td  style='border-top:1px solid black;'>-</td>"
                                End If

                                out &= "</tr>"
                                End If
                            If color = "pink" Then
                                color = oldcolor
                            End If
                        End While


                    End If
                    If tbgt > 0 Then
                        Dim unpaid As Double
                        unpaid = expbal - paidbymony
                        out &= "<tr><td colspan='8'><b>Total Bugeted:" & tbgt.ToString & " | Available:" & avail.ToString & " | Used:" & usedx.ToString & " | Balance:" & bal.ToString & " | Exp. Bal: " & expbal.ToString & " | Paid Bal:" & paidbymony.ToString
                        If unpaid > 0 Then
                            out &= " | unpaid bal: " & unpaid.ToString
                        End If
                        out &= "</td></tr>"
                    End If
                    rs.Close()


                    '  i = i + 1
                End If



            End While
            dt.Close()
            'Response.Write("<table cellpading='2' cellspacing='2' bordercolor='blue'>" & out & "</table>")
        End If
        If out = "" Then
            out = "<tr><td colspan='15' style='color:red;font-size:30pt;'>Sorry! Data are not Found</td></tr>"
        End If
        dbs = Nothing
        fm = Nothing
        Return out
    End Function
End Class
