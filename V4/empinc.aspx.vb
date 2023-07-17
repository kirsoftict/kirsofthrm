Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class empinc
    Inherits System.Web.UI.Page
    Public fm As New formMaker
    Public dbx As New dbclass
    Public idx As String
    Public keyp As String
    Public flg As Integer
    Public flg2 As Integer
    Public rd As String
    Public sssql As String
    Public msg As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim keyp As String = ""

        Dim emp_id, emptid As String
        emptid = ""
        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        Else
            keyp = "save"
        End If

        idx = Request.QueryString("id")
        Dim msg As String = ""

        Dim sql As String = ""

        ' Response.Write(sc.d_encryption("zewde@123"))
        rd = ""

        Dim tbl As String = ""
        Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                    ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                    flg = dbx.save(sql, Session("con"), Session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                        msg = "Data Updated"
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
                    Dim arrname() As String
                    sql = ""
                    If Request.QueryString.HasKeys = True Then
                        arrname = Request.QueryString("vname").Split(" ")
                        'Response.Write(arrnafm.Length.ToString)
                        If arrname.Length >= 3 Then
                            sql = "Select * from emp_static_info where first_name='" & arrname(0) & "' and middle_name='" & arrname(1) & "' and last_name='" & arrname(2) & "'"

                        End If
                    End If
                    Dim dtt As DataTableReader
                    emptid = ""
                    emp_id = ""
                    If sql <> "" Then
                        dtt = dbx.dtmake("tblstatic", sql, Session("con"))

                        If dtt.HasRows Then
                            dtt.Read()
                            emp_id = dtt.Item("emp_id")
                            ' Response.Write("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and '" & CDate(Request.Form("inc_date")).AddMonths(-1) & _
                            '              "' between hire_date and isnull(end_date,'" & Today.ToShortDateString & "') order by id")
                            Try
                                emptid = CInt(fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and '" & CDate(Request.Form("inc_date")).AddMonths(-1) & _
                                 "' between hire_date and isnull(end_date,'" & Today.ToShortDateString & "') order by id", Session("con")))

                            Catch ex As Exception
                                Response.Write(ex.ToString)
                            End Try
                            ' Response.Write("emptid===>" & emptid.ToString)
                        End If
                    End If
                    If emp_id <> "" And emptid <> "" Then
                        Dim pda As Object
                        If Request.QueryString("paid_date") = "" Then
                            pda = "Null"
                        Else
                            pda = "'" & Request.QueryString("paid_date") & "'"
                        End If
                        sql = "insert into emp_inc(emptid,emp_id,inc_date,reason,amt,amt2,paid_date,who_reg,date_reg) Values('" & emptid & "','" & emp_id & "','" & Request.QueryString("inc_date") & _
                        "','" & Request.QueryString("reason") & "','" & Request.QueryString("amt") & "','" & Request.QueryString("amt2") & "'," & pda.ToString & ",'" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')"
                       ' Response.Write("<br>" & sql)
                        Try
                            flg = dbx.excutes(sql, Session("con"), Session("path"))
                        Catch ex As Exception
                            flg = 2
                            Response.Write(Session("path") & "<br>" & ex.ToString)
                        End Try

                    Else
                        msg = "Sorry! data is not saved! check the nafm."
                    End If
                    ' sql = dbx.makest(tbl, Request.QueryString, session("con"), key)

                    '
                    If flg = 1 Then
                        msg = "Data Saved"
                    Else
                        msg = "Data is not Saved flag:" & flg
                    End If
                    End If

                'MsgBox(rd)

                ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
                ' Response.Write(msg)



            End If
        End If
        If emptid.ToString = "" Then
            emptid = "0"
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


    End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Function edit_del_list_wname_inc(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim fm As New formMaker
        Dim sec As New k_security
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        Dim arr() As String = {""}
        rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        If dt.HasRows = True Then
            Dim color As String = "E3EAEB"
            Dim empid As String
            Dim datex As Boolean = False
            rtstr &= "<form id='frms' method='post' name='frms' action=''>"
            While dt.Read


                If fm.searcharray(arr, sec.Str2ToHex(dt.Item("paid_date"))) = False Then

                    Dim arrbound As Integer = UBound(arr)
                    ReDim Preserve arr(arrbound + 1)
                    arr(arrbound) = sec.Str2ToHex(dt.Item("paid_date"))
                    ' rtstr &= fm.searcharray(arr, dt.Item("paid_date")).ToString
                    If datex = True Then
                        rtstr &= "</table></div>"
                        datex = False
                    End If
                End If

                If datex = False Then
                    rtstr &= "<br><div id='" & sec.Str2ToHex(dt.Item("paid_date")) & "' class='collapsed' style='height:25px;width:900px; background-color:blue; cursor:pointer;' onclick=" & Chr(34) & "javascript:showHideSubMenu(this,'tbl" & _
                                              sec.Str2ToHex(dt.Item("paid_date")) & "')" & Chr(34) & _
                                              ">" & dt.Item("paid_date") & "</div><div id='tbl" & sec.Str2ToHex(dt.Item("paid_date")) & "' style='display:none;'><table  cellspacing='0' cellpadding='0'>" & _
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
                    rtstr = rtstr & "</tr>"
                    datex = True

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
                rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "</td>"
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
                    ElseIf dt.GetDataTypeName(k) = "Double" Then
                        If dt.IsDBNull(k) = False Then
                            rtstr = rtstr & "<td  style='padding-right:20px;text-align:right;'>" & FormatNumber(dt.Item(k), 2, TriState.True, TriState.True, TriState.True).ToString & "</td>"
                        Else
                            rtstr = rtstr & "<td  style='padding-right:20px;text-align:right'>0.00</td>"
                        End If
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
                       " emp_inc.id, emp_inc.emptid, emp_static_info.first_name, emp_static_info.middle_name, emp_static_info.last_name," & _
                        "emp_inc.inc_date, emp_inc.reason, emp_inc.amt, emp_inc.paid_date, emp_inc.paidref,emp_inc.amt2 " & _
"FROM emp_static_info INNER JOIN " & _
                        "emprec ON emp_static_info.emp_id = emprec.emp_id INNER JOIN " & _
                        "emp_inc ON emprec.id = emp_inc.emptid " & _
"ORDER BY emp_inc.id desc"
        End If
        Return sssql
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
        sssql = "SELECT DISTINCT " & _
                   " emp_inc.id, emp_inc.emptid, emp_static_info.first_name, emp_static_info.middle_name, emp_static_info.last_name," & _
                    "emp_inc.inc_date, emp_inc.reason, emp_inc.amt, emp_inc.paid_date, emp_inc.paidref,emp_inc.amt2 " & _
"FROM emp_static_info INNER JOIN " & _
                    "emprec ON emp_static_info.emp_id = emprec.emp_id INNER JOIN " & _
                    "emp_inc ON emprec.id = emp_inc.emptid " & _
"ORDER BY emp_inc.id desc"
        'Response.Write(sssql)
        Return sql
        If IsPostBack = True Then
            Response.Write("<script>$('#viewg').css({ display:'inline'});</script>")

        End If
    End Function


    Function paidlistp(ByVal pproj As String, ByVal pdate1 As Date, ByVal pdate2 As Date) As Nullable
        ' Response.Write(pproj)
        If pproj <> "" Then

        End If
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
    Public Function edit_del_list2_wname(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String, ByVal floc As String, ByVal btnedit As Boolean, ByVal btndel As Boolean, ByVal btnupload As Boolean, ByVal fc As Boolean) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass

        Dim dt As DataTableReader
        Dim hdr() As String
        Dim fl As New file_list
        hdr = heading.Split(",")
        Dim i As Integer
        rtstr = "<script type='text/javascript'>function goclicked(whr,id){  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
        rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & _
        "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        If heading <> "" Then
            For i = 0 To hdr.Length - 1
                rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
            Next
        Else
            For i = 1 To dt.FieldCount - 1
                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

            Next
        End If

        rtstr = rtstr & "</tr>"

        If dt.HasRows = True Then
            Dim color As String = "E3EAEB"
            Dim empid As String
            While dt.Read
                empid = dt.Item("emptid")
                If color <> "#E3EAEB" Then
                    color = "#E3EAEB"
                Else
                    color = "#fefefe"
                End If

                rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>"
                ' rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "</td>"

                If btnedit = True Then
                    rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> "
                Else
                    rtstr = rtstr & "<td  style='padding-right:20px;'><img src='images/png/editx.png' title='Edit Disabled'  /></td> "

                End If
                If btndel = True Then
                    rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>"

                Else
                    rtstr = rtstr & "<td  style='padding-right:20px;'><img src='images/png/deletex.png' title='Delete Canceled' style='curser:pointr;' /></td>"

                End If
                rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "</td>"

                For k As Integer = 1 To dt.FieldCount - 2
                    rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                Next
                If btnupload = True Then
                    rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/upload.png' title='upload' style='curser:pointr;' alt='upload'  width='20' height='20' />UPLOAD FILES</td>"

                End If
                If fc = True Then
                    Dim fileno As Integer
                    Dim lfloc As String

                    lfloc = floc & "\" & dt.Item("id") & "\"
                    fileno = fl.fileno(lfloc)
                    If fileno > 0 Then
                        rtstr = rtstr & "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goupload(" & Chr(34) & "upload" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & fileno & "</td>"
                    Else
                        rtstr = rtstr & "<td title='" & lfloc & "'>No Upload Files</td>"
                    End If
                End If
                rtstr = rtstr & "</tr>"
            End While
        Else
            rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>"
        End If
        rtstr = rtstr & "</table></form>"
        dt.Close()
        dc = Nothing
        Return rtstr

    End Function
    Public Function edit_del_list_wname(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        'Response.Write(sql)
        Dim fm As New formMaker
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        rtstr = "<script type='text/javascript'>function goclicked(whr,id){ alert('called');  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());$('#frms').submit();}</script>"
        rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & _
        "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'><td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>"
        dt = dc.dtmake(tbl & Today.ToLongDateString, sql, con)
        If heading <> "" Then
            For i = 0 To hdr.Length - 1
                rtstr = rtstr & "<td style='padding-right:20px;'>" & hdr(i) & "</td>"
            Next
        Else
            For i = 1 To dt.FieldCount - 1
                rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.GetName(i) & "</td>"

            Next
        End If

        rtstr = rtstr & "</tr>"

        If dt.HasRows = True Then
            Dim color As String = "E3EAEB"
            Dim empid As String
            While dt.Read
                empid = dt.Item("emptid")
                If color <> "#E3EAEB" Then
                    color = "#E3EAEB"
                Else
                    color = "#fefefe"
                End If
                rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>"
                rtstr &= "<td>" & fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & empid, con), con) & "</td>"
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
        rtstr = rtstr & "</table></form>"
        dt.Close()
        dc = Nothing
        Return rtstr

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
                        ' ds = rs.Item("date_from")
                        de = sdate
                    Else
                        did = fm.getinfo2("select resign_date from emp_resign where emptid='" & rs.Item("emptid") & "'", con)

                        If IsDate(did) Then
                            If CDate(did) <> rs.Item("date_end") Then
                                de = rs.Item("date_end")
                                ' Response.Write("<br>emptid" & rs.Item("emptid"))
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
                    '                   Response.Write("  ====         " & rs.Item("emptid") & fm.getfullname(rs.Item("emp_id"), con) & "<br>")
                    If ds <= sdate And sdate <= de Then
                        rtn &= "'" & rs.Item("emptid") & "',"

                    Else

                    End If


                End While
            Catch ex As Exception
                '              Response.Write(ex.ToString)
                rtn = "'',"
                fm.exception_hand(ex)
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
End Class
