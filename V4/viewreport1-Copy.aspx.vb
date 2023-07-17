Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class viewreportcopy1
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
        Dim rt() As String = {"", "", ""}
        If Request.QueryString.HasKeys = True Then
            active = Request.QueryString("active")

            If active <> "" Then
                If active = "y" Then
                    active = "(Active Employees)"
                Else
                    active = "(Deactive Employess)"
                End If
            Else
                active = "(All Employees)"
            End If
            Select Case (Request.QueryString("val"))
                Case "all"
                    sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract' and emprec.end_date is null ORDER BY emp_static_info.first_name, emprec.id DESC"
                    out = headermk("first", "<br>List of Contract Emplyee's ")
                    out &= makeformx(sql)
                    out &= "</table>"

                Case "bydep"

                    Dim dep() As String
                    dep = Request.QueryString("department").Split("|")
                    title &= "<br>List of Emplyee's Filtered by Department " & dep(1) & " " & active
                    sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) order by emp_static_info.first_name"


                    ' Response.Write(sql)

                    out = headermk("first", "<br>List of Emplyee's by Department ")
                    out &= makeformx(sql)
                    out &= "</table>"
                Case "byprojdate"

                    sql = ""
                    If Request.QueryString("projdateto") = "" And Request.QueryString("projdate") <> "" Then

                        sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.id in(select emptid from emp_job_assign where date_from >='" & Request.QueryString("projdate") & "') order by emp_static_info.first_name"

                    ElseIf Request.QueryString("projdateto") <> "" And Request.QueryString("projdate") <> "" Then

                        sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') order by emp_static_info.first_name desc"

                    Else
                        out = "Sorry date is not selected"
                    End If
                    If sql <> "" Then
                        out = headermk("first", "<br>List of Emplyee's Project Date ")
                        out &= makeformx(sql)
                        out &= "</table>"
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

                    sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.id in(select emptid from emp_job_assign where project_id='" & _
                    dep(0) & "' and date_end is null)  order by emp_static_info.first_name desc"

                    title &= "<br>List of Emplyee's Filtered by Project " & dep(1) & " " & active
                    ' Response.Write(sql)
                    If sql <> "" Then
                        out = headermk("first", "<br>List of Emplyee's By Project(" & dep(1) & ")")
                        out &= makeformx(sql)
                        out &= "</table>"
                    End If
                Case "bydis"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Field of Study " & Request.QueryString("discipline") & " " & active

                    sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "')  order by emp_static_info.first_name"


                    If sql <> "" Then
                        out = headermk("first", "<br>List of Emplyee's By Field of Study ")
                        out &= makeformx(sql)
                        out &= "</table>"
                    End If 'Response.Write(sql)

                    ' For Each k As String In Request.QueryString
                    'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                    'Next
                Case "byrectime"
                    ' For Each k As String In Request.QueryString
                    'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                    'Next
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where hire_date between '" & Request.QueryString("recdate") & "' and '" & Request.QueryString("recdateto") & "' and active='" & Request.QueryString("active") & "' order by hire_date, id desc"
                    Else
                        sql = "select * from emprec where hire_date between '" & Request.QueryString("recdate") & "' and '" & Request.QueryString("recdateto") & "' order by hire_date, id desc"


                    End If
                    If sql <> "" Then
                        out = headermk("first", "<br>List of Emplyee's By Hire Date")
                        out &= makeformx(sql)
                        out &= "</table>"
                    End If
                Case "byqual"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Qualification " & Request.QueryString("qualification") & " " & active

                    sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') and active='y' order by first_name"

                    If sql <> "" Then
                        out = headermk("first", "<br>List of Emplyee's By Level of Study")
                        out &= makeformx(sql)
                        out &= "</table>"
                    End If 'Response.Write(sql)

                Case "bypost"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by position " & Request.QueryString("position") & " " & active

                    sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.id in(select emptid from emp_job_assign where position='" & Request.QueryString("position") & "' and date_end is null ) order by emp_static_info.first_name"

                    If sql <> "" Then
                        out = headermk("first", "<br>List of Emplyee's By Position(" & Request.QueryString("position") & ")")
                        out &= makeformx(sql)
                        out &= "</table>"
                        'Response.Write(sql)
                    End If
                Case "pp"
                    '45 days workers
                    sql = "select * from emprec where dateadd(d,45,hire_date)>'" & Today.ToShortDateString & "'"
                    ''Response.Write(sql)
                    out = headermk("second", "<br>List of Emplyee's In Probation Period ")
                    out &= makeformxy(sql)
                    out &= "</table>"

                Case "namex"
                    Dim name() As String
                    name = Request.QueryString("vname").Split(" ")
                    If name.Length > 3 Then
                        If name(0).ToString <> " " Then
                            sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%') order by emp_static_info.first_name"

                        End If
                    ElseIf name.Length = 3 Then
                        sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%' and middle_name like '%" & name(1) & "%' and last_name like '%" & name(2) & "%')  order by emp_static_info.first_name"

                    ElseIf name.Length = 1 Then
                        sql = "SELECT emprec.id, emprec.emp_id, emprec.type_recuritment, emprec.hire_date, emprec.end_date, emprec.holiday, emprec.who_reg, " & _
                    " emprec.date_reg, emprec.active FROM emprec INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id" & _
                    " where emprec.active='y' and emprec.Type_recuritment='Contract'  and emprec.emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%')  order by emp_static_info.first_name"

                    End If

                    'Response.Write(sql)
                    out = headermk("first", "<br>List of Emplyee's Searched! ")
                    out &= makeformx(sql)
                    out &= "</table>"

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
        Dim curst As String = ""
        empid(0) = ""
        Dim i As Integer = 0
        dt = dbs.dtmake("md", sql, Session("con"))
        If dt.HasRows Then
            out = ""
            While dt.Read
                rowspan = 0
                'Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)

                If fm.searcharray(empid, dt.Item("emp_id")) = False Then
                    ' Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)
                    curst = fm.getinfo2("select Type_recuritment from emprec where emp_id='" & dt.Item("emp_id") & "' order by hire_date desc", Session("con"))
                    If curst = "Contract" Then
                        ReDim Preserve empid(i + 1)
                        empid(i) = dt.Item("emp_id")

                        For j As Integer = 0 To 15
                            col(j) = ""
                        Next
                        col(0) = (i + 1).ToString
                        'Response.Write("<br>" & col(0))
                        fullname = fm.getfullname(dt.Item("emp_id"), Session("con"))
                        col(1) = "<a href='dataallview.aspx?empid=" & dt.Item("emp_id") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fullname & "</a>"
                        col(2) = fm.getinfo2("select sex from emp_static_info where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                        fld = fm.getqualification(dt.Item("emp_id"), Session("con"))
                        col(3) = dt.Item("hire_date")
                        position = fm.getinfo2("select position from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' order by id desc", Session("con"))
                        col(9) = "<a href='?val=bypost&position=" & position.Trim & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & position & "</a>"
                        active = fm.getinfo2("select active from emprec where emp_id='" & dt.Item("emp_id") & "' and end_date is null", Session("con"))

                        sal = dbs.getsal(dt.Item("id"), Session("con"))
                        If IsNumeric(sal(0)) Then
                            col(10) = fm.numdigit(CDbl(sal(0)), 2)
                        Else
                            col(10) = "salary Not Setted"
                        End If
                        col(11) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & dt.Item("id") & " and istaxable='n' and to_date is null group by istaxable", Session("con"))
                        If IsNumeric(col(11)) Then
                            col(11) = fm.numdigit(CDbl(col(11)), 2)
                        Else
                            col(11) = "None"
                        End If

                        col(12) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & dt.Item("id") & " and to_date is null and istaxable='y' group by istaxable", Session("con"))
                        If IsNumeric(col(12)) Then
                            col(12) = fm.numdigit(CDbl(col(12)), 2)
                        Else
                            col(12) = "None"
                        End If
                        col(13) = fm.getinfo2("select pardime from emp_pardime where emptid=" & dt.Item("id") & " and to_date is null order by id desc", Session("con"))
                        If IsNumeric(col(13)) Then
                            col(13) = fm.numdigit(CDbl(col(13)), 2)
                        Else
                            col(13) = "None"
                        End If
                        col(15) = fm.getinfo2("select mob from emp_address where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                        Dim projid As String = ""
                        projid = fm.getinfo2("select project_id from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' and date_end is null", Session("con"))
                        proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                        col(14) = "<a href='?val=byproj&projx=" & projid & "|" & proj & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & proj & "</a>"

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
                            color = "red"
                        End If
                        out &= "<tr style='background-color:" & color & "'>" & Chr(13)
                        ' Response.Write(active & "<br>")

                        '  Response.Write(color)
                        For p As Integer = 0 To 15

                            If rowspan > 0 And (p >= 8 Or p <= 3) Then
                                If IsNumeric(col(p)) = True Then
                                    out &= "<td rowspan='" & (rowspan).ToString & "' class='cell' style='text-align:right; '>" & col(p) & "</td>" & Chr(13)
                                Else
                                    out &= "<td rowspan='" & (rowspan).ToString & "' class='cell'>" & col(p) & "</td>" & Chr(13)

                                End If
                            ElseIf p > 3 And p < 8 Then
                                If rowspan = 0 Then
                                    out &= "<td class='cell'><a href='?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                    out &= "<td class='cell'><a href='?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                    out &= "<td class='cell'>" & fld(2, 0) & "</td>"
                                    out &= "<td class='cell'>" & fld(3, 0) & "</td>"
                                    p = 8
                                Else
                                    out &= "<td class='cell' ><a href='?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                    out &= "<td class='cell' ><a href='?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                    out &= "<td class='cell' >" & fld(2, 0) & "</td>"
                                    out &= "<td class='cell' >" & fld(3, 0) & "</td>"
                                    p = 8
                                End If
                            Else
                                If IsNumeric(col(p)) = True Then
                                    out &= "<td class='cell' style='text-align:right;'>" & col(p) & "</td>"

                                Else
                                    out &= "<td class='cell'>" & col(p) & "</td>"

                                End If

                            End If
                        Next
                        out &= "</tr>"
                        If rowspan > 1 Then
                            For k As Integer = 1 To CInt(fld(0, 0)) - 1
                                out &= "<tr>" & _
                                "<td class='cell'><a href='?val=bydis&discipline=" & fld(4, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, k) & "</a></td>"
                                out &= "<td class='cell'><a href='?val=byqual&qualification=" & fld(1, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, k) & "</a></td>"
                                out &= "<td class='cell'>" & fld(2, k) & "</td>"
                                out &= "<td class='cell'>" & fld(3, k) & "</td></tr>"

                            Next

                        End If

                        i = i + 1
                    End If
                End If

            End While

            'Response.Write("<table cellpading='2' cellspacing='2' bordercolor='blue'>" & out & "</table>")
        End If
        If out = "" Then
            out = "<tr><td colspan='15' style='color:red;font-size:30pt;'>Sorry! Data are not Found</td></tr>"
        End If
        Return out
    End Function
    Public Function makeformxy(ByVal sql As String)
        Dim fullname, position, sal(), proj As String
        ' Dim sql As String = ""
        Dim nrow As Integer = 0
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim dt As DataTableReader
        Dim out As String = ""
        Dim fld(,) As String
        Dim empid(1) As String
        Dim active As String = ""
        Dim rowspan As Integer = 0
        Dim col(16) As String
        Dim color As String = ""
        empid(0) = ""
        Dim i As Integer = 0
        dt = dbs.dtmake("md", sql, Session("con"))
        If dt.HasRows Then
            out = ""
            While dt.Read
                rowspan = 0
                'Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)

                If fm.searcharray(empid, dt.Item("emp_id")) = False Then
                    ' Response.Write("<br>" & dt.Item("emp_id") & " ==>" & fm.searcharray(empid, dt.Item("emp_id")).ToString)
                    ReDim Preserve empid(i + 1)
                    empid(i) = dt.Item("emp_id")

                    For j As Integer = 0 To 15
                        col(j) = ""
                    Next
                    col(0) = (i + 1).ToString
                    'Response.Write("<br>" & col(0))
                    fullname = fm.getfullname(dt.Item("emp_id"), Session("con"))
                    col(1) = "<a href='dataallview.aspx?empid=" & dt.Item("emp_id") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fullname & "</a>"
                    col(2) = fm.getinfo2("select sex from emp_static_info where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    fld = fm.getqualification(dt.Item("emp_id"), Session("con"))
                    col(3) = dt.Item("hire_date")
                    col(4) = CDate(col(3)).AddDays(45)
                    position = fm.getinfo2("select position from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' order by id desc", Session("con"))
                    col(10) = "<a href='?val=bypost&position=" & position.Trim & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & position & "</a>"
                    active = fm.getinfo2("select active from emprec where emp_id='" & dt.Item("emp_id") & "' and end_date is null", Session("con"))

                    sal = dbs.getsal(dt.Item("id"), Session("con"))
                    If IsNumeric(sal(0)) Then
                        col(11) = fm.numdigit(CDbl(sal(0)), 2)
                    Else
                        col(11) = "salary Not Setted"
                    End If
                    col(12) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & dt.Item("id") & " and istaxable='n' and to_date is null group by istaxable", Session("con"))
                    If IsNumeric(col(11)) Then
                        col(12) = fm.numdigit(CDbl(col(11)), 2)
                    Else
                        col(12) = "None"
                    End If

                    col(13) = fm.getinfo2("select sum(amount) from emp_alloance_rec where emptid=" & dt.Item("id") & " and to_date is null and istaxable='y' group by istaxable", Session("con"))
                    If IsNumeric(col(13)) Then
                        col(13) = fm.numdigit(CDbl(col(13)), 2)
                    Else
                        col(13) = "None"
                    End If
                    col(14) = fm.getinfo2("select pardime from emp_pardime where emptid=" & dt.Item("id") & " and to_date is null order by id desc", Session("con"))
                    If IsNumeric(col(14)) Then
                        col(14) = fm.numdigit(CDbl(col(14)), 2)
                    Else
                        col(14) = "None"
                    End If
                    col(16) = fm.getinfo2("select mob from emp_address where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    Dim projid As String = ""
                    projid = fm.getinfo2("select project_id from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' and date_end is null", Session("con"))
                    proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                    col(15) = "<a href='?val=byproj&projx=" & projid & "|" & proj & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & proj & "</a>"

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
                        color = "red"
                    End If
                    out &= "<tr style='background-color:" & color & "'>" & Chr(13)
                    ' Response.Write(active & "<br>")

                    '  Response.Write(color)
                    For p As Integer = 0 To 16

                        If rowspan > 0 And (p >= 9 Or p <= 4) Then
                            If p > 10 And p <= 13 Then
                                out &= "<td rowspan='" & (rowspan).ToString & "' class='cell' style='text-align:right; '>" & col(p) & "</td>" & Chr(13)
                            Else
                                out &= "<td rowspan='" & (rowspan).ToString & "' class='cell'>" & col(p) & "</td>" & Chr(13)

                            End If
                        ElseIf p > 4 And p < 9 Then
                            If rowspan = 0 Then
                                out &= "<td class='cell'><a href='?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                out &= "<td class='cell'><a href='?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                out &= "<td class='cell'>" & fld(2, 0) & "</td>"
                                out &= "<td class='cell'>" & fld(3, 0) & "</td>"
                                p = 9
                            Else
                                out &= "<td class='cell' ><a href='?val=bydis&discipline=" & fld(4, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, 0) & "</a></td>"
                                out &= "<td class='cell' ><a href='?val=byqual&qualification=" & fld(1, 0) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, 0) & "</a></td>"
                                out &= "<td class='cell' >" & fld(2, 0) & "</td>"
                                out &= "<td class='cell' >" & fld(3, 0) & "</td>"
                                p = 9
                            End If
                        Else
                            If p > 10 And p <= 14 Then
                                out &= "<td class='cell'>" & col(p) & "</td>"

                            Else
                                out &= "<td class='cell'>" & col(p) & "</td>"

                            End If

                        End If
                    Next
                    out &= "</tr>"
                    If rowspan > 1 Then
                        For k As Integer = 1 To CInt(fld(0, 0)) - 1
                            out &= "<tr>" & _
                            "<td class='cell'><a href='?val=bydis&discipline=" & fld(4, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(4, k) & "</a></td>"
                            out &= "<td class='cell'><a href='?val=byqual&qualification=" & fld(1, k) & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & fld(1, k) & "</a></td>"
                            out &= "<td class='cell'>" & fld(2, k) & "</td>"
                            out &= "<td class='cell'>" & fld(3, k) & "</td></tr>"

                        Next

                    End If
                    i = i + 1
                End If

            End While

            'Response.Write("<table cellpading='2' cellspacing='2' bordercolor='blue'>" & out & "</table>")
        End If
        If out = "" Then
            out = "<tr><td colspan='15' style='color:red;font-size:30pt;'>Sorry! Data are not Found</td></tr>"
        End If
        Return out
    End Function
    Function headermk(ByVal see As String, ByVal title As String)
        Dim rt As String = ""


        rt = "<table id='tb1' border='0' cellpadding='0' cellspacing='0'>" & Chr(13) & _
   " <tr><td colspan='15' style='text-align:center;font-size:16pt; color:Blue'>" & Application("company_name") & title & "</td></tr>" & Chr(13) & _
   " <tr  style='height: 15.75pt; font-size:12pt; color:#020202; font-weight:bold;background-color: #367898'>" & Chr(13) & _
        "<td class='dw' rowspan='2'>" & Chr(13) & _
        "No" & Chr(13) & _
            "</td>" & Chr(13) & _
        "<td class='fxname' rowspan='2'>                   " & Chr(13) & _
            "Full name</td> " & Chr(13) & _
       " <td class='dw' rowspan='2' >" & Chr(13) & _
            "<span style='font-size: 10pt'>" & Chr(13) & _
           " sex</span></td>" & Chr(13) & _
        "<td class='fitx' rowspan='2' >" & Chr(13) & _
           " Employment date</td>" & Chr(13)
        If see = "second" Then
            rt &= "<td class='fitx' rowspan='2' >" & Chr(13) & _
                       " Probation date</td>" & Chr(13)
        End If


        rt &= "<td class='fitx' rowspan='2'>" & Chr(13) & _
            "Field of study</td>" & Chr(13) & _
        "<td class='fitx' rowspan='2' >" & Chr(13) & _
           " Certificate</td>" & Chr(13) & _
        "<td class='fitx' rowspan='2'>" & Chr(13) & _
           " &nbsp;Year of Graduation</td>" & Chr(13) & _
        "<td class='fitx'  rowspan='2'>" & Chr(13) & _
           " &nbsp;Institution</td>" & Chr(13) & _
        "<td class='fitx' rowspan='2' >" & Chr(13) & _
            "Position</td>" & Chr(13) & _
       " <td class='fitx' rowspan='2' >" & Chr(13) & _
            "Salary</td>" & Chr(13) & _
        "<td class='fitx' colspan='2' >" & Chr(13) & _
           " Allowance</td>" & Chr(13) & _
        "<td class='fitx' rowspan='2' >" & Chr(13) & _
            "Perdiem</td>" & Chr(13) & _
        "<td class='projectx' rowspan='2'>" & Chr(13) & _
           " project</td>" & Chr(13) & _
        "<td class='tel' rowspan='2' >" & Chr(13) & _
            "Tel/Mob</td>" & Chr(13) & _
   " </tr>" & Chr(13) & _
    "<tr style='font-weight: bold;   font-family:Times New Roman; height: 31.5pt;" & Chr(13) & _
       "font-size:12pt; color:#020202; font-weight:bold;background-color: #367898'>" & Chr(13) & _
        "<td class='fitx'>" & Chr(13) & _
        "Non" & Chr(13) & _
            "<br />" & Chr(13) & _
            "Taxable</td>" & Chr(13) & _
        "<td class='fitx' >" & Chr(13) & _
            "Taxable</td>" & Chr(13) & _
    "</tr>"

        Return rt
    End Function
End Class
