Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Web.UI.Page
Imports System.Security
Imports System.IO
Imports System.Web.HttpServerUtility
Imports System.Web.HttpException
Imports System.Web.HttpRuntime
Imports System.Net.Mail

Imports System.Collections.Generic
Imports System.Text
Imports System
Imports System.DateTime
Imports System.Web.SessionState.HttpSessionState
Imports System.Text.RegularExpressions
Imports System.Configuration
Imports System.Xml


Namespace Kirsoft.hrm

    Public Class formMaker
        Public Function exception_hand(ByVal ex As Object) As Object
         

            Dim msg As New mail_system
            Dim outmsg As Object
            outmsg = msg.mailprep("z.kirubel@gmail.com", "ictnetconst@gmail.com", ex.ToString, "Error on Kirsoft.hrm")
            outmsg = msg.mailsend(outmsg)
            outmsg = msg.sendemail(ex.ToString, HttpContext.Current.Session("epwd"), HttpContext.Current.Session("efrom"), HttpContext.Current.Session("eto"), "Error: ON:" & HttpContext.Current.Session("company_name"), HttpContext.Current.Session("smtp"), HttpContext.Current.Session("eport"))

            If LCase(outmsg) <> "mail sent" Then
                If Directory.Exists("c:\temp\email") = False Then
                    MkDir("c:\temp\email")
                End If
                Dim obj As String
                obj = Now.ToString & Chr(13)
                obj &= "z.kirubl@gmail.com" & Chr(13)
                obj &= "Error on KirSoft.hrm" & Chr(13)
                obj &= ex.ToString & Chr(13)
                File.AppendAllText("c:\temp\email\email.txt", obj.ToString)
            End If

        End Function
        Public Function exception_hand(ByVal ex As Object, ByVal hdr As String) As Object


            Dim msg As New mail_system
            Dim outmsg As Object
            Dim sec As New k_security
            Dim paath As String = HttpContext.Current.Session("path")
            ' outmsg = msg.mailprep("z.kirubel@gmail.com", "ictnetconst@gmail.com", ex.ToString, hdr)
            ' outmsg = msg.mailsend(outmsg)
            outmsg = msg.sendemail(ex.ToString, HttpContext.Current.Session("epwd"), HttpContext.Current.Session("efrom"), HttpContext.Current.Session("eto"), hdr & HttpContext.Current.Session("company_name"), HttpContext.Current.Session("smtp"), HttpContext.Current.Session("eport"))

            If LCase(outmsg) <> "mail sent" Then
                If Directory.Exists(paath & "temp\email") = False Then
                    MkDir(HttpContext.Current.Session("path") & "temp\email")

                End If
                Dim obj As String
                obj = Now.ToString & Chr(13)
                obj &= "z.kirubl@gmail.com" & Chr(13)
                obj &= "Error on KirSoft.hrm" & Chr(13)
                obj &= ex.ToString & Chr(13)
                File.AppendAllText(paath & "temp\email\" & sec.Kir_StrToHex(hdr) & Now.ToString("ddmmyy") & ".txt", obj.ToString)
            End If

        End Function
        Public Function mailsender(ByVal msg As Object, ByVal toh As String, ByVal fromh As String, ByVal subject As String) As Object

            Dim mi As New mail_system
            Dim outmsg As Object
            outmsg = mi.mailprep(toh, fromh, msg, subject)
            outmsg = mi.mailsend(outmsg)
            If outmsg <> "Message sent" Then
                If Directory.Exists("c:\temp\email") = False Then
                    MkDir("c:\temp\email")
                End If
                Dim obj As String
                obj = Now.ToString & Chr(13)
                obj &= toh & Chr(13)
                obj &= fromh & Chr(13)
                obj &= subject & Chr(13)
                obj &= msg.ToString & Chr(13)
                File.AppendAllText("c:\temp\email\email.txt", obj.ToString)
            End If

        End Function
        Public Function saveleave(ByVal emptid As String, ByVal reqid As Integer, ByVal con As SqlConnection) As String
            Dim sql As String = ""
            Dim rt As DataTableReader
            Dim rtn As String = ""
            Dim dbx As New dbclass
            sql = "select * from show_leave_bal where emptid='" & emptid & "'"
            rt = dbx.dtmake("saveleave", sql, con)
            If rt.HasRows = True Then
                While rt.Read
                    rtn &= rt.Item("id") & "_____" & rt.Item("Budget") & rt.Item("Used") & rt.Item("Balance") & "<br>"

                End While
            End If
            Return rtn
        End Function
        Public Function helpback(ByVal hlp As Boolean, ByVal bak As Boolean, ByVal print4 As Boolean, ByVal print3 As Boolean, ByVal url As String, ByVal cmp As String) As Object
            Dim outp As String
            outp = " <div style='top:0px;width:400px; height:25px; padding: 0px 0px 0px 0px;float:right; font-size:9pt;'>"
            outp &= " <div style='float:right; width:300px; background:white;'>"
            If bak = True Then
                outp &= "<span style='color:Gray; cursor:pointer;' onclick=" & Chr(34) & "back('-1')" & Chr(34) & ">" & _
       " <img src='images/gif/ARW04LT.gif'  style='vertical-align:top;' title='Back' alt='<<Back'/>Back</span>&nbsp;&nbsp;"
            End If
            If hlp = True Then
                outp &= " |&nbsp;&nbsp;" & _
        " <span class='hlpm'  style='color:Gray; cursor:pointer;' onclick=" & Chr(34) & "javascript:gotoinfo('" & url & "')" & Chr(34) & ">Help</span> &nbsp;&nbsp;"
            End If
            If print4 = True Then
                outp &= "|&nbsp;&nbsp;<span id='print'  style='width:100px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('allList','Report_print','" & cmp & "','" & (Today.ToLongDateString) & "','printA4');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print A4</span>&nbsp;&nbsp;"

            End If
            If print3 = True Then
                outp &= "|&nbsp;&nbsp;<span id='print'  style='width:100px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('allList','Report_print','" & cmp & "','" & (Today.ToLongDateString) & "','printA3');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print A3</span>&nbsp;&nbsp;"

            End If
            outp &= "</div></div>"
            Return outp
        End Function
        Public Function datesort(ByVal dt() As Date) As Array


            Dim i As Integer = 0
            Array.Sort(dt)
            Return dt
        End Function

        Public Function catt(ByVal emptid As Integer, ByVal con As SqlConnection, ByVal pdate1 As Date, ByVal pdate2 As Date) As Object
            Try


                Dim ca As Double = 0
                Dim r As String
                r = Me.getinfo2("select count(id) from emp_att where emptid=" & emptid & " and att_date between '" & pdate1 & "' and '" & pdate2 & "' and status='A' and (daypartition='F' or daypartition is null)", con)
                If IsNumeric(r) Then
                    ca += CDbl(r)
                End If
                r = Me.getinfo2("select count(id) from emp_att where emptid=" & emptid & " and att_date between '" & pdate1 & "' and '" & pdate2 & "' and status='A' and (daypartition='M')", con)
                If IsNumeric(r) Then
                    ca += (CDbl(r) * 0.5)
                End If
                r = Me.getinfo2("select count(id) from emp_att where emptid=" & emptid & " and att_date between '" & pdate1 & "' and '" & pdate2 & "' and status='A' and (daypartition='A')", con)
                If IsNumeric(r) Then
                    ca += (CDbl(r) * 0.5)
                End If
                Return ca
            Catch ex As Exception
                'exception_hand(ex.ToString & " catt function, mastercode")
                Me.exception_hand(ex, "master page Erro")
            End Try
        End Function
        Public Function rightmk(ByVal val As String) As Object
            Dim fx() As String = {""}
            If String.IsNullOrEmpty(val) = False Then
                fx = val.Split(";")
                ReDim Preserve fx(UBound(fx) + 1)
                fx(UBound(fx) - 1) = ""
            End If
            Return fx
        End Function
        Public Function projtrans(ByVal proj As String, ByVal d1 As Date, ByVal con As SqlConnection) As String
            Dim rt(2) As String
            Dim dbx As New dbclass
            Dim fm As New formMaker
            Dim rs As DataTableReader
            Dim spl() As String
            Dim transproj As String
            Dim nod As Integer
            Dim cc As Integer = 1
            rt(0) = ""
            nod = Date.DaysInMonth((d1.Year), (d1.Month))
            If String.IsNullOrEmpty(proj) = False Then
                spl = proj.Split("|")
                rs = dbx.dtmake("trans", "select * from emp_job_assign where date_end between '" & d1.ToShortDateString & "' and '" & d1.AddDays(nod - 1) & "' and date_from<>'" & d1.ToShortDateString & "'", con)
                If rs.HasRows = True Then
                    While rs.Read
                        If cc = 1 Then
                            rt(0) = "Remark:<br>"
                        End If
                        'Response.Write(proj & "=-==" & fm.getinfo2("select project_name from tblproject where project_id='" & rs.Item("project_id") & "'", con) & "<br>")
                        Dim ris As String


                        If spl(0) = fm.getinfo2("select project_name from tblproject where project_id='" & rs.Item("project_id") & "'", con) Then
                            ris = fm.getinfo2("select id from emp_resign where emptid=" & rs.Item("emptid") & " and resign_date between '" & d1 & "' and '" & d1.AddDays(nod - 1) & "'", con)
                            If IsNumeric(ris) Then

                                rt(0) &= cc.ToString & ". " & fm.getfullname(rs.Item("emp_id"), con) & " Has Terminated in this Month<br>"
                                cc += 1
                            Else
                                transproj = fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid=" & rs.Item("emptid") & " and  (date_from between '" & d1.ToShortDateString & "' and '" & d1.AddDays(nod - 1) & "')", con) & "'", con)
                                If transproj <> "None" Then
                                    rt(0) &= cc.ToString & ". " & fm.getfullname(rs.Item("emp_id"), con) & " has transfer from " & proj & " to " & transproj & "<br>"
                                    cc += 1
                                End If
                            End If
                        End If
                    End While
                End If
                rs.Close()
            End If
            dbx = Nothing
            fm = Nothing
            Return rt(0)
        End Function

        Public Function projtransid(ByVal empid As String, ByVal d1 As Date, ByVal con As SqlConnection, ByVal pathx As String) As Object
            Dim rt As String

            Dim fm As New formMaker
            Dim dx As New dbclass
            Dim sql As String
            Dim transproj As String
            Dim rptd(2) As String
            Dim nod As Integer
            Dim cc As Integer = 1
            'Dim emppid As String
            rptd(0) = ""
            rptd(1) = ""
            nod = Date.DaysInMonth((d1.Year), (d1.Month))
            ' d1 = d1.Month & "/1/" & d1.Year
            Dim proj As String = ""
            transproj = fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid=" & empid & " and  date_from ='" & d1.ToShortDateString & "'", con) & "'", con)
            proj = fm.getinfo2("select project_name from tblproject where project_id='" & fm.getinfo2("select project_id from emp_job_assign where emptid=" & empid & " and  (date_end = '" & d1.AddDays(-1).ToShortDateString & "')", con) & "'", con)
            rt = "<td>"
            If proj = "None" Then
                sql = "select id from rptdataupdate where Report LIKE 'computer id. " & empid & " project end date is not entered correctly!'"
                Dim datae As String = ""
                datae = fm.getinfo2(sql, con)
                rt &= datae
                If IsNumeric(datae) = False Then
                    If datae = "None" Then
                        sql = "Begin Transaction" & Chr(13) & "insert into rptdataupdate(reporttype,report,datee,seen,reportto) values('project_assign','computer id. " & empid & " project end date is not entered correctly!','" & Today.AddDays(30) & _
                            "',0,'1;3')" & Chr(13) & "COMMIT"

                        Try
                            Dim flg As Object
                            flg = dx.excutes(sql, con, pathx)
                            rptd(1) &= sql & flg.ToString
                        Catch ex As Exception
                            rptd(1) &= empid & "-transpfer-" & ex.ToString
                            exception_hand(ex, "Master page Erro")
                        End Try
                    End If
                End If
                rt &= "Error on date</td><td>" & transproj & "<td>"
            Else
                rt &= proj & "</td><td>" & transproj & "<td>"
            End If
            rptd(0) = rt
            fm = Nothing
            Return rptd
        End Function
        Function istransfer(ByVal emptid As Integer, ByVal d1 As Date, ByVal con As SqlConnection) As Boolean
            Dim frm, hrd As String
            frm = getinfo2("select date_from from emp_job_assign where '" & d1.Month & "'=month(date_from) and '" & d1.Year & "'=year(date_from) and emptid=" & emptid, con)
            hrd = getinfo2("select hire_date from emprec where '" & d1.Month & "'=month(date_from) and '" & d1.Year & "'=year(date_from) and emptid=" & emptid, con)
           
            If IsDate(frm) Then
                If IsDate(hrd) Then
                    If CDate(hrd).Subtract(CDate(frm)).Days <> 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function
        Function whrtrans(ByVal emptid As Integer, ByVal d1 As Date, ByVal con As SqlConnection) As Object
            Dim frm As String
            If istransfer(emptid, d1, con) Then
                frm = getinfo2("select project_id from emp_job_assign where '" & d1.Month & "'=month(date_from) and '" & d1.Year & "'=year(date_from) and emptid=" & emptid, con)
                Return frm
            Else
                Return "None"
            End If

        End Function
        Function isResign(ByVal emptid As Integer, ByVal con As SqlConnection) As Array
            Dim rtn(2) As String
            Dim did As Object
            did = getinfo2("select resign_date from emp_resign where emptid='" & emptid & "'", con)
            If IsDate(did) Then
                rtn(0) = Me.getinfo2("select active from emprec where id=" & emptid, con)
                rtn(1) = Me.getinfo2("select end_date from emprec where id=" & emptid & " and active='n'", con)
            Else
                rtn(0) = "None"
                rtn(1) = "None"
            End If
            Return rtn
        End Function

        Public Function getprojemp_no(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection) As Array
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            Dim fm As New formMaker
            Dim did As String
            rs = dbs.dtmake("listemp", "select emptid,date_from,date_end from emp_job_assign where project_id='" & projid & "' order by id desc", con)
            Dim d1, d2 As Date

            Dim rtn(2) As String
            rtn(0) = ""
            rtn(1) = ""
            Dim count As Integer = 0
            If rs.HasRows Then

                Try
                    While rs.Read

                        If rs.IsDBNull(2) Then
                            d2 = sdate
                            d1 = rs.Item("date_from")

                        Else
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)
                            If IsDate(did) Then
                                d2 = did
                                d1 = rs.Item("date_from")
                            Else
                                d2 = rs.Item("date_end")
                                d1 = rs.Item("date_from")
                            End If

                        End If
                        ' Response.Write(sdate.Subtract(d1).Days.ToString & "===>")

                        If sdate.Subtract(d1).Days >= 0 And sdate.Subtract(d2).Days <= 0 Then
                            'Response.Write(sdate.Subtract(d1).Days.ToString & "===>")
                            'Response.Write(rs.Item("emptid") & "<br>")
                            Dim rtnx() As String
                            rtnx = isResign(rs.Item("emptid"), con)
                            Dim pass As String = ""
                            If IsDate(rtnx(1)) = True Then
                                If d2.Month = CDate(rtnx(1)).Month And d2.Year = CDate(rtnx(1)).Year Then
                                    pass = "y"
                                Else
                                    pass = "n"
                                End If
                            End If
                            If rtnx(0) = "y" Or pass = "y" Then

                                count = count + 1
                                rtn(0) &= "'" & rs.Item("emptid") & "',"
                            End If

                            'Response.Write(rtn)
                        End If

                    End While
                Catch ex As Exception
                    rtn(0) = "'',"
                    count = 0
                    exception_hand(ex, "Master page Erro")
                End Try

            End If
            If rtn(0) = "" Then
                rtn(0) = "'',"
                count = 0
            End If
            rs.Close()
            dbs = Nothing
            rtn(0) = rtn(0).Substring(0, rtn(0).Length - 1)
            Dim sp() As String = rtn(0).Split(",")
            'Response.Write(sp.Length)
            rtn(1) = count.ToString

            Return rtn
        End Function
        Public Function getprojemp(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection) As Object

            Dim dbs As New dbclass
            Dim rs As DataTableReader
            Dim fm As New formMaker
            Dim did As String
            Dim d1, d2, de, ds, pdate1 As Date
            d1 = Nothing
            d2 = Nothing
            pdate1 = sdate.Month & "/1/" & sdate.Year
            rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' order by emp_id,emptid desc", con)

            Dim rtn As String = ""
            If rs.HasRows Then

                Try
                    'Response.Write(" start    ====     requested     ====    Date end<br>")
                    While rs.Read
                        ds = "#1/1/1900#"
                        de = "#1/1/1900#"
                        ds = rs.Item("date_from")
                        If rs.IsDBNull(3) Then
                            'ds = sdate
                            ds = rs.Item("date_from")
                            de = sdate
                        Else
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)
                            If IsDate(did) Then
                                If CDate(did) <> rs.Item("date_end") Then
                                    de = rs.Item("date_end")
                                Else

                                    If CDate(did).Month = sdate.Month And CDate(did).Year = sdate.Year Then
                                        de = sdate
                                    Else
                                        ' Response.Write(did.ToString & ">.........")
                                        de = CDate(did)
                                    End If



                                End If


                            Else

                                de = rs.Item("date_end")
                            End If
                        End If

                        '  Response.Write(ds.ToShortDateString)
                        ' Response.Write("     ====     ")
                        'Response.Write(sdate.ToShortDateString)
                        'Response.Write("     ====      ")

                        '                    Response.Write(de.ToShortDateString)
                        '                   Response.Write("  ====         " & rs.Item("emptid") & fm.getfullname(rs.Item("emp_id"), con) & "<br>")
                        If ds <= sdate And sdate <= de Then
                            rtn &= "'" & rs.Item("emptid") & "',"

                        Else
                            If IsDate(did) Then
                                If CDate(did) <= sdate And pdate1 < CDate(did) Then
                                    rtn &= "'" & rs.Item("emptid") & "',"
                                End If

                            End If
                        End If


                    End While
                Catch ex As Exception
                    '              Response.Write(ex.ToString)
                    rtn = "'',"
                    fm.exception_hand(ex, "master page")
                End Try

            End If
            If rtn = "" Then
                rtn = "'',"
            End If
            rs.Close()
            dbs = Nothing
            rtn &= "''"
            Dim sp() As String = rtn.Split(",")
            'Response.Write(sp.Length)

            Return rtn
        End Function
        Public Function getprojempxxx(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection) As String
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            Dim fm As New formMaker
            Dim did As String
            rs = dbs.dtmake("listemp", "select emptid,date_from,date_end from emp_job_assign where project_id='" & projid & "' order by id desc", con)
            Dim d1, d2 As Date

            Dim rtn As String = ""
            If rs.HasRows Then

                Try
                    While rs.Read

                        If rs.IsDBNull(2) Then
                            d2 = sdate
                            d1 = rs.Item("date_from")

                        Else
                            did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)
                            If IsDate(did) Then
                                d2 = sdate
                                d1 = rs.Item("date_from")
                            Else
                                d2 = rs.Item("date_end")
                                d1 = rs.Item("date_from")
                            End If

                        End If
                        ' Response.Write(sdate.Subtract(d1).Days.ToString & "===>")

                        If sdate.Subtract(d1).Days >= 0 And sdate.Subtract(d2).Days <= 0 Then
                            'Response.Write(sdate.Subtract(d1).Days.ToString & "===>")
                            'Response.Write(rs.Item("emptid") & "<br>")
                            Dim rtnx() As String
                            rtnx = isResign(rs.Item("emptid"), con)
                            Dim pass As String = ""
                            If IsDate(rtnx(1)) = True Then
                                If d2.Month = CDate(rtnx(1)).Month And d2.Year = CDate(rtnx(1)).Year Then
                                    pass = "y"
                                Else
                                    pass = "n"
                                End If
                            End If
                            If rtnx(0) = "y" Or pass = "y" Then

                                ' Count = Count + 1
                                rtn &= "'" & rs.Item("emptid") & "',"
                            End If
                            ' rtn &= "'" & rs.Item("emptid") & "',"
                            'Response.Write(rtn)
                        End If

                    End While
                Catch ex As Exception
                    rtn = "'',"
                    exception_hand(ex, "master page Erro")
                End Try

            End If
            If rtn = "" Then
                rtn = "'',"
            End If
            rs.Close()
            dbs = Nothing
            rtn = rtn.Substring(0, rtn.Length - 1)
            Dim sp() As String = rtn.Split(",")
            'Response.Write(sp.Length)
            Return rtn
        End Function
        Public Function getallowance(ByVal emptid As String, ByVal altype As String, ByVal pdate1 As Date, ByVal con As SqlConnection)
            Dim nod As Integer
            Dim rtn(2) As String
            Dim rpt As String = ""
            Dim d2 As Date
            Dim amt As Double
            nod = Date.DaysInMonth(pdate1.Year, pdate1.Month)
            d2 = pdate1.Month & "/" & nod.ToString & "/" & pdate1.Year
            Dim dvar1, dvar2 As Integer
            Dim rt As String = ""
            Dim rs As DataTableReader
            Dim dbx As New dbclass
            amt = 0
            rs = dbx.dtmake(altype, "select * from emp_alloance_rec where to_date between '" & pdate1 & "' and '" & d2 & "' and istaxable='" & altype & "' and emptid='" & emptid & "'", con)
            If rs.HasRows Then
                'rpt &= "has rows"
                While rs.Read
                    dvar1 = rs.Item("to_date").Subtract(pdate1).days + 1
                    ' rpt &= "has rows" & dvar1.ToString & "<br>"
                    If dvar1 > 0 Then
                        rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allownace_type") & " has been closed on:" & rs.Item("to_date") & " and emptid='" & emptid & "'<br>"
                        amt += CDbl(rs.Item("amount")) / nod * dvar1
                    End If
                End While
            End If
            rs.Close()
            rs = dbx.dtmake(altype, "select * from emp_alloance_rec where to_date is null and from_date<='" & pdate1 & "' and  (emptid=" & emptid & ") and (istaxable='" & altype & "')", con)
            If rs.HasRows Then
                While rs.Read
                    If CDate(rs.Item("from_date")).Month = pdate1.Month And CDate(rs.Item("from_date")).Year = pdate1.Year Then
                        dvar1 = CDate(rs.Item("from_date")).Subtract(pdate1).Days
                        rpt &= rs.Item("id").ToString & " Allowance type:" & rs.Item("allowance_type") & " has been started on:" & rs.Item("to_date") & " and emptid='" & emptid & "'<br>"

                    Else
                        dvar1 = nod
                    End If

                    If dvar1 > 0 Then
                        amt += CDbl(rs.Item("amount")) / nod * dvar1
                    End If
                End While
            End If
            rs.Close()
            dbx = Nothing
            rtn(0) = amt
            rtn(1) = rpt
            Return rtn
        End Function
        Public Function otcalc(ByVal scale As String, ByVal otdate As String, ByVal d1 As Date, ByVal d2 As Date, ByVal emptid As Integer, ByVal paidst As String, ByVal con As SqlConnection) As Array
            Dim rt(2), hr As String
            Dim sumreg As Double = 0
            hr = ""
            rt(0) = ""
            rt(1) = ""
            Dim reg As String = ""
            reg = Me.getinfo2("select sum(time_diff) from emp_ot where " & otdate & " between '" & d1 & "' and '" & d2 & "' and emptid=" & emptid & " and factored='" & scale & "'  and paidstatus='" & paidst & "'", con)


            If reg.ToString <> "" And reg.ToString <> "None" Then
                rt(0) = reg
                hr = Me.getinfo2("select rate from emp_ot where " & otdate & " between '" & d1 & "' and '" & d2 & "' and emptid=" & emptid & " and factored='" & scale & "'  and paidstatus='" & paidst & "'", con)
                If hr = "None" Then
                    hr = "1"
                End If

                sumreg = (CDbl(reg) * CDbl(hr)).ToString
                '  Response.Write(sumreg & "=" & reg & "*" & hr & "<br>")
            Else
                sumreg = "0"
            End If
            rt(1) = sumreg

            Return rt

        End Function
        Public Function otcalc(ByVal scale As String, ByVal emptid As Integer, ByVal ref As String, ByVal con As SqlConnection) As Array
            Dim rt(2), hr As String
            Dim sumreg As Double = 0
            hr = ""
            rt(0) = ""
            rt(1) = ""
            Dim reg As String = ""
            reg = Me.getinfo2("select sum(time_diff) from emp_ot where  emptid=" & emptid & " and factored='" & scale & "'  and ref='" & ref & "'", con)

            If reg.ToString <> "" And reg.ToString <> "None" Then
                rt(0) = reg
                hr = Me.getinfo2("select rate from emp_ot where emptid=" & emptid & " and factored='" & scale & "'  and ref='" & ref & "'", con)
                If hr = "None" Then
                    hr = "1"
                End If

                sumreg = (CDbl(reg) * CDbl(hr)).ToString
                '  Response.Write(sumreg & "=" & reg & "*" & hr & "<br>")
            Else
                sumreg = "0"
            End If
            rt(1) = sumreg

            Return rt

        End Function
        Public Function getproj(ByVal emptid As Integer, ByVal pd1 As Date, ByVal pd2 As Date, ByVal con As SqlConnection)
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            Dim rt(2) As String
            Dim hrd As String
            hrd = getinfo2("select hire_date from emprec where id=" & emptid, con)
            rs = dbs.dtmake("dbsproj", "select project_id,date_from,date_end from emp_job_assign where emptid=" & emptid & " and ('" & pd1 & "' between date_from and isnull(date_end,'" & pd2 & "') or (month(date_from)='" & CDate(hrd).Month & "' and year(date_from)='" & CDate(hrd).Year & "')) order by date_from", con)
            If rs.HasRows Then
                While rs.Read()

                    ' Response.Write("<br>" & emptid.ToString & "===" & rs.Item("date_from"))

                    rt(0) = rs.Item("project_id")
                    rt(1) = dbs.getprojectname(rt(0), con)

                End While

            End If
            rs.Close()
            dbs = Nothing
            Return rt
        End Function
        Public Function getproj_on_date(ByVal emptid As Integer, ByVal pd1 As Date, ByVal con As SqlConnection)
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            Dim rt(3) As String
            rt(0) = ""
            rt(1) = ""
            rt(2) = ""
            Dim hrd As String = getinfo2("select hire_date from emprec where id=" & emptid, con)

            Dim sql As String = "select project_id,date_from,date_end from emp_job_assign where " & _
                                "emptid=" & emptid & " and ('" & pd1 & "' between date_from and isnull(date_end,'" & pd1.AddDays(Date.DaysInMonth(pd1.Year, pd1.Month) - 1) & "') or (month(date_from)='" & CDate(hrd).Month & "' and year(date_from)='" & CDate(hrd).Year & "')) order by date_from"

            Try




                rs = dbs.dtmake("dbsprojx", sql, con)
                If rs.HasRows Then

                    While rs.Read()

                        ' Response.Write("<br>" & emptid.ToString & "===" & rs.Item("date_from"))

                        rt(0) = rs.Item("project_id")
                        rt(1) = dbs.getprojectname(rt(0), con)
                        rt(2) &= rt(1) & ","
                    End While

                End If
            Catch ex As Exception
                rt(2) = "<br>errorrrrrrr: masterr 661 <br> " & ex.ToString
                exception_hand(ex, "master page Erro")
                Return rt
            End Try
            rt(2) &= "<br> " & Sql & "--------------------------------------------------------------------------<br>"
            rs.Close()
            dbs = Nothing
            Return rt
        End Function
        Public Function lvb(ByVal emptid As String, ByVal isexp As Boolean, ByVal paids As Integer, ByVal fired As String, ByVal con As SqlConnection)
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            Dim addav As Double = 0
            If paids = 0 Then
                rs = dbs.dtmake("leave", "select * from show_leave_bal where emptid=" & emptid & " and id Not in(select bgtid from leav_settled where emptid=" & emptid & ") order by 'year end'", con)
            ElseIf paids = 1 Then
                rs = dbs.dtmake("leave", "select * from show_leave_bal where emptid=" & emptid & " and id in(select bgtid from leav_settled where emptid=" & emptid & ") order by 'year end'", con)
            Else
                rs = dbs.dtmake("leave", "select * from show_leave_bal where emptid=" & emptid & " order by 'year end'", con)

            End If
            Select Case isexp
                Case True
                    If rs.HasRows Then
                        While rs.Read
                            If Me.isexp(rs.Item("Year Start"), rs.Item("Year End"), 2, "y") Then
                                If fired = "" Then
                                    addav += CDbl(Me.showavdate(rs.Item("Year Start"), rs.Item("Year End"), rs.Item("Budget"))) - CDbl(rs.Item("Used"))
                                Else
                                    addav += CDbl(Me.showavdate(rs.Item("Year Start"), rs.Item("Year End"), fired, rs.Item("Budget"))) - CDbl(rs.Item("Used"))

                                End If
                            End If
                        End While
                    End If

                Case False
                    If rs.HasRows Then
                        While rs.Read
                            If Me.isexp(rs.Item("Year Start"), rs.Item("Year End"), 2, "y") = False Then
                                If fired = "" Then
                                    addav += CDbl(Me.showavdate(rs.Item("Year Start"), rs.Item("Year End"), rs.Item("Budget"))) - CDbl(rs.Item("Used"))
                                Else
                                    addav += CDbl(Me.showavdate(rs.Item("Year Start"), rs.Item("Year End"), fired, rs.Item("Budget"))) - CDbl(rs.Item("Used"))

                                End If
                            End If
                        End While
                    End If
            End Select
            Return addav
        End Function
        Public Function getnow()
            Return Now.Month.ToString & "/" & Now.Day.ToString & "/" & Now.Year.ToString & " " & Now.ToLongTimeString
        End Function
        Public Function isinproject(ByVal emptid As String, ByVal fdate As Date, ByVal con As SqlConnection)
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            rs = dbs.dtmake("vwisproj", "select * from emp_job_assign where emptid=" & emptid & " and '" & fdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "') or date_from  between '" & fdate & "' and isnull(date_end,'" & Today.ToShortDateString & "')", con)
            If rs.HasRows = True Then
                rs.Read()
                Return rs.Item("project_id")
            Else
                Return "None"
            End If
        End Function
        Function getendmonth(ByVal df As Date)
            Dim nod As Integer
            nod = Date.DaysInMonth(df.Year, df.Month)
            Return df.Month.ToString & "/" & nod.ToString & "/" & df.Year.ToString
        End Function
        Public Function isinproject2(ByVal emptid As String, ByVal fdate As Date, ByVal con As SqlConnection)
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            rs = dbs.dtmake("vwisproj", "select * from emp_job_assign where emptid=" & emptid & " and '" & fdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "') or date_from  between '" & fdate & "' and isnull(date_end,'" & Today.ToShortDateString & "')  order by date_from", con)
            If rs.HasRows = True Then
                rs.Read()
                Return rs.Item("project_id")
            Else
                Return "None"
            End If
        End Function
        Public Function signpartx()
            Dim outp As String = ""
            outp &= "<tr class='sss'><td style='height:10px;border-style:none none;' colspan='17'>&nbsp;</td></tr>"
            outp &= "<tr class='sss1' >"

            outp &= "<td class='l2'  colspan='3'>________________<br>Prepared&nbsp; By</td>"

            outp &= "<td class='l3' colspan='3'>________________<br>Checked By</td>"

            outp &= "<td class='l4' colspan='3'>________________<br>Verified By</td>"

            outp &= " <td class='l5' colspan='3'>________________<br>Approved By</td>"

            outp &= "</tr>" & Chr(13)



            outp &= "<tr class='sss1'>"


            outp &= "<td class='l2' colspan='3'>________________<br>Date</td>"

            outp &= "<td class='l3' colspan='3'>________________<br>Date</td>"

            outp &= "<td class='l4' colspan='3'>________________<br>Date</td>"

            outp &= "<td class='l5' colspan='3'>________________<br>Date</td>"
            outp &= "</tr>" & Chr(13)

            Return outp
        End Function
        Public Function signpart()
            Dim outp As String = ""
            outp &= "<tr class='sss'><td style='height:10px;border-style:none none;' colspan='17'>&nbsp;</td></tr>"
            outp &= "<tr class='sss1' >"

            outp &= "<td class='l2'  colspan='3'>________________<br>Prepared&nbsp; By</td>"

            outp &= "<td class='l3' colspan='3'>________________<br>Checked By</td>"

            outp &= "<td class='l4' colspan='3'><br></td>"

            outp &= " <td class='l5' colspan='3'>________________<br>Approved By</td>"

            outp &= "</tr>" & Chr(13)



            outp &= "<tr class='sss1'>"


            outp &= "<td class='l2' colspan='3'>________________<br>Date</td>"

            outp &= "<td class='l3' colspan='3'>________________<br>Date</td>"

            outp &= "<td class='l4' colspan='3'><br></td>"

            outp &= "<td class='l5' colspan='3'>________________<br>Date</td>"
            outp &= "</tr>" & Chr(13)

            Return outp
        End Function
        Public Function signpart2()
            Dim outp As String = ""
            outp &= "<tr class='sss'><td style='height:50px;border-style:none none;' colspan='17'>&nbsp;</td></tr>"
            outp &= "<tr class='sss1' >"


            outp &= "<td class='l2'  colspan='3'>________________</td>"

            outp &= "<td class='l3' colspan='3'>________________</td>"

            outp &= "<td class='l4' colspan='3'></td>"

            outp &= " <td class='l5' colspan='2'>________________</td>"

            outp &= "</tr>" & Chr(13)
            outp &= "<tr>"



            outp &= "<td class='l2' colspan='3'>Prepared&nbsp; By</td>"

            outp &= "<td class='l3' colspan='3'>Checked By</td>"

            outp &= "<td class='l4' colspan='3'></td>"

            outp &= "<td class='l5' colspan='2'>Approved By</td>"
            outp &= " </tr>" & Chr(13)
            outp &= "<tr class='sss'><td style='border-style:none none;' >&nbsp;</td></tr>" & Chr(13)

            outp &= "<tr class='sss1'>"


            outp &= "<td class='l2' colspan='3'>________________</td>"

            outp &= "<td class='l3' colspan='3'>________________</td>"

            outp &= "<td class='l4' colspan='3'></td>"

            outp &= "<td class='l5' colspan='2'>________________</td>"
            outp &= "</tr>" & Chr(13)
            outp &= " <tr>"


            outp &= "<td class='l2' colspan='3'>Date</td>"

            outp &= "<td class='l3' colspan='3'>Date</td>"

            outp &= " <td class='l4' colspan='3'></td>"

            outp &= " <td class='l5' colspan='2'>Date</td>"
            outp &= "</tr>"
            Return outp
        End Function

        Public Function ispension(ByVal empid As Integer, ByVal date1 As Date, ByVal con As SqlConnection) As Boolean
            ' Dim rt As Boolean
            Dim info As String
            info = Me.getinfo2("select emptid from emp_pen_start where emptid=" & empid & " and penstart<='" & date1 & "' order by id desc", con)
            If info <> "None" Then
                Return True

            End If
            Return False
        End Function
        Public Function getsal(ByVal emp_id As String, ByVal date1 As Date, ByVal con As SqlConnection) As String
            Dim sal As String
            sal = Me.getinfo2("select basic_salary from emp_sal_info where emp_id='" & emp_id & "' and date_start<='" & date1 & "' order by id desc", con)
            If sal <> "None" Then
                Return sal
            Else
                Return 0
            End If
        End Function
        Function getempid(ByVal fullname As String, ByVal con As SqlConnection)
            Dim empid, emptid As String
            Dim dbs As New dbclass

            Dim spl() As String
            Dim rtn(3) As String
            rtn(0) = ""
            rtn(1) = ""
            rtn(2) = ""
            Dim sql As String
            spl = fullname.Split(" ")
            If spl.Length > 1 Then
                sql = "Select emp_id from emp_static_info where first_name='" & spl(0) & "' and middle_name='" & spl(1) & "' and last_name='" & spl(2) & "'"
                empid = Me.getinfo2(sql, con)
                emptid = Me.getinfo2("select id from emprec where emp_id='" & empid & "' order by hire_date desc", con)
                rtn(0) = empid
                rtn(1) = emptid
                rtn(2) = sql

            End If
            Return rtn
        End Function

        Public Function getids(ByVal x As HttpRequest, ByVal ftr As String)
            Dim idarr() As String = {"none"}
            Dim spl() As String
            Dim con As String = ""
            Dim i As Integer = 0
            Dim arrkey() As String = x.Form(ftr).Split("&")
            For i = 0 To arrkey.Length - 1
                'Response.Write(arrkey(i) & "<br>")
                spl = arrkey(i).Split("=")(0).Split("_")
                If con <> spl(0).Trim Then
                    If spl.Length > 1 Then
                        If spl(0).Trim(" ") <> "" Then
                            ReDim Preserve idarr(i + 1)
                            idarr(i) = spl(0).ToString
                            con = spl(0)
                            i += 1
                        End If
                    End If
                End If
            Next

            Return idarr
        End Function
        Public Function headerjq(ByVal v As Integer)
            Dim str As String = ""
            If v = 7 Then
                str = "<script language='javascript' type='text/javascript' src='jq/jquery-1.7.2.js'></script>" & Chr(13) & _
    "<link rel='stylesheet' href='jq/themes/ui-lightness/jquery.ui.all.css' />" & Chr(13) & _
    "<script type='text/javascript' src='jq/ui/jquery.ui.core.js'></script>" & Chr(13) & _
    "<script type='text/javascript' src='jq/ui/jquery.ui.widget.js'></script>" & Chr(13) & _
    "	<script type='text/javascript'  src='jq/ui/jquery.ui.mouse.js'></script>" & Chr(13) & _
    "	<script type='text/javascript' src='jq/ui/jquery.ui.draggable.js'></script>" & Chr(13) & _
    "	<script type='text/javascript' src='scripts/form.js'></script>" & Chr(13) & _
    "	<script type='text/javascript' src='jq/ui/jquery.ui.position.js'></script>" & Chr(13) & _
    "	<script type='text/javascript' src='jq/ui/jquery.ui.resizable.js'></script>" & Chr(13) & _
    "	<!--script type='text/javascript' src='jq/ui/jquery.ui.button.js'></script-->" & Chr(13) & _
    "<script type='text/javascript' src='jq/ui/jquery.ui.dialog.js'></script>" & Chr(13) & _
    "	<script type='text/javascript' src='jq/ui/jquery.ui.datepicker.js'></script>" & Chr(13) & _
    "<script type='text/javascript' src='jq/ui/jquery.ui.autocomplete.js'></script>"
            ElseIf v = 9 Then
                str = "<link rel='stylesheet' href='jqq/themes/base/jquery.ui.all.css'>" & Chr(13) & _
                "<link rel='stylesheet' href='jq/themes/ui-lightness/jquery.ui.all.css' />" & Chr(13) & _
                 "<script src='jqq/jquery-1.9.1.js'></script>" & Chr(13) & _
              " <script src='jqq/ui/jquery.ui.core.js'></script> " & Chr(13) & _
 "<script src='jqq/ui/jquery.ui.position.js'></script>" & Chr(13) & _
 "<script src='jqq/ui/jquery.ui.menu.js'></script>" & Chr(13) & _
 "<script src='jqq/ui/jquery.ui.draggable.js'></script>" & Chr(13) & _
                "	<script type='text/javascript' src='jqq/ui/jquery.ui.autocomplete.js'></script>" & Chr(13) & _
                " <script type='text/javascript' src='jqq/ui/jquery.ui.datepicker.js'></script>" & _
                "	<link rel='stylesheet' href='../demos.css'>"

            End If


            Return str
        End Function
        Public Function headerkirsoft()
            Dim str As String = ""
            str = "<link rel='stylesheet' type='text/css' href='css/kir.login.css' /> " & Chr(13) & _
