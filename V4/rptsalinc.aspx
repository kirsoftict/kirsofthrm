<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptsalinc.aspx.vb" Inherits="rptsalinc" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="System.Data.sqlclient" %>
<%@ Import Namespace="System.IO" %>
<%
    Session.Timeout = "60"
    Dim namelist As String
    Dim fm As New formMaker
    Dim fl As New file_list
    Dim sec As New k_security
    Dim projname As String
    Dim projid As String
    Dim spl() As String
    'Dim t1, t2 As Date
   
    ' Response.Write(String.IsNullOrEmpty(Session("pprroojj")))
    
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link href="images/kir.ico" rel="shortcut icon" />
    <title></title>
    <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />
    <link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
    <script src="jqq/jquery-1.9.1.js"></script>
    <script src="jqq/ui/jquery.ui.core.js"></script>
    <script src="jqq/ui/jquery.ui.widget.js"></script>
    <script src="jqq/ui/jquery.ui.position.js"></script>
    <script src="jqq/ui/jquery.ui.menu.js"></script>
    <script src="jqq/ui/jquery.ui.autocomplete.js"></script>
    <script src="jqq/ui/jquery.ui.progressbar.js"></script>
    <script src="jqq/ui/jquery.ui.datepicker.js"></script>
    <script src="jqq/ui/jquery.ui.button.js"></script>
    <script src="jqq/ui/jquery.ui.dialog.js"></script>
    <script src="jqq/ui/jquery.ui.tabs.js"></script>
    <link rel="stylesheet" href="jq/demos.css">
    <script type="text/javascript" src="scripts/form.js"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" />
    <script src="scripts/kirsoft.java.js" type="text/javascript"></script>
    <script src="scripts/script.js" type="text/javascript"></script>
    <link href="css/payrol.css" media="screen" rel="stylesheet" type="text/css" />
    <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
    <script src="scripts/kirsoft.payrol.js" type="text/javascript"></script>
    <% 
        Dim keyp As String = ""
        If Request.QueryString("dox") = "edit" Then
            keyp = "update"
        ElseIf Request.QueryString("dox") = "delete" Then
            keyp = "delete"
        Else
            keyp = "save"
        End If
 
        Dim emp_id, emptid As String
    
        Dim idx As String = ""
        idx = Request.QueryString("id")
        Dim msg As String = ""
        Dim flg As Integer = 0
        Dim flg2 As Integer = 0
      
        Dim rd As String = ""

        Dim tbl As String = ""
        Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        ' namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", session("con"), " ")


    %>
    <style type="text/css" enableviewstate="true">
        #listatt
        {
            font-size: 9pt;
            border: 1px blue solid;
            text-align: left;
        }
        .font5
	{color:black;
	font-size:10.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:Calibri, sans-serif;
	}
td
	{border-style: none;
            border-color: inherit;
            border-width: medium;
            padding-top:1px;
	        padding-right:1px;
	        padding-left:1px;
	        color:black;
	        font-size:11.0pt;
	        font-weight:400;
	        font-style:normal;
	        text-decoration:none;
	        font-family:Calibri, sans-serif;
	        text-align:general;
	        vertical-align:bottom;
	        white-space:nowrap;
	}
        </style>
    <title></title>
    <%  	   
        namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
	

    %>
    <script type="text/javascript">
// var stime=Date.now();
<% 
if Request.QueryString("month")<>"" then
response.write("var month=" & chr(34) & request.querystring("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.querystring("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.querystring("projname")) & chr(34) & ";")

else
response.write("var month=" & chr(34) & request.form("month") & chr(34) & ";")
response.write("var year=" & chr(34) & request.form("year") & chr(34) & ";")
response.write("var projname=" & chr(34) & sec.dbStrToHex(request.form("projname")) & chr(34) & ";")

end if 

%>

  
 var paymth="<% response.write(request.querystring("paymth")) %>";
 //alert(document.referrer.toString())
 var ppname="<% response.write(sec.dbStrToHex(request.querystring("projname"))) %>";
 
   
  
 var nameproj=[<% response.write(namelist) %>];
 
    $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  nameproj, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});

 
 
 

 var prv;
  prv="";
