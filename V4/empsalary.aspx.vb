Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class empsalary
    Inherits System.Web.UI.Page
    Function thisinc()
        Dim dd As Date
        Dim dbx As New dbclass
        Dim oldsal, newsal, msal, diffsal, rate1, rate2 As Double
        Dim ds, de, dp As Date
        Dim nod, d1, d2 As Integer
        Dim rs As DataTableReader
        dd = Request.QueryString("date_start")
        Dim outp As String = ""
        Dim sql As String = ""
        Dim fm As New formMaker
        If dd.Month.ToString <> Today.Month.ToString Then
            If Today.Subtract(dd).Days > 30 Then
               
                Dim dc As Date
                dc = Today
                Dim rpt2 As String = ""
                rs = dbx.dtmake("payrolx", "select * from payrollx where date_paid between '" & dd.ToString & "' and '" & Now.ToShortDateString & "' and remark='monthly' and  emptid=" & Session("emptid") & " order by id", Session("con"))
                If rs.HasRows Then
                    While rs.Read
                        dp = rs.Item("date_paid")
                        If dp.Subtract(dd).Days > 0 And dp.Month = dd.Month And dp.Year = dd.Year Then
                            nod = Date.DaysInMonth(dp.Year, dp.Month)
                            d1 = dp.Subtract(dd).Days + 1
                            d2 = nod - d1
                            oldsal = CDbl(rs.Item("b_sal"))
                            newsal = Request.QueryString("basic_salary")
                            msal = (newsal / nod) * d1 + (oldsal / nod) * d2
                            outp &= ("------" & MonthName(dp.Month) & " &nbsp;" & dp.Year.ToString & "--------" & "<br>")
                            outp &= ("Empid:" & Session("emptid") & "<br>")
                            outp &= "Name:" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & Session("emptid"), Session("con")), Session("con")) & "<br>"
                            outp &= ("Salary would be:" & FormatNumber(msal.ToString, 2) & "<br>")
                            outp &= ("Salary Paid:" & FormatNumber(oldsal.ToString, 2, TriState.True, TriState.True, TriState.True) & "<br>")
                            outp &= ("Salary due/excess:" & FormatNumber((msal - oldsal).ToString, 2, TriState.True, TriState.True, TriState.True) & "<br>")
                            outp &= ("---------------" & "<br>")
                        Else
                            oldsal = CDbl(rs.Item("b_sal"))
                            newsal = Request.QueryString("basic_salary")
                            msal = newsal
                            outp &= ("------" & MonthName(dp.Month) & " &nbsp;" & dp.Year.ToString & "----" & "<br>")
                            outp &= ("Empid:" & Session("emptid") & "<br>")
                            outp &= "Name:" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & Session("emptid"), Session("con")), Session("con")) & "<br>"
                            outp &= ("Salary would be:" & FormatNumber(msal.ToString, 2) & "<br>")
                            outp &= ("Salary Paid:" & FormatNumber(oldsal.ToString, 2, TriState.True, TriState.True, TriState.True) & "<br>")
                            outp &= ("Salary due/excess:" & FormatNumber((msal - oldsal).ToString, 2, TriState.True, TriState.True, TriState.True) & "<br>")
                            outp &= ("--------------" & "<br>")
                        End If
                    End While
                Else
                    outp &= ("Still there is back pays, but the Computerise Payroll system is not establish at salary starts from." & "<br>")
                    outp &= ("Empid:" & Session("emptid") & "<br>")
                    outp &= "Name:" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & Session("emptid"), Session("con")), Session("con")) & "<br>"
                    outp &= ("Salary would be:" & FormatNumber(msal.ToString, 2) & "<br>")

                End If
               

            End If
            If outp <> "" Then
                sql = "insert into rptdataupdate(reporttype,Report,datee,seen) values('Payroll','" & outp & "','" & dd.ToShortDateString & "',0)"
                outp = dbx.save(sql, session("con"), session("path"))
                If outp <> "1" Then
                    
                    Response.Write(outp.ToString)
                End If

            End If
        End If
    End Function
End Class
