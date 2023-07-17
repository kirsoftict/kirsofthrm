<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empinc.aspx.vb" Inherits="empinc" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% Dim sec As New k_security
    
    Dim pdate1, pdate2 As Date
    pdate1 = "#1/1/1900#"
    pdate2 = "#1/1/1900#"

    Dim projname As String
    If String.IsNullOrEmpty(Session("pprroojj")) Then
        
        Session("pprroojj") = ""
        If Request.Form("projname") <> "" Then
            If Request.Form("projname") <> Session("pprroojj") And Request.Form("month") & "/1/" & Request.Form("year") <> Session("whn") Then
                Session("pprroojj") = Request.Form("projname")
                Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                pdate1 = Session("whn")
                pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
           
            End If
        End If
        
    Else
        If Request.QueryString("sub") = "" Then
            
            If Request.Form("projname") <> Session("pprroojj") And Request.QueryString("task") = "" Then
                If Request.Form("projname") <> "" Then
                    Session("pprroojj") = Request.Form("projname")
                    Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate1 = Session("whn")
                    pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
                End If
            ElseIf Request.Form("month") <> "" Then
                If Request.Form("month") & "/1/" & Request.Form("year") <> Session("whn") Then
                    Session("whn") = Request.Form("month") & "/1/" & Request.Form("year")
                    pdate1 = Session("whn")
                    pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
                End If
            Else
                
                pdate1 = Session("whn")
                pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
    
            End If
        Else
            ' Response.Write(Request.QueryString("sub"))
            Session("pprroojj") = ""
            Session("whn") = "#1/1/0001#"
        End If
    End If
    If IsError(Session("whn")) = False And Session("whn") <> "" Then
        ' Session("pprroojj") = ""
        ' Session("whn") = "#1/1/0001#"
        pdate1 = Session("whn")
        pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
    
        
    End If
    %>
   
    
   
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>


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
	<script src="jqq/ui/jquery.ui.button.js"></script>
	<script src="jqq/ui/jquery.ui.dialog.js"></script>
		<script type="text/javascript" src="scripts/form.js"></script>
         <script src="jqq/ui/jquery.ui.tabs.js"></script>

<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
  <%      Dim namelist As String
      namelist = fm.getjavalist2("tblproject", "project_name,project_id", Session("con"), "|")
      

 %><script>
 var nameproj=[<% response.write(namelist) %>]; 
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
 Dim rson As String
      rson = fm.getjavalist2("emp_inc", "distinct reason", session("con"), "")
      namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", session("con"), " ")

%>

  
  var ppname="<% response.write(sec.dbStrToHex(request.querystring("projname"))) %>";
  var id;
var focused="";
var requf=["inc_date","reason","amt","x"];
var fieldlist=["emptid","emp_id","inc_date","reason","amt","apporved_by","approved_date","who_reg","date_reg","active","x"];
function validation1(){
if ($('#inc_date').val() == '') {showMessage('inc_date cannot be empty','inc_date');$('#inc_date').focus();return false;}
if ($('#reason').val() == '') {showMessage('reason cannot be empty','reason');$('#reason').focus();return false;}
if ($('#amt').val() == '') {showMessage('Amount cannot be empty','amt');$('#amt').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_inc").formSerialize();
   $("#frmemp_inc").attr("action","?tbl=emp_inc&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_inc").submit();
  return true;}
  }
} 
 </script>
  <script type="text/javascript">
  var likeid;
  function showHideSubMenu(link,id) {

        var uldisplay="";
        var newClass="";
        
        
        if (link.className == 'expanded') {

            // Need to hide
            
            uldisplay = 'none';
            newClass = 'collapsed';

        } else {
            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
        }

       
        $("#"+id).css({'display':  uldisplay});
        link.className = newClass;
    }
    function showHideSubMenux(linkid,id) {

        var uldisplay="";
        var newClass="";
        
        link=document.getElementById(linkid)
        if (link.className == 'expanded') {

            // Need to hide
            
            uldisplay = 'none';
            newClass = 'collapsed';

        } else {
            // Need to show
            uldisplay = 'block';
            newClass = 'expanded';
        }

       
        $("#"+id).css({'display':  uldisplay});
        link.className = newClass;
    }
var prv;
  prv="";
 var namelist=[<% response.write(namelist) %>];
  var rson=[<% response.write(rson) %>];
   $(document).ready(function() {
     $( "#projname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep( nameproj, function(item){ 
        return matcher.test(item); }) ); 
        }
		});

         
     $( "#vname" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  namelist, function(item){ 
        return matcher.test(item); }) ); 
        }
		});

         $( "#reason" ).autocomplete({
  source: function(req, response) { 
    var re = $.ui.autocomplete.escapeRegex(req.term); 
    var matcher = new RegExp( "^" + re, "i" ); 
    response($.grep(  rson, function(item){ 
        return matcher.test(item); }) ); 
        }
		});
		});
   
    
		
   
    
       
	
         $(function() {
		$( "#tabs" ).tabs();
	});

 </script>  
 <style>
  #tabs
  {
  	font-size:12px;}
 </style>
