<%@ Page Language="VB" AutoEventWireup="false" CodeFile="viewreport1-copy.aspx.vb" Inherits="viewreportcopy1" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css"/>
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
<script type="text/javascript" src="scripts/form.js"></script>
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  
	<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
     function print(loc,title,head,footer,st)
    { //Response.AppendHeader("Content-Disposition", "attachment; filename='c:\temp\Error.xls'")
    //Response.ContentType = "application/ms-excel"
    
    var printstyle=document.getElementById(st)
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("print.htm",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<html><head><title>" + head + "</title><style>" + printstyle.innerHTML + "</style></head><body>" + printFriendly.innerHTML + "</body></html>" );
    printWin.document.close();
  printWin.window.print();   

  // printWin.close();
    }
    function printy(loc,title,head,footer)
    { 
    var ysno=confirm("please change the paper layout to landscape");
    
    if(ysno==true){
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("about:blank",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write("<html><head><title>" + head +"</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
    printWin.close();
    }
    
    }
    function searchv(val)
        {var str=""
            switch(val)
            {
                case "alls":
                     alert("all called");
                break;
                case "byname":
                // alert("byname");
               str=$("#frmsbyname").formSerialize();
              // alert(str);
   $("#frmsbyname").attr("action","?val=namex&" + str);
    $("#frmsbyname").attr("target","frm_tar");
    $("#frmsbyname").submit();
                break;
                }
              }
    </script>
</head>
<body>
<div style="top:0px;width:100%; height:25px; padding:0px 0px 0px 0px; position :fixed;font-size:9pt;"> 
<div id="tim" style='vertical-align:top;top:0px; height:60px; position:relative; float:left'></div>
     <div style="float:right; width:300px; background:white; position:inherit">
      <div id="mmenu"> 
        <span style='color:Gray; cursor:pointer;' onclick="back('-1')">
                 <span class='hlpm'  style='color:Gray; cursor:pointer;' onclick="javascript:gotoinfo('<% Response.Write(request.servervariables("URL")) %>')">Help</span>
        </div> 
        
        </div>

</div>
	<% dim outp() as string
	    outp = getout()
	    ' Response.Write(outp(1))
    Dim fm As New formMaker
        
        Dim namelist As String = ""
        Dim dept As String = ""
        Dim proj As String
	    'dept = fm.getjavalist2("tbldepartment", "dep_id,dep_name", Session("con"), "| ")
	    ' Response.Write(outp(2))
	    
	    If outp(1) <> "" Then
	        namelist = fm.getjavalist22(outp(2), "emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	        ' Response.Write(fm.getjavalist22(outp(2), "emp_static_info", "first_name,middle_name,last_name", Session("con"), " ").ToString)
	    Else
	        namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
	        'Response.Write("kkkkk")

	    End If
	    proj = fm.getjavalist2("tblproject", "project_id,project_name", Session("con"), "| ")
	    Dim disc As String = fm.getjavalist2("tbldiscipline", "discipline", Session("con"), "")
	    Dim qual As String = fm.getjavalist2("tblqualification", "qualification", Session("con"), "")
	    Dim post As String = fm.getjavalist2("tblposition", "job_position", Session("con"), "")
        
        %>
    <script language="javascript" type="text/javascript">
    var namelist=[<% response.write(namelist) %>];
    var deplist=[<% response.write(dept) %>];
    var proj=[<% response.write(proj) %>];
     var disc=[<% response.write(disc) %>];
      var qual=[<% response.write(qual) %>];
      var posx=[<% response.write(post) %>];
</script>
<form id='frmsbyname' method='post' action=''>
    <table cellspacing='0px' cellpadding='0px'><tr><td>
    <input type='text' name='vname' id='vname' style='font-size:9pt;'  onkeyup="javascript:startwith('vname',namelist);" /><br/>
    <label class='lblsmall'>write name</label></td>
    <td valign='top'><input type='button' class='searchx' id='searchx' value='Go' name='searchx' onclick="searchv('byname')" />
    </td></tr></table></form>
    <div id="Div2"> 
                <span class='hlpm'  style='color:Gray; cursor:pointer;' onclick="javascript:gotoinfo('<% Response.Write(request.servervariables("URL")) %>')">Help</span>
        &nbsp;&nbsp;|&nbsp;&nbsp;<span id="Span1"  style="width:100px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('allList','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');"><img src='images/ico/print.ico' alt="print"/>print A4</span>&nbsp;&nbsp;|&nbsp;&nbsp;      
       <span id="Div1"  style="width:100px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('allList','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA3');"><img src='images/ico/print.ico' alt="print"/>print A3</span>       
        </div> 
<style id='printA3'>
            #tb1 
            {
                width:1400px;
                border:1px solid black;
                font-size:9pt;
                           }
                             .toptitle
            {
                font-size:16pt;
            }
          #tb1 td
            {
             border-top: 1px solid black;
             border-left:1px solid black;
             
                font-size:9pt;
            }
              .fxname
            {
                
                width:250px;
               
            }
            .fitx
            {
           
                width:55px;
               
            }
           .dw
            {
            
                width:30px;
                
            }
            .tel
            {
                width:90px;
            }
           .projectx
            {
                width:80px;
            }
            .cell
            {
            height:45px;
               border-top: 1px solid black;
             border-left:1px solid black;
             
             border-right: 1px solid black;
             border-bottom:1px solid black;
            }
          
            </style>
            <style id='printA4'>
            #bigprint
            {
                width:800px;
                font-size:6.8px;
            }
            #tb1 
            {
                font-size:6.4pt;
                margin-left:0px;
                width:900px;
                border:1px solid black;
                
                           }
          #tb1 td
            {
             border-top: 1px solid black;
             border-left:1px solid black;
             
                font-size:6.5pt;
            }
                .fxname
            {
                
                width:150px;
               
            }
            
            .tel
            {
                width:90px;
            }
            
            .fitx
            {
           
                width:15px;
               
            }
           .dw
            {
            
                width:10px;
                
            }
            .projectx
            {
                width:80px;
            }
            .toptitle
            {
                font-size:12pt;
            }
            .cell
            {
            height:30px;
               border-top: 1px solid black;
             border-left:1px solid black;
             
             border-right: 1px solid black;
             border-bottom:1px solid black;
            }
          
            </style>
            <style>
             #bigprint
            {
                width:1400px;
                font-size:9px;
            }
            #tb1 
            {
                font-size:9pt;
                margin-left:15px;
                width:1300px;
                border:1px solid black;
               
                           }
          #tb1 td
            {
         
               
                font-size:9pt;
                
            } 
             .fxname
            {
                
                width:200px;
               
            }
            .fitx
            {
           
                width:50px;
               
            }
             .projectx
            {
                width:110px;
            }
           .dw
            {
            
                width:30px;
                
            }
            .tel
            {
                width:90px;
            }
            
            .cell
            {
            width:auto;
            height:45px;
               border-top: 1px solid black;
             border-left:1px solid black;
             
             border-right: 1px solid black;
             border-bottom:1px solid black;
            }
          </style>

 <div style="float:left; width:5px;"></div>

  <div id="print"  style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;float:left" onclick="javascirpt:printy('allList','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico' alt="print"/>print</div>        
  <div style="float:left; width:5px;"></div>
   <%
        
    Dim obj As Object
       obj = outp(1)
       Dim loc As String = Server.MapPath("download") & "\contractemp.txt"
    obj = "1;2;3" & Chr(13) & obj
    File.WriteAllText(loc, obj)
    
    
       If outp(1) <> "" Then%>
        <div id='expxls' style="float:left; width:250px;">
<form id="exportexcel" name="exprtexcel" action="print.aspx?pagename=viewrpt-<% response.write(request.querystring("val")) %>" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="from file" />
    <input type="hidden" name="loc" id="loc" value="<% response.write(loc) %>" />
    <input type="hidden" name="right" id="right" value="<% response.write(session("right")) %>" />
    <div style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick="javascript:$('#exportexcel').submit();" >
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="Excel" /> Export to Excel</div>
</form></div> <div style="clear:both"></div>
  <% End If  %>    <div id="allList">
       
           <% Response.Write(outp(1))%>
        
   
    </div>
        
  
</body>
</html>
