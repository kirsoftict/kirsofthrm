<%@ Page Language="VB" AutoEventWireup="false" CodeFile="exporttoexcel.aspx.vb" Inherits="exporttoexcel" Debug="true" %>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="kirsoft.hrm"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<% 
    '  Response.AppendHeader("Content-Disposition", "attachment; filename=Error.xls")
    ' Response.ContentType = "application/ms-excel"
    'Response.ContentType = "application/vnd.ms-excel"%>

          
    <html>
    <head>
    <title></title></head>
    <body>
     <%
       Dim sec As New k_security
       
             Response.Write(Request.Form("loc") & "<br>" & (Request.Form("datatra")) & "<br>" & Request.Form("css"))
       '    File.WriteAllText(Request.Form("loc"), Request.Form("css") & sec.dbHexToStr(Request.Form("data")))
         %>
    </body></html>
  
  