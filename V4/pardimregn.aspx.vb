Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Imports System.Web.HttpRequest
Partial Class pardimregxn
    Inherits System.Web.UI.Page
    Public sssql As String
    Public Function datamanip()
        Dim namelist As String = ""
        Dim fm As New formMaker
        Dim ds As New dbclass
        Dim fl As New file_list
        Dim spl2() As String
        Dim arrv(4) As String
        Dim check As String = ""
        Dim cach As String = ""
        Dim sql() As String
        Dim i As Integer = 0
        Dim msg As String = ""
        Dim paiddate As String
        Dim paidmthd As String
        Dim paidstate As String
        Dim adv As Double = 0.0
        Dim ref As String
        Dim emptid As String
        ReDim Preserve sql(i + 1)
        sql(0) = ""
        If Request.QueryString("save") = "on" And Request.QueryString("savebd") = "fm1" Then
            Dim ids() As String
            'ids = fm.getids(Request, "hidpass")
            ' Response.Write("save here")
            Dim df As Integer
            Dim d1, d2 As Date
            Dim pb As String
            Dim pbx As String = ""
            paiddate = ""
            paidmthd = ""
            paidstate = "n"
            pb = Request.Form("pb")
            If pb <> "1" Then
                Response.Write(pb)
                pb = "0"

            End If
            'pbx = Request.Item("payreason")

            d1 = Request.QueryString("from_date")
            d2 = Request.QueryString("to_date")
            df = d2.Subtract(d1).Days + 1
            'Response.Write(df)
            Dim val() As String
            Dim spl() As String
            Dim arr() As String = {"3"}
            Dim arrkey() As String
            If Request.Form("hidpass") <> "" Then
                arrkey = Request.Form("hidpass").Split("&")
            Else


            End If
            Dim ax() As String = {""}
            i = 0
            Dim pamt As String = ""
            If Request.QueryString("paiddate") <> "" And Request.QueryString("mthd") <> "" Then
                paiddate = Request.QueryString("paiddate")
                paidmthd = Request.QueryString("mthd")
                paidstate = "y"


            End If

            If Request.Form("paiddate") <> "" And Request.Form("mthd") <> "" Then
                paiddate = Request.Form("paiddate")
                paidmthd = Request.Form("mthd")
                paidstate = "y"
                'Response.Write(Request.Form("paiddate"))
                ' Response.Write(Request.Form("mthd"))

            End If
            If IsArray(arrkey) = True Then

                For k As Integer = 0 To arrkey.Length - 1
                    val = arrkey(k).Split("=")
                    check = ""

                    ref = ""
                    emptid = ""
                    ' Response.Write(val(0))

                    If val.Length > 1 Then
                        spl = val(0).Split("_")
                        ' Response.Write(spl(0))
                        ' Response.Write(Request.Form(spl(0)))
                        emptid = spl(0)
                        'Response.Write(emptid)
                        If spl(1) = 3 Then
                            pamt = val(1)


                            ref = Now.Ticks.ToString & emptid.ToString
                            check = fm.getinfo2("select id from pardimpay where emptid=" & emptid & " and (('" & d1 & "'>=from_date and '" & d1 & "' <=to_date)" & " or " & _
                            "('" & d2 & "'>=from_date and '" & d2 & "' <= to_date))  and payreason='" & Request.Form("payreason") & "'", Session("con"))
                            msg &= (check & "===>" & Request.Form("payreason"))
                            If check = "None" Or pb = "1" Then
                                If fm.searcharray(ax, emptid.ToString) = False Then

                                    ReDim Preserve ax(UBound(ax) + 1)
                                    ax(UBound(ax) - 1) = emptid.ToString


                                    If String.IsNullOrEmpty(Request.Form("adv")) = True Then
                                        adv = 0.0
                                    Else
                                        adv = Request.Form("adv")
                                    End If
                                    If pb = "1" Then
                                        sql(UBound(sql)) = "insert into pardimpay(emptid,pardim,reason,no_days,from_date,to_date,ref,paid_date,mthd,paid_state,adv,payreason,who_reg,date_reg,backpay) values("
                                        sql(UBound(sql)) &= emptid.ToString & "," & pamt & ",'" & Request.Form("reason") & "',"
                                        sql(UBound(sql)) &= df.ToString & ",'" & d1 & "','" & d2 & "','" & _
                                        ref & "','" & paiddate & "','" & paidmthd & "','" & paidstate & "','" & adv & "','" & _
                                       Request.Form("payreason") & "','" & Request.Form("who_reg") & "','" & Request.Form("date_reg") & "',1)"


                                    Else
                                        sql(UBound(sql)) = "insert into pardimpay(emptid,pardim,reason,no_days,from_date,to_date,ref,paid_date,mthd,paid_state,adv,payreason,who_reg,date_reg) values("
                                        sql(UBound(sql)) &= emptid.ToString & "," & pamt & ",'" & Request.Form("reason") & "',"
                                        sql(UBound(sql)) &= df.ToString & ",'" & d1 & "','" & d2 & "','" & _
                                        ref & "','" & paiddate & "','" & paidmthd & "','" & paidstate & "','" & adv & "','" & _
                                         Request.Form("payreason") & "','" & Request.Form("who_reg") & "','" & Request.Form("date_reg") & "')"


                                    End If
                                    ReDim Preserve sql(UBound(sql) + 1)
                                End If
                            Else
                                msg &= "<br>Unsave Data, due to already existed data on the given date! Emp. Id.:" & fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con"))

                            End If
                        End If

                    End If

                    ' Response.Write(p & "==" & Request.QueryString(p) & "<br>")
                Next
            End If
            Dim mm() As String
            Dim n() As String
            Dim emp_id As String
            Dim takn As String
            Dim sqlp() As String = {""}

            For Each k As String In Request.Form
                'Response.Write(k)
                mm = k.Split("-")
                If mm.Length > 1 Then

                    ' Response.Write(mm(1))

                    takn = mm(1).Substring(mm(1).Length - 1, 1)
                    ' Response.Write(takn)
                    If mm(1).Substring(0, mm(1).Length - 1) = "name" Then


                        Dim trmval As String
                        trmval = Request.Form(mm(1)).Trim

                        n = trmval.Split(" ")
                        'Response.Write(n.Length)

                        If n.Length = 3 Then
                            emp_id = fm.getinfo2("select emp_id from emp_static_info where first_name='" & n(0) & "' and middle_name='" & n(1) & "' and last_name='" & n(2) & "'", Session("con"))

                            emptid = (fm.getinfo2("select id from emprec where emp_id='" & emp_id & "' and '" & d1 & "'<= isnull(end_date,'" & d2 & "') order by id desc", Session("con")))

                            ref = Now.Day.ToString & Now.Year.ToString & Now.Month.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & emptid.ToString
                            check = fm.getinfo2("select id from pardimpay where emptid=" & emptid & " and (('" & d1 & "'>=from_date and '" & d1 & "' <=to_date)" & " or " & _
                       "('" & d2 & "'>=from_date and '" & d2 & "' <= to_date)) and payreason='" & Request.Form("payreason") & "'", Session("con"))
                            msg &= (check & "===>" & Request.Form("payreason"))
                            If check = "None" Or pb = "1" Then
                                If IsNumeric(emptid) Then
                                    pamt = Request.Form("aamt" & takn.ToString)

                                    If pb = "1" Then
                                        sql(UBound(sql)) = "insert into pardimpay(emptid,pardim,reason,no_days,from_date,to_date,ref,paid_date,mthd,paid_state,adv,payreason,who_reg,date_reg,backpay) values("
                                        sql(UBound(sql)) &= emptid.ToString & "," & pamt & ",'" & Request.Form("reason") & "',"
                                        sql(UBound(sql)) &= df.ToString & ",'" & d1 & "','" & d2 & "','" & _
                                        ref & "','" & paiddate & "','" & paidmthd & "','" & paidstate & "','" & adv & "','" & _
                                       Request.Form("payreason") & "','" & Request.Form("who_reg") & "','" & Request.Form("date_reg") & "',1)"


                                    Else
                                        sql(UBound(sql)) = "insert into pardimpay(emptid,pardim,reason,no_days,from_date,to_date,ref,paid_date,mthd,paid_state,adv,payreason,who_reg,date_reg) values("
                                        sql(UBound(sql)) &= emptid.ToString & "," & pamt & ",'" & Request.Form("reason") & "',"
                                        sql(UBound(sql)) &= df.ToString & ",'" & d1 & "','" & d2 & "','" & _
                                        ref & "','" & paiddate & "','" & paidmthd & "','" & paidstate & "','" & adv & "','" & _
                                        Request.Form("payreason") & "','" & Request.Form("who_reg") & "','" & Request.Form("date_reg") & "')"


                                    End If
                                    ReDim Preserve sql(UBound(sql) + 1)

                                End If
                            End If
                        End If
                    End If

                End If
            Next
            Dim flg As String = ""
            Dim txtmove As String = ""

            If sqlp.Length > 0 Then

                txtmove = "BEGIN TRANSACTION" & Chr(13)
                'ds.excutes("begin transaction", Session("con"))
                'Response.Write(sql.Length.ToString & "===" & UBound(sql).ToString)
                For i = 0 To UBound(sql)
                    flg = ""
                    'flg = ds.excutes(sql(i), Session("con"))

                    If sql(i) <> "" Then
                        txtmove &= sql(i) & Chr(13)
                        'Response.Write("<br>" & sql(i))
                    Else
                        ' Response.Write("Unsave query:" & i.ToString & "<br>")
                    End If
                    ' Response.Write(flg)
                    ' Response.Write(sql(i) & "<br>")

                Next
                ' Response.Write(flg)

                flg = ds.excutes(txtmove, Session("con"), Session("path"))

                msg &= txtmove
                If CInt(flg) >= 0 Then
                    flg = ds.excutes("Commit", Session("con"), Session("path"))
                    If flg <> "-1" Then
                        ds.excutes("RollBack", Session("con"), Session("path"))
                    Else
                        msg = "Data is Saved"
                    End If
                Else

                    ds.excutes("RollBack", Session("con"), Session("path"))

                    msg &= "<br> Data is not Saved, may it existed"
                End If
                msg &= "<textarea cols='100' rows='14'>" & txtmove & "</textarea>"
            End If

        End If

        If Request.QueryString("save") = "on" And Request.QueryString("savebd") = "fm2" Then
            Dim ids() As String
            'ids = fm.getids(Request, "hidpass")
            Dim df As Integer
            Dim d1, d2 As Date
            Dim pb As String
            pb = Request.Form("pb")
            If pb <> "1" Then
                pb = "0"
            End If
            d1 = Request.QueryString("from_date")
            d2 = Request.QueryString("to_date")
            df = d2.Subtract(d1).Days + 1
            Dim val() As String
            Dim spl() As String
            Dim arr() As String = {"3"}
            Dim arrkey() As String
            If Request.Form("hidpass") <> "" Then
                arrkey = Request.Form("hidpass").Split("&")
            Else
                arrkey = Request.QueryString("hidpass").Split("&")
            End If

            i = 0
            For k As Integer = 0 To arrkey.Length - 1

                val = arrkey(k).Split("=")

                check = ""
                paiddate = ""
                paidmthd = ""
                paidstate = "n"
                ref = ""
                emptid = ""
                Dim pamt As String = ""

                If val.Length > 1 Then
                    spl = val(0).Split("_")
                    emptid = spl(0).Split("-")(0)

                    If spl(1) = 3 Then
                        pamt = val(1)
                        'Response.Write("<br>" & emptid & "==" & val(1))
                        If Request.QueryString("paiddate") <> "" And Request.QueryString("mthd") <> "" Then
                            paiddate = Request.QueryString("paiddate")
                            paidmthd = Request.QueryString("mthd")
                            paidstate = "y"
                            ref = Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & Now.Millisecond.ToString & emptid.ToString
                        End If
                        check = fm.getinfo2("select id from pardimpay where emptid=" & emptid & _
                        " and (('" & d1 & "'>=from_date and '" & d1 & "' <=to_date)" & _
                        " or " & _
                        "('" & d2 & "'>=from_date and '" & d2 & "' <= to_date))  and payreason='" & Request.Form("payreason") & "'", Session("con"))
                        ' Response.Write("select id from pardimpay where emptid=" & emptid & _
                        '" and (('" & d1 & "'>=from_date and '" & d1 & "' <=to_date)" & _
                        '" or " & _
                        '"('" & d2 & "'>=from_date and '" & d2 & "' <= to_date))")
                        If check = "None" Or pb = "1" Then
                            ReDim Preserve sql(i + 1)
                            If String.IsNullOrEmpty(Request.Form("adv")) = True Then
                                adv = 0.0
                            Else
                                adv = Request.Form("adv")
                            End If
                            If pb = "1" Then
                                sql(i) = "insert into pardimpay(emptid,pardim,reason,no_days,from_date,to_date,ref,adv,paid_date,mthd,paid_state,who_reg,date_reg,backpay) values("
                                sql(i) &= emptid.ToString & "," & pamt & ",'" & Request.Form("reason") & "',"
                                sql(i) &= df.ToString & ",'" & d1 & "','" & d2 & "','" & _
                                ref & "','" & adv & "','" & paiddate & "','" & paidmthd & "','" & paidstate & "','" & _
                                Request.Form("who_reg") & "','" & Request.Form("date_reg") & "',1)"

                                i += 1
                            Else
                                sql(i) = "insert into pardimpay(emptid,pardim,reason,no_days,from_date,to_date,ref,adv,paid_date,mthd,paid_state,who_reg,date_reg) values("
                                sql(i) &= emptid.ToString & "," & pamt & ",'" & Request.Form("reason") & "',"
                                sql(i) &= df.ToString & ",'" & d1 & "','" & d2 & "','" & _
                                ref & "','" & adv & "','" & paiddate & "','" & paidmthd & "','" & paidstate & "','" & _
                                Request.Form("who_reg") & "','" & Request.Form("date_reg") & "')"

                                i += 1
                            End If

                        Else
                            msg &= "<br>Unsave Data, due to already existed data on the given date! Emp. Id.:" & fm.getinfo2("select emp_id from emprec where id=" & emptid.ToString, Session("con")) & " computer id:" & emptid

                        End If
                    End If

                End If

                ' Response.Write(p & "==" & Request.QueryString(p) & "<br>")
            Next
            Dim flg As String = ""
            Dim dsave As String = ""
            If sql.Length > 0 Then
                dsave = "BEGIN TRANSACTION" & Chr(13)
                For i = 0 To UBound(sql) - 1

                    If String.IsNullOrEmpty(sql(i)) = False Then
                        dsave &= sql(i) & Chr(13)
                    End If
                    'Response.Write(sql(i) & "<br>")
                    ' Response.Write(sql(i) & "<br>")

                Next
                ' Response.Write(flg)
                flg = ds.excutes(dsave, Session("con"), Session("path"))
                If IsNumeric(flg) Then
                    If CInt(flg) > 0 Then
                        ds.excutes("Commit", Session("con"), Session("path"))
                        msg &= "Data is Saved"
                    Else
                        ds.excutes("RollBack", Session("con"), Session("path"))
                        msg &= "Data is not Saved"
                    End If
                Else
                    ds.excutes("RollBack", Session("con"), Session("path"))
                    msg &= "Data is not Saved"
                End If
            End If

        End If
        fm = Nothing
        ds = Nothing
        If msg <> "" Then
            'Response.Write(msg)
            Response.Write(fl.msgboxt("msgsave", "Data Status", msg))

            Response.Write("<script>showobj('msgsave');</script>")
            'Response.Write(outpx())
        End If
        Return Nothing
    End Function
    Function mksql()
        Dim pdate1, pdate2 As Date
        Dim spl() As String
        Dim projid As String
        Dim fm As New formMaker
        pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
        pdate2 = Request.Form("month") & "/" & Date.DaysInMonth(Request.Form("year"), Request.Form("month")).ToString & "/" & Request.Form("year")
        Dim accstr As String = ""

        spl = Request.Form("projname").Split("|")
        If spl.Length <= 1 Then
            ReDim spl(2)
            spl(0) = Request.Form("projname")
            spl(1) = ""
        End If
        projid = spl(1)
        Dim rtnvalue As String
        ' Response.Write(projid & pdate2.ToShortDateString)
        rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
        Dim sql As String = ""
        sql = "SELECT DISTINCT " & _
                              " pardimpay.id, pardimpay.emptid, emp_static_info.first_name, emp_static_info.middle_name, emp_static_info.last_name, pardimpay.pardim, pardimpay.reason," & _
                               "pardimpay.no_days, pardimpay.from_date, pardimpay.to_date, pardimpay.adv, pardimpay.ref, pardimpay.paid_date,pardimpay.payreason, pardimpay.mthd, pardimpay.paid_state" & _
              " " & _
        "FROM emp_static_info INNER JOIN " & _
                               "emprec ON emp_static_info.emp_id = emprec.emp_id INNER JOIN " & _
                               "pardimpay ON emprec.id = pardimpay.emptid " & _
                               "where pardimpay.emptid in(" & rtnvalue & ") and from_date>='" & pdate1 & "' and to_date<='" & pdate2 & "'" & _
        " ORDER BY pardimpay.id desc"
        'Response.Write(sql)
        Return sql
        If IsPostBack = True Then
            Response.Write("<script>$('#viewg').css({ display:'inline'});</script>")

        End If
    End Function
    Public Function outpx()
        'Response.Write(Request.Form("projname"))
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim spl(), outp As String
        Dim projid As String = ""
        Dim rs As DataTableReader
        Dim copy As String = ""
        Dim i As Integer
        Dim fdate, pdate1, pdate2 As Date
        pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
        pdate2 = Request.Form("month") & "/" & Date.DaysInMonth(Request.Form("year"), Request.Form("month")).ToString & "/" & Request.Form("year")
        Dim accstr As String = ""

        spl = Request.Form("projname").Split("|")
        If spl.Length <= 1 Then
            ReDim spl(2)
            spl(0) = Request.Form("projname")
            spl(1) = ""
        End If
        outp = ""
        projid = spl(1)
        Dim cell(5) As Object
        Dim k As Integer = 0
        If spl.Length > 1 Then
            outp = "<table id='tb1' cellspacing='0' cellpadding='3' style='width:600px;font-size:9pt;' border='0'>" & Chr(13)
            outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
            outp &= "<td style='text-align:center;font-weight:bold;' colspan='16' >" & Session("company_name") & Chr(13) & _
            "<br> Project Name:"

            outp &= spl(0).ToString

            outp &= "<br>Perdiem Entery Form"

            outp &= "</tr>" & Chr(13)
            outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
            outp &= "<td >No.</td><td style='width:90px;' >Emp. ID</td>"
            outp &= "<td style='width:250px;'>Full Name</td>"
            outp &= "<td>Perdiem Amount</td>" & Chr(13)
            outp &= "<td  id='chkall' style='cursor:pointer' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)
            outp &= "</tr>" & Chr(13)
            rs = ds.dtmake("mmkj", "select * from emp_pardime as ep inner join emp_static_info as empsf on empsf.emp_id=ep.emp_id  where emptid in(select emptid from emp_job_assign where project_id='" & projid & "' and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')) and '" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "') order by empsf.first_name", Session("con"))
            If rs.HasRows Then




                k = 1
                Dim emptid As String = ""
                While rs.Read
                    emptid = rs.Item("emptid")
                    fdate = CDate(fm.getinfo2("select from_date from emp_pardime where emptid=" & emptid, Session("con")))
                    If fdate.Month = Now.Month And fdate.Year = Now.Year And fdate.Day > 1 Then
                        accstr = "red"
                    Else
                        accstr = ""
                    End If
                    cell(0) = k
                    cell(1) = rs.Item("emp_id")
                    cell(2) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                    cell(3) = fm.getinfo2("select pardime from emp_pardime where emptid=" & emptid & " and '" & pdate1 & "' between from_date and isnull(to_date,'" & pdate2 & "')", Session("con"))
                    cell(4) = " <input type='checkbox' name='paid-" & emptid.ToString & "' id='paid-" & emptid.ToString & "' class='chkbox' checked='checked'>"
                    '  rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE amt > 0 AND (paid_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is null", Session("con"))
                    If cell(3) <> "None" Then
                        outp &= "<tr bgcolor='" & accstr & "'>"

                        For i = 0 To 4

                            outp &= "<td id='" & emptid & "_" & i & "'>" & cell(i).ToString & "</td>"

                        Next

                        outp &= "</tr>" & Chr(13)
                        k += 1
                    End If

                End While
                outp &= "<tr><td id='name1_" & k.ToString & "'>" & k.ToString & "</td><td>&nbsp;</td><td><input type='text' id='name1' name='name1'  onkeyup=" & Chr(34) & "javascript:startwith('name1',namelist);" & Chr(34) & "></td><td><input type='text' id='aamt1' name='aamt1' size='5'></td><td><input type='checkbox' name='paid-name1' id='paid-name1' value='name1'></td></tr>"
                outp &= "<tr><td id='name2_" & k + 1.ToString & "'>" & k + 1.ToString & "</td><td>&nbsp;</td><td><input type='text' id='name2' name='name2'   onkeyup=" & Chr(34) & "javascript:startwith('name2',namelist);" & Chr(34) & "></td><td><input type='text' id='aamt2' name='aamt2' size='5'></td><td><input type='checkbox' name='paid-name2' id='paid-name2' value='name2'></td></tr>"
                outp &= "<tr><td id='name3_" & k + 2.ToString & "'>" & k + 2.ToString & "</td><td>&nbsp;</td><td><input type='text' id='name3' name='name3'  onkeyup=" & Chr(34) & "javascript:startwith('name3',namelist);" & Chr(34) & "></td><td><input type='text' id='aamt3' name='aamt3' size='5'></td><td><input type='checkbox' name='paid-name3' id='paid-name3' value='name3'></td></tr>"
                outp &= "</table>"
                outp &= "<table><tr><td>Back-Payment</td><td><input type='checkbox' name='pb' id='pb' value='1'></td></tr>" & _
                    "<tr>"
                outp &= "<td>Payment type</td><td>:</td><td><input type='text' name='payreason' id='payreason' value='Per-dime' onkeyup=" & Chr(34) & "javascript:startwith('payreason',ptype);" & Chr(34) & "></td></tr>" & _
                 "<tr>"
                outp &= "<td>Reason:</td><td>:</td><td><input type='text' name='reason' id='reason' /></td></tr><tr><td>Date Start</td><td>:</td><td><input type='text' id='from_date' name='from_date'></td>"


                outp &= "<td>Date End</td><td>:</td><td><input type='text' id='to_date' name='to_date'></td></tr>"
                outp &= "<script language='javascript' type='text/javascript'> " & Chr(13) & _
     "$(function() {$( '#from_date').datepicker({changeMonth: true,changeYear: true,onClose: function( selectedDate ) {" & _
      " $( '#to_date' ).datepicker( 'option', 'minDate', selectedDate );" & _
    " }});" & Chr(13) & _
     " $( '#from_date' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>"
                outp &= "<script language='javascript' type='text/javascript'>" & Chr(13) & _
                "$(function() {$( '#to_date').datepicker({changeMonth: true,changeYear: true}); " & Chr(13) & _
     "$( '#to_date' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>"
                outp &= "</tr><tr><td>Advance</td><td>:</td><td><input type='text' id='adv' name='adv' style='text-align:right'></td></tr>"
                outp &= "<tr>" & Chr(13) & _
                                       "<td>Payment Method</td><td>:</td><td><select name='mthd' id='mthd'>" & Chr(13) & _
    "<option value='' style='color:gray' selected>Select</option>" & Chr(13) & _
    "<option value='Cheque'>Cheque</option>" & Chr(13) & _
                                       "<option value='Bank'>Bank</option>" & Chr(13) & _
                                        "<option value='Cash'>Cash</option></select>" & Chr(13) & _
                                        "</td></tr><tr>" & Chr(13) & _
                                        "<td>Paid Date</td><td>:</td><td><input type='text' id='paiddate' name='paiddate' value=''></td>" & Chr(13) & _
                                        "<script language='javascript' type='text/javascript'> $(function() {$( '#paiddate').datepicker({changeMonth: true,changeYear: true	}); $( '#paiddate' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & Chr(13) & _
                                        "</tr>"
                outp &= "<tr><td colspan=4><input type='button' id='post' onclick=" & Chr(34) & "javascript:findid('fm1');" & Chr(34) & " name='post' value='Post' /></td></tr>"
                outp &= "</table>"
                outp &= "  <input type='hidden' id='who_reg' name='who_reg' value='" & Session("username") & "' />"
                outp &= "<input type='hidden' id='date_reg' name='date_reg'  value='"
                Dim lucur(3) As String
                lucur(2) = Today.Year.ToString
                lucur(1) = Today.Month.ToString
                lucur(0) = Today.Day.ToString
                Dim sdate As String = lucur(1) & "/" & lucur(0) & "/" & lucur(2) & " " & Now.ToLongTimeString
                outp &= sdate.ToString & "'/>"
            Else
                outp &= "<tr><td id='name1_" & k.ToString & "'>" & k.ToString & "</td><td>&nbsp;</td><td><input type='text' id='name1' name='name1'  onkeyup=" & Chr(34) & "javascript:startwith('name1',namelist);" & Chr(34) & "></td><td><input type='text' id='aamt1' name='aamt1' size='5'></td><td><input type='checkbox' name='paid-name1' id='paid-name1' value='name1'></td></tr>"
                outp &= "<tr><td id='name2_" & k + 1.ToString & "'>" & k + 1.ToString & "</td><td>&nbsp;</td><td><input type='text' id='name2' name='name2'   onkeyup=" & Chr(34) & "javascript:startwith('name2',namelist);" & Chr(34) & "></td><td><input type='text' id='aamt2' name='aamt2' size='5'></td><td><input type='checkbox' name='paid-name2' id='paid-name2' value='name2'></td></tr>"
                outp &= "<tr><td id='name3_" & k + 2.ToString & "'>" & k + 2.ToString & "</td><td>&nbsp;</td><td><input type='text' id='name3' name='name3'  onkeyup=" & Chr(34) & "javascript:startwith('name3',namelist);" & Chr(34) & "></td><td><input type='text' id='aamt3' name='aamt3' size='5'></td><td><input type='checkbox' name='paid-name3' id='paid-name3' value='name3'></td></tr>"
                outp &= "</table>"
                outp &= "<table><tr><td>Back-Payment</td><td><input type='checkbox' name='pb' id='pb' value='1'></td></tr>" & _
                    "<tr>"
                outp &= "<td>Payment type</td><td><input type='text' name='payreason' id='payreason' value='Per-diem' onkeyup=" & Chr(34) & "javascript:startwith('payreason',ptype);" & Chr(34) & "></td></tr>" & _
                   "<tr>"
                outp &= "<td>Reason:</td><td>:</td><td><input type='text' name='reason' id='reason' /></td></tr><tr><td>Date Start</td><td>:</td><td><input type='text' id='from_date' name='from_date'></td>"


                outp &= "<td>Date End</td><td>:</td><td><input type='text' id='to_date' name='to_date'></td></tr>"
                outp &= "<script language='javascript' type='text/javascript'> " & Chr(13) & _
     "$(function() {$( '#from_date').datepicker({changeMonth: true,changeYear: true,onClose: function( selectedDate ) {" & _
      " $( '#to_date' ).datepicker( 'option', 'minDate', selectedDate );" & _
    " }});" & Chr(13) & _
     " $( '#from_date' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>"
                outp &= "<script language='javascript' type='text/javascript'>" & Chr(13) & _
                "$(function() {$( '#to_date').datepicker({changeMonth: true,changeYear: true}); " & Chr(13) & _
     "$( '#to_date' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>"
                outp &= "</tr><tr><td>Advance</td><td>:</td><td><input type='text' id='adv' name='adv' style='text-align:right'></td></tr>"
                outp &= "<tr>" & Chr(13) & _
                                       "<td>Payment Method</td><td>:</td><td><select name='mthd' id='mthd'>" & Chr(13) & _
    "<option value='' style='color:gray' selected>Select</option>" & Chr(13) & _
    "<option value='Cheque'>Cheque</option>" & Chr(13) & _
                                       "<option value='Bank'>Bank</option>" & Chr(13) & _
                                        "<option value='Cash'>Cash</option></select>" & Chr(13) & _
                                        "</td></tr><tr>" & Chr(13) & _
                                        "<td>Paid Date</td><td>:</td><td><input type='text' id='paiddate' name='paiddate' value=''></td>" & Chr(13) & _
                                        "<script language='javascript' type='text/javascript'> $(function() {$( '#paiddate').datepicker({changeMonth: true,changeYear: true	}); $( '#paiddate' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & Chr(13) & _
                                        "</tr>"
                outp &= "<tr><td colspan=4><input type='button' id='post' onclick=" & Chr(34) & "javascript:findid('fm1');" & Chr(34) & " name='post' value='Post' /></td></tr>"
                outp &= "</table>"
                outp &= "  <input type='hidden' id='who_reg' name='who_reg' value='" & Session("username") & "' />"
                outp &= "<input type='hidden' id='date_reg' name='date_reg'  value='"
                Dim lucur(3) As String
                lucur(2) = Today.Year.ToString
                lucur(1) = Today.Month.ToString
                lucur(0) = Today.Day.ToString
                Dim sdate As String = lucur(1) & "/" & lucur(0) & "/" & lucur(2) & " " & Now.ToLongTimeString
                outp &= sdate.ToString & "'/>"
            End If
            rs.Close()





        End If

        ' Response.Write("sadfasdfasdfasdfasdf")
        rs = Nothing
        ds = Nothing
        fm = Nothing
        Return outp
    End Function
    Public Function outpy()
        'Response.Write(Request.Form("projname"))
        Dim ds As New dbclass
        Dim fm As New formMaker
        Dim spl(), outp As String
        Dim projid As String = ""
        Dim rs As DataTableReader
        Dim copy As String = ""
        Dim i As Integer
        Dim fdate, pdate1, pdate2 As Date
        pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
        pdate2 = Request.Form("month") & "/" & Date.DaysInMonth(Request.Form("year"), Request.Form("month")).ToString & "/" & Request.Form("year")

        Dim accstr As String = ""

        spl = Request.Form("projname").Split("|")
        If spl.Length <= 1 Then
            ReDim spl(2)
            spl(0) = Request.Form("projname")
            spl(1) = ""
        End If
        outp = ""
        projid = spl(1)
        Dim cell(5) As Object
        Dim k As Integer = 0
        If spl.Length > 1 Then

            rs = ds.dtmake("mmkj", "select * from emp_pardime  where emptid in(select emptid from emp_job_assign where project_id='" & projid & "' and date_end is null) and from_date between '" & pdate1.ToShortDateString & "' and '" & pdate2.ToShortDateString & "' and to_date is null", Session("con"))
            ' rs = ds.dtmake("mkprj", "select * from emprec where id in(select emptid from emp_job_assign where project_id='" & projid & "' and date_end is null) and end_date is null", Session("con"))
            If rs.HasRows Then

                outp = "<table id='tb1' cellspacing='0' cellpadding='3' style='width:600px;font-size:9pt;' border='0'>" & Chr(13)
                outp &= "<tr style='text-align:center;font-weight:bold;font-size:16pt' >" & Chr(13)
                outp &= "<td style='text-align:center;font-weight:bold;' colspan='16' >" & Session("company_name") & Chr(13) & _
                "<br> Project Name:"

                outp &= spl(0).ToString

                outp &= "<br>Perdiem Entery Form"

                outp &= "</tr>" & Chr(13)

                outp &= "<tr style='text-align:center;font-weight:bold;' >" & Chr(13)
                outp &= "<td >No.</td><td style='width:90px;' >Emp. ID</td>"
                outp &= "<td style='width:250px;'>Full Name</td>"
                outp &= "<td>Perdiem Amount</td>" & Chr(13)
                outp &= "<td  id='chkall' style='cursor:pointer' onclick='javascript:checkall();'>Clear all</td>" & Chr(13)
                outp &= "</tr>" & Chr(13)
                k = 1
                Dim rs2 As DataTableReader

                Dim emptid As String = ""
                While rs.Read
                    emptid = rs.Item("emptid")
                    rs2 = ds.dtmake("mkt", "select * from emp_pardime where emptid=" & emptid & " order by id desc", Session("con"))

                    Dim count As Integer = 0 ' Maximumu one increament per month
                    If rs2.HasRows = True Then

                        While rs2.Read
                            cell(0) = k
                            count += 1

                            cell(1) = rs.Item("emp_id")
                            cell(2) = fm.getfullname(rs.Item("emp_id"), Session("con"))
                            cell(3) = rs2.Item("pardime")
                            cell(4) = " <input type='checkbox' value='" & cell(3).ToString & "' name='paid-" & emptid.ToString & "-" & k.ToString & "' id='paid-" & emptid.ToString & "-" & k.ToString & "' class='chkbox' checked='checked'>"
                            '  rs2 = dbs.dtmake("dbpen", "SELECT distinct reason  FROM emp_inc WHERE amt > 0 AND (paid_date between '" & pdate3 & "' and '" & pdate4 & "') and paidref is null", Session("con"))
                            If cell(3).ToString <> "None" Then
                                outp &= "<tr bgcolor='" & accstr & "'>"

                                For i = 0 To 4

                                    outp &= "<td id='" & emptid & "-" & k.ToString & "_" & i & "'>" & cell(i).ToString & "</td>"

                                Next

                                outp &= "</tr>" & Chr(13)
                                k += 1
                            End If
                            If count = 2 Then
                                Exit While
                            End If
                        End While
                        k += 1
                    End If
                    rs2.Close()


                End While

                outp &= "</table>"
                outp &= "<table><tr><td>Back-Payment</td><td><input type='checkbox' name='pb' id='pb' value='1'></td></tr>" & _
                    "<tr>"
                outp &= "<td>Reason:</td><td>:</td><td><input type='text' name='reason' id='reason' /></td></tr><tr><td>Date Start</td><td>:</td><td><input type='text' id='from_date' name='from_date'></td>"
                outp &= "<script language='javascript' type='text/javascript'> " & Chr(13) & _
      "$(function() {$( '#from_date').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'	}); " & Chr(13) & _
      "$( '#from_date' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>"

                outp &= "<td>Date End</td><td>:</td><td><input type='text' id='to_date' name='to_date'></td></tr>"
                outp &= "<script language='javascript' type='text/javascript'> " & Chr(13) & _
     "$(function() {$( '#to_date').datepicker({changeMonth: true,changeYear: true,maxDate:'0d'}); " & Chr(13) & _
