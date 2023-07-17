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


Namespace Kirsoft.contract
    Public Class cn_mail
        Public Function mail_send(ByVal sender As String, ByVal tom As String, ByVal ccto As String, ByVal subject As String, ByVal body As String, ByVal footer As String)

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
                        File.AppendAllText(p2 & "backup.log", File.ReadAllText(pathx))
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

            Dim flinf As FileInfo
            Dim cmd As New SqlCommand
            Dim p2 As String = ""
            ' cmdb.Connection
            ' MsgBox(sql)
            p2 = pathx & "\log\"
            pathx = pathx & "\log\logkir.txt"

            With cmd
                .Connection = conx
                .CommandType = CommandType.Text
                .CommandText = sql.ToString

                Try
                    If conx.State = ConnectionState.Closed Then
                        conx.Open()
                    End If
                    flinf = New FileInfo(pathx)
                    If CInt(flinf.Length) / 1024000 > 2 Then
                        File.AppendAllText(p2 & "backup.log", File.ReadAllText(pathx))
                        File.WriteAllText(pathx, "")

                    End If

                    rowaff = .ExecuteNonQuery()
                    File.AppendAllText(pathx.ToString, Chr(13) & Now & Chr(13) & sql.ToString & "=>" & HttpContext.Current.Session("emp_iid").ToString)
                Catch ex As Exception

                    rowaff = ex.ToString
                    ' writeerro(ex.ToString)
                End Try
            End With
            cmd = Nothing
            Return rowaff
        End Function

        Function writeerro(ByVal err As String)

            If File.Exists(Path.GetFullPath("employee") & "\error.log") = False Then
                File.WriteAllText("error.log", err)
            Else
                err &= err & File.ReadAllText("error.log")
                File.WriteAllText("error.log", err)
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
End Namespace
