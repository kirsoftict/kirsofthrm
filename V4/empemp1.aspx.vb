Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient

Partial Class empemp1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub
    Function outp()
        Dim fm As New formMaker
        Dim hr As String
        hr = fm.getinfo2("select hire_date from emprec where id=" & Session("emptid") & " and type_recuritment='Contract' and end_date is null order by hire_date desc", Session("con"))
        If hr <> "1/1/1900" Then
            If hr <> "None" Then
                If fm.getinfo2("select id from emp_contract where emptid =" & Session("emptid") & " and '" & CDate(Today.ToShortDateString).AddDays(5) & "' < dateend", Session("con")) = "None" Then
                    Response.Write("<script> $('#frmx').attr('target','fpay');")
                    Response.Write("$('#fpay').attr('frameborder','0');")
                    Response.Write(" $('#frmx').attr('action','contract_entry.aspx');")
                    Response.Write("$('#frmx').submit();")
                    Response.Write("$('#pay').css({top:'0px',left:'0px'});")
                    Response.Write("$('#pay').remove('display');")
                    Response.Write("$('#pay').dialog({")
                    Response.Write("title:'Add Contract', height:300,width:600,modal:true});")
                    Response.Write("</script>")
                End If
            End If
        End If
    End Function

End Class
