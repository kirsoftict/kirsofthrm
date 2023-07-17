Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO

Partial Class pensionnew
    Inherits System.Web.UI.Page
    Private Shared pd1, pd2, pe, pc, mname As String

    Private Shared fm As New formMaker
    Private Shared dt As New dtime
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      
       
    End Sub
    Function mksql()
        Dim sql As String
        sql = "select distinct emptid from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0"
        sql = "select emp_id,emprec.id from emprec inner join payrollx as px on emprec.id=px.emptid where px.pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and px.pen_e>0 and px.pen_c>0 order by px.ref"
        'Response.Write(sql)
        Return sql
    End Function
    Function first_page_title(ByVal pgid As String)
        Dim rtn As String = "    <header>" & _
        "<table class='data' border='1'>" & _
          "  <tbody><tr style='height:70px;'>" & _
           "     <td align='center'><img src='./Pension_files/fdre_logo.jpg' width='66'></td>" & _
            "    <td width='38%' style='background-color: #666; color: #FFF;' colspan='3'>" & _
             "       <div class='govt_office' style='background-color: #666; color: #FFF;min-height:66px;'>" & _
              "          በኢትዮጵያ ፌዴራላዊ ዲሞክራሲያዊ ሪፐብሊክ<br>" & _
               "         የኢትዮጵያ ገቢዎችና ጉምሩክ ባለሥልጣን" & _
                "    </div>" & _
                "</td>" & _
                "<td width='42%' align='center' colspan='3'>" & _
                 "   <span class='form_title'>" & _
                  "      የግል ድርጅት ሠራተኞች የጡረታ መዋጮ ማሳወቂያ ቅፅ" & _
                   " </span>" & _
                    "<span style='font-size: 14px;'> (በግል ድርጅቶች ሠራተኞች ጡረታ አዋጅ ቁጥር 715/2003)</span>" & _
                "</td>" & _
                "<td align='center'><img src='./Pension_files/erca_logo.gif' width='66'></td>" & _
           " </tr>" & _
        "</tbody></table>" & _
        "<div style='clear: both;'></div>" & _
    "</header>" & _
    "<div>" & _
     "   <div class='table_title'>ክፍል 1 - የጡረታ መዋጮውን የሚከፍለው ድርጅት ዝርዝር መረጃ</div>" & _
      "  <table width='100%' class='table1' border='1'>" & _
       "     <tbody><tr>" & _
        "        <td colspan='3' rowspan='2' valign='top'><span class='header2'>1. የግብር ከፋይ ስም</span><br>" & Application("company_name_amharic") & _
"</td>" & _
"<td rowspan='2' valign='top' style='width: 140px;'><span class='header2'>3. የግብር ከፋይ መለያ ቁጥር</span><br>" & _
Session("tin") & _
"</td><td rowspan='2' valign='top'><span class='header2'>4. የግብር ሂሳብ ቁጥር</span></td>" & _
"<td colspan='2' valign='top' align='center' style='line-height: 10px;'><span class='header2'>8. የክፍያ ግዜ</span></td>" & _
"<td rowspan='2' width='8%' align='center' class='english header2'>Page <span class='this_page'>" & pgid & "</span> of <span class='page_count'></span></td>" & _
 "           </tr>" & _
  "          <tr style='line-height: 10px;'>" & _
   "             <td width='100' class='header2'>ወር <span class='print_month' id='print_month-" & pgid & "'>" & mname & "</span></td><td width='90'>ዓ.ም. <span class='year'></span></td>" & _
     "       </tr>" & _
      "      <tr>" & _
       "         <td valign='top'><span class='header2'>2a. ክልል</span><br>&nbsp;" & Session("kelele") & "</td><td colspan='2' valign='top'><span class='header2'>2b. ዞን/ክፍለ ከተማ</span><br>" & Session("zone") & "</td><td colspan='2' valign='top'><span class='header2'>5. የግብር ስብሰቢ መ/ቤት ስም</span></td><td rowspan='2' colspan='3' valign='top' class='header2'>የሰነድ ቁጥር (ለቢሮ አገልግሎት ብቻ)</td>" & _
        "    </tr>" & _
         "   <tr>" & _
          "      <td valign='top'><span class='header2'>2c. ወረዳ</span><br>&nbsp;" & Session("worda") & "</td><td valign='top'><span class='header2'>2d. ቀበሌ/የገበሬ ማህበር</span><br>" & Session("Kebele") & "</td><td valign='top'><span class='header2'>2e. የቤት ቁጥር</span><br>" & Session("hno") & "</td><td valign='top'><span class='header2'>6. ስልክ ቁጥር</span><br>" & Session("tel") & "</td><td valign='top'><span class='header2'>7. ፋክስ ቁጥር</span><br>" & Session("fax") & "</td>" & _
           " </tr>" & _
        "</tbody></table>" & _
        "        <div class='table_title'>ሠንጠረዥ 2 - ማስታወቂያ ዝርዝር መረጃ</div>"
        Return rtn
    End Function
    Function other_page_title(ByVal pageno As Integer, ByVal pgid As String)
        Dim rtn As String = "  <header>" & _
        "<table class='data' border='1'>" & _
           " <tbody><tr>" & _
                "<td align='center'><img src='./Pension_files/fdre_logo.jpg' width='66'></td>" & _
             "   <td width='38%' style='background-color: #666; color: #FFF;' colspan='3'>" & _
                "    <div class='govt_office'>" & _
                      "  በኢትዮጵያ ፌዴራላዊ ዲሞክራሲያዊ ሪፐብሊክ<br>" & _
                     "   የኢትዮጵያ ገቢዎችና ጉምሩክ ባለሥልጣን" & _
                   " </div>" & _
                "</td>" & _
                "<td width='42%' align='center' colspan='3'>" & _
                 "   <span class='form_title'>" & _
                 "       የግል ድርጅት ሠራተኞች የጡረታ መዋጮ ማሳወቂያ ቅፅ" & _
                  "  </span>" & _
                            "<span style='font-size: 14px;'> (በግል ድርጅቶች ሠራተኞች ጡረታ አዋጅ ቁጥር 715/2003)<br>ቅጽ ቁጥር 2/2003 የተጨማሪ ማስታወቂያ ቀጽ</span>" & _
                "</td>" & _
                "<td align='center'><img src='./Pension_files/erca_logo.gif' width='66'></td>" & _
            "</tr>" & _
        "</tbody></table>" & _
        "<div style='clear: both;'></div>" & _
    "</header>" & _
    "<div>" & _
     "   <div class='table_title'>ክፍል - 1 የጡረታ መዋጮውን የሚከፍለው ድርጅት ዝርዝር መረጃ</div>" & _
      "  <table width='100%' class='table1' border='1'>" & _
       "     <tbody><tr>" & _
        "        <td colspan='3' rowspan='2' valign='top'><span class='header2'>1. የግብር ከፋይ ስም</span><br>" & Application("company_name_amharic") & _
