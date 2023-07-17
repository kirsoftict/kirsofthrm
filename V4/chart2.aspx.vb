Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.Web
Imports Microsoft.VisualBasic.Interaction


Partial Class chart2
    Inherits System.Web.UI.Page
    Function makechart()

        'Response.Buffer = True
        'Response.ContentType = "text/xml"

        Dim xdata() As String
        Dim ydata() As String

        xdata = piedatax()
        ydata = piedatay()

        'Connect to the SQL Server Northwind database.
       

        'Build an array for the ProductName field and an array for the ProductSales field.
        
        'Create a new bar chart.
        Dim ChartSpace1, Cht, c As Object
        ChartSpace1 = CreateObject("OWC10.Chartspace")
        c = ChartSpace1.Constants
        Cht = ChartSpace1.Charts.Add
        Cht.Type = c.chChartTypeBarClustered

        'Add the data to the chart.
        Cht.SetData(c.chDimSeriesNames, c.chDataLiteral, "Sales")
        Cht.SetData(c.chDimCategories, c.chDataLiteral, xdata)
        Cht.SeriesCollection(0).SetData(c.chDimValues, c.chDataLiteral, ydata)

        'Format chart elements.
        ChartSpace1.Border.Color = "Rosybrown"
        ChartSpace1.Border.Weight = c.owcLineWeightMedium
        Cht.SeriesCollection(0).Interior.Color = "Rosybrown"
        Cht.PlotArea.Interior.Color = "Wheat"

        'Return the new chart's XML.
        Response.Write(ChartSpace1.XMLData)
        Response.End()

    End Function
    Function piedatax()
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs1, rs2 As DataTableReader
        Dim sql As String = ""
        Dim rtn As String = ""
        Dim fld() As String = {""}
        Dim k As Integer = 0
        Dim rr(3, 1) As String
        Dim xax() As String
        Dim yax() As String
        rs1 = dbs.dtmake("rec", "select emp_id from emprec where end_date is null", Session("con"))
        If rs1.HasRows Then


            While rs1.Read
                rtn = ""
                sql = "SELECT emp_education.qualification " & _
"FROM            emp_education INNER JOIN " & _
                         "tblqualification ON emp_education.qualification = tblqualification.qualification INNER JOIN " & _
                        " emp_static_info ON emp_education.emp_id = emp_static_info.emp_id " & _
                        "where emp_education.emp_id='" & rs1.Item(0) & "' " & _
"ORDER BY emp_education.emp_id, tblqualification.hr"
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
                            ReDim Preserve yax(k + 1)
                            yax(k) = ""
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

            If fld.Length > 0 Then

                darr = ""
                color = "["
                lbl = ""
                Dim rand As New Random(999999999)




                For i As Integer = 0 To fld.Length - 1

                    If IsError(Session(fld(i))) = False Then
                        If String.IsNullOrEmpty(fld(i)) = False Then
                            'Response.Write(fld(i) & "==" & Session(fld(i)) & "<br>")
                            darr &= "['" & Chr(ch) & "'," & Session(fld(i)) & "],"
                            crclr = "'#" & rand.Next().ToString.Substring(0, 6) & "'"
                            color &= crclr & ","
                            lbl &= "myChart.setLegend(" & crclr & ",'" & fld(i) & "');" & Chr(13)

                            ch += 1
                        End If
                    End If
                Next
                darr = darr.Substring(0, darr.Length - 1)
                color = color.Substring(0, color.Length - 1)
                color &= "]"
            End If



        End If


        rs1.Close()
        dbs = Nothing
        fm = Nothing
        Return fld
    End Function
    Function piedatay()
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim rs1, rs2 As DataTableReader
        Dim sql As String = ""
        Dim rtn As String = ""
        Dim fld() As String = {""}
        Dim k As Integer = 0

        Dim xax() As String
        Dim yax() As String
        rs1 = dbs.dtmake("rec", "select emp_id from emprec where end_date is null", Session("con"))
        If rs1.HasRows Then


            While rs1.Read
                rtn = ""
                sql = "SELECT emp_education.qualification " & _
"FROM            emp_education INNER JOIN " & _
                         "tblqualification ON emp_education.qualification = tblqualification.qualification INNER JOIN " & _
                        " emp_static_info ON emp_education.emp_id = emp_static_info.emp_id " & _
                        "where emp_education.emp_id='" & rs1.Item(0) & "' " & _
"ORDER BY emp_education.emp_id, tblqualification.hr"
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
                            ReDim Preserve yax(k + 1)
                            yax(k) = ""
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

            If fld.Length > 0 Then

                darr = ""
                color = "["
                lbl = ""
                Dim rand As New Random(999999999)




                For i As Integer = 0 To fld.Length - 1

                    If IsError(Session(fld(i))) = False Then
                        If String.IsNullOrEmpty(fld(i)) = False Then
                            'Response.Write(fld(i) & "==" & Session(fld(i)) & "<br>")
                            darr &= "['" & Chr(ch) & "'," & Session(fld(i)) & "],"
                            crclr = "'#" & rand.Next().ToString.Substring(0, 6) & "'"
                            color &= crclr & ","
                            lbl &= "myChart.setLegend(" & crclr & ",'" & fld(i) & "');" & Chr(13)
                            yax(i) = Session(fld(i))
                            ch += 1
                        End If
                    End If
                Next
                darr = darr.Substring(0, darr.Length - 1)
                color = color.Substring(0, color.Length - 1)
                color &= "]"
            End If



        End If


        rs1.Close()
        dbs = Nothing
        fm = Nothing
        Return yax
    End Function
End Class
