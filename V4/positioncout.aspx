<%@ Page Language="VB" AutoEventWireup="false" CodeFile="positioncout.aspx.vb" Inherits="positioncout" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.IO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%  Dim sec As New k_security%>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Untitled Page</title>
    <link rel="stylesheet" href="css/printsetup.css">
    <link rel="stylesheet" href="css/kir.login.css">
    <script src="jqq/jquery-1.9.1.js"></script>
<script>
function printpv()
{
window.print();
}
</script>
</head>
<body>
 <div style='vertical-align:middle; height:34px; position:relative; float:left; cursor:pointer' onclick='javascript:printpv();'><img src='images/ico/print.ico' alt='print'/>Print</div>


    <div style="float:left; width:5px;"></div>
   <%
        
    Dim obj As Object
       obj = Me.outp.Text
       Dim loc As String = Server.MapPath("download") & "\operatorview.txt"
    obj = "1;2;3" & Chr(13) & obj
    File.WriteAllText(loc, obj)
    
    
       If Me.outp.Text <> "" Then%>
        <div id='expxls' style="float:left; width:250px;">
<form id="exportexcel" name="exprtexcel" action="print.aspx?pagename=viewrpt-<% response.write(request.querystring("val")) %>" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="from file" />
    <input type="hidden" name="loc" id="loc" value="<% response.write(loc) %>" />
    <input type="hidden" name="right" id="right" value="<% response.write(session("right")) %>" />
    <div style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick="javascript:$('#exportexcel').submit();" >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="Excel" /> Export to Excel</div>
</form></div> <div style="clear:both"></div>
  <% End If  %> 
     <div id='book' class="book">
     <div class="page">
        <div class="subpage">
    <asp:Literal ID="outp" runat="server"></asp:Literal>
    </div>
    </div>
    </div>
                
</body>
</html>
