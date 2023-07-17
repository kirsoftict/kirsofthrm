Imports Kirsoft.hrm
Imports System.IO

Partial Class print
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim filename As String = Request.Form("filename")
        Dim filetype As String = Request.Form("filetype")
        Dim floc As String = Request.Form("fileloc")
        Dim right As String = Request.Form("rigt")
        Dim css As String = Request.Form("cssexport")

        If right = "" Then
            right = "1;2;3;4;5"
        End If
           Dim fm As New formMaker
        Dim fx() As String
        Dim wrt() As String
        Dim show As String = ""
        'fx = Session("right").split(";")
        ' Response.Write(floc)
        'Response.Write(Request.Form("loc"))
        Dim obj() As String
        If IsError(Session("con")) = True Then
            Response.Redirect("logout.aspx")
        End If
        ' Response.Write(Session("right"))
        If Request.Form("right") <> "" Then
            fx = Request.Form("right").Split(";")
        Else
            fx = Request.Form("rigt").Split(";")
        End If
        'Response.Write(Request.Form("nnp"))
        If Request.Form("nnp") <> "" Then
            floc = Request.Form("loc")
            obj = File.ReadAllLines(floc)
            If obj.Length > 0 Then
                wrt = obj(0).Split(";")
                For i As Integer = 0 To wrt.Length - 1
                    If fm.searcharray(fx, wrt(i)) = True Then
                        show = "ok"
                        Exit For
                    End If
                Next
                If show = "ok" Then
                    show = ""
                    For i As Integer = 1 To obj.Length - 1
                        show &= obj(i)
                    Next
                    filename = Request.QueryString("pagename")
                    exelp_2(show, filename & "." & "xls", "application/excel")
                End If
            End If
        Else


            '  Response.Write(floc)
            obj = File.ReadAllLines(floc)
            '  Response.Write(obj.Length)
            If obj.Length > 0 Then
                wrt = obj(0).Split(";")
                ' Response.Write(wrt.Length)
                If Not IsNumeric(wrt(0)) Then
                    wrt = "1;2;3;4;5".Split(";")
                End If
                For i As Integer = 0 To wrt.Length - 1
                    If fm.searcharray(fx, wrt(i)) = True Then
                        show = "ok"

                    End If
                Next
            End If
            If show = "ok" Then
                'Response.Write(show)
                show = ""

                'Response.Write(filetype)
                Select Case filetype
                    Case "xls"
                        ' Response.Write("<textarea rows='40' cols='120'>")
                        ' Response.Write("xls")
                        For i As Integer = 0 To obj.Length - 1

                            show &= obj(i)
                        Next

                        If Request.QueryString("export") = "on" Then
                            Response.Write("export is on")
                            exelp_3(show, filename & "." & filetype, "application/excel")
                        ElseIf Request.QueryString("exporttax") = "on" Then
                            exelp_4(show, filename & "." & filetype, "application/excel")
                        Else

                            exelp_2(show, filename & "." & filetype, "application/excel")
                        End If

                    Case "doc"
                        For i As Integer = 0 To obj.Length - 1
                            show &= obj(i)
                        Next
                        '  Response.Write("<textarea cols=600 rows=20>" & show & "</textarea><br>")
                        ' Response.Write(css)
                        exelp_word(show, filename & "." & filetype, css)
                    Case "docx"
                        For i As Integer = 1 To obj.Length - 1
                            show &= obj(i)
                        Next
                        exelp_2(show, filename & "." & filetype, "application/ms-word")
                    Case Else
                        For i As Integer = 1 To obj.Length - 1
                            show &= obj(i)
                        Next
                        exelp_2(show, filename & ".txt", "application/notepad")
                End Select
            End If
        End If



    End Sub
    Function exelp_3(ByVal outpx As String, ByVal filename As String, ByVal conttype As String)
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & filename)
        Response.ContentType = conttype
        Dim outp As String
        Dim sec As New k_security
        outp = "<style type='text/css'>" & _
           " #tb1 " & _
           " {" & _
                "border:1px solid black;" & _
                "font-size:12pt;" & _
         "   }" & _
        "  #tb1 td" & _
           "{" & _
          "border-top: 1px solid black;" & _
            "border-left:1px solid black;" & _
"        font-size:10pt;" & _
           " }" & _
       " .cell()" & _
            "{" & _
              " border-top: 1px solid black;" & _
             "border-left:1px solid black;" & _