" $( '#to_date' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>"
                outp &= "</tr><tr><td>Advance</td><td>:</td><td><input type='text' id='adv' name='adv' style='text-align:right'></td></tr>"
                outp &= "<tr>" & Chr(13) & _
                                       "<td>Payment Method</td><td>:</td><td><select name='mthd' id='mthd'>" & Chr(13) & _
    "<option value='' style='color:gray' selected>Select</option>" & Chr(13) & Chr(13) & _
    "<option value='Cheque'>Cheque</option>" & Chr(13) & Chr(13) & _
                                       "<option value='Bank'>Bank</option>" & Chr(13) & Chr(13) & _
                                        "<option value='Cash'>Cash</option></select>" & Chr(13) & _
                                        "</td></tr><tr>" & Chr(13) & Chr(13) & _
                                        "<td>Paid Date</td><td>:</td><td><input type='text' id='paiddate' name='paiddate' value=''></td>" & Chr(13) & _
                                        "<script language='javascript' type='text/javascript'> " & Chr(13) & _
                                        "$(function() {$( '#paiddate').datepicker({changeMonth: true,changeYear: true	}); " & Chr(13) & _
"$( '#paiddate' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & Chr(13) & Chr(13) & _
                                        "</tr>"
                outp &= "<tr><td colspan=4><input type='button' id='post' onclick=" & Chr(34) & "javascript:findid('fm2');" & Chr(34) & " name='post' value='Post' /></td></tr>"
                outp &= "</table>"
                outp &= "  <input type='hidden' id='who_reg' name='who_reg' value='" & Session("username") & "' />"
                outp &= "<input type='hidden' id='date_reg' name='date_reg'  value='"
                Dim lucur(3) As String
                lucur(2) = Today.Year.ToString
                lucur(1) = Today.Month.ToString
                lucur(0) = Today.Day.ToString
                Dim sdate As String = lucur(1) & "/" & lucur(0) & "/" & lucur(2)
                outp &= sdate.ToString & "'/>"
            End If
            rs.Close()
        Else
            Response.Write("pppnodata")
        End If


        rs = Nothing
        ds = Nothing
        fm = Nothing
        Return outp
    End Function
    Function gridx()
        Dim outp As String = ""
        outp = "<form id='form1' runat='server'>" & _
   "<div style='font-size:9pt;'>" & _
       " <asp:GridView ID='GridView1' runat='server' DataSourceID='SqlDataSource1' " & _
           " CellPadding='4' ForeColor='#333333' GridLines='None'> " & _
            "<AlternatingRowStyle BackColor='White' />" & _
            "<Columns>" & _
                "<asp:CommandField ShowDeleteButton='True' />" & _
            "</Columns> " & _
            "<EditRowStyle BackColor='#7C6F57' />" & _
            "<FooterStyle BackColor='#1C5E55' Font-Bold='True' ForeColor='White' />" & _
            "<HeaderStyle BackColor='#1C5E55' Font-Bold='True' ForeColor='White' />" & _
            "<PagerStyle BackColor='#666666' ForeColor='White' HorizontalAlign='Center' />" & _
            "<RowStyle BackColor='#E3EAEB' />" & _
            "<SelectedRowStyle BackColor='#C5BBAF' Font-Bold='True' ForeColor='#333333' />" & _
        "</asp:GridView>" & _
        "<asp:SqlDataSource ID='SqlDataSource1' runat='server' ></asp:SqlDataSource>" & _
   " </div> " & _
    "</form>"
        Response.Write(outp)
    End Function
    Function sssssql()
        'Response.Write(Request.QueryString("projname"))
        sssql = ""
        If Request.Form("projname") <> "" Then
            Session("projj") = Request.Form("projname")
            Session("month") = Request.Form("month")
            Session("year") = Request.Form("year")
            sssql = mksql()
        Else
            sssql = "SELECT DISTINCT " & _
                       " pardimpay.id, pardimpay.emptid, emp_static_info.first_name, emp_static_info.middle_name, emp_static_info.last_name, pardimpay.pardim, pardimpay.reason," & _
                        "pardimpay.no_days, pardimpay.from_date, pardimpay.to_date, pardimpay.adv, pardimpay.ref, pardimpay.paid_date,pardimpay.payreason, pardimpay.mthd, pardimpay.paid_state " & _
