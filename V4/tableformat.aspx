<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tableformat.aspx.vb" Inherits="tableformat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Untitled Page</title>
 
 
</head>
<body>
    
   
    
    <%  Session.Timeout = "60"
        Dim t1, t2 As Date
        t1 = Now
        If Request.Form("do") = "table" Then
            mkrow()
        ElseIf Request.Form("do") = "pagger" Then
            pagger()
        End If
        
        t2 = Now
        Dim timout As String = ""
        timout = t2.Subtract(t1).Minutes & " Mins " & t2.Subtract(t1).Seconds & "Secs"
        %>
   
   
   
</body>
</html>