"<script src='scripts/kirsoft.java.js' type='text/javascript'></script>" & Chr(13) & _
"<script src='scripts/script.js' type='text/javascript'></script>"


            Return str
        End Function
        Public Function headerkirsoftreq()

            Return "<script src='scripts/kirsoft.required.js' type='text/javascript'></script>"
        End Function
        Public Function numdigit(ByVal num As String, ByVal dig As Integer)
            Dim spl(2) As String
            Dim rtv As String = "."
            If IsNumeric(num) Then
                Return FormatNumber(num, dig, TriState.True, TriState.True, TriState.True)
            Else
                Return num
            End If
        End Function
        Public Function loan(ByVal empid As Integer, ByVal con As SqlConnection)
            Dim dbx As New dbclass
            Dim tloan, subt, tbal, subbal, sett As Double
            tloan = 0
            subt = 0
            tbal = 0
            subbal = 0
            sett = 0
            Dim rs, rs2 As DataTableReader
            Dim ret As String = ""
            rs = dbx.dtmake("inloan", "select * from emp_loan_req where emptid=" & empid, con)
            If rs.HasRows Then
                ret = "<style>td {padding-right:20px;}tr{ padding-right:20px;font-size:12pt;}</style>"
                ret &= "<table width='700px' cellspacing='0' cellpadding='0' border=1 style='border-color:blue;'>"

                While rs.Read
                    ret &= "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
                    ret &= "<td style='padding-right:20px;'><strong> Date start</strong></td><td style='padding-right:20px;'>:</td><td style='padding-right:20px;'>" & rs.Item("deduction_starts") & "</td>"
                    ret &= "<td><strong> Loan Type </strong></td><td>:</td><td> " & rs.Item("reason") & "</td>" & _
                    "<td><strong>Loan Date</strong></td><td>:</td><td>" & rs.Item("loan_date") & "</td>" & _
                    "<td><strong>Amount</strong></td><td>:</td><td>" & rs.Item("amt") & "</td>"
                    ret &= "</tr>"
                    rs2 = dbx.dtmake("innerloan", "select * from emp_loan_settlement where loan_no=" & rs.Item("id"), con)
                    subt = rs.Item("amt")
                    If rs2.HasRows Then
                        ret &= "<tr><td colspan='12' style='padding-right:20px;'><table width='500px'>"
                        ret &= "<tr><td>Settlement</td></tr><tr>"
                        ret &= "<td style='padding-right:20px;'>Ref.</td><td>Date</td><td>amount</td></tr>"
                        sett = 0
                        While rs2.Read
                            ret &= "<tr>"
                            ret &= "<td>" & rs2.Item("ref") & "</td><td>" & rs2.Item("date_payment") & "</td><td>" & rs2.Item("amount") & "</td></tr>"
                            sett += CDbl(rs2.Item("amount"))
                        End While
                        ret &= "<tr><td colspan='3'><strong>Total Settlement:</strong></td><td>" & FormatNumber(sett, 2, TriState.True, TriState.True, TriState.True) & "</td></tr>" & _
                        "<tr><td colspan='3'><strong>Balance:</strong></td><td>" & FormatNumber((subt - sett), 2, TriState.True, TriState.True, TriState.True) & "</td></tr></table></td></tr>"
                    End If


                End While
                ret &= "</table>"
            End If

            Return ret
        End Function
        Public Function pay_tax2(ByVal amt As Double)
            Dim tax As Double
            'tax = IF(amt>5000,(C2-5000)*35%+1087.5,IF(C2>3550,(C2-3550)*0.3+652.5,
            'IF(C2>2350,(C2-2350)*0.25+352.5,IF(C2>1400,(C2-1400)*0.2+162.5,IF(C2>650,(C2-650)*0.15+50,IF(C2>150,(C2-150)*0.1,0))))))

            If amt > 10833 Then
                tax = amt * 0.35 - 1487.55
            Else
                If amt > 7758 Then
                    tax = (amt) * 0.3 - 945.9
                Else
                    If amt > 5195 Then
                        tax = (amt) * 0.25 - 558
                    Else
                        If amt > 3145 Then
                            tax = (amt) * 0.2 - 295.25
                        Else
                            If amt > 1650 Then
                                tax = (amt) * 0.15 - 141
                            Else
                                If amt > 585 Then
                                    tax = (amt) * 0.1 - 58.5
                                Else
                                    tax = 0
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Return tax
        End Function
        Public Function pay_tax(ByVal amt As Double)
            Dim tax As Double
            'tax = IF(amt>5000,(C2-5000)*35%+1087.5,IF(C2>3550,(C2-3550)*0.3+652.5,
            'IF(C2>2350,(C2-2350)*0.25+352.5,IF(C2>1400,(C2-1400)*0.2+162.5,IF(C2>650,(C2-650)*0.15+50,IF(C2>150,(C2-150)*0.1,0))))))
           

            If amt > 5000 Then
                tax = amt * 0.35 - 662.5
            Else
                If amt > 3500 Then
                    tax = (amt) * 0.3 - 412.5
                Else
                    If amt > 2350 Then
                        tax = (amt) * 0.25 - 235
                    Else
                        If amt > 1400 Then
                            tax = (amt) * 0.2 - 117.5
                        Else
                            If amt > 650 Then
                                tax = (amt) * 0.15 - 47.5
                            Else
                                If amt > 150 Then
                                    tax = (amt) * 0.1 - 15
                                Else
                                    tax = 0
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Return tax
        End Function
        Function pay_tax(ByVal amt As Double, ByVal pd As Date, ByVal con As SqlConnection)
            Dim fm As New formMaker
            Dim l() As String
            Dim px() As String
            Dim rtn As String = ""
            Dim tax As Double = 0
            rtn = fm.getinfo2("select path from tbltax where pdate<='" & pd & "' order by pdate desc", con)
            If rtn <> "" Then
                '  Response.Write(amt & "xxxxxxxxxx" & rtn & "<br>")
                Try


                    l = File.ReadAllLines(rtn)

                    For i As Integer = 0 To l.Length - 1
                        'Response.Write(l(i) & "<br>")
                        px = l(i).Split(",")
                        If CDbl(amt) > CDbl(px(0)) Then
                            tax = CDbl(amt) * CDbl(px(2)) - px(1)
                            ' Response.Write(amt & "====" & px(2) & " ===" & px(1) & "<br>")

                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    exception_hand(ex, "master page Erro")
                    'Response.Write(ex.ToString)
                    ' Response.Write(rtn & "<br>")
                    For i As Integer = 0 To l.Length - 1
                        px = l(i).Split(",")
                        '  Response.Write(px(0) & "===" & px(1) & "====" & px(2) & "<br>")
                    Next
                    tax = 0
                End Try

            End If
            ' Response.Write("tax===" & tax & "<br>")
            Return tax



        End Function
        Public Function pension(ByVal amt As Double, ByVal rate As Double)
            Return amt * rate
        End Function

        Public Function getot(ByVal date1 As Date, ByVal date2 As Date, ByVal emptid As Integer, ByVal con As SqlConnection)
            Dim dbx As New dbclass
            Dim rtamt As Double
            Dim rs As DataTableReader
            rs = dbx.dtmake("selectdb", "select sum(amt) as amt from emp_ot where ot_date between '" & date1 & "' and '" & date2 & "' and emptid=" & emptid & " and (month(datepaid)=month(ot_date) and year(datepaid)=year(ot_date)) group by emptid", con)
            If rs.HasRows Then
                rs.Read()

                rtamt = rs.Item("amt")

            Else

                rtamt = 0
            End If
            rs.Close()
            Return rtamt
        End Function
        Public Function getotunp(ByVal date1 As Date, ByVal date2 As Date, ByVal emptid As Integer, ByVal con As SqlConnection)
            Dim dbx As New dbclass
            Dim rtamt As Double
            Dim rs As DataTableReader
            rs = dbx.dtmake("selectdb", "select sum(amt) as amt from emp_ot where ot_date between '" & date1 & "' and '" & date2 & "' and emptid=" & emptid & " and ref is null group by emptid", con)
            If rs.HasRows Then
                rs.Read()

                rtamt = rs.Item("amt")

            Else

                rtamt = 0
            End If
            rs.Close()
            Return rtamt
        End Function
        Public Function getotpaidin(ByVal date1 As Date, ByVal date2 As Date, ByVal emptid As Integer, ByVal con As SqlConnection)
            Dim dbx As New dbclass
            Dim rtamt As Double
            Dim rs As DataTableReader
            rs = dbx.dtmake("selectdb", "select sum(amt) as amt from emp_ot where ot_date between '" & date1 & "' and '" & date2 & "' and emptid=" & emptid & " and ref is Not null group by emptid", con)
            If rs.HasRows Then
                rs.Read()

                rtamt = rs.Item("amt")

            Else

                rtamt = 0
            End If
            rs.Close()
            Return rtamt
        End Function

        Public Function lwpinmonth(ByVal date1 As Date, ByVal date2 As Date, ByVal emptid As Integer, ByVal con As SqlConnection)
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            rs = dbx.dtmake("select db", "select * from emp_leave_take where leave_type='Leave without Pay' and emptid=" & emptid, con)
            Dim count As Double = 0
            If rs.HasRows Then
                Dim dtr, dtf As Date
                Dim nod, nnd As Double
                nod = Date.DaysInMonth(date2.Year, date2.Month)

                While rs.Read
                    dtr = rs.Item("date_return")
                    dtf = rs.Item("date_taken_from")
                    If dtr > date2 Then
                        nod = date2.Subtract(date1).Days
                    Else
                        nod = dtr.Subtract(date1).Days
                    End If
                    ' nod = rs.Item("no_days")
                    Dim ndat As Date
                    If date1 <= dtf Then
                        date1 = dtf

                    End If
                    For i As Integer = 0 To nod
                        ndat = date1.AddDays(i)

                        If ndat >= date1 And ndat <= date2 And dtr > ndat Then
                            count += 1
                        End If
                    Next

                End While
                rs.Close()
                Return count
            Else
                rs.Close()
                Return 0
            End If

        End Function
        Public Function makelist(ByVal con As SqlConnection, ByVal fields As String, ByVal tblname As String, ByVal sp As Integer, ByVal pageno As Integer, ByVal ordercol As String) As String
            Dim dt As DataTableReader
            Dim dc As New dbclass
            Dim thisb As String = ""
            Dim rtstr As String = "Data base is new! No Data Inside"
            Dim sql As String = "select id," & fields & " from " & tblname

            dt = dc.dtmake("NewQ" & Today.ToString, sql, con)

            If dt.HasRows = True Then
                Dim i As Integer = 0
                rtstr = ("<form id='frmemplist' method='post' action='empcontener.aspx' target='_parent'><input type='text' id='datatake' name='datatake' /><table cellspacing='0'>")
                Dim color As String = "#E3EAEB"
                Dim fc As Double
                fc = ((900 / dt.FieldCount) * 100) / 900

                While dt.Read
                    If i Mod 2 Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If

                    If i = 0 Then

                        rtstr &= "<span style='background-color:#7595f7; display:block; width:900px;' >"
                        For j As Integer = 1 To dt.FieldCount - 2
                            rtstr &= "<span style='width:" & fc & "%'>" & dt.GetName(j) & "</span>"
                        Next

                        rtstr &= "</span>"

                    End If
                    i = i + 1
                    rtstr &= "<span id='" & dt.Item(0) & "' style='background-color:" & color & "; cursor:pointer; width:900px;border:1px solid " & color & ";display:block;' onclick=" & _
                                      Chr(34) & "javascript:orderpass('" & dt.Item(0) & "');" & Chr(34) & " onmouseover=" & Chr(34) & _
                                      "javascript: this.style.background='#00ff00';" & Chr(34) & " onmouseout=" & Chr(34) & _
                                      "javascript: this.style.background='" & color & "';" & Chr(34) & "> "
                    For j As Integer = 1 To dt.FieldCount - 2
                        rtstr &= "<span style='width:" & fc & "%'>" & dt.Item(j) & "</span>"
                    Next
                    rtstr &= "</span>"
                End While
            End If
            rtstr &= ("</table></form><br />")
            dt.Close()
            dt = Nothing
            dc = Nothing
            Return rtstr

        End Function
        Function fileslistview(ByVal pathx As String, ByVal root As String) As String
            Dim loc As String = pathx
            'Dim f As Directory
            Dim up As New file_list
            Dim rtstr As String = ""
            If Directory.Exists(loc) = True Then

                Dim ext As String = ""
                Dim fname As String = ""
                'rtstr = "what  what..." & loc
                For Each d As String In Directory.GetDirectories(loc)
                    rtstr &= "<div title='" & d & "' style='display block; clear:both;'>"
                    Dim fld() As String
                    fld = d.Split("\")
                    rtstr &= fld(fld.Length - 1)
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
                        rtstr = rtstr & fn & "</span><br />" & _
               " <span><a href=" & Chr(34) & "javascript:del('" & ff & "','1st');" & Chr(34) & ">delete</a></span>&nbsp;&nbsp;&nbsp;<span>" & _
               "<a href=" & Chr(34) & root & "/" & fld(fld.Length - 1) & "/" & up.findfilename(k) & Chr(34) & ">View " & up.findfilename(k) & "</a></span>" & _
            "</span></div>" & _
            "<div style='width:15px; float:left;'>&nbsp;</div>"
                    Next
                    rtstr &= "</div><div style='clear:both'>"
                Next
            Else
                rtstr = "<div>file  doesnt found</div>"
            End If
            If rtstr = "" Then
                rtstr = "Empty"
            End If
            Return rtstr
        End Function
        Public Function showavdate(ByVal ds As Date, ByVal de As Date, ByVal v As Double) As Double
            Dim dif As Integer
            dif = de.Subtract(ds).Days
            Dim dt As Integer
            dt = Today.Subtract(ds).Days
            Dim str As Double = 0
            'str = dif.ToString & "===" & dt.ToString
            If (dt - dif) < 0 Then
                str = (dt * v) / dif
            Else
                str = v
            End If
            Return str
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
            'str = dif.ToString & "===" & dt.ToString
            If (dt - dif) <= 0 Then
                str = (dt * v) / dif
            Else
                str = v
            End If
            Return str
        End Function
        Public Function showavdate(ByVal ds As Date, ByVal de As Date, ByVal datec As Date, ByVal v As Double) As Double
            Dim dif As Integer
            dif = de.Subtract(ds).Days
            Dim dt As Integer
            dt = datec.Subtract(ds).Days
            Dim str As Double = 0
            'str = dif.ToString & "===" & dt.ToString
            If (dt - dif) < 0 Then
                str = (dt * v) / dif
            Else
                str = v
            End If
            Return str
        End Function
        Public Function isAbs(ByVal emptid As Integer, ByVal stm As Date, ByVal con As SqlConnection)
            Dim dbs As New dbclass
            Dim bol As Boolean = False
            Dim dt As DataTableReader
            dt = dbs.dtmake("attseek", "select * from emp_att where att_date= '" & stm & "' and emptid='" & emptid & "' and status='A'", con)
            If dt.HasRows Then
                bol = True
            End If
            dt.Close()
            dt = Nothing
            dbs = Nothing
            Return bol
        End Function
        Public Function leaveab(ByVal name As String)
            Select Case LCase(name)
                Case "annual leave"
                    Return "AL"
                Case "maternity leave"
                    Return "ML"
                Case "sick leave"
                    Return "SL"
                Case "marriage leave"
                    Return "MA"
                Case "leave without pay"
                    Return "lwp"
                    Return "lwp"
                Case "exam leave"
                    Return "EL"
                Case "other"
                    Return "O"
                Case Else
                    Return ""
            End Select
        End Function
        Public Function isOnleave(ByVal emptid As Integer, ByVal stm As Date, ByVal con As SqlConnection)
            Dim dbs As New dbclass
            Dim bol As Object = False
            Dim ret(3) As String
            Dim sql As String
            ret(0) = ""
            ret(1) = ""
            ret(2) = ""
            Dim dt As DataTableReader
            sql = "select * from emp_leave_take where date_taken_from <='" & stm & "' and date_return>='" & stm & "' and emptid='" & emptid & "' and approved_date is not null"
            dt = dbs.dtmake("attseek", sql, con)
            If dt.HasRows Then
                bol = True
                dt.Read()
                ret(1) = dt.Item("leave_type")
                If dt.Item("byhalfday") = "y" Then
                    ret(2) = "0.5"
                ElseIf (CDbl(dt.Item("no_days")) - CInt(dt.Item("no_days"))) < 1 Then
                    If stm.AddDays(1) = CDate(dt.Item("date_return")) Then
                        ret(2) = "0.5"
                    Else
                        ret(2) = "1"
                    End If

                Else
                    ret(2) = "1"
                End If
            Else
                ret(1) = sql
            End If
            ' ret(2) = sql
                dt.Close()
                dt = Nothing
                dbs = Nothing
                ret(0) = bol.ToString
                Return ret
        End Function
        Public Function calcwhen3(ByVal dtstart As Date, ByVal noday As Double, ByVal empid As String, ByVal con As SqlConnection, ByVal half As String)
            Dim hcount As Integer = 0
            Dim we As Integer = 0
            Dim wday As Integer = 0
            Dim dlist As String = ""
            Dim dateend As Date
            Dim makecal As String = "<tr>"
            dateend = dtstart
            Dim sysd As New datetimecal
            Dim i As Integer = 0
            Dim flg As Boolean
            If half = "y" Then
                noday = noday * 2
            End If
            Dim cnext As Date
            wday = 0
            Do
dot:
                dateend = dtstart.AddDays(i)
                If sysd.isPublic(dateend, con) = True Then
                    hcount += 1
                    makecal &= "<td style='background-color:green'>" & dateend.ToString("ddd") & "/" & dateend.ToString("dd") & "<td>"
                ElseIf sysd.isWeekEnd(dateend, empid, con).ToString = "True" Then
                    we += 1
                    makecal &= "<td style='background-color:blue'>" & dateend.ToString("ddd") & "/" & dateend.ToString("dd") & "<td>"

                Else
                    wday += 1
                    makecal &= "<td style='background-color:white'>" & dateend.ToString("ddd") & "/" & dateend.ToString("dd") & "<td>"

                End If
                If CInt(Math.Ceiling(noday)).ToString = wday.ToString Then

                    cnext = dtstart.AddDays(i + 1)
                    If sysd.isPublic(cnext, con) = True Then

                        hcount += 1
                        'Response.Write("found Public h day at the end<br>")
                        i += 1
                        GoTo dot
                    ElseIf sysd.isWeekEnd(cnext, empid, con).ToString = "True" Then
                        'Response.Write("found we at the end<br>")
                        we += 1
                        i += 1
                        GoTo dot
                    End If
                End If
                i += 1
                If i Mod 7 = 0 Then
                    makecal &= "</tr><tr>"
                End If
            Loop While Math.Round(noday, 0) > wday
            makecal &= "</tr>"

            'dateend = dtstart.AddDays(wday + hcount + we)
            ' dateend = dtstart.AddDays(wday)

            Dim intarr(7) As Object
            intarr(0) = dtstart
            intarr(4) = dateend
            intarr(1) = hcount
            intarr(2) = we
            intarr(3) = dlist

            intarr(5) = dateend.AddDays(1)
            intarr(6) = makecal
            Return intarr
        End Function
        Public Function calcwhen(ByVal dtstart As Date, ByVal noday As Double, ByVal empid As String, ByVal con As SqlConnection, ByVal half As String)
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
            While CDbl(noday) >= CDbl(wday)
                flg = False
                dateend = dtstart.AddDays(i)
                cnext = dtstart.AddDays(i + 1)
                If sysd.isPublic(dateend, con) = True Then
                    hcount += 1
                    dlist &= dateend.ToShortDateString.ToString & " Holiday day-" & (i + 1).ToString & "<br>"
                    flg = True
                ElseIf sysd.isWeekEnd(dateend, empid, con).ToString = "True" And flg = False Then
                    we += 1
                    dlist &= dateend.ToShortDateString & " weekEnd day-" & (i + 1).ToString & "<br>"
                    flg = True
                Else
                    wday += 1

                End If
                i = i + 1

            End While
            Dim rt() As String
            rt = getno_of_hw(dtstart, noday, empid, con)
            dateend = dtstart.AddDays(noday + CInt(rt(0)))

            'dateend = dtstart.AddDays(wday + hcount + we)
            ' dateend = dtstart.AddDays(wday)

            Dim intarr(6) As Object
            intarr(0) = dtstart
            intarr(4) = dateend.AddDays(-1)
            intarr(1) = hcount
            intarr(2) = we
            intarr(3) = dlist

            intarr(5) = dateend
            Return intarr
        End Function
        Public Function getno_of_hw(ByVal dtstart As Date, ByVal noday As Double, ByVal empid As String, ByVal con As SqlConnection)
            Dim nox As Double = 0
            Dim i As Double = 0
            Dim nday As Date
            'Dim dreturn As Date
            Dim wday As Integer = 0
            Dim sysd As New datetimecal
            Dim rt(2) As String
            Dim flg As Boolean = False
            'Dim addv As Double
            If (noday - CInt(noday) = 0.5) Then
                noday = Math.Ceiling(noday)

            End If
            While noday >= i
                flg = False
                nday = dtstart.AddDays(i)
                If sysd.isPublic(nday, con) = True Then
                    nox += 1
                    flg = True
                ElseIf sysd.isWeekEnd(nday, empid, con).ToString = "True" Then
                    nox += 1
                    flg = True
                Else
                    wday += 1

                End If
                If flg = True Then
                    If noday = i Then
                        noday += 1
                    End If

                Else
                    If noday = i Then
                        wday -= 1
                    End If

                End If

                i = i + 1
            End While
            rt(0) = nox
            rt(1) = wday

            Return rt
        End Function
        Public Function calcwhen2(ByVal dtstart As Date, ByVal noday As Double, ByVal empid As String, ByVal con As SqlConnection, ByVal half As String)
            Dim hcount As Integer = 0
            Dim we As Integer = 0
            Dim wday As Integer = 0
            Dim dlist As String = ""
            Dim dateend As Date
            dateend = dtstart.AddDays(noday)
            Dim intarr(6) As Object
            intarr(0) = dtstart
            intarr(4) = dateend.AddDays(1)
            intarr(1) = hcount
            intarr(2) = we
            intarr(3) = dlist

            intarr(5) = dateend
            Return intarr
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
                                        exception_hand(ex, "master page Erro")
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
                        output &= "<tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'><b> Available from this section:</b></td><td align='right'><b>" & Math.Round((ndav - used - lvst), 2).ToString & "</b></td></tr></table></tr>"

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
                exception_hand(ex, "master page Erro")
            End Try

            output &= "   </ul></div>"

            dbs = Nothing
            Return output
        End Function
        Public Function getleaveinfo2(ByVal emptid As Integer, ByVal con As SqlConnection)
            Dim output As String = ""
            output = "<div id=" & Chr(34) & "divleave" & Chr(34) & " > " & _
            "<ul id=" & Chr(34) & "me" & Chr(34) & " style=" & Chr(34) & "display:none; vertical-align:top; " & Chr(34) & "> "

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
            Try
                dt = dbs.dtmake("taxpayer", "select id,l_s_year,l_e_year,no_days_with_period from emp_leave_budget where emptid=" & emptid & " ORDER BY ID", con)


                If dt.HasRows Then
                    output &= "<table width='700' >" & _
                    "<tr><td><label>Year End</label></td>" & _
                    "<td style='text-align:right;'>Bugdeted</td></tr>"
                    tndav = 0
                    While dt.Read
                        used = 0
                        ndav = 0
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
                                ndav = fm.showavdate(dt.Item("l_s_year"), dt.Item("l_e_year"), dt.Item("no_days_with_period"))
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
                                If Me.getinfo2("select byhalfday from emp_leave_take where id='" & dt2.Item("req_id") & "'", con) = "y" Then
                                    output &= "by half days, Most likely day * 2"
                                End If
                                output &= "&nbsp;</td>"
                                output &= "</tr>"

                            End While

                            output &= "<tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Days taken: </td><td align='left'>" & used.ToString & _
                            "</td></tr><tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available from this section:</td><td align='right'>" & Math.Round((ndav - used), 2).ToString & "</td></tr></table></tr>"

                        End If
                        If color <> "red" Then
                            tused = Math.Round((tused + used), 2)
                            tndav = Math.Round((tndav + (ndav - used)), 2)
                        End If
                    End While
                    output &= "<tr style='background-color:#aabbcc;text-align:right;'><td colspan=2>Total Day taken: </td><td align='left'>" & tused.ToString & _
                           "</td></tr><tr style='background-color:#aabbcc;text-align:right;'><td colspan='2'> Available Days: </td><td align='right'>" & tndav.ToString & "</td></tr></table>"
                End If
                'Response.Write(output)
                ' Return output
            Catch ex As Exception
                output &= "etttototo <script type='text/javascript'>//document.location.href='admin_home.php'" & Chr(13) & _
                "window.location='empcontener.aspx';</script>"
                exception_hand(ex, "master page Erro")
            End Try

            output &= "   </ul></div>"
            Return output
        End Function

        Public Function getleaveinfobal(ByVal emptid As Integer, ByVal con As SqlConnection)
            Dim output As String = ""
            output = "<div id=" & Chr(34) & "divleave" & Chr(34) & " > " & _
            "<ul id=" & Chr(34) & "me" & Chr(34) & " style=" & Chr(34) & "display:none; vertical-align:top; " & Chr(34) & "> "

            Dim bgtid As Integer
            Dim dbs As New dbclass
            'Dim rqid As Integer
            Dim bgtno As Double = 0
            Dim ndav As Double = 0
            Dim used As Double = 0
            Dim tused As Double = 0
            Dim tndav As Double = 0
            Dim dt As DataTableReader
            Dim color, ref, rmk As String
            Dim fm As New formMaker
            Dim i As Integer
            color = "blue"
            Dim dt2 As DataTableReader
            Dim amt As Double = 0

            Dim otp As String = ""
            'Dim emptid As Integer
            ' Dim fm As New formMaker
            Try
                dt = dbs.dtmake("leavesee", "select * from show_leave_bal where emptid=" & emptid & " ORDER BY ID", con)


                If dt.HasRows Then
                    output &= "<script> function clickedlv(type,url){var sub;" & _
                        "switch(type)" & _
"{case 'new': sub='type=new&'+url; break; case 'edit': sub='type=edit&'+url; break;}" & _
 " $('#fpay').attr('frameborder','0');" & _
          "  $('#frmx').attr('target','fpay');" & _
           " $('#frmx').attr('action','leavesettled.aspx?' + sub);" & _
             "$('#pay').css({top:'0px',left:'0px'});" & _
            "$('#pay').remove('display');" & _
           " $('#pay').dialog({" & _
           " top:30," & _
"title:              'Leave balance settlement reg.'," & _
     "               height:600," & _
      "              width:850," & _
                   " modal:true});" & _
                    " $('#fpay').css({width:'800px'});" & _
                    "$('#frmx').submit();" & _
"}</script><table width='700' >" & _
                    "<tr><td><label>Bdgt. Id.</label></td><td><label>Year End</label></td>" & _
                    "<td><label>Bugdeted</label></td><td>Used</td><td><label>Avl</label></td><td></label>Balance</label></td></tr>"
                    tndav = 0
                    Dim url As String = ""
                    Dim sttl As String
                    While dt.Read
                        used = 0
                        ndav = 0
                        sttl = 0
                        If color <> "#E3EAEB" Then
                            color = "#E3EAEB"
                        Else
                            color = "#fdafda"
                        End If
                        If fm.isexp(Today.ToShortDateString, dt.Item("Year End"), 2, "y") Then
                            color = "red"
                        End If


                        bgtid = dt.Item("id")
                        sttl = (fm.getinfo2("select bal from leav_settled where bgtid=" & bgtid, con))
                        If Not IsNumeric(sttl) Then
                            sttl = "0"
                        End If
                        output &= "<tr style='background-color:" & color & ";'>"
                        Dim isdr As String = fm.getinfo2("select resign_date from emp_resign where emptid=" & emptid, con)
                        If IsDate(isdr) Then
                            ndav = fm.showavdate_rev(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"), isdr)
                        Else
                            ndav = fm.showavdate_rev(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"), "")
                        End If
                        'ndav = fm.showavdate(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"))
                        For i = 0 To dt.FieldCount - 3
                            If i <> 1 And i <> 2 And i <> 3 Then
                                If i = 4 Then
                                    Dim dd As Date
                                    dd = dt.Item(i)
                                    output &= "<td>" & MonthName(dd.Month) & " " & dt.Item(i).day.ToString & ", " & dt.Item(i).year.ToString & "</td>"
                                Else
                                    output &= "<td>" & dt.Item(i) & "</td>"


                                End If
                            End If
                        Next
                        output &= "<td>" & FormatNumber(ndav, 2).ToString & "</td>"
                        ' output &= "<td>" & FormatNumber(dt.Item(6), 2).ToString & "</td>"

                        output &= "<td>" & FormatNumber(ndav, 2) - CDbl(dt.Item(6)) & "<td>"
                        output &= "<td>"
                        amt = 0
                        url = ""
                        amt = Me.getleaveset(emptid, dt.Item("Year End"), (FormatNumber(ndav, 2) - CDbl(dt.Item(6))).ToString, con)
                        url = "emptid=" & emptid & "&bgtid=" & bgtid & "&bno=" & dt.Item(7)
                        otp = ""
                        ref = ""
                        rmk = ""
                        otp = fm.getinfo2("select paidamt from leav_settled where bgtid='" & bgtid & "'", con)

                        If IsNumeric(otp) = True Then
                            If CInt(otp) > 0 Then
                                ref = fm.getinfo2("select ref from leav_settled where bgtid='" & bgtid & "'", con)
                                rmk = fm.getinfo2("select remark from leav_settled where bgtid='" & bgtid & "'", con)
                                url &= "&ref=" & ref & "&remark=" & rmk & "&amt=" & FormatNumber(otp.ToString, 2, TriState.True, TriState.True, TriState.True).ToString
                                output &= "<span style='color:blue;cursor:pointer;' onclick=" & Chr(34) & "javascript:clickedlv('edit','" & url & "');" & Chr(34) & ">Settled Amt:" & FormatNumber(otp.ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</span>"
                            End If
                        Else

                            If otp = "None" Then
                                url &= "&amt=" & FormatNumber(amt.ToString, 2, TriState.True, TriState.True, TriState.True).ToString
                                output &= "<span style='color:blue;cursor:pointer;' onclick=" & Chr(34) & "javascript:clickedlv('new','" & url & "');" & Chr(34) & ">Pay Amt:" & FormatNumber(amt.ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</span>"
                            End If
                        End If
                        output &= "</td></tr>"


                    End While
                    output &= "</table>"
                End If
                'Response.Write(output)
                ' Return output
            Catch ex As Exception
                output &= " <script type='text/javascript'>//document.location.href='admin_home.php'" & Chr(13) & _
                "window.location='empcontener.aspx?msg=" & ex.ToString & "';</script>"
                exception_hand(ex, "master page Erro")
            End Try
            dt = Nothing
            dbs = Nothing
            output &= "   </ul></div>"
            Return output
        End Function
        Public Function getleaveinfobalo(ByVal emptid As Integer, ByVal con As SqlConnection)
            Dim output As String = ""
            output = "<div id=" & Chr(34) & "divleave" & Chr(34) & " > " & _
            "<ul id=" & Chr(34) & "me" & Chr(34) & " style=" & Chr(34) & "display:none; vertical-align:top; " & Chr(34) & "> "

            Dim bgtid As Integer
            Dim dbs As New dbclass
            'Dim rqid As Integer
            Dim bgtno As Double = 0
            Dim ndav As Double = 0
            Dim used As Double = 0
            Dim tused As Double = 0
            Dim tndav As Double = 0
            Dim dt As DataTableReader
            Dim color, ref, rmk As String
            Dim fm As New formMaker
            Dim i As Integer
            color = "blue"
            Dim dt2 As DataTableReader
            Dim amt As Double = 0

            Dim otp As String = ""
            'Dim emptid As Integer
            ' Dim fm As New formMaker
            Try
                dt = dbs.dtmake("taxpayer", "select * from show_leave_bal where emptid=" & emptid & " ORDER BY ID", con)


                If dt.HasRows Then
                    output &= "<script> function clickedlv(type,url){var sub;" & _
                        "switch(type)" & _
"{case 'new': sub='type=new&'+url; break; case 'edit': sub='type=edit&'+url; break;}" & _
 " $('#fpay').attr('frameborder','0');" & _
          "  $('#frmx').attr('target','fpay');" & _
           " $('#frmx').attr('action','leavesettled.aspx?' + sub);" & _
             "$('#pay').css({top:'0px',left:'0px'});" & _
            "$('#pay').remove('display');" & _
           " $('#pay').dialog({" & _
           " top:30," & _
"title:              'Leave balance settlement reg.'," & _
     "               height:600," & _
      "              width:850," & _
                   " modal:true});" & _
                    " $('#fpay').css({width:'800px'});" & _
                    "$('#frmx').submit();" & _
"}</script><table width='700' >" & _
                    "<tr><td><label>Bdgt. Id.</label></td><td><label>Year End</label></td>" & _
                    "<td><label>Bugdeted</label></td><td>Used</td><td><label>Avl</label></td><td></label>Balance</label></td></tr>"
                    tndav = 0
                    Dim url As String = ""
                    While dt.Read
                        used = 0
                        ndav = 0
                        If color <> "#E3EAEB" Then
                            color = "#E3EAEB"
                        Else
                            color = "#fdafda"
                        End If
                        If fm.isexp(Today.ToShortDateString, dt.Item("Year End"), 2, "y") Then
                            color = "red"
                        End If
                        bgtid = dt.Item("id")
                        output &= "<tr style='background-color:" & color & ";'>"
                        Dim isdr As String = fm.getinfo2("select resign_date from emp_resign where emptid=" & emptid, con)
                        If IsDate(isdr) Then
                            ndav = fm.showavdate_rev(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"), isdr)
                        Else
                            ndav = fm.showavdate_rev(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"), "")
                        End If
                        'ndav = fm.showavdate(dt.Item("Year Start"), dt.Item("Year End"), dt.Item("Budget"))
                        For i = 0 To dt.FieldCount - 3
                            If i <> 1 And i <> 2 And i <> 3 Then
                                If i = 4 Then
                                    Dim dd As Date
                                    dd = dt.Item(i)
                                    output &= "<td>" & MonthName(dd.Month) & " " & dt.Item(i).day.ToString & ", " & dt.Item(i).year.ToString & "</td>"
                                Else
                                    output &= "<td>" & dt.Item(i) & "</td>"


                                End If
                            End If
                        Next
                        output &= "<td>" & FormatNumber(ndav, 2).ToString & "</td>"
                        ' output &= "<td>" & FormatNumber(dt.Item(6), 2).ToString & "</td>"

                        output &= "<td>" & FormatNumber(ndav, 2) - CDbl(dt.Item(6)) & "<td>"
                        output &= "<td>"
                        amt = 0
                        url = ""
                        amt = Me.getleaveset(emptid, dt.Item("Year End"), (FormatNumber(ndav, 2) - CDbl(dt.Item(6))).ToString, con)
                        url = "emptid=" & emptid & "&bgtid=" & bgtid & "&bno=" & dt.Item(7)
                        otp = ""
                        ref = ""
                        rmk = ""
                        otp = fm.getinfo2("select paidamt from leav_settled where bgtid='" & bgtid & "'", con)

                        If IsNumeric(otp) = True Then
                            If CInt(otp) > 0 Then
                                ref = fm.getinfo2("select ref from leav_settled where bgtid='" & bgtid & "'", con)
                                rmk = fm.getinfo2("select remark from leav_settled where bgtid='" & bgtid & "'", con)
                                url &= "&ref=" & ref & "&remark=" & rmk & "&amt=" & FormatNumber(otp.ToString, 2, TriState.True, TriState.True, TriState.True).ToString
                                output &= "<span style='color:blue;cursor:pointer;' onclick=" & Chr(34) & "javascript://clickedlv('edit','" & url & "');" & Chr(34) & ">Settled Amt:" & FormatNumber(otp.ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</span>"
                            End If
                        Else

                            If otp = "None" Then
                                url &= "&amt=" & FormatNumber(amt.ToString, 2, TriState.True, TriState.True, TriState.True).ToString
                                output &= "<span style='color:blue;cursor:pointer;' onclick=" & Chr(34) & "javascript://clickedlv('new','" & url & "');" & Chr(34) & ">Pay Amt:" & FormatNumber(amt.ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</span>"
                            End If
                        End If
                        output &= "</td></tr>"


                    End While
                    output &= "</table>"
                End If
                'Response.Write(output)
                ' Return output
            Catch ex As Exception
                output &= " <script type='text/javascript'>//document.location.href='admin_home.php'" & Chr(13) & _
                "window.location='empcontener.aspx?msg=" & ex.ToString & "';</script>"
                exception_hand(ex, "master page Erro")
            End Try
            dt = Nothing
            dbs = Nothing
            output &= "   </ul></div>"
            Return output
        End Function
        Public Function getleaveset(ByVal emptid As Integer, ByVal datev As Date, ByVal lvba As String, ByVal con As SqlConnection)
            Dim salary() As String
            Dim dbs As New dbclass
            Dim hr, amttop As Double
            Dim fm As New formMaker
            salary = dbs.getsal_sp(emptid, datev, con)
            If IsNumeric(salary(0)) = False Then
                ' Response.Write(salary)
                salary(0) = "0.00"
            End If
            amttop = 0
            ' salary = dbx.getsal(emptid, con)
            'salary = 3800
            hr = CDbl(salary(0)) / 200.67
            '  lvba = fm.getinfo2("select balance from show_leave_bal where id=" & bdgtid, con)
            ' Response.Write(res.ToString & "<br>")
            If CDbl(lvba) > 0 Then
                amttop = (CInt(lvba) * 8 * hr)
                If (CDbl(lvba) - CInt(lvba)) > 0.5 Then
                    amttop += (0.5 * 8 * hr)
                End If

            End If
            Return amttop
        End Function
        Public Function getinfo(ByVal fname As String, ByVal tbl As String, ByVal where As String, ByVal con As SqlConnection)
            Dim db As New dbclass
            Dim dt As DataTableReader
            dt = db.dtmake("fullinfo" & Today.ToLongDateString, "select " & fname & " from " & tbl & " where " & where, con)
            If dt.HasRows Then
                dt.Read()
                Return dt.Item(0)
            End If
            dt.Close()
            db = Nothing
        End Function
        Public Function getinfo2(ByVal sql As String, ByVal con As SqlConnection) As String
            Dim db As New dbclass
            Dim dt As DataTableReader
            Try
                dt = db.dtmake("fullinfox" & Today.ToLongDateString, sql, con)
            Catch ex As Exception
                Return ex.ToString & "<br>" & sql
                exception_hand(ex, "master page Erro")
            End Try

            If dt.HasRows Then
                dt.Read()
                Return dt.Item(0).ToString
            Else
                Return "None"
            End If
            dt.Close()
            db = Nothing
            Return "None "
        End Function
        Public Function edit_del_list3(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String, ByVal floc As String, ByVal btnedit As Boolean, ByVal btndel As Boolean, ByVal btnupload As Boolean, ByVal fc As Boolean) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim dt As DataTableReader
            Dim hdr() As String
            Dim fl As New file_list
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
            rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0' width='900px'>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                Next
            End If

            rtstr = rtstr & "</tr>"
            Dim outval As String
            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If

                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    If btnedit = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' ><img id='img" & dt.Item(0) & "' src='images/png/edit.png' title='Edit' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");' /></td> "
                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='images/png/editx.png' title='Edit Disabled'  /></td> "

                    End If
                    If btndel = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;'><img id='nimg" & dt.Item(0) & "' src='images/png/delete.png' title='Delete' style='curser:pointr;'   onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'/></td>"

                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='images/png/deletex.png' title='Delete Canceled' style='curser:pointr;' /></td>"

                    End If
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                outval = "yes"
                            ElseIf dt.Item(k).ToString = "n" Then
                                outval = "No"
                            Else
                                outval = dt.Item(k)
                            End If
                        Else
                            outval = "-"
                        End If
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & outval & "</td>"
                    Next

                    If btnupload = True Then
                        Dim fileno As Integer
                        Dim lfloc As String
                        If fc = True Then

                            lfloc = floc & "\" & dt.Item("id") & "\"
                            fileno = fl.fileno(lfloc)

                        End If
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer; font-size:9pt;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/upload.png' title='upload' style='curser:pointr;' alt='upload'  width='20' height='20' />UPLOAD FILES(" & dt.Item("id") & ")#"
                        If fileno > 0 Then
                            rtstr &= fileno.ToString
                        Else
                            rtstr &= "0"
                        End If
                        rtstr &= "</td>"

                    End If




                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table></form>"
            dt.Close()
            dc = Nothing
            Return rtstr

        End Function
        Public Function edit_del_list34(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String, ByVal floc As String, ByVal btnedit As Boolean, ByVal btndel As Boolean, ByVal btnupload As Boolean, ByVal fc As Boolean) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim dt As DataTableReader
            Dim hdr() As String
            Dim fl As New file_list
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
            rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0' width='900px'>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                Next
            End If

            rtstr = rtstr & "</tr>"
            Dim outval As String
            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If

                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    If btnedit = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' ><img id='img" & dt.Item(0) & "' src='../images/png/edit.png' title='Edit' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");' /></td> "
                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='../images/png/editx.png' title='Edit Disabled'  /></td> "

                    End If
                    If btndel = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;'><img id='nimg" & dt.Item(0) & "' src='../images/png/delete.png' title='Delete' style='curser:pointr;'   onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'/></td>"

                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='../images/png/deletex.png' title='Delete Canceled' style='curser:pointr;' /></td>"

                    End If
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                outval = "yes"
                            ElseIf dt.Item(k).ToString = "n" Then
                                outval = "No"
                            Else
                                outval = dt.Item(k)
                            End If
                        Else
                            outval = "-"
                        End If
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & outval & "</td>"
                    Next

                    If btnupload = True Then
                        Dim fileno As Integer
                        Dim lfloc As String
                        If fc = True Then

                            lfloc = floc & "\" & dt.Item("id") & "\"
                            fileno = fl.fileno(lfloc)

                        End If
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer; font-size:9pt;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/upload.png' title='upload' style='curser:pointr;' alt='upload'  width='20' height='20' />UPLOAD FILES(" & dt.Item("id") & ")#"
                        If fileno > 0 Then
                            rtstr &= fileno.ToString
                        Else
                            rtstr &= "0"
                        End If
                        rtstr &= "</td>"

                    End If




                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table></form>"
            dt.Close()
            dc = Nothing
            Return rtstr

        End Function
        Public Function getfullname(ByVal empid As String, ByVal con As SqlConnection)
            Dim dt As DataTableReader
            Dim db As New dbclass
            Dim fname As String = ""
            dt = db.dtmake("getfullname", "select * from emp_static_info where emp_id='" & empid & "'", con)
            If dt.HasRows Then
                dt.Read()
                fname = dt.Item("first_name").trim(" ")
                fname &= " " & dt.Item("middle_name").trim(" ")
                fname &= " " & dt.Item("last_name").trim(" ")
            End If
            dt.Close()
            db = Nothing
            Return fname
        End Function
        Public Function getqualification(ByVal empid As String, ByVal con As SqlConnection)

            ' Dim sql As String
            Dim dt As DataTableReader
            Dim db As New dbclass
            Dim fname As String = ""
            Dim i As Integer = 0
            Dim rt(1, 1) As String
            Dim fld(5, 1) As String

            fld(0, 0) = "0"
            dt = db.dtmake("getqul", "select qualification,diciplin,school,year_g from emp_education where emp_id='" & empid & "'", con)
            If dt.HasRows Then
                While dt.Read
                    ReDim Preserve fld(5, i + 1)
                    fld(0, i) = i.ToString
                    fld(1, i) = dt.Item("qualification").ToString
                    fld(2, i) = dt.Item("year_g").ToString
                    fld(3, i) = dt.Item("school").ToString
                    fld(4, i) = dt.Item("diciplin").ToString
                    i = i + 1
                End While
            End If
            dt.Close()
            db = Nothing
            If i > 1 Then

                fld(0, 0) = i.ToString

                Return fld

            End If

            Return fld
        End Function

        Public Function edit_del_list2(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String, ByVal floc As String, ByVal btnedit As Boolean, ByVal btndel As Boolean, ByVal btnupload As Boolean, ByVal fc As Boolean) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass

            Dim dt As DataTableReader
            Dim hdr() As String
            Dim fl As New file_list
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
            rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                Next
            End If

            rtstr = rtstr & "</tr>"

            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If

                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    If btnedit = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> "
                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='images/png/editx.png' title='Edit Disabled'  /></td> "

                    End If
                    If btndel = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>"

                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='images/png/deletex.png' title='Delete Canceled' style='curser:pointr;' /></td>"

                    End If
                    For k As Integer = 1 To dt.FieldCount - 1
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                    Next
                    If btnupload = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/upload.png' title='upload' style='curser:pointr;' alt='upload'  width='20' height='20' />UPLOAD FILES</td>"

                    End If
                    If fc = True Then
                        Dim fileno As Integer
                        Dim lfloc As String

                        lfloc = floc & "\" & dt.Item("id") & "\"
                        fileno = fl.fileno(lfloc)
                        If fileno > 0 Then
                            rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & fileno & "</td>"
                        Else
                            rtstr = rtstr & "<td title='" & lfloc & "'>No Upload Files</td>"
                        End If
                    End If
                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table></form>"
            dt.Close()
            dc = Nothing
            Return rtstr

        End Function
        Public Function edit_del_list234(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String, ByVal floc As String, ByVal btnedit As Boolean, ByVal btndel As Boolean, ByVal btnupload As Boolean, ByVal fc As Boolean) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim dt As DataTableReader
            Dim hdr() As String
            Dim fl As New file_list
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
            rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                Next
            End If

            rtstr = rtstr & "</tr>"

            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If

                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    If btnedit = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='../images/png/edit.png' title='Edit'  /></td> "
                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='../images/png/editx.png' title='Edit Disabled'  /></td> "

                    End If
                    If btndel = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='../images/png/delete.png' title='Delete' style='curser:pointr;' /></td>"

                    Else
                        rtstr = rtstr & "<td  style='padding-right:20px;'><img src='../images/png/deletex.png' title='Delete Canceled' style='curser:pointr;' /></td>"

                    End If
                    For k As Integer = 1 To dt.FieldCount - 1
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                    Next
                    If btnupload = True Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='../images/png/upload.png' title='upload' style='curser:pointr;' alt='upload'  width='20' height='20' />UPLOAD FILES</td>"

                    End If
                    If fc = True Then
                        Dim fileno As Integer
                        Dim lfloc As String

                        lfloc = floc & "\" & dt.Item("id") & "\"
                        fileno = fl.fileno(lfloc)
                        If fileno > 0 Then
                            rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & fileno & "</td>"
                        Else
                            rtstr = rtstr & "<td title='" & lfloc & "'>No Upload Files</td>"
                        End If
                    End If
                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table></form>"
            dt.Close()
            dc = Nothing
            Return rtstr

        End Function

        Public Function edit_del_list(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<script type='text/javascript'>" & Chr(13) & _
            "function goclicked(whr,id)" & Chr(13) & _
            "{  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());" & Chr(13) & _
            "$('#frms').submit();}</script>" & Chr(13)
            rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & Chr(13) & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>" & Chr(13) & _
            "<td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>" & Chr(13)
            Try

            
                dt = dc.dtmake(tbl & "x", sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                Next
            End If

            rtstr = rtstr & "</tr>" & Chr(13)

            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & Chr(13) & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & Chr(13) & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & Chr(13) & _
                    "<img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>" & Chr(13)
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.Item(k).ToString = "y" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                        ElseIf dt.Item(k).ToString = "n" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                        ElseIf dt.GetName(k) = "department" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        ElseIf dt.GetName(k) = "project_id" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select project_name from tblproject where project_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        Else

                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                        End If

                    Next
                    rtstr = rtstr & "</tr>" & Chr(13)
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>" & Chr(13)
                End If
                dt.Close()
            Catch ex As Exception
                rtstr &= ex.ToString & "<br>" & sql & tbl
                exception_hand(ex, "master page Erro")
            End Try
            rtstr = rtstr & "</table></form>"

            dc = Nothing

            Return rtstr

        End Function
        Public Function edit_del_list_hlp(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim sec As New k_security
            Dim fl As New file_list
            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<script type='text/javascript'>" & Chr(13) & _
            "function goclicked(whr,id)" & Chr(13) & _
            "{  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());" & Chr(13) & _
            "$('#frms').submit();}</script>" & Chr(13)
            rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & Chr(13) & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>" & Chr(13) & _
            "<td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>" & Chr(13)
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                Next
            End If

            rtstr = rtstr & "</tr>" & Chr(13)

            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & Chr(13) & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & Chr(13) & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & Chr(13) & _
                    "<img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>" & Chr(13)
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.Item(k).ToString = "y" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                        ElseIf dt.Item(k).ToString = "n" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                        ElseIf dt.GetName(k) = "department" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        ElseIf dt.GetName(k) = "project_id" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select project_name from tblproject where project_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        ElseIf (dt.FieldCount - 1) = k Then
                            rtstr &= "<td><div id='x" & dt.Item(0) & "' onclick=" & Chr(34) & "javascript:$('#that" & dt.Item(0) & "').remove('dispaly');$('#that" & dt.Item(0) & "').dialog({title:'Data Saving',height:300,width:600,modal:true});" & Chr(34) & " style='cursor:pointer;color:blue;'>view</div>"

                            rtstr &= "<div id='that" & dt.Item(0) & "' title='view' style='display:none;'>" & sec.dbHexToStr(dt.Item(k)) & "</div>"
                        Else

                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                        End If

                    Next
                    rtstr = rtstr & "</tr>" & Chr(13)
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>" & Chr(13)
            End If
            rtstr = rtstr & "</table></form>"
            dt.Close()
            dc = Nothing
            Return rtstr

        End Function

        Public Function edit_del_list_wname_ot(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim sec As New k_security
            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            Dim arr() As String = {""}
            rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                Dim empid As String
                Dim datex As Boolean = False
                rtstr &= "<form id='frms' method='post' name='frms' action=''>"
                While dt.Read
                    If Me.searcharray(arr, dt.Item("datework")) = False Then
                        Dim arrbound As Integer = UBound(arr)
                        ReDim Preserve arr(arrbound + 1)
                        arr(arrbound) = dt.Item("datework")
                        If datex = True Then
                            rtstr &= "</table></div>"
                            datex = False
                        End If
                        If datex = False Then
                            rtstr &= "<br><div id='" & sec.Str2ToHex(dt.Item("datework")) & "' class='collapsed' style='height:25px;width:900px; background-color:blue; cursor:pointer;' onclick=" & Chr(34) & "javascript:showHideSubMenu(this,'tbl" & _
                                                      dt.Item("datework") & "')" & Chr(34) & _
                                                      ">" & dt.Item("datework") & "</div><div id='tbl" & sec.Str2ToHex(dt.Item("datework")) & "' style='display:none;'><table  cellspacing='0' cellpadding='0'>" & _
                                          "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
                            If heading <> "" Then
                                For i = 0 To hdr.Length - 1
                                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                                Next
                            Else
                                For i = 1 To dt.FieldCount - 1
                                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"
                                Next
                            End If
                            rtstr = rtstr & "</tr>"
                            datex = True

                        End If
                    End If
                    empid = dt.Item("emptid")
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>"
                    rtstr &= "<td>" & Me.getfullname(Me.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "</td>"
                    For k As Integer = 1 To dt.FieldCount - 2
                        If dt.Item(k).ToString = "y" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                        ElseIf dt.Item(k).ToString = "n" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                        ElseIf dt.GetName(k) = "department" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        ElseIf dt.GetName(k) = "project_id" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select project_name from tblproject where project_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        Else

                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                        End If

                    Next
                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table></div></form>"
            dt.Close()
            dc = Nothing
            Return rtstr
        End Function
        Public Function edit_del_list_wname(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
            rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If heading <> "" Then
                For i = 0 To hdr.Length - 1
                    rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
                Next
            Else
                For i = 1 To dt.FieldCount - 1
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                Next
            End If

            rtstr = rtstr & "</tr>"

            If dt.HasRows = True Then
                Dim color As String = "E3EAEB"
                Dim empid As String
                While dt.Read
                    empid = dt.Item("emptid")
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & _
                    "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>"
                    rtstr &= "<td>" & Me.getfullname(Me.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "</td>"
                    For k As Integer = 1 To dt.FieldCount - 2
                        If dt.Item(k).ToString = "y" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                        ElseIf dt.Item(k).ToString = "n" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                        ElseIf dt.GetName(k) = "department" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        ElseIf dt.GetName(k) = "project_id" Then
                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                            Me.getinfo2("select project_name from tblproject where project_id='" & _
                            dt.Item(k).ToString & "'", con) & "</td>"
                        Else

                            rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                        End If

                    Next
                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table></form>"
            dt.Close()
            dc = Nothing
            Return rtstr

        End Function
        Public Function tableview(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<table id='tblall' cellspacing='0' cellpadding='3' border=1>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
            Try
                dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            Catch ex As Exception
                Return sql & "..." & ex.ToString
                exception_hand(ex, "master page Erro")
            End Try

            If dt.HasRows = True Then
                If heading <> "" Then
                    For i = 0 To hdr.Length - 1
                        rtstr = rtstr & "<td style='padding-right:10px;'><label>" & hdr(i) & "</label></td>"
                    Next
                Else
                    For i = 1 To dt.FieldCount - 1

                        rtstr = rtstr & "<td  style='padding-right:10px;'>" & dt.GetName(i) & "</td>"

                    Next
                End If
                rtstr = rtstr & "</tr>"
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    ' Dim outp As String
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>Yes</td>"
                            ElseIf dt.Item(k).ToString = "n" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>No</td>"
                            ElseIf dt.GetName(k) = "department" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>" & _
                                Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                                dt.Item(k).ToString & "'", con) & "</td>"
                            ElseIf dt.GetName(k) = "project_id" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>" & _
                                Me.getinfo2("select project_name from tblproject where project_id='" & _
                                dt.Item(k).ToString & "'", con).ToString & "</td>"
                            ElseIf dt.GetName(k) = "Total" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                            ElseIf dt.GetName(k) = "pardim" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"

                            Else

                                rtstr = rtstr & "<td  style='padding-right:10px;'>" & dt.Item(k) & "</td>"
                            End If
                            ' rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>" & outp & "</td>"
                        Else
                            rtstr = rtstr & "<td  style='padding-right:10px;font-size:11pt;'>-</td>"
                        End If
                    Next
                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table>"
            dt.Close()
            dc = Nothing
            dt = Nothing
            Return rtstr

        End Function
        Public Function tableviewsal(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass

            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<table cellspacing='0' cellpadding='7' width='800' border=1>" & _
            "<th colspan='4'>" & tbl & "</th>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
            Try
                dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            Catch ex As Exception
                Return "Error: copy the following error and mail it to the developer<br>" & sql & "<br>System shows:" & ex.ToString
            End Try

            If dt.HasRows = True Then
                If heading <> "" Then
                    For i = 0 To hdr.Length - 1
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'><label>" & hdr(i) & "</label></td>"
                    Next
                Else
                    For i = 1 To dt.FieldCount - 1

                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                    Next
                End If

                rtstr = rtstr & "</tr>"
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    ' Dim outp As String
                    For k As Integer = 1 To dt.FieldCount - 3
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                            ElseIf dt.Item(k).ToString = "n" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                            ElseIf dt.GetName(k) = "department" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                                Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                                dt.Item(k).ToString & "'", con) & "</td>"
                            ElseIf dt.GetName(k) = "project_id" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                                Me.getinfo2("select project_name from tblproject where project_id='" & _
                                dt.Item(k).ToString & "'", con).ToString & "</td>"
                            ElseIf dt.GetName(k) = "Total" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                            ElseIf dt.GetName(k) = "pardim" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"

                            ElseIf dt.GetName(k) = "basic_salary" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"


                            Else

                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                            End If
                            ' rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>" & outp & "</td>"
                        Else
                            rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>-</td>"
                        End If
                    Next
                    rtstr &= "<td>"
                    rtstr &= (getinfo2("select project_name from tblproject where project_id in(select project_id from emp_job_assign where '" & dt.Item("date_start") & "' between date_from and isnull(date_end,'" & Now.ToShortDateString & "') and emp_id='" & dt.Item("emp_id") & "')", con))
                    rtstr &= "</td>"

                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table>"
            dt.Close()
            dc = Nothing
            dt = Nothing
            Return rtstr

        End Function
        Public Function tableviewsalpaid(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass

            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<table cellspacing='0' cellpadding='7' width='800' border=1>" & _
            "<th colspan='4'>" & tbl & "</th>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
            Try
                dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            Catch ex As Exception
                Return "Error: copy the following error and mail it to the developer<br>" & sql & "<br>System shows:" & ex.ToString
            End Try

            If dt.HasRows = True Then
                If heading <> "" Then
                    For i = 0 To hdr.Length - 1
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'><label>" & hdr(i) & "</label></td>"
                    Next
                Else
                    For i = 1 To dt.FieldCount - 1

                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                    Next
                End If

                rtstr = rtstr & "</tr>"
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    ' Dim outp As String
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"

                            ElseIf dt.GetName(k) = "emptid" Or dt.GetName(k) = "pay_date" Then

                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                            Else

                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"

                            End If
                            ' rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>" & outp & "</td>"
                        Else
                            rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>-</td>"
                        End If
                    Next


                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table>"
            dt.Close()
            dc = Nothing
            dt = Nothing
            Return rtstr

        End Function
        Public Function tableviewsalpaidx(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass

            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<table cellspacing='0' cellpadding='7' width='800' border=1>" & _
            "<th colspan='14'>" & tbl & "</th>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
            Try
                dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            Catch ex As Exception
                Return "Error: copy the following error and mail it to the developer<br>" & sql & "<br>System shows:" & ex.ToString
            End Try

            If dt.HasRows = True Then
                If heading <> "" Then
                    For i = 0 To hdr.Length - 1
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'><label>" & hdr(i) & "</label></td>"
                    Next
                Else
                    For i = 1 To dt.FieldCount - 1

                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                    Next
                End If

                rtstr = rtstr & "</tr>"
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    ' Dim outp As String
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"

                            ElseIf dt.GetName(k) = "emptid" Or dt.GetName(k) = "date_paid" Then

                                rtstr = rtstr & "<td  style='padding-right:20px;'> " & MonthName(CDate(dt.Item(k)).Month, True) & " " & CDate(dt.Item(k)).Year.ToString & "</td>"
                            Else

                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>"
                                If IsNumeric(dt.Item(k)) = True Then
                                    rtstr &= FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True)
                                Else
                                    rtstr &= dt.Item(k)
                                End If
                                rtstr &= "</td>"

                            End If
                            ' rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>" & outp & "</td>"
                        Else
                            rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>-</td>"
                        End If
                    Next


                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table>"
            dt.Close()
            dc = Nothing
            dt = Nothing
            Return rtstr

        End Function
        Public Function tableviewpar(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass

            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            rtstr = "<table cellspacing='0' cellpadding='7' width='800' border=1>" & _
            "<th colspan='4'>" & tbl & "</th>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
            Try
                dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            Catch ex As Exception
                Return "Error: copy the following error and mail it to the developer<br>" & sql & "<br>System shows:" & ex.ToString
            End Try

            If dt.HasRows = True Then
                If heading <> "" Then
                    For i = 0 To hdr.Length - 1
                        rtstr = rtstr & "<td style='padding-right:20px;font-size:12pt;'><label>" & hdr(i) & "</label></td>"
                    Next
                Else
                    For i = 1 To dt.FieldCount - 1

                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

                    Next
                End If

                rtstr = rtstr & "</tr>"
                Dim color As String = "E3EAEB"
                While dt.Read
                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    ' Dim outp As String
                    For k As Integer = 1 To dt.FieldCount - 3
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                            ElseIf dt.Item(k).ToString = "n" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                            ElseIf dt.GetName(k) = "department" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                                Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                                dt.Item(k).ToString & "'", con) & "</td>"
                            ElseIf dt.GetName(k) = "project_id" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                                Me.getinfo2("select project_name from tblproject where project_id='" & _
                                dt.Item(k).ToString & "'", con).ToString & "</td>"
                            ElseIf dt.GetName(k) = "Total" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"
                            ElseIf dt.GetName(k) = "pardim" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"

                            ElseIf dt.GetName(k) = "pardime" Then
                                rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True) & "</td>"


                            Else

                                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                            End If
                            ' rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>" & outp & "</td>"
                        Else
                            rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>-</td>"
                        End If
                    Next
                    rtstr &= "<td>"
                    Dim d2 As Date
                    d2 = Today.ToShortDateString
                    rtstr &= getinfo2("select project_name from tblproject where project_id in(select project_id from emp_job_assign where ('" & dt.Item("from_date") & "'  between date_from and isnull(date_end,'" & d2 & "') or  date_from between '" & dt.Item("from_date") & "' and isnull(date_end,'" & d2 & "'))  and emp_id='" & dt.Item("emp_id") & "')", con)
                    rtstr &= "</td>"

                    rtstr = rtstr & "</tr>"
                End While
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table>"
            dt.Close()
            dc = Nothing
            dt = Nothing
            Return rtstr

        End Function
        Public Function tableview_wexp(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
            Dim rtstr As String = ""
            Dim dc As New dbclass
            Dim dt As DataTableReader
            Dim hdr() As String
            hdr = heading.Split(",")
            Dim i As Integer
            Dim d, d1 As Date
            Dim exp, sumexp As Double
            exp = 0
            sumexp = 0
            rtstr = "<table id='wexp' cellspacing='0' cellpadding='3' border=1>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
            dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
            If dt.HasRows = True Then
                If heading <> "" Then
                    For i = 0 To hdr.Length - 1
                        rtstr = rtstr & "<td style='padding-right:10px;'><label>" & hdr(i) & "</label></td>"
                    Next
                Else
                    For i = 1 To dt.FieldCount - 1

                        rtstr = rtstr & "<td  style='padding-right:10px;'><label>" & dt.GetName(i) & "</label></td>"

                    Next
                End If
                rtstr = rtstr & "<td  style='padding-right:10px;'><label>Experiance</label></td>"

                rtstr = rtstr & "</tr>"
                Dim color As String = "E3EAEB"
                Dim newf As Boolean = False
                Dim expout As String = ""
                While dt.Read
                    newf = False
                    If dt.IsDBNull(2) = False And dt.IsDBNull(3) = False Then
                        d = dt.Item("from_d")
                        d1 = dt.Item("to_d")
                        newf = True

                    End If
                    If newf = True Then
                        exp = (d1.Subtract(d).Days / 30.4)
                        expout = CInt(exp).ToString & "Ms "
                        If CInt(exp) > 12 Then
                            expout = Math.Floor(exp / 12).ToString & "Yrs " & CInt(exp Mod 12).ToString & "Ms"
                        End If

                    Else
                        exp = 0
                    End If
                    If dt.IsDBNull(5) = False Then
                        If dt.Item(5) = "Full Time" Then
                            sumexp += exp
                        Else
                            newf = False
                        End If
                    Else
                        sumexp += exp
                    End If

                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    ' Dim outp As String
                    For k As Integer = 1 To dt.FieldCount - 1
                        If dt.IsDBNull(k) = False Then
                            If dt.Item(k).ToString = "y" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>Yes</td>"
                            ElseIf dt.Item(k).ToString = "n" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>No</td>"
                            ElseIf dt.GetName(k) = "department" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>" & _
                                Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                                dt.Item(k).ToString & "'", con) & "</td>"
                            ElseIf dt.GetName(k) = "project_id" Then
                                rtstr = rtstr & "<td  style='padding-right:10px;'>" & _
                                Me.getinfo2("select project_name from tblproject where proj_id='" & _
                                dt.Item(k).ToString & "'", con) & "</td>"
                            Else

                                rtstr = rtstr & "<td  style='padding-right:10px;'>" & dt.Item(k) & "</td>"
                            End If
                            ' rtstr = rtstr & "<td  style='padding-right:20px;font-size:11pt;'>" & outp & "</td>"
                        Else
                            rtstr = rtstr & "<td  style='padding-right:10px;'>-</td>"
                        End If

                    Next
                    rtstr = rtstr & "<td  style='padding-right:10px;'>" & expout & "</td>"
                    rtstr = rtstr & "</tr>"
                End While
                rtstr &= "<tr><td colspan='8' style='text-align:right'>Total Exp:</td><td>"
                If CInt(exp) > 12 Then
                    expout = Math.Floor(sumexp / 12).ToString & "Yrs " & CInt(sumexp Mod 12).ToString & "Ms"
                Else
                    expout = Math.Floor(sumexp / 12).ToString & "Yrs " & CInt(sumexp Mod 12).ToString & "Ms"
                End If
                rtstr &= expout & "</td></tr>"
            Else
                rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
            End If
            rtstr = rtstr & "</table>"
            dt.Close()
            dc = Nothing
            dt = Nothing
            Return rtstr

        End Function
        Public Function jmakelist(ByVal sql As String, ByVal con As SqlConnection) As String
            Dim dt As DataTableReader
            Dim dc As New dbclass
            Dim thisb As String = ""
            Dim norep() As String
            Dim rtstr As String = "Data base is new! No Data Inside"
            ' Dim sql = "select id," & fields & " from " & tblname
            Dim fc As Double
            If sql <> "" Then
                dt = dc.dtmake("NewQ", sql, con)

                If dt.HasRows = True Then
                    Dim i As Integer = 0
                    rtstr = "<div stryle='font-size:12pt;'><form id='frmemplist' method='post' action='empcontener.aspx' target='_parent'><input type='hidden' id='datatake' name='datatake' /><table cellspacing='0'>"
                    Dim color As String = "#E3EAEB"

                    fc = Math.Round((900 / dt.FieldCount))
                    ReDim norep(1)
                    norep(0) = "x"
                    While dt.Read
                        If i Mod 2 Then
                            color = "#E3EAEB"
                        Else
                            color = "#fefefe"
                        End If

                        If i = 0 Then

                            rtstr &= "<div style='background-color:#243695;height:16px; display:block; width:1200px;font-size:13pt;color:white;' >"
                            For j As Integer = 1 To dt.FieldCount - 3
                                rtstr &= "<div style='width:11%;float:left'>" & dt.GetName(j) & "&nbsp;</div>"
                            Next

                            rtstr &= "</div><div style='clear:both;'></div>"

                        End If

                        If searcharray(norep, dt.Item(0).ToString) = False Then
                            i = i + 1

                            ReDim Preserve norep(i + 1)
                            norep(i) = dt.Item(0).ToString
                            rtstr &= "<div id='" & dt.Item(0) & "' style='height:13px;background-color:" & color & "; cursor:pointer;font-size:11pt; width:1200px;border:1px solid " & color & ";display:block;height:14px;' onclick=" & _
                                              Chr(34) & "javascript:orderpass('" & dt.Item(0) & "');" & Chr(34) & " onmouseover=" & Chr(34) & _
                                              "javascript: this.style.background='#00ff00';" & Chr(34) & " onmouseout=" & Chr(34) & _
                                              "javascript: this.style.background='" & color & "';" & Chr(34) & "> "

                            For j As Integer = 1 To dt.FieldCount - 3
                                If dt.GetName(j) = "department" Then
                                    rtstr &= "<div style='width:11%;float:left;'>" & _
                                    Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                                    dt.Item(j).ToString & "'", con) & "&nbsp;</div>"
                                Else

                                    rtstr &= "<div style='width:11%;float:left;'>" & dt.Item(j) & "&nbsp;</div>"
                                End If
                                rtstr &= "<div style='width:11%;float:left;'>" & dt.Item(j) & "&nbsp;</div>"
                            Next
                            rtstr &= "</div><div style='clear:both;'></div>"
                        End If
                    End While
                End If
                rtstr &= ("</table></form></div>")
                dt.Close()
            End If
            dt = Nothing
            dc = Nothing

            'Return sql & "<br>" & rtstr
            Return rtstr
        End Function
        Public Function jmakelist22(ByVal sql As String, ByVal con As SqlConnection) As String
            Dim dt As DataTableReader
            Dim dc As New dbclass
            Dim thisb As String = ""
            Dim norep() As String
            Dim rtstr As String = "Data base is new! No Data Inside"
            ' Dim sql = "select id," & fields & " from " & tblname
            Dim fc As Double
            If sql <> "" Then
                dt = dc.dtmake("NewQ", sql, con)

                If dt.HasRows = True Then
                    Dim i As Integer = 0
                    rtstr = "<form id='frmemplist' method='post' action='empcontener.aspx' target='_parent'><input type='hidden' id='datatake' name='datatake' />" & _
                    "<table stryle='font-size:10pt; width:1000px' cellspacing='0'>"
                    Dim color As String = "#E3EAEB"

                    fc = Math.Round((900 / dt.FieldCount))
                    ReDim norep(1)
                    norep(0) = "x"
                    While dt.Read
                        If i Mod 2 Then
                            color = "#E3EAEB"
                        Else
                            color = "#fefefe"
                        End If

                        If i = 0 Then

                            rtstr &= "<tr style='background-color:#243695;height:Auto;font-size:13pt;color:white;' >"
                            For j As Integer = 1 To dt.FieldCount - 4
                                rtstr &= "<td style='width:11%;'>" & dt.GetName(j) & "&nbsp;</td>"
                            Next

                            rtstr &= "</tr>"

                        End If

                        If searcharray(norep, dt.Item(0).ToString) = False Then
                            i = i + 1

                            ReDim Preserve norep(i + 1)
                            norep(i) = dt.Item(0).ToString
                            rtstr &= "<tr id='" & dt.Item(0) & "' style='height:13px;background-color:" & color & "; cursor:pointer;font-size:11pt;border:1px solid " & color & ";height:14px;' onclick=" & _
                                              Chr(34) & "javascript:orderpass('" & dt.Item(0) & "');" & Chr(34) & " onmouseover=" & Chr(34) & _
                                              "javascript: this.style.background='#00ff00';" & Chr(34) & " onmouseout=" & Chr(34) & _
                                              "javascript: this.style.background='" & color & "';" & Chr(34) & "> "

                            For j As Integer = 1 To dt.FieldCount - 4

                                If dt.Item(j).ToString = "y" Then
                                    rtstr &= "<td style='width:11%;'>Yes &nbsp;</td>"
                                ElseIf dt.Item(j).ToString = "n" Then
                                    rtstr &= "<td style='width:11%;'>No &nbsp;</td>"
                                ElseIf dt.GetName(j) = "department" Then
                                    rtstr &= "<td style='width:11%;'>" & _
                                    Me.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                                    dt.Item(j).ToString & "'", con) & "&nbsp;</td>"
                                ElseIf dt.GetName(j) = "type_recuritment" Then
                                    rtstr &= "<td style='width:11%;'>" & _
                                    Me.getinfo2("select type_recuritment from emprec where emp_id='" & dt.Item(0) & "' order by id desc", con).ToString & "</td>"

                                Else
                                    rtstr &= "<td style='width:11%;'>" & dt.Item(j).ToString & " &nbsp;</td>"
                                End If





                            Next
                            rtstr &= "</tr>"
                        End If
                    End While
                End If
                rtstr &= ("</table></form>")
                dt.Close()
            End If
            dt = Nothing
            dc = Nothing

            'Return sql & "<br>" & rtstr
            Return rtstr
        End Function
        Public Function searcharray(ByVal arrc() As String, ByVal key As Object) As Boolean
            Dim fm As New file_list
            Dim i As Integer
            For i = 0 To UBound(arrc)

                'If String.IsNullOrEmpty(arrc(i).ToString) = False Then
                If arrc(i) <> "" Then
                    If key.ToString = arrc(i).ToString Then
                        Return True
                    End If
                End If




            Next
            Return False
        End Function
        Public Function searcharray(ByVal arrc() As String, ByVal key As Object, ByVal cases As Boolean) As Boolean
            Dim fm As New file_list
            Dim i As Integer
            Select Case cases
                Case True
                    For i = 0 To UBound(arrc) - 1

                        'If String.IsNullOrEmpty(arrc(i).ToString) = False Then
                        If arrc(i) <> "" And String.IsNullOrEmpty(arrc(i)) = False Then
                            If key.ToString = arrc(i).ToString Then
                                Return True
                            End If
                        End If




                    Next
                Case False
                    For i = 0 To UBound(arrc) - 1

                        'If String.IsNullOrEmpty(arrc(i).ToString) = False Then
                        If arrc(i) <> "" And String.IsNullOrEmpty(arrc(i)) = False Then
                            If LCase(key.ToString) = LCase(arrc(i).ToString) Then
                                Return True
                            End If
                        End If

                    Next
            End Select

            Return False
        End Function
        Public Function getjavalist22(ByVal sqlx As String, ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection, ByVal sp As String) As String
            Dim db As New dbclass
            Dim sql As String
            Dim dt, dt2 As DataTableReader
            Dim retstr As String = ""
            Dim arrid(1) As String
            Dim disp() As String
            Dim dispn As Integer = 1
            disp = dis.Split(",")
            Dim optdis As String = ""
            If disp.Length > 1 Then
                dispn = disp.Length
            End If

            arrid(0) = ""
            Try
                dt2 = db.dtmake("check", sqlx, conx)
            Catch ex As Exception
                Return ex.ToString & "<br>" & sqlx
            End Try

            If dt2.HasRows Then
                While dt2.Read
                    If searcharray(arrid, dt2.Item("emp_id").trim) = False Then
                        ReDim Preserve arrid(arrid.Length + 1)
                        arrid(arrid.Length - 2) = dt2.Item("emp_id").trim

                        sql = "select " & dis & " from " & dbtabl
                        ' select emprec.* from emprec inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id order by emp_static_info.first_name, emprec.id desc 
                        sql &= " where (emp_id='" & dt2.Item("emp_id") & "')"
                        Try
                            dt = db.dtmake("dbtbl", sql, conx)
                        Catch ex As Exception
                            Return ex.ToString & "<br>" & sql
                        End Try


                        If dt.HasRows = True Then
                            dt.Read()
                            retstr &= Chr(34)

                            For i As Integer = 0 To disp.Length - 1
                                If dt.IsDBNull(i) = False Then
                                    If LCase(dt.GetDataTypeName(i)) = "string" Then
                                        retstr &= dt.Item(i).trim & sp
                                    Else
                                        retstr &= dt.Item(i).trim & sp
                                    End If
                                End If

                            Next
                            retstr &= Chr(34)
                            retstr &= ","
                            'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","


                        End If
                        dt.Close()
                    End If
                End While
                retstr &= Chr(34) & "xx" & Chr(34)
            End If
            dt2.Close()
            Return retstr
        End Function
        Public Function getrowinfo(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection, ByVal sp As String) As String
            Dim db As New dbclass
            Dim sql As String = "select  " & dis & " from " & dbtabl
            Dim dt As DataTableReader
            Dim retstr As String = ""
            Try
                dt = db.dtmake("dbtbl", sql, conx)
            Catch ex As Exception
                Return ex.ToString & "<br>" & sql
            End Try

            Dim disp() As String
            Dim dispn As Integer = 1
            disp = dis.Split(",")
            Dim optdis As String = ""

            If disp.Length > 1 Then
                dispn = disp.Length
            End If
            If dt.HasRows = True Then
                dt.Read()


                For i As Integer = 0 To disp.Length - 1
                    If dt.IsDBNull(i) = False Then
                        If LCase(dt.GetDataTypeName(i)) = "string" Then
                            retstr &= dt.Item(i).trim & sp
                        Else
                            retstr &= dt.Item(i) & sp
                        End If
                    End If

                Next


                'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","


            End If
            If retstr <> "" Then
                retstr = retstr.Substring(0, retstr.Length - 1)

            End If
            Return retstr
        End Function
        Public Function getjavalist2(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection, ByVal sp As String) As String
            Dim db As New dbclass
            Dim sql As String = "select  " & dis & " from " & dbtabl
            Dim dt As DataTableReader
            Dim retstr As String = ""
            Try
                dt = db.dtmake("dbtbl", sql, conx)
            Catch ex As Exception
                Return ex.ToString & "<br>" & sql
            End Try

            Dim disp() As String
            Dim dispn As Integer = 1
            disp = dis.Split(",")
            Dim optdis As String = ""

            If disp.Length > 1 Then
                dispn = disp.Length
            End If
            If dt.HasRows = True Then
                While dt.Read
                    retstr &= Chr(34)

                    For i As Integer = 0 To disp.Length - 1
                        If dt.IsDBNull(i) = False Then
                            If LCase(dt.GetDataTypeName(i)) = "string" Then
                                retstr &= dt.Item(i).trim & sp
                            Else
                                retstr &= dt.Item(i) & sp
                            End If
                        End If

                    Next

                    retstr &= Chr(34) & ","
                    'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
                End While

            End If
            retstr &= Chr(34) & "xx" & Chr(34)
            Return retstr
        End Function
        Public Function getjavanum(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection, ByVal sp As String) As String
            Dim db As New dbclass
            Dim sql As String = "select  " & dis & " from " & dbtabl
            Dim dt As DataTableReader
            Dim retstr As String = ""
            Try
                dt = db.dtmake("dbtbl", sql, conx)
            Catch ex As Exception
                Return ex.ToString & "<br>" & sql
            End Try

            Dim disp() As String
            Dim dispn As Integer = 1
            disp = dis.Split(",")
            Dim optdis As String = ""

            If disp.Length > 1 Then
                dispn = disp.Length
            End If
            If dt.HasRows = True Then
                While dt.Read
                    retstr &= ""

                    For i As Integer = 0 To disp.Length - 1
                        If dt.IsDBNull(i) = False Then
                            If LCase(dt.GetDataTypeName(i)) = "string" Then
                                retstr &= dt.Item(i).trim & sp
                            Else
                                retstr &= dt.Item(i) & sp
                            End If
                        End If

                    Next

                    retstr &= ""
                    'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
                End While

            End If
            retstr &= ""
            If retstr <> "" Then
                retstr = retstr.Substring(0, retstr.Length - 1)

            End If
            Return retstr
        End Function
        Public Function getjavalist(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection) As String
            If dis <> "" Then


                Dim db As New dbclass

                Dim sql As String = "select " & dis & " from " & dbtabl
                Dim dt As DataTableReader
                Dim retstr As String = ""
                dt = db.dtmake("dbtbl" & Today.ToLongTimeString, sql, conx)
                Dim disp() As String
                Dim dispn As Integer = 1
                disp = dis.Split(",")
                Dim optdis As String = ""
                If disp.Length > 1 Then
                    dispn = disp.Length
                End If
                If dt.HasRows = True Then
                    While dt.Read
                        For i As Integer = 0 To dispn - 1
                            If dt.IsDBNull(i) = False Then
                                If LCase(dt.GetDataTypeName(i)) = "string" Then
                                    retstr &= Chr(34) & dt.Item(i).trim & Chr(34) & " "
                                Else
                                    retstr &= Chr(34) & dt.Item(i) & Chr(34) & " "
                                End If
                            End If

                        Next
                        retstr &= ","
                        'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
                    End While
                    retstr &= Chr(34) & "xx" & Chr(34)
                End If
                Return retstr
            Else
                Return ""
            End If

        End Function
        Public Function getalllist(ByVal conx As SqlConnection, ByVal tbl As String, ByVal fshow As String) As String
            If conx.State = ConnectionState.Open Then
                conx.Close()
                conx.Open()
            End If
            Dim st As String
            Dim st2() As String = fshow.Split(",")
            If st2.Length = 1 Then
                st = "select distinct " & fshow & " from " & tbl.Replace("|", ",")
            Else
                st = "select " & fshow & " from " & tbl.Replace("|", ",")
            End If
            Dim dt As DataTableReader
            Dim dx As New dbclass
            Try
                dt = dx.dtmake("list" & Today.ToLongTimeString, st, conx)
                Dim rt As String = ""
                If dt.HasRows = True Then
                    While dt.Read
                        rt &= Chr(34)
                        For i As Integer = 0 To dt.FieldCount - 1
                            rt &= dt.Item(i) & " "
                        Next
                        rt &= Chr(34) & "," & Chr(13)
                    End While
                    Return rt.Substring(0, rt.Length - 2)
                End If

            Catch ex As Exception
                Return ex.ToString & "<br>" & Chr(34) & "none" & Chr(34) & st
            End Try

        End Function

        Public Function form(ByVal con As SqlConnection, ByVal tbl As String) As String
            Dim db As New dbclass
            Dim jv As String = ""
            Dim vart As String = "<script type=" & Chr(34) & "text/javascript" & Chr(34) & ">" & Chr(13) & "var prv;" & Chr(13) & "  prv=" & Chr(34) & Chr(34) & ";" & Chr(13) & _
            "var id;" & Chr(13) & _
  "var focused=" & Chr(34) & Chr(34) & ";" & Chr(13) & "var requf="
            Dim requf As String = "[" & Chr(34)
            Dim formx As String = ""
            Dim rs As DataTableReader
            rs = db.dtmake(tbl, "", con)
            Dim icount, i As Integer
            icount = rs.FieldCount
            Dim fldlist As String = ""
            jv = "function validation1(){"
            formx = "<form method='post' id='frm" & tbl & "' name='frm" & tbl & "'> " & Chr(13)
            formx = formx & "<table>"
            Dim tr As String = "off"

            For i = 1 To icount - 1
                fldlist &= "<br>" & rs.GetDataTypeName(i)
                If i Mod 2 = 1 Then
                    If tr = "on" Then
                        formx = formx & "</tr><tr>"
                    Else
                        formx = formx & "<tr>"
                        tr = "on"
                    End If
                End If

                formx = formx & "<td>" & rs.GetName(i) & "<sup style='color:red;'>*</sup></td><td>:</td>"
                formx = formx & "<td>"
                If rs.GetDataTypeName(i) = "Int32" Or rs.GetDataTypeName(i) = "Double" Then
                    formx &= "<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                    "'" & _
                                    " value='' onkeyup=""javascritp:alert(this.value());if(!isNaN(this.value()){showMessage('" & rs.GetName(i) & " cannot be empty','" & rs.GetName(i) & "');}"" />"
                Else
                    formx &= "<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                "'" & _
                " value='' />"
                End If

                formx &= "<br />"
                jv &= "if ($('#" & rs.GetName(i) & "').val() == '') {" & _
                "showMessage('" & rs.GetName(i) & " cannot be empty','" & rs.GetName(i) & "');" & _
                "$('#" & rs.GetName(i) & "').focus();" & _
                "return false;}" & Chr(13)
                requf &= rs.GetName(i) & Chr(34) & "," & Chr(34)
                If rs.GetName(i).StartsWith("pemail") = True Or rs.GetName(i).StartsWith("wemail") Or rs.GetName(i).StartsWith("email") Then
                    formx = formx & "<label class='lblsmall'>username@domain.com</label>"
                Else
                    formx = formx & "<label class='lblsmall'></label>"
                End If

                formx = formx & "</td>" & Chr(13)

                If rs.GetDataTypeName(i) = "DateTime" And rs.GetName(i) <> "date_reg" Then
                    formx = formx & "<script language='javascript' type='text/javascript'> " & _
                    "$(function() {" & _
   "$( " & Chr(34) & "#" & rs.GetName(i) & Chr(34) & ").datepicker({" & _
    "changeMonth: true," & _
   "changeYear: true" & _
 "	});" & _
 " $( " & Chr(34) & "#" & rs.GetName(i) & Chr(34) & " ).datepicker( " & Chr(34) & "option" & Chr(34) & _
                    "," & Chr(34) & "dateFormat" & Chr(34) & "," & Chr(34) & "mm/dd/yy" & Chr(34) & ");" & _
 "});</script>"
                End If

            Next
            ' jv &= "}</script>"
            If tr = "on" Then
                formx = formx & "</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />" & _
          "<input type='reset' onclick=" & Chr(34) & "javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" & Chr(34) & " /></td></tr>"
            End If
            requf = requf & "x" & Chr(34) & "];"
            vart &= requf & Chr(13) & "var fieldlist=" & requf
            formx = vart & Chr(13) & jv & "else if(focused==" & Chr(34) & Chr(34) & ") { var ans;" & Chr(13) & _
    "ans=checkblur();" & Chr(13) & _
    "if(ans!=true){ " & Chr(13) & _
        " $(" & Chr(34) & "#" & Chr(34) & " + ans).focus();" & Chr(13) & _
    "}else{" & Chr(13) & _
"   var str=$(" & Chr(34) & "#frm" & tbl & Chr(34) & ").formSerialize();" & Chr(13) & _
"   $(" & Chr(34) & "#frm" & tbl & Chr(34) & ").attr(" & Chr(34) & "action" & Chr(34) & "," & Chr(34) & "?tbl=" & tbl & "&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" & Chr(34) & " + str);" & Chr(13) & _
"    $(" & Chr(34) & "#frm" & tbl & Chr(34) & ").submit();" & Chr(13) & _
"  return true;}" & Chr(13) & _
  "  }" & Chr(13) & _
 "} </script>" & formx & "</table></form>"
            rs.Close()
            Return formx & fldlist
        End Function
        Public Function getoption(ByVal dbtabl As String, ByVal val As String, ByVal dis As String, ByVal conx As SqlConnection) As String
            Dim opt As String = ""
            Dim db As New dbclass
            Dim rs As DataTableReader
            Dim sql As String = ""
            Try



                If val = dis Then
                    Sql = "select distinct " & val & " from " & dbtabl
                Else
                    Sql = "select distinct " & val & "," & dis & " from " & dbtabl
                End If
                Dim disp() As String
                Dim dispn As Integer = 0
                disp = dis.Split(",")
                Dim optdis As String = ""
                If disp.Length > 1 Then
                    dispn = disp.Length
                End If
                rs = db.dtmake(dbtabl & Today.ToLongTimeString, Sql, conx)
                If rs.HasRows = True Then
                    Dim ck As String = ""
                    While rs.Read
                        If ck <> rs.Item(val) Then
                            ck = rs.Item(val)

                            If rs.FieldCount = 1 Then
                                If LCase(rs.GetDataTypeName(0)) = "string" Then
                                    opt &= "<option value='" & rs.Item(val).trim & "'>" & rs.Item(val) & "</option>" & Chr(13)
                                Else
                                    opt &= "<option value='" & rs.Item(val) & "'>" & rs.Item(val) & "</option>" & Chr(13)
                                End If
                                '  opt &= "<option value='" & rs.Item(val) & "'>" & rs.Item(val) & "</option>" & Chr(13)
                            Else

                                If LCase(rs.GetDataTypeName(0)) = "string" Then
                                    opt &= "<option value='" & rs.Item(val).trim & "'>"
                                Else
                                    opt &= "<option value='" & rs.Item(val) & "'>"
                                End If
                                optdis = ""
                                For c As Integer = 1 To rs.FieldCount - 1
                                    optdis &= rs.Item(c) & " "
                                Next
                                opt &= optdis & "</option>" & Chr(13)



                                ' opt &= "<option value='" & rs.Item(val) & "'>" & rs.Item(dis) & "</option>" & Chr(13)
                            End If
                        End If
                    End While
                End If
                Return opt
            Catch ex As Exception
                Return sql & ex.ToString
            End Try
        End Function
        Public Function filldata(ByVal sql As String, ByVal tbl As String, ByVal con As SqlConnection) As String
            Dim retst As String = ""
            Return retst
        End Function
        Public Function fdb(ByVal pathx As String, ByVal conx As SqlConnection) As String
            Dim str As String = ""
            Dim str2 As String = ""
            If File.Exists(pathx) Then
                Dim lin() As String
                lin = File.ReadAllLines(pathx)
                Dim db As New dbclass
                For Each l As String In lin
                    str &= "<br />" & l
                    If l.Length > 5 Then
                        If l.Substring(0, 6).ToLower = "insert" Then
                            ' str = File.ReadAllText(pathx)

                            db.save(l, conx, pathx)
                        Else
                            If String.IsNullOrEmpty(l) = False Then
                                str2 = "insert into tblposition(position) values('" & l & "')"
                                Try
                                    db.save(str2, conx, pathx)
                                Catch ex As Exception
                                    Continue For
                                End Try


                            End If
                        End If
                    End If
                Next
                If str2 <> "" Then

                    File.WriteAllText("F:\kirold\WebSite2\diff data\text.txt", str2)
                End If
                db = Nothing
            End If
            Return str
        End Function
        Public Function calc(ByVal conx As SqlConnection, ByVal empid As String, ByVal emptid As Integer, ByVal pathx As String)
            'Response.Write("called")
            Dim dbx As New dbclass
            Dim fm As New formMaker
            Dim ks As New kirsoftsystem
            Dim save_err As String = ""
            Dim dt As DataTableReader
            Dim dt2 As DataTableReader
            ' Dim dt3 As DataTableReader
            Dim strout As String = ""
            Dim stravl As String = ""
            Dim strexp As String = ""
            Dim intavl As Integer = 0
            Dim intexp As Integer = 0
            ' Dim conx As SqlConnection = con
            ' Dim empid As String = Session("emp_id")
            Dim i As Integer = 0
            Dim dyr As Double
            Dim sql As String = ""
            dt = dbx.dtmake("newdata" & Today.ToLongDateString, "select hire_date from emprec where emp_id='" & empid & "' and end_date is Null order by id desc", conx)
            dt2 = dbx.dtmake("new2" & Today.ToLongDateString, "select no_days,year_end,user_rec_date,emptid from emp_leave_info where emp_id='" & empid & "' and emptid=" & emptid & " order by id DESC", conx)

            If dt.HasRows = True Then
                dt.Read()
                Dim d_hire As Date
                Dim d2 As String

                d_hire = dt.Item("hire_date")
                If dt2.HasRows = True Then
                    dt2.Read()
                    If dt2.Item("user_rec_date") = "n" Then
                        d2 = dt2.Item("year_end")
                        Dim x1 As Integer = d_hire.Year
                        Dim n_days As Integer = dt2.Item("no_days")
                        dyr = ((Today.Subtract(d_hire).Days / 30.41) / 12) ' - x1
                        ' Response.Write(dyr)
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
                        ye = y_end
                        'Response.Write("<br>" & d_hire.ToString & "===>" & y_end.ToString)
                        ' Response.Write("<br>m1=" & m1 & " and m2=" & m2 & "<br>")
                        i = 0
                        Dim data_c_c As Integer = 0
                        Dim nnd As Integer
                        Do
                            If i = 0 Then
                                nnd = n_days + i
                            Else
                                nnd = n_days + i - 1
                            End If
                            rate = (nnd) / 12
                            dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)

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
                                dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emp_id='" & empid & "' and l_s_year='" & hd.Year.ToString & "/" & hd.Month.ToString & "/" & hd.Day.ToString & " 00:00:00' and l_e_year='" & ye.Year.ToString & "/" & ye.Month.ToString & "/" & ye.Day.ToString & " 00:00:00'", conx)
                                If dt4.HasRows = True Then
                                    'strout &= ("<tr><td>has rowwww</td></tr>")
                                Else
                                    sql = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period,emptid) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye, "MM/DD/YYYY").ToString & "'," & dbgt.ToString & "," & emptid.ToString & ")"
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
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
                            ye = d_hire.AddYears(i + 1).AddDays(-1)
                            dbgt = nnd - dbgt '(Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)
                            If Today.Subtract(ye).Days <= 0 Then
                                davail = (Math.Round(((Today.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)
                            Else
                                davail = dbgt
                            End If
                            If davail < 0 Then
                                davail = 0
                            End If
                            If dbgt <> 0 And davail <> 0 Then
                                dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emp_id='" & empid & "' and l_s_year='" & hd.Year.ToString & "/" & hd.Month.ToString & "/" & hd.Day.ToString & " 00:00:00' and l_e_year='" & ye.Year.ToString & "/" & ye.Month.ToString & "/" & ye.Day.ToString & " 00:00:00'", conx)
                                If dt4.HasRows = True Then
                                    'strout &= ("<tr><td>has rowwww</td></tr>")
                                Else
                                    sql = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period,emptid) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye.ToString, "MM/DD/YYYY") & "'," & dbgt.ToString & "," & emptid & ")"
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
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
                        Loop Until Today.Subtract(ye).Days / 30.41 < -2


                        If data_c_c > 0 Then
                            strout &= data_c_c.ToString & " new Data has been added"
                        Else
                            strout &= "No Change in the database"
                        End If
                        ' strout = strout.Length.ToString
                    Else
                        strout = "No Calculation"
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
        Public Function view_leave_bal(ByVal companyname As String, ByVal emptid As String, ByVal empid As String, ByVal con As SqlConnection)
            Dim db As New dbclass
            'Dim dt As DataTableReader
            '  Dim loc As String
            Dim noav As Double = 0
            Dim nobgt As Double = 0
            Dim nobal As Double = 0
            Dim nouse As Double = 0
            Dim strview As String = ""
            Dim nod As Integer = 0
            Dim dt2 As DataTableReader
            Dim sumavx As Double
            Dim cout, give, v As Double
            Dim outp As String
            Dim sumav As Double
            'Dim dx As Date
            'Dim r() As Object
            Dim byhalf As String = "n"
            sumav = 0

            'Dim rid As Integer
            Dim bgt, nd As String
            Dim req As String
            Dim redirect As String = ""
            'Dim appdate As Date
            Dim finof As String = ""
            Dim res As String = ""
            Dim Sql As String = ""
            give = 0
            bgt = ""
            nd = ""
            req = ""
            Dim lucur(3) As String
            Dim sdate As String = ""
            Dim i As Integer = 0
            'Dim rqid As Integer
            Dim days(3) As String
            Dim bald(3) As String
            Sql = "select * from show_leave_bal where emp_id='" & empid & "' and emptid=" & emptid
            '  Sql = "select * from show_leave_bal where emp_id='" & empid & "' and emptid=" & emptid

            dt2 = db.dtmake("thscals", Sql, con)

            If dt2.HasRows = True Then
                strview = "<table cellspacing='0' cellpadding='0' width='900px'>" & _
            "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
                For j As Integer = 1 To dt2.FieldCount - 1
                    strview &= "<td  style='padding-right:10px;'>" & dt2.GetName(j) & "</td>"
                Next
                strview &= "</tr>"
                Dim color As String = "E3EAEB"
                ' Dim remx As Double

                cout = 0
                sumavx = 0
                While dt2.Read

                    If color <> "#E3EAEB" Then
                        color = "#E3EAEB"
                    Else
                        color = "#fefefe"
                    End If
                    If isexp(Today.ToShortDateString, dt2.Item("Year End"), 2, "y") Then
                        color = "red"
                    Else
                        v = showavdate(dt2.Item("Year Start"), dt2.Item("Year End"), dt2.Item("Balance"))
                        noav = noav + v
                        nobgt = nobgt + dt2.Item("Budget")
                        nobal = nobal + dt2.Item("Balance")
                        nouse = nouse + dt2.Item("Used")
                        Dim d1, d2 As Date
                        d1 = dt2.Item("Year Start")
                        d2 = dt2.Item("Year End")
                        If cout < 1 Then

                            days(cout) = MonthName(d2.Month) & " " & d2.Day & ", " & d2.Year
                            bald(cout) = dt2.Item("Budget")
                        Else
                            days(cout) = MonthName(d2.Month) & " " & d2.Day & ", " & d2.Year
                            bald(cout) = v
                        End If
                        cout += 1
                        If give > 0 Then

                            ' dx = dt.Item("date_taken_from")
                            If give <= v Then

                                bgt &= dt2.Item("id") & ","
                                nd &= give.ToString & ","
                                give = give - v
                            Else
                                bgt &= dt2.Item("id") & ","
                                nd &= v.ToString & ","
                                give = give - v
                            End If
                        End If
                    End If
                    strview &= "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                    For i = 1 To dt2.FieldCount - 1
                        If dt2.IsDBNull(i) = False Then
                            strview &= "<td  style='padding-right:10px;'>" & dt2.Item(i) & "</td>"
                        Else
                            strview &= "<td>&nbsp;</td>"
                        End If
                    Next
                    strview &= "</tr>"

                End While
                strview &= "<tr style='font-weight:bold;' bgcolor='gray'><td colspan='4' align='middle'>Summary</td><td>" & nobgt.ToString & "</td><td>" & nouse.ToString & "</td><td>" & nobal & "</td></tr>"
            End If
            ' Dim r() As Object
            ' r = calcwhen(dt.Item("date_taken_from"), dt.Item("no_days"), empid, con, byhalf)
            strview &= "</table>"
            outp = "<div id='uproved'><table><tr><td colspan='6'>" & _
           "</td></tr><tr><td>" & _
          "Total Bagdeted:</td><td>" & nobgt.ToString & "</td><td>No Days Used: </td><td>" & _
          CInt(nouse).ToString & _
         "</td></tr><tr><td>No. Available Days:</td><td>" & CInt(noav).ToString & "</td></tr>"

            outp &= " </td></tr></table> "
            outp &= "</div>"
            outp &= "<div id=" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('uproved','Report_print','" & companyname & "<BR> Leave Summery','" & Today.ToLongDateString & "');" & Chr(34) & ">print</div>"
            Dim vew(3, 3) As String
            vew(0, 0) = strview & outp
            vew(1, 0) = bald(0)
            vew(1, 1) = bald(1)
            vew(1, 2) = bald(2)
            vew(2, 0) = days(0)
            vew(2, 1) = days(1)
            vew(2, 2) = days(2)
            vew(0, 1) = noav.ToString
            vew(0, 2) = nouse.ToString
            dt2.Close()
            db = Nothing
            Return vew
        End Function
        Public Function view_leave_bal2(ByVal companyname As String, ByVal emptid As String, ByVal empid As String, ByVal ridx As String, ByVal con As SqlConnection)
            Dim db As New dbclass
            Dim dt As DataTableReader
            Dim dt2 As DataTableReader
            'dt2 = db.dtmake("dbval", "select * from  show_leave_bal where emptid=" & emptid, con)
            dt2 = db.dtmake("dbbb", "select * from emp_leave_take where emptid=" & emptid & " and id<=" & ridx & " order by id", con)
            If dt2.HasRows Then
                While dt2.Read

                End While
            End If
            dt2.Close()
            db = Nothing



        End Function
        Public Function calc2(ByVal conx As SqlConnection, ByVal empid As String, ByVal emptid As Integer, ByVal pathx As String)
            'Response.Write("called")
            Dim dbx As New dbclass
            Dim fm As New formMaker
            Dim ks As New kirsoftsystem
            Dim save_err As String = ""
            Dim dt As DataTableReader
            Dim dt2 As DataTableReader
            ' Dim dt3 As DataTableReader
            Dim strout As String = ""
            Dim stravl As String = ""
            Dim strexp As String = ""
            Dim intavl As Integer = 0
            Dim intexp As Integer = 0
            ' Dim conx As SqlConnection = con
            ' Dim empid As String = Session("emp_id")
            Dim i As Integer = 0
            Dim dyr As Double
            Dim sql As String = ""

            dt = dbx.dtmake("newdatax" & Today.ToLongDateString, "select hire_date from emprec where emp_id='" & empid & "' and id=" & emptid.ToString & " and end_date is Null order by id desc", conx)
            dt2 = dbx.dtmake("new2" & Today.ToLongDateString, "select no_days,year_end,user_rec_date,emptid from emp_leave_info where emp_id='" & empid & "' and emptid=" & emptid & " order by id DESC", conx)

            If dt.HasRows = True Then
                dt.Read()
                Dim d_hire As Date
                Dim d2 As String
                d_hire = dt.Item("hire_date")
                If dt2.HasRows = True Then
                    dt2.Read()
                    If dt2.Item("user_rec_date") = "n" Then
                        d2 = dt2.Item("year_end")
                        Dim x1 As Integer = d_hire.Year
                        Dim n_days As Integer = dt2.Item("no_days")
                        dyr = ((Today.Subtract(d_hire).Days / 30.41) / 12) ' - x1
                        ' Response.Write(dyr)
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
                                    If dbgt > 30 Then
                                        dbgt = 30
                                    End If
                                    sql = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period,emptid) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye, "MM/DD/YYYY").ToString & "'," & dbgt.ToString & "," & emptid.ToString & ")"
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
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
                        ' strout = strout.Length.ToString
                    Else
                        d2 = dt2.Item("year_end")
                        Dim x1 As Integer = d_hire.Year
                        Dim n_days As Integer = dt2.Item("no_days")
                        dyr = ((Today.Subtract(d_hire).Days / 30.41) / 12) ' - x1
                        ' Response.Write(dyr)
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
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
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
            Dim fl As File
            If fl.Exists(HttpContext.Current.Session("path") & "kst/maxdate.ks") Then
                Dim rdfl() As String = fl.ReadAllLines(HttpContext.Current.Session("path") & "kst/maxdate.ks")
                mxdate = rdfl(0)
            Else
                mxdate = 30
            End If
            ' Dim conx As SqlConnection = con
            ' Dim empid As String = Session("emp_id")
            Dim i As Integer = 0
            Dim dyr As Double
            Dim sql As String = ""

            dt = dbx.dtmake("newdatax" & Today.ToLongDateString, "select hire_date from emprec where emp_id='" & empid & "' and id=" & emptid.ToString & " and end_date is Null order by id desc", conx)
            dt2 = dbx.dtmake("new2" & Today.ToLongDateString, "select no_days,year_end,user_rec_date,emptid from emp_leave_info where emp_id='" & empid & "' and emptid=" & emptid & " order by id DESC", conx)

            If dt.HasRows = True Then
                dt.Read()
                Dim d_hire As Date
                Dim d2 As String
                d_hire = dt.Item("hire_date")
                If dt2.HasRows = True Then
                    dt2.Read()
                    If dt2.Item("user_rec_date") = "n" Then
                        d2 = dt2.Item("year_end")
                        Dim x1 As Integer = d_hire.Year
                        Dim n_days As Integer = dt2.Item("no_days")
                        dyr = ((Today.Subtract(d_hire).Days / 30.41) / 12) ' - x1
                        ' Response.Write(dyr)
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
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
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
                        ' strout = strout.Length.ToString
                    Else
                        d2 = dt2.Item("year_end")
                        Dim x1 As Integer = d_hire.Year
                        Dim n_days As Integer = dt2.Item("no_days")
                        dyr = ((Today.Subtract(d_hire).Days / 30.4375) / 12) ' - x1
                        ' Response.Write(dyr)
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
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
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
        Public Function leavebugetcal(ByVal conx As SqlConnection, ByVal empid As String, ByVal pathx As String) As String
            Dim dbx As New dbclass
            Dim ks As New kirsoftsystem
            Dim save_err As String = ""
            Dim dt As DataTableReader
            Dim dt2 As DataTableReader
            ' Dim dt3 As DataTableReader
            Dim strout As String = ""
            Dim stravl As String = ""
            Dim strexp As String = ""
            Dim intavl As Integer = 0
            Dim intexp As Integer = 0
            Dim i As Integer = 0
            Dim dyr As Double
            Dim sql As String = ""
            dt = dbx.dtmake("newdata" & Today.ToLongDateString, "select hire_date from emprec where emp_id='" & empid & "' and end_date is Null order by id desc", conx)
            dt2 = dbx.dtmake("new2" & Today.ToLongDateString, "select no_days,year_end,user_rec_date from emp_leave_info where emp_id='" & empid & "' order by id DESC", conx)

            If dt.HasRows = True Then
                dt.Read()
                Dim d_hire As Date
                Dim d2 As String

                d_hire = dt.Item("hire_date")
                If dt2.HasRows = True Then
                    dt2.Read()
                    If dt2.Item("user_rec_date") = "n" Then
                        d2 = dt2.Item("year_end")

                        Dim x1 As Integer = d_hire.Year

                        Dim n_days As Integer = dt2.Item("no_days")
                        dyr = ((Today.Subtract(d_hire).Days / 30.41) / 12) ' - x1
                        ' Response.Write(dyr)
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
                        ye = y_end
                        'Response.Write("<br>" & d_hire.ToString & "===>" & y_end.ToString)
                        ' Response.Write("<br>m1=" & m1 & " and m2=" & m2 & "<br>")
                        strout &= "<div style='clear:left;'><table class='tbl1' style='background:#ffccaa; border:solid 1px blue;' cellpadding='7' cellspacing='0' border='1'><tr style='font-weight:bold; font-size:14pt;'><td>From</td><td>To </td><td>Budgeted date</td><td>Available Date</td></tr>"
                        i = 0
                        Do
                            Dim nnd As Integer = n_days + i
                            rate = (nnd) / 12
                            dbgt = (Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)

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
                                dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emp_id='" & empid & "' and l_s_year='" & hd.Year.ToString & "/" & hd.Month.ToString & "/" & hd.Day.ToString & " 00:00:00' and l_e_year='" & ye.Year.ToString & "/" & ye.Month.ToString & "/" & ye.Day.ToString & " 00:00:00'", conx)
                                If dt4.HasRows = True Then
                                    strout &= ("<tr><td>has rowwww</td></tr>")
                                Else
                                    sql = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye, "MM/DD/YYYY").ToString & "'," & dbgt.ToString & ")"
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
                                    If save_err <> "1" Then
                                        strout &= "<tr><td colspan='6'>Save error</td></tr>"
                                    End If
                                End If

                                If isexp(Today.ToShortDateString, ye, 2, "y") Then
                                    strout &= ("<tr style='background:#0000ff;'>")
                                    intexp += davail 'send expire list
                                Else
                                    strout &= ("<tr>")
                                    intavl += davail 'send avail list
                                End If
                                strout &= ("<td>" & hd.ToShortDateString & "</td><td>" & ye.ToShortDateString & "</td>")
                                strout &= ("<td>" & dbgt & "</td><td>")
                                strout &= (davail & "</td></tr>")
                            End If
                            hd = ye.AddDays(1)
                            ye = d_hire.AddYears(i + 1).AddDays(-1)
                            dbgt = nnd - dbgt '(Math.Round(((ye.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)
                            If Today.Subtract(ye).Days <= 0 Then
                                davail = (Math.Round(((Today.Subtract(hd.ToShortDateString).Days) / 30) * (rate)).ToString)
                            Else
                                davail = dbgt
                            End If
                            If davail < 0 Then
                                davail = 0
                            End If
                            If dbgt <> 0 And davail <> 0 Then
                                dt4 = dbx.dtmake("new3", "select l_e_year,l_s_year,no_days_with_period from emp_leave_budget where emp_id='" & empid & "' and l_s_year='" & hd.Year.ToString & "/" & hd.Month.ToString & "/" & hd.Day.ToString & " 00:00:00' and l_e_year='" & ye.Year.ToString & "/" & ye.Month.ToString & "/" & ye.Day.ToString & " 00:00:00'", conx)
                                If dt4.HasRows = True Then
                                    'strout &= ("<tr><td>has rowwww</td></tr>")
                                Else
                                    sql = "insert into emp_leave_budget(emp_id,l_s_year,l_e_year,no_days_with_period) values('" & empid & "','" & ks.dateconv(hd, "MM/DD/YYYY") & "','" & ks.dateconv(ye.ToString, "MM/DD/YYYY") & "'," & dbgt.ToString & ")"
                                    'strout &= ("<tr><td colspan='6'>" & sql & "</td></tr>")
                                    save_err = dbx.save(sql, conx, pathx).ToString
                                    If save_err <> "1" Then
                                        strout &= "<tr><td colspan='6'>Save error</td></tr>"
                                    End If
                                End If
                                If isexp(Today.ToShortDateString, ye, 2, "y") Then
                                    strout &= ("<tr style='background:#0000ff;'>")
                                    intexp += davail 'send expire list
                                Else
                                    strout &= ("<tr>")
                                    intavl += davail 'send avail list
                                End If
                                strout &= ("<td>" & hd.ToShortDateString & "</td><td>" & ye.ToShortDateString & "</td>")
                                strout &= ("<td>" & dbgt & "</td><td>")
                                strout &= (davail & "</td></tr>")
                            End If
                            hd = ye.AddDays(1)
                            ye = y_end.AddYears(i + 1)
                            i = i + 1
                        Loop Until Today.Subtract(ye).Days / 30.41 < -1


                        strout &= ("</table></div>")
                    Else

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

        Public Function isexp(ByVal date1 As Date, ByVal date2 As Date, ByVal exptime As Integer, ByVal consider As String) As Object
            Dim dtdiff As Double
            Select Case LCase(consider)
                Case "m"
                    dtdiff = date1.Subtract(date2).Days / 30
                    If dtdiff < 0 Then
                        dtdiff = dtdiff * -1
                    End If
                    If exptime < dtdiff Then
                        Return True
                    End If
                Case "y"
                    dtdiff = (date1.Subtract(date2).Days / 30) / 12
                    If dtdiff < 0 Then
                        dtdiff = dtdiff * -1
                    End If
                    If exptime < dtdiff Then
                        Return True
                    End If
                Case "d"
                    dtdiff = date1.Subtract(date2).Days
                    If dtdiff < 0 Then
                        dtdiff = dtdiff * -1
                    End If
                    If exptime < dtdiff Then
                        Return True
                    End If
                Case Else
                    Return False
            End Select
            Return False
        End Function
        Public Function isexpforward(ByVal date1 As Date, ByVal date2 As Date, ByVal exptime As Integer, ByVal consider As String) As Object
            Dim dtdiff As Double
            Select Case LCase(consider)
                Case "m"
                    dtdiff = date1.Subtract(date2).Days / 30
                    If exptime > dtdiff Then
                        Return True
                    End If
                Case "y"
                    dtdiff = (date1.Subtract(date2).Days / 30) / 12

                    If exptime > dtdiff Then
                        Return True
                    End If
                Case "d"
                    dtdiff = date1.Subtract(date2).Days

                    If exptime < dtdiff Then
                        Return True
                    End If
                Case Else
                    Return False
            End Select
            Return False
        End Function
        Public Function popupwindow(ByVal id As String, ByVal title As String, ByVal cont As String)
            Dim str As String
            str = "<div id=" & Chr(34) & CStr(id) & Chr(34) & "class='popup' title='" & title & "'>" & _
            "<div style=" & Chr(34) & "height:30px; background:url(images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & ">" & Chr(13) & _
            "<div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:90%; float:left; left:2px;position:relative;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & "><b>" & _
            CStr(title) & "</b></div>" & Chr(13) & _
            "<div id='btncls' style=" & Chr(34) & "cursor:pointer; text-align:left; color:red; height:30px; width:10%; background-image:url(images/gif/xx.gif) no-repeat; float:right;" & Chr(34) & " title='close'  onclick=" & Chr(34) & "javascript: document.getElementById('" & CStr(id) & "').style.visibility='hidden';" & Chr(34) & _
                        ">&nbsp;close </div></div>" & Chr(13) & "<br /><br />" & _
                   "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
                   ">&nbsp;&nbsp;" & CStr(cont) & "</div>" & _
            "</div>"
            Return str
        End Function
    End Class

    Public Class menubuilder
        Public pathxe As String = "e:\" '        Implements CodeDom.Compiler ' Public Shared this As New menubuilder
        Public Function mainmenu(ByVal conx As SqlConnection, ByVal url As String, ByVal auth() As String) As Array
            Dim sqlrs As DataTableReader
            Dim submenux As String
            Dim rstring As String
            Dim db As New dbclass
            Dim ret() As String = {"", "", "", ""}
            submenux = "0"
            rstring = ""
            'MsgBox(url)
            sqlrs = db.dtmake("menu", "SELECT * from menu where publish='y' order by menu_order", conx)
            If sqlrs.HasRows = False Then
                sqlrs.Close()
            Else
                Dim x As String
                Dim i, j As Integer
                Dim link(1) As String
                Dim arry() As String
                Dim m(1) As String
                Dim sm(1) As String
                Dim menu(1) As String
                submenux = ""
                i = 0
                j = 0
                m(0) = ""
                sm(0) = ""
                link(0) = ""
                menu(0) = ""
                While sqlrs.Read
                    If arrcomp(sqlrs.Item("Id"), auth) = True Then
                        x = sqlrs.Item("menu_id")
                        If x.ToString.Length = 2 Then
                            ReDim Preserve m(i + 1)
                            ReDim Preserve menu(i + 1)
                            ReDim Preserve link(i + 1)
                            m(i) = CStr(x)
                            menu(i) = sqlrs.Item("menu_name")
                            If sqlrs.IsDBNull(4) = False Then
                                link(i) = sqlrs.Item("menu_link")
                            Else
                                link(i) = ""
                            End If
                            i += 1
                        End If

                        If x.ToString.Length > 2 And Array.BinarySearch(sm, x.ToString.Substring(0, 2)) < 0 Then
                            ReDim Preserve sm(j + 1)
                            sm(j) = x.ToString.Substring(0, 2)
                            'MsgBox(sm(j))
                            j += 1
                        End If
                        ' MsgBox(CStr(Array.BinarySearch(auth, CStr(sqlrs.Item("Id")))) & "--->" & sqlrs.Item("menu_name"))

                    End If
                End While

                arry = Split(url, "/")
                rstring += "<span style='opacity:0.1;filter:alpha(opacity=10); baground:transparent;'>&nbsp;</span>" & Chr(13)
                For i = 0 To UBound(m) - 1
                    rstring += "<span  class=" + Chr(34) + "no-border" + Chr(34) + ">" & Chr(13)
                    If Array.BinarySearch(sm, m(i)) < 0 Then
                        If InStr(link(i), arry(UBound(arry))) > 0 Then
                            submenux = m(i)
                            rstring += "<a id=" & Chr(34) & "m" & CStr(m(i)) & Chr(34) & " href=" + Chr(34) + link(i) + Chr(34) & _
                            " style=" + Chr(34) + "background-color:#254080" + Chr(34) & " target='frm_tar'>" + menu(i) + "</a></span>" & Chr(13)
                            ret(2) = menu(i)
                            ret(3) = link(i) '0924828505
                        Else
                            rstring += "<a id=" & Chr(34) & "m" & CStr(m(i)) & Chr(34) & " href=" + Chr(34) + link(i) + Chr(34) & _
                            " target='frm_tar'>" + menu(i) & _
                            "</a></span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>" & Chr(13)
                        End If
                    Else
                        If InStr(link(i), arry(UBound(arry))) > 0 Then
                            submenux = m(i)
                            rstring += "<a id=" & Chr(34) & "m" & CStr(m(i)) & Chr(34) & " href=" + Chr(34) + link(i) + Chr(34) & _
                            " style=" + Chr(34) + "background-color:#ff0000" + Chr(34) + " onmouseover=" & Chr(34) & _
                            "javascript:showobjar('m" & CStr(m(i)) & "','sub" & CStr(m(i)) & "',0,20);" & Chr(34) & _
                            " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(m(i)) & "');" & Chr(34) & _
                            " onclick=" & Chr(34) & "javascript:ha('sub" & CStr(m(i)) & "');" & Chr(34) & " target='frm_tar'>" + menu(i) + "</a></span>" & Chr(13)
                            ret(2) = menu(i)
                            ret(3) = link(i) '0924828505
                        Else
                            rstring += "<a id=" & Chr(34) & "m" & CStr(m(i)) & Chr(34) & " href=" + Chr(34) + link(i) + Chr(34) & _
                            " onmouseover=" & Chr(34) & "javascript:showobjar('m" & CStr(m(i)) & "','sub" & CStr(m(i)) & "',0,20);" & _
                            Chr(34) & " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(m(i)) & "');" & Chr(34) & _
                            " Onclick=" & Chr(34) & "javascript:ha('sub" & CStr(m(i)) & "');" & Chr(34) & " target='frm_tar'>" + menu(i) & _
                            "</a></span>" & Chr(13)
                        End If
                    End If
                Next


            End If
            sqlrs.Close()
            'rstring += " </>"
            ret(0) = rstring
            ret(1) = CStr(submenux)
            Return ret
        End Function
        Public Function groupmenu(ByVal con As SqlConnection, ByVal auth() As String)
            Dim dbx As New dbclass
            Dim rs, rs2 As DataTableReader
            Dim rtn As String = ""
            rs = dbx.dtmake("submenu", "select * from menu where menu_id>0 and menu_id<=99 and publish='y'", con)
            If rs.HasRows Then
                Dim hmenu As String
                While rs.Read
                    hmenu = rs.Item("menu_id")
                    rs2 = dbx.dtmake("summ", "select * from menu where menu_id like '" & hmenu & "%' and publish='y' order by groupby", con)
                    rtn &= "<div class='submenukir' id='sub" & rs.Item("menu_id") & "' style='visibility:hidden; left:0px; top:0px; position:absolute;background-color:white;border-left:gray 1px solid;border-right:black 1px solid; border-bottom:black 1px solid;'" & _
                                        " onmouseover=" & Chr(34) & "javascript:showobjar('m" & rs.Item("menu_id") & "','sub" & rs.Item("menu_id") & "',0,18);" & Chr(34) & _
                                        " onmouseout=" & Chr(34) & "javascript:ha('sub" & rs.Item("menu_id") & "');" & Chr(34) & ">" & Chr(13)

                    If rs2.HasRows Then
                        Dim ot As String = ""
                        Dim gr As String = ""
                        Dim cc1 As Integer = 4
                        Dim cc As Integer
                        While rs2.Read

                            If rs2.IsDBNull(8) Then
                                ot = "Other"
                            Else
                                ot = rs2.Item("groupby")
                            End If
                            If gr = "" Then
                                gr = ot
                                If cc = 0 Or cc >= 4 Then
                                    rtn &= "<span class='newc'>" & Chr(13)
                                    cc = 0
                                End If
                                rtn &= "<ul>" & gr
                            ElseIf gr <> ot Then

                                rtn &= "</ul>"
                                If cc >= 4 Then
                                    cc = 0
                                    rtn &= "</span><span class='newc'>" & Chr(13)

                                End If
                                gr = ot
                                rtn &= "<ul>" & gr
                            Else
                                cc += 1
                                If arrcomp(rs2.Item("Id"), auth) = True Then
                                    rtn &= "<li style='color:gray; decorection:none;'>"

                                    rtn &= " <a class=" + Chr(34) + "menu_a" + Chr(34) & "href='" & CStr(rs2.Item("menu_link")) & _
             "?sub=" & rs2.Item("menu_id") & "' style='height:auto;'onmouseover=" & Chr(34) & "javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" & Chr(34) & _
                                " onmouseout=" & Chr(34) & "this.style.background='white';this.style.color='gray'" & Chr(34) & "onclick=" & Chr(34) & "javascript:document.getElementById('frm_tar').src='" & rs2.Item("menu_link") & "';ha('sub" & (rs.Item("menu_id")) & "'); " & Chr(34) & " target='frm_tar'>" & rs2.Item("menu_name") & "</a>" & _
                       "</li>" & Chr(13)
                                Else
                                    rtn &= "<li>" & Chr(13) & rs2.Item("menu_name") & "</li>" & Chr(13)
                                End If
                            End If

                        End While
                    End If
                    rtn &= "</ul></span></div>"
                End While
            End If
            Return rtn
        End Function
        Public Function mainmenu2(ByVal conx As SqlConnection, ByVal url As String, ByVal auth() As String) As String
            Dim sqlrs As DataTableReader
            Dim submenux As String
            Dim rstring As String
            Dim db As New dbclass
            Dim ret() As String = {"", "", "", ""}
            submenux = "0"
            rstring = ""
            'MsgBox(url)
            sqlrs = db.dtmake("menuxx", "SELECT * from menu where publish='y' order by menu_order", conx)
            If sqlrs.HasRows = False Then
                sqlrs.Close()
            Else
                Dim x As String
                Dim i, j As Integer
                Dim link As String
                submenux = ""
                i = 0
                j = 0
                link = ""
                rstring += "<span style='opacity:0.1;filter:alpha(opacity=10); baground:transparent;'>&nbsp;</span>" & Chr(13)

                While sqlrs.Read
                    If arrcomp(sqlrs.Item("Id"), auth) = True Then
                        x = sqlrs.Item("menu_id")
                        If x.ToString.Length = 2 Then
                            If sqlrs.IsDBNull(4) = False Then
                                link = sqlrs.Item("menu_link")
                            Else
                                link = ""
                            End If
                            submenux = sqlrs.Item("menu_name")

                            rstring += "<span  class=" + Chr(34) + "no-border" + Chr(34) + ">" & Chr(13)
                            If isHasSubMenu(conx, x) = True Then
                                If link <> "" Then
                                    rstring += "<a id=" & Chr(34) & "m" & CStr(x) & Chr(34) & " href=" + Chr(34) + link + Chr(34) & _
                                                               " style=" + Chr(34) + "" + Chr(34) + " onmouseover=" & Chr(34) & _
                                                               "javascript:clickxx('sub" & CStr(x) & "');showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',-15,18);" & Chr(34) & _
                                                               " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(x) & "');" & Chr(34) & _
                                                               " onclick=" & Chr(34) & "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',-15,18);" & Chr(34) & " target='frm_tar'>" + submenux + "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>" & Chr(13)
                                Else
                                    rstring += "<a id=" & Chr(34) & "m" & CStr(x) & Chr(34) & " href=" + Chr(34) & "" & Chr(34) & _
                                                              " style=" + Chr(34) + "" + Chr(34) + " onmouseover=" & Chr(34) & _
                                                              "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',-15,18);" & Chr(34) & _
                                                              " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(x) & "');" & Chr(34) & _
                                                              " onclick=" & Chr(34) & "javascript:ha('sub" & CStr(x) & "');" & Chr(34) & " target='frm_tar'>" + submenux + "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>" & Chr(13)
                                End If
                            Else
                                rstring += "<a id=" & Chr(34) & "m" & CStr(x) & Chr(34) & " href=" + Chr(34) & link & Chr(34) & _
                                                           " target='frm_tar'>" & Chr(13) & submenux & _
                                                           "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>"
                            End If
                            rstring += ""
                        End If
                    Else
                        x = sqlrs.Item("menu_id")
                        If x.ToString.Length = 2 Then
                            If sqlrs.IsDBNull(4) = False Then
                                link = sqlrs.Item("menu_link")
                            Else
                                link = ""
                            End If
                            submenux = sqlrs.Item("menu_name")

                            rstring += "<span  class=" + Chr(34) + "no-border" + Chr(34) + ">" & Chr(13)
                            If isHasSubMenu(conx, x) = True Then
                                If link <> "" Then
                                    rstring += "<a id=" & Chr(34) & "m" & CStr(x) & Chr(34) & _
                                                               " href='' class='ax' style=" + Chr(34) + "" + Chr(34) + " onmouseover=" & Chr(34) & _
                                                               "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',0,18);" & Chr(34) & _
                                                               " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(x) & "');" & Chr(34) & _
                                                               ">" + submenux + "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>" & Chr(13)
                                Else
                                    rstring += "<a id=" & Chr(34) & "m" & CStr(x) & Chr(34) & _
                                                              " href=''  class='ax' style=" + Chr(34) + "" + Chr(34) + " onmouseover=" & Chr(34) & _
                                                              "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',0,18);" & Chr(34) & _
                                                              " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(x) & "');" & Chr(34) & _
                                                              ">" + submenux + "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>" & Chr(13)
                                End If
                            Else
                                rstring += "<a id=" & Chr(34) & "m" & CStr(x) & Chr(34) & _
                                                           " href='javascript: return none;' class='ax'>" & Chr(13) & submenux & _
                                                           "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>"
                            End If
                            rstring += ""
                        End If
                    End If
                End While



            End If
            sqlrs.Close()

            Return rstring
        End Function
        Public Function mainmenu34(ByVal conx As SqlConnection, ByVal url As String, ByVal auth() As String) As String
            Dim sqlrs As DataTableReader
            Dim submenux As String
            Dim rstring As String
            Dim db As New dbclass
            Dim ret() As String = {"", "", "", ""}
            submenux = "0"
            rstring = ""
            'MsgBox(url)
            sqlrs = db.dtmake("menu", "SELECT * from menu where publish='y' order by menu_order", conx)
            If sqlrs.HasRows = False Then
                sqlrs.Close()
            Else
                Dim x As String
                Dim i, j As Integer
                Dim link As String
                submenux = ""
                i = 0
                j = 0
                link = ""
                rstring += "<span style='opacity:0.1;filter:alpha(opacity=10); baground:transparent;'>&nbsp;</span>"
                While sqlrs.Read
                    If arrcomp(sqlrs.Item("Id"), auth) = True Then
                        x = sqlrs.Item("menu_id")
                        If x.ToString.Length = 2 Then
                            If sqlrs.IsDBNull(4) = False Then
                                link = sqlrs.Item("menu_link")
                            Else
                                link = ""
                            End If
                            submenux = sqlrs.Item("menu_name")
                            rstring += "<span  class=" + Chr(34) + "no-border" + Chr(34) + ">" & Chr(13)
                            If isHasSubMenu(conx, x) = True Then
                                If link <> "" Then
                                    rstring += "<span class='topmenu' id=" & Chr(34) & "m" & CStr(x) & Chr(34) & _
                                                               " style=" & Chr(34) & "cursor:pointer;" & Chr(34) & " onmouseover=" & Chr(34) & _
                                                               "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',0,20);" & Chr(34) & _
                                                               " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(x) & "');" & Chr(34) & _
                                                               " onclick=" & Chr(34) & "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',0,20);" & Chr(34) & " target='frm_tar'>" + submenux + "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>" & Chr(13)
                                Else
                                    rstring += "<span class='topmenu' id=" & Chr(34) & "m" & CStr(x) & Chr(34) & _
                                                              " style=" & Chr(34) & "cursor:pointer" & Chr(34) & " onmouseover=" & Chr(34) & _
                                                              "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',0,20);" & Chr(34) & _
                                                              " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(x) & "');" & Chr(34) & _
                                                              " onclick=" & Chr(34) & "javascript:showobjar('m" & CStr(x) & "','sub" & CStr(x) & "',0,20);" & Chr(34) & " target='frm_tar'>" + submenux + "</a></span><div style='float:left; width:0px;'>&nbsp;</div>" & Chr(13)
                                End If
                            Else
                                rstring += "<span class='topmenu' id=" & Chr(34) & "m" & CStr(x) & Chr(34) & _
                                                           " target='frm_tar'>" & submenux & _
                                                           "</a>" & Chr(13) & "</span>" & Chr(13) & "<div style='float:left; width:0px;'>&nbsp;</div>"
                            End If
                            rstring += ""
                        End If
                    End If
                End While
            End If
            sqlrs.Close()
            Return rstring
        End Function
        Private Function isHasSubMenu(ByVal conx As SqlConnection, ByVal id As Integer) As Boolean
            Dim dt As DataTableReader
            Dim db As New dbclass
            dt = db.dtmake("dtishassubmenu", "select * from menu where menu_id like '" & CStr(id) & "%'", conx)
            If dt.HasRows = True Then
                Dim n As Integer = 0
                While dt.Read
                    n = n + 1
                End While
                If n > 1 Then
                    dt.Close()
                    db = Nothing
                    Return True
                Else
                    dt.Close()
                    db = Nothing
                    Return False
                End If
            Else
                dt.Close()
                db = Nothing
                Return False

            End If
        End Function
        Public Function arrcomp(ByVal key As String, ByVal key2() As String) As Boolean
            For j As Integer = 0 To key2.Length - 1
                If key = key2(j) Then
                    Return True
                End If
            Next
            Return False
        End Function
        Public Function submenu2(ByVal conx As SqlConnection, ByVal right() As String) As String
            Dim sqlrsx As DataTableReader
            Dim mmenu() As String = {""}
            Dim m_link() As String = {""}
            Dim submenux As String
            Dim rstring As String
            Dim dbc As New dbclass
            Dim x As String
            Dim i As Integer = 0
            Dim ret() As String = {"", "", "", ""}
            submenux = "0"
            rstring = ""
            x = ""
            sqlrsx = dbc.dtmake("menuxy", "SELECT * from menu where menu_id>=10 and menu_id<99 and publish='y' order by menu_order", conx)
            If sqlrsx.HasRows = False Then
                sqlrsx.Close()
            Else
                While sqlrsx.Read

                    ReDim Preserve mmenu(i + 1)
                    ReDim Preserve m_link(i + 1)
                    mmenu(i) = sqlrsx.Item("menu_id").ToString
                    m_link(i) = sqlrsx.Item("menu_link").ToString
                    i = i + 1

                End While
            End If
            sqlrsx.Close()
            For i = 0 To mmenu.Length - 2
                sqlrsx = dbc.dtmake("menux", "SELECT * from menu where menu_id like '" & CStr(mmenu(i)) & "%' and menu_id>999 and publish='y' order by menu_order", conx)
                If sqlrsx.HasRows = True Then
                    x &= "<div class='submenu' id='sub" & (mmenu(i)) & "' style='visibility:hidden;height:25px auto; left:0px; top:0px; position:absolute;background-image:url(images/menubg.jpg);'" & _
                    " onmouseover=" & Chr(34) & "javascript:showobjar('m" & CStr(mmenu(i)) & "','sub" & CStr(mmenu(i)) & "',0,18);" & Chr(34) & _
                    " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(mmenu(i)) & "');" & Chr(34) & ">" & Chr(13)

                    While sqlrsx.Read
                        If arrcomp(sqlrsx.Item("Id"), right) = True Then

                            x &= " <a class=" + Chr(34) + "menu_a" + Chr(34) & "href='" & CStr(sqlrsx.Item("menu_link")) & _
     "?sub=" & sqlrsx.Item("menu_id") & "' style='height:auto;'onmouseover=" & Chr(34) & "javascript:this.style.background='url(images/middle_title_bar.png) repeat-x';" & Chr(34) & _
                        " onmouseout=" & Chr(34) & "this.style.background='url(images/blue_banner-760x147.jpg) #224488';" & Chr(34) & "onclick=" & Chr(34) & "javascript:document.getElementById('frm_tar').src='" & m_link(i) & "';ha('sub" & (mmenu(i)) & "'); " & Chr(34) & " target='frm_tar'>" & sqlrsx.Item("menu_name") & "</a>" & Chr(13)
                        End If
                    End While
                    x &= "</div>" & Chr(13)
                End If
                sqlrsx.Close()
            Next
            'db = Nothing
            Return x
        End Function
        Public Function submenu34(ByVal conx As SqlConnection, ByVal right() As String) As String
            Dim sqlrsx As DataTableReader
            Dim mmenu() As String = {""}
            Dim m_link() As String = {""}
            Dim submenux As String
            Dim rstring As String
            Dim dbc As New dbclass
            Dim x As String
            Dim i As Integer = 0
            Dim ret() As String = {"", "", "", ""}
            submenux = "0"
            rstring = ""
            x = ""
            sqlrsx = dbc.dtmake("menu", "SELECT * from menu where menu_id>=10 and menu_id<99 and publish='y'", conx)
            If sqlrsx.HasRows = False Then
                sqlrsx.Close()
            Else
                While sqlrsx.Read

                    ReDim Preserve mmenu(i + 1)
                    ReDim Preserve m_link(i + 1)
                    mmenu(i) = sqlrsx.Item("menu_id").ToString
                    m_link(i) = sqlrsx.Item("menu_link").ToString
                    i = i + 1

                End While
            End If
            sqlrsx.Close()
            For i = 0 To mmenu.Length - 2
                sqlrsx = dbc.dtmake("menux", "SELECT * from menu where menu_id like '" & CStr(mmenu(i)) & "%' and menu_id>999 and publish='y'", conx)
                If sqlrsx.HasRows = True Then
                    x &= "<div class='submenu' id='sub" & (mmenu(i)) & "' style='visibility:hidden; left:0px; top:0px; position:absolute;background-image:url(images/menubg.jpg);'" & _
                    " onmouseover=" & Chr(34) & "javascript:showobjar('m" & CStr(mmenu(i)) & "','sub" & CStr(mmenu(i)) & "',0,20);" & Chr(34) & _
                    " onmouseout=" & Chr(34) & "javascript:ha('sub" & CStr(mmenu(i)) & "');" & Chr(34) & ">" & Chr(13)

                    While sqlrsx.Read
                        If arrcomp(sqlrsx.Item("Id"), right) = True Then

                            x &= "<a class=" + Chr(34) + "menu_a" + Chr(34) & "href='" & CStr(sqlrsx.Item("menu_link")) & _
     "?sub=" & sqlrsx.Item("menu_id") & "' onmouseover=" & Chr(34) & "javascript:this.style.background='url(../images/middle_title_bar.png) repeat-x';" & Chr(34) & _
                        " onmouseout=" & Chr(34) & "this.style.background='url(images/blue_banner-760x147.jpg) #224488';" & Chr(34) & "onclick=" & Chr(34) & "javascript:document.getElementById('frm_tar').src='" & m_link(i) & "';ha('sub" & (mmenu(i)) & "'); " & Chr(34) & " target='frm_tar'>" & sqlrsx.Item("menu_name") & "</a>" & Chr(13)
                        End If
                    End While
                    x &= "</div>" & Chr(13)
                End If
                sqlrsx.Close()
            Next
            'db = Nothing
            Return x
        End Function
        Public Function submenu(ByVal conx As SqlConnection, ByVal subvalue As String, ByVal subno As String) As Array
            Dim sqlrsx As DataTableReader
            Dim dbc As New dbclass
            Dim submenux As String
            Dim rstring, rstring2 As String
            Dim x As String
            Dim ret() As String = {"", "", "", "", ""}
            submenux = "0"
            rstring = ""
            x = ""
            sqlrsx = dbc.dtmake("menu", "SELECT * from menu where menu_id  like '" + subvalue + "%' and publish='y'", conx)
            If sqlrsx.HasRows = False Then
                sqlrsx.Close()
            Else

                Dim link As String
                'Dim url As String
                Dim menu As String

                While sqlrsx.Read
                    menu = sqlrsx.Item("menu_name")
                    x = sqlrsx.Item("Menu_id")
                    If Len(CStr(x)) > 2 And Len(CStr(x)) < 5 Then
                        If sqlrsx.IsDBNull(4) = False Then
                            link = sqlrsx.Item("menu_link")
                        Else
                            link = ""
                        End If
                        ' MsgBox(subno)
                        If InStr(subno, x) > 0 Then
                            rstring += "<li class='no-border' ><a href='" + link + "?sub=" + x + "' style='background-color:#ff00ff;'>" + menu & "</a></li>"
                            ret(2) = menu
                            ret(3) = link + "?sub=" + x
                        Else
                            rstring += "<li class='no-border'><a href='" + link + "?sub=" + x + "'>" + menu & "</a></li>"
                        End If

                    End If
                End While
            End If
            rstring2 = rstring
            If rstring = "" Then
                rstring = "<script type='text/javascript'>document.getElementById('submenu').style.visibility='hidden';document.getElementById('submenu').style.position='relative';</script>"
            End If
            sqlrsx.Close()
            ret(0) = rstring
            ret(1) = x
            ret(4) = rstring2
            'db = Nothing
            Return ret
        End Function
        Public Function bannerget(ByVal conx As SqlConnection, ByVal menuid As String, ByVal url As String) As String

            Dim dbc As New dbclass
            'Dim id() As String
            Dim id1 As String = ""
            Dim ret As String
            ret = "header.gif"
            If menuid = "" Or String.IsNullOrEmpty(menuid) Then
                ' id = mainmenu(conx, url, "")
                'id1 = id(1)
            Else
                id1 = menuid
            End If
            Dim sql As String
            If id1 <> "" Then
                sql = "select banner from banner where menu_id=" + id1

                Dim sqlrs As DataTableReader

                sqlrs = dbc.dtmake("banner", sql, conx)
                If sqlrs.HasRows = False Then
                    sqlrs.Close()
                    Dim idx As String
                    idx = id1.Substring(0, 2)

                    sql = "select banner from banner where menu_id=" + idx
                    sqlrs = dbc.dtmake("banner", sql, conx)
                    If sqlrs.HasRows = True Then
                        sqlrs.Read()
                        ret = sqlrs.Item("banner")
                        sqlrs.Close()
                    Else
                        ret = "logo_label.gif"
                        sqlrs.Close()
                    End If

                Else
                    While sqlrs.Read
                        If sqlrs.IsDBNull(0) = False Then
                            ret = sqlrs.Item(0)
                        Else
                            ret = "logo_label.gif"
                        End If
                    End While
                    sqlrs.Close()
                End If
                ' dt = Nothing
            Else
                ret = "logo_label.gif"
            End If
            Return "banner/" & ret
        End Function
        Public Function userrights(ByVal right As String, ByVal conx As SqlConnection) As Array
            Dim r1() As String
            Dim r2() As String
            Dim rtv(1) As String
            Dim db As New dbclass
            Dim rs As DataTableReader
            rs = db.dtmake("menu", "select id,aut from menu where publish='y'", conx)
            r1 = right.Split(";")
            If rs.HasRows = True Then
                While rs.Read
                    r2 = rs.Item("aut").split(";")
                    For i As Integer = 0 To r1.Length - 1
                        For j As Integer = 0 To r2.Length - 1
                            If r1(i) = r2(j) Then
                                ' MsgBox(CStr(rtv.Length))
                                ReDim Preserve rtv(rtv.Length)
                                rtv(rtv.Length - 3) = rs.Item("Id")
                                ' MsgBox(CStr(rtv.Length - 3) & "---" & rtv(rtv.Length - 3))
                            End If
                        Next
                    Next
                End While
            End If
            Return rtv
        End Function
        Public Function cmdx(ByVal conx As SqlConnection, ByVal sqlst As String) As Object
            Dim sqlcmd As New SqlCommand
            Dim sqlrsxxx As SqlDataReader
            With sqlcmd
                .Connection = conx
                .CommandType = CommandType.Text
                .CommandText = sqlst
                sqlrsxxx = .ExecuteReader

            End With

            Return sqlrsxxx

        End Function
        Public Function cmdx2(ByVal conx As SqlConnection, ByVal sqlst As String) As Object
            Dim sqlcmd As New SqlCommand
            Dim sqlrsxxx As SqlDataReader
            Dim sqlrt As New Object

            With sqlcmd
                .Connection = conx
                .CommandType = CommandType.Text
                .CommandText = sqlst
                sqlrsxxx = .ExecuteReader
                .Dispose()
            End With

            Return sqlrsxxx

        End Function

        Public Function savecmd(ByVal conx As SqlConnection, ByVal st As String) As Integer
            Dim sqlcmd As New SqlCommand
            Dim txt As Integer

            With sqlcmd
                .Connection = conx
                .CommandType = CommandType.Text
                .CommandText = st

                Try
                    txt = .ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.ToString)
                    'Response.write(ex.ToString)
                    ' MsgBox(ex.ToString & "please this error has sent to the admin we will correct it soon")
                End Try

            End With
            sqlcmd.Dispose()
            Return txt
        End Function
        Public Function rightadv(ByVal conx As SqlConnection, ByVal pageid As String) As String
            Dim retx As String = ""
            Dim rs As DataTableReader
            Dim db As New dbclass
            rs = db.dtmake("adv", "select * from adv where publish='y'", conx)
            If rs.HasRows = True Then
                While rs.Read
                    If rs.IsDBNull(5) = False Then
                        If rs.IsDBNull(4) = False Then
                            retx = retx & "<span onclick=" & Chr(34) & "javascript:  window.open('http://" & rs.Item("link") & "');" & Chr(34) & " style='cursor:pointer'>"
                            retx = retx & "<img src=" & Chr(34) & rs.Item("file_name") & Chr(34) & " width='175px' title=" & Chr(34) & _
                            rs.Item("advname") & Chr(34) & "/>"
                            retx = retx & "</span>"

                        End If
                    End If
                End While

            End If
            rs.Close()
            Return retx
        End Function

        Public Function pathxmaker(ByVal getmenuid As String, ByVal geturl As String) As String
            Dim url() As String
            Dim fasp() As String

            url = geturl.Split("/")
            fasp = url(url.Length - 1).Split("?")

            Return fasp(0)
        End Function
    End Class
    Public Class file_list

        Inherits System.Web.UI.Page

        Dim _FileOperationException As Exception

        Public Function filesview(ByVal pathx As String, ByVal root As String, ByVal delon As Boolean, ByVal viewon As Boolean) As String
            'Dim f As Directory
            Dim up As New file_list
            Dim rtstr As String = ""

            If Directory.Exists(pathx) = True Then

                Dim ext As String = ""
                Dim fname As String = ""
                For Each k As String In Directory.GetFiles(pathx)
                    rtstr &= "<div style='display:block; float:left; width:100px;'>" & _
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
                    rtstr &= " <img src='images/gif/" & fname & ".gif' height='40px' width='40px' alt=' " & up.findfilename(k) & "' title='" & up.findfilename(k) & "' />" & _
                    " <br /><span>"
                    Dim fn As String = up.findfilename(k)
                    If fn.Length > 8 Then
                        fn = fn.Substring(0, 5) & "~." & up.file_ext(k)
                    End If
                    rtstr &= fn & "</span><br />"
                    If delon = True Then
                        rtstr &= " <span><a href=" & Chr(34) & "javascript:del('" & ff & "','1st');" & Chr(34) & _
                                  ">delete</a></span>"
                    End If

                    If viewon = True Then
                        rtstr &= "&nbsp;&nbsp;&nbsp;<span><a href=" & Chr(34) & root & up.findfilename(k) & Chr(34) & ">View..</a></span>" & _
        "</span>"
                    End If
                    rtstr &= "</div>" & _
        "<div style='width:15px; float:left;'>&nbsp;</div>"
                Next
            Else
                rtstr = "file doesnt found"
            End If
            Return rtstr
        End Function
        Public Function fcopy(ByVal sorce As String, ByVal dest As String) As Object
            Try
                File.Copy(sorce, dest)
            Catch ex As Exception
                Return ex.ToString
            End Try
            ' File.Copy(sorce, dest)
            Return Nothing
        End Function
        Public Function filesview(ByVal pathx As String, ByVal root As String) As String
            'Dim f As Directory
            Dim up As New file_list
            Dim rtstr As String = ""

            If Directory.Exists(pathx) = True Then

                Dim ext As String = ""
                Dim fname As String = ""
                For Each k As String In Directory.GetFiles(pathx)
                    rtstr &= "<div style='display:block; float:left; width:100px;'>" & _
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
                    rtstr &= " <img src='images/gif/" & fname & ".gif' height='40px' width='40px' alt=' " & up.findfilename(k) & "' title='" & up.findfilename(k) & "' />" & _
                    " <br /><span>"
                    Dim fn As String = up.findfilename(k)
                    If fn.Length > 8 Then
                        fn = fn.Substring(0, 5) & "~." & up.file_ext(k)
                    End If
                    rtstr &= fn & "</span><br />" & _
           " <span><a href=" & Chr(34) & "javascript:del('" & ff & "','1st');" & Chr(34) & _
           ">delete</a></span>&nbsp;&nbsp;&nbsp;<span><a href=" & Chr(34) & root & up.findfilename(k) & Chr(34) & ">View..</a></span>" & _
        "</span></div>" & _
        "<div style='width:15px; float:left;'>&nbsp;</div>"
                Next
            Else
                rtstr = "file doesnt found"
            End If
            Return rtstr
        End Function
        Public pathxx As String = ""
        ' pathx=server.mappathx("")
        Public Function filelist(ByVal dir As String) As String
            Dim str As String
            'Dim dr As Directory
            str = ""
            ' Dim x As String
            If Directory.Exists(dir) Then
                str = str + filelist_make(dir) + "<br />"
                For Each dra As String In Directory.GetDirectories(dir)
                    str = str + CStr(dra) + "<br />"
                    str += "=>" + filelist_make(dra) + "<br />"
                Next
            Else
                str = dir
            End If
            Return str
        End Function
        Private Function getfilename(ByVal dra As String, ByVal ffl As String) As String
            Dim str As String
            Dim sss() As String
            sss = ffl.Split("\")
            str = ""
            str = Right(ffl, CInt(ffl.Length - dra.Length))
            str = sss(UBound(sss))
            Return str
        End Function

        Private Function filelist_make(ByVal dir As String) As String
            'Dim ffl As String
            Dim x, str As String
            str = ""
            For Each ffl As String In Directory.GetFiles(CStr(dir))
                x = getfilename(dir, ffl)
                str += "=>" + CStr(ffl) + "<br />" + x + "<br />"
            Next
            Return str
        End Function
        Public Function deletefile(ByVal pathx As String) As String
            Dim str As String = ""
            Try
                If File.Exists(pathx) Then
                    File.Delete(pathx)
                    str = "File is Deleted"
                Else
                    str = "Sorry File is not found..."
                End If
            Catch ex As Exception
                str = "Sorry file is not deleted"
            End Try


            Return str
        End Function
        Public Function findpathx(ByVal pathx As String) As String
            Dim i As Integer
            Dim npathx As String
            Dim arr() As String

            arr = pathx.Split("\")

            npathx = ""
            For i = 0 To arr.Length - 2
                npathx = npathx & arr(i) & "\"
            Next
            Return npathx
        End Function
        Private Function GetFileInfoTable() As DataTable
            Dim dt As New DataTable
            With dt.Columns
                .Add(New DataColumn("Name", GetType(System.String)))
                .Add(New DataColumn("IsFolder", GetType(System.Boolean)))
                .Add(New DataColumn("FileExtension", GetType(System.String)))
                .Add(New DataColumn("Attr", GetType(System.String)))
                .Add(New DataColumn("Size", GetType(System.Int64)))
                .Add(New DataColumn("Modified", GetType(System.DateTime)))
                .Add(New DataColumn("Created", GetType(System.DateTime)))
            End With
            Return dt
        End Function
        Private Sub AddRowToFileInfoTable(ByVal fi As FileSystemInfo, ByVal dt As DataTable)
            Dim dr As DataRow = dt.NewRow
            Dim Attr As String = AttribString(fi.Attributes)
            With dr
                .Item("Name") = fi.Name
                .Item("FileExtension") = Path.GetExtension(fi.Name)
                .Item("Attr") = Attr
                If Attr.IndexOf("d") > -1 Then
                    .Item("IsFolder") = True
                    .Item("Size") = 0
                Else
                    .Item("IsFolder") = False
                    .Item("Size") = New FileInfo(fi.FullName).Length
                End If
                .Item("Modified") = fi.LastWriteTime
                .Item("Created") = fi.CreationTime
            End With
            dt.Rows.Add(dr)
        End Sub
        Private Function AttribString(ByVal a As IO.FileAttributes) As String
            Dim sb As New StringBuilder
            If (a And FileAttributes.ReadOnly) > 0 Then sb.Append("r")
            If (a And FileAttributes.Hidden) > 0 Then sb.Append("h")
            If (a And FileAttributes.System) > 0 Then sb.Append("s")
            If (a And FileAttributes.Directory) > 0 Then sb.Append("d")
            If (a And FileAttributes.Archive) > 0 Then sb.Append("a")
            If (a And FileAttributes.Compressed) > 0 Then sb.Append("c")
            Return sb.ToString
        End Function
        Public Function fupload(ByVal obj As HttpPostedFile, ByVal dest As String, ByVal size As Double, ByVal ftype() As String) As String
            Dim f As New file_list
            Dim ext, extrpt As String
            Dim ufname As String
            Dim px As Integer
            Dim flag As Boolean = False

            ext = LCase(System.IO.Path.GetExtension(obj.FileName))
            For px = 0 To ftype.Length - 1
                extrpt &= "ext:" & ext & " filetype : " & ftype(px) & "<br>"
                If ext = ftype(px) Then
                    '   MsgBox(ext)
                    flag = True
                    Exit For
                End If
            Next
            If size < obj.ContentLength Then
                ' MsgBox(obj.ContentLength & "is Greater than " & size, MsgBoxStyle.OkOnly, "File size problem")
                Return "The file Size which you try upload is not correct max size is:" & size & " and file size is: " & obj.ContentLength

            ElseIf flag = False Then
                ' MsgBox(Array.BinarySearch(ftype, ext).ToString & ext)
                Return "file type Problem! Only enter the file type which is specified" & LCase(System.IO.Path.GetExtension(obj.FileName)) & extrpt
            ElseIf Directory.Exists(dest) = False Then
                makedir(dest)
                ' MsgBox(dest)
                Try
                    ufname = f.findfilename(obj.FileName)
                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete"
            Else

                Try
                    ufname = f.findfilename(obj.FileName)
                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete"
            End If
        End Function
        Public Function fupload(ByVal obj As HttpPostedFile, ByVal dest As String, ByVal size As Double, ByVal ftype() As String, ByVal filename As String) As String
            Dim f As New file_list
            Dim ext As String
            Dim ufname As String
            Dim px As Integer
            Dim flag As Boolean = False

            ext = LCase(System.IO.Path.GetExtension(obj.FileName))
            Dim extrpt As String = ""
            For px = 0 To ftype.Length - 1
                extrpt &= "ext:" & ext & " file allow: " & ftype(px) & "<br>"
                If ext = ftype(px) Then
                    '   MsgBox(ext)
                    flag = True
                    Exit For
                End If
            Next
            If size < obj.ContentLength Then
                ' MsgBox(obj.ContentLength & "is Greater than " & size, MsgBoxStyle.OkOnly, "File size problem")
                Return "The file Size which you try upload is not correct max size is:" & size & " and file size is: " & obj.ContentLength

            ElseIf flag = False Then
                ' MsgBox(Array.BinarySearch(ftype, ext).ToString & ext)
                Return "file type Problem! Only enter the file type which is specified" & LCase(System.IO.Path.GetExtension(obj.FileName)) & extrpt
            ElseIf Directory.Exists(dest) = False Then
                makedir(dest)
                ' MsgBox(dest)
                Try
                    If filename <> "" Then
                        ufname = filename & ext
                    Else
                        ufname = f.findfilename(obj.FileName)
                    End If

                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete"
            Else

                Try
                    If filename <> "" Then
                        ufname = filename & ext
                    Else
                        ufname = f.findfilename(obj.FileName)
                    End If
                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete 2"
            End If
        End Function
        Function makedir(ByVal pa As String) As String
            Dim dinfo As DirectoryInfo
            If pa <> "" Then
                If Directory.Exists(pa) = False Then
                    dinfo = Directory.CreateDirectory(pa)
                Else
                    dinfo = Directory.GetParent(pa)

                End If
                Return dinfo.FullName
            Else
                Return ""
            End If
        End Function
        Private Sub ZipFileOrFolder(ByVal FileList As ArrayList)
            Dim ZipTargetFile As String

            If FileList.Count = 1 Then
                ZipTargetFile = Path.ChangeExtension(Convert.ToString(FileList.Item(0)), ".zip")
            Else
                ZipTargetFile = "ZipFile.zip"
            End If

            Dim zfs As FileStream
            Dim zs As ICSharpCode.SharpZipLib.Zip.ZipOutputStream
            Try
                If File.Exists(ZipTargetFile) Then
                    zfs = File.OpenWrite(ZipTargetFile)
                Else
                    zfs = File.Create(ZipTargetFile)
                End If

                zs = New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(zfs)

                'ExpandFileList(FileList)

                For Each strName As String In FileList
                    Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                    '-- the ZipEntry requires a preceding slash if the file is a folder
                    If strName.IndexOf("\") > -1 And Not strName.StartsWith("\") Then
                        ze = New ICSharpCode.SharpZipLib.Zip.ZipEntry("\" & strName)
                    Else
                        ze = New ICSharpCode.SharpZipLib.Zip.ZipEntry(strName)
                    End If

                    ze.DateTime = DateTime.Now
                    zs.PutNextEntry(ze)

                    Dim fs As FileStream
                    Try
                        fs = File.OpenRead(strName)
                        Dim buffer(2048) As Byte
                        Dim len As Integer = fs.Read(buffer, 0, buffer.Length)
                        Do While len > 0
                            zs.Write(buffer, 0, len)
                            len = fs.Read(buffer, 0, buffer.Length)
                        Loop
                    Catch ex As Exception
                        _FileOperationException = ex
                    Finally
                        If Not fs Is Nothing Then fs.Close()
                        zs.CloseEntry()
                    End Try
                Next
            Finally
                If Not zs Is Nothing Then zs.Close()
                If Not zfs Is Nothing Then zfs.Close()
            End Try
        End Sub

        Public Function getfilecontx(ByVal pathx As String) As String
            Dim str As String = ""
            If File.Exists(pathx) Then
                str = File.ReadAllText(pathx)
            Else
                str = "sorry the detail is not set."
            End If

            Return str
        End Function
        Public Function getfcontline(ByVal pathx As String) As Array
            Dim str() As String
            Dim i As Integer = 0

            If File.Exists(pathx) Then

                Dim x As System.IO.StreamReader
                x = File.OpenText(pathx)
                While x.EndOfStream = False
                    If i = 0 Then
                        ReDim str(i + 1)
                    Else
                        ReDim Preserve str(i + 1)
                    End If
                    str(i) = x.ReadLine
                    i += 1
                End While



            Else
                ReDim str(1)
                str(0) = "sorry the detail is not set."
            End If

            Return str
        End Function
        Public Function findfilename(ByVal pathx As String) As String
            Dim fname() As String
            fname = pathx.Split("\")
            Return fname(fname.Length - 1)
        End Function
        Public Function hasfile(ByVal pathx As String) As Boolean
            Dim files() As String
            Try
                files = Directory.GetFiles(pathx)
            Catch ex As Exception
                Return False
            End Try

            If files.Length > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function fileno(ByVal pathx As String)
            Dim files() As String
            Try
                files = Directory.GetFiles(pathx)
            Catch ex As Exception
                Return -1
            End Try
            If files.Length > 0 Then
                Return files.Length
            Else
                Return -1
            End If
        End Function
        Public Function getempImage(ByVal id As String, ByVal pathx As String) As String
            Dim filess As Object
            filess = Directory.GetFiles(pathx & "\idpic\")
            If (filess.Length > 0) Then
                Return "employee/" & id & "/idpic/" & findfilename(filess(0))
            Else
                Return ""
            End If
        End Function
        Public Function listfile(ByVal pathx As String) As Object
            Dim arr() As String = {""}
            If Directory.Exists(pathx) Then
                arr = Directory.GetFiles(pathx)

                Return arr
            Else
                Return arr
            End If

        End Function
        Public Function getfolder(ByVal pathx As String) As Object
            Dim arr() As String = {""}
            If Directory.Exists(pathx) Then
                arr = Directory.GetDirectories(pathx)

                Return arr
            Else
                Return arr
            End If

        End Function
        Public Function file_ext(ByVal pathx As String) As String
            Dim ext As String
            ext = LCase(System.IO.Path.GetExtension(pathx))
            Return ext
        End Function
        Public Function creatdate(ByVal filep As String) As String
            Return File.GetCreationTime(filep)
        End Function

        Function msgboxt(ByVal id As String, ByVal title As String, ByVal cont As String) As String
            Dim str As String
            str = "<div id=" & Chr(34) & CStr(id) & Chr(34) & "class='msgboxt' title='" & title & "'><div style=" & Chr(34) & "height:30px; background:url(../images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            ">" & Chr(13) & "<div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:75%; float:left; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>" & CStr(title) & "</b></div>" & Chr(13) & "<div style=" & Chr(34) & "cursor:pointer; text-align:right; height:30px; width:22%; background-image:url(../images/gif/x.gif) no-reapt; float:left;" & Chr(34) & " title='close'  onclick=" & Chr(34) & "javascript: document.getElementById('" & CStr(id) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp;close </div></div>" & Chr(13) & "<br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(cont) & "</div></div>" & _
 Chr(13) & "<script type='text/javascript'>" & _
           "//$( '#" & id & "').dialog('destroy');" & _
  "//$( '#" & id & "' ).dialog({" & _
  "resizable: true," & _
   "modal: true" & _
 "});</script>"
            Return str
        End Function
        Function dialog(ByVal id As String, ByVal title As String, ByVal cont As String)
            Dim str As String
            str = "<div id='" & id & "' class='dialogb' title='" & title & "' style='display:none;'>" & cont & "</div>"
            Return str
        End Function
        Function dbox(ByVal id As String, ByVal title As String, ByVal cont As String) As String
            Dim str As String
            str = "<div id=" & Chr(34) & CStr(id) & Chr(34) & "class='dbox' title='" & title & "'><div style=" & Chr(34) & "height:30px; background:url(../images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            ">" & Chr(13) & "<div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:550px; float:left; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>" & CStr(title) & "</b></div>" & Chr(13) & "<div style=" & Chr(34) & "cursor:pointer; text-align:left; height:30px; width:22px; background-image:url(../images/gif/x.gif); float:left;" & Chr(34) & " title='close'  onclick=" & Chr(34) & "javascript: document.getElementById('" & CStr(id) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp;close </div></div>" & Chr(13) & "<br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(cont) & "</div></div>" & _
 Chr(13) & "<script type='text/javascript'>" & _
           "//$( '#" & id & "').dialog('destroy');" & _
  "//$( '#" & id & "' ).dialog({" & _
  "resizable: true," & _
   "modal: true" & _
 "});</script>"
            Return str
        End Function
        Public Function sendmail(ByVal fromx As String, ByVal tom As String, ByVal subj As String, ByVal msg As String) As Integer
            Return 0
        End Function
        Function file_duplicate(ByVal pathx As String, ByVal filename As String)
            If File.Exists(pathx & filename) = True Then
                Return True
            Else
                Return False
            End If
        End Function

        Function findpath(ByVal p1 As String) As String
            Throw New NotImplementedException
        End Function


    End Class

    Public Class dbclass
        Public Function dbxmlview(ByVal db As String, ByVal con As SqlConnection) As String
            Dim ds As New DataSet
            Dim dt As SqlDataAdapter = New SqlDataAdapter
            dt.TableMappings.Add("Table", db)
            Dim sql As String = "select * from " & db
            dt.SelectCommand = New SqlCommand(sql, con)
            dt.Fill(ds)
            Dim xml As String = ""
            xml = ds.GetXmlSchema
            xml &= Chr(13)
            xml &= ds.GetXml
            ds.WriteXml("c:\temp\dbxml\t-" & db & ".xml")
            ds = Nothing
            dt = Nothing
            Return xml
        End Function
        Public Function dbxmlread(ByVal filename As String)
            '  Dim xmlx As New XmlDataSource
            Dim ds As New DataSet
            Dim sda As New SqlDataAdapter
            Dim xmlc As New XmlReadMode

            Dim dt As New XmlDataSource
            dt.DataFile = ("c:\temp\dbxml\t-login.xml")
            dt.DataBind()
            'dt.GetXmlDocument()


        End Function

        Public Function Backup(ByVal con As SqlConnection, ByVal pathx As String)
            ' conn = New SqlConnection(My.Settings.DataConnectionString)

            Try

                Dim cmd = New SqlCommand("BACKUP DATABASE '" & pathx & "' TO DISK = 'c:\temp\bck_" & Now.Ticks & ".bak'", con)
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Return ex.ToString
            End Try


        End Function
        Public Function float(ByVal val As Double, ByVal dgt As Integer) As String
            Dim num As Double
            Dim rnum As String = ""
            num = CDbl(val)
            rnum = Math.Round(Decimal.Add(CDbl(num), 0.00001), dgt).ToString
            Return rnum
        End Function

        Public Function getsal(ByVal emptid As Integer, ByVal conx As SqlConnection)
            Dim dt As DataTableReader
            Dim rt(2) As String
            dt = Me.dtmake("mmm", "select basic_salary,currency,date_start from emp_sal_info where emptid=" & emptid & " order by date_start desc", conx)
            If dt.HasRows Then
                While dt.Read
                    If Today.Subtract(CDate(dt.Item("date_start"))).Days >= 0 Then
                        rt(0) = dt.Item(0)
                        rt(1) = dt.Item(1)
                        Exit While
                    End If
                End While
            Else
                rt(0) = "Sorry This employee salary info is not setted!"
                rt(1) = "empsalary.aspx"
            End If
            dt.Close()
            Return rt
        End Function
        Public Function getsal(ByVal emptid As Integer, ByVal d1 As Date, ByVal conx As SqlConnection)
            Dim dt As DataTableReader
            Dim rt(2) As String
            Dim d2 As Date

            d2 = CDate(d1.Month.ToString & "/" & Date.DaysInMonth(CInt(d1.Year), CInt(d1.Month)).ToString & "/" & d1.Year.ToString)
            dt = Me.dtmake("mmm", "select basic_salary,currency,date_start from emp_sal_info where emptid=" & emptid & _
                           " and (date_start between '" & d1 & "' and '" & d2 & "' or '" & d1 & _
                           "' between date_start and isnull(date_end,'" & d2 & "')) order by id desc", conx)
            ' dt = Me.dtmake("mmm", "select basic_salary,currency,date_start from emp_sal_info where emptid=" & emptid & " order by id desc", conx)

            If dt.HasRows Then
                While dt.Read
                    'If d1.Subtract(CDate(dt.Item("date_start"))).Days >= 0 Then
                    rt(0) = dt.Item(0)
                    rt(1) = dt.Item(1)
                    ' Exit While
                    ' End If
                End While
            Else
                rt(0) = "0"
                rt(1) = "empsalary.aspx"
            End If
            dt.Close()
            Return rt
        End Function
        Public Function getsal_sp(ByVal emptid As Integer, ByVal d1 As Date, ByVal conx As SqlConnection)
            Dim dt As DataTableReader
            Dim rt(2) As String
            Dim d2 As Date

            Dim fm As New formMaker
            Dim rdate() As String
            rdate = fm.isResign(emptid, conx)
            If IsDate(rdate(1)) = True Then
                d1 = fm.getinfo2("select resign_date from emp_resign where emptid=" & emptid, conx)
                d2 = CDate(d1.Month.ToString & "/" & Date.DaysInMonth(CInt(d1.Year), CInt(d1.Month)).ToString & "/" & d1.Year.ToString)

            End If
            dt = Me.dtmake("mmm", "select basic_salary,currency,date_start,date_end from emp_sal_info where emptid=" & emptid & " and ( '" & d1 & "' between date_start  and isnull(date_end,'" & d2 & "')) order by id desc", conx)
            ' dt = Me.dtmake("mmm", "select basic_salary,currency,date_start from emp_sal_info where emptid=" & emptid & " order by id desc", conx)
            rt(0) = ""
            rt(1) = ""
            If dt.HasRows Then
                While dt.Read
                    If dt.IsDBNull(3) = True Then
                        d2 = Today.ToShortDateString
                    Else
                        d2 = dt.Item(3)
                    End If
                    If d1 >= CDate(dt.Item("date_start")) And d1 <= d2 Then
                        rt(0) = dt.Item(0)
                        rt(1) = dt.Item(1)
                        Exit While
                    End If
                End While
            Else
                rt(0) = "0"
                rt(1) = "empsalary.aspx"
            End If
            dt.Close()
            Return rt
        End Function
        Public Function getperdim(ByVal emptid As Integer, ByVal conx As SqlConnection)
            Dim dt As DataTableReader
            Dim rt As String
            dt = Me.dtmake("mmm", "select pardime from emp_pardime where emptid=" & emptid & " and to_date is null order by id desc", conx)
            If dt.HasRows Then
                dt.Read()
                rt = dt.Item(0)
            Else
                rt = "0.00"
            End If
            Return rt
        End Function
        Public Function getallow(ByVal emptid As Integer, ByVal conx As SqlConnection) As Array
            Dim dt As DataTableReader
            Dim at() As String
            Dim amt() As Double
            Dim ist() As String
            Dim i As Integer
            i = 0
            dt = Me.dtmake("mmm", "select * from emp_alloance_rec where emptid=" & emptid & " and to_date is null order by id desc,istaxable", conx)
            Dim arrr(,) As String
            Dim arr(,) As String
            If dt.HasRows Then
                ReDim at(1)
                ReDim amt(1)
                ReDim ist(1)
                While dt.Read
                    ReDim Preserve arrr(4, i + 1)

                    ReDim Preserve at(i + 1)
                    ReDim Preserve amt(i + 1)
                    ReDim Preserve ist(i + 1)
                    at(i) = dt.Item("allownace_type")
                    amt(i) = dt.Item("amount")
                    ist(i) = dt.Item("istaxable")

                    arrr(0, i) = dt.Item("allownace_type")
                    arrr(1, i) = dt.Item("amount")
                    If dt.Item("istaxable") = "y" Then
                        arrr(2, i) = "Yes"
                    Else
                        arrr(2, i) = "No"
                    End If

                    arrr(3, i) = dt.Item("from_date")
                    i = i + 1
                End While
            Else
                ReDim arrr(1, 1)
                arrr(0, 0) = "N/A"
            End If

            Return arrr
        End Function

        Public Function getemptid(ByVal emp_id As String, ByVal conx As SqlConnection)
            Dim dpname As String = ""
            Dim dt As DataTableReader
            dt = dtmake("dtmake1xxx", "select * from emprec where emp_id='" & emp_id & "' order by id desc", conx)
            If dt.HasRows Then
                dt.Read()
                dpname = dt.Item("id")
            End If
            dt.Close()
            Return dpname
        End Function
        Public Function getdepname(ByVal depid As String, ByVal conx As SqlConnection)
            Dim dpname As String = ""
            Dim dt As DataTableReader
            dt = dtmake("dtmake1", "select * from tbldepartment where dep_id='" & depid & "'", conx)
            If dt.HasRows Then
                dt.Read()
                dpname = dt.Item("dep_name")
            End If
            dt.Close()
            Return dpname
        End Function
        Public Function getprojectname(ByVal depid As String, ByVal conx As SqlConnection)
            Dim dpname As String = ""
            Dim dt As DataTableReader
            dt = dtmake("dtmake1", "select * from tblproject where project_id='" & depid & "'", conx)
            If dt.HasRows Then
                dt.Read()
                dpname = dt.Item("project_name")
            End If
            dt.Close()

            Return dpname
        End Function
        Public Function isactive(ByVal sql As String, ByVal conx As SqlConnection)
            Dim dpname As String = ""
            Dim dt As DataTableReader
            dt = dtmake("dtmake1", sql, conx)
            If dt.HasRows Then
                dt.Read()
                dpname = dt.Item("active")
            End If
            dt.Close()
            If LCase(dpname) = "y" Then
                Return True
            Else
                Return False
            End If

        End Function
        Function makest(ByVal st As String, ByVal obj As Object, ByVal conx As SqlConnection, ByVal key As String) As String
            Dim rst As String = ""
            Dim wr As String = ""
            Dim ds As New DataSet
            Dim returnval As String = ""
            Dim dt As SqlDataAdapter = New SqlDataAdapter
            dt.TableMappings.Add("Table", st)
            Dim wrstr As String = ""
            Dim ke() As String = {""}
            If key <> "" Then
                ke = key.Split(",")
                For d As Integer = 0 To ke.Length - 1
                    wr &= ke(d) & "='" & obj(ke(d)) & "'"
                    If (d <> ke.Length - 1) Then
                        wr = " and "
                    End If
                Next
                wrstr &= " where " & wr
            End If
            dt.SelectCommand = New SqlCommand("select * from " & st & wrstr, conx)
            dt.Fill(ds)
            Dim rsp As DataTableReader
            rsp = ds.CreateDataReader
            Dim i As Integer
            i = rsp.FieldCount
            rst = "insert into " & st & "("
            Dim fd As String = ""
            Dim val As String = ""
            Dim val2 As String = ""
            val = " Values("
            For i = 0 To rsp.FieldCount - 1
                If obj(rsp.GetName(i)) <> "" Then
                    fd &= rsp.GetName(i) & ","
                    If LCase(rsp.GetDataTypeName(i)) = "datetime" Then
                        val2 = val2 & "'" & obj(rsp.GetName(i)) & "',"
                        val &= "'" & obj(rsp.GetName(i)) & "',"
                    ElseIf LCase(rsp.GetDataTypeName(i)) = "string" Then
                        val2 = val2 & "'" & obj(rsp.GetName(i)) & "',"
                        val &= "'" & obj(rsp.GetName(i)).trim & "',"
                    Else
                        val2 = val2 & "'" & obj(rsp.GetName(i)) & "',"
                        val &= obj(rsp.GetName(i)) & ","
                    End If

                End If
                'MsgBox(rsp.GetName(i) & "..." & rsp.GetDataTypeName(i))
            Next
            fd = fd.Substring(0, fd.Length - 1)
            val = val.Substring(0, val.Length - 1)
            val2 = val2.Substring(0, val2.Length - 1)
            fd &= ")"
            val &= ")"
            rst &= fd & val

            If key <> "" Then
                If rsp.HasRows = True Then
                    While rsp.Read
                        If ke.Length = 1 Then
                            If obj(ke(0)) = rsp.Item(ke(0)) Then
                                returnval = " <span style='font-size:14pt; color:red;'>Due to data duplication</span>"
                            End If
                        Else
                            For c As Integer = 0 To ke.Length - 1
                                If obj(ke(c)) = rsp.Item(ke(c)) Then
                                    returnval = (" <span style='font-size:14pt; color:red;'>Due to data duplication</span>")
                                End If
                            Next

                        End If
                    End While
                    ' MsgBox(returnval)
                Else
                    ' MsgBox(rst)
                    returnval = rst
                End If

            Else
                returnval = rst
            End If
            rsp.Close()
            ds.Dispose()
            dt.Dispose()
            rsp = Nothing
            Return returnval
        End Function
        Public Function makeupdate(ByVal st As String, ByVal obj As Object, ByVal conp As SqlConnection, ByVal where As String) As Array
            Dim rst() As String = {"", ""}
            ' Dim dp() As String
            Dim ds As DataSet = New DataSet
            Dim dt As SqlDataAdapter = New SqlDataAdapter()

            dt.TableMappings.Add("Table", st)

            dt.SelectCommand = New SqlCommand("select * from " & st, conp)
            dt.Fill(ds)
            Dim rsp As DataTableReader
            rsp = ds.CreateDataReader
            Dim i As Integer
            rst(0) = "update " & st & " set "
            Dim fd As String = ""
            Dim val As String = ""
            Dim val2 As String = ""
            val = ""
            Dim kk As Integer = 0
            For i = 1 To rsp.FieldCount - 1

                val &= rsp.GetName(i) & "=@" & rsp.GetName(i) & ","
            Next
            val = val.Substring(0, val.Length - 1)
            val &= " where " & rsp.GetName(0) & "= @" & rsp.GetName(0)
            rst(0) &= val
            rst(1) = rsp.GetName(0)

            rsp.Close()
            ds.Dispose()
            dt.Dispose()
            rsp = Nothing
            Return rst
        End Function
        Public Function makeupdatea(ByVal st As String, ByVal sql As String, ByVal conp As SqlConnection, ByVal where As String) As Array
            Dim rst() As String = {"", ""}
            Dim arrf() As String = {""}
            Dim fm As New formMaker
            If where <> "" Then
                arrf = where.Split(",")


                Try



                    ' Dim dp() As String
                    Dim ds As DataSet = New DataSet
                    Dim dt As SqlDataAdapter = New SqlDataAdapter()

                    dt.TableMappings.Add("Table", st)

                    dt.SelectCommand = New SqlCommand(sql, conp)

                    dt.Fill(ds)
                    Dim rsp As DataTableReader
                    rsp = ds.CreateDataReader
                    Dim i As Integer
                    rst(0) = "update " & st & " set "
                    Dim fd As String = ""
                    Dim val As String = ""
                    Dim val2 As String = ""
                    val = ""
                    Dim kk As Integer = 0
                    For i = 1 To rsp.FieldCount - 1
                        If arrf.Length > 1 Then
                            If fm.searcharray(arrf, rsp.GetName(i)) = True Then
                                val &= rsp.GetName(i) & "=@" & rsp.GetName(i) & ","
                            End If
                        End If

                    Next
                    val = val.Substring(0, val.Length - 1)
                    val &= " where " & rsp.GetName(0) & "=@" & rsp.GetName(0)
                    rst(0) &= val
                    rst(1) = rsp.GetName(0)

                    rsp.Close()
                    ds.Dispose()
                    dt.Dispose()
                    rsp = Nothing
                    Return rst
                Catch ex As Exception
                    rst(0) = ex.ToString & "=>" & sql
                    Return rst
                End Try
            Else
                Try



                    ' Dim dp() As String
                    Dim ds As DataSet = New DataSet
                    Dim dt As SqlDataAdapter = New SqlDataAdapter()

                    dt.TableMappings.Add("Table", st)

                    dt.SelectCommand = New SqlCommand(sql, conp)

                    dt.Fill(ds)
                    Dim rsp As DataTableReader
                    rsp = ds.CreateDataReader
                    Dim i As Integer
                    rst(0) = "update " & st & " set "
                    Dim fd As String = ""
                    Dim val As String = ""
                    Dim val2 As String = ""
                    val = ""
                    Dim kk As Integer = 0
                    For i = 1 To rsp.FieldCount - 1
                        val &= rsp.GetName(i) & "=@" & rsp.GetName(i) & ","


                    Next
                    val = val.Substring(0, val.Length - 1)
                    val &= " where " & rsp.GetName(0) & "=@" & rsp.GetName(0)
                    rst(0) &= val
                    rst(1) = rsp.GetName(0)

                    rsp.Close()
                    ds.Dispose()
                    dt.Dispose()
                    rsp = Nothing
                    Return rst
                Catch ex As Exception
                    rst(0) = ex.ToString & "=>" & sql
                    Return rst
                End Try
            End If

        End Function
        Public Function idlist_jqry(ByVal tbl As String, ByVal fld As String, ByVal con As SqlConnection) As String
            Dim sql As String = "select " & fld & " from " & tbl
            Dim dt As DataTableReader
            Dim retstr As String = ""
            dt = dtmake(tbl & Today.ToLongTimeString, sql, con)

            If dt.HasRows = True Then
                While dt.Read
                    If dt.IsDBNull(0) = False Then
                        If LCase(dt.GetDataTypeName(0)) = "string" Then
                            retstr &= Chr(34) & dt.Item(0).trim & Chr(34) & ","
                        Else
                            retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
                        End If
                    End If
                    'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
                End While
                retstr &= Chr(34) & "xx" & Chr(34)
            End If
            Return retstr
        End Function

        Public Function edited_field(ByVal tbl As String, ByVal con As SqlConnection, ByVal key As String, ByVal key_val As String) As Array
            Dim strfil As String = ""
            Dim enable As String = ""
            Dim arr() As String = {"", ""}
            If key_val <> "" Then

                Dim rs As DataTableReader
                Dim ds As New dbclass
                rs = ds.dtmake("newtbl" & Today.ToLongTimeString, "select * from " & tbl & " where " & key & "='" & key_val & "'", con)
                If rs.HasRows = True Then
                    Dim fc As Integer = rs.FieldCount - 3
                    rs.Read()
                    For i As Integer = 1 To fc

                        If LCase(rs.GetDataTypeName(i)) = "datetime" And rs.IsDBNull(i) = False Then
                            ' MsgBox(LCase(rs.GetDataTypeName(i)))

                            Dim sdate As Date = rs.Item(i)
                            Dim d As String = sdate.ToShortDateString
                            Dim da As String = sdate.Day
                            Dim mm As String = sdate.Month
                            Dim yy As String = sdate.Year
                            d = mm & "/" & da & "/" & yy
                            strfil &= "$('#" & rs.GetName(i) & "').val('" & d & "');" & _
                            "$('#" & rs.GetName(i) & "').attr('disabled','disabled');" & _
                            Chr(13) & "$(function() {" & _
                           Chr(13) & "$('#" & rs.GetName(i) & "').datepicker( 'option','dateFormat','mm/dd/yy');" & _
                           Chr(13) & "$('#" & rs.GetName(i) & "').datepicker({changeMonth: true,changeYear: true,minDate: '-70Y', maxDate: '-18Y',defaultDate:'" & d & "'}); " & _
                           Chr(13) & "});"
                            enable &= "disable_obj('" & rs.GetName(i) & "','none');"
                        Else
                            strfil &= "$('#" & rs.GetName(i) & "').val('" & rs.Item(i) & "');$('#" & rs.GetName(i) & "').attr('disabled','disabled');"
                            enable &= "disable_obj('" & rs.GetName(i) & "','none');"
                        End If
                    Next
                End If
                rs.Close()
                rs = Nothing
                ds = Nothing
            End If
            arr(0) = strfil
            arr(1) = enable
            Return arr
        End Function
        Public Function makeupdate_statement_obj(ByVal st As String, ByVal obj As Object, ByVal conp As SqlConnection, ByVal where_field As String, ByVal where_val As String) As String
            Dim rt As DataTableReader
            Dim rtval As String = ""
            Dim sql As String = "select * from " & st & " where " & where_field & "='" & where_val & "'"
            Dim val As String = ""
            val = "update " & st & " set "
            rt = Me.dtmake("mk" & Today.ToString, sql, conp)
            If rt.HasRows Then
                rt.Read()
                For jk As Integer = 1 To rt.FieldCount - 1

                    If obj(rt.GetName(jk)) <> "" Then
                        If val.EndsWith("set ") = False Then
                            val = val & ", "
                        End If
                        If rt.GetDataTypeName(jk).Contains("float,int,double,int32,int16,int64,long") = True Then
                            val = val & rt.GetName(jk) & "=" & obj(rt.GetName(jk))
                        Else
                            val = val & rt.GetName(jk) & "='" & obj(rt.GetName(jk)) & "'"
                        End If
                    Else

                        If rt.GetDataTypeName(jk).Contains("float,int,double,int32,int16,int64,long") = True Then
                            If rt.IsDBNull(jk) = False Then
                                If rt.Item(jk) > 0 Then
                                    If val.EndsWith("set ") = False Then
                                        val = val & ", "
                                    End If
                                    val = val & rt.GetName(jk) & "=0"
                                End If
                            End If
                        ElseIf rt.GetDataTypeName(jk).ToString = "DateTime" Then
                            If rt.IsDBNull(jk) = False Then
                                If rt.Item(jk).ToString <> "" Then
                                    If val.EndsWith("set ") = False Then
                                        val = val & ", "
                                    End If
                                    val = val & rt.GetName(jk) & "=Null"
                                End If
                            End If
                        Else
                            'MsgBox(rt.GetDataTypeName(jk))
                            If rt.IsDBNull(jk) = False Then
                                If rt.Item(jk).ToString <> "" Then
                                    If val.EndsWith("set ") = False Then
                                        val = val & ", "
                                    End If
                                    val = val & rt.GetName(jk) & "=''"
                                End If
                            End If

                        End If
                    End If
                Next
            End If
            If val.EndsWith("set ") = False Then
                val = val & " where " & where_field & "='" & where_val & "'"
            End If
            Return val
        End Function
        Public Function makeupdate_statement(ByVal st As String, ByVal obj As Object, ByVal conp As SqlConnection, ByVal where_field As String, ByVal where_val As String) As String
            Dim rt As DataTableReader
            Dim rtval As String = ""
            Dim sql As String = "select * from " & st & " where " & where_field & "='" & where_val & "'"
            Dim val As String = ""
            val = "update " & st & " set "
            rt = Me.dtmake("mk" & Today.ToString, sql, conp)
            If rt.HasRows Then
                rt.Read()
                For jk As Integer = 1 To rt.FieldCount - 1
                    For Each k As String In obj
                        If rt.GetName(jk) = k Then
                            If obj(rt.GetName(jk)) <> "" Then
                                If val.EndsWith("set ") = False Then
                                    val = val & ", "
                                End If
                                If rt.GetDataTypeName(jk).Contains("float,int,double,int32,int16,int64,long") = True Then
                                    val = val & rt.GetName(jk) & "=" & obj(rt.GetName(jk))
                                Else
                                    val = val & rt.GetName(jk) & "='" & obj(rt.GetName(jk)) & "'"
                                End If
                            Else

                                If rt.GetDataTypeName(jk).Contains("float,int,double,int32,int16,int64,long") = True Then
                                    If rt.IsDBNull(jk) = False Then
                                        If rt.Item(jk) > 0 Then
                                            If val.EndsWith("set ") = False Then
                                                val = val & ", "
                                            End If
                                            val = val & rt.GetName(jk) & "=0"
                                        End If
                                    End If
                                ElseIf rt.GetDataTypeName(jk).ToString = "DateTime" Then
                                    If rt.IsDBNull(jk) = False Then
                                        If rt.Item(jk).ToString <> "" Then
                                            If val.EndsWith("set ") = False Then
                                                val = val & ", "
                                            End If
                                            val = val & rt.GetName(jk) & "=Null"
                                        End If
                                    End If
                                Else
                                    'MsgBox(rt.GetDataTypeName(jk))
                                    If rt.IsDBNull(jk) = False Then
                                        If rt.Item(jk).ToString <> "" Then
                                            If val.EndsWith("set ") = False Then
                                                val = val & ", "
                                            End If
                                            val = val & rt.GetName(jk) & "=''"
                                        End If
                                    End If

                                End If
                            End If
                        End If
                    Next


                Next
            End If
            If val.EndsWith("set ") = False Then
                val = val & " where " & where_field & "='" & where_val & "'"
            End If

            Return val
        End Function
        Public Function listtable(ByVal conx As SqlConnection) As Array
            Dim ds As New DataSet
            Dim st() As String = {""}
            Dim dt As SqlDataAdapter = New SqlDataAdapter()
            dt.TableMappings.Add("Table", "sys.objects")

            dt.SelectCommand = New SqlCommand("SELECT * FROM sys.objects WHERE type in (N'U') order by name", conx)
            dt.Fill(ds)
            Dim rsp As DataTableReader
            rsp = ds.CreateDataReader
            Dim i As Integer = 0
            ' MsgBox(rsp.FieldCount.ToString)
            If rsp.HasRows = True Then
                While rsp.Read

                    ReDim Preserve st(i + 1)
                    st(i) = rsp.Item(0)
                    i += 1
                End While
                'MsgBox(st)
            End If
            ds = Nothing
            dt = Nothing
            Return st
        End Function
        Function save(ByVal sql As String, ByVal conx As SqlConnection, ByVal pathx As String) As Integer
            Dim rowaff As Integer = -1
            Dim cmd As New SqlCommand
            Dim flinf As FileInfo
            ' MsgBox(sql)
            Dim p2 As String
            With cmd
                .Connection = conx
                .CommandType = CommandType.Text
                .CommandText = sql
                Try
                    p2 = pathx & "\log\"
                    pathx = pathx & "\log\logkir.txt"
                    rowaff = .ExecuteNonQuery()
                    flinf = New FileInfo(pathx)
                    If CInt(flinf.Length) / 1024000 > 2 Then
                        File.AppendAllText(p2 & "backup" & Now.Ticks.ToString & ".log", File.ReadAllText(pathx))
                        File.WriteAllText(pathx, "")

                    End If

                    ' rowaff = .ExecuteNonQuery()
                    File.AppendAllText(pathx.ToString, Chr(13) & Now & Chr(13) & sql.ToString & "=>" & HttpContext.Current.Session("emp_iid").ToString)
                Catch ex As Exception
                    rowaff = -2
                    ' writeerro(ex.ToString)
                End Try
            End With
            Return rowaff
        End Function
        Function excutes(ByVal sql As Object, ByVal conx As SqlConnection, ByVal pathx As String) As Object
            Dim rowaff As Object = "non"
            Dim lm As New leavemgt
            Dim flinf As FileInfo
            Dim cmd As New SqlCommand
            Dim fm As New formMaker
            Dim p2 As String = ""
            Dim pathpass As String = pathx
            ' cmdb.Connection
            ' MsgBox(sql)
            If sql.ToString.Contains("delete") = True Or LCase(sql.ToString).Contains("delete") = True Then
                fm.exception_hand("Deleted by " & HttpContext.Current.Session("username") & "<br>" & sql, HttpContext.Current.Session("company_name") & ": deletion")

            End If
            p2 = pathx & "\log\"
            pathx = pathx & "\log\logkir.txt"

            With cmd
                .Connection = conx
                .CommandType = CommandType.Text
                .CommandText = sql.ToString

                Try

                    flinf = New FileInfo(pathx)
                    If CInt(flinf.Length) / 1024000 > 2 Then
                        File.AppendAllText(p2 & "backup" & Now.Ticks.ToString & ".log", File.ReadAllText(pathx))
                        File.WriteAllText(pathx, "")
                    End If

                    rowaff = .ExecuteNonQuery()
                    File.AppendAllText(pathx.ToString, Chr(13) & Now & Chr(13) & " " & sql.ToString & "=>" & HttpContext.Current.Session("emp_iid").ToString & "<br>Row affect-> " & rowaff & "<br>")
                Catch ex As Exception
                    rowaff = ex.ToString
                    fm.exception_hand(ex.ToString & "<br>" & sql, HttpContext.Current.Session("company_name") & ":excutes function")
                End Try
            End With
            cmd = Nothing
            Return rowaff
        End Function

        Function writeerro(ByVal err As String)
            Dim emailx As New mail_system

            If File.Exists(HttpContext.Current.Session("path") & "log\werror-" & Format(Today, "ddMMyyyy") & ".log") = False Then
                File.WriteAllText(HttpContext.Current.Session("path") & "log\werror-" & Format(Today, "ddMMyyyy") & ".log", err)
                emailx.sendemail(err.ToString, HttpContext.Current.Session("epwd"), HttpContext.Current.Session("efrom"), HttpContext.Current.Session("eto"), "Error: ON:" & HttpContext.Current.Session("company_name"), HttpContext.Current.Session("smtp"), HttpContext.Current.Session("eport"))

            Else
                err = File.ReadAllText(HttpContext.Current.Session("path") & "log\werror-" & Format(Today, "ddMMyyyy") & ".log") & err
                File.WriteAllText(HttpContext.Current.Session("path") & "log\werror-" & Format(Today, "ddMMyyyy") & ".log", err)
                emailx.sendemail(err.ToString, HttpContext.Current.Session("epwd"), HttpContext.Current.Session("efrom"), HttpContext.Current.Session("eto"), "Error: ON:" & HttpContext.Current.Session("company_name"), HttpContext.Current.Session("smtp"), HttpContext.Current.Session("eport"))

            End If
        End Function
        Public Function dtmake(ByVal st As String, ByVal sql As String, ByVal conx As SqlConnection) As Object
            Dim rst As String = ""
            Dim ds As New DataSet
            Dim dt As SqlDataAdapter = New SqlDataAdapter
            dt.TableMappings.Add("Table", st)
            If sql <> "" Then
                dt.SelectCommand = New SqlCommand(sql, conx)
            Else
                dt.SelectCommand = New SqlCommand("select * from " & st, conx)
            End If
            Try
                dt.Fill(ds)
            Catch ex As Exception
                Return "select * from " & st & "<br>" & ex.ToString

            End Try

            Dim rsp As DataTableReader
            rsp = ds.CreateDataReader
            ds = Nothing
            Return rsp
        End Function
        Public Function getdatefields(ByVal tbl As String, ByVal conx As SqlConnection) As Array
            Dim rsp As DataTableReader
            Dim i As Integer
            Dim dp() As String = {""}
            Dim kk As Integer = 0
            rsp = dtmake(tbl, "", conx)
            For i = 1 To rsp.FieldCount - 1
                If LCase(rsp.GetDataTypeName(i)) = "datetime" Then
                    ReDim Preserve dp(kk + 1)
                    dp(kk) = rsp.GetName(i)
                    kk += 1
                End If
            Next
            Return dp
        End Function
        Public Function getdatafields(ByVal tbl As String, ByVal conx As SqlConnection) As Array
            Dim rsp As DataTableReader
            Dim i As Integer
            Dim dp() As String = {""}
            Dim kk As Integer = 0
            rsp = dtmake(tbl, "", conx)
            For i = 0 To rsp.FieldCount - 1

                ReDim Preserve dp(kk + 1)
                dp(kk) = rsp.GetName(i)
                kk += 1

            Next
            Return dp
        End Function
        Public Function listnames(ByVal con As SqlConnection, ByVal fieldbind As String, ByVal table As String) As String
            Dim dt As DataTableReader
            Dim sql As String
            sql = "select " & fieldbind & " from " & table
            dt = dtmake(table, sql, con)
            Dim arr As String = "["
            If dt.HasRows Then
                Dim itmc As Integer = dt.FieldCount
                While dt.Read
                    arr &= Chr(34)
                    For i As Integer = 0 To itmc - 1
                        arr &= dt.Item(i) & " "
                    Next
                    arr &= Chr(34) & ","
                End While

            End If
            arr &= Chr(34) & "&" & Chr(34) & "];"
            Return arr
        End Function

        Public Function makerpt(ByVal p1 As String, ByVal p2 As String, ByVal rto As String, ByVal con As SqlConnection, ByVal pathx As String) As Object
            Dim sql As String = "insert into rptdataupdate(reporttype,Report,date,seen,reportto) values('" & _
                p1 & "','" & p2 & "','" & Now & "','n','" & rto & "')"
            sql = "Begin Transaction" & Chr(13) & sql & Chr(13)
            Dim flg As Object = Me.excutes(sql, con, pathx)
            If IsNumeric(flg) Then
                If CInt(flg) > 0 Then
                    Me.excutes("COMMIT", con, pathx)

                End If
            End If
            Return Nothing
        End Function

    End Class
    Public Class k_security
        Public Function StrToHex3(ByVal Data As String) As String
            Dim mail As New mail_system
            Dim sHex As String = ""
            Try
                Data = Kir_StrToHex(Data)
                sHex = Conversion.Hex(Data)
            Catch ex As Exception
                mail.sendemail(ex.ToString, HttpContext.Current.Session("epwd"), HttpContext.Current.Session("efrom"), HttpContext.Current.Session("eto"), "Error: ON:" & HttpContext.Current.Session("company_name"), HttpContext.Current.Session("smtp"), HttpContext.Current.Session("eport"))
            End Try



            Return sHex
        End Function
        Public Function HexToStr3(ByVal Data As String) As String

            Dim str As String = ""
            Dim fm As New formMaker
            Try
                str = Conversion.Str(Data)
                str = Kir_HexToStr(str)
            Catch ex As Exception
                fm.exception_hand(ex, "master page Erro")
            End Try


            Return str
        End Function
        Public Function StrToHex(ByVal Data As String) As String
            Dim sVal As String
            Dim sHex As String = ""
            If String.IsNullOrEmpty(Data) = False Then
                While Data.Length > 0
                    sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
                    Data = Data.Substring(1, Data.Length - 1)
                    sHex = sHex & sVal & "⌡"
                End While
            End If

            Return sHex
        End Function
        Public Function Kir_StrToHex(ByVal Data As String) As String
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
        Public Function Kir_HexToStr(ByVal data As String) As String
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
        Public Function dbStrToHex(ByVal Data As String) As String
            Dim sVal As String
            Dim sHex As String = ""
            If String.IsNullOrEmpty(Data) = False Then
                While Data.Length > 0
                    sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
                    Data = Data.Substring(1, Data.Length - 1)
                    sHex = sHex & sVal & ")"
                End While


            End If
            Return sHex
        End Function
        Public Function HexToStr(ByVal data As String) As String
            Dim sval As String
            Dim sstr As String = ""
            Dim dd() As String
            If String.IsNullOrEmpty(data) = False Then
                dd = data.Split("⌡")

                For i As Integer = 0 To UBound(dd) - 1
                    sval = Chr(Convert.ToInt32((dd(i)), 16))

                    sstr = sstr & sval
                Next
            End If
            Return sstr
        End Function
        Public Function dbHexToStr(ByVal data As String) As String
            Dim sval As String
            Dim sstr As String = ""
            Dim dd() As String
            If String.IsNullOrEmpty(data) = False Then
                dd = data.Split(")")

                For i As Integer = 0 To UBound(dd) - 1
                    sval = Chr(Convert.ToInt32((dd(i)), 16))

                    sstr = sstr & sval
                Next
            End If
            Return sstr
        End Function
        Public Function d_encryption(ByVal data As String) As String
            Dim uni As New UnicodeEncoding()
            '  Dim hxa As New hexadec
            ' Create a string that contains Unicode characters.
            Dim bstr As String = ""
            Dim unicodeString As String = data
            ' Encode the string.
            Dim encodedBytes As Byte() = uni.GetBytes(unicodeString)

            For Each b As Byte In encodedBytes
                bstr &= b & ";"
            Next

            ' Decode bytes back to string.
            ' Notice Pi and Sigma characters are still present.
            Dim decodedString As String = uni.GetString(encodedBytes)

            Return bstr
        End Function
        Public Function bcodex(ByVal sf As String) As String
            Dim uni As New UnicodeEncoding

            Dim r As String
            r = uni.GetBytes(sf).ToString

            Return r
        End Function
        Public Function Str2ToHex(ByVal Data As String) As String
            Dim sVal As String
            Dim sHex As String = ""
            If String.IsNullOrEmpty(Data) = False Then
                While Data.Length > 0
                    sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
                    Data = Data.Substring(1, Data.Length - 1)
                    sHex = sHex & sVal
                End While
            End If
            Return sHex
        End Function
    End Class
    Public Class kirsoftsystem
        Public Function StringToDate(ByVal strDate As String, ByVal pattern As String)
            Dim myDTFI As New System.Globalization.DateTimeFormatInfo()
            myDTFI.ShortDatePattern = pattern
            Dim dtDate As DateTime
            Try
                dtDate = DateTime.Parse(strDate, myDTFI)
            Catch ex As Exception
                Return False
            End Try
            Return dtDate
        End Function
        Public Function dateconv(ByVal strdate As Date, ByVal pattern As String)
            Dim rdate As String = ""
            Dim dd, mm, yyyy As String
            dd = strdate.Day.ToString
            mm = strdate.Month.ToString
            yyyy = strdate.Year.ToString
            If LCase(pattern) = "mm/dd/yyyy" Then
                rdate = mm & "/" & dd & "/" & yyyy
            ElseIf LCase(pattern) = "dd/mm/yyyy" Then
                rdate = dd & "/" & mm & "/" & yyyy
            End If
            Return rdate
        End Function
    End Class
    Public Class datetimecal
        Public Function isPublic(ByVal d As Date, ByVal conx As SqlConnection) As Boolean
            Dim sql As String = "select * from holidays where date_lie='" & d.Year.ToString & "/" & d.Month.ToString & "/" & d.Day.ToString & " 00:00:00'"
            Dim dt As DataTableReader
            Dim db As New dbclass
            dt = db.dtmake("chkdate", sql, conx)
            If dt.HasRows = True Then
                Return True
            Else
                Return False
            End If

        End Function
        Public Function isWeekEnd(ByVal d As Date, ByVal empid As String, ByVal conx As SqlConnection) As Object
            Dim sql As String = "select * from emprec where emp_id='" & empid & "' and (end_date is null or end_date='' or end_date='1/1/1900')"
            Dim dt As DataTableReader
            Dim db As New dbclass
            Dim hdd() As String
            Dim hd As String

            dt = db.dtmake("chkdate", sql, conx)
            If dt.HasRows = True Then
                dt.Read()
                If dt.IsDBNull(5) = False Then
                    hd = dt.Item("holiday")
                    hdd = hd.Split(",")
                    For i As Integer = 0 To hdd.Length - 1
                        '"G", "g", "X", "x", "F", "f", "D" or "d". 
                        If LCase(hdd(i)).Trim = LCase((d.DayOfWeek).ToString) Then
                            Return True
                        End If
                    Next
                Else
                    Return "There is no speciefic Date"
                End If
            Else
                Return "This person not existed or resigned"
            End If
            Return False
        End Function
        Function dayname(ByVal n As Integer)
            Dim dayn() As String = {"", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
            Return dayn(n)
        End Function

    End Class
    Public Class leavemgt
        Public Function getleaveinfo(ByVal emp_id As String, ByVal con As SqlConnection)
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
            Dim rtn(6) As String
            dt = dbs.dtmake("md", "select * from emprec where emp_id='" & emp_id & "'", con)
            If dt.HasRows Then
                out = ""

                While dt.Read
                    rtn(0) = ""
                    rtn(1) = ""
                    rtn(2) = ""
                    rtn(3) = ""
                    rtn(4) = ""
                    rtn(5) = ""
                    rowspan = 0
                    'Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)
                    retu = fm.searcharray(empx, sec.Str2ToHex(dt.Item("emp_id"))).ToString
                    ' Response.Write(retu.ToString & "<br>")
                    If retu = False Then
                        ' Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)

                        ReDim Preserve empx(i + 1)
                        empx(i) = sec.Str2ToHex(dt.Item("emp_id"))


                        'get leave info here

                        Dim oldcolor As String = ""
                        Dim rs As DataTableReader
                        rs = dbs.dtmake("leave", "select * from show_leave_bal where emptid=" & dt.Item("id") & " order by 'year end'", con)
                        Dim tbgt, usedx, avail, bal, expbal As Double
                        tbgt = 0
                        Dim paidbymony As Double = 0
                        If rs.HasRows = True Then
                            Dim no_row As String = fm.getinfo2("select count(id) from show_leave_bal where emptid=" & dt.Item("id"), con)
                            ' Response.Write(no_row)
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
                                pdst = fm.getinfo2("select paidamt from leav_settled where bgtid=" & rs.Item("id"), con)
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

                                    flg = 1
                                Else



                                End If

                            End While


                        End If

                        If tbgt > 0 Then
                            Dim unpaid As Double
                            unpaid = expbal - paidbymony
                            ' out &= "<tr><td colspan='8'><b>Total Bugeted:" &  & " | Available:" &  & " | Used:" & usedx.ToString & " | Balance:" & bal.ToString & " | Exp. Bal: " & expbal.ToString & " | Paid Bal:" & paidbymony.ToString
                            rtn(0) = tbgt.ToString '""
                            rtn(1) = avail.ToString '""
                            rtn(2) = usedx.ToString ' ""
                            rtn(3) = bal.ToString '""
                            rtn(4) = expbal.ToString '""
                            rtn(5) = paidbymony.ToString

                        End If
                        rs.Close()


                        '  i = i + 1
                    End If



                End While
                dt.Close()
                'Response.Write("<table cellpading='2' cellspacing='2' bordercolor='blue'>" & out & "</table>")
            End If

            dbs = Nothing
            fm = Nothing
            Return rtn
        End Function
        Function writeerro(ByVal err As String, ByVal pathx As String)
            Dim flinf As FileInfo
            Dim email As New mail_system
            Dim p2 As String = ""
            p2 = pathx & "\log\error.log"
            flinf = New FileInfo(p2)
            If File.Exists(p2) = True Then
                If CInt(flinf.Length) / 1024000 > 2 Then
                    File.AppendAllText(pathx & "\log\" & Now.Ticks & "backuperror.log", File.ReadAllText(p2))
                    File.WriteAllText(p2, "")

                End If

            End If

            If File.Exists(p2) = False Then
                File.WriteAllText(p2, err)
            Else
                err &= err & File.ReadAllText(p2)
                File.AppendAllText(p2, err & "<=======" & Now & vbNewLine)
                email.sendemail(err, HttpContext.Current.Session("epwd"), HttpContext.Current.Session("efrom"), HttpContext.Current.Session("eto"), HttpContext.Current.Session("company_name") & " error", HttpContext.Current.Session("smtp"), HttpContext.Current.Session("eport"))

            End If
        End Function
    End Class

    Public Class mail_system
        Dim fm As New formMaker

        Public Function mailprep(ByVal txtTo As String, ByVal txtFrom As String, ByVal txtbody As String, ByVal txtsubject As String) As Object
            Try

                Dim e_mail As New MailMessage()


                e_mail = New MailMessage()
                e_mail.From = New MailAddress(txtFrom)
                e_mail.To.Add(txtTo)
                e_mail.Subject = txtsubject
                e_mail.IsBodyHtml = False
                e_mail.Body = txtbody
                ' Smtp_Server.Send(e_mail)
                ' MsgBox("Mail Sent")
                Return e_mail
            Catch error_t As Exception
                MsgBox(error_t.ToString)
            End Try

        End Function
        Public Function mailsend(ByVal e_mail_obj As Object)

            Try
                Dim Smtp_Server As New SmtpClient
                Dim e_mail As New MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential("kirsoftet@gmail.com", "493 15kir")
                Smtp_Server.Port = 587
                Smtp_Server.EnableSsl = True
                Smtp_Server.Host = "smtp.gmail.com"
                e_mail = e_mail_obj
                Smtp_Server.Send(e_mail)
                Return "Message sent"
            Catch ex As Exception

                fm.exception_hand(ex, "master page Erro")
                Return ex.ToString

            End Try

        End Function


        Function sendemail(ByVal msg As String, ByVal passwordx As String, ByVal froms As String, ByVal tos As String, ByVal subj As String)
            Try
                Dim Smtp_Server As New SmtpClient("smtp.gmail.com", "587")

                Dim e_mail As New MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential(froms, passwordx)
                ' Smtp_Server.Port = 587

                Smtp_Server.EnableSsl = True
                e_mail = New MailMessage()
                e_mail.From = New MailAddress(froms)
                e_mail.To.Add(tos)
                e_mail.Subject = subj
                e_mail.IsBodyHtml = True
                e_mail.Body = msg
                Smtp_Server.Send(e_mail)
                Return "msg sent"

            Catch error_t As Exception
                Dim pathx As String = "C:\temp\email\email" & Now.Day & Now.Month & Now.Year & ".txt"
                If File.Exists(pathx) Then
                    Try
                        msg = Now.ToString & error_t.ToString & ">>>" & msg & ">>>to:" & tos & ">>>>>>>>>>>>>>>\n"
                        File.AppendAllText(pathx, msg)
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                Else
                    msg = Now.ToString & error_t.ToString & ">>>" & msg & ">>>to:" & tos & ">>>>>>>>>>>>>>>\n"
                    File.WriteAllText(pathx, msg)
                End If
                Return "not sent because " & error_t.ToString
            End Try
        End Function
        Function sendemail(ByVal msg As String, ByVal passwordx As String, ByVal froms As String, ByVal tos As String, ByVal subj As String, ByVal smtp As String, ByVal port As Integer)
            Dim rtn As String = ""
            Try
                Dim Smtp_Server As New SmtpClient
                Dim e_mail As New MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential(froms, passwordx)
                Smtp_Server.Port = port
                Smtp_Server.EnableSsl = True
                Smtp_Server.Host = smtp

                e_mail = New MailMessage()
                e_mail.From = New MailAddress(froms)
                e_mail.To.Add(tos)
                e_mail.Subject = subj
                e_mail.IsBodyHtml = True
                e_mail.Body = msg
                Smtp_Server.Send(e_mail)
                Return ("Mail Sent")

            Catch error_t As Exception
                If File.Exists("C:\temp\email\email" & Format(Today, "ddmmY") & ".txt") Then
                    Try
                        msg = Now.ToString & error_t.ToString & ">>>" & msg & ">>>to:" & tos & ">>>>>>>>>>>>>>>\n"
                        File.AppendAllText("C:\temp\email\email" & Format(Today, "ddmmY") & ".txt", msg)

                    Catch ex As Exception
                        ' MsgBox(ex.ToString)
                        Return ex.ToString
                    End Try
                Else
                    File.WriteAllText("C:\temp\email\email" & Format(Today, "ddmmY") & ".txt", msg)

                End If
                Return "not sent because " & error_t.ToString
            End Try
        End Function
    End Class
    Public Class payrollmake
        Shared Function makeformpaidx_payroll(ByVal ref As String, ByVal con As SqlConnection, ByVal cname As String) 'View
            'view paid employees
            If ref <> "" Then
                'Session.Timeout = 60
                Dim pdate1, pdate2 As Date
                Dim nod As Integer
                Dim paidlist As String = ""
                Dim fl As New file_list
                Dim namelist As String = ""
                Dim emptid As String
                Dim cell(17) As Object
                Dim cellb() As String
                Dim cellbval() As String
                Dim rrr As DataTableReader
                Dim sum(15) As Double
                Dim avalue As String = ""
                Dim pemp, pco, netincom As Double
                ' Dim ref As String

                Dim sumbsal, sumbearn, sumtalw, sumalw, sumot, sumgross, sumtincome, sumtax, sumloan(), sumpemp, sumpco, sumnet, sumtd As Double
                sumbsal = 0
                sumbearn = 0
                sumtalw = 0
                sumalw = 0
                sumot = 0
                sumgross = 0
                sumtincome = 0
                sumtax = 0
                sumpemp = 0
                sumpco = 0
                sumtd = 0
                sumnet = 0
                Dim sal As String
                Dim headcop As String = "none"
                Dim dbs As New dbclass
                Dim strc As Object = "0"
                Dim fm As New formMaker
                Dim sec As New k_security
                Dim sql As String
                Dim outp As String = ""
                Dim calc, ca, clwp, newemp As Double
                Dim damt As Double = 0
                'For Each k As String In Request.ServerVariables
                'Response.Write(k & "=" & Request.ServerVariables(k) & "<br>")
                '  Next

                'Response.Write(fl.msgboxt("onfram", "Progression", "Progression shown"))
                'Response.Write("<script>showobj('progressbar');</script>")

                ' Response.Write(ref)
                Dim spl() As String
                ' nod = Date.DaysInMonth(Request.QueryString("year"), Request.QueryString("month"))

                Dim rs As DataTableReader
                Dim rs2 As DataTableReader
                Dim ccol As Integer = 0
                Dim paid As String
                Dim tempempid As String
                Dim j As Integer
                Dim ccol2 As Integer
                Dim reasonname() As String
                Dim paypaid As Integer = 0
                ' paypaid = fm.getinfo2("select id from payrol_reg where month='" & Request.Form("month") & "' and year=" & Request.Form("year"), con)
                If paypaid <> 0 Then
                    ' Response.Write(fm.getinfo2("select count(id) from emp_loan_settlement where ref=" & paypaid & " group by loan_no", con))

                End If
                paid = ""
                paid = fm.getinfo2("select ref from payrollx where ref='" & ref & "'", con)


                ccol = 0
                If paid <> "None" Then
                    ccol = CInt(fm.getinfo2("SELECT COUNT(cnt) AS Expr1 froM (SELECT distinct reason AS cnt FROM vwloanref WHERE  ref='" & ref & "')  AS derivedtbl_1", con))
                    ' ' Response.Write("SELECT COUNT(cnt) AS Expr1 froM (SELECT distinct reason AS cnt FROM vwloanref WHERE  ref='" & ref & "')  AS derivedtbl_1")
                    pdate2 = fm.getinfo2("select pddate from payrollx where ref='" & ref & "'", con)
                    tempempid = fm.getinfo2("select emptid from payrollx where ref='" & ref & "'", con)
                End If

                pdate1 = pdate2.Month & "/1/" & pdate2.Year
                pdate2 = pdate1.Month & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month) & "/" & pdate1.Year
                'Response.Write(ccol)
                'ccol = CInt(fm.getinfo2("SELECT COUNT(count) AS Expr1 froM (SELECT reason AS count FROM vw_loan WHERE Bal > 0 AND (dstart <= '" & pdate1 & "') GROUP BY reason) AS derivedtbl_1", con))
                ReDim cellb(ccol)
                ReDim cellbval(ccol)
                ReDim sumloan(ccol)
                ReDim reasonname(ccol)
                Dim stylew(4) As String
                Dim tcell As Integer
                Dim wdthpay As Integer = 1400
                tcell = ccol + 13

                Dim ratiow As Double
                ratiow = (wdthpay - 350 - 30 - 60) / tcell
                stylew(0) = "30px"
                stylew(1) = "60px"
                stylew(2) = "350px"
                stylew(3) = Math.Round(ratiow, 0).ToString & "px"

                strc = (fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", con))
                If strc.ToString.Length > 3 Then
                    pemp = 0
                Else
                    pemp = CDbl(strc)
                End If
                strc = "0"
                strc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<='" & pdate1 & "' order by id desc", con)
                If strc.Length > 3 Then
                    pco = "0"
                Else
                    pco = CDbl(strc)
                End If
                If ref <> "" Then
                    rs = dbs.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.id in(select emptid from payrollx where ref='" & ref.ToString & "') ORDER BY emp_static_info.first_name", con)
                Else
                    rs = Nothing
                End If
                outp = "Sorry Can't Process"
                Dim projname() As String
                If rs.HasRows Then
                    outp = "<table id='tb1' cellspacing='0' cellpadding='3'>" & Chr(13)
                    outp &= "<tr style='text-align:center;font-weight:bold;font-size:17pt' >" & Chr(13)
                    outp &= "<td class='toptitle' style='text-align:center;font-weight:bold;' colspan='" & (19 + ccol).ToString & "' >" & cname & _
                    "<br> Project Name:"
                    'rs.Read()
                    ''Session("chgref") = (Request.ServerVariables("HTTP_REFERER")) & "?paidst=paid&" & (Request.ServerVariables("QUERY_STRING"))

                    ' Response.Write(Session("chgref"))
                    'Response.Write(Request.QueryString("projname"))
                    projname = fm.getproj(tempempid, pdate1, pdate2, con)
                    outp &= projname(1)
                    outp &= "<br> Payroll Sheet for the month: " & MonthName(pdate1.Month) & " " & pdate1.Year & _
                    "<br>" & ref & "</td>" & Chr(13)

                    outp &= "</tr>" & Chr(13)

                    outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                    outp &= "<td class='dw' rowspan='2' >No.</td><td class='dw' rowspan='2'>Emp. Id</td>"
                    outp &= "<td class='fxname'  rowspan='2'>Full Name</td>"
                    outp &= "<td class='fitx' rowspan='2'>Basic Salary</td>" & Chr(13)
                    outp &= "<td class='dw' rowspan='2'>Days Worked</td>"
                    outp &= "<td class='fitx' rowspan='2'>Basic Earning</td>"
                    outp &= "<td class='fitx' rowspan='2'>Taxable Allowance</td>"
                    outp &= "<td class='fitx' rowspan='2'> Allowance</td>" & Chr(13)
                    outp &= "<td class='fitx' rowspan='2'> Overtime</td>"
                    outp &= "<td class='fitx' rowspan='2'>Gross Earning</td>" & Chr(13)
                    outp &= "<td  class='fitx' rowspan='2'>Taxable Incom</td>"
                    outp &= "<td class='fitx' rowspan='2'>Tax</td>" & Chr(13)
                    outp &= "<td class='dedct' style='' colspan='" & ccol.ToString & "' >Deduction</td>"
                    outp &= "<td class='fitx' rowspan='2'>Pension " & pemp & "%</td>" & Chr(13)
                    outp &= "<td class='fitx' rowspan='2'>pension " & pco & "%</td>"
                    outp &= "<td class='fitx' rowspan='2'>Total Deduction</td>" & Chr(13)
                    outp &= "<td class='fitx' rowspan='2'>Net Pay</td>"


                    outp &= "<td class='dw' id='chkall' style='cursor:pointer;width:30px;' rowspan='2' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)

                    outp &= "<td rowspan='2' class='signpart'>Signature</td></tr>" & Chr(13)

                    rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vwloanref where ref='" & ref & "' order by reason", con)


                    ' rs2 = dbs.dtmake("dbpen", "select DISTINCT reason from vw_loan where bal>0  and dstart < ='" & pdate1 & "'", con)
                    outp &= "<tr>" & Chr(13)
                    If ccol = 0 Then
                        outp &= "<td class='dedctx' >&nbsp;</td>"
                    Else
                        If rs2.HasRows Then
                            Dim i As Integer = 0
                            While rs2.Read

                                If rs2.Item("reason") = "-" Then
                                    outp &= "<td class='dedctx'>Other</td>"
                                Else
                                    outp &= "<td  class='dedctx'>" & rs2.Item("reason").ToString & "</td>"
                                End If
                                reasonname(i) = rs2.Item("reason")
                                i += 1
                            End While
                        End If
                    End If


                    outp &= "</tr>" & Chr(13)
                    rs2.Close()
                    Dim k As Integer = 1
                    Dim color As String = ""
                    Dim resing As Date
                    Dim did As String
                    Dim loanid As String = ""
                    Dim otid As String = ""

                    While rs.Read
                        ' nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))

                        emptid = rs.Item("id")
                        paid = ""
                        paid = fm.getinfo2("select id from payrollx where ref='" & ref & "'", con)
                        '  Response.Write(paid.ToString & "<br>...")
                        If paid.ToString <> "None" Then
                            ' paidlist &= fm.getinfo2("select id from paryrollx where emptid='" & emptid & "' and payroll_id in(select id from payrol_reg where month='" & pdate1.Month & "' and year='" & pdate1.Year & "') and (ref_inc='0' or ref_inc IS NULL)", con) & ","
                            '  Response.Write(paidlist & "<br>")
                            resing = "#1/1/1900#"
                            If color <> "#aabbcc" Then
                                color = "#aabbcc"
                            Else
                                color = "white"
                            End If

                            cell(0) = rs.Item("emp_id")
                            cell(1) = fm.getfullname(rs.Item("emp_id"), con)
                            ' sql = "select basic_salary from emp_sal_info where date_start<='" & pdate2 & "' and ISNULL(date_end, { fn NOW() })>'" & pdate1 & "' and emptid=" & rs.Item("id").ToString
                            'Response.Write(sql & "<br>")
                            sal = fm.getinfo2("select b_sal from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            cell(2) = sal
                            If cell(2) = "Sorry This employee salary info is not setted!" Then
                                cell(2) = "0"
                                '  color = "#ccaa99"
                            End If
                            'Response.Write(paid)

                            'Response.Write(calc.ToString)

                            cell(3) = fm.getinfo2("select no_day from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            'nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                            cell(4) = fm.getinfo2("select b_e from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            cell(5) = fm.getinfo2("select talw from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            '  Response.Write(cell(5).ToString)

                            cell(6) = fm.getinfo2("select alw from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            cell(7) = fm.getinfo2("select ot from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)

                            'otid &= fm.getinfo2("select id from emp_ot where emptid=" & emptid & " and ot_date='" & pdate2 & "' and paidstatus='y'", con) & ","

                            cell(8) = fm.getinfo2("select gross_earnings from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            'Response.Write(cell(8).ToString & "<br>")
                            cell(9) = fm.getinfo2("select txinco from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            cell(10) = fm.getinfo2("select tax from payrollx where emptid=" & emptid & " and ref='" & ref & "'", con)
                            ' Response.Write(cell(10) & "<br>")
                            rs2.Close()
                            j = 0
                            Dim loaninf As String
                            loaninf = fm.getinfo2("select ref from emp_loan_settlement where ref='" & ref & "'", con)

                            If loaninf <> "None" Then
                                rs2 = dbs.dtmake("loan", "select id,loan_no,reason from vwloanref where ref='" & ref & "' and emptid=" & emptid & " order by reason", con)

                                If rs2.HasRows Then
                                    j = 0
                                    While rs2.Read
                                        'Response.Write(reasonname.Length.ToString)
                                        For j = 0 To reasonname.Length - 1
                                            ' Response.Write(rs2.Item("loan_no") & "=" & emptid & "<br>")

                                            ' Response.Write(reasonname(j) & "=" & rs2.Item("reason") & "<br>")
                                            If reasonname(j) = rs2.Item("reason") Then
                                                cellb(j) = fm.getinfo2("select amount from emp_loan_settlement where id='" & rs2.Item("id") & "'", con)
                                                cellbval(j) = "0"
                                                sumloan(j) += CDbl(cellb(j))
                                                ' j += 1

                                            ElseIf reasonname(j) <> rs2.Item("reason") Then
                                                If cellbval(j) = "" Then
                                                    cellb(j) = "0.00"
                                                    cellbval(j) = "0"
                                                    sumloan(j) += CDbl(cellb(j))
                                                End If
                                                ' j += 1
                                            End If

                                        Next

                                    End While
                                Else
                                    For j = 0 To reasonname.Length - 1
                                        cellb(j) = "0.00"
                                        cellbval(j) = "0"
                                        sumloan(j) += CDbl(cellb(j))
                                    Next

                                End If
                            Else

                                For j = 0 To reasonname.Length - 1
                                    cellb(j) = "0.00"
                                    cellbval(j) = "0"
                                    sumloan(j) += CDbl(cellb(j))
                                Next

                            End If
                            rs2.Close()

                        End If
                        ' rs2 = dbs.dtmake("loan", "select id,bal,nopay,amt,reason from vw_loan where bal>0 and dstart < ='" & pdate1 & "' and emptid=" & emptid & " order by id", con)
                        Dim penss As String
                        penss = fm.getinfo2("select id from emp_pen_start where emptid=" & emptid & " and penstart<='" & pdate1 & "' order by id desc", con)
                        ' Response.Write(penss & "<br>")

                        cell(12) = fm.getinfo2("select pen_e from payrollx where ref='" & ref & "' and emptid=" & rs.Item("id"), con)
                        If cell(12) = "None" Then
                            cell(12) = "0"
                        End If



                        cell(13) = fm.getinfo2("select pen_c from payrollx where ref='" & ref & "' and emptid=" & rs.Item("id"), con)
                        If cell(13) = "None" Then
                            cell(13) = "0"
                        End If




                        ' Response.Write(cell(3).ToString & "===")

                        sumbsal += CDbl(cell(2))
                        sumbearn += CDbl(cell(4))
                        sumtalw += CDbl(cell(5))
                        sumalw += CDbl(cell(6))
                        sumot += CDbl(cell(7))
                        sumgross += CDbl(cell(8))
                        sumtincome += CDbl(cell(9))
                        sumtax += CDbl(cell(10))
                        sumpemp += CDbl(cell(12))
                        sumpco += CDbl(cell(13))


                        Dim tdd As Double = 0
                        For i As Integer = 0 To ccol - 1
                            If color <> "#ccaa99" Then
                                tdd += CDbl(cellb(i))
                            End If
                        Next
                        cell(14) = (CDbl(cell(12)) + tdd + CDbl(cell(10))).ToString
                        If CDbl(cell(14)) > 0 Then
                            sumtd += CDbl(cell(14))
                        End If
                        If cell(2).ToString <> "0" And cell(3) <> "0" Then
                            outp &= "<tr><td id='" & emptid & "_0-" & k.ToString & "'>" & k.ToString & Chr(13)

                            For i As Integer = 0 To 14
                                If (cell(3).ToString = "0" And IsNumeric(cell(i)) = True) Then

                                    cell(i) = 0
                                End If
                                If color = "#ccaa99" And IsNumeric(cell(i)) = True Then
                                    cell(i) = 0
                                End If
                                If i <> 11 Then
                                    'Response.Write("isNUmeric: " & cell(i) & "=" & IsNumeric(cell(i)).ToString & "***")

                                    '   Response.Write(cell(i).ToString)
                                    If i = 0 Or i = 1 Or i = 3 Then
                                        outp &= ("<td style='text-align:left;' id='" & emptid.ToString & "_" & i & "'>&nbsp;" & cell(i).ToString & "</td>")
                                    Else
                                        outp &= ("<td style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>" & fm.numdigit(cell(i).ToString, 2) & "</td>")

                                    End If
                                Else
                                    For j = 0 To ccol - 1
                                        If String.IsNullOrEmpty(cellbval(j)) = False Then
                                            outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-" & cellbval(j).ToString & "'>" & fm.numdigit(cellb(j).ToString, 2) & "</td>")
                                        Else
                                            outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i.ToString & "-" & j.ToString & "-0'>0.00</td>")
                                        End If
                                    Next
                                    If ccol = 0 Then
                                        outp &= ("<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_" & i & "'>&nbsp;</td>")
                                    End If
                                End If
                            Next
                            netincom = CDbl(cell(8)) - CDbl(cell(14))
                            If color <> "#ccaa99" Then
                                sumnet += netincom
                            End If
                            ' Response.Write(netincom.ToString & "<----<br>")
                            outp &= "<td  style='text-align:right;mso-number-format:\#\,\#\#0\.00'  id='" & emptid.ToString & "_15'>" & fm.numdigit(CDbl(netincom), 2).ToString & "</td>"
                            outp &= "<td  style='text-align:right;'>"
                            If paid.ToString = "None" And CDbl(cell(2)) <> 0 Then
                                outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked' onclick='javascript:sumcolx();'></td>"
                            Else
                                If CDbl(cell(2)) <> 0 Then
                                    outp &= " Paid</td>"

                                    '  outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' style='visibility:hidden;'>Paid</td>"
                                Else
                                    outp &= " None</td>"

                                    'outp &= " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='' disabled='disabled' ></td>"

                                End If


                            End If
                            outp &= "<td style='text-align:right;'>&nbsp;</td></tr>" & Chr(13)
                            k += 1
                        End If

                    End While


                    outp &= "<tr style='text-weight:bold;text-align:right;'>" & _
                    "<td class='cooo' >&nbsp;</td><td class='cooo'>..&nbsp;</td><td class='cooo'>..&nbsp;</td>" & _
                    "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bsal'>" & fm.numdigit(sumbsal.ToString, 2).ToString & "</td>" & _
                     "<td class='cooo' style='text-align:right;'>&nbsp;</td>" & _
                     "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='bearn'>" & fm.numdigit(sumbearn, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtalw'>" & fm.numdigit(sumtalw, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumalw'>" & fm.numdigit(sumalw.ToString, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumot'>" & fm.numdigit(sumot, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumgross'>" & fm.numdigit(sumgross, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumti'>" & fm.numdigit(sumtincome, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumt'>" & fm.numdigit(sumtax, 2).ToString & "</td>"


                    For j = 0 To ccol - 1
                        outp &= ("<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='lon-" & j & "'>" & fm.numdigit(sumloan(j).ToString, 2) & "</td>")
                    Next
                    If ccol = 0 Then
                        outp &= ("<td class='cooo' style='text-align:right;'>&nbsp;</td>")
                    End If
                    outp &= "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpemp'>" & fm.numdigit(sumpemp, 2).ToString & "</td>" & _
                    "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumpco'>" & fm.numdigit(sumpco, 2).ToString & "</td>" & _
                     "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumtd'>" & fm.numdigit(sumtd, 2).ToString & "</td>" & _
                      "<td class='cooo' style='text-align:right;mso-number-format:\#\,\#\#0\.00' id='sumnet'>" & fm.numdigit(sumnet, 2).ToString & "</td>" & _
                      "<td class='cooo'  colspan='2'>..&nbsp;" & _
            "</td></tr><tr id='result'></tr>"
                    outp &= fm.signpart()
                    outp &= "</table>"
                    rs.Close()
                    outp &= fm.projtrans(projname(1), pdate1, con)
                    fm = Nothing
                    dbs = Nothing
                    ' Response.Write(outp)
                    ' Response.Write("<input type=hidden id='delpaid' name='delpaid' value='" & paidlist & "⌡" & loanid & "⌡" & otid & "'>")
                    ' Response.Write("payrol no. list:" & paidlist & "<br>loan settled list: " & loanid & "<br>OT List:" & otid)
                End If
                'Dim xprint As String = ""
                ' bsal = fm.getinfo2("select basic_salary from emp_sal_info where date_start>='" & pdate1 & " and date_start='" & date2 & "' and emptid=" & emptid, con)
                Return outp
            End If
        End Function
       
        Function payroll1()

        End Function
        Function payroll2()

        End Function
        Function payroll3()

        End Function
    End Class
    Class mail_system_new

        Private emsg As String
        Private subj As String
        Private tos As String
        Private froms As String
        Private port As String
        Private con As SqlConnection
        Private pathx As String
        Private smtpemail As String
        Private smtp As String
        Private passwordx As String
        Dim sec As New k_security
        Dim fm As New formMaker
        Dim dbs As New dbclass
        Function init(ByVal emsgx, ByVal subjx, ByVal tosx, ByVal fromsx, ByVal portx, ByVal conx, ByVal pathh, ByVal smtpx, ByVal smtpemailx, ByVal passwordxx)
            emsg = emsgx
            subj = subjx
            tos = tosx
            froms = fromsx
            port = portx
            con = conx
            pathx = pathh
            smtp = smtpx
            smtpemail = smtpemailx
            passwordx = passwordxx
        End Function
        Public Function isemailexist()




            If fm.getinfo2("select mailid from tblemail where mailcont='" & sec.StrToHex3(emsg) & "' and senddate='" & Today.ToShortDateString & "'", con) = "None" Then
                Return False

            Else
                Return True
            End If

        End Function
        Function mailreg()
            Dim flname As String

            flname = sec.StrToHex3(Now.ToString)

            Dim sql As String = "insert into tblemail(mailid,maildatetime,sendrpt,subject,mailcont) values('" & flname & _
                       "','" & Now.ToString & _
                       "','unsend" & _
                       "','" & subj & "','" & _
                       sec.StrToHex3(emsg) & "')"
            dbs.excutes(sql, con, pathx)
        End Function



        Function sendemail()
            Try
                Dim Smtp_Server As New SmtpClient(smtp, port)

                Dim e_mail As New MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential(smtpemail, passwordx)
                ' Smtp_Server.Port = 587

                Smtp_Server.EnableSsl = True
                e_mail = New MailMessage()
                e_mail.From = New MailAddress(froms)
                e_mail.To.Add(tos)
                e_mail.Subject = subj
                e_mail.IsBodyHtml = True
                e_mail.Body = emsg
                Smtp_Server.Send(e_mail)
                Return "msg sent"
                Dim sql As String = "update tblemail set sendrpt='sent', senddate='" & Now.ToString & "' where mailcont='" & sec.StrToHex3(emsg) & "'"
                dbs.excutes(sql, con, pathx)
            Catch error_t As Exception
                If File.Exists("C:\temp\email\email.txt") Then
                    Try
                        emsg = Now.ToString & error_t.ToString & ">>>" & emsg & ">>>to:" & tos & ">>>>>>>>>>>>>>>\n"
                        File.AppendAllText("C:\temp\email\email.txt", emsg)
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try

                End If
                Return "not sent because " & error_t.ToString
            End Try
        End Function
    End Class
    Public Class dtime
        Function convert_to_GC(ByVal da As String)


            Try

                Dim datefx, datef(3) As String
                datefx = da.ToString
                Dim datefxx() As String
                datefxx = da.Split(".")
                'Response.Write(datefxx(0))
                datef(0) = datefxx(2)
                datef(1) = datefxx(1)
                datef(2) = datefxx(0)
                Dim ma As Integer = 0
                Dim daydiff() As Integer
                Dim md As Integer = CInt(8 - 12)
                Dim dayinm() As Integer = {0, 30, 31, 30, 31, 31, 28, 31, 30, 31, 30, 31, 31}
                Dim dd, mm, yy As Integer
                If datef(1) >= 1 And datef(1) < 7 Then
                    If CInt(datef(0) - 1) Mod 4 = 3 Then 'leap year date differnce
                        '  Response.Write("leap year<br>")


                        daydiff = New Integer() {0, 11, 11, 10, 10, 9, 8, 8, 8, 8, 8, 7, 7, 5}
                        dayinm = New Integer() {0, 30, 31, 30, 31, 31, 29, 31, 30, 31, 30, 31, 31}
                    Else
                        '{0, 9, 8, 9, 8, 8, 7, 7, 10, 10, 10, 9, 9}
                        daydiff = New Integer() {0, 10, 10, 9, 9, 8, 7, 8, 9, 8, 8, 7, 7, 5}
                    End If
                Else


                    If CInt(datef(0)) Mod 4 = 0 Then 'leap year date differnce

                        daydiff = New Integer() {0, 11, 11, 10, 10, 9, 8, 9, 8, 8, 7, 7, 6, 5}
                        dayinm = New Integer() {0, 30, 31, 30, 31, 31, 29, 31, 30, 31, 30, 31, 31}
                    Else
                        daydiff = New Integer() {0, 10, 10, 10, 9, 9, 9, 9, 8, 8, 7, 7, 6, 5}
                    End If
                End If
                If datef(1) <> 13 Then
                    If (datef(2) + daydiff(datef(1))) > dayinm(datef(1)) Then
                        ma = 1
                        dd = datef(2) + daydiff(datef(1)) - dayinm(datef(1))
                        ' Response.Write(daydiff(datef(1)) & "====" & dayinm(datef(1)) & "<br>")
                    Else
                        ma = 0
                        dd = daydiff(datef(1)) + datef(2)

                        'yy = datef(0) + 7
                    End If
                Else
                    mm = 9
                    If (datef(2) + daydiff(datef(1))) >= dayinm(datef(1) - 1) Then
                        ma = 1
                        dd = datef(2) + daydiff(datef(1)) - dayinm(datef(1) - 1)

                    Else
                        ma = 0
                        dd = daydiff(datef(1)) + datef(2)
                    End If

                End If
                If datef(1) < 5 And mm <> 9 Then
                    mm = datef(1) + 8 + ma
                ElseIf mm <> 9 Then
                    mm = datef(1) - 4 + ma
                End If
                If mm = 13 Then
                    mm = 1
                End If
                If mm > 9 And ma <= 12 Then

                    yy = datef(0) + 7


                ElseIf mm >= 1 And mm < 9 Then

                    yy = datef(0) + 8

                ElseIf mm = 9 Then
                    If datef(1) = 13 Then
                        yy = datef(0) + 8
                    Else
                        If daydiff(datef(1)) >= datef(2) Then
                            'Response.Write(daydiff(datef(1)) & " <= " & datef(2) & "<br>")
                            yy = datef(0) + 7
                        Else
                            yy = datef(0) + 8
                        End If
                    End If


                End If
                ' yy = 2016
                Return mm.ToString & "/" & dd.ToString & "/" & yy.ToString
            Catch ex As Exception
                Return ex.ToString
            End Try
        End Function
        Public Function convert_to_ethx(ByVal da As Date)
            Dim lp As Boolean = False
            Dim lpd As Integer = 0
            Dim d, m, y As Integer
            d = da.Day
            m = da.Month
            y = da.Year
            Dim etd, etm, ety As Integer
            Dim dx() As Integer
            Dim rtn As String = ""
            If y Mod 4 = 3 Then
                lp = True   '0123456789101112
                dx = New Integer() {0, 8, 7, 9, 8, 8, 7, 7, 6, 11, 11, 10, 10}

            ElseIf y Mod 4 = 0 Then
                dx = New Integer() {0, 9, 8, 9, 8, 8, 7, 7, 6, 10, 10, 9, 9}
            Else

                dx = New Integer() {0, 8, 7, 9, 8, 8, 7, 7, 6, 10, 10, 9, 9}
            End If
            etd = CInt(d) - CInt(dx(m))
            Dim etdd As Integer = CInt(d) - CInt(dx(m))
            Dim yrm As Integer = 0
            Dim dtmx As Integer = 0
            If etd <= 0 Then
                ' rtn &= ">>>>>" & etd.ToString & "<<<<--dmmm-change>>>>"
                dtmx = 1
                If lp = True Then
                    lpd = 6
                Else
                    lpd = 5
                End If
                If m = 9 Then
                    etd = 30 + etd + lpd
                    etm = 12
                    If etd > 30 Then
                        etm = 13
                        etd = lpd + etdd
                    End If

                Else
                    If m = 9 Then
                        yrm = 1
                    End If
                    etd = 30 + etd


                End If
            Else
                If m = 9 Then

                End If
                'rtn &= etd & "grater date>>>>>"

            End If

            If m <= 8 Then
                ety = y - 8
            ElseIf etm = 13 Then

                ety = y - 8


            ElseIf etm <> 0 Then
                ety = y - 8
            Else

                ety = y - 7
                ' rtn &= " >>>>yr<<<< " & ety & "<<<<"

            End If
            If etm = 0 Then
                If etm <> 13 Then
                    If m > 9 Then
                        '      rtn &= "mmmm " & m & "<<<<<"
                        etm = m - 8 - dtmx
                    ElseIf m = 9 Then
                        If dtmx > 1 Then
                            etm = 12
                        Else
                            etm = m - 8 - dtmx
                        End If
                    Else
                        '       rtn &= "mmmm " & m & "<<<<<"
                        etm = m + 4 - dtmx
                    End If
                End If
            End If
            Return etd.ToString & "." & etm.ToString & "." & ety.ToString & rtn

        End Function
        Function convert_year_gc_eth(ByVal y As Integer, ByVal m As Integer, ByVal d As Integer)
            Dim rtnyr As Integer
            Dim ld As Integer
            If y Mod 4 = 3 Then
                ld = 11
            Else
                ld = 10

            End If
            If d = 0 Then
                If m >= 9 Then
                    rtnyr = y - 7
                ElseIf m < 9 Then
                    rtnyr = y - 8
                End If
            ElseIf d > 0 Then
                If m = 9 And d <= ld Then
                    rtnyr = y - 8
                ElseIf m >= 9 Then
                    rtnyr = y - 7

                End If


            End If
            Return rtnyr
        End Function


        Function getmonth(ByVal m As Integer, ByVal rtn As String)
            Dim amhmonthNames() As String = {"", "መስከረም", "ጥቅምት", "ኅዳር", "ታህሣሥ", "ጥር", "የካቲት", "መጋቢት", "ሚያዝያ", "ግንቦት", "ሰኔ", "ሐምሌ", "ነሐሴ", "ጳጉሜ"}
            Dim amhmonthshort() As String = {"", "መስከ", "ጥቅም", "ኅዳር", "ታህሣ", "ጥር", "የካቲ", "መጋቢ", "ሚያዝ", "ግንቦ", "ሰኔ", "ሐምሌ", "ነሐሴ", "ጳጉሜ"}
            Dim ethmonth() As String = {"", "Meskerm", "Tikmet", "Hidar", "Tahisas", "Tir", "Yekatite", "Megabit", "Miazia", "Ginbot", "Sene", "Hamele", "Nehasse", "Pagume"}

            If rtn = "amh" Then
                Return amhmonthNames(m)
            ElseIf rtn = "amh_short" Then
                Return amhmonthshort(m)

            ElseIf rtn = "amheng" Then
                Return ethmonth(m)
            End If
        End Function
        Function getdatepart_et(ByVal part As String, ByVal ethdate As String)
            Dim spl() As String
            spl = ethdate.Split(".")
            Dim rtn As String = IIf(part = "m", spl(1), IIf(part = "d", spl(0), IIf(part = "y", spl(2), 0)))
            Return rtn

        End Function
    End Class
End Namespace
