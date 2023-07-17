Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports kirsoft.hrm
Partial Class process
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Write(Session("path"))
        Dim db As New dbclass
        Dim fm As New formMaker
        Dim fl As New file_list
        Session("conxstr") = System.Configuration.ConfigurationManager.ConnectionStrings("contractcon").ToString() ' "Data Source=.\SQLEXPRESS;AttachDbFilename=" & Session("path") & "\app_data\hrmp.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True"
        Session("conx") = New SqlConnection(Session("conxstr"))
        'Session("con") = con
        Session("conx").open()
        Dim fld(6) As String
        If LCase(Session("emp_iid")) = "admin" Then
            fld(0) = Session("path") & "/employee/" & Session("emp_iid")
            fld(1) = fld(0) & "/" & Session("username")
        Else
            fld(0) = Session("path") & "/employee/" & Session("emp_iid")
            fld(1) = fld(0)
        End If

        fld(2) = fld(1) & "/log"
        fld(3) = fld(1) & "/notification"
        fld(4) = fld(1) & "/email"
        fld(5) = fld(1) & "/js"
        Dim sql As String
        Dim sdate, fdate As Object
        Try
            If Session("conx").state = ConnectionState.Open Then
                Response.Write("<br>it is open</br>")
            End If

            For i As Integer = 0 To UBound(fld)
                If Directory.Exists(fld(i)) = False Then
                    fl.makedir(fld(i))
                    Response.Write(fld(i) & " Created <br>")
                Else
                    Response.Write("already Created!<br>")
                End If
            Next
            Dim js As String
            If LCase(Session("emp_iid")) = "admin" Then
                sql = "select sys_date_time from cm_daily_sch where sch_by='" & Session("username") & "' order by id desc"
                sdate = (fm.getinfo2(sql, Session("conx")))
                If sdate.ToString = "None" Then
                    Response.Write("File is not exist")
                End If
                '  sdate = sdate.ToShortDateString
            Else
                sdate = fm.getinfo2("select sys_date_time from cm_daily_sch where sch_by='" & Session("emp_iid") & "' order by id desc", Session("conx"))
                '  sdate()
            End If
            fdate = "#1/1/2000 00:00:00#"
            If IsDate(sdate) = False Then
                sdate = "#1/1/2000 00:00:00#"
            End If
            If fl.hasfile(fld(5)) = True Then
                Response.Write(fld(5))
                If File.Exists(fld(5) & "/schedule.js") Then
                    fdate = fl.creatdate(fld(5) & "/schedule.js")
                End If
            End If
            If DateDiff(DateInterval.Minute, fdate, sdate) > 0 Then
                If sdate <> "#1/1/2000 00:00:00#" Then
                    Response.Write(DateDiff(DateInterval.Minute, sdate, fdate) & " on jsfile")

                End If


            End If
            Response.Write(DateDiff(DateInterval.Minute, sdate, fdate) & " on jsfile")
            If fdate = sdate Then

                Dim intistring As String = "$(function() {" & _
    "	var todayDate = moment().startOf('day');" & _
    "var YM = todayDate.format('YYYY-MM');" & _
    "var YESTERDAY = todayDate.clone().subtract(1, 'day').format('YYYY-MM-DD');" & _
    "var TODAY = todayDate.format('YYYY-MM-DD');" & _
    "var TOMORROW = todayDate.clone().add(1, 'day').format('YYYY-MM-DD');" & _
    "$('#calendar').fullCalendar({" & _
    "header: {" & _
    "left:           'prev,next today'," & _
    "center:         'title'," & _
    "right:          'month,agendaWeek,agendaDay,listWeek'" & _
    "}," & _
    " editable: true," & _
    "eventLimit: true, // allow more link when too many events" & _
    "navLinks: true," & _
    "backgroundColor:  '#1f2e86',  " & _
    "eventTextColor:  '#1f2e86'," & _
    "textColor:      '#378006'," & _
    " dayClick: function(date, jsEvent, view) {" & _
     " alert('Clicked on: ' + date.format());" & _
        "alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);" & _
    "        alert('Current view: ' + view.name);" & _
       " &  // change the day's background color just for fun" & _
       " $(this).css('background-color', 'red');" & _
    " },events:[])});"
                File.WriteAllText(fld(5) & "/schedule.js", intistring)
                Response.Write("file iserted into " & fld(5) & "/schedule.js")
            End If

            Response.Write(sdate.ToString & "=======" & fdate)
        Catch ex As Exception
            Response.Write("Eror occor<br>" & ex.ToString & "<br>" & sdate.ToString & "=======" & fdate & "<br \>" & sql)
        End Try


    End Sub
End Class
