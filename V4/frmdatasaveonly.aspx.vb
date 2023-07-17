Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm

Partial Class frmdatasaveonly
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sql As String
        sql = Request.QueryString("sql")
        Dim dbs As New dbclass
        sql = "Begin Transaction" & Chr(13) & sql
        Dim flg As Object = dbs.excutes(sql, Session("con"), Session("path"))
        If IsNumeric(flg) Then
            If CInt(flg) > 0 Then
                dbs.excutes("Commit", Session("con"), session("path"))
                outp.Text = "Data is Saved"
                If Request.QueryString("tar") <> "" Then
                    outp.Text &= "<script> $('#frmx').attr('target','frm_tar');"
                    outp.Text &= " $('#frmx').attr('action','" & Request.QueryString("tar") & "');"
                    outp.Text &= " $('#frmx').submit();</script>"
                End If

            Else
                dbs.excutes("RollBack", Session("con"), Session("path"))
                outp.Text = "Sorry Data is not saved!" & sql & flg.ToString
            End If
        Else
            dbs.excutes("RollBack", Session("con"), Session("path"))
            outp.Text = "Sorry Data is not saved!" & sql & flg.ToString
        End If
    End Sub
End Class
