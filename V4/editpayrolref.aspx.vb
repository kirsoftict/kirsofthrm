Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm

Partial Class editpayrolref
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      
    End Sub
    Function comex()
        If Request.QueryString("editref") = "editref" Then
            Response.Write("<form name='frmx' id='frmx' action='' method='post'><table><tr><td colspan='2'>Edit Payrol Ref.</td></tr>" & _
            "<tr><td>Old ref. No:</td><td>" & Request.QueryString("ref") & "<input type='hidden' value='" & Request.QueryString("ref") & "' name='ref' id='ref'></td></tr>" & _
            "<tr><td>New Ref. No:</td><td><input type='text' id='reff' name='reff'></td></tr>" & _
            "<tr><td colspan='2' align='center'><input type='button' value='Change' onclick='javascript:goclick();'></td></tr></table></form>")
        ElseIf Request.QueryString("editref") = "save" Then
            Dim sql As String = ""
            Dim dbs As New dbclass
            sql = "Begin Trans " & Session("emp_iid") & Chr(13) & sql & Chr(13)
            sql &= "Update emp_ot set ref='" & Request.Form("reff") & "' where ref='" & Request.Form("ref") & "'" & Chr(13)
            sql &= "Update emp_loan_settlement set ref='" & Request.Form("reff") & "' where ref='" & Request.Form("ref") & "'" & Chr(13)
            sql &= "Update emp_inc set paidref='" & Request.Form("reff") & "' where paidref='" & Request.Form("ref") & "'" & Chr(13)
            sql &= "Update payrollx set ref='" & Request.Form("reff") & "' where ref='" & Request.Form("ref") & "'"


            Dim flg As Object = dbs.excutes(sql, Session("con"), Session("path"))
            If CInt(flg) > 0 Then
                dbs.excutes("Commit Trans " & Session("emp_iid"), Session("con"), Session("path"))
                Response.Write("It is Updated")
                Response.Write(Session("chgref"))
                Session("chgref") = ""
                Response.Write("<script>submitx();</script>")
                'Response.Write("<script>ha('pay');</script>")
            Else
                dbs.excutes("RollBack Trans " & Session("emp_iid"), Session("con"), Session("path"))
                Response.Write("<script>submitx();</script>")
                'Response.Redirect("payroll.aspx")
            End If
        ElseIf Request.QueryString("editref") = "del" Then

        End If
    End Function
    Function deleteallx()
        Dim ref As String = Request.QueryString("ref")
        Dim emptid As String = Request.QueryString("emptid")
        Dim dbs As New dbclass
        Dim flg As String = ""
        Dim msg As String = ""
        Dim sqlst As String = "BEGIN Trans " & Session("emp_iid")


        sqlst &= "delete from payrollx where ref='" & ref & "' and emptid='" & emptid & "'" & Chr(13)

        sqlst &= "delete from emp_loan_settlement where ref='" & ref & "' and id in(select id from vwloanref where emptid='" & emptid & "' and ref='" & ref & "')" & Chr(13)

        sqlst &= "Update emp_ot set paidstatus='n',ref=NULL where ref='" & ref & "' and emptid='" & emptid & "'" & Chr(13)
        sqlst &= "COMMIT Trans " & Session("emp_iid")

        'Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
        flg = dbs.excutes(sqlst, Session("con"), session("path"))
        'Response.Write(flg.ToString)
        If IsNumeric(flg) = True Then
            ' Response.Write(flg)
            If CInt(flg) <= 0 Then
                dbs.excutes("RollBack Trans " & Session("emp_iid"), Session("con"), Session("path"))
                msg = "Data is not saved"
            Else
                'dbs.excutes("Commit", Session("con"),session("path"))
                msg = "Data Saved"
            End If



        End If

        dbs = Nothing
        Return msg
    End Function
    Function deleteallxinc()
        Dim ref As String = Request.QueryString("ref")
        Dim emptid As String = Request.QueryString("emptid")
        Dim dbs As New dbclass
        Dim flg As String = ""
        Dim msg As String = ""
        Dim sqlst As String = "BEGIN Trans " & Session("emp_iid") & Chr(13)


        sqlst &= "delete from payrollx where ref='" & ref & "' and emptid='" & emptid & "'" & Chr(13)

        ' sqlst &= "delete from emp_loan_settlement where ref='" & ref & "' and id in(select id from vwloanref where emptid='" & emptid & "' and ref='" & ref & "')" & Chr(13)

        sqlst &= "Update emp_inc set paidref=Null where paidref='" & ref & "' and emptid='" & emptid & "'" & Chr(13)


        Response.Write("<textarea cols='100' rows='15'>" & sqlst & "</textarea>")
        flg = dbs.excutes(sqlst, Session("con"), session("path"))
        Response.Write(flg.ToString)
        If IsNumeric(flg) = True Then
            ' Response.Write(flg)
            If CInt(flg) <= 0 Then
                dbs.excutes("RollBack Trans " & Session("emp_iid"), Session("con"), Session("path"))
                msg = "Data is not saved"
            Else
                dbs.excutes("Commit Trans " & Session("emp_iid"), Session("con"), Session("path"))
                msg = "Data Saved"
            End If



        End If

        dbs = Nothing
        Return msg
    End Function
End Class
