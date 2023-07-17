Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.IO
Imports Kirsoft.hrm
Partial Class iframe
    Inherits System.Web.UI.Page
    Public Function form(ByVal con As SqlConnection, ByVal tbl As String) As String
        Dim db As New dbclass
        Dim fm As New formmaker
        Dim sql As String
        Dim jv As String = ""
        If IsError(con) Then
            Response.Write("error on conx")
        End If
       
        Dim vart As String = "<script type=" & Chr(34) & "text/javascript" & Chr(34) & ">" & Chr(13) & "var prv;" & Chr(13) & "  prv=" & Chr(34) & Chr(34) & ";" & Chr(13) & _
        "var id;" & Chr(13) & _
"var focused=" & Chr(34) & Chr(34) & ";" & Chr(13) & "var requf="
        Dim requf As String = "[" & Chr(34)
        Dim formx As String = ""
        Dim rs As DataTableReader
        Dim ors As Object
        Try
            sql = "select * from " & tbl
            Response.Write(con.ConnectionString.ToString)
            rs = db.dtmake(tbl, sql, con)


            Dim icount, i As Integer
            icount = rs.FieldCount
            jv = "function validation1(){"
            formx = "<div class='form-style-2'>" & _
    "<div class='form-style-2-heading'>Provide your information</div>" & _
    "<form method='post' id='frm" & tbl & "' name='frm" & tbl & "'> " & Chr(13)
            ' formx = formx & "<table>"
            Dim tr As String = "off"

            For i = 1 To icount - 1
                If i Mod 2 = 1 Then
                    If tr = "on" Then
                        '  formx = formx & "</tr><tr>"
                    Else
                        '   formx = formx & "<tr>"
                        tr = "on"
                    End If
                End If
                formx &= "   <div class='form-group-inner'>"


                jv &= "if ($('#" & rs.GetName(i) & "').val() == '') {" & _
                "showMessage('" & rs.GetName(i) & " cannot be empty','" & rs.GetName(i) & "');" & _
                "$('#" & rs.GetName(i) & "').focus();" & _
                "return false;}" & Chr(13)
                requf &= rs.GetName(i) & Chr(34) & "," & Chr(34)
                If rs.GetName(i).StartsWith("pemail") = True Or rs.GetName(i).StartsWith("wemail") Or rs.GetName(i).StartsWith("email") Or rs.GetName(i).Contains("email") = True Then
                    formx &= "<div class='row'>" & _
                                                           " <div class='col-lg-3 col-md-3 col-sm-3 col-xs-12'>" & Chr(13) & _
                                                               " <label class='pull-right pull-right-pro'>" & rs.GetName(i) & "</label>" & _
                                                          "  </div>" & Chr(13) & _
                                                           " <div class='col-lg-9 col-md-9 col-sm-9 col-xs-12'>" & Chr(13) & _
                                                              "  <input type='text' class='form-control'  name='" & rs.GetName(i) & "' id='" & rs.GetName(i) & _
                        "' value='' Placeholder='username@domain.com'  onblur=" & Chr(34) & _
                     "javascript:chkmail('" & rs.GetName(i) & "');" & Chr(34) & "/>" & Chr(13) & _
                                                            "</div>" & Chr(13) & _
                                                       " </div>" & Chr(13)
                  

                    '  formx = formx & "<label class='lblsmall'>username@domain.com</label>"
                ElseIf LCase(rs.GetDataTypeName(i)) = "string" Then
                    formx = formx & "<div class='col-sm-6 col-md-6'>     " & Chr(13) & _
                                            "<div class='form-group data-custon-pick data-custom-mg'>" & Chr(13) & _
                                       " <label>" & rs.GetName(i) & "</label>" & Chr(13) & _
                                       " <div class='input-daterange input-group' id=''>" & Chr(13) & _
                                          " <input class='form-control' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & "' value=''/>" & Chr(13) & _
                                       " </div>" & Chr(13) & _
                                    "</div> " & Chr(13) & _
                            "</div>"


                ElseIf LCase(rs.GetDataTypeName(i)) = "datetime" Or LCase(rs.GetDataTypeName(i)) = "date" Then
                    If LCase(rs.GetDataTypeName(i)) = "datetime" Then
                        formx = formx & " <div class='col-sm-6 col-md-6'>" & Chr(13) & _
            "<label class='label-2'>" & rs.GetName(i) & "</label>" & Chr(13) & _
              "   <div class='input-group date' id='datetimepicker1'>" & Chr(13) & _
                  "  <input type='text' class='form-control' id='" & rs.GetName(i) & "' name='" & rs.GetName(i) & "'>" & Chr(13) & _
                 "   <span class='input-group-addon'>" & Chr(13) & _
                  "      <span class='glyphicon glyphicon-calendar'></span>" & Chr(13) & _
                  "  </span>" & Chr(13) & _
              "  </div>" & Chr(13) & _
          "  </div>"

                    Else
                        formx = formx & "<div class='form-group data-custon-pick data-custom-mg' id='data_5'>" & Chr(13) & _
                                      " <label>" & rs.GetName(i) & "</label>" & Chr(13) & _
                                      " <div class='input-daterange input-group' id='datepicker'>" & Chr(13) & _
                                          " <input type='text' class='form-control'  name='" & rs.GetName(i) & "' id='" & _
                                       rs.GetName(i) & "' value='" & Now.ToShortDateString & "'/>" & Chr(13) & _
                                          "</div>" & Chr(13) & _
                                   "</div>" & Chr(13)
                    End If
                   
                Else

                    formx = formx & "<label for='" & rs.GetName(i) & "'><span>" & rs.GetName(i) & rs.GetDataTypeName(i) & " <span class='required'>*</span></span>" & _
                     "<input type='text' class='input-field' name='" & rs.GetName(i) & "' id='" & rs.GetName(i) & _
                     "' value='' /></label>"

                    'formx = formx & "<label class='lblsmall'></label>"
                End If

                formx = formx & Chr(13)

                If rs.GetDataTypeName(i) = "DateTime" Then
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
            formx &= "</div>"
            formx = formx & "<label><span>&nbsp;</span><input type='button' name='btnSave' id='btnSave' value='Save' />" & _
     "</label></form></div>"

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
    "} </script>" & formx
            rs.Close()
            Return formx
        Catch ex As Exception
            Response.Write(ex.ToString & "<br>" & tbl & "<br>>>" & sql)
        End Try
    End Function
End Class
