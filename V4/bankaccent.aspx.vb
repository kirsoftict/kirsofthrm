Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class bankaccent
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
                    Dim saveok As String = ""
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
                            'Response.Write(da)

                        End If
                        dtt.Close()
                        saveok = fm.getinfo2("select id from empbank where accountno='" & Request.QueryString("accountno") & "'", Session("con"))
                        If saveok = "None" Then
                            If emptid > 0 Then
                                ' Response.Write(Request.QueryString("st"))
                                sql = "insert into empbank(emptid,bankname,branch,accountno,date_from,active,who_reg,date_reg) " & _
                                "values('" & emptid & "','" & Request.QueryString("bankname") & "','" & Request.QueryString("branch") & "','" & _
                                Request.QueryString("accountno") & "','" & Request.QueryString("date_from") & "','" & Request.QueryString("active") & "','" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')"
                                ' Response.Write(sql)

                                flg = dbx.save(sql, session("con"), session("path"))
                                'Response.Write(flg)
                                If flg = 1 Then
                                    msg = "Data Saved"
                                End If
                            Else
                                msg = "The employee not existed, please re enter the name"
                            End If
                        Else
                            msg = "Account No. Alread Existed"
                        End If

                    End If

                    'MsgBox(rd)

                    ' sql = db.makest(tbl, Request.QueryString, session("con"), key)

                End If



            End If
        End If

    End Function
    Function viewforedit(ByVal pproj As String, ByVal pdate1 As Date, ByVal pdate2 As Date)
        If pproj <> "" Then
            Dim fm As New formMaker

            Dim reg, sun, nig, hd As String
            Dim sumreg, sumsun, sumnig, sumhd, sal(2), otbir, hr, shr, fhrs As String
            Dim i As Integer = 1
            otbir = "0"
            Dim dbx As New dbclass
            Dim rs As DataTableReader
            Dim spl() As String
            Dim projid As String = ""
            Dim nod As Integer
            Dim sqlx As String
            If pproj <> "" Then 'Then
                spl = pproj.Split("|")
                projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
            End If

            Dim rs2 As DataTableReader
            If projid = "" Then
                rs2 = dbx.dtmake("selc_rec", "select * from emprec ", session("con"))
            Else
                'rs2 = dbx.dtmake('selc_rec', 'select * from emprec where end_date is null', Session('con'))
                Dim listinproj As String = fm.getprojemp(projid, pdate2, Session("con"))
                rs2 = dbx.dtmake("selc_rec", "select * from emprec as erc inner join emp_static_info as esi on esi.emp_id = erc.emp_id where erc.id in(" & listinproj & ") order by esi.first_name", Session("con"))
                ' Response.Write('select * from emprec where end_date is null and id in(select emptid from emp_job_assign project_id='' & projid & '')')
            End If
            Me.outp.Text = "<div id='printview'>"
            Me.outp.Text &= "  <style> " & Chr(13) & _
                ".tb1" & Chr(13) & _
            "{" & Chr(13) & _
               " border:1px solid black;" & Chr(13) & _
               " font-size:10pt;" & Chr(13) & _
            "}" & Chr(13) & _
            ".tb1 td" & Chr(13) & _
            "{" & _
             "border-top: 1px solid black;" & Chr(13) & _
             "border-left:1px solid black;" & Chr(13) & _
               " font-size:9pt;" & Chr(13) & _
            "}" & Chr(13) & _
"          </style>" & Chr(13) & _
" <table class='tb1' id='tblot' width='550px' cellpadding='0' cellspacing='0'>" & Chr(13) & _
 "<tr id='r0' style='text-align:center;font-weight:bold;' >" & Chr(13) & _
              " <td id='h1' style='text-align:center;font-weight:bold;font-size:10pt' colspan='5' >" & Session("company_name") & _
               "<br /> Project Name:"
            If projid <> "" Then
                Me.outp.Text &= (spl(0).ToString)
            Else
                Me.outp.Text &= ("All Projects")
            End If
            Me.outp.Text &= "<br /> Bank Account</td>" & _
