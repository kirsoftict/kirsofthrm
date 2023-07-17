<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empidmake.aspx.vb" Inherits="empidmake" %>

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
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
<script type="text/javascript" src="scripts/kirsoft.java.js"></script>

<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
	<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/mkid.css" />
    <script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
function printpv()
{
window.print();
}
function printx(loc,title,head,footer)
    { 
  
    var printFriendly = document.getElementById(loc);
    var printcode=document.getElementById('code1');
    var printstyle=document.getElementById('style1');
    var printWin = window.open("printview.htm",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write('<html><head><title>' + head +'</title>' + printcode.innerHTML +"<style>"+ printstyle.innerHTML + '</style></head><body>' + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
   // printWin.close();
    }


</script>

     
     <%	       
         Dim fm As New formMaker
         Dim namelist As String = ""
         
      namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
 %>
  <script language="javascript" type="text/javascript">
    var namelist=[<% response.write(namelist) %>];
     function searchv(val)
        {var str=""
           
                // alert("byname");
               str=$("#frmsbyname").formSerialize();
              // alert(str);
   $("#frmsbyname").attr("action","?val=namex&" + str);
    $("#frmsbyname").attr("target","frm_tar");
    $("#frmsbyname").submit();
               
              }
    </script>
</head>

<body>

<%  If Request.QueryString("vname") = "" Then%>
<form id='frmsbyname' method='post' action=''>
    <table cellspacing='0px' cellpadding='0px'><tr><td>
    <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" /><br/>
    <label class='lblsmall'>write name</label></td>
    <td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick="searchv('byname')" />
    </td></tr></table></form>
    <%End If%>
   
    <div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:print('print','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','mkid');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>   
   <%If Request.QueryString("val") <> "" Then%>     
   <div style="width: 5px; float: left;">
         <form id="export" name="export" action="" method="post" target="_blank" style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="word-idmaker-<% response.write(now.tostring) %>" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="word" />
        <input type="hidden" name="rigt" id="rigt" value="" />
    
</form> <div class="fileexp" style=" border:none; width:150px;height:28px; 
            background:url(images/blue_banner-760x147x.jpg) #224488;color:White; 
            cursor:pointer; display:none;" onclick="javascript://export();" >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="Excel" /> Export to Excel</div>
       
          
        </div> 
   <div id='print'>
      <div id='book' class="book">
    <div class="page">
        <div class="subpage">
       <asp:Literal ID="idview" runat="server"></asp:Literal> 
      <center><% Response.Write(idtebl)%>  </center>      
    </div> 
    </div> 
      <div class="page">
        <div class="subpage">
       
      <center><% Response.Write(backpage)%>  </center>      
    </div> 
    </div> 
     </div></div>
    <%
      
    Dim obj As Object
           
    Dim loc As String = Server.MapPath("download") & "\lastid.txt"
    loc = loc.Replace("\", "/")
    'Response.Write(loc)
    
        
               Dim cod As String = idtebl()
              ' Response.Write(cod)
               If String.IsNullOrEmpty(cod) = False Then
                   ' cod = sec.StrToHex(cod)
                   ' Response.Write(cod)
                   obj = cod
                   obj = "1;2;3" & Chr(13) & obj
                   File.WriteAllText(loc, obj)
            Response.Write("<script>$('.fileexp').css({display:'inline'});")
           %>
            $("#filetype").val("doc");
                $("#fileloc").val("<% response.write(loc) %>");
                $("#filename").val("<% response.write("id-export-" & now.tostring) %>");
                $("#rigt").val("1;2;3");
                </script>
           <%
               End If
                
           End If%>
            
    
</body>
</html>
