Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class empidmake
    Inherits System.Web.UI.Page
    Dim logo, image, name, position, adwd, adkb, adsub, hno, mob, hp, fname, emp_id As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("val") <> "" Then
            Dim dbs As New dbclass
            Dim fm As New formMaker
            Dim spl() As String
            Dim fullname As String
            Dim sql As String = ""
            Dim address As String
            fullname = Request.QueryString("vname")
            spl = fullname.Split(" ")
            If spl.Length > 2 Then
                sql = "select * from emp_static_info where first_name='" & spl(0).Trim & "' and middle_name='" & spl(1).Trim & "' and last_name='" & spl(2).Trim & "'"
                Dim rs As DataTableReader
                rs = dbs.dtmake("mdk", sql, Session("con"))
                If rs.HasRows Then
                    rs.Read()
                    Dim rs2 As DataTableReader
                    rs2 = dbs.dtmake("mm", "select * from emp_address where emp_id='" & rs.Item("emp_id") & "' order by id desc", Session("con"))
                    If rs2.HasRows Then
                        rs2.Read()
                        Me.idview.Text = "<style> .pd div{ padding-top:4px;}</style>" & _
                        "<div id='bcard'>"
                        Me.idview.Text &= "<div class='frmcom'>" & Chr(13) & _
                        "<div style='float:left;width:10%'>" & Chr(13) & _
                        "<img src='" & Application("logo") & "' width='50px' style='float:left;'>" & _
                        "</div>" & Chr(13) & _
                        "<div style='width:90%;float:left;'>" & Chr(13) & _
"<span class='cname'>" & Session("company_name") & "</span><br>" & _
                        "<span class='baddress'>" & Application("baddress") & "</span>" & _
                        "<div style='padding:3px 3px 3px 3px;clear:both;'>" & Chr(13) & "<label class='lb'><b>Emp. ID. </b></label><u><i>" & rs.Item("emp_id") & "</i></u></div>" & _
                        "</div>" & Chr(13)
                        Me.idview.Text &= "</div>" & Chr(13) & "<div style='clear:left'></div>" & Chr(13) & _
                        "<div  class='dcont' style='clear:left;'>" & Chr(13) & _
                        "<div class='photo' style='float:left;padding:4px 4px 4px 4px;'>" & Chr(13) & _
