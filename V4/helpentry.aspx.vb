Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.IO
Partial Class helpentry
    Inherits System.Web.UI.Page
    Public msg As String
    Public keyp As String
    Public idx As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        Else
            keyp = "save"
        End If
        Dim dbx As New dbclass
        Dim sec As New k_security
        Dim idx As String = ""
        idx = Request.QueryString("id")
        'Dim msg As String = ""
        '  Dim dbx As New dbclass
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
        If Request.QueryString.HasKeys = True And Request.QueryString("task") <> "" Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                    'e("<script type='text/javascript'>alert('updating....');</script>")
                    'sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                    ' flg = dbx.save(sql, session("con"),session("path"))
                    '  Response.Write(sql)
                    updatedata()
                ElseIf Request.QueryString("task") = "delete" Then
                    Response.Write("<script type='text/javascript'>//alert('deleting....');</script>")
                    'sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, session("con"), session("path"))

                    ' Response.Write(sql)
                    If flg = 1 Then
                        msg = "Data deleted"
                    End If
                ElseIf Request.QueryString("task") = "save" Then
                    ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")
                    savethis()


                End If
            End If
        End If
    End Sub
    Function savethis()
        Dim sql As String
        Dim sec As New k_security
        Dim orx, hdr, lik, hlp As String
        Dim flg As Integer

        orx = Request.Form("help_order")
        If Request.Form("help_order") = "" Then
            orx = "0"

        End If
        hdr = Request.Form("heading_help")
        lik = Request.Form("helptext")
        hlp = sec.dbStrToHex(Request.Form("helpx"))
        If Request.Form("help_order") = "" Then
            orx = "0"

        End If
        ' sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
        Sql = "insert into tblhelp(help_order,heading_help,helptext,help) values('"
        sql &= orx & "','" & hdr & "','" & lik & "','" & hlp & "')"
        'Response.Write(sql)
        sql = "BEGIN TRANSACTION " & Chr(13) & sql & Chr(13)
        Dim dbx As New dbclass
        flg = dbx.excutes(sql, Session("con"), session("path"))
        If flg > 0 Then
            flg = dbx.excutes("Commit", Session("con"), session("path"))
            If flg = -1 Then
                msg = "Data is Saved"
            Else
                dbx.excutes("RollBack", Session("con"), session("path"))
                msg = "Data is not saved"
            End If
        Else
            dbx.excutes("RollBack", Session("con"), session("path"))


            msg = "Data base doesnt change"
        End If
        Session("con").close()
        Session("con").open()
        '  flg = dbx.save(sql, session("con"),session("path"))
    End Function
    Function updatedata()


        Dim sql As String
        Dim sec As New k_security
        Dim orx, hdr, lik, hlp As String
        Dim flg As Integer

        orx = Request.Form("help_order")
        If Request.Form("help_order") = "" Then
            orx = "0"

        End If
        hdr = Request.Form("heading_help")
        lik = Request.Form("helptext")
        hlp = sec.dbStrToHex(Request.Form("helpx"))
        If Request.Form("help_order") = "" Then
            orx = "0"

        End If
        ' sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
        sql = "update tblhelp set help_order='" & orx & "',heading_help='" & hdr & "',helptext='" & lik & "',help='" & hlp & "' where id=" & Request.QueryString("keyval")
        'Response.Write(sql)
        sql = "BEGIN TRANSACTION " & Chr(13) & sql & Chr(13)
        'Response.Write(sql)
        Dim dbx As New dbclass
        flg = dbx.excutes(sql, Session("con"), session("path"))
        If flg > 0 Then
            flg = dbx.excutes("Commit", Session("con"), session("path"))
            If flg = -1 Then
                msg = "Data is Saved"
            Else
                dbx.excutes("RollBack", Session("con"), session("path"))
                msg = "Data is not saved"
            End If
        Else
            dbx.excutes("RollBack", Session("con"), session("path"))


            msg = "Data base doesnt change"
        End If
        Session("con").close()
        Session("con").open()
        '  flg =
    End Function
    Function whydoc()
        Dim outpx As String
        Dim db As New dbclass
        Dim f As New file_list
        Dim sec As New k_security
        outpx = ""
        If Request.Form.HasKeys = True Then
            For Each p As String In Request.Form
                ' Response.Write(p & "==>" & Request.Form(p) & "<br>")
            Next
        End If
        If Request.QueryString("edit") = "on" Then

        Else
            outpx &= "<form method='post' id='frmtbl_leave_type' name='frmtbl_leave_type' action=''> <table id='inputform'><tr><td>Order</td><td>:</td><td><input type='text' name='help_order' id='help_order' /></td></tr>"
            outpx &= "<tr><td>Header</td><td>:</td><td><input type='text' name='heading_help' id='heading_help' /></td></tr>"
            outpx &= "<tr><td>Page</td><td>:</td><td><select name='helptext' id='helptext' />"

            If Directory.Exists(Session("path")) = True Then
                'Response.Write(Session("path"))
                For Each k As String In Directory.GetFiles(Session("path") & "\")
                    'Response.Write(f.file_ext(k) & "<br>")
                    If LCase(f.file_ext(k)).ToString = ".aspx" Then
                        outpx &= "<option value='" & f.findfilename(k.ToString) & "'>" & f.findfilename(k.ToString) & "</option>"
                    End If
                Next
            End If
            outpx &= "</select></td></tr>"
            outpx &= "<tr><td>Help Text</td><td>:</td><td><textarea name='helpx' id='helpx'>"
            Dim dt As DataTableReader
            If keyp = "update" Then
                dt = db.dtmake("new" & Today.ToLocalTime, "select * from tblhelp where id=" & Request.QueryString("id"), Session("con"))
                If dt.HasRows = True Then
                    dt.Read()
                    outpx &= sec.dbHexToStr(dt.Item("help"))
                End If
            End If
            outpx &= "</textarea></td></tr>"
            outpx &= "<tr><td><input type='button' name='btnSave' id='btnSave' value='Save' /><input type='reset' onclick=" & Chr(34) & "javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" & Chr(34) & "  /></td></tr></table></form>"
            Response.Write(outpx)
            End If
    End Function
    Function edit_del_list_hlp(ByVal tbl As String, ByVal sql As String, ByVal heading As String, ByVal con As SqlConnection, ByVal loc As String) As String
        Dim rtstr As String = ""
        Dim dc As New dbclass
        Dim sec As New k_security
        Dim fl As New file_list
        Dim fm As New formMaker
        Dim dt As DataTableReader
        Dim hdr() As String
        hdr = heading.Split(",")
        Dim i As Integer
        rtstr = "<script type='text/javascript'>" & Chr(13) & _
        "function goclicked(whr,id)" & Chr(13) & _
        "{  $('#frms').attr('action','" & loc & "?dox=' + whr + '&id='+id.toString());" & Chr(13) & _
        "$('#frms').submit();}</script>" & Chr(13)
        rtstr &= "<form id='frms' method='post' name='frms' action=''><table cellspacing='0' cellpadding='0'>" & Chr(13) & _
        "<tr style='background:#7595f7; padding:0px 10px 0px 0px; font-size:9pt;font-weight:bold;'>" & Chr(13) & _
        "<td  style='padding-right:20px;'>Edit</td><td  style='padding-right:20px;'>delete</td>" & Chr(13)
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

        rtstr = rtstr & "</tr>" & Chr(13)

        If dt.HasRows = True Then
            Dim color As String = "E3EAEB"
            While dt.Read
                If color <> "#E3EAEB" Then
                    color = "#E3EAEB"
                Else
                    color = "#fefefe"
                End If
                rtstr = rtstr & "<tr style='background:" & color & ";padding:0px -1px 0px 0px;'>" & Chr(13) & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "edit" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'><img src='images/png/edit.png' title='Edit'  /></td> " & Chr(13) & _
                "<td  style='padding-right:20px;cursor:pointer;' onclick='javascript: goclicked(" & Chr(34) & "delete" & Chr(34) & "," & Chr(34) & dt.Item(0) & Chr(34) & ");'>" & Chr(13) & _
                "<img src='images/png/delete.png' title='Delete' style='curser:pointr;' /></td>" & Chr(13)
                For k As Integer = 1 To dt.FieldCount - 1
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
                    ElseIf (dt.FieldCount - 1) = k Then
                        rtstr &= "<td><div id='x" & dt.Item(0) & "' onclick=" & Chr(34) & "javascript:$('#that" & dt.Item(0) & "').remove('dispaly');$('#that" & dt.Item(0) & "').dialog({title:'View Contents',height:300,width:600,modal:false});" & Chr(34) & " style='cursor:pointer;color:blue;'>view</div>"

                        rtstr &= "<div id='that" & dt.Item(0) & "' title='view' style='display:none; font-size:10pt;'>" & sec.dbHexToStr(dt.Item(k)) & "</div>"
                    Else

                        rtstr = rtstr & "<td  style='padding-right:20px;'>" & dt.Item(k) & "</td>"
                    End If

                Next
                rtstr = rtstr & "</tr>" & Chr(13)
            End While
        Else
            rtstr = rtstr & "<tr><td colspan='4'>No Data Found</td></tr>" & Chr(13)
        End If
        rtstr = rtstr & "</table></form>"
        dt.Close()
        dc = Nothing
        Return rtstr

    End Function
End Class
