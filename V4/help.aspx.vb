Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class help
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("con").close()
        Session("con").open()
        Dim ds As New dbclass
        Dim rs As DataTableReader
        Dim sec As New k_security
        If Request.QueryString("url") <> "" Then
            rs = ds.dtmake("dshelp", "select  * from tblhelp where helptext like '%" & Request.QueryString("url") & "%'", Session("con"))
            If rs.HasRows Then
                While rs.Read
                    outp.Text &= sec.HexToStr(rs.Item("help"))

                End While
            End If
        Else
            rs = ds.dtmake("dshelp", "select * from tblhelp where helptext", Session("con"))

        End If


    End Sub
End Class
