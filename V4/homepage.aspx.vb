Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.Web
Imports System.IO



Partial Class homepage
    Inherits System.Web.UI.Page
    Public passno As Integer = 0
    Function emponleave()
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim sql As String
        Dim out As String = ""
        sql = "select * from emp_leave_take where date_taken_from<='" & Now.ToShortDateString.ToString & "' and date_return>'" & Now.ToShortDateString.ToString & "' order by date_return"
        Dim rs As DataTableReader
        'Dim currentCapabilities As Mobile.MobileCapabilities = CType(Request.Browser, Mobile.MobileCapabilities)

        rs = dbs.dtmake("getleave", sql, Session("con"))
        'If currentCapabilities.Browser <> "IE" Then
        ' out &= "Best viewed with Internet Explorer"

        'End If

        out &= "<table><tr style='color:blue;font-weight:bold;'><td>Name </td><td>Project</td><td>Leave <br>type</td><td>Return Date</td></tr>"
        If rs.HasRows Then
            While rs.Read
                out &= "<tr>"
                out &= "<td>" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td>"
                out &= "<td>" & fm.getinfo2("select project_name from tblproject where project_id in(select project_id from emp_job_assign where emptid=" & rs.Item("emptid") & " and date_end is Null)", Session("con")) & "</td>"
                out &= "<td>" & rs.Item("leave_type") & "</td>"
                out &= "<td style='cursor:pointer' title='Will be on Duety " & rs.Item("date_return").tolongdatestring & "'>" & rs.Item("date_return").toshortdatestring & "</td>"

                out &= "</tr>"
                out &= "<tr><td colspan='4'><hr/></td></tr>"
            End While
        Else
            out &= "<tr>"
            out &= "<td>Temporary data is not available</td>"

            out &= "</tr>"
        End If
        out &= "</table>"
        Return out
    End Function
    Function contract_list(ByVal d As Integer)
        Dim hr As String
        Dim contd As String
        Dim fm As New formMaker
        Dim ds As New dbclass
        Dim rs, rs2 As DataTableReader
        Dim fname As String
        Dim dtrm As Integer
        Dim rtn() As String = {""}
        Dim i As Integer = 0
        Dim outp As String = "<table style='  width:800px;'><tr><td colspan='6' style='font-size:11pt;'><b>Contract Employee's</b></td></tr>" & _
            "<tr style=' text-align:center; font-weight:bold;color:blue;'>" & _
            "<td style='''>&nbsp;</td><td style='text-align:center;'>Employee's Name</td>" & _
            "<td style=''>Employment date</td>" & _
            "<td style=''>Contract<br>Start/Renew Date</td>" & _
            "<td style=''>Contract End</td>" & _
            "<td style=''>Remaining Days </td></tr>" & _
            "<tr style='margin-top:-5px;'><td colspan='6' style='height:5px;text-align:top;'><hr  style='border-color:blue;' valign='top'></hr></td></tr>"
        rs = ds.dtmake("contx", "select * from emprec where type_recuritment='contract' and end_date is null order by hire_date desc", Session("con"))
        Dim strpass As String = ""
        Dim sec As New k_security
        If rs.HasRows Then
            While rs.Read
                fname = fm.getfullname(rs.Item("emp_id"), Session("con"))

                rs2 = ds.dtmake("dbcont", "select * from emp_contract where emptid=" & rs.Item("id") & " and (status='n' or status is null) order by id desc", Session("con"))
                If rs2.HasRows Then
                    rs2.Read()
                    i = i + 1
                    dtrm = CDate(rs2.Item("dateend")).Subtract(Today).Days
                    'Response.Write(Today)
                    If dtrm < d And dtrm >= 0 Then
                        ReDim Preserve rtn(rtn.Length + 1)


                        strpass = "<tr><td style=''>" & i.ToString & "</td>" & _
                        "<td style=''>" & fname & "</td>" & _
                        "<td style='text-align:center;'>" & rs.Item("hire_date") & "</td>" & _
                        "<td style=text-align:center;'>" & rs2.Item("datestart") & "</td>" & _
                        "<td style='text-align:center;'>" & rs2.Item("dateend") & "</td>" & _
                        "<td style='text-align:center;'>" & dtrm.ToString & "</td></tr>"
                        rtn(rtn.Length - 1) = sec.dbStrToHex(strpass)
                    End If
                End If
            End While
        End If

        rs.Close()
        ds = Nothing
        fm = Nothing
        Return rtn
    End Function
    Function contract_list()
        Dim hr As String
        Dim contd As String
        Dim fm As New formMaker
        Dim ds As New dbclass
        Dim rs, rs2 As DataTableReader
        Dim fname As String
        Dim dtrm As Integer
        Dim rtn() As String = {""}
        Dim i As Integer = 0
        Dim outp As String = "<table style='  width:800px;'><tr><td colspan='6' style='font-size:11pt;'><b>Contract Employee's</b></td></tr>" & _
            "<tr style=' text-align:center; font-weight:bold;color:blue;'>" & _
            "<td style='''>&nbsp;</td><td style='text-align:center;'>Employee's Name</td>" & _
            "<td style=''>Employment date</td>" & _
            "<td style=''>Contract<br>Start/Renew Date</td>" & _
            "<td style=''>Contract End</td>" & _
            "<td style=''>Remaining Days </td></tr>" & _
            "<tr style='margin-top:-5px;'><td colspan='6' style='height:5px;text-align:top;'><hr  style='border-color:blue;' valign='top'></hr></td></tr>"
        rs = ds.dtmake("contx", "select * from emprec where type_recuritment='contract' and end_date is null order by hire_date desc", Session("con"))
        If rs.HasRows Then
            While rs.Read
                fname = fm.getfullname(rs.Item("emp_id"), Session("con"))

                rs2 = ds.dtmake("dbcont", "select * from emp_contract where emptid=" & rs.Item("id") & " and (status='n' or status is null) order by id desc", Session("con"))
                If rs2.HasRows Then
                    rs2.Read()
                    i = i + 1
                    dtrm = CDate(rs2.Item("dateend")).Subtract(Today).Days
                    'Response.Write(Today)
                    If dtrm < 5 And dtrm >= 0 Then
                        ReDim Preserve rtn(rtn.Length + 1)
                        Dim sec As New k_security
                        Dim strpass As String
                        strpass = "<tr><td style=''>" & i.ToString & "</td>" & _
                        "<td style=''>" & fname & "</td>" & _
                        "<td style='text-align:center;'>" & rs.Item("hire_date") & "</td>" & _
                        "<td style=text-align:center;'>" & rs2.Item("datestart") & "</td>" & _
                        "<td style='text-align:center;'>" & rs2.Item("dateend") & "</td>" & _
                        "<td style='text-align:center;'>" & dtrm.ToString & "</td></tr>"
                        rtn(rtn.Length - 1) = sec.dbStrToHex(strpass)
                        outp &= strpass
                    ElseIf dtrm <= -1 Then
                        ' outp &= "<script> alert('" & rs.Item("id") & " has exp');</script>"
                        autoresign(rs.Item("id"))
                        outp &= "<tr title='auto resign has done' style='background-color:red'><td style=''>" & i.ToString & "</td>" & _
                      "<td style=''>" & fname & "</td>" & _
                      "<td style='text-align:center;'>" & rs.Item("hire_date") & "</td>" & _
                      "<td style=text-align:center;'>" & rs2.Item("datestart") & "</td>" & _
                      "<td style='text-align:center;'>" & rs2.Item("dateend") & "</td>" & _
                      "<td style='text-align:center;'>" & dtrm.ToString & "</td></tr>"

                    End If
                    ' outp &= "<tr><td style=''>" & i.ToString & "</td>" & _
                    '    "<td style=''>" & fname & "</td>" & _
                    '   "<td style='text-align:center;'>" & rs.Item("hire_date") & "</td>" & _
                    '  "<td style=text-align:center;'>" & rs2.Item("datestart") & "</td>" & _
                    ' "<td style='text-align:center;'>" & rs2.Item("dateend") & "</td>" & _
                    ' "<td style='text-align:center;'>" & dtrm.ToString & "</td></tr>"
                    ' Response.Write(i.ToString & ". " rs.Item("hire_date") & " ==>Contact end:" & rs2.Item("dateend") & "<br>")
                Else
                    ' outp &= "<tr><td style=''>" & i.ToString & "</td>" & _
                    '  "<td style=''>" & fname & "</td>" & _
                    ' "<td style='text-align:center;'>" & rs.Item("hire_date") & "</td>" & _
                    '"<td style=text-align:center;'>-</td>" & _
                    '"<td style='text-align:center;'>-</td>" & _
                    '"<td style='text-align:center;'>-</td></tr>"
                End If
                rs2.Close()
            End While
        End If
        outp &= "</table>"
        'Response.Write(outp)
        Return outp
        rs.Close()
        ds = Nothing
        fm = Nothing
    End Function
    Function autoresign(ByVal emptid As String)

        Dim fm As New formMaker
        Dim rdate As String
        rdate = fm.getinfo2("select dateend from emp_contract where emptid=" & emptid & " and (status is null or status='n') order by id desc", Session("con"))
        If rdate <> "None" Then
            Dim empid As String = ""
            empid = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))

            ' Response.Write(rdate)
            Dim sql As String = "Begin Transaction" & Chr(13)
            sql &= "update emprec set end_date='" & rdate & "',active='n' where id=" & emptid & Chr(13)
            sql &= "update emp_job_assign set date_end='" & rdate & "' where emptid=" & emptid & " and date_end is null" & Chr(13)
            sql &= "update emp_sal_info set date_end='" & rdate & "' where emptid=" & emptid & " and date_end is null" & Chr(13)
            sql &= "update emp_pardime set to_date='" & rdate & "' where emptid=" & emptid & " and to_date is null" & Chr(13)
            sql &= "update emp_alloance_rec set to_date='" & rdate & "' where emptid=" & emptid & " and to_date is null" & Chr(13)
            sql &= "insert into emp_resign(emptid,emp_id,reason,resign_date,who_reg,date_reg) values(" & _
                "'" & emptid & "','" & empid & "','auto resign','" & CDate(rdate).ToShortDateString & "','system','" & Today.ToShortDateString & "')" & Chr(13)
            sql &= "update emp_contract set status='y' where emptid=" & emptid & Chr(13)
            ' Response.Write(sql)
            Dim dbs As New dbclass
            Dim flg As String
            flg = dbs.excutes(sql, Session("con"), Session("path"))
            If IsNumeric(flg) Then
                If CInt(flg) > 0 Then
                    dbs.excutes("Commit", Session("con"), Session("path"))
                Else
                    dbs.excutes("RollBack", Session("con"), Session("path"))
                End If
            Else
                Response.Write(flg)
                dbs.excutes("RollBack", Session("con"), Session("path"))
            End If
        End If

    End Function
    Function loadrep()
        Dim fm As New formMaker
        Try


            Dim dbs As New dbclass

            Dim rs As DataTableReader
            Dim res As String = ""
            Dim outp As String
            Dim emptid, emp_id As String
            Dim bgcolor As String = "white"
            'Response.Write("whhho")
            Dim resigndate As Date
            rs = dbs.dtmake("vwres", "select * from emp_resign order by resign_date desc", Session("con"))
            If rs.HasRows = True Then
                outp &= "<table id='tb1' style='width:800px;'><tr style='font-weight:bold;'><td>Employee Name</td><td>Resign Date</td><td>Payroll paid</td><td>OT Unpaid</td><td>Leave Paid</td><td>Leave Balance</td><td>Leave Pay Amt</td><td>Closed</td></tr>"
                While rs.Read
                    res = ""
                    If bgcolor = "white" Then
                        bgcolor = "#1256ff"
                    Else
                        bgcolor = "white"
                    End If
                    emptid = rs.Item("emptid")
                    outp &= "<tr style='background:" & bgcolor & "'><td>" & rs.Item("emptid")
                    If IsNumeric(emptid) = True Then
                        'Response.Write(emptid & "<br>")
                        emp_id = fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con"))
                    Else
                        ' Response.Write(emptid)
                        emp_id = "00/00"
                        emptid = "0"
                    End If
                    If emp_id <> "None" Then
                        outp &= fm.getfullname(emp_id, Session("con"))
                        outp &= "</td>"
                        outp &= "<td>"
                        outp &= Format(rs.Item("resign_date"), "MMM dd, yyyy")
                        outp &= "</td>"
                        outp &= "<td>"
                        resigndate = rs.Item("resign_date")
                        If Day(resigndate) = 1 Then
                            resigndate = resigndate.AddDays(-1)
                        End If
                        res = fm.getinfo2("select id from payrollx where emptid=" & rs.Item("emptid") & " and (month(date_paid)='" & Month(resigndate) & "' and year(date_paid)='" & Year(resigndate) & "') and remark='monthly'", Session("con"))
                        If res <> "None" And res.Length < 6 Then
                            outp &= "Yes"
                        Else
                            outp &= "&nbsp;"
                        End If
                        outp &= "</td>"
                        outp &= "<td style='text-align:right;'>"
                        res = ""
                        res = fm.getinfo2("select sum(amt) from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'", Session("con"))
                        ' Response.Write(res)
                        If IsNumeric(res) Then
                            If CInt(res.ToString) > 0 Then
                                outp &= FormatNumber(res, 2, TriState.True, TriState.True, TriState.True)
                            Else
                                outp &= "0.00"
                            End If
                        Else
                            outp &= "0.00"
                        End If
                        outp &= "</td>"
                        outp &= "<td style='text-align:right;'>"
                        res = ""
                        res = fm.getinfo2("select sum(amt) as exp2 from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'", Session("con"))
                        ' Response.Write(res.ToString & "<br>...." & "select sum(amt) from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'")

                        If res <> "None" And res.Length < 6 And res <> "" Then

                            If CDbl(res) > 0 Then
                                outp &= FormatNumber(res, 2, TriState.True, TriState.True, TriState.True)
                            End If
                        Else
                            res = 0
                            outp &= "0.00"
                        End If
                        Dim lvba As String
                        lvba = fm.lvb(emptid, False, 0, rs.Item("resign_date"), Session("con")).ToString()
                        outp &= "</td><td style='text-align:right;'>"
                        outp &= FormatNumber(lvba.ToString(), 2)
                        Dim salary() As String
                        Dim hr As String
                        Dim unpaidamt As Double = 0
                        salary = dbs.getsal(emptid, rs.Item("resign_date"), Session("con"))
                        If IsNumeric(salary(0)) = False Then
                            ' Response.Write(salary)
                            salary(0) = "0.00"
                        End If
                        ' salary = dbx.getsal(emptid, Session("con"))
                        'salary = 3800
                        hr = CDbl(salary(0)) / 200.67
                        Dim amttop As Double = 0
                        ' Response.Write(res.ToString & "<br>")
                        If CDbl(lvba) > 0 Then
                            amttop = (CInt(lvba) * 8 * hr)
                            If (CDbl(lvba) - CInt(lvba)) > 0.5 Then
                                amttop += (0.5 * 8 * hr)
                            End If

                        End If

                        outp &= "</td><td>" & FormatNumber(amttop.ToString(), 2).ToString & "</td><td>"
                        If amttop > 0 Then
                            outp &= "<span onclick=" & Chr(34) & "javascript://balancep('" & emptid & "');" & Chr(34) & " style='color:blue; cursor:pointer;'>Open Leave Payment<span></td></tr>"
                        Else
                            outp &= "<span onclick=" & Chr(34) & "javascript:closedx('" & rs.Item("id").ToString & "');" & Chr(34) & " style='color:blue; cursor:pointer;'>Close<span></td></tr>"

                        End If
                    Else
                        outp &= "</td><td colspan=7>Data is not found manually removed!!!!</td></tr>"
                    End If 'End of emp_id=None
                    ' outp &= rs.Item("emptid").ToString & "<br>"
                End While
                outp &= "</table>"
            End If

            Return outp
            dbs = Nothing
            fm = Nothing
        Catch ex As Exception
            Response.Write(ex.ToString)
            fm.exception_hand(ex)
        End Try
    End Function
    ' under progress.....idea: unpaid salary active employess
    Function active_report()
        Dim fm As New formMaker
        Try


            Dim dbs As New dbclass

            Dim rs As DataTableReader
            Dim res As String = ""
            Dim outp As String
            Dim emptid, emp_id As String
            Dim bgcolor As String = "white"
            'Response.Write("whhho")
            Dim sql As String
            Dim pdate1, pdate2 As Date
            pdate1 = "#" & Today.Month & "/1/" & Today.Year & "#"
            pdate2 = pdate1.AddMonths(1)
            pdate2 = pdate2.AddDays(-1)
            sql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                      "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                      "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                      "and emprec.id in(select emptid from emp_job_assign " & _
                                                                      "where " & _
                                                                      "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                                                                      " ORDER BY emp_static_info.first_name,emprec.id desc "
            rs = dbs.dtmake("vwres", sql, Session("con"))
            If rs.HasRows = True Then
                outp &= "<table id='tb1' style='width:800px;'><tr style='font-weight:bold;'><td>Employee Name</td><td>Resign Date</td><td>Payroll paid</td><td>OT Unpaid</td><td>Leave Paid</td><td>Leave Balance</td><td>Leave Pay Amt</td><td>Closed</td></tr>"
                While rs.Read
                    res = ""
                    If bgcolor = "white" Then
                        bgcolor = "#1256ff"
                    Else
                        bgcolor = "white"
                    End If

                    outp &= "<tr style='background:" & bgcolor & "'><td>"
                    emptid = rs.Item("id")
                    emp_id = rs.Item("emp_id")
                    If emp_id <> "None" Then
                        outp &= fm.getfullname(emp_id, Session("con"))
                        outp &= "</td>"
                        outp &= "<td>"
                        outp &= Format(rs.Item("resign_date"), "MMM dd, yyyy")
                        outp &= "</td>"
                        outp &= "<td>"
                        'Salary Review and find skip months

                        res = fm.getinfo2("select id from payrollx where emptid=" & rs.Item("emptid") & " and (month(date_paid)='" & Month(rs.Item("end_date")) & "' and year(date_paid)='" & Year(rs.Item("resign_date")) & "') and remark='monthly'", Session("con"))
                        If res <> "None" And res.Length < 6 Then
                            outp &= "Yes"
                        Else
                            outp &= "&nbsp;"
                        End If
                        outp &= "</td>"
                        outp &= "<td style='text-align:right;'>"
                        res = ""
                        res = fm.getinfo2("select sum(amt) from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'", Session("con"))
                        ' Response.Write(res)
                        If IsNumeric(res) Then
                            If CInt(res.ToString) > 0 Then
                                outp &= FormatNumber(res, 2, TriState.True, TriState.True, TriState.True)
                            Else
                                outp &= "0.00"
                            End If
                        Else
                            outp &= "0.00"
                        End If
                        outp &= "</td>"
                        outp &= "<td style='text-align:right;'>"
                        res = ""
                        res = fm.getinfo2("select sum(amt) as exp2 from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'", Session("con"))
                        ' Response.Write(res.ToString & "<br>...." & "select sum(amt) from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'")

                        If res <> "None" And res.Length < 6 And res <> "" Then

                            If CDbl(res) > 0 Then
                                outp &= FormatNumber(res, 2, TriState.True, TriState.True, TriState.True)
                            End If
                        Else
                            res = 0
                            outp &= "0.00"
                        End If
                        Dim lvba As String
                        lvba = fm.lvb(emptid, False, 0, rs.Item("resign_date"), Session("con")).ToString()
                        outp &= "</td><td style='text-align:right;'>"
                        outp &= FormatNumber(lvba.ToString(), 2)
                        Dim salary() As String
                        Dim hr As String
                        Dim unpaidamt As Double = 0
                        salary = dbs.getsal(emptid, rs.Item("resign_date"), Session("con"))
                        If IsNumeric(salary(0)) = False Then
                            ' Response.Write(salary)
                            salary(0) = "0.00"
                        End If
                        ' salary = dbx.getsal(emptid, Session("con"))
                        'salary = 3800
                        hr = CDbl(salary(0)) / 200.67
                        Dim amttop As Double
                        ' Response.Write(res.ToString & "<br>")
                        If CDbl(lvba) > 0 Then
                            amttop = (CInt(lvba) * 8 * hr)
                            If (CDbl(lvba) - CInt(lvba)) > 0.5 Then
                                amttop += (0.5 * 8 * hr)
                            End If

                        End If

                        outp &= "</td><td>" & FormatNumber(amttop.ToString(), 2).ToString & "</td><td>"
                        If amttop > 0 Then
                            outp &= "<span onclick=" & Chr(34) & "javascript://balancep('" & emptid & "');" & Chr(34) & " style='color:blue; cursor:pointer;'>Open Leave Payment<span></td></tr>"
                        Else
                            outp &= "<span onclick=" & Chr(34) & "javascript:closedx('" & rs.Item("id").ToString & "');" & Chr(34) & " style='color:blue; cursor:pointer;'>Close<span></td></tr>"

                        End If
                    End If 'End of emp_id=None
                    ' outp &= rs.Item("emptid").ToString & "<br>"
                End While
                outp &= "</table>"
            End If

            Return outp
            dbs = Nothing
            fm = Nothing
        Catch ex As Exception
            Response.Write(ex.ToString)
            fm.exception_hand(ex)
        End Try
    End Function

    Function rpt_show() As String
        Dim rt As String = ""
        Dim db As New dbclass
        Dim rs As DataTableReader
        Dim color As String = "ffffff"
        rs = db.dtmake("mkrpt", "select * from rptdataupdate where seen=0 and (reseen='y' or reseen is null)", Session("con"))
        If rs.HasRows Then
            rt = "<table id='tb1' style='width:500px;'><tr style='font-weight:bold;'><td style='width:100px;'>Report type</td><td style='width:300px;'>Description</td><td style='width:100px;'>Report Date</td><td>seen</td>"
            While rs.Read
                If color = "ffffff" Then
                    color = "123691"
                Else
                    color = "ffffff"
                End If
                rt &= "<tr style='background:" & color & ";'>"
                rt &= "<td >" & rs.Item("reporttype") & "</td>"
                rt &= "<td>" & rs.Item("Report") & "</td>"
                rt &= "<td>" & rs.Item("datee") & "</td>"
                rt &= "<td><span onclick=" & Chr(34) & "javascript:seenon('" & rs.Item("id") & "','" & Session("emp_iid") & "','" & Now & "');" & Chr(34) & " style='cursor:pointer;color:blue;' title='Click here not see it again'>Seen</span></td>"
                rt &= "</tr>"

            End While
            rt &= "</table>"
        End If
        rptinc()
        Return rt
    End Function
    Function rptinc()
        Dim sql As String = "select emp_sal_info.*,first_name,middle_name,emp_static_info.emp_id from emp_sal_info inner join emprec on emprec.id=emp_sal_info.emptid inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where day(date_start)>1 and hire_date<>date_start order by date_start desc"
        Dim ds As New dbclass
        Dim rs As DataTableReader
        Dim outpx As String
        rs = ds.dtmake("inccc", sql, Session("con"))

        If rs.HasRows Then
            outpx = "<table><tr><td>Com. No</td><td>Emp. Id</td><td>Employee's Name</td><td>Date Increament</td></tr>"
            While rs.Read
                outpx &= ("<tr><td>" & rs.Item("emptid") & "</td><td>" & rs.Item("emp_id") & "</td><td>" & rs.Item("first_name") & " " & rs.Item("middle_name") & "</td><td>" & rs.Item("date_start") & "</td></tr>")
            End While
            outpx &= "</table>"
        End If
        Return outpx
    End Function
    Function rpttransfer()
        Dim sql As String = "select emp_job_assign.*,first_name,middle_name,emp_static_info.emp_id from emp_job_assign " & _
            "inner join emprec on emprec.id=emp_job_assign.emptid inner join " & _
            "emp_static_info on emprec.emp_id=emp_static_info.emp_id where " & _
           " hire_date<>date_from order by date_from desc"
        Dim ds As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim outp As String = ""
        Dim rrtn() As String
        Try
            rs = ds.dtmake("inccc", sql, Session("con"))

            If rs.HasRows Then
                outp = "<table><tr><td>Com. No</td><td>Emp. Id</td><td>Employee's Name</td><td>Date transpfer</td><td>From</td><td>To</td></tr>"
                While rs.Read
                    If DateDiff(DateInterval.Month, rs.Item("date_from"), Today) < 7 And rs.Item("ass_for") <> "Hired" Then
                        rrtn = fm.projtransid(rs.Item("emptid"), rs.Item("date_from"), Session("con"), Session("path"))
                        outp &= ("<tr><td>" & rs.Item("emptid") & "</td><td>" & rs.Item("emp_id") & "</td><td>" & rs.Item("first_name") & " " & rs.Item("middle_name") & "</td><td>" & rs.Item("date_from") & "</td>" & rrtn(0) & "</tr><tr><td colspan='6'><hr></td></tr>")
                    End If
                    ' Response.Write("<br>" & rrtn(1))

                End While
                outp &= "</table>"
            End If

            If outp = "" Then
                ' Response.Write(".........")
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
            fm.exception_hand(ex)
        End Try
        Return (outp)
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim page1 As String = ""
        Dim fl As New file_list
        Dim re As Object
        Dim rtx() As String
        Dim parts(5) As String
        Dim fm As New formMaker
        Dim fx() As String = {""}
        If String.IsNullOrEmpty(Session("right")) = False Then
            fx = Session("right").split(";")
            ReDim Preserve fx(UBound(fx) + 1)
            fx(UBound(fx) - 1) = ""
        End If
        Session("page1") = ""
        If Session("page1") = "" Then
            page1 = " <form id='frmx' method='post' action=''><input type='hidden' name='hidpass' id='hidpass'/></form>"
            page1 &= "<div id='tabs'>"
            page1 &= " <ul style='background:white; border:none none;'>"
            page1 &= "<li id='w1'><a href='#tabs-1'>Wel-Come</a></li>"
            page1 &= "<li id='w2'><a href='#tabs-2'>Information</a></li>"
            page1 &= "<li id='w3'><a href='#tabs-3'>HR Only</a></li>"
            page1 &= "<li id='w4'><a href='#tabs-4'>Accounting Only</a></li>"
            '    page1 &= "<li id='w4'><a href='#tabs-5'>Unpaid list</a></li>"
            page1 &= " </ul>"
            parts(0) = ""
            parts(0) &= "<div id='tabs-1' style='min-height:500px;'>"
            If fm.searcharray(fx, "1") Then
                parts(0) &= "<form id='uphtml' name='uphtml' enctype='multipart/form-data' method='post' action=''><input type='file' id='flupload' name='flupload' onchange=" & Chr(34) & "javascript:$('#uphtml').submit();" & Chr(34) & " /><input type='hidden' name='passh' value='ok'><br><label style='color:gray;font-size:8pt;'>upload file type: .htm or .html</label></form> "
                Try


                    If Request.Form("passh") <> "" Then
                        Dim typ() As String = {".htm", ".html"}
                        re = fl.fupload(Request.Files("flupload"), "c:\temp\welcom\", 1550000, typ, "welcome")
                        parts(0) &= (re.ToString)

                    End If
                Catch ex As Exception
                    'fm.exception_hand(ex)
                End Try
                ' Dim sec As New k_security
                'Dim secval As String = sec.Kir_StrToHex("493 15kir")
                ' Response.Write(secval)
                'Response.Write("<br>" & sec.Kir_HexToStr(secval))

            End If

            parts(0) &= getwelcom()
            parts(0) &= Session("efrom") & "</div>"

            parts(1) = ""
            parts(1) &= "<div id='tabs-2' style=' display:none;'>"
            parts(1) &= "<div class='templatemo_leftcol_subcol' style='float:left;'>"
            parts(1) &= "<div class='regular_section'>"
            parts(1) &= "<h2>Employee's on leave</h2>" & _
    "<div class='newsbox'><p>" & _
        emponleave() & _
    "  </p>				  </div>"
            parts(1) &= "                </div>"
            Dim sql As String = "select ROW_NUMBER() OVER(ORDER BY id) AS 'row number', * from emprec where dateadd(d,45,hire_date)>'" & Today.ToShortDateString & "' order by 'row number' desc"
            Dim db As New dbclass
            Dim rs As DataTableReader



            'Response.Write(fm.searcharray(fx, "1"))
            rs = db.dtmake("emprecx", sql, Session("con"))
            If rs.HasRows And (fm.searcharray(fx, "1") Or fm.searcharray(fx, "2") Or fm.searcharray(fx, "3") Or fm.searcharray(fx, "4")) Then
                parts(1) &= "                        <div class='regular_section'>"
                parts(1) &= "<h2>Probational Period</h2>"
                parts(1) &= "<div class='newsbox'>"
                Dim fname As String = ""
                Dim hrd, dateend As Date
                parts(1) &= "<table><tr style='color:blue;'><td>Name</td><td>Hire Date</td><td>P. End</td><td>Remain</td></tr>"
                While rs.Read
                    hrd = rs.Item("hire_date")
                    dateend = hrd.AddDays(45)
                    fname = fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & rs.Item("id"), Session("con")), Session("con"))
                    parts(1) &= "<tr><td>" & fname & "</td><td>" & hrd.ToShortDateString & "</td><td> " & _
                     dateend.ToShortDateString & "</td><td>" & Today.Subtract(dateend).Days & " Days</td></tr><tr><td colspan='4'><hr></td></tr>"

                End While
                parts(1) &= "</table>"

                parts(1) &= " </div>"
                parts(1) &= "</div>"


            End If
            parts(1) &= "</div>"
            parts(1) &= "  <div id='Div1' style='float:left;width:400px; height:300px;'>"

            If (fm.searcharray(fx, "1") Or fm.searcharray(fx, "2") Or fm.searcharray(fx, "3") Or fm.searcharray(fx, "4")) Then
                parts(1) &= "<div style=' width:400px;' >"
                parts(1) &= "<iframe style='width:600px; height:300px; opacity:0.1' scrolling='no' src='chartemp.aspx?chart=bar' frameborder='0' ></iframe>"
                parts(1) &= "<span onclick=" & Chr(34) & "javascript:openwin('chartemp.aspx?yr=on&chart=bar','Chart View');" & Chr(34) & " style='font-size:9pt; color:Blue; cursor:pointer;'>View Large</span>"
                parts(1) &= "</div>"
            End If

            parts(1) &= "</div>"
            parts(1) &= "<div id='Div2' style='float:left;width:300px; height:300px;'>"
            If (fm.searcharray(fx, "1") Or fm.searcharray(fx, "2") Or fm.searcharray(fx, "3") Or fm.searcharray(fx, "4")) Then
                parts(1) &= "<div style=' width:300px;' >"
                parts(1) &= "<iframe style='width:600px; height:300px;' scrolling='no' src='chartemp.aspx?chart=pie' frameborder='0' ></iframe>"
                parts(1) &= "<span onclick=" & Chr(34) & "javascript:openwin('chartemp.aspx?yr=on&chart=pie','Chart View');" & Chr(34) & " style='font-size:9pt; color:Blue; cursor:pointer;'>View Large</span>"
                parts(1) &= "</div>"
            End If

            parts(1) &= "</div>"
            parts(1) &= "<div style='width:500px; float:left;margin-top:12px;'>"

            parts(1) &= contract_list()
            parts(1) &= "</div>"
            parts(1) &= "</div>"
            parts(2) = ""
            parts(2) &= "<div id='tabs-3' style='display:none; min-height:500px;' >"

            If (fm.searcharray(fx, "1") Or fm.searcharray(fx, "2")) Then
                parts(2) &= "     <div id='lft' style='float:left;width:15%;'>"

                parts(2) &= " <input type='button' class='lft-1' onclick=" & Chr(34) & "javascript:showdis(1,'shw',this);" & Chr(34) & " style='width:100%; cursor:pointer;text-align:left;' value='Errors' />"
                parts(2) &= " <input type='button' class='lft-2' onclick=" & Chr(34) & "javascript:showdis(2,'shw',this);" & Chr(34) & " style='width:100%; cursor:pointer;text-align:left;' value= 'Contract Employess' />"
                parts(2) &= " <input type='button' class='lft-3' onclick=" & Chr(34) & "javascript:showdis(3,'shw',this);" & Chr(34) & " style='width:100%; cursor:pointer;text-align:left;' value= 'Internal Report' /> "
                parts(2) &= " <input type='button' class='lft-4' onclick=" & Chr(34) & "javascript:showdis(4,'shw',this);" & Chr(34) & " style='width:100%; cursor:pointer;text-align:left;' value= 'Trasferred' /> <br />"
                parts(2) &= " <input type='button' class='lft-4' onclick=" & Chr(34) & "javascript:showdis(5,'shw',this);" & Chr(34) & " style='width:100%; cursor:pointer;text-align:left;' value= 'Error date on transfer' /> <br />"
                parts(2) &= " <input type='button' class='lft-4' onclick=" & Chr(34) & "javascript:showdis(6,'shw',this);" & Chr(34) & " style='width:100%; cursor:pointer;text-align:left;' value= 'Error on salary closing' /> <br />"

                parts(2) &= " </div>"
                parts(2) &= " <div id='shoow' style='float:inherit; width:85%; min-height:500px; background:fafafa;'>"
                parts(2) &= "     <div class='showin'>"

                parts(2) &= "    <span class='shw' id='1' style='display:none; top:0px; left:0px; position:relative; min-height:500px; '>"
                parts(2) &= " <iframe style='width:100%;min-height:400px;' scrolling='veritcal' src='errorfind.aspx' frameborder='0'></iframe>"
                parts(2) &= "</span>"
                parts(2) &= "    <span class='shw' id='2' style='display:none; top:0px; left:0px; position:relative;'>"
                'parts(2) &= "<div style='width:500px; float:left;'>"
                rtx = contract_list(5)
                Dim passto As String = ""
                If rtx.Length >= 1 Then
                    For i As Integer = 0 To rtx.Length - 1
                        If String.IsNullOrEmpty(rtx(i)) = False Then
                            passto &= rtx(i)
                            'Response.Write(rtx(i))
                        End If

                    Next
                End If
                Dim readfile() As String
                If FileIO.FileSystem.FileExists("c:\temp\backup-log.csv") Then
                    readfile = File.ReadAllLines("c:\temp\backup-log.csv")
                    If readfile.Length > 0 Then
                        For Each k As String In readfile
                            'parts(2) &= "<i>" & k.ToCharArray & "</i><br>"

                        Next
                    End If

                End If
                Dim sec As New k_security
                If passto <> "" Then
                    passto = sec.dbStrToHex("<b>The following list of employee(s) will be resign after the remaining day(s) get over automatically by the system.</b><table style='width:700px;'><tr><td colspan='6' style='font-size:11pt;'><b>Contract Employee's</b></td></tr>" & _
                    "<tr style=' text-align:center; font-weight:bold;color:blue;'>" & _
                    "<td style='''>&nbsp;</td><td style='text-align:center;'>Employee's Name</td>" & _
                    "<td style=''>Employment date</td>" & _
                    "<td style=''>Contract<br>Start/Renew Date</td>" & _
                    "<td style=''>Contract End</td>" & _
                    "<td style=''>Remaining Days </td></tr>" & _
                    "<tr style='margin-top:-5px;'><td colspan='6' style='height:5px;text-align:top;'><hr  style='border-color:blue;' valign='top'></hr></td></tr>") & passto & sec.dbStrToHex("</table>")
                    parts(2) &= "<script>"
                    parts(2) &= " $('#hidpass').val('" & passto & "');"
                    parts(2) &= "</script>"
                    passno = 1
                End If
                ' parts(2) &= "  </div>"
                parts(2) &= contract_list()
                parts(2) &= "</span>" 'left tab2
                parts(2) &= "    <span class='shw' id='3' style='display:none; top:0px; left:0px; position:relative; min-height:500px; '>"
                If (fm.searcharray(fx, "1") Or fm.searcharray(fx, "2") Or fm.searcharray(fx, "3")) Then
                    parts(2) &= "<div id='rpthome-p-1' style=' width:300px; height:auto; float:left;'>.............."
                    parts(2) &= rpt_show()

                    parts(2) &= " </div> "


                End If
                parts(2) &= "</span>" 'left tab3
                parts(2) &= "    <span class='shw' id='4' style='display:none; top:0px; left:0px; position:relative; min-height:500px; '>"


                parts(2) &= rpttransfer()
                parts(2) &= "</span>"
                parts(2) &= "    <span class='shw' id='5' style='display:none; top:0px; left:0px; position:relative; min-height:500px; '>"


                parts(2) &= dateerror_job_ass()
                parts(2) &= "</span>"
                parts(2) &= "    <span class='shw' id='6' style='display:none; top:0px; left:0px; position:relative; min-height:500px; '>"


                parts(2) &= erroonSalary()
                parts(2) &= "</span>"
                parts(2) &= "    </div>" ' resignrecord()
                parts(2) &= " </div>"
                'new era is up


            End If




            parts(2) &= "  </div>"
            parts(3) = ""
            parts(3) &= "<div id='tabs-4' style='display:none; min-height:500px; clear:both;' >"

            If (fm.searcharray(fx, "1") Or fm.searcharray(fx, "3")) Then
                parts(3) &= "<div id='tabs2'>"
                parts(3) &= " <ul style='background:blue; border:none none;'>"
                parts(3) &= "<li id='x1'><a href='#tabs2-1'>Resign</a></li>"
                parts(3) &= "<li id='x2'><a href='#tabs2-2'>Transfer</a></li>"
                parts(3) &= "<li id='x3'><a href='#tabs2-3'>Increament In the middle</a></li>"
                parts(3) &= "<li id='x4'><a href='#tabs2-4'>Unpaid Active</a></li>"
                parts(3) &= "<li id='x5'><a href='#tabs2-5'>Unpaid Resign</a></li>"
                parts(3) &= "<li id='x6'><a href='#tabs2-6'>Per-dime error</a></li>"
                parts(3) &= "<li id='x7'><a href='#tabs2-7'>payroll error</a></li>"
                parts(3) &= " </ul>"
                parts(3) &= "<div id='tabs2-1'>" & resign_emp() & "</div>"
                parts(3) &= "<div id='tabs2-2'>" & rpttransfer() & "</div>"
                parts(3) &= "<div id='tabs2-3'>" & rptinc() & "</div>"
                parts(3) &= "<div id='tabs2-4'>..</div>"
                parts(3) &= "<div id='tabs2-5'>" & loadrep() & "</div>"
                parts(3) &= "<div id='tabs2-6'>" & perimerror() & "</div>"
                parts(3) &= "<div id='tabs2-7'>" & erroronpayroll() & "</div>"
                parts(3) &= "</div>"

                parts(3) &= "<style type='text/css'>"
                parts(3) &= ".c1"
                parts(3) &= "{"
                parts(3) &= "width:300px;"
                parts(3) &= "float:left;"
                parts(3) &= "background:#ffffbd;"
                parts(3) &= "height:500px;"
                parts(3) &= "overflow:auto;"

                parts(3) &= "}"
                parts(3) &= ".c2"
                parts(3) &= "{"
                parts(3) &= "width:550px;"
                parts(3) &= "float:left;"
                parts(3) &= "background:#ffcdff;"
                parts(3) &= "height:500px;"
                parts(3) &= "overflow:auto;"
                parts(3) &= "   	}"
                parts(3) &= ".c3"
                parts(3) &= "{"
                parts(3) &= "width:300px;"
                parts(3) &= "float:left;"
                parts(3) &= "background:#ffbbaa;"
                parts(3) &= "height:500px;"
                parts(3) &= "overflow:auto;"
                parts(3) &= "    	}"
                parts(3) &= "      .sp"
                parts(3) &= "{"
                parts(3) &= "width:10px;"
                parts(3) &= "float:left;"

                parts(3) &= "    		}"

                parts(3) &= "</style>"





                'parts(3) &= loadrep()
                ' Dim fl As New file_list
                'Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))


                '        parts(3) &= "<div class='c1'>"
                '       parts(3) &= "<div id='resignx' onclick=" & Chr(34) & "javascript:showHideSubMenu(this,'divresign');" & Chr(34) & " style='height:42px; width:300px;color:blue; font-size:14px;'>"
                '      parts(3) &= "<span style='width:80px; float:left; position:relative;'>Resign</span>"
                '     parts(3) &= "<span style='float:left;width:75px; position:relative; background: url(images/gif/collapsed.gif) no-repeat;'>&nbsp;</span></div>"
                '
                'parts(3) &= "<div id='divresign' style='display:none;'>"
                'parts(3) &= resign_emp()



                '         parts(3) &= "    </div>"
                '        parts(3) &= "<div id='div3' style='display:none;'>"
                '       parts(3) &= resignrecord()
                '

                'parts(3) &= "  </div>"
                'parts(3) &= "<div id='div4' style='display:none;'>"
                'parts(3) &= rptinc()


                '         parts(3) &= "  </div>"
                '        parts(3) &= "</div>"
                '       parts(3) &= "<div class='sp'>&nbsp;</div>"

                'parts(3) &= "<div class='c2'>"
                '    parts(3) &= "<div id='rpta' onclick=" & Chr(34) & "javascript:showHideSubMenu(this,'vewrpt');" & Chr(34) & " style='height:42px; width:auto; color:blue; font-size:16px;'>"
                '   parts(3) &= "<span style='width:80px; float:left; position:relative;'>Transfer</span>"
                '  parts(3) &= "<span style='float:left;width:75px; position:relative; background: url(images/gif/collapsed.gif) no-repeat;'>&nbsp;</span></div>"

                'parts(3) &= "<div id='vewrpt' style='display:none;'>"
                'parts(3) &= "<asp:Literal ID='otp2' runat='server'></asp:Literal>"
                'parts(3) &= rpttransfer()

                'parts(3) &= "</div>"

                'parts(3) &= "</div>"
                'parts(3) &= "<div class='sp'>&nbsp;</div>"

                'parts(3) &= "<div class='c3'>"
                'parts(3) &= "<div id='rpta' onclick=" & Chr(34) & "javascript:showHideSubMenu(this,'viewinc');" & Chr(34) & " style='height:42px; width:auto; color:blue; font-size:16px;'>"
                'parts(3) &= "<span style='width:60px; float:left; position:relative;'>Increaments</span>"
                'parts(3) &= "<span style='float:left;width:90px; position:relative; background: url(images/gif/collapsed.gif) no-repeat;'>&nbsp;</span></div>"

                '                parts(3) &= "<div id='viewinc' style='display:none;'>"
                '               'parts(3) &= "<asp:Literal ID='otp2' runat='server'></asp:Literal>"
                '              parts(3) &= rptinc()

                'parts(3) &= "</div>"



                'parts(3) &= "  </div>"


            End If






            parts(3) &= "</div>"
            parts(4) = ""
            parts(4) &= "</div>"
            parts(4) &= "<script type='text/javascript'>"
            parts(4) &= "$(document).ready(function () {"
            parts(4) &= "  $('#frmx').attr('target', 'fpay');"
            parts(4) &= "$('#frmx').attr('action', 'accouting.aspx');"
            parts(4) &= " $('#frmx').submit();"


            parts(4) &= "   $('#tab-2').css('display', 'inline');"
            parts(4) &= "   $('#tab-3').css('display', 'inline');"
            parts(4) &= "  $('#tab-4').css('display', 'inline');"
            parts(4) &= "  $('#tab').mouseenter();"

            parts(4) &= "});"
            parts(4) &= "</script>"



            rs.Close()
            db = Nothing
            fm = Nothing
        End If
        If page1 <> "" Then
            'Session("page1") = page1

        End If
        Me.pagecont.Text = page1 & parts(0) & parts(1) & parts(2) & parts(3) & parts(4) 'Session("page1")
    End Sub
    Function resignrecord()
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim rtn As String = ""
        rs = ds.dtmake("rsign", "select * from emprec where end_date is not null and id not in(select emptid from emp_resign)", Session("con"))
        If rs.HasRows Then
            While rs.Read
                rtn &= ("id==>" & rs.Item("id") & " Name=" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "<br>")
            End While
        End If
        rs.Close()
        ds = Nothing
        Return rtn
    End Function
    Function erroronpayroll()
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rtn As String = ""
        Dim wemail As String = ""
        Dim whor() As String = {""}
        Dim email As New mail_system
        rs = dbs.dtmake("errorx", "select * from payrollx where ref='' or ref is null", Session("con"))
        '    rtn = "<table><tr><td colspan='3'>Error on payroll making</td></tr><tr><td>payroll month</td><td>emp. Id</td><td>Payroll type</td></tr>"
        If rs.HasRows Then
            While rs.Read
                ' Response.Write("<br>" & rs.Item("emp_id"))
                If fm.searcharray(whor, rs.Item("who_reg")) = False Then
                    ReDim Preserve whor(UBound(whor) + 1)
                    whor(whor.Length - 1) = rs.Item("who_reg")
                    Session(rs.Item("who_reg")) = "<tr><td colspan='3'>Error on payroll making</td></tr><tr><td>payroll month</td><td>Employee's id</td><td>Payroll type</td></tr>"
                End If
                Session(rs.Item("who_reg")) &= "<tr><td>" & rs.Item("date_paid") & "</td><td>" & rs.Item("emptid") & "</td><td>" & rs.Item("remark") & "</td></tr>"
            End While

        End If
        For i As Integer = 0 To UBound(whor)
            If whor(i) <> "" Then
                rtn &= ("<table><tr><td>" & whor(i) & "</td></tr>" & Session(whor(i)) & "</table>")
                wemail = fm.getinfo2("select emp_id from login where username='" & whor(i) & "'", Session("con"))
                wemail = fm.getinfo2("select pemail from emp_address where emp_id='" & wemail & "' and wemail<>pemail", Session("con"))
                If wemail <> "None" Then
                    Session("eto") = wemail & ",z.kirubel@gmail.com"
                    email.sendemail(("<table><tr><td>" & whor(i) & "</td></tr>" & Session(whor(i)) & "</table>"), Session("epwd"), Session("efrom"), Session("eto"), "Eror on Payroll", Session("smtp"), Session("eport"))

                End If
            End If
        Next
        Return rtn
    End Function
    Function erroonSalary()
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim rtn As String = ""
        Dim info As String = ""
        Dim fm As New formMaker
        Dim mail As New mail_system
        Dim sql As String
        ' Dim emptid As string
        rs = dbs.dtmake("vwsalactive", "select * from emprec where active='y'", Session("con"))
        If rs.HasRows Then
            While rs.Read
                info = fm.getinfo2("select count(id) from emp_sal_info where emptid=" & rs.Item("id") & " and active='y'", Session("con"))
                If CInt(info) > 1 Then
                    rtn &= "<tr><td>" & fm.getfullname(rs.Item("emp_id"), Session("con")) & "</td>"
                    rtn &= "<td>" & info & " not deactive</td>"
                    info = fm.getinfo2("select id from emp_sal_info where emptid=" & rs.Item("id") & " order by id desc", Session("con"))
                    If IsNumeric(info) Then
                        sql &= "update emp_sal_info set active='n' where date_end is not null and id<" & info & Chr(13)
                        rtn &= "<td>" & info & "</td>"
                    End If
                    rtn &= "</tr>"
                End If

            End While

            If sql <> "" Then
                If dbs.excutes(sql, Session("con"), Session("path")) > 0 Then
                    mail.sendemail("<table>" & rtn & "</table><br>" & sql & "<br> Datas are updated! ", Session("passk"), Session("fromk"), Session("eto"), "Eror on salary date updated", Session("smtp"), Session("eport"))
                    Return "<table>" & rtn & sql & "</table>"
                End If
            End If

        End If
        info = fm.getinfo2("select id from emp_pen_start where who_reg='system'", Session("con"))
        If IsNumeric(info) Then
            sql = "delete from emp_pen_start where who_reg='system'"
            dbs.excutes(sql, Session("con"), Session("path"))
            mail.sendemail("<table>" & sql & "<br> Datas are deleted! ", Session("passk"), Session("fromk"), Session("eto"), "Eror on pension")

        End If
        Return "NO Data!!!!"
    End Function
    Function resign_emp()
        Dim d1, d2 As Date
        d2 = Today
        d1 = d2.AddDays(-60)
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim rs As DataTableReader
        Dim rtn As String = ""
        Dim empid As String
        rs = ds.dtmake("rsign", "select * from emp_resign where resign_date between '" & d1 & "' and '" & d2 & "' order by resign_date desc", Session("con"))
        rtn = "<table border=1><tr><td colspan=4><b>Resign Employees in Last 2 Months</td></tr>"
        If rs.HasRows Then
            While rs.Read
                If rs.IsDBNull(1) = True Then
                    empid = fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con"))
                Else
                    empid = rs.Item("emp_id")
                End If
                rtn &= "<tr><td>" & rs.Item("emptid") & "</td><td>" & fm.getfullname(empid, Session("con")) & "</td><td>" & rs.Item("resign_date") & "</td></tr>"
            End While
        End If
        rtn &= "</table>"
        rs.Close()
        ds = Nothing
        Return rtn
    End Function
    Function dateerror_job_ass()
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rtn As String = ""
        Dim idc As Integer = 0
        rs = dbs.dtmake("errorx", "select emptid,emp_id,count(emptid) as cc from emp_job_assign where date_end is null group by emptid,emp_id", Session("con"))
        rtn = "<form id='frmemplist' name='frmemplist' action='' method='post'><input type='hidden' id='datatake' name='datatake'><table><tr><td colspan='3'>Error date closing on project transfer</td></tr><tr><td>Emp.id</td><td>Com. Id</td><td>Employee's Name</td><td>&nbsp;</td></tr>"
        If rs.HasRows Then
            While rs.Read
                ' Response.Write("<br>" & rs.Item("emp_id"))
                If CInt(rs.Item(2)) > 1 Then
                    rtn &= "<tr><td>" & rs.Item(1) & "</td><td>" & rs.Item(0) & "</td><td>" & fm.getfullname(rs.Item(1), Session("con")) & "</td><td><span onclick=" & Chr(34) & "javascript:orderpass('" & rs.Item(1) & "');" & Chr(34) & ">goto>></span></td></tr>"
                    idc += 1
                End If
            End While

        End If
        If idc <> 0 Then
            rtn &= "</table></form>"
        Else
            rtn &= "<tr><td colspan='4'>No Error</td></tr></table></form>"
        End If
        Return rtn
    End Function
    Function getwelcom()
        Dim wnote As String

        If File.Exists("C:\temp\welcom\welcome.htm") Then
            wnote = File.ReadAllText("c:\temp\welcom\welcome.htm")
        ElseIf File.Exists("C:\temp\welcom\welcome.html") Then
            wnote = File.ReadAllText("c:\temp\welcom\welcome.html")

        Else
            wnote = ""
        End If
        Return wnote
    End Function
    Function perimerror()
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim outp As String = ""
        Dim fldc, i As Integer
        rs = dbs.dtmake("vwperimerror", "select *,datediff(dd,from_date,to_date)+1 as Diff from pardimpay where datediff(dd,from_date,to_date)+1<>no_days order by id", Session("con"))
        If rs.HasRows Then
            outp &= "<table>"
            fldc = rs.FieldCount
            While rs.Read
                outp &= "<tr>"
                For i = 0 To fldc - 1
                    outp &= "<td>" & rs.Item(i) & "</td>"
                Next

                outp &= "<tr>"

            End While
            outp &= "</table>"
        End If
        Return outp
    End Function
    Function Kir_StrToHex(ByVal Data As String) As String
        Dim sVal As String
        Dim sHex As String = ""
        If String.IsNullOrEmpty(Data) = False Then
            While Data.Length > 0
                sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
                Data = Data.Substring(1, Data.Length - 1)
                sHex = sHex & sVal
            End While
        End If
        sVal = sHex
        Dim tmpval As String
        If sVal.Length > 5 Then
            For i As Integer = 3 To sVal.Length - 1
                tmpval &= sVal(i)

            Next
        End If
        For i As Integer = 3 To 0 Step -1
            tmpval &= sVal(i)
        Next
        Return tmpval
    End Function
    Function Kir_HexToStr(ByVal data As String) As String
        Dim sval As String
        Dim sstr As String = ""
        Dim dd As String
        Dim tmp As String = ""
        If data.Length > 5 Then
            For k As Integer = data.Length - 1 To data.Length - 3 Step -1
                tmp &= data(k)
            Next
            For k As Integer = 0 To data.Length - 4
                tmp &= data(k)

            Next
        End If
        data = tmp

        If String.IsNullOrEmpty(data) = False Then
            ' dd = data.Split("⌡")

            For i As Integer = 0 To data.Length - 2 Step 2
                dd = data.Substring(i, 2)
                sval = Chr(Convert.ToInt32(dd, 16))

                sstr = sstr & sval
            Next
        End If
        Return sstr
    End Function
End Class
