Imports System.Data
Imports System.Data.Sql
Imports Kirsoft.hrm
Partial Class perdiemsheet
    Inherits System.Web.UI.Page
    Function gosaved()
        Dim sql As String = ""
        Dim ref, rtn As String
        Dim ntick As String
        Dim reflist() As String = {""}
        Dim dbs As New dbclass
        Dim sec As New k_security
        Dim mail As New mail_system
        Dim fm As New formMaker
        Dim keychk As String = ""
        Dim pdate As Date = Request.Form("paiddate2")
        Dim pmthd As String = Request.Form("mthd")
        Dim countback As Integer = 0
        rtn = ""
        For Each j As String In Request.QueryString
            If IsNumeric(j) Then
                Dim spl() As String
                spl = Request.QueryString(j).Split(",")
                If spl.Length > 0 Then
backhere:
                    If countback > 10 Then
                        mail.sendemail("illtration on perdiemsheet.asp.vb line 24", "493 15kir", "kirsoftet@gmail.com", "z.kirubel@gmail.com", "Error")
                        rtn &= " Illtration Problem!"

                        GoTo finishhere
                    End If
                    ' ntick = Now.Day.ToString & "/" & Now.Month.ToString & "/" & (Now.Year.ToString).Substring(2, 2) & "/" & Now.Hour.ToString & "/" & Now.Minute.ToString & "/" & Now.Second.ToString & "/" & Now.Millisecond.ToString

                    ' rtn &= (j.ToString & " ==> " & ntick & "<br>")
                    ' ntick = ntick.Replace("/", "")
                    ref = StrToHex3(Now.Ticks.ToString) & j.ToString
                    'ref = GenerateString(5) & j.ToString
                    rtn &= "<br> <u>" & ref & "</u></br>"
                    keychk = fm.getinfo2("select id from pardimpay where ref='" & ref & "'", Session("con"))

                    If keychk <> "None" Then
                        countback += 1

                        GoTo backhere
                    End If
                    If fm.searcharray(reflist, ref) = True Then
                        countback += 1
                        Response.Write(ref & "************************<br>")
                        GoTo backhere
                    End If

                    ' ref = sec.StrToHex3(j.ToString & ntick)
                    rtn &= " vs =>" & ref & "<br>"

                    For k As Integer = 0 To UBound(spl)
                        If spl(k) <> "" Then
                            sql &= "update pardimpay set ref='" & ref & "', paid_date='" & pdate & "',mthd='" & pmthd & "',paid_state='y' where id=" & spl(k) & Chr(13)
                            ReDim Preserve reflist(reflist.Length + 1)
                            reflist(reflist.Length - 1) = ref
                            '   Response.Write("<br>update pardimpay set ref='" & ref & "', paid_date='" & pdate & "',mthd='" & pmthd & "',paid_state='y' where id=" & spl(k))
                        End If
                    Next
                End If
            End If
            '  Response.Write("<br>" & j & "=" & Request.QueryString(j))
        Next
        If sql <> "" Then
            Dim rslt As String = dbs.excutes(sql, Session("con"), Session("path"))
            If IsNumeric(rslt) Then
                If CInt(rslt) > 0 Then
                    rtn &= "data is saved"


                Else
                    rtn &= "data isnot saved =>" & rslt
                End If

            Else
                rtn &= "<br>data isnot saved =>" & rslt
            End If
        End If
finishhere:
        Return rtn
    End Function
    Public Function StrToHex3(ByVal Data As String) As String
        Dim mail As New mail_system
        Dim sec As New k_security
        Dim sHex As String = ""
        Try
            'Data = sec.Kir_StrToHex(Data)
            Response.Write(Data & "<br>")
            sHex = Conversion.Hex(Data)
        Catch ex As Exception
            mail.sendemail(ex.ToString & "<br>" & Data, "493 15kir", "no-reply@gmail.com", "z.kirubel@gmail.com", "Error msg")
        End Try



        Return sHex
    End Function
    Public Function GenerateString(ByVal ss As Integer)

        Dim xCharArray() As Char = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray
        '  Dim xNoArray() As Char = "".ToCharArray
        Dim xGenerator As System.Random = New System.Random()
        Dim xStr As String = String.Empty

        While xStr.Length < ss


            xStr &= xCharArray(xGenerator.Next(2, xCharArray.Length))
           

        End While

        Return xstr


    End Function
End Class
