Imports Kirsoft.hrm
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Class Default4
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Dim client As New WebClient()
        '' client.DownloadFile("https://kirsoft-pc:4433/reportnonactive.aspx?val=all", "c:\temp\html\reportnon.html")
        Response.Write(Session("path"))
        Dim sec As New k_security
        ' Dim secval As String = sec.Kir_StrToHex("493 15kir")
        'Response.Write("<br> pwd" & secval & "<br>")
        '  Response.Write("<br>" & sec.Kir_HexToStr(secval))
        'secval = sec.Kir_StrToHex("kirsoftet@gmail.com")
        'Response.Write("<br>email:" & secval & "<br>")
        'Response.Write("<br>" & sec.Kir_HexToStr(secval))
        'secval = sec.Kir_StrToHex("smtp.gmail.com")
        'Response.Write("<br>SMTP:" & secval & "<br>")
        'Response.Write("<br>" & sec.Kir_HexToStr(secval))
        'secval = sec.Kir_StrToHex("587")
        'Response.Write("<br>Port:" & secval & "<br>")
        'Response.Write("<br>" & sec.Kir_HexToStr(secval))
        Dim kiemail As New mail_system
        Response.Write(kiemail.sendemail("test", Session("epwd"), Session("efrom"), "z.kirubel@gmail.com", "test2").ToString)
    End Sub
    Public Function calc3(ByVal conx As SqlConnection, ByVal empid As String, ByVal emptid As Integer, ByVal pathx As String) As Object
        'Response.Write("called")
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
        Dim spl() As String
        dt = dbx.dtmake("newdatax" & Today.ToLongDateString, "select hire_date from emprec where emp_id='" & empid & "' and id=" & emptid.ToString & " and end_date is Null order by id desc", conx)
        dt2 = dbx.dtmake("new2" & Today.ToLongDateString, "select no_days,year_end,user_rec_date,emptid from emp_leave_info where emp_id='" & empid & "' and emptid=" & emptid & " order by id DESC", conx)

        If dt.HasRows = True Then
            dt.Read()
            Dim d_hire As Date
            Dim d2 As String
            d_hire = dt.Item("hire_date")
            ginf = fm.getjavalist2("tblleavinc where start_from>'" & d_hire.ToShortDateString & "' order by id desc", "incmon,mindate,steps,start_from", conx, ",")
            spl = ginf.Split(",")
            For jk As Integer = 0 To spl.Length - 1
                Response.Write("<br>" & spl(jk) & "<br>")
            Next
            If dt2.HasRows = True Then
                dt2.Read()
                If dt2.Item("user_rec_date") = "n" Then
                    d2 = dt2.Item("year_end")
                    Dim x1 As Integer = d_hire.Year
                    Dim n_days As Integer = dt2.Item("no_days")
                    dyr = ((Today.Subtract(d_hire).Days / 30.41) / 12) ' - x1
                    Response.Write(dyr & "<br>")
                    If dyr < 1 Then
                        dyr = 1
                    Else
                        dyr = Math.Ceiling(dyr)
                    End If
                    ' dyr = Math.Round(dyr)
                    Dim dbgt, davail As Integer
                    Dim rate As Double
                    Dim m1 As Double
                    Dim m2 As Double
                    Dim y_end As Date = ks.StringToDate(d2 & ", " & d_hire.Year.ToString, "MMMM dd, yyyy")
                    m1 = (y_end.Subtract(d_hire).Days / 30)
                    Response.Write("M1=>" & m1 & "<br>")
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
                    'Response.Write("<br>" & d_hire.ToString & "===>" & y_end.ToString)
                    ' Response.Write("<br>m1=" & m1 & " and m2=" & m2 & "<br>")
                    i = 0
                    Dim nnd As Integer
                    Dim data_c_c As Integer = 0

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

                        dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)

                        'strout &= dbgt
                        If ye.Subtract(Today).Days > 0 Then
                            davail = (Math.Round(((Today.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)

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

                            dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emptid='" & emptid.ToString & "' and l_s_year='" & hd.ToShortDateString & "' order by id desc", conx)
                            If dt4.HasRows = True Then
                                strout &= ("<tr><td>has row</td></tr>")
                            Else
                                If dbgt > mxdate Then
                                    dbgt = mxdate
                                End If
                                sql = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period,emptid) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye, "MM/DD/YYYY").ToString & "'," & dbgt.ToString & "," & emptid.ToString & ")"
                                strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                ' save_err = dbx.save(sql, conx, pathx).ToString
                                If save_err <> "1" Then
                                    strout &= "Save error"
                                Else
                                    data_c_c += 1
                                End If
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

                    Loop Until Today.Subtract(hd).Days / 30.4375 < 0


                    If data_c_c > 0 Then
                        strout &= data_c_c.ToString & " new Data has been added"
                    Else
                        strout &= "No Change in the database"
                    End If
                    ' strout = strout.Length.ToString
                Else
                    d2 = dt2.Item("year_end")
                    Dim x1 As Integer = d_hire.Year
                    Dim n_days As Integer = dt2.Item("no_days")
                    dyr = ((Today.Subtract(d_hire).Days / 30.4375) / 12) ' - x1
                    Response.Write("dyr: " & dyr & "<br>")
                    If dyr < 1 Then
                        dyr = 1
                    Else
                        dyr = Math.Ceiling(dyr)
                    End If
                    ' dyr = Math.Round(dyr)
                    Dim dbgt, davail As Integer
                    Dim rate As Double
                    Dim m1 As Double
                    Dim m2 As Double
                    Dim y_end As Date = ks.StringToDate(d2 & ", " & d_hire.Year.ToString, "MMMM dd, yyyy")
                    m1 = (y_end.Subtract(d_hire).Days / 30.4375)

                    ' Response.Write(m1)
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
                    Dim nnd As Integer
                    Dim data_c_c As Integer = 0

                    Do
                        If i = 0 Then
                            nnd = n_days + i
                        Else

                            nnd = n_days + i

                        End If
                        rate = (nnd) / 12
                        strout &= "Rate:" & rate.ToString
                        dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)
                        If dbgt > 30 Then
                            dbgt = 30
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
                        'strout &= "<br>Date avail:" & davail
                        'Response.Write(rate.ToString & "===" & dbgt & "----" & davail)
                        If dbgt <> 0 Then

                        End If
                        Dim dt4 As DataTableReader
                        If dbgt <> 0 Then
                            If dbgt > 30 Then
                                dbgt = 30
                            End If
                            dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emptid='" & emptid.ToString & "' and l_s_year='" & hd.ToShortDateString & "' order by id desc", conx)

                            If dt4.HasRows = True Then
                                'strout &= ("<tr><td>has rowwww</td></tr>")
                            Else
                                sql = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period,emptid) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye, "MM/DD/YYYY").ToString & "'," & dbgt.ToString & "," & emptid.ToString & ")"
                                strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                '  save_err = dbx.save(sql, conx, pathx).ToString
                                If save_err <> "1" Then
                                    strout &= "Save error"
                                Else
                                    data_c_c += 1
                                End If
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

                    Loop Until Today.Subtract(hd).Days / 30.41 < 0


                    If data_c_c > 0 Then
                        strout &= data_c_c.ToString & " new Data has been added"
                    Else
                        strout &= "No Change in the database"
                    End If

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
End Class
