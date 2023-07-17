Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports Microsoft.VisualBasic
Imports Kirsoft.hrm
Partial Class rptpardimview
    Inherits System.Web.UI.Page
    Dim ds As New dbclass
    Dim fm As New formMaker
    Dim emptid As String
    Function outpx()
        ' Response.Write(Request.Form("projname"))
        Dim namelist As String = ""
        Dim arrv(4) As String
        Dim check As String = ""
        Dim cach As String = ""

        Dim i As Integer = 0
        Dim msg As String = ""


        Dim rs As DataTableReader
        Dim emptid As String
        Dim spl(), outp As String
        Dim projid As String = ""
        Dim fdate, edate As Date
        If Request.Form("projname") <> "" Then
            'Response.Write(Request.Form("projname"))
            spl = Request.Form("projname").Split("|")
            If spl.Length <= 1 Then
                ReDim spl(2)
                spl(0) = Request.Form("projname")
                spl(1) = ""
            End If
            outp = ""
            projid = spl(1)
            If Request.Form("month") <> "" And Request.Form("year") <> "" Then
                fdate = Request.Form("month") & "/1/" & Request.Form("year")
                edate = fdate.AddDays(Date.DaysInMonth(Request.Form("year"), Request.Form("month")) - 1)
                Response.Write(edate.ToString) 'del
            Else
                fdate = "1/1/1900"
                edate = "1/1/1900"
            End If
            Response.Write(fdate.ToShortDateString & "===>" & edate.ToShortDateString)
            If Request.Form("paidst") = "no" Then
                mkform(projid, "no", fdate, edate)
            Else
                mkform(projid, "yes", fdate, edate)
            End If
        Else
            Response.Write("project is not selected")
        End If





    End Function
    Function mkform(ByVal projid As String, ByVal ps As String, ByVal fdate As Date, ByVal edate As Date)
        If ps = "no" Then
            mkformps(projid, fdate, edate)
        Else
            mkformview(projid, fdate, edate)
        End If

    End Function
    Function mkformview(ByVal projid As String, ByVal fdate As Date, ByVal edate As Date)
        Dim rs As DataTableReader
        'Response.Write(edate)
        Dim tblmakeflg As Boolean = False
        If fdate <> "1/1/1900" Then
            '  rs = ds.dtmake("selectpar", "select * from pardimpay inner join emprec on emprec.id=pardimpay.emptid INNER JOIN emp_static_info AS empsf ON emprec.emp_id = empsf.emp_id " & _
            '                "where emptid in(select emptid from emp_job_assign where project_id='" & projid & "'" & _
            '               " and ('" & fdate & "' between date_from and isnull(date_end,'" & edate & "') or date_from between '" & fdate & "' and isnull(date_end,'" & edate & "'))) and paid_state='y' and paid_date between '" & fdate & "' and '" & edate & "' order by empsf.first_name", Session("con"))
            rs = ds.dtmake("selectpar", "select * from pardimpay inner join emprec on emprec.id=pardimpay.emptid INNER JOIN emp_static_info AS empsf ON emprec.emp_id = empsf.emp_id " & _
                          "where paid_state='y' and paid_date between '" & fdate & "' and '" & edate & "' order by paid_date,empsf.first_name", Session("con"))

            ' "INNER JOIN emp_static_info AS empsf ON emprec.emp_id = empsf.emp_id " & _
            ' "WHERE (pardimpay.emptid IN(" & _
            ' "select emptid from emp_job_assign inner join emprec on emprec.id=emp_job_assign.emptid " & _
            '  "where project_id='" & projid & "' and ('" & fdate & "' between date_from and isnull(date_end,'" & edate & "') or hire_date<'" & edate & "')) and ('" & fdate & "' between pardimpay.from_date and isnull(pardimpay.to_date,'" & edate & "'))) order by empsf.first_name")
        Else
            '  rs = ds.dtmake("selectpar", "select * from pardimpay where emptid in(select emptid from emp_job_assign where project_id='" & projid & "' and date_end is null) and paid_state='y' order by paid_date desc, ref", Session("con"))
            rs = ds.dtmake("mmkj", "SELECT pardimpay.* FROM pardimpay INNER JOIN emprec ON pardimpay.emptid = emprec.id INNER JOIN emp_static_info ON emprec.emp_id = emp_static_info.emp_id ORDER BY pardimpay.paid_date, emp_static_info.first_name", Session("con"))

        End If
        Dim copy As String = ""
        Dim header As String = ""
        Dim color As String = ""
        Dim amt As Double = 0
        Dim refc As String = ""
        Dim ppd As Date = "1/1/1900"
        Dim ppd2 As Date
        If rs.HasRows = True Then
            header = ""

            Dim flgd As String = 0
            Dim divon As String = ""
            Dim gdate As String = ""
            Dim remid As String
            refc = ""
            Dim rt() As String
            While rs.Read
                emptid = rs.Item("emptid")
                fdate = rs.Item("from_date")
                edate = rs.Item("to_date")
                rt = fm.getproj(emptid, fdate, edate, Session("con"))
                If rt.Length > 0 Then
                    'Response.Write("<br>" & projid & "=" & rt(0))
                    If projid = rt(0) Then
                        'Response.Write("has rows")
                        remid = ""
                        refc = rs.Item("ref")


                        ' ppd = rs.Item("paid_date")
                        ' remid = isinproject2(emptid, fdate, edate, Session("con"))
                        ' Response.Write(projid & "=" & remid & "<br>")
                        ' If remid = projid Then
                        '  Response.Write(remid)
                        ' result()
                        'Response.Write(emptid & "<br>")
                        If copy <> refc Then
                            'gdate &= refc & ","
                            'Response.Write(refc & "<br>")
                            If color = "#aabbbf" Then
                                color = "#C8DBF0"
                                ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                            Else
                                color = "#aabbbf"
                                '  
                            End If
                            ' copy = refc
                            If tblmakeflg = True Then
                                header &= "<tr><td colspan='6' style='text-align:center;cursor:pointer; color:red;' onclick=" & Chr(34) & "javascript:view('view-" & copy & "');" & Chr(34) & " >View</td>" & Chr(13) & _
                                "</tr></table></form></td></tr></table>"
                                gdate &= copy & ","
                                If ppd <> rs.Item("paid_date") Then
                                    header &= "</div>" & Chr(13)
                                    header &= "<script>$('#txt" & ppd.Ticks & "').val('" & gdate & "');</script>"
                                    gdate = ""
                                End If
                                'header &= "<script>$('txt" & ppd & "').val('" & gdate & "');</script>"
                            End If

                            If ppd <> rs.Item("paid_date") Then
                                ' header &= "<script>$('txt" & ppd & "').val('" & gdate & "');</script>"
                                ' gdate = refc & ","
                                ppd2 = rs.Item("paid_date")
                                header &= Chr(13) & "<div id='mainf" & ppd2.Ticks & "' style='width:800px; height:30px; background:url(images/blue_banner-760x147.jpg);color:white;font-size:14pt;cursor:pointer;' title='click' onclick=" & _
                                Chr(34) & "javascript:showMenu('" & ppd2 & "')" & Chr(34) & ">" & Chr(13) & _
                                "<input type='hidden' id='txt" & ppd2.Ticks & "' width='300'>" & _
                                MonthName(ppd2.Month) & " " & ppd2.Day.ToString & ", " & ppd2.Year.ToString & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a id='flton" & ppd2.Ticks & "' href=" & Chr(34) & "javascript:printp('txt" & ppd2.Ticks & "'); " & Chr(34) & "> Print all</a><img id='ic-" & rs.Item("paid_date") & "'  src='images/gif/collapsed_.gif' alt='cc' style='float:right;margin:0px 0px 0px 100px;width:17px; ' /></div>" & Chr(13) & _
                                "<div style='height:5px;'></div>" & Chr(13)
                                header &= Chr(13) & "<div id='" & rs.Item("paid_date") & "' style='width:800px;display:none;'>"
                                ppd = rs.Item("paid_date")
                                flgd = 0
                            End If
                            ' gdate &= refc & ","
                            ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                            header &= "<table id='tb1'><tr style='background:" & color & ";'>" & _
                            "<td colspan='8'>" & Chr(13) & _
        "<table width='500px' align='left'><tr><td>Name</td><td>:</td><td>" & _
        fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & _
        emptid, Session("con")), Session("con")).ToString & _
                            "</td><td>Emp. Id</td><td>:</td><td>" & emptid & _
                            "</td></tr></table>" & Chr(13) & _
                            "</td></tr><tr style='background:" & color & _
                            ";font-weight:bold;'><td colspan='7' style='padding-left:20px;'>" & _
                            "<form id='view-" & refc & "' name='view-" & refc & "' method='post' action=''>" & Chr(13) & _
                            "<table style='border:1px solid black;'>" & Chr(13) & _
                            "<tr><td>Reason</td><td>Date Depart<span style='font-size:8pt; color:gray;'>" & Chr(13) & _
                            "(MM/DD/YYYY)</span></td>" & Chr(13) & _
                            "<td>Date Return<span style='font-size:8pt; color:gray;'>(MM/DD/YYYY)</span></td>" & Chr(13) & _
                            "<td>No. Days</td><td>Per diem</td><td>Amount</td></tr>"
                            copy = refc
                            tblmakeflg = True
                        End If
                        amt = CDbl(rs.Item("no_days")) * CDbl(rs.Item("pardim"))
                        header &= "<tr style='background:" & color & ";'><td>" & rs.Item("reason") & Chr(13) & _
                        "</td><td>" & rs.Item("from_date") & "</td><td>" & rs.Item("to_date") & "</td><td>" & Chr(13) & _
                        rs.Item("no_days") & "</td><td style='text-align:right'>" & _
                        fm.numdigit(rs.Item("pardim"), 2).ToString & "</td>" & Chr(13) & _
                        "<td style='text-align:right;'>" & fm.numdigit(amt, 2).ToString & _
                        "</td><td></td></tr>"

                        ' End If
                        refc = ""
                    End If ' find proj
                End If ' find proj
            End While
            'Response.Write(refc & "===" & copy)
            header &= "<tr><td colspan='6' style='text-align:center; cursor:pointer; color:red;' onclick=" & _
             Chr(34) & "javascript:view('view-" & copy & "');" & Chr(34) & " >view</td></tr></table></form></td></tr></table></div>"
            gdate &= copy & ","
            header &= "<script language='javascript' type='text/javascript'>$('#txt" & ppd.Ticks & "').val('" & gdate & "');</script>"

            header = "<div><style> #tb1  { border:1px solid black;} #tb1 td{ border-bottom:1px solid black; border-right:1px solid black;}</style>" & header & "</div>"
            header &= "<script>$('#txt" & ppd.Ticks & "').val('" & gdate & "');</script>"
            Response.Write(header)
            Response.Write("<form id='frmprint' name='frmprint' method='post' action=''><input type='hidden' id='txtprint' name='txtprint' /></form>")
        Else
            Response.Write("Data is not found")
        End If
    End Function
    Function mkformps(ByVal projid As String, ByVal fdate As Date, ByVal edate As Date)
        Dim copy As String = ""
        Dim header As String = ""
        Dim color As String = ""
        Dim amt As Double = 0
        Dim rs As DataTableReader
        Dim sql As String
        Dim tblmakeflg As Boolean = False
        Try


            If fdate = "1/1/1900" Then
                rs = ds.dtmake("selectpar", "select * from pardimpay where emptid in(select emptid from emp_job_assign where project_id='" & projid & "') and paid_state='n' order by emptid,from_date desc", Session("con"))
            Else
                sql = "select * from pardimpay where emptid in(" & fm.getprojemp(projid, edate, Session("con")) & ") and paid_state='n' order by emptid desc"
                rs = ds.dtmake("selectpar", sql, Session("con"))
            End If
            'rs = ds.dtmake("selectpar", "select * from pardimpay where emptid in(select emptid from emp_job_assign where project_id='" & projid & "') and paid_state='n' order by emptid desc", Session("con"))

            If rs.HasRows = True Then
                header = ""
                header = "<table id='tb1' cellspacing='0' cellpadding='0' width='700px'><tr><td>"
                While rs.Read
                    emptid = rs.Item("emptid")
                    If copy <> emptid Then

                        If color = "#aabbbf" Then
                            color = "#C8DBF0"
                            ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                        Else

                            color = "#aabbbf"
                            '  
                        End If
                        ' header &= "<table class='inner' cellspacing='0' cellpadding='0' width='700px' style='background:" & color & ";'>"
                        'Response.Write("innnner")
                        If tblmakeflg = True Then
                            header &= "<tr>" & _
                                       "<td colspan='5'><select name='mthd' id='mthd'>" & _
                                       "<option value='Cheque'>Cheque</option>" & _
                                       "<option value='Bank'>Bank</option>" & _
                                        "<option value='Cash'>Cash</option></select>" & _
                                        "<input type='text' id='paiddate' name='paiddate' value='" & Today.Month & "/" & Today.Day & "/" & Today.Year & "'></td>" & _
                                        "<script language='javascript' type='text/javascript'> $(function() {$( '#paiddate-" & copy & "').datepicker({changeMonth: true,changeYear: true	}); $( '#paiddate-" & copy & "' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
                                        "<td colspan='' style='text-align:center;cursor:pointer; color:red;' onclick=" & Chr(34) & _
                                        "javascript:findid('form-" & copy & "');" & Chr(34) & _
                                        " >Paid</td></tr></table></form></td></tr></table>"
                        End If
                        ' header &= "<tr><td colspan='4' style='text-align:center;'>Paid</td></tr></table>"
                        header &= "<table class='inner'><tr style='background:" & color & ";'><td colspan='8'><table width='500px' align='left'>" & _
                        "<tr><td>Name</td><td>:</td><td>" & _
                        fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con")).ToString & _
                        "</td><td>Emp. Id</td><td>:</td><td>" & emptid & _
                        "</td></tr></table></td></tr><tr style='background:" & color & _
                        ";font-weight:bold;'><td colspan='7' style='padding-left:20px;'>" & _
                        "<form id='form-" & emptid & "' name='form-" & emptid & _
                        "' method='post' action=''><table style='border:1px solid black;'><!--input type='hidden' name='nextpage' id='nextpage'--><tr><td>Reason</td><td>Date Depart<span style='font-size:8pt; color:gray;'>(MM/DD/YYYY)</span></td><td>Date Return<span style='font-size:8pt; color:gray;'>(MM/DD/YYYY)</span></td><td>No. Days</td><td>Per diem</td><td>Amount</td></tr>"
                        copy = emptid
                        tblmakeflg = False
                    End If


                    amt = CDbl(rs.Item("no_days")) * CDbl(rs.Item("pardim"))
                    header &= "<tr style='background:" & color & ";'><td>" & _
                    rs.Item("reason") & "</td><td>" & rs.Item("from_date") & "</td><td>" & _
                    rs.Item("to_date") & "</td><td>" & rs.Item("no_days") & _
                    "</td><td style='text-align:right'>" & fm.numdigit(rs.Item("pardim"), 2).ToString & _
                    "</td><td style='text-align:right;'>" & fm.numdigit(amt, 2).ToString & _
                    "</td><td><input type='checkbox' name='paid-" & emptid & "-" & _
                    rs.Item("id") & "' id='paid-" & emptid & "-" & rs.Item("id") & "' value='" & _
                    emptid & "-" & rs.Item("id") & "'/></td></tr>"
                    tblmakeflg = True



                End While
                header &= "<tr>" & _
               "<td colspan='5'><select name='mthd' id='mthd'>" & _
               "<option value='Cheque'>Cheque</option>" & _
               "<option value='Bank'>Bank</option>" & _
                "<option value='Cash'>Cash</option></select>" & _
                "<input type='text' id='paiddate' name='paiddate' value='" & Today.Month & _
                "/" & Today.Day & "/" & Today.Year & "'></td>" & _
                "<script language='javascript' type='text/javascript'> $(function() {$( '#paiddate').datepicker({changeMonth: true,changeYear: true	}); $( '#paiddate' ).datepicker( 'option','dateFormat','mm/dd/yy');});</script>" & _
                "<td colspan='' style='text-align:center;cursor:pointer; color:red;' onclick=" & Chr(34) & _
                "javascript:findid('form-" & emptid & "');" & Chr(34) & _
                " >Paid</td></tr></table></form></table>"
                header = "<div><style> #tb1  { border:1px solid black;} #tb1 td{ border-bottom:1px solid black; border-right:1px solid black;}</style>" & header & "</div>"
                Response.Write(header)
            Else
                Response.Write("Data is not found")
            End If
        Catch ex As Exception
            Response.Write(ex.ToString & sql)
        End Try
        Return Nothing
    End Function
    Function isinproject2(ByVal emptid As String, ByVal fdate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        rs = dbs.dtmake("vwisproj", "select * from emp_job_assign where emptid=" & emptid & " and '" & fdate & "' between date_from and isnull(date_end,'" & Today.ToShortDateString & "') or date_from  between '" & fdate & "' and isnull(date_end,'" & Today.ToShortDateString & "')  order by date_from", con)
        If rs.HasRows = True Then
            rs.Read()
            Return rs.Item("project_id")
        Else
            Return "None"
        End If
    End Function
    Function isinproject2(ByVal emptid As String, ByVal fdate As Date, ByVal edate As Date, ByVal con As SqlConnection)
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        rs = dbs.dtmake("vwisproj", "select * from emp_job_assign where emptid=" & emptid & " and '" & fdate & "' between date_from and isnull(date_end,'" & edate & "') or date_from  between '" & fdate & "' and isnull(date_end,'" & edate & "')  order by date_from", con)
        If rs.HasRows = True Then
            rs.Read()
            Return rs.Item("project_id")
        Else
            Return "None"
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class


