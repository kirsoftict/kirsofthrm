﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ISOHRM.aspx.vb" Inherits="ISOHRM" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="Microsoft.VisualBasic"%>
<%
    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim fl As New file_list
    Dim sec As New k_security
    Dim t1, t2 As Date
    t1 = Now
    Dim url As String
    url = Request.ServerVariables("HTTP_HOST")
    
'Response.Write(url)
    ' Response.AppendHeader("Content-Disposition", "attachment; filename=Error.xls")
    'Response.ContentType = "application/ms-excel"
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><link href="images/kir.ico" rel="shortcut icon" />
    <title></title>
<link rel="stylesheet" href="http://<%=url %>/jqq/themes/base/jquery.ui.all.css" />
<meta http-equiv='cache-control' content='no-cache' />
<meta http-equiv='expires' content='0' />
<meta http-equiv='pargma' content='co-cache' />
<link rel="stylesheet" href="http://<%=url %>/jq/themes/ui-lightness/jquery.ui.all.css" />
	<script type="text/javascript" src="http://<%=url %>/jqq/jquery-1.9.1.js "></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.progressbar.js"></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.datepicker.js"></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.button.js"></script>
	<script type="text/javascript" src="http://<%=url %>/jqq/ui/jquery.ui.dialog.js"></script>
	 
       
   
	<link rel="stylesheet" href="http://<%=url %>/jq/demos.css">
	<script type="text/javascript" src="http://<%=url %>/scripts/form.js"></script>



<script src="http://<%=url %>/scripts/script.js" type="text/javascript"></script>


   <script language="javascript" type="text/javascript" src="http://<%=url %>/scripts/kirsoft.iso.js"></script> 
            <script type="text/javascript" src="http://<%=url %>/chart/sources/jscharts.js"></script>
            
             <link href="http://<%=url %>/css/iso.css" media="screen" rel="Stylesheet" type="text/css" />
            
            <%
          
 
                'Response.Write(Request.Browser())
                
	   ' namelist = getjavalist2("tblproject", "project_name,project_id", Session("con"), "|", "$")
	

 %>
 <script type="text/javascript">
 var month;
 var year;
 var isotype;
 isotype="";
 pd1="";
 pd2="";
 month="";
 year="";
// var stime=Date.now();
<% if isdate(Request.QueryString("pd1")) then
response.write("var pd1=" & chr(34) & request.querystring("pd1") & chr(34) & ";")
response.write("var pd2=" & chr(34) & request.querystring("pd2") & chr(34) & ";")
response.write("var isotype=" & chr(34) & (request.querystring("isotype")) & chr(34) & ";")
response.write(" month=" & chr(34) & cdate(request.querystring("pd1")).month & chr(34) & ";")
response.write(" year=" & chr(34) & cdate(request.querystring("pd1")).year & chr(34) & ";")
else if isdate(Request.form("pd1")) then
response.write("var pd1=" & chr(34) & request.form("pd1") & chr(34) & ";")
response.write("var pd2=" & chr(34) & request.form("pd2") & chr(34) & ";")
response.write("var isotype=" & chr(34) & (request.form("isotype")) & chr(34) & ";")
response.write("month=" & chr(34) & cdate(request.form("pd1")).month & chr(34) & ";")
response.write("year=" & chr(34) & cdate(request.form("pd1")).year & chr(34) & ";")
end if 


