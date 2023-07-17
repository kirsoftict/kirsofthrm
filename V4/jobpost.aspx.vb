Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class jobpost
    Inherits System.Web.UI.Page
    Public keyp As String = ""
    Public idx As String = ""
    Public msgout As String = ""
    Public webpost As String = ""

    Function iniit()

        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        Else
            keyp = "save"
        End If
        Dim idx As String = ""
        idx = Request.Form("id")
        Dim msg As String = ""
        Dim msg2 As String = ""
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim locid As Object
        Dim fl As New file_list
        Dim sec As New k_security
        Dim sql As String = ""
        Dim flg As Integer = 0
        Dim flg2 As Integer = 0
        ' Response.Write(sc.d_encryption("zewde@123"))
        Dim rd As String = ""

        Dim tbl As String = ""
        Dim key As String = ""
        Dim keyval As String = ""
        Dim sqlx As String = ""
        tbl = Request.QueryString("tbl")
        Dim sd As String
        Dim ed, rgd As String
        'Response.Write(sql)
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                Response.Write(Request.QueryString("tbl") & "<br>")
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                    ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    Response.Write("iiiiiiiiiiiiiin<br>")
                    sql = makeup(tbl, Request.Form, Session("con"), "job_no", Request.Form("job_no"))
                    Response.Write(sql)
                    sqlx = sql
                    flg = dbx.save(sql, Session("con"), Session("path"))
                    ' locid = fm.getinfo2("select id from emp_sal_info where date_end is null and emptid=" & Session("emptid") & " order by id desc", Session("con"))
                    ' Response.Write(sql)
                    webpost = "<form name='frmhidden' id='frmhidden' method='post' action='http://jobs.netconsult.com.et/addnew.php'  target='_blank'><input type='hidden' name='sql' id='sql'/></form>"
                    sd = (Format(CDate(Request.Form("start_date")), "yyyy-M-d"))
                    ed = (Format(CDate(Request.Form("end_date")), "yyyy-M-d"))
                    rgd = (Format(CDate(Request.Form("date_reg")), "yyyy-M-d HH:mm:ss "))
                    Response.Write("<br>....<br>" & Request.Form("date_reg") & "<br>,........<br>" & rgd & ",,,,,,,,,,,,,,,,,,,,,,,,<br>")
                    sqlx = Replace(sqlx, Request.Form("start_date"), sd)
                    sqlx = Replace(sqlx, Request.Form("end_date"), ed)
                    sqlx = Replace(sqlx, Request.Form("date_reg"), rgd)
                    webpost &= "<script>$('#sql').val('" & Kir_StrToHex(sqlx) & "');  $('#frmhidden').submit();</script>"
                    If flg = 1 Then
                        msgout = "Data Updated"

                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                    'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '

                    flg = dbx.save(sql, Session("con"), Session("path"))

                    'Response.Write(sql)
                    If flg = 1 Then
                        msgout = "Data deleted"
                    End If
                ElseIf Request.QueryString("task") = "save" Then
                    'Response.Write(Request.Form("job_no"))
                    ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")
                    locid = fm.getinfo2("select id from tblhrjobs where job_no ='" & Request.Form("job_no") & "' order by id desc", Session("con"))
                    Response.Write(locid)
                   
                    'flg = dbx.save(sql, session("con"),session("path"))
                    'Response.Write(flg)
                    If sql = "" Then


                        ' Response.Write(dd.ToString)
                        'Response.Write(locid.ToString)
                        If IsNumeric(locid) Then
                            '   sql &= "update tblhrjobs set job_no='job-" & GenerateString(4) & "' where id=" & locid.ToString & Chr(13)
                            ' Response.Write(sql)
                            msgout = "can't save this please resave it"
                            ' dbx.save(sql, session("con"),session("path")) 
                            sqlx = makest(tbl, Request.Form, Session("con"), key) & Chr(13)
                            webpost = "<form name='frmhidden' id='frmhidden' method='post' action='http://jobs.netconsult.com.et/addnew.php'  target='_blank'><input type='hidden' name='sql' id='sql'/></form>"
                            sd = (Format(CDate(Request.Form("start_date")), "yyyy-M-d"))
                            ed = (Format(CDate(Request.Form("end_date")), "yyyy-M-d"))
                            rgd = (Format(CDate(Request.Form("date_reg")), "yyyy-M-d HH:mm:ss"))
                            Response.Write("<br>....." & rgd & "<br>")
                            sqlx = Replace(sqlx, Request.Form("start_date"), sd)
                            sqlx = Replace(sqlx, Request.Form("end_date"), ed)
                            sqlx = Replace(sqlx, Request.Form("date_reg"), rgd)
                            webpost &= "<script>$('#sql').val('" & Kir_StrToHex(sqlx) & "');  $('#frmhidden').submit();</script>"
                        Else

                            sql = "Begin Transaction " & Session("emp_iid") & Chr(13)
                            sqlx = makest(tbl, Request.Form, Session("con"), key) & Chr(13)

                            sql &= sqlx
                            '  Response.Write(sql)
                            Try
                                flg = dbx.excutes(sql, Session("con"), Session("path"))
                                Dim rpt() As String = {""}
                                If CInt(flg) > 0 Then
                                    dbx.excutes("Commit Transaction " & Session("emp_iid"), Session("con"), Session("path"))
                                    msgout = "Data is Saved"
                                    ' thisinc()
                                    ' Response.Write(msg)


                                    webpost = "<form name='frmhidden' id='frmhidden' method='post' action='http://jobs.netconsult.com.et/addnew.php'  target='_blank'><input type='hidden' name='sql' id='sql'/></form>"
                                    sd = (Format(CDate(Request.Form("start_date")), "yyyy-M-d"))
                                    ed = (Format(CDate(Request.Form("end_date")), "yyyy-M-d"))
                                    rgd = (Format(CDate(Request.Form("date_reg")), "yyyy-M-d HH:mm:ss"))
                                    sqlx = Replace(sqlx, Request.Form("start_date"), sd)
                                    sqlx = Replace(sqlx, Request.Form("end_date"), ed)
                                    sqlx = Replace(sqlx, Request.Form("date_reg"), rgd)
                                    webpost &= "<script>$('#sql').val('" & Kir_StrToHex(sqlx) & "');  $('#frmhidden').submit();</script>"

                                    flg = 1
                                Else
                                    dbx.excutes("Rollback Transaction " & Session("emp_iid"), Session("con"), Session("path"))
                                End If
                            Catch ex As Exception
                                Response.Write(ex.ToString & "<br>" & sql)
                            End Try

                        End If

                    End If

                End If
            End If
        'MsgBox(rd)

        ' sql = db.makest(tbl, Request.QueryString, session("con"), key)

        If flg <> 1 Then
            ' Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
        End If


        End If

    End Function
    Public Function pageaddnew(ByVal tbl As String) As String
        Response.Write(tbl)
        If Session("con").state = ConnectionState.Closed Then
            Session("con").open()
        End If
        Dim sql As String
        sql = "select * from " & tbl
        If tbl <> "" Then
            Dim cm As New menubuilder
            Dim rs As SqlDataReader
            Dim formx As String = ""
            rs = cm.cmdx(Session("con"), sql)
            Dim icount, i As Integer
            icount = rs.FieldCount
            formx = "<form method='post' id='frmpage_" & tbl & "' name='frmpage_" & tbl & "' enctype='multipart/form-data' action='dbadmin.aspx?updated=on&dtable=" & tbl & "'>"
            formx = formx & "<table>"
            For i = 0 To icount - 1
                If i = 0 Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                    "<input type='hidden' name='" & rs.GetName(i) & "' value=''><b></b></td></tr>"
                ElseIf LCase(rs.GetDataTypeName(i)) = "text" Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                         "<textarea class='txtarea' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & "'>"
                    formx = formx & "</textarea></td></tr>"


                ElseIf LCase(rs.GetDataTypeName(i)) = "datetime" Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                                              "<input class='txt' type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                                              "' value='" & Today.Month & "/" & Today.Day & "/" & Today.Year & "'>" & _
                                                        "<script type='text/javascript'>" & _
     "$(function () {" & _
        " $(" & Chr(34) & "#" & rs.GetName(i) & Chr(34) & ").datepicker({ minDate: " & Chr(34) & "-20Y" & Chr(34) & ", maxDate: " & Chr(34) & "+20Y" & Chr(34) & " });" & _
        "   });" & _
        "$('#" & rs.GetName(i) & "').datepicker( 'option','dateFormat','mm/dd/yy');" & _
 "</script>" & _
                                                             " </td></tr> "
                Else
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                                            "<input class='txt' type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                                       "' value=''></td></tr>"


                End If
            Next
            formx = formx & "<tr><td><button type='button' name='btnupdate' value='update' onclick=" & Chr(34) & "javascript:document.frmpage_" & tbl & ".submit();" & Chr(34) & " >Save</button></table></form>"
            'formx = formx & "<br />Uploads: images video"
            rs.Close()
            cm = Nothing
            Return formx
        Else
            Return ""
        End If
    End Function
    Public Function GenerateString(ByVal ss As Integer) As String

        Dim xCharArray() As Char = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray
        Dim xNoArray() As Char = "0123456789".ToCharArray
        Dim xGenerator As System.Random = New System.Random()
        Dim xStr As String = String.Empty

        While xStr.Length < ss

            If xGenerator.Next(0, 2) = 0 Then
                xStr &= xCharArray(xGenerator.Next(0, xCharArray.Length))
            Else
                xStr &= xNoArray(xGenerator.Next(0, xNoArray.Length))
            End If

        End While

        Return xStr


    End Function
    Public Function getjavalist(ByVal dbtabl As String, ByVal dis As String, ByVal conx As SqlConnection) As String
        Dim db As New dbclass
        Dim sql As String = "select " & dis & " from " & dbtabl
        Dim dt As DataTableReader
        Dim retstr As String = ""
        dt = db.dtmake("dbtbl" & Today.ToLongTimeString, sql, conx)
        Dim disp() As String
        Dim dispn As Integer = 0
        disp = dis.Split(",")
        Dim optdis As String = ""
       
        If dt.HasRows = True Then
            While dt.Read

                If dt.IsDBNull(0) = False Then
                    If LCase(dt.GetDataTypeName(0)) = "string" Then
                        retstr &= Chr(34) & dt.Item(0).trim & Chr(34) & " "
                    Else
                        retstr &= Chr(34) & dt.Item(0) & Chr(34) & " "
                    End If
                End If


                retstr &= ","
                'retstr &= Chr(34) & dt.Item(0) & Chr(34) & ","
            End While
            retstr &= Chr(34) & "xx" & Chr(34)
        End If
        Return retstr
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
        ' Response.Write("<br>" & rst & "<br>")
        For i = 0 To rsp.FieldCount - 1
            ' Response.Write("<br>" & rsp.GetName(i) & "===" & obj(rsp.GetName(i)) & "<br>")
            If obj(rsp.GetName(i)) <> "" Then
                '  Response.Write("<br>" & rsp.GetName(i))
                fd &= rsp.GetName(i) & ","
                If LCase(rsp.GetDataTypeName(i)) = "datetime" Then
                    val2 = val2 & "'" & obj(rsp.GetName(i)) & "',"
                    val &= "'" & obj(rsp.GetName(i)) & "',"
                ElseIf LCase(rsp.GetDataTypeName(i)) = "string" Then
                    val2 = val2 & "'" & Replace(obj(rsp.GetName(i)), "'", "\'") & "',"
                    val &= "'" & Replace(obj(rsp.GetName(i)).trim, "'", "\'") & "',"
                Else
                    val2 = val2 & "'" & obj(rsp.GetName(i)) & "',"
                    val &= obj(rsp.GetName(i)) & ","
                End If

            End If
            'MsgBox(rsp.GetName(i) & "..." & rsp.GetDataTypeName(i))
        Next
        Try


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
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
        rsp.Close()
        ds.Dispose()
        dt.Dispose()
        rsp = Nothing
        Return returnval
    End Function
    Public Function Kir_StrToHex(ByVal Data As String) As String
        Dim sVal As String
        Dim sHex As String = ""
        If String.IsNullOrEmpty(Data) = False Then
            While Data.Length > 0
                ' Response.Write("<br>" & Data.Substring(0, 1) & "|" & Strings.Asc(Data.Substring(0, 1).ToString()) & "|")
                sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
                Response.Write(sVal)
                If (sVal.Length = 1) Then
                    sVal = "⌠" & sVal & "⌡"
                End If
                Data = Data.Substring(1, Data.Length - 1)
                sHex = sHex & sVal
            End While
        End If

        Return sHex
    End Function
    Public Function makeup(ByVal st As String, ByVal obj As Object, ByVal conp As SqlConnection, ByVal where_field As String, ByVal where_val As String) As String
        Dim rt As DataTableReader
        Dim dt As New dbclass
        Dim sec As New k_security
        Dim rtval As String = ""
        Dim sql As String = "select * from " & st & " where " & where_field & "='" & where_val & "'"
        Dim val As String = ""
        Response.Write(sec.Kir_HexToStr(obj("hidpass")))
        Response.Write("<br>" & sql & "<br>")
        val = "update " & st & " set "
        rt = dt.dtmake("mk" & Today.ToString, sql, conp)
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

End Class
