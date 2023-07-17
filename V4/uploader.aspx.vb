Imports System.Data
Imports System.IO
Imports Kirsoft.hrm
Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports Sage.Peachtree.Interop

Partial Class uploader
    Inherits System.Web.UI.Page
    Function incdist(ByVal filepath As String)

        Dim sec As New k_security

        Dim newcs As New OleDbConnection
        ' newcs.ConnectionString("Provider=Microsoft.jet.oledb.4.0;Data Source=
        Dim fls As New file_list
        Dim fname As String
        Dim flp As String = sec.dbHexToStr(Request.QueryString("fp"))
        Dim fl() As String
        'renamesheet(flp)
        Dim rs As OleDbDataReader
        fname = fls.findfilename(flp)

        fl = fname.Split(".")
        Dim dbx As New dbclass
        Dim rrs As DataTable
        Dim fm As New formMaker
        Dim mktable As String = ""
        Try
            Response.Write("<br>" & flp & "<br>")
            newcs.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & flp & "';Extended Properties=" & Chr(34) & "Excel 8.0;HDR=Yes;IMEX=1" & Chr(34)
            Response.Write("connection string works")
            newcs.Open()
        Catch ex As Exception
            Try
                newcs.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & flp & "';Extended Properties=" & Chr(34) & "Excel 12.0;HDR=Yes;IMEX=1" & Chr(34)

            Catch ex2 As Exception
                Response.Write("<br> 2." & ex2.ToString)

            End Try

            Response.Write("<br> 1......" & ex.ToString)


        End Try
        Try


            If newcs.State = ConnectionState.Closed Then
                newcs.Open()
                Dim st() As String
                Dim fldcount As Integer

                st = listtable(newcs)
                If st.Length > 0 Then
                    Dim clate As String
                    Dim abs As String
                    Dim namex As String = ""
                    Dim emptid, emp_id, amt1, amt2, pda, incdate, reason, who_reg, date_reg As String
                    Dim sql As String = "select * from [" & st(0) & "]"
                    rs = dtmake(st(0), sql, newcs)
                    Dim cc As Integer = 1
                    If rs.HasRows Then
                        mktable = "<div id='book' class='book'>"
                        fldcount = rs.FieldCount

                        Dim summ As String = ""
                        While rs.Read
                            emptid = ""
                            emp_id = ""
                            amt1 = "0"
                            amt2 = "0"

                            '  emptid = CInt(fm.getinfo2("select id from emprec where emp_id='" & dtt.Item("emp_id") & "' and '" & CDate(Request.Form("inc_date")).AddMonths(-1) & _
                            '     "' between hire_date and isnull(end_date,'" & Today.ToShortDateString & "') order by id", Session("con")))

                            sql &= "insert into emp_inc(emptid,emp_id,inc_date,reason,amt,amt2,paid_date,who_reg,date_reg) Values('" & emptid & "','" & emp_id & "','" & Request.QueryString("inc_date") & _
                     "','" & Request.QueryString("reason") & "','" & Request.QueryString("amt") & "','" & Request.QueryString("amt2") & "'," & pda.ToString & ",'" & Request.QueryString("who_reg") & "','" & Request.QueryString("date_reg") & "')"

                        End While
                        mktable &= "</table></div></div>"
                        ' Response.Write(mktable)
                    End If
                    rs.Close()



                End If
                newcs.Close()
                newcs = Nothing
            Else
                Response.Write("connection is opppppeeennn")
            End If
        Catch ex3 As Exception
            Response.Write("<br>.....3. " & ex3.ToString & "<br>")
        End Try

        ' Response.Write(flp)

        Return mktable
    End Function

    Public Function listtable(ByRef conx As OleDbConnection) As Array
        Dim ds As New DataSet
        Dim st() As String = {""}
        Dim dd As New DataTable
        Dim dt As OleDbDataAdapter = New OleDbDataAdapter()

        Dim i As Integer = 0
        '   dt.TableMappings.Add("Table", "sys.objects")
        Dim sql As String = ""
        dd = conx.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "Table"})
        For Each kx As DataRow In dd.Rows
            ReDim Preserve st(i + 1)
            st(i) = kx("TABLE_NAME")
            st(i) = st(i).Replace("'", "")
            i += 1
        Next
        Return st
    End Function
    Public Function dtmake(ByVal st As String, ByVal sql As String, ByRef conx As OleDbConnection) As Object
        Dim cmdx As New OleDbCommand
        Dim rs As OleDbDataReader
        'sql = "select * from [" & st(i) & "]"
        ' Response.Write(sql & "<br>")
        With cmdx
            .Connection = conx
            .CommandType = CommandType.Text
            .CommandText = sql
            rs = .ExecuteReader
        End With
        Return rs
    End Function
End Class
