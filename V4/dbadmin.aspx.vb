Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm

Partial Class _dbadmin
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Session("con").open()
        Catch ex As Exception
            Session("con").close()
            Session("con").open()
        End Try
        Dim sds As SqlDataSource
        Dim db As New dbclass
        Dim sql As String = ""

        If Request.QueryString("updated") = "on" Then
            'Response.Write(Request.QueryString("updated") & "4564654654")
            sql = db.makest(Request.QueryString("dtable"), Request.Form, Session("con"), "")
            ' MsgBox(sql)
            Dim obj As Object
            obj = db.excutes(sql, Session("con"), Session("path"))
            Response.Write(obj.ToString)
            If IsNumeric(obj) Then
                'MsgBox("saved")
                If CInt(obj) >= 1 Then
                    Response.Redirect("dbadmin.aspx?dtable=" & Request.QueryString("dtable"))
                End If
            Else
                Response.Write("Sorry Data is not enter duto " & sql)
            End If
        End If
        Dim nstr() As String = {"", ""}
        sds = SqlDataSource1
        sds.ProviderName = "System.Data.SqlClient"
        sds.ConnectionString = Session("constr")
        'MsgBox(Session("constr"))
        If Request.QueryString("dtable") <> "" Then
            sds.SelectCommand = "select * from " & Request.QueryString("dtable")
            nstr = db.makeupdate(Request.QueryString("dtable"), "", Session("con"), "")
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
                df = db.getdatefields(Request.QueryString("dtable"), Session("con"))
                If df.Length > 0 Then
                    For i As Integer = 0 To df.Length - 1
                        Dim param1 As New Parameter(df(i), TypeCode.DateTime, DateTime.Now.ToString())
                        SqlDataSource1.UpdateParameters.Add(param1)
                    Next
                End If

                sds.DeleteCommand = "Delete from " & Request.QueryString("dtable") & " where " & nstr(1) & "= @" & nstr(1)


                ' nstr(0) = " update weather set city=@city,condition=@condition,mxtemp=@mxtemp,mtemp=@mtemp,datetemp=@datetemp,publish=@publish,dateent=@dateent where id= @id"
                'Response.Write(nstr(0))
                sds.UpdateCommand = nstr(0)
            End If
        End If

        If Request.QueryString("dtable") <> "" Then
            File.WriteAllText("c:\temp\" & Request.QueryString("dtable") & ".xml", dbxmlview(Request.QueryString("dtable")))
        End If


    End Sub
    Public Function pageaddnew() As String

        Dim sql As String
        sql = "select * from " & Request.QueryString("dtable")
        If Request.QueryString("dtable") <> "" Then
            Dim cm As New menubuilder
            Dim rs As SqlDataReader
            Dim formx As String = ""
            rs = cm.cmdx(session("con"), sql)
            Dim icount, i As Integer
            icount = rs.FieldCount
            formx = "<form method='post' id='frmpage_" & Request.QueryString("dtable") & "' name='frmpage_" & Request.QueryString("dtable") & "' enctype='multipart/form-data' action='dbadmin.aspx?updated=on&dtable=" & Request.QueryString("dtable") & "'>"
            formx = formx & "<table>"
            For i = 0 To icount - 1
                If i = 0 Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                    "<input type='hidden' name='" & rs.GetName(i) & "' value=''><b></b></td></tr>"
                ElseIf LCase(rs.GetDataTypeName(i)) = "text" Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                         "<textarea id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & "'>"
                    formx = formx & "</textarea></td></tr>"


                ElseIf LCase(rs.GetDataTypeName(i)) = "datetime" Then
                    formx = formx & "<tr><td>" & rs.GetName(i) & "</td><td>:</td><td>" & _
                                                              "<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
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
                                                            "<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
                                                       "' value=''></td></tr>"


                End If
            Next
            formx = formx & "<tr><td><button name='btnupdate' value='update' onclick=" & Chr(34) & "javascript:document.frmpage_" & Request.QueryString("dtable") & ".submit();" & Chr(34) & " >Save</button></table></form>"
            'formx = formx & "<br />Uploads: images video"
            rs.Close()
            cm = Nothing
            Return formx
        Else
            Return ""
        End If
    End Function
    Public Function dbxmlview(ByVal db As String) As String
        Dim ds As New DataSet
        Dim dt As SqlDataAdapter = New SqlDataAdapter
        dt.TableMappings.Add("Table", db)
        Dim sql As String = "select * from " & db
        dt.SelectCommand = New SqlCommand(sql, session("con"))
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

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete


    End Sub
End Class