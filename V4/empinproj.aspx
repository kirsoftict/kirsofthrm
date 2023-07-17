<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empinproj.aspx.vb" Inherits="empinproj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<div style="width:100%; background:#6879aa; text-align:center;color:White; font-size:19pt;">View No. Employess in the each active project

    <form name="frmlistout" action="empinproj.aspx" method="post" style="font-size:12pt;">
       
        
               <select id="month" name="month">
            <%  Dim i As Integer
                For i = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
        
         <select id="year" name="year">
            <%  For i = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
        Increment/Decrement in Selected month: <input type="checkbox" value="thismonth" id="thism" name="thism" />
         <input type="submit" value="submit" />
    </form>
    </div>
    
    <div>
        <% If Request.Form("year") <> "" Then
                maketable(Request.Form("year"), Request.Form("month"))
            Else
                maketable(Today.Year.ToString, Today.Month.ToString)
            End If%>
    </div>
    
</body>
</html>
