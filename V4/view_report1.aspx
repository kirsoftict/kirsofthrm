<%@ Page Language="VB" AutoEventWireup="false" CodeFile="view_report1.aspx.vb" Inherits="view_report1" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
    function print(loc,title,head,footer)
    { 
    var ysno=confirm("please change the paper layout to landscape");
    
    if(ysno==true){
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<html><head><title>" + head +"</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
    printWin.close();
    }
    
    }
    </script>
</head>
<body>
	
<%  
    For Each k As String In Request.QueryString
        Session(k) = Request.QueryString(k)
    Next
    Dim title As String = Session("company_name")
    Dim sql As String = ""
    Dim out As String = ""
    Dim active As String = ""
    If Request.QueryString.HasKeys = True Then
        active = Request.QueryString("active")
   
        If active <> "" Then
            If active = "y" Then
                active = "(Active Employees)"
            Else
                active = "(Deactive Employess)"
            End If
        Else
            active = "(All Employees)"
        End If
        Select Case (Request.QueryString("val"))
            Case "all"
                sql = "select * from emprec order by hire_date,id desc"
                out = makeformx(sql)
                title &= "<br>List of Emplyee's "
            Case "bydep"
                
                Dim dep() As String
                dep = Request.QueryString("department").Split("|")
                title &= "<br>List of Emplyee's Filtered by Department " & dep(1) & " " & active
                If Request.QueryString("active") <> "" Then
                    sql = "select * from emprec where emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) and active='" & Request.QueryString("active") & "' order by id desc"
                Else
                    sql = "select * from emprec where emp_id in(select emp_id from emp_job_assign where department='" & dep(0) & "' and date_end is null) order by id desc"

                End If
                
                ' Response.Write(sql)
                
                out = makeformx(sql)
            Case "byprojdate"
                
                sql = ""
                If Request.QueryString("projdateto") = "" And Request.QueryString("projdate") <> "" Then
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where id in(select emptid from tblproj_assign where start_date >='" & Request.QueryString("projdate") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where id in(select emptid from tblproj_assign where start_date >='" & Request.QueryString("projdate") & "') order by id desc"

                    End If
                ElseIf Request.QueryString("projdateto") <> "" And Request.QueryString("projdate") <> "" Then
                    If Request.QueryString("active") <> "" Then
                        sql = "select * from emprec where id in(select emptid from tblproj_assign where start_date between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                    Else
                        sql = "select * from emprec where id in(select emptid from tblproj_assign where start_date between '" & Request.QueryString("projdate") & "' and '" & Request.QueryString("projdateto") & "') order by id desc"

                    End If
                Else
                    out = "Sorry date is not selected"
                End If
                If sql <> "" Then
                    out = makeformx(sql)
                End If
               
            Case "byproj"
                sql = ""
                Dim dep() As String
                dep = Request.QueryString("projx").Split("|")
                title &= "<br>List of Emplyee's Filtered by Project " & dep(1) & " " & active
                If Request.QueryString("active") <> "" Then
                    sql = "select * from emprec where id in(select emptid from tblproj_assign where project_id='" & dep(0) & "') and active='" & Request.QueryString("active") & "' order by id desc"
                Else
                    sql = "select * from emprec where id in(select emptid from tblproj_assign where Project_id='" & dep(0) & "') order by id desc"

                End If
                
                ' Response.Write(sql)
                If sql <> "" Then
                    out = makeformx(sql)
                End If
            Case "bydis"
                sql = ""
                title &= "<br>List of Emplyee's Filtered by Field of Study " & Request.QueryString("discipline") & " " & active
                If Request.QueryString("active") <> "" Then
                    sql = "select * from emprec where emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                Else
                    sql = "select * from emprec where emp_id in(select emp_id from emp_education where diciplin='" & Request.QueryString("discipline") & "') order by id desc"
                End If
                
                If sql <> "" Then
                    out = makeformx(sql)
                End If 'Response.Write(sql)
               
                ' For Each k As String In Request.QueryString
                'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                'Next
            Case "byrectime"
                ' For Each k As String In Request.QueryString
                'Response.Write(k & " ____>" & Request.QueryString(k) & "<br>")
                'Next
                If Request.QueryString("active") <> "" Then
                    sql = "select * from emprec where hire_date between '" & Request.QueryString("recdate") & "' and '" & Request.QueryString("recdateto") & "' and active='" & Request.QueryString("active") & "' order by hire_date, id desc"
                Else
                    sql = "select * from emprec where hire_date between '" & Request.QueryString("recdate") & "' and '" & Request.QueryString("recdateto") & "' order by hire_date, id desc"


                End If
                If sql <> "" Then
                    out = makeformx(sql)
                End If
            Case "byqual"
                sql = ""
                title &= "<br>List of Emplyee's Filtered by Qualification " & Request.QueryString("qualification") & " " & active
                If Request.QueryString("active") <> "" Then
                    sql = "select * from emprec where emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') and active='" & Request.QueryString("active") & "' order by id desc"
                Else
                    sql = "select * from emprec where emp_id in(select emp_id from emp_education where qualification='" & Request.QueryString("qualification") & "') order by id desc"
                End If
                If sql <> "" Then
                    out = makeformx(sql)
                End If 'Response.Write(sql)
               
            Case "bypost"
                sql = ""
                title &= "<br>List of Emplyee's Filtered by position " & Request.QueryString("position") & " " & active
                If Request.QueryString("active") <> "" Then
                    sql = "select * from emprec where id in(select emptid from emp_job_assign where position='" & Request.QueryString("position") & "' and date_end is null ) and active='" & Request.QueryString("active") & "' order by id desc"
                Else
                    sql = "select * from emprec where id in(select emptid from emp_job_assign where position='" & Request.QueryString("position") & "' and date_end is null )  order by id desc"
                End If
                If sql <> "" Then
                    out = makeformx(sql)
                    'Response.Write(sql)
                End If
            Case "pp"
                '45 days workers
                sql = "select * from emprec where dateadd(d,45,hire_date)>'" & Today.ToShortDateString & "'"
                Response.Write(sql)
                out = makeformx(sql)

               
        End Select
    End If
    ' fld = Nothing
    
    %>
  <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('allList','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico'/>print</div>        
    <div id="allList">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 888pt; border-collapse: collapse; height: 76px;">
            <colgroup>
                <col style="width: 16pt; mso-width-source: userset; mso-width-alt: 768" width="21" />
                <col style="width: 95pt; mso-width-source: userset; mso-width-alt: 4608" width="126" />
                <col style="width: 20pt; mso-width-source: userset; mso-width-alt: 987" width="27" />
                <col style="width: 68p ; mso-width-source: userset; mso-width-alt: 3291" width="90" />
                <col style="width: 56pt; mso-width-source: userset; mso-width-alt: 2742" width="75" />
                <col span="3" style="width: 48pt" width="64" />
                <col style="width: 42pt; mso-width-source: userset; mso-width-alt: 2048" width="56" />
                <col style="width: 32pt; mso-width-source: userset; mso-width-alt: 1536" width="42" />
                <col style="width: 32pt; mso-width-source: userset; mso-width-alt: 1572" width="43" />
                <col style="width: 34pt; mso-width-source: userset; mso-width-alt: 1645" width="45" />
                <col style="width: 35pt; mso-width-source: userset; mso-width-alt: 1682" width="46" />
                <col style="width: 48pt" width="64" />
            </colgroup>
            <tr><td colspan="15" style="text-align:center;font-size:16pt; color:Blue"><%response.write(title) %></td></tr>
            <tr  style="height: 15.75pt; font-size:12pt; color:#020202; font-weight:bold;background-color: #367898">
                <td class="xl67" height="63" rowspan="2" style="border-right: windowtext 1pt solid;
                    border-top: windowtext 1pt solid; border-left: windowtext 1pt solid; width: 16pt;
                    border-bottom: black 1pt solid; height: 47.25pt;">
                    No
                    </td>
                <td class="xl67" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 231pt;
                    border-bottom: black 1pt solid;  font-family:Times New Roman; ">
                   
                    Full name</td>
                <td class="xl67" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 34pt;
                    border-bottom: black 1pt solid;  font-family:Times New Roman; ">
                    <span style="font-size: 10pt">
                    sex</span></td>
                <td class="xl67" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 137pt;
                    border-bottom: black 1pt solid;  font-family:Times New Roman; ">
                    Employment date</td>
                <td class="xl67" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 119pt;
                    border-bottom: black 1pt solid;  font-family:Times New Roman; ">
                    Field of study</td>
                <td class="xl67" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 44pt;
                    border-bottom: black 1pt solid;  font-family:Times New Roman; ">
                    Certificate</td>
                <td class="xl67" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext; width: 47pt;  border-bottom: black 1pt solid;
                     font-family:Times New Roman; " rowspan="2">
                    &nbsp;Year of Graduation</td>
                <td class="xl67" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext; width: 106pt; border-bottom: black 1pt solid;
                     font-family:Times New Roman; " rowspan="2">
                    &nbsp;College/
                    Univeristy</td>
                <td class="xl67" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 79pt;
                    border-bottom: black 1pt solid;  font-family:Times New Roman; ">
                    Position</td>
                <td class="xl67" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 50pt;
                    border-bottom: black 1pt solid;  font-family:Times New Roman; ">
                    Salary</td>
                <td class="xl69" colspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext; width: 66pt; border-bottom: windowtext 1pt solid;
                     font-family:Times New Roman; " width="88">
                    Allowance</td>
                <td class="xl69" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 35pt;
                    border-bottom: windowtext 1pt solid;  font-family:Times New Roman; "
                    width="46">
                    Perdiem</td>
                <td class="xl69" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 85pt;
                    border-bottom: windowtext 1pt solid; font-family:Times New Roman; ">
                    project</td>
                <td class="xl69" rowspan="2" style="border-right: windowtext 1pt solid; border-top: windowtext 1pt solid;
                    font-weight: bold;  border-left: windowtext 1pt solid; width: 90pt;
                    border-bottom: windowtext 1pt solid;  font-family:Times New Roman; ">
                    Tel/Mob</td>
            </tr>
            <tr height="42" style="font-weight: bold;   font-family:Times New Roman; height: 31.5pt;
                mso-height-source: userset;font-size:12pt; color:#020202; font-weight:bold;background-color: #367898">
                <td class="xl64" style="border-right: windowtext 1pt solid; border-top: windowtext;
                    border-left: windowtext; width: 42pt; border-bottom: windowtext 1pt solid; height: 31pt;
                    ">
                    Non
                    <br />
                    Taxable</td>
                <td class="xl64" style="border-right: windowtext 1pt solid; border-top: windowtext;
                    border-left: windowtext; width: 44pt; border-bottom: windowtext 1pt solid; height: 31pt;
                    ">
                    Taxable</td>
            </tr>
           <% response.write(out) %>
        </tr></table>
    <table><tr><td width="15" style="height:14px; background-color:Red">&nbsp;</td><td>None Active Employees</td></tr></table>
    </div>
        
  
</body>
</html>
