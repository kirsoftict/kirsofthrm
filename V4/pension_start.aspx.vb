Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class pension_start
    Inherits System.Web.UI.Page
    Public keyp
    Public idx
    Public msg
    Public dbx As New dbclass
    Public sql As String = ""
    Public flg As Integer = 0
    Public flg2 As Integer = 0
    ' Response.Write(sc.d_encryption("zewde@123"))
    Public rd As String = ""

    Public tbl As String = ""
    Public key As String = ""
    Public keyval As String = ""
    Public fm As New formMaker
    Public emp_id As String
    Public emptid As Integer
    Function pageon()
        ' Dim keyp As String = ""

        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        Else
            keyp = "save"
        End If
        'Dim idx As String = ""
        idx = Request.QueryString("id")
        ' Dim msg As String = ""

        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                    ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval)
                    flg = dbx.save(sql, session("con"), session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                        msg = "Data Updated"
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
                        'Response.Write(arrname.Length.ToString)
                        If arrname.Length > 2 Then
                            sql = "Select * from emp_static_info where first_name='" & arrname(0).Trim & "' and middle_name='" & arrname(1).Trim & "' and last_name='" & arrname(2).Trim & "'"

                        End If
                    End If


                    Dim dtt As DataTableReader
                    If sql <> "" Then
                        ' Response.Write(sql)
                        dtt = dbx.dtmake("tblstatic", sql, Session("con"))

                        If dtt.HasRows Then
                            dtt.Read()
                            Dim da As String
                            emp_id = dtt.Item("emp_id")
                            'Response.Write(emp_id)
                            da = (fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' order by id desc", Session("con")))
                            If da.ToLower = "none" Or da.Length > 12 Then
                                emptid = 0
                            Else
                                emptid = CInt(da)
                            End If
                            Response.Write(da)

                        End If
                        If emptid > 0 Then
                            ' Response.Write(Request.QueryString("st"))
                            sql = "insert into empbank(emptid,bankname,branch,accountno,active,who_reg,date_reg) " & _
                            "values('" & emptid & "','" & Request.QueryString("bankname") & "','" & Request.QueryString("branch") & "','" & _
                            Request.QueryString("accountno") & "','" & Request.QueryString("active") & "','" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')"
                            ' Response.Write(sql)

                            flg = dbx.save(sql, session("con"), session("path"))
                            'Response.Write(flg)
                            If flg = 1 Then
                                msg = "Data Saved"
                            End If
                        Else
                            msg = "The employee not existed, please re enter the name"
                        End If
                    End If

                    'MsgBox(rd)

                    ' sql = db.makest(tbl, Request.QueryString, session("con"), key)

                End If



            End If
        End If

    End Function
    Function gnamelist()
        Dim fm As New formMaker

        Dim namelist As String = ""
        namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
        Return namelist
    End Function
End Class
