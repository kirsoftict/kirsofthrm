Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.SessionState
Imports System.IO
Imports System.Security.AccessControl
Imports Kirsoft.hrm
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Function groupmenux(ByVal con As SqlConnection, ByVal auth() As String)
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim rs, rs2 As DataTableReader
        Dim rtn As String = ""
        rs = dbx.dtmake("submenu", "select * from menu where menu_id>0 and menu_id<=99 and publish='y'", con)
        If rs.HasRows Then
            Dim hmenu As String
            While rs.Read
                hmenu = rs.Item("menu_id")
                rs2 = dbx.dtmake("summ", "select * from menu where menu_id like '" & hmenu & "%' and publish='y' order by groupby,menu_order", con)
                rtn &= "<div class='submenukir' id='sub" & rs.Item("menu_id") & _
                    "' style='visibility:hidden; left:0px; top:0px;background-image:url(images/menubg.jpg); position:fixed;" & _
"border-left:gray 1px solid;border-right:black 1px solid;border-bottom:1px solid gray;width:25px auto;height:auto;'" & _
                                    " onmouseover=" & Chr(34) & "javascript:showobjar('m" & rs.Item("menu_id") & "','sub" & rs.Item("menu_id") & "',0,18);" & Chr(34) & _
                                    " onmouseout=" & Chr(34) & "javascript:ha('sub" & rs.Item("menu_id") & "');" & Chr(34) & ">" & Chr(13)

                If rs2.HasRows Then
                    Dim ot As String = ""
                    Dim gr As String = ""
                    Dim cc1 As Integer = 4
                    Dim cc As Integer
                    While rs2.Read

                        If rs2.IsDBNull(8) Then
                            ot = Chr(12)
                        Else
                            ot = rs2.Item("groupby")
                        End If
                        If gr = "" Then
                            gr = ot
                            rtn &= "<span class='newc'>" & Chr(13)
                            rtn &= "<ul>" & gr
                        ElseIf gr <> ot Then

                            rtn &= "</ul>"

                            rtn &= "</span><span class='newc'>" & Chr(13)


                            gr = ot
                            rtn &= "<ul>" & gr
                        Else
                            cc += 1

                        End If
                        If fm.searcharray(auth, rs2.Item("Id")) = True And rs2.Item("menu_id").ToString.Length > 2 Then
                            rtn &= "<li style='color:gray; decorection:none;'>"

                            rtn &= " <a class=" + Chr(34) + "menu_a" + Chr(34) & " href='" & CStr(rs2.Item("menu_link")) & _
     "?sub=" & rs2.Item("menu_id") & "' style='' onmouseover=" & Chr(34) & "javascript:this.style.background='#f0f0f0';" & Chr(34) & _
                        " onmouseout=" & Chr(34) & "javascript:this.style.background='white';" & Chr(34) & " onclick=" & Chr(34) & "javascript:document.getElementById('frm_tar').src='" & rs2.Item("menu_link") & "';ha('sub" & (rs.Item("menu_id")) & "'); " & Chr(34) & " target='frm_tar'>" & rs2.Item("menu_name") & "</a>" & _
               "</li>" & Chr(13)
                        ElseIf rs2.Item("menu_id").ToString.Length > 2 Then
                            rtn &= "<li><span class='menu_a'>" & Chr(13) & rs2.Item("menu_name") & "</span></li>" & Chr(13)
                        End If

                    End While
                End If

                rtn &= "</ul></span></div>"
            End While
        End If
        Return rtn
    End Function
    Function groupmenu(ByVal con As SqlConnection, ByVal auth() As String)
        Dim dbx As New dbclass
        Dim fm As New formMaker
        Dim rs, rs2 As DataTableReader
        Dim rtn As String = ""
        rs = dbx.dtmake("submenu", "select * from menu where menu_id>0 and menu_id<=99 and publish='y'", con)
        If rs.HasRows Then
            Dim hmenu As String
            While rs.Read
                hmenu = rs.Item("menu_id")
                rs2 = dbx.dtmake("summ", "select distinct groupby from menu where menu_id like '" & hmenu & "%' and publish='y'", con)
                rtn &= "<div class='submenukir' id='sub" & rs.Item("menu_id") & _
                    "' style='visibility:hidden; left:0px; top:0px;background-image:url(images/menubg.jpg);;  position:fixed;" & _
"border-left:gray 1px solid;border-right:black 1px solid;width:25px auto;height:auto;'" & _
                                    " onmouseover=" & Chr(34) & "javascript:showobjar('m" & rs.Item("menu_id") & "','sub" & rs.Item("menu_id") & "',-15,18);" & Chr(34) & _
                                    " onmouseout=" & Chr(34) & "javascript:ha('sub" & rs.Item("menu_id") & "');" & Chr(34) & ">" & Chr(13)

                If rs2.HasRows Then
                    Dim ot As String = ""
                    Dim gr As String = ""
                    Dim cc1 As Integer = 4
                    Dim cc As Integer
                    While rs2.Read
                        If rs.IsDBNull(0) = True Then
                            ot = ""
                        Else
                            ot = rs2.Item("groupby").ToString
                        End If
                        rtn &= mkfram(ot, rs.Item("menu_id"), auth)

                    End While
                End If
                rtn &= "<div style='clear:both'></div>"
                rtn &= "</div>" & Chr(13) & Chr(13) & Chr(13)
            End While
        End If
        Return rtn
    End Function
    Function mkfram(ByVal grp As String, ByVal idr As String, ByVal auth() As String)
        Dim rtn As String = ""
        rtn = "<div class='gpclass'>"
        Dim ds As New dbclass
        Dim rs2 As DataTableReader
        Dim fm As New formMaker
        If grp <> "" Then
            rs2 = ds.dtmake("submenu", "select * from menu where menu_id like '" & idr & "%' and publish='y' and groupby='" & grp & "' order by menu_order ", Session("con"))
        Else
            rs2 = ds.dtmake("submenu", "select * from menu where menu_id like '" & idr & "%' and publish='y' and  groupby is null order by menu_order ", Session("con"))
        End If
        If rs2.HasRows Then
            rtn &= "<ul style='float:left'>" & grp
            While rs2.Read
                If fm.searcharray(auth, rs2.Item("Id")) = True And rs2.Item("menu_id").ToString.Length > 2 Then
                    rtn &= "<li style='color:gray; decorection:none;'>"

                    rtn &= " <a class=" + Chr(34) + "menu_s_a" + Chr(34) & " href='" & CStr(rs2.Item("menu_link")) & _