</head>

<body style="height:auto;">
<form id='frmx' name='frmx' method="post"></form>
  
<div id="st"></div>
<div style="width:100%; height:40px; background:#6879aa; text-align:center;color:White; font-size:13pt; padding-top:10px;">

    <form name="frmlistout" id='frmlistout' action="empinc.aspx" method="post" style="font-size:12pt;">
 Increment Registration<br />     Month: <select id="month" name="month">
            <%
                For i As Integer = 1 To 12
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
         Enter Project: <input type="text" name="projname" id="projname"  />
       
         <input type="button" value="Go" id="firstform" name="firstform" onclick="javascript:$('#frmlistout').submit();" />
    </form>
    </div>
    <%
    
    
    If Session("pprroojj") <> "" And Session("whn") <> "" Then%>


 <%   pdate1 = Session("whn")
     pdate2 = pdate1.Month.ToString & "/" & Date.DaysInMonth(pdate1.Year, pdate1.Month).ToString & "/" & pdate1.Year.ToString
     ' Response.Write("<br>" & getprojemp(projid, pdate2, Session("con")))
    %>
       <% If Request.QueryString("chkupdate") = "on" Then
          ' updatedb()
               '  updatedb(Session("pprroojj"), pdate1, pdate2)
       End If%>
    <div id="tabs">
  <ul>
    <li><a href="#tabs-1">Add Data and Edit</a></li>
    <li><a href="#tabs-2">Increament in the month already paid</a></li>
    
  </ul>
  <div id="tabs-1"> 
<div id="formouterbox" style="width:650px; float:left">
    <div id="reg"> <%  
     
     
'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))

'Response.Write("<br />" & Request.Form("do"))
      If session("pprroojj") <> "" Then%>


 <div id="formouterbox_small" style="width:650px;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div>
        <div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messageboxx"></span>


<form method='post' id='frmemp_inc' name='frmemp_inc'> 
<table><tr><td>Name</td><td>:</td><td colspan='3'>
<input type='text' name='vname' id='vname' style='font-size:9pt;width:300px;' />

