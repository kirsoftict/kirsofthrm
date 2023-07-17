<%@ Page Language="VB" AutoEventWireup="false" CodeFile="errorfind.aspx.vb" Inherits="errorfind" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Untitled Page</title>
</head>
<body style='background:#ededed;'>
    <% me.salary()
        ' me.pardim()
        ' me.allowance()
        ' me.jobassign() %>
        <table>
        <tr><td>Leave Notice...</td></tr>
        <tr><td><%leaveerror() %></td></tr>
        </table>
        <div>
        <% leave_b_c()%>
        </div>
</body>
</html>
