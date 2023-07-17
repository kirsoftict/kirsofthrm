Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Partial Class scripts_leavesummery
    Inherits System.Web.UI.Page
    Function refresh()
        Response.Redirect("leavesummery.aspx")
        Return False
    End Function
    Public Function calc3(ByVal conx As SqlConnection, ByVal empid As String, ByVal emptid As Integer, ByVal pathx As String) As Object
        ' Response.Write("called")
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim ks As New kirsoftsystem
        Dim save_err As String = ""
        Dim dt As DataTableReader
        Dim dt2 As DataTableReader
        Dim mxdate As Double
        ' Dim dt3 As DataTableReader
        Dim strout As String = ""
        Dim stravl As String = ""
        Dim strexp As String = ""
        Dim intavl As Integer = 0
        Dim intexp As Integer = 0
        Dim ginf As String
        Dim yrstep, dateinc As Double
        yrstep = 0
        dateinc = 0
        If File.Exists(HttpContext.Current.Session("path") & "kst/maxdate.ks") Then
            Dim rdfl() As String = File.ReadAllLines(HttpContext.Current.Session("path") & "kst/maxdate.ks")
            mxdate = rdfl(0)
        Else
            mxdate = 30
        End If
        ' Dim conx As SqlConnection = con
        ' Dim empid As String = Session("emp_id")
        Dim i As Integer = 0
        Dim dyr As Double
        Dim sql As String = ""
        Dim sql2() As String
        Dim spl() As String
        ' Response.Write("select hire_date from emprec where emp_id='" & empid & "' and id=" & emptid.ToString & " and end_date is Null order by id desc")
        dt = dbx.dtmake("newdatax" & Today.ToLongDateString, "select hire_date from emprec where emp_id='" & empid & "' and id=" & emptid.ToString & " and end_date is Null order by id desc", conx)
        dt2 = dbx.dtmake("new2" & Today.ToLongDateString, "select no_days,year_end,user_rec_date,emptid from emp_leave_info where emp_id='" & empid & "' and emptid=" & emptid & " order by id DESC", conx)
        If Session("emp_iid").ToString <> "" Then
            sql = "Begin Trans " & Session("emp_iid") & Chr(13)
        Else
            Response.Write("<script>window.locations='logout.aspx?msg=session expired';</script>")
        End If
        Dim data_c_c As Integer = 0
        If dt.HasRows = True Then
            dt.Read()
            ' Response.Write("Step in")
            Dim d_hire As Date
            Dim d2 As String
            d_hire = dt.Item("hire_date")
            'Response.Write("tblleavinc where start_from<='" & Today.ToShortDateString & "' order by id, incmon,mindate,steps,start_from")
            stravl = fm.getinfo2("select start_from from tblleavinc where start_from<='" & Today.ToShortDateString & "' order by id desc", Session("con"))
            ' Response.Write("<br> start From: " & stravl)

            ginf = fm.getrowinfo("tblleavinc where start_from='" & CDate(stravl).ToShortDateString & "' order by id", "incmon,mindate,steps,start_from", conx, ",")
            stravl = ""
            spl = ginf.Split(",")
            For jk As Integer = 0 To spl.Length - 1
                '  Response.Write("<br>" & spl(jk) & "<br>")
            Next
            If dt2.HasRows = True Then
                dt2.Read()
                If dt2.Item("user_rec_date") = "n" Then
                    d2 = dt2.Item("year_end")
                    Dim x1 As Integer = d_hire.Year
                    Dim n_days As Integer = dt2.Item("no_days")
                    dyr = ((Today.Subtract(d_hire).Days / 30.4375) / 12) ' - x1
                    ' Response.Write(dyr & "<br>")
                    dyr = getxyear(spl(2), emptid)
                    If dyr = 0 Then
                        dyr = 0
                    ElseIf dyr = 1 Then
                        dyr = CDbl(spl(0))
                    End If
                    If dyr < 1 Then
                        '     dyr = 1
                    Else
                        '  dyr = Math.Ceiling(dyr)
                    End If
                    ' dyr = Math.Round(dyr)
                    Dim dbgt, davail As Integer
                    Dim rate As Double
                    Dim m1 As Double
                    Dim m2 As Double
                    Dim y_end As Date = ks.StringToDate(d2 & ", " & d_hire.Year.ToString, "MMMM dd, yyyy")
                    m1 = (y_end.Subtract(d_hire).Days / 30)
                    ' Response.Write("M1=>" & m1 & "<br>")

                    If m1 < 0 Then
                        m1 = Math.Floor(m1)
                        m2 = 12 + m1
                        m1 = m1 * -1
                        y_end = y_end.AddYears(1)
                    Else
                        m1 = Math.Round(m1)
                        m2 = 12 - m1
                    End If

                    Dim hd, ye As Date
                    hd = d_hire
                    ye = y_end
                    ' Response.Write("<br>" & d_hire.ToString & "===>" & y_end.ToString)
                    'Response.Write("<br>m1=" & m1 & " and m2=" & m2 & "<br>")
                    i = 0
                    Dim nnd As Integer


                    Do
                        If i = 0 Then
                            nnd = n_days + i
                        Else
                            If d_hire.Month <= 6 Then
                                nnd = n_days + i - 1
                            Else
                                nnd = n_days + i
                            End If
                        End If
                        rate = (nnd) / 12
                        If ye.Subtract(CDate(spl(3)).AddYears(1)).Days <= 0 Then
                            dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30.4375) * rate).ToString)
                            ' Response.Write("<br>yearx:" & (ye.Subtract(CDate(spl(3))).Days).ToString & "<br>")
                        Else
                            ' Response.Write("<br>yearx:" & (ye.Subtract(CDate(spl(3))).Days).ToString & "<br>")

                            dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30.4375) * rate).ToString)
                            If dyr = 0 Then
                                '   Response.Write(dbgt & "-1=>" & (dbgt - 1).ToString & "<br>")
                                dbgt = dbgt - 1
                            ElseIf dyr = spl(0) Then
                                '   Response.Write(dbgt & "+" & spl(0) & "-1=>" & (dbgt - 1).ToString & "<br>")
                                dbgt = dbgt + CInt(spl(0)) - 1
                            End If
                        End If

                        '  Response.Write("nnd=>" & nnd & " rate=>" & rate & "<br>BGT: " & dbgt.ToString & "<br>")
                        'strout &= dbgt
                        If hd.Subtract(Today).Days > 0 Then
                            davail = (Math.Round(((Today.Subtract(hd.ToShortDateString).Days) / 30) * rate).ToString)
                            '  Response.Write("<br>in davail:" & davail & "<br>")
                        Else
                            davail = dbgt
                        End If

                        If davail < 0 Then
                            davail = 0
                        End If
                        'Response.Write(rate.ToString & "===" & dbgt & "----" & davail)
                        If dbgt <> 0 Then

                        End If
                        Dim dt4 As DataTableReader
                        If dbgt <> 0 And davail <> 0 Then

                            dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emptid='" & emptid.ToString & "' and l_e_year='" & ye.ToShortDateString & "' order by id desc", conx)
                            If dt4.HasRows = True Then
                                strout &= ("<tr><td>" & ye & " has row</td></tr><br>")
                            Else
                                If dbgt > mxdate Then
                                    dbgt = mxdate
                                End If
                                ReDim Preserve sql2(i + 1)
                                data_c_c += 1
                                sql2(i) = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period,emptid) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye, "MM/DD/YYYY").ToString & "'," & dbgt.ToString & "," & emptid.ToString & ")"
                                ' strout &= ("<tr><td colspan='6'>" & sql2(i) & "</td></tr>")
                                '  Response.Write(i & ") " & sql2(i) & "<br>")
                                '        save_err = dbx.excutes(sql, conx, pathx).ToString
                                
                            End If

                            If fm.isexp(Today.ToShortDateString, ye, 2, "y") Then

                                intexp += davail 'send expire list
                            Else

                                intavl += davail 'send avail list
                            End If

                        End If

                        hd = ye.AddDays(1)
                        ye = y_end.AddYears(i + 1)

                        i = i + 1

                        ' Response.Write("<br>hd:" & Today.Subtract(hd).Days / 30 & "<br>")
                    Loop Until Today.Subtract(hd).Days / 30 < 0
                    ' Response.Write(strout)

                   
                    ' strout = strout.Length.ToString
                Else
                    d2 = dt2.Item("year_end")
                    Dim x1 As Integer = d_hire.Year
                    Dim n_days As Integer = dt2.Item("no_days")
                    dyr = getxyear(spl(2), emptid)
                    If dyr = 0 Then
                        dyr = 0
                    ElseIf dyr = 1 Then
                        dyr = CDbl(spl(0))
                    End If
                    If dyr < 1 Then
                        '     dyr = 1
                    Else
                        '  dyr = Math.Ceiling(dyr)
                    End If
                    '  dyr = ((Today.Subtract(d_hire).Days / 30.4375) / 12) ' - x1
                    'Response.Write("dyr: " & dyr & "<br>" & d2.ToString)

                    ' dyr = Math.Round(dyr)
                    Dim dbgt, davail As Integer
                    Dim rate As Double
                    Dim m1 As Double
                    Dim m2 As Double
                    Dim y_end As Date = ks.StringToDate(d2 & ", " & d_hire.Year.ToString, "MMMM dd, yyyy")
                    m1 = (y_end.Subtract(d_hire).Days / 30.4375)

                    '     Response.Write("<br>M1:" & m1)
                    If m1 < 0 Then
                        m1 = Math.Floor(m1)
                        m2 = 12 + m1
                        m1 = m1 * -1
                        y_end = y_end.AddYears(1)
                    Else
                        m1 = Math.Round(m1)
                        m2 = 12 - m1
                    End If
                    Dim hd, ye As Date
                    hd = d_hire
                    ye = y_end.AddYears(1).AddDays(-1)
                    'Response.Write("<br>" & d_hire.ToString & "===>" & y_end.ToString)
                    ' Response.Write("<br>m1=" & m1 & " and m2=" & m2 & "<br>")
                    i = 0
                    Dim nnd As Integer = 0


                    Do

                        nnd = n_days + i
                        'Response.Write("<br>i=>" & i & " nnd: " & nnd)
                        dbgt = 0

                        If hd.Subtract(CDate(spl(3)).AddYears(1)).Days <= 0 Then
                            dbgt = nnd
                            '  Response.Write("<br>yearx:" & (ye.Subtract(CDate(spl(3))).Days).ToString & "<br>BBBBBBB=>" & dbgt)
                        Else
                            '   Response.Write("<br>yearx:" & (ye.Subtract(CDate(spl(3))).Days).ToString & "<br>")

                            'dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30.4375) * rate).ToString)
                            If dyr = 0 Then
                                '   Response.Write(dbgt & "-1=>" & (dbgt - 1).ToString & "<br>")
                                dbgt = nnd - 1
                            ElseIf dyr = spl(0) Then
                                '   Response.Write(dbgt & "+" & spl(0) & "-1=>" & (dbgt - 1).ToString & "<br>")
                                dbgt = nnd + CInt(spl(0)) - 1
                            End If
                        End If
                        ' dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)

                        If dbgt > mxdate Then
                            dbgt = mxdate
                        End If
                        strout &= "<br>Budget:" & dbgt.ToString
                        strout &= "<br>year end:" & ye.ToShortDateString
                        strout &= "<br>hire date:" & d_hire.ToShortDateString
                        'strout &= dbgt
                        If ye.Subtract(Today).Days > 0 Then
                            davail = (Math.Round(((Today.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)
                        Else
                            davail = dbgt
                        End If
                        If davail < 0 Then
                            davail = 0
                        End If
                        strout &= "<br>Date avail:" & davail
                        '  Response.Write(strout & "<br>" & rate.ToString & "===" & dbgt & "----" & davail)

                        Dim dt4 As DataTableReader
                        If dbgt <> 0 Then

                            dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emptid='" & emptid.ToString & "' and l_s_year='" & hd.ToShortDateString & "' order by id desc", conx)

                            If dt4.HasRows = True Then
                                'strout &= ("<tr><td>has rowwww</td></tr>")
                            Else
                                ReDim Preserve sql2(i + 1)
                                data_c_c += 1
                                sql2(i) = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period,emptid) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye, "MM/DD/YYYY").ToString & "'," & dbgt.ToString & "," & emptid.ToString & ")"
                                ' strout &= ("<tr><td colspan='6'>" & sql2(i) & "</td></tr>")
                                '  Response.Write(i & ") " & sql2(i) & "<br>")
                            End If

                            If fm.isexp(Today.ToShortDateString, ye, 2, "y") Then

                                intexp += davail 'send expire list
                            Else

                                intavl += davail 'send avail list
                            End If

                        End If
                        i = i + 1
                        hd = ye.AddDays(1)
                        ye = y_end.AddYears(i + 1)
                        ye = ye.AddDays(-1)



                    Loop Until Today.Subtract(hd).Days / 30 < 0

                    

                End If
                '  Response.Write("<br>dcc:++++ " & data_c_c & "<<<<<<<<<<<<<<br>")
                If data_c_c > 0 Then
                    If sql2.Length > 0 Then
                        Dim exp As Object
                        Try


                            dbx.excutes("Begin Trans " & Session("emp_iid"), Session("con"), Session("path"))
                            For ksp As Integer = 0 To sql2.Length - 1
                                '  Response.Write("sql2=>" & sql2(ksp) & "<br>")
                                save_err = dbx.excutes(sql2(ksp), Session("con"), Session("path"))
                                If IsNumeric(save_err) = False Then
                                    strout &= "error: " & save_err

                                End If
                            Next
                            dbx.excutes("commit trans " & Session("emp_iid"), Session("con"), Session("path"))
                        Catch ex As Exception

                            dbx.excutes("rollback trans " & Session("emp_iid"), Session("con"), Session("path"))
                        End Try
                    End If
                    strout &= data_c_c.ToString & " new Data has been added"
                Else
                    strout &= "No Change in the database"
                End If

                If data_c_c > 0 Then
                    strout &= data_c_c.ToString & " new Data has been added"
                Else
                    strout &= "No Change in the database"
                End If
            Else
                strout = ("empleavesetup.aspx")
            End If

        Else
            strout = ("empemp1.aspx")
        End If
        dt.Close()
        dt2.Close()
        dbx = Nothing
        dt = Nothing
        dt2 = Nothing
        Return strout
    End Function
    Function getxyear(ByVal xyr As Integer, ByVal emptid As Integer) As Integer
        Dim dt As New dbclass
        Dim rs As DataTableReader
        Dim count As Integer = 0
        Dim nodate As Integer = 0
        Dim sql As String = "select * from (select *,row_number() over (order by l_e_year desc) as row from emp_leave_budget where emptid=" & emptid & ") tbl1 where tbl1.row>=0 and tbl1.row<=" & xyr
        '"select * from emp_leave_budget where emptid=" & emptid & " order by l_e_year desc top " & xyr
        ' Response.Write("<br>.....sql............<br>" & sql & "<br>.................<br>")
        Try

            rs = dt.dtmake("leavebgt", sql, Session("con"))
            If rs.HasRows Then
                While rs.Read
                    If rs.Item("no_days_with_period") = nodate Then

                        count += 1
                    Else
                        nodate = rs.Item("no_days_with_period")
                    End If
                End While
            End If
            If count = xyr Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql & "<br>")
        End Try



    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("con").state = Data.ConnectionState.Closed Then
            ' Response.Write("database is closed")
            Session("con").open()
        End If
    End Sub
End Class
