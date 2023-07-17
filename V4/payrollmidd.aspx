<%@ Page Language="VB" AutoEventWireup="false" CodeFile="payrollmidd.aspx.vb" Inherits="payrollx" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%
    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim fl As New file_list
    Dim sec As New k_security
    ' Response.AppendHeader("Content-Disposition", "attachment; filename=Error.xls")
    'Response.ContentType = "application/ms-excel"
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><title></title>

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script src="jqq/ui/jquery.ui.datepicker.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>

<style type="text/css" enableviewstate="true">
#listatt
{
    font-size:9pt;
    border: 1px blue solid;
    text-align:left;
}

</style>
	<%  	   
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")

 %>
 <script type="text/javascript">
 
 $("#st").text(window.status);
 function checkall()
 {
    if($(".chkbox").is(':checked'))
    {
        $("#chkall").text("Checked All");
        
        $(".chkbox").attr("checked",false);
       
    }
    else
    {
        $(".chkbox").attr("checked",true);
       $("#chkall").text("clear All");
    }
  sumcolx();
 }
  
 //alert(document.referrer.toString())
function sumcolx()
{
  
    var bsal=0;
    var sumcol=[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
    var trs = document.getElementsByTagName("td");
    var arrid=[]; 
    var arrid2=[];
    var count=0;
    var con="";
    var rc=""
    var rowc=0;
    for(var i=0;i<trs.length;i++)
    {
        arrid=trs[i].id.split("_");
        
        if(arrid.length>1)
        {
            
        obj="#paid-" + arrid[0];
        if($(obj).is(':checked'))
        {
           arrid2=arrid[1].split("-");
           if(con!==obj)
           {
            count=0
                con=obj;
           
           } 
           //count++;
            if(arrid2.length>1)
            {
                 val=$("#" +trs[i].id).text();
                val=val.replace(",","");
                //alert(arrid2[0]);
                sumcol[count] +=parseFloat(val);
              //  alert(sumcol[arrid2[0]]);
              // alert("col: " + count);
            }
            else
            {
                val=$("#" +trs[i].id).text();
                val=val.replace(",","");
                // alert(arrid2[0]);
                sumcol[count] +=parseFloat(val);
              //  alert("col: " + count);
                
            }
            count++;
            
        }
        else
        {
         if(con!==obj)
           {
            count=0
                con=obj;
           
           } 
        sumcol[count]+=0
         count++;
        }
        }
        else
        {
            //alert($("#" +trs[i].id).text() + " ===" + sumcol[i]);
        }
    }
    var st=""
    var table = document.getElementById("tb1");
   var tr=document.getElementById("tb1").rows;
   
   var rowCount = table.rows.length;
   var td=tr[rowCount-2].cells;
  // table.deleteRow(rowCount-2);
   // var tr = table.insertRow();
    //tr.cells.style.text-align="right";
    
    var tdval=""
   //alert(rowCount);
    for(i=0;i<count;i++)
    {
     //var td = tr.insertCell();
     if(!isNaN(sumcol[i]))
     {
        tdval=numberToCurrency(sumcol[i].toFixed(2),"");
     }
     else
     {
        tdval=""
     }
    
     if(i==0 || i==1 || i==2 || i==4)
     tdval=""; 
     //alert(tdval);
         td[i].innerHTML=  tdval;
   // td.childNodes[i].style.text-align="right";
    //td.style.text-align="right";
    }
  //  alert(st);
   // document.getElementById('result').innerHTML=st;
  } 
   
  
 function findid()
 {
 var year="<% response.write(request.querystring("year")) %>";
 var month="<% response.write(request.querystring("month")) %>";
 var idArr = [];
 var val=[];
 var output="";

var trs = document.getElementsByTagName("td");

for(var i=0;i<trs.length;i++)
{
   var arrid=[]; 
    
   arrid=trs[i].id.split("_")
   if(arrid.length>1)
   {
        obj="#paid-" + arrid[0];
       
        idArr.push(trs[i].id);
   }
  
}
    for(var i=0;i<idArr.length;i++){
     val.push($("#" + idArr[i]).text());
         //alert(val[i]);
   if(val[i]!=="")
   output +=idArr[i]+"="+val[i]+"&";
   }
    
     //  alert("koooookuuuuluuuu" +idArr[i]+"===="+ $(obj).val());
        
       var str=$("#frmpay").fieldValue();
       //foreach(document.getElementById)
 // alert(output);
   // var loct="<%response.write(session("emp_id")) %>/leave/" +fn+"/";
   if(output!==""){
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;

  $("#pay").css({width:'900px',height:'500px'});
  $("#nextpage").val(output);
    //alert(whr + fn.toString());
    $('#frmpay').attr("target","fpay");
      $("#fpay").attr("frameborder","0");
      
       $('#frmpay').attr("action","payroll2.aspx?first=1&"+"month="+month+"&year="+year+"&projname=<%response.write(Request.Form("projname")) %>");
       $('#frmpay').submit();
    $('#pay').css({top:'0px',left:'0px'});
     showobj("pay");
       }
 //  $("#nextpage").val(output);
  // $("#frmpay").attr("action","?" );
// $("#frmpay").submit();
 
 }
function submittop()
{
document.frmlistout.action="payrollmidd.aspx";
document.frmlistout.submit();
}
  function findid2()
 {
 var year="<% response.write(request.form("year")) %>";
 var month="<% response.write(request.form("month")) %>";
 var idArr = [];
 var val=[];
 var output="";

var trs = document.getElementsByTagName("td");

for(var i=0;i<trs.length;i++)
{
   var arrid=[]; 
    
   arrid=trs[i].id.split("_")
   if(arrid.length>1)
   {
        obj="#paid-" + arrid[0];
       // alert($(obj).attr('disabled'));
        if($(obj).is(':checked')&& $(obj).attr('disabled')!="disabled")
    {
        idArr.push(trs[i].id);
        
    }
        
        
   }
  
}
    for(var i=0;i<idArr.length;i++){
     val.push($("#" + idArr[i]).text());
         //alert(val[i]);
   if(val[i]!=="")
   output +=idArr[i]+"="+val[i]+"&";
   }
     //  alert("koooookuuuuluuuu" +idArr[i]+"===="+ $(obj).val());
        
       var str=$("#frmpay").fieldValue();
       //foreach(document.getElementById)
 // alert(output);
   // var loct="<%response.write(session("emp_id")) %>/leave/" +fn+"/";
   if(output!==""){
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;
  $("#pay").css({width:'900px',height:'500px'});
  
   // alert(whr + fn.toString());
    $('#frmpay').attr("target","frm_tar");
       //$("#fpay").attr("frameborder","0");
      // alert(output);
      $('#nextpage').val(output);
       $('#frmpay').attr("action","payrollmidd.aspx?first=1&month="+month+"&year="+year+"&projname=<%response.write(Request.Form("projname")) %>");
       
       $('#frmpay').submit();
     //  $('#pay').css({top:'0px',left:'0px'});
      // showobj("pay");
       }
  // $("#nextpage").val(output);
  // $("#frmpay").attr("action","?" );
  //$("#frmpay").submit();
 
 }
 var namelist=[<% response.write(namelist) %>];
    $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  namelist, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});
 </script>
 <script type="text/javascript" language="javascript">
    function print(loc,title,head,footer)
    { //Response.AppendHeader("Content-Disposition", "attachment; filename='c:\temp\Error.xls'")
    //Response.ContentType = "application/ms-excel"
    
    var printFriendly = document.getElementById(loc)
    var printWin = window.open("print.htm",title,"menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    printWin.document.write("<meta http-equiv='content-type' content='application/ms-excel' /><meta name='content-disposition' content='attachment; filename=payrolmid.xls' /><title>" + head + "</title></head>" + printFriendly.innerHTML );
    printWin.document.close();
  // printWin.window.print();   
  // printWin.close();
    }
   
    </script>
</head>

<body style="height:auto;">
<div style="width:100%; height:45px; background:#6879aa; text-align:center;color:White; font-size:19pt;">Payroll Maker Increament in the middle of the Month</div>

   <%  Response.Write(fl.msgboxt("pay", "Confirmation", "<iframe name='fpay' id='fpay' width='900' height='500' frameborder='0' src='' scrolling='yes'></iframe>"))
%>
<div id="st"></div>
<%  If Request.QueryString.HasKeys = True Then
        For Each k As String In Request.QueryString
            '  Response.Write(k & "==" & Request.QueryString(k) & "<br>")
        Next
    End If
    If Request.Form.HasKeys = True Then
        For Each k As String In Request.Form
            ' Response.Write(k & "==" & Request.Form(k) & "<br>")
        Next
    End If
    
    %>
    <form name="frmlistout" action="" method="post">
        <select id="month" name="month">
            <%  For i As Integer = 1 To 12
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                Next
                
                %>
        </select>
        <script type="text/javascript">
            $("#month").val("<% response.write(today.month) %>");
        </script>
         <select id="year" name="year">
            <%  For i As Integer = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
         Enter Project<input type="text" name="projname" id="projname" />
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:submittop();" />
    </form>
     <form name="frmpay" id="frmpay" action="" method="post">
     <input type="hidden" name="nextpage" id="nextpage" />
    <div id="bigprint">
             <style type="text/css">
            #tb1 
            {
                border:1px solid black;
                font-size:12pt;
            }
          #tb1 td
            {
          border-top: 1px solid black;
             border-left:1px solid black;
             
                font-size:10pt;
            }
            .cell
            {
               border-top: 1px solid black;
             border-left:1px solid black;
             
             border-right: 1px solid black;
             border-bottom:1px solid black;
            }
            </style>
    
     
   <% 
       Dim cod As String
       cod = ""
       If Request.QueryString("first") = "" Then
           cod = makeform()
       Else
           Dim arrx() As String
           arrx = getids()
           
               
           ReDim Preserve arrx(UBound(arrx) + 1)
           arrx(UBound(arrx)) = "xxx"
           
           'Response.Write("innn")
           cod = makeform2(arrx)
       End If
       If String.IsNullOrEmpty(cod) = False Then
           cod = sec.StrToHex(cod)
       End If
       ' cod = sec.StrToHex(cod)
        %>
        </div>
        </form>
       <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');"><img src='images/ico/print.ico' alt="print"/>print</div>        
<form id="exportexcel" name="exprtexcel" action="print.aspx?pagename=payrolmid(<% response.write(request.querystring("month") & "-" & request.querystring("year")) %>" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="<% Response.Write(cod)%>" />
    <input type="submit" value="Export to Excel" />
</form>
<script type="text/jscript">

$("#frmpay").attr("target","chksess");
$("#frmpay").attr("action","checksession.aspx");
$("#frmpay").submit();

</script>
<%  'response.write(cod) %>
</body>
</html>


