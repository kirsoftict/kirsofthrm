Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class scripts_leavesettled
    Inherits System.Web.UI.Page

  
    Function topscript()
        For Each k As String In Request.ServerVariables
            'Response.Write(k & "==>" & Request.ServerVariables(k) & "<br>")
        Next
        Response.Write("<script type='text/javascript'>")
        If Request.QueryString("emptid") <> "" Then
            Response.Write("$(document).ready(function() {")
            Response.Write("$('#emptid').text('" & Request.QueryString("emptid") & "');" & Chr(13))
            Response.Write("$('#bgtid').text('" & Request.QueryString("bgtid") & "');" & Chr(13))
            Response.Write("$('#emptidd').val('" & Request.QueryString("emptid") & "');" & Chr(13))
            Response.Write("$('#bgtidd').val('" & Request.QueryString("bgtid") & "');" & Chr(13))
            Response.Write("$('#bno').val ('" & Request.QueryString("bno") & "');" & Chr(13))
            Response.Write("$('#amt').val ('" & Request.QueryString("amt") & "');" & Chr(13))
            Response.Write("$('#ref').val ('" & Request.QueryString("ref") & "');" & Chr(13))
            Response.Write("$('#remark').val ('" & Request.QueryString("remark") & "');" & Chr(13))
            Response.Write("});")
        End If

       
        Response.Write("</script>")
        ' Response.Write(Request.QueryString("type"))
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("action") = "save" Then
            Dim sql As String
            Dim db As New dbclass
            Response.Write(Request.QueryString("btnSave"))
            Dim remark As String = Request.QueryString("remark")
            Dim amt As String
            amt = Request.QueryString("amt").Replace(",", "")
            sql = "insert into leav_settled(emptid,bgtid,bal,paidamt,ref,remark,who_reg,date_reg) " & _
                "values(" & Request.QueryString("emptidd") & ",'" & Request.QueryString("bgtidd") & "','" & _
                Request.QueryString("bno") & "','" & amt & "','" & _
                Request.QueryString("ref") & "','" & remark & "','" & Request.QueryString("who_reg") & "','" & _
                Request.QueryString("date_reg") & "')"
            Dim flg As String = db.excutes(sql, Session("con"), session("path"))
            If IsNumeric(flg) Then
                If CInt(flg) > 0 Then
                    sql = "insert into emp_leave_take(emptid,bgtid,bal,paidamt,ref,remark,who_reg,date_reg) " & _
                "values(" & Request.QueryString("emptidd") & ",'" & Request.QueryString("bgtidd") & "','" & _
                Request.QueryString("bno") & "','" & amt & "','" & _
                Request.QueryString("ref") & "','" & remark & "','" & Request.QueryString("who_reg") & "','" & _
                Request.QueryString("date_reg") & "')"
                    ' Response.Write("<script>$('#pay').css({display:none});</script>")
                End If
                ' Response.Write("<br>" & sql)
            Else
                Response.Write(flg)
            End If
        ElseIf Request.QueryString("action") = "update" Then
            Dim sql As String
            Dim db As New dbclass
            Dim remark As String = Request.QueryString("remark")
            Dim amt As String
            amt = Request.QueryString("amt").Replace(",", "")
            sql = "update leav_settled set "
            sql &= "bal='" & Request.QueryString("bno") & "',"
            sql &= "paidamt='" & amt & "',"
            sql &= "ref='" & Request.QueryString("ref") & "',"
            sql &= "remark='" & remark & "',"
            sql &= "who_reg='" & Request.QueryString("who_reg") & "',"
            sql &= "date_reg='" & Request.QueryString("date_reg") & "' "
            sql &= " Where bgtid='" & Request.QueryString("bgtidd") & "'"

            Dim flg As String = db.excutes(sql, Session("con"), session("path"))
            If IsNumeric(flg) Then
                If CInt(flg) > 0 Then
                    ' Response.Write("<script type='text/javascript'>$('#pay').css({'display':none});</script>")
                End If

            Else
                Response.Write(flg)
            End If
        End If
    End Sub
End Class