%>

  
   
  function datechk()
  {
    if($("#paydx").val()!==$("#ppdate").val())
    {
        alert("Payroll date and Pay date is not similar");
    }
  }
 
 function checkedc(lac) {

            if ($("#" + lac).is(':checked')) 
                {
                    //$("#chkall").text("Checked All");

                   // alert("chhhh");
                    $(".csssign").text(" ");

                }
               
               
            }
 
        function submitiso()
        {
         $('#frmlistout').attr("action", "?isotype="+isotype+"&pd1=" + pd1 + "&pd2=" + pd2 );
                  
         $("#frmlistout").submit();
       
        }
    </script>
   
    

	 

	 <script language="javascript" type="text/javascript">
     function refresh()
     {
    // location.reload();
     }
     $(document).ready(function () {
         // alert("<% response.write(request.querystring("chkpen")) %>");
         /*alert(month + 'and' +year);
         alert($("#p1").outerHeight());
         alert($("#p1").height());
         alert($("#p1").innerHeight());
         alert($("#subp1").outerHeight());
         alert($("#subp1").height());
         alert($("#subp1").innerHeight());*/
         if(isNaN(<%=pgno %>))
         {
            alert("<%=pgno %>....................");
         }
         var subclass=$(".subpage");
         var mpage=$(".page");
         var smak;
         var addsum=1;
         var maxs=$("#bigprintx").innerHeight();
   //  alert(maxs);
         if(pd1!==""){
         for(i=0;i<mpage.length;i++)
         {
         addsum=1;
         // alert(i + "==>" + parseInt(mpage.eq(i).innerHeight()) + "<" + parseInt(subclass.eq(i).innerHeight()));
            if(parseInt(mpage.eq(i).innerHeight())<parseInt(subclass.eq(i).innerHeight()))
            {

          smak=parseInt(subclass.eq(i).innerHeight())-parseInt(mpage.eq(i).innerHeight())
          smak=(parseInt(parseInt(smak)/20));
           if(smak>0)
           {
            addsum=smak-1;
           // alert(smak);
           }
                <% if request.querystring("pagesize")<>"" then %>
              $("#pagesize").val('<%=request.querystring("pagesize") %>' - addsum);
               <% elseif request.Form("pagesize")<>"" then %>
                $("#pagesize").val('<%=request.form("pagesize") %>' - addsum);
             <% end if %>
             alert($("#pagesize").val());
            
           
             $("#pd1").val('<%=request.form("pd1") %>');
             $("#pd2").val('<%=request.form("pd2") %>');
             $("#isotype").val('<%=request.form("isotype") %>');

             $('#frmlistout').attr("action", "?isotype=" + isotype + "&pd1=" + pd1 + "&pd2=" + pd2);

           //  $("#frmlistout").submit();  
            if(addsum>1)
                break;
           
         }
         else
         {
          $("#pper").val($("#pper").val() + ',' + $("#pagesize").val());
      //   alert($("#pper").val());
         }
   
         }
      }
         // $("#ppaytop").css({display:'inline'});
         //  showobj("bigprint","cover",0,0);
         //   $("#cover").css({hieght:findh(document.getElementById("bigprint"))+"px",width:"100px"});
         // alert(findh(document.getElementById("bigprint")));
     });
    
</script>
<style>

</style>
</head>

<body>

 <% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

       
    
         <%  
             ' Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
           '  Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
<div id='tim' style='left:0px;top:0px; height:20px; position:fixed;'></div>
<form id='frmx' name='frmx' method="post" style='display:none'><input type="text" name="txtpass" /></form>
  
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px; z-index:1000; position:relative;">
HRM ISO Report
    <form name="frmlistout" id="frmlistout" action="" method="post" style="font-size:12pt;">
        Date Gap: 
        <input type="text" id='pd1' name='pd1' value='<% response.write(today.addmonths(-1).toshortdatestring) %>' /> to 
        <input type="text" id='pd2' name='pd2' value='<% response.write(today.toshortdatestring) %>' />
        <script language="javascript" type="text/javascript">
           
          
 dateafteronly('pd1', 'pd2');
        </script>
   
        <%  ' If Request.Item("pd1") <> "" Then
            'Response.Write(Request.Item("pd1"))
            '  End If %>
        <input type="hidden" name="projname" id="projname" value='<%=namelist %>' />
          Report Type<select id="isotype" name="isotype">
          <option value="All">All</option>
                 <option value="Hired">Hired</option>
       <option value="Termination">Termination</option>
      <option value="Promotion"> Promotion</option>
      <option value="Transfer">Transfer</option>
       <option value="active">Active</option>
 <option value="Vacancy">Vacancy</option>
         </select>
         <input type="hidden" id="pagesize" name="pagesize" value="20" />
         <input type="hidden" id="pper" name="pper" value="P1:" />
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submitiso();" />
    </form>
    </div>     
    <form id="export" name="export" action="" method="post"  style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
      <input type="hidden" name="cssexport" id="cssexport" value="exportword.css" />
      <input type="hidden" name="cssprint" id="cssprint" value="exportword.css" />
</form> 

     <div>   <%  If Request.Form("pd1") <> "" Or Request.QueryString("pd1") <> "" Then
                     'Response.Write("In")
                     init()
                 Else
                     allr()
                 End If
                 'exportword()
                 For Each x As String In Request.ServerVariables
                     '    Response.Write(x & "==" & Request.ServerVariables(x) & "<br>")
                 Next
                 %>
            </div>
    
         
 <%        
   
    t2 = Now
    Dim timout As String = ""
    timout = t2.Subtract(t1).Minutes & "Mins " & t2.Subtract(t1).Seconds & "Secs"
   
    %>
   

<script type="text/jscript">
$("#tim").text("<% response.write("Loading Time: " & timout) %>");
$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();

</script>
<div style="background:YELLOW;">

  
 
    </div>

</body>
</html>


