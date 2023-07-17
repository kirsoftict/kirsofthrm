Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class reportnonactive
    Inherits System.Web.UI.Page
    Function getout()
        For Each k As String In Request.QueryString
            Session(k) = Request.QueryString(k)
        Next
        Dim title As String = Session("company_name")
        Dim sql As String = ""
        Dim out As String = ""
        Dim active As String = ""
        Dim fm As New formMaker
        Dim rt(3) As String
        If Request.QueryString.HasKeys = True Then
            'active = Request.QueryString("active")
            active = "n"

            Select Case (Request.QueryString("val"))
                Case "all"
                    sql = "select * from emprec inner join emp_static_info as esi on emprec.emp_id=esi.emp_id where active='" & active & "' and emprec.emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by esi.first_name, esi.middle_name"
                    'Response.Write(sql)
                    out = makeformx(sql)
                    title &= "<br>List of Emplyee's x-Staff "
                Case "bydep"

                    Dim dep() As String
                    dep = Request.QueryString("department").Split("|")
                    title &= "<br>List of Emplyee's Filtered by Department " & dep(1) & " " & active
                    
                    sql = "select * from emprec where emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) and active='" & active & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by end_date desc"



                    ' Response.Write(sql)

                    out = makeformx(sql)
                Case "byprojdate"

                    sql = ""
                    If Request.QueryString("projdateto") = "" And Request.QueryString("projdate") <> "" Then
                       
                        sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from >='" & Request.QueryString("projdate") & "') order by end_date desc"


                    ElseIf Request.QueryString("projdateto") <> "" And Request.QueryString("projdate") <> "" Then
                        
                        sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') order by end_date desc"


                    Else
                        out = "Sorry date is not selected"
                    End If
                    If sql <> "" Then

                        out = makeformx(sql)
                    End If

                Case "byproj"
                    sql = ""
                    Dim dep() As String
                    dep = Request.QueryString("projx").Split("|")
                    'title &= "<br>List of Emplyee's Filtered by Project " & dep(1) & " " & active
                    'project is joined to empjobassign
                    ' Response.Write(dep(0))
                    If dep.Length <= 1 Then

                        dep = Request.QueryString("projx").Split("|")
                        ReDim Preserve dep(2)
                        dep(1) = ""

                    End If

                    sql = "select * from emprec where id in(select emptid from emp_job_assign where project_id='" & _
                    dep(0) & "') and active='" & active & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by id desc"


                    title &= "<br>List of Emplyee's Filtered by Project " & dep(1) & " " & active
                    ' Response.Write(sql)
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If
                Case "bydis"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Field of Study " & Request.QueryString("discipline") & " " & active
                    
                    sql = "select * from emprec where emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "') and active='n' and emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by id desc"


                    If sql <> "" Then
                        out = makeformx(sql)
                    End If
                    ' Response.Write(sql)

                    ' For Each k As String In Request.QueryString
                    'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                    'Next
                Case "byrectime"
                    ' For Each k As String In Request.QueryString
                    'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                    'Next
                   
                        sql = "select * from emprec where hire_date between '" & Request.QueryString("recdate") & "' and '" & Request.QueryString("recdateto") & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by hire_date, id desc"



                    If sql <> "" Then
                        out = makeformx(sql)
                    End If
                Case "byqual"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Qualification " & Request.QueryString("qualification") & " " & active

                    sql = "select * from emprec where emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') and active='" & active & "' and emprec.id not in(select id from emprec where active='y' and end_date is null) order by id desc"
                    'Response.Write(sql)
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If


                Case "bypost"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by position " & Request.QueryString("position") & " " & active

                    sql = "select * from emprec where id in(select emptid from emp_job_assign where position='" & Request.QueryString("position") & "' ) and active='" & active & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null)  order by id desc"

                    If sql <> "" Then
                        out = makeformx(sql)
                        'Response.Write(sql)
                    End If
                Case "pp"
                    '45 days workers
                    sql = "select * from emprec where dateadd(d,45,hire_date)>'" & Today.ToShortDateString & "' and active='" & active & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null)"
                    ''Response.Write(sql)
                    out = makeformx(sql)
                Case "namex"
                    Dim name() As String
                    name = Request.QueryString("vname").Split(" ")
                    If name.Length > 3 Then
                        If name(0).ToString <> " " Then
                            sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%') and active='" & active & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by id desc"

                        End If
                    ElseIf name.Length = 3 Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%' and second_name like '%" & name(1) & "%' and last_name like '%" & name(0) & "%') and active='" & active & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by id desc"

                    ElseIf name.Length = 1 Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%')  and active='" & active & "' and emp_id not in(select emp_id from emprec where active='y' and end_date is null) order by id desc"

                    End If

                    ''Response.Write(sql)
                    out = makeformx(sql)

            End Select
        End If
        rt(0) = title
        rt(1) = out
        rt(2) = sql
        Return rt
    End Function
    Public Function makeformx(ByVal sql As String)
        Dim fullname, position, sal(), proj As String
        ' Dim sql As String = ""
        ' Response.Write(sql)
        Dim nrow As Integer = 0
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim dt As DataTableReader
        Dim out As String = ""
        Dim fld(,) As String
        Dim empid(1) As String
        Dim active As String = ""
        Dim rowspan As Integer = 0
        Dim col(15) As String
        Dim color As String = ""
        empid(0) = ""
        Dim i As Integer = 0
        Dim cc As Integer = 1
        'Response.Write(sql)
        Try

       
        dt = dbs.dtmake("md", sql, Session("con"))
        If dt.HasRows Then
            out = ""

            While dt.Read
                rowspan = 0
                ' Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)

                If fm.searcharray(empid, dt.Item("emp_id")) = False Then
                    ' Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)
                    ReDim Preserve empid(i + 1)
                    empid(i) = dt.Item("emp_id")
                    For j As Integer = 0 To 15
                        col(j) = ""
                    Next
                    col(0) = (cc).ToString
                    cc += 1
                    'Response.Write("<br>" & col(0))
                    fullname = fm.getfullname(dt.Item("emp_id"), Session("con"))
                    col(1) = "<a href='dataallview.aspx?empid=" & dt.Item("emp_id") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fullname & "</a>"
                    col(2) = fm.getinfo2("select sex from emp_static_info where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    fld = fm.getqualification(dt.Item("emp_id"), Session("con"))
                    col(3) = dt.Item("hire_date")
                    If dt.IsDBNull(4) = False Then
                        col(4) = dt.Item("end_date")
                    Else
                        col(4) = ""
                    End If
                    position = fm.getinfo2("select position from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' order by id desc", Session("con"))
                    Dim emptid As String
                    emptid = fm.getinfo2("select emptid from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' order by id desc", Session("con"))

                        col(9) = "<a href='reportnonactive.aspx?val=bypost&position=" & position.Trim & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & position & "</a>"
                    active = fm.getinfo2("select active from emprec where emp_id='" & dt.Item("emp_id") & "' and end_date is null", Session("con"))

                    sal = dbs.getsal(dt.Item("id"), Session("con"))
                    If IsNumeric(sal(0)) Then
                        col(10) = fm.numdigit(CDbl(sal(0)), 2)
                    Else
                        col(10) = "salary Not Setted"
                    End If
                    col(11) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and istaxable='n' and to_date is not null group by to_date order by to_date desc", Session("con"))
                    If IsNumeric(col(11)) Then
                        col(11) = fm.numdigit(CDbl(col(11)), 2)
                    Else
                        col(11) = "None"
                    End If

                    col(12) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & emptid & " and to_date is not null and istaxable='y' group by istaxable,to_date", Session("con"))
                    If IsNumeric(col(12)) Then
                        col(12) = fm.numdigit(CDbl(col(12)), 2)
                    Else
                        col(12) = "None"
                    End If
                    col(13) = fm.getinfo2("select pardime from emp_pardime where emptid=" & emptid & " and to_date is not null order by to_date desc", Session("con"))
                    If IsNumeric(col(13)) Then
                        col(13) = fm.numdigit(CDbl(col(13)), 2)
                    Else
                        col(13) = "None"
                    End If
                    col(15) = fm.getinfo2("select mob from emp_address where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    Dim projid As String = ""
                    projid = fm.getinfo2("select project_id from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' and date_end is Not null order by date_end desc", Session("con"))
                    proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                        col(14) = "<a href='reportnonactive.aspx?val=byproj&projx=" & projid & "|" & proj & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & proj & "</a>"

                    'col(13) = ""
                    'Response.Write(dt.Item("emp_id") & "data " & fld(0, 0) & "<br>")
                    rowspan = CInt(fld(0, 0))
                    If color = "#e8f1fa" Then
                        color = "white"
                    Else
                        color = "#e8f1fa"
                    End If
                    ' Response.Write(active)

                    If active.Trim = "None" Then
                        out &= "<tr style=' font-family:Times New Roman; height: 31.5pt;font-size:9pt; mso-height-source: userset;background-color:" & color & "'>" & Chr(13)
                        ' Response.Write(active & "<br>")

                        '  Response.Write(color)

                        For p As Integer = 0 To 15

                            If rowspan > 0 And (p >= 9 Or p <= 4) Then
                                If p > 9 And p <= 13 Then
                                    out &= "<td rowspan='" & (rowspan).ToString & "' class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                    "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt; text-align:right; '>" & col(p) & "</td>" & Chr(13)
                                Else
                                    out &= "<td rowspan='" & (rowspan).ToString & "' class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                  "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt; '>" & col(p) & "</td>" & Chr(13)

                                End If
                            ElseIf p > 4 And p < 8 Then
                                If rowspan = 0 Then
                                        out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                    "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='reportnonactive.aspx?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                        out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                    "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='reportnonactive.aspx?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                    out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(2, 0) & "</td>"
                                    out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt;'>" & fld(3, 0) & "</td>"
                                    p = 8
                                Else
                                        out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                    "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='reportnonactive.aspx?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                        out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                    "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='reportnonactive.aspx?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                    out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(2, 0) & "</td>"
                                    out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt;'>" & fld(3, 0) & "</td>"
                                    p = 8
                                End If
                            Else
                                If p > 9 And p <= 13 Then
                                    out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                 "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt;text-align:right;'>" & col(p) & "</td>"

                                Else
                                    out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                  "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt;'>" & col(p) & "</td>"

                                End If

                            End If
                        Next

                        out &= "</tr>"
                    End If
                    If rowspan > 1 Then
                        For k As Integer = 1 To CInt(fld(0, 0)) - 1
                                out &= "<tr style=' font-family:Times New Roman; height: 31.5pt;font-size:9pt;background-color:" & color & "'><td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='reportnonactive.aspx?val=bydis&discipline=" & fld(4, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, k) & "</a></td>"
                                out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
                "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '><a href='reportnonactive.aspx?val=byqual&qualification=" & fld(1, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, k) & "</a></td>"
                            out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(2, k) & "</td>"
                            out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
            "border-left: windowtext; border-bottom: windowtext 1pt solid; height: 29pt; '>" & fld(3, k) & "</td></tr>"

                        Next

                    End If
                    i = i + 1
                End If

                End While

                'Response.Write("<table cellpading='2' cellspacing='2' bordercolor='blue'>" & out & "</table>")
            End If
            dt.Close()
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & sql)
        End Try
        If out = "" Then
            out = "<tr><td colspan='15' style='color:red;font-size:30pt;'>Sorry! Data are not Found</td></tr>"
        End If
        Return out
    End Function
End Class
