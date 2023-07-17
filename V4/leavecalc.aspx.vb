Imports System.Data.SqlClient
Imports System.Data
Imports Kirsoft.hrm
Partial Class leavecalc
    Inherits System.Web.UI.Page
    Function outpage()
        Dim sql, strview As String
        Dim db As New dbclass
        Dim fm As New formMaker
        Dim dbcal As New datetimecal
        Dim dt, dt2 As DataTableReader
        Dim nod, sumav As Double
        Dim d As Date
        Dim give As Double
        Dim r() As Object
        Dim cout As Integer = 0

        'Dim rqid As Integer
        Dim outp As String = ""
        sumav = 0
        Dim v, sumavx As Double
        Dim rid As Integer
        Dim bgt As String
        Dim req As String
        Dim redirect As String = ""
        Dim appdate As Date
        bgt = ""
        req = ""
        Dim i As Integer = 0
        If Request.QueryString("other") = "on" Then
            appdate = Request.QueryString("appdate")
            If appdate.ToString = "" Then
                appdate = Today.ToShortDateString
            Else
                appdate = appdate & " 12:00:00 AM"
            End If
            sql = "update emp_leave_take set date_return='" & Request.QueryString("datereturn") & "',approved_by='" & Session("emp_iid") & "',approved_date='" & appdate & "' where id=" & Request.QueryString("id")
            If db.save(sql, session("con"), session("path")) = 1 Then
                redirect = "leavetake.aspx"
            Else
                Response.Write("Sorry, Not Approved")
            End If
        Else

            If Request.QueryString("approved") = "on" Then
                Dim bgt1() As String = Request.QueryString("bgt").Split(",")
                Dim req1() As String = Request.QueryString("nd").Split(",")
                appdate = Request.QueryString("appdate")
                If appdate.ToString = "" Then
                    appdate = Today.ToShortDateString
                Else
                    appdate = appdate & " 12:00:00 AM"
                End If
                Dim val, fld As String
                val = "("
                fld = "("
                Dim sf(8) As String
                For i = 0 To req1.Length - 2
                    val = "("
                    If req1(i).ToString <> "" Then
                        val &= "'" & req1(i) & "'"
                        If bgt1(i).ToString <> "" Then
                            val &= ",'" & bgt1(i) & "'"
                        Else
                            val &= ",'0'"
                        End If

                        val &= ",'" & Request.QueryString("id").ToString & "','y')"
                        sql = "insert into empleavapp(used,bgt_id,req_id,approved) values " & val

                        'sf(i) = db.save(sql, session("con"),session("path"))
                        Response.Write("<br>" & sql)
                        sql = ""
                    Else
                        sf(i) = ""
                    End If

                Next

                sql = "update emp_leave_take set date_return='" & Request.QueryString("datereturn") & "',approved_by='" & Session("emp_iid") & "',approved_date='" & appdate & "' where id=" & Request.QueryString("id")

                Dim flg As Boolean = False
                For j As Integer = i To 0 Step -1
                    If sf(j) <> "" Then
                        flg = True
                    End If
                Next
                If flg = True Then
                    sf(6) = db.save(sql, session("con"), session("path"))
                    Response.Write(sql)
                End If
                If sf(6) <> 1 Then
                    Response.Write("<label style='color:red;'>Sorry Data is not saved try later and/or contact admin :<br> error:" & sql & "<br>" & Today.ToShortDateString & "</lable>")
                Else
                    redirect = "leavetake.aspx"
                End If
                ' Response.Write(sf(i + 1).ToString)
                'Response.Write(sql)
                sql = ""

            End If
        End If
        Select Case Request.QueryString("task")
            Case "Approved"
                Dim finof As String = ""
                Dim info As String = ""
                finof = "<table><tr><td>Emp-Id:" & Session("emp_id") & "</td></tr><tr><td>Full Name</td><td>:</td><td>" & getinfo("first_name", "emp_static_info", "emp_id='" & Session("emp_id") & "'") & " " & getinfo("middle_name", "emp_static_info", "emp_id='" & Session("emp_id") & "'") & " " & getinfo("last_name", "emp_static_info", "emp_id='" & Session("emp_id") & "'") & "</td></tr>"
                info = getinfo("project_id", "emp_job_assign", "emp_id='" & Session("emp_id") & "'")
                finof &= "<tr><td>project Id:<a href=" & Chr(34) & "javascript:getproject('" & info & "');" & Chr(34) & ">" & info & "</a></td></tr>"
                finof &= "</table>"
                sql = "select * from emp_leave_take where id=" & Request.QueryString("id")
                dt = db.dtmake("thscal", sql, Session("con"))
                sql = "select * from show_leave_bal where emp_id='" & Session("emp_id") & "' and emptid=" & Session("emptid")
                dt2 = db.dtmake("thscals", sql, Session("con"))
                If dt.HasRows = True Then
                    dt.Read()
                    If LCase(dt.Item("leave_type")) = "annual leave" Then
                        If dt2.HasRows = True Then
                            strview = "<table cellspacing='0' cellpadding='0' width='900px'>" & _
                        "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>"
                            For j As Integer = 1 To dt2.FieldCount - 1
                                strview &= "<td  style='padding-right:10px;'>" & dt2.GetName(j) & "</td>"
                            Next
                            strview &= "</tr>"
                            Dim color As String = "E3EAEB"
                            ' Dim remx As Double
                            nod = dt.Item("no_days")
                            cout = 0

                            outp = "<table cellspacing='0' cellpadding='0' width='900px'>"
                            outp &= "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td>Request Id</td><td>Budget id</td><td>Period</td><td>Budget days</td><td>Avl. Days</td><td>No. Days Req.</td><td>under</td></tr>"

                            sumavx = 0
                            While dt2.Read
                                give = 0

                                If color <> "#E3EAEB" Then
                                    color = "#E3EAEB"
                                Else
                                    color = "#fefefe"
                                End If
                                If fm.isexp(Today.ToShortDateString, dt2.Item("Year End"), 2, "y") Then
                                    color = "red"
                                Else
                                    v = showavdate(dt2.Item("Year Start"), dt2.Item("Year End"), dt2.Item("Balance"))
                                    sumavx = sumavx + v
                                    If dt2.Item("Balance") > 0 And color <> "red" Then
                                        sumav += dt2.Item("Balance")
                                    End If

                                    If dt2.Item("Balance") > 0 Then
                                        If nod <= dt2.Item("Balance") Then
                                            give = nod
                                            nod = 0
                                        Else
                                            give = dt2.Item("Balance")
                                            nod = nod - dt2.Item("Balance")
                                        End If

                                    End If
                                    If give > 0 Then
                                        outp &= "<tr>"
                                        outp &= "<td>" & Request.QueryString("id") & "</td>"
                                        outp &= "<td>" & dt2.Item("id") & "</td>"
                                        bgt &= (dt2.Item("id")) & ","
                                        outp &= "<td>" & dt2.Item("Year Start") & " - " & dt2.Item("Year End") & "</td>"
                                        'Dim v As Object = showavdate(dt2.Item("Year Start"), dt2.Item("Year End"), dt2.Item("Budget"))
                                        If Val(v) <= dt2.Item("Balance") Then
                                            outp &= "<td>" & dt2.Item("budget") & "</td>"
                                            outp &= "<td>" & v & "</td>"
                                        Else
                                            outp &= "<td>" & dt2.Item("Balance") & "</td>"

                                            outp &= "<td>" & give & "</td>"
                                        End If
                                        outp &= "<td>" & give.ToString & "</td>"
                                        req &= give.ToString & ","
                                        If Val(v) - give < 0 Then
                                            outp &= "<td style='color:red;'>" & (CInt(v) - give).ToString & "</td>"
                                        Else
                                            outp &= "<td></td>"
                                        End If
                                        outp &= "</tr>"


                                    End If
                                    d = dt.Item("date_taken_from")
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
                                cout += 1
                            End While
                            If nod > 0 Then
                                outp &= "<tr>"
                                outp &= "<td>" & Request.QueryString("id") & "</td>"
                                outp &= "<td>no bugdet</td>"
                                outp &= "<td>-</td>"
                                outp &= "<td>-</td>"
                                outp &= "<td>-</td>"
                                outp &= "<td>" & nod.ToString & "</td>"
                                outp &= "<td style='color:red;'>-" & nod.ToString & "</td>"

                            End If
                            outp &= "</tr>"

                        End If
                        ' Dim r() As Object
                        r = calcwhen(dt.Item("date_taken_from"), dt.Item("no_days"), Session("emp_id"))
                        strview &= "</table>"
                        outp &= "</table>"
                        outp = "<br><br>Please the down information comes only once if you want print it and have it for Evidence<div id='uproved'>" & finof & outp & "<br> Total Bagdeted: " & sumav & "<br> Total Avalable Day: " & CInt(sumavx).ToString & _
                       "<br>" & r(0).ToString & _
                       "<br>Holiday:" & r(1).ToString & _
                       "<br>Weekend:" & r(2).ToString & _
                        "<br>" & r(3) & "</div>"

                        outp &= "<div id=" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('uproved','Report_print','" & Session("company_name") & "<BR> Leave Summery','" & Today.ToLongDateString & "');" & Chr(34) & ">print</div>"

                        Response.Write(strview)
                        Response.Write(outp)
                        Response.Write("<form id=" & Chr(34) & "appvd" & Chr(34) & " name=" & Chr(34) & "appvd" & Chr(34) & " method=" & Chr(34) & "post" & Chr(34) & " action=" & Chr(34) & "" & Chr(34) & "> " & _
                  " Date:<input type=" & Chr(34) & "text" & Chr(34) & " id=" & Chr(34) & "appdate" & Chr(34) & " name=" & Chr(34) & "appdate" & Chr(34) & " value=" & Chr(34))
                        Dim lucur(3) As String
                        lucur(2) = Today.Year.ToString
                        lucur(1) = Today.Month.ToString
                        lucur(0) = Today.Day.ToString
                        Dim sdate As String = lucur(1) & "/" & lucur(0) & "/" & lucur(2)
                        Response.Write(" sdate " & Chr(34) & "/>")
                        Response.Write(" <script language='javascript' type='text/javascript'> ")
                        Response.Write("$(function() {")
                        Response.Write(" $( " & Chr(34) & "#appdate" & Chr(34) & ").datepicker({changeMonth: true,changeYear: true}); ")
                        Response.Write("$( " & Chr(34) & "#appdate" & Chr(34) & " ).datepicker( " & Chr(34) & "option" & Chr(34) & "," & Chr(34) & "dateFormat" & Chr(34) & "," & Chr(34) & "mm/dd/yy" & Chr(34) & ");});")
                        Response.Write("</script>")
                        Response.Write("<a href=" & Chr(34) & "javascript:approvednow('?approved=on&id=" & Request.QueryString("id") & "&datereturn=" & r(4).ToString & "&rid=" & rid & "&bgt=" & bgt & "&nd=" & req & "');>approved</a>")
                        Response.Write("</form>")
                    Else
                        r = calcwhen(dt.Item("date_taken_from"), dt.Item("no_days"), Session("emp_id"))
                        outp = "<div id='uproved'><br><br>Please the down information comes only once if you want print it and have it for Evidence<div id='uproved'>" & outp & "<br> Total Bagdeted: " & sumav & "<br> Total Avalable Day: " & CInt(sumavx).ToString & _
                   "<br>" & r(0).ToString & _
                   "<br>Holiday:" & r(1).ToString & _
                   "<br>Weekend:" & r(2).ToString & _
                    "<br>" & r(3) & "</div>"
                        outp &= "<div id=" & Chr(34) & "print" & Chr(34) & " style=" & Chr(34) & " float:right; width:59px; height:33px; color:Gray;cursor:pointer" & Chr(34) & " onclick=" & Chr(34) & "javascirpt:print('uproved','Report_print','" & Session("company_name") & "<br>Leave Information','" & Today.ToLongDateString & "');" & Chr(34) & ">print</div>"
                        ' Response.Write(strview)
                        Response.Write(finof & outp)
                        ' Response.Write("see what wee see")
                        Response.Write("<form id=" & Chr(34) & "appvd" & Chr(34) & " name=" & Chr(34) & "appvd" & Chr(34) & " method=" & Chr(34) & "post" & Chr(34) & " action=" & Chr(34) & "" & Chr(34) & ">")
                        Response.Write("Date:<input type=" & Chr(34) & "text" & Chr(34) & " id=" & Chr(34) & "appdate" & Chr(34) & " name=" & Chr(34) & "appdate" & Chr(34) & " value=" & Chr(34))
                        Dim lucur(3) As String
                        lucur(2) = Today.Year.ToString
                        lucur(1) = Today.Month.ToString
                        lucur(0) = Today.Day.ToString
                        Dim sdate As String = lucur(1) & "/" & lucur(0) & "/" & lucur(2)
                        Response.Write(sdate & "/>")
                        Response.Write(" <script language='javascript' type='text/javascript'> $(function() {")
                        Response.Write("$( " & Chr(34) & "#appdate" & Chr(34) & ").datepicker({changeMonth: true,changeYear: true	}); ")
                        Response.Write(" $( " & Chr(34) & "#appdate" & Chr(34) & " ).datepicker( " & Chr(34) & "option" & Chr(34) & "," & Chr(34) & "dateFormat" & Chr(34) & "," & Chr(34) & "mm/dd/yy" & Chr(34) & ");});")
                        Response.Write("</script>")
                        Response.Write("  <a href=" & Chr(34) & "javascript:approvednow('?other=on&approved=on&id=<% response.write(Request.QueryString(" & Chr(34) & "id" & Chr(34) & ")) %>&datereturn=<%response.write(r(4).tostring) %>&rid=<%response.write(rid) %>&bgt=<%response.write(bgt) %>&nd=<%response.write(req) %>');" & Chr(34) & ">approved</a>")
                        Response.Write("</form>")
                    End If

                Else
                    Response.Write("top if")
                End If
            Case "disApproved"
                sql = "update emp_leave_take set date_return=Null,approved_by='" & Session("emp_iid") & "',canceled='y' where id=" & Request.QueryString("id")
                db.save(sql, session("con"), session("path"))
                sql = "Delete from empleavapp where req_id=" & Request.QueryString("id")
                db.save(sql, session("con"), session("path"))


                Response.Write(sql)
            Case "canceled"

                sql = "update emp_leave_take set date_return=Null,approved_by='" & Session("emp_iid") & "',canceled='y' where id=" & Request.QueryString("id")
                Response.Write(db.save(sql, session("con"), session("path")).ToString)
                sql = "Delete from empleavapp where req_id=" & Request.QueryString("id")
                Response.Write(sql)
                db.save(sql, session("con"), session("path"))
            Case Else
                Response.Write(Request.QueryString("task"))
        End Select
        If redirect = "leavetake.aspx" Then
            Response.Write("Approved <a href=" & Chr(34) & "javascript:redirectc();" & Chr(34) & ">Close</a>")
            Response.Write("<script type=" & Chr(34) & "text/javascript" & Chr(34) & ">")
            Response.Write("function redirectc(){")
            Response.Write("$(" & Chr(34) & "#form1" & Chr(34) & ").attr(" & Chr(34) & "target" & Chr(34) & "," & Chr(34) & "workarea" & Chr(34) & ");")
            Response.Write("$(" & Chr(34) & "#form1" & Chr(34) & ").attr(" & Chr(34) & "action" & Chr(34) & "," & Chr(34) & redirect & Chr(34) & ");")
            Response.Write("$(" & Chr(34) & "#form1" & Chr(34) & ").submit();}")
            Response.Write("</script>")

        End If
    End Function
    Function showavdate(ByVal ds As Date, ByVal de As Date, ByVal v As Double) As Double
        Dim dif As Integer
        dif = de.Subtract(ds).Days
        Dim dt As Integer
        dt = Today.Subtract(ds).Days
        Dim str As Double = 0
        'str = dif.ToString & " & chr(34) & "===" & dt.ToString
        If (dt - dif) < 0 Then
            str = (dt * v) / dif
        Else
            str = v
        End If
        Return str
    End Function
    Function calcwhen(ByVal dtstart As Date, ByVal noday As Integer, ByVal empid As String) As Array
        Dim hcount As Integer = 0
        Dim we As Integer = 0
        Dim wday As Integer = 0
        Dim dlist As String = ""
        Dim dateend As Date
        dateend = dtstart
        Dim sysd As New datetimecal
        Dim i As Integer = 0
        Dim flg As Boolean
        While noday > 0
            flg = False
            dateend = dtstart.AddDays(i)
            If sysd.isPublic(dateend, Session("con")) = True Then
                hcount += 1
                dlist &= dateend.ToShortDateString.ToString & " Holiday day-" & (i + 1).ToString & "<br>"
                flg = True
            End If
            If sysd.isWeekEnd(dateend, empid, Session("con")).ToString = "True" And flg = False Then
                we += 1
                dlist &= dateend.ToShortDateString & " weekEnd day-" & (i + 1).ToString & "<br>"
                flg = True
            End If
            If flg = False Then
                dlist &= dateend.ToShortDateString & " Week day-" & (i + 1).ToString & "<br>"
                noday -= 1
            End If
            i = i + 1
        End While
        Dim intarr(5) As Object
        intarr(0) = "start Date: " & dtstart & "<br>End date" & dateend
        intarr(1) = hcount
        intarr(2) = we
        intarr(3) = dlist
        intarr(4) = dateend.AddDays(1)
        Return intarr
    End Function
    Function getinfo(ByVal fname As String, ByVal tbl As String, ByVal where As String)
        Dim db As New dbclass
        Dim dt As DataTableReader
        dt = db.dtmake("fullinfo", "select " & fname & " from " & tbl & " where " & where, Session("con"))
        If dt.HasRows Then
            dt.Read()
            Return dt.Item(0)
        End If
        dt.Close()
        db = Nothing
    End Function
End Class
