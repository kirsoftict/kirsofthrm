Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Web.UI.Page
Imports System.Security
'Imports System.Web.SessionState.HttpSessionState
Imports System.IO
Imports System.Web.HttpServerUtility
Imports System.Web.HttpException
Imports System.Web.HttpRuntime
Imports System.Net.Mail
Imports System.Collections.Generic
Imports System.Text
Imports Kirsoft.hrm
Partial Class leavetake
    Inherits System.Web.UI.Page
    Function newcalc()
        Dim sql As String = ""
        Dim db As New dbclass
        Dim dt, dt2 As DataTableReader
        Dim fm As New formMaker
        Dim rtn(2) As String
        Dim sec As New k_security
        Dim loan As Double = 0
        Dim rslt As String = ""
        Dim rddate As Date
        Dim sqlp As String = " Begin Transaction" & Chr(13)
        Sql = "select * from emp_leave_take where approved_date is null and emptid=" & Session("emptid")
        Dim rqd As Double
        Try
            rslt = fm.getinfo2("select sum(isnull(loan,0)) from emp_leave_take where emptid=" & Session("emptid"), Session("con"))
            If IsNumeric(rslt) Then
                loan = CDbl(rslt)
            Else
                loan = 0
            End If
            dt = db.dtmake("thscal", sql, Session("con"))
            If dt.HasRows Then
                dt.Read()
                rddate = dt.Item("date_taken_from")
                rqd = dt.Item("no_days")
                ' Response.Write(rqd.ToString)
                Dim rid As Integer = dt.Item("id")

                sql = "select * from show_leave_bal where emp_id='" & Session("emp_id") & "' and emptid=" & Session("emptid") & " and Balance>0 order by id"
                dt2 = db.dtmake("thscals", sql, Session("con"))
                Dim d1, d2 As Date
                Dim bgt As Double
                Dim bgid As String
                Dim used As Double
                Dim bal As Double
                Dim remain As Double = 0
                Dim taken As Double = 0
                Dim v As Double
                If loan > 0 Then
                    rtn(0) = sec.dbStrToHex(sqlp)
                    If rqd < 0 Then
                        rqd = 0
                    End If
                    rtn(1) = rqd.ToString
                Else


                    If dt2.HasRows Then
                        While dt2.Read

                            d1 = dt2.Item("Year Start")
                            d2 = dt2.Item("Year End")
                            bgt = dt2.Item("Budget")
                            v = fm.showavdate(dt2.Item("Year Start"), dt2.Item("Year End"), bgt)
                            If fm.isexp(rddate, d2, 2, "y") Then

                            Else
                                If dt2.IsDBNull(8) = True Then
                                    bgid = dt2.Item("id")
                                    used = dt2.Item("Used")
                                    bal = dt2.Item("Balance")
                                    ' Response.Write(bal.ToString & "===av:" & v.ToString & "used:" & used.ToString & "---" & (v - used).ToString)
                                    If v - used > rqd Then

                                        ' Response.Write("take=>" & bgid & " amt2=" & rqd & "<br>")
                                        If rqd <> 0 Then
                                            sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & rqd & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                        End If
                                        ' sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & rqd & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                        rqd = 0
                                    Else
                                        '  Response.Write("take=>" & bgid & " amt>=" & CInt(v) & "<br>")
                                        If v - used >= 0.5 Then
                                            If v - used < 1 Then
                                                v = 0.5
                                            Else
                                                v = v - used
                                                If v < CDbl(v) + 0.5 Then
                                                    v = CDbl(v)
                                                Else
                                                    v = CInt(v) + 0.5
                                                End If
                                            End If
                                            sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & v & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                        Else
                                            ' sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & CInt(v) & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                            v = 0
                                        End If


                                        rqd = rqd - CDbl(v)


                                    End If

                                End If
                            End If
                        End While

                    End If
                    rtn(0) = sec.dbStrToHex(sqlp)
                    If rqd < 0 Then
                        rqd = 0
                    End If
                    rtn(1) = rqd.ToString
                End If
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
            '  Response.Write("<script type='text/javascript'> //window.location = 'logout.aspx';</script>")

            ' Response.Redirect("logout.aspx")
        End Try
        Return rtn
    End Function

    Function newcalc2()
        ' Response.Write("expried portion")
        Dim sql As String = ""
        Dim db As New dbclass
        Dim dt, dt2 As DataTableReader
        Dim fm As New formMaker
        Dim rtn(2) As String
        Dim sec As New k_security
        Dim sqlp As String = " Begin Transaction" & Chr(13)
        sql = "select * from emp_leave_take where approved_date is null and emptid=" & Session("emptid")
        Dim rqd As Double
        Try
            dt = db.dtmake("thscal", sql, Session("con"))
            If dt.HasRows Then
                dt.Read()
                rqd = dt.Item("no_days")
                Response.Write(rqd.ToString)
                Dim rid As Integer = dt.Item("id")

                sql = "select * from show_leave_bal where emp_id='" & Session("emp_id") & "' and emptid=" & Session("emptid") & " and Balance>0 order by id"
                dt2 = db.dtmake("thscals", sql, Session("con"))
                Dim d1, d2 As Date
                Dim bgt As Double
                Dim bgid As String
                Dim used As Double
                Dim bal As Double
                Dim remain As Double = 0
                Dim taken As Double = 0
                Dim v As Double

                If dt2.HasRows Then
                    Dim sttl As String = "0"
                    While dt2.Read

                        If dt2.IsDBNull(8) = True Then
                            d1 = dt2.Item("Year Start")
                            d2 = dt2.Item("Year End")
                            bgt = dt2.Item("Budget")
                            v = fm.showavdate(dt2.Item("Year Start"), dt2.Item("Year End"), bgt)

                            bgid = dt2.Item("id")
                            used = dt2.Item("Used")
                            bal = dt2.Item("Balance")
                            '  Response.Write("<br>" & v.ToString & "======" & bgid & "======" & used & "<br>")
                            sttl = (fm.getinfo2("select bal from leav_settled where bgtid=" & bgid, Session("con")))
                            If Not IsNumeric(sttl) Then
                                sttl = "0"
                            End If

                            ' Response.Write(bal.ToString & "===av:" & v.ToString & "used:" & used.ToString & "---" & (v - used).ToString)
                            If v - used - CDbl(sttl) > rqd Then

                                ' Response.Write("take=>" & bgid & " amt2=" & rqd & "<br>")
                                If rqd <> 0 Then
                                    sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & rqd & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                End If
                                rqd = 0
                            Else

                                If v - used - CDbl(sttl) >= 0.5 Then
                                    If v - used < 1 Then
                                        v = 0.5
                                    Else

                                        v = v - used

                                        If v < CInt(v) + 0.5 Then
                                            v = CInt(v)
                                            Response.Write("take=>" & bgid & " amt>=" & CInt(v) & "<br>")
                                        Else
                                            v = CInt(v) + 0.5
                                        End If
                                    End If
                                    ' Response.Write("<br>v===" & v.ToString & "insert into empleavapp(used,bgt_id,req_id,approved) values(" & v & "," & bgid & "," & rid & ",'y')" & Chr(13))

                                    If CDbl(v) > 0 Then
                                        sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & v & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                        ' Response.Write("<br>v===" & "insert into empleavapp(used,bgt_id,req_id,approved) values(" & v & "," & bgid & "," & rid & ",'y')" & Chr(13))
                                    End If
                                Else
                                    ' sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & CInt(v) & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                    v = 0
                                End If


                                rqd = rqd - CDbl(v)


                            End If

                        End If
                    End While

                End If
                rtn(0) = sec.dbStrToHex(sqlp)
                If rqd < 0 Then
                    rqd = 0
                End If
                rtn(1) = rqd.ToString

            End If

        Catch ex As Exception
            Response.Write("<script type='text/javascript'> window.location = 'logout.aspx';</script>")

            Response.Redirect("logout.aspx")
        End Try

        Return rtn
    End Function

    Function newcalcloan(ByVal id As String)

        Dim sql As String = ""
        Dim db As New dbclass
        Dim dt, dt2 As DataTableReader
        Dim fm As New formMaker
        Dim rtn(2) As String
        Dim sec As New k_security
        Dim sqlp As String = ""
        Dim rqdate As Date
        sql = "select * from emp_leave_take where id=" & id
        Dim rqd As Double
        Try
            dt = db.dtmake("thscal", sql, Session("con"))
            If dt.HasRows Then
                dt.Read()
                rqd = dt.Item("loan")
                rqdate = dt.Item("date_taken_from")
                'Response.Write("<br>....." & rqd.ToString)
                Dim rid As Integer = dt.Item("id")

                sql = "select * from show_leave_bal where emp_id='" & Session("emp_id") & "' and emptid=" & Session("emptid") & " and Balance>0 "
                dt2 = db.dtmake("thscals", sql, Session("con"))
                Dim d1, d2 As Date
                Dim bgt As Double
                Dim bgid As String
                Dim used As Double
                Dim bal As Double
                Dim remain As Double = 0
                Dim taken As Double = 0
                Dim v As Double

                If dt2.HasRows Then
                    While dt2.Read

                        d1 = dt2.Item("Year Start")
                        d2 = dt2.Item("Year End")
                        bgt = dt2.Item("Budget")
                        v = fm.showavdate(dt2.Item("Year Start"), dt2.Item("Year End"), bgt)
                        If fm.isexp(rqdate, d2, 2, "y") Then

                        Else

                            'Response.Write(d1.ToString & "....." & d2.ToString & "<br>")
                            If dt2.IsDBNull(8) = True Then
                                bgid = dt2.Item("id")
                                used = dt2.Item("Used")
                                bal = dt2.Item("Balance")
                                ' Response.Write(bal.ToString & "===av:" & v.ToString & "used:" & used.ToString & "---" & (v - used).ToString)
                                If v - used > rqd Then

                                    ' Response.Write("take=>" & bgid & " amt2=" & rqd & "<br>")
                                    sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & rqd & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                    rqd = 0
                                Else
                                    '  Response.Write("take=>" & bgid & " amt>=" & CInt(v) & "<br>")
                                    If v - used >= 0.5 Then
                                        If v - used < 1 Then
                                            v = 0.5
                                        Else

                                            v = v - used
                                            If v < CInt(v) + 0.5 Then
                                                v = CInt(v)
                                            Else
                                                v = CInt(v) + 0.5
                                            End If
                                        End If
                                        sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & v & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                    Else
                                        'sqlp &= "insert into empleavapp(used,bgt_id,req_id,approved) values(" & CInt(v) & "," & bgid & "," & rid & ",'y')" & Chr(13)
                                        v = 0
                                    End If

                                    ' Response.Write("<br>" & rqd.ToString & "---" & v & "<br>")
                                    rqd = rqd - v


                                End If
                            End If
                        End If
                    End While

                End If
                rtn(0) = sec.dbStrToHex(sqlp)
                If rqd < 0 Then
                    rqd = 0
                End If
                rtn(1) = rqd.ToString

            End If

        Catch ex As Exception
            Response.Write("<script type='text/javascript'> window.location = 'logout.aspx';</script>")

            Response.Redirect("logout.aspx")
        End Try
        Return rtn
    End Function
    Function approved(ByVal pass As Integer)
        Dim appdate As Date
        Dim sql2(1) As String
        Dim i As Integer
        Dim db As New dbclass
        Dim flg As String
        Dim msg As String = ""
        If pass = 1 Then

            Dim bgt1() As String = Request.QueryString("bgt").Split(",")
            Dim req1() As String = Request.QueryString("nd").Split(",")
            appdate = Request.QueryString("appdate")
            If appdate.ToString = "" Then
                appdate = Now
            Else
                appdate = appdate
            End If
            Dim val, fld As String
            val = "("
            fld = "("
            Dim sf(8) As String
            sql2(0) = ""
            For i = 0 To req1.Length - 1
                val = "("
                'Response.Write(req1(i).ToString)
                If req1(i).ToString <> "" Then
                    Response.Write(req1(i).ToString)
                    If CDbl(req1(i)) > 0 Then

                        ReDim Preserve sql2(sql2.Length + 1)
                        val &= "'" & req1(i) & "'"
                        If bgt1(i).ToString <> "" Then
                            val &= ",'" & bgt1(i) & "'"
                        Else
                            val &= ",'0'"
                        End If

                        val &= ",'" & Request.QueryString("id").ToString & "','y')"
                        sql2(sql2.Length - 1) = "insert into empleavapp(used,bgt_id,req_id,approved) values " & val

                        'sf(i) = db.save(sql, session("con"),session("path"))
                        'Response.Write("<br>" & sql)
                        ' sql = ""
                    End If
                Else
                    sf(i) = ""
                End If
                'Response.Write(sql)
            Next
            ReDim Preserve sql2(sql2.Length + 1)
            sql2(sql2.Length - 1) = "update emp_leave_take set date_return='" & Request.QueryString("datereturn") & "',approved_by='" & Session("emp_iid") & "',approved_date='" & appdate & "' where id=" & Request.QueryString("id")
            Dim dbwriter As String = "Begin Transaction" & Chr(13)

            For i = 0 To sql2.Length - 1
                If String.IsNullOrEmpty(sql2(i)) = False Then
                    dbwriter &= sql2(i) & Chr(13)
                End If
            Next
            'Response.Write(sql)
            ' Response.Write("<textarea>" & dbwriter & "</textarea>")

            flg = db.excutes(dbwriter, Session("con"), Session("path"))
            If flg > 0 Then
                flg = db.excutes("Commit", Session("con"), session("path"))
                msg = "Approved!"
                If flg <> -1 Then
                    db.excutes("RollBack", Session("con"), Session("path"))
                    msg = "data is not approved"
                End If
            Else
                msg = "Not Approved"
                db.excutes("RollBack", Session("con"), session("path"))
            End If
            ' Response.Write(sf(i + 1).ToString)
            'Response.Write(sql)

        End If
        Return msg
    End Function
    Function newapproved(ByVal pass As Integer)
        Dim appdate As Date
        Dim sql2 As String
        Dim sql As String
        Dim i As Integer
        Dim db As New dbclass
        Dim sec As New k_security
        Dim flg As String
        Dim msg As String = ""
        Dim geti() As String
        If pass = 1 Then


            geti = newcalc() ' approved form active leave

            appdate = Request.Form("appdate")
            If appdate.ToString = "" Then
                appdate = Now
            End If
            sql2 = "update emp_leave_take set loan=" & geti(1) & ", date_return='" & Request.QueryString("datereturn") & "',approved_by='" & Session("emp_iid") & "',approved_date='" & appdate & "' where id=" & Request.QueryString("id")
            sql = Request.Form("hiddenpass")
            ' Response.Write("<textarea id='just' cols='200' rows='10'>" & sec.dbHexToStr(geti(0)) & Chr(13) & sql2 & "</textarea>")

        Else

            geti = newcalc2() 'approved from expired portion.

            appdate = Request.Form("appdate")
            If appdate.ToString = "" Then
                appdate = Today.ToShortDateString
            End If
            sql2 = "update emp_leave_take set loan=" & geti(1) & ", date_return='" & Request.QueryString("datereturn") & "',approved_by='" & Session("emp_iid") & "',approved_date='" & appdate & "' where id=" & Request.QueryString("id")
            sql = Request.Form("hiddenpass")
            Response.Write("<textarea id='just' cols='200' rows='10'>" & sec.dbHexToStr(geti(0)) & Chr(13) & sql2 & "</textarea>")
        End If
        flg = 0
        Response.Write(sec.dbHexToStr(geti(0)) & sql2.Replace(Chr(13), "<br>"))
        flg = db.excutes(sec.dbHexToStr(geti(0)) & sql2, Session("con"), Session("path"))
        Response.Write(flg)
        If IsNumeric(flg) Then
            If CInt(flg) > 0 Then
                flg = db.excutes("Commit", Session("con"), session("path"))
                If flg <> -1 Then
                    db.excutes("RollBack", Session("con"), Session("path"))
                Else
                    msg = "Leave is Approved"
                End If

            Else
                db.excutes("RollBack", Session("con"), session("path"))
                msg = "Sorry Data is not Saved"
                Response.Write("sorry Data is not Save")
            End If
        End If

        Return msg
    End Function
    Function filesview(ByVal path As String, ByVal empid As String, ByVal folder As String, ByVal root As String) As String
        Dim loc As String = path & "\" & empid & "\" & folder & "\"
        'Dim f As Directory
        Dim up As New file_list
        Dim rtstr As String = ""
        root &= "/" & empid & "/" & folder
        If Directory.Exists(loc) = True Then

            Dim ext As String = ""
            Dim fname As String = ""
            'rtstr = "what  what..." & loc
            For Each d As String In Directory.GetDirectories(loc)
                rtstr &= "<div title='' style='display block; clear:both;'>"
                Dim fld() As String
                fld = d.Split("\")
                rtstr &= "<span style='background:#243772;font-size:15pt; color:white;'>Directory: " & fld(fld.Length - 1) & "</span><br>"
                For Each k As String In Directory.GetFiles(d)
                    rtstr = rtstr & "<div style='display:inline; float:left; width:100px;'>" & _
                "<span style=' display:block'>"
                    Select Case up.file_ext(k).ToLower
                        Case ".doc", ".docx"
                            fname = "msword"
                        Case ".pdf"
                            fname = "pdf_icon"
                        Case Else
                            fname = "unknown"
                    End Select
                    Dim ff As String

                    ff = k.Replace("\", "~")
                    rtstr = rtstr & " <img src='images/gif/" & fname & ".gif' height='40px' width='40px' alt='" & up.findfilename(k) & "' title='" & up.findfilename(k) & "' />" & _
                        " <br /><span()>"
                    Dim fn As String = up.findfilename(k)
                    If fn.Length > 8 Then
                        fn = fn.Substring(0, 5) & "~." & up.file_ext(k)
                    End If
                    rtstr = rtstr & fn & "</span>" & _
           " <span><a href=" & Chr(34) & "javascript:del('" & ff & "','1st');" & Chr(34) & ">delete</a></span>&nbsp;&nbsp;&nbsp;<span>" & _
           "<a href=" & Chr(34) & root & "/" & fld(fld.Length - 1) & "/" & up.findfilename(k) & Chr(34) & ">View</a></span>" & _
        "</span></div>" & _
        "<div style='width:15px; float:left;'>&nbsp;</div>"
                Next
                rtstr &= "</div><div style='clear:both'>"
            Next
        Else
            rtstr = "<div style='height:75px;'>file doesnt found<br></div>"
        End If
        If rtstr = "" Then
            rtstr = "is empty"
        End If
        Return rtstr
    End Function
    Function calcwhen3(ByVal dtstart As Date, ByVal noday As Double, ByVal empid As String, ByVal half As String)
        Dim hcount As Integer = 0
        Dim we As Integer = 0
        Dim wday As Integer = 0
        Dim dlist As String = ""
        Dim dateend As Date
        dateend = dtstart
        Dim sysd As New datetimecal
        Dim i As Integer = 0
        Dim flg As Boolean
        If half = "y" Then
            noday = noday * 2
        End If
        Dim cnext As Date
        wday = 0
        Dim rdate As Integer = 0

        Do
dot:
            dateend = dtstart.AddDays(i)
            If sysd.isPublic(dateend, Session("con")) = True Then
                hcount += 1
                'Response.Write("Public Holiday:" & hcount & " date: " & dateend.ToShortDateString & "<br>")
            ElseIf sysd.isWeekEnd(dateend, empid, Session("con")).ToString = "True" Then
                we += 1
                'Response.Write("we Holiday:" & we & " date: " & dateend.ToShortDateString & "<br>")
            Else
                wday += 1
                ' Response.Write("wday Holiday:" & wday & " date: " & dateend.ToShortDateString & "<br>")
            End If
            ' Response.Write(CInt(Math.Round(noday, 0)).ToString & " = " & wday.ToString & "<br>")

            If CInt(Math.Ceiling(noday)).ToString = wday.ToString Then
                ' Response.Write(Math.Ceiling(noday))
                cnext = dtstart.AddDays(i + 1)
                If sysd.isPublic(cnext, Session("con")) = True Then

                    rdate += 1
                    'Response.Write("found Public h day at the end<br>")
                    i += 1
                    GoTo dot
                ElseIf sysd.isWeekEnd(cnext, empid, Session("con")).ToString = "True" Then
                    'Response.Write("found we at the end<br>")
                    rdate += 1
                    i += 1
                    GoTo dot
                End If
            End If
            i += 1
        Loop While Math.Round(noday, 0) > wday

        '  dateend = dtstart.AddDays(hcount + we + wday)

        'dateend = dtstart.AddDays(wday + hcount + we)
        ' dateend = dtstart.AddDays(wday)

        Dim intarr(6) As Object
        intarr(0) = dtstart
        intarr(4) = dateend
        intarr(1) = hcount
        intarr(2) = we
        intarr(3) = dlist

        intarr(5) = dateend.AddDays(1)
        Return intarr
    End Function

    Public Function getleaveinfox0(ByVal emptid As Integer, ByVal con As SqlConnection)
        Dim output As String = ""
        output = "<div id=" & Chr(34) & "divleave" & Chr(34) & " class=" & Chr(34) & "collapsed" & Chr(34) & " style=" & Chr(34) & "" & Chr(34) & " > " & _
  "<div id=" & Chr(34) & "print2" & Chr(34) & "style=" & Chr(34) & "float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & _
  "javascirpt:print('divleave','Report_print','<br> Leave Summery','<%response.write(Today.ToLongDateString)%>');" & _
  Chr(34) & "><img src='images/ico/print.ico' alt=" & Chr(34) & "print" & Chr(34) & "/>print</div>" & _
  " <span class=" & Chr(34) & "collapsed" & Chr(34) & "  onclick=" & Chr(34) & "showHideSubMenu(this)" & Chr(34) & _
  " style=" & Chr(34) & "cursor:pointer; color:blue;" & Chr(34) & _
  ">See more Details</span> " & _
  "<span>Hire-Date:"
        Dim bgtid As Integer
        Dim dbs As New dbclass
        'Dim rqid As Integer
        Dim bgtno As Double = 0
        Dim ndav As Double = 0
        Dim used As Double = 0
        Dim tused As Double = 0
        Dim tndav As Double = 0
        Dim dt As DataTableReader
        Dim color As String
        Dim fm As New formMaker
        Dim i As Integer
        color = "blue"
        Dim dt2 As DataTableReader
        'Dim emptid As Integer
        ' Dim fm As New formMaker

        Dim gethr As String = fm.getinfo2("select hire_date from emprec where id=" & emptid, con)
        Dim isrd As String = fm.getinfo2("select resign_date from emp_resign where emptid=" & emptid, con)

        If IsDate(gethr) = True Then
            gethr = CDate(gethr).ToShortDateString
            gethr = MonthName(CDate(gethr).Month) & " " & CDate(gethr).Day.ToString & ", " & CDate(gethr).Year.ToString
        End If
        output &= gethr & "</span>" & _
       "<ul id=" & Chr(34) & "me" & Chr(34) & " style=" & Chr(34) & "display:none; vertical-align:top; " & Chr(34) & "> "

        Try
            dt = dbs.dtmake("taxpayer", "select id,l_s_year,l_e_year,no_days_with_period from emp_leave_budget where emptid=" & emptid & " ORDER BY ID", con)

            Dim loan As Double = 0
            If dt.HasRows Then
                output &= "<table width='700' >" & _
                "<tr><td><label>Year End</label></td>" & _
                "<td style='text-align:right;'>Bugdeted</td></tr>"
                tndav = 0
                While dt.Read
                    used = 0
                    ndav = 0
                    '   loan = 0
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fdafda"
                    End If
                    If fm.isexp(Today.ToShortDateString, dt.Item("l_e_year"), 2, "y") Then
                        color = "red"
                    End If
                    bgtid = dt.Item("id")
                    output &= "<tr style='background-color:" & color & ";'>"
                    For i = 2 To dt.FieldCount - 1
                        If i = 2 Then
                            Dim dd As Date
                            dd = dt.Item(i)
                            output &= "<td>" & MonthName(dd.Month) & " " & dt.Item(i).day.ToString & ", " & dt.Item(i).year.ToString & "</td>"
                        Else

                            If IsDate(isrd) Then
                                Dim ddd As Integer

                                Try

                                    ddd = CDate(dt.Item("l_e_year")).Subtract(CDate(isrd)).Days

                                    If ddd < 0 Then
                                        ndav = showavdate_rev(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"), "")
                                    Else
                                        ndav = showavdate_rev(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"), isrd)
                                    End If
                                Catch ex As Exception
                                    Response.Write("error" & ddd.ToString & ex.ToString & "<br>")
                                    ' Response.Write("<br>" & dt.Item("l_e_year") & "<br>")
                                End Try
                                'Response.Write("<br>" & ndav & "<br>")



                            Else
                                ndav = fm.showavdate(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"))


                            End If
                            If Math.Round(ndav, 2) < dt.Item("no_days_with_period") Then
                                output &= "<td align='right'>Total Days:" & dt.Item("no_days_with_period") & " | Available :" & Math.Round(ndav, 2).ToString & "</td>"
                            Else
                                output &= "<td align='right'>" & Math.Round(ndav, 2).ToString & "</td>"
                            End If


                        End If
                    Next
                    output &= "</tr>"
                    dt2 = dbs.dtmake("bgtused", "select * from empleavapp where bgt_id=" & bgtid & " order by id desc", con)

                    If dt2.HasRows Then
                        output &= "<tr><td colspan='2' width='700' bgcolor='" & color & "'>" & _
                        "<table bgcolor='" & color & "' width='700' align='left'>" & _
                        "<tr><td><label>Request Id</label></td><td><label>Budget id</label></td><td><label>Days taken</label></td></tr>"

                        While dt2.Read
                            output &= "<tr>"
                            For i = 1 To dt2.FieldCount - 2
                                If i = 3 Then
                                    used = used + CDbl(dt2.Item(i))
                                End If
                                output &= "<td>" & dt2.Item(i) & "</td>"
                            Next
                            output &= "<td>"
                            If fm.getinfo2("select byhalfday from emp_leave_take where id='" & dt2.Item("req_id") & "'", con) = "y" Then
                                output &= "by half days, Most likely day * 2"
                            End If
                            output &= "&nbsp;</td>"
                            output &= "</tr>"
                            Dim lamt As Object = fm.getinfo2("SELECT loan FROM emp_leave_take where id='" & dt2.Item("req_id") & "'", con)
                            ' Response.Write(dt2.Item("req_id"))
                            If IsNumeric(lamt) Then

                                loan = loan + lamt

                            Else
                                ' Response.write(lamt.ToString)
                                loan = loan + 0

                            End If
                        End While

                        output &= "<tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Days taken: </td><td align='left'>" & used.ToString & _
                             "</td>" & _
"</tr><tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available from this section:</td><td align='right'>" & Math.Round((ndav - used), 2).ToString & "</td></tr></table></tr>"

                    End If
                    dt2.Close()
                    If color <> "red" Then
                        tused = Math.Round((tused + used), 2)
                        tndav = Math.Round((tndav + (ndav - used)), 2)
                    End If

                End While

                Dim othloan As String
                othloan = fm.getinfo2("select sum(isnull(loan,0)) as loanx from emp_leave_take where id Not in(select req_id from empleavapp) and  emptid=" & emptid, con)
                If IsNumeric(othloan) Then
                    loan = loan + othloan
                End If
                output &= "<tr  style='background-color:#aabbcc;text-align:right;'><td colspan=2>Total Day taken: </td><td align='left'>" & tused.ToString & _
                         "</td></tr>" & _
                         "<tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Deficient</td><td>(" & (loan).ToString & _
                                             ")</td></tr><tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available Days: </td><td align='right' width='20px'>"
                If tndav - CDbl(loan) < 0 Then
                    output &= "(" & (tndav - CDbl(loan)) * -1.ToString & ")</td></tr></table>"
                Else
                    output &= "" & (tndav - CDbl(loan)).ToString & "</td></tr></table>"

                End If
                dt.Close()
            End If
            '    Response.Write(output)loan()
            'Return output
        Catch ex As Exception
            output &= ex.ToString & "<br> <script type='text/javascript'>//document.location.href='admin_home.php'" & Chr(13) & _
            "window.location='empcontener.aspx';</script>"
        End Try

        output &= "   </ul></div>"

        dbs = Nothing
        Return output
    End Function
 Public Function getleaveinfo(ByVal emptid As Integer, ByVal con As SqlConnection)
        Dim output As String = ""
        output = "<div id=" & Chr(34) & "divleave" & Chr(34) & " class=" & Chr(34) & "collapsed" & Chr(34) & " style=" & Chr(34) & "" & Chr(34) & " > " & _
  "<div id=" & Chr(34) & "print2" & Chr(34) & "style=" & Chr(34) & "float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & _
  "javascirpt:print('divleave','Report_print','<br> Leave Summery','<%response.write(Today.ToLongDateString)%>');" & _
  Chr(34) & "><img src='images/ico/print.ico' alt=" & Chr(34) & "print" & Chr(34) & "/>print</div>" & _
  " <span class=" & Chr(34) & "collapsed" & Chr(34) & "  onclick=" & Chr(34) & "showHideSubMenu(this)" & Chr(34) & _
  " style=" & Chr(34) & "cursor:pointer; color:blue;" & Chr(34) & _
  ">See more Details</span> " & _
  "<span>Hire-Date:"
        Dim bgtid As Integer
        Dim dbs As New dbclass
        'Dim rqid As Integer
        Dim bgtno As Double = 0
        Dim ndav As Double = 0
        Dim used As Double = 0
        Dim tused As Double = 0
        Dim tndav As Double = 0
        Dim dt As DataTableReader
        Dim color As String
        Dim fm As New formMaker
        Dim i As Integer
        color = "blue"
        Dim dt2 As DataTableReader
        'Dim emptid As Integer
        ' Dim fm As New formMaker

        Dim gethr As String = fm.getinfo2("select hire_date from emprec where id=" & emptid, con)
        Dim isrd As String = fm.getinfo2("select resign_date from emp_resign where emptid=" & emptid, con)
        If IsDate(gethr) = True Then
            gethr = CDate(gethr).ToShortDateString
            gethr = MonthName(CDate(gethr).Month) & " " & CDate(gethr).Day.ToString & ", " & CDate(gethr).Year.ToString
        End If
        output &= gethr & "</span>" & _
       "<ul id=" & Chr(34) & "me" & Chr(34) & " style=" & Chr(34) & "display:none; vertical-align:top; " & Chr(34) & "> "

        Try
            dt = dbs.dtmake("taxpayer", "select id,l_s_year,l_e_year,no_days_with_period from emp_leave_budget where emptid=" & emptid & " ORDER BY ID", con)

            Dim loan As Double = 0
            If dt.HasRows Then
                output &= "<table width='700' >" & _
                "<tr><td><label>Year End</label></td>" & _
                "<td style='text-align:right;'>Bugdeted</td></tr>"
                tndav = 0
                Dim riddup() As String = {""}
                While dt.Read
                    used = 0
                    ndav = 0
                    '  loan = 0
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fdafda"
                    End If
                    If fm.isexp(Today.ToShortDateString, dt.Item("l_e_year"), 2, "y") Then
                        color = "red"
                    End If
                    bgtid = dt.Item("id")
                    output &= "<tr style='background-color:" & color & ";'>"
                    For i = 2 To dt.FieldCount - 1
                        If i = 2 Then
                            Dim dd As Date
                            dd = dt.Item(i)
                            output &= "<td>" & MonthName(dd.Month) & " " & dt.Item(i).day.ToString & ", " & dt.Item(i).year.ToString & "</td>"
                        Else
                            If IsDate(isrd) Then
                                Dim ddd As Integer

                                Try

                                    ddd = CDate(dt.Item("l_e_year")).Subtract(CDate(isrd)).Days

                                    If ddd < 0 Then
                                        ndav = fm.showavdate_rev(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"), "")
                                    Else
                                        ndav = fm.showavdate_rev(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"), isrd)
                                    End If
                                Catch ex As Exception
                                    'Response.Write("error" & ddd.ToString & ex.ToString & "<br>")
                                    ' Response.Write("<br>" & dt.Item("l_e_year") & "<br>")
                                End Try
                                'Response.Write("<br>" & ndav & "<br>")



                            Else
                                ndav = fm.showavdate(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"))


                            End If
                            If Math.Round(ndav, 2) < dt.Item("no_days_with_period") Then
                                output &= "<td align='right'>Total Days:" & dt.Item("no_days_with_period") & " | Available :" & Math.Round(ndav, 2).ToString & "</td>"
                            Else
                                output &= "<td align='right'>" & Math.Round(ndav, 2).ToString & "</td>"
                            End If


                        End If
                    Next
                    output &= "</tr>"
                    Dim lvst As Double = 0
                    Dim rt As String = ""
                    rt = fm.getinfo2("select bal from leav_settled where bgtid=" & bgtid, con)
                    If IsNumeric(rt) Then
                        lvst = CDbl(rt)
                    End If
                    dt2 = dbs.dtmake("bgtused", "select * from empleavapp where bgt_id=" & bgtid & " order by id desc", con)

                    If dt2.HasRows Then
                        output &= "<tr><td colspan='2' width='700' bgcolor='" & color & "'>" & _
                        "<table bgcolor='" & color & "' width='700' align='left'>" & _
                        "<tr><td><label>Request Id</label></td><td><label>Budget id</label></td><td><label>Days taken</label></td></tr>"

                        While dt2.Read
                            output &= "<tr>"
                            For i = 1 To dt2.FieldCount - 2
                                If i = 3 Then
                                    used = used + CDbl(dt2.Item(i))
                                End If
                                output &= "<td>" & dt2.Item(i) & "</td>"
                            Next
                            output &= "<td>"
                            If fm.getinfo2("select byhalfday from emp_leave_take where id='" & dt2.Item("req_id") & "'", con) = "y" Then
                                output &= "by half days, Most likely day * 2"
                            End If
                            output &= "&nbsp;</td>"
                            output &= "</tr>"
                            Dim lamt As Object = fm.getinfo2("SELECT loan FROM emp_leave_take where id='" & dt2.Item("req_id") & "'", con)
                            If IsNumeric(lamt) Then
                                If fm.searcharray(riddup, dt2.Item("req_id")) = False Then
                                    riddup(riddup.Length - 1) = dt2.Item("req_id")
                                    ReDim Preserve riddup(riddup.Length + 1)
                                    riddup(riddup.Length - 1) = ""
                                    loan = loan + lamt
                                    ' output &= "<br> " & lamt & "=====>" & dt2.Item("req_id")
                                End If

                            Else
                                ' Response.write(lamt.ToString)
                                loan = loan + 0

                            End If
                        End While
                    Else

                        output &= "<tr><td colspan='2' width='700' bgcolor='" & color & "'>" & _
                                               "<table bgcolor='" & color & "' width='700' align='left'>" & _
                                               "<tr><td><label>Request Id</label></td><td><label>Budget id</label></td><td><label>Days taken</label></td></tr>"
                    End If
                    dt2.Close()
                    output &= "<tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Days taken: </td><td align='left'>" & used.ToString & _
                           "</td></tr>"
                    If lvst > 0 Then
                        output &= "<tr style='background-color:#fda9aa;text-align:right;'><td colspan='2'>Paid by money:</td><td align='right'>" & Math.Round((lvst), 2).ToString & "</td></tr>"

                    End If
                    output &= "<tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available from this section:</td><td align='right'>" & Math.Round((ndav - used - lvst), 2).ToString & "</td></tr></table></tr>"

                    If color <> "red" Then
                        tused = Math.Round((tused + used + lvst), 2)
                        tndav = Math.Round((tndav + (ndav - used - lvst)), 2)
                    End If
                End While
                Dim othloan As String
                othloan = fm.getinfo2("select sum(isnull(loan,0)) as loanx from emp_leave_take where id Not in(select req_id from empleavapp) and  emptid=" & emptid, con)
                If IsNumeric(othloan) Then
                    loan = loan + othloan
                End If
                output &= "<tr  style='background-color:#aabbcc;text-align:right;'><td colspan=2>Total Day taken: </td><td align='left'>" & tused.ToString & _
                         "</td></tr>" & _
                         "<tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Deficient</td><td>(" & (loan).ToString & _
                                             ")</td></tr><tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available Days: </td><td align='right' width='20px'>" & tndav - CDbl(loan).ToString & "</td></tr></table>"
                dt.Close()
            End If
            '    Response.Write(output)loan()
            'Return output
        Catch ex As Exception
            output &= ex.ToString & "<br> <script type='text/javascript'>//document.location.href='admin_home.php'" & Chr(13) & _
            "window.location='empcontener.aspx';</script>"
        End Try

        output &= "   </ul></div>"

        dbs = Nothing
        Return output
    End Function
    Public Function getleaveinfo22(ByVal emptid As Integer, ByVal con As SqlConnection)
        Dim output As String = ""
        output = "<div id=" & Chr(34) & "divleave" & Chr(34) & " class=" & Chr(34) & "collapsed" & Chr(34) & " style=" & Chr(34) & "" & Chr(34) & " > " & _
  "<div id=" & Chr(34) & "print2" & Chr(34) & "style=" & Chr(34) & "float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & _
  "javascirpt:print('divleave','Report_print','<br> Leave Summery','<%response.write(Today.ToLongDateString)%>');" & _
  Chr(34) & "><img src='images/ico/print.ico' alt=" & Chr(34) & "print" & Chr(34) & "/>print</div>" & _
  " <span class=" & Chr(34) & "collapsed" & Chr(34) & "  onclick=" & Chr(34) & "showHideSubMenu(this)" & Chr(34) & _
  " style=" & Chr(34) & "cursor:pointer; color:blue;" & Chr(34) & _
  ">See more Details</span> " & _
  "<span>Hire-Date:"
        Dim bgtid As Integer
        Dim dbs As New dbclass
        'Dim rqid As Integer
        Dim bgtno As Double = 0
        Dim ndav As Double = 0
        Dim used As Double = 0
        Dim tused As Double = 0
        Dim tndav As Double = 0
        Dim dt As DataTableReader
        Dim color As String
        Dim fm As New formMaker
        Dim i As Integer
        color = "blue"
        Dim dt2 As DataTableReader
        'Dim emptid As Integer
        ' Dim fm As New formMaker

        Dim gethr As String = fm.getinfo2("select hire_date from emprec where id=" & emptid, con)
        Dim isrd As String = fm.getinfo2("select resign_date from emp_resign where emptid=" & emptid, con)
        If IsDate(gethr) = True Then
            gethr = CDate(gethr).ToShortDateString
            gethr = MonthName(CDate(gethr).Month) & " " & CDate(gethr).Day.ToString & ", " & CDate(gethr).Year.ToString
        End If
        output &= gethr & "</span>" & _
       "<ul id=" & Chr(34) & "me" & Chr(34) & " style=" & Chr(34) & "display:none; vertical-align:top; " & Chr(34) & "> "

        Try
            dt = dbs.dtmake("taxpayerx", "select * from emp_leave_budget where emptid=" & emptid & " ORDER BY ID", con)

            Dim loan As Double = 0
            Dim rt As String
            Dim outdetail As String = ""
            Dim outsum As String = ""
            If dt.HasRows Then
                While dt.Read
                    used = 0
                    ndav = 0
                    bgtid = dt.Item("id")
                    rt = fm.getinfo2("select bal from leav_settled where bgtid=" & bgtid, con)

                 
                    '  loan = 0
                  
                    If fm.isexp(Today.ToShortDateString, dt.Item("l_e_year"), 2, "y") Then
                        color = "red"
                    End If

                    output &= "<div style='background-color:" & color & ";'>"
                    Dim dd As Date



                    dd = dt.Item("l_s_year")

                    output &= "Budget year Starts:" & MonthName(dd.Month) & " " & dd.Day.ToString & ", " & dd.Year.ToString & ""

                    If IsDate(isrd) Then
                        Dim ddd As Integer

                        Try

                            ddd = CDate(dt.Item("l_e_year")).Subtract(CDate(isrd)).Days

                            If ddd < 0 Then
                                ndav = fm.showavdate_rev(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"), "")
                            Else
                                ndav = fm.showavdate_rev(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"), isrd)
                            End If
                        Catch ex As Exception
                            'Response.Write("error" & ddd.ToString & ex.ToString & "<br>")
                            ' Response.Write("<br>" & dt.Item("l_e_year") & "<br>")
                        End Try
                        'Response.Write("<br>" & ndav & "<br>")



                    Else
                        ndav = fm.showavdate(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"))


                    End If
                    Dim balx As String
                    balx = fm.getinfo2("Select sum(used)  from empleavapp where bgt_id=" & bgtid, con)
                    Response.Write("<br> used balance:" & balx)
                    If IsNumeric(balx) = False Then
                        balx = "0"
                    End If
                    If Math.Round(ndav, 2) < dt.Item("no_days_with_period") Then
                        output &= "<div align='right'>Total Days:" & dt.Item("no_days_with_period") & " | Available :" & Math.Round(ndav, 2).ToString & "</div>"
                    Else
                        output &= "<div align='right'>" & Math.Round(ndav, 2).ToString & "</div>"
                    End If
                    'Response.Write("<br> used balance:" & balx)



                    output &= "</div>"

                    If CInt(balx) = 0 Then
                        outdetail = ""
                        outdetail &= "<div class='detail'>NO Leave"
                    Else
                        dt2 = dbs.dtmake("bgtused", "select * from empleavapp where bgt_id=" & bgtid & " order by id desc", con)

                        If dt2.HasRows Then
                            outdetail = ""
                            Response.Write("<br>balav after used:" & ndav)
                            outdetail &= "<div class='detail'>INNN"
                            outdetail &= "<table><tr><td>Detail:</td></tr><tr><td>req id</td><td>Bdt id</td><td>used</td><td>&nbsp;</td></tr>"
                            used = 0
                            While dt2.Read
                                outdetail &= "<tr><td>" & dt2.Item("req_id") & "</td><td>" & dt2.Item("bgt_id") & "</td>"

                                used = used + CDbl(dt2.Item("used"))

                                outdetail &= "<td>" & dt2.Item("used") & "</td>"

                                outdetail &= "<td>"
                                If fm.getinfo2("select byhalfday from emp_leave_take where id='" & dt2.Item("req_id") & "'", con) = "y" Then
                                    outdetail &= "by half days, Most likely day * 2"
                                End If
                                outdetail &= "&nbsp;</td>"
                                outdetail &= "</tr>"
                                Dim lamt As Object = fm.getinfo2("SELECT loan FROM emp_leave_take where id='" & dt2.Item("req_id") & "'", con)
                                If IsNumeric(lamt) Then

                                    loan = loan + lamt
                                    ' output &= "<br> " & lamt & "=====>" & dt2.Item("req_id")
                                End If


                            End While

                            outdetail &= "</table>"
                        End If

                        dt2.Close()
                    End If
                    outdetail &= "</div>"
                    outsum = ""
                    outsum &= "<div class='summeryx'><table><tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Days taken: </td><td align='left'>" & used.ToString & _
                        "</td></tr>"
                    'Response.Write("<br>" & rt.ToString)
                    If IsNumeric(rt) Then

                        outsum &= "<tr style='background-color:#fda9aa;text-align:right;'><td colspan='2'>Paid by money:</td><td align='right'>" & Math.Round((CDbl(rt)), 2).ToString & "</td></tr>"
                    Else
                        rt = "0"
                    End If
                    outsum &= "<tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available from this section:</td><td align='right'>" & Math.Round((ndav - used - CDbl(rt)), 2).ToString & "</td></tr></table></div></div>"

                    If color <> "red" Then
                        tused = Math.Round((tused + used + CDbl(rt)), 2)
                        tndav = Math.Round((tndav + (ndav - used - CDbl(rt))), 2)
                    End If

                    

                    output &= outdetail & outsum
                End While
                dt.Close()
                output &= "<div class='totalsum'><table><tr  style='background-color:#aabbcc;text-align:right;'><td colspan=2>Total Day taken: </td><td align='left'>" & tused.ToString & _
                             "</td></tr>" & _
                             "<tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Deficient</td><td>(" & (loan).ToString & _
                                                 ")</td></tr><tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available Days: </td><td align='right' width='20px'>" & tndav - CDbl(loan).ToString & "</td></tr></table></div>"

            End If
            '    Response.Write(output)loan()
            'Return output
        Catch ex As Exception
            output &= ex.ToString & "<br> "
        End Try

        output &= "   </ul></div>"

        dbs = Nothing
        Return output
    End Function
    Public Function getleaveinfox(ByVal emptid As Integer, ByVal con As SqlConnection)
        Dim output As String = ""

        Dim bgtid As Integer
        Dim dbs As New dbclass
        'Dim rqid As Integer
        Dim bgtno As Double = 0
        Dim ndav As Double = 0
        Dim used As Double = 0
        Dim tused As Double = 0
        Dim tndav As Double = 0
        Dim dt As DataTableReader
        Dim rtn(4) As Double
        Dim color As String
        Dim fm As New formMaker
        Dim i As Integer
        color = "blue"
        Dim dt2 As DataTableReader
        'Dim emptid As Integer
        ' Dim fm As New formMaker

        Dim gethr As String = fm.getinfo2("select hire_date from emprec where id=" & emptid, con)

        If IsDate(gethr) = True Then
            gethr = CDate(gethr).ToShortDateString
            gethr = MonthName(CDate(gethr).Month) & " " & CDate(gethr).Day.ToString & ", " & CDate(gethr).Year.ToString
        End If

        Try
            dt = dbs.dtmake("taxpayer", "select id,l_s_year,l_e_year,no_days_with_period from emp_leave_budget where emptid=" & emptid & " ORDER BY ID", con)

            Dim loan As Double = 0
            If dt.HasRows Then

                tndav = 0
                While dt.Read
                    used = 0
                    ndav = 0
                    '  loan = 0
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fdafda"
                    End If
                    If fm.isexp(Today.ToShortDateString, dt.Item("l_e_year"), 2, "y") Then
                        color = "red"
                    End If
                    bgtid = dt.Item("id")

                    For i = 2 To dt.FieldCount - 1
                        If i = 2 Then
                            Dim dd As Date
                            dd = dt.Item(i)
                        Else
                            ndav = fm.showavdate(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"))


                        End If
                    Next
                    dt2 = dbs.dtmake("bgtused", "select * from empleavapp where bgt_id=" & bgtid & " order by id desc", con)

                    If dt2.HasRows Then

                        While dt2.Read
                            output &= "<tr>"
                            For i = 1 To dt2.FieldCount - 2
                                If i = 3 Then
                                    used = used + CDbl(dt2.Item(i))
                                End If

                            Next

                            Dim lamt As Object = fm.getinfo2("SELECT loan FROM emp_leave_take where id='" & dt2.Item("req_id") & "'", con)
                            If IsNumeric(lamt) Then
                                loan = loan + lamt
                            Else
                                ' Response.write(lamt.ToString)
                                loan = loan + 0

                            End If
                        End While

                        ' Math.Round((ndav - used), 2).ToString & "</td></tr></table></tr>"

                    End If
                    dt2.Close()
                    If color <> "red" Then
                        tused = Math.Round((tused + used), 2)
                        tndav = Math.Round((tndav + (ndav - used)), 2)
                    End If
                End While

                rtn(0) = tused
                rtn(1) = loan
                rtn(2) = (tndav - CDbl(loan))
                rtn(3) = tndav
                dt.Close()
            End If
            '    Response.Write(output)loan()
            'Return output
        Catch ex As Exception
            output &= ex.ToString & "<br> <script type='text/javascript'>//document.location.href='admin_home.php'" & Chr(13) & _
            "window.location='empcontener.aspx';</script>"
        End Try



        dbs = Nothing
        Return rtn
    End Function
    Function loansettle()
        Dim fm As New formMaker
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim sql As String
        Dim rtnv() As String
        Dim othloan, bgid, req_id As Object
        Dim sec As New k_security
        Dim lrt() As Double
        Dim flg As Object
        ' Response.Write("loansee")
        lrt = getleaveinfox(Session("emptid"), Session("con"))
        For i As Integer = 0 To lrt.Length - 1
            '    Response.Write("<br>" & lrt(i).ToString)
        Next
        sql = "select * from emp_leave_take where emptid=" & Session("emptid") & " and loan>0"
        Try
            If lrt(3) >= 1 Then
                rs = dbs.dtmake("lvbal", sql, Session("con"))

                If rs.HasRows Then
                    While rs.Read
                        If lrt(3) > 1 Then
                            bgid = fm.getinfo2("select bgt_id from empleavapp where req_id=" & rs.Item("id"), Session("con"))
                            If IsNumeric(bgid) Then
                                rtnv = newcalcloan(rs.Item("id"))
                                sql = "Begin Transaction" & Chr(13)
                                sql &= sec.dbHexToStr(rtnv(0)).ToString & Chr(13)
                                sql &= "update emp_leave_take set loan=" & rtnv(1) & " where id=" & rs.Item("id")

                                Try
                                    flg = dbs.excutes(sql, Session("con"), Session("path"))


                                    If IsNumeric(flg) Then
                                        If CInt(flg) > 0 Then
                                            flg = dbs.excutes("Commit", Session("con"), Session("path"))
                                            Response.Write("Leave Update")
                                            If CInt(flg) <> -1 Then
                                                dbs.excutes("RollBack", Session("con"), Session("path"))
                                                Response.Write("Leave Not Update!" & flg.ToString)
                                            End If
                                        Else
                                            dbs.excutes("RollBack", Session("con"), Session("path"))
                                            Response.Write("Leave Not Update!" & flg.ToString)
                                        End If
                                    End If
                                Catch ex As Exception
                                    Response.Write(ex.ToString & " leavetake.aspx page error has occour " & sql)
                                End Try


                            Else
                                rtnv = newcalcloan(rs.Item("id"))
                                sql = "Begin Transaction" & Chr(13)
                                sql &= sec.dbHexToStr(rtnv(0)).ToString & Chr(13)
                                sql &= "update emp_leave_take set loan=" & rtnv(1) & " where id=" & rs.Item("id")

                                Try
                                    flg = dbs.excutes(sql, Session("con"), Session("path"))

                                    If IsNumeric(flg) Then
                                        If CInt(flg) > 0 Then
                                            flg = dbs.excutes("Commit", Session("con"), Session("path"))
                                            Response.Write("Leave Update")
                                            If CInt(flg) <> -1 Then
                                                dbs.excutes("RollBack", Session("con"), Session("path"))
                                                Response.Write("Leave Not Update!" & flg.ToString)
                                            End If
                                        Else
                                            dbs.excutes("RollBack", Session("con"), Session("path"))
                                            Response.Write("Leave Not Update!" & flg.ToString)
                                        End If
                                    End If
                                Catch ex As Exception
                                    Response.Write(ex.ToString & " leavetake.aspx page error has occour " & sql)
                                End Try


                            End If
                        End If
                        'Response.Write(rs.Item("loan").ToString & "<br>")
                    End While
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & Sql)
        End Try

        ' get balance
        ' send approved

        'othloan = fm.getinfo2("select sum(isnull(loan,0)) as loanx from emp_leave_take where id Not in(select req_id from empleavapp) and  emptid=" & Session("emptid") & " order by id", Session("con"))
        bgid = fm.getinfo2("select sum(isnull(loan,0)) as loanx from emp_leave_take where   emptid=" & Session("emptid") & " order by id", Session("con"))

        rs = dbs.dtmake("loanleave", "select * from emp_leave_take where id in(select req_id from empleavapp) and emptid=" & Session("emptid") & " and loan>0", Session("con"))

        If rs.HasRows Then


        End If
        rs.Close()
        dbs = Nothing


    End Function
    Public Function showavdate_rev(ByVal ds As Date, ByVal de As Date, ByVal v As Double, ByVal isrs As String) As Double
        Dim dif As Integer
        dif = de.Subtract(ds).Days
        Dim dt As Integer
        If IsDate(isrs) Then
            dt = CDate(isrs).Subtract(ds).Days
        Else
            dt = Today.Subtract(ds).Days
        End If

        Dim str As Double = 0

        Response.Write("<br>......" & dif.ToString & "===" & dt.ToString & ".....<br>")
        If (dt - dif) <= 0 Then

            str = (dt * v) / dif
            Response.Write(str.ToString)

        Else
            str = v
        End If
        Return str
    End Function
End Class
