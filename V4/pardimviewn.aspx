<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pardimviewn.aspx.vb" Inherits="pardimviewn" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>

<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>

<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
<title><%  Response.Write(Session("company_name"))
           Session.Timeout = 60
           %></title>
<link rel="stylesheet" href="css/print600.css" />
<script language='javascript' type='text/javascript' src='jqq/jquery-1.9.1.js'></script>
            <script src='jqq/ui/jquery.ui.core.js'></script>
            <script src='jqq/ui/jquery.ui.core.js'></script><script src='jqq/ui/jquery.ui.widget.js'></script>
<script type="text/javascript" src="scripts/kirsoft.java.js"></script>
<script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
function printpv()
{
window.print();
}
function printx(loc,title,head,footer)
    { 
  
    var printFriendly = document.getElementById(loc);
    var printWin = window.open("printview.htm",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write('<html><head><title>' + head +"</title><link rel='stylesheet' href='css/printA4.css' /></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
 
   printWin.document.open();
    printWin.window.print();   
   // printWin.close();
    }

   
</script>


</head>

<body>
<% Dim fm As New formMaker
    'Response.Write(fm.helpback(False, True, False, False, "", ""))
    Dim loc As String = Server.MapPath("download") & "\perdim(" & Now.Month.ToString & Now.Day.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & ").txt"
    loc = loc.Replace("\", "/")
    %>



    <input type="text" id="txtx" name="txtx" style='display:none;' />

    <div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('print','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','print600');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>    
       <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form>
        <div class="clickexp" style=" float:left; border:none; width:150px;height:28px; 
            background:url(images/blue_banner-760x147x.jpg) #224488;color:White; 
            cursor:pointer; " onclick='javascript:exportx("perdiem(<%response.write(now.day.tostring & now.month.tostring & now.year.tostring) %>)","doc","<%response.write(loc) %>","export","2;3");' >
    <img src="images/gif/msword.gif" height="28px" style="float:left;" alt="excel" /> Export to Word</div>
  
      <br /><br />
      <div id="print">
   <div id='book' class="book">
  <%Dim rtnv(2) As String
      rtnv(0) = ""
      rtnv(1) = ""
         Dim expcss As String
      If Request.QueryString("part") = "p2" Then%>
 
    <div class="page" id='page'>
        <div class="subpage">Page 1/1

       <%  
           rtnv = outpx(Request.QueryString("s"))
           Response.Write(rtnv(0))
           ' Response.Write(loc)
          
           expcss = "<style  type='text/css'>" & _
           ".tb1" & _
  "{" & _
   " margin-right:auto;" & _
    "margin-left:1.5cm;" & _
   " height:21.7cm;" & _
  "}" & _
   "        .tb1 td" & _
   "{" & _
  " 	font-size:12pt;" & _
 "border: 1px #000 solid;" & _
  " 	}" & _
   " .headerp {" & _
 "text-align: center;" & _
 "font-weight: bold;" & _
"}" & _
".tcenter {" & _
" text-align: right;" & _
" font-weight: bold;" & _
"}" & _
"#tblneed" & _
"{" & _
 "border: 1px solid #000;" & _
 " margin-left:auto;" & _
"margin-right:auto;" & _
 "}" & _
 "#tblneed td" & _
 "{" & _
 "border-right: 1px solid #000;" & _
"	border-bottom: 1px solid #000;" & _
 " }" & _
 "  </style>"
           Try
               File.WriteAllText(loc, expcss & rtnv(0))
           Catch ex As Exception
Response.Write(ex.ToString)
           End Try
           
      
           
          %>         
    </div> 
    </div>
 
 <%
 
      ElseIf Request.QueryString("part") = "p1" Then
     rtnv = outpall(Request.Form("txtprint"))
  
     expcss = "<style  type='text/css'>" & _
     ".tb1" & _
"{" & _
" margin-right:auto;" & _
"margin-left:1.5cm;" & _
" height:21.7cm;" & _
"}" & _
"        .tb1 td" & _
"{" & _
" 	font-size:12pt;" & _
"border: 1px #000 solid;" & _
" 	}" & _
" .headerp {" & _
"text-align: center;" & _
"font-weight: bold;" & _
"}" & _
".tcenter {" & _
" text-align: right;" & _
" font-weight: bold;" & _
"}" & _
"#tblneed" & _
"{" & _
"border: 1px solid #000;" & _
" margin-left:auto;" & _
"margin-right:auto;" & _
"}" & _
"#tblneed td" & _
"{" & _
"border-right: 1px solid #000;" & _
"	border-bottom: 1px solid #000;" & _
" }" & _
"  </style>"
     Try
         File.WriteAllText(loc, expcss & rtnv(0))
     Catch ex As Exception
         Response.Write(ex.ToString)
     End Try
   End If
    %>
</div>
</div>
 
             <span style='vertical-align:middle; height:34px; position:fixed; cursor:pointer' onclick='javascript:printpv();'><img src='images/ico/print.ico' alt='print'/>Print</span>

<span style="vertical-align:baseline; position:relative"></span>
       <% 'Response.Write("<script>$(document).ready(function() {" & rtnv(1) & " });</script>")%>
</body>
</html>
