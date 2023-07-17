<%@ Page Language="VB" AutoEventWireup="false" CodeFile="activeemp.aspx.vb" Inherits="activeemp" %>
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
    
    ' Response.AppendHeader("Content-Disposition", "attachment; filename=Error.xls")
    'Response.ContentType = "application/ms-excel"
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><link href="images/kir.ico" rel="shortcut icon" />
    <title></title>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />
<meta http-equiv-'cache-control' content='no-cache' />
<meta http-equiv='expires' content='0' />
<meta http-equiv='pargma' content'co-cache' />
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script type="text/javascript" src="jqq/jquery-1.9.1.js "></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.menu.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.button.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.dialog.js"></script>
	 
       
   
	<link rel="stylesheet" href="jq/demos.css">
	<script type="text/javascript" src="scripts/form.js"></script>



<script src="scripts/script.js" type="text/javascript"></script>


   <script language="javascript" type="text/javascript" src="scripts/kirsoft.iso.js"></script> 
            <script type="text/javascript" src="chart/sources/jscharts.js"></script>
            
             <link href="css/iso.css" media="screen" rel="Stylesheet" type="text/css" />
            
            <%
          
 
		   
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
     // alert(maxs);
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
 body {
    margin: 0;
    padding: 0;
    background-color: #FAFAFA;
    font: 12pt "Tahoma";
}
* {
    box-sizing: border-box;
    -moz-box-sizing: border-box;
}
.page {
    width: 29.7cm;
    max-height: 21cm;
    padding: 0.5cm;
    margin: 0.5cm auto;
    border: 1px #D3D3D3 solid;
    border-radius: 5px;
    background: white;
    box-shadow: 0 0 2px rgba(0, 0, 0, 0.1);
}
.subpage {
    padding: 0px;
    border: 5px red solid;
    height: 18cm;
    outline: 4px #FFEAEA solid;
}
.table_title{
	width: 100%;
	font-size: 17px;
	text-align: center;
	padding-top: 4px;
}
table.data, table.sub_data{
	width: 100%;
	border-width: 1px;
	border-collapse: collapse;
}
table.data tr th{
	background-color: #CCC;
	height: 30px;
}
table.data tr td{
	padding: 1px;
}
div.label{
	float: left;
	padding-right: 40px;
}
input.subtle_box{
	font-size: 14px;
	border: 0px solid #333;
	height: 18px;
	width: 100%;
	text-align: left;
	margin-top: 1px;
	margin-bottom: 1px;
}
input.underlined_box{
	font-size: 14px;
	border: 0px solid #333;
	border-bottom: 1px solid #666;
	height: 18px;
	text-align: left;
	margin-top: 1px;
	margin-bottom: 1px;
}
input.underlined_box_dotted{
	font-size: 14px;
	border: 0px solid #333;
	border-bottom: 1px dotted #666;
	height: 18px;
	text-align: left;
	margin-top: 1px;
	margin-bottom: 1px;
}
span.print{
	font-size: 14px;
	border: 0px solid #333;
	/*border-bottom: 1px solid #666;*/
	height: 18px;
	text-align: left;
	margin-top: 1px;
	margin-bottom: 1px;
	display: none;
}
select.screen{
	border: none;
}
input.table_box_left{
	font-size: 13px;
	border: 0px solid #333;
	width: 100%;
	/*height: 23px;*/
	text-align: left;
	margin-top: 0px;
	margin-bottom: 0px;
}
input.table_box_right{
	font-size: 13px;
	border: 0px solid #333;
	width: 100%;
	/*height: 23px;*/
	text-align: right;
	margin-top: 0px;
	margin-bottom: 0px;
}
input.date{
	text-align: center;
	padding: 0px;
}
.form_footer{
	margin-top: 20px;
}
table.signature tr td span{
	border-top: 1px solid #333;
	padding-left: 30px;
	padding-right: 30px;
}
.stamp{
	padding: 8px;
	border: 1px dashed #999;
	text-align: center;
}
.underlined{
	display: inline;
	border-bottom: 1px solid #333;
	padding-left: 15px;
	padding-right: 15px;
}
.table2, .table3{
	line-height: 14px;
}
.table4{
	line-height: 12px;
}
.table1{
	line-height: 13px;
	width: 100%;
	border-width: 1px;
	border-collapse: collapse;
}
table.table1 tr td{
	padding: 1px;
}
@page {
    size: A4 landscape;
    margin: 0;
}
@media print 
{
  
	body { font-size: 10pt 
    {
		
        margin: 0;
        border: initial;
        border-radius: initial;
        width: initial;
        min-height: initial;
        box-shadow: initial;
        background: initial;
        page-break-after: always;
    }
}
</style>
</head>

<body>

 <% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

       
    
         <%  
             Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;width:400px; height:55px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
             Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
%>
<div id='tim' style='left:0px;top:0px; height:20px; position:inherit;'></div>
<form id='frmx' name='frmx' method="post" style='display:none'><input type="text" name="txtpass" /></form>
  
<div style="width:100%; height:90px; background:#6879aa; text-align:center;color:White; font-size:19pt; padding-top:10px; z-index:99999; position:relative;">
HRM ISO Report Active Employee as date of
    <form name="frmlistout" id="frmlistout" action="" method="post" style="font-size:12pt;">
       <input type="text" id='pd2' name='pd2' value='<% response.write(today.toshortdatestring) %>' />
        <script language="javascript" type="text/javascript">

            $("#pd2").datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                changeYear: true,
                numberOfMonths: 1
            });
           
            $("#pd2").datepicker('option', 'dateFormat', 'mm/dd/yy');
           
        </script>
   
        <%  ' If Request.Item("pd1") <> "" Then
            'Response.Write(Request.Item("pd1"))
            '  End If %>
        <input type="hidden" name="projname" id="projname" value='<%=namelist %>' />
         
         <input type="hidden" id="pagesize" name="pagesize" value="25" />
         <input type="hidden" id="pper" name="pper" value="P1:" />
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submitiso();" />
    </form>
    </div>
   
                     
      <div style="width:5px; float:left;"></div> 
     
    <form id="export" name="export" action="" method="post"  style='display:none;' >
    <input type="hidden" name="filename" id="filename" value="" />
    <input type="hidden" name="fileloc" id="fileloc" value="" />
       <input type="hidden" name="filetype" id="filetype" value="" />
        <input type="hidden" name="rigt" id="rigt" value="" />
      <input type="hidden" name="css" id="css" value="iso.css" />
</form> 

     <div>   <%  If Request.Form("pd2") <> "" Or Request.QueryString("pd2") <> "" Then
                     'Response.Write("In")
                     init()
                 Else
                     allr()
                 End If%>
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




