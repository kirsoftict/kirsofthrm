Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class empot
    Inherits System.Web.UI.Page
    Function bee()
        If Session("username") = "" Then
            Response.Redirect("logout.aspx")
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
        Dim flg As Integer = 0
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
                    Dim dtt As DataTableReader
                    If sql <> "" Then
                        dtt = dbx.dtmake("tblstatic", sql, Session("con"))

                        If dtt.HasRows Then
                            dtt.Read()
                            emp_id = dtt.Item("emp_id")
                            emptid = CInt(fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and (end_date is null or end_date='') order by id desc", Session("con")))
                            salary = dbx.getsal(emptid, date2, Session("con"))
                            'salary = dbx.getsal(emptid, Session("con"))
                            hr = CDbl(salary(0)) / 200.67


                        End If
                        Dim amt, rate As Double

                        Dim factored As String
                        Dim timedif As Double

                        For i As Integer = 0 To arrx.Length - 1
                            If Request.QueryString(arrx(i)) <> "" Then

                                Select Case arrx(i)
                                    Case "Reg"
                                        rate = 1.25
                                        factored = "Reg"
                                        timedif = Request.QueryString(arrx(i))
                                    Case "Nig"
                                        rate = 1.5
                                        timedif = Request.QueryString(arrx(i))
                                        factored = "Nig"
                                    Case "WE"
                                        rate = 2
                                        timedif = Request.QueryString(arrx(i))
                                        factored = "WE"
                                    Case "HD"
                                        rate = 2.5
                                        timedif = Request.QueryString(arrx(i))
                                        factored = "HD"
                                End Select


                            End If
                        Next


                        amt = hr * rate * timedif
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

                        flg = dbx.save(sql, session("con"), session("path"))
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
                    flg = dbx.save(sql, session("con"), session("path"))

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
                            res = fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and end_date is null order by id desc", Session("con"))
                            If IsNumeric(res) Then
                                emptid = res
                            Else
                                emptid = fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' order by id desc", Session("con"))
                            End If
                            ' Response.Write("select basic_salary from emp_sal_info where emp_id='" & emp_id & "' and date_start<='" & date2 & "' order by id desc")

                            salary = dbx.getsal(emptid, date1, Session("con"))
                            ' Response.Write(salary(0))
                            ' salary = dbx.getsal(emptid, Session("con"))
                            'salary = 3800
                            hr = CDbl(salary(0)) / 200.67


                        End If
                        Dim amt, rate As Double

                        'rate = CDbl(Request.QueryString("rate"))
                        Dim timedif As Double
                        ' timedif = Request.QueryString("time_diff")
                        'Dim arrx() As String = {"Reg", "Nig", "WE", "HD"}
                        dtt.Close()
                        amt = hr * rate * timedif
                        sql = "BEGIN TRANSACTION" & Chr(13)
                        For i As Integer = 0 To arrx.Length - 1
                            If Request.QueryString(arrx(i)) <> "" Then
                                factored = ""
                                Select Case arrx(i)
                                    Case "Reg"
                                        rate = 1.25
                                        factored = "Reg"
                                        timedif = Request.QueryString(arrx(i))
                                    Case "Nig"
                                        rate = 1.5
                                        timedif = Request.QueryString(arrx(i))
                                        factored = "Nig"
                                    Case "WE"
                                        rate = 2
                                        timedif = Request.QueryString(arrx(i))
                                        factored = "WE"
                                    Case "HD"
                                        rate = 2.5
                                        timedif = Request.QueryString(arrx(i))
                                        factored = "HD"
                                End Select
                                amt = hr * rate * timedif
                                If Request.QueryString("datepaid") <> "" Then
                                    sql &= "insert into emp_ot(emp_id,emptid,ot_date,time_diff,rate,factored,amt,datepaid,who_reg,date_reg) " & _
                            "values('" & emp_id & "','" & emptid & "','" & date1.ToShortDateString & "','" & timedif & "','" & rate & _
                            "','" & factored & "','" & Math.Round(amt, 2).ToString & "','" & date2 & "','" & Request.QueryString("who_reg") & _
                            "','" & Request.QueryString("date_reg") & "')" & Chr(13)
                                Else
                                    sql &= "insert into emp_ot(emp_id,emptid,ot_date,time_diff,rate,factored,amt,who_reg,date_reg) " & _
                            "values('" & emp_id & "','" & emptid & "','" & date1.ToShortDateString & "','" & timedif & "','" & rate & _
                            "','" & factored & "','" & Math.Round(amt, 2).ToString & "','" & Request.QueryString("who_reg") & _
                            "','" & Request.QueryString("date_reg") & "')" & Chr(13)
                                End If
                                'Response.Write(sql & "<br>")
                            End If
                        Next
                        sql &= "COMMIT" & Chr(13)
                        ' Response.Write(sql)
                        flg = dbx.excutes(sql, Session("con"), session("path"))
                        'Response.Write(flg1)
                        'flg = 1
                        'flg1 = ds.save("commit", Session("con"))
                        If IsNumeric(flg) = True Then
                            ' Response.Write(flg1)
                            If flg <= 0 Then
                                dbx.save("rollback", Session("con"), Session("path"))
                                Response.Write("data is not saved")
                            Else
                                Response.Write("Data Saved")
                            End If

                        Else
                            If flg.ToString <> "-1" Then
                                dbx.save("rollback", Session("con"), Session("path"))
                                Response.Write("data is not saved")
                            Else
                                Response.Write("Data(s) is/are saved")
                            End If

                        End If


                        '  flg = dbx.save(sql, session("con"),session("path"))
                        ' Response.Write(flg)
                        If flg = 1 Then
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
End Class
