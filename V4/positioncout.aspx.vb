Imports Kirsoft.hrm
Imports System.Data
Imports System.Data.SqlClient
Partial Class positioncout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sql As String = ""
        sql = "SELECT position, COUNT(position) AS Nox FROM emp_job_assign WHERE emptid in (select id from emprec where active='y') and date_end Is NULL GROUP BY position ORDER BY position"
        Dim dbs As New dbclass
        Dim rs As DataTableReader
        Dim temp As Integer = 0
        Dim sec As New k_security
        Dim sno As Integer = 1
        Try
            rs = dbs.dtmake("rportxx", sql, Session("con"))
            If rs.HasRows = True Then
                Me.outp.Text = "<table class='leavec'><tr><td colspan='3'>" & Session("company_name") & "</td></tr>"
                Me.outp.Text &= "<tr><td>SNo.</td><td>Position</td><td>No. Employees</td></tr>"
                While rs.Read
                    temp += rs.Item("Nox")
                    Me.outp.Text &= "<tr><td>" & sno.ToString & "</td>"
                    Dim position As String = ""
                    position = "<a href='viewreport1.aspx?val=bypost&position=" & sec.dbStrToHex(rs.Item("position")) & "&active=y' target='frm_tar' style=' color:#0025ff;text-decoration:none'>" & rs.Item("position") & "</a>"

                    Me.outp.Text &= "<td>" & position & "</td><td>" & rs.Item("Nox").ToString & "</td></tr>"
                    sno += 1
                End While
                Me.outp.Text &= "<tr><td></td><td><b>Total No. Employees</b></td><td><b>" & temp.ToString & "</b></td></tr></table>"
            End If
            rs.Close()
        Catch ex As Exception
            Response.Write(ex.ToString & " <br>" & sql)
        End Try
        dbs = Nothing
        sec = Nothing
    End Sub
End Class