"FROM emp_static_info INNER JOIN " & _
                        "emprec ON emp_static_info.emp_id = emprec.emp_id INNER JOIN " & _
                        "pardimpay ON emprec.id = pardimpay.emptid " & _
"ORDER BY pardimpay.id desc"
        End If
        Return sssql
    End Function
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("con").Close()
        Session("con").open()
        Dim sds As SqlDataSource
        Dim db As New dbclass
        Dim sql As String = ""
        ' db.dtmake("vwdba", Session("sql"), Session("con"))
        If Request.QueryString("updated") = "on" Then

            sql = db.makest("pardimpay", Request.Form, Session("con"), "")
            ' Response.Write(sql)
            If db.save(sql, Session("con"), Session("path")) = 1 Then

                Response.Redirect("pardimreg.aspx?dtable=" & Request.QueryString("dtable"))
            Else
                '   Response.Write("Sorry Data is not enter duto " & sql)
            End If
        End If
        Dim nstr() As String = {"", ""}
        sds = SqlDataSource1
        sds.ProviderName = "System.Data.SqlClient"
        sds.ConnectionString = Session("constr")
        'MsgBox(Session("constr"))


        If IsPostBack = True Then
            sssql = Session("sql")
        Else
            If sssssql() = "" Then
                sssql = Session("sql")
            Else
                Session("sql") = sssssql()
                sssql = Session("sql")
            End If
        End If
        'Response.Write(sssql)
        sds.SelectCommand = sssql 'select * from " & Request.QueryString("dtable")
        nstr = db.makeupdatea("pardimpay", sssql, Session("con"), "id,emptid,pardim,reason,no_days,from_date,to_date,adv,ref,paid_date,mthd,paid_state")
        ' Response.Write(nstr(0))
        Dim nk() As String = {""}
        nk(0) = nstr(1) 'on it
        GridView1.DataKeyNames = nk 'on it
        GridView1.BackColor = Drawing.Color.AliceBlue

        GridView1.AutoGenerateEditButton = True
        GridView1.AllowPaging = True
        GridView1.PageSize = 10
        GridView1.AllowSorting = True



        If IsPostBack = True Then
            For Each k As String In Request.Form
                ' Response.Write("<br>" & k & " ====> " & Request.Form(k))
            Next
            GridView1.AllowPaging = True
            GridView1.PageSize = 10
            If nstr(0) <> "" Then
                Dim st(2) As String
                ' GridView1.EditRowStyle.
                Dim df() As String
                df = db.getdatefields("pardimpay", Session("con"))
                If df.Length > 0 Then
                    For i As Integer = 0 To df.Length - 1
                        Dim param1 As New Parameter(df(i), TypeCode.DateTime, DateTime.Now.ToString())
                        '  SqlDataSource1.UpdateParameters.Add(param1)
                    Next
                End If

                sds.DeleteCommand = "Delete from pardimpay where " & nstr(1) & "= @" & nstr(1)


                ' nstr(0) = " update weather set city=@city,condition=@condition,mxtemp=@mxtemp,mtemp=@mtemp,datetemp=@datetemp,publish=@publish,dateent=@dateent where id= @id"
                ' Response.Write(nstr(0))
                sds.UpdateCommand = nstr(0)
            End If
        End If


    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Session("command") = e.CommandName
        If e.CommandName = "Edit" Then
            GridView1.AllowPaging = True
            GridView1.PageSize = 10
        End If
    End Sub
End Class
