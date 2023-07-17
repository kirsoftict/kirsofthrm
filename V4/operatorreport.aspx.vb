Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class operatorreport
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
                    "  ORDER BY emp_static_info.first_name, emprec.id DESC"
                    out = makeformx(sql)
                    title &= "<br>List of Emplyee's "
                Case "bydep"

                    Dim dep() As String
                    dep = Request.QueryString("department").Split("|")
                    title &= "<br>List of Emplyee's Filtered by Department " & dep(1) & " " & active
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) order by id desc"
                    Else
                        sql = "select * from emprec where emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) order by id desc"

                    End If

                    ' Response.Write(sql)

                    out = makeformx(sql)
                Case "byprojdate"

                    sql = ""
                    If Request.QueryString("projdateto") = "" And Request.QueryString("projdate") <> "" Then
                        If Request.QueryString("active") <> "" Then
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from >='" & Request.QueryString("projdate") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                        Else
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from >='" & Request.QueryString("projdate") & "') order by id desc"

                        End If
                    ElseIf Request.QueryString("projdateto") <> "" And Request.QueryString("projdate") <> "" Then
                        If Request.QueryString("active") <> "" Then
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                        Else
                            sql = "select * from emprec where id in(select emptid from emp_job_assign where date_from between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') order by id desc"

                        End If
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
                    dep(0) & "' and date_end is null) order by id desc"
                    title &= "<br>List of Emplyee's Filtered by Project " & dep(1) & " " & active
                    ' Response.Write(sql)
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If
                Case "bydis"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Field of Study " & Request.QueryString("discipline") & " " & active
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "') order by id desc"
                    End If

                    If sql <> "" Then
                        out = makeformx(sql)
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
                        out = makeformx(sql)
                    End If
                Case "byqual"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by Qualification " & Request.QueryString("qualification") & " " & active
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') order by id desc"
                    End If
                    If sql <> "" Then
                        out = makeformx(sql)
                    End If 'Response.Write(sql)

                Case "bypost"
                    sql = ""
                    title &= "<br>List of Emplyee's Filtered by position " & Request.QueryString("position") & " " & active
                   
                    sql = "select * from emprec where id in(select emptid from emp_job_assign where position='" & Request.QueryString("position") & "' )  order by id desc"

                    If sql <> "" Then
                        out = makeformx(sql)
                        'Response.Write(sql)
                    End If
                Case "pp"
                    '45 days workers
                    sql = "select * from emprec where dateadd(d,45,hire_date)>'" & Today.ToShortDateString & "'"
                    ''Response.Write(sql)
                    out = makeformx(sql)
                Case "namex"
                    Dim name() As String
                    name = Request.QueryString("vname").Split(" ")
                    If name.Length > 3 Then
                        If name(0).ToString <> " " Then
                            sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%')  order by id desc"

                        End If
                    ElseIf name.Length = 3 Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%' and middle_name like '%" & name(1) & "%' and last_name like '%" & name(2) & "%')  order by id desc"

                    ElseIf name.Length = 1 Then
                        sql = "select * from emprec where emp_id in(select emp_id from emp_static_info where first_name like '%" & name(0) & "%')  order by id desc"

                    End If

                    'Response.Write(sql)
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
                    col(1) = fullname
                    col(2) = fm.getinfo2("select sex from emp_static_info where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    fld = fm.getqualification(dt.Item("emp_id"), Session("con"))
                    col(3) = dt.Item("hire_date")
                    position = fm.getinfo2("select position from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' order by id desc", Session("con"))
                    col(4) = "<a href='?val=bypost&position=" & position.Trim & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & position & "</a>"
                    active = fm.getinfo2("select active from emprec where emp_id='" & dt.Item("emp_id") & "' and end_date is null", Session("con"))
                    col(7) = fm.getinfo2("select htel from emp_address where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    col(8) = fm.getinfo2("select mob from emergency_contact where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    Dim email As String = ""
                    email = fm.getinfo2("select pemail from emp_address where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    col(9) = "<a href='mailto:" & email & "'>" & email & "</a>"

                    col(6) = fm.getinfo2("select mob from emp_address where emp_id='" & dt.Item("emp_id") & "'", Session("con"))
                    Dim projid As String = ""
                    projid = fm.getinfo2("select project_id from emp_job_assign where emp_id='" & dt.Item("emp_id") & "' and date_end is null", Session("con"))
                    proj = fm.getinfo2("select project_name from tblproject where project_id='" & projid & "'", Session("con"))
                    col(5) = "<a href='?val=byproj&projx=" & projid & "|" & proj & "&active=" & Request.QueryString("active") & "' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & proj & "</a>"

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
                    out &= "<tr style=' font-family:Times New Roman; height: 31.5pt;font-size:9pt; mso-height-source: userset;background-color:" & color & "'>" & Chr(13)
                    ' Response.Write(active & "<br>")

                    '  Response.Write(color)
                    For p As Integer = 0 To 9

                        out &= "<td class='xl65' style='border-right: windowtext 1pt solid; border-top: windowtext;" & Chr(13) & _
        "border-left: windowtext 1pt solid; border-bottom: windowtext 1pt solid; height: 29pt; text-align:left; '>" & col(p) & "</td>" & Chr(13)
                            

                    Next
                    out &= "</tr>"
                    
                End If
                i = i + 1
            End While

            'Response.Write("<table cellpading='2' cellspacing='2' bordercolor='blue'>" & out & "</table>")
        End If
        If out = "" Then
            out = "<tr><td colspan='15' style='color:red;font-size:30pt;'>Sorry! Data are not Found</td></tr>"
        End If
        Return out
    End Function
    Function allout(ByVal outp As String)
        Dim all As String = ""
        all &= "<div id='allList'>"
        all &= "<table border='0' cellpadding='0' cellspacing='0' style='width: 600pt; border-collapse: collapse; height: 76px;'>"
        all &= " <tr><td colspan='10' style='text-align:center;font-size:16pt; color:Blue'><%response.write(outp(0)) %></td></tr>"
        all &= " <tr  style='height: 15.75pt; font-size:12pt; color:#020202; font-weight:bold;background-color: #367898'>"
        all &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid;"
        all &= "border-top: windowtext 1pt solid; border-left: windowtext 1pt solid; width: 16pt;"
        all &= "   border-bottom: black 1pt solid; height: 47.25pt;'>"
        all &= " No"
        all &= "  </td>"
        all &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 231pt;"
        all &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>                   "
        all &= " Full name</td>"
        all &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "  font-weight: bold;  border-left: windowtext 1pt solid; width: 34pt;"
        all &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
        all &= "<span style='font-size: 10pt'>"
        all &= "sex</span></td>"
        all &= "<td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= " font-weight: bold;  border-left: windowtext 1pt solid; width: 74pt;"
        all &= "border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
        all &= " Employment date</td>"

        all &= "                <td class='xl67' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "            font-weight: bold;  border-left: windowtext 1pt solid; width: 79pt;"
        all &= "    border-bottom: black 1pt solid;  font-family:Times New Roman; '>"
        all &= "         Position</td>"
        all &= "<td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "    font-weight: bold;  border-left: windowtext 1pt solid; width: 98pt;"
        all &= "border-bottom: windowtext 1pt solid; font-family:Times New Roman; '>"
        all &= "project</td>"
        all &= "<td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "  font-weight: bold;  border-left: windowtext 1pt solid; width: 90pt;"
        all &= "   border-bottom: windowtext 1pt solid;  font-family:Times New Roman; '>"
        all &= " Mob</td>"
        all &= "<td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 90pt;"
        all &= "border-bottom: windowtext 1pt solid;  font-family:Times New Roman; '>"
        all &= " Home Line</td>"
        all &= " <td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 90pt;"
        all &= " border-bottom: windowtext 1pt solid;  font-family:Times New Roman; '>"
        all &= "Emergency Conact</td>"
        all &= "<td class='xl69' rowspan='2' style='border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;"
        all &= "font-weight: bold;  border-left: windowtext 1pt solid; width: 90pt;"
        all &= "border-bottom: windowtext 1pt solid;  font-family:Times New Roman; '>"
        all &= "Email</td>"
        all &= "</tr>"
        all &= "<tr><td>&nbsp;</td></tr>"

        all &= outp
        all &= "</table>"
        all &= " <table><tr><td width='15' style='height:14px; background-color:Red'>&nbsp;</td><td>None Active Employees</td></tr></table>"
        all &= "</div>"
        Return all
    End Function
End Class