"?sub=" & rs2.Item("menu_id") & "' style='' onmouseover=" & Chr(34) & "javascript:this.style.background='#f0f0f0';" & Chr(34) & _
                " onmouseout=" & Chr(34) & "javascript:this.style.background='white';" & Chr(34) & " onclick=" & Chr(34) & "javascript:document.getElementById('frm_tar').src='" & rs2.Item("menu_link") & "';ha('sub" & idr & "'); " & Chr(34) & " target='frm_tar'>" & rs2.Item("menu_name") & "</a>" & _
       "</li>" & Chr(13)
                ElseIf rs2.Item("menu_id").ToString.Length > 2 Then
                    rtn &= "<li><span class='menu_a'>" & Chr(13) & rs2.Item("menu_name") & "</span></li>" & Chr(13)
                End If
            End While
            rtn &= "</ul>"
        End If
        rtn &= "</div>"
        Return rtn
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim str As String

        str = "<script language=" & Chr(34) & "javascript" & Chr(34) & " type=" & Chr(34) & "text/javascript" & Chr(34) & ">" & _
            "hw= window.screen.availHeight;ww=window.screen.availWidth; " & _
             "document.body.style.height=(hw*.9) + 'px';" & _
"document.body.style.width=(ww*.90) + 'px';" & _
            "document.getElementById(" & Chr(34) & "main_fram" & Chr(34) & ").style.height=hw*.95 + 'px';" & _
            " document.getElementById(" & Chr(34) & "main_fram" & Chr(34) & ").style.width=(ww*9) + 'px'; " & _
            "document.getElementById(" & Chr(34) & "main_fram" & Chr(34) & ").style.top=" & Chr(34) & "0px" & Chr(34) & ";" & _
            "document.getElementById(" & Chr(34) & "main_fram" & Chr(34) & ").style.left=" & Chr(34) & "0px" & Chr(34) & ";" & _
            "document.getElementById(" & Chr(34) & "main_fram" & Chr(34) & ").style.position=" & Chr(34) & "absolute" & Chr(34) & ";" & _
           "document.getElementById(" & Chr(34) & "main_fram" & Chr(34) & ").style.backgroundColor=" & Chr(34) & "#eeeeee" & Chr(34) & ";" & _
           "document.getElementById(" & Chr(34) & "topcont" & Chr(34) & ").style.width=(ww) + 'px';" & _
            "document.getElementById(" & Chr(34) & "topcont" & Chr(34) & ").style.height=(hw*0.14) + 'px';" & _
                       "document.getElementById(" & Chr(34) & "header_loc" & Chr(34) & ").style.width=(ww) + 'px';" & _
            "document.getElementById(" & Chr(34) & "header_loc" & Chr(34) & ").style.height='93%';" & _
        "document.getElementById(" & Chr(34) & "menu_sec" & Chr(34) & ").style.height='20%';" & _
            "document.getElementById(" & Chr(34) & "menu_sec" & Chr(34) & ").style.backgroundImage= " & Chr(34) & "url(images/blue_banner-760x147.jpg)" & Chr(34) & ";" & _
                       "document.getElementById(" & Chr(34) & "intrnal" & Chr(34) & ").style.height=(hw*0.72) + 'px';" & _
           "document.getElementById(" & Chr(34) & "intrnal" & Chr(34) & ").style.width=(ww*.9) + 'px';" & _
           "document.getElementById(" & Chr(34) & "frm_tar" & Chr(34) & ").style.height=(hw*0.785) + 'px';" & _
           "document.getElementById(" & Chr(34) & "frm_tar" & Chr(34) & ").style.width=(ww*.99) + 'px';" & _
           "document.getElementById(" & Chr(34) & "frm_tar" & Chr(34) & ").style.left=0 + 'px';" & _
          "document.getElementById(" & Chr(34) & "footer" & Chr(34) & ").style.height=(20) + 'px';" & _
            "document.getElementById(" & Chr(34) & "footer" & Chr(34) & ").style.width=ww + 'px';" & _
             "document.getElementById(" & Chr(34) & "footer" & Chr(34) & ").style.top=hw*.95 + 'px';" & _
             "document.getElementById(" & Chr(34) & "footer" & Chr(34) & ").style.left=0 + 'px';" & _
             "document.getElementById(" & Chr(34) & "footer" & Chr(34) & ").style.textAlign='center';" & _
        "document.getElementById(" & Chr(34) & "footer" & Chr(34) & ").style.position='absolute';" & _
         "</script>"
        Me.script.Text = str
        ' "document.getElementById(" & Chr(34) & "left_fram" & Chr(34) & ").style.height=(hw*0.83) + 'px';" & _

    End Sub
End Class

