Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class sqlselectview
    Inherits System.Web.UI.Page
    Function getout(ByVal rs As DataTableReader)
        If rs.HasRows Then
            Dim c As Integer = rs.FieldCount
            Dim rtn As String = "<table border=1 style='font-size:12px;'><tr>"
            For k As Integer = 0 To c - 1
                rtn &= "<td>" & rs.GetName(k) & "</td>"
            Next
            rtn &= "</tr>"
            While rs.Read
                rtn &= "<tr>"
                For k As Integer = 0 To c - 1
                    If rs.IsDBNull(k) = True Then
                        rtn &= "<td></td>"
                    Else
                        rtn &= "<td>" & rs.Item(k) & "</td>"
                    End If

                Next
                rtn &= "</tr>"
            End While
            rtn &= "</table>"
            Response.Write(rtn)
        End If
    End Function
End Class
