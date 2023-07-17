Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Imports System.Web
Imports System.IO
Partial Class iframe
    Inherits System.Web.UI.Page
    Public Function form(ByVal con As SqlConnection, ByVal tbl As String) As String
        Dim db As New dbclass
        Dim jv As String = ""
        Dim vart As String = "<script type=" & Chr(34) & "text/javascript" & Chr(34) & ">" & Chr(13) & "var prv;" & Chr(13) & "  prv=" & Chr(34) & Chr(34) & ";" & Chr(13) & _
        "var id;" & Chr(13) & _
"var focused=" & Chr(34) & Chr(34) & ";" & Chr(13) & "var requf="
        Dim requf As String = "[" & Chr(34)
        Dim formx As String = ""
        Dim rs As DataTableReader
        rs = db.dtmake(tbl, "", con)
        Dim icount, i As Integer
        icount = rs.FieldCount
        jv = "function validation1(){"
        formx = "<form method='post' id='frm" & tbl & "' name='frm" & tbl & "'> " & Chr(13)
        formx = formx & "<table>"
        Dim tr As String = "off"

        For i = 1 To icount - 1
            If i Mod 2 = 1 Then
                If tr = "on" Then
                    formx = formx & "</tr><tr>"
                Else
                    formx = formx & "<tr>"
                    tr = "on"
                End If
            End If

            formx = formx & "<td>" & rs.GetName(i) & "<sup style='color:red;'>*</sup></td><td>:</td><td>" & _
"<input type='text' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & _
            "'" & _
            " value='' /><br />"
            jv &= "if ($('#" & rs.GetName(i) & "').val() == '') {" & _
            "showMessage('" & rs.GetName(i) & " cannot be empty','" & rs.GetName(i) & "');" & _
            "$('#" & rs.GetName(i) & "').focus();" & _
            "return false;}" & Chr(13)
            requf &= rs.GetName(i) & Chr(34) & "," & Chr(34)
            If rs.GetName(i).StartsWith("pemail") = True Or rs.GetName(i).StartsWith("wemail") Or rs.GetName(i).StartsWith("email") Then
                formx = formx & "<label class='lblsmall'>username@domain.com</label>"
            Else
                formx = formx & "<label class='lblsmall'></label>"
            End If

            formx = formx & "</td>" & Chr(13)

            If rs.GetDataTypeName(i) = "DateTime" And rs.GetName(i) <> "date_reg" Then
                formx = formx & "<script language='javascript' type='text/javascript'> " & _
                "$(function() {" & _
"$( " & Chr(34) & "#" & rs.GetName(i) & Chr(34) & ").datepicker({" & _
"changeMonth: true," & _
"changeYear: true" & _
"	});" & _
" $( " & Chr(34) & "#" & rs.GetName(i) & Chr(34) & " ).datepicker( " & Chr(34) & "option" & Chr(34) & _
                "," & Chr(34) & "dateFormat" & Chr(34) & "," & Chr(34) & "mm/dd/yy" & Chr(34) & ");" & _
"});</script>"
            End If

        Next
        ' jv &= "}</script>"
        If tr = "on" Then
            formx = formx & "</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />" & _
      "<input type='reset' onclick=" & Chr(34) & "javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" & Chr(34) & " /></td></tr>"
        End If
        requf = requf & "x" & Chr(34) & "];"
        vart &= requf & Chr(13) & "var fieldlist=" & requf
        formx = vart & Chr(13) & jv & "else if(focused==" & Chr(34) & Chr(34) & ") { var ans;" & Chr(13) & _
"ans=checkblur();" & Chr(13) & _
"if(ans!=true){ " & Chr(13) & _
    " $(" & Chr(34) & "#" & Chr(34) & " + ans).focus();" & Chr(13) & _
"}else{" & Chr(13) & _
"   var str=$(" & Chr(34) & "#frm" & tbl & Chr(34) & ").formSerialize();" & Chr(13) & _
"   $(" & Chr(34) & "#frm" & tbl & Chr(34) & ").attr(" & Chr(34) & "action" & Chr(34) & "," & Chr(34) & "?tbl=" & tbl & "&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" & Chr(34) & " + str);" & Chr(13) & _
"    $(" & Chr(34) & "#frm" & tbl & Chr(34) & ").submit();" & Chr(13) & _
"  return true;}" & Chr(13) & _
"  }" & Chr(13) & _
"} </script>" & formx & "</table></form>"
        rs.Close()
        Return formx
    End Function
End Class
