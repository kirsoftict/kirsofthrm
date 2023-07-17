<%@ Page Language="VB" AutoEventWireup="false" CodeFile="msgbox.aspx.vb" Inherits="scripts_msgbox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 82px;
            height: 36px;
        }
        .style3
        {
            height: 36px;
        }
    </style>
</head>
<body>
   
    <%msg(Request.QueryString("titlex"),Request.QueryString("msg"),Request.QueryString("msgtype"))%>
    </body>
</html>