<input type='hidden' id='emptid' name='emptid' value='' />
<input type='hidden' id='emp_id' name='emp_id' value='' /><br /><label class='lblsmall'></label></td>
</td></tr><tr><td>Reason<sup style='color:red;'>*</sup></td><td>:</td><td><input type='text' id='reason' name='reason' value='' /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Amount Taxable<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='amt' name='amt' value='' style="text-align:right;" /><br /><label class='lblsmall'></label></td>
<td>Amount Non-Taxable<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='amt2' name='amt2' value='0' style="text-align:right;" /><br /><label class='lblsmall'></label></td>
</tr><tr><td>Increment Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='inc_date' name='inc_date' value='' /><br /><label class='lblsmall'></label></td>
<script language='javascript' type='text/javascript'>    $(function () { $("#inc_date").datepicker({ changeMonth: true, changeYear: true }); $("#inc_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>
<td>Date paid<sup style='color:red;'></sup></td><td>:</td><td>
<input type='text' id='paid_date' name='paid_date' value='' /><br /><label class='lblsmall'></label>
<script language='javascript' type='text/javascript'>    $(function () { $("#paid_date").datepicker({ changeMonth: true, changeYear: true }); $("#paid_date").datepicker("option", "dateFormat", "mm/dd/yy"); });</script>

<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
</td></tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr>
</table></form>

</div>
    <sup style="color:Red;">*</sup>Required Fields
 </div><% End If%>
  </div>
 </div> 
 <div style="float:left; width:10px;"></div>
 <div style="float:left; width:600px;font-size:10px;">
   <% 
        Dim db As New dbclass
       Dim dt As DataTableReader
       Dim locx As String = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)

        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_inc where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
               dt.Read()
               Dim vname As String = ""
              
               vname = fm.getfullname(dt.Item("emp_id"), Session("con"))
               
               Response.Write("<script type='text/javascript'>")
               If vname <> "" Then
                   Response.Write("$('#vname').val('" & vname & "');")
               End If
               For k As Integer = 0 To dt.FieldCount - 3
                   'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                   If LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                        %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k).trim & "');")%>
                    <% 
                    ElseIf LCase(dt.GetDataTypeName(k)) = "datetime" And dt.IsDBNull(k) = False Then
                        Dim sdatex As Date = dt.Item(k)
                        Dim d As String = sdatex.ToShortDateString
                        Dim da As String = sdatex.Day
                        Dim mm As String = sdatex.Month
                        Dim yy As String = sdatex.Year
                        d = mm & "/" & da & "/" & yy
                        Response.Write("$('#" & dt.GetName(k) & "').val('" & d & "');")
                    Else
                         %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k) & "');")%>
                    <%
                    End If
                   
                Next
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
                End If
                dt.Close()
            End If
            db = Nothing
            dt = Nothing
       Dim mk As New formMaker
            Dim row As String
        
       If Session("pprroojj") <> "" Then
           Dim projid() As String
           projid = Session("pprroojj").split("|")
            Dim idsin As String = getprojemp(projid(1).ToString, pdate2.AddMonths(-1), Session("con"))
            'Response.Write(Session("pprroojj") & "  ")
            ' Response.Write(pdate2.ToShortDateString & "<br>")
            ' Response.Write(Request.QueryString("id"))
            '   Response.Write("select id,inc_date,reason,amt,amt2 from emp_inc where id in(" & idsin & ") and inc_date between '" & pdate1 & "' and '" & pdate2 & "'  order by id desc")
            Dim sqlx As String = "select id,inc_date,reason,amt,amt2,emptid from emp_inc where emptid in(" & idsin & ") and inc_date between '" & pdate1 & "' and '" & pdate2 & "' and (paidref is Null or paidref='' ) order by id desc"
            row = edit_del_list_wname("emp_inc", sqlx, "Name,Date Increment,Reason,Amount,Non-taxable Amount", Session("con"), locx)
           Response.Write(row)
       End If%> 
 
 </div>
 </div>
 <%  ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:delx('" & idx & "','yes','del123');" & Chr(34) & "></span>"
           fs.msgboxt("del123", "Caution! Deleting", con)
           str = "<div id='del123' style=" & Chr(34) & "opacity:0.9;filter:alpha(opacity=90); background:#9fdfaf; left:400px; top:200px; width:600px; height:400px; text-align:center; vertical-align:middle; position:absolute; content:open-quote;" & Chr(34) & _
            "><div style=" & Chr(34) & "height:30px; background:url(images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            "><div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:120px; position:absolute; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>Warrening</b></div><div style=" & Chr(34) & "cursor:pointer; text-align:right; height:30px; width:22px; color:#CC0000; background:url(../images/xp.gif); background-repeat:no-repeat; right:0px; position:absolute" & Chr(34) & " dir=" & Chr(34) & "rtl" & Chr(34) & " onClick=" & Chr(34) & "javascript: document.getElementById('" & CStr(ID) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp; </div></div><br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(con) & "</div></div>"
           ' Response.Write(str)
           %> 
           <div id="dialog-modal" title="Caution"><% response.write(con) %></div>
           <script type="text/javascript">

               //$( "#dialog:ui-dialog" ).dialog( "destroy" );

               $("#dialog-modal").dialog({
                   resizable: true,
                   modal: true
               });
		
           </script>
           <%  fs = Nothing
              
           End If
         %>
   
   <script type="text/javascript">
       function delx(val, ans, hd) {

           if (ans == "yes") {
               // alert(val + ans);
               $('#frmx').attr("target", "_self");
               $('#frmx').attr("action", "<% response.write(locx) %>?task=delete&id=" + val + "&tbl=emp_inc");
               $('#frmx').submit();
           }
           else {
               ha(hd);
           }
       }
   </script>
       
           
 <% 'viewforedit(Session("pprroojj"),pdate1,pdate2)
   '  Response.Write(locx)
     
     %>
    
  <div id="tabs-2">
<%
    'viewpaid(Session("pprroojj"), pdate1, pdate2)%>
     
    <div>
   <%    If Session("pprroojj") <> "" Then
           Dim projid() As String
           projid = Session("pprroojj").split("|")
           Dim idsin As String = getprojemp(projid(1).ToString, pdate2, Session("con"))
           Response.Write(Session("pprroojj") & "  ")
           Response.Write(pdate2.ToShortDateString & "<br>")
           ' Response.Write(Request.QueryString("id"))
           locx = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
           '   Response.Write("select id,inc_date,reason,amt,amt2 from emp_inc where id in(" & idsin & ") and inc_date between '" & pdate1 & "' and '" & pdate2 & "'  order by id desc")
           Dim sqlx As String = "select id,inc_date,reason,amt,amt2,paid_date,emptid from emp_inc where emptid in(" & idsin & ") and inc_date between '" & pdate1 & "' and '" & pdate2 & "' and paidref is not null order by id desc"
           row = Me.edit_del_list2_wname("emp_inc", sqlx, "Name,Date Increment,Reason,Amount,Non-taxable Amount,Date Paid", Session("con"), locx, "", False, False, False, False)
           Response.Write(row)
       End If%> 
     
    </div>  </div>
    </div>

    <% ' Registration form %>

<% Else
    Response.Write("<b>Please select Project and date</b>")
    End If%>


<% ' end registration %>

<div style='float:left;width:1%;'>&nbsp;</div>

 
    
 
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Increament Request!");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
  
   
    
    <%If msg <> "" Then
           %><script>
           $('#messageboxx').text("<%=msg%>");
           </script>
           <%
        End If
           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","_parent");
       
        //$('#frmx').attr("action","<% response.write(rd) %>");
       // $('#frmx').submit();
    </script>
   <%  End If
       
       
       %>
   
   
</body>
</html>
<script src="scripts/kirsoft.required.js" type="text/javascript"></script>