"             border-right: 1px solid black;" & _
             "border-bottom:1px solid black;" & _
           " }" & _
            "</style>"
        outp &= " <link href='css/pension.css' media='screen' rel='Stylesheet' type='text/css' />"
        outp &= outpx
        Response.Write(outp)
        ' Response.Write(Request.Form("nnp"))

    End Function
    Function exelp_4(ByVal outpx As String, ByVal filename As String, ByVal conttype As String)
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & filename)
        Response.ContentType = conttype
        Dim outp As String
        Dim sec As New k_security
        outp = "<html><head><style type='text/css'>" & File.ReadAllText(MapPath("css") & "/tax.css") & "</style></head><body>"
        outp &= outpx & "</body></html>"
        Response.Write(outp)
        ' Response.Write(Request.Form("nnp"))

    End Function
    Function exelp_5(ByVal outpx As String, ByVal filename As String, ByVal conttype As String, ByVal css As String)
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & filename)
        Response.ContentType = conttype
        Dim outp As String
        Dim sec As New k_security
        outp = "<html><head><style type='text/css'>" & File.ReadAllText(MapPath("css") & "/" & css) & "</style></head><body>"
        outp &= outpx & "</body></html>"
        Response.Write(outp)
        ' Response.Write(Request.Form("nnp"))

    End Function
    Function exelp_2(ByVal outpx As String, ByVal filename As String, ByVal conttype As String)
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & filename)
        Response.ContentType = conttype
        Dim outp As String
        Dim sec As New k_security
        outp = "<style type='text/css'>" & _
           " #tb1 " & _
           " {" & _
                "border:1px solid black;" & _
                "font-size:12pt;" & _
         "   }" & _
        "  #tb1 td" & _
           "{" & _
          "border-top: 1px solid black;" & _
            "border-left:1px solid black;" & _
"        font-size:10pt;" & _
           " }" & _
       " .cell" & _
            "{" & _
              " border-top: 1px solid black;" & _
             "border-left:1px solid black;" & _
"             border-right: 1px solid black;" & _
             "border-bottom:1px solid black;" & _
           " }" & _
            "</style>"

        outp &= outpx
        Response.Write(outp)
        ' Response.Write(Request.Form("nnp"))

    End Function
    Function exelp_1()
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & Request.QueryString("pagename") & ".xls")
        Response.ContentType = "application/ms-excel"
        Dim outp As String
        Dim sec As New k_security
        outp = "<style type='text/css'>" & _
           " #tb1 " & _
           " {" & _
                "border:1px solid black;" & _
                "font-size:12pt;" & _
         "   }" & _
        "  #tb1 td" & _
           "{" & _
          "border-top: 1px solid black;" & _
            "border-left:1px solid black;" & _
"        font-size:10pt;" & _
           " }" & _
       " .cell()" & _
            "{" & _
              " border-top: 1px solid black;" & _
             "border-left:1px solid black;" & _
"             border-right: 1px solid black;" & _
             "border-bottom:1px solid black;" & _
           " }" & _
            "</style>"
        outp &= sec.HexToStr(Request.Form("nnp"))
        Response.Write(outp)
        ' Response.Write(Request.Form("nnp"))

    End Function
    Function exelp_word(ByVal outpx As String, ByVal filename As String, ByVal cssx As String)
        '  Response.AppendHeader("Content-Disposition", "attachment; filename=" & filename)
        '  Response.ContentType = conttype
        Dim strBody As New System.Text.StringBuilder("")

        strBody.Append("<html " & _
                "xmlns:o='urn:schemas-microsoft-com:office:office' " & _
                "xmlns:w='urn:schemas-microsoft-com:office:word'" & _
                "xmlns='http://www.w3.org/TR/REC-html40'>" & _
                "<head><title>Time</title>")

        'The setting specifies document's view after it is downloaded as Print
        'instead of the default Web Layout
        strBody.Append("<!--[if gte mso 9]>" & _
                                 "<xml>" & _
                                 "<w:WordDocument>" & _
                                 "<w:View>Print</w:View>" & _
                                 "<w:Zoom>90</w:Zoom>" & _
                                 "<w:DoNotOptimizeForBrowser/>" & _
                                 "</w:WordDocument>" & _
                                 "</xml>" & _
                                 "<![endif]-->")

        strBody.Append("<style>" & _
                                 File.ReadAllText(MapPath("css") & "/" & cssx) & _
                               "</style></head>")

        strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" & _
                                outpx & " </body></html>")

        'Force this content to be downloaded 
        'as a Word document with the name of your choice
        Response.AppendHeader("Content-Type", "application/msword")
        Response.AppendHeader("Content-disposition", _
                                "attachment; filename=" & filename & ".doc")
        Response.Write(strBody)


        ' Response.Write(Request.Form("nnp"))

    End Function

End Class
