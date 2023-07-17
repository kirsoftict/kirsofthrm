Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm

Partial Class pardimviewn
    Inherits System.Web.UI.Page
    Function outpx(ByVal ref As String)

        Dim fm As New formMaker
        Dim sec As New k_security
        Dim ds As New dbclass
        Dim emptid As String
        Dim arrp() As String
        Dim spl() As String
        Dim d1, d2 As Date
        Dim i As Integer = 0
        Dim pref As String
        Dim rs As DataTableReader
        pref = Request.QueryString("s")
        Dim projname() As String
        projname = sec.dbHexToStr(Request.QueryString("projname")).ToString.Split("|")
        If pref = "" Then

            pref = ref
        End If
        If projname.Length > 0 Then
            ' Response.Write(
        End If
        ' rs = ds.dtmake("view", "select * from pardimpay where ref='" & pref & "'", Session("con"))
        rs = ds.dtmake("view", "SELECT pardimpay.* FROM pardimpay INNER JOIN emprec ON pardimpay.emptid = emprec.id INNER JOIN " & _
                         "emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                        "where ref='" & pref & "' " & _
"ORDER BY emp_static_info.first_name", Session("con"))
       
        Dim pb As String = "0"
        Dim prdd As Date
        Dim wrd As String = ""
        Dim payreason As String = ""
        If rs.HasRows Then
            While rs.Read
                Session("maker") = ""
                ReDim Preserve arrp(i + 1)
                emptid = rs.Item("emptid")
                Session("maker") = rs.Item("who_reg")
                arrp(i) = rs.Item("id")
                d1 = rs.Item("paid_date")
                d2 = "#" & d1.Month & "/1/" & d1.Year & "#"
                prdd = rs.Item("from_date")
                payreason = rs.Item("payreason").ToString
                ' Response.Write(payreason)
                If payreason.ToString = "" Then
                    payreason = "Perdiem"
                End If
                If rs.IsDBNull(12) = False Then
                    pb = rs.Item("backpay").ToString
                End If
                i = i + 1
            End While
        End If

        Dim outp As String = ""

        outp &= " <table class='tb1' cellpadding='2' cellspacing='0' width='640px;'> "
        outp &= "   <tr>" & Chr(13)
        outp &= "  <td colspan='13' class='headerp'><center><strong>" & Session("company_name") & "</strong></center></td>"
        outp &= " </tr>" & Chr(13)
        'line 1
        outp &= "  <tr>"
        outp &= " <td colspan='13'  class='headerp'><center><strong>Addis Ababa</strong></center></td>"
        outp &= " </tr>" & Chr(13)
        'line 2
        outp &= " <tr>"
        outp &= " <td colspan='13'  class='headerp'><center><strong>" & payreason & " Settlement    Form</strong></center></td>"
        outp &= "  </tr>" & Chr(13)
        'line3
        'outp &= " <tr><td colspan='13' class='headerp' style='font-size:9pt;'><center>Payment Ref:" & pref
        If pb = "True" Then
            ' outp &= "<span style='font-style:italic; color:red;'>&nbsp;(Back Payment)</span>"
        End If
        ' outp &= "</center></td></tr>" & Chr(13)
        'line 4
        outp &= "  <tr> "
        outp &= "  <td colspan=" & Chr(34) & "13" & Chr(34) & " style=" & Chr(34) & "height: 23px;text-align:right" & Chr(34) & ">"
        outp &= "    <strong>Date:</strong><span style=" & Chr(34) & "text-decoration: underline" & Chr(34) & ">&nbsp;"
        outp &= MonthName(d1.Month) & " " & d1.Day & ", " & d1.Year & "</span>"
        outp &= "  </td>"
        outp &= "  </tr>" & Chr(13)
        'line 5
        outp &= " <tr>"
        outp &= " <td style=" & Chr(34) & "height: 60px;" & Chr(34) & " colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "  </tr>" & Chr(13)
        'line 6
        outp &= " <tr>"
        outp &= " <td style=" & Chr(34) & "" & Chr(34) & " >"
        outp &= "  <strong>Traveller's Name</strong></td><td>:</td>"
        outp &= " <td style=" & Chr(34) & "width:300px;" & Chr(34) & " colspan='4'>"
        outp &= fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con")) & "</td>"
        outp &= " <td colspan='7' width='200px'>&nbsp;</td></tr>" & Chr(13)
        'linep
        outp &= "  <tr>"
        outp &= "  <td  style=" & Chr(34) & "" & Chr(34) & ">"
        outp &= "  <strong>Position</strong></td><td>:</td>"
        outp &= " <td colspan='2'>"
        Dim posi As String

        posi = fm.getinfo2("select position from emp_job_assign where emptid=" & emptid & " and date_end is null", Session("con"))
        If posi <> "None" Then
            outp &= posi & "</td>"
        Else
            posi = fm.getinfo2("select position from emp_job_assign where emptid=" & emptid & " and (date_from between '" & prdd & "' and isnull(date_end,'" & prdd.AddDays(Date.DaysInMonth(prdd.Year, prdd.Month)) & "') or  '" & prdd & "' between date_from and isnull(date_end,'" & prdd.AddDays(Date.DaysInMonth(prdd.Year, prdd.Month)) & "') ) ", Session("con"))
            outp &= posi & "</td>"
        End If

        outp &= " <td colspan='9'>&nbsp; </td></tr>" & Chr(13)
        'line 7
        outp &= " <tr>"
        outp &= " <td style=" & Chr(34) & "" & Chr(34) & ">"
        outp &= "  <strong>project Name..</strong></td><td>:</td>"
        Dim rtp() As String
        '  Response.Write(prdd)
        rtp = fm.getproj_on_date(emptid, prdd, Session("con"))
        outp &= "  <td colspan='2'>" & _
        rtp(1) & "</td>"
        outp &= " <td colspan='9'>&nbsp;</td></tr>" & Chr(13)
        'line 8
        outp &= " <tr>"
        outp &= " <td> "
        outp &= " <strong >Perdiem Rate (Birr):</strong></td><td>:</td>"
        outp &= "   <td colspan='2'>"
        Dim rate As Double = CDbl(fm.numdigit(fm.getinfo2("select pardim from pardimpay where emptid=" & emptid & " and ref='" & pref & "'", Session("con")), 2))
        outp &= rate.ToString & "</td>"
        outp &= " <td colsapn='9'>&nbsp;</td></tr>" & Chr(13)
        'line 9
        outp &= " <tr>"
        outp &= " <td style=" & Chr(34) & "height: 21px;" & Chr(34) & " colspan=" & Chr(34) & "13" & Chr(34) & " >"
        outp &= " &nbsp;</td>"
        outp &= " </tr>" & Chr(13)
        'line 10
        outp &= " <tr>"
        outp &= " <td colspan='13'>"
        outp &= " <table align='center' id='tblneed' cellpadding='0' cellspacing='0'><tr><td>"
        outp &= "  Purpose of Travel</td><td>"
        outp &= " <strong>Departure Date (M/D/Y)</strong></td>"
        outp &= " <td width='9'>&nbsp;</td>"
        outp &= " <td>"
        outp &= "   <strong>Return Date (M/D/Y)</strong></td>"
        outp &= " <td width='77'>"
        outp &= "   <strong>No. Days</strong></td>"
        outp &= "   </tr>" & Chr(13)
        'line 11
        ' Dim ref As String
        ref = Now.Ticks.ToString + emptid.ToString
        ' Response.Write(Now.Ticks.ToString)
        ref = sec.StrToHex(ref)

        ' Dim sql() As String = {" & chr(34) & "" & chr(34) & ", " & chr(34) & "", "", "", "", ""}

        Dim sumdays As Integer = 0
        Dim noday As Integer
        Dim adv As Double = 0
        Dim paypar As Double = 0
        Dim advtext As String = 0
        Dim j As Integer = 0
        For i = 0 To UBound(arrp)
            If String.IsNullOrEmpty(arrp(i)) = False Then


                noday = fm.getinfo2("select no_days from pardimpay where id=" & arrp(i), Session("con"))
                outp &= "   <tr>"
                outp &= " <td >"
                outp &= fm.getinfo2("select reason from pardimpay where id=" & arrp(i), Session("con"))
                outp &= " </td>"
                outp &= " <td width='177'>"
                outp &= CDate(fm.getinfo2("select from_date from pardimpay where id=" & arrp(i), Session("con"))).ToShortDateString
                outp &= " </td>"
                outp &= "  <td>&nbsp;</td>"
                outp &= " <td width='177'>"
                outp &= CDate(fm.getinfo2("select to_date from pardimpay where id=" & arrp(i), Session("con"))).ToShortDateString
                outp &= " </td>"
                outp &= " <td style=" & Chr(34) & "text-align:right;" & Chr(34) & ">"
                outp &= fm.getinfo2("select no_days from pardimpay where id=" & arrp(i), Session("con"))
                outp &= "     </td>"
                outp &= "   </tr>" & Chr(13)
                advtext = fm.getinfo2("select adv from pardimpay where id=" & arrp(i), Session("con")).ToString

                If String.IsNullOrEmpty(advtext) = False Then
                    If advtext <> "None" Then
                        ' Response.Write(advtext)
                        adv += CDbl(advtext.ToString)
                    End If
                End If
                sumdays += CDbl(noday)

            End If

        Next

        outp &= " <tr>"
        outp &= " <td colspan='4' class='tcenter'>Total No. of Days</td>"
        outp &= " <td style=" & Chr(34) & "text-align:right;" & Chr(34) & ">&nbsp;"
        outp &= sumdays.ToString
        ' Response.Write(ref)
        outp &= " </td>"
        outp &= " </tr>" & Chr(13)
        outp &= " </table></td>"
        outp &= " </tr>" & Chr(13)
        'line 
        outp &= " <tr>"
        outp &= " <td style=" & Chr(34) & "width: 30px" & Chr(34) & " colspan=" & Chr(34) & "13" & Chr(34) & "></td>"
        outp &= " </tr>" & Chr(13)
        'line
        outp &= " <tr>"
        outp &= " <td colspan='' style=" & Chr(34) & "height: 23px" & Chr(34) & ">"
        outp &= " Perdiem Amount (Birr):-</td><td>&nbsp;</td>"
        outp &= " <td  colspan='4' style=" & Chr(34) & "height: 23px" & Chr(34) & ">"
        paypar = CDbl(rate) * CDbl(sumdays)
        outp &= (fm.numdigit(paypar, 2).ToString)
        outp &= " </td>"

        outp &= "<td colspan='7'>&nbsp;</td> </tr>" & Chr(13)

        outp &= "<tr>"
        outp &= " <td colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= " <td colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "</tr>" & Chr(13)
        outp &= " <tr>"
        outp &= " <td style=" & Chr(34) & "" & Chr(34) & " >"
        outp &= " <strong>Amount in Words</strong></td><td>:</td>"
        outp &= " <td colspan='9' style=" & Chr(34) & "height: 23px;text-align:left;width:300px" & Chr(34) & ">" & Chr(13)
        outp &= "<span id='amtword-" & pref & "' style=" & Chr(34) & "font-style:italic; text-decoration:underline;" & Chr(34) & "></span>" & Chr(13)
        outp &= "</td>" & Chr(13)
        wrd &= "getwordjs('" & paypar & "','amtword-" & pref & "');"
        outp &= "<td colspan='2'>&nbsp;</td></tr>" & Chr(13)
        outp &= "<tr>"
        outp &= "<td style=" & Chr(34) & "height: 23px" & Chr(34) & " >" & Chr(13)
        outp &= "<strong>perdiem expense</strong></td><td>&nbsp;</td>" & Chr(13)
        outp &= "<td colspan='5' style=" & Chr(34) & "height: 23px; text-align:right;" & Chr(34) & ">"
        outp &= (fm.numdigit(paypar, 2).ToString)
        outp &= "&nbsp;</td><td colspan='7' style='width:150px;'>&nbsp;</td></tr>" & Chr(13)
        outp &= "<tr>"
        outp &= "<td colspan=" & Chr(34) & "" & Chr(34) & ">"
        outp &= "<strong>advance cpv#</strong></td><td>&nbsp;</td>"
        outp &= "<td colspan='5' style=" & Chr(34) & "text-align:right;" & Chr(34) & ">"
        outp &= "<span style='border-bottom: 1px solid #000;'>(" & fm.numdigit(adv, 2).ToString & ")</span></td>"
        outp &= "<td colspan='7' style='width:150px;'>&nbsp;</td></tr>" & Chr(13)
        outp &= "<tr>"
        outp &= " <td style=" & Chr(34) & "height: 20px" & Chr(34) & ">"
        outp &= "<strong>Balance Due to Traveller:</strong></td><td></td>"
        outp &= " <td colspan='5' style=" & Chr(34) & "height: 20px; text-align:right" & Chr(34) & "><span style=" & Chr(34) & "text-decoration:underline;border-bottom: 1px solid #000;" & Chr(34) & ">  "
        outp &= (fm.numdigit(CDbl(paypar) - CDbl(adv), 2).ToString)
        outp &= "</span>"
        outp &= "</td>"

        outp &= "<td colspan='7'  style='width:150px;'>&nbsp;</td></tr>" & Chr(13)
        outp &= "<tr>"
        outp &= "<td colspan=" & Chr(34) & "13" & Chr(34) & " ></td>"

        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= "    <td style=" & Chr(34) & "height: 23px" & Chr(34) & "><strong>Payment Method</strong> </td><td></td>"
        outp &= "<td colsapn='5' style=" & Chr(34) & "height: 23px" & Chr(34) & " >&nbsp;"
        outp &= (fm.getinfo2("select mthd from pardimpay where ref='" & pref & "'", Session("con")))

        outp &= "</td><td colsapn='7'>&nbsp;</td>"
        outp &= "</tr><tr>"
        outp &= "<td style=" & Chr(34) & "height: 23px;" & Chr(34) & " colspan=" & Chr(34) & "13" & Chr(34) & "></td>"
        outp &= "</tr>" & Chr(13)
        outp &= "  <tr>"
        outp &= "<td style=" & Chr(34) & "height: 23px;" & Chr(34) & " colspan=" & Chr(34) & "13" & Chr(34) & "></td>"

        outp &= "<td colspan=" & Chr(34) & "13" & Chr(34) & "></td>"
        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= " <td colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= " <td colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= " <td colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "</tr>" & Chr(13)

        outp &= "<tr>"
        outp &= "<td style=" & Chr(34) & "height: 22px;" & Chr(34) & " colspan=" & Chr(34) & "13" & Chr(34) & "></td>"
        outp &= "</tr>" & Chr(13)

        outp &= " <tr>"
        outp &= "<td colspan=" & Chr(34) & "13" & Chr(34) & ">"
        outp &= "<table cellspacing='0' cellpadding='0' align='center' width=600>"
        outp &= "<tr>"
        outp &= "<td style='width:100px;text-align:center;' colspan='3'>____________</td>"

        outp &= "<td style='width:100px;text-align:center;'colspan='3'>_____________</td>"

        outp &= "<td style='width:100px;text-align:center;'colspan='3'>____________</td>"

        outp &= " <td style='width:100px;text-align:center;'colspan='3'>____________</td>"

        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"

        outp &= "<td style='width:100px;text-align:center;' colspan='3'> Receiver's <br>Signature</td>"

        outp &= "<td style='width:100px;text-align:center;'  colspan='3'>Prepared&nbsp; By</td>"

        outp &= "<td style='width:100px;text-align:center;' colspan='3'>Checked By</td>"

        outp &= "<td style='width:100px;text-align:center;' colspan='3'>Approved By</td>"
        outp &= " </tr>" & Chr(13)
        outp &= "<tr>"
        outp &= " <td colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= " <td colspan=" & Chr(34) & "13" & Chr(34) & ">&nbsp;</td>"
        outp &= "</tr>" & Chr(13)
        outp &= "<tr>"
        outp &= "<td style='width:100px;text-align:center;' colspan='3'>____________</td>"

        outp &= "<td style='width:100px;text-align:center;'colspan='3'>____________</td>"

        outp &= "<td style='width:100px;text-align:center;'colspan='3'>____________</td>"

        outp &= "<td style='width:100px;text-align:center;'colspan='3'>____________</td>"
        outp &= "</tr>" & Chr(13)
        outp &= " <tr>"
        outp &= "<td style='width:100px;text-align:center;' colspan='3'> Date</td>"

        outp &= "<td style='width:100px;text-align:center;'colspan='3'>Date</td>"

        outp &= "<td style='width:100px;text-align:center;'colspan='3'>Date</td>"

        outp &= " <td style='width:100px;text-align:center;'colspan='3'>Date</td>"
        outp &= "</tr></table>" & Chr(13)
        outp &= "</td>"
        outp &= "</tr>" & Chr(13)

        outp &= "</table>"
        Dim wrx(), wrx2() As String
        wrx = isinproject(emptid, d1, d2, Session("con")).split(",")


        outp &= "<table><tr><td>"
        For p As Integer = 0 To wrx.Length - 1
            ' outp &= wrx(p)
        Next

        outp &= "</td></tr></table>"
        Dim rtn(2) As String
        rtn(0) = outp
        rtn(1) = wrd
        Return rtn
    End Function
    Function outpall(ByVal txt As String)
        ' Response.Write(txt)
        Dim spl(), mk, jv, rtn(2) As String
        mk = ""
        jv = ""
        Dim i As Integer = 0
        spl = txt.Split(",")

        For i = 0 To spl.Length - 2

            mk &= "<div class='page'>"
            mk &= " <div class='subpage' >" & i + 1 & "/" & spl.Length - 1
            rtn = outpx(spl(i).ToString)
            mk &= rtn(0)

            mk &= "</div> "
            If i = spl.Length - 2 Then
                ' mk &= " <span style='vertical-align:middle;position:relative;cursor:pointer' onclick='javascript:printpv();'><img src='images/ico/print.ico' alt='print'/>Print</span>"
            End If
            mk &= "<span class='footer' style='font-size:10px;color:gray;'><u>" & Session("maker") & "</u></span>"
            mk &= "</div>"
            jv &= rtn(1)


        Next
        rtn(0) = mk
        rtn(1) = jv
        Response.Write(mk)
       
        Return rtn
    End Function
    Function isinproject2(ByVal emptid As String, ByVal fdate As Date, ByVal edate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        rs = dbs.dtmake("vwisproj", "select * from emp_job_assign where emptid=" & emptid & " and '" & fdate & "' between date_from and isnull(date_end,'" & edate & "') or date_from  between '" & fdate & "' and isnull(date_end,'" & edate & "')  order by date_from", con)
        If rs.HasRows = True Then
            rs.Read()
            Return rs.Item("project_id")
        Else
            Return "None"
        End If
    End Function
    Function isinproject(ByVal emptid As String, ByVal fdate As Date, ByVal edate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim rtn As String = ""
        rs = dbs.dtmake("vwisproj", "select * from emp_job_assign where emptid=" & emptid & " and '" & fdate & "' between date_from and isnull(date_end,'" & edate & "') or date_from  between '" & fdate & "' and isnull(date_end,'" & edate & "')  order by date_from", con)
        '  Response.Write("select * from emp_job_assign where emptid=" & emptid & " and '" & fdate & "' between date_from and isnull(date_end,'" & edate & "') or date_from  between '" & fdate & "' and isnull(date_end,'" & edate & "')  order by date_from<br>")
        If rs.HasRows = True Then
            While rs.Read
                rtn &= rs.Item("project_id") & "$" & rs.Item("date_end") & ","
            End While
            Return rtn.Substring(0, rtn.Length - 1)
        Else
            Return "None"
        End If

    End Function
End Class
