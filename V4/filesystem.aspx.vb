Imports System.IO
Imports System.IO.Path
Imports Kirsoft.hrm
Imports System.Collections.Generic

Partial Class filesystem
    Inherits System.Web.UI.Page
    Dim fl As New file_list
    Dim loc As String = Server.MapPath("")
    Public count As Integer
    Public cfld As Integer
    Public pathfmain, pathf As String
    Private actualpath As String
    Dim orderedFiles As Object

    Public Sub New()
        pathfmain = Server.MapPath("")
    End Sub
    Function address(ByVal p As String)
        Dim rtnv As String = ""
        'Response.Write(p)
        rtnv = p.Substring(loc.Length, (p.Length - loc.Length))

        If rtnv = "" Then
            rtnv = "\"
        End If
        actualpath = p
        '  Response.Write(actualpath & "<br>")
        Return rtnv
    End Function
    Protected Sub filesystem_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '  Response.Write(pathfmain)
        count = 0
        cfld = 0
    End Sub
    
    Function files(ByVal p As String)
        Dim pimg As String = ""
        Dim url As String
        Dim pth_m As String = ""
        Dim sec As New k_security
        Dim rtn As String = "<table>"
        For Each k As String In Directory.GetFiles(p)

            count += 1
            ' Response.Write(GetExtension(k) & "<br>")

            If GetExtension(k).Length > 1 Then
                pimg = "images/filesystem/blue/" & GetExtension(k).Substring(1, GetExtension(k).Length - 1) & ".png"
                '  Response.Write(loc & "/" & pimg & "<br>")
                If File.Exists(loc & "/" & pimg) = False Then
                    pimg = "images/filesystem/blue/unknown.png"
                End If
            Else
                pimg = "images/filesystem/blue/unknown.png"
            End If
            ''  Response.Write(k.Length.ToString & k & "===" & loc.Length.ToString & loc & "<br>")
            Dim ik, jk As Integer
            ik = k.Length
            jk = loc.Length
            pth_m = k.Substring(loc.Length, (k.Length - loc.Length))
            pth_m = pth_m.Replace("\", "/")

            url = "http://" & Request.ServerVariables("HTTP_HOST") & pth_m
            rtn &= "<tr><td><!--input type=checkbox id='file" & count & "' name='file" & count & "'--></td><td><img src='" & pimg & "' height='20px'>"
            If LCase(GetExtension(k)) = ".htm" Or LCase(GetExtension(k)) = ".html" Then
                rtn &= "<a href=" & Chr(34) & url & Chr(34) & " target='_blank'>" & fl.findfilename(k) & "</a>"
            Else
                rtn &= fl.findfilename(k)
            End If
            rtn &= "</td><td><img src='images\gif\btn_delete.gif' height='20' onclick=" & Chr(34) & "javascript:deletef('" & sec.dbStrToHex(k) & "');" & Chr(34) & " style='curser:pointer;'/></td>"
            rtn &= "<td><img src='images\filesystem\blue\download.jpg' height='20' onclick=" & Chr(34) & "javascript:download('" & sec.dbStrToHex(k) & "');" & Chr(34) & " style='curser:pointer;'/></td>"
            rtn &= "</tr>"
            ' Response.Write(count.ToString & "." & fl.findfilename(k) & loc & "\images\filesystem\blue\" & fl.file_ext(k) & "<br>")
        Next
        rtn &= "</table>"
        Response.Write(rtn)
    End Function
    Function folders(ByVal p As String)
        Dim fldlist As New ArrayList
        Dim fllist As New ArrayList
        Dim sec As New k_security
        Dim fld() As Object
        fld = sortdir(p)
        Dim rtn As String = "<table>"
        For k As Integer = 0 To UBound(fld) - 1
            If isdir(fld(k)) Then

                If isempty(fld(k)) Then
                    rtn &= ("<tr class='fldview'><td><img src='images/filesystem/blue/folder2.gif' width='25px'><a href=" & Chr(34) & "javascript:clicked('" & sec.dbStrToHex(fld(k)) & "','folder');" & Chr(34) & ">" & get_name(fld(k)) & "</a></td><td>&nbsp;</td></tr>")
                   
                Else
                    rtn &= ("<tr class='fldview'><td><img src='images/filesystem/blue/folder1.gif' width='25px'><a href=" & Chr(34) & "javascript:clicked('" & sec.dbStrToHex(fld(k)) & "','folder');" & Chr(34) & ">" & get_name(fld(k)) & "</a></td><td><img src='images\gif\btn_delete.gif' height='20' onclick=" & Chr(34) & "javascript:delfld('" & sec.dbStrToHex(get_name(fld(k))) & "');" & Chr(34) & "/></td></tr>")


                End If
                '   Response.Write(k & "<br>")
                ' files(k)
                'folders(k)
            End If
        Next
        rtn &= "</table>"
        Response.Write(rtn)
    End Function
    Function isdir(ByVal p As String)
        If Directory.Exists(p) Then Return True Else Return False
    End Function
    Function isempty(ByVal p As String)
        Dim c As Integer = 0
        For Each k As String In Directory.GetFiles(p)
            c = c + 1

        Next
        For Each k As String In Directory.GetDirectories(p)
            c = c + 1

        Next
        If c > 0 Then Return True Else Return False
    End Function
    Function get_name(ByVal p As String)
        Dim spl() As String
        spl = p.Split("\")
        Return spl(spl.Length - 1)
    End Function
    Function getinfo(ByVal p As String)
        Dim fattr As New FileInfo(p)
        Dim rtn(6) As String
        rtn(0) = fattr.Name.ToString
        rtn(1) = fattr.FullName.ToString
        rtn(2) = fattr.Extension
        rtn(3) = fattr.DirectoryName
        rtn(4) = fattr.CreationTime
        rtn(5) = fattr.Length
        Return rtn

    End Function
    Function getdirinfo(ByVal p As String)
        Dim fattr As New DirectoryInfo(p)
        Dim rtn(6) As String
        rtn(0) = fattr.Name.ToString
        rtn(1) = fattr.FullName.ToString

        rtn(2) = fattr.CreationTime
        rtn(3) = ""
        rtn(4) = ""
        rtn(5) = ""

        Return rtn

    End Function
    Function othersort(ByVal path As String)

       
       
    End Function
    Function sortdir(ByVal path As String)
        Dim fldl() As Object = {""}
        Dim dtf() As Object = {""}
        If Directory.Exists(path) Then
            Dim filesx As New DirectoryInfo(path)
            Dim fld() As DirectoryInfo
            fld = filesx.GetDirectories()



            For Each k As DirectoryInfo In fld
                If (k.Name <> " ") Or k.Name <> "." Then
                    ' Response.Write(k.Name & "==" & k.CreationTime & "<br>")

                    pusharray(fldl, k.FullName)
                    pusharray(dtf, k.CreationTime)
                End If

            Next
            Dim temp1, tem2 As Object
            For i As Integer = 0 To UBound(fldl) - 2
                For j As Integer = i + 1 To UBound(fldl) - 1
                    If compair(dtf(i), dtf(j)) Then
                        temp1 = dtf(i)
                        dtf(i) = dtf(j)
                        dtf(j) = temp1
                        temp1 = fldl(i)
                        fldl(i) = fldl(j)
                        fldl(j) = temp1
                        temp1 = ""
                    End If
                Next


            Next
            Return fldl
        End If
        Return fldl
        ' Response.Write(fldl.Length)
    End Function
    Function compair(ByVal d1 As Object, ByVal d2 As Object)
        If IsDate(d1) And IsDate(d2) Then
            If CDate(d1).Subtract(CDate(d2)).Ticks < 0 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Function pusharray(ByRef arr() As Object, ByVal item As String) As Array
        If IsError(item) Or item <> "" Then
            Dim l As Integer
            l = arr.Length
            ' Response.Write(l & "<br>")
            ReDim Preserve arr(l)
            arr(l - 1) = item
        End If

    End Function
    Private Function CompareFileInfos(ByVal file1 As FileInfo, ByVal file2 As FileInfo) As Integer
        Dim result = 0


        If result = 0 Then


            result = file2.LastWriteTime.CompareTo(file1.LastWriteTime)

        End If

        Return result
    End Function
    Public Function createdir(ByVal p As String)
        ' Response.Write("try create: " & Session("folder") & "\" & p & "xxxxxxxxxxxxxxxx<br>")
        Dim rs As Object
        If Directory.Exists(Session("folder") & "\" & p) = False Then
            Try
                MkDir(Session("folder") & "/" & p)
                Response.Write("created!!!!!!!" & Session("folder") & "/" & p)
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

        Else
            Response.Write("Existed")
        End If
       

    End Function
    Function deldir(ByVal p As String)
        Response.Write(p & "=======>" & Session("folder"))
        If Directory.Exists(Session("folder") & "\" & p) = True Then
            If isempty(Session("folder") & "\" & p) = True Then
                Response.Write("Folder is not Empty!!!")
            Else
                Try
                    RmDir(Session("folder") & "/" & p)
                    Response.Write("folder is deleted!!")
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End If
        Else
            Response.Write("Folder is not exists")
        End If
    End Function
    Private Sub del(ByVal p As String)
        Throw New NotImplementedException
    End Sub
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
    Public Function fupload(ByVal obj As HttpPostedFile, ByVal dest As String, ByVal size As Double, ByVal ftype() As String, ByVal fnot() As String) As String
        Dim f As New file_list
        Dim ext As String
        Dim ufname As String
        Dim px As Integer
        Dim flag As Boolean = False

        ext = LCase(System.IO.Path.GetExtension(obj.FileName))
        If ftype.Length > 1 Then
            For px = 0 To ftype.Length - 1
                If ext = ftype(px) Then
                    '   MsgBox(ext)
                    flag = True
                    Exit For
                End If
            Next
        Else
            flag = True

        End If
        If fnot.Length > 1 Then
            For px = 0 To fnot.Length - 1
                If ext = fnot(px) Then
                    '   MsgBox(ext)
                    flag = False
                    Exit For
                End If
            Next
        Else
            flag = True

        End If
        ' Response.Write(obj.ContentLength)
        ' Response.Write(dest)
        If size < obj.ContentLength Then
            ' MsgBox(obj.ContentLength & "is Greater than " & size, MsgBoxStyle.OkOnly, "File size problem")
            Return "The file Size which you try upload is not correct"

        ElseIf flag = False Then
            ' MsgBox(Array.BinarySearch(ftype, ext).ToString & ext)
            Return "file type Problem! Only enter the file type which is specified" & LCase(System.IO.Path.GetExtension(obj.FileName))
        ElseIf Directory.Exists(dest) = False Then
            MkDir(dest)
            ' MsgBox(dest)
            Try
                ufname = f.findfilename(obj.FileName)
                If File.Exists(dest & "/" & ufname) = False Then
                    obj.SaveAs(dest & "/" & ufname)
                Else
                    If Request.Form("replace") = "on" Then
                        obj.SaveAs(dest & "/" & ufname)
                    End If
                End If

                ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                'Str = Str() & " small_image" & "='" & httppathx & "', "
            Catch ex As Exception
                Return ex.ToString

            End Try
            Return "upload complete"
        Else

            Try
                ufname = f.findfilename(obj.FileName)
                If File.Exists(dest & "/" & ufname) = False Then
                    obj.SaveAs(dest & "/" & ufname)
                Else
                    If Request.Form("replace") = "on" Then
                        obj.SaveAs(dest & "/" & ufname)
                    Else
                        Return "File existed!!"
                    End If
                End If

                ' httppathx = "pages/" & Request.Form("menu_id") & "/" & Request.Form("id") & "/images/" & ufname
                'Str = Str() & " small_image" & "='" & httppathx & "', "
            Catch ex As Exception
                Return ex.ToString

            End Try
            Return "upload complete"
        End If
    End Function

   
End Class
Public Class DateComparer
    Implements System.Collections.IComparer

    Public Function Compare(ByVal info1 As Object, ByVal info2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim FileInfo1 As System.IO.FileInfo = DirectCast(info1, System.IO.FileInfo)
        Dim FileInfo2 As System.IO.FileInfo = DirectCast(info2, System.IO.FileInfo)

        Dim Date1 As DateTime = FileInfo1.CreationTime
        Dim Date2 As DateTime = FileInfo2.CreationTime

        If Date1 > Date2 Then Return 1
        If Date1 < Date2 Then Return -1
        Return 0
    End Function
End Class