Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ot3
    Inherits System.Web.UI.Page
    Function viewforedit(ByVal pproj As String, ByVal pdate1 As Date, ByVal pdate2 As Date)
        Dim fhr As String
        If File.Exists(Session("path") & "\text\factor.txt") = False Then
            fhr = "200.67"
        Else
            fhr = File.ReadAllText(Session("path") & "\text\factor.txt")
            ' Response.Write(fhr)
        End If
        If pproj <> "" Then

            Dim fm As New formMaker
            Dim rmk As String = ""
            Dim reg, sun, nig, hd As String
            Dim sumreg, sumsun, sumnig, sumhd, sal(2), otbir, hr, shr, fhrs As String
            Dim i As Integer = 1
            otbir = "0"
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim spl() As String
            Dim projid As String = ""
            Dim nod As Integer
            Dim sqlx As String
            Dim rtnvalue, rssql, rssqlnew As String
            If pproj <> "" Then 'Then
                spl = pproj.Split("|")
                projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
            End If

            Dim rs2 As DataTableReader
            If projid = "" Then
                rs2 = dbx.dtmake("selc_rec", "select * from emprec ", Session("con"))
            Else
                'rs2 = dbx.dtmake('selc_rec', 'select * from emprec where end_date is null', Session('con'))

                ' rs2 = dbx.dtmake("selc_rec", "select * from emprec as erc inner join emp_static_info as esi on esi.emp_id = erc.emp_id where erc.id in(select emptid from emp_job_assign where project_id=" & projid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between  '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))) order by esi.first_name", Session("con"))
                ' Response.Write('select * from emprec where end_date is null and id in(select emptid from emp_job_assign project_id='' & projid & '')')

                rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "and emprec.id in(select emptid from emp_job_assign " & _
                                                                  "where project_id='" & projid.ToString & "' " & _
                                                                  "and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "
                '  rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                 "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                "and emprec.id in(select emptid from emp_job_assign " & _
                '                                               "where project_id='" & projid.ToString & "' " & _
                '                                              "and ('" & pdate2 & "' >= date_from and '" & pdate2 & "' <= isnull(date_end,'" & Today.ToShortDateString & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                '                                             " ORDER BY emp_static_info.first_name,emprec.id desc "

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "
                'Response.Write(rssqlnew & "<br>")
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                                    "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                                  "and emprec.id in(" & rtnvalue & ") ORDER BY emp_static_info.first_name,emprec.id desc ")
                Session("payrolllist") = rssqlnew
                'Response.Write(rtnvalue)
                rs2 = dbx.dtmake("selectemp", rssqlnew, Session("con"))
            End If
            Me.outp.Text = "<div id='printview'>"
            Me.outp.Text &= "  <style> " & Chr(13) & _
                ".tb1" & Chr(13) & _
            "{" & Chr(13) & _
               " border:1px solid black;" & Chr(13) & _
               " font-size:10pt;" & Chr(13) & _
            "}" & Chr(13) & _
            ".tb1 td" & Chr(13) & _
            "{" & _
             "border-top: 1px solid black;" & Chr(13) & _
             "border-left:1px solid black;" & Chr(13) & _
               " font-size:9pt;" & Chr(13) & _
            "}" & Chr(13) & _
"          </style>" & Chr(13) & _
" Unpaid List<table class='tb1' id='tblot' width='550px' cellpadding='0' cellspacing='0'>" & Chr(13) & _
 "<tr id='r0' style='text-align:center;font-weight:bold;' >" & Chr(13) & _
              " <td id='h1' style='text-align:center;font-weight:bold;font-size:10pt' colspan='10' >" & Session("company_name") & _
               "<br /> Project Name:"
            If projid <> "" Then
                Me.outp.Text &= (spl(0).ToString)
            Else
                Me.outp.Text &= ("All Projects")
            End If
            Me.outp.Text &= "<br /> Overtime Payment Sheet for the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</td>" & _
"              </tr>" & Chr(13) & _
"<tr id='r1' style=' text-align:center;'>" & Chr(13) & _
   " <td id='h2-1' rowspan='2'><span class='headtxt'><strong>&nbsp;No</strong></span></td>" & Chr(13) & _
    "<td id='h2-2' rowspan='2'><span class='headtxt'><strong>&nbsp;Employee Name</strong></span></td>" & Chr(13) & _
    "<td id='h2-3'  rowspan='2'><span class='headtxt'><strong>&nbsp;Basic Salary</strong></span></td>" & Chr(13) & _
