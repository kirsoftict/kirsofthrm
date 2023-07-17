Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class accouting
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim fm As New formMaker
        Try


            Dim dbs As New dbclass

            Dim rs As DataTableReader
            Dim res As String = ""
            Dim emptid, emp_id As String
            'Response.Write("whhho")
            rs = dbs.dtmake("vwres", "select * from emp_resign order by resign_date desc", Session("con"))
            If rs.HasRows = True Then
                Me.outp.Text &= "<table id='tb1' style='width:800px;'><tr style='font-weight:bold;'><td>Employee Name</td><td>Resign Date</td><td>Payroll paid</td><td>OT Unpaid</td><td>Leave Paid</td><td>Leave Balance</td><td>Leave Pay Amt</td><td>Closed</td></tr>"
                While rs.Read
                    res = ""
                    emptid = rs.Item("emptid")
                    Me.outp.Text &= "<tr><td>"
                    If IsNumeric(emptid) = True Then
                        'Response.Write(emptid & "<br>")
                        emp_id = fm.getinfo2("select emp_id from emprec where id=" & rs.Item("emptid"), Session("con"))
                    Else
                        ' Response.Write(emptid)
                        emp_id = "00/00"
                        emptid = "0"
                    End If
                    If emp_id <> "None" Then
                        Me.outp.Text &= fm.getfullname(emp_id, Session("con"))
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td>"
                        Me.outp.Text &= Format(rs.Item("resign_date"), "MMM dd, yyyy")
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td>"
                        res = fm.getinfo2("select id from payrollx where emptid=" & rs.Item("emptid") & " and (month(date_paid)='" & Month(rs.Item("resign_date")) & "' and year(date_paid)='" & Year(rs.Item("resign_date")) & "') and remark='monthly'", Session("con"))
                        If res <> "None" And res.Length < 6 Then
                            Me.outp.Text &= "Yes"
                        Else
                            Me.outp.Text &= "&nbsp;"
                        End If
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td style='text-align:right;'>"
                        res = ""
                        res = fm.getinfo2("select sum(amt) from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'", Session("con"))
                        ' Response.Write(res)
                        If IsNumeric(res) Then
                            If CInt(res.ToString) > 0 Then
                                Me.outp.Text &= FormatNumber(res, 2, TriState.True, TriState.True, TriState.True)
                            Else
                                Me.outp.Text &= "0.00"
                            End If
                        Else
                            Me.outp.Text &= "0.00"
                        End If
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td style='text-align:right;'>"
                        res = ""
                        res = fm.getinfo2("select sum(amt) as exp2 from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'", Session("con"))
                        ' Response.Write(res.ToString & "<br>...." & "select sum(amt) from emp_ot where emptid=" & rs.Item("emptid") & " and paidstatus='n'")

                        If res <> "None" And res.Length < 6 And res <> "" Then

                            If CDbl(res) > 0 Then
                                Me.outp.Text &= FormatNumber(res, 2, TriState.True, TriState.True, TriState.True)
                            End If
                        Else
                            res = 0
                            Me.outp.Text &= "0.00"
                        End If
                        Dim lvba As String
                        lvba = fm.lvb(emptid, False, 0, rs.Item("resign_date"), Session("con")).ToString()
                        Me.outp.Text &= "</td><td style='text-align:right;'>"
                        Me.outp.Text &= FormatNumber(lvba.ToString(), 2)
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

                        Me.outp.Text &= "</td><td>" & FormatNumber(amttop.ToString(), 2).ToString & "</td><td>"
                        If amttop > 0 Then
                            Me.outp.Text &= "<span onclick=" & Chr(34) & "javascript://balancep('" & emptid & "');" & Chr(34) & " style='color:blue; cursor:pointer;'>Open Leave Payment<span></td></tr>"
                        Else
                            Me.outp.Text &= "<span onclick=" & Chr(34) & "javascript:closedx('" & rs.Item("id").ToString & "');" & Chr(34) & " style='color:blue; cursor:pointer;'>Close<span></td></tr>"

                        End If
                    End If 'End of emp_id=None
                    ' Me.outp.Text &= rs.Item("emptid").ToString & "<br>"
                End While
                Me.outp.Text &= "</table>"
            End If
            Me.otp2.Text = rpt_show()


            dbs = Nothing
            fm = Nothing
        Catch ex As Exception
            Response.Write(ex.ToString)
            fm.exception_hand(ex, "Error on accouting.asp: ")
        End Try
    End Sub
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
        rs = ds.dtmake("inccc", sql, Session("con"))

        If rs.HasRows Then
            Me.outp3.Text = "<table><tr><td>Com. No</td><td>Emp. Id</td><td>Employee's Name</td><td>Date Increament</td></tr>"
            While rs.Read
                Me.outp3.Text &= ("<tr><td>" & rs.Item("emptid") & "</td><td>" & rs.Item("emp_id") & "</td><td>" & rs.Item("first_name") & " " & rs.Item("middle_name") & "</td><td>" & rs.Item("date_start") & "</td></tr>")
            End While
            Me.outp3.Text &= "</table>"
        End If
    End Function
End Class
