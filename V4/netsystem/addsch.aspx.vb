
Partial Class netsystem_addsch
    Inherits System.Web.UI.Page
    Function getkey(ByVal num As Integer)
        Dim keyx As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 0 To num
            Dim idx As String = r.Next(0, keyx.Length)
            sb.Append(keyx.Substring(idx, 1))

        Next
        Return sb.ToString
    End Function
End Class
