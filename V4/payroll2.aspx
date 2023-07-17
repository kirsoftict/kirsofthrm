<%@ Page Language="VB" AutoEventWireup="false" CodeFile="payroll2.aspx.vb" Inherits="payroll2" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>

    <title>Untitled Page</title>
</head>
<body>
    <div>
    <script type="text/javascript">
  var s = window.location.href;
//document.write(s);
    
//document.write(document.referrer);
    </script>
        <%  'proces2()
            If Session("con").state = ConnectionState.Closed Then
                Session("con").open()
            End If
            If Session("username") <> "" Then
                processnew() 'proces()
            Else
                Response.Write("data is not saved! please relogin")
                
            End If
           
            %>
    </div>
</body>
</html>
