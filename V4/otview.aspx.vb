Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class otview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Form("paidlist") = "list paid" Then
            paidlistp()
            'paidpp()
            ' paidlist_in_month_ref()
        Else


            If Request.Form("month") <> "" Then
                Dim fm As New formMaker

                Dim reg, sun, nig, hd As String
                Dim sumreg, sumsun, sumnig, sumhd, sal(2), otbir, hr, shr, fhrs As String
                Dim i As Integer = 1
                otbir = "0"
                Dim dbx As New dbclass
                Dim rs As DataTableReader
                Dim spl() As String
                Dim projid As String = ""
                Dim nod As Integer
                Dim pdate1, pdate2 As Date
                If Request.Form("projname") <> "" Then 'Then
                    spl = Request.Form("projname").Split("|")
                    projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
                End If

                nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
                pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
                pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
                Dim rs2 As DataTableReader
                If projid = "" Then
                    rs2 = dbx.dtmake("selc_rec", "select * from emprec ", Session("con"))
                Else
                    'rs2 = dbx.dtmake('selc_rec', 'select * from emprec where end_date is null', Session('con'))

                    rs2 = dbx.dtmake("selc_rec", "select * from emprec where id in(select emptid from emp_job_assign where project_id=" & projid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between  '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "')))", Session("con"))
                    ' Response.Write('select * from emprec where end_date is null and id in(select emptid from emp_job_assign project_id='' & projid & '')')
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
    " <table class='tb1' width='600px' cellpadding='0' cellspacing='0'>" & Chr(13) & _
     "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
                  " <td style='text-align:center;font-weight:bold;font-size:13pt' colspan='10' >" & Session("company_name") & _
                   "<br /> Project Name:"
                If projid <> "" Then
                    Me.outp.Text &= (spl(0).ToString)
                Else
                    Me.outp.Text &= ("All Projects")
                End If
                Me.outp.Text &= "<br /> Overtime Payment Sheet for the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</td>" & _
    "              </tr>" & Chr(13) & _
    "<tr style=' text-align:center;'>" & Chr(13) & _
       " <td width='20' rowspan='2'><span class='headtxt'><strong>&nbsp;No</strong></span></td>" & Chr(13) & _
        "<td width='120' rowspan='2'><span class='headtxt'><strong>&nbsp;Employee Name</strong></span></td>" & Chr(13) & _
        "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;Basic Salary</strong></span></td>" & Chr(13) & _
    "    <td colspan='4'><span class='headtxt'><strong>&nbsp;Overtime Hours</strong></span></td>" & Chr(13) & _
        "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;H.Rate</strong></span></td>" & Chr(13) & _
        "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;F.Hrs **</strong></span></td>" & Chr(13) & _
        "<td width='79' rowspan='2'><span class='headtxt'><strong>&nbsp;OT. Birr</strong></span></td>" & Chr(13) & _
      "</tr>" & Chr(13) & _
      "<tr>" & Chr(13) & _
        "<td width='30'><span class='headtxt'><strong>&nbsp;Reg.</strong></span></td>" & Chr(13) & _
        "<td width='30'><span class='headtxt'><strong>&nbsp;Nig.</strong></span></td>" & Chr(13) & _
        "<td width='30'><span class='headtxt'><strong>&nbsp;Sun.</strong></span></td>" & Chr(13) & _
        "<td width='30'><span class='headtxt'><strong>&nbsp;H.D</strong></span></td>" & Chr(13) & _
      "</tr>" & Chr(13)
                If rs2.HasRows Then
                    otbir = "0"
                    While rs2.Read
                        rs = dbx.dtmake("otx", "select * from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='n'", Session("con"))


                        If rs.HasRows Then
                            rs.Read()
                            Dim wd As Date
                            reg = ""
                            sun = ""
                            nig = ""
                            hd = ""
                            sumreg = ""
                            sumnig = ""
                            sumhd = ""
                            sumsun = ""
                            Me.outp.Text &= "  <tr>" & Chr(13) & _
                            " <td>&nbsp;" & i.ToString & "</td>" & Chr(13) & _
                             "<td>&nbsp;" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & " </td>" & Chr(13) & _
       " <td style='text-align:right'>&nbsp;" & Chr(13)
                            If rs.IsDBNull(11) = False Then
                                wd = rs.Item("ot_date")
                            End If
                            sal = dbx.getsal(rs2.Item("id"), wd.ToShortDateString, Session("con"))
                            shr = (CDbl(sal(0)) / 200.67).ToString
                            Me.outp.Text &= (FormatNumber(sal(0), 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"

                            Me.outp.Text &= "<td>&nbsp;"
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
                            Me.outp.Text &= " <td>&nbsp;"
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
                            Me.outp.Text &= " <td>&nbsp;"
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
                            Me.outp.Text &= "<td>&nbsp;"
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
                            Me.outp.Text &= "<td style='text-align:right;'>&nbsp;"
                            Me.outp.Text &= (FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"
                            Me.outp.Text &= "<td style='text-align:right;'>&nbsp;"

                            fhrs = (CDbl(sumsun) + CDbl(sumreg) + CDbl(sumhd) + CDbl(sumnig)).ToString
                            ' Response.Write(fhrs & "=" & sumsun & " + " & (sumreg) & " + " & (sumhd) & " + " & (sumnig) & "<br>")
                            Me.outp.Text &= (FormatNumber(fhrs, 2, TriState.True, TriState.True, TriState.True).ToString) & " </td>"
                            Me.outp.Text &= "<td style='text-align:right;'>&nbsp;"
                            otbir = (Math.Round(CDbl(otbir), 2) + (Math.Round(CDbl(fhrs) * CDbl(shr), 2))).ToString
                            Me.outp.Text &= (FormatNumber((CDbl(fhrs) * CDbl(shr)).ToString, 2, TriState.True, TriState.True, TriState.True).ToString)
                            If rs.IsDBNull(3) = True Then
                                Me.outp.Text &= "."
                            End If
                            Me.outp.Text &= "  </td>"
                            Me.outp.Text &= "  </tr>"
                            i += 1
                        End If

                    End While
                End If
                Me.outp.Text &= " <tr><td>&nbsp;</td><td>&nbsp;Total</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
                "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;'>"
                Me.outp.Text &= (FormatNumber(otbir, 2, TriState.True, TriState.True, TriState.True)) & "</td></tr>"
                Me.outp.Text &= "</table>"
                Me.outp.Text &= "<div style='font-size:10pt;'> *Hourly Rate=Basic Salary/200.67<br />"
                Me.outp.Text &= "Where 200.67 is Average Normal Working Hours in a month<br /> " & _
                "**Factored Hours: 1.25 X Reg Hours +2 X Sunday(Weekends) + 2.5 X Public Holiday +  1.5 X Night Hours." & _
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
    " <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printview','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

            End If
            paidpp()
        End If
    End Sub
    Function paidpp() As Nullable
        'Response.Write("paidpp()...")
        If Request.Form("month") <> "" Then
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
            Dim pdate1, pdate2 As Date
            If Request.Form("projname") <> "" Then 'Then
                spl = Request.Form("projname").Split("|")
                projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
            End If

            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            Dim rs2 As DataTableReader
            If projid = "" Then
                rs2 = dbx.dtmake("selc_rec", "select * from emprec where end_date is null", Session("con"))
            Else
                'rs2 = dbx.dtmake('selc_rec', 'select * from emprec where end_date is null', Session('con'))

                rs2 = dbx.dtmake("selc_rec", "select * from emprec inner join emp_static_info as esi  on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "') )) order by esi.first_name", Session("con"))

                ' Response.Write("select * from emprec inner join emp_static_info as esi  on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')) order by esi.first_name")
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
            Me.outp2.Text &= "<br /> Overtime Payment Sheet for the month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</td>" & _
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
            If rs2.HasRows Then
                otbir = "0"
                While rs2.Read
                    rs = dbx.dtmake("ot", "select * from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' order by ot_date", Session("con"))


                    If rs.HasRows Then
                        rs.Read()
                        reg = ""
                        sun = ""
                        nig = ""
                        hd = ""

                        Me.outp2.Text &= "  <tr>" & Chr(13) & _
                        " <td>&nbsp;" & i.ToString & "</td>" & Chr(13) & _
                         "<td>&nbsp;" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & " </td>" & Chr(13) & _
   " <td style='text-align:right'>&nbsp;" & Chr(13)
                        sal = dbx.getsal(rs2.Item("id"), pdate2, Session("con"))
                        shr = (CDbl(sal(0)) / 200.67).ToString
                        Me.outp2.Text &= (FormatNumber(sal(0), 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"

                        Me.outp2.Text &= "<td>&nbsp;"
                        reg = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y'", Session("con"))
                        If reg.ToString <> "" And reg.ToString <> "None" Then
                            Me.outp2.Text &= (reg)
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y'", Session("con"))
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
                        nig = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig' and time_diff is Not Null  and paidstatus='y'", Session("con"))
                        If nig.ToString <> "" And nig <> "None" Then
                            Me.outp2.Text &= nig
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig'  and paidstatus='y'", Session("con"))
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
                        sun = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y'", Session("con"))

                        If sun.ToString <> "" And sun <> "None" Then
                            Me.outp2.Text &= sun
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y'", Session("con"))
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
                        hd = fm.getinfo2("select sum(time_diff) from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y'", Session("con"))

                        If hd.ToString <> "" And hd <> "None" Then
                            Me.outp2.Text &= (hd)
                            hr = fm.getinfo2("select rate from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y'", Session("con"))
                            If hr = "None" Then
                                hr = "1"
                            End If
                            'Response.Write(hd & hr & 'hd<br>')
                            sumhd = (CDbl(hd) * CDbl(hr)).ToString
                        Else
                            sumhd = "0"
                        End If
                        Me.outp2.Text &= "</td>"
                        Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                        Me.outp2.Text &= (FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"
                        Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                        fhrs = (CDbl(sumsun) + CDbl(sumreg) + CDbl(sumhd) + CDbl(sumnig)).ToString
                        Me.outp2.Text &= (FormatNumber(fhrs, 2, TriState.True, TriState.True, TriState.True).ToString) & " </td>"
                        Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                        otbir = (Math.Round(CDbl(otbir), 2) + (Math.Round(CDbl(fhrs) * CDbl(shr), 2))).ToString
                        Me.outp2.Text &= (FormatNumber((CDbl(fhrs) * CDbl(shr)).ToString, 2, TriState.True, TriState.True, TriState.True).ToString)
                        Me.outp2.Text &= "  </td>"
                        Me.outp2.Text &= "<td>" & rs.Item("datepaid") & "</td>"
                        Me.outp2.Text &= "  </tr>"
                        i += 1
                    End If
                    rs.Close()
                End While
            End If
            Me.outp2.Text &= " <tr><td>&nbsp;</td><td>&nbsp;Total</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
            "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;'>"
            Me.outp2.Text &= (FormatNumber(otbir, 2, TriState.True, TriState.True, TriState.True)) & "</td><td>&nbsp;</td></tr>"
            Me.outp2.Text &= "</table>"
            Me.outp2.Text &= "<div style='font-size:10pt;'> *Hourly Rate=Basic Salary/200.67<br />"
            Me.outp2.Text &= "Where 200.67 is Average Normal Working Hours in a month<br /> " & _
            "**Factored Hours: 1.25 X Reg Hours +2 X Sunday(Weekends) + 2.5 X Public Holiday +  1.5 X Night Hours." & _
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
" <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printviewpaid','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

        End If

    End Function

    Function paidlistp() As Nullable
        If Request.Form("month") <> "" Then
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
            Dim pdate1, pdate2 As Date
            If Request.Form("projname") <> "" Then 'Then
                spl = Request.Form("projname").Split("|")
                projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
            End If

            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            Dim rs2 As DataTableReader
            If projid = "" Then
                rs2 = dbx.dtmake("selc_rec", "select * from emprec ", Session("con"))
            Else
                'rs2 = dbx.dtmake('selc_rec', 'select * from emprec where end_date is null', Session('con'))

                rs2 = dbx.dtmake("selc_rec", "select * from emprec inner join emp_static_info as esi on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and ('" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "') or date_from between '" & pdate1 & "' and isnull(date_end,'" & pdate2 & "'))) order by esi.first_name", Session("con"))
                'Response.Write("select * from emprec inner join emp_static_info as esi on emprec.emp_id=esi.emp_id where emprec.id in(select emptid from emp_job_assign where project_id=" & projid & " and '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "')) order by esi.first_name")
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
            Me.outp2.Text &= "<br /> Overtime Paid on month:" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</td>" & _
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
            If rs2.HasRows Then

                otbir = "0"
                Dim ot_date As Date
                Dim otamt As String

                Dim rt() As String
                While rs2.Read
                    rt = fm.getproj(rs2.Item("id"), pdate1, pdate2, Session("con"))
                    If rt.Length > 0 Then
                        If projid = rt(0) Then
                         
                    rs = dbx.dtmake("ot", "select * from emp_ot  where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref is not null order by datepaid", Session("con"))
                    otamt = fm.getinfo2("select sum(amt) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid='" & rs2.Item("id") & "' and paidstatus='y' and ref is not null", Session("con"))
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
                            ot_date = rs.Item("ot_date")
                            Me.outp2.Text &= "  <tr>" & Chr(13) & _
                            " <td>&nbsp;" & i.ToString & "</td>" & Chr(13) & _
                             "<td>&nbsp;" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & " </td>" & Chr(13) & _
       " <td style='text-align:right'>&nbsp;" & Chr(13)
                            sal = dbx.getsal(rs2.Item("id"), ot_date, Session("con"))
                            shr = (CDbl(sal(0)) / 200.67).ToString
                            Me.outp2.Text &= (FormatNumber(sal(0), 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"

                            Me.outp2.Text &= "<td>&nbsp;"
                            reg = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y' and ref is not null", Session("con"))
                            If reg.ToString <> "" And reg.ToString <> "None" Then
                                Me.outp2.Text &= (reg)
                                hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='reg' and paidstatus='y'  and ref is not null", Session("con"))
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
                            nig = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig' and time_diff is Not Null  and paidstatus='y'  and ref is not null", Session("con"))
                            If nig.ToString <> "" And nig <> "None" Then
                                Me.outp2.Text &= nig
                                hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='nig'  and paidstatus='y' and ref is not null", Session("con"))
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
                            sun = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y' and ref is not null", Session("con"))

                            If sun.ToString <> "" And sun <> "None" Then
                                Me.outp2.Text &= sun
                                hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='we'  and paidstatus='y' and ref is not null", Session("con"))
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
                            hd = fm.getinfo2("select sum(time_diff) from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y' and ref is not null", Session("con"))

                            If hd.ToString <> "" And hd <> "None" Then
                                Me.outp2.Text &= (hd)
                                hr = fm.getinfo2("select rate from emp_ot where datepaid between '" & pdate1 & "' and '" & pdate2 & "' and emptid=" & rs2.Item("id") & " and factored='hd'  and paidstatus='y' and ref is not null", Session("con"))
                                If hr = "None" Then
                                    hr = "1"
                                End If
                                'Response.Write(hd & hr & 'hd<br>')
                                sumhd = (CDbl(hd) * CDbl(hr)).ToString
                            Else
                                sumhd = "0"
                            End If
                            Me.outp2.Text &= "</td>"
                            Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                            Me.outp2.Text &= (FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"
                            Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                            fhrs = (CDbl(sumsun) + CDbl(sumreg) + CDbl(sumhd) + CDbl(sumnig)).ToString
                            Me.outp2.Text &= (FormatNumber(fhrs, 2, TriState.True, TriState.True, TriState.True).ToString) & " </td>"
                            Me.outp2.Text &= "<td style='text-align:right;'>&nbsp;"
                            otbir = (Math.Round(CDbl(otbir), 2) + (Math.Round(CDbl(fhrs) * CDbl(shr), 2))).ToString
                            Me.outp2.Text &= (FormatNumber((CDbl(fhrs) * CDbl(shr)).ToString, 2, TriState.True, TriState.True, TriState.True).ToString)
                            Me.outp2.Text &= "  </td>"
                            Me.outp2.Text &= "<td>" & rs.Item("ot_date") & "</td>"
                            Me.outp2.Text &= "  </tr>"
                            i += 1
                        End If
                    End If
                            rs.Close()
                            ' Response.Write("<br>" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & "=" & rt(0))
                        End If
                    End If
                End While
            End If
            Me.outp2.Text &= " <tr><td>&nbsp;</td><td>&nbsp;Total</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
            "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;'>"
            Me.outp2.Text &= (FormatNumber(otbir, 2, TriState.True, TriState.True, TriState.True)) & "</td><td>&nbsp;</td></tr>"
            Me.outp2.Text &= "</table>"
            Me.outp2.Text &= "<div style='font-size:10pt;'> *Hourly Rate=Basic Salary/200.67<br />"
            Me.outp2.Text &= "Where 200.67 is Average Normal Working Hours in a month<br /> " & _
            "**Factored Hours: 1.25 X Reg Hours +2 X Sunday(Weekends) + 2.5 X Public Holiday +  1.5 X Night Hours." & _
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
" <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printviewpaid','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

        End If
    End Function

    Function paidlist_in_month_ref()
        If Request.Form("month") <> "" Then
            Dim fm As New formMaker

            Dim reg, sun, nig, hd As String
            Dim sumreg, sumsun, sumnig, sumhd, sal(2), otbir, hr, shr, fhrs As String
            Dim i As Integer = 1
            otbir = "0"
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim spl() As String
            Dim projid As String = ""
            Dim nod As Integer
            Dim pdate1, pdate2 As Date
            If Request.Form("projname") <> "" Then 'Then
                spl = Request.Form("projname").Split("|")
                projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
            End If

            nod = Date.DaysInMonth(Request.Form("year"), Request.Form("month"))
            pdate1 = Request.Form("month") & "/1/" & Request.Form("year")
            pdate2 = Request.Form("month") & "/" & nod & "/" & Request.Form("year")
            Dim rs2 As DataTableReader
            If projid = "" Then
                rs2 = dbx.dtmake("db", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' ORDER BY emp_static_info.first_name,emprec.id desc", Session("con"))
            Else
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where emprec.hire_date<='" & pdate2 & "' and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and date_end is null) and end_date is null ORDER BY emp_static_info.first_name ")
                rs2 = dbx.dtmake("selectemp", "SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ", Session("con"))
                'Response.Write("SELECT emprec.* FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id where (emprec.hire_date between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between emprec.hire_date and isnull(emprec.end_date,'" & pdate2 & "')) and emprec.id in(select emptid from emp_job_assign where project_id='" & projid.ToString & "' and (date_from between '" & pdate1 & "' and '" & pdate2 & "' or '" & pdate1 & "' between date_from and isnull(date_end,'" & pdate2 & "'))) ORDER BY emp_static_info.first_name,emprec.id desc ")
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
" <table class='tb1' width='600px' cellpadding='0' cellspacing='0'>" & Chr(13) & _
 "<tr style='text-align:center;font-weight:bold;' >" & Chr(13) & _
              " <td style='text-align:center;font-weight:bold;font-size:13pt' colspan='10' >" & Session("company_name") & _
               "<br /> Project Name:"
            If projid <> "" Then
                Me.outp.Text &= (spl(0).ToString)
            Else
                Me.outp.Text &= ("All Projects")
            End If
            Me.outp.Text &= "<br /> Overtime Payment Sheet Paid on :" & MonthName(pdate1.Month) & " " & pdate1.Year.ToString & "</td>" & _
"              </tr>" & Chr(13) & _
"<tr style=' text-align:center;'>" & Chr(13) & _
   " <td width='20' rowspan='2'><span class='headtxt'><strong>&nbsp;No</strong></span></td>" & Chr(13) & _
    "<td width='140' rowspan='2'><span class='headtxt'><strong>&nbsp;Employee Name</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;Basic Salary</strong></span></td>" & Chr(13) & _
"    <td colspan='4'><span class='headtxt'><strong>&nbsp;Overtime Hours</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;H.Rate</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;F.Hrs **</strong></span></td>" & Chr(13) & _
    "<td width='79' rowspan='2'><span class='headtxt'><strong>&nbsp;OT. Birr</strong></span></td>" & Chr(13) & _
    "<td width='50' rowspan='2'><span class='headtxt'><strong>&nbsp;Date Work</strong></span></td>" & Chr(13) & _
  "</tr>" & Chr(13) & _
  "<tr>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Reg.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Nig.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;Sun.</strong></span></td>" & Chr(13) & _
    "<td width='30'><span class='headtxt'><strong>&nbsp;H.D</strong></span></td>" & Chr(13) & _
  "</tr>" & Chr(13)
            If rs2.HasRows Then
                otbir = "0"
                Dim arr() As String = {""}
                Dim ref As String = ""
                While rs2.Read
                    Dim rt() As String
                    ref = fm.getinfo2("select ref from payrollx where date_paid='" & pdate2 & "' and emptid=" & rs2.Item("id"), Session("con"))
                    rs = dbx.dtmake("ot", "select * from emp_ot where ref='" & ref & "' and emptid=" & rs2.Item("id"), Session("con"))
                    If fm.searcharray(arr, ref) = False Then
                        Me.outp.Text &= "<tr><td colspan='11'>" & ref & "</td></tr>"
                        ReDim Preserve arr(UBound(arr) + 1)
                        arr(UBound(arr) - 1) = ref

                    End If

                    If rs.HasRows Then
                        rs.Read()
                        Dim wd As Date
                        reg = ""
                        sun = ""
                        nig = ""
                        hd = ""
                        sumreg = ""
                        sumnig = ""
                        sumhd = ""
                        sumsun = ""
                        Me.outp.Text &= "  <tr>" & Chr(13) & _
                        " <td>&nbsp;" & i.ToString & "</td>" & Chr(13) & _
                         "<td>&nbsp;" & fm.getfullname(rs2.Item("emp_id"), Session("con")) & " </td>" & Chr(13) & _
   " <td style='text-align:right'>&nbsp;" & Chr(13)
                        If rs.IsDBNull(11) = False Then
                            wd = rs.Item("datepaid")
                        End If
                        sal = dbx.getsal(rs2.Item("id"), wd.ToShortDateString, Session("con"))
                        shr = (CDbl(sal(0)) / 200.67).ToString
                        Me.outp.Text &= (FormatNumber(sal(0), 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"

                        Me.outp.Text &= "<td>&nbsp;"
                        rt = fm.otcalc("reg", rs2.Item("id"), ref, Session("con"))

                        If rt(0) <> "" Then
                            Me.outp.Text &= rt(0)
                            ' Response.Write(rt(0) & "<br>")
                        End If
                        If rt(1) <> "" Then
                            sumreg = rt(1)
                        Else
                            sumreg = "0"
                        End If

                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= " <td>&nbsp;"
                        rt = fm.otcalc("nig", rs2.Item("id"), ref, Session("con"))

                        If rt(0) <> "" Then
                            Me.outp.Text &= rt(0)
                            'Response.Write(rt(0) & "<br>")
                        End If
                        If rt(1) <> "" Then
                            sumnig = rt(1)
                        Else
                            sumnig = "0"
                        End If

                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= " <td>&nbsp;"
                        rt = fm.otcalc("we", rs2.Item("id"), ref, Session("con"))

                        If rt(0) <> "" Then
                            Me.outp.Text &= rt(0)
                            ' Response.Write(rt(0) & "<br>")
                        End If
                        If rt(1) <> "" Then
                            sumsun = rt(1)
                        Else
                            sumsun = "0"
                        End If
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td>&nbsp;"
                        rt = fm.otcalc("hd", rs2.Item("id"), ref, Session("con"))

                        If rt(0) <> "" Then
                            Me.outp.Text &= rt(0)
                            ' Response.Write(rt(0) & "<br>")
                        End If
                        If rt(1) <> "" Then
                            sumhd = rt(1)
                        Else
                            sumhd = "0"
                        End If
                        Me.outp.Text &= "</td>"
                        Me.outp.Text &= "<td style='text-align:right;'>&nbsp;"
                        Me.outp.Text &= (FormatNumber(shr, 2, TriState.True, TriState.True, TriState.True).ToString) & "</td>"
                        Me.outp.Text &= "<td style='text-align:right;'>&nbsp;"

                        fhrs = (CDbl(sumsun) + CDbl(sumreg) + CDbl(sumhd) + CDbl(sumnig)).ToString
                        ' Response.Write(fhrs & "=" & sumsun & " + " & (sumreg) & " + " & (sumhd) & " + " & (sumnig) & "<br>")
                        Me.outp.Text &= (FormatNumber(fhrs, 2, TriState.True, TriState.True, TriState.True).ToString) & " </td>"
                        Me.outp.Text &= "<td style='text-align:right;'>&nbsp;"
                        otbir = (Math.Round(CDbl(otbir), 2) + (Math.Round(CDbl(fhrs) * CDbl(shr), 2))).ToString
                        Me.outp.Text &= (FormatNumber((CDbl(fhrs) * CDbl(shr)).ToString, 2, TriState.True, TriState.True, TriState.True).ToString)
                        Me.outp.Text &= "  </td>"
                        Me.outp.Text &= " <td>" & rs.Item("datepaid").toshortdatestring & "</td>"
                        Me.outp.Text &= "  </tr>"
                        i += 1
                    End If

                End While
            End If
            Me.outp.Text &= " <tr><td>&nbsp;</td><td>&nbsp;Total</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" & _
            "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='text-align:right;'>"
            Me.outp.Text &= (FormatNumber(otbir, 2, TriState.True, TriState.True, TriState.True)) & "</td><td>&nbsp;</td></tr>"
            Me.outp.Text &= "</table>"
            Me.outp.Text &= "<div style='font-size:10pt;'> *Hourly Rate=Basic Salary/200.67<br />"
            Me.outp.Text &= "Where 200.67 is Average Normal Working Hours in a month<br /> " & _
            "**Factored Hours: 1.25 X Reg Hours +2 X Sunday(Weekends) + 2.5 X Public Holiday +  1.5 X Night Hours." & _
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
" <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printview','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

        End If
    End Function
    Function getproj(ByVal emptid As Integer, ByVal pd1 As Date, ByVal pd2 As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim fm As New formMaker
        Dim rt(2) As String
        Dim hrd As String
        hrd = fm.getinfo2("select hire_date from emprec where id=" & emptid, con)
        rs = dbs.dtmake("dbsproj", "select project_id,date_from,date_end from emp_job_assign where emptid=" & emptid & " and ('" & pd1 & "' between date_from and isnull(date_end,'" & pd2 & "') or (month(date_from)='" & CDate(hrd).Month & "' and year(date_from)='" & CDate(hrd).Year & "'))", con)
        If rs.HasRows Then
            While rs.Read()

                ' Response.Write("<br>" & emptid.ToString & "===" & rs.Item("date_from"))
               
                rt(0) = rs.Item("project_id")
                rt(1) = dbs.getprojectname(rt(0), con)

            End While
           
        End If
        Return rt
    End Function
End Class
