Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Imports System.Web.HttpRequest
Partial Class grid
    Inherits System.Web.UI.Page
    Function calledshow(ByVal tbl As String, ByVal pagen As String, ByVal sql As String)
        If Session("con").state = Data.ConnectionState.Closed Then
            'Response.Write("database is closed")
            Session("con").open()
        End If
        Dim sds As SqlDataSource
        Dim db As New dbclass
        Dim sssql As String = ""
        Dim nstr() As String = {"", ""}
        If Request.QueryString("dtable") <> "" Then
            tbl = Request.QueryString("dtable")
        End If
        ' db.dtmake("vwdba", Session("sql"), session("con"))


        sds = SqlDataSource1
        sds.ProviderName = "System.Data.SqlClient"
        sds.ConnectionString = Session("constr")
        'MsgBox(Session("constr"))


        If IsPostBack = True Then
            '  Response.Write(Session("sql"))
            sssql = Session("sql")
        Else

            Session("sql") = sql
            sssql = Session("sql")
        End If

        ' Response.Write(sssql)
        sds.SelectCommand = sssql 'select * from " & Request.QueryString("dtable")

        GridView1.BackColor = Drawing.Color.AliceBlue
        GridView1.AllowPaging = True
        GridView1.AutoGenerateEditButton = True
        GridView1.AllowPaging = True
        GridView1.PageSize = 10
        GridView1.AllowSorting = True



        If IsPostBack = True Then

            nstr = db.makeupdatea(tbl, sql, session("con"), "")
            ' Response.Write(nstr(0))
            Dim nk() As String = {""}
            nk(0) = nstr(1) 'on it
            GridView1.DataKeyNames = nk 'on it
            ' Response.Write(nstr(0))
            If nstr(0) <> "" Then
                Dim st(2) As String
                ' GridView1.EditRowStyle.
                Dim df() As String




                ' Response.Write("Delete from " & tbl & " where " & nstr(1) & "= @" & nstr(1))
                Try
                    sds.DeleteCommand = "Delete from " & tbl & " where " & nstr(1) & "= @" & nstr(1)

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try


                ' nstr(0) = " update weather set city=@city,condition=@condition,mxtemp=@mxtemp,mtemp=@mtemp,datetemp=@datetemp,publish=@publish,dateent=@dateent where id= @id"

                Try
                    ' Response.Write(nstr(0))

                    sds.UpdateCommand = nstr(0)
                    ' Response.Write("<script>redirect('frm_tar','" & Request.ServerVariables("HTTP_REFERER") & "');</script>")

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End If
        End If



    End Function

    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sds As SqlDataSource
        Dim db As New dbclass
        Dim sql As String = ""
        Dim sec As New k_security
        Dim sqlx As String
        Dim tbl As String = ""
        Dim fullname As String
        sqlx = sec.dbHexToStr(Request.QueryString("sql"))
        fullname = Request.QueryString("fname")
        Dim sssql As String = ""
        Dim fname As String = ""
        If IsPostBack = True Then
            '  Response.Write(Session("sql"))
            sssql = Session("sql")
            fname = Session("fname")
        Else
            Session("fname") = fullname
            fname = Session("fname")

            Session("sql") = sqlx
            sssql = Session("sql")
        End If
        ffname.Text = fname
        tbl = Request.QueryString("tbl")
        If Request.QueryString("updated") = "on" Then
            'Response.Write(Request.QueryString("updated") & "4564654654")
            sql = db.makest(tbl, Request.Form, session("con"), "")
            ' MsgBox(sql)
            If db.save(sql, session("con"), session("path")) = 1 Then
                'MsgBox("saved")
                Response.Redirect("grid.aspx?tbl=" & Request.QueryString("tbl"))
            Else
                Response.Write("Sorry Data is not enter duto " & sql)
            End If
        End If
        Dim nstr() As String = {"", ""}
        sds = SqlDataSource1
        sds.ProviderName = "System.Data.SqlClient"
        sds.ConnectionString = Session("constr")
        'MsgBox(Session("constr"))
        If Request.QueryString("tbl") <> "" Then
            sds.SelectCommand = sssql
            nstr = db.makeupdatea(Request.QueryString("tbl"), sssql, session("con"), "")
            Dim nk() As String = {""}
            nk(0) = nstr(1)
            GridView1.DataKeyNames = nk
        End If
        GridView1.BackColor = Drawing.Color.AliceBlue
        GridView1.AllowPaging = True
        GridView1.AutoGenerateEditButton = True
        GridView1.AllowPaging = True
        GridView1.PageSize = 10
        GridView1.AllowSorting = True


        If IsPostBack = True Then
            If nstr(0) <> "" Then

                Dim st(2) As String

                ' GridView1.EditRowStyle.
                Dim df() As String
                df = db.getdatefields(Request.QueryString("tbl"), session("con"))
                If df.Length > 0 Then
                    For i As Integer = 0 To df.Length - 1
                        Dim param1 As New Parameter(df(i), TypeCode.DateTime, DateTime.Now.ToString())
                        'SqlDataSource1.UpdateParameters.Add(param1)
                    Next
                End If

                sds.DeleteCommand = "Delete from " & Request.QueryString("tbl") & " where " & nstr(1) & "= @" & nstr(1)


                ' nstr(0) = " update weather set city=@city,condition=@condition,mxtemp=@mxtemp,mtemp=@mtemp,datetemp=@datetemp,publish=@publish,dateent=@dateent where id= @id"
                'Response.Write(nstr(0))

                sds.UpdateCommand = nstr(0)
                updatedb(Request.QueryString("tbl"))

            End If
        End If



    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        'Response.Write(Session("pprroojj") & "<br>")
        If e.CommandName = "Update" Then
            Session("wht") = "Update"
        Else
            Session("wht") = ""
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
    Function updatedb(ByVal tbl As String)

        Dim dbx As New dbclass
        Dim rs As DataTableReader
        Dim spl() As String
        Dim projid As String = ""
        Dim nod As Integer
        Dim sqlx As String
        Dim pdate1, pdate2 As Date
        Dim pproj As String
        pproj = Session("pprroojj")
        pdate1 = Session("whn")
        pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
        'Response.Write(Session("pprroojj"))
        If IsError(Session("pprroojj")) = True Or String.IsNullOrEmpty(Session("pprroojj")) = True Then
            Response.Redirect("logout.aspx")
        Else
            If pproj <> "" Then 'Then
                spl = pproj.Split("|")
                projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
            End If

            If tbl = "emp_ot" Then

                rs = dbx.dtmake("selc_rec", "select * from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "'", Session("con"))
                ' Response.Write('select * from emprec where end_date is null and id in(select emptid from emp_job_assign project_id='' & projid & '')')
                If rs.HasRows = True Then

                    Dim emptid As String = ""
                    Dim sal() As String
                    Dim hr As Double
                    Dim rate As Double
                    Dim timedif As Double
                    Dim oldamt, amt As Double

                    While rs.Read
                        emptid = rs.Item("emptid")
                        sal = dbx.getsal(emptid, pdate1, Session("con"))
                        hr = sal(0) / 200.67
                        rate = rs.Item("rate")
                        'If rs.IsDBNull(6) = False Then
                        timedif = rs.Item("time_diff")
                        ' Else

                        ' End If
                        oldamt = rs.Item("amt")

                        amt = FormatNumber((hr * rate * timedif), 2)

                        If oldamt <> amt Then
                            ' Response.Write(oldamt.ToString & ", " & amt.ToString)
                            sqlx = "Begin Transaction" & Chr(13) & "Update emp_ot set amt='" & amt & "' where id=" & rs.Item("id") & Chr(13) & " Commit "
                            '  Response.Write(sqlx)
                            Dim flg As String = ""

                            flg = dbx.excutes(sqlx, Session("con"), Session("path"))
                            'Response.Write(flg.ToString)
                        End If

                    End While
                End If
                rs.Close()

            End If
            dbx = Nothing
        End If
        If Session("wht") = "Update" Then
            'Response.Write(Request.ServerVariables("HTTP_REFERER"))
        End If
        For Each k As String In Request.ServerVariables
            'Response.Write(k & "====" & Request.ServerVariables(k) & "<br>")
        Next
    End Function

End Class
