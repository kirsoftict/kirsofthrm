<%@ Page Language="VB" AutoEventWireup="false" CodeFile="deletefile.aspx.vb" Inherits="deletefile" %>
<%@ Import Namespace="kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
<script type="text/javascript">
alert("deleee");</script>
   <%  If Request.QueryString.HasKeys = True Then
           If Request.QueryString("delete") = "true" Then
               Dim up As New file_list
               Dim str As String
               str = up.deletefile(Request.QueryString("path"))
               Response.Write(str)
           End If
       End If
       Response.Redirect("home.aspx")
       %>
</body>
</html>
