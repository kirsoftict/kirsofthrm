Imports System.Data
Imports System.Data.SqlClient
Imports Kirsoft.hrm
Partial Class bankacctrans1
    Inherits System.Web.UI.Page
    Public Function makebank()
        Dim col() As String = {"", "", "", "", "", "", ""}
        Dim outp As String = ""
        Dim projectname As String = "All Project"
        Dim date1 As Date
        Dim projid As String = ""
        Dim sql As String = ""
        If Request.Form("projname") <> "" Then
            Dim spl() As String
            spl = Request.Form("projname").Split("|")
            If spl.Length > 1 Then
                projectname = spl(0)
                projid = spl(1)
            Else

            End If
            date1 = Request.Form("to_date")
            Dim dbs As New dbclass
            Dim rs As DataTableReader
            Dim fm As New formMaker
            outp = "<table>"
            outp &= "<tr><td colspan='7'>" & Session("companyname") & "</td></tr>" & Chr(13)
            outp &= "<tr><td colspan='7'>" & projectname & "</td></tr>" & Chr(13)
            outp &= "<tr><td colspan='7'>List of Account Number. at date of:" & date1.ToLongDateString & "</td></tr>" & Chr(13)
            outp &= "<tr><td>No.</td><td>Name of Employee</td><td>Account No.</td><td>Perdiem</td><td>Salary</td><td>Total</td><td>Branch</td></tr>" & Chr(13)
            Sql = "select * from emprec inner join emp_static_info on emprec.emp_id=emp_static_info.emp_id where id in(select emptid from emp_job_assign where project_id='" & projid & "') order by emp_static_info.first_name"



        End If
       End Function
End Class