"<img src='" & rs.Item("imglink") & "' width='20%'></div>" & Chr(13) & _
                        "<div style='float:left;width:75%;' class='detail'>" & Chr(13) & _
                        "<div style='padding-top:3px;'>" & Chr(13) & "<span class='lbsp'>Name:</span><span class='de'><u><i>" & fullname & "</i></u></span></div>" & Chr(13) & _
                        "<div style='padding-top:3px;'>" & Chr(13) & "<span class='lbsp'>Position:</span><span class='de'><u><i>"
                        Dim dt As DataTableReader
                        Dim emptdate As Date
                        Dim resdate As Date
                        Dim emptid As String
                        Dim dptname As String
                        Dim position As String
                        Dim projid As String
                        dt = dbs.dtmake("emprec his", "select * from emprec where emp_id='" & rs.Item("emp_id") & "' order by id desc ", Session("con"))
                        If dt.HasRows Then
                            dt.Read()

                            emptdate = dt.Item("hire_date")
                            If dt.IsDBNull(4) = False Then
                                resdate = dt.Item("end_date")
                            End If
                            emptid = dt.Item("id")
                            dt.Close()
                            sql = "Select ass_for as 'Assignment Type',position as 'Position',department as 'Department',date_from as 'Start Date',date_end as 'Last Date',project_id as project from emp_job_assign where emptid=" & emptid & " order by id desc"
                            dt = dbs.dtmake("jobassign", sql, Session("con"))

                            If dt.HasRows Then
                                dt.Read()
                                dptname = dbs.getdepname(dt.Item("department"), Session("con"))
                                position = dt.Item("position").ToString
                                projid = dt.Item("project").ToString
                            End If


                        End If
                        dt.Close()

                        Me.idview.Text &= position & "</i></u></span></div>" & Chr(13) & _
                                                "<div  style='padding-top:3px;'>" & Chr(13) & "<span class='lbsp'>Hire Date:</span><span class='de'><u><i>" & Format(emptdate, "MMM d, yyyy") & "</i></u></span></div>" & Chr(13) & _
                                                "<div style='padding-top:3px;'>" & Chr(13) & "<label class='lb'>Address: <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subcity:</label><span style='padding-left:10px;'><u><i>" & Chr(13)
                        If rs2.IsDBNull(10) = False Then
                            Me.idview.Text &= rs2.Item(10)
                        Else
                            Me.idview.Text &= "-"
                        End If
                        Me.idview.Text &= "</i></u></span>" & Chr(13) & "<label class='lb' >&nbsp;&nbsp;&nbsp;&nbsp; Worda.</label><span style='padding-left:10px;'><u><i>"
                        If rs2.IsDBNull(11) = False Then
                            Me.idview.Text &= rs2.Item(11)
                        Else
                            Me.idview.Text &= "-"
                        End If
                        Me.idview.Text &= "</i></u></span>" & Chr(13) & "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label class='lb'> Kebele.</label><span style='padding-left:10px;'><u><i>"
                        If rs2.IsDBNull(12) = False Then
                            Me.idview.Text &= rs2.Item(12)
                        Else
                            Me.idview.Text &= "-"
                        End If
                        Me.idview.Text &= "</i></u></span>" & Chr(13) & "<label class='lb'>&nbsp;&nbsp;&nbsp;&nbsp; H.No.</label><span style='padding-left:10px;'><u><i>"
                        If rs2.IsDBNull(13) = False Then
                            Me.idview.Text &= rs2.Item(13)
                        Else
                            Me.idview.Text &= "-"
                        End If
                        Me.idview.Text &= "</i></u></span></div>" & Chr(13) & " </div>" & Chr(13) & "<div style='clear:both;'><label class='lb'>Mob.</label><u><i>"
                        If rs2.IsDBNull(6) = False Then
                            Me.idview.Text &= rs2.Item(6)
                        Else
                            Me.idview.Text &= "-"
                        End If
                        Me.idview.Text &= "</i></u>&nbsp;<label class='lb'>H.Phone.</label><u><i>" & Chr(13)
                        If rs2.IsDBNull(4) = False Then
                            Me.idview.Text &= rs2.Item(4)
                        Else
                            Me.idview.Text &= "-"
                        End If
                        Me.idview.Text &= "</i></u></div>" & Chr(13) & "&nbsp;</div>" & Chr(13)
                        Me.idview.Text &= "</div>" & Chr(13)
                    End If
                    rs2.Close()
                End If
                rs.Close()
            End If
            dbs = Nothing
            fm = Nothing
        End If
        Me.idview.Text = ""
    End Sub
    Function calledr()
        Return Me.idview.Text
    End Function
    Function idtebl()
        Dim dbs As New dbclass
        Dim fm As New formMaker
        Dim spl() As String
        Dim fullname As String
        Dim sql As String = ""
        Dim address As String
        Dim rtn As String = ""
        fullname = Request.QueryString("vname")
        spl = fullname.Split(" ")
           logo = Application("logo")
        Image = ""
        position = ""
        adwd = ""
        adkb = ""
        adsub = ""
        hno = ""
        mob = ""
        hp = ""
        If spl.Length > 2 Then
            sql = "select * from emp_static_info where first_name='" & spl(0).Trim & "' and middle_name='" & spl(1).Trim & "' and last_name='" & spl(2).Trim & "'"
            Dim rs As DataTableReader
            rs = dbs.dtmake("mdk", sql, Session("con"))
            If rs.HasRows Then
                rs.Read()
                Dim rs2 As DataTableReader
                rs2 = dbs.dtmake("mm", "select * from emp_address where emp_id='" & rs.Item("emp_id") & "' order by id desc", Session("con"))
                If rs2.HasRows Then
                    rs2.Read()
                    emp_id = (rs.Item("emp_id"))
                    image = rs.Item("imglink")
                    fname = fullname
                    Dim dt As DataTableReader
                    Dim emptdate As Date
                    Dim resdate As Date
                    Dim emptid As String
                    Dim dptname As String
                    Dim projid As String
                    dt = dbs.dtmake("emprec his", "select * from emprec where emp_id='" & rs.Item("emp_id") & "' order by id desc ", Session("con"))
                    If dt.HasRows Then
                        dt.Read()

                        emptdate = dt.Item("hire_date")
                        If dt.IsDBNull(4) = False Then
                            resdate = dt.Item("end_date")
                        End If
                        emptid = dt.Item("id")
                        dt.Close()
                        sql = "Select ass_for as 'Assignment Type',position as 'Position',department as 'Department',date_from as 'Start Date',date_end as 'Last Date',project_id as project from emp_job_assign where emptid=" & emptid & " order by id desc"
                        dt = dbs.dtmake("jobassign", sql, Session("con"))

                        If dt.HasRows Then
                            dt.Read()
                            dptname = dbs.getdepname(dt.Item("department"), Session("con"))
                            position = dt.Item("position").ToString
                            projid = dt.Item("project").ToString
                        End If


                    End If
                    dt.Close()

                    If rs2.IsDBNull(10) = False Then
                        adsub = rs2.Item(10)
                    Else
                        adsub = "-"
                    End If

                    If rs2.IsDBNull(11) = False Then
                        adwd = rs2.Item(11)
                    Else
                        adwd = "-"
                    End If
                      If rs2.IsDBNull(12) = False Then
                        adkb = rs2.Item(12)
                    Else
                        adkb = "-"
                    End If
                      If rs2.IsDBNull(13) = False Then
                        hno = rs2.Item(13)
                    Else
                        hno = "-"
                    End If
                       If rs2.IsDBNull(6) = False Then
                        mob = rs2.Item(6)
                    Else
                        mob = "-"
                    End If
                      If rs2.IsDBNull(4) = False Then
                        hp = rs2.Item(4)
                    Else
                        hp = "-"
                    End If
                   
                End If
                rs2.Close()
            End If
            rs.Close()
            rtn = Chr(13) & _
          " <div>" & Chr(13) & _
            "<table cellpadding='0' cellspacing='0' id='style1' style='border:1px solid black;'>" & Chr(13) & _
                   "<tr>" & Chr(13) & _
                       "<td colspan='5' valign='top'>" & Chr(13) & _
                       "<table style='border-bottom:1px solid black;'><tr><td valign='top' width='40px'>" & Chr(13) & _
                       "<img width='40px' src='" & logo & "' /></td><td width='90%'><center>" & Chr(13) & _
                    " <span class='headern'> " & Application("company_name_amharic") & "<br>" & Application("company_name") & Chr(13) & _
                              "</span>" & Chr(13) & _
                         "</center></td></tr></table></td>" & Chr(13) & _
                   "</tr> <tr><td colspan='5'><table width='100%'><tr><td>" & _
