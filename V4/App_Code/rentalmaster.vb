
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Web.UI.Page
Imports System.Security
Imports System.IO
Imports System.Web.HttpServerUtility
Imports System.Web.HttpException
Imports System.Web.HttpRuntime
Imports System.Net.Mail
Imports System.Collections.Generic
Imports System.Text
Imports System
Imports System.DateTime
Imports System.Web.SessionState.HttpSessionState
Imports System.Text.RegularExpressions
Imports System.Configuration
Imports System.Timers
Imports System.Web
Imports System.Collections
Imports Kirsoft.hrm


Namespace Kirsoft.rental
    Public Class mail_system

        Dim fm As New formMaker
        Public Function mailprep(ByVal txtTo As String, ByVal txtFrom As String, ByVal cc As String, ByVal txtbody As String, ByVal txtsubject As String) As Object
            Dim e_mail As New MailMessage
            Dim eadd() As String
            eadd = txtTo.Split(";")
            Try

                e_mail = New MailMessage()
                e_mail.From = New MailAddress(txtFrom)
                For i As Integer = 0 To eadd.Length - 1
                    ' Response.Write("<br>....................<br>" & eadd(i) & "<br>....................<br>")
                    If eadd(i).Length > 4 Then
                        e_mail.To.Add(New MailAddress(eadd(i)))
                    End If
                Next
                If cc <> "" Then
                    e_mail.CC.Add(New MailAddress(cc))
                End If
                e_mail.Subject = txtsubject
                e_mail.IsBodyHtml = True
                e_mail.BodyEncoding = System.Text.Encoding.UTF8
                e_mail.Body = "<html><body>" & txtbody & "</body></html>"
                '  Smtp_Server.Send(e_mail)
                ' MsgBox("Mail Sent")



                'Response.Write("<br><html><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><body>" & txtbody & "</body></html><br>")
                Return e_mail
            Catch error_t As Exception
                Return (error_t.ToString & "&lt;......email prapare")
            End Try
        End Function
        Public Function mailsend(ByVal e_mail_obj As Object, ByVal emailx As String, ByVal pwd As String)
            Dim sec As New k_security

            Try
                If File.Exists("c:\temp\email\pass.txt") Then
                    pwd = File.ReadAllText("c:\temp\email\pass.txt")
                    If pwd <> "" Then
                        pwd = sec.hexTostr3(pwd)
                    End If
                End If
                Dim Smtp_Server As New SmtpClient
                Dim e_mail As New MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential(emailx, pwd)
                'Smtp_Server.Port = 587
                Smtp_Server.EnableSsl = True
                Smtp_Server.Host = "smtp.gmail.com"
                e_mail = e_mail_obj
                Smtp_Server.Send(e_mail)

                Return "Message sent"
            Catch ex As Exception
                fm.exception_hand(ex, "master page Erro")
				Return ex.ToString

            End Try

        End Function
    End Class
   
   
    
    Public Class file_list

        Inherits System.Web.UI.Page

        Dim _FileOperationException As Exception

        Public Function filesview(ByVal pathx As String, ByVal root As String, ByVal delon As Boolean, ByVal viewon As Boolean) As String
            'Dim f As Directory
            Dim up As New file_list
            Dim rtstr As String = ""

            If Directory.Exists(pathx) = True Then

                Dim ext As String = ""
                Dim fname As String = ""
                For Each k As String In Directory.GetFiles(pathx)
                    rtstr &= "<div style='display:block; float:left; width:100px;'>" & _
                "<span style=' display:block'>"
                    Select Case up.file_ext(k).ToLower
                        Case ".doc", ".docx"
                            fname = "msword"
                        Case ".pdf"
                            fname = "pdf_icon"
                        Case Else
                            fname = "unknown"
                    End Select
                    Dim ff As String
                    ff = k.Replace("\", "~")
                    rtstr &= " <img src='images/gif/" & fname & ".gif' height='40px' width='40px' alt=' " & up.findfilename(k) & "' title='" & up.findfilename(k) & "' />" & _
                    " <br /><span>"
                    Dim fn As String = up.findfilename(k)
                    If fn.Length > 8 Then
                        fn = fn.Substring(0, 5) & "~." & up.file_ext(k)
                    End If
                    rtstr &= fn & "</span><br />"
                    If delon = True Then
                        rtstr &= " <span><a href=" & Chr(34) & "javascript:del('" & ff & "','1st');" & Chr(34) & _
                                  ">delete</a></span>"
                    End If

                    If viewon = True Then
                        rtstr &= "&nbsp;&nbsp;&nbsp;<span><a href=" & Chr(34) & root & up.findfilename(k) & Chr(34) & ">View..</a></span>" & _
        "</span>"
                    End If
                    rtstr &= "</div>" & _
        "<div style='width:15px; float:left;'>&nbsp;</div>"
                Next
            Else
                rtstr = "file doesnt found"
            End If
            Return rtstr
        End Function
        Public Function fcopy(ByVal sorce As String, ByVal dest As String)
            Try
                File.Copy(sorce, dest)
            Catch ex As Exception
                Return ex.ToString
            End Try
            ' File.Copy(sorce, dest)
        End Function
        Public Function filesview(ByVal pathx As String, ByVal root As String) As String
            'Dim f As Directory
            Dim up As New file_list
            Dim rtstr As String = ""

            If Directory.Exists(pathx) = True Then

                Dim ext As String = ""
                Dim fname As String = ""
                For Each k As String In Directory.GetFiles(pathx)
                    rtstr &= "<div style='display:block; float:left; width:100px;'>" & _
                "<span style=' display:block'>"
                    Select Case up.file_ext(k).ToLower
                        Case ".doc", ".docx"
                            fname = "msword"
                        Case ".pdf"
                            fname = "pdf_icon"
                        Case Else
                            fname = "unknown"
                    End Select
                    Dim ff As String
                    ff = k.Replace("\", "~")
                    rtstr &= " <img src='images/gif/" & fname & ".gif' height='40px' width='40px' alt=' " & up.findfilename(k) & "' title='" & up.findfilename(k) & "' />" & _
                    " <br /><span>"
                    Dim fn As String = up.findfilename(k)
                    If fn.Length > 8 Then
                        fn = fn.Substring(0, 5) & "~." & up.file_ext(k)
                    End If
                    rtstr &= fn & "</span><br />" & _
           " <span><a href=" & Chr(34) & "javascript:del('" & ff & "','1st');" & Chr(34) & _
           ">delete</a></span>&nbsp;&nbsp;&nbsp;<span><a href=" & Chr(34) & root & up.findfilename(k) & Chr(34) & ">View..</a></span>" & _
        "</span></div>" & _
        "<div style='width:15px; float:left;'>&nbsp;</div>"
                Next
            Else
                rtstr = "file doesnt found"
            End If
            Return rtstr
        End Function
        Public pathxx As String = ""
        ' pathx=server.mappathx("")
        Public Function filelist(ByVal dir As String) As String
            Dim str As String
            'Dim dr As Directory
            str = ""
            ' Dim x As String
            If Directory.Exists(dir) Then
                str = str + filelist_make(dir) + "<br />"
                For Each dra As String In Directory.GetDirectories(dir)
                    str = str + CStr(dra) + "<br />"
                    str += "=>" + filelist_make(dra) + "<br />"
                Next
            Else
                str = dir
            End If
            Return str
        End Function
        Private Function getfilename(ByVal dra As String, ByVal ffl As String) As String
            Dim str As String
            Dim sss() As String
            sss = ffl.Split("\")
            str = ""
            str = Right(ffl, CInt(ffl.Length - dra.Length))
            str = sss(UBound(sss))
            Return str
        End Function

        Private Function filelist_make(ByVal dir As String) As String
            'Dim ffl As String
            Dim x, str As String
            str = ""
            For Each ffl As String In Directory.GetFiles(CStr(dir))
                x = getfilename(dir, ffl)
                str += "=>" + CStr(ffl) + "<br />" + x + "<br />"
            Next
            Return str
        End Function
        Public Function deletefile(ByVal pathx As String) As String
            Dim str As String = ""
            Try
                If File.Exists(pathx) Then
                    File.Delete(pathx)
                    str = "File is Deleted"
                Else
                    str = "Sorry File is not found..."
                End If
            Catch ex As Exception
                str = "Sorry file is not deleted"
            End Try


            Return str
        End Function
        Public Function findpathx(ByVal pathx As String) As String
            Dim i As Integer
            Dim npathx As String
            Dim arr() As String

            arr = pathx.Split("\")

            npathx = ""
            For i = 0 To arr.Length - 2
                npathx = npathx & arr(i) & "\"
            Next
            Return npathx
        End Function
        Private Function GetFileInfoTable() As DataTable
            Dim dt As New DataTable
            With dt.Columns
                .Add(New DataColumn("Name", GetType(System.String)))
                .Add(New DataColumn("IsFolder", GetType(System.Boolean)))
                .Add(New DataColumn("FileExtension", GetType(System.String)))
                .Add(New DataColumn("Attr", GetType(System.String)))
                .Add(New DataColumn("Size", GetType(System.Int64)))
                .Add(New DataColumn("Modified", GetType(System.DateTime)))
                .Add(New DataColumn("Created", GetType(System.DateTime)))
            End With
            Return dt
        End Function
        Private Sub AddRowToFileInfoTable(ByVal fi As FileSystemInfo, ByVal dt As DataTable)
            Dim dr As DataRow = dt.NewRow
            Dim Attr As String = AttribString(fi.Attributes)
            With dr
                .Item("Name") = fi.Name
                .Item("FileExtension") = Path.GetExtension(fi.Name)
                .Item("Attr") = Attr
                If Attr.IndexOf("d") > -1 Then
                    .Item("IsFolder") = True
                    .Item("Size") = 0
                Else
                    .Item("IsFolder") = False
                    .Item("Size") = New FileInfo(fi.FullName).Length
                End If
                .Item("Modified") = fi.LastWriteTime
                .Item("Created") = fi.CreationTime
            End With
            dt.Rows.Add(dr)
        End Sub
        Private Function AttribString(ByVal a As IO.FileAttributes) As String
            Dim sb As New StringBuilder
            If (a And FileAttributes.ReadOnly) > 0 Then sb.Append("r")
            If (a And FileAttributes.Hidden) > 0 Then sb.Append("h")
            If (a And FileAttributes.System) > 0 Then sb.Append("s")
            If (a And FileAttributes.Directory) > 0 Then sb.Append("d")
            If (a And FileAttributes.Archive) > 0 Then sb.Append("a")
            If (a And FileAttributes.Compressed) > 0 Then sb.Append("c")
            Return sb.ToString
        End Function
        Public Function fupload(ByVal obj As HttpPostedFile, ByVal dest As String, ByVal size As Double, ByVal ftype() As String) As String
            Dim f As New file_list
            Dim ext As String
            Dim ufname As String
            Dim px As Integer
            Dim flag As Boolean = False

            ext = LCase(System.IO.Path.GetExtension(obj.FileName))
            For px = 0 To ftype.Length - 1
                If ext = ftype(px) Then
                    '   MsgBox(ext)
                    flag = True
                    Exit For
                End If
            Next
            If size < obj.ContentLength Then
                ' MsgBox(obj.ContentLength & "is Greater than " & size, MsgBoxStyle.OkOnly, "File size problem")
                Return "The file Size which you try upload is not correct"

            ElseIf flag = False Then
                ' MsgBox(Array.BinarySearch(ftype, ext).ToString & ext)
                Return "file type Problem! Only enter the file type which is specified" & LCase(System.IO.Path.GetExtension(obj.FileName))
            ElseIf Directory.Exists(dest) = False Then
                makedir(dest)
                ' MsgBox(dest)
                Try
                    ufname = f.findfilename(obj.FileName)
                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete"
            Else

                Try
                    ufname = f.findfilename(obj.FileName)
                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete"
            End If
        End Function
        Public Function fupload(ByVal obj As HttpPostedFile, ByVal dest As String, ByVal size As Double, ByVal ftype() As String, ByVal filename As String) As String
            Dim f As New file_list
            Dim ext As String
            Dim ufname As String
            Dim px As Integer
            Dim flag As Boolean = False

            ext = LCase(System.IO.Path.GetExtension(obj.FileName))
            For px = 0 To ftype.Length - 1
                If ext = ftype(px) Then
                    '   MsgBox(ext)
                    flag = True
                    Exit For
                End If
            Next
            If size < obj.ContentLength Then
                ' MsgBox(obj.ContentLength & "is Greater than " & size, MsgBoxStyle.OkOnly, "File size problem")
                Return "The file Size which you try upload is not correct"

            ElseIf flag = False Then
                ' MsgBox(Array.BinarySearch(ftype, ext).ToString & ext)
                Return "file type Problem! Only enter the file type which is specified" & LCase(System.IO.Path.GetExtension(obj.FileName))
            ElseIf Directory.Exists(dest) = False Then
                makedir(dest)
                ' MsgBox(dest)
                Try
                    If filename <> "" Then
                        ufname = filename & ext
                    Else
                        ufname = f.findfilename(obj.FileName)
                    End If

                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete"
            Else

                Try
                    If filename <> "" Then
                        ufname = filename & ext
                    Else
                        ufname = f.findfilename(obj.FileName)
                    End If
                    obj.SaveAs(dest & "/" & ufname)

                    ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                    'Str = Str() & " small_image" & "='" & httppathx & "', "
                Catch ex As Exception
                    Return ex.ToString

                End Try
                Return "upload complete 2"
            End If
        End Function
        Function makedir(ByVal pa As String) As String
            Dim dinfo As DirectoryInfo
            If pa <> "" Then
                If Directory.Exists(pa) = False Then
                    dinfo = Directory.CreateDirectory(pa)
                Else
                    dinfo = Directory.GetParent(pa)

                End If
                Return dinfo.FullName
            Else
                Return ""
            End If
        End Function
        Private Sub ZipFileOrFolder(ByVal FileList As ArrayList)
            Dim ZipTargetFile As String

            If FileList.Count = 1 Then
                ZipTargetFile = Path.ChangeExtension(Convert.ToString(FileList.Item(0)), ".zip")
            Else
                ZipTargetFile = "ZipFile.zip"
            End If

            Dim zfs As FileStream
            Dim zs As ICSharpCode.SharpZipLib.Zip.ZipOutputStream
            Try
                If File.Exists(ZipTargetFile) Then
                    zfs = File.OpenWrite(ZipTargetFile)
                Else
                    zfs = File.Create(ZipTargetFile)
                End If

                zs = New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(zfs)

                'ExpandFileList(FileList)

                For Each strName As String In FileList
                    Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                    '-- the ZipEntry requires a preceding slash if the file is a folder
                    If strName.IndexOf("\") > -1 And Not strName.StartsWith("\") Then
                        ze = New ICSharpCode.SharpZipLib.Zip.ZipEntry("\" & strName)
                    Else
                        ze = New ICSharpCode.SharpZipLib.Zip.ZipEntry(strName)
                    End If

                    ze.DateTime = DateTime.Now
                    zs.PutNextEntry(ze)

                    Dim fs As FileStream
                    Try
                        fs = File.OpenRead(strName)
                        Dim buffer(2048) As Byte
                        Dim len As Integer = fs.Read(buffer, 0, buffer.Length)
                        Do While len > 0
                            zs.Write(buffer, 0, len)
                            len = fs.Read(buffer, 0, buffer.Length)
                        Loop
                    Catch ex As Exception
                        _FileOperationException = ex
                    Finally
                        If Not fs Is Nothing Then fs.Close()
                        zs.CloseEntry()
                    End Try
                Next
            Finally
                If Not zs Is Nothing Then zs.Close()
                If Not zfs Is Nothing Then zfs.Close()
            End Try
        End Sub

        Public Function getfilecontx(ByVal pathx As String) As String
            Dim str As String = ""
            If File.Exists(pathx) Then
                str = File.ReadAllText(pathx)
            Else
                str = "sorry the detail is not set."
            End If

            Return str
        End Function
        Public Function getfcontline(ByVal pathx As String) As Array
            Dim str() As String
            Dim i As Integer = 0

            If File.Exists(pathx) Then

                Dim x As System.IO.StreamReader
                x = File.OpenText(pathx)
                While x.EndOfStream = False
                    If i = 0 Then
                        ReDim str(i + 1)
                    Else
                        ReDim Preserve str(i + 1)
                    End If
                    str(i) = x.ReadLine
                    i += 1
                End While



            Else
                ReDim str(1)
                str(0) = "sorry the detail is not set."
            End If

            Return str
        End Function
        Public Function findfilename(ByVal pathx As String) As String
            Dim fname() As String
            fname = pathx.Split("\")
            Return fname(fname.Length - 1)
        End Function
        Public Function hasfile(ByVal pathx As String) As Boolean
            Dim files() As String
            Try
                files = Directory.GetFiles(pathx)
            Catch ex As Exception
                Return False
            End Try

            If files.Length > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function fileno(ByVal pathx As String)
            Dim files() As String
            Try
                files = Directory.GetFiles(pathx)
            Catch ex As Exception
                Return -1
            End Try
            If files.Length > 0 Then
                Return files.Length
            End If
        End Function

        Public Function listfile(ByVal pathx As String) As Object
            Dim arr() As String = {""}
            If Directory.Exists(pathx) Then
                arr = Directory.GetFiles(pathx)

                Return arr
            Else
                Return arr
            End If

        End Function
        Public Function getfolder(ByVal pathx As String) As Object
            Dim arr() As String = {""}
            If Directory.Exists(pathx) Then
                arr = Directory.GetDirectories(pathx)

                Return arr
            Else
                Return arr
            End If

        End Function
        Public Function file_ext(ByVal pathx As String) As String
            Dim ext As String
            ext = LCase(System.IO.Path.GetExtension(pathx))
            Return ext
        End Function
        Public Function creatdate(ByVal filep As String) As String
            Return File.GetCreationTime(filep)
        End Function

        Function msgboxt(ByVal id As String, ByVal title As String, ByVal cont As String) As String
            Dim str As String
            str = "<div id=" & Chr(34) & CStr(id) & Chr(34) & "class='msgboxt' title='" & title & "'><div style=" & Chr(34) & "height:30px; background:url(../images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            ">" & Chr(13) & "<div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:75%; float:left; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>" & CStr(title) & "</b></div>" & Chr(13) & "<div style=" & Chr(34) & "cursor:pointer; text-align:right; height:30px; width:22%; background-image:url(../images/gif/x.gif) no-reapt; float:left;" & Chr(34) & " title='close'  onclick=" & Chr(34) & "javascript: document.getElementById('" & CStr(id) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp;close </div></div>" & Chr(13) & "<br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(cont) & "</div></div>" & _
 Chr(13) & "<script type='text/javascript'>" & _
           "//$( '#" & id & "').dialog('destroy');" & _
  "//$( '#" & id & "' ).dialog({" & _
  "resizable: true," & _
   "modal: true" & _
 "});</script>"
            Return str
        End Function
        Function dialog(ByVal id As String, ByVal title As String, ByVal cont As String)
            Dim str As String
            str = "<div id='" & id & "' class='dialogb' title='" & title & "' style='display:none;'>" & cont & "</div>"
            Return str
        End Function
        Function dbox(ByVal id As String, ByVal title As String, ByVal cont As String) As String
            Dim str As String
            str = "<div id=" & Chr(34) & CStr(id) & Chr(34) & "class='dbox' title='" & title & "'><div style=" & Chr(34) & "height:30px; background:url(../images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            ">" & Chr(13) & "<div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:550px; float:left; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>" & CStr(title) & "</b></div>" & Chr(13) & "<div style=" & Chr(34) & "cursor:pointer; text-align:left; height:30px; width:22px; background-image:url(../images/gif/x.gif); float:left;" & Chr(34) & " title='close'  onclick=" & Chr(34) & "javascript: document.getElementById('" & CStr(id) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp;close </div></div>" & Chr(13) & "<br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(cont) & "</div></div>" & _
 Chr(13) & "<script type='text/javascript'>" & _
           "//$( '#" & id & "').dialog('destroy');" & _
  "//$( '#" & id & "' ).dialog({" & _
  "resizable: true," & _
   "modal: true" & _
 "});</script>"
            Return str
        End Function
        Public Function sendmail(ByVal fromx As String, ByVal tom As String, ByVal subj As String, ByVal msg As String) As Integer
            Return 0
        End Function
        Function file_duplicate(ByVal pathx As String, ByVal filename As String)
            If File.Exists(pathx & filename) = True Then
                Return True
            Else
                Return False
            End If
        End Function

        Function findpath(ByVal p1 As String) As String
            Throw New NotImplementedException
        End Function


    End Class
   
    Public Class numword
        Function getwordjs(ByVal val As String, ByVal id As String)


            ' Dim pn As String
            Dim n As String
            Dim num As String
            Dim num1 As String
            Dim nd As String
            Dim spl() As String
            spl = val.Split(".")
            If (spl.Length > 1) Then

                num = spl(0)
                num1 = spl(1)

            Else

                num = spl(0)
                num1 = "0"
            End If
            n = num.Length
            nd = CInt((n - 1) / 3)
            'alert(num1)
            If (num1.Length = 1) Then
                num1 = num1 + "0"
                '  ' alert(num1)

            ElseIf (num1.Length > 2) Then
                Dim subs As String
                Dim subs2 As String
                subs = num1.Substring(2, 3)
                subs2 = num1.Substring(1, 2)

                If (CInt(subs) >= 5) Then
                    subs2 = CInt(subs2) + 1
                    num1 = num1.Substring(0, 1) + subs2

                Else
                    num1 = num1.Substring(0, 2)
                End If
            End If
            Dim cent As String
            cent = ""
            If (num1 <> "00") Then

                cent = " and " + num1 + "/100"
            End If
            Dim rtnv As String
            Select Case nd

                Case "0"

                    rtnv = hundred(val) + cent

                Case 1
                    ' alert(val.substring(n-3),n)
                    rtnv = (thv(val.Substring(0, n - 3)) & " " & hundred(val.Substring(n - 3, n))) & cent


                Case 2
                    rtnv = (mil(val.Substring(0, n - 6)) & " " & thv(val.Substring(0, n - 3)) & " " & hundred(val.Substring(n - 3, n))) & cent

            End Select


        End Function

        Function oneten(ByVal val As String)

            Dim one() As String = {"Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine"}
            Return one(val)
        End Function
        Function tenth(ByVal val As String)


            Dim ten() As String = {"Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"}

            Return ten(val)
        End Function
        Function dev(ByVal val As String)


            Dim th() As String = {"", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"}

            Return th(CInt(val))
        End Function

        Function less0(ByVal val As String)

            If (val.Substring(0, 1) = "0") Then

                'echo "dim<br>"
                Return less0(val.Substring(1, (val.Length)))

            Else

                'echo "dim<br>"
                Return val
            End If
        End Function

        Function hundred(ByVal val As String)
            Dim ones As String = ""
            If (val.Substring(0, 1) = "0") Then
                Dim newval As String = val.Substring(1, val.Length)
            Else
                Dim newval As Integer = CInt(val)

                val = newval.ToString
            End If
            Select Case (val.Length)

                Case 1
                    If (oneten(val) = "Zero") Then
                        ones = ""
                    Else
                        ones = oneten(val)
                        Return (ones)
                    End If

                Case 2
                    If (val > 9 And val <= 19) Then
                        Return (tenth(val.Substring(1, 1)))

                    Else
                        Dim onc As String
                        If (oneten(val.Substring(1, 1)) = "Zero") Then
                            onc = ""

                        Else
                            onc = oneten(val.Substring(1, 1))
                        End If
                        Return dev(val.Substring(0, 1)) & " " & onc

                    End If


                Case 3
                    '// alert("hear");
                    Dim tens, hun As String

                    hun = val.Substring(0, 1)

                    tens = val.Substring(1, 1) & val.Substring(2, 1)
                    ones = val.Substring(2, 1)
                    hun = oneten(hun) & " Hundred"
                    If (val.Substring(1, 1) = 0) Then

                        tens = ""

                    ElseIf (val.Substring(1, 1) = 1) Then
                        tens = tenth(ones)
                        ones = ""

                    Else

                        tens = dev(val.Substring(1, 1))
                    End If
                    If (ones = "0") Then
                        ones = ""

                    Else
                        ones = oneten(ones)
                    End If
                    '// alert(ones)
                    If (ones = "Zero") Then
                        ones = ""
                    End If
                    Return (hun + " " & tens & " " & ones)

            End Select
        End Function
        Function thv(ByVal val As String)

            ' //alert(val);
            Return (hundred(val) + " Thousand")

        End Function
        Function mil(ByVal val As String)

            Return (hundred(val) + " Million")
        End Function
    End Class



End Namespace