"</td><td rowspan='2' valign='top'><span class='header2'>" & _
"2. የግብር ከፋይ መለያ ቁጥር</span><br>" & Session("tin") & "" & _
"</td><td colspan='2' valign='top' align='center' style='line-height: 10px;'><span class='header2'>8. የክፍያ ግዜ</span> </td>" & _
"<td rowspan='2' width='8%' align='center' class='english'>Page <span class='this_page'>" & pageno & "</span> of <span class='page_count'></span></td>" & _
         "   </tr>" & _
          "  <tr style='line-height: 10px;'>" & _
           "     <td width='100'><span class='header2'>ወር</span> <span class='print_month' id='print_month-" & pgid & "'>" & mname & " </span></td><td width='90'><span class='header2'>ዓ.ም.</span> <span class='year'></span></td>" & _
 "           </tr>" & _
  "      </tbody></table>" & _
   "             <div class='table_title'>ክፍል 2 - ማስታወቂያ ዝርዝር መረጃ</div>"
        Return rtn
    End Function
    Function first_footer()
        Dim rtn As String = ""
        rtn = " <table width='100%' class='table_title' border='0' style='padding-top: 0px;'>" & _
        "<tbody><tr>" & _
         "   <td>ክፍል 3 - የወሩ የተጠቃለለ ሂሳብ</td><td>ክፍል 4 - በዚህ ወር የሥራ ውላቸው የተቋረጠ ሠራተኞች ዝርዝር መረጃ</td><td>ለቢሮ አገልግሎት ብቻ</td>" & _
        "</tr>" & _
    "</tbody></table>" & _
    "<table width='100%' class='sub_data table3' border='0'>" & _
     "   <tbody><tr>" & _
      "      <td width='40%' valign='top'> " & _
       "         <table width='95%' border='1' class='data table4' style='margin:auto;'>" & _
        "            <tbody><tr>" & _
         "               <td width='1%'></td><td width='20%' colspan='2'>በዚህ ወር ደመወዝ የሚከፈላቸው የሠራተኞች ብዛት</td><td width='10%' style='text-align:right;'><span class='table_box_right' id='months_salaried' style='text-align: center;'></span></td>" & _
          "          </tr>" & _
           "         <tr>" & _
            "            <td>20</td><td  colspan='2'>የወሩ ጠቅላላ የሠራተኞች ደመወዝ (ከላይ ካለው ከሠንጠረዥ (ሠ))</td><td style='text-align:right;'><span class='table_box_right' id='months_salary' style='text-align: center;'></span></td>" & _
             "       </tr>" & _
              "      <tr>" & _
               "         <td>30</td><td colspan='2'>የወሩ ጠቅላላ የሠራተኞች መዋጮ መጠን (ከላይ ካለው ከሠንጠረዥ (ረ))</td><td style='text-align:right;'> <span class='table_box_right' id='months_employee_contrib' style='text-align: center;'></span></td>" & _
                "    </tr>" & _
                 "   <tr>" & _
                  "      <td>40</td><td colspan='2'>የወሩ ጠቅላላ የአሰሪው መዋጮ መጠን (ከላይ ካለው ከሠንጠረዥ (ሰ))</td><td style='text-align:right;'><span class='table_box_right' id='months_employer_contrib' style='text-align: center;'></span></td>" & _
                   " </tr>" & _
                    "<tr>" & _
                     "   <td>50</td><td colspan='2'>የወሩ ጠቅላላ ጥቅል መዋጮ መጠን (ከላይ ካለው ከሠንጠረዥ (ሸ))</td><td style='text-align:right;'><span class='table_box_right' id='months_total_contrib' style='text-align:right;'></span></td>" & _
                    "</tr>" & _
   "             </tbody></table>" & _
    "        </td>" & _
     "       <td width='35%' valign='top'>" & _
      "          <table  width='95%' border='1' class='data table4' style='margin:auto;'>" & _
       "             <tbody><tr>" & _
        "                <td width='1%'>ተ.ቁ</td><td width='20%'>የሠራተኛው የግብር<br>ከፋይ ቁጥር</td><td width='30%'>የሠራተኛው /ስም የአባት ስምና የአያት ስም/</td>" & _
         "           </tr>" & _
          "          <tr>" & _
           "             <td>&nbsp;</td><td class='table_box_right' id='quit_tin1'></td><td class='table_box_right' id='quit_name1'></td>" & _
            "        </tr>" & _
             "       <tr>" & _
              "          <td>&nbsp;</td><td class='table_box_right' id='quit_tin2'></td><td class='table_box_right' id='quit_name2'></td>" & _
               "     </tr>" & _
                "    <tr>" & _
                 "       <td>&nbsp;</td><td class='table_box_right' id='quit_tin3'></td><td class='table_box_right' id='quit_name3'></td>" & _
                  "  </tr>" & _
                   " <tr>" & _
                    "    <td>&nbsp;</td><td class='table_box_right' id='quit_tin4'></td><td class='table_box_right' id='quit_name4'></td>" & _
                    "</tr>" & _
                "</tbody></table>" & _
            "</td>" & _
            "<td width='25%' valign='top'>" & _
             "   <table width='95%' class='data table4' border='1'>" & _
              "      <tbody><tr>" & _
               "         <td width='42%'>የተከፈለበት ቀን</td><td class='table_box_right' readonly='readonly'></td>" & _
                "    </tr>" & _
                 "   <tr>" & _
                  "      <td>የደረሰኝ ቁጥር</td><td class='table_box_right' readonly='readonly'></td>" & _
                   " </tr>" & _
                    "<tr>" & _
                     "   <td>የገንዘብ ልክ</td><td class='table_box_right' readonly='readonly'></td>" & _
              "      </tr>" & _
               "     <tr>" & _
                "        <td>ቼክ ቁጥር</td><td class='table_box_right' readonly='readonly'></td>" & _
                 "   </tr>" & _
                  "  <tr>" & _
                   "     <td nowrap=''>የገንዘብ ተቀባይ ፊርማ</td><td class='table_box_right' readonly='readonly'></td>" & _
                    "</tr>" & _
                "</tbody></table>" & _
       "     </td>" & _
    "</tr></tbody></table>" & _
    "<div class='table_title'>ክፍል 5 - የትክክለኛነት ማረጋገጫ</div>" & _
   " <table width='100%' class='sub_data table3' border='1'>" & _
    "    <tbody><tr>" & _
     "       <td width='32%' style='line-height: 20px;' colspan='2'>በላይ የተገለፀው ማስታወቂያና የተሰጠው መረጃ በሙሉ የተሞላና ትክክለኛ መሆኑን አረጋግጣለሁ፡፡ ትክክለኛ ያልሆነ መረጃ ማቅረብ በግብር ሕጎችም ሆነ በወንጀለኛ መቅጫ ሕግ የሚያስቀጣ መሆኑን እገነዘባለሁ፡፡</td>" & _
      "      <td colspan='2'>የግብር ከፋዩ/ሕጋዊ ወኪሉ<br>ስም <span class='underlined_box ethiopic replicate' id='signee-a' name='signee' size='30' jgeez-index='0'>___________________________________</span><br>ፊርማ <span class='underlined_box' size='10' readonly='readonly'>______________</span> ቀን <span class='underlined_box date replicate hasCalendarsPicker' id='signature_date-a' name='signature_date' size='14'>__________</span></br>&nbsp;</td>" & _
       "     <td width='12%' valign='top' align='center'>ማህተም</td>" & _
        "    <td width='30%' colspan='2'>የግብር ባለሥልጣን ስም <span class='underlined_box' size='30' readonly='readonly'>_______________________________________</span><br>ፊርማ <span class='underlined_box' size='30' readonly='readonly'>_________________________</span><br>ቀን <span class='underlined_box' size='30' readonly='readonly'>____________________________</span></td>" & _
        "</tr>" & _
    "</tbody></table>" & _
     "    <small class='english'>" & _
     "<span class='right_spaced'>powerd by KirSoft</span>" & _
       " <span class='right_spaced'>Ethiopian Revenue and Customs Authority (as of 8--/5/2011)</span>" & _
        "ERCA(Form - ---(P1 / 2011))" & _
 "</small><br>" & _
    "<small>ማሳሰቢያ፦ የሠራተኞችን ዝርዝር መሙያ ተጨማሪ ቦታ ካስፈለገዎት የተጨማሪ ማስታወቂያ ቅፁን ይጠቀሙ</small>"
        Return rtn
    End Function
    Function other_footer()
        Dim rtn As String = "<table width='100%' class='sub_data table3' border='0'><tbody><tr>" & _
           " <td colspan='9'>የግብር ከፋዩ/ሕጋዊ ወኪሉ ስም <span class='underlined_box ethiopic replicate' id='signee-b' name='signee' width='30' jgeez-index='1'>____________________________</sapn> ፊርማ <span class='underlined_box' size='15' readonly='readonly'>______________________</span> ቀን <span class='underlined_box date hasCalendarsPicker' id='signature_date-b' name='signature_date' size='18'>_____________________</span></td>" & _
       " </tr>    </tbody></table> <small class='english'>powred by KirSoft</small>"
        Return rtn
    End Function
    Function tabletitle()
        Dim rtn As String = " <table width='100%'  border='1' class='table1' style='border:1px solid black'><tbody><tr align='center'>" & _
               " <td style='width:3%;' class='header2'>ሀ) ተ.ቁ</td><td style='width:7%;' class='header2'>ለ) የቋሚ የሠራተኛው የግብር ከፋይ መለያ ቁጥር (TIN)</td><td style='width:20%' class='header2'>ሐ) የሠራተኛው ስም ፣ የአባት ስም እና የአያት ስም</td><td style='width:7%' class='header2'>መ) የተቀጠሩበት ቀን /ቀን/ወር/ዓም/</td><td style='width:10%' class='header2'>ሠ) የወር ደመወዝ /ብር/</td><td style='width:10%' class='header2'>ረ) የሰራተኛው መዋጮ መጠን<br>" & pe & "% /ብር/</td><td style='line-height: 11px; width:10%' class='header2'>ሰ) የአሰሪው<br>መዋጮ መጠን<br>" & pc & "% /ብር/</td><td class='header2' width='10%' class='header2'>ሸ) በአሰሪው የሚገባ ጥቅል መዋጮ<br>" & CInt(pc) + CInt(pe) & "% /ብር/ (ረ + ሰ)</td><td width='10%' class='header2'>ፊርማ</td>" & _
           " </tr>"
        Return rtn
    End Function
    Function collectiononly()
        pd1 = dt.convert_to_ethx(Request.Form("pd1"))
        pd2 = dt.convert_to_ethx(Request.Form("pd2"))
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & Request.Form("pd1") & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & Request.Form("pd1") & "' order by id desc", Session("con"))
        Dim nextpage() As Double = {0, 0, 0, 0}
        Dim onpage() As Double = {0, 0, 0, 0}
        Dim firstpagesum() As Double = {0, 0, 0, 0}
        Dim jv As String = "var suffix_list = new Array("
        Dim pen, epen, sal, sump As Double
        Dim db As New dbclass
        Dim rs As DataTableReader
        Dim numpages As Integer
        Dim firstpage As Integer = 6
        Dim perpage As Integer = 20
        Dim numrows As Integer
        Dim tpg, pgno As Integer
        Dim emptid, rowid As String
        Dim ri As Integer
        Dim print As String = ""
        Dim empname As String
        Dim cell(12) As String
        Dim sumpen As Double
        Dim tin, hdate As String
        Dim empid As String
        Dim ethcal() As String = pd1.Split(".")
        Response.Write(pd1.ToString)
        mname = dt.getmonth(ethcal(1), "amh")
        Dim rtn As String = ""
        Dim arrproject() As String = {""}
        Dim ref As String = ""
        Dim proj() As String = {""}
        Dim spl() As String
        Dim sumtable As String = ""
        Dim sumemp, sumcomp, groupt, totall As Double
        Dim sumpenrpt As String = fm.getinfo2("select sum(pen_e) from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "'", Session("con"))

        Dim hhhd As String = ""
        Dim sumpencrpt As String = fm.getinfo2("select sum(pen_c) from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "'", Session("con"))
        Response.Write("<div class='blue_btn' id='print' onclick=" & Chr(34) & "javascript:print('sumprt','" & Session("conmpany_name") & "','','','pension')" & Chr(34) & ");'>Print</div>")
        hhhd = "<div id='sumprt'><table class='data' border='1'><thead><tr><td colspan=2  class='table_title'>Summery Table</td></tr></thead>"
        If sumpencrpt <> "None" Then
            hhhd &= "<tbody><tr><td>Pension Employee Contrbution </td><td class='numberx'>" & fm.numdigit(sumpenrpt, 2) & "</td></tr><tr>" & _
                "<td>Pension Employee Contrbution </td><td class='numberx'>" & fm.numdigit(sumpencrpt, 2) & "</td></tr><tr>" & _
                "<td>Total Pension Contrbution </td><td class='numberx'>" & fm.numdigit(CDbl(sumpencrpt) + CDbl(sumpenrpt), 2) & "</td></tr></tbody></table></div>"
        Else
            hhhd &= "<tbody><tr><td>data is not found please check the data</td></tr></tbody></table></div>"
        End If
        Response.Write(hhhd)
        Try
            rs = db.dtmake("vwsql", mksql(), Session("con"))
            If rs.HasRows Then
                Dim sql As String = "select count(id) from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 and emptid in(select distinct emptid from payrollx  where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0)"

                numrows = fm.getinfo2("select  ROW_NUMBER() Over(order by emptid asc) as Rowno from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 group by emptid order by rowno desc", Session("con"))
                tpg = Math.Ceiling((numrows - 6) / perpage) + 1
                pgno = 1
                ri = 1
                sumcomp = 0
                sumemp = 0
                groupt = 0
                totall = 0
                ' If tpg > 1 Then


                ' Else

                'End If
                Dim refxxx As String = ""
                ' Response.Write("<br>" & sql & "<<<<<<<<<<<<br>" & numrows)
                Dim idclause As String
                sumtable = "<table>"
                Dim emxid As String = ""
                While rs.Read
                    idclause = ""
                    empid = rs.Item("emp_id")
                    ' Response.Write(empid & "<br>")
                    ' emptid = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                    emptid = getids(empid, Request.Form("pd1"), Request.Form("pd2"))
                    emptid = rs.Item(1)

                    If emptid = "" Then
                        idclause = fm.getinfo2("select id from emprec where emp_id='" & empid & "' order by id desc", Session("con"))
                        idclause = "emptid=" & idclause & " and "
                    Else
                        idclause = " emptid in(" & emptid & ") and "
                    End If
                    If empid <> emxid Then
                        emxid = empid
                    Else
                        Response.Write("<br>---------------<br>" & empid & " is duplicated <br>----------------------------")
                    End If
                    ' Response.Write(empid)
                    hdate = fm.getinfo2("select hire_date from emprec where emp_id='" & empid & "' order by id desc", Session("con"))
                    ' Response.Write("<br>select hire_date from emprec where id in(" & emptid & ") order by desc")
                    hdate = Format(CDate(hdate), "MM/dd/yyyy")
                    ' empname = fm.getfullname(rs.Item("emp_id"), Session("con"))
                    Try
                        Dim kx, mx, rx As String
                        kx = fm.getinfo2("select sum(pen_c) from payrollx where " & idclause & "  pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_c>0", Session("con"))
                        mx = fm.getinfo2("select sum(pen_e) from payrollx where " & idclause & "  pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0", Session("con"))
                        sal = fm.getinfo2("select sum(b_e) from payrollx where  " & idclause & " pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 ", Session("con"))

                        If IsNumeric(kx) = False Then
                            Response.Write(kx & "<br>")
                            pen = 0
                        Else
                            pen = kx
                        End If
                        If IsNumeric(mx) = False Then
                            Response.Write(mx)
                            epen = 0
                        Else
                            epen = mx
                        End If


                    Catch exp As Exception
                        Response.Write("error on idclose: " & idclause & "<br>")
                        '  pen = fm.getinfo2("select sum(pen_c) from payrollx where " & idclause & "  pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0", Session("con"))
                        '  epen = fm.getinfo2("select sum(pen_e) from payrollx where " & idclause & "  pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0", Session("con"))
                        ' sal = fm.getinfo2("select sum(b_sal) from payrollx where  " & idclause & " pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 ", Session("con"))
                        pen = 0
                        epen = 0

                    End Try

                    sumpen = pen + epen
                    tin = fm.getinfo2("select emp_tin from emp_static_info where  emp_id='" & empid & "'", Session("con"))
                    ref = fm.getinfo2("select ref from payrollx where  " & idclause & "  pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 ", Session("con"))

                    Dim spl2() As String
                    If fm.searcharray(arrproject, ref) = False Then
                        Dim pdate As String = (fm.getinfo2("select date_paid from payrollx where  ref='" & ref & "' ", Session("con")))

                        ReDim Preserve arrproject(UBound(arrproject) + 1)
                        arrproject(UBound(arrproject) - 1) = ref
                        ' spl = fm.getproj(emptid, Request.Form("pd1"), Request.Form("pd2"), Session("con"))

                        spl = fm.getproj_on_date(emptid, CDate(pdate).ToShortDateString, Session("con"))




                        If fm.searcharray(proj, spl(0)) = False Then
                            ReDim Preserve proj(UBound(proj) + 1)
                            proj(UBound(proj) - 1) = spl(0)
                            Session(spl(0)) = ref & ","



                            '  Response.Write("<br>" & ref & " => " & sumemp & " => " & sumcomp & " => " & (sumcomp + sumemp) & "<br>")
                        Else

                            Session(spl(0)) &= ref & ","
                            '  Response.Write("<br>" & ref & " => " & sumemp & " => " & sumcomp & " => " & (sumcomp + sumemp) & "<br>")
                        End If



                        ' Response.Write("+++" & idclause & "===========" & ref & "========" & UBound(arrproject) & "======<br>")
                    Else

                    End If
                    If pgno = 1 Then
                        If ri = 1 Then
                            print = "<div class='a4_landscape'>" & _
        first_page_title(pgno) & tabletitle()

                        End If
                        If ri Mod 7 = 0 Then
                            ' firstpagesum = onpage

                            firstpagesum(0) = onpage(0)

                            firstpagesum(1) = onpage(1)

                            firstpagesum(2) = onpage(2)


                            firstpagesum(3) = onpage(3)
                            print &= "  </tr> <tr>" & Chr(13) & _
               " <td colspan='4' align='right'>ከአባሪ ቅጾች ፣ የመጣ ድምር</td><td style='text-align:right;' id='other_pages_total_salary-" & pgno & "'>&nbsp;</td><td style='text-align:right;' class='table_box_right' id='other_pages_total_employee_contrib-" & pgno & "' size='10'>&nbsp;</td><td style='text-align:right;' class='table_box_right' id='other_pages_total_employer_contrib-" & pgno & "' size='10'>&nbsp;</td><td style='text-align:right;' class='table_box_right' id='other_pages_total_contrib-" & pgno & "'>&nbsp;</td>" & _
           " </tr><tr>" & Chr(13)

                            print &= "      <td colspan='4' align='right'>ድምር</td><td align='center' class='table_box_right' id='total_salary-" & pgno & "'><em>(line 20)</em>" & FormatNumber(firstpagesum(3), 2, TriState.True, TriState.True) & "</td><td align='center' class='table_box_right' id='total_employee_contrib-" & pgno & "'><em>(line 30)</em>" & FormatNumber(onpage(0), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' align='center' id='total_employer_contrib-" & pgno & "'><em>(line 40)</em>" & FormatNumber(onpage(1), 2, TriState.True, TriState.True) & "</td><td align='center' class='table_box_right' id='total_contrib-" & pgno & "'><em>(line 50)</em>" & FormatNumber(onpage(2), 2, TriState.True, TriState.True) & "<input type='hidden' id='count-" & pgno & "'></td>" & _
                             "  </tr>" & Chr(13)

                            onpage(0) = 0
                            onpage(1) = 0
                            onpage(2) = 0
                            onpage(3) = 0
                            print &= "</tbody></table></div>"
                            print &= first_footer()
                            print &= "</div><div id='page_separator'></div>"
                            print &= Chr(13) & "<div class='a4_landscape'>"
                            pgno = 2
                            print &= other_page_title(pgno, tpg) & tabletitle()
                            ' Response.Write(firstpagesum(0))


                        End If
                    End If
                    If pgno > 1 Then
                        If (ri - 6) Mod 20 = 0 Then

                            print &= "  </tr> <tr>" & _
               " <td colspan='4' align='right'>ከአባሪ ቅጾች ፣ የመጣ ድምር</td><td class='table_box_right' id='other_pages_total_salary-" & pgno & "' size='10'>" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='other_pages_total_employee_contrib-" & pgno & "' size='10' >" & FormatNumber(nextpage(0), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='other_pages_total_employer_contrib-" & pgno & "' size='10'>" & FormatNumber(nextpage(1), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='other_pages_total_contrib-" & pgno & "' >" & FormatNumber(nextpage(2), 2, TriState.True, TriState.True) & "</td>" & _
           " </tr>" & Chr(13)
                            nextpage(0) = nextpage(0) + onpage(0)
                            nextpage(1) = nextpage(1) + onpage(1)
                            nextpage(2) = nextpage(2) + onpage(2)
                            nextpage(3) = nextpage(3) + onpage(3)

                            print &= "     <tr> <td colspan='4' align='right'>on page ድምር</td><td class='table_box_right' id='onpagetotal_salary-" & pgno & "' >" & FormatNumber(onpage(3), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='onpagetotal_employee_contrib-" & pgno & "' size='10' >" & FormatNumber(onpage(0), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='onpagetotal_employer_contrib-" & pgno & "' >" & FormatNumber(onpage(1), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='onpagetotal_contrib-" & pgno & "'>" & FormatNumber(onpage(2), 2, TriState.True, TriState.True) & "</td></tr>" & Chr(13)

                            print &= "     <tr> <td colspan='4' align='right'>ድምር</td><td class='table_box_right' id='total_salary-" & pgno & "'  >" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='total_employee_contrib-" & pgno & "'>" & FormatNumber(nextpage(0), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='total_employer_contrib-" & pgno & "'>" & FormatNumber(nextpage(1), 2, TriState.True, TriState.True) & "</td><td class='table_box_right' id='total_contrib-" & pgno & "'>'" & FormatNumber(nextpage(2), 2, TriState.True, TriState.True) & "<input type='hidden' id='count-" & pgno & "' /></td></tr>" & Chr(13)
                            onpage(0) = 0
                            onpage(1) = 0
                            onpage(2) = 0
                            onpage(3) = 0
                            print &= "</tbody></table>"
                            print &= other_footer()
                            print &= "</div></div><div id='page_separator'></div>"
                            pgno += 1
                            print &= "<div class='a4_landscape'>"
                            print &= other_page_title(pgno, tpg) & tabletitle()
                        End If
                    End If
                    onpage(0) = epen + onpage(0)
                    onpage(1) = pen + onpage(1)
                    onpage(2) = sumpen + onpage(2)
                    onpage(3) = sal + onpage(3)

                    print &= "  <tr style='line-height: 20px;' class='repeatable-data-row'>"
                    print &= "  <td id='row_no-" & pgno & ri & "' class='data-p'>" & ri & "</td>" & Chr(13) & _
        "<td class='table_box_right' id='tin-" & pgno & ri & "'>" & tin & "</td>" & Chr(13) & _
        "<td class='table_box_left' id='name-" & pgno & ri & "'>" & Chr(13) & _
             fm.getfullname(empid, Session("con")) & "</td>" & Chr(13) & _
        "<td class='table_box_left date' id='hire_date-" & pgno & ri & "'>" & hdate & "</td>" & Chr(13) & _
        "       <td class='table_box_right' id='salary-" & pgno & ri & "' data='-" & pgno & "'>" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.UseDefault) & "</td>" & _
        "      <td class='table_box_right' id='employee_contrib-" & pgno & ri & "'>" & FormatNumber(epen, 2, TriState.True, TriState.True, TriState.UseDefault) & "</td>" & _
        "     <td class='table_box_right' id='employer_contrib-" & pgno & ri & "'>" & FormatNumber(pen, 2, TriState.True, TriState.True, TriState.UseDefault) & "</td>" & _
        "    <td class='table_box_right' id='total_contrib-" & pgno & ri & "'>" & FormatNumber(sumpen, 2, TriState.True, TriState.True, TriState.UseDefault) & "</td>" & _
        "   <td class='table_box_right' readonly='readonly'>&nbsp;</td></tr>" & Chr(13) & Chr(13)

                    '               print &= "<td>" & ri & "</td>" & _
                    '      "<td>" & tin & "</td><td><input type='text' class='table_box_left ethiopic' id='name-b2' style='width: 260px;' jgeez-index='1' value='" & _
                    '      fm.getfullname(empid, Session("con")) & "'></td>" & _
                    '    "<td><span class='table_box_left date hasCalendarsPicker' id='hire_date-" & pgno & ri & "'>" & hdate & "</span></td> " & _
                    '   "<td><span class='table_box_right calc' id='salary-" & pgno & ri & "' data='" & ri & "'>" & sal & "<span</td>" & _
                    '  "<td><span class='table_box_right' id='employee_contrib-" & pgno & ri & "'>" & epen & "</span></td>" & _
                    ' "<td><span class='table_box_right' id='employer_contrib-" & pgno & ri & "'> " & pen & "</span></td>" & _
                    '"<td><span class='table_box_right' id='total_contrib-" & pgno & ri & "'>" & sumpen & "</span></td>" & _
                    '"<td><span class='table_box_right' readonly='readonly'></span></td> </tr>"

                    ri = ri + 1

                End While
            End If
            rs.Close()
        Catch ex As Exception
            Response.Write(mksql() & "ON errror" & ex.ToString)
        End Try
        ' Response.Write(ri)
        ' If ri - 1 < numrows Then
        'numrows = ri - 1
        'Else

        'End If
        While (ri - 6) Mod 20 > 0
            print &= "  <tr style='line-height: 9px;' class='repeatable-data-row'>"
            print &= "  <td><span id='row_no-" & pgno & ri & "'>&nbsp;</span></td>" & Chr(13) & _
"<td><span class='table_box_right' id='tin-" & pgno & ri & "' >&nbsp;</span></td>" & _
"<td><span class='table_box_left ethiopic' id='name-" & pgno & ri & "' style='width: 260px;' jgeez-index='0' >&nbsp;</span></td>" & _
"<td><span class='table_box_left date hasCalendarsPicker' id='hire_date-" & pgno & ri & "'>&nbsp;</span></td>" & _
"       <td><span class='table_box_right calc' id='salary-" & pgno & ri & "' data='-" & pgno & "'>&nbsp;</span></td>" & _
"      <td><span class='table_box_right' id='employee_contrib-" & pgno & ri & "' >&nbsp;</span></td>" & Chr(13) & _
"     <td><span class='table_box_right' id='employer_contrib-" & pgno & ri & "'>&nbsp;</span></td>" & Chr(13) & _
"    <td><span class='table_box_right' id='total_contrib-" & pgno & ri & "'>&nbsp;</span></td>" & _
"   <td><span class='table_box_right' readonly='readonly'>&nbsp;</span></td></tr>" & Chr(13)
            ri = ri + 1
        End While
        print &= "   <tr>" & _
           " <td colspan='4' align='right'>ከአባሪ ቅጾች ፣ የመጣ ድምር</td><td class='table_box_right'><span class='table_box_right' id='other_pages_total_salary-" & pgno & "' size='10'>" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True, TriState.True) & "</span></td><td class='table_box_right'><span class='table_box_right' id='other_pages_total_employee_contrib-" & pgno & "' size='10'>" & FormatNumber(nextpage(0), 2, TriState.True, TriState.True, TriState.True) & "</span></td><td class='table_box_right'><span class='table_box_right' id='other_pages_total_employer_contrib-" & pgno & "' size='10'>" & FormatNumber(nextpage(1), 2, TriState.True, TriState.True, TriState.True) & "</span></td><td class='table_box_right'><span class='table_box_right' id='other_pages_total_contrib-" & pgno & "'>" & FormatNumber(nextpage(2), 2, TriState.True, TriState.True, TriState.True) & "</span></td>" & Chr(13) & _
       " </tr>" & Chr(13)
        nextpage(0) = nextpage(0) + onpage(0)
        nextpage(1) = nextpage(1) + onpage(1)
        nextpage(2) = nextpage(2) + onpage(2)
        nextpage(3) = nextpage(3) + onpage(3)
        print &= "     <tr> <td colspan='4' align='right'>ድምር</td><td class='table_box_right' id='total_salary-" & pgno & "' size='10'>" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True, TriState.True) & "</td><td class='table_box_right'><span class='table_box_right' id='total_employee_contrib-" & pgno & "' size='10' >" & FormatNumber(nextpage(0), 2, TriState.True) & "</span></td><td class='table_box_right'><span class='table_box_right' id='total_employer_contrib-" & pgno & "' size='10'>" & nextpage(1) & "</span></td><td class='table_box_right'><span class='table_box_right' id='total_contrib-" & pgno & "'>" & nextpage(2) & "</span><input type='hidden' id='count-" & pgno & "' /></td></tr>" & Chr(13)

        print &= "</tbody></table>"
        print &= other_footer()
        print &= "</div></div>" & Chr(13)
        'Response.Write("<br>Emp Cont:=" & firstpagesum(0) & " + " & nextpage(0) & "<br>")
        firstpagesum(0) = firstpagesum(0) + nextpage(0)
        'Response.Write("<br>comp cont:=" & firstpagesum(1) & " + " & nextpage(1) & "<br>")
        firstpagesum(1) = firstpagesum(1) + nextpage(1)
        'Response.Write("<br>sum cont:=" & firstpagesum(2) & " + " & nextpage(2) & "<br>")
        firstpagesum(2) = firstpagesum(2) + nextpage(2)
        'Response.Write("<br>Bsal:=" & firstpagesum(3) & " + " & nextpage(3) & "<br>")
        firstpagesum(3) = firstpagesum(3) + nextpage(3)


        For kp As Integer = 0 To UBound(proj) - 1
            Dim sxpl() As String
            Dim strx As String
            strx = Session(proj(kp))
            strx = strx.Substring(0, strx.Length - 1)
            sxpl = strx.Split(",")
            '  Response.Write(fm.getinfo2("select project_name from tblproject where project_id='" & proj(kp) & "'", Session("con")) & "==><br>" & Session(proj(kp)) & "<br>")

            sumtable &= "<tr style='border:2px solid gray'><td colspan=5><b>" & fm.getinfo2("select project_name from tblproject where project_id='" & proj(kp) & "'", Session("con")) & "</b></td><td></td></tr>"
            For xx As Integer = 0 To UBound(sxpl)
                sumcomp = fm.getinfo2("select sum(pen_c) from payrollx where ref='" & sxpl(xx) & "'", Session("con"))
                sumemp = fm.getinfo2("select sum(pen_e) from payrollx where ref='" & sxpl(xx) & "'", Session("con"))
                groupt += sumcomp + sumemp
                sumtable &= "<tr style='border-bottom:1px solid gray;'><td><a href='rptpayroll.aspx?prid=" & sxpl(xx) & "' target=blank>" & sxpl(xx) & " </a></td><td style='text-align:right'> " & FormatNumber(sumemp, 2, TriState.True) & " </td><td style='text-align:right;'> " & FormatNumber(sumcomp, 2, TriState.True) & " </td><td style='text-align:right;'><b> " & FormatNumber((sumcomp + sumemp), 2, TriState.True) & "</b></td><td></td></tr>"
            Next
            If groupt > 0 Then
                sumtable &= "<tr><td colspan=4>&nbsp;</td><td style='text-align:right;'><u><b>" & FormatNumber(groupt, 2, TriState.True) & "</b></u></td></tr><tr><td colspan=5>&nbsp;</td></tr>"
                totall += groupt
                groupt = 0

            End If

        Next

        sumtable &= "<tr><td colspan=4>&nbsp;</td><td>" & totall + groupt & "</td></tr></table>"
        Response.Write("<div class='blue_btn' id='print2' onclick=" & Chr(34) & "javascript:print('sumprint','netconsult:pension','netconsult','','pension')" & Chr(34) & ">Summery Print</div>")

        Response.Write("<div id='sumprint'>" & sumtable & "</div>")
        print &= "<script>$('#other_pages_total_contrib-1').text('" & FormatNumber(nextpage(2), 2, TriState.True, TriState.True, TriState.UseDefault) & "');$('#other_pages_total_employee_contrib-1').text('" & FormatNumber(nextpage(0), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); $('#other_pages_total_salary-1').text('" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True, TriState.UseDefault) & "');$('#other_pages_total_employer_contrib-1').text('" & FormatNumber(nextpage(1), 2, TriState.True, TriState.True, TriState.UseDefault) & "');" & _
            " $('#months_salaried').text('" & numrows & "');  $('#months_salary').text('" & FormatNumber(firstpagesum(3), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); $('#months_employee_contrib').text('" & FormatNumber(firstpagesum(0), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); $('#months_employer_contrib').text('" & FormatNumber(firstpagesum(1), 2, TriState.True, TriState.True, TriState.UseDefault) & "');  $('#months_total_contrib').text('" & FormatNumber(firstpagesum(2), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); " & _
         " $('#total_salary-1').text('(line 20)" & FormatNumber(firstpagesum(3), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); $('#total_employee_contrib-1').text('(line 30)" & FormatNumber(firstpagesum(0), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); $('#total_employer_contrib-1').text('(line 40)" & FormatNumber(firstpagesum(1), 2, TriState.True, TriState.True, TriState.UseDefault) & "');  $('#total_contrib-1').text('" & FormatNumber(firstpagesum(2), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); " & _
        "</script>"

        Response.Write("<div class='blue_btn' id='print' onclick=" & Chr(34) & "javascript:print('bigprintx','netconsult:pension','netconsult','','pension')" & Chr(34) & ">Print</div>")


        Dim loc As String = Server.MapPath("download") & "\pension(" & Now.Ticks.ToString & ").txt"
        loc = loc.Replace("\", "/")
        'Response.Write(loc)
        File.WriteAllText(loc, print)
        Response.Write(" <div class='clickexp blue_btn' style=' float:left;' onclick=" & Chr(34) & "javascript:exportx('pensionrpt(" & Now.Ticks.ToString & ")','xls','" & loc & "','export','2;3');" & Chr(34) & " >" & _
    "<img src='images/png/excel.png' height='28px' style='float:left;' alt='excel' /> Export to Excel</div>")
        Response.Write(" </div>")
        Response.Write("<div id=bigprintx>" & print & "</div>")

        'makerow(arrproject)

        '  Response.Write("<br>" & dt.convert_to_ethx(pd1) & ">>>>><<<<<" & pd1 & "<br>")
        Response.Write(jslast(ethcal(1), ethcal(2), tpg))
        '  Response.Write(pd1 & "<br>" & pd2 & "<br>" & mksql())
    End Function
    Function makerow(ByVal ref() As String)
        Dim proj() As String
        Dim emptid As String = ""
        Dim projn As String = ""
        Dim spl() As String = {""}
        ' Response.Write("<br>" & UBound(ref) & "<br>")
        For k As Integer = 0 To UBound(ref)
            '  Response.Write(ref(k) & "<br>")
            emptid = fm.getinfo2("select emptid from payrollx where ref='" & ref(k) & "'", Session("con"))
            If emptid <> "None" Then
                proj = fm.getproj(emptid, Request.Form("pd1"), Request.Form("pd2"), Session("con"))
                ' spl = projn.Split("|")
                If fm.searcharray(spl, proj(1)) = False Then
                    ' ReDim Preserve proj(UBound(proj) + 1)
                    ReDim Preserve spl(UBound(spl) + 1)
                    spl(UBound(spl) - 1) = proj(1)
                    Session(proj(1)) = ref(k) & ","
                Else
                    Session(proj(1)) = ref(k) & ","

                End If


            End If
        Next
        Dim pen, epen, sal, sumpen, sumtotal As Double

        sumtotal = 0
        Dim whr As String
        Dim rref As String = ""
        For k As Integer = 0 To UBound(spl)
            Response.Write(k & ") " & spl(k) & "<br>----------------------------------------------------------------</br>")
            If IsError(spl(k)) = False Then


                Response.Write(Session(spl(k)) & "<br>")
                Dim spx() As String
                Try
                    rref = Session(spl(k)).substring(0, Session(spl(k)).length - 1)
                Catch ex As Exception
                    Response.Write(rref & "errrorrorororo")
                End Try


                spx = rref.Split(",")
                If (spx.Length <= 1) Then
                    whr = " ref='" & rref & "'"
                Else
                    whr = " ref in(" & rref & ")"

                End If

                pen = 0
                epen = 0
                sal = 0
                sumpen = 0
                pen = fm.getinfo2("select sum(pen_c) from payrollx where " & whr, Session("con"))
                epen = fm.getinfo2("select sum(pen_e) from payrollx where " & whr, Session("con"))
                sal = fm.getinfo2("select sum(b_sal) from payrollx where  " & whr, Session("con"))
                sumpen = pen + epen
                sumtotal += sumpen
                Response.Write(pen.ToString & " + " & epen.ToString & "===" & sumpen.ToString & "<br>-------------------------------------------------------------------------------------------<br>")
            End If

        Next
        Response.Write("Total " & sumtotal & "<br>-------------------------------------------------------------------------------------------<br>")

    End Function
    Function allincludeunpaid()
        pd1 = dt.convert_to_ethx(Request.Form("pd1"))
        pd2 = dt.convert_to_ethx(Request.Form("pd2"))
        pe = fm.getinfo2("select p_rate_empr from emp_pen_rate where start_date<'" & Request.Form("pd1") & "' order by id desc", Session("con"))
        pc = fm.getinfo2("select p_rate_empee from emp_pen_rate where start_date<'" & Request.Form("pd1") & "' order by id desc", Session("con"))
        Dim nextpage() As Double = {0, 0, 0, 0}
        Dim onpage() As Double = {0, 0, 0, 0}
        Dim firstpagesum() As Double = {0, 0, 0, 0}
        Dim jv As String = "var suffix_list = new Array("
        Dim pen, epen, sal, sump As Double
        Dim db As New dbclass
        Dim rs As DataTableReader
        Dim numpages As Integer
        Dim firstpage As Integer = 6
        Dim perpage As Integer = 20
        Dim numrows As Integer
        Dim tpg, pgno As Integer
        Dim emptid, rowid As String
        Dim ri As Integer
        Dim print As String = ""
        Dim firstp As String = ""
        Dim empname As String
        Dim cell(12) As String
        Dim sumpen As Double
        Dim tin, hdate As String
        Dim empid As String
        Dim ethcal() As String = pd1.Split(".")
        mname = dt.getmonth(ethcal(1), "amh")
        Dim rtn As String = ""
        Dim sumpenrpt As String = fm.getinfo2("select sum(pen_e) from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "'", Session("con"))


        Dim sumpencrpt As String = fm.getinfo2("select sum(pen_c) from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "'", Session("con"))
        Response.Write("<div class='blue_btn' id='print' onclick=" & Chr(34) & "javascript:print('sumprt','" & Session("conmpany_name") & "','','','pension')" & Chr(34) & ");'>Print</div>")
        Response.Write("<div id='sumprt'><table class='data' border='1'><thead><tr><td colspan=2  class='table_title'>Summery Table</td></tr></thead>" & _
                       "<tbody><tr><td>Pension Employee Contrbution </td><td class='numberx'>" & fm.numdigit(sumpenrpt, 2) & "</td></tr><tr>" & _
                       "<td>Pension Employee Contrbution </td><td class='numberx'>" & fm.numdigit(sumpencrpt, 2) & "</td></tr><tr>" & _
                       "<td>Total Pension Contrbution </td><td class='numberx'>" & fm.numdigit(CDbl(sumpencrpt) + CDbl(sumpenrpt), 2) & "</td></tr></tbody></table></div>")

        Try
            rs = db.dtmake("vwsql", mksql(), Session("con"))
            If rs.HasRows Then
                Dim sql As String = "select count(id) from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 and emptid in(select distinct emptid from payrollx  where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0)"

                numrows = fm.getinfo2("select  ROW_NUMBER() Over(order by emptid asc) as Rowno from payrollx where pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 group by emptid order by rowno desc", Session("con"))
                tpg = Math.Ceiling((numrows - 6) / perpage) + 1
                pgno = 1
                ri = 1
                If tpg > 1 Then


                Else

                End If

                ' Response.Write("<br>" & sql & "<<<<<<<<<<<<br>" & numrows)
                Dim idclause As String
                Response.Write("<div class='blue_btn' id='print' onclick=" & Chr(34) & "javascript:print('bigprint','netconsult:pension','netconsult','','pension')" & Chr(34) & ">Print</div>")
                While rs.Read
                    idclause = ""
                    empid = rs.Item("emp_id")
                    ' Response.Write(empid & "<br>")
                    ' emptid = fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con"))
                    emptid = getids(empid, Request.Form("pd1"), Request.Form("pd2"))

                    If emptid = "" Then
                        idclause = fm.getinfo2("select id from emprec where emp_id='" & empid & "' order by id desc", Session("con"))
                        idclause = "emptid=" & idclause & " and "
                    Else
                        idclause = " emptid in(" & emptid & ") and "
                    End If

                    ' Response.Write(empid)
                    hdate = fm.getinfo2("select hire_date from emprec where emp_id='" & empid & "' order by id desc", Session("con"))
                    ' Response.Write("<br>select hire_date from emprec where id in(" & emptid & ") order by desc")
                    hdate = Format(CDate(hdate), "MM/dd/yyyy")
                    ' empname = fm.getfullname(rs.Item("emp_id"), Session("con"))

                    pen = fm.getinfo2("select sum(pen_c) from payrollx where " & idclause & "  pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0", Session("con"))
                    epen = fm.getinfo2("select sum(pen_e) from payrollx where " & idclause & "  pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0", Session("con"))
                    sal = fm.getinfo2("select sum(b_sal) from payrollx where  " & idclause & " pddate between '" & Request.Form("pd1") & "' and '" & Request.Form("pd2") & "' and pen_e>0 ", Session("con"))

                    sumpen = pen + epen
                    tin = fm.getinfo2("select emp_tin from emp_static_info where  emp_id='" & empid & "'", Session("con"))




                    If pgno = 1 Then
                        If ri = 1 Then
                            print = "<div class='a4_landscape'>" & _
    first_page_title(pgno) & tabletitle()
                            firstp = print
                        End If
                        If ri Mod 7 = 0 Then
                            ' firstpagesum = onpage

                            firstpagesum(0) = onpage(0)

                            firstpagesum(1) = onpage(1)

                            firstpagesum(2) = onpage(2)

                            firstpagesum(3) = onpage(3)

                            print &= "  </tr> <tr>" & Chr(13) & _
               " <td colspan='4' align='right'>ከአባሪ ቅጾች ፣ የመጣ ድምር</td><td><input type='text' class='table_box_right' id='other_pages_total_salary-" & pgno & "' size='10'></td><td><input type='text' class='table_box_right' id='other_pages_total_employee_contrib-" & pgno & "' size='10'></td><td><input type='text' class='table_box_right' id='other_pages_total_employer_contrib-" & pgno & "' size='10'></td><td><input type='text' class='table_box_right' id='other_pages_total_contrib-" & pgno & "'></td>" & _
           " </tr><tr>" & Chr(13)
                            print &= "     <tr> <td colspan='4' align='right'>on page ድምር</td><td><input type='text' class='table_box_right' id='onpagetotal_salary-" & pgno & "' size='10'  value='" & FormatNumber(onpage(3), 2, TriState.True, TriState.True) & "'  /></td><td><input type='text' class='table_box_right' id='onpagetotal_employee_contrib-" & pgno & "' size='10'  value='" & FormatNumber(onpage(0), 2, TriState.True, TriState.True) & "' /></td><td><input type='text' class='table_box_right' id='onpagetotal_employer_contrib-" & pgno & "' size='10'  value='" & FormatNumber(onpage(1), 2, TriState.True, TriState.True) & "' /></td><td><input type='text' class='table_box_right' id='onpagetotal_contrib-" & pgno & "'  value='" & FormatNumber(onpage(2), 2, TriState.True, TriState.True) & "'/></td></tr>" & Chr(13)

                            print &= "      <td colspan='4' align='right'>ድምር</td><td align='center'><em>(line 20)<span class='sumall1'>" & onpage(3) & "</span></em><input type='hidden' id='total_salary-" & pgno & "'></td><td align='right'><em>(line 30)<span class='sumall2'> " & firstpagesum(3) & "</span></em><input type='hidden' id='total_employee_contrib-" & pgno & "'></td><td align='center'><em>(line 40)</em><input type='hidden' id='total_employer_contrib-" & pgno & "'></td><td align='center'><em>(line 50)</em><input type='hidden' id='total_contrib-" & pgno & "'><input type='hidden' id='count-" & pgno & "'></td>" & _
          "  </tr>" & Chr(13)

                            onpage(0) = 0
                            onpage(1) = 0
                            onpage(2) = 0
                            onpage(3) = 0
                            print &= "</tbody></table></div>"
                            print &= first_footer()
                            print &= "</div><div id='page_separator'></div>"
                            print &= "<div class='a4_landscape'>"
                            pgno = 2
                            print &= other_page_title(pgno, tpg) & tabletitle()
                            ' Response.Write(firstpagesum(0))


                        End If
                    End If
                    If pgno > 1 Then
                        If (ri - 6) Mod 23 = 0 Then

                            print &= "  </tr> <tr>" & _
               " <td colspan='4' align='right'>ከአባሪ ቅጾች ፣ የመጣ ድምር</td><td><input type='text' class='table_box_right' id='other_pages_total_salary-" & pgno & "' size='10' value='" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True) & "'></td><td><input type='text' class='table_box_right' id='other_pages_total_employee_contrib-" & pgno & "' size='10' value='" & FormatNumber(nextpage(0), 2, TriState.True, TriState.True) & "'></td><td><input type='text' class='table_box_right' id='other_pages_total_employer_contrib-" & pgno & "' size='10' value='" & FormatNumber(nextpage(1), 2, TriState.True, TriState.True) & "'></td><td><input type='text' class='table_box_right' id='other_pages_total_contrib-" & pgno & "' value='" & FormatNumber(nextpage(2), 2, TriState.True, TriState.True) & "'></td>" & _
           " </tr>" & Chr(13)
                            nextpage(0) = nextpage(0) + onpage(0)
                            nextpage(1) = nextpage(1) + onpage(1)
                            nextpage(2) = nextpage(2) + onpage(2)
                            nextpage(3) = nextpage(3) + onpage(3)

                            print &= "     <tr> <td colspan='4' align='right'>on page ድምር</td><td><input type='text' class='table_box_right' id='onpagetotal_salary-" & pgno & "' size='10'  value='" & FormatNumber(onpage(3), 2, TriState.True, TriState.True) & "'  /></td><td><input type='text' class='table_box_right' id='onpagetotal_employee_contrib-" & pgno & "' size='10'  value='" & FormatNumber(onpage(0), 2, TriState.True, TriState.True) & "' /></td><td><input type='text' class='table_box_right' id='onpagetotal_employer_contrib-" & pgno & "' size='10'  value='" & FormatNumber(onpage(1), 2, TriState.True, TriState.True) & "' /></td><td><input type='text' class='table_box_right' id='onpagetotal_contrib-" & pgno & "'  value='" & FormatNumber(onpage(2), 2, TriState.True, TriState.True) & "'/></td></tr>" & Chr(13)

                            print &= "     <tr> <td colspan='4' align='right'>ድምር</td><td><input type='text' class='table_box_right' id='total_salary-" & pgno & "' size='10'  value='" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True) & "'  /></td><td><input type='text' class='table_box_right' id='total_employee_contrib-" & pgno & "' size='10'  value='" & FormatNumber(nextpage(0), 2, TriState.True, TriState.True) & "' /></td><td><input type='text' class='table_box_right' id='total_employer_contrib-" & pgno & "' size='10'  value='" & FormatNumber(nextpage(1), 2, TriState.True, TriState.True) & "' /></td><td><input type='text' class='table_box_right' id='total_contrib-" & pgno & "'  value='" & FormatNumber(nextpage(2), 2, TriState.True, TriState.True) & "'/><input type='hidden' id='count-" & pgno & "' /></td></tr>" & Chr(13)
                            onpage(0) = 0
                            onpage(1) = 0
                            onpage(2) = 0
                            onpage(3) = 0
                            print &= "</tbody></table>"
                            print &= other_footer()
                            print &= "</div></div><div id='page_separator'></div>"
                            pgno += 1
                            print &= "<div class='a4_landscape'>"
                            print &= other_page_title(pgno, tpg) & tabletitle()
                        End If
                    End If
                    onpage(0) = epen + onpage(0)
                    onpage(1) = pen + onpage(1)
                    onpage(2) = sumpen + onpage(2)
                    onpage(3) = sal + onpage(3)

                    print &= "  <tr style='line-height: 9px;' class='repeatable-data-row'>"
                    print &= "  <td><span id='row_no-" & pgno & ri & "'>" & ri & "</span></td>" & Chr(13) & _
    "<td><input type='text' class='table_box_right' id='tin-" & pgno & ri & "' value='" & tin & "'></td>" & Chr(13) & _
    "<td><input type='text' class='table_box_left ethiopic' id='name-" & pgno & ri & "' style='width: 260px;' jgeez-index='0' value='" & Chr(13) & _
             fm.getfullname(empid, Session("con")) & "'></td>" & Chr(13) & _
    "<td><input type='text' class='table_box_left date hasCalendarsPicker' id='hire_date-" & pgno & ri & "' value='" & hdate & "'></td>" & Chr(13) & _
    "       <td><input type='text' class='table_box_right calc' id='salary-" & pgno & ri & "' data='-" & pgno & "' value='" & FormatNumber(sal, 2, TriState.True, TriState.True, TriState.UseDefault) & "'></td>" & _
    "      <td><input type='text' class='table_box_right' id='employee_contrib-" & pgno & ri & "' value='" & FormatNumber(epen, 2, TriState.True, TriState.True, TriState.UseDefault) & "'></td>" & _
    "     <td><input type='text' class='table_box_right' id='employer_contrib-" & pgno & ri & "' value='" & FormatNumber(pen, 2, TriState.True, TriState.True, TriState.UseDefault) & "'></td>" & _
    "    <td><input type='text' class='table_box_right' id='total_contrib-" & pgno & ri & "' value='" & FormatNumber(sumpen, 2, TriState.True, TriState.True, TriState.UseDefault) & "'></td>" & _
     "   <td><input type='text' class='table_box_right' readonly='readonly'></td></tr>" & Chr(13) & Chr(13)

                    '               print &= "<td>" & ri & "</td>" & _
                    '      "<td>" & tin & "</td><td><input type='text' class='table_box_left ethiopic' id='name-b2' style='width: 260px;' jgeez-index='1' value='" & _
                    '      fm.getfullname(empid, Session("con")) & "'></td>" & _
                    '    "<td><span class='table_box_left date hasCalendarsPicker' id='hire_date-" & pgno & ri & "'>" & hdate & "</span></td> " & _
                    '   "<td><span class='table_box_right calc' id='salary-" & pgno & ri & "' data='" & ri & "'>" & sal & "<span</td>" & _
                    '  "<td><span class='table_box_right' id='employee_contrib-" & pgno & ri & "'>" & epen & "</span></td>" & _
                    ' "<td><span class='table_box_right' id='employer_contrib-" & pgno & ri & "'> " & pen & "</span></td>" & _
                    '"<td><span class='table_box_right' id='total_contrib-" & pgno & ri & "'>" & sumpen & "</span></td>" & _
                    '"<td><span class='table_box_right' readonly='readonly'></span></td> </tr>"
                    ri = ri + 1

                End While
            End If
            rs.Close()
        Catch ex As Exception
            Response.Write(mksql() & "ON errror" & ex.ToString)
        End Try

        While (ri - 6) Mod 23 > 0
            print &= "  <tr style='line-height: 9px;' class='repeatable-data-row'>"
            print &= "  <td><span id='row_no-" & pgno & ri & "'></span></td>" & Chr(13) & _
"<td><span class='table_box_right' id='tin-" & pgno & ri & "' >&nbsp;</span></td>" & _
"<td><span class='table_box_left ethiopic' id='name-" & pgno & ri & "' style='width: 260px;' jgeez-index='0' value=''>&nbsp;</span></td>" & _
"<td><span class='table_box_left date hasCalendarsPicker' id='hire_date-" & pgno & ri & "'>&nbsp;</span></td>" & _
"       <td><span class='table_box_right calc' id='salary-" & pgno & ri & "' data='-" & pgno & "'>&nbsp;</span></td>" & _
"      <td><span class='table_box_right' id='employee_contrib-" & pgno & ri & "' >&nbsp;</span></td>" & Chr(13) & _
"     <td><span class='table_box_right' id='employer_contrib-" & pgno & ri & "'>&nbsp;</span></td>" & Chr(13) & _
"    <td><span class='table_box_right' id='total_contrib-" & pgno & ri & "'>&nbsp;</span></td>" & _
"   <td><span class='table_box_right' readonly='readonly'>&nbsp;</span></td></tr>" & Chr(13)
            ri = ri + 1
        End While
        print &= "   <tr>" & _
           " <td colspan='4' align='right'>ከአባሪ ቅጾች ፣ የመጣ ድምር</td><td><span class='table_box_right' id='other_pages_total_salary-" & pgno & "'>" & nextpage(3) & "</span></td><td><span class='table_box_right' id='other_pages_total_employee_contrib-" & pgno & "' size='10' >" & nextpage(0) & "</span></td><td><span class='table_box_right' id='other_pages_total_employer_contrib-" & pgno & "' >" & nextpage(1) & "</span></td><td><span class='table_box_right' id='other_pages_total_contrib-" & pgno & "'>" & nextpage(2) & "</span></td>" & Chr(13) & _
       " </tr>" & Chr(13)
        nextpage(0) = nextpage(0) + onpage(0)
        nextpage(1) = nextpage(1) + onpage(1)
        nextpage(2) = nextpage(2) + onpage(2)
        nextpage(3) = nextpage(3) + onpage(3)
        print &= "     <tr> <td colspan='4' align='right'>ድምር</td><td><span class='table_box_right' id='total_salary-" & pgno & "'/>" & nextpage(3) & "</span></td><td><span class='table_box_right' id='total_employee_contrib-" & pgno & "'/>" & nextpage(0) & "</span></td><td><span class='table_box_right' id='total_employer_contrib-" & pgno & "'/>" & nextpage(1) & "</span></td><td><span class='table_box_right' id='total_contrib-" & pgno & "'/>" & nextpage(2) & "</span><input type='hidden' id='count-" & pgno & "' /></td></tr>" & Chr(13)

        print &= "</tbody></table>"
        print &= other_footer()
        print &= "</div></div>" & Chr(13)
        'Response.Write("<br>Emp Cont:=" & firstpagesum(0) & " + " & nextpage(0) & "<br>")
        firstpagesum(0) = firstpagesum(0) + nextpage(0)
        'Response.Write("<br>comp cont:=" & firstpagesum(1) & " + " & nextpage(1) & "<br>")
        firstpagesum(1) = firstpagesum(1) + nextpage(1)
        'Response.Write("<br>sum cont:=" & firstpagesum(2) & " + " & nextpage(2) & "<br>")
        firstpagesum(2) = firstpagesum(2) + nextpage(2)
        'Response.Write("<br>Bsal:=" & firstpagesum(3) & " + " & nextpage(3) & "<br>")
        firstpagesum(3) = firstpagesum(3) + nextpage(3)

        print &= "<script>$('#other_pages_total_contrib-1').val('" & FormatNumber(nextpage(2), 2, TriState.True, TriState.True, TriState.UseDefault) & "');$('#other_pages_total_employee_contrib-1').val('" & FormatNumber(nextpage(0), 2, TriState.True, TriState.True, TriState.UseDefault) & "');$('#other_pages_total_salary-1').val('" & FormatNumber(nextpage(3), 2, TriState.True, TriState.True, TriState.UseDefault) & "');$('#other_pages_total_employer_contrib-1').val('" & FormatNumber(nextpage(1), 2, TriState.True, TriState.True, TriState.UseDefault) & "');" & _
            " $('#months_salaried').text('" & numrows & "');  $('#months_salary').text('" & FormatNumber(firstpagesum(3), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); $('#months_employee_contrib').text('" & FormatNumber(firstpagesum(0), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); $('#months_employer_contrib').text('" & FormatNumber(firstpagesum(1), 2, TriState.True, TriState.True, TriState.UseDefault) & "');  $('#months_total_contrib').text('" & FormatNumber(firstpagesum(2), 2, TriState.True, TriState.True, TriState.UseDefault) & "'); </script>"
        Response.Write("<div id=bigprint>" & print & "</div>")


        '  Response.Write("<br>" & dt.convert_to_ethx(pd1) & ">>>>><<<<<" & pd1 & "<br>")
        Response.Write(jslast(ethcal(1), ethcal(2), tpg))
        '  Response.Write(pd1 & "<br>" & pd2 & "<br>" & mksql())
    End Function
    Function getids(ByVal emp_id As String, ByVal pd1 As String, ByVal pd2 As String)
        Dim sql As String = "select id from emprec where emp_id='" & emp_id & "' and (hire_date between '" & pd1 & "' and '" & pd2 & "' or end_date between '" & pd1 & "' and '" & pd2 & "')"
        Dim rs As DataTableReader
        Dim ds As New dbclass
        Dim rtn As String = ""
        rs = ds.dtmake("sqlv", sql, Session("con"))
        If rs.HasRows Then
            While rs.Read
                rtn &= rs.Item("id") & ","

            End While
        End If
        If rtn <> "" Then
            rtn = rtn.Substring(0, rtn.Length - 1)

        End If
        ' Response.Write("<br>+++rtn" & rtn & "<br>" & sql)
        Return rtn

    End Function
    Function listactiveon(ByVal pd2 As Date)


    End Function
    Function listpaid(ByVal pd1 As Date, ByVal pd2 As Date)

    End Function
    Public Function getjavalist2(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection, ByVal sp As String, ByVal spx As String) As String
        Dim db As New dbclass
        Dim sql As String = "select  " & dis & " from " & dbtabl
        '  Response.Write(sql & "<br>")
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
                retstr &= Chr(34)
                retstr &= spx
                'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
            End While

        End If
        dt.Close()

        retstr = retstr.Substring(0, retstr.Length - 1)
        Return retstr
    End Function
    Function jslast(ByVal month As Integer, ByVal year As Integer, ByVal nopages As Integer)
        Dim rtn As String = "<script>"
        rtn &= " $(document).ready(function () {"

        rtn &= "$('.print_month').text('" & dt.getmonth(month, "amh") & "');"
        '  rtn &= "alert($('.print_month').text());"
        rtn &= "$('.year').text('" & year & "');"
        rtn &= "$('.page_count').text('" & nopages & "');"
        rtn &= " });</script>"
        Return rtn
    End Function
End Class