"<center><span style='font-size:8pt;'>" & Application("baddress") & "</span></center></td>" & Chr(13) & _
                                  "                      </tr></table></td></tr>                  <tr>" & Chr(13) & _
                       "<td rowspan='5' valign='top' width='60'>" & Chr(13) & _
                          "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <img width='80px' src='" & image & "' alt='image'/>" & Chr(13) & _
                       "</td>" & Chr(13) & _
                             " <td class='lbl'>" & Chr(13) & _
            "               የመ.ቁ  <br> Emp. ID    " & Chr(13) & _
                       "</td>" & Chr(13) & _
"                       <td >:" & Chr(13) & _
                     "           </td>" & Chr(13) & _
             "          <td colspan='2' class='datav'>&nbsp;" & Chr(13) & _
                                 emp_id & Chr(13) & _
                    "   </td></tr><tr>" & Chr(13) & _
                      " <td class='lbl'>" & Chr(13) & _
            "                       ሙሉ ስም <br>  Full Name" & Chr(13) & _
                       "</td>" & Chr(13) & _
"                       <td >" & Chr(13) & _
             "              :" & Chr(13) & _
            "           </td>" & Chr(13) & _
             "          <td colspan='2' class='datav'>" & Chr(13) & _
                                  fname & Chr(13) & _
                    "   </td>" & Chr(13) & _
                   "</tr>" & Chr(13) & _
                  " <tr>" & Chr(13) & _
                       "<td class='lbl' >" & Chr(13) & _
                         "የሥራ መደብ<br> Position " & Chr(13) & _
             "          </td>" & Chr(13) & _
            "           <td>" & Chr(13) & _
                                    ":" & Chr(13) & _
                       "</td>" & Chr(13) & _
                       "<td colspan='2' class='datav' >" & position & Chr(13) & _
                      " </td>" & Chr(13) & _
            "       </tr>" & Chr(13) & _
        "       </table>" & Chr(13) & _
                   "</div>" & Chr(13)
        End If

        dbs = Nothing
        fm = Nothing
        Me.idview.Text = rtn
        Return rtn
    End Function
    Function backpage()
        Dim rtn As String = ""
        rtn = Chr(13) & _
   " <div>" & Chr(13) & _
     "<table cellpadding='0' cellspacing='0' id='backpage' style='border:1px solid black;'>" & Chr(13) & _
             "      <tr><td><table><tr>" & Chr(13) & _
            "           <td class='lbl'>" & Chr(13) & _
                         "                 Address</td>" & Chr(13) & _
            "           <td >" & Chr(13) & _
                                          ":" & Chr(13) & _
                                          "     </td>" & Chr(13) & _
            "           <td colspan='5' >" & Chr(13) & _
                                     "          </td>" & Chr(13) & _
              "     </tr>" & Chr(13) & _
             "      <tr>" & Chr(13) & _
            "           <td class='lbl'>              " & Chr(13) & _
                         "             ክ/ከተማ <br>Subcity" & Chr(13) & _
            "                   </td><td class='datav'>:" & adsub & "</td>" & Chr(13) & _
            "           <td class='lbl'>" & Chr(13) & _
               "                           ወረዳ <br>Worda.<td class='dabav'>" & adwd & "</td>" & Chr(13) & _
            "           <td class='lbl'>ቀበሌ<br>Kebele</td><td class='datav'>" & adkb & "</td>" & Chr(13) & _
            "           <td class='lbl'>የቤት ቁ. <br>H.No.</td> <td class='datav'>" & hno & Chr(13) & _
               "        </td>" & Chr(13) & _
              "     </tr></table></td></tr>" & Chr(13) & _
             "      <tr>" & Chr(13) & _
            "           <td ><table><tr><td class='lbl'>" & Chr(13) & _
                         "Mob.</td><td class='datav'>" & mob & "</td>&nbsp;&nbsp;&nbsp;" & Chr(13) & _
            "                   <td class='lbl'> H.Phone.</td><td class='datav'>" & hp & Chr(13) & _
                      "             </td>" & Chr(13) & _
         "          </tr></table></td></tr>" & Chr(13) & _
            "<tr>" & Chr(13) & _
                "<td><table><tr><td><br>Date of Issue______________________<br></td></tr><tr><td><br>" & _
                "Authorized Signature & Seal ____________________</td></tr><tr><td>" & _
                "We here by that the brearer of the Identification card is our Employee" & _
"</td></tr></table></td></tr>" & _
 "       </table>" & Chr(13) & _
            "</div>" & Chr(13)
        Return rtn
    End Function
End Class
