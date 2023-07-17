<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptpardimview.aspx.vb" Inherits="rptpardimview" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%

    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim sec As New k_security
  
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
<script src="jq/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
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
	    namelist = fm.getjavalist2("tblproject", "project_name,project_id", session("con"), "|")

 %>
 <script type="text/javascript">
 //var year="<% 'response.write(request.form("year")) %>";
 //var month="<%' response.write((cint(request.form("month"))-1).tostring) %>";
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
       $("#chkall").text("Un-checked All");
    }
 
 }
 //alert(document.referrer.toString())
function view(frm)
{
    var spl=frm.split("-");
    $('#'+frm).attr("action","pardimview.aspx?part=p2&s=" + spl[1]+"&projname=<%  response.write(sec.dbStrToHex(Request.Form("projname"))) %>");
       
      $('#' +frm).submit();
   // alert(spl[1]);
}
function printp(x)
{

    $("#txtprint").val($('#' + x).val());
    $("#frmprint").attr("action","pardimview.aspx?part=p1&projname=<%  response.write(sec.dbStrToHex(Request.Form("projname"))) %>");
   $("#frmprint").submit();
   // alert( $("#txtprint").val())
}
 function findid(frm)
 {
    
 var idArr = [];
 var val=[];
 var output="";

var trs = document.getElementsByTagName("input");

for(var i=0;i<trs.length;i++)
{
   var arrid=[]; 
  
   
   arrid= trs[i].id.split("-")
   if(arrid.length>1)
   {
        
        obj="#" + trs[i].id;
       // alert($(obj).attr('disabled'));
        if($(obj).is(':checked')&& $(obj).attr('disabled')!="disabled")
    {
        idArr.push(trs[i].id);
        
    }
        
        
   }
  
}
    for(var i=0;i<idArr.length;i++){
     
    output +=i+"="+idArr[i]+"&";
   }
     //  alert("koooookuuuuluuuu" +idArr[i]+"===="+ $(obj).val());
          var str2=$("#" +frm).formSerialize();
          //alert(str2)
     //  var str=$("#" +frm).fieldValue();
       //foreach(document.getElementById)
 //alert(output+str2);
   // var loct="<%response.write(session("emp_id")) %>/leave/" +fn+"/";
   if(output!==""){
  var hw= window.screen.availHeight;
  var ww=window.screen.availWidth;
  //$("#pay").css({width:'900px',height:'500px'});
  
   // alert(whr + fn.toString());
  
   
    // $('#'+frm).attr("target","fpay");
     // $("#"+frm).attr("frameborder","0");
   // output="perdiemsheet.htm?" + str2;
     //    var printWinx = window.open("perdiemsheet.aspx","Per-diem","menubar=no,status=yes,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
    
if(confirm("Data is going to save, are you sure")){
   // $("#nextpage").val(output);
      $('#'+frm).attr("action","perdiemsheet.aspx?save=on&"+str2);
       
      $('#' +frm).submit();
       }
      // $('#pay').css({top:'0px',left:'0px'});
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

     function print(loc, title, head, footer) {

         var printFriendly = document.getElementById(loc)
         var printWin = window.open("print.htm", title, "menubar=no,status=no,toolbar=no,width=900px,scrollbars=yes,height=400px,top=20px,left=20px");
         printWin.document.write("<html><head><meta http-equiv='content-type' content='application/winword' /><meta name='content-disposition' content='attachment; filename=payrol.doc' /><title>" + head + "</title></head><body>" + printFriendly.innerHTML + "<label class='smalllbl'>" + footer + "</label></body></html>");
         printWin.document.close();
         printWin.window.print();
         // printWin.close();
     }
   
    </script>
</head>
<body>
<% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

<div style="width:100%; background:#6879aa; text-align:center;color:White; font-size:19pt;">Perdiem Payment View Form
    <form name="frmlistout" action="" method="post" style="font-size:13pt;">
       
         Type Project Name:<input type="text" name="projname" id="projname"/>
       
       &nbsp;&nbsp;&nbsp;Date From Month:
       <select id="month" name="month">
            <%  
                Response.Write("<option value='' selected='selected'>Select</option>")
                For i As Integer = 1 To 12
                   
                    Response.Write("<option value='" & i.ToString & "'>" & MonthName(i) & "</option>")
                    
                    
                Next%>
        </select>&nbsp;&nbsp;&nbsp;
        Year
         <select id="year" name="year">
            <%  Response.Write("<option value=''>Select</option>")
                For i As Integer = Today.Year To Today.Year - 9 Step -1
                    Response.Write("<option value='" & i.ToString & "'>" & i.ToString & "</option>")
                Next%>
        </select>
         <input type="submit" value="submit" />
    </form></div>
    <%  outpx()
     %>
</body>
</html>