"    <td id='h2-4' colspan='4'><span class='headtxt'><strong>&nbsp;Overtime Hours</strong></span></td>" & Chr(13) & _
    "<td id='h2-5'  rowspan='2'><span class='headtxt'><strong>&nbsp;H.Rate</strong></span></td>" & Chr(13) & _
    "<td id='h2-6'  rowspan='2'><span class='headtxt'><strong>&nbsp;F.Hrs **</strong></span></td>" & Chr(13) & _
    "<td id='h2-7'  rowspan='2'><span class='headtxt'><strong>&nbsp;OT. Birr</strong></span></td>" & Chr(13) & _
     "<td id='h2-7'  rowspan='2'><span class='headtxt'><strong>&nbsp;Remark</strong></span></td>" & Chr(13) & _
  "</tr>" & Chr(13) & _
  "<tr id='rx'>" & Chr(13) & _
    "<td  id='h2-4-1' ><span class='headtxt'><strong>&nbsp;Reg.</strong></span></td>" & Chr(13) & _
    "<td id='h2-4-2' ><span class='headtxt'><strong>&nbsp;Nig.</strong></span></td>" & Chr(13) & _
    "<td id='h2-4-3' ><span class='headtxt'><strong>&nbsp;Sun.</strong></span></td>" & Chr(13) & _
    "<td id='h2-4-4' ><span class='headtxt'><strong>&nbsp;H.D</strong></span></td>" & Chr(13) & _
  "</tr>" & Chr(13)
            Dim sec As New k_security
            Dim fullname As String
            Dim inforate As String = "**Factored Hours: "
            Dim irn(), ira(), rrate As String
            Dim rrs As DataTableReader
            i = 0
            rrs = dbx.dtmake("otratet", "select distinct ot_abr, ot_name from tblot_rate", Session("con"))
            If rrs.HasRows Then
                While rrs.Read
                    ReDim Preserve irn(i + 1)
                    ReDim Preserve ira(i + 1)
                    irn(i) = rrs.Item("ot_name")
                    ira(i) = rrs.Item("ot_abr")
                    i = i + 1
                End While
            End If

            rrs.Close()
            rrs = Nothing
            i = 0
            If irn.Length > 0 Then
                For i = 0 To UBound(irn) - 1
                    rrate = fm.getinfo2("select rate from tblot_rate where '" & pdate1 & "' between date_start and isnull(date_end,'" & Today.ToShortDateString & "') and ot_abr='" & ira(i) & "'", Session("con"))
                    inforate &= ira(i) & "(" & irn(i) & ")" & " X " & rrate
                    If i < UBound(ira) - 1 Then
                        inforate &= " + "
                    End If

                Next
            End If



            If rs2.HasRows Then
                otbir = "0"
                Dim rowc, colc As Integer
                rowc = 0

                Dim rt() As String
                While rs2.Read
                    fullname = ""


                    rs = dbx.dtmake("otx", "select * from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='n'", Session("con"))


                    If rs.HasRows Then
                        rowc = rowc + 1
                        rs.Read()
                        sqlx = "select id,ot_date,time_diff,factored,rate,amt,datepaid,remark from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='n'"
                        sqlx = sec.dbStrToHex(sqlx)
                        Dim wd As Date
                        reg = ""
                        sun = ""
                        nig = ""
                        hd = ""
                        If rs.IsDBNull(14) = True Then
                            rmk = "&nbsp;"
                        Else


                            rmk = rs.Item("remark")


                            'make array which is not same project...
                        End If

                        sumreg = ""
                        sumnig = ""
                        sumhd = ""
                        sumsun = ""
                        fullname = fm.getfullname(rs2.Item("emp_id"), Session("con"))
                        Me.outp.Text &= "  <tr style='font-size:7pt;font-weight:none' id='" & rowc.ToString & "'>" & Chr(13) & _
                        " <td id='" & rowc.ToString & "-1'><span style='color:blue;cursor:pointer;' onclick=" & Chr(34) & _
                        "edit('" & sqlx & "','" & pdate1 & "','" & pdate2 & "','" & fullname & " ') " & _
                        Chr(34) & " title='Edit/Delete'>" & rowc.ToString & "</span></td>" & Chr(13) & _
                         "<td id='" & rowc.ToString & "-2'>&nbsp;" & fullname & " </td>" & Chr(13) & _
    " <td style='text-align:right'>&nbsp;" & Chr(13)
                        If rs.IsDBNull(11) = False Then
                            wd = rs.Item("ot_date")
                        End If
                        sal = dbx.getsal(rs2.Item("id"), wd.ToShortDateString, Session("con"))
                        shr = Math.Round(CDbl(sal(0)) / CDbl(fhr), 2).ToString
                        Me.outp.Text &= (FormatNumber(sal(0), 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"

                        Me.outp.Text &= "<td id='" & rowc.ToString & "-3'>&nbsp;"
                        reg = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg'  and paidstatus='n'", Session("con"))
                        If reg.ToString <> "" And reg.ToString <> "None" Then
                            Me.outp.Text &= (reg)
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg'  and paidstatus='n'", Session("con"))
                            If hr = "None" Then
                                hr = "1"
                            End If

                            sumreg = (CDbl(reg) * CDbl(hr)).ToString
                            '  Response.Write(sumreg & "=" & reg & "*" & hr & "<br>")
                        Else
                            sumreg = "0"
                        End If
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= " <td id='" & rowc.ToString & "-4'>&nbsp;"
                        nig = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig' and time_diff is Not Null  and paidstatus='n'", Session("con"))
                        If nig.ToString <> "" And nig <> "None" Then
                            Me.outp.Text &= nig
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig' and paidstatus='n'", Session("con"))
                            If hr = "None" Then
                                hr = "1"

                            End If
                            ' Response.Write(nig.ToString & hr & 'nig<br>')
                            sumnig = (CDbl(nig) * CDbl(hr)).ToString
                        Else
                            sumnig = "0"
                        End If

                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= " <td id='" & rowc.ToString & "-5'>&nbsp;"
                        sun = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we' and paidstatus='n'", Session("con"))

                        If sun.ToString <> "" And sun <> "None" Then
                            Me.outp.Text &= sun
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='n'", Session("con"))
                            If hr = "None" Then
                                hr = "1"

                            End If
                            'Response.Write(sun & hr & 'sun<br>')
                            sumsun = (CDbl(sun) * CDbl(hr)).ToString
                        Else
                            sumsun = "0"
                        End If

                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td id='" & rowc.ToString & "-6'>&nbsp;"
                        hd = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd' and paidstatus='n'", Session("con"))

                        If hd.ToString <> "" And hd <> "None" Then
                            Me.outp.Text &= (hd)
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd' and paidstatus='n'", Session("con"))
                            If hr = "None" Then
                                hr = "1"
                            End If
                            'Response.Write(hd & hr & 'hd<br>')
                            sumhd = (CDbl(hd) * CDbl(hr)).ToString
                        Else
                            sumhd = "0"
                        End If
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td id='" & rowc.ToString & "-7' style='text-align:right;'>&nbsp;"
                        Me.outp.Text &= (FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"
                        Me.outp.Text &= "<td style='text-align:right;' id='" & rowc.ToString & "-8'>&nbsp;"
                        Dim showamt As Double = 0
                        sqlx = "select sum(amt) as exp1 from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='n'"
                        showamt = fm.getinfo2(sqlx, Session("con"))
                        ' Response.Write(sqlx & "<br>")

                        fhrs = (CDbl(sumsun) + CDbl(sumreg) + CDbl(sumhd) + CDbl(sumnig)).ToString
                        Dim amtx As Double = Math.Round((CDbl(fhrs) * CDbl(shr)), 2)

                        ' Response.Write("<br>..............................<br>" & sumsun & "<br>" & sumreg & "<br>" & sumhd & "<br>" & sumnig & "<br>FH: " & fhrs & "<br>Sal: " & sal(0) & "<br>" & amtx & "====" & showamt & "<br>" & shr & "<br>" & Math.Round((CDbl(showamt) - CDbl(amtx)), 2).ToString & " <br>..................<br>")


                        ' Response.Write(fhrs & "=" & sumsun & " + " & (sumreg) & " + " & (sumhd) & " + " & (sumnig) & "==>" & FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString & "==" & amtx.ToString & "<br>")
                        Me.outp.Text &= (FormatNumber(fhrs, 2, TriState.True, TriState.True, TriState.True).ToString) & " </td>"
                        Me.outp.Text &= "<td  id='" & rowc.ToString & "-9' style='text-align:right;'>&nbsp;"
                        otbir = (Math.Round(CDbl(otbir), 2) + showamt).ToString

                        Me.outp.Text &= FormatNumber((showamt), 2, TriState.True).ToString
                        If rs.IsDBNull(3) = True Then
                            Me.outp.Text &= ""
                        End If
                        Me.outp.Text &= "  </td><td>&nbsp;" & rmk & "</td>"
                        Me.outp.Text &= "  </tr>"
                        i += 1
                    End If

                End While
            End If
            Me.outp.Text &= " <tr><td>&nbsp;</td><td>&nbsp;Total</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
            "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;'>"
            Me.outp.Text &= (FormatNumber(otbir, 2, TriState.True, TriState.True, TriState.True)) & "</td><td>&nbsp;</td></tr>"
            Me.outp.Text &= "</table>"
            Me.outp.Text &= "<div style='font-size:10pt;'> *Hourly Rate=Basic Salary/" & fhr & "<br />"
            Me.outp.Text &= "Where " & fhr & " is Average Normal Working Hours in a month<br /> " & _
           inforate & _
      " </div>" & Chr(13) & _
     " <div style='height:15px;'></div>" & _
      "<div style='float:left; width:196px; font-size:9pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>Prepared By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>      " & Chr(13) & _
      "</table>" & Chr(13) & _
     " </div>" & Chr(13) & _
      "<div style='float:left; width:4px;'></div>" & Chr(13) & _
      "<div style='float:left; width:196px;font-size:10pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt; font-size:9pt;'>Chekced By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
"      </table>" & Chr(13) & _
      "</div>" & Chr(13) & _
      "<div style='float:left; width:4px;'></div>" & Chr(13) & _
       "<div style='float:left; width:196px;font-size:10pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>Verified By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
"      </table>" & Chr(13) & _
      "</div>" & Chr(13) & _
      "<div style='clear:both;'></div>" & Chr(13) & _
      "</div>" & Chr(13) & _
" <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printview','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

        End If
    End Function
    Function viewpaid(ByVal pproj As String, ByVal pdate1 As Date, ByVal pdate2 As Date)
        Dim fhr As String
        If File.Exists(Session("path") & "\text\factor.txt") = False Then
            fhr = "200.67"
        Else
            fhr = File.ReadAllText(Session("path") & "\text\factor.txt")
            ' Response.Write(fhr)
        End If
        If pproj <> "" Then

            Dim fm As New formMaker
            ' Dim pa As otview
            Dim reg, sun, nig, hd As String
            Dim sumreg, sumsun, sumnig, sumhd, sal(), otbir, hr, shr, fhrs As String
            Dim i As Integer = 1
            otbir = "0"
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim spl() As String
            Dim projid As String = ""
            Dim nod As Integer

            If pproj <> "" Then 'Then
                spl = pproj.Split("|")
                projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
            End If


            Dim rs2 As DataTableReader
            If projid = "" Then
                rs2 = dbx.dtmake("selc_rec", "select * from emprec ", Session("con"))
            Else
                'rs2 = dbx.dtmake('selc_rec', 'select * from emprec where end_date is null', Session('con'))

                ' rs2 = dbx.dtmake("selc_rec", "select * from emprec inner join emp_static_info as esi on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or  date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))) order by esi.first_name", session("con"))
                'Response.Write("select * from emprec inner join emp_static_info as esi on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')) order by esi.first_name")
                Dim rtnvalue, rssql, rssqlnew As String
                rtnvalue = getprojemp(projid.ToString, pdate2, Session("con"))
                '  rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                 "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                "and emprec.id in(select emptid from emp_job_assign " & _
                '                                               "where project_id='" & projid.ToString & "' " & _
                '                                              "and ('" & pdate2 & "' >= date_from and '" & pdate2 & "' <= isnull(date_end,'" & Today.ToShortDateString & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                '                                             " ORDER BY emp_static_info.first_name,emprec.id desc "

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                                                                  "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                                                                  "and emprec.id in(" & rtnvalue & ")" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "
                'Response.Write(rssqlnew & "<br>")
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                                    "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                                  "and emprec.id in(" & rtnvalue & ") ORDER BY emp_static_info.first_name,emprec.id desc ")
                'Session("payrolllist") = rssqlnew
                'Response.Write(rtnvalue)
                rs2 = dbx.dtmake("selectemp", rssqlnew, Session("con"))
            End If

            Me.outp2.Text &= "<div id='printviewpaid'>" & _
            "  <style> " & Chr(13) & _
                ".tb1" & Chr(13) & _
            "{" & Chr(13) & _
               " border:1px solid black;" & Chr(13) & _
               " font-size:10pt;" & Chr(13) & _
            "}" & Chr(13) & _
            ".tb1 td" & Chr(13) & _
            "{" & _
             "border-top: 1px solid black;" & Chr(13) & _
             "border-left:1px solid black;" & Chr(13) & _
               " font-size:9pt;" & Chr(13) & _
            "}" & Chr(13) & _
"          </style>" & Chr(13) & _
" <table class='tb1' width='600px' cellpadding='0' cellspacing='0'>" & Chr(13) & _
 "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
              " <td style='text-align:center;font-weight:bold;font-size:13pt' colspan='10' >" & Session("company_name") & _
               "<br /> Project Name:"
            If projid <> "" Then
                Me.outp2.Text &= (spl(0).ToString)
            Else
                Me.outp2.Text &= ("All Projects")
            End If
            Me.outp2.Text &= "<br /> Overtime Total Paid for the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</td>" & _
            "<br><span style='color:red;'>Paid List</span>" & _
"              </tr>" & Chr(13) & _
"<tr style=' text-align:center;'>" & Chr(13) & _
   " <td width='20' rowspan='2'><span class='headtxt'><strong>&nbsp;No</strong></span></td>" & Chr(13) & _
    "<td width='150' rowspan='2'><span class='headtxt'><strong>&nbsp;Employee Name</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;Basic Salary</strong></span></td>" & Chr(13) & _
"    <td colspan='4'><span class='headtxt'><strong>&nbsp;Overtime Hours</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;H.Rate</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;F.Hrs **</strong></span></td>" & Chr(13) & _
    "<td width='79' rowspan='2'><span class='headtxt'><strong>&nbsp;OT. Birr</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;Paid Date</strong></span></td>" & Chr(13) & _
  "</tr>" & Chr(13) & _
  "<tr>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Reg.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Nig.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Sun.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;H.D</strong></span></td>" & Chr(13) & _
  "</tr>" & Chr(13)
            Dim inforate As String = "**Factored Hours: "
            Dim irn(), ira(), rrate As String
            Dim rrs As DataTableReader
            i = 0
            rrs = dbx.dtmake("otratet", "select distinct ot_abr, ot_name from tblot_rate", Session("con"))
            If rrs.HasRows Then
                While rrs.Read
                    ReDim Preserve irn(i + 1)
                    ReDim Preserve ira(i + 1)
                    irn(i) = rrs.Item("ot_name")
                    ira(i) = rrs.Item("ot_abr")
                    i = i + 1
                End While
            End If

            rrs.Close()
            rrs = Nothing
            i = 0
            If irn.Length > 0 Then
                For i = 0 To UBound(irn) - 1
                    rrate = fm.getinfo2("select rate from tblot_rate where '" & pdate1 & "' between date_start and isnull(date_end,'" & Today.ToShortDateString & "') and ot_abr='" & ira(i) & "'", Session("con"))
                    inforate &= ira(i) & "(" & irn(i) & ")" & " X " & rrate
                    If i < UBound(ira) - 1 Then
                        inforate &= " + "
                    End If

                Next
            End If
            If rs2.HasRows Then
                otbir = "0"
                Dim ot_date As Date
                Dim otamt As String
                
                Dim rt() As String
                Dim xrs As String = ""
                While rs2.Read

                    rs = dbx.dtmake("ot", "select * from emp_ot  where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref is not null order by datepaid", Session("con"))
                    otamt = fm.getinfo2("select sum(amt) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref is not null", Session("con"))
                    'Response.Write(otamt.ToString)

                    If IsNumeric(otamt) = False Then
                        otamt = "0"
                    End If
                    If CDbl(otamt) > 0 Then

                        If rs.HasRows Then
                            rs.Read()
                            reg = ""
                            sun = ""
                            nig = ""
                            hd = ""
                            xrs = fm.getinfo2("select ot from payrollx where ref='" & rs.Item("ref") & "' and emptid='" & rs.Item("emptid") & "'", Session("con"))


                            ot_date = rs.Item("ot_date")
                            Me.outp2.Text &= "  <tr>" & Chr(13) & _
                            " <td>&nbsp;" & i.ToString & "</td>" & Chr(13) & _
                             "<td>&nbsp;" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & " </td>" & Chr(13) & _
       " <td style='text-align:right'>&nbsp;" & Chr(13)
                            sal = dbx.getsal(rs2.Item("id"), ot_date, Session("con"))
                            shr = Math.Round((CDbl(sal(0)) / CDbl(fhr)), 2).ToString
                            Me.outp2.Text &= (FormatNumber(sal(0), 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"

                            Me.outp2.Text &= "<td>&nbsp;"
                            reg = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y' and ref is not null", Session("con"))
                            If reg.ToString <> "" And reg.ToString <> "None" Then
                                Me.outp2.Text &= (reg)
                                hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y'  and ref is not null", Session("con"))
                                If hr = "None" Then
                                    hr = "1"
                                End If
                                'Response.Write(reg & hr & 'reg<br>')
                                sumreg = (CDbl(reg) * CDbl(hr)).ToString
                            Else
                                sumreg = "0"
                            End If
                            Me.outp2.Text &= "</td>"
                            Me.outp2.Text &= " <td>&nbsp;"
                            nig = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig' and time_diff is Not Null  and paidstatus='y'  and ref is not null", Session("con"))
                            If nig.ToString <> "" And nig <> "None" Then
                                Me.outp2.Text &= nig
                                hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig'  and paidstatus='y' and ref is not null", Session("con"))
                                If hr = "None" Then
                                    hr = "1"

                                End If
                                ' Response.Write(nig.ToString & hr & 'nig<br>')
                                sumnig = (CDbl(nig) * CDbl(hr)).ToString
                            Else
                                sumnig = "0"
                            End If

                            Me.outp2.Text &= "</td>"
                            Me.outp2.Text &= " <td>&nbsp;"
                            sun = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y' and ref is not null", Session("con"))

                            If sun.ToString <> "" And sun <> "None" Then
                                Me.outp2.Text &= sun
                                hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y' and ref is not null", Session("con"))
                                If hr = "None" Then
                                    hr = "1"

                                End If
                                'Response.Write(sun & hr & 'sun<br>')
                                sumsun = (CDbl(sun) * CDbl(hr)).ToString
                            Else
                                sumsun = "0"
                            End If

                            Me.outp2.Text &= "</td>"
                            Me.outp2.Text &= "<td>&nbsp;"
                            hd = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y' and ref is not null", Session("con"))

                            If hd.ToString <> "" And hd <> "None" Then
                                Me.outp2.Text &= (hd)
                                hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y' and ref is not null", Session("con"))
                                If hr = "None" Then
                                    hr = "1"
                                End If
                                'Response.Write(hd & hr & 'hd<br>')
                                sumhd = (CDbl(hd) * CDbl(hr)).ToString
                            Else
                                sumhd = "0"
                            End If
                            Dim showamt As Double = 0
                            showamt = fm.getinfo2("select sum(amt) as amt from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref='" & rs.Item("ref") & "'", Session("con"))


                            Me.outp2.Text &= "</td>"
                            Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                            Me.outp2.Text &= (FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"
                            Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                            fhrs = (CDbl(sumsun) + CDbl(sumreg) + CDbl(sumhd) + CDbl(sumnig)).ToString
                            ' If (Math.Round(CDbl(xrs), 2) <> Math.Round(showamt, 2)) Then
                            '  Dim msgx As String = (xrs.ToString & "==" & showamt.ToString & "==" & (FormatNumber(CDbl(shr), 2, TriState.True, TriState.True, TriState.True) * CDbl(fhrs)).ToString & "==" & shr.ToString & "==" & hd.ToString & " =")
                            ' fm.mailsender("<div>Net Consult msg!<br>" & msgx & "</div>", "z.kirubel@gmail.com", "kirsoftet@gmail.com", "Net OT Error line496")
                            ' Response.Write("<br>xrs=" & xrs & " ====showamt:" & showamt)
                            '  Response.Write("<br>....................<br>" & msgx & "<br>..........................")
                            'End If
                            ' Response.Write("select sum(amt) as amt from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y'====" & fhrs & "<br>")
                            Me.outp2.Text &= (FormatNumber(fhrs, 2, TriState.True, TriState.True, TriState.True).ToString) & " </td>"
                            Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"

                            otbir = CDbl(otbir) + CDbl(xrs)
                            ' otbir = (Math.Round(CDbl(otbir), 2) + otamt).ToString
                            Me.outp2.Text &= (FormatNumber(otamt, 2, TriState.True, TriState.True, TriState.True).ToString)
                            Me.outp2.Text &= "</td>"
                            Me.outp2.Text &= "<td>" & rs.Item("datepaid") & "</td>"
                            Me.outp2.Text &= "  </tr>"
                            i += 1
                        End If
                    End If
                    rs.Close()

                End While
            End If
            Me.outp2.Text &= " <tr><td>&nbsp;</td><td>&nbsp;Total</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
            "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;'>"
            Me.outp2.Text &= (FormatNumber(otbir, 2, TriState.True, TriState.True, TriState.True)) & "</td><td>&nbsp;</td></tr>"
            Me.outp2.Text &= "</table>"
            Me.outp2.Text &= "<div style='font-size:10pt;'> *Hourly Rate=Basic Salary/" & fhr & "<br />"
            Me.outp2.Text &= "Where " & fhr & " is Average Normal Working Hours in a month<br /> " & _
            inforate & _
      " </div>" & Chr(13) & _
     " <div style='height:15px;'></div>" & _
      "<div style='float:left; width:196px; font-size:9pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>Prepared By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>      " & Chr(13) & _
      "</table>" & Chr(13) & _
     " </div>" & Chr(13) & _
             "<div style='float:left; width:4px;'></div>" & Chr(13) & _
      "<div style='float:left; width:196px;font-size:10pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt; font-size:9pt;'>Chekced By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
"      </table>" & Chr(13) & _
      "</div>" & Chr(13) & _
      "<div style='float:left; width:4px;'></div>" & Chr(13) & _
      "<div style='float:left; width:196px;font-size:10pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>Verified By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
"      </table>" & Chr(13) & _
      "</div>" & Chr(13) & _
      "<div style='clear:both;'></div>" & Chr(13) & _
      "</div>" & Chr(13) & _
" <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printviewpaid','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

        End If
    End Function
    Function bee()
        Dim fhr As String
        If File.Exists(Session("path") & "\text\factor.txt") = False Then
            fhr = "200.67"
        Else
            fhr = File.ReadAllText(Session("path") & "\text\factor.txt")
            ' Response.Write(fhr)
        End If
        If Session("username") = "" Then
            '     Response.Redirect("logout.aspx")
        End If
        Dim keyp As String = ""
        Dim fm As New formMaker
        Dim emp_id, emptid As String
        Dim arrx() As String = {"Reg", "Nig", "WE", "HD"}

        ' Response.Write(keyp)
        Dim idx As String = ""
        idx = Request.QueryString("id")
        Dim msg As String = ""
        Dim dbx As New dbclass
        Dim sql As String = ""
        Dim flg As Object = 0
        Dim flg2 As Integer = 0
        ' Response.Write(sc.d_encryption("zewde@123"))
        Dim rd As String = ""

        Dim tbl As String = ""
        Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" And Request.QueryString("id") <> "" Then
                    Dim arrname() As String
                    sql = ""
                    If Request.QueryString.HasKeys = True Then
                        arrname = Request.QueryString("vname").Split(" ")
                        Response.Cookies("vname").Value = Request.QueryString("vname")

                        'Response.Write(arrname.Length.ToString)
                        If arrname.Length >= 3 Then

                            sql = "Select * from emp_static_info where first_name='" & arrname(0) & "' and middle_name='" & arrname(1) & "' and last_name='" & arrname(2) & "'"

                        End If
                    End If
                    Dim salary() As String
                    Dim hr As Double
                    Dim date1, date2 As Date
                    If Request.QueryString("datepaid") <> "" Then
                        date2 = Request.QueryString("datepaid")
                    End If
                    date1 = Request.QueryString("ot_date")
                    Response.Write(date1.ToShortDateString)
                    Dim dtt As DataTableReader
                    If sql <> "" Then
                        dtt = dbx.dtmake("tblstatic", sql, Session("con"))

                        If dtt.HasRows Then
                            dtt.Read()

                            emp_id = dtt.Item("emp_id")
                            Response.Write(emp_id & "<br>")
                            emptid = CInt(fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and ('" & date1 & "' between hire_date and isnull(end_date,'" & Today.ToShortDateString & "'))", Session("con")))
                            salary = dbx.getsal(emptid, date1, Session("con"))
                            'salary = dbx.getsal(emptid, session("con"))
                            hr = Math.Round(CDbl(salary(0)) / CDbl(fhr), 2)


                        End If

                        Dim amt, rate As Double

                        Dim factored As String
                        Dim timedif As Double

                        For i As Integer = 0 To arrx.Length - 1
                            If Request.QueryString(arrx(i)) <> "" Then
                                Response.Write("select rate from tblot_rate where '" & date1 & "' between date_start and isnull(date_end,'" & Today.ToShortDateString & "') and ot_abr='" & arrx(i) & "'<br>")
                                rate = fm.getinfo2("select rate from tblot_rate where '" & date1 & "' between date_start and isnull(date_end,'" & Today.ToShortDateString & "') and ot_abr='" & arrx(i) & "'", Session("con"))
                                factored = arrx(i)
                                timedif = (Request.QueryString(arrx(i)))


                                '   Select Case arrx(i)
                                '      Case "Reg"
                                ' rate = 1.5
                                'factored = "Reg"
                                'timedif = Request.QueryString(arrx(i))
                                '   Case "Nig"
                                'rate = 1.5
                                'timedif = Request.QueryString(arrx(i))
                                'factored = "Nig"
                                '   Case "WE"
                                'rate = 2
                                'timedif = Request.QueryString(arrx(i))
                                'factored = "WE"
                                '   Case "HD"
                                'rate = 2.5
                                'timedif = Request.QueryString(arrx(i))
                                'factored = "HD"
                                'End Select


                            End If
                        Next
                        ' Response.Write(amt.ToString & ",.,..,.," & hr.ToString & "===" & rate.ToString & "=====" & timedif.ToString & "<br>")

                        amt = FormatNumber(hr, 2, TriState.True, TriState.True, TriState.True) * rate * timedif
                        sql = "update emp_ot set "

                        sql &= "ot_date='" & date1.ToShortDateString & "',"

                        sql &= "time_diff='" & timedif & "'," & _
                  " rate=" & rate.ToString & "," & _
                  " factored='" & factored & "'," & _
                  " amt='" & Math.Round(amt, 2).ToString & "',"

                        If Request.QueryString("datepaid") <> "" Then
                            sql &= " datepaid='" & date2.ToShortDateString & "',"
                        End If
                        sql &= " who_reg='" & Request.QueryString("who_reg") & "'," & _
                  " date_reg='" & Request.QueryString("date_reg") & "'" & _
                  " where id=" & Request.QueryString("id")
                        'Response.Write(sql)
                        dtt.Close()



                        'Response.Write(sql)

                        flg = dbx.save(sql, Session("con"), Session("path"))
                        ' Response.Write(flg)
                        If flg = 1 Then
                            msg = "Data Saved"
                        Else
                            msg = "Data is not saved, try again. If this continue for long please contact your IT officer"
                        End If
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                    'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, Session("con"), Session("path"))

                    ' Response.Write(sql)
                    If flg = 1 Then
                        msg = "Data deleted"
                    End If
                ElseIf Request.QueryString("task") = "save" Then
                    ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")
                    Dim arrname() As String
                    sql = ""
                    If Request.QueryString.HasKeys = True Then
                        arrname = Request.QueryString("vname").Split(" ")
                        Response.Cookies("vname").Value = Request.QueryString("vname")

                        'Response.Write(arrname.Length.ToString)
                        If arrname.Length >= 3 Then

                            sql = "Select * from emp_static_info where first_name='" & arrname(0) & "' and middle_name='" & arrname(1) & "' and last_name='" & arrname(2) & "'"

                        End If
                    End If
                    Dim res As String = ""
                    Dim salary() As String
                    Dim hr As Double
                    Dim date1, date2 As Date
                    Dim factored As String
                    If Request.QueryString("datepaid") <> "" Then
                        date2 = Request.QueryString("datepaid")
                    End If
                    date1 = Request.QueryString("ot_date")

                    Dim dtt As DataTableReader
                    If sql <> "" Then
                        dtt = dbx.dtmake("tblstatic", sql, Session("con"))

                        If dtt.HasRows Then
                            dtt.Read()
                            emp_id = dtt.Item("emp_id")
                            Dim rtnvalue, projid As String
                            Dim pro() As String
                            pro = Session("pprroojj").split("|")
                            projid = pro(1)

                            rtnvalue = getprojemp(projid.ToString, date1, Session("con"))
                            ' Response.Write(rtnvalue & "<br>")
                            res = fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and id in(" & rtnvalue & ")", Session("con"))
                            If IsNumeric(res) Then
                                emptid = res
                            Else
                                emptid = fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' order by id desc", Session("con"))
                            End If
                            ' Response.Write("select basic_salary from emp_sal_info where emp_id='" & emp_id & "' and date_start<='" & date2 & "' order by id desc")

                            salary = dbx.getsal(emptid, date1, Session("con"))
                            '    Response.Write(salary(0) & "<<<<<<<<")
                            ' salary = dbx.getsal(emptid, session("con"))
                            'salary = 3800
                            If salary(0) = 0 Then

                                salary(0) = (fm.getinfo2("select basic_salary from emp_sal_info where emp_id='" & emp_id & "' and date_start<='" & date1.ToShortDateString & "'", Session("con")))
                            End If
                            ' Response.Write(salary(2) & emptid & "<br>")
                            hr = Math.Round(CDbl(salary(0)) / CDbl(fhr), 2)
                            '  Response.Write(hr.ToString & "<br>")

                        End If
                        Dim amt, rate As Double

                        'rate = CDbl(Request.QueryString("rate"))
                        Dim timedif As Double
                        ' timedif = Request.QueryString("time_diff")
                        'Dim arrx() As String = {"Reg", "Nig", "WE", "HD"}
                        dtt.Close()
                        '  amt = hr * rate * timedif
                        sql = "BEGIN TRANSACTION " & Session("username") & Chr(13)
                        For i As Integer = 0 To arrx.Length - 1
                            If Request.QueryString(arrx(i)) <> "" Then
                                ' Response.Write("select rate from tblot_rate where '" & date1 & "' between date_start and isnull(date_end,'" & Today.ToShortDateString & "') and ot_abr='" & arrx(i) & "'<br>")
                                rate = fm.getinfo2("select rate from tblot_rate where '" & date1 & "' between date_start and isnull(date_end,'" & Today.ToShortDateString & "') and ot_abr='" & arrx(i) & "'", Session("con"))
                                factored = arrx(i)
                                timedif = (Request.QueryString(arrx(i)))
                                ' factored = ""
                                ' Select Case "x"
                                '    Case "Reg"
                                'rate = 1.25
                                'factored = "Reg"
                                'timedif = Request.QueryString(arrx(i))
                                '   Case "Nig"
                                'rate = 1.5
                                'timedif = Request.QueryString(arrx(i))
                                'factored = "Nig"
                                '   Case "WE"
                                'rate = 2
                                'timedif = Request.QueryString(arrx(i))
                                'factored = "WE"
                                '   Case "HD"
                                'rate = 2.5
                                'timedif = Request.QueryString(arrx(i))
                                'factored = "HD"
                                'End Select
                                amt = Math.Round((hr * rate * timedif), 2)
                                Dim rmk As String = ""
                                If Request.QueryString("remark") <> "" Then
                                    If LCase(Request.QueryString("remark")) = "remark" Then
                                        rmk = "''"
                                    Else
                                        rmk = "'" & Request.QueryString("remark") & "'"
                                    End If

                                Else
                                    rmk = "''"
                                End If
                                If Request.QueryString("datepaid") <> "" Then

                                    sql &= "insert into emp_ot(emp_id,emptid,ot_date,time_diff,rate,factored,amt,datepaid,who_reg,date_reg,remark) " & _
                            "values('" & emp_id & "','" & emptid & "','" & date1.ToShortDateString & "','" & timedif & "','" & rate & _
                            "','" & factored & "','" & Math.Round(amt, 2).ToString & "','" & date2 & "','" & Request.QueryString("who_reg") & _
                            "','" & Request.QueryString("date_reg") & "'," & rmk & ")" & Chr(13)
                                Else
                                    sql &= "insert into emp_ot(emp_id,emptid,ot_date,time_diff,rate,factored,amt,who_reg,date_reg,remark) " & _
                            "values('" & emp_id & "','" & emptid & "','" & date1.ToShortDateString & "','" & timedif & "','" & rate & _
                            "','" & factored & "','" & Math.Round(amt, 2).ToString & "','" & Request.QueryString("who_reg") & _
                            "','" & Request.QueryString("date_reg") & "'," & rmk & ")" & Chr(13)
                                End If
                                'Response.Write(sql & "<br>")
                            End If
                        Next
                        sql &= "COMMIT TRANSACTION " & Session("username") & Chr(13)
                        ' Response.Write(sql)
                        Try
                            flg = dbx.excutes(sql, Session("con"), Session("path"))

                        Catch ex As Exception
                            Response.Write(ex.ToString & "<br>" & sql)
                            flg = 0
                        End Try

                        'Response.Write(flg1)
                        'flg = 1
                        'flg1 = ds.save("commit", session("con"))
                        If IsNumeric(flg) = True Then
                            ' Response.Write(flg1)
                            If flg <= 0 Then
                                dbx.save("rollback TRANSACTION " & Session("username"), Session("con"), Session("path"))
                                Response.Write("data is not saved")
                            Else
                                Response.Write("Data Saved")
                            End If

                        Else
                            If flg.ToString <> "-1" Then
                                dbx.save("rollback TRANSACTION " & Session("username"), Session("con"), Session("path"))
                                Response.Write("data is not saved")
                            Else
                                Response.Write("Data(s) is/are saved")
                            End If

                        End If


                        '  flg = dbx.save(sql, session("con"),session("path"))
                        ' Response.Write(flg)
                        If IsNumeric(flg) = True Then
                            msg = "Data Saved"
                        Else
                            msg = "Data is not saved, try again. If this continue for long please contact your IT officer"
                        End If
                    End If

                    'MsgBox(rd)

                    ' sql = db.makest(tbl, Request.QueryString, session("con"), key)

                End If



            End If
        End If


        Dim namelist As String = ""
        namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")

        'Response.Write(Session("fullempname"))

        Dim sc As New k_security
        ' Response.Write(sc.d_encryption("zewde@123"))
        If Request.Form.HasKeys = True Then
            'Dim db As New dbclass
            ' Dim sql As String
            ' sql = db.makest("tblworkexp", Request.Form, session("con"), "")
            'Response.Write(sql)
            'db.save(sql, session("con"),session("path"))
        End If
        For Each p As String In Request.Form
            'Response.Write(" <br />" & p & "=>" & Request.Form(p))

        Next
        For Each k As String In Request.ServerVariables
            ' Response.Write("<br />" & k & "=>" & Request.ServerVariables(k))
        Next
        'Response.Write("<br />" & Request.Form("do"))

    End Function
    Public Function edit_del_list_wname_ot(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        Dim arr() As String = {""}
        'Response.Write(sql)
        rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        If dt.HasRows = True Then
            Dim color As String = "E3EAEB"
            Dim empid As String
            Dim datex As Boolean = False
            rtstr &= "<form id='frms' method='post' name='frms' action=''>"
            While dt.Read
                If fm.searcharray(arr, sec.Str2ToHex(dt.Item("ot_date"))) = False Then
                    Dim arrbound As Integer = UBound(arr)
                    ReDim Preserve arr(arrbound + 1)
                    arr(arrbound) = sec.Str2ToHex(dt.Item("ot_date"))

                    If datex = True Then
                        rtstr &= "</table></div>" & Chr(13)
                        datex = False
                    End If
                    If datex = False Then
                        rtstr &= "<br><div id='" & sec.Str2ToHex(dt.Item("ot_date")) & "' class='collapsed' style='height:25px;width:900px; background-color:blue; cursor:pointer;color:white; font-weight:bold;' onclick=" & Chr(34) & "javascript:showHideSubMenu(this,'tbl" & _
                                                  sec.Str2ToHex(dt.Item("ot_date")) & "')" & Chr(34) & _
                                                  "><span style='float:left'>" & dt.Item("ot_date") & "</span><img src='images/gif/collapsed_.gif' style='float:right;top:10px;'></div>" & Chr(13) & "<div id='tbl" & sec.Str2ToHex(dt.Item("ot_date")) & "' style='display:none;'><table  cellspacing='0' cellpadding='0'>" & _
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
                        rtstr = rtstr & "</tr>" & Chr(13)
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
                rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "(" & empid & ")</td>"
                For k As Integer = 1 To dt.FieldCount - 2
                    If dt.Item(k).ToString = "y" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>Yes</td>"
                    ElseIf dt.Item(k).ToString = "n" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>No</td>"
                    ElseIf dt.GetName(k) = "department" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                        fm.getinfo2("select dep_name from tbldepartment where dep_id='" & _
                        dt.Item(k).ToString & "'", con) & "</td>"
                    ElseIf dt.GetName(k) = "project_id" Then
                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & _
                        fm.getinfo2("select project_name from tblproject where project_id='" & _
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
    Function updatedb(ByVal pproj As String, ByVal pdate1 As Date, ByVal pdate2 As Date)
        Dim fhr As String
        If File.Exists(Session("path") & "\text\factor.txt") = False Then
            fhr = "200.67"
        Else
            fhr = File.ReadAllText(Session("path") & "\text\factor.txt")
            ' Response.Write(fhr)
        End If
        Dim dbx As New dbclass
        Dim rs As DataTableReader
        Dim spl() As String
        Dim projid As String = ""
        Dim nod As Integer
        Dim sqlx As String
        If pproj <> "" Then 'Then
            spl = pproj.Split("|")
            projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
        End If



        rs = dbx.dtmake("selc_rec", "select * from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "'", Session("con"))
        ' Response.Write("select * from emprec where end_date is null and id in(select emptid from emp_job_assign project_id='" & projid & "')<br>")
        If rs.HasRows = True Then
            Dim emptid As String = ""
            Dim sal() As String
            Dim hr As Double
            Dim rate As Double
            Dim timedif As Double
            Dim oldamt, amt As Double

            While rs.Read
                emptid = rs.Item("emptid")
                sal = dbx.getsal(emptid, pdate1, Session("con"))
                hr = Math.Round(sal(0) / CDbl(fhr), 2)
                rate = rs.Item("rate")
                timedif = rs.Item("time_diff")
                oldamt = rs.Item("amt")

                amt = FormatNumber((FormatNumber(hr, 2) * rate * timedif), 2)
                If oldamt <> amt Then
                    sqlx = "Update emp_ot set amt='" & amt & "' where id=" & rs.Item("id")
                    ' Response.Write(sqlx & "<br>")
                    dbx.excutes(sqlx, Session("con"), Session("path"))
                End If

            End While
        End If
        rs.Close()
        dbx = Nothing

    End Function
    Function updatedb_byemp(ByVal empid As String, ByVal pdate1 As Date, ByVal pdate2 As Date)
        Dim fhr As String
        If File.Exists(Session("path") & "\text\factor.txt") = False Then
            fhr = "200.67"
        Else
            fhr = File.ReadAllText(Session("path") & "\text\factor.txt")
            ' Response.Write(fhr)
        End If
        Dim dbx As New dbclass
        Dim rs As DataTableReader
        Dim spl() As String
        Dim projid As String = ""
        Dim nod As Integer
        Dim sqlx As String




        rs = dbx.dtmake("selc_rec", "select * from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid in (" & empid & ")", Session("con"))
        ' Response.Write("select * from emprec where end_date is null and id in(select emptid from emp_job_assign project_id='" & projid & "')<br>")
        If rs.HasRows = True Then
            Dim emptid As String = ""
            Dim sal() As String
            Dim hr As Double
            Dim rate As Double
            Dim timedif As Double
            Dim oldamt, amt As Double

            While rs.Read
                emptid = rs.Item("emptid")
                sal = dbx.getsal(emptid, pdate1, Session("con"))
                hr = Math.Round(sal(0) / CDbl(fhr), 2)
                rate = rs.Item("rate")
                timedif = rs.Item("time_diff")
                oldamt = rs.Item("amt")

                amt = FormatNumber((FormatNumber(hr, 2) * rate * timedif), 2)
                If oldamt <> amt Then
                    sqlx = "Update emp_ot set amt='" & amt & "' where id=" & rs.Item("id")
                    ' Response.Write(sqlx & "<br>")
                    dbx.excutes(sqlx, Session("con"), Session("path"))
                End If

            End While
        End If
        rs.Close()
        dbx = Nothing

    End Function
    Function paidlistp(ByVal pproj As String, ByVal pdate1 As Date, ByVal pdate2 As Date) As Nullable
        ' Response.Write(pproj)
        Dim fhr As String
        If File.Exists(Session("path") & "\text\factor.txt") = False Then
            fhr = "200.67"
        Else
            fhr = File.ReadAllText(Session("path") & "\text\factor.txt")
            ' Response.Write(fhr)
        End If
        If pproj <> "" Then
            Dim fm As New formMaker
            ' Dim pa As otview
            Dim reg, sun, nig, hd As String
            Dim sumreg, sumsun, sumnig, sumhd, sal(), otbir, hr, shr, fhrs As String
            Dim i As Integer = 1
            otbir = "0"
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim spl() As String
            Dim projid As String = ""
            Dim nod As Integer


            spl = pproj.Split("|")
            projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))



            Dim rs2 As DataTableReader
            Dim rtn2 As String
            If projid = "" Then
                rs2 = dbx.dtmake("selc_rec", "select * from emprec ", Session("con"))
            Else
                'rs2 = dbx.dtmake('selc_rec', 'select * from emprec where end_date is null', Session('con'))

                'rs2 = dbx.dtmake("selc_rec", "select * from emprec inner join emp_static_info as esi on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))) order by esi.first_name", Session("con"))
                'Response.Write("select * from emprec inner join emp_static_info as esi on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')) order by esi.first_name")
                Dim rtnvalue, rssql, rssqlnew As String
                ' rtnvalue = fm.getprojemp(projid.ToString, pdate2, Session("con"))
                rtnvalue = serch_get_paid(pdate2, projid)
                '  rssql = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                  "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                 "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                "and emprec.id in(select emptid from emp_job_assign " & _
                '                                               "where project_id='" & projid.ToString & "' " & _
                '                                              "and ('" & pdate2 & "' >= date_from and '" & pdate2 & "' <= isnull(date_end,'" & Today.ToShortDateString & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')  or (month(date_end)=" & pdate2.Month & " and year(date_end)=" & pdate1.Year & ")))" & _
                '                                             " ORDER BY emp_static_info.first_name,emprec.id desc "

                rssqlnew = "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                                                                  "where emprec.id in(" & rtnvalue & ")" & _
                                                                  " ORDER BY emp_static_info.first_name,emprec.id desc "
                'Response.Write(rssqlnew & "<br>")
                ' Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id " & _
                '                                                                    "where ('" & pdate1 & "'  between  emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & _
                '                                                                   "') or emprec.hire_date between '" & pdate1 & "' and isnull(emprec.end_date,'" & pdate2 & "' )) " & _
                '                                                                  "and emprec.id in(" & rtnvalue & ") ORDER BY emp_static_info.first_name,emprec.id desc ")
                'Session("payrolllist") = rssqlnew

                Try
                    rs2 = dbx.dtmake("selectemp", rssqlnew, Session("con"))
                Catch ex As Exception
                    Response.Write(rssqlnew.ToString)
                End Try

            End If

            Me.outp3.Text &= "<div id='printviewpaidin'>" & _
            "  <style> " & Chr(13) & _
                ".tb1" & Chr(13) & _
            "{" & Chr(13) & _
               " border:1px solid black;" & Chr(13) & _
               " font-size:10pt;" & Chr(13) & _
            "}" & Chr(13) & _
            ".tb1 td" & Chr(13) & _
            "{" & _
             "border-top: 1px solid black;" & Chr(13) & _
             "border-left:1px solid black;" & Chr(13) & _
               " font-size:9pt;" & Chr(13) & _
            "}" & Chr(13) & _
    "          </style>" & Chr(13) & _
    " <table class='tb1' width='730px' cellpadding='0' cellspacing='0'>" & Chr(13) & _
    "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
              " <td style='text-align:center;font-weight:bold;font-size:13pt' colspan='12' >" & Session("company_name") & _
               "<br /> Project Name:"

            If projid <> "" Then
                Me.outp3.Text &= (spl(0).ToString)
            Else
                Me.outp3.Text &= ("All Projects")
            End If
            Me.outp3.Text &= "<br /> Overtime Paid on month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</td>" & _
            "<br><span style='color:red;'>Paid List in Selected month</span>" & _
    "              </tr>" & Chr(13) & _
    "<tr style=' text-align:center;'>" & Chr(13) & _
    " <td width='20' rowspan='2'><span class='headtxt'><strong>&nbsp;No</strong></span></td>" & Chr(13) & _
    "<td width='150' rowspan='2'><span class='headtxt'><strong>&nbsp;Employee Name</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;Basic Salary</strong></span></td>" & Chr(13) & _
    "    <td colspan='4'><span class='headtxt'><strong>&nbsp;Overtime Hours</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;H.Rate</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;F.Hrs **</strong></span></td>" & Chr(13) & _
    "<td width='79' rowspan='2'><span class='headtxt'><strong>&nbsp;OT. Birr</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;OT Date</strong></span></td>" & Chr(13) & _
     "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;Referance</strong></span></td>" & Chr(13) & _
    "</tr>" & Chr(13) & _
    "<tr>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Reg.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Nig.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Sun.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;H.D</strong></span></td>" & Chr(13) & _
    "</tr>" & Chr(13)
            If rs2.HasRows Then

                otbir = "0"
                Dim ot_date As Date
                Dim otamt As String

                Dim rt() As String
                While rs2.Read

                    ' Response.Write("select * from emp_ot  where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref is not null order by datepaid")
                    rs = dbx.dtmake("ot", "select * from emp_ot  where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref is not null order by datepaid", Session("con"))
                        '  Response.Write("<br>" & otamt.ToString)
                    Dim ref As String = ""
                   

                    Dim sumamt As Double
                    Dim cname As Integer = 0
                    If rs.HasRows Then
                        While rs.Read
                            If ref <> rs.Item("ref") Then

                                ref = rs.Item("ref")
                                otamt = fm.getinfo2("select sum(amt) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref='" & ref & "'", Session("con"))
                                If IsNumeric(otamt) = False Then
                                    otamt = "0"

                                End If
                                If CDbl(otamt) > 0 Then
                                    cname += 1
                                    sumamt += CDbl(otamt)
                                    reg = ""
                                    sun = ""
                                    nig = ""
                                    hd = ""
                                    ot_date = rs.Item("ot_date")
                                    Me.outp3.Text &= "  <tr>" & Chr(13) & _
                                    " <td>&nbsp;" & i.ToString & "</td>" & Chr(13) & _
                                     "<td>&nbsp;" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & " </td>" & Chr(13) & _
               " <td style='text-align:right'>&nbsp;" & Chr(13)
                                    sal = dbx.getsal(rs2.Item("id"), ot_date, Session("con"))
                                    shr = Math.Round((CDbl(sal(0)) / CDbl(fhr)), 2).ToString
                                    Me.outp3.Text &= (FormatNumber(sal(0), 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"

                                    Me.outp3.Text &= "<td>&nbsp;"
                                    reg = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y' and ref ='" & ref & "'", Session("con"))
                                    If reg.ToString <> "" And reg.ToString <> "None" Then
                                        Me.outp3.Text &= (reg)
                                        hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y'  and ref ='" & ref & "'", Session("con"))
                                        If hr = "None" Then
                                            hr = "1"
                                        End If
                                        'Response.Write(reg & hr & 'reg<br>')
                                        sumreg = (CDbl(reg) * CDbl(hr)).ToString
                                    Else
                                        sumreg = "0"
                                    End If
                                    Me.outp3.Text &= "</td>"
                                    Me.outp3.Text &= " <td>&nbsp;"
                                    nig = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig' and time_diff is Not Null  and paidstatus='y'  and ref ='" & ref & "'", Session("con"))
                                    If nig.ToString <> "" And nig <> "None" Then
                                        Me.outp3.Text &= nig
                                        hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig'  and paidstatus='y' and ref ='" & ref & "'", Session("con"))
                                        If hr = "None" Then
                                            hr = "1"

                                        End If
                                        ' Response.Write(nig.ToString & hr & 'nig<br>')
                                        sumnig = (CDbl(nig) * CDbl(hr)).ToString
                                    Else
                                        sumnig = "0"
                                    End If

                                    Me.outp3.Text &= "</td>"
                                    Me.outp3.Text &= " <td>&nbsp;"
                                    sun = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y' and ref ='" & ref & "'", Session("con"))

                                    If sun.ToString <> "" And sun <> "None" Then
                                        Me.outp3.Text &= sun
                                        hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y' and ref='" & ref & "'", Session("con"))
                                        If hr = "None" Then
                                            hr = "1"

                                        End If
                                        'Response.Write(sun & hr & 'sun<br>')
                                        sumsun = (CDbl(sun) * CDbl(hr)).ToString
                                    Else
                                        sumsun = "0"
                                    End If

                                    Me.outp3.Text &= "</td>"
                                    Me.outp3.Text &= "<td>&nbsp;"
                                    hd = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y' and ref='" & ref & "'", Session("con"))

                                    If hd.ToString <> "" And hd <> "None" Then
                                        Me.outp3.Text &= (hd)
                                        hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y' and ref ='" & ref & "'", Session("con"))
                                        If hr = "None" Then
                                            hr = "1"
                                        End If
                                        'Response.Write(hd & hr & 'hd<br>')
                                        sumhd = (CDbl(hd) * CDbl(hr)).ToString
                                    Else
                                        sumhd = "0"
                                    End If
                                    Me.outp3.Text &= "</td>"
                                    Me.outp3.Text &= "<td style='text-align:right;'>&nbsp;"
                                    Me.outp3.Text &= (FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"
                                    Me.outp3.Text &= "<td style='text-align:right;'>&nbsp;"
                                    fhrs = (CDbl(sumsun) + CDbl(sumreg) + CDbl(sumhd) + CDbl(sumnig)).ToString
                                    Me.outp3.Text &= (FormatNumber(fhrs, 2, TriState.True, TriState.True, TriState.True).ToString) & " </td>"
                                    Me.outp3.Text &= "<td style='text-align:right;'>&nbsp;"
                                    otbir = (Math.Round(CDbl(otbir), 2) + otamt).ToString
                                    Me.outp3.Text &= (FormatNumber(otamt, 2, TriState.True, TriState.True, TriState.True).ToString)
                                    Me.outp3.Text &= "  </td>"
                                    Me.outp3.Text &= "<td>" & rs.Item("ot_date") & "</td><td>" & ref & "</td>"
                                    Me.outp3.Text &= "  </tr>"

                                    i += 1
                                End If
                               
                            End If
                        End While
                        If cname > 1 Then
                            Me.outp3.Text &= "<tr style=' font-style:italic;'><td colspan='9'>Summery Sum</td><td  style='text-align:right;'>" & FormatNumber(sumamt.ToString, 2, TriState.True, TriState.True, TriState.True) & "</td><td>&nbsp</td><td>&nbsp</td></tr>"
                        Else
                            Me.outp3.Text &= "<tr style=' font-style:italic;font-weight:bold;height:10px;'><td colspan='12' style='font-size:2pt;height:10px;'>&nbsp;</td></tr>"
                        End If
                        sumamt = 0
                    End If

                        rs.Close()
                        ' Response.Write("<br>" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & "=" & rt(0))

                End While
            End If
            Me.outp3.Text &= " <tr style='font-style:italic;font-weight:bold'><td colspan='9'>&nbsp;Total</td><td style='text-align:right;'>"
            Me.outp3.Text &= (FormatNumber(otbir, 2, TriState.True, TriState.True, TriState.True)) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
            Me.outp3.Text &= "</table>"
            Me.outp3.Text &= "<div style='font-size:10pt;'> *Hourly Rate=Basic Salary/" & fhr & "<br />"

            Dim inforate As String = "**Factored Hours: "
            Dim irn(), ira(), rrate As String
            Dim rrs As DataTableReader
            i = 0
            rrs = dbx.dtmake("otratet", "select distinct ot_abr, ot_name from tblot_rate", Session("con"))
            If rrs.HasRows Then
                While rrs.Read
                    ReDim Preserve irn(i + 1)
                    ReDim Preserve ira(i + 1)
                    irn(i) = rrs.Item("ot_name")
                    ira(i) = rrs.Item("ot_abr")
                    i = i + 1
                End While
            End If

            rrs.Close()
            rrs = Nothing
            i = 0
            If irn.Length > 0 Then
                For i = 0 To UBound(irn) - 1
                    rrate = fm.getinfo2("select rate from tblot_rate where '" & pdate1 & "' between date_start and isnull(date_end,'" & Today.ToShortDateString & "') and ot_abr='" & ira(i) & "'", Session("con"))
                    inforate &= ira(i) & "(" & irn(i) & ")" & " X " & rrate
                    If i < UBound(ira) - 1 Then
                        inforate &= " + "
                    End If

                Next
            End If

            Me.outp3.Text &= "Where " & fhr & " is Average Normal Working Hours in a month<br /> " & _
            inforate & _
      " </div>" & Chr(13) & _
     " <div style='height:15px;'></div>" & _
      "<div style='float:left; width:196px; font-size:9pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>Paid By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>      " & Chr(13) & _
      "</table>" & Chr(13) & _
     " </div>" & Chr(13) & _
      "<div style='float:left; width:4px;'></div>" & Chr(13) & _
      "<div style='float:left; width:196px;font-size:10pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>Prepared By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
    "      </table>" & Chr(13) & _
      "</div>" & Chr(13) & _
       "<div style='float:left; width:4px;'></div>" & Chr(13) & _
      "<div style='float:left; width:196px;font-size:10pt;'>" & Chr(13) & _
      "<table cellpadding='0' cellspacing='0' style='padding-bottom:3px;' border='0' width='196px'><tr>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt; font-size:9pt;'>Chekced By</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr ><td style= 'padding-bottom:13px;font-size:9pt;'>Signature</td>" & Chr(13) & _
      "<td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
      "<tr><td style= 'padding-bottom:13px;font-size:9pt;'>Date</td><td style= 'padding-bottom:13px;font-size:9pt;'>________________</td></tr>" & Chr(13) & _
    "      </table>" & Chr(13) & _
      "</div>" & Chr(13) & _
      "<div style='clear:both;'></div>" & Chr(13) & _
      "</div>" & Chr(13) & _
    " <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printviewpaidin','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

        End If
    End Function

    Public Function getprojemp(ByVal projid As String, ByVal sdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim did As String
       
        rs = dbs.dtmake("listemp", "select emptid,emp_id,date_from,date_end from emp_job_assign where project_id='" & projid & "' order by emp_id,emptid desc", con)
        Dim d1, d2, de, ds As Date
        d1 = Nothing
        d2 = Nothing

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
                    '                   Response.Write("  ====         " & rs.Item("emptid") & fm.getfullname(rs.Item("emp_id"), Session("con")) & "<br>")
                    If ds <= sdate And sdate <= de Then
                        rtn &= "'" & rs.Item("emptid") & "',"

                    Else

                    End If


                End While
            Catch ex As Exception
                '              Response.Write(ex.ToString)
                rtn = "'',"
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
    Function serch_get_paid(ByVal pdate As Date, ByVal projid As String)
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rtnvalue As String = ""
        Dim ddate As Date
        Dim rs, r2 As DataTableReader
        Dim arr1(), arr2(1) As String
        Dim oldsize As Integer = 0
        arr2(0) = ""
        rs = dbs.dtmake("stpayroll", "select * from payrollx where month(pddate)='" & pdate.Month & "' and year(pddate)='" & pdate.Year & "' and ot>0", Session("con"))
        If rs.HasRows Then
            While rs.Read
                ddate = "#1/1/1900#"
                ddate = rs.Item("date_paid")
                If ddate <> "#1/1/1900#" Then
                    arr1 = fm.getprojemp(projid.ToString, ddate, Session("con")).split(",")
                    If arr1.Length <> oldsize Then

                        oldsize = arr1.Length

                    End If
                    For i As Integer = 0 To oldsize - 1

                        If fm.searcharray(arr2, arr1(i)) = False Then
                            If arr1(i) <> "" Then
                                arr2(arr2.Length - 1) = arr1(i)
                                ReDim Preserve arr2(arr2.Length)
                            End If
                        End If

                    Next
                End If

            End While
        End If
        For i As Integer = 0 To arr2.Length - 1
            If arr2(i) <> "" Then
                rtnvalue &= (arr2(i) & ",")
            End If
        Next
        If rtnvalue.Length > 1 Then
            Return rtnvalue.Substring(0, rtnvalue.Length - 1)
        Else
            Return "0"

        End If


    End Function

    Protected Sub ot3_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rs As DataTableReader
        Dim dbs As New dbclass
        Dim mails As New mail_system
        Dim fm As New formMaker
        Dim msg As String = ""
       

        Dim tose As String = ""
        Dim fx() As String
        If String.IsNullOrEmpty(Session("right")) = False Then
            fx = Session("right").split(";")
            ReDim Preserve fx(UBound(fx) + 1)
            fx(UBound(fx) - 1) = ""
        End If
        If fm.searcharray(fx, "1") = False Or fm.searcharray(fx, "9") = False Then
            rs = dbs.dtmake("vwnell", "select id,emptid,emp_id,time_diff,ot_date from emp_ot where time_diff is null", Session("con"))
            If rs.HasRows Then
                Response.Write("Error on data input ot on:<br>")
                While rs.Read

                    Response.Write("Please re insert the last data!" & rs.Item(0) & "===" & rs.Item(1) & "=====" & rs.Item(2) & "<br>")
                    msg &= "Please re insert the last data!" & rs.Item(0) & "===" & rs.Item(1) & "=====" & rs.Item(2) & "=>" & fm.getfullname(rs.Item(2), Session("con")) & Chr(13) & " < br >"
                    ' dbs.excutes("delete from emp_ot where id=" & rs.Item(0), Session("con"), Session("path"))

                End While
            Else
            End If
            tose = fm.getinfo2("select pemail from emp_address where emp_id='" & Session("emp_iid") & "'", Session("con"))



            If tose <> "" And tose <> "None" Then
                tose &= ",z.kirubel@gmail.com"
            Else
                tose &= "z.kirubel@gmail.com"
            End If

            mails.sendemail(msg, Session("epwd"), Session("efrom"), Session("eto"), Session("company_name") & "OT Reg. Error", Session("smtp"), Session("eport"))
        End If
    End Sub
End Class
