Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.Web
Partial Class chartemp
    Inherits System.Web.UI.Page
    Function mkjava(ByVal yr As Integer, ByVal title As String)
        Dim dbx As New dbclass
        Dim rs As DataTableReader
        Dim rtn(5) As String
        Dim rand As New Random
        Dim crclr As String = ""
        Dim cc As String = ""
        Dim mkch As Boolean = False

        rs = dbx.dtmake("chrt", "SELECT MONTH(hire_date) AS month, COUNT(id) AS noemp FROM emprec WHERE (YEAR(hire_date) = '" & yr & "') GROUP BY MONTH(hire_date)", Session("con"))
        Dim id As Integer = 1
        If rs.HasRows Then
            mkch = True
            rtn(0) &= "['Months',"
            While rs.Read
                rtn(0) &= rs.Item(1) & ","
                crclr = "'#"
                For j As Integer = 1 To 3
                    cc = Conversion.Hex(rand.Next(CInt(255)))


                    If cc.Length < 2 Then
                        cc = "0" & cc
                    End If
                    crclr &= cc
                Next
                ' crclr = "'#" & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & "'"
                crclr &= "'"
                rtn(1) &= "myChart.setLegendForBar(" & id & ",'" & MonthName(rs.Item(0), True) & "');" & Chr(13)
                rtn(2) &= "myChart.setBarColor( " & crclr & ", " & id & ");" & Chr(13)
                id = id + 1
                rand.Next(CInt(255))
                rand.Next(CInt(255))
                ' rgb(rand.Next(255), rand.Next(255), rand.Next(255))
            End While
            rtn(3) = title & " " & yr.ToString
            rtn(0) = rtn(0).Substring(0, rtn(0).Length - 1) & "]"
            ' rtn(1) = rtn(1).Substring(0, rtn(1).
        Else
            rs.Close()
            rs = dbx.dtmake("chrt", "SELECT MONTH(hire_date) AS month, COUNT(id) AS noemp FROM emprec WHERE (YEAR(hire_date) = '" & (yr - 1) & "') GROUP BY MONTH(hire_date)", Session("con"))
            id = 1
            yr = yr - 1
            If rs.HasRows Then
                mkch = True
                rtn(0) &= "['Months',"
                While rs.Read
                    rtn(0) &= rs.Item(1) & ","
                    crclr = "'#"
                    For j As Integer = 1 To 3
                        cc = Conversion.Hex(rand.Next(CInt(255)))
                        Response.Write(cc & "<br>")

                        If cc.Length < 2 Then
                            cc = "0" & cc
                        End If
                        crclr &= cc
                    Next
                    ' crclr = "'#" & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & "'"
                    crclr &= "'"
                    rtn(1) &= "myChart.setLegendForBar(" & id & ",'" & MonthName(rs.Item(0), True) & "');" & Chr(13)
                    rtn(2) &= "myChart.setBarColor( " & crclr & ", " & id & ");" & Chr(13)
                    id = id + 1
                    rand.Next(CInt(255))
                    rand.Next(CInt(255))
                    ' rgb(rand.Next(255), rand.Next(255), rand.Next(255))
                End While
                rtn(3) = title & " " & yr.ToString
                rtn(0) = rtn(0).Substring(0, rtn(0).Length - 1) & "]"
            End If
        End If
        rs.Close()
        rtn(4) = mkch
        Return rtn
    End Function
    Function getmin()
        Dim fm As New formMaker
        Dim rt As String
        rt = fm.getinfo2("select min(year(hire_date)) from emprec", Session("con"))
        Return rt
    End Function
    Function piedata()
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs1, rs2 As DataTableReader
        Dim sql As String = ""
        Dim rtn As String = ""
        Dim fld() As String = {""}
        Dim k As Integer = 0
        Dim rr(3) As String
        Dim selcolor() As String = {"#0000ff", "#31A03C", "#9EDE0F", "#D8527C", "#994C3A", "#CE2A45", "#8C7DE7", "#3F0C88", "#E023BA", "#19203B"}
        rr(0) = ""
        rr(1) = ""
        rr(2) = ""
        rs1 = dbs.dtmake("rec", "select emp_id from emprec where end_date is null", Session("con"))
        If rs1.HasRows Then


            While rs1.Read
                rtn = ""
                sql = "SELECT emp_education.qualification " & _