"              </tr>" & Chr(13) & _
"<tr id='r1' style=' text-align:center;'>" & Chr(13) & _
   " <td id='h2-1'><span class='headtxt'><strong>&nbsp;No</strong></span></td>" & Chr(13) & _
    "<td id='h2-2'><span class='headtxt'><strong>&nbsp;Employee Name</strong></span></td>" & Chr(13) & _
   "    <td id='h2-5'><span class='headtxt'><strong>Bank</strong></span></td>" & Chr(13) & _
    "<td id='h2-6'><span class='headtxt'><strong>Branch</strong></span></td>" & Chr(13) & _
     "<td id='h2-3' ><span class='headtxt'><strong>&nbsp;Account No</strong></span></td>" & Chr(13) & _
  "</tr>" & Chr(13)
            Dim sec As New k_security
            Dim fullname As String

            If rs2.HasRows Then
                otbir = "0"
                Dim rowc, colc As Integer
                rowc = 0

                Dim rt() As String
                While rs2.Read
                    fullname = ""
                    'Response.Write("select * from empbank where emptid='" & rs2.Item("id") & "' and ISNULL(date_from,date_reg) <= '" & pdate1 & "' order by date_from desc<br>")

                    rs = dbx.dtmake("bankacc", "select * from empbank where emptid='" & rs2.Item("id") & "' and ISNULL(date_from,date_reg) <= '" & pdate1 & "' order by date_from desc", Session("con"))


                            If rs.HasRows Then
                                rowc = rowc + 1
                                rs.Read()
                        sqlx = "select id,Bankname,branch,accountno,date_from from empbank where emptid='" & rs2.Item("id") & "'"
                                sqlx = sec.dbStrToHex(sqlx)
                                Dim wd As Date
                                reg = ""
                                sun = ""
                                nig = ""
                                hd = ""
                                sumreg = ""
                                sumnig = ""
                                sumhd = ""
                                sumsun = ""
                                fullname = fm.getfullname(rs2.Item("emp_id"), Session("con"))
                                Me.outp.Text &= "  <tr style='font-size:7pt;font-weight:none' id='" & rowc.ToString & "'>" & Chr(13) & _
                                " <td id='" & rowc.ToString & "-1'><span style='color:blue;cursor:pointer;' onclick=" & Chr(34) & _
                                "edit('" & sqlx & "','" & pdate1 & "','" & pdate2 & "','" & fullname & " ') " & _
                                Chr(34) & " title='Edit/Delete'>" & rowc.ToString & "</span></td>" & Chr(13) & _
                                 "<td id='" & rowc.ToString & "-2'>&nbsp;" & fullname & " </td>" & Chr(13) & _
           " <td style='text-align:right'>&nbsp;" & Chr(13)
                               
                                Me.outp.Text &= rs.Item("bankname") & "</td>"

                                Me.outp.Text &= "<td id='" & rowc.ToString & "-3'>&nbsp;"
                               
                                Me.outp.Text &= rs.Item("branch") & "</td>"
                                Me.outp.Text &= " <td id='" & rowc.ToString & "-4'>&nbsp;"
                                

                                Me.outp.Text &= rs.Item("accountno") & "</td>"
                               
                               
                                Me.outp.Text &= "  </tr>"
                                i += 1
                            End If
                    
                End While
            End If
           
            Me.outp.Text &= "</table>" & _
" <div id='print'  style=' width:59px; height:33px; color:Gray;cursor:pointer' onclick=" & Chr(34) & "javascirpt:print('printview','Report_print','" & Session("company_name") & "','" & Today.ToLongDateString & "');" & Chr(34) & "><img src='images/ico/print.ico' alt='print'/>print</div>" & Chr(13)

        End If
    End Function
    
    
    
    Function updatedb(ByVal pproj As String, ByVal pdate1 As Date, ByVal pdate2 As Date)
        Dim dbx As New dbclass
        Dim rs As DataTableReader
        Dim spl() As String
        Dim projid As String = ""
        Dim nod As Integer
        Dim sqlx As String
        If pproj <> "" Then 'Then
            spl = pproj.Split("|")
            projid = spl(1) 'fm.getinfo2('select project_id from tblproject where project_name='' & Request.Form('projname') & '' order by Project_end', session('con'))
        End If



        rs = dbx.dtmake("selc_rec", "select * from emp_ot where ot_date between '" & pdate1 & "' and '" & pdate2 & "'", session("con"))
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
                sal = dbx.getsal(emptid, pdate1, session("con"))
                hr = sal(0) / 200.67
                rate = rs.Item("rate")
                timedif = rs.Item("time_diff")
                oldamt = rs.Item("amt")

                amt = FormatNumber((hr * rate * timedif), 2)
                If oldamt <> amt Then
                    sqlx = "Update emp_ot set amt='" & amt & "' where id=" & rs.Item("id")
                    dbx.excutes(sqlx, Session("con"), Session("path"))
                End If

            End While
        End If
        rs.Close()
        dbx = Nothing

    End Function
    
End Class
