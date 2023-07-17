Imports Kirsoft.hrm
Partial Class jqq_download
    Inherits System.Web.UI.Page
    Function downloadfile(ByVal ref As String)
        Dim sec As New k_security
        ref = sec.dbHexToStr(ref)
        If ref <> "" Then
            ' Dim path As String = Server.MapPath(ref)
            Dim file As System.IO.FileInfo = New System.IO.FileInfo(ref)
            If file.Exists Then
                Response.Clear()
                Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
                Response.AddHeader("Content-Length", file.Length.ToString())
                Response.ContentType = "application/octet-stream"
                Response.WriteFile(file.FullName)
                Response.Write("download Complited")
            Else
                Response.Write("This file does not exist.")
            End If
        Else
            Response.Write("Click link to download.")
        End If
    End Function
End Class