"FROM            emp_education INNER JOIN " & _
                         "tblqualification ON emp_education.qualification = tblqualification.qualification INNER JOIN " & _
                        " emp_static_info ON emp_education.emp_id = emp_static_info.emp_id " & _
                        "where emp_education.emp_id='" & rs1.Item(0) & "' " & _
"ORDER BY emp_education.emp_id,tblqualification.hr"
                ' Response.Write(sql & "<br>")
                rtn = fm.getinfo2(sql, Session("con"))
                'Response.Write(fm.getinfo2("select first_name from emp_static_info where emp_id='" & rs1.Item(0) & "'", Session("con")))

                If rtn <> "None" Then
                    'Response.Write(rtn & "<br>")
                    If IsError(Session(rtn)) = True Then
                        Session(rtn) = 1
                        ' Response.Write(" ....")
                        If fm.searcharray(fld, rtn) = False Then
                            ReDim Preserve fld(k + 1)
                            fld(k) = rtn
                            k = k + 1
                        End If
                    Else
                        If fm.searcharray(fld, rtn, False) = False Then
                            ReDim Preserve fld(k + 1)
                            fld(k) = rtn
                            Session(rtn) = 0
                            k = k + 1
                        End If
                        Session(rtn) = Session(rtn) + 1
                    End If
                Else
                    ' Response.Write(sql & rtn & "<br>")
                End If
            End While
            Dim darr, lbl, color, crclr As String
            Dim ch As Integer = 65
            crclr = ""
            If fld.Length > 0 Then

                darr = ""
                color = "["
                lbl = ""
                Dim rand As New Random


                Dim cc As String

                For i As Integer = 0 To fld.Length - 1

                    If IsError(Session(fld(i))) = False Then
                        If String.IsNullOrEmpty(fld(i)) = False Then
                            'Response.Write(fld(i) & "==" & Session(fld(i)) & "<br>")
                            darr &= "['" & Chr(ch) & "'," & Session(fld(i)) & "],"
                            If (i > (selcolor.Length - 1)) Then

                                crclr = "'#"
                                For j As Integer = 1 To 3
                                    cc = Conversion.Hex(rand.Next(CInt(255))) 'making random color


                                    If cc.Length < 2 Then
                                        cc = "0" & cc
                                    End If
                                    crclr &= cc
                                Next
                                ' crclr = "'#" & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & Conversion.Hex(rand.Next(255)).ToString & "'"
                                crclr &= "'"
                                ' Response.Write(crclr & "<br>")
                                color &= crclr & ","
                            Else
                                crclr = "'" & selcolor(i) & "'"
                                color &= "'" & selcolor(i) & "',"
                            End If
                            lbl &= "myChart.setLegend(" & crclr & ",'" & fld(i) & "(" & Session(fld(i)) & ")');" & Chr(13)
                            rand.Next()
                            rand.Next()
                            rand.Next()
                            rand.Next()
                            'rand.Next()
                            ch += 1
                        End If
                    End If
                Next
                darr = darr.Substring(0, darr.Length - 1)
                color = color.Substring(0, color.Length - 1)
                color &= "]"
            End If
            rr(0) = darr
            rr(1) = color
            rr(2) = lbl
        End If


        rs1.Close()
        dbs = Nothing
        fm = Nothing
        Return rr
    End Function
    Function chartmk(ByVal ctp As String)
        Dim rtntxt As String = ""
        Select Case ctp
            Case "bar"

                If Request.QueryString("yr") = "on" Then

                    rtntxt &= "<div><form method='post' action='' id='frmth' name='frmth'>"
                    rtntxt &= "year:<select name='year' id='year' onchange=" & Chr(34) & "javascript:document.frmth.submit();" & Chr(34) & ">"
                    rtntxt &= "<option value=''>Select</option>"
                    For i As Integer = Today.Year To CInt(getmin()) Step -1
                        rtntxt &= "<option value='" & i.ToString & "'>" & i.ToString & "</option>"
                    Next
                    rtntxt &= " </select>"

                    rtntxt &= " </form></div>"
                    rtntxt &= "  <div id='graph' style='float:left;'>Loading graph...</div>"

                    Dim rt() As String
                    If Request.Form("year") <> "" Then
                        rt = mkjava(Request.Form("year"), "Employment")
                    Else
                        rt = mkjava(Today.Year, "Employment")
                    End If

                    If rt(4) = True Then
                        rtntxt &= "<script type='text/javascript'>" & Chr(13)
                        rtntxt &= "var myData = new Array(" & (rt(0)) & ");" & Chr(13)
                        rtntxt &= "var myChart = new JSChart('graph', 'bar');" & Chr(13)
                        rtntxt &= "myChart.setDataArray(myData);" & Chr(13)
                        rtntxt &= "myChart.setTitle('" & (rt(3)) & "');" & Chr(13)
                        rtntxt &= "myChart.setTitleColor('#8E8E8E');" & Chr(13)
                        rtntxt &= " myChart.setAxisNameX('');" & Chr(13)
                        rtntxt &= "myChart.setAxisNameY('Number');" & Chr(13)
                        rtntxt &= "myChart.setAxisNameFontSize(16);" & Chr(13)
                        rtntxt &= "myChart.setAxisNameColor('#999');" & Chr(13)
                        rtntxt &= "myChart.setAxisValuesAngle(30);" & Chr(13)
                        rtntxt &= "myChart.setAxisValuesColor('#777');" & Chr(13)
                        rtntxt &= "myChart.setAxisColor('#B5B5B5');" & Chr(13)
                        rtntxt &= "myChart.setAxisWidth(4);" & Chr(13)
                        rtntxt &= "myChart.setBarValuesColor('#2F6D99');" & Chr(13)
                        rtntxt &= "myChart.setAxisPaddingTop(60);" & Chr(13)
                        rtntxt &= "myChart.setAxisPaddingBottom(60);" & Chr(13)
                        rtntxt &= "myChart.setAxisPaddingLeft(45);" & Chr(13)
                        rtntxt &= "myChart.setTitleFontSize(11);" & Chr(13)
                        rtntxt &= (rt(2)) & Chr(13)
                        rtntxt &= "myChart.setBarBorderWidth(0);" & Chr(13)
                        rtntxt &= "myChart.setBarSpacingRatio(50);" & Chr(13)
                        rtntxt &= "myChart.setLegendPadding(10);" & Chr(13)

                        'rtntxt &= "myChart.setBarOpacity(0.1);" & Chr(13)
                        rtntxt &= "myChart.setFlagRadius(6);" & Chr(13)
                        'rtntxt &= "myChart.setTooltip(['North America', 'Click me', 1], callback);" & chr(13)
                        rtntxt &= "myChart.setTooltipPosition('nw');" & Chr(13)
                        rtntxt &= "myChart.setTooltipOffset(3);" & Chr(13)
                        rtntxt &= "myChart.setLegendShow(true);" & Chr(13)
                        rtntxt &= "myChart.setLegendPosition('right');" & Chr(13)
                        rtntxt &= (rt(1)) & Chr(13)
                        rtntxt &= "myChart.setSize(700, 500);" & Chr(13)
                        rtntxt &= "myChart.setGridColor('#C6C6C6');" & Chr(13)
                        rtntxt &= "myChart.draw();" & Chr(13)


                        rtntxt &= "</script>" & Chr(13)
                    End If

                Else

                    rtntxt &= "<div id='graph'>Loading graph...</div>" & Chr(13)

                    Dim rt() As String
                    rt = mkjava(Today.Year, "Employment")
                    If rt(4) = True Then
                        rtntxt &= "<script type='text/javascript'>" & Chr(13)
                        rtntxt &= "var myData = new Array(" & (rt(0)) & ");" & Chr(13)
                        rtntxt &= " var myChart = new JSChart('graph', 'bar');" & Chr(13)
                        rtntxt &= "myChart.setDataArray(myData);" & Chr(13)
                        rtntxt &= "myChart.setTitle('" & (rt(3)) & "');" & Chr(13)
                        rtntxt &= "myChart.setTitleColor('#8E8E8E');" & Chr(13)
                        rtntxt &= "myChart.setAxisNameX('');" & Chr(13)
                        rtntxt &= "myChart.setAxisNameY('Number');" & Chr(13)
                        rtntxt &= "myChart.setAxisNameFontSize(16);" & Chr(13)
                        rtntxt &= "myChart.setAxisNameColor('#999');" & Chr(13)
                        rtntxt &= "myChart.setAxisValuesAngle(30);" & Chr(13)
                        rtntxt &= "myChart.setAxisValuesColor('#777');" & Chr(13)
                        rtntxt &= "myChart.setAxisColor('#B5B5B5');" & Chr(13)
                        rtntxt &= "myChart.setAxisWidth(1);" & Chr(13)
                        rtntxt &= "myChart.setBarValuesColor('#2F6D99');" & Chr(13)
                        rtntxt &= "myChart.setAxisPaddingTop(60);" & Chr(13)
                        rtntxt &= "myChart.setAxisPaddingBottom(60);" & Chr(13)
                        rtntxt &= "myChart.setAxisPaddingLeft(45);" & Chr(13)
                        rtntxt &= "myChart.setTitleFontSize(11);" & Chr(13)
                        rtntxt &= rt(2) & Chr(13)
                        rtntxt &= "myChart.setBarBorderWidth(0);" & Chr(13)
                        rtntxt &= "myChart.setBarSpacingRatio(50);" & Chr(13)
                        'rtntxt &= "myChart.setBarOpacity(0.9);" & Chr(13)
                        rtntxt &= "myChart.setFlagRadius(6);" & Chr(13)
                        'rtntxt &= "myChart.setTooltip(['North America', 'Click me', 1], callback);" & chr(13)
                        rtntxt &= "myChart.setTooltipPosition('nw');" & Chr(13)
                        rtntxt &= "myChart.setTooltipOffset(3);" & Chr(13)
                        rtntxt &= "myChart.setLegendShow(true);" & Chr(13)
                        rtntxt &= "myChart.setLegendPosition('right top');" & Chr(13)
                        rtntxt &= (rt(1)) & Chr(13)
                        rtntxt &= "myChart.setSize(400, 300);" & Chr(13)
                        rtntxt &= "myChart.setGridColor('#C6C6C6');" & Chr(13)
                        rtntxt &= "myChart.draw();" & Chr(13)

                        rtntxt &= "</script>"

                    End If
                End If
            Case "pie"
                If Request.QueryString("yr") = "on" Then
                    Dim pice() As String
                    pice = piedata()
                    rtntxt &= "  <div id='piec'>Loading...</div> " & Chr(13)
                    Dim rt() As String
                    rt = mkjava(Today.Year, "Employment")

                    If rt(4) = True Then
                        rtntxt &= "<script type='text/javascript'>" & Chr(13)
                        rtntxt &= "myChart = new JSChart('piec', 'pie');" & Chr(13)
                        rtntxt &= "myChart.setDataArray([" & (pice(0)) & "]);" & Chr(13)
                        rtntxt &= "myChart.colorize(" & (pice(1)) & ");" & Chr(13)
                        rtntxt &= "myChart.setSize(700, 700);" & Chr(13)
                        rtntxt &= "myChart.setTitle('Employee Qualification');" & Chr(13)
                        rtntxt &= "myChart.setTitlePosition('center');" & Chr(13)
                        rtntxt &= "myChart.setTitleFontFamily('Calibri');" & Chr(13)
                        rtntxt &= "myChart.setTitleFontSize(12);" & Chr(13)
                        rtntxt &= "myChart.setTitleColor('#0F0F0F');" & Chr(13)
                        rtntxt &= "myChart.setPieRadius(180);" & Chr(13)
                        rtntxt &= "myChart.setPieValuesColor('#FFFFFF');" & Chr(13)
                        rtntxt &= "myChart.setPieValuesFontSize(8);" & Chr(13)
                        rtntxt &= "myChart.setPiePosition(200, 250);" & Chr(13)
                        rtntxt &= "myChart.setShowXValues(false);" & Chr(13)
                        rtntxt &= (pice(2)) & Chr(13)
                        rtntxt &= "myChart.setLegendFontFamily('Calibri');" & Chr(13)
                        rtntxt &= "myChart.setLegendFontSize(10);" & Chr(13)
                        rtntxt &= "myChart.setLegendColor('#000000');" & Chr(13)
                        rtntxt &= "myChart.setLegendPosition(400,100);" & Chr(13)
                        rtntxt &= "myChart.setLegendShow(true);" & Chr(13)
                        rtntxt &= "myChart.setPieAngle(0);" & Chr(13)
                        rtntxt &= "myChart.set3D(false);" & Chr(13)
                        rtntxt &= "myChart.draw();" & Chr(13)
                        rtntxt &= "</script>"
                    End If

                Else
                    Dim pice() As String
                    pice = piedata()
                    rtntxt &= "  <div id='piec'>Loading...</div> " & Chr(13)
                    Dim rt() As String
                    rt = mkjava(Today.Year, "Employment")
                    If rt(4) = True Then
                        rtntxt &= "<script type='text/javascript'>" & Chr(13)
                        rtntxt &= "myChart = new JSChart('piec', 'pie');" & Chr(13)
                        rtntxt &= "myChart.setDataArray([" & (pice(0)) & "]);" & Chr(13)
                        rtntxt &= "myChart.colorize(" & (pice(1)) & ");" & Chr(13)
                        rtntxt &= "myChart.setSize(450, 300);" & Chr(13)
                        rtntxt &= "myChart.setTitle('Employee Qualification');" & Chr(13)
                        rtntxt &= "myChart.setTitlePosition('center');" & Chr(13)
                        rtntxt &= "myChart.setTitleFontFamily('Calibri');" & Chr(13)
                        rtntxt &= "myChart.setTitleFontSize(12);" & Chr(13)
                        rtntxt &= "myChart.setTitleColor('#0F0F0F');" & Chr(13)
                        rtntxt &= "myChart.setPieRadius(100);" & Chr(13)
                        rtntxt &= "myChart.setPieValuesColor('#FFFFFF');" & Chr(13)
                        rtntxt &= "myChart.setPieValuesFontSize(8);" & Chr(13)
                        rtntxt &= "myChart.setPiePosition(180, 135);" & Chr(13)
                        rtntxt &= "myChart.setShowXValues(false);" & Chr(13)
                        rtntxt &= (pice(2)) & Chr(13)
                        rtntxt &= "myChart.setLegendFontFamily('Calibri');" & Chr(13)
                        rtntxt &= "myChart.setLegendFontSize(8);" & Chr(13)
                        rtntxt &= "myChart.setLegendColor('#000000');" & Chr(13)
                        rtntxt &= "myChart.setLegendPosition(300, 80);" & Chr(13)
                        rtntxt &= "myChart.setLegendShow(true);" & Chr(13)
                        rtntxt &= "myChart.setPieAngle(30);" & Chr(13)
                        rtntxt &= "myChart.set3D(false);" & Chr(13)
                        rtntxt &= "myChart.draw();" & Chr(13)
                        rtntxt &= "</script>"
                    End If
                End If

            Case Else

        End Select
        Response.Write(rtntxt)
        'ltrtext.Text = rtntxt
    End Function
    Function rgb(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer)
        Dim sec As New k_security
        Response.Write("<br>" & r.ToString & "," & (g.ToString) & "," & (b.ToString) & "," & Conversion.Hex(255))

    End Function
End Class