var id;
var focused="";
var requf=["vname","bankname","accountno","x"];
var fieldlist=["id","emptid","bankname","accountno","who_reg","date_reg","x"];
function validation1(){
if ($('#vname').val() == '') {showMessage('vname cannot be empty','vname');$('#vname').focus();return false;}
if ($('#bankname').val() == '') {showMessage('Bank name cannot be empty','bankname');$('#bankname').focus();return false;}
if ($('#accountno').val() == '') {showMessage('Account no. cannot be empty','accountno');$('#accountno').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{//alert("intovalid");

   var str=$("#frmempbank").formSerialize();
 //  alert(str)
   $("#frmempbank").attr("action","?tbl=empbank&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmempbank").submit();
  return true;}
  }
} 
    var namelist=[<% response.write(namelist) %>];
    
 
   function edit(val,date1,date2,fulname)
   {
         $("#pay").css({ width: '900px', height: '500px' });
          $("#fpay").css({ width: '900px', height: '500px' });
         //alert(whr + fn.toString());
         $('#frmx').attr("target", "fpay");
         $("#fpay").attr("frameborder", "0");

         $('#frmx').attr("action", "grid.aspx?task=edit&sql=" + val + "&tbl=empbank&page=bankaccent.aspx&fname="+ fulname);
       // alert("grid.aspx?task=edit&sql" + val + "&tbl=emp_ot");
         $('#frmx').submit();
       
         $('#pay').css({ top: '0px', left: '0px' });
         $("#pay").remove("display");
         $("#pay").dialog({
             height: 300,
             width: 800,
             modal: true,
             buttons: {

'Refresh and Close': function() {
$('#frmx').attr("target", "frm_tar");
        // $("#fpay").attr("frameborder", "0");
      
         $('#frmx').attr("action", "bankaccent.aspx?chkupdate=on");
      
         $('#frmx').submit();
       
$(this).dialog('close');
// Update Rating
}
}
         }); 
   
   }
  
	$(function() {
		$( "#tabs" ).tabs();
	});
	$(document).ready(function(){
    //otmgr();
    
    });
    </script>
    <% 'response.write(namelist) %>
</head>
<body>
    <% ' 'Response.Write(fm.helpback(False, True, False, False, "", ""))%>
 
    <%  
        Response.Write("<div id='progressbar' style='visibility:hidden;top:200px; left:200px;position:absolute;'><div class='progress-label'>Loading...</div></div>")

             
        Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' frameborder='0' src='' scrolling='no'></iframe>"))
    %>
    <form id='frmx' name='frmx' method="post">
    </form>
    <div id="st">
    </div>
    <div style="width: 100%; height: 50px; background: #6879aa; text-align: center; color: White;
        font-size: 13pt; padding-top: 10px;">
        <form name="frmlistout" id='frmlistout' action="rptsalinc.aspx" method="post" style="font-size: 12pt;">
       Enter Project:
        <input type="text" name="projname" id="projname" />
        <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:$('#frmlistout').submit();" />
        </form>
    </div>
    <% 
       
        Dim obj2 As Object
        Dim obj As Object = ""
        obj2 = makeinc()
        If obj2 <> "" Then
            obj = obj2
        End If
        
        Dim loc As String = Server.MapPath("download") & "\incsal.txt"
        obj2 = "1;2;3;4;" & Chr(13) & obj2
        File.WriteAllText(loc, obj2)
    
    
        If obj.ToString <> "" Then%>
        <div id='expxls' style="float:left;">
<form id="exportexcel" name="exprtexcel" action="print.aspx?pagename=viewrpt-<% response.write(request.querystring("val")) %>" method="post" target="_blank">
    <input type="hidden" name="nnp" id="nnp" value="from file" />
    <input type="hidden" name="loc" id="loc" value="<% response.write(loc) %>" />
    <input type="hidden" name="right" id="right" value="<% response.write(session("right")) %>" />
    <div style=" border:none; width:150px;height:28px; background:url(images/blue_banner-760x147x.jpg) #224488;color:White; cursor:pointer;" onclick="javascript:$('#exportexcel').submit();" />
    <img src="images/png/excel.png" height="28px" style="float:left;" alt="excel" > Export to Excel</div>
</form></div> 
<div id="Span1"  style="width:100px; height:28px;background:url(images/blue_banner-760x147x.jpg) #224488;color:White;cursor:pointer;float:left;" onclick="javascirpt:printincsal('bigprint','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>','printA4');">
      <img src='images/png/printer2.png' alt="print" height="28px" style="float:left;"/>print A4 </div>     
  <%   Response.Write(obj.ToString)
      End If
     
      %>

  </body>
</html>