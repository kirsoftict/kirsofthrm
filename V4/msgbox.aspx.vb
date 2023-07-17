
Partial Class scripts_msgbox
    Inherits System.Web.UI.Page
    Function msg(ByVal msgtitle As String, ByVal msgx As String, ByVal msgtype As String)
        Dim outp As String
        outp = "<table style='width:100%;'>"
        outp &= "<tr>"
        outp &= " <td class='style2' id='symbole'>&nbsp;"
        outp &= "<img src='" & msgty(msgtype) & "' width='60' />"
        outp &= " </td>"
        outp &= " <td class='style3' id='title'>"
        outp &= msgtitle
        outp &= " </td>"
        outp &= " </tr>"
        outp &= "<tr>"
        outp &= " <td class='style1' id='msg'>" & msgx
        outp &= "    &nbsp;</td>"
        outp &= "  <td>"
        outp &= "    &nbsp;</td>"
        outp &= "</tr>"
        outp &= "<tr>"
        outp &= " <td class='style1' colspan='2'>"
        outp &= " &nbsp;</td>"
        outp &= "</tr>"
        outp &= "</table>"
        Response.Write(outp)
    End Function
    Function msgty(ByVal msgtype As String)
        Select Case msgtype
            Case "Caution"
                Return "images/png/24-message-warn.png"
            Case "Information"
                Return "images/png/24-message-info.png"
            Case "Error"
                Return "images/png/error.png"
        End Select
    End Function
End Class
