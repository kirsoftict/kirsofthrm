<%@ Page Language="VB" AutoEventWireup="false" CodeFile="nokia.aspx.vb" Inherits="nokia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <% 'mailthis("zkirubel@gmail.com", "z.kirubel@gmail.com", "hi", "hi this is test msg")
        nokia_sms()
        %>
    <asp:TextBox ID=textbox1 runat=server></asp:TextBox>
    </div>
    </form>
</body>
</html>
